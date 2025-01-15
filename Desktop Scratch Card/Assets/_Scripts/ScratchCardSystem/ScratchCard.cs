using System;
using System.Collections.Generic;
using _Scripts.ScratchCardSystem.GridSystem;
using UnityEngine;
using DG.Tweening;

namespace _Scripts.ScratchCardSystem
{
    public class ScratchCard : MonoBehaviour
    {
        public GridData gridData { get; } = new GridData();

        // private readonly List<Vector2Int> _rewardsList = new List<Vector2Int>();

        private void OnEnable()
        {
            ScratchCardManager.onCoverRevealed += ShowCount;
        }

        private void OnDisable()
        {
            ScratchCardManager.onCoverRevealed -= ShowCount;
        }

        public void Initialize(int rows, int columns)
        {
            gridData.items = new GridItem[rows, columns];
        }

        public void SetCardItemMatrix(int row, int column, GridItem gridItem)
        {
            gridData.items[row, column] = gridItem;
        }

        // public void AddReward(GridItemType itemType, int count)
        // {
        //     _rewardsList.Add(new Vector2Int((int)itemType, count));
        // }

        // public List<Vector2Int> GetRewardList()
        // {
        //     return _rewardsList;
        // }

        private void ShowCount(Vector2Int revealedGrid)
        {
            gridData.items[revealedGrid.x, revealedGrid.y].itemCountText.DOFade(1, 0.1f);
        }

        public void SelfDestroy()
        {
            Destroy(gameObject);
        }
    }
}