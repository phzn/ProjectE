using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200005C RID: 92
	internal class ParticleSystemExtensionsImpl
	{
		// Token: 0x0600070E RID: 1806
		[FreeFunction(Name = "ParticleSystemScriptBindings::GetSafeCollisionEventSize")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetSafeCollisionEventSize([NotNull("ArgumentNullException")] ParticleSystem ps);

		// Token: 0x0600070F RID: 1807
		[FreeFunction(Name = "ParticleSystemScriptBindings::GetCollisionEventsDeprecated")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetCollisionEventsDeprecated([NotNull("ArgumentNullException")] ParticleSystem ps, GameObject go, [Out] ParticleCollisionEvent[] collisionEvents);

		// Token: 0x06000710 RID: 1808
		[FreeFunction(Name = "ParticleSystemScriptBindings::GetSafeTriggerParticlesSize")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetSafeTriggerParticlesSize([NotNull("ArgumentNullException")] ParticleSystem ps, int type);

		// Token: 0x06000711 RID: 1809
		[FreeFunction(Name = "ParticleSystemScriptBindings::GetCollisionEvents")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetCollisionEvents([NotNull("ArgumentNullException")] ParticleSystem ps, [NotNull("ArgumentNullException")] GameObject go, [NotNull("ArgumentNullException")] List<ParticleCollisionEvent> collisionEvents);

		// Token: 0x06000712 RID: 1810
		[FreeFunction(Name = "ParticleSystemScriptBindings::GetTriggerParticles")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetTriggerParticles([NotNull("ArgumentNullException")] ParticleSystem ps, int type, [NotNull("ArgumentNullException")] List<ParticleSystem.Particle> particles);

		// Token: 0x06000713 RID: 1811
		[FreeFunction(Name = "ParticleSystemScriptBindings::GetTriggerParticlesWithData")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern int GetTriggerParticlesWithData([NotNull("ArgumentNullException")] ParticleSystem ps, int type, [NotNull("ArgumentNullException")] List<ParticleSystem.Particle> particles, ref ParticleSystem.ColliderData colliderData);

		// Token: 0x06000714 RID: 1812
		[FreeFunction(Name = "ParticleSystemScriptBindings::SetTriggerParticles")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetTriggerParticles([NotNull("ArgumentNullException")] ParticleSystem ps, int type, [NotNull("ArgumentNullException")] List<ParticleSystem.Particle> particles, int offset, int count);
	}
}
