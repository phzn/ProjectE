using System;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vMelee
{
	// Token: 0x0200051A RID: 1306
	[vClassHeader("Melee Weapon", true, "icon_v2", false, "", openClose = false)]
	public class vMeleeWeapon : vMeleeAttackObject
	{
		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x06001FA6 RID: 8102 RVA: 0x000BC28C File Offset: 0x000BA48C
		// (set) Token: 0x06001FA7 RID: 8103 RVA: 0x000BC294 File Offset: 0x000BA494
		public virtual bool IsEquipped { get; set; }

		// Token: 0x06001FA8 RID: 8104 RVA: 0x000BC29D File Offset: 0x000BA49D
		public virtual void OnDefense()
		{
			this.onDefense.Invoke();
		}

		// Token: 0x0400205F RID: 8287
		[Header("Melee Weapon Settings")]
		public vMeleeType meleeType = vMeleeType.OnlyAttack;

		// Token: 0x04002060 RID: 8288
		[Header("Attack Settings")]
		public bool useStrongAttack = true;

		// Token: 0x04002061 RID: 8289
		[Tooltip("Simple AI Only")]
		public float distanceToAttack = 1f;

		// Token: 0x04002062 RID: 8290
		[Tooltip("Trigger a Attack Animation")]
		public int attackID;

		// Token: 0x04002063 RID: 8291
		[Tooltip("Change the MoveSet when using this Weapon")]
		public int movesetID;

		// Token: 0x04002064 RID: 8292
		[Header("* Third Person Controller Only *")]
		[Tooltip("How much stamina will be consumed when attack")]
		public float staminaCost;

		// Token: 0x04002065 RID: 8293
		[Tooltip("How much time the stamina will wait to start recover")]
		public float staminaRecoveryDelay;

		// Token: 0x04002066 RID: 8294
		[Header("Defense Settings")]
		[Range(0f, 100f)]
		public int defenseRate = 100;

		// Token: 0x04002067 RID: 8295
		[Range(0f, 180f)]
		public float defenseRange = 90f;

		// Token: 0x04002068 RID: 8296
		[Tooltip("Trigger a Defense Animation")]
		public int defenseID;

		// Token: 0x04002069 RID: 8297
		[Tooltip("What recoil animatil will trigger")]
		public int recoilID;

		// Token: 0x0400206A RID: 8298
		[Tooltip("Can break the oponent attack, will trigger a recoil animation")]
		public bool breakAttack;

		// Token: 0x0400206B RID: 8299
		[HideInInspector]
		public UnityEvent onDefense;
	}
}
