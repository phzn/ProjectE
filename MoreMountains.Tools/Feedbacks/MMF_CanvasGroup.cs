using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000295 RID: 661
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you control the opacity of a canvas group over time.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("UI/CanvasGroup")]
	public class MMF_CanvasGroup : MMF_FeedbackBase
	{
		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06001291 RID: 4753 RVA: 0x00050032 File Offset: 0x0004E232
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06001292 RID: 4754 RVA: 0x00050035 File Offset: 0x0004E235
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001293 RID: 4755 RVA: 0x00050038 File Offset: 0x0004E238
		protected override void AutomateTargetAcquisition()
		{
			this.TargetCanvasGroup = this.FindAutomatedTarget<CanvasGroup>();
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x00050046 File Offset: 0x0004E246
		public override void OnAddFeedback()
		{
			base.OnAddFeedback();
			this.RelativeValues = false;
		}

		// Token: 0x06001295 RID: 4757 RVA: 0x00050058 File Offset: 0x0004E258
		protected override void FillTargets()
		{
			if (this.TargetCanvasGroup == null)
			{
				return;
			}
			MMF_FeedbackBaseTarget mmf_FeedbackBaseTarget = new MMF_FeedbackBaseTarget();
			mmf_FeedbackBaseTarget.Target = new MMPropertyReceiver
			{
				TargetObject = this.TargetCanvasGroup.gameObject,
				TargetComponent = this.TargetCanvasGroup,
				TargetPropertyName = "alpha",
				RelativeValue = this.RelativeValues
			};
			mmf_FeedbackBaseTarget.LevelCurve = this.AlphaCurve;
			mmf_FeedbackBaseTarget.RemapLevelZero = this.RemapZero;
			mmf_FeedbackBaseTarget.RemapLevelOne = this.RemapOne;
			mmf_FeedbackBaseTarget.InstantLevel = this.InstantAlpha;
			this._targets.Add(mmf_FeedbackBaseTarget);
		}

		// Token: 0x040014EC RID: 5356
		[MMFInspectorGroup("Canvas Group", true, 12, true, false)]
		[Tooltip("the receiver to write the level to")]
		public CanvasGroup TargetCanvasGroup;

		// Token: 0x040014ED RID: 5357
		[Tooltip("the curve to tween the opacity on")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public MMTweenType AlphaCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x040014EE RID: 5358
		[Tooltip("the value to remap the opacity curve's 0 to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float RemapZero;

		// Token: 0x040014EF RID: 5359
		[Tooltip("the value to remap the opacity curve's 1 to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float RemapOne = 1f;

		// Token: 0x040014F0 RID: 5360
		[Tooltip("the value to move the opacity to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float InstantAlpha;
	}
}
