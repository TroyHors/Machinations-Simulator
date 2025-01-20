using DemoProject.Core;
using UnityEngine;

public class Consumer : NodeBase {
    public ResourceType consumeType;
    public float consumeRate = 1f; // 每秒需要消耗多少
    private float timer;

    public override void NodeUpdate( float deltaTime ) {
        timer += deltaTime;
        if (timer >= 1f) {
            float amountToConsume = consumeRate * Mathf.Floor( timer );
            timer -= Mathf.Floor( timer );

            // 1) 先尝试本地库存
            bool success = TryConsumeResource( consumeType , amountToConsume );
            if (!success) {
                // 2) 如果本地库存不足，则主动从输入连接拉取剩余量
                float needed = amountToConsume - GetResource( consumeType );
                if (needed > 0f) {
                    PullResourceFromConnections( consumeType , needed );
                }

                // 3) 再次尝试消耗
                TryConsumeResource( consumeType , amountToConsume );
            }
        }
    }

    /// <summary>
    /// 如果库存不足，从所有 inputConnections 中尝试拉取 needed 数量的指定资源。
    /// </summary>
    private void PullResourceFromConnections( ResourceType type , float needed ) {
        float remainingNeeded = needed;

        // 按顺序从每条输入连接的 sourceNode 中拉取
        for (int i = 0 ; i < inputConnections.Count && remainingNeeded > 0f ; i++) {
            NodeBase source = inputConnections[ i ].sourceNode;
            float available = source.GetResource( type );

            if (available > 0) {
                // 实际可拉取量 = min(可用, 需求)
                float pullAmount = Mathf.Min( available , remainingNeeded );
                bool pullSuccess = source.TryConsumeResource( type , pullAmount );
                if (pullSuccess) {
                    // 拉成功后加入自己库存
                    AddResource( type , pullAmount );
                    remainingNeeded -= pullAmount;
                }
            }
        }
    }
}
