using System;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002B9 RID: 697
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you control the character spacing of a target TMP over time.")]
	[FeedbackPath("TextMesh Pro/TMP Character Spacing")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.TextMeshPro", null)]
	public class MMF_TMPCharacterSpacing : MMF_FeedbackBase
	{
		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060013A7 RID: 5031 RVA: 0x000550EA File Offset: 0x000532EA
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060013A8 RID: 5032 RVA: 0x000550ED File Offset: 0x000532ED
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x000550F0 File Offset: 0x000532F0
		protected override void AutomateTargetAcquisition()
		{
			this.TargetTMPText = this.FindAutomatedTarget<TMP_Text>();
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x00055100 File Offset: 0x00053300
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
				TargetPropertyName = "characterSpacing",
				RelativeValue = this.RelativeValues
			};
			mmf_FeedbackBaseTarget.LevelCurve = this.CharacterSpacingCurve;
			mmf_FeedbackBaseTarget.RemapLevelZero = this.RemapZero;
			mmf_FeedbackBaseTarget.RemapLevelOne = this.RemapOne;
			mmf_FeedbackBaseTarget.InstantLevel = this.InstantFontSize;
			this._targets.Add(mmf_FeedbackBaseTarget);
		}

		// Token: 0x040016B4 RID: 5812
		[MMFInspectorGroup("Target", true, 12, true, false)]
		[Tooltip("the TMP_Text component to control")]
		public TMP_Text TargetTMPText;

		// Token: 0x040016B5 RID: 5813
		[MMFInspectorGroup("Character Spacing", true, 16, false, false)]
		[Tooltip("the curve to tween on")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public MMTweenType CharacterSpacingCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x040016B6 RID: 5814
		[Tooltip("the value to remap the curve's 0 to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float RemapZero;

		// Token: 0x040016B7 RID: 5815
		[Tooltip("the value to remap the curve's 1 to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float RemapOne = 1f;

		// Token: 0x040016B8 RID: 5816
		[Tooltip("the value to move to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float InstantFontSize;
	}
}
