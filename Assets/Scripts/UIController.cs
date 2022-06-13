using System;
using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private TMP_Text shieldText;
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text levelText;

    private void Update()
    {
        healthText.text = "HEALTH: " + playerController.currentHealth;
        shieldText.text = "SHIELD: " + playerController.currentShield;
        coinsText.text = "COINS: " + playerController.currentCoins;
        levelText.text = "LEVEL: " + GameManager.level;
    }

    public void SetLevel(int level)
    {
        levelText.text = "LEVEL: " + level;
    }
}
