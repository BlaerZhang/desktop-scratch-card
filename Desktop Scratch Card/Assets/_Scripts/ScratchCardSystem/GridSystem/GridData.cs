using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.ScratchCardSystem.GridSystem
{
    public class GridData
    {
        public GridItem[,] items { get; set; }
        public List<Vector2Int> revealedGrids { get; set; }

        public GridData()
        {
            revealedGrids = new List<Vector2Int>();
        }
    }
}
