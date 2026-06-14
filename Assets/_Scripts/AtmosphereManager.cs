using UnityEngine;

public class AtmosphereManager : MonoBehaviour
{
    public Light directionalLight; // Reference to the directional light in the scene, can be set in the Unity editor to link it to the appropriate light object in the scene
    public float[] fogStartDistances = new float[5]; // Array to store the fog start distances for each difficulty level, can be set in the Unity editor to define how far the fog should start based on the current difficulty level
    public float[] fogEndDistances = new float[5]; // Array to store the fog end distances for each difficulty level, can be set in the Unity editor to define how far the fog should extend based on the current difficulty level
    public float[] lightIntensities = new float[5]; // Array to store the light intensities for each difficulty level, can be set in the Unity editor to define how bright the directional light should be based on the current difficulty level
    public Color[] lightColors = new Color[5]; // Array to store the light colors for each difficulty level, can be set in the Unity editor to define the color of the directional light based on the current difficulty level

    void Start()
    {
        RenderSettings.fog = true; // Enable fog in the scene to create a more atmospheric effect based on the current difficulty level
        SetDifficultyLevel(0); // Set the initial difficulty level to 0 (or the desired starting level) to initialize the fog and light settings based on the defined values for that level
    }

    public void SetDifficultyLevel(int levelIndex)
    {
        RenderSettings.fogStartDistance = fogStartDistances[levelIndex]; // Set the fog start distance based on the defined values for the current difficulty level using the provided index
        RenderSettings.fogEndDistance = fogEndDistances[levelIndex]; // Set the fog end distance based on the defined values for the current difficulty level using the provided index
        directionalLight.intensity = lightIntensities[levelIndex]; // Set the light intensity of the directional light based on the defined values for the current difficulty level using the provided index
        directionalLight.color = lightColors[levelIndex]; // Set the light color of the directional light based on the defined values for the current difficulty level using the provided index
    }
}
