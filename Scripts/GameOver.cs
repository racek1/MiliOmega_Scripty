using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI killText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI bestWaveText;

    void Start()
    {
        StartCoroutine(ShowKills());
        Cursor.visible = true;
        int wave = PlayerPrefs.GetInt("lastGameWave",0);
        int bestWave = PlayerPrefs.GetInt("highestWave",1);
        waveText.text = "Wave: " + wave;
        if (wave > bestWave)
        {
            PlayerPrefs.SetInt("highestWave",wave);
        }
        bestWaveText.text = "Best wave: " + PlayerPrefs.GetInt("highestWave", 1);
    }

    private IEnumerator ShowKills()
    {
        int kills = PlayerPrefs.GetInt("lastGameKills",0);
        WaitForSeconds delay = new WaitForSeconds(3 / kills);
        for (int i = 1; i < kills + 1; i++)
        {
            killText.text = i.ToString();
            yield return delay;
        }
    }
    public void GotoScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
