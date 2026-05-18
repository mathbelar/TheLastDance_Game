---
name: Projectile System
description: How bullets are spawned, configured, and destroyed; Bullet.cs public API and impact callback flow
type: project
---

All projectiles share a single `Bullet.cs` MonoBehaviour attached to `Assets/PreFabs/Bullet.prefab`.

**Spawn flow:**
- Both `Ally.cs::SpawnBullet` and `PlayerShoot.cs::SpawnBullet` call `Instantiate(bulletPrefab, position, rotation)` and then set public fields on the resulting `Bullet` component in the same frame before `Start()` runs. This means fields set after Instantiate are safe — `Start()` reads them.

**Bullet.cs public fields (as of this session):**
- `speed`, `damage`, `piercing`, `explosive`, `explosionRadius` — core ballistics
- `bloodParticlePrefab` — particle spawned on any hit (blood splash)
- `overrideSprite` — if non-null, replaces the SpriteRenderer sprite in Start()
- `explosionEffectPrefab` — if non-null, Instantiated at impact point inside Explode()/ExplodeDino() only

**Impact callbacks:**
- `OnTriggerEnter2D` handles Enemy and EnemyDino hits. For explosive bullets it calls Explode() or ExplodeDino().
- `OnTriggerStay2D` handles non-piercing, non-explosive lingering contacts.
- `Explode()` / `ExplodeDino()` do OverlapCircleAll, deal AoE damage, then Destroy(gameObject).
- `OnBecameInvisible()` destroys bullet when off-screen (no explosion effect spawned here — intentional).
- `Destroy(gameObject, 3f)` in Start() is a safety timeout — also no explosion effect.

**Why:** Explosion effect is intentionally gated to Explode/ExplodeDino so it only fires on real collisions, not timeouts or out-of-bounds.
