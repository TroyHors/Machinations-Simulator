using System.Collections.Generic;
using UnityEngine;

namespace DemoProject.Core {
    public class ResourceFlowManager : MonoBehaviour {
        // 单例模式，便于随时访问
        public static ResourceFlowManager Instance { get; private set; }

        private List<NodeBase> allNodes = new List<NodeBase>();

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy( gameObject );
            }
        }

        // 节点启动时会调用此方法来注册自己
        public void RegisterNode( NodeBase node ) {
            if (!allNodes.Contains( node )) {
                allNodes.Add( node );
            }
        }

        private void Update() {
            float deltaTime = Time.deltaTime;
            // 在此统一更新所有节点
            for (int i = 0 ; i < allNodes.Count ; i++) {
                allNodes[ i ].NodeUpdate( deltaTime );
            }
        }
    }
}
