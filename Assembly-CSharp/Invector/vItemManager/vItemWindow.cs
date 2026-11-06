using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Invector.vItemManager
{
	// Token: 0x020005CB RID: 1483
	[vClassHeader("Item Window", true, "icon_v2", false, "", openClose = false)]
	public class vItemWindow : vMonoBehaviour
	{
		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x0600227C RID: 8828 RVA: 0x000C6437 File Offset: 0x000C4637
		// (set) Token: 0x0600227D RID: 8829 RVA: 0x000C643F File Offset: 0x000C463F
		public vItemSlot currentSelectedSlot { get; set; }

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x0600227E RID: 8830 RVA: 0x000C6448 File Offset: 0x000C4648
		// (set) Token: 0x0600227F RID: 8831 RVA: 0x000C6450 File Offset: 0x000C4650
		protected UnityAction<vItemSlot> onSubmitSlotCallback { get; set; }

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06002280 RID: 8832 RVA: 0x000C6459 File Offset: 0x000C4659
		// (set) Token: 0x06002281 RID: 8833 RVA: 0x000C6461 File Offset: 0x000C4661
		protected UnityAction<vItemSlot> onSelectCallback { get; set; }

		// Token: 0x06002282 RID: 8834 RVA: 0x000C646C File Offset: 0x000C466C
		public virtual void ReloadItems(List<vItem> items)
		{
			int num = (this.slots.Contains(this.currentSelectedSlot) ? this.slots.IndexOf(this.currentSelectedSlot) : 0);
			for (int i = 0; i < this.slots.Count; i++)
			{
				if (i >= 0 && i < this.slots.Count)
				{
					if (this.slots[i] != null && (this.slots[i].item == null || !items.Contains(this.slots[i].item)))
					{
						Object.Destroy(this.slots[i].gameObject);
						this.slots.Remove(this.slots[i]);
						if (i == num)
						{
							this.currentSelectedSlot = ((i - 1 >= 0) ? this.slots[i - 1] : ((this.slots.Count - 1 > 0) ? this.slots[0] : null));
							if (this.currentSelectedSlot != null)
							{
								this.CreateFullItemDescription(this.currentSelectedSlot);
							}
						}
						i--;
					}
					else if (this.slots[i] == null)
					{
						this.slots.RemoveAt(i);
						i--;
					}
				}
			}
			if (this.currentSelectedSlot == null || this.currentSelectedSlot.item == null || this.slots.Count == 0)
			{
				this.CreateFullItemDescription(null);
				if (this.slots.Count == 0)
				{
					this.onClearSlots.Invoke();
					return;
				}
			}
			else
			{
				this.CreateFullItemDescription(this.currentSelectedSlot);
			}
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x000C662C File Offset: 0x000C482C
		public virtual void CreateEquipmentWindow(List<vItem> items, UnityAction<vItemSlot> onPickUpItemCallBack = null, UnityAction<vItemSlot> onSelectSlotCallBack = null, bool destroyAdictionSlots = true)
		{
			base.StartCoroutine(this.CreateEquipmentWindowRoutine(items, onPickUpItemCallBack, onSelectSlotCallBack, destroyAdictionSlots));
		}

		// Token: 0x06002284 RID: 8836 RVA: 0x000C6640 File Offset: 0x000C4840
		public virtual void CreateEquipmentWindow(List<vItem> items, List<vItemType> type, vItem currentItem = null, UnityAction<vItemSlot> onPickUpItemCallback = null, UnityAction<vItemSlot> onSelectSlotCallBack = null)
		{
			this.currentItem = currentItem;
			List<vItem> list = items.FindAll((vItem item) => type.Contains(item.type) && (item.canBeEquipped || !this.isEquipmentWindow));
			base.StartCoroutine(this.CreateEquipmentWindowRoutine(list, onPickUpItemCallback, null, true));
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x000C668D File Offset: 0x000C488D
		protected virtual IEnumerator CreateEquipmentWindowRoutine(List<vItem> items, UnityAction<vItemSlot> onPickUpItemCallBack = null, UnityAction<vItemSlot> onSelectSlotCallBack = null, bool destroyAdictionSlots = true)
		{
			List<vItem> _items = ((this.supportedItems.Count == 0) ? items : items.FindAll((vItem i) => this.supportedItems.Contains(i.type)));
			if (_items.Count == 0)
			{
				this.CreateFullItemDescription(null);
				this.onClearSlots.Invoke();
				if (this.slots.Count > 0 && destroyAdictionSlots && this.updateSlotCount)
				{
					int num;
					for (int k = 0; k < this.slots.Count; k = num + 1)
					{
						yield return null;
						Object.Destroy(this.slots[k].gameObject);
						num = k;
					}
					this.slots.Clear();
				}
			}
			else
			{
				int num;
				if (this.slots.Count > _items.Count && destroyAdictionSlots && this.updateSlotCount)
				{
					int k = this.slots.Count - _items.Count;
					for (int j = 0; j < k; j = num + 1)
					{
						yield return null;
						Object.Destroy(this.slots[0].gameObject);
						this.slots.RemoveAt(0);
						num = j;
					}
				}
				bool selecItem = false;
				this.onSubmitSlotCallback = onPickUpItemCallBack;
				this.onSelectCallback = onSelectSlotCallBack;
				if (this.slots == null)
				{
					this.slots = new List<vItemSlot>();
				}
				int count = items.Count;
				for (int k = 0; k < _items.Count; k = num + 1)
				{
					vItemSlot slot = null;
					if (k < this.slots.Count)
					{
						slot = this.slots[k];
					}
					else
					{
						slot = Object.Instantiate<vItemSlot>(this.slotPrefab);
						this.slots.Add(slot);
						RectTransform component = slot.GetComponent<RectTransform>();
						component.SetParent(this.contentWindow);
						component.localPosition = Vector3.zero;
						component.localScale = Vector3.one;
						yield return null;
					}
					slot.AddItem(_items[k]);
					slot.CheckItem(_items[k].isInEquipArea);
					slot.onSubmitSlotCallBack = new ItemSlotEvent(this.OnSubmit);
					slot.onSelectSlotCallBack = new ItemSlotEvent(this.OnSelect);
					if (this.currentItem != null && this.currentItem == _items[k])
					{
						selecItem = true;
						this.currentSelectedSlot = slot;
						this.SetSelectable(slot.gameObject);
					}
					slot.UpdateDisplays();
					slot = null;
					num = k;
				}
				if (this.slots.Count > 0 && !selecItem)
				{
					this.currentSelectedSlot = this.slots[0];
					base.StartCoroutine(this.SetSelectableHandle(this.slots[0].gameObject));
				}
			}
			if (this.slots.Count > 0)
			{
				this.onAddSlots.Invoke();
				this.CreateFullItemDescription(this.currentSelectedSlot);
			}
			this.onCompleteSlotListCallBack.Invoke(this.slots);
			yield break;
		}

		// Token: 0x06002286 RID: 8838 RVA: 0x000C66B9 File Offset: 0x000C48B9
		public virtual IEnumerator SetSelectableHandle(GameObject target)
		{
			if (base.enabled)
			{
				yield return this.WaitForEndOfFrame;
				this.SetSelectable(target);
			}
			yield break;
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x000C66D0 File Offset: 0x000C48D0
		public virtual void SetSelectable(GameObject target)
		{
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			ExecuteEvents.Execute<IPointerExitHandler>(EventSystem.current.currentSelectedGameObject, pointerEventData, ExecuteEvents.pointerExitHandler);
			EventSystem.current.SetSelectedGameObject(target, new BaseEventData(EventSystem.current));
			ExecuteEvents.Execute<ISelectHandler>(target, pointerEventData, ExecuteEvents.selectHandler);
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x000C6720 File Offset: 0x000C4920
		public virtual void OnSubmit(vItemSlot slot)
		{
			this.currentSelectedSlot = slot;
			UnityAction<vItemSlot> onSubmitSlotCallback = this.onSubmitSlotCallback;
			if (onSubmitSlotCallback != null)
			{
				onSubmitSlotCallback(slot);
			}
			this.onSubmitSlot.Invoke(slot);
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x000C6747 File Offset: 0x000C4947
		public virtual void OnSelect(vItemSlot slot)
		{
			this.currentSelectedSlot = slot;
			this.CreateFullItemDescription(slot);
			UnityAction<vItemSlot> onSelectCallback = this.onSelectCallback;
			if (onSelectCallback != null)
			{
				onSelectCallback(slot);
			}
			this.onSelectSlot.Invoke(slot);
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x000C6778 File Offset: 0x000C4978
		protected virtual void CreateFullItemDescription(vItemSlot slot)
		{
			string text = ((slot && slot.item) ? slot.item.name : "");
			string text2 = ((slot && slot.item) ? slot.item.ItemTypeText() : "");
			string text3 = ((slot && slot.item) ? slot.item.amount.ToString() : "");
			string text4 = ((slot && slot.item) ? slot.item.description : "");
			string text5 = ((slot && slot.item) ? slot.item.GetItemAttributesText(this.ignoreAttributes) : "");
			if (this.displayNameText)
			{
				this.displayNameText.text = text;
			}
			this.onChangeName.Invoke(text);
			if (this.displayTypeText)
			{
				this.displayTypeText.text = text2;
			}
			this.onChangeType.Invoke(text2);
			if (this.displayAmountText)
			{
				this.displayAmountText.text = text3;
			}
			this.onChangeAmount.Invoke(text3);
			if (this.displayDescriptionText)
			{
				this.displayDescriptionText.text = text4;
			}
			this.onChangeDescription.Invoke(text4);
			if (this.displayAttributesText)
			{
				this.displayAttributesText.text = text5;
			}
			this.onChangeAttributes.Invoke(text5);
		}

		// Token: 0x0600228B RID: 8843 RVA: 0x000C6914 File Offset: 0x000C4B14
		public virtual void OnCancel()
		{
			this.onCancelSlot.Invoke();
		}

		// Token: 0x040022C2 RID: 8898
		[vReadOnly(true)]
		public vItem currentItem;

		// Token: 0x040022C3 RID: 8899
		public vItemSlot slotPrefab;

		// Token: 0x040022C4 RID: 8900
		public RectTransform contentWindow;

		// Token: 0x040022C5 RID: 8901
		public List<vItemSlot> slots;

		// Token: 0x040022C6 RID: 8902
		public List<vItemType> supportedItems;

		// Token: 0x040022C7 RID: 8903
		public bool updateSlotCount = true;

		// Token: 0x040022C8 RID: 8904
		public bool isEquipmentWindow;

		// Token: 0x040022C9 RID: 8905
		public Text displayNameText;

		// Token: 0x040022CA RID: 8906
		public Text displayTypeText;

		// Token: 0x040022CB RID: 8907
		public Text displayAmountText;

		// Token: 0x040022CC RID: 8908
		public Text displayDescriptionText;

		// Token: 0x040022CD RID: 8909
		public Text displayAttributesText;

		// Token: 0x040022CE RID: 8910
		[vHelpBox("You can ignore display Attributes using this property", vHelpBoxAttribute.MessageType.None)]
		public List<vItemAttributes> ignoreAttributes;

		// Token: 0x040022CF RID: 8911
		[vEditorToolbar("Text Events", false, "", false, false)]
		public InputField.OnChangeEvent onChangeName;

		// Token: 0x040022D0 RID: 8912
		public InputField.OnChangeEvent onChangeType;

		// Token: 0x040022D1 RID: 8913
		public InputField.OnChangeEvent onChangeAmount;

		// Token: 0x040022D2 RID: 8914
		public InputField.OnChangeEvent onChangeDescription;

		// Token: 0x040022D3 RID: 8915
		public InputField.OnChangeEvent onChangeAttributes;

		// Token: 0x040022D4 RID: 8916
		[vEditorToolbar("Events", false, "", false, false)]
		public OnCompleteSlotList onCompleteSlotListCallBack;

		// Token: 0x040022D5 RID: 8917
		public OnHandleSlot onSubmitSlot;

		// Token: 0x040022D6 RID: 8918
		public OnHandleSlot onSelectSlot;

		// Token: 0x040022D7 RID: 8919
		public UnityEvent onCancelSlot;

		// Token: 0x040022D8 RID: 8920
		[Tooltip("Called when item window has slots on enable")]
		public UnityEvent onAddSlots;

		// Token: 0x040022D9 RID: 8921
		[Tooltip("Called when item window dont have slots on enable")]
		public UnityEvent onClearSlots;

		// Token: 0x040022DD RID: 8925
		protected readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
	}
}
