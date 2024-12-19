using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightflicker : MonoBehaviour
{
public Light myLight;
public float maxWait = 1;
public float maxFlicker = 0.2f;

float timer;
float interval;

void Update()
{
    timer += Time.deltaTime;
    if (timer > interval)
    {
        ToggleLight();
    }
}

void ToggleLight()
{
    myLight.enabled = !myLight.enabled;
    if (myLight.enabled)
    {
        interval = Random.Range(0, maxWait);
    }
    else 
    {
        interval = Random.Range(0, maxFlicker);
    }
    
    timer = 0;
}
}