using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using DA_Assets.DAB;
using Features.Achievements;
using Features.Detection;
using Features.GameOver;
using Features.Loading;
using Features.Localization;
using Features.Missions;
using Features.Rank;
using Features.Score;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.PoiTravel;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Features.MatchRecap
{
	// Token: 0x02000FC7 RID: 4039
	[DisallowMultipleComponent]
	public class MatchRecapController : MonoBehaviour
	{
		// Token: 0x170013BE RID: 5054
		// (get) Token: 0x060079EE RID: 31214 RVA: 0x0031CDD8 File Offset: 0x0031AFD8
		// (set) Token: 0x060079EF RID: 31215 RVA: 0x0031CDDF File Offset: 0x0031AFDF
		public static MatchRecapController Instance { get; private set; }

		// Token: 0x170013BF RID: 5055
		// (get) Token: 0x060079F0 RID: 31216 RVA: 0x0031CDE7 File Offset: 0x0031AFE7
		// (set) Token: 0x060079F1 RID: 31217 RVA: 0x0031CDEF File Offset: 0x0031AFEF
		public bool IsShowing { get; private set; }

		// Token: 0x060079F2 RID: 31218 RVA: 0x0031CDF8 File Offset: 0x0031AFF8
		private void Awake()
		{
			if (MatchRecapController.Instance != null && MatchRecapController.Instance != this)
			{
				Object.Destroy(this);
				return;
			}
			MatchRecapController.Instance = this;
			this.BindRecapChrome();
			this.EnsureScoreKpiRow();
			this.DisableLegacyRankRows();
			this.EnsureRankProgressWidget();
			this.ResetVisualStateImmediate();
			this.WireButton(this.tryAgainButton, new UnityAction(this.OnTryAgainClicked), true);
			this.WireButton(this.continueToPrisonButton, new UnityAction(this.OnContinueToPrisonClicked), true);
		}

		// Token: 0x060079F3 RID: 31219 RVA: 0x0031CE7C File Offset: 0x0031B07C
		private void OnEnable()
		{
			GameLocalization.LocaleChanged += this.RefreshHomeMissionLocale;
			this.TryRegisterGameOverListener();
			if (this.IsShowing && this._showRoutine == null && base.isActiveAndEnabled)
			{
				this.ResumeShowSequence();
			}
		}

		// Token: 0x060079F4 RID: 31220 RVA: 0x0031CEB4 File Offset: 0x0031B0B4
		private void BindRecapChrome()
		{
			foreach (TMP_Text tmp_Text in ((this.rootObject != null) ? this.rootObject.transform : base.transform).GetComponentsInChildren<TMP_Text>(true))
			{
				if (tmp_Text == this.distanceLabel || tmp_Text == this.distanceWithVehiclesLabel || tmp_Text == this.timesSeenLabel || tmp_Text == this.timesExhibitionedLabel || tmp_Text == this.locationsVisitedLabel || tmp_Text == this.carSurfCountLabel || tmp_Text == this.averageHeartbeatLabel || tmp_Text == this.rankXpGainedLabel || tmp_Text == this.rankLevelLabel || tmp_Text == this.rankUpLabel)
				{
					LocalizedGameText.BindFontOnly(tmp_Text);
				}
				else
				{
					LocalizedGameText.BindSource(tmp_Text, "rank_ui", tmp_Text.text, null).FitTranslationToRectangle(false);
				}
			}
		}

		// Token: 0x060079F5 RID: 31221 RVA: 0x0031CFAE File Offset: 0x0031B1AE
		private void Start()
		{
			this.TryRegisterGameOverListener();
		}

		// Token: 0x060079F6 RID: 31222 RVA: 0x0031CFB8 File Offset: 0x0031B1B8
		private void OnDisable()
		{
			GameLocalization.LocaleChanged -= this.RefreshHomeMissionLocale;
			base.StopAllCoroutines();
			this._showRoutine = null;
			this.RestoreCursor();
			this.ResetVisualStateImmediate();
			if (this._gameOverListenerRegistered && GameOverManager.Instance != null)
			{
				GameOverManager.Instance.OnGameOver.RemoveListener(new UnityAction<GameOverReason>(this.OnGameOverTriggered));
			}
			this._gameOverListenerRegistered = false;
		}

		// Token: 0x060079F7 RID: 31223 RVA: 0x0031D028 File Offset: 0x0031B228
		private void OnDestroy()
		{
			this.WireButton(this.tryAgainButton, new UnityAction(this.OnTryAgainClicked), false);
			this.WireButton(this.continueToPrisonButton, new UnityAction(this.OnContinueToPrisonClicked), false);
			if (MatchRecapController.Instance == this)
			{
				MatchRecapController.Instance = null;
			}
		}

		// Token: 0x060079F8 RID: 31224 RVA: 0x0031D07C File Offset: 0x0031B27C
		private void TryRegisterGameOverListener()
		{
			if (this._gameOverListenerRegistered)
			{
				return;
			}
			if (!this.autoShowOnGameOver)
			{
				return;
			}
			if (GameOverManager.Instance == null)
			{
				return;
			}
			GameOverManager.Instance.OnGameOver.AddListener(new UnityAction<GameOverReason>(this.OnGameOverTriggered));
			this._gameOverListenerRegistered = true;
			if (GameOverManager.Instance.IsGameOver)
			{
				this.OnGameOverTriggered(GameOverManager.Instance.CurrentReason);
			}
		}

		// Token: 0x060079F9 RID: 31225 RVA: 0x0031D0E7 File Offset: 0x0031B2E7
		public void Show()
		{
			if (this.IsShowing)
			{
				return;
			}
			this.EnsureGameObjectActive();
			this.IsShowing = true;
			this._showAtRealtime = Time.realtimeSinceStartup;
			this.ResumeShowSequence();
		}

		// Token: 0x060079FA RID: 31226 RVA: 0x0031D110 File Offset: 0x0031B310
		public void Hide()
		{
			if (!this.IsShowing)
			{
				return;
			}
			this.IsShowing = false;
			base.StopAllCoroutines();
			this._showRoutine = null;
			if (base.isActiveAndEnabled)
			{
				this._showRoutine = base.StartCoroutine(this.PlayHideSequence());
				return;
			}
			this.ResetVisualStateImmediate();
			this.RestoreCursor();
		}

		// Token: 0x060079FB RID: 31227 RVA: 0x0031D161 File Offset: 0x0031B361
		public void Toggle()
		{
			if (this.IsShowing)
			{
				this.Hide();
				return;
			}
			this.Show();
		}

		// Token: 0x060079FC RID: 31228 RVA: 0x0031D178 File Offset: 0x0031B378
		private void OnGameOverTriggered(GameOverReason reason)
		{
			GameOverManager instance = GameOverManager.Instance;
			if (instance != null && this._observedGameOver == instance && this._observedGameOverSequence == instance.GameOverSequenceId)
			{
				return;
			}
			this._observedGameOver = instance;
			this._observedGameOverSequence = ((instance != null) ? instance.GameOverSequenceId : (-1));
			this._hasCachedSnapshot = false;
			this._hasCachedOutcome = false;
			this._hasAnimatedOnce = false;
			this.ConfigureHomeReturnPresentation(reason == GameOverReason.ReturnedHome);
			if (this.continueToPrisonButton != null)
			{
				bool flag = reason == GameOverReason.PoliceCapture || reason == GameOverReason.TaserStun;
				this.continueToPrisonButton.gameObject.SetActive(flag);
			}
			this.IsShowing = true;
			this._showAtRealtime = ((instance != null) ? instance.StartedAtRealtime : Time.realtimeSinceStartup) + ((reason == GameOverReason.ReturnedHome) ? 0.85f : this.showDelayAfterGameOver);
			if (base.isActiveAndEnabled)
			{
				this.ResumeShowSequence();
			}
		}

		// Token: 0x060079FD RID: 31229 RVA: 0x0031D25C File Offset: 0x0031B45C
		private void ResumeShowSequence()
		{
			base.StopAllCoroutines();
			this._showRoutine = base.StartCoroutine(this.PlayShowSequence(Mathf.Max(0f, this._showAtRealtime - Time.realtimeSinceStartup)));
		}

		// Token: 0x060079FE RID: 31230 RVA: 0x0031D28C File Offset: 0x0031B48C
		private IEnumerator PlayShowSequence(float preDelay)
		{
			this.ResetVisualStateImmediate();
			if (preDelay > 0f)
			{
				yield return new WaitForSecondsRealtime(preDelay);
			}
			if (!this._hasCachedSnapshot)
			{
				this._cachedSnapshot = ((MatchStatsService.Instance != null) ? MatchStatsService.Instance.GetSnapshot() : MatchRecapController.DefaultSnapshot());
				this._hasCachedSnapshot = true;
			}
			MatchStatsSnapshot snap = this._cachedSnapshot;
			if (this._showingHomeReturn)
			{
				yield return this.PlayHomeReturnSequence(snap);
				this._showRoutine = null;
				yield break;
			}
			if (!this._hasCachedOutcome)
			{
				RankRuntimeController rankRuntimeController = RankRuntimeController.Current;
				if (rankRuntimeController != null && rankRuntimeController.HasLastRunOutcome)
				{
					this._cachedOutcome = rankRuntimeController.LastRunOutcome;
					this._hasCachedOutcome = true;
				}
			}
			if (this._hasAnimatedOnce)
			{
				this.RevealStateImmediate(snap);
				this._showRoutine = null;
				yield break;
			}
			this.UnlockCursorForUI();
			if (this.rootGroup != null)
			{
				this.rootGroup.interactable = true;
				this.rootGroup.blocksRaycasts = true;
			}
			if (this.buttonsGroup != null)
			{
				this.buttonsGroup.interactable = true;
				this.buttonsGroup.blocksRaycasts = true;
			}
			base.StartCoroutine(this.FadeAlpha(this.rootGroup, 1f, this.panelFadeInDuration));
			base.StartCoroutine(this.FadeAlpha(this.buttonsGroup, 1f, this.buttonsFadeInDuration));
			if (this.panelFadeInDuration > 0f)
			{
				yield return new WaitForSecondsRealtime(this.panelFadeInDuration);
			}
			this.EnsureTickAudioSource();
			yield return this.RevealRow(0, this.distanceLabel, snap.DistanceMeters, true, false);
			yield return this.RevealRow(1, this._scoreLabel, (float)snap.Points, false, false);
			yield return this.RevealRow(2, this.distanceWithVehiclesLabel, snap.DistanceWithVehiclesMeters, true, false);
			yield return this.RevealRow(3, this.timesSeenLabel, (float)snap.TimesSeen, false, false);
			yield return this.RevealRow(4, this.timesExhibitionedLabel, (float)snap.TimesExhibitioned, false, false);
			yield return this.RevealRow(5, this.locationsVisitedLabel, (float)snap.LocationsVisited, false, false);
			yield return this.RevealRow(6, this.carSurfCountLabel, (float)snap.CarSurfCount, false, false);
			yield return this.RevealRow(7, this.averageHeartbeatLabel, (float)snap.AverageHeartbeatBpm, false, true);
			if (this._hasCachedOutcome && this._rankWidget != null)
			{
				yield return this._rankWidget.Reveal(this._cachedOutcome);
			}
			this._hasAnimatedOnce = true;
			this._showRoutine = null;
			yield break;
		}

		// Token: 0x060079FF RID: 31231 RVA: 0x0031D2A2 File Offset: 0x0031B4A2
		private IEnumerator RevealRankRows(RunOutcome outcome)
		{
			if (this.rankXpGainedLabel != null)
			{
				yield return this.FadeRankRow(0);
				yield return this.CountUpLabel(this.rankXpGainedLabel, outcome.xpGained, false, false);
				MatchRecapController.SetRankXpText(this.rankXpGainedLabel, outcome);
				this.PlayCompletionSound();
				if (this.rowGapAfterCount > 0f)
				{
					yield return new WaitForSecondsRealtime(this.rowGapAfterCount);
				}
			}
			if (this.rankLevelLabel != null)
			{
				yield return this.FadeRankRow(1);
				LocalizedGameText.BindFormat(this.rankLevelLabel, "rank_ui", (outcome.levelsGained > 0) ? "LEVEL UP!  Lv {0}" : "Level {0}", new object[] { outcome.newLevel }).FitTranslationToRectangle(false);
				this.PlayCompletionSound();
				if (this.rowGapAfterCount > 0f)
				{
					yield return new WaitForSecondsRealtime(this.rowGapAfterCount);
				}
			}
			if (this.rankUpLabel != null)
			{
				if (outcome.rankChanged)
				{
					yield return this.FadeRankRow(2);
					MatchRecapController.BindRankPromotion(this.rankUpLabel, outcome.newRankIndex);
					this.PlayCompletionSound();
				}
				else
				{
					this.rankUpLabel.text = string.Empty;
					CanvasGroup canvasGroup = this.RankRowGroup(2);
					if (canvasGroup != null)
					{
						canvasGroup.alpha = 0f;
					}
				}
			}
			yield break;
		}

		// Token: 0x06007A00 RID: 31232 RVA: 0x0031D2B8 File Offset: 0x0031B4B8
		private IEnumerator FadeRankRow(int index)
		{
			CanvasGroup canvasGroup = this.RankRowGroup(index);
			if (canvasGroup != null)
			{
				yield return this.FadeAlpha(canvasGroup, 1f, this.rowFadeInDuration);
			}
			yield break;
		}

		// Token: 0x06007A01 RID: 31233 RVA: 0x0031D2CE File Offset: 0x0031B4CE
		private CanvasGroup RankRowGroup(int index)
		{
			if (this.rankRowGroups == null || index < 0 || index >= this.rankRowGroups.Length)
			{
				return null;
			}
			return this.rankRowGroups[index];
		}

		// Token: 0x06007A02 RID: 31234 RVA: 0x0031D2F1 File Offset: 0x0031B4F1
		private static void SetRankXpText(TMP_Text label, RunOutcome outcome)
		{
			if (label == null)
			{
				return;
			}
			LocalizedGameText.BindFormat(label, "rank_ui", "+{0} XP", new object[] { Mathf.RoundToInt(outcome.xpGained) });
		}

		// Token: 0x06007A03 RID: 31235 RVA: 0x0031D328 File Offset: 0x0031B528
		private static string ResolveRankName(int rankIndex)
		{
			RankProgressionConfigSO config = RankProgressionService.Config;
			if (config != null && config.ranksInOrder != null && rankIndex >= 0 && rankIndex < config.ranksInOrder.Count)
			{
				RankDefinitionSO rankDefinitionSO = config.ranksInOrder[rankIndex];
				if (rankDefinitionSO != null && !string.IsNullOrEmpty(rankDefinitionSO.displayName))
				{
					return GameLocalization.GetSource("rank", rankDefinitionSO.displayName);
				}
			}
			return GameLocalization.FormatSource("rank_ui", "Rank {0}", new object[] { rankIndex });
		}

		// Token: 0x06007A04 RID: 31236 RVA: 0x0031D3B0 File Offset: 0x0031B5B0
		private static void BindRankPromotion(TMP_Text label, int rankIndex)
		{
			LocalizedGameText localizedGameText = LocalizedGameText.BindComputed(label, () => GameLocalization.FormatSource("rank_ui", "RANK UP  →  {0}", new object[] { MatchRecapController.ResolveRankName(rankIndex) }));
			if (localizedGameText == null)
			{
				return;
			}
			localizedGameText.FitTranslationToRectangle(false);
		}

		// Token: 0x06007A05 RID: 31237 RVA: 0x0031D3E8 File Offset: 0x0031B5E8
		private void RevealStateImmediate(MatchStatsSnapshot snap)
		{
			this.UnlockCursorForUI();
			if (this.rootGroup != null)
			{
				this.rootGroup.alpha = 1f;
				this.rootGroup.interactable = true;
				this.rootGroup.blocksRaycasts = true;
			}
			if (this.statRowGroups != null)
			{
				for (int i = 0; i < this.statRowGroups.Length; i++)
				{
					if (this.statRowGroups[i] != null)
					{
						this.statRowGroups[i].alpha = 1f;
					}
				}
			}
			if (this.buttonsGroup != null)
			{
				this.buttonsGroup.alpha = 1f;
				this.buttonsGroup.interactable = true;
				this.buttonsGroup.blocksRaycasts = true;
			}
			MatchRecapController.SetLabelMeters(this.distanceLabel, snap.DistanceMeters);
			MatchRecapController.SetLabelMeters(this.distanceWithVehiclesLabel, snap.DistanceWithVehiclesMeters);
			MatchRecapController.SetLabelInt(this.timesSeenLabel, snap.TimesSeen);
			MatchRecapController.SetLabelInt(this.timesExhibitionedLabel, snap.TimesExhibitioned);
			MatchRecapController.SetLabelInt(this.locationsVisitedLabel, snap.LocationsVisited);
			MatchRecapController.SetLabelInt(this.carSurfCountLabel, snap.CarSurfCount);
			MatchRecapController.SetLabelBpm(this.averageHeartbeatLabel, Mathf.RoundToInt((float)snap.AverageHeartbeatBpm));
			MatchRecapController.SetLabelInt(this._scoreLabel, snap.Points);
			if (this._hasCachedOutcome && this._rankWidget != null)
			{
				this._rankWidget.ShowImmediate(this._cachedOutcome);
			}
		}

		// Token: 0x06007A06 RID: 31238 RVA: 0x0031D55C File Offset: 0x0031B75C
		private void RevealRankRowsImmediate()
		{
			if (this.rankRowGroups != null)
			{
				for (int i = 0; i < this.rankRowGroups.Length; i++)
				{
					if (this.rankRowGroups[i] != null)
					{
						this.rankRowGroups[i].alpha = 1f;
					}
				}
			}
			if (!this._hasCachedOutcome)
			{
				if (this.rankXpGainedLabel != null)
				{
					this.rankXpGainedLabel.text = string.Empty;
				}
				if (this.rankLevelLabel != null)
				{
					this.rankLevelLabel.text = string.Empty;
				}
				if (this.rankUpLabel != null)
				{
					this.rankUpLabel.text = string.Empty;
				}
				return;
			}
			RunOutcome cachedOutcome = this._cachedOutcome;
			MatchRecapController.SetRankXpText(this.rankXpGainedLabel, cachedOutcome);
			if (this.rankLevelLabel != null)
			{
				LocalizedGameText.BindFormat(this.rankLevelLabel, "rank_ui", (cachedOutcome.levelsGained > 0) ? "LEVEL UP!  Lv {0}" : "Level {0}", new object[] { cachedOutcome.newLevel }).FitTranslationToRectangle(false);
			}
			if (this.rankUpLabel != null)
			{
				if (cachedOutcome.rankChanged)
				{
					MatchRecapController.BindRankPromotion(this.rankUpLabel, cachedOutcome.newRankIndex);
				}
				else
				{
					LocalizedGameText.BindSource(this.rankUpLabel, "rank_ui", string.Empty, null);
				}
				if (!cachedOutcome.rankChanged)
				{
					CanvasGroup canvasGroup = this.RankRowGroup(2);
					if (canvasGroup != null)
					{
						canvasGroup.alpha = 0f;
					}
				}
			}
		}

		// Token: 0x06007A07 RID: 31239 RVA: 0x0031D6CD File Offset: 0x0031B8CD
		private IEnumerator RevealRow(int index, TMP_Text label, float target, bool isFloat, bool isBpm)
		{
			CanvasGroup canvasGroup = ((this.statRowGroups != null && index < this.statRowGroups.Length) ? this.statRowGroups[index] : null);
			if (canvasGroup != null)
			{
				yield return this.FadeAlpha(canvasGroup, 1f, this.rowFadeInDuration);
			}
			yield return this.CountUpLabel(label, target, isFloat, isBpm);
			this.PlayCompletionSound();
			if (this.rowGapAfterCount > 0f)
			{
				yield return new WaitForSecondsRealtime(this.rowGapAfterCount);
			}
			yield break;
		}

		// Token: 0x06007A08 RID: 31240 RVA: 0x0031D701 File Offset: 0x0031B901
		private IEnumerator CountUpLabel(TMP_Text label, float target, bool isFloat, bool isBpm)
		{
			if (label == null)
			{
				yield break;
			}
			float elapsed = 0f;
			while (elapsed < this.countUpDuration)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = this.countUpEasing.Evaluate(Mathf.Clamp01(elapsed / this.countUpDuration));
				if (isFloat)
				{
					MatchRecapController.SetLabelMeters(label, target * num);
				}
				else
				{
					int num2 = Mathf.RoundToInt(target * num);
					if (isBpm)
					{
						MatchRecapController.SetLabelBpm(label, num2);
					}
					else
					{
						MatchRecapController.SetLabelInt(label, num2);
					}
				}
				yield return null;
			}
			if (isFloat)
			{
				MatchRecapController.SetLabelMeters(label, target);
			}
			else if (isBpm)
			{
				MatchRecapController.SetLabelBpm(label, Mathf.RoundToInt(target));
			}
			else
			{
				MatchRecapController.SetLabelInt(label, Mathf.RoundToInt(target));
			}
			yield break;
		}

		// Token: 0x06007A09 RID: 31241 RVA: 0x0031D72D File Offset: 0x0031B92D
		private IEnumerator PlayHideSequence()
		{
			if (this.rootGroup != null)
			{
				this.rootGroup.interactable = false;
				this.rootGroup.blocksRaycasts = false;
			}
			this.RestoreCursor();
			yield return this.FadeAlpha(this.rootGroup, 0f, 0.3f);
			if (this.rootGroup != null)
			{
				this.rootGroup.alpha = 0f;
			}
			this._showRoutine = null;
			yield break;
		}

		// Token: 0x06007A0A RID: 31242 RVA: 0x0031D73C File Offset: 0x0031B93C
		private void ResetVisualStateImmediate()
		{
			if (this.rootGroup != null)
			{
				this.rootGroup.alpha = 0f;
				this.rootGroup.interactable = false;
				this.rootGroup.blocksRaycasts = false;
			}
			if (this.statRowGroups != null)
			{
				for (int i = 0; i < this.statRowGroups.Length; i++)
				{
					if (this.statRowGroups[i] != null)
					{
						this.statRowGroups[i].alpha = 0f;
					}
				}
			}
			if (this.buttonsGroup != null)
			{
				this.buttonsGroup.alpha = 0f;
				this.buttonsGroup.interactable = false;
				this.buttonsGroup.blocksRaycasts = false;
			}
			MatchRecapController.SetLabelMeters(this.distanceLabel, 0f);
			MatchRecapController.SetLabelMeters(this.distanceWithVehiclesLabel, 0f);
			MatchRecapController.SetLabelInt(this.timesSeenLabel, 0);
			MatchRecapController.SetLabelInt(this.timesExhibitionedLabel, 0);
			MatchRecapController.SetLabelInt(this.locationsVisitedLabel, 0);
			MatchRecapController.SetLabelInt(this.carSurfCountLabel, 0);
			MatchRecapController.SetLabelBpm(this.averageHeartbeatLabel, 0);
			MatchRecapController.SetLabelInt(this._scoreLabel, 0);
			if (this.rankRowGroups != null)
			{
				for (int j = 0; j < this.rankRowGroups.Length; j++)
				{
					if (this.rankRowGroups[j] != null)
					{
						this.rankRowGroups[j].alpha = 0f;
					}
				}
			}
			if (this.rankXpGainedLabel != null)
			{
				this.rankXpGainedLabel.text = string.Empty;
			}
			if (this.rankLevelLabel != null)
			{
				this.rankLevelLabel.text = string.Empty;
			}
			if (this.rankUpLabel != null)
			{
				this.rankUpLabel.text = string.Empty;
			}
			if (this._rankWidget != null)
			{
				this._rankWidget.ResetVisuals();
			}
		}

		// Token: 0x06007A0B RID: 31243 RVA: 0x0031D90C File Offset: 0x0031BB0C
		private void DisableLegacyRankRows()
		{
			if (this.rankRowGroups == null)
			{
				return;
			}
			for (int i = 0; i < this.rankRowGroups.Length; i++)
			{
				CanvasGroup canvasGroup = this.rankRowGroups[i];
				if (canvasGroup != null)
				{
					canvasGroup.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06007A0C RID: 31244 RVA: 0x0031D954 File Offset: 0x0031BB54
		private void EnsureScoreKpiRow()
		{
			if (this._scoreLabel != null || this.averageHeartbeatLabel == null)
			{
				return;
			}
			Transform parent = this.averageHeartbeatLabel.transform.parent;
			if (parent == null || parent.parent == null)
			{
				return;
			}
			Transform transform = parent.parent.Find("StatRow_Score");
			GameObject gameObject = ((transform != null) ? transform.gameObject : Object.Instantiate<GameObject>(parent.gameObject, parent.parent));
			gameObject.name = "StatRow_Score";
			gameObject.SetActive(true);
			Transform transform2 = ((this.distanceLabel != null) ? this.distanceLabel.transform.parent : null);
			gameObject.transform.SetSiblingIndex((transform2 != null) ? (transform2.GetSiblingIndex() + 1) : (parent.GetSiblingIndex() + 1));
			Transform transform3 = gameObject.transform.Find("Label");
			TMP_Text tmp_Text = ((transform3 != null) ? transform3.GetComponent<TMP_Text>() : null);
			Transform transform4 = gameObject.transform.Find("Value");
			this._scoreLabel = ((transform4 != null) ? transform4.GetComponent<TMP_Text>() : null);
			LocalizedGameText.BindSource(tmp_Text, "rank_ui", "Score", null);
			if (this._scoreLabel != null)
			{
				this._scoreLabel.text = "0";
			}
			CanvasGroup component = gameObject.GetComponent<CanvasGroup>();
			if (component == null || this.ContainsStatRow(component))
			{
				return;
			}
			int num = ((this.statRowGroups != null) ? this.statRowGroups.Length : 0);
			CanvasGroup[] array = new CanvasGroup[num + 1];
			int num2 = Mathf.Min(1, num);
			for (int i = 0; i < num2; i++)
			{
				array[i] = this.statRowGroups[i];
			}
			array[num2] = component;
			for (int j = num2; j < num; j++)
			{
				array[j + 1] = this.statRowGroups[j];
			}
			this.statRowGroups = array;
		}

		// Token: 0x06007A0D RID: 31245 RVA: 0x0031DB3C File Offset: 0x0031BD3C
		private bool ContainsStatRow(CanvasGroup candidate)
		{
			if (this.statRowGroups == null)
			{
				return false;
			}
			for (int i = 0; i < this.statRowGroups.Length; i++)
			{
				if (this.statRowGroups[i] == candidate)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007A0E RID: 31246 RVA: 0x0031DB7C File Offset: 0x0031BD7C
		private void EnsureRankProgressWidget()
		{
			if (this._rankWidget != null)
			{
				return;
			}
			Transform transform = ((this.rootObject != null) ? this.rootObject.transform : base.transform);
			TMP_Text tmp_Text = ((this.rankXpGainedLabel != null) ? this.rankXpGainedLabel : this.distanceLabel);
			Image image;
			if (!(this.rootObject != null))
			{
				image = null;
			}
			else
			{
				Transform transform2 = this.rootObject.transform.Find("PanelCenter/Banner");
				image = ((transform2 != null) ? transform2.GetComponent<Image>() : null);
			}
			Image image2 = image;
			this._rankWidget = MatchRecapRankWidget.Create(transform, tmp_Text, image2, this.tickClip, this.tickVolume);
		}

		// Token: 0x06007A0F RID: 31247 RVA: 0x0031DC24 File Offset: 0x0031BE24
		private IEnumerator FadeAlpha(CanvasGroup cg, float target, float duration)
		{
			if (cg == null)
			{
				yield break;
			}
			if (duration <= 0f)
			{
				cg.alpha = target;
				yield break;
			}
			float start = cg.alpha;
			float t = 0f;
			while (t < duration)
			{
				t += Time.unscaledDeltaTime;
				cg.alpha = Mathf.Lerp(start, target, Mathf.Clamp01(t / duration));
				yield return null;
			}
			cg.alpha = target;
			yield break;
		}

		// Token: 0x06007A10 RID: 31248 RVA: 0x0031DC44 File Offset: 0x0031BE44
		private void EnsureGameObjectActive()
		{
			if (this.rootObject != null && !this.rootObject.activeSelf)
			{
				this.rootObject.SetActive(true);
			}
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
			}
		}

		// Token: 0x06007A11 RID: 31249 RVA: 0x0031DC91 File Offset: 0x0031BE91
		private void UnlockCursorForUI()
		{
			if (this._cursorStateSaved)
			{
				return;
			}
			this._savedCursorLock = Cursor.lockState;
			this._savedCursorVisible = Cursor.visible;
			this._cursorStateSaved = true;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		// Token: 0x06007A12 RID: 31250 RVA: 0x0031DCC5 File Offset: 0x0031BEC5
		private void RestoreCursor()
		{
			if (!this._cursorStateSaved)
			{
				return;
			}
			Cursor.lockState = this._savedCursorLock;
			Cursor.visible = this._savedCursorVisible;
			this._cursorStateSaved = false;
		}

		// Token: 0x06007A13 RID: 31251 RVA: 0x0031DCF0 File Offset: 0x0031BEF0
		private void WireButton(MonoBehaviour btn, UnityAction handler, bool addListener)
		{
			if (btn == null)
			{
				return;
			}
			Button button = btn as Button;
			if (button != null)
			{
				if (addListener)
				{
					button.onClick.AddListener(handler);
					return;
				}
				button.onClick.RemoveListener(handler);
				return;
			}
			else
			{
				DAButton dabutton = btn as DAButton;
				if (dabutton == null)
				{
					Debug.LogWarning(string.Concat(new string[]
					{
						"[MatchRecapController] Bottone '",
						btn.name,
						"' di tipo ",
						btn.GetType().Name,
						" non supportato."
					}));
					return;
				}
				if (addListener)
				{
					dabutton.OnPointerClickEvent.AddListener(handler);
					return;
				}
				dabutton.OnPointerClickEvent.RemoveListener(handler);
				return;
			}
		}

		// Token: 0x06007A14 RID: 31252 RVA: 0x0031DD94 File Offset: 0x0031BF94
		private void EnsureTickAudioSource()
		{
			if (this.tickClip == null)
			{
				return;
			}
			if (this.tickAudioSource != null)
			{
				return;
			}
			this.tickAudioSource = base.gameObject.AddComponent<AudioSource>();
			this.tickAudioSource.playOnAwake = false;
			this.tickAudioSource.spatialBlend = 0f;
			this.tickAudioSource.loop = false;
		}

		// Token: 0x06007A15 RID: 31253 RVA: 0x0031DDF8 File Offset: 0x0031BFF8
		private void PlayCompletionSound()
		{
			if (this.tickClip == null || this.tickAudioSource == null)
			{
				return;
			}
			float num = this.tickBasePitch + ((this.tickPitchJitter > 0f) ? Random.Range(-this.tickPitchJitter, this.tickPitchJitter) : 0f);
			this.tickAudioSource.pitch = num;
			this.tickAudioSource.PlayOneShot(this.tickClip, this.tickVolume);
		}

		// Token: 0x06007A16 RID: 31254 RVA: 0x0031DE73 File Offset: 0x0031C073
		private static void SetLabelMeters(TMP_Text label, float meters)
		{
			if (label != null)
			{
				label.text = string.Format("{0}m", Mathf.RoundToInt(meters));
			}
		}

		// Token: 0x06007A17 RID: 31255 RVA: 0x0031DE99 File Offset: 0x0031C099
		private static void SetLabelInt(TMP_Text label, int value)
		{
			if (label != null)
			{
				label.text = value.ToString();
			}
		}

		// Token: 0x06007A18 RID: 31256 RVA: 0x0031DEB1 File Offset: 0x0031C0B1
		private static void SetLabelBpm(TMP_Text label, int bpm)
		{
			LocalizedGameText.BindFormat(label, "rank_ui", "{0}bpm", new object[] { bpm });
		}

		// Token: 0x06007A19 RID: 31257 RVA: 0x0031DED4 File Offset: 0x0031C0D4
		private static MatchStatsSnapshot DefaultSnapshot()
		{
			return new MatchStatsSnapshot
			{
				DistanceMeters = 0f,
				DistanceWithVehiclesMeters = 0f,
				TimesSeen = 0,
				TimesExhibitioned = 0,
				LocationsVisited = 0,
				CarSurfCount = 0,
				AverageHeartbeatBpm = 0
			};
		}

		// Token: 0x06007A1A RID: 31258 RVA: 0x0031DF2C File Offset: 0x0031C12C
		private void OnTryAgainClicked()
		{
			NpcPhotoSessionArchive.Clear();
			if (GameOverManager.Instance != null)
			{
				GameOverManager.Instance.ReturnToHome();
				return;
			}
			SceneTransitionManager instance = SceneTransitionManager.Instance;
			if (instance != null)
			{
				instance.LoadGameplayScene(this.homeSceneName);
				return;
			}
			Scene activeScene = SceneManager.GetActiveScene();
			if (LoadingScreenController.Instance != null)
			{
				LoadingScreenController.Instance.LoadSceneAsync(activeScene.buildIndex);
				return;
			}
			SceneManager.LoadScene(activeScene.buildIndex);
		}

		// Token: 0x06007A1B RID: 31259 RVA: 0x0031DFA4 File Offset: 0x0031C1A4
		private void OnContinueToPrisonClicked()
		{
			if (string.IsNullOrEmpty(this.prisonSceneName))
			{
				return;
			}
			AchievementTrackingService.ArmPrisonArrival(this.prisonSceneName);
			NpcPhotoSessionArchive.Clear();
			MatchRecapController.ResetPendingPlayerAction();
			if (LoadingScreenController.Instance != null)
			{
				LoadingScreenController.Instance.LoadSceneAsync(this.prisonSceneName);
				return;
			}
			SceneManager.LoadScene(this.prisonSceneName);
		}

		// Token: 0x06007A1C RID: 31260 RVA: 0x0031E000 File Offset: 0x0031C200
		private static void ResetPendingPlayerAction()
		{
			vGenericAction vGenericAction = Object.FindObjectOfType<vGenericAction>();
			if (vGenericAction != null && vGenericAction.doingAction)
			{
				vGenericAction.ResetTriggerSettings(false);
			}
		}

		// Token: 0x06007A1D RID: 31261 RVA: 0x0031E02B File Offset: 0x0031C22B
		private void RefreshHomeMissionLocale()
		{
			if (this._showingHomeReturn && this._homeMissionPanel != null && this._homeMissionSnapshot != null)
			{
				this.ShowHomeMissionConfirmation(this._homeMissionSnapshot.Value, false);
			}
		}

		// Token: 0x06007A1E RID: 31262 RVA: 0x0031E064 File Offset: 0x0031C264
		private void ConfigureHomeReturnPresentation(bool success)
		{
			this._showingHomeReturn = success;
			Transform transform = ((this.rootObject != null) ? this.rootObject.transform : base.transform);
			if (this._ordinaryRecap == null)
			{
				this._ordinaryRecap = transform.Find("PanelCenter");
			}
			if (this._ordinaryRecap != null)
			{
				this._ordinaryRecap.gameObject.SetActive(!success);
			}
			if (success && this._homeReturnPage == null)
			{
				this.BuildHomeReturnPage(transform);
			}
			if (this._homeReturnPage != null)
			{
				this._homeReturnPage.SetActive(success);
			}
		}

		// Token: 0x06007A1F RID: 31263 RVA: 0x0031E110 File Offset: 0x0031C310
		private void BuildHomeReturnPage(Transform parent)
		{
			RectTransform rectTransform = MatchRecapController.HomeRect("HomeReturnResults", parent, Vector2.zero, Vector2.one);
			this._homeReturnPage = rectTransform.gameObject;
			rectTransform.gameObject.AddComponent<Image>().color = new Color32(23, 15, 31, byte.MaxValue);
			RawImage rawImage = MatchRecapController.HomeRect("Ciel Signature Pattern", rectTransform, Vector2.zero, Vector2.one).gameObject.AddComponent<RawImage>();
			rawImage.color = new Color(1f, 1f, 1f, 0.12f);
			rawImage.raycastTarget = false;
			PoiPortalPresentation.ConfigurePattern(rawImage);
			this.HomeText(rectTransform, "Mode", "CLASSIC", 18f, MatchRecapController.HomeAccent, new Vector2(0.14f, 0.85f), new Vector2(0.55f, 0.9f), TextAlignmentOptions.Left);
			this.HomeText(rectTransform, "Heading", "BACK HOME", 68f, MatchRecapController.HomeIvory, new Vector2(0.14f, 0.72f), new Vector2(0.6f, 0.84f), TextAlignmentOptions.Left);
			TMP_Text tmp_Text = this.HomeText(rectTransform, "Subtitle", "You turned back. You're not ready yet, or not brave enough.", 25f, MatchRecapController.HomeMuted, new Vector2(0.14f, 0.59f), new Vector2(0.49f, 0.72f), TextAlignmentOptions.TopLeft);
			tmp_Text.enableWordWrapping = true;
			tmp_Text.fontStyle = FontStyles.Normal;
			this._homeSubtitle = tmp_Text;
			this.BuildHomeMissionConfirmation(rectTransform);
			this.HomeText(rectTransform, "ScoreHeading", "SCORE", 19f, MatchRecapController.HomeAccent, new Vector2(0.59f, 0.79f), new Vector2(0.86f, 0.84f), TextAlignmentOptions.Right);
			this._homeTotal = this.HomeText(rectTransform, "Total", "0", 96f, MatchRecapController.HomeIvory, new Vector2(0.56f, 0.64f), new Vector2(0.86f, 0.79f), TextAlignmentOptions.Right);
			this.HomeText(rectTransform, "BaseHeading", "Run", 20f, MatchRecapController.HomeMuted, new Vector2(0.59f, 0.575f), new Vector2(0.73f, 0.625f), TextAlignmentOptions.Left);
			this._homeBase = this.HomeText(rectTransform, "Base", "0", 24f, MatchRecapController.HomeIvory, new Vector2(0.73f, 0.575f), new Vector2(0.86f, 0.625f), TextAlignmentOptions.Right);
			this.HomeText(rectTransform, "BonusHeading", "Return bonus", 20f, MatchRecapController.HomeMuted, new Vector2(0.59f, 0.52f), new Vector2(0.76f, 0.57f), TextAlignmentOptions.Left);
			this._homeBonus = this.HomeText(rectTransform, "Bonus", "+0", 24f, MatchRecapController.HomeAccent, new Vector2(0.76f, 0.52f), new Vector2(0.86f, 0.57f), TextAlignmentOptions.Right);
			MatchRecapController.HomeBand(rectTransform, "Divider", new Vector2(0.14f, 0.475f), new Vector2(0.86f, 0.477f), new Color32(104, 61, 129, byte.MaxValue));
			this.HomeText(rectTransform, "DistanceHeading", "FURTHEST FROM HOME", 16f, MatchRecapController.HomeMuted, new Vector2(0.14f, 0.38f), new Vector2(0.4f, 0.425f), TextAlignmentOptions.Left);
			this._homeDistance = this.HomeText(rectTransform, "Distance", "", 34f, MatchRecapController.HomeIvory, new Vector2(0.14f, 0.31f), new Vector2(0.4f, 0.38f), TextAlignmentOptions.Left);
			this._homeJourney = this.HomeText(rectTransform, "Journey", "", 21f, MatchRecapController.HomeMuted, new Vector2(0.44f, 0.31f), new Vector2(0.7f, 0.425f), TextAlignmentOptions.Left);
			this._homeRewards = this.HomeText(rectTransform, "Rewards", "", 20f, MatchRecapController.HomeAccent, new Vector2(0.14f, 0.225f), new Vector2(0.86f, 0.295f), TextAlignmentOptions.Left);
			this._homeJourney.enableWordWrapping = (this._homeRewards.enableWordWrapping = true);
			this._homeJourney.fontStyle = (this._homeRewards.fontStyle = FontStyles.Normal);
			RectTransform rectTransform2 = MatchRecapController.HomeRect("ContinueHome", rectTransform, new Vector2(0.64f, 0.105f), new Vector2(0.86f, 0.177f));
			Image image = rectTransform2.gameObject.AddComponent<Image>();
			image.color = new Color32(169, 96, 201, byte.MaxValue);
			this._homeContinue = rectTransform2.gameObject.AddComponent<Button>();
			this._homeContinue.targetGraphic = image;
			ColorBlock colors = this._homeContinue.colors;
			colors.highlightedColor = new Color(1f, 0.86f, 1f);
			colors.selectedColor = colors.highlightedColor;
			colors.pressedColor = new Color(0.7f, 0.52f, 0.82f);
			this._homeContinue.colors = colors;
			this._homeContinue.onClick.AddListener(delegate
			{
				if (!this._homeContinue.interactable)
				{
					return;
				}
				this._homeContinue.interactable = false;
				this.OnTryAgainClicked();
			});
			this.HomeText(rectTransform2, "Label", "CONTINUE", 22f, MatchRecapController.HomeIvory, new Vector2(0.06f, 0.1f), new Vector2(0.94f, 0.9f), TextAlignmentOptions.Center);
		}

		// Token: 0x06007A20 RID: 31264 RVA: 0x0031E6C1 File Offset: 0x0031C8C1
		private IEnumerator PlayHomeReturnSequence(MatchStatsSnapshot snapshot)
		{
			this._homeContinue.interactable = true;
			int basePoints = Mathf.Max(0, snapshot.Points - snapshot.HomeReturnBonus);
			LocalizedGameText.BindFormat(this._homeDistance, "rank_ui", "{0:N0} m", new object[] { Math.Floor((double)snapshot.PeakDistanceMeters) });
			LocalizedGameText.BindFormat(this._homeJourney, "rank_ui", "{0} places visited\n{1} times seen", new object[] { snapshot.LocationsVisited, snapshot.TimesSeen });
			RankRuntimeController rankRuntimeController = RankRuntimeController.Current;
			bool hasOutcome = rankRuntimeController != null && rankRuntimeController.HasLastRunOutcome;
			RunOutcome outcome = (hasOutcome ? rankRuntimeController.LastRunOutcome : default(RunOutcome));
			LocalizedGameText.BindComputed(this._homeRewards, delegate
			{
				string text = (hasOutcome ? (GameLocalization.FormatSource("rank_ui", "+{0:N0} XP   Level {1}", new object[] { outcome.xpGained, outcome.newLevel }) + (outcome.rankChanged ? GameLocalization.GetSource("rank_ui", "   Rank up!") : string.Empty)) : string.Empty);
				if (snapshot.HomeReturnMissionsCompleted <= 0)
				{
					return text;
				}
				return text + ((text.Length > 0) ? "\n" : string.Empty) + GameLocalization.FormatSource("rank_ui", "{0} return missions completed   +{1:N0} XP", new object[] { snapshot.HomeReturnMissionsCompleted, snapshot.HomeReturnMissionXp });
			});
			this.ShowHomeMissionConfirmation(snapshot, true);
			this.UnlockCursorForUI();
			if (this.rootGroup != null)
			{
				this.rootGroup.alpha = 1f;
				this.rootGroup.interactable = true;
				this.rootGroup.blocksRaycasts = true;
			}
			if (EventSystem.current != null)
			{
				EventSystem.current.SetSelectedGameObject(this._homeContinue.gameObject);
			}
			ScreenFadeController instance = ScreenFadeController.Instance;
			if (instance != null)
			{
				instance.FadeIn(0.45f, null);
			}
			if (this._hasAnimatedOnce)
			{
				this._homeBase.text = basePoints.ToString("N0");
				this._homeBonus.text = "+" + snapshot.HomeReturnBonus.ToString("N0");
				this._homeTotal.text = snapshot.Points.ToString("N0");
				yield break;
			}
			this._homeBase.text = "0";
			this._homeBonus.text = "+0";
			this._homeTotal.text = "0";
			yield return new WaitForSecondsRealtime(0.35f);
			yield return this.CountHomeScore(basePoints, 0, 0.65f, false);
			yield return new WaitForSecondsRealtime(0.15f);
			yield return this.CountHomeScore(snapshot.HomeReturnBonus, basePoints, 0.8f, true);
			this._homeTotal.text = snapshot.Points.ToString("N0");
			this._hasAnimatedOnce = true;
			yield break;
		}

		// Token: 0x06007A21 RID: 31265 RVA: 0x0031E6D8 File Offset: 0x0031C8D8
		private void BuildHomeMissionConfirmation(RectTransform page)
		{
			RectTransform rectTransform = MatchRecapController.HomeRect("Completed Return Missions", page, new Vector2(0.14f, 0.5f), new Vector2(0.54f, 0.715f));
			this._homeMissionPanel = rectTransform.gameObject;
			rectTransform.gameObject.AddComponent<Image>().color = new Color32(41, 26, 53, byte.MaxValue);
			MissionLocalization.Bind(this.HomeText(rectTransform, "Confirmation", "MISSIONS COMPLETED", 21f, MatchRecapController.HomeAccent, new Vector2(0.035f, 0.8f), new Vector2(0.965f, 0.97f), TextAlignmentOptions.Left), "MISSIONS COMPLETED", true);
			RectTransform rectTransform2 = MatchRecapController.HomeRect("Viewport", rectTransform, new Vector2(0.035f, 0.04f), new Vector2(0.965f, 0.78f));
			rectTransform2.gameObject.AddComponent<RectMask2D>();
			this._homeMissionDetails = this.HomeText(rectTransform2, "Mission Rewards", "", 19f, MatchRecapController.HomeIvory, new Vector2(0f, 1f), Vector2.one, TextAlignmentOptions.TopLeft);
			this._homeMissionDetails.enableAutoSizing = false;
			this._homeMissionDetails.enableWordWrapping = true;
			this._homeMissionDetails.fontStyle = FontStyles.Normal;
			this._homeMissionDetails.richText = false;
			this._homeMissionDetails.rectTransform.pivot = new Vector2(0.5f, 1f);
			this._homeMissionScroll = rectTransform.gameObject.AddComponent<ScrollRect>();
			this._homeMissionScroll.viewport = rectTransform2;
			this._homeMissionScroll.content = this._homeMissionDetails.rectTransform;
			this._homeMissionScroll.horizontal = false;
			this._homeMissionScroll.movementType = ScrollRect.MovementType.Clamped;
			this._homeMissionScroll.scrollSensitivity = 35f;
			RectTransform rectTransform3 = MatchRecapController.HomeRect("Reward Scrollbar", rectTransform, new Vector2(0.975f, 0.04f), new Vector2(0.983f, 0.78f));
			rectTransform3.gameObject.AddComponent<Image>().color = new Color32(65, 43, 78, byte.MaxValue);
			RectTransform rectTransform4 = MatchRecapController.HomeRect("Handle", rectTransform3, Vector2.zero, Vector2.one);
			Image image = rectTransform4.gameObject.AddComponent<Image>();
			image.color = MatchRecapController.HomeAccent;
			Scrollbar scrollbar = rectTransform3.gameObject.AddComponent<Scrollbar>();
			scrollbar.direction = Scrollbar.Direction.BottomToTop;
			scrollbar.targetGraphic = image;
			scrollbar.handleRect = rectTransform4;
			this._homeMissionScroll.verticalScrollbar = scrollbar;
			this._homeMissionScroll.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
		}

		// Token: 0x06007A22 RID: 31266 RVA: 0x0031E964 File Offset: 0x0031CB64
		private void ShowHomeMissionConfirmation(MatchStatsSnapshot snapshot, bool resetScroll = true)
		{
			this._homeMissionSnapshot = new MatchStatsSnapshot?(snapshot);
			bool flag = snapshot.HomeReturnMissionsCompleted > 0;
			this._homeMissionPanel.SetActive(flag);
			this._homeSubtitle.gameObject.SetActive(!flag);
			if (!flag)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (snapshot.HomeReturnMissionRewards != null)
			{
				foreach (MissionCompletion missionCompletion in snapshot.HomeReturnMissionRewards)
				{
					if (missionCompletion.Mission != null)
					{
						string text = ((missionCompletion.Mission.scope == MissionScope.Run) ? "RUN" : "ACCOUNT");
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append("\n\n");
						}
						stringBuilder.Append(MissionLocalization.Text(text)).Append("  +").Append(missionCompletion.XpOutcome.xpGained.ToString("N0"))
							.Append(" XP\n")
							.Append(MissionLocalization.Full(missionCompletion.Mission));
					}
				}
			}
			if (stringBuilder.Length == 0)
			{
				stringBuilder.Append(MissionLocalization.Format("{0} missions completed  +{1:N0} XP", new object[] { snapshot.HomeReturnMissionsCompleted, snapshot.HomeReturnMissionXp }));
			}
			MissionLocalization.Set(this._homeMissionDetails, stringBuilder.ToString());
			float num = Mathf.Max(1f, this._homeMissionScroll.viewport.rect.width);
			float y = this._homeMissionDetails.GetPreferredValues(stringBuilder.ToString(), num, float.PositiveInfinity).y;
			this._homeMissionDetails.rectTransform.sizeDelta = new Vector2(0f, y);
			if (resetScroll)
			{
				this._homeMissionScroll.StopMovement();
				this._homeMissionScroll.verticalNormalizedPosition = 1f;
			}
		}

		// Token: 0x06007A23 RID: 31267 RVA: 0x0031EB3A File Offset: 0x0031CD3A
		private IEnumerator CountHomeScore(int amount, int basePoints, float duration, bool bonus)
		{
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / duration);
				int num2 = (int)Math.Round((double)amount * (1.0 - Math.Pow(1.0 - (double)num, 3.0)));
				if (bonus)
				{
					this._homeBonus.text = "+" + num2.ToString("N0");
				}
				else
				{
					this._homeBase.text = num2.ToString("N0");
				}
				this._homeTotal.text = ((long)basePoints + (long)num2).ToString("N0");
				yield return null;
			}
			this.EnsureTickAudioSource();
			if (this.tickAudioSource != null && this.tickClip != null)
			{
				this.tickAudioSource.PlayOneShot(this.tickClip, this.tickVolume);
			}
			yield break;
		}

		// Token: 0x06007A24 RID: 31268 RVA: 0x0031EB68 File Offset: 0x0031CD68
		private static RectTransform HomeRect(string name, Transform parent, Vector2 min, Vector2 max)
		{
			RectTransform component = new GameObject(name, new Type[] { typeof(RectTransform) }).GetComponent<RectTransform>();
			component.SetParent(parent, false);
			component.anchorMin = min;
			component.anchorMax = max;
			component.offsetMin = (component.offsetMax = Vector2.zero);
			return component;
		}

		// Token: 0x06007A25 RID: 31269 RVA: 0x0031EBBD File Offset: 0x0031CDBD
		private static void HomeBand(Transform parent, string name, Vector2 min, Vector2 max, Color color)
		{
			Image image = MatchRecapController.HomeRect(name, parent, min, max).gameObject.AddComponent<Image>();
			image.color = color;
			image.raycastTarget = false;
		}

		// Token: 0x06007A26 RID: 31270 RVA: 0x0031EBE0 File Offset: 0x0031CDE0
		private TMP_Text HomeText(Transform parent, string name, string value, float size, Color color, Vector2 min, Vector2 max, TextAlignmentOptions alignment = TextAlignmentOptions.Center)
		{
			TextMeshProUGUI textMeshProUGUI = MatchRecapController.HomeRect(name, parent, min, max).gameObject.AddComponent<TextMeshProUGUI>();
			PoiArrivalCatalogSO poiArrivalCatalogSO = Resources.Load<PoiArrivalCatalogSO>("PoiArrivalCatalog");
			if (poiArrivalCatalogSO != null && poiArrivalCatalogSO.PortalFont != null)
			{
				textMeshProUGUI.font = poiArrivalCatalogSO.PortalFont;
			}
			else if (this.distanceLabel != null)
			{
				textMeshProUGUI.font = LocalizedGameText.SourceFont(this.distanceLabel);
			}
			textMeshProUGUI.text = value;
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.enableAutoSizing = true;
			textMeshProUGUI.fontSizeMin = size * 0.55f;
			textMeshProUGUI.fontSizeMax = size;
			textMeshProUGUI.fontStyle = FontStyles.Bold;
			textMeshProUGUI.color = color;
			textMeshProUGUI.alignment = alignment;
			textMeshProUGUI.enableWordWrapping = false;
			textMeshProUGUI.raycastTarget = false;
			if (!string.IsNullOrEmpty(value) && Regex.IsMatch(value, "[A-Za-z]"))
			{
				LocalizedGameText.BindSource(textMeshProUGUI, "rank_ui", value, null);
			}
			else
			{
				LocalizedGameText.BindFontOnly(textMeshProUGUI);
			}
			return textMeshProUGUI;
		}

		// Token: 0x04007D3A RID: 32058
		[Header("Root")]
		[Tooltip("CanvasGroup root del pannello. Gestito via alpha + interactable + blocksRaycasts.")]
		[SerializeField]
		private CanvasGroup rootGroup;

		// Token: 0x04007D3B RID: 32059
		[Tooltip("GameObject root da riattivare in Show se inactive. Tipicamente il GO stesso del controller.")]
		[SerializeField]
		private GameObject rootObject;

		// Token: 0x04007D3C RID: 32060
		[Header("Stats TMP Labels")]
		[SerializeField]
		private TMP_Text distanceLabel;

		// Token: 0x04007D3D RID: 32061
		[SerializeField]
		private TMP_Text distanceWithVehiclesLabel;

		// Token: 0x04007D3E RID: 32062
		[SerializeField]
		private TMP_Text timesSeenLabel;

		// Token: 0x04007D3F RID: 32063
		[SerializeField]
		private TMP_Text timesExhibitionedLabel;

		// Token: 0x04007D40 RID: 32064
		[SerializeField]
		private TMP_Text locationsVisitedLabel;

		// Token: 0x04007D41 RID: 32065
		[SerializeField]
		private TMP_Text carSurfCountLabel;

		// Token: 0x04007D42 RID: 32066
		[SerializeField]
		private TMP_Text averageHeartbeatLabel;

		// Token: 0x04007D43 RID: 32067
		[Header("Stats Reveal — Sequential Row by Row")]
		[Tooltip("CanvasGroup di ogni StatRow nello stesso ordine dei *Label (Distance, DistanceWithVehicles, TimesSeen, TimesExhibitioned, LocationsVisited, CarSurf, AverageHeartbeat). Le row partono ad alpha=0 e si svelano una alla volta DOPO che la precedente ha contato.")]
		[SerializeField]
		private CanvasGroup[] statRowGroups;

		// Token: 0x04007D44 RID: 32068
		[Tooltip("CanvasGroup dei bottoni TryAgain/ContinueToPrison. Fade-in all'inizio della Show; interactable=true SUBITO (l'utente puo' cliccare anche durante l'animazione stats).")]
		[SerializeField]
		private CanvasGroup buttonsGroup;

		// Token: 0x04007D45 RID: 32069
		[Header("Rank Progression (Fase 3 step 1) — tutti opzionali")]
		[Tooltip("Label 'XP guadagnati' della run. Se null, la riga XP e' saltata (nessun errore).")]
		[SerializeField]
		private TMP_Text rankXpGainedLabel;

		// Token: 0x04007D46 RID: 32070
		[Tooltip("Label livello (con 'LEVEL UP!' se il livello e' salito).")]
		[SerializeField]
		private TMP_Text rankLevelLabel;

		// Token: 0x04007D47 RID: 32071
		[Tooltip("Label 'RANK UP -> <nome>' mostrata SOLO se il rank e' cambiato in questa run.")]
		[SerializeField]
		private TMP_Text rankUpLabel;

		// Token: 0x04007D48 RID: 32072
		[Tooltip("CanvasGroup delle righe rank (XP, Level, RankUp) nello stesso ordine. Fade-in dopo le stat. Opzionali.")]
		[SerializeField]
		private CanvasGroup[] rankRowGroups;

		// Token: 0x04007D49 RID: 32073
		[Tooltip("Durata fade-in del pannello root (s).")]
		[Min(0f)]
		[SerializeField]
		private float panelFadeInDuration = 0.25f;

		// Token: 0x04007D4A RID: 32074
		[Tooltip("Durata fade-in di ogni StatRow prima del suo count (s).")]
		[Min(0.01f)]
		[SerializeField]
		private float rowFadeInDuration = 0.25f;

		// Token: 0x04007D4B RID: 32075
		[Tooltip("Pausa fra la fine del count di una row e il fade-in della successiva (s).")]
		[Min(0f)]
		[SerializeField]
		private float rowGapAfterCount = 0.1f;

		// Token: 0x04007D4C RID: 32076
		[Tooltip("Durata fade-in del blocco bottoni (parte all'INIZIO della Show, in parallelo al reveal stats — i bottoni sono cliccabili subito).")]
		[Min(0f)]
		[SerializeField]
		private float buttonsFadeInDuration = 0.35f;

		// Token: 0x04007D4D RID: 32077
		[Header("Count-up Tween")]
		[Tooltip("Durata del count-up tween per ogni numero (s). I numeri vanno da 0 al target.")]
		[Min(0.05f)]
		[SerializeField]
		private float countUpDuration = 1.1f;

		// Token: 0x04007D4E RID: 32078
		[Tooltip("Easing del count-up. EaseOut: parte rapido, decelera al target.")]
		[SerializeField]
		private AnimationCurve countUpEasing = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f, 0f, 4f),
			new Keyframe(0.5f, 0.85f, 1.2f, 1.2f),
			new Keyframe(1f, 1f, 0.05f, 0f)
		});

		// Token: 0x04007D4F RID: 32079
		[Header("Count-up Audio")]
		[Tooltip("Clip suonata UNA volta al completamento del count-up di ogni row (es. MMSequencingTick).")]
		[SerializeField]
		private AudioClip tickClip;

		// Token: 0x04007D50 RID: 32080
		[Tooltip("AudioSource. Se null, creato runtime su questo GO.")]
		[SerializeField]
		private AudioSource tickAudioSource;

		// Token: 0x04007D51 RID: 32081
		[Range(0f, 1f)]
		[SerializeField]
		private float tickVolume = 0.55f;

		// Token: 0x04007D52 RID: 32082
		[Range(0f, 0.5f)]
		[SerializeField]
		private float tickPitchJitter = 0.05f;

		// Token: 0x04007D53 RID: 32083
		[Range(0.5f, 2f)]
		[SerializeField]
		private float tickBasePitch = 1f;

		// Token: 0x04007D54 RID: 32084
		[Header("Buttons")]
		[Tooltip("Bottone Try Again. UnityEngine.UI.Button o DA_Assets.DAB.DAButton.")]
		[SerializeField]
		private MonoBehaviour tryAgainButton;

		// Token: 0x04007D55 RID: 32085
		[Tooltip("Bottone Continue to Prison. UnityEngine.UI.Button o DA_Assets.DAB.DAButton.")]
		[SerializeField]
		private MonoBehaviour continueToPrisonButton;

		// Token: 0x04007D56 RID: 32086
		[Tooltip("Nome della scena Prison. Default 'GameOver - Prison'.")]
		[SerializeField]
		private string prisonSceneName = "GameOver - Prison";

		// Token: 0x04007D57 RID: 32087
		[Tooltip("Nome della scena hub/casa per il fallback di 'Try Again' quando GameOverManager non è disponibile. Normalmente il ritorno a casa passa da GameOverManager.ReturnToHome.")]
		[SerializeField]
		private string homeSceneName = "CielHome";

		// Token: 0x04007D58 RID: 32088
		[Header("Auto-show on GameOver")]
		[Tooltip("Secondi di attesa dopo OnGameOver prima di mostrare il pannello.")]
		[Min(0f)]
		[SerializeField]
		private float showDelayAfterGameOver = 3f;

		// Token: 0x04007D59 RID: 32089
		[Tooltip("Se true il controller si registra a GameOverManager.OnGameOver.")]
		[SerializeField]
		private bool autoShowOnGameOver = true;

		// Token: 0x04007D5B RID: 32091
		private Coroutine _showRoutine;

		// Token: 0x04007D5C RID: 32092
		private float _showAtRealtime;

		// Token: 0x04007D5D RID: 32093
		private GameOverManager _observedGameOver;

		// Token: 0x04007D5E RID: 32094
		private int _observedGameOverSequence = -1;

		// Token: 0x04007D5F RID: 32095
		private MatchStatsSnapshot _cachedSnapshot;

		// Token: 0x04007D60 RID: 32096
		private bool _hasCachedSnapshot;

		// Token: 0x04007D61 RID: 32097
		private RunOutcome _cachedOutcome;

		// Token: 0x04007D62 RID: 32098
		private bool _hasCachedOutcome;

		// Token: 0x04007D63 RID: 32099
		private MatchRecapRankWidget _rankWidget;

		// Token: 0x04007D64 RID: 32100
		private TMP_Text _scoreLabel;

		// Token: 0x04007D65 RID: 32101
		private bool _hasAnimatedOnce;

		// Token: 0x04007D66 RID: 32102
		private bool _cursorStateSaved;

		// Token: 0x04007D67 RID: 32103
		private CursorLockMode _savedCursorLock;

		// Token: 0x04007D68 RID: 32104
		private bool _savedCursorVisible;

		// Token: 0x04007D69 RID: 32105
		private bool _gameOverListenerRegistered;

		// Token: 0x04007D6A RID: 32106
		private static readonly Color HomeAccent = new Color32(199, 133, 242, byte.MaxValue);

		// Token: 0x04007D6B RID: 32107
		private static readonly Color HomeIvory = new Color32(242, 219, byte.MaxValue, byte.MaxValue);

		// Token: 0x04007D6C RID: 32108
		private static readonly Color HomeMuted = new Color32(190, 168, 204, byte.MaxValue);

		// Token: 0x04007D6D RID: 32109
		private bool _showingHomeReturn;

		// Token: 0x04007D6E RID: 32110
		private GameObject _homeReturnPage;

		// Token: 0x04007D6F RID: 32111
		private Transform _ordinaryRecap;

		// Token: 0x04007D70 RID: 32112
		private TMP_Text _homeTotal;

		// Token: 0x04007D71 RID: 32113
		private TMP_Text _homeBase;

		// Token: 0x04007D72 RID: 32114
		private TMP_Text _homeBonus;

		// Token: 0x04007D73 RID: 32115
		private TMP_Text _homeDistance;

		// Token: 0x04007D74 RID: 32116
		private TMP_Text _homeJourney;

		// Token: 0x04007D75 RID: 32117
		private TMP_Text _homeRewards;

		// Token: 0x04007D76 RID: 32118
		private Button _homeContinue;

		// Token: 0x04007D77 RID: 32119
		private TMP_Text _homeSubtitle;

		// Token: 0x04007D78 RID: 32120
		private GameObject _homeMissionPanel;

		// Token: 0x04007D79 RID: 32121
		private TMP_Text _homeMissionDetails;

		// Token: 0x04007D7A RID: 32122
		private ScrollRect _homeMissionScroll;

		// Token: 0x04007D7B RID: 32123
		private MatchStatsSnapshot? _homeMissionSnapshot;
	}
}
