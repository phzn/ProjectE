using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using Features.Detection;
using Features.NPC;
using Features.NPC.StateMachine;
using Features.RenderingPrecision;
using NPCJobs;
using Pathfinding;
using ProjectExhibitionist.Core.SceneManagement;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.AzureSky;

// Token: 0x020000D0 RID: 208
public class NPCZoneManager : MonoBehaviour
{
	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x06000818 RID: 2072 RVA: 0x0003A8EB File Offset: 0x00038AEB
	// (set) Token: 0x06000819 RID: 2073 RVA: 0x0003A8F2 File Offset: 0x00038AF2
	public static NPCZoneManager Instance { get; private set; }

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x0600081A RID: 2074 RVA: 0x0003A8FC File Offset: 0x00038AFC
	private Transform PlayerTransform
	{
		get
		{
			PlayerProximityProvider instance = PlayerProximityProvider.Instance;
			if (instance != null && instance.Player != null)
			{
				return instance.Player;
			}
			return this.player;
		}
	}

	// Token: 0x170001CB RID: 459
	// (get) Token: 0x0600081B RID: 2075 RVA: 0x0003A934 File Offset: 0x00038B34
	private Vector3 PlayerPos
	{
		get
		{
			if (this.HasPreparationFocus)
			{
				return this._preparationPosition;
			}
			PlayerProximityProvider instance = PlayerProximityProvider.Instance;
			if (instance != null && instance.Player != null)
			{
				return instance.Position;
			}
			if (!(this.player != null))
			{
				return Vector3.zero;
			}
			return this.player.position;
		}
	}

	// Token: 0x14000012 RID: 18
	// (add) Token: 0x0600081C RID: 2076 RVA: 0x0003A994 File Offset: 0x00038B94
	// (remove) Token: 0x0600081D RID: 2077 RVA: 0x0003A9CC File Offset: 0x00038BCC
	public event NPCZoneManager.NPCEvent OnNPCSpawned;

	// Token: 0x14000013 RID: 19
	// (add) Token: 0x0600081E RID: 2078 RVA: 0x0003AA04 File Offset: 0x00038C04
	// (remove) Token: 0x0600081F RID: 2079 RVA: 0x0003AA3C File Offset: 0x00038C3C
	public event NPCZoneManager.NPCEvent OnNPCDespawned;

	// Token: 0x14000014 RID: 20
	// (add) Token: 0x06000820 RID: 2080 RVA: 0x0003AA74 File Offset: 0x00038C74
	// (remove) Token: 0x06000821 RID: 2081 RVA: 0x0003AAAC File Offset: 0x00038CAC
	public event NPCZoneManager.NPCEvent OnNPCActivated;

	// Token: 0x14000015 RID: 21
	// (add) Token: 0x06000822 RID: 2082 RVA: 0x0003AAE4 File Offset: 0x00038CE4
	// (remove) Token: 0x06000823 RID: 2083 RVA: 0x0003AB1C File Offset: 0x00038D1C
	public event NPCZoneManager.NPCEvent OnNPCDeactivated;

	// Token: 0x170001CC RID: 460
	// (get) Token: 0x06000824 RID: 2084 RVA: 0x0003AB51 File Offset: 0x00038D51
	public int CurrentNPCCount
	{
		get
		{
			return this._currentNpcCount;
		}
	}

	// Token: 0x170001CD RID: 461
	// (get) Token: 0x06000825 RID: 2085 RVA: 0x0003AB59 File Offset: 0x00038D59
	public int ActiveNPCCount
	{
		get
		{
			return this._activeNpcCount;
		}
	}

	// Token: 0x170001CE RID: 462
	// (get) Token: 0x06000826 RID: 2086 RVA: 0x0003AB61 File Offset: 0x00038D61
	public int MaxNPCCount
	{
		get
		{
			return this._maxTotalNpc;
		}
	}

	// Token: 0x170001CF RID: 463
	// (get) Token: 0x06000827 RID: 2087 RVA: 0x0003AB69 File Offset: 0x00038D69
	public int PendingSpawns
	{
		get
		{
			return this._pendingSpawns.Count;
		}
	}

	// Token: 0x170001D0 RID: 464
	// (get) Token: 0x06000828 RID: 2088 RVA: 0x0003AB76 File Offset: 0x00038D76
	public bool UseJobs
	{
		get
		{
			return this.useJobs;
		}
	}

	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x06000829 RID: 2089 RVA: 0x0003AB7E File Offset: 0x00038D7E
	public float CurrentPopulationHour
	{
		get
		{
			return this._currentPopulationHour;
		}
	}

	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x0600082A RID: 2090 RVA: 0x0003AB86 File Offset: 0x00038D86
	public float TimeOfDayPopulationMultiplier
	{
		get
		{
			return this._timeOfDayMultiplier;
		}
	}

	// Token: 0x170001D3 RID: 467
	// (get) Token: 0x0600082B RID: 2091 RVA: 0x0003AB8E File Offset: 0x00038D8E
	public int DesiredAmbientNpc
	{
		get
		{
			return this._desiredAmbientNpc;
		}
	}

	// Token: 0x170001D4 RID: 468
	// (get) Token: 0x0600082C RID: 2092 RVA: 0x0003AB96 File Offset: 0x00038D96
	public int CurrentNearCount
	{
		get
		{
			if (!(this.PlayerTransform != null))
			{
				return 0;
			}
			return this.CountNpcsInNearRadius(this.PlayerPos);
		}
	}

	// Token: 0x170001D5 RID: 469
	// (get) Token: 0x0600082D RID: 2093 RVA: 0x0003ABB4 File Offset: 0x00038DB4
	public int CurrentAmbientCount
	{
		get
		{
			if (!(this.PlayerTransform != null))
			{
				return 0;
			}
			return this.CountNpcsInAmbientRadius(this.PlayerPos);
		}
	}

	// Token: 0x170001D6 RID: 470
	// (get) Token: 0x0600082E RID: 2094 RVA: 0x0003ABD4 File Offset: 0x00038DD4
	public int EffectiveNearTarget
	{
		get
		{
			int num;
			int num2;
			this.GetEffectivePopulationTargets(out num, out num2);
			return num;
		}
	}

	// Token: 0x170001D7 RID: 471
	// (get) Token: 0x0600082F RID: 2095 RVA: 0x0003ABEC File Offset: 0x00038DEC
	public int EffectiveAmbientTarget
	{
		get
		{
			int num;
			int num2;
			this.GetEffectivePopulationTargets(out num, out num2);
			return num2;
		}
	}

	// Token: 0x170001D8 RID: 472
	// (get) Token: 0x06000830 RID: 2096 RVA: 0x0003AC04 File Offset: 0x00038E04
	public bool IsInitialPopulationComplete
	{
		get
		{
			return this._initialPopulationComplete;
		}
	}

	// Token: 0x170001D9 RID: 473
	// (get) Token: 0x06000831 RID: 2097 RVA: 0x0003AC0C File Offset: 0x00038E0C
	public bool IsSpawnPoolReady
	{
		get
		{
			return !this.useRuntimeSpawner || (this.runtimeSpawner != null && !this.runtimeSpawner.IsTearingDown && this.runtimeSpawner.IsPrePoolComplete && this.runtimeSpawner.PrePoolFailure == null);
		}
	}

	// Token: 0x170001DA RID: 474
	// (get) Token: 0x06000832 RID: 2098 RVA: 0x0003AC5B File Offset: 0x00038E5B
	public string SpawnPoolFailure
	{
		get
		{
			if (!this.useRuntimeSpawner || !(this.runtimeSpawner != null))
			{
				return null;
			}
			return this.runtimeSpawner.PrePoolFailure;
		}
	}

