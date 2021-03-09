using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTimer : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 3f;

    void Update()
    {
        timeToDestroy -= Time.deltaTime;
        if(timeToDestroy <= 0f)
            Destroy(this.gameObject);    
    }
}
