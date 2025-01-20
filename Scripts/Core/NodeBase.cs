using UnityEngine;
using System.Collections.Generic;

namespace DemoProject.Core {
    /// <summary>
    /// 所有节点（资源池、产出器、消耗器、转换器）的基类。
    /// 持有 ResourceInventory，并在初始化时注册到全局的管理器中。
    /// </summary>
    public abstract class NodeBase : MonoBehaviour {
        [Header( "Base Node Info" )]
        public ResourceInventory inventory = new ResourceInventory();

        // 节点的输入、输出连接列表
        [HideInInspector] public List<ConnectionLine> inputConnections = new List<ConnectionLine>();
        [HideInInspector] public List<ConnectionLine> outputConnections = new List<ConnectionLine>();

        // 在这里可以设定节点通用的唯一 ID、名字等信息（可选）
        public string nodeName = "BaseNode";

        protected virtual void Start() {
            // 在启动时注册到管理器
            ResourceFlowManager.Instance.RegisterNode( this );
        }

        /// <summary>
        /// 每帧或每固定时间的逻辑，统一由管理器调用
        /// </summary>
        public virtual void NodeUpdate( float deltaTime ) {
            // 子类重写
        }

        // 提供一些常用的资源操作
        public virtual float GetResource( ResourceType type ) {
            return inventory.GetResourceAmount( type );
        }

        public virtual void AddResource( ResourceType type , float amount ) {
            inventory.AddResource( type , amount );
        }

        public virtual bool TryConsumeResource( ResourceType type , float amount ) {
            return inventory.TryConsumeResource( type , amount );
        }
    }
}
