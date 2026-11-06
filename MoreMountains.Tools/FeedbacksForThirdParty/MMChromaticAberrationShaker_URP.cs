using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200035B RID: 859
	[RequireComponent(typeof(Volume))]
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMChromaticAberrationShaker_URP")]
	public class MMChromaticAberrationShaker_URP : MMShaker
	{
		// Token: 0x06001797 RID: 6039 RVA: 0x00063FFE File Offset: 0x000621FE
		protected override void Initialization()
		{
			base.Initialization();
			this._volume = base.gameObject.GetComponent<Volume>();
			this._volume.profile.TryGet<ChromaticAberration>(out this._chromaticAberration);
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x00064030 File Offset: 0x00062230
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakeIntensity, this.RemapIntensityZero, this.RemapIntensityOne, this.RelativeIntensity, this._initialIntensity);
			this._chromaticAberration.intensity.Override(num);
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x00064073 File Offset: 0x00062273
		protected override void GrabInitialValues()
		{
			this._initialIntensity = this._chromaticAberration.intensity.value;
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x0006408C File Offset: 0x0006228C
		public virtual void OnMMChromaticAberrationShakeEvent(AnimationCurve intensity, float duration, float remapMin, float remapMax, bool relativeIntensity = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
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
				this._originalShakeDuration = this.ShakeDuration;
				this._originalShakeIntensity = this.ShakeIntensity;
				this._originalRemapIntensityZero = this.RemapIntensityZero;
				this._originalRemapIntensityOne = this.RemapIntensityOne;
				this._originalRelativeIntensity = this.RelativeIntensity;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.TimescaleMode = timescaleMode;
				this.ShakeDuration = duration;
				this.ShakeIntensity = intensity;
				this.RemapIntensityZero = remapMin * attenuation;
				this.RemapIntensityOne = remapMax * attenuation;
				this.RelativeIntensity = relativeIntensity;
				this.ForwardDirection = forwardDirection;
			}
			this.Play();
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x00064172 File Offset: 0x00062372
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this._chromaticAberration.intensity.Override(this._initialIntensity);
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x00064190 File Offset: 0x00062390
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.ShakeIntensity = this._originalShakeIntensity;
			this.RemapIntensityZero = this._originalRemapIntensityZero;
			this.RemapIntensityOne = this._originalRemapIntensityOne;
			this.RelativeIntensity = this._originalRelativeIntensity;
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x000641DF File Offset: 0x000623DF
		public override void StartListening()
		{
			base.StartListening();
			MMChromaticAberrationShakeEvent_URP.Register(new MMChromaticAberrationShakeEvent_URP.Delegate(this.OnMMChromaticAberrationShakeEvent));
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x000641F9 File Offset: 0x000623F9
		public override void StopListening()
		{
			base.StopListening();
			MMChromaticAberrationShakeEvent_URP.Unregister(new MMChromaticAberrationShakeEvent_URP.Delegate(this.OnMMChromaticAberrationShakeEvent));
		}

		// Token: 0x04001B35 RID: 6965
		[MMInspectorGroup("Chromatic Aberration Intensity", true, 45, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeIntensity;

		// Token: 0x04001B36 RID: 6966
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeIntensity = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001B37 RID: 6967
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 1f)]
		public float RemapIntensityZero;

		// Token: 0x04001B38 RID: 6968
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapIntensityOne = 1f;

		// Token: 0x04001B39 RID: 6969
		protected Volume _volume;

		// Token: 0x04001B3A RID: 6970
		protected ChromaticAberration _chromaticAberration;

		// Token: 0x04001B3B RID: 6971
		protected float _initialIntensity;

		// Token: 0x04001B3C RID: 6972
		protected float _originalShakeDuration;

		// Token: 0x04001B3D RID: 6973
		protected AnimationCurve _originalShakeIntensity;

		// Token: 0x04001B3E RID: 6974
		protected float _originalRemapIntensityZero;

		// Token: 0x04001B3F RID: 6975
		protected float _originalRemapIntensityOne;

		// Token: 0x04001B40 RID: 6976
		protected bool _originalRelativeIntensity;
	}
}
