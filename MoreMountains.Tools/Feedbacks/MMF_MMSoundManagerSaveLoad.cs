using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002A2 RID: 674
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.MMTools", null)]
	[FeedbackPath("Audio/MMSoundManager Save and Load")]
	[FeedbackHelp("This feedback will let you trigger save, load, and reset on MMSoundManager settings. You will need a MMSoundManager in your scene for this to work.")]
	public class MMF_MMSoundManagerSaveLoad : MMF_Feedback
	{
		// Token: 0x060012F1 RID: 4849 RVA: 0x00051930 File Offset: 0x0004FB30
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_MMSoundManagerSaveLoad.FeedbackTypeAuthorized)
			{
				return;
			}
			switch (this.Mode)
			{
			case MMF_MMSoundManagerSaveLoad.Modes.Save:
				MMSoundManagerEvent.Trigger(MMSoundManagerEventTypes.SaveSettings);
				return;
			case MMF_MMSoundManagerSaveLoad.Modes.Load:
				MMSoundManagerEvent.Trigger(MMSoundManagerEventTypes.LoadSettings);
				return;
			case MMF_MMSoundManagerSaveLoad.Modes.Reset:
				MMSoundManagerEvent.Trigger(MMSoundManagerEventTypes.ResetSettings);
				return;
			default:
				return;
			}
		}

		// Token: 0x04001581 RID: 5505
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001582 RID: 5506
		[MMFInspectorGroup("MMSoundManager Save and Load", true, 30, false, false)]
		[Tooltip("the selected mode to interact with save settings on the MMSoundManager")]
		public MMF_MMSoundManagerSaveLoad.Modes Mode;

		// Token: 0x02000539 RID: 1337
		public enum Modes
		{
			// Token: 0x040023F8 RID: 9208
			Save,
			// Token: 0x040023F9 RID: 9209
			Load,
			// Token: 0x040023FA RID: 9210
			Reset
		}
	}
}
