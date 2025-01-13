using _Scripts.General.GameData;
using UnityEngine;

namespace _Scripts.General
{
    public class GameManager : MonoBehaviour
    {
        private GameDataManager _dataManager;
        public static GameManager Instance { get; private set; }
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
            _dataManager = GetComponentInChildren<GameDataManager>();
        }
    }
}