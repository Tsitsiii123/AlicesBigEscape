using UnityEngine;
using UnityEngine.AI;

public class RabbitAI : MonoBehaviour
{
    public Transform player; // Reference to the player's transform to track their position
    private NavMeshAgent agent; // Reference to the NavMeshAgent component for pathfinding and movement
    public float fleeDistance = 10f; // Distance at which the rabbit will start fleeing from the player
    private Rigidbody playerRb; // Reference to the player's Rigidbody component for potential future use (e.g., checking player velocity)
    public float speedMultiplier = 1.2f; // Multiplier to increase the rabbit's speed when fleeing
    public float normalSpeed = 5f; // Normal speed of the rabbit when not fleeing
    public float wanderRadius = 50f; // Radius within which the rabbit will wander when not fleeing

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component attached to the rabbit
        playerRb = player.GetComponent<Rigidbody>(); // Get the Rigidbody component attached to the player for potential future use
    }

    void Update()
    {
        FleeFromPlayer();
    }

    public void FleeFromPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position); // Calculate the distance between the rabbit and the player

        if (distanceToPlayer < fleeDistance) // Check if the player is within the flee distance
        {
            Vector3 fleeDirection = (transform.position - player.position).normalized; // Calculate the direction to flee by taking the vector from the player to the rabbit and normalizing it
            Vector3 newPos = transform.position + fleeDirection * fleeDistance; // Calculate the target position to flee to by moving in the flee direction by the defined flee distance
            agent.speed = Mathf.Max(normalSpeed, playerRb.linearVelocity.magnitude * speedMultiplier); // Set the NavMeshAgent's speed to either the normal speed or a speed based on the player's velocity multiplied by the speed multiplier, whichever is greater, to ensure the rabbit can effectively flee from the player
            agent.SetDestination(newPos); // Set the NavMeshAgent's destination to the calculated flee target position
        }

        else
        {
            agent.speed = normalSpeed; // Set the NavMeshAgent's speed to the normal speed when not fleeing
            Wander(); // Call the method to wander around when the player is not within the flee distance
        }
    }

    public void Wander()
    {
        if (agent.remainingDistance < 1f) // Check if the NavMeshAgent has reached its current destination (within a small threshold)
        {
            Vector3 randomPos = transform.position + Random.insideUnitSphere * wanderRadius; // Generate a random position within a sphere of the defined wander radius around the rabbit's current position
            NavMeshHit hit; // Variable to store the result of the NavMesh sampling
            NavMesh.SamplePosition(randomPos, out hit, wanderRadius, NavMesh.AllAreas); // Sample the NavMesh to find a valid position for the rabbit to move to within the wander radius
            agent.SetDestination(hit.position); // Set the NavMeshAgent's destination to the valid position found by sampling the NavMesh, allowing the rabbit to wander around when not fleeing from the player
        }
    }
}
