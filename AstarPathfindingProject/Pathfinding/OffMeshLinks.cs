using System;
using System.Collections.Generic;
using Pathfinding.Collections;
using Pathfinding.Pooling;
using Unity.Collections;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000089 RID: 137
	public class OffMeshLinks
	{
		// Token: 0x060004BA RID: 1210 RVA: 0x000181F8 File Offset: 0x000163F8
		public OffMeshLinks(AstarPath astar)
		{
			this.astar = astar;
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00018220 File Offset: 0x00016420
		public List<NavGraph> ConnectedGraphs(OffMeshLinks.OffMeshLinkSource link)
		{
			List<NavGraph> list = ListPool<NavGraph>.Claim();
			if (link.status != OffMeshLinks.OffMeshLinkStatus.Active)
			{
				return list;
			}
			OffMeshLinks.OffMeshLinkConcrete concrete = this.tree[link.treeKey].concrete;
			for (int i = 0; i < concrete.startNodes.Length; i++)
			{
				NavGraph graph = concrete.startNodes[i].Graph;
				if (!list.Contains(graph))
				{
					list.Add(graph);
				}
			}
			for (int j = 0; j < concrete.endNodes.Length; j++)
			{
				NavGraph graph2 = concrete.endNodes[j].Graph;
				if (!list.Contains(graph2))
				{
					list.Add(graph2);
				}
			}
			return list;
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x000182C0 File Offset: 0x000164C0
		public void Add(OffMeshLinks.OffMeshLinkSource link)
		{
			if (link == null)
			{
				throw new ArgumentNullException("link");
			}
			if (link.status != OffMeshLinks.OffMeshLinkStatus.Inactive)
			{
				throw new ArgumentException("Link is already added");
			}
			this.pendingAdd.Add(link);
			link.status = OffMeshLinks.OffMeshLinkStatus.Pending;
			this.ScheduleUpdate();
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00018300 File Offset: 0x00016500
		internal void OnDisable()
		{
			List<OffMeshLinks.OffMeshLinkCombined> list = new List<OffMeshLinks.OffMeshLinkCombined>();
			this.tree.Query(new Bounds(Vector3.zero, Vector3.positiveInfinity), list);
			for (int i = 0; i < list.Count; i++)
			{
				list[i].source.status = OffMeshLinks.OffMeshLinkStatus.Inactive;
				list[i].source.treeKey = default(AABBTree<OffMeshLinks.OffMeshLinkCombined>.Key);
			}
			this.tree.Clear();
			for (int j = 0; j < this.pendingAdd.Count; j++)
			{
				this.pendingAdd[j].status = OffMeshLinks.OffMeshLinkStatus.Inactive;
				this.pendingAdd[j].treeKey = default(AABBTree<OffMeshLinks.OffMeshLinkCombined>.Key);
			}
			this.pendingAdd.Clear();
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x000183C0 File Offset: 0x000165C0
		public void Remove(OffMeshLinks.OffMeshLinkSource link)
		{
			if (link == null)
			{
				throw new ArgumentNullException("link");
			}
			if (link.status == OffMeshLinks.OffMeshLinkStatus.Inactive || (link.status & OffMeshLinks.OffMeshLinkStatus.PendingRemoval) != (OffMeshLinks.OffMeshLinkStatus)0)
			{
				return;
			}
			if (link.status == OffMeshLinks.OffMeshLinkStatus.Pending)
			{
				link.status = OffMeshLinks.OffMeshLinkStatus.Inactive;
				this.pendingAdd.Remove(link);
			}
			else
			{
				link.status |= OffMeshLinks.OffMeshLinkStatus.Pending | OffMeshLinks.OffMeshLinkStatus.PendingRemoval;
				this.tree.Tag(link.treeKey);
			}
			this.ScheduleUpdate();
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00018438 File Offset: 0x00016638
		private bool ClampSegment(OffMeshLinks.Anchor anchor, GraphMask graphMask, float maxSnappingDistance, out OffMeshLinks.Anchor result, List<GraphNode> nodes)
		{
			NearestNodeConstraint walkable = NearestNodeConstraint.Walkable;
			walkable.distanceMetric = DistanceMetric.Euclidean;
			walkable.graphMask = graphMask;
			NNInfo nninfo = this.astar.GetNearest(0.5f * (anchor.point1 + anchor.point2), walkable);
			if (nninfo.distanceCostSqr > maxSnappingDistance * maxSnappingDistance)
			{
				nninfo = default(NNInfo);
			}
			if (nninfo.node == null)
			{
				result = default(OffMeshLinks.Anchor);
				return false;
			}
			if (anchor.width > 0f)
			{
				IRaycastableGraph raycastableGraph = nninfo.node.Graph as IRaycastableGraph;
				if (raycastableGraph != null)
				{
					Vector3 vector = 0.5f * (anchor.point2 - anchor.point1);
					GraphHitInfo graphHitInfo;
					raycastableGraph.Linecast(nninfo.position, nninfo.position - vector, nninfo.node, out graphHitInfo, nodes);
					GraphHitInfo graphHitInfo2;
					raycastableGraph.Linecast(nninfo.position, nninfo.position + vector, nninfo.node, out graphHitInfo2, nodes);
					result = new OffMeshLinks.Anchor
					{
						center = (graphHitInfo.point + graphHitInfo2.point) * 0.5f,
						rotation = anchor.rotation,
						width = (graphHitInfo.point - graphHitInfo2.point).magnitude
					};
					nodes.Sort((GraphNode a, GraphNode b) => a.NodeIndex.CompareTo(b.NodeIndex));
					for (int i = nodes.Count - 1; i >= 0; i--)
					{
						GraphNode graphNode = nodes[i];
						for (int j = i - 1; j >= 0; j--)
						{
							if (nodes[j] == graphNode)
							{
								nodes.RemoveAtSwapBack<GraphNode>(i);
								break;
							}
						}
					}
					return true;
				}
			}
			result = new OffMeshLinks.Anchor
			{
				center = nninfo.position,
				rotation = anchor.rotation,
				width = 0f
			};
			nodes.Add(nninfo.node);
			return true;
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00018659 File Offset: 0x00016859
		public void DirtyBounds(Bounds bounds)
		{
			this.tree.Tag(bounds);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00018667 File Offset: 0x00016867
		public void Dirty(OffMeshLinks.OffMeshLinkSource link)
		{
			this.DirtyNoSchedule(link);
			this.ScheduleUpdate();
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00018676 File Offset: 0x00016876
		internal void DirtyNoSchedule(OffMeshLinks.OffMeshLinkSource link)
		{
			this.tree.Tag(link.treeKey);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0001868C File Offset: 0x0001688C
		private void ScheduleUpdate()
		{
			if (!this.updateScheduled && !this.astar.isScanning && !this.astar.IsAnyWorkItemInProgress)
			{
				this.updateScheduled = true;
				this.astar.AddWorkItem(delegate
				{
				});
			}
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x000186EC File Offset: 0x000168EC
		public OffMeshLinks.OffMeshLinkTracer GetNearest(Vector3 point, float maxDistance)
		{
			if (maxDistance < 0f)
			{
				return default(OffMeshLinks.OffMeshLinkTracer);
			}
			if (!float.IsFinite(maxDistance))
			{
				throw new ArgumentOutOfRangeException("maxDistance");
			}
			List<OffMeshLinks.OffMeshLinkCombined> list = ListPool<OffMeshLinks.OffMeshLinkCombined>.Claim();
			this.tree.Query(new Bounds(point, new Vector3(2f * maxDistance, 2f * maxDistance, 2f * maxDistance)), list);
			OffMeshLinks.OffMeshLinkConcrete offMeshLinkConcrete = null;
			bool flag = false;
			float num = maxDistance * maxDistance;
			for (int i = 0; i < list.Count; i++)
			{
				OffMeshLinks.OffMeshLinkConcrete concrete = list[i].concrete;
				float num2 = VectorMath.SqrDistancePointSegment(concrete.start.point1, concrete.start.point2, point);
				if (num2 < num)
				{
					num = num2;
					offMeshLinkConcrete = concrete;
					flag = false;
				}
				num2 = VectorMath.SqrDistancePointSegment(concrete.end.point1, concrete.end.point2, point);
				if (num2 < num)
				{
					num = num2;
					offMeshLinkConcrete = concrete;
					flag = true;
				}
			}
			ListPool<OffMeshLinks.OffMeshLinkCombined>.Release(ref list);
			if (offMeshLinkConcrete == null)
			{
				return default(OffMeshLinks.OffMeshLinkTracer);
			}
			return new OffMeshLinks.OffMeshLinkTracer(offMeshLinkConcrete, flag);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x000187F4 File Offset: 0x000169F4
		internal void Refresh()
		{
			this.updateScheduled = false;
			List<OffMeshLinks.OffMeshLinkCombined> list = ListPool<OffMeshLinks.OffMeshLinkCombined>.Claim();
			this.tree.QueryTagged(list, true);
			for (int i = 0; i < this.pendingAdd.Count; i++)
			{
				OffMeshLinks.OffMeshLinkSource offMeshLinkSource = this.pendingAdd[i];
				OffMeshLinks.OffMeshLinkCombined offMeshLinkCombined = new OffMeshLinks.OffMeshLinkCombined
				{
					source = offMeshLinkSource,
					concrete = null
				};
				offMeshLinkSource.treeKey = this.tree.Add(offMeshLinkSource.bounds, offMeshLinkCombined);
				list.Add(offMeshLinkCombined);
			}
			this.pendingAdd.Clear();
			List<GraphNode> list2 = ListPool<GraphNode>.Claim();
			List<GraphNode> list3 = ListPool<GraphNode>.Claim();
			for (int j = 0; j < list.Count; j++)
			{
				for (int k = 0; k < j; k++)
				{
					if (list[j].source == list[k].source)
					{
						throw new Exception("Duplicate link");
					}
				}
				OffMeshLinks.OffMeshLinkSource source = list[j].source;
				OffMeshLinks.OffMeshLinkCombined offMeshLinkCombined2 = this.tree[source.treeKey];
				OffMeshLinks.OffMeshLinkConcrete concrete = offMeshLinkCombined2.concrete;
				if ((source.status & OffMeshLinks.OffMeshLinkStatus.PendingRemoval) != (OffMeshLinks.OffMeshLinkStatus)0)
				{
					if (concrete != null)
					{
						concrete.Disconnect();
						offMeshLinkCombined2.concrete = null;
					}
					this.tree.Remove(source.treeKey);
					source.treeKey = default(AABBTree<OffMeshLinks.OffMeshLinkCombined>.Key);
					source.status = OffMeshLinks.OffMeshLinkStatus.Inactive;
				}
				else
				{
					list2.Clear();
					OffMeshLinks.Anchor anchor;
					if (!this.ClampSegment(source.start, source.graphMask, source.maxSnappingDistance, out anchor, list2))
					{
						if (concrete != null)
						{
							concrete.Disconnect();
							offMeshLinkCombined2.concrete = null;
						}
						source.status = OffMeshLinks.OffMeshLinkStatus.FailedToConnectStart;
					}
					else
					{
						list3.Clear();
						OffMeshLinks.Anchor anchor2;
						if (!this.ClampSegment(source.end, source.graphMask, source.maxSnappingDistance, out anchor2, list3))
						{
							if (concrete != null)
							{
								concrete.Disconnect();
								offMeshLinkCombined2.concrete = null;
							}
							source.status = OffMeshLinks.OffMeshLinkStatus.FailedToConnectEnd;
						}
						else
						{
							OffMeshLinks.OffMeshLinkConcrete offMeshLinkConcrete = new OffMeshLinks.OffMeshLinkConcrete
							{
								start = anchor,
								end = anchor2,
								startNodes = list2.ToArrayFromPool<GraphNode>(),
								endNodes = list3.ToArrayFromPool<GraphNode>(),
								source = source,
								directionality = source.directionality,
								tag = source.tag,
								costFactor = source.costFactor
							};
							if (concrete != null && !concrete.staleConnections && concrete.Equivalent(offMeshLinkConcrete))
							{
								source.status &= ~OffMeshLinks.OffMeshLinkStatus.Pending;
							}
							else
							{
								if (concrete != null)
								{
									concrete.Disconnect();
									ArrayPool<GraphNode>.Release(ref concrete.startNodes, false);
									ArrayPool<GraphNode>.Release(ref concrete.endNodes, false);
								}
								if (this.astar.data.linkGraph == null)
								{
									this.astar.data.AddGraph<LinkGraph>().name = "Internal graph for off-mesh links";
								}
								offMeshLinkConcrete.Connect(this.astar.data.linkGraph, source);
								offMeshLinkCombined2.concrete = offMeshLinkConcrete;
								source.status = OffMeshLinks.OffMeshLinkStatus.Active;
							}
						}
					}
				}
			}
			ListPool<OffMeshLinks.OffMeshLinkCombined>.Release(ref list);
			ListPool<GraphNode>.Release(ref list2);
			ListPool<GraphNode>.Release(ref list3);
		}

		// Token: 0x040002F7 RID: 759
		private AABBTree<OffMeshLinks.OffMeshLinkCombined> tree = new AABBTree<OffMeshLinks.OffMeshLinkCombined>();

		// Token: 0x040002F8 RID: 760
		private List<OffMeshLinks.OffMeshLinkSource> pendingAdd = new List<OffMeshLinks.OffMeshLinkSource>();

		// Token: 0x040002F9 RID: 761
		private bool updateScheduled;

		// Token: 0x040002FA RID: 762
		private AstarPath astar;

		// Token: 0x0200008A RID: 138
		public struct Anchor
		{
			// Token: 0x170000F5 RID: 245
			// (get) Token: 0x060004C6 RID: 1222 RVA: 0x00018B00 File Offset: 0x00016D00
			public readonly Vector3 point1
			{
				get
				{
					return this.center + this.rotation * new Vector3(-0.5f * this.width, 0f, 0f);
				}
			}

			// Token: 0x170000F6 RID: 246
			// (get) Token: 0x060004C7 RID: 1223 RVA: 0x00018B33 File Offset: 0x00016D33
			public readonly Vector3 point2
			{
				get
				{
					return this.center + this.rotation * new Vector3(0.5f * this.width, 0f, 0f);
				}
			}

			// Token: 0x060004C8 RID: 1224 RVA: 0x00018B66 File Offset: 0x00016D66
			public static bool operator ==(OffMeshLinks.Anchor a, OffMeshLinks.Anchor b)
			{
				return a.center == b.center && a.rotation == b.rotation && a.width == b.width;
			}

			// Token: 0x060004C9 RID: 1225 RVA: 0x00018B9E File Offset: 0x00016D9E
			public static bool operator !=(OffMeshLinks.Anchor a, OffMeshLinks.Anchor b)
			{
				return a.center != b.center || a.rotation != b.rotation || a.width != b.width;
			}

			// Token: 0x060004CA RID: 1226 RVA: 0x00018BD9 File Offset: 0x00016DD9
			public override bool Equals(object obj)
			{
				return obj is OffMeshLinks.Anchor && this == (OffMeshLinks.Anchor)obj;
			}

			// Token: 0x060004CB RID: 1227 RVA: 0x00018BF6 File Offset: 0x00016DF6
			public override int GetHashCode()
			{
				return (((this.center.GetHashCode() * 23) ^ this.rotation.GetHashCode()) * 23) ^ this.width.GetHashCode();
			}

			// Token: 0x040002FB RID: 763
			public Vector3 center;

			// Token: 0x040002FC RID: 764
			public Quaternion rotation;

			// Token: 0x040002FD RID: 765
			public float width;
		}

		// Token: 0x0200008B RID: 139
		public enum Directionality
		{
			// Token: 0x040002FF RID: 767
			OneWay,
			// Token: 0x04000300 RID: 768
			TwoWay
		}

		// Token: 0x0200008C RID: 140
		[Flags]
		public enum OffMeshLinkStatus
		{
			// Token: 0x04000302 RID: 770
			Inactive = 1,
			// Token: 0x04000303 RID: 771
			Pending = 2,
			// Token: 0x04000304 RID: 772
			Active = 4,
			// Token: 0x04000305 RID: 773
			FailedToConnectStart = 9,
			// Token: 0x04000306 RID: 774
			FailedToConnectEnd = 17,
			// Token: 0x04000307 RID: 775
			PendingRemoval = 32
		}

		// Token: 0x0200008D RID: 141
		public readonly struct OffMeshLinkTracer
		{
			// Token: 0x060004CC RID: 1228 RVA: 0x00018C30 File Offset: 0x00016E30
			public OffMeshLinkTracer(OffMeshLinks.OffMeshLinkConcrete link, bool reversed)
			{
				this.link = link;
				this.relativeStart = (reversed ? link.end.center : link.start.center);
				this.relativeEnd = (reversed ? link.start.center : link.end.center);
				this.isReverse = reversed;
			}

			// Token: 0x060004CD RID: 1229 RVA: 0x00018C8D File Offset: 0x00016E8D
			public OffMeshLinkTracer(OffMeshLinks.OffMeshLinkConcrete link, Vector3 relativeStart, Vector3 relativeEnd, bool isReverse)
			{
				this.link = link;
				this.relativeStart = relativeStart;
				this.relativeEnd = relativeEnd;
				this.isReverse = isReverse;
			}

			// Token: 0x170000F7 RID: 247
			// (get) Token: 0x060004CE RID: 1230 RVA: 0x00018CAC File Offset: 0x00016EAC
			public Component component
			{
				get
				{
					return this.link.component;
				}
			}

			// Token: 0x170000F8 RID: 248
			// (get) Token: 0x060004CF RID: 1231 RVA: 0x00018CB9 File Offset: 0x00016EB9
			public GameObject gameObject
			{
				get
				{
					return this.link.gameObject;
				}
			}

			// Token: 0x04000308 RID: 776
			public readonly OffMeshLinks.OffMeshLinkConcrete link;

			// Token: 0x04000309 RID: 777
			public readonly Vector3 relativeStart;

			// Token: 0x0400030A RID: 778
			public readonly Vector3 relativeEnd;

			// Token: 0x0400030B RID: 779
			public readonly bool isReverse;
		}

		// Token: 0x0200008E RID: 142
		public class OffMeshLinkSource
		{
			// Token: 0x170000F9 RID: 249
			// (get) Token: 0x060004D0 RID: 1232 RVA: 0x00018CC6 File Offset: 0x00016EC6
			public GameObject gameObject
			{
				get
				{
					if (!(this.component != null))
					{
						return null;
					}
					return this.component.gameObject;
				}
			}

			// Token: 0x170000FA RID: 250
			// (get) Token: 0x060004D1 RID: 1233 RVA: 0x00018CE3 File Offset: 0x00016EE3
			// (set) Token: 0x060004D2 RID: 1234 RVA: 0x00018CEB File Offset: 0x00016EEB
			public OffMeshLinks.OffMeshLinkStatus status { get; internal set; } = OffMeshLinks.OffMeshLinkStatus.Inactive;

			// Token: 0x170000FB RID: 251
			// (get) Token: 0x060004D3 RID: 1235 RVA: 0x00018CF4 File Offset: 0x00016EF4
			public Bounds bounds
			{
				get
				{
					Bounds bounds = default(Bounds);
					bounds.SetMinMax(this.start.point1, this.start.point2);
					bounds.Encapsulate(this.end.point1);
					bounds.Encapsulate(this.end.point2);
					bounds.Expand(this.maxSnappingDistance * 2f);
					return bounds;
				}
			}

			// Token: 0x0400030C RID: 780
			public OffMeshLinks.Anchor start;

			// Token: 0x0400030D RID: 781
			public OffMeshLinks.Anchor end;

			// Token: 0x0400030E RID: 782
			public OffMeshLinks.Directionality directionality;

			// Token: 0x0400030F RID: 783
			public PathfindingTag tag;

			// Token: 0x04000310 RID: 784
			public float costFactor;

			// Token: 0x04000311 RID: 785
			public float maxSnappingDistance;

			// Token: 0x04000312 RID: 786
			public GraphMask graphMask;

			// Token: 0x04000313 RID: 787
			public IOffMeshLinkHandler handler;

			// Token: 0x04000314 RID: 788
			public Component component;

			// Token: 0x04000315 RID: 789
			internal AABBTree<OffMeshLinks.OffMeshLinkCombined>.Key treeKey;
		}

		// Token: 0x0200008F RID: 143
		internal class OffMeshLinkCombined
		{
			// Token: 0x04000317 RID: 791
			public OffMeshLinks.OffMeshLinkSource source;

			// Token: 0x04000318 RID: 792
			public OffMeshLinks.OffMeshLinkConcrete concrete;
		}

		// Token: 0x02000090 RID: 144
		public class OffMeshLinkConcrete
		{
			// Token: 0x170000FC RID: 252
			// (get) Token: 0x060004D6 RID: 1238 RVA: 0x00018D6D File Offset: 0x00016F6D
			public IOffMeshLinkHandler handler
			{
				get
				{
					return this.source.handler;
				}
			}

			// Token: 0x170000FD RID: 253
			// (get) Token: 0x060004D7 RID: 1239 RVA: 0x00018D7A File Offset: 0x00016F7A
			public Component component
			{
				get
				{
					return this.source.component;
				}
			}

			// Token: 0x170000FE RID: 254
			// (get) Token: 0x060004D8 RID: 1240 RVA: 0x00018D87 File Offset: 0x00016F87
			public GameObject gameObject
			{
				get
				{
					if (!(this.source.component != null))
					{
						return null;
					}
					return this.source.component.gameObject;
				}
			}

			// Token: 0x060004D9 RID: 1241 RVA: 0x00018DB0 File Offset: 0x00016FB0
			public bool Equivalent(OffMeshLinks.OffMeshLinkConcrete other)
			{
				if (this.start != other.start)
				{
					return false;
				}
				if (this.end != other.end)
				{
					return false;
				}
				if (this.startNodes.Length != other.startNodes.Length || this.endNodes.Length != other.endNodes.Length)
				{
					return false;
				}
				if (this.directionality != other.directionality || this.tag != other.tag || this.costFactor != other.costFactor)
				{
					return false;
				}
				for (int i = 0; i < this.startNodes.Length; i++)
				{
					if (this.startNodes[i] != other.startNodes[i])
					{
						return false;
					}
				}
				for (int j = 0; j < this.endNodes.Length; j++)
				{
					if (this.endNodes[j] != other.endNodes[j])
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x060004DA RID: 1242 RVA: 0x00018E94 File Offset: 0x00017094
			public void Disconnect()
			{
				if (this.startLinkNode != null && !this.startLinkNode.Destroyed)
				{
					LinkGraph linkGraph = this.startLinkNode.Graph as LinkGraph;
					linkGraph.RemoveNode(this.startLinkNode);
					linkGraph.RemoveNode(this.endLinkNode);
				}
				this.startLinkNode = null;
				this.endLinkNode = null;
			}

			// Token: 0x060004DB RID: 1243 RVA: 0x00018EEC File Offset: 0x000170EC
			public void Connect(LinkGraph linkGraph, OffMeshLinks.OffMeshLinkSource source)
			{
				this.startLinkNode = linkGraph.AddNode();
				this.startLinkNode.linkSource = source;
				this.startLinkNode.linkConcrete = this;
				this.startLinkNode.position = (Int3)this.start.center;
				this.startLinkNode.Tag = this.tag;
				this.endLinkNode = linkGraph.AddNode();
				this.endLinkNode.position = (Int3)this.end.center;
				this.endLinkNode.linkSource = source;
				this.endLinkNode.linkConcrete = this;
				this.endLinkNode.Tag = this.tag;
				for (int i = 0; i < this.startNodes.Length; i++)
				{
					float magnitude = (VectorMath.ClosestPointOnSegment(this.start.point1, this.start.point2, (Vector3)this.startNodes[i].position) - (Vector3)this.startNodes[i].position).magnitude;
					uint num = (uint)(1000f * magnitude);
					GraphNode.Connect(this.startNodes[i], this.startLinkNode, num, this.directionality);
				}
				for (int j = 0; j < this.endNodes.Length; j++)
				{
					float magnitude2 = (VectorMath.ClosestPointOnSegment(this.end.point1, this.end.point2, (Vector3)this.endNodes[j].position) - (Vector3)this.endNodes[j].position).magnitude;
					uint num2 = (uint)(1000f * magnitude2);
					GraphNode.Connect(this.endLinkNode, this.endNodes[j], num2, this.directionality);
				}
				uint num3 = (uint)(1000f * this.costFactor * (this.end.center - this.start.center).magnitude);
				GraphNode.Connect(this.startLinkNode, this.endLinkNode, num3, this.directionality);
				this.staleConnections = false;
			}

			// Token: 0x060004DC RID: 1244 RVA: 0x0001910B File Offset: 0x0001730B
			public OffMeshLinks.OffMeshLinkTracer GetTracer(LinkNode firstNode)
			{
				return new OffMeshLinks.OffMeshLinkTracer(this, firstNode == this.endLinkNode);
			}

			// Token: 0x04000319 RID: 793
			public OffMeshLinks.Anchor start;

			// Token: 0x0400031A RID: 794
			public OffMeshLinks.Anchor end;

			// Token: 0x0400031B RID: 795
			public GraphNode[] startNodes;

			// Token: 0x0400031C RID: 796
			public GraphNode[] endNodes;

			// Token: 0x0400031D RID: 797
			public LinkNode startLinkNode;

			// Token: 0x0400031E RID: 798
			public LinkNode endLinkNode;

			// Token: 0x0400031F RID: 799
			public OffMeshLinks.Directionality directionality;

			// Token: 0x04000320 RID: 800
			public PathfindingTag tag;

			// Token: 0x04000321 RID: 801
			public float costFactor;

			// Token: 0x04000322 RID: 802
			internal bool staleConnections;

			// Token: 0x04000323 RID: 803
			internal OffMeshLinks.OffMeshLinkSource source;
		}
	}
}
