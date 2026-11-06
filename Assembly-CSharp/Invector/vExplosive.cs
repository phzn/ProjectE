using System;
using System.Collections;
using System.Collections.Generic;
using Invector.vEventSystems;
using UnityEngine;
using UnityEngine.Events;

namespace Invector
{
	// Token: 0x020004E2 RID: 1250
	[vClassHeader("Explosive", true, "icon_v2", false, "", openClose = false)]
	public class vExplosive : vMonoBehaviour
	{
		// Token: 0x06001E1B RID: 7707 RVA: 0x000B7278 File Offset: 0x000B5478
		private void OnDrawGizmosSelected()
		{
			if (!this.showGizmos)
			{
				return;
			}
			Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
			Gizmos.DrawSphere(base.transform.position, this.minExplosionRadius);
			Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
			Gizmos.DrawSphere(base.transform.position, this.maxExplosionRadius);
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x000B72F6 File Offset: 0x000B54F6
		public void SetOverrideDamageSender(Transform target)
		{
			this.overrideDamageSender = target;
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x000B72FF File Offset: 0x000B54FF
		public void SetDamage(vDamage damage)
		{
			this.damage = damage;
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x000B7308 File Offset: 0x000B5508
		protected virtual void Start()
		{
			this.collidersReached = new List<GameObject>();
			if (this.method == vExplosive.ExplosiveMethod.timer)
			{
				base.StartCoroutine(this.StartTimer());
			}
		}

		// Token: 0x06001E1F RID: 7711 RVA: 0x000B732B File Offset: 0x000B552B
		protected virtual IEnumerator StartTimer()
		{
			if (!this.inTimer)
			{
				this.onInitTimer.Invoke();
				this.inTimer = true;
				float startTime = Time.time;
				float num = 0f;
				while (num < this.timeToExplode)
				{
					yield return new WaitForEndOfFrame();
					num = Time.time - startTime;
					this.onUpdateTimer.Invoke(this.normalizeTime ? (num / this.timeToExplode) : num);
				}
				if (base.gameObject)
				{
					this.onUpdateTimer.Invoke(this.normalizeTime ? 1f : this.timeToExplode);
					this.Explode();
				}
			}
			yield break;
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x000B733A File Offset: 0x000B553A
		protected virtual IEnumerator DestroyBomb()
		{
			yield return new WaitForSeconds(0.1f);
			Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x06001E21 RID: 7713 RVA: 0x000B7349 File Offset: 0x000B5549
		protected virtual void OnCollisionEnter(Collision collision)
		{
			if (this.method == vExplosive.ExplosiveMethod.collisionEnter)
			{
				this.Explode();
				return;
			}
			if (this.method == vExplosive.ExplosiveMethod.collisionEnterTimer)
			{
				base.StartCoroutine(this.StartTimer());
			}
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x000B7370 File Offset: 0x000B5570
		protected virtual void Explode()
		{
			this.onExplode.Invoke();
			Collider[] array = Physics.OverlapSphere(base.transform.position, this.maxExplosionRadius, this.applyDamageLayer);
			if (this.collidersReached == null)
			{
				this.collidersReached = new List<GameObject>();
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null && array[i].gameObject != null && !this.collidersReached.Contains(array[i].gameObject))
				{
					this.collidersReached.Add(array[i].gameObject);
					vDamage vDamage = new vDamage(this.damage);
					vDamage.sender = (this.overrideDamageSender ? this.overrideDamageSender : base.transform);
					vDamage.hitPosition = array[i].ClosestPointOnBounds(base.transform.position);
					vDamage.receiver = array[i].transform;
					float num = Vector3.Distance(base.transform.position, vDamage.hitPosition);
					float num2 = ((num <= this.minExplosionRadius) ? (this.damage.damageValue * this.damageOnMinRangeMultiplier) : Mathf.Lerp(this.damage.damageValue * this.damageOnMaxRangeMultiplier, this.damage.damageValue * this.damageOnMinRangeMultiplier, this.EvaluateDistance(num)));
					vDamage.activeRagdoll = num <= this.maxExplosionRadius * 0.5f && vDamage.activeRagdoll;
					vDamage.damageValue = (float)((int)num2);
					this.onHit.Invoke(array[i]);
					array[i].gameObject.ApplyDamage(vDamage, null);
				}
			}
			base.StartCoroutine(this.ApplyExplosionForce());
			if (this.destroyAfterExplode)
			{
				base.StartCoroutine(this.DestroyBomb());
			}
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x000B753E File Offset: 0x000B573E
		protected virtual IEnumerator ApplyExplosionForce()
		{
			yield return new WaitForSeconds(0.1f);
			Collider[] array = Physics.OverlapSphere(base.transform.position, this.maxExplosionRadius, this.applyForceLayer);
			for (int i = 0; i < array.Length; i++)
			{
				Rigidbody component = array[i].GetComponent<Rigidbody>();
				float num = Vector3.Distance(base.transform.position, array[i].ClosestPointOnBounds(base.transform.position));
				float num2 = ((num <= this.minExplosionRadius) ? this.explosionForce : this.GetPercentageForce(num, this.explosionForce));
				if (component)
				{
					component.AddExplosionForce(num2, base.transform.position, this.maxExplosionRadius, this.upwardsModifier, this.forceMode);
				}
			}
			yield break;
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x000B7550 File Offset: 0x000B5750
		protected float EvaluateDistance(float distance)
		{
			if (distance > this.maxExplosionRadius)
			{
				distance = this.maxExplosionRadius;
			}
			float num = this.maxExplosionRadius - this.minExplosionRadius;
			float num2 = Mathf.Clamp(distance - this.minExplosionRadius, 0f, num);
			return Mathf.Clamp(num - num2, 0f, num) / num * 100f * 0.01f;
		}

		// Token: 0x06001E25 RID: 7717 RVA: 0x000B75AC File Offset: 0x000B57AC
		protected float GetPercentageForce(float distance, float value)
		{
			return value * this.EvaluateDistance(distance);
		}

		// Token: 0x06001E26 RID: 7718 RVA: 0x000B75B7 File Offset: 0x000B57B7
		public virtual void SetCollisionEnterMethod()
		{
			this.method = vExplosive.ExplosiveMethod.collisionEnter;
		}

		// Token: 0x06001E27 RID: 7719 RVA: 0x000B75C0 File Offset: 0x000B57C0
		public virtual void SetCollisionEnterTimerMethod(int timer)
		{
			this.method = vExplosive.ExplosiveMethod.collisionEnterTimer;
			this.timeToExplode = (float)timer;
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x000B75D1 File Offset: 0x000B57D1
		public virtual void SetRemoveMethod()
		{
			this.method = vExplosive.ExplosiveMethod.remote;
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x000B75DA File Offset: 0x000B57DA
		public virtual void SetRemoveTimerMethod(int timer)
		{
			this.method = vExplosive.ExplosiveMethod.remoteTimer;
			this.timeToExplode = (float)timer;
		}

		// Token: 0x06001E2A RID: 7722 RVA: 0x000B75EB File Offset: 0x000B57EB
		public virtual void SetTimerMethod(int timer)
		{
			this.method = vExplosive.ExplosiveMethod.timer;
			this.timeToExplode = (float)timer;
		}

		// Token: 0x06001E2B RID: 7723 RVA: 0x000B75FC File Offset: 0x000B57FC
		public virtual void ActiveExplosion()
		{
			if (this.method == vExplosive.ExplosiveMethod.remote)
			{
				this.Explode();
				return;
			}
			if (this.method == vExplosive.ExplosiveMethod.remoteTimer)
			{
				base.StartCoroutine(this.StartTimer());
			}
		}

		// Token: 0x06001E2C RID: 7724 RVA: 0x000B7624 File Offset: 0x000B5824
		public void RemoveParent()
		{
			base.transform.parent = null;
		}

		// Token: 0x06001E2D RID: 7725 RVA: 0x000B7632 File Offset: 0x000B5832
		public void RemoveParentOfOther(Transform other)
		{
			other.parent = null;
		}

		// Token: 0x04001F1B RID: 7963
		public vDamage damage;

		// Token: 0x04001F1C RID: 7964
		[vHelpBox("Use this values To define the min and max damage based on range", vHelpBoxAttribute.MessageType.None)]
		[Range(0f, 1f)]
		public float damageOnMinRangeMultiplier = 1f;

		// Token: 0x04001F1D RID: 7965
		[Range(0f, 1f)]
		public float damageOnMaxRangeMultiplier;

		// Token: 0x04001F1E RID: 7966
		[Tooltip("Assign this to set other damage sender")]
		public Transform overrideDamageSender;

		// Token: 0x04001F1F RID: 7967
		public float explosionForce;

		// Token: 0x04001F20 RID: 7968
		public float minExplosionRadius;

		// Token: 0x04001F21 RID: 7969
		public float maxExplosionRadius;

		// Token: 0x04001F22 RID: 7970
		public float upwardsModifier = 1f;

		// Token: 0x04001F23 RID: 7971
		public ForceMode forceMode;

		// Token: 0x04001F24 RID: 7972
		public vExplosive.ExplosiveMethod method;

		// Token: 0x04001F25 RID: 7973
		public LayerMask applyDamageLayer;

		// Token: 0x04001F26 RID: 7974
		public LayerMask applyForceLayer;

		// Token: 0x04001F27 RID: 7975
		public float timeToExplode = 10f;

		// Token: 0x04001F28 RID: 7976
		public bool destroyAfterExplode = true;

		// Token: 0x04001F29 RID: 7977
		[Tooltip("convert to progress 0 to 1")]
		public bool normalizeTime;

		// Token: 0x04001F2A RID: 7978
		public bool showGizmos;

		// Token: 0x04001F2B RID: 7979
		public UnityEvent onInitTimer;

		// Token: 0x04001F2C RID: 7980
		public vExplosive.OnUpdateTime onUpdateTimer;

		// Token: 0x04001F2D RID: 7981
		public UnityEvent onExplode;

		// Token: 0x04001F2E RID: 7982
		public vExplosive.ColliderEvent onHit;

		// Token: 0x04001F2F RID: 7983
		private bool inTimer;

		// Token: 0x04001F30 RID: 7984
		protected List<GameObject> collidersReached;

		// Token: 0x020004E3 RID: 1251
		[Serializable]
		public class OnUpdateTime : UnityEvent<float>
		{
		}

		// Token: 0x020004E4 RID: 1252
		[Serializable]
		public class ColliderEvent : UnityEvent<Collider>
		{
		}

		// Token: 0x020004E5 RID: 1253
		public enum ExplosiveMethod
		{
			// Token: 0x04001F32 RID: 7986
			collisionEnter,
			// Token: 0x04001F33 RID: 7987
			collisionEnterTimer,
			// Token: 0x04001F34 RID: 7988
			remote,
			// Token: 0x04001F35 RID: 7989
			timer,
			// Token: 0x04001F36 RID: 7990
			remoteTimer
		}
	}
}
