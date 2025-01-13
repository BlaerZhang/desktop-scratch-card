using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class TimeTable : MonoBehaviour
{
    [Header("References")]
    private TimeScheduler timeScheduler;
    [SerializeField] private GameObject eventBlockPrefab;
    [SerializeField] private RectTransform contentParent;

    [Header("Settings")]
    [SerializeField] private float previewHours = 3f;
    [SerializeField] private float refreshInterval = 1f;
    
    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = false;

    private Dictionary<string, GameObject> activeEventBlocks = new Dictionary<string, GameObject>();
    private float nextRefreshTime;

    private void Start()
    {
        if (timeScheduler == null)
        {
            timeScheduler = FindObjectOfType<TimeScheduler>();
        }
        RefreshTimeTable();
    }

    private void Update()
    {
        if (Time.time >= nextRefreshTime)
        {
            RefreshTimeTable();
            nextRefreshTime = Time.time + refreshInterval;
        }
    }

    private void RefreshTimeTable()
    {
        DateTime currentTime = DateTime.Now;
        TimeSpan currentTimeOfDay = currentTime.TimeOfDay;
        TimeSpan endPreviewTime = currentTimeOfDay.Add(TimeSpan.FromHours(previewHours));

        // 获取所有事件并检查它们是否应该显示
        var eventsToShow = new List<TimeScheduler.ScheduledEvent>();
        var allEvents = timeScheduler.GetEventsInTimeRange(TimeSpan.Zero, TimeSpan.FromHours(24));
        
        foreach (var evt in allEvents)
        {
            TimeSpan eventStartTime = new TimeSpan(evt.hours, evt.minutes, 0);
            TimeSpan eventEndTime = eventStartTime.Add(TimeSpan.FromSeconds(evt.duration));

            // 计算事件的实际结束时间（考虑跨天的情况）
            if (eventEndTime < eventStartTime)
            {
                eventEndTime = eventEndTime.Add(TimeSpan.FromHours(24));
            }

            bool shouldShow = false;
            
            // 检查事件是否在预览时间范围内
            if (eventStartTime >= currentTimeOfDay && eventStartTime <= endPreviewTime)
            {
                shouldShow = true;
            }
            // 检查事件是否正在进行中
            else if (evt.isActive)
            {
                shouldShow = true;
            }
            // 检查跨天的情况
            else if (endPreviewTime < currentTimeOfDay && 
                    (eventStartTime >= currentTimeOfDay || eventStartTime <= endPreviewTime))
            {
                shouldShow = true;
            }

            if (shouldShow && !evt.hideInTimeTable)
            {
                if (showDebugLogs)
                {
                    Debug.Log($"显示事件: {evt.eventName}, 开始时间: {eventStartTime}, 持续时间: {evt.duration}秒, 是否活跃: {evt.isActive}");
                }
                eventsToShow.Add(evt);
            }
        }

        // 更新UI显示
        HashSet<string> eventsToKeep = new HashSet<string>();

        foreach (var evt in eventsToShow)
        {
            string eventKey = GetEventKey(evt);
            eventsToKeep.Add(eventKey);

            if (!activeEventBlocks.ContainsKey(eventKey))
            {
                CreateEventBlock(evt, currentTime);
                if (showDebugLogs)
                {
                    Debug.Log($"创建事件块: {evt.eventName}");
                }
            }
            else
            {
                UpdateEventBlock(activeEventBlocks[eventKey], evt, currentTime);
                if (showDebugLogs)
                {
                    Debug.Log($"更新事件块: {evt.eventName}");
                }
            }
        }

        // 移除不需要的事件块
        List<string> eventKeysToRemove = new List<string>();
        foreach (var kvp in activeEventBlocks)
        {
            if (!eventsToKeep.Contains(kvp.Key))
            {
                if (showDebugLogs)
                {
                    Debug.Log($"移除事件块: {kvp.Key}");
                }
                eventKeysToRemove.Add(kvp.Key);
                Destroy(kvp.Value);
            }
        }

        foreach (var key in eventKeysToRemove)
        {
            activeEventBlocks.Remove(key);
        }

        SortEventBlocks();
    }

    private void CreateEventBlock(TimeScheduler.ScheduledEvent evt, DateTime currentTime)
    {
        GameObject blockObj = Instantiate(eventBlockPrefab, contentParent);
        string eventKey = GetEventKey(evt);
        activeEventBlocks.Add(eventKey, blockObj);
        UpdateEventBlock(blockObj, evt, currentTime);
    }

    private void UpdateEventBlock(GameObject blockObj, TimeScheduler.ScheduledEvent evt, DateTime currentTime)
    {
        DateTime eventTime = currentTime.Date.Add(new TimeSpan(evt.hours, evt.minutes, 0));
        
        // 处理跨天的情况
        if (eventTime < currentTime && !evt.isActive)
        {
            eventTime = eventTime.AddDays(1);
        }

        TimeSpan timeUntilEvent = eventTime - currentTime;
        if (evt.isActive)
        {
            // 如果事件正在进行中，显示剩余时间而不是开始时间
            DateTime eventEndTime = eventTime.Add(TimeSpan.FromSeconds(evt.duration));
            timeUntilEvent = eventEndTime - currentTime;
        }

        var eventNameText = blockObj.transform.Find("Event Name")?.GetComponent<TMP_Text>();
        var timeText = blockObj.transform.Find("Time Info")?.GetComponent<TMP_Text>();
        var statusIndicator = blockObj.transform.Find("Status Indicator")?.GetComponent<Image>();

        if (eventNameText != null)
        {
            eventNameText.text = evt.eventName;
        }

        if (timeText != null)
        {
            string timeDisplay = $"{evt.hours:D2}:{evt.minutes:D2}";
            string countdownDisplay = evt.isActive ? 
                $"Ends in: {FormatTimeSpan(timeUntilEvent)}" : 
                $"Starts in: {FormatTimeSpan(timeUntilEvent)}";
            // timeText.text = $"{timeDisplay}\n{countdownDisplay}
            timeText.text = $"{timeDisplay}";
        }

        if (statusIndicator != null)
        {
            statusIndicator.color = evt.isActive ? Color.green : Color.gray;
        }
    }

    private string FormatTimeSpan(TimeSpan timeSpan)
    {
        if (timeSpan.TotalHours >= 1)
        {
            return $"{(int)timeSpan.TotalHours}h {timeSpan.Minutes}m";
        }
        else if (timeSpan.TotalMinutes >= 1)
        {
            return $"{timeSpan.Minutes}m {timeSpan.Seconds}s";
        }
        else
        {
            return $"{timeSpan.Seconds}s";
        }
    }

    private void SortEventBlocks()
    {
        var blocks = new List<Transform>();
        for (int i = 0; i < contentParent.childCount; i++)
        {
            blocks.Add(contentParent.GetChild(i));
        }

        blocks.Sort((a, b) =>
        {
            var timeA = GetEventTimeFromBlock(a.gameObject);
            var timeB = GetEventTimeFromBlock(b.gameObject);
            
            // 如果某个事件正在进行中，将其放在列表最前面
            bool isActiveA = IsEventBlockActive(a.gameObject);
            bool isActiveB = IsEventBlockActive(b.gameObject);
            
            if (isActiveA && !isActiveB) return -1;
            if (!isActiveA && isActiveB) return 1;
            
            return timeA.CompareTo(timeB);
        });

        for (int i = 0; i < blocks.Count; i++)
        {
            blocks[i].SetSiblingIndex(i);
        }
    }

    private bool IsEventBlockActive(GameObject block)
    {
        foreach (var kvp in activeEventBlocks)
        {
            if (kvp.Value == block)
            {
                // 从事件名中判断是否为活跃事件
                var statusIndicator = block.transform.Find("StatusIndicator")?.GetComponent<Image>();
                return statusIndicator != null && statusIndicator.color == Color.green;
            }
        }
        return false;
    }

    private int GetEventTimeFromBlock(GameObject block)
    {
        foreach (var kvp in activeEventBlocks)
        {
            if (kvp.Value == block)
            {
                string[] timeParts = kvp.Key.Split('_');
                if (timeParts.Length >= 2)
                {
                    string[] timeComponents = timeParts[1].Split(':');
                    if (timeComponents.Length == 2)
                    {
                        int hours = int.Parse(timeComponents[0]);
                        int minutes = int.Parse(timeComponents[1]);
                        return hours * 60 + minutes;
                    }
                }
            }
        }
        return 0;
    }

    private string GetEventKey(TimeScheduler.ScheduledEvent evt)
    {
        return $"{evt.eventName}_{evt.hours:D2}:{evt.minutes:D2}";
    }
}