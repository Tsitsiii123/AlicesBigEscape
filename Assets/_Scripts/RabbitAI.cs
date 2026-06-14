using Microsoft.VisualBasic;
using UnityEngine;
using UnityEngine.AI;

public class RabbitAI : MonoBehaviour
{   
    private QuestManager questManager; // Reference to the QuestManager to check if the player has completed the quest to find the rabbit
    public Transform player; // Reference to the player's transform to track their position
    private NavMeshAgent agent; // Reference to the NavMeshAgent component for pathfinding and movement
    public float fleeDistance = 10f; // Distance at which the rabbit will start fleeing from the player
    private Rigidbody playerRb; // Reference to the player's Rigidbody component for potential future use (e.g., checking player velocity)
    public float speedMultiplier = 1.2f; // Multiplier to increase the rabbit's speed when fleeing
    public float normalSpeed = 5f; // Normal speed of the rabbit when not fleeing
    public float wanderRadius = 50f; // Radius within which the rabbit will wander when not fleeing
    public bool canHeal = true; // Flag to determine if the rabbit can heal the player upon collision, can be set to false if healing is not desired
    public float cooldownDuration = 60f; // Duration of the cooldown period after healing, during which the rabbit cannot heal again, can be adjusted as needed
    public float cooldownTimer = 0f; // Timer to track the cooldown period for healing, initialized to 0

    void Start()
    {
        questManager = FindObjectOfType<QuestManager>(); // Find the QuestManager in the scene to check if the player has completed the quest to find the rabbit
        agent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component attached to the rabbit
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player game object by tag and get its transform to track the player's position
        playerRb = player.GetComponent<Rigidbody>(); // Get the Rigidbody component attached to the player for potential future use
    }

    void Update()
    {
        FleeFromPlayer();
        
        if (!canHeal) // Check if the rabbit is currently on cooldown and cannot heal
        {
            cooldownTimer -= Time.deltaTime; // Increment the cooldown timer by the time elapsed since the last frame

            if (cooldownTimer <= 0f) // Check if the cooldown timer has reached zero
            {
                canHeal = true; // Reset the canHeal flag to true, allowing the rabbit to heal again after the cooldown period has ended
                cooldownTimer = 0f; // Reset the cooldown timer to 0 for the next healing cycle
            }
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the rabbit collides with the player
        {
            agent.isStopped = true; // Stop the NavMeshAgent's movement when colliding with the player
            transform.LookAt(player.position); // Make the rabbit look towards the player upon collision

            Debug.Log("Rabbit collided with the player! Implement collision logic here."); // Placeholder for collision logic, can be replaced with actual functionality as needed
        
            if (canHeal) // Check if the rabbit collides with the player and is currently able to heal
            {
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>(); // Get the PlayerHealth component from the player to access health-related functionality

                if (playerHealth != null) // Check if the PlayerHealth component was successfully retrieved
                {
                    playerHealth.EatCake(); // Call the EatCake method on the player's health to restore health when colliding with the rabbit, simulating a healing effect
                    Debug.Log("Ha ha! That was fun! Well done catching me! Here is a small reward!");
                    canHeal = false; // Set the canHeal flag to false to start the cooldown period after healing
                    cooldownTimer = cooldownDuration; // Reset the cooldown timer to the defined cooldown duration for tracking when the rabbit can heal again after this collision
                }

                string msg = "Ha ha! That was fun! Well done catching me! Here is a small reward!"; // Create a message to display when the rabbit heals the player
                questManager.UpdateDialogue(msg); // Call the UpdateDialogue method on the QuestManager to display the message in the UI, providing feedback to the player about the interaction with the rabbit and the reward received
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the rabbit stops colliding with the player
        {
            agent.isStopped = false; // Resume the NavMeshAgent's movement when no longer colliding with the player

            Debug.Log("Rabbit stopped colliding with the player! Implement logic for when the rabbit can move again."); // Placeholder for logic to handle when the rabbit can move again after colliding with the player, can be replaced with actual functionality as needed  
        }
    }
}
