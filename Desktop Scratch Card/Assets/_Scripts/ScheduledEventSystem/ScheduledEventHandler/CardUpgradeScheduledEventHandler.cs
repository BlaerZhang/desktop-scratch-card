using System.Collections.Generic;
using UnityEngine;

public class CardUpgradeScheduledEventHandler : BaseScheduledEventHandler
{
    public List<GameObject> cardUpgradeUI;
    public override void OnEventStart()
    {
        cardUpgradeUI[Random.Range(0,cardUpgradeUI.Count)].SetActive(true);
    }
    
    public override void OnEventEnd()
    {
        foreach (var gameObject in cardUpgradeUI) gameObject.SetActive(false);
    }
}
