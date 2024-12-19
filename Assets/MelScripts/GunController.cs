using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class GunController : MonoBehaviour
{
    public GameObject bulletPrefab; // Bullet prefab to instantiate
    public Transform barrelEnd; // The end of the gun's barrel
    public AudioClip gunshotSound; // Gunshot sound to play
    public float bulletForce = 500f; // Force to apply to the bullet
    public float shootCooldown = 0.2f; // Cooldown time between shots

    private AudioSource audioSource;
    private XRGrabInteractable grabInteractable;
    private XRController controller; // To detect trigger press
    private float lastShotTime; // Track the last time a shot was fired

    public InputHelpers.Button pinchButton = InputHelpers.Button.Trigger; // Pinch button (Trigger)
    private bool isGrabbed = false; // Tracks whether the gun is grabbed

    private void Awake()
    {
        // Get the XRGrabInteractable and AudioSource components
        grabInteractable = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();

        // Add event listeners for grabbing and releasing the gun
        grabInteractable.onSelectEntered.AddListener(OnGrab);
        grabInteractable.onSelectExited.AddListener(OnRelease);
    }

    private void OnDestroy()
    {
        // Remove event listeners to avoid memory leaks
        grabInteractable.onSelectEntered.RemoveListener(OnGrab);
        grabInteractable.onSelectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(XRBaseInteractor interactor)
    {
        controller = interactor.GetComponent<XRController>();
        isGrabbed = true;

        Debug.Log($"Gun grabbed by {interactor.name}: Controller assigned.");
    }

    private void OnRelease(XRBaseInteractor interactor)
    {
        controller = null;
        isGrabbed = false;

        Debug.Log($"Gun released by {interactor.name}: Controller cleared.");
    }

    private void Update()
    {
        // Check if the gun is grabbed and the pinch button (or mouse) is pressed
        if (isGrabbed && (IsPinched() || Input.GetMouseButton(0))) // Left mouse button for pinch
        {
            Shoot();
        }
    }

    private bool IsPinched()
    {
        // Check if the pinch button is pressed on the controller
        if (controller != null && InputHelpers.IsPressed(controller.inputDevice, pinchButton, out bool isPressed))
        {
            return isPressed;
        }
        return false;
    }

    private void Shoot()
    {
        // Enforce cooldown
        if (Time.time - lastShotTime < shootCooldown)
            return;

        lastShotTime = Time.time;

        // Instantiate a bullet and apply force
        if (bulletPrefab != null && barrelEnd != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, barrelEnd.position, barrelEnd.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(barrelEnd.forward * bulletForce, ForceMode.Impulse);
            }

            // Destroy the bullet after 5 seconds
            Destroy(bullet, 5f);
        }
        else
        {
            Debug.LogWarning("Bullet prefab or barrel end not assigned.");
        }

        // Play the gunshot sound
        if (audioSource != null && gunshotSound != null)
        {
            audioSource.PlayOneShot(gunshotSound);
        }
        else
        {
            Debug.LogWarning("Gunshot sound or audio source not assigned.");
        }

        Debug.Log("Gun fired!");
    }
}
