using System;
using System.Collections.Generic;
using Pathfinding.Pooling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000F7 RID: 247
	public class PointKDTree
	{
		// Token: 0x060008CA RID: 2250 RVA: 0x0002D1BC File Offset: 0x0002B3BC
		public PointKDTree()
		{
			this.tree[1] = new PointKDTree.Node
			{
				data = this.GetOrCreateList()
			};
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x0002D214 File Offset: 0x0002B414
		public void Add(GraphNode node)
		{
			this.numNodes++;
			this.Add(node, 1, 0);
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0002D22D File Offset: 0x0002B42D
		public void Remove(GraphNode node)
		{
			if (!this.Remove(node, 1, 0))
			{
				throw new ArgumentException("The node is not in the lookup tree. Has it been moved?");
			}
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x0002D248 File Offset: 0x0002B448
		public void Rebuild(GraphNode[] nodes, int start, int end)
		{
			if (start < 0 || end < start || end > nodes.Length)
			{
				throw new ArgumentException();
			}
			for (int i = 0; i < this.tree.Length; i++)
			{
				GraphNode[] data = this.tree[i].data;
				if (data != null)
				{
					for (int j = 0; j < 21; j++)
					{
						data[j] = null;
					}
					this.arrayCache.Push(data);
					this.tree[i].data = null;
				}
			}
			this.numNodes = end - start;
			this.Build(1, new List<GraphNode>(nodes), start, end);
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x0002D2D8 File Offset: 0x0002B4D8
		private GraphNode[] GetOrCreateList()
		{
			if (this.arrayCache.Count <= 0)
			{
				return new GraphNode[21];
			}
			return this.arrayCache.Pop();
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x0002D2FB File Offset: 0x0002B4FB
		private int Size(int index)
		{
			if (this.tree[index].data == null)
			{
				return this.Size(2 * index) + this.Size(2 * index + 1);
			}
			return (int)this.tree[index].count;
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x0002D338 File Offset: 0x0002B538
		private void CollectAndClear(int index, List<GraphNode> buffer)
		{
			GraphNode[] data = this.tree[index].data;
			ushort count = this.tree[index].count;
			if (data != null)
			{
				this.tree[index] = default(PointKDTree.Node);
				for (int i = 0; i < (int)count; i++)
				{
					buffer.Add(data[i]);
					data[i] = null;
				}
				this.arrayCache.Push(data);
				return;
			}
			this.CollectAndClear(index * 2, buffer);
			this.CollectAndClear(index * 2 + 1, buffer);
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x0002D3BA File Offset: 0x0002B5BA
		private static int MaxAllowedSize(int numNodes, int depth)
		{
			return Math.Min(5 * numNodes / 2 >> depth, 3 * numNodes / 4);
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x0002D3D0 File Offset: 0x0002B5D0
		private void Rebalance(int index)
		{
			this.CollectAndClear(index, this.largeList);
			this.Build(index, this.largeList, 0, this.largeList.Count);
			this.largeList.ClearFast<GraphNode>();
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x0002D404 File Offset: 0x0002B604
		private void EnsureSize(int index)
		{
			if (index >= this.tree.Length)
			{
				PointKDTree.Node[] array = new PointKDTree.Node[Math.Max(index + 1, this.tree.Length * 2)];
				this.tree.CopyTo(array, 0);
				this.tree = array;
			}
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x0002D448 File Offset: 0x0002B648
		private void Build(int index, List<GraphNode> nodes, int start, int end)
		{
			this.EnsureSize(index);
			if (end - start <= 10)
			{
				GraphNode[] array = (this.tree[index].data = this.GetOrCreateList());
				this.tree[index].count = (ushort)(end - start);
				for (int i = start; i < end; i++)
				{
					array[i - start] = nodes[i];
				}
				return;
			}
			Int3 position;
			Int3 @int = (position = nodes[start].position);
			for (int j = start; j < end; j++)
			{
				Int3 position2 = nodes[j].position;
				position = new Int3(Math.Min(position.x, position2.x), Math.Min(position.y, position2.y), Math.Min(position.z, position2.z));
				@int = new Int3(Math.Max(@int.x, position2.x), Math.Max(@int.y, position2.y), Math.Max(@int.z, position2.z));
			}
			Int3 int2 = @int - position;
			int num = ((int2.x > int2.y) ? ((int2.x > int2.z) ? 0 : 2) : ((int2.y > int2.z) ? 1 : 2));
			nodes.Sort(start, end - start, PointKDTree.comparers[num]);
			int num2 = (start + end) / 2;
			this.tree[index].split = (nodes[num2 - 1].position[num] + nodes[num2].position[num]) / 2;
			this.tree[index].splitAxis = (byte)num;
			this.Build(index * 2, nodes, start, num2);
			this.Build(index * 2 + 1, nodes, num2, end);
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0002D634 File Offset: 0x0002B834
		private void Add(GraphNode point, int index, int depth = 0)
		{
			while (this.tree[index].data == null)
			{
				index = 2 * index + ((point.position[(int)this.tree[index].splitAxis] < this.tree[index].split) ? 0 : 1);
				depth++;
			}
			GraphNode[] data = this.tree[index].data;
			PointKDTree.Node[] array = this.tree;
			int num = index;
			ushort count = array[num].count;
			array[num].count = count + 1;
			data[(int)count] = point;
			if (this.tree[index].count >= 21)
			{
				int num2 = 0;
				while (depth - num2 > 0 && this.Size(index >> num2) > PointKDTree.MaxAllowedSize(this.numNodes, depth - num2))
				{
					num2++;
				}
				this.Rebalance(index >> num2);
			}
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x0002D70C File Offset: 0x0002B90C
		private bool Remove(GraphNode point, int index, int depth = 0)
		{
			Int3 position = point.position;
			while (this.tree[index].data == null)
			{
				int num = position[(int)this.tree[index].splitAxis];
				if (num == this.tree[index].split)
				{
					return this.Remove(point, 2 * index, depth + 1) || this.Remove(point, 2 * index + 1, depth + 1);
				}
				index = 2 * index + ((num < this.tree[index].split) ? 0 : 1);
				depth++;
			}
			int num2 = Array.IndexOf<GraphNode>(this.tree[index].data, point);
			if (num2 == -1)
			{
				return false;
			}
			PointKDTree.Node node = this.tree[index];
			node.count -= 1;
			node.data[num2] = node.data[(int)node.count];
			node.data[(int)node.count] = null;
			this.tree[index] = node;
			this.numNodes--;
			if (node.count == 0 && index != 1)
			{
				this.Rebalance(index >> 1);
			}
			return true;
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x0002D838 File Offset: 0x0002BA38
		public GraphNode GetNearest(Int3 point, ref NearestNodeConstraint constraint, ref float distanceSqr)
		{
			GraphNode graphNode = null;
			long num = ((distanceSqr < float.PositiveInfinity) ? ((long)(1000000f * distanceSqr)) : long.MaxValue);
			this.GetNearestInternal(1, point, ref constraint, ref graphNode, ref num);
			distanceSqr = ((graphNode != null) ? (1.0000001E-06f * (float)num) : float.PositiveInfinity);
			return graphNode;
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x0002D888 File Offset: 0x0002BA88
		private void GetNearestInternal(int index, Int3 point, ref NearestNodeConstraint constraint, ref GraphNode best, ref long bestSqrDist)
		{
			GraphNode[] data = this.tree[index].data;
			if (data != null)
			{
				for (int i = (int)(this.tree[index].count - 1); i >= 0; i--)
				{
					long sqrMagnitudeLong = (data[i].position - point).sqrMagnitudeLong;
					if (sqrMagnitudeLong < bestSqrDist && constraint.Suitable(data[i]))
					{
						bestSqrDist = sqrMagnitudeLong;
						best = data[i];
					}
				}
				return;
			}
			long num = (long)(point[(int)this.tree[index].splitAxis] - this.tree[index].split);
			int num2 = 2 * index + ((num < 0L) ? 0 : 1);
			this.GetNearestInternal(num2, point, ref constraint, ref best, ref bestSqrDist);
			if (num * num < bestSqrDist)
			{
				this.GetNearestInternal(num2 ^ 1, point, ref constraint, ref best, ref bestSqrDist);
			}
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x0002D960 File Offset: 0x0002BB60
		public GraphNode GetNearestConnection(Int3 point, ref NearestNodeConstraint constraint, long maximumSqrConnectionLength)
		{
			GraphNode graphNode = null;
			long maxValue = long.MaxValue;
			long num = (maximumSqrConnectionLength + 3L) / 4L;
			this.GetNearestConnectionInternal(1, point, ref constraint, ref graphNode, ref maxValue, num);
			return graphNode;
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x0002D990 File Offset: 0x0002BB90
		private void GetNearestConnectionInternal(int index, Int3 point, ref NearestNodeConstraint constraint, ref GraphNode best, ref long bestSqrDist, long distanceThresholdOffset)
		{
			GraphNode[] data = this.tree[index].data;
			if (data != null)
			{
				Vector3 vector = (Vector3)point;
				for (int i = (int)(this.tree[index].count - 1); i >= 0; i--)
				{
					long sqrMagnitudeLong = (data[i].position - point).sqrMagnitudeLong;
					if (sqrMagnitudeLong - distanceThresholdOffset < bestSqrDist && constraint.Suitable(data[i]))
					{
						Connection[] connections = (data[i] as PointNode).connections;
						if (connections != null)
						{
							Vector3 vector2 = (Vector3)data[i].position;
							for (int j = 0; j < connections.Length; j++)
							{
								Vector3 vector3 = ((Vector3)connections[j].node.position + vector2) * 0.5f;
								long num = (long)(VectorMath.SqrDistancePointSegment(vector2, vector3, vector) * 1000f * 1000f);
								if (num < bestSqrDist)
								{
									bestSqrDist = num;
									best = data[i];
								}
							}
						}
						if (sqrMagnitudeLong < bestSqrDist)
						{
							bestSqrDist = sqrMagnitudeLong;
							best = data[i];
						}
					}
				}
				return;
			}
			long num2 = (long)(point[(int)this.tree[index].splitAxis] - this.tree[index].split);
			int num3 = 2 * index + ((num2 < 0L) ? 0 : 1);
			this.GetNearestConnectionInternal(num3, point, ref constraint, ref best, ref bestSqrDist, distanceThresholdOffset);
			if (num2 * num2 - distanceThresholdOffset < bestSqrDist)
			{
				this.GetNearestConnectionInternal(num3 ^ 1, point, ref constraint, ref best, ref bestSqrDist, distanceThresholdOffset);
			}
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x0002DB1A File Offset: 0x0002BD1A
		public void GetInRange(Int3 point, long sqrRadius, List<GraphNode> buffer)
		{
			this.GetInRangeInternal(1, point, sqrRadius, buffer);
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x0002DB28 File Offset: 0x0002BD28
		private void GetInRangeInternal(int index, Int3 point, long sqrRadius, List<GraphNode> buffer)
		{
			GraphNode[] data = this.tree[index].data;
			if (data != null)
			{
				for (int i = (int)(this.tree[index].count - 1); i >= 0; i--)
				{
					if ((data[i].position - point).sqrMagnitudeLong < sqrRadius)
					{
						buffer.Add(data[i]);
					}
				}
				return;
			}
			long num = (long)(point[(int)this.tree[index].splitAxis] - this.tree[index].split);
			int num2 = 2 * index + ((num < 0L) ? 0 : 1);
			this.GetInRangeInternal(num2, point, sqrRadius, buffer);
			if (num * num < sqrRadius)
			{
				this.GetInRangeInternal(num2 ^ 1, point, sqrRadius, buffer);
			}
		}

		// Token: 0x04000551 RID: 1361
		public const int LeafSize = 10;

		// Token: 0x04000552 RID: 1362
		public const int LeafArraySize = 21;

		// Token: 0x04000553 RID: 1363
		private PointKDTree.Node[] tree = new PointKDTree.Node[16];

		// Token: 0x04000554 RID: 1364
		private int numNodes;

		// Token: 0x04000555 RID: 1365
		private readonly List<GraphNode> largeList = new List<GraphNode>();

		// Token: 0x04000556 RID: 1366
		private readonly Stack<GraphNode[]> arrayCache = new Stack<GraphNode[]>();

		// Token: 0x04000557 RID: 1367
		private static readonly IComparer<GraphNode>[] comparers = new IComparer<GraphNode>[]
		{
			new PointKDTree.CompareX(),
			new PointKDTree.CompareY(),
			new PointKDTree.CompareZ()
		};

		// Token: 0x020000F8 RID: 248
		private struct Node
		{
			// Token: 0x04000558 RID: 1368
			public GraphNode[] data;

			// Token: 0x04000559 RID: 1369
			public int split;

			// Token: 0x0400055A RID: 1370
			public ushort count;

			// Token: 0x0400055B RID: 1371
			public byte splitAxis;
		}

		// Token: 0x020000F9 RID: 249
		private class CompareX : IComparer<GraphNode>
		{
			// Token: 0x060008DE RID: 2270 RVA: 0x0002DC0C File Offset: 0x0002BE0C
			public int Compare(GraphNode lhs, GraphNode rhs)
			{
				return lhs.position.x.CompareTo(rhs.position.x);
			}
		}

		// Token: 0x020000FA RID: 250
		private class CompareY : IComparer<GraphNode>
		{
			// Token: 0x060008E0 RID: 2272 RVA: 0x0002DC29 File Offset: 0x0002BE29
			public int Compare(GraphNode lhs, GraphNode rhs)
			{
				return lhs.position.y.CompareTo(rhs.position.y);
			}
		}

		// Token: 0x020000FB RID: 251
		private class CompareZ : IComparer<GraphNode>
		{
			// Token: 0x060008E2 RID: 2274 RVA: 0x0002DC46 File Offset: 0x0002BE46
			public int Compare(GraphNode lhs, GraphNode rhs)
			{
				return lhs.position.z.CompareTo(rhs.position.z);
			}
		}
	}
}
