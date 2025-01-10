using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.GridSystem
{
    public class GridData
    {
        public GridItem[,] items { get; set; }
        // public MergerGridCover[,] covers { get; set; }
        public List<Vector2Int> revealedGrids { get; set; }

        public GridData()
        {
            revealedGrids = new List<Vector2Int>();
        }
    }
}
