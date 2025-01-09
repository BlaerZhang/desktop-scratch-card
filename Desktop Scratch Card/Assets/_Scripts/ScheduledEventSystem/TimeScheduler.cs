using UnityEngine;
using System;
using System.Collections.Generic;

public class TimeScheduler : MonoBehaviour
{
    // 基础的计划事件数据结构，用于表示具体的时间点事件
    [System.Serializable]
    public class ScheduledEvent
    {
        public string eventName;        // 事件名称
        public int hours;              // 事件触发的小时（0-23）
        public int minutes;            // 事件触发的分钟（0-59）
        public float duration;         // 事件持续时间（秒）
        public BaseScheduledEventHandler eventHandler;  // 事件处理器
        public bool isActive;          // 事件是否正在进行中
    }

    // 周期性事件的配置结构，用于初始化时生成具体的计划事件
    [System.Serializable]
    public class PeriodicEventConfig
    {
        public string eventName;       // 周期事件名称
        public int periodMinutes;      // 周期（分钟）
        public float duration;         // 每次触发的持续时间
        public BaseScheduledEventHandler eventHandler;  // 事件处理器
    }

    // 在Inspector中配置的事件列表
    [SerializeField] 
    private List<ScheduledEvent> scheduledEvents = new List<ScheduledEvent>();

    // 在Inspector中配置的周期性事件
    [SerializeField] 
    private List<PeriodicEventConfig> periodicEventConfigs = new List<PeriodicEventConfig>();

    // 当前活跃的事件列表，用于追踪正在进行的事件
    private HashSet<ScheduledEvent> activeEvents = new HashSet<ScheduledEvent>();

    private void Start()
    {
        InitializeEventSystem();
    }

    private void InitializeEventSystem()
    {
        // 展开周期性事件为具体的计划事件
        ExpandPeriodicEvents();
        // 对所有事件按时间排序
        SortScheduledEvents();
        // 初始化当前状态
        CheckCurrentTimeEvents();
    }

    // 将周期性事件配置展开为具体的计划事件
    private void ExpandPeriodicEvents()
    {
        foreach (var periodicConfig in periodicEventConfigs)
        {
            // 计算一天内这个周期性事件会发生多少次
            int eventsPerDay = 1440 / periodicConfig.periodMinutes; // 1440 = 24小时 * 60分钟

            for (int i = 0; i < eventsPerDay; i++)
            {
                // 计算每次事件的具体时间点
                int totalMinutes = i * periodicConfig.periodMinutes;
                int eventHours = totalMinutes / 60;
                int eventMinutes = totalMinutes % 60;

                // 创建具体的计划事件
                var scheduledEvent = new ScheduledEvent
                {
                    eventName = $"{periodicConfig.eventName}_{eventHours:D2}:{eventMinutes:D2}",
                    hours = eventHours,
                    minutes = eventMinutes,
                    duration = periodicConfig.duration,
                    eventHandler = periodicConfig.eventHandler,
                    isActive = false
                };

                scheduledEvents.Add(scheduledEvent);
            }
        }
    }

    // 对事件列表按时间排序
    private void SortScheduledEvents()
    {
        scheduledEvents.Sort((a, b) =>
        {
            int timeA = a.hours * 60 + a.minutes;
            int timeB = b.hours * 60 + b.minutes;
            return timeA.CompareTo(timeB);
        });
    }

    // 检查当前是否有事件需要启动或结束
    private void Update()
    {
        DateTime currentTime = DateTime.Now;
        TimeSpan currentTimeOfDay = currentTime.TimeOfDay;

        // 检查所有计划事件
        foreach (var evt in scheduledEvents)
        {
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

    // 检查游戏开始时是否有事件应该处于活跃状态
    private void CheckCurrentTimeEvents()
    {
        DateTime currentTime = DateTime.Now;
        TimeSpan currentTimeOfDay = currentTime.TimeOfDay;

        foreach (var evt in scheduledEvents)
        {
            // 计算事件的开始和结束时间
            TimeSpan eventStartTime = new TimeSpan(evt.hours, evt.minutes, 0);
            TimeSpan eventEndTime = eventStartTime.Add(TimeSpan.FromSeconds(evt.duration));

            // 如果当前时间在事件的时间范围内，启动该事件
            if (currentTimeOfDay >= eventStartTime && currentTimeOfDay < eventEndTime)
            {
                evt.isActive = true;
                activeEvents.Add(evt);
                evt.eventHandler?.OnEventStart();
            }
        }
    }

    // 检查事件是否应该开始
    private bool ShouldStartEvent(TimeSpan currentTime, ScheduledEvent evt)
    {
        TimeSpan eventTime = new TimeSpan(evt.hours, evt.minutes, 0);
        var timeDifference = (currentTime - eventTime).TotalSeconds;
        return timeDifference >= 0 && timeDifference <= 1.0f;
    }

    // 检查事件是否应该结束
    private bool ShouldEndEvent(DateTime currentTime, ScheduledEvent evt)
    {
        TimeSpan eventStartTime = new TimeSpan(evt.hours, evt.minutes, 0);
        DateTime eventStartDateTime = currentTime.Date + eventStartTime;
        return (currentTime - eventStartDateTime).TotalSeconds >= evt.duration;
    }

    // 启动事件
    private void StartEvent(ScheduledEvent evt)
    {
        evt.isActive = true;
        activeEvents.Add(evt);
        Debug.Log($"开始事件: {evt.eventName} at {DateTime.Now}");
        evt.eventHandler?.OnEventStart();
    }

    // 结束事件
    private void EndEvent(ScheduledEvent evt)
    {
        evt.isActive = false;
        activeEvents.Remove(evt);
        Debug.Log($"结束事件: {evt.eventName} at {DateTime.Now}");
        evt.eventHandler?.OnEventEnd();
    }

    // 手动添加一次性事件的方法
    public void AddScheduledEvent(string name, int hours, int minutes, 
        float duration, BaseScheduledEventHandler handler)
    {
        var newEvent = new ScheduledEvent
        {
            eventName = name,
            hours = hours,
            minutes = minutes,
            duration = duration,
            eventHandler = handler,
            isActive = false
        };

        scheduledEvents.Add(newEvent);
        SortScheduledEvents();
    }

    // 手动添加周期性事件的方法
    public void AddPeriodicEvent(string name, int periodMinutes, 
        float duration, BaseScheduledEventHandler handler)
    {
        var newConfig = new PeriodicEventConfig
        {
            eventName = name,
            periodMinutes = periodMinutes,
            duration = duration,
            eventHandler = handler
        };

        periodicEventConfigs.Add(newConfig);
        // 展开新添加的周期性事件
        ExpandPeriodicEvents();
        SortScheduledEvents();
    }

    // 用于编辑器调试：获取特定时间段内的所有事件
    public List<ScheduledEvent> GetEventsInTimeRange(TimeSpan startTime, TimeSpan endTime)
    {
        return scheduledEvents.FindAll(evt => 
        {
            var eventTime = new TimeSpan(evt.hours, evt.minutes, 0);
            return eventTime >= startTime && eventTime <= endTime;
        });
    }
}