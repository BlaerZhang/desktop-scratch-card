using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.General.GameData
{
    [CreateAssetMenu(fileName = "AbilityUpgradeData", menuName = "Scriptable Objects/Ability Upgrade Data", order = 0)]
    public class AbilityUpgradeData : SerializedScriptableObject
    {
        [SerializeField] private float cardMeanSpawnReduction;
        public float CardMeanSpawnReduction
        {
            get => cardMeanSpawnReduction;
            set => cardMeanSpawnReduction = Mathf.Min(0, value);
        }

        [SerializeField] private float orderRewardBoost = 1;
        public float OrderRewardBoost
        {
            get => orderRewardBoost;
            set => orderRewardBoost = Mathf.Max(0, value);
        }
    }
}