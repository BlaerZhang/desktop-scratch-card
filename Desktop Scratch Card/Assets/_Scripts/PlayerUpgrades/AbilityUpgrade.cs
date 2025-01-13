using UnityEngine;

namespace _Scripts.PlayerUpgrades
{
    // TODO: upgrade shared data
    public class AbilityUpgrade : BaseUpgrade
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
        private float _result;

        /// <summary>
        /// variable calculation formula
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        protected virtual float CalculateResult(int level)
        {
            // add formula for each specific upgrade

            return _result;
        }

        /// <summary>
        /// set the value of the variable as the calculation result
        /// </summary>
        public virtual void ApplyEffect()
        {

        }
    }
}