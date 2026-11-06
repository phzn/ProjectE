using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000207 RID: 519
	[AddComponentMenu("")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Transform/Scale")]
	[FeedbackHelp("This feedback will animate the target's scale on the 3 specified animation curves, for the specified duration (in seconds). You can apply a multiplier, that will multiply each animation curve value.")]
	public class MMF_Scale : MMF_Feedback
	{
		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000DF6 RID: 3574 RVA: 0x0003DF5C File Offset: 0x0003C15C
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000DF7 RID: 3575 RVA: 0x0003DF5F File Offset: 0x0003C15F
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0003DF62 File Offset: 0x0003C162
		protected override void AutomateTargetAcquisition()
		{
			this.AnimateScaleTarget = this.FindAutomatedTarget<Transform>();
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000DF9 RID: 3577 RVA: 0x0003DF70 File Offset: 0x0003C170
		// (set) Token: 0x06000DFA RID: 3578 RVA: 0x0003DF7E File Offset: 0x0003C17E
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

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000DFB RID: 3579 RVA: 0x0003DF87 File Offset: 0x0003C187
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0003DF8A File Offset: 0x0003C18A
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.Active && this.AnimateScaleTarget != null)
			{
				this.GetInitialScale();
			}
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0003DFAF File Offset: 0x0003C1AF
		protected virtual void GetInitialScale()
		{
			this._initialScale = this.AnimateScaleTarget.localScale;
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x0003DFC4 File Offset: 0x0003C1C4
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Scale.FeedbackTypeAuthorized || this.AnimateScaleTarget == null)
			{
				return;
			}
			if (this.DetermineScaleOnPlay && this.NormalPlayDirection)
			{
				this.GetInitialScale();
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			if (this.Active || this.Owner.AutoPlayOnEnable)
			{
				if (this.Mode == MMF_Scale.Modes.Absolute || this.Mode == MMF_Scale.Modes.Additive)
				{
					if (!this.AllowAdditivePlays && this._coroutine != null)
					{
						return;
					}
					if (this._coroutine != null)
					{
						this.Owner.StopCoroutine(this._coroutine);
					}
					this._coroutine = this.Owner.StartCoroutine(this.AnimateScale(this.AnimateScaleTarget, Vector3.zero, this.FeedbackDuration, this.AnimateScaleTweenX, this.AnimateScaleTweenY, this.AnimateScaleTweenZ, this.RemapCurveZero * num, this.RemapCurveOne * num));
				}
				if (this.Mode == MMF_Scale.Modes.ToDestination)
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

		// Token: 0x06000DFF RID: 3583 RVA: 0x0003E0F5 File Offset: 0x0003C2F5
		protected virtual IEnumerator ScaleToDestination()
		{
			if (this.AnimateScaleTarget == null)
			{
				yield break;
			}
			if (this.AnimateScaleTweenX == null || this.AnimateScaleTweenY == null || this.AnimateScaleTweenZ == null)
			{
				yield break;
			}
			if (this.FeedbackDuration == 0f)
			{
				yield break;
			}
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			this._initialScale = this.AnimateScaleTarget.localScale;
			this._newScale = this._initialScale;
			this.IsPlaying = true;
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = Mathf.Clamp01(journey / this.FeedbackDuration);
				if (this.AnimateX)
				{
					this._newScale.x = Mathf.LerpUnclamped(this._initialScale.x, this.DestinationScale.x, this.AnimateScaleTweenX.Evaluate(num) + this.Offset);
					this._newScale.x = MMFeedbacksHelpers.Remap(this._newScale.x, 0f, 1f, this.RemapCurveZero, this.RemapCurveOne);
				}
				if (this.AnimateY)
				{
					this._newScale.y = Mathf.LerpUnclamped(this._initialScale.y, this.DestinationScale.y, this.AnimateScaleTweenY.Evaluate(num) + this.Offset);
					this._newScale.y = MMFeedbacksHelpers.Remap(this._newScale.y, 0f, 1f, this.RemapCurveZero, this.RemapCurveOne);
				}
				if (this.AnimateZ)
				{
					this._newScale.z = Mathf.LerpUnclamped(this._initialScale.z, this.DestinationScale.z, this.AnimateScaleTweenZ.Evaluate(num) + this.Offset);
					this._newScale.z = MMFeedbacksHelpers.Remap(this._newScale.z, 0f, 1f, this.RemapCurveZero, this.RemapCurveOne);
				}
				if (this.UniformScaling)
				{
					this._newScale.y = this._newScale.x;
					this._newScale.z = this._newScale.x;
				}
				this.AnimateScaleTarget.localScale = this._newScale;
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.AnimateScaleTarget.localScale = (this.NormalPlayDirection ? this.DestinationScale : this._initialScale);
			this._coroutine = null;
			this.IsPlaying = false;
			yield return null;
			yield break;
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x0003E104 File Offset: 0x0003C304
		protected virtual IEnumerator AnimateScale(Transform targetTransform, Vector3 vector, float duration, MMTweenType curveX, MMTweenType curveY, MMTweenType curveZ, float remapCurveZero = 0f, float remapCurveOne = 1f)
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
			this._initialScale = targetTransform.localScale;
			this.IsPlaying = true;
			while (journey >= 0f && journey <= duration && duration > 0f)
			{
				vector = Vector3.zero;
				float num = Mathf.Clamp01(journey / duration);
				if (this.AnimateX)
				{
					vector.x = (this.AnimateX ? (curveX.Evaluate(num) + this.Offset) : targetTransform.localScale.x);
					vector.x = MMFeedbacksHelpers.Remap(vector.x, 0f, 1f, remapCurveZero, remapCurveOne);
					if (this.Mode == MMF_Scale.Modes.Additive)
					{
						vector.x += this._initialScale.x;
					}
				}
				else
				{
					vector.x = targetTransform.localScale.x;
				}
				if (this.AnimateY)
				{
					vector.y = (this.AnimateY ? (curveY.Evaluate(num) + this.Offset) : targetTransform.localScale.y);
					vector.y = MMFeedbacksHelpers.Remap(vector.y, 0f, 1f, remapCurveZero, remapCurveOne);
					if (this.Mode == MMF_Scale.Modes.Additive)
					{
						vector.y += this._initialScale.y;
					}
				}
				else
				{
					vector.y = targetTransform.localScale.y;
				}
				if (this.AnimateZ)
				{
					vector.z = (this.AnimateZ ? (curveZ.Evaluate(num) + this.Offset) : targetTransform.localScale.z);
					vector.z = MMFeedbacksHelpers.Remap(vector.z, 0f, 1f, remapCurveZero, remapCurveOne);
					if (this.Mode == MMF_Scale.Modes.Additive)
					{
						vector.z += this._initialScale.z;
					}
				}
				else
				{
					vector.z = targetTransform.localScale.z;
				}
				if (this.UniformScaling)
				{
					vector.y = vector.x;
					vector.z = vector.x;
				}
				targetTransform.localScale = vector;
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			vector = Vector3.zero;
			if (this.AnimateX)
			{
				vector.x = (this.AnimateX ? (curveX.Evaluate(this.FinalNormalizedTime) + this.Offset) : targetTransform.localScale.x);
				vector.x = MMFeedbacksHelpers.Remap(vector.x, 0f, 1f, remapCurveZero, remapCurveOne);
				if (this.Mode == MMF_Scale.Modes.Additive)
				{
					vector.x += this._initialScale.x;
				}
			}
			else
			{
				vector.x = targetTransform.localScale.x;
			}
			if (this.AnimateY)
			{
				vector.y = (this.AnimateY ? (curveY.Evaluate(this.FinalNormalizedTime) + this.Offset) : targetTransform.localScale.y);
				vector.y = MMFeedbacksHelpers.Remap(vector.y, 0f, 1f, remapCurveZero, remapCurveOne);
				if (this.Mode == MMF_Scale.Modes.Additive)
				{
					vector.y += this._initialScale.y;
				}
			}
			else
			{
				vector.y = targetTransform.localScale.y;
			}
			if (this.AnimateZ)
			{
				vector.z = (this.AnimateZ ? (curveZ.Evaluate(this.FinalNormalizedTime) + this.Offset) : targetTransform.localScale.z);
				vector.z = MMFeedbacksHelpers.Remap(vector.z, 0f, 1f, remapCurveZero, remapCurveOne);
				if (this.Mode == MMF_Scale.Modes.Additive)
				{
					vector.z += this._initialScale.z;
				}
			}
			else
			{
				vector.z = targetTransform.localScale.z;
			}
			if (this.UniformScaling)
			{
				vector.y = vector.x;
				vector.z = vector.x;
			}
			targetTransform.localScale = vector;
			this.IsPlaying = false;
			this._coroutine = null;
			yield return null;
			yield break;
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0003E15B File Offset: 0x0003C35B
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Scale.FeedbackTypeAuthorized || this._coroutine == null)
			{
				return;
			}
			this.IsPlaying = false;
			this.Owner.StopCoroutine(this._coroutine);
			this._coroutine = null;
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x0003E194 File Offset: 0x0003C394
		public override void OnDisable()
		{
			this._coroutine = null;
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x0003E1A0 File Offset: 0x0003C3A0
		public override void OnValidate()
		{
			base.OnValidate();
			MMFeedbacksHelpers.MigrateCurve(this.AnimateScaleX, this.AnimateScaleTweenX, this.Owner);
			MMFeedbacksHelpers.MigrateCurve(this.AnimateScaleY, this.AnimateScaleTweenY, this.Owner);
			MMFeedbacksHelpers.MigrateCurve(this.AnimateScaleZ, this.AnimateScaleTweenZ, this.Owner);
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x0003E1F8 File Offset: 0x0003C3F8
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Scale.FeedbackTypeAuthorized)
			{
				return;
			}
			this.AnimateScaleTarget.localScale = this._initialScale;
		}

		// Token: 0x0400101B RID: 4123
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x0400101C RID: 4124
		[MMFInspectorGroup("Scale Mode", true, 12, true, false)]
		[Tooltip("the mode this feedback should operate onAbsolute : follows the curveAdditive : adds to the current scale of the targetToDestination : sets the scale to the destination target, whatever the current scale is")]
		public MMF_Scale.Modes Mode;

		// Token: 0x0400101D RID: 4125
		[Tooltip("the object to animate")]
		public Transform AnimateScaleTarget;

		// Token: 0x0400101E RID: 4126
		[MMFInspectorGroup("Scale Animation", true, 13, false, false)]
		[Tooltip("the duration of the animation")]
		public float AnimateScaleDuration = 0.2f;

		// Token: 0x0400101F RID: 4127
		[Tooltip("the value to remap the curve's 0 value to")]
		public float RemapCurveZero = 1f;

		// Token: 0x04001020 RID: 4128
		[Tooltip("the value to remap the curve's 1 value to")]
		[FormerlySerializedAs("Multiplier")]
		public float RemapCurveOne = 2f;

		// Token: 0x04001021 RID: 4129
		[Tooltip("how much should be added to the curve")]
		public float Offset;

		// Token: 0x04001022 RID: 4130
		[Tooltip("if this is true, should animate the X scale value")]
		public bool AnimateX = true;

		// Token: 0x04001023 RID: 4131
		[Tooltip("the x scale animation definition")]
		[MMFCondition("AnimateX", true)]
		public MMTweenType AnimateScaleTweenX = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1.5f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x04001024 RID: 4132
		[Tooltip("if this is true, should animate the Y scale value")]
		public bool AnimateY = true;

		// Token: 0x04001025 RID: 4133
		[Tooltip("the y scale animation definition")]
		[MMFCondition("AnimateY", true)]
		public MMTweenType AnimateScaleTweenY = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1.5f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x04001026 RID: 4134
		[Tooltip("if this is true, should animate the z scale value")]
		public bool AnimateZ = true;

		// Token: 0x04001027 RID: 4135
		[Tooltip("the z scale animation definition")]
		[MMFCondition("AnimateZ", true)]
		public MMTweenType AnimateScaleTweenZ = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1.5f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x04001028 RID: 4136
		[Tooltip("if this is true, the AnimateX curve only will be used, and applied to all axis")]
		public bool UniformScaling;

		// Token: 0x04001029 RID: 4137
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x0400102A RID: 4138
		[Tooltip("if this is true, initial and destination scales will be recomputed on every play")]
		public bool DetermineScaleOnPlay;

		// Token: 0x0400102B RID: 4139
		[Tooltip("the scale to reach when in ToDestination mode")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public Vector3 DestinationScale = new Vector3(0.5f, 0.5f, 0.5f);

		// Token: 0x0400102C RID: 4140
		[HideInInspector]
		public AnimationCurve AnimateScaleX;

		// Token: 0x0400102D RID: 4141
		[HideInInspector]
		public AnimationCurve AnimateScaleY;

		// Token: 0x0400102E RID: 4142
		[HideInInspector]
		public AnimationCurve AnimateScaleZ;

		// Token: 0x0400102F RID: 4143
		protected Vector3 _initialScale;

		// Token: 0x04001030 RID: 4144
		protected Vector3 _newScale;

		// Token: 0x04001031 RID: 4145
		protected Coroutine _coroutine;

		// Token: 0x020004E6 RID: 1254
		public enum Modes
		{
			// Token: 0x040022DF RID: 8927
			Absolute,
			// Token: 0x040022E0 RID: 8928
			Additive,
			// Token: 0x040022E1 RID: 8929
			ToDestination
		}

		// Token: 0x020004E7 RID: 1255
		public enum TimeScales
		{
			// Token: 0x040022E3 RID: 8931
			Scaled,
			// Token: 0x040022E4 RID: 8932
			Unscaled
		}
	}
}
