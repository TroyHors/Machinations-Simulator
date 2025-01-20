using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour {
    public float cellSize = 1f;

    // 已占用格子集合
    private HashSet<Vector2Int> occupiedCells = new HashSet<Vector2Int>();

    /// <summary>
    /// 将世界坐标转换为整数网格坐标 (x,y)
    /// </summary>
    public Vector2Int WorldToGrid( Vector3 worldPosition ) {
        int x = Mathf.FloorToInt( worldPosition.x / cellSize );
        int y = Mathf.FloorToInt( worldPosition.y / cellSize );
        return new Vector2Int( x , y );
    }

    /// <summary>
    /// 将世界坐标对齐到网格中心点
    /// 用于确定放置物体的最终位置
    /// </summary>
    public Vector3 GetSnappedPosition( Vector3 worldPosition ) {
        Vector2Int gridPos = WorldToGrid( worldPosition );
        float snappedX = gridPos.x * cellSize + cellSize / 2f;
        float snappedY = gridPos.y * cellSize + cellSize / 2f;
        return new Vector3( snappedX , snappedY , 0f );
    }

    /// <summary>
    /// 检查某个网格坐标是否被占用
    /// </summary>
    public bool IsCellOccupied( Vector2Int gridPos ) {
        return occupiedCells.Contains( gridPos );
    }

    /// <summary>
    /// 标记某个网格坐标为已占用
    /// </summary>
    public void OccupyCell( Vector2Int gridPos ) {
        if (!occupiedCells.Contains( gridPos )) {
            occupiedCells.Add( gridPos );
        }
    }

    /// <summary>
    /// 如果需要，提供释放格子的接口
    /// </summary>
    public void FreeCell( Vector2Int gridPos ) {
        if (occupiedCells.Contains( gridPos )) {
            occupiedCells.Remove( gridPos );
        }
    }
}
