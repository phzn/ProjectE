using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Invector
{
	// Token: 0x020004B1 RID: 1201
	[vClassHeader("Rotate To Target", true, "icon_v2", false, "")]
	public class vRotateToTarget : vMonoBehaviour
	{
		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001D3E RID: 7486 RVA: 0x000B3AEF File Offset: 0x000B1CEF
		// (set) Token: 0x06001D3D RID: 7485 RVA: 0x000B3AE6 File Offset: 0x000B1CE6
		public bool targetIsInAngleRange { get; protected set; }

		// Token: 0x06001D3F RID: 7487 RVA: 0x000B3AF7 File Offset: 0x000B1CF7
		protected virtual void Start()
		{
			if (this.angleRootH == null)
			{
				this.angleRootH = base.transform;
			}
			if (this.angleRootV == null)
			{
				this.angleRootV = base.transform;
			}
		}

		// Token: 0x06001D40 RID: 7488 RVA: 0x000B3B30 File Offset: 0x000B1D30
		protected virtual void Update()
		{
			if (!this.angleRootH || !this.rotatorV)
			{
				return;
			}
			Transform transform = this.targetTransform;
			if (this.rotatorV)
			{
				this.angleV = this.rotatorV.localEulerAngles.x;
			}
			if (this.rotatorH)
			{
				this.angleH = this.rotatorH.localEulerAngles.y;
			}
			if (transform)
			{
				Vector3 vector = transform.position - this.angleRootV.position;
				Vector3 vector2 = transform.position - this.angleRootH.position;
				float x = this.angleRootV.forward.AngleFormOtherDirection(vector.normalized).x;
				float y = this.angleRootH.forward.AngleFormOtherDirection(vector2.normalized).y;
				bool flag = Mathf.Abs(x) <= this.maxAngleVertical && Mathf.Abs(y) <= this.maxAngleHorizontal;
				if (flag != this.targetIsInAngleRange)
				{
					if (flag)
					{
						this.onEnterAngle.Invoke();
					}
					else
					{
						this.onExitAngle.Invoke();
					}
					this.targetIsInAngleRange = flag;
				}
				if (this.targetIsInAngleRange)
				{
					this.onStayAngle.Invoke();
					this.angleV = Mathf.LerpAngle(this.angleV, x, this.rotationSpeedInAngle * Time.deltaTime);
					this.angleH = Mathf.LerpAngle(this.angleH, y, this.rotationSpeedInAngle * Time.deltaTime);
				}
				else
				{
					this.angleV = Mathf.LerpAngle(this.angleV, 0f, this.rotationSpeedOutAngle * Time.deltaTime);
					this.angleH = Mathf.LerpAngle(this.angleH, 0f, this.rotationSpeedOutAngle * Time.deltaTime);
				}
			}
			else
			{
				if (this.targetIsInAngleRange)
				{
					this.onExitAngle.Invoke();
					this.targetIsInAngleRange = false;
				}
				if (this.rotatorV.localEulerAngles.magnitude > 0f)
				{
					this.angleV = Mathf.LerpAngle(this.angleV, 0f, this.rotationSpeedOutAngle * Time.deltaTime);
					this.angleH = Mathf.LerpAngle(this.angleH, 0f, this.rotationSpeedOutAngle * Time.deltaTime);
				}
			}
			if (this.rotateV && this.rotateV)
			{
				Vector3 localEulerAngles = this.rotatorV.localEulerAngles;
				localEulerAngles.x = this.angleV;
				this.rotatorV.localEulerAngles = localEulerAngles;
			}
			if (this.rotateH && this.rotatorH)
			{
				Vector3 localEulerAngles2 = this.rotatorH.localEulerAngles;
				localEulerAngles2.y = this.angleH;
				this.rotatorH.localEulerAngles = localEulerAngles2;
			}
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x000B3DF3 File Offset: 0x000B1FF3
		public void SetTarget(Transform target)
		{
			this.targetTransform = target;
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x000B3DFC File Offset: 0x000B1FFC
		public void ClearTarget()
		{
			this.targetTransform = null;
		}

		// Token: 0x04001E40 RID: 7744
		public Transform targetTransform;

		// Token: 0x04001E41 RID: 7745
		[FormerlySerializedAs("angleRoot")]
		public Transform angleRootH;

		// Token: 0x04001E42 RID: 7746
		public Transform rotatorH;

		// Token: 0x04001E43 RID: 7747
		public Transform angleRootV;

		// Token: 0x04001E44 RID: 7748
		[FormerlySerializedAs("rotator")]
		public Transform rotatorV;

		// Token: 0x04001E45 RID: 7749
		public bool rotateH;

		// Token: 0x04001E46 RID: 7750
		public bool rotateV;

		// Token: 0x04001E47 RID: 7751
		public float rotationSpeedInAngle;

		// Token: 0x04001E48 RID: 7752
		public float rotationSpeedOutAngle;

		// Token: 0x04001E49 RID: 7753
		[Range(0f, 180f)]
		public float maxAngleVertical = 60f;

		// Token: 0x04001E4A RID: 7754
		[Range(0f, 180f)]
		public float maxAngleHorizontal = 60f;

		// Token: 0x04001E4B RID: 7755
		[Range(0f, 180f)]
		public float angleToReachTarget = 45f;

		// Token: 0x04001E4C RID: 7756
		public UnityEvent onEnterAngle;

		// Token: 0x04001E4D RID: 7757
		public UnityEvent onStayAngle;

		// Token: 0x04001E4E RID: 7758
		public UnityEvent onExitAngle;

		// Token: 0x04001E50 RID: 7760
		protected float angleH;

		// Token: 0x04001E51 RID: 7761
		protected float angleV;
	}
}
