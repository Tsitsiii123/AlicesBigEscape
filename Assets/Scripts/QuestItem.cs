using UnityEngine;

public class QuestItem : MonoBehaviour
{
    private QuestManager questManager; // Reference to the QuestManager script to access the current quest step and status for this quest item, can be set in the Unity editor to link it to the appropriate quest manager object in the scene

    [Header("Quest Item Settings")]
    public int requiredStateToCollect; // Variable to specify the required quest state for the player to be able to collect this item, can be set in the Unity editor to define which quest state the player must be in to interact with this item and progress the quest
    public int stateAfterCollection; // Variable to specify the quest state to transition to after collecting this item, can be set in the Unity editor to define which quest state should be set in the quest manager after the player collects this item to progress the quest
    
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
                questManager.storyState = stateAfterCollection; // Update the story state to the next state after collecting this item
                gameObject.SetActive(false); // Deactivate this quest item in the scene after it has been found by the player to visually indicate that it has been collected and to prevent further interactions with it
            }
        }
    }
}
