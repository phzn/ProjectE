using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000069 RID: 105
	[AddComponentMenu("More Mountains/Tools/Movement/MMPreventPassingThrough")]
	public class MMPreventPassingThrough : MonoBehaviour
	{
		// Token: 0x06000270 RID: 624 RVA: 0x0000B8BB File Offset: 0x00009ABB
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000B8C4 File Offset: 0x00009AC4
		protected virtual void Initialization()
		{
			this._rigidbody = base.GetComponent<Rigidbody>();
			this._positionLastFrame = this._rigidbody.position;
			this._collider = base.GetComponent<Collider>();
			this._smallestBoundsWidth = Mathf.Min(Mathf.Min(this._collider.bounds.extents.x, this._collider.bounds.extents.y), this._collider.bounds.extents.z);
			this._adjustedSmallestBoundsWidth = this._smallestBoundsWidth * (1f - this.SkinWidth);
			this._squaredBoundsWidth = this._smallestBoundsWidth * this._smallestBoundsWidth;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000B97E File Offset: 0x00009B7E
		protected virtual void OnEnable()
		{
			this._positionLastFrame = base.transform.position;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000B994 File Offset: 0x00009B94
		protected virtual void FixedUpdate()
		{
			this._lastMovement = this._rigidbody.position - this._positionLastFrame;
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
						this._rigidbody.position = raycastHit.point - this._lastMovement / num * this._adjustedSmallestBoundsWidth;
					}
				}
			}
			this._positionLastFrame = this._rigidbody.position;
		}

		// Token: 0x04000366 RID: 870
		public LayerMask ObstaclesLayerMask;

		// Token: 0x04000367 RID: 871
		public float SkinWidth = 0.1f;

		// Token: 0x04000368 RID: 872
		protected float _smallestBoundsWidth;

		// Token: 0x04000369 RID: 873
		protected float _adjustedSmallestBoundsWidth;

		// Token: 0x0400036A RID: 874
		protected float _squaredBoundsWidth;

		// Token: 0x0400036B RID: 875
		protected Vector3 _positionLastFrame;

		// Token: 0x0400036C RID: 876
		protected Rigidbody _rigidbody;

		// Token: 0x0400036D RID: 877
		protected Collider _collider;

		// Token: 0x0400036E RID: 878
		protected Vector3 _lastMovement;

		// Token: 0x0400036F RID: 879
		protected float _lastMovementSquared;
	}
}
