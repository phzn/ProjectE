using System;
using System.Collections;
using System.Collections.Generic;
using Invector.vItemManager;
using UnityEngine;

namespace Invector.Throw
{
	// Token: 0x020004FE RID: 1278
	[vClassHeader("THROW MANAGER WITH INVENTORY", true, "icon_v2", false, "")]
	public class vThrowManagerInventory : vThrowManagerBase
	{
		// Token: 0x06001F04 RID: 7940 RVA: 0x000B96BB File Offset: 0x000B78BB
		protected override IEnumerator Start()
		{
			yield return base.Start();
			this.itemManager = base.GetComponentInParent<vItemManager>();
			this.itemManager.inventory.OnUpdateInventory += this.UpdateThrowableItems;
			this.targetEquipArea = this.itemManager.inventory.equipAreas[this.indexOfEquipmentArea];
			if (!this.useEquipArea)
			{
				if (this.itemManager.items.Count > 0)
				{
					List<vItem> list = this.itemManager.items.FindAll(delegate(vItem item)
					{
						vThrowableObject vThrowableObject;
						return item.originalObject != null && item.originalObject.TryGetComponent<vThrowableObject>(out vThrowableObject);
					});
					for (int i = 0; i < list.Count; i++)
					{
						list[i].canBeEquipped = false;
					}
				}
				this.itemManager.onAddItem.AddListener(delegate(vItem item)
				{
					vThrowableObject vThrowableObject2;
					if (item.originalObject != null && item.originalObject.TryGetComponent<vThrowableObject>(out vThrowableObject2))
					{
						item.canBeEquipped = false;
					}
				});
			}
			this.UpdateThrowableItems();
			this.UpdateCurrentThrowableItem();
			yield break;
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001F05 RID: 7941 RVA: 0x000B96CA File Offset: 0x000B78CA
		// (set) Token: 0x06001F06 RID: 7942 RVA: 0x000B96D2 File Offset: 0x000B78D2
		protected virtual vItemManager itemManager { get; set; }

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06001F07 RID: 7943 RVA: 0x000B96DB File Offset: 0x000B78DB
		// (set) Token: 0x06001F08 RID: 7944 RVA: 0x000B96E3 File Offset: 0x000B78E3
		protected virtual vThrowUI _ui { get; set; }

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001F09 RID: 7945 RVA: 0x000B96EC File Offset: 0x000B78EC
		public virtual vThrowUI ui
		{
			get
			{
				if (!this._ui)
				{
					this._ui = base.GetComponentInChildren<vThrowUI>();
					if (this._ui)
					{
						this._ui.UpdateCount(this, true);
					}
				}
				return this._ui;
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06001F0A RID: 7946 RVA: 0x000B9727 File Offset: 0x000B7927
		// (set) Token: 0x06001F0B RID: 7947 RVA: 0x000B972F File Offset: 0x000B792F
		protected virtual int lastItemID { get; set; }

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06001F0C RID: 7948 RVA: 0x000B9738 File Offset: 0x000B7938
		// (set) Token: 0x06001F0D RID: 7949 RVA: 0x000B9740 File Offset: 0x000B7940
		public virtual List<vItem> throwables { get; set; }

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001F0E RID: 7950 RVA: 0x000B9749 File Offset: 0x000B7949
		public virtual vItem CurrentThrowableItem
		{
			get
			{
				this.UpdateCurrentThrowableItem();
				return this._throwableItem;
			}
		}

		// Token: 0x06001F0F RID: 7951 RVA: 0x000B9758 File Offset: 0x000B7958
		public virtual Transform GetHandler(string name)
		{
			Transform transform = Array.Find<Transform>(this.customHandlers, (Transform c) => c.gameObject.name == name);
			if (transform == null)
			{
				transform = this.defaultHandler;
			}
			return transform;
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06001F10 RID: 7952 RVA: 0x000B979B File Offset: 0x000B799B
		// (set) Token: 0x06001F11 RID: 7953 RVA: 0x000B97A3 File Offset: 0x000B79A3
		public virtual vEquipArea targetEquipArea { get; set; }

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06001F12 RID: 7954 RVA: 0x000B97AC File Offset: 0x000B79AC
		// (set) Token: 0x06001F13 RID: 7955 RVA: 0x000B97B4 File Offset: 0x000B79B4
		public virtual int indexOfCurrentThrowable { get; set; }

		// Token: 0x06001F14 RID: 7956 RVA: 0x000B97C0 File Offset: 0x000B79C0
		public virtual void UpdateThrowableItems()
		{
			if (!this.useEquipArea)
			{
				this.throwables = this.itemManager.itemListData.items.FindAll(delegate(vItem i)
				{
					vThrowableObject vThrowableObject;
					return i.type == this.itemType && i.originalObject != null && i.originalObject.TryGetComponent<vThrowableObject>(out vThrowableObject);
				});
			}
			if (this.useEquipArea && this.targetEquipArea.currentEquippedItem != this._throwableItem && !this.isThrowing)
			{
				bool isAiming = this.isAiming;
				this.DisableAimMode();
				this.ExitThrowMode();
				this.UpdateCurrentThrowableItem();
				if (isAiming && this.CurrentThrowAmount > 0)
				{
					this.EnterThrowMode();
				}
			}
			this.UpdateUI();
		}

		// Token: 0x06001F15 RID: 7957 RVA: 0x000B9854 File Offset: 0x000B7A54
		public virtual void UpdateCurrentThrowableItem()
		{
			if (this.isThrowing)
			{
				return;
			}
			if (this.useEquipArea)
			{
				if (this.targetEquipArea.currentEquippedItem != this._throwableItem)
				{
					this._throwableItem = this.targetEquipArea.currentEquippedItem;
				}
			}
			else
			{
				try
				{
					this._throwableItem = this.throwables[this.indexOfCurrentThrowable];
				}
				catch
				{
				}
			}
			if (this._throwableItem != null)
			{
				this.lastItemID = this._throwableItem.id;
				if (this.defaultHandler == null)
				{
					Debug.LogWarning("THROWMANAGER NEEDS A HANDLER ASSIGNED IN THE INSPECTOR", this);
					return;
				}
				vThrowableObject vThrowableObject;
				if (this._throwableItem.originalObject && this._throwableItem.originalObject.TryGetComponent<vThrowableObject>(out vThrowableObject))
				{
					if (!this.throwableInHandler.ContainsKey(this.lastItemID))
					{
						this.throwableInHandler.Add(this.lastItemID, null);
					}
					if (this.throwableInHandler[this.lastItemID] == null)
					{
						this.throwableInHandler[this.lastItemID] = this.InstantiateNewThrowable(vThrowableObject, this.GetHandler(this._throwableItem.customHandler));
					}
				}
			}
			if (this._throwableItem && this.throwableInHandler.ContainsKey(this.lastItemID))
			{
				this._objectToThrow = this.throwableInHandler[this.lastItemID];
				return;
			}
			if (this.useEquipArea && !this.inEnterThrowMode && !this.isThrowing)
			{
				this._objectToThrow = null;
				this.DisableAimMode();
				this.ExitThrowMode();
			}
		}

		// Token: 0x06001F16 RID: 7958 RVA: 0x000B99F8 File Offset: 0x000B7BF8
		protected virtual vThrowableObject InstantiateNewThrowable(vThrowableObject _throwable, Transform _handler)
		{
			vThrowableObject vThrowableObject = Object.Instantiate<vThrowableObject>(_throwable, _handler);
			vThrowableObject.gameObject.SetActive(false);
			vThrowableObject.transform.localPosition = Vector3.zero;
			vThrowableObject.transform.localEulerAngles = Vector3.zero;
			return vThrowableObject;
		}

		// Token: 0x06001F17 RID: 7959 RVA: 0x000B9A30 File Offset: 0x000B7C30
		public virtual void NextThrowable()
		{
			int num = 0;
			if (this.indexOfCurrentThrowable + 1 < this.throwables.Count)
			{
				num = this.indexOfCurrentThrowable + 1;
			}
			if (this.itemManager.GetAllAmount(this.throwables[num].id) > 0 || !this.isAiming)
			{
				this.SelectThrowable(num);
			}
			else if (this.isAiming && this.throwables.Exists((vItem t) => this.itemManager.GetAllAmount(t.id) > 0))
			{
				for (int i = 0; i < this.throwables.Count; i++)
				{
					if (num + 1 < this.throwables.Count)
					{
						num++;
					}
					else
					{
						num = 0;
					}
					if (this.itemManager.GetAllAmount(this.throwables[num].id) > 0)
					{
						this.SelectThrowable(num);
						break;
					}
				}
			}
			this.UpdateUI();
		}

		// Token: 0x06001F18 RID: 7960 RVA: 0x000B9B0C File Offset: 0x000B7D0C
		public virtual void PreviousThrowable()
		{
			int num = this.throwables.Count - 1;
			if (this.indexOfCurrentThrowable - 1 >= 0)
			{
				num = this.indexOfCurrentThrowable - 1;
			}
			if (this.itemManager.GetAllAmount(this.throwables[num].id) > 0 || !this.isAiming)
			{
				this.SelectThrowable(num);
			}
			else if (this.isAiming && this.throwables.Exists((vItem t) => this.itemManager.GetAllAmount(t.id) > 0))
			{
				for (int i = 0; i < this.throwables.Count; i++)
				{
					if (num - 1 >= 0)
					{
						num--;
					}
					else
					{
						num = this.throwables.Count - 1;
					}
					if (this.itemManager.GetAllAmount(this.throwables[num].id) > 0)
					{
						this.SelectThrowable(num);
						break;
					}
				}
			}
			this.UpdateUI();
		}

		// Token: 0x06001F19 RID: 7961 RVA: 0x000B9BEC File Offset: 0x000B7DEC
		public virtual void SelectThrowable(int indexOfThrowable)
		{
			if (this.useEquipArea || this.inEnterThrowMode || this.isThrowing)
			{
				return;
			}
			bool isAiming = this.isAiming;
			this.DisableAimMode();
			this.ExitThrowMode();
			if (indexOfThrowable >= 0 && indexOfThrowable < this.throwables.Count)
			{
				this.indexOfCurrentThrowable = indexOfThrowable;
			}
			if (isAiming && this.CurrentThrowAmount > 0)
			{
				this.EnterThrowMode();
			}
			this.UpdateUI();
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x000B9C54 File Offset: 0x000B7E54
		public virtual void UpdateUI()
		{
			if (this.ui != null)
			{
				this.ui.UpdateCount(this, false);
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001F1B RID: 7963 RVA: 0x000B9C71 File Offset: 0x000B7E71
		public override int MaxThrowObjects
		{
			get
			{
				if (!(this.CurrentThrowableItem != null))
				{
					return 0;
				}
				return this.CurrentThrowableItem.maxStack;
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001F1C RID: 7964 RVA: 0x000B9C8E File Offset: 0x000B7E8E
		public override Sprite CurrentThrowableSprite
		{
			get
			{
				if (!(this.CurrentThrowableItem != null))
				{
					return null;
				}
				return this.CurrentThrowableItem.icon;
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001F1D RID: 7965 RVA: 0x000B9CAB File Offset: 0x000B7EAB
		public override int CurrentThrowAmount
		{
			get
			{
				if (!this.CurrentThrowableItem)
				{
					return 0;
				}
				if (!this.useEquipArea)
				{
					return this.itemManager.GetAllAmount(this.CurrentThrowableItem.id);
				}
				return this.CurrentThrowableItem.amount;
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001F1E RID: 7966 RVA: 0x000B9CE6 File Offset: 0x000B7EE6
		public override vThrowableObject ObjectToThrow
		{
			get
			{
				return this._objectToThrow;
			}
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x000B9CF0 File Offset: 0x000B7EF0
		protected override void StartThrow()
		{
			this.itemManager.LockInventoryInput(true);
			base.StartThrow();
			if (this.itemManager)
			{
				if (this.CurrentThrowableItem && this.throwableInHandler.ContainsKey(this.lastItemID) && this.throwableInHandler[this.lastItemID])
				{
					this.throwableInHandler[this.lastItemID] = null;
				}
				this.itemManager.DestroyItem(this.itemManager.GetItem(this.lastItemID), 1);
			}
			if (!this.useEquipArea)
			{
				this.UpdateUI();
			}
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x000B9D91 File Offset: 0x000B7F91
		protected override void Throw()
		{
			base.Throw();
			this.itemManager.LockInventoryInput(false);
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x000B9DA8 File Offset: 0x000B7FA8
		protected override void DisableAimMode()
		{
			base.DisableAimMode();
			if (!this.isThrowing && this.throwableInHandler.ContainsKey(this.lastItemID) && this.throwableInHandler[this.lastItemID])
			{
				this.throwableInHandler[this.lastItemID].gameObject.SetActive(false);
			}
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x000B9E0C File Offset: 0x000B800C
		protected override void EquipThrowObject()
		{
			if (this.throwableInHandler.ContainsKey(this.lastItemID) && this.throwableInHandler[this.lastItemID])
			{
				this.throwableInHandler[this.lastItemID].gameObject.SetActive(true);
			}
			base.EquipThrowObject();
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x000B7E0A File Offset: 0x000B600A
		protected override vThrowableObject GetInstanceOfThrowable()
		{
			return this.ObjectToThrow;
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x000B9E66 File Offset: 0x000B8066
		public override bool CanCollectThrowable(string throwableName, out int remainingAmount)
		{
			remainingAmount = 99999;
			return true;
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x00004F31 File Offset: 0x00003131
		public override void OnCollectThrowable(string throwableName, int amount = 1)
		{
		}

		// Token: 0x04001FAE RID: 8110
		[vEditorToolbar("Throwable", false, "", false, false)]
		public bool useEquipArea;

		// Token: 0x04001FAF RID: 8111
		[vHideInInspector("useEquipArea", false)]
		public int indexOfEquipmentArea;

		// Token: 0x04001FB0 RID: 8112
		[vHideInInspector("useEquipArea", true)]
		public vItemType itemType;

		// Token: 0x04001FB1 RID: 8113
		public Transform defaultHandler;

		// Token: 0x04001FB2 RID: 8114
		public Transform[] customHandlers;

		// Token: 0x04001FB6 RID: 8118
		public Dictionary<int, vThrowableObject> throwableInHandler = new Dictionary<int, vThrowableObject>();

		// Token: 0x04001FB8 RID: 8120
		[vEditorToolbar("Debug", false, "", false, false)]
		[SerializeField]
		protected vItem _throwableItem;

		// Token: 0x04001FBA RID: 8122
		[vEditorToolbar("Debug", false, "", false, false)]
		[SerializeField]
		protected vThrowableObject _objectToThrow;
	}
}
