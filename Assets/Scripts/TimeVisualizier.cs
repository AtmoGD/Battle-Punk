using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeVisualizier : MonoBehaviour
{
    [SerializeField] private ArenaController arena = null;
    [SerializeField] private Text roundText = null;
    [SerializeField] private Text timeText = null;
    void Update()
    {
        if(!arena) return;

        roundText.text = arena.activeRound.ToString();
        timeText.text = arena.activeTimer.ToString("F0");
    }
}
