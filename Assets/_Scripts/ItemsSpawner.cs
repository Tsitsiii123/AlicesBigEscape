using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class ItemsSpawner : MonoBehaviour
{
    [Header("Prefabs & Player")]
    public GameObject cakePrefab; // Reference to the cake prefab to be spawned
    public GameObject VanishPotionPrefab; // Reference to the vanish potion prefab to be spawned
    public Transform player; // Reference to the player's transform to determine where to spawn items

    [Header("Spawn NumberLimits")]
    public int maxCakes = 20; // Maximum number of cakes that can be present in the scene at once
    public int maxVanishPotions = 40; // Maximum number of vanish potions that can be present in the scene at once

    [Header("Timers per Difficulty Level")]
    public int[] cakeTimersPerLevel = new int[5]; // Array to store the maximum number of cakes allowed per level
    public int[] vanishPotionTimersPerLevel = new int[5]; // Array to store the maximum number of vanish potions allowed per level
    private float cakeRespawnTime; // Time in seconds after which a new cake will be spawned if the current number of cakes is below the maximum limit
    private float vanishPotionRespawnTime; // Time in seconds after which a new vanish potion will be spawned if the current number of vanish potions is below the maximum limit

    [Header("Spawn Distances")]
    public float minSpawnDistanceFromPlayer = 30f; // Minimum distance from the player at which items can spawn
    public float minSpawnDistanceFromOtherCakes = 25f; // Minimum distance from other items at which new items can spawn
    public float minSpawnDistanceFromOtherVanishPotions = 25f; // Minimum distance from other items at which new items can spawn

    private float searchTimer = 0f; // Timer to track the time elapsed since the last search for missing items
    private List<float> cakeTimers = new List<float>(); // List to track the respawn timers for each cake currently in the scene
    private List<float> vanishPotionTimers = new List<float>(); // List to track the respawn timers for each vanish potion currently in the scene

    public float mapMinX = -292f; // Minimum X coordinate for spawning items
    public float mapMaxX = 305f; // Maximum X coordinate for spawning items
    public float mapMinZ = -293f; // Minimum Z coordinate for spawning items
    public float mapMaxZ = 12f; // Maximum Z coordinate for spawning items


    void Start()
    {
        SetDifficultyLevel(0); // Initialize the item respawn timers based on the first level's settings at the start of the game
    }
    void Update()
    {
        searchTimer += Time.deltaTime; // Increment the search timer by the time elapsed since the last frame

        if (searchTimer >= 2f) // Check if 2 seconds have passed since the last search for missing items
        {
            CheckMissingCakes(); // Call the method to check for missing cakes and add timers for respawning them if needed
            CheckMissingVanishPotions(); // Call the method to check for missing vanish potions and add timers for respawning them if needed
            searchTimer = 0f; // Reset the search timer to start counting for the next 2-second interval
        }
        TickTimersAndSpawn(); // Call the method to update the respawn timers and spawn items when their timers reach zero
    }

    public void CheckMissingCakes()
    {
        GameObject[] currentCakes = GameObject.FindGameObjectsWithTag("Cake"); // Find all existing cakes in the scene to count how many are currently active
        int totalCakes = currentCakes.Length + cakeTimers.Count;  // Get the current number of cakes in the scene plus the number of cakes that are currently in the respawn process (tracked by the timers)    

        if (totalCakes < maxCakes) // Check if the total number of cakes (active + in respawn) is below the maximum limit for cakes
        {
            cakeTimers.Add(cakeRespawnTime); // If it is below the limit, add a new timer to the list to start the respawn process for a new cake
        }
    }
    public void CheckMissingVanishPotions()
    {
        GameObject[] currentVanishPotions = GameObject.FindGameObjectsWithTag("VanishPotion"); // Find all existing vanish potions in the scene to count how many are currently active
        int totalVanishPotions = currentVanishPotions.Length + vanishPotionTimers.Count;  // Get the current number of vanish potions in the scene plus the number of vanish potions that are currently in the respawn process (tracked by the timers)    

        if (totalVanishPotions < maxVanishPotions) // Check if the total number of vanish potions (active + in respawn) is below the maximum limit for vanish potions
        {
            vanishPotionTimers.Add(vanishPotionRespawnTime); // If it is below the limit, add a new timer to the list to start the respawn process for a new vanish potion
        }
    }

    public void TickTimersAndSpawn()
    {
        //--Cakes--
        for (int i = cakeTimers.Count - 1; i >= 0; i--) // Loop through the list of cake timers in reverse to safely remove timers while iterating
        {
            cakeTimers[i] -= Time.deltaTime; // Decrease each cake timer by the time elapsed since the last frame

            if (cakeTimers[i] <= 0f) // Check if this cake timer has reached zero, indicating that it's time to spawn a new cake
            {
                GameObject[] currentCakes = GameObject.FindGameObjectsWithTag("Cake"); // Find all existing cakes in the scene to ensure we have the most up-to-date count of active cakes
                Vector3 safePos = FindValidSpawnPos(currentCakes, minSpawnDistanceFromOtherCakes); // Get a valid spawn position for the new cake that is not too close to the player or other cakes

                if (safePos != Vector3.zero) // Check if a valid spawn position was found
                {
                    Instantiate(cakePrefab, safePos, Quaternion.identity); // Instantiate a new cake at the valid spawn position with no rotation
                }

                cakeTimers.RemoveAt(i); // Remove this timer from the list since we have spawned the cake for it
                Debug.Log("Cake spawned!");
            }
        }
        //--Vanish Potions--
        for (int i = vanishPotionTimers.Count - 1; i >= 0; i--) // Loop through the list of vanish potion timers in reverse to safely remove timers while iterating
        {
            vanishPotionTimers[i] -= Time.deltaTime; // Decrease each vanish potion timer by the time elapsed since the last frame

            if (vanishPotionTimers[i] <= 0f) // Check if this vanish potion timer has reached zero, indicating that it's time to spawn a new vanish potion
            {
                GameObject[] currentVanishPotions = GameObject.FindGameObjectsWithTag("VanishPotion"); // Find all existing vanish potions in the scene to ensure we have the most up-to-date count of active vanish potions
                Vector3 safePos = FindValidSpawnPos(currentVanishPotions, minSpawnDistanceFromOtherVanishPotions); // Get a valid spawn position for the new vanish potion that is not too close to the player or other vanish potions

                if (safePos != Vector3.zero) // Check if a valid spawn position was found
                {
                    Instantiate(VanishPotionPrefab, safePos, Quaternion.identity); // Instantiate a new vanish potion at the valid spawn position with no rotation
                }

                vanishPotionTimers.RemoveAt(i); // Remove this timer from the list since we have spawned the vanish potion for it
                Debug.Log("Vanish Potion spawned!");
            }
        }
    }
    public void SetDifficultyLevel(int levelIndex)
    {
        cakeRespawnTime = cakeTimersPerLevel[levelIndex]; // Set the respawn time for cakes based on the defined timers for the current difficulty level using the provided index
        vanishPotionRespawnTime = vanishPotionTimersPerLevel[levelIndex]; // Set the respawn time for vanish potions based on the defined timers for the current difficulty level using the provided index
    }
    
    private Vector3 FindValidSpawnPos(GameObject[] currentItems, float minDistance) // Method to find a valid spawn position for an item that is not too close to the player or other items
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

                bool farEnoughFromOtherItems = true; // Flag to track if the potential spawn position is far enough from any existing item

                foreach (GameObject item in currentItems) // Loop through each existing item to check the distance from the potential spawn position
                {
                    if (Vector3.Distance(potentialSpawnPos, item.transform.position) < minDistance) // Check if the potential spawn position is too close to this item
                    {
                        farEnoughFromOtherItems = false; // If it is too close to this item, set the flag to false
                        break; // Break out of the loop since we already know this position is not valid
                    }
                }

                if (farEnoughFromOtherItems) // If the potential spawn position is far enough from all existing items, return this position as a valid spawn location
                {
                    return potentialSpawnPos; // Return the valid spawn position for the item to be spawned at
                }
            }
        }

        return Vector3.zero; // If no valid spawn position was found after the maximum number of attempts, return Vector3.zero to indicate failure
    }    
}




