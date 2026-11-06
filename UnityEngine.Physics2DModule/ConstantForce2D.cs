using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200003E RID: 62
	[RequireComponent(typeof(Rigidbody2D))]
	[NativeHeader("Modules/Physics2D/ConstantForce2D.h")]
	public sealed class ConstantForce2D : PhysicsUpdateBehaviour2D
	{
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x000089BC File Offset: 0x00006BBC
		// (set) Token: 0x060004E7 RID: 1255 RVA: 0x000089D2 File Offset: 0x00006BD2
		public Vector2 force
		{
			get
			{
				Vector2 vector;
				this.get_force_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_force_Injected(ref value);
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x000089DC File Offset: 0x00006BDC
		// (set) Token: 0x060004E9 RID: 1257 RVA: 0x000089F2 File Offset: 0x00006BF2
		public Vector2 relativeForce
		{
			get
			{
				Vector2 vector;
				this.get_relativeForce_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_relativeForce_Injected(ref value);
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060004EA RID: 1258
		// (set) Token: 0x060004EB RID: 1259
		public extern float torque
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060004ED RID: 1261
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_force_Injected(out Vector2 ret);

		// Token: 0x060004EE RID: 1262
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_force_Injected(ref Vector2 value);

		// Token: 0x060004EF RID: 1263
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_relativeForce_Injected(out Vector2 ret);

		// Token: 0x060004F0 RID: 1264
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_relativeForce_Injected(ref Vector2 value);
	}
}
