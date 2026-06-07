using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
   private GameObject[] checkpoints; // Array to store references to all checkpoint game objects in the scene

   void Start()
   {
       checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint"); // Find all game objects in the scene with the tag "Checkpoint" and store them in the checkpoints array
   }

   public Vector3 FindClosestCheckpoint(Vector3 playerPosition)
   {
       GameObject closestCheckpoint = null; // Variable to keep track of the closest checkpoint found
       float closestDistance = Mathf.Infinity; // Initialize the closest distance to infinity for comparison

       foreach (GameObject checkpointObj in checkpoints) // Loop through each checkpoint game object in the checkpoints array
       {
           float distance = Vector3.Distance(playerPosition, checkpointObj.transform.position); // Calculate the distance from the player's position to the checkpoint's position

           if (distance < closestDistance) // Check if this checkpoint is closer than the previously found closest checkpoint
           {
               closestDistance = distance; // Update the closest distance to this checkpoint's distance
               closestCheckpoint = checkpointObj; // Update the closest checkpoint reference to this checkpoint
           }
       }

       if (closestCheckpoint != null) // If a closest checkpoint was found, return its position to be used for respawning the player
        {
            return closestCheckpoint.transform.position; // Return the position of the closest checkpoint for respawning the player
        }

        else
        {
            Debug.LogWarning("No checkpoints found in the scene!"); // Log a warning message if no checkpoints were found in the scene
            return playerPosition; // Return the player's current position as a fallback if no checkpoints are available            
        }
    }
}
