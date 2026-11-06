using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000032 RID: 50
	[NativeHeader("Modules/Physics2D/RelativeJoint2D.h")]
	public sealed class RelativeJoint2D : Joint2D
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600043A RID: 1082
		// (set) Token: 0x0600043B RID: 1083
		public extern float maxForce
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600043C RID: 1084
		// (set) Token: 0x0600043D RID: 1085
		public extern float maxTorque
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600043E RID: 1086
		// (set) Token: 0x0600043F RID: 1087
		public extern float correctionScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000440 RID: 1088
		// (set) Token: 0x06000441 RID: 1089
		public extern bool autoConfigureOffset
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x00008808 File Offset: 0x00006A08
		// (set) Token: 0x06000443 RID: 1091 RVA: 0x0000881E File Offset: 0x00006A1E
		public Vector2 linearOffset
		{
			get
			{
				Vector2 vector;
				this.get_linearOffset_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_linearOffset_Injected(ref value);
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000444 RID: 1092
		// (set) Token: 0x06000445 RID: 1093
		public extern float angularOffset
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x00008828 File Offset: 0x00006A28
		public Vector2 target
		{
			get
			{
				Vector2 vector;
				this.get_target_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x06000448 RID: 1096
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_linearOffset_Injected(out Vector2 ret);

		// Token: 0x06000449 RID: 1097
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_linearOffset_Injected(ref Vector2 value);

		// Token: 0x0600044A RID: 1098
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_target_Injected(out Vector2 ret);
	}
}
