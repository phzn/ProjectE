using System;
using UnityEngine;

namespace Invector.vCharacterController
{
	// Token: 0x020006B9 RID: 1721
	public class vBlockUnarmedAttack : MonoBehaviour
	{
		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x06002A4A RID: 10826 RVA: 0x000ED636 File Offset: 0x000EB836
		// (set) Token: 0x06002A4B RID: 10827 RVA: 0x000ED63E File Offset: 0x000EB83E
		public bool IsActiveUnarmedAttack
		{
			get
			{
				return this.useUnarmedAttack;
			}
			protected set
			{
				this.useUnarmedAttack = value;
			}
		}

		// Token: 0x06002A4C RID: 10828 RVA: 0x000ED647 File Offset: 0x000EB847
		private void Start()
		{
			this.meleeCombatInput = base.GetComponent<vMeleeCombatInput>();
			this.meleeCombatInput.onUpdate += this.HandleAttackInput;
		}

		// Token: 0x06002A4D RID: 10829 RVA: 0x000ED66C File Offset: 0x000EB86C
		private void HandleAttackInput()
		{
			if (!this.IsActiveUnarmedAttack)
			{
				this.meleeCombatInput.weakAttackInput.useInput = this.meleeCombatInput.isArmed;
				this.meleeCombatInput.strongAttackInput.useInput = this.meleeCombatInput.isArmed;
			}
		}

		// Token: 0x06002A4E RID: 10830 RVA: 0x000ED6AC File Offset: 0x000EB8AC
		public void SetActiveUnarmedAttack(bool value)
		{
			if (value != this.IsActiveUnarmedAttack)
			{
				this.IsActiveUnarmedAttack = value;
				this.meleeCombatInput.weakAttackInput.useInput = value;
				this.meleeCombatInput.strongAttackInput.useInput = value;
			}
		}

		// Token: 0x04002A5D RID: 10845
		private vMeleeCombatInput meleeCombatInput;

		// Token: 0x04002A5E RID: 10846
		[SerializeField]
		protected bool useUnarmedAttack;
	}
}
