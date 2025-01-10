using System;
using UnityEngine;

namespace _Scripts.GridSystem
{
    [Serializable]
    public struct GridItemData
    {
        public int level;
        public string id;
        // public GridItemType type;
        public Sprite image;
        public int prize;
        [Tooltip("probability must be within 0.0-1.0, cumulative probability must be 1")]
        public float probability;
    }
}
