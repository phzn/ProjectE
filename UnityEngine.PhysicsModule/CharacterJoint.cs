using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000034 RID: 52
	[NativeHeader("Modules/Physics/CharacterJoint.h")]
	[NativeClass("Unity::CharacterJoint")]
	public class CharacterJoint : Joint
	{
		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060003DA RID: 986 RVA: 0x00005D24 File Offset: 0x00003F24
		// (set) Token: 0x060003DB RID: 987 RVA: 0x00005D3A File Offset: 0x00003F3A
		public Vector3 swingAxis
		{
			get
			{
				Vector3 vector;
				this.get_swingAxis_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_swingAxis_Injected(ref value);
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060003DC RID: 988 RVA: 0x00005D44 File Offset: 0x00003F44
		// (set) Token: 0x060003DD RID: 989 RVA: 0x00005D5A File Offset: 0x00003F5A
		public SoftJointLimitSpring twistLimitSpring
		{
			get
			{
				SoftJointLimitSpring softJointLimitSpring;
				this.get_twistLimitSpring_Injected(out softJointLimitSpring);
				return softJointLimitSpring;
			}
			set
			{
				this.set_twistLimitSpring_Injected(ref value);
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060003DE RID: 990 RVA: 0x00005D64 File Offset: 0x00003F64
		// (set) Token: 0x060003DF RID: 991 RVA: 0x00005D7A File Offset: 0x00003F7A
		public SoftJointLimitSpring swingLimitSpring
		{
			get
			{
				SoftJointLimitSpring softJointLimitSpring;
				this.get_swingLimitSpring_Injected(out softJointLimitSpring);
				return softJointLimitSpring;
			}
			set
			{
				this.set_swingLimitSpring_Injected(ref value);
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x00005D84 File Offset: 0x00003F84
		// (set) Token: 0x060003E1 RID: 993 RVA: 0x00005D9A File Offset: 0x00003F9A
		public SoftJointLimit lowTwistLimit
		{
			get
			{
				SoftJointLimit softJointLimit;
				this.get_lowTwistLimit_Injected(out softJointLimit);
				return softJointLimit;
			}
			set
			{
				this.set_lowTwistLimit_Injected(ref value);
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060003E2 RID: 994 RVA: 0x00005DA4 File Offset: 0x00003FA4
		// (set) Token: 0x060003E3 RID: 995 RVA: 0x00005DBA File Offset: 0x00003FBA
		public SoftJointLimit highTwistLimit
		{
			get
			{
				SoftJointLimit softJointLimit;
				this.get_highTwistLimit_Injected(out softJointLimit);
				return softJointLimit;
			}
			set
			{
				this.set_highTwistLimit_Injected(ref value);
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x00005DC4 File Offset: 0x00003FC4
		// (set) Token: 0x060003E5 RID: 997 RVA: 0x00005DDA File Offset: 0x00003FDA
		public SoftJointLimit swing1Limit
		{
			get
			{
				SoftJointLimit softJointLimit;
				this.get_swing1Limit_Injected(out softJointLimit);
				return softJointLimit;
			}
			set
			{
				this.set_swing1Limit_Injected(ref value);
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x00005DE4 File Offset: 0x00003FE4
		// (set) Token: 0x060003E7 RID: 999 RVA: 0x00005DFA File Offset: 0x00003FFA
		public SoftJointLimit swing2Limit
		{
			get
			{
				SoftJointLimit softJointLimit;
				this.get_swing2Limit_Injected(out softJointLimit);
				return softJointLimit;
			}
			set
			{
				this.set_swing2Limit_Injected(ref value);
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060003E8 RID: 1000
		// (set) Token: 0x060003E9 RID: 1001
		public extern bool enableProjection
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060003EA RID: 1002
		// (set) Token: 0x060003EB RID: 1003
		public extern float projectionDistance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060003EC RID: 1004
		// (set) Token: 0x060003ED RID: 1005
		public extern float projectionAngle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060003EF RID: 1007
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_swingAxis_Injected(out Vector3 ret);

		// Token: 0x060003F0 RID: 1008
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_swingAxis_Injected(ref Vector3 value);

		// Token: 0x060003F1 RID: 1009
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_twistLimitSpring_Injected(out SoftJointLimitSpring ret);

		// Token: 0x060003F2 RID: 1010
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_twistLimitSpring_Injected(ref SoftJointLimitSpring value);

		// Token: 0x060003F3 RID: 1011
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_swingLimitSpring_Injected(out SoftJointLimitSpring ret);

		// Token: 0x060003F4 RID: 1012
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_swingLimitSpring_Injected(ref SoftJointLimitSpring value);

		// Token: 0x060003F5 RID: 1013
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_lowTwistLimit_Injected(out SoftJointLimit ret);

		// Token: 0x060003F6 RID: 1014
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_lowTwistLimit_Injected(ref SoftJointLimit value);

		// Token: 0x060003F7 RID: 1015
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_highTwistLimit_Injected(out SoftJointLimit ret);

		// Token: 0x060003F8 RID: 1016
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_highTwistLimit_Injected(ref SoftJointLimit value);

		// Token: 0x060003F9 RID: 1017
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_swing1Limit_Injected(out SoftJointLimit ret);

		// Token: 0x060003FA RID: 1018
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_swing1Limit_Injected(ref SoftJointLimit value);

		// Token: 0x060003FB RID: 1019
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_swing2Limit_Injected(out SoftJointLimit ret);

		// Token: 0x060003FC RID: 1020
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_swing2Limit_Injected(ref SoftJointLimit value);

		// Token: 0x040000BE RID: 190
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("TargetRotation not in use for Unity 5 and assumed disabled.", true)]
		public Quaternion targetRotation;

		// Token: 0x040000BF RID: 191
		[Obsolete("TargetAngularVelocity not in use for Unity 5 and assumed disabled.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public Vector3 targetAngularVelocity;

		// Token: 0x040000C0 RID: 192
		[Obsolete("RotationDrive not in use for Unity 5 and assumed disabled.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public JointDrive rotationDrive;
	}
}
