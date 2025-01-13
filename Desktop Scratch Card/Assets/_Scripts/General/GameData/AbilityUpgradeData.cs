using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.General.GameData
{
    [CreateAssetMenu(fileName = "AbilityUpgradeData", menuName = "Scriptable Objects/Ability Upgrade Data", order = 0)]
    public class AbilityUpgradeData : SerializedScriptableObject
    {
        [SerializeField] private float cardMinSpawnTime;
        public float CardMinSpawnTime
        {
            get => cardMinSpawnTime;
            set => cardMinSpawnTime = Mathf.Max(0, value);
        }
    }
}