using System;
using System.Runtime.CompilerServices;
using Pathfinding.Drawing;
using Pathfinding.Serialization;
using Pathfinding.Util;
using Unity.Mathematics;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000DD RID: 221
	public abstract class NavGraph : IGraphInternals
	{
		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600075C RID: 1884 RVA: 0x00025772 File Offset: 0x00023972
		internal bool exists
		{
			get
			{
				return this.active != null;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600075D RID: 1885
		public abstract bool isScanned { get; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x00019DC2 File Offset: 0x00017FC2
		public virtual bool persistent
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600075F RID: 1887 RVA: 0x00019DC2 File Offset: 0x00017FC2
		public virtual bool showInInspector
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x00025780 File Offset: 0x00023980
		public virtual Bounds bounds
		{
			get
			{
				return new Bounds(Vector3.zero, Vector3.positiveInfinity);
			}
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x00025794 File Offset: 0x00023994
		public virtual int CountNodes()
		{
			int num = 0;
			this.GetNodes<int>(delegate(GraphNode _, ref int count)
			{
				count++;
			}, ref num);
			return num;
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x000257CC File Offset: 0x000239CC
		public void GetNodes(Func<GraphNode, bool> action)
		{
			bool cont = true;
			this.GetNodes(delegate(GraphNode node)
			{
				if (cont)
				{
					cont &= action(node);
				}
			});
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x000257FF File Offset: 0x000239FF
		public virtual void GetNodes(Action<GraphNode> action)
		{
			this.GetNodes<Action<GraphNode>>(delegate(GraphNode node, ref Action<GraphNode> action)
			{
				action(node);
			}, ref action);
		}

		// Token: 0x06000764 RID: 1892
		public abstract void GetNodes<T>(GraphNode.NodeActionWithData<T> action, ref T data);

		// Token: 0x06000765 RID: 1893 RVA: 0x00025828 File Offset: 0x00023A28
		public virtual bool IsPointOnNavmesh(Vector3 position)
		{
			NearestNodeConstraint none = NearestNodeConstraint.None;
			none.distanceMetric = DistanceMetric.ClosestAsSeenFromAbove();
			none.maxDistanceSqr = 0.0001f;
			NNInfo nearest = this.GetNearest(position, none);
			return nearest.node != null && nearest.node.Walkable;
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x00019DC2 File Offset: 0x00017FC2
		public virtual bool IsInsideBounds(Vector3 point)
		{
			return true;
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x00025871 File Offset: 0x00023A71
		protected void AssertSafeToUpdateGraph()
		{
			if (!this.active.IsAnyWorkItemInProgress && !this.active.isScanning)
			{
				throw new Exception("Trying to update graphs when it is not safe to do so. Graph updates must be done inside a work item or when a graph is being scanned. See AstarPath.AddWorkItem");
			}
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00025898 File Offset: 0x00023A98
		protected void DirtyBounds(Bounds bounds)
		{
			this.active.DirtyBounds(bounds);
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x000258A6 File Offset: 0x00023AA6
		public virtual void RelocateNodes(Matrix4x4 deltaMatrix)
		{
			this.AssertSafeToUpdateGraph();
			this.GetNodes<Matrix4x4>(delegate(GraphNode node, ref Matrix4x4 deltaMatrix)
			{
				node.position = (Int3)deltaMatrix.MultiplyPoint((Vector3)node.position);
			}, ref deltaMatrix);
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00005C0D File Offset: 0x00003E0D
		public virtual float NearestNodeDistanceSqrLowerBound(Vector3 position, ref NearestNodeConstraint constraint)
		{
			return 0f;
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x000258D8 File Offset: 0x00023AD8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public NNInfo GetNearest(Vector3 position)
		{
			NearestNodeConstraint none = NearestNodeConstraint.None;
			return this.GetNearest(position, ref none);
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x000258F4 File Offset: 0x00023AF4
		[Obsolete("Use the overload that takes a NearestNodeConstraint instead. See the migration guide for version 5.4 for more details.")]
		public NNInfo GetNearest(Vector3 position, NNConstraint constraint)
		{
			return this.GetNearest(position, (constraint != null) ? constraint.ToNearestNodeConstraint() : NearestNodeConstraint.None);
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0002590D File Offset: 0x00023B0D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public NNInfo GetNearest(Vector3 position, NearestNodeConstraint constraint)
		{
			return this.GetNearest(position, ref constraint);
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00025918 File Offset: 0x00023B18
		public virtual NNInfo GetNearest(Vector3 position, ref NearestNodeConstraint constraint)
		{
			float maxDistanceSqr = constraint.maxDistanceSqrOrDefault(this.active);
			GraphNode minNode = null;
			NearestNodeConstraint constraintCopy = constraint;
			this.GetNodes(delegate(GraphNode node)
			{
				float sqrMagnitude = (position - (Vector3)node.position).sqrMagnitude;
				if (sqrMagnitude < maxDistanceSqr && constraintCopy.Suitable(node))
				{
					maxDistanceSqr = sqrMagnitude;
					minNode = node;
				}
			});
			if (minNode == null)
			{
				return NNInfo.Empty;
			}
			return new NNInfo(minNode, (Vector3)minNode.position, maxDistanceSqr);
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x00025998 File Offset: 0x00023B98
		[Obsolete("Use GetNearest instead")]
		public NNInfo GetNearestForce(Vector3 position, NNConstraint constraint)
		{
			return this.GetNearest(position, constraint);
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x000259A2 File Offset: 0x00023BA2
		[Obsolete("Use the overload that takes a NearestNodeConstraint instead. See the migration guide for version 5.4 for more details.")]
		public NNInfo RandomPointOnSurface(NNConstraint nnConstraint, bool highQuality = true)
		{
			return this.RandomPointOnSurface((nnConstraint != null) ? nnConstraint.ToNearestNodeConstraint() : NearestNodeConstraint.None, highQuality);
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x000259BB File Offset: 0x00023BBB
		public NNInfo RandomPointOnSurface(bool highQuality = true)
		{
			return this.RandomPointOnSurface(NearestNodeConstraint.None, highQuality);
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x000259CC File Offset: 0x00023BCC
		public virtual NNInfo RandomPointOnSurface(NearestNodeConstraint constraint, bool highQuality = true)
		{
			GraphNode bestNode = null;
			float weight = 0f;
			bool allNodesAreSuitable = constraint.allNodesAreSuitable;
			this.GetNodes(delegate(GraphNode node)
			{
				if (allNodesAreSuitable || constraint.Suitable(node))
				{
					float num = node.SurfaceArea();
					if (num <= 0f)
					{
						num = 0.001f;
					}
					weight += num;
					if (bestNode == null || global::UnityEngine.Random.value < num / weight)
					{
						bestNode = node;
					}
				}
			});
			if (bestNode == null)
			{
				return NNInfo.Empty;
			}
			return new NNInfo(bestNode, bestNode.RandomPointOnSurface(), 0f);
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x00025A44 File Offset: 0x00023C44
		protected virtual void OnDestroy()
		{
			this.DestroyAllNodes();
			this.DisposeUnmanagedData();
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x00003788 File Offset: 0x00001988
		protected virtual void DisposeUnmanagedData()
		{
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x00025A52 File Offset: 0x00023C52
		protected virtual void DestroyAllNodes()
		{
			this.GetNodes(delegate(GraphNode node)
			{
				node.Destroy();
			});
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00003792 File Offset: 0x00001992
		public virtual IGraphSnapshot Snapshot(Bounds bounds)
		{
			return null;
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00025A79 File Offset: 0x00023C79
		public void Scan()
		{
			this.active.Scan(this);
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0001C8BA File Offset: 0x0001AABA
		protected virtual IGraphUpdatePromise ScanInternal()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x00025A87 File Offset: 0x00023C87
		protected virtual IGraphUpdatePromise ScanInternal(bool async)
		{
			return this.ScanInternal();
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x00003788 File Offset: 0x00001988
		protected virtual void SerializeExtraInfo(GraphSerializationContext ctx)
		{
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00003788 File Offset: 0x00001988
		protected virtual void DeserializeExtraInfo(GraphSerializationContext ctx)
		{
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00003788 File Offset: 0x00001988
		protected virtual void PostDeserialization(GraphSerializationContext ctx)
		{
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x00025A90 File Offset: 0x00023C90
		public virtual void OnDrawGizmos(DrawingData gizmos, bool drawNodes, RedrawScope redrawScope, bool renderInGame)
		{
			if (!drawNodes)
			{
				return;
			}
			NodeHasher nodeHasher = new NodeHasher(this.active);
			this.GetNodes<NodeHasher>(delegate(GraphNode node, ref NodeHasher hasher)
			{
				hasher.HashNode(node);
			}, ref nodeHasher);
			if (!gizmos.Draw(nodeHasher, redrawScope))
			{
				GraphGizmoHelper gizmoHelper = GraphGizmoHelper.GetGizmoHelper(gizmos, this.active, nodeHasher, redrawScope, renderInGame);
				if (gizmoHelper.showSearchTree)
				{
					gizmoHelper.builder.PushLineWidth(2f, true);
				}
				this.GetNodes<GraphGizmoHelper>(delegate(GraphNode node, ref GraphGizmoHelper helper)
				{
					helper.DrawConnections(node);
				}, ref gizmoHelper);
				if (gizmoHelper.showSearchTree)
				{
					gizmoHelper.builder.PopLineWidth();
				}
				gizmoHelper.Dispose();
			}
			if (this.active.showUnwalkableNodes)
			{
				this.DrawUnwalkableNodes(gizmos, this.active.unwalkableNodeDebugSize, redrawScope, renderInGame);
			}
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00025B78 File Offset: 0x00023D78
		protected void DrawUnwalkableNodes(DrawingData gizmos, float size, RedrawScope redrawScope, bool renderInGame)
		{
			DrawingData.Hasher hasher2 = default(DrawingData.Hasher);
			hasher2.Add<NavGraph>(this);
			this.GetNodes<DrawingData.Hasher>(delegate(GraphNode node, ref DrawingData.Hasher hasher)
			{
				hasher.Add<bool>(node.Walkable);
				if (!node.Walkable)
				{
					hasher.Add<Int3>(node.position);
				}
			}, ref hasher2);
			if (!gizmos.Draw(hasher2, redrawScope))
			{
				CommandBuilder builder = gizmos.GetBuilder(hasher2, default(RedrawScope), renderInGame);
				using (builder.WithColor(AstarColor.UnwalkableNode))
				{
					ValueTuple<CommandBuilder, float> valueTuple = new ValueTuple<CommandBuilder, float>(builder, size);
					this.GetNodes<ValueTuple<CommandBuilder, float>>(delegate(GraphNode node, ref ValueTuple<CommandBuilder, float> ctx)
					{
						if (!node.Walkable)
						{
							ctx.Item1.SolidBox((Vector3)node.position, new float3(ctx.Item2, ctx.Item2, ctx.Item2));
						}
					}, ref valueTuple);
				}
				builder.Dispose();
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x00025C40 File Offset: 0x00023E40
		// (set) Token: 0x06000780 RID: 1920 RVA: 0x00025C48 File Offset: 0x00023E48
		string IGraphInternals.SerializedEditorSettings
		{
			get
			{
				return this.serializedEditorSettings;
			}
			set
			{
				this.serializedEditorSettings = value;
			}
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x00025C51 File Offset: 0x00023E51
		void IGraphInternals.OnDestroy()
		{
			this.OnDestroy();
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x00025C59 File Offset: 0x00023E59
		void IGraphInternals.DisposeUnmanagedData()
		{
			this.DisposeUnmanagedData();
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x00025C61 File Offset: 0x00023E61
		void IGraphInternals.DestroyAllNodes()
		{
			this.DestroyAllNodes();
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x00025C69 File Offset: 0x00023E69
		IGraphUpdatePromise IGraphInternals.ScanInternal(bool async)
		{
			return this.ScanInternal(async);
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x00025C72 File Offset: 0x00023E72
		void IGraphInternals.SerializeExtraInfo(GraphSerializationContext ctx)
		{
			this.SerializeExtraInfo(ctx);
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x00025C7B File Offset: 0x00023E7B
		void IGraphInternals.DeserializeExtraInfo(GraphSerializationContext ctx)
		{
			this.DeserializeExtraInfo(ctx);
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00025C84 File Offset: 0x00023E84
		void IGraphInternals.PostDeserialization(GraphSerializationContext ctx)
		{
			this.PostDeserialization(ctx);
		}

		// Token: 0x040004C9 RID: 1225
		public AstarPath active;

		// Token: 0x040004CA RID: 1226
		[JsonMember]
		public Guid guid;

		// Token: 0x040004CB RID: 1227
		[JsonMember]
		public uint initialPenalty;

		// Token: 0x040004CC RID: 1228
		[JsonMember]
		public bool open;

		// Token: 0x040004CD RID: 1229
		public uint graphIndex;

		// Token: 0x040004CE RID: 1230
		[JsonMember]
		public string name;

		// Token: 0x040004CF RID: 1231
		[JsonMember]
		public bool drawGizmos = true;

		// Token: 0x040004D0 RID: 1232
		[JsonMember]
		public bool infoScreenOpen;

		// Token: 0x040004D1 RID: 1233
		[JsonMember]
		private string serializedEditorSettings;
	}
}
