using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000211 RID: 529
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback lets you control the contents of a target Text over time.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("UI/Text")]
	public class MMF_Text : MMF_Feedback
	{
		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000E75 RID: 3701 RVA: 0x00040713 File Offset: 0x0003E913
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x00040716 File Offset: 0x0003E916
		protected override void AutomateTargetAcquisition()
		{
			this.TargetText = this.FindAutomatedTarget<Text>();
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x00040724 File Offset: 0x0003E924
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Text.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.TargetText == null)
			{
				return;
			}
			this._initialText = this.TargetText.text;
			this.TargetText.text = this.NewText;
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x00040772 File Offset: 0x0003E972
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Text.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetText.text = this._initialText;
		}

		// Token: 0x040010BD RID: 4285
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040010BE RID: 4286
		[MMFInspectorGroup("Text", true, 76, true, false)]
		[Tooltip(" Text component to control")]
		public Text TargetText;

		// Token: 0x040010BF RID: 4287
		[Tooltip("the new text to replace the old one with")]
		[TextArea]
		public string NewText = "Hello World";

		// Token: 0x040010C0 RID: 4288
		protected string _initialText;

		// Token: 0x020004FB RID: 1275
		public enum ColorModes
		{
			// Token: 0x0400234F RID: 9039
			Instant,
			// Token: 0x04002350 RID: 9040
			Gradient,
			// Token: 0x04002351 RID: 9041
			Interpolate
		}
	}
}
