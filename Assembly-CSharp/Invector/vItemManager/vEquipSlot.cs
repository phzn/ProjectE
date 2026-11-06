using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Invector.vItemManager
{
	// Token: 0x02000551 RID: 1361
	public class vEquipSlot : vItemSlot
	{
		// Token: 0x0600209C RID: 8348 RVA: 0x000C033C File Offset: 0x000BE53C
		public virtual void SetLockToEquip(bool value)
		{
			if (value)
			{
				this.onSetLockToEquip.Invoke();
				return;
			}
			this.onUnlockToEquip.Invoke();
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x000C0358 File Offset: 0x000BE558
		public override void AddItem(vItem item)
		{
			if (item)
			{
				item.isInEquipArea = true;
			}
			base.AddItem(item);
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x000C0370 File Offset: 0x000BE570
		public override void CheckItem(bool value)
		{
			if (this.checkIcon && this.checkIcon.gameObject.activeSelf)
			{
				this.checkIcon.gameObject.SetActive(false);
			}
		}

		// Token: 0x0600209F RID: 8351 RVA: 0x000C03A2 File Offset: 0x000BE5A2
		public override void RemoveItem()
		{
			if (this.item != null)
			{
				this.item.isInEquipArea = false;
			}
			base.RemoveItem();
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x000C03C4 File Offset: 0x000BE5C4
		public virtual void OnCancel()
		{
			this.onCancel.Invoke();
		}

		// Token: 0x060020A1 RID: 8353 RVA: 0x000C03D1 File Offset: 0x000BE5D1
		public override void OnDeselect(BaseEventData eventData)
		{
			if (this.autoDeselect)
			{
				base.OnDeselect(eventData);
			}
		}

		// Token: 0x060020A2 RID: 8354 RVA: 0x000C03E2 File Offset: 0x000BE5E2
		public override void OnPointerExit(PointerEventData eventData)
		{
			if (this.autoDeselect)
			{
				base.OnPointerExit(eventData);
			}
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x000C03F3 File Offset: 0x000BE5F3
		public override void OnPointerClick(PointerEventData eventData)
		{
			if (this.clickToOpen)
			{
				base.OnPointerClick(eventData);
			}
		}

		// Token: 0x04002152 RID: 8530
		[vEditorToolbar("Default", false, "", false, false)]
		[vHelpBox("Select what ItemType this EquipSlot will equip", vHelpBoxAttribute.MessageType.Warning)]
		public List<vItemType> itemType;

		// Token: 0x04002153 RID: 8531
		public bool clickToOpen = true;

		// Token: 0x04002154 RID: 8532
		public bool autoDeselect = true;

		// Token: 0x04002155 RID: 8533
		public UnityEvent onCancel;

		// Token: 0x04002156 RID: 8534
		public UnityEvent onSetLockToEquip;

		// Token: 0x04002157 RID: 8535
		public UnityEvent onUnlockToEquip;
	}
}
