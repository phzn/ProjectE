using System;
using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using Invector.vEventSystems;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vItemManager
{
	// Token: 0x02000589 RID: 1417
	[vClassHeader("ItemManager", true, "icon_v2", false, "")]
	public class vItemManager : vMonoBehaviour, IActionReceiver, IActionController
	{
		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06002166 RID: 8550 RVA: 0x000C2398 File Offset: 0x000C0598
		// (set) Token: 0x06002167 RID: 8551 RVA: 0x000C23A0 File Offset: 0x000C05A0
		public virtual Dictionary<vItem, vEquipment> equipments { get; set; }

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x06002168 RID: 8552 RVA: 0x000C23A9 File Offset: 0x000C05A9
		// (set) Token: 0x06002169 RID: 8553 RVA: 0x000C23B1 File Offset: 0x000C05B1
		public virtual Dictionary<GameObject, vEquipment> equipmentsObject { get; set; }

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x0600216A RID: 8554 RVA: 0x000C23BA File Offset: 0x000C05BA
		// (set) Token: 0x0600216B RID: 8555 RVA: 0x000C23C2 File Offset: 0x000C05C2
		protected virtual GameObject equipmentContainer { get; set; }

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x0600216C RID: 8556 RVA: 0x000C23CB File Offset: 0x000C05CB
		// (set) Token: 0x0600216D RID: 8557 RVA: 0x000C23D3 File Offset: 0x000C05D3
		public virtual List<vItem> items { get; set; }

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x0600216E RID: 8558 RVA: 0x000C23DC File Offset: 0x000C05DC
		// (set) Token: 0x0600216F RID: 8559 RVA: 0x000C23E4 File Offset: 0x000C05E4
		internal virtual bool inEquip { get; set; }

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06002170 RID: 8560 RVA: 0x000C23ED File Offset: 0x000C05ED
		// (set) Token: 0x06002171 RID: 8561 RVA: 0x000C23F5 File Offset: 0x000C05F5
		internal virtual bool usingItem { get; set; }

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06002172 RID: 8562 RVA: 0x000C23FE File Offset: 0x000C05FE
		// (set) Token: 0x06002173 RID: 8563 RVA: 0x000C2406 File Offset: 0x000C0606
		protected virtual float equipTimer { get; set; }

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06002174 RID: 8564 RVA: 0x000C240F File Offset: 0x000C060F
		// (set) Token: 0x06002175 RID: 8565 RVA: 0x000C2417 File Offset: 0x000C0617
		protected virtual float unequipTimer { get; set; }

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06002176 RID: 8566 RVA: 0x000C2420 File Offset: 0x000C0620
		// (set) Token: 0x06002177 RID: 8567 RVA: 0x000C2428 File Offset: 0x000C0628
		protected virtual Animator animator { get; set; }

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06002178 RID: 8568 RVA: 0x000C2431 File Offset: 0x000C0631
		// (set) Token: 0x06002179 RID: 8569 RVA: 0x000C2439 File Offset: 0x000C0639
		protected virtual bool inCollectItemRoutine { get; set; }

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x0600217A RID: 8570 RVA: 0x000C2442 File Offset: 0x000C0642
		// (set) Token: 0x0600217B RID: 8571 RVA: 0x000C244A File Offset: 0x000C064A
		protected virtual List<ItemReference> itemsToCollect { get; set; }

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x0600217C RID: 8572 RVA: 0x000C2453 File Offset: 0x000C0653
		// (set) Token: 0x0600217D RID: 8573 RVA: 0x000C245B File Offset: 0x000C065B
		public virtual vIAnimatorStateInfoController animatorStateInfos { get; set; }

		// Token: 0x1400003C RID: 60
		// (add) Token: 0x0600217E RID: 8574 RVA: 0x000C2464 File Offset: 0x000C0664
		// (remove) Token: 0x0600217F RID: 8575 RVA: 0x000C249C File Offset: 0x000C069C
		public event vItemManager.CanUseItemDelegate canUseItemDelegate;

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06002180 RID: 8576 RVA: 0x000C24D4 File Offset: 0x000C06D4
		public virtual bool playItemAnimation
		{
			get
			{
				return this.inventory != null && ((this.inventory.isOpen && this.inventory.playItemAnimation) || !this.inventory.isOpen) && !this.temporarilyIgnoreItemAnimation;
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x06002181 RID: 8577 RVA: 0x000C2521 File Offset: 0x000C0721
		// (set) Token: 0x06002182 RID: 8578 RVA: 0x000C2529 File Offset: 0x000C0729
		public virtual bool temporarilyIgnoreItemAnimation { get; set; }

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x06002183 RID: 8579 RVA: 0x000C2532 File Offset: 0x000C0732
		// (set) Token: 0x06002184 RID: 8580 RVA: 0x000C253A File Offset: 0x000C073A
		public virtual EquipPoint defaultLeftArmEquipPoint { get; protected set; }

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x06002185 RID: 8581 RVA: 0x000C2543 File Offset: 0x000C0743
		// (set) Token: 0x06002186 RID: 8582 RVA: 0x000C254B File Offset: 0x000C074B
		public virtual EquipPoint defaultRightArmEquipPoint { get; protected set; }

		// Token: 0x06002187 RID: 8583 RVA: 0x000C2554 File Offset: 0x000C0754
		protected virtual void Awake()
		{
			this.itemsToCollect = new List<ItemReference>();
			this.equipments = new Dictionary<vItem, vEquipment>();
			this.equipmentsObject = new Dictionary<GameObject, vEquipment>();
			this.items = new List<vItem>();
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x000C2582 File Offset: 0x000C0782
		protected virtual IEnumerator Start()
		{
			if (!this.inventory)
			{
				this.inventory = base.transform.GetComponentInChildren<vInventory>();
			}
			if (!this.inventory && this.debugMode)
			{
				Debug.LogWarning("Missing Inventory prefab - You need to Drag and drop a Inventory Prefab inside the Character");
			}
			if (this.inventory)
			{
				this.equipmentContainer = new GameObject("Equipment Container");
				this.equipmentContainer.transform.parent = base.transform;
				this.equipmentContainer.transform.localPosition = Vector3.zero;
				this.equipmentContainer.transform.localEulerAngles = Vector3.zero;
				this.inventory.GetItemsHandler = new vInventory.GetItemsDelegate(this.GetItems);
				this.inventory.GetItemsAllHandler = new vInventory.GetItemsDelegate(this.GetAllItems);
				this.inventory.AddItemsHandler = new vInventory.AddItemDelegate(this.AddItem);
				this.inventory.GetAllAmount = new vInventory.GetAllAmountDelegate(this.GetAllAmount);
				this.inventory.onEquipItem.AddListener(new UnityAction<vEquipArea, vItem>(this.EquipItem));
				this.inventory.onUnequipItem.AddListener(new UnityAction<vEquipArea, vItem>(this.UnequipItem));
				this.inventory.onDropItem.AddListener(new UnityAction<vItem, int>(this.DropItem));
				this.inventory.onDestroyItem.AddListener(new UnityAction<vItem, int>(this.DestroyItem));
				this.inventory.onUseItem.AddListener(new UnityAction<vItem>(this.UseItem));
				this.inventory.onOpenCloseInventory.AddListener(new UnityAction<bool>(this.OnOpenCloseInventory));
				vMeleeCombatInput melee = base.GetComponent<vMeleeCombatInput>();
				if (melee)
				{
					this.inventory.IsLockedEvent = () => melee.lockInventory;
				}
			}
			this.defaultLeftArmEquipPoint = this.equipPoints.Find((EquipPoint e) => e.equipPointName.Equals("LeftArm"));
			this.defaultRightArmEquipPoint = this.equipPoints.Find((EquipPoint e) => e.equipPointName.Equals("RightArm"));
			this.animator = base.GetComponent<Animator>();
			this.animatorStateInfos = base.GetComponent<vIAnimatorStateInfoController>();
			yield return new WaitForEndOfFrame();
			this.RegisterDefaultEquipPointListeners();
			this.items = new List<vItem>();
			if (this.itemListData)
			{
				for (int i = 0; i < this.startItems.Count; i++)
				{
					this.AddItem(this.startItems[i], true, null);
				}
			}
			yield break;
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x000C2594 File Offset: 0x000C0794
		public virtual void RegisterDefaultEquipPointListeners()
		{
			IWeaponEquipmentListener[] components = base.GetComponents<IWeaponEquipmentListener>();
			if (components.Length != 0)
			{
				for (int i = 0; i < components.Length; i++)
				{
					if (this.defaultLeftArmEquipPoint != null)
					{
						this.defaultLeftArmEquipPoint.onInstantiateEquiment.AddListener(new UnityAction<GameObject>(components[i].SetLeftWeapon));
					}
					if (this.defaultRightArmEquipPoint != null)
					{
						this.defaultRightArmEquipPoint.onInstantiateEquiment.AddListener(new UnityAction<GameObject>(components[i].SetRightWeapon));
					}
				}
			}
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x000C2608 File Offset: 0x000C0808
		public virtual void LockInventoryInput(bool value)
		{
			if (this.inventory)
			{
				this.inventory.lockInventoryInput = value;
			}
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x000C2623 File Offset: 0x000C0823
		protected virtual void OnOpenCloseInventory(bool value)
		{
			this.onOpenCloseInventory.Invoke(value);
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x000C2631 File Offset: 0x000C0831
		public virtual void SaveItemsExample()
		{
			this.SaveInventory();
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x000C2639 File Offset: 0x000C0839
		public virtual void LoadItemsExample()
		{
			this.LoadInventory();
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x000C2641 File Offset: 0x000C0841
		public virtual bool IsAnimatorTag(string tag)
		{
			return !(this.animator == null) && (this.animatorStateInfos.isValid() && this.animatorStateInfos.animatorStateInfos.HasTag(tag));
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x000C2678 File Offset: 0x000C0878
		protected virtual void CheckIsLockedToEquip()
		{
			vEquipArea vEquipArea = Array.Find<vEquipArea>(this.inventory.equipAreas, (vEquipArea a) => a.equipPointName.Equals("RightArm"));
			vEquipArea vEquipArea2 = Array.Find<vEquipArea>(this.inventory.equipAreas, (vEquipArea a) => a.equipPointName.Equals("LeftArm"));
			if (vEquipArea == null || vEquipArea2 == null)
			{
				return;
			}
			bool flag = false;
			bool flag2 = vEquipArea2.currentEquippedItem && vEquipArea.currentEquippedItem && vEquipArea.currentEquippedItem.twoHandWeapon;
			if (flag2 && vEquipArea2.currentEquippedItem && this.equipments.ContainsKey(vEquipArea2.currentEquippedItem))
			{
				vEquipment vEquipment = this.equipments[vEquipArea2.currentEquippedItem];
				if (vEquipment.gameObject.activeSelf)
				{
					vEquipment.gameObject.SetActive(false);
				}
			}
			if (!flag2)
			{
				flag = vEquipArea.currentEquippedItem && vEquipArea2.currentEquippedItem && vEquipArea2.currentEquippedItem.twoHandWeapon;
				if (flag && vEquipArea.currentEquippedItem && this.equipments.ContainsKey(vEquipArea.currentEquippedItem))
				{
					vEquipment vEquipment2 = this.equipments[vEquipArea.currentEquippedItem];
					if (vEquipment2.gameObject.activeSelf)
					{
						vEquipment2.gameObject.SetActive(false);
					}
				}
			}
			if (flag2 != vEquipArea2.isLockedToEquip)
			{
				vEquipArea2.isLockedToEquip = flag2;
				this.onSetLockedToEquip.Invoke(vEquipArea2);
			}
			if (flag != vEquipArea.isLockedToEquip)
			{
				vEquipArea.isLockedToEquip = flag;
				this.onSetLockedToEquip.Invoke(vEquipArea);
			}
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x000C282C File Offset: 0x000C0A2C
		public virtual bool ContainItem(int id)
		{
			return this.items.Exists((vItem i) => i.id == id);
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x000C2860 File Offset: 0x000C0A60
		public virtual bool ContainItem(string itemName)
		{
			return this.items.Exists((vItem i) => i.name == itemName);
		}

		// Token: 0x06002192 RID: 8594 RVA: 0x000C2891 File Offset: 0x000C0A91
		public virtual bool ContainItem(int id, int amount)
		{
			return this.GetAllAmount(id) >= amount;
		}

		// Token: 0x06002193 RID: 8595 RVA: 0x000C28A0 File Offset: 0x000C0AA0
		public virtual bool ContainItem(string itemName, int amount)
		{
			vItem vItem = this.items.Find((vItem i) => i.name == itemName && i.amount >= amount);
			return vItem != null && this.GetAllAmount(vItem.id) >= amount;
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x000C28FB File Offset: 0x000C0AFB
		public virtual bool EquipAreaHasSomeItem(int indexOfArea)
		{
			return this.inventory.equipAreas[indexOfArea].equipSlots.Exists((vEquipSlot slot) => slot.item != null);
		}

		// Token: 0x06002195 RID: 8597 RVA: 0x000C2934 File Offset: 0x000C0B34
		public virtual bool ItemIsInSomeEquipArea(int id)
		{
			if (!this.inventory || this.inventory.equipAreas.Length == 0)
			{
				return false;
			}
			Predicate<vEquipSlot> <>9__0;
			for (int i = 0; i < this.inventory.equipAreas.Length; i++)
			{
				List<vEquipSlot> equipSlots = this.inventory.equipAreas[i].equipSlots;
				Predicate<vEquipSlot> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = (vEquipSlot slot) => slot.item.id.Equals(id));
				}
				if (equipSlots.Exists(predicate))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x000C29C0 File Offset: 0x000C0BC0
		public virtual bool ItemIsInSomeEquipArea(string itemName)
		{
			if (!this.inventory || this.inventory.equipAreas.Length == 0)
			{
				return false;
			}
			Predicate<vEquipSlot> <>9__0;
			for (int i = 0; i < this.inventory.equipAreas.Length; i++)
			{
				List<vEquipSlot> equipSlots = this.inventory.equipAreas[i].equipSlots;
				Predicate<vEquipSlot> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = (vEquipSlot slot) => slot.item.name.Equals(itemName));
				}
				if (equipSlots.Exists(predicate))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002197 RID: 8599 RVA: 0x000C2A4C File Offset: 0x000C0C4C
		public virtual bool ItemIsInSpecificEquipArea(int id, int indexOfArea)
		{
			return this.inventory && this.inventory.equipAreas.Length != 0 && indexOfArea <= this.inventory.equipAreas.Length - 1 && this.inventory.equipAreas[indexOfArea].equipSlots.Exists((vEquipSlot slot) => slot.item.id.Equals(id));
		}

		// Token: 0x06002198 RID: 8600 RVA: 0x000C2AC0 File Offset: 0x000C0CC0
		public virtual bool ItemIsInSpecificEquipArea(string itemName, int indexOfArea)
		{
			return this.inventory && this.inventory.equipAreas.Length != 0 && indexOfArea <= this.inventory.equipAreas.Length - 1 && this.inventory.equipAreas[indexOfArea].equipSlots.Exists((vEquipSlot slot) => slot.item.name.Equals(itemName));
		}

		// Token: 0x06002199 RID: 8601 RVA: 0x000C2B34 File Offset: 0x000C0D34
		public virtual bool EquipPointHasSomeItem(string equipPointName)
		{
			return this.equipPoints.Exists((EquipPoint ep) => ep.equipPointName.Equals(equipPointName) && ep.equipmentReference != null && ep.equipmentReference.item != null);
		}

		// Token: 0x0600219A RID: 8602 RVA: 0x000C2B68 File Offset: 0x000C0D68
		public virtual bool ItemIsInSomeEquipPont(int id)
		{
			return this.equipPoints.Exists((EquipPoint ep) => ep.equipmentReference != null && ep.equipmentReference.item != null && ep.equipmentReference.item.id.Equals(id));
		}

		// Token: 0x0600219B RID: 8603 RVA: 0x000C2B9C File Offset: 0x000C0D9C
		public virtual bool ItemIsInSomeEquipPont(string itemName)
		{
			return this.equipPoints.Exists((EquipPoint ep) => ep.equipmentReference != null && ep.equipmentReference.item != null && ep.equipmentReference.item.name.Equals(itemName));
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x000C2BD0 File Offset: 0x000C0DD0
		public virtual bool ItemIsInSpecificEquipPoint(int id, string equipPointName)
		{
			return this.equipPoints.Exists((EquipPoint ep) => ep.equipPointName.Equals(equipPointName) && ep.equipmentReference != null && ep.equipmentReference.item != null && ep.equipmentReference.item.id.Equals(id));
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x000C2C08 File Offset: 0x000C0E08
		public virtual bool ItemIsInSpecificEquipPoint(string itemName, string equipPointName)
		{
			return this.equipPoints.Exists((EquipPoint ep) => ep.equipPointName.Equals(equipPointName) && ep.equipmentReference != null && ep.equipmentReference.item != null && ep.equipmentReference.item.name.Equals(itemName));
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x000C2C40 File Offset: 0x000C0E40
		public virtual int GetAllAmount(int id)
		{
			List<vItem> items = this.GetItems(id);
			int num = 0;
			for (int i = 0; i < items.Count; i++)
			{
				num += items[i].amount;
			}
			return num;
		}

		// Token: 0x0600219F RID: 8607 RVA: 0x000C2C78 File Offset: 0x000C0E78
		public virtual List<vItem> GetItems()
		{
			return this.items;
		}

		// Token: 0x060021A0 RID: 8608 RVA: 0x000C2C80 File Offset: 0x000C0E80
		public virtual List<vItem> GetAllItems()
		{
			if (!this.itemListData)
			{
				return null;
			}
			return this.itemListData.items;
		}

		// Token: 0x060021A1 RID: 8609 RVA: 0x000C2C9C File Offset: 0x000C0E9C
		public virtual vItem GetItem(int id)
		{
			return this.items.Find((vItem i) => i.id == id);
		}

		// Token: 0x060021A2 RID: 8610 RVA: 0x000C2CD0 File Offset: 0x000C0ED0
		public virtual vItem GetItem(string itemName)
		{
			return this.items.Find((vItem i) => i.name == itemName);
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x000C2D04 File Offset: 0x000C0F04
		public virtual vItem GetItemInEquipPoint(string equipPointName)
		{
			EquipPoint equipPoint = this.equipPoints.Find((EquipPoint ep) => ep.equipPointName.Equals(equipPointName));
			if (equipPoint != null && equipPoint.equipmentReference != null && equipPoint.equipmentReference.item)
			{
				return equipPoint.equipmentReference.item;
			}
			return null;
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x000C2D60 File Offset: 0x000C0F60
		public virtual List<vItem> GetItems(int id)
		{
			return this.items.FindAll((vItem i) => i.id == id);
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x000C2D94 File Offset: 0x000C0F94
		public virtual List<vItem> GetItems(string itemName)
		{
			return this.items.FindAll((vItem i) => i.name == itemName);
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x000C2DC8 File Offset: 0x000C0FC8
		public virtual List<vItem> GetItemsInEquipArea(int indexOfArea)
		{
			List<vItem> list = new List<vItem>();
			if (!this.inventory || this.inventory.equipAreas.Length == 0 || indexOfArea > this.inventory.equipAreas.Length - 1)
			{
				return list;
			}
			List<vEquipSlot> list2 = this.inventory.equipAreas[indexOfArea].equipSlots.FindAll((vEquipSlot s) => s.item);
			for (int i = 0; i < list2.Count; i++)
			{
				list.Add(list2[i].item);
			}
			return list;
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x000C2E68 File Offset: 0x000C1068
		public virtual List<vItem> GetAllItemInAllEquipAreas()
		{
			List<vItem> list = new List<vItem>();
			if (!this.inventory || this.inventory.equipAreas.Length == 0)
			{
				return list;
			}
			for (int i = 0; i < this.inventory.equipAreas.Length; i++)
			{
				List<vEquipSlot> list2 = this.inventory.equipAreas[i].equipSlots.FindAll((vEquipSlot s) => s.item);
				for (int j = 0; j < list2.Count; j++)
				{
					list.Add(list2[j].item);
				}
			}
			return list;
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x000C2F0C File Offset: 0x000C110C
		protected virtual vEquipment EquipEquipment(vItem item, bool startActive = true)
		{
			if (this.equipments.ContainsKey(item))
			{
				if (!startActive)
				{
					if (this.debugMode)
					{
						Debug.Log(string.Format("<color=green>Disable Equipment {0} </color>", this.equipments[item].gameObject));
					}
					this.equipments[item].gameObject.SetActive(false);
				}
				else
				{
					if (this.debugMode)
					{
						Debug.Log(string.Format("<color=green>Enable Equipment {0} </color>", this.equipments[item].gameObject));
					}
					this.equipments[item].gameObject.SetActive(true);
				}
				return this.equipments[item];
			}
			if (item.originalObject)
			{
				vEquipment vEquipment = item.originalObject.GetComponent<vEquipment>();
				if (vEquipment != null)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(item.originalObject);
					if (!startActive)
					{
						if (this.debugMode)
						{
							Debug.Log(string.Format("<color=green>Instantiate and disable Equipment {0} </color>", gameObject.gameObject));
						}
						gameObject.gameObject.SetActive(false);
					}
					else if (this.debugMode)
					{
						Debug.Log(string.Format("<color=green>Instantiate and enable Equipment {0} </color>", gameObject.gameObject));
					}
					gameObject.transform.SetParent(this.equipmentContainer.transform);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localEulerAngles = Vector3.zero;
					vEquipment = gameObject.GetComponent<vEquipment>();
					this.equipments.Add(item, vEquipment);
					this.equipmentsObject.Add(gameObject, vEquipment);
					return vEquipment;
				}
			}
			return null;
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x000C3094 File Offset: 0x000C1294
		protected virtual vEquipment EquipEquipment(vItem item, Vector3 position, Quaternion rotation, Transform parent = null)
		{
			vEquipment vEquipment = this.EquipEquipment(item, true);
			if (vEquipment)
			{
				if (parent)
				{
					vEquipment.transform.parent = parent;
				}
				vEquipment.transform.position = position;
				vEquipment.transform.rotation = rotation;
				vEquipment.OnEquip(item);
				return vEquipment;
			}
			return null;
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x000C30EC File Offset: 0x000C12EC
		protected virtual void UnequipEquipment(vItem item)
		{
			if (this.equipments.ContainsKey(item))
			{
				if (this.debugMode)
				{
					Debug.Log(string.Format("<color=red>Disable Equipment {0} </color>", this.equipments[item].gameObject));
				}
				this.equipments[item].gameObject.SetActive(false);
				this.equipments[item].gameObject.transform.SetParent(this.equipmentContainer.transform);
				this.equipments[item].gameObject.transform.localPosition = Vector3.zero;
				this.equipments[item].gameObject.transform.localEulerAngles = Vector3.zero;
				this.equipments[item].equipPoint = null;
				this.equipments[item].OnUnequip(item);
			}
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x000C31D4 File Offset: 0x000C13D4
		public virtual void AddItem(ItemReference itemReference, bool ignoreItemAnimation = false, UnityAction<vItem> onFinish = null)
		{
			if (itemReference != null && this.itemListData != null && this.itemListData.items.Count > 0)
			{
				vItem item = this.itemListData.items.Find((vItem t) => t.id.Equals(itemReference.id));
				if (item)
				{
					List<vItem> list = this.items.FindAll((vItem i) => i.stackable && i.id == item.id && i.amount < i.maxStack);
					if (list.Count == 0)
					{
						vItem vItem = Object.Instantiate<vItem>(item);
						vItem.name = vItem.name.Replace("(Clone)", string.Empty);
						if (itemReference.attributes != null && vItem.attributes != null && item.attributes.Count == itemReference.attributes.Count)
						{
							for (int j = 0; j < vItem.attributes.Count; j++)
							{
								itemReference.attributes[j].CopyTo(vItem.attributes[j]);
							}
						}
						vItem.amount = 0;
						int num = 0;
						while (num < item.maxStack && vItem.amount < vItem.maxStack && itemReference.amount > 0)
						{
							vItem.amount++;
							itemReference.amount--;
							num++;
						}
						this.items.Add(vItem);
						this.onAddItem.Invoke(vItem);
						this.onAddItemID.Invoke(vItem.id);
						if (itemReference.addToEquipArea)
						{
							itemReference.addToEquipArea = false;
							this.AutoEquipItem(vItem, itemReference.indexArea, itemReference.autoEquip, ignoreItemAnimation);
						}
						if (itemReference.amount > 0)
						{
							this.AddItem(itemReference, false, onFinish);
						}
						else if (onFinish != null)
						{
							onFinish(vItem);
						}
					}
					else
					{
						int num2 = this.items.IndexOf(list[0]);
						bool flag = false;
						int num3 = 0;
						while (num3 < this.items[num2].maxStack && this.items[num2].amount < this.items[num2].maxStack && itemReference.amount > 0)
						{
							this.items[num2].amount++;
							itemReference.amount--;
							flag = true;
							num3++;
						}
						if (flag)
						{
							this.onChangeItemAmount.Invoke(this.items[num2]);
						}
						if (itemReference.amount > 0)
						{
							this.AddItem(itemReference, false, onFinish);
						}
						else if (flag && onFinish != null)
						{
							onFinish(this.items[num2]);
						}
					}
				}
			}
			this.inventory.UpdateInventory();
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x000C3504 File Offset: 0x000C1704
		public virtual void AutoEquipItem(vItem item, int indexArea, bool autoEquip = false, bool ignoreItemAnimation = true)
		{
			if (!this.inventory)
			{
				return;
			}
			if (this.inventory.equipAreas != null && this.inventory.equipAreas.Length != 0 && indexArea < this.inventory.equipAreas.Length)
			{
				vEquipSlot vEquipSlot = this.inventory.equipAreas[indexArea].equipSlots.Find((vEquipSlot slot) => slot.isValid && slot.item == null && slot.itemType.Contains(item.type));
				if (vEquipSlot == null && autoEquip && this.inventory.equipAreas[indexArea].currentEquippedSlot && this.inventory.equipAreas[indexArea].currentEquippedSlot.item == null)
				{
					vEquipSlot = this.inventory.equipAreas[indexArea].currentEquippedSlot;
				}
				if (vEquipSlot && !this.inventory.equipAreas[indexArea].equipSlots.Exists((vEquipSlot slot) => slot.item == item))
				{
					int num = this.inventory.equipAreas[indexArea].equipSlots.IndexOf(vEquipSlot);
					if (vEquipSlot.item != item)
					{
						this.EquipItemToEquipSlot(indexArea, num, item, autoEquip, ignoreItemAnimation);
						return;
					}
				}
			}
			else if (this.debugMode)
			{
				Debug.LogWarning("Fail to auto equip " + item.name + " on equipArea " + indexArea.ToString());
			}
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x000C3678 File Offset: 0x000C1878
		protected virtual void EquipItem(vEquipArea equipArea, vItem item)
		{
			this.CheckIsLockedToEquip();
			if (!item)
			{
				return;
			}
			item.isEquiped = true;
			this.onEquipItem.Invoke(equipArea, item);
			if (this.debugMode)
			{
				Debug.Log(string.Format("<color=green>Start Equip {0} </color>", item));
			}
			this.inventory.UpdateInventory();
			if (item != equipArea.currentEquippedItem)
			{
				if (this.debugMode)
				{
					Debug.Log(string.Format("<color=green>Not Current Equip {0} </color>{1}", item, equipArea.indexOfEquippedItem), equipArea.currentEquippedItem);
				}
				this.EquipEquipment(item, false);
				OnChangeEquipmentEvent onChangeEquipmentEvent = this.onFinishEquipItem;
				if (onChangeEquipmentEvent != null)
				{
					onChangeEquipmentEvent.Invoke(equipArea, item);
				}
				if (this.debugMode)
				{
					Debug.Log(string.Format("<color=green>Finish Equip {0} </color>", item));
				}
				return;
			}
			EquipPoint equipPoint = this.equipPoints.Find((EquipPoint ep) => ep.equipPointName == equipArea.equipPointName);
			if (equipPoint != null && item != null && equipPoint.equipmentReference.item != item && item.originalObject && item.originalObject.GetComponentInChildren<vEquipment>() != null)
			{
				equipPoint.area = equipArea;
				base.StartCoroutine(this.EquipItemRoutine(equipPoint, item, delegate
				{
					OnChangeEquipmentEvent onChangeEquipmentEvent2 = this.onFinishEquipItem;
					if (onChangeEquipmentEvent2 == null)
					{
						return;
					}
					onChangeEquipmentEvent2.Invoke(equipArea, item);
				}));
			}
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x000C3832 File Offset: 0x000C1A32
		protected virtual IEnumerator EquipItemRoutine(EquipPoint equipPoint, vItem item, UnityAction onFinish)
		{
			this.LockInventoryInput(true);
			while (this.inEquip || this.IsAnimatorTag("IsEquipping"))
			{
				yield return new WaitForEndOfFrame();
			}
			if (!equipPoint.area.isLockedToEquip && this.playItemAnimation)
			{
				if (this.debugMode)
				{
					Debug.Log(string.Format("<color=green>Play Equip Animation {0} </color>", item));
				}
				this.equipTimer = item.enableDelayTime;
				this.animator.SetBool("FlipEquip", equipPoint.equipPointName.Contains("Left"));
				this.animator.CrossFade(item.EnableAnim, 0.25f);
			}
			if (!this.inEquip)
			{
				this.inEquip = true;
				this.inventory.canEquip = false;
				if (equipPoint != null)
				{
					if (item.originalObject)
					{
						if (equipPoint.equipmentReference != null && equipPoint.equipmentReference.equipedObject != null && equipPoint.equipmentReference.item)
						{
							this.UnequipEquipment(equipPoint.equipmentReference.item);
							equipPoint.equipmentReference.item = null;
						}
						if (!equipPoint.area.isLockedToEquip && this.playItemAnimation && !string.IsNullOrEmpty(item.EnableAnim))
						{
							if (this.debugMode && this.equipTimer > 0f)
							{
								Debug.Log(string.Format("<color=green>In Equip delay {0} </color>", item));
							}
							while (this.equipTimer > 0f && !(item == null))
							{
								yield return null;
								this.equipTimer -= vTime.deltaTime;
							}
						}
						this.inEquip = false;
						Transform transform = equipPoint.handler.customHandlers.Find((Transform p) => p.name == item.customHandler);
						Transform transform2 = ((transform != null) ? transform : equipPoint.handler.defaultHandler);
						vEquipment vEquipment = this.EquipEquipment(item, transform2.position, transform2.rotation, transform2);
						if (equipPoint.area.isLockedToEquip)
						{
							vEquipment.gameObject.SetActive(false);
						}
						vEquipment.equipPoint = equipPoint;
						equipPoint.equipmentReference.item = item;
						equipPoint.equipmentReference.equipedObject = vEquipment.gameObject;
						equipPoint.onInstantiateEquiment.Invoke(vEquipment.gameObject);
					}
					else if (equipPoint.equipmentReference != null && equipPoint.equipmentReference.equipedObject != null && equipPoint.equipmentReference.item)
					{
						this.UnequipEquipment(equipPoint.equipmentReference.item);
						this.equipments[equipPoint.equipmentReference.item].equipPoint = null;
						equipPoint.equipmentReference.item = null;
					}
				}
			}
			this.LockInventoryInput(false);
			if (onFinish != null)
			{
				onFinish();
			}
			if (this.debugMode)
			{
				Debug.Log(string.Format("<color=green>Finish Equip {0} </color>", item));
			}
			this.inEquip = false;
			this.inventory.canEquip = true;
			yield break;
		}

		// Token: 0x060021AF RID: 8623 RVA: 0x000C3858 File Offset: 0x000C1A58
		public virtual void EquipItemToEquipSlot(int indexOfArea, int indexOfSlot, vItem item, bool autoEquip = false, bool ignoreItemAnimation = false)
		{
			if (!this.inventory)
			{
				return;
			}
			if (ignoreItemAnimation)
			{
				this.temporarilyIgnoreItemAnimation = ignoreItemAnimation;
			}
			if (this.inventory.equipAreas != null && indexOfArea < this.inventory.equipAreas.Length)
			{
				vEquipArea vEquipArea = this.inventory.equipAreas[indexOfArea];
				if (vEquipArea != null)
				{
					vEquipArea.AddItemToEquipSlot(indexOfSlot, item, autoEquip);
				}
			}
			if (ignoreItemAnimation)
			{
				this.temporarilyIgnoreItemAnimation = false;
			}
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x000C38C8 File Offset: 0x000C1AC8
		public virtual void EquipItemToCurrentEquipSlot(vItem item, int indexOfArea, bool ignoreItemAnimation = true)
		{
			if (!this.inventory && this.items.Count == 0)
			{
				return;
			}
			if (ignoreItemAnimation)
			{
				this.temporarilyIgnoreItemAnimation = ignoreItemAnimation;
			}
			if (this.inventory.equipAreas != null && indexOfArea < this.inventory.equipAreas.Length)
			{
				this.inventory.equipAreas[indexOfArea].AddCurrentItem(item);
			}
			if (ignoreItemAnimation)
			{
				this.temporarilyIgnoreItemAnimation = false;
			}
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x000C3934 File Offset: 0x000C1B34
		protected virtual void UnequipItem(vEquipArea equipArea, vItem item)
		{
			if (!item)
			{
				return;
			}
			item.isEquiped = false;
			this.onUnequipItem.Invoke(equipArea, item);
			if (this.debugMode)
			{
				Debug.Log(string.Format("<color=red>Start Unequip {0}</color>", item));
			}
			EquipPoint equipPoint = this.equipPoints.Find((EquipPoint ep) => ep.equipPointName == equipArea.equipPointName && ep.equipmentReference.item != null && ep.equipmentReference.item == item);
			if (equipPoint != null && item != null)
			{
				equipPoint.onInstantiateEquiment.Invoke(null);
				this.unequipTimer = item.disableDelayTime;
				if (item.originalObject && item.originalObject.GetComponentInChildren<vEquipment>() != null)
				{
					if (!this.inventory.isOpen && this.playItemAnimation && !this.inEquip && equipPoint.equipmentReference.equipedObject.activeInHierarchy)
					{
						if (this.debugMode)
						{
							Debug.Log(string.Format("<color=red>Play Unequip Animation {0}</color>", item));
						}
						this.animator.SetBool("FlipEquip", equipArea.equipPointName.Contains("Left"));
						this.animator.CrossFade(item.DisableAnim, 0.25f);
					}
					base.StartCoroutine(this.UnequipItemRoutine(equipPoint, item, delegate
					{
						OnChangeEquipmentEvent onChangeEquipmentEvent = this.onFinishUnequipItem;
						if (onChangeEquipmentEvent == null)
						{
							return;
						}
						onChangeEquipmentEvent.Invoke(equipArea, item);
					}));
				}
			}
			else if (item != null)
			{
				if (this.debugMode)
				{
					Debug.Log(string.Format("<color=red>Finish Unequip {0}</color>", item));
				}
				this.onFinishUnequipItem.Invoke(equipArea, item);
			}
			this.inventory.UpdateInventory();
			this.CheckIsLockedToEquip();
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x000C3B28 File Offset: 0x000C1D28
		public virtual void UnequipItem(vItem item, bool ignoreItemAnimation = true)
		{
			Predicate<vEquipSlot> <>9__1;
			vEquipArea vEquipArea = Array.Find<vEquipArea>(this.inventory.equipAreas, delegate(vEquipArea e)
			{
				List<vEquipSlot> equipSlots = e.equipSlots;
				Predicate<vEquipSlot> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = (vEquipSlot s) => s.item != null && s.item.id.Equals(item.id));
				}
				return equipSlots.Exists(predicate);
			});
			if (vEquipArea != null)
			{
				if (ignoreItemAnimation)
				{
					this.temporarilyIgnoreItemAnimation = ignoreItemAnimation;
				}
				this.UnequipItem(vEquipArea, item);
			}
			this.inventory.UpdateInventory();
			if (ignoreItemAnimation)
			{
				this.temporarilyIgnoreItemAnimation = false;
			}
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x000C3B94 File Offset: 0x000C1D94
		public virtual void UnequipItemOfEquipSlot(int indexOfArea, int indexOfSlot, bool ignoreItemAnimation = true)
		{
			if (!this.inventory)
			{
				return;
			}
			if (ignoreItemAnimation)
			{
				this.temporarilyIgnoreItemAnimation = ignoreItemAnimation;
			}
			if (this.inventory.equipAreas != null && indexOfArea < this.inventory.equipAreas.Length)
			{
				vEquipArea vEquipArea = this.inventory.equipAreas[indexOfArea];
				if (vEquipArea != null)
				{
					vEquipArea.RemoveItemOfEquipSlot(indexOfSlot);
				}
			}
			if (ignoreItemAnimation)
			{
				this.temporarilyIgnoreItemAnimation = false;
			}
		}

		// Token: 0x060021B4 RID: 8628 RVA: 0x000C3C00 File Offset: 0x000C1E00
		public virtual void UnequipCurrentEquipedItem(int indexOfArea, bool ignoreItemAnimation = true)
		{
			if (!this.inventory && this.items.Count == 0)
			{
				return;
			}
			if (ignoreItemAnimation)
			{
				this.temporarilyIgnoreItemAnimation = ignoreItemAnimation;
			}
			if (this.inventory.equipAreas != null && indexOfArea < this.inventory.equipAreas.Length)
			{
				this.inventory.equipAreas[indexOfArea].RemoveCurrentItem();
			}
			if (ignoreItemAnimation)
			{
				this.temporarilyIgnoreItemAnimation = false;
			}
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x000C3C6B File Offset: 0x000C1E6B
		protected virtual IEnumerator UnequipItemRoutine(EquipPoint equipPoint, vItem item, UnityAction onFinish)
		{
			this.LockInventoryInput(true);
			if (!this.inEquip)
			{
				this.inEquip = true;
				this.inventory.canEquip = false;
				if (equipPoint != null && equipPoint.equipmentReference != null && equipPoint.equipmentReference.equipedObject != null)
				{
					if (!this.inventory.isOpen && this.playItemAnimation)
					{
						if (this.debugMode && this.unequipTimer > 0f)
						{
							Debug.Log(string.Format("<color=red>In Unequip delay {0} </color>", item));
						}
						while (this.unequipTimer > 0f && !string.IsNullOrEmpty(item.DisableAnim))
						{
							this.unequipTimer -= vTime.deltaTime;
							yield return null;
						}
					}
					if (equipPoint != null && equipPoint.equipmentReference != null && equipPoint.equipmentReference.equipedObject)
					{
						this.UnequipEquipment(item);
						equipPoint.equipmentReference.item = null;
					}
				}
				this.inEquip = false;
				this.inventory.canEquip = true;
			}
			if (this.debugMode)
			{
				Debug.Log(string.Format("<color=red>Finish Unequip {0}</color>", item));
			}
			if (onFinish != null)
			{
				onFinish();
			}
			this.LockInventoryInput(false);
			yield break;
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x000C3C90 File Offset: 0x000C1E90
		public virtual bool CanUseItem(vItem item)
		{
			if (this.canUseItemDelegate != null)
			{
				List<bool> list = new List<bool>();
				this.canUseItemDelegate(item, ref list);
				return !list.Contains(false);
			}
			return item.canBeUsed;
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x000C3CCA File Offset: 0x000C1ECA
		public virtual void UseItem(vItem item)
		{
			if (item)
			{
				if (this.CanUseItem(item))
				{
					base.StartCoroutine(this.UseItemRoutine(item));
					return;
				}
				this.onUseItemFail.Invoke(item);
			}
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x000C3CF8 File Offset: 0x000C1EF8
		protected virtual IEnumerator UseItemRoutine(vItem item)
		{
			this.usingItem = true;
			this.LockInventoryInput(true);
			this.onStartItemUsage.Invoke(item);
			bool canUse = this.CanUseItem(item);
			if (canUse)
			{
				float time = item.enableDelayTime;
				if (!this.inventory.isOpen && this.playItemAnimation && !string.IsNullOrEmpty(item.EnableAnim))
				{
					this.animator.SetBool("FlipAnimation", false);
					this.animator.CrossFade(item.EnableAnim, 0.25f);
					while (this.usingItem && time > 0f && canUse)
					{
						canUse = this.CanUseItem(item);
						time -= vTime.deltaTime;
						yield return null;
					}
				}
				if (this.usingItem && canUse)
				{
					if (item.destroyAfterUse)
					{
						item.amount--;
					}
					this.onUseItem.Invoke(item);
					if (item.attributes != null && item.attributes.Count > 0 && this.applyAttributeEvents.Count > 0)
					{
						using (List<ApplyAttributeEvent>.Enumerator enumerator = this.applyAttributeEvents.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								ApplyAttributeEvent attributeEvent = enumerator.Current;
								foreach (vItemAttribute vItemAttribute in item.attributes.FindAll((vItemAttribute a) => a.name.Equals(attributeEvent.attribute)))
								{
									attributeEvent.onApplyAttribute.Invoke(vItemAttribute.value);
								}
							}
						}
					}
					if (item.destroyAfterUse && item.amount <= 0 && this.items.Contains(item))
					{
						this.DestroyItem(item);
					}
					else
					{
						this.onRemoveItemID.Invoke(item.id);
					}
					this.usingItem = false;
					this.inventory.CheckEquipmentChanges();
				}
				else
				{
					this.onUseItemFail.Invoke(item);
				}
			}
			else
			{
				this.onUseItemFail.Invoke(item);
			}
			this.LockInventoryInput(false);
			this.inventory.UpdateInventory();
			yield break;
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x000C3D0E File Offset: 0x000C1F0E
		public virtual void DestroyItem(vItem item)
		{
			this.DestroyItem(item, item.amount);
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x000C3D20 File Offset: 0x000C1F20
		public virtual void DestroyItem(vItem item, int amount)
		{
			item.amount -= amount;
			this.onDestroyItem.Invoke(item, amount);
			if (item.amount <= 0)
			{
				Predicate<vEquipSlot> <>9__1;
				vEquipArea vEquipArea = Array.Find<vEquipArea>(this.inventory.equipAreas, delegate(vEquipArea e)
				{
					List<vEquipSlot> equipSlots = e.equipSlots;
					Predicate<vEquipSlot> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = (vEquipSlot s) => s.item != null && s.item.id.Equals(item.id));
					}
					return equipSlots.Exists(predicate);
				});
				if (vEquipArea != null)
				{
					this.temporarilyIgnoreItemAnimation = true;
					vEquipArea.UnequipItem(item);
					this.temporarilyIgnoreItemAnimation = false;
				}
				int id = item.id;
				if (this.items.Contains(item))
				{
					this.items.Remove(item);
				}
				if (this.equipments.ContainsKey(item))
				{
					vEquipment vEquipment = this.equipments[item];
					if (vEquipment != null)
					{
						Object.Destroy(vEquipment.gameObject);
					}
					this.equipments.Remove(item);
				}
				Object.Destroy(item);
				this.onRemoveItemID.Invoke(id);
			}
			this.inventory.UpdateInventory();
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x000C3E50 File Offset: 0x000C2050
		public virtual void DestroyAllItems()
		{
			for (int i = this.items.Count - 1; i >= 0; i--)
			{
				this.DestroyItem(this.items[i]);
			}
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x000C3E88 File Offset: 0x000C2088
		public virtual void DestroyCurrentEquipedItem(int indexOfArea, bool ignoreItemAnimation = true)
		{
			if (!this.inventory && this.items.Count == 0)
			{
				return;
			}
			if (ignoreItemAnimation)
			{
				this.temporarilyIgnoreItemAnimation = ignoreItemAnimation;
			}
			if (this.inventory.equipAreas != null && indexOfArea < this.inventory.equipAreas.Length)
			{
				vItem currentEquippedItem = this.inventory.equipAreas[indexOfArea].currentEquippedItem;
				if (currentEquippedItem)
				{
					this.DestroyItem(currentEquippedItem, currentEquippedItem.amount);
				}
			}
			if (ignoreItemAnimation)
			{
				this.temporarilyIgnoreItemAnimation = false;
			}
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x000C3F09 File Offset: 0x000C2109
		public virtual void DropItem(vItem item)
		{
			this.DropItem(item, item.amount);
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x000C3F18 File Offset: 0x000C2118
		public virtual void DropItem(vItem item, int amount)
		{
			item.amount -= amount;
			if (item.dropObject != null)
			{
				vItemCollection component = Object.Instantiate<GameObject>(item.dropObject, base.transform.position, base.transform.rotation).GetComponent<vItemCollection>();
				if (component != null)
				{
					component.items.Clear();
					ItemReference itemReference = new ItemReference(item.id);
					itemReference.amount = amount;
					itemReference.attributes = new List<vItemAttribute>(item.attributes);
					component.items.Add(itemReference);
				}
			}
			this.onDropItem.Invoke(item, amount);
			if (item.amount <= 0 && this.items.Contains(item))
			{
				Predicate<vEquipSlot> <>9__1;
				vEquipArea vEquipArea = Array.Find<vEquipArea>(this.inventory.equipAreas, delegate(vEquipArea e)
				{
					List<vEquipSlot> equipSlots = e.equipSlots;
					Predicate<vEquipSlot> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = (vEquipSlot s) => s.item != null && s.item.id.Equals(item.id));
					}
					return equipSlots.Exists(predicate);
				});
				if (vEquipArea != null)
				{
					vEquipArea.UnequipItem(item);
				}
				this.items.Remove(item);
				this.DestroyItem(item);
			}
			this.inventory.UpdateInventory();
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x000C4060 File Offset: 0x000C2260
		public virtual void DropAllItens()
		{
			List<ItemReference> list = new List<ItemReference>();
			List<GameObject> list2 = new List<GameObject>();
			for (int i = this.items.Count - 1; i >= 0; i--)
			{
				vItem item = this.items[i];
				ItemReference itemReference = list.Find((ItemReference _item) => _item.id == item.id);
				if (itemReference == null)
				{
					itemReference = new ItemReference(item.id);
					list.Add(itemReference);
					list2.Add(item.dropObject);
				}
				itemReference.amount += item.amount;
				this.DestroyItem(item);
			}
			for (int j = 0; j < list2.Count; j++)
			{
				GameObject gameObject = list2[j];
				ItemReference itemReference2 = list[j];
				if (gameObject)
				{
					GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, base.transform.position, base.transform.rotation);
					vItemCollection vItemCollection = gameObject2.GetComponent<vItemCollection>();
					if (!vItemCollection)
					{
						vItemCollection = gameObject2.AddComponent<vItemCollection>();
					}
					if (vItemCollection != null)
					{
						vItemCollection.items.Clear();
						vItemCollection.items.Add(itemReference2);
					}
				}
			}
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x000C41B0 File Offset: 0x000C23B0
		public virtual void DropCurrentEquippedItem(int indexOfArea, bool ignoreItemAnimation = true)
		{
			if (!this.inventory && this.items.Count == 0)
			{
				return;
			}
			if (ignoreItemAnimation)
			{
				this.temporarilyIgnoreItemAnimation = ignoreItemAnimation;
			}
			if (this.inventory.equipAreas != null && indexOfArea < this.inventory.equipAreas.Length)
			{
				vItem currentEquippedItem = this.inventory.equipAreas[indexOfArea].currentEquippedItem;
				if (currentEquippedItem)
				{
					this.DropItem(currentEquippedItem, currentEquippedItem.amount);
				}
			}
			if (ignoreItemAnimation)
			{
				this.temporarilyIgnoreItemAnimation = false;
			}
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x000C4234 File Offset: 0x000C2434
		public virtual void CollectItem(ItemReference itemRef, float onCollectDelay = 0f, float textDelay = 0f, bool ignoreItemAnimation = true)
		{
			vItem vItem = this.items.Find((vItem i) => i.id == itemRef.id);
			if (vItem != null && !vItem.stackable && !vItem.createNewItem)
			{
				OnCollectItemRefEvent onCollectItemRefEvent = this.onCollectItemRef;
				if (onCollectItemRefEvent == null)
				{
					return;
				}
				onCollectItemRefEvent.Invoke(itemRef);
				return;
			}
			else
			{
				if (!this.inCollectItemRoutine)
				{
					this.itemsToCollect.Add(itemRef);
					base.StartCoroutine(this.CollectItemsRoutine(onCollectDelay, textDelay, ignoreItemAnimation));
					return;
				}
				this.itemsToCollect.Add(itemRef);
				return;
			}
		}

		// Token: 0x060021C2 RID: 8642 RVA: 0x000C42D4 File Offset: 0x000C24D4
		public virtual void CollectItems(List<ItemReference> collection, float onCollectDelay = 0f, float textDelay = 0f, bool ignoreItemAnimation = true)
		{
			foreach (ItemReference itemReference in collection)
			{
				this.CollectItem(itemReference, onCollectDelay, textDelay, ignoreItemAnimation);
			}
		}

		// Token: 0x060021C3 RID: 8643 RVA: 0x000C4328 File Offset: 0x000C2528
		protected virtual IEnumerator CollectItemsRoutine(float onCollectDelay = 0f, float textDelay = 0f, bool ignoreItemAnimation = true)
		{
			while (this.inCollectItemRoutine)
			{
				yield return null;
			}
			this.inCollectItemRoutine = true;
			yield return new WaitForSeconds(onCollectDelay);
			List<vItemManager.CollectedItemInfo> collectedItems = new List<vItemManager.CollectedItemInfo>();
			for (int j = 0; j < this.inventory.equipAreas.Length; j++)
			{
				this.inventory.equipAreas[j].ignoreEquipEvents = true;
			}
			for (int k = 0; k < this.itemsToCollect.Count; k++)
			{
				ItemReference itemReference = this.itemsToCollect[k];
				vItemManager.CollectedItemInfo collectedItemInfo = default(vItemManager.CollectedItemInfo);
				collectedItemInfo.amount = this.itemsToCollect[k].amount;
				UnityAction<vItem> equipAction = null;
				if (itemReference.addToEquipArea)
				{
					bool autoEquip = this.itemsToCollect[k].autoEquip;
					int indexOfArea = this.itemsToCollect[k].indexArea;
					this.itemsToCollect[k].addToEquipArea = false;
					this.itemsToCollect[k].autoEquip = false;
					equipAction = delegate(vItem _item)
					{
						this.AutoEquipItem(_item, indexOfArea, autoEquip, true);
					};
				}
				UnityAction<vItem> unityAction = delegate(vItem _item)
				{
					collectedItemInfo.item = _item;
					collectedItems.Add(collectedItemInfo);
					UnityAction<vItem> equipAction2 = equipAction;
					if (equipAction2 == null)
					{
						return;
					}
					equipAction2(_item);
				};
				this.AddItem(this.itemsToCollect[k].Clone(), ignoreItemAnimation, unityAction);
			}
			this.inCollectItemRoutine = false;
			this.itemsToCollect.Clear();
			base.StartCoroutine(this.DisplayCollectedItems(textDelay, collectedItems.ToArray()));
			new List<vItemManager.CollectedItemInfo>();
			if (ignoreItemAnimation)
			{
				this.temporarilyIgnoreItemAnimation = true;
			}
			int num;
			for (int i = 0; i < this.inventory.equipAreas.Length; i = num + 1)
			{
				vEquipArea area = this.inventory.equipAreas[i];
				if (area.isLockedToEquip)
				{
					this.temporarilyIgnoreItemAnimation = true;
				}
				else
				{
					while (!ignoreItemAnimation && (this.inEquip || this.IsAnimatorTag("IsEquipping")))
					{
						yield return null;
					}
				}
				area.ignoreEquipEvents = false;
				area.EquipCurrentSlot();
				if (area.isLockedToEquip)
				{
					this.temporarilyIgnoreItemAnimation = false;
				}
				area = null;
				num = i;
			}
			if (ignoreItemAnimation)
			{
				this.temporarilyIgnoreItemAnimation = false;
			}
			yield break;
		}

		// Token: 0x060021C4 RID: 8644 RVA: 0x000C434C File Offset: 0x000C254C
		protected virtual IEnumerator DisplayCollectedItems(float delay, params vItemManager.CollectedItemInfo[] _items)
		{
			int num;
			for (int i = 0; i < _items.Length; i = num + 1)
			{
				this.onCollectItem.Invoke(_items[i]);
				if (vItemCollectionDisplay.Instance)
				{
					string text = string.Format("Acquired: {0} {1}", _items[i].amount, _items[i].item.name);
					vItemCollectionDisplay.Instance.FadeText(text, 4f, 0.25f);
				}
				yield return new WaitForSeconds(delay);
				num = i;
			}
			yield break;
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x000C436C File Offset: 0x000C256C
		public virtual void OnReceiveAction(vTriggerGenericAction action)
		{
			vItemCollection componentInChildren = action.GetComponentInChildren<vItemCollection>();
			if (componentInChildren != null && componentInChildren.enabled && componentInChildren.items.Count > 0)
			{
				List<ItemReference> list = componentInChildren.items.vCopy<ItemReference>();
				this.CollectItems(list, componentInChildren.onCollectDelay, componentInChildren.textDelay, componentInChildren.ignoreItemAnimation);
			}
		}

		// Token: 0x060021C7 RID: 8647 RVA: 0x000C43E2 File Offset: 0x000C25E2
		bool IActionController.get_enabled()
		{
			return base.enabled;
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x000C43EA File Offset: 0x000C25EA
		void IActionController.set_enabled(bool value)
		{
			base.enabled = value;
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x000B0592 File Offset: 0x000AE792
		GameObject IActionController.get_gameObject()
		{
			return base.gameObject;
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x000AD00A File Offset: 0x000AB20A
		Transform IActionController.get_transform()
		{
			return base.transform;
		}

		// Token: 0x060021CB RID: 8651 RVA: 0x000C43F3 File Offset: 0x000C25F3
		string IActionController.get_name()
		{
			return base.name;
		}

		// Token: 0x060021CC RID: 8652 RVA: 0x000C43FB File Offset: 0x000C25FB
		Type IActionController.GetType()
		{
			return base.GetType();
		}

		// Token: 0x040021FF RID: 8703
		[vHelpBox("Place a Inventory Prefab inside your Character, it will be auto-assigned when you enter Playmode.\nYou can find the prefab at the Inventory/Prefabs folder", vHelpBoxAttribute.MessageType.Info)]
		public vInventory inventory;

		// Token: 0x04002200 RID: 8704
		[vHelpBox("You can find the default ItemListData at the Inventory/ItemListData folder, or create a new list at the Invector Menu", vHelpBoxAttribute.MessageType.Info)]
		public vItemListData itemListData;

		// Token: 0x04002201 RID: 8705
		public List<ItemReference> startItems = new List<ItemReference>();

		// Token: 0x04002202 RID: 8706
		public List<EquipPoint> equipPoints;

		// Token: 0x04002203 RID: 8707
		public List<ApplyAttributeEvent> applyAttributeEvents;

		// Token: 0x04002204 RID: 8708
		public bool debugMode;

		// Token: 0x04002205 RID: 8709
		[HideInInspector]
		public List<vItemType> itemsFilter = new List<vItemType>();

		// Token: 0x04002206 RID: 8710
		public OnHandleItemEvent onStartItemUsage;

		// Token: 0x04002207 RID: 8711
		public OnHandleItemEvent onUseItem;

		// Token: 0x04002208 RID: 8712
		public OnHandleItemEvent onUseItemFail;

		// Token: 0x04002209 RID: 8713
		public OnHandleItemEvent onAddItem;

		// Token: 0x0400220A RID: 8714
		public OnHandleItemEvent onChangeItemAmount;

		// Token: 0x0400220B RID: 8715
		public OnHandleItemIDEvent onAddItemID;

		// Token: 0x0400220C RID: 8716
		public OnHandleItemIDEvent onRemoveItemID;

		// Token: 0x0400220D RID: 8717
		public OnCollectItemEvent onCollectItem;

		// Token: 0x0400220E RID: 8718
		public OnCollectItemRefEvent onCollectItemRef;

		// Token: 0x0400220F RID: 8719
		public OnChangeItemAmount onDestroyItem;

		// Token: 0x04002210 RID: 8720
		public OnChangeItemAmount onDropItem;

		// Token: 0x04002211 RID: 8721
		public OnOpenCloseInventory onOpenCloseInventory;

		// Token: 0x04002212 RID: 8722
		public OnChangeEquipmentEvent onEquipItem;

		// Token: 0x04002213 RID: 8723
		public OnChangeEquipmentEvent onUnequipItem;

		// Token: 0x04002214 RID: 8724
		public OnChangeEquipmentEvent onFinishEquipItem;

		// Token: 0x04002215 RID: 8725
		public OnChangeEquipmentEvent onFinishUnequipItem;

		// Token: 0x04002216 RID: 8726
		public OnSelectEquipArea onSetLockedToEquip;

		// Token: 0x04002217 RID: 8727
		public UnityEvent onSaveItems;

		// Token: 0x04002218 RID: 8728
		public UnityEvent onLoadItems;

		// Token: 0x0200058A RID: 1418
		// (Invoke) Token: 0x060021CE RID: 8654
		public delegate void CanUseItemDelegate(vItem item, ref List<bool> validationList);

		// Token: 0x0200058B RID: 1419
		public struct CollectedItemInfo
		{
			// Token: 0x04002229 RID: 8745
			public vItem item;

			// Token: 0x0400222A RID: 8746
			public int amount;
		}
	}
}
