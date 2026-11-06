using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002B0 RID: 688
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you control the size delta property (the size of this RectTransform relative to the distances between the anchors) of a RectTransform, over time")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("UI/RectTransformSizeDelta")]
	public class MMF_RectTransformSizeDelta : MMF_FeedbackBase
	{
		// Token: 0x17000218 RID: 536
		// (get) Token: 0x0600134E RID: 4942 RVA: 0x00053567 File Offset: 0x00051767
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x0600134F RID: 4943 RVA: 0x0005356A File Offset: 0x0005176A
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x0005356D File Offset: 0x0005176D
		protected override void AutomateTargetAcquisition()
		{
			this.TargetRectTransform = this.FindAutomatedTarget<RectTransform>();
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x0005357C File Offset: 0x0005177C
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
				TargetPropertyName = "sizeDelta",
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

		// Token: 0x04001641 RID: 5697
		[MMFInspectorGroup("Target RectTransform", true, 37, true, false)]
		[Tooltip("the rect transform we want to impact")]
		public RectTransform TargetRectTransform;

		// Token: 0x04001642 RID: 5698
		[MMFInspectorGroup("Size Delta", true, 38, false, false)]
		[Tooltip("the speed at which we should animate the size delta")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public MMTweenType SpeedCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		}));

		// Token: 0x04001643 RID: 5699
		[Tooltip("the value to remap the curve's 0 to, randomized between its min and max - put the same value in both min and max if you don't want any randomness")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		[MMFVector(new string[] { "Min", "Max" })]
		public Vector2 RemapZero = Vector2.zero;

		// Token: 0x04001644 RID: 5700
		[Tooltip("the value to remap the curve's 1 to, randomized between its min and max - put the same value in both min and max if you don't want any randomness")]
		[MMFEnumCondition("Mode", new int[] { 0, 1 })]
		[MMFVector(new string[] { "Min", "Max" })]
		public Vector2 RemapOne = Vector2.one;
	}
}
