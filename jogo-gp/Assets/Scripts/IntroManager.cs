using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// IntroManager — Attach this to a GameObject in your Intro Scene.
///
/// Sequência:
///   1. Tela preta → Fade IN do texto "Dos criadores de Space Block 1 e Space Block 2"
///   2. Fade OUT do texto
///   3. Fade IN da capa do jogo
///   4. Fade OUT da capa
///   5. Para a música e carrega a cena do Menu
/// </summary>
public class IntroManager : MonoBehaviour
{
    [Header("Referências UI")]
    [Tooltip("Imagem de fundo preta (ocupa a tela toda)")]
    public Image blackBackground;

    [Tooltip("Texto 'Dos criadores de...'")]
    public Text creditText;

    [Tooltip("Imagem da capa do jogo")]
    public Image coverImage;

    [Header("Configurações")]
    [Tooltip("Nome da cena do Menu principal")]
    public string menuSceneName = "MenuScene";

    [Tooltip("Velocidade do fade (quanto maior, mais rápido)")]
    [Range(0.1f, 3f)]
    public float fadeSpeed = 1f;

    [Tooltip("Quanto tempo o texto fica visível no auge")]
    public float textHoldTime = 2f;

    [Tooltip("Quanto tempo a capa fica visível no auge")]
    public float coverHoldTime = 2.5f;

    [Header("Música")]
    [Tooltip("AudioClip da música da intro")]
    public AudioClip introMusic;

    [Tooltip("Volume da música (0 a 1)")]
    [Range(0f, 1f)]
    public float musicVolume = 1f;

    private AudioSource audioSource;

    // ---------------------------------------------------------------

    void Start()
    {
        // Configura o AudioSource automaticamente
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = introMusic;
        audioSource.volume = musicVolume;
        audioSource.loop = false;
        audioSource.playOnAwake = false;

        // Garante que tudo começa invisível (exceto o fundo preto)
        SetAlpha(creditText, 0f);
        SetAlpha(coverImage, 0f);
        SetAlpha(blackBackground, 1f);

        if (creditText != null)
            creditText.text = "Dos criadores de\nSpace Block 1 e Space Block 2";

        StartCoroutine(PlayIntro());
    }

    // ---------------------------------------------------------------

    IEnumerator PlayIntro()
    {
        // Inicia a música
        if (introMusic != null)
            audioSource.Play();

        // ── Etapa 1: Fade IN do texto ──────────────────────────────
        yield return StartCoroutine(FadeElement(creditText, 0f, 1f));

        // ── Etapa 2: Aguarda o texto visível ──────────────────────
        yield return new WaitForSeconds(textHoldTime);

        // ── Etapa 3: Fade OUT do texto ─────────────────────────────
        yield return StartCoroutine(FadeElement(creditText, 1f, 0f));

        // ── Etapa 4: Fade IN da capa ───────────────────────────────
        yield return StartCoroutine(FadeElement(coverImage, 0f, 1f));

        // ── Etapa 5: Aguarda a capa visível ───────────────────────
        yield return new WaitForSeconds(coverHoldTime);

        // ── Etapa 6: Fade OUT da capa ──────────────────────────────
        yield return StartCoroutine(FadeOutCover());

        // ── Etapa 7: Para a música e carrega o Menu ────────────────
        if (audioSource.isPlaying)
            audioSource.Stop();

        SceneManager.LoadScene(menuSceneName);
    }

    // ---------------------------------------------------------------
    IEnumerator FadeElement(Graphic element, float startAlpha, float endAlpha)
    {
        if (element == null) yield break;

        float elapsed = 0f;
        float duration = 1f / fadeSpeed;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            SetAlpha(element, Mathf.Lerp(startAlpha, endAlpha, t));
            yield return null;
        }

        SetAlpha(element, endAlpha);
    }

    IEnumerator FadeOutCover()
    {
        float elapsed = 0f;
        float duration = 1f / fadeSpeed;

        SetAlpha(blackBackground, 1f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            SetAlpha(coverImage, Mathf.Lerp(1f, 0f, t));
            yield return null;
        }

        SetAlpha(coverImage, 0f);
    }

    void SetAlpha(Graphic element, float alpha)
    {
        if (element == null) return;
        Color c = element.color;
        c.a = alpha;
        element.color = c;
    }

    // Pressionar qualquer tecla pula a intro
    void Update()
    {
        if (Input.anyKeyDown)
        {
            StopAllCoroutines();
            if (audioSource != null && audioSource.isPlaying)
                audioSource.Stop();
            SceneManager.LoadScene(menuSceneName);
        }
    }
}