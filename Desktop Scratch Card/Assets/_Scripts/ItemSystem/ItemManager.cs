using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.ScratchCardSystem;
using _Scripts.ScratchCardSystem.GridSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager : SerializedMonoBehaviour
{
    public List<Item> playerItemList;
    
    public Dictionary<GridItemType, int> playerItemStats;
    
    [DisableInPlayMode] public Dictionary<GridItemType, GameObject> itemPrefabDict;

    public static Action onItemUpdated;

    private void OnEnable()
    {
        ScratchCardManager.onScratchCardSubmitted += AddItems;
        OrderManager.onSubmissionCancelled += AddItems;
        CalculateItems();
    }

    private void OnDisable()
    {
        ScratchCardManager.onScratchCardSubmitted -= AddItems;
        OrderManager.onSubmissionCancelled -= AddItems;
    }

    private void CalculateItems()
    {
        // Init if not set right
        if (playerItemStats == null) 
            playerItemStats = new Dictionary<GridItemType, int>();

        // 获取所有类型
        var itemTypes = Enum.GetValues(typeof(GridItemType));

        // 确保字典包含所有类型
        foreach (GridItemType type in itemTypes)
        {
            if (!playerItemStats.ContainsKey(type))
            {
                playerItemStats.Add(type, 0);
            }
        }

        // 为每个类型计算数量
        foreach (GridItemType type in itemTypes)
        {
            playerItemStats[type] = playerItemList.Count(item => item.itemType == type);
        }
        
        onItemUpdated?.Invoke();
    }

    private void AddItems(ScratchCard card)
    {
        var items = card.gridData.items;
        foreach (var item in items)
        {
            AddItem(item.type, item.itemCount);
        }
    }
    
    private void AddItems(List<Vector2Int> items)
    {
        foreach (var item in items)
        {
            AddItem((GridItemType)item.x, item.y);
        }
    }

    /// <summary>
    /// Use this function to add and spawn items when getting them from scratch card
    /// </summary>
    /// <param name="type"></param>
    /// <param name="quantity">default set to 1</param>
    public void AddItem(GridItemType type, int quantity = 1)
    {
        for (int i = 0; i < quantity; i++)
        {
            GameObject newItemObject = SpawnItem(type);
            Item newItem = newItemObject.GetComponent<Item>();
            playerItemList.Add(newItem);
        }
        
        CalculateItems();
    }
    
    private GameObject SpawnItem(GridItemType type)
    {
        Vector2 spawnPos = new Vector2(Random.Range(-4f, 4f), 6); //TODO concise spawn area
        GameObject newItemObject = Instantiate(itemPrefabDict[type], spawnPos, Quaternion.identity);

        return newItemObject;
    }

    /// <summary>
    /// Use this function to remove items when spending them. Doesn't check if fund is sufficient. Please check before Removing.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="quantity">default set to 1</param>
    public void RemoveItemByItemType(GridItemType type, int quantity = 1)
    {
        for (int i = 0; i < quantity; i++)
        {
            // 从列表开头找到第一个匹配的项
            var itemToRemove = playerItemList.FirstOrDefault(item => item.itemType == type);
        
            // 如果找不到匹配的项就退出
            if (itemToRemove == null) break;
        
            // 移除找到的项
            playerItemList.Remove(itemToRemove);
            
            itemToRemove.OnItemRemoved();
        }
    
        // 更新统计
        CalculateItems();
    }

    public void RemoveGivenItem(Item item)
    {
        playerItemList.Remove(item);
        
        item.OnItemRemoved();
        
        CalculateItems();
    }
}
