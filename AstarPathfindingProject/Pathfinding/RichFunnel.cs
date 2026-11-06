using System;
using System.Collections.Generic;
using Pathfinding.Pooling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000021 RID: 33
	public class RichFunnel : RichPathPart
	{
		// Token: 0x06000211 RID: 529 RVA: 0x00009640 File Offset: 0x00007840
		public RichFunnel()
		{
			this.left = ListPool<Vector3>.Claim();
			this.right = ListPool<Vector3>.Claim();
			this.nodes = new List<TriangleMeshNode>();
			this.graph = null;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00009690 File Offset: 0x00007890
		public RichFunnel Initialize(RichPath path, NavmeshBase graph)
		{
			if (graph == null)
			{
				throw new ArgumentNullException("graph");
			}
			if (this.graph != null)
			{
				throw new InvalidOperationException("Trying to initialize an already initialized object. " + ((graph != null) ? graph.ToString() : null));
			}
			this.graph = graph;
			this.path = path;
			return this;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x000096DF File Offset: 0x000078DF
		public override void OnEnterPool()
		{
			this.left.Clear();
			this.right.Clear();
			this.nodes.Clear();
			this.graph = null;
			this.currentNode = 0;
			this.checkForDestroyedNodesCounter = 0;
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000214 RID: 532 RVA: 0x00009718 File Offset: 0x00007918
		public TriangleMeshNode CurrentNode
		{
			get
			{
				TriangleMeshNode triangleMeshNode = this.nodes[this.currentNode];
				if (!triangleMeshNode.Destroyed)
				{
					return triangleMeshNode;
				}
				return null;
			}
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00009744 File Offset: 0x00007944
		public void BuildFunnelCorridor(List<GraphNode> nodes, int start, int end)
		{
			this.exactStart = (nodes[start] as MeshNode).ClosestPointOnNode(this.exactStart);
			this.exactEnd = (nodes[end] as MeshNode).ClosestPointOnNode(this.exactEnd);
			this.left.Clear();
			this.right.Clear();
			this.left.Add(this.exactStart);
			this.right.Add(this.exactStart);
			this.nodes.Clear();
			if (this.funnelSimplification)
			{
				List<GraphNode> list = ListPool<GraphNode>.Claim(end - start);
				TraversalConstraint none = TraversalConstraint.None;
				none.tags = ((this.path.seeker != null) ? this.path.seeker.traversableTags : (-1));
				TraversalCosts traversalCosts = default(TraversalCosts);
				traversalCosts.tagCostMultipliers = ((this.path.seeker != null) ? this.path.seeker.tagCostMultipliers : null);
				traversalCosts.tagEntryCosts = ((this.path.seeker != null) ? this.path.seeker.tagEntryCosts : null);
				Funnel.Simplify(new Funnel.PathPart
				{
					startIndex = start,
					endIndex = end,
					startPoint = this.exactStart,
					endPoint = this.exactEnd,
					type = Funnel.PartType.NodeSequence
				}, this.graph, nodes, list, ref traversalCosts, ref none);
				if (this.nodes.Capacity < list.Count)
				{
					this.nodes.Capacity = list.Count;
				}
				for (int i = 0; i < list.Count; i++)
				{
					TriangleMeshNode triangleMeshNode = list[i] as TriangleMeshNode;
					if (triangleMeshNode != null)
					{
						this.nodes.Add(triangleMeshNode);
					}
				}
				ListPool<GraphNode>.Release(ref list);
			}
			else
			{
				if (this.nodes.Capacity < end - start)
				{
					this.nodes.Capacity = end - start;
				}
				for (int j = start; j <= end; j++)
				{
					TriangleMeshNode triangleMeshNode2 = nodes[j] as TriangleMeshNode;
					if (triangleMeshNode2 != null)
					{
						this.nodes.Add(triangleMeshNode2);
					}
				}
			}
			for (int k = 0; k < this.nodes.Count - 1; k++)
			{
				Vector3 vector;
				Vector3 vector2;
				if (this.nodes[k].GetPortal(this.nodes[k + 1], out vector, out vector2))
				{
					this.left.Add(vector);
					this.right.Add(vector2);
				}
				else
				{
					this.left.Add((Vector3)this.nodes[k].position);
					this.right.Add((Vector3)this.nodes[k].position);
					this.left.Add((Vector3)this.nodes[k + 1].position);
					this.right.Add((Vector3)this.nodes[k + 1].position);
				}
			}
			this.left.Add(this.exactEnd);
			this.right.Add(this.exactEnd);
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00009A8C File Offset: 0x00007C8C
		private void UpdateFunnelCorridor(int splitIndex, List<TriangleMeshNode> prefix)
		{
			this.nodes.RemoveRange(0, splitIndex);
			this.nodes.InsertRange(0, prefix);
			this.left.Clear();
			this.right.Clear();
			this.left.Add(this.exactStart);
			this.right.Add(this.exactStart);
			for (int i = 0; i < this.nodes.Count - 1; i++)
			{
				Vector3 vector;
				Vector3 vector2;
				if (this.nodes[i].GetPortal(this.nodes[i + 1], out vector, out vector2))
				{
					this.left.Add(vector);
					this.right.Add(vector2);
				}
			}
			this.left.Add(this.exactEnd);
			this.right.Add(this.exactEnd);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x00009B64 File Offset: 0x00007D64
		private bool CheckForDestroyedNodes()
		{
			int i = 0;
			int count = this.nodes.Count;
			while (i < count)
			{
				if (this.nodes[i].Destroyed)
				{
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000218 RID: 536 RVA: 0x00009BA0 File Offset: 0x00007DA0
		public float DistanceToEndOfPath
		{
			get
			{
				TriangleMeshNode triangleMeshNode = this.CurrentNode;
				Vector3 vector = ((triangleMeshNode != null) ? triangleMeshNode.ClosestPointOnNode(this.currentPosition) : this.currentPosition);
				return (this.exactEnd - vector).magnitude;
			}
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00009BE0 File Offset: 0x00007DE0
		public Vector3 ClampToNavmesh(Vector3 position)
		{
			if (this.path.transform != null)
			{
				position = this.path.transform.InverseTransform(position);
			}
			this.ClampToNavmeshInternal(ref position);
			if (this.path.transform != null)
			{
				position = this.path.transform.Transform(position);
			}
			return position;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00009C38 File Offset: 0x00007E38
		public Vector3 Update(Vector3 position, List<Vector3> buffer, int numCorners, out bool lastCorner, out bool requiresRepath)
		{
			if (this.path.transform != null)
			{
				position = this.path.transform.InverseTransform(position);
			}
			lastCorner = false;
			requiresRepath = false;
			if (this.checkForDestroyedNodesCounter >= 10)
			{
				this.checkForDestroyedNodesCounter = 0;
				requiresRepath |= this.CheckForDestroyedNodes();
			}
			else
			{
				this.checkForDestroyedNodesCounter++;
			}
			bool flag = this.ClampToNavmeshInternal(ref position);
			this.currentPosition = position;
			if (flag)
			{
				requiresRepath = true;
				lastCorner = false;
				buffer.Add(position);
			}
			else if (!this.FindNextCorners(position, this.currentNode, buffer, numCorners, out lastCorner))
			{
				Debug.LogError("Failed to find next corners in the path");
				buffer.Add(position);
			}
			if (this.path.transform != null)
			{
				for (int i = 0; i < buffer.Count; i++)
				{
					buffer[i] = this.path.transform.Transform(buffer[i]);
				}
				position = this.path.transform.Transform(position);
			}
			return position;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00009D34 File Offset: 0x00007F34
		private bool ClampToNavmeshInternal(ref Vector3 position)
		{
			TriangleMeshNode triangleMeshNode = this.nodes[this.currentNode];
			if (triangleMeshNode.Destroyed)
			{
				return true;
			}
			if (triangleMeshNode.ContainsPoint(position))
			{
				return false;
			}
			Queue<TriangleMeshNode> queue = RichFunnel.navmeshClampQueue;
			List<TriangleMeshNode> list = RichFunnel.navmeshClampList;
			Dictionary<TriangleMeshNode, TriangleMeshNode> dictionary = RichFunnel.navmeshClampDict;
			triangleMeshNode.TemporaryFlag1 = true;
			dictionary[triangleMeshNode] = null;
			queue.Enqueue(triangleMeshNode);
			list.Add(triangleMeshNode);
			float num = float.PositiveInfinity;
			Vector3 vector = position;
			TriangleMeshNode triangleMeshNode2 = null;
			while (queue.Count > 0)
			{
				TriangleMeshNode triangleMeshNode3 = queue.Dequeue();
				Vector3 vector2 = triangleMeshNode3.ClosestPointOnNodeXZ(position);
				float num2 = VectorMath.MagnitudeXZ(vector2 - position);
				if (num2 <= num * 1.05f + 0.001f)
				{
					if (num2 < num)
					{
						num = num2;
						vector = vector2;
						triangleMeshNode2 = triangleMeshNode3;
					}
					for (int i = 0; i < triangleMeshNode3.connections.Length; i++)
					{
						if (triangleMeshNode3.connections[i].isOutgoing)
						{
							TriangleMeshNode triangleMeshNode4 = triangleMeshNode3.connections[i].node as TriangleMeshNode;
							if (triangleMeshNode4 != null && !triangleMeshNode4.TemporaryFlag1)
							{
								triangleMeshNode4.TemporaryFlag1 = true;
								dictionary[triangleMeshNode4] = triangleMeshNode3;
								queue.Enqueue(triangleMeshNode4);
								list.Add(triangleMeshNode4);
							}
						}
					}
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				list[j].TemporaryFlag1 = false;
			}
			list.ClearFast<TriangleMeshNode>();
			int num3 = this.nodes.IndexOf(triangleMeshNode2);
			position.x = vector.x;
			position.z = vector.z;
			if (num3 == -1)
			{
				List<TriangleMeshNode> list2 = RichFunnel.navmeshClampList;
				while (num3 == -1)
				{
					list2.Add(triangleMeshNode2);
					triangleMeshNode2 = dictionary[triangleMeshNode2];
					num3 = this.nodes.IndexOf(triangleMeshNode2);
				}
				this.exactStart = position;
				this.UpdateFunnelCorridor(num3, list2);
				list2.ClearFast<TriangleMeshNode>();
				this.currentNode = 0;
			}
			else
			{
				this.currentNode = num3;
			}
			dictionary.Clear();
			return this.currentNode + 1 < this.nodes.Count && this.nodes[this.currentNode + 1].Destroyed;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00009F71 File Offset: 0x00008171
		public void FindWalls(List<Vector3> wallBuffer, float range)
		{
			this.FindWalls(this.currentNode, wallBuffer, this.currentPosition, range);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00009F88 File Offset: 0x00008188
		private void FindWalls(int nodeIndex, List<Vector3> wallBuffer, Vector3 position, float range)
		{
			if (range <= 0f)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			range *= range;
			int num = 0;
			while (!flag || !flag2)
			{
				if ((num >= 0 || !flag) && (num <= 0 || !flag2))
				{
					if (num < 0 && nodeIndex + num < 0)
					{
						flag = true;
					}
					else if (num > 0 && nodeIndex + num >= this.nodes.Count)
					{
						flag2 = true;
					}
					else
					{
						TriangleMeshNode triangleMeshNode = ((nodeIndex + num - 1 < 0) ? null : this.nodes[nodeIndex + num - 1]);
						TriangleMeshNode triangleMeshNode2 = this.nodes[nodeIndex + num];
						TriangleMeshNode triangleMeshNode3 = ((nodeIndex + num + 1 >= this.nodes.Count) ? null : this.nodes[nodeIndex + num + 1]);
						if (triangleMeshNode2.Destroyed)
						{
							break;
						}
						Vector3 vector = triangleMeshNode2.ClosestPointOnNodeXZ(position) - position;
						vector.y = 0f;
						if (vector.sqrMagnitude > range)
						{
							if (num < 0)
							{
								flag = true;
							}
							else
							{
								flag2 = true;
							}
						}
						else
						{
							for (int i = 0; i < 3; i++)
							{
								this.triBuffer[i] = 0;
							}
							for (int j = 0; j < triangleMeshNode2.connections.Length; j++)
							{
								TriangleMeshNode triangleMeshNode4 = triangleMeshNode2.connections[j].node as TriangleMeshNode;
								if (triangleMeshNode4 != null)
								{
									int num2 = -1;
									for (int k = 0; k < 3; k++)
									{
										for (int l = 0; l < 3; l++)
										{
											if (triangleMeshNode2.GetVertex(k) == triangleMeshNode4.GetVertex((l + 1) % 3) && triangleMeshNode2.GetVertex((k + 1) % 3) == triangleMeshNode4.GetVertex(l))
											{
												num2 = k;
												k = 3;
												break;
											}
										}
									}
									if (num2 != -1)
									{
										this.triBuffer[num2] = ((triangleMeshNode4 == triangleMeshNode || triangleMeshNode4 == triangleMeshNode3) ? 2 : 1);
									}
								}
							}
							for (int m = 0; m < 3; m++)
							{
								if (this.triBuffer[m] == 0)
								{
									wallBuffer.Add((Vector3)triangleMeshNode2.GetVertex(m));
									wallBuffer.Add((Vector3)triangleMeshNode2.GetVertex((m + 1) % 3));
								}
							}
						}
					}
				}
				num = ((num < 0) ? (-num) : (-num - 1));
			}
			if (this.path.transform != null)
			{
				for (int n = 0; n < wallBuffer.Count; n++)
				{
					wallBuffer[n] = this.path.transform.Transform(wallBuffer[n]);
				}
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000A20C File Offset: 0x0000840C
		private bool FindNextCorners(Vector3 origin, int startIndex, List<Vector3> funnelPath, int numCorners, out bool lastCorner)
		{
			lastCorner = false;
			if (this.left == null)
			{
				throw new Exception("left list is null");
			}
			if (this.right == null)
			{
				throw new Exception("right list is null");
			}
			if (funnelPath == null)
			{
				throw new ArgumentNullException("funnelPath");
			}
			if (this.left.Count != this.right.Count)
			{
				throw new ArgumentException("left and right lists must have equal length");
			}
			int count = this.left.Count;
			if (count == 0)
			{
				throw new ArgumentException("no diagonals");
			}
			if (count - startIndex < 3)
			{
				funnelPath.Add(this.left[count - 1]);
				lastCorner = true;
				return true;
			}
			while (this.left[startIndex + 1] == this.left[startIndex + 2] && this.right[startIndex + 1] == this.right[startIndex + 2])
			{
				startIndex++;
				if (count - startIndex <= 3)
				{
					return false;
				}
			}
			Vector3 vector = this.left[startIndex + 2];
			if (vector == this.left[startIndex + 1])
			{
				vector = this.right[startIndex + 2];
			}
			while (VectorMath.IsColinearXZ(origin, this.left[startIndex + 1], this.right[startIndex + 1]) || VectorMath.RightOrColinearXZ(this.left[startIndex + 1], this.right[startIndex + 1], vector) == VectorMath.RightOrColinearXZ(this.left[startIndex + 1], this.right[startIndex + 1], origin))
			{
				startIndex++;
				if (count - startIndex < 3)
				{
					funnelPath.Add(this.left[count - 1]);
					lastCorner = true;
					return true;
				}
				vector = this.left[startIndex + 2];
				if (vector == this.left[startIndex + 1])
				{
					vector = this.right[startIndex + 2];
				}
			}
			Vector3 vector2 = origin;
			Vector3 vector3 = this.left[startIndex + 1];
			Vector3 vector4 = this.right[startIndex + 1];
			int num = startIndex + 1;
			int num2 = startIndex + 1;
			int i = startIndex + 2;
			while (i < count)
			{
				if (funnelPath.Count >= numCorners)
				{
					return true;
				}
				if (funnelPath.Count > 2000)
				{
					Debug.LogWarning("Avoiding infinite loop. Remove this check if you have this long paths.");
					break;
				}
				Vector3 vector5 = this.left[i];
				Vector3 vector6 = this.right[i];
				if (VectorMath.SignedTriangleAreaTimes2XZ(vector2, vector4, vector6) < 0f)
				{
					goto IL_02AE;
				}
				if (vector2 == vector4 || VectorMath.SignedTriangleAreaTimes2XZ(vector2, vector3, vector6) <= 0f)
				{
					vector4 = vector6;
					num = i;
					goto IL_02AE;
				}
				funnelPath.Add(vector3);
				vector2 = vector3;
				int num3 = num2;
				vector3 = vector2;
				vector4 = vector2;
				num2 = num3;
				num = num3;
				i = num3;
				IL_02FB:
				i++;
				continue;
				IL_02AE:
				if (VectorMath.SignedTriangleAreaTimes2XZ(vector2, vector3, vector5) > 0f)
				{
					goto IL_02FB;
				}
				if (vector2 == vector3 || VectorMath.SignedTriangleAreaTimes2XZ(vector2, vector4, vector5) >= 0f)
				{
					vector3 = vector5;
					num2 = i;
					goto IL_02FB;
				}
				funnelPath.Add(vector4);
				vector2 = vector4;
				int num4 = num;
				vector3 = vector2;
				vector4 = vector2;
				num2 = num4;
				num = num4;
				i = num4;
				goto IL_02FB;
			}
			lastCorner = true;
			funnelPath.Add(this.left[count - 1]);
			return true;
		}

		// Token: 0x04000112 RID: 274
		private readonly List<Vector3> left;

		// Token: 0x04000113 RID: 275
		private readonly List<Vector3> right;

		// Token: 0x04000114 RID: 276
		private List<TriangleMeshNode> nodes;

		// Token: 0x04000115 RID: 277
		public Vector3 exactStart;

		// Token: 0x04000116 RID: 278
		public Vector3 exactEnd;

		// Token: 0x04000117 RID: 279
		private NavmeshBase graph;

		// Token: 0x04000118 RID: 280
		private int currentNode;

		// Token: 0x04000119 RID: 281
		private Vector3 currentPosition;

		// Token: 0x0400011A RID: 282
		private int checkForDestroyedNodesCounter;

		// Token: 0x0400011B RID: 283
		private RichPath path;

		// Token: 0x0400011C RID: 284
		private int[] triBuffer = new int[3];

		// Token: 0x0400011D RID: 285
		public bool funnelSimplification = true;

		// Token: 0x0400011E RID: 286
		private static Queue<TriangleMeshNode> navmeshClampQueue = new Queue<TriangleMeshNode>();

		// Token: 0x0400011F RID: 287
		private static List<TriangleMeshNode> navmeshClampList = new List<TriangleMeshNode>();

		// Token: 0x04000120 RID: 288
		private static Dictionary<TriangleMeshNode, TriangleMeshNode> navmeshClampDict = new Dictionary<TriangleMeshNode, TriangleMeshNode>();
	}
}
