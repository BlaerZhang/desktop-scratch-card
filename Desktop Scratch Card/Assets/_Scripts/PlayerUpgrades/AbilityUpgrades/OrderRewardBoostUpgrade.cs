using System;
using _Scripts.General;
using _Scripts.PlayerUpgrades;
using _Scripts.PlayerUpgrades.AbilityUpgrades;
using UnityEngine;

public class OrderRewardBoostUpgrade : AbilityUpgrade
{
    protected override void CalculateResult(int level)
    {
        GameManager.Instance.dataManager.abilityUpgradeData.OrderRewardBoost = 1f + level / 10f;
    }

    protected override int CalculateUpgradePrice(int level)
    {
        return Mathf.RoundToInt(initialPrice * Mathf.Pow(1.07f, level));
    }
}
