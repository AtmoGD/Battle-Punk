using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject heroPrefab = null;
    [SerializeField] public HeroController hero = null;
    [SerializeField] public float cursorSpeed = 5f;
    [SerializeField] public int wins = 0;
    [SerializeField] public int gold = 0;
    public Material accentMaterial { get; private set; }
    public TeamColor color { get; private set; }
    public ArenaController arena { get; private set; }
    public RectTransform cursor { get; private set; }
    public TileController selectedTile = null;

    public void Init(TeamColor _color, ArenaController _arena, Material _material, RectTransform _cursor)
    {
        color = _color;
        arena = _arena;
        accentMaterial = _material;
        cursor = _cursor;
        ResetHero();
    }

    public void Died()
    {
        arena.HeroDied(this);
    }

    public void ResetHero()
    {
        hero = null;
        GameObject newHero = Instantiate(heroPrefab, this.transform);
        hero = newHero.GetComponent<HeroController>();
        hero.Init(this, arena.GetRandomSpawnPosition());
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
            hero.DistanceAttack();
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

    public void CheckCursorPosition() {
        Ray ray = Camera.main.ScreenPointToRay(cursor.position);
        if(Physics.Raycast(ray, out RaycastHit hit)) {
            if(hit.collider.CompareTag("GroundTile")) {
                if(selectedTile) {
                    selectedTile.OnReset();
                    selectedTile = null;
                }
                Debug.Log("FoundTile");
                TileController controller = hit.collider.GetComponent<TileController>();
                selectedTile = controller;
                selectedTile.OnSelect(accentMaterial);
            }
        }
    }

    public void OnChangeSelectionLeft(InputAction.CallbackContext _context)
    {
        Debug.Log("OnChangeSelectionLeft");
    }

    public void OnChangeSelectionRight(InputAction.CallbackContext _context)
    {
        Debug.Log("OnChangeSelectionRight");
    }

    public void OnSelect(InputAction.CallbackContext _context)
    {
        Debug.Log("OnSelect");
    }
}

