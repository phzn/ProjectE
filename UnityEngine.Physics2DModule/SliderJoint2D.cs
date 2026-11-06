using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000033 RID: 51
	[NativeHeader("Modules/Physics2D/SliderJoint2D.h")]
	public sealed class SliderJoint2D : AnchoredJoint2D
	{
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600044B RID: 1099
		// (set) Token: 0x0600044C RID: 1100
		public extern bool autoConfigureAngle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600044D RID: 1101
		// (set) Token: 0x0600044E RID: 1102
		public extern float angle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600044F RID: 1103
		// (set) Token: 0x06000450 RID: 1104
		public extern bool useMotor
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000451 RID: 1105
		// (set) Token: 0x06000452 RID: 1106
		public extern bool useLimits
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x00008840 File Offset: 0x00006A40
		// (set) Token: 0x06000454 RID: 1108 RVA: 0x00008856 File Offset: 0x00006A56
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

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x00008860 File Offset: 0x00006A60
		// (set) Token: 0x06000456 RID: 1110 RVA: 0x00008876 File Offset: 0x00006A76
		public JointTranslationLimits2D limits
		{
			get
			{
				JointTranslationLimits2D jointTranslationLimits2D;
				this.get_limits_Injected(out jointTranslationLimits2D);
				return jointTranslationLimits2D;
			}
			set
			{
				this.set_limits_Injected(ref value);
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000457 RID: 1111
		public extern JointLimitState2D limitState
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000458 RID: 1112
		public extern float referenceAngle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000459 RID: 1113
		public extern float jointTranslation
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600045A RID: 1114
		public extern float jointSpeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x0600045B RID: 1115
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetMotorForce(float timeStep);

		// Token: 0x0600045D RID: 1117
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_motor_Injected(out JointMotor2D ret);

		// Token: 0x0600045E RID: 1118
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_motor_Injected(ref JointMotor2D value);

		// Token: 0x0600045F RID: 1119
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_limits_Injected(out JointTranslationLimits2D ret);

		// Token: 0x06000460 RID: 1120
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_limits_Injected(ref JointTranslationLimits2D value);
	}
}
