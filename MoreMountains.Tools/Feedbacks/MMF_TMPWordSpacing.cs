using System;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002C6 RID: 710
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you control the word spacing of a target TMP over time.")]
	[FeedbackPath("TextMesh Pro/TMP Word Spacing")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.TextMeshPro", null)]
	public class MMF_TMPWordSpacing : MMF_FeedbackBase
	{
		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06001422 RID: 5154 RVA: 0x00056DD6 File Offset: 0x00054FD6
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06001423 RID: 5155 RVA: 0x00056DD9 File Offset: 0x00054FD9
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x00056DDC File Offset: 0x00054FDC
		protected override void AutomateTargetAcquisition()
		{
			this.TargetTMPText = this.FindAutomatedTarget<TMP_Text>();
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x00056DEC File Offset: 0x00054FEC
		protected override void FillTargets()
		{
			if (this.TargetTMPText == null)
			{
				return;
			}
			MMF_FeedbackBaseTarget mmf_FeedbackBaseTarget = new MMF_FeedbackBaseTarget();
			mmf_FeedbackBaseTarget.Target = new MMPropertyReceiver
			{
				TargetObject = this.TargetTMPText.gameObject,
				TargetComponent = this.TargetTMPText,
				TargetPropertyName = "wordSpacing",
				RelativeValue = this.RelativeValues
			};
			mmf_FeedbackBaseTarget.LevelCurve = this.WordSpacingCurve;
			mmf_FeedbackBaseTarget.RemapLevelZero = this.RemapZero;
			mmf_FeedbackBaseTarget.RemapLevelOne = this.RemapOne;
			mmf_FeedbackBaseTarget.InstantLevel = this.InstantFontSize;
			this._targets.Add(mmf_FeedbackBaseTarget);
		}

		// Token: 0x0400172E RID: 5934
		[MMFInspectorGroup("Target", true, 12, true, false)]
		[Tooltip("the TMP_Text component to control")]
		public TMP_Text TargetTMPText;

		// Token: 0x0400172F RID: 5935
		[MMFInspectorGroup("Word Spacing", true, 15, false, false)]
		[Tooltip("the curve to tween on")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public MMTweenType WordSpacingCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x04001730 RID: 5936
		[Tooltip("the value to remap the curve's 0 to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float RemapZero;

		// Token: 0x04001731 RID: 5937
		[Tooltip("the value to remap the curve's 1 to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float RemapOne = 10f;

		// Token: 0x04001732 RID: 5938
		[Tooltip("the value to move to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float InstantFontSize;
	}
}
