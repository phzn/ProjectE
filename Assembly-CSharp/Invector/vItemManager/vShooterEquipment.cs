using System;
using Invector.vShooter;

namespace Invector.vItemManager
{
	// Token: 0x020005FA RID: 1530
	[vClassHeader("Shooter Equipment", true, "icon_v2", false, "", openClose = false, useHelpBox = true, helpBoxText = "Use this component if you also use the ItemManager in your Character")]
	public class vShooterEquipment : vMeleeEquipment
	{
		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x0600235F RID: 9055 RVA: 0x000C9655 File Offset: 0x000C7855
		public virtual vEquipment secondaryEquipment
		{
			get
			{
				return this._secondaryEquipment;
			}
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06002360 RID: 9056 RVA: 0x000C965D File Offset: 0x000C785D
		public virtual vShooterWeapon shooterWeapon
		{
			get
			{
				if (!this._shooter && !this.withoutShooterWeapon)
				{
					this._shooter = base.GetComponent<vShooterWeapon>();
					if (!this._shooter)
					{
						this.withoutShooterWeapon = true;
					}
				}
				return this._shooter;
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x06002361 RID: 9057 RVA: 0x000C969A File Offset: 0x000C789A
		// (set) Token: 0x06002362 RID: 9058 RVA: 0x000C96A2 File Offset: 0x000C78A2
		public override bool IsEquipped
		{
			get
			{
				return base.IsEquipped;
			}
			set
			{
				base.IsEquipped = value;
				if (this.shooterWeapon)
				{
					this.shooterWeapon.IsEquipped = value;
				}
			}
		}

		// Token: 0x06002363 RID: 9059 RVA: 0x000C96C4 File Offset: 0x000C78C4
		public override void OnEquip(vItem item)
		{
			if (this.shooterWeapon)
			{
				this.shooterWeapon.changeAmmoHandle = new vShooterWeapon.ChangeAmmoHandle(this.ChangeAmmo);
				this.shooterWeapon.checkAmmoHandle = new vShooterWeapon.CheckAmmoHandle(this.CheckAmmo);
				vItemAttribute itemAttribute = item.GetItemAttribute(vItemAttributes.Damage);
				if (itemAttribute != null)
				{
					this.shooterWeapon.maxDamage = itemAttribute.value;
				}
				if (this.secondaryEquipment)
				{
					this.secondaryEquipment.OnEquip(item);
				}
			}
			base.OnEquip(item);
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x000C974C File Offset: 0x000C794C
		public override void OnUnequip(vItem item)
		{
			if (this.shooterWeapon)
			{
				this.shooterWeapon.changeAmmoHandle = null;
				this.shooterWeapon.checkAmmoHandle = null;
				if (this.secondaryEquipment)
				{
					this.secondaryEquipment.OnUnequip(item);
				}
			}
			base.OnUnequip(item);
		}

		// Token: 0x06002365 RID: 9061 RVA: 0x000C97A0 File Offset: 0x000C79A0
		protected virtual bool CheckAmmo(ref bool isValid, ref int totalAmmo)
		{
			if (!this.referenceItem)
			{
				return false;
			}
			vItemAttribute itemAttribute = this.referenceItem.GetItemAttribute(vItemAttributes.AmmoCount);
			isValid = itemAttribute != null && !itemAttribute.isBool;
			if (isValid)
			{
				totalAmmo = itemAttribute.value;
			}
			return isValid && itemAttribute.value > 0;
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x000C97F8 File Offset: 0x000C79F8
		protected virtual void ChangeAmmo(int value)
		{
			if (!this.referenceItem)
			{
				return;
			}
			vItemAttribute itemAttribute = this.referenceItem.GetItemAttribute(vItemAttributes.AmmoCount);
			if (itemAttribute != null)
			{
				itemAttribute.value += value;
			}
		}

		// Token: 0x04002377 RID: 9079
		protected vShooterWeapon _shooter;

		// Token: 0x04002378 RID: 9080
		protected vEquipment _secondaryEquipment;

		// Token: 0x04002379 RID: 9081
		protected bool withoutShooterWeapon;
	}
}
