using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002A4 RID: 676
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("Audio/MMSoundManager Sound Control")]
	[FeedbackHelp("This feedback will let you control a specific sound (or sounds), targeted by SoundID, which has to match the SoundID of the sound you intially played. You will need a MMSoundManager in your scene for this to work.")]
	public class MMF_MMSoundManagerSoundControl : MMF_Feedback
	{
		// Token: 0x0600130A RID: 4874 RVA: 0x00052629 File Offset: 0x00050829
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_MMSoundManagerSoundControl.FeedbackTypeAuthorized)
			{
				return;
			}
			MMSoundManagerSoundControlEvent.Trigger(this.ControlMode, this.SoundID, null);
		}

		// Token: 0x040015C6 RID: 5574
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040015C7 RID: 5575
		[MMFInspectorGroup("MMSoundManager Sound Control", true, 30, false, false)]
		[Tooltip("the action to trigger on the specified sound")]
		public MMSoundManagerSoundControlEventTypes ControlMode;

		// Token: 0x040015C8 RID: 5576
		[Tooltip("the ID of the sound, has to match the one you specified when playing it")]
		public int SoundID;

		// Token: 0x040015C9 RID: 5577
		protected AudioSource _targetAudioSource;
	}
}
