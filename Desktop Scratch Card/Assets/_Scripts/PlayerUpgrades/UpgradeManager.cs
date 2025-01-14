using System;
using System.Collections.Generic;
using _Scripts.PlayerUpgrades.AbilityUpgrades;
using _Scripts.PlayerUpgrades.ScratchCardUpgrades;
using _Scripts.ScratchCardSystem;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.PlayerUpgrades
{
    public class UpgradeManager : MonoBehaviour
    {
        private GameObject _scratchCardUpgradesHolder;
        private GameObject _abilityUpgradesHolder;

        // 存储所有可用的升级
        public List<AbilityUpgrade> abilityUpgradesPool = new List<AbilityUpgrade>();
        public List<ScratchCardUpgrade> scratchCardUpgradesPool = new List<ScratchCardUpgrade>();
        // 存储已激活的升级
        private Dictionary<string, ScratchCardUpgrade> _activeCardUpgrades = new Dictionary<string, ScratchCardUpgrade>();
        private Dictionary<string, AbilityUpgrade> _activeAbilityUpgrades = new Dictionary<string, AbilityUpgrade>();

        private void Awake()
        {
            _scratchCardUpgradesHolder = new GameObject("Active Scratch Card Upgrades")
            {
                transform =
                {
                    parent = transform
                }
            };
            _abilityUpgradesHolder = new GameObject("Active Ability Upgrades")
            {
                transform =
                {
                    parent = transform
                }
            };
        }

        private void OnEnable()
        {
            ScratchCardManager.onAllCardCoverRevealed += PlayScratchCardUpgrade;
        }

        private void OnDisable()
        {
            ScratchCardManager.onScratchCardSubmitted += PlayScratchCardUpgrade;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q)) AddAbilityUpgrade("0");
            if (Input.GetKeyDown(KeyCode.W))
            {
                print("card upgraded: 0");
                AddCardUpgrade("0");
            }
        }

        /// <summary>
        /// play all available card bonus upgrade
        /// </summary>
        /// <param name="card"></param>
        private void PlayScratchCardUpgrade(ScratchCard card)
        {
            Sequence upgradeSequence = DOTween.Sequence();
            foreach (var upgrade in _activeCardUpgrades)
            {
                if (upgrade.Value.CheckWin(card, out Sequence winEffect))
                {
                    upgradeSequence.Append(winEffect);
                }
            }

            upgradeSequence?.Play();
        }

        /// <summary>
        /// triggered by events
        /// </summary>
        /// <param name="id"></param>
        private void AddCardUpgrade(string id)
        {
            ScratchCardUpgrade currentUpgrade = scratchCardUpgradesPool.Find(upgrade => upgrade.id == id);
            if (_activeCardUpgrades.TryAdd(id, currentUpgrade))
            {
                Instantiate(currentUpgrade, _scratchCardUpgradesHolder.transform);
            }
        }

        private void RemoveCardUpgrade(string id)
        {
            Destroy(_activeCardUpgrades[id].gameObject);
            _activeCardUpgrades.Remove(id);
        }

        /// <summary>
        /// triggered by update
        /// </summary>
        /// <param name="id"></param>
        private void AddAbilityUpgrade(string id)
        {
            // level up
            if (!_activeAbilityUpgrades.TryGetValue(id, out var activeAbilityUpgrade))
            {
                var abilityUpgrade = abilityUpgradesPool.Find(upgrade => upgrade.id == id);
                var currentUpgrade = Instantiate(abilityUpgrade, _abilityUpgradesHolder.transform);
                _activeAbilityUpgrades.Add(id, currentUpgrade);
                currentUpgrade.Level++;
            }

            if (activeAbilityUpgrade != null) activeAbilityUpgrade.Level++;
        }
    }
}