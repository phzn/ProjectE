using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Invector.vItemManager
{
	// Token: 0x0200054C RID: 1356
	public class vEquipmentDisplay : vItemSlot
	{
		// Token: 0x0600208F RID: 8335 RVA: 0x000C016D File Offset: 0x000BE36D
		public virtual void SetLockToEquip(bool value)
		{
			if (value)
			{
				this.onSetLockToEquip.Invoke();
				return;
			}
			this.onUnlockToEquip.Invoke();
		}

		// Token: 0x06002090 RID: 8336 RVA: 0x000C018C File Offset: 0x000BE38C
		public virtual void ItemIdentifier(int identifier = 0, bool showIdentifier = false)
		{
			if (!this.slotIdentifier)
			{
				return;
			}
			if (showIdentifier)
			{
				if (this.slotIdentifier)
				{
					this.slotIdentifier.text = identifier.ToString();
				}
				this.onChangeIdentifier.Invoke(identifier.ToString());
				return;
			}
			if (this.slotIdentifier)
			{
				this.slotIdentifier.text = string.Empty;
			}
			this.onChangeIdentifier.Invoke(string.Empty);
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x000C0209 File Offset: 0x000BE409
		public override void AddItem(vItem item)
		{
			base.AddItem(item);
			this.hasItem = item != null;
		}

		// Token: 0x04002143 RID: 8515
		public Text slotIdentifier;

		// Token: 0x04002144 RID: 8516
		public InputField.OnChangeEvent onChangeIdentifier;

		// Token: 0x04002145 RID: 8517
		public UnityEvent onSelect;

		// Token: 0x04002146 RID: 8518
		public UnityEvent onDeselect;

		// Token: 0x04002147 RID: 8519
		public bool hasItem;

		// Token: 0x04002148 RID: 8520
		public UnityEvent onSetLockToEquip;

		// Token: 0x04002149 RID: 8521
		public UnityEvent onUnlockToEquip;
	}
}
