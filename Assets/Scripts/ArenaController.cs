using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArenaController : MonoBehaviour
{
    [SerializeField] private List<TileController> tiles = new List<TileController>();
    [SerializeField] private List<GameObject> walls = new List<GameObject>();
    [SerializeField] private List<PlayerController> players = new List<PlayerController>();
    [SerializeField] private List<PlayerFightUIController> uiController = new List<PlayerFightUIController>();
    [SerializeField] private List<PlayerBuildUIController> buildUIController = new List<PlayerBuildUIController>();
    [SerializeField] private List<RectTransform> cursors = new List<RectTransform>();
    [SerializeField] public RectTransform canvas = null;
    [SerializeField] private float spawnPositionCheckRadius = 2f;
    [SerializeField] private float fightTime = 100f;
    [SerializeField] private float buildTime = 25f;
    [SerializeField] private int numPlayers = 2;

    [Header("Materials")]
    [SerializeField] private List<Material> materials = new List<Material>();
    public bool gameStarted = false;
    public RoundType activeRound = RoundType.BUILD;
    public float activeTimer = 0f;
    public void OnPlayerJoined(PlayerInput _player)
    {
        _player.transform.SetParent(this.transform);
        Material playerMaterial = materials[players.Count];
        PlayerController newPlayer = _player.GetComponent<PlayerController>();
        newPlayer.Init((TeamColor)players.Count, this, playerMaterial, cursors[players.Count]);
        uiController[players.Count].gameObject.SetActive(true);
        uiController[players.Count].TakeHero(newPlayer);
        buildUIController[players.Count].TakePlayer(newPlayer);
        players.Add(newPlayer);

        if (players.Count == numPlayers)
            StartGame();
        else
            StartBuild();
    }

    private void Update()
    {
        if (!gameStarted) return;

        activeTimer -= Time.deltaTime;
        if (activeTimer <= 0f)
            ChangeRound();
    }

    public void StartGame()
    {
        gameStarted = true;
        StartBuild();
    }

    public void ChangeRound()
    {
        switch (activeRound)
        {
            case RoundType.BUILD:
                StartFight();
                break;
            case RoundType.FIGHT:
                StartBuild();
                break;
        }
    }

    public void StartFight()
    {
        activeRound = RoundType.FIGHT;
        activeTimer = fightTime;
        foreach (PlayerController player in players)
        {
            player.ChangeGameMode(RoundType.FIGHT);
        }
    }

    public void StartBuild()
    {
        activeRound = RoundType.BUILD;
        activeTimer = buildTime;
        foreach (PlayerController player in players)
        {
            player.ChangeGameMode(RoundType.BUILD);
        }
    }

    public Vector3 GetRandomSpawnPosition()
    {
        int rndTile = Random.Range(0, tiles.Count);
        TileController tile = tiles[rndTile];

        if (tile.blocked || tile.towerPlaced)
            return GetRandomSpawnPosition();
        else

            // RaycastHit[] hitList = Physics.SphereCastAll(tile.position, spawnPositionCheckRadius, tile.up);
            // foreach (RaycastHit hit in hitList)
            // {
            //     if (hit.collider.CompareTag("Hero") || hit.collider.CompareTag("Wall") || hit.collider.CompareTag("Tower"))
            //     {
            //         return GetRandomSpawnPosition();
            //     }
            // }
            return tile.transform.position;
    }

    public PlayerController GetEnemy(PlayerController _player) {
        foreach(PlayerController player in players) {
            if(player != _player)
                return player;
        }
        return null;
    }

    public void HeroDied(PlayerController _player)
    {
        foreach (PlayerController player in players)
        {
            if (player != _player)
            {
                player.wins++;
            }
            else
            {
                player.ResetHero();
            }
        }
    }
}

