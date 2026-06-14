using UnityEngine;
using UnityEngine.AI;

public class QuestTrail : MonoBehaviour
{
    private LineRenderer line; // Reference to the LineRenderer component to draw the trail for the quest, can be set in the Unity editor to link it to the appropriate LineRenderer component in the scene
    private NavMeshPath path; // Reference to the NavMeshPath component to calculate the path
    private QuestManager questManager; // Reference to the QuestManager script to access the current quest step and status for this quest trail, can be set in the Unity editor to link it to the appropriate quest manager object in the scene
    private bool isTrailActive = false; // Variable to track whether the trail is currently active for the quest, can be used to enable or disable the trail based on the current quest state

    [Header("Trail Targets")]
    public Transform rabbitTarget; // Reference to the Transform component of the rabbit target for the quest trail, can be set in the Unity editor to link it to the appropriate target object in the scene
    public Transform bookTarget; // Reference to the Transform component of the book target for the quest trail, can be set in the Unity editor to link it to the appropriate target object in the scene
    public Transform potTarget; // Reference to the Transform component of the pot target for the quest trail, can be set in the Unity editor to link it to the appropriate target object in the scene
    void Start()
    {
        line = GetComponent<LineRenderer>(); // Get the LineRenderer component attached to this GameObject to initialize the reference for the quest trail
        path = new NavMeshPath(); // Create a new instance of the NavMeshPath component to initialize the reference for the quest trail
        questManager = FindObjectOfType<QuestManager>(); // Find the QuestManager script in the scene to initialize the reference for this quest trail
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) // Check if the player has pressed the "H" key to toggle the trail for the quest
        {
            isTrailActive = !isTrailActive; // Toggle the isTrailActive flag to enable or disable the trail for the quest
        }

        if (!isTrailActive) // Check if the trail is not active to determine if we should skip the trail drawing logic for the quest
        {
            line.positionCount = 0; // Set the position count of the LineRenderer to 0 to clear the trail if it is not active for the quest
            return; // Exit the Update method early to skip the trail drawing logic for the quest
        }

        Transform currentTarget = null; // Initialize a variable to hold the current target for the quest trail, starting with null

        if (questManager.storyState == 1 || questManager.storyState == 6 || questManager.storyState == 9) // Check if the current story state is 1, 6, or 9 to determine if the rabbit target should be used for the quest trail
        {
            currentTarget = rabbitTarget; // Set the current target to the rabbit target for the quest trail
        }
        else if (questManager.storyState == 2) // Check if the current story state is 2, 7, or 10 to determine if the book target should be used for the quest trail
        {
            currentTarget = bookTarget; // Set the current target to the book target for the quest trail
        }
        else if (questManager.storyState == 8) // Check if the current story state is 8 to determine if the rabbit target should be used for the quest trail
        {
            currentTarget = potTarget; // Set the current target to the pot target for the quest trail
        }
        else if (questManager.storyState == 5) // Check if the current story state is 5 to determine if the rabbit target should be used for the quest trail
        {
            currentTarget = FindClosestCarrot(); // Set the current target to the closest carrot for the quest trail by calling the FindClosestCarrot method
        }

        if (currentTarget != null && currentTarget.gameObject.activeSelf) // Check if the current target is not null and is active in the scene to determine if we should calculate and draw the path for the quest trail
        {
            Vector3 exactTargetPosition = currentTarget.GetComponent<Collider>().bounds.center; // Get the exact position of the current target by accessing its Collider component and retrieving the center of its bounds for the quest trail
            DrawPathToTarget(exactTargetPosition); // Call the DrawPathToTarget method to calculate and draw the path to the exact target position for the quest trail
        }
            
        else
        {
            line.positionCount = 0; // Set the position count of the LineRenderer to 0 to clear the trail if there is no valid target for the quest trail
        }
    }

    void DrawPathToTarget(Vector3 targetPosition)
    {
        NavMeshHit startHit; // Variable to hold the NavMeshHit information for the starting position of the path calculation for the quest trail
        NavMeshHit endHit; // Variable to hold the NavMeshHit information for the ending position of the path calculation for the quest trail

        bool validStart = NavMesh.SamplePosition(transform.position, out startHit, 5f, NavMesh.AllAreas); // Sample the NavMesh at the current position to find a valid starting point for the path calculation for the quest trail
        bool validEnd = NavMesh.SamplePosition(targetPosition, out endHit, 5f, NavMesh.AllAreas); // Sample the NavMesh at the target position to find a valid ending point for the path calculation for the quest trail

        if (validStart && validEnd)
        {
            NavMesh.CalculatePath(startHit.position, endHit.position, NavMesh.AllAreas, path); // Calculate the path from the current position to the target position using the NavMesh to determine the path for the quest trail

            if (path.corners.Length > 1) // Check if the calculated path has more than one corner to determine if we should draw the trail for the quest
            {
                line.positionCount = path.corners.Length; // Set the position count of the LineRenderer to the number of corners in the calculated path to prepare for drawing the trail
                line.SetPositions(path.corners); // Set the positions of the LineRenderer to the corners of the calculated path to draw the trail for the quest
            }
            
            else
            {
                line.positionCount = 0; // Set the position count of the LineRenderer to 0 to clear the trail if there are not enough corners in the calculated path for the quest
            }
        }
    }

    public Transform FindClosestCarrot()
    {
        GameObject[] carrots = GameObject.FindGameObjectsWithTag("Carrot"); // Find all GameObjects in the scene with the tag "Carrot" to get an array of all carrot objects for the quest trail
        GameObject closestCarrot = null; // Initialize a variable to hold the closest carrot, starting with null
        float closestDistance = Mathf.Infinity; // Initialize a variable to hold the closest distance, starting with infinity

        foreach (GameObject carrot in carrots) // Loop through each carrot in the array of carrots to find the closest one for the quest trail
        {
            if (carrot.activeSelf) // Check if the carrot is active in the scene to determine if it should be considered for finding the closest carrot
            {
                float distance = Vector3.Distance(transform.position, carrot.transform.position); // Calculate the distance from the current position to the carrot's position to determine how far away it is for the quest trail

                if (distance < closestDistance) // Check if the calculated distance is less than the closest distance found so far to update the closest carrot
                {
                    closestDistance = distance; // Update the closest distance to the newly found closer distance for the quest trail
                    closestCarrot = carrot; // Update the closest carrot to the newly found closer carrot for the quest trail
                }
            }
        }

        return closestCarrot != null ? closestCarrot.transform : null; // Return the Transform of the closest carrot if one was found, otherwise return null for the quest trail
    }


}
