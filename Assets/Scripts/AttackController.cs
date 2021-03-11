using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AttackController : MonoBehaviour, Attackable
{
    [Header("Materials")]
    [SerializeField] protected int changeMaterialAtPosition = 1;
    [SerializeField] protected GameObject changeMaterialAtObject = null;
    [SerializeField] protected AttackType type = AttackType.DISTANCE;
    [SerializeField] protected GameObject diePrefab = null;

    protected Rigidbody rb = null;
    public HeroController hero = null;

    protected float power = 0f;
    public virtual void Init(HeroController _sender)
    {
        rb = GetComponent<Rigidbody>();
        hero = _sender;
        MeshRenderer rend = changeMaterialAtObject.GetComponent<MeshRenderer>();
        Material[] mat = rend.materials;
        mat[changeMaterialAtPosition] = hero.player.accentMaterial;
        rend.materials = mat;
    }
    public virtual void Kill()
    {
        if (diePrefab)
        {
            Instantiate(diePrefab, transform.position, Quaternion.identity);
        }
        Destroy(this.gameObject);
    }

    public HeroController GetHeroController() { return hero; }
    public virtual void TakeDamage(HeroController _hero, float _amount, AttackType _type)
    {
        Kill();
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        Attackable isAttackable = other.GetComponent<Attackable>();

        if (isAttackable != null)
        {
            if (isAttackable.GetHeroController() != hero)
            {
                isAttackable.TakeDamage(hero, power, type);
            }
            if (hero.gameObject != other.gameObject)
                Kill();
        }
        else if (other.CompareTag("Blockable"))
        {
            Kill();
        }
    }
}
