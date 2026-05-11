using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public GameObject gameOverPanel;
    public GameObject shopPanel;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI shopMoneyText;

    private bool gameOver = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (shopPanel != null)
            shopPanel.SetActive(false);
    }

    public void OpenShop()
    {
        Time.timeScale = 0f;

        if (waveText != null)
            waveText.text = "Wave " + WaveManager.Instance.GetCurrentWave() + " completa!";

        if (shopMoneyText != null)
        {
            HUDManager hud = FindObjectOfType<HUDManager>();
            if (hud != null)
                shopMoneyText.text = "$ " + hud.money;
        }

        if (shopPanel != null)
            shopPanel.SetActive(true);

        if (ShopManager.Instance != null)
            ShopManager.Instance.UpdateButtons();
    }

    public void ContinueGame()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);

        Time.timeScale = 1f;
        WaveManager.Instance.StartWave();
    }

    public void TriggerGameOver()
    {
        if (gameOver) return;
        gameOver = true;

        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}