using UnityEngine;
using System.Collections.Generic;

public class IllusionManager : MonoBehaviour
{
    [Header("Materials and References")]
    public Transform mazeWallsParent; // Reference to the parent transform that contains all the maze wall objects to manage their visibility for the illusion effect
    public Material invisibleMaterial; // Reference to the material that will be applied to the maze walls when they are made invisible for the illusion effect
    public Material originalMaterial; // Reference to store the original material of the maze walls so that we can restore it after the illusion effect ends

    [Header("Player Settings")] 
    public Transform playerTransform; // Reference to the player's transform to potentially use for distance-based illusion effects or other player-related logic for the illusion effect, can be set in the Unity editor to link it to the player object in the scene
    public float safeDistance = 20f; // Distance threshold to ensure that walls are not made invisible if the player is too close to them for the illusion effect, can be adjusted in the Unity editor based on the desired gameplay experience for the illusion effect

    [Header("Difficulty Settings")]
    private int currentWallsToChange; // Counter to track how many walls have been changed to invisible during the illusion effect, used to ensure we only change the intended number of walls
    public int[] wallsToChangePerLevel = new int[5]; // Array to store the number of walls to change for the illusion effect based on the difficulty level, can be set in the Unity editor for each level
    
    private List<GameObject> activeIllusionWalls = new List<GameObject>(); // List to keep track of the currently invisible walls during the illusion effect so that we can easily restore their visibility later
    
    void Start()
    {
        for (int i = 0; i < mazeWallsParent.childCount; i++) // Loop through all the child objects of the maze walls parent transform to store their original materials for later restoration after the illusion effect ends
        {
            GameObject wall = mazeWallsParent.GetChild(i).gameObject; // Get the wall at the current index
            Renderer wallRenderer = wall.GetComponent<Renderer>(); // Get the renderer component of this wall to access its material for storing the original material for later restoration after the illusion effect ends
            wallRenderer.material = originalMaterial; // Set the material of this wall to the original material to ensure all walls start with the correct material for the illusion effect and to store it for later restoration after the illusion effect ends
        }

        SetDifficultyLevel(0); // Set the initial difficulty level to 0 (or the desired starting level) to initialize the number of walls to change for the illusion effect based on the defined values for that level
    }

    public void SetDifficultyLevel(int levelIndex)
    {
        currentWallsToChange = wallsToChangePerLevel[levelIndex]; // Set the number of walls to change for the illusion effect based on the defined values for the current difficulty level using the provided index
        ApplyIllusionEffect(); // Call the method to apply the illusion effect based on the new difficulty level settings
    }

    public void ApplyIllusionEffect()
    {
        int currentInvisibleWalls = activeIllusionWalls.Count; // Get the current number of invisible walls to determine how many more we need to change to reach the target for the illusion effect

        if (currentWallsToChange > currentInvisibleWalls) // Check if we need to make more walls invisible to reach the target for the illusion effect
        {
            int wallsToAdd = currentWallsToChange - currentInvisibleWalls; // Calculate how many more walls we need to make invisible to reach the target for the illusion effect

            int maxAttempts = 100; // Set a maximum number of attempts to avoid potential infinite loops when trying to find walls to change for the illusion effect
            int attempts = 0; // Initialize a counter for the number of attempts made to find walls to change for the illusion effect

            for (int i = 0; i < wallsToAdd; i++) // Loop through the number of walls we need to make invisible
            {
                attempts++; // Increment the attempts counter for each iteration to track how many attempts have been made to find walls to change for the illusion effect

                if (attempts > maxAttempts) // Check if we have exceeded the maximum number of attempts to avoid potential infinite loops when trying to find walls to change for the illusion effect
                {
                    Debug.LogWarning("Max attempts reached while trying to find walls to change for the illusion effect."); // Log a warning message to indicate that we have reached the maximum number of attempts for finding walls to change for the illusion effect
                    break; // Break out of the loop to avoid an infinite loop and ensure we do not continue trying to find walls to change for the illusion effect
                }
                
                int randomIndex = Random.Range(0, mazeWallsParent.childCount); // Get a random index to select a random wall from the maze walls parent transform
                GameObject wallToChange = mazeWallsParent.GetChild(randomIndex).gameObject; // Get the wall at the random index

                if (!activeIllusionWalls.Contains(wallToChange) && Vector3.Distance(playerTransform.position, wallToChange.transform.position) > safeDistance) // Check if this wall is not already invisible to avoid changing the same wall multiple times for the illusion effect
                {
                    activeIllusionWalls.Add(wallToChange); // Add this wall to the list of currently invisible walls so we can track it for later restoration
                    wallToChange.GetComponent<Renderer>().material = invisibleMaterial; // Change the material of this wall to the invisible material to create the illusion effect
                
                    wallToChange.AddComponent<InvisibleWall>(); // Add the script component that handles the invisible wall behavior to this wall to ensure it behaves as an invisible wall for the illusion effect
                }

                else
                {
                    i--; // If this wall is already invisible, decrement the loop counter to try again with a different random wall to ensure we reach the target number of walls to change for the illusion effect
                }
            }
        }

        else if (currentWallsToChange < currentInvisibleWalls) // Check if we need to restore some walls to visible to reach the target for the illusion effect
        {
            int wallsToRemove = currentInvisibleWalls - currentWallsToChange; // Calculate how many walls we need to restore to visible to reach the target for the illusion effect

            for (int i = 0; i < wallsToRemove; i++) // Loop through the number of walls we need to restore to visible
            {
                if (activeIllusionWalls.Count > 0) // Check if there are any currently invisible walls to restore
                {
                    List<GameObject> safeWallsToRestore = new List<GameObject>(); // Create a list to store walls that are safe to restore based on the player's distance
                    foreach (GameObject wall in activeIllusionWalls) // Loop through the currently invisible walls
                    {
                        if (Vector3.Distance(playerTransform.position, wall.transform.position) > safeDistance) // Check if this wall is far enough from the player to safely restore it to visible for the illusion effect
                        {
                            safeWallsToRestore.Add(wall); // Add this wall to the list of safe walls to restore based on the player's distance
                        }
                    }

                    if (safeWallsToRestore.Count > 0)
                    {
                        int randomListIndex = Random.Range(0, safeWallsToRestore.Count); // Get a random index to select a random wall from the list of safe walls to restore
                        GameObject wallToRestore = safeWallsToRestore[randomListIndex]; // Get the wall at the random index from the list of safe walls to restore
                        activeIllusionWalls.Remove(wallToRestore); // Remove this wall from the list of currently invisible walls since we are restoring it to visible
                        wallToRestore.GetComponent<Renderer>().material = originalMaterial; // Change the material of this wall back to the original material to end the illusion effect for this wall

                        InvisibleWall scriptToRemove = wallToRestore.GetComponent<InvisibleWall>(); // Get the script component that handles the invisible wall behavior to disable it since we are restoring this wall to visible
                        
                        if (scriptToRemove != null) // Check if this wall has the script component for the invisible wall behavior to disable it
                        {
                            Destroy(scriptToRemove); // Destroy the script component that handles the invisible wall behavior to ensure this wall is fully restored to visible and does not have any remaining effects from being invisible for the illusion effect
                        }
                    }

                    else
                    {
                        break; // If there are no safe walls to restore, break out of the loop to avoid an infinite loop and ensure we do not attempt to restore walls that are too close to the player for the illusion effect
                    }
                }
            }
        }    
    }
}
