using UnityEngine;

public class MannequinAI : MonoBehaviour
{
    public Transform player; // The player object
    public Light flashlight; // Assign the flashlight's Light component
    public float chaseDistance = 20f; // Distance to start chasing the player
    public float attackDistance = 2f; // Distance to start attacking the player
    public float moveSpeed = 3f; // Speed of movement
    public float detectionRadius = 30f; // Overall detection range
    public float distanceTolerance = 1f; // Tolerance for distance checks

    private Animator animator;
    private bool isChasing = false;
    private bool isAttacking = false;
    private bool isFrozen = false;
    private bool isAttacked = false;

    public bool IsAttacked => isAttacked; // Public getter for isAttacked

    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log("MannequinAI initialized.");
    }

    void Update()
    {
        if (player == null)
        {
            ResetState();
            return;
        }

        // Calculate distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if the mannequin is in the flashlight's cone
        if (IsInFlashlight())
        {
            FreezeOnFlashlight(); // Freeze when in flashlight
            UpdateAnimationState();
            return; // Stop further processing if frozen
        }

        // If the mannequin is outside the flashlight range, move based on player distance
        if (flashlight != null && Vector3.Distance(transform.position, flashlight.transform.position) > flashlight.range)
        {
            // If the mannequin is within the attack range, start attacking
            if (distanceToPlayer <= attackDistance + distanceTolerance)
            {
                StartAttack();
            }
            // If the mannequin is within the chase range, start chasing
            else if (distanceToPlayer <= chaseDistance + distanceTolerance)
            {
                StartChase();
            }
            // If the mannequin is outside both chase and attack ranges, stop all actions
            else
            {
                ResetState();
            }
        }
        // If within flashlight range, don’t move and freeze
        else
        {
            ResetState(); // Stop all actions (no chase or attack) when in flashlight range
        }

        // Update animation states
        UpdateAnimationState();
    }

    void StartChase()
    {
        if (isFrozen) return;

        if (!isChasing)
        {
            Debug.Log("Mannequin starts chasing the player.");
        }

        isChasing = true;
        isAttacking = false;

        // Move toward the player's position
        Vector3 targetPosition = player.position;
        targetPosition.y = transform.position.y; // Keep mannequin at the same height
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Rotate toward the player
        Vector3 directionToPlayer = (targetPosition - transform.position).normalized;
        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    void StartAttack()
    {
        if (isFrozen) return;

        if (!isAttacking)
        {
            Debug.Log("Mannequin starts attacking.");
        }

        isChasing = false;
        isAttacking = true;
        isAttacked = true;
    }

    void ResetState()
    {
        if (isChasing || isAttacking || isFrozen)
        {
            Debug.Log("Mannequin stops all actions.");
        }

        isChasing = false;
        isAttacking = false;
        isFrozen = false;
        isAttacked = false; // Reset isAttacked
        UpdateAnimationState();
    }

    void FreezeOnFlashlight()
    {
        if (!isFrozen)
        {
            Debug.Log("Mannequin freezes due to flashlight.");
        }

        isFrozen = true;
        isChasing = false;
        isAttacking = false;
    }

    bool IsInFlashlight()
    {
        if (flashlight == null)
        {
            Debug.LogWarning("Flashlight reference is missing!");
            return false;
        }

        // Calculate the vector from the flashlight to the mannequin
        Vector3 toMannequin = transform.position - flashlight.transform.position;
        float distanceToMannequin = toMannequin.magnitude;

        // Log the distance to help debug
        Debug.Log($"Mannequin distance to flashlight: {distanceToMannequin}, Flashlight range: {flashlight.range}");

        // Check if the mannequin is within the flashlight's range
        if (distanceToMannequin > flashlight.range)
        {
            Debug.Log("Mannequin is out of flashlight range.");
            return false; // Out of flashlight range
        }

        // Calculate the angle between the flashlight's forward direction and the mannequin
        float angleToMannequin = Vector3.Angle(flashlight.transform.forward, toMannequin);

        // Calculate dynamic tolerance based on distance to mannequin
        float dynamicTolerance = Mathf.Lerp(0.5f, 20f, (flashlight.range - distanceToMannequin) / flashlight.range); // Closer = more tolerance

        // Ensure tolerance doesn't become too small when very close (minimum of 1)
        dynamicTolerance = Mathf.Max(dynamicTolerance, 1f);

        // Adjusted half cone angle considering the tolerance
        float adjustedHalfAngle = flashlight.spotAngle / 2f + dynamicTolerance;

        // Log the angle and adjusted cone angle for debugging
        Debug.Log($"Angle to mannequin: {angleToMannequin}, Flashlight half-cone angle: {flashlight.spotAngle / 2f}, Tolerance: {dynamicTolerance}");

        // Check if the angle is within the flashlight cone plus tolerance
        if (angleToMannequin <= adjustedHalfAngle)
        {
            Debug.Log("Mannequin is within flashlight cone and range.");
            return true; // Mannequin is within the flashlight's cone
        }
        else
        {
            Debug.Log("Mannequin is out of flashlight cone (with tolerance).");
            return false; // Mannequin is out of the flashlight's cone
        }
    }

    void UpdateAnimationState()
    {
        animator.SetBool("isRunning", isChasing);
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isFrozen", isFrozen);
    }

    void OnDrawGizmos()
    {
        if (flashlight != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(flashlight.transform.position, flashlight.range);

            // Adjusted cone based on dynamic tolerance
            float dynamicTolerance = Mathf.Lerp(0.5f, 20f, (flashlight.range - Vector3.Distance(transform.position, flashlight.transform.position)) / flashlight.range);
            dynamicTolerance = Mathf.Max(dynamicTolerance, 1f); // Ensure tolerance doesn't get too small

            float adjustedHalfAngle = flashlight.spotAngle / 2f + dynamicTolerance;

            // Draw cone edges for the flashlight
            Vector3 coneEdge1 = Quaternion.Euler(0, -adjustedHalfAngle, 0) * flashlight.transform.forward;
            Vector3 coneEdge2 = Quaternion.Euler(0, adjustedHalfAngle, 0) * flashlight.transform.forward;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(flashlight.transform.position, flashlight.transform.position + coneEdge1 * flashlight.range);
            Gizmos.DrawLine(flashlight.transform.position, flashlight.transform.position + coneEdge2 * flashlight.range);
        }
    }

    // Visualization in Scene view for debugging
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance + distanceTolerance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackDistance + distanceTolerance);
    }
}
