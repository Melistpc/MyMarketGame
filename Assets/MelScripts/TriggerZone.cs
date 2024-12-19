using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    // Start is called before the first frame update
       Animator myAn;
    void Start()
    {
        myAn = gameObject.GetComponent<Animator>();
       // myAn.SetTrigger("Opendoor");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
   public void OnTriggerEnter(Collider other)
{
    if (myAn == null)
    {
        Debug.LogError("Animator not found!");
        return;
    }

    Debug.Log("OnTriggerEnter called with: " + other.gameObject.name);  
    myAn.SetTrigger("Opendoor");
}

}
