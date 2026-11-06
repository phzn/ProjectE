using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002B3 RID: 691
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you control the font size of a target Text over time.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("UI/Text Font Size")]
	public class MMF_TextFontSize : MMF_FeedbackBase
	{
		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06001371 RID: 4977 RVA: 0x000543F6 File Offset: 0x000525F6
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06001372 RID: 4978 RVA: 0x000543F9 File Offset: 0x000525F9
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x000543FC File Offset: 0x000525FC
		protected override void AutomateTargetAcquisition()
		{
			this.TargetText = this.FindAutomatedTarget<Text>();
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x0005440C File Offset: 0x0005260C
		protected override void FillTargets()
		{
			if (this.TargetText == null)
			{
				return;
			}
			MMF_FeedbackBaseTarget mmf_FeedbackBaseTarget = new MMF_FeedbackBaseTarget();
			mmf_FeedbackBaseTarget.Target = new MMPropertyReceiver
			{
				TargetObject = this.TargetText.gameObject,
				TargetComponent = this.TargetText,
				TargetPropertyName = "fontSize",
				RelativeValue = this.RelativeValues
			};
			mmf_FeedbackBaseTarget.LevelCurve = this.FontSizeCurve;
			mmf_FeedbackBaseTarget.RemapLevelZero = this.RemapZero;
			mmf_FeedbackBaseTarget.RemapLevelOne = this.RemapOne;
			mmf_FeedbackBaseTarget.InstantLevel = this.InstantFontSize;
			this._targets.Add(mmf_FeedbackBaseTarget);
		}

		// Token: 0x04001683 RID: 5763
		[MMFInspectorGroup("Target", true, 58, true, false)]
		[Tooltip("the TMP_Text component to control")]
		public Text TargetText;

		// Token: 0x04001684 RID: 5764
		[MMFInspectorGroup("Font Size", true, 59, false, false)]
		[Tooltip("the curve to tween on")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public MMTweenType FontSizeCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x04001685 RID: 5765
		[Tooltip("the value to remap the curve's 0 to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float RemapZero;

		// Token: 0x04001686 RID: 5766
		[Tooltip("the value to remap the curve's 1 to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float RemapOne = 1f;

		// Token: 0x04001687 RID: 5767
		[Tooltip("the value to move to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float InstantFontSize;
	}
}
