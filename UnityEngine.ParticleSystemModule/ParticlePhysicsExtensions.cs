using System;
using System.Collections.Generic;

namespace UnityEngine
{
	// Token: 0x0200002E RID: 46
	public static class ParticlePhysicsExtensions
	{
		// Token: 0x060006AB RID: 1707 RVA: 0x00005C50 File Offset: 0x00003E50
		[Obsolete("GetCollisionEvents function using ParticleCollisionEvent[] is deprecated. Use List<ParticleCollisionEvent> instead.", false)]
		public static int GetCollisionEvents(this ParticleSystem ps, GameObject go, ParticleCollisionEvent[] collisionEvents)
		{
			bool flag = go == null;
			if (flag)
			{
				throw new ArgumentNullException("go");
			}
			bool flag2 = collisionEvents == null;
			if (flag2)
			{
				throw new ArgumentNullException("collisionEvents");
			}
			return ParticleSystemExtensionsImpl.GetCollisionEventsDeprecated(ps, go, collisionEvents);
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00005C94 File Offset: 0x00003E94
		public static int GetSafeCollisionEventSize(this ParticleSystem ps)
		{
			return ParticleSystemExtensionsImpl.GetSafeCollisionEventSize(ps);
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x00005CAC File Offset: 0x00003EAC
		public static int GetCollisionEvents(this ParticleSystem ps, GameObject go, List<ParticleCollisionEvent> collisionEvents)
		{
			return ParticleSystemExtensionsImpl.GetCollisionEvents(ps, go, collisionEvents);
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00005CC8 File Offset: 0x00003EC8
		public static int GetSafeTriggerParticlesSize(this ParticleSystem ps, ParticleSystemTriggerEventType type)
		{
			return ParticleSystemExtensionsImpl.GetSafeTriggerParticlesSize(ps, (int)type);
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00005CE4 File Offset: 0x00003EE4
		public static int GetTriggerParticles(this ParticleSystem ps, ParticleSystemTriggerEventType type, List<ParticleSystem.Particle> particles)
		{
			return ParticleSystemExtensionsImpl.GetTriggerParticles(ps, (int)type, particles);
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x00005D00 File Offset: 0x00003F00
		public static int GetTriggerParticles(this ParticleSystem ps, ParticleSystemTriggerEventType type, List<ParticleSystem.Particle> particles, out ParticleSystem.ColliderData colliderData)
		{
			bool flag = type == ParticleSystemTriggerEventType.Exit;
			if (flag)
			{
				throw new InvalidOperationException("Querying the collider data for the Exit event is not currently supported.");
			}
			bool flag2 = type == ParticleSystemTriggerEventType.Outside;
			if (flag2)
			{
				throw new InvalidOperationException("Querying the collider data for the Outside event is not supported, because when a particle is outside the collision volume, it is always outside every collider.");
			}
			colliderData = default(ParticleSystem.ColliderData);
			return ParticleSystemExtensionsImpl.GetTriggerParticlesWithData(ps, (int)type, particles, ref colliderData);
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x00005D48 File Offset: 0x00003F48
		public static void SetTriggerParticles(this ParticleSystem ps, ParticleSystemTriggerEventType type, List<ParticleSystem.Particle> particles, int offset, int count)
		{
			bool flag = particles == null;
			if (flag)
			{
				throw new ArgumentNullException("particles");
			}
			bool flag2 = offset >= particles.Count;
			if (flag2)
			{
				throw new ArgumentOutOfRangeException("offset", "offset should be smaller than the size of the particles list.");
			}
			bool flag3 = offset + count >= particles.Count;
			if (flag3)
			{
				throw new ArgumentOutOfRangeException("count", "offset+count should be smaller than the size of the particles list.");
			}
			ParticleSystemExtensionsImpl.SetTriggerParticles(ps, (int)type, particles, offset, count);
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00005DB8 File Offset: 0x00003FB8
		public static void SetTriggerParticles(this ParticleSystem ps, ParticleSystemTriggerEventType type, List<ParticleSystem.Particle> particles)
		{
			ParticleSystemExtensionsImpl.SetTriggerParticles(ps, (int)type, particles, 0, particles.Count);
		}
	}
}
