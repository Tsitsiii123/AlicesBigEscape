using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed at which the player moves
    private Rigidbody rb; // Reference to the player's Rigidbody component

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component attached to the player
    }
    void Update()
    {
        HandleInput(); // Call the method to handle player input every frame
    }

    public void HandleInput()
    {
        //if (gameManager.IsGameOver) // Check if the game is over to prevent any input processing after the game has ended
        //{
        //    return; // If the game is over, exit the method and do not process input
        //}

        Vector3 direction = Vector3.zero; // Initialize a direction vector to store the movement direction based on player input

        if (Input.GetKey(KeyCode.W))
        {
            direction.z = 1f; // Move forward (positive z direction)
        }

        if (Input.GetKey(KeyCode.S))
        {
            direction.z = -1f; // Move backward (negative z direction)
        }

        if (Input.GetKey(KeyCode.A))
        {
            direction.x = -1f; // Move left (negative x direction)
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction.x = 1f; // Move right (positive x direction)
        }

        rb.linearVelocity = new Vector3(direction.x * moveSpeed, rb.linearVelocity.y, direction.z * moveSpeed); // Set the player's velocity based on the input direction and move speed while preserving the y velocity for gravity

    }
}
