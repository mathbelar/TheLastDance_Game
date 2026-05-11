using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public GameObject bloodParticlePrefab;

    private float nextFireTime = 0f;

    void Update()
    {
        WeaponManager.WeaponData weapon = WeaponManager.Instance.GetCurrentWeapon();

        if (Mouse.current.leftButton.isPressed && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + weapon.fireRate;
            Shoot(weapon);
        }
    }

    void Shoot(WeaponManager.WeaponData weapon)
    {
        if (weapon.bulletCount == 1)
        {
            SpawnBullet(firePoint.rotation, weapon);
        }
        else
        {
            // Escopeta — spawna varias balas em cone
            float halfSpread = weapon.spreadAngle / 2f;
            for (int i = 0; i < weapon.bulletCount; i++)
            {
                float angle = Random.Range(-halfSpread, halfSpread);
                Quaternion spreadRot = firePoint.rotation * Quaternion.Euler(0, 0, angle);
                SpawnBullet(spreadRot, weapon);
            }
        }
    }

    void SpawnBullet(Quaternion rotation, WeaponManager.WeaponData weapon)
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.speed = weapon.bulletSpeed;
            bullet.damage = weapon.damage;
            bullet.piercing = weapon.piercing;
            bullet.explosive = weapon.explosive;
            bullet.explosionRadius = weapon.explosionRadius;
            bullet.bloodParticlePrefab = bloodParticlePrefab;
        }
    }
}