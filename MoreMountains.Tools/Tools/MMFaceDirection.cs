using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000064 RID: 100
	public class MMFaceDirection : MonoBehaviour
	{
		// Token: 0x06000233 RID: 563 RVA: 0x0000AAE2 File Offset: 0x00008CE2
		protected virtual void Awake()
		{
			this.Initialization();
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000AAEC File Offset: 0x00008CEC
		protected virtual void Initialization()
		{
			this._transform = base.transform;
			this._positionLastFrame = this._transform.position;
			switch (this.ForwardVector)
			{
			case MMFaceDirection.ForwardVectors.Forward:
				this._upwards = Vector3.forward;
				return;
			case MMFaceDirection.ForwardVectors.Up:
				this._upwards = Vector3.up;
				return;
			case MMFaceDirection.ForwardVectors.Right:
				this._upwards = Vector3.right;
				return;
			default:
				return;
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000AB54 File Offset: 0x00008D54
		protected virtual void FaceDirection()
		{
			if (this.FacingMode == MMFaceDirection.FacingModes.Target)
			{
				this._targetPosition = this.FacingTarget.position;
				if (this.LockXAxis)
				{
					this._targetPosition.x = this._transform.position.x;
				}
				if (this.LockYAxis)
				{
					this._targetPosition.y = this._transform.position.y;
				}
				if (this.LockZAxis)
				{
					this._targetPosition.z = this._transform.position.z;
				}
				this._direction = this._targetPosition - this._transform.position;
				this._direction = Quaternion.Euler(this.DirectionRotationAngles.x, this.DirectionRotationAngles.y, this.DirectionRotationAngles.z) * this._direction;
				this.ApplyRotation();
			}
			if (this.FacingMode == MMFaceDirection.FacingModes.MovementDirection)
			{
				this._direction = (this._transform.position - this._positionLastFrame).normalized;
				if (this.LockXAxis)
				{
					this._direction.x = 0f;
				}
				if (this.LockYAxis)
				{
					this._direction.y = 0f;
				}
				if (this.LockZAxis)
				{
					this._direction.z = 0f;
				}
				this._direction = Quaternion.Euler(this.DirectionRotationAngles.x, this.DirectionRotationAngles.y, this.DirectionRotationAngles.z) * this._direction;
				if (Vector3.Distance(this._transform.position, this._positionLastFrame) > this.MinimumMovementThreshold)
				{
					this.ApplyRotation();
					this._positionLastFrame = this._transform.position;
				}
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000AD23 File Offset: 0x00008F23
		protected virtual void ApplyRotation()
		{
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation(this._direction, this._upwards), this.InterpolationSpeed * Time.time);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000AD5D File Offset: 0x00008F5D
		protected virtual void Update()
		{
			if (this.UpdateMode == MMFaceDirection.UpdateModes.Update)
			{
				this.FaceDirection();
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000AD6D File Offset: 0x00008F6D
		protected virtual void LateUpdate()
		{
			if (this.UpdateMode == MMFaceDirection.UpdateModes.LateUpdate)
			{
				this.FaceDirection();
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000AD7E File Offset: 0x00008F7E
		protected virtual void FixedUpdate()
		{
			if (this.UpdateMode == MMFaceDirection.UpdateModes.FixedUpdate)
			{
				this.FaceDirection();
			}
		}

		// Token: 0x0400031C RID: 796
		[Header("Facing Mode")]
		public MMFaceDirection.FacingModes FacingMode;

		// Token: 0x0400031D RID: 797
		[MMEnumCondition("FacingMode", new int[] { 1 })]
		public Transform FacingTarget;

		// Token: 0x0400031E RID: 798
		[MMEnumCondition("FacingMode", new int[] { 0 })]
		public float MinimumMovementThreshold = 0.2f;

		// Token: 0x0400031F RID: 799
		[Header("Directions")]
		public MMFaceDirection.ForwardVectors ForwardVector;

		// Token: 0x04000320 RID: 800
		public Vector3 DirectionRotationAngles = Vector3.zero;

		// Token: 0x04000321 RID: 801
		[Header("Axis Locks")]
		public bool LockXAxis;

		// Token: 0x04000322 RID: 802
		public bool LockYAxis;

		// Token: 0x04000323 RID: 803
		public bool LockZAxis;

		// Token: 0x04000324 RID: 804
		[Header("Timing")]
		public MMFaceDirection.UpdateModes UpdateMode = MMFaceDirection.UpdateModes.LateUpdate;

		// Token: 0x04000325 RID: 805
		public float InterpolationSpeed = 0.15f;

		// Token: 0x04000326 RID: 806
		protected Vector3 _direction;

		// Token: 0x04000327 RID: 807
		protected Vector3 _positionLastFrame;

		// Token: 0x04000328 RID: 808
		protected Transform _transform;

		// Token: 0x04000329 RID: 809
		protected Vector3 _upwards;

		// Token: 0x0400032A RID: 810
		protected Vector3 _targetPosition;

		// Token: 0x020003D2 RID: 978
		public enum UpdateModes
		{
			// Token: 0x04001E0B RID: 7691
			Update,
			// Token: 0x04001E0C RID: 7692
			LateUpdate,
			// Token: 0x04001E0D RID: 7693
			FixedUpdate
		}

		// Token: 0x020003D3 RID: 979
		public enum ForwardVectors
		{
			// Token: 0x04001E0F RID: 7695
			Forward,
			// Token: 0x04001E10 RID: 7696
			Up,
			// Token: 0x04001E11 RID: 7697
			Right
		}

		// Token: 0x020003D4 RID: 980
		public enum FacingModes
		{
			// Token: 0x04001E13 RID: 7699
			MovementDirection,
			// Token: 0x04001E14 RID: 7700
			Target
		}
	}
}
