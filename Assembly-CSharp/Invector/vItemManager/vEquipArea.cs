using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Invector.vItemManager
{
	// Token: 0x0200053F RID: 1343
	[vClassHeader("Equip Area", true, "icon_v2", false, "", openClose = false)]
	public class vEquipArea : vMonoBehaviour
	{
		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06002038 RID: 8248 RVA: 0x000BECCE File Offset: 0x000BCECE
		// (set) Token: 0x06002039 RID: 8249 RVA: 0x000BECD6 File Offset: 0x000BCED6
		[HideInInspector]
		public virtual vEquipSlot currentSelectedSlot { get; protected set; }

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x0600203A RID: 8250 RVA: 0x000BECDF File Offset: 0x000BCEDF
		// (set) Token: 0x0600203B RID: 8251 RVA: 0x000BECE7 File Offset: 0x000BCEE7
		public virtual vEquipSlot lastSelectedSlot { get; protected set; }

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x0600203C RID: 8252 RVA: 0x000BECF0 File Offset: 0x000BCEF0
		// (set) Token: 0x0600203D RID: 8253 RVA: 0x000BECF8 File Offset: 0x000BCEF8
		[HideInInspector]
		public virtual int indexOfEquippedItem
		{
			get
			{
				return this._indexOfEquippedSlot;
			}
			set
			{
				if (this._indexOfEquippedSlot != value || !this.equipSlotUpdated)
				{
					this.equipSlotUpdated = true;
					this._indexOfEquippedSlot = value;
					this.onChangeEquipSlot.Invoke(this, this.currentEquippedItem);
				}
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x0600203E RID: 8254 RVA: 0x000BED2B File Offset: 0x000BCF2B
		// (set) Token: 0x0600203F RID: 8255 RVA: 0x000BED33 File Offset: 0x000BCF33
		public virtual vItem lastEquippedItem { get; protected set; }

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x06002040 RID: 8256 RVA: 0x000BED3C File Offset: 0x000BCF3C
		// (set) Token: 0x06002041 RID: 8257 RVA: 0x000BED44 File Offset: 0x000BCF44
		protected bool _isLockedToEquip { get; set; }

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x06002042 RID: 8258 RVA: 0x000BED4D File Offset: 0x000BCF4D
		// (set) Token: 0x06002043 RID: 8259 RVA: 0x000BED55 File Offset: 0x000BCF55
		public bool isLockedToEquip
		{
			get
			{
				return this._isLockedToEquip;
			}
			set
			{
				if (this._isLockedToEquip != value)
				{
					this.onSetLockToEquip.Invoke(value);
				}
				this._isLockedToEquip = value;
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06002044 RID: 8260 RVA: 0x000BED73 File Offset: 0x000BCF73
		// (set) Token: 0x06002045 RID: 8261 RVA: 0x000BED7B File Offset: 0x000BCF7B
		public bool ignoreEquipEvents { get; set; }

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06002046 RID: 8262 RVA: 0x000BED84 File Offset: 0x000BCF84
		// (set) Token: 0x06002047 RID: 8263 RVA: 0x000BED8C File Offset: 0x000BCF8C
		internal bool isInit { get; set; }

		// Token: 0x06002048 RID: 8264 RVA: 0x000BED95 File Offset: 0x000BCF95
		public virtual void Init()
		{
			if (!this.isInit)
			{
				this.Start();
			}
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x000BEDA8 File Offset: 0x000BCFA8
		protected virtual void Start()
		{
			if (!this.isInit)
			{
				this.isInit = true;
				this.inventory = base.GetComponentInParent<vInventory>();
				if (this.equipSlots.Count == 0)
				{
					vEquipSlot[] componentsInChildren = base.GetComponentsInChildren<vEquipSlot>(true);
					this.equipSlots = componentsInChildren.vToList<vEquipSlot>();
				}
				foreach (vEquipSlot vEquipSlot in this.equipSlots)
				{
					vEquipSlot.onSubmitSlotCallBack = new ItemSlotEvent(this.OnSubmitSlot);
					vEquipSlot.onSelectSlotCallBack = new ItemSlotEvent(this.OnSelectSlot);
					vEquipSlot.onDeselectSlotCallBack = new ItemSlotEvent(this.OnDeselect);
					this.onSetLockToEquip.AddListener(new UnityAction<bool>(vEquipSlot.SetLockToEquip));
					if (vEquipSlot.displayAmountText)
					{
						vEquipSlot.displayAmountText.text = "";
					}
					vEquipSlot.onChangeAmount.Invoke("");
				}
			}
		}

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x0600204A RID: 8266 RVA: 0x000BEEB8 File Offset: 0x000BD0B8
		public virtual vEquipSlot currentEquippedSlot
		{
			get
			{
				return this.equipSlots[this.indexOfEquippedItem];
			}
		}

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x0600204B RID: 8267 RVA: 0x000BEECB File Offset: 0x000BD0CB
		public virtual vItem currentEquippedItem
		{
			get
			{
				return this.currentEquippedSlot.item;
			}
		}

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x0600204C RID: 8268 RVA: 0x000BEED8 File Offset: 0x000BD0D8
		public virtual List<vEquipSlot> ValidSlots
		{
			get
			{
				return this.equipSlots.FindAll((vEquipSlot slot) => slot.isValid && (!this.skipEmptySlots || slot.item != null));
			}
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x000BEEF4 File Offset: 0x000BD0F4
		public virtual bool ContainsItem(vItem item)
		{
			return this.equipSlots.Find((vEquipSlot slot) => slot.item == item) != null;
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x000BEF2C File Offset: 0x000BD12C
		public virtual void OnSubmitSlot(vItemSlot slot)
		{
			this.lastSelectedSlot = this.currentSelectedSlot;
			if (this.itemPicker != null)
			{
				this.currentSelectedSlot = slot as vEquipSlot;
				if (this.setEquipSlotWhenSubmit)
				{
					this.SetEquipSlot(this.equipSlots.IndexOf(this.currentSelectedSlot));
				}
				this.itemPicker.gameObject.SetActive(true);
				this.itemPicker.onCancelSlot.RemoveAllListeners();
				this.itemPicker.onCancelSlot.AddListener(new UnityAction(this.CancelCurrentSlot));
				this.itemPicker.CreateEquipmentWindow(this.inventory.items, this.currentSelectedSlot.itemType, slot.item, new UnityAction<vItemSlot>(this.OnPickItem), null);
				this.onInitPickUpItem.Invoke();
			}
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x000BEFFF File Offset: 0x000BD1FF
		public virtual void CancelCurrentSlot()
		{
			if (this.currentSelectedSlot == null)
			{
				this.currentSelectedSlot = this.lastSelectedSlot;
			}
			if (this.currentSelectedSlot != null)
			{
				this.currentSelectedSlot.OnCancel();
			}
			this.onFinishPickUpItem.Invoke();
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x000BF040 File Offset: 0x000BD240
		public virtual void UnequipItem(vEquipSlot slot)
		{
			if (slot)
			{
				vItem item = slot.item;
				if (this.equipSlots[this.indexOfEquippedItem].item == item)
				{
					this.lastEquippedItem = item;
				}
				slot.RemoveItem();
				this.onUnequipItem.Invoke(this, item);
			}
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x000BF094 File Offset: 0x000BD294
		public virtual void UnequipItem(vItem item)
		{
			vEquipSlot vEquipSlot = this.equipSlots.Find((vEquipSlot _slot) => _slot.item == item);
			if (vEquipSlot)
			{
				if (this.equipSlots[this.indexOfEquippedItem].item == item)
				{
					this.lastEquippedItem = item;
				}
				vEquipSlot.RemoveItem();
				this.onUnequipItem.Invoke(this, item);
			}
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x000BF118 File Offset: 0x000BD318
		public virtual void UnequipCurrentItem()
		{
			if (this.currentSelectedSlot && this.currentSelectedSlot.item)
			{
				vItem item = this.currentSelectedSlot.item;
				if (this.equipSlots[this.indexOfEquippedItem].item == item)
				{
					this.lastEquippedItem = item;
				}
				this.currentSelectedSlot.RemoveItem();
				this.onUnequipItem.Invoke(this, item);
			}
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x000BF18D File Offset: 0x000BD38D
		public virtual void OnSelectSlot(vItemSlot slot)
		{
			if (this.equipSlots.Contains(slot as vEquipSlot))
			{
				this.currentSelectedSlot = slot as vEquipSlot;
			}
			else
			{
				this.currentSelectedSlot = null;
			}
			this.onSelectEquipArea.Invoke(this);
			this.CreateFullItemDescription(slot);
		}

		// Token: 0x06002054 RID: 8276 RVA: 0x000BF1CA File Offset: 0x000BD3CA
		public virtual void OnDeselect(vItemSlot slot)
		{
			if (this.equipSlots.Contains(slot as vEquipSlot))
			{
				this.currentSelectedSlot = null;
			}
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x000BF1E8 File Offset: 0x000BD3E8
		protected virtual void CreateFullItemDescription(vItemSlot slot)
		{
			string text = (slot.item ? slot.item.name : "");
			string text2 = (slot.item ? slot.item.ItemTypeText() : "");
			string text3 = (slot.item ? slot.item.amount.ToString() : "");
			string text4 = (slot.item ? slot.item.description : "");
			string text5 = (slot.item ? slot.item.GetItemAttributesText(this.ignoreAttributes) : "");
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

		// Token: 0x06002056 RID: 8278 RVA: 0x000BF35C File Offset: 0x000BD55C
		public virtual void OnPickItem(vItemSlot slot)
		{
			if (!this.currentSelectedSlot)
			{
				this.currentSelectedSlot = this.lastSelectedSlot;
			}
			if (!this.currentSelectedSlot)
			{
				return;
			}
			if (this.currentSelectedSlot.item != null && slot.item != this.currentSelectedSlot.item)
			{
				this.currentSelectedSlot.item.isInEquipArea = false;
				vItem item = this.currentSelectedSlot.item;
				if (item == slot.item)
				{
					this.lastEquippedItem = item;
				}
				this.currentSelectedSlot.RemoveItem();
				this.onUnequipItem.Invoke(this, item);
			}
			if (slot.item != this.currentSelectedSlot.item)
			{
				if (this.onPickUpItemCallBack != null)
				{
					this.onPickUpItemCallBack(this, slot);
				}
				this.currentSelectedSlot.AddItem(slot.item);
				if (!this.ignoreEquipEvents)
				{
					this.onEquipItem.Invoke(this, this.currentSelectedSlot.item);
				}
			}
			this.currentSelectedSlot.OnCancel();
			this.currentSelectedSlot = null;
			this.lastSelectedSlot = null;
			this.itemPicker.gameObject.SetActive(false);
			this.onFinishPickUpItem.Invoke();
		}

		// Token: 0x06002057 RID: 8279 RVA: 0x000BF498 File Offset: 0x000BD698
		public virtual void NextEquipSlot()
		{
			if (this.equipSlots == null || this.equipSlots.Count == 0)
			{
				return;
			}
			this.lastEquippedItem = this.currentEquippedItem;
			List<vEquipSlot> list = this.equipSlots;
			int num = this.indexOfEquippedItem;
			for (int i = 0; i < this.equipSlots.Count; i++)
			{
				if (num + 1 < this.equipSlots.Count)
				{
					num++;
				}
				else
				{
					num = 0;
				}
				if (this.equipSlots[num].isValid && (!this.skipEmptySlots || this.equipSlots[num].item != null))
				{
					this.indexOfEquippedItem = num;
					break;
				}
			}
			if (this.lastEquippedItem != this.currentEquippedItem)
			{
				if (this.currentEquippedItem != null && !this.ignoreEquipEvents)
				{
					this.onEquipItem.Invoke(this, this.currentEquippedItem);
				}
				this.onUnequipItem.Invoke(this, this.lastEquippedItem);
			}
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x000BF590 File Offset: 0x000BD790
		public virtual void PreviousEquipSlot()
		{
			if (this.equipSlots == null || this.equipSlots.Count == 0)
			{
				return;
			}
			this.lastEquippedItem = this.currentEquippedItem;
			List<vEquipSlot> list = this.equipSlots;
			int num = this.indexOfEquippedItem;
			for (int i = 0; i < this.equipSlots.Count; i++)
			{
				if (num - 1 >= 0)
				{
					num--;
				}
				else
				{
					num = this.equipSlots.Count - 1;
				}
				if (this.equipSlots[num].isValid && (!this.skipEmptySlots || this.equipSlots[num].item != null))
				{
					this.indexOfEquippedItem = num;
					break;
				}
			}
			if (this.lastEquippedItem != this.currentEquippedItem)
			{
				if (this.currentEquippedItem != null && !this.ignoreEquipEvents)
				{
					this.onEquipItem.Invoke(this, this.currentEquippedItem);
				}
				this.onUnequipItem.Invoke(this, this.lastEquippedItem);
			}
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x000BF688 File Offset: 0x000BD888
		public virtual void SetEquipSlot(int indexOfSlot)
		{
			if (this.equipSlots == null || this.equipSlots.Count == 0)
			{
				return;
			}
			if (indexOfSlot < this.equipSlots.Count && indexOfSlot >= 0 && this.equipSlots[indexOfSlot].isValid && (!this.skipEmptySlots || this.equipSlots[indexOfSlot].item != null))
			{
				this.lastEquippedItem = this.currentEquippedItem;
				this.indexOfEquippedItem = indexOfSlot;
				if (this.currentEquippedItem != null && !this.ignoreEquipEvents)
				{
					this.onEquipItem.Invoke(this, this.currentEquippedItem);
				}
				if (this.currentEquippedItem != this.lastEquippedItem)
				{
					this.onUnequipItem.Invoke(this, this.lastEquippedItem);
				}
			}
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x000BF75C File Offset: 0x000BD95C
		public virtual void EquipCurrentSlot()
		{
			if (!this.currentEquippedSlot || (this.currentEquippedSlot.item != null && this.currentEquippedSlot.item.isEquiped))
			{
				return;
			}
			if (this.currentEquippedItem)
			{
				this.onEquipItem.Invoke(this, this.currentEquippedItem);
				return;
			}
			if (this.lastEquippedItem)
			{
				this.onUnequipItem.Invoke(this, this.lastEquippedItem);
			}
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x000BF7DB File Offset: 0x000BD9DB
		public virtual void AddItemToEquipSlot(vItemSlot slot, vItem item, bool autoEquip = false)
		{
			if (slot is vEquipSlot && this.equipSlots.Contains(slot as vEquipSlot))
			{
				this.AddItemToEquipSlot(this.equipSlots.IndexOf(slot as vEquipSlot), item, autoEquip);
			}
		}

		// Token: 0x0600205C RID: 8284 RVA: 0x000BF814 File Offset: 0x000BDA14
		public virtual void AddItemToEquipSlot(int indexOfSlot, vItem item, bool autoEquip = false)
		{
			if (indexOfSlot < this.equipSlots.Count && item != null)
			{
				vEquipSlot slot = this.equipSlots[indexOfSlot];
				if (slot != null && slot.isValid && slot.itemType.Contains(item.type))
				{
					vEquipSlot vEquipSlot = this.equipSlots.Find((vEquipSlot s) => s.item == item && s != slot);
					if (vEquipSlot != null)
					{
						this.RemoveItemOfEquipSlot(this.equipSlots.IndexOf(vEquipSlot));
					}
					if (slot.item != null && slot.item != item)
					{
						vItem item2 = slot.item;
						if (this.currentEquippedItem == item2)
						{
							this.lastEquippedItem = item2;
						}
						slot.RemoveItem();
						item2.isInEquipArea = false;
						this.onUnequipItem.Invoke(this, item2);
					}
					item.isInEquipArea = true;
					slot.AddItem(item);
					if (autoEquip)
					{
						this.SetEquipSlot(indexOfSlot);
						return;
					}
					if (!this.ignoreEquipEvents)
					{
						this.onEquipItem.Invoke(this, item);
					}
				}
			}
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x000BF983 File Offset: 0x000BDB83
		public virtual void RemoveItemOfEquipSlot(vItemSlot slot)
		{
			if (slot is vEquipSlot && this.equipSlots.Contains(slot as vEquipSlot))
			{
				this.RemoveItemOfEquipSlot(this.equipSlots.IndexOf(slot as vEquipSlot));
			}
		}

		// Token: 0x0600205E RID: 8286 RVA: 0x000BF9B8 File Offset: 0x000BDBB8
		public void RemoveItemOfEquipSlot(int indexOfSlot)
		{
			if (indexOfSlot < this.equipSlots.Count)
			{
				vEquipSlot vEquipSlot = this.equipSlots[indexOfSlot];
				if (vEquipSlot != null && vEquipSlot.item != null)
				{
					vItem item = vEquipSlot.item;
					item.isInEquipArea = false;
					if (this.currentEquippedItem == item)
					{
						this.lastEquippedItem = this.currentEquippedItem;
					}
					vEquipSlot.RemoveItem();
					this.onUnequipItem.Invoke(this, item);
				}
			}
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x000BFA34 File Offset: 0x000BDC34
		public virtual void AddCurrentItem(vItem item)
		{
			if (this.indexOfEquippedItem < this.equipSlots.Count)
			{
				vEquipSlot vEquipSlot = this.equipSlots[this.indexOfEquippedItem];
				if (vEquipSlot.item != null && item != vEquipSlot.item)
				{
					if (this.currentEquippedItem == vEquipSlot.item)
					{
						this.lastEquippedItem = vEquipSlot.item;
					}
					vEquipSlot.item.isInEquipArea = false;
					vEquipSlot vEquipSlot2 = this.equipSlots.Find((vEquipSlot i) => i.item == item);
					if (vEquipSlot2 != null)
					{
						vEquipSlot2.RemoveItem();
					}
					vEquipSlot.RemoveItem();
					this.onUnequipItem.Invoke(this, this.lastEquippedItem);
				}
				else
				{
					vEquipSlot vEquipSlot3 = this.equipSlots.Find((vEquipSlot i) => i.item == item);
					if (vEquipSlot3 != null)
					{
						vEquipSlot3.RemoveItem();
					}
				}
				vEquipSlot.AddItem(item);
				if (!this.ignoreEquipEvents)
				{
					this.onEquipItem.Invoke(this, item);
				}
			}
		}

		// Token: 0x06002060 RID: 8288 RVA: 0x000BFB50 File Offset: 0x000BDD50
		public virtual void RemoveCurrentItem()
		{
			if (!this.currentEquippedItem)
			{
				return;
			}
			this.lastEquippedItem = this.currentEquippedItem;
			this.equipSlots[this.indexOfEquippedItem].RemoveItem();
			this.onUnequipItem.Invoke(this, this.lastEquippedItem);
		}

		// Token: 0x04002108 RID: 8456
		public vEquipArea.OnPickUpItem onPickUpItemCallBack;

		// Token: 0x04002109 RID: 8457
		public vInventory inventory;

		// Token: 0x0400210A RID: 8458
		public vItemWindow itemPicker;

		// Token: 0x0400210B RID: 8459
		[Tooltip("Set current equipped slot when submit an slot of this area")]
		public bool setEquipSlotWhenSubmit;

		// Token: 0x0400210C RID: 8460
		[Tooltip("Skip empty slots when switching between slots")]
		public bool skipEmptySlots;

		// Token: 0x0400210D RID: 8461
		public List<vEquipSlot> equipSlots;

		// Token: 0x0400210E RID: 8462
		public string equipPointName;

		// Token: 0x0400210F RID: 8463
		public Text displayNameText;

		// Token: 0x04002110 RID: 8464
		public Text displayTypeText;

		// Token: 0x04002111 RID: 8465
		public Text displayAmountText;

		// Token: 0x04002112 RID: 8466
		public Text displayDescriptionText;

		// Token: 0x04002113 RID: 8467
		public Text displayAttributesText;

		// Token: 0x04002114 RID: 8468
		[vHelpBox("You can ignore display Attributes using this property", vHelpBoxAttribute.MessageType.None)]
		public List<vItemAttributes> ignoreAttributes;

		// Token: 0x04002115 RID: 8469
		public UnityEvent onInitPickUpItem;

		// Token: 0x04002116 RID: 8470
		public UnityEvent onFinishPickUpItem;

		// Token: 0x04002117 RID: 8471
		public InputField.OnChangeEvent onChangeName;

		// Token: 0x04002118 RID: 8472
		public InputField.OnChangeEvent onChangeType;

		// Token: 0x04002119 RID: 8473
		public InputField.OnChangeEvent onChangeAmount;

		// Token: 0x0400211A RID: 8474
		public InputField.OnChangeEvent onChangeDescription;

		// Token: 0x0400211B RID: 8475
		public InputField.OnChangeEvent onChangeAttributes;

		// Token: 0x0400211C RID: 8476
		public OnChangeEquipmentEvent onEquipItem;

		// Token: 0x0400211D RID: 8477
		public OnChangeEquipmentEvent onUnequipItem;

		// Token: 0x0400211E RID: 8478
		public OnChangeEquipmentEvent onChangeEquipSlot;

		// Token: 0x0400211F RID: 8479
		public OnSelectEquipArea onSelectEquipArea;

		// Token: 0x04002120 RID: 8480
		public Toggle.ToggleEvent onSetLockToEquip;

		// Token: 0x04002123 RID: 8483
		protected bool equipSlotUpdated;

		// Token: 0x04002124 RID: 8484
		protected int _indexOfEquippedSlot;

		// Token: 0x02000540 RID: 1344
		// (Invoke) Token: 0x06002064 RID: 8292
		public delegate void OnPickUpItem(vEquipArea area, vItemSlot slot);
	}
}
