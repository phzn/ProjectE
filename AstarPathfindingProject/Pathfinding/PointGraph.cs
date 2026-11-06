using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding.Pooling;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000FC RID: 252
	[JsonOptIn]
	[Preserve]
	public class PointGraph : NavGraph, IUpdatableGraph
	{
		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060008E4 RID: 2276 RVA: 0x0002DC63 File Offset: 0x0002BE63
		// (set) Token: 0x060008E5 RID: 2277 RVA: 0x0002DC6B File Offset: 0x0002BE6B
		public int nodeCount { get; protected set; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060008E6 RID: 2278 RVA: 0x0002DC74 File Offset: 0x0002BE74
		public override bool isScanned
		{
			get
			{
				return this.nodes != null;
			}
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0002DC7F File Offset: 0x0002BE7F
		public override int CountNodes()
		{
			return this.nodeCount;
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0002DC88 File Offset: 0x0002BE88
		public override void GetNodes(Action<GraphNode> action)
		{
			if (this.nodes == null)
			{
				return;
			}
			int nodeCount = this.nodeCount;
			for (int i = 0; i < nodeCount; i++)
			{
				action(this.nodes[i]);
			}
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0002DCC0 File Offset: 0x0002BEC0
		public override void GetNodes<T>(GraphNode.NodeActionWithData<T> action, ref T data)
		{
			if (this.nodes == null)
			{
				return;
			}
			int nodeCount = this.nodeCount;
			for (int i = 0; i < nodeCount; i++)
			{
				action(this.nodes[i], ref data);
			}
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0002DCF8 File Offset: 0x0002BEF8
		public override NNInfo GetNearest(Vector3 position, ref NearestNodeConstraint constraint)
		{
			if (this.nodes == null)
			{
				return NNInfo.Empty;
			}
			Int3 @int = (Int3)position;
			if (this.lookupTree != null != this.optimizeForSparseGraph)
			{
				Debug.LogWarning("Lookup tree is not in the correct state. Have you changed PointGraph.optimizeForSparseGraph without calling RebuildNodeLookup?");
			}
			float num = constraint.maxDistanceSqrOrDefault(this.active);
			if (this.lookupTree != null)
			{
				if (this.nearestNodeDistanceMode == PointGraph.NodeDistanceMode.Node)
				{
					GraphNode nearest = this.lookupTree.GetNearest(@int, ref constraint, ref num);
					if (nearest != null)
					{
						return new NNInfo(nearest, (Vector3)nearest.position, num);
					}
					return NNInfo.Empty;
				}
				else
				{
					GraphNode nearestConnection = this.lookupTree.GetNearestConnection(@int, ref constraint, this.maximumConnectionLength);
					if (nearestConnection != null)
					{
						return this.FindClosestConnectionPoint(nearestConnection as PointNode, position, num);
					}
					return NNInfo.Empty;
				}
			}
			else
			{
				PointNode pointNode = null;
				long num2 = AstarMath.SaturatingConvertFloatToLong(num * 1000f * 1000f);
				for (int i = 0; i < this.nodeCount; i++)
				{
					PointNode pointNode2 = this.nodes[i];
					long sqrMagnitudeLong = (@int - pointNode2.position).sqrMagnitudeLong;
					if (sqrMagnitudeLong < num2 && constraint.Suitable(pointNode2))
					{
						num2 = sqrMagnitudeLong;
						pointNode = pointNode2;
					}
				}
				if (1.0000001E-06f * (float)num2 >= num || pointNode == null)
				{
					return NNInfo.Empty;
				}
				return new NNInfo(pointNode, (Vector3)pointNode.position, 1.0000001E-06f * (float)num2);
			}
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0002DE40 File Offset: 0x0002C040
		private NNInfo FindClosestConnectionPoint(PointNode node, Vector3 position, float maxDistanceSqr)
		{
			Vector3 vector = (Vector3)node.position;
			Connection[] connections = node.connections;
			Vector3 vector2 = (Vector3)node.position;
			if (connections != null)
			{
				for (int i = 0; i < connections.Length; i++)
				{
					Vector3 vector3 = ((Vector3)connections[i].node.position + vector2) * 0.5f;
					Vector3 vector4 = VectorMath.ClosestPointOnSegment(vector2, vector3, position);
					float sqrMagnitude = (vector4 - position).sqrMagnitude;
					if (sqrMagnitude < maxDistanceSqr)
					{
						maxDistanceSqr = sqrMagnitude;
						vector = vector4;
					}
				}
			}
			return new NNInfo(node, vector, maxDistanceSqr);
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x0002DED8 File Offset: 0x0002C0D8
		public override NNInfo RandomPointOnSurface(NearestNodeConstraint constraint, bool highQuality = true)
		{
			if (!this.isScanned || this.nodes.Length == 0)
			{
				return NNInfo.Empty;
			}
			for (int i = 0; i < 10; i++)
			{
				PointNode pointNode = this.nodes[Random.Range(0, this.nodes.Length)];
				if (pointNode != null && constraint.Suitable(pointNode))
				{
					return new NNInfo(pointNode, pointNode.RandomPointOnSurface(), 0f);
				}
			}
			return base.RandomPointOnSurface(constraint, highQuality);
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x0002DF46 File Offset: 0x0002C146
		public PointNode AddNode(Int3 position)
		{
			return this.AddNode<PointNode>(new PointNode(this.active), position);
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x0002DF5C File Offset: 0x0002C15C
		public T AddNode<T>(T node, Int3 position) where T : PointNode
		{
			base.AssertSafeToUpdateGraph();
			if (this.nodes == null || this.nodeCount == this.nodes.Length)
			{
				PointNode[] array = new PointNode[(this.nodes != null) ? Math.Max(this.nodes.Length + 4, this.nodes.Length * 2) : 4];
				if (this.nodes != null)
				{
					this.nodes.CopyTo(array, 0);
				}
				this.nodes = array;
				this.RebuildNodeLookup();
			}
			node.position = position;
			node.GraphIndex = this.graphIndex;
			node.Walkable = true;
			this.nodes[this.nodeCount] = node;
			int nodeCount = this.nodeCount;
			this.nodeCount = nodeCount + 1;
			if (this.lookupTree != null)
			{
				this.lookupTree.Add(node);
			}
			return node;
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x0002E03C File Offset: 0x0002C23C
		public void RemoveNode(PointNode node)
		{
			base.AssertSafeToUpdateGraph();
			if (node.Destroyed)
			{
				throw new ArgumentException("The node has already been destroyed");
			}
			if (node.GraphIndex != this.graphIndex)
			{
				throw new ArgumentException("The node does not belong to this graph");
			}
			if (!this.isScanned)
			{
				throw new InvalidOperationException("Graph contains no nodes");
			}
			int num = Array.IndexOf<PointNode>(this.nodes, node);
			if (num == -1)
			{
				throw new ArgumentException("The node is not in the graph");
			}
			int nodeCount = this.nodeCount;
			this.nodeCount = nodeCount - 1;
			this.nodes[num] = this.nodes[this.nodeCount];
			this.nodes[this.nodeCount] = null;
			node.Destroy();
			if (this.lookupTree != null)
			{
				this.lookupTree.Remove(node);
			}
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0002E0F8 File Offset: 0x0002C2F8
		public void Clear()
		{
			base.AssertSafeToUpdateGraph();
			if (this.nodes == null)
			{
				return;
			}
			for (int i = 0; i < this.nodeCount; i++)
			{
				this.nodes[i].Destroy();
			}
			this.nodeCount = 0;
			this.nodes = new PointNode[0];
			this.RebuildNodeLookup();
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x0002E14C File Offset: 0x0002C34C
		protected static int CountChildren(Transform tr)
		{
			int num = 0;
			foreach (object obj in tr)
			{
				Transform transform = (Transform)obj;
				num++;
				num += PointGraph.CountChildren(transform);
			}
			return num;
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x0002E1AC File Offset: 0x0002C3AC
		protected static void AddChildren(PointNode[] nodes, ref int c, Transform tr)
		{
			foreach (object obj in tr)
			{
				Transform transform = (Transform)obj;
				nodes[c].position = (Int3)transform.position;
				nodes[c].Walkable = true;
				nodes[c].gameObject = transform.gameObject;
				c++;
				PointGraph.AddChildren(nodes, ref c, transform);
			}
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0002E234 File Offset: 0x0002C434
		public void RebuildNodeLookup()
		{
			this.lookupTree = PointGraph.BuildNodeLookup(this.nodes, this.nodeCount, this.optimizeForSparseGraph);
			this.RebuildConnectionDistanceLookup();
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0002E25C File Offset: 0x0002C45C
		private static PointKDTree BuildNodeLookup(PointNode[] nodes, int nodeCount, bool optimizeForSparseGraph)
		{
			if (optimizeForSparseGraph && nodes != null)
			{
				PointKDTree pointKDTree = new PointKDTree();
				pointKDTree.Rebuild(nodes, 0, nodeCount);
				return pointKDTree;
			}
			return null;
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0002E281 File Offset: 0x0002C481
		public void RebuildConnectionDistanceLookup()
		{
			if (this.nearestNodeDistanceMode == PointGraph.NodeDistanceMode.Connection)
			{
				this.maximumConnectionLength = PointGraph.LongestConnectionLength(this.nodes, this.nodeCount);
				return;
			}
			this.maximumConnectionLength = 0L;
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0002E2AC File Offset: 0x0002C4AC
		private static long LongestConnectionLength(PointNode[] nodes, int nodeCount)
		{
			long num = 0L;
			for (int i = 0; i < nodeCount; i++)
			{
				PointNode pointNode = nodes[i];
				Connection[] connections = pointNode.connections;
				if (connections != null)
				{
					for (int j = 0; j < connections.Length; j++)
					{
						long sqrMagnitudeLong = (pointNode.position - connections[j].node.position).sqrMagnitudeLong;
						num = Math.Max(num, sqrMagnitudeLong);
					}
				}
			}
			return num;
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0002E31A File Offset: 0x0002C51A
		public void RegisterConnectionLength(long sqrLength)
		{
			this.maximumConnectionLength = Math.Max(this.maximumConnectionLength, sqrLength);
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x0002E330 File Offset: 0x0002C530
		protected virtual PointNode[] CreateNodes(int count)
		{
			PointNode[] array = new PointNode[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = new PointNode(this.active);
			}
			return array;
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x0002E35F File Offset: 0x0002C55F
		protected override void DestroyAllNodes()
		{
			base.DestroyAllNodes();
			this.nodes = null;
			this.lookupTree = null;
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x0002E375 File Offset: 0x0002C575
		protected override IGraphUpdatePromise ScanInternal()
		{
			return new PointGraph.PointGraphScanPromise
			{
				graph = this
			};
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x0002E384 File Offset: 0x0002C584
		public void ConnectNodes()
		{
			base.AssertSafeToUpdateGraph();
			IEnumerator<float> enumerator = PointGraph.ConnectNodesAsync(this.nodes, this.nodeCount, this.lookupTree, this.maxDistance, this.limits, this).GetEnumerator();
			while (enumerator.MoveNext())
			{
			}
			this.RebuildConnectionDistanceLookup();
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0002E3CF File Offset: 0x0002C5CF
		private static IEnumerable<float> ConnectNodesAsync(PointNode[] nodes, int nodeCount, PointKDTree lookupTree, float maxDistance, Vector3 limits, PointGraph graph)
		{
			if (maxDistance >= 0f)
			{
				List<Connection> connections = new List<Connection>();
				List<GraphNode> candidateConnections = new List<GraphNode>();
				long maxSquaredRange;
				if (maxDistance == 0f && (limits.x == 0f || limits.y == 0f || limits.z == 0f))
				{
					maxSquaredRange = long.MaxValue;
				}
				else
				{
					maxSquaredRange = (long)(Mathf.Max(limits.x, Mathf.Max(limits.y, Mathf.Max(limits.z, maxDistance))) * 1000f) + 1L;
					maxSquaredRange *= maxSquaredRange;
				}
				int num3;
				for (int i = 0; i < nodeCount; i = num3 + 1)
				{
					if (i % 512 == 0)
					{
						yield return (float)i / (float)nodeCount;
					}
					connections.Clear();
					PointNode pointNode = nodes[i];
					if (lookupTree != null)
					{
						candidateConnections.Clear();
						lookupTree.GetInRange(pointNode.position, maxSquaredRange, candidateConnections);
						for (int j = 0; j < candidateConnections.Count; j++)
						{
							PointNode pointNode2 = candidateConnections[j] as PointNode;
							float num;
							if (pointNode2 != pointNode && graph.IsValidConnection(pointNode, pointNode2, out num))
							{
								connections.Add(new Connection(pointNode2, (uint)Mathf.RoundToInt(num * 1000f), true, true));
							}
						}
					}
					else
					{
						for (int k = 0; k < nodeCount; k++)
						{
							if (i != k)
							{
								PointNode pointNode3 = nodes[k];
								float num2;
								if (graph.IsValidConnection(pointNode, pointNode3, out num2))
								{
									connections.Add(new Connection(pointNode3, (uint)Mathf.RoundToInt(num2 * 1000f), true, true));
								}
							}
						}
					}
					pointNode.connections = connections.ToArray();
					pointNode.SetConnectivityDirty();
					num3 = i;
				}
				connections = null;
				candidateConnections = null;
			}
			yield break;
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0002E404 File Offset: 0x0002C604
		public virtual bool IsValidConnection(GraphNode a, GraphNode b, out float dist)
		{
			dist = 0f;
			if (!a.Walkable || !b.Walkable)
			{
				return false;
			}
			Vector3 vector = (Vector3)(b.position - a.position);
			if ((!Mathf.Approximately(this.limits.x, 0f) && Mathf.Abs(vector.x) > this.limits.x) || (!Mathf.Approximately(this.limits.y, 0f) && Mathf.Abs(vector.y) > this.limits.y) || (!Mathf.Approximately(this.limits.z, 0f) && Mathf.Abs(vector.z) > this.limits.z))
			{
				return false;
			}
			dist = vector.magnitude;
			if (this.maxDistance != 0f && dist >= this.maxDistance)
			{
				return false;
			}
			if (!this.raycast)
			{
				return true;
			}
			Ray ray = new Ray((Vector3)a.position, vector);
			Ray ray2 = new Ray((Vector3)b.position, -vector);
			if (this.use2DPhysics)
			{
				if (this.thickRaycast)
				{
					return !Physics2D.CircleCast(ray.origin, this.thickRaycastRadius, ray.direction, dist, this.mask) && !Physics2D.CircleCast(ray2.origin, this.thickRaycastRadius, ray2.direction, dist, this.mask);
				}
				return !Physics2D.Linecast((Vector3)a.position, (Vector3)b.position, this.mask) && !Physics2D.Linecast((Vector3)b.position, (Vector3)a.position, this.mask);
			}
			else
			{
				if (this.thickRaycast)
				{
					return !Physics.SphereCast(ray, this.thickRaycastRadius, dist, this.mask) && !Physics.SphereCast(ray2, this.thickRaycastRadius, dist, this.mask);
				}
				return !Physics.Linecast((Vector3)a.position, (Vector3)b.position, this.mask) && !Physics.Linecast((Vector3)b.position, (Vector3)a.position, this.mask);
			}
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x0002E6B9 File Offset: 0x0002C8B9
		IGraphUpdatePromise IUpdatableGraph.ScheduleGraphUpdates(List<GraphUpdateObject> graphUpdates)
		{
			if (!this.isScanned)
			{
				return null;
			}
			return new PointGraph.PointGraphUpdatePromise
			{
				graph = this,
				graphUpdates = graphUpdates
			};
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x0002E6D8 File Offset: 0x0002C8D8
		protected override void PostDeserialization(GraphSerializationContext ctx)
		{
			this.RebuildNodeLookup();
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x0002E6E0 File Offset: 0x0002C8E0
		public override void RelocateNodes(Matrix4x4 deltaMatrix)
		{
			base.RelocateNodes(deltaMatrix);
			this.RebuildNodeLookup();
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x0002E6F0 File Offset: 0x0002C8F0
		protected override void SerializeExtraInfo(GraphSerializationContext ctx)
		{
			if (this.nodes == null)
			{
				ctx.writer.Write(-1);
			}
			ctx.writer.Write(this.nodeCount);
			for (int i = 0; i < this.nodeCount; i++)
			{
				if (this.nodes[i] == null)
				{
					ctx.writer.Write(-1);
				}
				else
				{
					ctx.writer.Write(0);
					this.nodes[i].SerializeNode(ctx);
				}
			}
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x0002E768 File Offset: 0x0002C968
		protected override void DeserializeExtraInfo(GraphSerializationContext ctx)
		{
			int num = ctx.reader.ReadInt32();
			if (num == -1)
			{
				this.nodes = null;
				return;
			}
			this.nodes = new PointNode[num];
			this.nodeCount = num;
			for (int i = 0; i < this.nodes.Length; i++)
			{
				if (ctx.reader.ReadInt32() != -1)
				{
					this.nodes[i] = new PointNode(this.active);
					this.nodes[i].DeserializeNode(ctx);
				}
			}
		}

		// Token: 0x0400055C RID: 1372
		[JsonMember]
		public Transform root;

		// Token: 0x0400055D RID: 1373
		[JsonMember]
		public string searchTag;

		// Token: 0x0400055E RID: 1374
		[JsonMember]
		public float maxDistance;

		// Token: 0x0400055F RID: 1375
		[JsonMember]
		public Vector3 limits;

		// Token: 0x04000560 RID: 1376
		[JsonMember]
		public bool raycast = true;

		// Token: 0x04000561 RID: 1377
		[JsonMember]
		public bool use2DPhysics;

		// Token: 0x04000562 RID: 1378
		[JsonMember]
		public bool thickRaycast;

		// Token: 0x04000563 RID: 1379
		[JsonMember]
		public float thickRaycastRadius = 1f;

		// Token: 0x04000564 RID: 1380
		[JsonMember]
		public bool recursive = true;

		// Token: 0x04000565 RID: 1381
		[JsonMember]
		public LayerMask mask;

		// Token: 0x04000566 RID: 1382
		[JsonMember]
		public bool optimizeForSparseGraph;

		// Token: 0x04000567 RID: 1383
		private PointKDTree lookupTree = new PointKDTree();

		// Token: 0x04000568 RID: 1384
		private long maximumConnectionLength;

		// Token: 0x04000569 RID: 1385
		public PointNode[] nodes;

		// Token: 0x0400056A RID: 1386
		[JsonMember]
		public PointGraph.NodeDistanceMode nearestNodeDistanceMode;

		// Token: 0x020000FD RID: 253
		public enum NodeDistanceMode
		{
			// Token: 0x0400056D RID: 1389
			Node,
			// Token: 0x0400056E RID: 1390
			Connection
		}

		// Token: 0x020000FE RID: 254
		private class PointGraphScanPromise : IGraphUpdatePromise
		{
			// Token: 0x06000904 RID: 2308 RVA: 0x0002E80E File Offset: 0x0002CA0E
			public IEnumerator<JobHandle> Prepare()
			{
				Transform root = this.graph.root;
				if (root == null)
				{
					GameObject[] array = ((this.graph.searchTag != null) ? GameObject.FindGameObjectsWithTag(this.graph.searchTag) : null);
					if (array == null)
					{
						this.nodes = new PointNode[0];
					}
					else
					{
						this.nodes = this.graph.CreateNodes(array.Length);
						for (int i = 0; i < array.Length; i++)
						{
							PointNode pointNode = this.nodes[i];
							pointNode.position = (Int3)array[i].transform.position;
							pointNode.Walkable = true;
							pointNode.gameObject = array[i].gameObject;
						}
					}
				}
				else
				{
					if (!this.graph.recursive)
					{
						int childCount = root.childCount;
						this.nodes = this.graph.CreateNodes(childCount);
						int num = 0;
						using (IEnumerator enumerator = root.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								Transform transform = (Transform)obj;
								PointNode pointNode2 = this.nodes[num];
								pointNode2.position = (Int3)transform.position;
								pointNode2.Walkable = true;
								pointNode2.gameObject = transform.gameObject;
								num++;
							}
							goto IL_01A9;
						}
					}
					int num2 = PointGraph.CountChildren(root);
					this.nodes = this.graph.CreateNodes(num2);
					int num3 = 0;
					PointGraph.AddChildren(this.nodes, ref num3, root);
				}
				IL_01A9:
				JobHandle jobHandle = default(JobHandle);
				this.lookupTree = PointGraph.BuildNodeLookup(this.nodes, this.nodes.Length, this.graph.optimizeForSparseGraph);
				foreach (float num4 in PointGraph.ConnectNodesAsync(this.nodes, this.nodes.Length, this.lookupTree, this.graph.maxDistance, this.graph.limits, this.graph))
				{
					jobHandle = default(JobHandle);
				}
				IEnumerator<float> enumerator2 = null;
				yield break;
				yield break;
			}

			// Token: 0x06000905 RID: 2309 RVA: 0x0002E820 File Offset: 0x0002CA20
			public void Apply(IGraphUpdateContext ctx)
			{
				this.graph.DestroyAllNodes();
				this.graph.lookupTree = this.lookupTree;
				this.graph.nodes = this.nodes;
				this.graph.nodeCount = this.nodes.Length;
				this.graph.maximumConnectionLength = ((this.graph.nearestNodeDistanceMode == PointGraph.NodeDistanceMode.Connection) ? PointGraph.LongestConnectionLength(this.nodes, this.nodes.Length) : 0L);
			}

			// Token: 0x0400056F RID: 1391
			public PointGraph graph;

			// Token: 0x04000570 RID: 1392
			private PointKDTree lookupTree;

			// Token: 0x04000571 RID: 1393
			private PointNode[] nodes;
		}

		// Token: 0x02000100 RID: 256
		private class PointGraphUpdatePromise : IGraphUpdatePromise
		{
			// Token: 0x0600090E RID: 2318 RVA: 0x0002EBE4 File Offset: 0x0002CDE4
			public void Apply(IGraphUpdateContext ctx)
			{
				PointNode[] nodes = this.graph.nodes;
				for (int i = 0; i < this.graphUpdates.Count; i++)
				{
					GraphUpdateObject graphUpdateObject = this.graphUpdates[i];
					for (int j = 0; j < this.graph.nodeCount; j++)
					{
						PointNode pointNode = nodes[j];
						if (graphUpdateObject.bounds.Contains((Vector3)pointNode.position))
						{
							graphUpdateObject.WillUpdateNode(pointNode);
							graphUpdateObject.Apply(pointNode);
						}
					}
					if (graphUpdateObject.updatePhysics)
					{
						Bounds bounds = graphUpdateObject.bounds;
						if (this.graph.thickRaycast)
						{
							bounds.Expand(this.graph.thickRaycastRadius * 2f);
						}
						List<Connection> list = ListPool<Connection>.Claim();
						for (int k = 0; k < this.graph.nodeCount; k++)
						{
							PointNode pointNode2 = this.graph.nodes[k];
							Vector3 vector = (Vector3)pointNode2.position;
							List<Connection> list2 = null;
							for (int l = 0; l < this.graph.nodeCount; l++)
							{
								if (l != k)
								{
									Vector3 vector2 = (Vector3)nodes[l].position;
									if (VectorMath.SegmentIntersectsBounds(bounds, vector, vector2))
									{
										PointNode pointNode3 = nodes[l];
										bool flag = pointNode2.ContainsOutgoingConnection(pointNode3);
										float num;
										bool flag2 = this.graph.IsValidConnection(pointNode2, pointNode3, out num);
										if (list2 == null && flag != flag2)
										{
											list.Clear();
											list2 = list;
											if (pointNode2.connections != null)
											{
												list2.AddRange(pointNode2.connections);
											}
										}
										if (!flag && flag2)
										{
											uint num2 = (uint)Mathf.RoundToInt(num * 1000f);
											list2.Add(new Connection(pointNode3, num2, true, true));
											this.graph.RegisterConnectionLength((pointNode3.position - pointNode2.position).sqrMagnitudeLong);
										}
										else if (flag && !flag2)
										{
											for (int m = 0; m < list2.Count; m++)
											{
												if (list2[m].node == pointNode3)
												{
													list2.RemoveAt(m);
													break;
												}
											}
										}
									}
								}
							}
							if (list2 != null)
							{
								pointNode2.connections = list2.ToArray();
								pointNode2.SetConnectivityDirty();
							}
						}
						ListPool<Connection>.Release(ref list);
						ctx.DirtyBounds(graphUpdateObject.bounds);
					}
				}
				ListPool<GraphUpdateObject>.Release(ref this.graphUpdates);
			}

			// Token: 0x04000576 RID: 1398
			public PointGraph graph;

			// Token: 0x04000577 RID: 1399
			public List<GraphUpdateObject> graphUpdates;
		}
	}
}
