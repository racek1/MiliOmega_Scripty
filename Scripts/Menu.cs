using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("PlayerPrefs deleted!");
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            PlayerPrefs.SetInt("Coins",9999);
            Debug.Log("Coins set to 9999");
        }
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void PlaySound(string soundName)
    {
        Toolkit.playSound(soundName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
