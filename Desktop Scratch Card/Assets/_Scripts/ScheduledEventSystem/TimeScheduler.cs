using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeScheduler : MonoBehaviour
{
    [System.Serializable]
    public class ScheduledEvent
    {
        public string eventName;        // 事件名称
        [Tooltip("The target time is formatted as \"Hours:Minutes\"")]
        public int hours;               // 开始时间-小时
        [Tooltip("The target time is formatted as \"Hours:Minutes\"")]
        public int minutes;             // 开始时间-分钟
        [Tooltip("In Seconds")]
        public float duration;          // 持续时间（秒）
        public bool isActive;           // 事件是否正在进行
        public BaseScheduledEventHandler scheduledEventHandler;  // 引用事件处理脚本
    }

    [SerializeField]
    private List<ScheduledEvent> scheduledEvents = new List<ScheduledEvent>();

    private void Update()
    {
        DateTime currentTime = DateTime.Now;
        TimeSpan currentTimeOfDay = currentTime.TimeOfDay;

        foreach (var evt in scheduledEvents)
        {
            // 确保事件处理器存在
            if (evt.scheduledEventHandler == null) continue;

            // 检查是否应该开始事件
            if (!evt.isActive && ShouldStartEvent(currentTimeOfDay, evt))
            {
                StartEvent(evt);
            }
            // 检查是否应该结束事件
            else if (evt.isActive && ShouldEndEvent(currentTime, evt))
            {
                EndEvent(evt);
            }
        }
    }

    private bool ShouldStartEvent(TimeSpan currentTime, ScheduledEvent evt)
    {
        TimeSpan eventTime = new TimeSpan(evt.hours, evt.minutes, 0);
        var timeDifference = (currentTime - eventTime).TotalSeconds;
        return timeDifference >= 0 && timeDifference <= 1.0f;
    }

    private bool ShouldEndEvent(DateTime currentTime, ScheduledEvent evt)
    {
        TimeSpan eventTime = new TimeSpan(evt.hours, evt.minutes, 0);
        DateTime startDateTime = currentTime.Date + eventTime;
        return (currentTime - startDateTime).TotalSeconds >= evt.duration;
    }

    private void StartEvent(ScheduledEvent evt)
    {
        evt.isActive = true;
        evt.scheduledEventHandler.OnEventStart();
    }

    private void EndEvent(ScheduledEvent evt)
    {
        evt.isActive = false;
        evt.scheduledEventHandler.OnEventEnd();
    }
    
    public void AddScheduledEvent(string name, int hours, int minutes, float duration, BaseScheduledEventHandler scheduledEventHandler)
    {
        var newEvent = new ScheduledEvent
        {
            eventName = name,
            hours = hours,
            minutes = minutes,
            duration = duration,
            isActive = false,
            scheduledEventHandler = scheduledEventHandler
        };
        scheduledEvents.Add(newEvent);
    }
}