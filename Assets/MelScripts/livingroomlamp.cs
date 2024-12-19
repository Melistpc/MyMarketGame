using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class livingroomlamp : MonoBehaviour
{
    public AudioClip clip;   
    private AudioSource source; 

    private bool isColliding = false;  

    public UnityEvent onPressed; 

    // Reference to the spotlight GameObject
    public GameObject spotlight;  // Assign your spotlight GameObject in the Inspector

    void Start()
    {
        source = GetComponent<AudioSource>();  

        // Optionally, you can initialize the spotlight as inactive or active based on your need
        if (spotlight != null)
        {
            spotlight.SetActive(false);  // Ensure spotlight is off initially (or set to true if desired)
        }
    }

    void Update()
    {
        if (isColliding)  // You can replace this with your VR controller input
        {
            Debug.Log("Button pressed");  // Debug log to see if input is detected
            PlayAudio(); 

            // Toggle the spotlight based on its current state
            ToggleSpotlight();

            isColliding = false; // Reset flag after processing collision
            onPressed?.Invoke();  // Invoke the UnityEvent if the button is pressed
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with: " + other.gameObject.name);  // See if it's entering the trigger
        isColliding = true;
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit called with: " + other.gameObject.name);  // Check when it's exiting the trigger
        isColliding = false;
    }

    // Play the audio only if it's not already playing
    public void PlayAudio()
    {
        if (clip != null && source != null)
        {
            // Check if the audio is already playing to prevent overlapping sounds
            if (!source.isPlaying)  
            {
                Debug.Log("Playing audio...");
                source.PlayOneShot(clip);  // Play the audio clip once
            }
            else
            {
                Debug.Log("Audio is already playing, not playing again.");
            }
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is missing.");
        }
    }

    // Toggle the spotlight based on its current active state
    private void ToggleSpotlight()
    {
        if (spotlight != null)
        {
            // If spotlight is active, deactivate it, otherwise activate it
            bool isActive = spotlight.activeSelf;
            spotlight.SetActive(false);   // Check if the spotlight is currently active
            spotlight.SetActive(!isActive);  // Toggle the active state

            // Log the action
            Debug.Log(isActive ? "Spotlight is now OFF." : "Spotlight is now ON.");
        }
        else
        {
            Debug.LogWarning("Spotlight is not assigned.");
        }
    }
}
