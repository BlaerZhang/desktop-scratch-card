using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.GridSystem
{
    public class ScratchCard : MonoBehaviour
    {
        private GridData _gridData = new GridData();
        private List<Vector2Int> _rewardsList = new List<Vector2Int>();

        public void Initialize(int rows, int columns)
        {
            _gridData.items = new GridItem[rows, columns];
        }

        public void SetCardItemMatrix(int row, int column, GridItem gridItem)
        {
            _gridData.items[row, column] = gridItem;
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