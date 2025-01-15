using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.General.GameData
{
    [CreateAssetMenu(fileName = "AbilityUpgradeData", menuName = "Scriptable Objects/Ability Upgrade Data", order = 0)]
    public class AbilityUpgradeData : SerializedScriptableObject
    {
        [SerializeField] private float cardMeanSpawnTime;
        public float CardMeanSpawnTime
        {
            get => cardMeanSpawnTime;
            set => cardMeanSpawnTime = Mathf.Max(0, value);
        }

        [SerializeField] private float orderRewardBoost = 1;
        public float OrderRewardBoost
        {
            get => orderRewardBoost;
            set => orderRewardBoost = Mathf.Max(0, value);
        }
    }
}