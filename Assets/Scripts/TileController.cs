using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [SerializeField] private Material standardMaterial = null;
    [SerializeField] private Material selectedMaterialGreen = null;
    [SerializeField] private Material selectedMaterialPurple = null;
    [SerializeField] private Material blockedMaterial = null;
    [SerializeField] private int changeMaterialAtPosition = 1;

    MeshRenderer rend = null;

    public void Awake()
    {
        rend = GetComponent<MeshRenderer>();
    }

    public void OnSelect(Material _material)
    {
        rend.materials[changeMaterialAtPosition] = _material;
        // switch (_color)
        // {
        //     case TeamColor.GREEN:
        //         rend.materials[changeMaterialAtPosition] = selectedMaterialGreen;
        //         break;
        //     case TeamColor.PURPLE:
        //         rend.materials[changeMaterialAtPosition] = selectedMaterialPurple;
        //         break;
        // }
    }

    public void OnReset()
    {
        rend.materials[changeMaterialAtPosition] = standardMaterial;
    }

    public void OnBlock()
    {
        rend.materials[changeMaterialAtPosition] = blockedMaterial;
    }
}
