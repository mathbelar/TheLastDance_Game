---
name: Player RPG Wiring
description: How PlayerShoot passes per-weapon overrides (sprite, explosion effect) to Bullet at spawn time for the RPG weapon type
type: project
---

**PlayerShoot.cs** does not have weapon-type-specific prefabs — all shots use `bulletPrefab` (a single shared prefab reference set in the Inspector). Per-weapon visual differences are applied at spawn time by setting fields on the `Bullet` component.

**New fields added to PlayerShoot:**
- `public Sprite rpgBulletSprite` — assign `rpg-bullet_0` in Inspector on the Player object
- `public GameObject explosionEffectPrefab` — assign `ExplosionEffect` prefab in Inspector on the Player object

**Where to wire in Unity Editor:**
Find the Player GameObject in the scene, select the `PlayerShoot` component, and assign:
- `Rpg Bullet Sprite` → the `rpg-bullet_0` sprite from `Assets/Sprites/rpg-bullet.png`
- `Explosion Effect Prefab` → `Assets/PreFabs/ExplosionEffect.prefab`

**WeaponData:** The RPG WeaponData entry in WeaponManager has `explosive: true, explosionRadius: 1.5f`. The `weapon.type == WeaponType.RPG` check in SpawnBullet uses this enum to gate the sprite/effect assignment.

**Note:** Unlike AllyRPG.prefab (which was wired directly in the prefab YAML), the Player object lives in the scene (`GameScene.unity`). The Inspector wiring for PlayerShoot must be done manually in the Unity Editor after opening the scene.
