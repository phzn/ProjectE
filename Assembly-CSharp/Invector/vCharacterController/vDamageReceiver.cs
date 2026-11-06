using System;
using Invector.vEventSystems;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vCharacterController
{
	// Token: 0x020006B1 RID: 1713
	[vClassHeader("DAMAGE RECEIVER", "You can add damage multiplier for example causing twice damage on Headshots", openClose = false)]
	[vClassHeader("DAMAGE RECEIVER", "You can add damage multiplier for example causing twice damage on Headshots", openClose = false)]
	public class vDamageReceiver : vMonoBehaviour, vIDamageReceiver, vIAttackReceiver
	{
		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x060029FC RID: 10748 RVA: 0x000EBE4B File Offset: 0x000EA04B
		// (set) Token: 0x060029FD RID: 10749 RVA: 0x000EBE53 File Offset: 0x000EA053
		public OnReceiveDamage onStartReceiveDamage
		{
			get
			{
				return this._onStartReceiveDamage;
			}
			protected set
			{
				this._onStartReceiveDamage = value;
			}
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x060029FE RID: 10750 RVA: 0x000EBE5C File Offset: 0x000EA05C
		// (set) Token: 0x060029FF RID: 10751 RVA: 0x000EBE64 File Offset: 0x000EA064
		public OnReceiveDamage onReceiveDamage
		{
			get
			{
				return this._onReceiveDamage;
			}
			protected set
			{
				this._onReceiveDamage = value;
			}
		}

		// Token: 0x06002A00 RID: 10752 RVA: 0x000EBE6D File Offset: 0x000EA06D
		protected virtual void Start()
		{
			this.ragdoll = base.GetComponentInParent<vRagdoll>();
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x000EBE7B File Offset: 0x000EA07B
		protected virtual void OnCollisionEnter(Collision collision)
		{
			if (collision != null && this.ragdoll && this.ragdoll.isActive)
			{
				this.ragdoll.OnRagdollCollisionEnter(new vRagdollCollision(base.gameObject, collision));
			}
		}

		// Token: 0x06002A02 RID: 10754 RVA: 0x000EBEB4 File Offset: 0x000EA0B4
		public virtual void TakeDamage(vDamage damage)
		{
			if (this.healthController == null && this.targetReceiver)
			{
				this.healthController = this.targetReceiver.GetComponent<vIHealthController>();
			}
			else if (this.healthController == null)
			{
				this.healthController = base.GetComponentInParent<vIHealthController>();
			}
			if (this.healthController != null)
			{
				this.onStartReceiveDamage.Invoke(damage);
				vDamage vDamage = this.ApplyDamageModifiers(damage);
				this.healthController.TakeDamage(vDamage);
				this.onReceiveDamage.Invoke(vDamage);
			}
		}

		// Token: 0x06002A03 RID: 10755 RVA: 0x000EBF34 File Offset: 0x000EA134
		public virtual vDamage ApplyDamageModifiers(vDamage damage)
		{
			float num = ((this.useRandomValues && !this.fixedValues) ? Random.Range(this.minDamageMultiplier, this.maxDamageMultiplier) : ((this.useRandomValues && this.fixedValues) ? (this.randomChange ? this.maxDamageMultiplier : this.minDamageMultiplier) : this.damageMultiplier));
			vDamage vDamage = new vDamage(damage);
			vDamage.damageValue *= num;
			if (num == this.maxDamageMultiplier)
			{
				this.OnGetMaxValue.Invoke();
			}
			this.OverrideReaction(ref vDamage);
			return vDamage;
		}

		// Token: 0x06002A04 RID: 10756 RVA: 0x000EBFC8 File Offset: 0x000EA1C8
		protected virtual void OverrideReaction(ref vDamage damage)
		{
			if (this.overrideReactionID)
			{
				if (this.useRandomValues && !this.fixedValues)
				{
					damage.reaction_id = Random.Range(this.minReactionID, this.maxReactionID);
					return;
				}
				if (this.useRandomValues && this.fixedValues)
				{
					damage.reaction_id = (this.randomChange ? this.maxReactionID : this.minReactionID);
					return;
				}
				damage.reaction_id = this.reactionID;
			}
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x06002A05 RID: 10757 RVA: 0x000EC041 File Offset: 0x000EA241
		protected virtual bool randomChange
		{
			get
			{
				return Random.Range(0f, 100f) < this.changeToMaxValue;
			}
		}

		// Token: 0x06002A06 RID: 10758 RVA: 0x000EC05C File Offset: 0x000EA25C
		public void OnReceiveAttack(vDamage damage, vIMeleeFighter attacker)
		{
			if (this.ragdoll && !this.ragdoll.iChar.isDead)
			{
				vDamage vDamage = this.ApplyDamageModifiers(damage);
				this.ragdoll.gameObject.ApplyDamage(vDamage, attacker);
				this.onReceiveDamage.Invoke(vDamage);
				return;
			}
			if (this.targetReceiver)
			{
				vDamage vDamage2 = this.ApplyDamageModifiers(damage);
				this.targetReceiver.gameObject.ApplyDamage(vDamage2, attacker);
				this.onReceiveDamage.Invoke(vDamage2);
				return;
			}
			this.TakeDamage(damage);
		}

		// Token: 0x06002A08 RID: 10760 RVA: 0x000AD00A File Offset: 0x000AB20A
		Transform vIDamageReceiver.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06002A09 RID: 10761 RVA: 0x000B0592 File Offset: 0x000AE792
		GameObject vIDamageReceiver.get_gameObject()
		{
			return base.gameObject;
		}

		// Token: 0x04002A09 RID: 10761
		[vEditorToolbar("Default", false, "", false, false)]
		public GameObject targetReceiver;

		// Token: 0x04002A0A RID: 10762
		public vIHealthController healthController;

		// Token: 0x04002A0B RID: 10763
		public float damageMultiplier = 1f;

		// Token: 0x04002A0C RID: 10764
		[HideInInspector]
		public vRagdoll ragdoll;

		// Token: 0x04002A0D RID: 10765
		public bool overrideReactionID;

		// Token: 0x04002A0E RID: 10766
		[vHideInInspector("overrideReactionID", false)]
		public int reactionID;

		// Token: 0x04002A0F RID: 10767
		[vEditorToolbar("Random", false, "", false, false)]
		public bool useRandomValues;

		// Token: 0x04002A10 RID: 10768
		[vHideInInspector("useRandomValues", false)]
		public bool fixedValues;

		// Token: 0x04002A11 RID: 10769
		[vHideInInspector("useRandomValues", false)]
		public float minDamageMultiplier;

		// Token: 0x04002A12 RID: 10770
		[vHideInInspector("useRandomValues", false)]
		public float maxDamageMultiplier;

		// Token: 0x04002A13 RID: 10771
		[vHideInInspector("useRandomValues", false)]
		public int minReactionID;

		// Token: 0x04002A14 RID: 10772
		[vHideInInspector("useRandomValues", false)]
		public int maxReactionID;

		// Token: 0x04002A15 RID: 10773
		[vHideInInspector("useRandomValues;fixedValues", false)]
		[Tooltip("Change Between 0 and 100")]
		public float changeToMaxValue;

		// Token: 0x04002A16 RID: 10774
		public UnityEvent OnGetMaxValue;

		// Token: 0x04002A17 RID: 10775
		[vEditorToolbar("Events", false, "", false, false)]
		[SerializeField]
		protected OnReceiveDamage _onStartReceiveDamage = new OnReceiveDamage();

		// Token: 0x04002A18 RID: 10776
		[SerializeField]
		protected OnReceiveDamage _onReceiveDamage = new OnReceiveDamage();
	}
}
