using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Variables")]
    public GameObject[] normalEnemies;
    public GameObject[] advancedEnemies;
    public GameObject rainObj;
    public SpriteRenderer fog;
    public ParticleSystem rainEffect;
    public GameObject boss;

    private bool game = true;
    private float offsetFromPlayer = 15f;
    private Player player;
    private MapGenerator mapGenerator;
    private WaitForSeconds spawnDelay;
    private int stage = 1;
    [HideInInspector] public int wave = 1;
    private int enemiesAmount = 10;
    private float spawnTime = 0.5f;
    private float enemyHpMultiplier = 1;
    private bool isRaining = false;

    private List<GameObject> currentEnemies = new List<GameObject>();


    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        mapGenerator = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
        spawnDelay = new WaitForSeconds(spawnTime);
        StartCoroutine(GameLoop());
    }



    private IEnumerator GameLoop()
    {
        WaitForSeconds enemyCheckDelay = new WaitForSeconds(1f);
        yield return new WaitForSeconds(3);
        while (game)
        {
            StartCoroutine(SendWave());
            while (!allEnemiesKilled()) { yield return enemyCheckDelay; }
            EndWave();
        }

    }

    private IEnumerator SendWave()
    {
        player.ShowMessage("Wave " + wave + " incoming!");
        if (!isRaining && Random.Range(0, 3) == 0)
        {
            StartCoroutine(StartRain());
        }
        else if (isRaining && Random.Range(0, 2) == 0)
        {
            StartCoroutine(EndRain());
        }

        if (wave % 10 == 0) //Boss wave
        {
            SpawnBoss();
        }
        for (int i = 0; i < enemiesAmount; i++)
        {
            SpawnEnemy();
            yield return spawnDelay;
        }
    }

    private void EndWave()
    {
        enemiesAmount = Mathf.RoundToInt(enemiesAmount * 1.15f);
        wave++;
        spawnTime -= 0.05f;
        enemyHpMultiplier += 0.2f;
        spawnTime = Mathf.Clamp(spawnTime,0.1f,9999);
        player.GeneratePerks();
    }

    private void SpawnBoss()
    {
        int dirMult = 1;
        if (Random.Range(0, 2) == 0) { dirMult = -1; }
        Enemy enemyObj = boss.GetComponent<Enemy>();
        float posX = dirMult * Random.Range(player.transform.position.x + offsetFromPlayer, mapGenerator.size / 2);
        float posY = dirMult * Random.Range(player.transform.position.y + offsetFromPlayer, mapGenerator.size / 2);
        posX = Mathf.Clamp(posX, -mapGenerator.size / 2, mapGenerator.size / 2);
        posY = Mathf.Clamp(posY, -mapGenerator.size / 2, mapGenerator.size / 2);
        GameObject enemy = Instantiate(enemyObj.gameObject, new Vector2(posX, posY), Quaternion.identity);
        enemy.GetComponent<Enemy>().maxHp = Mathf.RoundToInt(enemyObj.maxHp * enemyHpMultiplier);
        currentEnemies.Add(enemy);
    }

    private void SpawnEnemy()
    {
        int dirMult = 1;
        if (Random.Range(0, 2) == 0) { dirMult = -1; }
        Enemy enemyObj = GetRandomEnemy();
        float posX = dirMult * Random.Range(player.transform.position.x + offsetFromPlayer,mapGenerator.size/2);
        float posY = dirMult * Random.Range(player.transform.position.y + offsetFromPlayer,mapGenerator.size/2);
        posX = Mathf.Clamp(posX,-mapGenerator.size/2,mapGenerator.size/2);
        posY = Mathf.Clamp(posY,-mapGenerator.size/2,mapGenerator.size/2);
        GameObject enemy = Instantiate(enemyObj.gameObject,new Vector2(posX,posY),Quaternion.identity);
        enemy.GetComponent<Enemy>().maxHp = Mathf.RoundToInt(enemyObj.maxHp * enemyHpMultiplier);
        enemy.GetComponent<Enemy>().speed *= Random.Range(0.8f,1.2f);
        currentEnemies.Add(enemy);
    }

    private Enemy GetRandomEnemy()
    {
        if (wave >= 3)
        {
            if (Random.Range(0, 6) == 0)
            {
                return advancedEnemies[Random.Range(0, advancedEnemies.Length)].GetComponent<Enemy>();
            }
            else
            {
                return normalEnemies[Random.Range(0, normalEnemies.Length)].GetComponent<Enemy>();
            }
        }
        else
        {
            return normalEnemies[Random.Range(0,normalEnemies.Length)].GetComponent<Enemy>();
        }
    }

    private bool allEnemiesKilled()
    {
        if (currentEnemies.Count != 0) { return false; }
        return true;
    }

    public void RemoveEnemyFromList()
    {
        currentEnemies.RemoveAt(currentEnemies.Count-1);
    }

    public IEnumerator StartRain()
    {
        isRaining = true;
        rainObj.SetActive(true);
        WaitForSeconds delay = new WaitForSeconds(0.075f);
        for (byte i = 0; i < 65; i++)
        {
            fog.color = new Color32(53,59,98,i);
            yield return delay;
        }
        rainEffect.gameObject.SetActive(true);
    }

    private IEnumerator EndRain()
    {
        rainEffect.gameObject.SetActive(false);
        WaitForSeconds delay = new WaitForSeconds(0.075f);
        for (byte i = 65; i > 0; i--)
        {
            fog.color = new Color32(53, 59, 98, i);
            yield return delay;
        }
        isRaining = false;
        rainObj.SetActive(false);
    }
}
