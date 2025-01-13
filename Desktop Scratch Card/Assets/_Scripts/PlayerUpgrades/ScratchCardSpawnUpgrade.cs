using _Scripts.General;
using UnityEngine;

namespace _Scripts.PlayerUpgrades
{
    public class ScratchCardSpawnUpgrade : AbilityUpgrade
    {
        protected override void CalculateResult(int level)
        {
            GameManager.Instance.dataManager.abilityUpgradeData.CardMinSpawnTime--;
        }
    }
}