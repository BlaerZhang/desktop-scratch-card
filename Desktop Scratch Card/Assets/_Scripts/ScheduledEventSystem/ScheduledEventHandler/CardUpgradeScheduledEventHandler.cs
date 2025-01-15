using UnityEngine;

public class CardUpgradeScheduledEventHandler : BaseScheduledEventHandler
{
    public GameObject cardUpgradeUI;
    public override void OnEventStart()
    {
        cardUpgradeUI.SetActive(true);
    }
    
    public override void OnEventEnd()
    {
        cardUpgradeUI.SetActive(false);
    }
}
