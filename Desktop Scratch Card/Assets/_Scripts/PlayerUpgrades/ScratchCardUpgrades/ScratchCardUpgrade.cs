using _Scripts.ScratchCardSystem;
using DG.Tweening;

namespace _Scripts.PlayerUpgrades.ScratchCardUpgrades
{
    public abstract class ScratchCardUpgrade : BaseUpgrade
    {
        public float winEffectDuration = 0.5f;
        public int Level
        {
            get => level;
            set
            {
                level = value;
            }
        }

        public abstract bool CheckWin(ScratchCard card, out Sequence winEffect);
    }
}