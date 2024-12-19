using UnityEngine;

public class PlayerController : MonoBehaviour
{
    

    public Animator playerAnimator; // Reference to the player's Animator component
    public MannequinAI mannequin; // Reference to the MannequinAI script
    void TestHitAnimation()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Hit");
            Debug.Log("Forced 'Hit' animation trigger.");
        }
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) // Replace KeyCode.H with any desired key
        {
            TestHitAnimation();
        }
        // Check if mannequin exists and isAttacked is true
        if (mannequin != null && mannequin.IsAttacked)
        {
            Debug.Log("Mannequin's isAttacked is TRUE. Playing Hit animation.");
            PlayHitAnimation();
        }
        else if (mannequin == null)
        {
            Debug.LogWarning("Mannequin reference is missing.");
        }
        else if (!mannequin.IsAttacked)
        {
            Debug.Log("Mannequin's isAttacked is FALSE.");
        }
    }

    void PlayHitAnimation()
    {
        if (playerAnimator != null)
        {
            Debug.Log("Setting 'Hit' trigger on playerAnimator.");
            playerAnimator.SetTrigger("Hit");
        }
        else
        {
            Debug.LogError("PlayerAnimator is not assigned!");
        }
    }



}