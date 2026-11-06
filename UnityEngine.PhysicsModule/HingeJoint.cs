using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000031 RID: 49
	[NativeClass("Unity::HingeJoint")]
	[NativeHeader("Modules/Physics/HingeJoint.h")]
	public class HingeJoint : Joint
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x00005CB8 File Offset: 0x00003EB8
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x00005CCE File Offset: 0x00003ECE
		public JointMotor motor
		{
			get
			{
				JointMotor jointMotor;
				this.get_motor_Injected(out jointMotor);
				return jointMotor;
			}
			set
			{
				this.set_motor_Injected(ref value);
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x00005CD8 File Offset: 0x00003ED8
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x00005CEE File Offset: 0x00003EEE
		public JointLimits limits
		{
			get
			{
				JointLimits jointLimits;
				this.get_limits_Injected(out jointLimits);
				return jointLimits;
			}
			set
			{
				this.set_limits_Injected(ref value);
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x00005CF8 File Offset: 0x00003EF8
		// (set) Token: 0x060003BA RID: 954 RVA: 0x00005D0E File Offset: 0x00003F0E
		public JointSpring spring
		{
			get
			{
				JointSpring jointSpring;
				this.get_spring_Injected(out jointSpring);
				return jointSpring;
			}
			set
			{
				this.set_spring_Injected(ref value);
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060003BB RID: 955
		// (set) Token: 0x060003BC RID: 956
		public extern bool useMotor
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060003BD RID: 957
		// (set) Token: 0x060003BE RID: 958
		public extern bool useLimits
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060003BF RID: 959
		// (set) Token: 0x060003C0 RID: 960
		public extern bool extendedLimits
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060003C1 RID: 961
		// (set) Token: 0x060003C2 RID: 962
		public extern bool useSpring
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060003C3 RID: 963
		public extern float velocity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060003C4 RID: 964
		public extern float angle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060003C5 RID: 965
		// (set) Token: 0x060003C6 RID: 966
		public extern bool useAcceleration
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060003C8 RID: 968
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_motor_Injected(out JointMotor ret);

		// Token: 0x060003C9 RID: 969
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_motor_Injected(ref JointMotor value);

		// Token: 0x060003CA RID: 970
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_limits_Injected(out JointLimits ret);

		// Token: 0x060003CB RID: 971
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_limits_Injected(ref JointLimits value);

		// Token: 0x060003CC RID: 972
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_spring_Injected(out JointSpring ret);

		// Token: 0x060003CD RID: 973
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_spring_Injected(ref JointSpring value);
	}
}
