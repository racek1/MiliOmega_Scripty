using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WeaponSelection : MonoBehaviour
{
    public SceneController sceneController;
    private string[] weapons = {
        "Smg","Ar","Shotgun","SniperRifle","Revolver"
    };
    void Start()
    {
        UpdateUI();
    }

    public void PlaySound(string soundName)
    {
        Toolkit.playSound(soundName);
    }

    public void SelectWeapon(string weaponName)
    {
        PlayerPrefs.SetString("equippedWeapon",weaponName);
        PlaySound("Select");
        sceneController.GotoScene("MainScene");
    }

    private void UpdateUI()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (PlayerPrefs.GetInt(weapons[i] + "Unlocked", 0) == 1)
            {
                GameObject.Find("Lock_" + weapons[i]).SetActive(false);
            }
        }
    }
}
