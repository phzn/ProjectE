using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000203 RID: 515
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will animate the target's position (not its rotation), on an arc around the specified rotation center, for the specified duration (in seconds).")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Transform/Rotate Position Around")]
	public class MMF_RotatePositionAround : MMF_Feedback
	{
		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x0003CA25 File Offset: 0x0003AC25
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x0003CA28 File Offset: 0x0003AC28
		protected override void AutomateTargetAcquisition()
		{
			this.AnimateRotationTarget = this.FindAutomatedTarget<Transform>();
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x0003CA36 File Offset: 0x0003AC36
		// (set) Token: 0x06000DB8 RID: 3512 RVA: 0x0003CA44 File Offset: 0x0003AC44
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.AnimateRotationDuration);
			}
			set
			{
				this.AnimateRotationDuration = value;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x0003CA4D File Offset: 0x0003AC4D
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x0003CA50 File Offset: 0x0003AC50
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.Active && this.AnimateRotationTarget != null)
			{
				this.GetInitialPosition();
			}
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x0003CA75 File Offset: 0x0003AC75
		protected virtual void GetInitialPosition()
		{
			this._initialPosition = this.AnimateRotationTarget.transform.position;
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x0003CA90 File Offset: 0x0003AC90
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_RotatePositionAround.FeedbackTypeAuthorized || this.AnimateRotationTarget == null)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			if (this.Active || this.Owner.AutoPlayOnEnable)
			{
				if (!this.AllowAdditivePlays && this._coroutine != null)
				{
					return;
				}
				if (this.DetermineRotationOnPlay && this.NormalPlayDirection)
				{
					this.GetInitialPosition();
				}
				this.ClearCoroutine();
				this._coroutine = this.Owner.StartCoroutine(this.AnimateRotation(this.AnimateRotationTarget, Vector3.zero, this.FeedbackDuration, this.AnimateRotationX, this.AnimateRotationY, this.AnimateRotationZ, this.RemapCurveZero * num, this.RemapCurveOne * num));
			}
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x0003CB50 File Offset: 0x0003AD50
		protected virtual void ClearCoroutine()
		{
			if (this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
				this._coroutine = null;
			}
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x0003CB74 File Offset: 0x0003AD74
		protected virtual IEnumerator AnimateRotation(Transform targetTransform, Vector3 vector, float duration, AnimationCurve curveX, AnimationCurve curveY, AnimationCurve curveZ, float remapZero, float remapOne)
		{
			if (targetTransform == null)
			{
				yield break;
			}
			if (curveX == null || curveY == null || curveZ == null)
			{
				yield break;
			}
			if (duration == 0f)
			{
				yield break;
			}
			float journey = (this.NormalPlayDirection ? 0f : duration);
			this.IsPlaying = true;
			while (journey >= 0f && journey <= duration && duration > 0f)
			{
				float num = Mathf.Clamp01(journey / duration);
				this.ApplyRotation(targetTransform, remapZero, remapOne, curveX, curveY, curveZ, num);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.ApplyRotation(targetTransform, remapZero, remapOne, curveX, curveY, curveZ, this.FinalNormalizedTime);
			this._coroutine = null;
			this.IsPlaying = false;
			yield break;
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x0003CBC4 File Offset: 0x0003ADC4
		protected virtual void ApplyRotation(Transform targetTransform, float remapZero, float remapOne, AnimationCurve curveX, AnimationCurve curveY, AnimationCurve curveZ, float percent)
		{
			targetTransform.position = this._initialPosition;
			this._rotationAngles.x = 0f;
			this._rotationAngles.y = 0f;
			this._rotationAngles.z = 0f;
			if (this.AnimateX)
			{
				this._rotationAngles.x = curveX.Evaluate(percent);
				this._rotationAngles.x = MMFeedbacksHelpers.Remap(this._rotationAngles.x, 0f, 1f, remapZero, remapOne);
			}
			if (this.AnimateY)
			{
				this._rotationAngles.y = curveY.Evaluate(percent);
				this._rotationAngles.y = MMFeedbacksHelpers.Remap(this._rotationAngles.y, 0f, 1f, remapZero, remapOne);
			}
			if (this.AnimateZ)
			{
				this._rotationAngles.z = curveZ.Evaluate(percent);
				this._rotationAngles.z = MMFeedbacksHelpers.Remap(this._rotationAngles.z, 0f, 1f, remapZero, remapOne);
			}
			targetTransform.position = MMMaths.RotatePointAroundPivot(targetTransform.position, this.AnimateRotationCenter.position, this._rotationAngles);
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x0003CCF8 File Offset: 0x0003AEF8
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (this.Active && MMF_RotatePositionAround.FeedbackTypeAuthorized && this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
				this._coroutine = null;
				this.IsPlaying = false;
			}
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0003CD30 File Offset: 0x0003AF30
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_RotatePositionAround.FeedbackTypeAuthorized)
			{
				return;
			}
			this.AnimateRotationTarget.transform.position = this._initialPosition;
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x0003CD58 File Offset: 0x0003AF58
		public override void OnDisable()
		{
			this._coroutine = null;
		}

		// Token: 0x04000FC9 RID: 4041
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000FCA RID: 4042
		[MMFInspectorGroup("Animation Targets", true, 61, true, false)]
		[Tooltip("the object whose rotation you want to animate")]
		public Transform AnimateRotationTarget;

		// Token: 0x04000FCB RID: 4043
		[Tooltip("the object around which to rotate AnimateRotationTarget")]
		public Transform AnimateRotationCenter;

		// Token: 0x04000FCC RID: 4044
		[MMFInspectorGroup("Transition", true, 63, false, false)]
		[Tooltip("the duration of the transition")]
		public float AnimateRotationDuration = 0.2f;

		// Token: 0x04000FCD RID: 4045
		[Tooltip("the value to remap the curve's 0 value to")]
		public float RemapCurveZero;

		// Token: 0x04000FCE RID: 4046
		[Tooltip("the value to remap the curve's 1 value to")]
		public float RemapCurveOne = 180f;

		// Token: 0x04000FCF RID: 4047
		[Tooltip("if this is true, should animate movement on the X axis")]
		public bool AnimateX;

		// Token: 0x04000FD0 RID: 4048
		[Tooltip("how the x part of the movement should animate over time, in degrees")]
		[MMCondition("AnimateX", true)]
		public AnimationCurve AnimateRotationX = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000FD1 RID: 4049
		[Tooltip("if this is true, should animate movement on the Y axis")]
		public bool AnimateY = true;

		// Token: 0x04000FD2 RID: 4050
		[Tooltip("how the y part of the rotation should animate over time, in degrees")]
		[MMCondition("AnimateY", true)]
		public AnimationCurve AnimateRotationY = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000FD3 RID: 4051
		[Tooltip("if this is true, should animate movement on the Z axis")]
		public bool AnimateZ;

		// Token: 0x04000FD4 RID: 4052
		[Tooltip("how the z part of the rotation should animate over time, in degrees")]
		[MMCondition("AnimateZ", true)]
		public AnimationCurve AnimateRotationZ = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		});

		// Token: 0x04000FD5 RID: 4053
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x04000FD6 RID: 4054
		[Tooltip("if this is true, initial and destination rotations will be recomputed on every play")]
		public bool DetermineRotationOnPlay;

		// Token: 0x04000FD7 RID: 4055
		protected Vector3 _initialPosition;

		// Token: 0x04000FD8 RID: 4056
		protected Vector3 _rotationAngles;

		// Token: 0x04000FD9 RID: 4057
		protected Coroutine _coroutine;

		// Token: 0x020004DE RID: 1246
		public enum TimeScales
		{
			// Token: 0x040022B3 RID: 8883
			Scaled,
			// Token: 0x040022B4 RID: 8884
			Unscaled
		}
	}
}
