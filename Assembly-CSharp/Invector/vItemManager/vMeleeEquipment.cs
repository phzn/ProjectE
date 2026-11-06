using System;
using Invector.vMelee;

namespace Invector.vItemManager
{
	// Token: 0x020005E3 RID: 1507
	[vClassHeader("Melee Equipment", true, "icon_v2", false, "", openClose = false, useHelpBox = true, helpBoxText = "Use this component if you also use the ItemManager in your Character")]
	public class vMeleeEquipment : vEquipment
	{
		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x0600230B RID: 8971 RVA: 0x000C8812 File Offset: 0x000C6A12
		protected virtual vMeleeWeapon meleeWeapon
		{
			get
			{
				if (!this._weapon && !this.withoutMeleeWeapon)
				{
					this._weapon = base.GetComponent<vMeleeWeapon>();
					if (!this._weapon)
					{
						this.withoutMeleeWeapon = true;
					}
				}
				return this._weapon;
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x0600230C RID: 8972 RVA: 0x000C884F File Offset: 0x000C6A4F
		// (set) Token: 0x0600230D RID: 8973 RVA: 0x000C8857 File Offset: 0x000C6A57
		public override bool IsEquipped
		{
			get
			{
				return base.IsEquipped;
			}
			set
			{
				base.IsEquipped = value;
				if (this.meleeWeapon)
				{
					this.meleeWeapon.IsEquipped = value;
				}
			}
		}

		// Token: 0x0600230E RID: 8974 RVA: 0x000C887C File Offset: 0x000C6A7C
		public override void OnEquip(vItem item)
		{
			if (this.meleeWeapon)
			{
				vItemAttribute itemAttribute = item.GetItemAttribute(vItemAttributes.Damage);
				vItemAttribute itemAttribute2 = item.GetItemAttribute(vItemAttributes.StaminaCost);
				vItemAttribute itemAttribute3 = item.GetItemAttribute(vItemAttributes.DefenseRate);
				bool itemAttribute4 = item.GetItemAttribute(vItemAttributes.DefenseRange) != null;
				if (itemAttribute != null)
				{
					this.meleeWeapon.damage.damageValue = (float)itemAttribute.value;
				}
				if (itemAttribute2 != null)
				{
					this.meleeWeapon.staminaCost = (float)itemAttribute2.value;
				}
				if (itemAttribute3 != null)
				{
					this.meleeWeapon.defenseRate = itemAttribute3.value;
				}
				if (itemAttribute4)
				{
					this.meleeWeapon.defenseRange = (float)itemAttribute3.value;
				}
			}
			base.OnEquip(item);
		}

		// Token: 0x0600230F RID: 8975 RVA: 0x000C8913 File Offset: 0x000C6B13
		public override void OnUnequip(vItem item)
		{
			base.OnUnequip(item);
		}

		// Token: 0x04002342 RID: 9026
		private vMeleeWeapon _weapon;

		// Token: 0x04002343 RID: 9027
		protected bool withoutMeleeWeapon;
	}
}
