using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
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
        public SerializedDictionary<GridItemType, Vector2Int> orderDetail;
        public int reward;
        public bool isActive;

        /// <summary>
        /// Constructor of Order
        /// </summary>
        /// <param name="orderIndex">Must match the index of configs and buttons</param>
        /// <param name="orderRequirements">Type required, Quantity of the type required</param>
        public Order(int orderIndex, Dictionary<GridItemType,int> orderRequirements)
        {
            this.orderIndex = orderIndex;
            orderDetail = new SerializedDictionary<GridItemType, Vector2Int>();

            int basicReward = 0;
            foreach (var requirement in orderRequirements)
            {
                orderDetail.Add(requirement.Key, new Vector2Int(0, requirement.Value));
                basicReward += requirement.Value * 10; //TODO: reward per item of type to design
            }
            // print($"Basic Reward: {basicReward}");
            // print($"Quantity F: {0.9f + basicReward / 100f}");
            // print($"Type F: {0.9f + orderRequirements.Count / 10f}");

            reward = Mathf.RoundToInt(basicReward * (0.9f + basicReward / 100f) * (0.9f + orderRequirements.Count / 10f));

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
    // public GameObject orderSubmissionArea;
    [SerializeField] private Button originalOrderButton;
    [SerializeField] private List<Button> orderButtons;
    
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
        Dictionary<GridItemType, int> requirements = new Dictionary<GridItemType, int>();

        GridItemType[] requiredTypes =
            EnumExtensions.GetRandomUniqueValues<GridItemType>(Random.Range(config.minTypeCount, config.maxTypeCount));
        
        foreach (var type in requiredTypes)
        {
            int requiredAmount = Random.Range(config.minRequiredAmount, config.maxRequiredAmount);
            requirements.Add(type, requiredAmount);
        }
        
        return new Order(config.configIndex, requirements);
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
        UpdateUI();
        
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
            FindFirstObjectByType<EconomyManager>().Currency += currentOrder.reward; //Fulfill & Pay
            orderList[currentDealingOrderIndex] = GenerateOrderFromConfig(orderConfigs[currentDealingOrderIndex]); //generate new order
            
            //Reset Buttons
            foreach (var button in orderButtons) button.interactable = true;
            UpdateUI();
        }
        
        else CancelSubmission();

        isSubmitting = false;
    }

    public void CancelSubmission()
    {
        Order currentOrder = orderList[currentDealingOrderIndex];
        List<Vector2Int> returningItems = new List<Vector2Int>();
        List<GridItemType> keys = new List<GridItemType>(currentOrder.orderDetail.Keys);
    
        foreach (var key in keys)
        {
            var value = currentOrder.orderDetail[key];
            returningItems.Add(new Vector2Int((int)key, value.x));
            currentOrder.orderDetail[key] = new Vector2Int(0, value.y);
        }
        
        onSubmissionCancelled?.Invoke(returningItems);
        isSubmitting = false;
        
        //Reset Buttons
        foreach (var button in orderButtons) button.interactable = true;
        UpdateUI();
    }
    
}
