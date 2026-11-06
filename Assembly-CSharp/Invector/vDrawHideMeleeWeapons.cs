using System;
using Invector.vCharacterController;
using Invector.vItemManager;
using UnityEngine;
using UnityEngine.Events;

namespace Invector
{
	// Token: 0x020004D0 RID: 1232
	[vClassHeader("Draw/Hide Melee Weapons", "This component works with vItemManager, vWeaponHolderManager and vMeleeCombatInput", useHelpBox = true)]
	public class vDrawHideMeleeWeapons : vMonoBehaviour
	{
		// Token: 0x06001DB1 RID: 7601 RVA: 0x000B5E20 File Offset: 0x000B4020
		protected virtual void Start()
		{
			this.holderManager = base.GetComponent<vWeaponHolderManager>();
			this.melee = base.GetComponent<vMeleeCombatInput>();
			if (this.holderManager && this.melee)
			{
				this.melee.onUpdate -= this.ControlWeapons;
				this.melee.onUpdate += this.ControlWeapons;
				if (this.melee == null)
				{
					Debug.LogWarning("You're missing a vMeleeCombatInput, please add one", base.gameObject);
				}
			}
		}

		// Token: 0x06001DB2 RID: 7602 RVA: 0x000B5EB0 File Offset: 0x000B40B0
		protected virtual void ControlWeapons()
		{
			if (this.isLocked || this.melee.cc == null || this.melee.cc.customAction)
			{
				return;
			}
			this.HandleInput();
			this.DrawWeaponsImmediateHandle();
			this.HideWeaponsAutomatically();
		}

		// Token: 0x06001DB3 RID: 7603 RVA: 0x000B5F00 File Offset: 0x000B4100
		protected virtual GameObject RightWeaponObject(bool checkIsActve = false)
		{
			if (!this.melee || !this.melee.meleeManager || !this.melee.meleeManager.rightWeapon || (checkIsActve && !this.melee.meleeManager.rightWeapon.gameObject.activeInHierarchy))
			{
				return null;
			}
			return this.melee.meleeManager.rightWeapon.gameObject;
		}

