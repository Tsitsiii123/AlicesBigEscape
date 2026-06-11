using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision) // Method called when another collider enters the trigger collider attached to this invisible wall object
    {
        if (collision.gameObject.CompareTag("Player")) // Check if the colliding object has the tag "Player" to ensure that only the player can trigger the effect of this invisible wall
        {
            Color wallColor = GetComponent<Renderer>().material.color; // Get the current color of the wall's material to modify its alpha value for the invisible effect
            wallColor.a = 1f; // Set the alpha value of the wall's color
            GetComponent<Renderer>().material.color = wallColor; // Apply the modified color with the new alpha value back to the wall's material to make it partially transparent and create the illusion of an invisible wall
        }
    }
}
