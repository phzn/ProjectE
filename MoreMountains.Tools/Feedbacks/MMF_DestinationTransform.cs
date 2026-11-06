using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001E1 RID: 481
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you animate the position/rotation/scale of a target transform to match the one of a destination transform.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Transform/Destination")]
	public class MMF_DestinationTransform : MMF_Feedback
	{
		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000C4B RID: 3147 RVA: 0x000356F1 File Offset: 0x000338F1
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x000356F4 File Offset: 0x000338F4
		protected override void AutomateTargetAcquisition()
		{
			this.TargetTransform = this.FindAutomatedTarget<Transform>();
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000C4D RID: 3149 RVA: 0x00035702 File Offset: 0x00033902
		// (set) Token: 0x06000C4E RID: 3150 RVA: 0x00035710 File Offset: 0x00033910
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.Duration);
			}
			set
			{
				this.Duration = value;
			}
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x0003571C File Offset: 0x0003391C
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_DestinationTransform.FeedbackTypeAuthorized || this.TargetTransform == null)
			{
				return;
			}
			if (this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
			}
			this._coroutine = this.Owner.StartCoroutine(this.AnimateToDestination());
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x00035777 File Offset: 0x00033977
		protected virtual IEnumerator AnimateToDestination()
		{
			this._initialPosition = this.TargetTransform.position;
			this._initialRotation = this.TargetTransform.rotation;
			this._initialScale = this.TargetTransform.localScale;
			this._pointAPosition = (this.ForceOrigin ? this.Origin.transform.position : this.TargetTransform.position);
			this._pointBPosition = this.Destination.transform.position;
			if (!this.AnimatePositionX)
			{
				this._pointAPosition.x = this.TargetTransform.position.x;
				this._pointBPosition.x = this._pointAPosition.x;
			}
			if (!this.AnimatePositionY)
			{
				this._pointAPosition.y = this.TargetTransform.position.y;
				this._pointBPosition.y = this._pointAPosition.y;
			}
			if (!this.AnimatePositionZ)
			{
				this._pointAPosition.z = this.TargetTransform.position.z;
				this._pointBPosition.z = this._pointAPosition.z;
			}
			this._pointARotation = (this.ForceOrigin ? this.Origin.transform.rotation : this.TargetTransform.rotation);
			this._pointBRotation = this.Destination.transform.rotation;
			if (!this.AnimateRotationX)
			{
				this._pointARotation.x = this.TargetTransform.rotation.x;
				this._pointBRotation.x = this._pointARotation.x;
			}
			if (!this.AnimateRotationY)
			{
				this._pointARotation.y = this.TargetTransform.rotation.y;
				this._pointBRotation.y = this._pointARotation.y;
			}
			if (!this.AnimateRotationZ)
			{
				this._pointARotation.z = this.TargetTransform.rotation.z;
				this._pointBRotation.z = this._pointARotation.z;
			}
			if (!this.AnimateRotationW)
			{
				this._pointARotation.w = this.TargetTransform.rotation.w;
				this._pointBRotation.w = this._pointARotation.w;
			}
			this._pointAScale = (this.ForceOrigin ? this.Origin.transform.localScale : this.TargetTransform.localScale);
			this._pointBScale = this.Destination.transform.localScale;
			if (!this.AnimateScaleX)
			{
				this._pointAScale.x = this.TargetTransform.localScale.x;
				this._pointBScale.x = this._pointAScale.x;
			}
			if (!this.AnimateScaleY)
			{
				this._pointAScale.y = this.TargetTransform.localScale.y;
				this._pointBScale.y = this._pointAScale.y;
			}
			if (!this.AnimateScaleZ)
			{
				this._pointAScale.z = this.TargetTransform.localScale.z;
				this._pointBScale.z = this._pointAScale.z;
			}
			this.IsPlaying = true;
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = Mathf.Clamp01(journey / this.FeedbackDuration);
				this.ChangeTransformValues(num);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.ChangeTransformValues(1f);
			this.IsPlaying = false;
			this._coroutine = null;
			yield break;
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x00035788 File Offset: 0x00033988
		protected virtual void ChangeTransformValues(float percent)
		{
			this._animationTweenType = (this.SeparatePositionCurve ? this.AnimatePositionTween : this.GlobalAnimationTween);
			this._newPosition = Vector3.LerpUnclamped(this._pointAPosition, this._pointBPosition, this._animationTweenType.Evaluate(percent));
			this._animationTweenType = (this.SeparateRotationCurve ? this.AnimateRotationTween : this.GlobalAnimationTween);
			this._newRotation = Quaternion.LerpUnclamped(this._pointARotation, this._pointBRotation, this._animationTweenType.Evaluate(percent));
			this._animationTweenType = (this.SeparateScaleCurve ? this.AnimateScaleTween : this.GlobalAnimationTween);
			this._newScale = Vector3.LerpUnclamped(this._pointAScale, this._pointBScale, this._animationTweenType.Evaluate(percent));
			this.TargetTransform.position = this._newPosition;
			this.TargetTransform.rotation = this._newRotation;
			this.TargetTransform.localScale = this._newScale;
		}

		// Token: 0x06000C52 RID: 3154 RVA: 0x00035888 File Offset: 0x00033A88
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_DestinationTransform.FeedbackTypeAuthorized)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			this.IsPlaying = false;
			if (this.TargetTransform != null && this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
			}
		}

		// Token: 0x06000C53 RID: 3155 RVA: 0x000358DC File Offset: 0x00033ADC
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_DestinationTransform.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TargetTransform.position = this._initialPosition;
			this.TargetTransform.rotation = this._initialRotation;
			this.TargetTransform.localScale = this._initialScale;
		}

		// Token: 0x06000C54 RID: 3156 RVA: 0x0003592C File Offset: 0x00033B2C
		public override void OnValidate()
		{
			base.OnValidate();
			MMFeedbacksHelpers.MigrateCurve(this.GlobalAnimationCurve, this.GlobalAnimationTween, this.Owner);
			MMFeedbacksHelpers.MigrateCurve(this.AnimatePositionCurve, this.AnimatePositionTween, this.Owner);
			MMFeedbacksHelpers.MigrateCurve(this.AnimateRotationCurve, this.AnimateRotationTween, this.Owner);
			MMFeedbacksHelpers.MigrateCurve(this.AnimateScaleCurve, this.AnimateScaleTween, this.Owner);
		}

		// Token: 0x04000DED RID: 3565
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000DEE RID: 3566
		[MMFInspectorGroup("Target to animate", true, 61, true, false)]
		[Tooltip("the target transform we want to animate properties on")]
		public Transform TargetTransform;

		// Token: 0x04000DEF RID: 3567
		[Tooltip("whether or not we want to force an origin transform. If not, the current position of the target transform will be used as origin instead")]
		public bool ForceOrigin;

		// Token: 0x04000DF0 RID: 3568
		[Tooltip("the transform to use as origin in ForceOrigin mode")]
		[MMFCondition("ForceOrigin", true)]
		public Transform Origin;

		// Token: 0x04000DF1 RID: 3569
		[Tooltip("the destination transform whose properties we want to match")]
		public Transform Destination;

		// Token: 0x04000DF2 RID: 3570
		[MMFInspectorGroup("Transition", true, 63, false, false)]
		[Tooltip("a global curve to animate all properties on, unless dedicated ones are specified")]
		public MMTweenType GlobalAnimationTween = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x04000DF3 RID: 3571
		[Tooltip("the duration of the transition, in seconds")]
		public float Duration = 0.2f;

		// Token: 0x04000DF4 RID: 3572
		[MMFInspectorGroup("Axis Locks", true, 64, false, false)]
		[Tooltip("whether or not to animate the X Position")]
		public bool AnimatePositionX = true;

		// Token: 0x04000DF5 RID: 3573
		[Tooltip("whether or not to animate the Y Position")]
		public bool AnimatePositionY = true;

		// Token: 0x04000DF6 RID: 3574
		[Tooltip("whether or not to animate the Z Position")]
		public bool AnimatePositionZ = true;

		// Token: 0x04000DF7 RID: 3575
		[Tooltip("whether or not to animate the X rotation")]
		public bool AnimateRotationX = true;

		// Token: 0x04000DF8 RID: 3576
		[Tooltip("whether or not to animate the Y rotation")]
		public bool AnimateRotationY = true;

		// Token: 0x04000DF9 RID: 3577
		[Tooltip("whether or not to animate the Z rotation")]
		public bool AnimateRotationZ = true;

		// Token: 0x04000DFA RID: 3578
		[Tooltip("whether or not to animate the W rotation")]
		public bool AnimateRotationW = true;

		// Token: 0x04000DFB RID: 3579
		[Tooltip("whether or not to animate the X scale")]
		public bool AnimateScaleX = true;

		// Token: 0x04000DFC RID: 3580
		[Tooltip("whether or not to animate the Y scale")]
		public bool AnimateScaleY = true;

		// Token: 0x04000DFD RID: 3581
		[Tooltip("whether or not to animate the Z scale")]
		public bool AnimateScaleZ = true;

		// Token: 0x04000DFE RID: 3582
		[MMFInspectorGroup("Separate Curves", true, 65, false, false)]
		[Tooltip("whether or not to use a separate animation curve to animate the position")]
		public bool SeparatePositionCurve;

		// Token: 0x04000DFF RID: 3583
		[Tooltip("the curve to use to animate the position on")]
		[MMFCondition("SeparatePositionCurve", true)]
		public MMTweenType AnimatePositionTween = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x04000E00 RID: 3584
		[Tooltip("whether or not to use a separate animation curve to animate the rotation")]
		public bool SeparateRotationCurve;

		// Token: 0x04000E01 RID: 3585
		[Tooltip("the curve to use to animate the rotation on")]
		[MMFCondition("SeparateRotationCurve", true)]
		public MMTweenType AnimateRotationTween = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x04000E02 RID: 3586
		[Tooltip("whether or not to use a separate animation curve to animate the scale")]
		public bool SeparateScaleCurve;

		// Token: 0x04000E03 RID: 3587
		[Tooltip("the curve to use to animate the scale on")]
		[MMFCondition("SeparateScaleCurve", true)]
		public MMTweenType AnimateScaleTween = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x04000E04 RID: 3588
		[HideInInspector]
		public AnimationCurve GlobalAnimationCurve;

		// Token: 0x04000E05 RID: 3589
		[HideInInspector]
		public AnimationCurve AnimateScaleCurve;

		// Token: 0x04000E06 RID: 3590
		[HideInInspector]
		public AnimationCurve AnimatePositionCurve;

		// Token: 0x04000E07 RID: 3591
		[HideInInspector]
		public AnimationCurve AnimateRotationCurve;

		// Token: 0x04000E08 RID: 3592
		protected Coroutine _coroutine;

		// Token: 0x04000E09 RID: 3593
		protected Vector3 _newPosition;

		// Token: 0x04000E0A RID: 3594
		protected Quaternion _newRotation;

		// Token: 0x04000E0B RID: 3595
		protected Vector3 _newScale;

		// Token: 0x04000E0C RID: 3596
		protected Vector3 _pointAPosition;

		// Token: 0x04000E0D RID: 3597
		protected Vector3 _pointBPosition;

		// Token: 0x04000E0E RID: 3598
		protected Quaternion _pointARotation;

		// Token: 0x04000E0F RID: 3599
		protected Quaternion _pointBRotation;

		// Token: 0x04000E10 RID: 3600
		protected Vector3 _pointAScale;

		// Token: 0x04000E11 RID: 3601
		protected Vector3 _pointBScale;

		// Token: 0x04000E12 RID: 3602
		protected MMTweenType _animationTweenType;

		// Token: 0x04000E13 RID: 3603
		protected Vector3 _initialPosition;

		// Token: 0x04000E14 RID: 3604
		protected Vector3 _initialScale;

		// Token: 0x04000E15 RID: 3605
		protected Quaternion _initialRotation;

		// Token: 0x020004AF RID: 1199
		public enum TimeScales
		{
			// Token: 0x040021DA RID: 8666
			Scaled,
			// Token: 0x040021DB RID: 8667
			Unscaled
		}
	}
}
