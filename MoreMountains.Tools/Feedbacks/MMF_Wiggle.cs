using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000218 RID: 536
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you trigger position, rotation and/or scale wiggles on an object equipped with a MMWiggle component, for the specified durations.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Transform/Wiggle")]
	public class MMF_Wiggle : MMF_Feedback
	{
		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000EB3 RID: 3763 RVA: 0x000417F4 File Offset: 0x0003F9F4
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x000417F7 File Offset: 0x0003F9F7
		protected override void AutomateTargetAcquisition()
		{
			this.TargetWiggle = this.FindAutomatedTarget<MMWiggle>();
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000EB5 RID: 3765 RVA: 0x00041805 File Offset: 0x0003FA05
		// (set) Token: 0x06000EB6 RID: 3766 RVA: 0x0004183F File Offset: 0x0003FA3F
		public override float FeedbackDuration
		{
			get
			{
				return Mathf.Max(new float[]
				{
					this.ApplyTimeMultiplier(this.WigglePositionDuration),
					this.ApplyTimeMultiplier(this.WiggleRotationDuration),
					this.ApplyTimeMultiplier(this.WiggleScaleDuration)
				});
			}
			set
			{
				this.WigglePositionDuration = value;
				this.WiggleRotationDuration = value;
				this.WiggleScaleDuration = value;
			}
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x00041858 File Offset: 0x0003FA58
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Wiggle.FeedbackTypeAuthorized || this.TargetWiggle == null)
			{
				return;
			}
			this.TargetWiggle.enabled = true;
			if (this.WigglePosition)
			{
				this.TargetWiggle.PositionWiggleProperties.UseUnscaledTime = !this.InScaledTimescaleMode;
				this.TargetWiggle.WigglePosition(this.ApplyTimeMultiplier(this.WigglePositionDuration));
			}
			if (this.WiggleRotation)
			{
				this.TargetWiggle.RotationWiggleProperties.UseUnscaledTime = !this.InScaledTimescaleMode;
				this.TargetWiggle.WiggleRotation(this.ApplyTimeMultiplier(this.WiggleRotationDuration));
			}
			if (this.WiggleScale)
			{
				this.TargetWiggle.ScaleWiggleProperties.UseUnscaledTime = !this.InScaledTimescaleMode;
				this.TargetWiggle.WiggleScale(this.ApplyTimeMultiplier(this.WiggleScaleDuration));
			}
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x00041937 File Offset: 0x0003FB37
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Wiggle.FeedbackTypeAuthorized || this.TargetWiggle == null)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			this.TargetWiggle.enabled = false;
		}

		// Token: 0x06000EB9 RID: 3769 RVA: 0x0004196B File Offset: 0x0003FB6B
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Wiggle.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetWiggle.RestoreInitialValues();
		}

		// Token: 0x0400110B RID: 4363
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400110C RID: 4364
		[MMFInspectorGroup("Target", true, 54, true, false)]
		[Tooltip("the Wiggle component to target")]
		public MMWiggle TargetWiggle;

		// Token: 0x0400110D RID: 4365
		[MMFInspectorGroup("Position", true, 55, false, false)]
		[Tooltip("whether or not to wiggle position")]
		public bool WigglePosition = true;

		// Token: 0x0400110E RID: 4366
		[Tooltip("the duration (in seconds) of the position wiggle")]
		public float WigglePositionDuration;

		// Token: 0x0400110F RID: 4367
		[MMFInspectorGroup("Rotation", true, 56, false, false)]
		[Tooltip("whether or not to wiggle rotation")]
		public bool WiggleRotation;

		// Token: 0x04001110 RID: 4368
		[Tooltip("the duration (in seconds) of the rotation wiggle")]
		public float WiggleRotationDuration;

		// Token: 0x04001111 RID: 4369
		[MMFInspectorGroup("Scale", true, 57, false, false)]
		[Tooltip("whether or not to wiggle scale")]
		public bool WiggleScale;

		// Token: 0x04001112 RID: 4370
		[Tooltip("the duration (in seconds) of the scale wiggle")]
		public float WiggleScaleDuration;
	}
}
