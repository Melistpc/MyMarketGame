using UnityEngine;

public class MannequinChaseGame : MonoBehaviour
{
    [Header("Game Characters")]
    public Transform player;
    public Transform mannequin;
    public Animator mannequinAnimator;

    [Header("Flashlight Settings")]
    public Light spotLight;
    public float flashlightRange = 10f;

    [Header("Movement Settings")]
    public float mannequinSpeed = 3f;
    public float detectionRange = 10f;

    public bool isChaseRunning = false;
    public bool isMannequinFrozen = false;
    private Renderer mannequinRenderer;

    void Start()
    {
        ValidateComponents();
    }

    void Update()
    {
        // Null checks
        if (player == null || mannequin == null || mannequinAnimator == null)
            return;

        // Check if player is within detection range
        float distanceToPlayer = Vector3.Distance(mannequin.position, player.position);

        if (distanceToPlayer <= detectionRange && !isMannequinFrozen)
        {
            isChaseRunning = true;
            mannequinAnimator.SetBool("isRunning", true);
            
        }
        else
        {
            isChaseRunning = false;
            mannequinAnimator.SetBool("isRunning", false);
        }

        // Check if flashlight is hitting the mannequin
        bool isFlashlightOnMannequin = IsFlashlightOnMannequin();

        // Mannequin movement logic
        if (isChaseRunning && !isMannequinFrozen)
        {
            ChasePlayer(distanceToPlayer);
        }

        // Freeze/Unfreeze mannequin based on flashlight
        UpdateMannequinState(isFlashlightOnMannequin);

        // Check for game over or attack condition
        CheckGameOver(distanceToPlayer);
    }

    void ChasePlayer(float distanceToPlayer)
    {
        if (distanceToPlayer > 2f)
        {
            // Move towards the player if not within attack range
            Vector3 directionToPlayer = (player.position - mannequin.position).normalized;
            mannequin.rotation = Quaternion.LookRotation(directionToPlayer);
            mannequin.position += directionToPlayer * mannequinSpeed * Time.deltaTime;
        }
        else
        {
            // Stop moving and prepare to attack
            mannequinAnimator.SetBool("isAttack", true);
            isChaseRunning = false; // Stop chasing
            mannequinAnimator.SetBool("isRunning", false);
        }
    }

    bool IsFlashlightOnMannequin()
    {
        if (spotLight == null || mannequin == null)
            return false;

        Vector3 directionToMannequin = mannequin.position - spotLight.transform.position;
        float angle = Vector3.Angle(spotLight.transform.forward, directionToMannequin);
        float distance = directionToMannequin.magnitude;

        // Check if mannequin is within flashlight's cone and range
        return angle < spotLight.spotAngle / 2f && distance <= flashlightRange;
    }

    void UpdateMannequinState(bool isHitByFlashlight)
    {
        isMannequinFrozen = isHitByFlashlight;
        mannequinAnimator.SetBool("isFrozen", isMannequinFrozen);

        if (mannequinRenderer != null)
        {
            mannequinRenderer.material.color = isMannequinFrozen ? Color.gray : Color.red;
        }
    }

    void CheckGameOver(float distanceToPlayer)
    {
        if (distanceToPlayer < 2f && !isMannequinFrozen)
        {
            // Trigger attack
            mannequinAnimator.SetBool("isAttack", true);
            //GameOver();
        }
        else
        {
            mannequinAnimator.SetBool("isAttack", false);
        }
    }

    void ValidateComponents()
    {
        if (player == null)
            Debug.LogError("Player is not assigned!");

        if (mannequin == null)
            Debug.LogError("Mannequin is not assigned!");

        if (mannequinAnimator == null)
            Debug.LogError("Mannequin Animator is not assigned!");

        mannequinRenderer = mannequin?.GetComponent<Renderer>();

        if (mannequinRenderer == null)
            Debug.LogWarning("No Renderer found on mannequin.");
    }

    void GameOver()
    {
        Debug.Log("Game Over! Mannequin caught the player!");
    }

    void OnDrawGizmos()
    {
        if (spotLight != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(spotLight.transform.position, flashlightRange);

            float angle = spotLight.spotAngle / 2f;
            Vector3 direction = spotLight.transform.forward;

            Gizmos.color = Color.yellow;
            Vector3 leftRay = Quaternion.Euler(0, -angle, 0) * direction * flashlightRange;
            Vector3 rightRay = Quaternion.Euler(0, angle, 0) * direction * flashlightRange;

            Gizmos.DrawLine(spotLight.transform.position, spotLight.transform.position + leftRay);
            Gizmos.DrawLine(spotLight.transform.position, spotLight.transform.position + rightRay);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(spotLight.transform.position + direction * flashlightRange, 0.5f);
        }
    }
}