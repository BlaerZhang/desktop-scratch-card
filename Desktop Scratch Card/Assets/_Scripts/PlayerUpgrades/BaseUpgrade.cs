using _Scripts.GridSystem;
using UnityEngine;

namespace _Scripts.PlayerUpgrades
{
    public abstract class BaseUpgrade : MonoBehaviour
    {
        public string id { get; }
        public string name { get; }
        public string description { get; }

        protected virtual void ApplyEffect(){}
        protected virtual void ApplyEffect(ScratchCard card){}
    }
}