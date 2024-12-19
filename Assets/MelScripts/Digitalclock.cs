using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DigitalClock : MonoBehaviour
{
    public TMP_Text timeText;   // Reference to the TMP_Text component to display time
    public float timeScale = 5f; // Real-time to game-time scaling factor (2 min real = 10 min game)
    private float gameTimeInMinutes = 0f;  // Tracks the game time in minutes
    private int gameDay = 1;   
    public Light directionalLight; // Reference to the Directional Light
    public float fullDayLength = 1440f; // Total minutes for a full day (24 hours = 1440 minutes)
    public float dayLightIntensity = 1f; // Intensity of the light during the day
    public float nightLightIntensity = 0.1f; // Intensity of the light during the night
    public Transform lightTransform;  // Reference to the Directional Light's Transform

    private float dayStartAngleY = 45f; // Rotation angle for daytime on Y-axis
    private float nightStartAngleY = -90f; // Rotation angle for nighttime on Y-axis
    private float dayStartAngleX = -30f; // Pitch angle for daytime (morning)
    private float noonAngleX = 90f; // Pitch angle for midday (high noon)
    private float nightStartAngleX = -60f; // Pitch angle for nighttime (evening)

    private float currentRotationY = 0f; // Current Y-axis rotation
    private float currentRotationX = 0f; // Current X-axis pitch

    void Start()
    {
        if (timeText == null)
            timeText = GetComponent<TMP_Text>();  
        
        if (directionalLight == null)
            directionalLight = FindObjectOfType<Light>();  // Automatically find the Directional Light in the scene
        
        if (lightTransform == null)
            lightTransform = directionalLight.transform;  // Automatically grab the Transform of the Directional Light
    }

    void Update()
    {
        // Increase game time by the scaled real-time deltaTime (adjust this based on timeScale)
        gameTimeInMinutes += Time.deltaTime * timeScale;

        // If game time exceeds 1440 minutes (24 hours), increment the day
        if (gameTimeInMinutes >= fullDayLength)
        {
            gameTimeInMinutes -= fullDayLength; // Subtract 1440 minutes (24 hours) from game time
            gameDay++; // New day
        }

        // Convert game time to hours and minutes
        int gameHours = Mathf.FloorToInt(gameTimeInMinutes / 60);   // Get the hours (from total minutes)
        int gameMinutes = Mathf.FloorToInt(gameTimeInMinutes % 60); // Get the minutes

        // Convert to 12-hour format and determine AM/PM
        string period = "AM";
        if (gameHours >= 12)
        {
            period = "PM";
            if (gameHours > 12)
                gameHours -= 12; // Convert to 12-hour format
        }
        else if (gameHours == 0)
        {
            gameHours = 12; // Handle midnight as 12 AM
        }

        // Format the game time as "h h m m AM/PM"
        string gameTime = string.Format("{0} h {1:D2} m {2}", gameHours, gameMinutes, period);

        // Format the game day (e.g., Day 1)
        string dayInfo = "Day " + gameDay;

        // Set the combined time and day text
        timeText.text = gameTime + "\n" + dayInfo;

        // Decrease light intensity based on game time (without rotating the light)
        AdjustLightIntensity(gameHours);

        // Smoothly adjust the light rotation (both Y and X)
        SmoothLightRotation(gameTimeInMinutes);
    }

    // Function to adjust the light intensity based on game time (in hours)
    private void AdjustLightIntensity(int hours)
    {
        // Set light intensity for daytime (6 AM to 6 PM) and nighttime (6 PM to 6 AM)
        if (hours >= 6 && hours < 18) // Daytime (6 AM to 6 PM)
        {
            // Increase intensity during the day
            directionalLight.intensity = Mathf.Lerp(nightLightIntensity, dayLightIntensity, (hours - 6) / 12f);
        }
        else // Nighttime (6 PM to 6 AM)
        {
            // Decrease intensity during the night
            directionalLight.intensity = Mathf.Lerp(dayLightIntensity, nightLightIntensity, (hours - 18 + 24) / 12f);
        }
    }

    // Smoothly adjust the light's rotation (both Y and X)
    private void SmoothLightRotation(float timeInMinutes)
    {
        // Calculate the percentage of the day that has passed (0 to 1)
        float dayPercentage = (timeInMinutes % fullDayLength) / fullDayLength;

        // Y-axis rotation (the rotation of the light around the Y-axis, simulating sunrise to sunset)
        float targetRotationY = Mathf.Lerp(dayStartAngleY, nightStartAngleY, Mathf.Sin(dayPercentage * Mathf.PI));

        // X-axis rotation (simulating the sun's elevation angle from -30° to 90° and back to -30°)
        float targetRotationX = Mathf.Lerp(dayStartAngleX, noonAngleX, dayPercentage); // Daytime transition (morning to noon)
        if (dayPercentage >= 0.5f) // After midday (from noon to sunset)
        {
            targetRotationX = Mathf.Lerp(noonAngleX, nightStartAngleX, (dayPercentage - 0.5f) * 2f); // Transition from noon to evening
        }

        // Smoothly interpolate the current rotations towards the target rotations
        currentRotationY = Mathf.LerpAngle(currentRotationY, targetRotationY, Time.deltaTime * 0.1f); // Y-axis smoothing
        currentRotationX = Mathf.LerpAngle(currentRotationX, targetRotationX, Time.deltaTime * 0.1f); // X-axis smoothing

        // Apply the smooth rotation to the light's transform
        lightTransform.rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0f);
    }
}
