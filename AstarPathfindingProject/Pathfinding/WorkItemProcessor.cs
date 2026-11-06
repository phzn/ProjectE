using System;
using System.Collections.Generic;
using Pathfinding.Drawing;
using Pathfinding.Sync;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000096 RID: 150
	internal class WorkItemProcessor : IWorkItemContext, IGraphUpdateContext
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060004F8 RID: 1272 RVA: 0x000193D0 File Offset: 0x000175D0
		// (remove) Token: 0x060004F9 RID: 1273 RVA: 0x00019408 File Offset: 0x00017608
		public event Action OnGraphsUpdated;

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x0001943D File Offset: 0x0001763D
		// (set) Token: 0x060004FB RID: 1275 RVA: 0x00019445 File Offset: 0x00017645
		public bool workItemsInProgressRightNow { get; private set; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x0001944E File Offset: 0x0001764E
		public bool anyQueued
		{
			get
			{
				return this.workItems.Count > 0;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x0001945E File Offset: 0x0001765E
		// (set) Token: 0x060004FE RID: 1278 RVA: 0x00019466 File Offset: 0x00017666
		public bool workItemsInProgress { get; private set; }

		// Token: 0x060004FF RID: 1279 RVA: 0x0001946F File Offset: 0x0001766F
		public void VisualizeOriginalGraphUpdateBounds(Bounds bounds)
		{
			if ((this.astar.graphUpdateDebugMode & GraphUpdateDebugMode.VisualizeOriginalBounds) != GraphUpdateDebugMode.Nothing)
			{
				this.boundsVisualizations.Add(new WorkItemProcessor.BoundsVisualization(bounds, WorkItemProcessor.BoundsVisualization.BoundsVisualizationType.OriginalBounds));
			}
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00003788 File Offset: 0x00001988
		void IWorkItemContext.QueueFloodFill()
		{
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00019492 File Offset: 0x00017692
		void IWorkItemContext.PreUpdate()
		{
			if (!this.preUpdateEventSent && !this.astar.isScanning)
			{
				this.preUpdateEventSent = true;
				GraphModifier.TriggerEvent(GraphModifier.EventType.PreUpdate);
			}
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x000194B6 File Offset: 0x000176B6
		void IWorkItemContext.SetGraphDirty(NavGraph graph)
		{
			this.astar.DirtyBounds(graph.bounds);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x000194C9 File Offset: 0x000176C9
		void IGraphUpdateContext.DirtyBounds(Bounds bounds)
		{
			this.astar.DirtyBounds(bounds);
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x000194D7 File Offset: 0x000176D7
		internal void DirtyBounds(Bounds bounds)
		{
			if ((this.astar.graphUpdateDebugMode & GraphUpdateDebugMode.VisualizeAffectedBounds) != GraphUpdateDebugMode.Nothing)
			{
				this.boundsVisualizations.Add(new WorkItemProcessor.BoundsVisualization(bounds, WorkItemProcessor.BoundsVisualization.BoundsVisualizationType.AffectedBounds));
			}
			this.anyGraphsDirty = true;
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00019504 File Offset: 0x00017704
		public void EnsureValidFloodFill()
		{
			using (WorkItemProcessor.MapBoostFloodFillMarker.Auto())
			{
				this.astar.hierarchicalGraph.RecalculateIfNecessary();
			}
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00019550 File Offset: 0x00017750
		public WorkItemProcessor(AstarPath astar)
		{
			this.astar = astar;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0001957C File Offset: 0x0001777C
		public void AddWorkItem(AstarWorkItem item)
		{
			this.workItems.Enqueue(item);
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0001958C File Offset: 0x0001778C
		private bool ProcessWorkItems(bool force, bool sendEvents)
		{
			bool flag4;
			using (WorkItemProcessor.MapBoostWorkItemsMarker.Auto())
			{
				if (this.workItemsInProgressRightNow)
				{
					throw new Exception("Processing work items recursively. Please do not wait for other work items to be completed inside work items. If you think this is not caused by any of your scripts, this might be a bug.");
				}
				RWLock.LockSync lockSync;
				using (WorkItemProcessor.MapBoostGraphLockMarker.Auto())
				{
					lockSync = this.astar.LockGraphDataForWritingSync();
				}
				this.astar.data.LockGraphStructure(true);
				using (WorkItemProcessor.MapBoostPhysicsSyncMarker.Auto())
				{
					Physics.SyncTransforms();
					Physics2D.SyncTransforms();
				}
				this.workItemsInProgressRightNow = true;
				try
				{
					bool flag = false;
					bool flag2 = false;
					while (this.workItems.Count > 0)
					{
						if (!this.workItemsInProgress)
						{
							this.workItemsInProgress = true;
						}
						AstarWorkItem astarWorkItem = this.workItems[0];
						bool flag3;
						try
						{
							if (astarWorkItem.init != null)
							{
								astarWorkItem.init();
								astarWorkItem.init = null;
							}
							if (astarWorkItem.initWithContext != null)
							{
								astarWorkItem.initWithContext(this);
								astarWorkItem.initWithContext = null;
							}
							this.workItems[0] = astarWorkItem;
							if (astarWorkItem.update != null)
							{
								flag3 = astarWorkItem.update(force);
							}
							else
							{
								flag3 = astarWorkItem.updateWithContext == null || astarWorkItem.updateWithContext(this, force);
							}
						}
						catch
						{
							this.workItems.Dequeue();
							throw;
						}
						if (!flag3)
						{
							if (force)
							{
								Debug.LogError("Misbehaving WorkItem. 'force'=true but the work item did not complete.\nIf force=true is passed to a WorkItem it should always return true.");
							}
							flag = true;
							break;
						}
						this.workItems.Dequeue();
						flag2 = true;
					}
					if (sendEvents && flag2)
					{
						if (this.anyGraphsDirty)
						{
							GraphModifier.TriggerEvent(GraphModifier.EventType.PostUpdateBeforeAreaRecalculation);
						}
						using (WorkItemProcessor.MapBoostOffMeshRefreshMarker.Auto())
						{
							this.astar.offMeshLinks.Refresh();
						}
						this.EnsureValidFloodFill();
						if (this.anyGraphsDirty)
						{
							using (WorkItemProcessor.MapBoostPostUpdateMarker.Auto())
							{
								GraphModifier.TriggerEvent(GraphModifier.EventType.PostUpdate);
								if (this.OnGraphsUpdated != null)
								{
									this.OnGraphsUpdated();
								}
							}
						}
					}
					if (flag)
					{
						return false;
					}
				}
				finally
				{
					lockSync.Unlock();
					this.astar.data.UnlockGraphStructure();
					this.workItemsInProgressRightNow = false;
				}
				this.anyGraphsDirty = false;
				this.preUpdateEventSent = false;
				this.workItemsInProgress = false;
				flag4 = true;
			}
			return flag4;
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x000198A0 File Offset: 0x00017AA0
		public bool ProcessWorkItemsForScan(bool force)
		{
			return this.ProcessWorkItems(force, false);
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x000198AA File Offset: 0x00017AAA
		public bool ProcessWorkItemsForUpdate(bool force)
		{
			return this.ProcessWorkItems(force, true);
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x000198B4 File Offset: 0x00017AB4
		public unsafe void DrawGizmos()
		{
			if (!Application.isEditor && !this.astar.showGraphsInStandalonePlayer)
			{
				this.boundsVisualizations.Clear();
				return;
			}
			int frameCount = Time.frameCount;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			CommandBuilder commandBuilder = (Application.isEditor ? (*Draw.editor) : (*Draw.ingame));
			int num = 0;
			GraphUpdateDebugMode graphUpdateDebugMode = this.astar.graphUpdateDebugMode;
			using (commandBuilder.WithLineWidth(2f, true))
			{
				for (int i = 0; i < this.boundsVisualizations.Count; i++)
				{
					WorkItemProcessor.BoundsVisualization boundsVisualization = this.boundsVisualizations[i];
					bool flag = boundsVisualization.createdFrame == frameCount;
					Color color;
					bool flag2;
					if (boundsVisualization.type == WorkItemProcessor.BoundsVisualization.BoundsVisualizationType.OriginalBounds)
					{
						color = Palette.Colorbrewer.Set1.Yellow;
						flag2 = (graphUpdateDebugMode & GraphUpdateDebugMode.VisualizeOriginalBounds) > GraphUpdateDebugMode.Nothing;
					}
					else
					{
						if (boundsVisualization.type != WorkItemProcessor.BoundsVisualization.BoundsVisualizationType.AffectedBounds)
						{
							throw new ArgumentOutOfRangeException();
						}
						color = Palette.Colorbrewer.Set1.Orange;
						flag2 = (graphUpdateDebugMode & GraphUpdateDebugMode.VisualizeAffectedBounds) > GraphUpdateDebugMode.Nothing;
					}
					if (!flag)
					{
						color.a *= 0.5f * math.exp(-2f * (realtimeSinceStartup - boundsVisualization.createdTime));
					}
					if (color.a < 0.01f)
					{
						num = i + 1;
					}
					if (flag2)
					{
						commandBuilder.WireBox(boundsVisualization.bounds, color);
					}
				}
			}
			if ((graphUpdateDebugMode & GraphUpdateDebugMode.VisualizeOverTime) == GraphUpdateDebugMode.Nothing || !Application.isPlaying)
			{
				this.boundsVisualizations.Clear();
				return;
			}
			if (num > 0)
			{
				this.boundsVisualizations.RemoveRange(0, num);
			}
		}

		// Token: 0x04000331 RID: 817
		private static readonly ProfilerMarker MapBoostWorkItemsMarker = new ProfilerMarker("MapBoost.Nav.WorkItems");

		// Token: 0x04000332 RID: 818
		private static readonly ProfilerMarker MapBoostGraphLockMarker = new ProfilerMarker("MapBoost.Nav.GraphWriteFence");

		// Token: 0x04000333 RID: 819
		private static readonly ProfilerMarker MapBoostPhysicsSyncMarker = new ProfilerMarker("MapBoost.Nav.PhysicsSync");

		// Token: 0x04000334 RID: 820
		private static readonly ProfilerMarker MapBoostFloodFillMarker = new ProfilerMarker("MapBoost.Nav.FloodFill");

		// Token: 0x04000335 RID: 821
		private static readonly ProfilerMarker MapBoostOffMeshRefreshMarker = new ProfilerMarker("MapBoost.Nav.OffMeshRefresh");

		// Token: 0x04000336 RID: 822
		private static readonly ProfilerMarker MapBoostPostUpdateMarker = new ProfilerMarker("MapBoost.Nav.PostUpdate");

		// Token: 0x04000339 RID: 825
		private readonly AstarPath astar;

		// Token: 0x0400033A RID: 826
		private readonly WorkItemProcessor.IndexedQueue<AstarWorkItem> workItems = new WorkItemProcessor.IndexedQueue<AstarWorkItem>();

		// Token: 0x0400033B RID: 827
		private readonly List<WorkItemProcessor.BoundsVisualization> boundsVisualizations = new List<WorkItemProcessor.BoundsVisualization>();

		// Token: 0x0400033C RID: 828
		private bool anyGraphsDirty = true;

		// Token: 0x0400033D RID: 829
		private bool preUpdateEventSent;

		// Token: 0x02000097 RID: 151
		private struct BoundsVisualization
		{
			// Token: 0x0600050D RID: 1293 RVA: 0x00019A9F File Offset: 0x00017C9F
			public BoundsVisualization(Bounds bounds, WorkItemProcessor.BoundsVisualization.BoundsVisualizationType type)
			{
				this.bounds = bounds;
				this.type = type;
				this.createdTime = Time.realtimeSinceStartup;
				this.createdFrame = Time.frameCount;
			}

			// Token: 0x0400033F RID: 831
			public Bounds bounds;

			// Token: 0x04000340 RID: 832
			public float createdTime;

			// Token: 0x04000341 RID: 833
			public int createdFrame;

			// Token: 0x04000342 RID: 834
			public WorkItemProcessor.BoundsVisualization.BoundsVisualizationType type;

			// Token: 0x02000098 RID: 152
			public enum BoundsVisualizationType
			{
				// Token: 0x04000344 RID: 836
				OriginalBounds,
				// Token: 0x04000345 RID: 837
				AffectedBounds
			}
		}

		// Token: 0x02000099 RID: 153
		private class IndexedQueue<T>
		{
			// Token: 0x17000104 RID: 260
			public T this[int index]
			{
				get
				{
					if (index < 0 || index >= this.Count)
					{
						throw new IndexOutOfRangeException();
					}
					return this.buffer[(this.start + index) % this.buffer.Length];
				}
				set
				{
					if (index < 0 || index >= this.Count)
					{
						throw new IndexOutOfRangeException();
					}
					this.buffer[(this.start + index) % this.buffer.Length] = value;
				}
			}

			// Token: 0x17000105 RID: 261
			// (get) Token: 0x06000510 RID: 1296 RVA: 0x00019B28 File Offset: 0x00017D28
			// (set) Token: 0x06000511 RID: 1297 RVA: 0x00019B30 File Offset: 0x00017D30
			public int Count { get; private set; }

			// Token: 0x06000512 RID: 1298 RVA: 0x00019B3C File Offset: 0x00017D3C
			public void Enqueue(T item)
			{
				if (this.Count == this.buffer.Length)
				{
					T[] array = new T[this.buffer.Length * 2];
					for (int i = 0; i < this.Count; i++)
					{
						array[i] = this[i];
					}
					this.buffer = array;
					this.start = 0;
				}
				this.buffer[(this.start + this.Count) % this.buffer.Length] = item;
				int count = this.Count;
				this.Count = count + 1;
			}

			// Token: 0x06000513 RID: 1299 RVA: 0x00019BC8 File Offset: 0x00017DC8
			public T Dequeue()
			{
				if (this.Count == 0)
				{
					throw new InvalidOperationException();
				}
				T t = this.buffer[this.start];
				this.start = (this.start + 1) % this.buffer.Length;
				int count = this.Count;
				this.Count = count - 1;
				return t;
			}

			// Token: 0x04000346 RID: 838
			private T[] buffer = new T[4];

			// Token: 0x04000347 RID: 839
			private int start;
		}
	}
}
