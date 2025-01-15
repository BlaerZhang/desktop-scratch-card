using _Scripts.General;
using UnityEngine;

namespace _Scripts.PlayerUpgrades.AbilityUpgrades
{
    public class ScratchCardSpawnUpgrade : AbilityUpgrade
    {
        public override float CalculateResult(int level)
        {
            return -level;
        }

        protected override void ApplyResult(float result)
        {
            GameManager.Instance.dataManager.abilityUpgradeData.CardMeanSpawnReduction = result;
        }

        protected override int CalculateUpgradePrice(int level)
        {
            return Mathf.RoundToInt(initialPrice * Mathf.Pow(1.07f, level));
        }
    }
}