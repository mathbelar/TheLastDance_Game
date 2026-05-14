using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    public GameObject enemyPrefab;
    public GameObject dinoPrefab;      // arraste EnemyDino aqui
    public GameObject strongEnemyPrefab; // zumbi fortao
    public float spawnX = 10f;
    public float spawnYMin = -3.5f;
    public float spawnYMax = 0.5f;
    public float timeBetweenSpawns = 1.5f;
    public int dinoStartWave = 3;      // dino começa na wave 3
    public int strongEnemyStartWave = 5;

    private int currentWave = 0;
    private int enemiesAlive = 0;
    private int enemiesSpawned = 0;
    private int totalEnemies = 0;


    private int strongEnemiesSpawned = 0; // numero de zumbis fortões já spawnados na wave atual
    private int strongEnemiesThisWave = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        StartWave();
    }

    public void StartWave()
    {
        currentWave++;
        strongEnemiesSpawned = 0; // resetar o número de zumbis fortões spawnados
        totalEnemies = 20 + (currentWave - 1) * 5;
        enemiesAlive = 0;
        enemiesSpawned = 0;

        if (currentWave >= strongEnemyStartWave)
            strongEnemiesThisWave = 1 + (currentWave - strongEnemyStartWave);
        else
            strongEnemiesThisWave = 0;


        Debug.Log("Wave " + currentWave + " iniciada! Inimigos: " + totalEnemies);
        StartCoroutine(SpawnEnemies(totalEnemies));
    }

    IEnumerator SpawnEnemies(int total)
    {
        while (enemiesSpawned < total)
        {
            SpawnEnemy();
            enemiesSpawned++;
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

void SpawnEnemy()
{
    float randomY = Random.Range(spawnYMin, spawnYMax);
    Vector3 spawnPos = new Vector3(spawnX, randomY, 0f);

    if (currentWave >= 4 && dinoPrefab != null && Random.value < 0.3f)
    {
        Instantiate(dinoPrefab, spawnPos, Quaternion.identity);
    }
    else if (strongEnemyPrefab != null && currentWave >= strongEnemyStartWave && strongEnemiesSpawned < strongEnemiesThisWave)
        {
        Instantiate(strongEnemyPrefab, spawnPos, Quaternion.identity);
        strongEnemiesSpawned++;
    }
        else
    {
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }

    enemiesAlive++;
}

    public void OnEnemyDied()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0 && enemiesSpawned >= totalEnemies)
        {
            Debug.Log("Wave " + currentWave + " concluída!");
            GameController.Instance.OpenShop();
        }
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }
}