using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public int damage = 10;
    public int hp = 30;
    public float attackRate = 1f;
    public GameObject bloodParticlePrefab; 

    private bool reachedBarrier = false;
    private Barrier barrier;
    private float nextAttackTime = 0f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isWalking", true);
    }

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

            if (animator != null)
                animator.SetBool("isWalking", false);
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
                hud.AddMoney(10);

            Destroy(gameObject, 0.05f); 
        }
    }
}