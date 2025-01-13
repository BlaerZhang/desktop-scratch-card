using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.ScratchCardSystem.GridSystem
{
    public class ClusterDetector
    {
        private GridItemSO _gridItemSo;
        private GridData _gridData;

        private int _rows;
        private int _columns;

        private bool[,] visitedGrids;
        private List<Vector2Int> cluster = new List<Vector2Int>();

        // private List<Vector2Int> scoredGrid = new List<Vector2Int>();

        public ClusterDetector(int rows, int columns, GridItemSO gridItemSo, GridData gridData)
        {
            _rows = rows;
            _columns = columns;
            _gridItemSo = gridItemSo;
            _gridData = gridData;
        }

        public List<Vector2Int> CheckClusters(Vector2Int originItem)
        {
            var gridItem = _gridData.items[originItem.x, originItem.y];
            if (gridItem.GridItemData.level == _gridItemSo.itemPool[gridItem.type].itemLevelData.Count - 1) return null;

            visitedGrids = new bool[_rows, _columns];

            // Debug.Log("origin: " + originItem);

            cluster = new List<Vector2Int>();
            FindCluster(originItem.x, originItem.y, _gridData.items[originItem.x, originItem.y].GridItemData.id);

            return cluster;
        }

        private void FindCluster(int x, int y, string iconId)
        {
            // 检查是否越界或者已经访问过
            if (x < 0 || x >= _rows || y < 0 || y >= _columns || visitedGrids[x, y])
                return;

            if (!_gridData.revealedGrids.Contains(new Vector2Int(x, y)))
                return;

            // 如果当前格子的类型不匹配，直接返回
            if (_gridData.items[x, y].GridItemData.id != iconId)
                return;

            // 标记为已访问
            visitedGrids[x, y] = true;

            // 将当前格子加入到聚集中
            cluster.Add(new Vector2Int(x, y));

            // 递归检查上下左右四个方向
            FindCluster(x + 1, y, iconId); // 右
            FindCluster(x + 1, y + 1, iconId); // 右上
            FindCluster(x + 1, y - 1, iconId); // 右下
            FindCluster(x - 1, y, iconId); // 左
            FindCluster(x - 1, y + 1, iconId); // 左上
            FindCluster(x - 1, y - 1, iconId); // 左下
            FindCluster(x, y + 1, iconId); // 上
            FindCluster(x, y - 1, iconId); // 下
        }
    }
}
