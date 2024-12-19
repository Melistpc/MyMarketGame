using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewAgent : MonoBehaviour
{
    public float wanderRadius;
    public float wanderTimer;
    private Animator anim;

    public float itemDetectionRadius; // Radius to detect items
    public float grabDistance; // Distance to grab the item
    public float dropDelay = 0.5f; // Delay between dropping items

    private NavMeshAgent agent;
    private bool isExiting = false; // Indicates if the agent is exiting the area

    private float timer;

    // Variables to track collected items
    private int totalCollectedItems = 0; // Total number of items collected
    private int maxItems = 10; // Max items to collect
    private List<GameObject> collectedItems = new List<GameObject>(); // List of collected items

    private GameObject box; // Reference to the box to drop items
    private GameObject waitingArea; // Reference to the waiting area
    private bool isWaiting = false; // Indicates if the agent is waiting for payment

    void OnEnable()
    {
        anim = gameObject.GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;

        // Find the Box object
        box = GameObject.FindGameObjectWithTag("Box");
        if (box == null)
        {
            Debug.LogError("No object with 'Box' tag found in the scene.");
        }

        // Find the WaitingArea object
        waitingArea = GameObject.FindGameObjectWithTag("WaitingArea");
        if (waitingArea == null)
        {
            Debug.LogError("No object with 'WaitingArea' tag found in the scene.");
        }
    }

   void Update()
{
    // Skip behavior if the agent is waiting or exiting
    if (isWaiting || isExiting) return;

    // If the agent has collected the maximum number of items, move to the box
    if (totalCollectedItems >= maxItems)
    {
        MoveToBox();
        return; // Skip the rest of the update when moving to the box
    }

    // Wandering logic
    timer += Time.deltaTime;
    if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
    {
        if (timer >= wanderTimer)
        {
            Vector3 newPos = NewAgent.RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }
    else
    {
        timer = 0;
    }

    // Check for items to collect
    CheckForItems();
}


    private void CheckForItems()
    {
        if (totalCollectedItems >= maxItems) return;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, itemDetectionRadius);
        GameObject closestItem = null;
        float closestDistance = grabDistance;

        foreach (var hitCollider in hitColliders)
        {
            StoreItems storeItem = hitCollider.GetComponent<StoreItems>();
            if (storeItem != null)
            {
                float distanceToItem = Vector3.Distance(transform.position, storeItem.transform.position);
                if (distanceToItem <= closestDistance)
                {
                    closestItem = hitCollider.gameObject;
                    closestDistance = distanceToItem;
                }
            }
        }

        if (closestItem != null)
        {
            GrabItem(closestItem);
            Debug.Log("Grabbed item: " + closestItem.name);
        }
    }

    private void GrabItem(GameObject item)
    {
        if (item != null)
        {
            totalCollectedItems++;
            collectedItems.Add(item);

            Debug.Log($"Collected {totalCollectedItems} / {maxItems} items.");
            item.SetActive(false);
        }
    }

    private void MoveToBox()
    {
        if (box != null)
        {
            Vector3 directionToBox = (box.transform.position - transform.position).normalized;
            float stopDistance = 2.0f;
            Vector3 stopPosition = box.transform.position - directionToBox * stopDistance;

            if (agent.destination != stopPosition)
            {
                agent.SetDestination(stopPosition);
                Debug.Log("Moving to a position near the box to drop items...");
            }

            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                StartCoroutine(DropItemsOneByOne());
            }
        }
        else
        {
            Debug.LogError("Box not found!");
        }
    }

    private IEnumerator DropItemsOneByOne()
    {
        List<GameObject> itemsToDrop = new List<GameObject>(collectedItems);

        foreach (var item in itemsToDrop)
        {
            if (item != null)
            {
                Vector3 dropPosition = box.transform.position + new Vector3(0, 1.0f, 0);
                item.transform.position = dropPosition;
                item.SetActive(true);

                Rigidbody rb = item.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.AddForce(Vector3.down * 2f, ForceMode.Impulse);
                }

                item.transform.SetParent(box.transform);
                yield return new WaitForSeconds(dropDelay);
            }
        }

        collectedItems.Clear();
        totalCollectedItems = 0;

        Debug.Log("All items dropped in the box. Moving to waiting area...");
        MoveToWaitingArea();
    }

    private void MoveToWaitingArea()
    {
        if (waitingArea != null)
        {
            agent.SetDestination(waitingArea.transform.position);
            if (anim != null)
            {
                anim.SetBool("isPay", true);
            }
            isWaiting = true;
            Debug.Log("Moving to waiting area...");
        }
        else
        {
            Debug.LogError("Waiting area not found!");
        }
    }

   public void LeaveWaitingArea(Vector3 leavingAreaPosition)
{
    isWaiting = false;
    isExiting = true; // Set exiting flag to true
    if (anim != null)
    {
        anim.SetBool("isPay", false);
    }
    agent.SetDestination(leavingAreaPosition);
    Debug.Log("Leaving waiting area...");
}


    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
