using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRotation : MonoBehaviour
{
    [SerializeField] private bool blockX = true;
    [SerializeField] private bool blockY = false;
    [SerializeField] private bool blockZ = true;
    void FixedUpdate()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        rot.x = blockX ? 0 : rot.x;
        rot.y = blockY ? 0 : rot.y;
        rot.z = blockZ ? 0 : rot.z;
        transform.rotation = Quaternion.Euler(rot);
    }
}
