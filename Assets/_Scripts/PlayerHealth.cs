using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth = 80; // Variable to track the player's current health
    public int maxHealth = 100; // Variable to define the player's maximum health
    private GameManager gameManager; // Reference to the GameManager to handle game over state when health reaches 0
    private CheckpointManager checkpointManager; // Reference to the CheckpointManager to find the closest checkpoint for respawning the player
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // Find the GameManager in the scene to manage game over state
        checkpointManager = FindObjectOfType<CheckpointManager>(); // Find the CheckpointManager in the scene to manage player respawning at checkpoints
    }
    public void EatCake()
    {
        currentHealth += 20; // Increase the player's current health by 20 when consuming a LifeItem

        if (currentHealth > maxHealth) // Check if the current health exceeds the maximum health
        {
            currentHealth = maxHealth; // If it does, set the current health to the maximum health to prevent it from exceeding the limit
        }

        Debug.Log($"Ate cake. Current health: {currentHealth}"); // Log a message to the console indicating that the player ate a cake and showing the current health after consuming the item
    }

    public void LoseLife()
    {
        currentHealth -= 20; // Decrease the player's current health by 20 when taking damage

        if (currentHealth <= 0) // Check if the current health falls below 0
        {
            currentHealth = 0; // If it does, set the current health to 0 to prevent it from going negative
            gameManager.GameOver(); // Call the GameOver method on the GameManager to handle the game over state when the player's health reaches 0
        }

        else
        {
            Vector3 respawnPosition = checkpointManager.FindClosestCheckpoint(transform.position); // Find the closest checkpoint position using the CheckpointManager to respawn the player after taking damage
            transform.position = respawnPosition; // Move the player's position to the closest checkpoint for respawning after taking damage
        }

        Debug.Log($"Took damage. Current health: {currentHealth}"); // Log a message to the console indicating that the player took damage and showing the current health after taking damage


    }
}
