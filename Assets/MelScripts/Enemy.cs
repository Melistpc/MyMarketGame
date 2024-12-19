using UnityEngine;
using System; // For the Action delegate.

public class Enemy : MonoBehaviour
{
    public bool IsAttacking = false; // Tracks whether the enemy is attacking.

    // Event to notify when the enemy attacks.
    public event Action OnAttack;

    // Simulate an attack for testing purposes.
    public void Attack()
    {
        IsAttacking = true;

        // Trigger the OnAttack event to notify listeners (e.g., the player).
        OnAttack?.Invoke();

        Debug.Log("Enemy is attacking!");

        // Optionally, reset the attack state after a delay.
        Invoke(nameof(StopAttack), 1f); // Simulate attack duration of 1 second.
    }

    private void StopAttack()
    {
        IsAttacking = false;
        Debug.Log("Enemy finished attacking.");
    }
}
