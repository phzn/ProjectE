using System;
using System.Collections.Generic;
using Features.Rank;
using Features.RenderingPrecision;
using Features.RunModes;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Features.Vehicles.Spawning
{
	// Token: 0x02000B8A RID: 2954
	public class VehicleSpawnManager : MonoBehaviour
	{
		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x06005451 RID: 21585 RVA: 0x0020B738 File Offset: 0x00209938
		public bool IsInitialSpawnComplete
		{
			get
			{
				return this._advancedMechanicsBlocked || GuidedRankGameplayPolicy.RideableVehiclesBlocked || (this._isSceneInitialized && (!this.bindToChunkManager || (this._isChunkManagerBound && this._chunkManager != null)) && this._queuedSpawnPoints.Count == 0 && this._queuedDespawns.Count == 0 && this._guaranteedRetryAt.Count == 0);
			}
		}

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x06005452 RID: 21586 RVA: 0x0020B7A6 File Offset: 0x002099A6
		public AudioClip VehicleExpirySfx
		{
			get
			{
				return this.vehicleExpirySfx;
			}
		}

		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x06005453 RID: 21587 RVA: 0x0020B7AE File Offset: 0x002099AE
		public float VehicleExpirySfxVolume
		{
			get
			{
				return Mathf.Clamp01(this.vehicleExpirySfxVolume);
			}
		}

		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x06005454 RID: 21588 RVA: 0x0020B7BB File Offset: 0x002099BB
		public float VehicleExpirySfxPitch
		{
			get
			{
				return Mathf.Clamp(this.vehicleExpirySfxPitch, 0.5f, 2f);
			}
		}

		// Token: 0x06005455 RID: 21589 RVA: 0x0020B7D2 File Offset: 0x002099D2
		private void OnEnable()
		{
			RankProgressionService.Changed += this.ApplyRankPolicy;
			RunModeService.Changed += this.ApplyRankPolicy;
			this.ApplyRankPolicy();
			if (this._isSceneInitialized && this.bindToChunkManager)
			{
				this.TryBindChunkManager();
			}
		}

		// Token: 0x06005456 RID: 21590 RVA: 0x0020B812 File Offset: 0x00209A12
		private void OnDisable()
		{
			RankProgressionService.Changed -= this.ApplyRankPolicy;
			RunModeService.Changed -= this.ApplyRankPolicy;
			this.UnbindChunkManager();
		}

		// Token: 0x06005457 RID: 21591 RVA: 0x0020B83C File Offset: 0x00209A3C
		private void Start()
		{
			if (this._isSceneInitialized)
			{
				return;
			}
			if (this.clearSpawnedVehiclesOnStart)
			{
				this.ClearSpawnedVehicles();
			}
			if (!this.autoSpawnOnStart)
			{
				return;
			}
			this.ReinitializeForCurrentScene(this.spawnPointsRoot, this.bindToChunkManager);
		}

		// Token: 0x06005458 RID: 21592 RVA: 0x0020B870 File Offset: 0x00209A70
		private void Update()
		{
			this.ApplyRankPolicy();
			if (this._advancedMechanicsBlocked)
			{
				if (this._spawnedByPoint.Count > 0 || this._claimedVehicles.Count > 0)
				{
					this.DestroyTrackedVehicles(true);
				}
				return;
			}
			if (this._isSceneInitialized && this.bindToChunkManager && !this._isChunkManagerBound)
			{
				this.TryBindChunkManager();
			}
			this.PruneDestroyedVehicleState();
			this.QueueDueGuaranteedSpawnRetries();
			this.ProcessLifecycleQueues();
		}

		// Token: 0x06005459 RID: 21593 RVA: 0x0020B8E0 File Offset: 0x00209AE0
		private void ApplyRankPolicy()
		{
			this.ApplyRankPolicy(GuidedRankGameplayPolicy.RideableVehiclesBlocked);
		}

		// Token: 0x0600545A RID: 21594 RVA: 0x0020B8F0 File Offset: 0x00209AF0
		private void ApplyRankPolicy(bool blocked)
		{
			if (this._advancedMechanicsBlocked == blocked)
			{
				return;
			}
			this._advancedMechanicsBlocked = blocked;
			if (blocked)
			{
				this.ClearSpawnedVehicles();
				return;
			}
			if (!this._isSceneInitialized)
			{
				return;
			}
			if (this.bindToChunkManager)
			{
				this.TryBindChunkManager();
				this.QueueSpawnForKnownChunks();
				return;
			}
			this.QueueAllSceneSpawnPoints();
		}

		// Token: 0x0600545B RID: 21595 RVA: 0x0020B93C File Offset: 0x00209B3C
		public void ReinitializeForCurrentScene(Transform staticSpawnPointsRoot, bool useChunkManager)
		{
			Scene activeScene = SceneManager.GetActiveScene();
			int num = (activeScene.IsValid() ? activeScene.handle : (-1));
			if (this._isSceneInitialized && this._configuredSceneHandle == num && this.bindToChunkManager == useChunkManager && this.spawnPointsRoot == staticSpawnPointsRoot)
			{
				return;
			}
			this.UnbindChunkManager();
			this.DestroyTrackedVehicles(true);
			this.ResetPointTracking();
			this.spawnPointsRoot = staticSpawnPointsRoot;
			this.bindToChunkManager = useChunkManager;
			this._configuredSceneHandle = num;
			this._isSceneInitialized = true;
			this.EnsureSpawnedVehiclesContainer();
			if (this.bindToChunkManager)
			{
				this.TryBindChunkManager();
			}
			else
			{
				this.QueueAllSceneSpawnPoints();
			}
			if (this.debugLogs)
			{
				Debug.Log("[VehicleSpawnManager] Reinitialized for '" + activeScene.name + "' " + string.Format("(chunkMode={0}, queued={1}).", this.bindToChunkManager, this._queuedSpawnPoints.Count));
			}
		}

		// Token: 0x0600545C RID: 21596 RVA: 0x0020BA21 File Offset: 0x00209C21
		public void ShutdownForCurrentScene()
		{
			this.UnbindChunkManager();
			this.DestroyTrackedVehicles(true);
			this.ResetPointTracking();
			this._isSceneInitialized = false;
			this._configuredSceneHandle = -1;
		}

		// Token: 0x0600545D RID: 21597 RVA: 0x0020BA48 File Offset: 0x00209C48
		[ContextMenu("Spawn All Vehicles")]
		public void SpawnAll()
		{
			if (!this._isSceneInitialized)
			{
				this._isSceneInitialized = true;
				this._configuredSceneHandle = SceneManager.GetActiveScene().handle;
				this.bindToChunkManager = false;
			}
			this.QueueAllSceneSpawnPoints();
		}

		// Token: 0x0600545E RID: 21598 RVA: 0x0020BA84 File Offset: 0x00209C84
		[ContextMenu("Clear Spawned Vehicles")]
		public void ClearSpawnedVehicles()
		{
			int num = this.DestroyTrackedVehicles(true);
			this._despawnQueue.Clear();
			this._queuedDespawns.Clear();
			this._spawnQueue.Clear();
			this._queuedSpawnPoints.Clear();
			this._resolvedWithoutVehicle.Clear();
			this._guaranteedRetryAt.Clear();
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[VehicleSpawnManager] Cleared {0} spawned vehicles; mounted vehicles were preserved.", num));
			}
		}

		// Token: 0x0600545F RID: 21599 RVA: 0x0020BAF8 File Offset: 0x00209CF8
		public void RespawnAllForWorldShift()
		{
			this.GatherTrackedVehicles(this._scratchVehicles);
			for (int i = 0; i < this._scratchVehicles.Count; i++)
			{
				GameObject gameObject = this._scratchVehicles[i];
				if (gameObject != null && VehicleSpawnManager.IsVehicleMounted(gameObject))
				{
					this.MarkVehicleClaimed(gameObject);
					Debug.LogError("[VehicleSpawnManager] World shift cleanup refused: a spawned vehicle ('" + gameObject.name + "') is still mounted. Dismount before travelling.", gameObject);
					return;
				}
			}
			int num = this.DestroyTrackedVehicles(false);
			this._despawnQueue.Clear();
			this._queuedDespawns.Clear();
			this._spawnQueue.Clear();
			this._queuedSpawnPoints.Clear();
			this._resolvedWithoutVehicle.Clear();
			this._guaranteedRetryAt.Clear();
			int num2 = 0;
			foreach (KeyValuePair<ChunkInstance, List<VehicleSpawnPoint>> keyValuePair in this._chunkPoints)
			{
				bool flag;
				if (!this._chunkActiveState.TryGetValue(keyValuePair.Key, out flag) || flag)
				{
					List<VehicleSpawnPoint> value = keyValuePair.Value;
					for (int j = 0; j < value.Count; j++)
					{
						if (!(value[j] == null))
						{
							this.QueuePointForSpawn(value[j]);
							num2++;
						}
					}
				}
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[VehicleSpawnManager] World shift: {0} veicoli rimossi, ", num) + string.Format("{0} punti rimessi in coda.", num2));
			}
		}

		// Token: 0x06005460 RID: 21600 RVA: 0x0020BC84 File Offset: 0x00209E84
		public void RefreshSpawnPoints()
		{
			this._scratchPoints.Clear();
			this._scratchPointSet.Clear();
			if (this.spawnPointsRoot != null)
			{
				this.CollectUniquePointsFromRoot(this.spawnPointsRoot.gameObject, this._scratchPoints, this._scratchPointSet);
				return;
			}
			Scene activeScene = SceneManager.GetActiveScene();
			if (!activeScene.IsValid() || !activeScene.isLoaded)
			{
				return;
			}
			GameObject[] rootGameObjects = activeScene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				this.CollectUniquePointsFromRoot(rootGameObjects[i], this._scratchPoints, this._scratchPointSet);
			}
		}

		// Token: 0x06005461 RID: 21601 RVA: 0x0020BD18 File Offset: 0x00209F18
		private void TryBindChunkManager()
		{
			if (!this.bindToChunkManager || this._isChunkManagerBound)
			{
				return;
			}
			if (ChunkManager.Instance == null || !ChunkManager.Instance.enabled)
			{
				return;
			}
			this._chunkManager = ChunkManager.Instance;
			this._chunkManager.OnChunkSpawned += this.OnChunkSpawned;
			this._chunkManager.OnChunkRemoved += this.OnChunkRemoved;
			this._chunkManager.OnChunkActivated += this.OnChunkActivated;
			this._chunkManager.OnChunkDeactivated += this.OnChunkDeactivated;
			this._chunkManager.OnChunkZonesReady += this.OnChunkZonesReady;
			this._isChunkManagerBound = true;
			this.QueueSpawnForKnownChunks();
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[VehicleSpawnManager] Bound to ChunkManager. Known chunks: {0}", this._chunkManager.SpawnedChunks.Count));
			}
		}

		// Token: 0x06005462 RID: 21602 RVA: 0x0020BE08 File Offset: 0x0020A008
		private void UnbindChunkManager()
		{
			if (!this._isChunkManagerBound)
			{
				return;
			}
			if (this._chunkManager != null)
			{
				this._chunkManager.OnChunkSpawned -= this.OnChunkSpawned;
				this._chunkManager.OnChunkRemoved -= this.OnChunkRemoved;
				this._chunkManager.OnChunkActivated -= this.OnChunkActivated;
				this._chunkManager.OnChunkDeactivated -= this.OnChunkDeactivated;
				this._chunkManager.OnChunkZonesReady -= this.OnChunkZonesReady;
			}
			this._chunkManager = null;
			this._isChunkManagerBound = false;
		}

		// Token: 0x06005463 RID: 21603 RVA: 0x0020BEAD File Offset: 0x0020A0AD
		private void OnChunkSpawned(ChunkInstance chunk)
		{
			this.RefreshChunkPoints(chunk);
		}

		// Token: 0x06005464 RID: 21604 RVA: 0x0020BEAD File Offset: 0x0020A0AD
		private void OnChunkZonesReady(ChunkInstance chunk)
		{
			this.RefreshChunkPoints(chunk);
		}

		// Token: 0x06005465 RID: 21605 RVA: 0x0020BEB6 File Offset: 0x0020A0B6
		private void OnChunkRemoved(ChunkInstance chunk)
		{
			this.UnregisterChunkPoints(chunk);
		}

		// Token: 0x06005466 RID: 21606 RVA: 0x0020BEBF File Offset: 0x0020A0BF
		private void OnChunkActivated(ChunkInstance chunk)
		{
			if (chunk != null && !this._chunkPoints.ContainsKey(chunk))
			{
				this.RefreshChunkPoints(chunk);
			}
			this.SetChunkPointsActive(chunk, true);
		}

		// Token: 0x06005467 RID: 21607 RVA: 0x0020BEE1 File Offset: 0x0020A0E1
		private void OnChunkDeactivated(ChunkInstance chunk)
		{
			this.SetChunkPointsActive(chunk, false);
		}

		// Token: 0x06005468 RID: 21608 RVA: 0x0020BEEC File Offset: 0x0020A0EC
		private void QueueSpawnForKnownChunks()
		{
			if (this._chunkManager == null)
			{
				return;
			}
			foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in this._chunkManager.SpawnedChunks)
			{
				this.RefreshChunkPoints(keyValuePair.Value);
			}
		}

		// Token: 0x06005469 RID: 21609 RVA: 0x0020BF54 File Offset: 0x0020A154
		private void QueueAllSceneSpawnPoints()
		{
			this.RefreshSpawnPoints();
			for (int i = 0; i < this._scratchPoints.Count; i++)
			{
				this.QueuePointForSpawn(this._scratchPoints[i]);
			}
		}

		// Token: 0x0600546A RID: 21610 RVA: 0x0020BF90 File Offset: 0x0020A190
		private void RefreshChunkPoints(ChunkInstance chunk)
		{
			if (chunk == null)
			{
				return;
			}
			List<VehicleSpawnPoint> list = new List<VehicleSpawnPoint>(16);
			this._scratchPointSet.Clear();
			ChunkContentRootTraversal.Collect(chunk, true, this._chunkPointRootScratch);
			for (int i = 0; i < this._chunkPointRootScratch.Count; i++)
			{
				this.CollectUniquePointsFromRoot(this._chunkPointRootScratch[i], list, this._scratchPointSet);
			}
			this._chunkPointRootScratch.Clear();
			List<VehicleSpawnPoint> list2;
			bool flag = this._chunkPoints.TryGetValue(chunk, out list2);
			bool flag3;
			bool flag2 = !this._chunkActiveState.TryGetValue(chunk, out flag3) || flag3;
			if (flag)
			{
				for (int j = 0; j < list2.Count; j++)
				{
					VehicleSpawnPoint vehicleSpawnPoint = list2[j];
					if (vehicleSpawnPoint != null && !this._scratchPointSet.Contains(vehicleSpawnPoint))
					{
						this.ReleasePointFromChunk(vehicleSpawnPoint);
					}
				}
			}
			this._chunkPoints[chunk] = list;
			this._chunkActiveState[chunk] = flag2;
			for (int k = 0; k < list.Count; k++)
			{
				VehicleSpawnPoint vehicleSpawnPoint2 = list[k];
				if (!(vehicleSpawnPoint2 == null))
				{
					if (flag2)
					{
						this._inactiveChunkPoints.Remove(vehicleSpawnPoint2);
						this.QueuePointForSpawn(vehicleSpawnPoint2);
					}
					else
					{
						this._inactiveChunkPoints.Add(vehicleSpawnPoint2);
					}
				}
			}
			if (this.debugLogs && (!flag || list.Count != list2.Count))
			{
				string text = string.Format("[VehicleSpawnManager] Cached {0} unique points for chunk ", list.Count);
				string text2 = "'{0}' (zonesReady refresh={1}).";
				GameObject chunkRoot = chunk.chunkRoot;
				Debug.Log(text + string.Format(text2, (chunkRoot != null) ? chunkRoot.name : null, flag));
			}
		}

		// Token: 0x0600546B RID: 21611 RVA: 0x0020C128 File Offset: 0x0020A328
		private void UnregisterChunkPoints(ChunkInstance chunk)
		{
			List<VehicleSpawnPoint> list;
			if (chunk == null || !this._chunkPoints.TryGetValue(chunk, out list))
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				VehicleSpawnPoint vehicleSpawnPoint = list[i];
				if (vehicleSpawnPoint != null)
				{
					this.ReleasePointFromChunk(vehicleSpawnPoint);
				}
			}
			this._chunkPoints.Remove(chunk);
			this._chunkActiveState.Remove(chunk);
		}

		// Token: 0x0600546C RID: 21612 RVA: 0x0020C188 File Offset: 0x0020A388
		private void ReleasePointFromChunk(VehicleSpawnPoint point)
		{
			this._queuedSpawnPoints.Remove(point);
			this._inactiveChunkPoints.Remove(point);
			this._resolvedWithoutVehicle.Remove(point);
			this._guaranteedRetryAt.Remove(point);
			GameObject gameObject;
			if (!this._spawnedByPoint.TryGetValue(point, out gameObject))
			{
				return;
			}
			this._spawnedByPoint.Remove(point);
			if (gameObject == null)
			{
				return;
			}
			if (this.IsProtectedFromOriginChunk(gameObject))
			{
				this.MarkVehicleClaimed(gameObject);
				return;
			}
			this.QueueVehicleForDespawn(gameObject);
		}

		// Token: 0x0600546D RID: 21613 RVA: 0x0020C20C File Offset: 0x0020A40C
		private void SetChunkPointsActive(ChunkInstance chunk, bool isActive)
		{
			List<VehicleSpawnPoint> list;
			if (chunk == null || !this._chunkPoints.TryGetValue(chunk, out list))
			{
				return;
			}
			this._chunkActiveState[chunk] = isActive;
			for (int i = 0; i < list.Count; i++)
			{
				VehicleSpawnPoint vehicleSpawnPoint = list[i];
				if (vehicleSpawnPoint != null)
				{
					if (!isActive)
					{
						this._queuedSpawnPoints.Remove(vehicleSpawnPoint);
						this._inactiveChunkPoints.Add(vehicleSpawnPoint);
						GameObject gameObject;
						if (this._spawnedByPoint.TryGetValue(vehicleSpawnPoint, out gameObject) && gameObject != null)
						{
							if (this.IsProtectedFromOriginChunk(gameObject))
							{
								this.MarkVehicleClaimed(gameObject);
							}
							else if (gameObject.activeSelf)
							{
								gameObject.SetActive(false);
							}
						}
					}
					else
					{
						this._inactiveChunkPoints.Remove(vehicleSpawnPoint);
						GameObject gameObject2;
						if (this._spawnedByPoint.TryGetValue(vehicleSpawnPoint, out gameObject2))
						{
							if (gameObject2 != null)
							{
								if (!gameObject2.activeSelf && !this.IsProtectedFromOriginChunk(gameObject2))
								{
									gameObject2.SetActive(true);
								}
							}
							else
							{
								this._spawnedByPoint.Remove(vehicleSpawnPoint);
								this._resolvedWithoutVehicle.Remove(vehicleSpawnPoint);
								this.QueuePointForSpawn(vehicleSpawnPoint);
							}
						}
						else
						{
							this.QueuePointForSpawn(vehicleSpawnPoint);
						}
					}
				}
			}
		}

		// Token: 0x0600546E RID: 21614 RVA: 0x0020C330 File Offset: 0x0020A530
		private void CollectUniquePointsFromRoot(GameObject root, List<VehicleSpawnPoint> destination, HashSet<VehicleSpawnPoint> seen)
		{
			if (root == null)
			{
				return;
			}
			this._componentScratchPoints.Clear();
			root.GetComponentsInChildren<VehicleSpawnPoint>(true, this._componentScratchPoints);
			for (int i = 0; i < this._componentScratchPoints.Count; i++)
			{
				VehicleSpawnPoint vehicleSpawnPoint = this._componentScratchPoints[i];
				if (vehicleSpawnPoint != null && seen.Add(vehicleSpawnPoint))
				{
					destination.Add(vehicleSpawnPoint);
				}
			}
			this._componentScratchPoints.Clear();
		}

		// Token: 0x0600546F RID: 21615 RVA: 0x0020C3A8 File Offset: 0x0020A5A8
		private void QueuePointForSpawn(VehicleSpawnPoint point)
		{
			if (GuidedRankGameplayPolicy.RideableVehiclesBlocked)
			{
				return;
			}
			if (point == null || this._inactiveChunkPoints.Contains(point) || this._spawnedByPoint.ContainsKey(point) || this._resolvedWithoutVehicle.Contains(point) || this._guaranteedRetryAt.ContainsKey(point))
			{
				return;
			}
			if (this._queuedSpawnPoints.Add(point))
			{
				this._spawnQueue.Enqueue(point);
			}
		}

		// Token: 0x06005470 RID: 21616 RVA: 0x0020C419 File Offset: 0x0020A619
		private void ScheduleGuaranteedSpawnRetry(VehicleSpawnPoint point)
		{
			if (point == null || !point.GuaranteedSpawn || this._guaranteedRetryAt.ContainsKey(point))
			{
				return;
			}
			this._guaranteedRetryAt.Add(point, Time.time + 0.5f);
		}

		// Token: 0x06005471 RID: 21617 RVA: 0x0020C454 File Offset: 0x0020A654
		private void QueueDueGuaranteedSpawnRetries()
		{
			if (this._guaranteedRetryAt.Count == 0)
			{
				return;
			}
			this._scratchPointKeys.Clear();
			foreach (KeyValuePair<VehicleSpawnPoint, float> keyValuePair in this._guaranteedRetryAt)
			{
				if (keyValuePair.Key == null || Time.time >= keyValuePair.Value)
				{
					this._scratchPointKeys.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < this._scratchPointKeys.Count; i++)
			{
				VehicleSpawnPoint vehicleSpawnPoint = this._scratchPointKeys[i];
				this._guaranteedRetryAt.Remove(vehicleSpawnPoint);
				if (vehicleSpawnPoint != null)
				{
					this.QueuePointForSpawn(vehicleSpawnPoint);
				}
			}
			this._scratchPointKeys.Clear();
		}

		// Token: 0x06005472 RID: 21618 RVA: 0x0020C534 File Offset: 0x0020A734
		private void QueueVehicleForDespawn(GameObject vehicle)
		{
			if (vehicle != null && this._queuedDespawns.Add(vehicle))
			{
				this._despawnQueue.Enqueue(vehicle);
			}
		}

		// Token: 0x06005473 RID: 21619 RVA: 0x0020C55C File Offset: 0x0020A75C
		private void ProcessLifecycleQueues()
		{
			if (GuidedRankGameplayPolicy.RideableVehiclesBlocked)
			{
				return;
			}
			int num = 0;
			int num2 = Mathf.Max(1, this.maxSpawnOpsPerFrame);
			float num3 = Mathf.Max(0.05f, this.spawnFrameBudgetMs);
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if (this._spawnQueue.Count > 0)
			{
				Physics.SyncTransforms();
			}
			while (num < num2 && (Time.realtimeSinceStartup - realtimeSinceStartup) * 1000f < num3)
			{
				bool flag = false;
				if (this._despawnQueue.Count > 0)
				{
					GameObject gameObject = this._despawnQueue.Dequeue();
					if (gameObject != null)
					{
						this._queuedDespawns.Remove(gameObject);
					}
					if (gameObject != null)
					{
						if (this.IsProtectedFromOriginChunk(gameObject))
						{
							this.MarkVehicleClaimed(gameObject);
						}
						else
						{
							this.DestroyManagedVehicle(gameObject);
						}
					}
					flag = true;
				}
				else if (this._spawnQueue.Count > 0)
				{
					VehicleSpawnPoint vehicleSpawnPoint = this._spawnQueue.Dequeue();
					if (vehicleSpawnPoint != null && !this._queuedSpawnPoints.Remove(vehicleSpawnPoint))
					{
						num++;
						continue;
					}
					flag = true;
					if (vehicleSpawnPoint != null && !this._inactiveChunkPoints.Contains(vehicleSpawnPoint) && !this._spawnedByPoint.ContainsKey(vehicleSpawnPoint) && !this._resolvedWithoutVehicle.Contains(vehicleSpawnPoint))
					{
						GameObject gameObject2;
						if (RunModeService.VehicleSpawnMultiplier <= 0f)
						{
							this._resolvedWithoutVehicle.Add(vehicleSpawnPoint);
						}
						else if (!this.HasSpawnSurfaceSupport(vehicleSpawnPoint))
						{
							this._resolvedWithoutVehicle.Add(vehicleSpawnPoint);
							if (this.debugLogs)
							{
								Debug.LogWarning("[VehicleSpawnManager] Skipped unsupported point " + string.Format("'{0}' at {1}.", vehicleSpawnPoint.name, vehicleSpawnPoint.SpawnPosition), vehicleSpawnPoint);
							}
						}
						else if (this.IsSpawnAreaOccupied(vehicleSpawnPoint))
						{
							if (vehicleSpawnPoint.GuaranteedSpawn)
							{
								this.ScheduleGuaranteedSpawnRetry(vehicleSpawnPoint);
							}
							else
							{
								this._resolvedWithoutVehicle.Add(vehicleSpawnPoint);
							}
						}
						else if (vehicleSpawnPoint.TryPickVehiclePrefab(out gameObject2) && gameObject2 != null)
						{
							this._guaranteedRetryAt.Remove(vehicleSpawnPoint);
							Vector3 spawnPositionWithPrefabYOffset = VehicleSpawnManager.GetSpawnPositionWithPrefabYOffset(vehicleSpawnPoint, gameObject2);
							Transform transform = this.EnsureSpawnedVehiclesContainer();
							GameObject gameObject3 = Object.Instantiate<GameObject>(gameObject2, spawnPositionWithPrefabYOffset, vehicleSpawnPoint.SpawnRotation, transform);
							VehicleSpawnManager.MakeRigidbodyParkedImmediately(gameObject3);
							SpawnedVehicleGroundSafety spawnedVehicleGroundSafety = gameObject3.GetComponent<SpawnedVehicleGroundSafety>();
							if (spawnedVehicleGroundSafety == null)
							{
								spawnedVehicleGroundSafety = gameObject3.AddComponent<SpawnedVehicleGroundSafety>();
							}
							spawnedVehicleGroundSafety.Initialize(spawnPositionWithPrefabYOffset, vehicleSpawnPoint.SpawnRotation, vehicleSpawnPoint.ClearanceSize, this.occupancyMask);
							this.InstallLifetime(gameObject3);
							this._spawnedByPoint[vehicleSpawnPoint] = gameObject3;
							this.SubscribeToMountEvents(gameObject3);
							if (BicycleRideDiagnostics.RecordingEnabled)
							{
								BicycleRideDiagnostics.RecordSpawn(gameObject3.transform, gameObject2, string.Concat(new string[]
								{
									"point=",
									BicycleRideDiagnostics.Path(vehicleSpawnPoint.transform),
									"; clearance=",
									vehicleSpawnPoint.ClearanceSize.ToString(),
									"; supportMask=",
									this.occupancyMask.value.ToString()
								}));
							}
						}
						else
						{
							this._resolvedWithoutVehicle.Add(vehicleSpawnPoint);
						}
					}
				}
				if (!flag)
				{
					break;
				}
				num++;
			}
			if (this.debugLogs && num > 0)
			{
				Debug.Log(string.Format("[VehicleSpawnManager] Processed {0} ops. ", num) + string.Format("SpawnQ={0}, DespawnQ={1}, ", this._queuedSpawnPoints.Count, this._queuedDespawns.Count) + string.Format("Parked={0}, Claimed={1}", this._spawnedByPoint.Count, this._claimedVehicles.Count));
			}
		}

		// Token: 0x06005474 RID: 21620 RVA: 0x0020C910 File Offset: 0x0020AB10
		private bool IsSpawnAreaOccupied(VehicleSpawnPoint point)
		{
			Vector3 vector = point.ClearanceSize * 0.5f;
			int num = Physics.OverlapBoxNonAlloc(point.ClearanceCenter, vector, this._occupancyBuffer, point.SpawnRotation, this.occupancyMask, QueryTriggerInteraction.Ignore);
			float num2 = point.SpawnPosition.y + 0.08f;
			for (int i = 0; i < num; i++)
			{
				Collider collider = this._occupancyBuffer[i];
				this._occupancyBuffer[i] = null;
				if (!(collider == null) && !collider.isTrigger && !(collider is TerrainCollider) && collider.bounds.max.y > num2)
				{
					return true;
				}
			}
			return num >= this._occupancyBuffer.Length;
		}

		// Token: 0x06005475 RID: 21621 RVA: 0x0020C9C8 File Offset: 0x0020ABC8
		private bool HasSpawnSurfaceSupport(VehicleSpawnPoint point)
		{
			Vector3 vector = point.SpawnRotation * Vector3.right * (point.ClearanceSize.x * 0.42f);
			Vector3 vector2 = point.SpawnRotation * Vector3.forward * (point.ClearanceSize.z * 0.42f);
			Vector3 spawnPosition = point.SpawnPosition;
			Vector3[] array = new Vector3[]
			{
				spawnPosition,
				spawnPosition + vector + vector2,
				spawnPosition + vector - vector2,
				spawnPosition - vector + vector2,
				spawnPosition - vector - vector2
			};
			float num = float.PositiveInfinity;
			float num2 = float.NegativeInfinity;
			for (int i = 0; i < array.Length; i++)
			{
				float num3;
				if (!this.TryGetSupportHeight(array[i], out num3))
				{
					return false;
				}
				num = Mathf.Min(num, num3);
				num2 = Mathf.Max(num2, num3);
			}
			return num2 - num <= 0.16f;
		}

		// Token: 0x06005476 RID: 21622 RVA: 0x0020CAE4 File Offset: 0x0020ACE4
		private bool TryGetSupportHeight(Vector3 position, out float supportY)
		{
			int num = Physics.RaycastNonAlloc(position + Vector3.up * 1.2f, Vector3.down, this._supportBuffer, 2.5f, this.occupancyMask, QueryTriggerInteraction.Ignore);
			supportY = float.NegativeInfinity;
			for (int i = 0; i < num; i++)
			{
				RaycastHit raycastHit = this._supportBuffer[i];
				this._supportBuffer[i] = default(RaycastHit);
				if (raycastHit.collider != null && raycastHit.normal.y >= 0.82f)
				{
					supportY = Mathf.Max(supportY, raycastHit.point.y);
				}
			}
			return !float.IsNegativeInfinity(supportY);
		}

		// Token: 0x06005477 RID: 21623 RVA: 0x0020CB9C File Offset: 0x0020AD9C
		private static void MakeRigidbodyParkedImmediately(GameObject instance)
		{
			if (instance == null)
			{
				return;
			}
			foreach (Rigidbody rigidbody in instance.GetComponentsInChildren<Rigidbody>(true))
			{
				if (!(rigidbody == null))
				{
					if (!rigidbody.isKinematic)
					{
						rigidbody.velocity = Vector3.zero;
						rigidbody.angularVelocity = Vector3.zero;
						rigidbody.isKinematic = true;
					}
					rigidbody.Sleep();
				}
			}
		}

		// Token: 0x06005478 RID: 21624 RVA: 0x0020CC04 File Offset: 0x0020AE04
		private SpawnedVehicleLifetime InstallLifetime(GameObject vehicle)
		{
			if (vehicle == null)
			{
				return null;
			}
			SpawnedVehicleLifetime spawnedVehicleLifetime = vehicle.GetComponent<SpawnedVehicleLifetime>();
			if (spawnedVehicleLifetime == null)
			{
				spawnedVehicleLifetime = vehicle.AddComponent<SpawnedVehicleLifetime>();
			}
			spawnedVehicleLifetime.Initialize(this);
			return spawnedVehicleLifetime;
		}

		// Token: 0x06005479 RID: 21625 RVA: 0x0020CC3C File Offset: 0x0020AE3C
		private void SubscribeToMountEvents(GameObject vehicle)
		{
			if (vehicle == null || this._mountCallbacks.ContainsKey(vehicle))
			{
				return;
			}
			UnityAction unityAction = delegate
			{
				this.MarkVehicleClaimed(vehicle);
			};
			List<UnityEvent> list = new List<UnityEvent>(2);
			vVehicleController[] componentsInChildren = vehicle.GetComponentsInChildren<vVehicleController>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				VehicleSpawnManager.AddMountEvent((componentsInChildren[i] != null) ? componentsInChildren[i].OnMount : null, unityAction, list);
			}
			vBikeController[] componentsInChildren2 = vehicle.GetComponentsInChildren<vBikeController>(true);
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				VehicleSpawnManager.AddMountEvent((componentsInChildren2[j] != null) ? componentsInChildren2[j].OnMount : null, unityAction, list);
			}
			SkateboardInteractable[] componentsInChildren3 = vehicle.GetComponentsInChildren<SkateboardInteractable>(true);
			for (int k = 0; k < componentsInChildren3.Length; k++)
			{
				VehicleSpawnManager.AddMountEvent((componentsInChildren3[k] != null) ? componentsInChildren3[k].OnMount : null, unityAction, list);
			}
			SimpleRollerSkatesInteractable[] componentsInChildren4 = vehicle.GetComponentsInChildren<SimpleRollerSkatesInteractable>(true);
			for (int l = 0; l < componentsInChildren4.Length; l++)
			{
				VehicleSpawnManager.AddMountEvent((componentsInChildren4[l] != null) ? componentsInChildren4[l].OnMount : null, unityAction, list);
			}
			this._mountCallbacks[vehicle] = unityAction;
			this._mountEvents[vehicle] = list;
			if (list.Count == 0 && this.debugLogs)
			{
				Debug.LogWarning("[VehicleSpawnManager] '" + vehicle.name + "' exposes no supported mount event; mounted-state checks will still protect it during chunk lifecycle.", vehicle);
			}
		}

		// Token: 0x0600547A RID: 21626 RVA: 0x0020CDF0 File Offset: 0x0020AFF0
		private static void AddMountEvent(UnityEvent mountEvent, UnityAction callback, List<UnityEvent> destination)
		{
			if (mountEvent == null || destination.Contains(mountEvent))
			{
				return;
			}
			mountEvent.AddListener(callback);
			destination.Add(mountEvent);
		}

		// Token: 0x0600547B RID: 21627 RVA: 0x0020CE10 File Offset: 0x0020B010
		private void UnsubscribeMountEvents(GameObject vehicle)
		{
			UnityAction unityAction;
			if (vehicle == null || !this._mountCallbacks.TryGetValue(vehicle, out unityAction))
			{
				return;
			}
			List<UnityEvent> list;
			if (this._mountEvents.TryGetValue(vehicle, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					UnityEvent unityEvent = list[i];
					if (unityEvent != null)
					{
						unityEvent.RemoveListener(unityAction);
					}
				}
			}
			this._mountEvents.Remove(vehicle);
			this._mountCallbacks.Remove(vehicle);
		}

		// Token: 0x0600547C RID: 21628 RVA: 0x0020CE7F File Offset: 0x0020B07F
		private void MarkVehicleClaimed(GameObject vehicle)
		{
			if (vehicle == null || !this._claimedVehicles.Add(vehicle))
			{
				return;
			}
			if (this.debugLogs)
			{
				Debug.Log("[VehicleSpawnManager] '" + vehicle.name + "' claimed by player; origin chunk lifetime detached.", vehicle);
			}
		}

		// Token: 0x0600547D RID: 21629 RVA: 0x0020CEBC File Offset: 0x0020B0BC
		private bool IsProtectedFromOriginChunk(GameObject vehicle)
		{
			return vehicle != null && (this._claimedVehicles.Contains(vehicle) || VehicleSpawnManager.IsVehicleMounted(vehicle));
		}

		// Token: 0x0600547E RID: 21630 RVA: 0x0020CEE0 File Offset: 0x0020B0E0
		private static bool IsVehicleMounted(GameObject vehicle)
		{
			if (vehicle == null)
			{
				return false;
			}
			foreach (MonoBehaviour monoBehaviour in vehicle.GetComponentsInChildren<MonoBehaviour>(true))
			{
				if (monoBehaviour != null)
				{
					IInteractable interactable = monoBehaviour as IInteractable;
					if (interactable != null && interactable.IsBeingUsed)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600547F RID: 21631 RVA: 0x0020CF30 File Offset: 0x0020B130
		private int DestroyTrackedVehicles(bool preserveMounted)
		{
			this.GatherTrackedVehicles(this._scratchVehicles);
			int num = 0;
			for (int i = 0; i < this._scratchVehicles.Count; i++)
			{
				GameObject gameObject = this._scratchVehicles[i];
				if (!(gameObject == null))
				{
					if (preserveMounted && VehicleSpawnManager.IsVehicleMounted(gameObject))
					{
						this.MarkVehicleClaimed(gameObject);
						this.UntrackVehicleFromPoints(gameObject);
					}
					else
					{
						this.DestroyManagedVehicle(gameObject);
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06005480 RID: 21632 RVA: 0x0020CFA0 File Offset: 0x0020B1A0
		internal void ExpireVehicleFromLifetime(GameObject vehicle)
		{
			if (vehicle == null)
			{
				return;
			}
			this._scratchPointKeys.Clear();
			foreach (KeyValuePair<VehicleSpawnPoint, GameObject> keyValuePair in this._spawnedByPoint)
			{
				if (keyValuePair.Value == vehicle)
				{
					this._scratchPointKeys.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < this._scratchPointKeys.Count; i++)
			{
				VehicleSpawnPoint vehicleSpawnPoint = this._scratchPointKeys[i];
				if (vehicleSpawnPoint != null)
				{
					this._resolvedWithoutVehicle.Add(vehicleSpawnPoint);
				}
			}
			this._scratchPointKeys.Clear();
			this.DestroyManagedVehicle(vehicle);
		}

		// Token: 0x06005481 RID: 21633 RVA: 0x0020D064 File Offset: 0x0020B264
		private void GatherTrackedVehicles(List<GameObject> destination)
		{
			destination.Clear();
			HashSet<GameObject> hashSet = new HashSet<GameObject>();
			foreach (KeyValuePair<VehicleSpawnPoint, GameObject> keyValuePair in this._spawnedByPoint)
			{
				if (keyValuePair.Value != null && hashSet.Add(keyValuePair.Value))
				{
					destination.Add(keyValuePair.Value);
				}
			}
			foreach (GameObject gameObject in this._claimedVehicles)
			{
				if (gameObject != null && hashSet.Add(gameObject))
				{
					destination.Add(gameObject);
				}
			}
			foreach (GameObject gameObject2 in this._despawnQueue)
			{
				if (gameObject2 != null && hashSet.Add(gameObject2))
				{
					destination.Add(gameObject2);
				}
			}
		}

		// Token: 0x06005482 RID: 21634 RVA: 0x0020D188 File Offset: 0x0020B388
		private void DestroyManagedVehicle(GameObject vehicle)
		{
			if (vehicle == null)
			{
				return;
			}
			this.UntrackVehicleFromPoints(vehicle);
			this._claimedVehicles.Remove(vehicle);
			this._queuedDespawns.Remove(vehicle);
			this.UnsubscribeMountEvents(vehicle);
			if (vehicle != null)
			{
				vehicle.SetActive(false);
				if (Application.isPlaying)
				{
					Object.Destroy(vehicle);
					return;
				}
				Object.DestroyImmediate(vehicle);
			}
		}

		// Token: 0x06005483 RID: 21635 RVA: 0x0020D1E8 File Offset: 0x0020B3E8
		private void UntrackVehicleFromPoints(GameObject vehicle)
		{
			this._scratchPointKeys.Clear();
			foreach (KeyValuePair<VehicleSpawnPoint, GameObject> keyValuePair in this._spawnedByPoint)
			{
				if (keyValuePair.Value == vehicle)
				{
					this._scratchPointKeys.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < this._scratchPointKeys.Count; i++)
			{
				this._spawnedByPoint.Remove(this._scratchPointKeys[i]);
			}
			this._scratchPointKeys.Clear();
		}

		// Token: 0x06005484 RID: 21636 RVA: 0x0020D29C File Offset: 0x0020B49C
		private void PruneDestroyedVehicleState()
		{
			this._scratchPointKeys.Clear();
			foreach (KeyValuePair<VehicleSpawnPoint, GameObject> keyValuePair in this._spawnedByPoint)
			{
				if (keyValuePair.Value == null)
				{
					this._scratchPointKeys.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < this._scratchPointKeys.Count; i++)
			{
				VehicleSpawnPoint vehicleSpawnPoint = this._scratchPointKeys[i];
				this._spawnedByPoint.Remove(vehicleSpawnPoint);
				this._resolvedWithoutVehicle.Remove(vehicleSpawnPoint);
				this.QueuePointForSpawn(vehicleSpawnPoint);
			}
			this._scratchPointKeys.Clear();
			this._scratchVehicleKeys.Clear();
			foreach (GameObject gameObject in this._claimedVehicles)
			{
				if (gameObject == null)
				{
					this._scratchVehicleKeys.Add(gameObject);
				}
			}
			for (int j = 0; j < this._scratchVehicleKeys.Count; j++)
			{
				this._claimedVehicles.Remove(this._scratchVehicleKeys[j]);
			}
			this._scratchVehicleKeys.Clear();
			foreach (KeyValuePair<GameObject, UnityAction> keyValuePair2 in this._mountCallbacks)
			{
				if (keyValuePair2.Key == null)
				{
					this._scratchVehicleKeys.Add(keyValuePair2.Key);
				}
			}
			for (int k = 0; k < this._scratchVehicleKeys.Count; k++)
			{
				this.UnsubscribeMountEvents(this._scratchVehicleKeys[k]);
			}
			this._scratchVehicleKeys.Clear();
		}

		// Token: 0x06005485 RID: 21637 RVA: 0x0020D498 File Offset: 0x0020B698
		private void ResetPointTracking()
		{
			this._chunkPoints.Clear();
			this._chunkActiveState.Clear();
			this._inactiveChunkPoints.Clear();
			this._resolvedWithoutVehicle.Clear();
			this._guaranteedRetryAt.Clear();
			this._spawnedByPoint.Clear();
			this._spawnQueue.Clear();
			this._queuedSpawnPoints.Clear();
			this._despawnQueue.Clear();
			this._queuedDespawns.Clear();
		}

		// Token: 0x06005486 RID: 21638 RVA: 0x0020D514 File Offset: 0x0020B714
		private Transform EnsureSpawnedVehiclesContainer()
		{
			if (this.spawnedVehiclesContainer == null)
			{
				GameObject gameObject = new GameObject("SpawnedParkedVehicles");
				this.spawnedVehiclesContainer = gameObject.transform;
				this.spawnedVehiclesContainer.SetParent(base.transform, false);
			}
			if (this.spawnedVehiclesContainer.GetComponent<WorldOriginGroup>() == null)
			{
				this.spawnedVehiclesContainer.gameObject.AddComponent<WorldOriginGroup>();
			}
			return this.spawnedVehiclesContainer;
		}

		// Token: 0x06005487 RID: 21639 RVA: 0x0020D584 File Offset: 0x0020B784
		private static Vector3 GetSpawnPositionWithPrefabYOffset(VehicleSpawnPoint point, GameObject prefab)
		{
			Vector3 spawnPosition = point.SpawnPosition;
			spawnPosition.y += prefab.transform.position.y;
			return spawnPosition;
		}

		// Token: 0x040054A4 RID: 21668
		private const float GuaranteedRetryDelaySeconds = 0.5f;

		// Token: 0x040054A5 RID: 21669
		[Header("References")]
		[SerializeField]
		private Transform spawnPointsRoot;

		// Token: 0x040054A6 RID: 21670
		[SerializeField]
		private Transform spawnedVehiclesContainer;

		// Token: 0x040054A7 RID: 21671
		[Header("Runtime")]
		[SerializeField]
		private bool autoSpawnOnStart;

		// Token: 0x040054A8 RID: 21672
		[SerializeField]
		private bool clearSpawnedVehiclesOnStart = true;

		// Token: 0x040054A9 RID: 21673
		[SerializeField]
		private int maxSpawnOpsPerFrame = 2;

		// Token: 0x040054AA RID: 21674
		[SerializeField]
		private float spawnFrameBudgetMs = 1f;

		// Token: 0x040054AB RID: 21675
		[SerializeField]
		private bool bindToChunkManager = true;

		// Token: 0x040054AC RID: 21676
		[SerializeField]
		private LayerMask occupancyMask = -1;

		// Token: 0x040054AD RID: 21677
		[SerializeField]
		private bool debugLogs;

		// Token: 0x040054AE RID: 21678
		[Header("Lifetime expiry audio")]
		[SerializeField]
		private AudioClip vehicleExpirySfx;

		// Token: 0x040054AF RID: 21679
		[SerializeField]
		[Range(0f, 1f)]
		private float vehicleExpirySfxVolume = 0.45f;

		// Token: 0x040054B0 RID: 21680
		[SerializeField]
		[Range(0.5f, 2f)]
		private float vehicleExpirySfxPitch = 0.88f;

		// Token: 0x040054B1 RID: 21681
		private readonly List<VehicleSpawnPoint> _scratchPoints = new List<VehicleSpawnPoint>(64);

		// Token: 0x040054B2 RID: 21682
		private readonly List<VehicleSpawnPoint> _componentScratchPoints = new List<VehicleSpawnPoint>(64);

		// Token: 0x040054B3 RID: 21683
		private readonly HashSet<VehicleSpawnPoint> _scratchPointSet = new HashSet<VehicleSpawnPoint>();

		// Token: 0x040054B4 RID: 21684
		private readonly List<GameObject> _chunkPointRootScratch = new List<GameObject>(8);

		// Token: 0x040054B5 RID: 21685
		private readonly List<GameObject> _scratchVehicles = new List<GameObject>(256);

		// Token: 0x040054B6 RID: 21686
		private readonly List<VehicleSpawnPoint> _scratchPointKeys = new List<VehicleSpawnPoint>(256);

		// Token: 0x040054B7 RID: 21687
		private readonly List<GameObject> _scratchVehicleKeys = new List<GameObject>(256);

		// Token: 0x040054B8 RID: 21688
		private readonly Collider[] _occupancyBuffer = new Collider[64];

		// Token: 0x040054B9 RID: 21689
		private readonly RaycastHit[] _supportBuffer = new RaycastHit[32];

		// Token: 0x040054BA RID: 21690
		private readonly Dictionary<ChunkInstance, List<VehicleSpawnPoint>> _chunkPoints = new Dictionary<ChunkInstance, List<VehicleSpawnPoint>>(64);

		// Token: 0x040054BB RID: 21691
		private readonly Dictionary<ChunkInstance, bool> _chunkActiveState = new Dictionary<ChunkInstance, bool>(64);

		// Token: 0x040054BC RID: 21692
		private readonly Dictionary<VehicleSpawnPoint, GameObject> _spawnedByPoint = new Dictionary<VehicleSpawnPoint, GameObject>(256);

		// Token: 0x040054BD RID: 21693
		private readonly HashSet<VehicleSpawnPoint> _inactiveChunkPoints = new HashSet<VehicleSpawnPoint>();

		// Token: 0x040054BE RID: 21694
		private readonly HashSet<VehicleSpawnPoint> _resolvedWithoutVehicle = new HashSet<VehicleSpawnPoint>();

		// Token: 0x040054BF RID: 21695
		private readonly Dictionary<VehicleSpawnPoint, float> _guaranteedRetryAt = new Dictionary<VehicleSpawnPoint, float>(4);

		// Token: 0x040054C0 RID: 21696
		private readonly Queue<VehicleSpawnPoint> _spawnQueue = new Queue<VehicleSpawnPoint>(256);

		// Token: 0x040054C1 RID: 21697
		private readonly HashSet<VehicleSpawnPoint> _queuedSpawnPoints = new HashSet<VehicleSpawnPoint>();

		// Token: 0x040054C2 RID: 21698
		private readonly Queue<GameObject> _despawnQueue = new Queue<GameObject>(256);

		// Token: 0x040054C3 RID: 21699
		private readonly HashSet<GameObject> _queuedDespawns = new HashSet<GameObject>();

		// Token: 0x040054C4 RID: 21700
		private readonly HashSet<GameObject> _claimedVehicles = new HashSet<GameObject>();

		// Token: 0x040054C5 RID: 21701
		private readonly Dictionary<GameObject, UnityAction> _mountCallbacks = new Dictionary<GameObject, UnityAction>();

		// Token: 0x040054C6 RID: 21702
		private readonly Dictionary<GameObject, List<UnityEvent>> _mountEvents = new Dictionary<GameObject, List<UnityEvent>>();

		// Token: 0x040054C7 RID: 21703
		private ChunkManager _chunkManager;

		// Token: 0x040054C8 RID: 21704
		private bool _isChunkManagerBound;

		// Token: 0x040054C9 RID: 21705
		private bool _isSceneInitialized;

		// Token: 0x040054CA RID: 21706
		private bool _advancedMechanicsBlocked;

		// Token: 0x040054CB RID: 21707
		private int _configuredSceneHandle = -1;
	}
}
