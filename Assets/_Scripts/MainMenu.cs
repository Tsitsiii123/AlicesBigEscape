using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene"); // Load the main game scene when the player clicks the "Play" button in the main menu
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application when the player clicks the "Quit" button in the main menu
        Debug.Log("Game Quit"); // Log a message to the console indicating that the game has been quit (useful for testing in the Unity editor)
    }
}
