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

        if(!player.activeTowerData) return;
        
        activeTowerText.text = player.activeTowerData.name;
        activeTowerCosts.text = player.activeTowerData.costs.ToString("F0");
    }

    public void TakePlayer(PlayerController _player) { player = _player; }
}
