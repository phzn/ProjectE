using System;
using Features.Detection;
using Features.GameOver;
using Features.NPC;
using Features.RunModes;
using Features.TimeFreeze;
using Features.WardrobeCombos;
using Pathfinding;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Police
{
	// Token: 0x02000DF2 RID: 3570
	[RequireComponent(typeof(NPCSenses))]
	[RequireComponent(typeof(AStarNPCController))]
	public class PoliceOfficerChaser : MonoBehaviour, IDetectionUISource
	{
		// Token: 0x140000B0 RID: 176
		// (add) Token: 0x06006A14 RID: 27156 RVA: 0x002A889C File Offset: 0x002A6A9C
		// (remove) Token: 0x06006A15 RID: 27157 RVA: 0x002A88D0 File Offset: 0x002A6AD0
		public static event Action<PoliceOfficerChaser> OnChaseStarted;

		// Token: 0x140000B1 RID: 177
		// (add) Token: 0x06006A16 RID: 27158 RVA: 0x002A8904 File Offset: 0x002A6B04
		// (remove) Token: 0x06006A17 RID: 27159 RVA: 0x002A8938 File Offset: 0x002A6B38
		public static event Action<PoliceOfficerChaser> OnChaseEscaped;

		// Token: 0x17001171 RID: 4465
		// (get) Token: 0x06006A18 RID: 27160 RVA: 0x002A896B File Offset: 0x002A6B6B
		public bool VisualActive
		{
			get
			{
				return this._senses != null && this._senses.HasLineOfSight;
			}
		}

		// Token: 0x17001172 RID: 4466
		// (get) Token: 0x06006A19 RID: 27161 RVA: 0x002A8988 File Offset: 0x002A6B88
		public bool AudioActive
		{
			get
			{
				return this._senses != null && this._senses.HasAudioContact;
			}
		}

		// Token: 0x17001173 RID: 4467
		// (get) Token: 0x06006A1A RID: 27162 RVA: 0x002A89A8 File Offset: 0x002A6BA8
		public float DetectionLevel
		{
			get
			{
				if (this._senses == null || !this._senses.HasLineOfSight)
				{
					return 0f;
				}
				SensoryManager instance = SensoryManager.Instance;
				StealthActor stealthActor = ((instance != null) ? instance.GetPlayer() : null);
				if (stealthActor == null)
				{
					return 0f;
				}
				float num = Vector3.Distance(this._senses.transform.position, stealthActor.Position);
				float num2 = Mathf.Max(this.visualDetectionMaxRange, this.catchDistance + 0.01f);
				if (num <= this.catchDistance)
				{
					return 1f;
				}
				if (num >= num2)
				{
					return 0f;
				}
				return Mathf.Clamp01((num2 - num) / (num2 - this.catchDistance));
			}
		}

		// Token: 0x06006A1B RID: 27163 RVA: 0x002A8A5C File Offset: 0x002A6C5C
		private void Awake()
		{
			this._senses = base.GetComponent<NPCSenses>();
			this._ctrl = base.GetComponent<AStarNPCController>();
			this._follower = base.GetComponent<FollowerEntity>();
			this._poiNav = base.GetComponent<SimplePOINavigator>();
			this._attentionUI = base.GetComponent<AttentionUIController>();
			if (this._follower != null)
			{
				this._baselineMaxSpeed = this._follower.maxSpeed;
			}
			if (this._attentionUI != null)
			{
				this._attentionUI.SetUISource(this);
			}
		}

		// Token: 0x06006A1C RID: 27164 RVA: 0x002A8ADE File Offset: 0x002A6CDE
		private void Start()
		{
			this.TryHookDirector();
		}

		// Token: 0x06006A1D RID: 27165 RVA: 0x002A8AE6 File Offset: 0x002A6CE6
		private void OnEnable()
		{
			this._simulationPauseActive = false;
			NPCSimulationPause.PauseChanged += this.HandleSimulationPauseChanged;
			NPCSimulationPause.RegisterParticipant(this);
			this.HandleSimulationPauseChanged(NPCSimulationPause.IsPaused);
		}

		// Token: 0x06006A1E RID: 27166 RVA: 0x002A8B11 File Offset: 0x002A6D11
		private void OnDisable()
		{
			NPCSimulationPause.PauseChanged -= this.HandleSimulationPauseChanged;
			this.UnhookDirector();
		}

		// Token: 0x06006A1F RID: 27167 RVA: 0x002A8B2A File Offset: 0x002A6D2A
		private void OnDestroy()
		{
			this.UnhookDirector();
		}

		// Token: 0x06006A20 RID: 27168 RVA: 0x002A8B34 File Offset: 0x002A6D34
		private void Update()
		{
			if (DetectionTransitionGate.IsBlocked || NPCSimulationPause.IsPaused)
			{
				this.BeginSimulationPauseIfNeeded();
				return;
			}
			this.ResumeSimulationIfNeeded();
			this.ShiftSimulationTimestamps(NPCSimulationPause.FrameTimeLoss);
			if (NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				return;
			}
			this.TryHookDirector();
			if (GameOverManager.Instance != null && GameOverManager.Instance.IsGameOver)
			{
				if (this._state != PoliceOfficerChaser.OfficerState.Stopped)
				{
					this.EnterStopped();
				}
				return;
			}
			if (this._directorActive || (PerpWalkController.Instance != null && PerpWalkController.Instance.IsActive))
			{
				if (this._state != PoliceOfficerChaser.OfficerState.Stopped)
				{
					this.EnterStopped();
				}
				return;
			}
			if (this._resumeAfterDirector || this._state == PoliceOfficerChaser.OfficerState.Stopped)
			{
				this._resumeAfterDirector = false;
				this.ResumeAfterDirector();
			}
			if (WardrobeComboRuntime.ShouldIgnore(DetectionActorType.Police) || !RunModeService.EncountersEnabled)
			{
				if (this._state == PoliceOfficerChaser.OfficerState.Chasing)
				{
					this.ExitChasing(false);
					return;
				}
				if (this._state == PoliceOfficerChaser.OfficerState.AudioTurning)
				{
					this.ExitAudioTurning();
				}
				return;
			}
			else
			{
				switch (this._state)
				{
				case PoliceOfficerChaser.OfficerState.Roaming:
					this.TickRoaming();
					return;
				case PoliceOfficerChaser.OfficerState.Chasing:
					this.TickChasing();
					return;
				case PoliceOfficerChaser.OfficerState.AudioTurning:
					this.TickAudioTurning();
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x06006A21 RID: 27169 RVA: 0x002A8C50 File Offset: 0x002A6E50
		private void HandleSimulationPauseChanged(bool paused)
		{
			if (paused)
			{
				this.BeginSimulationPauseIfNeeded();
				return;
			}
			this.ResumeSimulationIfNeeded();
		}

		// Token: 0x06006A22 RID: 27170 RVA: 0x002A8C62 File Offset: 0x002A6E62
		private void BeginSimulationPauseIfNeeded()
		{
			if (this._simulationPauseActive)
			{
				return;
			}
			this._simulationPauseActive = true;
			this._simulationPauseStartedAt = Time.time;
		}

		// Token: 0x06006A23 RID: 27171 RVA: 0x002A8C7F File Offset: 0x002A6E7F
		private void ResumeSimulationIfNeeded()
		{
			if (!this._simulationPauseActive)
			{
				return;
			}
			this.ShiftSimulationTimestamps(Mathf.Max(0f, Time.time - this._simulationPauseStartedAt));
			this._simulationPauseActive = false;
		}

		// Token: 0x06006A24 RID: 27172 RVA: 0x002A8CAD File Offset: 0x002A6EAD
		private void ShiftSimulationTimestamps(float amount)
		{
			if (amount <= 0f)
			{
				return;
			}
			if (this._audioContactStartTime >= 0f)
			{
				this._audioContactStartTime += amount;
			}
			if (this._state == PoliceOfficerChaser.OfficerState.Chasing)
			{
				this._lastChaseSightTime += amount;
			}
		}

		// Token: 0x06006A25 RID: 27173 RVA: 0x002A8CEC File Offset: 0x002A6EEC
		private void TryHookDirector()
		{
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			if (!this._directorHooked || this._hookedDirector != instance)
			{
				this.UnhookDirector();
				this._hookedDirector = instance;
				if (instance != null)
				{
					UnityEvent<NPCSenses> onDetectionStarted = instance.OnDetectionStarted;
					if (onDetectionStarted != null)
					{
						onDetectionStarted.AddListener(new UnityAction<NPCSenses>(this.HandleDirectorStarted));
					}
					UnityEvent onSequenceComplete = instance.OnSequenceComplete;
					if (onSequenceComplete != null)
					{
						onSequenceComplete.AddListener(new UnityAction(this.HandleDirectorComplete));
					}
					this._directorHooked = true;
				}
			}
			if (instance != null && instance.IsEventActive)
			{
				this._directorActive = true;
				return;
			}
			if (this._directorActive)
			{
				this.HandleDirectorComplete();
			}
		}

		// Token: 0x06006A26 RID: 27174 RVA: 0x002A8D98 File Offset: 0x002A6F98
		private void UnhookDirector()
		{
			if (!this._directorHooked)
			{
				return;
			}
			ExhibitionEventDirector hookedDirector = this._hookedDirector;
			if (hookedDirector != null)
			{
				if (hookedDirector.OnDetectionStarted != null)
				{
					hookedDirector.OnDetectionStarted.RemoveListener(new UnityAction<NPCSenses>(this.HandleDirectorStarted));
				}
				if (hookedDirector.OnSequenceComplete != null)
				{
					hookedDirector.OnSequenceComplete.RemoveListener(new UnityAction(this.HandleDirectorComplete));
				}
			}
			this._directorHooked = false;
			this._hookedDirector = null;
		}

		// Token: 0x06006A27 RID: 27175 RVA: 0x002A8E0A File Offset: 0x002A700A
		private void HandleDirectorStarted(NPCSenses _)
		{
			this._directorActive = true;
		}

		// Token: 0x06006A28 RID: 27176 RVA: 0x002A8E13 File Offset: 0x002A7013
		private void HandleDirectorComplete()
		{
			this._directorActive = false;
			this._resumeAfterDirector = true;
		}

		// Token: 0x06006A29 RID: 27177 RVA: 0x002A8E23 File Offset: 0x002A7023
		private void ResumeAfterDirector()
		{
			if (this._state == PoliceOfficerChaser.OfficerState.Stopped)
			{
				if (this._follower != null)
				{
					this._follower.isStopped = false;
				}
				this.SetPOINavigatorEnabled(true);
				this._state = PoliceOfficerChaser.OfficerState.Roaming;
			}
		}

		// Token: 0x06006A2A RID: 27178 RVA: 0x002A8E58 File Offset: 0x002A7058
		private void TickRoaming()
		{
			if (this._senses == null || this._senses.IsInCooldown || PlayerStealthBridge.IsFullyHidden)
			{
				return;
			}
			SensoryManager instance = SensoryManager.Instance;
			if (instance == null)
			{
				return;
			}
			if (this._senses.HasLineOfSight && RunModeService.EncountersEnabled && !this._senses.BlocksFinalDetectionWhileSeated && !instance.DisableFinalDetection)
			{
				this.EnterChasing();
				return;
			}
			if (this._senses.HasAudioContact)
			{
				if (this._audioContactStartTime < 0f)
				{
					this._audioContactStartTime = Time.time;
				}
				if (Time.time - this._audioContactStartTime >= this.audioPersistenceTime)
				{
					this.EnterAudioTurning();
					return;
				}
			}
			else
			{
				this._audioContactStartTime = -1f;
			}
		}

		// Token: 0x06006A2B RID: 27179 RVA: 0x002A8F14 File Offset: 0x002A7114
		private void EnterChasing()
		{
			Action<PoliceOfficerChaser> onChaseStarted = PoliceOfficerChaser.OnChaseStarted;
			if (onChaseStarted != null)
			{
				onChaseStarted(this);
			}
			if (this.debugDetection)
			{
				Debug.Log("[PoliceOfficerChaser." + base.name + "] Roaming → Chasing");
			}
			this.SetPOINavigatorEnabled(false);
			this._lastChaseSightTime = Time.time;
			this._audioContactStartTime = -1f;
			if (this._follower != null)
			{
				this._follower.isStopped = false;
				this._follower.maxSpeed = this.chaseSpeed;
			}
			AStarNPCController ctrl = this._ctrl;
			if (ctrl != null)
			{
				ctrl.SetSpeedOverride(this.chaseSpeed);
			}
			this._state = PoliceOfficerChaser.OfficerState.Chasing;
			this.ReaimAtPlayer();
		}

		// Token: 0x06006A2C RID: 27180 RVA: 0x002A8FC0 File Offset: 0x002A71C0
		private void TickChasing()
		{
			if (this._senses == null)
			{
				this.ExitChasing(false);
				return;
			}
			SensoryManager instance = SensoryManager.Instance;
			StealthActor stealthActor = ((instance != null) ? instance.GetPlayer() : null);
			if (stealthActor == null)
			{
				this.ExitChasing(false);
				return;
			}
			if (this._senses.HasLineOfSight && !PlayerStealthBridge.IsFullyHidden)
			{
				this._lastChaseSightTime = Time.time;
			}
			float num = Vector3.Distance(base.transform.position, stealthActor.Position);
			if (num <= this.catchDistance && !TimeFreezeController.IsAnyActive && !PlayerStealthBridge.IsFullyHidden)
			{
				if (this.debugDetection)
				{
					Debug.Log(string.Format("[PoliceOfficerChaser.{0}] Catch! dist={1:F1}m", base.name, num));
				}
				if (this.TriggerCaptureEncounter())
				{
					this.EnterStopped();
					return;
				}
			}
			if (this.chaseLoseSightTimeout > 0f && Time.time - this._lastChaseSightTime > this.chaseLoseSightTimeout)
			{
				if (this.debugDetection)
				{
					Debug.Log("[PoliceOfficerChaser." + base.name + "] Chasing → Roaming (lost sight)");
				}
				this.ExitChasing(this.cooldownOnChaseEscape);
				return;
			}
			if (this.chasingReaimEveryFrames > 0 && Time.frameCount % this.chasingReaimEveryFrames == 0)
			{
				this.ReaimAtPlayer();
			}
		}

		// Token: 0x06006A2D RID: 27181 RVA: 0x002A90FC File Offset: 0x002A72FC
		private void ReaimAtPlayer()
		{
			SensoryManager instance = SensoryManager.Instance;
			StealthActor stealthActor = ((instance != null) ? instance.GetPlayer() : null);
			if (stealthActor == null)
			{
				return;
			}
			AStarNPCController ctrl = this._ctrl;
			if (ctrl == null)
			{
				return;
			}
			ctrl.SetDestination(stealthActor.Position);
		}

		// Token: 0x06006A2E RID: 27182 RVA: 0x002A9144 File Offset: 0x002A7344
		private bool TriggerCaptureEncounter()
		{
			if (!RunModeService.EncountersEnabled)
			{
				return false;
			}
			if (TimeFreezeController.IsAnyActive || DetectionTransitionGate.IsBlocked || PlayerStealthBridge.IsFullyHidden)
			{
				return false;
			}
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			if ((instance != null && instance.IsEventActive) || (PerpWalkController.Instance != null && PerpWalkController.Instance.IsActive))
			{
				return true;
			}
			if (this.captureProfile != null && instance != null && this._senses != null && !instance.IsEventActive)
			{
				NPCSenses capturedSenses = this._senses;
				EncounterRequest encounterRequest = new EncounterRequest
				{
					initiator = capturedSenses,
					additionalActors = null,
					profile = this.captureProfile,
					onSceneEnded = delegate
					{
						if (PerpWalkController.Instance != null)
						{
							PerpWalkController.Instance.BeginPerpWalk(capturedSenses, null);
						}
					}
				};
				if (this.debugDetection)
				{
					Debug.Log(string.Concat(new string[]
					{
						"[PoliceOfficerChaser.",
						base.name,
						"] TriggerCaptureEncounter via Director (profile='",
						this.captureProfile.name,
						"')"
					}));
				}
				instance.TriggerEncounter(encounterRequest);
				return instance.IsEventActive;
			}
			if (this.debugDetection)
			{
				Debug.LogWarning("[PoliceOfficerChaser." + base.name + "] No captureProfile or Director busy → fallback GameOver");
			}
			if (GameOverManager.Instance != null)
			{
				GameOverManager.Instance.TriggerCapture(this);
			}
			return GameOverManager.Instance != null && GameOverManager.Instance.IsGameOver;
		}

		// Token: 0x06006A2F RID: 27183 RVA: 0x002A92D0 File Offset: 0x002A74D0
		private void ExitChasing(bool toCooldown)
		{
			AStarNPCController ctrl = this._ctrl;
			if (ctrl != null)
			{
				ctrl.ClearSpeedOverride();
			}
			if (this._follower != null)
			{
				this._follower.maxSpeed = this._baselineMaxSpeed;
				this._follower.isStopped = false;
			}
			this._audioContactStartTime = -1f;
			if (toCooldown && this._senses != null)
			{
				this._senses.StartCooldown();
			}
			if (toCooldown)
			{
				Action<PoliceOfficerChaser> onChaseEscaped = PoliceOfficerChaser.OnChaseEscaped;
				if (onChaseEscaped != null)
				{
					onChaseEscaped(this);
				}
			}
			this.SetPOINavigatorEnabled(true);
			this._state = PoliceOfficerChaser.OfficerState.Roaming;
		}

		// Token: 0x06006A30 RID: 27184 RVA: 0x002A9364 File Offset: 0x002A7564
		private void EnterAudioTurning()
		{
			if (this.debugDetection)
			{
				Debug.Log("[PoliceOfficerChaser." + base.name + "] Roaming → AudioTurning");
			}
			this.SetPOINavigatorEnabled(false);
			if (this._follower != null)
			{
				this._follower.isStopped = true;
			}
			this._state = PoliceOfficerChaser.OfficerState.AudioTurning;
		}

		// Token: 0x06006A31 RID: 27185 RVA: 0x002A93BC File Offset: 0x002A75BC
		private void TickAudioTurning()
		{
			if (this._senses == null || PlayerStealthBridge.IsFullyHidden)
			{
				this.ExitAudioTurning();
				return;
			}
			SensoryManager instance = SensoryManager.Instance;
			if (instance != null && this._senses.HasLineOfSight && !this._senses.BlocksFinalDetectionWhileSeated && !instance.DisableFinalDetection)
			{
				this.ExitAudioTurning();
				this.EnterChasing();
				return;
			}
			if (!this._senses.HasAudioContact)
			{
				this.ExitAudioTurning();
				return;
			}
			Vector3 vector = this._senses.LastKnownPlayerPosition - base.transform.position;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.0001f)
			{
				return;
			}
			Quaternion quaternion = Quaternion.LookRotation(vector.normalized, Vector3.up);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, this.audioTurnSpeed * NPCSimulationPause.SimulationDeltaTime);
		}

		// Token: 0x06006A32 RID: 27186 RVA: 0x002A94A8 File Offset: 0x002A76A8
		private void ExitAudioTurning()
		{
			this._audioContactStartTime = -1f;
			if (this._follower != null)
			{
				this._follower.isStopped = false;
			}
			this.SetPOINavigatorEnabled(true);
			this._state = PoliceOfficerChaser.OfficerState.Roaming;
		}

		// Token: 0x06006A33 RID: 27187 RVA: 0x002A94E0 File Offset: 0x002A76E0
		private void EnterStopped()
		{
			this.SetPOINavigatorEnabled(false);
			if (this._follower != null)
			{
				this._follower.isStopped = true;
				this._follower.maxSpeed = this._baselineMaxSpeed;
			}
			AStarNPCController ctrl = this._ctrl;
			if (ctrl != null)
			{
				ctrl.ClearSpeedOverride();
			}
			this._state = PoliceOfficerChaser.OfficerState.Stopped;
		}

		// Token: 0x06006A34 RID: 27188 RVA: 0x002A9537 File Offset: 0x002A7737
		private void SetPOINavigatorEnabled(bool enabled)
		{
			if (this._poiNav != null)
			{
				this._poiNav.enabled = enabled;
			}
		}

		// Token: 0x04006CD8 RID: 27864
		[Header("Chase")]
		[Tooltip("Distance under which the officer 'catches' the player → game over.")]
		public float catchDistance = 2.5f;

		// Token: 0x04006CD9 RID: 27865
		[Tooltip("Movement speed (m/s) during chase. Overrides FollowerEntity.maxSpeed.")]
		public float chaseSpeed = 2f;

		// Token: 0x04006CDA RID: 27866
		[Tooltip("How often (frames) the chase re-aims at the player's current position.")]
		public int chasingReaimEveryFrames = 10;

		// Token: 0x04006CDB RID: 27867
		[Tooltip("Seconds without line-of-sight before abandoning the chase. 0 = chase forever.")]
		public float chaseLoseSightTimeout = 4f;

		// Token: 0x04006CDC RID: 27868
		[Tooltip("If true, escape-by-timeout triggers NPCSenses cooldown so the officer can't re-detect immediately.")]
		public bool cooldownOnChaseEscape = true;

		// Token: 0x04006CDD RID: 27869
		[Header("Audio Turn")]
		[Tooltip("Seconds of CONSECUTIVE HasAudioContact before the officer turns toward the player.")]
		public float audioPersistenceTime = 3f;

		// Token: 0x04006CDE RID: 27870
		[Tooltip("Rotation speed (deg/s) toward the player during audio-turn.")]
		public float audioTurnSpeed = 180f;

		// Token: 0x04006CDF RID: 27871
		[Header("UI Detection Level")]
		[Tooltip("Distance beyond which the UI detection level reads 0 even with LoS. Usually = NPCSenses.BaseViewRadius.")]
		public float visualDetectionMaxRange = 15f;

		// Token: 0x04006CE0 RID: 27872
		[Header("Capture (Director encounter)")]
		[Tooltip("EncounterProfileSO usato quando l'officer cattura il player (catch by distance). Tipicamente EncounterProfile_PoliceCapture (skip choice → emote Police). Se null, ripiega su GameOverManager.TriggerCapture (vecchio comportamento, reload scena).")]
		public EncounterProfileSO captureProfile;

		// Token: 0x04006CE1 RID: 27873
		[Header("Debug")]
		[SerializeField]
		private bool debugDetection;

		// Token: 0x04006CE2 RID: 27874
		private PoliceOfficerChaser.OfficerState _state;

		// Token: 0x04006CE3 RID: 27875
		private NPCSenses _senses;

		// Token: 0x04006CE4 RID: 27876
		private AStarNPCController _ctrl;

		// Token: 0x04006CE5 RID: 27877
		private FollowerEntity _follower;

		// Token: 0x04006CE6 RID: 27878
		private SimplePOINavigator _poiNav;

		// Token: 0x04006CE7 RID: 27879
		private AttentionUIController _attentionUI;

		// Token: 0x04006CE8 RID: 27880
		private float _baselineMaxSpeed;

		// Token: 0x04006CE9 RID: 27881
		private float _audioContactStartTime = -1f;

		// Token: 0x04006CEA RID: 27882
		private float _lastChaseSightTime;

		// Token: 0x04006CEB RID: 27883
		private bool _directorActive;

		// Token: 0x04006CEC RID: 27884
		private bool _directorHooked;

		// Token: 0x04006CED RID: 27885
		private ExhibitionEventDirector _hookedDirector;

		// Token: 0x04006CEE RID: 27886
		private bool _resumeAfterDirector;

		// Token: 0x04006CEF RID: 27887
		private bool _simulationPauseActive;

		// Token: 0x04006CF0 RID: 27888
		private float _simulationPauseStartedAt;

		// Token: 0x02000DF3 RID: 3571
		private enum OfficerState
		{
			// Token: 0x04006CF2 RID: 27890
			Roaming,
			// Token: 0x04006CF3 RID: 27891
			Chasing,
			// Token: 0x04006CF4 RID: 27892
			AudioTurning,
			// Token: 0x04006CF5 RID: 27893
			Stopped
		}
	}
}
