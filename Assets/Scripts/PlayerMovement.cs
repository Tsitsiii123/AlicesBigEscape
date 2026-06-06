using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed at which the player moves
    public float mouseSensitivity = 100f; // Sensitivity for mouse movement to control player rotation
    private Rigidbody rb; // Reference to the player's Rigidbody component
    private GameManager gameManager; // Reference to the GameManager to check game state


    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component attached to the player
        gameManager = FindObjectOfType<GameManager>(); // Find the GameManager in the scene to access its properties and methods
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen for better control during gameplay
    }
    void Update()
    {
        HandleInput(); // Call the method to handle player input every frame
        HandleMouseLook(); // Call the method to handle mouse look every frame
    }

    public void HandleInput()
    {
        if (gameManager.isGameOver || gameManager.isPaused) // Check if the game is over or paused to prevent any input processing
        {
            return; // If the game is over or paused, exit the method and do not process input
        }

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
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 20f; // Increase move speed when Left Shift is held down (sprint)
        }
        else
        {
            moveSpeed = 10f; // Reset move speed to normal when Left Shift is not held down
        }

        Vector3 finalMovement = (transform.forward * direction.z + transform.right * direction.x).normalized; // Calculate the final movement vector by combining the forward and right directions of the player with the input direction
        rb.linearVelocity = new Vector3(finalMovement.x * moveSpeed, rb.linearVelocity.y, finalMovement.z * moveSpeed); // Set the player's velocity based on the calculated movement vector and the defined move speed, while preserving the current vertical velocity (y-axis)
    }

    public void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; // Get horizontal mouse movement and apply sensitivity and time scaling

        transform.Rotate(Vector3.up * mouseX); // Rotate the player horizontally based on mouse movement
    }
}
