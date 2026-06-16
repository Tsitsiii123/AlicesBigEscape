using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject mainCam; // Reference to the main camera game object to enable or disable it based on whether the player has the map or not
    public GameObject mapCam; // Reference to the map camera game object to enable or disable it based on whether the player has the map or not
    public bool hasMap = false; // Flag to track whether the player has obtained the map item, initialized to false since the player starts without the map
    private bool isMapActive = false; // Flag to track whether the map camera is currently active, initialized to false since the map camera starts disabled
    public GameObject inGameHUD; // Reference to the in-game HUD game object to enable or disable it based on whether the player is viewing the map or not
    void Start()
    {
        mainCam.SetActive(true); // Ensure the main camera is active at the start of the game since the player does not have the map yet
        mapCam.SetActive(false); // Ensure the map camera is disabled at the start of the game since the player does not have the map yet
        inGameHUD.SetActive(true); // Ensure the in-game HUD is active at the start of the game
    }

    void Update()
    {
        if (hasMap) // Check if the player has obtained the map item
        {
            if (Input.GetKeyDown(KeyCode.M)) // Check if the player presses the M key to toggle the map view
            {
                isMapActive = !isMapActive; // Toggle the isMapActive flag to switch between the main camera and map camera views

                mapCam.SetActive(isMapActive); // Set the map camera active state based on the isMapActive flag, enabling it when the player wants to view the map and disabling it when they want to return to the main view
                mainCam.SetActive(!isMapActive); // Set the main camera active state to the opposite of the isMapActive flag, disabling it when the player wants to view the map and enabling it when they want to return to the main view
            
                if (isMapActive)
                {
                    Time.timeScale = 0f; // Pause the game when the map camera is active to allow the player to view the map without any in-game distractions or time progression
                    AudioListener.pause = true; // Pause the audio listener to stop all in-game sounds when the map camera is active, enhancing the player's focus on the map
                    inGameHUD.SetActive(false); // Hide the in-game HUD when the map camera is active to provide a clear view of the map without any UI elements obstructing it
                }
                else
                {
                    Time.timeScale = 1f; // Resume the game when the map camera is deactivated to allow the player to continue playing normally
                    AudioListener.pause = false; // Resume the audio listener to allow in-game sounds to play again when the map camera is deactivated
                    inGameHUD.SetActive(true); // Show the in-game HUD when the map camera is deactivated to provide UI elements for the player
                }
            }
        }
    }
}
