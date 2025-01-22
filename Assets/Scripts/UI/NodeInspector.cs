using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DemoProject.Nodes;
using DemoProject.Core;
using TMPro;

public class NodeInspector : MonoBehaviour {
    [Header( "Panel Root" )]
    public GameObject panelRoot;         // 整个Inspector面板的根对象

    [Header( "Generic Fields" )]
    public TextMeshProUGUI nodeNameText;            // 显示 NodeBase.nodeName

    // Producer Fields
    [Header( "Producer Fields" )]
    public GameObject producerGroup;     // 父容器用来显示/隐藏Producer相关UI
    public TMP_Dropdown producerTypeDropdown;
    public TMP_InputField producerRateInput;

    // Consumer Fields
    [Header( "Consumer Fields" )]
    public GameObject consumerGroup;
    public TMP_Dropdown consumerTypeDropdown;
    public TMP_InputField consumerRateInput;

    // Converter Fields
    [Header( "Converter Fields" )]
    public GameObject converterGroup;
    public TMP_Dropdown inputTypeDropdown;
    public TMP_Dropdown outputTypeDropdown;
    public TMP_InputField ratioInInput;
    public TMP_InputField ratioOutInput;
    public TMP_InputField convertRateInput;

    // ResourcePool Fields (如有特殊字段，也可添加)
    [Header( "ResourcePool Fields" )]
    public GameObject poolGroup;
    // 如果 Pool 有特别字段，如 pullAmountPerTick，可以在这里加
    // public InputField pullAmountInput;

    private NodeBase currentNode;

    void Start() {
        panelRoot.SetActive( false );
    }

    public void ShowNode( NodeBase node ) {
        if (node == null) {
            HidePanel();
            return;
        }

        currentNode = node;
        panelRoot.SetActive( true );

        // 通用
        nodeNameText.text = node.nodeName;

        // 隐藏所有组
        producerGroup.SetActive( false );
        consumerGroup.SetActive( false );
        converterGroup.SetActive( false );
        poolGroup.SetActive( false );

        // 检查具体节点类型
        if (node is Producer) {
            Producer prod = (Producer) node;
            producerGroup.SetActive( true );

            // 设置下拉选项
            producerTypeDropdown.value = (int) prod.produceType;
            producerRateInput.text = prod.produceRate.ToString();
        } else if (node is Consumer) {
            Consumer cons = (Consumer) node;
            consumerGroup.SetActive( true );

            consumerTypeDropdown.value = (int) cons.consumeType;
            consumerRateInput.text = cons.consumeRate.ToString();
        } else if (node is Converter) {
            Converter conv = (Converter) node;
            converterGroup.SetActive( true );

            inputTypeDropdown.value = (int) conv.inputType;
            outputTypeDropdown.value = (int) conv.outputType;
            ratioInInput.text = conv.ratioIn.ToString();
            ratioOutInput.text = conv.ratioOut.ToString();
            convertRateInput.text = conv.convertRate.ToString();
        } else if (node is ResourcePool) {
            ResourcePool pool = (ResourcePool) node;
            poolGroup.SetActive( true );
            // 如果有独特字段，如 pullAmountPerTick，则在此设定
            // pullAmountInput.text = pool.pullAmountPerTick.ToString();
        }
    }

    public void HidePanel() {
        panelRoot.SetActive( false );
        currentNode = null;
    }

    // —— 以下是对应UI交互的保存逻辑 ——

    // Producer
    public void OnProducerTypeChanged( int index ) {
        if (currentNode is Producer prod) {
            prod.produceType = (ResourceType) index;
        }
    }
    public void OnProducerRateChanged() {
        if (currentNode is Producer prod) {
            if (float.TryParse( producerRateInput.text , out float val )) {
                prod.produceRate = val;
            }
        }
    }

    // Consumer
    public void OnConsumerTypeChanged( int index ) {
        if (currentNode is Consumer cons) {
            cons.consumeType = (ResourceType) index;
        }
    }
    public void OnConsumerRateChanged() {
        if (currentNode is Consumer cons) {
            if (float.TryParse( consumerRateInput.text , out float val )) {
                cons.consumeRate = val;
            }
        }
    }

    // Converter
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
    public void OnRatioInChanged() {
        if (currentNode is Converter conv) {
            if (float.TryParse( ratioInInput.text , out float val )) {
                conv.ratioIn = val;
            }
        }
    }
    public void OnRatioOutChanged() {
        if (currentNode is Converter conv) {
            if (float.TryParse( ratioOutInput.text , out float val )) {
                conv.ratioOut = val;
            }
        }
    }
    public void OnConvertRateChanged() {
        if (currentNode is Converter conv) {
            if (float.TryParse( convertRateInput.text , out float val )) {
                conv.convertRate = val;
            }
        }
    }

    // ResourcePool
    // public void OnPoolPullAmountChanged() { ... } // 如果有类似字段
}
