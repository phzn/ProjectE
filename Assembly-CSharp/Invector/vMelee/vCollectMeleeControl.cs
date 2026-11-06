using System;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vMelee
{
	// Token: 0x02000508 RID: 1288
	[vClassHeader("Collect Melee Control", "This component is used when you're character doesn't have a ItemManager to manage items, this will allow you to pickup 1 weapon at the time.")]
	public class vCollectMeleeControl : vMonoBehaviour
	{
		// Token: 0x06001F48 RID: 8008 RVA: 0x000BA3D4 File Offset: 0x000B85D4
		protected virtual void Start()
		{
			this.meleeManager = base.GetComponent<vMeleeManager>();
			if (this.controlDisplayPrefab)
			{
				this.currentDisplay = Object.Instantiate<vControlDisplayWeaponStandalone>(this.controlDisplayPrefab);
			}
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x000BA400 File Offset: 0x000B8600
		protected virtual void Update()
		{
			this.UnequipWeaponHandle();
			this.CheckIsEquipedWifhWeapon();
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x000BA40E File Offset: 0x000B860E
		public virtual void HandleCollectableInput(vCollectableStandalone collectableStandAlone)
		{
			if (!this.meleeManager)
			{
				return;
			}
			if (collectableStandAlone != null && collectableStandAlone.weapon != null)
			{
				this.EquipMeleeWeapon(collectableStandAlone);
			}
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x000BA43C File Offset: 0x000B863C
		protected virtual void EquipMeleeWeapon(vCollectableStandalone collectable)
		{
			vMeleeWeapon component = collectable.weapon.GetComponent<vMeleeWeapon>();
			if (!component)
			{
				return;
			}
			if (component.meleeType != vMeleeType.OnlyDefense)
			{
				Transform equipPoint = this.GetEquipPoint(this.rightHandler, collectable.targetEquipPoint);
				if (!equipPoint)
				{
					return;
				}
				collectable.weapon.transform.SetParent(equipPoint);
				collectable.weapon.transform.localPosition = Vector3.zero;
				collectable.weapon.transform.localEulerAngles = Vector3.zero;
				if (this.rightWeapon && this.rightWeapon.gameObject != collectable.gameObject)
				{
					this.RemoveRightWeapon();
				}
				if (collectable.twoHandWeapon || (this.leftWeapon && this.leftWeapon.twoHandWeapon))
				{
					this.RemoveLeftWeapon();
				}
				this.meleeManager.SetRightWeapon(component.gameObject);
				collectable.OnEquip.Invoke();
				this.rightWeapon = collectable;
				this.onEquipRightWeapon.Invoke();
				this.UpdateRightDisplay(collectable);
			}
			if (component.meleeType != vMeleeType.OnlyAttack && component.meleeType != vMeleeType.AttackAndDefense)
			{
				Transform equipPoint2 = this.GetEquipPoint(this.leftHandler, collectable.targetEquipPoint);
				if (!equipPoint2)
				{
					return;
				}
				collectable.weapon.transform.SetParent(equipPoint2);
				collectable.weapon.transform.localPosition = Vector3.zero;
				collectable.weapon.transform.localEulerAngles = Vector3.zero;
				if (this.leftWeapon && this.leftWeapon.gameObject != collectable.gameObject)
				{
					this.RemoveLeftWeapon();
				}
				if (collectable.twoHandWeapon || (this.rightWeapon && this.rightWeapon.twoHandWeapon))
				{
					this.RemoveRightWeapon();
				}
				this.onEquipLeftWeapon.Invoke();
				this.meleeManager.SetLeftWeapon(component.gameObject);
				collectable.OnEquip.Invoke();
				this.leftWeapon = collectable;
				this.UpdateLeftDisplay(collectable);
			}
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x000BA640 File Offset: 0x000B8840
		protected virtual Transform GetEquipPoint(vHandler point, string name)
		{
			Transform transform = point.defaultHandler;
			Transform transform2 = point.customHandlers.Find((Transform _p) => _p.name.Equals(name));
			if (transform2)
			{
				transform = transform2;
			}
			return transform;
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x000BA684 File Offset: 0x000B8884
		protected virtual void UnequipWeaponHandle()
		{
			if (this.rightWeapon && this.unequipRightInput.GetButtonDown())
			{
				this.RemoveRightWeapon();
			}
			if (this.leftWeapon && this.unequipLeftInput.GetButtonDown())
			{
				this.RemoveLeftWeapon();
			}
		}

		// Token: 0x06001F4E RID: 8014 RVA: 0x000BA6D4 File Offset: 0x000B88D4
		public virtual void RemoveLeftWeapon()
		{
			if (this.leftWeapon)
			{
				this.leftWeapon.weapon.transform.parent = null;
				this.leftWeapon.OnDrop.Invoke();
				this.onUnEquipLeftWeapon.Invoke();
			}
			if (this.meleeManager)
			{
				this.meleeManager.leftWeapon = null;
			}
			this.UpdateLeftDisplay(null);
		}

		// Token: 0x06001F4F RID: 8015 RVA: 0x000BA740 File Offset: 0x000B8940
		public virtual void RemoveRightWeapon()
		{
			if (this.rightWeapon)
			{
				this.rightWeapon.weapon.transform.parent = null;
				this.rightWeapon.OnDrop.Invoke();
				this.onUnEquipRightWeapon.Invoke();
			}
			if (this.meleeManager)
			{
				this.meleeManager.rightWeapon = null;
			}
			this.UpdateRightDisplay(null);
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06001F50 RID: 8016 RVA: 0x000BA7AB File Offset: 0x000B89AB
		public virtual bool isUsingTwoHandWeapon
		{
			get
			{
				return (this.rightWeapon != null && this.rightWeapon.twoHandWeapon) || (this.leftWeapon != null && this.leftWeapon.twoHandWeapon);
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06001F51 RID: 8017 RVA: 0x000BA7E8 File Offset: 0x000B89E8
		public virtual bool isUsingMeleeWeapon
		{
			get
			{
				return this.meleeManager && ((this.meleeManager.leftWeapon && this.meleeManager.leftWeapon.gameObject.activeInHierarchy) || (this.meleeManager.rightWeapon && this.meleeManager.rightWeapon.gameObject.activeInHierarchy));
			}
		}

		// Token: 0x06001F52 RID: 8018 RVA: 0x000BA858 File Offset: 0x000B8A58
		protected virtual void CheckIsEquipedWifhWeapon()
		{
			if (this.wasUsingMeleeWeapon && !this.isUsingMeleeWeapon)
			{
				this.onUnequipMeleeWeapon.Invoke();
				this.wasUsingMeleeWeapon = false;
				return;
			}
			if (!this.wasUsingMeleeWeapon && this.isUsingMeleeWeapon)
			{
				this.onEquipMeleeWeapon.Invoke();
				this.wasUsingMeleeWeapon = true;
			}
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x000BA8AC File Offset: 0x000B8AAC
		protected virtual void UpdateLeftDisplay(vCollectableStandalone collectable = null)
		{
			if (!this.currentDisplay)
			{
				return;
			}
			if (collectable)
			{
				this.currentDisplay.SetLeftWeaponIcon(collectable.weaponIcon);
				this.currentDisplay.SetLeftWeaponText(collectable.weaponText);
				return;
			}
			this.currentDisplay.RemoveLeftWeaponIcon();
			this.currentDisplay.RemoveLeftWeaponText();
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x000BA908 File Offset: 0x000B8B08
		protected virtual void UpdateRightDisplay(vCollectableStandalone collectable = null)
		{
			if (!this.currentDisplay)
			{
				return;
			}
			if (collectable)
			{
				this.currentDisplay.SetRightWeaponIcon(collectable.weaponIcon);
				this.currentDisplay.SetRightWeaponText(collectable.weaponText);
				return;
			}
			this.currentDisplay.RemoveRightWeaponIcon();
			this.currentDisplay.RemoveRightWeaponText();
		}

		// Token: 0x04001FE9 RID: 8169
		[HideInInspector]
		public vMeleeManager meleeManager;

		// Token: 0x04001FEA RID: 8170
		[Header("Handlers")]
		public vHandler rightHandler = new vHandler();

		// Token: 0x04001FEB RID: 8171
		public vHandler leftHandler = new vHandler();

		// Token: 0x04001FEC RID: 8172
		[Header("Unequip Inputs")]
		public GenericInput unequipRightInput;

		// Token: 0x04001FED RID: 8173
		public GenericInput unequipLeftInput;

		// Token: 0x04001FEE RID: 8174
		[HideInInspector]
		public vCollectableStandalone leftWeapon;

		// Token: 0x04001FEF RID: 8175
		[HideInInspector]
		public vCollectableStandalone rightWeapon;

		// Token: 0x04001FF0 RID: 8176
		public vControlDisplayWeaponStandalone controlDisplayPrefab;

		// Token: 0x04001FF1 RID: 8177
		protected vControlDisplayWeaponStandalone currentDisplay;

		// Token: 0x04001FF2 RID: 8178
		[vEditorToolbar("Melee Events", false, "", false, false)]
		public UnityEvent onEquipMeleeWeapon;

		// Token: 0x04001FF3 RID: 8179
		[vEditorToolbar("Melee Events", false, "", false, false)]
		public UnityEvent onUnequipMeleeWeapon;

		// Token: 0x04001FF4 RID: 8180
		[vEditorToolbar("Melee Events", false, "", false, false)]
		public UnityEvent onEquipRightWeapon;

		// Token: 0x04001FF5 RID: 8181
		[vEditorToolbar("Melee Events", false, "", false, false)]
		public UnityEvent onEquipLeftWeapon;

		// Token: 0x04001FF6 RID: 8182
		[vEditorToolbar("Melee Events", false, "", false, false)]
		public UnityEvent onUnEquipRightWeapon;

		// Token: 0x04001FF7 RID: 8183
		[vEditorToolbar("Melee Events", false, "", false, false)]
		public UnityEvent onUnEquipLeftWeapon;

		// Token: 0x04001FF8 RID: 8184
		internal bool wasUsingMeleeWeapon;
	}
}
