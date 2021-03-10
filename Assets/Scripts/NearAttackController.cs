using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearAttackController : AttackController
{
    private float time = 1f;
    private Vector3 startRadius = Vector3.zero;
    private Vector3 endRadius = Vector3.zero;
    public override void Init(HeroController _sender)
    {
        hero = _sender;
        power = hero.GetNearAttackPower();
        time = hero.GetNearAttackTime();
        startRadius = Vector3.one * hero.GetNearAttackStartRadius();
        endRadius = Vector3.one * hero.GetNearAttackEndRadius();

        transform.SetParent(hero.transform);
        transform.localScale = startRadius;
    }

    private void Update()
    {
        Vector3 add = (endRadius - startRadius) * (Time.deltaTime / time);
        transform.localScale = transform.localScale + add;

        if (transform.localScale.magnitude >= endRadius.magnitude)
        {
            Kill();
        }
    }

    // protected override void OnTriggerEnter(Collider other)
    // {
    //     // Attackable isAttackable = other.GetComponent<Attackable>();
    //     // if (isAttackable != null)
    //     // {
    //     //     if (other.gameObject == hero.gameObject)
    //     //         return;
    //     //     else
    //     //         isAttackable.TakeDamage(hero, power, type);
    //     // }
    // }
}
