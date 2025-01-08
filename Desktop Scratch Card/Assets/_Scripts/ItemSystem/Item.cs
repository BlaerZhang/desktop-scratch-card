using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(PhysicsDragger))]
public class Item : MonoBehaviour
{
    public ItemType itemType;
    public float itemSize;
    public float itemWeight;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        
    }
    
    private void OnMouseOver()
    {
        //Show tooltip
    }
}
