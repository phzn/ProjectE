using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000297 RID: 663
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback doesn't do anything by default, it's just meant as a comment, you can store text in it for future reference, maybe to remember how you setup a particular MMFeedbacks. Optionally it can also output that comment to the console on Play.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("Debug/Comment")]
	public class MMF_DebugComment : MMF_Feedback
	{
		// Token: 0x0600129B RID: 4763 RVA: 0x0005019C File Offset: 0x0004E39C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_DebugComment.FeedbackTypeAuthorized || !this.LogComment)
			{
				return;
			}
			MMDebug.DebugLogInfo(this.Comment, "", 3, true);
		}

		// Token: 0x040014F2 RID: 5362
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040014F3 RID: 5363
		[MMFInspectorGroup("Comment", true, 61, false, false)]
		[Tooltip("the comment / note associated to this feedback")]
		[TextArea(10, 30)]
		public string Comment;

		// Token: 0x040014F4 RID: 5364
		[Tooltip("if this is true, the comment will be output to the console on Play")]
		public bool LogComment;

		// Token: 0x040014F5 RID: 5365
		[Tooltip("the color of the message when in DebugLogTime mode")]
		[MMCondition("LogComment", true)]
		public Color DebugColor = Color.gray;
	}
}
