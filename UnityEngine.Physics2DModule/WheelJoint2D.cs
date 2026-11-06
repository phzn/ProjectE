using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000036 RID: 54
	[NativeHeader("Modules/Physics2D/WheelJoint2D.h")]
	public sealed class WheelJoint2D : AnchoredJoint2D
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x000088C0 File Offset: 0x00006AC0
		// (set) Token: 0x06000479 RID: 1145 RVA: 0x000088D6 File Offset: 0x00006AD6
		public JointSuspension2D suspension
		{
			get
			{
				JointSuspension2D jointSuspension2D;
				this.get_suspension_Injected(out jointSuspension2D);
				return jointSuspension2D;
			}
			set
			{
				this.set_suspension_Injected(ref value);
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600047A RID: 1146
		// (set) Token: 0x0600047B RID: 1147
		public extern bool useMotor
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x000088E0 File Offset: 0x00006AE0
		// (set) Token: 0x0600047D RID: 1149 RVA: 0x000088F6 File Offset: 0x00006AF6
		public JointMotor2D motor
		{
			get
			{
				JointMotor2D jointMotor2D;
				this.get_motor_Injected(out jointMotor2D);
				return jointMotor2D;
			}
			set
			{
				this.set_motor_Injected(ref value);
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600047E RID: 1150
		public extern float jointTranslation
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600047F RID: 1151
		public extern float jointLinearSpeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000480 RID: 1152
		public extern float jointSpeed
		{
			[NativeMethod("GetJointAngularSpeed")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000481 RID: 1153
		public extern float jointAngle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000482 RID: 1154
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetMotorTorque(float timeStep);

		// Token: 0x06000484 RID: 1156
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_suspension_Injected(out JointSuspension2D ret);

		// Token: 0x06000485 RID: 1157
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_suspension_Injected(ref JointSuspension2D value);

		// Token: 0x06000486 RID: 1158
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_motor_Injected(out JointMotor2D ret);

		// Token: 0x06000487 RID: 1159
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_motor_Injected(ref JointMotor2D value);
	}
}
