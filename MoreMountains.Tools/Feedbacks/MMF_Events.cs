using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001E4 RID: 484
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback allows you to bind any type of Unity events to this feebdack's Play, Stop, Initialization and Reset methods.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Events/Unity Events")]
	public class MMF_Events : MMF_Feedback
	{
		// Token: 0x06000C67 RID: 3175 RVA: 0x00035EEC File Offset: 0x000340EC
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.Active && this.InitializationEvents != null)
			{
				this.InitializationEvents.Invoke();
			}
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x00035F10 File Offset: 0x00034110
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Events.FeedbackTypeAuthorized || this.PlayEvents == null)
			{
				return;
			}
			this.PlayEvents.Invoke();
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x00035F35 File Offset: 0x00034135
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Events.FeedbackTypeAuthorized || this.StopEvents == null)
			{
				return;
			}
			this.StopEvents.Invoke();
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x00035F5A File Offset: 0x0003415A
		protected override void CustomReset()
		{
			if (!this.Active || !MMF_Events.FeedbackTypeAuthorized || this.ResetEvents == null)
			{
				return;
			}
			base.CustomReset();
			this.ResetEvents.Invoke();
		}

		// Token: 0x04000E27 RID: 3623
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000E28 RID: 3624
		[MMFInspectorGroup("Events", true, 44, false, false)]
		[Tooltip("the events to trigger when the feedback is played")]
		public UnityEvent PlayEvents;

		// Token: 0x04000E29 RID: 3625
		[Tooltip("the events to trigger when the feedback is stopped")]
		public UnityEvent StopEvents;

		// Token: 0x04000E2A RID: 3626
		[Tooltip("the events to trigger when the feedback is initialized")]
		public UnityEvent InitializationEvents;

		// Token: 0x04000E2B RID: 3627
		[Tooltip("the events to trigger when the feedback is reset")]
		public UnityEvent ResetEvents;
	}
}
