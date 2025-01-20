using UnityEngine;
using DemoProject.Core;

namespace DemoProject.Systems {
    public class ConnectionTool : MonoBehaviour {
        public GameObject connectionLinePrefab;
        // 是否启用连线模式
        public bool isActive = false;

        private NodeBase firstNode = null;

        void Update() {
            if(Input.GetKeyDown(KeyCode.C)) {
                SetConnectionMode(!isActive);
            }

            // 如果连线模式未启用，直接跳过
            if (!isActive) return;

            // 鼠标左键点击
            if (Input.GetMouseButtonDown( 0 )) {
                // 2D 射线检测
                RaycastHit2D hit = Physics2D.Raycast(
                    Camera.main.ScreenToWorldPoint( Input.mousePosition ) ,
                    Vector2.zero
                );
                if (hit.collider != null) {
                    NodeBase node = hit.collider.GetComponent<NodeBase>();
                    if (node != null) {
                        if (firstNode == null) {
                            firstNode = node;
                        } else {
                            // 第二次点击，创建连接
                            CreateConnection( firstNode , node );
                            firstNode = null;
                        }
                    }
                }
            }
        }

        void CreateConnection( NodeBase source , NodeBase target ) {
            if (connectionLinePrefab == null) return;

            GameObject lineObj = Instantiate( connectionLinePrefab , Vector3.zero , Quaternion.identity );
            ConnectionLine line = lineObj.GetComponent<ConnectionLine>();
            line.sourceNode = source;
            line.targetNode = target;

            source.outputConnections.Add( line );
            target.inputConnections.Add( line );
        }

        /// <summary>
        /// 对外提供启用/禁用连线模式的接口
        /// </summary>
        public void SetConnectionMode( bool active ) {
            isActive = active;
            // 如果要在关闭连线模式时清空 firstNode，可以在此处处理
            if (!active) {
                firstNode = null;
            }
        }
    }
}
