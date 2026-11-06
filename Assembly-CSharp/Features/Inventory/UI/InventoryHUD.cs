using System;
using System.Collections.Generic;
using Features.PhoneCompanion;
using Features.RapidActions;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.Inventory.UI
{
	// Token: 0x0200114F RID: 4431
	public class InventoryHUD : MonoBehaviour
	{
		// Token: 0x060086AF RID: 34479 RVA: 0x0037092E File Offset: 0x0036EB2E
		private void OnEnable()
		{
			this.TryBind();
		}

		// Token: 0x060086B0 RID: 34480 RVA: 0x0037092E File Offset: 0x0036EB2E
		private void Start()
		{
			this.TryBind();
		}

		// Token: 0x060086B1 RID: 34481 RVA: 0x00370938 File Offset: 0x0036EB38
		private void Update()
		{
			if (this._bound && (this.inventoryManager == null || (PlayerInventoryManager.Instance != null && this.inventoryManager != PlayerInventoryManager.Instance)))
			{
				this.Unbind();
			}
			if (!this._bound)
			{
				this.TryBind();
			}
			bool isActive = ControllerInput.IsActive;
			for (int i = 0; i < this.slots.Count; i++)
			{
				if (this.slots[i] != null)
				{
					this.slots[i].RefreshInputDevice(isActive, i == 0);
				}
			}
			this.RefreshSelectionHint(isActive);
		}

		// Token: 0x060086B2 RID: 34482 RVA: 0x003709DC File Offset: 0x0036EBDC
		private void RefreshSelectionHint(bool controller)
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			bool flag = controller && this.inventoryManager != null && this.inventoryManager.CanClearControllerSelection && InventoryControllerInput.CanRead && !InventoryControllerInput.PlacementHeld && !ControllerInput.Modifier && !RadialControllerNavigation.AnyWheelOpen && (!SmartphoneController.IsAnyPhoneOut || SmartphoneController.IsStableTripodCameraOwner);
			if (this._hintHud != null && (!flag || this._hintHud != instance))
			{
				this._hintHud.HideSupplemental(this);
				this._hintHud = null;
			}
			if (!flag || instance == null)
			{
				return;
			}
			if (this._selectionHint == null)
			{
				this._selectionHint = ScriptableObject.CreateInstance<InteractionConfigSO>();
				this._selectionHint.hideFlags = HideFlags.HideAndDontSave;
				this._selectionHint.usableActions = new List<UsableAction>
				{
					new UsableAction
					{
						keyDisplayLabel = "B"
					}
				};
				ControllerContextBindings.MarkControllerLabels(this._selectionHint, true);
			}
			this._selectionHint.usableActions[0].actionName = ((this.inventoryManager.ActiveClothIndex >= 0) ? "Deselect clothing" : "Put away item");
			this._hintHud = instance;
			instance.ShowSupplemental(this, this._selectionHint);
		}

		// Token: 0x060086B3 RID: 34483 RVA: 0x00370B18 File Offset: 0x0036ED18
		private void TryBind()
		{
			if (this._bound)
			{
				return;
			}
			if (PlayerInventoryManager.Instance != null)
			{
				this.inventoryManager = PlayerInventoryManager.Instance;
			}
			if (this.inventoryManager == null)
			{
				return;
			}
			for (int i = 0; i < this.slots.Count; i++)
			{
				if (this.slots[i] != null)
				{
					this.slots[i].Init(i);
				}
			}
			this.inventoryManager.OnItemAdded += this.HandleItemAdded;
			this.inventoryManager.OnItemVisualChanged += this.HandleItemAdded;
			this.inventoryManager.OnItemRemoved += this.HandleItemRemoved;
			this.inventoryManager.OnItemStackChanged += this.HandleItemStackChanged;
			this.inventoryManager.OnActiveSlotChanged += this.HandleActiveSlotChanged;
			this.inventoryManager.OnDurabilityChanged += this.HandleDurabilityChanged;
			this._bound = true;
			for (int j = 0; j < this.slots.Count; j++)
			{
				this.HandleItemAdded(j, this.inventoryManager.GetItemAt(j));
			}
			this.HandleActiveSlotChanged(this.inventoryManager.ActiveSlotIndex);
		}

		// Token: 0x060086B4 RID: 34484 RVA: 0x00370C5C File Offset: 0x0036EE5C
		private void HandleItemAdded(int index, InventoryItemSO item)
		{
			if (index >= 0 && index < this.slots.Count)
			{
				this.slots[index].SetItem(item);
				this.slots[index].UpdateDurability(this.inventoryManager.GetItemDurability01(index));
			}
		}

		// Token: 0x060086B5 RID: 34485 RVA: 0x00370CAA File Offset: 0x0036EEAA
		private void HandleItemRemoved(int index)
		{
			if (index >= 0 && index < this.slots.Count)
			{
				this.slots[index].ClearSlot();
			}
		}

		// Token: 0x060086B6 RID: 34486 RVA: 0x00370CD0 File Offset: 0x0036EED0
		private void HandleActiveSlotChanged(int index)
		{
			for (int i = 0; i < this.slots.Count; i++)
			{
				this.slots[i].SetSelected(i == index);
			}
		}

		// Token: 0x060086B7 RID: 34487 RVA: 0x00370D08 File Offset: 0x0036EF08
		private void HandleItemStackChanged(int index, int previous, int current)
		{
			if (index >= 0 && index < this.slots.Count)
			{
				this.slots[index].SetStackCount(current);
			}
		}

		// Token: 0x060086B8 RID: 34488 RVA: 0x00370D2E File Offset: 0x0036EF2E
		private void HandleDurabilityChanged(int index, float normalizedValue)
		{
			if (index >= 0 && index < this.slots.Count)
			{
				this.slots[index].UpdateDurability(normalizedValue);
			}
		}

		// Token: 0x060086B9 RID: 34489 RVA: 0x00370D54 File Offset: 0x0036EF54
		private void OnDisable()
		{
			this.Unbind();
			if (this._hintHud != null)
			{
				this._hintHud.HideSupplemental(this);
			}
			this._hintHud = null;
		}

		// Token: 0x060086BA RID: 34490 RVA: 0x00370D7D File Offset: 0x0036EF7D
		private void OnDestroy()
		{
			this.OnDisable();
			if (this._selectionHint != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this._selectionHint);
					return;
				}
				Object.DestroyImmediate(this._selectionHint);
			}
		}

		// Token: 0x060086BB RID: 34491 RVA: 0x00370DB4 File Offset: 0x0036EFB4
		private void Unbind()
		{
			if (this._bound && this.inventoryManager != null)
			{
				this.inventoryManager.OnItemAdded -= this.HandleItemAdded;
				this.inventoryManager.OnItemVisualChanged -= this.HandleItemAdded;
				this.inventoryManager.OnItemRemoved -= this.HandleItemRemoved;
				this.inventoryManager.OnItemStackChanged -= this.HandleItemStackChanged;
				this.inventoryManager.OnActiveSlotChanged -= this.HandleActiveSlotChanged;
				this.inventoryManager.OnDurabilityChanged -= this.HandleDurabilityChanged;
			}
			this._bound = false;
		}

		// Token: 0x04008969 RID: 35177
		[Header("References")]
		[SerializeField]
		private PlayerInventoryManager inventoryManager;

		// Token: 0x0400896A RID: 35178
		[SerializeField]
		private List<InventorySlotUI> slots = new List<InventorySlotUI>();

		// Token: 0x0400896B RID: 35179
		private bool _bound;

		// Token: 0x0400896C RID: 35180
		private InteractionConfigSO _selectionHint;

		// Token: 0x0400896D RID: 35181
		private InteractionHUDController _hintHud;
	}
}
