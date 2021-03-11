using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : AttackController
{
    private float time = 1f;
    private float radius = 1f;
    public override void Init(HeroController _sender)
    {
        hero = _sender;
        time = hero.GetShieldTime();
        radius = hero.GetShieldRadius();

        transform.SetParent(hero.transform);
        transform.localScale = new Vector3(radius, radius, radius);

    }

    private void FixedUpdate()
    {
        time -= Time.deltaTime;
        if (time <= 0f)
            Kill();
    }

    public override void TakeDamage(HeroController _hero, float _amount, AttackType _type) {
    }

    protected override void OnTriggerEnter(Collider other)
    {
    }
}
