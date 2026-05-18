using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class MusicManagerSetup : EditorWindow
{
    private AudioClip menuMusicClip;
    private AudioClip gameMusicClip;
    private float volume = 0.5f;

    [MenuItem("Tools/Setup Music Manager")]
    public static void OpenWindow()
    {
        GetWindow<MusicManagerSetup>("Setup Music Manager");
    }

    void OnGUI()
    {
        GUILayout.Label("Music Manager Setup", EditorStyles.boldLabel);
        GUILayout.Space(10);

        menuMusicClip = (AudioClip)EditorGUILayout.ObjectField("Menu Music", menuMusicClip, typeof(AudioClip), false);
        gameMusicClip = (AudioClip)EditorGUILayout.ObjectField("Game Music", gameMusicClip, typeof(AudioClip), false);
        volume = EditorGUILayout.Slider("Volume", volume, 0f, 1f);

        GUILayout.Space(10);
        EditorGUILayout.HelpBox("Isto cria o MusicManager na cena atual.\nAbre a MenuScene antes de clicar.", MessageType.Info);
        GUILayout.Space(5);

        if (GUILayout.Button("Criar MusicManager na cena atual", GUILayout.Height(40)))
            CreateMusicManager();
    }

    void CreateMusicManager()
    {
        // Remove duplicado se existir
        MusicManager existing = Object.FindObjectOfType<MusicManager>();
        if (existing != null)
        {
            DestroyImmediate(existing.gameObject);
            Debug.Log("[MusicManager] Removido MusicManager antigo.");
        }

        // Cria novo objeto
        GameObject go = new GameObject("MusicManager");
        MusicManager mm = go.AddComponent<MusicManager>();
        mm.menuMusic = menuMusicClip;
        mm.gameMusic = gameMusicClip;
        mm.volume = volume;

        // Marca cena como modificada
        EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        Debug.Log("[MusicManager] MusicManager criado com sucesso! Guarda a cena (Ctrl+S).");
        EditorUtility.DisplayDialog("Sucesso!", "MusicManager criado na cena!\n\nGuarda a cena com Ctrl+S.", "OK");
    }
}
