using _Scripts.GridSystem;
using UnityEngine;
using DG.Tweening;

namespace _Scripts.PlayerUpgrades
{
    public class ScratchCardUpgrade : BaseUpgrade
    {
        // void OnEnable()
        // {
        //     ScratchCardManager.onScratchCardSubmitted += ApplyEffect;
        // }
        //
        // void OnDisable()
        // {
        //     ScratchCardManager.onScratchCardSubmitted -= ApplyEffect;
        // }

        public virtual bool CheckCondition()
        {
            return false;
        }

        // private void Tween ApplyEffect()
        // {
        //     if (!CheckCondition()) return;
        //     Tween d;
        //     return d;
        // }
    }
}