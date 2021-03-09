using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AttackController : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] protected int changeMaterialAtPosition = 1;
    [SerializeField] protected GameObject changeMaterialAtObject = null;

    protected Rigidbody rb = null;
    protected HeroController hero = null;
    
    public virtual void Init(HeroController _sender) {
        rb = GetComponent<Rigidbody>();
        hero = _sender;
        MeshRenderer rend = changeMaterialAtObject.GetComponent<MeshRenderer>();
        Material mat = rend.material;
        mat = hero.player.accentMaterial;
        rend.material = mat;
    }
}
