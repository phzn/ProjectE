using System;
using System.Collections.Generic;
using Invector.vEventSystems;
using UnityEngine;
using UnityEngine.Events;

namespace Invector
{
	// Token: 0x020004BF RID: 1215
	[vClassHeader("OBJECT DAMAGE", true, "icon_v2", false, "", iconName = "DamageIcon")]
	public class vObjectDamage : vMonoBehaviour
	{
		// Token: 0x06001D73 RID: 7539 RVA: 0x000B50B0 File Offset: 0x000B32B0
		public void SetOverrideDamageSender(Transform target)
		{
			this.overrideDamageSender = target;
		}

		// Token: 0x06001D74 RID: 7540 RVA: 0x000B50B9 File Offset: 0x000B32B9
		protected virtual void Start()
		{
			this.targets = new List<Collider>();
			this.disabledTarget = new List<Collider>();
			if (this.collisionMethod == vObjectDamage.CollisionMethod.OnParticleCollision)
			{
				this.part = base.GetComponent<ParticleSystem>();
				this.collisionEvents = new List<ParticleCollisionEvent>();
			}
		}

		// Token: 0x06001D75 RID: 7541 RVA: 0x000B50F4 File Offset: 0x000B32F4
		protected virtual void Update()
		{
			if (!base.enabled)
			{
				return;
			}
			if (this.continuousDamage && this.targets != null && this.targets.Count > 0)
			{
				if (this.currentTime > 0f)
				{
					this.currentTime -= Time.deltaTime;
					return;
				}
				this.currentTime = this.damageFrequency;
				foreach (Collider collider in this.targets)
				{
					if (collider != null)
					{
						if (collider.enabled)
						{
							this.ApplyDamage(collider, base.transform.position);
						}
						else
						{
							this.disabledTarget.Add(collider);
						}
					}
				}
				if (this.disabledTarget.Count > 0)
				{
					int num = this.disabledTarget.Count;
					while (num >= 0 && this.disabledTarget.Count != 0)
					{
						try
						{
							if (this.targets.Contains(this.disabledTarget[num]))
							{
								this.targets.Remove(this.disabledTarget[num]);
							}
						}
						catch
						{
							break;
						}
						num--;
					}
				}
				if (this.disabledTarget.Count > 0)
				{
					this.disabledTarget.Clear();
				}
			}
		}

		// Token: 0x06001D76 RID: 7542 RVA: 0x000B525C File Offset: 0x000B345C
		protected virtual void OnCollisionEnter(Collision hit)
		{
			if (!base.enabled)
			{
				return;
			}
			if (this.collisionMethod != vObjectDamage.CollisionMethod.OnColliderEnter || this.continuousDamage)
			{
				return;
			}
			if (this.CanApplyDamage(hit.gameObject))
			{
				this.ApplyDamage(hit.collider, hit.contacts[0].point);
			}
		}

		// Token: 0x06001D77 RID: 7543 RVA: 0x000B52B0 File Offset: 0x000B34B0
		protected virtual void OnTriggerEnter(Collider hit)
		{
			if (!base.enabled)
			{
				return;
			}
			if (this.collisionMethod != vObjectDamage.CollisionMethod.OnTriggerEnter)
			{
				return;
			}
			if (this.continuousDamage && this.CanApplyDamage(hit.gameObject) && !this.targets.Contains(hit))
			{
				this.targets.Add(hit);
				return;
			}
			if (this.CanApplyDamage(hit.gameObject))
			{
				this.ApplyDamage(hit, base.transform.position);
			}
		}

		// Token: 0x06001D78 RID: 7544 RVA: 0x000B5320 File Offset: 0x000B3520
		protected virtual bool CanApplyDamage(GameObject hitObject)
		{
			return ((this.tags.Count == 0 || this.tags.Contains(hitObject.tag)) && this.layerToCollide == 0) || this.layerToCollide.ContainsLayer(hitObject.layer);
		}

