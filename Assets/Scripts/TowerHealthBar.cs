using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerHealthBar : MonoBehaviour
{
    [SerializeField] private TowerController tower = null;

    [SerializeField] Slider slider = null;

    private void Update()
    {
        float fillAmount = tower.health / tower.healthMax;
        slider.value = fillAmount;
    }
}
