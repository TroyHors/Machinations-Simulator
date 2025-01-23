using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;
using DemoProject.Core;
using DemoProject.Nodes;
using TMPro;

public class NodeInspector : MonoBehaviour {
    [Header( "Main Panel Root" )]
    public GameObject panelRoot;      // 整个Inspector面板的根对象 (显示/隐藏)

    // ==== 通用字段 ====
    [Header( "Common Fields" )]
    public TMP_InputField nodeNameInput;          // 编辑 nodeName
    public RectTransform inventoryContainer;  // 容器：动态生成 Resource 行
    public GameObject resourceLinePrefab;     // 预制体：显示"资源名称 + 数量输入框"
    public TextMeshProUGUI inputConnectionsText;         // 展示输入连接
    public TextMeshProUGUI outputConnectionsText;        // 展示输出连接

    // ==== Producer ====
    [Header( "Producer Fields" )]
    public GameObject producerGroup;
    public TMP_Dropdown producerTypeDropdown;
    public TMP_InputField producerRateInput;

    // ==== Consumer ====
    [Header( "Consumer Fields" )]
    public GameObject consumerGroup;
    public TMP_Dropdown consumerTypeDropdown;
    public TMP_InputField consumerRateInput;

    // ==== Converter ====
    [Header( "Converter Fields" )]
    public GameObject converterGroup;
    public TMP_Dropdown inputTypeDropdown;
    public TMP_Dropdown outputTypeDropdown;
    public TMP_InputField ratioInInput;
    public TMP_InputField ratioOutInput;
    public TMP_InputField convertRateInput;

    // ==== ResourcePool ====
    [Header( "ResourcePool Fields" )]
    public GameObject poolGroup;
    // 如果 ResourcePool 有 pullAmountPerTick 等，也在此添加对应 InputField

    private NodeBase currentNode;
    private List<GameObject> inventoryLines = new List<GameObject>(); // 动态生成的资源行

    void Start() {
        panelRoot.SetActive( false );
    }

    /// <summary>
    /// 显示指定节点的所有参数
    /// </summary>
    public void ShowNode( NodeBase node ) {
        if (node == null) {
            HidePanel();
            return;
        }
        currentNode = node;

        // 打开面板
        panelRoot.SetActive( true );

        // 1) 清理旧的资源行UI
        ClearInventoryLines();

        // 2) 显示通用字段
        nodeNameInput.text = node.nodeName;
        PopulateInventory( node );
        PopulateConnections( node );

        // 3) 隐藏所有子类组
        producerGroup.SetActive( false );
        consumerGroup.SetActive( false );
        converterGroup.SetActive( false );
        poolGroup.SetActive( false );

        // 4) 针对具体类型显示对应字段
        if (node is Producer prod) {
            producerGroup.SetActive( true );
            producerTypeDropdown.value = (int) prod.produceType;
            producerRateInput.text = prod.produceRate.ToString();
        } else if (node is Consumer cons) {
            consumerGroup.SetActive( true );
            consumerTypeDropdown.value = (int) cons.consumeType;
            consumerRateInput.text = cons.consumeRate.ToString();
        } else if (node is Converter conv) {
            converterGroup.SetActive( true );
            inputTypeDropdown.value = (int) conv.inputType;
            outputTypeDropdown.value = (int) conv.outputType;
            ratioInInput.text = conv.ratioIn.ToString();
            ratioOutInput.text = conv.ratioOut.ToString();
            convertRateInput.text = conv.convertRate.ToString();
        } else if (node is ResourcePool pool) {
            poolGroup.SetActive( true );
            // 如有 pool.pullAmountPerTick 等字段，在这里赋值
        }
    }

    public void HidePanel() {
        panelRoot.SetActive( false );
        currentNode = null;
        ClearInventoryLines();
    }

    // ======== 通用字段的保存逻辑 ========
    public void OnNodeNameChanged( string newName ) {
        if (currentNode != null) {
            currentNode.nodeName = newName;
        }
    }

