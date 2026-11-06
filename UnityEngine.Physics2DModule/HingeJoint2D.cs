using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000031 RID: 49
	[NativeHeader("Modules/Physics2D/HingeJoint2D.h")]
	public sealed class HingeJoint2D : AnchoredJoint2D
	{
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000428 RID: 1064
		// (set) Token: 0x06000429 RID: 1065
		public extern bool useMotor
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600042A RID: 1066
		// (set) Token: 0x0600042B RID: 1067
		public extern bool useLimits
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x000087C8 File Offset: 0x000069C8
		// (set) Token: 0x0600042D RID: 1069 RVA: 0x000087DE File Offset: 0x000069DE
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

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x000087E8 File Offset: 0x000069E8
		// (set) Token: 0x0600042F RID: 1071 RVA: 0x000087FE File Offset: 0x000069FE
		public JointAngleLimits2D limits
		{
			get
			{
				JointAngleLimits2D jointAngleLimits2D;
				this.get_limits_Injected(out jointAngleLimits2D);
				return jointAngleLimits2D;
			}
			set
			{
				this.set_limits_Injected(ref value);
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000430 RID: 1072
		public extern JointLimitState2D limitState
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000431 RID: 1073
		public extern float referenceAngle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000432 RID: 1074
		public extern float jointAngle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000433 RID: 1075
		public extern float jointSpeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000434 RID: 1076
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetMotorTorque(float timeStep);

		// Token: 0x06000436 RID: 1078
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_motor_Injected(out JointMotor2D ret);

		// Token: 0x06000437 RID: 1079
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_motor_Injected(ref JointMotor2D value);

		// Token: 0x06000438 RID: 1080
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_limits_Injected(out JointAngleLimits2D ret);

		// Token: 0x06000439 RID: 1081
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_limits_Injected(ref JointAngleLimits2D value);
	}
}
