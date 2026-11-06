using System;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002C2 RID: 706
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you control the paragraph spacing of a target TMP over time.")]
	[FeedbackPath("TextMesh Pro/TMP Paragraph Spacing")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.TextMeshPro", null)]
	public class MMF_TMPParagraphSpacing : MMF_FeedbackBase
	{
		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060013FA RID: 5114 RVA: 0x0005624A File Offset: 0x0005444A
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060013FB RID: 5115 RVA: 0x0005624D File Offset: 0x0005444D
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x00056250 File Offset: 0x00054450
		protected override void AutomateTargetAcquisition()
		{
			this.TargetTMPText = this.FindAutomatedTarget<TMP_Text>();
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x00056260 File Offset: 0x00054460
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
				TargetPropertyName = "paragraphSpacing",
				RelativeValue = this.RelativeValues
			};
			mmf_FeedbackBaseTarget.LevelCurve = this.ParagraphSpacingCurve;
			mmf_FeedbackBaseTarget.RemapLevelZero = this.RemapZero;
			mmf_FeedbackBaseTarget.RemapLevelOne = this.RemapOne;
			mmf_FeedbackBaseTarget.InstantLevel = this.InstantFontSize;
			this._targets.Add(mmf_FeedbackBaseTarget);
		}

		// Token: 0x04001705 RID: 5893
		[MMFInspectorGroup("Target", true, 12, true, false)]
		[Tooltip("the TMP_Text component to control")]
		public TMP_Text TargetTMPText;

		// Token: 0x04001706 RID: 5894
		[MMFInspectorGroup("Paragraph Spacing", true, 21, false, false)]
		[Tooltip("the curve to tween on")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public MMTweenType ParagraphSpacingCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x04001707 RID: 5895
		[Tooltip("the value to remap the curve's 0 to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float RemapZero;

		// Token: 0x04001708 RID: 5896
		[Tooltip("the value to remap the curve's 1 to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float RemapOne = 10f;

		// Token: 0x04001709 RID: 5897
		[Tooltip("the value to move to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float InstantFontSize;
	}
}