    // ======== Producer 保存 ========
    public void OnProducerTypeChanged( int index ) {
        if (currentNode is Producer prod) {
            prod.produceType = (ResourceType) index;
        }
    }
    public void OnProducerRateChanged( string val ) {
        if (currentNode is Producer prod) {
            if (float.TryParse( val , out float f )) {
                prod.produceRate = f;
            }
        }
    }

    // ======== Consumer 保存 ========
    public void OnConsumerTypeChanged( int index ) {
        if (currentNode is Consumer cons) {
            cons.consumeType = (ResourceType) index;
        }
    }
    public void OnConsumerRateChanged( string val ) {
        if (currentNode is Consumer cons) {
            if (float.TryParse( val , out float f )) {
                cons.consumeRate = f;
            }
        }
    }

    // ======== Converter 保存 ========
    public void OnConverterInputChanged( int index ) {
        if (currentNode is Converter conv) {
            conv.inputType = (ResourceType) index;
        }
    }
    public void OnConverterOutputChanged( int index ) {
        if (currentNode is Converter conv) {
            conv.outputType = (ResourceType) index;
        }
    }
    public void OnRatioInChanged( string val ) {
        if (currentNode is Converter conv) {
            if (float.TryParse( val , out float f )) {
                conv.ratioIn = f;
            }
        }
    }
    public void OnRatioOutChanged( string val ) {
        if (currentNode is Converter conv) {
            if (float.TryParse( val , out float f )) {
                conv.ratioOut = f;
            }
        }
    }
    public void OnConvertRateChanged( string val ) {
        if (currentNode is Converter conv) {
            if (float.TryParse( val , out float f )) {
                conv.convertRate = f;
            }
        }
    }

    // ======== ResourcePool 保存 ========
    // 如果有 pool.pullAmountPerTick 等字段, 在此添加类似 OnPoolPullAmountChanged(...) {}

    // ======== 库存(Inventory)显示与修改 ========
    private void PopulateInventory( NodeBase node ) {
        if (node.inventory == null) return;

        // 根据 ResourceType 枚举遍历
        foreach (ResourceType rt in System.Enum.GetValues( typeof( ResourceType ) )) {
            float amount = node.inventory.GetResourceAmount( rt );

            // 动态生成一行
            GameObject lineObj = Instantiate( resourceLinePrefab , inventoryContainer );
            inventoryLines.Add( lineObj );

            // lineObj 内应包含两个UI元素: Text(资源名), InputField(资源量)
            Text nameText = lineObj.transform.Find( "ResourceName" ).GetComponent<Text>();
            InputField amountField = lineObj.transform.Find( "ResourceAmount" ).GetComponent<InputField>();

            nameText.text = rt.ToString();
            amountField.text = amount.ToString();

            // 监听更改
            amountField.onEndEdit.AddListener( ( string newVal ) => {
                if (float.TryParse( newVal , out float val )) {
                    node.inventory.resourceDict[ rt ] = val;
                }
            } );
        }
    }

    private void ClearInventoryLines() {
        for (int i = 0 ; i < inventoryLines.Count ; i++) {
            Destroy( inventoryLines[ i ] );
        }
        inventoryLines.Clear();
    }

    // ======== 连接信息(只读显示) ========
    private void PopulateConnections( NodeBase node ) {
        // input
        StringBuilder sbIn = new StringBuilder();
        foreach (var c in node.inputConnections) {
            if (c.sourceNode != null)
                sbIn.AppendLine( c.sourceNode.nodeName );
        }
        inputConnectionsText.text = sbIn.ToString();

        // output
        StringBuilder sbOut = new StringBuilder();
        foreach (var c in node.outputConnections) {
            if (c.targetNode != null)
                sbOut.AppendLine( c.targetNode.nodeName );
        }
        outputConnectionsText.text = sbOut.ToString();
    }
}
