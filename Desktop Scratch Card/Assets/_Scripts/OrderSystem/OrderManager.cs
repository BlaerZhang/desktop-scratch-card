using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class OrderManager : SerializedMonoBehaviour
{
    [System.Serializable]
    public class Order
    {
        public int orderIndex;
        public int requiredTypeCount;
        public SerializedDictionary<ItemType, Vector2> orderDetail;
        public int reward;
        public bool isActive;

        public Order(int orderIndex, int typeCount, int minRequiredAmount, int maxRequiredAmount)
        {
            requiredTypeCount = typeCount;
            orderDetail = new SerializedDictionary<ItemType, Vector2>();
            ItemType[] requiredTypes = EnumExtensions.GetRandomUniqueValues<ItemType>(typeCount);
            foreach (var type in requiredTypes)
            {
                int requiredAmount = Random.Range(minRequiredAmount, maxRequiredAmount);
                orderDetail.Add(type, new Vector2(0, requiredAmount));
                reward += requiredAmount * 10; //TODO: reward per item of type to design
            }

            isActive = false;
        }
    }
    
    [System.Serializable]
    public class OrderGenerationConfig
    {
        public int configIndex;
        public int minTypeCount;
        public int maxTypeCount;
        public int minRequiredAmount;
        public int maxRequiredAmount;
    }
    
    [Title("Setup")]
    [DisableInPlayMode] public List<OrderGenerationConfig> orderConfigs;
    
    [Title("Submitting")] 
    public List<Order> orderList;
    
    [Title("State Control")] 
    public bool isSubmitting = false;
    public static Action onSubmissionStart;
    public static Action onSubmissionEnd;

    [Title("UI")] 
    public GameObject orderSubmissionArea;
    [FormerlySerializedAs("buttonPrototype")] public Button originalOrderButton;
    public List<Button> orderButtons;
    
    void Start()
    {
        if (orderConfigs.Count <= 0)
        {
            Debug.LogError("Order Configuration of Order Manager is not set up yet");
            return;
        }
        InitOrders();
        InitUI();
    }

    private void InitUI()
    {
        orderButtons.Add(originalOrderButton);
        for (int i = 1; i < orderConfigs.Count; i++)
        {
            orderButtons.Add(Instantiate(originalOrderButton, originalOrderButton.transform.parent));
        }
        
        UpdateUI();
    }

    private void InitOrders()
    {
        foreach (var config in orderConfigs)
        {
            orderList.Add(new Order(config.configIndex, Random.Range(config.minTypeCount, config.maxTypeCount),
                config.minRequiredAmount, config.maxRequiredAmount));
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < orderButtons.Count; i++)
        {
            string requirementText = "<u>";
            foreach (var requirement in orderList[i].orderDetail)
            {
                requirementText += $"{requirement.Key.ToString()}\t{requirement.Value.x}/{requirement.Value.y}\n";
            }

            requirementText += $"</u>\nReward\t${orderList[i].reward}";

            orderButtons[i].GetComponentInChildren<TMP_Text>().text = requirementText;
        }
    }
    
    
}
