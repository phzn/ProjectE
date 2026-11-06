using System;
using MoreMountains.FeedbacksForThirdParty;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001DC RID: 476
	[AddComponentMenu("")]
	[FeedbackHelp("Define camera shake properties (duration in seconds, amplitude and frequency), and this will broadcast a MMCameraShakeEvent with these same settings. You'll need to add a MMCameraShaker on your camera for this to work (or a MMCinemachineCameraShaker component on your virtual camera if you're using Cinemachine). Note that although this event and system was built for cameras in mind, you could technically use it to shake other objects as well.")]
	[FeedbackPath("Camera/Camera Shake")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	public class MMF_CameraShake : MMF_Feedback
	{
		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000C24 RID: 3108 RVA: 0x00034E31 File Offset: 0x00033031
		// (set) Token: 0x06000C25 RID: 3109 RVA: 0x00034E44 File Offset: 0x00033044
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.CameraShakeProperties.Duration);
			}
			set
			{
				this.CameraShakeProperties.Duration = value;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000C26 RID: 3110 RVA: 0x00034E52 File Offset: 0x00033052
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000C27 RID: 3111 RVA: 0x00034E55 File Offset: 0x00033055
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x00034E58 File Offset: 0x00033058
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_CameraShake.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMCameraShakeEvent.Trigger(this.FeedbackDuration, this.CameraShakeProperties.Amplitude * num, this.CameraShakeProperties.Frequency, this.CameraShakeProperties.AmplitudeX * num, this.CameraShakeProperties.AmplitudeY * num, this.CameraShakeProperties.AmplitudeZ * num, this.RepeatUntilStopped, this.ChannelData, this.ComputedTimescaleMode == TimescaleModes.Unscaled);
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x00034EDD File Offset: 0x000330DD
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_CameraShake.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMCameraShakeStopEvent.Trigger(this.ChannelData);
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x00034F04 File Offset: 0x00033104
		public override void AutomaticShakerSetup()
		{
			if (false)
			{
				MMCinemachineHelpers.AutomaticCinemachineShakersSetup(this.Owner, "CinemachineImpulse");
				return;
			}
			if ((MMCameraShaker)Object.FindObjectOfType(typeof(MMCameraShaker)) != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("CameraRig");
			gameObject.transform.position = Camera.main.transform.position;
			GameObject gameObject2 = new GameObject("CameraShaker");
			gameObject2.transform.SetParent(gameObject.transform);
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.AddComponent<MMCameraShaker>();
			MMWiggle component = gameObject2.GetComponent<MMWiggle>();
			component.PositionActive = true;
			component.PositionWiggleProperties = new WiggleProperties();
			component.PositionWiggleProperties.WigglePermitted = false;
			component.PositionWiggleProperties.WiggleType = WiggleTypes.Noise;
			Camera.main.transform.SetParent(gameObject2.transform);
			MMDebug.DebugLogInfo("Added a CameraRig to the main camera. You're all set.", "", 3, true);
		}

		// Token: 0x04000DD7 RID: 3543
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000DD8 RID: 3544
		[MMFInspectorGroup("Camera Shake", true, 57, false, false)]
		[Tooltip("whether or not this shake should repeat forever, until stopped")]
		public bool RepeatUntilStopped;

		// Token: 0x04000DD9 RID: 3545
		[Tooltip("the properties of the shake (duration, intensity, frequenc)")]
		public MMCameraShakeProperties CameraShakeProperties = new MMCameraShakeProperties(0.1f, 0.2f, 40f, 0f, 0f, 0f);
	}
}
