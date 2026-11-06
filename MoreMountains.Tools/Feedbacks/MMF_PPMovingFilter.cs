using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002AA RID: 682
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will trigger a post processing moving filter event, meant to be caught by a MMPostProcessingMovableFilter object")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("PostProcess/PPMovingFilter")]
	public class MMF_PPMovingFilter : MMF_Feedback
	{
		// Token: 0x17000208 RID: 520
		// (get) Token: 0x0600131D RID: 4893 RVA: 0x00052A34 File Offset: 0x00050C34
		// (set) Token: 0x0600131E RID: 4894 RVA: 0x00052A42 File Offset: 0x00050C42
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.TransitionDuration);
			}
			set
			{
				this.TransitionDuration = value;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x0600131F RID: 4895 RVA: 0x00052A4B File Offset: 0x00050C4B
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001320 RID: 4896 RVA: 0x00052A50 File Offset: 0x00050C50
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_PPMovingFilter.FeedbackTypeAuthorized)
			{
				return;
			}
			this._active = this.Mode == MMF_PPMovingFilter.Modes.On;
			this._toggle = this.Mode == MMF_PPMovingFilter.Modes.Toggle;
			MMPostProcessingMovingFilterEvent.Trigger(this.Curve, this._active, this._toggle, this.FeedbackDuration, this.Channel, false, false);
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x00052AB0 File Offset: 0x00050CB0
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_PPMovingFilter.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMPostProcessingMovingFilterEvent.Trigger(this.Curve, this._active, this._toggle, this.FeedbackDuration, 0, true, false);
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x00052AEA File Offset: 0x00050CEA
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_PPMovingFilter.FeedbackTypeAuthorized)
			{
				return;
			}
			MMPostProcessingMovingFilterEvent.Trigger(this.Curve, this._active, this._toggle, this.FeedbackDuration, 0, false, true);
		}

		// Token: 0x04001611 RID: 5649
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001612 RID: 5650
		[MMFInspectorGroup("PostProcessing Profile Moving Filter", true, 54, false, false)]
		[Tooltip("the selected mode for this feedback")]
		public MMF_PPMovingFilter.Modes Mode;

		// Token: 0x04001613 RID: 5651
		[Tooltip("the duration of the transition")]
		public float TransitionDuration = 1f;

		// Token: 0x04001614 RID: 5652
		[Tooltip("the curve to move along to")]
		public MMTweenType Curve = new MMTweenType(MMTween.MMTweenCurve.EaseInCubic);

		// Token: 0x04001615 RID: 5653
		protected bool _active;

		// Token: 0x04001616 RID: 5654
		protected bool _toggle;

		// Token: 0x0200053D RID: 1341
		public enum Modes
		{
			// Token: 0x04002412 RID: 9234
			Toggle,
			// Token: 0x04002413 RID: 9235
			On,
			// Token: 0x04002414 RID: 9236
			Off
		}
	}
}
