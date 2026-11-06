using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Pathfinding.Collections;
using Pathfinding.Pooling;
using Pathfinding.Util;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000B5 RID: 181
	[BurstCompile]
	public abstract class Path : IPathInternals
	{
		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060005E8 RID: 1512 RVA: 0x0001CB41 File Offset: 0x0001AD41
		// (set) Token: 0x060005E9 RID: 1513 RVA: 0x0001CB49 File Offset: 0x0001AD49
		public PathState PipelineState { get; private set; }

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060005EA RID: 1514 RVA: 0x0001CB52 File Offset: 0x0001AD52
		// (set) Token: 0x060005EB RID: 1515 RVA: 0x0001CB5F File Offset: 0x0001AD5F
		[Obsolete("Use path.traversalConstraint.traversalProvider and path.traversalCosts.traversalProvider instead")]
		public ITraversalProvider traversalProvider
		{
			get
			{
				return this.traversalConstraint.traversalProvider;
			}
			set
			{
				this.traversalConstraint.traversalProvider = value;
				this.traversalCosts.traversalProvider = value;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060005EC RID: 1516 RVA: 0x0001CB79 File Offset: 0x0001AD79
		// (set) Token: 0x060005ED RID: 1517 RVA: 0x0001CB84 File Offset: 0x0001AD84
		public PathCompleteState CompleteState
		{
			get
			{
				return this.completeState;
			}
			protected set
			{
				lock (this)
				{
					if (this.completeState != PathCompleteState.Error)
					{
						this.completeState = value;
					}
				}
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060005EE RID: 1518 RVA: 0x0001CBCC File Offset: 0x0001ADCC
		public bool error
		{
			get
			{
				return this.CompleteState == PathCompleteState.Error;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x0001CBD7 File Offset: 0x0001ADD7
		// (set) Token: 0x060005F0 RID: 1520 RVA: 0x0001CBDF File Offset: 0x0001ADDF
		public string errorLog { get; private set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x0001CBE8 File Offset: 0x0001ADE8
		// (set) Token: 0x060005F2 RID: 1522 RVA: 0x0001CBF0 File Offset: 0x0001ADF0
		public float duration { get; internal set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x0001CBF9 File Offset: 0x0001ADF9
		// (set) Token: 0x060005F4 RID: 1524 RVA: 0x0001CC01 File Offset: 0x0001AE01
		public int searchedNodes { get; protected set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x0001CC0A File Offset: 0x0001AE0A
		// (set) Token: 0x060005F6 RID: 1526 RVA: 0x0001CC12 File Offset: 0x0001AE12
		bool IPathInternals.Pooled { get; set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x0001CC1B File Offset: 0x0001AE1B
		[Obsolete("Use the traversalConstraint field instead. Check the migration guide for version 5.4 for more information.")]
		public NNConstraintPathProxy nnConstraint
		{
			get
			{
				return new NNConstraintPathProxy(this);
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060005F8 RID: 1528 RVA: 0x0001CC23 File Offset: 0x0001AE23
		// (set) Token: 0x060005F9 RID: 1529 RVA: 0x0001CC2B File Offset: 0x0001AE2B
		public ushort pathID { get; private set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060005FA RID: 1530 RVA: 0x0001CC34 File Offset: 0x0001AE34
		// (set) Token: 0x060005FB RID: 1531 RVA: 0x0001CC41 File Offset: 0x0001AE41
		[Obsolete("Use traversalConstraint.tags instead")]
		public int enabledTags
		{
			get
			{
				return this.traversalConstraint.tags;
			}
			set
			{
				this.traversalConstraint.tags = value;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060005FC RID: 1532 RVA: 0x0001CC4F File Offset: 0x0001AE4F
		// (set) Token: 0x060005FD RID: 1533 RVA: 0x0001CC5C File Offset: 0x0001AE5C
		[Obsolete("Use traversalCosts.tagEntryCosts instead")]
		public uint[] tagPenalties
		{
			get
			{
				return this.traversalCosts.tagEntryCosts;
			}
			set
			{
				this.traversalCosts.tagEntryCosts = value;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060005FE RID: 1534 RVA: 0x0001CC6C File Offset: 0x0001AE6C
		public NearestNodeConstraint nearestNodeConstraint
		{
			get
			{
				NearestNodeConstraint nearestNodeConstraint = this.traversalConstraint.ToNearestNodeConstraint();
				nearestNodeConstraint.distanceMetric = this.nearestNodeDistanceMetric;
				return nearestNodeConstraint;
			}
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x0001CC93 File Offset: 0x0001AE93
		public void UseSettings(PathRequestSettings settings)
		{
			this.traversalConstraint = settings.ToTraversalConstraint();
			this.traversalCosts = settings.ToTraversalCosts();
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x0001CCB0 File Offset: 0x0001AEB0
		public float GetTotalLength()
		{
			if (this.vectorPath == null)
			{
				return float.PositiveInfinity;
			}
			float num = 0f;
			for (int i = 0; i < this.vectorPath.Count - 1; i++)
			{
				num += Vector3.Distance(this.vectorPath[i], this.vectorPath[i + 1]);
			}
			return num;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x0001CD0C File Offset: 0x0001AF0C
		public IEnumerator WaitForPath()
		{
			if (this.PipelineState == PathState.Created)
			{
				throw new InvalidOperationException("This path has not been started yet");
			}
			while (this.PipelineState != PathState.Returned)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0001CD1B File Offset: 0x0001AF1B
		public void BlockUntilCalculated()
		{
			AstarPath.BlockUntilCalculated(this);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0001CD23 File Offset: 0x0001AF23
		[Obsolete("Use traversalCosts instead")]
		public uint GetTagPenalty(int tag)
		{
			return this.traversalCosts.GetTagEntryCost((uint)tag);
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0001CD31 File Offset: 0x0001AF31
		[Obsolete("Use traversalConstraint.CanTraverse instead")]
		public bool CanTraverse(GraphNode node)
		{
			return this.traversalConstraint.CanTraverse(node);
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0001CD3F File Offset: 0x0001AF3F
		[Obsolete("Use traversalConstraint.CanTraverse instead")]
		public bool CanTraverse(GraphNode from, GraphNode to)
		{
			return this.traversalConstraint.CanTraverse(from, to);
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0001AA07 File Offset: 0x00018C07
		[Obsolete("Use traversalCosts instead", true)]
		public uint GetTraversalCost(GraphNode node)
		{
			return 0U;
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0001CD4E File Offset: 0x0001AF4E
		public bool IsDone()
		{
			return this.PipelineState > PathState.Processing;
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0001CD5C File Offset: 0x0001AF5C
		void IPathInternals.AdvanceState(PathState s)
		{
			lock (this)
			{
				this.PipelineState = (PathState)Math.Max((int)this.PipelineState, (int)s);
			}
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0001CDA4 File Offset: 0x0001AFA4
		public void FailWithError(string msg)
		{
			this.Error();
			if (this.errorLog != "")
			{
				this.errorLog = this.errorLog + "\n" + msg;
				return;
			}
			this.errorLog = msg;
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x0001CDDD File Offset: 0x0001AFDD
		public void Error()
		{
			this.CompleteState = PathCompleteState.Error;
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0001CDE8 File Offset: 0x0001AFE8
		private void ErrorCheck()
		{
			if (!this.hasBeenReset)
			{
				this.FailWithError("Please use the static Construct function for creating paths, do not use the normal constructors.");
			}
			if (((IPathInternals)this).Pooled)
			{
				this.FailWithError("The path is currently in a path pool. Are you sending the path for calculation twice?");
			}
			if (this.PipelineState > PathState.Processing)
			{
				this.FailWithError("This path has already been processed. Do not request a path with the same path object twice.");
			}
			this.CheckTraversalProviderCompatibility();
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00003788 File Offset: 0x00001988
		private void CheckTraversalProviderCompatibility()
		{
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0001CE38 File Offset: 0x0001B038
		protected virtual void OnEnterPool()
		{
			if (this.vectorPath != null)
			{
				ListPool<Vector3>.Release(ref this.vectorPath);
			}
			if (this.path != null)
			{
				ListPool<GraphNode>.Release(ref this.path);
			}
			this.callback = null;
			this.immediateCallback = null;
			this.traversalConstraint = TraversalConstraint.None;
			this.traversalCosts = default(TraversalCosts);
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x0001CE90 File Offset: 0x0001B090
		protected virtual void Reset()
		{
			if (AstarPath.active == null)
			{
				throw new NullReferenceException("No AstarPath object found in the scene. Make sure there is one or do not create paths in Awake");
			}
			this.hasBeenReset = true;
			this.PipelineState = PathState.Created;
			this.releasedNotSilent = false;
			this.callback = null;
			this.immediateCallback = null;
			this.errorLog = "";
			this.completeState = PathCompleteState.NotCalculated;
			this.path = ListPool<GraphNode>.Claim();
			this.vectorPath = ListPool<Vector3>.Claim();
			this.duration = 0f;
			this.searchedNodes = 0;
			this.heuristic = AstarPath.active.heuristic;
			this.heuristicScale = AstarPath.active.heuristicScale;
			this.pathID = AstarPath.active.GetNextPathID();
			this.traversalConstraint = TraversalConstraint.None;
			this.traversalCosts = default(TraversalCosts);
			this.nearestNodeDistanceMetric = DistanceMetric.Euclidean;
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0001CF60 File Offset: 0x0001B160
		public void Claim(object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			for (int i = 0; i < this.claimCount; i++)
			{
				if (this.claimed[i] == o)
				{
					throw new ArgumentException("You have already claimed the path with that object (" + ((o != null) ? o.ToString() : null) + "). Are you claiming the path with the same object twice?");
				}
			}
			if (this.claimCount == this.claimed.Length)
			{
				Memory.Realloc<object>(ref this.claimed, this.claimCount * 2);
			}
			this.claimed[this.claimCount] = o;
			this.claimCount++;
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0001CFF8 File Offset: 0x0001B1F8
		public void Release(object o, bool silent = false)
		{
			for (int i = 0; i < this.claimCount; i++)
			{
				if (this.claimed[i] == o)
				{
					this.claimCount--;
					this.claimed[i] = this.claimed[this.claimCount];
					this.claimed[this.claimCount] = null;
					if (!silent)
					{
						this.releasedNotSilent = true;
					}
					if (this.claimCount == 0 && this.releasedNotSilent)
					{
						PathPool.Pool(this);
					}
					return;
				}
			}
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			if (this.claimCount == 0)
			{
				throw new ArgumentException("You are releasing a path which is not claimed at all (most likely it has been pooled already). Are you releasing the path with the same object (" + ((o != null) ? o.ToString() : null) + ") twice?\nCheck out the documentation on path pooling for help.");
			}
			throw new ArgumentException("You are releasing a path which has not been claimed with this object (" + ((o != null) ? o.ToString() : null) + "). Are you releasing the path with the same object twice?\nCheck out the documentation on path pooling for help.");
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x0001D0CA File Offset: 0x0001B2CA
		protected virtual void Trace(ref Path.SearchContext ctx, uint fromPathNodeIndex)
		{
			this.Trace(ref ctx, fromPathNodeIndex, true);
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0001D0D8 File Offset: 0x0001B2D8
		protected void Trace(ref Path.SearchContext ctx, uint fromPathNodeIndex, bool reverse)
		{
			uint num = fromPathNodeIndex;
			int num2 = 0;
			PathHandler pathHandler = ctx.pathHandler;
			UnsafeSpan<PathNode> pathNodes = pathHandler.pathNodes;
			while (num != 0U)
			{
				num = pathNodes[num].parentIndex;
				num2++;
				if (num2 > 16384)
				{
					Debug.LogWarning("Infinite loop? >16384 node path. Remove this message if you really have that long paths (Path.cs, Trace method)");
					break;
				}
			}
			if (this.path.Capacity < num2)
			{
				this.path.Capacity = num2;
			}
			num = fromPathNodeIndex;
			GraphNode graphNode = null;
			for (int i = 0; i < num2; i++)
			{
				GraphNode graphNode2;
				if (pathHandler.IsTemporaryNode(num))
				{
					graphNode2 = pathHandler.GetNode(pathHandler.GetTemporaryNode(num).associatedNode);
				}
				else
				{
					graphNode2 = pathHandler.GetNode(num);
				}
				if (graphNode2 != graphNode)
				{
					this.path.Add(graphNode2);
					graphNode = graphNode2;
				}
				num = pathNodes[num].parentIndex;
			}
			if (reverse)
			{
				this.path.Reverse();
			}
			num2 = this.path.Count;
			if (this.vectorPath.Capacity < num2)
			{
				this.vectorPath.Capacity = num2;
			}
			for (int j = 0; j < num2; j++)
			{
				this.vectorPath.Add((Vector3)this.path[j].position);
			}
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x0001D204 File Offset: 0x0001B404
		protected void DebugStringPrefix(PathLog logMode, StringBuilder text)
		{
			text.Append(this.error ? "Path Failed : " : "Path Completed : ");
			text.Append("Computation Time ");
			text.Append(this.duration.ToString((logMode == PathLog.Heavy) ? "0.000 ms " : "0.00 ms "));
			text.Append("Searched Nodes ").Append(this.searchedNodes);
			if (!this.error)
			{
				text.Append(" Path Length ");
				text.Append((this.path == null) ? "Null" : this.path.Count.ToString());
			}
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x0001D2B4 File Offset: 0x0001B4B4
		protected void DebugStringSuffix(PathLog logMode, StringBuilder text)
		{
			if (this.error)
			{
				text.Append("\nError: ").Append(this.errorLog);
			}
			if (logMode == PathLog.Heavy && !AstarPath.active.IsUsingMultithreading)
			{
				text.Append("\nCallback references ");
				if (this.callback != null)
				{
					text.Append(this.callback.Target.GetType().FullName).AppendLine();
				}
				else
				{
					text.AppendLine("NULL");
				}
			}
			text.Append("\nUnique Path ID: ").Append(this.pathID);
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x0001D34A File Offset: 0x0001B54A
		protected virtual void DebugString(StringBuilder builder, PathLog logMode)
		{
			if (logMode == PathLog.None || (!this.error && logMode == PathLog.OnlyErrors))
			{
				return;
			}
			this.DebugStringPrefix(logMode, builder);
			this.DebugStringSuffix(logMode, builder);
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x0001D36C File Offset: 0x0001B56C
		protected virtual void ReturnPath()
		{
			if (this.callback != null)
			{
				this.callback(this);
			}
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x0001D384 File Offset: 0x0001B584
		protected void PrepareBase(PathHandler pathHandler)
		{
			pathHandler.InitializeForPath(this);
			try
			{
				this.ErrorCheck();
			}
			catch (Exception ex)
			{
				this.FailWithError(ex.Message);
			}
		}

		// Token: 0x06000618 RID: 1560
		protected abstract void Prepare(ref Path.SearchContext ctx);

		// Token: 0x06000619 RID: 1561 RVA: 0x0001D3C0 File Offset: 0x0001B5C0
		protected virtual void Cleanup(ref Path.SearchContext ctx)
		{
			PathHandler pathHandler = ctx.pathHandler;
			UnsafeSpan<PathNode> pathNodes = pathHandler.pathNodes;
			uint num = 0U;
			while ((ulong)num < (ulong)((long)pathHandler.numTemporaryNodes))
			{
				uint num2 = pathHandler.temporaryNodeStartIndex + num;
				ref TemporaryNode temporaryNode = ref pathHandler.GetTemporaryNode(num2);
				GraphNode node = pathHandler.GetNode(temporaryNode.associatedNode);
				uint num3 = 0U;
				while ((ulong)num3 < (ulong)((long)node.PathNodeVariants))
				{
					pathNodes[temporaryNode.associatedNode + num3].flag1 = false;
					pathNodes[temporaryNode.associatedNode + num3].flag2 = false;
					num3 += 1U;
				}
				num += 1U;
			}
		}

		// Token: 0x0600061A RID: 1562
		protected abstract void OnHeapExhausted(ref Path.SearchContext ctx);

		// Token: 0x0600061B RID: 1563
		protected abstract void OnFoundEndNode(ref Path.SearchContext ctx, uint pathNode, uint hScore, uint gScore);

		// Token: 0x0600061C RID: 1564 RVA: 0x00003788 File Offset: 0x00001988
		public virtual void OnVisitNode(ref Path.SearchContext ctx, uint pathNode, uint hScore, uint gScore)
		{
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0001D458 File Offset: 0x0001B658
		protected unsafe virtual void CalculateStep(ref Path.SearchContext ctx, long targetTick)
		{
			int num = 0;
			uint temporaryNodeStartIndex = ctx.pathHandler.temporaryNodeStartIndex;
			PathHandler pathHandler = ctx.pathHandler;
			while (this.CompleteState == PathCompleteState.NotCalculated)
			{
				int searchedNodes = this.searchedNodes;
				this.searchedNodes = searchedNodes + 1;
				if (pathHandler.heap.isEmpty)
				{
					this.OnHeapExhausted(ref ctx);
					return;
				}
				uint num3;
				uint num4;
				uint num2 = pathHandler.heap.Remove(pathHandler.pathNodes, out num3, out num4);
				if (num2 >= temporaryNodeStartIndex)
				{
					TemporaryNode temporaryNode = *pathHandler.GetTemporaryNode(num2);
					if (temporaryNode.type == TemporaryNodeType.Start)
					{
						pathHandler.GetNode(temporaryNode.associatedNode).OpenAtPoint(ref ctx, num2, temporaryNode.position, num3);
					}
					else if (temporaryNode.type == TemporaryNodeType.End)
					{
						pathHandler.LogVisitedNode(temporaryNode.associatedNode, num4, num3);
						this.OnFoundEndNode(ref ctx, num2, num4, num3);
						if (this.CompleteState == PathCompleteState.Complete)
						{
							return;
						}
					}
				}
				else
				{
					pathHandler.LogVisitedNode(num2, num4, num3);
					this.OnVisitNode(ref ctx, num2, num4, num3);
					pathHandler.GetNode(num2).Open(ref ctx, num2, num3);
				}
				if (num > 500)
				{
					if (DateTime.UtcNow.Ticks >= targetTick)
					{
						return;
					}
					num = 0;
					if (this.searchedNodes > 1000000)
					{
						throw new Exception("Probable infinite loop. Over 1,000,000 nodes searched");
					}
				}
				num++;
			}
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x0001D591 File Offset: 0x0001B791
		void IPathInternals.OnEnterPool()
		{
			this.OnEnterPool();
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x0001D599 File Offset: 0x0001B799
		void IPathInternals.Reset()
		{
			this.Reset();
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x0001D5A1 File Offset: 0x0001B7A1
		void IPathInternals.ReturnPath()
		{
			this.ReturnPath();
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0001D5A9 File Offset: 0x0001B7A9
		void IPathInternals.PrepareBase(PathHandler handler)
		{
			this.PrepareBase(handler);
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x0001D5B2 File Offset: 0x0001B7B2
		void IPathInternals.Prepare(ref Path.SearchContext ctx)
		{
			this.Prepare(ref ctx);
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x0001D5BB File Offset: 0x0001B7BB
		void IPathInternals.Cleanup(ref Path.SearchContext ctx)
		{
			this.Cleanup(ref ctx);
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x0001D5C4 File Offset: 0x0001B7C4
		void IPathInternals.CalculateStep(ref Path.SearchContext ctx, long targetTick)
		{
			this.CalculateStep(ref ctx, targetTick);
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x0001D5CE File Offset: 0x0001B7CE
		void IPathInternals.DebugString(StringBuilder builder, PathLog logMode)
		{
			this.DebugString(builder, logMode);
		}

		// Token: 0x040003CF RID: 975
		public OnPathDelegate callback;

		// Token: 0x040003D0 RID: 976
		public OnPathDelegate immediateCallback;

		// Token: 0x040003D2 RID: 978
		protected PathCompleteState completeState;

		// Token: 0x040003D4 RID: 980
		public List<GraphNode> path;

		// Token: 0x040003D5 RID: 981
		public List<Vector3> vectorPath;

		// Token: 0x040003D9 RID: 985
		protected bool hasBeenReset;

		// Token: 0x040003DA RID: 986
		public TraversalCosts traversalCosts;

		// Token: 0x040003DB RID: 987
		public TraversalConstraint traversalConstraint = TraversalConstraint.None;

		// Token: 0x040003DC RID: 988
		public DistanceMetric nearestNodeDistanceMetric;

		// Token: 0x040003DD RID: 989
		public Heuristic heuristic;

		// Token: 0x040003DE RID: 990
		public float heuristicScale = 1f;

		// Token: 0x040003E0 RID: 992
		public static readonly ProfilerMarker MarkerOpenCandidateConnectionsToEnd = new ProfilerMarker("OpenCandidateConnectionsToEnd");

		// Token: 0x040003E1 RID: 993
		public static readonly ProfilerMarker MarkerTrace = new ProfilerMarker("Trace");

		// Token: 0x040003E2 RID: 994
		private object[] claimed = new object[2];

		// Token: 0x040003E3 RID: 995
		private int claimCount;

		// Token: 0x040003E4 RID: 996
		private bool releasedNotSilent;

		// Token: 0x020000B6 RID: 182
		[BurstCompile]
		public struct SearchContext
		{
			// Token: 0x06000628 RID: 1576 RVA: 0x0001D624 File Offset: 0x0001B824
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public unsafe bool ShouldConsiderPathNode(uint pathNodeIndex)
			{
				PathNode pathNode = *this.pathHandler.pathNodes[pathNodeIndex];
				return pathNode.pathID != this.pathID || pathNode.heapIndex != ushort.MaxValue;
			}

			// Token: 0x06000629 RID: 1577 RVA: 0x0001D668 File Offset: 0x0001B868
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void SkipOverNode(ref Path.SearchContext ctx, uint pathNodeIndex, uint parentNodeIndex, uint fractionAlongEdge, uint hScore, uint gScore)
			{
				ref PathNode ptr = ref this.pathHandler.pathNodes[pathNodeIndex];
				ptr.pathID = this.pathID;
				ptr.heapIndex = ushort.MaxValue;
				ptr.parentIndex = parentNodeIndex;
				ptr.fractionAlongEdge = fractionAlongEdge;
				this.path.OnVisitNode(ref ctx, pathNodeIndex, hScore, gScore);
				this.pathHandler.LogVisitedNode(pathNodeIndex, hScore, gScore);
			}

			// Token: 0x0600062A RID: 1578 RVA: 0x0001D6CC File Offset: 0x0001B8CC
			public void OpenCandidateConnectionsToEndNode(Int3 position, uint parentPathNode, uint parentNodeIndex, uint parentG, float traversalCostFactor)
			{
				if (this.pathHandler.pathNodes[parentNodeIndex].flag1)
				{
					uint num = 0U;
					while ((ulong)num < (ulong)((long)this.pathHandler.numTemporaryNodes))
					{
						uint num2 = this.pathHandler.temporaryNodeStartIndex + num;
						ref TemporaryNode temporaryNode = ref this.pathHandler.GetTemporaryNode(num2);
						if (temporaryNode.type == TemporaryNodeType.End && temporaryNode.associatedNode == parentNodeIndex)
						{
							uint num3 = (uint)Math.Round((double)((position - temporaryNode.position).magnitude * traversalCostFactor));
							this.OpenCandidateConnection(parentPathNode, num2, parentG + num3, 0U, temporaryNode.position);
						}
						num += 1U;
					}
				}
			}

			// Token: 0x0600062B RID: 1579 RVA: 0x0001D76C File Offset: 0x0001B96C
			public void OpenCandidateConnection(uint parentPathNode, uint targetPathNode, uint targetG, uint fractionAlongEdge, Int3 targetNodePosition)
			{
				if (!this.ShouldConsiderPathNode(targetPathNode))
				{
					return;
				}
				Path.SearchContext.OpenCandidateParams openCandidateParams = new Path.SearchContext.OpenCandidateParams
				{
					pathID = this.pathID,
					parentPathNode = parentPathNode,
					targetPathNode = targetPathNode,
					targetNodeIndex = (this.pathHandler.IsTemporaryNode(targetPathNode) ? 0U : this.pathHandler.GetNode(targetPathNode).NodeIndex),
					targetG = targetG,
					fractionAlongEdge = fractionAlongEdge,
					targetNodePosition = (int3)targetNodePosition,
					pathNodes = this.pathHandler.pathNodes
				};
				Path.SearchContext.OpenCandidateConnectionBurst(ref openCandidateParams, ref this.pathHandler.heap, ref this.heuristicObjective);
			}

			// Token: 0x0600062C RID: 1580 RVA: 0x0001D81C File Offset: 0x0001BA1C
			[BurstCompile]
			public static void OpenCandidateConnectionBurst(ref Path.SearchContext.OpenCandidateParams pars, ref BinaryHeap heap, ref HeuristicObjective heuristicObjective)
			{
				Path.SearchContext.OpenCandidateConnectionBurst_000005EA$BurstDirectCall.Invoke(ref pars, ref heap, ref heuristicObjective);
			}

			// Token: 0x0600062D RID: 1581 RVA: 0x0001D828 File Offset: 0x0001BA28
			public readonly int3 FirstTemporaryEndNode()
			{
				uint num = 0U;
				while ((ulong)num < (ulong)((long)this.pathHandler.numTemporaryNodes))
				{
					uint num2 = this.pathHandler.temporaryNodeStartIndex + num;
					ref TemporaryNode temporaryNode = ref this.pathHandler.GetTemporaryNode(num2);
					if (temporaryNode.type == TemporaryNodeType.End)
					{
						return (int3)temporaryNode.position;
					}
					num += 1U;
				}
				throw new InvalidOperationException("There are no end nodes in the path");
			}

			// Token: 0x0600062E RID: 1582 RVA: 0x0001D888 File Offset: 0x0001BA88
			public readonly void TemporaryEndNodesBoundingBox(out int3 mn, out int3 mx)
			{
				mn = int.MaxValue;
				mx = int.MinValue;
				uint num = 0U;
				while ((ulong)num < (ulong)((long)this.pathHandler.numTemporaryNodes))
				{
					uint num2 = this.pathHandler.temporaryNodeStartIndex + num;
					ref TemporaryNode temporaryNode = ref this.pathHandler.GetTemporaryNode(num2);
					if (temporaryNode.type == TemporaryNodeType.End)
					{
						mn = math.min(mn, (int3)temporaryNode.position);
						mx = math.max(mx, (int3)temporaryNode.position);
					}
					num += 1U;
				}
			}

			// Token: 0x0600062F RID: 1583 RVA: 0x0001D92C File Offset: 0x0001BB2C
			public void MarkNodesAdjacentToTemporaryEndNodes()
			{
				UnsafeSpan<PathNode> pathNodes = this.pathHandler.pathNodes;
				uint num = 0U;
				while ((ulong)num < (ulong)((long)this.pathHandler.numTemporaryNodes))
				{
					uint num2 = this.pathHandler.temporaryNodeStartIndex + num;
					ref TemporaryNode temporaryNode = ref this.pathHandler.GetTemporaryNode(num2);
					if (temporaryNode.type == TemporaryNodeType.End)
					{
						GraphNode node = this.pathHandler.GetNode(temporaryNode.associatedNode);
						uint num3 = 0U;
						while ((ulong)num3 < (ulong)((long)node.PathNodeVariants))
						{
							pathNodes[temporaryNode.associatedNode + num3].flag1 = true;
							num3 += 1U;
						}
					}
					num += 1U;
				}
			}

			// Token: 0x06000630 RID: 1584 RVA: 0x0001D9C4 File Offset: 0x0001BBC4
			public void AddStartNodesToHeap()
			{
				UnsafeSpan<PathNode> pathNodes = this.pathHandler.pathNodes;
				uint num = 0U;
				while ((ulong)num < (ulong)((long)this.pathHandler.numTemporaryNodes))
				{
					uint num2 = this.pathHandler.temporaryNodeStartIndex + num;
					if (this.pathHandler.GetTemporaryNode(num2).type == TemporaryNodeType.Start)
					{
						this.pathHandler.heap.Add(pathNodes, num2, 0U, 0U);
					}
					num += 1U;
				}
			}

			// Token: 0x06000631 RID: 1585 RVA: 0x0001DA2C File Offset: 0x0001BC2C
			[BurstCompile]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			internal static void OpenCandidateConnectionBurst$BurstManaged(ref Path.SearchContext.OpenCandidateParams pars, ref BinaryHeap heap, ref HeuristicObjective heuristicObjective)
			{
				ushort num = pars.pathID;
				uint parentPathNode = pars.parentPathNode;
				uint targetPathNode = pars.targetPathNode;
				uint targetG = pars.targetG;
				uint fractionAlongEdge = pars.fractionAlongEdge;
				int3 targetNodePosition = pars.targetNodePosition;
				UnsafeSpan<PathNode> pathNodes = pars.pathNodes;
				ref PathNode ptr = ref pathNodes[targetPathNode];
				if (ptr.pathID != num)
				{
					ptr.fractionAlongEdge = fractionAlongEdge;
					ptr.pathID = num;
					ptr.parentIndex = parentPathNode;
					uint num2 = (uint)heuristicObjective.Calculate(targetNodePosition, pars.targetNodeIndex);
					heap.Add(pathNodes, targetPathNode, targetG, num2);
					return;
				}
				uint f = heap.GetF((int)ptr.heapIndex);
				uint h = heap.GetH((int)ptr.heapIndex);
				uint num3;
				if (ptr.fractionAlongEdge != fractionAlongEdge)
				{
					num3 = (uint)heuristicObjective.Calculate(targetNodePosition, pars.targetNodeIndex);
				}
				else
				{
					num3 = h;
				}
				if (targetG + num3 < f)
				{
					ptr.fractionAlongEdge = fractionAlongEdge;
					ptr.parentIndex = parentPathNode;
					heap.Add(pathNodes, targetPathNode, targetG, num3);
				}
			}

			// Token: 0x040003E5 RID: 997
			public Path path;

			// Token: 0x040003E6 RID: 998
			public PathHandler pathHandler;

			// Token: 0x040003E7 RID: 999
			public HeuristicObjective heuristicObjective;

			// Token: 0x040003E8 RID: 1000
			public TraversalConstraint traversalConstraint;

			// Token: 0x040003E9 RID: 1001
			public TraversalCosts traversalCosts;

			// Token: 0x040003EA RID: 1002
			public ushort pathID;

			// Token: 0x020000B7 RID: 183
			public struct OpenCandidateParams
			{
				// Token: 0x040003EB RID: 1003
				public UnsafeSpan<PathNode> pathNodes;

				// Token: 0x040003EC RID: 1004
				public uint parentPathNode;

				// Token: 0x040003ED RID: 1005
				public uint targetPathNode;

				// Token: 0x040003EE RID: 1006
				public uint targetNodeIndex;

				// Token: 0x040003EF RID: 1007
				public uint targetG;

				// Token: 0x040003F0 RID: 1008
				public uint fractionAlongEdge;

				// Token: 0x040003F1 RID: 1009
				public int3 targetNodePosition;

				// Token: 0x040003F2 RID: 1010
				public ushort pathID;
			}

			// Token: 0x020000B8 RID: 184
			// (Invoke) Token: 0x06000633 RID: 1587
			internal delegate void OpenCandidateConnectionBurst_000005EA$PostfixBurstDelegate(ref Path.SearchContext.OpenCandidateParams pars, ref BinaryHeap heap, ref HeuristicObjective heuristicObjective);

			// Token: 0x020000B9 RID: 185
			internal static class OpenCandidateConnectionBurst_000005EA$BurstDirectCall
			{
				// Token: 0x06000636 RID: 1590 RVA: 0x0001DB1B File Offset: 0x0001BD1B
				[BurstDiscard]
				private static void GetFunctionPointerDiscard(ref IntPtr A_0)
				{
					if (Path.SearchContext.OpenCandidateConnectionBurst_000005EA$BurstDirectCall.Pointer == 0)
					{
						Path.SearchContext.OpenCandidateConnectionBurst_000005EA$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(Path.SearchContext.OpenCandidateConnectionBurst_000005EA$BurstDirectCall.DeferredCompilation, methodof(Path.SearchContext.OpenCandidateConnectionBurst$BurstManaged(ref Path.SearchContext.OpenCandidateParams, ref BinaryHeap, ref HeuristicObjective)).MethodHandle, typeof(Path.SearchContext.OpenCandidateConnectionBurst_000005EA$PostfixBurstDelegate).TypeHandle);
					}
					A_0 = Path.SearchContext.OpenCandidateConnectionBurst_000005EA$BurstDirectCall.Pointer;
				}

				// Token: 0x06000637 RID: 1591 RVA: 0x0001DB48 File Offset: 0x0001BD48
				private static IntPtr GetFunctionPointer()
				{
					IntPtr intPtr = (IntPtr)0;
					Path.SearchContext.OpenCandidateConnectionBurst_000005EA$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
					return intPtr;
				}

				// Token: 0x06000638 RID: 1592 RVA: 0x0001DB60 File Offset: 0x0001BD60
				public static void Constructor()
				{
					Path.SearchContext.OpenCandidateConnectionBurst_000005EA$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(Path.SearchContext.OpenCandidateConnectionBurst(ref Path.SearchContext.OpenCandidateParams, ref BinaryHeap, ref HeuristicObjective)).MethodHandle);
				}

				// Token: 0x06000639 RID: 1593 RVA: 0x00003788 File Offset: 0x00001988
				public static void Initialize()
				{
				}

				// Token: 0x0600063A RID: 1594 RVA: 0x0001DB71 File Offset: 0x0001BD71
				// Note: this type is marked as 'beforefieldinit'.
				static OpenCandidateConnectionBurst_000005EA$BurstDirectCall()
				{
					Path.SearchContext.OpenCandidateConnectionBurst_000005EA$BurstDirectCall.Constructor();
				}

				// Token: 0x0600063B RID: 1595 RVA: 0x0001DB78 File Offset: 0x0001BD78
				public static void Invoke(ref Path.SearchContext.OpenCandidateParams pars, ref BinaryHeap heap, ref HeuristicObjective heuristicObjective)
				{
					if (BurstCompiler.IsEnabled)
					{
						IntPtr functionPointer = Path.SearchContext.OpenCandidateConnectionBurst_000005EA$BurstDirectCall.GetFunctionPointer();
						if (functionPointer != 0)
						{
							calli(System.Void(Pathfinding.Path/SearchContext/OpenCandidateParams&,Pathfinding.BinaryHeap&,Pathfinding.HeuristicObjective&), ref pars, ref heap, ref heuristicObjective, functionPointer);
							return;
						}
					}
					Path.SearchContext.OpenCandidateConnectionBurst$BurstManaged(ref pars, ref heap, ref heuristicObjective);
				}

				// Token: 0x040003F3 RID: 1011
				private static IntPtr Pointer;

				// Token: 0x040003F4 RID: 1012
				private static IntPtr DeferredCompilation;
			}
		}
	}
}
