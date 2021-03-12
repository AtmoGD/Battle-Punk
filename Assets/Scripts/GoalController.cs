using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalController : MonoBehaviour, Attackable
{
    [SerializeField] public float healthPointsMax = 200;
    [SerializeField] private ArenaController arena = null;
    PlayerController player = null;
    public float healthPoints = 0f;

    public void TakeDamage(HeroController _hero, float _amount, AttackType _type)
    {
        healthPoints -= _amount;

        if (healthPoints <= 0f)
        {
            Die();
        }
    }
    private void Die()
    {
        arena.PlayerDied(player);
    }
    public void TakePlayer(PlayerController _hero)
    {
        healthPoints = healthPointsMax;
        player = _hero;
    }
    public HeroController GetHeroController() { return player.hero; }
}
