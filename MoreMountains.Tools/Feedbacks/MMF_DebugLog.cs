using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000298 RID: 664
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you output a message to the console, using a custom MM debug method, or Log, Assertion, Error or Warning logs.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("Debug/Log")]
	public class MMF_DebugLog : MMF_Feedback
	{
		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x0600129E RID: 4766 RVA: 0x000501E3 File Offset: 0x0004E3E3
		public override float FeedbackDuration
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x000501EC File Offset: 0x0004E3EC
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_DebugLog.FeedbackTypeAuthorized)
			{
				return;
			}
			switch (this.DebugLogMode)
			{
			case MMF_DebugLog.DebugLogModes.DebugLogTime:
			{
				string text = "#" + ColorUtility.ToHtmlStringRGB(this.DebugColor);
				MMDebug.DebugLogTime(this.DebugMessage, text, 3, this.DisplayFrameCount);
				break;
			}
			case MMF_DebugLog.DebugLogModes.Log:
				Debug.Log(this.DebugMessage);
				return;
			case MMF_DebugLog.DebugLogModes.Assertion:
				break;
			case MMF_DebugLog.DebugLogModes.Error:
				Debug.LogError(this.DebugMessage);
				return;
			case MMF_DebugLog.DebugLogModes.Warning:
				Debug.LogWarning(this.DebugMessage);
				return;
			default:
				return;
			}
		}

		// Token: 0x040014F6 RID: 5366
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040014F7 RID: 5367
		[MMFInspectorGroup("Debug", true, 17, false, false)]
		[Tooltip("the selected debug mode")]
		public MMF_DebugLog.DebugLogModes DebugLogMode;

		// Token: 0x040014F8 RID: 5368
		[Tooltip("the message to display")]
		[TextArea]
		public string DebugMessage = "YOUR DEBUG MESSAGE GOES HERE";

		// Token: 0x040014F9 RID: 5369
		[Tooltip("the color of the message when in DebugLogTime mode")]
		[MMFEnumCondition("DebugLogMode", new int[] { 0 })]
		public Color DebugColor = Color.cyan;

		// Token: 0x040014FA RID: 5370
		[Tooltip("whether or not to display the frame count when in DebugLogTime mode")]
		[MMFEnumCondition("DebugLogMode", new int[] { 0 })]
		public bool DisplayFrameCount = true;

		// Token: 0x0200052C RID: 1324
		public enum DebugLogModes
		{
			// Token: 0x040023C0 RID: 9152
			DebugLogTime,
			// Token: 0x040023C1 RID: 9153
			Log,
			// Token: 0x040023C2 RID: 9154
			Assertion,
			// Token: 0x040023C3 RID: 9155
			Error,
			// Token: 0x040023C4 RID: 9156
			Warning
		}
	}
}
