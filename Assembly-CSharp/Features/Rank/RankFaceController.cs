using System;
using System.Collections.Generic;
using Features.Detection;
using Features.ExposeLingerie;
using Features.Inventory.Wearable;
using Features.PlayerEmotions;
using UnityEngine;

namespace Features.Rank
{
	// Token: 0x02000DA6 RID: 3494
	public class RankFaceController : MonoBehaviour
	{
		// Token: 0x170010E7 RID: 4327
		// (get) Token: 0x060066C0 RID: 26304 RVA: 0x002913D4 File Offset: 0x0028F5D4
		private bool DetectionOverridesFlash
		{
			get
			{
				return this._rank != null && this._rank.rankId == "curious";
			}
		}

		// Token: 0x170010E8 RID: 4328
		// (get) Token: 0x060066C1 RID: 26305 RVA: 0x002913FB File Offset: 0x0028F5FB
		private float CurrentHighestAttention
		{
			get
			{
				if (!(this._boundEmotion != null))
				{
					return 0f;
				}
				return this._boundEmotion.HighestCurrentAttention;
			}
		}

		// Token: 0x170010E9 RID: 4329
		// (get) Token: 0x060066C2 RID: 26306 RVA: 0x0029141C File Offset: 0x0028F61C
		private bool KeepsPlayerSelectedStance
		{
			get
			{
				return this._rank != null && string.Equals(this._rank.rankId, "hornyslut_declared", StringComparison.Ordinal);
			}
		}

		// Token: 0x060066C3 RID: 26307 RVA: 0x00291444 File Offset: 0x0028F644
		public void Configure(RankDefinitionSO rank)
		{
			bool flag = this._rank != rank;
			this._rank = rank;
			if (flag)
			{
				this._curiousSafeSince = -1f;
				this._hornySlutBareReaction = false;
				this._hornySlutBareReactionConsumed = false;
			}
			this.BuildPhysicalItemLists(rank);
			if (rank != null)
			{
				bool flag2 = false;
				if (this._bareActive && !rank.supremeWhileTopBare && !rank.supremeWhileFullyBare)
				{
					this._bareActive = false;
					flag2 = true;
				}
				if (this._flashActive && !this.SupportsFlashSupreme)
				{
					this._flashActive = false;
					this._flashCooldownUntil = -1f;
					flag2 = true;
				}
				if (this._towelDropped && !rank.embWhileTowelDropped)
				{
					this._towelDropped = false;
					flag2 = true;
				}
				if (this._lingerieActive && (!rank.allowLingerieOpenAnimations || !rank.confidentDuringLingerieOpen))
				{
					this._lingerieActive = false;
					this._lingerieCooldownUntil = 0f;
					flag2 = true;
				}
				if (flag2)
				{
					Debug.Log("[RankFace] Configure: stati persistenti non supportati dal nuovo rank azzerati.");
					this.ReassertPersistentStates();
				}
			}
			this.EnsureSubscribed();
		}

		// Token: 0x060066C4 RID: 26308 RVA: 0x00291538 File Offset: 0x0028F738
		private void BuildPhysicalItemLists(RankDefinitionSO rank)
		{
			this._topPhysicalItems.Clear();
			this._bottomPhysicalItems.Clear();
			this._topSeenWorn = false;
			this._bottomSeenWorn = false;
			if (rank == null || rank.startOutfitItems == null)
			{
				return;
			}
			foreach (WearableItemSO wearableItemSO in rank.startOutfitItems)
			{
				if (!(wearableItemSO == null))
				{
					if (wearableItemSO.physicallyCoversTop)
					{
						this._topPhysicalItems.Add(wearableItemSO);
					}
					if (wearableItemSO.physicallyCoversBottom)
					{
						this._bottomPhysicalItems.Add(wearableItemSO);
					}
				}
			}
		}

		// Token: 0x060066C5 RID: 26309 RVA: 0x002915EC File Offset: 0x0028F7EC
		private void OnEnable()
		{
			this.EnsureSubscribed();
		}

		// Token: 0x060066C6 RID: 26310 RVA: 0x002915F4 File Offset: 0x0028F7F4
		private void OnDisable()
		{
			if (this._boundEmotion != null)
			{
				this._boundEmotion.OnHighestDetectionAttentionChanged -= this.HandleAttentionChanged;
			}
			this._boundEmotion = null;
			if (this._boundDirector != null)
			{
				this._boundDirector.OnEncounterStarted -= this.HandleEncounterStarted;
				this._boundDirector.OnPlayerCategoryChosen -= this.HandleCategoryChosen;
			}
			this._boundDirector = null;
			this._boundWear = null;
			this.ClearReaction();
			this._hornySlutBareReactionConsumed = false;
			if (EmotionManager.Instance != null)
			{
				EmotionManager.Instance.SetDecaySuspended(false);
			}
		}

