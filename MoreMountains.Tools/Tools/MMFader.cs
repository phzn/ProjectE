using System;
using UnityEngine;
using UnityEngine.UI;

namespace MoreMountains.Tools
{
	// Token: 0x02000163 RID: 355
	[RequireComponent(typeof(CanvasGroup))]
	[RequireComponent(typeof(Image))]
	[AddComponentMenu("More Mountains/Tools/GUI/MMFader")]
	public class MMFader : MMMonoBehaviour, MMEventListener<MMFadeEvent>, MMEventListenerBase, MMEventListener<MMFadeInEvent>, MMEventListener<MMFadeOutEvent>, MMEventListener<MMFadeStopEvent>
	{
		// Token: 0x06000827 RID: 2087 RVA: 0x000267E3 File Offset: 0x000249E3
		protected virtual void ResetFader()
		{
			this._canvasGroup.alpha = this.InactiveAlpha;
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x000267F8 File Offset: 0x000249F8
		protected virtual void DefaultFade()
		{
			MMFadeEvent.Trigger(this.DefaultDuration, this.ActiveAlpha, this.DefaultTween, this.ID, true, default(Vector3));
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x0002682C File Offset: 0x00024A2C
		protected virtual void FadeIn1Second()
		{
			MMFadeInEvent.Trigger(1f, new MMTweenType(MMTween.MMTweenCurve.LinearTween), 0, true, default(Vector3));
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x00026854 File Offset: 0x00024A54
		protected virtual void FadeOut1Second()
		{
			MMFadeOutEvent.Trigger(1f, new MMTweenType(MMTween.MMTweenCurve.LinearTween), 0, true, default(Vector3));
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0002687C File Offset: 0x00024A7C
		protected virtual void Awake()
		{
			this.Initialization();
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x00026884 File Offset: 0x00024A84
		protected virtual void Initialization()
		{
			this._canvasGroup = base.GetComponent<CanvasGroup>();
			this._image = base.GetComponent<Image>();
			if (this.ForcedInitState == MMFader.ForcedInitStates.Inactive)
			{
				this._canvasGroup.alpha = this.InactiveAlpha;
				this._image.enabled = false;
				return;
			}
			if (this.ForcedInitState == MMFader.ForcedInitStates.Active)
			{
				this._canvasGroup.alpha = this.ActiveAlpha;
				this._image.enabled = true;
			}
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x000268F6 File Offset: 0x00024AF6
		protected virtual void Update()
		{
			if (this._canvasGroup == null)
			{
				return;
			}
			if (this._fading)
			{
				this.Fade();
			}
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x00026918 File Offset: 0x00024B18
		protected virtual void Fade()
		{
			float num = (this.IgnoreTimescale ? Time.unscaledTime : Time.time);
			if (this._frameCountOne)
			{
				if (Time.frameCount <= 2)
				{
					this._canvasGroup.alpha = this._initialAlpha;
					return;
				}
				this._fadeStartedAt = (this.IgnoreTimescale ? Time.unscaledTime : Time.time);
				num = this._fadeStartedAt;
				this._frameCountOne = false;
			}
			float num2 = this._fadeStartedAt + this._currentDuration;
			if (num - this._fadeStartedAt < this._currentDuration)
			{
				float num3 = MMTween.Tween(num, this._fadeStartedAt, num2, this._initialAlpha, this._currentTargetAlpha, this._currentCurve);
				this._canvasGroup.alpha = num3;
				return;
			}
			this.StopFading();
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x000269D5 File Offset: 0x00024BD5
		protected virtual void StopFading()
		{
			this._canvasGroup.alpha = this._currentTargetAlpha;
			this._fading = false;
			if (this._canvasGroup.alpha == this.InactiveAlpha)
			{
				this.DisableFader();
			}
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x00026A08 File Offset: 0x00024C08
		protected virtual void DisableFader()
		{
			this._image.enabled = false;
			if (this.ShouldBlockRaycasts)
			{
				this._canvasGroup.blocksRaycasts = false;
			}
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x00026A2A File Offset: 0x00024C2A
		protected virtual void EnableFader()
		{
			this._image.enabled = true;
			if (this.ShouldBlockRaycasts)
			{
				this._canvasGroup.blocksRaycasts = true;
			}
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00026A4C File Offset: 0x00024C4C
		protected virtual void StartFading(float initialAlpha, float endAlpha, float duration, MMTweenType curve, bool ignoreTimeScale)
		{
			if (!this.CanFadeToCurrentAlpha && this._canvasGroup.alpha == endAlpha)
			{
				return;
			}
			this.IgnoreTimescale = ignoreTimeScale;
			this.EnableFader();
			this._fading = true;
			this._initialAlpha = initialAlpha;
			this._currentTargetAlpha = endAlpha;
			this._fadeStartedAt = (this.IgnoreTimescale ? Time.unscaledTime : Time.time);
			this._currentCurve = curve;
			this._currentDuration = duration;
			if (Time.frameCount == 1)
			{
				this._frameCountOne = true;
			}
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x00026ACB File Offset: 0x00024CCB
		public virtual void OnMMEvent(MMFadeEvent fadeEvent)
		{
			if (fadeEvent.ID != this.ID)
			{
				return;
			}
			this.Fade(fadeEvent.TargetAlpha, fadeEvent.Duration, fadeEvent.Curve, fadeEvent.IgnoreTimeScale);
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00026AFA File Offset: 0x00024CFA
		public virtual void OnMMEvent(MMFadeInEvent fadeEvent)
		{
			if (fadeEvent.ID != this.ID)
			{
				return;
			}
			this.FadeIn(fadeEvent.Duration, fadeEvent.Curve, fadeEvent.IgnoreTimeScale);
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00026B23 File Offset: 0x00024D23
		public virtual void OnMMEvent(MMFadeOutEvent fadeEvent)
		{
			if (fadeEvent.ID != this.ID)
			{
				return;
			}
			this.FadeOut(fadeEvent.Duration, fadeEvent.Curve, fadeEvent.IgnoreTimeScale);
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x00026B4C File Offset: 0x00024D4C
		public virtual void Fade(float targetAlpha, float duration, MMTweenType curve, bool ignoreTimeScale)
		{
			this._currentTargetAlpha = ((targetAlpha == -1f) ? this.ActiveAlpha : targetAlpha);
			this.StartFading(this._canvasGroup.alpha, this._currentTargetAlpha, duration, curve, ignoreTimeScale);
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x00026B80 File Offset: 0x00024D80
		public virtual void FadeIn(float duration, MMTweenType curve, bool ignoreTimeScale = true)
		{
			this.StartFading(this.InactiveAlpha, this.ActiveAlpha, duration, curve, ignoreTimeScale);
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00026B97 File Offset: 0x00024D97
		public virtual void FadeOut(float duration, MMTweenType curve, bool ignoreTimeScale = true)
		{
			this.StartFading(this.ActiveAlpha, this.InactiveAlpha, duration, curve, ignoreTimeScale);
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00026BAE File Offset: 0x00024DAE
		public virtual void OnMMEvent(MMFadeStopEvent fadeStopEvent)
		{
			if (fadeStopEvent.ID == this.ID)
			{
				this._fading = false;
				if (fadeStopEvent.Restore)
				{
					this._canvasGroup.alpha = this._initialAlpha;
				}
			}
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x00026BDE File Offset: 0x00024DDE
		protected virtual void OnEnable()
		{
			this.MMEventStartListening<MMFadeEvent>();
			this.MMEventStartListening<MMFadeStopEvent>();
			this.MMEventStartListening<MMFadeInEvent>();
			this.MMEventStartListening<MMFadeOutEvent>();
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x00026BF8 File Offset: 0x00024DF8
		protected virtual void OnDisable()
		{
			this.MMEventStopListening<MMFadeEvent>();
			this.MMEventStopListening<MMFadeStopEvent>();
			this.MMEventStopListening<MMFadeInEvent>();
			this.MMEventStopListening<MMFadeOutEvent>();
		}

		// Token: 0x04000997 RID: 2455
		[MMInspectorGroup("Identification", true, 122, false)]
		[Tooltip("the ID for this fader (0 is default), set more IDs if you need more than one fader")]
		public int ID;

		// Token: 0x04000998 RID: 2456
		[MMInspectorGroup("Opacity", true, 123, false)]
		[Tooltip("the opacity the fader should be at when inactive")]
		public float InactiveAlpha;

		// Token: 0x04000999 RID: 2457
		[Tooltip("the opacity the fader should be at when active")]
		public float ActiveAlpha = 1f;

		// Token: 0x0400099A RID: 2458
		[Tooltip("determines whether a state should be forced on init")]
		public MMFader.ForcedInitStates ForcedInitState = MMFader.ForcedInitStates.Inactive;

		// Token: 0x0400099B RID: 2459
		[MMInspectorGroup("Timing", true, 124, false)]
		[Tooltip("the default duration of the fade in/out")]
		public float DefaultDuration = 0.2f;

		// Token: 0x0400099C RID: 2460
		[Tooltip("the default curve to use for this fader")]
		public MMTweenType DefaultTween = new MMTweenType(MMTween.MMTweenCurve.LinearTween);

		// Token: 0x0400099D RID: 2461
		[Tooltip("whether or not the fade should happen in unscaled time")]
		public bool IgnoreTimescale = true;

		// Token: 0x0400099E RID: 2462
		[Tooltip("whether or not this fader can cause a fade if the requested final alpha is the same as the current one")]
		public bool CanFadeToCurrentAlpha = true;

		// Token: 0x0400099F RID: 2463
		[MMInspectorGroup("Interaction", true, 125, false)]
		[Tooltip("whether or not the fader should block raycasts when visible")]
		public bool ShouldBlockRaycasts;

		// Token: 0x040009A0 RID: 2464
		[MMInspectorGroup("Debug", true, 126, false)]
		[MMInspectorButtonBar(new string[] { "FadeIn1Second", "FadeOut1Second", "DefaultFade", "ResetFader" }, new string[] { "FadeIn1Second", "FadeOut1Second", "DefaultFade", "ResetFader" }, new bool[] { true, true, true, true }, new string[] { "main-call-to-action", "", "", "" })]
		public bool DebugToolbar;

		// Token: 0x040009A1 RID: 2465
		protected CanvasGroup _canvasGroup;

		// Token: 0x040009A2 RID: 2466
		protected Image _image;

		// Token: 0x040009A3 RID: 2467
		protected float _initialAlpha;

		// Token: 0x040009A4 RID: 2468
		protected float _currentTargetAlpha;

		// Token: 0x040009A5 RID: 2469
		protected float _currentDuration;

		// Token: 0x040009A6 RID: 2470
		protected MMTweenType _currentCurve;

		// Token: 0x040009A7 RID: 2471
		protected bool _fading;

		// Token: 0x040009A8 RID: 2472
		protected float _fadeStartedAt;

		// Token: 0x040009A9 RID: 2473
		protected bool _frameCountOne;

		// Token: 0x02000464 RID: 1124
		public enum ForcedInitStates
		{
			// Token: 0x0400208C RID: 8332
			None,
			// Token: 0x0400208D RID: 8333
			Active,
			// Token: 0x0400208E RID: 8334
			Inactive
		}
	}
}
