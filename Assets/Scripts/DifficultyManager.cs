using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public int currentLevel = 0; // Current difficulty level, starting at 0

    private EnemySpawner enemySpawner; // Reference to the EnemySpawner component to manage guard spawning

    void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>(); // Find the EnemySpawner component in the scene to control guard spawning based on difficulty level

        enemySpawner.SpawnGuard(currentLevel); // Spawn guards for the initial difficulty level when the game starts
        
    }

    public void IncreaseDifficulty()
    {
        if (currentLevel < 4) // Check if the current level is less than the maximum level (4 in this case)
        {
            currentLevel++; // Increment the difficulty level by 1
            enemySpawner.SpawnGuard(currentLevel); // Spawn guards for the new difficulty level to increase the challenge for the player
        }
    }
}
