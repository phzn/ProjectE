using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000205 RID: 517
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Transform/Rotation Shake")]
	[FeedbackHelp("This feedback lets you emit a RotationShake event. This will be caught by MMRotationShakers (on the specified channel). Rotation shakers, as the name suggests, are used to shake the rotation of a transform, along a direction, with optional noise and other fine control options.")]
	public class MMF_RotationShake : MMF_Feedback
	{
		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000DD8 RID: 3544 RVA: 0x0003D3F5 File Offset: 0x0003B5F5
		// (set) Token: 0x06000DD9 RID: 3545 RVA: 0x0003D403 File Offset: 0x0003B603
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

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000DDA RID: 3546 RVA: 0x0003D40C File Offset: 0x0003B60C
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000DDB RID: 3547 RVA: 0x0003D40F File Offset: 0x0003B60F
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000DDC RID: 3548 RVA: 0x0003D412 File Offset: 0x0003B612
		public override bool HasRange
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000DDD RID: 3549 RVA: 0x0003D415 File Offset: 0x0003B615
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0003D418 File Offset: 0x0003B618
		protected override void AutomateTargetAcquisition()
		{
			this.TargetShaker = this.FindAutomatedTarget<MMRotationShaker>();
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x0003D428 File Offset: 0x0003B628
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_RotationShake.FeedbackTypeAuthorized)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			if (this.TargetShaker == null)
			{
				MMRotationShakeEvent.Trigger(this.Duration, this.ShakeSpeed, this.ShakeRange, this.ShakeMainDirection, this.RandomizeDirection, this.ShakeAltDirection, this.RandomizeDirectionOnPlay, this.AddDirectionalNoise, this.DirectionalNoiseStrengthMin, this.DirectionalNoiseStrengthMax, this.RandomnessSeed, this.RandomizeSeedOnShake, this.UseAttenuation, this.AttenuationCurve, this.UseRange, this.RangeDistance, this.UseRangeFalloff, this.RangeFalloff, this.RemapRangeFalloff, position, num, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
				return;
			}
			MMRotationShaker targetShaker = this.TargetShaker;
			if (targetShaker == null)
			{
				return;
			}
			targetShaker.OnMMRotationShakeEvent(this.Duration, this.ShakeSpeed, this.ShakeRange, this.ShakeMainDirection, this.RandomizeDirection, this.ShakeAltDirection, this.RandomizeDirectionOnPlay, this.AddDirectionalNoise, this.DirectionalNoiseStrengthMin, this.DirectionalNoiseStrengthMax, this.RandomnessSeed, this.RandomizeSeedOnShake, this.UseAttenuation, this.AttenuationCurve, this.UseRange, this.RangeDistance, this.UseRangeFalloff, this.RangeFalloff, this.RemapRangeFalloff, position, num, this.TargetShaker.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false, false);
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x0003D5A4 File Offset: 0x0003B7A4
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_RotationShake.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			if (this.TargetShaker == null)
			{
				MMRotationShakeEvent.Trigger(this.Duration, this.ShakeSpeed, this.ShakeRange, this.ShakeMainDirection, this.RandomizeDirection, this.ShakeAltDirection, this.RandomizeDirectionOnPlay, this.AddDirectionalNoise, this.DirectionalNoiseStrengthMin, this.DirectionalNoiseStrengthMax, this.RandomnessSeed, this.RandomizeSeedOnShake, this.UseAttenuation, this.AttenuationCurve, false, 0f, false, null, default(Vector2), default(Vector3), 1f, null, true, true, true, TimescaleModes.Scaled, true, false);
				return;
			}
			MMRotationShaker targetShaker = this.TargetShaker;
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
			targetShaker.OnMMRotationShakeEvent(duration, shakeSpeed, shakeRange, shakeMainDirection, randomizeDirection, shakeAltDirection, randomizeDirectionOnPlay, addDirectionalNoise, directionalNoiseStrengthMin, directionalNoiseStrengthMax, randomnessSeed, randomizeSeedOnShake, useAttenuation, attenuationCurve, flag, num, flag2, animationCurve, default(Vector2), default(Vector3), 1f, channelData, true, true, true, TimescaleModes.Scaled, true, false);
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0003D6F0 File Offset: 0x0003B8F0
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_RotationShake.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.TargetShaker == null)
			{
				MMRotationShakeEvent.Trigger(this.Duration, this.ShakeSpeed, this.ShakeRange, this.ShakeMainDirection, this.RandomizeDirection, this.ShakeAltDirection, this.RandomizeDirectionOnPlay, this.AddDirectionalNoise, this.DirectionalNoiseStrengthMin, this.DirectionalNoiseStrengthMax, this.RandomnessSeed, this.RandomizeSeedOnShake, this.UseAttenuation, this.AttenuationCurve, false, 0f, false, null, default(Vector2), default(Vector3), 1f, null, true, true, true, TimescaleModes.Scaled, false, true);
				return;
			}
			MMRotationShaker targetShaker = this.TargetShaker;
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
			targetShaker.OnMMRotationShakeEvent(duration, shakeSpeed, shakeRange, shakeMainDirection, randomizeDirection, shakeAltDirection, randomizeDirectionOnPlay, addDirectionalNoise, directionalNoiseStrengthMin, directionalNoiseStrengthMax, randomnessSeed, randomizeSeedOnShake, useAttenuation, attenuationCurve, flag, num, flag2, animationCurve, default(Vector2), default(Vector3), 1f, channelData, true, true, true, TimescaleModes.Scaled, false, true);
		}

		// Token: 0x04000FF4 RID: 4084
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000FF5 RID: 4085
		[MMFInspectorGroup("Optional Target", true, 33, false, false)]
		[Tooltip("a specific (and optional) shaker to target, regardless of its channel")]
		public MMRotationShaker TargetShaker;

		// Token: 0x04000FF6 RID: 4086
		[MMFInspectorGroup("Rotation Shake", true, 28, false, false)]
		[Tooltip("the duration of the shake, in seconds")]
		public float Duration = 0.5f;

		// Token: 0x04000FF7 RID: 4087
		[Tooltip("whether or not to reset shaker values after shake")]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04000FF8 RID: 4088
		[Tooltip("whether or not to reset the target's values after shake")]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04000FF9 RID: 4089
		[MMFInspectorGroup("Shake Settings", true, 42, false, false)]
		[Tooltip("the speed at which the transform should shake")]
		public float ShakeSpeed = 20f;

		// Token: 0x04000FFA RID: 4090
		[Tooltip("the maximum distance from its initial rotation the transform will move to during the shake")]
		public float ShakeRange = 50f;

		// Token: 0x04000FFB RID: 4091
		[MMFInspectorGroup("Direction", true, 43, false, false)]
		[Tooltip("the direction along which to shake the transform's rotation")]
		public Vector3 ShakeMainDirection = Vector3.up;

		// Token: 0x04000FFC RID: 4092
		[Tooltip("if this is true, instead of using ShakeMainDirection as the direction of the shake, a random vector3 will be generated, randomized between ShakeMainDirection and ShakeAltDirection")]
		public bool RandomizeDirection;

		// Token: 0x04000FFD RID: 4093
		[Tooltip("when in RandomizeDirection mode, a vector against which to randomize the main direction")]
		[MMFCondition("RandomizeDirection", true)]
		public Vector3 ShakeAltDirection = Vector3.up;

		// Token: 0x04000FFE RID: 4094
		[Tooltip("if this is true, a new direction will be randomized every time a shake happens")]
		public bool RandomizeDirectionOnPlay;

		// Token: 0x04000FFF RID: 4095
		[MMFInspectorGroup("Directional Noise", true, 47, false, false)]
		[Tooltip("whether or not to add noise to the main direction")]
		public bool AddDirectionalNoise = true;

		// Token: 0x04001000 RID: 4096
		[Tooltip("when adding directional noise, noise strength will be randomized between this value and DirectionalNoiseStrengthMax")]
		[MMFCondition("AddDirectionalNoise", true)]
		public Vector3 DirectionalNoiseStrengthMin = new Vector3(0.25f, 0.25f, 0.25f);

		// Token: 0x04001001 RID: 4097
		[Tooltip("when adding directional noise, noise strength will be randomized between this value and DirectionalNoiseStrengthMin")]
		[MMFCondition("AddDirectionalNoise", true)]
		public Vector3 DirectionalNoiseStrengthMax = new Vector3(0.25f, 0.25f, 0.25f);

		// Token: 0x04001002 RID: 4098
		[MMFInspectorGroup("Randomness", true, 44, false, false)]
		[Tooltip("a unique seed you can use to get different outcomes when shaking more than one transform at once")]
		public Vector3 RandomnessSeed;

		// Token: 0x04001003 RID: 4099
		[Tooltip("whether or not to generate a unique seed automatically on every shake")]
		public bool RandomizeSeedOnShake = true;

		// Token: 0x04001004 RID: 4100
		[MMFInspectorGroup("One Time", true, 45, false, false)]
		[Tooltip("whether or not to use attenuation, which will impact the amplitude of the shake, along the defined curve")]
		public bool UseAttenuation = true;

		// Token: 0x04001005 RID: 4101
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
