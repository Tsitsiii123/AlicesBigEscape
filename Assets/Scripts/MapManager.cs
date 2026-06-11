using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject mainCam; // Reference to the main camera game object to enable or disable it based on whether the player has the map or not
    public GameObject mapCam; // Reference to the map camera game object to enable or disable it based on whether the player has the map or not
    public bool hasMap = false; // Flag to track whether the player has obtained the map item, initialized to false since the player starts without the map
    private bool isMapActive = false; // Flag to track whether the map camera is currently active, initialized to false since the map camera starts disabled

    void Start()
    {
        mainCam.SetActive(true); // Ensure the main camera is active at the start of the game since the player does not have the map yet
        mapCam.SetActive(false); // Ensure the map camera is disabled at the start of the game since the player does not have the map yet
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
            }
        }
    }
}
