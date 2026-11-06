using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001E8 RID: 488
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will freeze the timescale for the specified duration (in seconds). I usually go with 0.01s or 0.02s, but feel free to tweak it to your liking. It requires a MMTimeManager in your scene to work.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Time/Freeze Frame")]
	public class MMF_FreezeFrame : MMF_Feedback
	{
		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000C8D RID: 3213 RVA: 0x00036D5D File Offset: 0x00034F5D
		// (set) Token: 0x06000C8E RID: 3214 RVA: 0x00036D6B File Offset: 0x00034F6B
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.FreezeFrameDuration);
			}
			set
			{
				this.FreezeFrameDuration = value;
			}
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x00036D74 File Offset: 0x00034F74
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_FreezeFrame.FeedbackTypeAuthorized)
			{
				return;
			}
			if (Time.timeScale < this.MinimumTimescaleThreshold)
			{
				return;
			}
			MMFreezeFrameEvent.Trigger(this.FeedbackDuration);
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x00036D9F File Offset: 0x00034F9F
		public override void AutomaticShakerSetup()
		{
			if (this.Owner.gameObject.MMFindOrCreateObjectOfType<MMTimeManager>("MMTimeManager", null, false).Item2)
			{
				MMDebug.DebugLogInfo("Added a MMTimeManager to the scene. You're all set.", "", 3, true);
			}
		}

		// Token: 0x04000E51 RID: 3665
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000E52 RID: 3666
		[MMFInspectorGroup("Freeze Frame", true, 63, false, false)]
		[Tooltip("the duration of the freeze frame")]
		public float FreezeFrameDuration = 0.02f;

		// Token: 0x04000E53 RID: 3667
		[Tooltip("the minimum value the timescale should be at for this freeze frame to happen. This can be useful to avoid triggering freeze frames when the timescale is already frozen.")]
		public float MinimumTimescaleThreshold = 0.1f;
	}
}
