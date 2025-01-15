using System;
using _Scripts.General;
using _Scripts.PlayerUpgrades;
using _Scripts.PlayerUpgrades.AbilityUpgrades;
using UnityEngine;

public class OrderRewardBoostUpgrade : AbilityUpgrade
{
    public override float CalculateResult(int level)
    {
        return 1f + level / 10f;
    }

    protected override void ApplyResult(float result)
    {
        GameManager.Instance.dataManager.abilityUpgradeData.OrderRewardBoost = result;
    }

    protected override int CalculateUpgradePrice(int level)
    {
        return Mathf.RoundToInt(initialPrice * Mathf.Pow(1.07f, level));
    }
}
