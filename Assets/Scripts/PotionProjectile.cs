using UnityEngine;

public class PotionProjectile : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Check if the collided object has the tag "Player"
        {
            return; // If the collided object is the player, do not destroy the potion projectile to allow it to affect the player (e.g., apply invisibility or other effects). The actual logic for applying the potion's effects would be implemented in the Interact method of the VanishPotion class or through additional logic in this method.
        }
        if (collision.gameObject.CompareTag("Guard")) // Check if the collided object has the tag "Guard"
        {
            GuardAI guard = collision.gameObject.GetComponent<GuardAI>(); // Get the GuardAI component from the collided guard object

            if (guard != null) // Check if the GuardAI component exists on the collided object
            {
                guard.Vanish(); // Call the Vanish method on the guard to make it vanish
            }
        }
        
        Debug.Log("Potion projectile collided and was destroyed."); // Log a message to the console indicating that the potion projectile has collided and was destroyed. This can help with debugging to confirm that the collision logic is working as intended.
        Destroy(gameObject); // Destroy the potion projectile if it collides with anything other than a guard (e.g., walls, floor) to prevent it from lingering in the scene
    }
}
