using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000027 RID: 39
	[NativeHeader("Modules/Physics2D/Public/BoxCollider2D.h")]
	public sealed class BoxCollider2D : Collider2D
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060003BC RID: 956 RVA: 0x000083E8 File Offset: 0x000065E8
		// (set) Token: 0x060003BD RID: 957 RVA: 0x000083FE File Offset: 0x000065FE
		public Vector2 size
		{
			get
			{
				Vector2 vector;
				this.get_size_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_size_Injected(ref value);
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060003BE RID: 958
		// (set) Token: 0x060003BF RID: 959
		public extern float edgeRadius
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060003C0 RID: 960
		// (set) Token: 0x060003C1 RID: 961
		public extern bool autoTiling
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x00008408 File Offset: 0x00006608
		// (set) Token: 0x060003C3 RID: 963 RVA: 0x00005567 File Offset: 0x00003767
		[Obsolete("BoxCollider2D.center has been deprecated. Use BoxCollider2D.offset instead (UnityUpgradable) -> offset", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public Vector2 center
		{
			get
			{
				return Vector2.zero;
			}
			set
			{
			}
		}

		// Token: 0x060003C5 RID: 965
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_size_Injected(out Vector2 ret);

		// Token: 0x060003C6 RID: 966
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_size_Injected(ref Vector2 value);
	}
}
