using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject guardPrefab; // Reference to the guard prefab to be spawned
    public Transform player; // Reference to the player's transform to determine where to spawn guards
    public int[] maxGuardsPerLevel = new int[5]; // Array to store the maximum number of guards allowed per level
    public float minSpawnDistanceFromPlayer = 30f; // Minimum distance from the player at which guards can spawn
    public float minSpawnDistanceFromOtherGuards = 15f; // Minimum distance from other guards at which new guards can spawn

    public float mapMinX = -292f; // Minimum X coordinate for spawning guards
    public float mapMaxX = 305f; // Maximum X coordinate for spawning guards
    public float mapMinZ = -293f; // Minimum Z coordinate for spawning guards
    public float mapMaxZ = 12f; // Maximum Z coordinate for spawning guards

    public void SpawnGuard(int levelIndex)
    {
        int targetGuardCount = maxGuardsPerLevel[levelIndex]; // Get the target number of guards for the current level from the array

        GameObject[] currentGuards = GameObject.FindGameObjectsWithTag("Guard"); // Find all existing guards in the scene to count how many are currently active
        int currentGuardCount = currentGuards.Length; // Get the current number of guards in the scene

        int guardsToSpawn = targetGuardCount - currentGuardCount; // Calculate how many guards need to be spawned to reach the target count for the current level

        if (guardsToSpawn > 0) // Check if there are guards that need to be spawned
        {
            for (int i = 0; i < guardsToSpawn; i++) // Loop to spawn the required number of guards
            {
                Vector3 safePos = FindValidSpawnPos(currentGuards); // Get a valid spawn position for the new guard that is not too close to the player or other guards

                if (safePos != Vector3.zero) // Check if a valid spawn position was found
                {
                    Instantiate(guardPrefab, safePos, Quaternion.identity); // Instantiate a new guard at the valid spawn position with no rotation
                    currentGuards = GameObject.FindGameObjectsWithTag("Guard"); // Update the array of current guards to include the newly spawned guard for the next iteration of finding valid spawn positions
                }
                else
                {
                    Debug.LogWarning("Could not find a valid spawn position for a guard. Retrying...)"); // Log a warning message if a valid spawn position could not be found for a guard, indicating that the spawn attempt will be retried   
                }
            }
        }
    }

    private Vector3 FindValidSpawnPos(GameObject[] currentGuards)
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

                bool farEnoughFromOtherGuards = true; // Flag to track if the potential spawn position is far enough from any existing guard

                foreach (GameObject guard in currentGuards) // Loop through each existing guard to check the distance from the potential spawn position
                {
                    if (Vector3.Distance(potentialSpawnPos, guard.transform.position) < minSpawnDistanceFromOtherGuards) // Check if the potential spawn position is too close to this guard
                    {
                        farEnoughFromOtherGuards = false; // If it is too close to this guard, set the flag to false
                        break; // Break out of the loop since we already know this position is not valid
                    }
                }

                if (farEnoughFromOtherGuards) // If the potential spawn position is far enough from all existing guards, return this position as a valid spawn location
                {
                    return potentialSpawnPos; // Return the valid spawn position for the guard to be spawned at
                }
            }
        }

        return Vector3.zero; // If no valid spawn position was found after the maximum number of attempts, return Vector3.zero to indicate failure
    }
        
}
