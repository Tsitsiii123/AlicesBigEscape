using UnityEngine;

public class QuestItem : MonoBehaviour
{
    private QuestManager questManager; // Reference to the QuestManager script to access the current quest step and status for this quest item, can be set in the Unity editor to link it to the appropriate quest manager object in the scene

    [Header("Quest Item Settings")]
    public string itemName; // Variable to store the name of this quest item, can be set in the Unity editor to define the name of the item that will be displayed in the UI when collected
    public int requiredStateToCollect; // Variable to specify the required quest state for the player to be able to collect this item, can be set in the Unity editor to define which quest state the player must be in to interact with this item and progress the quest
    public int stateAfterCollection; // Variable to specify the quest state to transition to after collecting this item, can be set in the Unity editor to define which quest state should be set in the quest manager after the player collects this item to progress the quest
    
    public bool isMultipleItem; // Variable to indicate whether this quest item can be collected multiple times, can be set in the Unity editor to define whether the player can collect this item more than once or if it should only be collected once to progress the quest
    public int totalItemsNeeded; // Variable to specify the total number of this quest item needed to complete the quest, can be set in the Unity editor to define how many of this item the player must collect to progress the quest
    
    [Header("Timer Settings")]
    public float minTime; // Variable to specify the minimum time for the timer associated with this quest item, can be set in the Unity editor to define the lower bound of the timer duration for this quest step
    public float maxTime; // Variable to specify the maximum time for the timer associated with this quest item, can be set in the Unity editor to define the upper bound of the timer duration for this quest step

    void Start()
    {
        questManager = FindObjectOfType<QuestManager>(); // Find the QuestManager script in the scene to initialize the reference for this quest item
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player has entered the trigger collider of this quest item to determine if we should update the quest status and dialogue for this item
        {
            if (questManager.storyState == requiredStateToCollect) // Check if the player is in the required state to collect this item
            {
                if (isMultipleItem) // Check if this quest item can be collected multiple times (ex. carrots)
                {
                    questManager.collectedAmount++; // Increment the collected amount for this quest item in the quest manager to track how many of this item the player has collected
                    
                    string msg = "You collected " + itemName + "! (" + questManager.collectedAmount + "/" + totalItemsNeeded + ")"; // Create a message to display the collected item and the current count for this quest item
                    questManager.ShowCollectionMessage(msg); // Display the collection message in the UI for the quest manager
                    
                    questManager.StopAndCheckTimer(minTime, maxTime); // Stop the current timer and check if the player has collected enough of this item to complete the quest, using the specified minimum and maximum time for the timer associated with this quest item

                    if (questManager.collectedAmount >= totalItemsNeeded) // Check if the player has collected enough of this item to complete the quest
                    {
                        questManager.storyState = stateAfterCollection; // Update the story state to the next state after collecting enough of this item
                        questManager.collectedAmount = 0; // Reset the collected amount for this quest item in the quest manager
                    }
                }

                else // Logic for single collection items (ex. book)
                {
                    string msg = "You collected " + itemName + "!"; // Create a message to display the collected item for this quest item
                    questManager.ShowCollectionMessage(msg); // Display the collection message in the UI for the quest manager
                    
                    questManager.StopAndCheckTimer(minTime, maxTime); // Stop the current timer and check if the player has collected enough of this item to complete the quest, using the specified minimum and maximum time for the timer associated with this quest item

                    questManager.storyState = stateAfterCollection; // Update the story state to the next state after collecting this item
                }

                gameObject.SetActive(false); // Deactivate this quest item in the scene after it has been found by the player to visually indicate that it has been collected and to prevent further interactions with it
            }
        }
    }
}
