using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    private CharacterController characterController;
    public float moveSpeed = 5f;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Get input axes
        float horizontal = Input.GetAxis("Horizontal"); // A/D keys or Left/Right arrows
        float vertical = Input.GetAxis("Vertical"); // W/S keys or Up/Down arrows

        // Determine if the player is walking
        bool isWalking = horizontal != 0 || vertical != 0;
        animator.SetBool("isWalking", isWalking);

        // Move the player
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;
        if (isWalking)
        {
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
            // Optionally, rotate the character to face the movement direction
            if (moveDirection != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.deltaTime);
            }
        }
    }
}

        

