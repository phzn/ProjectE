using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000252 RID: 594
	public class MMScaleShaker : MMShaker
	{
		// Token: 0x1700019B RID: 411
		// (get) Token: 0x0600106C RID: 4204 RVA: 0x000494CC File Offset: 0x000476CC
		public virtual float Randomness
		{
			get
			{
				return this.RandomnessSeed.x + this.RandomnessSeed.y + this.RandomnessSeed.z;
			}
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x000494F4 File Offset: 0x000476F4
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
			this.GrabInitialScale();
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x00049544 File Offset: 0x00047744
		public virtual void GrabInitialScale()
		{
			MMScaleShaker.Modes mode = this.Mode;
			if (mode == MMScaleShaker.Modes.Transform)
			{
				this._initialScale = this.TargetTransform.localScale;
				return;
			}
			if (mode != MMScaleShaker.Modes.RectTransform)
			{
				return;
			}
			this._initialScale = this.TargetRectTransform.localScale;
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x00049583 File Offset: 0x00047783
		protected virtual void Reset()
		{
			this.ShakeDuration = 0.5f;
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x00049590 File Offset: 0x00047790
		protected override void ShakeStarts()
		{
			this.GrabInitialScale();
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

		// Token: 0x06001071 RID: 4209 RVA: 0x000495FC File Offset: 0x000477FC
		protected override void Shake()
		{
			this._oscillation = Mathf.Sin(this.ShakeSpeed * (this.Randomness + this._journey));
			float num = MMFeedbacksHelpers.Remap(this._journey, 0f, this.ShakeDuration, 0f, 1f);
			this._attenuation = this.ComputeAttenuation(num);
			this._workDirection = this.ShakeMainDirection + this.ComputeNoise(this._journey);
			this._workDirection.Normalize();
			this._newScale = this.ComputeNewScale();
			this.ApplyNewScale(this._newScale);
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x00049696 File Offset: 0x00047896
		protected override void ShakeComplete()
		{
			base.ShakeComplete();
			this._attenuation = 0f;
			this._newScale = this.ComputeNewScale();
			if (this.TargetTransform != null)
			{
				this.ApplyNewScale(this._newScale);
			}
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x000496D0 File Offset: 0x000478D0
		protected virtual void ApplyNewScale(Vector3 newScale)
		{
			MMScaleShaker.Modes mode = this.Mode;
			if (mode == MMScaleShaker.Modes.Transform)
			{
				this.TargetTransform.localScale = newScale;
				return;
			}
			if (mode != MMScaleShaker.Modes.RectTransform)
			{
				return;
			}
			this.TargetRectTransform.localScale = newScale;
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x00049705 File Offset: 0x00047905
		protected virtual Vector3 ComputeNewScale()
		{
			return this._initialScale + this._workDirection * this._oscillation * this.ShakeRange * this._attenuation;
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x00049739 File Offset: 0x00047939
		protected virtual float ComputeAttenuation(float remappedTime)
		{
			if (!this.UseAttenuation || this.PermanentShake)
			{
				return 1f;
			}
			return this.AttenuationCurve.Evaluate(remappedTime);
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x00049760 File Offset: 0x00047960
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

		// Token: 0x06001077 RID: 4215 RVA: 0x00049824 File Offset: 0x00047A24
		public virtual void OnMMScaleShakeEvent(float duration, float shakeSpeed, float shakeRange, Vector3 shakeMainDirection, bool randomizeDirection, Vector3 shakeAltDirection, bool randomizeDirectionOnPlay, bool addDirectionalNoise, Vector3 directionalNoiseStrengthMin, Vector3 directionalNoiseStrengthMax, Vector3 randomnessSeed, bool randomizeSeedOnShake, bool useAttenuation, AnimationCurve attenuationCurve, bool useRange = false, float rangeDistance = 0f, bool useRangeFalloff = false, AnimationCurve rangeFalloff = null, Vector2 remapRangeFalloff = default(Vector2), Vector3 rangePosition = default(Vector3), float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
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

		// Token: 0x06001078 RID: 4216 RVA: 0x000499D4 File Offset: 0x00047BD4
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			MMScaleShaker.Modes mode = this.Mode;
			if (mode == MMScaleShaker.Modes.Transform)
			{
				this.TargetTransform.localScale = this._initialScale;
				return;
			}
			if (mode != MMScaleShaker.Modes.RectTransform)
			{
				return;
			}
			this.TargetRectTransform.localScale = this._initialScale;
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x00049A1C File Offset: 0x00047C1C
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

		// Token: 0x0600107A RID: 4218 RVA: 0x00049AD7 File Offset: 0x00047CD7
		public override void StartListening()
		{
			base.StartListening();
			MMScaleShakeEvent.Register(new MMScaleShakeEvent.Delegate(this.OnMMScaleShakeEvent));
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x00049AF1 File Offset: 0x00047CF1
		public override void StopListening()
		{
			base.StopListening();
			MMScaleShakeEvent.Unregister(new MMScaleShakeEvent.Delegate(this.OnMMScaleShakeEvent));
		}

		// Token: 0x04001330 RID: 4912
		[MMInspectorGroup("Target", true, 41, false)]
		[Tooltip("whether this shaker should target Transforms or RectTransforms")]
		public MMScaleShaker.Modes Mode;

		// Token: 0x04001331 RID: 4913
		[Tooltip("the transform to shake the scale of. If left blank, this component will target the transform it's put on.")]
		[MMEnumCondition("Mode", new int[] { 0 })]
		public Transform TargetTransform;

		// Token: 0x04001332 RID: 4914
		[Tooltip("the rect transform to shake the scale of. If left blank, this component will target the transform it's put on.")]
		[MMEnumCondition("Mode", new int[] { 1 })]
		public RectTransform TargetRectTransform;

		// Token: 0x04001333 RID: 4915
		[MMInspectorGroup("Shake Settings", true, 42, false)]
		[Tooltip("the speed at which the transform should shake")]
		public float ShakeSpeed = 20f;

		// Token: 0x04001334 RID: 4916
		[Tooltip("the maximum distance from its initial scale the transform will move to during the shake")]
		public float ShakeRange = 0.5f;

		// Token: 0x04001335 RID: 4917
		[MMInspectorGroup("Direction", true, 43, false)]
		[Tooltip("the direction along which to shake the transform's scale")]
		public Vector3 ShakeMainDirection = Vector3.up;

		// Token: 0x04001336 RID: 4918
		[Tooltip("if this is true, instead of using ShakeMainDirection as the direction of the shake, a random vector3 will be generated, randomized between ShakeMainDirection and ShakeAltDirection")]
		public bool RandomizeDirection;

		// Token: 0x04001337 RID: 4919
		[Tooltip("when in RandomizeDirection mode, a vector against which to randomize the main direction")]
		[MMCondition("RandomizeDirection", true)]
		public Vector3 ShakeAltDirection = Vector3.up;

		// Token: 0x04001338 RID: 4920
		[Tooltip("if this is true, a new direction will be randomized every time a shake happens")]
		public bool RandomizeDirectionOnPlay;

		// Token: 0x04001339 RID: 4921
		[MMInspectorGroup("Directional Noise", true, 47, false)]
		[Tooltip("whether or not to add noise to the main direction")]
		public bool AddDirectionalNoise = true;

		// Token: 0x0400133A RID: 4922
		[Tooltip("when adding directional noise, noise strength will be randomized between this value and DirectionalNoiseStrengthMax")]
		[MMCondition("AddDirectionalNoise", true)]
		public Vector3 DirectionalNoiseStrengthMin = new Vector3(0.25f, 0.25f, 0.25f);

		// Token: 0x0400133B RID: 4923
		[Tooltip("when adding directional noise, noise strength will be randomized between this value and DirectionalNoiseStrengthMin")]
		[MMCondition("AddDirectionalNoise", true)]
		public Vector3 DirectionalNoiseStrengthMax = new Vector3(0.25f, 0.25f, 0.25f);

		// Token: 0x0400133C RID: 4924
		[MMInspectorGroup("Randomness", true, 44, false)]
		[Tooltip("a unique seed you can use to get different outcomes when shaking more than one transform at once")]
		public Vector3 RandomnessSeed;

		// Token: 0x0400133D RID: 4925
		[Tooltip("whether or not to generate a unique seed automatically on every shake")]
		public bool RandomizeSeedOnShake = true;

		// Token: 0x0400133E RID: 4926
		[MMInspectorGroup("One Time", true, 45, false)]
		[Tooltip("whether or not to use attenuation, which will impact the amplitude of the shake, along the defined curve")]
		public bool UseAttenuation = true;

		// Token: 0x0400133F RID: 4927
		[Tooltip("the animation curve used to define attenuation, impacting the amplitude of the shake")]
		[MMCondition("UseAttenuation", true)]
		public AnimationCurve AttenuationCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001340 RID: 4928
		[MMInspectorGroup("Test", true, 46, false)]
		[MMInspectorButton("StartShaking")]
		public bool StartShakingButton;

		// Token: 0x04001341 RID: 4929
		protected float _attenuation = 1f;

		// Token: 0x04001342 RID: 4930
		protected float _oscillation;

		// Token: 0x04001343 RID: 4931
		protected Vector3 _initialScale;

		// Token: 0x04001344 RID: 4932
		protected Vector3 _workDirection;

		// Token: 0x04001345 RID: 4933
		protected Vector3 _noiseVector;

		// Token: 0x04001346 RID: 4934
		protected Vector3 _newScale;

		// Token: 0x04001347 RID: 4935
		protected Vector3 _randomNoiseStrength;

		// Token: 0x04001348 RID: 4936
		protected Vector3 _noNoise = Vector3.zero;

		// Token: 0x04001349 RID: 4937
		protected Vector3 _randomizedDirection;

		// Token: 0x0400134A RID: 4938
		protected float _originalDuration;

		// Token: 0x0400134B RID: 4939
		protected float _originalShakeSpeed;

		// Token: 0x0400134C RID: 4940
		protected float _originalShakeRange;

		// Token: 0x0400134D RID: 4941
		protected Vector3 _originalShakeMainDirection;

		// Token: 0x0400134E RID: 4942
		protected bool _originalRandomizeDirection;

		// Token: 0x0400134F RID: 4943
		protected Vector3 _originalShakeAltDirection;

		// Token: 0x04001350 RID: 4944
		protected bool _originalRandomizeDirectionOnPlay;

		// Token: 0x04001351 RID: 4945
		protected bool _originalAddDirectionalNoise;

		// Token: 0x04001352 RID: 4946
		protected Vector3 _originalDirectionalNoiseStrengthMin;

		// Token: 0x04001353 RID: 4947
		protected Vector3 _originalDirectionalNoiseStrengthMax;

		// Token: 0x04001354 RID: 4948
		protected Vector3 _originalRandomnessSeed;

		// Token: 0x04001355 RID: 4949
		protected bool _originalRandomizeSeedOnShake;

		// Token: 0x04001356 RID: 4950
		protected bool _originalUseAttenuation;

		// Token: 0x04001357 RID: 4951
		protected AnimationCurve _originalAttenuationCurve;

		// Token: 0x02000523 RID: 1315
		public enum Modes
		{
			// Token: 0x040023A2 RID: 9122
			Transform,
			// Token: 0x040023A3 RID: 9123
			RectTransform
		}
	}
}
