using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200002E RID: 46
	[NativeHeader("Modules/Physics/SphereCollider.h")]
	[RequiredByNativeCode]
	public class SphereCollider : Collider
	{
		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600037B RID: 891 RVA: 0x00005B60 File Offset: 0x00003D60
		// (set) Token: 0x0600037C RID: 892 RVA: 0x00005B76 File Offset: 0x00003D76
		public Vector3 center
		{
			get
			{
				Vector3 vector;
				this.get_center_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_center_Injected(ref value);
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x0600037D RID: 893
		// (set) Token: 0x0600037E RID: 894
		public extern float radius
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000380 RID: 896
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_center_Injected(out Vector3 ret);

		// Token: 0x06000381 RID: 897
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_center_Injected(ref Vector3 value);
	}
}
