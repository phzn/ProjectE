using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200029D RID: 669
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the alpha of a target Image over time.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("UI/Image Alpha")]
	public class MMF_ImageAlpha : MMF_Feedback
	{
		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060012CE RID: 4814 RVA: 0x00051107 File Offset: 0x0004F307
		public override bool HasCustomInspectors
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x060012CF RID: 4815 RVA: 0x0005110A File Offset: 0x0004F30A
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x0005110D File Offset: 0x0004F30D
		protected override void AutomateTargetAcquisition()
		{
			this.BoundImage = this.FindAutomatedTarget<Image>();
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x060012D1 RID: 4817 RVA: 0x0005111B File Offset: 0x0004F31B
		// (set) Token: 0x060012D2 RID: 4818 RVA: 0x00051138 File Offset: 0x0004F338
		public override float FeedbackDuration
		{
			get
			{
				if (this.Mode != MMF_ImageAlpha.Modes.Instant)
				{
					return this.ApplyTimeMultiplier(this.Duration);
				}
				return 0f;
			}
			set
			{
				this.Duration = value;
			}
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x00051144 File Offset: 0x0004F344
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ImageAlpha.FeedbackTypeAuthorized)
			{
				return;
			}
			this._initialColor = this.BoundImage.color;
			this.Turn(true);
			switch (this.Mode)
			{
			case MMF_ImageAlpha.Modes.OverTime:
				if (!this.AllowAdditivePlays && this._coroutine != null)
				{
					return;
				}
				if (this._coroutine != null)
				{
					this.Owner.StopCoroutine(this._coroutine);
				}
				this._coroutine = this.Owner.StartCoroutine(this.ImageSequence());
				return;
			case MMF_ImageAlpha.Modes.Instant:
				this._imageColor = this.BoundImage.color;
				this._imageColor.a = this.InstantAlpha;
				this.BoundImage.color = this._imageColor;
				return;
			case MMF_ImageAlpha.Modes.ToDestination:
				if (!this.AllowAdditivePlays && this._coroutine != null)
				{
					return;
				}
				if (this._coroutine != null)
				{
					this.Owner.StopCoroutine(this._coroutine);
				}
				this._coroutine = this.Owner.StartCoroutine(this.ImageSequence());
				return;
			default:
				return;
			}
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x0005124A File Offset: 0x0004F44A
		protected virtual IEnumerator ImageSequence()
		{
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			this._imageColor = this.BoundImage.color;
			this._initialAlpha = this.BoundImage.color.a;
			this.IsPlaying = true;
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = MMFeedbacksHelpers.Remap(journey, 0f, this.FeedbackDuration, 0f, 1f);
				this.SetAlpha(num);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.SetAlpha(this.FinalNormalizedTime);
			this._coroutine = null;
			this.IsPlaying = false;
			yield return null;
			yield break;
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x0005125C File Offset: 0x0004F45C
		protected virtual void SetAlpha(float time)
		{
			float num = 0f;
			if (this.Mode == MMF_ImageAlpha.Modes.OverTime)
			{
				num = MMTween.Tween(time, 0f, 1f, this.CurveRemapZero, this.CurveRemapOne, this.Curve);
			}
			else if (this.Mode == MMF_ImageAlpha.Modes.ToDestination)
			{
				num = MMTween.Tween(time, 0f, 1f, this._initialAlpha, this.DestinationAlpha, this.Curve);
			}
			this._imageColor.a = num;
			this.BoundImage.color = this._imageColor;
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x000512E8 File Offset: 0x0004F4E8
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ImageAlpha.FeedbackTypeAuthorized)
			{
				return;
			}
			this.IsPlaying = false;
			base.CustomStopFeedback(position, feedbacksIntensity);
			if (this.Active && this.DisableOnStop)
			{
				this.Turn(false);
			}
			if (this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
			}
			this._coroutine = null;
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x0005134B File Offset: 0x0004F54B
		protected virtual void Turn(bool status)
		{
			this.BoundImage.gameObject.SetActive(status);
			this.BoundImage.enabled = status;
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x0005136A File Offset: 0x0004F56A
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_ImageAlpha.FeedbackTypeAuthorized)
			{
				return;
			}
			this.BoundImage.color = this._initialColor;
		}

		// Token: 0x04001548 RID: 5448
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001549 RID: 5449
		[MMFInspectorGroup("Target Image", true, 12, true, false)]
		[Tooltip("the Image to affect when playing the feedback")]
		public Image BoundImage;

		// Token: 0x0400154A RID: 5450
		[MMFInspectorGroup("Image Alpha Animation", true, 24, false, false)]
		[Tooltip("whether the feedback should affect the Image instantly or over a period of time")]
		public MMF_ImageAlpha.Modes Mode;

		// Token: 0x0400154B RID: 5451
		[Tooltip("how long the Image should change over time")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public float Duration = 0.2f;

		// Token: 0x0400154C RID: 5452
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x0400154D RID: 5453
		[Tooltip("the alpha to move to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float InstantAlpha = 1f;

		// Token: 0x0400154E RID: 5454
		[Tooltip("the curve to use when interpolating towards the destination alpha")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public MMTweenType Curve = new MMTweenType(MMTween.MMTweenCurve.EaseInCubic);

		// Token: 0x0400154F RID: 5455
		[Tooltip("the value to which the curve's 0 should be remapped")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float CurveRemapZero;

		// Token: 0x04001550 RID: 5456
		[Tooltip("the value to which the curve's 1 should be remapped")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float CurveRemapOne = 1f;

		// Token: 0x04001551 RID: 5457
		[Tooltip("the alpha to aim towards when in ToDestination mode")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public float DestinationAlpha = 1f;

		// Token: 0x04001552 RID: 5458
		[Tooltip("if this is true, the target will be disabled when this feedbacks is stopped")]
		public bool DisableOnStop;

		// Token: 0x04001553 RID: 5459
		protected Coroutine _coroutine;

		// Token: 0x04001554 RID: 5460
		protected Color _imageColor;

		// Token: 0x04001555 RID: 5461
		protected Color _initialColor;

		// Token: 0x04001556 RID: 5462
		protected float _initialAlpha;

		// Token: 0x02000534 RID: 1332
		public enum Modes
		{
			// Token: 0x040023E3 RID: 9187
			OverTime,
			// Token: 0x040023E4 RID: 9188
			Instant,
			// Token: 0x040023E5 RID: 9189
			ToDestination
		}
	}
}
