using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject heroPrefab = null;
    [SerializeField] private HeroController hero = null;
    public Material accentMaterial { get; private set; }
    public TeamColor color { get; private set; }
    public ArenaController arena { get; private set; }

    public void Init(TeamColor _color, ArenaController _arena, Material _material)
    {
        color = _color;
        arena = _arena;
        accentMaterial = _material;

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
        if(hero)
            hero.DistanceAttack();
    }

    public void OnStrongDistanceAttack(InputAction.CallbackContext _context)
    {
        Debug.Log("OnStrongDistanceAttack");
    }

    public void OnShield(InputAction.CallbackContext _context)
    {
        Debug.Log("OnShield");
    }

    public void OnNearAttack(InputAction.CallbackContext _context)
    {
        Debug.Log("OnNearAttack");
    }

    public void OnMoveCursor(InputAction.CallbackContext _context)
    {
        Debug.Log("OnMoveCursor");
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

