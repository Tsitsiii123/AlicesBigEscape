using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    private GameManager gameManager; // Reference to the GameManager script to access the current game state and manage the victory condition, can be set in the Unity editor to link it to the appropriate game manager object in the scene

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // Find the GameManager script in the scene to initialize the reference for this victory trigger
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player has entered the trigger collider of this victory trigger to determine if we should trigger the victory condition
        {
            gameManager.Victory(); // Call the Victory method in the GameManager script to handle the victory condition and transition to the victory state
        }
    }
}
