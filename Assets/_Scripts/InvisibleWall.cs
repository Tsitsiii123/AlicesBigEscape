using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
    private Material originalMaterial; // Variable to store the original material of the wall so that we can restore it after the illusion effect ends

    void Start()
    {
        IllusionManager illusionManager = FindObjectOfType<IllusionManager>(); // Find the IllusionManager in the scene to manage the illusion effect on walls
        originalMaterial = illusionManager.originalMaterial; // Store the original material of the wall when the script starts to ensure we can restore it later after the illusion effect ends
    }
    public void OnCollisionEnter(Collision collision) // Method called when another collider enters the trigger collider attached to this invisible wall object
    {
        if (collision.gameObject.CompareTag("Player")) // Check if the colliding object has the tag "Player" to ensure that only the player can trigger the effect of this invisible wall
        {
            GetComponent<Renderer>().material = originalMaterial; // Restore the original material of the wall when the player collides with it
        }
    }
}
