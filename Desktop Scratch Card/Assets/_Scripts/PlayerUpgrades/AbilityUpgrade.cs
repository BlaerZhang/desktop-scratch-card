using _Scripts.General;
using UnityEngine;

namespace _Scripts.PlayerUpgrades
{
    // TODO: upgrade shared data
    public abstract class AbilityUpgrade : BaseUpgrade
    {
        private int _level = 0;
        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                CalculateResult(_level);
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