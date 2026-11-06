using System;
using System.Collections.Generic;
using Invector.vEventSystems;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vMelee
{
	// Token: 0x0200050E RID: 1294
	[vClassHeader("Melee Object", true, "icon_v2", false, "", openClose = false)]
	public class vMeleeAttackObject : vMonoBehaviour
	{
		// Token: 0x06001F68 RID: 8040 RVA: 0x000BB0FC File Offset: 0x000B92FC
		protected virtual void Start()
		{
			this.targetColliders = new Dictionary<vHitBox, List<GameObject>>();
			if (this.hitBoxes.Count > 0)
			{
				using (List<vHitBox>.Enumerator enumerator = this.hitBoxes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						vHitBox vHitBox = enumerator.Current;
						vHitBox.attackObject = this;
						this.targetColliders.Add(vHitBox, new List<GameObject>());
					}
					return;
				}
			}
			base.enabled = false;
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x000BB180 File Offset: 0x000B9380
		public virtual void SetActiveDamage(bool value)
		{
			this.canApplyDamage = value;
			for (int i = 0; i < this.hitBoxes.Count; i++)
			{
				vHitBox vHitBox = this.hitBoxes[i];
				vHitBox.trigger.enabled = value;
				if (!value && this.targetColliders != null)
				{
					this.targetColliders[vHitBox].Clear();
				}
			}
			if (value)
			{
				this.onEnableDamage.Invoke();
				return;
			}
			this.onDisableDamage.Invoke();
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x000BB1FC File Offset: 0x000B93FC
		public virtual void OnHit(vHitBox hitBox, Collider other)
		{
			if (this.canApplyDamage && !this.targetColliders[hitBox].Contains(other.gameObject) && this.meleeManager != null && other.gameObject != this.meleeManager.gameObject)
			{
				bool flag = false;
				bool flag2 = false;
				if (this.meleeManager == null)
				{
					this.meleeManager = base.GetComponentInParent<vMeleeManager>();
				}
				HitProperties hitProperties = this.meleeManager.hitProperties;
				if (((hitBox.triggerType & vHitBoxType.Damage) != (vHitBoxType)0 && hitProperties.hitDamageTags == null) || hitProperties.hitDamageTags.Count == 0)
				{
					flag = true;
				}
				else if ((hitBox.triggerType & vHitBoxType.Damage) != (vHitBoxType)0 && hitProperties.hitDamageTags.Contains(other.tag))
				{
					flag = true;
				}
				else if ((hitBox.triggerType & vHitBoxType.Recoil) != (vHitBoxType)0 && hitProperties.hitRecoilLayer == (hitProperties.hitRecoilLayer | (1 << other.gameObject.layer)))
				{
					flag2 = true;
				}
				if (flag || flag2)
				{
					this.targetColliders[hitBox].Add(other.gameObject);
					vHitInfo vHitInfo = new vHitInfo(this, hitBox, other, hitBox.transform.position);
					if (flag)
					{
						if (this.meleeManager)
						{
							this.meleeManager.OnDamageHit(ref vHitInfo);
						}
						else
						{
							this.damage.sender = (this.overrideDamageSender ? this.overrideDamageSender : base.transform);
						}
						if (!vHitInfo.targetIsBlocking)
						{
							this.onDamageHit.Invoke(vHitInfo);
						}
					}
					if (flag2)
					{
						if (this.meleeManager)
						{
							this.meleeManager.OnRecoilHit(vHitInfo);
						}
						this.onRecoilHit.Invoke(vHitInfo);
					}
				}
			}
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x000BB3B4 File Offset: 0x000B95B4
		public virtual bool ApplyDamage(vHitBox hitBox, Collider other, vDamage damage)
		{
			vDamage vDamage = new vDamage(damage);
			vDamage.receiver = other.transform;
			vDamage.damageValue = (float)Mathf.RoundToInt((damage.damageValue + (float)this.damageModifier) * ((float)hitBox.damagePercentage * 0.01f));
			vDamage.hitPosition = hitBox.transform.position;
			other.gameObject.ApplyDamage(vDamage, this.meleeManager.fighter);
			if (vDamage.hitReaction && vDamage.damageValue > 0f)
			{
				this.onPassDamage.Invoke(vDamage);
			}
			return vDamage.hitReaction;
		}

		// Token: 0x04002017 RID: 8215
		[vReadOnly(false)]
		public string attackObjectName;

		// Token: 0x04002018 RID: 8216
		public vDamage damage;

		// Token: 0x04002019 RID: 8217
		public Transform overrideDamageSender;

		// Token: 0x0400201A RID: 8218
		public List<vHitBox> hitBoxes;

		// Token: 0x0400201B RID: 8219
		public int damageModifier;

		// Token: 0x0400201C RID: 8220
		[HideInInspector]
		public bool canApplyDamage;

		// Token: 0x0400201D RID: 8221
		[vHelpBox("Event called when attack was successful", vHelpBoxAttribute.MessageType.None)]
		public OnHitEnter onDamageHit;

		// Token: 0x0400201E RID: 8222
		[vHelpBox("Event called when the attack causes recoil", vHelpBoxAttribute.MessageType.None)]
		public OnHitEnter onRecoilHit;

		// Token: 0x0400201F RID: 8223
		[vHelpBox("Event called when causes damage ", vHelpBoxAttribute.MessageType.None)]
		public OnReceiveDamage onPassDamage;

		// Token: 0x04002020 RID: 8224
		[vHelpBox("Events called when  Damage applier (HitBoxes) is enabled or disabled ", vHelpBoxAttribute.MessageType.None)]
		public UnityEvent onEnableDamage;

		// Token: 0x04002021 RID: 8225
		public UnityEvent onDisableDamage;

		// Token: 0x04002022 RID: 8226
		private Dictionary<vHitBox, List<GameObject>> targetColliders;

		// Token: 0x04002023 RID: 8227
		[HideInInspector]
		public vMeleeManager meleeManager;
	}
}
