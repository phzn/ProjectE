using System;
using Pathfinding.ECS.RVO;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Properties;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Serialization;

namespace Pathfinding.ECS
{
	// Token: 0x020002FD RID: 765
	[TypeManager.TypeOverridesAttribute(true, true, true)]
	[Serializable]
	public class ManagedState : IComponentData, IQueryTypeParameter, IDisposable, ICloneable
	{
		// Token: 0x1700030C RID: 780
		// (get) Token: 0x060012FA RID: 4858 RVA: 0x00003792 File Offset: 0x00001992
		[Obsolete("Use FollowerEntity.autoRepath, or the Pathfinding.ECS.AutoRepathPolicy component instead", true)]
		public AutoRepathPolicy autoRepath
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x060012FB RID: 4859 RVA: 0x0007365A File Offset: 0x0007185A
		// (set) Token: 0x060012FC RID: 4860 RVA: 0x00073662 File Offset: 0x00071862
		public Path pendingPath { get; private set; }

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x060012FD RID: 4861 RVA: 0x0007366B File Offset: 0x0007186B
		// (set) Token: 0x060012FE RID: 4862 RVA: 0x00073673 File Offset: 0x00071873
		public Path activePath { get; private set; }

		// Token: 0x060012FF RID: 4863 RVA: 0x0007367C File Offset: 0x0007187C
		public static void SetPath(Path path, ManagedState state, in AgentMovementPlane movementPlane, ref DestinationPoint destination)
		{
			if (path == null)
			{
				state.CancelCurrentPathRequest();
				state.ClearPath();
				return;
			}
			if (path.PipelineState == PathState.Created)
			{
				state.CancelCurrentPathRequest();
				state.pendingPath = path;
				path.Claim(state);
				AstarPath.StartPath(path, false, false);
				return;
			}
			if (path.PipelineState < PathState.ReturnQueue)
			{
				throw new ArgumentException("You must call the SetPath method with a path that either has been completely calculated or one whose path calculation has not been started at all. It looks like the path calculation for the path you tried to use has been started, but is not yet finished.");
			}
			if (state.pendingPath == path)
			{
				state.pendingPath = null;
			}
			else
			{
				state.CancelCurrentPathRequest();
				path.Claim(state);
			}
			ABPath abpath = path as ABPath;
			if (abpath == null)
			{
				throw new ArgumentException("This function only works with ABPaths, or paths inheriting from ABPath");
			}
			if (abpath.error)
			{
				abpath.Release(state, false);
				state.pathTracer.SetEmptyAndUnrepairable();
				return;
			}
			try
			{
				state.pathTracer.SetPath(abpath, movementPlane.value);
				if (state.activePath != null)
				{
					state.activePath.Release(state, false);
				}
				state.activePath = abpath;
			}
			catch (Exception ex)
			{
				abpath.Release(state, false);
				state.ClearPath();
				Debug.LogException(ex);
			}
			if (!abpath.endPointKnownBeforeCalculation)
			{
				destination = new DestinationPoint
				{
					destination = abpath.originalEndPoint,
					facingDirection = default(float3)
				};
				return;
			}
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x000737B4 File Offset: 0x000719B4
		public void ClearPath()
		{
			this.pathTracer.Clear();
			if (this.activePath != null)
			{
				this.activePath.Release(this, false);
				this.activePath = null;
			}
		}

		// Token: 0x06001301 RID: 4865 RVA: 0x000737DD File Offset: 0x000719DD
		public void CancelCurrentPathRequest()
		{
			if (this.pendingPath != null)
			{
				this.pendingPath.FailWithError("Canceled by script");
				this.pendingPath.Release(this, false);
				this.pendingPath = null;
			}
		}

		// Token: 0x06001302 RID: 4866 RVA: 0x0007380C File Offset: 0x00071A0C
		public void Dispose()
		{
			this.pathTracer.Dispose();
			if (this.pendingPath != null)
			{
				this.pendingPath.FailWithError("Canceled because entity was destroyed");
				this.pendingPath.Release(this, false);
				this.pendingPath = null;
			}
			if (this.activePath != null)
			{
				this.activePath.Release(this, false);
				this.activePath = null;
			}
		}

		// Token: 0x06001303 RID: 4867 RVA: 0x0007386C File Offset: 0x00071A6C
		public void PopNextLinkFromPath()
		{
			if (this.pathTracer.partCount < 2 && this.pathTracer.GetPartType(1) != Funnel.PartType.OffMeshLink)
			{
				throw new InvalidOperationException("The next part in the path is not an off-mesh link.");
			}
			this.pathTracer.PopParts(2);
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x000738A1 File Offset: 0x00071AA1
		object ICloneable.Clone()
		{
			return new ManagedState
			{
				pathTracer = this.pathTracer.Clone(),
				pendingPath = null,
				activePath = null
			};
		}

		// Token: 0x06001305 RID: 4869 RVA: 0x000738C7 File Offset: 0x00071AC7
		[Preserve]
		public ManagedState()
		{
		}

		// Token: 0x04000DCA RID: 3530
		public PathTracer pathTracer;

		// Token: 0x04000DCB RID: 3531
		[FormerlySerializedAs("rvoAgent")]
		[Obsolete("Use FollowerEntity.rvoSettings or the RVOAgent ECS component instead", false)]
		[DontCreateProperty]
		public RVOAgent rvoSettings = RVOAgent.Default;

		// Token: 0x04000DCC RID: 3532
		[Obsolete("Use ManagedSettings.onTraverseOffMeshLink instead", false)]
		[DontCreateProperty]
		[NonSerialized]
		public IOffMeshLinkHandler onTraverseOffMeshLink;

		// Token: 0x04000DCD RID: 3533
		[Obsolete("Use ManagedSettings.pathfindingSettings instead", false)]
		[DontCreateProperty]
		public PathRequestSettings pathfindingSettings;

		// Token: 0x04000DCE RID: 3534
		[FormerlySerializedAs("rvoEnabled")]
		[Obsolete("Use FollowerEntity.enableLocalAvoidance or remove/add the RVOAgent ECS component instead", false)]
		[DontCreateProperty]
		public bool enableLocalAvoidance;

		// Token: 0x04000DCF RID: 3535
		[Obsolete("Use FollowerEntity.enableGravity or toggle the enabled state of the GravityState ECS component instead", false)]
		[DontCreateProperty]
		public bool enableGravity = true;
	}
}
