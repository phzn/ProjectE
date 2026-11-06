using System;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200029A RID: 666
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you trigger a one time play on a target FloatController.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("GameObject/FloatController")]
	public class MMF_FloatController : MMF_Feedback
	{
		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060012AB RID: 4779 RVA: 0x00050624 File Offset: 0x0004E824
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060012AC RID: 4780 RVA: 0x00050627 File Offset: 0x0004E827
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060012AD RID: 4781 RVA: 0x0005062A File Offset: 0x0004E82A
		public override bool ForceInitialValueDelayed
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060012AE RID: 4782 RVA: 0x0005062D File Offset: 0x0004E82D
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x00050630 File Offset: 0x0004E830
		protected override void AutomateTargetAcquisition()
		{
			this.TargetFloatController = this.FindAutomatedTarget<FloatController>();
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060012B0 RID: 4784 RVA: 0x0005063E File Offset: 0x0004E83E
		// (set) Token: 0x060012B1 RID: 4785 RVA: 0x00050661 File Offset: 0x0004E861
		public override float FeedbackDuration
		{
			get
			{
				if (this.Mode != MMF_FloatController.Modes.OneTime)
				{
					return this.ApplyTimeMultiplier(this.ToDestinationDuration);
				}
				return this.ApplyTimeMultiplier(this.OneTimeDuration);
			}
			set
			{
				this.OneTimeDuration = value;
				this.ToDestinationDuration = value;
			}
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x00050674 File Offset: 0x0004E874
		protected override void CustomInitialization(MMF_Player owner)
		{
			if (this.Active && this.TargetFloatController != null)
			{
				this._oneTimeDurationStorage = this.TargetFloatController.OneTimeDuration;
				this._oneTimeAmplitudeStorage = this.TargetFloatController.OneTimeAmplitude;
				this._oneTimeCurveStorage = this.TargetFloatController.OneTimeCurve;
				this._oneTimeRemapMinStorage = this.TargetFloatController.OneTimeRemapMin;
				this._oneTimeRemapMaxStorage = this.TargetFloatController.OneTimeRemapMax;
				this._toDestinationCurveStorage = this.TargetFloatController.ToDestinationCurve;
				this._toDestinationDurationStorage = this.TargetFloatController.ToDestinationDuration;
				this._toDestinationValueStorage = this.TargetFloatController.ToDestinationValue;
				this._revertToInitialValueAfterEndStorage = this.TargetFloatController.RevertToInitialValueAfterEnd;
			}
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x00050738 File Offset: 0x0004E938
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_FloatController.FeedbackTypeAuthorized || this.TargetFloatController == null)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			this.HandleFloatController(this.TargetFloatController, num);
			foreach (FloatController floatController in this.ExtraTargetFloatControllers)
			{
				this.HandleFloatController(floatController, num);
			}
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x000507C0 File Offset: 0x0004E9C0
		protected virtual void HandleFloatController(FloatController target, float intensityMultiplier)
		{
			target.RevertToInitialValueAfterEnd = this.RevertToInitialValueAfterEnd;
			if (this.Mode == MMF_FloatController.Modes.OneTime)
			{
				target.OneTimeDuration = this.FeedbackDuration;
				target.OneTimeAmplitude = this.OneTimeAmplitude;
				target.OneTimeCurve = this.OneTimeCurve;
				if (this.NormalPlayDirection)
				{
					target.OneTimeRemapMin = this.OneTimeRemapMin * intensityMultiplier;
					target.OneTimeRemapMax = this.OneTimeRemapMax * intensityMultiplier;
				}
				else
				{
					target.OneTimeRemapMin = this.OneTimeRemapMax * intensityMultiplier;
					target.OneTimeRemapMax = this.OneTimeRemapMin * intensityMultiplier;
				}
				target.OneTime();
			}
			if (this.Mode == MMF_FloatController.Modes.ToDestination)
			{
				target.ToDestinationCurve = this.ToDestinationCurve;
				target.ToDestinationDuration = this.FeedbackDuration;
				target.ToDestinationValue = this.ToDestinationValue;
				target.ToDestination();
			}
		}

		// Token: 0x060012B5 RID: 4789 RVA: 0x00050880 File Offset: 0x0004EA80
		protected override void CustomReset()
		{
			base.CustomReset();
			if (this.Active && MMF_FloatController.FeedbackTypeAuthorized && this.TargetFloatController != null)
			{
				this.ResetFloatController(this.TargetFloatController);
				foreach (FloatController floatController in this.ExtraTargetFloatControllers)
				{
					this.ResetFloatController(floatController);
				}
			}
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x00050904 File Offset: 0x0004EB04
		protected virtual void ResetFloatController(FloatController controller)
		{
			controller.OneTimeDuration = this._oneTimeDurationStorage;
			controller.OneTimeAmplitude = this._oneTimeAmplitudeStorage;
			controller.OneTimeCurve = this._oneTimeCurveStorage;
			controller.OneTimeRemapMin = this._oneTimeRemapMinStorage;
			controller.OneTimeRemapMax = this._oneTimeRemapMaxStorage;
			controller.ToDestinationCurve = this._toDestinationCurveStorage;
			controller.ToDestinationDuration = this._toDestinationDurationStorage;
			controller.ToDestinationValue = this._toDestinationValueStorage;
			controller.RevertToInitialValueAfterEnd = this._revertToInitialValueAfterEndStorage;
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x00050980 File Offset: 0x0004EB80
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_FloatController.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.TargetFloatController != null)
			{
				this.TargetFloatController.Stop();
				foreach (FloatController floatController in this.ExtraTargetFloatControllers)
				{
					floatController.Stop();
				}
			}
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x000509FC File Offset: 0x0004EBFC
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_FloatController.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetFloatController.RestoreInitialValues();
			foreach (FloatController floatController in this.ExtraTargetFloatControllers)
			{
				floatController.RestoreInitialValues();
			}
		}

		// Token: 0x04001509 RID: 5385
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400150A RID: 5386
		[MMFInspectorGroup("Float Controller", true, 36, true, false)]
		[Tooltip("the mode this controller is in")]
		public MMF_FloatController.Modes Mode;

		// Token: 0x0400150B RID: 5387
		[Tooltip("the float controller to trigger a one time play on")]
		public FloatController TargetFloatController;

		// Token: 0x0400150C RID: 5388
		[Tooltip("a list of extra and optional float controllers to trigger a one time play on")]
		public List<FloatController> ExtraTargetFloatControllers;

		// Token: 0x0400150D RID: 5389
		[Tooltip("whether this should revert to original at the end")]
		public bool RevertToInitialValueAfterEnd;

		// Token: 0x0400150E RID: 5390
		[Tooltip("the duration of the One Time shake")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float OneTimeDuration = 1f;

		// Token: 0x0400150F RID: 5391
		[Tooltip("the amplitude of the One Time shake (this will be multiplied by the curve's height)")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float OneTimeAmplitude = 1f;

		// Token: 0x04001510 RID: 5392
		[Tooltip("the low value to remap the normalized curve value to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float OneTimeRemapMin;

		// Token: 0x04001511 RID: 5393
		[Tooltip("the high value to remap the normalized curve value to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float OneTimeRemapMax = 1f;

		// Token: 0x04001512 RID: 5394
		[Tooltip("the curve to apply to the one time shake")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public AnimationCurve OneTimeCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001513 RID: 5395
		[Tooltip("the value to move this float controller to")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float ToDestinationValue = 1f;

		// Token: 0x04001514 RID: 5396
		[Tooltip("the duration over which to move the value")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float ToDestinationDuration = 1f;

		// Token: 0x04001515 RID: 5397
		[Tooltip("the curve over which to move the value in ToDestination mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public AnimationCurve ToDestinationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001516 RID: 5398
		protected float _oneTimeDurationStorage;

		// Token: 0x04001517 RID: 5399
		protected float _oneTimeAmplitudeStorage;

		// Token: 0x04001518 RID: 5400
		protected float _oneTimeRemapMinStorage;

		// Token: 0x04001519 RID: 5401
		protected float _oneTimeRemapMaxStorage;

		// Token: 0x0400151A RID: 5402
		protected AnimationCurve _oneTimeCurveStorage;

		// Token: 0x0400151B RID: 5403
		protected float _toDestinationValueStorage;

		// Token: 0x0400151C RID: 5404
		protected float _toDestinationDurationStorage;

		// Token: 0x0400151D RID: 5405
		protected AnimationCurve _toDestinationCurveStorage;

		// Token: 0x0400151E RID: 5406
		protected bool _revertToInitialValueAfterEndStorage;

		// Token: 0x0200052F RID: 1327
		public enum Modes
		{
			// Token: 0x040023CF RID: 9167
			OneTime,
			// Token: 0x040023D0 RID: 9168
			ToDestination
		}
	}
}
