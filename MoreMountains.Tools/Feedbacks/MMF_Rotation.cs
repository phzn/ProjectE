using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000204 RID: 516
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will animate the target's rotation on the 3 specified animation curves (one per axis), for the specified duration (in seconds).")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Transform/Rotation")]
	public class MMF_Rotation : MMF_Feedback
	{
		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000DC5 RID: 3525 RVA: 0x0003CE95 File Offset: 0x0003B095
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000DC6 RID: 3526 RVA: 0x0003CE98 File Offset: 0x0003B098
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000DC7 RID: 3527 RVA: 0x0003CE9B File Offset: 0x0003B09B
		protected override void AutomateTargetAcquisition()
		{
			this.AnimateRotationTarget = this.FindAutomatedTarget<Transform>();
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000DC8 RID: 3528 RVA: 0x0003CEA9 File Offset: 0x0003B0A9
		// (set) Token: 0x06000DC9 RID: 3529 RVA: 0x0003CEB7 File Offset: 0x0003B0B7
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

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000DCA RID: 3530 RVA: 0x0003CEC0 File Offset: 0x0003B0C0
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x0003CEC3 File Offset: 0x0003B0C3
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.Active && this.AnimateRotationTarget != null)
			{
				this.GetInitialRotation();
			}
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0003CEE8 File Offset: 0x0003B0E8
		protected virtual void GetInitialRotation()
		{
			this._initialRotation = ((this.RotationSpace == Space.World) ? this.AnimateRotationTarget.rotation : this.AnimateRotationTarget.localRotation);
			this._initialToDestinationAngles = this._initialRotation.eulerAngles;
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x0003CF24 File Offset: 0x0003B124
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Rotation.FeedbackTypeAuthorized || this.AnimateRotationTarget == null)
			{
				return;
			}
			float num = this.ComputeIntensity(feedbacksIntensity, position);
			if (this.Active || this.Owner.AutoPlayOnEnable)
			{
				if (this.Mode == MMF_Rotation.Modes.Absolute || this.Mode == MMF_Rotation.Modes.Additive)
				{
					if (!this.AllowAdditivePlays && this._coroutine != null)
					{
						return;
					}
					if (this.DetermineRotationOnPlay && this.NormalPlayDirection)
					{
						this.GetInitialRotation();
					}
					this.ClearCoroutine();
					this._coroutine = this.Owner.StartCoroutine(this.AnimateRotation(this.AnimateRotationTarget, Vector3.zero, this.FeedbackDuration, this.AnimateRotationTweenX, this.AnimateRotationTweenY, this.AnimateRotationTweenZ, this.RemapCurveZero * num, this.RemapCurveOne * num));
					return;
				}
				else if (this.Mode == MMF_Rotation.Modes.ToDestination)
				{
					if (!this.AllowAdditivePlays && this._coroutine != null)
					{
						return;
					}
					if (this.DetermineRotationOnPlay && this.NormalPlayDirection)
					{
						this.GetInitialRotation();
					}
					this.ClearCoroutine();
					this._coroutine = this.Owner.StartCoroutine(this.RotateToDestination());
				}
			}
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x0003D046 File Offset: 0x0003B246
		protected virtual void ClearCoroutine()
		{
			if (this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
				this._coroutine = null;
			}
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x0003D068 File Offset: 0x0003B268
		protected virtual IEnumerator RotateToDestination()
		{
			if (this.AnimateRotationTarget == null)
			{
				yield break;
			}
			if (this.AnimateRotationTweenX == null || this.AnimateRotationTweenY == null || this.AnimateRotationTweenZ == null)
			{
				yield break;
			}
			if (this.FeedbackDuration == 0f)
			{
				yield break;
			}
			Vector3 destinationAngles = (this.NormalPlayDirection ? this.DestinationAngles : this._initialToDestinationAngles);
			float journey = (this.NormalPlayDirection ? 0f : this.FeedbackDuration);
			this._initialRotation = this.AnimateRotationTarget.transform.rotation;
			if (this.ToDestinationSpace == Space.Self)
			{
				this.AnimateRotationTarget.transform.localRotation = Quaternion.Euler(destinationAngles);
			}
			else
			{
				this.AnimateRotationTarget.transform.rotation = Quaternion.Euler(destinationAngles);
			}
			this._destinationRotation = this.AnimateRotationTarget.transform.rotation;
			this.AnimateRotationTarget.transform.rotation = this._initialRotation;
			this.IsPlaying = true;
			while (journey >= 0f && journey <= this.FeedbackDuration && this.FeedbackDuration > 0f)
			{
				float num = Mathf.Clamp01(journey / this.FeedbackDuration);
				num = this.ToDestinationTween.Evaluate(num);
				Quaternion quaternion = Quaternion.LerpUnclamped(this._initialRotation, this._destinationRotation, num);
				this.AnimateRotationTarget.transform.rotation = quaternion;
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			if (this.ToDestinationSpace == Space.Self)
			{
				this.AnimateRotationTarget.transform.localRotation = Quaternion.Euler(destinationAngles);
			}
			else
			{
				this.AnimateRotationTarget.transform.rotation = Quaternion.Euler(destinationAngles);
			}
			this.IsPlaying = false;
			this._coroutine = null;
			yield break;
		}

		// Token: 0x06000DD0 RID: 3536 RVA: 0x0003D078 File Offset: 0x0003B278
		protected virtual IEnumerator AnimateRotation(Transform targetTransform, Vector3 vector, float duration, MMTweenType curveX, MMTweenType curveY, MMTweenType curveZ, float remapZero, float remapOne)
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
			if (this.Mode == MMF_Rotation.Modes.Additive)
			{
				this._initialRotation = ((this.RotationSpace == Space.World) ? targetTransform.rotation : targetTransform.localRotation);
			}
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

		// Token: 0x06000DD1 RID: 3537 RVA: 0x0003D0C8 File Offset: 0x0003B2C8
		protected virtual void ApplyRotation(Transform targetTransform, float remapZero, float remapOne, MMTweenType curveX, MMTweenType curveY, MMTweenType curveZ, float percent)
		{
			if (this.RotationSpace == Space.World)
			{
				targetTransform.transform.rotation = this._initialRotation;
			}
			else
			{
				targetTransform.transform.localRotation = this._initialRotation;
			}
			if (this.AnimateX)
			{
				float num = MMTween.Tween(percent, 0f, 1f, remapZero, remapOne, curveX);
				targetTransform.Rotate(Vector3.right, num, this.RotationSpace);
			}
			if (this.AnimateY)
			{
				float num2 = MMTween.Tween(percent, 0f, 1f, remapZero, remapOne, curveY);
				targetTransform.Rotate(Vector3.up, num2, this.RotationSpace);
			}
			if (this.AnimateZ)
			{
				float num3 = MMTween.Tween(percent, 0f, 1f, remapZero, remapOne, curveZ);
				targetTransform.Rotate(Vector3.forward, num3, this.RotationSpace);
			}
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x0003D191 File Offset: 0x0003B391
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (this.Active && MMF_Rotation.FeedbackTypeAuthorized && this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
				this._coroutine = null;
				this.IsPlaying = false;
			}
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0003D1C9 File Offset: 0x0003B3C9
		public override void OnDisable()
		{
			this._coroutine = null;
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x0003D1D4 File Offset: 0x0003B3D4
		public override void OnValidate()
		{
			base.OnValidate();
			MMFeedbacksHelpers.MigrateCurve(this.AnimateRotationX, this.AnimateRotationTweenX, this.Owner);
			MMFeedbacksHelpers.MigrateCurve(this.AnimateRotationY, this.AnimateRotationTweenY, this.Owner);
			MMFeedbacksHelpers.MigrateCurve(this.AnimateRotationZ, this.AnimateRotationTweenZ, this.Owner);
			MMFeedbacksHelpers.MigrateCurve(this.ToDestinationCurve, this.ToDestinationTween, this.Owner);
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0003D243 File Offset: 0x0003B443
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Rotation.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.RotationSpace == Space.World)
			{
				this.AnimateRotationTarget.rotation = this._initialRotation;
				return;
			}
			this.AnimateRotationTarget.localRotation = this._initialRotation;
		}

		// Token: 0x04000FDA RID: 4058
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000FDB RID: 4059
		[MMFInspectorGroup("Rotation Target", true, 61, true, false)]
		[Tooltip("the object whose rotation you want to animate")]
		public Transform AnimateRotationTarget;

		// Token: 0x04000FDC RID: 4060
		[MMFInspectorGroup("Transition", true, 63, false, false)]
		[Tooltip("whether this feedback should animate in absolute values or additive")]
		public MMF_Rotation.Modes Mode;

		// Token: 0x04000FDD RID: 4061
		[Tooltip("whether this feedback should play on local or world rotation")]
		public Space RotationSpace;

		// Token: 0x04000FDE RID: 4062
		[Tooltip("the duration of the transition")]
		public float AnimateRotationDuration = 0.2f;

		// Token: 0x04000FDF RID: 4063
		[Tooltip("the value to remap the curve's 0 value to")]
		[MMFEnumCondition("Mode", new int[] { 0, 1 })]
		public float RemapCurveZero;

		// Token: 0x04000FE0 RID: 4064
		[Tooltip("the value to remap the curve's 1 value to")]
		[MMFEnumCondition("Mode", new int[] { 0, 1 })]
		public float RemapCurveOne = 360f;

		// Token: 0x04000FE1 RID: 4065
		[Tooltip("if this is true, should animate the X rotation")]
		[MMFEnumCondition("Mode", new int[] { 0, 1 })]
		public bool AnimateX = true;

		// Token: 0x04000FE2 RID: 4066
		[Tooltip("how the x part of the rotation should animate over time, in degrees")]
		[MMFCondition("AnimateX")]
		public MMTweenType AnimateRotationTweenX = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x04000FE3 RID: 4067
		[Tooltip("if this is true, should animate the Y rotation")]
		[MMFEnumCondition("Mode", new int[] { 0, 1 })]
		public bool AnimateY = true;

		// Token: 0x04000FE4 RID: 4068
		[Tooltip("how the y part of the rotation should animate over time, in degrees")]
		[MMFCondition("AnimateY")]
		public MMTweenType AnimateRotationTweenY = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x04000FE5 RID: 4069
		[Tooltip("if this is true, should animate the Z rotation")]
		[MMFEnumCondition("Mode", new int[] { 0, 1 })]
		public bool AnimateZ = true;

		// Token: 0x04000FE6 RID: 4070
		[Tooltip("how the z part of the rotation should animate over time, in degrees")]
		[MMFCondition("AnimateZ")]
		public MMTweenType AnimateRotationTweenZ = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x04000FE7 RID: 4071
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x04000FE8 RID: 4072
		[Tooltip("if this is true, initial and destination rotations will be recomputed on every play")]
		public bool DetermineRotationOnPlay;

		// Token: 0x04000FE9 RID: 4073
		[Header("To Destination")]
		[Tooltip("the space in which the ToDestination mode should operate")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public Space ToDestinationSpace;

		// Token: 0x04000FEA RID: 4074
		[Tooltip("the angles to match when in ToDestination mode")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public Vector3 DestinationAngles = new Vector3(0f, 180f, 0f);

		// Token: 0x04000FEB RID: 4075
		[Tooltip("how the x part of the rotation should animate over time, in degrees")]
		[MMFEnumCondition("Mode", new int[] { 2 })]
		public MMTweenType ToDestinationTween = new MMTweenType(MMTween.MMTweenCurve.EaseInQuintic);

		// Token: 0x04000FEC RID: 4076
		[HideInInspector]
		public AnimationCurve AnimateRotationX;

		// Token: 0x04000FED RID: 4077
		[HideInInspector]
		public AnimationCurve AnimateRotationY;

		// Token: 0x04000FEE RID: 4078
		[HideInInspector]
		public AnimationCurve AnimateRotationZ;

		// Token: 0x04000FEF RID: 4079
		[HideInInspector]
		public AnimationCurve ToDestinationCurve;

		// Token: 0x04000FF0 RID: 4080
		protected Quaternion _initialRotation;

		// Token: 0x04000FF1 RID: 4081
		protected Vector3 _initialToDestinationAngles;

		// Token: 0x04000FF2 RID: 4082
		protected Quaternion _destinationRotation;

		// Token: 0x04000FF3 RID: 4083
		protected Coroutine _coroutine;

		// Token: 0x020004E0 RID: 1248
		public enum Modes
		{
			// Token: 0x040022C1 RID: 8897
			Absolute,
			// Token: 0x040022C2 RID: 8898
			Additive,
			// Token: 0x040022C3 RID: 8899
			ToDestination
		}

		// Token: 0x020004E1 RID: 1249
		public enum TimeScales
		{
			// Token: 0x040022C5 RID: 8901
			Scaled,
			// Token: 0x040022C6 RID: 8902
			Unscaled
		}
	}
}
