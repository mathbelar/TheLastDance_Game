using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 10;
    public bool piercing = false;
    public bool explosive = false;
    public float explosionRadius = 0f;
    public GameObject bloodParticlePrefab;

    // Assign to override the SpriteRenderer sprite at spawn time.
    // Used by PlayerShoot and Ally to swap in the rpg-bullet sprite
    // without needing a separate prefab variant per weapon.
    public Sprite overrideSprite = null;

    // Prefab for the explosion visual effect (particles).
    // Only spawned on impact — not on timeout or out-of-bounds destruction.
    public GameObject explosionEffectPrefab = null;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;

        // Apply optional sprite override so caller code can change visuals
        // without needing a dedicated prefab per weapon type.
        if (overrideSprite != null)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.sprite = overrideSprite;
        }

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
        // Spawn the explosion visual at the exact point of impact before
        // destroying the bullet so the position is still valid.
        SpawnExplosion();

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
        // Spawn the explosion visual at the exact point of impact.
        SpawnExplosion();

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

    // Spawns the explosion particle effect at the bullet's current world position.
    // Called only from Explode/ExplodeDino so it never fires on timeout destruction.
    void SpawnExplosion()
    {
        if (explosionEffectPrefab != null)
        {
            GameObject fx = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            fx.transform.parent = null;
        }
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