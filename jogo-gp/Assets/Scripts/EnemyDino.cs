using UnityEngine;

public class EnemyDino : MonoBehaviour
{
    public float speed = 3.5f;      // mais rápido que zumbi normal
    public int damage = 25;         // mais forte
    public int hp = 80;             // mais vida
    public float attackRate = 0.8f;
    public GameObject bloodParticlePrefab;

    private bool reachedBarrier = false;
    private Barrier barrier;
    private float nextAttackTime = 0f;

    void Update()
    {
        if (reachedBarrier)
        {
            if (Time.time >= nextAttackTime && barrier != null)
            {
                barrier.TakeDamage(damage);
                nextAttackTime = Time.time + attackRate;
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Barrier")
        {
            reachedBarrier = true;
            barrier = col.gameObject.GetComponent<Barrier>();
        }
    }

    public void TakeDamage(int amount)
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
                hud.AddMoney(25); // dropta mais dinheiro

            if (WaveManager.Instance != null)
                WaveManager.Instance.OnEnemyDied();

            Destroy(gameObject, 0.05f);
        }
    }
}