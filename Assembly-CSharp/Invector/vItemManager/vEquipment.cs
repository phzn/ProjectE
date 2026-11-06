using System;
using UnityEngine;

namespace Invector.vItemManager
{
	// Token: 0x0200054B RID: 1355
	[vClassHeader("Equipment", true, "icon_v2", false, "", openClose = false, helpBoxText = "Use this component if you also use the ItemManager in your Character")]
	public class vEquipment : vMonoBehaviour
	{
		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06002085 RID: 8325 RVA: 0x000C0109 File Offset: 0x000BE309
		// (set) Token: 0x06002086 RID: 8326 RVA: 0x000C0111 File Offset: 0x000BE311
		public EquipPoint equipPoint { get; set; }

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x06002087 RID: 8327 RVA: 0x000C011A File Offset: 0x000BE31A
		// (set) Token: 0x06002088 RID: 8328 RVA: 0x000C0122 File Offset: 0x000BE322
		public virtual bool IsEquipped
		{
			get
			{
				return this.isEquipped;
			}
			set
			{
				this.isEquipped = value;
			}
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x00004F31 File Offset: 0x00003131
		public virtual void OnDestroy()
		{
		}

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x0600208A RID: 8330 RVA: 0x000C012B File Offset: 0x000BE32B
		// (set) Token: 0x0600208B RID: 8331 RVA: 0x000C0133 File Offset: 0x000BE333
		public virtual vItem referenceItem
		{
			get
			{
				return this.item;
			}
			protected set
			{
				this.item = value;
			}
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x000C013C File Offset: 0x000BE33C
		public virtual void OnEquip(vItem item)
		{
			this.IsEquipped = true;
			this.referenceItem = item;
			this.onEquip.Invoke(item);
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x000C0158 File Offset: 0x000BE358
		public virtual void OnUnequip(vItem item)
		{
			this.IsEquipped = false;
			this.onUnequip.Invoke(item);
		}

		// Token: 0x0400213E RID: 8510
		public OnHandleItemEvent onEquip;

		// Token: 0x0400213F RID: 8511
		public OnHandleItemEvent onUnequip;

		// Token: 0x04002140 RID: 8512
		[SerializeField]
		[vReadOnly(true)]
		protected vItem item;

		// Token: 0x04002141 RID: 8513
		[SerializeField]
		[vReadOnly(true)]
		protected bool isEquipped;
	}
}
