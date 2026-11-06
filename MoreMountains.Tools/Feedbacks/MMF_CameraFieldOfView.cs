using System;
using MoreMountains.FeedbacksForThirdParty;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001DA RID: 474
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Camera/Field of View")]
	[FeedbackHelp("This feedback lets you control a camera's field of view over time. You'll need a MMCameraFieldOfViewShaker on your camera.")]
	public class MMF_CameraFieldOfView : MMF_Feedback
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000C0F RID: 3087 RVA: 0x000349F6 File Offset: 0x00032BF6
		// (set) Token: 0x06000C10 RID: 3088 RVA: 0x00034A04 File Offset: 0x00032C04
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

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000C11 RID: 3089 RVA: 0x00034A0D File Offset: 0x00032C0D
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000C12 RID: 3090 RVA: 0x00034A10 File Offset: 0x00032C10
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000C13 RID: 3091 RVA: 0x00034A13 File Offset: 0x00032C13
		public override bool ForceInitialValueDelayed
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000C14 RID: 3092 RVA: 0x00034A16 File Offset: 0x00032C16
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x00034A1C File Offset: 0x00032C1C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_CameraFieldOfView.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			MMCameraFieldOfViewShakeEvent.Trigger(this.ShakeFieldOfView, this.FeedbackDuration, this.RemapFieldOfViewZero, this.RemapFieldOfViewOne, this.RelativeFieldOfView, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x00034A88 File Offset: 0x00032C88
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_CameraFieldOfView.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMCameraFieldOfViewShakeEvent.Trigger(this.ShakeFieldOfView, this.FeedbackDuration, this.RemapFieldOfViewZero, this.RemapFieldOfViewOne, false, 1f, null, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x00034AD8 File Offset: 0x00032CD8
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_CameraFieldOfView.FeedbackTypeAuthorized)
			{
				return;
			}
			MMCameraFieldOfViewShakeEvent.Trigger(this.ShakeFieldOfView, this.FeedbackDuration, this.RemapFieldOfViewZero, this.RemapFieldOfViewOne, false, 1f, null, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x00034B20 File Offset: 0x00032D20
		public override void AutomaticShakerSetup()
		{
			if (false)
			{
				MMCinemachineHelpers.AutomaticCinemachineShakersSetup(this.Owner, "CinemachineImpulse");
				return;
			}
			if ((MMCameraFieldOfViewShaker)Object.FindObjectOfType(typeof(MMCameraFieldOfViewShaker)) != null)
			{
				return;
			}
			Camera.main.gameObject.MMGetOrAddComponent<MMCameraFieldOfViewShaker>();
			MMDebug.DebugLogInfo("Added a MMCameraFieldOfViewShaker to the main camera. You're all set.", "", 3, true);
		}

		// Token: 0x04000DC7 RID: 3527
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000DC8 RID: 3528
		[MMFInspectorGroup("Field of View", true, 37, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 2f;

		// Token: 0x04000DC9 RID: 3529
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04000DCA RID: 3530
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04000DCB RID: 3531
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeFieldOfView;

		// Token: 0x04000DCC RID: 3532
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeFieldOfView = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000DCD RID: 3533
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 179f)]
		public float RemapFieldOfViewZero = 60f;

		// Token: 0x04000DCE RID: 3534
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 179f)]
		public float RemapFieldOfViewOne = 120f;
	}
}
