using UnityEngine;

namespace _Scripts.ScratchCardSystem.GridSystem
{
    public class GridItem : MonoBehaviour
    {
        public GridItemType type { get; private set; }
        public int itemCount;
        public GridItemData GridItemData { get; private set; }

        public void Initialize(GridItemType type, int itemCount, GridItemData gridItemData)
        {
            this.type = type;
            this.itemCount = itemCount;
            this.GridItemData = gridItemData;
        }
    }
}
