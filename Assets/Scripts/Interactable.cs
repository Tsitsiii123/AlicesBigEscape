using UnityEngine;

public class Interactable : MonoBehaviour
{
    public StoryAction[] actionsPerStep; // Array to store the dialogue for each quest step for this interactable object, can be set in the Unity editor to define the dialogue for each step of the quest when interacting with this object
    private QuestManager questManager;

    void Start()
    {
        questManager = FindObjectOfType<QuestManager>();
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")) // Check if the player has collided with this interactable object to determine if we should update the quest dialogue and status based on the player's interaction with this object
        {   
            int state = questManager.storyState; // Get the current story state from the quest manager to determine which dialogue to display for this interactable object based on the player's progress in the story

            if (state >= actionsPerStep.Length)
            {
                state = actionsPerStep.Length - 1; // If the current story state exceeds the number of defined actions, set it to the last action to ensure we have a valid dialogue to display for this interactable object based on the player's progress in the story
            }

            if (state < actionsPerStep.Length) // Check if the current story state is within the bounds of the actionsPerStep array to ensure we have a valid dialogue to display for this interactable object based on the player's progress in the story
            {
                StoryAction currentAction = actionsPerStep[state]; // Get the StoryAction for the current story state from the actionsPerStep array to determine which dialogue and item to spawn for this interactable object based on the player's progress in the story

                if (currentAction.message != "") // Check if there is a dialogue message defined for this StoryAction to determine if we should update the dialogue for this interactable object based on the player's interaction with it
                {
                    questManager.UpdateDialogue(currentAction.message); // Update the dialogue in the quest manager with the message from the current StoryAction to provide feedback to the player based on their interaction with this interactable object
                }

                if (currentAction.itemToSpawn != null) // Check if there is an item to spawn defined for this StoryAction to determine if we should spawn an item for the player based on their interaction with this interactable object
                {
                    currentAction.itemToSpawn.SetActive(true); // Activate the item to spawn defined in the current StoryAction to make it appear in the scene for the player based on their interaction with this interactable object
                }

                if (currentAction.itemToHide != null) // Check if there is an item to hide defined for this StoryAction to determine if we should hide an item for the player based on their interaction with this interactable object
                {
                    currentAction.itemToHide.SetActive(false); // Deactivate the item to hide defined in the current StoryAction to make it disappear from the scene for the player based on their interaction with this interactable object
                }

                if (currentAction.changesState) // Check if this StoryAction is defined to change the story state to determine if we should update the story state in the quest manager based on the player's interaction with this interactable object
                {
                    questManager.storyState = currentAction.nextState; // Set the story state in the quest manager to the next state defined in the current StoryAction to progress the story based on the player's interaction with this interactable object
                    questManager.StartTimer(); // Start the timer in the quest manager to track the time elapsed for the current quest step based on the player's interaction with this interactable object
                }
            }
        }
    }        
}

[System.Serializable]
public class StoryAction
{
    [TextArea(2, 5)]
    public string message; // Variable to store the dialogue message for this quest dialogue, can be set in the Unity editor to define the dialogue that should be displayed when this dialogue is triggered
    
    public bool changesState; // Variable to indicate whether this quest dialogue should change the story state when triggered, can be set in the Unity editor to define whether interacting with this dialogue should progress the story state in the quest manager
    public int nextState; // Variable to specify the next story state to transition to when this quest dialogue is triggered, can be set in the Unity editor to define which story state should be set in the quest manager when this dialogue is triggered
    public GameObject itemToSpawn; // Reference to the GameObject that represents the item to spawn for this quest dialogue, can be set in the Unity editor to link it to the appropriate item prefab that should be spawned when this dialogue is triggered
    public GameObject itemToHide; // Reference to the GameObject that represents the item to hide for this quest dialogue, can be set in the Unity editor to link it to the appropriate item prefab that should be hidden when this dialogue is triggered
}
