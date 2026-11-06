using System;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.UI.TimedEffects
{
	// Token: 0x02000BC4 RID: 3012
	public class TimedEffectHUDEntry : MonoBehaviour
	{
		// Token: 0x17000E56 RID: 3670
		// (get) Token: 0x06005667 RID: 22119 RVA: 0x002194E3 File Offset: 0x002176E3
		public string EffectId
		{
			get
			{
				return this.effectId;
			}
		}

		// Token: 0x17000E57 RID: 3671
		// (get) Token: 0x06005668 RID: 22120 RVA: 0x002194EB File Offset: 0x002176EB
		public bool IsActive
		{
			get
			{
				return this.isActive;
			}
		}

		// Token: 0x17000E58 RID: 3672
		// (get) Token: 0x06005669 RID: 22121 RVA: 0x002194F3 File Offset: 0x002176F3
		public float RemainingTime
		{
			get
			{
				return this.remainingTime;
			}
		}

		// Token: 0x17000E59 RID: 3673
		// (get) Token: 0x0600566A RID: 22122 RVA: 0x002194FB File Offset: 0x002176FB
		internal bool CanPresent
		{
			get
			{
				return this.isActive && !this.isFadingOut && this.remainingTime > 0f;
			}
		}

		// Token: 0x17000E5A RID: 3674
		// (get) Token: 0x0600566B RID: 22123 RVA: 0x0021951C File Offset: 0x0021771C
		internal bool IsPaused
		{
			get
			{
				return this.isPaused;
			}
		}

		// Token: 0x17000E5B RID: 3675
		// (get) Token: 0x0600566C RID: 22124 RVA: 0x00219524 File Offset: 0x00217724
		internal bool IsPresented
		{
			get
			{
				return this.presentationVisible && this.CanPresent;
			}
		}

		// Token: 0x17000E5C RID: 3676
		// (get) Token: 0x0600566D RID: 22125 RVA: 0x00219536 File Offset: 0x00217736
		public float NormalizedProgress
		{
			get
			{
				if (float.IsPositiveInfinity(this.remainingTime))
				{
					return 1f;
				}
				if (this.totalDuration <= 0f)
				{
					return 0f;
				}
				return Mathf.Clamp01(this.remainingTime / this.totalDuration);
			}
		}

		// Token: 0x0600566E RID: 22126 RVA: 0x00219570 File Offset: 0x00217770
		internal void SetPresentationVisible(bool visible)
		{
			if (this.presentationVisible == visible)
			{
				return;
			}
			this.presentationVisible = visible;
			this.ApplyPresentationAlpha();
		}

		// Token: 0x0600566F RID: 22127 RVA: 0x00219589 File Offset: 0x00217789
		private void ApplyPresentationAlpha()
		{
			if (this.canvasGroup != null)
			{
				this.canvasGroup.alpha = (this.presentationVisible ? this.fadeAlpha : 0f);
			}
		}

		// Token: 0x06005670 RID: 22128 RVA: 0x002195BC File Offset: 0x002177BC
		internal void SetCompactHudSize(float width, float height)
		{
			RectTransform rectTransform = base.transform as RectTransform;
			if (rectTransform == null)
			{
				return;
			}
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
			HorizontalLayoutGroup component = base.GetComponent<HorizontalLayoutGroup>();
			if (component == null)
			{
				return;
			}
			component.padding.top = (component.padding.bottom = 0);
			float num = (float)component.padding.horizontal;
			int num2 = 0;
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				if ((!(this.labelText != null) || !(transform == this.labelText.transform)) && transform.gameObject.activeSelf)
				{
					RectTransform rectTransform2 = transform as RectTransform;
					if (rectTransform2 != null)
					{
						rectTransform2.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 36f);
						rectTransform2.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 36f);
						num += rectTransform2.rect.width;
						num2++;
					}
				}
			}
			if (this.timerText != null)
			{
				this.timerText.fontSize = 16f;
				this.timerText.enableAutoSizing = true;
				this.timerText.fontSizeMin = 12f;
				this.timerText.fontSizeMax = 16f;
				this.timerText.enableWordWrapping = false;
			}
			if (this.labelText != null)
			{
				this.labelText.enableAutoSizing = true;
				this.labelText.enableWordWrapping = true;
				this.labelText.fontSizeMin = 14f;
				this.labelText.fontSizeMax = 18f;
				this.labelText.overflowMode = TextOverflowModes.Ellipsis;
				this.labelText.maxVisibleLines = 2;
				this.labelText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Max(16f, width - num - component.spacing * (float)num2));
				this.labelText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
			}
		}

		// Token: 0x06005671 RID: 22129 RVA: 0x002197D4 File Offset: 0x002179D4
		public void Setup(TimedEffectData data)
		{
			if (data == null)
			{
				return;
			}
			this.effectId = data.EffectId;
			this.totalDuration = Mathf.Max(0.01f, data.TotalDuration);
			this.remainingTime = this.totalDuration;
			this.isActive = true;
			this.isFadingOut = false;
			this.externalTimeControl = data.ExternalTimeControl;
			this.isPaused = false;
			this.displayName = data.DisplayName;
			this.fadeProgress = 0f;
			this.fadeAlpha = 0f;
			if (this.iconImage != null)
			{
				this.iconImage.sprite = data.Icon;
				this.iconImage.enabled = data.Icon != null;
			}
			if (this.radialFill != null)
			{
				this.radialFill.color = data.AccentColor;
				this.radialFill.fillAmount = 1f;
			}
			this.RefreshLabel();
			LocalizedGameText.BindFontOnly(this.timerText);
			this.RefreshTimerText(true);
			this.ApplyPresentationAlpha();
			base.gameObject.SetActive(true);
		}

		// Token: 0x06005672 RID: 22130 RVA: 0x002198E6 File Offset: 0x00217AE6
		public void SetRemainingTime(float seconds)
		{
			this.remainingTime = Mathf.Clamp(seconds, 0f, this.totalDuration);
		}

		// Token: 0x06005673 RID: 22131 RVA: 0x002198FF File Offset: 0x00217AFF
		public void SetNormalizedProgress(float normalized01)
		{
			this.remainingTime = ((normalized01 <= 0f) ? 0f : (Mathf.Clamp01(normalized01) * this.totalDuration));
		}

		// Token: 0x06005674 RID: 22132 RVA: 0x00219923 File Offset: 0x00217B23
		public void SetPaused(bool paused)
		{
			if (this.isPaused == paused)
			{
				return;
			}
			this.isPaused = paused;
			this.RefreshLabel();
		}

		// Token: 0x06005675 RID: 22133 RVA: 0x0021993C File Offset: 0x00217B3C
		public void BeginFadeOut()
		{
			if (this.isFadingOut)
			{
				return;
			}
			this.isFadingOut = true;
			this.fadeProgress = 0f;
		}

		// Token: 0x06005676 RID: 22134 RVA: 0x0021995C File Offset: 0x00217B5C
		public bool Tick(float deltaTime)
		{
			if (!this.isFadingOut && this.fadeAlpha < 1f)
			{
				this.fadeProgress += deltaTime;
				this.fadeAlpha = Mathf.Clamp01(this.fadeProgress / Mathf.Max(0.01f, this.fadeInDuration));
				this.ApplyPresentationAlpha();
			}
			if (!this.isFadingOut)
			{
				if (this.remainingTime > 0f && !float.IsPositiveInfinity(this.remainingTime) && !this.externalTimeControl && !this.isPaused)
				{
					this.remainingTime = Mathf.Max(0f, this.remainingTime - deltaTime);
				}
				float normalizedProgress = this.NormalizedProgress;
				if (this.radialFill != null)
				{
					this.radialFill.fillAmount = normalizedProgress;
				}
				this.RefreshTimerText(false);
				if (this.pulseThreshold > 0f && normalizedProgress <= this.pulseThreshold && normalizedProgress > 0f)
				{
					float num = Mathf.Lerp(this.pulseMinAlpha, 1f, (Mathf.Sin(Time.unscaledTime * this.pulseSpeed * 3.1415927f * 2f) + 1f) * 0.5f);
					if (this.iconImage != null)
					{
						Color color = this.iconImage.color;
						color.a = num;
						this.iconImage.color = color;
					}
				}
				else if (this.iconImage != null)
				{
					Color color2 = this.iconImage.color;
					color2.a = 1f;
					this.iconImage.color = color2;
				}
				if (this.remainingTime <= 0f && !this.isFadingOut)
				{
					this.BeginFadeOut();
				}
				return true;
			}
			this.fadeProgress += deltaTime;
			float num2 = Mathf.Clamp01(this.fadeProgress / Mathf.Max(0.01f, this.fadeOutDuration));
			this.fadeAlpha = 1f - num2;
			this.ApplyPresentationAlpha();
			if (num2 >= 1f)
			{
				this.isActive = false;
				base.gameObject.SetActive(false);
				return false;
			}
			return true;
		}

		// Token: 0x06005677 RID: 22135 RVA: 0x00219B5C File Offset: 0x00217D5C
		public void ResetEntry()
		{
			this.effectId = null;
			this.totalDuration = 0f;
			this.remainingTime = 0f;
			this.isActive = false;
			this.isFadingOut = false;
			this.externalTimeControl = false;
			this.isPaused = false;
			this.displayName = null;
			this.fadeProgress = 0f;
			this.fadeAlpha = 0f;
			this.presentationVisible = false;
			this.ApplyPresentationAlpha();
			this.hasRenderedTimer = false;
			base.gameObject.SetActive(false);
		}

		// Token: 0x06005678 RID: 22136 RVA: 0x00219BDF File Offset: 0x00217DDF
		private void RefreshLabel()
		{
			if (this.labelText == null)
			{
				return;
			}
			LocalizedGameText localizedGameText = LocalizedGameText.BindComputed(this.labelText, new Func<string>(this.ResolveLabel));
			if (localizedGameText == null)
			{
				return;
			}
			localizedGameText.FitTranslationToRectangle(false);
		}

		// Token: 0x06005679 RID: 22137 RVA: 0x00219C14 File Offset: 0x00217E14
		private string ResolveLabel()
		{
			string text = this.ResolveDisplayName();
			if (!this.isPaused || string.IsNullOrEmpty(this.displayName))
			{
				return text;
			}
			return GameLocalization.FormatSource("timed_effects", "{0} - PAUSED", new object[] { text });
		}

		// Token: 0x0600567A RID: 22138 RVA: 0x00219C58 File Offset: 0x00217E58
		private string ResolveDisplayName()
		{
			string text = this.effectId;
			if (text == "MudBodyPaint" || text == "TintBodyPaint" || text == "ShowerBubbles" || text == "GymTimeFreeze" || text == "CustomBodyPaint" || text == "PencilBodyPaint")
			{
				return GameLocalization.GetSource("timed_effects", this.displayName ?? "");
			}
			return this.displayName ?? "";
		}

		// Token: 0x0600567B RID: 22139 RVA: 0x00219CE4 File Offset: 0x00217EE4
		private void RefreshTimerText(bool force = false)
		{
			if (this.timerText == null)
			{
				return;
			}
			if (float.IsPositiveInfinity(this.remainingTime))
			{
				if (force || this.timerText.text != "∞")
				{
					this.timerText.text = "∞";
				}
				this.hasRenderedTimer = false;
				return;
			}
			bool flag = this.remainingTime >= 60f;
			int num = (flag ? Mathf.FloorToInt(this.remainingTime / 60f) : 0);
			int num2 = ((this.remainingTime <= 0f) ? 0 : (flag ? Mathf.CeilToInt(this.remainingTime % 60f) : Mathf.CeilToInt(this.remainingTime)));
			if (!force && this.hasRenderedTimer && this.renderedTimerUsesMinutes == flag && this.renderedTimerMinutes == num && this.renderedTimerSeconds == num2)
			{
				return;
			}
			this.hasRenderedTimer = true;
			this.renderedTimerUsesMinutes = flag;
			this.renderedTimerMinutes = num;
			this.renderedTimerSeconds = num2;
			this.timerText.text = TimedEffectHUDEntry.FormatTime(this.remainingTime);
		}

		// Token: 0x0600567C RID: 22140 RVA: 0x00219DF4 File Offset: 0x00217FF4
		private static string FormatTime(float seconds)
		{
			if (float.IsPositiveInfinity(seconds))
			{
				return "∞";
			}
			if (seconds <= 0f)
			{
				return "0";
			}
			if (seconds < 60f)
			{
				return string.Format("{0}", Mathf.CeilToInt(seconds));
			}
			int num = Mathf.FloorToInt(seconds / 60f);
			int num2 = Mathf.CeilToInt(seconds % 60f);
			return string.Format("{0}:{1:D2}", num, num2);
		}

		// Token: 0x040056E9 RID: 22249
		[Header("UI References")]
		[Tooltip("Filled image using Image.Type = Filled (Radial360) for the countdown ring")]
		[SerializeField]
		private Image radialFill;

		// Token: 0x040056EA RID: 22250
		[Tooltip("Background ring behind the radial fill (static, shows empty state)")]
		[SerializeField]
		private Image radialBackground;

		// Token: 0x040056EB RID: 22251
		[Tooltip("Icon displayed at the center of the radial timer")]
		[SerializeField]
		private Image iconImage;

		// Token: 0x040056EC RID: 22252
		[Tooltip("Optional text showing remaining seconds")]
		[SerializeField]
		private TextMeshProUGUI timerText;

		// Token: 0x040056ED RID: 22253
		[Tooltip("Optional label showing the effect display name")]
		[SerializeField]
		private TextMeshProUGUI labelText;

		// Token: 0x040056EE RID: 22254
		[Header("Animation")]
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x040056EF RID: 22255
		[SerializeField]
		private float fadeInDuration = 0.2f;

		// Token: 0x040056F0 RID: 22256
		[SerializeField]
		private float fadeOutDuration = 0.2f;

		// Token: 0x040056F1 RID: 22257
		[Header("Pulse Warning")]
		[Tooltip("Normalized time remaining at which the icon starts pulsing (0 = never)")]
		[SerializeField]
		[Range(0f, 0.5f)]
		private float pulseThreshold = 0.15f;

		// Token: 0x040056F2 RID: 22258
		[SerializeField]
		private float pulseSpeed = 4f;

		// Token: 0x040056F3 RID: 22259
		[SerializeField]
		private float pulseMinAlpha = 0.4f;

		// Token: 0x040056F4 RID: 22260
		private string effectId;

		// Token: 0x040056F5 RID: 22261
		private float totalDuration;

		// Token: 0x040056F6 RID: 22262
		private float remainingTime;

		// Token: 0x040056F7 RID: 22263
		private bool isActive;

		// Token: 0x040056F8 RID: 22264
		private bool isFadingOut;

		// Token: 0x040056F9 RID: 22265
		private bool externalTimeControl;

		// Token: 0x040056FA RID: 22266
		private bool isPaused;

		// Token: 0x040056FB RID: 22267
		private string displayName;

		// Token: 0x040056FC RID: 22268
		private float fadeProgress;

		// Token: 0x040056FD RID: 22269
		private float fadeAlpha;

		// Token: 0x040056FE RID: 22270
		private bool presentationVisible = true;

		// Token: 0x040056FF RID: 22271
		private bool hasRenderedTimer;

		// Token: 0x04005700 RID: 22272
		private bool renderedTimerUsesMinutes;

		// Token: 0x04005701 RID: 22273
		private int renderedTimerMinutes;

		// Token: 0x04005702 RID: 22274
		private int renderedTimerSeconds;
	}
}
