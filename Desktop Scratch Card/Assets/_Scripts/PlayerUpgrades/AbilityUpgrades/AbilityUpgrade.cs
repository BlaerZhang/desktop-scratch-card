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
                ApplyResult(CalculateResult(level));
            }
        }

        [SerializeField] protected int initialPrice;
        public int Price => CalculateUpgradePrice(level);

        /// <summary>
        /// variable calculation formula
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public abstract float CalculateResult(int level);

        protected abstract void ApplyResult(float result);

        protected abstract int CalculateUpgradePrice(int level);
    }
}