		// Token: 0x060066C7 RID: 26311 RVA: 0x0029169C File Offset: 0x0028F89C
		private void EnsureSubscribed()
		{
			if (this._boundEmotion == null)
			{
				this._boundEmotion = PlayerEmotionController.Instance;
				if (this._boundEmotion != null)
				{
					this._boundEmotion.OnHighestDetectionAttentionChanged += this.HandleAttentionChanged;
				}
			}
			if (this._boundDirector == null)
			{
				this._boundDirector = ExhibitionEventDirector.Instance;
				if (this._boundDirector != null)
				{
					this._boundDirector.OnEncounterStarted += this.HandleEncounterStarted;
					this._boundDirector.OnPlayerCategoryChosen += this.HandleCategoryChosen;
				}
			}
			if (this._boundWear == null)
			{
				this._boundWear = Object.FindObjectOfType<WearableUseController>();
			}
			if (this._towelItem == null)
			{
				this._towelItem = Resources.Load<WearableItemSO>("Wardrobe/Cloth_Towel");
			}
			if (this._boundLingerie == null)
			{
				this._boundLingerie = Object.FindObjectOfType<ExposeLingerieController>();
			}
		}

		// Token: 0x060066C8 RID: 26312 RVA: 0x0029178C File Offset: 0x0028F98C
		private void Update()
		{
			if (this._boundEmotion == null || this._boundDirector == null || this._boundLingerie == null || this._boundWear == null)
			{
				this.EnsureSubscribed();
			}
			this.UpdateCuriousDetectionRecovery(this.CurrentHighestAttention, Time.time);
			if (this._reactionActive && this._rank != null && (this._encounterReaction || this._rank.detectionReaction != DetectionFaceReaction.EmbSupremeLatched))
			{
				bool flag = this._boundDirector != null && this._boundDirector.IsEventActive;
				bool flag2 = this._encounterReaction || (Time.time >= this._timedUntil && (this._hornySlutBareReaction || this.CurrentHighestAttention < 0.5f));
				if (!flag && flag2)
				{
					this.RestoreAfterTimed();
				}
				else if (flag && this._encounterReaction)
				{
					this.EnforceEncounterSupreme();
				}
				else if (this._hornySlutBareReaction)
				{
					this.EnforceHornySlutBareSupreme();
				}
			}
			this.PollBare();
			this.TryStartHornySlutBareReactionAboveThreshold();
			this.PollFlash();
			this.PollTowel();
			this.PollLingerie();
			this.UpdateDecaySuspension();
		}

		// Token: 0x060066C9 RID: 26313 RVA: 0x002918C4 File Offset: 0x0028FAC4
		private void UpdateDecaySuspension()
		{
			EmotionManager instance = EmotionManager.Instance;
			if (instance == null)
			{
				return;
			}
			if (this._rank == null)
			{
				instance.SetDecaySuspended(false);
				return;
			}
			bool flag = this._reactionActive || this._towelDropped || this._bareActive || (this._flashActive && this.SupportsFlashSupreme) || this._lingerieActive;
			instance.SetDecaySuspended(flag);
		}

		// Token: 0x060066CA RID: 26314 RVA: 0x00291930 File Offset: 0x0028FB30
		private void PollLingerie()
		{
			if (this._rank == null || !this._rank.allowLingerieOpenAnimations || !this._rank.confidentDuringLingerieOpen)
			{
				return;
			}
			if (this._boundLingerie == null)
			{
				return;
			}
			bool anyOpen = this._boundLingerie.AnyOpen;
			if (anyOpen && !this._lingerieOpenNow)
			{
				this._lingerieCooldownUntil = 0f;
				if (!this._lingerieActive)
				{
					this._lingerieActive = true;
					this.ApplyLingerieConfident();
				}
			}
			else if (!anyOpen && this._lingerieOpenNow)
			{
				RankProgressionConfigSO config = RankProgressionService.Config;
				float num = ((config != null) ? Mathf.Max(0f, config.lingerieConfidentCooldownSeconds) : 3f);
				this._lingerieCooldownUntil = Time.time + num;
			}
			this._lingerieOpenNow = anyOpen;
			if (this._lingerieActive && !anyOpen && this._lingerieCooldownUntil > 0f && Time.time >= this._lingerieCooldownUntil)
			{
				this._lingerieActive = false;
				this._lingerieCooldownUntil = 0f;
				this.ExitLingerieConfident();
			}
		}

