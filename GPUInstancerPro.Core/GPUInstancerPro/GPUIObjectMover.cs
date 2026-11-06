using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x0200004B RID: 75
	public class GPUIObjectMover : MonoBehaviour
	{
		// Token: 0x06000291 RID: 657 RVA: 0x0000E434 File Offset: 0x0000C634
		private void OnEnable()
		{
			this._cachedTransform = base.transform;
			this._startPosition = this._cachedTransform.position;
			this._startEulerAngles = this._cachedTransform.rotation.eulerAngles;
			if (this.isRandomDirection)
			{
				int num = ((Random.value >= 0.5f) ? 1 : (-1));
				this.forwardMove *= (float)num;
				this.upwardMove *= (float)num;
				this.positionChange *= (float)num;
				this.rotationChange *= (float)num;
			}
			this._endPosition = this._startPosition;
			this._endEulerAngles = this._startEulerAngles;
			if (this.orbitCenter != null)
			{
				this._orbitDistance = Vector3.Distance(this._startPosition, this.orbitCenter.position);
			}
			if (!this.isOrbiting && this.isLooping)
			{
				float num2 = 0f;
				if (this.forwardMove != 0f)
				{
					this._endPosition = this._startPosition + this._cachedTransform.forward.normalized * this.loopDistance;
					num2 = Mathf.Abs(this.loopDistance / this.forwardMove);
				}
				else if (this.positionChange != Vector3.zero)
				{
					this._endPosition = this._startPosition + this.positionChange.normalized * this.loopDistance;
					num2 = Mathf.Abs(this.loopDistance / this.positionChange.magnitude);
				}
				float num3 = 0f;
				if (this.rotationChange != Vector3.zero)
				{
					if (this.loopAngle != 0f)
					{
						this._endEulerAngles = this._startEulerAngles + this.rotationChange.normalized * this.loopAngle;
						num3 = Mathf.Abs(this.loopAngle / this.rotationChange.magnitude);
					}
					else if (num2 > 0f)
					{
						this._endEulerAngles = this._startEulerAngles + this.rotationChange * num2;
					}
				}
				if (num2 > 0f)
				{
					this._loopDuration = num2;
				}
				else
				{
					this._loopDuration = num3;
				}
				this._startTime = Time.time;
				this.isLooping = this._loopDuration > 0f;
			}
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0000E698 File Offset: 0x0000C898
		private void Update()
		{
			if (this.isOrbiting)
			{
				if (this.orbitCenter == null)
				{
					return;
				}
				Vector3 position = this.orbitCenter.position;
				Vector3 vector = this._cachedTransform.position + this._cachedTransform.right * this.orbitSpeed * Time.deltaTime;
				vector.y = this._startPosition.y;
				this._cachedTransform.position = vector;
				this._cachedTransform.LookAt(this.orbitCenter);
				vector = position - this._cachedTransform.forward * this._orbitDistance;
				vector.y = this._startPosition.y;
				this._cachedTransform.position = vector;
				Vector3 eulerAngles = this._cachedTransform.rotation.eulerAngles;
				eulerAngles.x = this._startEulerAngles.x;
				this._cachedTransform.rotation = Quaternion.Euler(eulerAngles);
				return;
			}
			else
			{
				Vector3 vector2;
				if (this.isLooping)
				{
					bool flag = this.loopChangeDirection && Time.time / this._loopDuration % 2f > 1f;
					float num = Time.time % this._loopDuration / this._loopDuration;
					Vector3 vector3;
					if (flag)
					{
						vector2 = Vector3.Lerp(this._endPosition, this._startPosition, num);
						vector3 = Vector3.Lerp(this._endEulerAngles, this._startEulerAngles, num);
					}
					else
					{
						vector2 = Vector3.Lerp(this._startPosition, this._endPosition, num);
						vector3 = Vector3.Lerp(this._startEulerAngles, this._endEulerAngles, num);
					}
					this._cachedTransform.position = vector2;
					this._cachedTransform.rotation = Quaternion.Euler(vector3);
					return;
				}
				vector2 = this._cachedTransform.position;
				if (this.forwardMove != 0f)
				{
					vector2 += this._cachedTransform.forward * this.forwardMove * Time.deltaTime;
				}
				if (this.upwardMove != 0f)
				{
					vector2 += this._cachedTransform.up * this.upwardMove * Time.deltaTime;
				}
				vector2 += this.positionChange * Time.deltaTime;
				RaycastHit raycastHit;
				if (this.isRayCasting && Physics.Raycast(vector2, Vector3.down, out raycastHit, this.rayCastMaxDistance, 1 << this.rayCastLayer))
				{
					vector2.y = raycastHit.point.y + this.rayCastHeight;
				}
				this._cachedTransform.position = vector2;
				Vector3 vector4 = this._cachedTransform.rotation.eulerAngles + this.rotationChange * Time.deltaTime;
				this._cachedTransform.rotation = Quaternion.Euler(vector4);
				return;
			}
		}

		// Token: 0x040001E1 RID: 481
		[Range(-100f, 100f)]
		public float forwardMove;

		// Token: 0x040001E2 RID: 482
		[Range(-100f, 100f)]
		public float upwardMove;

		// Token: 0x040001E3 RID: 483
		public Vector3 positionChange;

		// Token: 0x040001E4 RID: 484
		public Vector3 rotationChange;

		// Token: 0x040001E5 RID: 485
		public bool isRandomDirection;

		// Token: 0x040001E6 RID: 486
		public bool isLooping;

		// Token: 0x040001E7 RID: 487
		public float loopDistance;

		// Token: 0x040001E8 RID: 488
		public float loopAngle;

		// Token: 0x040001E9 RID: 489
		public bool loopChangeDirection;

		// Token: 0x040001EA RID: 490
		public bool isRayCasting;

		// Token: 0x040001EB RID: 491
		public float rayCastHeight = 15f;

		// Token: 0x040001EC RID: 492
		public int rayCastLayer = 6;

		// Token: 0x040001ED RID: 493
		public float rayCastMaxDistance = 200f;

		// Token: 0x040001EE RID: 494
		public bool isOrbiting;

		// Token: 0x040001EF RID: 495
		public Transform orbitCenter;

		// Token: 0x040001F0 RID: 496
		public float orbitSpeed = 1f;

		// Token: 0x040001F1 RID: 497
		private Transform _cachedTransform;

		// Token: 0x040001F2 RID: 498
		private Vector3 _startPosition;

		// Token: 0x040001F3 RID: 499
		private Vector3 _startEulerAngles;

		// Token: 0x040001F4 RID: 500
		private Vector3 _endPosition;

		// Token: 0x040001F5 RID: 501
		private Vector3 _endEulerAngles;

		// Token: 0x040001F6 RID: 502
		private float _orbitDistance;

		// Token: 0x040001F7 RID: 503
		private float _loopDuration;

		// Token: 0x040001F8 RID: 504
		private float _startTime;
	}
}
