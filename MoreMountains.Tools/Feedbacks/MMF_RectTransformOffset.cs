using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002AE RID: 686
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you control the offset of the lower left corner of the rectangle relative to the lower left anchor, and the offset of the upper right corner of the rectangle relative to the upper right anchor.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("UI/RectTransform Offset")]
	public class MMF_RectTransformOffset : MMF_FeedbackBase
	{
		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06001344 RID: 4932 RVA: 0x000531E5 File Offset: 0x000513E5
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06001345 RID: 4933 RVA: 0x000531E8 File Offset: 0x000513E8
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x000531EB File Offset: 0x000513EB
		protected override void AutomateTargetAcquisition()
		{
			this.TargetRectTransform = this.FindAutomatedTarget<RectTransform>();
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x000531FC File Offset: 0x000513FC
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
				TargetPropertyName = "offsetMin",
				RelativeValue = this.RelativeValues,
				Vector2RemapZero = this.OffsetMinRemapZero,
				Vector2RemapOne = this.OffsetMinRemapOne,
				ShouldModifyValue = this.ModifyOffsetMin
			};
			mmf_FeedbackBaseTarget.LevelCurve = this.OffsetMinCurve;
			mmf_FeedbackBaseTarget.RemapLevelZero = 0f;
			mmf_FeedbackBaseTarget.RemapLevelOne = 1f;
			mmf_FeedbackBaseTarget.InstantLevel = 1f;
			this._targets.Add(mmf_FeedbackBaseTarget);
			MMF_FeedbackBaseTarget mmf_FeedbackBaseTarget2 = new MMF_FeedbackBaseTarget();
			mmf_FeedbackBaseTarget2.Target = new MMPropertyReceiver
			{
				TargetObject = this.TargetRectTransform.gameObject,
				TargetComponent = this.TargetRectTransform,
				TargetPropertyName = "offsetMax",
				RelativeValue = this.RelativeValues,
				Vector2RemapZero = this.OffsetMaxRemapZero,
				Vector2RemapOne = this.OffsetMaxRemapOne,
				ShouldModifyValue = this.ModifyOffsetMax
			};
			mmf_FeedbackBaseTarget2.LevelCurve = this.OffsetMaxCurve;
			mmf_FeedbackBaseTarget2.RemapLevelZero = 0f;
			mmf_FeedbackBaseTarget2.RemapLevelOne = 1f;
			mmf_FeedbackBaseTarget2.InstantLevel = 1f;
			this._targets.Add(mmf_FeedbackBaseTarget2);
		}

		// Token: 0x04001634 RID: 5684
		[MMFInspectorGroup("Target RectTransform", true, 37, true, false)]
		public RectTransform TargetRectTransform;

		// Token: 0x04001635 RID: 5685
		[MMFInspectorGroup("Offset Min", true, 40, false, false)]
		[Tooltip("whether we should modify the offset min or not")]
		public bool ModifyOffsetMin = true;

		// Token: 0x04001636 RID: 5686
		[Tooltip("the curve to animate the min offset on")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public MMTweenType OffsetMinCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		}));

		// Token: 0x04001637 RID: 5687
		[Tooltip("the value to remap the min curve's 0 on")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public Vector2 OffsetMinRemapZero = Vector2.zero;

		// Token: 0x04001638 RID: 5688
		[Tooltip("the value to remap the min curve's 1 on")]
		[MMFEnumCondition("Mode", new int[] { 0, 1 })]
		public Vector2 OffsetMinRemapOne = Vector2.one;

		// Token: 0x04001639 RID: 5689
		[MMFInspectorGroup("Offset Max", true, 41, false, false)]
		[Tooltip("whether we should modify the offset max or not")]
		public bool ModifyOffsetMax = true;

		// Token: 0x0400163A RID: 5690
		[Tooltip("the curve to animate the max offset on")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public MMTweenType OffsetMaxCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		}));

		// Token: 0x0400163B RID: 5691
		[Tooltip("the value to remap the max curve's 0 on")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public Vector2 OffsetMaxRemapZero = Vector2.zero;

		// Token: 0x0400163C RID: 5692
		[Tooltip("the value to remap the max curve's 1 on")]
		[MMFEnumCondition("Mode", new int[] { 0, 1 })]
		public Vector2 OffsetMaxRemapOne = Vector2.one;
	}
}
