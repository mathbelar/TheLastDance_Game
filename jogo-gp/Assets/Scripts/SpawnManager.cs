using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 2f;   
    public float spawnX = 10f;         
    public float spawnYMin = -4.5f;    
    public float spawnYMax = 0f;      

    void Start()
    {
        InvokeRepeating("SpawnEnemy", 1f, spawnInterval);
    }

    void SpawnEnemy()
    {
        float randomY = Random.Range(spawnYMin, spawnYMax);
        Vector3 spawnPos = new Vector3(spawnX, randomY, 0f);

        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}