using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;

namespace UnityEngine
{
	// Token: 0x0200001E RID: 30
	[NativeClass("Unity::ArticulationBody")]
	[NativeHeader("Modules/Physics/ArticulationBody.h")]
	public class ArticulationBody : Behaviour
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000065 RID: 101
		// (set) Token: 0x06000066 RID: 102
		public extern ArticulationJointType jointType
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00002C3C File Offset: 0x00000E3C
		// (set) Token: 0x06000068 RID: 104 RVA: 0x00002C52 File Offset: 0x00000E52
		public Vector3 anchorPosition
		{
			get
			{
				Vector3 vector;
				this.get_anchorPosition_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_anchorPosition_Injected(ref value);
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00002C5C File Offset: 0x00000E5C
		// (set) Token: 0x0600006A RID: 106 RVA: 0x00002C72 File Offset: 0x00000E72
		public Vector3 parentAnchorPosition
		{
			get
			{
				Vector3 vector;
				this.get_parentAnchorPosition_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_parentAnchorPosition_Injected(ref value);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00002C7C File Offset: 0x00000E7C
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00002C92 File Offset: 0x00000E92
		public Quaternion anchorRotation
		{
			get
			{
				Quaternion quaternion;
				this.get_anchorRotation_Injected(out quaternion);
				return quaternion;
			}
			set
			{
				this.set_anchorRotation_Injected(ref value);
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00002C9C File Offset: 0x00000E9C
		// (set) Token: 0x0600006E RID: 110 RVA: 0x00002CB2 File Offset: 0x00000EB2
		public Quaternion parentAnchorRotation
		{
			get
			{
				Quaternion quaternion;
				this.get_parentAnchorRotation_Injected(out quaternion);
				return quaternion;
			}
			set
			{
				this.set_parentAnchorRotation_Injected(ref value);
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600006F RID: 111
		public extern bool isRoot
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000070 RID: 112
		// (set) Token: 0x06000071 RID: 113
		public extern bool matchAnchors
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000072 RID: 114
		// (set) Token: 0x06000073 RID: 115
		public extern ArticulationDofLock linearLockX
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000074 RID: 116
		// (set) Token: 0x06000075 RID: 117
		public extern ArticulationDofLock linearLockY
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000076 RID: 118
		// (set) Token: 0x06000077 RID: 119
		public extern ArticulationDofLock linearLockZ
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000078 RID: 120
		// (set) Token: 0x06000079 RID: 121
		public extern ArticulationDofLock swingYLock
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600007A RID: 122
		// (set) Token: 0x0600007B RID: 123
		public extern ArticulationDofLock swingZLock
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600007C RID: 124
		// (set) Token: 0x0600007D RID: 125
		public extern ArticulationDofLock twistLock
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00002CBC File Offset: 0x00000EBC
		// (set) Token: 0x0600007F RID: 127 RVA: 0x00002CD2 File Offset: 0x00000ED2
		public ArticulationDrive xDrive
		{
			get
			{
				ArticulationDrive articulationDrive;
				this.get_xDrive_Injected(out articulationDrive);
				return articulationDrive;
			}
			set
			{
				this.set_xDrive_Injected(ref value);
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00002CDC File Offset: 0x00000EDC
		// (set) Token: 0x06000081 RID: 129 RVA: 0x00002CF2 File Offset: 0x00000EF2
		public ArticulationDrive yDrive
		{
			get
			{
				ArticulationDrive articulationDrive;
				this.get_yDrive_Injected(out articulationDrive);
				return articulationDrive;
			}
			set
			{
				this.set_yDrive_Injected(ref value);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00002CFC File Offset: 0x00000EFC
		// (set) Token: 0x06000083 RID: 131 RVA: 0x00002D12 File Offset: 0x00000F12
		public ArticulationDrive zDrive
		{
			get
			{
				ArticulationDrive articulationDrive;
				this.get_zDrive_Injected(out articulationDrive);
				return articulationDrive;
			}
			set
			{
				this.set_zDrive_Injected(ref value);
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000084 RID: 132
		// (set) Token: 0x06000085 RID: 133
		public extern bool immovable
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000086 RID: 134
		// (set) Token: 0x06000087 RID: 135
		public extern bool useGravity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000088 RID: 136
		// (set) Token: 0x06000089 RID: 137
		public extern float linearDamping
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600008A RID: 138
		// (set) Token: 0x0600008B RID: 139
		public extern float angularDamping
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600008C RID: 140
		// (set) Token: 0x0600008D RID: 141
		public extern float jointFriction
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00002D1C File Offset: 0x00000F1C
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00002D32 File Offset: 0x00000F32
		public LayerMask excludeLayers
		{
			get
			{
				LayerMask layerMask;
				this.get_excludeLayers_Injected(out layerMask);
				return layerMask;
			}
			set
			{
				this.set_excludeLayers_Injected(ref value);
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00002D3C File Offset: 0x00000F3C
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00002D52 File Offset: 0x00000F52
		public LayerMask includeLayers
		{
			get
			{
				LayerMask layerMask;
				this.get_includeLayers_Injected(out layerMask);
				return layerMask;
			}
			set
			{
				this.set_includeLayers_Injected(ref value);
			}
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00002D5C File Offset: 0x00000F5C
		public Vector3 GetAccumulatedForce([DefaultValue("Time.fixedDeltaTime")] float step)
		{
			Vector3 vector;
			this.GetAccumulatedForce_Injected(step, out vector);
			return vector;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00002D74 File Offset: 0x00000F74
		[ExcludeFromDocs]
		public Vector3 GetAccumulatedForce()
		{
			return this.GetAccumulatedForce(Time.fixedDeltaTime);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00002D94 File Offset: 0x00000F94
		public Vector3 GetAccumulatedTorque([DefaultValue("Time.fixedDeltaTime")] float step)
		{
			Vector3 vector;
			this.GetAccumulatedTorque_Injected(step, out vector);
			return vector;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00002DAC File Offset: 0x00000FAC
		[ExcludeFromDocs]
		public Vector3 GetAccumulatedTorque()
		{
			return this.GetAccumulatedTorque(Time.fixedDeltaTime);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00002DC9 File Offset: 0x00000FC9
		public void AddForce(Vector3 force, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			this.AddForce_Injected(ref force, mode);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00002DD4 File Offset: 0x00000FD4
		[ExcludeFromDocs]
		public void AddForce(Vector3 force)
		{
			this.AddForce(force, ForceMode.Force);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00002DE0 File Offset: 0x00000FE0
		public void AddRelativeForce(Vector3 force, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			this.AddRelativeForce_Injected(ref force, mode);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00002DEB File Offset: 0x00000FEB
		[ExcludeFromDocs]
		public void AddRelativeForce(Vector3 force)
		{
			this.AddRelativeForce(force, ForceMode.Force);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00002DF7 File Offset: 0x00000FF7
		public void AddTorque(Vector3 torque, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			this.AddTorque_Injected(ref torque, mode);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00002E02 File Offset: 0x00001002
		[ExcludeFromDocs]
		public void AddTorque(Vector3 torque)
		{
			this.AddTorque(torque, ForceMode.Force);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00002E0E File Offset: 0x0000100E
		public void AddRelativeTorque(Vector3 torque, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			this.AddRelativeTorque_Injected(ref torque, mode);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00002E19 File Offset: 0x00001019
		[ExcludeFromDocs]
		public void AddRelativeTorque(Vector3 torque)
		{
			this.AddRelativeTorque(torque, ForceMode.Force);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00002E25 File Offset: 0x00001025
		public void AddForceAtPosition(Vector3 force, Vector3 position, [DefaultValue("ForceMode.Force")] ForceMode mode)
		{
			this.AddForceAtPosition_Injected(ref force, ref position, mode);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00002E32 File Offset: 0x00001032
		[ExcludeFromDocs]
		public void AddForceAtPosition(Vector3 force, Vector3 position)
		{
			this.AddForceAtPosition(force, position, ForceMode.Force);
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x00002E40 File Offset: 0x00001040
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x00002E56 File Offset: 0x00001056
		public Vector3 velocity
		{
			get
			{
				Vector3 vector;
				this.get_velocity_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_velocity_Injected(ref value);
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00002E60 File Offset: 0x00001060
		// (set) Token: 0x060000A3 RID: 163 RVA: 0x00002E76 File Offset: 0x00001076
		public Vector3 angularVelocity
		{
			get
			{
				Vector3 vector;
				this.get_angularVelocity_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_angularVelocity_Injected(ref value);
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000A4 RID: 164
		// (set) Token: 0x060000A5 RID: 165
		public extern float mass
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000A6 RID: 166
		// (set) Token: 0x060000A7 RID: 167
		public extern bool automaticCenterOfMass
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00002E80 File Offset: 0x00001080
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x00002E96 File Offset: 0x00001096
		public Vector3 centerOfMass
		{
			get
			{
				Vector3 vector;
				this.get_centerOfMass_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_centerOfMass_Injected(ref value);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00002EA0 File Offset: 0x000010A0
		public Vector3 worldCenterOfMass
		{
			get
			{
				Vector3 vector;
				this.get_worldCenterOfMass_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000AB RID: 171
		// (set) Token: 0x060000AC RID: 172
		public extern bool automaticInertiaTensor
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00002EB8 File Offset: 0x000010B8
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00002ECE File Offset: 0x000010CE
		public Vector3 inertiaTensor
		{
			get
			{
				Vector3 vector;
				this.get_inertiaTensor_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_inertiaTensor_Injected(ref value);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00002ED8 File Offset: 0x000010D8
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00002EEE File Offset: 0x000010EE
		public Quaternion inertiaTensorRotation
		{
			get
			{
				Quaternion quaternion;
				this.get_inertiaTensorRotation_Injected(out quaternion);
				return quaternion;
			}
			set
			{
				this.set_inertiaTensorRotation_Injected(ref value);
			}
		}

		// Token: 0x060000B1 RID: 177
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ResetCenterOfMass();

		// Token: 0x060000B2 RID: 178
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ResetInertiaTensor();

		// Token: 0x060000B3 RID: 179
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Sleep();

		// Token: 0x060000B4 RID: 180
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsSleeping();

		// Token: 0x060000B5 RID: 181
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void WakeUp();

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000B6 RID: 182
		// (set) Token: 0x060000B7 RID: 183
		public extern float sleepThreshold
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000B8 RID: 184
		// (set) Token: 0x060000B9 RID: 185
		public extern int solverIterations
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000BA RID: 186
		// (set) Token: 0x060000BB RID: 187
		public extern int solverVelocityIterations
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000BC RID: 188
		// (set) Token: 0x060000BD RID: 189
		public extern float maxAngularVelocity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000BE RID: 190
		// (set) Token: 0x060000BF RID: 191
		public extern float maxLinearVelocity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000C0 RID: 192
		// (set) Token: 0x060000C1 RID: 193
		public extern float maxJointVelocity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000C2 RID: 194
		// (set) Token: 0x060000C3 RID: 195
		public extern float maxDepenetrationVelocity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00002EF8 File Offset: 0x000010F8
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00002F0E File Offset: 0x0000110E
		public ArticulationReducedSpace jointPosition
		{
			get
			{
				ArticulationReducedSpace articulationReducedSpace;
				this.get_jointPosition_Injected(out articulationReducedSpace);
				return articulationReducedSpace;
			}
			set
			{
				this.set_jointPosition_Injected(ref value);
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00002F18 File Offset: 0x00001118
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00002F2E File Offset: 0x0000112E
		public ArticulationReducedSpace jointVelocity
		{
			get
			{
				ArticulationReducedSpace articulationReducedSpace;
				this.get_jointVelocity_Injected(out articulationReducedSpace);
				return articulationReducedSpace;
			}
			set
			{
				this.set_jointVelocity_Injected(ref value);
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00002F38 File Offset: 0x00001138
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00002F4E File Offset: 0x0000114E
		public ArticulationReducedSpace jointAcceleration
		{
			get
			{
				ArticulationReducedSpace articulationReducedSpace;
				this.get_jointAcceleration_Injected(out articulationReducedSpace);
				return articulationReducedSpace;
			}
			[Obsolete("Setting joint accelerations is not supported in forward kinematics. To have inverse dynamics take acceleration into account, use GetJointForcesForAcceleration instead", true)]
			set
			{
				this.set_jointAcceleration_Injected(ref value);
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00002F58 File Offset: 0x00001158
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00002F6E File Offset: 0x0000116E
		public ArticulationReducedSpace jointForce
		{
			get
			{
				ArticulationReducedSpace articulationReducedSpace;
				this.get_jointForce_Injected(out articulationReducedSpace);
				return articulationReducedSpace;
			}
			set
			{
				this.set_jointForce_Injected(ref value);
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00002F78 File Offset: 0x00001178
		public ArticulationReducedSpace driveForce
		{
			get
			{
				ArticulationReducedSpace articulationReducedSpace;
				this.get_driveForce_Injected(out articulationReducedSpace);
				return articulationReducedSpace;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000CD RID: 205
		public extern int dofCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000CE RID: 206
		public extern int index
		{
			[NativeMethod("GetBodyIndex")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00002F8E File Offset: 0x0000118E
		public void TeleportRoot(Vector3 position, Quaternion rotation)
		{
			this.TeleportRoot_Injected(ref position, ref rotation);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00002F9C File Offset: 0x0000119C
		public Vector3 GetClosestPoint(Vector3 point)
		{
			Vector3 vector;
			this.GetClosestPoint_Injected(ref point, out vector);
			return vector;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00002FB4 File Offset: 0x000011B4
		public Vector3 GetRelativePointVelocity(Vector3 relativePoint)
		{
			Vector3 vector;
			this.GetRelativePointVelocity_Injected(ref relativePoint, out vector);
			return vector;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00002FCC File Offset: 0x000011CC
		public Vector3 GetPointVelocity(Vector3 worldPoint)
		{
			Vector3 vector;
			this.GetPointVelocity_Injected(ref worldPoint, out vector);
			return vector;
		}

		// Token: 0x060000D3 RID: 211
		[NativeMethod("GetDenseJacobian")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetDenseJacobian_Internal(ref ArticulationJacobian jacobian);

		// Token: 0x060000D4 RID: 212 RVA: 0x00002FE4 File Offset: 0x000011E4
		public int GetDenseJacobian(ref ArticulationJacobian jacobian)
		{
			bool flag = jacobian.elements == null;
			if (flag)
			{
				jacobian.elements = new List<float>();
			}
			return this.GetDenseJacobian_Internal(ref jacobian);
		}

		// Token: 0x060000D5 RID: 213
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetJointPositions(List<float> positions);

		// Token: 0x060000D6 RID: 214
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetJointPositions(List<float> positions);

		// Token: 0x060000D7 RID: 215
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetJointVelocities(List<float> velocities);

		// Token: 0x060000D8 RID: 216
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetJointVelocities(List<float> velocities);

		// Token: 0x060000D9 RID: 217
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetJointAccelerations(List<float> accelerations);

		// Token: 0x060000DA RID: 218
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetJointForces(List<float> forces);

		// Token: 0x060000DB RID: 219
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetJointForces(List<float> forces);

		// Token: 0x060000DC RID: 220 RVA: 0x00003018 File Offset: 0x00001218
		public ArticulationReducedSpace GetJointForcesForAcceleration(ArticulationReducedSpace acceleration)
		{
			ArticulationReducedSpace articulationReducedSpace;
			this.GetJointForcesForAcceleration_Injected(ref acceleration, out articulationReducedSpace);
			return articulationReducedSpace;
		}

		// Token: 0x060000DD RID: 221
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetDriveForces(List<float> forces);

		// Token: 0x060000DE RID: 222
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetJointGravityForces(List<float> forces);

		// Token: 0x060000DF RID: 223
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetJointCoriolisCentrifugalForces(List<float> forces);

		// Token: 0x060000E0 RID: 224
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetJointExternalForces(List<float> forces, float step);

		// Token: 0x060000E1 RID: 225
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetDriveTargets(List<float> targets);

		// Token: 0x060000E2 RID: 226
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetDriveTargets(List<float> targets);

		// Token: 0x060000E3 RID: 227
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetDriveTargetVelocities(List<float> targetVelocities);

		// Token: 0x060000E4 RID: 228
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetDriveTargetVelocities(List<float> targetVelocities);

		// Token: 0x060000E5 RID: 229
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetDofStartIndices(List<int> dofStartIndices);

		// Token: 0x060000E6 RID: 230
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetDriveTarget(ArticulationDriveAxis axis, float value);

		// Token: 0x060000E7 RID: 231
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetDriveTargetVelocity(ArticulationDriveAxis axis, float value);

		// Token: 0x060000E8 RID: 232
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetDriveLimits(ArticulationDriveAxis axis, float lower, float upper);

		// Token: 0x060000E9 RID: 233
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetDriveStiffness(ArticulationDriveAxis axis, float value);

		// Token: 0x060000EA RID: 234
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetDriveDamping(ArticulationDriveAxis axis, float value);

		// Token: 0x060000EB RID: 235
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetDriveForceLimit(ArticulationDriveAxis axis, float value);

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000EC RID: 236
		// (set) Token: 0x060000ED RID: 237
		public extern CollisionDetectionMode collisionDetectionMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00003030 File Offset: 0x00001230
		public void SnapAnchorToClosestContact()
		{
			bool flag = !base.transform.parent;
			if (!flag)
			{
				ArticulationBody articulationBody = base.transform.parent.GetComponentInParent<ArticulationBody>();
				while (articulationBody && !articulationBody.enabled)
				{
					articulationBody = articulationBody.transform.parent.GetComponentInParent<ArticulationBody>();
				}
				bool flag2 = !articulationBody;
				if (!flag2)
				{
					Vector3 worldCenterOfMass = articulationBody.worldCenterOfMass;
					Vector3 closestPoint = this.GetClosestPoint(worldCenterOfMass);
					this.anchorPosition = base.transform.InverseTransformPoint(closestPoint);
					this.anchorRotation = Quaternion.FromToRotation(Vector3.right, base.transform.InverseTransformDirection(worldCenterOfMass - closestPoint).normalized);
				}
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000EF RID: 239 RVA: 0x000030F9 File Offset: 0x000012F9
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x00003101 File Offset: 0x00001301
		[Obsolete("computeParentAnchor has been renamed to matchAnchors (UnityUpgradable) -> matchAnchors")]
		public bool computeParentAnchor
		{
			get
			{
				return this.matchAnchors;
			}
			set
			{
				this.matchAnchors = value;
			}
		}

		// Token: 0x060000F1 RID: 241
		[Obsolete("Setting joint accelerations is not supported in forward kinematics. To have inverse dynamics take acceleration into account, use GetJointForcesForAcceleration instead", true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetJointAccelerations(List<float> accelerations);

		// Token: 0x060000F3 RID: 243
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_anchorPosition_Injected(out Vector3 ret);

		// Token: 0x060000F4 RID: 244
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_anchorPosition_Injected(ref Vector3 value);

		// Token: 0x060000F5 RID: 245
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_parentAnchorPosition_Injected(out Vector3 ret);

		// Token: 0x060000F6 RID: 246
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_parentAnchorPosition_Injected(ref Vector3 value);

		// Token: 0x060000F7 RID: 247
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_anchorRotation_Injected(out Quaternion ret);

		// Token: 0x060000F8 RID: 248
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_anchorRotation_Injected(ref Quaternion value);

		// Token: 0x060000F9 RID: 249
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_parentAnchorRotation_Injected(out Quaternion ret);

		// Token: 0x060000FA RID: 250
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_parentAnchorRotation_Injected(ref Quaternion value);

		// Token: 0x060000FB RID: 251
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_xDrive_Injected(out ArticulationDrive ret);

		// Token: 0x060000FC RID: 252
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_xDrive_Injected(ref ArticulationDrive value);

		// Token: 0x060000FD RID: 253
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_yDrive_Injected(out ArticulationDrive ret);

		// Token: 0x060000FE RID: 254
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_yDrive_Injected(ref ArticulationDrive value);

		// Token: 0x060000FF RID: 255
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_zDrive_Injected(out ArticulationDrive ret);

		// Token: 0x06000100 RID: 256
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_zDrive_Injected(ref ArticulationDrive value);

		// Token: 0x06000101 RID: 257
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_excludeLayers_Injected(out LayerMask ret);

		// Token: 0x06000102 RID: 258
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_excludeLayers_Injected(ref LayerMask value);

		// Token: 0x06000103 RID: 259
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_includeLayers_Injected(out LayerMask ret);

		// Token: 0x06000104 RID: 260
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_includeLayers_Injected(ref LayerMask value);

		// Token: 0x06000105 RID: 261
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetAccumulatedForce_Injected([DefaultValue("Time.fixedDeltaTime")] float step, out Vector3 ret);

		// Token: 0x06000106 RID: 262
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetAccumulatedTorque_Injected([DefaultValue("Time.fixedDeltaTime")] float step, out Vector3 ret);

		// Token: 0x06000107 RID: 263
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddForce_Injected(ref Vector3 force, [DefaultValue("ForceMode.Force")] ForceMode mode);

		// Token: 0x06000108 RID: 264
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddRelativeForce_Injected(ref Vector3 force, [DefaultValue("ForceMode.Force")] ForceMode mode);

		// Token: 0x06000109 RID: 265
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddTorque_Injected(ref Vector3 torque, [DefaultValue("ForceMode.Force")] ForceMode mode);

		// Token: 0x0600010A RID: 266
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddRelativeTorque_Injected(ref Vector3 torque, [DefaultValue("ForceMode.Force")] ForceMode mode);

		// Token: 0x0600010B RID: 267
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddForceAtPosition_Injected(ref Vector3 force, ref Vector3 position, [DefaultValue("ForceMode.Force")] ForceMode mode);

		// Token: 0x0600010C RID: 268
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_velocity_Injected(out Vector3 ret);

		// Token: 0x0600010D RID: 269
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_velocity_Injected(ref Vector3 value);

		// Token: 0x0600010E RID: 270
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_angularVelocity_Injected(out Vector3 ret);

		// Token: 0x0600010F RID: 271
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_angularVelocity_Injected(ref Vector3 value);

		// Token: 0x06000110 RID: 272
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_centerOfMass_Injected(out Vector3 ret);

		// Token: 0x06000111 RID: 273
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_centerOfMass_Injected(ref Vector3 value);

		// Token: 0x06000112 RID: 274
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_worldCenterOfMass_Injected(out Vector3 ret);

		// Token: 0x06000113 RID: 275
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_inertiaTensor_Injected(out Vector3 ret);

		// Token: 0x06000114 RID: 276
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_inertiaTensor_Injected(ref Vector3 value);

		// Token: 0x06000115 RID: 277
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_inertiaTensorRotation_Injected(out Quaternion ret);

		// Token: 0x06000116 RID: 278
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_inertiaTensorRotation_Injected(ref Quaternion value);

		// Token: 0x06000117 RID: 279
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_jointPosition_Injected(out ArticulationReducedSpace ret);

		// Token: 0x06000118 RID: 280
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_jointPosition_Injected(ref ArticulationReducedSpace value);

		// Token: 0x06000119 RID: 281
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_jointVelocity_Injected(out ArticulationReducedSpace ret);

		// Token: 0x0600011A RID: 282
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_jointVelocity_Injected(ref ArticulationReducedSpace value);

		// Token: 0x0600011B RID: 283
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_jointAcceleration_Injected(out ArticulationReducedSpace ret);

		// Token: 0x0600011C RID: 284
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_jointAcceleration_Injected(ref ArticulationReducedSpace value);

		// Token: 0x0600011D RID: 285
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_jointForce_Injected(out ArticulationReducedSpace ret);

		// Token: 0x0600011E RID: 286
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_jointForce_Injected(ref ArticulationReducedSpace value);

		// Token: 0x0600011F RID: 287
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_driveForce_Injected(out ArticulationReducedSpace ret);

		// Token: 0x06000120 RID: 288
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void TeleportRoot_Injected(ref Vector3 position, ref Quaternion rotation);

		// Token: 0x06000121 RID: 289
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetClosestPoint_Injected(ref Vector3 point, out Vector3 ret);

		// Token: 0x06000122 RID: 290
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetRelativePointVelocity_Injected(ref Vector3 relativePoint, out Vector3 ret);

		// Token: 0x06000123 RID: 291
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetPointVelocity_Injected(ref Vector3 worldPoint, out Vector3 ret);

		// Token: 0x06000124 RID: 292
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetJointForcesForAcceleration_Injected(ref ArticulationReducedSpace acceleration, out ArticulationReducedSpace ret);
	}
}
