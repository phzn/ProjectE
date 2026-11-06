using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x0200035F RID: 863
	[RequireComponent(typeof(Volume))]
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMDepthOfFieldShaker_URP")]
	public class MMDepthOfFieldShaker_URP : MMShaker
	{
		// Token: 0x060017B6 RID: 6070 RVA: 0x00064BBC File Offset: 0x00062DBC
		protected override void Initialization()
		{
			base.Initialization();
			this._volume = base.gameObject.GetComponent<Volume>();
			this._volume.profile.TryGet<DepthOfField>(out this._depthOfField);
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x00064BEC File Offset: 0x00062DEC
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakeFocusDistance, this.RemapFocusDistanceZero, this.RemapFocusDistanceOne, this.RelativeValues, this._initialFocusDistance);
			this._depthOfField.focusDistance.Override(num);
			float num2 = this.ShakeFloat(this.ShakeAperture, this.RemapApertureZero, this.RemapApertureOne, this.RelativeValues, this._initialAperture);
			this._depthOfField.aperture.Override(num2);
			float num3 = this.ShakeFloat(this.ShakeFocalLength, this.RemapFocalLengthZero, this.RemapFocalLengthOne, this.RelativeValues, this._initialFocalLength);
			this._depthOfField.focalLength.Override(num3);
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x00064C9B File Offset: 0x00062E9B
		protected virtual void Reset()
		{
			this.ShakeDuration = 2f;
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x00064CA8 File Offset: 0x00062EA8
		protected override void GrabInitialValues()
		{
			this._initialFocusDistance = this._depthOfField.focusDistance.value;
			this._initialAperture = this._depthOfField.aperture.value;
			this._initialFocalLength = this._depthOfField.focalLength.value;
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x00064CF8 File Offset: 0x00062EF8
		public virtual void OnDepthOfFieldShakeEvent(AnimationCurve focusDistance, float duration, float remapFocusDistanceMin, float remapFocusDistanceMax, AnimationCurve aperture, float remapApertureMin, float remapApertureMax, AnimationCurve focalLength, float remapFocalLengthMin, float remapFocalLengthMax, bool relativeValues = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
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
				this._originalShakeFocusDistance = this.ShakeFocusDistance;
				this._originalRemapFocusDistanceZero = this.RemapFocusDistanceZero;
				this._originalRemapFocusDistanceOne = this.RemapFocusDistanceOne;
				this._originalShakeAperture = this.ShakeAperture;
				this._originalRemapApertureZero = this.RemapApertureZero;
				this._originalRemapApertureOne = this.RemapApertureOne;
				this._originalShakeFocalLength = this.ShakeFocalLength;
				this._originalRemapFocalLengthZero = this.RemapFocalLengthZero;
				this._originalRemapFocalLengthOne = this.RemapFocalLengthOne;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.TimescaleMode = timescaleMode;
				this.ShakeDuration = duration;
				this.RelativeValues = relativeValues;
				this.ShakeFocusDistance = focusDistance;
				this.RemapFocusDistanceZero = remapFocusDistanceMin;
				this.RemapFocusDistanceOne = remapFocusDistanceMax;
				this.ShakeAperture = aperture;
				this.RemapApertureZero = remapApertureMin;
				this.RemapApertureOne = remapApertureMax;
				this.ShakeFocalLength = focalLength;
				this.RemapFocalLengthZero = remapFocalLengthMin;
				this.RemapFocalLengthOne = remapFocalLengthMax;
				this.ForwardDirection = forwardDirection;
			}
			this.Play();
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x00064E54 File Offset: 0x00063054
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this._depthOfField.focusDistance.Override(this._initialFocusDistance);
			this._depthOfField.aperture.Override(this._initialAperture);
			this._depthOfField.focalLength.Override(this._initialFocalLength);
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x00064EAC File Offset: 0x000630AC
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.RelativeValues = this._originalRelativeValues;
			this.ShakeFocusDistance = this._originalShakeFocusDistance;
			this.RemapFocusDistanceZero = this._originalRemapFocusDistanceZero;
			this.RemapFocusDistanceOne = this._originalRemapFocusDistanceOne;
			this.ShakeAperture = this._originalShakeAperture;
			this.RemapApertureZero = this._originalRemapApertureZero;
			this.RemapApertureOne = this._originalRemapApertureOne;
			this.ShakeFocalLength = this._originalShakeFocalLength;
			this.RemapFocalLengthZero = this._originalRemapFocalLengthZero;
			this.RemapFocalLengthOne = this._originalRemapFocalLengthOne;
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x00064F43 File Offset: 0x00063143
		public override void StartListening()
		{
			base.StartListening();
			MMDepthOfFieldShakeEvent_URP.Register(new MMDepthOfFieldShakeEvent_URP.Delegate(this.OnDepthOfFieldShakeEvent));
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x00064F5D File Offset: 0x0006315D
		public override void StopListening()
		{
			base.StopListening();
			MMDepthOfFieldShakeEvent_URP.Unregister(new MMDepthOfFieldShakeEvent_URP.Delegate(this.OnDepthOfFieldShakeEvent));
		}

		// Token: 0x04001B6D RID: 7021
		public bool RelativeValues = true;

		// Token: 0x04001B6E RID: 7022
		[MMInspectorGroup("Focus Distance", true, 51, false)]
		[Tooltip("the curve used to animate the focus distance value on")]
		public AnimationCurve ShakeFocusDistance = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001B6F RID: 7023
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapFocusDistanceZero;

		// Token: 0x04001B70 RID: 7024
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapFocusDistanceOne = 3f;

		// Token: 0x04001B71 RID: 7025
		[MMInspectorGroup("Aperture", true, 52, false)]
		[Tooltip("the curve used to animate the aperture value on")]
		public AnimationCurve ShakeAperture = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001B72 RID: 7026
		[Range(0.1f, 32f)]
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapApertureZero;

		// Token: 0x04001B73 RID: 7027
		[Range(0.1f, 32f)]
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapApertureOne;

		// Token: 0x04001B74 RID: 7028
		[MMInspectorGroup("Focal Length", true, 53, false)]
		[Tooltip("the curve used to animate the focal length value on")]
		public AnimationCurve ShakeFocalLength = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001B75 RID: 7029
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 300f)]
		public float RemapFocalLengthZero;

		// Token: 0x04001B76 RID: 7030
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 300f)]
		public float RemapFocalLengthOne;

		// Token: 0x04001B77 RID: 7031
		protected Volume _volume;

		// Token: 0x04001B78 RID: 7032
		protected DepthOfField _depthOfField;

		// Token: 0x04001B79 RID: 7033
		protected float _initialFocusDistance;

		// Token: 0x04001B7A RID: 7034
		protected float _initialAperture;

		// Token: 0x04001B7B RID: 7035
		protected float _initialFocalLength;

		// Token: 0x04001B7C RID: 7036
		protected float _originalShakeDuration;

		// Token: 0x04001B7D RID: 7037
		protected bool _originalRelativeValues;

		// Token: 0x04001B7E RID: 7038
		protected AnimationCurve _originalShakeFocusDistance;

		// Token: 0x04001B7F RID: 7039
		protected float _originalRemapFocusDistanceZero;

		// Token: 0x04001B80 RID: 7040
		protected float _originalRemapFocusDistanceOne;

		// Token: 0x04001B81 RID: 7041
		protected AnimationCurve _originalShakeAperture;

		// Token: 0x04001B82 RID: 7042
		protected float _originalRemapApertureZero;

		// Token: 0x04001B83 RID: 7043
		protected float _originalRemapApertureOne;

		// Token: 0x04001B84 RID: 7044
		protected AnimationCurve _originalShakeFocalLength;

		// Token: 0x04001B85 RID: 7045
		protected float _originalRemapFocalLengthZero;

		// Token: 0x04001B86 RID: 7046
		protected float _originalRemapFocalLengthOne;
	}
}
