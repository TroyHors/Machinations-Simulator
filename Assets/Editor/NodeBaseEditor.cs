using UnityEngine;
using UnityEditor;  // 引入编辑器命名空间

// 假设 NodeBase 在 DemoProject.Core 命名空间下，按实际情况修改
using DemoProject.Core;

/// <summary>
/// 自定义编辑器，适用于 NodeBase 及其所有子类
/// </summary>
[CustomEditor( typeof( NodeBase ) , true )]
public class NodeBaseEditor : Editor {
    public override void OnInspectorGUI() {
        // 先绘制默认的 Inspector 内容
        base.OnInspectorGUI();

        // 拿到当前检查的脚本实例
        NodeBase nodeBase = (NodeBase) target;

        // 如果节点里有 inventory，显示其内容
        EditorGUILayout.Space();
        EditorGUILayout.LabelField( "===== Runtime Debug Info =====" , EditorStyles.boldLabel );

        // 1. 显示库存信息
        if (nodeBase.inventory != null && nodeBase.inventory.resourceDict != null) {
            EditorGUILayout.LabelField( "Current Resource Inventory:" );
            EditorGUI.indentLevel++;
            foreach (var kvp in nodeBase.inventory.resourceDict) {
                string resourceName = kvp.Key.ToString();
                float resourceAmount = kvp.Value;
                EditorGUILayout.LabelField( resourceName , resourceAmount.ToString() );
            }
            EditorGUI.indentLevel--;
        }

        // 2. 显示其他调试信息（若有）
        //   如果您想要显示“是否在进行资源传输”等状态，则可以在 NodeBase 或其子类
        //   声明一个 public/内部只读的布尔或字符串属性，通过这里读取并显示。
        //   例如：
        /*
        var converter = nodeBase as Converter;
        if (converter != null)
        {
            EditorGUILayout.LabelField("IsTransferring", converter.IsTransferring.ToString());
        }
        */

        EditorGUILayout.Space();
    }
}
