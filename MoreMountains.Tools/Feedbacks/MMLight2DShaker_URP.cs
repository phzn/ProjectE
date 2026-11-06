using System;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020002C8 RID: 712
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Lights/MMLight2DShaker_URP")]
	[RequireComponent(typeof(Light2D))]
	public class MMLight2DShaker_URP : MMShaker
	{
		// Token: 0x0600142C RID: 5164 RVA: 0x00056F6F File Offset: 0x0005516F
		protected override void Initialization()
		{
			base.Initialization();
			if (this.BoundLight == null)
			{
				this.BoundLight = base.gameObject.GetComponent<Light2D>();
			}
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x00056F96 File Offset: 0x00055196
		protected virtual void Reset()
		{
			this.ShakeDuration = 1f;
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x00056FA4 File Offset: 0x000551A4
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.FalloffCurve, this.FalloffRangeZero, this.RemapFalloffOne, this.RelativeValues, this._initialRange);
			this.BoundLight.shapeLightFalloffSize = num;
			float num2 = this.ShakeFloat(this.IntensityCurve, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeValues, this._initialIntensity);
			this.BoundLight.intensity = num2;
			float num3 = this.ShakeFloat(this.ShadowStrengthCurve, this.RemapShadowStrengthZero, this.RemapShadowStrengthOne, this.RelativeValues, this._initialShadowStrength);
			this.BoundLight.shadowIntensity = Mathf.Clamp01(num3);
			if (this.ModifyColor)
			{
				this.BoundLight.color = this.ColorOverTime.Evaluate(this._remappedTimeSinceStart);
			}
		}

		// Token: 0x0600142F RID: 5167 RVA: 0x00057070 File Offset: 0x00055270
		protected override void GrabInitialValues()
		{
			this._initialColor = this.BoundLight.color;
			this._initialRange = this.BoundLight.shapeLightFalloffSize;
			this._initialIntensity = this.BoundLight.intensity;
			this._initialShadowStrength = this.BoundLight.shadowIntensity;
		}

		// Token: 0x06001430 RID: 5168 RVA: 0x000570C4 File Offset: 0x000552C4
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this.BoundLight.color = this._initialColor;
			this.BoundLight.shapeLightFalloffSize = this._initialRange;
			this.BoundLight.intensity = this._initialIntensity;
			this.BoundLight.shadowIntensity = this._initialShadowStrength;
		}

		// Token: 0x06001431 RID: 5169 RVA: 0x0005711C File Offset: 0x0005531C
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
			this.FalloffCurve = this._originalRangeCurve;
			this.FalloffRangeZero = this._originalRemapRangeZero;
			this.RemapFalloffOne = this._originalRemapRangeOne;
			this.ShadowStrengthCurve = this._originalShadowStrengthCurve;
			this.RemapShadowStrengthZero = this._originalRemapShadowStrengthZero;
			this.RemapShadowStrengthOne = this._originalRemapShadowStrengthOne;
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x000571CB File Offset: 0x000553CB
		public override void StartListening()
		{
			base.StartListening();
			MMLight2DShakeEvent.Register(new MMLight2DShakeEvent.Delegate(this.OnMMLight2DShakeEvent));
		}

		// Token: 0x06001433 RID: 5171 RVA: 0x000571E5 File Offset: 0x000553E5
		public override void StopListening()
		{
			base.StopListening();
			MMLight2DShakeEvent.Unregister(new MMLight2DShakeEvent.Delegate(this.OnMMLight2DShakeEvent));
		}

		// Token: 0x06001434 RID: 5172 RVA: 0x00057200 File Offset: 0x00055400
		public virtual void OnMMLight2DShakeEvent(float shakeDuration, bool relativeValues, bool modifyColor, Gradient colorOverTime, AnimationCurve intensityCurve, float remapIntensityZero, float remapIntensityOne, AnimationCurve rangeCurve, float remapRangeZero, float remapRangeOne, AnimationCurve shadowStrengthCurve, float remapShadowStrengthZero, float remapShadowStrengthOne, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool useRange = false, float eventRange = 0f, Vector3 eventOriginPosition = default(Vector3))
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
				this._originalRangeCurve = this.FalloffCurve;
				this._originalRemapRangeZero = this.FalloffRangeZero;
				this._originalRemapRangeOne = this.RemapFalloffOne;
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
				this.FalloffCurve = rangeCurve;
				this.FalloffRangeZero = remapRangeZero;
				this.RemapFalloffOne = remapRangeOne;
				this.ShadowStrengthCurve = shadowStrengthCurve;
				this.RemapShadowStrengthZero = remapShadowStrengthZero;
				this.RemapShadowStrengthOne = remapShadowStrengthOne;
			}
			this.Play();
		}

		// Token: 0x04001734 RID: 5940
		[MMInspectorGroup("Light", true, 37, false)]
		[Tooltip("the light to affect when playing the feedback")]
		public Light2D BoundLight;

		// Token: 0x04001735 RID: 5941
		[Tooltip("whether or not that light should be turned off on start")]
		public bool StartsOff = true;

		// Token: 0x04001736 RID: 5942
		[Tooltip("whether or not the values should be relative or not")]
		public bool RelativeValues = true;

		// Token: 0x04001737 RID: 5943
		[MMInspectorGroup("Color", true, 41, false)]
		[Tooltip("whether or not this shaker should modify color")]
		public bool ModifyColor = true;

		// Token: 0x04001738 RID: 5944
		[Tooltip("the colors to apply to the light over time")]
		public Gradient ColorOverTime;

		// Token: 0x04001739 RID: 5945
		[MMInspectorGroup("Intensity", true, 40, false)]
		[Tooltip("the intensity to apply to the light over time")]
		public AnimationCurve IntensityCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400173A RID: 5946
		[Tooltip("the value to remap the intensity curve's 0 to")]
		public float RemapIntensityZero;

		// Token: 0x0400173B RID: 5947
		[Tooltip("the value to remap the intensity curve's 1 to")]
		public float RemapIntensityOne = 1f;

		// Token: 0x0400173C RID: 5948
		[MMInspectorGroup("Range", true, 39, false)]
		[Tooltip("the range to apply to the light over time")]
		public AnimationCurve FalloffCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x0400173D RID: 5949
		[Tooltip("the value to remap the range curve's 0 to")]
		public float FalloffRangeZero;

		// Token: 0x0400173E RID: 5950
		[Tooltip("the value to remap the range curve's 0 to")]
		public float RemapFalloffOne = 10f;

		// Token: 0x0400173F RID: 5951
		[MMInspectorGroup("Shadow Strength", true, 38, false)]
		[Tooltip("the range to apply to the light over time")]
		public AnimationCurve ShadowStrengthCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001740 RID: 5952
		[Tooltip("the value to remap the shadow strength's curve's 0 to")]
		public float RemapShadowStrengthZero;

		// Token: 0x04001741 RID: 5953
		[Tooltip("the value to remap the shadow strength's curve's 1 to")]
		public float RemapShadowStrengthOne = 1f;

		// Token: 0x04001742 RID: 5954
		protected Color _initialColor;

		// Token: 0x04001743 RID: 5955
		protected float _initialRange;

		// Token: 0x04001744 RID: 5956
		protected float _initialIntensity;

		// Token: 0x04001745 RID: 5957
		protected float _initialShadowStrength;

		// Token: 0x04001746 RID: 5958
		protected bool _originalRelativeValues;

		// Token: 0x04001747 RID: 5959
		protected bool _originalModifyColor;

		// Token: 0x04001748 RID: 5960
		protected float _originalShakeDuration;

		// Token: 0x04001749 RID: 5961
		protected Gradient _originalColorOverTime;

		// Token: 0x0400174A RID: 5962
		protected AnimationCurve _originalIntensityCurve;

		// Token: 0x0400174B RID: 5963
		protected float _originalRemapIntensityZero;

		// Token: 0x0400174C RID: 5964
		protected float _originalRemapIntensityOne;

		// Token: 0x0400174D RID: 5965
		protected AnimationCurve _originalRangeCurve;

		// Token: 0x0400174E RID: 5966
		protected float _originalRemapRangeZero;

		// Token: 0x0400174F RID: 5967
		protected float _originalRemapRangeOne;

		// Token: 0x04001750 RID: 5968
		protected AnimationCurve _originalShadowStrengthCurve;

		// Token: 0x04001751 RID: 5969
		protected float _originalRemapShadowStrengthZero;

		// Token: 0x04001752 RID: 5970
		protected float _originalRemapShadowStrengthOne;
	}
}
