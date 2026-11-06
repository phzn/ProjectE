using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001D9 RID: 473
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Camera/Clipping Planes")]
	[FeedbackHelp("This feedback lets you control a camera's clipping planes over time. You'll need a MMCameraClippingPlanesShaker on your camera.")]
	public class MMF_CameraClippingPlanes : MMF_Feedback
	{
		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000C06 RID: 3078 RVA: 0x00034792 File Offset: 0x00032992
		// (set) Token: 0x06000C07 RID: 3079 RVA: 0x000347A0 File Offset: 0x000329A0
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

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000C08 RID: 3080 RVA: 0x000347A9 File Offset: 0x000329A9
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000C09 RID: 3081 RVA: 0x000347AC File Offset: 0x000329AC
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x000347B0 File Offset: 0x000329B0
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_CameraClippingPlanes.FeedbackTypeAuthorized)
			{
				return;
			}
			feedbacksIntensity = this.ComputeIntensity(feedbacksIntensity, position);
			MMCameraClippingPlanesShakeEvent.Trigger(this.ShakeNear, this.FeedbackDuration, this.RemapNearZero, this.RemapNearOne, this.ShakeFar, this.RemapFarZero, this.RemapFarOne, this.RelativeClippingPlanes, feedbacksIntensity, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x00034830 File Offset: 0x00032A30
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_CameraClippingPlanes.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMCameraClippingPlanesShakeEvent.Trigger(this.ShakeNear, this.FeedbackDuration, this.RemapNearZero, this.RemapNearOne, this.ShakeFar, this.RemapFarZero, this.RemapFarOne, false, 1f, null, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x00034894 File Offset: 0x00032A94
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_CameraClippingPlanes.FeedbackTypeAuthorized)
			{
				return;
			}
			MMCameraClippingPlanesShakeEvent.Trigger(this.ShakeNear, this.FeedbackDuration, this.RemapNearZero, this.RemapNearOne, this.ShakeFar, this.RemapFarZero, this.RemapFarOne, false, 1f, null, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x04000DBC RID: 3516
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000DBD RID: 3517
		[MMFInspectorGroup("Clipping Planes", true, 52, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 2f;

		// Token: 0x04000DBE RID: 3518
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04000DBF RID: 3519
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04000DC0 RID: 3520
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeClippingPlanes;

		// Token: 0x04000DC1 RID: 3521
		[MMFInspectorGroup("Near Plane", true, 53, false, false)]
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeNear = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000DC2 RID: 3522
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapNearZero = 0.01f;

		// Token: 0x04000DC3 RID: 3523
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapNearOne = 6.25f;

		// Token: 0x04000DC4 RID: 3524
		[MMFInspectorGroup("Far Plane", true, 54, false, false)]
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeFar = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000DC5 RID: 3525
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapFarZero = 1000f;

		// Token: 0x04000DC6 RID: 3526
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapFarOne = 5000f;
	}
}
