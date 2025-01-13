using UnityEngine;

namespace _Scripts.PlayerUpgrades
{
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

        protected virtual float CalculateResult(int level)
        {
            // add formula for each specific upgrade
            return level;
        }
    }
}