using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000165 RID: 357
	[RequireComponent(typeof(CanvasGroup))]
	[AddComponentMenu("More Mountains/Tools/GUI/MMFaderRound")]
	public class MMFaderRound : MMMonoBehaviour, MMEventListener<MMFadeEvent>, MMEventListenerBase, MMEventListener<MMFadeInEvent>, MMEventListener<MMFadeOutEvent>, MMEventListener<MMFadeStopEvent>
	{
		// Token: 0x06000854 RID: 2132 RVA: 0x00027195 File Offset: 0x00025395
		protected virtual void ResetFader()
		{
			this.FaderMask.transform.localScale = this.MaskScale.x * Vector3.one;
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x000271BC File Offset: 0x000253BC
		protected virtual void DefaultFade()
		{
			MMFadeEvent.Trigger(this.DefaultDuration, this.MaskScale.y, this.DefaultTween, this.ID, this.IgnoreTimescale, this.DebugWorldPositionTarget.transform.position);
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x000271F6 File Offset: 0x000253F6
		protected virtual void FadeIn1Second()
		{
			MMFadeInEvent.Trigger(1f, this.DefaultTween, this.ID, this.IgnoreTimescale, this.DebugWorldPositionTarget.transform.position);
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x00027224 File Offset: 0x00025424
		protected virtual void FadeOut1Second()
		{
			MMFadeOutEvent.Trigger(1f, this.DefaultTween, this.ID, this.IgnoreTimescale, this.DebugWorldPositionTarget.transform.position);
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x00027252 File Offset: 0x00025452
		protected virtual void Awake()
		{
			this.Initialization();
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0002725C File Offset: 0x0002545C
		protected virtual void Initialization()
		{
			if (this.CameraMode == MMFaderRound.CameraModes.Main)
			{
				this.TargetCamera = Camera.main;
			}
			this._canvasGroup = base.GetComponent<CanvasGroup>();
			this.FaderMask.transform.localScale = this.MaskScale.x * Vector3.one;
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x000272AD File Offset: 0x000254AD
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

		// Token: 0x0600085B RID: 2139 RVA: 0x000272CC File Offset: 0x000254CC
		protected virtual void Fade()
		{
			float num = (this.IgnoreTimescale ? Time.unscaledTime : Time.time);
			float num2 = this._fadeStartedAt + this._currentDuration;
			if (num - this._fadeStartedAt < this._currentDuration)
			{
				float num3 = MMTween.Tween(num, this._fadeStartedAt, num2, this._initialScale, this._currentTargetScale, this._currentCurve);
				this.FaderMask.transform.localScale = num3 * Vector3.one;
				return;
			}
			this.StopFading();
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x00027350 File Offset: 0x00025550
		protected virtual void StopFading()
		{
			this.FaderMask.transform.localScale = this._currentTargetScale * Vector3.one;
			this._fading = false;
			if (this.FaderMask.transform.localScale == this.MaskScale.y * Vector3.one)
			{
				this.DisableFader();
			}
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x000273B6 File Offset: 0x000255B6
		protected virtual void DisableFader()
		{
			if (this.ShouldBlockRaycasts)
			{
				this._canvasGroup.blocksRaycasts = false;
			}
			this._canvasGroup.alpha = 0f;
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x000273DC File Offset: 0x000255DC
		protected virtual void EnableFader()
		{
			if (this.ShouldBlockRaycasts)
			{
				this._canvasGroup.blocksRaycasts = true;
			}
			this._canvasGroup.alpha = 1f;
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x00027404 File Offset: 0x00025604
		protected virtual void StartFading(float initialAlpha, float endAlpha, float duration, MMTweenType curve, int id, bool ignoreTimeScale, Vector3 worldPosition)
		{
			if (id != this.ID)
			{
				return;
			}
			if (this.TargetCamera == null)
			{
				Debug.LogWarning(base.name + " : You're using a fader round but its TargetCamera hasn't been setup in its inspector. It can't fade.");
				return;
			}
			this.FaderMask.anchoredPosition = Vector3.zero;
			Vector3 vector = this.TargetCamera.WorldToViewportPoint(worldPosition);
			vector.x = Mathf.Clamp01(vector.x);
			vector.y = Mathf.Clamp01(vector.y);
			vector.z = Mathf.Clamp01(vector.z);
			this.FaderMask.anchorMin = vector;
			this.FaderMask.anchorMax = vector;
			this.IgnoreTimescale = ignoreTimeScale;
			this.EnableFader();
			this._fading = true;
			this._initialScale = initialAlpha;
			this._currentTargetScale = endAlpha;
			this._fadeStartedAt = (this.IgnoreTimescale ? Time.unscaledTime : Time.time);
			this._currentCurve = curve;
			this._currentDuration = duration;
			float num = MMTween.Tween(0f, 0f, duration, this._initialScale, this._currentTargetScale, this._currentCurve);
			this.FaderMask.transform.localScale = num * Vector3.one;
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x00027548 File Offset: 0x00025748
		public virtual void OnMMEvent(MMFadeEvent fadeEvent)
		{
			this._currentTargetScale = ((fadeEvent.TargetAlpha == -1f) ? this.MaskScale.y : fadeEvent.TargetAlpha);
			this.StartFading(this.FaderMask.transform.localScale.x, this._currentTargetScale, fadeEvent.Duration, fadeEvent.Curve, fadeEvent.ID, fadeEvent.IgnoreTimeScale, fadeEvent.WorldPosition);
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x000275BC File Offset: 0x000257BC
		public virtual void OnMMEvent(MMFadeInEvent fadeEvent)
		{
			if (fadeEvent.Duration > 0f)
			{
				this.StartFading(this.MaskScale.y, this.MaskScale.x, fadeEvent.Duration, fadeEvent.Curve, fadeEvent.ID, fadeEvent.IgnoreTimeScale, fadeEvent.WorldPosition);
				return;
			}
			this.FaderMask.transform.localScale = this.MaskScale.x * Vector3.one;
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x00027638 File Offset: 0x00025838
		public virtual void OnMMEvent(MMFadeOutEvent fadeEvent)
		{
			if (fadeEvent.Duration > 0f)
			{
				this.StartFading(this.MaskScale.x, this.MaskScale.y, fadeEvent.Duration, fadeEvent.Curve, fadeEvent.ID, fadeEvent.IgnoreTimeScale, fadeEvent.WorldPosition);
				return;
			}
			this.FaderMask.transform.localScale = this.MaskScale.y * Vector3.one;
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x000276B2 File Offset: 0x000258B2
		public virtual void OnMMEvent(MMFadeStopEvent fadeStopEvent)
		{
			if (fadeStopEvent.ID == this.ID)
			{
				this._fading = false;
				if (fadeStopEvent.Restore)
				{
					this.FaderMask.transform.localScale = this._initialScale * Vector3.one;
				}
			}
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x000276F1 File Offset: 0x000258F1
		protected virtual void OnEnable()
		{
			this.MMEventStartListening<MMFadeEvent>();
			this.MMEventStartListening<MMFadeStopEvent>();
			this.MMEventStartListening<MMFadeInEvent>();
			this.MMEventStartListening<MMFadeOutEvent>();
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0002770B File Offset: 0x0002590B
		protected virtual void OnDisable()
		{
			this.MMEventStopListening<MMFadeEvent>();
			this.MMEventStopListening<MMFadeStopEvent>();
			this.MMEventStopListening<MMFadeInEvent>();
			this.MMEventStopListening<MMFadeOutEvent>();
		}

		// Token: 0x040009C0 RID: 2496
		[MMInspectorGroup("Bindings", true, 121, false)]
		public MMFaderRound.CameraModes CameraMode;

		// Token: 0x040009C1 RID: 2497
		[MMEnumCondition("CameraMode", new int[] { 1 })]
		public Camera TargetCamera;

		// Token: 0x040009C2 RID: 2498
		public RectTransform FaderBackground;

		// Token: 0x040009C3 RID: 2499
		public RectTransform FaderMask;

		// Token: 0x040009C4 RID: 2500
		[MMInspectorGroup("Identification", true, 122, false)]
		public int ID;

		// Token: 0x040009C5 RID: 2501
		[MMInspectorGroup("Mask", true, 127, false)]
		[MMVector(new string[] { "min", "max" })]
		public Vector2 MaskScale;

		// Token: 0x040009C6 RID: 2502
		[MMInspectorGroup("Timing", true, 124, false)]
		public float DefaultDuration = 0.2f;

		// Token: 0x040009C7 RID: 2503
		public MMTweenType DefaultTween = new MMTweenType(MMTween.MMTweenCurve.LinearTween);

		// Token: 0x040009C8 RID: 2504
		public bool IgnoreTimescale = true;

		// Token: 0x040009C9 RID: 2505
		[MMInspectorGroup("Interaction", true, 125, false)]
		public bool ShouldBlockRaycasts;

		// Token: 0x040009CA RID: 2506
		[MMInspectorGroup("Debug", true, 126, false)]
		public Transform DebugWorldPositionTarget;

		// Token: 0x040009CB RID: 2507
		[MMInspectorButtonBar(new string[] { "FadeIn1Second", "FadeOut1Second", "DefaultFade", "ResetFader" }, new string[] { "FadeIn1Second", "FadeOut1Second", "DefaultFade", "ResetFader" }, new bool[] { true, true, true, true }, new string[] { "main-call-to-action", "", "", "" })]
		public bool DebugToolbar;

		// Token: 0x040009CC RID: 2508
		protected CanvasGroup _canvasGroup;

		// Token: 0x040009CD RID: 2509
		protected float _initialScale;

		// Token: 0x040009CE RID: 2510
		protected float _currentTargetScale;

		// Token: 0x040009CF RID: 2511
		protected float _currentDuration;

		// Token: 0x040009D0 RID: 2512
		protected MMTweenType _currentCurve;

		// Token: 0x040009D1 RID: 2513
		protected bool _fading;

		// Token: 0x040009D2 RID: 2514
		protected float _fadeStartedAt;

		// Token: 0x02000467 RID: 1127
		public enum CameraModes
		{
			// Token: 0x0400209D RID: 8349
			Main,
			// Token: 0x0400209E RID: 8350
			Override
		}
	}
}
