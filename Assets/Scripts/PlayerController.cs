using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject heroPrefab = null;
    [SerializeField] public HeroController hero = null;
    [SerializeField] public string placeTowerSound = "";
    [SerializeField] public string cancelSound = "";
    [SerializeField] public float cursorSpeed = 5f;
    [SerializeField] public float respawnSpeed = 3f;
    [SerializeField] public int wins = 0;
    [SerializeField] public int gold = 0;
    [SerializeField] public List<TowerData> towerData = new List<TowerData>();
    // [SerializeField] public List<GameObject> towers = new List<GameObject>();

    public Material accentMaterial { get; private set; }
    public TeamColor color { get; private set; }
    public ArenaController arena { get; private set; }
    public GoalController goal { get; private set; }
    public RectTransform cursor { get; private set; }
    public TileController startTile { get; private set; }
    public TileController selectedTile = null;
    public TowerData activeTowerData = null;
    public GameObject activeTower = null;
    public PlayerInput input = null;
    private bool shouldRespawn = false;
    private float activeRespawnCooldown = 0f;

    private void Update()
    {
        if (shouldRespawn)
        {
            activeRespawnCooldown -= Time.deltaTime;
            if (activeRespawnCooldown <= 0f && arena.activeRound == RoundType.FIGHT)
            {
                ResetHero();
            }
        }
    }

    public void Init(TeamColor _color, ArenaController _arena, Material _material, RectTransform _cursor, GoalController _goal, TileController _startTile)
    {
        input = GetComponent<PlayerInput>();
        color = _color;
        startTile = _startTile;
        goal = _goal;
        arena = _arena;
        accentMaterial = _material;
        cursor = _cursor;
        ResetHero();
    }

    private void PlaceTower()
    {
        if (!selectedTile) return;

        if (!activeTowerData)
            activeTowerData = towerData[0];

        if (activeTower)
        {
            Destroy(activeTower);
            activeTower = null;
        }

        activeTower = Instantiate(activeTowerData.prefab, selectedTile.transform.position, Quaternion.identity);
        TowerController tower = activeTower.GetComponent<TowerController>();
        tower.TakePlayer(this);
    }

    private void ReplaceTower()
    {
        if (!selectedTile || !activeTower) return;
        activeTower.transform.position = selectedTile.transform.position;
    }

    public void ChangeGameMode(RoundType _type)
    {
        switch (_type)
        {
            case RoundType.BUILD:
                if (activeTower)
                    activeTower.SetActive(true);

                input.currentActionMap = input.actions.FindActionMap("Build");
                if (hero)
                    hero.gameObject.SetActive(false);
                break;

            case RoundType.FIGHT:
                if (activeTower)
                    activeTower.SetActive(false);

                if (selectedTile)
                    selectedTile.OnReset();

                input.currentActionMap = input.actions.FindActionMap("Fight");
                if (hero)
                {
                    hero.gameObject.SetActive(true);
                    hero.SetPosition(startTile.transform.position);
                }
                break;
        }
    }

    public void Died()
    {
        shouldRespawn = true;
        activeRespawnCooldown = respawnSpeed;
        // arena.HeroDied(this);
    }

    public void ResetHero()
    {
        hero = null;
        GameObject newHero = Instantiate(heroPrefab, this.transform);
        hero = newHero.GetComponent<HeroController>();
        hero.Init(this, startTile.transform.position);
        shouldRespawn = false;
    }

    public void OnMove(InputAction.CallbackContext _context)
    {
        if (hero)
            hero.Movement = _context.ReadValue<Vector2>();
    }

    public void OnRotate(InputAction.CallbackContext _context)
    {
        if (hero)
            hero.Rotation = _context.ReadValue<Vector2>();
    }

    public void OnDistanceAttack(InputAction.CallbackContext _context)
    {
        if (hero)
            hero.DistanceAttack(_context.started);
    }

    public void OnStrongDistanceAttack(InputAction.CallbackContext _context)
    {
        if (hero)
            hero.StrongDistanceAttack();
    }

    public void OnShield(InputAction.CallbackContext _context)
    {
        if (hero)
            hero.Shield();
    }

    public void OnNearAttack(InputAction.CallbackContext _context)
    {
        if (hero)
            hero.NearAttack();
    }

    public void OnMoveCursor(InputAction.CallbackContext _context)
    {
        if (!cursor) return;

        Vector2 move = _context.ReadValue<Vector2>() * Time.deltaTime * cursorSpeed;
        Vector3 actualPos = cursor.localPosition;
        actualPos += new Vector3(move.x, move.y, 0);
        actualPos.x = Mathf.Clamp(actualPos.x, -(arena.canvas.rect.width / 2), (arena.canvas.rect.width / 2));
        actualPos.y = Mathf.Clamp(actualPos.y, -(arena.canvas.rect.height / 2), (arena.canvas.rect.height / 2));
        cursor.localPosition = actualPos;
        CheckCursorPosition();
    }

    public void OnRestartGame(InputAction.CallbackContext _context)
    {
        if (_context.started)
            SceneManager.LoadScene("Game");
    }

    public void CheckCursorPosition()
    {
        if (!arena.gameStarted) return;

        Vector3 camPos = Camera.main.transform.position;
        Vector3 rayDir = cursor.position - camPos;
        Ray ray = new Ray(camPos, rayDir);

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, 1 << LayerMask.NameToLayer("GroundTile")))
        {
            TileController newTile = hit.collider.GetComponent<TileController>();
            if (newTile.blocked || newTile.towerPlaced || newTile.GetPlayer() != this) return;

            if (newTile != selectedTile)
            {
                if (selectedTile)
                {
                    selectedTile.OnReset();
                    selectedTile = null;
                }

                selectedTile = hit.collider.GetComponent<TileController>();
                selectedTile.OnSelect(accentMaterial);

                if (activeTower)
                    ReplaceTower();
                else
                    PlaceTower();
            }
        }
    }

    public void OnChangeSelectionLeft(InputAction.CallbackContext _context)
    {
        if (!_context.started) return;
        int index = towerData.IndexOf(activeTowerData);
        index--;
        if (index < 0)
            index = towerData.Count - 1;
        activeTowerData = towerData[index];
        PlaceTower();
    }

    public void OnRotateSelectionLeft(InputAction.CallbackContext _context)
    {
        if (!activeTower) return;

        activeTower.transform.Rotate(new Vector3(0f, -90f, 0f));
    }

    public void OnRotateSelectionRight(InputAction.CallbackContext _context)
    {
        if (!activeTower) return;

        activeTower.transform.Rotate(new Vector3(0f, 90f, 0f));
    }

    public void OnChangeSelectionRight(InputAction.CallbackContext _context)
    {
        if (!_context.started) return;
        int index = towerData.IndexOf(activeTowerData);
        index++;
        if (index > towerData.Count - 1)
            index = 0;
        activeTowerData = towerData[index];
        PlaceTower();
    }

    public void OnSelect(InputAction.CallbackContext _context)
    {
        if (!activeTower || !activeTowerData) return;

        if (activeTowerData.costs > gold)
        {
            AudioManager.instance.Play(cancelSound);
            return;
        }
        if (selectedTile)
        {
            if (selectedTile.towerPlaced)
                return;

            AudioManager.instance.Play(placeTowerSound);
            gold -= activeTowerData.costs;
            selectedTile.PlaceTower(activeTower);
            activeTower = null;
        }

    }
}

