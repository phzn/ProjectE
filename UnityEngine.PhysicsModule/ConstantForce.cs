using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200002F RID: 47
	[RequireComponent(typeof(Rigidbody))]
	[NativeHeader("Modules/Physics/ConstantForce.h")]
	public class ConstantForce : Behaviour
	{
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000382 RID: 898 RVA: 0x00005B80 File Offset: 0x00003D80
		// (set) Token: 0x06000383 RID: 899 RVA: 0x00005B96 File Offset: 0x00003D96
		public Vector3 force
		{
			get
			{
				Vector3 vector;
				this.get_force_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_force_Injected(ref value);
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000384 RID: 900 RVA: 0x00005BA0 File Offset: 0x00003DA0
		// (set) Token: 0x06000385 RID: 901 RVA: 0x00005BB6 File Offset: 0x00003DB6
		public Vector3 relativeForce
		{
			get
			{
				Vector3 vector;
				this.get_relativeForce_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_relativeForce_Injected(ref value);
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000386 RID: 902 RVA: 0x00005BC0 File Offset: 0x00003DC0
		// (set) Token: 0x06000387 RID: 903 RVA: 0x00005BD6 File Offset: 0x00003DD6
		public Vector3 torque
		{
			get
			{
				Vector3 vector;
				this.get_torque_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_torque_Injected(ref value);
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000388 RID: 904 RVA: 0x00005BE0 File Offset: 0x00003DE0
		// (set) Token: 0x06000389 RID: 905 RVA: 0x00005BF6 File Offset: 0x00003DF6
		public Vector3 relativeTorque
		{
			get
			{
				Vector3 vector;
				this.get_relativeTorque_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_relativeTorque_Injected(ref value);
			}
		}

		// Token: 0x0600038B RID: 907
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_force_Injected(out Vector3 ret);

		// Token: 0x0600038C RID: 908
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_force_Injected(ref Vector3 value);

		// Token: 0x0600038D RID: 909
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_relativeForce_Injected(out Vector3 ret);

		// Token: 0x0600038E RID: 910
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_relativeForce_Injected(ref Vector3 value);

		// Token: 0x0600038F RID: 911
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_torque_Injected(out Vector3 ret);

		// Token: 0x06000390 RID: 912
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_torque_Injected(ref Vector3 value);

		// Token: 0x06000391 RID: 913
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_relativeTorque_Injected(out Vector3 ret);

		// Token: 0x06000392 RID: 914
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_relativeTorque_Injected(ref Vector3 value);
	}
}
