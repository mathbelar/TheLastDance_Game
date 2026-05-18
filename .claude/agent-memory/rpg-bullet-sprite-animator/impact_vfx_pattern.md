---
name: Impact VFX Pattern
description: How explosion and blood particle effects are instantiated on hit; the stopAction Destroy pattern used for all one-shot VFX
type: project
---

**Pattern used in this project:**
All one-shot VFX (blood, explosion) are Unity ParticleSystem prefabs with:
- `looping: 0` — plays once
- `playOnAwake: 1` — starts immediately on Instantiate
- `stopAction: 2` — destroys the GameObject automatically when the system finishes (no manual Destroy call needed)

**BloodParticle.prefab** — red splatter, ~0.3s lifetime, used for all weapon hits.
**ExplosionEffect.prefab** — orange/yellow radial burst, ~0.5s lifetime, 12-20 particles in a 360-degree cone from a 0.1 radius origin. Uses SizeByLifetime curve (grow then shrink). ColorOverLifetime fades from yellow-orange to grey transparent.

**Instantiation call site:**
Both effects use the same pattern in Bullet.cs:
```csharp
GameObject fx = Instantiate(prefab, transform.position, Quaternion.identity);
fx.transform.parent = null;
```
The `parent = null` call ensures the effect survives after the bullet GameObject is destroyed.

**Why stopAction: 2 is the right choice:** It avoids the need for a cleanup script on each VFX prefab. Unity destroys the object the frame after the particle system stops, so there is no memory leak.
