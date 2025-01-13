using UnityEngine;

namespace _Scripts.PlayerUpgrades
{
    public abstract class BaseUpgrade : MonoBehaviour
    {
        public string id;
        public string name;
        [TextArea]
        public string description;
    }
}