		// Token: 0x06001DB4 RID: 7604 RVA: 0x000B5F7C File Offset: 0x000B417C
		protected virtual GameObject LeftWeaponObject(bool checkIsActve = false)
		{
			if (!this.melee || !this.melee.meleeManager || !this.melee.meleeManager.leftWeapon || (checkIsActve && !this.melee.meleeManager.leftWeapon.gameObject.activeInHierarchy))
			{
				return null;
			}
			return this.melee.meleeManager.leftWeapon.gameObject;
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001DB5 RID: 7605 RVA: 0x000B5FF5 File Offset: 0x000B41F5
		// (set) Token: 0x06001DB6 RID: 7606 RVA: 0x000B5FFD File Offset: 0x000B41FD
		public virtual vMeleeCombatInput melee { get; set; }

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001DB7 RID: 7607 RVA: 0x000B6006 File Offset: 0x000B4206
		// (set) Token: 0x06001DB8 RID: 7608 RVA: 0x000B600E File Offset: 0x000B420E
		public virtual vWeaponHolderManager holderManager { get; set; }

		// Token: 0x06001DB9 RID: 7609 RVA: 0x000B6017 File Offset: 0x000B4217
		public virtual void ReturnToLastState(bool immediate = false)
		{
			if (this.previouslyWeaponsHided)
			{
				this.HideWeapons(immediate);
				return;
			}
			this.DrawWeapons(immediate);
		}

		// Token: 0x06001DBA RID: 7610 RVA: 0x000B6030 File Offset: 0x000B4230
		public virtual void LockDrawHideInput(bool value)
		{
			this.isLocked = value;
			if (this.lockInventoryInputOnLock && this.holderManager.itemManager)
			{
				this.holderManager.itemManager.LockInventoryInput(value);
			}
		}

		// Token: 0x06001DBB RID: 7611 RVA: 0x000B6064 File Offset: 0x000B4264
		public virtual void HideWeapons(bool immediate = false)
		{
			this.previouslyWeaponsHided = this.weaponsHided;
			if (this.CanHideRightWeapon())
			{
				this.weaponsHided = true;
				this.HideRightWeapon(immediate);
				return;
			}
			if (this.CanHideLeftWeapon())
			{
				this.weaponsHided = true;
				this.HideLeftWeapon(immediate);
			}
		}

		// Token: 0x06001DBC RID: 7612 RVA: 0x000B609F File Offset: 0x000B429F
		public virtual void ForceHideWeapons(bool immediate = false)
		{
			this.forceHide = true;
			this.HideWeapons(immediate);
			base.Invoke("ResetForceHide", 1f);
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x000B60BF File Offset: 0x000B42BF
		protected virtual void ResetForceHide()
		{
			this.forceHide = false;
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x000B60C8 File Offset: 0x000B42C8
		public virtual void DrawWeapons(bool immediate = false)
		{
			if (this.CanDrawRightWeapon())
			{
				this.previouslyWeaponsHided = this.weaponsHided;
				this.weaponsHided = false;
				this.DrawRightWeapon(immediate);
				return;
			}
			if (this.CanDrawLeftWeapon())
			{
				this.previouslyWeaponsHided = this.weaponsHided;
				this.weaponsHided = false;
				this.DrawLeftWeapon(immediate);
			}
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x000B611C File Offset: 0x000B431C
		protected virtual void HideWeaponsAutomatically()
		{
			if (this.hideWeaponsAutomatically)
			{
				if (this.HideTimerConditions())
				{
					this.currentTimer += Time.deltaTime;
				}
				else
				{
					this.currentTimer = 0f;
				}
				if (this.currentTimer >= this.hideWeaponsTimer && !this.IsEquipping)
				{
					this.currentTimer = 0f;
					this.HideWeapons(false);
					return;
				}
			}
			else if (this.currentTimer > 0f)
			{
				this.currentTimer = 0f;
			}
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x000B6199 File Offset: 0x000B4399
		protected virtual bool HideTimerConditions()
		{
			return this.CanHideWeapons() && (this.CanHideRightWeapon() || this.CanHideLeftWeapon());
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x000B61B8 File Offset: 0x000B43B8
		protected virtual bool CanHideWeapons()
		{
			return this.melee && this.melee.meleeManager && (this.forceHide || (!this.melee.isAttacking && !this.melee.isBlocking && (this.melee.meleeManager.rightWeapon || this.melee.meleeManager.leftWeapon)));
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x000B623A File Offset: 0x000B443A
		protected virtual bool CanDrawWeapons()
		{
			return this.melee && this.melee.meleeManager;
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x000B625B File Offset: 0x000B445B
		protected virtual bool CanHideRightWeapon()
		{
			return this.CanHideWeapons() && this.RightWeaponObject(false) && this.RightWeaponObject(false).activeInHierarchy;
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x000B6281 File Offset: 0x000B4481
		protected virtual bool CanHideLeftWeapon()
		{
			return this.CanHideWeapons() && this.LeftWeaponObject(false) && this.LeftWeaponObject(false).activeInHierarchy;
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x000B62A7 File Offset: 0x000B44A7
		protected virtual bool CanDrawRightWeapon()
		{
			return this.CanDrawWeapons() && this.RightWeaponObject(false) && !this.RightWeaponObject(false).activeInHierarchy;
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x000B62D0 File Offset: 0x000B44D0
		protected virtual bool CanDrawLeftWeapon()
		{
			return this.CanDrawWeapons() && this.LeftWeaponObject(false) && !this.LeftWeaponObject(false).activeInHierarchy;
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001DC7 RID: 7623 RVA: 0x000B62F9 File Offset: 0x000B44F9
		protected virtual bool IsEquipping
		{
			get
			{
				return this.melee != null && this.melee.cc && this.melee.cc.IsAnimatorTag("IsEquipping");
			}
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x000B6334 File Offset: 0x000B4534
		protected virtual void HandleInput()
		{
			if (this.hideAndDrawWeaponsInput.GetButtonDown() && !this.IsEquipping)
			{
				if (this.CanHideRightWeapon() || this.CanHideLeftWeapon())
				{
					this.HideWeapons(false);
					return;
				}
				if (this.CanDrawRightWeapon() || this.CanDrawLeftWeapon())
				{
					this.DrawWeapons(false);
				}
			}
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x000B6385 File Offset: 0x000B4585
		protected virtual void DrawWeaponsImmediateHandle()
		{
			if (this.DrawWeaponsImmediateConditions())
			{
				this.DrawWeapons(true);
			}
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x000B6398 File Offset: 0x000B4598
		protected virtual bool DrawWeaponsImmediateConditions()
		{
			return this.melee && !this.melee.cc.customAction && this.melee.meleeManager && (!(this.melee.meleeManager.CurrentAttackWeapon == null) || !(this.melee.meleeManager.CurrentDefenseWeapon == null)) && ((this.melee.weakAttackInput.GetButton() && this.meleeWeakAttack) || (this.melee.strongAttackInput.GetButton() && this.meleeStrongAttack) || (this.melee.blockInput.GetButton() && this.meleeBlock));
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x000B6458 File Offset: 0x000B4658
		protected virtual void HideRightWeapon(bool immediate = false)
		{
			GameObject weapon = this.RightWeaponObject(true);
			if (weapon)
			{
				vEquipment component = weapon.GetComponent<vEquipment>();
				if (component == null || component.equipPoint == null || component.equipPoint.area == null)
				{
					return;
				}
				vWeaponHolder holder = this.holderManager.GetHolder(weapon.gameObject, component.referenceItem.id);
				this.HideWeaponsHandle(this.melee, component, null, delegate
				{
					if (holder)
					{
						holder.SetActiveWeapon(true);
					}
					if (weapon)
					{
						weapon.gameObject.SetActive(false);
					}
					if (this.CanHideLeftWeapon())
					{
						this.HideLeftWeapon(immediate);
					}
				}, immediate);
			}
		}

		// Token: 0x06001DCC RID: 7628 RVA: 0x000B650C File Offset: 0x000B470C
		protected virtual void HideLeftWeapon(bool immediate = false)
		{
			GameObject weapon = this.LeftWeaponObject(true);
			if (weapon)
			{
				vEquipment component = weapon.GetComponent<vEquipment>();
				if (component == null || component.equipPoint == null || component.equipPoint.area == null)
				{
					return;
				}
				vWeaponHolder holder = this.holderManager.GetHolder(weapon.gameObject, component.referenceItem.id);
				this.HideWeaponsHandle(this.melee, component, null, delegate
				{
					if (holder)
					{
						holder.SetActiveWeapon(true);
					}
					if (weapon)
					{
						weapon.gameObject.SetActive(false);
					}
				}, immediate);
			}
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x000B65AC File Offset: 0x000B47AC
		protected virtual void DrawRightWeapon(bool immediate = false)
		{
			GameObject weapon = this.RightWeaponObject(false);
			if (weapon)
			{
				vEquipment component = weapon.GetComponent<vEquipment>();
				if (component == null || component.equipPoint == null || component.equipPoint.area == null)
				{
					return;
				}
				if (component.equipPoint.area.isLockedToEquip)
				{
					return;
				}
				vWeaponHolder holder = this.holderManager.GetHolder(weapon.gameObject, component.referenceItem.id);
				this.DrawWeaponsHandle(this.melee, component, null, delegate
				{
					if (holder)
					{
						holder.SetActiveWeapon(false);
					}
					if (weapon && weapon.gameObject)
					{
						weapon.gameObject.SetActive(true);
					}
					if (this.CanDrawLeftWeapon())
					{
						this.DrawLeftWeapon(immediate);
					}
				}, immediate);
			}
		}

		// Token: 0x06001DCE RID: 7630 RVA: 0x000B6678 File Offset: 0x000B4878
		protected virtual void DrawLeftWeapon(bool immediate = false)
		{
			GameObject weapon = this.LeftWeaponObject(false);
			if (weapon)
			{
				vEquipment component = weapon.GetComponent<vEquipment>();
				if (component == null || component.equipPoint == null || component.equipPoint.area == null)
				{
					return;
				}
				if (component.equipPoint.area.isLockedToEquip)
				{
					return;
				}
				vWeaponHolder holder = this.holderManager.GetHolder(weapon.gameObject, component.referenceItem.id);
				this.DrawWeaponsHandle(this.melee, component, null, delegate
				{
					if (holder)
					{
						holder.SetActiveWeapon(false);
					}
					if (weapon && weapon.gameObject)
					{
						weapon.gameObject.SetActive(true);
					}
				}, immediate);
			}
		}

		// Token: 0x06001DCF RID: 7631 RVA: 0x000B6730 File Offset: 0x000B4930
		protected virtual void DrawWeaponsHandle(vThirdPersonInput tpInput, vEquipment equipment, UnityAction onStart, UnityAction onFinish, bool immediate = false)
		{
			if (this.holderManager.inEquip && !immediate)
			{
				return;
			}
			if (!immediate)
			{
				if (!string.IsNullOrEmpty(equipment.referenceItem.EnableAnim) && equipment != null && equipment.equipPoint != null)
				{
					tpInput.animator.SetBool("FlipEquip", equipment.equipPoint.equipPointName.Contains("Left"));
					tpInput.animator.CrossFade(equipment.referenceItem.EnableAnim, 0.25f);
				}
				else
				{
					immediate = true;
				}
			}
			if (equipment)
			{
				equipment.IsEquipped = true;
			}
			base.StartCoroutine(this.holderManager.EquipRoutine(equipment.referenceItem.enableDelayTime, immediate, onStart, onFinish, ""));
		}

		// Token: 0x06001DD0 RID: 7632 RVA: 0x000B67F4 File Offset: 0x000B49F4
		protected virtual void HideWeaponsHandle(vThirdPersonInput tpInput, vEquipment equipment, UnityAction onStart, UnityAction onFinish, bool immediate = false)
		{
			if (this.holderManager.inUnequip && !immediate)
			{
				return;
			}
			if (!immediate)
			{
				if (!string.IsNullOrEmpty(equipment.referenceItem.DisableAnim) && equipment != null && equipment.equipPoint != null)
				{
					tpInput.animator.SetBool("FlipEquip", equipment.equipPoint.equipPointName.Contains("Left"));
					tpInput.animator.CrossFade(equipment.referenceItem.DisableAnim, 0.25f);
				}
				else
				{
					immediate = true;
				}
			}
			if (equipment)
			{
				equipment.IsEquipped = false;
			}
			base.StartCoroutine(this.holderManager.UnequipRoutine(equipment.referenceItem.disableDelayTime, immediate, onStart, onFinish, ""));
		}

		// Token: 0x04001ECF RID: 7887
		[vEditorToolbar("Default", false, "", false, false)]
		public bool hideWeaponsAutomatically;

		// Token: 0x04001ED0 RID: 7888
		[vHideInInspector("hideWeaponsAutomatically", false)]
		public float hideWeaponsTimer = 5f;

		// Token: 0x04001ED1 RID: 7889
		[vHelpBox("Set Lock input to Inventory when Lock method is called", vHelpBoxAttribute.MessageType.None)]
		public bool lockInventoryInputOnLock;

		// Token: 0x04001ED2 RID: 7890
		[vReadOnly(true)]
		public bool isLocked;

		// Token: 0x04001ED3 RID: 7891
		public GenericInput hideAndDrawWeaponsInput = new GenericInput("H", "LB", "LB");

		// Token: 0x04001ED4 RID: 7892
		[vEditorToolbar("Melee", false, "", false, false)]
		[Header("Draw Immediate Conditions")]
		public bool meleeWeakAttack = true;

		// Token: 0x04001ED5 RID: 7893
		public bool meleeStrongAttack = true;

		// Token: 0x04001ED6 RID: 7894
		public bool meleeBlock = true;

		// Token: 0x04001ED7 RID: 7895
		[vEditorToolbar("Debug", false, "", false, false)]
		[vReadOnly(false)]
		public bool weaponsHided;

		// Token: 0x04001ED8 RID: 7896
		[vReadOnly(false)]
		public bool previouslyWeaponsHided;

		// Token: 0x04001ED9 RID: 7897
		protected float currentTimer;

		// Token: 0x04001EDA RID: 7898
		protected bool forceHide;
	}
}
