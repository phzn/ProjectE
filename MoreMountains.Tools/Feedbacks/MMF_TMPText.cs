using System;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002C4 RID: 708
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you change the text of a target TMP text component")]
	[FeedbackPath("TextMesh Pro/TMP Text")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.TextMeshPro", null)]
	public class MMF_TMPText : MMF_Feedback
	{
		// Token: 0x17000241 RID: 577
		// (get) Token: 0x0600140C RID: 5132 RVA: 0x0005660C File Offset: 0x0005480C
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x0005660F File Offset: 0x0005480F
		protected override void AutomateTargetAcquisition()
		{
			this.TargetTMPText = this.FindAutomatedTarget<TMP_Text>();
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x00056620 File Offset: 0x00054820
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_TMPText.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.TargetTMPText == null)
			{
				return;
			}
			this._initialText = this.TargetTMPText.text;
			this.TargetTMPText.text = this.NewText;
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x0005666E File Offset: 0x0005486E
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_TMPText.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetTMPText.text = this._initialText;
		}

		// Token: 0x04001716 RID: 5910
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001717 RID: 5911
		[MMFInspectorGroup("TextMeshPro Change Text", true, 12, true, false)]
		[Tooltip("the target TMP_Text component we want to change the text on")]
		public TMP_Text TargetTMPText;

		// Token: 0x04001718 RID: 5912
		[Tooltip("the new text to replace the old one with")]
		[TextArea]
		public string NewText = "Hello World";

		// Token: 0x04001719 RID: 5913
		protected string _initialText;
	}
}
