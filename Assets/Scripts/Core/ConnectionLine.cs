using UnityEngine;

namespace DemoProject.Core {
    [RequireComponent( typeof( LineRenderer ) )]
    public class ConnectionLine : MonoBehaviour {
        public NodeBase sourceNode;
        public NodeBase targetNode;

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
                lineRenderer.SetPosition( 0 , sourceNode.transform.position );
                lineRenderer.SetPosition( 1 , targetNode.transform.position );
            }
        }
    }
}
