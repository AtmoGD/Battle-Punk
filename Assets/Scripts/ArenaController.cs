using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ArenaController : MonoBehaviour
{
    [SerializeField] private string playerJoinedSound = "";
    [SerializeField] private string themeSound = "";
    [SerializeField] private List<TileController> tiles = new List<TileController>();
    [SerializeField] private List<GameObject> walls = new List<GameObject>();
    [SerializeField] private GoalController leftGoal = null;
    [SerializeField] private List<TileController> leftField = new List<TileController>();
    [SerializeField] private TileController leftStart = null;
    [SerializeField] private GoalController rightGoal = null;
    [SerializeField] private List<TileController> rightField = new List<TileController>();
    [SerializeField] private TileController rightStart = null;
    [SerializeField] private List<PlayerController> players = new List<PlayerController>();
    [SerializeField] private List<PlayerFightUIController> uiController = new List<PlayerFightUIController>();
    [SerializeField] private List<PlayerBuildUIController> buildUIController = new List<PlayerBuildUIController>();
    [SerializeField] private List<RectTransform> cursors = new List<RectTransform>();
    [SerializeField] public RectTransform canvas = null;
    [SerializeField] public GameObject gameOverScreen = null;
    [SerializeField] public Text wonField = null;
    [SerializeField] private float spawnPositionCheckRadius = 2f;
    [SerializeField] private float fightTime = 100f;
    [SerializeField] private float buildTime = 25f;
    [SerializeField] private int numPlayers = 2;

    [Header("Materials")]
    [SerializeField] private List<Material> materials = new List<Material>();
    public bool gameStarted = false;
    public RoundType activeRound = RoundType.BUILD;
    public float activeTimer = 0f;
    private void Start()
    {
        AudioManager.instance.Play(themeSound);
    }

    public void OnPlayerJoined(PlayerInput _player)
    {
        AudioManager.instance.Play(playerJoinedSound);
        _player.transform.SetParent(this.transform);
        Material playerMaterial = materials[players.Count];
        PlayerController newPlayer = _player.GetComponent<PlayerController>();

        if (players.Count == 0)
        {
            newPlayer.Init((TeamColor)players.Count, this, playerMaterial, cursors[players.Count], leftGoal, leftStart);
            foreach (TileController tile in leftField)
            {
                tile.TakePlayer(newPlayer);
            }
            leftGoal.TakePlayer(newPlayer);
        }
        else
        {
            newPlayer.Init((TeamColor)players.Count, this, playerMaterial, cursors[players.Count], rightGoal, rightStart);
            foreach (TileController tile in rightField)
            {
                tile.TakePlayer(newPlayer);
            }

            rightGoal.TakePlayer(newPlayer);
        }

        uiController[players.Count].gameObject.SetActive(true);
        uiController[players.Count].TakeHero(newPlayer);
        buildUIController[players.Count].TakePlayer(newPlayer);
        players.Add(newPlayer);

        if (players.Count == numPlayers)
            StartGame();
        // else
        //     StartBuild();
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
            return tile.transform.position;
    }

    public PlayerController GetEnemy(PlayerController _player)
    {
        foreach (PlayerController player in players)
        {
            if (player != _player)
                return player;
        }
        return null;
    }

    public void PlayerDied(PlayerController _player)
    {
        if (!gameStarted) return;

        gameStarted = false;
        wonField.text = _player == players[0] ? "Player 2 has won!" : "Player 1 has won!";
        gameOverScreen.SetActive(true);
    }
}

