using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerBuildUIController : MonoBehaviour
{
    [SerializeField] private GameObject buildUI = null;
    [SerializeField] private GameObject cursor = null;
    [SerializeField] private Text activeTowerText = null;
    [SerializeField] private Text activeTowerCosts = null;

    private PlayerController player = null;
    void Update()
    {
        if (!player) return;

        buildUI.SetActive(player.arena.activeRound == RoundType.BUILD);
        cursor.SetActive(player.arena.activeRound == RoundType.BUILD);
    }

    public void TakePlayer(PlayerController _player) { player = _player; }
}
