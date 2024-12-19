using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.AI;

public class ShoppingCart : MonoBehaviour
{
    public TMP_Text displayItemText; // Reference to the TMP_Text for displaying item info
    public TMP_Text totalCostText; // Reference to display total cost
    public Button paymentButton; // Reference to the payment button

    private Dictionary<string, int> selectedItems = new Dictionary<string, int>();
    private float totalCost = 0f;

    private GameObject leavingArea; // Reference to the LeavingArea
    private NavMeshAgent agent; // Reference to the agent

    private void Start()
    {
        paymentButton.onClick.AddListener(OnPaymentButtonClicked); // Set up the payment button
        UpdateTotalCostDisplay();

        // Find the LeavingArea object
        leavingArea = GameObject.FindGameObjectWithTag("LeavingArea");
        if (leavingArea == null)
        {
            Debug.LogError("No object with 'LeavingArea' tag found in the scene.");
        }

        // Get the NavMeshAgent component from the agent GameObject
        agent = GameObject.FindObjectOfType<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("No NavMeshAgent found in the scene.");
        }
    }

    public void AddItem(string itemType, float itemPrice)
    {
        // Add or update the item count
        if (selectedItems.ContainsKey(itemType))
        {
            selectedItems[itemType]++;
        }
        else
        {
            selectedItems[itemType] = 1;
        }
        totalCost += itemPrice; // Update total cost
        UpdateTotalCostDisplay(); // Update the total cost display
        ShowItemDetails(); // Update the item details display
    }

    private void ShowItemDetails()
    {
        string itemList = "Items:\n";
        
        foreach (var item in selectedItems)
        {
            itemList += $"{item.Key} x{item.Value}\n"; // Show item with quantity
        }
        itemList += $"Total: ${totalCost:F2}"; // Show total cost
        displayItemText.text = itemList; // Display items in the TMP_Text
    }

    public void UpdateTotalCostDisplay()
    {
        totalCostText.text = $"Total: ${totalCost:F2}"; // Display total cost
    }
public void OnPaymentButtonClicked()
{
    Debug.Log("Payment made. Total cost: $" + totalCost);
    ResetSelections(); // Optionally reset selections after payment

    // Move the agent to the LeavingArea
    if (leavingArea != null && agent != null)
    {
        // Call the agent's method to leave the waiting area
        NewAgent agentScript = agent.GetComponent<NewAgent>();
        if (agentScript != null)
        {
            agentScript.LeaveWaitingArea(leavingArea.transform.position);
        }
        else
        {
            Debug.LogError("Agent does not have a NewAgent script attached.");
        }
    }
    else
    {
        Debug.LogError("Cannot move agent to LeavingArea. Either LeavingArea or agent is missing.");
    }
}

    public void ResetSelections()
    {
        selectedItems.Clear();
        totalCost = 0f;
        totalCostText.text = "Total: $0.00"; // Reset the total display
        displayItemText.text = ""; // Clear the item display
        Debug.Log("Selections reset.");
    }
}