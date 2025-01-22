using DemoProject.Core;
using DemoProject.Systems;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementSystem : MonoBehaviour {
    public GridManager gridManager;             // 拖入场景中的 GridManager
    public ConnectionTool connectionTool;       // 拖入场景中的 ConnectionTool（可选）
    public Camera mainCamera;                   // 主相机

    private GameObject selectedPrefab;          // 当前选中的元件预制体
    private Vector2Int currentGridPos;          // 实时网格坐标
    private Vector3 currentWorldPos;            // 对齐后的世界坐标
    private bool canPlace;                      // 是否允许放置

    public NodeInspector nodeInspector;

    void Update() {
        // 1. 如果连线模式打开，则禁用放置功能
        if (connectionTool != null && connectionTool.isActive) {
            return; // 直接退出，不做放置检测
        }

        // 2. ESC 取消当前选中
        if (Input.GetKeyDown( KeyCode.Escape )) {
            selectedPrefab = null;
        }

        // 3. 如果有选中的预制体，实时检测能否放置
        if (selectedPrefab != null) {
            UpdatePlacementCheck();
        }

        // 4. 鼠标左键点击 -> 尝试放置
        if (Input.GetMouseButtonDown( 0 )) {
            
            RaycastHit2D hit = Physics2D.Raycast( Camera.main.ScreenToWorldPoint( Input.mousePosition ) , Vector2.zero );
            if (hit.collider != null) {
                NodeBase node = hit.collider.GetComponent<NodeBase>();
                if (node != null) {
                    nodeInspector.ShowNode( node );
                }
            }
            
            // 未选预制体则不操作
            if (selectedPrefab == null) return;

            // 若点在 UI 上，则不操作
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

            // 如果 canPlace 为 true，则放置
            if (canPlace) {
                Instantiate( selectedPrefab , currentWorldPos , Quaternion.identity );
                // 标记格子占用
                gridManager.OccupyCell( currentGridPos );
            }
        }
    }

    /// <summary>
    /// 实时检测鼠标位置的网格坐标，判断是否可放置
    /// </summary>
    private void UpdatePlacementCheck() {
        // 获取鼠标世界坐标
        Vector3 mousePos = mainCamera.ScreenToWorldPoint( Input.mousePosition );
        mousePos.z = 0f;

        // 网格对齐坐标
        currentWorldPos = gridManager.GetSnappedPosition( mousePos );
        // 整数网格坐标
        currentGridPos = gridManager.WorldToGrid( mousePos );

        // 若该格子已被占用，则不能放置
        canPlace = !gridManager.IsCellOccupied( currentGridPos );
    }

    /// <summary>
    /// 供 UI Button 调用，用于选中要放置的预制体
    /// </summary>
    public void SelectPrefab( GameObject prefab ) {
        selectedPrefab = prefab;
    }
}
