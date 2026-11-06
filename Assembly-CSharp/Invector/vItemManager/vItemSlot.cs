using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Invector.vItemManager
{
	// Token: 0x020005C6 RID: 1478
	[vClassHeader("Item Slot", true, "icon_v2", false, "", openClose = false)]
	public class vItemSlot : vMonoBehaviour, IPointerClickHandler, IEventSystemHandler, ISelectHandler, IDeselectHandler, ISubmitHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x0600225F RID: 8799 RVA: 0x000C5CF0 File Offset: 0x000C3EF0
		protected virtual void OnEnable()
		{
			this.onEnable.Invoke();
			this.UpdateDisplays(this.item);
		}

		// Token: 0x06002260 RID: 8800 RVA: 0x000C5D09 File Offset: 0x000C3F09
		protected virtual void OnDisable()
		{
			this.onDisable.Invoke();
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x000C5D18 File Offset: 0x000C3F18
		protected virtual void Start()
		{
			vInventory componentInParent = base.GetComponentInParent<vInventory>();
			if (componentInParent)
			{
				componentInParent.OnUpdateInventory += this.UpdateDisplays;
			}
			this.selectable = base.GetComponent<Selectable>();
			this.SetValid(this.isValid);
		}

		// Token: 0x06002262 RID: 8802 RVA: 0x000C5D5F File Offset: 0x000C3F5F
		public virtual void UpdateDisplays()
		{
			this.UpdateDisplays(this.item);
		}

		// Token: 0x06002263 RID: 8803 RVA: 0x000C5D70 File Offset: 0x000C3F70
		protected virtual void OnDestroy()
		{
			vInventory componentInParent = base.GetComponentInParent<vInventory>();
			if (componentInParent)
			{
				componentInParent.OnUpdateInventory -= this.UpdateDisplays;
			}
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x000C5D9F File Offset: 0x000C3F9F
		public virtual void CheckItem(bool value)
		{
			this.isChecked = value;
			if (this.checkIcon)
			{
				this.checkIcon.gameObject.SetActive(this.isChecked);
			}
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x000C5DCC File Offset: 0x000C3FCC
		public virtual void SetValid(bool value)
		{
			this.isValid = value;
			if (this.selectable)
			{
				this.selectable.interactable = value;
			}
			if (this.blockIcon == null)
			{
				return;
			}
			this.blockIcon.color = (value ? Color.clear : Color.white);
			this.blockIcon.SetAllDirty();
			this.isValid = value;
		}

		// Token: 0x06002266 RID: 8806 RVA: 0x000C5E34 File Offset: 0x000C4034
		public virtual void AddItem(vItem item)
		{
			if (item != null)
			{
				this.onAddItem.Invoke(item);
				this.item = item;
				this.UpdateDisplays(item);
				return;
			}
			this.RemoveItem();
		}

		// Token: 0x06002267 RID: 8807 RVA: 0x000C5E60 File Offset: 0x000C4060
		protected virtual void UpdateDisplays(vItem item)
		{
			this.ChangeDisplayIcon(item);
			this.ChangeDisplayName(item);
			this.ChangeDisplayType(item);
			this.ChangeDisplayAmount(item);
			this.ChangeDisplayDescription(item);
			this.ChangeDisplayAttributes(item);
			this.CheckItem(item != null && item.isInEquipArea);
		}

		// Token: 0x06002268 RID: 8808 RVA: 0x000C5EB0 File Offset: 0x000C40B0
		protected virtual void ChangeDisplayType(vItem item)
		{
			if (item)
			{
				this.onChangeType.Invoke(item.ItemTypeText());
				if (this.displayTypeText)
				{
					this.displayTypeText.text = item.ItemTypeText();
					return;
				}
			}
			else
			{
				this.onChangeType.Invoke("");
				if (this.displayTypeText)
				{
					this.displayTypeText.text = "";
				}
			}
		}

		// Token: 0x06002269 RID: 8809 RVA: 0x000C5F24 File Offset: 0x000C4124
		protected virtual void ChangeDisplayAttributes(vItem item)
		{
			if (item)
			{
				if (this.displayAttributesText)
				{
					this.displayAttributesText.text = item.GetItemAttributesText(this.ignoreAttributes);
				}
				this.onChangeAttributes.Invoke(item.GetItemAttributesText(this.ignoreAttributes));
				int i;
				Predicate<vItemSlot.AttributeDisplay> <>9__0;
				int k;
				for (i = 0; i < item.attributes.Count; i = k + 1)
				{
					List<vItemSlot.AttributeDisplay> list = this.customAttributeDisplay;
					Predicate<vItemSlot.AttributeDisplay> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = (vItemSlot.AttributeDisplay att) => att.name.Equals(item.attributes[i].name));
					}
					vItemSlot.AttributeDisplay attributeDisplay = list.Find(predicate);
					if (attributeDisplay != null)
					{
						string displayText = item.attributes[i].GetDisplayText(null);
						if (attributeDisplay.text)
						{
							attributeDisplay.text.text = displayText;
						}
						attributeDisplay.onChangeDisplay.Invoke(displayText);
					}
					k = i;
				}
				return;
			}
			if (this.displayAttributesText)
			{
				this.displayAttributesText.text = "";
			}
			this.onChangeAttributes.Invoke("");
			for (int j = 0; j < this.customAttributeDisplay.Count; j++)
			{
				if (this.customAttributeDisplay[j].text)
				{
					this.customAttributeDisplay[j].text.text = "";
				}
				this.customAttributeDisplay[j].onChangeDisplay.Invoke("");
			}
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x000C60D8 File Offset: 0x000C42D8
		protected virtual void ChangeDisplayIcon(vItem item)
		{
			if (this.icon && item)
			{
				this.icon.sprite = item.icon;
				this.color.a = 1f;
				this.icon.color = this.color;
			}
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x000C612C File Offset: 0x000C432C
		protected virtual void ChangeDisplayDescription(vItem item)
		{
			if (item)
			{
				this.onChangeDescription.Invoke(item.description);
				if (this.displayDescriptionText)
				{
					this.displayDescriptionText.text = item.description;
					return;
				}
			}
			else
			{
				this.onChangeDescription.Invoke("");
				if (this.displayDescriptionText)
				{
					this.displayDescriptionText.text = "";
				}
			}
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x000C61A0 File Offset: 0x000C43A0
		protected virtual void ChangeDisplayAmount(vItem item)
		{
			string text = "";
			if (item != null && base.gameObject.activeSelf)
			{
				if (item.stackable)
				{
					text = "x" + item.amount.ToString();
				}
				else
				{
					text = "";
				}
			}
			else if (item == null)
			{
				text = "";
			}
			if (this.displayAmountText)
			{
				this.displayAmountText.text = text;
			}
			this.onChangeAmount.Invoke(text);
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x000C6228 File Offset: 0x000C4428
		protected virtual void ChangeDisplayName(vItem item)
		{
			if (item)
			{
				this.onChangeName.Invoke(item.name);
				if (this.displayNameText)
				{
					this.displayNameText.text = item.name;
					return;
				}
			}
			else
			{
				this.onChangeName.Invoke("");
				if (this.displayNameText)
				{
					this.displayNameText.text = "";
				}
			}
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x000C629C File Offset: 0x000C449C
		public virtual void RemoveItem()
		{
			this.onRemoveItem.Invoke(this.item);
			this.item = null;
			if (this.icon)
			{
				this.color.a = 0f;
				this.icon.color = this.color;
				this.icon.sprite = null;
				this.icon.SetAllDirty();
			}
			this.UpdateDisplays(null);
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x000C630D File Offset: 0x000C450D
		public virtual bool CheckIfSlotHasItem()
		{
			return this.item != null;
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x000C631B File Offset: 0x000C451B
		public virtual void OnSelect(BaseEventData eventData)
		{
			if (this.onSelectSlotCallBack != null)
			{
				this.onSelectSlotCallBack(this);
			}
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x000C6331 File Offset: 0x000C4531
		public virtual void OnDeselect(BaseEventData eventData)
		{
			if (this.onDeselectSlotCallBack != null)
			{
				this.onDeselectSlotCallBack(this);
			}
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x000C6347 File Offset: 0x000C4547
		public virtual void OnSubmit(BaseEventData eventData)
		{
			if (this.isValid)
			{
				this.onClick.Invoke();
				if (this.onSubmitSlotCallBack != null)
				{
					this.onSubmitSlotCallBack(this);
				}
			}
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x000C6370 File Offset: 0x000C4570
		public virtual void OnPointerEnter(PointerEventData eventData)
		{
			EventSystem.current.SetSelectedGameObject(base.gameObject);
			if (this.onSelectSlotCallBack != null)
			{
				this.onSelectSlotCallBack(this);
			}
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x000C6331 File Offset: 0x000C4531
		public virtual void OnPointerExit(PointerEventData eventData)
		{
			if (this.onDeselectSlotCallBack != null)
			{
				this.onDeselectSlotCallBack(this);
			}
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x000C6396 File Offset: 0x000C4596
		public virtual void OnPointerClick(PointerEventData eventData)
		{
			if (eventData.button == PointerEventData.InputButton.Left && this.isValid)
			{
				this.onClick.Invoke();
				if (this.onSubmitSlotCallBack != null)
				{
					this.onSubmitSlotCallBack(this);
				}
			}
		}

		// Token: 0x0400229F RID: 8863
		[vEditorToolbar("Default", false, "", false, false)]
		public vItem item;

		// Token: 0x040022A0 RID: 8864
		public bool isValid = true;

		// Token: 0x040022A1 RID: 8865
		[HideInInspector]
		public bool isChecked;

		// Token: 0x040022A2 RID: 8866
		[vEditorToolbar("Optional", false, "", false, false)]
		public Image icon;

		// Token: 0x040022A3 RID: 8867
		public Image blockIcon;

		// Token: 0x040022A4 RID: 8868
		public Image checkIcon;

		// Token: 0x040022A5 RID: 8869
		public Text displayNameText;

		// Token: 0x040022A6 RID: 8870
		public Text displayTypeText;

		// Token: 0x040022A7 RID: 8871
		public Text displayAmountText;

		// Token: 0x040022A8 RID: 8872
		public Text displayDescriptionText;

		// Token: 0x040022A9 RID: 8873
		public Text displayAttributesText;

		// Token: 0x040022AA RID: 8874
		[vHelpBox("You can ignore display Attributes using this property", vHelpBoxAttribute.MessageType.None)]
		public List<vItemAttributes> ignoreAttributes;

		// Token: 0x040022AB RID: 8875
		[vEditorToolbar("Events", false, "", false, false)]
		public InputField.OnChangeEvent onChangeName;

		// Token: 0x040022AC RID: 8876
		public InputField.OnChangeEvent onChangeType;

		// Token: 0x040022AD RID: 8877
		public InputField.OnChangeEvent onChangeAmount;

		// Token: 0x040022AE RID: 8878
		public InputField.OnChangeEvent onChangeDescription;

		// Token: 0x040022AF RID: 8879
		public InputField.OnChangeEvent onChangeAttributes;

		// Token: 0x040022B0 RID: 8880
		public List<vItemSlot.AttributeDisplay> customAttributeDisplay;

		// Token: 0x040022B1 RID: 8881
		[vEditorToolbar("Events", false, "", false, false)]
		public ItemSlotEvent onSubmitSlotCallBack;

		// Token: 0x040022B2 RID: 8882
		[vEditorToolbar("Events", false, "", false, false)]
		public ItemSlotEvent onSelectSlotCallBack;

		// Token: 0x040022B3 RID: 8883
		[vEditorToolbar("Events", false, "", false, false)]
		public ItemSlotEvent onDeselectSlotCallBack;

		// Token: 0x040022B4 RID: 8884
		public OnHandleItemEvent onAddItem;

		// Token: 0x040022B5 RID: 8885
		public OnHandleItemEvent onRemoveItem;

		// Token: 0x040022B6 RID: 8886
		public UnityEvent onEnable;

		// Token: 0x040022B7 RID: 8887
		public UnityEvent onDisable;

		// Token: 0x040022B8 RID: 8888
		public UnityEvent onClick;

		// Token: 0x040022B9 RID: 8889
		protected Selectable selectable;

		// Token: 0x040022BA RID: 8890
		protected Color color = Color.white;

		// Token: 0x020005C7 RID: 1479
		[Serializable]
		public class AttributeDisplay
		{
			// Token: 0x040022BB RID: 8891
			public vItemAttributes name;

			// Token: 0x040022BC RID: 8892
			[Tooltip("Special Tags\n(NAME) = Display name of the Attribute\n(VALUE) = Display the value of the Attribute\n ***Keep Empty to use default attribute display***")]
			public string displayFormat = "(VALUE)";

			// Token: 0x040022BD RID: 8893
			public Text text;

			// Token: 0x040022BE RID: 8894
			public InputField.OnChangeEvent onChangeDisplay;
		}
	}
}
