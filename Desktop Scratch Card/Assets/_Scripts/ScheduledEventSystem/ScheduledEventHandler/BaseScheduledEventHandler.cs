using UnityEngine;

public abstract class BaseScheduledEventHandler : MonoBehaviour
{
    // 事件开始时的处理逻辑
    public abstract void OnEventStart();
    
    // 事件结束时的处理逻辑
    public abstract void OnEventEnd();
}