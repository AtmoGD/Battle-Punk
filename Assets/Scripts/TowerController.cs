﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerController : MonoBehaviour, Attackable
{
    public Action OnDie;
    [SerializeField] private GameObject changeMaterialObject = null;
    [SerializeField] private int changeMaterialAtPos = 1;
    [SerializeField] public float healthMax = 200f; 
    [SerializeField] public float health = 200f;
    [SerializeField] private List<GameObject> drops = new List<GameObject>();
    [SerializeField] private float dropRadius = 2f;
    [SerializeField] protected PlayerController player = null;

    public void Die() {
        OnDie?.Invoke();
        Drop();
        Destroy(this.gameObject);
    }
    private void Drop() {
        foreach(GameObject drop in drops) {
            Vector3 pos = transform.position;
            pos.x += UnityEngine.Random.Range(-dropRadius, dropRadius);
            pos.z += UnityEngine.Random.Range(-dropRadius, dropRadius);
            Collectable collectable = Instantiate(drop, pos, Quaternion.identity).GetComponent<Collectable>();
            collectable.TakeTarget(player.arena.GetEnemy(player));
        }
    }

    public void TakePlayer(PlayerController _player) {
        player = _player;
        health = healthMax;
        ChangeMaterial();
    }
    public void TakeDamage(HeroController _hero, float _amount, AttackType _type)
    {
        if (_hero == player.hero) return;

        health -= _amount;
        if (health <= 0f)
            Die();
    }
    public void ChangeMaterial() {
        MeshRenderer ren = changeMaterialObject.GetComponent<MeshRenderer>();
        Material[] mats = ren.materials;
        mats[changeMaterialAtPos] = player.accentMaterial;
        ren.materials = mats;
    }
    public HeroController GetHeroController() { return player.hero; }
}
