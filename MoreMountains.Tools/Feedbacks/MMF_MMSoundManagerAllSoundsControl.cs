using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002A1 RID: 673
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("Audio/MMSoundManager All Sounds Control")]
	[FeedbackHelp("A feedback used to control all sounds playing on the MMSoundManager at once. It'll let you pause, play, stop and free (stop and returns the audiosource to the pool) sounds. You will need a MMSoundManager in your scene for this to work.")]
	public class MMF_MMSoundManagerAllSoundsControl : MMF_Feedback
	{
		// Token: 0x060012EE RID: 4846 RVA: 0x000518B4 File Offset: 0x0004FAB4
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_MMSoundManagerAllSoundsControl.FeedbackTypeAuthorized)
			{
				return;
			}
			switch (this.ControlMode)
			{
			case MMSoundManagerAllSoundsControlEventTypes.Pause:
				MMSoundManagerAllSoundsControlEvent.Trigger(MMSoundManagerAllSoundsControlEventTypes.Pause);
				return;
			case MMSoundManagerAllSoundsControlEventTypes.Play:
				MMSoundManagerAllSoundsControlEvent.Trigger(MMSoundManagerAllSoundsControlEventTypes.Play);
				return;
			case MMSoundManagerAllSoundsControlEventTypes.Stop:
				MMSoundManagerAllSoundsControlEvent.Trigger(MMSoundManagerAllSoundsControlEventTypes.Stop);
				return;
			case MMSoundManagerAllSoundsControlEventTypes.Free:
				MMSoundManagerAllSoundsControlEvent.Trigger(MMSoundManagerAllSoundsControlEventTypes.Free);
				return;
			case MMSoundManagerAllSoundsControlEventTypes.FreeAllButPersistent:
				MMSoundManagerAllSoundsControlEvent.Trigger(MMSoundManagerAllSoundsControlEventTypes.FreeAllButPersistent);
				return;
			case MMSoundManagerAllSoundsControlEventTypes.FreeAllLooping:
				MMSoundManagerAllSoundsControlEvent.Trigger(MMSoundManagerAllSoundsControlEventTypes.FreeAllLooping);
				return;
			default:
				return;
			}
		}

		// Token: 0x0400157F RID: 5503
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001580 RID: 5504
		[MMFInspectorGroup("MMSoundManager All Sounds Control", true, 30, false, false)]
		[Tooltip("The selected control mode")]
		public MMSoundManagerAllSoundsControlEventTypes ControlMode;
	}
}