		// Token: 0x060066CB RID: 26315 RVA: 0x00291A34 File Offset: 0x0028FC34
		private void ApplyLingerieConfident()
		{
			if (this._reactionActive || this._towelDropped || this._bareActive || this._flashActive)
			{
				return;
			}
			RankProgressionConfigSO config = RankProgressionService.Config;
			float num = ((config != null) ? config.lingerieConfidentBalance : 80f);
			this.ApplyAutomaticPositiveState(num, "RankLingerie", true);
		}

		// Token: 0x060066CC RID: 26316 RVA: 0x00291A8C File Offset: 0x0028FC8C
		private void ExitLingerieConfident()
		{
			this.ReassertPersistentStates();
		}

		// Token: 0x170010EA RID: 4330
		// (get) Token: 0x060066CD RID: 26317 RVA: 0x00291A94 File Offset: 0x0028FC94
		private bool SupportsFlashSupreme
		{
			get
			{
				return this._rank != null && (this._rank.confidentSupremeDuringFlash || (this._rank.allowLingerieOpenAnimations && this._rank.confidentDuringLingerieOpen));
			}
		}

		// Token: 0x060066CE RID: 26318 RVA: 0x00291AD0 File Offset: 0x0028FCD0
		private void PollFlash()
		{
			if (!this.SupportsFlashSupreme)
			{
				return;
			}
			bool flag = this._rank.confidentSupremeDuringFlash && PlayerExposureState.IsFlashActive;
			bool flag2 = this._rank.allowLingerieOpenAnimations && this._rank.confidentDuringLingerieOpen && this._boundLingerie != null && (this._boundLingerie.AnyOpen || this._boundLingerie.IsPlaying);
			this.UpdateFlashState(flag || flag2, Time.time);
		}

		// Token: 0x060066CF RID: 26319 RVA: 0x00291B54 File Offset: 0x0028FD54
		private void UpdateFlashState(bool flashing, float now)
		{
			if (flashing)
			{
				this._flashCooldownUntil = -1f;
				if (!this._flashActive)
				{
					EmotionManager instance = EmotionManager.Instance;
					if (instance != null)
					{
						this._preFlashBalance = instance.EmotionBalance;
					}
					this._flashActive = true;
				}
				if (this._reactionActive && !this._encounterReaction && !this.DetectionOverridesFlash)
				{
					this._reactionActive = false;
					this._hornySlutBareReaction = false;
				}
				this.ApplyFlashSupreme();
				return;
			}
			if (!this._flashActive)
			{
				return;
			}
			if (this._flashCooldownUntil < 0f)
			{
				this._flashCooldownUntil = now + Mathf.Max(0f, this._rank.flashSupremeCooldownSeconds);
			}
			if (now < this._flashCooldownUntil)
			{
				this.ApplyFlashSupreme();
				return;
			}
			this._flashActive = false;
			this._flashCooldownUntil = -1f;
			if (!this._reactionActive && !this._towelDropped && !this._bareActive)
			{
				EmotionManager instance2 = EmotionManager.Instance;
				if (instance2 != null)
				{
					this.RestoreRankLock();
					this.RestoreBalancePreservingDeclaredChoice(instance2, this._preFlashBalance, "RankFlash");
				}
				this.ReassertPersistentStates();
				if (this.CurrentHighestAttention >= 0.5f && this._rank.detectionReaction != DetectionFaceReaction.EmbSupremeEncounterOnly)
				{
					this._triggered = true;
					this.BeginReaction();
					return;
				}
			}
			else if (!this._reactionActive && !this._towelDropped && this._bareActive && this.IsNormalHornySlutFullyBare() && this.CurrentHighestAttention >= 0.5f && this._rank.detectionReaction != DetectionFaceReaction.EmbSupremeEncounterOnly)
			{
				this._triggered = true;
				this.BeginReaction();
			}
		}

		// Token: 0x060066D0 RID: 26320 RVA: 0x00291CD1 File Offset: 0x0028FED1
		private void ApplyFlashSupreme()
		{
			if (this._reactionActive || this._towelDropped || this._bareActive)
			{
				return;
			}
			this.ApplyAutomaticPositiveState(100f, "RankFlash", true);
		}

