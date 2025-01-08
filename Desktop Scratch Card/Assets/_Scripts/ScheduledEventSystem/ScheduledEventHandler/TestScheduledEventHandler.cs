using UnityEngine;

public class TestScheduledEventHandler : BaseScheduledEventHandler
{
    public override void OnEventStart()
    {
        Debug.Log("Test Start");
    }
    
    public override void OnEventEnd()
    {
        Debug.Log("Test End");
    }
}