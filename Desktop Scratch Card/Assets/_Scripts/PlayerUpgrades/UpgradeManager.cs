using System;
using System.Collections.Generic;
using _Scripts.General;
using _Scripts.PlayerUpgrades.AbilityUpgrades;
using _Scripts.PlayerUpgrades.ScratchCardUpgrades;
using _Scripts.ScratchCardSystem;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.PlayerUpgrades
{
    public class UpgradeManager : SerializedMonoBehaviour
    {
        private GameObject _scratchCardUpgradesHolder;
        private GameObject _abilityUpgradesHolder;

        // 存储所有可用的升级
        public List<AbilityUpgrade> abilityUpgradesPool = new List<AbilityUpgrade>();
        public List<ScratchCardUpgrade> scratchCardUpgradesPool = new List<ScratchCardUpgrade>();
        // 存储已激活的升级
        private Dictionary<string, ScratchCardUpgrade> _activeCardUpgrades = new Dictionary<string, ScratchCardUpgrade>();
        private Dictionary<string, AbilityUpgrade> _activeAbilityUpgrades = new Dictionary<string, AbilityUpgrade>();

        [Title("Ability Upgrade UI")] 
        public Dictionary<string, GameObject> upgradeUIComponents = new Dictionary<string, GameObject>();

        public static Action onAbilityUpgraded;

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
            EconomyManager.onCurrencyChanged += UpdateUpgradeUI;
        }

        private void OnDisable()
        {
            ScratchCardManager.onScratchCardSubmitted += PlayScratchCardUpgrade;
            EconomyManager.onCurrencyChanged -= UpdateUpgradeUI;
        }

        private void Start()
        { 
            UpdateUpgradeUI();
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

        public void BuyAbilityUpgrade(string id)
        {
            AbilityUpgrade currentUpgrade = _activeAbilityUpgrades.ContainsKey(id)
                ? _activeAbilityUpgrades[id]
                : abilityUpgradesPool.Find(upgrade => upgrade.id == id);
            if (GameManager.Instance.economyManager.Currency < currentUpgrade.Price) return;
            GameManager.Instance.economyManager.Currency -= currentUpgrade.Price;
            AddAbilityUpgrade(id);
            onAbilityUpgraded?.Invoke();
            UpdateUpgradeUI();
        }

        private void UpdateUpgradeUI()
        {
            foreach (var uiComponentKVP in upgradeUIComponents)
            {
                TMP_Text levelText = uiComponentKVP.Value.transform.Find("Level Text").GetComponent<TMP_Text>();
                TMP_Text effectPreviewText = uiComponentKVP.Value.transform.Find("Effect Preview Text").GetComponent<TMP_Text>();
                TMP_Text priceText = uiComponentKVP.Value.transform.Find("Button").GetComponentInChildren<TMP_Text>();
                Button button = uiComponentKVP.Value.GetComponentInChildren<Button>();
                AbilityUpgrade currentUpgrade = _activeAbilityUpgrades.ContainsKey(uiComponentKVP.Key)
                    ? _activeAbilityUpgrades[uiComponentKVP.Key]
                    : abilityUpgradesPool.Find(upgrade => upgrade.id == uiComponentKVP.Key);

                levelText.text = $"Lv.{currentUpgrade.Level}";
                effectPreviewText.text =
                    $"{currentUpgrade.CalculateResult(currentUpgrade.Level)}   →   {currentUpgrade.CalculateResult(currentUpgrade.Level + 1)}";
                priceText.text = $"{currentUpgrade.Price}";
                button.interactable = GameManager.Instance.economyManager.Currency >= currentUpgrade.Price;
            }
        }
    }-
}