		// Token: 0x060066D1 RID: 26321 RVA: 0x00291D00 File Offset: 0x0028FF00
		private void HandleAttentionChanged(float previous, float current)
		{
			if (this._rank == null)
			{
				return;
			}
			if (!this._triggered && current >= 0.5f && previous < 0.5f)
			{
				this._triggered = true;
				this.PollBare();
				this.PollFlash();
				if ((!this._flashActive || !this.SupportsFlashSupreme || this.DetectionOverridesFlash) && this._rank.detectionReaction != DetectionFaceReaction.EmbSupremeEncounterOnly)
				{
					this.BeginReaction();
				}
			}
			if (current < 0.25f)
			{
				this._triggered = false;
				this._hornySlutBareReactionConsumed = false;
				if (this._reactionActive && !this._encounterReaction && this._rank.detectionReaction == DetectionFaceReaction.EmbSupremeLatched && !this.DetectionOverridesFlash)
				{
					this.ClearReaction();
				}
			}
			this.UpdateCuriousDetectionRecovery(current, Time.time);
		}

		// Token: 0x060066D2 RID: 26322 RVA: 0x00291DC8 File Offset: 0x0028FFC8
		private void UpdateCuriousDetectionRecovery(float attention, float now)
		{
			if (!this.DetectionOverridesFlash || !this._reactionActive || this._encounterReaction)
			{
				return;
			}
			if (attention >= 0.25f)
			{
				this._curiousSafeSince = -1f;
			}
			else
			{
				if (this._curiousSafeSince < 0f)
				{
					this._curiousSafeSince = now;
				}
				if (now - this._curiousSafeSince >= Mathf.Max(0f, this._rank.detectionEscapeRecoverySeconds))
				{
					this.ClearReaction();
					return;
				}
			}
			EmotionManager instance = EmotionManager.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetStanceLock(new EmotionStance?(EmotionStance.Embarassed));
			if (!Mathf.Approximately(instance.EmotionBalance, -100f))
			{
				instance.SetBalance(-100f, "RankDetection");
			}
		}

		// Token: 0x060066D3 RID: 26323 RVA: 0x00291E7B File Offset: 0x0029007B
		private void BeginReaction()
		{
			this.BeginReaction("RankDetection");
		}

		// Token: 0x060066D4 RID: 26324 RVA: 0x00291E88 File Offset: 0x00290088
		private void BeginReaction(string source)
		{
			EmotionManager instance = EmotionManager.Instance;
			if (instance == null)
			{
				return;
			}
			this._preReactionBalance = instance.EmotionBalance;
			this._reactionActive = true;
			this._curiousSafeSince = -1f;
			bool flag = this.IsNormalHornySlutFullyBare();
			DetectionFaceReaction detectionFaceReaction = RankFaceController.ResolveDetectionReaction(this._rank, instance.CurrentStance, flag);
			this._hornySlutBareReaction = flag;
			if (flag)
			{
				this._hornySlutBareReactionConsumed = true;
			}
			Debug.Log(string.Format("[RankFace] BeginReaction ({0}): {1}, preBalance={2:0.#}", source, detectionFaceReaction, this._preReactionBalance));
			switch (detectionFaceReaction)
			{
			case DetectionFaceReaction.EmbSupremeTimed:
			case DetectionFaceReaction.EmbSupremeEncounterOnly:
				instance.SetStanceLock(this.KeepsPlayerSelectedStance ? null : new EmotionStance?(EmotionStance.Embarassed));
				instance.SetBalance(-100f, source);
				this._timedUntil = Time.time + Mathf.Max(0.1f, this._rank.detectionReactionSeconds);
				return;
			case DetectionFaceReaction.HornySupreme:
				instance.SetStanceLock(this.KeepsPlayerSelectedStance ? null : new EmotionStance?(EmotionStance.Confident));
				instance.SetBalance(100f, source);
				this._timedUntil = Time.time + Mathf.Max(0.1f, this._rank.detectionReactionSeconds);
				return;
			}
			instance.SetStanceLock(new EmotionStance?(EmotionStance.Embarassed));
			instance.SetBalance(-100f, source);
		}

