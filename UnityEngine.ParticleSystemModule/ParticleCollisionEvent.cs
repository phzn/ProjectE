using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200005B RID: 91
	[RequiredByNativeCode(Optional = true)]
	public struct ParticleCollisionEvent
	{
		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x000065BC File Offset: 0x000047BC
		public Vector3 intersection
		{
			get
			{
				return this.m_Intersection;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600070A RID: 1802 RVA: 0x000065D4 File Offset: 0x000047D4
		public Vector3 normal
		{
			get
			{
				return this.m_Normal;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x000065EC File Offset: 0x000047EC
		public Vector3 velocity
		{
			get
			{
				return this.m_Velocity;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x00006604 File Offset: 0x00004804
		public Component colliderComponent
		{
			get
			{
				return ParticleCollisionEvent.InstanceIDToColliderComponent(this.m_ColliderInstanceID);
			}
		}

		// Token: 0x0600070D RID: 1805
		[FreeFunction(Name = "ParticleSystemScriptBindings::InstanceIDToColliderComponent")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Component InstanceIDToColliderComponent(int instanceID);

		// Token: 0x0400018A RID: 394
		internal Vector3 m_Intersection;

		// Token: 0x0400018B RID: 395
		internal Vector3 m_Normal;

		// Token: 0x0400018C RID: 396
		internal Vector3 m_Velocity;

		// Token: 0x0400018D RID: 397
		internal int m_ColliderInstanceID;
	}
}
