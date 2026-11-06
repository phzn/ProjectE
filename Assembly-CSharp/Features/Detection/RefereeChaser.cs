using System;
using Bozo.AnimeCharacters;
using Features.NPC;
using Features.RunModes;
using Features.TimeFreeze;
using Features.WardrobeCombos;
using Pathfinding;
using ProjectExhibitionist.CharacterCustomization.Core.Storage;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Detection
{
	// Token: 0x020012DC RID: 4828
	[RequireComponent(typeof(NPCSenses))]
	[RequireComponent(typeof(AStarNPCController))]
	public class RefereeChaser : MonoBehaviour, IDetectionUISource
	{
		// Token: 0x1400010A RID: 266
		// (add) Token: 0x0600959B RID: 38299 RVA: 0x003D6B0C File Offset: 0x003D4D0C
		// (remove) Token: 0x0600959C RID: 38300 RVA: 0x003D6B40 File Offset: 0x003D4D40
		public static event Action<RefereeChaser> ChaseEscaped;

		// Token: 0x170017E2 RID: 6114
		// (get) Token: 0x0600959D RID: 38301 RVA: 0x003D6B73 File Offset: 0x003D4D73
		public bool VisualActive
		{
			get
			{
				return this._senses != null && this._senses.HasLineOfSight;
			}
		}

		// Token: 0x170017E3 RID: 6115
		// (get) Token: 0x0600959E RID: 38302 RVA: 0x003D6B90 File Offset: 0x003D4D90
		public bool AudioActive
		{
			get
			{
				return this._senses != null && this._senses.HasAudioContact;
			}
		}

		// Token: 0x170017E4 RID: 6116
		// (get) Token: 0x0600959F RID: 38303 RVA: 0x003D6BB0 File Offset: 0x003D4DB0
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

		// Token: 0x060095A0 RID: 38304 RVA: 0x003D6C64 File Offset: 0x003D4E64
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

		// Token: 0x060095A1 RID: 38305 RVA: 0x003D6CE6 File Offset: 0x003D4EE6
		private void OnEnable()
		{
			this._simulationPauseActive = false;
			NPCSimulationPause.PauseChanged += this.HandleSimulationPauseChanged;
			NPCSimulationPause.RegisterParticipant(this);
			this.HandleSimulationPauseChanged(NPCSimulationPause.IsPaused);
		}

		// Token: 0x060095A2 RID: 38306 RVA: 0x003D6D11 File Offset: 0x003D4F11
		private void Start()
		{
			this.InitializeDialogueIdentity();
		}

		// Token: 0x060095A3 RID: 38307 RVA: 0x003D6D1C File Offset: 0x003D4F1C
		private void InitializeDialogueIdentity()
		{
			NPCDialogueHolder component = base.GetComponent<NPCDialogueHolder>();
			OutfitSystem component2 = base.GetComponent<OutfitSystem>();
			CharacterObject characterObject = ((component2 != null) ? component2.characterData : null);
			if (component == null || characterObject == null || !string.IsNullOrEmpty(component.NpcIdentityId))
			{
				return;
			}
			string text = "sys_" + characterObject.name;
			CharacterData characterData = NpcLookStore.LoadLook(text);
			CharacterGeneralInfo characterGeneralInfo;
			if ((characterGeneralInfo = ((characterData != null) ? characterData.generalInfo : null)) == null)
			{
				CharacterData data = characterObject.data;
				characterGeneralInfo = ((data != null) ? data.generalInfo : null);
			}
			CharacterGeneralInfo characterGeneralInfo2 = characterGeneralInfo;
			if (characterGeneralInfo2 != null)
			{
				NPCPersonalityProfileSO personalityProfile = component.PersonalityProfile;
				component.Initialize(characterGeneralInfo2);
				if (personalityProfile != null)
				{
					component.PersonalityProfile = personalityProfile;
				}
			}
			component.SetNpcIdentity(text);
		}

		// Token: 0x060095A4 RID: 38308 RVA: 0x003D6DC8 File Offset: 0x003D4FC8
		private void OnDisable()
		{
			NPCSimulationPause.PauseChanged -= this.HandleSimulationPauseChanged;
			this.UnregisterDirectorListeners();
		}

		// Token: 0x060095A5 RID: 38309 RVA: 0x003D6DE4 File Offset: 0x003D4FE4
		private void TryRegisterDirectorListeners()
		{
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			if (this._listenersRegistered && this._hookedDirector == instance)
			{
				return;
			}
			this.UnregisterDirectorListeners();
			if (instance == null)
			{
				return;
			}
			instance.OnDetectionStarted.AddListener(new UnityAction<NPCSenses>(this.OnDirectorDetectionStarted));
			instance.OnCooldownEnded.AddListener(new UnityAction<NPCSenses>(this.OnDirectorCooldownEnded));
			this._hookedDirector = instance;
			this._listenersRegistered = true;
		}

		// Token: 0x060095A6 RID: 38310 RVA: 0x003D6E5C File Offset: 0x003D505C
		private void UnregisterDirectorListeners()
		{
			if (!this._listenersRegistered)
			{
				return;
			}
			ExhibitionEventDirector hookedDirector = this._hookedDirector;
			if (hookedDirector != null)
			{
				hookedDirector.OnDetectionStarted.RemoveListener(new UnityAction<NPCSenses>(this.OnDirectorDetectionStarted));
				hookedDirector.OnCooldownEnded.RemoveListener(new UnityAction<NPCSenses>(this.OnDirectorCooldownEnded));
			}
			this._listenersRegistered = false;
			this._hookedDirector = null;
		}

		// Token: 0x060095A7 RID: 38311 RVA: 0x003D6EC0 File Offset: 0x003D50C0
		private void Update()
		{
			if (DetectionInteractionGate.IsGameOver)
			{
				if (!this._gameOverSuspended)
				{
					this._gameOverSuspended = true;
					this._deferredDirectorTransition = 0;
					this.EnterSuspended();
				}
				return;
			}
			if (this._gameOverSuspended)
			{
				this._gameOverSuspended = false;
				this.ExitSuspended();
			}
			if (NPCSimulationPause.IsPaused || DetectionTransitionGate.IsBlocked)
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
			if (this._deferredDirectorTransition != 0)
			{
				int deferredDirectorTransition = this._deferredDirectorTransition;
				this._deferredDirectorTransition = 0;
				if (deferredDirectorTransition == 1)
				{
					this.EnterSuspended();
				}
				else if (deferredDirectorTransition == 2 && this._state == RefereeChaser.ChaseState.Suspended)
				{
					this.ExitSuspended();
				}
			}
			this.TryRegisterDirectorListeners();
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			if (this._state != RefereeChaser.ChaseState.Suspended && instance != null && instance.IsEventActive && instance.DetectingNPC == this._senses)
			{
				this.EnterSuspended();
			}
			if (WardrobeComboRuntime.ShouldIgnore(DetectionActorType.Referee) || !RunModeService.DetectionEnabled || !RunModeService.EncountersEnabled || (this._senses != null && this._senses.IsGhostFleeing))
			{
				if (this._state == RefereeChaser.ChaseState.Chasing)
				{
					this.ExitChasing(false);
				}
				else if (this._state == RefereeChaser.ChaseState.AudioTurning)
				{
					this.ExitAudioTurning();
				}
				if (this._senses != null && this._senses.IsGhostFleeing)
				{
					this.SetPOINavigatorEnabled(false);
				}
				return;
			}
			switch (this._state)
			{
			case RefereeChaser.ChaseState.Roaming:
				this.TickRoaming();
				return;
			case RefereeChaser.ChaseState.Chasing:
				this.TickChasing();
				return;
			case RefereeChaser.ChaseState.AudioTurning:
				this.TickAudioTurning();
				return;
			case RefereeChaser.ChaseState.Suspended:
				this.TickSuspended(instance);
				return;
			default:
				return;
			}
		}

		// Token: 0x060095A8 RID: 38312 RVA: 0x003D705C File Offset: 0x003D525C
		private void HandleSimulationPauseChanged(bool paused)
		{
			if (paused)
			{
				this.BeginSimulationPauseIfNeeded();
				return;
			}
			this.ResumeSimulationIfNeeded();
		}

		// Token: 0x060095A9 RID: 38313 RVA: 0x003D706E File Offset: 0x003D526E
		private void BeginSimulationPauseIfNeeded()
		{
			if (this._simulationPauseActive)
			{
				return;
			}
			this._simulationPauseActive = true;
			this._simulationPauseStartedAt = Time.time;
		}

		// Token: 0x060095AA RID: 38314 RVA: 0x003D708B File Offset: 0x003D528B
		private void ResumeSimulationIfNeeded()
		{
			if (!this._simulationPauseActive)
			{
				return;
			}
			this.ShiftSimulationTimestamps(Mathf.Max(0f, Time.time - this._simulationPauseStartedAt));
			this._simulationPauseActive = false;
		}

		// Token: 0x060095AB RID: 38315 RVA: 0x003D70B9 File Offset: 0x003D52B9
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
			if (this._state == RefereeChaser.ChaseState.Chasing)
			{
				this._lastChaseSightTime += amount;
			}
		}

		// Token: 0x060095AC RID: 38316 RVA: 0x003D70F8 File Offset: 0x003D52F8
		private void TickSuspended(ExhibitionEventDirector dir)
		{
			bool flag = dir != null && dir.IsEventActive && dir.DetectingNPC == this._senses;
			bool flag2 = this._senses != null && this._senses.IsInCooldown;
			if (!flag && !flag2)
			{
				this.ExitSuspended();
			}
		}

		// Token: 0x060095AD RID: 38317 RVA: 0x003D7152 File Offset: 0x003D5352
		private void OnDirectorDetectionStarted(NPCSenses npc)
		{
			if (npc != this._senses)
			{
				return;
			}
			if (NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				this._deferredDirectorTransition = 1;
				return;
			}
			this.EnterSuspended();
		}

		// Token: 0x060095AE RID: 38318 RVA: 0x003D7184 File Offset: 0x003D5384
		private void OnDirectorCooldownEnded(NPCSenses npc)
		{
			if (DetectionInteractionGate.IsGameOver)
			{
				return;
			}
			if (npc != this._senses)
			{
				return;
			}
			if (NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				this._deferredDirectorTransition = 2;
				return;
			}
			if (this._state == RefereeChaser.ChaseState.Suspended)
			{
				this.ExitSuspended();
			}
		}

		// Token: 0x060095AF RID: 38319 RVA: 0x003D71D4 File Offset: 0x003D53D4
		private void EnterSuspended()
		{
			if (this.debugDetection)
			{
				Debug.Log("[RefereeChaser." + base.name + "] → Suspended (Director)");
			}
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
			this._audioContactStartTime = -1f;
			this._state = RefereeChaser.ChaseState.Suspended;
		}

		// Token: 0x060095B0 RID: 38320 RVA: 0x003D7258 File Offset: 0x003D5458
		private void ExitSuspended()
		{
			if (this.debugDetection)
			{
				Debug.Log("[RefereeChaser." + base.name + "] Suspended → Roaming (Director done)");
			}
			if (this._follower != null)
			{
				this._follower.isStopped = false;
			}
			this.SetPOINavigatorEnabled(true);
			this._state = RefereeChaser.ChaseState.Roaming;
		}

		// Token: 0x060095B1 RID: 38321 RVA: 0x003D72B0 File Offset: 0x003D54B0
		private void TickRoaming()
		{
			if (DetectionInteractionGate.IsAutonomousDetectionBlocked)
			{
				return;
			}
			if (this._senses == null || this._senses.IsInCooldown)
			{
				return;
			}
			SensoryManager instance = SensoryManager.Instance;
			if (instance == null)
			{
				return;
			}
			if (this._senses.HasLineOfSight && !this._senses.BlocksFinalDetectionWhileSeated && RunModeService.EncountersEnabled && !instance.DisableFinalDetection && !WardrobeComboRuntime.ShouldDisableFinalDetection(DetectionActorType.Referee, base.transform.position))
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
				if (this.debugDetection && Time.frameCount % 60 == 0)
				{
					float num = Time.time - this._audioContactStartTime;
					Debug.Log(string.Format("[RefereeChaser.{0}] Audio building: {1:F1}s / {2:F1}s", base.name, num, this.audioPersistenceTime));
					return;
				}
			}
			else
			{
				this._audioContactStartTime = -1f;
			}
		}

		// Token: 0x060095B2 RID: 38322 RVA: 0x003D73C8 File Offset: 0x003D55C8
		private void EnterChasing()
		{
			if (this.debugDetection)
			{
				Debug.Log("[RefereeChaser." + base.name + "] Roaming → Chasing");
			}
			this.SetPOINavigatorEnabled(false);
			this._lastChaseSightTime = Time.time;
			this._chaseBlockedTime = 0f;
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
			this._state = RefereeChaser.ChaseState.Chasing;
			this.ReaimAtPlayer();
		}

		// Token: 0x060095B3 RID: 38323 RVA: 0x003D7470 File Offset: 0x003D5670
		private void TickChasing()
		{
			if (DetectionInteractionGate.IsAutonomousDetectionBlocked)
			{
				this._chaseBlockedTime += Time.deltaTime;
				return;
			}
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
			if (this._senses.HasLineOfSight)
			{
				this._lastChaseSightTime = Time.time;
				this._chaseBlockedTime = 0f;
			}
			float num = Vector3.Distance(base.transform.position, stealthActor.Position);
			if (num <= this.catchDistance && RunModeService.EncountersEnabled && !TimeFreezeController.IsAnyActive)
			{
				if (this.debugDetection)
				{
					Debug.Log(string.Format("[RefereeChaser.{0}] Catch! dist={1:F1}m ≤ {2:F1}m → trigger detection", base.name, num, this.catchDistance));
				}
				ExhibitionEventDirector instance2 = ExhibitionEventDirector.Instance;
				if (instance2 != null && !instance2.IsEventActive)
				{
					instance2.TriggerDetection(this._senses);
					if (this._state == RefereeChaser.ChaseState.Suspended)
					{
						return;
					}
				}
				this.ExitChasing(true);
				return;
			}
			if (this.chaseLoseSightTimeout > 0f && Time.time - this._lastChaseSightTime > this.chaseLoseSightTimeout)
			{
				if (this.debugDetection)
				{
					Debug.Log(string.Format("[RefereeChaser.{0}] Chasing → Roaming (lost sight per {1}s)", base.name, this.chaseLoseSightTimeout));
				}
				bool flag = Time.time - this._lastChaseSightTime - this._chaseBlockedTime > this.chaseLoseSightTimeout;
				this.ExitChasing(this.cooldownOnChaseEscape);
				if (flag)
				{
					Action<RefereeChaser> chaseEscaped = RefereeChaser.ChaseEscaped;
					if (chaseEscaped == null)
					{
						return;
					}
					chaseEscaped(this);
				}
				return;
			}
			if (this.chasingReaimEveryFrames > 0 && Time.frameCount % this.chasingReaimEveryFrames == 0)
			{
				this.ReaimAtPlayer();
			}
			if (this.debugDetection && Time.frameCount % 30 == 0)
			{
				float num2 = Time.time - this._lastChaseSightTime;
				Debug.Log(string.Format("[RefereeChaser.{0}] Chasing: dist={1:F1}m / catch={2:F1}m, ", base.name, num, this.catchDistance) + string.Format("sinceSight={0:F1}s / timeout={1:F1}s, LOS={2}", num2, this.chaseLoseSightTimeout, this._senses.HasLineOfSight));
			}
		}

		// Token: 0x060095B4 RID: 38324 RVA: 0x003D76A4 File Offset: 0x003D58A4
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

		// Token: 0x060095B5 RID: 38325 RVA: 0x003D76EC File Offset: 0x003D58EC
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
			}
			if (toCooldown && this._senses != null)
			{
				this._senses.StartCooldown();
			}
			this.SetPOINavigatorEnabled(true);
			this._state = RefereeChaser.ChaseState.Roaming;
		}

		// Token: 0x060095B6 RID: 38326 RVA: 0x003D7754 File Offset: 0x003D5954
		private void EnterAudioTurning()
		{
			if (this.debugDetection)
			{
				Debug.Log("[RefereeChaser." + base.name + "] Roaming → AudioTurning");
			}
			this.SetPOINavigatorEnabled(false);
			if (this._follower != null)
			{
				this._follower.isStopped = true;
			}
			this._state = RefereeChaser.ChaseState.AudioTurning;
		}

		// Token: 0x060095B7 RID: 38327 RVA: 0x003D77AC File Offset: 0x003D59AC
		private void TickAudioTurning()
		{
			if (DetectionInteractionGate.IsAutonomousDetectionBlocked)
			{
				return;
			}
			if (this._senses == null)
			{
				this.ExitAudioTurning();
				return;
			}
			SensoryManager instance = SensoryManager.Instance;
			if (instance != null && this._senses.HasLineOfSight && !this._senses.BlocksFinalDetectionWhileSeated && !instance.DisableFinalDetection && !WardrobeComboRuntime.ShouldDisableFinalDetection(DetectionActorType.Referee, base.transform.position))
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

		// Token: 0x060095B8 RID: 38328 RVA: 0x003D78AC File Offset: 0x003D5AAC
		private void ExitAudioTurning()
		{
			if (this.debugDetection)
			{
				Debug.Log("[RefereeChaser." + base.name + "] AudioTurning → Roaming");
			}
			this._audioContactStartTime = -1f;
			if (this._follower != null)
			{
				this._follower.isStopped = false;
			}
			this.SetPOINavigatorEnabled(true);
			this._state = RefereeChaser.ChaseState.Roaming;
		}

		// Token: 0x060095B9 RID: 38329 RVA: 0x003D790E File Offset: 0x003D5B0E
		private void SetPOINavigatorEnabled(bool enabled)
		{
			if (this._poiNav != null)
			{
				this._poiNav.enabled = enabled;
			}
		}

		// Token: 0x04009ABF RID: 39615
		public const string SystemPresetId = "sys_Stadium_Referee_01";

		// Token: 0x04009AC1 RID: 39617
		[Header("Chase")]
		[Tooltip("Distanza dal player sotto la quale l'arbitro 'ti ha preso': triggera la detection finale via SensoryManager.")]
		public float catchDistance = 2f;

		// Token: 0x04009AC2 RID: 39618
		[Tooltip("Velocità (m/s) di corsa durante il chase. Override del FollowerEntity.maxSpeed.")]
		public float chaseSpeed = 3.5f;

		// Token: 0x04009AC3 RID: 39619
		[Tooltip("Frequenza (frame) di re-aim della destinazione durante il chase (il player si muove).")]
		public int chasingReaimEveryFrames = 10;

		// Token: 0x04009AC4 RID: 39620
		[Tooltip("Tempo (s) durante il chase in cui può non vedere il player prima di abbandonare. 0 = chase eterno.")]
		public float chaseLoseSightTimeout = 3f;

		// Token: 0x04009AC5 RID: 39621
		[Tooltip("Se true, all'abbandono del chase per timeout entra in cooldown (NPCSenses) per non re-detectare subito.")]
		public bool cooldownOnChaseEscape = true;

		// Token: 0x04009AC6 RID: 39622
		[Header("Audio Turn")]
		[Tooltip("Tempo (s) di HasAudioContact CONSECUTIVO prima che l'arbitro si giri verso il player.\nIl timer si azzera ogni volta che HasAudioContact diventa false.")]
		public float audioPersistenceTime = 3f;

		// Token: 0x04009AC7 RID: 39623
		[Tooltip("Velocità (deg/s) di rotazione verso il player quando l'audio persiste.")]
		public float audioTurnSpeed = 180f;

		// Token: 0x04009AC8 RID: 39624
		[Header("UI Detection Level")]
		[Tooltip("Distanza (m) oltre la quale, anche con LOS attiva, il livello-detection UI è 0.\nTipicamente coincide con BaseViewRadius di NPCSenses (15m).")]
		public float visualDetectionMaxRange = 15f;

		// Token: 0x04009AC9 RID: 39625
		[Header("Debug")]
		[SerializeField]
		private bool debugDetection;

		// Token: 0x04009ACA RID: 39626
		private RefereeChaser.ChaseState _state;

		// Token: 0x04009ACB RID: 39627
		private NPCSenses _senses;

		// Token: 0x04009ACC RID: 39628
		private AStarNPCController _ctrl;

		// Token: 0x04009ACD RID: 39629
		private FollowerEntity _follower;

		// Token: 0x04009ACE RID: 39630
		private SimplePOINavigator _poiNav;

		// Token: 0x04009ACF RID: 39631
		private AttentionUIController _attentionUI;

		// Token: 0x04009AD0 RID: 39632
		private float _baselineMaxSpeed;

		// Token: 0x04009AD1 RID: 39633
		private float _audioContactStartTime = -1f;

		// Token: 0x04009AD2 RID: 39634
		private float _lastChaseSightTime;

		// Token: 0x04009AD3 RID: 39635
		private float _chaseBlockedTime;

		// Token: 0x04009AD4 RID: 39636
		private bool _listenersRegistered;

		// Token: 0x04009AD5 RID: 39637
		private ExhibitionEventDirector _hookedDirector;

		// Token: 0x04009AD6 RID: 39638
		private bool _gameOverSuspended;

		// Token: 0x04009AD7 RID: 39639
		private int _deferredDirectorTransition;

		// Token: 0x04009AD8 RID: 39640
		private bool _simulationPauseActive;

		// Token: 0x04009AD9 RID: 39641
		private float _simulationPauseStartedAt;

		// Token: 0x020012DD RID: 4829
		private enum ChaseState
		{
			// Token: 0x04009ADB RID: 39643
			Roaming,
			// Token: 0x04009ADC RID: 39644
			Chasing,
			// Token: 0x04009ADD RID: 39645
			AudioTurning,
			// Token: 0x04009ADE RID: 39646
			Suspended
		}
	}
}
