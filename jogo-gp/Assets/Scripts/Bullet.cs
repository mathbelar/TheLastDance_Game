using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 10;
    public bool piercing = false;
    public bool explosive = false;
    public float explosionRadius = 0f;
    public GameObject bloodParticlePrefab;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;
        Destroy(gameObject, 3f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null)
        {
            SpawnBlood();
            if (explosive) Explode();
            else { enemy.TakeDamage(damage); if (!piercing) Destroy(gameObject); }
            return;
        }

        // Dino
        EnemyDino dino = col.GetComponent<EnemyDino>();
        if (dino != null)
        {
            SpawnBlood();
            if (explosive) ExplodeDino();
            else { dino.TakeDamage(damage); if (!piercing) Destroy(gameObject); }
            return;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null && !piercing && !explosive)
        {
            SpawnBlood();
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    void Explode()
    {
        // Pega todos os inimigos no raio da explosão
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                SpawnBlood();
                enemy.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
    }
    void ExplodeDino()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null) { SpawnBlood(); enemy.TakeDamage(damage); }

            EnemyDino dino = hit.GetComponent<EnemyDino>();
            if (dino != null) { SpawnBlood(); dino.TakeDamage(damage); }
        }
        Destroy(gameObject);
    }

    void SpawnBlood()
    {
        if (bloodParticlePrefab != null)
        {
            GameObject blood = Instantiate(bloodParticlePrefab, transform.position, Quaternion.identity);
            blood.transform.parent = null;
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}