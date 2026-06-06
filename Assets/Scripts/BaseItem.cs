using UnityEngine;

public class BaseItem : MonoBehaviour
{
    public string itemType = "BaseItem"; // Type of the item, can be set in the inspector for different item types (e.g., "LifeItem", "AttackPotion")

    public virtual void Interact(PlayerInventory inventory) 
    {
        inventory.AddItem(itemType); // Call the AddItem method on the player's inventory to add this item based on its type
        Destroy(gameObject); // Destroy the item game object after it has been collected by the player
    }
}
