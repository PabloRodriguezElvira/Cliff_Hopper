using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    public float timeToDestroy;

    // Update is called once per frame
    void Update()
    {
        timeToDestroy -= Time.deltaTime;
        if (timeToDestroy <= 0.0f)
            Destroy(gameObject);
    }
}
