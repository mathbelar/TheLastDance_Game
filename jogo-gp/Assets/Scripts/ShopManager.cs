using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [Header("Botões das Armas")]
    public Button btnShotgun;
    public Button btnMachineGun;
    public Button btnSniper;
    public Button btnRPG;

    [Header("Botões dos Aliados")]
    public Button btnAllyShotgun;
    public Button btnAllyMachineGun;
    public Button btnAllySniper;
    public Button btnAllyRPG;

    [Header("Preços Armas")]
    public int priceShotgun = 600;
    public int priceMachineGun = 1200;
    public int priceSniper = 3000;
    public int priceRPG = 8000;

    [Header("Preços Aliados")]
    public int priceAllyShotgun = 1200;
    public int priceAllyMachineGun = 2400;
    public int priceAllySniper = 6000;
    public int priceAllyRPG = 12000;

    private bool boughtAllyShotgun = false;
    private bool boughtAllyMachineGun = false;
    private bool boughtAllySniper = false;
    private bool boughtAllyRPG = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Armas
        btnShotgun.onClick.AddListener(() => BuyWeapon(WeaponType.Shotgun, priceShotgun));
        btnMachineGun.onClick.AddListener(() => BuyWeapon(WeaponType.MachineGun, priceMachineGun));
        btnSniper.onClick.AddListener(() => BuyWeapon(WeaponType.Sniper, priceSniper));
        btnRPG.onClick.AddListener(() => BuyWeapon(WeaponType.RPG, priceRPG));

        // Aliados
        btnAllyShotgun.onClick.AddListener(() => BuyAllyByType(AllyType.Shotgun));
        btnAllyMachineGun.onClick.AddListener(() => BuyAllyByType(AllyType.MachineGun));
        btnAllySniper.onClick.AddListener(() => BuyAllyByType(AllyType.Sniper));
        btnAllyRPG.onClick.AddListener(() => BuyAllyByType(AllyType.RPG));
    }

    void BuyWeapon(WeaponType type, int price)
    {
        HUDManager hud = FindObjectOfType<HUDManager>();
        if (hud == null) return;

        if (WeaponManager.Instance.IsUnlocked(type))
        {
            WeaponManager.Instance.EquipWeapon(type);
            return;
        }

        if (hud.money >= price)
        {
            hud.money -= price;
            WeaponManager.Instance.UnlockWeapon(type);
            WeaponManager.Instance.EquipWeapon(type);
            UpdateButtons();
        }
        else
            Debug.Log("Dinheiro insuficiente!");
    }

    void BuyAllyByType(AllyType type)
    {
        HUDManager hud = FindObjectOfType<HUDManager>();
        if (hud == null) return;

        int price = 0;
        Button btn = null;

        switch (type)
        {
            case AllyType.Shotgun:
                if (boughtAllyShotgun) { Debug.Log("Já comprado!"); return; }
                price = priceAllyShotgun;
                btn = btnAllyShotgun;
                break;
            case AllyType.MachineGun:
                if (boughtAllyMachineGun) { Debug.Log("Já comprado!"); return; }
                price = priceAllyMachineGun;
                btn = btnAllyMachineGun;
                break;
            case AllyType.Sniper:
                if (boughtAllySniper) { Debug.Log("Já comprado!"); return; }
                price = priceAllySniper;
                btn = btnAllySniper;
                break;
            case AllyType.RPG:
                if (boughtAllyRPG) { Debug.Log("Já comprado!"); return; }
                price = priceAllyRPG;
                btn = btnAllyRPG;
                break;
        }

        if (!AllyManager.Instance.IsSlotAvailable())
        {
            Debug.Log("Sem slots disponíveis!");
            return;
        }

        if (hud.money >= price)
        {
            hud.money -= price;
            AllyManager.Instance.SpawnAlly(type);

            switch (type)
            {
                case AllyType.Shotgun: boughtAllyShotgun = true; break;
                case AllyType.MachineGun: boughtAllyMachineGun = true; break;
                case AllyType.Sniper: boughtAllySniper = true; break;
                case AllyType.RPG: boughtAllyRPG = true; break;
            }

            if (btn != null) btn.interactable = false;
            Debug.Log("Aliado comprado: " + type);
        }
        else
        {
            Debug.Log("Dinheiro insuficiente!");
        }
    }

    public void UpdateButtons()
    {
        WeaponType current = WeaponManager.Instance.currentWeapon;

        btnShotgun.interactable = !WeaponManager.Instance.IsUnlocked(WeaponType.Shotgun) && current != WeaponType.Shotgun;
        btnMachineGun.interactable = !WeaponManager.Instance.IsUnlocked(WeaponType.MachineGun) && current != WeaponType.MachineGun;
        btnSniper.interactable = !WeaponManager.Instance.IsUnlocked(WeaponType.Sniper) && current != WeaponType.Sniper;
        btnRPG.interactable = !WeaponManager.Instance.IsUnlocked(WeaponType.RPG) && current != WeaponType.RPG;
    }
}