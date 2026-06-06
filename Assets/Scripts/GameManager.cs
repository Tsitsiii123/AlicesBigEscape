using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isGameOver = false; // Flag to track if the game is over
    public bool isPaused = false; // Flag to track if the game is paused

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
        isGameOver = false; // Set the game over flag to false to indicate the game is active
        isPaused = false; // Set the paused flag to false to ensure the game is not paused when starting
        Time.timeScale = 1f; // Set the time scale to normal to allow the game to run at normal speed

        Debug.Log("Game Started"); // Log a message to the console indicating that the game has started
    }

    public void PauseGame()
    {
        isPaused = !isPaused; // Toggle the paused flag to switch between paused and unpaused states

        if (isPaused)
        {
            Time.timeScale = 0f; // Set the time scale to 0 to effectively pause all game activity
            Debug.Log("Game Paused"); // Log a message to the console indicating that the game has been paused
        }

        else
        {
            Time.timeScale = 1f; // Set the time scale back to normal to resume all game activity
            Debug.Log("Game Resumed"); // Log a message to the console indicating that the game has been resumed
        }
    }

    public void GameOver()
    {
        isGameOver = true; // Set the game over flag to true to indicate that the game has ended
        Time.timeScale = 0f; // Set the time scale to 0 to stop all game activity when the game is over

        Debug.Log("Game Over"); // Log a message to the console indicating that the game is over
    }

    public void Victory()
    {
        isGameOver = true; // Set the game over flag to true to indicate that the game has ended
        Time.timeScale = 0f; // Set the time scale to 0 to stop all game activity when the player achieves victory

        Debug.Log("Victory!"); // Log a message to the console indicating that the player has won the game
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene to restart the game
    }
}
