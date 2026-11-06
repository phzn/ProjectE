using System;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;

namespace MoreMountains.Feedbacks
{
	// Token: 0x020001FD RID: 509
	[AddComponentMenu("")]
	[FeedbackHelp("This feedback will animate the target object's position over time, for the specified duration, from the chosen initial position to the chosen destination. These can either be relative Vector3 offsets from the Feedback's position, or Transforms. If you specify transforms, the Vector3 values will be ignored.")]
	[MovedFrom(false, null, "MoreMountains.Feedbacks", null)]
	[FeedbackPath("Transform/Position")]
	public class MMF_Position : MMF_Feedback
	{
		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000D71 RID: 3441 RVA: 0x0003AEC7 File Offset: 0x000390C7
		public override bool HasRandomness
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000D72 RID: 3442 RVA: 0x0003AECA File Offset: 0x000390CA
		public override bool CanForceInitialValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000D73 RID: 3443 RVA: 0x0003AECD File Offset: 0x000390CD
		public override bool HasAutomatedTargetAcquisition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x0003AED0 File Offset: 0x000390D0
		protected override void AutomateTargetAcquisition()
		{
			this.AnimatePositionTarget = this.FindAutomatedTargetGameObject();
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000D75 RID: 3445 RVA: 0x0003AEDE File Offset: 0x000390DE
		// (set) Token: 0x06000D76 RID: 3446 RVA: 0x0003AEEC File Offset: 0x000390EC
		public override float FeedbackDuration
		{
			get
			{
				return this.ApplyTimeMultiplier(this.AnimatePositionDuration);
			}
			set
			{
				this.AnimatePositionDuration = value;
			}
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x0003AEF8 File Offset: 0x000390F8
		protected override void CustomInitialization(MMF_Player owner)
		{
			base.CustomInitialization(owner);
			if (this.Active)
			{
				if (this.AnimatePositionTarget == null)
				{
					Debug.LogWarning("The animate position target for " + ((this != null) ? this.ToString() : null) + " is null, you have to define it in the inspector");
					return;
				}
				if (this.Space == MMF_Position.Spaces.RectTransform)
				{
					this._rectTransform = this.AnimatePositionTarget.GetComponent<RectTransform>();
				}
				if (!this.DeterminePositionsOnPlay)
				{
					this.DeterminePositions();
				}
			}
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x0003AF6C File Offset: 0x0003916C
		protected virtual void DeterminePositions()
		{
			if (this.DeterminePositionsOnPlay && this.RelativePosition && this.InitialPosition != Vector3.zero)
			{
				return;
			}
			if (this.InitialPositionTransform != null)
			{
				this._workInitialPosition = this.GetPosition(this.InitialPositionTransform);
			}
			else
			{
				this._workInitialPosition = (this.RelativePosition ? (this.GetPosition(this.AnimatePositionTarget.transform) + this.InitialPosition) : this.InitialPosition);
				if (this.Space == MMF_Position.Spaces.Self && !this.RelativePosition)
				{
					this._workInitialPosition = this.AnimatePositionTarget.transform.position + this.InitialPosition;
				}
			}
			if (this.DestinationPositionTransform != null)
			{
				this._workDestinationPosition = this.GetPosition(this.DestinationPositionTransform);
				return;
			}
			this._workDestinationPosition = (this.RelativePosition ? (this.GetPosition(this.AnimatePositionTarget.transform) + this.DestinationPosition) : this.DestinationPosition);
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x0003B078 File Offset: 0x00039278
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Position.FeedbackTypeAuthorized || this.AnimatePositionTarget == null)
			{
				return;
			}
			if (this.Active || this.Owner.AutoPlayOnEnable)
			{
				if (this.DeterminePositionsOnPlay && this.NormalPlayDirection)
				{
					this.DeterminePositions();
				}
				switch (this.Mode)
				{
				case MMF_Position.Modes.AtoB:
					if (!this.AllowAdditivePlays && this._coroutine != null)
					{
						return;
					}
					if (this._coroutine != null)
					{
						this.Owner.StopCoroutine(this._coroutine);
					}
					this._coroutine = this.Owner.StartCoroutine(this.MoveFromTo(this.AnimatePositionTarget, this._workInitialPosition, this._workDestinationPosition, this.FeedbackDuration, this.AnimatePositionTween));
					return;
				case MMF_Position.Modes.AlongCurve:
				{
					if (!this.AllowAdditivePlays && this._coroutine != null)
					{
						return;
					}
					float num = this.ComputeIntensity(feedbacksIntensity, position);
					this._remapCurveZero = (this.RandomizeRemap ? Random.Range(this.RemapCurveZero, this.RemapCurveZeroAlt) : this.RemapCurveZero);
					this._remapCurveOne = (this.RandomizeRemap ? Random.Range(this.RemapCurveOne, this.RemapCurveOneAlt) : this.RemapCurveOne);
					if (this._coroutine != null)
					{
						this.Owner.StopCoroutine(this._coroutine);
					}
					this._coroutine = this.Owner.StartCoroutine(this.MoveAlongCurve(this.AnimatePositionTarget, this._workInitialPosition, this.FeedbackDuration, num));
					break;
				}
				case MMF_Position.Modes.ToDestination:
					this._initialPosition = this.GetPosition(this.AnimatePositionTarget.transform);
					this._destinationPosition = this._workDestinationPosition;
					if (this.DestinationPositionTransform != null)
					{
						this._destinationPosition = this.GetPosition(this.DestinationPositionTransform);
					}
					if (this._coroutine != null)
					{
						this.Owner.StopCoroutine(this._coroutine);
					}
					this._coroutine = this.Owner.StartCoroutine(this.MoveFromTo(this.AnimatePositionTarget, this._initialPosition, this._destinationPosition, this.FeedbackDuration, this.AnimatePositionTween));
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x06000D7A RID: 3450 RVA: 0x0003B288 File Offset: 0x00039488
		protected virtual IEnumerator MoveAlongCurve(GameObject movingObject, Vector3 initialPosition, float duration, float intensityMultiplier)
		{
			this.IsPlaying = true;
			float journey = (this.NormalPlayDirection ? 0f : duration);
			while (journey >= 0f && journey <= duration && duration > 0f)
			{
				float num = Mathf.Clamp01(journey / duration);
				this.ComputeNewCurvePosition(movingObject, initialPosition, num, intensityMultiplier);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			this.ComputeNewCurvePosition(movingObject, initialPosition, this.FinalNormalizedTime, intensityMultiplier);
			this._coroutine = null;
			this.IsPlaying = false;
			yield break;
		}

		// Token: 0x06000D7B RID: 3451 RVA: 0x0003B2B4 File Offset: 0x000394B4
		protected virtual void ComputeNewCurvePosition(GameObject movingObject, Vector3 initialPosition, float percent, float intensityMultiplier)
		{
			float num = MMTween.Tween(percent, 0f, 1f, this._remapCurveZero * intensityMultiplier, this._remapCurveOne * intensityMultiplier, this.AnimatePositionTweenX);
			float num2 = MMTween.Tween(percent, 0f, 1f, this._remapCurveZero * intensityMultiplier, this._remapCurveOne * intensityMultiplier, this.AnimatePositionTweenY);
			float num3 = MMTween.Tween(percent, 0f, 1f, this._remapCurveZero * intensityMultiplier, this._remapCurveOne * intensityMultiplier, this.AnimatePositionTweenZ);
			this._newPosition = initialPosition;
			this._currentPosition = this.GetPosition(movingObject.transform);
			if (this.RelativePosition)
			{
				this._newPosition.x = (this.AnimateX ? (initialPosition.x + num) : this._currentPosition.x);
				this._newPosition.y = (this.AnimateY ? (initialPosition.y + num2) : this._currentPosition.y);
				this._newPosition.z = (this.AnimateZ ? (initialPosition.z + num3) : this._currentPosition.z);
			}
			else
			{
				this._newPosition.x = (this.AnimateX ? num : this._currentPosition.x);
				this._newPosition.y = (this.AnimateY ? num2 : this._currentPosition.y);
				this._newPosition.z = (this.AnimateZ ? num3 : this._currentPosition.z);
			}
			if (this.Space == MMF_Position.Spaces.Self)
			{
				this._newPosition.x = (this.AnimateX ? num : 0f);
				this._newPosition.y = (this.AnimateY ? num2 : 0f);
				this._newPosition.z = (this.AnimateZ ? num3 : 0f);
			}
			this.SetPosition(movingObject.transform, this._newPosition);
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x0003B4A6 File Offset: 0x000396A6
		protected virtual IEnumerator MoveFromTo(GameObject movingObject, Vector3 pointA, Vector3 pointB, float duration, MMTweenType tweenType)
		{
			this.IsPlaying = true;
			float journey = (this.NormalPlayDirection ? 0f : duration);
			while (journey >= 0f && journey <= duration && duration > 0f)
			{
				float num = MMTween.Tween(journey, 0f, duration, 0f, 1f, tweenType);
				this._newPosition = Vector3.LerpUnclamped(pointA, pointB, num);
				this.SetPosition(movingObject.transform, this._newPosition);
				journey += (this.NormalPlayDirection ? this.FeedbackDeltaTime : (-this.FeedbackDeltaTime));
				yield return null;
			}
			if (this.NormalPlayDirection)
			{
				this.SetPosition(movingObject.transform, pointB);
			}
			else
			{
				this.SetPosition(movingObject.transform, pointA);
			}
			this._coroutine = null;
			this.IsPlaying = false;
			yield break;
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x0003B4DC File Offset: 0x000396DC
		protected virtual Vector3 GetPosition(Transform target)
		{
			switch (this.Space)
			{
			case MMF_Position.Spaces.World:
				return target.position;
			case MMF_Position.Spaces.Local:
				return target.localPosition;
			case MMF_Position.Spaces.RectTransform:
				return target.gameObject.GetComponent<RectTransform>().anchoredPosition;
			case MMF_Position.Spaces.Self:
				return target.position;
			default:
				return Vector3.zero;
			}
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x0003B538 File Offset: 0x00039738
		protected virtual void SetPosition(Transform target, Vector3 newPosition)
		{
			switch (this.Space)
			{
			case MMF_Position.Spaces.World:
				target.position = newPosition;
				return;
			case MMF_Position.Spaces.Local:
				target.localPosition = newPosition;
				return;
			case MMF_Position.Spaces.RectTransform:
				this._rectTransform.anchoredPosition = newPosition;
				return;
			case MMF_Position.Spaces.Self:
				target.position = this._workInitialPosition;
				if (this.Mode == MMF_Position.Modes.AtoB || this.Mode == MMF_Position.Modes.ToDestination)
				{
					newPosition -= this._workInitialPosition;
				}
				target.Translate(newPosition, target);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x0003B5B8 File Offset: 0x000397B8
		protected override void CustomStopFeedback(Vector3 position, float feedbacksIntensity = 1f)
		{
			if (!this.Active || !MMF_Position.FeedbackTypeAuthorized || this._coroutine == null)
			{
				return;
			}
			this.IsPlaying = false;
			this.Owner.StopCoroutine(this._coroutine);
			this._coroutine = null;
		}

		// Token: 0x06000D80 RID: 3456 RVA: 0x0003B5F1 File Offset: 0x000397F1
		protected override void CustomRestoreInitialValues()
		{
			if (!this.Active || !MMF_Position.FeedbackTypeAuthorized)
			{
				return;
			}
			this.SetPosition(this.AnimatePositionTarget.transform, this._workInitialPosition);
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0003B61A File Offset: 0x0003981A
		public override void OnDisable()
		{
			this._coroutine = null;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x0003B624 File Offset: 0x00039824
		public override void OnValidate()
		{
			base.OnValidate();
			MMFeedbacksHelpers.MigrateCurve(this.AnimatePositionCurve, this.AnimatePositionTween, this.Owner);
			MMFeedbacksHelpers.MigrateCurve(this.AnimatePositionCurveX, this.AnimatePositionTweenX, this.Owner);
			MMFeedbacksHelpers.MigrateCurve(this.AnimatePositionCurveY, this.AnimatePositionTweenY, this.Owner);
			MMFeedbacksHelpers.MigrateCurve(this.AnimatePositionCurveZ, this.AnimatePositionTweenZ, this.Owner);
		}

		// Token: 0x04000F5D RID: 3933
		public static bool FeedbackTypeAuthorized = true;

		// Token: 0x04000F5E RID: 3934
		[MMFInspectorGroup("Position Target", true, 61, true, false)]
		[Tooltip("the object this feedback will animate the position for")]
		public GameObject AnimatePositionTarget;

		// Token: 0x04000F5F RID: 3935
		[MMFInspectorGroup("Transition", true, 63, false, false)]
		[Tooltip("the mode this animation should follow (either going from A to B, or moving along a curve)")]
		public MMF_Position.Modes Mode;

		// Token: 0x04000F60 RID: 3936
		[Tooltip("the space in which to move the position in")]
		public MMF_Position.Spaces Space;

		// Token: 0x04000F61 RID: 3937
		[Tooltip("whether or not to randomize remap values between their base and alt values on play, useful to add some variety every time you play this feedback")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public bool RandomizeRemap;

		// Token: 0x04000F62 RID: 3938
		[Tooltip("the duration of the animation on play")]
		public float AnimatePositionDuration = 0.2f;

		// Token: 0x04000F63 RID: 3939
		[Tooltip("the MMTween curve definition to use instead of the animation curve to define the acceleration of the movement")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public MMTweenType AnimatePositionTween = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		}));

		// Token: 0x04000F64 RID: 3940
		[MMFEnumCondition("Mode", new int[] { 1 })]
		[Tooltip("the value to remap the curve's 0 value to")]
		public float RemapCurveZero;

		// Token: 0x04000F65 RID: 3941
		[MMFCondition("RandomizeRemap", true)]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		[Tooltip("in randomize remap mode, the value to remap the curve's 0 value to (randomized between this and RemapCurveZero")]
		public float RemapCurveZeroAlt;

		// Token: 0x04000F66 RID: 3942
		[Tooltip("the value to remap the curve's 1 value to")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		[FormerlySerializedAs("CurveMultiplier")]
		public float RemapCurveOne = 1f;

		// Token: 0x04000F67 RID: 3943
		[Tooltip("in randomize remap mode, the value to remap the curve's 1 value to (randomized between this and RemapCurveOne)")]
		[MMFCondition("RandomizeRemap", true)]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public float RemapCurveOneAlt = 1f;

		// Token: 0x04000F68 RID: 3944
		[Tooltip("if this is true, the x position will be animated")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public bool AnimateX;

		// Token: 0x04000F69 RID: 3945
		[Tooltip("the acceleration of the movement")]
		[MMFCondition("AnimateX", true)]
		public MMTweenType AnimatePositionTweenX = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(0.6f, -1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x04000F6A RID: 3946
		[Tooltip("if this is true, the y position will be animated")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public bool AnimateY;

		// Token: 0x04000F6B RID: 3947
		[Tooltip("the acceleration of the movement")]
		[MMFCondition("AnimateY", true)]
		public MMTweenType AnimatePositionTweenY = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(0.6f, -1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x04000F6C RID: 3948
		[Tooltip("if this is true, the z position will be animated")]
		[MMFEnumCondition("Mode", new int[] { 1 })]
		public bool AnimateZ;

		// Token: 0x04000F6D RID: 3949
		[Tooltip("the acceleration of the movement")]
		[MMFCondition("AnimateZ", true)]
		public MMTweenType AnimatePositionTweenZ = new MMTweenType(new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(0.3f, 1f),
			new Keyframe(0.6f, -1f),
			new Keyframe(1f, 0f)
		}));

		// Token: 0x04000F6E RID: 3950
		[Tooltip("if this is true, calling that feedback will trigger it, even if it's in progress. If it's false, it'll prevent any new Play until the current one is over")]
		public bool AllowAdditivePlays;

		// Token: 0x04000F6F RID: 3951
		[MMFInspectorGroup("Positions", true, 64, false, false)]
		[Tooltip("if this is true, movement will be relative to the object's initial position. So moving its y position along a curve going from 0 to 1 will move it up one unit. If this is false, in that same example, it'll be moved from 0 to 1 in absolute coordinates.")]
		public bool RelativePosition = true;

		// Token: 0x04000F70 RID: 3952
		[Tooltip("if this is true, initial and destination positions will be recomputed on every play")]
		public bool DeterminePositionsOnPlay;

		// Token: 0x04000F71 RID: 3953
		[Tooltip("the initial position")]
		[MMFEnumCondition("Mode", new int[] { 0, 1 })]
		public Vector3 InitialPosition = Vector3.zero;

		// Token: 0x04000F72 RID: 3954
		[Tooltip("the destination position")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public Vector3 DestinationPosition = Vector3.one;

		// Token: 0x04000F73 RID: 3955
		[Tooltip("the initial transform - if set, takes precedence over the Vector3 above")]
		[MMFEnumCondition("Mode", new int[] { 0, 1 })]
		public Transform InitialPositionTransform;

		// Token: 0x04000F74 RID: 3956
		[Tooltip("the destination transform - if set, takes precedence over the Vector3 above")]
		[MMFEnumCondition("Mode", new int[] { 0, 2 })]
		public Transform DestinationPositionTransform;

		// Token: 0x04000F75 RID: 3957
		[HideInInspector]
		public AnimationCurve AnimatePositionCurveX;

		// Token: 0x04000F76 RID: 3958
		[HideInInspector]
		public AnimationCurve AnimatePositionCurveY;

		// Token: 0x04000F77 RID: 3959
		[HideInInspector]
		public AnimationCurve AnimatePositionCurveZ;

		// Token: 0x04000F78 RID: 3960
		[HideInInspector]
		public AnimationCurve AnimatePositionCurve;

		// Token: 0x04000F79 RID: 3961
		protected Vector3 _newPosition;

		// Token: 0x04000F7A RID: 3962
		protected Vector3 _currentPosition;

		// Token: 0x04000F7B RID: 3963
		protected RectTransform _rectTransform;

		// Token: 0x04000F7C RID: 3964
		protected Vector3 _initialPosition;

		// Token: 0x04000F7D RID: 3965
		protected Vector3 _destinationPosition;

		// Token: 0x04000F7E RID: 3966
		protected Coroutine _coroutine;

		// Token: 0x04000F7F RID: 3967
		protected Vector3 _workInitialPosition;

		// Token: 0x04000F80 RID: 3968
		protected Vector3 _workDestinationPosition;

		// Token: 0x04000F81 RID: 3969
		protected float _remapCurveZero;

		// Token: 0x04000F82 RID: 3970
		protected float _remapCurveOne;

		// Token: 0x020004D4 RID: 1236
		public enum Spaces
		{
			// Token: 0x04002282 RID: 8834
			World,
			// Token: 0x04002283 RID: 8835
			Local,
			// Token: 0x04002284 RID: 8836
			RectTransform,
			// Token: 0x04002285 RID: 8837
			Self
		}

		// Token: 0x020004D5 RID: 1237
		public enum Modes
		{
			// Token: 0x04002287 RID: 8839
			AtoB,
			// Token: 0x04002288 RID: 8840
			AlongCurve,
			// Token: 0x04002289 RID: 8841
			ToDestination
		}

		// Token: 0x020004D6 RID: 1238
		public enum TimeScales
		{
			// Token: 0x0400228B RID: 8843
			Scaled,
			// Token: 0x0400228C RID: 8844
			Unscaled
		}
	}
}
