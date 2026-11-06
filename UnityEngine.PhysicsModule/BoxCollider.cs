using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200002D RID: 45
	[NativeHeader("Modules/Physics/BoxCollider.h")]
	[RequiredByNativeCode]
	public class BoxCollider : Collider
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000370 RID: 880 RVA: 0x00005AE8 File Offset: 0x00003CE8
		// (set) Token: 0x06000371 RID: 881 RVA: 0x00005AFE File Offset: 0x00003CFE
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

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000372 RID: 882 RVA: 0x00005B08 File Offset: 0x00003D08
		// (set) Token: 0x06000373 RID: 883 RVA: 0x00005B1E File Offset: 0x00003D1E
		public Vector3 size
		{
			get
			{
				Vector3 vector;
				this.get_size_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_size_Injected(ref value);
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000374 RID: 884 RVA: 0x00005B28 File Offset: 0x00003D28
		// (set) Token: 0x06000375 RID: 885 RVA: 0x00005B4A File Offset: 0x00003D4A
		[Obsolete("Use BoxCollider.size instead. (UnityUpgradable) -> size")]
		public Vector3 extents
		{
			get
			{
				return this.size * 0.5f;
			}
			set
			{
				this.size = value * 2f;
			}
		}

		// Token: 0x06000377 RID: 887
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_center_Injected(out Vector3 ret);

		// Token: 0x06000378 RID: 888
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_center_Injected(ref Vector3 value);

		// Token: 0x06000379 RID: 889
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_size_Injected(out Vector3 ret);

		// Token: 0x0600037A RID: 890
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_size_Injected(ref Vector3 value);
	}
}
