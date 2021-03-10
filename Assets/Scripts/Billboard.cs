using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
