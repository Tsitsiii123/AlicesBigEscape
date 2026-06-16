using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public Dictionary<string, int> collectedItems = new Dictionary<string, int>(); // Dictionary to store the player's inventory items and their quantities
    private GameManager gameManager; // Reference to the GameManager to update the UI when items are collected or consumed
    public AudioClip collectSound; // Reference to the audio clip that will play when an item is collected

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // Find the GameManager in the scene to manage UI updates for the inventory
        UpdateUI(); // Update the UI to reflect the player's current inventory when the game starts
    }

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

        AudioSource.PlayClipAtPoint(collectSound, transform.position); // Play the collect sound at the player's position when an item is collected
        UpdateUI(); // Update the UI to reflect the player's current inventory after adding an item
        Debug.Log($"Collected {itemType}. Total: {collectedItems[itemType]}"); // Log a message to the console indicating the item collected and the total quantity of that item type in the inventory
    }

    public bool ConsumeItem(string itemType)
    {
        if (collectedItems.ContainsKey(itemType) && collectedItems[itemType] > 0) // Check if the item type exists in the inventory and has a quantity greater than 0
        {
            collectedItems[itemType]--; // If it exists and has quantity, decrement the quantity of that item type

            UpdateUI(); // Update the UI to reflect the player's current inventory after consuming an item
            Debug.Log($"Consumed {itemType}. Remaining: {collectedItems[itemType]}"); // Log a message to the console indicating the item consumed and the remaining quantity of that item type in the inventory

            return true;
        }

        else
        {
            Debug.Log($"No {itemType} left to consume."); // If the item type does not exist or has no quantity, log a message indicating that there are no items left to consume
            
            return false;
        }
    }

    private void UpdateUI()
    {
        int cakes = 0; // Variable to track the number of cakes in the inventory
        int potions = 0; // Variable to track the number of potions in the inventory

        if (collectedItems.ContainsKey("LifeItem")) // Check if the inventory contains any LifeItem (cake)
        {
            cakes = collectedItems["LifeItem"]; // If it does, set the cakes variable to the quantity of LifeItem in the inventory
        }

        if (collectedItems.ContainsKey("VanishPotion")) // Check if the inventory contains any VanishPotion
        {
            potions = collectedItems["VanishPotion"]; // If it does, set the potions variable to the quantity of VanishPotion in the inventory
        }

        gameManager.UpdateInventoryUI(cakes, potions); // Call the UpdateInventoryUI method on the GameManager to update the UI with the current number of cakes and potions in the inventory
    }
}
    