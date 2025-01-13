using _Scripts.General.GameData;
using UnityEngine;

namespace _Scripts.General
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public GameDataManager dataManager;

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