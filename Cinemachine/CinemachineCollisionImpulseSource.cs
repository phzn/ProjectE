using System;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000052 RID: 82
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[SaveDuringPlay]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachineCollisionImpulseSource.html")]
	public class CinemachineCollisionImpulseSource : CinemachineImpulseSource
	{
		// Token: 0x06000379 RID: 889 RVA: 0x0001581F File Offset: 0x00013A1F
		private void Start()
		{
			this.mRigidBody = base.GetComponent<Rigidbody>();
			this.mRigidBody2D = base.GetComponent<Rigidbody2D>();
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00015839 File Offset: 0x00013A39
		private void OnEnable()
		{
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0001583B File Offset: 0x00013A3B
		private void OnCollisionEnter(Collision c)
		{
			this.GenerateImpactEvent(c.collider, c.relativeVelocity);
		}

		// Token: 0x0600037C RID: 892 RVA: 0x0001584F File Offset: 0x00013A4F
		private void OnTriggerEnter(Collider c)
		{
			this.GenerateImpactEvent(c, Vector3.zero);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x00015860 File Offset: 0x00013A60
		private float GetMassAndVelocity(Collider other, ref Vector3 vel)
		{
			bool flag = vel == Vector3.zero;
			float num = 1f;
			if (this.m_ScaleImpactWithMass || this.m_ScaleImpactWithSpeed || this.m_UseImpactDirection)
			{
				if (this.mRigidBody != null)
				{
					if (this.m_ScaleImpactWithMass)
					{
						num *= this.mRigidBody.mass;
					}
					if (flag)
					{
						vel = -this.mRigidBody.velocity;
					}
				}
				Rigidbody rigidbody = ((other != null) ? other.attachedRigidbody : null);
				if (rigidbody != null)
				{
					if (this.m_ScaleImpactWithMass)
					{
						num *= rigidbody.mass;
					}
					if (flag)
					{
						vel += rigidbody.velocity;
					}
				}
			}
			return num;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00015924 File Offset: 0x00013B24
		private void GenerateImpactEvent(Collider other, Vector3 vel)
		{
			if (!base.enabled)
			{
				return;
			}
			if (other != null)
			{
				int layer = other.gameObject.layer;
				if (((1 << layer) & this.m_LayerMask) == 0)
				{
					return;
				}
				if (this.m_IgnoreTag.Length != 0 && other.CompareTag(this.m_IgnoreTag))
				{
					return;
				}
			}
			float num = this.GetMassAndVelocity(other, ref vel);
			if (this.m_ScaleImpactWithSpeed)
			{
				num *= Mathf.Sqrt(vel.magnitude);
			}
			Vector3 vector = this.m_DefaultVelocity;
			if (this.m_UseImpactDirection && !vel.AlmostZero())
			{
				vector = -vel.normalized * vector.magnitude;
			}
			base.GenerateImpulseWithVelocity(vector * num);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x000159DE File Offset: 0x00013BDE
		private void OnCollisionEnter2D(Collision2D c)
		{
			this.GenerateImpactEvent2D(c.collider, c.relativeVelocity);
		}

		// Token: 0x06000380 RID: 896 RVA: 0x000159F7 File Offset: 0x00013BF7
		private void OnTriggerEnter2D(Collider2D c)
		{
			this.GenerateImpactEvent2D(c, Vector3.zero);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00015A08 File Offset: 0x00013C08
		private float GetMassAndVelocity2D(Collider2D other2d, ref Vector3 vel)
		{
			bool flag = vel == Vector3.zero;
			float num = 1f;
			if (this.m_ScaleImpactWithMass || this.m_ScaleImpactWithSpeed || this.m_UseImpactDirection)
			{
				if (this.mRigidBody2D != null)
				{
					if (this.m_ScaleImpactWithMass)
					{
						num *= this.mRigidBody2D.mass;
					}
					if (flag)
					{
						vel = -this.mRigidBody2D.velocity;
					}
				}
				Rigidbody2D rigidbody2D = ((other2d != null) ? other2d.attachedRigidbody : null);
				if (rigidbody2D != null)
				{
					if (this.m_ScaleImpactWithMass)
					{
						num *= rigidbody2D.mass;
					}
					if (flag)
					{
						Vector3 vector = rigidbody2D.velocity;
						vel += vector;
					}
				}
			}
			return num;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x00015AD8 File Offset: 0x00013CD8
		private void GenerateImpactEvent2D(Collider2D other2d, Vector3 vel)
		{
			if (!base.enabled)
			{
				return;
			}
			if (other2d != null)
			{
				int layer = other2d.gameObject.layer;
				if (((1 << layer) & this.m_LayerMask) == 0)
				{
					return;
				}
				if (this.m_IgnoreTag.Length != 0 && other2d.CompareTag(this.m_IgnoreTag))
				{
					return;
				}
			}
			float num = this.GetMassAndVelocity2D(other2d, ref vel);
			if (this.m_ScaleImpactWithSpeed)
			{
				num *= Mathf.Sqrt(vel.magnitude);
			}
			Vector3 vector = this.m_DefaultVelocity;
			if (this.m_UseImpactDirection && !vel.AlmostZero())
			{
				vector = -vel.normalized * vector.magnitude;
			}
			base.GenerateImpulseWithVelocity(vector * num);
		}

		// Token: 0x04000253 RID: 595
		[Header("Trigger Object Filter")]
		[Tooltip("Only collisions with objects on these layers will generate Impulse events")]
		public LayerMask m_LayerMask = 1;

		// Token: 0x04000254 RID: 596
		[TagField]
		[Tooltip("No Impulse evemts will be generated for collisions with objects having these tags")]
		public string m_IgnoreTag = string.Empty;

		// Token: 0x04000255 RID: 597
		[Header("How To Generate The Impulse")]
		[Tooltip("If checked, signal direction will be affected by the direction of impact")]
		public bool m_UseImpactDirection;

		// Token: 0x04000256 RID: 598
		[Tooltip("If checked, signal amplitude will be multiplied by the mass of the impacting object")]
		public bool m_ScaleImpactWithMass;

		// Token: 0x04000257 RID: 599
		[Tooltip("If checked, signal amplitude will be multiplied by the speed of the impacting object")]
		public bool m_ScaleImpactWithSpeed;

		// Token: 0x04000258 RID: 600
		private Rigidbody mRigidBody;

		// Token: 0x04000259 RID: 601
		private Rigidbody2D mRigidBody2D;
	}
}
