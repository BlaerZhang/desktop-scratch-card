using System.Collections.Generic;
using _Scripts.ScratchCardSystem.GridSystem;
using UnityEngine;

namespace _Scripts.ScratchCardSystem
{
    public class ScratchCard : MonoBehaviour
    {
        public GridData gridData { get; } = new GridData();

        private readonly List<Vector2Int> _rewardsList = new List<Vector2Int>();

        public void Initialize(int rows, int columns)
        {
            gridData.items = new GridItem[rows, columns];
        }

        public void SetCardItemMatrix(int row, int column, GridItem gridItem)
        {
            gridData.items[row, column] = gridItem;
        }

        public void AddReward(GridItemType itemType, int count)
        {
            _rewardsList.Add(new Vector2Int((int)itemType, count));
        }

        public List<Vector2Int> GetRewardList()
        {
            return _rewardsList;
        }

        public void SelfDestroy()
        {
            Destroy(gameObject);
        }
    }
}