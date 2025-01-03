using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class consumermel : MonoBehaviour
{
    Collider _collider;
    
    void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        Consumable consumable = other.GetComponent<Consumable>();
        if (consumable != null && !consumable.IsFinished)
        {
            consumable.Consume();
        }
    }
} 
