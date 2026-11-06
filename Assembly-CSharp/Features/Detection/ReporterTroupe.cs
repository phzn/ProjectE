using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Bozo.AnimeCharacters;
using Features.Detection.Customization;
using Features.NPC;
using Features.RunModes;
using Features.TimeFreeze;
using Features.WardrobeCombos;
using Pathfinding;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Features.Detection
{
	// Token: 0x020012EE RID: 4846
	public class ReporterTroupe : MonoBehaviour, IDetectionUISource
	{
		// Token: 0x1400010C RID: 268
		// (add) Token: 0x0600964C RID: 38476 RVA: 0x003DAE88 File Offset: 0x003D9088
		// (remove) Token: 0x0600964D RID: 38477 RVA: 0x003DAEBC File Offset: 0x003D90BC
		public static event Action<ReporterTroupe> OnChaseStarted;

		// Token: 0x1400010D RID: 269
		// (add) Token: 0x0600964E RID: 38478 RVA: 0x003DAEF0 File Offset: 0x003D90F0
		// (remove) Token: 0x0600964F RID: 38479 RVA: 0x003DAF24 File Offset: 0x003D9124
		public static event Action<ReporterTroupe> OnChaseEscaped;

		// Token: 0x17001800 RID: 6144
		// (get) Token: 0x06009650 RID: 38480 RVA: 0x003DAF57 File Offset: 0x003D9157
		// (set) Token: 0x06009651 RID: 38481 RVA: 0x003DAF5F File Offset: 0x003D915F
		public TroupeState CurrentState { get; private set; }

		// Token: 0x17001801 RID: 6145
		// (get) Token: 0x06009652 RID: 38482 RVA: 0x003DAF68 File Offset: 0x003D9168
		public bool HasUnresolvedRunThreat
		{
			get
			{
				return this.CurrentState == TroupeState.Chasing || this.CurrentState == TroupeState.Engaging || this._audioTurnActive;
			}
		}

		// Token: 0x17001802 RID: 6146
		// (get) Token: 0x06009653 RID: 38483 RVA: 0x003DAF84 File Offset: 0x003D9184
		public NPCSenses Initiator
		{
			get
			{
				return this._initiator;
			}
		}

		// Token: 0x17001803 RID: 6147
		// (get) Token: 0x06009654 RID: 38484 RVA: 0x003DAF8C File Offset: 0x003D918C
		public IReadOnlyList<NPCSenses> AdditionalActors
		{
			get
			{
				return this._additionalActors;
			}
		}

		// Token: 0x17001804 RID: 6148
		// (get) Token: 0x06009655 RID: 38485 RVA: 0x003DAF94 File Offset: 0x003D9194
		public NPCGroupConfigSO Config
		{
			get
			{
				return this._config;
			}
		}

		// Token: 0x17001805 RID: 6149
		// (get) Token: 0x06009656 RID: 38486 RVA: 0x003DAF9C File Offset: 0x003D919C
		public bool VisualActive
		{
			get
			{
				return this._initiator != null && this._initiator.HasLineOfSight;
			}
		}

		// Token: 0x17001806 RID: 6150
		// (get) Token: 0x06009657 RID: 38487 RVA: 0x003DAFB9 File Offset: 0x003D91B9
		public bool AudioActive
		{
			get
			{
				return this._initiator != null && this._initiator.HasAudioContact;
			}
		}

		// Token: 0x17001807 RID: 6151
		// (get) Token: 0x06009658 RID: 38488 RVA: 0x003DAFD8 File Offset: 0x003D91D8
		public float DetectionLevel
		{
			get
			{
				if (this._initiator == null || !this._initiator.HasLineOfSight)
				{
					return 0f;
				}
				SensoryManager instance = SensoryManager.Instance;
				StealthActor stealthActor = ((instance != null) ? instance.GetPlayer() : null);
				if (stealthActor == null)
				{
					return 0f;
				}
				float num = Vector3.Distance(this._initiator.transform.position, stealthActor.Position);
				float num2 = Mathf.Max(this.visualDetectionMaxRange, this.encounterTriggerDistance + 0.01f);
				if (num <= this.encounterTriggerDistance)
				{
					return 1f;
				}
				if (num >= num2)
				{
					return 0f;
				}
				return Mathf.Clamp01((num2 - num) / (num2 - this.encounterTriggerDistance));
			}
		}

		// Token: 0x06009659 RID: 38489 RVA: 0x003DB08C File Offset: 0x003D928C
		public void Initialize(NPCSenses initiator, List<NPCSenses> additionalActors, NPCGroupConfigSO config, TVStudioController tvStudio = null)
		{
			this._initiator = initiator;
			this._additionalActors = additionalActors ?? new List<NPCSenses>();
			this._config = config;
			this._tvStudio = tvStudio;
			this._encounterTriggered = false;
			this.CacheBaselineSpeeds();
			if (this._initiator != null)
			{
				AttentionUIController component = this._initiator.GetComponent<AttentionUIController>();
				if (component != null)
				{
					component.SetTroupe(this);
				}
			}
			this.CurrentState = TroupeState.Roaming;
			this.EnterState(TroupeState.Roaming);
		}

		// Token: 0x0600965A RID: 38490 RVA: 0x003DB104 File Offset: 0x003D9304
		private void CacheBaselineSpeeds()
		{
			this._baselineMaxSpeed.Clear();
			this.CacheBaseline(this._initiator);
			foreach (NPCSenses npcsenses in this._additionalActors)
			{
				this.CacheBaseline(npcsenses);
			}
		}

		// Token: 0x0600965B RID: 38491 RVA: 0x003DB170 File Offset: 0x003D9370
		private void CacheBaseline(NPCSenses npc)
		{
			if (npc == null)
			{
				return;
			}
			FollowerEntity component = npc.GetComponent<FollowerEntity>();
			if (component != null)
			{
				this._baselineMaxSpeed[npc] = component.maxSpeed;
			}
		}

		// Token: 0x0600965C RID: 38492 RVA: 0x003DB1AC File Offset: 0x003D93AC
		public void OnMemberDetected(NPCSenses detector)
		{
			if (DetectionInteractionGate.IsAutonomousDetectionBlocked)
			{
				return;
			}
			if (!RunModeService.EncountersEnabled)
			{
				return;
			}
			if (PlayerStealthBridge.IsFullyHidden || TimeFreezeController.IsAnyActive || NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				return;
			}
			if (this.CurrentState != TroupeState.Roaming)
			{
				return;
			}
			if (this._encounterTriggered)
			{
				return;
			}
			NPCSenses initiator = this._initiator;
			if (initiator != null && initiator.IsInCooldown)
			{
				return;
			}
			if (ExhibitionEventDirector.Instance == null)
			{
				return;
			}
			if (ExhibitionEventDirector.Instance.IsEventActive)
			{
				return;
			}
			if (WardrobeComboRuntime.ShouldIgnore(DetectionActorType.Reporter))
			{
				return;
			}
			if (this.initiatorOnlyDetection && detector != this._initiator)
			{
				return;
			}
			if (!detector.HasLineOfSight)
			{
				return;
			}
			if (detector.BlocksFinalDetectionWhileSeated)
			{
				return;
			}
			if (WardrobeComboRuntime.ShouldDisableFinalDetection(DetectionActorType.Reporter, detector.transform.position))
			{
				return;
			}
			if (SensoryManager.Instance != null && SensoryManager.Instance.DisableFinalDetection)
			{
				return;
			}
			SensoryManager instance = SensoryManager.Instance;
			StealthActor stealthActor = ((instance != null) ? instance.GetPlayer() : null);
			float num = ((stealthActor != null) ? Vector3.Distance(detector.transform.position, stealthActor.Position) : float.PositiveInfinity);
			this._encounterTriggered = true;
			this.TransitionTo((num <= this.encounterTriggerDistance) ? TroupeState.Engaging : TroupeState.Chasing);
		}

		// Token: 0x0600965D RID: 38493 RVA: 0x003DB2E0 File Offset: 0x003D94E0
		private void TransitionTo(TroupeState next)
		{
			Debug.Log(string.Format("[ReporterTroupe] {0}: {1} → {2}", base.name, this.CurrentState, next));
			this.ExitState(this.CurrentState);
			this.CurrentState = next;
			this.EnterState(next);
		}

		// Token: 0x0600965E RID: 38494 RVA: 0x003DB32D File Offset: 0x003D952D
		private void Start()
		{
			this.TryHookDirector();
		}

		// Token: 0x0600965F RID: 38495 RVA: 0x003DB335 File Offset: 0x003D9535
		private void OnEnable()
		{
			this._simulationPauseActive = false;
			NPCSimulationPause.PauseChanged += this.HandleSimulationPauseChanged;
			NPCSimulationPause.RegisterParticipant(this);
			this.HandleSimulationPauseChanged(NPCSimulationPause.IsPaused);
		}

		// Token: 0x06009660 RID: 38496 RVA: 0x003DB360 File Offset: 0x003D9560
		private void OnDisable()
		{
			NPCSimulationPause.PauseChanged -= this.HandleSimulationPauseChanged;
			this.UnhookDirector();
			if (this.CurrentState != TroupeState.Disbanding)
			{
				this.RestoreEncounterMovement();
			}
		}

		// Token: 0x06009661 RID: 38497 RVA: 0x003DB388 File Offset: 0x003D9588
		private void OnDestroy()
		{
			this.UnhookDirector();
		}

		// Token: 0x06009662 RID: 38498 RVA: 0x003DB390 File Offset: 0x003D9590
		private void Update()
		{
			if (DetectionInteractionGate.IsGameOver)
			{
				if (!this._gameOverSuspended)
				{
					this._gameOverSuspended = true;
					this._resumeAfterDirector = false;
					this.HoldEncounterMovement();
				}
				return;
			}
			if (this._gameOverSuspended)
			{
				this._gameOverSuspended = false;
				this.RestoreEncounterMovement();
				if (this.CurrentState != TroupeState.Disbanding)
				{
					this.TransitionTo(TroupeState.Roaming);
				}
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
			if (this._resumeAfterDirector)
			{
				this._resumeAfterDirector = false;
				this.ResumeAfterDirector();
			}
			this.TryHookDirector();
			ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
			if (this.CurrentState == TroupeState.Engaging && (instance == null || !instance.IsEventActive))
			{
				this.RestoreEncounterMovement();
				this.TransitionTo(TroupeState.Roaming);
			}
			if (this._externalDirectorActive || (this.CurrentState == TroupeState.Engaging && instance != null && instance.IsEventActive))
			{
				this.HoldEncounterMovement();
				return;
			}
			if (WardrobeComboRuntime.ShouldIgnore(DetectionActorType.Reporter) || !RunModeService.EncountersEnabled)
			{
				if (this.CurrentState == TroupeState.Chasing)
				{
					this.TransitionTo(TroupeState.Roaming);
				}
				return;
			}
			TroupeState currentState = this.CurrentState;
			if (currentState == TroupeState.Roaming)
			{
				this.UpdateRoamingDetection();
				this.UpdateRoamingFormation();
				return;
			}
			if (currentState != TroupeState.Chasing)
			{
				return;
			}
			this.UpdateChasing();
		}

		// Token: 0x06009663 RID: 38499 RVA: 0x003DB4CC File Offset: 0x003D96CC
		private void HandleSimulationPauseChanged(bool paused)
		{
			if (paused)
			{
				this.BeginSimulationPauseIfNeeded();
				return;
			}
			this.ResumeSimulationIfNeeded();
		}

		// Token: 0x06009664 RID: 38500 RVA: 0x003DB4DE File Offset: 0x003D96DE
		private void BeginSimulationPauseIfNeeded()
		{
			if (this._simulationPauseActive)
			{
				return;
			}
			this._simulationPauseActive = true;
			this._simulationPauseStartedAt = Time.time;
		}

		// Token: 0x06009665 RID: 38501 RVA: 0x003DB4FB File Offset: 0x003D96FB
		private void ResumeSimulationIfNeeded()
		{
			if (!this._simulationPauseActive)
			{
				return;
			}
			this.ShiftSimulationTimestamps(Mathf.Max(0f, Time.time - this._simulationPauseStartedAt));
			this._simulationPauseActive = false;
		}

		// Token: 0x06009666 RID: 38502 RVA: 0x003DB529 File Offset: 0x003D9729
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
			if (this.CurrentState == TroupeState.Chasing)
			{
				this._lastChaseSightTime += amount;
			}
		}

		// Token: 0x06009667 RID: 38503 RVA: 0x003DB568 File Offset: 0x003D9768
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
			if (instance != null && instance.IsEventActive && instance.DetectingNPC != this._initiator)
			{
				this._externalDirectorActive = true;
				return;
			}
			if (this._externalDirectorActive)
			{
				this.HandleDirectorComplete();
			}
		}

		// Token: 0x06009668 RID: 38504 RVA: 0x003DB628 File Offset: 0x003D9828
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

		// Token: 0x06009669 RID: 38505 RVA: 0x003DB69A File Offset: 0x003D989A
		private void HandleDirectorStarted(NPCSenses initiator)
		{
			this.HoldEncounterMovement();
			if (initiator != null && initiator == this._initiator)
			{
				return;
			}
			this._externalDirectorActive = true;
		}

		// Token: 0x0600966A RID: 38506 RVA: 0x003DB6C1 File Offset: 0x003D98C1
		private void HandleDirectorComplete()
		{
			this._externalDirectorActive = false;
			if (NPCSimulationPause.IsPaused || NPCSimulationPause.SimulationScale <= 0.0001f)
			{
				this._resumeAfterDirector = true;
				return;
			}
			this.ResumeAfterDirector();
		}

		// Token: 0x0600966B RID: 38507 RVA: 0x003DB6EC File Offset: 0x003D98EC
		private void ResumeAfterDirector()
		{
			if (DetectionInteractionGate.IsGameOver)
			{
				return;
			}
			if (this.CurrentState == TroupeState.Engaging || this.CurrentState == TroupeState.Disbanding)
			{
				return;
			}
			this.RestoreEncounterMovement();
			this.ForEachMember(delegate(NPCSenses npc)
			{
				if (npc == null)
				{
					return;
				}
				FollowerEntity component = npc.GetComponent<FollowerEntity>();
				if (component != null)
				{
					component.isStopped = false;
				}
			});
		}

		// Token: 0x0600966C RID: 38508 RVA: 0x003DB740 File Offset: 0x003D9940
		private void EnterState(TroupeState state)
		{
			switch (state)
			{
			case TroupeState.Roaming:
				this._encounterTriggered = false;
				this._audioTurnActive = false;
				this._audioContactStartTime = -1f;
				this.ConfigureRoamingRoles();
				return;
			case TroupeState.Chasing:
			{
				Action<ReporterTroupe> onChaseStarted = ReporterTroupe.OnChaseStarted;
				if (onChaseStarted != null)
				{
					onChaseStarted(this);
				}
				ReporterTroupe.SetPOINavigatorEnabled(this._initiator, false);
				this.SendTroupeRunningTowardPlayer();
				this._lastChaseSightTime = Time.time;
				this.TriggerChasingDialogue();
				return;
			}
			case TroupeState.Engaging:
			{
				if (DetectionInteractionGate.IsAutonomousDetectionBlocked || ExhibitionEventDirector.Instance == null || !RunModeService.EncountersEnabled || ExhibitionEventDirector.Instance.IsEventActive || PlayerStealthBridge.IsFullyHidden || this._initiator == null || WardrobeComboRuntime.ShouldDisableFinalDetection(DetectionActorType.Reporter, this._initiator.transform.position))
				{
					if (this.debugDetection)
					{
						Debug.Log("[ReporterTroupe] Encounter unavailable, remaining in Chasing.");
					}
					this.CurrentState = TroupeState.Chasing;
					return;
				}
				if (this._tvStudio == null)
				{
					this._tvStudio = Object.FindObjectOfType<TVStudioController>(true);
				}
				ExhibitionEventDirector instance = ExhibitionEventDirector.Instance;
				EncounterRequest encounterRequest = new EncounterRequest();
				encounterRequest.initiator = this._initiator;
				encounterRequest.additionalActors = this._additionalActors;
				NPCGroupConfigSO config = this._config;
				encounterRequest.profile = ((config != null) ? config.encounterProfile : null);
				encounterRequest.onSceneEnded = new Action(this.OnSceneEnded);
				encounterRequest.completeUnderFade = true;
				encounterRequest.preReactionPhase = this._tvStudio;
				encounterRequest.postEmoteMode = this._tvStudio;
				instance.TriggerEncounter(encounterRequest);
				if (!ExhibitionEventDirector.Instance.IsEventActive)
				{
					this.CurrentState = TroupeState.Chasing;
					return;
				}
				break;
			}
			case TroupeState.Disbanding:
			{
				Action<ReporterTroupe> onTroupeDisbanding = this.OnTroupeDisbanding;
				if (onTroupeDisbanding == null)
				{
					return;
				}
				onTroupeDisbanding(this);
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x0600966D RID: 38509 RVA: 0x003DB8D8 File Offset: 0x003D9AD8
		private void UpdateChasing()
		{
			if (DetectionInteractionGate.IsAutonomousDetectionBlocked)
			{
				return;
			}
			if (this._initiator == null)
			{
				return;
			}
			SensoryManager instance = SensoryManager.Instance;
			if (instance == null)
			{
				return;
			}
			StealthActor player = instance.GetPlayer();
			if (player == null)
			{
				return;
			}
			if (this._initiator.HasLineOfSight && !PlayerStealthBridge.IsFullyHidden)
			{
				this._lastChaseSightTime = Time.time;
			}
			float num = Vector3.Distance(this._initiator.transform.position, player.Position);
			if (this.chaseLoseSightTimeout > 0f && Time.time - this._lastChaseSightTime > this.chaseLoseSightTimeout)
			{
				if (this.debugDetection)
				{
					Debug.Log(string.Format("[ReporterTroupe.{0}] Chasing → Roaming (lost sight per {1}s)", base.name, this.chaseLoseSightTimeout));
				}
				if (this.cooldownOnChaseEscape)
				{
					this._initiator.StartCooldown();
				}
				Action<ReporterTroupe> onChaseEscaped = ReporterTroupe.OnChaseEscaped;
				if (onChaseEscaped != null)
				{
					onChaseEscaped(this);
				}
				this._encounterTriggered = false;
				this.TransitionTo(TroupeState.Roaming);
				return;
			}
			if (this.chasingReaimEveryFrames > 0 && Time.frameCount % this.chasingReaimEveryFrames == 0)
			{
				this.SendTroupeRunningTowardPlayer();
			}
			if (this.debugDetection && Time.frameCount % 30 == 0)
			{
				float num2 = Time.time - this._lastChaseSightTime;
				Debug.Log(string.Format("[ReporterTroupe.{0}] Chasing: dist={1:F1}m / trigger={2:F1}m, ", base.name, num, this.encounterTriggerDistance) + string.Format("sinceSight={0:F1}s / timeout={1:F1}s, LOS={2}", num2, this.chaseLoseSightTimeout, this._initiator.HasLineOfSight));
			}
			if (num <= this.encounterTriggerDistance && RunModeService.EncountersEnabled && !PlayerStealthBridge.IsFullyHidden && !TimeFreezeController.IsAnyActive && !WardrobeComboRuntime.ShouldDisableFinalDetection(DetectionActorType.Reporter, this._initiator.transform.position))
			{
				ExhibitionEventDirector instance2 = ExhibitionEventDirector.Instance;
				if (instance2 == null || instance2.IsEventActive)
				{
					if (this.debugDetection && Time.frameCount % 60 == 0)
					{
						Debug.Log("[ReporterTroupe." + base.name + "] Chasing in attesa: director occupato.");
					}
					return;
				}
				if (this.debugDetection)
				{
					Debug.Log(string.Format("[ReporterTroupe.{0}] Chasing → Engaging (dist={1:F1}m)", base.name, num));
				}
				this.TransitionTo(TroupeState.Engaging);
			}
		}

		// Token: 0x0600966E RID: 38510 RVA: 0x003DBB1C File Offset: 0x003D9D1C
		private void ExitState(TroupeState state)
		{
			if (state == TroupeState.Chasing)
			{
				SimpleDialogueManager instance = SimpleDialogueManager.Instance;
				if (instance != null && instance.IsDialogueActive)
				{
					instance.ForceEndDialogue();
				}
			}
		}

		// Token: 0x0600966F RID: 38511 RVA: 0x003DBB4C File Offset: 0x003D9D4C
		private void TriggerChasingDialogue()
		{
			SimpleDialogueManager instance = SimpleDialogueManager.Instance;
			if (instance == null || instance.IsDialogueActive)
			{
				return;
			}
			NPCGroupConfigSO config = this._config;
			NPCDialogueProfileSO npcdialogueProfileSO;
			if (config == null)
			{
				npcdialogueProfileSO = null;
			}
			else
			{
				EncounterProfileSO encounterProfile = config.encounterProfile;
				npcdialogueProfileSO = ((encounterProfile != null) ? encounterProfile.dialogueOverride : null);
			}
			NPCDialogueProfileSO npcdialogueProfileSO2 = npcdialogueProfileSO;
			NPCDialogueHolder npcdialogueHolder = ((this._initiator != null) ? this._initiator.GetComponent<NPCDialogueHolder>() : null);
			DialogueLine[] array = DialogueResolver.Resolve((npcdialogueHolder != null) ? npcdialogueHolder.NpcIdentityId : null, (npcdialogueHolder != null) ? npcdialogueHolder.Personality : PersonalityType.Neutral, (npcdialogueHolder != null) ? npcdialogueHolder.Gender : Gender.Male, DialogueCategory.Chasing, DialogueActorGroup.Journalists, npcdialogueProfileSO2);
			if (array == null || array.Length == 0)
			{
				return;
			}
			string text = ((npcdialogueHolder != null && !string.IsNullOrWhiteSpace(npcdialogueHolder.FullName)) ? npcdialogueHolder.FullName : ((npcdialogueHolder != null) ? npcdialogueHolder.FirstName : null));
			bool flag = !string.IsNullOrWhiteSpace(text);
			if (!flag)
			{
				text = ((npcdialogueProfileSO2 != null) ? npcdialogueProfileSO2.npcDisplayName : "Reporter");
			}
			foreach (DialogueLine dialogueLine in array)
			{
				dialogueLine.speakerName = text;
				dialogueLine.speakerLocalizationScope = (flag ? null : "dialogue_speaker");
			}
			instance.StartAutoDialogue(array, this.chasingDialogueDwellDuration, null);
		}

		// Token: 0x06009670 RID: 38512 RVA: 0x003DBC90 File Offset: 0x003D9E90
		private void OnSceneEnded()
		{
			if (this.CurrentState == TroupeState.Disbanding)
			{
				return;
			}
			this.TransitionTo(TroupeState.Disbanding);
		}

		// Token: 0x06009671 RID: 38513 RVA: 0x003DBCA3 File Offset: 0x003D9EA3
		private void HoldEncounterMovement()
		{
			this.ForEachMember(delegate(NPCSenses npc)
			{
				if (npc == null)
				{
					return;
				}
				FollowerEntity component = npc.GetComponent<FollowerEntity>();
				if (component == null || !component.entityExists)
				{
					return;
				}
				AStarNPCController component2 = npc.GetComponent<AStarNPCController>();
				if (!this._encounterMovement.ContainsKey(npc))
				{
					this._encounterMovement.Add(npc, new ValueTuple<bool, bool>(component.simulateMovement, component2 != null && component2.IsNavigationLocked));
				}
				if (component2 != null)
				{
					component2.StopAndLock();
				}
				component.isStopped = true;
				if (component.simulateMovement)
				{
					component.simulateMovement = false;
				}
			});
		}

		// Token: 0x06009672 RID: 38514 RVA: 0x003DBCB8 File Offset: 0x003D9EB8
		private void RestoreEncounterMovement()
		{
			foreach (KeyValuePair<NPCSenses, ValueTuple<bool, bool>> keyValuePair in this._encounterMovement)
			{
				if (!(keyValuePair.Key == null))
				{
					FollowerEntity component = keyValuePair.Key.GetComponent<FollowerEntity>();
					if (component != null && component.entityExists)
					{
						component.simulateMovement = keyValuePair.Value.Item1;
					}
					AStarNPCController component2 = keyValuePair.Key.GetComponent<AStarNPCController>();
					if (component2 != null && !keyValuePair.Value.Item2)
					{
						component2.UnlockNavigation(false);
					}
				}
			}
			this._encounterMovement.Clear();
		}

		// Token: 0x06009673 RID: 38515 RVA: 0x003DBD7C File Offset: 0x003D9F7C
		private void ConfigureRoamingRoles()
		{
			this.RestoreRoamingMovement(this._initiator);
			ReporterTroupe.SetSensesEnabled(this._initiator, true);
			ReporterTroupe.SetAttentionUIEnabled(this._initiator, true);
			ReporterTroupe.SetPOINavigatorEnabled(this._initiator, true);
			foreach (NPCSenses npcsenses in this._additionalActors)
			{
				this.RestoreRoamingMovement(npcsenses);
				ReporterTroupe.SetSensesEnabled(npcsenses, false);
				ReporterTroupe.SetAttentionUIEnabled(npcsenses, false);
				ReporterTroupe.SetPOINavigatorEnabled(npcsenses, false);
			}
		}

		// Token: 0x06009674 RID: 38516 RVA: 0x002AAC2B File Offset: 0x002A8E2B
		private static void SetSensesEnabled(NPCSenses npc, bool enabled)
		{
			if (npc == null)
			{
				return;
			}
			if (NPCPoolRecovery.DeferSenses(npc, enabled))
			{
				return;
			}
			npc.enabled = enabled;
		}

		// Token: 0x06009675 RID: 38517 RVA: 0x003DBE14 File Offset: 0x003DA014
		private static void SetAttentionUIEnabled(NPCSenses npc, bool enabled)
		{
			if (npc == null)
			{
				return;
			}
			AttentionUIController component = npc.GetComponent<AttentionUIController>();
			if (component != null)
			{
				component.enabled = enabled;
			}
		}

		// Token: 0x06009676 RID: 38518 RVA: 0x003DBE44 File Offset: 0x003DA044
		private static void SetPOINavigatorEnabled(NPCSenses npc, bool enabled)
		{
			if (npc == null)
			{
				return;
			}
			if (NPCPoolRecovery.DeferNavigator(npc, enabled))
			{
				return;
			}
			SimplePOINavigator component = npc.GetComponent<SimplePOINavigator>();
			if (component != null)
			{
				component.enabled = enabled;
			}
		}

		// Token: 0x06009677 RID: 38519 RVA: 0x003DBE7C File Offset: 0x003DA07C
		private void UpdateRoamingFormation()
		{
			if (this._initiator == null || this._additionalActors == null || this._additionalActors.Count == 0)
			{
				return;
			}
			Vector3 position = this._initiator.transform.position;
			Vector3 forward = this._initiator.transform.forward;
			Vector3 right = this._initiator.transform.right;
			forward.y = 0f;
			forward.Normalize();
			right.y = 0f;
			right.Normalize();
			for (int i = 0; i < this._additionalActors.Count; i++)
			{
				NPCSenses npcsenses = this._additionalActors[i];
				if (!(npcsenses == null))
				{
					float num = ((i % 2 == 0) ? (-1f) : 1f);
					int num2 = i / 2 + 1;
					Vector3 vector = -forward * ((float)num2 * this.roamingFollowDistance) + right * (num * this.roamingLateralSpacing);
					Vector3 vector2 = position + vector;
					AStarNPCController component = npcsenses.GetComponent<AStarNPCController>();
					if (!(component == null) && !component.IsNavigationLocked && !NPCPoolRecovery.IsActorRecovering(npcsenses))
					{
						FollowerEntity component2 = npcsenses.GetComponent<FollowerEntity>();
						if (!(component2 == null))
						{
							if (Vector3.Distance(component2.destination, vector2) > this.roamingFormationUpdateThreshold)
							{
								component.SetDestination(vector2);
							}
							if (Vector3.Distance(npcsenses.transform.position, vector2) <= this.roamingFaceLeaderDistance)
							{
								component2.isStopped = true;
								Vector3 vector3 = position - npcsenses.transform.position;
								vector3.y = 0f;
								if (vector3.sqrMagnitude > 0.0001f)
								{
									Quaternion quaternion = Quaternion.LookRotation(vector3.normalized, Vector3.up);
									npcsenses.transform.rotation = Quaternion.RotateTowards(npcsenses.transform.rotation, quaternion, this.roamingFaceLeaderSpeed * NPCSimulationPause.SimulationDeltaTime);
								}
							}
							else
							{
								component2.isStopped = false;
							}
						}
					}
				}
			}
		}

		// Token: 0x06009678 RID: 38520 RVA: 0x003DC088 File Offset: 0x003DA288
		private void UpdateRoamingDetection()
		{
			if (DetectionInteractionGate.IsAutonomousDetectionBlocked)
			{
				return;
			}
			if (TimeFreezeController.IsAnyActive)
			{
				return;
			}
			if (PlayerStealthBridge.IsFullyHidden)
			{
				this._audioContactStartTime = -1f;
				if (this._audioTurnActive)
				{
					this.EndAudioTurn();
				}
				return;
			}
			if (this._initiator == null || this._encounterTriggered)
			{
				return;
			}
			if (this._initiator.IsInCooldown)
			{
				return;
			}
			SensoryManager instance = SensoryManager.Instance;
			if (instance == null)
			{
				if (this.debugDetection && Time.frameCount % 120 == 0)
				{
					Debug.LogWarning("[ReporterTroupe] SensoryManager.Instance is null — detection inattiva");
				}
				return;
			}
			if (this.debugDetection && Time.frameCount % 60 == 0)
			{
				StealthActor player = instance.GetPlayer();
				if (player != null)
				{
					Vector3 vector = player.Position - this._initiator.transform.position;
					float magnitude = vector.magnitude;
					vector.y = 0f;
					Vector3 forward = this._initiator.transform.forward;
					forward.y = 0f;
					float num = Mathf.Acos(Mathf.Clamp(Vector3.Dot(forward.normalized, vector.normalized), -1f, 1f)) * 57.29578f;
					bool flag = num <= this._initiator.EffectiveViewAngle * 0.5f;
					bool flag2 = magnitude <= this._initiator.EffectiveViewRadius;
					float num2 = ((this.audioTurnMaxRange > 0f) ? Mathf.Clamp01(1f - magnitude / this.audioTurnMaxRange) : 0f);
					Debug.Log(string.Format("[ReporterTroupe.{0}] LOS={1} Audio={2} ", base.name, this._initiator.HasLineOfSight, this._initiator.HasAudioContact) + string.Format("Att={0:F2} dist={1:F1}m angle={2:F0}° inFOV={3} inRange={4} ", new object[]
					{
						this._initiator.CurrentAttention,
						magnitude,
						num,
						flag,
						flag2
					}) + string.Format("myAudioLvl={0:F2}/{1:F2} audioTurnActive={2} ", num2, this.audioTurnThreshold, this._audioTurnActive) + string.Format("disableFinal={0}", instance.DisableFinalDetection));
				}
			}
			if (this._initiator.HasLineOfSight && RunModeService.EncountersEnabled && !this._initiator.BlocksFinalDetectionWhileSeated && !instance.DisableFinalDetection && !WardrobeComboRuntime.ShouldDisableFinalDetection(DetectionActorType.Reporter, this._initiator.transform.position))
			{
				StealthActor player2 = instance.GetPlayer();
				if (player2 != null)
				{
					float num3 = Vector3.Distance(this._initiator.transform.position, player2.Position);
					if (this._audioTurnActive)
					{
						this.EndAudioTurn();
					}
					if (num3 <= this.encounterTriggerDistance)
					{
						if (this.debugDetection)
						{
							Debug.Log(string.Format("[ReporterTroupe.{0}] LOS+close ({1:F1}m ≤ {2:F1}m) → Engaging diretto", base.name, num3, this.encounterTriggerDistance));
						}
						this._encounterTriggered = true;
						this.TransitionTo(TroupeState.Engaging);
						return;
					}
					if (this.debugDetection)
					{
						Debug.Log(string.Format("[ReporterTroupe.{0}] LOS+far ({1:F1}m > {2:F1}m) → Chasing", base.name, num3, this.encounterTriggerDistance));
					}
					this._encounterTriggered = true;
					this.TransitionTo(TroupeState.Chasing);
					return;
				}
			}
			if (this._initiator.HasAudioContact)
			{
				if (this._audioContactStartTime < 0f)
				{
					this._audioContactStartTime = Time.time;
				}
				float num4 = Time.time - this._audioContactStartTime;
				if (num4 >= this.audioPersistenceTime)
				{
					StealthActor player3 = instance.GetPlayer();
					if (player3 != null)
					{
						this.BeginOrUpdateAudioTurn(player3.Position);
						return;
					}
				}
				else if (this.debugDetection && Time.frameCount % 60 == 0)
				{
					Debug.Log(string.Format("[ReporterTroupe.{0}] Audio building: {1:F1}s / {2:F1}s", base.name, num4, this.audioPersistenceTime));
				}
			}
			else
			{
				this._audioContactStartTime = -1f;
			}
			if (this._audioTurnActive)
			{
				this.EndAudioTurn();
			}
		}

		// Token: 0x06009679 RID: 38521 RVA: 0x003DC4A0 File Offset: 0x003DA6A0
		private void BeginOrUpdateAudioTurn(Vector3 playerPos)
		{
			if (!this._audioTurnActive)
			{
				this._audioTurnActive = true;
				if (this.debugDetection)
				{
					Debug.Log(string.Format("[ReporterTroupe.{0}] BeginAudioTurn → ruoto verso player @ {1}", base.name, playerPos));
				}
				ReporterTroupe.SetPOINavigatorEnabled(this._initiator, false);
				FollowerEntity component = this._initiator.GetComponent<FollowerEntity>();
				if (component != null)
				{
					component.isStopped = true;
				}
			}
			Vector3 vector = playerPos - this._initiator.transform.position;
			vector.y = 0f;
			if (vector.sqrMagnitude < 0.0001f)
			{
				return;
			}
			Quaternion quaternion = Quaternion.LookRotation(vector.normalized, Vector3.up);
			this._initiator.transform.rotation = Quaternion.RotateTowards(this._initiator.transform.rotation, quaternion, this.audioTurnSpeed * NPCSimulationPause.SimulationDeltaTime);
		}

		// Token: 0x0600967A RID: 38522 RVA: 0x003DC580 File Offset: 0x003DA780
		private void EndAudioTurn()
		{
			this._audioTurnActive = false;
			if (this._initiator == null)
			{
				return;
			}
			if (this.debugDetection)
			{
				Debug.Log("[ReporterTroupe." + base.name + "] EndAudioTurn → riprendo POI navigator");
			}
			FollowerEntity component = this._initiator.GetComponent<FollowerEntity>();
			if (component != null)
			{
				component.isStopped = false;
			}
			ReporterTroupe.SetPOINavigatorEnabled(this._initiator, true);
		}

		// Token: 0x0600967B RID: 38523 RVA: 0x003DC5F0 File Offset: 0x003DA7F0
		private void RestoreRoamingMovement(NPCSenses npc)
		{
			if (npc == null)
			{
				return;
			}
			FollowerEntity component = npc.GetComponent<FollowerEntity>();
			AStarNPCController component2 = npc.GetComponent<AStarNPCController>();
			if (component != null)
			{
				component.isStopped = false;
			}
			if (component2 != null)
			{
				component2.ClearSpeedOverride();
			}
			float num;
			if (component != null && this._baselineMaxSpeed.TryGetValue(npc, out num))
			{
				component.maxSpeed = num;
			}
		}

		// Token: 0x0600967C RID: 38524 RVA: 0x003DC64F File Offset: 0x003DA84F
		private void StopAllMovement()
		{
			this.ForEachMember(delegate(NPCSenses npc)
			{
				if (npc == null)
				{
					return;
				}
				FollowerEntity component = npc.GetComponent<FollowerEntity>();
				if (component != null)
				{
					component.isStopped = true;
				}
			});
		}

		// Token: 0x0600967D RID: 38525 RVA: 0x003DC678 File Offset: 0x003DA878
		private void SendTroupeRunningTowardPlayer()
		{
			SensoryManager instance = SensoryManager.Instance;
			StealthActor stealthActor = ((instance != null) ? instance.GetPlayer() : null);
			if (stealthActor == null)
			{
				Debug.LogWarning("[ReporterTroupe] SensoryManager player null, fallback a stop.");
				this.StopAllMovement();
				return;
			}
			Vector3 position = stealthActor.Position;
			int num = 1 + this._additionalActors.Count;
			int num2 = 0;
			this.ApplyRunDestination(this._initiator, position, num2, num);
			num2++;
			foreach (NPCSenses npcsenses in this._additionalActors)
			{
				this.ApplyRunDestination(npcsenses, position, num2, num);
				num2++;
			}
		}

		// Token: 0x0600967E RID: 38526 RVA: 0x003DC73C File Offset: 0x003DA93C
		private void ApplyRunDestination(NPCSenses npc, Vector3 playerPos, int memberIndex, int total)
		{
			if (npc == null || NPCPoolRecovery.IsActorRecovering(npc))
			{
				return;
			}
			FollowerEntity component = npc.GetComponent<FollowerEntity>();
			if (component == null || !component.enabled || !component.entityExists)
			{
				return;
			}
			Vector3 vector = playerPos - npc.transform.position;
			vector.y = 0f;
			Vector3 vector2 = ((vector.sqrMagnitude > 0.001f) ? vector.normalized : Vector3.forward);
			Vector3 vector3 = playerPos - vector2 * this.approachStopDistance;
			float num = 360f / (float)Mathf.Max(1, total) * (float)memberIndex;
			Vector3 vector4 = Quaternion.LookRotation(vector2) * (Quaternion.Euler(0f, num, 0f) * Vector3.forward) * this.formationRadius;
			Vector3 vector5 = vector3 + vector4;
			if (!NPCPoolRecovery.TryConstrainDestination(npc.transform, ref vector5))
			{
				AStarNPCController component2 = npc.GetComponent<AStarNPCController>();
				if (component2 == null)
				{
					return;
				}
				component2.Stop();
				return;
			}
			else
			{
				if (component != null)
				{
					component.isStopped = false;
					component.maxSpeed = this.engagingRunSpeed;
					component.SetDestination(vector5, default(float3));
				}
				AStarNPCController component3 = npc.GetComponent<AStarNPCController>();
				if (component3 == null)
				{
					return;
				}
				component3.SetSpeedOverride(this.engagingRunSpeed);
				return;
			}
		}

		// Token: 0x0600967F RID: 38527 RVA: 0x003DC880 File Offset: 0x003DAA80
		private void ForEachMember(Action<NPCSenses> action)
		{
			action(this._initiator);
			if (this._additionalActors != null)
			{
				foreach (NPCSenses npcsenses in this._additionalActors)
				{
					action(npcsenses);
				}
			}
		}

		// Token: 0x04009B6D RID: 39789
		[Header("Approach Behavior")]
		[Tooltip("Distanza dal player a cui i membri della troupe si fermano in Engaging.")]
		public float approachStopDistance = 2f;

		// Token: 0x04009B6E RID: 39790
		[Tooltip("Velocità di corsa (m/s) usata durante l'Engaging mentre raggiungono il player.\nCoincide col runMaxSpeed dell'AStarNPCController per un blend pulito.")]
		public float engagingRunSpeed = 2.2f;

		// Token: 0x04009B6F RID: 39791
		[Tooltip("Spazio in cerchio fra i membri quando arrivano davanti al player.")]
		public float formationRadius = 1f;

		// Token: 0x04009B70 RID: 39792
		[Tooltip("Distanza dall'interviewer al player sotto la quale, durante il Chasing, parte l'encounter (transizione Chasing → Engaging).\nTipicamente >= approachStopDistance + qualche cm per garantire il trigger quando arrivano in formazione.")]
		public float encounterTriggerDistance = 2.1f;

		// Token: 0x04009B71 RID: 39793
		[Tooltip("Frequenza (frame) di re-aim della destinazione durante il Chasing. Il player si muove e i membri devono inseguire.")]
		public int chasingReaimEveryFrames = 10;

		// Token: 0x04009B72 RID: 39794
		[Tooltip("Seconds the chasing dialogue phrase remains visible after the typewriter effect completes.")]
		public float chasingDialogueDwellDuration = 2.5f;

		// Token: 0x04009B73 RID: 39795
		[Tooltip("Tempo massimo (s) durante il Chasing in cui l'interviewer può non vedere il player prima di abbandonare l'inseguimento e tornare in Roaming.\nIl timer si azzera ogni frame in cui HasLineOfSight=true. 0 o negativo = nessun timeout (chase eterno fino al raggiungimento).")]
		public float chaseLoseSightTimeout = 3f;

		// Token: 0x04009B74 RID: 39796
		[Tooltip("Se true, all'abbandono del chase per timeout l'interviewer entra in cooldown (NPCSenses) per non re-detectare immediatamente.")]
		public bool cooldownOnChaseEscape = true;

		// Token: 0x04009B75 RID: 39797
		[Header("Roaming Formation")]
		[Tooltip("Distanza dietro all'interviewer a cui i follower si dispongono in formazione a V durante il Roaming.")]
		public float roamingFollowDistance = 1.2f;

		// Token: 0x04009B76 RID: 39798
		[Tooltip("Distanza laterale tra i follower nella formazione a V.")]
		public float roamingLateralSpacing = 1f;

		// Token: 0x04009B77 RID: 39799
		[Tooltip("Soglia (m) di drift prima di re-impostare la destinazione del follower. Evita spam di SetDestination ogni frame.")]
		public float roamingFormationUpdateThreshold = 0.4f;

		// Token: 0x04009B78 RID: 39800
		[Tooltip("Distanza (m) dallo slot di formazione sotto la quale il follower smette di muoversi e si gira verso l'interviewer.")]
		public float roamingFaceLeaderDistance = 0.6f;

		// Token: 0x04009B79 RID: 39801
		[Tooltip("Velocità (deg/s) di rotazione del follower verso l'interviewer quando ha raggiunto il suo slot.")]
		public float roamingFaceLeaderSpeed = 360f;

		// Token: 0x04009B7A RID: 39802
		[Header("Detection Routing")]
		[Tooltip("Se true, solo l'interviewer (initiator) può innescare l'encounter.\nLe detection degli altri membri vengono ignorate. Coerente con la formazione a V dove il leader è davanti.")]
		public bool initiatorOnlyDetection = true;

		// Token: 0x04009B7B RID: 39803
		[Header("Reporter Detection")]
		[Tooltip("Tempo (s) per cui l'interviewer deve avere HasAudioContact CONSECUTIVO prima di girarsi verso il player.\nEvita reazioni a rumori brevi/casuali. Il timer si azzera ogni volta che HasAudioContact diventa false.")]
		public float audioPersistenceTime = 3f;

		// Token: 0x04009B7C RID: 39804
		[Tooltip("Velocità (deg/s) di rotazione dell'interviewer verso il player quando l'audio persiste.")]
		public float audioTurnSpeed = 180f;

		// Token: 0x04009B7D RID: 39805
		[Tooltip("Distanza (m) massima per il calcolo del livello audio per il debug.")]
		public float audioTurnMaxRange = 15f;

		// Token: 0x04009B7E RID: 39806
		[Tooltip("Soglia di livello audio per attivare la rotazione verso il player.")]
		public float audioTurnThreshold = 0.5f;

		// Token: 0x04009B7F RID: 39807
		[Tooltip("Distanza (m) oltre la quale, anche con LOS attiva, il livello-detection UI è 0.\nTipicamente coincide con BaseViewRadius del NPCSenses (15m).")]
		public float visualDetectionMaxRange = 15f;

		// Token: 0x04009B80 RID: 39808
		[Tooltip("Logga ogni 60 frame lo stato di detection dell'interviewer (HasLOS, HasAudio, distanza). Disattiva in produzione.")]
		public bool debugDetection;

		// Token: 0x04009B82 RID: 39810
		public Action<ReporterTroupe> OnTroupeDisbanding;

		// Token: 0x04009B83 RID: 39811
		private NPCSenses _initiator;

		// Token: 0x04009B84 RID: 39812
		private List<NPCSenses> _additionalActors;

		// Token: 0x04009B85 RID: 39813
		private NPCGroupConfigSO _config;

		// Token: 0x04009B86 RID: 39814
		private TVStudioController _tvStudio;

		// Token: 0x04009B87 RID: 39815
		private readonly Dictionary<NPCSenses, float> _baselineMaxSpeed = new Dictionary<NPCSenses, float>();

		// Token: 0x04009B88 RID: 39816
		private bool _encounterTriggered;

		// Token: 0x04009B89 RID: 39817
		private bool _gameOverSuspended;

		// Token: 0x04009B8A RID: 39818
		private bool _audioTurnActive;

		// Token: 0x04009B8B RID: 39819
		private float _audioContactStartTime = -1f;

		// Token: 0x04009B8C RID: 39820
		private float _lastChaseSightTime;

		// Token: 0x04009B8D RID: 39821
		private bool _externalDirectorActive;

		// Token: 0x04009B8E RID: 39822
		private bool _directorHooked;

		// Token: 0x04009B8F RID: 39823
		private ExhibitionEventDirector _hookedDirector;

		// Token: 0x04009B90 RID: 39824
		private bool _resumeAfterDirector;

		// Token: 0x04009B91 RID: 39825
		private bool _simulationPauseActive;

		// Token: 0x04009B92 RID: 39826
		private float _simulationPauseStartedAt;

		// Token: 0x04009B93 RID: 39827
		[TupleElementNames(new string[] { "simulated", "navigationLocked" })]
		private readonly Dictionary<NPCSenses, ValueTuple<bool, bool>> _encounterMovement = new Dictionary<NPCSenses, ValueTuple<bool, bool>>();
	}
}
