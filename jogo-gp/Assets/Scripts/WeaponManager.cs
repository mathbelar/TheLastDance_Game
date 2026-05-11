using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public enum WeaponType
{
    Pistol,
    Shotgun,
    MachineGun,
    Sniper,
    RPG
}

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance;

    public WeaponType currentWeapon = WeaponType.Pistol;

    // Armas que o jogador já comprou
    private HashSet<WeaponType> unlockedWeapons = new HashSet<WeaponType>();

    [System.Serializable]
    public class WeaponData
    {
        public WeaponType type;
        public float fireRate;
        public int damage;
        public float bulletSpeed;
        public int bulletCount;
        public float spreadAngle;
        public bool piercing;
        public bool explosive;
        public float explosionRadius;
    }

    public WeaponData[] weapons = new WeaponData[]
    {
        new WeaponData { type = WeaponType.Pistol,     fireRate = 0.3f,  damage = 10, bulletSpeed = 15f, bulletCount = 1, spreadAngle = 0f,  piercing = false, explosive = false, explosionRadius = 0f },
        new WeaponData { type = WeaponType.Shotgun,    fireRate = 0.1f,  damage = 10,  bulletSpeed = 12f, bulletCount = 5, spreadAngle = 5f, piercing = false, explosive = false, explosionRadius = 0f },
        new WeaponData { type = WeaponType.MachineGun, fireRate = 0.08f, damage = 5,  bulletSpeed = 18f, bulletCount = 1, spreadAngle = 5f,  piercing = false, explosive = false, explosionRadius = 0f },
        new WeaponData { type = WeaponType.Sniper,     fireRate = 1.2f,  damage = 80, bulletSpeed = 30f, bulletCount = 1, spreadAngle = 0f,  piercing = true,  explosive = false, explosionRadius = 0f },
        new WeaponData { type = WeaponType.RPG,        fireRate = 2.0f,  damage = 50, bulletSpeed = 8f,  bulletCount = 1, spreadAngle = 0f,  piercing = false, explosive = true,  explosionRadius = 1.5f }
    };

    void Awake()
    {   
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Pistola sempre desbloqueada
        unlockedWeapons.Add(WeaponType.Pistol);
    }

    void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            TryEquip(WeaponType.Pistol);

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
            TryEquip(WeaponType.Shotgun);

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
            TryEquip(WeaponType.MachineGun);

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
            TryEquip(WeaponType.Sniper);

        if (Keyboard.current.digit5Key.wasPressedThisFrame)
            TryEquip(WeaponType.RPG);
    }

    void TryEquip(WeaponType type)
    {
        if (unlockedWeapons.Contains(type))
        {
            EquipWeapon(type);
        }
        else
        {
            Debug.Log("Arma não desbloqueada: " + type + " — compre na loja!");
        }
    }

    public void EquipWeapon(WeaponType type)
    {
        currentWeapon = type;
        Debug.Log("Arma equipada: " + type);
    }

    public void UnlockWeapon(WeaponType type)
    {
        unlockedWeapons.Add(type);
        Debug.Log("Arma desbloqueada: " + type);
    }

    public bool IsUnlocked(WeaponType type)
    {
        return unlockedWeapons.Contains(type);
    }

    public WeaponData GetCurrentWeapon()
    {
        foreach (var w in weapons)
            if (w.type == currentWeapon)
                return w;
        return weapons[0];
    }
}