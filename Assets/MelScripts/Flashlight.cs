using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Flashlight : MonoBehaviour
{
    private Light myflash;
    public Material lens;
    private AudioSource _audioSource;
    private XRGrabInteractable grabInteractable;

    void Start()
    {
        myflash = GetComponentInChildren<Light>();
        _audioSource = GetComponent<AudioSource>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        
        grabInteractable.onSelectEntered.AddListener(OnGrab);
        grabInteractable.onSelectExited.AddListener(OnRelease);
    }

    private void OnGrab(XRBaseInteractor interactor)
    {
        LightOn();
    }

    // When the flashlight is released, turn off the light
    private void OnRelease(XRBaseInteractor interactor)
    {
        LightOff();
    }

    // Method to turn the light on
    public void LightOn()
    {
        _audioSource.Play();
        myflash.enabled = true;
        lens.EnableKeyword("_EMISSION");
    }

    // Method to turn the light off
    public void LightOff()
    {
        _audioSource.Play();
        myflash.enabled = false;
        lens.DisableKeyword("_EMISSION");
    }
}
