using UnityEngine;

public class Barrier : MonoBehaviour
{
    public int maxHP = 1000;
    public int currentHP;

    void Start()
    {
        currentHP = maxHP;
        Debug.Log("Barreira iniciada com HP: " + currentHP);
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log("Barreira tomou dano! HP atual: " + currentHP);

        if (currentHP <= 0)
        {
            currentHP = 0;  
            GameController.Instance.TriggerGameOver();
        }
    }
}