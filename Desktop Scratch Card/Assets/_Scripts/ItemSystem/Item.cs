using System;
using _Scripts.ScratchCardSystem.GridSystem;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(PhysicsDragger))]
public class Item : MonoBehaviour
{
    public GridItemType itemType;
    public float itemSize;
    public float itemWeight;
    public PhysicsDragger physicsDragger;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        physicsDragger = GetComponent<PhysicsDragger>();
    }

    public void OnItemRemoved()
    {
        Destroy(GetComponent<Rigidbody2D>()); //Stop physics simulation
        transform.DOScale(0, 0.5f).SetEase(Ease.InElastic).OnComplete((() => Destroy(gameObject))); //Destroy self
    }
    
    private void OnMouseOver()
    {
        //Show tooltip
    }
}
