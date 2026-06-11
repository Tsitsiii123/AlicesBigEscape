using UnityEngine;

public class MapItem : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the rabbit collides with the player
        {
            FindObjectOfType<MapManager>().hasMap = true; // Set the hasMap property of the Player script to true when colliding with the map, indicating that the player has obtained the map item
            Debug.Log("You found the map! This will help you navigate the world and find your way to safety!"); // Log a message to the console to inform the player that they have found the map item and its significance for gameplay
            Destroy(gameObject); // Destroy the map item game object after the player collides with it to remove it from the scene since it has been collected by the player
        }
    }       
}
