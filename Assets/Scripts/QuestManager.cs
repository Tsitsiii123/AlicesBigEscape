using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI dialogueText; // Reference to the TextMeshProUGUI component that will display the dialogue text for the quest manager, can be set in the Unity editor to link it to the appropriate UI text element in the scene
    public GameObject dialoguePanel; // Reference to the GameObject that represents the dialogue panel for the quest manager, can be set in the Unity editor to link it to the appropriate UI panel in the scene

    [Header("Story Progress")]
    public int storyState = 0; // Variable to track the current state of the story for the quest manager

    public void UpdateDialogue(string message)
    {
        dialoguePanel.SetActive(true); // Activate the dialogue panel to make it visible in the UI for the quest manager
        dialogueText.text = message; // Update the text of the dialogueText component to display the provided message for the quest manager
    
        CancelInvoke("HideDialogue"); // Cancel any previously scheduled invocation of the HideDialogue method to prevent it from being called multiple times and ensure that the dialogue panel remains visible for the appropriate duration
        Invoke("HideDialogue", 5f); // Schedule the HideDialogue method to be called after a delay of 5 seconds to automatically hide the dialogue panel after displaying the message for the quest manager
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false); // Deactivate the dialogue panel to hide it from the UI for the quest manager
    }

}
