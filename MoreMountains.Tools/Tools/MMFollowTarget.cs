using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000159 RID: 345
	[AddComponentMenu("More Mountains/Tools/Movement/MMFollowTarget")]
	public class MMFollowTarget : MonoBehaviour
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x00024617 File Offset: 0x00022817
		protected bool _localSpace
		{
			get
			{
				return this.PositionSpace == MMFollowTarget.PositionSpaces.Local;
			}
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x00024622 File Offset: 0x00022822
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x0002462A File Offset: 0x0002282A
		public virtual void Initialization()
		{
			this.SetInitialPosition();
			this.SetOffset();
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x00024638 File Offset: 0x00022838
		public virtual void StopFollowing()
		{
			this.FollowPosition = false;
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x00024641 File Offset: 0x00022841
		public virtual void StartFollowing()
		{
			this.FollowPosition = true;
			this.SetInitialPosition();
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x00024650 File Offset: 0x00022850
		protected virtual void SetInitialPosition()
		{
			this._initialPosition = (this._localSpace ? base.transform.localPosition : base.transform.position);
			this._initialRotation = base.transform.rotation;
			this._lastTargetPosition = (this._localSpace ? base.transform.localPosition : base.transform.position);
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x000246BC File Offset: 0x000228BC
		protected virtual void SetOffset()
		{
			if (this.Target == null)
			{
				return;
			}
			Vector3 vector = base.transform.position - this.Target.transform.position;
			this.Offset.x = (this.AddInitialDistanceXToXOffset ? vector.x : this.Offset.x);
			this.Offset.y = (this.AddInitialDistanceYToYOffset ? vector.y : this.Offset.y);
			this.Offset.z = (this.AddInitialDistanceZToZOffset ? vector.z : this.Offset.z);
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x0002476B File Offset: 0x0002296B
		protected virtual void Update()
		{
			if (this.Target == null)
			{
				return;
			}
			if (this.UpdateMode == MMFollowTarget.UpdateModes.Update)
			{
				this.FollowTargetRotation();
				this.FollowTargetScale();
				this.FollowTargetPosition();
			}
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x00024796 File Offset: 0x00022996
		protected virtual void FixedUpdate()
		{
			if (this.UpdateMode == MMFollowTarget.UpdateModes.FixedUpdate)
			{
				this.FollowTargetRotation();
				this.FollowTargetScale();
				this.FollowTargetPosition();
			}
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x000247B3 File Offset: 0x000229B3
		protected virtual void LateUpdate()
		{
			if (this.UpdateMode == MMFollowTarget.UpdateModes.LateUpdate)
			{
				this.FollowTargetRotation();
				this.FollowTargetScale();
				this.FollowTargetPosition();
			}
		}

		// Token: 0x060007AB RID: 1963 RVA: 0x000247D0 File Offset: 0x000229D0
		protected virtual void FollowTargetPosition()
		{
			if (this.Target == null)
			{
				return;
			}
			if (!this.FollowPosition)
			{
				return;
			}
			this._newTargetPosition = this.Target.position + this.Offset;
			if (!this.FollowPositionX)
			{
				this._newTargetPosition.x = this._initialPosition.x;
			}
			if (!this.FollowPositionY)
			{
				this._newTargetPosition.y = this._initialPosition.y;
			}
			if (!this.FollowPositionZ)
			{
				this._newTargetPosition.z = this._initialPosition.z;
			}
			this._direction = (this._newTargetPosition - base.transform.position).normalized;
			float num = Vector3.Distance(base.transform.position, this._newTargetPosition);
			float num2 = num;
			if (this.InterpolatePosition)
			{
				switch (this.FollowPositionMode)
				{
				case MMFollowTarget.FollowModes.RegularLerp:
					num2 = Mathf.Lerp(0f, num, Time.deltaTime * this.FollowPositionSpeed);
					num2 = this.ApplyMinMaxDistancing(num, num2);
					base.transform.Translate(this._direction * num2, Space.World);
					break;
				case MMFollowTarget.FollowModes.MMLerp:
					num2 = MMMaths.Lerp(0f, num, this.FollowPositionSpeed, Time.deltaTime);
					num2 = this.ApplyMinMaxDistancing(num, num2);
					base.transform.Translate(this._direction * num2, Space.World);
					break;
				case MMFollowTarget.FollowModes.MMSpring:
					this._newPosition = base.transform.position;
					MMMaths.Spring(ref this._newPosition, this._newTargetPosition, ref this._positionVelocity, this.PositionSpringDamping, this.PositionSpringFrequency, Time.deltaTime);
					if (this._localSpace)
					{
						base.transform.localPosition = this._newPosition;
					}
					else
					{
						base.transform.position = this._newPosition;
					}
					break;
				}
			}
			else
			{
				num2 = this.ApplyMinMaxDistancing(num, num2);
				base.transform.Translate(this._direction * num2, Space.World);
			}
			if (this.AnchorToInitialPosition && Vector3.Distance(base.transform.position, this._initialPosition) > this.MaxDistanceToAnchor)
			{
				if (this._localSpace)
				{
					base.transform.localPosition = this._initialPosition + Vector3.ClampMagnitude(base.transform.localPosition - this._initialPosition, this.MaxDistanceToAnchor);
					return;
				}
				base.transform.position = this._initialPosition + Vector3.ClampMagnitude(base.transform.position - this._initialPosition, this.MaxDistanceToAnchor);
			}
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x00024A7A File Offset: 0x00022C7A
		protected virtual float ApplyMinMaxDistancing(float trueDistance, float interpolatedDistance)
		{
			if (this.UseMinimumDistanceBeforeFollow && trueDistance - interpolatedDistance < this.MinimumDistanceBeforeFollow)
			{
				interpolatedDistance = 0f;
			}
			if (this.UseMaximumDistance && trueDistance - interpolatedDistance >= this.MaximumDistance)
			{
				interpolatedDistance = trueDistance - this.MaximumDistance;
			}
			return interpolatedDistance;
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x00024AB4 File Offset: 0x00022CB4
		protected virtual void FollowTargetRotation()
		{
			if (this.Target == null)
			{
				return;
			}
			if (!this.FollowRotation)
			{
				return;
			}
			this._newTargetRotation = this.Target.rotation;
			this._newTargetRotationEulerAngles = this.Target.rotation.eulerAngles;
			this._currentRotationEulerAngles = base.transform.rotation.eulerAngles;
			if (this.FollowRotationMode == MMFollowTarget.FollowModes.MMSpring && this._newTargetRotationEulerAnglesLastFrame != this._newTargetRotationEulerAngles)
			{
				this._rotationBeforeSpring = base.transform.rotation;
				this._rotationFloatCurrent = 0f;
				this._rotationFloatTarget = Mathf.Abs(this._newTargetRotation.eulerAngles.x) + Mathf.Abs(this._newTargetRotation.eulerAngles.y) + Mathf.Abs(this._newTargetRotation.z) - (Mathf.Abs(this._currentRotationEulerAngles.x) + Mathf.Abs(this._currentRotationEulerAngles.y) + Mathf.Abs(this._currentRotationEulerAngles.z));
				this._rotationFloatTarget = Mathf.Abs(this._rotationFloatTarget);
			}
			if (this.InterpolateRotation)
			{
				switch (this.FollowRotationMode)
				{
				case MMFollowTarget.FollowModes.RegularLerp:
					base.transform.rotation = Quaternion.Lerp(base.transform.rotation, this._newTargetRotation, Time.deltaTime * this.FollowRotationSpeed);
					break;
				case MMFollowTarget.FollowModes.MMLerp:
					base.transform.rotation = MMMaths.Lerp(base.transform.rotation, this._newTargetRotation, this.FollowRotationSpeed, Time.deltaTime);
					break;
				case MMFollowTarget.FollowModes.MMSpring:
					if (this._rotationFloatCurrent != this._rotationFloatTarget)
					{
						MMMaths.Spring(ref this._rotationFloatCurrent, this._rotationFloatTarget, ref this._rotationFloatVelocity, this.RotationSpringDamping, this.RotationSpringFrequency, Time.deltaTime);
						float num = MMMaths.Remap(this._rotationFloatCurrent, 0f, this._rotationFloatTarget, 0f, 1f);
						base.transform.rotation = Quaternion.LerpUnclamped(this._rotationBeforeSpring, this._newTargetRotation, num);
					}
					break;
				}
			}
			else
			{
				base.transform.rotation = this._newTargetRotation;
			}
			this._newTargetRotationEulerAnglesLastFrame = this._newTargetRotationEulerAngles;
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x00024D00 File Offset: 0x00022F00
		protected virtual void FollowTargetScale()
		{
			if (this.Target == null)
			{
				return;
			}
			if (!this.FollowScale)
			{
				return;
			}
			this._newTargetScale = this.Target.localScale * this.FollowScaleFactor;
			if (!this.InterpolateScale)
			{
				base.transform.localScale = this._newTargetScale;
				return;
			}
			switch (this.FollowScaleMode)
			{
			case MMFollowTarget.FollowModes.RegularLerp:
				base.transform.localScale = Vector3.Lerp(base.transform.localScale, this._newTargetScale, Time.deltaTime * this.FollowScaleSpeed);
				return;
			case MMFollowTarget.FollowModes.MMLerp:
				base.transform.localScale = MMMaths.Lerp(base.transform.localScale, this._newTargetScale, this.FollowScaleSpeed, Time.deltaTime);
				return;
			case MMFollowTarget.FollowModes.MMSpring:
				this._newScale = base.transform.localScale;
				MMMaths.Spring(ref this._newScale, this._newTargetScale, ref this._scaleVelocity, this.ScaleSpringDamping, this.ScaleSpringFrequency, Time.deltaTime);
				base.transform.localScale = this._newScale;
				return;
			default:
				return;
			}
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x00024E1D File Offset: 0x0002301D
		public virtual void ChangeFollowTarget(Transform newTarget)
		{
			this.Target = newTarget;
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x00024E26 File Offset: 0x00023026
		protected virtual void OnDisable()
		{
			if (this.DisableSelfOnSetActiveFalse)
			{
				base.enabled = false;
			}
		}

		// Token: 0x0400093E RID: 2366
		[Header("Follow Position")]
		public bool FollowPosition = true;

		// Token: 0x0400093F RID: 2367
		[MMCondition("FollowPosition", true)]
		public bool FollowPositionX = true;

		// Token: 0x04000940 RID: 2368
		[MMCondition("FollowPosition", true)]
		public bool FollowPositionY = true;

		// Token: 0x04000941 RID: 2369
		[MMCondition("FollowPosition", true)]
		public bool FollowPositionZ = true;

		// Token: 0x04000942 RID: 2370
		[MMCondition("FollowPosition", true)]
		public MMFollowTarget.PositionSpaces PositionSpace;

		// Token: 0x04000943 RID: 2371
		[Header("Follow Rotation")]
		public bool FollowRotation = true;

		// Token: 0x04000944 RID: 2372
		[Header("Follow Scale")]
		public bool FollowScale = true;

		// Token: 0x04000945 RID: 2373
		[MMCondition("FollowScale", true)]
		public float FollowScaleFactor = 1f;

		// Token: 0x04000946 RID: 2374
		[Header("Target")]
		public Transform Target;

		// Token: 0x04000947 RID: 2375
		[MMCondition("FollowPosition", true)]
		public Vector3 Offset;

		// Token: 0x04000948 RID: 2376
		[MMCondition("FollowPosition", true)]
		public bool AddInitialDistanceXToXOffset;

		// Token: 0x04000949 RID: 2377
		[MMCondition("FollowPosition", true)]
		public bool AddInitialDistanceYToYOffset;

		// Token: 0x0400094A RID: 2378
		[MMCondition("FollowPosition", true)]
		public bool AddInitialDistanceZToZOffset;

		// Token: 0x0400094B RID: 2379
		[Header("Position Interpolation")]
		public bool InterpolatePosition = true;

		// Token: 0x0400094C RID: 2380
		[MMCondition("InterpolatePosition", true)]
		public MMFollowTarget.FollowModes FollowPositionMode = MMFollowTarget.FollowModes.MMLerp;

		// Token: 0x0400094D RID: 2381
		[MMCondition("InterpolatePosition", true)]
		public float FollowPositionSpeed = 10f;

		// Token: 0x0400094E RID: 2382
		[MMEnumCondition("FollowPositionMode", new int[] { 2 })]
		[Range(0.01f, 1f)]
		public float PositionSpringDamping = 0.3f;

		// Token: 0x0400094F RID: 2383
		[MMEnumCondition("FollowPositionMode", new int[] { 2 })]
		public float PositionSpringFrequency = 3f;

		// Token: 0x04000950 RID: 2384
		[Header("Rotation Interpolation")]
		public bool InterpolateRotation = true;

		// Token: 0x04000951 RID: 2385
		[MMCondition("InterpolateRotation", true)]
		public MMFollowTarget.FollowModes FollowRotationMode = MMFollowTarget.FollowModes.MMLerp;

		// Token: 0x04000952 RID: 2386
		[MMCondition("InterpolateRotation", true)]
		public float FollowRotationSpeed = 10f;

		// Token: 0x04000953 RID: 2387
		[MMEnumCondition("FollowRotationMode", new int[] { 2 })]
		[Range(0.01f, 1f)]
		public float RotationSpringDamping = 0.3f;

		// Token: 0x04000954 RID: 2388
		[MMEnumCondition("FollowRotationMode", new int[] { 2 })]
		public float RotationSpringFrequency = 3f;

		// Token: 0x04000955 RID: 2389
		[Header("Scale Interpolation")]
		public bool InterpolateScale = true;

		// Token: 0x04000956 RID: 2390
		[MMCondition("InterpolateScale", true)]
		public MMFollowTarget.FollowModes FollowScaleMode = MMFollowTarget.FollowModes.MMLerp;

		// Token: 0x04000957 RID: 2391
		[MMCondition("InterpolateScale", true)]
		public float FollowScaleSpeed = 10f;

		// Token: 0x04000958 RID: 2392
		[MMEnumCondition("FollowScaleMode", new int[] { 2 })]
		[Range(0.01f, 1f)]
		public float ScaleSpringDamping = 0.3f;

		// Token: 0x04000959 RID: 2393
		[MMEnumCondition("FollowScaleMode", new int[] { 2 })]
		public float ScaleSpringFrequency = 3f;

		// Token: 0x0400095A RID: 2394
		[Header("Mode")]
		public MMFollowTarget.UpdateModes UpdateMode;

		// Token: 0x0400095B RID: 2395
		public bool DisableSelfOnSetActiveFalse;

		// Token: 0x0400095C RID: 2396
		[Header("Distances")]
		public bool UseMinimumDistanceBeforeFollow;

		// Token: 0x0400095D RID: 2397
		public float MinimumDistanceBeforeFollow = 1f;

		// Token: 0x0400095E RID: 2398
		public bool UseMaximumDistance;

		// Token: 0x0400095F RID: 2399
		public float MaximumDistance = 1f;

		// Token: 0x04000960 RID: 2400
		[Header("Anchor")]
		public bool AnchorToInitialPosition;

		// Token: 0x04000961 RID: 2401
		[MMCondition("AnchorToInitialPosition", true)]
		public float MaxDistanceToAnchor = 1f;

		// Token: 0x04000962 RID: 2402
		protected Vector3 _positionVelocity = Vector3.zero;

		// Token: 0x04000963 RID: 2403
		protected Vector3 _scaleVelocity = Vector3.zero;

		// Token: 0x04000964 RID: 2404
		protected Vector3 _rotationVelocity = Vector3.zero;

		// Token: 0x04000965 RID: 2405
		protected Vector3 _initialPosition;

		// Token: 0x04000966 RID: 2406
		protected Vector3 _direction;

		// Token: 0x04000967 RID: 2407
		protected Vector3 _newPosition;

		// Token: 0x04000968 RID: 2408
		protected Vector3 _newRotation;

		// Token: 0x04000969 RID: 2409
		protected Vector3 _newScale;

		// Token: 0x0400096A RID: 2410
		protected Vector3 _newTargetPosition;

		// Token: 0x0400096B RID: 2411
		protected Quaternion _newTargetRotation;

		// Token: 0x0400096C RID: 2412
		protected Vector3 _newTargetRotationEulerAngles;

		// Token: 0x0400096D RID: 2413
		protected Vector3 _newTargetRotationEulerAnglesLastFrame;

		// Token: 0x0400096E RID: 2414
		protected Vector3 _newTargetScale;

		// Token: 0x0400096F RID: 2415
		protected float _rotationFloatVelocity;

		// Token: 0x04000970 RID: 2416
		protected float _rotationFloatCurrent;

		// Token: 0x04000971 RID: 2417
		protected float _rotationFloatTarget;

		// Token: 0x04000972 RID: 2418
		protected Vector3 _currentRotationEulerAngles;

		// Token: 0x04000973 RID: 2419
		protected Quaternion _rotationBeforeSpring;

		// Token: 0x04000974 RID: 2420
		protected Quaternion _initialRotation;

		// Token: 0x04000975 RID: 2421
		protected Vector3 _lastTargetPosition;

		// Token: 0x0200045A RID: 1114
		public enum UpdateModes
		{
			// Token: 0x04002023 RID: 8227
			Update,
			// Token: 0x04002024 RID: 8228
			FixedUpdate,
			// Token: 0x04002025 RID: 8229
			LateUpdate
		}

		// Token: 0x0200045B RID: 1115
		public enum FollowModes
		{
			// Token: 0x04002027 RID: 8231
			RegularLerp,
			// Token: 0x04002028 RID: 8232
			MMLerp,
			// Token: 0x04002029 RID: 8233
			MMSpring
		}

		// Token: 0x0200045C RID: 1116
		public enum PositionSpaces
		{
			// Token: 0x0400202B RID: 8235
			World,
			// Token: 0x0400202C RID: 8236
			Local
		}
	}
}
