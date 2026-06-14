using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    [Header("Dialogue UI Elements")]
    public TextMeshProUGUI dialogueText; // Reference to the TextMeshProUGUI component that will display the dialogue text for the quest manager, can be set in the Unity editor to link it to the appropriate UI text element in the scene
    public GameObject dialoguePanel; // Reference to the GameObject that represents the dialogue panel for the quest manager, can be set in the Unity editor to link it to the appropriate UI panel in the scene

    [Header("Collection UI Elements")]
    public TextMeshProUGUI collectionText; // Reference to the TextMeshProUGUI component that will display the collected items text for the quest manager, can be set in the Unity editor to link it to the appropriate UI text element in the scene
    public GameObject collectionPanel; // Reference to the GameObject that represents the collection panel for the quest manager, can be set in the Unity editor to link it to the appropriate UI panel in the scene

    [Header("Story Progress")]
    public int storyState = 0; // Variable to track the current state of the story for the quest manager

    public int collectedAmount = 0; // Variable to track the number of items collected by the player for the quest manager

    [Header("Difficulty System")]
    public DifficultyManager difficultyManager; // Reference to the DifficultyManager component to control the difficulty level of the game

    private float currentTimer = 0f; // Variable to track the current time elapsed for the timer in the quest manager
    private bool isTimerRunning = false; // Variable to track whether the timer is currently running for the quest manager

    void Update()
    {
        if (isTimerRunning) // Check if the timer is currently running to determine if we should update the timer for the quest manager
        {
            currentTimer += Time.deltaTime; // Increment the current timer by the time elapsed since the last frame to track the total time elapsed for the quest manager
        }
    }

    public void UpdateDialogue(string message)
    {
        dialoguePanel.SetActive(true); // Activate the dialogue panel to make it visible in the UI for the quest manager
        dialogueText.text = message; // Update the text of the dialogueText component to display the provided message for the quest manager
    
        CancelInvoke("HideDialogue"); // Cancel any previously scheduled invocation of the HideDialogue method to prevent it from being called multiple times and ensure that the dialogue panel remains visible for the appropriate duration
        Invoke("HideDialogue", 2f); // Schedule the HideDialogue method to be called after a delay of 2 seconds to automatically hide the dialogue panel after displaying the message for the quest manager
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false); // Deactivate the dialogue panel to hide it from the UI for the quest manager
    }

    public void ShowCollectionMessage(string message)
    {
        collectionPanel.SetActive(true); // Activate the collection panel to make it visible in the UI for the quest manager
        collectionText.text = message; // Update the text of the collectionText component to display the provided message for the quest manager
    
        CancelInvoke("HideCollectionMessage"); // Cancel any previously scheduled invocation of the HideCollectionMessage method to prevent it from being called multiple times and ensure that the collection panel remains visible for the appropriate duration
        Invoke("HideCollectionMessage", 1f); // Schedule the HideCollectionMessage method to be called after a delay of 1 seconds to automatically hide the collection panel after displaying the message for the quest manager
    }

    public void HideCollectionMessage()
    {
        collectionPanel.SetActive(false); // Deactivate the collection panel to hide it from the UI for the quest manager
    }

    public void StartTimer()
    {
        currentTimer = 0f; // Reset the current timer to 0 to start tracking the time elapsed for the quest manager
        isTimerRunning = true; // Set the isTimerRunning flag to true to indicate that the timer is currently running for the quest manager
    }

    public void StopAndCheckTimer(float minTime, float maxTime)
    {
        isTimerRunning = false; // Set the isTimerRunning flag to false to indicate that the timer is no longer running for the quest manager
        
        if (currentTimer < minTime) // Check if the elapsed time is less than the minimum time
        {
            difficultyManager.IncreaseDifficulty(); // Increase the difficulty level if the player completed the task too quickly, making the game more challenging for the player
            AddSystemMessage("Difficulty increased."); // Show a message to the player indicating that the difficulty has been increased due to completing the task too quickly
        }
        else if (currentTimer > maxTime) // Check if the elapsed time is greater than the maximum time
        {
            difficultyManager.DecreaseDifficulty(); // Decrease the difficulty level if the player took too long to complete the task, making the game less challenging for the player  
            AddSystemMessage("Difficulty decreased."); // Show a message to the player indicating that the difficulty has been decreased due to taking too long to complete the task
        }

        currentTimer = 0f; // Reset the current timer to 0 after checking the elapsed time for the quest manager
        isTimerRunning = true; // Set the isTimerRunning flag to true to indicate that the timer is currently running for the quest manager, allowing it to continue tracking time for future tasks
    }

    public void AddSystemMessage(string message)
    {
        collectionText.text += "\n" + message; // Append the provided message to the existing text in the collectionText component, allowing multiple system messages to be displayed in the UI for the quest manager
    }
}
