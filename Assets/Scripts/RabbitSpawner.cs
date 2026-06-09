using UnityEngine;
using UnityEngine.AI;

public class RabbitSpawner : MonoBehaviour
{
    public GameObject rabbitPrefab; // Reference to the rabbit prefab to be spawned
    public Transform player; // Reference to the player's transform to determine where to spawn rabbits
    public float minSpawnDistanceFromPlayer = 100f; // Minimum distance from the player at which rabbits can spawn
    public float minSpawnDistanceFromOtherRabbits = 60f; // Minimum distance from other rabbits at which new rabbits can spawn

    public float mapMinX = -292f; // Minimum X coordinate for spawning rabbits
    public float mapMaxX = 305f; // Maximum X coordinate for spawning rabbits
    public float mapMinZ = -293f; // Minimum Z coordinate for spawning rabbits
    public float mapMaxZ = 12f; // Maximum Z coordinate for spawning rabbits

    public void SpawnRabbit()
    {
        
        int rabbitsToSpawn = 10; // Calculate how many guards need to be spawned to reach the target count for the current level
        
        GameObject[] currentRabbits = GameObject.FindGameObjectsWithTag("Rabbit"); // Find all existing rabbits in the scene to count how many are currently active

        for (int i = 0; i < rabbitsToSpawn; i++) // Loop to spawn the required number of rabbits
        {
            Vector3 safePos = FindValidSpawnPos(currentRabbits); // Get a valid spawn position for the new rabbit that is not too close to the player or other rabbits

            if (safePos != Vector3.zero) // Check if a valid spawn position was found
            {
                Instantiate(rabbitPrefab, safePos, Quaternion.identity); // Instantiate a new rabbit at the valid spawn position with no rotation
                currentRabbits = GameObject.FindGameObjectsWithTag("Rabbit"); // Update the array of current rabbits to include the newly spawned rabbit for the next iteration of finding valid spawn positions
            }
            else
            {
                Debug.LogWarning("Could not find a valid spawn position for a rabbit. Retrying...)"); // Log a warning message if a valid spawn position could not be found for a rabbit, indicating that the spawn attempt will be retried   
            }
        }        
    }

    private Vector3 FindValidSpawnPos(GameObject[] currentRabbits) // Method to find a valid spawn position for a rabbit that is not too close to the player or other rabbits
    {
        int maxAttempts = 30; // Maximum number of attempts to find a valid spawn position to prevent infinite loops

        for (int i = 0; i < maxAttempts; i++)
        {
            float randomX = Random.Range(mapMinX, mapMaxX); // Generate a random X coordinate within the defined map boundaries
            float randomZ = Random.Range(mapMinZ, mapMaxZ); // Generate a random Z coordinate within the defined map boundaries
            Vector3 randomPos = new Vector3(randomX, 0f, randomZ); // Create a new Vector3 for the random spawn position with Y set to 0

            NavMeshHit hit; // Variable to store the result of the NavMesh sampling
            
            if (NavMesh.SamplePosition(randomPos, out hit, 5f, NavMesh.AllAreas)) // Sample the NavMesh to find a valid position for the guard to spawn within a radius of 5 units from the random position
            {
                Vector3 potentialSpawnPos = hit.position; // Get the valid spawn position from the NavMesh sampling result

                if (Vector3.Distance(potentialSpawnPos, player.position) < minSpawnDistanceFromPlayer) // Check if the potential spawn position is too close to the player
                {
                    continue; // If it is too close to the player, skip this position and try another one
                }

                bool farEnoughFromOtherRabbits = true; // Flag to track if the potential spawn position is far enough from any existing rabbit

                foreach (GameObject rabbit in currentRabbits) // Loop through each existing rabbit to check the distance from the potential spawn position
                {
                    if (Vector3.Distance(potentialSpawnPos, rabbit.transform.position) < minSpawnDistanceFromOtherRabbits) // Check if the potential spawn position is too close to this rabbit
                    {
                        farEnoughFromOtherRabbits = false; // If it is too close to this rabbit, set the flag to false
                        break; // Break out of the loop since we already know this position is not valid
                    }
                }

                if (farEnoughFromOtherRabbits) // If the potential spawn position is far enough from all existing rabbits, return this position as a valid spawn location
                {
                    return potentialSpawnPos; // Return the valid spawn position for the rabbit to be spawned at
                }
            }
        }

        return Vector3.zero; // If no valid spawn position was found after the maximum number of attempts, return Vector3.zero to indicate failure
    }    
}



