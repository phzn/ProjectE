using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000250 RID: 592
	public class MMRotationShaker : MMShaker
	{
		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06001055 RID: 4181 RVA: 0x00048CAA File Offset: 0x00046EAA
		public virtual float Randomness
		{
			get
			{
				return this.RandomnessSeed.x + this.RandomnessSeed.y + this.RandomnessSeed.z;
			}
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x00048CD0 File Offset: 0x00046ED0
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
			this.GrabLocalRotation();
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x00048D20 File Offset: 0x00046F20
		public virtual void GrabLocalRotation()
		{
			MMRotationShaker.Modes mode = this.Mode;
			if (mode == MMRotationShaker.Modes.Transform)
			{
				this._initialRotation = this.TargetTransform.localRotation.eulerAngles;
				return;
			}
			if (mode != MMRotationShaker.Modes.RectTransform)
			{
				return;
			}
			this._initialRotation = this.TargetRectTransform.localRotation.eulerAngles;
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x00048D6F File Offset: 0x00046F6F
		protected virtual void Reset()
		{
			this.ShakeDuration = 0.5f;
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x00048D7C File Offset: 0x00046F7C
		protected override void ShakeStarts()
		{
			this.GrabLocalRotation();
			if (this.RandomizeSeedOnShake)
			{
				this.RandomnessSeed = Random.insideUnitSphere;
			}
			if (this.RandomizeDirectionOnPlay)
			{
				this.ShakeMainDirection = Random.insideUnitSphere;
				this.ShakeAltDirection = Random.insideUnitSphere;
			}
			this._randomizedDirection = (this.RandomizeDirection ? MMMaths.RandomVector3(this.ShakeMainDirection, this.ShakeAltDirection) : this.ShakeMainDirection);
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x00048DE8 File Offset: 0x00046FE8
		protected override void Shake()
		{
			this._oscillation = Mathf.Sin(this.ShakeSpeed * (this.Randomness + this._journey));
			float num = MMFeedbacksHelpers.Remap(this._journey, 0f, this.ShakeDuration, 0f, 1f);
			this._attenuation = this.ComputeAttenuation(num);
			this._workDirection = this.ShakeMainDirection + this.ComputeNoise(this._journey);
			this._workDirection.Normalize();
			this._newRotation = this.ComputeNewRotation();
			this.ApplyNewRotation(this._newRotation);
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x00048E82 File Offset: 0x00047082
		protected override void ShakeComplete()
		{
			base.ShakeComplete();
			this._attenuation = 0f;
			this._newRotation = this.ComputeNewRotation();
			if (this.TargetTransform != null)
			{
				this.ApplyNewRotation(this._newRotation);
			}
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x00048EBC File Offset: 0x000470BC
		protected virtual void ApplyNewRotation(Vector3 newRotation)
		{
			MMRotationShaker.Modes mode = this.Mode;
			if (mode == MMRotationShaker.Modes.Transform)
			{
				this.TargetTransform.localRotation = Quaternion.Euler(newRotation);
				return;
			}
			if (mode != MMRotationShaker.Modes.RectTransform)
			{
				return;
			}
			this.TargetRectTransform.localRotation = Quaternion.Euler(newRotation);
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x00048EFB File Offset: 0x000470FB
		protected virtual Vector3 ComputeNewRotation()
		{
			return this._initialRotation + this._workDirection * this._oscillation * this.ShakeRange * this._attenuation;
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x00048F2F File Offset: 0x0004712F
		protected virtual float ComputeAttenuation(float remappedTime)
		{
			if (!this.UseAttenuation || this.PermanentShake)
			{
				return 1f;
			}
			return this.AttenuationCurve.Evaluate(remappedTime);
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x00048F54 File Offset: 0x00047154
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

		// Token: 0x06001060 RID: 4192 RVA: 0x00049018 File Offset: 0x00047218
		public virtual void OnMMRotationShakeEvent(float duration, float shakeSpeed, float shakeRange, Vector3 shakeMainDirection, bool randomizeDirection, Vector3 shakeAltDirection, bool randomizeDirectionOnPlay, bool addDirectionalNoise, Vector3 directionalNoiseStrengthMin, Vector3 directionalNoiseStrengthMax, Vector3 randomnessSeed, bool randomizeSeedOnShake, bool useAttenuation, AnimationCurve attenuationCurve, bool useRange = false, float rangeDistance = 0f, bool useRangeFalloff = false, AnimationCurve rangeFalloff = null, Vector2 remapRangeFalloff = default(Vector2), Vector3 rangePosition = default(Vector3), float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			if (!this.CheckEventAllowed(channelData, false, 0f, default(Vector3)) || (!this.Interruptible && this.Shaking))
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

		// Token: 0x06001061 RID: 4193 RVA: 0x000491C8 File Offset: 0x000473C8
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			MMRotationShaker.Modes mode = this.Mode;
			if (mode == MMRotationShaker.Modes.Transform)
			{
				this.TargetTransform.localRotation = Quaternion.Euler(this._initialRotation);
				return;
			}
			if (mode != MMRotationShaker.Modes.RectTransform)
			{
				return;
			}
			this.TargetRectTransform.localRotation = Quaternion.Euler(this._initialRotation);
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x00049218 File Offset: 0x00047418
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
			this.AddDirectionalNoise = this._originalAddDirectionalNoise;
			this.DirectionalNoiseStrengthMin = this._originalDirectionalNoiseStrengthMin;
			this.DirectionalNoiseStrengthMax = this._originalDirectionalNoiseStrengthMax;
			this.RandomnessSeed = this._originalRandomnessSeed;
			this.RandomizeSeedOnShake = this._originalRandomizeSeedOnShake;
			this.UseAttenuation = this._originalUseAttenuation;
			this.AttenuationCurve = this._originalAttenuationCurve;
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x000492D3 File Offset: 0x000474D3
		public override void StartListening()
		{
			base.StartListening();
			MMRotationShakeEvent.Register(new MMRotationShakeEvent.Delegate(this.OnMMRotationShakeEvent));
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x000492ED File Offset: 0x000474ED
		public override void StopListening()
		{
			base.StopListening();
			MMRotationShakeEvent.Unregister(new MMRotationShakeEvent.Delegate(this.OnMMRotationShakeEvent));
		}

		// Token: 0x04001307 RID: 4871
		[MMInspectorGroup("Target", true, 41, false)]
		[Tooltip("whether this shaker should target Transforms or RectTransforms")]
		public MMRotationShaker.Modes Mode;

		// Token: 0x04001308 RID: 4872
		[Tooltip("the transform to shake the rotation of. If left blank, this component will target the transform it's put on.")]
		[MMEnumCondition("Mode", new int[] { 0 })]
		public Transform TargetTransform;

		// Token: 0x04001309 RID: 4873
		[Tooltip("the rect transform to shake the rotation of. If left blank, this component will target the transform it's put on.")]
		[MMEnumCondition("Mode", new int[] { 1 })]
		public RectTransform TargetRectTransform;

		// Token: 0x0400130A RID: 4874
		[MMInspectorGroup("Shake Settings", true, 42, false)]
		[Tooltip("the speed at which the transform should shake")]
		public float ShakeSpeed = 20f;

		// Token: 0x0400130B RID: 4875
		[Tooltip("the maximum distance from its initial rotation the transform will move to during the shake")]
		public float ShakeRange = 50f;

		// Token: 0x0400130C RID: 4876
		[MMInspectorGroup("Direction", true, 43, false)]
		[Tooltip("the direction along which to shake the transform's rotation")]
		public Vector3 ShakeMainDirection = Vector3.up;

		// Token: 0x0400130D RID: 4877
		[Tooltip("if this is true, instead of using ShakeMainDirection as the direction of the shake, a random vector3 will be generated, randomized between ShakeMainDirection and ShakeAltDirection")]
		public bool RandomizeDirection;

		// Token: 0x0400130E RID: 4878
		[Tooltip("when in RandomizeDirection mode, a vector against which to randomize the main direction")]
		[MMCondition("RandomizeDirection", true)]
		public Vector3 ShakeAltDirection = Vector3.up;

		// Token: 0x0400130F RID: 4879
		[Tooltip("if this is true, a new direction will be randomized every time a shake happens")]
		public bool RandomizeDirectionOnPlay;

		// Token: 0x04001310 RID: 4880
		[MMInspectorGroup("Directional Noise", true, 47, false)]
		[Tooltip("whether or not to add noise to the main direction")]
		public bool AddDirectionalNoise = true;

		// Token: 0x04001311 RID: 4881
		[Tooltip("when adding directional noise, noise strength will be randomized between this value and DirectionalNoiseStrengthMax")]
		[MMCondition("AddDirectionalNoise", true)]
		public Vector3 DirectionalNoiseStrengthMin = new Vector3(0.25f, 0.25f, 0.25f);

		// Token: 0x04001312 RID: 4882
		[Tooltip("when adding directional noise, noise strength will be randomized between this value and DirectionalNoiseStrengthMin")]
		[MMCondition("AddDirectionalNoise", true)]
		public Vector3 DirectionalNoiseStrengthMax = new Vector3(0.25f, 0.25f, 0.25f);

		// Token: 0x04001313 RID: 4883
		[MMInspectorGroup("Randomness", true, 44, false)]
		[Tooltip("a unique seed you can use to get different outcomes when shaking more than one transform at once")]
		public Vector3 RandomnessSeed;

		// Token: 0x04001314 RID: 4884
		[Tooltip("whether or not to generate a unique seed automatically on every shake")]
		public bool RandomizeSeedOnShake = true;

		// Token: 0x04001315 RID: 4885
		[MMInspectorGroup("One Time", true, 45, false)]
		[Tooltip("whether or not to use attenuation, which will impact the amplitude of the shake, along the defined curve")]
		public bool UseAttenuation = true;

		// Token: 0x04001316 RID: 4886
		[Tooltip("the animation curve used to define attenuation, impacting the amplitude of the shake")]
		[MMCondition("UseAttenuation", true)]
		public AnimationCurve AttenuationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001317 RID: 4887
		[MMInspectorGroup("Test", true, 46, false)]
		[MMInspectorButton("StartShaking")]
		public bool StartShakingButton;

		// Token: 0x04001318 RID: 4888
		protected float _attenuation = 1f;

		// Token: 0x04001319 RID: 4889
		protected float _oscillation;

		// Token: 0x0400131A RID: 4890
		protected Vector3 _initialRotation;

		// Token: 0x0400131B RID: 4891
		protected Vector3 _workDirection;

		// Token: 0x0400131C RID: 4892
		protected Vector3 _noiseVector;

		// Token: 0x0400131D RID: 4893
		protected Vector3 _newRotation;

		// Token: 0x0400131E RID: 4894
		protected Vector3 _randomNoiseStrength;

		// Token: 0x0400131F RID: 4895
		protected Vector3 _noNoise = Vector3.zero;

		// Token: 0x04001320 RID: 4896
		protected Vector3 _randomizedDirection;

		// Token: 0x04001321 RID: 4897
		protected float _originalDuration;

		// Token: 0x04001322 RID: 4898
		protected float _originalShakeSpeed;

		// Token: 0x04001323 RID: 4899
		protected float _originalShakeRange;

		// Token: 0x04001324 RID: 4900
		protected Vector3 _originalShakeMainDirection;

		// Token: 0x04001325 RID: 4901
		protected bool _originalRandomizeDirection;

		// Token: 0x04001326 RID: 4902
		protected Vector3 _originalShakeAltDirection;

		// Token: 0x04001327 RID: 4903
		protected bool _originalRandomizeDirectionOnPlay;

		// Token: 0x04001328 RID: 4904
		protected bool _originalAddDirectionalNoise;

		// Token: 0x04001329 RID: 4905
		protected Vector3 _originalDirectionalNoiseStrengthMin;

		// Token: 0x0400132A RID: 4906
		protected Vector3 _originalDirectionalNoiseStrengthMax;

		// Token: 0x0400132B RID: 4907
		protected Vector3 _originalRandomnessSeed;

		// Token: 0x0400132C RID: 4908
		protected bool _originalRandomizeSeedOnShake;

		// Token: 0x0400132D RID: 4909
		protected bool _originalUseAttenuation;

		// Token: 0x0400132E RID: 4910
		protected AnimationCurve _originalAttenuationCurve;

		// Token: 0x02000521 RID: 1313
		public enum Modes
		{
			// Token: 0x0400239F RID: 9119
			Transform,
			// Token: 0x040023A0 RID: 9120
			RectTransform
		}
	}
}
