using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public GameObject zombiePrefab;
    public float spawnInterval = 20f;
    public float spawnX = 11f;
    public float spawnY = -2.5f;

    void Start()
    {
        StartCoroutine(SpawnZombies());
    }

    IEnumerator SpawnZombies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnZombie();
        }
    }

    void SpawnZombie()
    {
        if (zombiePrefab != null)
        {
            Vector3 pos = new Vector3(spawnX, spawnY, 0f);
            Instantiate(zombiePrefab, pos, Quaternion.identity);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OpenTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}