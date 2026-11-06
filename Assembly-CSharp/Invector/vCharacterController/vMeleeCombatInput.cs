using System;
using Invector.vEventSystems;
using Invector.vMelee;
using UnityEngine;

namespace Invector.vCharacterController
{
	// Token: 0x020006BA RID: 1722
	[vClassHeader("MELEE INPUT MANAGER", true, "icon_v2", false, "", iconName = "inputIcon")]
	public class vMeleeCombatInput : vThirdPersonInput, vIMeleeFighter, vIAttackReceiver, vIAttackListener
	{
		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06002A50 RID: 10832 RVA: 0x000ED6E0 File Offset: 0x000EB8E0
		// (set) Token: 0x06002A51 RID: 10833 RVA: 0x000ED6E8 File Offset: 0x000EB8E8
		protected virtual bool _isAttacking { get; set; }

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06002A52 RID: 10834 RVA: 0x000ED6F1 File Offset: 0x000EB8F1
		// (set) Token: 0x06002A53 RID: 10835 RVA: 0x000ED70D File Offset: 0x000EB90D
		public virtual bool isAttacking
		{
			get
			{
				return this._isAttacking || this.cc.IsAnimatorTag("Attack");
			}
			protected set
			{
				this._isAttacking = value;
			}
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06002A54 RID: 10836 RVA: 0x000ED716 File Offset: 0x000EB916
		// (set) Token: 0x06002A55 RID: 10837 RVA: 0x000ED71E File Offset: 0x000EB91E
		public virtual bool isBlocking { get; protected set; }

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x06002A56 RID: 10838 RVA: 0x000ED728 File Offset: 0x000EB928
		public virtual bool isArmed
		{
			get
			{
				return this.meleeManager != null && (this.meleeManager.rightWeapon != null || (this.meleeManager.leftWeapon != null && this.meleeManager.leftWeapon.meleeType > vMeleeType.OnlyDefense));
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x06002A57 RID: 10839 RVA: 0x000ED782 File Offset: 0x000EB982
		// (set) Token: 0x06002A58 RID: 10840 RVA: 0x000ED78A File Offset: 0x000EB98A
		public virtual bool isEquipping { get; protected set; }

		// Token: 0x06002A59 RID: 10841 RVA: 0x000ED793 File Offset: 0x000EB993
		public void SetLockMeleeInput(bool value)
		{
			this.lockMeleeInput = value;
			if (value)
			{
				this.isAttacking = false;
				this.isBlocking = false;
			}
		}

		// Token: 0x06002A5A RID: 10842 RVA: 0x000ED7AD File Offset: 0x000EB9AD
		public override void SetLockAllInput(bool value)
		{
			base.SetLockAllInput(value);
			this.SetLockMeleeInput(value);
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x06002A5B RID: 10843 RVA: 0x000ED7BD File Offset: 0x000EB9BD
		public virtual bool lockInventory
		{
			get
			{
				return this.isAttacking || this.cc.isDead || this.cc.customAction || this.cc.isRolling;
			}
		}

		// Token: 0x06002A5C RID: 10844 RVA: 0x000ED7EE File Offset: 0x000EB9EE
		protected override void Start()
		{
			base.Start();
		}

		// Token: 0x06002A5D RID: 10845 RVA: 0x000ED7F6 File Offset: 0x000EB9F6
		protected override void LateUpdate()
		{
			this.UpdateMeleeAnimations();
			base.LateUpdate();
		}

		// Token: 0x06002A5E RID: 10846 RVA: 0x000ED804 File Offset: 0x000EBA04
		protected override void FixedUpdate()
		{
			base.FixedUpdate();
		}

		// Token: 0x06002A5F RID: 10847 RVA: 0x000ED80C File Offset: 0x000EBA0C
		public override void InputHandle()
		{
			if (this.cc == null || this.cc.isDead)
			{
				return;
			}
			base.InputHandle();
			if (this.MeleeAttackConditions() && !this.lockMeleeInput)
			{
				this.MeleeWeakAttackInput();
				this.MeleeStrongAttackInput();
				this.BlockingInput();
				return;
			}
			this.ResetAttackTriggers();
			this.isBlocking = false;
		}

		// Token: 0x06002A60 RID: 10848 RVA: 0x000ED86B File Offset: 0x000EBA6B
		public virtual void MeleeWeakAttackInput()
		{
			if (base.animator == null)
			{
				return;
			}
			if (this.weakAttackInput.GetButtonDown() && this.MeleeAttackStaminaConditions())
			{
				this.TriggerWeakAttack();
			}
		}

		// Token: 0x06002A61 RID: 10849 RVA: 0x000ED897 File Offset: 0x000EBA97
		public virtual void TriggerWeakAttack()
		{
			base.animator.SetInteger(vAnimatorParameters.AttackID, this.AttackID);
			base.animator.SetTrigger(vAnimatorParameters.WeakAttack);
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x000ED8C0 File Offset: 0x000EBAC0
		public virtual void MeleeStrongAttackInput()
		{
			if (base.animator == null)
			{
				return;
			}
			if (this.strongAttackInput.GetButtonDown() && (!this.meleeManager.CurrentActiveAttackWeapon || this.meleeManager.CurrentActiveAttackWeapon.useStrongAttack) && this.MeleeAttackStaminaConditions())
			{
				this.TriggerStrongAttack();
			}
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x000ED91B File Offset: 0x000EBB1B
		public virtual void TriggerStrongAttack()
		{
			base.animator.SetInteger(vAnimatorParameters.AttackID, this.AttackID);
			base.animator.SetTrigger(vAnimatorParameters.StrongAttack);
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x000ED944 File Offset: 0x000EBB44
		public virtual void BlockingInput()
		{
			if (base.animator == null)
			{
				return;
			}
			this.isBlocking = this.blockInput.GetButton() && this.cc.currentStamina > 0f && !this.cc.customAction && !this.isAttacking;
		}

		// Token: 0x06002A65 RID: 10853 RVA: 0x000ED9A0 File Offset: 0x000EBBA0
		public override void SprintInput()
		{
			if (this.sprintInput.useInput)
			{
				bool flag = (this.cc.useContinuousSprint ? this.sprintInput.GetButtonDown() : this.sprintInput.GetButton());
				this.cc.Sprint(flag && !this.isAttacking);
			}
		}

		// Token: 0x06002A66 RID: 10854 RVA: 0x000ED9FA File Offset: 0x000EBBFA
		protected virtual bool MeleeAttackStaminaConditions()
		{
			return this.cc.ignoreStaminaConsumption || this.cc.currentStamina - this.meleeManager.GetAttackStaminaCost() >= 0f;
		}

		// Token: 0x06002A67 RID: 10855 RVA: 0x000EDA2C File Offset: 0x000EBC2C
		protected virtual bool MeleeAttackConditions()
		{
			if (this.meleeManager == null)
			{
				this.meleeManager = base.GetComponent<vMeleeManager>();
			}
			return this.meleeManager != null && this.cc.isGrounded && !this.cc.customAction && !this.cc.isJumping && !this.cc.isCrouching && !this.cc.isRolling && !this.isEquipping && !base.animator.IsInTransition(this.cc.baseLayer);
		}

		// Token: 0x06002A68 RID: 10856 RVA: 0x000EDAC5 File Offset: 0x000EBCC5
		public override bool JumpConditions()
		{
			return !this.isAttacking && base.JumpConditions();
		}

		// Token: 0x06002A69 RID: 10857 RVA: 0x000EDAD8 File Offset: 0x000EBCD8
		public override bool RollConditions()
		{
			return base.RollConditions() && !this.isAttacking && !base.animator.IsInTransition(this.cc.upperBodyLayer) && !base.animator.IsInTransition(this.cc.fullbodyLayer);
		}

		// Token: 0x06002A6A RID: 10858 RVA: 0x000EDB28 File Offset: 0x000EBD28
		protected virtual void UpdateMeleeAnimations()
		{
			if (base.animator == null || this.meleeManager == null)
			{
				return;
			}
			base.animator.SetInteger(vAnimatorParameters.AttackID, this.AttackID);
			base.animator.SetInteger(vAnimatorParameters.DefenseID, this.DefenseID);
			base.animator.SetBool(vAnimatorParameters.IsBlocking, this.isBlocking);
			base.animator.SetFloat(vAnimatorParameters.MoveSet_ID, (float)this.meleeMoveSetID, 0.2f, vTime.fixedDeltaTime);
			this.isEquipping = this.cc.IsAnimatorTag("IsEquipping");
		}

		// Token: 0x170007D6 RID: 2006
		// (get) Token: 0x06002A6B RID: 10859 RVA: 0x000EDBCB File Offset: 0x000EBDCB
		// (set) Token: 0x06002A6C RID: 10860 RVA: 0x000EDBD3 File Offset: 0x000EBDD3
		public virtual int defaultMoveSetID { get; set; }

		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06002A6D RID: 10861 RVA: 0x000EDBDC File Offset: 0x000EBDDC
		// (set) Token: 0x06002A6E RID: 10862 RVA: 0x000EDBE4 File Offset: 0x000EBDE4
		public virtual bool overrideWeaponMoveSetID { get; set; }

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06002A6F RID: 10863 RVA: 0x000EDBF0 File Offset: 0x000EBDF0
		public virtual int meleeMoveSetID
		{
			get
			{
				int num = this.meleeManager.GetMoveSetID();
				if (num == 0 || this.overrideWeaponMoveSetID)
				{
					num = this.defaultMoveSetID;
				}
				return num;
			}
		}

		// Token: 0x06002A70 RID: 10864 RVA: 0x000EDC1C File Offset: 0x000EBE1C
		public virtual void ResetMeleeAnimations()
		{
			if (this.meleeManager == null || !base.animator)
			{
				return;
			}
			base.animator.SetBool(vAnimatorParameters.IsBlocking, false);
		}

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06002A71 RID: 10865 RVA: 0x000EDC4B File Offset: 0x000EBE4B
		public virtual int AttackID
		{
			get
			{
				if (!this.meleeManager)
				{
					return 0;
				}
				return this.meleeManager.GetAttackID();
			}
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06002A72 RID: 10866 RVA: 0x000EDC67 File Offset: 0x000EBE67
		public virtual int DefenseID
		{
			get
			{
				if (!this.meleeManager)
				{
					return 0;
				}
				return this.meleeManager.GetDefenseID();
			}
		}

		// Token: 0x06002A73 RID: 10867 RVA: 0x000EDC84 File Offset: 0x000EBE84
		public virtual void OnEnableAttack()
		{
			if (this.meleeManager == null)
			{
				this.meleeManager = base.GetComponent<vMeleeManager>();
			}
			if (this.meleeManager == null)
			{
				return;
			}
			if (!this.cc.ignoreStaminaConsumption)
			{
				this.cc.currentStaminaRecoveryDelay = this.meleeManager.GetAttackStaminaRecoveryDelay();
				this.cc.currentStamina -= this.meleeManager.GetAttackStaminaCost();
			}
			this.isAttacking = true;
			this.cc.isSprinting = false;
		}

		// Token: 0x06002A74 RID: 10868 RVA: 0x000EDD0D File Offset: 0x000EBF0D
		public virtual void OnDisableAttack()
		{
			this.isAttacking = false;
		}

		// Token: 0x06002A75 RID: 10869 RVA: 0x000EDD16 File Offset: 0x000EBF16
		public virtual void ResetAttackTriggers()
		{
			base.animator.ResetTrigger(vAnimatorParameters.WeakAttack);
			base.animator.ResetTrigger(vAnimatorParameters.StrongAttack);
		}

		// Token: 0x06002A76 RID: 10870 RVA: 0x000EDD38 File Offset: 0x000EBF38
		public virtual void BreakAttack(int breakAtkID)
		{
			this.ResetAttackTriggers();
			this.OnRecoil(breakAtkID);
		}

		// Token: 0x06002A77 RID: 10871 RVA: 0x000EDD48 File Offset: 0x000EBF48
		public virtual void OnRecoil(int recoilID)
		{
			base.animator.SetInteger(vAnimatorParameters.RecoilID, recoilID);
			base.animator.SetTrigger(vAnimatorParameters.TriggerRecoil);
			base.animator.SetTrigger(vAnimatorParameters.ResetState);
			base.animator.ResetTrigger(vAnimatorParameters.WeakAttack);
			base.animator.ResetTrigger(vAnimatorParameters.StrongAttack);
		}

		// Token: 0x06002A78 RID: 10872 RVA: 0x000EDDA8 File Offset: 0x000EBFA8
		public virtual void OnReceiveAttack(vDamage damage, vIMeleeFighter attacker)
		{
			if (!damage.ignoreDefense && this.isBlocking && this.meleeManager != null && this.meleeManager.CanBlockAttack(damage.sender.position))
			{
				int defenseRate = this.meleeManager.GetDefenseRate();
				if (defenseRate > 0)
				{
					damage.ReduceDamage((float)defenseRate);
				}
				if (attacker != null && this.meleeManager != null && this.meleeManager.CanBreakAttack())
				{
					attacker.BreakAttack(this.meleeManager.GetDefenseRecoilID());
				}
				this.meleeManager.OnDefense();
				if (!this.cc.ignoreStaminaConsumption)
				{
					this.cc.currentStaminaRecoveryDelay = damage.staminaRecoveryDelay;
					this.cc.currentStamina -= damage.staminaBlockCost;
				}
			}
			damage.hitReaction = !this.isBlocking || damage.ignoreDefense;
			this.cc.TakeDamage(damage);
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x06002A79 RID: 10873 RVA: 0x000EDEA2 File Offset: 0x000EC0A2
		public virtual vICharacter character
		{
			get
			{
				return this.cc;
			}
		}

		// Token: 0x06002A7B RID: 10875 RVA: 0x000AD00A File Offset: 0x000AB20A
		Transform vIMeleeFighter.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06002A7C RID: 10876 RVA: 0x000B0592 File Offset: 0x000AE792
		GameObject vIMeleeFighter.get_gameObject()
		{
			return base.gameObject;
		}

		// Token: 0x04002A5F RID: 10847
		[vEditorToolbar("Inputs", false, "", false, false)]
		[Header("Melee Inputs")]
		public GenericInput weakAttackInput = new GenericInput("Mouse0", "RB", "RB");

		// Token: 0x04002A60 RID: 10848
		public GenericInput strongAttackInput = new GenericInput("Alpha1", false, "RT", true, "RT", false);

		// Token: 0x04002A61 RID: 10849
		public GenericInput blockInput = new GenericInput("Mouse1", "LB", "LB");

		// Token: 0x04002A62 RID: 10850
		internal vMeleeManager meleeManager;

		// Token: 0x04002A66 RID: 10854
		[HideInInspector]
		public bool lockMeleeInput;
	}
}
