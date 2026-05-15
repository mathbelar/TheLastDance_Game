using UnityEngine;

public class AllyManager : MonoBehaviour
{
    public static AllyManager Instance;

    public GameObject allyPrefabShotgun;
    public GameObject allyPrefabMachineGun;
    public GameObject allyPrefabSniper;
    public GameObject allyPrefabRPG;

    public Vector3[] allyPositions = new Vector3[]
    {
        new Vector3(-6.6f, -1.3f, 0f),
        new Vector3(-6.6f, -2.0f, 0f),
        new Vector3(-6.6f, -2.7f, 0f),
        new Vector3(-6.6f, -3.4f, 0f)
    };

    private bool[] slotOccupied = new bool[4];
    private GameObject[] allyObjects = new GameObject[4];

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool SpawnAlly(AllyType type)
    {
        for (int i = 0; i < allyPositions.Length; i++)
        {
            if (!slotOccupied[i])
            {
                slotOccupied[i] = true;

                GameObject prefab = GetPrefabByType(type);
                if (prefab == null)
                {
                    Debug.Log("Prefab não encontrado para: " + type);
                    return false;
                }

                GameObject ally = Instantiate(prefab, allyPositions[i], Quaternion.identity);
                Ally allyScript = ally.GetComponent<Ally>();
                allyScript.allyType = type;
                allyScript.SetupByType();
                allyObjects[i] = ally;
                Debug.Log("Aliado spawnado: " + type + " no slot " + i);
                return true;
            }
        }
        Debug.Log("Todos os slots de aliados estão ocupados!");
        return false;
    }

    GameObject GetPrefabByType(AllyType type)
    {
        switch (type)
        {
            case AllyType.Shotgun: return allyPrefabShotgun;
            case AllyType.MachineGun: return allyPrefabMachineGun;
            case AllyType.Sniper: return allyPrefabSniper;
            case AllyType.RPG: return allyPrefabRPG;
            default: return null;
        }
    }

    public bool IsSlotAvailable()
    {
        foreach (bool b in slotOccupied)
            if (!b) return true;
        return false;
    }
}