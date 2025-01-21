using UnityEngine;

namespace DemoProject.Core {
    [RequireComponent( typeof( LineRenderer ) )]
    public class ConnectionLine : MonoBehaviour {
        public NodeBase sourceNode;
        public NodeBase targetNode;

        public GameObject arrowIndicator;

        private LineRenderer lineRenderer;

        private void Awake() {
            lineRenderer = GetComponent<LineRenderer>();
            // 可以在此设置一些默认可视化属性
            lineRenderer.startColor = new Color( 1f , 1f , 1f , 0.5f );
            lineRenderer.endColor = new Color( 1f , 1f , 1f , 0.5f );
            lineRenderer.positionCount = 2;
            lineRenderer.widthMultiplier = 0.05f;
        }

        private void Update() {
            if (sourceNode != null && targetNode != null) {
                Vector3 startPos = sourceNode.transform.position;
                Vector3 endPos = targetNode.transform.position;

                lineRenderer.SetPosition( 0 , startPos );
                lineRenderer.SetPosition( 1 , endPos );

                // 新增：将 arrowIndicator 放在连线中点并朝向目标
                if (arrowIndicator != null) {
                    Vector3 midPos = ( startPos + endPos ) * 0.5f;
                    arrowIndicator.transform.position = midPos;

                    Vector3 dir = endPos - startPos;
                    float angle = Mathf.Atan2( dir.y , dir.x ) * Mathf.Rad2Deg;
                    arrowIndicator.transform.rotation = Quaternion.Euler( 0f , 0f , angle );
                }
            }
        }
    }
}
