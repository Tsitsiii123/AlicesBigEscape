using Microsoft.VisualBasic;
using UnityEngine;

public class MapItem : MonoBehaviour
{
    private QuestManager questManager; // Reference to the QuestManager to check if the player has completed the quest to find the map

    void Start()
    {
        questManager = FindObjectOfType<QuestManager>(); // Find the QuestManager in the scene to check if the player has completed the quest to find the map
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the rabbit collides with the player
        {
            string message = "You found the map! Press 'M' to toggle the map view and navigate the world more easily!"; // Create a message to display when the player collides with the map item
            questManager.ShowCollectionMessage(message); // Call the ShowCollectionMessage method on the QuestManager to display the message to the player when they collide with the map item

            FindObjectOfType<MapManager>().hasMap = true; // Set the hasMap property of the Player script to true when colliding with the map, indicating that the player has obtained the map item
            Debug.Log("You found the map! This will help you navigate the world and find your way to safety!"); // Log a message to the console to inform the player that they have found the map item and its significance for gameplay
            Destroy(gameObject); // Destroy the map item game object after the player collides with it to remove it from the scene since it has been collected by the player
        }
    }       
}
