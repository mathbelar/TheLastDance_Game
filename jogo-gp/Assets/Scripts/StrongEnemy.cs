using UnityEngine;

public class StrongEnemy : Enemy
{
    public int moneyReward = 150;

    void Start()
    {
        speed = 0.5f;
        damage = 25;
        hp = 200;
        attackRate = 0.5f;
    }

    public override void TakeDamage(int amount)
    {
        hp -= amount;

        if (bloodParticlePrefab != null)
        {
            GameObject blood = Instantiate(bloodParticlePrefab, transform.position, Quaternion.identity);
            blood.transform.parent = null;
        }

        if (hp <= 0)
        {
            HUDManager hud = FindObjectOfType<HUDManager>();
            if (hud != null)
                hud.AddMoney(moneyReward);

            if (WaveManager.Instance != null)
                WaveManager.Instance.OnEnemyDied();

            Destroy(gameObject, 0.05f);
        }
    }
}