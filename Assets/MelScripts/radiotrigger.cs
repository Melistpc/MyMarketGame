using UnityEngine;
using UnityEngine.Events;

public class ButtonPokeAudio : MonoBehaviour
{
    public AudioClip clip;   
    private AudioSource source; 

    private bool isColliding = false;  
    
    public UnityEvent onPressed; 

    void Start()
    {
        source = GetComponent<AudioSource>();  
    }

    void Update()
    {
        if (isColliding)  // You can replace this with your VR controller input
        {
            Debug.Log("Button pressed");  // Debug log to see if input is detected
            PlayAudio(); 
            isColliding = false; // Call the audio play function
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
        
         Debug.Log("Controller exited button trigger: " + other.gameObject.name);
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
                Debug.Log("Playing audio...");  // Add this log to check if the method is being called.
                source.PlayOneShot(clip); 
                 // Play the audio clip once
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
}
