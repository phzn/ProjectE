using System;
using System.Collections;
using BoZo.CharacterCreator.MC.StyleSystem;
using ExhibitionistDemo.UI;
using Features.Detection;
using Features.GameOver;
using Features.Inventory.Wearable;
using Features.MatchRecap;
using Features.NPC;
using Features.PlayerEmotions;
using Features.Police;
using Features.RunModes;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.PoiTravel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Features.Rank
{
	// Token: 0x02000DB4 RID: 3508
	[RequireComponent(typeof(RankRunTracker))]
	public class RankRuntimeController : MonoBehaviour
	{
		// Token: 0x170010FE RID: 4350
		// (get) Token: 0x06006745 RID: 26437 RVA: 0x00295275 File Offset: 0x00293475
		// (set) Token: 0x06006746 RID: 26438 RVA: 0x0029527C File Offset: 0x0029347C
		public static RankRuntimeController Current { get; private set; }

		// Token: 0x170010FF RID: 4351
		// (get) Token: 0x06006747 RID: 26439 RVA: 0x00295284 File Offset: 0x00293484
		// (set) Token: 0x06006748 RID: 26440 RVA: 0x0029528B File Offset: 0x0029348B
		public static bool InitialPresentationReady { get; private set; }

		// Token: 0x17001100 RID: 4352
		// (get) Token: 0x06006749 RID: 26441 RVA: 0x00295293 File Offset: 0x00293493
		public bool KeepAliveOutsideRun
		{
			get
			{
				return this.keepAliveOutsideRun;
			}
		}

		// Token: 0x17001101 RID: 4353
		// (get) Token: 0x0600674A RID: 26442 RVA: 0x0029529C File Offset: 0x0029349C
		public bool UsesCustomizationPresetInCurrentContext
		{
			get
			{
				return RunModeService.UsesHomePresetOutfit || (this.keepAliveOutsideRun && base.gameObject.scene.name == "CielHome" && (!RankWardrobePolicy.IsRestricted || RankWardrobePolicy.UsesCertifiedHomePreset));
			}
		}

		// Token: 0x17001102 RID: 4354
		// (get) Token: 0x0600674B RID: 26443 RVA: 0x002952EC File Offset: 0x002934EC
		public bool IsAtHome
		{
			get
			{
				return RunModeService.IsAtHome || (this.keepAliveOutsideRun && base.gameObject.scene.name == "CielHome");
			}
		}

		// Token: 0x17001103 RID: 4355
		// (get) Token: 0x0600674C RID: 26444 RVA: 0x00295329 File Offset: 0x00293529
		// (set) Token: 0x0600674D RID: 26445 RVA: 0x00295331 File Offset: 0x00293531
		public RunOutcome LastRunOutcome { get; private set; }

		// Token: 0x17001104 RID: 4356
		// (get) Token: 0x0600674E RID: 26446 RVA: 0x0029533A File Offset: 0x0029353A
		// (set) Token: 0x0600674F RID: 26447 RVA: 0x00295342 File Offset: 0x00293542
		public bool HasLastRunOutcome { get; private set; }

		// Token: 0x17001105 RID: 4357
		// (get) Token: 0x06006750 RID: 26448 RVA: 0x0029534B File Offset: 0x0029354B
		public RankRunTracker Tracker
		{
			get
			{
				return this._tracker;
			}
		}

		// Token: 0x17001106 RID: 4358
		// (get) Token: 0x06006751 RID: 26449 RVA: 0x00295353 File Offset: 0x00293553
		public bool IsRunInProgress
		{
			get
			{
				return this._startedInGameplay && !this._runResultsApplied && !this.IsAtHome;
			}
		}

		// Token: 0x17001107 RID: 4359
		// (get) Token: 0x06006752 RID: 26450 RVA: 0x00295370 File Offset: 0x00293570
		public bool HasPendingQualifyingDistance
		{
			get
			{
				bool startedInGameplay = this._startedInGameplay;
				bool runResultsApplied = this._runResultsApplied;
				bool flag = this._startedAsAdmin || RankProgressionService.IsAdminRankActive;
				bool isAtHome = this.IsAtHome;
				string startingRankId = this._startingRankId;
				RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
				return RankRuntimeController.CanShowPendingQualifyingDistance(startedInGameplay, runResultsApplied, flag, isAtHome, startingRankId, (currentRank != null) ? currentRank.rankId : null);
			}
		}

		// Token: 0x06006753 RID: 26451 RVA: 0x002953B0 File Offset: 0x002935B0
		public static bool CanShowPendingQualifyingDistance(bool startedInGameplay, bool resultsApplied, bool admin, bool atHome, string startingRankId, string currentRankId)
		{
			return startedInGameplay && !resultsApplied && !admin && !atHome && !string.IsNullOrEmpty(startingRankId) && string.Equals(startingRankId, currentRankId, StringComparison.Ordinal);
		}

		// Token: 0x17001108 RID: 4360
		// (get) Token: 0x06006754 RID: 26452 RVA: 0x002953D3 File Offset: 0x002935D3
		public int LossesThisRun
		{
			get
			{
				return this._lossesThisRun;
			}
		}

		// Token: 0x17001109 RID: 4361
		// (get) Token: 0x06006755 RID: 26453 RVA: 0x002953DB File Offset: 0x002935DB
		public int MaxLossesThisRun
		{
			get
			{
				if (!(RankProgressionService.CurrentRank != null))
				{
					return 1;
				}
				return RankProgressionService.CurrentRank.maxLossesPerRun;
			}
		}

		// Token: 0x06006756 RID: 26454 RVA: 0x002953F6 File Offset: 0x002935F6
		private void Awake()
		{
			this._tracker = base.GetComponent<RankRunTracker>();
		}

		// Token: 0x06006757 RID: 26455 RVA: 0x00295404 File Offset: 0x00293604
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticState()
		{
			RankRuntimeController.Current = null;
			RankRuntimeController.InitialPresentationReady = false;
		}

		// Token: 0x06006758 RID: 26456 RVA: 0x00295412 File Offset: 0x00293612
		private void OnEnable()
		{
			RankRuntimeController.Current = this;
			this.BeginRun();
			RankProgressionService.Changed += this.HandleProgressionChanged;
		}

		// Token: 0x06006759 RID: 26457 RVA: 0x00295434 File Offset: 0x00293634
		private void OnDisable()
		{
			RankProgressionService.Changed -= this.HandleProgressionChanged;
			if (this._boundGameOver != null && this._boundGameOver.OnGameOver != null)
			{
				this._boundGameOver.OnGameOver.RemoveListener(new UnityAction<GameOverReason>(this.HandleGameOver));
			}
			this._boundGameOver = null;
			if (this._boundDirector != null)
			{
				this._boundDirector.OnEncounterResolved -= this.HandleEncounterResolved;
			}
			this._boundDirector = null;
			if (EmotionManager.Instance != null)
			{
				this.RestoreDecayIfDisabled(EmotionManager.Instance);
				EmotionManager.Instance.SetStanceLock(null);
			}
			if (DetectionEmotionSource.Instance != null)
			{
				DetectionEmotionSource.Instance.SetStanceAwarePreDetection(false);
			}
			this.PopGate();
			if (RankRuntimeController.Current == this)
			{
				if (SceneManager.GetActiveScene().name != "CielHome")
				{
					RankNightController.Release();
				}
				RankRuntimeController.Current = null;
			}
		}

		// Token: 0x0600675A RID: 26458 RVA: 0x00295538 File Offset: 0x00293738
		private void BeginRun()
		{
			RankRuntimeController.InitialPresentationReady = false;
			this._runResultsApplied = false;
			this.HasLastRunOutcome = false;
			this._outfitAppliedThisRun = false;
			this._lossesThisRun = 0;
			this._stanceAppliedRank = null;
			RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
			this._observedRankId = ((currentRank != null) ? currentRank.rankId : null);
			this._startingRankId = RunModeService.PreparedStartingRankId ?? this._observedRankId;
			this._startedAsAdmin = (RunModeService.HasPreparedRun ? RunModeService.PreparedStartedAsAdmin : RankProgressionService.IsAdminRankActive);
			SceneContext sceneContext = SceneContext.Current;
			this._startedInGameplay = !this.IsAtHome && (sceneContext == null || sceneContext.Profile == null || sceneContext.Profile.participatesInRun);
			if (MatchStatsService.Instance != null)
			{
				MatchStatsService.Instance.SetRunTrackingActive(true);
				MatchStatsService.Instance.ResetForNewRun();
				MatchStatsService.Instance.SetMockedOverride(false);
			}
			DistanceTracker distanceTracker = Object.FindObjectOfType<DistanceTracker>();
			if (distanceTracker != null)
			{
				distanceTracker.ResetForNewRun();
			}
			if (this._tracker != null)
			{
				this._tracker.ResetForNewRun();
			}
			if (GameOverManager.Instance != null)
			{
				this._boundGameOver = GameOverManager.Instance;
				if (this._boundGameOver.OnGameOver != null)
				{
					this._boundGameOver.OnGameOver.RemoveListener(new UnityAction<GameOverReason>(this.HandleGameOver));
					this._boundGameOver.OnGameOver.AddListener(new UnityAction<GameOverReason>(this.HandleGameOver));
				}
			}
			if (ExhibitionEventDirector.Instance != null)
			{
				this._boundDirector = ExhibitionEventDirector.Instance;
				this._boundDirector.OnEncounterResolved -= this.HandleEncounterResolved;
				this._boundDirector.OnEncounterResolved += this.HandleEncounterResolved;
			}
			RankDefinitionSO currentRank2 = RankProgressionService.CurrentRank;
			Debug.Log("[RankRuntimeController] Run iniziata. Rank: " + ((currentRank2 != null) ? currentRank2.displayName : "<no config>") + " " + string.Format("(livello {0}, XP {1:0}).", RankProgressionService.Level, RankProgressionService.Xp));
			this.ApplyCurrentRank(false);
		}

		// Token: 0x0600675B RID: 26459 RVA: 0x00295744 File Offset: 0x00293944
		private void HandleProgressionChanged()
		{
			RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
			string text = ((currentRank != null) ? currentRank.rankId : null);
			if (this._observedRankId == text)
			{
				return;
			}
			RankProgressionConfigSO config = RankProgressionService.Config;
			string text2;
			if (config == null)
			{
				text2 = null;
			}
			else
			{
				RankDefinitionSO adminRank = config.adminRank;
				text2 = ((adminRank != null) ? adminRank.rankId : null);
			}
			string text3 = text2;
			bool flag = text3 != null && (text == text3 || this._observedRankId == text3);
			if (this._startedInGameplay && RankProgressionService.IsAdminRankActive)
			{
				this._startedAsAdmin = true;
			}
			this._observedRankId = text;
			if (this.IsAtHome)
			{
				this.ApplyCurrentRank(true);
				return;
			}
			if (flag)
			{
				this.ApplyCurrentRank(false);
			}
		}

		// Token: 0x0600675C RID: 26460 RVA: 0x002957E8 File Offset: 0x002939E8
		public void ApplyCurrentRank(bool forceOutfit = false)
		{
			RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
			if (currentRank == null)
			{
				RankRuntimeController.InitialPresentationReady = true;
				Debug.LogWarning("[RankRuntimeController] ApplyCurrentRank: nessun rank (config assente).");
				return;
			}
			if (forceOutfit && !this.IsAtHome)
			{
				RunModeService.DisqualifyLeaderboardRun("The starting outfit was restored with a debug control.");
			}
			this.ApplyGate(currentRank);
			this.ApplyStance(currentRank, forceOutfit);
			RankNightController.Apply(currentRank, RankProgressionService.Config);
			this.EnsureFaceController(currentRank);
			this.ApplyStreetPresence(currentRank);
			this.ApplyPreDetectionStance(currentRank);
			bool flag = !forceOutfit && PoiTravelState.IsFastTravelVisit && !RunModeService.IsMapsVisit;
			bool flag2 = forceOutfit || (!flag && !this._outfitAppliedThisRun);
			if (flag2)
			{
				this._outfitAppliedThisRun = true;
				this.ApplyStartOutfit(currentRank, forceOutfit);
			}
			else
			{
				if (flag)
				{
					this._outfitAppliedThisRun = true;
				}
				RankRuntimeController.InitialPresentationReady = true;
			}
			Debug.Log("[RankRuntimeController] ApplyCurrentRank -> " + currentRank.displayName + ": gate applicato" + (flag2 ? ", outfit start richiesto." : (flag ? " (outfit di casa preservato per fast travel)." : " (outfit gia' applicato in questa run).")));
		}

		// Token: 0x0600675D RID: 26461 RVA: 0x002958DC File Offset: 0x00293ADC
		private void ApplyGate(RankDefinitionSO rank)
		{
			PlayerActivity playerActivity = PlayerActivity.None;
			if (!rank.allowEmoteWheel)
			{
				playerActivity |= PlayerActivity.Emote;
			}
			if (!rank.allowFlash)
			{
				playerActivity |= PlayerActivity.Flash;
			}
			if (!rank.allowStanceSwitch || RunModeService.IsLongWayHome)
			{
				playerActivity |= PlayerActivity.StanceSwitch;
			}
			if (!this.IsAtHome && !rank.allowFullUndress && !rank.allowBraRemoval)
			{
				playerActivity |= PlayerActivity.Undress;
			}
			if (!rank.allowLingerieOpenAnimations)
			{
				playerActivity |= PlayerActivity.LingerieOpen;
			}
			if (!rank.allowCoverPose)
			{
				playerActivity |= PlayerActivity.ArmsPose;
			}
			this.PopGate();
			if (playerActivity != PlayerActivity.None)
			{
				PlayerActivityGate.Push("Rank", playerActivity);
				this._gatePushed = true;
			}
		}

		// Token: 0x0600675E RID: 26462 RVA: 0x0029596F File Offset: 0x00293B6F
		private void PopGate()
		{
			if (!this._gatePushed)
			{
				return;
			}
			PlayerActivityGate.Pop("Rank");
			this._gatePushed = false;
		}

		// Token: 0x0600675F RID: 26463 RVA: 0x0029598C File Offset: 0x00293B8C
		private void ApplyStartOutfit(RankDefinitionSO rank, bool refreshCustomizationPreset)
		{
			if (this._outfitRoutine != null)
			{
				base.StopCoroutine(this._outfitRoutine);
				this._outfitRoutine = null;
			}
			if (this.UsesCustomizationPresetInCurrentContext)
			{
				if (refreshCustomizationPreset)
				{
					RankOutfitApplier outfitApplier = this._outfitApplier;
					if (outfitApplier != null)
					{
						outfitApplier.ClearAppliedRankItems();
					}
					MainCharacterAppearanceEvents.RaiseInUsePresetChanged();
				}
				RankRuntimeController.InitialPresentationReady = true;
				return;
			}
			if (!rank.startOutfitReplacesCustom)
			{
				RankRuntimeController.InitialPresentationReady = true;
				return;
			}
			if (!base.isActiveAndEnabled)
			{
				RankRuntimeController.InitialPresentationReady = true;
				return;
			}
			if (this._outfitApplier == null)
			{
				this._outfitApplier = new RankOutfitApplier();
			}
			this._outfitRoutine = base.StartCoroutine(this.ApplyStartOutfitAndSignalReady(rank));
		}

		// Token: 0x06006760 RID: 26464 RVA: 0x00295A20 File Offset: 0x00293C20
		public void RefreshOutfitAfterAppearance(MCStyleCustomizationService.WardrobeSnapshot previousWardrobe = null)
		{
			if (!RankWardrobePolicy.IsRestricted || !base.isActiveAndEnabled)
			{
				return;
			}
			RankRuntimeController.InitialPresentationReady = false;
			if (this._outfitApplier == null)
			{
				this._outfitApplier = new RankOutfitApplier();
			}
			this._outfitApplier.AppearanceRollback = previousWardrobe;
			this.ApplyStartOutfit(RankProgressionService.CurrentRank, false);
		}

		// Token: 0x06006761 RID: 26465 RVA: 0x00295A6E File Offset: 0x00293C6E
		private IEnumerator ApplyStartOutfitAndSignalReady(RankDefinitionSO rank)
		{
			yield return this._outfitApplier.ApplyRoutine(rank);
			RankRuntimeController.InitialPresentationReady = true;
			this._outfitRoutine = null;
			yield break;
		}

		// Token: 0x06006762 RID: 26466 RVA: 0x00295A84 File Offset: 0x00293C84
		private void ApplyStance(RankDefinitionSO rank, bool force)
		{
			EmotionManager instance = EmotionManager.Instance;
			if (instance == null)
			{
				return;
			}
			if (!force && this._stanceAppliedRank == rank)
			{
				return;
			}
			instance.SetStance(RunModeService.IsLongWayHome ? EmotionStance.Embarassed : rank.defaultStance, "Rank");
			if (rank.allowStanceSwitch)
			{
				instance.SetStanceLock(null);
			}
			else
			{
				instance.SetStanceLock(new EmotionStance?(rank.defaultStance));
			}
			this.ApplyDecayPolicy(rank, instance);
			this._stanceAppliedRank = rank;
		}

		// Token: 0x06006763 RID: 26467 RVA: 0x00295B02 File Offset: 0x00293D02
		private void ApplyDecayPolicy(RankDefinitionSO rank, EmotionManager em)
		{
			if (rank.disableEmotionDecay)
			{
				if (!this._decayDisabledByRank)
				{
					this._decayPrevEnabled = em.EnableDecay;
					this._decayDisabledByRank = true;
				}
				em.EnableDecay = false;
				return;
			}
			this.RestoreDecayIfDisabled(em);
		}

		// Token: 0x06006764 RID: 26468 RVA: 0x00295B36 File Offset: 0x00293D36
		private void RestoreDecayIfDisabled(EmotionManager em)
		{
			if (!this._decayDisabledByRank || em == null)
			{
				return;
			}
			em.EnableDecay = this._decayPrevEnabled;
			this._decayDisabledByRank = false;
		}

		// Token: 0x06006765 RID: 26469 RVA: 0x00295B5D File Offset: 0x00293D5D
		private void EnsureFaceController(RankDefinitionSO rank)
		{
			if (this._faceController == null)
			{
				this._faceController = base.GetComponent<RankFaceController>() ?? base.gameObject.AddComponent<RankFaceController>();
			}
			this._faceController.Configure(rank);
		}

		// Token: 0x06006766 RID: 26470 RVA: 0x00295B94 File Offset: 0x00293D94
		private void ApplyStreetPresence(RankDefinitionSO rank)
		{
			PoliceManager instance = PoliceManager.Instance;
			if (instance != null)
			{
				if (!rank.enablePolice)
				{
					instance.enabled = false;
				}
				else if (RankRuntimeController.ProfileAllowsPolice())
				{
					instance.enabled = true;
				}
			}
			this.ApplyReporterState();
		}

		// Token: 0x06006767 RID: 26471 RVA: 0x00295BD8 File Offset: 0x00293DD8
		private void ApplyReporterState()
		{
			ReporterProceduralSpawner instance = ReporterProceduralSpawner.Instance;
			if (instance == null)
			{
				return;
			}
			instance.enabled = RankRuntimeController.ProfileAllowsReporters();
		}

		// Token: 0x06006768 RID: 26472 RVA: 0x00295C00 File Offset: 0x00293E00
		private void ApplyPreDetectionStance(RankDefinitionSO rank)
		{
			DetectionEmotionSource instance = DetectionEmotionSource.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetStanceAwarePreDetection(rank != null && rank.preDetectionFollowsStance);
		}

		// Token: 0x06006769 RID: 26473 RVA: 0x00295C38 File Offset: 0x00293E38
		private static bool ProfileAllowsPolice()
		{
			SceneContext sceneContext = SceneContext.Current;
			return sceneContext != null && sceneContext.Profile != null && sceneContext.Profile.police;
		}

		// Token: 0x0600676A RID: 26474 RVA: 0x00295C70 File Offset: 0x00293E70
		private static bool ProfileAllowsReporters()
		{
			SceneContext sceneContext = SceneContext.Current;
			return sceneContext != null && sceneContext.Profile != null && sceneContext.Profile.reporters && sceneContext.Profile.npcs;
		}

		// Token: 0x1700110A RID: 4362
		// (get) Token: 0x0600676B RID: 26475 RVA: 0x00295CB4 File Offset: 0x00293EB4
		public bool AllowsLingerieOpen
		{
			get
			{
				return RankProgressionService.CurrentRank != null && RankProgressionService.CurrentRank.allowLingerieOpenAnimations;
			}
		}

		// Token: 0x1700110B RID: 4363
		// (get) Token: 0x0600676C RID: 26476 RVA: 0x00295CCF File Offset: 0x00293ECF
		public bool AllowsMasturbationEmotes
		{
			get
			{
				return RankProgressionService.CurrentRank != null && RankProgressionService.CurrentRank.allowMasturbationEmotes;
			}
		}

		// Token: 0x1700110C RID: 4364
		// (get) Token: 0x0600676D RID: 26477 RVA: 0x00295CEA File Offset: 0x00293EEA
		public bool NextEncounterEndsRun
		{
			get
			{
				return !RunModeService.IsLongWayHome && this._lossesThisRun >= Mathf.Max(1, this.MaxLossesThisRun) - 1;
			}
		}

		// Token: 0x1700110D RID: 4365
		// (get) Token: 0x0600676E RID: 26478 RVA: 0x00295D0E File Offset: 0x00293F0E
		public EncounterChoiceMode ChoiceMode
		{
			get
			{
				if (this.NextEncounterEndsRun || RunModeService.IsLongWayHome || !(RankProgressionService.CurrentRank != null))
				{
					return EncounterChoiceMode.ForceShame;
				}
				return RankProgressionService.CurrentRank.choiceMode;
			}
		}

		// Token: 0x0600676F RID: 26479 RVA: 0x00295D38 File Offset: 0x00293F38
		public bool CanSlipPiece(string wornName)
		{
			if (this.IsAtHome)
			{
				return true;
			}
			RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
			return currentRank == null || currentRank.allowFullUndress || (currentRank.allowBraRemoval && RankRuntimeController.IsTopPiece(wornName));
		}

		// Token: 0x06006770 RID: 26480 RVA: 0x00295D7C File Offset: 0x00293F7C
		public bool CanUnwearItem(WearableItemSO item)
		{
			if (this.IsAtHome)
			{
				return true;
			}
			RankDefinitionSO currentRank = RankProgressionService.CurrentRank;
			return currentRank == null || currentRank.allowFullUndress || (currentRank.allowBraRemoval && item != null && item.mcStylePartId == "Outfit.Top");
		}

		// Token: 0x06006771 RID: 26481 RVA: 0x00295DD4 File Offset: 0x00293FD4
		private static bool IsTopPiece(string wornName)
		{
			if (string.IsNullOrEmpty(wornName))
			{
				return false;
			}
			RankProgressionConfigSO config = RankProgressionService.Config;
			string[] array2;
			if (!(config != null) || config.topPieceNameMarkers == null || config.topPieceNameMarkers.Length == 0)
			{
				string[] array = new string[2];
				array[0] = "bra";
				array2 = array;
				array[1] = "top";
			}
			else
			{
				array2 = config.topPieceNameMarkers;
			}
			string[] array3 = array2;
			string text = wornName.ToLowerInvariant();
			for (int i = 0; i < array3.Length; i++)
			{
				if (!string.IsNullOrEmpty(array3[i]) && text.Contains(array3[i].ToLowerInvariant()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006772 RID: 26482 RVA: 0x00295E5C File Offset: 0x0029405C
		private void HandleEncounterResolved()
		{
			this._lossesThisRun++;
			if (RunModeService.IsLongWayHome)
			{
				return;
			}
			int maxLossesThisRun = this.MaxLossesThisRun;
			Debug.Log(string.Format("[RankRuntimeController] Loss {0}/{1}.", this._lossesThisRun, maxLossesThisRun));
			if (this._lossesThisRun >= maxLossesThisRun)
			{
				Debug.Log("[RankRuntimeController] Loss max raggiunto → RankDefeat (PerpRun).");
				if (GameOverManager.Instance != null)
				{
					GameOverManager.Instance.TriggerRankDefeat();
				}
			}
		}

		// Token: 0x06006773 RID: 26483 RVA: 0x00295ED0 File Offset: 0x002940D0
		private void HandleGameOver(GameOverReason reason)
		{
			this.ApplyRunResults(reason.ToString());
		}

		// Token: 0x06006774 RID: 26484 RVA: 0x00295EE8 File Offset: 0x002940E8
		private void ApplyRunResults(string context)
		{
			if (this._runResultsApplied)
			{
				return;
			}
			this._runResultsApplied = true;
			if (this._tracker != null)
			{
				this._tracker.Freeze();
			}
			int num = 0;
			int num2 = 0;
			if (MatchStatsService.Instance != null)
			{
				MatchStatsSnapshot snapshot = MatchStatsService.Instance.GetSnapshot();
				num = snapshot.TimesSeen;
				num2 = snapshot.Points;
			}
			float num3 = ((this._tracker != null) ? this._tracker.PeakDistance : 0f);
			int num4 = ((this._tracker != null) ? this._tracker.ValidEmotes : 0);
			RunResult runResult = ((this._tracker != null) ? this._tracker.BuildRunResult(num, num2) : new RunResult
			{
				peakDistanceMeters = num3,
				timesSeen = num,
				validEmotes = num4,
				scorePoints = num2
			});
			runResult.startingRankId = this._startingRankId;
			runResult.startedAsAdmin = this._startedAsAdmin;
			runResult.skipOutfitIntroduction = !this._startedInGameplay || context == "DevTool";
			RunOutcome runOutcome = RankProgressionService.AddRunResults(runResult);
			this.LastRunOutcome = runOutcome;
			this.HasLastRunOutcome = true;
			Debug.Log(string.Concat(new string[]
			{
				"[RankRuntimeController] Run finita (",
				context,
				"). ",
				string.Format("Picco {0:0}m, visto {1}x, emote valide {2}, ", num3, num, num4),
				string.Format("score {0} (XP x{1}) => ", num2, runOutcome.scoreDistanceMultiplier),
				string.Format("+{0:0} XP, +{1} lvl (ora lvl {2}), ", runOutcome.xpGained, runOutcome.levelsGained, runOutcome.newLevel),
				string.Format("rank {0} (idx {1}).", runOutcome.rankChanged ? "PROMOSSO" : "invariato", runOutcome.newRankIndex)
			}));
		}

		// Token: 0x06006775 RID: 26485 RVA: 0x002960DD File Offset: 0x002942DD
		public void DebugForceApplyRunResults()
		{
			if (!this._runResultsApplied)
			{
				RunModeService.DisqualifyLeaderboardRun("Run results were forced with a debug control.");
			}
			this.ApplyRunResults("DevTool");
		}

		// Token: 0x06006776 RID: 26486 RVA: 0x002960FC File Offset: 0x002942FC
		public void DebugSimulateLoss()
		{
			RunModeService.DisqualifyLeaderboardRun("An encounter outcome was simulated with a debug control.");
			this.HandleEncounterResolved();
		}

		// Token: 0x040069FA RID: 27130
		private const string GateReason = "Rank";

		// Token: 0x040069FB RID: 27131
		private const string HomeSceneName = "CielHome";

		// Token: 0x040069FC RID: 27132
		[Header("Lifecycle")]
		[Tooltip("Debug host serializzato per scene fuori-run (es. CielHome). Se attivo, RankBootstrap mantiene questo [RankSystem] così l'Inspector può cambiare e applicare il rank a runtime.")]
		[SerializeField]
		private bool keepAliveOutsideRun;

		// Token: 0x040069FF RID: 27135
		private RankRunTracker _tracker;

		// Token: 0x04006A00 RID: 27136
		private GameOverManager _boundGameOver;

		// Token: 0x04006A01 RID: 27137
		private ExhibitionEventDirector _boundDirector;

		// Token: 0x04006A02 RID: 27138
		private RankOutfitApplier _outfitApplier;

		// Token: 0x04006A03 RID: 27139
		private Coroutine _outfitRoutine;

		// Token: 0x04006A04 RID: 27140
		private RankFaceController _faceController;

		// Token: 0x04006A05 RID: 27141
		private RankDefinitionSO _stanceAppliedRank;

		// Token: 0x04006A06 RID: 27142
		private string _startingRankId;

		// Token: 0x04006A07 RID: 27143
		private bool _startedAsAdmin;

		// Token: 0x04006A08 RID: 27144
		private bool _startedInGameplay;

		// Token: 0x04006A09 RID: 27145
		private string _observedRankId;

		// Token: 0x04006A0A RID: 27146
		private bool _runResultsApplied;

		// Token: 0x04006A0B RID: 27147
		private bool _gatePushed;

		// Token: 0x04006A0C RID: 27148
		private bool _outfitAppliedThisRun;

		// Token: 0x04006A0D RID: 27149
		private int _lossesThisRun;

		// Token: 0x04006A0E RID: 27150
		private bool _decayDisabledByRank;

		// Token: 0x04006A0F RID: 27151
		private bool _decayPrevEnabled;
	}
}
