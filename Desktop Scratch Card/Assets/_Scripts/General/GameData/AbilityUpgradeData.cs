using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Scripts.General.GameData
{
    [CreateAssetMenu(fileName = "AbilityUpgradeData", menuName = "Scriptable Objects/Ability Upgrade Data", order = 0)]
    public class AbilityUpgradeData : SerializedScriptableObject
    {
        [DictionaryDrawerSettings(KeyLabel = "ability ID", ValueLabel = "ability variable value")]
        public Dictionary<string, float> abilityUpgradeData;
    }
}