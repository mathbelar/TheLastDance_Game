---
name: Ally Architecture
description: AllyType enum, SetupByType configuration pattern, and SpawnBullet wiring in Ally.cs
type: project
---

**AllyType enum:** Shotgun, MachineGun, Sniper, RPG (value 3 in serialized prefab data).

**Configuration:** `SetupByType()` called from `Start()` sets all private firing parameters (rate, damage, speed, count, spread, piercing, explosive, explosionRadius) by switch on `allyType`.

**SpawnBullet pattern:** All ally types share one bullet prefab (`allyBulletPrefab`). Stats are applied to the instantiated `Bullet` component each time. Per-weapon visual/effect overrides are applied with an `if (allyType == AllyType.RPG)` guard immediately after.

**Serialized fields on AllyRPG.prefab:**
- `allyType: 3` (RPG)
- `allyBulletPrefab` → Bullet.prefab
- `allyBloodPrefab` → BloodParticle.prefab
- `allyFirePoint` → child transform at local x:0.4
- `rpgBulletSprite` → rpg-bullet_0 sprite from rpg-bullet.png
- `allyExplosionEffectPrefab` → ExplosionEffect.prefab

**FindClosestTarget()** exists twice in Ally.cs (as FindClosestTarget and FindClosestEnemy) — the latter returns `Enemy` component and appears unused by Update; FindClosestTarget returns `Transform` and is what Update actually calls. This is existing code, not touched.
