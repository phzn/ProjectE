using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200024B RID: 587
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Lights/MMLightShaker")]
	[RequireComponent(typeof(Light))]
	public class MMLightShaker : MMShaker
	{
		// Token: 0x06001023 RID: 4131 RVA: 0x000478B6 File Offset: 0x00045AB6
		protected override void Initialization()
		{
			base.Initialization();
			if (this.BoundLight == null)
			{
				this.BoundLight = base.gameObject.GetComponent<Light>();
			}
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x000478DD File Offset: 0x00045ADD
		protected virtual void Reset()
		{
			this.ShakeDuration = 1f;
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x000478EC File Offset: 0x00045AEC
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.RangeCurve, this.RemapRangeZero, this.RemapRangeOne, this.RelativeValues, this._initialRange);
			this.BoundLight.range = num;
			float num2 = this.ShakeFloat(this.IntensityCurve, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeValues, this._initialIntensity);
			this.BoundLight.intensity = num2;
			float num3 = this.ShakeFloat(this.ShadowStrengthCurve, this.RemapShadowStrengthZero, this.RemapShadowStrengthOne, this.RelativeValues, this._initialShadowStrength);
			this.BoundLight.shadowStrength = Mathf.Clamp01(num3);
			if (this.ModifyColor)
			{
				this.BoundLight.color = this.ColorOverTime.Evaluate(this._remappedTimeSinceStart);
			}
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x000479B8 File Offset: 0x00045BB8
		protected override void GrabInitialValues()
		{
			this._initialColor = this.BoundLight.color;
			this._initialRange = this.BoundLight.range;
			this._initialIntensity = this.BoundLight.intensity;
			this._initialShadowStrength = this.BoundLight.shadowStrength;
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x00047A0C File Offset: 0x00045C0C
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this.BoundLight.color = this._initialColor;
			this.BoundLight.range = this._initialRange;
			this.BoundLight.intensity = this._initialIntensity;
			this.BoundLight.shadowStrength = this._initialShadowStrength;
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x00047A64 File Offset: 0x00045C64
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ModifyColor = this._originalModifyColor;
			this.RelativeValues = this._originalRelativeValues;
			this.ShakeDuration = this._originalShakeDuration;
			this.ColorOverTime = this._originalColorOverTime;
			this.IntensityCurve = this._originalIntensityCurve;
			this.RemapIntensityZero = this._originalRemapIntensityZero;
			this.RemapIntensityOne = this._originalRemapIntensityOne;
			this.RangeCurve = this._originalRangeCurve;
			this.RemapRangeZero = this._originalRemapRangeZero;
			this.RemapRangeOne = this._originalRemapRangeOne;
			this.ShadowStrengthCurve = this._originalShadowStrengthCurve;
			this.RemapShadowStrengthZero = this._originalRemapShadowStrengthZero;
			this.RemapShadowStrengthOne = this._originalRemapShadowStrengthOne;
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x00047B13 File Offset: 0x00045D13
		public override void StartListening()
		{
			base.StartListening();
			MMLightShakeEvent.Register(new MMLightShakeEvent.Delegate(this.OnMMLightShakeEvent));
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x00047B2D File Offset: 0x00045D2D
		public override void StopListening()
		{
			base.StopListening();
			MMLightShakeEvent.Unregister(new MMLightShakeEvent.Delegate(this.OnMMLightShakeEvent));
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x00047B48 File Offset: 0x00045D48
		public virtual void OnMMLightShakeEvent(float shakeDuration, bool relativeValues, bool modifyColor, Gradient colorOverTime, AnimationCurve intensityCurve, float remapIntensityZero, float remapIntensityOne, AnimationCurve rangeCurve, float remapRangeZero, float remapRangeOne, AnimationCurve shadowStrengthCurve, float remapShadowStrengthZero, float remapShadowStrengthOne, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool useRange = false, float eventRange = 0f, Vector3 eventOriginPosition = default(Vector3))
		{
			if (!this.CheckEventAllowed(channelData, useRange, eventRange, eventOriginPosition) || (!this.Interruptible && this.Shaking))
			{
				return;
			}
			this._resetShakerValuesAfterShake = resetShakerValuesAfterShake;
			this._resetTargetValuesAfterShake = resetTargetValuesAfterShake;
			if (resetShakerValuesAfterShake)
			{
				this._originalModifyColor = this.ModifyColor;
				this._originalRelativeValues = this.RelativeValues;
				this._originalShakeDuration = this.ShakeDuration;
				this._originalColorOverTime = this.ColorOverTime;
				this._originalIntensityCurve = this.IntensityCurve;
				this._originalRemapIntensityZero = this.RemapIntensityZero;
				this._originalRemapIntensityOne = this.RemapIntensityOne;
				this._originalRangeCurve = this.RangeCurve;
				this._originalRemapRangeZero = this.RemapRangeZero;
				this._originalRemapRangeOne = this.RemapRangeOne;
				this._originalShadowStrengthCurve = this.ShadowStrengthCurve;
				this._originalRemapShadowStrengthZero = this.RemapShadowStrengthZero;
				this._originalRemapShadowStrengthOne = this.RemapShadowStrengthOne;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.ModifyColor = modifyColor;
				this.RelativeValues = relativeValues;
				this.ShakeDuration = shakeDuration;
				this.ColorOverTime = colorOverTime;
				this.IntensityCurve = intensityCurve;
				this.RemapIntensityZero = remapIntensityZero;
				this.RemapIntensityOne = remapIntensityOne;
				this.RangeCurve = rangeCurve;
				this.RemapRangeZero = remapRangeZero;
				this.RemapRangeOne = remapRangeOne;
				this.ShadowStrengthCurve = shadowStrengthCurve;
				this.RemapShadowStrengthZero = remapShadowStrengthZero;
				this.RemapShadowStrengthOne = remapShadowStrengthOne;
			}
			this.Play();
		}

		// Token: 0x0400129C RID: 4764
		[MMInspectorGroup("Light", true, 37, false)]
		[Tooltip("the light to affect when playing the feedback")]
		public Light BoundLight;

		// Token: 0x0400129D RID: 4765
		[Tooltip("whether or not that light should be turned off on start")]
		public bool StartsOff = true;

		// Token: 0x0400129E RID: 4766
		[Tooltip("whether or not the values should be relative or not")]
		public bool RelativeValues = true;

		// Token: 0x0400129F RID: 4767
		[MMInspectorGroup("Color", true, 41, false)]
		[Tooltip("whether or not this shaker should modify color")]
		public bool ModifyColor = true;

		// Token: 0x040012A0 RID: 4768
		[Tooltip("the colors to apply to the light over time")]
		public Gradient ColorOverTime;

		// Token: 0x040012A1 RID: 4769
		[MMInspectorGroup("Intensity", true, 40, false)]
		[Tooltip("the intensity to apply to the light over time")]
		public AnimationCurve IntensityCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040012A2 RID: 4770
		[Tooltip("the value to remap the intensity curve's 0 to")]
		public float RemapIntensityZero;

		// Token: 0x040012A3 RID: 4771
		[Tooltip("the value to remap the intensity curve's 1 to")]
		public float RemapIntensityOne = 1f;

		// Token: 0x040012A4 RID: 4772
		[MMInspectorGroup("Range", true, 39, false)]
		[Tooltip("the range to apply to the light over time")]
		public AnimationCurve RangeCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040012A5 RID: 4773
		[Tooltip("the value to remap the range curve's 0 to")]
		public float RemapRangeZero;

		// Token: 0x040012A6 RID: 4774
		[Tooltip("the value to remap the range curve's 0 to")]
		public float RemapRangeOne = 10f;

		// Token: 0x040012A7 RID: 4775
		[MMInspectorGroup("Shadow Strength", true, 38, false)]
		[Tooltip("the range to apply to the light over time")]
		public AnimationCurve ShadowStrengthCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040012A8 RID: 4776
		[Tooltip("the value to remap the shadow strength's curve's 0 to")]
		public float RemapShadowStrengthZero;

		// Token: 0x040012A9 RID: 4777
		[Tooltip("the value to remap the shadow strength's curve's 1 to")]
		public float RemapShadowStrengthOne = 1f;

		// Token: 0x040012AA RID: 4778
		protected Color _initialColor;

		// Token: 0x040012AB RID: 4779
		protected float _initialRange;

		// Token: 0x040012AC RID: 4780
		protected float _initialIntensity;

		// Token: 0x040012AD RID: 4781
		protected float _initialShadowStrength;

		// Token: 0x040012AE RID: 4782
		protected bool _originalRelativeValues;

		// Token: 0x040012AF RID: 4783
		protected bool _originalModifyColor;

		// Token: 0x040012B0 RID: 4784
		protected float _originalShakeDuration;

		// Token: 0x040012B1 RID: 4785
		protected Gradient _originalColorOverTime;

		// Token: 0x040012B2 RID: 4786
		protected AnimationCurve _originalIntensityCurve;

		// Token: 0x040012B3 RID: 4787
		protected float _originalRemapIntensityZero;

		// Token: 0x040012B4 RID: 4788
		protected float _originalRemapIntensityOne;

		// Token: 0x040012B5 RID: 4789
		protected AnimationCurve _originalRangeCurve;

		// Token: 0x040012B6 RID: 4790
		protected float _originalRemapRangeZero;

		// Token: 0x040012B7 RID: 4791
		protected float _originalRemapRangeOne;

		// Token: 0x040012B8 RID: 4792
		protected AnimationCurve _originalShadowStrengthCurve;

		// Token: 0x040012B9 RID: 4793
		protected float _originalRemapShadowStrengthZero;

		// Token: 0x040012BA RID: 4794
		protected float _originalRemapShadowStrengthOne;
	}
}
