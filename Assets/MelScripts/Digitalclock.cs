using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DigitalClock : MonoBehaviour
{
    public TMP_Text timeText;           // Reference to the TMP_Text component to display time
    public float timeScale = 5f;        // Real-time to game-time scaling factor
    private float gameTimeInMinutes = 0f; // Tracks the game time in minutes
    private int gameDay = 1;           // Tracks the game day

    public Light directionalLight;     // Reference to the Directional Light
    public float fullDayLength = 1440f; // Total minutes for a full day (24 hours)
    public float dayLightIntensity = 1f;
    public float nightLightIntensity = 0.1f;

    public GameObject npcPrefab;       // NPC prefab to spawn
    public Transform[] spawnPoints;    // Array of spawn points
    private Dictionary<string, bool> spawnedNPCs = new Dictionary<string, bool>();
    private int lastSpawnedHour = -1;

    private readonly string[] npcNames = new string[]
    {
        "Alice", "Bob", "Charlie", "Diana", "Edward",
        "Fiona", "George", "Hannah", "Isaac", "Jenna"
    };

    void Start()
    {
        if (timeText == null)
            timeText = GetComponent<TMP_Text>();

        if (directionalLight == null)
            directionalLight = FindObjectOfType<Light>();
    }

    void Update()
    {
        // Update game time
        gameTimeInMinutes += Time.deltaTime * timeScale;

        if (gameTimeInMinutes >= fullDayLength)
        {
            gameTimeInMinutes -= fullDayLength;
            gameDay++;
        }

        int gameHours = Mathf.FloorToInt(gameTimeInMinutes / 60);
        int gameMinutes = Mathf.FloorToInt(gameTimeInMinutes % 60);

        string period = "AM";
        if (gameHours >= 12)
        {
            period = "PM";
            if (gameHours > 12) gameHours -= 12;
        }
        else if (gameHours == 0)
        {
            gameHours = 12;
        }

        string gameTime = $"{gameHours:D2}:{gameMinutes:D2} {period}";
        timeText.text = $"{gameTime}\nDay {gameDay}";

        // Spawn NPCs between 9 AM and 5 PM
        if (gameHours >= 9 && gameHours < 17 && gameMinutes == 0 && lastSpawnedHour != gameHours)
        {
            SpawnNPC(gameHours);
            lastSpawnedHour = gameHours;
        }
    }

    private void SpawnNPC(int hour)
    {
        // Get the next available NPC name
        string npcName = null;
        foreach (var name in npcNames)
        {
            if (!spawnedNPCs.ContainsKey(name))
            {
                npcName = name;
                break;
            }
        }

        if (npcName == null)
        {
            Debug.LogWarning("All NPCs have been spawned!");
            return;
        }

        // Choose a random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Instantiate the NPC and assign its name
        GameObject newNPC = Instantiate(npcPrefab, spawnPoint.position, Quaternion.identity);
        newNPC.name = npcName;

        // Register the NPC as spawned
        spawnedNPCs[npcName] = true;

        Debug.Log($"Spawned NPC: {npcName} at {spawnPoint.position} at {hour}:00.");
    }
}
