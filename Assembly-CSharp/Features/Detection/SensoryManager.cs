using System;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Missions;
using Features.NPC;
using Features.PlayerEmotions;
using Features.Rank;
using Features.RunModes;
using Features.Suspicion;
using Features.TimeFreeze;
using Features.WardrobeCombos;
using ProjectExhibitionist.Core.SceneManagement;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.AzureSky;
using UnityEngine.SceneManagement;

namespace Features.Detection
{
	// Token: 0x020012E3 RID: 4835
	[DefaultExecutionOrder(-100)]
	public class SensoryManager : MonoBehaviour
	{
		// Token: 0x170017EC RID: 6124
		// (get) Token: 0x060095DD RID: 38365 RVA: 0x003D829A File Offset: 0x003D649A
		// (set) Token: 0x060095DE RID: 38366 RVA: 0x003D82A1 File Offset: 0x003D64A1
		public static SensoryManager Instance { get; private set; }

		// Token: 0x1400010B RID: 267
		// (add) Token: 0x060095DF RID: 38367 RVA: 0x003D82AC File Offset: 0x003D64AC
		// (remove) Token: 0x060095E0 RID: 38368 RVA: 0x003D82E0 File Offset: 0x003D64E0
		public static event Action<NPCSenses, int> OnPassiveReactionStarted;