		// Token: 0x06001D79 RID: 7545 RVA: 0x000B5370 File Offset: 0x000B3570
		protected virtual void OnTriggerExit(Collider hit)
		{
			if (!base.enabled)
			{
				return;
			}
			if (this.collisionMethod == vObjectDamage.CollisionMethod.OnColliderEnter && !this.continuousDamage)
			{
				return;
			}
			if (this.CanApplyDamage(hit.gameObject) && this.targets.Contains(hit))
			{
				this.targets.Remove(hit);
			}
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x000B53C4 File Offset: 0x000B35C4
		protected virtual void OnParticleCollision(GameObject hit)
		{
			if (!base.enabled)
			{
				return;
			}
			if (this.CanApplyDamage(hit))
			{
				if (this.collisionMethod != vObjectDamage.CollisionMethod.OnParticleCollision)
				{
					return;
				}
				if (this.particleCollisionTime > Time.time)
				{
					return;
				}
				if (this.continuousDamage)
				{
					this.particleCollisionTime = Time.time + this.damageFrequency;
				}
				int num = this.part.GetCollisionEvents(hit, this.collisionEvents);
				int i = 0;
				Collider collider2;
				if (this.continuousDamage)
				{
					Collider collider;
					if (hit.TryGetComponent<Collider>(out collider))
					{
						while (i < num)
						{
							this.ApplyDamage(collider, this.collisionEvents[0].intersection);
							Debug.Log(collider);
							i++;
						}
						return;
					}
				}
				else if (hit.TryGetComponent<Collider>(out collider2))
				{
					this.ApplyDamage(collider2, this.collisionEvents[0].intersection);
				}
			}
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x000B5491 File Offset: 0x000B3691
		public virtual void ClearTargets()
		{
			this.targets.Clear();
		}

		// Token: 0x06001D7C RID: 7548 RVA: 0x000B54A0 File Offset: 0x000B36A0
		protected virtual void ApplyDamage(Collider target, Vector3 hitPoint)
		{
			this.damage.hitReaction = true;
			this.damage.sender = (this.overrideDamageSender ? this.overrideDamageSender : base.transform);
			this.damage.hitPosition = hitPoint;
			this.damage.receiver = target.transform;
			vDamage vDamage = new vDamage(this.damage);
			target.gameObject.ApplyDamage(vDamage, null);
			this.onSendDamage.Invoke(vDamage);
			this.onHit.Invoke(target);
		}

		// Token: 0x04001E97 RID: 7831
		public vDamage damage;

		// Token: 0x04001E98 RID: 7832
		[Tooltip("Assign this to set other damage sender")]
		public Transform overrideDamageSender;

		// Token: 0x04001E99 RID: 7833
		[Tooltip("List of layers that can be hit, nothing will apply to all layers")]
		public LayerMask layerToCollide;

		// Token: 0x04001E9A RID: 7834
		[Tooltip("List of tags that can be hit, nothing will apply to all tags")]
		public vTagMask tags;

		// Token: 0x04001E9B RID: 7835
		[Tooltip("Check to use the damage Frequency")]
		public bool continuousDamage;

		// Token: 0x04001E9C RID: 7836
		[Tooltip("Apply damage to each end of the frequency in seconds ")]
		public float damageFrequency = 0.5f;

		// Token: 0x04001E9D RID: 7837
		protected List<Collider> targets;

		// Token: 0x04001E9E RID: 7838
		protected List<Collider> disabledTarget;

		// Token: 0x04001E9F RID: 7839
		protected float currentTime;

		// Token: 0x04001EA0 RID: 7840
		public vObjectDamage.OnHitEvent onHit;

		// Token: 0x04001EA1 RID: 7841
		public vObjectDamage.OnSendEvent onSendDamage;

		// Token: 0x04001EA2 RID: 7842
		protected float particleCollisionTime;

		// Token: 0x04001EA3 RID: 7843
		public vObjectDamage.CollisionMethod collisionMethod;

		// Token: 0x04001EA4 RID: 7844
		public ParticleSystem part;

		// Token: 0x04001EA5 RID: 7845
		public bool limitParticleCollisionEvent;

		// Token: 0x04001EA6 RID: 7846
		public int maxParticleCollisionEvent = 1;

		// Token: 0x04001EA7 RID: 7847
		public List<ParticleCollisionEvent> collisionEvents;

		// Token: 0x020004C0 RID: 1216
		[Serializable]
		public class OnHitEvent : UnityEvent<Collider>
		{
		}

		// Token: 0x020004C1 RID: 1217
		[Serializable]
		public class OnSendEvent : UnityEvent<vDamage>
		{
		}

		// Token: 0x020004C2 RID: 1218
		public enum CollisionMethod
		{
			// Token: 0x04001EA9 RID: 7849
			OnTriggerEnter,
			// Token: 0x04001EAA RID: 7850
			OnColliderEnter,
			// Token: 0x04001EAB RID: 7851
			OnParticleCollision
		}
	}
}
