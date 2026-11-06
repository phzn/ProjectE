using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x0200003F RID: 63
	public class GPUICameraRig : MonoBehaviour
	{
		// Token: 0x0600025A RID: 602 RVA: 0x0000CC98 File Offset: 0x0000AE98
		private void Start()
		{
			if (this.m_Target == null)
			{
				return;
			}
			this.targetRigidbody = this.m_Target.GetComponent<Rigidbody>();
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000CCBA File Offset: 0x0000AEBA
		private void FixedUpdate()
		{
			this.FollowTarget(Time.deltaTime);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000CCC8 File Offset: 0x0000AEC8
		private void FollowTarget(float deltaTime)
		{
			if (deltaTime <= 0f || this.m_Target == null)
			{
				return;
			}
			Vector3 vector = this.m_Target.forward;
			Vector3 vector2 = this.m_Target.up;
			if (this.m_FollowVelocity && Application.isPlaying)
			{
				Vector3 velocity = this.targetRigidbody.velocity;
				if (velocity.magnitude > this.m_TargetVelocityLowerLimit)
				{
					vector = velocity.normalized;
					vector2 = Vector3.up;
				}
				else
				{
					vector2 = Vector3.up;
				}
				this.m_CurrentTurnAmount = Mathf.SmoothDamp(this.m_CurrentTurnAmount, 1f, ref this.m_TurnSpeedVelocityChange, this.m_SmoothTurnTime);
			}
			else
			{
				float num = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
				if (this.m_SpinTurnLimit > 0f)
				{
					float num2 = Mathf.Abs(Mathf.DeltaAngle(this.m_LastFlatAngle, num)) / deltaTime;
					float num3 = Mathf.InverseLerp(this.m_SpinTurnLimit, this.m_SpinTurnLimit * 0.75f, num2);
					float num4 = ((this.m_CurrentTurnAmount > num3) ? 0.1f : 1f);
					if (Application.isPlaying)
					{
						this.m_CurrentTurnAmount = Mathf.SmoothDamp(this.m_CurrentTurnAmount, num3, ref this.m_TurnSpeedVelocityChange, num4);
					}
					else
					{
						this.m_CurrentTurnAmount = num3;
					}
				}
				else
				{
					this.m_CurrentTurnAmount = 1f;
				}
				this.m_LastFlatAngle = num;
			}
			base.transform.position = Vector3.Lerp(base.transform.position, this.m_Target.position, deltaTime * this.m_MoveSpeed);
			if (!this.m_FollowTilt)
			{
				vector.y = 0f;
				if (vector.sqrMagnitude < 1E-45f)
				{
					vector = base.transform.forward;
				}
			}
			Quaternion quaternion = Quaternion.LookRotation(vector, this.m_RollUp);
			this.m_RollUp = ((this.m_RollSpeed > 0f) ? Vector3.Slerp(this.m_RollUp, vector2, this.m_RollSpeed * deltaTime) : Vector3.up);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, quaternion, this.m_TurnSpeed * this.m_CurrentTurnAmount * deltaTime);
		}

		// Token: 0x04000170 RID: 368
		public Transform m_Target;

		// Token: 0x04000171 RID: 369
		public float m_MoveSpeed = 3f;

		// Token: 0x04000172 RID: 370
		public float m_TurnSpeed = 1f;

		// Token: 0x04000173 RID: 371
		public float m_RollSpeed = 0.2f;

		// Token: 0x04000174 RID: 372
		public bool m_FollowVelocity;

		// Token: 0x04000175 RID: 373
		public bool m_FollowTilt = true;

		// Token: 0x04000176 RID: 374
		public float m_SpinTurnLimit = 90f;

		// Token: 0x04000177 RID: 375
		public float m_TargetVelocityLowerLimit = 4f;

		// Token: 0x04000178 RID: 376
		public float m_SmoothTurnTime = 0.2f;

		// Token: 0x04000179 RID: 377
		private Vector3 m_LastTargetPosition;

		// Token: 0x0400017A RID: 378
		private Rigidbody targetRigidbody;

		// Token: 0x0400017B RID: 379
		private float m_LastFlatAngle;

		// Token: 0x0400017C RID: 380
		private float m_CurrentTurnAmount;

		// Token: 0x0400017D RID: 381
		private float m_TurnSpeedVelocityChange;

		// Token: 0x0400017E RID: 382
		private Vector3 m_RollUp = Vector3.up;
	}
}
