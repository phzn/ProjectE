using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x020004D8 RID: 1240
	[vClassHeader("Weapon Holder", true, "icon_v2", false, "", openClose = false)]
	public class vWeaponHolder : vMonoBehaviour
	{
		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001DE1 RID: 7649 RVA: 0x000B6B78 File Offset: 0x000B4D78
		public bool inUse
		{
			get
			{
				return this.isHolderActve && !this.isWeaponActive;
			}
		}

		// Token: 0x06001DE2 RID: 7650 RVA: 0x000B6B8D File Offset: 0x000B4D8D
		public void SetActiveHolder(bool active)
		{
			if (this.holderObject)
			{
				this.holderObject.SetActive(active);
			}
			this.isHolderActve = active;
		}

		// Token: 0x06001DE3 RID: 7651 RVA: 0x000B6BAF File Offset: 0x000B4DAF
		public void SetActiveWeapon(bool active)
		{
			if (this.weaponObject)
			{
				this.weaponObject.SetActive(active);
			}
			this.isWeaponActive = active;
		}

		// Token: 0x04001EF1 RID: 7921
		[Tooltip("add LeftArm or RightArm, you can create new EquipPoints on the ItemManager")]
		public string equipPointName;

		// Token: 0x04001EF2 RID: 7922
		[Tooltip("Check the ItemID of this item on the Inventory Window")]
		public int itemID;

		// Token: 0x04001EF3 RID: 7923
		[Tooltip("The Holder object is just the weapon mesh without any colliders or components")]
		public GameObject holderObject;

		// Token: 0x04001EF4 RID: 7924
		[Tooltip("The Weapon object is the prefab of your weapon, you can find examples inside the folder Prefabs > Weapons")]
		public GameObject weaponObject;

		// Token: 0x04001EF5 RID: 7925
		[HideInInspector]
		public float equipDelayTime;

		// Token: 0x04001EF6 RID: 7926
		private bool isHolderActve;

		// Token: 0x04001EF7 RID: 7927
		private bool isWeaponActive;
	}
}
