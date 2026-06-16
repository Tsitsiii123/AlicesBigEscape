using UnityEngine;

public class ContinuousMusic : MonoBehaviour
{
    private static ContinuousMusic instance; // Static instance to ensure only one instance of ContinuousMusic exists across scenes

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Ensure the music persists across scene changes
            GetComponent<AudioSource>().ignoreListenerPause = true;
        }
        else
        {
            Destroy(gameObject); // Destroy any duplicate instances
        }
    }
}
