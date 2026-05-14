using UnityEngine;

public enum AllyType
{
    Shotgun,
    MachineGun,
    Sniper,
    RPG
}

public class Ally : MonoBehaviour
{
    public AllyType allyType;
    public GameObject allyBulletPrefab;
    public GameObject allyBloodPrefab;
    public Transform allyFirePoint;

    private float nextFireTime = 0f;
    private float allyFireRate;
    private int allyDamage;
    private float allyBulletSpeed;
    private int allyBulletCount;
    private float allySpreadAngle;
    private bool allyPiercing;
    private bool allyExplosive;
    private float allyExplosionRadius;

    void Start()
    {
        SetupByType();
    }

    public void SetupByType()
    {
        switch (allyType)
        {
            case AllyType.Shotgun:
                allyFireRate = 1.0f;
                allyDamage = 8;
                allyBulletSpeed = 12f;
                allyBulletCount = 5;
                allySpreadAngle = 20f;
                allyPiercing = false;
                allyExplosive = false;
                allyExplosionRadius = 0f;
                break;

            case AllyType.MachineGun:
                allyFireRate = 0.1f;
                allyDamage = 5;
                allyBulletSpeed = 18f;
                allyBulletCount = 1;
                allySpreadAngle = 5f;
                allyPiercing = false;
                allyExplosive = false;
                allyExplosionRadius = 0f;
                break;

            case AllyType.Sniper:
                allyFireRate = 1.5f;
                allyDamage = 80;
                allyBulletSpeed = 30f;
                allyBulletCount = 1;
                allySpreadAngle = 0f;
                allyPiercing = true;
                allyExplosive = false;
                allyExplosionRadius = 0f;
                break;

            case AllyType.RPG:
                allyFireRate = 2.5f;
                allyDamage = 50;
                allyBulletSpeed = 8f;
                allyBulletCount = 1;
                allySpreadAngle = 0f;
                allyPiercing = false;
                allyExplosive = true;
                allyExplosionRadius = 1.5f;
                break;
        }
    }
    Transform FindClosestTarget()
    {
        float minDist = Mathf.Infinity;
        Transform closest = null;

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy e in enemies)
        {
            float dist = Vector2.Distance(transform.position, e.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = e.transform;
            }
        }

        EnemyDino[] dinos = FindObjectsOfType<EnemyDino>();
        foreach (EnemyDino d in dinos)
        {
            float dist = Vector2.Distance(transform.position, d.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = d.transform;
            }
        }

        return closest;
    }
    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Transform target = FindClosestTarget();
            if (target != null)
            {
                nextFireTime = Time.time + allyFireRate;
                ShootAt(target.position);
            }
        }
    }

    Enemy FindClosestEnemy()
    {
        float minDist = Mathf.Infinity;
        Transform closestTarget = null;

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy e in enemies)
        {
            float dist = Vector2.Distance(transform.position, e.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closestTarget = e.transform;
            }
        }

        EnemyDino[] dinos = FindObjectsOfType<EnemyDino>();
        foreach (EnemyDino d in dinos)
        {
            float dist = Vector2.Distance(transform.position, d.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closestTarget = d.transform;
            }
        }

        if (closestTarget == null) return null;

        return closestTarget.GetComponent<Enemy>();
    }

    void ShootAt(Vector3 targetPos)
    {
        Vector3 direction = targetPos - allyFirePoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion baseRot = Quaternion.Euler(0, 0, angle);

        if (allyBulletCount == 1)
        {
            SpawnBullet(baseRot);
        }
        else
        {
            float half = allySpreadAngle / 2f;
            for (int i = 0; i < allyBulletCount; i++)
            {
                float a = Random.Range(-half, half);
                Quaternion rot = baseRot * Quaternion.Euler(0, 0, a);
                SpawnBullet(rot);
            }
        }
    }

    void SpawnBullet(Quaternion rotation)
    {
        GameObject bulletObj = Instantiate(allyBulletPrefab, allyFirePoint.position, rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.speed = allyBulletSpeed;
            bullet.damage = allyDamage;
            bullet.piercing = allyPiercing;
            bullet.explosive = allyExplosive;
            bullet.explosionRadius = allyExplosionRadius;
            bullet.bloodParticlePrefab = allyBloodPrefab;
        }
    }
}