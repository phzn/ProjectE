using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002AD RID: 685
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you control the min and max anchors of a RectTransform over time. That's the normalized position in the parent RectTransform that the lower left and upper right corners are anchored to.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("UI/RectTransform Anchor")]
	public class MMF_RectTransformAnchor : MMF_FeedbackBase
	{
		// Token: 0x17000212 RID: 530
		// (get) Token: 0x0600133F RID: 4927 RVA: 0x00052F9B File Offset: 0x0005119B
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06001340 RID: 4928 RVA: 0x00052F9E File Offset: 0x0005119E
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001341 RID: 4929 RVA: 0x00052FA1 File Offset: 0x000511A1
		protected override void AutomateTargetAcquisition()
		{
			this.TargetRectTransform = this.FindAutomatedTarget<RectTransform>();
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x00052FB0 File Offset: 0x000511B0
		protected override void FillTargets()
		{
			if (this.TargetRectTransform == null)
			{
				return;
			}
			MMF_FeedbackBaseTarget mmf_FeedbackBaseTarget = new MMF_FeedbackBaseTarget();
			mmf_FeedbackBaseTarget.Target = new MMPropertyReceiver
			{
				TargetObject = this.TargetRectTransform.gameObject,
				TargetComponent = this.TargetRectTransform,
				TargetPropertyName = "anchorMin",
				RelativeValue = this.RelativeValues,
				Vector2RemapZero = this.AnchorMinRemapZero,
				Vector2RemapOne = this.AnchorMinRemapOne,
				ShouldModifyValue = this.ModifyAnchorMin
			};
			mmf_FeedbackBaseTarget.LevelCurve = this.AnchorMinCurve;
			mmf_FeedbackBaseTarget.RemapLevelZero = 0f;
			mmf_FeedbackBaseTarget.RemapLevelOne = 1f;
			mmf_FeedbackBaseTarget.InstantLevel = 1f;
			this._targets.Add(mmf_FeedbackBaseTarget);
			MMF_FeedbackBaseTarget mmf_FeedbackBaseTarget2 = new MMF_FeedbackBaseTarget();
			mmf_FeedbackBaseTarget2.Target = new MMPropertyReceiver
			{
				TargetObject = this.TargetRectTransform.gameObject,
				TargetComponent = this.TargetRectTransform,
				TargetPropertyName = "anchorMax",
				RelativeValue = this.RelativeValues,
				Vector2RemapZero = this.AnchorMaxRemapZero,
				Vector2RemapOne = this.AnchorMaxRemapOne,
				ShouldModifyValue = this.ModifyAnchorMax
			};
			mmf_FeedbackBaseTarget2.LevelCurve = this.AnchorMaxCurve;
			mmf_FeedbackBaseTarget2.RemapLevelZero = 0f;
			mmf_FeedbackBaseTarget2.RemapLevelOne = 1f;
			mmf_FeedbackBaseTarget2.InstantLevel = 1f;
			this._targets.Add(mmf_FeedbackBaseTarget2);
		}

		// Token: 0x0400162B RID: 5675
		[MMFInspectorGroup("Target RectTransform", true, 37, true, false)]
		[Tooltip("the target RectTransform to control")]
		public RectTransform TargetRectTransform;

		// Token: 0x0400162C RID: 5676
		[MMFInspectorGroup("Anchor Min", true, 43, false, false)]
		[Tooltip("whether or not to modify the min anchor")]
		public bool ModifyAnchorMin = true;

		// Token: 0x0400162D RID: 5677
		[Tooltip("the curve to animate the min anchor on")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public MMTweenType AnchorMinCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		}));

		// Token: 0x0400162E RID: 5678
		[Tooltip("the value to remap the min anchor curve's 0 on")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public Vector2 AnchorMinRemapZero = Vector2.zero;

		// Token: 0x0400162F RID: 5679
		[Tooltip("the value to remap the min anchor curve's 1 on")]
		[MMFEnumCondition("Mode", new int[] { 0, 1 })]
		public Vector2 AnchorMinRemapOne = Vector2.one;

		// Token: 0x04001630 RID: 5680
		[MMFInspectorGroup("Anchor Max", true, 44, false, false)]
		[Tooltip("whether or not to modify the max anchor")]
		public bool ModifyAnchorMax = true;

		// Token: 0x04001631 RID: 5681
		[Tooltip("the curve to animate the max anchor on")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public MMTweenType AnchorMaxCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		}));

		// Token: 0x04001632 RID: 5682
		[Tooltip("the value to remap the max anchor curve's 0 on")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public Vector2 AnchorMaxRemapZero = Vector2.zero;

		// Token: 0x04001633 RID: 5683
		[Tooltip("the value to remap the max anchor curve's 1 on")]
		[MMFEnumCondition("Mode", new int[] { 0, 1 })]
		public Vector2 AnchorMaxRemapOne = Vector2.one;
	}
}
