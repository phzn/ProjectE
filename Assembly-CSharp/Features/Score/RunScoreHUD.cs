using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Features.Detection;
using Features.GameOver;
using Features.Localization;
using Features.MainMenu;
using Features.MatchRecap;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Score
{
	// Token: 0x02000C79 RID: 3193
	[DisallowMultipleComponent]
	public sealed class RunScoreHUD : MonoBehaviour
	{
		// Token: 0x17000F28 RID: 3880
		// (get) Token: 0x06005CA3 RID: 23715 RVA: 0x0024600C File Offset: 0x0024420C
		public RectTransform RewardTarget
		{
			get
			{
				if (this.rewardTarget != null)
				{
					return this.rewardTarget;
				}
				if (this.rewardIcon != null)
				{
					return this.rewardIcon.rectTransform;
				}
				return base.transform as RectTransform;
			}
		}

		// Token: 0x17000F29 RID: 3881
		// (get) Token: 0x06005CA4 RID: 23716 RVA: 0x00246048 File Offset: 0x00244248
		public Sprite RewardIconSprite
		{
			get
			{
				if (this.rewardIconSprite != null)
				{
					return this.rewardIconSprite;
				}
				if (!(this.rewardIcon != null))
				{
					return null;
				}
				return this.rewardIcon.sprite;
			}
		}

		// Token: 0x06005CA5 RID: 23717 RVA: 0x0024607A File Offset: 0x0024427A
		private void Awake()
		{
			this.ResolveReferences();
			this.CacheVisualDefaults();
			this.EnsureSfxSource();
			this.EnsureToastUi();
		}

		// Token: 0x06005CA6 RID: 23718 RVA: 0x00246094 File Offset: 0x00244294
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticState()
		{
			RunScoreHUD.ActiveHuds.Clear();
			RunScoreHUD._toastOwner = null;
			RunScoreHUD._presentationOwner = null;
		}

		// Token: 0x06005CA7 RID: 23719 RVA: 0x002460AC File Offset: 0x002442AC
		private void OnEnable()
		{
			GameplayHudVisibility.RegisterCanvas(base.GetComponentInParent<Canvas>(true));
			this.ResolveReferences();
			this.CacheVisualDefaults();
			this.EnsureSfxSource();
			this.EnsureToastUi();
			if (!RunScoreHUD.ActiveHuds.Contains(this))
			{
				RunScoreHUD.ActiveHuds.Add(this);
			}
			RunScoreHUD.ElectToastOwner();
			if (!this.TryBindService())
			{
				this.SnapTo(0);
			}
		}

		// Token: 0x06005CA8 RID: 23720 RVA: 0x001216F4 File Offset: 0x0011F8F4
		private void OnTransformParentChanged()
		{
			GameplayHudVisibility.RegisterCanvas(base.GetComponentInParent<Canvas>(true));
		}

		// Token: 0x06005CA9 RID: 23721 RVA: 0x00246109 File Offset: 0x00244309
		private void Update()
		{
			if (this._boundService == null)
			{
				this.TryBindService();
			}
			if (this._toastRoot == null)
			{
				this.EnsureToastUi();
			}
		}

		// Token: 0x06005CAA RID: 23722 RVA: 0x00246134 File Offset: 0x00244334
		private void OnDisable()
		{
			RunScoreHUD.ReleaseScorePresentation(this);
			bool flag = RunScoreHUD._toastOwner == this;
			RunScoreHUD.ActiveHuds.Remove(this);
			if (flag)
			{
				RunScoreHUD.RevealAuthoritativeOnAllHuds(false);
				RunScoreHUD._toastOwner = null;
				RunScoreHUD.ElectToastOwner();
			}
			this.UnbindService();
			this._deferredPositiveChanges = 0;
			if (this._countRoutine != null)
			{
				base.StopCoroutine(this._countRoutine);
				this._countRoutine = null;
			}
			if (this._impactRoutine != null)
			{
				base.StopCoroutine(this._impactRoutine);
				this._impactRoutine = null;
			}
			if (this._toastRoutine != null)
			{
				base.StopCoroutine(this._toastRoutine);
				this._toastRoutine = null;
			}
			this._toastQueue.Clear();
			this.HideToastImmediate();
			this.RestoreImpactVisuals();
		}

		// Token: 0x06005CAB RID: 23723 RVA: 0x002461E6 File Offset: 0x002443E6
		private void OnDestroy()
		{
			this.UnbindService();
			if (this._toastRoot != null)
			{
				Object.Destroy(this._toastRoot.gameObject);
			}
		}

		// Token: 0x06005CAC RID: 23724 RVA: 0x0024620C File Offset: 0x0024440C
		private bool TryBindService()
		{
			MatchStatsService matchStatsService = ((this.scoreService != null) ? this.scoreService : MatchStatsService.Instance);
			if (matchStatsService == null)
			{
				return false;
			}
			if (this._boundService != matchStatsService)
			{
				this.UnbindService();
				this._boundService = matchStatsService;
				this._boundService.PointsChanged += this.HandlePointsChanged;
				this._boundService.ScoreAwarded += this.HandleScoreAwarded;
			}
			this.SnapTo(this._boundService.CurrentPoints);
			return true;
		}

		// Token: 0x06005CAD RID: 23725 RVA: 0x0024629C File Offset: 0x0024449C
		private void UnbindService()
		{
			if (this._boundService != null)
			{
				this._boundService.PointsChanged -= this.HandlePointsChanged;
				this._boundService.ScoreAwarded -= this.HandleScoreAwarded;
			}
			this._boundService = null;
		}

		// Token: 0x06005CAE RID: 23726 RVA: 0x002462EC File Offset: 0x002444EC
		private void HandleScoreAwarded(RunScoreAward award)
		{
			if (award.Presentation != RunScorePresentation.Generic || !base.isActiveAndEnabled)
			{
				return;
			}
			this._deferredPositiveChanges++;
			if (RunScoreHUD._toastOwner != this)
			{
				return;
			}
			this.EnqueueToast(award);
		}

		// Token: 0x06005CAF RID: 23727 RVA: 0x00246324 File Offset: 0x00244524
		private void EnqueueToast(RunScoreAward award)
		{
			RunScoreHUD.ToastEntry toastEntry = ((this._toastQueue.Last != null) ? this._toastQueue.Last.Value : null);
			if (toastEntry != null && !string.IsNullOrEmpty(award.GroupKey) && string.Equals(toastEntry.GroupKey, award.GroupKey, StringComparison.Ordinal) && award.UnscaledTimestamp - toastEntry.LastAwardTime <= Mathf.Max(0f, this.toastBatchWindow))
			{
				toastEntry.Points = RunScoreHUD.SaturatingAdd(toastEntry.Points, award.Points);
				toastEntry.TotalAfterAward = award.TotalAfterAward;
				toastEntry.LastAwardTime = award.UnscaledTimestamp;
			}
			else
			{
				this._toastQueue.AddLast(new RunScoreHUD.ToastEntry
				{
					Reason = award.Reason,
					Label = (string.IsNullOrWhiteSpace(award.Label) ? "SCORE" : award.Label),
					GroupKey = award.GroupKey,
					Points = award.Points,
					TotalAfterAward = award.TotalAfterAward,
					LastAwardTime = award.UnscaledTimestamp
				});
			}
			if (this._toastRoutine == null)
			{
				this._toastRoutine = base.StartCoroutine(this.ToastPump());
			}
		}

		// Token: 0x06005CB0 RID: 23728 RVA: 0x00246455 File Offset: 0x00244655
		private IEnumerator ToastPump()
		{
			this.EnsureToastUi();
			while (this._toastQueue.Count > 0)
			{
				float batchUntil = Time.unscaledTime + Mathf.Max(0f, this.toastBatchWindow);
				while (Time.unscaledTime < batchUntil)
				{
					yield return null;
				}
				RunScoreHUD.ToastEntry value = this._toastQueue.First.Value;
				this._toastQueue.RemoveFirst();
				yield return this.PresentToast(value);
			}
			this.HideToastImmediate();
			this._toastRoutine = null;
			yield break;
		}

		// Token: 0x06005CB1 RID: 23729 RVA: 0x00246464 File Offset: 0x00244664
		private IEnumerator PresentToast(RunScoreHUD.ToastEntry entry)
		{
			while (!RunScoreHUD.TryAcquireScorePresentation(this))
			{
				yield return null;
			}
			if (this._toastRoot == null || this._toastGroup == null)
			{
				RunScoreHUD.RevealAwardOnAllHuds(entry.TotalAfterAward, this);
				RunScoreHUD.ReleaseScorePresentation(this);
				yield break;
			}
			LocalizedLegacyGameText.BindSource(this._toastLabel, "score_ui", RunScoreHUD.ResolveAwardKeyword(entry)).FitTranslationToRectangle();
			this._toastPoints.text = "+" + Mathf.Max(0, entry.Points).ToString(CultureInfo.InvariantCulture);
			while (RunScoreHUD.IsDetectionPresentationBlocked())
			{
				yield return null;
			}
			this._toastRoot.gameObject.SetActive(true);
			this._toastRoot.SetAsLastSibling();
			this._toastRestPosition = this.ResolvePlayerToastPosition();
			Vector2 enterFrom = this._toastRestPosition + new Vector2(0f, -10f);
			this._toastRoot.anchoredPosition = enterFrom;
			this._toastRoot.localScale = Vector3.one * 0.78f;
			this._toastGroup.alpha = 0f;
			RunScoreHUD.SetTextAlpha(this._toastPoints, 1f);
			RunScoreHUD.SetTextAlpha(this._toastLabel, 1f);
			float elapsed = 0f;
			while (elapsed < this.toastEnterDuration)
			{
				if (RunScoreHUD.IsDetectionPresentationBlocked())
				{
					this._toastGroup.alpha = 0f;
					yield return null;
				}
				else
				{
					elapsed += Time.unscaledDeltaTime;
					float num = ((this.toastEnterDuration <= 0f) ? 1f : Mathf.Clamp01(elapsed / this.toastEnterDuration));
					float num2 = 1f - Mathf.Pow(1f - num, 3f);
					this._toastRoot.anchoredPosition = Vector2.LerpUnclamped(enterFrom, this._toastRestPosition, num2);
					float num3 = ((num < 0.7f) ? Mathf.LerpUnclamped(0.78f, 1.08f, num / 0.7f) : Mathf.LerpUnclamped(1.08f, 1f, (num - 0.7f) / 0.3f));
					this._toastRoot.localScale = Vector3.one * num3;
					this._toastGroup.alpha = num2;
					yield return null;
				}
			}
			this._toastRoot.anchoredPosition = this._toastRestPosition;
			this._toastRoot.localScale = Vector3.one;
			this._toastGroup.alpha = 1f;
			elapsed = 0f;
			while (elapsed < this.toastHoldDuration)
			{
				if (RunScoreHUD.IsDetectionPresentationBlocked())
				{
					this._toastGroup.alpha = 0f;
					yield return null;
				}
				else
				{
					this._toastGroup.alpha = 1f;
					elapsed += Time.unscaledDeltaTime;
					yield return null;
				}
			}
			Vector2 exitTo = this.ResolveScoreTargetPosition();
			Vector2 control = (this._toastRestPosition + exitTo) * 0.5f + new Vector2(0f, 75f);
			elapsed = 0f;
			while (elapsed < this.toastExitDuration)
			{
				if (RunScoreHUD.IsDetectionPresentationBlocked())
				{
					this._toastGroup.alpha = 0f;
					yield return null;
				}
				else
				{
					elapsed += Time.unscaledDeltaTime;
					float num4 = ((this.toastExitDuration <= 0f) ? 1f : Mathf.Clamp01(elapsed / this.toastExitDuration));
					float num5 = num4 * num4 * (3f - 2f * num4);
					this._toastGroup.alpha = 1f;
					this._toastRoot.anchoredPosition = RunScoreHUD.QuadraticBezier(this._toastRestPosition, control, exitTo, num5);
					this._toastRoot.localScale = Vector3.one * Mathf.Lerp(1f, 0.42f, num5);
					RunScoreHUD.SetTextAlpha(this._toastLabel, 1f - Mathf.Clamp01(num4 * 2.4f));
					RunScoreHUD.SetTextAlpha(this._toastPoints, 1f - Mathf.Clamp01((num4 - 0.82f) / 0.18f));
					yield return null;
				}
			}
			this._toastRoot.anchoredPosition = exitTo;
			RunScoreHUD.RevealAwardOnAllHuds(entry.TotalAfterAward, this);
			this.HideToastImmediate();
			RunScoreHUD.ReleaseScorePresentation(this);
			yield break;
		}

		// Token: 0x06005CB2 RID: 23730 RVA: 0x0024647C File Offset: 0x0024467C
		public static bool TryAcquireScorePresentation(object owner)
		{
			if (owner == null)
			{
				return false;
			}
			Object @object = RunScoreHUD._presentationOwner as Object;
			if (@object != null && @object == null)
			{
				RunScoreHUD._presentationOwner = null;
			}
			if (RunScoreHUD._presentationOwner != null)
			{
				return false;
			}
			RunScoreHUD._presentationOwner = owner;
			return true;
		}

		// Token: 0x06005CB3 RID: 23731 RVA: 0x002464BB File Offset: 0x002446BB
		public static void ReleaseScorePresentation(object owner)
		{
			if (owner != null && RunScoreHUD._presentationOwner == owner)
			{
				RunScoreHUD._presentationOwner = null;
			}
		}

		// Token: 0x06005CB4 RID: 23732 RVA: 0x002464D0 File Offset: 0x002446D0
		private void HandlePointsChanged(int newTotal, int appliedDelta)
		{
			int num = Mathf.Max(0, newTotal);
			if (appliedDelta > 0 && this._deferredPositiveChanges > 0)
			{
				this._deferredPositiveChanges--;
				return;
			}
			if (appliedDelta <= 0)
			{
				this._deferredPositiveChanges = 0;
				this.ClearToastQueue();
			}
			if (appliedDelta <= 0 || (double)num <= this._displayedValue || this.countUpDuration <= 0f)
			{
				this.SnapTo(num);
				return;
			}
			this.RetargetCountUp(num);
		}

		// Token: 0x06005CB5 RID: 23733 RVA: 0x0024653C File Offset: 0x0024473C
		private void RetargetCountUp(int target)
		{
			this._countFrom = this._displayedValue;
			this._countTarget = target;
			this._countElapsed = 0f;
			if (this._countRoutine == null)
			{
				this._countRoutine = base.StartCoroutine(this.CountUpRoutine());
			}
		}

		// Token: 0x06005CB6 RID: 23734 RVA: 0x00246576 File Offset: 0x00244776
		private IEnumerator CountUpRoutine()
		{
			for (;;)
			{
				this._countElapsed += Time.unscaledDeltaTime;
				float num = ((this.countUpDuration <= 0f) ? 1f : Mathf.Clamp01(this._countElapsed / this.countUpDuration));
				float num2 = ((this.countUpCurve != null) ? Mathf.Clamp01(this.countUpCurve.Evaluate(num)) : Mathf.SmoothStep(0f, 1f, num));
				this._displayedValue = this._countFrom + ((double)this._countTarget - this._countFrom) * (double)num2;
				int num3 = RunScoreHUD.ToSafeInt(this._displayedValue);
				if (num3 != this._displayedPoints)
				{
					this._displayedPoints = num3;
					this.RenderScore();
					this.PlayTickIfReady();
				}
				if (num >= 1f)
				{
					break;
				}
				yield return null;
			}
			this._displayedValue = (double)this._countTarget;
			this._displayedPoints = this._countTarget;
			this.RenderScore();
			this._countRoutine = null;
			yield break;
			yield break;
		}

		// Token: 0x06005CB7 RID: 23735 RVA: 0x00246588 File Offset: 0x00244788
		private void SnapTo(int total)
		{
			if (this._countRoutine != null)
			{
				base.StopCoroutine(this._countRoutine);
				this._countRoutine = null;
			}
			this._countTarget = Mathf.Max(0, total);
			this._countFrom = (double)this._countTarget;
			this._countElapsed = 0f;
			this._displayedValue = (double)this._countTarget;
			this._displayedPoints = this._countTarget;
			this.RenderScore();
		}

		// Token: 0x06005CB8 RID: 23736 RVA: 0x002465F4 File Offset: 0x002447F4
		private void RenderScore()
		{
			if (this.scoreText == null)
			{
				return;
			}
			string text = (string.IsNullOrWhiteSpace(this.numberFormat) ? "0" : this.numberFormat);
			try
			{
				this.scoreText.text = this._displayedPoints.ToString(text, CultureInfo.InvariantCulture);
			}
			catch (FormatException)
			{
				this.scoreText.text = this._displayedPoints.ToString(CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x06005CB9 RID: 23737 RVA: 0x00246678 File Offset: 0x00244878
		private static int ToSafeInt(double value)
		{
			if (value <= 0.0)
			{
				return 0;
			}
			if (value >= 2147483647.0)
			{
				return int.MaxValue;
			}
			return (int)Math.Round(value, MidpointRounding.AwayFromZero);
		}

		// Token: 0x06005CBA RID: 23738 RVA: 0x002466A2 File Offset: 0x002448A2
		public bool DeferNextPositiveChange()
		{
			if (!base.isActiveAndEnabled || this._boundService == null)
			{
				return false;
			}
			this._deferredPositiveChanges++;
			return true;
		}

		// Token: 0x06005CBB RID: 23739 RVA: 0x002466CB File Offset: 0x002448CB
		public void CancelDeferredPositiveChange()
		{
			this._deferredPositiveChanges = Mathf.Max(0, this._deferredPositiveChanges - 1);
		}

		// Token: 0x06005CBC RID: 23740 RVA: 0x002466E4 File Offset: 0x002448E4
		public void RevealAuthoritativeScore(bool animate)
		{
			this._deferredPositiveChanges = 0;
			MatchStatsService matchStatsService = ((this._boundService != null) ? this._boundService : ((this.scoreService != null) ? this.scoreService : MatchStatsService.Instance));
			int num = ((matchStatsService != null) ? matchStatsService.CurrentPoints : 0);
			if (animate && (double)num > this._displayedValue && this.countUpDuration > 0f)
			{
				this.RetargetCountUp(num);
				return;
			}
			this.SnapTo(num);
		}

		// Token: 0x06005CBD RID: 23741 RVA: 0x00246768 File Offset: 0x00244968
		public void PlayArrivalImpact()
		{
			int num = ((this._boundService != null) ? this._boundService.CurrentPoints : this._displayedPoints);
			this.PlayArrivalImpact(num);
		}

		// Token: 0x06005CBE RID: 23742 RVA: 0x002467A0 File Offset: 0x002449A0
		public void PlayArrivalImpact(int revealThroughTotal)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this.ResolveReferences();
			this.CacheVisualDefaults();
			this.RevealThrough(revealThroughTotal);
			if (this._impactRoutine != null)
			{
				base.StopCoroutine(this._impactRoutine);
				this._impactRoutine = null;
				this.RestoreImpactVisuals();
			}
			if (this.impactDuration > 0f)
			{
				this._impactRoutine = base.StartCoroutine(this.ArrivalImpactRoutine());
			}
			this.PlayOneShot(this.rewardSfx, this.rewardVolume);
		}

		// Token: 0x06005CBF RID: 23743 RVA: 0x0024681B File Offset: 0x00244A1B
		public void RevealThroughTotal(int revealThroughTotal)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			this.RevealThrough(revealThroughTotal);
		}

		// Token: 0x06005CC0 RID: 23744 RVA: 0x00246830 File Offset: 0x00244A30
		private void RevealThrough(int requestedTotal)
		{
			int num = ((this._boundService != null) ? this._boundService.CurrentPoints : Mathf.Max(0, requestedTotal));
			int num2 = Mathf.Clamp(requestedTotal, 0, num);
			if ((double)num2 <= this._displayedValue)
			{
				return;
			}
			if (this._countRoutine != null && num2 <= this._countTarget)
			{
				return;
			}
			if (this.countUpDuration <= 0f)
			{
				this.SnapTo(num2);
				return;
			}
			this.RetargetCountUp(num2);
		}

		// Token: 0x06005CC1 RID: 23745 RVA: 0x002468A2 File Offset: 0x00244AA2
		private IEnumerator ArrivalImpactRoutine()
		{
			float elapsed = 0f;
			while (elapsed < this.impactDuration)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / this.impactDuration);
				float num2 = Mathf.Sin(num * 3.1415927f) * (1f - num * 0.18f);
				if (this._textDefaultsCached && this.scoreText != null)
				{
					this.scoreText.rectTransform.localScale = this._textBaseScale * (1f + this.textScalePunch * num2);
					this.scoreText.color = Color.Lerp(this._textBaseColor, this.impactTint, this.textTintStrength * num2);
				}
				if (this._iconDefaultsCached && this.rewardIcon != null)
				{
					this.rewardIcon.rectTransform.localScale = this._iconBaseScale * (1f + this.iconScalePunch * num2);
					this.rewardIcon.color = Color.Lerp(this._iconBaseColor, this.impactTint, this.iconTintStrength * num2);
				}
				if (this._backgroundDefaultsCached && this.background != null)
				{
					this.background.color = Color.Lerp(this._backgroundBaseColor, this.impactTint, this.backgroundTintStrength * num2);
				}
				yield return null;
			}
			this.RestoreImpactVisuals();
			this._impactRoutine = null;
			yield break;
		}

		// Token: 0x06005CC2 RID: 23746 RVA: 0x002468B4 File Offset: 0x00244AB4
		private void PlayTickIfReady()
		{
			if (this.tickSfx == null || Time.unscaledTime < this._nextTickTime)
			{
				return;
			}
			this._nextTickTime = Time.unscaledTime + Mathf.Max(0.02f, this.tickInterval);
			this.PlayOneShot(this.tickSfx, this.tickVolume);
		}

		// Token: 0x06005CC3 RID: 23747 RVA: 0x0024690B File Offset: 0x00244B0B
		private void PlayOneShot(AudioClip clip, float volume)
		{
			if (clip == null)
			{
				return;
			}
			this.EnsureSfxSource();
			if (this.sfxSource != null)
			{
				this.sfxSource.PlayOneShot(clip, Mathf.Clamp01(volume));
			}
		}

		// Token: 0x06005CC4 RID: 23748 RVA: 0x00246940 File Offset: 0x00244B40
		private void EnsureSfxSource()
		{
			if (this.sfxSource == null)
			{
				this.sfxSource = base.GetComponent<AudioSource>();
			}
			if (this.sfxSource == null)
			{
				this.sfxSource = base.gameObject.AddComponent<AudioSource>();
			}
			this.sfxSource.playOnAwake = false;
			this.sfxSource.loop = false;
			this.sfxSource.spatialBlend = 0f;
			this.sfxSource.dopplerLevel = 0f;
			this.sfxSource.ignoreListenerPause = true;
			this.sfxSource.ignoreListenerVolume = false;
			this.sfxSource.bypassReverbZones = true;
			this.sfxSource.volume = 1f;
			GameAudioRouter.Route(this.sfxSource, GameAudioRouter.Channel.Sfx);
		}

		// Token: 0x06005CC5 RID: 23749 RVA: 0x00246A00 File Offset: 0x00244C00
		private static void ElectToastOwner()
		{
			if (RunScoreHUD._toastOwner != null && RunScoreHUD._toastOwner.isActiveAndEnabled)
			{
				return;
			}
			RunScoreHUD._toastOwner = null;
			for (int i = RunScoreHUD.ActiveHuds.Count - 1; i >= 0; i--)
			{
				RunScoreHUD runScoreHUD = RunScoreHUD.ActiveHuds[i];
				if (runScoreHUD == null)
				{
					RunScoreHUD.ActiveHuds.RemoveAt(i);
				}
				else if (RunScoreHUD._toastOwner == null && runScoreHUD.isActiveAndEnabled)
				{
					RunScoreHUD._toastOwner = runScoreHUD;
				}
			}
		}

		// Token: 0x06005CC6 RID: 23750 RVA: 0x00246A84 File Offset: 0x00244C84
		private static void RevealAwardOnAllHuds(int total, RunScoreHUD impactHud)
		{
			for (int i = RunScoreHUD.ActiveHuds.Count - 1; i >= 0; i--)
			{
				RunScoreHUD runScoreHUD = RunScoreHUD.ActiveHuds[i];
				if (runScoreHUD == null)
				{
					RunScoreHUD.ActiveHuds.RemoveAt(i);
				}
				else if (runScoreHUD.isActiveAndEnabled)
				{
					if (runScoreHUD == impactHud)
					{
						runScoreHUD.PlayArrivalImpact(total);
					}
					else
					{
						runScoreHUD.RevealThroughTotal(total);
					}
				}
			}
		}

		// Token: 0x06005CC7 RID: 23751 RVA: 0x00246AEC File Offset: 0x00244CEC
		private static void RevealAuthoritativeOnAllHuds(bool animate)
		{
			for (int i = RunScoreHUD.ActiveHuds.Count - 1; i >= 0; i--)
			{
				RunScoreHUD runScoreHUD = RunScoreHUD.ActiveHuds[i];
				if (runScoreHUD == null)
				{
					RunScoreHUD.ActiveHuds.RemoveAt(i);
				}
				else if (runScoreHUD.isActiveAndEnabled)
				{
					runScoreHUD.RevealAuthoritativeScore(animate);
				}
			}
		}

		// Token: 0x06005CC8 RID: 23752 RVA: 0x00246B44 File Offset: 0x00244D44
		private static bool IsDetectionPresentationBlocked()
		{
			GameOverManager instance = GameOverManager.Instance;
			if (instance != null && instance.IsGameOver)
			{
				return true;
			}
			ExhibitionEventDirector instance2 = ExhibitionEventDirector.Instance;
			return instance2 != null && instance2.isActiveAndEnabled && instance2.IsEventActive;
		}

		// Token: 0x06005CC9 RID: 23753 RVA: 0x00246B8C File Offset: 0x00244D8C
		private static string ResolveAwardKeyword(RunScoreHUD.ToastEntry entry)
		{
			switch (entry.Reason)
			{
			case RunScoreReason.PoiDiscovered:
				return "DISCOVERY";
			case RunScoreReason.Exhibition:
			case RunScoreReason.ExhibitionCrowd:
			case RunScoreReason.ExhibitionInPoi:
				return "EXHIBITION";
			case RunScoreReason.CarSurf:
			case RunScoreReason.CarSurfFullRide:
			case RunScoreReason.CarSurfEmote:
				return "SURF";
			case RunScoreReason.HiddenUnderCar:
				return "HIDDEN";
			case RunScoreReason.NpcEscape:
			case RunScoreReason.ReporterEscape:
			case RunScoreReason.PoliceEscape:
			case RunScoreReason.HidingEscape:
				return "ESCAPE";
			case RunScoreReason.VehicleDistance:
				return "RIDE";
			default:
			{
				if (string.IsNullOrWhiteSpace(entry.Label))
				{
					return "SCORE";
				}
				string text = entry.Label.Trim();
				int num = text.IndexOf(' ');
				return ((num > 0) ? text.Substring(0, num) : text).ToUpperInvariant();
			}
			}
		}

		// Token: 0x06005CCA RID: 23754 RVA: 0x00246C3E File Offset: 0x00244E3E
		private static int SaturatingAdd(int left, int right)
		{
			if (right > 0 && left > 2147483647 - right)
			{
				return int.MaxValue;
			}
			if (right < 0 && left < -2147483648 - right)
			{
				return int.MinValue;
			}
			return left + right;
		}

		// Token: 0x06005CCB RID: 23755 RVA: 0x00246C6C File Offset: 0x00244E6C
		private void EnsureToastUi()
		{
			this.ResolveReferences();
			Canvas componentInParent = base.GetComponentInParent<Canvas>();
			this._toastCanvas = ((componentInParent != null) ? componentInParent.rootCanvas : null);
			this._toastCanvasRect = ((this._toastCanvas != null) ? (this._toastCanvas.transform as RectTransform) : null);
			if (this._toastCanvasRect == null)
			{
				return;
			}
			if (this._toastRoot != null && this._toastRoot.parent == this._toastCanvasRect && this._toastRoot.name == "Floating Score Award")
			{
				return;
			}
			if (this._toastRoot != null)
			{
				Object.Destroy(this._toastRoot.gameObject);
			}
			Transform transform = base.transform.Find("Score Award Toast");
			if (transform != null)
			{
				Object.Destroy(transform.gameObject);
			}
			Font font = ((this.scoreText != null) ? LocalizedLegacyGameText.SourceFont(this.scoreText) : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"));
			if (font == null)
			{
				return;
			}
			GameObject gameObject = new GameObject("Floating Score Award", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasGroup)
			});
			this._toastRoot = gameObject.GetComponent<RectTransform>();
			this._toastRoot.SetParent(this._toastCanvasRect, false);
			RectTransform toastRoot = this._toastRoot;
			RectTransform toastRoot2 = this._toastRoot;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			toastRoot2.anchorMax = vector;
			toastRoot.anchorMin = vector;
			this._toastRoot.pivot = new Vector2(0.5f, 0.5f);
			this._toastRoot.sizeDelta = new Vector2(220f, 62f);
			this._toastRestPosition = Vector2.zero;
			this._toastRoot.anchoredPosition = this._toastRestPosition;
			this._toastRoot.SetAsLastSibling();
			this._toastGroup = gameObject.GetComponent<CanvasGroup>();
			this._toastGroup.interactable = false;
			this._toastGroup.blocksRaycasts = false;
			GameplayHudVisualMask.Attach(gameObject, HudDisplayPart.DistanceAndScore);
			this._toastPoints = RunScoreHUD.CreateToastText("Points", this._toastRoot, font, 30, FontStyle.Bold, this.toastPointsColor, TextAnchor.MiddleCenter);
			RunScoreHUD.SetToastRect(this._toastPoints.rectTransform, new Vector2(0f, 0.42f), Vector2.one, Vector2.zero, Vector2.zero);
			RunScoreHUD.AddToastOutline(this._toastPoints, new Color(0.16f, 0.02f, 0.22f, 0.94f), new Vector2(2f, -2f));
			this._toastLabel = RunScoreHUD.CreateToastText("Keyword", this._toastRoot, font, 13, FontStyle.Bold, this.toastKeywordColor, TextAnchor.UpperCenter);
			RunScoreHUD.SetToastRect(this._toastLabel.rectTransform, Vector2.zero, new Vector2(1f, 0.46f), Vector2.zero, Vector2.zero);
			RunScoreHUD.AddToastOutline(this._toastLabel, new Color(0.16f, 0.02f, 0.22f, 0.92f), new Vector2(1f, -1f));
			this.HideToastImmediate();
		}

		// Token: 0x06005CCC RID: 23756 RVA: 0x00246F8C File Offset: 0x0024518C
		private static Text CreateToastText(string name, Transform parent, Font font, int size, FontStyle style, Color color, TextAnchor alignment)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Text)
			});
			gameObject.GetComponent<RectTransform>().SetParent(parent, false);
			Text component = gameObject.GetComponent<Text>();
			component.font = font;
			component.fontSize = size;
			component.fontStyle = style;
			component.color = color;
			component.alignment = alignment;
			component.horizontalOverflow = HorizontalWrapMode.Overflow;
			component.verticalOverflow = VerticalWrapMode.Truncate;
			component.raycastTarget = false;
			component.supportRichText = false;
			return component;
		}

		// Token: 0x06005CCD RID: 23757 RVA: 0x00247020 File Offset: 0x00245220
		private static void AddToastOutline(Text text, Color color, Vector2 distance)
		{
			if (text == null)
			{
				return;
			}
			Outline outline = text.gameObject.AddComponent<Outline>();
			outline.effectColor = color;
			outline.effectDistance = distance;
			outline.useGraphicAlpha = true;
		}

		// Token: 0x06005CCE RID: 23758 RVA: 0x0018F85B File Offset: 0x0018DA5B
		private static void SetToastRect(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
		{
			rect.anchorMin = anchorMin;
			rect.anchorMax = anchorMax;
			rect.offsetMin = offsetMin;
			rect.offsetMax = offsetMax;
		}

		// Token: 0x06005CCF RID: 23759 RVA: 0x0024704C File Offset: 0x0024524C
		private void HideToastImmediate()
		{
			if (this._toastRoot == null)
			{
				return;
			}
			this._toastRoot.anchoredPosition = this._toastRestPosition;
			this._toastRoot.localScale = Vector3.one;
			if (this._toastGroup != null)
			{
				this._toastGroup.alpha = 0f;
			}
			this._toastRoot.gameObject.SetActive(false);
		}

		// Token: 0x06005CD0 RID: 23760 RVA: 0x002470B8 File Offset: 0x002452B8
		private Vector2 ResolvePlayerToastPosition()
		{
			Vector2 vector = new Vector2((this._toastCanvasRect != null) ? (this._toastCanvasRect.rect.width * 0.08f) : 120f, (this._toastCanvasRect != null) ? (this._toastCanvasRect.rect.height * 0.06f) : 55f);
			Camera main = Camera.main;
			SensoryManager instance = SensoryManager.Instance;
			StealthActor stealthActor = ((instance != null) ? instance.GetPlayer() : null);
			if (main == null || stealthActor == null || this._toastCanvasRect == null)
			{
				return this.ClampToastPosition(vector);
			}
			Vector3 vector2 = main.WorldToScreenPoint(stealthActor.transform.position + Vector3.up * 1.35f);
			if (vector2.z <= 0f)
			{
				return this.ClampToastPosition(vector);
			}
			Camera camera = this.ResolveCanvasCamera();
			Vector2 vector3;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this._toastCanvasRect, vector2, camera, out vector3))
			{
				return this.ClampToastPosition(vector);
			}
			return this.ClampToastPosition(vector3 + this.toastPlayerOffset);
		}

		// Token: 0x06005CD1 RID: 23761 RVA: 0x002471EC File Offset: 0x002453EC
		private Vector2 ResolveScoreTargetPosition()
		{
			if (this._toastCanvasRect == null || this.RewardTarget == null)
			{
				return this._toastRestPosition + new Vector2(280f, 180f);
			}
			Camera camera = this.ResolveCanvasCamera();
			Vector2 vector = RectTransformUtility.WorldToScreenPoint(camera, this.RewardTarget.position);
			Vector2 vector2;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this._toastCanvasRect, vector, camera, out vector2))
			{
				return this._toastRestPosition + new Vector2(280f, 180f);
			}
			return vector2;
		}

		// Token: 0x06005CD2 RID: 23762 RVA: 0x00247278 File Offset: 0x00245478
		private Camera ResolveCanvasCamera()
		{
			if (this._toastCanvas == null || this._toastCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				return null;
			}
			if (!(this._toastCanvas.worldCamera != null))
			{
				return Camera.main;
			}
			return this._toastCanvas.worldCamera;
		}

		// Token: 0x06005CD3 RID: 23763 RVA: 0x002472C8 File Offset: 0x002454C8
		private Vector2 ClampToastPosition(Vector2 position)
		{
			if (this._toastCanvasRect == null)
			{
				return position;
			}
			Rect rect = this._toastCanvasRect.rect;
			position.x = Mathf.Clamp(position.x, rect.xMin + 118f, rect.xMax - 118f);
			position.y = Mathf.Clamp(position.y, rect.yMin + 42f, rect.yMax - 42f);
			return position;
		}

		// Token: 0x06005CD4 RID: 23764 RVA: 0x0024734C File Offset: 0x0024554C
		private static Vector2 QuadraticBezier(Vector2 start, Vector2 control, Vector2 end, float t)
		{
			float num = 1f - t;
			return num * num * start + 2f * num * t * control + t * t * end;
		}

		// Token: 0x06005CD5 RID: 23765 RVA: 0x0024738C File Offset: 0x0024558C
		private static void SetTextAlpha(Text text, float alpha)
		{
			if (text == null)
			{
				return;
			}
			Color color = text.color;
			color.a = Mathf.Clamp01(alpha);
			text.color = color;
		}

		// Token: 0x06005CD6 RID: 23766 RVA: 0x002473BE File Offset: 0x002455BE
		private void ClearToastQueue()
		{
			this._toastQueue.Clear();
			RunScoreHUD.ReleaseScorePresentation(this);
			if (this._toastRoutine != null)
			{
				base.StopCoroutine(this._toastRoutine);
				this._toastRoutine = null;
			}
			this.HideToastImmediate();
		}

		// Token: 0x06005CD7 RID: 23767 RVA: 0x002473F4 File Offset: 0x002455F4
		private void ResolveReferences()
		{
			if (this.scoreText == null)
			{
				this.scoreText = base.GetComponentInChildren<Text>(true);
			}
			if (this.background == null)
			{
				this.background = base.GetComponent<Graphic>();
			}
			if (this.rewardIcon == null)
			{
				foreach (Image image in base.GetComponentsInChildren<Image>(true))
				{
					if (!(image == null) && !(image == this.background) && !(image.sprite == null))
					{
						this.rewardIcon = image;
						return;
					}
				}
			}
		}

		// Token: 0x06005CD8 RID: 23768 RVA: 0x0024748C File Offset: 0x0024568C
		private void CacheVisualDefaults()
		{
			if (!this._textDefaultsCached && this.scoreText != null)
			{
				this._textBaseScale = this.scoreText.rectTransform.localScale;
				this._textBaseColor = this.scoreText.color;
				this._textDefaultsCached = true;
			}
			if (!this._iconDefaultsCached && this.rewardIcon != null)
			{
				this._iconBaseScale = this.rewardIcon.rectTransform.localScale;
				this._iconBaseColor = this.rewardIcon.color;
				this._iconDefaultsCached = true;
			}
			if (!this._backgroundDefaultsCached && this.background != null)
			{
				this._backgroundBaseColor = this.background.color;
				this._backgroundDefaultsCached = true;
			}
		}

		// Token: 0x06005CD9 RID: 23769 RVA: 0x00247550 File Offset: 0x00245750
		private void RestoreImpactVisuals()
		{
			if (this._textDefaultsCached && this.scoreText != null)
			{
				this.scoreText.rectTransform.localScale = this._textBaseScale;
				this.scoreText.color = this._textBaseColor;
			}
			if (this._iconDefaultsCached && this.rewardIcon != null)
			{
				this.rewardIcon.rectTransform.localScale = this._iconBaseScale;
				this.rewardIcon.color = this._iconBaseColor;
			}
			if (this._backgroundDefaultsCached && this.background != null)
			{
				this.background.color = this._backgroundBaseColor;
			}
		}

		// Token: 0x04005DF0 RID: 24048
		[Header("References")]
		[Tooltip("Servizio autorevole. Se null usa MatchStatsService.Instance.")]
		[SerializeField]
		private MatchStatsService scoreService;

		// Token: 0x04005DF1 RID: 24049
		[Tooltip("Legacy UI.Text che visualizza il totale punti.")]
		[SerializeField]
		private Text scoreText;

		// Token: 0x04005DF2 RID: 24050
		[Tooltip("Icona dei punti, usata anche come fallback per RewardTarget e RewardIconSprite.")]
		[SerializeField]
		private Image rewardIcon;

		// Token: 0x04005DF3 RID: 24051
		[Tooltip("Background del blocco punti. Riceve un flash cromatico all'impatto.")]
		[SerializeField]
		private Graphic background;

		// Token: 0x04005DF4 RID: 24052
		[Tooltip("Destinazione UI per eventuali reward che volano verso il contatore.")]
		[SerializeField]
		private RectTransform rewardTarget;

		// Token: 0x04005DF5 RID: 24053
		[Tooltip("Sprite esposto ai presenter dei reward. Se null usa lo sprite di Reward Icon.")]
		[SerializeField]
		private Sprite rewardIconSprite;

		// Token: 0x04005DF6 RID: 24054
		[Header("Count-up")]
		[SerializeField]
		[Min(0f)]
		private float countUpDuration = 0.65f;

		// Token: 0x04005DF7 RID: 24055
		[SerializeField]
		private AnimationCurve countUpCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x04005DF8 RID: 24056
		[Tooltip("Formato numerico standard C#. '0' mantiene il conteggio senza separatori.")]
		[SerializeField]
		private string numberFormat = "0";

		// Token: 0x04005DF9 RID: 24057
		[Header("Arrival impact")]
		[SerializeField]
		[Min(0f)]
		private float impactDuration = 0.32f;

		// Token: 0x04005DFA RID: 24058
		[SerializeField]
		[Range(0f, 0.5f)]
		private float textScalePunch = 0.16f;

		// Token: 0x04005DFB RID: 24059
		[SerializeField]
		[Range(0f, 0.5f)]
		private float iconScalePunch = 0.24f;

		// Token: 0x04005DFC RID: 24060
		[SerializeField]
		[Range(0f, 1f)]
		private float textTintStrength = 0.72f;

		// Token: 0x04005DFD RID: 24061
		[SerializeField]
		[Range(0f, 1f)]
		private float iconTintStrength = 0.55f;

		// Token: 0x04005DFE RID: 24062
		[SerializeField]
		[Range(0f, 1f)]
		private float backgroundTintStrength = 0.42f;

		// Token: 0x04005DFF RID: 24063
		[SerializeField]
		private Color impactTint = new Color(1f, 0.78f, 0.24f, 1f);

		// Token: 0x04005E00 RID: 24064
		[Header("SFX (2D / SFX mixer)")]
		[Tooltip("Source dedicata. Se null viene riusata o creata una AudioSource sul GameObject.")]
		[SerializeField]
		private AudioSource sfxSource;

		// Token: 0x04005E01 RID: 24065
		[SerializeField]
		private AudioClip rewardSfx;

		// Token: 0x04005E02 RID: 24066
		[SerializeField]
		private AudioClip tickSfx;

		// Token: 0x04005E03 RID: 24067
		[SerializeField]
		[Range(0f, 1f)]
		private float rewardVolume = 0.8f;

		// Token: 0x04005E04 RID: 24068
		[SerializeField]
		[Range(0f, 1f)]
		private float tickVolume = 0.18f;

		// Token: 0x04005E05 RID: 24069
		[SerializeField]
		[Min(0.02f)]
		private float tickInterval = 0.075f;

		// Token: 0x04005E06 RID: 24070
		[Header("Floating reward feedback")]
		[SerializeField]
		[Min(0.05f)]
		private float toastEnterDuration = 0.15f;

		// Token: 0x04005E07 RID: 24071
		[SerializeField]
		[Min(0.1f)]
		private float toastHoldDuration = 0.55f;

		// Token: 0x04005E08 RID: 24072
		[Tooltip("Durata del volo dei punti verso il contatore.")]
		[SerializeField]
		[Min(0.05f)]
		private float toastExitDuration = 0.48f;

		// Token: 0x04005E09 RID: 24073
		[Tooltip("Breve finestra in cui i bonus prodotti dalla stessa azione vengono sommati.")]
		[SerializeField]
		[Range(0f, 0.5f)]
		private float toastBatchWindow = 0.12f;

		// Token: 0x04005E0A RID: 24074
		[SerializeField]
		private Vector2 toastPlayerOffset = new Vector2(135f, 85f);

		// Token: 0x04005E0B RID: 24075
		[SerializeField]
		private Color toastPointsColor = new Color(1f, 0.84f, 0.91f, 1f);

		// Token: 0x04005E0C RID: 24076
		[SerializeField]
		private Color toastKeywordColor = new Color(1f, 0.25f, 0.52f, 1f);

		// Token: 0x04005E0D RID: 24077
		private MatchStatsService _boundService;

		// Token: 0x04005E0E RID: 24078
		private Coroutine _countRoutine;

		// Token: 0x04005E0F RID: 24079
		private Coroutine _impactRoutine;

		// Token: 0x04005E10 RID: 24080
		private Coroutine _toastRoutine;

		// Token: 0x04005E11 RID: 24081
		private readonly LinkedList<RunScoreHUD.ToastEntry> _toastQueue = new LinkedList<RunScoreHUD.ToastEntry>();

		// Token: 0x04005E12 RID: 24082
		private RectTransform _toastRoot;

		// Token: 0x04005E13 RID: 24083
		private CanvasGroup _toastGroup;

		// Token: 0x04005E14 RID: 24084
		private Text _toastLabel;

		// Token: 0x04005E15 RID: 24085
		private Text _toastPoints;

		// Token: 0x04005E16 RID: 24086
		private Canvas _toastCanvas;

		// Token: 0x04005E17 RID: 24087
		private RectTransform _toastCanvasRect;

		// Token: 0x04005E18 RID: 24088
		private Vector2 _toastRestPosition;

		// Token: 0x04005E19 RID: 24089
		private static readonly List<RunScoreHUD> ActiveHuds = new List<RunScoreHUD>();

		// Token: 0x04005E1A RID: 24090
		private static RunScoreHUD _toastOwner;

		// Token: 0x04005E1B RID: 24091
		private static object _presentationOwner;

		// Token: 0x04005E1C RID: 24092
		private double _displayedValue;

		// Token: 0x04005E1D RID: 24093
		private int _displayedPoints;

		// Token: 0x04005E1E RID: 24094
		private double _countFrom;

		// Token: 0x04005E1F RID: 24095
		private int _countTarget;

		// Token: 0x04005E20 RID: 24096
		private float _countElapsed;

		// Token: 0x04005E21 RID: 24097
		private float _nextTickTime;

		// Token: 0x04005E22 RID: 24098
		private int _deferredPositiveChanges;

		// Token: 0x04005E23 RID: 24099
		private Vector3 _textBaseScale;

		// Token: 0x04005E24 RID: 24100
		private Vector3 _iconBaseScale;

		// Token: 0x04005E25 RID: 24101
		private Color _textBaseColor;

		// Token: 0x04005E26 RID: 24102
		private Color _iconBaseColor;

		// Token: 0x04005E27 RID: 24103
		private Color _backgroundBaseColor;

		// Token: 0x04005E28 RID: 24104
		private bool _textDefaultsCached;

		// Token: 0x04005E29 RID: 24105
		private bool _iconDefaultsCached;

		// Token: 0x04005E2A RID: 24106
		private bool _backgroundDefaultsCached;

		// Token: 0x02000C7A RID: 3194
		private sealed class ToastEntry
		{
			// Token: 0x04005E2B RID: 24107
			public RunScoreReason Reason;

			// Token: 0x04005E2C RID: 24108
			public string Label;

			// Token: 0x04005E2D RID: 24109
			public string GroupKey;

			// Token: 0x04005E2E RID: 24110
			public int Points;

			// Token: 0x04005E2F RID: 24111
			public int TotalAfterAward;

			// Token: 0x04005E30 RID: 24112
			public float LastAwardTime;
		}
	}
}
