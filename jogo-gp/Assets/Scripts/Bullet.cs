using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 10;
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
            if (bloodParticlePrefab != null)
            {
                GameObject blood = Instantiate(bloodParticlePrefab, transform.position, Quaternion.identity);
                blood.transform.parent = null;
            }

            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        Enemy enemy = col.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (bloodParticlePrefab != null)
            {
                GameObject blood = Instantiate(bloodParticlePrefab, transform.position, Quaternion.identity);
                blood.transform.parent = null;
            }

            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}