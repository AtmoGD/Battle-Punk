using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroHealthBar : MonoBehaviour
{
    [SerializeField] private HeroController hero = null;

    [SerializeField] Slider slider = null;
    private void Update()
    {
        float fillAmount = hero.healthPoints / hero.healthPointsMax;
        slider.value = fillAmount;
    }
}
