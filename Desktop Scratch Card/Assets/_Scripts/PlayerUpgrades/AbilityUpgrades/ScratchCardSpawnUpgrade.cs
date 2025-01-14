using _Scripts.General;
using UnityEngine;

namespace _Scripts.PlayerUpgrades.AbilityUpgrades
{
    public class ScratchCardSpawnUpgrade : AbilityUpgrade
    {
        protected override void CalculateResult(int level)
        {
            GameManager.Instance.dataManager.abilityUpgradeData.CardMeanSpawnTime--;
        }

        protected override int CalculateUpgradePrice(int level)
        {
            return Mathf.RoundToInt(initialPrice * Mathf.Pow(1.07f, level));
        }
    }
}