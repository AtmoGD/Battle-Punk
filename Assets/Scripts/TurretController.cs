using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : TowerController
{
    [SerializeField] private GameObject lookAtObject = null;
    [SerializeField] private GameObject laserPrefab = null;
    [SerializeField] private Transform spawnPosition = null;
    [SerializeField] private float attackSpeed = 2f;
    [SerializeField] private float attackDistance = 10f;
    [SerializeField] private float attackPower = 5f;
    [SerializeField] private float attackCooldown = 3f;

    private Transform target = null;
    private float cooldown = 0f;

    private void Update()
    {
        if (target)
            LookAtTarget();
        cooldown -= Time.deltaTime;

        if(player.arena.activeRound == RoundType.BUILD) {
            target = null;
        }
        
        if (cooldown <= 0f)
            Attack();
    }

    public void Attack()
    {
        if (!target) return;

        GameObject attack = Instantiate(laserPrefab, spawnPosition.position, spawnPosition.rotation);
        ProjectileController controller = attack.GetComponent<ProjectileController>();
        controller.Init(player.hero, attackSpeed, attackDistance, attackPower);
        cooldown = attackCooldown;
    }

    public void LookAtTarget()
    {
        Vector3 lookAtPos = target.position;
        lookAtPos.y = lookAtObject.transform.position.y;
        lookAtObject.transform.LookAt(lookAtPos);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == target)
            target = null;
    }

    private void OnTriggerStay(Collider other)
    {
        if (target) return;

        HeroController hero = other.GetComponent<HeroController>();

        if (hero)
        {
            if (hero == player.hero)
                return;

            target = hero.transform;
        }
    }
}
