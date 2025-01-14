using _Scripts.General;

namespace _Scripts.PlayerUpgrades.AbilityUpgrades
{
    public class ScratchCardSpawnUpgrade : AbilityUpgrade
    {
        protected override void CalculateResult(int level)
        {
            GameManager.Instance.dataManager.abilityUpgradeData.CardMinSpawnTime--;
        }
    }
}