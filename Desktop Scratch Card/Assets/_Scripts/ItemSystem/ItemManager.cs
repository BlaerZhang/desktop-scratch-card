using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager : SerializedMonoBehaviour
{
    public List<Item> playerItemList;
    
    public Dictionary<ItemType, int> playerItemStats;
    
    [DisableInPlayMode] public Dictionary<ItemType, GameObject> itemPrefabDict;

    private void CalculateItems()
    {
        // Init if not set right
        if (playerItemStats == null) 
            playerItemStats = new Dictionary<ItemType, int>();

        // 获取所有类型
        var itemTypes = Enum.GetValues(typeof(ItemType));

        // 确保字典包含所有类型
        foreach (ItemType type in itemTypes)
        {
            if (!playerItemStats.ContainsKey(type))
            {
                playerItemStats.Add(type, 0);
            }
        }

        // 为每个类型计算数量
        foreach (ItemType type in itemTypes)
        {
            playerItemStats[type] = playerItemList.Count(item => item.itemType == type);
        }
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) AddItem(ItemType.type1);
        if(Input.GetKeyDown(KeyCode.Alpha2)) AddItem(ItemType.type2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) AddItem(ItemType.type1, 3);
    }

    /// <summary>
    /// Use this function to add and spawn items when getting them from scratch card
    /// </summary>
    /// <param name="type"></param>
    /// <param name="quantity">default set to 1</param>
    public void AddItem(ItemType type, int quantity = 1)
    {
        for (int i = 0; i < quantity; i++)
        {
            GameObject newItemObject = SpawnItem(type);
            Item newItem = newItemObject.GetComponent<Item>();
            playerItemList.Add(newItem);
        }
        
        CalculateItems();
    }
    
    private GameObject SpawnItem(ItemType type)
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
    public void RemoveItem(ItemType type, int quantity = 1)
    {
        for (int i = 0; i < quantity; i++)
        {
            // 从列表开头找到第一个匹配的项
            var itemToRemove = playerItemList.FirstOrDefault(item => item.itemType == type);
        
            // 如果找不到匹配的项就退出
            if (itemToRemove == null) break;
        
            // 移除找到的项
            playerItemList.Remove(itemToRemove);
        }
    
        // 更新统计
        CalculateItems();
    }
}
