using System;
using System.Collections.Generic;
using Pathfinding.Collections;
using Pathfinding.Sync;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Profiling;
using UnityEngine;

namespace Pathfinding.Graphs.Navmesh
{
	// Token: 0x020001D4 RID: 468
	[Serializable]
	public class NavmeshUpdates
	{
		// Token: 0x06000D1A RID: 3354 RVA: 0x0004B320 File Offset: 0x00049520
		private static Rect ExpandedXZBounds(Bounds bounds)
		{
			Vector3 min = bounds.min;
			Vector3 size = bounds.size;
			Rect rect = new Rect(min.x, min.z, size.x, size.z);
			rect.xMin -= 0.020000001f;
			rect.yMin -= 0.020000001f;
			rect.xMax += 0.020000001f;
			rect.yMax += 0.020000001f;
			return rect;
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x0004B3A9 File Offset: 0x000495A9
		internal void OnEnable()
		{
			this.lastUpdateTime = float.NegativeInfinity;
			NavmeshClipper.RefreshEnabledList();
			NavmeshClipper.AddEnableCallback(new Action<NavmeshClipper>(this.HandleOnEnableCallback), new Action<NavmeshClipper>(this.HandleOnDisableCallback));
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x0004B3D8 File Offset: 0x000495D8
		internal void OnDisable()
		{
			NavmeshClipper.RemoveEnableCallback(new Action<NavmeshClipper>(this.HandleOnEnableCallback), new Action<NavmeshClipper>(this.HandleOnDisableCallback));
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x0004B3F8 File Offset: 0x000495F8
		public void ForceUpdateAround(NavmeshClipper clipper)
		{
			for (int i = 0; i < this.listeners.Count; i++)
			{
				this.listeners[i].Dirty(clipper);
			}
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x0004B430 File Offset: 0x00049630
		public void DiscardPending()
		{
			for (int i = 0; i < this.listeners.Count; i++)
			{
				this.listeners[i].DiscardPending();
			}
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0004B464 File Offset: 0x00049664
		private void HandleOnEnableCallback(NavmeshClipper obj)
		{
			for (int i = 0; i < this.listeners.Count; i++)
			{
				this.listeners[i].AddClipper(obj);
			}
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0004B49C File Offset: 0x0004969C
		private void HandleOnDisableCallback(NavmeshClipper obj)
		{
			for (int i = 0; i < this.listeners.Count; i++)
			{
				this.listeners[i].RemoveClipper(obj);
			}
			this.lastUpdateTime = float.NegativeInfinity;
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x0004B4DC File Offset: 0x000496DC
		private void AddListener(NavmeshUpdates.NavmeshUpdateSettings listener)
		{
			this.listeners.Add(listener);
			for (int i = 0; i < NavmeshClipper.allEnabled.Count; i++)
			{
				listener.AddClipper(NavmeshClipper.allEnabled[i]);
			}
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x0004B51B File Offset: 0x0004971B
		private void RemoveListener(NavmeshUpdates.NavmeshUpdateSettings listener)
		{
			this.listeners.Remove(listener);
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x0004B52C File Offset: 0x0004972C
		internal void Update()
		{
			if (this.astar.isScanning)
			{
				return;
			}
			bool flag = false;
			this.RefreshEnabledState();
			for (int i = 0; i < this.listeners.Count; i++)
			{
				flag |= this.listeners[i].anyTilesDirty;
			}
			if ((this.updateInterval >= 0f && Time.realtimeSinceStartup - this.lastUpdateTime > this.updateInterval) || flag)
			{
				this.ScheduleTileUpdates();
			}
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x0004B5A7 File Offset: 0x000497A7
		public void ForceUpdate()
		{
			this.RefreshEnabledState();
			this.ScheduleTileUpdates();
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x0004B5B8 File Offset: 0x000497B8
		private void RefreshEnabledState()
		{
			NavGraph[] graphs = this.astar.graphs;
			for (int i = 0; i < graphs.Length; i++)
			{
				NavmeshBase navmeshBase = graphs[i] as NavmeshBase;
				if (navmeshBase != null)
				{
					bool flag = navmeshBase.enableNavmeshCutting && navmeshBase.isScanned;
					if (navmeshBase.navmeshUpdateData.enabled != flag)
					{
						if (flag)
						{
							navmeshBase.navmeshUpdateData.Enable();
						}
						else
						{
							navmeshBase.navmeshUpdateData.Disable();
						}
					}
				}
			}
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x0004B628 File Offset: 0x00049828
		private void ScheduleTileUpdates()
		{
			this.lastUpdateTime = Time.realtimeSinceStartup;
			foreach (NavmeshUpdates.NavmeshUpdateSettings navmeshUpdateSettings in this.listeners)
			{
				if (navmeshUpdateSettings.attachedToGraph)
				{
					GridLookup<NavmeshClipper>.Root allItems = navmeshUpdateSettings.clipperLookup.AllItems;
					if (!navmeshUpdateSettings.anyTilesDirty)
					{
						bool flag = false;
						for (GridLookup<NavmeshClipper>.Root root = allItems; root != null; root = root.next)
						{
							if (root.obj.RequiresUpdate(root))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							continue;
						}
					}
					float navmeshCuttingCharacterRadius = navmeshUpdateSettings.graph.NavmeshCuttingCharacterRadius;
					bool flag2 = (this.astar.graphUpdateDebugMode & GraphUpdateDebugMode.VisualizeOriginalBounds) != GraphUpdateDebugMode.Nothing && (Application.isEditor || this.astar.showGraphsInStandalonePlayer);
					for (GridLookup<NavmeshClipper>.Root root2 = allItems; root2 != null; root2 = root2.next)
					{
						if (root2.obj.RequiresUpdate(root2))
						{
							navmeshUpdateSettings.MarkTilesDirty(root2.previousBounds);
							Bounds bounds = root2.obj.GetBounds(navmeshUpdateSettings.tileLayout.transform, navmeshCuttingCharacterRadius);
							if (flag2)
							{
								this.astar.VisualizeOriginalGraphUpdateBounds(navmeshUpdateSettings.tileLayout.transform.Transform(bounds));
							}
							Rect rect = NavmeshUpdates.ExpandedXZBounds(bounds);
							IntRect touchingTilesInGraphSpace = navmeshUpdateSettings.tileLayout.GetTouchingTilesInGraphSpace(rect);
							navmeshUpdateSettings.clipperLookup.Move(root2.obj, touchingTilesInGraphSpace);
							navmeshUpdateSettings.MarkTilesDirty(touchingTilesInGraphSpace);
							root2.obj.NotifyUpdated(root2);
						}
					}
					navmeshUpdateSettings.ScheduleDirtyTilesReload();
				}
			}
		}

		// Token: 0x040008CE RID: 2254
		private static readonly ProfilerMarker MapBoostCutReloadMarker = new ProfilerMarker("MapBoost.Nav.CutReload");

		// Token: 0x040008CF RID: 2255
		private static readonly ProfilerMarker MapBoostCutFenceMarker = new ProfilerMarker("MapBoost.Nav.CutFence");

		// Token: 0x040008D0 RID: 2256
		private static readonly ProfilerMarker MapBoostCutApplyMarker = new ProfilerMarker("MapBoost.Nav.CutApply");

		// Token: 0x040008D1 RID: 2257
		public static bool MapBoostAsyncCutReload = true;

		// Token: 0x040008D2 RID: 2258
		public float updateInterval;

		// Token: 0x040008D3 RID: 2259
		internal AstarPath astar;

		// Token: 0x040008D4 RID: 2260
		private List<NavmeshUpdates.NavmeshUpdateSettings> listeners = new List<NavmeshUpdates.NavmeshUpdateSettings>();

		// Token: 0x040008D5 RID: 2261
		private float lastUpdateTime = float.NegativeInfinity;

		// Token: 0x020001D5 RID: 469
		public class NavmeshUpdateSettings : IDisposable
		{
			// Token: 0x17000206 RID: 518
			// (get) Token: 0x06000D29 RID: 3369 RVA: 0x0004B827 File Offset: 0x00049A27
			public bool MapBoostCutReloadPending
			{
				get
				{
					return this.mapBoostAsyncQueued || this.mapBoostCutScheduled;
				}
			}

			// Token: 0x17000207 RID: 519
			// (get) Token: 0x06000D2A RID: 3370 RVA: 0x0004B839 File Offset: 0x00049A39
			// (set) Token: 0x06000D2B RID: 3371 RVA: 0x0004B841 File Offset: 0x00049A41
			public bool attachedToGraph { get; private set; }

			// Token: 0x17000208 RID: 520
			// (get) Token: 0x06000D2C RID: 3372 RVA: 0x0004B84A File Offset: 0x00049A4A
			public bool enabled
			{
				get
				{
					return this.clipperLookup != null;
				}
			}

			// Token: 0x17000209 RID: 521
			// (get) Token: 0x06000D2D RID: 3373 RVA: 0x0004B855 File Offset: 0x00049A55
			public bool anyTilesDirty
			{
				get
				{
					return this.dirtyTileCoordinates.Count > 0;
				}
			}

			// Token: 0x06000D2E RID: 3374 RVA: 0x0004B865 File Offset: 0x00049A65
			private void AssertEnabled()
			{
				if (!this.enabled)
				{
					throw new InvalidOperationException("This method cannot be called when the NavmeshUpdateSettings is disabled");
				}
			}

			// Token: 0x06000D2F RID: 3375 RVA: 0x0004B87A File Offset: 0x00049A7A
			public NavmeshUpdateSettings(NavmeshBase graph)
			{
				this.graph = graph;
				this.dirtyTiles = default(UnsafeBitArray);
			}

			// Token: 0x06000D30 RID: 3376 RVA: 0x0004B8A0 File Offset: 0x00049AA0
			public NavmeshUpdateSettings(NavmeshBase graph, TileLayout tileLayout)
			{
				this.graph = graph;
				if (graph.enableNavmeshCutting)
				{
					this.SetLayout(tileLayout);
				}
			}

			// Token: 0x06000D31 RID: 3377 RVA: 0x0004B8C9 File Offset: 0x00049AC9
			public void UpdateLayoutFromGraph()
			{
				if (this.enabled)
				{
					this.ForceUpdateLayoutFromGraph();
				}
			}

			// Token: 0x06000D32 RID: 3378 RVA: 0x0004B8DC File Offset: 0x00049ADC
			internal void TranslateWorldOrigin(Vector3 delta)
			{
				if (!this.enabled)
				{
					return;
				}
				this.tileLayout = TileLayout.FromGraph(this.graph);
				for (GridLookup<NavmeshClipper>.Root root = this.clipperLookup.AllItems; root != null; root = root.next)
				{
					root.previousPosition += delta;
				}
			}

			// Token: 0x06000D33 RID: 3379 RVA: 0x0004B92D File Offset: 0x00049B2D
			private void ForceUpdateLayoutFromGraph()
			{
				this.SetLayout(TileLayout.FromGraph(this.graph));
			}

			// Token: 0x06000D34 RID: 3380 RVA: 0x0004B940 File Offset: 0x00049B40
			private void SetLayout(TileLayout tileLayout)
			{
				this.Dispose();
				this.tileLayout = tileLayout;
				this.clipperLookup = new GridLookup<NavmeshClipper>(tileLayout.tileCount);
				this.dirtyTiles = new UnsafeBitArray(tileLayout.tileCount.x * tileLayout.tileCount.y, Allocator.Persistent, NativeArrayOptions.ClearMemory);
				this.graph.active.navmeshUpdates.AddListener(this);
			}

			// Token: 0x06000D35 RID: 3381 RVA: 0x0004B9AC File Offset: 0x00049BAC
			internal void MarkTilesDirty(IntRect rect)
			{
				if (!this.enabled)
				{
					return;
				}
				rect = IntRect.Intersection(rect, new IntRect(0, 0, this.tileLayout.tileCount.x - 1, this.tileLayout.tileCount.y - 1));
				for (int i = rect.ymin; i <= rect.ymax; i++)
				{
					for (int j = rect.xmin; j <= rect.xmax; j++)
					{
						int num = j + i * this.tileLayout.tileCount.x;
						if (!this.dirtyTiles.IsSet(num))
						{
							this.dirtyTiles.Set(num, true);
							this.dirtyTileCoordinates.Add(new Vector2Int(j, i));
						}
					}
				}
			}

			// Token: 0x06000D36 RID: 3382 RVA: 0x0004BA63 File Offset: 0x00049C63
			public void ReloadAllTiles()
			{
				if (!this.enabled)
				{
					return;
				}
				this.MarkTilesDirty(new IntRect(int.MinValue, int.MinValue, int.MaxValue, int.MaxValue));
				this.ScheduleDirtyTilesReload();
			}

			// Token: 0x06000D37 RID: 3383 RVA: 0x0004BA94 File Offset: 0x00049C94
			public void AttachToGraph()
			{
				if (this.graph.navmeshUpdateData != null)
				{
					this.graph.navmeshUpdateData.Dispose();
					this.graph.navmeshUpdateData.attachedToGraph = false;
				}
				this.graph.navmeshUpdateData = this;
				this.attachedToGraph = true;
			}

			// Token: 0x06000D38 RID: 3384 RVA: 0x0004BAE2 File Offset: 0x00049CE2
			public void Enable()
			{
				if (this.enabled)
				{
					throw new InvalidOperationException("Already enabled");
				}
				this.ForceUpdateLayoutFromGraph();
				this.ReloadAllTiles();
			}

			// Token: 0x06000D39 RID: 3385 RVA: 0x0004BB03 File Offset: 0x00049D03
			public void Disable()
			{
				if (!this.enabled)
				{
					return;
				}
				this.clipperLookup.Clear();
				this.ReloadAllTiles();
				this.graph.active.FlushWorkItems();
				this.Dispose();
			}

			// Token: 0x06000D3A RID: 3386 RVA: 0x0004BB38 File Offset: 0x00049D38
			public void Dispose()
			{
				this.MapBoostPrepareForGraphMutation(false);
				this.dirtyTileCoordinates.Clear();
				this.clipperLookup = null;
				if (this.dirtyTiles.IsCreated)
				{
					this.dirtyTiles.Dispose();
				}
				this.dirtyTiles = default(UnsafeBitArray);
				if (this.graph.active != null)
				{
					this.graph.active.navmeshUpdates.RemoveListener(this);
				}
			}

			// Token: 0x06000D3B RID: 3387 RVA: 0x0004BBAC File Offset: 0x00049DAC
			public void DiscardPending()
			{
				this.MapBoostPrepareForGraphMutation(false);
				if (!this.enabled)
				{
					return;
				}
				for (int i = 0; i < NavmeshClipper.allEnabled.Count; i++)
				{
					NavmeshClipper navmeshClipper = NavmeshClipper.allEnabled[i];
					GridLookup<NavmeshClipper>.Root root = this.clipperLookup.GetRoot(navmeshClipper);
					if (root != null)
					{
						navmeshClipper.NotifyUpdated(root);
					}
				}
				this.dirtyTileCoordinates.Clear();
				this.dirtyTiles.Clear();
			}

			// Token: 0x06000D3C RID: 3388 RVA: 0x0004BC18 File Offset: 0x00049E18
			public void OnResized(IntRect newTileBounds, TileLayout tileLayout)
			{
				this.MapBoostPrepareForGraphMutation(true);
				if (!this.enabled)
				{
					return;
				}
				this.clipperLookup.Resize(newTileBounds);
				this.tileLayout = tileLayout;
				float navmeshCuttingCharacterRadius = this.graph.NavmeshCuttingCharacterRadius;
				for (GridLookup<NavmeshClipper>.Root root = this.clipperLookup.AllItems; root != null; root = root.next)
				{
					Rect rect = NavmeshUpdates.ExpandedXZBounds(root.obj.GetBounds(tileLayout.transform, navmeshCuttingCharacterRadius));
					IntRect touchingTilesInGraphSpace = tileLayout.GetTouchingTilesInGraphSpace(rect);
					if (root.previousBounds != touchingTilesInGraphSpace)
					{
						this.clipperLookup.Dirty(root.obj);
						this.clipperLookup.Move(root.obj, touchingTilesInGraphSpace);
					}
				}
				for (int i = 0; i < this.dirtyTileCoordinates.Count; i++)
				{
					Vector2Int vector2Int = this.dirtyTileCoordinates[i];
					if (newTileBounds.Contains(vector2Int.x, vector2Int.y))
					{
						this.dirtyTileCoordinates[i] = new Vector2Int(vector2Int.x - newTileBounds.xmin, vector2Int.y - newTileBounds.ymin);
					}
					else
					{
						this.dirtyTileCoordinates.RemoveAtSwapBack<Vector2Int>(i);
						i--;
					}
				}
				this.dirtyTiles.Resize(newTileBounds.Width * newTileBounds.Height, NativeArrayOptions.UninitializedMemory);
				this.dirtyTiles.Clear();
				for (int j = 0; j < this.dirtyTileCoordinates.Count; j++)
				{
					this.dirtyTiles.Set(this.dirtyTileCoordinates[j].x + this.dirtyTileCoordinates[j].y * newTileBounds.Width, true);
				}
			}

			// Token: 0x06000D3D RID: 3389 RVA: 0x0004BDC2 File Offset: 0x00049FC2
			public void Dirty(NavmeshClipper obj)
			{
				if (this.enabled)
				{
					this.clipperLookup.Dirty(obj);
				}
			}

			// Token: 0x06000D3E RID: 3390 RVA: 0x0004BDD8 File Offset: 0x00049FD8
			public void AddClipper(NavmeshClipper obj)
			{
				this.AssertEnabled();
				if (!obj.graphMask.Contains(this.graph))
				{
					return;
				}
				float navmeshCuttingCharacterRadius = this.graph.NavmeshCuttingCharacterRadius;
				Rect rect = NavmeshUpdates.ExpandedXZBounds(obj.GetBounds(this.tileLayout.transform, navmeshCuttingCharacterRadius));
				IntRect touchingTilesInGraphSpace = this.tileLayout.GetTouchingTilesInGraphSpace(rect);
				this.clipperLookup.Add(obj, touchingTilesInGraphSpace);
			}

			// Token: 0x06000D3F RID: 3391 RVA: 0x0004BE40 File Offset: 0x0004A040
			public void RemoveClipper(NavmeshClipper obj)
			{
				this.AssertEnabled();
				GridLookup<NavmeshClipper>.Root root = this.clipperLookup.GetRoot(obj);
				if (root != null)
				{
					this.MarkTilesDirty(root.previousBounds);
					this.clipperLookup.Remove(obj);
				}
			}

			// Token: 0x06000D40 RID: 3392 RVA: 0x0004BE7C File Offset: 0x0004A07C
			public void ScheduleDirtyTilesReload()
			{
				this.AssertEnabled();
				if (this.dirtyTileCoordinates.Count == 0)
				{
					return;
				}
				if (!NavmeshUpdates.MapBoostAsyncCutReload)
				{
					TileLayout tileLayout = this.tileLayout;
					this.graph.active.AddWorkItem(delegate(IWorkItemContext ctx)
					{
						ctx.PreUpdate();
						this.ReloadDirtyTilesImmediately();
					});
					return;
				}
				NavmeshUpdates.NavmeshUpdateSettings.<>c__DisplayClass38_0 CS$<>8__locals1 = new NavmeshUpdates.NavmeshUpdateSettings.<>c__DisplayClass38_0();
				CS$<>8__locals1.<>4__this = this;
				if (this.mapBoostAsyncQueued)
				{
					return;
				}
				this.mapBoostAsyncQueued = true;
				NavmeshUpdates.NavmeshUpdateSettings.<>c__DisplayClass38_0 CS$<>8__locals2 = CS$<>8__locals1;
				int num = this.mapBoostReloadGeneration + 1;
				this.mapBoostReloadGeneration = num;
				CS$<>8__locals2.generation = num;
				this.graph.active.AddWorkItem(new AstarWorkItem((IWorkItemContext ctx, bool force) => CS$<>8__locals1.<>4__this.ProcessMapBoostAsyncReload(ctx, force, CS$<>8__locals1.generation)));
			}

			// Token: 0x06000D41 RID: 3393 RVA: 0x0004BF20 File Offset: 0x0004A120
			public void ReloadDirtyTilesImmediately()
			{
				if (this.mapBoostCutScheduled)
				{
					this.CompleteMapBoostCut(true);
				}
				if (!this.enabled || this.dirtyTileCoordinates.Count == 0)
				{
					return;
				}
				using (NavmeshUpdates.MapBoostCutReloadMarker.Auto())
				{
					Promise<TileCutter.TileCutterOutput> promise = RecastBuilder.CutTiles(this.graph, this.clipperLookup, this.tileLayout).Schedule(this.dirtyTileCoordinates);
					using (NavmeshUpdates.MapBoostCutFenceMarker.Auto())
					{
						promise.Complete();
					}
					TileCutter.TileCutterOutput value = promise.GetValue();
					using (NavmeshUpdates.MapBoostCutApplyMarker.Auto())
					{
						this.graph.StartBatchTileUpdate(false);
						if (!value.tileMeshes.tileMeshes.IsCreated)
						{
							for (int i = 0; i < this.dirtyTileCoordinates.Count; i++)
							{
								NavmeshTile tile = this.graph.GetTile(this.dirtyTileCoordinates[i].x, this.dirtyTileCoordinates[i].y);
								if (tile.isCut)
								{
									this.graph.ReplaceTilePostCut(tile.x, tile.z, tile.preCutVertsInTileSpace, tile.preCutTris, tile.preCutTags, true, true);
								}
							}
						}
						else
						{
							for (int j = 0; j < value.tileMeshes.tileMeshes.Length; j++)
							{
								TileMesh.TileMeshUnsafe tileMeshUnsafe = value.tileMeshes.tileMeshes[j];
								this.graph.ReplaceTilePostCut(this.dirtyTileCoordinates[j].x, this.dirtyTileCoordinates[j].y, tileMeshUnsafe.verticesInTileSpace, tileMeshUnsafe.triangles, tileMeshUnsafe.tags, true, true);
							}
						}
						value.Dispose();
						this.graph.EndBatchTileUpdate();
						this.dirtyTileCoordinates.Clear();
						this.dirtyTiles.Clear();
					}
				}
			}

			// Token: 0x06000D42 RID: 3394 RVA: 0x0004C194 File Offset: 0x0004A394
			private bool ProcessMapBoostAsyncReload(IWorkItemContext ctx, bool force, int generation)
			{
				if (generation != this.mapBoostReloadGeneration || !this.enabled)
				{
					return true;
				}
				bool flag;
				using (NavmeshUpdates.MapBoostCutReloadMarker.Auto())
				{
					try
					{
						for (;;)
						{
							if (!this.mapBoostCutScheduled)
							{
								if (this.dirtyTileCoordinates.Count == 0)
								{
									break;
								}
								ctx.PreUpdate();
								this.mapBoostCutTiles = new List<Vector2Int>(this.dirtyTileCoordinates);
								this.mapBoostCutPromise = RecastBuilder.CutTiles(this.graph, this.clipperLookup, this.tileLayout).Schedule(this.mapBoostCutTiles);
								this.mapBoostCutScheduled = true;
								this.dirtyTileCoordinates.Clear();
								this.dirtyTiles.Clear();
								if (!force)
								{
									goto Block_6;
								}
							}
							if (!force && !this.mapBoostCutPromise.IsCompleted)
							{
								goto Block_8;
							}
							this.CompleteMapBoostCut(true);
							if (!force || this.dirtyTileCoordinates.Count == 0)
							{
								goto IL_00DB;
							}
						}
						this.mapBoostAsyncQueued = false;
						return true;
						Block_6:
						return false;
						Block_8:
						return false;
						IL_00DB:
						this.mapBoostAsyncQueued = false;
						flag = true;
					}
					catch (Exception ex)
					{
						try
						{
							this.MapBoostPrepareForGraphMutation(false);
						}
						finally
						{
							this.graph.active.SuspendNavigation(ex);
						}
						throw;
					}
				}
				return flag;
			}

			// Token: 0x06000D43 RID: 3395 RVA: 0x0004C2E0 File Offset: 0x0004A4E0
			internal void MapBoostPrepareForGraphMutation(bool preserveDirty)
			{
				if (!this.mapBoostAsyncQueued && !this.mapBoostCutScheduled)
				{
					return;
				}
				this.mapBoostReloadGeneration++;
				this.mapBoostAsyncQueued = false;
				if (this.mapBoostCutScheduled && preserveDirty && this.enabled)
				{
					for (int i = 0; i < this.mapBoostCutTiles.Count; i++)
					{
						Vector2Int vector2Int = this.mapBoostCutTiles[i];
						this.MarkTilesDirty(new IntRect(vector2Int.x, vector2Int.y, vector2Int.x, vector2Int.y));
					}
				}
				this.CompleteMapBoostCut(false);
			}

			// Token: 0x06000D44 RID: 3396 RVA: 0x0004C378 File Offset: 0x0004A578
			private void CompleteMapBoostCut(bool apply)
			{
				if (!this.mapBoostCutScheduled)
				{
					return;
				}
				Promise<TileCutter.TileCutterOutput> promise = this.mapBoostCutPromise;
				List<Vector2Int> list = this.mapBoostCutTiles;
				this.mapBoostCutScheduled = false;
				this.mapBoostCutPromise = default(Promise<TileCutter.TileCutterOutput>);
				this.mapBoostCutTiles = null;
				try
				{
					using (NavmeshUpdates.MapBoostCutFenceMarker.Auto())
					{
						promise.Complete();
					}
					if (apply && this.enabled)
					{
						TileCutter.TileCutterOutput value = promise.GetValue();
						using (NavmeshUpdates.MapBoostCutApplyMarker.Auto())
						{
							this.graph.StartBatchTileUpdate(false);
							if (!value.tileMeshes.tileMeshes.IsCreated)
							{
								for (int i = 0; i < list.Count; i++)
								{
									NavmeshTile tile = this.graph.GetTile(list[i].x, list[i].y);
									if (tile.isCut)
									{
										this.graph.ReplaceTilePostCut(tile.x, tile.z, tile.preCutVertsInTileSpace, tile.preCutTris, tile.preCutTags, true, true);
									}
								}
							}
							else
							{
								for (int j = 0; j < value.tileMeshes.tileMeshes.Length; j++)
								{
									TileMesh.TileMeshUnsafe tileMeshUnsafe = value.tileMeshes.tileMeshes[j];
									this.graph.ReplaceTilePostCut(list[j].x, list[j].y, tileMeshUnsafe.verticesInTileSpace, tileMeshUnsafe.triangles, tileMeshUnsafe.tags, true, true);
								}
							}
							this.graph.EndBatchTileUpdate();
						}
					}
				}
				finally
				{
					promise.Dispose();
				}
			}

			// Token: 0x040008D6 RID: 2262
			internal readonly NavmeshBase graph;

			// Token: 0x040008D7 RID: 2263
			public GridLookup<NavmeshClipper> clipperLookup;

			// Token: 0x040008D8 RID: 2264
			public TileLayout tileLayout;

			// Token: 0x040008D9 RID: 2265
			private UnsafeBitArray dirtyTiles;

			// Token: 0x040008DA RID: 2266
			private List<Vector2Int> dirtyTileCoordinates = new List<Vector2Int>();

			// Token: 0x040008DB RID: 2267
			private bool mapBoostAsyncQueued;

			// Token: 0x040008DC RID: 2268
			private bool mapBoostCutScheduled;

			// Token: 0x040008DD RID: 2269
			private int mapBoostReloadGeneration;

			// Token: 0x040008DE RID: 2270
			private List<Vector2Int> mapBoostCutTiles;

			// Token: 0x040008DF RID: 2271
			private Promise<TileCutter.TileCutterOutput> mapBoostCutPromise;
		}
	}
}
