using System;
using UnityEngine;

public class OrderSubmissionArea : MonoBehaviour
{
    private Item itemDragging;
    private OrderManager orderManager;
    private ItemManager itemManager;
    private bool shouldSubmit = false;  // 新增标志位

    private void Start()
    {
        orderManager = FindFirstObjectByType<OrderManager>();
        itemManager = FindFirstObjectByType<ItemManager>();
    }

    private void Update()
    {
        // 在 Update 中检测输入
        if (itemDragging != null && Input.GetMouseButtonUp(0))
        {
            shouldSubmit = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Item item = other.attachedRigidbody.gameObject.GetComponent<Item>();
        if (item == null || !item.physicsDragger.isDragging) return;

        itemDragging = item;

        //feedback
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetFloat("_PingPongGlowFade", 1);
        itemDragging.transform.Find("Sprite").GetComponent<SpriteRenderer>().SetPropertyBlock(propertyBlock);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.attachedRigidbody.GetComponent<Item>() != itemDragging) return;

        // 检查并消耗标志位
        if (shouldSubmit)
        {
            shouldSubmit = false;  // 重置标志位
            if (orderManager.TrySubmit(itemDragging))
            {
                itemManager.RemoveGivenItem(itemDragging);
                itemDragging = null;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.attachedRigidbody.GetComponent<Item>() == itemDragging)
        {
            itemDragging = null;
            shouldSubmit = false;  // 确保在物品离开时重置标志位
        }
        
        Item item = other.attachedRigidbody.gameObject.GetComponent<Item>();
        //reset feedback
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetFloat("_PingPongGlowFade", 0);
        item.transform.Find("Sprite").GetComponent<SpriteRenderer>().SetPropertyBlock(propertyBlock);
    }
}