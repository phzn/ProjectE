using System;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200031F RID: 799
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you pilot a Global PostProcessing Volume AutoBlend component. A GPPVAB component is placed on a PostProcessing Volume, and will let you control and blend its weight over time on demand.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.PostProcessing", null)]
	public class MMF_GlobalPPVolumeAutoBlend : MMF_Feedback
	{
		// Token: 0x1700028E RID: 654
		// (get) Token: 0x0600162E RID: 5678 RVA: 0x0005E2F2 File Offset: 0x0005C4F2
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600162F RID: 5679 RVA: 0x0005E2F5 File Offset: 0x0005C4F5
		protected override void AutomateTargetAcquisition()
		{
			this.TargetAutoBlend = this.FindAutomatedTarget<MMGlobalPostProcessingVolumeAutoBlend>();
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06001630 RID: 5680 RVA: 0x0005E303 File Offset: 0x0005C503
		public override float FeedbackDuration
		{
			get
			{
				if (this.Mode == MMF_GlobalPPVolumeAutoBlend.Modes.Override)
				{
					return this.ApplyTimeMultiplier(this.BlendDuration);
				}
				if (this.TargetAutoBlend == null)
				{
					return 0.1f;
				}
				return this.ApplyTimeMultiplier(this.TargetAutoBlend.BlendDuration);
			}
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x0005E340 File Offset: 0x0005C540
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (this.Active)
			{
				bool feedbackTypeAuthorized = MMF_GlobalPPVolumeAutoBlend.FeedbackTypeAuthorized;
			}
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x0005E350 File Offset: 0x0005C550
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (this.Active)
			{
				bool feedbackTypeAuthorized = MMF_GlobalPPVolumeAutoBlend.FeedbackTypeAuthorized;
			}
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x0005E360 File Offset: 0x0005C560
		protected override void CustomRestoreInitialValues()
		{
			if (this.Active)
			{
				bool feedbackTypeAuthorized = MMF_GlobalPPVolumeAutoBlend.FeedbackTypeAuthorized;
			}
		}

		// Token: 0x04001984 RID: 6532
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001985 RID: 6533
		[MMFInspectorGroup("PostProcess Volume Blend", true, 53, true, false)]
		[Tooltip("the target auto blend to pilot with this feedback")]
		public MMGlobalPostProcessingVolumeAutoBlend TargetAutoBlend;

		// Token: 0x04001986 RID: 6534
		[Tooltip("the chosen mode")]
		public MMF_GlobalPPVolumeAutoBlend.Modes Mode;

		// Token: 0x04001987 RID: 6535
		[Tooltip("the chosen action when in default mode")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public MMF_GlobalPPVolumeAutoBlend.Actions BlendAction;

		// Token: 0x04001988 RID: 6536
		[Tooltip("the duration of the blend, in seconds when in override mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float BlendDuration = 1f;

		// Token: 0x04001989 RID: 6537
		[Tooltip("the curve to apply to the blend")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public AnimationCurve BlendCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x0400198A RID: 6538
		[Tooltip("the weight to blend from")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float InitialWeight;

		// Token: 0x0400198B RID: 6539
		[Tooltip("the weight to blend to")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float FinalWeight = 1f;

		// Token: 0x0400198C RID: 6540
		[Tooltip("whether or not to reset to the initial value at the end of the shake")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public bool ResetToInitialValueOnEnd = true;

		// Token: 0x0200056F RID: 1391
		public enum Modes
		{
			// Token: 0x040024A5 RID: 9381
			Default,
			// Token: 0x040024A6 RID: 9382
			Override
		}

		// Token: 0x02000570 RID: 1392
		public enum Actions
		{
			// Token: 0x040024A8 RID: 9384
			Blend,
			// Token: 0x040024A9 RID: 9385
			BlendBack
		}
	}
}
