using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x0200006B RID: 107
	[AddComponentMenu("More Mountains/Tools/Movement/MMPreventPassingThrough3D")]
	public class MMPreventPassingThrough3D : MonoBehaviour
	{
		// Token: 0x0600027A RID: 634 RVA: 0x0000BE0A File Offset: 0x0000A00A
		protected virtual void OnValidate()
		{
			if (this.RepositionRigidbody && this.RepositionRigidbodyLayerMask.value == 0)
			{
				this.RepositionRigidbodyLayerMask = this.ObstaclesLayerMask;
			}
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000BE2D File Offset: 0x0000A02D
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000BE38 File Offset: 0x0000A038
		protected virtual void Initialization()
		{
			this._rigidbody = base.GetComponent<Rigidbody>();
			this._positionLastFrame = this._rigidbody.position;
			this._collider = base.GetComponent<Collider>();
			this._adjustmentDistance = this.ComputeAdjustmentDistance();
			this._adjustedDistance = this._adjustmentDistance * (1f - this.SkinWidth);
			this._squaredBoundsWidth = this._adjustmentDistance * this._adjustmentDistance;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000BEA8 File Offset: 0x0000A0A8
		protected virtual float ComputeAdjustmentDistance()
		{
			switch (this.Adjustment)
			{
			case MMPreventPassingThrough3D.AdjustmentAxis.X:
				return this._collider.bounds.extents.x;
			case MMPreventPassingThrough3D.AdjustmentAxis.Y:
				return this._collider.bounds.extents.y;
			case MMPreventPassingThrough3D.AdjustmentAxis.Z:
				return this._collider.bounds.extents.z;
			default:
				return Mathf.Min(Mathf.Min(this._collider.bounds.extents.x, this._collider.bounds.extents.y), this._collider.bounds.extents.z);
			}
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000BF6F File Offset: 0x0000A16F
		protected virtual void OnEnable()
		{
			this._positionLastFrame = base.transform.position;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000BF84 File Offset: 0x0000A184
		protected virtual void FixedUpdate()
		{
			this._lastMovement = base.transform.position - this._positionLastFrame;
			this._lastMovementSquared = this._lastMovement.sqrMagnitude;
			if (this._lastMovementSquared > this._squaredBoundsWidth)
			{
				float num = Mathf.Sqrt(this._lastMovementSquared);
				RaycastHit raycastHit;
				if (Physics.Raycast(this._positionLastFrame, this._lastMovement, out raycastHit, num, this.ObstaclesLayerMask.value))
				{
					if (!raycastHit.collider)
					{
						return;
					}
					if (raycastHit.collider.isTrigger)
					{
						raycastHit.collider.SendMessage("OnTriggerEnter", this._collider);
					}
					if (!raycastHit.collider.isTrigger)
					{
						base.gameObject.SendMessage("PreventedCollision3D", raycastHit, SendMessageOptions.DontRequireReceiver);
						if (this.RepositionRigidbody)
						{
							int layer = raycastHit.collider.gameObject.layer;
							if (((1 << layer) & this.RepositionRigidbodyLayerMask) != 0)
							{
								base.transform.position = raycastHit.point - this._lastMovement / num * this._adjustedDistance;
								this._rigidbody.position = raycastHit.point - this._lastMovement / num * this._adjustedDistance;
							}
						}
					}
				}
			}
			this._positionLastFrame = base.transform.position;
		}

		// Token: 0x04000380 RID: 896
		public LayerMask ObstaclesLayerMask;

		// Token: 0x04000381 RID: 897
		public float SkinWidth = 0.1f;

		// Token: 0x04000382 RID: 898
		public bool RepositionRigidbody = true;

		// Token: 0x04000383 RID: 899
		public LayerMask RepositionRigidbodyLayerMask;

		// Token: 0x04000384 RID: 900
		public MMPreventPassingThrough3D.AdjustmentAxis Adjustment;

		// Token: 0x04000385 RID: 901
		protected float _adjustmentDistance;

		// Token: 0x04000386 RID: 902
		protected float _adjustedDistance;

		// Token: 0x04000387 RID: 903
		protected float _squaredBoundsWidth;

		// Token: 0x04000388 RID: 904
		protected Vector3 _positionLastFrame;

		// Token: 0x04000389 RID: 905
		protected Rigidbody _rigidbody;

		// Token: 0x0400038A RID: 906
		protected Collider _collider;

		// Token: 0x0400038B RID: 907
		protected Vector3 _lastMovement;

		// Token: 0x0400038C RID: 908
		protected float _lastMovementSquared;

		// Token: 0x020003E1 RID: 993
		public enum AdjustmentAxis
		{
			// Token: 0x04001E45 RID: 7749
			Auto,
			// Token: 0x04001E46 RID: 7750
			X,
			// Token: 0x04001E47 RID: 7751
			Y,
			// Token: 0x04001E48 RID: 7752
			Z
		}
	}
}
