using System;
using MoreMountains.FeedbacksForThirdParty;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001DD RID: 477
	[AddComponentMenu("")]
	[FeedbackHelp("Define zoom properties : For will set the zoom to the specified parameters for a certain duration, Set will leave them like that forever. Zoom properties include the field of view, the duration of the zoom transition (in seconds) and the zoom duration (the time the camera should remain zoomed in, in seconds). For this to work, you'll need to add a MMCameraZoom component to your Camera, or a MMCinemachineZoom if you're using virtual cameras.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Camera/Camera Zoom")]
	public class MMF_CameraZoom : MMF_Feedback
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000C2D RID: 3117 RVA: 0x00035029 File Offset: 0x00033229
		// (set) Token: 0x06000C2E RID: 3118 RVA: 0x00035037 File Offset: 0x00033237
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.ZoomDuration);
			}
			set
			{
				this.ZoomDuration = value;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000C2F RID: 3119 RVA: 0x00035040 File Offset: 0x00033240
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x00035043 File Offset: 0x00033243
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x00035048 File Offset: 0x00033248
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_CameraZoom.FeedbackTypeAuthorized)
			{
				return;
			}
			MMCameraZoomEvent.Trigger(this.ZoomMode, this.ZoomFieldOfView, this.ZoomTransitionDuration, this.FeedbackDuration, this.ChannelData, this.ComputedTimescaleMode == TimescaleModes.Unscaled, false, this.RelativeFieldOfView, false, this.ZoomTween);
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x000350A0 File Offset: 0x000332A0
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_CameraZoom.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			MMCameraZoomEvent.Trigger(this.ZoomMode, this.ZoomFieldOfView, this.ZoomTransitionDuration, this.FeedbackDuration, this.ChannelData, this.ComputedTimescaleMode == TimescaleModes.Unscaled, true, false, false, this.ZoomTween);
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x000350FC File Offset: 0x000332FC
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_CameraZoom.FeedbackTypeAuthorized)
			{
				return;
			}
			MMCameraZoomEvent.Trigger(this.ZoomMode, this.ZoomFieldOfView, this.ZoomTransitionDuration, this.FeedbackDuration, this.ChannelData, this.ComputedTimescaleMode == TimescaleModes.Unscaled, false, false, true, this.ZoomTween);
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x00035150 File Offset: 0x00033350
		public override void AutomaticShakerSetup()
		{
			if (false)
			{
				MMCinemachineHelpers.AutomaticCinemachineShakersSetup(this.Owner, "CinemachineImpulse");
				return;
			}
			if ((MMCameraZoom)Object.FindObjectOfType(typeof(MMCameraZoom)) != null)
			{
				return;
			}
			Camera.main.gameObject.MMGetOrAddComponent<MMCameraZoom>();
			MMDebug.DebugLogInfo("Added a MMCameraZoom to the main camera. You're all set.", "", 3, true);
		}

		// Token: 0x04000DDA RID: 3546
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000DDB RID: 3547
		[MMFInspectorGroup("Camera Zoom", true, 72, false, false)]
		[Tooltip("the zoom mode (for : forward for TransitionDuration, static for Duration, backwards for TransitionDuration)")]
		public MMCameraZoomModes ZoomMode;

		// Token: 0x04000DDC RID: 3548
		[Tooltip("the target field of view")]
		public float ZoomFieldOfView = 30f;

		// Token: 0x04000DDD RID: 3549
		[Tooltip("the zoom transition duration")]
		public float ZoomTransitionDuration = 0.05f;

		// Token: 0x04000DDE RID: 3550
		[Tooltip("the duration for which the zoom is at max zoom")]
		public float ZoomDuration = 0.1f;

		// Token: 0x04000DDF RID: 3551
		[Tooltip("whether or not ZoomFieldOfView should add itself to the current camera's field of view value")]
		public bool RelativeFieldOfView;

		// Token: 0x04000DE0 RID: 3552
		[Header("Transition Speed")]
		[Tooltip("the animation curve to apply to the zoom transition")]
		public MMTweenType ZoomTween = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		}));
	}
}
