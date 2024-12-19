using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShoppingCart1 : MonoBehaviour
{
    public TMP_Text displayItemText; // Reference to the TMP_Text for displaying item info
    public TMP_Text totalCostText; // Reference to display total cost
    public Button paymentButton; // Reference to the payment button

    private Dictionary<string, int> selectedItems = new Dictionary<string, int>();
    private float totalCost = 0f;

    private void Start()
    {
        paymentButton.onClick.AddListener(OnPaymentButtonClicked); // Set up the payment button
        UpdateTotalCostDisplay();
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