		// Token: 0x060095E1 RID: 38369 RVA: 0x003D8314 File Offset: 0x003D6514
		private void Awake()
		{
			if (SensoryManager.Instance != null && SensoryManager.Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			SensoryManager.Instance = this;
			PlayerRegistry.PlayerChanged += this.BindRegisteredPlayer;
			if (PlayerRegistry.Player != null)
			{
				this.BindRegisteredPlayer(PlayerRegistry.Player);
			}
			foreach (NPCSenses npcsenses in Object.FindObjectsOfType<NPCSenses>())
			{
				if (npcsenses.isActiveAndEnabled)
				{
					this.RegisterNPC(npcsenses);
				}
			}
		}

		// Token: 0x060095E2 RID: 38370 RVA: 0x003D839D File Offset: 0x003D659D
		private void OnDestroy()
		{
			PlayerRegistry.PlayerChanged -= this.BindRegisteredPlayer;
			if (SensoryManager.Instance == this)
			{
				SensoryManager.Instance = null;
			}
			this._pauseReasons.Clear();
			this.DisposeDetectionBuffers();
		}

		// Token: 0x060095E3 RID: 38371 RVA: 0x003D83D4 File Offset: 0x003D65D4
		private void EnsureDetectionBufferCapacity(int requiredCount)
		{
			if (requiredCount <= this._detectionBufferCapacity && this._npcPositions.IsCreated && this._npcForward.IsCreated && this._npcViewRadii.IsCreated && this._npcViewAngles.IsCreated && this._npcAttentionMultipliers.IsCreated && this._results.IsCreated)
			{
				return;
			}
			this.DisposeDetectionBuffers();
			this._detectionBufferCapacity = Mathf.NextPowerOfTwo(Mathf.Max(1, requiredCount));
			this._npcPositions = new NativeArray<Vector3>(this._detectionBufferCapacity, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this._npcForward = new NativeArray<Vector3>(this._detectionBufferCapacity, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this._npcViewRadii = new NativeArray<float>(this._detectionBufferCapacity, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this._npcViewAngles = new NativeArray<float>(this._detectionBufferCapacity, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this._npcAttentionMultipliers = new NativeArray<float>(this._detectionBufferCapacity, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			this._results = new NativeArray<int>(this._detectionBufferCapacity, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
		}

		// Token: 0x060095E4 RID: 38372 RVA: 0x003D84C4 File Offset: 0x003D66C4
		private void DisposeDetectionBuffers()
		{
			if (this._npcPositions.IsCreated)
			{
				this._npcPositions.Dispose();
			}
			if (this._npcForward.IsCreated)
			{
				this._npcForward.Dispose();
			}
			if (this._npcViewRadii.IsCreated)
			{
				this._npcViewRadii.Dispose();
			}
			if (this._npcViewAngles.IsCreated)
			{
				this._npcViewAngles.Dispose();
			}
			if (this._npcAttentionMultipliers.IsCreated)
			{
				this._npcAttentionMultipliers.Dispose();
			}
			if (this._results.IsCreated)
			{
				this._results.Dispose();
			}
			this._detectionBufferCapacity = 0;
		}

		// Token: 0x060095E5 RID: 38373 RVA: 0x003D8568 File Offset: 0x003D6768
		public void RegisterNPC(NPCSenses npc)
		{
			if (!this._registeredNPCs.Contains(npc))
			{
				if (DetectionInteractionGate.IsGameOver || DetectionTransitionGate.IsBlocked)
				{
					SensoryManager.ClearTransitionContact(npc);
				}
				this._registeredNPCs.Add(npc);
				if (this.DebugMode)
				{
					Debug.Log(string.Format("[SensoryManager] Registered NPC: {0}. Total: {1}", npc.name, this._registeredNPCs.Count));
				}
			}
		}

		// Token: 0x060095E6 RID: 38374 RVA: 0x003D85D0 File Offset: 0x003D67D0
		public void UnregisterNPC(NPCSenses npc)
		{
			if (this._registeredNPCs.Contains(npc))
			{
				this._registeredNPCs.Remove(npc);
				if (this.DebugMode)
				{
					Debug.Log(string.Format("[SensoryManager] Unregistered NPC: {0}. Total: {1}", npc.name, this._registeredNPCs.Count));
				}
			}
		}

		// Token: 0x060095E7 RID: 38375 RVA: 0x003D8628 File Offset: 0x003D6828
		public void SetPlayer(StealthActor player)
		{
			this._player = ((player != null) ? player : null);
			this._collarStyleOwner = null;
			this._playerStyles = null;
			if (this.DebugMode)
			{
				Debug.Log("[SensoryManager] Player set: " + ((this._player != null) ? this._player.name : "<none>"));
			}
		}

		// Token: 0x060095E8 RID: 38376 RVA: 0x003D868D File Offset: 0x003D688D
		private void BindRegisteredPlayer(Transform player)
		{
			this.SetPlayer((player != null) ? player.GetComponentInChildren<StealthActor>(true) : null);
		}

		// Token: 0x060095E9 RID: 38377 RVA: 0x003D86A8 File Offset: 0x003D68A8
		public IReadOnlyList<NPCSenses> GetRegisteredNPCs()
		{
			return this._registeredNPCs;
		}

		// Token: 0x060095EA RID: 38378 RVA: 0x003D86B0 File Offset: 0x003D68B0
		public StealthActor GetPlayer()
		{
			if (this._player == null)
			{
				this._player = null;
				if (PlayerRegistry.Player != null)
				{
					this.BindRegisteredPlayer(PlayerRegistry.Player);
				}
			}
			return this._player;
		}

		// Token: 0x060095EB RID: 38379 RVA: 0x003D86E5 File Offset: 0x003D68E5
		public void SetPaused(bool paused)
		{
			this._isPaused = paused;
			if (this.DebugMode)
			{
				Debug.Log("[SensoryManager] Detection processing " + (paused ? "PAUSED" : "RESUMED"));
			}
		}

		// Token: 0x170017ED RID: 6125
		// (get) Token: 0x060095EC RID: 38380 RVA: 0x003D8714 File Offset: 0x003D6914
		public bool IsPaused
		{
			get
			{
				return DetectionInteractionGate.IsGameOver || this._isPaused || this._pauseReasons.Count > 0 || TimeFreezeController.IsAnyActive;
			}
		}

		// Token: 0x060095ED RID: 38381 RVA: 0x003D873A File Offset: 0x003D693A
		public void PushPause(string reason)
		{
			if (string.IsNullOrEmpty(reason))
			{
				return;
			}
			if (this._pauseReasons.Add(reason) && this.DebugMode)
			{
				Debug.Log("[SensoryManager] Detection pause acquired: " + reason);
			}
		}

		// Token: 0x060095EE RID: 38382 RVA: 0x003D876B File Offset: 0x003D696B
		public void PopPause(string reason)
		{
			if (string.IsNullOrEmpty(reason))
			{
				return;
			}
			if (this._pauseReasons.Remove(reason) && this.DebugMode)
			{
				Debug.Log("[SensoryManager] Detection pause released: " + reason);
			}
		}

		// Token: 0x060095EF RID: 38383 RVA: 0x003D879C File Offset: 0x003D699C
		public void SetRunModeDetectionEnabled(bool enabled)
		{
			if (this._runModeDetectionEnabled == enabled)
			{
				return;
			}
			this._runModeDetectionEnabled = enabled;
			if (enabled)
			{
				return;
			}
			for (int i = 0; i < this._registeredNPCs.Count; i++)
			{
				NPCSenses npcsenses = this._registeredNPCs[i];
				SensoryManager.ClearTransitionContact(npcsenses);
				if (!(npcsenses == null))
				{
					npcsenses.IsPassiveReacting = false;
					npcsenses.PassiveReactionType = 0;
					npcsenses.PassiveReactionEntry = null;
				}
			}
			DetectionEmotionSource instance = DetectionEmotionSource.Instance;
			if (instance == null)
			{
				return;
			}
			instance.StopPreDetection();
		}

		// Token: 0x170017EE RID: 6126
		// (get) Token: 0x060095F0 RID: 38384 RVA: 0x003D8814 File Offset: 0x003D6A14
		public bool IsRunModeDetectionEnabled
		{
			get
			{
				return this._runModeDetectionEnabled;
			}
		}

		// Token: 0x170017EF RID: 6127
		// (get) Token: 0x060095F1 RID: 38385 RVA: 0x003D881C File Offset: 0x003D6A1C
		public WeatherDetectionModifiers CurrentWeatherDetection
		{
			get
			{
				return this._weatherDetection;
			}
		}

		// Token: 0x060095F2 RID: 38386 RVA: 0x003D8824 File Offset: 0x003D6A24
		private float GetCollarAttentionGainMultiplier()
		{
			if (this._collarStyleOwner != this._player || this._playerStyles == null)
			{
				this._collarStyleOwner = this._player;
				this._playerStyles = ((this._player != null) ? this._player.GetComponentInChildren<MCStyleCustomizationService>(true) : null);
			}
			return CollarDetectionPolicy.GetAttentionGainMultiplier(this._playerStyles);
		}

		// Token: 0x060095F3 RID: 38387 RVA: 0x003D888C File Offset: 0x003D6A8C
		private WeatherDetectionModifiers SampleWeatherDetection()
		{
			Scene activeScene = SceneManager.GetActiveScene();
			if (!activeScene.IsValid() || !DynamicWorldEnvironmentController.IsSupportedWorldScene(activeScene.name))
			{
				return WeatherDetectionModifiers.Clear;
			}
			if (this._weatherAzure == null)
			{
				if (Time.unscaledTime < this._nextWeatherBindAttempt)
				{
					return WeatherDetectionModifiers.Clear;
				}
				this._nextWeatherBindAttempt = Time.unscaledTime + 2f;
				foreach (AzureCoreSystem azureCoreSystem in Object.FindObjectsOfType<AzureCoreSystem>(true))
				{
					if (azureCoreSystem != null && DynamicWorldEnvironmentController.IsEnvironmentReady(azureCoreSystem))
					{
						this._weatherAzure = azureCoreSystem;
						break;
					}
				}
			}
			if (this._weatherAzure == null || !DynamicWorldEnvironmentController.IsEnvironmentReady(this._weatherAzure))
			{
				return WeatherDetectionModifiers.Clear;
			}
			AzureWeatherSystem weatherSystem = this._weatherAzure.weatherSystem;
			if (weatherSystem == null)
			{
				return WeatherDetectionModifiers.Clear;
			}
			return WeatherDetectionPolicy.Evaluate((weatherSystem.currentWeatherPreset != null) ? weatherSystem.currentWeatherPreset.name : string.Empty, (weatherSystem.targetWeatherPreset != null) ? weatherSystem.targetWeatherPreset.name : string.Empty, weatherSystem.isWeatherChanging, weatherSystem.weatherTransitionProgress, Shader.GetGlobalFloat(SensoryManager.DenseFogBlend));
		}

		// Token: 0x060095F4 RID: 38388 RVA: 0x003D89B8 File Offset: 0x003D6BB8
		public void PrewarmDetectionJob()
		{
			if (this.GetPlayer() == null || this._registeredNPCs.Count == 0 || this.GlobalConfig == null)
			{
				return;
			}
			this._weatherDetection = this.SampleWeatherDetection();
			this.EvaluateFieldOfView(this._registeredNPCs.Count, this._player.Position);
		}

		// Token: 0x060095F5 RID: 38389 RVA: 0x003D8A18 File Offset: 0x003D6C18
		private void EvaluateFieldOfView(int count, Vector3 playerPos)
		{
			this.EnsureDetectionBufferCapacity(count);
			for (int i = 0; i < count; i++)
			{
				NPCSenses npcsenses = this._registeredNPCs[i];
				if (npcsenses == null)
				{
					this._npcPositions[i] = Vector3.zero;
					this._npcForward[i] = Vector3.forward;
					this._npcViewRadii[i] = 0f;
					this._npcViewAngles[i] = 1f;
					this._npcAttentionMultipliers[i] = 0f;
				}
				else
				{
					this._npcPositions[i] = npcsenses.EyePosition;
					this._npcForward[i] = npcsenses.ViewDirection;
					this._npcViewRadii[i] = WeatherDetectionPolicy.ApplyViewRadius(NpcVisionDifficultyPolicy.ApplyRadius(npcsenses.EffectiveViewRadius), this._weatherDetection, npcsenses.HasLineOfSight);
					this._npcViewAngles[i] = Mathf.Cos(npcsenses.GetViewAngleForTarget(playerPos) / 2f * 0.017453292f);
					this._npcAttentionMultipliers[i] = npcsenses.AttentionMultiplier;
				}
			}
			new SensoryManager.DetectionJob
			{
				PlayerPosition = playerPos,
				NPCPositions = this._npcPositions,
				NPCForwards = this._npcForward,
				NPCViewRadii = this._npcViewRadii,
				NPCViewAngles = this._npcViewAngles,
				Results = this._results
			}.Schedule(count, 32, default(JobHandle)).Complete();
		}

		// Token: 0x060095F6 RID: 38390 RVA: 0x003D8B9C File Offset: 0x003D6D9C
		private void Update()
		{
			if (DetectionInteractionGate.IsGameOver || DetectionTransitionGate.IsBlocked)
			{
				if (!this._transitionContactsCleared)
				{
					for (int i = 0; i < this._registeredNPCs.Count; i++)
					{
						SensoryManager.ClearTransitionContact(this._registeredNPCs[i]);
					}
					DetectionEmotionSource instance = DetectionEmotionSource.Instance;
					if (instance != null)
					{
						instance.StopPreDetection();
					}
					this._transitionContactsCleared = true;
				}
				return;
			}
			this._transitionContactsCleared = false;
			if (this.IsPaused || NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f || !this._runModeDetectionEnabled)
			{
				return;
			}
			if (this.GetPlayer() == null || this._registeredNPCs.Count == 0 || this.GlobalConfig == null)
			{
				if (this.DebugMode && Time.frameCount % 60 == 0)
				{
					Debug.LogWarning("[SensoryManager] Missing dependencies: Player, NPCs, or Config.");
				}
				return;
			}
			int count = this._registeredNPCs.Count;
			Vector3 position = this._player.Position;
			float finalVisibility = this._player.FinalVisibility;
			float finalNoiseRadius = this._player.FinalNoiseRadius;
			this._weatherDetection = this.SampleWeatherDetection();
			float detectionAttentionMultiplier = PlayerExposureState.DetectionAttentionMultiplier;
			bool flag = detectionAttentionMultiplier > 0.0001f;
			bool progressionTuningActive = RankDetectionPolicy.ProgressionTuningActive;
			float garmentAttentionGainMultiplier = PlayerExposureState.GarmentAttentionGainMultiplier;
			bool flag2 = progressionTuningActive && PlayerExposureState.IsDirectedFlashActive;
			float collarAttentionGainMultiplier = this.GetCollarAttentionGainMultiplier();
			WardrobeCombo activeCombos = WardrobeComboRuntime.ActiveCombos;
			bool flag3 = (activeCombos & WardrobeCombo.Ghost) > WardrobeCombo.None;
			bool isFullyHidden = PlayerStealthBridge.IsFullyHidden;
			this.EvaluateFieldOfView(count, position);
			int num = 0;
			bool flag4 = false;
			float num2 = 0f;
			DetectionSource detectionSource = DetectionSource.None;
			for (int j = 0; j < count; j++)
			{
				NPCSenses npcsenses = this._registeredNPCs[j];
				if (!(npcsenses == null) && (!npcsenses.IsInCooldown || flag3))
				{
					NPCRagdollRecovery npcragdollRecovery;
					if (npcsenses.TryGetComponent<NPCRagdollRecovery>(out npcragdollRecovery) && npcragdollRecovery.IsRecovering)
					{
						npcsenses.HasLineOfSight = false;
						npcsenses.HasAudioContact = false;
						npcsenses.CurrentDetectionSource = DetectionSource.None;
					}
					else
					{
						int num3 = this._results[j];
						bool flag5 = false;
						bool flag6 = false;
						float sqrMagnitude = (this._npcPositions[j] - position).sqrMagnitude;
						float num4 = finalNoiseRadius;
						if (npcsenses.HasAudioContact)
						{
							num4 *= this.AudioHysteresis;
						}
						if (sqrMagnitude < num4 * num4)
						{
							if (this.AudioRequiresLineOfSight)
							{
								LayerMask layerMask = ((this.AudioObstacles != 0) ? this.AudioObstacles : this.GlobalConfig.VisionObstacles);
								RaycastHit raycastHit;
								if (!DetectionLineOfSight.IsBlocked(this._npcPositions[j], position, layerMask, npcsenses.transform, this._player.transform, out raycastHit, null))
								{
									flag6 = true;
									if (this.DebugMode)
									{
										Debug.Log(string.Format("[SensoryManager] {0} HEARS Player (DistSq: {1}, NoiseSq: {2})", npcsenses.name, sqrMagnitude, finalNoiseRadius * finalNoiseRadius));
									}
								}
								else if ((this.DebugMode || npcsenses.DebugLog) && raycastHit.collider != null)
								{
									Debug.Log(string.Format("[SensoryManager] {0} audio BLOCKED by '{1}' (layer={2}, dist={3:F1}m)", new object[]
									{
										npcsenses.name,
										raycastHit.collider.name,
										raycastHit.collider.gameObject.layer,
										raycastHit.distance
									}));
								}
							}
							else
							{
								flag6 = true;
								if (this.DebugMode)
								{
									Debug.Log(string.Format("[SensoryManager] {0} HEARS Player (DistSq: {1}, NoiseSq: {2})", npcsenses.name, sqrMagnitude, finalNoiseRadius * finalNoiseRadius));
								}
							}
						}
						if (num3 == 2 && finalVisibility > 0.01f)
						{
							if (num < this.MaxRaycastsPerFrame)
							{
								float num5 = Vector3.Distance(position, this._npcPositions[j]);
								RaycastHit raycastHit2;
								if (!DetectionLineOfSight.IsBlocked(this._npcPositions[j], position, this.GlobalConfig.VisionObstacles, npcsenses.transform, this._player.transform, out raycastHit2, this._player.VisibilityInteractionOccluder))
								{
									flag5 = true;
									num++;
									if (this.DebugMode)
									{
										Debug.Log(string.Format("[SensoryManager] {0} SEES Player! (FOV result: {1}, Vis: {2}, Dist: {3})", new object[] { npcsenses.name, num3, finalVisibility, num5 }));
									}
								}
								else if ((this.DebugMode || npcsenses.DebugLog) && raycastHit2.collider != null)
								{
									GameObject gameObject = raycastHit2.collider.gameObject;
									Debug.Log(string.Concat(new string[]
									{
										"[SensoryManager] ",
										npcsenses.name,
										" vision BLOCKED by '",
										gameObject.name,
										"' ",
										string.Format("(layer={0}:{1}, ", gameObject.layer, LayerMask.LayerToName(gameObject.layer)),
										string.Format("dist={0:F2}m). VisionObstacles mask={1}.", raycastHit2.distance, this.GlobalConfig.VisionObstacles.value)
									}), gameObject);
								}
							}
						}
						else if (this.DebugMode && num3 != 2 && sqrMagnitude < this._npcViewRadii[j] * this._npcViewRadii[j])
						{
							Vector3 normalized = (position - this._npcPositions[j]).normalized;
							float num6 = Mathf.Acos(Vector3.Dot(this._npcForward[j], normalized)) * 57.29578f;
							Debug.Log(string.Format("[SensoryManager] {0} Player in range but NOT in FOV. Angle: {1:F1}°, Required: {2:F1}°, NPC Forward: {3}, ToPlayer: {4}", new object[]
							{
								npcsenses.name,
								num6,
								npcsenses.GetViewAngleForTarget(position) / 2f,
								this._npcForward[j],
								normalized
							}));
						}
						if (flag5 && !isFullyHidden)
						{
							MissionGameplaySignals.NotifyPlayerSeen(npcsenses);
						}
						bool flag7 = WardrobeComboRules.ShouldFleeOnSight(activeCombos, flag5, isFullyHidden);
						npcsenses.UpdateGhostVisualContact(flag7);
						if (flag3 || npcsenses.IsGhostFleeing)
						{
							if (flag7)
							{
								GhostPerkFleeController.BeginOrRefresh(npcsenses, position);
							}
							npcsenses.SuppressDetectionForGhost();
						}
						else
						{
							if (!flag)
							{
								flag5 = false;
								flag6 = false;
							}
							DetectionActorType detectionActorType = ((activeCombos != WardrobeCombo.None) ? SensoryManager.ResolveActorType(npcsenses) : DetectionActorType.Generic);
							if (WardrobeComboRules.ShouldIgnore(activeCombos, detectionActorType))
							{
								flag5 = false;
								flag6 = false;
							}
							bool flag8 = this.DisableFinalDetection || !RunModeService.EncountersEnabled || npcsenses.BlocksFinalDetectionWhileSeated || WardrobeComboRules.ShouldDisableFinalDetection(activeCombos, detectionActorType, (npcsenses.transform.position - this._player.transform.position).sqrMagnitude);
							npcsenses.HasLineOfSight = flag5;
							npcsenses.HasAudioContact = flag6;
							float num9;
							if (npcsenses.HasLineOfSight)
							{
								npcsenses.CurrentDetectionSource = DetectionSource.Visual;
								float num7 = 1f / Mathf.Max(this.GlobalConfig.TimeToDetect, 0.1f);
								float num8 = 1f - Mathf.Clamp01(Mathf.Sqrt(sqrMagnitude) / this._npcViewRadii[j]);
								float visualRateMultiplier = WeatherDetectionPolicy.GetVisualRateMultiplier(this._weatherDetection, Mathf.Sqrt(sqrMagnitude));
								bool flag9 = flag2;
								bool hasLineOfSight = npcsenses.HasLineOfSight;
								bool flag10 = progressionTuningActive;
								Vector3 forward = this._player.transform.forward;
								Vector3 vector = this._npcPositions[j] - position;
								RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
								float directedFlashAttentionMultiplier = RankDetectionPolicy.GetDirectedFlashAttentionMultiplier(flag9, hasLineOfSight, flag10, forward, vector, (currentRank != null) ? currentRank.rankId : null);
								num9 = num7 * finalVisibility * (0.5f + num8) * this._npcAttentionMultipliers[j] * this.GlobalConfig.VisualDetectionMultiplier * detectionAttentionMultiplier * visualRateMultiplier * garmentAttentionGainMultiplier * collarAttentionGainMultiplier * directedFlashAttentionMultiplier * NPCSimulationPause.SimulationDeltaTime;
							}
							else if (npcsenses.HasAudioContact)
							{
								npcsenses.CurrentDetectionSource = DetectionSource.Audio;
								float num10 = 1f / Mathf.Max(this.GlobalConfig.TimeToDetect, 0.1f) * this.GlobalConfig.AudioDetectionMultiplier * detectionAttentionMultiplier * garmentAttentionGainMultiplier * collarAttentionGainMultiplier * npcsenses.StaticPhoneAttentionMultiplier;
								if (this.GlobalConfig.AudioScalesWithDistance)
								{
									float num11 = Mathf.Sqrt(sqrMagnitude);
									float num12 = npcsenses.EffectiveViewRadius * 1.5f;
									float num13 = 1f - Mathf.Clamp01(num11 / num12);
									num10 *= 0.3f + 0.7f * num13;
								}
								num9 = num10 * NPCSimulationPause.SimulationDeltaTime;
							}
							else
							{
								npcsenses.CurrentDetectionSource = DetectionSource.None;
								if (npcsenses.IsSearching)
								{
									num9 = 0f;
								}
								else
								{
									num9 = -(1f / Mathf.Max(this.GlobalConfig.CooldownTime, 0.1f)) * NPCSimulationPause.SimulationDeltaTime;
								}
							}
							float num14 = Mathf.Clamp01(npcsenses.CurrentAttention + num9);
							if (ExhibitionEventDirector.Instance != null && ExhibitionEventDirector.Instance.HasPendingDetection(npcsenses))
							{
								num14 = Mathf.Max(num14, npcsenses.CurrentAttention);
							}
							if (flag8 && num14 > 0.98f)
							{
								num14 = 0.98f;
							}
							npcsenses.SetAttentionState(num14);
							if ((this.DebugMode || npcsenses.DebugLog) && (num14 > 0.01f || npcsenses.HasAudioContact || npcsenses.HasLineOfSight))
							{
								float num15 = Mathf.Sqrt(sqrMagnitude);
								Debug.Log(string.Format("[SensoryManager] {0}: Att={1:F3} (Δ{2:+0.0000;-0.0000}), Src={3}, LoS={4}, Audio={5}, Dist={6:F1}m, NoiseRadius={7:F1}m", new object[] { npcsenses.name, num14, num9, npcsenses.CurrentDetectionSource, npcsenses.HasLineOfSight, npcsenses.HasAudioContact, num15, finalNoiseRadius }));
							}
							if (PlayerEmotionController.Instance != null)
							{
								PlayerEmotionController.Instance.OnNPCDetectionProgress(npcsenses, num14);
							}
							if (this.EnablePreDetectionEmotions && num14 > 0.01f && num14 < 1f && (!flag4 || num14 > num2))
							{
								flag4 = true;
								num2 = num14;
								detectionSource = npcsenses.CurrentDetectionSource;
							}
							if (flag8 && !npcsenses.UsesStaticPhoneReaction && num14 >= 0.98f)
							{
								npcsenses.LastKnownPlayerPosition = position;
								if (!npcsenses.IsPassiveReacting && (npcsenses.HasLineOfSight || npcsenses.HasAudioContact))
								{
									npcsenses.IsPassiveReacting = true;
									if (npcsenses.ReactionProfile != null)
									{
										npcsenses.PassiveReactionEntry = npcsenses.ReactionProfile.GetPassiveReactionEntry();
										npcsenses.PassiveReactionType = ((npcsenses.PassiveReactionEntry != null) ? ((npcsenses.PassiveReactionEntry.AnimatorReactionIndex > 0) ? npcsenses.PassiveReactionEntry.AnimatorReactionIndex : ((npcsenses.PassiveReactionEntry.Type == ReactionType.TakeVideo) ? 2 : 1)) : 1);
									}
									else
									{
										npcsenses.PassiveReactionEntry = null;
										npcsenses.PassiveReactionType = ((Random.value > 0.5f) ? 1 : 2);
									}
									Animator animator = npcsenses.GetComponent<Animator>();
									if (animator == null)
									{
										animator = npcsenses.GetComponentInChildren<Animator>();
									}
									if (animator != null)
									{
										animator.SetInteger("ReactionType", npcsenses.PassiveReactionType);
										animator.SetTrigger("ReactionTrigger");
									}
									if (this.DebugMode)
									{
										Debug.Log(string.Format("[SensoryManager] {0} passive reaction started (ReactionType={1}). DisableFinalDetection=ON.", npcsenses.name, npcsenses.PassiveReactionType));
									}
									Action<NPCSenses, int> onPassiveReactionStarted = SensoryManager.OnPassiveReactionStarted;
									if (onPassiveReactionStarted != null)
									{
										onPassiveReactionStarted(npcsenses, npcsenses.PassiveReactionType);
									}
								}
							}
							bool isRunActive = LongWayHomeRuntimeController.IsRunActive;
							if (!flag8 && num14 >= 1f && LongWayHomeRuntimeController.TryHandleFullDetection(npcsenses))
							{
								return;
							}
							float num16 = (isRunActive ? 1f : this.GlobalConfig.AlertThreshold);
							if (!flag8 && num14 >= num16 && !npcsenses.IsDetected)
							{
								npcsenses.LastKnownPlayerPosition = position;
								npcsenses.IsDetected = true;
								if (this.DebugMode)
								{
									Debug.Log("[SensoryManager] " + npcsenses.name + " DETECTED Player! Triggering event.");
								}
								bool flag11 = false;
								SuspicionSource component = npcsenses.GetComponent<SuspicionSource>();
								if (component != null && component.suspicionDelta == 0f)
								{
									flag11 = true;
								}
								if (!flag11 && ExhibitionEventDirector.Instance != null)
								{
									ExhibitionEventDirector.Instance.TriggerDetection(npcsenses);
								}
							}
							if (flag8 && npcsenses.IsPassiveReacting && !npcsenses.HasLineOfSight && !npcsenses.HasAudioContact)
							{
								npcsenses.IsPassiveReacting = false;
								npcsenses.PassiveReactionType = 0;
								npcsenses.PassiveReactionEntry = null;
								Animator animator2 = npcsenses.GetComponent<Animator>();
								if (animator2 == null)
								{
									animator2 = npcsenses.GetComponentInChildren<Animator>();
								}
								if (animator2 != null)
								{
									animator2.SetInteger("ReactionType", 0);
								}
								if (this.DebugMode)
								{
									Debug.Log("[SensoryManager] " + npcsenses.name + " passive reaction ended (player out of range). ReactionType reset.");
								}
							}
						}
					}
				}
			}
			if (this.EnablePreDetectionEmotions && DetectionEmotionSource.Instance != null)
			{
				if (flag4)
				{
					DetectionEmotionSource.Instance.UpdatePreDetection(detectionSource, num2);
					return;
				}
				DetectionEmotionSource.Instance.StopPreDetection();
			}
		}

		// Token: 0x060095F7 RID: 38391 RVA: 0x003D987C File Offset: 0x003D7A7C
		private static void ClearTransitionContact(NPCSenses npc)
		{
			if (npc == null)
			{
				return;
			}
			npc.IsDetected = false;
			npc.HasLineOfSight = false;
			npc.HasAudioContact = false;
			npc.CurrentDetectionSource = DetectionSource.None;
			npc.SetAttentionState(0f);
			PlayerEmotionController instance = PlayerEmotionController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.OnNPCDetectionProgress(npc, 0f);
		}

		// Token: 0x060095F8 RID: 38392 RVA: 0x003D98D0 File Offset: 0x003D7AD0
		public static DetectionActorType ResolveActorType(NPCSenses npc)
		{
			if (npc == null)
			{
				return DetectionActorType.Generic;
			}
			if (npc.GetComponent<TroupeDetectionProxy>() != null)
			{
				return DetectionActorType.Reporter;
			}
			if (npc.GetComponent<RefereeChaser>() != null)
			{
				return DetectionActorType.Referee;
			}
			SuspicionSource component = npc.GetComponent<SuspicionSource>();
			if (component != null && Mathf.Approximately(component.suspicionDelta, 0f))
			{
				return DetectionActorType.Police;
			}
			return DetectionActorType.Generic;
		}

		// Token: 0x04009AFD RID: 39677
		[Header("Configuration")]
		public DetectionConfigSO GlobalConfig;

		// Token: 0x04009AFE RID: 39678
		[Tooltip("Max Raycasts per frame to avoid Physics spikes")]
		public int MaxRaycastsPerFrame = 20;

		// Token: 0x04009AFF RID: 39679
		public bool DebugMode;

		// Token: 0x04009B00 RID: 39680
		[Header("Audio Detection")]
		[Tooltip("If true, audio detection requires line of sight (no walls between NPC and player)")]
		public bool AudioRequiresLineOfSight = true;

		// Token: 0x04009B01 RID: 39681
		[Tooltip("Layer mask for audio obstacles (walls, doors, etc.)")]
		public LayerMask AudioObstacles;

		// Token: 0x04009B02 RID: 39682
		[Tooltip("Hysteresis multiplier for audio range to prevent flickering (e.g., 1.1 = 10% extra range to exit)")]
		[Range(1f, 1.5f)]
		public float AudioHysteresis = 1.15f;

		// Token: 0x04009B03 RID: 39683
		[Header("Pre-Detection Systems")]
		[Tooltip("If true, uses DetectionEmotionSource for pre-detection emotion changes")]
		public bool EnablePreDetectionEmotions = true;

		// Token: 0x04009B04 RID: 39684
		[Header("Final Detection")]
		[Tooltip("If true, the final detection event (ExhibitionEventDirector) will NEVER be triggered. Useful for testing pre-detection systems.")]
		public bool DisableFinalDetection;

		// Token: 0x04009B05 RID: 39685
		[Header("Runtime")]
		[SerializeField]
		private StealthActor _player;

		// Token: 0x04009B06 RID: 39686
		private StealthActor _collarStyleOwner;

		// Token: 0x04009B07 RID: 39687
		private MCStyleCustomizationService _playerStyles;

		// Token: 0x04009B08 RID: 39688
		private List<NPCSenses> _registeredNPCs = new List<NPCSenses>();

		// Token: 0x04009B09 RID: 39689
		private bool _isPaused;

		// Token: 0x04009B0A RID: 39690
		private readonly HashSet<string> _pauseReasons = new HashSet<string>();

		// Token: 0x04009B0B RID: 39691
		private bool _runModeDetectionEnabled = true;

		// Token: 0x04009B0C RID: 39692
		private bool _transitionContactsCleared;

		// Token: 0x04009B0D RID: 39693
		private static readonly int DenseFogBlend = Shader.PropertyToID("_Project_AzureFogBlend");

		// Token: 0x04009B0E RID: 39694
		private AzureCoreSystem _weatherAzure;

		// Token: 0x04009B0F RID: 39695
		private float _nextWeatherBindAttempt;

		// Token: 0x04009B10 RID: 39696
		private WeatherDetectionModifiers _weatherDetection = WeatherDetectionModifiers.Clear;

		// Token: 0x04009B11 RID: 39697
		private NativeArray<Vector3> _npcPositions;

		// Token: 0x04009B12 RID: 39698
		private NativeArray<Vector3> _npcForward;

		// Token: 0x04009B13 RID: 39699
		private NativeArray<float> _npcViewRadii;

		// Token: 0x04009B14 RID: 39700
		private NativeArray<float> _npcViewAngles;

		// Token: 0x04009B15 RID: 39701
		private NativeArray<float> _npcAttentionMultipliers;

		// Token: 0x04009B16 RID: 39702
		private NativeArray<int> _results;

		// Token: 0x04009B17 RID: 39703
		private int _detectionBufferCapacity;

		// Token: 0x020012E4 RID: 4836
		[BurstCompile]
		private struct DetectionJob : IJobParallelFor
		{
			// Token: 0x060095FB RID: 38395 RVA: 0x003D99A0 File Offset: 0x003D7BA0
			public void Execute(int i)
			{
				Vector3 vector = this.PlayerPosition - this.NPCPositions[i];
				float sqrMagnitude = vector.sqrMagnitude;
				float num = this.NPCViewRadii[i] * this.NPCViewRadii[i];
				if (sqrMagnitude > num)
				{
					this.Results[i] = 0;
					return;
				}
				if (WeatherDetectionPolicy.IsWithinFieldOfView(this.NPCForwards[i], vector, this.NPCViewAngles[i]))
				{
					this.Results[i] = 2;
					return;
				}
				this.Results[i] = 1;
			}

			// Token: 0x04009B18 RID: 39704
			public Vector3 PlayerPosition;

			// Token: 0x04009B19 RID: 39705
			[ReadOnly]
			public NativeArray<Vector3> NPCPositions;

			// Token: 0x04009B1A RID: 39706
			[ReadOnly]
			public NativeArray<Vector3> NPCForwards;

			// Token: 0x04009B1B RID: 39707
			[ReadOnly]
			public NativeArray<float> NPCViewRadii;

			// Token: 0x04009B1C RID: 39708
			[ReadOnly]
			public NativeArray<float> NPCViewAngles;

			// Token: 0x04009B1D RID: 39709
			[WriteOnly]
			public NativeArray<int> Results;
		}
	}
}
