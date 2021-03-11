using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [SerializeField] private Material standardMaterial = null;
    [SerializeField] private int changeMaterialAtPosition = 1;
    private TowerController tower = null;
    private PlayerController player = null;
    public bool blocked = false;
    public bool towerPlaced = false;
    MeshRenderer rend = null;

    public void Awake()
    {
        rend = GetComponent<MeshRenderer>();
    }

    public void TakePlayer(PlayerController _player)
    {
        player = _player;
        standardMaterial = _player.accentMaterial;
        ChangeMaterial(standardMaterial);
    }

    public PlayerController GetPlayer() { return player; }

    public void OnSelect(Material _material)
    {
        ChangeMaterial(_material);
        blocked = true;
    }

    public void OnReset()
    {
        ChangeMaterial(standardMaterial);
        blocked = false;
    }

    public void PlaceTower(GameObject _tower)
    {
        tower = _tower.GetComponent<TowerController>();
        tower.OnDie += ResetTower;
        towerPlaced = true;
    }

    public void ResetTower()
    {
        tower.OnDie -= ResetTower;
        blocked = false;
        towerPlaced = false;
    }

    void ChangeMaterial(Material _material)
    {
        Material[] mats = rend.materials;
        mats[changeMaterialAtPosition] = _material;
        rend.materials = mats;
    }
}
