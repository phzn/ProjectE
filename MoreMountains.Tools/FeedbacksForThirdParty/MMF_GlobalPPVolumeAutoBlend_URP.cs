using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200034D RID: 845
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you pilot a Global PostProcessing Volume AutoBlend URP component. A GPPVAB component is placed on a PostProcessing Volume, and will let you control and blend its weight over time on demand.")]
	[FeedbackPath("PostProcess/Global PP Volume Auto Blend URP")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.URP", null)]
	public class MMF_GlobalPPVolumeAutoBlend_URP : MMF_Feedback
	{
		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06001720 RID: 5920 RVA: 0x00061D36 File Offset: 0x0005FF36
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001721 RID: 5921 RVA: 0x00061D39 File Offset: 0x0005FF39
		protected override void AutomateTargetAcquisition()
		{
			this.TargetAutoBlend = this.FindAutomatedTarget<MMGlobalPostProcessingVolumeAutoBlend_URP>();
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06001722 RID: 5922 RVA: 0x00061D47 File Offset: 0x0005FF47
		// (set) Token: 0x06001723 RID: 5923 RVA: 0x00061D84 File Offset: 0x0005FF84
		public override float FeedbackDuration
		{
			get
			{
				if (this.Mode == MMF_GlobalPPVolumeAutoBlend_URP.Modes.Override)
				{
					return this.ApplyTimeMultiplier(this.BlendDuration);
				}
				if (this.TargetAutoBlend == null)
				{
					return 0.1f;
				}
				return this.ApplyTimeMultiplier(this.TargetAutoBlend.BlendDuration);
			}
			set
			{
				this.BlendDuration = value;
				if (this.TargetAutoBlend != null)
				{
					this.TargetAutoBlend.BlendDuration = value;
				}
			}
		}

		// Token: 0x06001724 RID: 5924 RVA: 0x00061DA8 File Offset: 0x0005FFA8
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_GlobalPPVolumeAutoBlend_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.TargetAutoBlend == null)
			{
				Debug.LogWarning(this.Owner.name + " : this MMFeedbackGlobalPPVolumeAutoBlend needs a TargetAutoBlend, please set one in its inspector.");
				return;
			}
			if (this.Mode == MMF_GlobalPPVolumeAutoBlend_URP.Modes.Default)
			{
				if (this.BlendAction == MMF_GlobalPPVolumeAutoBlend_URP.Actions.Blend)
				{
					this.TargetAutoBlend.Blend();
					return;
				}
				if (this.BlendAction == MMF_GlobalPPVolumeAutoBlend_URP.Actions.BlendBack)
				{
					this.TargetAutoBlend.BlendBack();
					return;
				}
			}
			else
			{
				this.TargetAutoBlend.TimeScale = ((this.ComputedTimescaleMode == TimescaleModes.Scaled) ? MMGlobalPostProcessingVolumeAutoBlend_URP.TimeScales.Scaled : MMGlobalPostProcessingVolumeAutoBlend_URP.TimeScales.Unscaled);
				this.TargetAutoBlend.BlendDuration = this.FeedbackDuration;
				this.TargetAutoBlend.Curve = this.BlendCurve;
				this.TargetAutoBlend.InitialWeight = this.InitialWeight;
				this.TargetAutoBlend.FinalWeight = this.FinalWeight;
				this.TargetAutoBlend.Blend();
			}
		}

		// Token: 0x06001725 RID: 5925 RVA: 0x00061E85 File Offset: 0x00060085
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_GlobalPPVolumeAutoBlend_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			if (this.TargetAutoBlend != null)
			{
				this.TargetAutoBlend.StopBlending();
			}
		}

		// Token: 0x06001726 RID: 5926 RVA: 0x00061EB8 File Offset: 0x000600B8
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_GlobalPPVolumeAutoBlend_URP.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetAutoBlend.RestoreInitialValues();
		}

		// Token: 0x04001A84 RID: 6788
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001A85 RID: 6789
		[MMFInspectorGroup("PostProcess Volume Blend", true, 22, true, false)]
		public MMGlobalPostProcessingVolumeAutoBlend_URP TargetAutoBlend;

		// Token: 0x04001A86 RID: 6790
		public MMF_GlobalPPVolumeAutoBlend_URP.Modes Mode;

		// Token: 0x04001A87 RID: 6791
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public MMF_GlobalPPVolumeAutoBlend_URP.Actions BlendAction;

		// Token: 0x04001A88 RID: 6792
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float BlendDuration = 1f;

		// Token: 0x04001A89 RID: 6793
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public AnimationCurve BlendCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x04001A8A RID: 6794
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float InitialWeight;

		// Token: 0x04001A8B RID: 6795
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float FinalWeight = 1f;

		// Token: 0x02000582 RID: 1410
		public enum Modes
		{
			// Token: 0x040024D4 RID: 9428
			Default,
			// Token: 0x040024D5 RID: 9429
			Override
		}

		// Token: 0x02000583 RID: 1411
		public enum Actions
		{
			// Token: 0x040024D7 RID: 9431
			Blend,
			// Token: 0x040024D8 RID: 9432
			BlendBack
		}
	}
}
