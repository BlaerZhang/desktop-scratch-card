using System;
using UnityEngine;

public class OrderSubmissionArea : MonoBehaviour
{
    private Item itemDragging;
    private OrderManager orderManager;
    private ItemManager itemManager;

    private void Start()
    {
        orderManager = FindFirstObjectByType<OrderManager>();
        itemManager = FindFirstObjectByType<ItemManager>();
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

        if (Input.GetMouseButtonUp(0))
        {
            if (orderManager.TrySubmit(itemDragging))
            {
                
                itemManager.RemoveGivenItem(itemDragging);
                itemDragging = null;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.attachedRigidbody.GetComponent<Item>() == itemDragging) itemDragging = null;
        
        Item item = other.attachedRigidbody.gameObject.GetComponent<Item>();
        //reset feedback
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetFloat("_PingPongGlowFade", 0);
        item.transform.Find("Sprite").GetComponent<SpriteRenderer>().SetPropertyBlock(propertyBlock);
    }
}
