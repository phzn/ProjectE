using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000208 RID: 520
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Transform/Scale Shake")]
	[FeedbackHelp("This feedback lets you emit a ScaleShake event. This will be caught by MMScaleShakers (on the specified channel). Scale shakers, as the name suggests, are used to shake the scale of a transform, along a direction, with optional noise and other fine control options.")]
	public class MMF_ScaleShake : MMF_Feedback
	{
		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000E07 RID: 3591 RVA: 0x0003E38F File Offset: 0x0003C58F
		// (set) Token: 0x06000E08 RID: 3592 RVA: 0x0003E39D File Offset: 0x0003C59D
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

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000E09 RID: 3593 RVA: 0x0003E3A6 File Offset: 0x0003C5A6
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000E0A RID: 3594 RVA: 0x0003E3A9 File Offset: 0x0003C5A9
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000E0B RID: 3595 RVA: 0x0003E3AC File Offset: 0x0003C5AC
		public override bool HasRange
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000E0C RID: 3596 RVA: 0x0003E3AF File Offset: 0x0003C5AF
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x0003E3B2 File Offset: 0x0003C5B2
		protected override void AutomateTargetAcquisition()
		{
			this.TargetShaker = this.FindAutomatedTarget<MMScaleShaker>();
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x0003E3C0 File Offset: 0x0003C5C0
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ScaleShake.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			if (this.TargetShaker == null)
			{
				MMScaleShakeEvent.Trigger(this.Duration, this.ShakeSpeed, this.ShakeRange, this.ShakeMainDirection, this.RandomizeDirection, this.ShakeAltDirection, this.RandomizeDirectionOnPlay, this.AddDirectionalNoise, this.DirectionalNoiseStrengthMin, this.DirectionalNoiseStrengthMax, this.RandomnessSeed, this.RandomizeSeedOnShake, this.UseAttenuation, this.AttenuationCurve, this.UseRange, this.RangeDistance, this.UseRangeFalloff, this.RangeFalloff, this.RemapRangeFalloff, position, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
				return;
			}
			MMScaleShaker targetShaker = this.TargetShaker;
			if (targetShaker == null)
			{
				return;
			}
			targetShaker.OnMMScaleShakeEvent(this.Duration, this.ShakeSpeed, this.ShakeRange, this.ShakeMainDirection, this.RandomizeDirection, this.ShakeAltDirection, this.RandomizeDirectionOnPlay, this.AddDirectionalNoise, this.DirectionalNoiseStrengthMin, this.DirectionalNoiseStrengthMax, this.RandomnessSeed, this.RandomizeSeedOnShake, this.UseAttenuation, this.AttenuationCurve, this.UseRange, this.RangeDistance, this.UseRangeFalloff, this.RangeFalloff, this.RemapRangeFalloff, position, num, this.TargetShaker.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x0003E53C File Offset: 0x0003C73C
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_ScaleShake.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			if (this.TargetShaker == null)
			{
				MMScaleShakeEvent.Trigger(this.Duration, this.ShakeSpeed, this.ShakeRange, this.ShakeMainDirection, this.RandomizeDirection, this.ShakeAltDirection, this.RandomizeDirectionOnPlay, this.AddDirectionalNoise, this.DirectionalNoiseStrengthMin, this.DirectionalNoiseStrengthMax, this.RandomnessSeed, this.RandomizeSeedOnShake, this.UseAttenuation, this.AttenuationCurve, false, 0f, false, null, default(Vector2), default(Vector3), 1f, null, true, true, true, TimescaleModes.Scaled, true, false);
				return;
			}
			MMScaleShaker targetShaker = this.TargetShaker;
			if (targetShaker == null)
			{
				return;
			}
			float duration = this.Duration;
			float shakeSpeed = this.ShakeSpeed;
			float shakeRange = this.ShakeRange;
			Vector3 shakeMainDirection = this.ShakeMainDirection;
			bool randomizeDirection = this.RandomizeDirection;
			Vector3 shakeAltDirection = this.ShakeAltDirection;
			bool randomizeDirectionOnPlay = this.RandomizeDirectionOnPlay;
			bool addDirectionalNoise = this.AddDirectionalNoise;
			Vector3 directionalNoiseStrengthMin = this.DirectionalNoiseStrengthMin;
			Vector3 directionalNoiseStrengthMax = this.DirectionalNoiseStrengthMax;
			Vector3 randomnessSeed = this.RandomnessSeed;
			bool randomizeSeedOnShake = this.RandomizeSeedOnShake;
			bool useAttenuation = this.UseAttenuation;
			AnimationCurve attenuationCurve = this.AttenuationCurve;
			bool flag = false;
			float num = 0f;
			bool flag2 = false;
			AnimationCurve animationCurve = null;
			MMChannelData channelData = this.TargetShaker.ChannelData;
			targetShaker.OnMMScaleShakeEvent(duration, shakeSpeed, shakeRange, shakeMainDirection, randomizeDirection, shakeAltDirection, randomizeDirectionOnPlay, addDirectionalNoise, directionalNoiseStrengthMin, directionalNoiseStrengthMax, randomnessSeed, randomizeSeedOnShake, useAttenuation, attenuationCurve, flag, num, flag2, animationCurve, default(Vector2), default(Vector3), 1f, channelData, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x0003E688 File Offset: 0x0003C888
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_ScaleShake.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.TargetShaker == null)
			{
				MMScaleShakeEvent.Trigger(this.Duration, this.ShakeSpeed, this.ShakeRange, this.ShakeMainDirection, this.RandomizeDirection, this.ShakeAltDirection, this.RandomizeDirectionOnPlay, this.AddDirectionalNoise, this.DirectionalNoiseStrengthMin, this.DirectionalNoiseStrengthMax, this.RandomnessSeed, this.RandomizeSeedOnShake, this.UseAttenuation, this.AttenuationCurve, false, 0f, false, null, default(Vector2), default(Vector3), 1f, null, true, true, true, TimescaleModes.Scaled, false, true);
				return;
			}
			MMScaleShaker targetShaker = this.TargetShaker;
			if (targetShaker == null)
			{
				return;
			}
			float duration = this.Duration;
			float shakeSpeed = this.ShakeSpeed;
			float shakeRange = this.ShakeRange;
			Vector3 shakeMainDirection = this.ShakeMainDirection;
			bool randomizeDirection = this.RandomizeDirection;
			Vector3 shakeAltDirection = this.ShakeAltDirection;
			bool randomizeDirectionOnPlay = this.RandomizeDirectionOnPlay;
			bool addDirectionalNoise = this.AddDirectionalNoise;
			Vector3 directionalNoiseStrengthMin = this.DirectionalNoiseStrengthMin;
			Vector3 directionalNoiseStrengthMax = this.DirectionalNoiseStrengthMax;
			Vector3 randomnessSeed = this.RandomnessSeed;
			bool randomizeSeedOnShake = this.RandomizeSeedOnShake;
			bool useAttenuation = this.UseAttenuation;
			AnimationCurve attenuationCurve = this.AttenuationCurve;
			bool flag = false;
			float num = 0f;
			bool flag2 = false;
			AnimationCurve animationCurve = null;
			MMChannelData channelData = this.TargetShaker.ChannelData;
			targetShaker.OnMMScaleShakeEvent(duration, shakeSpeed, shakeRange, shakeMainDirection, randomizeDirection, shakeAltDirection, randomizeDirectionOnPlay, addDirectionalNoise, directionalNoiseStrengthMin, directionalNoiseStrengthMax, randomnessSeed, randomizeSeedOnShake, useAttenuation, attenuationCurve, flag, num, flag2, animationCurve, default(Vector2), default(Vector3), 1f, channelData, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x04001032 RID: 4146
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04001033 RID: 4147
		[MMFInspectorGroup("Optional Target", true, 33, false, false)]
		[Tooltip("a specific (and optional) shaker to target, regardless of its channel")]
		public MMScaleShaker TargetShaker;

		// Token: 0x04001034 RID: 4148
		[MMFInspectorGroup("Scale Shake", true, 28, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 0.5f;

		// Token: 0x04001035 RID: 4149
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04001036 RID: 4150
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04001037 RID: 4151
		[MMFInspectorGroup("Shake Settings", true, 42, false, false)]
		[Tooltip("the speed at which the transform should shake")]
		public float ShakeSpeed = 20f;

		// Token: 0x04001038 RID: 4152
		[Tooltip("the maximum distance from its initial scale the transform will move to during the shake")]
		public float ShakeRange = 0.5f;

		// Token: 0x04001039 RID: 4153
		[MMFInspectorGroup("Direction", true, 43, false, false)]
		[Tooltip("the direction along which to shake the transform's scale")]
		public Vector3 ShakeMainDirection = Vector3.up;

		// Token: 0x0400103A RID: 4154
		[Tooltip("if this is true, instead of using ShakeMainDirection as the direction of the shake, a random vector3 will be generated, randomized between ShakeMainDirection and ShakeAltDirection")]
		public bool RandomizeDirection;

		// Token: 0x0400103B RID: 4155
		[Tooltip("when in RandomizeDirection mode, a vector against which to randomize the main direction")]
		[MMFCondition("RandomizeDirection", true)]
		public Vector3 ShakeAltDirection = Vector3.up;

		// Token: 0x0400103C RID: 4156
		[Tooltip("if this is true, a new direction will be randomized every time a shake happens")]
		public bool RandomizeDirectionOnPlay;

		// Token: 0x0400103D RID: 4157
		[MMFInspectorGroup("Directional Noise", true, 47, false, false)]
		[Tooltip("whether or not to add noise to the main direction")]
		public bool AddDirectionalNoise = true;

		// Token: 0x0400103E RID: 4158
		[Tooltip("when adding directional noise, noise strength will be randomized between this value and DirectionalNoiseStrengthMax")]
		[MMFCondition("AddDirectionalNoise", true)]
		public Vector3 DirectionalNoiseStrengthMin = new Vector3(0.25f, 0.25f, 0.25f);

		// Token: 0x0400103F RID: 4159
		[Tooltip("when adding directional noise, noise strength will be randomized between this value and DirectionalNoiseStrengthMin")]
		[MMFCondition("AddDirectionalNoise", true)]
		public Vector3 DirectionalNoiseStrengthMax = new Vector3(0.25f, 0.25f, 0.25f);

		// Token: 0x04001040 RID: 4160
		[MMFInspectorGroup("Randomness", true, 44, false, false)]
		[Tooltip("a unique seed you can use to get different outcomes when shaking more than one transform at once")]
		public Vector3 RandomnessSeed;

		// Token: 0x04001041 RID: 4161
		[Tooltip("whether or not to generate a unique seed automatically on every shake")]
		public bool RandomizeSeedOnShake = true;

		// Token: 0x04001042 RID: 4162
		[MMFInspectorGroup("One Time", true, 45, false, false)]
		[Tooltip("whether or not to use attenuation, which will impact the amplitude of the shake, along the defined curve")]
		public bool UseAttenuation = true;

		// Token: 0x04001043 RID: 4163
		[Tooltip("the animation curve used to define attenuation, impacting the amplitude of the shake")]
		[MMFCondition("UseAttenuation", true)]
		public AnimationCurve AttenuationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});
	}
}
