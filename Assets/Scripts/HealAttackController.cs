using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAttackController : MonoBehaviour
{ 
    [SerializeField] private HeroController hero = null;
    [SerializeField] private float amountOfHeal = 5f;
    [SerializeField] private Vector3 startRadius = Vector3.zero;
    [SerializeField] private Vector3 endRadius = Vector3.zero;
    [SerializeField] private float time = 3;

    public void TakeHero(HeroController _hero) {
        hero = _hero;
        transform.localScale = startRadius;
    }

    private void Update()
    {
        Vector3 add = (endRadius - startRadius) * (Time.deltaTime / time);
        transform.localScale = transform.localScale + add;

        if (transform.localScale.magnitude >= endRadius.magnitude)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HeroController otherHero = other.GetComponent<HeroController>();
        if(otherHero && otherHero == hero) {
            otherHero.healthPoints = Mathf.Clamp(otherHero.healthPoints + amountOfHeal, 0, otherHero.healthPointsMax);
        }
    }
}
