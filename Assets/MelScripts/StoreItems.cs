using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoreItems : MonoBehaviour
{
    private static Dictionary<string, float> ItemPrices = new Dictionary<string, float>
    {
        // Your item price definitions
        { "pizza", 8.99f },
        { "croissant_plate", 5.40f },
        { "croissant", 2.40f },
        { "onion", 0.60f },
        { "ginger", 0.40f },
        { "cabbage", 0.80f },
        { "pumpkin", 1.20f },
        { "corn", 0.30f },
        { "potato", 0.45f },
        { "cucumber", 0.45f },
        { "watermelon", 2.30f },
        { "grapejuice", 0.95f },
        { "cola", 0.65f },
        { "water", 0.95f },
        { "energydrink", 0.50f },
        { "frappe", 1.75f },
        { "coffee", 2.30f },
        { "soda", 0.60f },
        { "sweetdrink", 0.30f },
        { "peach", 0.50f },
        { "banana", 0.60f },
        { "carrot", 0.45f },
        { "apple", 0.30f },
        { "tomato", 0.25f },
        { "eggplant", 0.55f },
        { "orange", 0.60f },
        { "melon", 0.75f },
        { "Wholechicken", 9.75f },
        { "Bigsausage", 3.45f },
        { "ham", 5.45f },
        { "steak", 6.50f },
        { "salmonfish", 7.00f },
        { "fish", 4.00f },
        { "sausage", 3.00f },
        { "chickendrum", 1.35f },
        { "sushi", 3.65f },
        { "icecream", 1.25f },
        { "udon", 4.50f },
        { "wasabi", 3.25f },
        { "unagi", 0.25f },
        { "tamago", 0.25f },
        { "roundbread", 0.75f },
        { "longloaf", 0.50f },
        { "breadroll", 0.45f},
        { "pretzel", 0.40f },
        { "roundcheese", 2.75f },
        { "cheeseslice", 1.25f },
        { "macarontower", 6.75f },
        { "macaronbox", 3.50f },
        { "cake", 15.50f },
        { "volcanocake", 9.45f },
        { "cheesecake", 7.50f },
        { "doughnut", 3.25f },
        { "toast", 3.25f },
        { "cereal", 3.50f },
        { "chips", 2.50f },
        { "olive oil", 5.45f },
        { "vanilla", 4.50f },
        { "butter", 1.25f },
        { "fruit juice", 3.25f },
        { "Milk", 3.50f },
        { "Cleaner", 2.50f },
        { "Medicine", 2.45f },
        { "Bandage", 3.50f },
        { "Medkit", 3.25f },
        { "Toothbrush", 1.25f },
        { "Money(10.00)", 10.00f },
        { "coin10", 0.10f },
        { "coin5", 0.05f },
    };

    public string ItemType; // Set this from the Inspector
    public TMP_Text displayItemText; // Reference to the TMP_Text component for displaying item info
    public ShoppingCart shoppingCart; // Reference to the ShoppingCart script

    private void Start()
    {
        // Check if the item type is valid at the start
        if (!ItemPrices.ContainsKey(ItemType))
        {
            Debug.LogError("Invalid item type");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("registryplate"))
        {
            Debug.Log("Valid item type");
            AddItemToCart(); // Add item to shopping cart
            ShowItemDetails(); // Show item details
            DestroyItem(); // Destroy the item
        }
    }

    private void AddItemToCart()
    {
        if (ItemPrices.TryGetValue(ItemType, out float price))
        {
            shoppingCart.AddItem(ItemType, price); // Call the method in ShoppingCart
        }
        else
        {
            Debug.LogError("Item type not found in ItemPrices: " + ItemType);
        }
    }

    public void ShowItemDetails()
    {
        if (ItemPrices.TryGetValue(ItemType, out float price))
        {
            displayItemText.text = $"{ItemType}: ${price:F2}"; // Display item details
        }
        else
        {
            displayItemText.text = "Item not found!";
        }
    }

    private void DestroyItem()
    {
        Debug.Log($"{ItemType} destroyed!");
        Destroy(gameObject); // Destroy the GameObject
    }
}
