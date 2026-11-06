using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200029E RID: 670
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you modify the fill value of a target Image over time.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("UI/Image Fill")]
	public class MMF_ImageFill : MMF_Feedback
	{
		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060012DB RID: 4827 RVA: 0x000513D5 File Offset: 0x0004F5D5
		public override bool HasCustomInspectors
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060012DC RID: 4828 RVA: 0x000513D8 File Offset: 0x0004F5D8
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x000513DB File Offset: 0x0004F5DB
		protected override void AutomateTargetAcquisition()
		{
			this.BoundImage = this.FindAutomatedTarget<Image>();
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060012DE RID: 4830 RVA: 0x000513E9 File Offset: 0x0004F5E9
		// (set) Token: 0x060012DF RID: 4831 RVA: 0x00051406 File Offset: 0x0004F606
		public override float FeedbackDuration
		{
			get
			{
				if (this.Mode != MMF_ImageFill.Modes.Instant)
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

		// Token: 0x060012E0 RID: 4832 RVA: 0x00051410 File Offset: 0x0004F610
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ImageFill.FeedbackTypeAuthorized)
			{
				return;
			}
			this._initialState = this.BoundImage.gameObject.activeInHierarchy;
			this.Turn(true);
			this._initialFill = this.BoundImage.fillAmount;
			switch (this.Mode)
			{
			case MMF_ImageFill.Modes.OverTime:
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
			case MMF_ImageFill.Modes.Instant:
				this.BoundImage.fillAmount = this.InstantFill;
				return;
			case MMF_ImageFill.Modes.ToDestination:
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

		// Token: 0x060012E1 RID: 4833 RVA: 0x0005150A File Offset: 0x0004F70A
		protected virtual IEnumerator ImageSequence()
		{
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			this._initialFill = this.BoundImage.fillAmount;
			this.IsPlaying = true;
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = MMFeedbacksHelpers.Remap(journey, 0f, this.FeedbackDuration, 0f, 1f);
				this.SetFill(num);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.SetFill(this.FinalNormalizedTime);
			this._coroutine = null;
			this.IsPlaying = false;
			yield return null;
			yield break;
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x0005151C File Offset: 0x0004F71C
		protected virtual void SetFill(float time)
		{
			float num = 0f;
			if (this.Mode == MMF_ImageFill.Modes.OverTime)
			{
				num = MMTween.Tween(time, 0f, 1f, this.CurveRemapZero, this.CurveRemapOne, this.Curve);
			}
			else if (this.Mode == MMF_ImageFill.Modes.ToDestination)
			{
				num = MMTween.Tween(time, 0f, 1f, this._initialFill, this.DestinationFill, this.Curve);
			}
			this.BoundImage.fillAmount = num;
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x00051594 File Offset: 0x0004F794
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ImageFill.FeedbackTypeAuthorized)
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

		// Token: 0x060012E4 RID: 4836 RVA: 0x000515F7 File Offset: 0x0004F7F7
		protected virtual void Turn(bool status)
		{
			this.BoundImage.gameObject.SetActive(status);
			this.BoundImage.enabled = status;
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x00051616 File Offset: 0x0004F816
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_ImageFill.FeedbackTypeAuthorized)
			{
				return;
			}
			this.Turn(this._initialState);
			this.BoundImage.fillAmount = this._initialFill;
		}

		// Token: 0x04001557 RID: 5463
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001558 RID: 5464
		[MMFInspectorGroup("Target Image", true, 12, true, false)]
		[Tooltip("the Image to affect when playing the feedback")]
		public Image BoundImage;

		// Token: 0x04001559 RID: 5465
		[MMFInspectorGroup("Image Fill Animation", true, 24, false, false)]
		[Tooltip("whether the feedback should affect the Image instantly or over a period of time")]
		public MMF_ImageFill.Modes Mode;

		// Token: 0x0400155A RID: 5466
		[Tooltip("how long the Image should change over time")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public float Duration = 0.2f;

		// Token: 0x0400155B RID: 5467
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x0400155C RID: 5468
		[Tooltip("the fill to move to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float InstantFill = 1f;

		// Token: 0x0400155D RID: 5469
		[Tooltip("the curve to use when interpolating towards the destination fill")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public MMTweenType Curve = new MMTweenType(MMTween.MMTweenCurve.EaseInCubic);

		// Token: 0x0400155E RID: 5470
		[Tooltip("the value to which the curve's 0 should be remapped")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float CurveRemapZero;

		// Token: 0x0400155F RID: 5471
		[Tooltip("the value to which the curve's 1 should be remapped")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float CurveRemapOne = 1f;

		// Token: 0x04001560 RID: 5472
		[Tooltip("the fill to aim towards when in ToDestination mode")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public float DestinationFill = 1f;

		// Token: 0x04001561 RID: 5473
		[Tooltip("if this is true, the target will be disabled when this feedbacks is stopped")]
		public bool DisableOnStop;

		// Token: 0x04001562 RID: 5474
		protected Coroutine _coroutine;

		// Token: 0x04001563 RID: 5475
		protected float _initialFill;

		// Token: 0x04001564 RID: 5476
		protected bool _initialState;

		// Token: 0x02000536 RID: 1334
		public enum Modes
		{
			// Token: 0x040023EB RID: 9195
			OverTime,
			// Token: 0x040023EC RID: 9196
			Instant,
			// Token: 0x040023ED RID: 9197
			ToDestination
		}
	}
}
