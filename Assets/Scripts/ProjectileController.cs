using System.Runtime.InteropServices.ComTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : AttackController
{
    private float speed = 5f;
    private float distance = 10f;
    public override void Init(HeroController _sender)
    {
        base.Init(_sender);

        switch (type)
        {
            case AttackType.DISTANCE:
                speed = hero.GetDistanceSpeed();
                distance = hero.GetDistanceAmount();
                power = hero.GetDistancePower();
                break;
            case AttackType.STRONGDISTANCE:
                speed = hero.GetStrongDistanceSpeed();
                distance = hero.GetStrongDistanceAmount();
                power = hero.GetStrongDistancePower();
                break;
        }
    }

    public void Init(HeroController _sender, float _speed, float _distance, float _power) {
        base.Init(_sender);
        speed = _speed;
        distance = _distance;
        power = _power;
    }

    private void FixedUpdate()
    {
        Vector3 newPos = transform.position + (transform.forward * speed * Time.deltaTime);
        float flightDistance = (newPos - transform.position).magnitude;
        rb.MovePosition(newPos);

        distance -= flightDistance;
        if (distance <= 0f)
            Kill();
    }
}
