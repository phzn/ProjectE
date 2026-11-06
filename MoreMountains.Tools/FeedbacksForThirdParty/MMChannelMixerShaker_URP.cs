using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000359 RID: 857
	[RequireComponent(typeof(Volume))]
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMChannelMixerShaker_URP")]
	public class MMChannelMixerShaker_URP : MMShaker
	{
		// Token: 0x06001787 RID: 6023 RVA: 0x00063A4C File Offset: 0x00061C4C
		protected override void Initialization()
		{
			base.Initialization();
			this._volume = base.gameObject.GetComponent<Volume>();
			this._volume.profile.TryGet<ChannelMixer>(out this._channelMixer);
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x00063A7C File Offset: 0x00061C7C
		protected virtual void Reset()
		{
			this.ShakeDuration = 0.8f;
		}

		// Token: 0x06001789 RID: 6025 RVA: 0x00063A8C File Offset: 0x00061C8C
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakeRed, this.RemapRedZero, this.RemapRedOne, this.RelativeValues, this._initialRed);
			this._channelMixer.redOutRedIn.Override(num);
			float num2 = this.ShakeFloat(this.ShakeGreen, this.RemapGreenZero, this.RemapGreenOne, this.RelativeValues, this._initialGreen);
			this._channelMixer.greenOutGreenIn.Override(num2);
			float num3 = this.ShakeFloat(this.ShakeBlue, this.RemapBlueZero, this.RemapBlueOne, this.RelativeValues, this._initialBlue);
			this._channelMixer.blueOutBlueIn.Override(num3);
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x00063B3C File Offset: 0x00061D3C
		protected override void GrabInitialValues()
		{
			this._initialRed = this._channelMixer.redOutRedIn.value;
			this._initialGreen = this._channelMixer.greenOutGreenIn.value;
			this._initialBlue = this._channelMixer.blueOutBlueIn.value;
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x00063B8C File Offset: 0x00061D8C
		public virtual void OnMMChannelMixerShakeEvent(AnimationCurve shakeRed, float remapRedZero, float remapRedOne, AnimationCurve shakeGreen, float remapGreenZero, float remapGreenOne, AnimationCurve shakeBlue, float remapBlueZero, float remapBlueOne, float duration, bool relativeValues = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
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
				this._originalRelativeValues = this.RelativeValues;
				this._originalShakeRed = this.ShakeRed;
				this._originalRemapRedZero = this.RemapRedZero;
				this._originalRemapRedOne = this.RemapRedOne;
				this._originalShakeGreen = this.ShakeGreen;
				this._originalRemapGreenZero = this.RemapGreenZero;
				this._originalRemapGreenOne = this.RemapGreenOne;
				this._originalShakeBlue = this.ShakeBlue;
				this._originalRemapBlueZero = this.RemapBlueZero;
				this._originalRemapBlueOne = this.RemapBlueOne;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.TimescaleMode = timescaleMode;
				this.ShakeDuration = duration;
				this.RelativeValues = relativeValues;
				this.ShakeRed = shakeRed;
				this.RemapRedZero = remapRedZero;
				this.RemapRedOne = remapRedOne;
				this.ShakeGreen = shakeGreen;
				this.RemapGreenZero = remapGreenZero;
				this.RemapGreenOne = remapGreenOne;
				this.ShakeBlue = shakeBlue;
				this.RemapBlueZero = remapBlueZero;
				this.RemapBlueOne = remapBlueOne;
				this.ForwardDirection = forwardDirection;
			}
			this.Play();
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x00063CE8 File Offset: 0x00061EE8
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this._channelMixer.redOutRedIn.Override(this._initialRed);
			this._channelMixer.greenOutGreenIn.Override(this._initialGreen);
			this._channelMixer.blueOutBlueIn.Override(this._initialBlue);
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x00063D40 File Offset: 0x00061F40
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.RelativeValues = this._originalRelativeValues;
			this.ShakeRed = this._originalShakeRed;
			this.RemapRedZero = this._originalRemapRedZero;
			this.RemapRedOne = this._originalRemapRedOne;
			this.ShakeGreen = this._originalShakeGreen;
			this.RemapGreenZero = this._originalRemapGreenZero;
			this.RemapGreenOne = this._originalRemapGreenOne;
			this.ShakeBlue = this._originalShakeBlue;
			this.RemapBlueZero = this._originalRemapBlueZero;
			this.RemapBlueOne = this._originalRemapBlueOne;
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x00063DD7 File Offset: 0x00061FD7
		public override void StartListening()
		{
			base.StartListening();
			MMChannelMixerShakeEvent_URP.Register(new MMChannelMixerShakeEvent_URP.Delegate(this.OnMMChannelMixerShakeEvent));
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x00063DF1 File Offset: 0x00061FF1
		public override void StopListening()
		{
			base.StopListening();
			MMChannelMixerShakeEvent_URP.Unregister(new MMChannelMixerShakeEvent_URP.Delegate(this.OnMMChannelMixerShakeEvent));
		}

		// Token: 0x04001B18 RID: 6936
		public bool RelativeValues = true;

		// Token: 0x04001B19 RID: 6937
		[MMInspectorGroup("Red", true, 43, false)]
		[Tooltip("the curve used to animate the red value on")]
		public AnimationCurve ShakeRed = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001B1A RID: 6938
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-200f, 200f)]
		public float RemapRedZero;

		// Token: 0x04001B1B RID: 6939
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-200f, 200f)]
		public float RemapRedOne = 200f;

		// Token: 0x04001B1C RID: 6940
		[MMInspectorGroup("Green", true, 44, false)]
		[Tooltip("the curve used to animate the green value on")]
		public AnimationCurve ShakeGreen = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001B1D RID: 6941
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-200f, 200f)]
		public float RemapGreenZero;

		// Token: 0x04001B1E RID: 6942
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-200f, 200f)]
		public float RemapGreenOne = 200f;

		// Token: 0x04001B1F RID: 6943
		[MMInspectorGroup("Blue", true, 45, false)]
		[Tooltip("the curve used to animate the blue value on")]
		public AnimationCurve ShakeBlue = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001B20 RID: 6944
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-200f, 200f)]
		public float RemapBlueZero;

		// Token: 0x04001B21 RID: 6945
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-200f, 200f)]
		public float RemapBlueOne = 200f;

		// Token: 0x04001B22 RID: 6946
		protected Volume _volume;

		// Token: 0x04001B23 RID: 6947
		protected ChannelMixer _channelMixer;

		// Token: 0x04001B24 RID: 6948
		protected float _initialRed;

		// Token: 0x04001B25 RID: 6949
		protected float _initialGreen;

		// Token: 0x04001B26 RID: 6950
		protected float _initialBlue;

		// Token: 0x04001B27 RID: 6951
		protected float _initialContrast;

		// Token: 0x04001B28 RID: 6952
		protected Color _initialColorFilterColor;

		// Token: 0x04001B29 RID: 6953
		protected float _originalShakeDuration;

		// Token: 0x04001B2A RID: 6954
		protected bool _originalRelativeValues;

		// Token: 0x04001B2B RID: 6955
		protected AnimationCurve _originalShakeRed;

		// Token: 0x04001B2C RID: 6956
		protected float _originalRemapRedZero;

		// Token: 0x04001B2D RID: 6957
		protected float _originalRemapRedOne;

		// Token: 0x04001B2E RID: 6958
		protected AnimationCurve _originalShakeGreen;

		// Token: 0x04001B2F RID: 6959
		protected float _originalRemapGreenZero;

		// Token: 0x04001B30 RID: 6960
		protected float _originalRemapGreenOne;

		// Token: 0x04001B31 RID: 6961
		protected AnimationCurve _originalShakeBlue;

		// Token: 0x04001B32 RID: 6962
		protected float _originalRemapBlueZero;

		// Token: 0x04001B33 RID: 6963
		protected float _originalRemapBlueOne;
	}
}
