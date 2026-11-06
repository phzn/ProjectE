using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Invector
{
	// Token: 0x0200049A RID: 1178
	[vClassHeader("HITDAMAGE PARTICLE", "Default hit Particle to instantiate every time you receive damage and Custom hit Particle to instantiate based on a custom DamageType that comes from the MeleeControl Behaviour (AnimatorController)")]
	public class vHitDamageParticle : vMonoBehaviour
	{
		// Token: 0x06001CEE RID: 7406 RVA: 0x000B2C57 File Offset: 0x000B0E57
		private IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();
			vIHealthController vIHealthController;
			if (base.TryGetComponent<vIHealthController>(out vIHealthController))
			{
				vIHealthController.onReceiveDamage.AddListener(new UnityAction<vDamage>(this.OnReceiveDamage));
			}
			yield break;
		}

		// Token: 0x06001CEF RID: 7407 RVA: 0x000B2C68 File Offset: 0x000B0E68
		public virtual void OnReceiveDamage(vDamage damage)
		{
			Vector3 vector = damage.hitPosition - new Vector3(base.transform.position.x, damage.hitPosition.y, base.transform.position.z);
			Quaternion quaternion = ((vector != Vector3.zero) ? Quaternion.LookRotation(vector) : base.transform.rotation);
			if (damage.damageValue > 0f)
			{
				this.TriggerEffect(new vDamageEffectInfo(damage.hitPosition, quaternion, damage.damageType, damage.receiver));
			}
		}

		// Token: 0x06001CF0 RID: 7408 RVA: 0x000B2D00 File Offset: 0x000B0F00
		protected virtual void TriggerEffect(vDamageEffectInfo damageEffectInfo)
		{
			if (this._random == null)
			{
				this._random = new vFisherYatesRandom();
			}
			vDamageEffect vDamageEffect = this.customDamageEffects.Find((vDamageEffect effect) => effect.damageType.Equals(damageEffectInfo.damageType));
			if (vDamageEffect != null)
			{
				vDamageEffect.onTriggerEffect.Invoke();
				if (vDamageEffect.customDamageEffect != null && vDamageEffect.customDamageEffect.Count > 0)
				{
					GameObject gameObject = vDamageEffect.customDamageEffect[this._random.Next(vDamageEffect.customDamageEffect.Count)];
					Object.Instantiate<GameObject>(gameObject, damageEffectInfo.position, vDamageEffect.rotateToHitDirection ? damageEffectInfo.rotation : gameObject.transform.rotation, (vDamageEffect.attachInReceiver && damageEffectInfo.receiver) ? damageEffectInfo.receiver : vObjectContainer.root);
					return;
				}
			}
			else if (this.defaultDamageEffects.Count > 0 && damageEffectInfo != null)
			{
				Object.Instantiate<GameObject>(this.defaultDamageEffects[this._random.Next(this.defaultDamageEffects.Count)], damageEffectInfo.position, damageEffectInfo.rotation, vObjectContainer.root);
			}
		}

		// Token: 0x06001CF1 RID: 7409 RVA: 0x000B2E4C File Offset: 0x000B104C
		protected virtual void Reset()
		{
			this.defaultDamageEffects = new List<GameObject>();
			Object @object = Resources.Load("defaultDamageEffect");
			if (@object != null)
			{
				this.defaultDamageEffects.Add(@object as GameObject);
			}
		}

		// Token: 0x04001E0C RID: 7692
		public List<GameObject> defaultDamageEffects = new List<GameObject>();

		// Token: 0x04001E0D RID: 7693
		public List<vDamageEffect> customDamageEffects = new List<vDamageEffect>();

		// Token: 0x04001E0E RID: 7694
		private vFisherYatesRandom _random;
	}
}
