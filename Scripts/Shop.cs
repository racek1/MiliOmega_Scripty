using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    private string[] weapons = {
        "Smg","Ar","Shotgun","SniperRifle","Revolver"
    };
    void Start()
    {
        UpdateUI();
    }

    public void BuyWeapon(string weaponName)
    {
        int price = 0;
        switch (weaponName)
        {
            case "Smg": price = 50; break;
            case "Ar": price = 50; break;
            case "Shotgun": price = 50; break;
            case "SniperRifle": price = 50; break;
            case "Revolver": price = 50; break;
        }
        int coins = PlayerPrefs.GetInt("Coins",0);
        if (coins >= price)
        {
            coins -= price;
            PlayerPrefs.SetInt("Coins",coins);
            PlayerPrefs.SetInt(weaponName + "Unlocked",1);
        }
        UpdateUI();
    }

    public void PlaySound(string soundName)
    {
        Toolkit.playSound(soundName);
    }

    private void UpdateUI()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (PlayerPrefs.GetInt(weapons[i] + "Unlocked", 0) == 1 && GameObject.Find("Lock_" + weapons[i]) != null)
            {
                GameObject.Find("Lock_" + weapons[i]).SetActive(false);
            }
        }
        coinText.text = PlayerPrefs.GetInt("Coins",0).ToString();
    }

    public void GotoScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