	// Token: 0x170001DB RID: 475
	// (get) Token: 0x06000833 RID: 2099 RVA: 0x0003AC80 File Offset: 0x00038E80
	public bool HasPreparationFocus
	{
		get
		{
			return this._preparationFocusOwned && this._preparationOwner != null && NPCSimulationPause.IsPaused;
		}
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x0003ACA0 File Offset: 0x00038EA0
	public bool TrySetPreparationFocus(Object owner, Vector3 position)
	{
		if (owner == null || !NPCSimulationPause.IsPaused || this.IsTearingDown || float.IsNaN(position.x) || float.IsNaN(position.y) || float.IsNaN(position.z) || float.IsInfinity(position.x) || float.IsInfinity(position.y) || float.IsInfinity(position.z))
		{
			return false;
		}
		if (this._preparationFocusOwned && this._preparationOwner != null && this._preparationOwner != owner)
		{
			return false;
		}
		if (this.HasPreparationFocus && this._preparationOwner == owner && (this._preparationPosition - position).sqrMagnitude < 0.0001f)
		{
			return true;
		}
		this._currentZoneJobHandle.Complete();
		this._zoneJobScheduled = false;
		this._scheduledNpcCount = 0;
		this._pendingSpawns.Clear();
		this._preparationOwner = owner;
		this._preparationFocusOwned = true;
		this._preparationPosition = position;
		this._lastPlayerPos = position;
		this._playerMoveDir = Vector3.zero;
		this._playerSpeed = 0f;
		this.InvalidatePreparationPopulation();
		this._lastZoneCheck = Time.time - Mathf.Max(0.001f, this.zoneCheckInterval);
		this._lastPositionSync = Time.time - Mathf.Max(0.001f, this.positionSyncInterval);
		return true;
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x0003AE04 File Offset: 0x00039004
	public bool IsPreparationPopulationReady(Object owner)
	{
		return owner != null && this.HasPreparationFocus && this._preparationOwner == owner && this._preparationRebalanced && this._preparationPopulationSatisfied && this._pendingSpawns.Count == 0 && this._preparationZoneChecked && this._initialGenerationGateOpen && (AStarGraphManager.Instance == null || AStarGraphManager.Instance.IsReady) && (!this.useRuntimeSpawner || this.runtimeSpawner == null || !this.runtimeSpawner.IsPrePooling);
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x0003AEA1 File Offset: 0x000390A1
	public void ReleasePreparationFocus(Object owner)
	{
		if (!this._preparationFocusOwned || owner == null || this._preparationOwner != owner)
		{
			return;
		}
		this.ClearPreparationFocus();
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x0003AECC File Offset: 0x000390CC
	private void ClearPreparationFocus()
	{
		if (!this._preparationFocusOwned)
		{
			return;
		}
		this._currentZoneJobHandle.Complete();
		this._zoneJobScheduled = false;
		this._scheduledNpcCount = 0;
		this._pendingSpawns.Clear();
		this._preparationOwner = null;
		this._preparationFocusOwned = false;
		this._preparationRebalanced = false;
		this._preparationPopulationSatisfied = false;
		this._preparationZoneChecked = false;
		this._lastPlayerPos = this.PlayerPos;
		this._playerMoveDir = Vector3.zero;
		this._playerSpeed = 0f;
		this._lastRebalance = Time.time - Mathf.Max(0.001f, this._rebalanceInterval);
		this._lastZoneCheck = Time.time - Mathf.Max(0.001f, this.zoneCheckInterval);
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x0003AF84 File Offset: 0x00039184
	private void InvalidatePreparationPopulation()
	{
		if (!this._preparationFocusOwned)
		{
			return;
		}
		this._preparationRevision++;
		this._preparationRebalanced = false;
		this._preparationPopulationSatisfied = false;
		this._preparationZoneChecked = false;
		this._lastRebalance = Time.time - Mathf.Max(0.001f, this._rebalanceInterval);
		this._lastZoneCheck = Time.time - Mathf.Max(0.001f, this.zoneCheckInterval);
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x0003AFF5 File Offset: 0x000391F5
	private void MarkPreparationZoneCheckComplete(int revision)
	{
		if (this.HasPreparationFocus && revision == this._preparationRevision)
		{
			this._preparationZoneChecked = true;
		}
	}

	// Token: 0x170001DC RID: 476
	// (get) Token: 0x0600083A RID: 2106 RVA: 0x0003B010 File Offset: 0x00039210
	public float InitialPopulationProgress
	{
		get
		{
			if (this._initialPopulationComplete)
			{
				return 1f;
			}
			if (!this._initialGenerationGateOpen)
			{
				return 0f;
			}
			if (AStarGraphManager.Instance != null && !AStarGraphManager.Instance.IsReady)
			{
				return 0.05f;
			}
			int effectiveNearTarget = this.EffectiveNearTarget;
			if (effectiveNearTarget <= 0 || this._initialPopulationSatisfied)
			{
				return 1f;
			}
			return Mathf.Clamp01((float)(this.CountNpcsInNearRadius(this.PlayerPos) + this.CountPendingInNearRadius(this.PlayerPos)) / (float)effectiveNearTarget);
		}
	}

	// Token: 0x170001DD RID: 477
	// (get) Token: 0x0600083B RID: 2107 RVA: 0x0003B093 File Offset: 0x00039293
	// (set) Token: 0x0600083C RID: 2108 RVA: 0x0003B09B File Offset: 0x0003929B
	public bool IsTearingDown { get; private set; }

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x0600083D RID: 2109 RVA: 0x0003B0A4 File Offset: 0x000392A4
	public float DensityMultiplier
	{
		get
		{
			return this._densityMultiplier;
		}
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x0003B0AC File Offset: 0x000392AC
	public void SetDensityMultiplier(float multiplier)
	{
		this._densityMultiplier = Mathf.Clamp01(multiplier);
	}

	// Token: 0x170001DF RID: 479
	// (get) Token: 0x0600083F RID: 2111 RVA: 0x0003B0BA File Offset: 0x000392BA
	public int DesiredActiveNpc
	{
		get
		{
			return this._desiredActiveNpc;
		}
	}

	// Token: 0x06000840 RID: 2112 RVA: 0x0003B0C2 File Offset: 0x000392C2
	public void SetPopulationTargets(int desiredNearPlayer, int maxTotal)
	{
		this._desiredActiveNpc = Mathf.Max(0, desiredNearPlayer);
		this._maxTotalNpc = Mathf.Clamp(maxTotal, 0, this.maxTotalNpc);
	}

	// Token: 0x06000841 RID: 2113 RVA: 0x0003B0E4 File Offset: 0x000392E4
	private void GetEffectivePopulationTargets(out int near, out int ambient)
	{
		float num = Mathf.Clamp01(this._densityMultiplier * this._timeOfDayMultiplier);
		near = Mathf.Clamp(Mathf.RoundToInt((float)this._desiredActiveNpc * num), 0, this._maxTotalNpc);
		int num2 = (this._allowAmbient ? Mathf.RoundToInt((float)this._desiredAmbientNpc * num) : 0);
		ambient = Mathf.Clamp(num2, 0, this._maxTotalNpc - near);
	}

	// Token: 0x06000842 RID: 2114 RVA: 0x0003B14C File Offset: 0x0003934C
	private void Awake()
	{
		if (NPCZoneManager.Instance != null && NPCZoneManager.Instance != this)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		NPCZoneManager.Instance = this;
		WorldOrigin.Shifted += this.OnWorldOriginShifted;
		NPCSimulationPause.PauseChanged += this.OnSimulationPauseChanged;
		if (this.spawnConfig == null)
		{
			this.spawnConfig = Resources.Load<NPCSpawnConfig>("NPCSpawnConfig");
		}
		if (this.spawnConfig != null)
		{
			float nearRadius = this.spawnConfig.nearRadius;
			float activeRadius = this.spawnConfig.activeRadius;
			float num = Mathf.Clamp(this.spawnConfig.ambientRadius, nearRadius, Mathf.Max(nearRadius, activeRadius));
			float spawnRadius = this.spawnConfig.spawnRadius;
			float despawnRadius = this.spawnConfig.despawnRadius;
			this._nearRadiusSqr = nearRadius * nearRadius;
			this._ambientRadiusSqr = num * num;
			this._activeRadiusSqr = activeRadius * activeRadius;
			this._despawnRadiusSqr = despawnRadius * despawnRadius;
			this._warmupRadiusSqr = spawnRadius * spawnRadius;
			this._preferredDistance = this.spawnConfig.preferredDistance;
			this._ambientPreferredDistance = (nearRadius + num) * 0.5f;
			this._minDistanceFromPlayerSqr = this.spawnConfig.minDistanceFromPlayer * this.spawnConfig.minDistanceFromPlayer;
			this._distanceWeight = this.spawnConfig.distanceWeight;
			this._randomWeight = this.spawnConfig.randomWeight;
			this._recentSpawnCooldown = this.spawnConfig.recentSpawnCooldown;
			this._minNpcSpawnSpacingSqr = this.spawnConfig.minNpcSpawnSpacing * this.spawnConfig.minNpcSpawnSpacing;
			this._maxSpawnNavmeshHorizontalCorrection = this.spawnConfig.maxSpawnNavmeshHorizontalCorrection;
			this._maxSpawnNavmeshVerticalCorrection = this.spawnConfig.maxSpawnNavmeshVerticalCorrection;
			this._desiredActiveNpc = this.spawnConfig.desiredActiveNpc;
			this._desiredAmbientNpc = this.spawnConfig.desiredAmbientNpc;
			this._maxTotalNpc = this.spawnConfig.maxTotalNpc;
			this._allowAmbient = this.spawnConfig.allowAmbientNpcs;
			this._spawnsPerFrame = this.spawnConfig.spawnsPerFrame;
			this._despawnsPerFrame = this.spawnConfig.despawnsPerFrame;
			this._maxSpawnsPerRebalance = Mathf.Max(1, this.spawnConfig.maxSpawnsPerRebalance);
			this._retirementsPerRebalance = Mathf.Max(1, this.spawnConfig.retirementsPerRebalance);
			this._rebalanceInterval = this.spawnConfig.rebalanceInterval;
			this._populationTimeUpdateInterval = Mathf.Max(0.1f, this.spawnConfig.populationTimeUpdateInterval);
			this._verboseDebug = this.spawnConfig.enableDebugLogs;
			NPCDebug.Verbose = this.spawnConfig.enableNpcVerboseLogs;
			this.maxTotalNpc = this._maxTotalNpc;
		}
		else
		{
			this._activeRadiusSqr = this.activeZoneRadius * this.activeZoneRadius;
			this._warmupRadiusSqr = this.warmupZoneRadius * this.warmupZoneRadius;
			this._nearRadiusSqr = 900f;
			this._ambientRadiusSqr = 3025f;
			this._despawnRadiusSqr = this.warmupZoneRadius * this.warmupZoneRadius * 1.3f * 1.3f;
			this._preferredDistance = 25f;
			this._ambientPreferredDistance = 42.5f;
			this._minDistanceFromPlayerSqr = 64f;
			this._distanceWeight = 0.7f;
			this._randomWeight = 0.3f;
			this._recentSpawnCooldown = 4f;
			this._minNpcSpawnSpacingSqr = 2.25f;
			this._maxSpawnNavmeshHorizontalCorrection = 1.5f;
			this._maxSpawnNavmeshVerticalCorrection = 2f;
			this._desiredActiveNpc = 15;
			this._desiredAmbientNpc = 10;
			this._maxTotalNpc = this.maxTotalNpc;
			this._allowAmbient = true;
			this._spawnsPerFrame = this.spawnsPerFrame;
			this._despawnsPerFrame = 8;
			this._maxSpawnsPerRebalance = 3;
			this._retirementsPerRebalance = 1;
			this._rebalanceInterval = 0.4f;
			this._populationTimeUpdateInterval = 2f;
			this._verboseDebug = this.enableDebugLogs;
		}
		this.AllocateNativeArrays();
	}

	// Token: 0x06000843 RID: 2115 RVA: 0x0003B51C File Offset: 0x0003971C
	private void Start()
	{
		this.RefreshTimeOfDayPopulation(true);
		this.ConfigureInitialSpawnSafety();
		this.InitializeReferences();
		this.InitializeChunkTracking();
		this.CacheSceneInterestPoints();
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Format("[NPCZoneManager] Started - Jobs: {0}, MaxNPC: {1}, TotalPOI: {2}", this.useJobs, this.maxTotalNpc, this.GetTotalPOICount()));
		}
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x0003B580 File Offset: 0x00039780
	public void ReinitializeForPlayer(Transform persistentPlayer)
	{
		this.ClearPreparationFocus();
		if (persistentPlayer != null)
		{
			this.player = persistentPlayer;
		}
		this.ConfigureInitialSpawnSafety();
		this.UnsubscribeChunkEvents();
		this.InitializeReferences();
		this.InitializeChunkTracking();
		this.CacheSceneInterestPoints();
		this._lastPlayerPos = this.PlayerPos;
		this._initialPopulationSatisfied = false;
		this._initialPopulationComplete = false;
		if (this.enableDebugLogs)
		{
			Debug.Log("[NPCZoneManager] ReinitializeForPlayer: player='" + ((this.player != null) ? this.player.name : "NULL") + "' " + string.Format("(live pos={0}).", this.PlayerPos));
		}
	}

	// Token: 0x06000845 RID: 2117 RVA: 0x0003B62C File Offset: 0x0003982C
	public void PrepareForSceneTransition()
	{
		this.ClearPreparationFocus();
		this._currentZoneJobHandle.Complete();
		this._zoneJobScheduled = false;
		this._scheduledNpcCount = 0;
		this.UnsubscribeChunkEvents();
		for (int i = 0; i < this._allNpcs.Count; i++)
		{
			NPCZoneManager.NPCState npcstate = this._allNpcs[i];
			if (npcstate != null && !(npcstate.npc == null))
			{
				try
				{
					NPCZoneManager.NPCEvent onNPCDespawned = this.OnNPCDespawned;
					if (onNPCDespawned != null)
					{
						onNPCDespawned(npcstate.npc, npcstate.chunk);
					}
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
				try
				{
					if (this.useRuntimeSpawner && this.runtimeSpawner != null)
					{
						this.runtimeSpawner.DespawnNPCImmediate(npcstate.npc, false);
					}
					else
					{
						Object.Destroy(npcstate.npc);
					}
				}
				catch (Exception ex2)
				{
					Debug.LogException(ex2);
					if (npcstate.npc != null)
					{
						Object.Destroy(npcstate.npc);
					}
				}
			}
		}
		this._allNpcs.Clear();
		this._chunkNpcs.Clear();
		this._chunksWithNpcs.Clear();
		this._chunkInterestPoints.Clear();
		this._chunkSpawnPoints.Clear();
		this._spawnPointToChunk.Clear();
		this._staticChunks.Clear();
		this._chunkIdMap.Clear();
		this._scenePOIs.Clear();
		this._spawnQueue.Clear();
		this._chunksQueued.Clear();
		this._candidateBuffer.Clear();
		this._pendingSpawns.Clear();
		this._recentSpawns.Clear();
		this._recentSpawnsKeys.Clear();
		this.staticChunksContainer = null;
		this._currentNpcCount = 0;
		this._activeNpcCount = 0;
		this._initialGenerationGateOpen = false;
		this._initialPopulationSatisfied = false;
		this._initialPopulationComplete = false;
		this.RefreshTimeOfDayPopulation(true);
		this._playerMoveDir = Vector3.zero;
		this._playerSpeed = 0f;
		this._lastZoneCheck = 0f;
		this._lastPositionSync = 0f;
		this._lastRebalance = 0f;
		if (this.enableDebugLogs)
		{
			Debug.Log("[NPCZoneManager] Stato scena sorgente rilasciato per la transizione.");
		}
	}

	// Token: 0x06000846 RID: 2118 RVA: 0x0003B858 File Offset: 0x00039A58
	private void UnsubscribeChunkEvents()
	{
		if (ChunkManager.Instance != null)
		{
			ChunkManager.Instance.OnChunkSpawned -= this.OnChunkSpawned;
			ChunkManager.Instance.OnChunkRemoved -= this.OnChunkRemoved;
			ChunkManager.Instance.OnChunkZonesReady -= this.OnChunkZonesReady;
			ChunkManager.Instance.OnInitialGenerationComplete -= this.OnInitialGenerationCompleteHandler;
		}
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x0003B8CC File Offset: 0x00039ACC
	private void CacheSceneInterestPoints()
	{
		NPCInterestPoint[] array = Object.FindObjectsOfType<NPCInterestPoint>();
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Format("[NPCZoneManager] CacheSceneInterestPoints: FindObjectsOfType found {0} NPCInterestPoint in scene", array.Length));
		}
		if (array.Length == 0)
		{
			Debug.LogWarning("[NPCZoneManager] No NPCInterestPoint found in scene!");
			return;
		}
		foreach (NPCInterestPoint npcinterestPoint in array)
		{
			if (npcinterestPoint != null && !this._scenePOIs.Contains(npcinterestPoint))
			{
				this._scenePOIs.Add(npcinterestPoint);
			}
		}
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Format("[NPCZoneManager] Cached {0} scene POI", this._scenePOIs.Count));
		}
	}

	// Token: 0x06000848 RID: 2120 RVA: 0x0003B96C File Offset: 0x00039B6C
	private int GetTotalPOICount()
	{
		int num = this._scenePOIs.Count;
		foreach (KeyValuePair<ChunkInstance, List<NPCInterestPoint>> keyValuePair in this._chunkInterestPoints)
		{
			num += keyValuePair.Value.Count;
		}
		return num;
	}

	// Token: 0x06000849 RID: 2121 RVA: 0x0003B9D4 File Offset: 0x00039BD4
	private void OnDestroy()
	{
		WorldOrigin.Shifted -= this.OnWorldOriginShifted;
		NPCSimulationPause.PauseChanged -= this.OnSimulationPauseChanged;
		this.ClearPreparationFocus();
		this._currentZoneJobHandle.Complete();
		this.DisposeNativeArrays();
		if (NPCZoneManager.Instance == this)
		{
			NPCZoneManager.Instance = null;
		}
		if (ChunkManager.Instance != null)
		{
			ChunkManager.Instance.OnChunkSpawned -= this.OnChunkSpawned;
			ChunkManager.Instance.OnChunkRemoved -= this.OnChunkRemoved;
			ChunkManager.Instance.OnChunkZonesReady -= this.OnChunkZonesReady;
			ChunkManager.Instance.OnInitialGenerationComplete -= this.OnInitialGenerationCompleteHandler;
		}
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x0003BA94 File Offset: 0x00039C94
	private void Update()
	{
		this.RefreshTimeOfDayPopulation(false);
		if (this._preparationFocusOwned && !this.HasPreparationFocus)
		{
			this.ClearPreparationFocus();
		}
		if (NPCSimulationPause.IsPaused && !this.HasPreparationFocus)
		{
			return;
		}
		if (this.PlayerTransform == null && !this.HasPreparationFocus)
		{
			return;
		}
		float time = Time.time;
		if (this._zoneJobScheduled)
		{
			this.CompleteZoneJob();
		}
		if (time - this._lastPositionSync >= this.positionSyncInterval)
		{
			this._lastPositionSync = time;
			this.SyncNPCPositions();
		}
		if (time - this._lastZoneCheck >= this.zoneCheckInterval)
		{
			this._lastZoneCheck = time;
			if (this.useJobs)
			{
				this.ScheduleZoneCheckJobs();
			}
			else
			{
				this.CheckZonesFallback();
			}
		}
		if (time - this._lastRebalance >= this._rebalanceInterval)
		{
			this._lastRebalance = time;
			this.DespawnFarNPCs(time);
			this.RebalanceSpawnQueue(time);
		}
		this.ProcessPendingSpawns();
		if (!this._initialPopulationComplete && this._initialPopulationSatisfied && this.CountPendingInNearRadius(this.PlayerPos) == 0)
		{
			this._initialPopulationComplete = true;
			if (this._initialPoiSafeRadiusSqr > 0f)
			{
				this._initialPoiSafetyEndsAtRealtime = Time.realtimeSinceStartup + this._initialPoiSafetyDuration;
			}
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Format("[NPCZoneManager] Primo popolamento NPC completo: {0} NPC presenti.", this._currentNpcCount));
			}
		}
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x0003BBD4 File Offset: 0x00039DD4
	private void OnWorldOriginShifted(Vector3 delta)
	{
		this._currentZoneJobHandle.Complete();
		this._zoneJobScheduled = false;
		this._scheduledNpcCount = 0;
		this._lastPlayerPos += delta;
		if (this._preparationFocusOwned)
		{
			this._preparationPosition += delta;
		}
		if (this._hasPoiEntryExclusion)
		{
			this._poiEntryPosition += delta;
		}
		foreach (ChunkInstance chunkInstance in this._staticChunks)
		{
			if (((chunkInstance != null) ? chunkInstance.chunkRoot : null) != null)
			{
				chunkInstance.worldPosition = chunkInstance.chunkRoot.transform.position;
			}
		}
		if (this._npcPositions.IsCreated && this._npcJobData.IsCreated)
		{
			this.SyncNPCPositions();
		}
		this._candidateBuffer.Clear();
		this._lastPositionSync = Time.time;
		this._lastZoneCheck = Time.time - Mathf.Max(0.001f, this.zoneCheckInterval);
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x0003BCFC File Offset: 0x00039EFC
	private void RefreshTimeOfDayPopulation(bool force = false)
	{
		if (this.spawnConfig == null || !this.spawnConfig.useTimeOfDayPopulation)
		{
			this._currentPopulationHour = -1f;
			this._timeOfDayMultiplier = 1f;
			return;
		}
		float unscaledTime = Time.unscaledTime;
		if (!force && unscaledTime < this._nextPopulationTimeUpdate)
		{
			return;
		}
		this._nextPopulationTimeUpdate = unscaledTime + this._populationTimeUpdateInterval;
		if (this._azureSystem == null)
		{
			this._azureSystem = Object.FindObjectOfType<AzureCoreSystem>();
		}
		float num = ((this._azureSystem != null && this._azureSystem.timeSystem != null) ? this._azureSystem.timeSystem.timeOfDay : 12f);
		float num2 = this.spawnConfig.EvaluateTimeOfDayMultiplier(num);
		bool flag = !Mathf.Approximately(num2, this._timeOfDayMultiplier);
		this._currentPopulationHour = Mathf.Repeat(num, 24f);
		this._timeOfDayMultiplier = num2;
		if (flag && this._verboseDebug)
		{
			Debug.Log(string.Format("[NPCZoneManager] Population hour={0:0.00}, multiplier={1:0.00}.", this._currentPopulationHour, this._timeOfDayMultiplier));
		}
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x0003BE0C File Offset: 0x0003A00C
	private void AllocateNativeArrays()
	{
		this._npcJobData = new NativeArray<NPCJobData>(this.maxTotalNpc, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this._npcZoneResults = new NativeArray<NPCZoneResult>(this.maxTotalNpc, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this._npcPositions = new NativeArray<float3>(this.maxTotalNpc, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		int num = 100;
		this._chunkJobData = new NativeArray<global::ChunkJobData>(num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		this._chunkZoneResults = new NativeArray<ChunkZoneResult>(num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x0003BE74 File Offset: 0x0003A074
	private void DisposeNativeArrays()
	{
		if (this._npcJobData.IsCreated)
		{
			this._npcJobData.Dispose();
		}
		if (this._npcZoneResults.IsCreated)
		{
			this._npcZoneResults.Dispose();
		}
		if (this._npcPositions.IsCreated)
		{
			this._npcPositions.Dispose();
		}
		if (this._chunkJobData.IsCreated)
		{
			this._chunkJobData.Dispose();
		}
		if (this._chunkZoneResults.IsCreated)
		{
			this._chunkZoneResults.Dispose();
		}
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x0003BEFC File Offset: 0x0003A0FC
	private void SyncNPCPositions()
	{
		for (int i = 0; i < this._allNpcs.Count; i++)
		{
			NPCZoneManager.NPCState npcstate = this._allNpcs[i];
			if (npcstate.npc != null)
			{
				Vector3 position = npcstate.npc.transform.position;
				this._npcPositions[i] = new float3(position.x, position.y, position.z);
				NPCJobData npcjobData = this._npcJobData[i];
				npcjobData.id = npcstate.id;
				npcjobData.position = this._npcPositions[i];
				npcjobData.isActive = npcstate.isActive;
				this._npcJobData[i] = npcjobData;
			}
		}
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x0003BFBC File Offset: 0x0003A1BC
	private void ScheduleZoneCheckJobs()
	{
		Vector3 playerPos = this.PlayerPos;
		this._scheduledPreparationRevision = this._preparationRevision;
		if (this._allNpcs.Count == 0)
		{
			this._scheduledNpcCount = 0;
			this.MarkPreparationZoneCheckComplete(this._scheduledPreparationRevision);
			return;
		}
		float3 @float = new float3(playerPos.x, playerPos.y, playerPos.z);
		NPCZoneCheckJob npczoneCheckJob = new NPCZoneCheckJob
		{
			npcs = this._npcJobData,
			playerPosition = @float,
			activeRadiusSqr = this._activeRadiusSqr,
			results = this._npcZoneResults
		};
		this._scheduledNpcCount = this._allNpcs.Count;
		this._currentZoneJobHandle = npczoneCheckJob.Schedule(this._scheduledNpcCount, 64, default(JobHandle));
		this._zoneJobScheduled = true;
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x0003C085 File Offset: 0x0003A285
	private void CompleteZoneJob()
	{
		if (!this._zoneJobScheduled)
		{
			return;
		}
		this._currentZoneJobHandle.Complete();
		this._zoneJobScheduled = false;
		this.ApplyZoneResults();
		this.MarkPreparationZoneCheckComplete(this._scheduledPreparationRevision);
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x0003C0B4 File Offset: 0x0003A2B4
	private void ApplyZoneResults()
	{
		int num = Mathf.Min(this._scheduledNpcCount, this._allNpcs.Count);
		int num2 = 0;
		while (num2 < num && num2 < this._allNpcs.Count)
		{
			NPCZoneManager.NPCState npcstate = this._allNpcs[num2];
			if (npcstate.npc == null)
			{
				if (npcstate.isActive)
				{
					this._activeNpcCount--;
				}
				this._allNpcs.RemoveAt(num2);
				this._currentNpcCount--;
				this.InvalidatePreparationPopulation();
				num2--;
			}
			else
			{
				NPCZoneResult npczoneResult = this._npcZoneResults[num2];
				if (npczoneResult.id == npcstate.id && npczoneResult.shouldBeActive != npcstate.isActive)
				{
					this.SetNPCActive(npcstate, npczoneResult.shouldBeActive, false);
				}
			}
			num2++;
		}
		this._scheduledNpcCount = 0;
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x0003C190 File Offset: 0x0003A390
	private void CheckZonesFallback()
	{
		Vector3 playerPos = this.PlayerPos;
		int preparationRevision = this._preparationRevision;
		for (int i = 0; i < this._allNpcs.Count; i++)
		{
			NPCZoneManager.NPCState npcstate = this._allNpcs[i];
			if (npcstate.npc == null)
			{
				if (npcstate.isActive)
				{
					this._activeNpcCount--;
				}
				this._allNpcs.RemoveAt(i);
				this._currentNpcCount--;
				this.InvalidatePreparationPopulation();
				i--;
			}
			else
			{
				bool flag = (playerPos - npcstate.npc.transform.position).sqrMagnitude <= this._activeRadiusSqr;
				if (flag != npcstate.isActive)
				{
					this.SetNPCActive(npcstate, flag, false);
				}
			}
		}
		this.MarkPreparationZoneCheckComplete(preparationRevision);
	}

	// Token: 0x06000854 RID: 2132 RVA: 0x0003C268 File Offset: 0x0003A468
	private void DespawnFarNPCs(float now)
	{
		if (this._allNpcs.Count == 0)
		{
			return;
		}
		Vector3 playerPos = this.PlayerPos;
		int num = this._despawnsPerFrame;
		int num2 = 0;
		while (num2 < this._allNpcs.Count && num > 0)
		{
			NPCZoneManager.NPCState npcstate = this._allNpcs[num2];
			if (npcstate != null && !(npcstate.npc == null))
			{
				Vector3 vector = playerPos - npcstate.npc.transform.position;
				float num3 = vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
				if (((npcstate.maxVerticalDistanceFromPlayer > 0f && Mathf.Abs(vector.y) > npcstate.maxVerticalDistanceFromPlayer + 1.5f) || num3 > this._despawnRadiusSqr) && this.DespawnSingleNPC(npcstate))
				{
					num--;
					num2--;
				}
			}
			num2++;
		}
	}

	// Token: 0x06000855 RID: 2133 RVA: 0x0003C368 File Offset: 0x0003A568
	private void RebalanceSpawnQueue(float now)
	{
		if (!this._initialGenerationGateOpen)
		{
			return;
		}
		if (this.useRuntimeSpawner && this.runtimeSpawner != null && !this.runtimeSpawner.HasCharacterData)
		{
			this._initialPopulationSatisfied = true;
			if (this.HasPreparationFocus)
			{
				this._preparationRebalanced = true;
				this._preparationPopulationSatisfied = this._pendingSpawns.Count == 0;
			}
			return;
		}
		if (AStarGraphManager.Instance != null && !AStarGraphManager.Instance.IsReady)
		{
			return;
		}
		if (this.useRuntimeSpawner && this.runtimeSpawner != null && this.runtimeSpawner.IsPrePooling)
		{
			return;
		}
		if (this.HasPreparationFocus)
		{
			this._preparationRebalanced = true;
			this._preparationPopulationSatisfied = false;
		}
		Vector3 playerPos = this.PlayerPos;
		Vector3 vector = playerPos - this._lastPlayerPos;
		this._playerSpeed = vector.magnitude / Mathf.Max(this._rebalanceInterval, 0.0001f);
		this._playerMoveDir = ((this._playerSpeed > 0.25f) ? vector.normalized : Vector3.zero);
		this._lastPlayerPos = playerPos;
		int num;
		int num2;
		this.GetEffectivePopulationTargets(out num, out num2);
		int num3 = num + num2;
		if (this._pendingSpawns.Count > 0 && this._currentNpcCount + this._pendingSpawns.Count > num3)
		{
			this._pendingSpawns.Clear();
		}
		int num4 = this.CountNpcsInNearRadius(playerPos);
		int num5 = this.CountNpcsInAmbientRadius(playerPos);
		this.RetireExcessPopulation(playerPos, num, num2, ref num4, ref num5);
		if (this.TrySettleExhaustedPartialPreparation())
		{
			this.EvictExpiredRecentSpawns(now);
			return;
		}
		int num6 = this.CountPendingInNearRadius(playerPos);
		int num7 = this.CountPendingInAmbientRadius(playerPos);
		int num8 = num - num4 - num6;
		int num9 = num2 - num5 - num7;
		if (num8 <= 0)
		{
			this._initialPopulationSatisfied = true;
		}
		int num10 = Mathf.Min(this._maxTotalNpc - this._currentNpcCount - this._pendingSpawns.Count, num3 - this._currentNpcCount - this._pendingSpawns.Count);
		int num11 = Mathf.Min(this._maxSpawnsPerRebalance, Mathf.Max(0, num10));
		if (num11 <= 0 || (num8 <= 0 && num9 <= 0))
		{
			if (num8 > 0)
			{
				this._initialPopulationSatisfied = true;
			}
			if (this.HasPreparationFocus)
			{
				this._preparationPopulationSatisfied = num8 <= 0 && num9 <= 0 && this._pendingSpawns.Count == 0;
			}
			this.EvictExpiredRecentSpawns(now);
			return;
		}
		uint num12;
		if (!NPCZoneManager.TryGetWalkableArea(playerPos, out num12))
		{
			this._initialPopulationSatisfied = true;
			return;
		}
		int num13 = 0;
		if (num8 > 0)
		{
			num13 = this.QueuePopulationBand(playerPos, num12, now, this._minDistanceFromPlayerSqr, this._nearRadiusSqr, this._preferredDistance, Mathf.Min(num8, num11));
			num11 -= num13;
			if (num13 == 0)
			{
				this._initialPopulationSatisfied = true;
			}
		}
		int num14 = 0;
		bool flag = false;
		if (this._allowAmbient && num9 > 0 && num11 > 0)
		{
			flag = true;
			num14 = this.QueuePopulationBand(playerPos, num12, now, this._nearRadiusSqr + 0.01f, this._ambientRadiusSqr, this._ambientPreferredDistance, Mathf.Min(num9, num11));
		}
		if (this.HasPreparationFocus)
		{
			bool flag2 = num8 <= 0 || num13 == 0;
			bool flag3 = num9 <= 0 || !this._allowAmbient || (flag && num14 == 0);
			this._preparationPopulationSatisfied = flag2 && flag3 && this._pendingSpawns.Count == 0;
		}
		this.EvictExpiredRecentSpawns(now);
		if (this._verboseDebug && num13 + num14 > 0)
		{
			Debug.Log(string.Format("[NPCZoneManager] Population near={0}/{1}, ambient={2}/{3}, queued={4}.", new object[]
			{
				num4,
				num,
				num5,
				num2,
				num13 + num14
			}));
		}
	}

	// Token: 0x06000856 RID: 2134 RVA: 0x0003C6FC File Offset: 0x0003A8FC
	private bool TrySettleExhaustedPartialPreparation()
	{
		if (!this.useRuntimeSpawner || this.runtimeSpawner == null || !this.runtimeSpawner.IsPartialPreparationExhausted)
		{
			return false;
		}
		foreach (NPCSpawnPoint npcspawnPoint in this._pendingSpawns)
		{
			if (npcspawnPoint != null)
			{
				this._recentSpawns.Remove(npcspawnPoint);
			}
		}
		this._pendingSpawns.Clear();
		this._initialPopulationSatisfied = true;
		if (this.HasPreparationFocus)
		{
			this._preparationPopulationSatisfied = true;
		}
		return true;
	}

	// Token: 0x06000857 RID: 2135 RVA: 0x0003C7A8 File Offset: 0x0003A9A8
	private int QueuePopulationBand(Vector3 playerPos, uint playerArea, float now, float minSqr, float maxSqr, float preferredDistance, int requested)
	{
		if (requested <= 0 || maxSqr <= minSqr)
		{
			return 0;
		}
		this._candidateBuffer.Clear();
		this.CollectPopulationCandidatesFromChunks(ChunkManager.Instance != null && ChunkManager.Instance.enabled, playerPos, playerArea, minSqr, maxSqr, preferredDistance);
		this.CollectPopulationCandidatesFromStaticChunks(playerPos, playerArea, minSqr, maxSqr, preferredDistance);
		if (this._candidateBuffer.Count == 0)
		{
			return 0;
		}
		this._candidateBuffer.Sort(NPCZoneManager.CandidateScoreCompareDesc);
		int num = 0;
		int num2 = 0;
		while (num2 < this._candidateBuffer.Count && num < requested)
		{
			NPCZoneManager.CandidateSpawnPoint candidateSpawnPoint = this._candidateBuffer[num2];
			Vector3 vector;
			if (!(candidateSpawnPoint.sp == null) && this.TryResolveSpawnPosition(candidateSpawnPoint.sp, playerArea, out vector) && this.IsSpawnPositionClear(vector, true))
			{
				this._pendingSpawns.Enqueue(candidateSpawnPoint.sp);
				this._recentSpawns[candidateSpawnPoint.sp] = now;
				num++;
			}
			num2++;
		}
		return num;
	}

	// Token: 0x06000858 RID: 2136 RVA: 0x0003C8A0 File Offset: 0x0003AAA0
	private void CollectPopulationCandidatesFromChunks(bool useProceduralChunks, Vector3 playerPos, uint playerArea, float minSqr, float maxSqr, float preferredDistance)
	{
		if (!useProceduralChunks)
		{
			return;
		}
		foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in ChunkManager.Instance.SpawnedChunks)
		{
			ChunkInstance value = keyValuePair.Value;
			if (value != null && !(value.chunkRoot == null))
			{
				this.CollectPopulationCandidatesFromChunk(value, playerPos, playerArea, minSqr, maxSqr, preferredDistance);
			}
		}
	}

	// Token: 0x06000859 RID: 2137 RVA: 0x0003C918 File Offset: 0x0003AB18
	private void CollectPopulationCandidatesFromStaticChunks(Vector3 playerPos, uint playerArea, float minSqr, float maxSqr, float preferredDistance)
	{
		for (int i = 0; i < this._staticChunks.Count; i++)
		{
			ChunkInstance chunkInstance = this._staticChunks[i];
			if (chunkInstance != null && !(chunkInstance.chunkRoot == null))
			{
				this.CollectPopulationCandidatesFromChunk(chunkInstance, playerPos, playerArea, minSqr, maxSqr, preferredDistance);
			}
		}
	}

	// Token: 0x0600085A RID: 2138 RVA: 0x0003C968 File Offset: 0x0003AB68
	private void CollectPopulationCandidatesFromChunk(ChunkInstance chunk, Vector3 playerPos, uint playerArea, float minSqr, float maxSqr, float preferredDistance)
	{
		if (chunk == null || chunk.chunkRoot == null || !chunk.chunkRoot.activeInHierarchy)
		{
			return;
		}
		List<NPCSpawnPoint> list;
		if (!this._chunkSpawnPoints.TryGetValue(chunk, out list))
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			this.ConsiderPopulationCandidate(list[i], playerPos, playerArea, minSqr, maxSqr, preferredDistance);
		}
	}

	// Token: 0x0600085B RID: 2139 RVA: 0x0003C9CC File Offset: 0x0003ABCC
	private void ConsiderPopulationCandidate(NPCSpawnPoint spawnPoint, Vector3 playerPos, uint playerArea, float minSqr, float maxSqr, float preferredDistance)
	{
		if (spawnPoint == null || !spawnPoint.isActiveAndEnabled)
		{
			return;
		}
		Vector3 vector = spawnPoint.transform.position - playerPos;
		if (!NPCZoneManager.IsWithinVerticalBand(spawnPoint, vector.y))
		{
			return;
		}
		float sqrMagnitude = vector.sqrMagnitude;
		if (sqrMagnitude < minSqr || sqrMagnitude > maxSqr)
		{
			return;
		}
		float num = vector.x * vector.x + vector.z * vector.z;
		if (this.IsInitialPoiSafetyActive && num < this._initialPoiSafeRadiusSqr)
		{
			return;
		}
		if (this.IsInsidePoiEntryExclusion(spawnPoint.transform.position))
		{
			return;
		}
		Vector3 vector2;
		if (!this.TryResolveSpawnPosition(spawnPoint, playerArea, out vector2))
		{
			return;
		}
		if (!this.IsSpawnOccludedWhenRequired(spawnPoint, vector2))
		{
			return;
		}
		if (!this.IsSpawnPositionClear(vector2, true))
		{
			return;
		}
		float num2 = Mathf.Sqrt(sqrMagnitude);
		float num3 = (1f - Mathf.Clamp01(Mathf.Abs(num2 - preferredDistance) / Mathf.Sqrt(maxSqr))) * this._distanceWeight;
		num3 += global::UnityEngine.Random.value * this._randomWeight;
		if (this._playerMoveDir.sqrMagnitude > 0.01f && num2 > 0.01f)
		{
			float num4 = Vector3.Dot(this._playerMoveDir, vector / num2);
			num3 *= 0.15f + 0.85f * Mathf.Clamp01((num4 + 1f) * 0.5f);
		}
		float num5;
		if (this._recentSpawns.TryGetValue(spawnPoint, out num5))
		{
			float num6 = Time.time - num5;
			if (num6 < this._recentSpawnCooldown)
			{
				num3 *= num6 / this._recentSpawnCooldown;
			}
		}
		this._candidateBuffer.Add(new NPCZoneManager.CandidateSpawnPoint
		{
			sp = spawnPoint,
			sqrDist = sqrMagnitude,
			score = num3
		});
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x0003CB78 File Offset: 0x0003AD78
	private int CountNpcsInNearRadius(Vector3 playerPos)
	{
		int num = 0;
		for (int i = 0; i < this._allNpcs.Count; i++)
		{
			NPCZoneManager.NPCState npcstate = this._allNpcs[i];
			if (npcstate != null && !(npcstate.npc == null))
			{
				Vector3 vector = playerPos - npcstate.npc.transform.position;
				if ((npcstate.maxVerticalDistanceFromPlayer <= 0f || Mathf.Abs(vector.y) <= npcstate.maxVerticalDistanceFromPlayer) && vector.x * vector.x + vector.y * vector.y + vector.z * vector.z <= this._nearRadiusSqr)
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x0003CC30 File Offset: 0x0003AE30
	private int CountNpcsInAmbientRadius(Vector3 playerPos)
	{
		int num = 0;
		for (int i = 0; i < this._allNpcs.Count; i++)
		{
			NPCZoneManager.NPCState npcstate = this._allNpcs[i];
			if (npcstate != null && !(npcstate.npc == null))
			{
				Vector3 vector = playerPos - npcstate.npc.transform.position;
				if (npcstate.maxVerticalDistanceFromPlayer <= 0f || Mathf.Abs(vector.y) <= npcstate.maxVerticalDistanceFromPlayer)
				{
					float sqrMagnitude = vector.sqrMagnitude;
					if (sqrMagnitude > this._nearRadiusSqr && sqrMagnitude <= this._ambientRadiusSqr)
					{
						num++;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x0003CCD0 File Offset: 0x0003AED0
	private int CountPendingInNearRadius(Vector3 playerPos)
	{
		if (this._pendingSpawns.Count == 0)
		{
			return 0;
		}
		int num = 0;
		foreach (NPCSpawnPoint npcspawnPoint in this._pendingSpawns)
		{
			if (this.IsPendingSpawnAvailable(npcspawnPoint))
			{
				Vector3 vector = playerPos - npcspawnPoint.transform.position;
				if (NPCZoneManager.IsWithinVerticalBand(npcspawnPoint, vector.y) && vector.x * vector.x + vector.y * vector.y + vector.z * vector.z <= this._nearRadiusSqr)
				{
					num++;
				}
			}
		}
		return num;
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x0003CD8C File Offset: 0x0003AF8C
	private int CountPendingInAmbientRadius(Vector3 playerPos)
	{
		int num = 0;
		foreach (NPCSpawnPoint npcspawnPoint in this._pendingSpawns)
		{
			if (this.IsPendingSpawnAvailable(npcspawnPoint))
			{
				Vector3 vector = playerPos - npcspawnPoint.transform.position;
				if (NPCZoneManager.IsWithinVerticalBand(npcspawnPoint, vector.y))
				{
					float sqrMagnitude = vector.sqrMagnitude;
					if (sqrMagnitude > this._nearRadiusSqr && sqrMagnitude <= this._ambientRadiusSqr)
					{
						num++;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x0003CE28 File Offset: 0x0003B028
	private bool IsPendingSpawnAvailable(NPCSpawnPoint spawnPoint)
	{
		if (spawnPoint == null || !spawnPoint.isActiveAndEnabled)
		{
			return false;
		}
		ChunkInstance chunkInstance = this.FindOwningChunk(spawnPoint);
		return chunkInstance != null && chunkInstance.chunkRoot != null && chunkInstance.chunkRoot.activeInHierarchy;
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x0003CE70 File Offset: 0x0003B070
	private void EvictExpiredRecentSpawns(float now)
	{
		this._recentSpawnsKeys.Clear();
		foreach (KeyValuePair<NPCSpawnPoint, float> keyValuePair in this._recentSpawns)
		{
			this._recentSpawnsKeys.Add(keyValuePair.Key);
		}
		for (int i = 0; i < this._recentSpawnsKeys.Count; i++)
		{
			NPCSpawnPoint npcspawnPoint = this._recentSpawnsKeys[i];
			if (npcspawnPoint == null)
			{
				this._recentSpawns.Remove(npcspawnPoint);
			}
			else if (now - this._recentSpawns[npcspawnPoint] > this._recentSpawnCooldown * 2f)
			{
				this._recentSpawns.Remove(npcspawnPoint);
			}
		}
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x0003CF40 File Offset: 0x0003B140
	private void ProcessPendingSpawns()
	{
		if (this._pendingSpawns.Count == 0)
		{
			return;
		}
		if (this.useRuntimeSpawner && this.runtimeSpawner != null && this.runtimeSpawner.IsPrePooling)
		{
			return;
		}
		if (this._currentNpcCount >= this._maxTotalNpc)
		{
			this._pendingSpawns.Clear();
			this._initialPopulationSatisfied = true;
			return;
		}
		if (this._zoneJobScheduled)
		{
			this._currentZoneJobHandle.Complete();
			this._zoneJobScheduled = false;
		}
		Vector3 playerPos = this.PlayerPos;
		uint num;
		if (!NPCZoneManager.TryGetWalkableArea(playerPos, out num))
		{
			this._pendingSpawns.Clear();
			this._initialPopulationSatisfied = true;
			return;
		}
		int num2 = 0;
		while (num2 < this._spawnsPerFrame && this._pendingSpawns.Count > 0 && this._currentNpcCount < this._maxTotalNpc)
		{
			if (this.TrySettleExhaustedPartialPreparation())
			{
				return;
			}
			NPCSpawnPoint npcspawnPoint = this._pendingSpawns.Dequeue();
			if (!(npcspawnPoint == null) && npcspawnPoint.isActiveAndEnabled && NPCZoneManager.IsWithinVerticalBand(npcspawnPoint, npcspawnPoint.transform.position.y - playerPos.y))
			{
				Vector3 vector = npcspawnPoint.transform.position - playerPos;
				float num3 = vector.x * vector.x + vector.z * vector.z;
				if ((!this.IsInitialPoiSafetyActive || num3 >= this._initialPoiSafeRadiusSqr) && !this.IsInsidePoiEntryExclusion(npcspawnPoint.transform.position))
				{
					ChunkInstance chunkInstance = this.FindOwningChunk(npcspawnPoint);
					Vector3 vector2;
					if (chunkInstance != null && !(chunkInstance.chunkRoot == null) && chunkInstance.chunkRoot.activeInHierarchy && this.TryResolveSpawnPosition(npcspawnPoint, num, out vector2) && this.IsSpawnOccludedWhenRequired(npcspawnPoint, vector2) && this.IsSpawnPositionClear(vector2, false))
					{
						this.SpawnNPC(vector2, npcspawnPoint.transform.rotation, chunkInstance, npcspawnPoint.maxVerticalDistanceFromPlayer);
						num2++;
					}
				}
			}
		}
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x0003D120 File Offset: 0x0003B320
	private void RetireExcessPopulation(Vector3 playerPos, int targetNear, int targetAmbient, ref int npcsNear, ref int npcsAmbient)
	{
		int num = targetNear + targetAmbient;
		int num2 = Mathf.Max(0, this._currentNpcCount - num);
		int num3 = Mathf.Max(0, targetNear - npcsNear - this.CountPendingInNearRadius(playerPos));
		int num4 = Mathf.Max(0, targetAmbient - npcsAmbient - this.CountPendingInAmbientRadius(playerPos));
		int num5 = Mathf.Max(0, this._currentNpcCount - npcsNear - npcsAmbient) + Mathf.Max(0, npcsNear - targetNear) + Mathf.Max(0, npcsAmbient - targetAmbient);
		int num6 = Mathf.Min(num3 + num4, num5);
		int num7 = Mathf.Max(0, this._maxTotalNpc - this._currentNpcCount - this._pendingSpawns.Count);
		int num8 = Mathf.Max(0, num3 - num7);
		int num9 = Mathf.Min(this._retirementsPerRebalance, Mathf.Max(num2, Mathf.Max(num6, num8)));
		for (int i = 0; i < num9; i++)
		{
			NPCZoneManager.NPCState npcstate = null;
			int num10 = -1;
			float num11 = float.NegativeInfinity;
			for (int j = 0; j < this._allNpcs.Count; j++)
			{
				NPCZoneManager.NPCState npcstate2 = this._allNpcs[j];
				if (this.CanRetirePopulationNpc(npcstate2))
				{
					Vector3 vector = npcstate2.npc.transform.position - playerPos;
					float sqrMagnitude = vector.sqrMagnitude;
					int num12 = ((npcstate2.maxVerticalDistanceFromPlayer > 0f && Mathf.Abs(vector.y) > npcstate2.maxVerticalDistanceFromPlayer) ? 2 : ((sqrMagnitude <= this._nearRadiusSqr) ? 0 : ((sqrMagnitude <= this._ambientRadiusSqr) ? 1 : 2)));
					if (num12 == 2 || (num12 == 1 && npcsAmbient > targetAmbient) || (num12 == 0 && npcsNear > targetNear))
					{
						float num13 = ((num12 == 2) ? 200000f : ((num12 == 1) ? 100000f : 0f)) + sqrMagnitude;
						if (num13 > num11)
						{
							npcstate = npcstate2;
							num10 = num12;
							num11 = num13;
						}
					}
				}
			}
			if (npcstate == null || !this.DespawnSingleNPC(npcstate))
			{
				break;
			}
			if (num10 == 0)
			{
				npcsNear = Mathf.Max(0, npcsNear - 1);
			}
			else if (num10 == 1)
			{
				npcsAmbient = Mathf.Max(0, npcsAmbient - 1);
			}
		}
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x0003D340 File Offset: 0x0003B540
	private bool CanRetirePopulationNpc(NPCZoneManager.NPCState state)
	{
		return state != null && !(state.npc == null) && (!(state.controller != null) || (!state.controller.SuppressPOIBehavior && !state.controller.IsManualMode && !state.controller.IsNavigationLocked)) && (!(state.wanderController != null) || !state.wanderController.ExternalControl) && (!(state.senses != null) || (!state.senses.IsAlert && !state.senses.IsDetected)) && (!(state.stateMachine != null) || !state.stateMachine.IsInAlertState) && NPCZoneManager.IsNpcOutsideCamera(state.npc.transform.position);
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x0003D410 File Offset: 0x0003B610
	private static bool IsNpcOutsideCamera(Vector3 position)
	{
		Camera main = Camera.main;
		return main == null || (!NPCZoneManager.IsPointInsideCamera(main, position + Vector3.up * 0.2f) && !NPCZoneManager.IsPointInsideCamera(main, position + Vector3.up * 1.05f) && !NPCZoneManager.IsPointInsideCamera(main, position + Vector3.up * 1.75f));
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x0003D488 File Offset: 0x0003B688
	private static bool IsPointInsideCamera(Camera gameplayCamera, Vector3 point)
	{
		Vector3 vector = gameplayCamera.WorldToViewportPoint(point);
		return vector.z > 0f && vector.x >= 0f && vector.x <= 1f && vector.y >= 0f && vector.y <= 1f;
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x0003D4E4 File Offset: 0x0003B6E4
	private static bool TryGetWalkableArea(Vector3 position, out uint area)
	{
		area = 0U;
		if (AstarPath.active == null)
		{
			return false;
		}
		NNInfo nearest = AstarPath.active.GetNearest(position, NearestNodeConstraint.Walkable);
		if (nearest.node == null || !nearest.node.Walkable)
		{
			return false;
		}
		area = nearest.node.Area;
		return true;
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x0003D53C File Offset: 0x0003B73C
	private bool TryResolveSpawnPosition(NPCSpawnPoint spawnPoint, uint requiredArea, out Vector3 resolvedPosition)
	{
		Vector3 vector = ((spawnPoint != null) ? spawnPoint.transform.position : Vector3.zero);
		resolvedPosition = vector;
		if (spawnPoint == null || AstarPath.active == null)
		{
			return false;
		}
		NNInfo nearest = AstarPath.active.GetNearest(vector, NearestNodeConstraint.Walkable);
		if (nearest.node == null || !nearest.node.Walkable)
		{
			return false;
		}
		if (spawnPoint.maxVerticalDistanceFromPlayer > 0f && nearest.node.Area != requiredArea)
		{
			return false;
		}
		Vector3 position = nearest.position;
		Vector2 vector2 = new Vector2(position.x - vector.x, position.z - vector.z);
		if (vector2.magnitude > this._maxSpawnNavmeshHorizontalCorrection)
		{
			return false;
		}
		if (Mathf.Abs(position.y - vector.y) > this._maxSpawnNavmeshVerticalCorrection)
		{
			return false;
		}
		resolvedPosition = position;
		return true;
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x0003D626 File Offset: 0x0003B826
	private static bool IsWithinVerticalBand(NPCSpawnPoint spawnPoint, float verticalDelta)
	{
		return spawnPoint == null || spawnPoint.maxVerticalDistanceFromPlayer <= 0f || Mathf.Abs(verticalDelta) <= spawnPoint.maxVerticalDistanceFromPlayer;
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x0003D654 File Offset: 0x0003B854
	private bool IsSpawnOccludedWhenRequired(NPCSpawnPoint spawnPoint, Vector3 resolvedPosition)
	{
		if (spawnPoint == null || !spawnPoint.requireCameraOcclusion)
		{
			return true;
		}
		Camera main = Camera.main;
		return main == null || (this.IsCameraSampleHidden(main, resolvedPosition + Vector3.up * 0.2f) && this.IsCameraSampleHidden(main, resolvedPosition + Vector3.up * 1.05f) && this.IsCameraSampleHidden(main, resolvedPosition + Vector3.up * 1.75f));
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x0003D6E0 File Offset: 0x0003B8E0
	private bool IsCameraSampleHidden(Camera gameplayCamera, Vector3 target)
	{
		Vector3 vector = gameplayCamera.WorldToViewportPoint(target);
		if (vector.z <= 0f || vector.x < 0f || vector.x > 1f || vector.y < 0f || vector.y > 1f)
		{
			return true;
		}
		Vector3 position = gameplayCamera.transform.position;
		Vector3 vector2 = target - position;
		float magnitude = vector2.magnitude;
		if (magnitude <= 0.25f)
		{
			return false;
		}
		vector2 /= magnitude;
		int num = Physics.RaycastNonAlloc(position, vector2, NPCZoneManager._spawnOcclusionHits, magnitude - 0.2f, -1, QueryTriggerInteraction.Ignore);
		for (int i = 0; i < num; i++)
		{
			Collider collider = NPCZoneManager._spawnOcclusionHits[i].collider;
			if (!(collider == null) && !(collider.transform == gameplayCamera.transform) && !collider.transform.IsChildOf(gameplayCamera.transform))
			{
				Transform playerTransform = this.PlayerTransform;
				if ((!(playerTransform != null) || (!(collider.transform == playerTransform) && !collider.transform.IsChildOf(playerTransform))) && !NPCZoneManager.IsTransparentSpawnOccluder(collider))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600086C RID: 2156 RVA: 0x0003D820 File Offset: 0x0003BA20
	private static bool IsTransparentSpawnOccluder(Collider collider)
	{
		Transform transform = ((collider != null) ? collider.transform : null);
		int num = 0;
		while (transform != null && num < 4)
		{
			string name = transform.name;
			if (NPCZoneManager.ContainsIgnoreCase(name, "glass") || NPCZoneManager.ContainsIgnoreCase(name, "window") || NPCZoneManager.ContainsIgnoreCase(name, "vetr") || NPCZoneManager.ContainsIgnoreCase(name, "crystal"))
			{
				return true;
			}
			num++;
			transform = transform.parent;
		}
		Renderer renderer = ((collider != null) ? collider.GetComponent<Renderer>() : null);
		Material material = ((renderer != null) ? renderer.sharedMaterial : null);
		if (material == null)
		{
			return false;
		}
		string name2 = material.name;
		return NPCZoneManager.ContainsIgnoreCase(name2, "glass") || NPCZoneManager.ContainsIgnoreCase(name2, "window") || NPCZoneManager.ContainsIgnoreCase(name2, "vetr") || NPCZoneManager.ContainsIgnoreCase(name2, "crystal");
	}

	// Token: 0x0600086D RID: 2157 RVA: 0x0002E9C8 File Offset: 0x0002CBC8
	private static bool ContainsIgnoreCase(string value, string token)
	{
		return !string.IsNullOrEmpty(value) && value.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x0003D910 File Offset: 0x0003BB10
	private bool IsSpawnPositionClear(Vector3 position, bool includePending)
	{
		if (this._minNpcSpawnSpacingSqr <= 0f)
		{
			return true;
		}
		for (int i = 0; i < this._allNpcs.Count; i++)
		{
			NPCZoneManager.NPCState npcstate = this._allNpcs[i];
			if (npcstate != null && !(npcstate.npc == null))
			{
				Vector3 vector = npcstate.npc.transform.position - position;
				vector.y = 0f;
				if (vector.sqrMagnitude < this._minNpcSpawnSpacingSqr)
				{
					return false;
				}
			}
		}
		if (includePending)
		{
			foreach (NPCSpawnPoint npcspawnPoint in this._pendingSpawns)
			{
				if (this.IsPendingSpawnAvailable(npcspawnPoint))
				{
					Vector3 vector2 = npcspawnPoint.transform.position - position;
					vector2.y = 0f;
					if (vector2.sqrMagnitude < this._minNpcSpawnSpacingSqr)
					{
						return false;
					}
				}
			}
			return true;
		}
		return true;
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x0003DA1C File Offset: 0x0003BC1C
	private ChunkInstance FindOwningChunk(NPCSpawnPoint sp)
	{
		if (sp == null)
		{
			return null;
		}
		ChunkInstance chunkInstance;
		if (this._spawnPointToChunk.TryGetValue(sp, out chunkInstance))
		{
			return chunkInstance;
		}
		return null;
	}

	// Token: 0x06000870 RID: 2160 RVA: 0x0003DA48 File Offset: 0x0003BC48
	private bool DespawnSingleNPC(NPCZoneManager.NPCState state)
	{
		if (state == null || state.npc == null)
		{
			return false;
		}
		if (state.isActive)
		{
			this._activeNpcCount--;
		}
		NPCZoneManager.NPCEvent onNPCDespawned = this.OnNPCDespawned;
		if (onNPCDespawned != null)
		{
			onNPCDespawned(state.npc, state.chunk);
		}
		if (this.useRuntimeSpawner && this.runtimeSpawner != null)
		{
			if (this.HasPreparationFocus)
			{
				this.runtimeSpawner.DespawnNPCImmediate(state.npc, false);
			}
			else
			{
				this.runtimeSpawner.DespawnNPC(state.npc, false);
			}
		}
		else
		{
			Object.Destroy(state.npc);
		}
		this._currentNpcCount--;
		this._allNpcs.Remove(state);
		this.InvalidatePreparationPopulation();
		List<NPCZoneManager.NPCState> list;
		if (state.chunk != null && this._chunkNpcs.TryGetValue(state.chunk, out list))
		{
			list.Remove(state);
			if (list.Count == 0)
			{
				this._chunkNpcs.Remove(state.chunk);
				this._chunksWithNpcs.Remove(state.chunk);
			}
		}
		return true;
	}

	// Token: 0x06000871 RID: 2161 RVA: 0x0003DB60 File Offset: 0x0003BD60
	private void InitializeReferences()
	{
		if (GameSystemsBootstrapper.Instance != null)
		{
			if (this.player == null)
			{
				this.player = GameSystemsBootstrapper.Instance.Player;
			}
			if (this.staticChunksContainer == null)
			{
				this.staticChunksContainer = GameSystemsBootstrapper.Instance.StaticChunksContainer;
			}
		}
		if (this.staticChunksContainer == null)
		{
			SceneContext sceneContext = SceneContext.Current;
			if (sceneContext != null && sceneContext.NpcSpawnContainer != null)
			{
				this.staticChunksContainer = sceneContext.NpcSpawnContainer;
				if (this.enableDebugLogs)
				{
					Debug.Log("[NPCZoneManager] staticChunksContainer preso da SceneContext: '" + this.staticChunksContainer.name + "'.");
				}
			}
		}
		if (this.player == null && PlayerProximityProvider.Instance != null)
		{
			this.player = PlayerProximityProvider.Instance.Player;
		}
		if (this.player == null)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			this.player = ((gameObject != null) ? gameObject.transform : null);
		}
		if (this.runtimeSpawner == null)
		{
			this.runtimeSpawner = Object.FindObjectOfType<NPCRuntimeSpawner>();
		}
	}

	// Token: 0x06000872 RID: 2162 RVA: 0x0003DC84 File Offset: 0x0003BE84
	private void ConfigureInitialSpawnSafety()
	{
		SceneContext sceneContext = SceneContext.Current;
		SceneProfileSO sceneProfileSO = ((sceneContext != null) ? sceneContext.Profile : null);
		float num = ((sceneProfileSO != null && !sceneProfileSO.proceduralMap) ? Mathf.Max(0f, sceneProfileSO.initialNpcSafeRadius) : 0f);
		this._initialPoiSafeRadiusSqr = num * num;
		this._initialPoiSafetyDuration = ((sceneProfileSO != null) ? Mathf.Max(0f, sceneProfileSO.initialNpcSafeDuration) : 0f);
		this._initialPoiSafetyEndsAtRealtime = ((num > 0f) ? float.PositiveInfinity : 0f);
		this._hasPoiEntryExclusion = num > 0f && sceneContext != null && sceneContext.PlayerSpawnPoint != null;
		this._poiEntryPosition = (this._hasPoiEntryExclusion ? sceneContext.PlayerSpawnPoint.position : Vector3.zero);
	}

	// Token: 0x170001E0 RID: 480
	// (get) Token: 0x06000873 RID: 2163 RVA: 0x0003DD62 File Offset: 0x0003BF62
	private bool IsInitialPoiSafetyActive
	{
		get
		{
			return this._initialPoiSafeRadiusSqr > 0f && Time.realtimeSinceStartup < this._initialPoiSafetyEndsAtRealtime;
		}
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x0003DD80 File Offset: 0x0003BF80
	private bool IsInsidePoiEntryExclusion(Vector3 position)
	{
		if (!this._hasPoiEntryExclusion)
		{
			return false;
		}
		Vector3 vector = position - this._poiEntryPosition;
		return vector.x * vector.x + vector.z * vector.z < this._initialPoiSafeRadiusSqr;
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x0003DDC8 File Offset: 0x0003BFC8
	private void InitializeChunkTracking()
	{
		if (ChunkManager.Instance != null && ChunkManager.Instance.enabled)
		{
			ChunkManager.Instance.OnChunkSpawned += this.OnChunkSpawned;
			ChunkManager.Instance.OnChunkRemoved += this.OnChunkRemoved;
			ChunkManager.Instance.OnChunkZonesReady += this.OnChunkZonesReady;
			if (ChunkManager.Instance.IsInitialGenerationComplete)
			{
				this._initialGenerationGateOpen = true;
			}
			else
			{
				this._initialGenerationGateOpen = false;
				ChunkManager.Instance.OnInitialGenerationComplete += this.OnInitialGenerationCompleteHandler;
			}
			foreach (KeyValuePair<Vector2Int, ChunkInstance> keyValuePair in ChunkManager.Instance.SpawnedChunks)
			{
				this.OnChunkSpawned(keyValuePair.Value);
			}
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Format("[NPCZoneManager] Initialized with {0} procedural chunks. Spawn gate open={1}.", ChunkManager.Instance.SpawnedChunks.Count, this._initialGenerationGateOpen));
				return;
			}
		}
		else
		{
			this.FindStaticChunks();
			this._initialGenerationGateOpen = true;
		}
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x0003DEF8 File Offset: 0x0003C0F8
	private void OnInitialGenerationCompleteHandler()
	{
		this._initialGenerationGateOpen = true;
		if (this.enableDebugLogs)
		{
			Debug.Log("[NPCZoneManager] Procedural map initial generation complete — opening NPC spawn gate.");
		}
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x0003DF14 File Offset: 0x0003C114
	private void FindStaticChunks()
	{
		this._staticChunks.Clear();
		if (this.staticChunksContainer == null)
		{
			if (this.enableDebugLogs)
			{
				Debug.LogWarning("[NPCZoneManager] staticChunksContainer non assegnato! Assegna il GameObject che contiene i chunk.");
			}
			return;
		}
		foreach (object obj in this.staticChunksContainer)
		{
			Transform transform = (Transform)obj;
			if (transform.GetComponentsInChildren<NPCSpawnPoint>().Length == 0)
			{
				if (this.enableDebugLogs)
				{
					Debug.LogWarning("[NPCZoneManager] Chunk '" + transform.name + "' non ha NPCSpawnPoint, ignorato.");
				}
			}
			else
			{
				ChunkInstance chunkInstance = new ChunkInstance(null, transform.position, transform.rotation);
				chunkInstance.chunkRoot = transform.gameObject;
				this._staticChunks.Add(chunkInstance);
				Dictionary<ChunkInstance, int> chunkIdMap = this._chunkIdMap;
				ChunkInstance chunkInstance2 = chunkInstance;
				int nextChunkId = this._nextChunkId;
				this._nextChunkId = nextChunkId + 1;
				chunkIdMap[chunkInstance2] = nextChunkId;
				this.CacheInterestPoints(chunkInstance);
				this.CacheSpawnPoints(chunkInstance);
			}
		}
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Format("[NPCZoneManager] Found {0} static chunks from '{1}'.", this._staticChunks.Count, this.staticChunksContainer.name));
		}
	}

	// Token: 0x06000878 RID: 2168 RVA: 0x0003E050 File Offset: 0x0003C250
	private void OnChunkSpawned(ChunkInstance chunk)
	{
		if (chunk == null)
		{
			return;
		}
		if (!this._chunkIdMap.ContainsKey(chunk))
		{
			Dictionary<ChunkInstance, int> chunkIdMap = this._chunkIdMap;
			int nextChunkId = this._nextChunkId;
			this._nextChunkId = nextChunkId + 1;
			chunkIdMap[chunk] = nextChunkId;
		}
		this.CacheInterestPoints(chunk);
		this.CacheSpawnPoints(chunk);
		this.InvalidatePreparationPopulation();
	}

	// Token: 0x06000879 RID: 2169 RVA: 0x0003E0A0 File Offset: 0x0003C2A0
	private void OnChunkZonesReady(ChunkInstance chunk)
	{
		if (chunk == null)
		{
			return;
		}
		this.CacheInterestPoints(chunk);
		this.CacheSpawnPoints(chunk);
		this.InvalidatePreparationPopulation();
	}

	// Token: 0x0600087A RID: 2170 RVA: 0x0003E0BC File Offset: 0x0003C2BC
	private void OnChunkRemoved(ChunkInstance chunk)
	{
		if (chunk == null)
		{
			return;
		}
		this.InvalidatePreparationPopulation();
		this._chunksQueued.Remove(chunk);
		this.DespawnChunkNPCsCore(chunk, true);
		this._chunkIdMap.Remove(chunk);
		if (this._chunkInterestPoints.ContainsKey(chunk))
		{
			this._chunkInterestPoints.Remove(chunk);
		}
		List<NPCSpawnPoint> list;
		if (this._chunkSpawnPoints.TryGetValue(chunk, out list))
		{
			for (int i = 0; i < list.Count; i++)
			{
				NPCSpawnPoint npcspawnPoint = list[i];
				ChunkInstance chunkInstance;
				if (npcspawnPoint != null && this._spawnPointToChunk.TryGetValue(npcspawnPoint, out chunkInstance) && chunkInstance == chunk)
				{
					this._spawnPointToChunk.Remove(npcspawnPoint);
				}
			}
			this._chunkSpawnPoints.Remove(chunk);
		}
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x0003E16C File Offset: 0x0003C36C
	private void CacheSpawnPoints(ChunkInstance chunk)
	{
		if (chunk == null)
		{
			return;
		}
		using (NPCZoneManager.CacheSpawnPointsMarker.Auto())
		{
			List<NPCSpawnPoint> list = new List<NPCSpawnPoint>(16);
			this.CollectUniqueChunkPoints<NPCSpawnPoint>(chunk, true, list, this._spawnPointComponentScratch, this._spawnPointDedupScratch);
			List<NPCSpawnPoint> list2;
			if (this._chunkSpawnPoints.TryGetValue(chunk, out list2))
			{
				for (int i = 0; i < list2.Count; i++)
				{
					NPCSpawnPoint npcspawnPoint = list2[i];
					ChunkInstance chunkInstance;
					if (npcspawnPoint != null && this._spawnPointToChunk.TryGetValue(npcspawnPoint, out chunkInstance) && chunkInstance == chunk)
					{
						this._spawnPointToChunk.Remove(npcspawnPoint);
					}
				}
			}
			this._chunkSpawnPoints[chunk] = list;
			for (int j = 0; j < list.Count; j++)
			{
				this._spawnPointToChunk[list[j]] = chunk;
			}
			if (this.enableDebugLogs)
			{
				string text = "[NPCZoneManager] Cached {0} spawn points for chunk '{1}'";
				object obj = list.Count;
				GameObject chunkRoot = chunk.chunkRoot;
				Debug.Log(string.Format(text, obj, (chunkRoot != null) ? chunkRoot.name : null));
			}
		}
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x0003E28C File Offset: 0x0003C48C
	private void CacheInterestPoints(ChunkInstance chunk)
	{
		if (chunk == null)
		{
			return;
		}
		using (NPCZoneManager.CacheInterestPointsMarker.Auto())
		{
			List<NPCInterestPoint> list = new List<NPCInterestPoint>();
			this.CollectUniqueChunkPoints<NPCInterestPoint>(chunk, false, list, this._interestPointComponentScratch, this._interestPointDedupScratch);
			if (list.Count > 0)
			{
				this._chunkInterestPoints[chunk] = list;
				if (this.enableDebugLogs)
				{
					string text = "[NPCZoneManager] Cached {0} interest points for chunk '{1}'";
					object obj = list.Count;
					GameObject chunkRoot = chunk.chunkRoot;
					Debug.Log(string.Format(text, obj, (chunkRoot != null) ? chunkRoot.name : null));
				}
			}
			else
			{
				this._chunkInterestPoints.Remove(chunk);
				if (this.enableDebugLogs)
				{
					string text2 = "[NPCZoneManager] NO interest points found in chunk '{0}' (chunkRoot={1}, spawnedZones={2})";
					GameObject chunkRoot2 = chunk.chunkRoot;
					object obj2 = ((chunkRoot2 != null) ? chunkRoot2.name : null);
					object obj3 = ((chunk.chunkRoot != null) ? "OK" : "NULL");
					List<GameObject> spawnedZones = chunk.spawnedZones;
					Debug.LogWarning(string.Format(text2, obj2, obj3, (spawnedZones != null) ? spawnedZones.Count : 0));
				}
			}
		}
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x0003E39C File Offset: 0x0003C59C
	private void CollectUniqueChunkPoints<T>(ChunkInstance chunk, bool includeInactive, List<T> points, List<T> componentScratch, HashSet<T> seen) where T : Component
	{
		ChunkContentRootTraversal.Collect(chunk, includeInactive, this._pointRootScratch);
		seen.Clear();
		for (int i = 0; i < this._pointRootScratch.Count; i++)
		{
			this._pointRootScratch[i].GetComponentsInChildren<T>(includeInactive, componentScratch);
			for (int j = 0; j < componentScratch.Count; j++)
			{
				T t = componentScratch[j];
				if (t != null && seen.Add(t))
				{
					points.Add(t);
				}
			}
		}
		componentScratch.Clear();
		seen.Clear();
		this._pointRootScratch.Clear();
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x0003E43C File Offset: 0x0003C63C
	public NPCInterestPoint GetRandomInterestPoint(Vector3 npcPosition, float searchRadius = 50f)
	{
		ChunkInstance chunkInstance = null;
		float num = float.MaxValue;
		float num2 = searchRadius * searchRadius;
		if (this.enableDebugLogs)
		{
			Debug.Log(string.Format("[NPCZoneManager] GetRandomInterestPoint: npcPos={0}, radius={1}, chunksWithPOI={2}, scenePOIs={3}", new object[]
			{
				npcPosition,
				searchRadius,
				this._chunkInterestPoints.Count,
				this._scenePOIs.Count
			}));
		}
		List<ChunkInstance> interestPointChunkScratch = this._interestPointChunkScratch;
		interestPointChunkScratch.Clear();
		float num3 = searchRadius * 1.5f * (searchRadius * 1.5f);
		foreach (KeyValuePair<ChunkInstance, List<NPCInterestPoint>> keyValuePair in this._chunkInterestPoints)
		{
			ChunkInstance key = keyValuePair.Key;
			if (!(key.chunkRoot == null) && key.chunkRoot.activeInHierarchy)
			{
				float num4 = float.PositiveInfinity;
				foreach (NPCInterestPoint npcinterestPoint in keyValuePair.Value)
				{
					if (npcinterestPoint != null && npcinterestPoint.isActiveAndEnabled && npcinterestPoint.IsAvailable)
					{
						num4 = Mathf.Min(num4, (npcinterestPoint.transform.position - npcPosition).sqrMagnitude);
					}
				}
				if (num4 <= num2)
				{
					interestPointChunkScratch.Add(key);
				}
				if (num4 <= num3 && num4 < num)
				{
					num = num4;
					chunkInstance = key;
				}
			}
		}
		if (this.enableDebugLogs)
		{
			string text = "[NPCZoneManager] Found {0} valid chunks in radius, bestChunk={1} at dist={2:F1}";
			object obj = interestPointChunkScratch.Count;
			object obj2;
			if (chunkInstance == null)
			{
				obj2 = "null";
			}
			else
			{
				GameObject chunkRoot = chunkInstance.chunkRoot;
				obj2 = ((chunkRoot != null) ? chunkRoot.name : null);
			}
			Debug.Log(string.Format(text, obj, obj2, Mathf.Sqrt(num)));
		}
		if (interestPointChunkScratch.Count == 0)
		{
			if (this._scenePOIs.Count > 0)
			{
				NPCInterestPoint npcinterestPoint2 = this.PickNearestAvailableScenePOI(npcPosition, num3);
				if (npcinterestPoint2 != null)
				{
					if (this.enableDebugLogs)
					{
						Debug.Log("[NPCZoneManager] Using nearby scene POI fallback: " + npcinterestPoint2.name);
					}
					return npcinterestPoint2;
				}
			}
			List<NPCInterestPoint> list;
			if (chunkInstance != null && this._chunkInterestPoints.TryGetValue(chunkInstance, out list))
			{
				if (this.enableDebugLogs)
				{
					string text2 = "[NPCZoneManager] Using fallback bestChunk '{0}' with {1} POIs (dist={2:F1})";
					GameObject chunkRoot2 = chunkInstance.chunkRoot;
					Debug.Log(string.Format(text2, (chunkRoot2 != null) ? chunkRoot2.name : null, list.Count, Mathf.Sqrt(num)));
				}
				return this.GetRandomAvailablePoint(list, npcPosition, num3);
			}
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Format("[NPCZoneManager] No chunks with POI found within {0:F0}m of NPC.", Mathf.Sqrt(num3)));
			}
			return null;
		}
		else
		{
			ChunkInstance chunkInstance2 = interestPointChunkScratch[global::UnityEngine.Random.Range(0, interestPointChunkScratch.Count)];
			interestPointChunkScratch.Clear();
			List<NPCInterestPoint> list2;
			if (this._chunkInterestPoints.TryGetValue(chunkInstance2, out list2))
			{
				if (this.enableDebugLogs)
				{
					string text3 = "[NPCZoneManager] Selected chunk '{0}' with {1} POIs";
					GameObject chunkRoot3 = chunkInstance2.chunkRoot;
					Debug.Log(string.Format(text3, (chunkRoot3 != null) ? chunkRoot3.name : null, list2.Count));
				}
				return this.GetRandomAvailablePoint(list2, npcPosition, num2);
			}
			if (this.enableDebugLogs)
			{
				Debug.Log("[NPCZoneManager] Selected chunk has no POIs in dictionary!");
			}
			return null;
		}
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x0003E77C File Offset: 0x0003C97C
	private NPCInterestPoint PickNearestAvailableScenePOI(Vector3 npcPosition, float maxSqrDist)
	{
		NPCInterestPoint npcinterestPoint = null;
		float num = maxSqrDist;
		for (int i = 0; i < this._scenePOIs.Count; i++)
		{
			NPCInterestPoint npcinterestPoint2 = this._scenePOIs[i];
			if (!(npcinterestPoint2 == null) && npcinterestPoint2.gameObject.activeInHierarchy && npcinterestPoint2.IsAvailable)
			{
				Vector3 vector = npcinterestPoint2.transform.position - npcPosition;
				float num2 = vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
				if (num2 <= num)
				{
					num = num2;
					npcinterestPoint = npcinterestPoint2;
				}
			}
		}
		return npcinterestPoint;
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x0003E820 File Offset: 0x0003CA20
	public NPCInterestPoint GetAmbientInterestPoint(Vector3 position, float searchRadius, NPCInterestPoint previous, bool seekCompany = true)
	{
		NPCAmbientLifeSettings npcambientLifeSettings = NPCAmbientLifeSettings.Current;
		if (npcambientLifeSettings == null || !npcambientLifeSettings.enableAmbientLife)
		{
			return this.GetRandomInterestPoint(position, searchRadius);
		}
		float num = Mathf.Min(searchRadius, npcambientLifeSettings.nearbyStopRadius);
		float num2 = num * num;
		NPCInterestPoint npcinterestPoint = null;
		NPCInterestPoint npcinterestPoint2 = null;
		NPCInterestPoint npcinterestPoint3 = null;
		float maxValue = float.MaxValue;
		float maxValue2 = float.MaxValue;
		float num3 = 0f;
		foreach (KeyValuePair<ChunkInstance, List<NPCInterestPoint>> keyValuePair in this._chunkInterestPoints)
		{
			if (!(keyValuePair.Key.chunkRoot == null) && keyValuePair.Key.chunkRoot.activeInHierarchy)
			{
				foreach (NPCInterestPoint npcinterestPoint4 in keyValuePair.Value)
				{
					NPCZoneManager.ConsiderAmbientPoint(npcinterestPoint4, position, previous, num2, seekCompany, ref npcinterestPoint, ref maxValue, ref npcinterestPoint2, ref maxValue2, ref npcinterestPoint3, ref num3);
				}
			}
		}
		foreach (NPCInterestPoint npcinterestPoint5 in this._scenePOIs)
		{
			NPCZoneManager.ConsiderAmbientPoint(npcinterestPoint5, position, previous, num2, seekCompany, ref npcinterestPoint, ref maxValue, ref npcinterestPoint2, ref maxValue2, ref npcinterestPoint3, ref num3);
		}
		if (npcinterestPoint != null && global::UnityEngine.Random.value < npcambientLifeSettings.joinGroupChance)
		{
			return npcinterestPoint;
		}
		if (npcinterestPoint2 != null && global::UnityEngine.Random.value < npcambientLifeSettings.startGroupChance)
		{
			return npcinterestPoint2;
		}
		if (npcinterestPoint3 != null && global::UnityEngine.Random.value < npcambientLifeSettings.nearbyStopChance)
		{
			return npcinterestPoint3;
		}
		NPCInterestPoint randomInterestPoint = this.GetRandomInterestPoint(position, searchRadius);
		if (seekCompany || !(randomInterestPoint != null) || !randomInterestPoint.SupportsAmbientConversation)
		{
			return randomInterestPoint;
		}
		return null;
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x0003E9F4 File Offset: 0x0003CBF4
	private static void ConsiderAmbientPoint(NPCInterestPoint point, Vector3 position, NPCInterestPoint previous, float radiusSqr, bool seekCompany, ref NPCInterestPoint company, ref float companyScore, ref NPCInterestPoint emptyGroup, ref float emptyDistance, ref NPCInterestPoint nearby, ref float totalWeight)
	{
		if (point == null || point == previous || !point.gameObject.activeInHierarchy || !point.CanReserve)
		{
			return;
		}
		if (!seekCompany && point.SupportsAmbientConversation)
		{
			return;
		}
		float sqrMagnitude = (point.transform.position - position).sqrMagnitude;
		if (sqrMagnitude > radiusSqr)
		{
			return;
		}
		float num = Mathf.Max(0.1f, point.selectionWeight) / (1f + sqrMagnitude / 100f);
		totalWeight += num;
		if (global::UnityEngine.Random.value * totalWeight < num)
		{
			nearby = point;
		}
		if (!point.SupportsAmbientConversation)
		{
			return;
		}
		int num2 = point.ReservationCount + point.OccupantCount;
		if (num2 > 0)
		{
			float num3 = sqrMagnitude + ((num2 > 1) ? radiusSqr : 0f);
			if (num3 < companyScore)
			{
				companyScore = num3;
				company = point;
				return;
			}
		}
		else if (sqrMagnitude < emptyDistance)
		{
			emptyDistance = sqrMagnitude;
			emptyGroup = point;
		}
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x0003EAD8 File Offset: 0x0003CCD8
	private NPCInterestPoint GetRandomAvailablePoint(List<NPCInterestPoint> points, Vector3 position, float radiusSqr)
	{
		if (points == null || points.Count == 0)
		{
			return null;
		}
		int num = 5;
		for (int i = 0; i < num; i++)
		{
			NPCInterestPoint npcinterestPoint = points[global::UnityEngine.Random.Range(0, points.Count)];
			if (npcinterestPoint != null && npcinterestPoint.IsAvailable && npcinterestPoint.isActiveAndEnabled && (npcinterestPoint.transform.position - position).sqrMagnitude <= radiusSqr)
			{
				return npcinterestPoint;
			}
		}
		for (int j = 0; j < points.Count; j++)
		{
			NPCInterestPoint npcinterestPoint2 = points[j];
			if (npcinterestPoint2 != null && npcinterestPoint2.IsAvailable && npcinterestPoint2.isActiveAndEnabled && (npcinterestPoint2.transform.position - position).sqrMagnitude <= radiusSqr)
			{
				return npcinterestPoint2;
			}
		}
		return null;
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x0003EBA8 File Offset: 0x0003CDA8
	private void SpawnNPC(Vector3 position, Quaternion rotation, ChunkInstance chunk, float maxVerticalDistanceFromPlayer)
	{
		if (this._currentNpcCount >= this.maxTotalNpc)
		{
			return;
		}
		GameObject gameObject = null;
		bool flag = false;
		if (this.useRuntimeSpawner && this.runtimeSpawner != null)
		{
			if (this.runtimeSpawner.HasPooledNPCs())
			{
				gameObject = this.runtimeSpawner.GetPooledNPC(position, rotation, chunk.chunkRoot.transform);
				flag = gameObject != null;
				if (this.enableDebugLogs && flag)
				{
					Debug.Log("[NPCZoneManager] Got NPC from pool: " + gameObject.name);
				}
			}
			if (gameObject == null)
			{
				CharacterObject randomCharacterData = this.runtimeSpawner.GetRandomCharacterData();
				if (randomCharacterData != null)
				{
					gameObject = this.runtimeSpawner.SpawnNPCSync(randomCharacterData, position, rotation, chunk.chunkRoot.transform, false);
					if (this.enableDebugLogs)
					{
						Debug.Log("[NPCZoneManager] Spawned new NPC (pool empty): " + ((gameObject != null) ? gameObject.name : null));
					}
				}
			}
		}
		else if (this.legacyPrefabs != null && this.legacyPrefabs.Length != 0)
		{
			GameObject gameObject2 = this.legacyPrefabs[global::UnityEngine.Random.Range(0, this.legacyPrefabs.Length)];
			if (gameObject2 != null)
			{
				gameObject = Object.Instantiate<GameObject>(gameObject2, position, rotation, chunk.chunkRoot.transform);
			}
		}
		if (gameObject != null)
		{
			NPCZoneManager.NPCState npcstate = this.ConfigureNPC(gameObject, chunk, flag, maxVerticalDistanceFromPlayer);
			if (npcstate != null)
			{
				if (this.HasPreparationFocus)
				{
					NPCSpawnFade component = gameObject.GetComponent<NPCSpawnFade>();
					if (component != null)
					{
						component.CompleteFadeInImmediately();
					}
				}
				if (!this._chunkNpcs.ContainsKey(chunk))
				{
					this._chunkNpcs[chunk] = new List<NPCZoneManager.NPCState>();
				}
				this._chunkNpcs[chunk].Add(npcstate);
				this._allNpcs.Add(npcstate);
				this._chunksWithNpcs.Add(chunk);
				this._currentNpcCount++;
				int num = this._allNpcs.Count - 1;
				bool flag2 = (this.PlayerPos - position).sqrMagnitude <= this._activeRadiusSqr;
				this._npcJobData[num] = new NPCJobData
				{
					id = npcstate.id,
					position = new float3(position.x, position.y, position.z),
					isActive = flag2
				};
				this.SetNPCActive(npcstate, flag2, true);
				this.InvalidatePreparationPopulation();
				NPCZoneManager.NPCEvent onNPCSpawned = this.OnNPCSpawned;
				if (onNPCSpawned == null)
				{
					return;
				}
				onNPCSpawned(gameObject, chunk);
			}
		}
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x0003EE0C File Offset: 0x0003D00C
	private NPCZoneManager.NPCState ConfigureNPC(GameObject npc, ChunkInstance chunk, bool isFromPool = false, float maxVerticalDistanceFromPlayer = 0f)
	{
		if (npc == null)
		{
			return null;
		}
		NPCZoneManager.NPCState npcstate = new NPCZoneManager.NPCState();
		int nextNpcId = this._nextNpcId;
		this._nextNpcId = nextNpcId + 1;
		npcstate.id = nextNpcId;
		npcstate.npc = npc;
		npcstate.chunk = chunk;
		npcstate.isActive = false;
		npcstate.maxVerticalDistanceFromPlayer = Mathf.Max(0f, maxVerticalDistanceFromPlayer);
		NPCZoneManager.NPCState npcstate2 = npcstate;
		float num = ((this.npcComponentConfig != null) ? this.npcComponentConfig.moveSpeed : this.npcSpeed);
		npcstate2.restorePreparationSpawnSpeed = this.HasPreparationFocus;
		npcstate2.preparationSpawnSpeed = num;
		float num2 = ((this.npcComponentConfig != null) ? this.npcComponentConfig.rotationSpeed : this.npcRotationSpeed);
		float num3 = ((this.npcComponentConfig != null) ? this.npcComponentConfig.radius : this.npcRadius);
		float num4 = ((this.npcComponentConfig != null) ? this.npcComponentConfig.height : this.npcHeight);
		bool flag = ((this.npcComponentConfig != null) ? this.npcComponentConfig.enableLocalAvoidance : this.enableLocalAvoidance);
		RuntimeAnimatorController runtimeAnimatorController = ((this.npcComponentConfig != null) ? this.npcComponentConfig.animatorController : this.animatorController);
		npcstate2.follower = npc.GetComponent<FollowerEntity>();
		if (npcstate2.follower == null)
		{
			npcstate2.follower = npc.AddComponent<FollowerEntity>();
		}
		npcstate2.follower.maxSpeed = num;
		npcstate2.follower.rotationSpeed = num2;
		npcstate2.follower.radius = num3;
		npcstate2.follower.height = num4;
		npcstate2.follower.enableLocalAvoidance = flag;
		npcstate2.controller = npc.GetComponent<AStarNPCController>();
		if (npcstate2.controller == null)
		{
			npcstate2.controller = npc.AddComponent<AStarNPCController>();
		}
		npcstate2.controller.follower = npcstate2.follower;
		NPCEquipmentManager component = npc.GetComponent<NPCEquipmentManager>();
		if (component != null)
		{
			npcstate2.controller.equipmentManager = component;
		}
		npcstate2.animator = npc.GetComponentInChildren<Animator>();
		if (npcstate2.animator != null)
		{
			bool flag2 = npc.GetComponent<NPCDiveController>() != null;
			if (runtimeAnimatorController != null && !flag2)
			{
				npcstate2.animator.runtimeAnimatorController = runtimeAnimatorController;
			}
			if (this.npcComponentConfig != null && this.npcComponentConfig.animatorAvatar != null)
			{
				npcstate2.animator.avatar = this.npcComponentConfig.animatorAvatar;
			}
			npcstate2.controller.animator = npcstate2.animator;
		}
		npcstate2.wanderController = npc.GetComponent<NPCWanderController>();
		if (npcstate2.wanderController == null)
		{
			npcstate2.wanderController = npc.AddComponent<NPCWanderController>();
		}
		if (npcstate2.wanderController == null)
		{
			SimpleAStarWander simpleAStarWander = npc.GetComponent<SimpleAStarWander>();
			if (simpleAStarWander == null)
			{
				simpleAStarWander = npc.AddComponent<SimpleAStarWander>();
			}
			simpleAStarWander.enableDebugLogs = this.enableDebugLogs;
			npcstate2.simpleWander = simpleAStarWander;
		}
		Rigidbody rigidbody = npc.GetComponent<Rigidbody>();
		if (rigidbody == null)
		{
			rigidbody = npc.AddComponent<Rigidbody>();
		}
		rigidbody.isKinematic = true;
		rigidbody.useGravity = false;
		NavMeshObstacle component2 = npc.GetComponent<NavMeshObstacle>();
		if (component2 != null)
		{
			Object.Destroy(component2);
		}
		NavMeshAgent component3 = npc.GetComponent<NavMeshAgent>();
		if (component3 != null)
		{
			Object.Destroy(component3);
		}
		npcstate2.senses = npc.GetComponent<NPCSenses>();
		npcstate2.stateMachine = npc.GetComponent<NPCStateMachine>();
		return npcstate2;
	}

	// Token: 0x06000885 RID: 2181 RVA: 0x0003F16C File Offset: 0x0003D36C
	private Transform FindChildRecursive(Transform parent, string name)
	{
		foreach (object obj in parent)
		{
			Transform transform = (Transform)obj;
			if (transform.name.Contains(name))
			{
				return transform;
			}
			Transform transform2 = this.FindChildRecursive(transform, name);
			if (transform2 != null)
			{
				return transform2;
			}
		}
		return null;
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x0003F1E8 File Offset: 0x0003D3E8
	private void SetNPCActive(NPCZoneManager.NPCState state, bool active, bool forceApply = false)
	{
		bool flag = state.isActive != active;
		if (!flag && !forceApply)
		{
			return;
		}
		if (flag)
		{
			state.isActive = active;
			if (active)
			{
				this._activeNpcCount++;
			}
			else
			{
				this._activeNpcCount--;
			}
		}
		if (this.HasPreparationFocus)
		{
			state.preparationActivationPending = true;
			if (state.animator != null)
			{
				state.animator.speed = 0f;
			}
			if (state.follower != null)
			{
				state.follower.isStopped = true;
			}
			if (state.senses != null)
			{
				state.senses.enabled = active;
			}
			if (state.npc != null && state.npc.activeInHierarchy)
			{
				NPCSimulationPause.RegisterParticipant(state.npc.transform);
			}
			return;
		}
		this.ApplyNPCActiveComponents(state, active);
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x0003F2CC File Offset: 0x0003D4CC
	private void OnSimulationPauseChanged(bool paused)
	{
		if (paused)
		{
			return;
		}
		for (int i = 0; i < this._allNpcs.Count; i++)
		{
			NPCZoneManager.NPCState npcstate = this._allNpcs[i];
			if (npcstate != null && !(npcstate.npc == null) && npcstate.preparationActivationPending)
			{
				npcstate.preparationActivationPending = false;
				if (npcstate.restorePreparationSpawnSpeed && npcstate.follower != null)
				{
					npcstate.follower.maxSpeed = npcstate.preparationSpawnSpeed;
				}
				npcstate.restorePreparationSpawnSpeed = false;
				this.ApplyNPCActiveComponents(npcstate, npcstate.isActive);
			}
		}
	}

	// Token: 0x06000888 RID: 2184 RVA: 0x0003F35C File Offset: 0x0003D55C
	private void ApplyNPCActiveComponents(NPCZoneManager.NPCState state, bool active)
	{
		if (active)
		{
			if (state.animator != null)
			{
				state.animator.speed = 1f;
			}
			if (state.controller != null)
			{
				state.controller.enabled = true;
			}
			if (state.follower != null)
			{
				state.follower.enabled = true;
				state.follower.isStopped = false;
			}
			if (state.senses != null)
			{
				state.senses.enabled = true;
			}
			if (state.wanderController != null)
			{
				state.wanderController.enabled = true;
			}
			if (state.simpleWander != null)
			{
				state.simpleWander.enabled = true;
			}
			NPCZoneManager.NPCEvent onNPCActivated = this.OnNPCActivated;
			if (onNPCActivated == null)
			{
				return;
			}
			onNPCActivated(state.npc, state.chunk);
			return;
		}
		else
		{
			if (state.wanderController != null)
			{
				state.wanderController.enabled = false;
			}
			if (state.simpleWander != null)
			{
				state.simpleWander.enabled = false;
			}
			if (state.senses != null)
			{
				state.senses.enabled = false;
			}
			if (state.follower != null)
			{
				state.follower.isStopped = true;
				state.follower.enabled = false;
			}
			if (state.animator != null)
			{
				state.animator.speed = 0f;
			}
			if (state.controller != null)
			{
				state.controller.enabled = false;
			}
			NPCZoneManager.NPCEvent onNPCDeactivated = this.OnNPCDeactivated;
			if (onNPCDeactivated == null)
			{
				return;
			}
			onNPCDeactivated(state.npc, state.chunk);
			return;
		}
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x0003F500 File Offset: 0x0003D700
	public void DespawnNPCsInChunk(ChunkInstance chunk)
	{
		if (chunk == null)
		{
			return;
		}
		this.DespawnChunkNPCs(chunk);
	}

	// Token: 0x0600088A RID: 2186 RVA: 0x0003F50D File Offset: 0x0003D70D
	private void DespawnChunkNPCs(ChunkInstance chunk)
	{
		this.DespawnChunkNPCsCore(chunk, false);
	}

	// Token: 0x0600088B RID: 2187 RVA: 0x0003F518 File Offset: 0x0003D718
	private void DespawnChunkNPCsCore(ChunkInstance chunk, bool immediate)
	{
		List<NPCZoneManager.NPCState> list;
		if (!this._chunkNpcs.TryGetValue(chunk, out list))
		{
			return;
		}
		this.InvalidatePreparationPopulation();
		foreach (NPCZoneManager.NPCState npcstate in list)
		{
			if (npcstate.npc != null)
			{
				if (npcstate.isActive)
				{
					this._activeNpcCount--;
				}
				NPCZoneManager.NPCEvent onNPCDespawned = this.OnNPCDespawned;
				if (onNPCDespawned != null)
				{
					onNPCDespawned(npcstate.npc, chunk);
				}
				if (this.useRuntimeSpawner && this.runtimeSpawner != null)
				{
					if (immediate || this.HasPreparationFocus)
					{
						this.runtimeSpawner.DespawnNPCImmediate(npcstate.npc, false);
					}
					else
					{
						this.runtimeSpawner.DespawnNPC(npcstate.npc, false);
					}
				}
				else
				{
					Object.Destroy(npcstate.npc);
				}
				this._currentNpcCount--;
			}
			this._allNpcs.Remove(npcstate);
		}
		this._chunkNpcs.Remove(chunk);
		this._chunksWithNpcs.Remove(chunk);
		if (this.enableDebugLogs && ((chunk != null) ? chunk.chunkRoot : null) != null)
		{
			Debug.Log(string.Format("[NPCZoneManager] Despawned from '{0}' (Total: {1})", chunk.chunkRoot.name, this._currentNpcCount));
		}
	}

	// Token: 0x0600088C RID: 2188 RVA: 0x0003F688 File Offset: 0x0003D888
	private void ShuffleList<T>(List<T> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			int num = global::UnityEngine.Random.Range(i, list.Count);
			int num2 = i;
			int num3 = num;
			T t = list[num];
			T t2 = list[i];
			list[num2] = t;
			list[num3] = t2;
		}
	}

	// Token: 0x0600088D RID: 2189 RVA: 0x0003F6E8 File Offset: 0x0003D8E8
	private void OnDrawGizmos()
	{
		if (!this.drawDebugGizmos || this.PlayerTransform == null)
		{
			return;
		}
		Vector3 playerPos = this.PlayerPos;
		float num = ((this.spawnConfig != null) ? this.spawnConfig.nearRadius : 30f);
		Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.55f);
		this.DrawCircle(playerPos, num, 32);
		float num2 = ((this.spawnConfig != null) ? this.spawnConfig.activeRadius : this.activeZoneRadius);
		Gizmos.color = new Color(0f, 1f, 0f, 0.35f);
		this.DrawCircle(playerPos, num2, 32);
		float num3 = ((this.spawnConfig != null) ? this.spawnConfig.ambientRadius : 55f);
		Gizmos.color = new Color(1f, 1f, 0f, 0.25f);
		this.DrawCircle(playerPos, num3, 48);
		float num4 = ((this.spawnConfig != null) ? this.spawnConfig.despawnRadius : (this.warmupZoneRadius * 1.3f));
		Gizmos.color = new Color(1f, 0.3f, 0.3f, 0.2f);
		this.DrawCircle(playerPos, num4, 48);
		if (Application.isPlaying)
		{
			foreach (NPCZoneManager.NPCState npcstate in this._allNpcs)
			{
				if (!(npcstate.npc == null))
				{
					Gizmos.color = (npcstate.isActive ? Color.green : Color.yellow);
					Gizmos.DrawWireSphere(npcstate.npc.transform.position, 0.5f);
				}
			}
		}
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x0003F8D0 File Offset: 0x0003DAD0
	private void DrawCircle(Vector3 center, float radius, int segments)
	{
		float num = 360f / (float)segments;
		Vector3 vector = center + new Vector3(radius, 0f, 0f);
		for (int i = 1; i <= segments; i++)
		{
			float num2 = (float)i * num * 0.017453292f;
			Vector3 vector2 = center + new Vector3(Mathf.Cos(num2) * radius, 0f, Mathf.Sin(num2) * radius);
			Gizmos.DrawLine(vector, vector2);
			vector = vector2;
		}
	}

	// Token: 0x0600088F RID: 2191 RVA: 0x0003F944 File Offset: 0x0003DB44
	public List<GameObject> GetAllNPCs()
	{
		List<GameObject> list = new List<GameObject>(this._allNpcs.Count);
		foreach (NPCZoneManager.NPCState npcstate in this._allNpcs)
		{
			if (npcstate.npc != null)
			{
				list.Add(npcstate.npc);
			}
		}
		return list;
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x0003F9BC File Offset: 0x0003DBBC
	public List<GameObject> GetActiveNPCs()
	{
		List<GameObject> list = new List<GameObject>(this._activeNpcCount);
		foreach (NPCZoneManager.NPCState npcstate in this._allNpcs)
		{
			if (npcstate.npc != null && npcstate.isActive)
			{
				list.Add(npcstate.npc);
			}
		}
		return list;
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x0003FA38 File Offset: 0x0003DC38
	public void DespawnAllNPCs()
	{
		this.InvalidatePreparationPopulation();
		this._currentZoneJobHandle.Complete();
		foreach (ChunkInstance chunkInstance in new List<ChunkInstance>(this._chunkNpcs.Keys))
		{
			this.DespawnChunkNPCs(chunkInstance);
		}
		this._spawnQueue.Clear();
		this._chunksQueued.Clear();
		this._pendingSpawns.Clear();
		this._candidateBuffer.Clear();
		this._recentSpawns.Clear();
		this._recentSpawnsKeys.Clear();
		this._allNpcs.Clear();
		this._chunkNpcs.Clear();
		this._chunksWithNpcs.Clear();
		this._currentNpcCount = 0;
		this._activeNpcCount = 0;
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x0003FB18 File Offset: 0x0003DD18
	public void BeginFastTeardown(List<GameObject> rootsOut)
	{
		if (this.IsTearingDown)
		{
			return;
		}
		this.IsTearingDown = true;
		this.ClearPreparationFocus();
		this._currentZoneJobHandle.Complete();
		this._zoneJobScheduled = false;
		base.StopAllCoroutines();
		base.enabled = false;
		this.UnsubscribeChunkEvents();
		if (rootsOut != null)
		{
			for (int i = 0; i < this._allNpcs.Count; i++)
			{
				NPCZoneManager.NPCState npcstate = this._allNpcs[i];
				if (((npcstate != null) ? npcstate.npc : null) != null)
				{
					rootsOut.Add(npcstate.npc);
				}
			}
		}
		this._allNpcs.Clear();
		this._chunkNpcs.Clear();
		this._chunksWithNpcs.Clear();
		this._chunkSpawnPoints.Clear();
		this._spawnPointToChunk.Clear();
		this._chunkInterestPoints.Clear();
		this._scenePOIs.Clear();
		this._pendingSpawns.Clear();
		this._recentSpawns.Clear();
		this._spawnQueue.Clear();
		this._chunksQueued.Clear();
		this._currentNpcCount = 0;
		this._activeNpcCount = 0;
		if (this.enableDebugLogs)
		{
			Debug.Log("[NPCZoneManager] Fast teardown avviato: NPC raccolti per distruzione in blocco, nessun ritorno al pool.");
		}
	}

	// Token: 0x0400072D RID: 1837
	private static readonly ProfilerMarker CacheSpawnPointsMarker = new ProfilerMarker("NPCZoneManager.CacheSpawnPoints");

	// Token: 0x0400072E RID: 1838
	private static readonly ProfilerMarker CacheInterestPointsMarker = new ProfilerMarker("NPCZoneManager.CacheInterestPoints");

	// Token: 0x04000730 RID: 1840
	[Header("Spawn Config (centralized)")]
	[Tooltip("ScriptableObject con tutti i parametri di spawn/despawn. Auto-caricato da Resources/NPCSpawnConfig.asset se null. Sovrascrive i campi locali.")]
	[SerializeField]
	private NPCSpawnConfig spawnConfig;

	// Token: 0x04000731 RID: 1841
	[Header("Zone Radii (fallback if spawnConfig is null)")]
	[SerializeField]
	private float activeZoneRadius = 60f;

	// Token: 0x04000732 RID: 1842
	[SerializeField]
	private float warmupZoneRadius = 120f;

	// Token: 0x04000733 RID: 1843
	[Header("Spawn Settings")]
	[SerializeField]
	private bool useRuntimeSpawner = true;

	// Token: 0x04000734 RID: 1844
	[SerializeField]
	private NPCRuntimeSpawner runtimeSpawner;

	// Token: 0x04000735 RID: 1845
	[SerializeField]
	private GameObject[] legacyPrefabs;

	// Token: 0x04000736 RID: 1846
	[Header("Static Chunks")]
	[Tooltip("Container dei chunk statici (es. StartingChunks). I figli diretti saranno considerati chunk.")]
	[SerializeField]
	private Transform staticChunksContainer;

	// Token: 0x04000737 RID: 1847
	[Header("NPC Density (fallback)")]
	[SerializeField]
	private int maxTotalNpc = 100;

	// Token: 0x04000738 RID: 1848
	[Header("Performance - Jobs")]
	[Tooltip("Usa Jobs per calcoli paralleli (richiede Burst package)")]
	[SerializeField]
	private bool useJobs = true;

	// Token: 0x04000739 RID: 1849
	[Tooltip("NPC da spawnare per frame (fallback)")]
	[SerializeField]
	private int spawnsPerFrame = 2;

	// Token: 0x0400073A RID: 1850
	[Header("Update Intervals (fallback)")]
	[SerializeField]
	private float zoneCheckInterval = 0.2f;

	// Token: 0x0400073B RID: 1851
	[SerializeField]
	private float positionSyncInterval = 0.1f;

	// Token: 0x0400073C RID: 1852
	[Header("NPC Configuration")]
	[SerializeField]
	private float npcSpeed = 3.5f;

	// Token: 0x0400073D RID: 1853
	[SerializeField]
	private float npcRotationSpeed = 360f;

	// Token: 0x0400073E RID: 1854
	[SerializeField]
	private float npcRadius = 0.5f;

	// Token: 0x0400073F RID: 1855
	[SerializeField]
	private float npcHeight = 2f;

	// Token: 0x04000740 RID: 1856
	[SerializeField]
	private bool enableLocalAvoidance = true;

	// Token: 0x04000741 RID: 1857
	[Header("NPC Component Configuration")]
	[Tooltip("Configurazione centralizzata dei componenti NPC. Se assegnata, sovrascrive le impostazioni individuali.")]
	[SerializeField]
	private NPCComponentConfig npcComponentConfig;

	// Token: 0x04000742 RID: 1858
	[Header("Animation (Legacy - usa NPCComponentConfig)")]
	[SerializeField]
	private RuntimeAnimatorController animatorController;

	// Token: 0x04000743 RID: 1859
	[Header("References")]
	[SerializeField]
	private Transform player;

	// Token: 0x04000744 RID: 1860
	[Header("Debug")]
	[SerializeField]
	private bool enableDebugLogs;

	// Token: 0x04000745 RID: 1861
	[SerializeField]
	private bool drawDebugGizmos = true;

	// Token: 0x04000746 RID: 1862
	private Dictionary<ChunkInstance, List<NPCZoneManager.NPCState>> _chunkNpcs = new Dictionary<ChunkInstance, List<NPCZoneManager.NPCState>>();

	// Token: 0x04000747 RID: 1863
	private Dictionary<ChunkInstance, List<NPCInterestPoint>> _chunkInterestPoints = new Dictionary<ChunkInstance, List<NPCInterestPoint>>();

	// Token: 0x04000748 RID: 1864
	private readonly List<ChunkInstance> _interestPointChunkScratch = new List<ChunkInstance>(8);

	// Token: 0x04000749 RID: 1865
	private Dictionary<ChunkInstance, List<NPCSpawnPoint>> _chunkSpawnPoints = new Dictionary<ChunkInstance, List<NPCSpawnPoint>>();

	// Token: 0x0400074A RID: 1866
	private Dictionary<NPCSpawnPoint, ChunkInstance> _spawnPointToChunk = new Dictionary<NPCSpawnPoint, ChunkInstance>(256);

	// Token: 0x0400074B RID: 1867
	private readonly List<GameObject> _pointRootScratch = new List<GameObject>(8);

	// Token: 0x0400074C RID: 1868
	private readonly List<NPCSpawnPoint> _spawnPointComponentScratch = new List<NPCSpawnPoint>(16);

	// Token: 0x0400074D RID: 1869
	private readonly HashSet<NPCSpawnPoint> _spawnPointDedupScratch = new HashSet<NPCSpawnPoint>();

	// Token: 0x0400074E RID: 1870
	private readonly List<NPCInterestPoint> _interestPointComponentScratch = new List<NPCInterestPoint>(16);

	// Token: 0x0400074F RID: 1871
	private readonly HashSet<NPCInterestPoint> _interestPointDedupScratch = new HashSet<NPCInterestPoint>();

	// Token: 0x04000750 RID: 1872
	private List<NPCZoneManager.NPCState> _allNpcs = new List<NPCZoneManager.NPCState>();

	// Token: 0x04000751 RID: 1873
	private HashSet<ChunkInstance> _chunksWithNpcs = new HashSet<ChunkInstance>();

	// Token: 0x04000752 RID: 1874
	private Queue<NPCZoneManager.ChunkState> _spawnQueue = new Queue<NPCZoneManager.ChunkState>();

	// Token: 0x04000753 RID: 1875
	private HashSet<ChunkInstance> _chunksQueued = new HashSet<ChunkInstance>();

	// Token: 0x04000754 RID: 1876
	private List<ChunkInstance> _staticChunks = new List<ChunkInstance>();

	// Token: 0x04000755 RID: 1877
	private Dictionary<ChunkInstance, int> _chunkIdMap = new Dictionary<ChunkInstance, int>();

	// Token: 0x04000756 RID: 1878
	private int _currentNpcCount;

	// Token: 0x04000757 RID: 1879
	private int _activeNpcCount;

	// Token: 0x04000758 RID: 1880
	private int _nextNpcId;

	// Token: 0x04000759 RID: 1881
	private int _nextChunkId;

	// Token: 0x0400075A RID: 1882
	private float _lastZoneCheck;

	// Token: 0x0400075B RID: 1883
	private float _lastPositionSync;

	// Token: 0x0400075C RID: 1884
	private NativeArray<NPCJobData> _npcJobData;

	// Token: 0x0400075D RID: 1885
	private NativeArray<NPCZoneResult> _npcZoneResults;

	// Token: 0x0400075E RID: 1886
	private NativeArray<float3> _npcPositions;

	// Token: 0x0400075F RID: 1887
	private NativeArray<global::ChunkJobData> _chunkJobData;

	// Token: 0x04000760 RID: 1888
	private NativeArray<ChunkZoneResult> _chunkZoneResults;

	// Token: 0x04000761 RID: 1889
	private JobHandle _currentZoneJobHandle;

	// Token: 0x04000762 RID: 1890
	private bool _zoneJobScheduled;

	// Token: 0x04000763 RID: 1891
	private int _scheduledNpcCount;

	// Token: 0x04000764 RID: 1892
	private float _activeRadiusSqr;

	// Token: 0x04000765 RID: 1893
	private float _warmupRadiusSqr;

	// Token: 0x04000766 RID: 1894
	private float _nearRadiusSqr;

	// Token: 0x04000767 RID: 1895
	private float _ambientRadiusSqr;

	// Token: 0x04000768 RID: 1896
	private float _despawnRadiusSqr;

	// Token: 0x04000769 RID: 1897
	private float _preferredDistance;

	// Token: 0x0400076A RID: 1898
	private float _ambientPreferredDistance;

	// Token: 0x0400076B RID: 1899
	private float _minDistanceFromPlayerSqr;

	// Token: 0x0400076C RID: 1900
	private float _initialPoiSafeRadiusSqr;

	// Token: 0x0400076D RID: 1901
	private float _initialPoiSafetyDuration;

	// Token: 0x0400076E RID: 1902
	private float _initialPoiSafetyEndsAtRealtime;

	// Token: 0x0400076F RID: 1903
	private Vector3 _poiEntryPosition;

	// Token: 0x04000770 RID: 1904
	private bool _hasPoiEntryExclusion;

	// Token: 0x04000771 RID: 1905
	private float _distanceWeight;

	// Token: 0x04000772 RID: 1906
	private float _randomWeight;

	// Token: 0x04000773 RID: 1907
	private float _recentSpawnCooldown;

	// Token: 0x04000774 RID: 1908
	private float _minNpcSpawnSpacingSqr;

	// Token: 0x04000775 RID: 1909
	private float _maxSpawnNavmeshHorizontalCorrection;

	// Token: 0x04000776 RID: 1910
	private float _maxSpawnNavmeshVerticalCorrection;

	// Token: 0x04000777 RID: 1911
	private int _desiredActiveNpc;

	// Token: 0x04000778 RID: 1912
	private int _desiredAmbientNpc;

	// Token: 0x04000779 RID: 1913
	private int _maxTotalNpc;

	// Token: 0x0400077A RID: 1914
	private float _densityMultiplier = 1f;

	// Token: 0x0400077B RID: 1915
	private float _timeOfDayMultiplier = 1f;

	// Token: 0x0400077C RID: 1916
	private float _currentPopulationHour = -1f;

	// Token: 0x0400077D RID: 1917
	private float _nextPopulationTimeUpdate;

	// Token: 0x0400077E RID: 1918
	private float _populationTimeUpdateInterval;

	// Token: 0x0400077F RID: 1919
	private AzureCoreSystem _azureSystem;

	// Token: 0x04000780 RID: 1920
	private bool _allowAmbient;

	// Token: 0x04000781 RID: 1921
	private int _spawnsPerFrame;

	// Token: 0x04000782 RID: 1922
	private int _despawnsPerFrame;

	// Token: 0x04000783 RID: 1923
	private int _maxSpawnsPerRebalance;

	// Token: 0x04000784 RID: 1924
	private int _retirementsPerRebalance;

	// Token: 0x04000785 RID: 1925
	private float _rebalanceInterval;

	// Token: 0x04000786 RID: 1926
	private bool _verboseDebug;

	// Token: 0x04000787 RID: 1927
	private List<NPCZoneManager.CandidateSpawnPoint> _candidateBuffer = new List<NPCZoneManager.CandidateSpawnPoint>(128);

	// Token: 0x04000788 RID: 1928
	private Queue<NPCSpawnPoint> _pendingSpawns = new Queue<NPCSpawnPoint>(32);

	// Token: 0x04000789 RID: 1929
	private Dictionary<NPCSpawnPoint, float> _recentSpawns = new Dictionary<NPCSpawnPoint, float>(32);

	// Token: 0x0400078A RID: 1930
	private List<NPCSpawnPoint> _recentSpawnsKeys = new List<NPCSpawnPoint>(32);

	// Token: 0x0400078B RID: 1931
	private static readonly RaycastHit[] _spawnOcclusionHits = new RaycastHit[24];

	// Token: 0x0400078C RID: 1932
	private float _lastRebalance;

	// Token: 0x0400078D RID: 1933
	private Vector3 _lastPlayerPos;

	// Token: 0x0400078E RID: 1934
	private Vector3 _playerMoveDir;

	// Token: 0x0400078F RID: 1935
	private float _playerSpeed;

	// Token: 0x04000790 RID: 1936
	private bool _initialGenerationGateOpen;

	// Token: 0x04000791 RID: 1937
	private bool _initialPopulationSatisfied;

	// Token: 0x04000792 RID: 1938
	private bool _initialPopulationComplete;

	// Token: 0x04000793 RID: 1939
	private Object _preparationOwner;

	// Token: 0x04000794 RID: 1940
	private bool _preparationFocusOwned;

	// Token: 0x04000795 RID: 1941
	private Vector3 _preparationPosition;

	// Token: 0x04000796 RID: 1942
	private bool _preparationRebalanced;

	// Token: 0x04000797 RID: 1943
	private bool _preparationPopulationSatisfied;

	// Token: 0x04000798 RID: 1944
	private bool _preparationZoneChecked;

	// Token: 0x04000799 RID: 1945
	private int _preparationRevision;

	// Token: 0x0400079A RID: 1946
	private int _scheduledPreparationRevision;

	// Token: 0x040007A0 RID: 1952
	private List<NPCInterestPoint> _scenePOIs = new List<NPCInterestPoint>();

	// Token: 0x040007A1 RID: 1953
	private static readonly Comparison<NPCZoneManager.CandidateSpawnPoint> CandidateScoreCompareDesc = (NPCZoneManager.CandidateSpawnPoint a, NPCZoneManager.CandidateSpawnPoint b) => b.score.CompareTo(a.score);

	// Token: 0x020000D1 RID: 209
	private class NPCState
	{
		// Token: 0x040007A2 RID: 1954
		public int id;

		// Token: 0x040007A3 RID: 1955
		public GameObject npc;

		// Token: 0x040007A4 RID: 1956
		public ChunkInstance chunk;

		// Token: 0x040007A5 RID: 1957
		public FollowerEntity follower;

		// Token: 0x040007A6 RID: 1958
		public Animator animator;

		// Token: 0x040007A7 RID: 1959
		public NPCWanderController wanderController;

		// Token: 0x040007A8 RID: 1960
		public AStarNPCController controller;

		// Token: 0x040007A9 RID: 1961
		public NPCSenses senses;

		// Token: 0x040007AA RID: 1962
		public NPCStateMachine stateMachine;

		// Token: 0x040007AB RID: 1963
		public SimpleAStarWander simpleWander;

		// Token: 0x040007AC RID: 1964
		public bool isActive;

		// Token: 0x040007AD RID: 1965
		public bool preparationActivationPending;

		// Token: 0x040007AE RID: 1966
		public bool restorePreparationSpawnSpeed;

		// Token: 0x040007AF RID: 1967
		public float preparationSpawnSpeed;

		// Token: 0x040007B0 RID: 1968
		public float maxVerticalDistanceFromPlayer;
	}

	// Token: 0x020000D2 RID: 210
	private class ChunkState
	{
		// Token: 0x040007B1 RID: 1969
		public int id;

		// Token: 0x040007B2 RID: 1970
		public ChunkInstance chunk;

		// Token: 0x040007B3 RID: 1971
		public List<NPCSpawnPoint> spawnPoints;

		// Token: 0x040007B4 RID: 1972
		public int spawnedCount;

		// Token: 0x040007B5 RID: 1973
		public int targetCount;
	}

	// Token: 0x020000D3 RID: 211
	private struct CandidateSpawnPoint
	{
		// Token: 0x040007B6 RID: 1974
		public NPCSpawnPoint sp;

		// Token: 0x040007B7 RID: 1975
		public float sqrDist;

		// Token: 0x040007B8 RID: 1976
		public float score;
	}

	// Token: 0x020000D4 RID: 212
	// (Invoke) Token: 0x06000898 RID: 2200
	public delegate void NPCEvent(GameObject npc, ChunkInstance chunk);
}
