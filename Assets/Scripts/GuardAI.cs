using UnityEngine;
using UnityEngine.AI; // Include the UnityEngine.AI namespace to use NavMeshAgent for pathfinding and movement

public class GuardAI : MonoBehaviour
{
    private Transform player; // Reference to the player's transform to track their position
    private NavMeshAgent agent; // Reference to the NavMeshAgent component for pathfinding and movement
    public float chaseDistance = 25f; // Distance at which the guard will start chasing the player
    public float chaseSpeed = 18f; // Multiplier to increase the guard's speed when chasing
    public float normalSpeed = 5f; // Normal speed of the guard when not chasing
    public float wanderRadius = 20f; // Radius within which the guard will wander when not chasing
    private PlayerHealth playerHealth; // Reference to the PlayerHealth component to manage the player's health when colliding with the guard

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player game object by tag and get its transform to track the player's position
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component attached to the guard
        playerHealth = player.GetComponent<PlayerHealth>(); // Get the PlayerHealth component attached to the player
    }

    void Update()
    {
        ChasePlayer();
    }

    public void Wander()
    {
        if (agent.remainingDistance < 1f) // Check if the NavMeshAgent has reached its current destination (within a small threshold)
        {
            Vector3 randomPos = transform.position + Random.insideUnitSphere * wanderRadius; // Generate a random position within a sphere of the defined wander radius around the guard's current position
            NavMeshHit hit; // Variable to store the result of the NavMesh sampling
            NavMesh.SamplePosition(randomPos, out hit, wanderRadius, NavMesh.AllAreas); // Sample the NavMesh to find a valid position for the guard to move to within the wander radius
            agent.SetDestination(hit.position); // Set the NavMeshAgent's destination to the valid position found by sampling the NavMesh, allowing the guard to wander around when not chasing the player
        }
    }

    public void ChasePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position); // Calculate the distance between the guard and the player

        if (distanceToPlayer < chaseDistance) // Check if the player is within the chase distance
        {
            agent.speed = chaseSpeed; // Set the NavMeshAgent's speed to the chase speed
            agent.SetDestination(player.position); // Set the NavMeshAgent's destination to the player's position
        }
        else
        {
            agent.speed = normalSpeed; // Set the NavMeshAgent's speed to the normal speed when not chasing
            Wander(); // Call the method to wander around when the player is not within the chase distance
        }
    }
            
    public void Vanish()
    {
        Debug.Log("Guard vanished."); // Log a message to the console indicating that the guard has vanished. The actual logic for making the guard vanish (e.g., disabling the guard's renderer, collider, and AI behavior) would be implemented here.
        Destroy(gameObject); // Destroy the guard game object to simulate vanishing. In a more complex implementation, you might want to disable the guard's components instead of destroying it, depending on how you want the vanish effect to work in your game.
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the guard collides with the player
        {
            playerHealth.LoseLife(); // Call the LoseLife method on the PlayerHealth component to reduce the player's health

            Debug.Log("Guard collided with the player! Implement collision logic here."); // Placeholder for collision logic, can be replaced with actual functionality as needed
        }
    }
}
