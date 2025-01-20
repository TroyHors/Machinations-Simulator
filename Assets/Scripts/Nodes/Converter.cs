using UnityEngine;
using DemoProject.Core;

namespace DemoProject.Nodes {
    public class Converter : NodeBase {
        [Header( "Converter Settings" )]
        public ResourceType inputType = ResourceType.Wood;
        public ResourceType outputType = ResourceType.Stone;
        public float ratioIn = 2f;   // 消耗多少输入资源
        public float ratioOut = 1f;  // 输出多少资源
        public float convertRate = 1f;  // 每秒尝试几次转换

        private float timer;

        public override void NodeUpdate( float deltaTime ) {
            timer += deltaTime * convertRate;

            // 当 timer >= 1，表示可进行一次或多次转换尝试
            if (timer >= 1f) {
                int conversionCount = Mathf.FloorToInt( timer );
                for (int i = 0 ; i < conversionCount ; i++) {
                    // 优先尝试消耗自身库存
                    if (TryConsumeResource( inputType , ratioIn )) {
                        AddResource( outputType , ratioOut );
                    } else {
                        // 自身库存不够，则尝试拉取
                        PullResourceFromConnections();
                    }
                }
                // 保留余数
                timer = timer - conversionCount;
            }
        }

        private void PullResourceFromConnections() {
            // 遍历所有 inputConnections
            foreach (var conn in inputConnections) {
                NodeBase source = conn.sourceNode;
                if (source.GetResource( inputType ) >= ratioIn) {
                    // 从 sourceNode 拉取
                    bool success = source.TryConsumeResource( inputType , ratioIn );
                    if (success) {
                        AddResource( outputType , ratioOut );
                        break; // 每次只转换一次
                    }
                }
            }
        }
    }
}
