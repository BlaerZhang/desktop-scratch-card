using UnityEngine;

namespace _Scripts.PlayerUpgrades
{
    public abstract class BaseUpgrade : MonoBehaviour
    {
        public string id;
        protected int level;
        [TextArea]
        public string description;
    }
}