		// Token: 0x060066D5 RID: 26325 RVA: 0x00291FDC File Offset: 0x002901DC
		private static DetectionFaceReaction ResolveDetectionReaction(RankDefinitionSO rank, EmotionStance currentStance, bool normalHornySlutFullyBare)
		{
			if (rank != null && normalHornySlutFullyBare && string.Equals(rank.rankId, "hornyslut", StringComparison.Ordinal))
			{
				return DetectionFaceReaction.EmbSupremeTimed;
			}
			if (rank != null && string.Equals(rank.rankId, "hornyslut_declared", StringComparison.Ordinal))
			{
				if (currentStance != EmotionStance.Embarassed)
				{
					return DetectionFaceReaction.HornySupreme;
				}
				return DetectionFaceReaction.EmbSupremeTimed;
			}
			else
			{
				if (!(rank != null))
				{
					return DetectionFaceReaction.EmbSupremeLatched;
				}
				return rank.detectionReaction;
			}
		}

		// Token: 0x060066D6 RID: 26326 RVA: 0x00292040 File Offset: 0x00290240
		private void HandleEncounterStarted()
		{
			if (this._rank == null)
			{
				return;
			}
			EmotionManager instance = EmotionManager.Instance;
			if (instance == null)
			{
				return;
			}
			this._triggered = true;
			this._encounterCategory = null;
			this._encounterReaction = true;
			this._hornySlutBareReaction = false;
			this._reactionActive = true;
			this._preReactionBalance = instance.EmotionBalance;
			this._timedUntil = Time.time + Mathf.Max(0.1f, this._rank.detectionReactionSeconds);
			instance.SetStanceLock(new EmotionStance?(EmotionStance.Embarassed));
			instance.SetBalance(-100f, "RankEncounter");
			Debug.Log("[RankFace] HandleEncounterStarted: EmbSupreme default (rank " + this._rank.displayName + ").");
		}

		// Token: 0x060066D7 RID: 26327 RVA: 0x002920FC File Offset: 0x002902FC
		private void HandleCategoryChosen(ChoiceCategory category, DetectionActorType actor)
		{
			if (this._rank == null)
			{
				return;
			}
			if (!this._reactionActive || !this._encounterReaction)
			{
				return;
			}
			this._encounterCategory = new ChoiceCategory?(category);
			Debug.Log(string.Format("[RankFace] CategoryChosen: {0} → {1}.", category, (category == ChoiceCategory.Exhibit) ? "supreme+ " : "EmbSupreme"));
			this.EnforceEncounterSupreme();
		}

		// Token: 0x060066D8 RID: 26328 RVA: 0x00292160 File Offset: 0x00290360
		private void EnforceEncounterSupreme()
		{
			EmotionManager instance = EmotionManager.Instance;
			if (instance == null)
			{
				return;
			}
			ChoiceCategory? encounterCategory = this._encounterCategory;
			ChoiceCategory choiceCategory = ChoiceCategory.Exhibit;
			bool flag = (encounterCategory.GetValueOrDefault() == choiceCategory) & (encounterCategory != null);
			float num = (flag ? 100f : (-100f));
			if (Mathf.Approximately(instance.EmotionBalance, num))
			{
				return;
			}
			instance.SetStanceLock(new EmotionStance?(flag ? EmotionStance.Confident : EmotionStance.Embarassed));
			instance.SetBalance(num, "RankEncounter");
		}

		// Token: 0x060066D9 RID: 26329 RVA: 0x002921D8 File Offset: 0x002903D8
		private void EnforceHornySlutBareSupreme()
		{
			EmotionManager instance = EmotionManager.Instance;
			if (instance == null)
			{
				return;
			}
			EmotionStance? stanceLock = instance.StanceLock;
			EmotionStance emotionStance = EmotionStance.Embarassed;
			if (!((stanceLock.GetValueOrDefault() == emotionStance) & (stanceLock != null)))
			{
				instance.SetStanceLock(new EmotionStance?(EmotionStance.Embarassed));
			}
			if (!Mathf.Approximately(instance.EmotionBalance, -100f))
			{
				instance.SetBalance(-100f, "RankDetection");
			}
		}

