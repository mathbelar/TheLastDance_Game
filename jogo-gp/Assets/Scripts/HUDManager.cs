using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI healthText;
    public Barrier barrier;

    public int money = 0;

    void Update()
    {
        moneyText.text = "$ " + money;

        if (barrier != null)
            healthText.text = "Vida: " + barrier.currentHP;
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }
}