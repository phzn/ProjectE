using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001F3 RID: 499
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will let you animate the rotation of a transform to look at a target over time. You can also use it to broadcast a MMLookAtShake event, that MMLookAtShakers on the right channel will be able to listen for and act upon.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Transform/LookAt")]
	public class MMF_LookAt : MMF_Feedback
	{
		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000CFD RID: 3325 RVA: 0x0003878B File Offset: 0x0003698B
		// (set) Token: 0x06000CFE RID: 3326 RVA: 0x00038799 File Offset: 0x00036999
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

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000CFF RID: 3327 RVA: 0x000387A2 File Offset: 0x000369A2
		public override bool HasChannel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000D00 RID: 3328 RVA: 0x000387A5 File Offset: 0x000369A5
		public override bool HasRange
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x000387A8 File Offset: 0x000369A8
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			switch (this.UpwardVector)
			{
			case MMF_LookAt.UpwardVectors.Forward:
				this._upwards = Vector3.forward;
				return;
			case MMF_LookAt.UpwardVectors.Up:
				this._upwards = Vector3.up;
				return;
			case MMF_LookAt.UpwardVectors.Right:
				this._upwards = Vector3.right;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x000387F9 File Offset: 0x000369F9
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_LookAt.FeedbackTypeAuthorized)
			{
				return;
			}
			if (this.Active || this.Owner.AutoPlayOnEnable)
			{
				this.InitiateLookAt(position);
			}
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x00038828 File Offset: 0x00036A28
		protected virtual void InitiateLookAt(Vector3 position)
		{
			this._initialRotation = this.TransformToRotate.transform.rotation;
			MMF_LookAt.Modes mode = this.Mode;
			if (mode == MMF_LookAt.Modes.Direct)
			{
				this.ClearCoroutine();
				this._coroutine = this.Owner.StartCoroutine(this.AnimateLookAt());
				return;
			}
			if (mode != MMF_LookAt.Modes.Event)
			{
				return;
			}
			MMLookAtShaker.MMLookAtShakeEvent.Trigger(this.Duration, this.LockXAxis, this.LockYAxis, this.LockZAxis, this.UpwardVector, this.LookAtTargetMode, this.LookAtTarget, this.LookAtTargetWorldPosition, this.LookAtDirection, null, this.LookAtTween, this.UseRange, this.RangeDistance, this.UseRangeFalloff, this.RangeFalloff, this.RemapRangeFalloff, position, 1f, this.ChannelData, this.ResetShakerValuesAfterShake, this.ResetTargetValuesAfterShake, this.NormalPlayDirection, this.ComputedTimescaleMode, false);
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x000388FD File Offset: 0x00036AFD
		protected virtual IEnumerator AnimateLookAt()
		{
			if (this.TransformToRotate != null)
			{
				this._initialDirectTargetTransformRotation = this.TransformToRotate.transform.rotation;
			}
			float duration = this.FeedbackDuration;
			float journey = (this.NormalPlayDirection ? 0f : duration);
			this.IsPlaying = true;
			while (journey >= 0f && journey <= duration && duration > 0f)
			{
				float num = Mathf.Clamp01(journey / duration);
				num = this.LookAtTween.Evaluate(num);
				this.ApplyRotation(num);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.ApplyRotation(this.LookAtTween.Evaluate(1f));
			this._coroutine = null;
			this.IsPlaying = false;
			yield break;
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x0003890C File Offset: 0x00036B0C
		protected virtual void ApplyRotation(float percent)
		{
			switch (this.LookAtTargetMode)
			{
			case MMF_LookAt.LookAtTargetModes.Transform:
				this._lookAtPosition = this.LookAtTarget.position;
				break;
			case MMF_LookAt.LookAtTargetModes.TargetWorldPosition:
				this._lookAtPosition = this.LookAtTargetWorldPosition;
				break;
			case MMF_LookAt.LookAtTargetModes.Direction:
				this._lookAtPosition = this.TransformToRotate.position + this.LookAtDirection;
				break;
			}
			this._direction = this._lookAtPosition - this.TransformToRotate.position;
			this._newRotation = Quaternion.LookRotation(this._direction, this._upwards);
			if (this.LockXAxis)
			{
				this._newRotation.x = this.TransformToRotate.rotation.x;
			}
			if (this.LockYAxis)
			{
				this._newRotation.y = this.TransformToRotate.rotation.y;
			}
			if (this.LockZAxis)
			{
				this._newRotation.z = this.TransformToRotate.rotation.z;
			}
			this.TransformToRotate.transform.rotation = Quaternion.SlerpUnclamped(this._initialDirectTargetTransformRotation, this._newRotation, percent);
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x00038A2F File Offset: 0x00036C2F
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_LookAt.FeedbackTypeAuthorized || this._coroutine == null)
			{
				return;
			}
			base.CustomStopFeedback(position, feedbacksIntensity);
			this.IsPlaying = false;
			this.ClearCoroutine();
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x00038A5E File Offset: 0x00036C5E
		protected virtual void ClearCoroutine()
		{
			if (this._coroutine != null)
			{
				this.Owner.StopCoroutine(this._coroutine);
				this._coroutine = null;
			}
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x00038A80 File Offset: 0x00036C80
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_LookAt.FeedbackTypeAuthorized)
			{
				return;
			}
			this.TransformToRotate.transform.rotation = this._initialRotation;
		}

		// Token: 0x04000EDD RID: 3805
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000EDE RID: 3806
		[MMFInspectorGroup("Look at settings", true, 37, true, false)]
		[Tooltip("the duration of this feedback, in seconds")]
		public float Duration = 1f;

		// Token: 0x04000EDF RID: 3807
		[Tooltip("the curve over which to animate the look at transition")]
		public MMTweenType LookAtTween = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		}));

		// Token: 0x04000EE0 RID: 3808
		[Tooltip("whether or not to lock rotation on the x axis")]
		public bool LockXAxis;

		// Token: 0x04000EE1 RID: 3809
		[Tooltip("whether or not to lock rotation on the y axis")]
		public bool LockYAxis;

		// Token: 0x04000EE2 RID: 3810
		[Tooltip("whether or not to lock rotation on the z axis")]
		public bool LockZAxis;

		// Token: 0x04000EE3 RID: 3811
		[MMFInspectorGroup("What we want to rotate", true, 37, true, false)]
		[Tooltip("whether to make a certain transform look at a target, or to broadcast an event")]
		public MMF_LookAt.Modes Mode;

		// Token: 0x04000EE4 RID: 3812
		[Tooltip("in Direct mode, the transform to rotate to have it look at our target")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public Transform TransformToRotate;

		// Token: 0x04000EE5 RID: 3813
		[Tooltip("the vector representing the up direction on the object we want to rotate and look at our target")]
		[MMFEnumCondition("Mode", new int[] { 0 })]
		public MMF_LookAt.UpwardVectors UpwardVector = MMF_LookAt.UpwardVectors.Up;

		// Token: 0x04000EE6 RID: 3814
		[Tooltip("whether or not to reset shaker values after shake")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public bool ResetShakerValuesAfterShake = true;

		// Token: 0x04000EE7 RID: 3815
		[Tooltip("whether or not to reset the target's values after shake")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public bool ResetTargetValuesAfterShake = true;

		// Token: 0x04000EE8 RID: 3816
		[MMFInspectorGroup("What we want to look at", true, 37, true, false)]
		[Tooltip("the different target modes : either a specific transform to look at, the coordinates of a world position, or a direction vector")]
		public MMF_LookAt.LookAtTargetModes LookAtTargetMode;

		// Token: 0x04000EE9 RID: 3817
		[Tooltip("the transform we want to look at")]
		[MMFEnumCondition("LookAtTargetMode", new int[] { 0 })]
		public Transform LookAtTarget;

		// Token: 0x04000EEA RID: 3818
		[Tooltip("the coordinates of a point the world that we want to look at")]
		[MMFEnumCondition("LookAtTargetMode", new int[] { 1 })]
		public Vector3 LookAtTargetWorldPosition = Vector3.forward;

		// Token: 0x04000EEB RID: 3819
		[Tooltip("a direction (from our rotating object) that we want to look at")]
		[MMFEnumCondition("LookAtTargetMode", new int[] { 2 })]
		public Vector3 LookAtDirection = Vector3.forward;

		// Token: 0x04000EEC RID: 3820
		protected Coroutine _coroutine;

		// Token: 0x04000EED RID: 3821
		protected Quaternion _initialDirectTargetTransformRotation;

		// Token: 0x04000EEE RID: 3822
		protected Quaternion _newRotation;

		// Token: 0x04000EEF RID: 3823
		protected Vector3 _lookAtPosition;

		// Token: 0x04000EF0 RID: 3824
		protected Vector3 _upwards;

		// Token: 0x04000EF1 RID: 3825
		protected Vector3 _direction;

		// Token: 0x04000EF2 RID: 3826
		protected Quaternion _initialRotation;

		// Token: 0x020004C4 RID: 1220
		public enum Modes
		{
			// Token: 0x0400222F RID: 8751
			Direct,
			// Token: 0x04002230 RID: 8752
			Event
		}

		// Token: 0x020004C5 RID: 1221
		public enum LookAtTargetModes
		{
			// Token: 0x04002232 RID: 8754
			Transform,
			// Token: 0x04002233 RID: 8755
			TargetWorldPosition,
			// Token: 0x04002234 RID: 8756
			Direction
		}

		// Token: 0x020004C6 RID: 1222
		public enum UpwardVectors
		{
			// Token: 0x04002236 RID: 8758
			Forward,
			// Token: 0x04002237 RID: 8759
			Up,
			// Token: 0x04002238 RID: 8760
			Right
		}
	}
}
