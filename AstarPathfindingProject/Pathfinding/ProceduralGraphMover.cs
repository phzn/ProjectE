using System;
using System.Collections.Generic;
using Pathfinding.Jobs;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200016E RID: 366
	[AddComponentMenu("Pathfinding/Procedural Graph Mover")]
	[HelpURL("https://arongranberg.com/astar/documentation/stable/proceduralgraphmover.html")]
	public class ProceduralGraphMover : VersionedMonoBehaviour
	{
		// Token: 0x06000B8B RID: 2955 RVA: 0x00040242 File Offset: 0x0003E442
		public void BindTarget(Transform currentTarget)
		{
			if (this.target != currentTarget)
			{
				this.targetGeneration++;
			}
			this.target = ((currentTarget != null) ? currentTarget : null);
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000B8C RID: 2956 RVA: 0x0004026E File Offset: 0x0003E46E
		// (set) Token: 0x06000B8D RID: 2957 RVA: 0x00040276 File Offset: 0x0003E476
		public bool updatingGraph { get; private set; }

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x00040280 File Offset: 0x0003E480
		public bool HasPendingRecastShift
		{
			get
			{
				if (!base.isActiveAndEnabled || this.target == null || this.graph == null || !this.graph.isScanned)
				{
					return false;
				}
				RecastGraph recastGraph = this.graph as RecastGraph;
				if (recastGraph == null)
				{
					return false;
				}
				Vector2Int vector2Int = ProceduralGraphMover.RecastGraphTileShift(recastGraph, this.target.position);
				return vector2Int.x != 0 || vector2Int.y != 0;
			}
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x000402F4 File Offset: 0x0003E4F4
		private void Start()
		{
			if (AstarPath.active == null)
			{
				throw new Exception("There is no AstarPath object in the scene");
			}
			if (this.graph == null)
			{
				if (this.graphIndex < 0)
				{
					throw new Exception("Graph index should not be negative");
				}
				if (this.graphIndex >= AstarPath.active.data.graphs.Length)
				{
					throw new Exception(string.Concat(new string[]
					{
						"The ProceduralGraphMover was configured to use graph index ",
						this.graphIndex.ToString(),
						", but only ",
						AstarPath.active.data.graphs.Length.ToString(),
						" graphs exist"
					}));
				}
				this.graph = AstarPath.active.data.graphs[this.graphIndex];
				if (!(this.graph is GridGraph) && !(this.graph is RecastGraph))
				{
					throw new Exception("The ProceduralGraphMover was configured to use graph index " + this.graphIndex.ToString() + " but that graph either does not exist or is not a GridGraph, LayerGridGraph or RecastGraph");
				}
				RecastGraph recastGraph = this.graph as RecastGraph;
				if (recastGraph != null && !recastGraph.useTiles)
				{
					Debug.LogWarning("The ProceduralGraphMover component only works with tiled recast graphs. Enable tiling in the recast graph inspector.", this);
				}
			}
			this.UpdateGraph(true);
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x00040423 File Offset: 0x0003E623
		private void OnDisable()
		{
			this.targetGeneration++;
			if (AstarPath.active != null)
			{
				AstarPath.active.FlushWorkItems();
			}
			this.updatingGraph = false;
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x00040454 File Offset: 0x0003E654
		private void Update()
		{
			if (AstarPath.active == null || this.graph == null || !this.graph.isScanned || this.target == null)
			{
				return;
			}
			GridGraph gridGraph = this.graph as GridGraph;
			if (gridGraph != null)
			{
				Vector3 vector = gridGraph.transform.InverseTransform(gridGraph.center);
				Vector3 vector2 = gridGraph.transform.InverseTransform(this.target.position);
				if (VectorMath.SqrDistanceXZ(vector, vector2) > this.updateDistance * this.updateDistance)
				{
					this.UpdateGraph(true);
					return;
				}
				return;
			}
			else
			{
				if (this.graph is RecastGraph)
				{
					this.UpdateGraph(true);
					return;
				}
				throw new Exception("ProceduralGraphMover cannot be used with graphs of type " + this.graph.GetType().Name);
			}
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x0004051C File Offset: 0x0003E71C
		public void UpdateGraph(bool async = true)
		{
			if (!base.enabled)
			{
				throw new InvalidOperationException("This component has been disabled");
			}
			if (AstarPath.active == null || this.graph == null || this.target == null)
			{
				return;
			}
			if (this.updatingGraph)
			{
				return;
			}
			Transform transform = this.target;
			Vector3 position = transform.position;
			GridGraph gridGraph = this.graph as GridGraph;
			if (gridGraph != null)
			{
				this.UpdateGridGraph(gridGraph, transform, position, async);
				return;
			}
			RecastGraph recastGraph = this.graph as RecastGraph;
			if (recastGraph != null)
			{
				Vector2Int vector2Int = ProceduralGraphMover.RecastGraphTileShift(recastGraph, position);
				if (this.maxRecastTilesPerMove > 0)
				{
					vector2Int.x = Mathf.Clamp(vector2Int.x, -this.maxRecastTilesPerMove, this.maxRecastTilesPerMove);
					vector2Int.y = Mathf.Clamp(vector2Int.y, -this.maxRecastTilesPerMove, this.maxRecastTilesPerMove);
				}
				if (vector2Int.x != 0 || vector2Int.y != 0)
				{
					this.updatingGraph = true;
					this.UpdateRecastGraph(recastGraph, transform, vector2Int, async);
				}
			}
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x00040616 File Offset: 0x0003E816
		private void UpdateGridGraph(GridGraph graph, Transform requestedTarget, Vector3 targetPosition, bool async)
		{
			AstarPath.active.AddWorkItem(this.CreateGridMove(graph, requestedTarget, targetPosition));
			if (!async)
			{
				AstarPath.active.FlushWorkItems();
			}
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x0004063C File Offset: 0x0003E83C
		private AstarWorkItem CreateGridMove(GridGraph graph, Transform requestedTarget, Vector3 targetPosition)
		{
			this.updatingGraph = true;
			int generation = this.targetGeneration;
			AstarPath owner = AstarPath.active;
			List<ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>> promises = new List<ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>>();
			return new AstarWorkItem(delegate(IWorkItemContext ctx)
			{
				if (!this.CanStartMove(owner, graph, requestedTarget, generation))
				{
					return;
				}
				Vector3 vector = graph.transform.InverseTransformVector(targetPosition - graph.center);
				int num = Mathf.RoundToInt(vector.x);
				int num2 = Mathf.RoundToInt(vector.z);
				if (num != 0 || num2 != 0)
				{
					IGraphUpdatePromise graphUpdatePromise = graph.TranslateInDirection(num, num2);
					promises.Add(new ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>(graphUpdatePromise, graphUpdatePromise.Prepare()));
				}
			}, delegate(IWorkItemContext ctx, bool force)
			{
				if (GraphUpdateProcessor.ProcessGraphUpdatePromises(promises, ctx, force ? TimeSlice.Infinite : TimeSlice.MillisFromNow(2f)) == -1)
				{
					this.updatingGraph = false;
					return true;
				}
				return false;
			});
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x000406B4 File Offset: 0x0003E8B4
		private bool CanStartMove(AstarPath owner, NavGraph requestedGraph, Transform requestedTarget, int generation)
		{
			return this != null && base.isActiveAndEnabled && owner != null && AstarPath.active == owner && generation == this.targetGeneration && this.graph == requestedGraph && requestedTarget != null && this.target == requestedTarget;
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x00040714 File Offset: 0x0003E914
		private static Vector2Int RecastGraphTileShift(RecastGraph graph, Vector3 targetCenter)
		{
			Vector3 vector = graph.transform.InverseTransform(targetCenter) - graph.transform.InverseTransform(graph.forcedBoundsCenter);
			if (Mathf.Abs(vector.x) > Mathf.Abs(vector.z))
			{
				vector.z = 0f;
			}
			else
			{
				vector.x = 0f;
			}
			return new Vector2Int((int)(Mathf.Max(0f, Mathf.Abs(vector.x) / graph.TileWorldSizeX + 0.5f - 0.2f) * Mathf.Sign(vector.x)), (int)(Mathf.Max(0f, Mathf.Abs(vector.z) / graph.TileWorldSizeZ + 0.5f - 0.2f) * Mathf.Sign(vector.z)));
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x000407E5 File Offset: 0x0003E9E5
		private void UpdateRecastGraph(RecastGraph graph, Transform requestedTarget, Vector2Int delta, bool async)
		{
			AstarPath.active.AddWorkItem(this.CreateRecastMove(graph, requestedTarget, delta));
			if (!async)
			{
				AstarPath.active.FlushWorkItems();
			}
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00040808 File Offset: 0x0003EA08
		private AstarWorkItem CreateRecastMove(RecastGraph graph, Transform requestedTarget, Vector2Int delta)
		{
			this.updatingGraph = true;
			int generation = this.targetGeneration;
			AstarPath owner = AstarPath.active;
			List<ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>> promises = new List<ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>>();
			return new AstarWorkItem(delegate(IWorkItemContext ctx)
			{
				if (!this.CanStartMove(owner, graph, requestedTarget, generation))
				{
					return;
				}
				IGraphUpdatePromise graphUpdatePromise = graph.TranslateInDirection(delta.x, delta.y);
				promises.Add(new ValueTuple<IGraphUpdatePromise, IEnumerator<JobHandle>>(graphUpdatePromise, graphUpdatePromise.Prepare()));
			}, delegate(IWorkItemContext ctx, bool force)
			{
				if (GraphUpdateProcessor.ProcessGraphUpdatePromises(promises, ctx, force ? TimeSlice.Infinite : TimeSlice.MillisFromNow(2f)) == -1)
				{
					this.updatingGraph = false;
					return true;
				}
				return false;
			});
		}

		// Token: 0x0400077D RID: 1917
		public float updateDistance = 10f;

		// Token: 0x0400077E RID: 1918
		public Transform target;

		// Token: 0x0400077F RID: 1919
		private int targetGeneration;

		// Token: 0x04000780 RID: 1920
		public int maxRecastTilesPerMove;

		// Token: 0x04000782 RID: 1922
		public NavGraph graph;

		// Token: 0x04000783 RID: 1923
		[HideInInspector]
		public int graphIndex;
	}
}
