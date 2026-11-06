using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000034 RID: 52
	[NativeHeader("Modules/Physics2D/TargetJoint2D.h")]
	public sealed class TargetJoint2D : Joint2D
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x00008880 File Offset: 0x00006A80
		// (set) Token: 0x06000462 RID: 1122 RVA: 0x00008896 File Offset: 0x00006A96
		public Vector2 anchor
		{
			get
			{
				Vector2 vector;
				this.get_anchor_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_anchor_Injected(ref value);
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x000088A0 File Offset: 0x00006AA0
		// (set) Token: 0x06000464 RID: 1124 RVA: 0x000088B6 File Offset: 0x00006AB6
		public Vector2 target
		{
			get
			{
				Vector2 vector;
				this.get_target_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_target_Injected(ref value);
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000465 RID: 1125
		// (set) Token: 0x06000466 RID: 1126
		public extern bool autoConfigureTarget
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000467 RID: 1127
		// (set) Token: 0x06000468 RID: 1128
		public extern float maxForce
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000469 RID: 1129
		// (set) Token: 0x0600046A RID: 1130
		public extern float dampingRatio
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600046B RID: 1131
		// (set) Token: 0x0600046C RID: 1132
		public extern float frequency
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x0600046E RID: 1134
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_anchor_Injected(out Vector2 ret);

		// Token: 0x0600046F RID: 1135
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_anchor_Injected(ref Vector2 value);

		// Token: 0x06000470 RID: 1136
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_target_Injected(out Vector2 ret);

		// Token: 0x06000471 RID: 1137
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_target_Injected(ref Vector2 value);
	}
}
