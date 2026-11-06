using System;
using System.Collections.Generic;
using Features.Detection;
using Features.GameOver;
using Features.MatchRecap;
using Features.PlayerEmotes;
using Features.Police;
using Features.RenderingPrecision;
using Features.Tutorials;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using UnityEngine;

namespace Features.Score
{
	// Token: 0x02000C77 RID: 3191
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-70)]
	public sealed class RunScoreDirector : MonoBehaviour
	{
		// Token: 0x17000F27 RID: 3879
		// (get) Token: 0x06005C7A RID: 23674 RVA: 0x00245173 File Offset: 0x00243373
		// (set) Token: 0x06005C7B RID: 23675 RVA: 0x0024517A File Offset: 0x0024337A
		public static RunScoreDirector Instance { get; private set; }

		// Token: 0x06005C7C RID: 23676 RVA: 0x00245184 File Offset: 0x00243384
		private void Awake()
		{
			if (RunScoreDirector.Instance != null && RunScoreDirector.Instance != this)
			{
				Object.Destroy(this);
				return;
			}
			RunScoreDirector.Instance = this;
			this._stats = base.GetComponent<MatchStatsService>();
			if (this._stats == null)
			{
				this._stats = MatchStatsService.Instance;
			}
		}

		// Token: 0x06005C7D RID: 23677 RVA: 0x002451E0 File Offset: 0x002433E0
		private void OnEnable()
		{
			WorldOrigin.Shifted += this.ApplyWorldOriginShift;
			NPCSenses.OnPlayerSpotted += this.HandlePlayerSpotted;
			NPCSenses.OnAttentionChanged += this.HandleAttentionChanged;
			ScoreGameplaySignals.HidingStarted += this.HandleHidingStarted;
			ReporterTroupe.OnChaseStarted += this.HandleReporterChaseStarted;
			ReporterTroupe.OnChaseEscaped += this.HandleReporterEscaped;
			PoliceOfficerChaser.OnChaseStarted += this.HandlePoliceChaseStarted;
			PoliceOfficerChaser.OnChaseEscaped += this.HandlePoliceEscaped;
			PolicePatrol.OnChaseStarted += this.HandlePolicePatrolChaseStarted;
			PolicePatrol.OnChaseEscaped += this.HandlePolicePatrolEscaped;
			vVehicleController.OnAnyVehicleMounted += this.HandleVehicleMounted;
			vVehicleController.OnAnyVehicleDismounted += this.HandleVehicleDismounted;
			SkateboardInteractable.OnAnyMounted += this.HandleSkateboardMounted;
			SkateboardInteractable.OnAnyDismounted += this.HandleSkateboardDismounted;
			SimpleRollerSkatesInteractable.OnAnyMounted += this.HandleRollersMounted;
			SimpleRollerSkatesInteractable.OnAnyDismounted += this.HandleRollersDismounted;
			this.ResolveSceneSources();
		}

		// Token: 0x06005C7E RID: 23678 RVA: 0x00245304 File Offset: 0x00243504
		private void OnDisable()
		{
			WorldOrigin.Shifted -= this.ApplyWorldOriginShift;
			NPCSenses.OnPlayerSpotted -= this.HandlePlayerSpotted;
			NPCSenses.OnAttentionChanged -= this.HandleAttentionChanged;
			ScoreGameplaySignals.HidingStarted -= this.HandleHidingStarted;
			ReporterTroupe.OnChaseStarted -= this.HandleReporterChaseStarted;
			ReporterTroupe.OnChaseEscaped -= this.HandleReporterEscaped;
			PoliceOfficerChaser.OnChaseStarted -= this.HandlePoliceChaseStarted;
			PoliceOfficerChaser.OnChaseEscaped -= this.HandlePoliceEscaped;
			PolicePatrol.OnChaseStarted -= this.HandlePolicePatrolChaseStarted;
			PolicePatrol.OnChaseEscaped -= this.HandlePolicePatrolEscaped;
			vVehicleController.OnAnyVehicleMounted -= this.HandleVehicleMounted;
			vVehicleController.OnAnyVehicleDismounted -= this.HandleVehicleDismounted;
			SkateboardInteractable.OnAnyMounted -= this.HandleSkateboardMounted;
			SkateboardInteractable.OnAnyDismounted -= this.HandleSkateboardDismounted;
			SimpleRollerSkatesInteractable.OnAnyMounted -= this.HandleRollersMounted;
			SimpleRollerSkatesInteractable.OnAnyDismounted -= this.HandleRollersDismounted;
			this.UnbindEmotes();
			this._activeVehicleTransform = null;
			this._activeVehicleType = null;
		}

		// Token: 0x06005C7F RID: 23679 RVA: 0x00245435 File Offset: 0x00243635
		private void ApplyWorldOriginShift(Vector3 delta)
		{
			if (this._activeVehicleTransform != null)
			{
				this._lastVehiclePosition += delta;
			}
		}

		// Token: 0x06005C80 RID: 23680 RVA: 0x00245457 File Offset: 0x00243657
		private void OnDestroy()
		{
			if (RunScoreDirector.Instance == this)
			{
				RunScoreDirector.Instance = null;
			}
		}

		// Token: 0x06005C81 RID: 23681 RVA: 0x0024546C File Offset: 0x0024366C
		private void Update()
		{
			if (this._stats == null)
			{
				this._stats = MatchStatsService.Instance;
			}
			if (this._emotes == null || this._poiContext == null)
			{
				this.ResolveSceneSources();
			}
			this.TickActiveVehicle();
		}

		// Token: 0x06005C82 RID: 23682 RVA: 0x002454BC File Offset: 0x002436BC
		public void ResetForNewRun()
		{
			this._activeEmote = null;
			this._emoteSession = 0;
			this._emoteWitnesses.Clear();
			this._witnessCooldownUntil.Clear();
			this._exhibitionPoiIds.Clear();
			this._riskEpisodes.Clear();
			this._escapeCooldownUntil.Clear();
			this._activeSpecialChases.Clear();
			this._specialChasesUsingHiding.Clear();
			this._escapeSequence = 0;
			this._underCarAwarded = false;
			this._activeVehicleTransform = null;
			this._activeVehicleType = null;
			this._vehicleDistanceByType.Clear();
			this._vehicleMilestones.Clear();
		}

		// Token: 0x06005C83 RID: 23683 RVA: 0x00245558 File Offset: 0x00243758
		private void ResolveSceneSources()
		{
			GameplayEmoteController gameplayEmoteController = Object.FindObjectOfType<GameplayEmoteController>();
			if (gameplayEmoteController != this._emotes)
			{
				this.UnbindEmotes();
				this._emotes = gameplayEmoteController;
				if (this._emotes != null)
				{
					this._emotes.OnEmoteStarted += this.HandleEmoteStarted;
					this._emotes.OnEmoteEnded += this.HandleEmoteEnded;
				}
			}
			this._poiContext = ((this._emotes != null) ? this._emotes.GetComponent<PlayerPoiEmoteContext>() : Object.FindObjectOfType<PlayerPoiEmoteContext>());
		}

		// Token: 0x06005C84 RID: 23684 RVA: 0x002455E8 File Offset: 0x002437E8
		private void UnbindEmotes()
		{
			if (this._emotes != null)
			{
				this._emotes.OnEmoteStarted -= this.HandleEmoteStarted;
				this._emotes.OnEmoteEnded -= this.HandleEmoteEnded;
			}
			this._emotes = null;
			this._activeEmote = null;
			this._emoteWitnesses.Clear();
		}

		// Token: 0x06005C85 RID: 23685 RVA: 0x0024564C File Offset: 0x0024384C
		private void HandleEmoteStarted(EmoteDefinitionSO emote)
		{
			if (!this.CanTrackRun() || emote == null)
			{
				return;
			}
			this._activeEmote = emote;
			this._emoteSession++;
			this._emoteWitnesses.Clear();
			SensoryManager instance = SensoryManager.Instance;
			if (instance == null)
			{
				return;
			}
			IReadOnlyList<NPCSenses> registeredNPCs = instance.GetRegisteredNPCs();
			for (int i = 0; i < registeredNPCs.Count; i++)
			{
				NPCSenses npcsenses = registeredNPCs[i];
				if (npcsenses != null && npcsenses.HasLineOfSight && npcsenses.CurrentDetectionSource == DetectionSource.Visual)
				{
					this.RegisterEmoteWitness(npcsenses);
				}
			}
		}

		// Token: 0x06005C86 RID: 23686 RVA: 0x002456DC File Offset: 0x002438DC
		private void HandleEmoteEnded(EmoteDefinitionSO emote)
		{
			if (this._activeEmote == null || emote == this._activeEmote)
			{
				this._activeEmote = null;
				this._emoteWitnesses.Clear();
			}
		}

		// Token: 0x06005C87 RID: 23687 RVA: 0x0024570C File Offset: 0x0024390C
		private void HandlePlayerSpotted(NPCSenses npc)
		{
			if (this._activeEmote != null && npc != null)
			{
				this.RegisterEmoteWitness(npc);
			}
		}

		// Token: 0x06005C88 RID: 23688 RVA: 0x0024572C File Offset: 0x0024392C
		private void RegisterEmoteWitness(NPCSenses npc)
		{
			if (!this.CanTrackRun() || RunScoreDirector.IsPresentationBlocked() || npc == null)
			{
				return;
			}
			int instanceID = npc.GetInstanceID();
			if (this._emoteWitnesses.Contains(instanceID))
			{
				return;
			}
			float num;
			if (this._witnessCooldownUntil.TryGetValue(instanceID, out num) && Time.time < num)
			{
				return;
			}
			this._emoteWitnesses.Add(instanceID);
			this._witnessCooldownUntil[instanceID] = Time.time + 8f;
			string text = "emote:" + this._emoteSession.ToString();
			if (this._emoteWitnesses.Count == 1)
			{
				this.Award(RunScoreReason.Exhibition, 250, "EXHIBITION", string.Empty, text);
				string text2 = ((this._poiContext != null && this._poiContext.CurrentProfile != null) ? this._poiContext.CurrentProfile.PoiId : string.Empty);
				if (!string.IsNullOrEmpty(text2) && this._exhibitionPoiIds.Add(text2))
				{
					this.Award(RunScoreReason.ExhibitionInPoi, 200, "EXHIBITION", "NEW POI", text);
					return;
				}
			}
			else if (this._emoteWitnesses.Count == 2)
			{
				this.Award(RunScoreReason.ExhibitionCrowd, 150, "EXHIBITION", "CROWD", text);
			}
		}

		// Token: 0x06005C89 RID: 23689 RVA: 0x00245874 File Offset: 0x00243A74
		private void HandleAttentionChanged(NPCSenses npc, float previous, float current)
		{
			if (!this.CanTrackRun() || npc == null || RunScoreDirector.IsSpecialChaser(npc))
			{
				return;
			}
			int instanceID = npc.GetInstanceID();
			if (current >= 0.5f && (npc.CurrentDetectionSource == DetectionSource.Visual || npc.HasLineOfSight))
			{
				RunScoreDirector.RiskEpisode riskEpisode;
				if (!this._riskEpisodes.TryGetValue(instanceID, out riskEpisode))
				{
					riskEpisode = new RunScoreDirector.RiskEpisode();
					this._riskEpisodes[instanceID] = riskEpisode;
				}
				riskEpisode.Armed = true;
				if (current >= 0.99f || npc.IsDetected)
				{
					riskEpisode.ReachedFinalDetection = true;
				}
				return;
			}
			RunScoreDirector.RiskEpisode riskEpisode2;
			if (current > 0.015f || !this._riskEpisodes.TryGetValue(instanceID, out riskEpisode2) || !riskEpisode2.Armed)
			{
				return;
			}
			this._riskEpisodes.Remove(instanceID);
			if (riskEpisode2.ReachedFinalDetection || RunScoreDirector.IsPresentationBlocked())
			{
				return;
			}
			float num;
			if (this._escapeCooldownUntil.TryGetValue(instanceID, out num) && Time.time < num)
			{
				return;
			}
			this._escapeCooldownUntil[instanceID] = Time.time + 10f;
			string text = "escape:";
			int num2 = this._escapeSequence + 1;
			this._escapeSequence = num2;
			string text2 = text + num2.ToString();
			this.Award(RunScoreReason.NpcEscape, 150, "ESCAPED", "NPC", text2);
			if (riskEpisode2.UsedHiding)
			{
				this.Award(RunScoreReason.HidingEscape, 200, "ESCAPED", "HIDING SPOT", text2);
			}
		}

		// Token: 0x06005C8A RID: 23690 RVA: 0x002459D4 File Offset: 0x00243BD4
		private void HandleHidingStarted(ScoreHidingSpotKind kind, Object source)
		{
			if (!this.CanTrackRun())
			{
				return;
			}
			foreach (RunScoreDirector.RiskEpisode riskEpisode in this._riskEpisodes.Values)
			{
				if (riskEpisode.Armed)
				{
					riskEpisode.UsedHiding = true;
				}
			}
			foreach (string text in this._activeSpecialChases)
			{
				this._specialChasesUsingHiding.Add(text);
			}
			if (kind == ScoreHidingSpotKind.UnderCar && !this._underCarAwarded)
			{
				this._underCarAwarded = this.Award(RunScoreReason.HiddenUnderCar, 150, "UNDER THE CAR", "FIRST TIME", "under-car:first");
			}
		}

		// Token: 0x06005C8B RID: 23691 RVA: 0x00245AB4 File Offset: 0x00243CB4
		private void HandleReporterChaseStarted(ReporterTroupe troupe)
		{
			if (troupe == null)
			{
				return;
			}
			string text = RunScoreDirector.SpecialChaseKey("reporter", troupe);
			this._specialChasesUsingHiding.Remove(text);
			this._activeSpecialChases.Add(text);
		}

		// Token: 0x06005C8C RID: 23692 RVA: 0x00245AF1 File Offset: 0x00243CF1
		private void HandleReporterEscaped(ReporterTroupe troupe)
		{
			this.AwardSpecialEscape(RunScoreReason.ReporterEscape, 500, "REPORTERS", RunScoreDirector.SpecialChaseKey("reporter", troupe));
		}

		// Token: 0x06005C8D RID: 23693 RVA: 0x00245B10 File Offset: 0x00243D10
		private void HandlePoliceChaseStarted(PoliceOfficerChaser officer)
		{
			if (officer == null)
			{
				return;
			}
			string text = RunScoreDirector.SpecialChaseKey("police", officer);
			this._specialChasesUsingHiding.Remove(text);
			this._activeSpecialChases.Add(text);
		}

		// Token: 0x06005C8E RID: 23694 RVA: 0x00245B4D File Offset: 0x00243D4D
		private void HandlePoliceEscaped(PoliceOfficerChaser officer)
		{
			this.AwardSpecialEscape(RunScoreReason.PoliceEscape, 750, "POLICE", RunScoreDirector.SpecialChaseKey("police", officer));
		}

		// Token: 0x06005C8F RID: 23695 RVA: 0x00245B6C File Offset: 0x00243D6C
		private void HandlePolicePatrolChaseStarted(PolicePatrol patrol)
		{
			if (patrol == null)
			{
				return;
			}
			string text = RunScoreDirector.SpecialChaseKey("patrol", patrol);
			this._specialChasesUsingHiding.Remove(text);
			this._activeSpecialChases.Add(text);
		}

		// Token: 0x06005C90 RID: 23696 RVA: 0x00245BA9 File Offset: 0x00243DA9
		private void HandlePolicePatrolEscaped(PolicePatrol patrol)
		{
			this.AwardSpecialEscape(RunScoreReason.PoliceEscape, 750, "POLICE", RunScoreDirector.SpecialChaseKey("patrol", patrol));
		}

		// Token: 0x06005C91 RID: 23697 RVA: 0x00245BC8 File Offset: 0x00243DC8
		private void AwardSpecialEscape(RunScoreReason reason, int points, string detail, string chaseKey)
		{
			if (!this.CanTrackRun() || RunScoreDirector.IsPresentationBlocked())
			{
				return;
			}
			string text = "escape:";
			int num = this._escapeSequence + 1;
			this._escapeSequence = num;
			string text2 = text + num.ToString();
			this.Award(reason, points, "ESCAPED", detail, text2);
			bool flag = !string.IsNullOrEmpty(chaseKey) && this._specialChasesUsingHiding.Remove(chaseKey);
			if (!string.IsNullOrEmpty(chaseKey))
			{
				this._activeSpecialChases.Remove(chaseKey);
			}
			if (flag)
			{
				this.Award(RunScoreReason.HidingEscape, 200, "ESCAPED", "HIDING SPOT", text2);
			}
		}

		// Token: 0x06005C92 RID: 23698 RVA: 0x00245C64 File Offset: 0x00243E64
		private static string SpecialChaseKey(string prefix, Object source)
		{
			if (!(source == null))
			{
				return prefix + ":" + source.GetInstanceID().ToString();
			}
			return string.Empty;
		}

		// Token: 0x06005C93 RID: 23699 RVA: 0x00245C9C File Offset: 0x00243E9C
		private void HandleVehicleMounted(vVehicleController vehicle, vThirdPersonController rider)
		{
			if (!this.CanTrackRun() || vehicle == null)
			{
				return;
			}
			this.BeginVehicleRide(vehicle.transform, vehicle.Type.ToString());
		}

		// Token: 0x06005C94 RID: 23700 RVA: 0x00245CDB File Offset: 0x00243EDB
		private void HandleVehicleDismounted(vVehicleController vehicle, vThirdPersonController rider)
		{
			if (vehicle != null && vehicle.transform == this._activeVehicleTransform)
			{
				this.EndVehicleRide();
			}
		}

		// Token: 0x06005C95 RID: 23701 RVA: 0x00245CFF File Offset: 0x00243EFF
		private void HandleSkateboardMounted(SkateboardInteractable vehicle, vThirdPersonController rider)
		{
			if (vehicle != null)
			{
				this.BeginVehicleRide(vehicle.transform, "Skateboard");
			}
		}

		// Token: 0x06005C96 RID: 23702 RVA: 0x00245CDB File Offset: 0x00243EDB
		private void HandleSkateboardDismounted(SkateboardInteractable vehicle, vThirdPersonController rider)
		{
			if (vehicle != null && vehicle.transform == this._activeVehicleTransform)
			{
				this.EndVehicleRide();
			}
		}

		// Token: 0x06005C97 RID: 23703 RVA: 0x00245D1B File Offset: 0x00243F1B
		private void HandleRollersMounted(SimpleRollerSkatesInteractable vehicle, vThirdPersonController rider)
		{
			if (vehicle != null)
			{
				this.BeginVehicleRide(vehicle.transform, "RollerSkates");
			}
		}

		// Token: 0x06005C98 RID: 23704 RVA: 0x00245CDB File Offset: 0x00243EDB
		private void HandleRollersDismounted(SimpleRollerSkatesInteractable vehicle, vThirdPersonController rider)
		{
			if (vehicle != null && vehicle.transform == this._activeVehicleTransform)
			{
				this.EndVehicleRide();
			}
		}

		// Token: 0x06005C99 RID: 23705 RVA: 0x00245D37 File Offset: 0x00243F37
		private void BeginVehicleRide(Transform vehicle, string type)
		{
			if (!this.CanTrackRun() || vehicle == null)
			{
				return;
			}
			this._activeVehicleTransform = vehicle;
			this._activeVehicleType = (string.IsNullOrWhiteSpace(type) ? "Vehicle" : type);
			this._lastVehiclePosition = vehicle.position;
		}

		// Token: 0x06005C9A RID: 23706 RVA: 0x00245D74 File Offset: 0x00243F74
		private void EndVehicleRide()
		{
			this._activeVehicleTransform = null;
			this._activeVehicleType = null;
		}

		// Token: 0x06005C9B RID: 23707 RVA: 0x00245D84 File Offset: 0x00243F84
		private void TickActiveVehicle()
		{
			if (!this.CanTrackRun() || this._activeVehicleTransform == null)
			{
				return;
			}
			Vector3 position = this._activeVehicleTransform.position;
			float num = Vector3.Distance(position, this._lastVehiclePosition);
			this._lastVehiclePosition = position;
			if (num <= 0.01f || num > 12f)
			{
				return;
			}
			string activeVehicleType = this._activeVehicleType;
			float num2;
			this._vehicleDistanceByType.TryGetValue(activeVehicleType, out num2);
			num2 += num;
			this._vehicleDistanceByType[activeVehicleType] = num2;
			this.TryAwardVehicleMilestone(activeVehicleType, num2, 100, 150);
			this.TryAwardVehicleMilestone(activeVehicleType, num2, 500, 250);
		}

		// Token: 0x06005C9C RID: 23708 RVA: 0x00245E24 File Offset: 0x00244024
		private void TryAwardVehicleMilestone(string type, float distance, int threshold, int points)
		{
			if (distance < (float)threshold)
			{
				return;
			}
			string text = type + ":" + threshold.ToString();
			if (this._vehicleMilestones.Contains(text))
			{
				return;
			}
			if (this.Award(RunScoreReason.VehicleDistance, points, "VEHICLE RIDE", type.ToUpperInvariant() + " · " + threshold.ToString() + "M", "vehicle:" + text))
			{
				this._vehicleMilestones.Add(text);
			}
		}

		// Token: 0x06005C9D RID: 23709 RVA: 0x00245EA0 File Offset: 0x002440A0
		private bool Award(RunScoreReason reason, int points, string label, string detail, string group)
		{
			TutorialService.TryShowFirstUse("score");
			RunScoreAward runScoreAward;
			return this._stats != null && this._stats.AwardPoints(new RunScoreAwardRequest(reason, points, label, detail, group, RunScorePresentation.Generic), out runScoreAward);
		}

		// Token: 0x06005C9E RID: 23710 RVA: 0x00245EE2 File Offset: 0x002440E2
		private bool CanTrackRun()
		{
			return this._stats != null && !this._stats.IsMocked && this._stats.IsRunTrackingActive;
		}

		// Token: 0x06005C9F RID: 23711 RVA: 0x00245F0C File Offset: 0x0024410C
		private static bool IsPresentationBlocked()
		{
			GameOverManager instance = GameOverManager.Instance;
			if (instance != null && instance.IsGameOver)
			{
				return true;
			}
			ExhibitionEventDirector instance2 = ExhibitionEventDirector.Instance;
			return instance2 != null && instance2.isActiveAndEnabled && instance2.IsEventActive;
		}

		// Token: 0x06005CA0 RID: 23712 RVA: 0x00245F51 File Offset: 0x00244151
		private static bool IsSpecialChaser(NPCSenses npc)
		{
			return npc.GetComponent<TroupeDetectionProxy>() != null || npc.GetComponent<PoliceOfficerChaser>() != null || npc.GetComponentInParent<PolicePatrol>() != null || npc.GetComponent<RefereeChaser>() != null;
		}

		// Token: 0x04005DD5 RID: 24021
		private const float WitnessCooldownSeconds = 8f;

		// Token: 0x04005DD6 RID: 24022
		private const float EscapeAttentionThreshold = 0.5f;

		// Token: 0x04005DD7 RID: 24023
		private const float EscapeResolvedThreshold = 0.015f;

		// Token: 0x04005DD8 RID: 24024
		private const float EscapeCooldownSeconds = 10f;

		// Token: 0x04005DDA RID: 24026
		private MatchStatsService _stats;

		// Token: 0x04005DDB RID: 24027
		private GameplayEmoteController _emotes;

		// Token: 0x04005DDC RID: 24028
		private PlayerPoiEmoteContext _poiContext;

		// Token: 0x04005DDD RID: 24029
		private EmoteDefinitionSO _activeEmote;

		// Token: 0x04005DDE RID: 24030
		private int _emoteSession;

		// Token: 0x04005DDF RID: 24031
		private readonly HashSet<int> _emoteWitnesses = new HashSet<int>();

		// Token: 0x04005DE0 RID: 24032
		private readonly Dictionary<int, float> _witnessCooldownUntil = new Dictionary<int, float>();

		// Token: 0x04005DE1 RID: 24033
		private readonly HashSet<string> _exhibitionPoiIds = new HashSet<string>();

		// Token: 0x04005DE2 RID: 24034
		private readonly Dictionary<int, RunScoreDirector.RiskEpisode> _riskEpisodes = new Dictionary<int, RunScoreDirector.RiskEpisode>();

		// Token: 0x04005DE3 RID: 24035
		private readonly Dictionary<int, float> _escapeCooldownUntil = new Dictionary<int, float>();

		// Token: 0x04005DE4 RID: 24036
		private readonly HashSet<string> _activeSpecialChases = new HashSet<string>();

		// Token: 0x04005DE5 RID: 24037
		private readonly HashSet<string> _specialChasesUsingHiding = new HashSet<string>();

		// Token: 0x04005DE6 RID: 24038
		private int _escapeSequence;

		// Token: 0x04005DE7 RID: 24039
		private bool _underCarAwarded;

		// Token: 0x04005DE8 RID: 24040
		private Transform _activeVehicleTransform;

		// Token: 0x04005DE9 RID: 24041
		private string _activeVehicleType;

		// Token: 0x04005DEA RID: 24042
		private Vector3 _lastVehiclePosition;

		// Token: 0x04005DEB RID: 24043
		private readonly Dictionary<string, float> _vehicleDistanceByType = new Dictionary<string, float>(StringComparer.Ordinal);

		// Token: 0x04005DEC RID: 24044
		private readonly HashSet<string> _vehicleMilestones = new HashSet<string>(StringComparer.Ordinal);

		// Token: 0x02000C78 RID: 3192
		private sealed class RiskEpisode
		{
			// Token: 0x04005DED RID: 24045
			public bool Armed;

			// Token: 0x04005DEE RID: 24046
			public bool ReachedFinalDetection;

			// Token: 0x04005DEF RID: 24047
			public bool UsedHiding;
		}
	}
}
