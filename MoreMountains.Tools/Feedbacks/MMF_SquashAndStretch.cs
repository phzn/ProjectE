using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace MoreMountains.Feedbacks
{
	// Token: 0x0200020F RID: 527
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Transform/Squash and Stretch")]
	[FeedbackHelp("This feedback will let you modify the scale of an object on an axis while the other two axis (or only one) get automatically modified to conserve mass.")]
	public class MMF_SquashAndStretch : MMF_Feedback
	{
		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000E51 RID: 3665 RVA: 0x0003FB91 File Offset: 0x0003DD91
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x0003FB94 File Offset: 0x0003DD94
		protected override void AutomateTargetAcquisition()
		{
			this.SquashAndStretchTarget = this.FindAutomatedTarget<Transform>();
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000E53 RID: 3667 RVA: 0x0003FBA2 File Offset: 0x0003DDA2
		// (set) Token: 0x06000E54 RID: 3668 RVA: 0x0003FBB0 File Offset: 0x0003DDB0
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.AnimateScaleDuration);
			}
			set
			{
				this.AnimateScaleDuration = value;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000E55 RID: 3669 RVA: 0x0003FBB9 File Offset: 0x0003DDB9
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x0003FBBC File Offset: 0x0003DDBC
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.Active && this.SquashAndStretchTarget != null)
			{
				this.GetInitialScale();
			}
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x0003FBE1 File Offset: 0x0003DDE1
		protected virtual void GetInitialScale()
		{
			this._initialScale = this.SquashAndStretchTarget.localScale;
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x0003FBF4 File Offset: 0x0003DDF4
		protected virtual void GetAxisScale()
		{
			switch (this.Axis)
			{
			case MMF_SquashAndStretch.PossibleAxis.XtoYZ:
				this._initialAxisScale = this.SquashAndStretchTarget.localScale.x;
				return;
			case MMF_SquashAndStretch.PossibleAxis.XtoY:
				this._initialAxisScale = this.SquashAndStretchTarget.localScale.x;
				return;
			case MMF_SquashAndStretch.PossibleAxis.XtoZ:
				this._initialAxisScale = this.SquashAndStretchTarget.localScale.x;
				return;
			case MMF_SquashAndStretch.PossibleAxis.YtoXZ:
				this._initialAxisScale = this.SquashAndStretchTarget.localScale.y;
				return;
			case MMF_SquashAndStretch.PossibleAxis.YtoX:
				this._initialAxisScale = this.SquashAndStretchTarget.localScale.y;
				return;
			case MMF_SquashAndStretch.PossibleAxis.YtoZ:
				this._initialAxisScale = this.SquashAndStretchTarget.localScale.y;
				return;
			case MMF_SquashAndStretch.PossibleAxis.ZtoXZ:
				this._initialAxisScale = this.SquashAndStretchTarget.localScale.z;
				return;
			case MMF_SquashAndStretch.PossibleAxis.ZtoX:
				this._initialAxisScale = this.SquashAndStretchTarget.localScale.z;
				return;
			case MMF_SquashAndStretch.PossibleAxis.ZtoY:
				this._initialAxisScale = this.SquashAndStretchTarget.localScale.z;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000E59 RID: 3673 RVA: 0x0003FD04 File Offset: 0x0003DF04
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_SquashAndStretch.FeedbackTypeAuthorized || this.SquashAndStretchTarget == null)
			{
				return;
			}
			if (this.DetermineScaleOnPlay && this.NormalPlayDirection)
			{
				this.GetInitialScale();
			}
			this.GetAxisScale();
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			if (this.Active || this.Owner.AutoPlayOnEnable)
			{
				if (this.Mode == MMF_SquashAndStretch.Modes.Absolute || this.Mode == MMF_SquashAndStretch.Modes.Additive)
				{
					if (!this.AllowAdditivePlays && this._coroutine != null)
					{
						return;
					}
					if (this._coroutine != null)
					{
						this.Owner.StopCoroutine(this._coroutine);
					}
					this._coroutine = this.Owner.StartCoroutine(this.AnimateScale(this.SquashAndStretchTarget, this.FeedbackDuration, this.AnimateCurve, this.Axis, this.RemapCurveZero, this.RemapCurveOne * num));
				}
				if (this.Mode == MMF_SquashAndStretch.Modes.ToDestination)
				{
					if (!this.AllowAdditivePlays && this._coroutine != null)
					{
						return;
					}
					if (this._coroutine != null)
					{
						this.Owner.StopCoroutine(this._coroutine);
					}
					this._coroutine = this.Owner.StartCoroutine(this.ScaleToDestination());
				}
			}
		}

		// Token: 0x06000E5A RID: 3674 RVA: 0x0003FE2E File Offset: 0x0003E02E
		protected virtual IEnumerator ScaleToDestination()
		{
			if (this.SquashAndStretchTarget == null)
			{
				yield break;
			}
			if (this.FeedbackDuration == 0f)
			{
				yield break;
			}
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			this._initialScale = this.SquashAndStretchTarget.localScale;
			this._newScale = this._initialScale;
			this.GetAxisScale();
			this.IsPlaying = true;
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = Mathf.Clamp01(journey / this.FeedbackDuration);
				float num2 = Mathf.LerpUnclamped(this._initialAxisScale, this.DestinationScale, this.AnimateCurve.Evaluate(num) + this.Offset);
				num2 = MMFeedbacksHelpers.Remap(num2, 0f, 1f, this.RemapCurveZero, this.RemapCurveOne);
				this.ApplyScale(num2);
				this.SquashAndStretchTarget.localScale = this._newScale;
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.ApplyScale(this.DestinationScale);
			this.SquashAndStretchTarget.localScale = (this.NormalPlayDirection ? this._newScale : this._initialScale);
			this._coroutine = null;
			this.IsPlaying = false;
			yield return null;
			yield break;
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x0003FE3D File Offset: 0x0003E03D
		protected virtual IEnumerator AnimateScale(Transform targetTransform, float duration, AnimationCurve curve, MMF_SquashAndStretch.PossibleAxis axis, float remapCurveZero = 0f, float remapCurveOne = 1f)
		{
			if (targetTransform == null)
			{
				yield break;
			}
			if (duration == 0f)
			{
				yield break;
			}
			float journey = (this.NormalPlayDirection ? 0f : duration);
			this._initialScale = targetTransform.localScale;
			this.IsPlaying = true;
			while (journey >= 0f && journey <= duration && duration > 0f)
			{
				float num = Mathf.Clamp01(journey / duration);
				this.ComputeAndApplyScale(num, curve, remapCurveZero, remapCurveOne, targetTransform);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.ComputeAndApplyScale(1f, curve, remapCurveZero, remapCurveOne, targetTransform);
			this._coroutine = null;
			this.IsPlaying = false;
			yield return null;
			yield break;
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x0003FE74 File Offset: 0x0003E074
		protected virtual void ComputeAndApplyScale(float percent, AnimationCurve curve, float remapCurveZero, float remapCurveOne, Transform targetTransform)
		{
			float num = curve.Evaluate(percent) + this.Offset;
			num = MMFeedbacksHelpers.Remap(num, 0f, 1f, remapCurveZero, remapCurveOne);
			if (this.Mode == MMF_SquashAndStretch.Modes.Additive)
			{
				num += this._initialAxisScale;
			}
			num = Mathf.Abs(num);
			this.ApplyScale(num);
			targetTransform.localScale = this._newScale;
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x0003FED4 File Offset: 0x0003E0D4
		protected virtual void ApplyScale(float newScale)
		{
			float num = 1f / Mathf.Sqrt(newScale);
			switch (this.Axis)
			{
			case MMF_SquashAndStretch.PossibleAxis.XtoYZ:
				this._newScale.x = newScale;
				this._newScale.y = num;
				this._newScale.z = num;
				return;
			case MMF_SquashAndStretch.PossibleAxis.XtoY:
				this._newScale.x = newScale;
				this._newScale.y = num;
				this._newScale.z = this._initialScale.z;
				return;
			case MMF_SquashAndStretch.PossibleAxis.XtoZ:
				this._newScale.x = newScale;
				this._newScale.y = this._initialScale.y;
				this._newScale.z = num;
				return;
			case MMF_SquashAndStretch.PossibleAxis.YtoXZ:
				this._newScale.x = num;
				this._newScale.y = newScale;
				this._newScale.z = num;
				return;
			case MMF_SquashAndStretch.PossibleAxis.YtoX:
				this._newScale.x = num;
				this._newScale.y = newScale;
				this._newScale.z = this._initialScale.z;
				return;
			case MMF_SquashAndStretch.PossibleAxis.YtoZ:
				this._newScale.x = newScale;
				this._newScale.y = this._initialScale.y;
				this._newScale.z = num;
				return;
			case MMF_SquashAndStretch.PossibleAxis.ZtoXZ:
				this._newScale.x = num;
				this._newScale.y = num;
				this._newScale.z = newScale;
				return;
			case MMF_SquashAndStretch.PossibleAxis.ZtoX:
				this._newScale.x = num;
				this._newScale.y = this._initialScale.y;
				this._newScale.z = newScale;
				return;
			case MMF_SquashAndStretch.PossibleAxis.ZtoY:
				this._newScale.x = this._initialScale.x;
				this._newScale.y = num;
				this._newScale.z = newScale;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x000400A8 File Offset: 0x0003E2A8
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (this.Active && this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
				this._coroutine = null;
				this.IsPlaying = false;
			}
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x000400D9 File Offset: 0x0003E2D9
		public override void OnDisable()
		{
			this._coroutine = null;
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x000400E2 File Offset: 0x0003E2E2
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_SquashAndStretch.FeedbackTypeAuthorized)
			{
				return;
			}
			this.SquashAndStretchTarget.localScale = this._initialScale;
		}

		// Token: 0x0400109B RID: 4251
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400109C RID: 4252
		[MMFInspectorGroup("Squash & Stretch", true, 54, true, false)]
		[Tooltip("the object to animate")]
		public Transform SquashAndStretchTarget;

		// Token: 0x0400109D RID: 4253
		[Tooltip("the mode this feedback should operate onAbsolute : follows the curveAdditive : adds to the current scale of the targetToDestination : sets the scale to the destination target, whatever the current scale is")]
		public MMF_SquashAndStretch.Modes Mode;

		// Token: 0x0400109E RID: 4254
		public MMF_SquashAndStretch.PossibleAxis Axis = MMF_SquashAndStretch.PossibleAxis.YtoXZ;

		// Token: 0x0400109F RID: 4255
		[Tooltip("the duration of the animation")]
		public float AnimateScaleDuration = 0.2f;

		// Token: 0x040010A0 RID: 4256
		[Tooltip("the value to remap the curve's 0 value to")]
		public float RemapCurveZero = 1f;

		// Token: 0x040010A1 RID: 4257
		[Tooltip("the value to remap the curve's 1 value to")]
		[FormerlySerializedAs("Multiplier")]
		public float RemapCurveOne = 2f;

		// Token: 0x040010A2 RID: 4258
		[Tooltip("how much should be added to the curve")]
		public float Offset;

		// Token: 0x040010A3 RID: 4259
		[Tooltip("the curve along which to animate the scale")]
		public AnimationCurve AnimateCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1.5f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x040010A4 RID: 4260
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x040010A5 RID: 4261
		[Tooltip("if this is true, initial and destination scales will be recomputed on every play")]
		public bool DetermineScaleOnPlay;

		// Token: 0x040010A6 RID: 4262
		[Tooltip("the scale to reach when in ToDestination mode")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public float DestinationScale = 2f;

		// Token: 0x040010A7 RID: 4263
		protected Vector3 _initialScale;

		// Token: 0x040010A8 RID: 4264
		protected float _initialAxisScale;

		// Token: 0x040010A9 RID: 4265
		protected Vector3 _newScale;

		// Token: 0x040010AA RID: 4266
		protected Coroutine _coroutine;

		// Token: 0x020004F3 RID: 1267
		public enum Modes
		{
			// Token: 0x04002320 RID: 8992
			Absolute,
			// Token: 0x04002321 RID: 8993
			Additive,
			// Token: 0x04002322 RID: 8994
			ToDestination
		}

		// Token: 0x020004F4 RID: 1268
		public enum PossibleAxis
		{
			// Token: 0x04002324 RID: 8996
			XtoYZ,
			// Token: 0x04002325 RID: 8997
			XtoY,
			// Token: 0x04002326 RID: 8998
			XtoZ,
			// Token: 0x04002327 RID: 8999
			YtoXZ,
			// Token: 0x04002328 RID: 9000
			YtoX,
			// Token: 0x04002329 RID: 9001
			YtoZ,
			// Token: 0x0400232A RID: 9002
			ZtoXZ,
			// Token: 0x0400232B RID: 9003
			ZtoX,
			// Token: 0x0400232C RID: 9004
			ZtoY
		}

		// Token: 0x020004F5 RID: 1269
		public enum TimeScales
		{
			// Token: 0x0400232E RID: 9006
			Scaled,
			// Token: 0x0400232F RID: 9007
			Unscaled
		}
	}
}
