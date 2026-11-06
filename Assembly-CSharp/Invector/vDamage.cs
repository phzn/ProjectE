using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x020004BE RID: 1214
	[Serializable]
	public class vDamage
	{
		// Token: 0x06001D6E RID: 7534 RVA: 0x000B4EA8 File Offset: 0x000B30A8
		public vDamage()
		{
			this.damageValue = 15f;
			this.staminaBlockCost = 5f;
			this.staminaRecoveryDelay = 1f;
			this.hitReaction = true;
		}

		// Token: 0x06001D6F RID: 7535 RVA: 0x000B4F0B File Offset: 0x000B310B
		public vDamage(int value)
		{
			this.damageValue = (float)value;
			this.hitReaction = true;
		}

		// Token: 0x06001D70 RID: 7536 RVA: 0x000B4F4C File Offset: 0x000B314C
		public vDamage(int value, bool ignoreReaction)
		{
			this.damageValue = (float)value;
			this.hitReaction = !ignoreReaction;
			if (ignoreReaction)
			{
				this.recoil_id = -1;
				this.reaction_id = -1;
			}
		}

		// Token: 0x06001D71 RID: 7537 RVA: 0x000B4FAC File Offset: 0x000B31AC
		public vDamage(vDamage damage)
		{
			this.damageValue = damage.damageValue;
			this.staminaBlockCost = damage.staminaBlockCost;
			this.staminaRecoveryDelay = damage.staminaRecoveryDelay;
			this.ignoreDefense = damage.ignoreDefense;
			this.activeRagdoll = damage.activeRagdoll;
			this.sender = damage.sender;
			this.receiver = damage.receiver;
			this.recoil_id = damage.recoil_id;
			this.reaction_id = damage.reaction_id;
			this.damageType = damage.damageType;
			this.hitPosition = damage.hitPosition;
			this.senselessTime = damage.senselessTime;
			this.force = damage.force;
		}

		// Token: 0x06001D72 RID: 7538 RVA: 0x000B5084 File Offset: 0x000B3284
		public void ReduceDamage(float damageReduction)
		{
			int num = (int)(this.damageValue - this.damageValue * damageReduction / 100f);
			this.damageValue = (float)num;
		}

		// Token: 0x04001E89 RID: 7817
		[Tooltip("Apply damage to the Character Health")]
		public float damageValue = 15f;

		// Token: 0x04001E8A RID: 7818
		[Tooltip("How much stamina the target will lost when blocking this attack")]
		public float staminaBlockCost = 5f;

		// Token: 0x04001E8B RID: 7819
		[Tooltip("How much time the stamina of the target will wait to recovery")]
		public float staminaRecoveryDelay = 1f;

		// Token: 0x04001E8C RID: 7820
		[Tooltip("Apply damage even if the Character is blocking")]
		public bool ignoreDefense;

		// Token: 0x04001E8D RID: 7821
		[Tooltip("Activated Ragdoll when hit the Character")]
		public bool activeRagdoll;

		// Token: 0x04001E8E RID: 7822
		[vHideInInspector("activeRagdoll", false)]
		[Tooltip("Time to keep Ragdoll active")]
		public float senselessTime;

		// Token: 0x04001E8F RID: 7823
		[HideInInspector]
		public Transform sender;

		// Token: 0x04001E90 RID: 7824
		[HideInInspector]
		public Transform receiver;

		// Token: 0x04001E91 RID: 7825
		[HideInInspector]
		public Vector3 hitPosition;

		// Token: 0x04001E92 RID: 7826
		public bool hitReaction = true;

		// Token: 0x04001E93 RID: 7827
		[HideInInspector]
		public int recoil_id;

		// Token: 0x04001E94 RID: 7828
		[HideInInspector]
		public int reaction_id;

		// Token: 0x04001E95 RID: 7829
		public string damageType;

		// Token: 0x04001E96 RID: 7830
		[HideInInspector]
		public Vector3 force;
	}
}
