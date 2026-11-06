using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoreMountains.Tools
{
	// Token: 0x0200006A RID: 106
	[AddComponentMenu("More Mountains/Tools/Movement/MMPreventPassingThrough2D")]
	public class MMPreventPassingThrough2D : MonoBehaviour
	{
		// Token: 0x06000275 RID: 629 RVA: 0x0000BAA0 File Offset: 0x00009CA0
		protected virtual void Start()
		{
			this.Initialization();
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000BAA8 File Offset: 0x00009CA8
		protected virtual void Initialization()
		{
			this._rigidbody = base.GetComponent<Rigidbody2D>();
			this._positionLastFrame = this._rigidbody.position;
			this._collider = base.GetComponent<Collider2D>();
			if (this._collider as BoxCollider2D != null)
			{
				this._colliderSize = (this._collider as BoxCollider2D).size;
			}
			this._smallestBoundsWidth = Mathf.Min(Mathf.Min(this._collider.bounds.extents.x, this._collider.bounds.extents.y), this._collider.bounds.extents.z);
			this._adjustedSmallestBoundsWidth = this._smallestBoundsWidth * (1f - this.SkinWidth);
			this._squaredBoundsWidth = this._smallestBoundsWidth * this._smallestBoundsWidth;
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000BB90 File Offset: 0x00009D90
		protected virtual void OnEnable()
		{
			this._positionLastFrame = base.transform.position;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000BBA4 File Offset: 0x00009DA4
		protected virtual void Update()
		{
			this._lastMovement = base.transform.position - this._positionLastFrame;
			this._lastMovementSquared = this._lastMovement.sqrMagnitude;
			if (this._lastMovementSquared > this._squaredBoundsWidth)
			{
				float num = Mathf.Sqrt(this._lastMovementSquared);
				if (this.Mode == MMPreventPassingThrough2D.Modes.Raycast)
				{
					this._hitInfo = MMDebug.RayCast(this._positionLastFrame, this._lastMovement.normalized, num, this.ObstaclesLayerMask, Color.blue, true);
				}
				else
				{
					Vector2 vector = this._positionLastFrame;
					Vector2 colliderSize = this._colliderSize;
					float num2 = 0f;
					int num3 = this.ObstaclesLayerMask;
					this._hitInfo = Physics2D.BoxCast(vector, colliderSize, num2, this._lastMovement.normalized, num, num3);
				}
				if (this._hitInfo.collider != null)
				{
					if (this._hitInfo.collider.isTrigger)
					{
						this._hitInfo.collider.SendMessage("OnTriggerEnter2D", this._collider, SendMessageOptions.DontRequireReceiver);
						if (this.RepositionRigidbodyIfHitTrigger)
						{
							base.transform.position = this._hitInfo.point - this._lastMovement / num * this._adjustedSmallestBoundsWidth;
							this._rigidbody.position = this._hitInfo.point - this._lastMovement / num * this._adjustedSmallestBoundsWidth;
						}
					}
					if (!this._hitInfo.collider.isTrigger)
					{
						this.Hit = this._hitInfo;
						base.gameObject.SendMessage("PreventedCollision2D", this.Hit, SendMessageOptions.DontRequireReceiver);
						if (this.RepositionRigidbodyIfHitNonTrigger)
						{
							base.transform.position = this._hitInfo.point - this._lastMovement / num * this._adjustedSmallestBoundsWidth;
							this._rigidbody.position = this._hitInfo.point - this._lastMovement / num * this._adjustedSmallestBoundsWidth;
						}
					}
				}
			}
			this._positionLastFrame = base.transform.position;
		}

		// Token: 0x04000370 RID: 880
		public MMPreventPassingThrough2D.Modes Mode;

		// Token: 0x04000371 RID: 881
		public LayerMask ObstaclesLayerMask;

		// Token: 0x04000372 RID: 882
		public float SkinWidth = 0.1f;

		// Token: 0x04000373 RID: 883
		public bool RepositionRigidbodyIfHitTrigger = true;

		// Token: 0x04000374 RID: 884
		[FormerlySerializedAs("RepositionRigidbody")]
		public bool RepositionRigidbodyIfHitNonTrigger = true;

		// Token: 0x04000375 RID: 885
		[Header("Debug")]
		[MMReadOnly]
		public RaycastHit2D Hit;

		// Token: 0x04000376 RID: 886
		protected float _smallestBoundsWidth;

		// Token: 0x04000377 RID: 887
		protected float _adjustedSmallestBoundsWidth;

		// Token: 0x04000378 RID: 888
		protected float _squaredBoundsWidth;

		// Token: 0x04000379 RID: 889
		protected Vector3 _positionLastFrame;

		// Token: 0x0400037A RID: 890
		protected Rigidbody2D _rigidbody;

		// Token: 0x0400037B RID: 891
		protected Collider2D _collider;

		// Token: 0x0400037C RID: 892
		protected Vector2 _lastMovement;

		// Token: 0x0400037D RID: 893
		protected float _lastMovementSquared;

		// Token: 0x0400037E RID: 894
		protected RaycastHit2D _hitInfo;

		// Token: 0x0400037F RID: 895
		protected Vector2 _colliderSize;

		// Token: 0x020003E0 RID: 992
		public enum Modes
		{
			// Token: 0x04001E42 RID: 7746
			Raycast,
			// Token: 0x04001E43 RID: 7747
			BoxCast
		}
	}
}
