using TMPro;
using UnityEngine;

namespace _Scripts.ScratchCardSystem.GridSystem
{
    public class GridItem : MonoBehaviour
    {
        public GridItemType type { get; private set; }
        public TMP_Text ItemCountText;
        public GridItemData GridItemData { get; private set; }

        public void Initialize(GridItemType type, TMP_Text ItemCountText, GridItemData gridItemData)
        {
            this.type = type;
            this.ItemCountText = ItemCountText;
            this.GridItemData = gridItemData;
        }
    }
}
