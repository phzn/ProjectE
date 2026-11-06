using System;
using System.Collections;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000164 RID: 356
	[RequireComponent(typeof(CanvasGroup))]
	[AddComponentMenu("More Mountains/Tools/GUI/MMFaderDirectional")]
	public class MMFaderDirectional : MMMonoBehaviour, MMEventListener<MMFadeEvent>, MMEventListenerBase, MMEventListener<MMFadeInEvent>, MMEventListener<MMFadeOutEvent>, MMEventListener<MMFadeStopEvent>
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x00026C54 File Offset: 0x00024E54
		public virtual float Width
		{
			get
			{
				return this._rectTransform.rect.width;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600083E RID: 2110 RVA: 0x00026C74 File Offset: 0x00024E74
		public virtual float Height
		{
			get
			{
				return this._rectTransform.rect.height;
			}
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x00026C94 File Offset: 0x00024E94
		protected virtual void ResetFader()
		{
			this._rectTransform.anchoredPosition = this._initialPosition;
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x00026CA7 File Offset: 0x00024EA7
		protected virtual void DefaultFade()
		{
			MMFadeEvent.Trigger(this.DefaultDuration, 1f, this.DefaultTween, this.ID, this.IgnoreTimescale, base.transform.position);
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x00026CD6 File Offset: 0x00024ED6
		protected virtual void FadeIn1Second()
		{
			MMFadeInEvent.Trigger(1f, this.DefaultTween, this.ID, this.IgnoreTimescale, base.transform.position);
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x00026CFF File Offset: 0x00024EFF
		protected virtual void FadeOut1Second()
		{
			MMFadeOutEvent.Trigger(1f, this.DefaultTween, this.ID, this.IgnoreTimescale, base.transform.position);
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x00026D28 File Offset: 0x00024F28
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x00026D30 File Offset: 0x00024F30
		protected virtual void Initialization()
		{
			this._canvasGroup = base.gameObject.GetComponent<CanvasGroup>();
			this._rectTransform = base.gameObject.GetComponent<RectTransform>();
			this._initialPosition = this._rectTransform.anchoredPosition;
			if (this.DisableOnInit)
			{
				this.DisableFader();
			}
			this._initialized = true;
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x00026D85 File Offset: 0x00024F85
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

		// Token: 0x06000846 RID: 2118 RVA: 0x00026DA4 File Offset: 0x00024FA4
		protected virtual void Fade()
		{
			float num = (this.IgnoreTimescale ? Time.unscaledTime : Time.time);
			float num2 = this._fadeStartedAt + this._currentDuration;
			if (num - this._fadeStartedAt < this._currentDuration)
			{
				this._newPosition = MMTween.Tween(num, this._fadeStartedAt, num2, this._fromPosition, this._toPosition, this._currentCurve);
				this._rectTransform.anchoredPosition = this._newPosition;
				return;
			}
			this.StopFading();
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x00026E21 File Offset: 0x00025021
		protected virtual void StopFading()
		{
			this._rectTransform.anchoredPosition = this._toPosition;
			this._fading = false;
			if (this._initialPosition != this._toPosition)
			{
				this.DisableFader();
			}
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x00026E54 File Offset: 0x00025054
		protected virtual IEnumerator StartFading(bool fadingIn, float duration, MMTweenType curve, int id, bool ignoreTimeScale, Vector3 worldPosition)
		{
			if (id != this.ID)
			{
				yield break;
			}
			if (this.InitialDelay > 0f)
			{
				yield return MMCoroutine.WaitFor(this.InitialDelay);
			}
			if (!this._initialized)
			{
				this.Initialization();
			}
			if (curve == null)
			{
				curve = this.DefaultTween;
			}
			this.IgnoreTimescale = ignoreTimeScale;
			this.EnableFader();
			this._fading = true;
			this._fadeStartedAt = (this.IgnoreTimescale ? Time.unscaledTime : Time.time);
			this._currentCurve = curve;
			this._currentDuration = duration;
			this._fromPosition = this._rectTransform.anchoredPosition;
			this._toPosition = (fadingIn ? this._initialPosition : this.ExitPosition());
			this._newPosition = MMTween.Tween(0f, 0f, duration, this._fromPosition, this._toPosition, this._currentCurve);
			this._rectTransform.anchoredPosition = this._newPosition;
			yield break;
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x00026E88 File Offset: 0x00025088
		protected virtual Vector2 BeforeEntryPosition()
		{
			switch (this.FadeInDirection)
			{
			case MMFaderDirectional.Directions.TopToBottom:
				return this._initialPosition + Vector2.up * this.Height;
			case MMFaderDirectional.Directions.LeftToRight:
				return this._initialPosition + Vector2.left * this.Width;
			case MMFaderDirectional.Directions.RightToLeft:
				return this._initialPosition + Vector2.right * this.Width;
			case MMFaderDirectional.Directions.BottomToTop:
				return this._initialPosition + Vector2.down * this.Height;
			default:
				return Vector2.zero;
			}
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x00026F2C File Offset: 0x0002512C
		protected virtual Vector2 ExitPosition()
		{
			switch (this.FadeOutDirection)
			{
			case MMFaderDirectional.Directions.TopToBottom:
				return this._initialPosition + Vector2.down * this.Height;
			case MMFaderDirectional.Directions.LeftToRight:
				return this._initialPosition + Vector2.right * this.Width;
			case MMFaderDirectional.Directions.RightToLeft:
				return this._initialPosition + Vector2.left * this.Width;
			case MMFaderDirectional.Directions.BottomToTop:
				return this._initialPosition + Vector2.up * this.Height;
			default:
				return Vector2.zero;
			}
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x00026FD0 File Offset: 0x000251D0
		protected virtual void DisableFader()
		{
			if (this.ShouldBlockRaycasts)
			{
				this._canvasGroup.blocksRaycasts = false;
			}
			this._active = false;
			this._canvasGroup.alpha = 0f;
			this._rectTransform.anchoredPosition = this.BeforeEntryPosition();
			base.enabled = false;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x00027020 File Offset: 0x00025220
		protected virtual void EnableFader()
		{
			base.enabled = true;
			if (this.ShouldBlockRaycasts)
			{
				this._canvasGroup.blocksRaycasts = true;
			}
			this._active = true;
			this._canvasGroup.alpha = 1f;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x00027054 File Offset: 0x00025254
		public virtual void OnMMEvent(MMFadeEvent fadeEvent)
		{
			bool flag = !this._active;
			base.StartCoroutine(this.StartFading(flag, fadeEvent.Duration, fadeEvent.Curve, fadeEvent.ID, fadeEvent.IgnoreTimeScale, fadeEvent.WorldPosition));
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x0002709A File Offset: 0x0002529A
		public virtual void OnMMEvent(MMFadeInEvent fadeEvent)
		{
			base.StartCoroutine(this.StartFading(true, fadeEvent.Duration, fadeEvent.Curve, fadeEvent.ID, fadeEvent.IgnoreTimeScale, fadeEvent.WorldPosition));
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x000270C8 File Offset: 0x000252C8
		public virtual void OnMMEvent(MMFadeOutEvent fadeEvent)
		{
			base.StartCoroutine(this.StartFading(false, fadeEvent.Duration, fadeEvent.Curve, fadeEvent.ID, fadeEvent.IgnoreTimeScale, fadeEvent.WorldPosition));
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x000270F6 File Offset: 0x000252F6
		public virtual void OnMMEvent(MMFadeStopEvent fadeStopEvent)
		{
			if (fadeStopEvent.ID == this.ID)
			{
				this._fading = false;
				if (fadeStopEvent.Restore)
				{
					this._rectTransform.anchoredPosition = this._initialPosition;
				}
			}
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x00027126 File Offset: 0x00025326
		protected virtual void OnEnable()
		{
			this.MMEventStartListening<MMFadeEvent>();
			this.MMEventStartListening<MMFadeStopEvent>();
			this.MMEventStartListening<MMFadeInEvent>();
			this.MMEventStartListening<MMFadeOutEvent>();
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x00027140 File Offset: 0x00025340
		protected virtual void OnDestroy()
		{
			this.MMEventStopListening<MMFadeEvent>();
			this.MMEventStopListening<MMFadeStopEvent>();
			this.MMEventStopListening<MMFadeInEvent>();
			this.MMEventStopListening<MMFadeOutEvent>();
		}

		// Token: 0x040009AA RID: 2474
		[MMInspectorGroup("Identification", true, 122, false)]
		[Tooltip("the ID for this fader (0 is default), set more IDs if you need more than one fader")]
		public int ID;

		// Token: 0x040009AB RID: 2475
		[MMInspectorGroup("Directional Fader", true, 123, false)]
		[Tooltip("the direction this fader should move in when fading in")]
		public MMFaderDirectional.Directions FadeInDirection = MMFaderDirectional.Directions.LeftToRight;

		// Token: 0x040009AC RID: 2476
		[Tooltip("the direction this fader should move in when fading out")]
		public MMFaderDirectional.Directions FadeOutDirection = MMFaderDirectional.Directions.LeftToRight;

		// Token: 0x040009AD RID: 2477
		[MMInspectorGroup("Timing", true, 124, false)]
		[Tooltip("the default duration of the fade in/out")]
		public float DefaultDuration = 0.2f;

		// Token: 0x040009AE RID: 2478
		[Tooltip("the default curve to use for this fader")]
		public MMTweenType DefaultTween = new MMTweenType(MMTween.MMTweenCurve.LinearTween);

		// Token: 0x040009AF RID: 2479
		[Tooltip("whether or not the fade should happen in unscaled time")]
		public bool IgnoreTimescale = true;

		// Token: 0x040009B0 RID: 2480
		[Tooltip("whether or not to automatically disable this fader on init")]
		public bool DisableOnInit = true;

		// Token: 0x040009B1 RID: 2481
		[MMInspectorGroup("Delay", true, 127, false)]
		[Tooltip("a delay (in seconds) to apply before playing this fade")]
		public float InitialDelay;

		// Token: 0x040009B2 RID: 2482
		[MMInspectorGroup("Interaction", true, 125, false)]
		[Tooltip("whether or not the fader should block raycasts when visible")]
		public bool ShouldBlockRaycasts;

		// Token: 0x040009B3 RID: 2483
		[MMInspectorGroup("Debug", true, 126, false)]
		[MMInspectorButtonBar(new string[] { "FadeIn1Second", "FadeOut1Second", "DefaultFade", "ResetFader" }, new string[] { "FadeIn1Second", "FadeOut1Second", "DefaultFade", "ResetFader" }, new bool[] { true, true, true, true }, new string[] { "main-call-to-action", "", "", "" })]
		public bool DebugToolbar;

		// Token: 0x040009B4 RID: 2484
		protected RectTransform _rectTransform;

		// Token: 0x040009B5 RID: 2485
		protected CanvasGroup _canvasGroup;

		// Token: 0x040009B6 RID: 2486
		protected float _currentDuration;

		// Token: 0x040009B7 RID: 2487
		protected MMTweenType _currentCurve;

		// Token: 0x040009B8 RID: 2488
		protected bool _fading;

		// Token: 0x040009B9 RID: 2489
		protected float _fadeStartedAt;

		// Token: 0x040009BA RID: 2490
		protected Vector2 _initialPosition;

		// Token: 0x040009BB RID: 2491
		protected Vector2 _fromPosition;

		// Token: 0x040009BC RID: 2492
		protected Vector2 _toPosition;

		// Token: 0x040009BD RID: 2493
		protected Vector2 _newPosition;

		// Token: 0x040009BE RID: 2494
		protected bool _active;

		// Token: 0x040009BF RID: 2495
		protected bool _initialized;

		// Token: 0x02000465 RID: 1125
		public enum Directions
		{
			// Token: 0x04002090 RID: 8336
			TopToBottom,
			// Token: 0x04002091 RID: 8337
			LeftToRight,
			// Token: 0x04002092 RID: 8338
			RightToLeft,
			// Token: 0x04002093 RID: 8339
			BottomToTop
		}
	}
}
