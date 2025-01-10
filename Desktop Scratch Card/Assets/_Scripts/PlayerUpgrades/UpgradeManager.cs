using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.PlayerUpgrades
{
    public class UpgradeManager : MonoBehaviour
    {
        private GameObject _scratchCardUpgradesHolder = new GameObject("Active Scratch Card Upgrades");
        private GameObject _abilityUpgradesHolder = new GameObject("Active Ability Upgrades");

        // 存储所有可用的升级
        public List<AbilityUpgrade> abilityUpgradesPool = new List<AbilityUpgrade>();
        public List<ScratchCardUpgradeEffect> scratchCardUpgradesPool = new List<ScratchCardUpgradeEffect>();
        // 存储已激活的升级
        private Dictionary<string, ScratchCardUpgradeEffect> _activeCardUpgrades = new Dictionary<string, ScratchCardUpgradeEffect>();
        private Dictionary<string, AbilityUpgrade> _activeAbilityUpgrades = new Dictionary<string, AbilityUpgrade>();

        private void AddCardUpgrade(string id)
        {
            ScratchCardUpgradeEffect currentUpgrade = scratchCardUpgradesPool.Find(upgrade => upgrade.id == id);
            Instantiate(currentUpgrade, _scratchCardUpgradesHolder.transform);
            _activeCardUpgrades.TryAdd(id, currentUpgrade);
        }

        private void RemoveCardUpgrade(string id)
        {
            Destroy(_activeCardUpgrades[id].gameObject);
            _activeCardUpgrades.Remove(id);
        }

        private void AddAbilityUpgrade(string id)
        {
            // level up
            if (!_activeAbilityUpgrades.TryGetValue(id, out var abilityUpgrade))
            {
                var currentUpgrade = abilityUpgradesPool.Find(upgrade => upgrade.id == id);
                Instantiate(currentUpgrade, _abilityUpgradesHolder.transform);
                _activeAbilityUpgrades.TryAdd(id, currentUpgrade);
            }

            if (abilityUpgrade != null) abilityUpgrade.Level++;
        }
    }
}