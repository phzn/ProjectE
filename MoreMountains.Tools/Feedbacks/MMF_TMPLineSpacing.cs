using System;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002BF RID: 703
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you control the line spacing of a target TMP over time.")]
	[FeedbackPath("TextMesh Pro/TMP Line Spacing")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.TextMeshPro", null)]
	public class MMF_TMPLineSpacing : MMF_FeedbackBase
	{
		// Token: 0x17000236 RID: 566
		// (get) Token: 0x060013E4 RID: 5092 RVA: 0x00055CAA File Offset: 0x00053EAA
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x060013E5 RID: 5093 RVA: 0x00055CAD File Offset: 0x00053EAD
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x00055CB0 File Offset: 0x00053EB0
		protected override void AutomateTargetAcquisition()
		{
			this.TargetTMPText = this.FindAutomatedTarget<TMP_Text>();
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x00055CC0 File Offset: 0x00053EC0
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
				TargetPropertyName = "lineSpacing",
				RelativeValue = this.RelativeValues
			};
			mmf_FeedbackBaseTarget.LevelCurve = this.LineSpacingCurve;
			mmf_FeedbackBaseTarget.RemapLevelZero = this.RemapZero;
			mmf_FeedbackBaseTarget.RemapLevelOne = this.RemapOne;
			mmf_FeedbackBaseTarget.InstantLevel = this.InstantFontSize;
			this._targets.Add(mmf_FeedbackBaseTarget);
		}

		// Token: 0x040016F0 RID: 5872
		[MMFInspectorGroup("Target", true, 12, true, false)]
		[Tooltip("the TMP_Text component to control")]
		public TMP_Text TargetTMPText;

		// Token: 0x040016F1 RID: 5873
		[MMFInspectorGroup("Paragraph Spacing", true, 37, false, false)]
		[Tooltip("the curve to tween on")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public MMTweenType LineSpacingCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x040016F2 RID: 5874
		[Tooltip("the value to remap the curve's 0 to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float RemapZero;

		// Token: 0x040016F3 RID: 5875
		[Tooltip("the value to remap the curve's 1 to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float RemapOne = 10f;

		// Token: 0x040016F4 RID: 5876
		[Tooltip("the value to move to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float InstantFontSize;
	}
}
