using System;
using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Invector.vItemManager
{
	// Token: 0x02000553 RID: 1363
	[vClassHeader("Inventory", true, "icon_v2", false, "")]
	public class vInventory : vMonoBehaviour
	{
		// Token: 0x1400003B RID: 59
		// (add) Token: 0x060020AD RID: 8365 RVA: 0x000C041C File Offset: 0x000BE61C
		// (remove) Token: 0x060020AE RID: 8366 RVA: 0x000C0454 File Offset: 0x000BE654
		public event vInventory.OnUpdateInventoryDelegate OnUpdateInventory;

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x060020AF RID: 8367 RVA: 0x000C0489 File Offset: 0x000BE689
		public List<vItem> items
		{
			get
			{
				if (this.GetItemsHandler != null)
				{
					return this.GetItemsHandler();
				}
				return new List<vItem>();
			}
		}

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x060020B0 RID: 8368 RVA: 0x000C04A4 File Offset: 0x000BE6A4
		public List<vItem> allItems
		{
			get
			{
				if (this.GetItemsAllHandler != null)
				{
					return this.GetItemsAllHandler();
				}
				return new List<vItem>();
			}
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x000C04C0 File Offset: 0x000BE6C0
		protected virtual void Start()
		{
			this.canEquip = true;
			this.inputModule = Object.FindObjectOfType<StandaloneInputModule>();
			if (this.inputModule == null)
			{
				this.inputModule = new GameObject("EventSystem").AddComponent<StandaloneInputModule>();
			}
			this.equipAreas = base.GetComponentsInChildren<vEquipArea>(true);
			foreach (vEquipArea vEquipArea in this.equipAreas)
			{
				vEquipArea.Init();
				vEquipArea.onEquipItem.AddListener(new UnityAction<vEquipArea, vItem>(this.OnEquipItem));
				vEquipArea.onUnequipItem.AddListener(new UnityAction<vEquipArea, vItem>(this.OnUnequipItem));
				vEquipArea.onSelectEquipArea.AddListener(new UnityAction<vEquipArea>(this.SetCurrentSelectedArea));
				vEquipArea.onChangeEquipSlot.AddListener(new UnityAction<vEquipArea, vItem>(this.ChangeEquipmentDisplay));
			}
			for (int j = 0; j < this.changeEquipmentControllers.Count; j++)
			{
				if (this.changeEquipmentControllers[j] != null && this.changeEquipmentControllers[j].equipArea && this.changeEquipmentControllers[j].display)
				{
					this.changeEquipmentControllers[j].equipArea.onSetLockToEquip.AddListener(new UnityAction<bool>(this.changeEquipmentControllers[j].display.SetLockToEquip));
				}
			}
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x000C061B File Offset: 0x000BE81B
		protected virtual void LateUpdate()
		{
			if (this.IsLocked())
			{
				return;
			}
			this.OpenCloseInventoryInput();
			if (this.isOpen)
			{
				this.UpdateEventSystemInput();
			}
			if (!this.isOpen)
			{
				this.ChangeEquipmentInput();
				return;
			}
			this.RemoveEquipmentInput();
		}

		// Token: 0x060020B3 RID: 8371 RVA: 0x000C064F File Offset: 0x000BE84F
		public virtual void SaveItemsExample()
		{
			base.GetComponentInParent<vItemManager>().SaveInventory();
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x000C065C File Offset: 0x000BE85C
		public virtual void LoadItemsExample()
		{
			base.GetComponentInParent<vItemManager>().LoadInventory();
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x000C0669 File Offset: 0x000BE869
		public virtual void OnReloadGame()
		{
			base.StartCoroutine(this.ReloadEquipment());
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x000C0678 File Offset: 0x000BE878
		protected virtual IEnumerator ReloadEquipment()
		{
			yield return new WaitForEndOfFrame();
			this.inputModule = Object.FindObjectOfType<StandaloneInputModule>();
			this.isOpen = true;
			for (int i = 0; i < this.equipAreas.Length; i++)
			{
				vEquipArea vEquipArea = this.equipAreas[i];
				for (int j = 0; j < vEquipArea.equipSlots.Count; j++)
				{
					vEquipSlot vEquipSlot = vEquipArea.equipSlots[j];
					if (vEquipArea.currentEquippedItem == null)
					{
						this.OnUnequipItem(vEquipArea, vEquipSlot.item);
						vEquipArea.UnequipItem(vEquipSlot);
					}
					else
					{
						vEquipArea.UnequipItem(vEquipSlot);
					}
				}
			}
			this.isOpen = false;
			yield break;
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x000C0687 File Offset: 0x000BE887
		public virtual bool IsLocked()
		{
			return (this.IsLockedEvent != null && this.IsLockedEvent()) || this.lockInventoryInput;
		}

		// Token: 0x060020B8 RID: 8376 RVA: 0x000C06A9 File Offset: 0x000BE8A9
		public virtual void SetLockInventoryInput(bool value)
		{
			this.lockInventoryInput = value;
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x000C06B2 File Offset: 0x000BE8B2
		public virtual void UpdateInventory()
		{
			vInventory.OnUpdateInventoryDelegate onUpdateInventoryDelegate = this.OnUpdateInventory;
			if (onUpdateInventoryDelegate != null)
			{
				onUpdateInventoryDelegate();
			}
			this.onUpdateInventory.Invoke();
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x000C06D0 File Offset: 0x000BE8D0
		public virtual void OpenCloseInventoryInput()
		{
			if (this.openInventory.GetButtonDown() && this.canEquip)
			{
				if (!this.isOpen)
				{
					this.OpenInventory();
					return;
				}
				this.CloseInventory();
			}
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x000C06FC File Offset: 0x000BE8FC
		public virtual void OpenInventory()
		{
			if (this.isOpen)
			{
				return;
			}
			this.isOpen = true;
			if (!this.updatedTimeScale)
			{
				this.updatedTimeScale = true;
				this.originalTimeScale = Time.timeScale;
				Time.timeScale = this.timeScaleWhileIsOpen;
			}
			this.onOpenCloseInventory.Invoke(true);
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x000C074A File Offset: 0x000BE94A
		public virtual void CloseInventory()
		{
			if (!this.isOpen)
			{
				return;
			}
			this.isOpen = false;
			if (this.updatedTimeScale)
			{
				Time.timeScale = this.originalTimeScale;
				this.updatedTimeScale = false;
			}
			this.onOpenCloseInventory.Invoke(false);
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x000C0782 File Offset: 0x000BE982
		protected virtual void RemoveEquipmentInput()
		{
			if (this.currentEquipArea != null && this.removeEquipment.GetButtonDown())
			{
				this.currentEquipArea.UnequipCurrentItem();
			}
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x000C07AA File Offset: 0x000BE9AA
		protected virtual void SetCurrentSelectedArea(vEquipArea equipArea)
		{
			this.currentEquipArea = equipArea;
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x000C07B4 File Offset: 0x000BE9B4
		protected virtual void ChangeEquipmentInput()
		{
			if (this.changeEquipmentControllers.Count > 0 && this.canEquip)
			{
				foreach (ChangeEquipmentControl changeEquipmentControl in this.changeEquipmentControllers)
				{
					this.UseItemInput(changeEquipmentControl);
					if (changeEquipmentControl.equipArea != null)
					{
						if (changeEquipmentControl.previousItemInput.GetButtonDown())
						{
							changeEquipmentControl.equipArea.PreviousEquipSlot();
						}
						else if (changeEquipmentControl.nextItemInput.GetButtonDown())
						{
							changeEquipmentControl.equipArea.NextEquipSlot();
						}
					}
				}
			}
		}

		// Token: 0x060020C0 RID: 8384 RVA: 0x000C0860 File Offset: 0x000BEA60
		public virtual void CheckEquipmentChanges()
		{
			for (int i = 0; i < this.equipAreas.Length; i++)
			{
				vEquipArea equipArea = this.equipAreas[i];
				Predicate<ChangeEquipmentControl> <>9__0;
				for (int j = 0; j < equipArea.equipSlots.Count; j++)
				{
					vEquipSlot vEquipSlot = equipArea.equipSlots[j];
					if (vEquipSlot.item != null && !this.items.Contains(vEquipSlot.item))
					{
						equipArea.UnequipItem(vEquipSlot);
						List<ChangeEquipmentControl> list = this.changeEquipmentControllers;
						Predicate<ChangeEquipmentControl> predicate;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = (ChangeEquipmentControl e) => e.equipArea.Equals(equipArea));
						}
						ChangeEquipmentControl changeEquipmentControl = list.Find(predicate);
						if (changeEquipmentControl != null && changeEquipmentControl.display)
						{
							changeEquipmentControl.display.RemoveItem();
						}
					}
				}
			}
		}

		// Token: 0x060020C1 RID: 8385 RVA: 0x000C0948 File Offset: 0x000BEB48
		protected virtual void UpdateEventSystemInput()
		{
			if (this.inputModule)
			{
				this.inputModule.horizontalAxis = this.horizontal.buttonName;
				this.inputModule.verticalAxis = this.vertical.buttonName;
				this.inputModule.submitButton = this.submit.buttonName;
				this.inputModule.cancelButton = this.cancel.buttonName;
				return;
			}
			this.inputModule = Object.FindObjectOfType<StandaloneInputModule>();
		}

		// Token: 0x060020C2 RID: 8386 RVA: 0x000C09C8 File Offset: 0x000BEBC8
		protected virtual void UseItemInput(ChangeEquipmentControl changeEquip)
		{
			if (changeEquip.display != null && changeEquip.display.item != null && changeEquip.display.item.type == vItemType.Consumable && changeEquip.useItemInput.GetButtonDown() && changeEquip.display.item.amount > 0)
			{
				this.OnUseItem(changeEquip.display.item);
			}
		}

		// Token: 0x060020C3 RID: 8387 RVA: 0x000C0A39 File Offset: 0x000BEC39
		internal virtual void OnUseItem(vItem item)
		{
			this.onUseItem.Invoke(item);
		}

		// Token: 0x060020C4 RID: 8388 RVA: 0x000C0A47 File Offset: 0x000BEC47
		internal virtual void OnDestroyItem(vItem item, int amount)
		{
			this.onDestroyItem.Invoke(item, amount);
			this.CheckEquipmentChanges();
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x000C0A5C File Offset: 0x000BEC5C
		internal virtual void OnDropItem(vItem item, int amount)
		{
			this.onDropItem.Invoke(item, amount);
			this.CheckEquipmentChanges();
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x000C0A71 File Offset: 0x000BEC71
		public virtual void OnEquipItem(vEquipArea equipArea, vItem item)
		{
			this.onEquipItem.Invoke(equipArea, item);
			this.ChangeEquipmentDisplay(equipArea, equipArea.currentEquippedItem);
		}

		// Token: 0x060020C7 RID: 8391 RVA: 0x000C0A8D File Offset: 0x000BEC8D
		public virtual void OnUnequipItem(vEquipArea equipArea, vItem item)
		{
			this.onUnequipItem.Invoke(equipArea, item);
			this.ChangeEquipmentDisplay(equipArea, equipArea.currentEquippedItem);
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x000C0AAC File Offset: 0x000BECAC
		protected virtual void ChangeEquipmentDisplay(vEquipArea equipArea, vItem item)
		{
			if (this.changeEquipmentControllers.Count > 0)
			{
				ChangeEquipmentControl changeEquipmentControl = this.changeEquipmentControllers.Find((ChangeEquipmentControl changeEquip) => changeEquip.equipArea != null && changeEquip.equipArea == equipArea && changeEquip.display != null);
				if (changeEquipmentControl != null)
				{
					changeEquipmentControl.display.AddItem(item);
					changeEquipmentControl.display.ItemIdentifier(changeEquipmentControl.equipArea.indexOfEquippedItem + 1, true);
				}
			}
		}

		// Token: 0x04002158 RID: 8536
		public vInventory.GetItemsDelegate GetItemsHandler;

		// Token: 0x04002159 RID: 8537
		public vInventory.GetItemsDelegate GetItemsAllHandler;

		// Token: 0x0400215A RID: 8538
		public vInventory.AddItemDelegate AddItemsHandler;

		// Token: 0x0400215B RID: 8539
		public vInventory.GetAllAmountDelegate GetAllAmount;

		// Token: 0x0400215C RID: 8540
		public vInventory.LockInventoryInputEvent IsLockedEvent;

		// Token: 0x0400215E RID: 8542
		[vEditorToolbar("Settings", false, "", false, false)]
		[vHelpBox("True: Play Item animation when the timeScale is 0 \n False: Ignore Item animation if timeScale equals 0", vHelpBoxAttribute.MessageType.None)]
		public bool playItemAnimation = true;

		// Token: 0x0400215F RID: 8543
		[Range(0f, 1f)]
		public float timeScaleWhileIsOpen;

		// Token: 0x04002160 RID: 8544
		[Tooltip("Check true to not destroy this object when changing scenes")]
		public bool dontDestroyOnLoad = true;

		// Token: 0x04002161 RID: 8545
		public List<ChangeEquipmentControl> changeEquipmentControllers;

		// Token: 0x04002162 RID: 8546
		[vEditorToolbar("Input Mapping", false, "", false, false)]
		public GenericInput openInventory = new GenericInput("I", "Start", "Start");

		// Token: 0x04002163 RID: 8547
		public GenericInput removeEquipment = new GenericInput("Mouse1", "X", "X");

		// Token: 0x04002164 RID: 8548
		[Header("This fields will override the EventSystem Input")]
		public GenericInput horizontal = new GenericInput("Horizontal", "D-Pad Horizontal", "Horizontal");

		// Token: 0x04002165 RID: 8549
		public GenericInput vertical = new GenericInput("Vertical", "D-Pad Vertical", "Vertical");

		// Token: 0x04002166 RID: 8550
		public GenericInput submit = new GenericInput("Return", "A", "A");

		// Token: 0x04002167 RID: 8551
		public GenericInput cancel = new GenericInput("Backspace", "B", "B");

		// Token: 0x04002168 RID: 8552
		[vEditorToolbar("Events", false, "", false, false)]
		public OnOpenCloseInventory onOpenCloseInventory;

		// Token: 0x04002169 RID: 8553
		public OnHandleItemEvent onUseItem;

		// Token: 0x0400216A RID: 8554
		public OnChangeItemAmount onDestroyItem;

		// Token: 0x0400216B RID: 8555
		public OnChangeItemAmount onDropItem;

		// Token: 0x0400216C RID: 8556
		public OnChangeEquipmentEvent onEquipItem;

		// Token: 0x0400216D RID: 8557
		public OnChangeEquipmentEvent onUnequipItem;

		// Token: 0x0400216E RID: 8558
		[SerializeField]
		protected UnityEvent onUpdateInventory;

		// Token: 0x0400216F RID: 8559
		[HideInInspector]
		public bool isOpen;

		// Token: 0x04002170 RID: 8560
		[HideInInspector]
		public bool canEquip;

		// Token: 0x04002171 RID: 8561
		[HideInInspector]
		public bool lockInventoryInput;

		// Token: 0x04002172 RID: 8562
		public vEquipArea[] equipAreas;

		// Token: 0x04002173 RID: 8563
		private float originalTimeScale = 1f;

		// Token: 0x04002174 RID: 8564
		private bool updatedTimeScale;

		// Token: 0x04002175 RID: 8565
		private vEquipArea currentEquipArea;

		// Token: 0x04002176 RID: 8566
		private StandaloneInputModule inputModule;

		// Token: 0x02000554 RID: 1364
		// (Invoke) Token: 0x060020CB RID: 8395
		public delegate List<vItem> GetItemsDelegate();

		// Token: 0x02000555 RID: 1365
		// (Invoke) Token: 0x060020CF RID: 8399
		public delegate void AddItemDelegate(ItemReference itemReference, bool immediate = true, UnityAction<vItem> onFinish = null);

		// Token: 0x02000556 RID: 1366
		// (Invoke) Token: 0x060020D3 RID: 8403
		public delegate bool LockInventoryInputEvent();

		// Token: 0x02000557 RID: 1367
		// (Invoke) Token: 0x060020D7 RID: 8407
		public delegate int GetAllAmountDelegate(int id);

		// Token: 0x02000558 RID: 1368
		// (Invoke) Token: 0x060020DB RID: 8411
		public delegate void OnUpdateInventoryDelegate();
	}
}
