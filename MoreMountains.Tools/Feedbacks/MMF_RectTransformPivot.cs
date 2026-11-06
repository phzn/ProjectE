using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002AF RID: 687
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you control the position of a RectTransform's pivot over time")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("UI/RectTransform Pivot")]
	public class MMF_RectTransformPivot : MMF_FeedbackBase
	{
		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06001349 RID: 4937 RVA: 0x00053431 File Offset: 0x00051631
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x0600134A RID: 4938 RVA: 0x00053434 File Offset: 0x00051634
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x00053437 File Offset: 0x00051637
		protected override void AutomateTargetAcquisition()
		{
			this.TargetRectTransform = this.FindAutomatedTarget<RectTransform>();
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x00053448 File Offset: 0x00051648
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
				TargetPropertyName = "pivot",
				RelativeValue = this.RelativeValues,
				Vector2RemapZero = this.RemapZero,
				Vector2RemapOne = this.RemapOne
			};
			mmf_FeedbackBaseTarget.LevelCurve = this.SpeedCurve;
			mmf_FeedbackBaseTarget.RemapLevelZero = 0f;
			mmf_FeedbackBaseTarget.RemapLevelOne = 1f;
			mmf_FeedbackBaseTarget.InstantLevel = 1f;
			this._targets.Add(mmf_FeedbackBaseTarget);
		}

		// Token: 0x0400163D RID: 5693
		[MMFInspectorGroup("Target RectTransform", true, 37, true, false)]
		[Tooltip("the RectTransform whose position you want to control over time")]
		public RectTransform TargetRectTransform;

		// Token: 0x0400163E RID: 5694
		[MMFInspectorGroup("Pivot", true, 39, false, false)]
		[Tooltip("The curve along which to evaluate the position of the RectTransform's pivot")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public MMTweenType SpeedCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		}));

		// Token: 0x0400163F RID: 5695
		[Tooltip("the position to remap the curve's 0 to, randomized between its min and max - put the same value in both min and max if you don't want any randomness")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		[MMFVector(new string[] { "Min", "Max" })]
		public Vector2 RemapZero = Vector2.zero;

		// Token: 0x04001640 RID: 5696
		[Tooltip("the position to remap the curve's 1 to, randomized between its min and max - put the same value in both min and max if you don't want any randomness")]
		[MMFEnumCondition("Mode", new int[] { 0, 1 })]
		[MMFVector(new string[] { "Min", "Max" })]
		public Vector2 RemapOne = Vector2.one;
	}
}
