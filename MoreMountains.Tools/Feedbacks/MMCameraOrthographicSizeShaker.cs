using System;
using MoreMountains.Tools;
using UnityEngine;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200023D RID: 573
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/Camera/MMCameraOrthographicSizeShaker")]
	[RequireComponent(typeof(Camera))]
	public class MMCameraOrthographicSizeShaker : MMShaker
	{
		// Token: 0x06000FDA RID: 4058 RVA: 0x00046B1E File Offset: 0x00044D1E
		protected override void Initialization()
		{
			base.Initialization();
			this._targetCamera = base.gameObject.GetComponent<Camera>();
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x00046B37 File Offset: 0x00044D37
		protected virtual void Reset()
		{
			this.ShakeDuration = 0.5f;
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x00046B44 File Offset: 0x00044D44
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakeOrthographicSize, this.RemapOrthographicSizeZero, this.RemapOrthographicSizeOne, this.RelativeOrthographicSize, this._initialOrthographicSize);
			this._targetCamera.orthographicSize = num;
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x00046B82 File Offset: 0x00044D82
		protected override void GrabInitialValues()
		{
			this._initialOrthographicSize = this._targetCamera.orthographicSize;
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x00046B98 File Offset: 0x00044D98
		public virtual void OnMMCameraOrthographicSizeShakeEvent(AnimationCurve distortionCurve, float duration, float remapMin, float remapMax, bool relativeDistortion = false, float feedbacksIntensity = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
		{
			if (!this.CheckEventAllowed(channelData, false, 0f, default(Vector3)))
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
			if (!this.Interruptible && this.Shaking)
			{
				return;
			}
			this._resetShakerValuesAfterShake = resetShakerValuesAfterShake;
			this._resetTargetValuesAfterShake = resetTargetValuesAfterShake;
			if (resetShakerValuesAfterShake)
			{
				this._originalShakeDuration = this.ShakeDuration;
				this._originalShakeOrthographicSize = this.ShakeOrthographicSize;
				this._originalRemapOrthographicSizeZero = this.RemapOrthographicSizeZero;
				this._originalRemapOrthographicSizeOne = this.RemapOrthographicSizeOne;
				this._originalRelativeOrthographicSize = this.RelativeOrthographicSize;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.TimescaleMode = timescaleMode;
				this.ShakeDuration = duration;
				this.ShakeOrthographicSize = distortionCurve;
				this.RemapOrthographicSizeZero = remapMin * feedbacksIntensity;
				this.RemapOrthographicSizeOne = remapMax * feedbacksIntensity;
				this.RelativeOrthographicSize = relativeDistortion;
				this.ForwardDirection = forwardDirection;
			}
			this.Play();
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x00046C7F File Offset: 0x00044E7F
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this._targetCamera.orthographicSize = this._initialOrthographicSize;
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x00046C98 File Offset: 0x00044E98
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.ShakeOrthographicSize = this._originalShakeOrthographicSize;
			this.RemapOrthographicSizeZero = this._originalRemapOrthographicSizeZero;
			this.RemapOrthographicSizeOne = this._originalRemapOrthographicSizeOne;
			this.RelativeOrthographicSize = this._originalRelativeOrthographicSize;
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x00046CE7 File Offset: 0x00044EE7
		public override void StartListening()
		{
			base.StartListening();
			MMCameraOrthographicSizeShakeEvent.Register(new MMCameraOrthographicSizeShakeEvent.Delegate(this.OnMMCameraOrthographicSizeShakeEvent));
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x00046D01 File Offset: 0x00044F01
		public override void StopListening()
		{
			base.StopListening();
			MMCameraOrthographicSizeShakeEvent.Unregister(new MMCameraOrthographicSizeShakeEvent.Delegate(this.OnMMCameraOrthographicSizeShakeEvent));
		}

		// Token: 0x0400125F RID: 4703
		[MMInspectorGroup("Orthographic Size", true, 37, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeOrthographicSize;

		// Token: 0x04001260 RID: 4704
		[Tooltip("the curve used to animate the intensity value on")]
		public AnimationCurve ShakeOrthographicSize = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001261 RID: 4705
		[Tooltip("the value to remap the curve's 0 to")]
		public float RemapOrthographicSizeZero = 5f;

		// Token: 0x04001262 RID: 4706
		[Tooltip("the value to remap the curve's 1 to")]
		public float RemapOrthographicSizeOne = 10f;

		// Token: 0x04001263 RID: 4707
		protected Camera _targetCamera;

		// Token: 0x04001264 RID: 4708
		protected float _initialOrthographicSize;

		// Token: 0x04001265 RID: 4709
		protected float _originalShakeDuration;

		// Token: 0x04001266 RID: 4710
		protected bool _originalRelativeOrthographicSize;

		// Token: 0x04001267 RID: 4711
		protected AnimationCurve _originalShakeOrthographicSize;

		// Token: 0x04001268 RID: 4712
		protected float _originalRemapOrthographicSizeZero;

		// Token: 0x04001269 RID: 4713
		protected float _originalRemapOrthographicSizeOne;
	}
}
