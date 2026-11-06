using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200024E RID: 590
	public class MMPositionShaker : MMShaker
	{
		// Token: 0x17000199 RID: 409
		// (get) Token: 0x0600103E RID: 4158 RVA: 0x000482D8 File Offset: 0x000464D8
		public virtual float Randomness
		{
			get
			{
				return this.RandomnessSeed.x + this.RandomnessSeed.y + this.RandomnessSeed.z;
			}
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x00048300 File Offset: 0x00046500
		protected override void Initialization()
		{
			base.Initialization();
			if (this.TargetTransform == null)
			{
				this.TargetTransform = base.transform;
			}
			if (this.TargetRectTransform == null)
			{
				this.TargetRectTransform = base.GetComponent<RectTransform>();
			}
			this.GrabInitialPosition();
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x00048350 File Offset: 0x00046550
		public virtual void GrabInitialPosition()
		{
			MMPositionShaker.Modes mode = this.Mode;
			if (mode == MMPositionShaker.Modes.Transform)
			{
				this._initialPosition = this.TargetTransform.localPosition;
				return;
			}
			if (mode != MMPositionShaker.Modes.RectTransform)
			{
				return;
			}
			this._initialPosition = this.TargetRectTransform.anchoredPosition;
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x00048394 File Offset: 0x00046594
		protected virtual void Reset()
		{
			this.ShakeDuration = 0.5f;
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x000483A4 File Offset: 0x000465A4
		protected override void ShakeStarts()
		{
			this.GrabInitialPosition();
			if (this.RandomizeSeedOnShake)
			{
				this.RandomnessSeed = Random.insideUnitSphere;
			}
			if (this.RandomizeDirectionOnPlay)
			{
				this.ShakeMainDirection.x = (this.RandomizeDirectionX ? Random.insideUnitSphere.x : this.ShakeMainDirection.x);
				this.ShakeMainDirection.y = (this.RandomizeDirectionY ? Random.insideUnitSphere.y : this.ShakeMainDirection.y);
				this.ShakeMainDirection.z = (this.RandomizeDirectionZ ? Random.insideUnitSphere.z : this.ShakeMainDirection.z);
				this.ShakeAltDirection.x = (this.RandomizeDirectionX ? Random.insideUnitSphere.x : this.ShakeAltDirection.x);
				this.ShakeAltDirection.y = (this.RandomizeDirectionY ? Random.insideUnitSphere.y : this.ShakeAltDirection.y);
				this.ShakeAltDirection.z = (this.RandomizeDirectionZ ? Random.insideUnitSphere.z : this.ShakeAltDirection.z);
			}
			this._randomizedDirection = (this.RandomizeDirection ? MMMaths.RandomVector3(this.ShakeMainDirection, this.ShakeAltDirection) : this.ShakeMainDirection);
			if (!this.RandomizeDirectionX)
			{
				this._randomizedDirection.x = this.ShakeMainDirection.x;
			}
			if (!this.RandomizeDirectionY)
			{
				this._randomizedDirection.y = this.ShakeMainDirection.y;
			}
			if (!this.RandomizeDirectionZ)
			{
				this._randomizedDirection.z = this.ShakeMainDirection.z;
			}
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x00048554 File Offset: 0x00046754
		protected override void Shake()
		{
			this._oscillation = this.OscillationOffset + Mathf.Sin(this.ShakeSpeed * (this.Randomness + this._journey));
			float num = MMFeedbacksHelpers.Remap(this._journey, 0f, this.ShakeDuration, 0f, 1f);
			this._attenuation = this.ComputeAttenuation(num);
			this._workDirection = this.ShakeMainDirection + this.ComputeNoise(this._journey);
			this._workDirection.Normalize();
			this._newPosition = this.ComputeNewPosition();
			this.ApplyNewPosition(this._newPosition);
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x000485F5 File Offset: 0x000467F5
		protected override void ShakeComplete()
		{
			base.ShakeComplete();
			this._attenuation = 0f;
			this._newPosition = this.ComputeNewPosition();
			if (this.TargetTransform != null)
			{
				this.ApplyNewPosition(this._newPosition);
			}
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x00048630 File Offset: 0x00046830
		protected virtual void ApplyNewPosition(Vector3 newPosition)
		{
			MMPositionShaker.Modes mode = this.Mode;
			if (mode == MMPositionShaker.Modes.Transform)
			{
				this.TargetTransform.localPosition = newPosition;
				return;
			}
			if (mode != MMPositionShaker.Modes.RectTransform)
			{
				return;
			}
			this.TargetRectTransform.anchoredPosition = newPosition;
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x0004866A File Offset: 0x0004686A
		protected virtual Vector3 ComputeNewPosition()
		{
			return this._initialPosition + this._workDirection * this._oscillation * this.ShakeRange * this._attenuation;
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x0004869E File Offset: 0x0004689E
		protected virtual float ComputeAttenuation(float remappedTime)
		{
			if (!this.UseAttenuation || this.PermanentShake)
			{
				return 1f;
			}
			return this.AttenuationCurve.Evaluate(remappedTime);
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x000486C4 File Offset: 0x000468C4
		protected virtual Vector3 ComputeNoise(float time)
		{
			if (!this.AddDirectionalNoise)
			{
				return this._noNoise;
			}
			this._randomNoiseStrength = MMMaths.RandomVector3(this.DirectionalNoiseStrengthMin, this.DirectionalNoiseStrengthMax);
			this._noiseVector.x = this._randomNoiseStrength.x * (Mathf.PerlinNoise(this.RandomnessSeed.x, time) - 0.5f);
			this._noiseVector.y = this._randomNoiseStrength.y * (Mathf.PerlinNoise(this.RandomnessSeed.y, time) - 0.5f);
			this._noiseVector.z = this._randomNoiseStrength.z * (Mathf.PerlinNoise(this.RandomnessSeed.z, time) - 0.5f);
			return this._noiseVector;
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x00048788 File Offset: 0x00046988
		public virtual void OnMMPositionShakeEvent(float duration, float shakeSpeed, float shakeRange, Vector3 shakeMainDirection, bool randomizeDirection, Vector3 shakeAltDirection, bool randomizeDirectionOnPlay, bool randomizeDirectionX, bool randomizeDirectionY, bool randomizeDirectionZ, bool addDirectionalNoise, Vector3 directionalNoiseStrengthMin, Vector3 directionalNoiseStrengthMax, Vector3 randomnessSeed, bool randomizeSeedOnShake, bool useAttenuation, AnimationCurve attenuationCurve, bool useRange = false, float rangeDistance = 0f, bool useRangeFalloff = false, AnimationCurve rangeFalloff = null, Vector2 remapRangeFalloff = default(Vector2), Vector3 rangePosition = default(Vector3), float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			if (!this.CheckEventAllowed(channelData, useRange, rangeDistance, rangePosition) || (!this.Interruptible && this.Shaking))
			{
				return;
			}
			if (stop)
			{
				this.Stop();
				return;
			}
			if (restore)
			{
				this.ResetTargetValues();
				return;
			}
			this._resetShakerValuesAfterShake = resetShakerValuesAfterShake;
			this._resetTargetValuesAfterShake = resetTargetValuesAfterShake;
			if (resetShakerValuesAfterShake)
			{
				this._originalDuration = this.ShakeDuration;
				this._originalShakeSpeed = this.ShakeSpeed;
				this._originalShakeRange = this.ShakeRange;
				this._originalShakeMainDirection = this.ShakeMainDirection;
				this._originalRandomizeDirection = this.RandomizeDirection;
				this._originalShakeAltDirection = this.ShakeAltDirection;
				this._originalRandomizeDirectionOnPlay = this.RandomizeDirectionOnPlay;
				this._originalRandomizeDirectionX = this.RandomizeDirectionX;
				this._originalRandomizeDirectionY = this.RandomizeDirectionY;
				this._originalRandomizeDirectionZ = this.RandomizeDirectionZ;
				this._originalAddDirectionalNoise = this.AddDirectionalNoise;
				this._originalDirectionalNoiseStrengthMin = this.DirectionalNoiseStrengthMin;
				this._originalDirectionalNoiseStrengthMax = this.DirectionalNoiseStrengthMax;
				this._originalRandomnessSeed = this.RandomnessSeed;
				this._originalRandomizeSeedOnShake = this.RandomizeSeedOnShake;
				this._originalUseAttenuation = this.UseAttenuation;
				this._originalAttenuationCurve = this.AttenuationCurve;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.TimescaleMode = timescaleMode;
				this.ShakeDuration = duration;
				this.ShakeSpeed = shakeSpeed;
				this.ShakeRange = shakeRange * feedbacksIntensity * this.ComputeRangeIntensity(useRange, rangeDistance, useRangeFalloff, rangeFalloff, remapRangeFalloff, rangePosition);
				this.ShakeMainDirection = shakeMainDirection;
				this.RandomizeDirection = randomizeDirection;
				this.ShakeAltDirection = shakeAltDirection;
				this.RandomizeDirectionOnPlay = randomizeDirectionOnPlay;
				this.RandomizeDirectionX = randomizeDirectionX;
				this.RandomizeDirectionY = randomizeDirectionY;
				this.RandomizeDirectionZ = randomizeDirectionZ;
				this.AddDirectionalNoise = addDirectionalNoise;
				this.DirectionalNoiseStrengthMin = directionalNoiseStrengthMin;
				this.DirectionalNoiseStrengthMax = directionalNoiseStrengthMax;
				this.RandomnessSeed = randomnessSeed;
				this.RandomizeSeedOnShake = randomizeSeedOnShake;
				this.UseAttenuation = useAttenuation;
				this.AttenuationCurve = attenuationCurve;
				this.ForwardDirection = forwardDirection;
			}
			this.Play();
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x0004896C File Offset: 0x00046B6C
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			MMPositionShaker.Modes mode = this.Mode;
			if (mode == MMPositionShaker.Modes.Transform)
			{
				this.TargetTransform.localPosition = this._initialPosition;
				return;
			}
			if (mode != MMPositionShaker.Modes.RectTransform)
			{
				return;
			}
			this.TargetRectTransform.anchoredPosition = this._initialPosition;
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x000489B8 File Offset: 0x00046BB8
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalDuration;
			this.ShakeSpeed = this._originalShakeSpeed;
			this.ShakeRange = this._originalShakeRange;
			this.ShakeMainDirection = this._originalShakeMainDirection;
			this.RandomizeDirection = this._originalRandomizeDirection;
			this.ShakeAltDirection = this._originalShakeAltDirection;
			this.RandomizeDirectionOnPlay = this._originalRandomizeDirectionOnPlay;
			this.RandomizeDirectionX = this._originalRandomizeDirectionX;
			this.RandomizeDirectionY = this._originalRandomizeDirectionY;
			this.RandomizeDirectionZ = this._originalRandomizeDirectionZ;
			this.AddDirectionalNoise = this._originalAddDirectionalNoise;
			this.DirectionalNoiseStrengthMin = this._originalDirectionalNoiseStrengthMin;
			this.DirectionalNoiseStrengthMax = this._originalDirectionalNoiseStrengthMax;
			this.RandomnessSeed = this._originalRandomnessSeed;
			this.RandomizeSeedOnShake = this._originalRandomizeSeedOnShake;
			this.UseAttenuation = this._originalUseAttenuation;
			this.AttenuationCurve = this._originalAttenuationCurve;
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x00048A97 File Offset: 0x00046C97
		public override void StartListening()
		{
			base.StartListening();
			MMPositionShakeEvent.Register(new MMPositionShakeEvent.Delegate(this.OnMMPositionShakeEvent));
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x00048AB1 File Offset: 0x00046CB1
		public override void StopListening()
		{
			base.StopListening();
			MMPositionShakeEvent.Unregister(new MMPositionShakeEvent.Delegate(this.OnMMPositionShakeEvent));
		}

		// Token: 0x040012D7 RID: 4823
		[MMInspectorGroup("Target", true, 41, false)]
		[Tooltip("whether this shaker should target Transforms or RectTransforms")]
		public MMPositionShaker.Modes Mode;

		// Token: 0x040012D8 RID: 4824
		[Tooltip("the transform to shake the position of. If left blank, this component will target the transform it's put on.")]
		[MMEnumCondition("Mode", new int[] { 0 })]
		public Transform TargetTransform;

		// Token: 0x040012D9 RID: 4825
		[Tooltip("the rect transform to shake the position of. If left blank, this component will target the transform it's put on.")]
		[MMEnumCondition("Mode", new int[] { 1 })]
		public RectTransform TargetRectTransform;

		// Token: 0x040012DA RID: 4826
		[MMInspectorGroup("Shake Settings", true, 42, false)]
		[Tooltip("the speed at which the transform should shake")]
		public float ShakeSpeed = 20f;

		// Token: 0x040012DB RID: 4827
		[Tooltip("the maximum distance from its initial position the transform will move to during the shake")]
		public float ShakeRange = 0.5f;

		// Token: 0x040012DC RID: 4828
		[Tooltip("an offset to apply to the oscillation")]
		public float OscillationOffset;

		// Token: 0x040012DD RID: 4829
		[MMInspectorGroup("Direction", true, 43, false)]
		[Tooltip("the direction along which to shake the transform's position")]
		public Vector3 ShakeMainDirection = Vector3.up;

		// Token: 0x040012DE RID: 4830
		[Tooltip("if this is true, instead of using ShakeMainDirection as the direction of the shake, a random vector3 will be generated, randomized between ShakeMainDirection and ShakeAltDirection")]
		public bool RandomizeDirection;

		// Token: 0x040012DF RID: 4831
		[Tooltip("when in RandomizeDirection mode, a vector against which to randomize the main direction")]
		[MMCondition("RandomizeDirection", true)]
		public Vector3 ShakeAltDirection = Vector3.up;

		// Token: 0x040012E0 RID: 4832
		[Tooltip("if this is true, a new direction will be randomized every time a shake happens")]
		public bool RandomizeDirectionOnPlay;

		// Token: 0x040012E1 RID: 4833
		[Tooltip("whether or not to randomize the x value of the main direction")]
		public bool RandomizeDirectionX = true;

		// Token: 0x040012E2 RID: 4834
		[Tooltip("whether or not to randomize the y value of the main direction")]
		public bool RandomizeDirectionY = true;

		// Token: 0x040012E3 RID: 4835
		[Tooltip("whether or not to randomize the z value of the main direction")]
		public bool RandomizeDirectionZ = true;

		// Token: 0x040012E4 RID: 4836
		[MMInspectorGroup("Directional Noise", true, 47, false)]
		[Tooltip("whether or not to add noise to the main direction")]
		public bool AddDirectionalNoise = true;

		// Token: 0x040012E5 RID: 4837
		[Tooltip("when adding directional noise, noise strength will be randomized between this value and DirectionalNoiseStrengthMax")]
		[MMCondition("AddDirectionalNoise", true)]
		public Vector3 DirectionalNoiseStrengthMin = new Vector3(0.25f, 0.25f, 0.25f);

		// Token: 0x040012E6 RID: 4838
		[Tooltip("when adding directional noise, noise strength will be randomized between this value and DirectionalNoiseStrengthMin")]
		[MMCondition("AddDirectionalNoise", true)]
		public Vector3 DirectionalNoiseStrengthMax = new Vector3(0.25f, 0.25f, 0.25f);

		// Token: 0x040012E7 RID: 4839
		[MMInspectorGroup("Randomness", true, 44, false)]
		[Tooltip("a unique seed you can use to get different outcomes when shaking more than one transform at once")]
		public Vector3 RandomnessSeed;

		// Token: 0x040012E8 RID: 4840
		[Tooltip("whether or not to generate a unique seed automatically on every shake")]
		public bool RandomizeSeedOnShake = true;

		// Token: 0x040012E9 RID: 4841
		[MMInspectorGroup("One Time", true, 45, false)]
		[Tooltip("whether or not to use attenuation, which will impact the amplitude of the shake, along the defined curve")]
		public bool UseAttenuation = true;

		// Token: 0x040012EA RID: 4842
		[Tooltip("the animation curve used to define attenuation, impacting the amplitude of the shake")]
		[MMCondition("UseAttenuation", true)]
		public AnimationCurve AttenuationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040012EB RID: 4843
		[MMInspectorGroup("Test", true, 46, false)]
		[MMInspectorButton("StartShaking")]
		public bool StartShakingButton;

		// Token: 0x040012EC RID: 4844
		protected float _attenuation = 1f;

		// Token: 0x040012ED RID: 4845
		protected float _oscillation;

		// Token: 0x040012EE RID: 4846
		protected Vector3 _initialPosition;

		// Token: 0x040012EF RID: 4847
		protected Vector3 _workDirection;

		// Token: 0x040012F0 RID: 4848
		protected Vector3 _noiseVector;

		// Token: 0x040012F1 RID: 4849
		protected Vector3 _newPosition;

		// Token: 0x040012F2 RID: 4850
		protected Vector3 _randomNoiseStrength;

		// Token: 0x040012F3 RID: 4851
		protected Vector3 _noNoise = Vector3.zero;

		// Token: 0x040012F4 RID: 4852
		protected Vector3 _randomizedDirection;

		// Token: 0x040012F5 RID: 4853
		protected float _originalDuration;

		// Token: 0x040012F6 RID: 4854
		protected float _originalShakeSpeed;

		// Token: 0x040012F7 RID: 4855
		protected float _originalShakeRange;

		// Token: 0x040012F8 RID: 4856
		protected Vector3 _originalShakeMainDirection;

		// Token: 0x040012F9 RID: 4857
		protected bool _originalRandomizeDirection;

		// Token: 0x040012FA RID: 4858
		protected Vector3 _originalShakeAltDirection;

		// Token: 0x040012FB RID: 4859
		protected bool _originalRandomizeDirectionOnPlay;

		// Token: 0x040012FC RID: 4860
		protected bool _originalRandomizeDirectionX;

		// Token: 0x040012FD RID: 4861
		protected bool _originalRandomizeDirectionY;

		// Token: 0x040012FE RID: 4862
		protected bool _originalRandomizeDirectionZ;

		// Token: 0x040012FF RID: 4863
		protected bool _originalAddDirectionalNoise;

		// Token: 0x04001300 RID: 4864
		protected Vector3 _originalDirectionalNoiseStrengthMin;

		// Token: 0x04001301 RID: 4865
		protected Vector3 _originalDirectionalNoiseStrengthMax;

		// Token: 0x04001302 RID: 4866
		protected Vector3 _originalRandomnessSeed;

		// Token: 0x04001303 RID: 4867
		protected bool _originalRandomizeSeedOnShake;

		// Token: 0x04001304 RID: 4868
		protected bool _originalUseAttenuation;

		// Token: 0x04001305 RID: 4869
		protected AnimationCurve _originalAttenuationCurve;

		// Token: 0x0200051F RID: 1311
		public enum Modes
		{
			// Token: 0x0400239C RID: 9116
			Transform,
			// Token: 0x0400239D RID: 9117
			RectTransform
		}
	}
}
