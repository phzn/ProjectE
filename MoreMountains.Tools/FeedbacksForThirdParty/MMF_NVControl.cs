using System;
using Lofelt.NiceVibrations;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000318 RID: 792
	[AddComponentMenu("")]
	[FeedbackPath("Haptics/Haptic Control")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.NiceVibrations", null)]
	[FeedbackHelp("Add this feedback to interact with haptics at a global level, stopping them all, enabling or disabling them, adjusting their global level or initializing/release the haptic engine.")]
	public class MMF_NVControl : MMF_Feedback
	{
		// Token: 0x060015FD RID: 5629 RVA: 0x0005D6B4 File Offset: 0x0005B8B4
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_NVControl.FeedbackTypeAuthorized)
			{
				return;
			}
			switch (this.ControlType)
			{
			case MMF_NVControl.ControlTypes.Stop:
				HapticController.Stop();
				return;
			case MMF_NVControl.ControlTypes.EnableHaptics:
				HapticController.hapticsEnabled = true;
				return;
			case MMF_NVControl.ControlTypes.DisableHaptics:
				HapticController.hapticsEnabled = false;
				return;
			case MMF_NVControl.ControlTypes.AdjustHapticsLevel:
				HapticController.outputLevel = this.OutputLevel;
				return;
			case MMF_NVControl.ControlTypes.Initialize:
				LofeltHaptics.Initialize();
				HapticController.Init();
				return;
			case MMF_NVControl.ControlTypes.Release:
				LofeltHaptics.Release();
				return;
			default:
				return;
			}
		}

		// Token: 0x04001943 RID: 6467
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001944 RID: 6468
		[MMFInspectorGroup("Haptic Control", true, 24, false, false)]
		[Tooltip("the type of control order to trigger when playing this feedback - check Nice Vibrations' documentation for the exact behaviour of these")]
		public MMF_NVControl.ControlTypes ControlType;

		// Token: 0x04001945 RID: 6469
		[Tooltip("the output level when in AdjustHapticsLevel mode")]
		[MMFEnumCondition("ControlType", new int[] { 3 })]
		public float OutputLevel = 1f;

		// Token: 0x0200056E RID: 1390
		public enum ControlTypes
		{
			// Token: 0x0400249E RID: 9374
			Stop,
			// Token: 0x0400249F RID: 9375
			EnableHaptics,
			// Token: 0x040024A0 RID: 9376
			DisableHaptics,
			// Token: 0x040024A1 RID: 9377
			AdjustHapticsLevel,
			// Token: 0x040024A2 RID: 9378
			Initialize,
			// Token: 0x040024A3 RID: 9379
			Release
		}
	}
}
