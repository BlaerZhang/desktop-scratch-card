using UnityEngine;

namespace _Scripts.PlayerUpgrades.AbilityUpgrades
{
    // TODO: upgrade shared data
    public abstract class AbilityUpgrade : BaseUpgrade
    {
        public int Level
        {
            get => level;
            set
            {
                level = value;
                CalculateResult(level);
            }
        }

        [SerializeField] protected int initialPrice;
        public int Price => CalculateUpgradePrice(level);

        /// <summary>
        /// variable calculation formula
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        protected abstract void CalculateResult(int level);

        protected abstract int CalculateUpgradePrice(int level);
    }
}