		// Token: 0x060066DA RID: 26330 RVA: 0x00292240 File Offset: 0x00290440
		private void RestoreAfterTimed()
		{
			this._reactionActive = false;
			this._encounterReaction = false;
			this._hornySlutBareReaction = false;
			if (this._rank != null && this._rank.detectionReaction == DetectionFaceReaction.EmbSupremeEncounterOnly && this._encounterCategory != null)
			{
				ChoiceCategory? encounterCategory = this._encounterCategory;
				ChoiceCategory choiceCategory = ChoiceCategory.Exhibit;
				bool flag = (encounterCategory.GetValueOrDefault() == choiceCategory) & (encounterCategory != null);
				Debug.Log(string.Format("[RankFace] RestoreAfterTimed (admin): stato finale {0} (scelta {1}).", flag ? "HornySupreme +100" : "EmbSupreme -100", this._encounterCategory));
				this._encounterCategory = null;
				this.RestoreRankLock();
				EmotionManager instance = EmotionManager.Instance;
				if (instance != null)
				{
					instance.SetBalance(flag ? 100f : (-100f), "RankEncounter");
				}
				return;
			}
			this._encounterCategory = null;
			Debug.Log(string.Format("[RankFace] RestoreAfterTimed: persistente={0}, preBalance={1:0.#}", this.HasPersistentState, this._preReactionBalance));
			if (this.HasPersistentState)
			{
				this.ReassertPersistentStates();
				return;
			}
			this.RestoreRankLock();
			EmotionManager instance2 = EmotionManager.Instance;
			if (instance2 != null)
			{
				this.RestoreBalancePreservingDeclaredChoice(instance2, this._preReactionBalance, "RankDetection");
			}
		}

		// Token: 0x060066DB RID: 26331 RVA: 0x00292380 File Offset: 0x00290580
		private void ClearReaction()
		{
			this._curiousSafeSince = -1f;
			this._reactionActive = false;
			this._encounterReaction = false;
			this._hornySlutBareReaction = false;
			this._encounterCategory = null;
			if (this.HasPersistentState)
			{
				this.ReassertPersistentStates();
				return;
			}
			this.RestoreRankLock();
		}

		// Token: 0x170010EB RID: 4331
		// (get) Token: 0x060066DC RID: 26332 RVA: 0x002923CE File Offset: 0x002905CE
		private bool HasPersistentState
		{
			get
			{
				return this._towelDropped || this._bareActive || (this._flashActive && this.SupportsFlashSupreme) || this._lingerieActive;
			}
		}

		// Token: 0x060066DD RID: 26333 RVA: 0x002923F8 File Offset: 0x002905F8
		private void RestoreRankLock()
		{
			EmotionManager instance = EmotionManager.Instance;
			if (instance == null)
			{
				return;
			}
			if (this._rank != null)
			{
				instance.SetStanceLock(this._rank.allowStanceSwitch ? null : new EmotionStance?(this._rank.defaultStance));
				return;
			}
			instance.SetStanceLock(null);
		}

		// Token: 0x060066DE RID: 26334 RVA: 0x00292464 File Offset: 0x00290664
		private void ApplyAutomaticPositiveState(float magnitude, string source, bool lockConfidentForOtherRanks)
		{
			EmotionManager instance = EmotionManager.Instance;
			if (instance == null)
			{
				return;
			}
			float num = Mathf.Abs(magnitude);
			if (this.KeepsPlayerSelectedStance)
			{
				EmotionStance currentStance = instance.CurrentStance;
				instance.SetStanceLock(null);
				instance.SetBalance((currentStance == EmotionStance.Embarassed) ? (-num) : num, source);
				return;
			}
			if (lockConfidentForOtherRanks)
			{
				instance.SetStanceLock(new EmotionStance?(EmotionStance.Confident));
			}
			else
			{
				this.RestoreRankLock();
			}
			instance.SetBalance(num, source);
		}

		// Token: 0x060066DF RID: 26335 RVA: 0x002924D8 File Offset: 0x002906D8
		private void RestoreBalancePreservingDeclaredChoice(EmotionManager em, float previousBalance, string source)
		{
			if (this.KeepsPlayerSelectedStance)
			{
				bool flag = previousBalance < 0f;
				bool flag2 = em.CurrentStance == EmotionStance.Embarassed;
				if (flag != flag2)
				{
					return;
				}
			}
			em.SetBalance(previousBalance, source);
		}

		// Token: 0x060066E0 RID: 26336 RVA: 0x0029250C File Offset: 0x0029070C
		private void ReassertPersistentStates()
		{
			if (this._reactionActive)
			{
				return;
			}
			EmotionManager instance = EmotionManager.Instance;
			if (instance == null)
			{
				return;
			}
			if (this._towelDropped)
			{
				instance.SetStanceLock(new EmotionStance?(EmotionStance.Embarassed));
				instance.SetBalance(-100f, "RankTowelDropped");
				return;
			}
			if (this._bareActive)
			{
				this.ApplyAutomaticPositiveState(100f, "RankBare", true);
				return;
			}
			if (this._flashActive && this.SupportsFlashSupreme)
			{
				this.ApplyFlashSupreme();
				return;
			}
			if (this._lingerieActive)
			{
				this.ApplyLingerieConfident();
				return;
			}
			this.RestoreRankLock();
		}

