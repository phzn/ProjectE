using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002A0 RID: 672
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will trigger a MMGameEvent of the specified name when played")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("Events/MMGameEvent")]
	public class MMF_MMGameEvent : MMF_Feedback
	{
		// Token: 0x060012EB RID: 4843 RVA: 0x00051868 File Offset: 0x0004FA68
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_MMGameEvent.FeedbackTypeAuthorized)
			{
				return;
			}
			MMGameEvent.Trigger(this.MMGameEventName, this.IntParameter, this.Vector2Parameter, this.Vector3Parameter, this.BoolParameter, this.StringParameter);
		}

		// Token: 0x04001578 RID: 5496
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001579 RID: 5497
		[MMFInspectorGroup("MMGameEvent", true, 57, true, false)]
		public string MMGameEventName;

		// Token: 0x0400157A RID: 5498
		[MMFInspectorGroup("Optional Payload", true, 58, true, false)]
		public int IntParameter;

		// Token: 0x0400157B RID: 5499
		public Vector2 Vector2Parameter;

		// Token: 0x0400157C RID: 5500
		public Vector3 Vector3Parameter;

		// Token: 0x0400157D RID: 5501
		public bool BoolParameter;

		// Token: 0x0400157E RID: 5502
		public string StringParameter;
	}
}
