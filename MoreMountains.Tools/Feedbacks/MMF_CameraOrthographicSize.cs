using System;
using MoreMountains.FeedbacksForThirdParty;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001DB RID: 475
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Camera/Orthographic Size")]
	[FeedbackHelp("This feedback lets you control a camera's orthographic size over time. You'll need a MMCameraOrthographicSizeShaker on your camera.")]
	public class MMF_CameraOrthographicSize : MMF_Feedback
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000C1B RID: 3099 RVA: 0x00034C1D File Offset: 0x00032E1D
		// (set) Token: 0x06000C1C RID: 3100 RVA: 0x00034C2B File Offset: 0x00032E2B
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.Duration);
			}
			set
			{
				this.Duration = value;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000C1D RID: 3101 RVA: 0x00034C34 File Offset: 0x00032E34
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x00034C38 File Offset: 0x00032E38
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_CameraOrthographicSize.FeedbackTypeAuthorized)
			{
				return;
			}
			MMCameraOrthographicSizeShakeEvent.Trigger(this.ShakeOrthographicSize, this.FeedbackDuration, this.RemapOrthographicSizeZero, this.RemapOrthographicSizeOne, this.RelativeOrthographicSize, feedbacksIntensity, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x00034C9C File Offset: 0x00032E9C
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_CameraOrthographicSize.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMCameraOrthographicSizeShakeEvent.Trigger(this.ShakeOrthographicSize, this.FeedbackDuration, this.RemapOrthographicSizeZero, this.RemapOrthographicSizeOne, false, 1f, null, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x00034CEC File Offset: 0x00032EEC
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_CameraOrthographicSize.FeedbackTypeAuthorized)
			{
				return;
			}
			MMCameraOrthographicSizeShakeEvent.Trigger(this.ShakeOrthographicSize, this.FeedbackDuration, this.RemapOrthographicSizeZero, this.RemapOrthographicSizeOne, false, 1f, null, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x00034D34 File Offset: 0x00032F34
		public override void AutomaticShakerSetup()
		{
			if (false)
			{
				MMCinemachineHelpers.AutomaticCinemachineShakersSetup(this.Owner, "CinemachineImpulse");
				return;
			}
			if ((MMCameraOrthographicSizeShaker)Object.FindObjectOfType(typeof(MMCameraOrthographicSizeShaker)) != null)
			{
				return;
			}
			Camera.main.gameObject.MMGetOrAddComponent<MMCameraOrthographicSizeShaker>();
			MMDebug.DebugLogInfo("Added a MMCameraOrthographicSizeShaker to the main camera. You're all set.", "", 3, true);
		}

		// Token: 0x04000DCF RID: 3535
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000DD0 RID: 3536
		[MMFInspectorGroup("Orthographic Size", true, 41, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 2f;

		// Token: 0x04000DD1 RID: 3537
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04000DD2 RID: 3538
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04000DD3 RID: 3539
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeOrthographicSize;

		// Token: 0x04000DD4 RID: 3540
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeOrthographicSize = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000DD5 RID: 3541
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapOrthographicSizeZero = 5f;

		// Token: 0x04000DD6 RID: 3542
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapOrthographicSizeOne = 10f;
	}
}