		// Token: 0x060066E1 RID: 26337 RVA: 0x002925A0 File Offset: 0x002907A0
		private void PollBare()
		{
			if (this._rank == null)
			{
				return;
			}
			bool supremeWhileTopBare = this._rank.supremeWhileTopBare;
			bool supremeWhileFullyBare = this._rank.supremeWhileFullyBare;
			if (!supremeWhileTopBare && !supremeWhileFullyBare)
			{
				return;
			}
			if (this._boundWear == null)
			{
				return;
			}
			bool flag = this.SamplePhysicalBareState(supremeWhileTopBare, supremeWhileFullyBare);
			if (flag && !this._bareActive)
			{
				this.EnterBare();
				return;
			}
			if (!flag && this._bareActive)
			{
				this.ExitBare();
			}
		}

		// Token: 0x060066E2 RID: 26338 RVA: 0x00292618 File Offset: 0x00290818
		private bool SamplePhysicalBareState(bool useTopOnly, bool useFully)
		{
			if (this._boundWear == null)
			{
				return false;
			}
			bool flag = this.AnyWorn(this._topPhysicalItems);
			bool flag2 = this.AnyWorn(this._bottomPhysicalItems);
			if (flag)
			{
				this._topSeenWorn = true;
			}
			if (flag2)
			{
				this._bottomSeenWorn = true;
			}
			bool flag3 = this._topSeenWorn && !flag;
			if (useFully)
			{
				bool flag4 = this._bottomSeenWorn && !flag2;
				return flag3 && flag4;
			}
			return useTopOnly && flag3;
		}

		// Token: 0x060066E3 RID: 26339 RVA: 0x0029268D File Offset: 0x0029088D
		private bool IsNormalHornySlutFullyBare()
		{
			return this._rank != null && string.Equals(this._rank.rankId, "hornyslut", StringComparison.Ordinal) && this.SamplePhysicalBareState(false, true);
		}

		// Token: 0x060066E4 RID: 26340 RVA: 0x002926C0 File Offset: 0x002908C0
		private void TryStartHornySlutBareReactionAboveThreshold()
		{
			if (this._hornySlutBareReactionConsumed || this._encounterReaction || this._flashActive)
			{
				return;
			}
			if (!this._bareActive || !this.IsNormalHornySlutFullyBare())
			{
				return;
			}
			if (this.CurrentHighestAttention < 0.5f)
			{
				return;
			}
			this._triggered = true;
			this.BeginReaction();
		}

