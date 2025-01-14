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

        /// <summary>
        /// variable calculation formula
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        protected abstract void CalculateResult(int level);
    }
}