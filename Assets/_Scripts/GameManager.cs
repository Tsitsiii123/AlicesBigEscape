using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool isGameOver = false; // Flag to track if the game is over
    public bool isPaused = false; // Flag to track if the game is paused
    private RabbitSpawner rabbitSpawner; // Reference to the RabbitSpawner script to manage rabbit spawning
    public GameObject pauseMenuUI; // Reference to the pause menu UI GameObject to show or hide the pause menu when the game is paused or resumed
    public GameObject gameOverUI; // Reference to the game over UI GameObject to show or hide the game over screen when the game ends
    public GameObject victoryUI; // Reference to the victory UI GameObject to show or hide the victory screen when the player wins the game
    public GameObject hudUI; // Reference to the HUD UI GameObject to show or hide the HUD when the game is paused or over

    [Header("UI Elements")]
    public RawImage[] hearts; // Array of RawImage elements to represent the player's health visually
    public TextMeshProUGUI cakesText; // Reference to the game over text element to display the amount of cakes available
    public TextMeshProUGUI potionsText; // Reference to the victory text element to display the amount of potions available
    void Start()
    {
        StartGame(); // Call the method to start the game when the scene loads
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Check if the Escape key is pressed to toggle pause
        {
            PauseGame(); // Call the method to pause or resume the game
        }
    }

    public void StartGame()
    {
        gameOverUI.SetActive(false); // Hide the game over UI when the scene loads
        pauseMenuUI.SetActive(false); // Hide the pause menu UI when the scene loads
        victoryUI.SetActive(false); // Hide the victory UI when the scene loads
        isGameOver = false; // Set the game over flag to false to indicate the game is active
        isPaused = false; // Set the paused flag to false to ensure the game is not paused when starting
        hudUI.SetActive(true); // Show the HUD UI when the game starts
        LockCursor(); // Lock the cursor to the center of the screen to prevent it from moving outside the game window

        Time.timeScale = 1f; // Set the time scale to normal to allow the game to run at normal speed
        
        rabbitSpawner = GetComponent<RabbitSpawner>(); // Find the RabbitSpawner script in the scene to manage rabbit spawning
        rabbitSpawner.SpawnRabbit(); // Call the method to spawn initial rabbits when the game starts

        Debug.Log("Game Started"); // Log a message to the console indicating that the game has started
    }

    public void PauseGame()
    {
        isPaused = !isPaused; // Toggle the paused flag to switch between paused and unpaused states

        if (isPaused)
        {
            Time.timeScale = 0f; // Set the time scale to 0 to effectively pause all game activity
            pauseMenuUI.SetActive(true); // Show the pause menu UI when the game is paused
            UnlockCursor(); // Unlock the cursor to allow the player to interact with the pause menu
            hudUI.SetActive(false); // Hide the HUD UI when the game is paused

            Debug.Log("Game Paused"); // Log a message to the console indicating that the game has been paused
        }

        else
        {
            Time.timeScale = 1f; // Set the time scale back to normal to resume all game activity
            pauseMenuUI.SetActive(false); // Hide the pause menu UI when the game is resumed
            LockCursor(); // Lock the cursor to the center of the screen to prevent it from moving outside the game window
            hudUI.SetActive(true); // Show the HUD UI when the game is resumed
            
            Debug.Log("Game Resumed"); // Log a message to the console indicating that the game has been resumed
        }
    }

    public void GameOver()
    {
        isGameOver = true; // Set the game over flag to true to indicate that the game has ended
        Time.timeScale = 0f; // Set the time scale to 0 to stop all game activity when the game is over

        hudUI.SetActive(false); // Hide the HUD UI when the game is over
        gameOverUI.SetActive(true); // Show the game over UI when the game is over
        UnlockCursor(); // Unlock the cursor to allow the player to interact with the game over UI

        Debug.Log("Game Over"); // Log a message to the console indicating that the game is over
    }

    public void Victory()
    {
        isGameOver = true; // Set the game over flag to true to indicate that the game has ended
        Time.timeScale = 0f; // Set the time scale to 0 to stop all game activity when the player achieves victory

        hudUI.SetActive(false); // Hide the HUD UI when the player achieves victory
        victoryUI.SetActive(true); // Show the victory UI when the player achieves victory
        UnlockCursor(); // Unlock the cursor to allow the player to interact with the victory UI

        Debug.Log("Victory!"); // Log a message to the console indicating that the player has won the game
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene to restart the game
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; // Reset the time scale to normal before returning to the main menu
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene when the player chooses to return to the main menu
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor to allow the player to interact with UI elements
        Cursor.visible = true; // Make the cursor visible when it is unlocked
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen to prevent it from moving outside the game window
        Cursor.visible = false; // Hide the cursor when it is locked to avoid distraction during gameplay
    }

    public void UpdateLivesUI(int currentHealth)
    {
        int activeHearts = currentHealth/20; // Get the current number of active hearts based on the player's health

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < activeHearts)
            {
                hearts[i].enabled = true; // Enable the heart image for each remaining life
            }
            else
            {
                hearts[i].enabled = false; // Disable the heart image for lost lives
            }
        }
    }

    public void UpdateInventoryUI(int cakes, int potions)
    {
        cakesText.text = "Cakes: " + cakes.ToString(); // Update the text element to display the current number of cakes
        potionsText.text = "Potions: " + potions.ToString(); // Update the text element to display the current number of potions
    }
}
