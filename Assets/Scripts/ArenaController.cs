using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArenaController : MonoBehaviour
{
    [SerializeField] private List<TileController> tiles = new List<TileController>();
    [SerializeField] private List<GameObject> walls = new List<GameObject>();
    [SerializeField] private List<PlayerController> players = new List<PlayerController>();
    [SerializeField] private float spawnPositionCheckRadius = 2f;

    [Header("Materials")]
    [SerializeField] private List<Material> materials = new List<Material>();

    public void OnPlayerJoined(PlayerInput _player) {
        _player.transform.SetParent(this.transform);
        Material playerMaterial = materials[players.Count];
        PlayerController newHero = _player.GetComponent<PlayerController>();
        newHero.Init((TeamColor)players.Count, this, playerMaterial);
        players.Add(newHero);
    }

    public Vector3 GetRandomSpawnPosition() {
        int rndTile = Random.Range(0, tiles.Count);
        Transform tile = tiles[rndTile].transform;

        RaycastHit[] hitList = Physics.SphereCastAll(tile.position, spawnPositionCheckRadius, tile.up);
        foreach(RaycastHit hit in hitList) {
            if(hit.collider.CompareTag("Hero") || hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Tower")) {
                return GetRandomSpawnPosition();
            }
        }
        return tile.position;
    }
}

