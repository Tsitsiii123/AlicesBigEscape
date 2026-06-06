using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerInventory inventory; 

    void Start()
    {
        inventory = GetComponent<PlayerInventory>(); // Get the PlayerInventory component attached to the player to manage collected items
    }

    void Update()
    {
       UseItem(); // Call the method to check for item usage input every frame
    }

    private void OnTriggerEnter(Collider other)
    {
        BaseItem item = other.GetComponent<BaseItem>(); // Check if the collided object has a BaseItem component, which indicates it is an item that can be collected   

        if (item != null) // If the collided object is an item that can be collected
        {
            item.Interact(inventory); // Call the Interact method on the item, passing the player's inventory to add the item to the inventory and handle any specific interactions defined in the item's Interact method
        }
    }

    public void UseItem()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Check if the player presses the E key to use an item
        {
            if (inventory.ConsumeItem("LifeItem")) // Try to consume a "LifeItem" from the inventory when the E key is pressed
            {
                EatCake(); // Call the EatCake method to handle the effects of consuming the item
            }
        }
        
        if (Input.GetMouseButtonDown(0)) // Check if the player presses the left mouse button to use an item
        {
            if (inventory.ConsumeItem("VanishPotion")) // Try to consume a "VanishPotion" from the inventory when the left mouse button is pressed
            {
                ThrowVanishPotion(); // Call the ThrowVanishPotion method to handle the effects of consuming the item
            }
        }
    }

    private void EatCake()
    {
        // Implement health restoration logic here (e.g., increase player's health)
    }

    private void ThrowVanishPotion()
    {
        // Implement invisibility logic here (e.g., make player invisible for a short duration)
    }
}        
