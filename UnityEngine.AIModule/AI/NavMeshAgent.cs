using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.AI
{
	// Token: 0x0200000A RID: 10
	[MovedFrom("UnityEngine")]
	[NativeHeader("Modules/AI/Components/NavMeshAgent.bindings.h")]
	[NativeHeader("Modules/AI/NavMesh/NavMesh.bindings.h")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.ai.navigation@1.1/manual/NavMeshAgent.html")]
	public sealed class NavMeshAgent : Behaviour
	{
		// Token: 0x06000055 RID: 85 RVA: 0x000028CD File Offset: 0x00000ACD
		public bool SetDestination(Vector3 target)
		{
			return this.SetDestination_Injected(ref target);
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000056 RID: 86 RVA: 0x000028D8 File Offset: 0x00000AD8
		// (set) Token: 0x06000057 RID: 87 RVA: 0x000028EE File Offset: 0x00000AEE
		public Vector3 destination
		{
			get
			{
				Vector3 vector;
				this.get_destination_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_destination_Injected(ref value);
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000058 RID: 88
		// (set) Token: 0x06000059 RID: 89
		public extern float stoppingDistance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600005A RID: 90 RVA: 0x000028F8 File Offset: 0x00000AF8
		// (set) Token: 0x0600005B RID: 91 RVA: 0x0000290E File Offset: 0x00000B0E
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

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002918 File Offset: 0x00000B18
		// (set) Token: 0x0600005D RID: 93 RVA: 0x0000292E File Offset: 0x00000B2E
		[NativeProperty("Position")]
		public Vector3 nextPosition
		{
			get
			{
				Vector3 vector;
				this.get_nextPosition_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_nextPosition_Injected(ref value);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00002938 File Offset: 0x00000B38
		public Vector3 steeringTarget
		{
			get
			{
				Vector3 vector;
				this.get_steeringTarget_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00002950 File Offset: 0x00000B50
		public Vector3 desiredVelocity
		{
			get
			{
				Vector3 vector;
				this.get_desiredVelocity_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000060 RID: 96
		public extern float remainingDistance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000061 RID: 97
		// (set) Token: 0x06000062 RID: 98
		public extern float baseOffset
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000063 RID: 99
		public extern bool isOnOffMeshLink
		{
			[NativeName("IsOnOffMeshLink")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000064 RID: 100
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ActivateCurrentOffMeshLink(bool activated);

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00002966 File Offset: 0x00000B66
		public OffMeshLinkData currentOffMeshLinkData
		{
			get
			{
				return this.GetCurrentOffMeshLinkDataInternal();
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002970 File Offset: 0x00000B70
		[FreeFunction("NavMeshAgentScriptBindings::GetCurrentOffMeshLinkDataInternal", HasExplicitThis = true)]
		internal OffMeshLinkData GetCurrentOffMeshLinkDataInternal()
		{
			OffMeshLinkData offMeshLinkData;
			this.GetCurrentOffMeshLinkDataInternal_Injected(out offMeshLinkData);
			return offMeshLinkData;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00002986 File Offset: 0x00000B86
		public OffMeshLinkData nextOffMeshLinkData
		{
			get
			{
				return this.GetNextOffMeshLinkDataInternal();
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002990 File Offset: 0x00000B90
		[FreeFunction("NavMeshAgentScriptBindings::GetNextOffMeshLinkDataInternal", HasExplicitThis = true)]
		internal OffMeshLinkData GetNextOffMeshLinkDataInternal()
		{
			OffMeshLinkData offMeshLinkData;
			this.GetNextOffMeshLinkDataInternal_Injected(out offMeshLinkData);
			return offMeshLinkData;
		}

		// Token: 0x06000069 RID: 105
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void CompleteOffMeshLink();

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600006A RID: 106
		// (set) Token: 0x0600006B RID: 107
		public extern bool autoTraverseOffMeshLink
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600006C RID: 108
		// (set) Token: 0x0600006D RID: 109
		public extern bool autoBraking
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600006E RID: 110
		// (set) Token: 0x0600006F RID: 111
		public extern bool autoRepath
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000070 RID: 112
		public extern bool hasPath
		{
			[NativeName("HasPath")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000071 RID: 113
		public extern bool pathPending
		{
			[NativeName("PathPending")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000072 RID: 114
		public extern bool isPathStale
		{
			[NativeName("IsPathStale")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000073 RID: 115
		public extern NavMeshPathStatus pathStatus
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000074 RID: 116 RVA: 0x000029A8 File Offset: 0x00000BA8
		[NativeProperty("EndPositionOfCurrentPath")]
		public Vector3 pathEndPosition
		{
			get
			{
				Vector3 vector;
				this.get_pathEndPosition_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000029BE File Offset: 0x00000BBE
		public bool Warp(Vector3 newPosition)
		{
			return this.Warp_Injected(ref newPosition);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000029C8 File Offset: 0x00000BC8
		public void Move(Vector3 offset)
		{
			this.Move_Injected(ref offset);
		}

		// Token: 0x06000077 RID: 119
		[Obsolete("Set isStopped to true instead.")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Stop();

		// Token: 0x06000078 RID: 120 RVA: 0x000029D2 File Offset: 0x00000BD2
		[Obsolete("Set isStopped to true instead.")]
		public void Stop(bool stopUpdates)
		{
			this.Stop();
		}

		// Token: 0x06000079 RID: 121
		[Obsolete("Set isStopped to false instead.")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Resume();

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600007A RID: 122
		// (set) Token: 0x0600007B RID: 123
		public extern bool isStopped
		{
			[FreeFunction("NavMeshAgentScriptBindings::GetIsStopped", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction("NavMeshAgentScriptBindings::SetIsStopped", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x0600007C RID: 124
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ResetPath();

		// Token: 0x0600007D RID: 125
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool SetPath([NotNull("ArgumentNullException")] NavMeshPath path);

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600007E RID: 126 RVA: 0x000029DC File Offset: 0x00000BDC
		// (set) Token: 0x0600007F RID: 127 RVA: 0x00002A00 File Offset: 0x00000C00
		public NavMeshPath path
		{
			get
			{
				NavMeshPath navMeshPath = new NavMeshPath();
				this.CopyPathTo(navMeshPath);
				return navMeshPath;
			}
			set
			{
				bool flag = value == null;
				if (flag)
				{
					throw new NullReferenceException();
				}
				this.SetPath(value);
			}
		}

		// Token: 0x06000080 RID: 128
		[NativeMethod("CopyPath")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void CopyPathTo([NotNull("ArgumentNullException")] NavMeshPath path);

		// Token: 0x06000081 RID: 129
		[NativeName("DistanceToEdge")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool FindClosestEdge(out NavMeshHit hit);

		// Token: 0x06000082 RID: 130 RVA: 0x00002A24 File Offset: 0x00000C24
		public bool Raycast(Vector3 targetPosition, out NavMeshHit hit)
		{
			return this.Raycast_Injected(ref targetPosition, out hit);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00002A30 File Offset: 0x00000C30
		public bool CalculatePath(Vector3 targetPosition, NavMeshPath path)
		{
			path.ClearCorners();
			return this.CalculatePathInternal(targetPosition, path);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00002A51 File Offset: 0x00000C51
		[FreeFunction("NavMeshAgentScriptBindings::CalculatePathInternal", HasExplicitThis = true)]
		private bool CalculatePathInternal(Vector3 targetPosition, [NotNull("ArgumentNullException")] NavMeshPath path)
		{
			return this.CalculatePathInternal_Injected(ref targetPosition, path);
		}

		// Token: 0x06000085 RID: 133
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool SamplePathPosition(int areaMask, float maxDistance, out NavMeshHit hit);

		// Token: 0x06000086 RID: 134
		[Obsolete("Use SetAreaCost instead.")]
		[NativeMethod("SetAreaCost")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetLayerCost(int layer, float cost);

		// Token: 0x06000087 RID: 135
		[Obsolete("Use GetAreaCost instead.")]
		[NativeMethod("GetAreaCost")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetLayerCost(int layer);

		// Token: 0x06000088 RID: 136
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetAreaCost(int areaIndex, float areaCost);

		// Token: 0x06000089 RID: 137
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetAreaCost(int areaIndex);

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00002A5C File Offset: 0x00000C5C
		public Object navMeshOwner
		{
			get
			{
				return this.GetOwnerInternal();
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600008B RID: 139
		// (set) Token: 0x0600008C RID: 140
		public extern int agentTypeID
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x0600008D RID: 141
		[NativeName("GetCurrentPolygonOwner")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Object GetOwnerInternal();

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00002A64 File Offset: 0x00000C64
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00002A7C File Offset: 0x00000C7C
		[Obsolete("Use areaMask instead.")]
		public int walkableMask
		{
			get
			{
				return this.areaMask;
			}
			set
			{
				this.areaMask = value;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000090 RID: 144
		// (set) Token: 0x06000091 RID: 145
		public extern int areaMask
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000092 RID: 146
		// (set) Token: 0x06000093 RID: 147
		public extern float speed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000094 RID: 148
		// (set) Token: 0x06000095 RID: 149
		public extern float angularSpeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000096 RID: 150
		// (set) Token: 0x06000097 RID: 151
		public extern float acceleration
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000098 RID: 152
		// (set) Token: 0x06000099 RID: 153
		public extern bool updatePosition
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600009A RID: 154
		// (set) Token: 0x0600009B RID: 155
		public extern bool updateRotation
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600009C RID: 156
		// (set) Token: 0x0600009D RID: 157
		public extern bool updateUpAxis
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600009E RID: 158
		// (set) Token: 0x0600009F RID: 159
		public extern float radius
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000A0 RID: 160
		// (set) Token: 0x060000A1 RID: 161
		public extern float height
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000A2 RID: 162
		// (set) Token: 0x060000A3 RID: 163
		public extern ObstacleAvoidanceType obstacleAvoidanceType
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000A4 RID: 164
		// (set) Token: 0x060000A5 RID: 165
		public extern int avoidancePriority
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000A6 RID: 166
		public extern bool isOnNavMesh
		{
			[NativeName("InCrowdSystem")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x060000A8 RID: 168
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool SetDestination_Injected(ref Vector3 target);

		// Token: 0x060000A9 RID: 169
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_destination_Injected(out Vector3 ret);

		// Token: 0x060000AA RID: 170
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_destination_Injected(ref Vector3 value);

		// Token: 0x060000AB RID: 171
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_velocity_Injected(out Vector3 ret);

		// Token: 0x060000AC RID: 172
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_velocity_Injected(ref Vector3 value);

		// Token: 0x060000AD RID: 173
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_nextPosition_Injected(out Vector3 ret);

		// Token: 0x060000AE RID: 174
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_nextPosition_Injected(ref Vector3 value);

		// Token: 0x060000AF RID: 175
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_steeringTarget_Injected(out Vector3 ret);

		// Token: 0x060000B0 RID: 176
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_desiredVelocity_Injected(out Vector3 ret);

		// Token: 0x060000B1 RID: 177
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetCurrentOffMeshLinkDataInternal_Injected(out OffMeshLinkData ret);

		// Token: 0x060000B2 RID: 178
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetNextOffMeshLinkDataInternal_Injected(out OffMeshLinkData ret);

		// Token: 0x060000B3 RID: 179
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_pathEndPosition_Injected(out Vector3 ret);

		// Token: 0x060000B4 RID: 180
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool Warp_Injected(ref Vector3 newPosition);

		// Token: 0x060000B5 RID: 181
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Move_Injected(ref Vector3 offset);

		// Token: 0x060000B6 RID: 182
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool Raycast_Injected(ref Vector3 targetPosition, out NavMeshHit hit);

		// Token: 0x060000B7 RID: 183
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool CalculatePathInternal_Injected(ref Vector3 targetPosition, NavMeshPath path);
	}
}
