using System;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace MoreMountains.FeedbacksForThirdParty
{
	// Token: 0x02000367 RID: 871
	[RequireComponent(typeof(Volume))]
	[AddComponentMenu("More Mountains/Feedbacks/Shakers/PostProcessing/MMPaniniProjectionShaker_URP")]
	public class MMPaniniProjectionShaker_URP : MMShaker
	{
		// Token: 0x060017F4 RID: 6132 RVA: 0x00065BB2 File Offset: 0x00063DB2
		protected override void Initialization()
		{
			base.Initialization();
			this._volume = base.gameObject.GetComponent<Volume>();
			this._volume.profile.TryGet<PaniniProjection>(out this._paniniProjection);
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x00065BE4 File Offset: 0x00063DE4
		protected override void Shake()
		{
			float num = this.ShakeFloat(this.ShakeDistance, this.RemapDistanceZero, this.RemapDistanceOne, this.RelativeDistance, this._initialDistance);
			this._paniniProjection.distance.Override(num);
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x00065C27 File Offset: 0x00063E27
		protected override void GrabInitialValues()
		{
			this._initialDistance = this._paniniProjection.distance.value;
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x00065C40 File Offset: 0x00063E40
		public virtual void OnPaniniProjectionShakeEvent(AnimationCurve distance, float duration, float remapMin, float remapMax, bool relativeDistance = false, float attenuation = 1f, MMChannelData channelData = null, bool resetShakerValuesAfterShake = true, bool resetTargetValuesAfterShake = true, bool forwardDirection = true, TimescaleModes timescaleMode = TimescaleModes.Scaled, bool stop = false, bool restore = false)
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
				this._originalShakeDistance = this.ShakeDistance;
				this._originalRemapDistanceZero = this.RemapDistanceZero;
				this._originalRemapDistanceOne = this.RemapDistanceOne;
				this._originalRelativeDistance = this.RelativeDistance;
			}
			if (!this.OnlyUseShakerValues)
			{
				this.TimescaleMode = timescaleMode;
				this.ShakeDuration = duration;
				this.ShakeDistance = distance;
				this.RemapDistanceZero = remapMin * attenuation;
				this.RemapDistanceOne = remapMax * attenuation;
				this.RelativeDistance = relativeDistance;
				this.ForwardDirection = forwardDirection;
			}
			this.Play();
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x00065D26 File Offset: 0x00063F26
		protected override void ResetTargetValues()
		{
			base.ResetTargetValues();
			this._paniniProjection.distance.Override(this._initialDistance);
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x00065D44 File Offset: 0x00063F44
		protected override void ResetShakerValues()
		{
			base.ResetShakerValues();
			this.ShakeDuration = this._originalShakeDuration;
			this.ShakeDistance = this._originalShakeDistance;
			this.RemapDistanceZero = this._originalRemapDistanceZero;
			this.RemapDistanceOne = this._originalRemapDistanceOne;
			this.RelativeDistance = this._originalRelativeDistance;
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x00065D93 File Offset: 0x00063F93
		public override void StartListening()
		{
			base.StartListening();
			MMPaniniProjectionShakeEvent_URP.Register(new MMPaniniProjectionShakeEvent_URP.Delegate(this.OnPaniniProjectionShakeEvent));
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x00065DAD File Offset: 0x00063FAD
		public override void StopListening()
		{
			base.StopListening();
			MMPaniniProjectionShakeEvent_URP.Unregister(new MMPaniniProjectionShakeEvent_URP.Delegate(this.OnPaniniProjectionShakeEvent));
		}

		// Token: 0x04001BAF RID: 7087
		[MMInspectorGroup("Distance", true, 62, false)]
		[Tooltip("whether or not to add to the initial value")]
		public bool RelativeDistance;

		// Token: 0x04001BB0 RID: 7088
		[Tooltip("the curve used to animate the distance value on")]
		public AnimationCurve ShakeDistance = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.5f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04001BB1 RID: 7089
		[Tooltip("the value to remap the curve's 0 to")]
		[Range(0f, 1f)]
		public float RemapDistanceZero;

		// Token: 0x04001BB2 RID: 7090
		[Tooltip("the value to remap the curve's 1 to")]
		[Range(0f, 1f)]
		public float RemapDistanceOne = 1f;

		// Token: 0x04001BB3 RID: 7091
		protected Volume _volume;

		// Token: 0x04001BB4 RID: 7092
		protected PaniniProjection _paniniProjection;

		// Token: 0x04001BB5 RID: 7093
		protected float _initialDistance;

		// Token: 0x04001BB6 RID: 7094
		protected float _originalShakeDuration;

		// Token: 0x04001BB7 RID: 7095
		protected AnimationCurve _originalShakeDistance;

		// Token: 0x04001BB8 RID: 7096
		protected float _originalRemapDistanceZero;

		// Token: 0x04001BB9 RID: 7097
		protected float _originalRemapDistanceOne;

		// Token: 0x04001BBA RID: 7098
		protected bool _originalRelativeDistance;
	}
}
