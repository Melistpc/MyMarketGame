using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    public PlayableDirector timeline; // The Timeline containing both the enemy attack and player fall.
    public Enemy enemy; // Reference to the Enemy script.

    private bool isTimelinePlaying = false;

    private void Start()
    {
        if (enemy != null)
        {
            enemy.OnAttack += StartTimeline; // Subscribe to the enemy attack event.
        }
    }

    private void StartTimeline()
    {
        if (!isTimelinePlaying) // Prevent restarting if it's already playing.
        {
            timeline.Play();
            isTimelinePlaying = true;
            StartCoroutine(ResetTimelineState());
        }
    }

    private System.Collections.IEnumerator ResetTimelineState()
    {
        // Wait for the timeline to finish, then reset the state.
        yield return new WaitForSeconds((float)timeline.duration);
        isTimelinePlaying = false;
    }

    private void OnDestroy()
    {
        if (enemy != null)
        {
            enemy.OnAttack -= StartTimeline; // Unsubscribe from the event.
        }
    }
}
