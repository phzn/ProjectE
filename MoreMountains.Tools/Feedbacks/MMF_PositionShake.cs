using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001FE RID: 510
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Transform/Position Shake")]
	[FeedbackHelp("This feedback lets you emit a PositionShake event. This will be caught by MMPositionShakers (on the specified channel). Position shakers, as the name suggests, are used to shake the position of a transform, along a direction, with optional noise and other fine control options.")]
	public class MMF_PositionShake : MMF_Feedback
	{
		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000D85 RID: 3461 RVA: 0x0003B879 File Offset: 0x00039A79
		// (set) Token: 0x06000D86 RID: 3462 RVA: 0x0003B887 File Offset: 0x00039A87
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

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000D87 RID: 3463 RVA: 0x0003B890 File Offset: 0x00039A90
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000D88 RID: 3464 RVA: 0x0003B893 File Offset: 0x00039A93
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000D89 RID: 3465 RVA: 0x0003B896 File Offset: 0x00039A96
		public override bool HasRange
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000D8A RID: 3466 RVA: 0x0003B899 File Offset: 0x00039A99
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x0003B89C File Offset: 0x00039A9C
		protected override void AutomateTargetAcquisition()
		{
			this.TargetShaker = this.FindAutomatedTarget<MMPositionShaker>();
		}

		// Token: 0x06000D8C RID: 3468 RVA: 0x0003B8AC File Offset: 0x00039AAC
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_PositionShake.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			if (this.TargetShaker == null)
			{
				MMPositionShakeEvent.Trigger(this.Duration, this.ShakeSpeed, this.ShakeRange, this.ShakeMainDirection, this.RandomizeDirection, this.ShakeAltDirection, this.RandomizeDirectionOnPlay, this.RandomizeDirectionX, this.RandomizeDirectionY, this.RandomizeDirectionZ, this.AddDirectionalNoise, this.DirectionalNoiseStrengthMin, this.DirectionalNoiseStrengthMax, this.RandomnessSeed, this.RandomizeSeedOnShake, this.UseAttenuation, this.AttenuationCurve, this.UseRange, this.RangeDistance, this.UseRangeFalloff, this.RangeFalloff, this.RemapRangeFalloff, position, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
				return;
			}
			MMPositionShaker targetShaker = this.TargetShaker;
			if (targetShaker == null)
			{
				return;
			}
			targetShaker.OnMMPositionShakeEvent(this.Duration, this.ShakeSpeed, this.ShakeRange, this.ShakeMainDirection, this.RandomizeDirection, this.ShakeAltDirection, this.RandomizeDirectionOnPlay, this.RandomizeDirectionX, this.RandomizeDirectionY, this.RandomizeDirectionZ, this.AddDirectionalNoise, this.DirectionalNoiseStrengthMin, this.DirectionalNoiseStrengthMax, this.RandomnessSeed, this.RandomizeSeedOnShake, this.UseAttenuation, this.AttenuationCurve, this.UseRange, this.RangeDistance, this.UseRangeFalloff, this.RangeFalloff, this.RemapRangeFalloff, position, num, this.TargetShaker.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x0003BA4C File Offset: 0x00039C4C
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_PositionShake.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			if (this.TargetShaker == null)
			{
				MMPositionShakeEvent.Trigger(this.Duration, this.ShakeSpeed, this.ShakeRange, this.ShakeMainDirection, this.RandomizeDirection, this.ShakeAltDirection, this.RandomizeDirectionOnPlay, this.RandomizeDirectionX, this.RandomizeDirectionY, this.RandomizeDirectionZ, this.AddDirectionalNoise, this.DirectionalNoiseStrengthMin, this.DirectionalNoiseStrengthMax, this.RandomnessSeed, this.RandomizeSeedOnShake, this.UseAttenuation, this.AttenuationCurve, false, 0f, false, null, default(Vector2), default(Vector3), 1f, null, true, true, true, TimescaleModes.Scaled, true, false);
				return;
			}
			MMPositionShaker targetShaker = this.TargetShaker;
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
			bool randomizeDirectionX = this.RandomizeDirectionX;
			bool randomizeDirectionY = this.RandomizeDirectionY;
			bool randomizeDirectionZ = this.RandomizeDirectionZ;
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
			targetShaker.OnMMPositionShakeEvent(duration, shakeSpeed, shakeRange, shakeMainDirection, randomizeDirection, shakeAltDirection, randomizeDirectionOnPlay, randomizeDirectionX, randomizeDirectionY, randomizeDirectionZ, addDirectionalNoise, directionalNoiseStrengthMin, directionalNoiseStrengthMax, randomnessSeed, randomizeSeedOnShake, useAttenuation, attenuationCurve, flag, num, flag2, animationCurve, default(Vector2), default(Vector3), 1f, channelData, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x0003BBBC File Offset: 0x00039DBC
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_PositionShake.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.TargetShaker == null)
			{
				MMPositionShakeEvent.Trigger(this.Duration, this.ShakeSpeed, this.ShakeRange, this.ShakeMainDirection, this.RandomizeDirection, this.ShakeAltDirection, this.RandomizeDirectionOnPlay, this.RandomizeDirectionX, this.RandomizeDirectionY, this.RandomizeDirectionZ, this.AddDirectionalNoise, this.DirectionalNoiseStrengthMin, this.DirectionalNoiseStrengthMax, this.RandomnessSeed, this.RandomizeSeedOnShake, this.UseAttenuation, this.AttenuationCurve, false, 0f, false, null, default(Vector2), default(Vector3), 1f, null, true, true, true, TimescaleModes.Scaled, false, true);
				return;
			}
			MMPositionShaker targetShaker = this.TargetShaker;
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
			bool randomizeDirectionX = this.RandomizeDirectionX;
			bool randomizeDirectionY = this.RandomizeDirectionY;
			bool randomizeDirectionZ = this.RandomizeDirectionZ;
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
			targetShaker.OnMMPositionShakeEvent(duration, shakeSpeed, shakeRange, shakeMainDirection, randomizeDirection, shakeAltDirection, randomizeDirectionOnPlay, randomizeDirectionX, randomizeDirectionY, randomizeDirectionZ, addDirectionalNoise, directionalNoiseStrengthMin, directionalNoiseStrengthMax, randomnessSeed, randomizeSeedOnShake, useAttenuation, attenuationCurve, flag, num, flag2, animationCurve, default(Vector2), default(Vector3), 1f, channelData, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x04000F83 RID: 3971
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000F84 RID: 3972
		[MMFInspectorGroup("Optional Target", true, 33, false, false)]
		[Tooltip("a specific (and optional) shaker to target, regardless of its channel")]
		public MMPositionShaker TargetShaker;

		// Token: 0x04000F85 RID: 3973
		[MMFInspectorGroup("Position Shake", true, 28, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 0.5f;

		// Token: 0x04000F86 RID: 3974
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04000F87 RID: 3975
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04000F88 RID: 3976
		[MMFInspectorGroup("Shake Settings", true, 42, false, false)]
		[Tooltip("the speed at which the transform should shake")]
		public float ShakeSpeed = 20f;

		// Token: 0x04000F89 RID: 3977
		[Tooltip("the maximum distance from its initial position the transform will move to during the shake")]
		public float ShakeRange = 0.5f;

		// Token: 0x04000F8A RID: 3978
		[MMFInspectorGroup("Direction", true, 43, false, false)]
		[Tooltip("the direction along which to shake the transform's position")]
		public Vector3 ShakeMainDirection = Vector3.up;

		// Token: 0x04000F8B RID: 3979
		[Tooltip("if this is true, instead of using ShakeMainDirection as the direction of the shake, a random vector3 will be generated, randomized between ShakeMainDirection and ShakeAltDirection")]
		public bool RandomizeDirection;

		// Token: 0x04000F8C RID: 3980
		[Tooltip("when in RandomizeDirection mode, a vector against which to randomize the main direction")]
		[MMFCondition("RandomizeDirection", true)]
		public Vector3 ShakeAltDirection = Vector3.up;

		// Token: 0x04000F8D RID: 3981
		[Tooltip("if this is true, a new direction will be randomized every time a shake happens")]
		public bool RandomizeDirectionOnPlay;

		// Token: 0x04000F8E RID: 3982
		[Tooltip("whether or not to randomize the x value of the main direction")]
		public bool RandomizeDirectionX = true;

		// Token: 0x04000F8F RID: 3983
		[Tooltip("whether or not to randomize the y value of the main direction")]
		public bool RandomizeDirectionY = true;

		// Token: 0x04000F90 RID: 3984
		[Tooltip("whether or not to randomize the z value of the main direction")]
		public bool RandomizeDirectionZ = true;

		// Token: 0x04000F91 RID: 3985
		[MMFInspectorGroup("Directional Noise", true, 47, false, false)]
		[Tooltip("whether or not to add noise to the main direction")]
		public bool AddDirectionalNoise = true;

		// Token: 0x04000F92 RID: 3986
		[Tooltip("when adding directional noise, noise strength will be randomized between this value and DirectionalNoiseStrengthMax")]
		[MMFCondition("AddDirectionalNoise", true)]
		public Vector3 DirectionalNoiseStrengthMin = new Vector3(0.25f, 0.25f, 0.25f);

		// Token: 0x04000F93 RID: 3987
		[Tooltip("when adding directional noise, noise strength will be randomized between this value and DirectionalNoiseStrengthMin")]
		[MMFCondition("AddDirectionalNoise", true)]
		public Vector3 DirectionalNoiseStrengthMax = new Vector3(0.25f, 0.25f, 0.25f);

		// Token: 0x04000F94 RID: 3988
		[MMFInspectorGroup("Randomness", true, 44, false, false)]
		[Tooltip("a unique seed you can use to get different outcomes when shaking more than one transform at once")]
		public Vector3 RandomnessSeed;

		// Token: 0x04000F95 RID: 3989
		[Tooltip("whether or not to generate a unique seed automatically on every shake")]
		public bool RandomizeSeedOnShake = true;

		// Token: 0x04000F96 RID: 3990
		[MMFInspectorGroup("One Time", true, 45, false, false)]
		[Tooltip("whether or not to use attenuation, which will impact the amplitude of the shake, along the defined curve")]
		public bool UseAttenuation = true;

		// Token: 0x04000F97 RID: 3991
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
