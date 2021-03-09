using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : AttackController
{
    private float speed = 5f;
    private float power = 10f;
    private float distance = 10f;

    public override void Init(HeroController _sender)
    {
        base.Init(_sender);

        speed = hero.GetDistanceSpeed();
        Debug.Log(hero.GetDistanceSpeed());
        power = hero.GetDistancePower();
        distance = hero.GetDistanceAmount();
    }

    private void FixedUpdate()
    {
        // Debug.Log(speed);
        Vector3 newPos = transform.position + (transform.forward * speed * Time.deltaTime);
        float flightDistance = (newPos - transform.position).magnitude;
        rb.MovePosition(newPos);

        distance -= flightDistance;
        if (distance <= 0f)
            Kill();
    }

    private void Kill()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Shield")) {
            //TODO
        } else {
            Kill();
        }
    }
}
