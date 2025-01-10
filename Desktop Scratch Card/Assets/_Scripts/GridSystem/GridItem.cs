using UnityEngine;

namespace _Scripts.GridSystem
{
    public class GridItem : MonoBehaviour
    {
        public GridItemType type { get; private set; }
        public GridItemData GridItemData { get; private set; }

        public void Initialize(GridItemType type, GridItemData gridItemData)
        {
            this.type = type;
            this.GridItemData = gridItemData;
        }
    }
}
