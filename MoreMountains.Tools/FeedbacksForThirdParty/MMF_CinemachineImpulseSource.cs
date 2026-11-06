using System;
using Cinemachine;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x020002E3 RID: 739
	[AddComponentMenu("")]
	[FeedbackPath("Camera/Cinemachine Impulse Source")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks.Cinemachine", null)]
	[FeedbackHelp("This feedback lets you generate an impulse on a Cinemachine Impulse source. You'll need a Cinemachine Impulse Listener on your camera for this to work.")]
	public class MMF_CinemachineImpulseSource : MMF_Feedback
	{
		// Token: 0x17000257 RID: 599
		// (get) Token: 0x060014AF RID: 5295 RVA: 0x000588D8 File Offset: 0x00056AD8
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x000588DB File Offset: 0x00056ADB
		protected override void AutomateTargetAcquisition()
		{
			this.ImpulseSource = this.FindAutomatedTarget<CinemachineImpulseSource>();
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x000588E9 File Offset: 0x00056AE9
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_CinemachineImpulseSource.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.ImpulseSource != null)
			{
				this.ImpulseSource.GenerateImpulse(this.Velocity);
			}
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x0005891A File Offset: 0x00056B1A
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_CinemachineImpulseSource.FeedbackTypeAuthorized || !this.ClearImpulseOnStop)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			CinemachineImpulseManager.Instance.Clear();
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x00058946 File Offset: 0x00056B46
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_CinemachineImpulseSource.FeedbackTypeAuthorized)
			{
				return;
			}
			CinemachineImpulseManager.Instance.Clear();
		}

		// Token: 0x040017A8 RID: 6056
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040017A9 RID: 6057
		[MMFInspectorGroup("Cinemachine Impulse Source", true, 28, false, false)]
		[Tooltip("the velocity to apply to the impulse shake")]
		public Vector3 Velocity = new Vector3(1f, 1f, 1f);

		// Token: 0x040017AA RID: 6058
		[Tooltip("the impulse definition to broadcast")]
		public CinemachineImpulseSource ImpulseSource;

		// Token: 0x040017AB RID: 6059
		[Tooltip("whether or not to clear impulses (stopping camera shakes) when the Stop method is called on that feedback")]
		public bool ClearImpulseOnStop;
	}
}
