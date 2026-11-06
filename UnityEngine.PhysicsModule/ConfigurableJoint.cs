using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000035 RID: 53
	[NativeHeader("Modules/Physics/ConfigurableJoint.h")]
	[NativeClass("Unity::ConfigurableJoint")]
	public class ConfigurableJoint : Joint
	{
		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x00005E04 File Offset: 0x00004004
		// (set) Token: 0x060003FE RID: 1022 RVA: 0x00005E1A File Offset: 0x0000401A
		public Vector3 secondaryAxis
		{
			get
			{
				Vector3 vector;
				this.get_secondaryAxis_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_secondaryAxis_Injected(ref value);
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060003FF RID: 1023
		// (set) Token: 0x06000400 RID: 1024
		public extern ConfigurableJointMotion xMotion
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000401 RID: 1025
		// (set) Token: 0x06000402 RID: 1026
		public extern ConfigurableJointMotion yMotion
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000403 RID: 1027
		// (set) Token: 0x06000404 RID: 1028
		public extern ConfigurableJointMotion zMotion
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000405 RID: 1029
		// (set) Token: 0x06000406 RID: 1030
		public extern ConfigurableJointMotion angularXMotion
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000407 RID: 1031
		// (set) Token: 0x06000408 RID: 1032
		public extern ConfigurableJointMotion angularYMotion
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000409 RID: 1033
		// (set) Token: 0x0600040A RID: 1034
		public extern ConfigurableJointMotion angularZMotion
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x00005E24 File Offset: 0x00004024
		// (set) Token: 0x0600040C RID: 1036 RVA: 0x00005E3A File Offset: 0x0000403A
		public SoftJointLimitSpring linearLimitSpring
		{
			get
			{
				SoftJointLimitSpring softJointLimitSpring;
				this.get_linearLimitSpring_Injected(out softJointLimitSpring);
				return softJointLimitSpring;
			}
			set
			{
				this.set_linearLimitSpring_Injected(ref value);
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x00005E44 File Offset: 0x00004044
		// (set) Token: 0x0600040E RID: 1038 RVA: 0x00005E5A File Offset: 0x0000405A
		public SoftJointLimitSpring angularXLimitSpring
		{
			get
			{
				SoftJointLimitSpring softJointLimitSpring;
				this.get_angularXLimitSpring_Injected(out softJointLimitSpring);
				return softJointLimitSpring;
			}
			set
			{
				this.set_angularXLimitSpring_Injected(ref value);
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x00005E64 File Offset: 0x00004064
		// (set) Token: 0x06000410 RID: 1040 RVA: 0x00005E7A File Offset: 0x0000407A
		public SoftJointLimitSpring angularYZLimitSpring
		{
			get
			{
				SoftJointLimitSpring softJointLimitSpring;
				this.get_angularYZLimitSpring_Injected(out softJointLimitSpring);
				return softJointLimitSpring;
			}
			set
			{
				this.set_angularYZLimitSpring_Injected(ref value);
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x00005E84 File Offset: 0x00004084
		// (set) Token: 0x06000412 RID: 1042 RVA: 0x00005E9A File Offset: 0x0000409A
		public SoftJointLimit linearLimit
		{
			get
			{
				SoftJointLimit softJointLimit;
				this.get_linearLimit_Injected(out softJointLimit);
				return softJointLimit;
			}
			set
			{
				this.set_linearLimit_Injected(ref value);
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x00005EA4 File Offset: 0x000040A4
		// (set) Token: 0x06000414 RID: 1044 RVA: 0x00005EBA File Offset: 0x000040BA
		public SoftJointLimit lowAngularXLimit
		{
			get
			{
				SoftJointLimit softJointLimit;
				this.get_lowAngularXLimit_Injected(out softJointLimit);
				return softJointLimit;
			}
			set
			{
				this.set_lowAngularXLimit_Injected(ref value);
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x00005EC4 File Offset: 0x000040C4
		// (set) Token: 0x06000416 RID: 1046 RVA: 0x00005EDA File Offset: 0x000040DA
		public SoftJointLimit highAngularXLimit
		{
			get
			{
				SoftJointLimit softJointLimit;
				this.get_highAngularXLimit_Injected(out softJointLimit);
				return softJointLimit;
			}
			set
			{
				this.set_highAngularXLimit_Injected(ref value);
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x00005EE4 File Offset: 0x000040E4
		// (set) Token: 0x06000418 RID: 1048 RVA: 0x00005EFA File Offset: 0x000040FA
		public SoftJointLimit angularYLimit
		{
			get
			{
				SoftJointLimit softJointLimit;
				this.get_angularYLimit_Injected(out softJointLimit);
				return softJointLimit;
			}
			set
			{
				this.set_angularYLimit_Injected(ref value);
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x00005F04 File Offset: 0x00004104
		// (set) Token: 0x0600041A RID: 1050 RVA: 0x00005F1A File Offset: 0x0000411A
		public SoftJointLimit angularZLimit
		{
			get
			{
				SoftJointLimit softJointLimit;
				this.get_angularZLimit_Injected(out softJointLimit);
				return softJointLimit;
			}
			set
			{
				this.set_angularZLimit_Injected(ref value);
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x00005F24 File Offset: 0x00004124
		// (set) Token: 0x0600041C RID: 1052 RVA: 0x00005F3A File Offset: 0x0000413A
		public Vector3 targetPosition
		{
			get
			{
				Vector3 vector;
				this.get_targetPosition_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_targetPosition_Injected(ref value);
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x00005F44 File Offset: 0x00004144
		// (set) Token: 0x0600041E RID: 1054 RVA: 0x00005F5A File Offset: 0x0000415A
		public Vector3 targetVelocity
		{
			get
			{
				Vector3 vector;
				this.get_targetVelocity_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_targetVelocity_Injected(ref value);
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x00005F64 File Offset: 0x00004164
		// (set) Token: 0x06000420 RID: 1056 RVA: 0x00005F7A File Offset: 0x0000417A
		public JointDrive xDrive
		{
			get
			{
				JointDrive jointDrive;
				this.get_xDrive_Injected(out jointDrive);
				return jointDrive;
			}
			set
			{
				this.set_xDrive_Injected(ref value);
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x00005F84 File Offset: 0x00004184
		// (set) Token: 0x06000422 RID: 1058 RVA: 0x00005F9A File Offset: 0x0000419A
		public JointDrive yDrive
		{
			get
			{
				JointDrive jointDrive;
				this.get_yDrive_Injected(out jointDrive);
				return jointDrive;
			}
			set
			{
				this.set_yDrive_Injected(ref value);
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x00005FA4 File Offset: 0x000041A4
		// (set) Token: 0x06000424 RID: 1060 RVA: 0x00005FBA File Offset: 0x000041BA
		public JointDrive zDrive
		{
			get
			{
				JointDrive jointDrive;
				this.get_zDrive_Injected(out jointDrive);
				return jointDrive;
			}
			set
			{
				this.set_zDrive_Injected(ref value);
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x00005FC4 File Offset: 0x000041C4
		// (set) Token: 0x06000426 RID: 1062 RVA: 0x00005FDA File Offset: 0x000041DA
		public Quaternion targetRotation
		{
			get
			{
				Quaternion quaternion;
				this.get_targetRotation_Injected(out quaternion);
				return quaternion;
			}
			set
			{
				this.set_targetRotation_Injected(ref value);
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x00005FE4 File Offset: 0x000041E4
		// (set) Token: 0x06000428 RID: 1064 RVA: 0x00005FFA File Offset: 0x000041FA
		public Vector3 targetAngularVelocity
		{
			get
			{
				Vector3 vector;
				this.get_targetAngularVelocity_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_targetAngularVelocity_Injected(ref value);
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000429 RID: 1065
		// (set) Token: 0x0600042A RID: 1066
		public extern RotationDriveMode rotationDriveMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x00006004 File Offset: 0x00004204
		// (set) Token: 0x0600042C RID: 1068 RVA: 0x0000601A File Offset: 0x0000421A
		public JointDrive angularXDrive
		{
			get
			{
				JointDrive jointDrive;
				this.get_angularXDrive_Injected(out jointDrive);
				return jointDrive;
			}
			set
			{
				this.set_angularXDrive_Injected(ref value);
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x00006024 File Offset: 0x00004224
		// (set) Token: 0x0600042E RID: 1070 RVA: 0x0000603A File Offset: 0x0000423A
		public JointDrive angularYZDrive
		{
			get
			{
				JointDrive jointDrive;
				this.get_angularYZDrive_Injected(out jointDrive);
				return jointDrive;
			}
			set
			{
				this.set_angularYZDrive_Injected(ref value);
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x00006044 File Offset: 0x00004244
		// (set) Token: 0x06000430 RID: 1072 RVA: 0x0000605A File Offset: 0x0000425A
		public JointDrive slerpDrive
		{
			get
			{
				JointDrive jointDrive;
				this.get_slerpDrive_Injected(out jointDrive);
				return jointDrive;
			}
			set
			{
				this.set_slerpDrive_Injected(ref value);
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000431 RID: 1073
		// (set) Token: 0x06000432 RID: 1074
		public extern JointProjectionMode projectionMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000433 RID: 1075
		// (set) Token: 0x06000434 RID: 1076
		public extern float projectionDistance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000435 RID: 1077
		// (set) Token: 0x06000436 RID: 1078
		public extern float projectionAngle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000437 RID: 1079
		// (set) Token: 0x06000438 RID: 1080
		public extern bool configuredInWorldSpace
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000439 RID: 1081
		// (set) Token: 0x0600043A RID: 1082
		public extern bool swapBodies
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x0600043C RID: 1084
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_secondaryAxis_Injected(out Vector3 ret);

		// Token: 0x0600043D RID: 1085
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_secondaryAxis_Injected(ref Vector3 value);

		// Token: 0x0600043E RID: 1086
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_linearLimitSpring_Injected(out SoftJointLimitSpring ret);

		// Token: 0x0600043F RID: 1087
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_linearLimitSpring_Injected(ref SoftJointLimitSpring value);

		// Token: 0x06000440 RID: 1088
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_angularXLimitSpring_Injected(out SoftJointLimitSpring ret);

		// Token: 0x06000441 RID: 1089
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_angularXLimitSpring_Injected(ref SoftJointLimitSpring value);

		// Token: 0x06000442 RID: 1090
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_angularYZLimitSpring_Injected(out SoftJointLimitSpring ret);

		// Token: 0x06000443 RID: 1091
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_angularYZLimitSpring_Injected(ref SoftJointLimitSpring value);

		// Token: 0x06000444 RID: 1092
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_linearLimit_Injected(out SoftJointLimit ret);

		// Token: 0x06000445 RID: 1093
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_linearLimit_Injected(ref SoftJointLimit value);

		// Token: 0x06000446 RID: 1094
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_lowAngularXLimit_Injected(out SoftJointLimit ret);

		// Token: 0x06000447 RID: 1095
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_lowAngularXLimit_Injected(ref SoftJointLimit value);

		// Token: 0x06000448 RID: 1096
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_highAngularXLimit_Injected(out SoftJointLimit ret);

		// Token: 0x06000449 RID: 1097
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_highAngularXLimit_Injected(ref SoftJointLimit value);

		// Token: 0x0600044A RID: 1098
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_angularYLimit_Injected(out SoftJointLimit ret);

		// Token: 0x0600044B RID: 1099
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_angularYLimit_Injected(ref SoftJointLimit value);

		// Token: 0x0600044C RID: 1100
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_angularZLimit_Injected(out SoftJointLimit ret);

		// Token: 0x0600044D RID: 1101
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_angularZLimit_Injected(ref SoftJointLimit value);

		// Token: 0x0600044E RID: 1102
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_targetPosition_Injected(out Vector3 ret);

		// Token: 0x0600044F RID: 1103
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_targetPosition_Injected(ref Vector3 value);

		// Token: 0x06000450 RID: 1104
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_targetVelocity_Injected(out Vector3 ret);

		// Token: 0x06000451 RID: 1105
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_targetVelocity_Injected(ref Vector3 value);

		// Token: 0x06000452 RID: 1106
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_xDrive_Injected(out JointDrive ret);

		// Token: 0x06000453 RID: 1107
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_xDrive_Injected(ref JointDrive value);

		// Token: 0x06000454 RID: 1108
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_yDrive_Injected(out JointDrive ret);

		// Token: 0x06000455 RID: 1109
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_yDrive_Injected(ref JointDrive value);

		// Token: 0x06000456 RID: 1110
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_zDrive_Injected(out JointDrive ret);

		// Token: 0x06000457 RID: 1111
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_zDrive_Injected(ref JointDrive value);

		// Token: 0x06000458 RID: 1112
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_targetRotation_Injected(out Quaternion ret);

		// Token: 0x06000459 RID: 1113
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_targetRotation_Injected(ref Quaternion value);

		// Token: 0x0600045A RID: 1114
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_targetAngularVelocity_Injected(out Vector3 ret);

		// Token: 0x0600045B RID: 1115
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_targetAngularVelocity_Injected(ref Vector3 value);

		// Token: 0x0600045C RID: 1116
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_angularXDrive_Injected(out JointDrive ret);

		// Token: 0x0600045D RID: 1117
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_angularXDrive_Injected(ref JointDrive value);

		// Token: 0x0600045E RID: 1118
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_angularYZDrive_Injected(out JointDrive ret);

		// Token: 0x0600045F RID: 1119
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_angularYZDrive_Injected(ref JointDrive value);

		// Token: 0x06000460 RID: 1120
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_slerpDrive_Injected(out JointDrive ret);

		// Token: 0x06000461 RID: 1121
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_slerpDrive_Injected(ref JointDrive value);
	}
}
