using System;
using MoreMountains.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002BE RID: 702
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you control the font size of a target TMP over time.")]
	[FeedbackPath("TextMesh Pro/TMP Font Size")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.TextMeshPro", null)]
	public class MMF_TMPFontSize : MMF_FeedbackBase
	{
		// Token: 0x17000234 RID: 564
		// (get) Token: 0x060013DF RID: 5087 RVA: 0x00055B7F File Offset: 0x00053D7F
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x060013E0 RID: 5088 RVA: 0x00055B82 File Offset: 0x00053D82
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060013E1 RID: 5089 RVA: 0x00055B85 File Offset: 0x00053D85
		protected override void AutomateTargetAcquisition()
		{
			this.TargetTMPText = this.FindAutomatedTarget<TMP_Text>();
		}

		// Token: 0x060013E2 RID: 5090 RVA: 0x00055B94 File Offset: 0x00053D94
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
				TargetPropertyName = "fontSize",
				RelativeValue = this.RelativeValues
			};
			mmf_FeedbackBaseTarget.LevelCurve = this.FontSizeCurve;
			mmf_FeedbackBaseTarget.RemapLevelZero = this.RemapZero;
			mmf_FeedbackBaseTarget.RemapLevelOne = this.RemapOne;
			mmf_FeedbackBaseTarget.InstantLevel = this.InstantFontSize;
			this._targets.Add(mmf_FeedbackBaseTarget);
		}

		// Token: 0x040016EB RID: 5867
		[MMFInspectorGroup("Target", true, 12, true, false)]
		[Tooltip("the TMP_Text component to control")]
		public TMP_Text TargetTMPText;

		// Token: 0x040016EC RID: 5868
		[MMFInspectorGroup("Font Size", true, 16, false, false)]
		[Tooltip("the curve to tween on")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public MMTweenType FontSizeCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x040016ED RID: 5869
		[Tooltip("the value to remap the curve's 0 to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float RemapZero;

		// Token: 0x040016EE RID: 5870
		[Tooltip("the value to remap the curve's 1 to")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float RemapOne = 1f;

		// Token: 0x040016EF RID: 5871
		[Tooltip("the value to move to in instant mode")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float InstantFontSize;
	}
}
