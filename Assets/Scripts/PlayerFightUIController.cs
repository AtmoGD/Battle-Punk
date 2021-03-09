using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFightUIController : MonoBehaviour
{
    [SerializeField] private Slider healthSlider = null;
    [SerializeField] private List<GameObject> winPoints = new List<GameObject>();
    [SerializeField] private Text goldAmount = null;
    [SerializeField] private Image distanceImage = null;
    [SerializeField] private Image strongDistanceImage = null;
    [SerializeField] private Image shieldImage = null;
    [SerializeField] private Image nearImage = null;

    private PlayerController player = null;

    public void TakeHero(PlayerController _player) {
        player = _player;

    }
    private void Update()
    {
        UpdateUI();
    }

    public void UpdateUI() {
        if(!player) return;

        Updatehealth();
        UpdateWinPoints();
        UpdateGold();
        UpdateCooldowns();
    }
    public void Updatehealth() {
        float life = player.hero.healthPoints / player.hero.healthPointsMax;
        healthSlider.value = life;
    }

    public void UpdateWinPoints() {
        for(int i = 0; i < winPoints.Count; i++) {
            winPoints[i].SetActive(player.wins > i);
        }
    }

    public void UpdateGold() {
        goldAmount.text = player.gold.ToString();
    }

    public void UpdateCooldowns() {
        float distanceFill = player.hero.activeDistanceCooldown / player.hero.distanceCooldown;
        distanceImage.fillAmount = distanceFill;

        float strongDistanceFill = player.hero.activeStrongDistanceCooldown / player.hero.strongDistanceCooldown;
        strongDistanceImage.fillAmount = strongDistanceFill;

        float shieldFill = player.hero.activeShieldCooldown / player.hero.shieldCooldown;
        shieldImage.fillAmount = shieldFill;

        float nearFill = player.hero.activeNearCooldown / player.hero.nearCooldown;
        nearImage.fillAmount = nearFill;
    }
}
