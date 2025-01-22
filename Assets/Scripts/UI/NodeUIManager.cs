using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DemoProject.Core;

public class NodeUIManager : MonoBehaviour {
    [Header( "Node Prefabs (All Pages)" )]
    public List<GameObject> nodePrefabs = new List<GameObject>();

    [Header( "Pagination Setup" )]
    public Transform slotsParent;  // 该对象下有 8 个子Slot
    public int slotsPerPage = 8;

    [Header( "Reference to Placement System" )]
    public PlacementSystem placementSystem;

    public GameObject uiRoot;  // 指向整个下方 UI 的最外层容器
    public bool isOpen = true;

    private int currentPage = 0;

    /// <summary>
    /// 在 Start() 时显示第一页
    /// </summary>
    void Start() {
        ShowPage( 0 );
    }

    /// <summary>
    /// 点击“上一页”按钮时调用
    /// </summary>
    public void PrevPage() {
        if (currentPage > 0) {
            currentPage--;
            ShowPage( currentPage );
        }
    }

    /// <summary>
    /// 点击“下一页”按钮时调用
    /// </summary>
    public void NextPage() {
        // 最后一页的起始索引: (pageCount - 1)
        int maxPageIndex = Mathf.CeilToInt( (float) nodePrefabs.Count / slotsPerPage ) - 1;
        if (currentPage < maxPageIndex) {
            currentPage++;
            ShowPage( currentPage );
        }
    }

    /// <summary>
    /// 刷新当前页显示
    /// </summary>
    private void ShowPage( int pageIndex ) {
        // 计算本页的起始和结束索引
        int startIndex = pageIndex * slotsPerPage;
        int endIndex = Mathf.Min( startIndex + slotsPerPage , nodePrefabs.Count );

        // 遍历 8 个槽位，将对应的预制体信息填入
        for (int i = 0 ; i < slotsPerPage ; i++) {
            Transform slot = slotsParent.GetChild( i );
            Button slotBtn = slot.GetComponent<Button>();
            Image slotImage = slot.GetComponentInChildren<Image>();

            int prefabIndex = startIndex + i;
            if (prefabIndex < endIndex) {
                // 有可用的预制体
                GameObject nodePrefab = nodePrefabs[ prefabIndex ];
                slot.gameObject.SetActive( true );

                // 设置按钮点击事件
                // 先移除旧监听，再添加新的监听
                slotBtn.onClick.RemoveAllListeners();
                slotBtn.onClick.AddListener( () => {
                    // 调用放置系统，选中此预制体
                    placementSystem.SelectPrefab( nodePrefab );
                } );

                // 从预制体上获取节点脚本及其图标
                NodeBase nodeScript = nodePrefab.GetComponent<NodeBase>();
                if (nodeScript != null && slotImage != null) {
                    slotImage.sprite = nodeScript.nodeIcon;  // 显示节点图标
                }

            } else {
                // 超出范围，没有此槽数据
                slot.gameObject.SetActive( false );
                slotBtn.onClick.RemoveAllListeners();
            }
        }
    }

    public void ToggleUIBar() {
        isOpen = !isOpen;
        uiRoot.SetActive( isOpen );
    }
}
