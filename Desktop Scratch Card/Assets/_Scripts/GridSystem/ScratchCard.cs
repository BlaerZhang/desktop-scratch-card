using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.GridSystem
{
    public class ScratchCard : MonoBehaviour
    {
        private List<Vector2Int> _rewardsList = new List<Vector2Int>();

        public void SelfDestroy()
        {
            Destroy(gameObject);
        }

        public void AddReward(GridItemType itemType, int count)
        {
            _rewardsList.Add(new Vector2Int((int)itemType, count));
        }

        public List<Vector2Int> GetRewardList()
        {
            return _rewardsList;
        }
    }
}