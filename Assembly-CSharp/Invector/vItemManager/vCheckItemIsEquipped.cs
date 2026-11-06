using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Invector.vItemManager
{
	// Token: 0x0200052B RID: 1323
	[vClassHeader("Check If Item Is Equipped", true, "icon_v2", false, "", openClose = false)]
	public class vCheckItemIsEquipped : vMonoBehaviour
	{
		// Token: 0x06001FFC RID: 8188 RVA: 0x000BDE28 File Offset: 0x000BC028
		private void Awake()
		{
			if (!this.itemManager)
			{
				if (this.getInParent)
				{
					this.itemManager = base.GetComponentInParent<vItemManager>();
				}
				else
				{
					this.itemManager = base.GetComponent<vItemManager>();
				}
				this.itemManager.onEquipItem.AddListener(new UnityAction<vEquipArea, vItem>(this.CheckIsEquipped));
				this.itemManager.onUnequipItem.AddListener(new UnityAction<vEquipArea, vItem>(this.CheckIsEquipped));
			}
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x000BDE9C File Offset: 0x000BC09C
		private void CheckIsEquipped(vEquipArea arg0, vItem arg1)
		{
			for (int i = 0; i < this.itemIDEvents.Count; i++)
			{
				vCheckItemIsEquipped.CheckItemIDEvent checkItemIDEvent = this.itemIDEvents[i];
				this.CheckItemID(checkItemIDEvent);
			}
			for (int j = 0; j < this.itemTypeEvents.Count; j++)
			{
				vCheckItemIsEquipped.CheckItemTypeEvent checkItemTypeEvent = this.itemTypeEvents[j];
				this.CheckItemType(checkItemTypeEvent);
			}
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x000BDF00 File Offset: 0x000BC100
		private void CheckItemID(vCheckItemIsEquipped.CheckItemIDEvent check)
		{
			bool flag = check._itemsID.Exists((int t) => this.itemManager.ItemIsEquipped(t));
			if (flag != check.isEquipped)
			{
				check.isEquipped = flag;
				if (check.isEquipped)
				{
					check.onIsItemEquipped.Invoke();
					return;
				}
				check.onIsItemUnequipped.Invoke();
			}
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x000BDF54 File Offset: 0x000BC154
		private void CheckItemType(vCheckItemIsEquipped.CheckItemTypeEvent check)
		{
			bool flag = check.itemTypes.Exists((vItemType t) => this.itemManager.ItemTypeIsEquipped(t));
			if (flag != check.isEquipped)
			{
				check.isEquipped = flag;
				if (check.isEquipped)
				{
					check.onIsItemEquipped.Invoke();
					return;
				}
				check.onIsItemUnequipped.Invoke();
			}
		}

		// Token: 0x040020C2 RID: 8386
		public vItemManager itemManager;

		// Token: 0x040020C3 RID: 8387
		public bool getInParent = true;

		// Token: 0x040020C4 RID: 8388
		[FormerlySerializedAs("itemChecks")]
		public List<vCheckItemIsEquipped.CheckItemIDEvent> itemIDEvents;

		// Token: 0x040020C5 RID: 8389
		public List<vCheckItemIsEquipped.CheckItemTypeEvent> itemTypeEvents;

		// Token: 0x0200052C RID: 1324
		[Serializable]
		public class CheckItemIDEvent
		{
			// Token: 0x040020C6 RID: 8390
			public string name;

			// Token: 0x040020C7 RID: 8391
			public List<int> _itemsID;

			// Token: 0x040020C8 RID: 8392
			public UnityEvent onIsItemEquipped;

			// Token: 0x040020C9 RID: 8393
			public UnityEvent onIsItemUnequipped;

			// Token: 0x040020CA RID: 8394
			internal bool isEquipped;
		}

		// Token: 0x0200052D RID: 1325
		[Serializable]
		public class CheckItemTypeEvent
		{
			// Token: 0x040020CB RID: 8395
			public string name;

			// Token: 0x040020CC RID: 8396
			public List<vItemType> itemTypes;

			// Token: 0x040020CD RID: 8397
			public UnityEvent onIsItemEquipped;

			// Token: 0x040020CE RID: 8398
			public UnityEvent onIsItemUnequipped;

			// Token: 0x040020CF RID: 8399
			internal bool isEquipped;
		}
	}
}
