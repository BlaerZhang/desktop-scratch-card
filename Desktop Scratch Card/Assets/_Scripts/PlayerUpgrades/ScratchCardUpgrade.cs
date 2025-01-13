using _Scripts.GridSystem;
using UnityEngine;

namespace _Scripts.PlayerUpgrades
{
    public class ScratchCardUpgrade : BaseUpgrade
    {
        void OnEnable()
        {
            ScratchCardManager.onScratchCardSubmitted += ApplyEffect;
        }

        void OnDisable()
        {
            ScratchCardManager.onScratchCardSubmitted -= ApplyEffect;
        }
    }
}