using System;
using Pathfinding.PID;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding.ECS
{
	// Token: 0x02000301 RID: 769
	public struct MovementState : IComponentData, IQueryTypeParameter
	{
		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06001308 RID: 4872 RVA: 0x000738F2 File Offset: 0x00071AF2
		// (set) Token: 0x06001309 RID: 4873 RVA: 0x000738FF File Offset: 0x00071AFF
		public bool reachedDestination
		{
			[IgnoredByDeepProfiler]
			get
			{
				return (this.flags & 1) > 0;
			}
			[IgnoredByDeepProfiler]
			set
			{
				this.flags = (ushort)(((int)this.flags & -2) | (value ? 1 : 0));
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x0600130A RID: 4874 RVA: 0x00073919 File Offset: 0x00071B19
		// (set) Token: 0x0600130B RID: 4875 RVA: 0x00073926 File Offset: 0x00071B26
		public bool reachedDestinationAndOrientation
		{
			[IgnoredByDeepProfiler]
			get
			{
				return (this.flags & 2) > 0;
			}
			[IgnoredByDeepProfiler]
			set
			{
				this.flags = (ushort)(((int)this.flags & -3) | (value ? 2 : 0));
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x0600130C RID: 4876 RVA: 0x00073940 File Offset: 0x00071B40
		// (set) Token: 0x0600130D RID: 4877 RVA: 0x0007394D File Offset: 0x00071B4D
		public bool reachedEndOfPath
		{
			[IgnoredByDeepProfiler]
			get
			{
				return (this.flags & 4) > 0;
			}
			[IgnoredByDeepProfiler]
			set
			{
				this.flags = (ushort)(((int)this.flags & -5) | (value ? 4 : 0));
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x0600130E RID: 4878 RVA: 0x00073967 File Offset: 0x00071B67
		// (set) Token: 0x0600130F RID: 4879 RVA: 0x00073974 File Offset: 0x00071B74
		public bool reachedEndOfPathAndOrientation
		{
			[IgnoredByDeepProfiler]
			get
			{
				return (this.flags & 8) > 0;
			}
			[IgnoredByDeepProfiler]
			set
			{
				this.flags = (ushort)(((int)this.flags & -9) | (value ? 8 : 0));
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06001310 RID: 4880 RVA: 0x0007398E File Offset: 0x00071B8E
		// (set) Token: 0x06001311 RID: 4881 RVA: 0x0007399C File Offset: 0x00071B9C
		public bool reachedEndOfPart
		{
			[IgnoredByDeepProfiler]
			get
			{
				return (this.flags & 16) > 0;
			}
			[IgnoredByDeepProfiler]
			set
			{
				this.flags = (ushort)(((int)this.flags & -17) | (value ? 16 : 0));
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06001312 RID: 4882 RVA: 0x000739B7 File Offset: 0x00071BB7
		// (set) Token: 0x06001313 RID: 4883 RVA: 0x000739C5 File Offset: 0x00071BC5
		public bool traversingLastPart
		{
			[IgnoredByDeepProfiler]
			get
			{
				return (this.flags & 32) > 0;
			}
			[IgnoredByDeepProfiler]
			set
			{
				this.flags = (ushort)(((int)this.flags & -33) | (value ? 32 : 0));
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06001314 RID: 4884 RVA: 0x000739E0 File Offset: 0x00071BE0
		// (set) Token: 0x06001315 RID: 4885 RVA: 0x000739EE File Offset: 0x00071BEE
		public bool hasValidEndPoint
		{
			[IgnoredByDeepProfiler]
			get
			{
				return (this.flags & 64) > 0;
			}
			[IgnoredByDeepProfiler]
			set
			{
				this.flags = (ushort)(((int)this.flags & -65) | (value ? 64 : 0));
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06001316 RID: 4886 RVA: 0x00073A09 File Offset: 0x00071C09
		// (set) Token: 0x06001317 RID: 4887 RVA: 0x00073A13 File Offset: 0x00071C13
		public uint graphIndex
		{
			[IgnoredByDeepProfiler]
			get
			{
				return (uint)(this.flags >> 8);
			}
			[IgnoredByDeepProfiler]
			internal set
			{
				this.flags = (ushort)(((int)this.flags & -65281) | (int)((ushort)(value << 8)));
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06001318 RID: 4888 RVA: 0x00073A2D File Offset: 0x00071C2D
		public bool isOnValidNode
		{
			get
			{
				return this.hierarchicalNodeIndex != -1;
			}
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x00073A3B File Offset: 0x00071C3B
		public MovementState(Vector3 agentPosition)
		{
			this = default(MovementState);
			this.SetPathIsEmpty(agentPosition);
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x00073A4C File Offset: 0x00071C4C
		public void SetPathIsEmpty(Vector3 agentPosition)
		{
			this.nextCorner = agentPosition;
			this.endOfPath = agentPosition;
			this.closestOnNavmesh = agentPosition;
			this.hierarchicalNodeIndex = -1;
			this.remainingDistanceToEndOfPart = float.PositiveInfinity;
			this.reachedEndOfPath = false;
			this.reachedDestination = false;
			this.reachedEndOfPart = false;
			this.reachedDestinationAndOrientation = false;
			this.reachedEndOfPathAndOrientation = false;
			this.traversingLastPart = true;
			this.graphIndex = 255U;
		}

		// Token: 0x04000DE8 RID: 3560
		public PIDMovement.PersistentState followerState;

		// Token: 0x04000DE9 RID: 3561
		public float3 nextCorner;

		// Token: 0x04000DEA RID: 3562
		public float3 endOfPath;

		// Token: 0x04000DEB RID: 3563
		public float3 closestOnNavmesh;

		// Token: 0x04000DEC RID: 3564
		public float3 positionOffset;

		// Token: 0x04000DED RID: 3565
		public int hierarchicalNodeIndex;

		// Token: 0x04000DEE RID: 3566
		public float remainingDistanceToEndOfPart;

		// Token: 0x04000DEF RID: 3567
		public float rotationOffset;

		// Token: 0x04000DF0 RID: 3568
		public float rotationOffset2;

		// Token: 0x04000DF1 RID: 3569
		public ushort pathTracerVersion;

		// Token: 0x04000DF2 RID: 3570
		private ushort flags;

		// Token: 0x04000DF3 RID: 3571
		private const int ReachedDestinationFlag = 1;

		// Token: 0x04000DF4 RID: 3572
		private const int reachedDestinationAndOrientationFlag = 2;

		// Token: 0x04000DF5 RID: 3573
		private const int ReachedEndOfPathFlag = 4;

		// Token: 0x04000DF6 RID: 3574
		private const int reachedEndOfPathAndOrientationFlag = 8;

		// Token: 0x04000DF7 RID: 3575
		private const int ReachedEndOfPartFlag = 16;

		// Token: 0x04000DF8 RID: 3576
		private const int TraversingLastPartFlag = 32;

		// Token: 0x04000DF9 RID: 3577
		private const int HasValidEndPointFlag = 64;

		// Token: 0x04000DFA RID: 3578
		private const int GraphIndexOffsetInFlags = 8;

		// Token: 0x04000DFB RID: 3579
		private const ushort GraphIndexMaskInFlags = 65280;
	}
}
