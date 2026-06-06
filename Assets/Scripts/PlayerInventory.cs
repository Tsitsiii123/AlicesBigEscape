using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public Dictionary<string, int> collectedItems = new Dictionary<string, int>(); // Dictionary to store the player's inventory items and their quantities

    public void AddItem(string itemType)
    {
        if (collectedItems.ContainsKey(itemType)) // Check if the item type already exists in the inventory
        {
            collectedItems[itemType]++; // If it exists, increment the quantity of that item type
        }
        else
        {
            collectedItems[itemType] = 1; // If it does not exist, add the item type to the inventory with a quantity of 1
        }

        Debug.Log($"Collected {itemType}. Total: {collectedItems[itemType]}"); // Log a message to the console indicating the item collected and the total quantity of that item type in the inventory
    }

    public bool ConsumeItem(string itemType)
    {
        if (collectedItems.ContainsKey(itemType) && collectedItems[itemType] > 0) // Check if the item type exists in the inventory and has a quantity greater than 0
        {
            collectedItems[itemType]--; // If it exists and has quantity, decrement the quantity of that item type

            Debug.Log($"Consumed {itemType}. Remaining: {collectedItems[itemType]}"); // Log a message to the console indicating the item consumed and the remaining quantity of that item type in the inventory

            return true;
        }

        else
        {
            Debug.Log($"No {itemType} left to consume."); // If the item type does not exist or has no quantity, log a message indicating that there are no items left to consume
            
            return false;
        }
    }
}
    