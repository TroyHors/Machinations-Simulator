using UnityEngine;
using DemoProject.Core;

namespace DemoProject.Nodes {
    public class Producer : NodeBase {
        [Header( "Producer Settings" )]
        public ResourceType produceType = ResourceType.Wood;
        public float produceRate = 1f; // 每秒产量

        private float timer;

        public override void NodeUpdate( float deltaTime ) {
            // 累计时间，达到 1 秒就产出
            timer += deltaTime;
            if (timer >= 1f) {
                // (1) 计算应产出的数量
                float amountToProduce = produceRate * Mathf.Floor( timer );

                // (2) 加入自身库存
                AddResource( produceType , amountToProduce );

                // (3) 重置计时器（只取余数，避免多次产出）
                timer = timer - Mathf.Floor( timer );
            }
        }
    }
}
