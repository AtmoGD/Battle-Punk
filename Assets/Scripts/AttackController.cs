using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AttackController : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] protected int changeMaterialAtPosition = 1;
    [SerializeField] protected GameObject changeMaterialAtObject = null;
    [SerializeField] protected AttackType type = AttackType.DISTANCE;

    protected Rigidbody rb = null;
    protected HeroController hero = null;

    public virtual void Init(HeroController _sender)
    {
        rb = GetComponent<Rigidbody>();
        hero = _sender;
        MeshRenderer rend = changeMaterialAtObject.GetComponent<MeshRenderer>();
        Material[] mat = rend.materials;
        mat[changeMaterialAtPosition] = hero.player.accentMaterial;
        rend.materials = mat;
    }
    protected void Kill()
    {
        Destroy(this.gameObject);
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        Attackable isAttackable = other.GetComponent<Attackable>();
        AttackController controller = other.GetComponent<AttackController>();
        HeroController heroController = other.GetComponent<HeroController>();

        if (isAttackable != null)
        {
            if (other.gameObject == hero.gameObject)
                return;
            else
                isAttackable.TakeDamage(hero, type);
        }

        if (controller != null)
        {
            if (controller.hero == hero)
                return;
        }

        Kill();
    }
}