		// Token: 0x060066E5 RID: 26341 RVA: 0x00292714 File Offset: 0x00290914
		private bool AnyWorn(List<WearableItemSO> items)
		{
			if (items == null || this._boundWear == null)
			{
				return false;
			}
			for (int i = 0; i < items.Count; i++)
			{
				if (items[i] != null && this._boundWear.IsWorn(items[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060066E6 RID: 26342 RVA: 0x0029276C File Offset: 0x0029096C
		private void EnterBare()
		{
			this._bareActive = true;
			Debug.Log("[RankFace] EnterBare: pezzo/i tolti → supreme positivo (+100).");
			if (this._reactionActive || this._towelDropped)
			{
				return;
			}
			if (EmotionManager.Instance == null)
			{
				return;
			}
			this.ApplyAutomaticPositiveState(100f, "RankBare", true);
		}

		// Token: 0x060066E7 RID: 26343 RVA: 0x002927BA File Offset: 0x002909BA
		private void ExitBare()
		{
			this._bareActive = false;
			Debug.Log("[RankFace] ExitBare: pezzo ri-indossato → catena ripristino.");
			this.ReassertPersistentStates();
		}

		// Token: 0x060066E8 RID: 26344 RVA: 0x002927D4 File Offset: 0x002909D4
		private void PollTowel()
		{
			if (this._rank == null || !this._rank.embWhileTowelDropped)
			{
				return;
			}
			if (this._boundWear == null || this._towelItem == null)
			{
				return;
			}
			if (this._boundWear.IsWorn(this._towelItem))
			{
				this._towelSeenWorn = true;
				if (this._towelDropped)
				{
					this.ExitTowelDropped();
					return;
				}
			}
			else if (this._towelSeenWorn && !this._towelDropped)
			{
				this.EnterTowelDropped();
			}
		}

		// Token: 0x060066E9 RID: 26345 RVA: 0x0029285C File Offset: 0x00290A5C
		private void EnterTowelDropped()
		{
			this._towelDropped = true;
			Debug.Log("[RankFace] EnterTowelDropped: towel non addosso → stato EMB.");
			EmotionManager instance = EmotionManager.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SetStanceLock(new EmotionStance?(EmotionStance.Embarassed));
			if (!this._reactionActive)
			{
				instance.SetBalance(-100f, "RankTowelDropped");
			}
		}

		// Token: 0x060066EA RID: 26346 RVA: 0x002928AE File Offset: 0x00290AAE
		private void ExitTowelDropped()
		{
			this._towelDropped = false;
			Debug.Log("[RankFace] ExitTowelDropped: towel ri-indossato → catena ripristino.");
			this.ReassertPersistentStates();
		}

		// Token: 0x0400696F RID: 26991
		private const string HornySlutRankId = "hornyslut";

		// Token: 0x04006970 RID: 26992
		private const string HornySlutDeclaredRankId = "hornyslut_declared";

		// Token: 0x04006971 RID: 26993
		private const float TriggerThreshold = 0.5f;

		// Token: 0x04006972 RID: 26994
		private const float ResetThreshold = 0.25f;

		// Token: 0x04006973 RID: 26995
		private const string BalanceSource = "RankDetection";

		// Token: 0x04006974 RID: 26996
		private const string EncounterSource = "RankEncounter";

		// Token: 0x04006975 RID: 26997
		private const string FlashSource = "RankFlash";

		// Token: 0x04006976 RID: 26998
		private const string TowelSource = "RankTowelDropped";

		// Token: 0x04006977 RID: 26999
		private const string LingerieSource = "RankLingerie";

		// Token: 0x04006978 RID: 27000
		private const string BareSource = "RankBare";

		// Token: 0x04006979 RID: 27001
		private const string TowelResourcePath = "Wardrobe/Cloth_Towel";

		// Token: 0x0400697A RID: 27002
		private RankDefinitionSO _rank;

		// Token: 0x0400697B RID: 27003
		private PlayerEmotionController _boundEmotion;

		// Token: 0x0400697C RID: 27004
		private ExhibitionEventDirector _boundDirector;

		// Token: 0x0400697D RID: 27005
		private WearableUseController _boundWear;

		// Token: 0x0400697E RID: 27006
		private WearableItemSO _towelItem;

		// Token: 0x0400697F RID: 27007
		private bool _towelSeenWorn;

		// Token: 0x04006980 RID: 27008
		private ExposeLingerieController _boundLingerie;

		// Token: 0x04006981 RID: 27009
		private bool _triggered;

		// Token: 0x04006982 RID: 27010
		private bool _reactionActive;

		// Token: 0x04006983 RID: 27011
		private bool _encounterReaction;

		// Token: 0x04006984 RID: 27012
		private bool _hornySlutBareReaction;

		// Token: 0x04006985 RID: 27013
		private bool _hornySlutBareReactionConsumed;

		// Token: 0x04006986 RID: 27014
		private float _timedUntil;

		// Token: 0x04006987 RID: 27015
		private float _preReactionBalance;

		// Token: 0x04006988 RID: 27016
		private float _curiousSafeSince = -1f;

		// Token: 0x04006989 RID: 27017
		private ChoiceCategory? _encounterCategory;

		// Token: 0x0400698A RID: 27018
		private bool _flashActive;

		// Token: 0x0400698B RID: 27019
		private float _flashCooldownUntil = -1f;

		// Token: 0x0400698C RID: 27020
		private float _preFlashBalance;

		// Token: 0x0400698D RID: 27021
		private bool _towelDropped;

		// Token: 0x0400698E RID: 27022
		private bool _lingerieActive;

		// Token: 0x0400698F RID: 27023
		private bool _lingerieOpenNow;

		// Token: 0x04006990 RID: 27024
		private float _lingerieCooldownUntil;

		// Token: 0x04006991 RID: 27025
		private bool _bareActive;

		// Token: 0x04006992 RID: 27026
		private bool _topSeenWorn;

		// Token: 0x04006993 RID: 27027
		private bool _bottomSeenWorn;

		// Token: 0x04006994 RID: 27028
		private readonly List<WearableItemSO> _topPhysicalItems = new List<WearableItemSO>();

		// Token: 0x04006995 RID: 27029
		private readonly List<WearableItemSO> _bottomPhysicalItems = new List<WearableItemSO>();
	}
}
