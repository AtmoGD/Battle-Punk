using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealTowerController : TowerController
{
    [SerializeField] private GameObject prefab = null;
    [SerializeField] private Transform spawnPosition = null;
    [SerializeField] private float cooldown = 4f;
    float actualCooldown = 4f;
    private void Update()
    {
        actualCooldown -= Time.deltaTime;
        if(actualCooldown <= 0f) {
            SpawnHeal();
            actualCooldown = cooldown;
        }
    }

    void SpawnHeal() {
        if(!player.arena.gameStarted || player.arena.activeRound != RoundType.FIGHT) return;

        GameObject heal = Instantiate(prefab, spawnPosition.position, Quaternion.identity);
        HealAttackController healController = heal.GetComponent<HealAttackController>();
        healController.TakeHero(player.hero);
    }
}
