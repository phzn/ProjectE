using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Invector
{
	// Token: 0x020004B7 RID: 1207
	public class vSpawnOnParticleCollision : MonoBehaviour
	{
		// Token: 0x06001D5A RID: 7514 RVA: 0x000B446F File Offset: 0x000B266F
		public void SetOverrideDamageSender(Transform sender)
		{
			this.damageSender = sender;
		}

		// Token: 0x06001D5B RID: 7515 RVA: 0x000B4478 File Offset: 0x000B2678
		protected virtual void OnParticleCollision(GameObject hit)
		{
			this.part.GetCollisionEvents(hit, this.collisionEvents);
			Vector3 intersection = this.collisionEvents[0].intersection;
			Vector3 normal = this.collisionEvents[0].normal;
			GameObject gameObject = Object.Instantiate<GameObject>(this.prefab, intersection, Quaternion.LookRotation(normal), vObjectContainer.root);
			vObjectDamage vObjectDamage;
			if (this.damageSender && gameObject.TryGetComponent<vObjectDamage>(out vObjectDamage))
			{
				vObjectDamage.SetOverrideDamageSender(this.damageSender);
				if (this.overrideDamage)
				{
					vObjectDamage.damage.damageValue = 0f;
					vObjectDamage.onSendDamage.AddListener(new UnityAction<vDamage>(this.OnSendDamage));
				}
			}
		}

		// Token: 0x06001D5C RID: 7516 RVA: 0x000B4530 File Offset: 0x000B2730
		protected virtual void OnSendDamage(vDamage damage)
		{
			if (!this.targetStorage.ContainsKey(damage.receiver.gameObject))
			{
				this.targetStorage.Add(damage.receiver.gameObject, new vSpawnOnParticleCollision.DamageHandle());
			}
			vSpawnOnParticleCollision.DamageHandle damageHandle;
			if (this.targetStorage.TryGetValue(damage.receiver.gameObject, out damageHandle))
			{
				damageHandle.damageFrequeny = this.damageFrequeny;
				if (damageHandle.canApplyDamage)
				{
					damage.damageValue = this.damage.damageValue;
					damage.damageType = this.damage.damageType;
					damage.reaction_id = this.damage.reaction_id;
					damage.recoil_id = this.damage.recoil_id;
					damageHandle.ApplyDamage(damage);
				}
			}
		}

		// Token: 0x04001E68 RID: 7784
		[vHelpBox("This component is used to spawn objects when particle collide with something\n", vHelpBoxAttribute.MessageType.None)]
		public ParticleSystem part;

		// Token: 0x04001E69 RID: 7785
		public GameObject prefab;

		// Token: 0x04001E6A RID: 7786
		public List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

		// Token: 0x04001E6B RID: 7787
		private Transform damageSender;

		// Token: 0x04001E6C RID: 7788
		[vHelpBox("When the prefab is a ObjectDamage you can use this properties to override the damage frequency and the damage value", vHelpBoxAttribute.MessageType.None)]
		public bool overrideDamage;

		// Token: 0x04001E6D RID: 7789
		public float damageFrequeny;

		// Token: 0x04001E6E RID: 7790
		public vDamage damage;

		// Token: 0x04001E6F RID: 7791
		protected Dictionary<GameObject, vSpawnOnParticleCollision.DamageHandle> targetStorage = new Dictionary<GameObject, vSpawnOnParticleCollision.DamageHandle>();

		// Token: 0x020004B8 RID: 1208
		protected class DamageHandle
		{
			// Token: 0x17000553 RID: 1363
			// (get) Token: 0x06001D5E RID: 7518 RVA: 0x000B4607 File Offset: 0x000B2807
			public bool canApplyDamage
			{
				get
				{
					return this.lastDamageTime < Time.time;
				}
			}

			// Token: 0x06001D5F RID: 7519 RVA: 0x000B4616 File Offset: 0x000B2816
			public void ApplyDamage(vDamage damage)
			{
				if (this.canApplyDamage)
				{
					this.lastDamageTime = Time.time + this.damageFrequeny;
					damage.receiver.gameObject.ApplyDamage(damage);
				}
			}

			// Token: 0x04001E70 RID: 7792
			public Transform target;

			// Token: 0x04001E71 RID: 7793
			public float damageFrequeny;

			// Token: 0x04001E72 RID: 7794
			private float lastDamageTime;
		}
	}
}
