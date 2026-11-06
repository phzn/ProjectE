using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200036B RID: 875
	[RequireComponent(typeof(Volume))]
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMWhiteBalanceShaker_URP")]
	public class MMWhiteBalanceShaker_URP : MMShaker
	{
		// Token: 0x06001812 RID: 6162 RVA: 0x000663A0 File Offset: 0x000645A0
		protected override void Initialization()
		{
			base.Initialization();
			this._volume = base.gameObject.GetComponent<Volume>();
			this._volume.profile.TryGet<WhiteBalance>(out this._whiteBalance);
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x000663D0 File Offset: 0x000645D0
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakeTemperature, this.RemapTemperatureZero, this.RemapTemperatureOne, this.RelativeValues, this._initialTemperature);
			this._whiteBalance.temperature.Override(num);
			float num2 = this.ShakeFloat(this.ShakeTint, this.RemapTintZero, this.RemapTintOne, this.RelativeValues, this._initialTint);
			this._whiteBalance.tint.Override(num2);
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x00066449 File Offset: 0x00064649
		protected override void GrabInitialValues()
		{
			this._initialTemperature = this._whiteBalance.temperature.value;
			this._initialTint = this._whiteBalance.tint.value;
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x00066478 File Offset: 0x00064678
		public virtual void OnWhiteBalanceShakeEvent(AnimationCurve temperature, float duration, float remapTemperatureMin, float remapTemperatureMax, AnimationCurve tint, float remapTintMin, float remapTintMax, bool relativeValues = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
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
				this._originalShakeTemperature = this.ShakeTemperature;
				this._originalRemapTemperatureZero = this.RemapTemperatureZero;
				this._originalRemapTemperatureOne = this.RemapTemperatureOne;
				this._originalRelativeValues = this.RelativeValues;
				this._originalShakeTint = this.ShakeTint;
				this._originalRemapTintZero = this.RemapTintZero;
				this._originalRemapTintOne = this.RemapTintOne;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.TimescaleMode = timescaleMode;
				this.ShakeDuration = duration;
				this.ShakeTemperature = temperature;
				this.RemapTemperatureZero = remapTemperatureMin * attenuation;
				this.RemapTemperatureOne = remapTemperatureMax * attenuation;
				this.RelativeValues = relativeValues;
				this.ShakeTint = tint;
				this.RemapTintZero = remapTintMin;
				this.RemapTintOne = remapTintMax;
				this.ForwardDirection = forwardDirection;
			}
			this.Play();
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x0006659A File Offset: 0x0006479A
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this._whiteBalance.temperature.Override(this._initialTemperature);
			this._whiteBalance.tint.Override(this._initialTint);
		}

		// Token: 0x06001817 RID: 6167 RVA: 0x000665D0 File Offset: 0x000647D0
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.ShakeTemperature = this._originalShakeTemperature;
			this.RemapTemperatureZero = this._originalRemapTemperatureZero;
			this.RemapTemperatureOne = this._originalRemapTemperatureOne;
			this.RelativeValues = this._originalRelativeValues;
			this.ShakeTint = this._originalShakeTint;
			this.RemapTintZero = this._originalRemapTintZero;
			this.RemapTintOne = this._originalRemapTintOne;
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x00066643 File Offset: 0x00064843
		public override void StartListening()
		{
			base.StartListening();
			MMWhiteBalanceShakeEvent_URP.Register(new MMWhiteBalanceShakeEvent_URP.Delegate(this.OnWhiteBalanceShakeEvent));
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x0006665D File Offset: 0x0006485D
		public override void StopListening()
		{
			base.StopListening();
			MMWhiteBalanceShakeEvent_URP.Unregister(new MMWhiteBalanceShakeEvent_URP.Delegate(this.OnWhiteBalanceShakeEvent));
		}

		// Token: 0x04001BD4 RID: 7124
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeValues = true;

		// Token: 0x04001BD5 RID: 7125
		[MMInspectorGroup("Temperature", true, 55, false)]
		[Tooltip("the curve used to animate the temperature value on")]
		public AnimationCurve ShakeTemperature = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001BD6 RID: 7126
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-100f, 100f)]
		public float RemapTemperatureZero;

		// Token: 0x04001BD7 RID: 7127
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-100f, 100f)]
		public float RemapTemperatureOne = 100f;

		// Token: 0x04001BD8 RID: 7128
		[MMInspectorGroup("Tint", true, 56, false)]
		[Tooltip("the curve used to animate the tint value on")]
		public AnimationCurve ShakeTint = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001BD9 RID: 7129
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(-100f, 100f)]
		public float RemapTintZero;

		// Token: 0x04001BDA RID: 7130
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(-100f, 100f)]
		public float RemapTintOne = 100f;

		// Token: 0x04001BDB RID: 7131
		protected Volume _volume;

		// Token: 0x04001BDC RID: 7132
		protected WhiteBalance _whiteBalance;

		// Token: 0x04001BDD RID: 7133
		protected float _initialTemperature;

		// Token: 0x04001BDE RID: 7134
		protected float _initialTint;

		// Token: 0x04001BDF RID: 7135
		protected float _originalShakeDuration;

		// Token: 0x04001BE0 RID: 7136
		protected bool _originalRelativeValues;

		// Token: 0x04001BE1 RID: 7137
		protected AnimationCurve _originalShakeTemperature;

		// Token: 0x04001BE2 RID: 7138
		protected float _originalRemapTemperatureZero;

		// Token: 0x04001BE3 RID: 7139
		protected float _originalRemapTemperatureOne;

		// Token: 0x04001BE4 RID: 7140
		protected AnimationCurve _originalShakeTint;

		// Token: 0x04001BE5 RID: 7141
		protected float _originalRemapTintZero;

		// Token: 0x04001BE6 RID: 7142
		protected float _originalRemapTintOne;
	}
}
