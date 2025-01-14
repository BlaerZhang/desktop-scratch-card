using _Scripts.General.GameData;
using _Scripts.PlayerUpgrades;
using UnityEngine;

namespace _Scripts.General
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public GameDataManager dataManager;
        public UpgradeManager upgradeManager;
        public EconomyManager economyManager;
        public OrderManager orderManager;
        public ItemManager itemManager;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                InitializeSystems();
            }
        }

        private void InitializeSystems()
        {
            // dataManager = GetComponentInChildren<GameDataManager>();
        }
    }
}