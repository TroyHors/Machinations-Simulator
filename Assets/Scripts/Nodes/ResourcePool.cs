using UnityEngine;
using System;
using DemoProject.Core;

public class ResourcePool : NodeBase {
    // 设定每秒进行几次拉取逻辑
    public float pullRate = 1f;

    // 每种资源类型单次拉取量
    public float pullAmountPerType = 1f;

    private float timer;

    public override void NodeUpdate( float deltaTime ) {
        // 按 pullRate 进行时间累积
        timer += deltaTime * pullRate;
        // 每当 timer >= 1，就进行一次“对所有资源类型的拉取”
        while (timer >= 1f) {
            timer -= 1f;
            PullAllResourceTypesFromConnections();
        }
    }

    /// <summary>
    /// 对所有可能的 ResourceType，从 inputConnections 拉取固定数量
    /// </summary>
    private void PullAllResourceTypesFromConnections() {
        foreach (ResourceType type in Enum.GetValues( typeof( ResourceType ) )) {
            PullResourceFromConnections( type , pullAmountPerType );
        }
    }

    /// <summary>
    /// 从 inputConnections 中主动拉取指定类型的资源 totalNeeded
    /// </summary>
    private void PullResourceFromConnections( ResourceType type , float totalNeeded ) {
        float remaining = totalNeeded;
        for (int i = 0 ; i < inputConnections.Count && remaining > 0f ; i++) {
            NodeBase source = inputConnections[ i ].sourceNode;
            float available = source.GetResource( type );

            if (available > 0f) {
                float pullAmount = Mathf.Min( available , remaining );
                bool success = source.TryConsumeResource( type , pullAmount );
                if (success) {
                    AddResource( type , pullAmount );
                    remaining -= pullAmount;
                }
            }
        }
    }
}
