using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public int currentLevel = 0; // Current difficulty level, starting at 0

    private EnemySpawner enemySpawner; // Reference to the EnemySpawner component to manage guard spawning
    private ItemsSpawner itemsSpawner; // Reference to the ItemSpawner component to manage item spawning
    private IllusionManager illusionManager; // Reference to the IllusionManager component to manage illusion effects based on difficulty level
    private AtmosphereManager atmosphereManager; // Reference to the AtmosphereManager component to manage atmospheric effects based on difficulty level
    void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>(); // Find the EnemySpawner component in the scene to control guard spawning based on difficulty level
        itemsSpawner = FindObjectOfType<ItemsSpawner>(); // Find the ItemsSpawner component in the scene to control item spawning based on difficulty level
        illusionManager = FindObjectOfType<IllusionManager>(); // Find the IllusionManager component in the scene to control illusion effects based on difficulty level
        atmosphereManager = FindObjectOfType<AtmosphereManager>(); // Find the AtmosphereManager component in the scene to control atmospheric effects based on difficulty level

        enemySpawner.SpawnGuard(currentLevel); // Spawn guards for the initial difficulty level when the game starts
        
    }

    public void IncreaseDifficulty()
    {
        if (currentLevel < 4) // Check if the current level is less than the maximum level (4 in this case)
        {
            currentLevel++; // Increment the difficulty level by 1
            enemySpawner.SpawnGuard(currentLevel); // Spawn guards for the new difficulty level to increase the challenge for the player
            itemsSpawner.SetDifficultyLevel(currentLevel); // Set the item spawning difficulty level in the ItemsSpawner to adjust item spawning based on the new difficulty level
            illusionManager.SetDifficultyLevel(currentLevel); // Set the illusion difficulty level in the IllusionManager to adjust illusion effects based on the new difficulty level
            atmosphereManager.SetDifficultyLevel(currentLevel); // Set the atmospheric difficulty level in the AtmosphereManager to adjust fog and lighting based on the new difficulty level
        }
    }

    public void DecreaseDifficulty()
    {
        if (currentLevel > 0) // Check if the current level is greater than the minimum level (0 in this case)
        {
            currentLevel--; // Decrement the difficulty level by 1
            enemySpawner.SpawnGuard(currentLevel); // Spawn guards for the new difficulty level to decrease the challenge for the player
            itemsSpawner.SetDifficultyLevel(currentLevel); // Set the item spawning difficulty level in the ItemsSpawner to adjust item spawning based on the new difficulty level
            illusionManager.SetDifficultyLevel(currentLevel); // Set the illusion difficulty level in the IllusionManager to adjust illusion effects based on the new difficulty level
            atmosphereManager.SetDifficultyLevel(currentLevel); // Set the atmospheric difficulty level in the AtmosphereManager to adjust fog and lighting based on the new difficulty level
        }
    }
}
