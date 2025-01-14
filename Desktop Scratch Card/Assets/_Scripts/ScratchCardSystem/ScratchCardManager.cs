using System;
using _Scripts.ItemCountGenerator;
using _Scripts.ScratchCardSystem.GridSystem;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

// using _Scripts.Merger;

namespace _Scripts.ScratchCardSystem
{
    public class ScratchCardManager : MonoBehaviour
    {
        public static Action<Vector2Int, bool> onCoverRevealStateChanged;
        public static Action<Vector2Int> onCoverRevealed;
        public static Action<Vector2Int> onMouseOverRevealedItem;
        public static Action onMouseExitRevealedItem;
        public static Action<Vector2Int> onMouseDownRevealedItem;

        public static Action<ScratchCard> onScratchCardSubmitted;

        [Header("Scratch Card Master")]
        public GridItemSO gridItemSo;
        public ScratchCardGenerator scratchCardGenerator;
        public GridItemCountGenerator gridItemCountGenerator;

        [Header("Grid Settings")]
        public Vector2Int gridDimension = new Vector2Int(3, 3);
        public Vector2 gridGapLength = Vector2.zero;
        public Vector2 generateStartPoint = Vector2.zero;
        public Vector2 generateStartAnimationOffset = Vector2.zero;

        [Header("Spawn Time")]
        public float meanSpawnTime = 15f;
        private float nextSpawnTime;         // 下次生成时间
        private System.Random random;

        private void OnEnable()
        {
            onCoverRevealStateChanged += OnGridRevealStateChanged;
            onMouseOverRevealedItem += OnMouseOverRevealedItem;
            onMouseExitRevealedItem += OnMouseExitRevealedItem;
            onMouseDownRevealedItem += OnMouseDownRevealedItem;

            onCoverRevealed += OnCoverRevealed;
        }

        private void OnDisable()
        {
            onCoverRevealStateChanged -= OnGridRevealStateChanged;
            onMouseOverRevealedItem -= OnMouseOverRevealedItem;
            onMouseExitRevealedItem -= OnMouseExitRevealedItem;
            onMouseDownRevealedItem -= OnMouseDownRevealedItem;

            onCoverRevealed -= OnCoverRevealed;
        }

        // TODO: trigger the generator of the scratch card

        private void Awake()
        {
            random = new System.Random();
            CalculateNextSpawnTime();
        }

        void Start()
        {
            // GenerateScratchCard();
            // _gridItemMerger = new GridItemMerger(gridItemSo, _gridData);
        }

        private void Update()
        {
            // 检查是否到达生成时间且未超过最大数量
            if (Time.time >= nextSpawnTime & _currentScratchCard == null)
            {
                GenerateScratchCard();
                CalculateNextSpawnTime();
            }

            if (Input.GetKeyDown(KeyCode.Space)) GenerateScratchCard();
            if (Input.GetKeyDown(KeyCode.Return)) SubmitScratchCard();
        }

        private void CalculateNextSpawnTime()
        {
            // 使用指数分布（泊松过程的时间间隔）
            float lambda = 1f / meanSpawnTime;
            float randomValue = (float)(-Math.Log(1f - (float)random.NextDouble()) / lambda);

            // 将当前时间加上随机间隔
            nextSpawnTime = Time.time + randomValue;
        }

        private void GenerateScratchCard()
        {
            print("in generating");
            if (_currentScratchCard != null) return;
            print("card is null");

            // TODO: give card
            var itemCounts = gridItemCountGenerator.GenerateGridItemCount(gridDimension);

            scratchCardGenerator.Initialize(gridDimension, gridGapLength, generateStartPoint, gridItemSo, itemCounts);
            _currentScratchCard = scratchCardGenerator.GenerateScratchCard();
            _currentScratchCard.transform.position += (Vector3)generateStartAnimationOffset;
            _currentScratchCard.transform.DOMove(
                (Vector2)_currentScratchCard.transform.position - generateStartAnimationOffset, 0.25f);
        }

        private ScratchCard _currentScratchCard;
        private int _revealedGrids = 0;
        private bool _scratchCardFinished = false;

        private void SubmitScratchCard()
        {
            if (_scratchCardFinished)
            {
                _currentScratchCard.transform
                    .DOMove((Vector2)_currentScratchCard.transform.position + generateStartAnimationOffset, 0.25f)
                    .OnComplete((
                        () =>
                        {
                            _scratchCardFinished = false;
                            // TODO: destroy after the bonus stage ends
                            _currentScratchCard.SelfDestroy();
                            _revealedGrids = 0;
                            // print("scratch card submitted");

                            // generate items
                            onScratchCardSubmitted?.Invoke(_currentScratchCard);
                        }));
            }

            CalculateNextSpawnTime();
        }

        private void OnCoverRevealed(Vector2Int revealedGrid)
        {
            _revealedGrids++;
            int totalGrids = gridDimension.x * gridDimension.y;
            if (_revealedGrids == totalGrids)
            {
                _scratchCardFinished = true;
            }
        }

        private void OnGridGenerated()
        {
            // 网格生成完成后的逻辑
        }

        private void OnGridRevealStateChanged(Vector2Int revealedGrid, bool isRevealed)
        {
            // if (isRevealed) _gridData.revealedGrids.Add(revealedGrid);
            // else _gridData.revealedGrids.Remove(revealedGrid);
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
