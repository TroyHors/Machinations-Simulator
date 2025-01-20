using System.Collections.Generic;
using UnityEngine;

namespace DemoProject.Core {
    [System.Serializable]
    public class ResourceInventory {
        // 以 ResourceType 为键，对应资源数量为值
        public Dictionary<ResourceType , float> resourceDict = new Dictionary<ResourceType , float>();

        public ResourceInventory() {
            // 初始化，将所有枚举值设为 0
            foreach (ResourceType type in System.Enum.GetValues( typeof( ResourceType ) )) {
                resourceDict[ type ] = 0f;
            }
        }

        public void AddResource( ResourceType type , float amount ) {
            if (amount > 0) {
                resourceDict[ type ] += amount;
            }
        }

        /// <summary>
        /// 消耗资源，若库存足够则返回 true，否则返回 false
        /// </summary>
        public bool TryConsumeResource( ResourceType type , float amount ) {
            if (resourceDict[ type ] >= amount && amount > 0) {
                resourceDict[ type ] -= amount;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取某种资源数量
        /// </summary>
        public float GetResourceAmount( ResourceType type ) {
            return resourceDict[ type ];
        }
    }
}
