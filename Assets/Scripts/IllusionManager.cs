using UnityEngine;
using System.Collections.Generic;

public class IllusionManager : MonoBehaviour
{
    [Header("Materials and References")]
    public Transform mazeWallsParent; // Reference to the parent transform that contains all the maze wall objects to manage their visibility for the illusion effect
    public Material invisibleMaterial; // Reference to the material that will be applied to the maze walls when they are made invisible for the illusion effect
    public Material originalMaterial; // Reference to store the original material of the maze walls so that we can restore it after the illusion effect ends
    
    [Header("Difficulty Settings")]
    private int currentWallsToChange; // Counter to track how many walls have been changed to invisible during the illusion effect, used to ensure we only change the intended number of walls
    public int[] wallsToChangePerLevel = new int[5]; // Array to store the number of walls to change for the illusion effect based on the difficulty level, can be set in the Unity editor for each level
    
    private List<GameObject> activeIllusionWalls = new List<GameObject>(); // List to keep track of the currently invisible walls during the illusion effect so that we can easily restore their visibility later
    
    void Start()
    {
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

            for (int i = 0; i < wallsToAdd; i++) // Loop through the number of walls we need to make invisible
            {
                int randomIndex = Random.Range(0, mazeWallsParent.childCount); // Get a random index to select a random wall from the maze walls parent transform
                GameObject wallToChange = mazeWallsParent.GetChild(randomIndex).gameObject; // Get the wall at the random index

                if (!activeIllusionWalls.Contains(wallToChange)) // Check if this wall is not already invisible to avoid changing the same wall multiple times for the illusion effect
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
                    int randomListIndex = Random.Range(0, activeIllusionWalls.Count); // Get a random index to select a random wall from the list of currently invisible walls
                    GameObject wallToRestore = activeIllusionWalls[randomListIndex]; // Get the wall at the random index from the list of currently invisible walls to restore its material for the illusion effect
                    activeIllusionWalls.RemoveAt(randomListIndex); // Remove this wall from the list of currently invisible walls since we are restoring it to visible
                    wallToRestore.GetComponent<Renderer>().material = originalMaterial; // Change the material of this wall back to the original material to end the illusion effect for this wall
                
                    InvisibleWall scriptToRemove = wallToRestore.GetComponent<InvisibleWall>(); // Get the script component that handles the invisible wall behavior to disable it since we are restoring this wall to visible
                    if (scriptToRemove != null) // Check if this wall has the script component for the invisible wall behavior to disable it
                    {
                        Destroy(scriptToRemove); // Destroy the script component that handles the invisible wall behavior to ensure this wall is fully restored to visible and does not have any remaining effects from being invisible for the illusion effect
                    }
                }
            }
        }    
    }
}
