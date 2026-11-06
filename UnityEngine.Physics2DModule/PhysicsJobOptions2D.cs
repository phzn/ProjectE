using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000020 RID: 32
	[RequiredByNativeCode(Optional = true, GenerateProxy = true)]
	[NativeClass("PhysicsJobOptions2D", "struct PhysicsJobOptions2D;")]
	[NativeHeader("Modules/Physics2D/Public/Physics2DSettings.h")]
	public struct PhysicsJobOptions2D
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000263 RID: 611 RVA: 0x00006FB4 File Offset: 0x000051B4
		// (set) Token: 0x06000264 RID: 612 RVA: 0x00006FCC File Offset: 0x000051CC
		public bool useMultithreading
		{
			get
			{
				return this.m_UseMultithreading;
			}
			set
			{
				this.m_UseMultithreading = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000265 RID: 613 RVA: 0x00006FD8 File Offset: 0x000051D8
		// (set) Token: 0x06000266 RID: 614 RVA: 0x00006FF0 File Offset: 0x000051F0
		public bool useConsistencySorting
		{
			get
			{
				return this.m_UseConsistencySorting;
			}
			set
			{
				this.m_UseConsistencySorting = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000267 RID: 615 RVA: 0x00006FFC File Offset: 0x000051FC
		// (set) Token: 0x06000268 RID: 616 RVA: 0x00007014 File Offset: 0x00005214
		public int interpolationPosesPerJob
		{
			get
			{
				return this.m_InterpolationPosesPerJob;
			}
			set
			{
				this.m_InterpolationPosesPerJob = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000269 RID: 617 RVA: 0x00007020 File Offset: 0x00005220
		// (set) Token: 0x0600026A RID: 618 RVA: 0x00007038 File Offset: 0x00005238
		public int newContactsPerJob
		{
			get
			{
				return this.m_NewContactsPerJob;
			}
			set
			{
				this.m_NewContactsPerJob = value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x0600026B RID: 619 RVA: 0x00007044 File Offset: 0x00005244
		// (set) Token: 0x0600026C RID: 620 RVA: 0x0000705C File Offset: 0x0000525C
		public int collideContactsPerJob
		{
			get
			{
				return this.m_CollideContactsPerJob;
			}
			set
			{
				this.m_CollideContactsPerJob = value;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600026D RID: 621 RVA: 0x00007068 File Offset: 0x00005268
		// (set) Token: 0x0600026E RID: 622 RVA: 0x00007080 File Offset: 0x00005280
		public int clearFlagsPerJob
		{
			get
			{
				return this.m_ClearFlagsPerJob;
			}
			set
			{
				this.m_ClearFlagsPerJob = value;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600026F RID: 623 RVA: 0x0000708C File Offset: 0x0000528C
		// (set) Token: 0x06000270 RID: 624 RVA: 0x000070A4 File Offset: 0x000052A4
		public int clearBodyForcesPerJob
		{
			get
			{
				return this.m_ClearBodyForcesPerJob;
			}
			set
			{
				this.m_ClearBodyForcesPerJob = value;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000271 RID: 625 RVA: 0x000070B0 File Offset: 0x000052B0
		// (set) Token: 0x06000272 RID: 626 RVA: 0x000070C8 File Offset: 0x000052C8
		public int syncDiscreteFixturesPerJob
		{
			get
			{
				return this.m_SyncDiscreteFixturesPerJob;
			}
			set
			{
				this.m_SyncDiscreteFixturesPerJob = value;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000273 RID: 627 RVA: 0x000070D4 File Offset: 0x000052D4
		// (set) Token: 0x06000274 RID: 628 RVA: 0x000070EC File Offset: 0x000052EC
		public int syncContinuousFixturesPerJob
		{
			get
			{
				return this.m_SyncContinuousFixturesPerJob;
			}
			set
			{
				this.m_SyncContinuousFixturesPerJob = value;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000275 RID: 629 RVA: 0x000070F8 File Offset: 0x000052F8
		// (set) Token: 0x06000276 RID: 630 RVA: 0x00007110 File Offset: 0x00005310
		public int findNearestContactsPerJob
		{
			get
			{
				return this.m_FindNearestContactsPerJob;
			}
			set
			{
				this.m_FindNearestContactsPerJob = value;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000277 RID: 631 RVA: 0x0000711C File Offset: 0x0000531C
		// (set) Token: 0x06000278 RID: 632 RVA: 0x00007134 File Offset: 0x00005334
		public int updateTriggerContactsPerJob
		{
			get
			{
				return this.m_UpdateTriggerContactsPerJob;
			}
			set
			{
				this.m_UpdateTriggerContactsPerJob = value;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000279 RID: 633 RVA: 0x00007140 File Offset: 0x00005340
		// (set) Token: 0x0600027A RID: 634 RVA: 0x00007158 File Offset: 0x00005358
		public int islandSolverCostThreshold
		{
			get
			{
				return this.m_IslandSolverCostThreshold;
			}
			set
			{
				this.m_IslandSolverCostThreshold = value;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600027B RID: 635 RVA: 0x00007164 File Offset: 0x00005364
		// (set) Token: 0x0600027C RID: 636 RVA: 0x0000717C File Offset: 0x0000537C
		public int islandSolverBodyCostScale
		{
			get
			{
				return this.m_IslandSolverBodyCostScale;
			}
			set
			{
				this.m_IslandSolverBodyCostScale = value;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600027D RID: 637 RVA: 0x00007188 File Offset: 0x00005388
		// (set) Token: 0x0600027E RID: 638 RVA: 0x000071A0 File Offset: 0x000053A0
		public int islandSolverContactCostScale
		{
			get
			{
				return this.m_IslandSolverContactCostScale;
			}
			set
			{
				this.m_IslandSolverContactCostScale = value;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600027F RID: 639 RVA: 0x000071AC File Offset: 0x000053AC
		// (set) Token: 0x06000280 RID: 640 RVA: 0x000071C4 File Offset: 0x000053C4
		public int islandSolverJointCostScale
		{
			get
			{
				return this.m_IslandSolverJointCostScale;
			}
			set
			{
				this.m_IslandSolverJointCostScale = value;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000281 RID: 641 RVA: 0x000071D0 File Offset: 0x000053D0
		// (set) Token: 0x06000282 RID: 642 RVA: 0x000071E8 File Offset: 0x000053E8
		public int islandSolverBodiesPerJob
		{
			get
			{
				return this.m_IslandSolverBodiesPerJob;
			}
			set
			{
				this.m_IslandSolverBodiesPerJob = value;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000283 RID: 643 RVA: 0x000071F4 File Offset: 0x000053F4
		// (set) Token: 0x06000284 RID: 644 RVA: 0x0000720C File Offset: 0x0000540C
		public int islandSolverContactsPerJob
		{
			get
			{
				return this.m_IslandSolverContactsPerJob;
			}
			set
			{
				this.m_IslandSolverContactsPerJob = value;
			}
		}

		// Token: 0x0400008E RID: 142
		private bool m_UseMultithreading;

		// Token: 0x0400008F RID: 143
		private bool m_UseConsistencySorting;

		// Token: 0x04000090 RID: 144
		private int m_InterpolationPosesPerJob;

		// Token: 0x04000091 RID: 145
		private int m_NewContactsPerJob;

		// Token: 0x04000092 RID: 146
		private int m_CollideContactsPerJob;

		// Token: 0x04000093 RID: 147
		private int m_ClearFlagsPerJob;

		// Token: 0x04000094 RID: 148
		private int m_ClearBodyForcesPerJob;

		// Token: 0x04000095 RID: 149
		private int m_SyncDiscreteFixturesPerJob;

		// Token: 0x04000096 RID: 150
		private int m_SyncContinuousFixturesPerJob;

		// Token: 0x04000097 RID: 151
		private int m_FindNearestContactsPerJob;

		// Token: 0x04000098 RID: 152
		private int m_UpdateTriggerContactsPerJob;

		// Token: 0x04000099 RID: 153
		private int m_IslandSolverCostThreshold;

		// Token: 0x0400009A RID: 154
		private int m_IslandSolverBodyCostScale;

		// Token: 0x0400009B RID: 155
		private int m_IslandSolverContactCostScale;

		// Token: 0x0400009C RID: 156
		private int m_IslandSolverJointCostScale;

		// Token: 0x0400009D RID: 157
		private int m_IslandSolverBodiesPerJob;

		// Token: 0x0400009E RID: 158
		private int m_IslandSolverContactsPerJob;
	}
}
