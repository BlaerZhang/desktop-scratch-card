using System;
using System.Collections.Generic;
using _Scripts.ItemCountGenerator;
using TMPro;
// using _Scripts.Merger;
using UnityEngine;

namespace _Scripts.GridSystem
{
    public class GridManager : MonoBehaviour
    {
        public static Action<Vector2Int, bool> onCoverRevealStateChanged;
        public static Action<Vector2Int> onCoverRevealed;
        public static Action<Vector2Int> onMouseOverRevealedItem;
        public static Action onMouseExitRevealedItem;
        public static Action<Vector2Int> onMouseDownRevealedItem;

        [Header("Grid Master")]
        public GridItemSO gridItemSo;
        public GridGenerator gridGenerator;
        public GridItemCountGenerator gridItemCountGenerator;

        [Header("Grid Settings")]
        public Vector2Int gridDimension = new Vector2Int(3, 3);
        public Vector2 gridGapLength = Vector2.zero;
        public Vector2 generateStartPoint = Vector2.zero;
        
        private GridData _gridData;

        private void OnEnable()
        {
            onCoverRevealStateChanged += OnGridRevealStateChanged;
            onMouseOverRevealedItem += OnMouseOverRevealedItem;
            onMouseExitRevealedItem += OnMouseExitRevealedItem;
            onMouseDownRevealedItem += OnMouseDownRevealedItem;
        }

        private void OnDisable()
        {
            onCoverRevealStateChanged -= OnGridRevealStateChanged;
            onMouseOverRevealedItem -= OnMouseOverRevealedItem;
            onMouseExitRevealedItem -= OnMouseExitRevealedItem;
            onMouseDownRevealedItem -= OnMouseDownRevealedItem;
        }

        // TODO: trigger the generator of the scratch card

        void Start()
        {
            _gridData = new GridData();

            GenerateScratchCard();
            // _gridItemMerger = new GridItemMerger(gridItemSo, _gridData);
        }

        private void GenerateScratchCard()
        {
            var itemCounts = gridItemCountGenerator.GenerateGridItemCount(gridDimension);

            gridGenerator.Initialize(gridDimension, gridGapLength, generateStartPoint, gridItemSo, _gridData, itemCounts);
            gridGenerator.GenerateAllGrids();
        }

        private void OnGridGenerated()
        {
            // 网格生成完成后的逻辑
        }

        // private void MoveIcons()
        // {
        //     iconMover.MoveIcons();
        // }

        private void OnGridRevealStateChanged(Vector2Int revealedGrid, bool isRevealed)
        {
            if (isRevealed) _gridData.revealedGrids.Add(revealedGrid);
            else _gridData.revealedGrids.Remove(revealedGrid);
        }

        // private List<Vector2Int> _cluster = new List<Vector2Int>();
        private void OnMouseOverRevealedItem(Vector2Int originItemGrid)
        {
            // _cluster = _clusterDetector.CheckClusters(originItemGrid);
            //
            // if (_cluster is null) return;
            //
            // print("cluster count: " + _cluster.Count);
            // if (_cluster.Count < 2) return;
            // // set bg color
            // foreach (var i in _cluster)
            // {
            //     Instantiate(clusterBGPrefab, _gridData.items[i.x, i.y].transform);
            // }
        }

        private void OnMouseExitRevealedItem()
        {
            // reset color
            // var clusterBGs = GameObject.FindGameObjectsWithTag("ClusterBG");
            // foreach (var c in clusterBGs) Destroy(c);
        }

        private void OnMouseDownRevealedItem(Vector2Int mergeOrigin)
        {
            // if (_cluster is null) return;
            // if (_cluster.Count < 2) return;
            // var gridItemSpawnData = _gridItemMerger.CheckMerge(_cluster, mergeOrigin);

            // var mergedGrids = gridItemSpawnData.mergedGrids;

            // if (_gridData.items[mergeOrigin.x, mergeOrigin.y].type == GridItemType.Apple)
            //     AudioSource.PlayClipAtPoint(treeSound, Vector3.zero);
            // else AudioSource.PlayClipAtPoint(chickSound, Vector3.zero);
            //
            // DeleteGridItem(_cluster);

            // generate upgraded items
            // foreach (var m in mergedGrids)
            // {
            //     _gridGenerator.GenerateSingleGrid(m.x, m.y, gridItemSpawnData.itemType, gridItemSpawnData.GridItemData.level);
            //     _cluster.Remove(m);
            // }

            // generate random items for vacant grids
            // foreach (var i in _cluster)
            // {
            //     _gridGenerator.GenerateRandomGrid(i.x, i.y);
            //     // reset all covers for newly generated grids
            //     _gridData.covers[i.x, i.y].Reset();
            //     _gridData.revealedGrids.Remove(i);
            // }
        }

        // private void DeleteGridItem(List<Vector2Int> deleteList)
        // {
        //     foreach (var d in deleteList)
        //     {
        //         Destroy(_gridData.items[d.x, d.y].gameObject);
        //     }
        // }
    }
}
