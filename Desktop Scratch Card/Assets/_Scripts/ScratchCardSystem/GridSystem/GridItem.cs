using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.ScratchCardSystem.GridSystem
{
    public class GridItem : MonoBehaviour
    {
        public GridItemType type { get; private set; }
        public int itemCount;
        public TMP_Text itemCountText;
        public GridItemData GridItemData { get; private set; }

        public void Initialize(GridItemType type, int itemCount, TMP_Text itemCountText, GridItemData gridItemData)
        {
            this.type = type;
            this.itemCount = itemCount;
            this.itemCountText = itemCountText;
            this.GridItemData = gridItemData;
        }
    }
}
