using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI waveHUDText;
    public Barrier barrier;
    public int money = 0;

    void Update()
    {
        moneyText.text = "$ " + money;

        if (barrier != null)
            healthText.text = "Vida: " + barrier.currentHP;

        // Atualiza arma atual
        if (weaponText != null && WeaponManager.Instance != null)
            weaponText.text = "Arma: " + GetWeaponName(WeaponManager.Instance.currentWeapon);

        // Atualiza wave atual
        if (waveHUDText != null && WaveManager.Instance != null)
            waveHUDText.text = "Wave " + WaveManager.Instance.GetCurrentWave();
    }

    string GetWeaponName(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Pistol: return "Pistola";
            case WeaponType.Shotgun: return "Escopeta";
            case WeaponType.MachineGun: return "Metralhadora";
            case WeaponType.Sniper: return "Sniper";
            case WeaponType.RPG: return "RPG";
            default: return "Pistola";
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }
}