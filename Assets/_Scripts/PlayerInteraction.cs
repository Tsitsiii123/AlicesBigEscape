using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerInventory inventory; 
    public GameObject potionProjectilePrefab; // Reference to the potion projectile prefab for throwing vanish potions
    public Transform throwPoint; // Reference to the point from which the potion projectile will be thrown
    public float throwForce = 2f; // Variable to define the force with which the potion projectile will be thrown
    public float throwUpwardForce = 2f; // Variable to define the upward force applied to the potion projectile when thrown
    private PlayerHealth playerHealth; // Reference to the PlayerHealth component to manage the player's health when consuming items
    public AudioClip throwSound; // Reference to the audio clip that will play when the player throws a vanish potion
    void Start()
    {
        inventory = GetComponent<PlayerInventory>(); // Get the PlayerInventory component attached to the player to manage collected items
        playerHealth = GetComponent<PlayerHealth>(); // Get the PlayerHealth component attached to the player to manage health
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
                playerHealth.EatCake(); // Call the EatCake method on the PlayerHealth component to handle the effects of consuming the item
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

    private void ThrowVanishPotion()
    {   
        GameObject potionProjectile = Instantiate(potionProjectilePrefab, throwPoint.position, throwPoint.rotation); // Instantiate the potion projectile prefab at the throw point's position and rotation
        Rigidbody rb = potionProjectile.GetComponent<Rigidbody>(); // Get the Rigidbody component of the instantiated potion projectile to apply physics forces to it
        Vector3 forceToAdd = (throwPoint.forward * throwForce) + (transform.up * throwUpwardForce); // Calculate the total force to apply to the potion projectile by combining forward and upward forces
        rb.AddForce(forceToAdd, ForceMode.Impulse); // Apply the calculated force to the potion projectile using Impulse mode

        AudioSource.PlayClipAtPoint(throwSound, transform.position); // Play the throw sound at the player's position when throwing a vanish potion
        Debug.Log("Threw vanish potion."); // Log a message to the console indicating that the player threw a vanish potion. The actual effects of the vanish potion would be implemented in the Interact method of the VanishPotion class or through additional logic in this method.
    }
}        
