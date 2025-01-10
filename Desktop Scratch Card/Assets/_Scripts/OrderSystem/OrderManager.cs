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
        public SerializedDictionary<GridItemType, Vector2Int> orderDetail;
        public int reward;
        public bool isActive;

        public Order(int orderIndex, int typeCount, int minRequiredAmount, int maxRequiredAmount)
        {
            requiredTypeCount = typeCount;
            orderDetail = new SerializedDictionary<GridItemType, Vector2Int>();
            GridItemType[] requiredTypes = EnumExtensions.GetRandomUniqueValues<GridItemType>(typeCount);
            foreach (var type in requiredTypes)
            {
                int requiredAmount = Random.Range(minRequiredAmount, maxRequiredAmount);
                orderDetail.Add(type, new Vector2Int(0, requiredAmount));
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
    public int currentDealingOrderIndex;
    public static Action onSubmissionStart;
    public static Action<List<Vector2Int>> onSubmissionCancelled;

    [Title("UI")] 
    public GameObject orderSubmissionArea;
    public Button originalOrderButton;
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
        originalOrderButton.onClick.AddListener(() => StartSubmission(0));
        
        for (int i = 1; i < orderList.Count; i++)
        {
            Button newButton = Instantiate(originalOrderButton, originalOrderButton.transform.parent);
            var index = i;
            newButton.onClick.AddListener(() => StartSubmission(index));
            orderButtons.Add(newButton);
        }
        
        UpdateUI();
    }

    private void InitOrders()
    {
        foreach (var config in orderConfigs)
        {
            orderList.Add(GenerateOrderFromConfig(config));
        }
    }

    private Order GenerateOrderFromConfig(OrderGenerationConfig config)
    {
        Order newOrder = new Order(config.configIndex, Random.Range(config.minTypeCount, config.maxTypeCount),
            config.minRequiredAmount, config.maxRequiredAmount);
        return newOrder;
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
    
    public bool TrySubmit(Item item)
    {
        bool canSubmit;
        Order currentOrder = orderList[currentDealingOrderIndex];
        canSubmit = currentOrder.orderDetail.ContainsKey(item.itemType);

        if (canSubmit) currentOrder.orderDetail[item.itemType] += Vector2Int.right; //item count +1
        
        return canSubmit;
    }

    public void StartSubmission(int orderIndex)
    {
        currentDealingOrderIndex = orderIndex;
        isSubmitting = true;
        onSubmissionStart?.Invoke();
        
        //set button
        foreach (var button in orderButtons)
        {
            if (orderIndex != orderButtons.IndexOf(button)) button.interactable = false;
        }
        
        //Show Submission Area is now set by button
    }

    public void TryFulfillOrder()
    {
        bool canFulfill = true;
        Order currentOrder = orderList[currentDealingOrderIndex];
        foreach (var orderKvp in currentOrder.orderDetail)
        {
            if (orderKvp.Value.x < orderKvp.Value.y) canFulfill = false;
        }

        if (canFulfill)
        {
            //TODO: Fulfill & Pay Money!!!
            orderList[currentDealingOrderIndex] = GenerateOrderFromConfig(orderConfigs[currentDealingOrderIndex]);
            UpdateUI();
        }
        
        else CancelSubmission();
    }

    public void CancelSubmission()
    {
        Order currentOrder = orderList[currentDealingOrderIndex];
        List<Vector2Int> returningItems = new List<Vector2Int>();
        foreach (var orderKvp in currentOrder.orderDetail)
        {
            returningItems.Add(new Vector2Int((int)orderKvp.Key, orderKvp.Value.x));
        }
        
        onSubmissionCancelled?.Invoke(returningItems);
        
        //Reset Buttons
        foreach (var button in orderButtons) button.interactable = true;
    }
    
}
