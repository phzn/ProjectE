using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000215 RID: 533
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback triggers a MMTimeScaleEvent, which, if you have a MMTimeManager object in your scene, will be caught and used to modify the timescale according to the specified settings. These settings are the new timescale (0.5 will be twice slower than normal, 2 twice faster, etc), the duration of the timescale modification, and the optional speed at which to transition between normal and altered time scale.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Time/Timescale Modifier")]
	public class MMF_TimescaleModifier : MMF_Feedback
	{
		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000EA3 RID: 3747 RVA: 0x000412E1 File Offset: 0x0003F4E1
		// (set) Token: 0x06000EA4 RID: 3748 RVA: 0x000412EF File Offset: 0x0003F4EF
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.TimeScaleDuration);
			}
			set
			{
				this.TimeScaleDuration = value;
			}
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x000412F8 File Offset: 0x0003F4F8
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_TimescaleModifier.FeedbackTypeAuthorized)
			{
				return;
			}
			switch (this.Mode)
			{
			case MMF_TimescaleModifier.Modes.Shake:
				MMTimeScaleEvent.Trigger(MMTimeScaleMethods.For, this.TimeScale, this.FeedbackDuration, this.TimeScaleLerp, this.TimeScaleLerpSpeed, false, this.TimescaleLerpMode, this.TimescaleLerpCurve, this.TimescaleLerpDuration, this.TimeScaleLerpOnReset, this.TimescaleLerpCurveOnReset, this.TimescaleLerpDurationOnReset);
				return;
			case MMF_TimescaleModifier.Modes.Change:
				MMTimeScaleEvent.Trigger(MMTimeScaleMethods.For, this.TimeScale, 0f, this.TimeScaleLerp, this.TimeScaleLerpSpeed, true, this.TimescaleLerpMode, this.TimescaleLerpCurve, this.TimescaleLerpDuration, this.TimeScaleLerpOnReset, this.TimescaleLerpCurveOnReset, this.TimescaleLerpDurationOnReset);
				return;
			case MMF_TimescaleModifier.Modes.Reset:
				MMTimeScaleEvent.Trigger(MMTimeScaleMethods.Reset, this.TimeScale, 0f, false, 0f, true, MMTimeScaleLerpModes.Speed, null, 0.2f, false, null, 0.2f);
				return;
			case MMF_TimescaleModifier.Modes.Unfreeze:
				MMTimeScaleEvent.Trigger(MMTimeScaleMethods.Unfreeze, this.TimeScale, 0f, false, 0f, true, MMTimeScaleLerpModes.Speed, null, 0.2f, false, null, 0.2f);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x00041408 File Offset: 0x0003F608
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_TimescaleModifier.FeedbackTypeAuthorized || !this.ResetTimescaleOnStop)
			{
				return;
			}
			MMTimeScaleEvent.Trigger(MMTimeScaleMethods.Reset, this.TimeScale, 0f, false, 0f, true, MMTimeScaleLerpModes.Speed, null, 0.2f, false, null, 0.2f);
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x00041454 File Offset: 0x0003F654
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_TimescaleModifier.FeedbackTypeAuthorized)
			{
				return;
			}
			MMTimeScaleEvent.Trigger(MMTimeScaleMethods.Reset, this.TimeScale, 0f, false, 0f, true, MMTimeScaleLerpModes.Speed, null, 0.2f, false, null, 0.2f);
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x00041497 File Offset: 0x0003F697
		public override void AutomaticShakerSetup()
		{
			if (this.Owner.gameObject.MMFindOrCreateObjectOfType<MMTimeManager>("MMTimeManager", null, false).Item2)
			{
				MMDebug.DebugLogInfo("Added a MMTimeManager to the scene. You're all set.", "", 3, true);
			}
		}

		// Token: 0x040010F0 RID: 4336
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x040010F1 RID: 4337
		[MMFInspectorGroup("Timescale Modifier", true, 63, false, false)]
		[Tooltip("the selected mode : shake : changes the timescale for a certain duration- change : sets the timescale to a new value, forever (until you change it again)- reset : resets the timescale to its previous value")]
		public MMF_TimescaleModifier.Modes Mode;

		// Token: 0x040010F2 RID: 4338
		[Tooltip("the new timescale to apply")]
		public float TimeScale = 0.5f;

		// Token: 0x040010F3 RID: 4339
		[Tooltip("the duration of the timescale modification")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public float TimeScaleDuration = 1f;

		// Token: 0x040010F4 RID: 4340
		[Tooltip("whether to reset the timescale on Stop or not")]
		public bool ResetTimescaleOnStop;

		// Token: 0x040010F5 RID: 4341
		[MMFInspectorGroup("Interpolation", true, 63, false, false)]
		[Tooltip("whether or not we should lerp the timescale")]
		public bool TimeScaleLerp;

		// Token: 0x040010F6 RID: 4342
		[Tooltip("whether to lerp over a set duration, or at a certain speed")]
		public MMTimeScaleLerpModes TimescaleLerpMode;

		// Token: 0x040010F7 RID: 4343
		[Tooltip("in Speed mode, the speed at which to lerp the timescale")]
		[MMFEnumCondition("TimescaleLerpMode", new int[] { 0 })]
		public float TimeScaleLerpSpeed = 1f;

		// Token: 0x040010F8 RID: 4344
		[Tooltip("in Duration mode, the curve to use to lerp the timescale")]
		[MMFEnumCondition("TimescaleLerpMode", new int[] { 1 })]
		public MMTweenType TimescaleLerpCurve = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		}));

		// Token: 0x040010F9 RID: 4345
		[Tooltip("in Duration mode, the duration of the timescale interpolation, in unscaled time seconds")]
		[MMFEnumCondition("TimescaleLerpMode", new int[] { 1 })]
		public float TimescaleLerpDuration = 1f;

		// Token: 0x040010FA RID: 4346
		[Tooltip("whether or not we should lerp the timescale as it goes back to normal afterwards")]
		[MMFEnumCondition("TimescaleLerpMode", new int[] { 1 })]
		public bool TimeScaleLerpOnReset;

		// Token: 0x040010FB RID: 4347
		[Tooltip("in Duration mode, the curve to use to lerp the timescale")]
		[MMFEnumCondition("TimescaleLerpMode", new int[] { 1 })]
		public MMTweenType TimescaleLerpCurveOnReset = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		}));

		// Token: 0x040010FC RID: 4348
		[Tooltip("in Duration mode, the duration of the timescale interpolation, in unscaled time seconds")]
		[MMFEnumCondition("TimescaleLerpMode", new int[] { 1 })]
		public float TimescaleLerpDurationOnReset = 1f;

		// Token: 0x02000502 RID: 1282
		public enum Modes
		{
			// Token: 0x0400236B RID: 9067
			Shake,
			// Token: 0x0400236C RID: 9068
			Change,
			// Token: 0x0400236D RID: 9069
			Reset,
			// Token: 0x0400236E RID: 9070
			Unfreeze
		}
	}
}
