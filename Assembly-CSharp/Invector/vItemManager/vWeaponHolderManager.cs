using System;
using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vItemManager
{
	// Token: 0x020005DC RID: 1500
	[vClassHeader("Weapon Holder Manager", "Create a new empty object inside a bone and add the vWeaponHolder script")]
	public class vWeaponHolderManager : vMonoBehaviour
	{
		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x060022E5 RID: 8933 RVA: 0x000C7D80 File Offset: 0x000C5F80
		// (set) Token: 0x060022E6 RID: 8934 RVA: 0x000C7D88 File Offset: 0x000C5F88
		public virtual bool inEquip { get; set; }

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x060022E7 RID: 8935 RVA: 0x000C7D91 File Offset: 0x000C5F91
		// (set) Token: 0x060022E8 RID: 8936 RVA: 0x000C7D99 File Offset: 0x000C5F99
		public virtual bool inUnequip { get; set; }

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x060022E9 RID: 8937 RVA: 0x000C7DA2 File Offset: 0x000C5FA2
		// (set) Token: 0x060022EA RID: 8938 RVA: 0x000C7DAA File Offset: 0x000C5FAA
		public virtual vItemManager itemManager { get; set; }

		// Token: 0x060022EB RID: 8939 RVA: 0x000C7DB3 File Offset: 0x000C5FB3
		protected virtual void OnDrawGizmosSelected()
		{
			this.holders = base.GetComponentsInChildren<vWeaponHolder>(true);
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x060022EC RID: 8940 RVA: 0x000C7DC2 File Offset: 0x000C5FC2
		protected virtual bool IsEquipping
		{
			get
			{
				return this.cc && this.cc.IsAnimatorTag("IsEquipping");
			}
		}

		// Token: 0x060022ED RID: 8941 RVA: 0x000C7DE4 File Offset: 0x000C5FE4
		protected virtual void Start()
		{
			this.itemManager = base.GetComponent<vItemManager>();
			this.cc = base.GetComponent<vThirdPersonController>();
			if (this.itemManager)
			{
				this.itemManager.onFinishEquipItem.AddListener(new UnityAction<vEquipArea, vItem>(this.EquipWeapon));
				this.itemManager.onFinishUnequipItem.AddListener(new UnityAction<vEquipArea, vItem>(this.UnequipWeapon));
				this.itemManager.onSetLockedToEquip.AddListener(new UnityAction<vEquipArea>(this.CheckAreaLockedToEquip));
				this.holders = base.GetComponentsInChildren<vWeaponHolder>(true);
				if (this.holders != null)
				{
					foreach (vWeaponHolder vWeaponHolder in this.holders)
					{
						if (!this.holderAreas.ContainsKey(vWeaponHolder.equipPointName))
						{
							this.holderAreas.Add(vWeaponHolder.equipPointName, new List<vWeaponHolder>());
							this.holderAreas[vWeaponHolder.equipPointName].Add(vWeaponHolder);
						}
						else
						{
							this.holderAreas[vWeaponHolder.equipPointName].Add(vWeaponHolder);
						}
						vWeaponHolder.SetActiveHolder(false);
						vWeaponHolder.SetActiveWeapon(false);
					}
				}
			}
		}

		// Token: 0x060022EE RID: 8942 RVA: 0x000C7F08 File Offset: 0x000C6108
		public virtual void CheckAreaLockedToEquip(vEquipArea equipArea)
		{
			if (equipArea.isLockedToEquip)
			{
				this.UnequipWeapon(equipArea, equipArea.currentEquippedItem);
			}
		}

		// Token: 0x060022EF RID: 8943 RVA: 0x000C7F20 File Offset: 0x000C6120
		public virtual void EquipWeapon(vEquipArea equipArea, vItem item)
		{
			if (item == null)
			{
				return;
			}
			List<vEquipSlot> slotsInArea = equipArea.equipSlots;
			if (slotsInArea != null && slotsInArea.Count > 0 && this.holderAreas.ContainsKey(equipArea.equipPointName))
			{
				int j;
				int i;
				for (i = 0; i < slotsInArea.Count; i = j + 1)
				{
					if (slotsInArea[i].item != null)
					{
						vWeaponHolder vWeaponHolder = this.holderAreas[equipArea.equipPointName].Find((vWeaponHolder h) => slotsInArea[i].item && slotsInArea[i].item.id == h.itemID && ((equipArea.currentEquippedItem != null && equipArea.currentEquippedItem != item && equipArea.currentEquippedItem != slotsInArea[i].item && equipArea.currentEquippedItem.id != item.id) || equipArea.currentEquippedItem == null || equipArea.isLockedToEquip));
						if (vWeaponHolder)
						{
							vWeaponHolder.SetActiveHolder(true);
							vWeaponHolder.SetActiveWeapon(true);
							if (this.debugMode)
							{
								string text = "Hold: ";
								vItem item2 = slotsInArea[i].item;
								Debug.Log(text + ((item2 != null) ? item2.ToString() : null));
							}
						}
					}
					j = i;
				}
				if (!equipArea.isLockedToEquip && equipArea.currentEquippedItem != null && equipArea.currentEquippedItem == item)
				{
					vWeaponHolder vWeaponHolder2 = this.holderAreas[equipArea.equipPointName].Find((vWeaponHolder h) => h.itemID == equipArea.currentEquippedItem.id);
					if (vWeaponHolder2)
					{
						if (equipArea.currentEquippedItem != item || this.itemManager.playItemAnimation)
						{
							bool flag = !string.IsNullOrEmpty(equipArea.currentEquippedItem.EnableAnim);
						}
						if (this.debugMode)
						{
							Debug.Log("UnHold: " + item.name);
						}
						vWeaponHolder2.SetActiveHolder(true);
						vWeaponHolder2.SetActiveWeapon(false);
					}
				}
			}
		}

		// Token: 0x060022F0 RID: 8944 RVA: 0x000C816C File Offset: 0x000C636C
		public virtual void UnequipWeapon(vEquipArea equipArea, vItem item)
		{
			if (this.holders.Length == 0 || item == null)
			{
				return;
			}
			if (this.itemManager.inventory != null && this.holderAreas.ContainsKey(equipArea.equipPointName))
			{
				vWeaponHolder vWeaponHolder = this.holderAreas[equipArea.equipPointName].Find((vWeaponHolder h) => item.id == h.itemID);
				if (vWeaponHolder)
				{
					bool flag = equipArea.equipSlots.Find((vEquipSlot slot) => slot.item == item) != null;
					if (this.debugMode)
					{
						Debug.Log(flag ? ("Hold: " + item.name) : ("Hide :" + item.name + " Holder"));
					}
					if (flag)
					{
						vWeaponHolder.SetActiveHolder(flag);
						vWeaponHolder.SetActiveWeapon(flag);
						return;
					}
					vWeaponHolder.SetActiveHolder(false);
					vWeaponHolder.SetActiveWeapon(false);
				}
			}
		}

		// Token: 0x060022F1 RID: 8945 RVA: 0x000C8274 File Offset: 0x000C6474
		public virtual vWeaponHolder GetHolder(GameObject equipment, int id)
		{
			EquipPoint equipPoint = this.itemManager.equipPoints.Find((EquipPoint e) => e.equipmentReference != null && e.equipmentReference.item && e.equipmentReference.item.id == id && e.equipmentReference.equipedObject == equipment);
			if (equipPoint != null && this.holderAreas.ContainsKey(equipPoint.equipPointName))
			{
				return this.holderAreas[equipPoint.equipPointName].Find((vWeaponHolder h) => id == h.itemID);
			}
			if (this.debugMode)
			{
				Debug.LogWarning(this.ToString() + " fail to find a holder with equipPointName " + equipPoint.equipPointName);
			}
			return null;
		}

		// Token: 0x060022F2 RID: 8946 RVA: 0x000C830F File Offset: 0x000C650F
		public virtual IEnumerator UnequipRoutine(float equipDelay, bool immediate = false, UnityAction onStart = null, UnityAction onFinish = null, string itemName = "")
		{
			if (this.debugMode)
			{
				Debug.Log("Start Unequip: " + itemName);
			}
			if (!immediate && !this.inEquip)
			{
				this.inUnequip = true;
			}
			while (!this.IsEquipping && !immediate && !this.inEquip)
			{
				yield return null;
			}
			if (onStart != null)
			{
				onStart();
			}
			if (!this.inEquip && !immediate)
			{
				float equipTime = equipDelay;
				while (!immediate && !this.inEquip && equipTime > 0f)
				{
					equipTime -= vTime.deltaTime;
					yield return null;
				}
			}
			this.inUnequip = false;
			if (onFinish != null)
			{
				onFinish();
			}
			if (this.debugMode)
			{
				Debug.Log("Finish Unequip: " + itemName);
			}
			yield break;
		}

		// Token: 0x060022F3 RID: 8947 RVA: 0x000C8343 File Offset: 0x000C6543
		public virtual IEnumerator EquipRoutine(float equipDelay, bool immediate = false, UnityAction onStart = null, UnityAction onFinish = null, string itemName = "")
		{
			if (this.debugMode)
			{
				Debug.Log("Start Equip: " + itemName);
			}
			if (!immediate)
			{
				this.inEquip = true;
			}
			while (!this.IsEquipping && !immediate)
			{
				yield return null;
			}
			if (onStart != null)
			{
				onStart();
			}
			if (!this.inUnequip && !immediate)
			{
				float equipTime = equipDelay;
				while (!immediate && !this.inUnequip && equipTime > 0f)
				{
					equipTime -= vTime.deltaTime;
					yield return null;
				}
			}
			this.inEquip = false;
			if (onFinish != null)
			{
				onFinish();
			}
			if (this.debugMode)
			{
				Debug.Log("Finish Equip: " + itemName);
			}
			yield break;
		}

		// Token: 0x04002320 RID: 8992
		public vWeaponHolder[] holders = new vWeaponHolder[0];

		// Token: 0x04002321 RID: 8993
		public bool debugMode;

		// Token: 0x04002322 RID: 8994
		public Dictionary<string, List<vWeaponHolder>> holderAreas = new Dictionary<string, List<vWeaponHolder>>();

		// Token: 0x04002323 RID: 8995
		protected float equipTime;

		// Token: 0x04002327 RID: 8999
		protected vThirdPersonController cc;
	}
}
