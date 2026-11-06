using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VehicleSpawnPlugin
{
	// Token: 0x02000008 RID: 8
	public class VehicleSpawner : MonoBehaviour
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00004F04 File Offset: 0x00003104
		public int ActiveVehicleCount
		{
			get
			{
				return this._activeVehicles.Count;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00004F11 File Offset: 0x00003111
		public int LoadedChunkCount
		{
			get
			{
				return this._loadedChunks.Count;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00004F1E File Offset: 0x0000311E
		public SpawnProfile Profile
		{
			get
			{
				return this.spawnProfile;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00004F26 File Offset: 0x00003126
		public Transform PoolRoot
		{
			get
			{
				if (this._pool == null)
				{
					return null;
				}
				return this._pool.Root;
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00004F40 File Offset: 0x00003140
		private void Awake()
		{
			this._chunkProvider = this.chunkProviderObject as IChunkProvider;
			if (this._chunkProvider == null)
			{
				Debug.LogError("[VSP] chunkProviderObject does not implement IChunkProvider — plugin disabled.");
				base.enabled = false;
				return;
			}
			GameObject gameObject = new GameObject("VehiclePool");
			gameObject.transform.SetParent(base.transform);
			this._pool = new VehiclePool(gameObject.transform);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004FA8 File Offset: 0x000031A8
		private void Start()
		{
			if (this.spawnProfile == null)
			{
				Debug.LogError("[VSP] SpawnProfile missing.");
				base.enabled = false;
				return;
			}
			if (this.playerTransform == null)
			{
				Debug.LogError("[VSP] PlayerTransform missing.");
				base.enabled = false;
				return;
			}
			this._mainCamera = Camera.main;
			this._pool.Initialize(this.spawnProfile);
			this._chunkProvider.OnChunkLoaded += this.HandleChunkLoaded;
			this._chunkProvider.OnChunkUnloaded += this.HandleChunkUnloaded;
			foreach (ChunkInfo chunkInfo in this._chunkProvider.GetActiveChunks())
			{
				this.HandleChunkLoaded(chunkInfo);
			}
			base.StartCoroutine(this.SpawnRoutine());
			base.StartCoroutine(this.DespawnRoutine());
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000050A0 File Offset: 0x000032A0
		private void HandleChunkLoaded(ChunkInfo info)
		{
			if (info.chunkObject == null)
			{
				return;
			}
			ChunkData component = info.chunkObject.GetComponent<ChunkData>();
			if (component == null)
			{
				Debug.LogWarning("[VSP] Chunk '" + info.id + "' has no ChunkData — ignored.");
				return;
			}
			this._loadedChunks[info.id] = component;
			foreach (TrackData trackData in component.tracks)
			{
				if (trackData != null && !this._trackCounts.ContainsKey(trackData))
				{
					this._trackCounts[trackData] = 0;
				}
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00005164 File Offset: 0x00003364
		private void HandleChunkUnloaded(ChunkInfo info)
		{
			ChunkData chunkData;
			if (!this._loadedChunks.TryGetValue(info.id, out chunkData))
			{
				return;
			}
			List<GameObject> list = new List<GameObject>();
			foreach (KeyValuePair<GameObject, TrackData> keyValuePair in this._activeVehicles)
			{
				if (chunkData.tracks.Contains(keyValuePair.Value))
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (GameObject gameObject in list)
			{
				if (VspDiag.Carry)
				{
					Debug.Log(string.Format("[CarryDiag] ChunkUnload '{0}' → despawning '{1}' HasPassenger={2} frame={3}", new object[]
					{
						info.id,
						gameObject.name,
						VehicleSpawner.HasPassenger(gameObject),
						Time.frameCount
					}));
				}
				VehicleSpawner.DetachPassengerBeforeDespawn(gameObject);
				this.DespawnVehicleMaybeFade(gameObject, "chunkUnload", false);
			}
			foreach (TrackData trackData in chunkData.tracks)
			{
				this._trackCounts.Remove(trackData);
			}
			this._loadedChunks.Remove(info.id);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000052E4 File Offset: 0x000034E4
		private static IEnumerator WaitForSimulationSeconds(float seconds)
		{
			return VehicleSimulationPause.WaitForSimulationSeconds(seconds);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000052EC File Offset: 0x000034EC
		private static IEnumerator WaitForSimulationResume()
		{
			return VehicleSimulationPause.WaitForSimulationResume();
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000052F3 File Offset: 0x000034F3
		private IEnumerator SpawnRoutine()
		{
			for (;;)
			{
				yield return VehicleSpawner.WaitForSimulationSeconds(this.spawnProfile.spawnInterval);
				if (this._pool.ActiveCount < this.spawnProfile.globalMaxVehicles)
				{
					this.TrySpawnVehicle();
				}
			}
			yield break;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00005304 File Offset: 0x00003504
		private void TrySpawnVehicle()
		{
			if (VehicleSimulationPause.IsPaused)
			{
				return;
			}
			List<ValueTuple<ChunkData, TrackData, Transform>> list = new List<ValueTuple<ChunkData, TrackData, Transform>>();
			foreach (ChunkData chunkData in this._loadedChunks.Values)
			{
				if (this.CountVehiclesInChunk(chunkData) < chunkData.maxVehiclesInChunk)
				{
					foreach (TrackData trackData in chunkData.tracks)
					{
						if (!(trackData == null) && trackData.HasValidPath && trackData.spawnPoints.Count != 0 && this.IsTrackAvailable(trackData) && string.IsNullOrEmpty(trackData.intersectionGroupId))
						{
							foreach (Transform transform in trackData.spawnPoints)
							{
								if (!(transform == null) && !this._invalidSpawnPoints.Contains(transform))
								{
									list.Add(new ValueTuple<ChunkData, TrackData, Transform>(chunkData, trackData, transform));
								}
							}
						}
					}
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			VehicleSpawner.Shuffle<ValueTuple<ChunkData, TrackData, Transform>>(list);
			foreach (ValueTuple<ChunkData, TrackData, Transform> valueTuple in list)
			{
				TrackData item = valueTuple.Item2;
				Transform item2 = valueTuple.Item3;
				Vector3 position = item2.position;
				float num = Vector3.Distance(this.playerTransform.position, position);
				if (num >= this.spawnProfile.minSpawnDistanceFromPlayer && num <= this.spawnProfile.maxSpawnDistanceFromPlayer && (!this.spawnProfile.spawnOutsideFrustum || !this.IsVisibleFromCamera(position)) && !this.IsSpawnPointOccupied(position, item2.forward))
				{
					VehicleDefinition weightedRandom = this.GetWeightedRandom();
					if (!(weightedRandom == null))
					{
						float closestT = item.GetClosestT(position);
						Vector3 worldTangent = item.GetWorldTangent(closestT);
						if (Vector3.Dot(item2.forward, worldTangent) < 0f)
						{
							if (this._invalidSpawnPoints.Add(item2) && VspDiag.Curve)
							{
								Debug.LogWarning(string.Concat(new string[]
								{
									"[VehicleSpawner] SpawnPoint '",
									item2.name,
									"' on track ",
									string.Format("'{0}' faces AGAINST the lane tangent at t={1:0.00} ", item.name, closestT),
									"— candidate skipped (check the authored spawn point)."
								}), item2);
							}
						}
						else
						{
							GameObject gameObject = this._pool.Spawn(weightedRandom, position, item2.rotation);
							if (!(gameObject == null))
							{
								VehicleAI component = gameObject.GetComponent<VehicleAI>();
								if (component != null)
								{
									float num2 = Random.Range(1f - weightedRandom.speedVariation, 1f + weightedRandom.speedVariation);
									component.cruiseSpeed = weightedRandom.cruiseSpeed * num2;
									component.turnSpeed = weightedRandom.turnSpeed;
									component.vehicleLength = weightedRandom.vehicleLength;
									component.detectionProfile = weightedRandom.detectionProfile;
									component.rotationOffset = weightedRandom.rotationOffset;
									component.simLodPlayer = this.playerTransform;
									component.fullSimRadius = this.spawnProfile.fullSimRadius;
									component.ignoreOtherVehicles = this.spawnProfile.oneVehiclePerLane;
									component.Initialize(item, closestT);
									component.SetInitialSpeed(component.cruiseSpeed * this.spawnInitialSpeedFraction);
									component.OnApproachingEnd += this.HandleVehicleApproachingEnd;
									component.OnEndReached += this.HandleVehicleEndReached;
									component.OnTransitioned += this.HandleVehicleTransitioned;
									component.OnStuck += this.HandleVehicleStuck;
								}
								this.RegisterVehicle(gameObject, item);
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000575C File Offset: 0x0000395C
		private IEnumerator DespawnRoutine()
		{
			List<GameObject> toRemove = new List<GameObject>();
			for (;;)
			{
				yield return VehicleSpawner.WaitForSimulationSeconds(this.spawnProfile.despawnCheckInterval);
				toRemove.Clear();
				foreach (KeyValuePair<GameObject, TrackData> keyValuePair in this._activeVehicles)
				{
					GameObject key = keyValuePair.Key;
					if (!VehicleSpawner.HasPassenger(key) && !this._fading.Contains(key) && this.ShouldDistanceDespawn(key.transform.position))
					{
						toRemove.Add(key);
					}
				}
				for (int i = 0; i < toRemove.Count; i++)
				{
					if (VspDiag.Carry)
					{
						Debug.Log(string.Format("[CarryDiag] DistanceDespawn '{0}' dist={1:F1} HasPassenger={2} frame={3}", new object[]
						{
							toRemove[i].name,
							Vector3.Distance(this.playerTransform.position, toRemove[i].transform.position),
							VehicleSpawner.HasPassenger(toRemove[i]),
							Time.frameCount
						}));
					}
					this.DespawnVehicle(toRemove[i], "distance");
				}
			}
			yield break;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000576C File Offset: 0x0000396C
		private bool ShouldDistanceDespawn(Vector3 pos)
		{
			float num = Vector3.Distance(this.playerTransform.position, pos);
			return num >= this.spawnProfile.safeDespawnRadius && (num > this.spawnProfile.despawnDistance || (num > this.spawnProfile.maxSpawnDistanceFromPlayer && (!this.spawnProfile.despawnOnlyOutsideFrustum || !this.IsVisibleFromCamera(pos))));
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000057D4 File Offset: 0x000039D4
		private void DespawnVehicle(GameObject go, string reason = "unspecified")
		{
			VehicleAI component = go.GetComponent<VehicleAI>();
			if (component != null && component.HasPassenger && !reason.StartsWith("forceAll"))
			{
				if (VspDiag.Carry)
				{
					Debug.LogError(string.Format("[CarryDiag] DespawnVehicle '{0}' reason={1} SKIPPED — HasPassenger=TRUE (guard). Car stays, stopped. frame={2}", go.name, reason, Time.frameCount), go);
				}
				component.Stop();
				return;
			}
			if (VspDiag.Carry)
			{
				if (component != null && component.HasPassenger)
				{
					Debug.LogError(string.Format("[CarryDiag] DespawnVehicle '{0}' reason={1} with HasPassenger=TRUE — protection bypassed! frame={2}", go.name, reason, Time.frameCount), go);
				}
				else
				{
					Debug.Log(string.Format("[CarryDiag] DespawnVehicle '{0}' reason={1} HasPassenger=false frame={2}", go.name, reason, Time.frameCount));
				}
			}
			if (component != null)
			{
				component.OnApproachingEnd -= this.HandleVehicleApproachingEnd;
				component.OnEndReached -= this.HandleVehicleEndReached;
				component.OnTransitioned -= this.HandleVehicleTransitioned;
				component.OnStuck -= this.HandleVehicleStuck;
				component.Stop();
			}
			VehicleFader component2 = go.GetComponent<VehicleFader>();
			if (component2 != null)
			{
				component2.RestoreImmediate();
			}
			this._fading.Remove(go);
			this._parkedUntilUnseen.Remove(go);
			this._abandonedUntilUnseen.Remove(go);
			this.UnregisterVehicle(go);
			this._pool.Despawn(go);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000593C File Offset: 0x00003B3C
		private void DespawnVehicleMaybeFade(GameObject go, string reason = "unspecified", bool parkIfVisible = false)
		{
			if (go == null)
			{
				return;
			}
			VehicleAI component = go.GetComponent<VehicleAI>();
			if (component != null && component.HasPassenger)
			{
				if (VspDiag.Carry)
				{
					Debug.LogError(string.Format("[CarryDiag] DespawnVehicleMaybeFade '{0}' reason={1} SKIPPED — HasPassenger=TRUE (guard). Car stays, stopped. frame={2}", go.name, reason, Time.frameCount), go);
				}
				component.Stop();
				return;
			}
			if (component != null && component.IsAbandoned && reason != "chunkUnload")
			{
				return;
			}
			if (VspDiag.Carry)
			{
				bool flag = VehicleSpawner.HasPassenger(go);
				Debug.Log(string.Format("[CarryDiag] DespawnVehicleMaybeFade '{0}' reason={1} visible={2} fadingCount={3} HasPassenger={4} frame={5}", new object[]
				{
					go.name,
					reason,
					this.IsVisibleFromCamera(go.transform.position),
					this._fading.Count,
					flag,
					Time.frameCount
				}));
				if (flag)
				{
					Debug.LogError(string.Concat(new string[] { "[CarryDiag] DespawnVehicleMaybeFade '", go.name, "' reason=", reason, " entered with HasPassenger=TRUE — no passenger guard on this path!" }), go);
				}
			}
			if (!this.IsVisibleFromCamera(go.transform.position) || this._fading.Count >= this.spawnProfile.maxConcurrentFades)
			{
				this.DespawnVehicle(go, reason + "/instant");
				return;
			}
			if (parkIfVisible && this._parkedUntilUnseen.Count < 6 && this._parkedUntilUnseen.Add(go))
			{
				base.StartCoroutine(this.ParkUntilUnseen(go, reason));
				return;
			}
			VehicleAI component2 = go.GetComponent<VehicleAI>();
			if (component2 != null)
			{
				component2.Stop();
			}
			this._fading.Add(go);
			VehicleFader fader = VehicleFader.GetOrAdd(go);
			fader.BeginFade(this.spawnProfile.visibleDespawnFadeDuration, delegate
			{
				if (VehicleSpawner.HasPassenger(go))
				{
					if (VspDiag.Carry)
					{
						Debug.LogError(string.Format("[CarryDiag] Fade completed on '{0}' (reason={1}) but HasPassenger became TRUE during the fade — despawn CANCELLED, materials restored. frame={2}", go.name, reason, Time.frameCount), go);
					}
					this._fading.Remove(go);
					fader.RestoreImmediate();
					VehicleAI component3 = go.GetComponent<VehicleAI>();
					if (component3 != null)
					{
						component3.Stop();
					}
					return;
				}
				this._fading.Remove(go);
				this.DespawnVehicle(go, reason + "/fadeComplete");
			});
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00005BB0 File Offset: 0x00003DB0
		private void HandleVehicleApproachingEnd(VehicleAI vehicle)
		{
			bool flag;
			TrackData trackData = this.FindConnectingTrack(vehicle.ExitPosition, vehicle.ExitTangent, out flag);
			if (VspDiag.Curve)
			{
				Debug.Log(string.Format("[CurveDiag] ApproachingEnd '{0}' track='{1}' → next={2} frame={3}", new object[]
				{
					vehicle.name,
					VehicleSpawner.TrackName(vehicle.CurrentTrack),
					(trackData != null) ? VehicleSpawner.TrackName(trackData) : "NULL (dead end!)",
					Time.frameCount
				}));
			}
			if (trackData != null)
			{
				this.ReserveAndSetNext(vehicle, trackData);
				return;
			}
			if (flag)
			{
				base.StartCoroutine(this.WaitForFreeSuccessor(vehicle));
				return;
			}
			vehicle.MarkDeadEndAhead();
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00005C58 File Offset: 0x00003E58
		private void HandleVehicleEndReached(VehicleAI vehicle)
		{
			if (VspDiag.Carry || VspDiag.Curve)
			{
				Debug.Log(string.Format("[CarryDiag][CurveDiag] EndReached '{0}' track='{1}' HasPassenger={2} frame={3}", new object[]
				{
					vehicle.name,
					VehicleSpawner.TrackName(vehicle.CurrentTrack),
					vehicle.HasPassenger,
					Time.frameCount
				}));
			}
			bool flag;
			TrackData trackData = this.FindConnectingTrack(vehicle.ExitPosition, vehicle.ExitTangent, out flag);
			if (trackData != null)
			{
				this.ReserveAndSetNext(vehicle, trackData);
				return;
			}
			if (flag)
			{
				base.StartCoroutine(this.WaitForFreeSuccessor(vehicle));
				return;
			}
			this.DespawnVehicleMaybeFade(vehicle.gameObject, "endReached", true);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00005D05 File Offset: 0x00003F05
		private IEnumerator ParkUntilUnseen(GameObject go, string reason)
		{
			VehicleAI ai = ((go != null) ? go.GetComponent<VehicleAI>() : null);
			if (ai != null)
			{
				ai.SetBlocked(true);
			}
			float waitDuration;
			for (float timeoutRemaining = 12f; timeoutRemaining > 0f; timeoutRemaining -= waitDuration)
			{
				yield return VehicleSpawner.WaitForSimulationResume();
				if (go == null || !this._parkedUntilUnseen.Contains(go))
				{
					break;
				}
				if (ai != null && ai.HasPassenger)
				{
					this._parkedUntilUnseen.Remove(go);
					yield break;
				}
				if (!this.IsVisibleFromCamera(go.transform.position))
				{
					this._parkedUntilUnseen.Remove(go);
					this.DespawnVehicle(go, reason + "/parkedUnseen");
					yield break;
				}
				TrackData trackData;
				if (this._activeVehicles.TryGetValue(go, out trackData) && this.GetTrackCount(trackData) > 1)
				{
					break;
				}
				waitDuration = Mathf.Min(0.4f, timeoutRemaining);
				yield return VehicleSpawner.WaitForSimulationSeconds(waitDuration);
			}
			if (go != null && this._parkedUntilUnseen.Remove(go))
			{
				this.DespawnVehicleMaybeFade(go, reason + "/parkReleased", false);
			}
			yield break;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00005D24 File Offset: 0x00003F24
		public void AbandonUntilUnseen(GameObject go)
		{
			if (go == null || !this._abandonedUntilUnseen.Add(go))
			{
				return;
			}
			VehicleAI component = go.GetComponent<VehicleAI>();
			if (component != null)
			{
				component.SetAbandoned(true);
			}
			base.StartCoroutine(this.AbandonedReclaim(go));
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00005D6E File Offset: 0x00003F6E
		private IEnumerator AbandonedReclaim(GameObject go)
		{
			VehicleAI ai = ((go != null) ? go.GetComponent<VehicleAI>() : null);
			float lingerRemaining = 12f;
			while (go != null && go.activeInHierarchy && this._abandonedUntilUnseen.Contains(go))
			{
				yield return VehicleSpawner.WaitForSimulationResume();
				if (go == null || !go.activeInHierarchy || !this._abandonedUntilUnseen.Contains(go))
				{
					break;
				}
				int num = ((ai != null && ai.HasPassenger) ? 1 : 0);
				bool flag = this.playerTransform == null || (go.transform.position - this.playerTransform.position).sqrMagnitude > 900f;
				if (num == 0 && flag && lingerRemaining <= 0f && !this.IsVisibleFromCamera(go.transform.position))
				{
					this._abandonedUntilUnseen.Remove(go);
					this.DespawnVehicle(go, "abandoned/unseen");
					yield break;
				}
				yield return VehicleSpawner.WaitForSimulationSeconds(0.4f);
				lingerRemaining = Mathf.Max(0f, lingerRemaining - 0.4f);
			}
			if (go != null)
			{
				this._abandonedUntilUnseen.Remove(go);
			}
			yield break;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00005D84 File Offset: 0x00003F84
		private IEnumerator WaitForFreeSuccessor(VehicleAI vehicle)
		{
			if (vehicle == null || !this._waitingForSuccessor.Add(vehicle))
			{
				yield break;
			}
			TrackData curTrack = vehicle.CurrentTrack;
			bool insideCrossing = curTrack != null && !string.IsNullOrEmpty(curTrack.intersectionGroupId);
			if (insideCrossing)
			{
				Debug.LogWarning(string.Format("[JunctionDiag] '{0}' WAITING INSIDE crossing on '{1}' (group={2}) — blocks the junction. frame={3}", new object[]
				{
					vehicle.name,
					curTrack.name,
					curTrack.intersectionGroupId,
					Time.frameCount
				}));
			}
			vehicle.SetBlocked(true);
			float waited = 0f;
			try
			{
				while (vehicle != null && this._activeVehicles.ContainsKey(vehicle.gameObject))
				{
					yield return VehicleSpawner.WaitForSimulationResume();
					if (vehicle == null || !this._activeVehicles.ContainsKey(vehicle.gameObject))
					{
						yield break;
					}
					if (vehicle.IsAbandoned)
					{
						yield break;
					}
					bool flag;
					TrackData trackData = this.FindConnectingTrack(vehicle.ExitPosition, vehicle.ExitTangent, out flag);
					if (trackData != null)
					{
						vehicle.SetBlocked(false);
						this.ReserveAndSetNext(vehicle, trackData);
						yield break;
					}
					if (!flag)
					{
						vehicle.SetBlocked(false);
						this.DespawnVehicleMaybeFade(vehicle.gameObject, "endReached/successorGone", false);
						yield break;
					}
					waited += 0.35f;
					if (insideCrossing && waited >= 12f)
					{
						Debug.LogWarning(string.Format("[JunctionDiag] '{0}' waited {1:F1}s INSIDE crossing '{2}' with no free exit — despawning to unplug the junction. frame={3}", new object[]
						{
							vehicle.name,
							waited,
							curTrack.name,
							Time.frameCount
						}));
						vehicle.SetBlocked(false);
						this.DespawnVehicleMaybeFade(vehicle.gameObject, "junctionWaitTimeout", true);
						yield break;
					}
					yield return VehicleSpawner.WaitForSimulationSeconds(0.35f);
				}
			}
			finally
			{
				if (vehicle != null)
				{
					this._waitingForSuccessor.Remove(vehicle);
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00005D9C File Offset: 0x00003F9C
		private void HandleVehicleStuck(VehicleAI vehicle)
		{
			if (vehicle != null && vehicle.HasPassenger)
			{
				return;
			}
			if (vehicle != null && vehicle.IsAbandoned)
			{
				return;
			}
			if (VspDiag.Carry)
			{
				Debug.Log(string.Format("[CarryDiag] Stuck-despawn '{0}' track='{1}' frame={2}", vehicle.name, VehicleSpawner.TrackName(vehicle.CurrentTrack), Time.frameCount));
			}
			this.DespawnVehicle(vehicle.gameObject, "stuck");
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00005E0F File Offset: 0x0000400F
		private static string TrackName(TrackData t)
		{
			if (!(t != null))
			{
				return "null";
			}
			return t.name;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00005E28 File Offset: 0x00004028
		private static bool HasPassenger(GameObject go)
		{
			VehicleAI vehicleAI = ((go != null) ? go.GetComponent<VehicleAI>() : null);
			return vehicleAI != null && vehicleAI.HasPassenger;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00005E5C File Offset: 0x0000405C
		private static void DetachPassengerBeforeDespawn(GameObject go)
		{
			VehicleAI vehicleAI = ((go != null) ? go.GetComponent<VehicleAI>() : null);
			if (vehicleAI == null || !vehicleAI.HasPassenger)
			{
				return;
			}
			foreach (Transform transform in go.GetComponentsInChildren<Transform>(true))
			{
				if (!(transform == go.transform) && transform.CompareTag("Player"))
				{
					bool flag = false;
					Transform transform2 = transform.parent;
					while (transform2 != null && transform2 != go.transform)
					{
						if (transform2.CompareTag("Player"))
						{
							flag = true;
							break;
						}
						transform2 = transform2.parent;
					}
					if (!flag)
					{
						transform.SetParent(null, true);
						Vector3 eulerAngles = transform.eulerAngles;
						transform.eulerAngles = new Vector3(0f, eulerAngles.y, 0f);
					}
				}
			}
			vehicleAI.HasPassenger = false;
			vehicleAI.Stop();
			Debug.LogWarning("[VSP] Chunk unloaded under a passenger-carrying vehicle — rider detached before despawn.");
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00005F54 File Offset: 0x00004154
		private void ReserveAndSetNext(VehicleAI vehicle, TrackData next)
		{
			TrackData trackData;
			if (this._reservedNext.TryGetValue(vehicle, out trackData) && trackData != next)
			{
				this.DecrementTrackCount(trackData);
			}
			this.IncrementTrackCount(next);
			this._reservedNext[vehicle] = next;
			vehicle.SetNextTrack(next);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00005F9C File Offset: 0x0000419C
		private void HandleVehicleTransitioned(VehicleAI vehicle, TrackData previousTrack)
		{
			if (!this._activeVehicles.ContainsKey(vehicle.gameObject))
			{
				return;
			}
			this.DecrementTrackCount(previousTrack);
			TrackData currentTrack = vehicle.CurrentTrack;
			this._activeVehicles[vehicle.gameObject] = currentTrack;
			TrackData trackData;
			if (this._reservedNext.TryGetValue(vehicle, out trackData) && trackData == currentTrack)
			{
				this._reservedNext.Remove(vehicle);
			}
			else
			{
				this.IncrementTrackCount(currentTrack);
			}
			vehicle.hasIntersectionPriority = currentTrack != null && !string.IsNullOrEmpty(currentTrack.intersectionGroupId);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000602C File Offset: 0x0000422C
		private TrackData FindConnectingTrack(Vector3 exitPos, Vector3 exitTangent, out bool anyExistedButFull)
		{
			float connectionThreshold = this.spawnProfile.connectionThreshold;
			float connectionMinDot = this.spawnProfile.connectionMinDot;
			Vector3 normalized = exitTangent.normalized;
			List<TrackData> list = new List<TrackData>();
			anyExistedButFull = false;
			foreach (ChunkData chunkData in this._loadedChunks.Values)
			{
				foreach (TrackData trackData in chunkData.tracks)
				{
					if (!(trackData == null) && trackData.HasValidPath)
					{
						Vector3 worldPosition = trackData.GetWorldPosition(0f);
						if (Vector3.Distance(exitPos, worldPosition) <= connectionThreshold && Vector3.Dot(normalized, trackData.GetWorldTangent(0f).normalized) >= connectionMinDot)
						{
							if (!this.IsTrackAvailable(trackData))
							{
								anyExistedButFull = true;
							}
							else if (!string.IsNullOrEmpty(trackData.intersectionGroupId) && !this.HasFreeExit(trackData))
							{
								anyExistedButFull = true;
							}
							else
							{
								list.Add(trackData);
							}
						}
					}
				}
			}
			if (list.Count == 0)
			{
				return null;
			}
			return VehicleSpawner.PickWeighted(list);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000617C File Offset: 0x0000437C
		private bool HasFreeExit(TrackData track)
		{
			float connectionThreshold = this.spawnProfile.connectionThreshold;
			float connectionMinDot = this.spawnProfile.connectionMinDot;
			Vector3 worldPosition = track.GetWorldPosition(1f);
			Vector3 normalized = track.GetWorldTangent(1f).normalized;
			foreach (ChunkData chunkData in this._loadedChunks.Values)
			{
				foreach (TrackData trackData in chunkData.tracks)
				{
					if (!(trackData == null) && !(trackData == track) && trackData.HasValidPath && Vector3.Distance(worldPosition, trackData.GetWorldPosition(0f)) <= connectionThreshold && Vector3.Dot(normalized, trackData.GetWorldTangent(0f).normalized) >= connectionMinDot && this.IsTrackAvailable(trackData))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000062AC File Offset: 0x000044AC
		private static TrackData PickWeighted(List<TrackData> candidates)
		{
			float num = 0f;
			for (int i = 0; i < candidates.Count; i++)
			{
				num += Mathf.Max(0f, candidates[i].connectionWeight);
			}
			if (num <= 0f)
			{
				return candidates[Random.Range(0, candidates.Count)];
			}
			float num2 = Random.Range(0f, num);
			float num3 = 0f;
			for (int j = 0; j < candidates.Count; j++)
			{
				num3 += Mathf.Max(0f, candidates[j].connectionWeight);
				if (num2 <= num3)
				{
					return candidates[j];
				}
			}
			return candidates[candidates.Count - 1];
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00006360 File Offset: 0x00004560
		private void RegisterVehicle(GameObject go, TrackData track)
		{
			this._activeVehicles[go] = track;
			this.IncrementTrackCount(track);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00006378 File Offset: 0x00004578
		private void UnregisterVehicle(GameObject go)
		{
			TrackData trackData;
			if (this._activeVehicles.TryGetValue(go, out trackData))
			{
				this.DecrementTrackCount(trackData);
				this._activeVehicles.Remove(go);
			}
			VehicleAI vehicleAI = ((go != null) ? go.GetComponent<VehicleAI>() : null);
			TrackData trackData2;
			if (vehicleAI != null && this._reservedNext.TryGetValue(vehicleAI, out trackData2))
			{
				this.DecrementTrackCount(trackData2);
				this._reservedNext.Remove(vehicleAI);
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000063EC File Offset: 0x000045EC
		private static string GroupKey(TrackData track)
		{
			if (track == null || string.IsNullOrEmpty(track.intersectionGroupId))
			{
				return null;
			}
			return track.transform.root.GetInstanceID().ToString() + "/" + track.intersectionGroupId;
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000643C File Offset: 0x0000463C
		private void IncrementTrackCount(TrackData track)
		{
			if (track == null)
			{
				return;
			}
			this._trackCounts[track] = this.GetTrackCount(track) + 1;
			string text = VehicleSpawner.GroupKey(track);
			if (text != null)
			{
				int num;
				this._groupCounts.TryGetValue(text, out num);
				this._groupCounts[text] = num + 1;
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00006490 File Offset: 0x00004690
		private void DecrementTrackCount(TrackData track)
		{
			if (track == null)
			{
				return;
			}
			this._trackCounts[track] = Mathf.Max(0, this.GetTrackCount(track) - 1);
			string text = VehicleSpawner.GroupKey(track);
			int num;
			if (text != null && this._groupCounts.TryGetValue(text, out num))
			{
				this._groupCounts[text] = Mathf.Max(0, num - 1);
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000064F4 File Offset: 0x000046F4
		private int GetTrackCount(TrackData track)
		{
			int num;
			if (!this._trackCounts.TryGetValue(track, out num))
			{
				return 0;
			}
			return num;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00006514 File Offset: 0x00004714
		private bool IsTrackAvailable(TrackData track)
		{
			if (this.GetTrackCount(track) >= track.maxVehicles)
			{
				return false;
			}
			string text = VehicleSpawner.GroupKey(track);
			if (text != null)
			{
				int num;
				this._groupCounts.TryGetValue(text, out num);
				if (num >= 1)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00006554 File Offset: 0x00004754
		private int CountVehiclesInChunk(ChunkData chunk)
		{
			int num = 0;
			foreach (TrackData trackData in chunk.tracks)
			{
				num += this.GetTrackCount(trackData);
			}
			return num;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000065B0 File Offset: 0x000047B0
		private VehicleDefinition GetWeightedRandom()
		{
			if (this.spawnProfile.vehicleTypes == null || this.spawnProfile.vehicleTypes.Count == 0)
			{
				return null;
			}
			float num = 0f;
			foreach (VehicleDefinition vehicleDefinition in this.spawnProfile.vehicleTypes)
			{
				if (vehicleDefinition != null)
				{
					num += vehicleDefinition.spawnWeight;
				}
			}
			float num2 = Random.Range(0f, num);
			float num3 = 0f;
			foreach (VehicleDefinition vehicleDefinition2 in this.spawnProfile.vehicleTypes)
			{
				if (!(vehicleDefinition2 == null))
				{
					num3 += vehicleDefinition2.spawnWeight;
					if (num2 <= num3)
					{
						return vehicleDefinition2;
					}
				}
			}
			return this.spawnProfile.vehicleTypes[0];
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000066C4 File Offset: 0x000048C4
		private bool IsSpawnPointOccupied(Vector3 spawnPos, Vector3 laneDir)
		{
			float num = this.spawnClearRadius * this.spawnClearRadius;
			if (this.playerTransform != null && (this.playerTransform.position - spawnPos).sqrMagnitude < num)
			{
				return true;
			}
			bool flag = laneDir.sqrMagnitude > 0.0001f;
			if (flag)
			{
				laneDir.Normalize();
			}
			foreach (KeyValuePair<GameObject, TrackData> keyValuePair in this._activeVehicles)
			{
				GameObject key = keyValuePair.Key;
				if (!(key == null))
				{
					Vector3 vector = spawnPos - key.transform.position;
					if (vector.sqrMagnitude < num)
					{
						return true;
					}
					if (flag)
					{
						float num2 = Vector3.Dot(vector, laneDir);
						if (num2 > 0f && num2 <= this.spawnClearBehind && (vector - laneDir * num2).sqrMagnitude < 4f)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000067E8 File Offset: 0x000049E8
		private bool IsVisibleFromCamera(Vector3 pos)
		{
			if (this._mainCamera == null)
			{
				return false;
			}
			float fieldOfView = this._mainCamera.fieldOfView;
			this._mainCamera.fieldOfView = fieldOfView + this.spawnProfile.frustumMargin * 2f;
			GeometryUtility.CalculateFrustumPlanes(this._mainCamera, this._visibilityPlanes);
			this._mainCamera.fieldOfView = fieldOfView;
			return GeometryUtility.TestPlanesAABB(this._visibilityPlanes, new Bounds(pos, Vector3.one));
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00006864 File Offset: 0x00004A64
		private static void Shuffle<T>(List<T> list)
		{
			for (int i = list.Count - 1; i > 0; i--)
			{
				int num = Random.Range(0, i + 1);
				int num2 = i;
				int num3 = num;
				T t = list[num];
				T t2 = list[i];
				list[num2] = t;
				list[num3] = t2;
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000068C0 File Offset: 0x00004AC0
		public void ForceDespawnAll()
		{
			if (VspDiag.Carry)
			{
				Debug.Log(string.Format("[CarryDiag] ForceDespawnAll: {0} vehicles, frame={1}", this._activeVehicles.Count, Time.frameCount));
			}
			this._fading.Clear();
			this._parkedUntilUnseen.Clear();
			this._abandonedUntilUnseen.Clear();
			foreach (GameObject gameObject in new List<GameObject>(this._activeVehicles.Keys))
			{
				VehicleSpawner.DetachPassengerBeforeDespawn(gameObject);
				this.DespawnVehicle(gameObject, "forceAll");
			}
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000697C File Offset: 0x00004B7C
		private void OnDestroy()
		{
			if (this._chunkProvider != null)
			{
				this._chunkProvider.OnChunkLoaded -= this.HandleChunkLoaded;
				this._chunkProvider.OnChunkUnloaded -= this.HandleChunkUnloaded;
			}
			VehiclePool pool = this._pool;
			if (pool == null)
			{
				return;
			}
			pool.DespawnAll();
		}

		// Token: 0x04000069 RID: 105
		[SerializeField]
		private SpawnProfile spawnProfile;

		// Token: 0x0400006A RID: 106
		[SerializeField]
		private Transform playerTransform;

		// Token: 0x0400006B RID: 107
		[SerializeField]
		private MonoBehaviour chunkProviderObject;

		// Token: 0x0400006C RID: 108
		private IChunkProvider _chunkProvider;

		// Token: 0x0400006D RID: 109
		private VehiclePool _pool;

		// Token: 0x0400006E RID: 110
		private Camera _mainCamera;

		// Token: 0x0400006F RID: 111
		private readonly Plane[] _visibilityPlanes = new Plane[6];

		// Token: 0x04000070 RID: 112
		private readonly Dictionary<GameObject, TrackData> _activeVehicles = new Dictionary<GameObject, TrackData>();

		// Token: 0x04000071 RID: 113
		private readonly Dictionary<string, ChunkData> _loadedChunks = new Dictionary<string, ChunkData>();

		// Token: 0x04000072 RID: 114
		private readonly Dictionary<TrackData, int> _trackCounts = new Dictionary<TrackData, int>();

		// Token: 0x04000073 RID: 115
		private readonly HashSet<Transform> _invalidSpawnPoints = new HashSet<Transform>();

		// Token: 0x04000074 RID: 116
		private readonly HashSet<GameObject> _fading = new HashSet<GameObject>();

		// Token: 0x04000075 RID: 117
		private readonly HashSet<VehicleAI> _waitingForSuccessor = new HashSet<VehicleAI>();

		// Token: 0x04000076 RID: 118
		private readonly HashSet<GameObject> _parkedUntilUnseen = new HashSet<GameObject>();

		// Token: 0x04000077 RID: 119
		private const int MaxParkedUntilUnseen = 6;

		// Token: 0x04000078 RID: 120
		private const float ParkedRecheckInterval = 0.4f;

		// Token: 0x04000079 RID: 121
		private const float ParkedTimeoutSeconds = 12f;

		// Token: 0x0400007A RID: 122
		private readonly HashSet<GameObject> _abandonedUntilUnseen = new HashSet<GameObject>();

		// Token: 0x0400007B RID: 123
		private const float AbandonedLingerSeconds = 12f;

		// Token: 0x0400007C RID: 124
		private const float AbandonedReclaimMinPlayerDist = 30f;

		// Token: 0x0400007D RID: 125
		private const float JunctionWaitTimeout = 12f;

		// Token: 0x0400007E RID: 126
		private readonly Dictionary<VehicleAI, TrackData> _reservedNext = new Dictionary<VehicleAI, TrackData>();

		// Token: 0x0400007F RID: 127
		private readonly Dictionary<string, int> _groupCounts = new Dictionary<string, int>();

		// Token: 0x04000080 RID: 128
		[SerializeField]
		[Tooltip("Min clear radius (m) around a spawn point: if any active vehicle is within this distance, the spawn is skipped (anti-overlap).")]
		private float spawnClearRadius = 6f;

		// Token: 0x04000081 RID: 129
		[SerializeField]
		[Range(0f, 1f)]
		[Tooltip("Fraction of cruise speed a car spawns with (spawn-at-cruise). 0 restores the legacy spawn-parked behaviour.")]
		private float spawnInitialSpeedFraction = 0.7f;

		// Token: 0x04000082 RID: 130
		[SerializeField]
		[Tooltip("Clear corridor (m) BEHIND a spawn point along the lane: if a vehicle is approaching within this distance, the spawn is skipped. A car doing 8 m/s twenty metres back reaches a freshly spawned car in ~2.5s — the radial check alone can't see it (probe 2026-07-17: 25+ spawn rear-endings).")]
		private float spawnClearBehind = 22f;
	}
}
