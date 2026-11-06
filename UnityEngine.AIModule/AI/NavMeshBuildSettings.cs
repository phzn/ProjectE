using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.AI
{
	// Token: 0x02000020 RID: 32
	[NativeHeader("Modules/AI/Public/NavMeshBuildSettings.h")]
	public struct NavMeshBuildSettings
	{
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600019D RID: 413 RVA: 0x00003610 File Offset: 0x00001810
		// (set) Token: 0x0600019E RID: 414 RVA: 0x00003628 File Offset: 0x00001828
		public int agentTypeID
		{
			get
			{
				return this.m_AgentTypeID;
			}
			set
			{
				this.m_AgentTypeID = value;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600019F RID: 415 RVA: 0x00003634 File Offset: 0x00001834
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x0000364C File Offset: 0x0000184C
		public float agentRadius
		{
			get
			{
				return this.m_AgentRadius;
			}
			set
			{
				this.m_AgentRadius = value;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x00003658 File Offset: 0x00001858
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x00003670 File Offset: 0x00001870
		public float agentHeight
		{
			get
			{
				return this.m_AgentHeight;
			}
			set
			{
				this.m_AgentHeight = value;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x0000367C File Offset: 0x0000187C
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x00003694 File Offset: 0x00001894
		public float agentSlope
		{
			get
			{
				return this.m_AgentSlope;
			}
			set
			{
				this.m_AgentSlope = value;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x000036A0 File Offset: 0x000018A0
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x000036B8 File Offset: 0x000018B8
		public float agentClimb
		{
			get
			{
				return this.m_AgentClimb;
			}
			set
			{
				this.m_AgentClimb = value;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x000036C4 File Offset: 0x000018C4
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x000036DC File Offset: 0x000018DC
		public float ledgeDropHeight
		{
			get
			{
				return this.m_LedgeDropHeight;
			}
			set
			{
				this.m_LedgeDropHeight = value;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x000036E8 File Offset: 0x000018E8
		// (set) Token: 0x060001AA RID: 426 RVA: 0x00003700 File Offset: 0x00001900
		public float maxJumpAcrossDistance
		{
			get
			{
				return this.m_MaxJumpAcrossDistance;
			}
			set
			{
				this.m_MaxJumpAcrossDistance = value;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001AB RID: 427 RVA: 0x0000370C File Offset: 0x0000190C
		// (set) Token: 0x060001AC RID: 428 RVA: 0x00003724 File Offset: 0x00001924
		public float minRegionArea
		{
			get
			{
				return this.m_MinRegionArea;
			}
			set
			{
				this.m_MinRegionArea = value;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00003730 File Offset: 0x00001930
		// (set) Token: 0x060001AE RID: 430 RVA: 0x0000374B File Offset: 0x0000194B
		public bool overrideVoxelSize
		{
			get
			{
				return this.m_OverrideVoxelSize != 0;
			}
			set
			{
				this.m_OverrideVoxelSize = (value ? 1 : 0);
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001AF RID: 431 RVA: 0x0000375C File Offset: 0x0000195C
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x00003774 File Offset: 0x00001974
		public float voxelSize
		{
			get
			{
				return this.m_VoxelSize;
			}
			set
			{
				this.m_VoxelSize = value;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x00003780 File Offset: 0x00001980
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x0000379B File Offset: 0x0000199B
		public bool overrideTileSize
		{
			get
			{
				return this.m_OverrideTileSize != 0;
			}
			set
			{
				this.m_OverrideTileSize = (value ? 1 : 0);
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x000037AC File Offset: 0x000019AC
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x000037C4 File Offset: 0x000019C4
		public int tileSize
		{
			get
			{
				return this.m_TileSize;
			}
			set
			{
				this.m_TileSize = value;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x000037D0 File Offset: 0x000019D0
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x000037E8 File Offset: 0x000019E8
		public uint maxJobWorkers
		{
			get
			{
				return this.m_MaxJobWorkers;
			}
			set
			{
				this.m_MaxJobWorkers = value;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x000037F4 File Offset: 0x000019F4
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x0000380F File Offset: 0x00001A0F
		public bool preserveTilesOutsideBounds
		{
			get
			{
				return this.m_PreserveTilesOutsideBounds != 0;
			}
			set
			{
				this.m_PreserveTilesOutsideBounds = (value ? 1 : 0);
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00003820 File Offset: 0x00001A20
		// (set) Token: 0x060001BA RID: 442 RVA: 0x0000383B File Offset: 0x00001A3B
		public bool buildHeightMesh
		{
			get
			{
				return this.m_BuildHeightMesh != 0;
			}
			set
			{
				this.m_BuildHeightMesh = (value ? 1 : 0);
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0000384C File Offset: 0x00001A4C
		// (set) Token: 0x060001BC RID: 444 RVA: 0x00003864 File Offset: 0x00001A64
		public NavMeshBuildDebugSettings debug
		{
			get
			{
				return this.m_Debug;
			}
			set
			{
				this.m_Debug = value;
			}
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00003870 File Offset: 0x00001A70
		public string[] ValidationReport(Bounds buildBounds)
		{
			return NavMeshBuildSettings.InternalValidationReport(this, buildBounds);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000388E File Offset: 0x00001A8E
		[FreeFunction]
		[NativeHeader("Modules/AI/Public/NavMeshBuildSettings.h")]
		private static string[] InternalValidationReport(NavMeshBuildSettings buildSettings, Bounds buildBounds)
		{
			return NavMeshBuildSettings.InternalValidationReport_Injected(ref buildSettings, ref buildBounds);
		}

		// Token: 0x060001BF RID: 447
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string[] InternalValidationReport_Injected(ref NavMeshBuildSettings buildSettings, ref Bounds buildBounds);

		// Token: 0x04000068 RID: 104
		private int m_AgentTypeID;

		// Token: 0x04000069 RID: 105
		private float m_AgentRadius;

		// Token: 0x0400006A RID: 106
		private float m_AgentHeight;

		// Token: 0x0400006B RID: 107
		private float m_AgentSlope;

		// Token: 0x0400006C RID: 108
		private float m_AgentClimb;

		// Token: 0x0400006D RID: 109
		private float m_LedgeDropHeight;

		// Token: 0x0400006E RID: 110
		private float m_MaxJumpAcrossDistance;

		// Token: 0x0400006F RID: 111
		private float m_MinRegionArea;

		// Token: 0x04000070 RID: 112
		private int m_OverrideVoxelSize;

		// Token: 0x04000071 RID: 113
		private float m_VoxelSize;

		// Token: 0x04000072 RID: 114
		private int m_OverrideTileSize;

		// Token: 0x04000073 RID: 115
		private int m_TileSize;

		// Token: 0x04000074 RID: 116
		private int m_BuildHeightMesh;

		// Token: 0x04000075 RID: 117
		private uint m_MaxJobWorkers;

		// Token: 0x04000076 RID: 118
		private int m_PreserveTilesOutsideBounds;

		// Token: 0x04000077 RID: 119
		private NavMeshBuildDebugSettings m_Debug;
	}
}
