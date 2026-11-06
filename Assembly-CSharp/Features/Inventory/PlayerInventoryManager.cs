using System;
using System.Collections;
using System.Collections.Generic;
using Features.CielOS;
using Features.DebugTools;
using Features.Detection;
using Features.HideFlash;
using Features.Inventory.Placement;
using Features.Inventory.Wearable;
using Features.MainMenu;
using Features.PhoneCompanion;
using Features.Rank;
using Features.RapidActions;
using Features.RunModes;
using Features.Tutorials;
using Features.Vehicles.Traffic;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using PoolWaterTools;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.CoverWall;
using ProjectExhibitionist.FirstPerson;
using ProjectExhibitionist.Interactions;
using ProjectExhibitionist.UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Features.Inventory
{
	// Token: 0x02001100 RID: 4352
	public class PlayerInventoryManager : MonoBehaviour
	{
		// Token: 0x170014DF RID: 5343
		// (get) Token: 0x060083A9 RID: 33705 RVA: 0x0035BB6E File Offset: 0x00359D6E
		// (set) Token: 0x060083AA RID: 33706 RVA: 0x0035BB75 File Offset: 0x00359D75
		public static PlayerInventoryManager Instance { get; private set; }

		// Token: 0x170014E0 RID: 5344
		// (get) Token: 0x060083AB RID: 33707 RVA: 0x0035BB80 File Offset: 0x00359D80
		public static bool InfiniteItemDurabilityInActiveScene
		{
			get
			{
				return string.Equals(SceneManager.GetActiveScene().name, "CielHome", StringComparison.Ordinal) || (RunModeService.IsGameplayBoot && RunModeService.UnlimitedItemDurability);
			}
		}

		// Token: 0x170014E1 RID: 5345
		// (get) Token: 0x060083AC RID: 33708 RVA: 0x0035BBB8 File Offset: 0x00359DB8
		public static bool InfiniteClothingDurabilityInActiveScene
		{
			get
			{
				return string.Equals(SceneManager.GetActiveScene().name, "CielHome", StringComparison.Ordinal) || GuidedRankGameplayPolicy.AdvancedMechanicsBlocked || (RunModeService.IsGameplayBoot && RunModeService.UnlimitedClothingDurability) || RankProgressionService.IsAdminRankActive;
			}
		}

		// Token: 0x170014E2 RID: 5346
		// (get) Token: 0x060083AD RID: 33709 RVA: 0x0035BBFB File Offset: 0x00359DFB
		public static bool InfiniteDurabilityInActiveScene
		{
			get
			{
				return PlayerInventoryManager.InfiniteItemDurabilityInActiveScene && PlayerInventoryManager.InfiniteClothingDurabilityInActiveScene;
			}
		}

		// Token: 0x140000E9 RID: 233
		// (add) Token: 0x060083AE RID: 33710 RVA: 0x0035BC0C File Offset: 0x00359E0C
		// (remove) Token: 0x060083AF RID: 33711 RVA: 0x0035BC44 File Offset: 0x00359E44
		public event Action<int, InventoryItemSO> OnItemAdded;

		// Token: 0x140000EA RID: 234
		// (add) Token: 0x060083B0 RID: 33712 RVA: 0x0035BC7C File Offset: 0x00359E7C
		// (remove) Token: 0x060083B1 RID: 33713 RVA: 0x0035BCB4 File Offset: 0x00359EB4
		public event Action<int, InventoryItemSO> OnItemRemoving;

		// Token: 0x140000EB RID: 235
		// (add) Token: 0x060083B2 RID: 33714 RVA: 0x0035BCEC File Offset: 0x00359EEC
		// (remove) Token: 0x060083B3 RID: 33715 RVA: 0x0035BD24 File Offset: 0x00359F24
		public event Action<int> OnItemRemoved;

		// Token: 0x140000EC RID: 236
		// (add) Token: 0x060083B4 RID: 33716 RVA: 0x0035BD5C File Offset: 0x00359F5C
		// (remove) Token: 0x060083B5 RID: 33717 RVA: 0x0035BD94 File Offset: 0x00359F94
		public event Action<int, int, int> OnItemStackChanged;

		// Token: 0x140000ED RID: 237
		// (add) Token: 0x060083B6 RID: 33718 RVA: 0x0035BDCC File Offset: 0x00359FCC
		// (remove) Token: 0x060083B7 RID: 33719 RVA: 0x0035BE04 File Offset: 0x0035A004
		public event Action<int> OnActiveSlotChanged;

		// Token: 0x140000EE RID: 238
		// (add) Token: 0x060083B8 RID: 33720 RVA: 0x0035BE3C File Offset: 0x0035A03C
		// (remove) Token: 0x060083B9 RID: 33721 RVA: 0x0035BE74 File Offset: 0x0035A074
		public event Action<int, float> OnDurabilityChanged;

		// Token: 0x140000EF RID: 239
		// (add) Token: 0x060083BA RID: 33722 RVA: 0x0035BEAC File Offset: 0x0035A0AC
		// (remove) Token: 0x060083BB RID: 33723 RVA: 0x0035BEE4 File Offset: 0x0035A0E4
		public event Action<int, InventoryItemSO> OnItemVisualChanged;

		// Token: 0x140000F0 RID: 240
		// (add) Token: 0x060083BC RID: 33724 RVA: 0x0035BF1C File Offset: 0x0035A11C
		// (remove) Token: 0x060083BD RID: 33725 RVA: 0x0035BF54 File Offset: 0x0035A154
		public event Action<int, InventoryItemSO> OnClothAdded;

		// Token: 0x140000F1 RID: 241
		// (add) Token: 0x060083BE RID: 33726 RVA: 0x0035BF8C File Offset: 0x0035A18C
		// (remove) Token: 0x060083BF RID: 33727 RVA: 0x0035BFC4 File Offset: 0x0035A1C4
		public event Action<int, InventoryItemSO> OnClothRemoving;

		// Token: 0x140000F2 RID: 242
		// (add) Token: 0x060083C0 RID: 33728 RVA: 0x0035BFFC File Offset: 0x0035A1FC
		// (remove) Token: 0x060083C1 RID: 33729 RVA: 0x0035C034 File Offset: 0x0035A234
		public event Action<int> OnClothRemoved;

		// Token: 0x140000F3 RID: 243
		// (add) Token: 0x060083C2 RID: 33730 RVA: 0x0035C06C File Offset: 0x0035A26C
		// (remove) Token: 0x060083C3 RID: 33731 RVA: 0x0035C0A4 File Offset: 0x0035A2A4
		public event Action<int> OnActiveClothChanged;

		// Token: 0x140000F4 RID: 244
		// (add) Token: 0x060083C4 RID: 33732 RVA: 0x0035C0DC File Offset: 0x0035A2DC
		// (remove) Token: 0x060083C5 RID: 33733 RVA: 0x0035C114 File Offset: 0x0035A314
		public event Action<int, float> OnClothDurabilityChanged;

		// Token: 0x140000F5 RID: 245
		// (add) Token: 0x060083C6 RID: 33734 RVA: 0x0035C14C File Offset: 0x0035A34C
		// (remove) Token: 0x060083C7 RID: 33735 RVA: 0x0035C184 File Offset: 0x0035A384
		public event Action<bool, int, int> OnSlotsMoved;

		// Token: 0x170014E3 RID: 5347
		// (get) Token: 0x060083C8 RID: 33736 RVA: 0x0035C1B9 File Offset: 0x0035A3B9
		public string PickupKey
		{
			get
			{
				return this.pickupInput.keyboard;
			}
		}

		// Token: 0x060083C9 RID: 33737 RVA: 0x0035C1C6 File Offset: 0x0035A3C6
		public GameObject GetSpawnedHandObject()
		{
			return this._spawnedHandObject;
		}

		// Token: 0x170014E4 RID: 5348
		// (get) Token: 0x060083CA RID: 33738 RVA: 0x0035C1CE File Offset: 0x0035A3CE
		public int MaxItemSlots
		{
			get
			{
				return this.maxSlots;
			}
		}

		// Token: 0x170014E5 RID: 5349
		// (get) Token: 0x060083CB RID: 33739 RVA: 0x0035C1D6 File Offset: 0x0035A3D6
		public int ActiveSlotIndex
		{
			get
			{
				return this.activeSlotIndex;
			}
		}

		// Token: 0x060083CC RID: 33740 RVA: 0x0035C1DE File Offset: 0x0035A3DE
		public InventoryItemSO GetItemAt(int index)
		{
			if (index < 0 || index >= this.slots.Count)
			{
				return null;
			}
			return this.slots[index];
		}

		// Token: 0x060083CD RID: 33741 RVA: 0x0035C200 File Offset: 0x0035A400
		public InventoryItemSO GetActiveSlotItem()
		{
			if (this.activeSlotIndex < 0 || this.activeSlotIndex >= this.slots.Count)
			{
				return null;
			}
			return this.slots[this.activeSlotIndex];
		}

		// Token: 0x170014E6 RID: 5350
		// (get) Token: 0x060083CE RID: 33742 RVA: 0x0035C231 File Offset: 0x0035A431
		public int MaxClothSlots
		{
			get
			{
				return this.maxClothSlots;
			}
		}

		// Token: 0x170014E7 RID: 5351
		// (get) Token: 0x060083CF RID: 33743 RVA: 0x0035C23C File Offset: 0x0035A43C
		public bool HasFreeClothingSlot
		{
			get
			{
				for (int i = 0; i < this.clothSlots.Count; i++)
				{
					if (this.clothSlots[i] == null)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170014E8 RID: 5352
		// (get) Token: 0x060083D0 RID: 33744 RVA: 0x0035C276 File Offset: 0x0035A476
		public int ActiveClothIndex
		{
			get
			{
				return this.activeClothIndex;
			}
		}

		// Token: 0x170014E9 RID: 5353
		// (get) Token: 0x060083D1 RID: 33745 RVA: 0x0035C27E File Offset: 0x0035A47E
		public int LastFocusedClothIndex
		{
			get
			{
				return this._lastFocusedClothIndex;
			}
		}

		// Token: 0x060083D2 RID: 33746 RVA: 0x0035C286 File Offset: 0x0035A486
		public InventoryItemSO GetActiveCloth()
		{
			return this._activeCloth;
		}

		// Token: 0x060083D3 RID: 33747 RVA: 0x0035C28E File Offset: 0x0035A48E
		public InventoryItemSO GetClothAt(int index)
		{
			if (index < 0 || index >= this.clothSlots.Count)
			{
				return null;
			}
			return this.clothSlots[index];
		}

		// Token: 0x060083D4 RID: 33748 RVA: 0x0035C2B0 File Offset: 0x0035A4B0
		public bool HasAnyCloth()
		{
			for (int i = 0; i < this.clothSlots.Count; i++)
			{
				if (this.clothSlots[i] != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x170014EA RID: 5354
		// (get) Token: 0x060083D5 RID: 33749 RVA: 0x0035C2EA File Offset: 0x0035A4EA
		public Animator PlayerAnimator
		{
			get
			{
				return this.playerAnimator;
			}
		}

		// Token: 0x170014EB RID: 5355
		// (get) Token: 0x060083D6 RID: 33750 RVA: 0x0035C2F2 File Offset: 0x0035A4F2
		public bool IsSlotInputLocked
		{
			get
			{
				return this._legacySlotInputLocked || this._slotInputLockOwners.Count > 0;
			}
		}

		// Token: 0x060083D7 RID: 33751 RVA: 0x0035C30C File Offset: 0x0035A50C
		public void LockSlotInput()
		{
			this._legacySlotInputLocked = true;
		}

		// Token: 0x060083D8 RID: 33752 RVA: 0x0035C315 File Offset: 0x0035A515
		public void UnlockSlotInput()
		{
			this._legacySlotInputLocked = false;
		}

		// Token: 0x060083D9 RID: 33753 RVA: 0x0035C31E File Offset: 0x0035A51E
		public bool AcquireSlotInputLock(Object owner)
		{
			if (owner == null)
			{
				return false;
			}
			this._slotInputLockOwners.Add(owner.GetInstanceID());
			return true;
		}

		// Token: 0x060083DA RID: 33754 RVA: 0x0035C33E File Offset: 0x0035A53E
		public void ReleaseSlotInputLock(Object owner)
		{
			if (owner != null)
			{
				this._slotInputLockOwners.Remove(owner.GetInstanceID());
			}
		}

		// Token: 0x060083DB RID: 33755 RVA: 0x0035C35C File Offset: 0x0035A55C
		private void Awake()
		{
			if (PlayerInventoryManager.Instance == null)
			{
				PlayerInventoryManager.Instance = this;
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
			for (int i = 0; i < this.maxSlots; i++)
			{
				this.slots.Add(null);
				this.durability.Add(0f);
				this.stackCounts.Add(0);
				this.itemRarities.Add(ItemRarity.Common);
				this.itemVariants.Add(-1);
			}
			for (int j = 0; j < this.maxClothSlots; j++)
			{
				this.clothSlots.Add(null);
				this.clothDurability.Add(0f);
				this.clothRarities.Add(ItemRarity.Common);
				this.clothVariants.Add(-1);
			}
		}

		// Token: 0x060083DC RID: 33756 RVA: 0x0035C420 File Offset: 0x0035A620
		private void Start()
		{
			if (this.playerAnimator == null)
			{
				this.playerAnimator = base.GetComponentInChildren<Animator>();
			}
			if (this.actionAnimator == null)
			{
				this.actionAnimator = base.GetComponent<PlayerActionAnimator>();
			}
			if (this.coverController == null)
			{
				this.coverController = base.GetComponent<CoverPoseController>();
			}
			if (this.playerAnimator != null)
			{
				this._holdLayerIndex = this.playerAnimator.GetLayerIndex(this.holdLayerName);
				this._armsLayerIndex = this.playerAnimator.GetLayerIndex(this.armsLayerName);
			}
		}

		// Token: 0x060083DD RID: 33757 RVA: 0x0035C4B7 File Offset: 0x0035A6B7
		private void Update()
		{
			this.HandleInputs();
			this.UpdateDurability();
			if (this._activeItem != null && this._armsLayerIndex != -1)
			{
				this.playerAnimator.SetLayerWeight(this._armsLayerIndex, 0f);
			}
		}

		// Token: 0x060083DE RID: 33758 RVA: 0x0035C4F4 File Offset: 0x0035A6F4
		private void HandleInputs()
		{
			if (FlyCamera.IsActive)
			{
				return;
			}
			if (CielOSController.AnyOpen)
			{
				return;
			}
			if (PlayerActivityGate.IsBlocked(PlayerActivity.Inventory))
			{
				return;
			}
			if (ControllerInput.IsActive && !ControllerInput.UiInputConsumedThisFrame && InventoryControllerInput.CanRead && !RadialControllerNavigation.AnyWheelOpen && (!SmartphoneController.IsAnySelfieActive || SmartphoneController.IsStableTripodCameraOwner))
			{
				if (this.TryCancelControllerSelection())
				{
					return;
				}
				if (ControllerInput.PreviousItemDown)
				{
					this.CycleControllerHotbar(-1);
				}
				else if (ControllerInput.NextItemDown)
				{
					this.CycleControllerHotbar(1);
				}
			}
			if (this.clothSelectInput.GetButtonDown() && this.CanUseInventoryInput(this.clothSelectInput))
			{
				this.ToggleClothFocus();
			}
			if (this.activeClothIndex != -1 && this.clothCycleInput.GetButtonDown())
			{
				this.CycleClothFromInput();
			}
			if (this.slot1Input.GetButtonDown())
			{
				this.SelectSlotFromInput(0);
			}
			if (this.slot2Input.GetButtonDown())
			{
				this.SelectSlotFromInput(1);
			}
			if (this.slot3Input.GetButtonDown())
			{
				this.SelectSlotFromInput(2);
			}
		}

		// Token: 0x060083DF RID: 33759 RVA: 0x0035C5E8 File Offset: 0x0035A7E8
		private bool CanUseInventoryInput(GenericInput input)
		{
			return !FlyCamera.IsActive && !SelfieRadialMenuController.AnyWheelOpen && !RadialControllerNavigation.AnyWheelOpen && !CielOSController.AnyOpen && !TextInputFocus.BlocksGameplayInput && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI) && PlayerActivityGate.Allows(PlayerActivity.Inventory) && (WallCoverActivityLock.ActiveController == null || (WallCoverActivityLock.IsPlayerInCover && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI))) && !InteractionCandidateRegistry.IsInventoryInputReserved(input);
		}

		// Token: 0x170014EC RID: 5356
		// (get) Token: 0x060083E0 RID: 33760 RVA: 0x0035C655 File Offset: 0x0035A855
		public GenericInput ClothCycleInput
		{
			get
			{
				return this.clothCycleInput;
			}
		}

		// Token: 0x170014ED RID: 5357
		// (get) Token: 0x060083E1 RID: 33761 RVA: 0x0035C65D File Offset: 0x0035A85D
		public bool CanCycleClothFromInput
		{
			get
			{
				return this.activeClothIndex >= 0 && this.CanUseInventoryInput(this.clothCycleInput);
			}
		}

		// Token: 0x060083E2 RID: 33762 RVA: 0x0035C678 File Offset: 0x0035A878
		private void SelectSlotFromInput(int index)
		{
			if (WallCoverActivityLock.ActiveController != null)
			{
				return;
			}
			GenericInput genericInput = ((index == 0) ? this.slot1Input : ((index == 1) ? this.slot2Input : ((index == 2) ? this.slot3Input : null)));
			if (genericInput != null && this.CanUseInventoryInput(genericInput))
			{
				this.SelectSlot(index);
			}
		}

		// Token: 0x060083E3 RID: 33763 RVA: 0x0035C6CB File Offset: 0x0035A8CB
		private void CycleClothFromInput()
		{
			if (this.CanUseInventoryInput(this.clothCycleInput))
			{
				this.CycleCloth(1);
			}
		}

		// Token: 0x170014EE RID: 5358
		// (get) Token: 0x060083E4 RID: 33764 RVA: 0x0035C6E2 File Offset: 0x0035A8E2
		public bool CanClearControllerSelection
		{
			get
			{
				return this.CanUseInventoryInput(PlayerInventoryManager.ControllerCancelSelectionInput) && (this.activeClothIndex >= 0 || (this.activeSlotIndex >= 0 && !this.IsSlotInputLocked));
			}
		}

		// Token: 0x060083E5 RID: 33765 RVA: 0x0035C714 File Offset: 0x0035A914
		private bool TryCancelControllerSelection()
		{
			if (!this.CanClearControllerSelection || InventoryControllerInput.PlacementHeld || !ControllerInput.CancelPressed || ControllerInput.Modifier || !EscapeInputCoordinator.TryConsume(this))
			{
				return false;
			}
			if (this.activeClothIndex >= 0)
			{
				this.ClearActiveCloth();
			}
			else
			{
				this.SelectSlot(this.activeSlotIndex);
			}
			ControllerInput.ConsumeUIInput();
			return true;
		}

		// Token: 0x060083E6 RID: 33766 RVA: 0x0035C76C File Offset: 0x0035A96C
		public void CycleControllerHotbar(int direction)
		{
			if (direction == 0 || GameplayInputGate.BlocksGameplayInput || !this.CanUseInventoryInput(this.clothSelectInput))
			{
				return;
			}
			if (InteractionCandidateRegistry.IsInventoryInputReserved((direction < 0) ? PlayerInventoryManager.ControllerPreviousSlotInput : PlayerInventoryManager.ControllerNextSlotInput))
			{
				return;
			}
			if (direction < 0)
			{
				if (this.activeClothIndex >= 0)
				{
					this.CycleCloth(1);
					return;
				}
				this.ToggleClothFocus();
				return;
			}
			else
			{
				if (WallCoverActivityLock.ActiveController != null)
				{
					return;
				}
				bool flag = this.activeClothIndex >= 0 || this.activeSlotIndex < 0;
				if (this.IsSlotInputLocked)
				{
					if (flag && this.GetItemAt(this.activeSlotIndex) != null)
					{
						this.SelectSlotFromInput(this.activeSlotIndex);
					}
					return;
				}
				int num = Mathf.Min(this.maxSlots, this.slots.Count);
				if (num == 0)
				{
					return;
				}
				int num2 = (flag ? ((this.GetItemAt(this._lastFocusedItemIndex) != null) ? this._lastFocusedItemIndex : Mathf.Max(0, this.activeSlotIndex)) : ((this.activeSlotIndex + 1) % num));
				for (int i = 0; i < num; i++)
				{
					int num3 = (num2 + i) % num;
					if (!(this.GetItemAt(num3) == null))
					{
						if (flag || this.activeSlotIndex != num3)
						{
							this.SelectSlotFromInput(num3);
						}
						return;
					}
				}
				return;
			}
		}

		// Token: 0x060083E7 RID: 33767 RVA: 0x0035C8A4 File Offset: 0x0035AAA4
		private void ToggleClothFocus()
		{
			if (this.activeClothIndex != -1)
			{
				this.ClearActiveCloth();
				return;
			}
			if (this._lastFocusedClothIndex >= 0 && this._lastFocusedClothIndex < this.clothSlots.Count && this.clothSlots[this._lastFocusedClothIndex] != null)
			{
				this.SelectCloth(this._lastFocusedClothIndex);
				return;
			}
			for (int i = 0; i < this.clothSlots.Count; i++)
			{
				if (this.clothSlots[i] != null)
				{
					this.SelectCloth(i);
					return;
				}
			}
		}

		// Token: 0x060083E8 RID: 33768 RVA: 0x0035C935 File Offset: 0x0035AB35
		private void UpdateDurability()
		{
			this.TickDurability(Time.deltaTime);
		}

		// Token: 0x060083E9 RID: 33769 RVA: 0x0035C942 File Offset: 0x0035AB42
		private void TickDurability(float deltaTime)
		{
			this.TickFocusedItemDurability(deltaTime);
			this.TickUnfocusedWornItemDurability(deltaTime);
		}

		// Token: 0x060083EA RID: 33770 RVA: 0x0035C954 File Offset: 0x0035AB54
		private void TickFocusedItemDurability(float deltaTime)
		{
			int num = this.activeSlotIndex;
			if (num < 0 || num >= this.slots.Count)
			{
				return;
			}
			InventoryItemSO inventoryItemSO = this.slots[num];
			if (inventoryItemSO == null || !inventoryItemSO.hasDurability || inventoryItemSO.maxDurability <= 0f)
			{
				return;
			}
			if (inventoryItemSO.durabilityDrainMode == DurabilityDrainMode.ManualPerUse)
			{
				if (!PlayerInventoryManager.FreeItemDurabilityDisabled && this.durability[num] <= 0f)
				{
					this.BreakItem(num);
				}
				return;
			}
			if (PlayerInventoryManager.InfiniteItemDurabilityInActiveScene)
			{
				return;
			}
			if (GameplayDurationPause.IsPaused)
			{
				return;
			}
			bool flag = this.IsTimedItemInUse(num, inventoryItemSO);
			bool flag2 = inventoryItemSO.durabilityDrainMode != DurabilityDrainMode.OnlyWhileInUse || ItemUsageSignal.Active;
			if (flag && flag2 && this.durability[num] > 0f)
			{
				this.DrainTimedItem(num, inventoryItemSO, deltaTime);
			}
			if (this.durability[num] <= 0f && !ItemUsageSignal.Active)
			{
				this.BreakItem(num);
			}
		}

		// Token: 0x060083EB RID: 33771 RVA: 0x0035CA40 File Offset: 0x0035AC40
		private void TickUnfocusedWornItemDurability(float deltaTime)
		{
			if (PlayerInventoryManager.InfiniteItemDurabilityInActiveScene)
			{
				return;
			}
			if (GameplayDurationPause.IsPaused)
			{
				return;
			}
			for (int i = 0; i < this.slots.Count; i++)
			{
				if (i != this.activeSlotIndex)
				{
					WearableItemSO wearableItemSO = this.slots[i] as WearableItemSO;
					if (wearableItemSO != null && wearableItemSO.hasDurability && wearableItemSO.maxDurability > 0f && wearableItemSO.durabilityDrainMode != DurabilityDrainMode.ManualPerUse && this.IsTimedItemInUse(i, wearableItemSO) && (wearableItemSO.durabilityDrainMode != DurabilityDrainMode.OnlyWhileInUse || ItemUsageSignal.Active))
					{
						if (this.durability[i] > 0f)
						{
							this.DrainTimedItem(i, wearableItemSO, deltaTime);
						}
						if (this.durability[i] <= 0f && !ItemUsageSignal.Active)
						{
							this.BreakItem(i);
						}
					}
				}
			}
		}

		// Token: 0x060083EC RID: 33772 RVA: 0x0035CB10 File Offset: 0x0035AD10
		private void DrainTimedItem(int index, InventoryItemSO item, float deltaTime)
		{
			float num = Mathf.Max(0.25f, RunModeService.ItemDurationMultiplier);
			this.durability[index] = Mathf.Max(0f, this.durability[index] - deltaTime / num);
			Action<int, float> onDurabilityChanged = this.OnDurabilityChanged;
			if (onDurabilityChanged == null)
			{
				return;
			}
			onDurabilityChanged(index, Mathf.Clamp01(this.durability[index] / item.maxDurability));
		}

		// Token: 0x060083ED RID: 33773 RVA: 0x0035CB7C File Offset: 0x0035AD7C
		private bool IsTimedItemInUse(int index, InventoryItemSO item)
		{
			WearableItemSO wearableItemSO = item as WearableItemSO;
			if (wearableItemSO != null)
			{
				if (this._wearableUseController == null)
				{
					vThirdPersonInput componentInParent = base.GetComponentInParent<vThirdPersonInput>(true);
					Transform transform = ((componentInParent != null) ? componentInParent.transform : base.transform.root);
					this._wearableUseController = transform.GetComponentInChildren<WearableUseController>(true);
				}
				return this._wearableUseController != null && this._wearableUseController.IsItemInstanceWorn(wearableItemSO, index);
			}
			if (item is HideFlashItemSO)
			{
				if (this._timedCoverOwner == null)
				{
					vThirdPersonInput componentInParent2 = base.GetComponentInParent<vThirdPersonInput>(true);
					Transform transform2 = ((componentInParent2 != null) ? componentInParent2.transform : base.transform.root);
					this._timedCoverOwner = transform2.GetComponentInChildren<HideFlashController>(true);
				}
				return this._activeItem == item && this._timedCoverOwner != null && this._timedCoverOwner.ActiveItem == item && HideFlashController.HasHeldCoverItem;
			}
			return this._activeItem == item;
		}

		// Token: 0x060083EE RID: 33774 RVA: 0x0035CC7E File Offset: 0x0035AE7E
		private void BreakItem(int index)
		{
			this.DropItem(index);
		}

		// Token: 0x060083EF RID: 33775 RVA: 0x0035CC88 File Offset: 0x0035AE88
		public float GetItemDurability01(int index)
		{
			InventoryItemSO itemAt = this.GetItemAt(index);
			if (itemAt == null || !itemAt.hasDurability || itemAt.maxDurability <= 0f)
			{
				return 1f;
			}
			return Mathf.Clamp01(this.durability[index] / itemAt.maxDurability);
		}

		// Token: 0x170014EF RID: 5359
		// (get) Token: 0x060083F0 RID: 33776 RVA: 0x0035CCD9 File Offset: 0x0035AED9
		private static bool FreeItemDurabilityDisabled
		{
			get
			{
				return RunModeService.IsGameplayBoot && RunModeService.UnlimitedItemDurability;
			}
		}

		// Token: 0x060083F1 RID: 33777 RVA: 0x0035CCEC File Offset: 0x0035AEEC
		public bool HasItemDurabilityAt(int index, InventoryItemSO expectedItem, float amount = 1f)
		{
			return expectedItem != null && amount > 0f && index >= 0 && index < this.slots.Count && index < this.durability.Count && this.slots[index] == expectedItem && expectedItem.hasDurability && expectedItem.maxDurability > 0f && (PlayerInventoryManager.FreeItemDurabilityDisabled || this.durability[index] + 0.0001f >= amount);
		}

		// Token: 0x060083F2 RID: 33778 RVA: 0x0035CD71 File Offset: 0x0035AF71
		public bool HasActiveItemDurability(InventoryItemSO expectedItem, float amount = 1f)
		{
			return this.HasItemDurabilityAt(this.activeSlotIndex, expectedItem, amount);
		}

		// Token: 0x060083F3 RID: 33779 RVA: 0x0035CD84 File Offset: 0x0035AF84
		public bool TryConsumeItemDurabilityAt(int index, InventoryItemSO expectedItem, float amount, out float remaining)
		{
			remaining = 0f;
			if (!this.HasItemDurabilityAt(index, expectedItem, amount))
			{
				return false;
			}
			if (PlayerInventoryManager.FreeItemDurabilityDisabled)
			{
				remaining = this.durability[index];
				return true;
			}
			this.durability[index] = Mathf.Max(0f, this.durability[index] - amount);
			remaining = this.durability[index];
			Action<int, float> onDurabilityChanged = this.OnDurabilityChanged;
			if (onDurabilityChanged != null)
			{
				onDurabilityChanged(index, Mathf.Clamp01(this.durability[index] / expectedItem.maxDurability));
			}
			if (this.durability[index] <= 0f)
			{
				this.BreakItem(index);
			}
			return true;
		}

		// Token: 0x060083F4 RID: 33780 RVA: 0x0035CE36 File Offset: 0x0035B036
		public bool TryConsumeActiveItemDurability(InventoryItemSO expectedItem, float amount, out float remaining)
		{
			return this.TryConsumeItemDurabilityAt(this.activeSlotIndex, expectedItem, amount, out remaining);
		}

		// Token: 0x060083F5 RID: 33781 RVA: 0x0035CE47 File Offset: 0x0035B047
		public bool AddItem(InventoryItemSO item)
		{
			return this.AddItem(item, (item != null) ? item.rarity : ItemRarity.Common, -1, 1f, true, true);
		}

		// Token: 0x060083F6 RID: 33782 RVA: 0x0035CE6C File Offset: 0x0035B06C
		public bool CanAcceptWorldPickup(InventoryItemSO item, ItemRarity? instanceRarity = null, int variantIndex = -1, bool registerFind = true)
		{
			if (item == null || item.isVaulted || !CollarAvailabilityPolicy.IsAvailable(item))
			{
				return false;
			}
			if (item.category == InventoryCategory.Clothing)
			{
				return this.HasFreeClothingSlot && LongWayHomeLootPolicy.CanAddClothing(item, registerFind);
			}
			ItemRarity itemRarity = instanceRarity ?? item.rarity;
			for (int i = 0; i < Mathf.Min(this.maxSlots, this.slots.Count); i++)
			{
				InventoryItemSO inventoryItemSO = this.slots[i];
				if (inventoryItemSO == null)
				{
					return true;
				}
				if (item.isStackable && this.GetItemRarityAt(i) == itemRarity && this.GetItemVariantAt(i) == variantIndex && this.GetItemStackCount(i) < Mathf.Max(1, item.maxStackSize))
				{
					if (!(inventoryItemSO == item))
					{
						IInventoryStackMergeable inventoryStackMergeable = inventoryItemSO as IInventoryStackMergeable;
						if (inventoryStackMergeable == null || !inventoryStackMergeable.CanMergeWith(item))
						{
							goto IL_00C0;
						}
					}
					return true;
				}
				IL_00C0:;
			}
			return this.CanSwapWorldPickupIntoSelectedSlot(item, this.activeSlotIndex, this.GetActiveSlotItem());
		}

		// Token: 0x060083F7 RID: 33783 RVA: 0x0035CF6C File Offset: 0x0035B16C
		private bool IsActiveSlotWornOnBody()
		{
			InventoryItemSO activeSlotItem = this.GetActiveSlotItem();
			return activeSlotItem is WearableItemSO && this.IsTimedItemInUse(this.activeSlotIndex, activeSlotItem);
		}

		// Token: 0x060083F8 RID: 33784 RVA: 0x0035CF98 File Offset: 0x0035B198
		private InventoryDropController ResolveSwapDropController()
		{
			InventoryDropController component = base.GetComponent<InventoryDropController>();
			if (!(component != null))
			{
				return InventoryDropController.Instance;
			}
			return component;
		}

		// Token: 0x060083F9 RID: 33785 RVA: 0x0035CFBC File Offset: 0x0035B1BC
		public bool CanSwapWorldPickupIntoSelectedSlot(InventoryItemSO incoming, int slotIndex, InventoryItemSO expectedItem)
		{
			if (incoming == null || incoming.category != InventoryCategory.Item || incoming.isVaulted || !CollarAvailabilityPolicy.IsAvailable(incoming) || expectedItem == null || slotIndex < 0 || slotIndex >= this.maxSlots || slotIndex != this.activeSlotIndex || this.GetItemAt(slotIndex) != expectedItem || this.IsSlotInputLocked || this.IsActiveSlotWornOnBody() || ItemUsageSignal.Active || (ExhibitionEventDirector.Instance != null && ExhibitionEventDirector.Instance.IsEventActive))
			{
				return false;
			}
			IInventoryWholeStackWorldProvider inventoryWholeStackWorldProvider = incoming as IInventoryWholeStackWorldProvider;
			int num = ((inventoryWholeStackWorldProvider != null) ? inventoryWholeStackWorldProvider.InventoryStackQuantity : 1);
			if (num < 1 || num > (incoming.isStackable ? Mathf.Max(1, incoming.maxStackSize) : 1))
			{
				return false;
			}
			InventoryDropController inventoryDropController = this.ResolveSwapDropController();
			return inventoryDropController != null && inventoryDropController.CanStageSwapDrop(this, expectedItem, this.GetItemStackCount(slotIndex));
		}

		// Token: 0x060083FA RID: 33786 RVA: 0x0035D0A0 File Offset: 0x0035B2A0
		public bool CanPrepareWorldPickupSwap(InventoryItemSO incoming, int slotIndex, InventoryItemSO expectedItem)
		{
			if (TextInputFocus.BlocksGameplayInput || !this.CanSwapWorldPickupIntoSelectedSlot(incoming, slotIndex, expectedItem))
			{
				return false;
			}
			HideFlashController cover = PlayerActivityGate.GetResourceOwner<HideFlashController>(PlayerContextResource.HeldItem);
			return cover != null && cover.CanStowForInventorySwap(this, slotIndex, expectedItem) && PlayerActivityGate.CanAcquireAfterReleasing(PlayerActivity.Interact, PlayerContextResource.WorldInteraction, (Object owner) => owner == cover);
		}

		// Token: 0x060083FB RID: 33787 RVA: 0x0035D108 File Offset: 0x0035B308
		public bool TryPrepareWorldPickupSwap(InventoryItemSO incoming, int slotIndex, InventoryItemSO expectedItem)
		{
			if (PlayerActivityGate.CanStartWorldInteraction)
			{
				return this.CanSwapWorldPickupIntoSelectedSlot(incoming, slotIndex, expectedItem);
			}
			if (!this.CanPrepareWorldPickupSwap(incoming, slotIndex, expectedItem))
			{
				return false;
			}
			HideFlashController resourceOwner = PlayerActivityGate.GetResourceOwner<HideFlashController>(PlayerContextResource.HeldItem);
			return !(resourceOwner == null) && resourceOwner.TryStowForInventorySwap(this, slotIndex, expectedItem) && PlayerActivityGate.CanStartWorldInteraction && this.CanSwapWorldPickupIntoSelectedSlot(incoming, slotIndex, expectedItem);
		}

		// Token: 0x060083FC RID: 33788 RVA: 0x0035D164 File Offset: 0x0035B364
		public bool TrySwapWorldPickupIntoSelectedSlot(InventoryItemSO incoming, ItemRarity rarity, int variantIndex, float durability01, bool registerFind, int slotIndex, InventoryItemSO expectedItem, bool selectOnAdd = true)
		{
			if (!this.CanSwapWorldPickupIntoSelectedSlot(incoming, slotIndex, expectedItem))
			{
				return false;
			}
			InventoryDropController inventoryDropController = this.ResolveSwapDropController();
			int itemStackCount = this.GetItemStackCount(slotIndex);
			ItemRarity itemRarityAt = this.GetItemRarityAt(slotIndex);
			int itemVariantAt = this.GetItemVariantAt(slotIndex);
			float itemDurability = this.GetItemDurability01(slotIndex);
			List<GameObject> list;
			if (!inventoryDropController.TryStageSwapDrops(this, expectedItem, itemStackCount, itemRarityAt, itemVariantAt, itemDurability, out list))
			{
				return false;
			}
			if (!this.CanSwapWorldPickupIntoSelectedSlot(incoming, slotIndex, expectedItem) || this.GetItemStackCount(slotIndex) != itemStackCount || this.GetItemRarityAt(slotIndex) != itemRarityAt || this.GetItemVariantAt(slotIndex) != itemVariantAt || this.GetItemDurability01(slotIndex) != itemDurability)
			{
				InventoryDropController.DiscardSwapDrops(list);
				return false;
			}
			this.DropEntireItemStack(slotIndex);
			this.slots[slotIndex] = incoming;
			List<int> list2 = this.stackCounts;
			IInventoryWholeStackWorldProvider inventoryWholeStackWorldProvider = incoming as IInventoryWholeStackWorldProvider;
			list2[slotIndex] = ((inventoryWholeStackWorldProvider != null) ? inventoryWholeStackWorldProvider.InventoryStackQuantity : 1);
			this.durability[slotIndex] = incoming.maxDurability * Mathf.Clamp01(durability01);
			this.itemRarities[slotIndex] = rarity;
			this.itemVariants[slotIndex] = variantIndex;
			InventoryDropController.ActivateSwapDrops(list);
			if (registerFind)
			{
				CollectionUnlockRegistry.RegisterGameplayItemFound(incoming, rarity, variantIndex, this);
			}
			Action<int, InventoryItemSO> onItemAdded = this.OnItemAdded;
			if (onItemAdded != null)
			{
				onItemAdded(slotIndex, incoming);
			}
			if (selectOnAdd)
			{
				this.SelectSlot(slotIndex);
			}
			return true;
		}

		// Token: 0x060083FD RID: 33789 RVA: 0x0035D2A4 File Offset: 0x0035B4A4
		public bool AddItem(InventoryItemSO item, ItemRarity instanceRarity, int variantIndex, float durability01 = 1f, bool registerFind = true, bool selectOnAdd = true)
		{
			if (item == null || item.isVaulted || !CollarAvailabilityPolicy.IsAvailable(item))
			{
				return false;
			}
			if (item.category == InventoryCategory.Clothing)
			{
				return this.AddClothingReturnIndex(item, true, instanceRarity, variantIndex, durability01, registerFind) >= 0;
			}
			if (item.isStackable)
			{
				int num = Mathf.Max(1, item.maxStackSize);
				for (int i = 0; i < this.maxSlots; i++)
				{
					InventoryItemSO inventoryItemSO = this.slots[i];
					bool flag = inventoryItemSO == item;
					IInventoryStackMergeable inventoryStackMergeable = inventoryItemSO as IInventoryStackMergeable;
					bool flag2 = inventoryStackMergeable != null && inventoryStackMergeable.CanMergeWith(item);
					if ((flag || flag2) && this.GetItemRarityAt(i) == instanceRarity && this.GetItemVariantAt(i) == variantIndex && this.GetItemStackCount(i) < num)
					{
						IInventoryWholeStackWorldProvider inventoryWholeStackWorldProvider = item as IInventoryWholeStackWorldProvider;
						int num2 = ((inventoryWholeStackWorldProvider != null) ? Mathf.Max(1, inventoryWholeStackWorldProvider.InventoryStackQuantity) : 1);
						if (flag || inventoryStackMergeable.MergeFrom(item))
						{
							int itemStackCount = this.GetItemStackCount(i);
							this.stackCounts[i] = Mathf.Min(num, itemStackCount + num2);
							if (registerFind)
							{
								CollectionUnlockRegistry.RegisterGameplayItemFound(inventoryItemSO, instanceRarity, variantIndex, this);
							}
							Action<int, int, int> onItemStackChanged = this.OnItemStackChanged;
							if (onItemStackChanged != null)
							{
								onItemStackChanged(i, itemStackCount, this.stackCounts[i]);
							}
							Action<int, InventoryItemSO> onItemAdded = this.OnItemAdded;
							if (onItemAdded != null)
							{
								onItemAdded(i, inventoryItemSO);
							}
							if (selectOnAdd)
							{
								if (this.activeSlotIndex != i)
								{
									this.SelectSlot(i);
								}
								else
								{
									Action<int> onActiveSlotChanged = this.OnActiveSlotChanged;
									if (onActiveSlotChanged != null)
									{
										onActiveSlotChanged(i);
									}
								}
							}
							return true;
						}
					}
				}
			}
			int num3 = -1;
			for (int j = 0; j < this.maxSlots; j++)
			{
				if (this.slots[j] == null)
				{
					num3 = j;
					break;
				}
			}
			if (num3 != -1)
			{
				this.slots[num3] = item;
				List<int> list = this.stackCounts;
				int num4 = num3;
				IInventoryWholeStackWorldProvider inventoryWholeStackWorldProvider2 = item as IInventoryWholeStackWorldProvider;
				list[num4] = ((inventoryWholeStackWorldProvider2 != null) ? Mathf.Clamp(inventoryWholeStackWorldProvider2.InventoryStackQuantity, 1, Mathf.Max(1, item.maxStackSize)) : 1);
				this.durability[num3] = item.maxDurability * Mathf.Clamp01(durability01);
				this.itemRarities[num3] = instanceRarity;
				this.itemVariants[num3] = variantIndex;
				if (registerFind)
				{
					CollectionUnlockRegistry.RegisterGameplayItemFound(item, instanceRarity, variantIndex, this);
				}
				Action<int, InventoryItemSO> onItemAdded2 = this.OnItemAdded;
				if (onItemAdded2 != null)
				{
					onItemAdded2(num3, item);
				}
				if (selectOnAdd)
				{
					this.SelectSlot(num3);
				}
				return true;
			}
			if (this.IsSlotInputLocked || this.IsActiveSlotWornOnBody())
			{
				ToastService.Show("Inventory is full! Remove the active item first.", "Inventory", null, -1f, "inventory_feedback");
				return false;
			}
			if (this.activeSlotIndex != -1)
			{
				return this.TrySwapWorldPickupIntoSelectedSlot(item, instanceRarity, variantIndex, durability01, registerFind, this.activeSlotIndex, this.GetActiveSlotItem(), selectOnAdd);
			}
			ToastService.Show("Inventory is full! Select a slot to swap items.", "Inventory", null, -1f, "inventory_feedback");
			return false;
		}

		// Token: 0x170014F0 RID: 5360
		// (get) Token: 0x060083FE RID: 33790 RVA: 0x0035D578 File Offset: 0x0035B778
		private bool CanUseInventoryOnRoof
		{
			get
			{
				return CarRoofCarrier.IsCarSurfing(base.transform) && PlayerActivityGate.Allows(PlayerActivity.Inventory) && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI);
			}
		}

		// Token: 0x060083FF RID: 33791 RVA: 0x0035D5A0 File Offset: 0x0035B7A0
		public void SelectSlot(int index)
		{
			if (PoolWaterEntryPreparation.IsSwimming(base.transform))
			{
				return;
			}
			if (WallCoverActivityLock.ActiveController != null)
			{
				return;
			}
			if (ExhibitionEventDirector.Instance != null && ExhibitionEventDirector.Instance.IsEventActive)
			{
				return;
			}
			if (index < 0 || index >= this.maxSlots)
			{
				return;
			}
			if (index < this.slots.Count && this.slots[index] != null && PlayerActivityGate.IsResourceOwned(PlayerContextResource.Rider) && !this.CanUseInventoryOnRoof)
			{
				return;
			}
			if (this.activeClothIndex != -1)
			{
				this.ClearActiveCloth();
			}
			if (this.slots[index] == null)
			{
				if (!this.IsSlotInputLocked && this.activeSlotIndex != -1)
				{
					this.UnequipCurrent(false);
				}
				if (!this.IsSlotInputLocked)
				{
					this.activeSlotIndex = -1;
					Action<int> onActiveSlotChanged = this.OnActiveSlotChanged;
					if (onActiveSlotChanged == null)
					{
						return;
					}
					onActiveSlotChanged(-1);
					return;
				}
				else
				{
					Action<int> onActiveSlotChanged2 = this.OnActiveSlotChanged;
					if (onActiveSlotChanged2 == null)
					{
						return;
					}
					onActiveSlotChanged2(this.activeSlotIndex);
					return;
				}
			}
			else if (this.activeSlotIndex == index)
			{
				if (this.IsSlotInputLocked)
				{
					Action<int> onActiveSlotChanged3 = this.OnActiveSlotChanged;
					if (onActiveSlotChanged3 == null)
					{
						return;
					}
					onActiveSlotChanged3(this.activeSlotIndex);
					return;
				}
				else
				{
					this.UnequipCurrent(false);
					this.activeSlotIndex = -1;
					Action<int> onActiveSlotChanged4 = this.OnActiveSlotChanged;
					if (onActiveSlotChanged4 == null)
					{
						return;
					}
					onActiveSlotChanged4(-1);
					return;
				}
			}
			else if (this.IsSlotInputLocked)
			{
				Action<int> onActiveSlotChanged5 = this.OnActiveSlotChanged;
				if (onActiveSlotChanged5 == null)
				{
					return;
				}
				onActiveSlotChanged5(this.activeSlotIndex);
				return;
			}
			else
			{
				if (TutorialService.TryShowFirstUse("item_types"))
				{
					return;
				}
				this.activeSlotIndex = index;
				this._lastFocusedItemIndex = index;
				this.EquipItem(this.slots[index]);
				Action<int> onActiveSlotChanged6 = this.OnActiveSlotChanged;
				if (onActiveSlotChanged6 == null)
				{
					return;
				}
				onActiveSlotChanged6(index);
				return;
			}
		}

		// Token: 0x06008400 RID: 33792 RVA: 0x0035D73D File Offset: 0x0035B93D
		private void LateUpdate()
		{
			if (this.coverController != null)
			{
				this.coverController.ContinueHeldItemCoverDeformationCleanup();
			}
		}

		// Token: 0x06008401 RID: 33793 RVA: 0x0035D758 File Offset: 0x0035B958
		private void EquipItem(InventoryItemSO item)
		{
			this.UnequipCurrent(false);
			if (item == null)
			{
				return;
			}
			WearableItemSO wearableItemSO = item as WearableItemSO;
			if (wearableItemSO != null && wearableItemSO.wearOnSelection)
			{
				return;
			}
			if (item is WearableItemSO && this.IsTimedItemInUse(this.activeSlotIndex, item))
			{
				return;
			}
			this._activeItem = item;
			if (this.coverController != null)
			{
				this.coverController.DeactivateCoverPose();
				this.coverController.enabled = false;
				this.coverController.ForceLayerWeight(0f);
				this.coverController.RestoreHeldItemCoverDeformations();
				Debug.Log("[PlayerInventoryManager] Deactivated CoverPose and disabled CoverPoseController for item equip");
			}
			Transform transform = ((this.playerAnimator != null && this.playerAnimator.isHuman) ? this.playerAnimator.GetBoneTransform(item.attachBone) : null);
			if (transform != null && item.handPrefab != null)
			{
				this._spawnedHandObject = Object.Instantiate<GameObject>(item.handPrefab, transform);
				FirstPersonVisibleProp.Mark(this._spawnedHandObject);
				this._spawnedHandObject.transform.localPosition = item.handPositionOffset;
				this._spawnedHandObject.transform.localRotation = Quaternion.Euler(item.handRotationOffset);
				this._spawnedHandObject.transform.localScale = Vector3.Scale(item.handPrefab.transform.localScale, item.EffectiveHandScaleMultiplier);
				IInventoryInstanceVisualProvider inventoryInstanceVisualProvider = item as IInventoryInstanceVisualProvider;
				if (inventoryInstanceVisualProvider != null)
				{
					inventoryInstanceVisualProvider.ConfigureHandInstance(this._spawnedHandObject);
				}
				this._spawnedHandObject.SetActive(true);
				this.EnableAllRenderers(this._spawnedHandObject);
				this.DisableInteractablesOnObject(this._spawnedHandObject);
				if (item.isVehicleItem)
				{
					CarriedVehiclePropConfigurator.ConfigureAsCarriedProp(this._spawnedHandObject);
				}
				Debug.Log(string.Format("[PlayerInventoryManager] Spawned hand object: {0} on bone {1}", this._spawnedHandObject.name, item.attachBone));
			}
			if (this.playerAnimator != null)
			{
				if (!string.IsNullOrEmpty(item.holdAnimationParameter) && PlayerInventoryManager.HasAnimatorParameter(this.playerAnimator, item.holdAnimationParameter))
				{
					this.playerAnimator.SetBool(item.holdAnimationParameter, true);
				}
				if (this.coverController == null && this._armsLayerIndex != -1)
				{
					this.playerAnimator.SetLayerWeight(this._armsLayerIndex, 0f);
				}
				this.StartLayerFade(this._holdLayerIndex, item.holdLayerWeight);
				int num = this.ResolveSecondaryHoldLayer(item);
				if (num != -1)
				{
					this.playerAnimator.SetLayerWeight(num, item.secondaryHoldLayerWeight);
				}
			}
			if (item.equipSFX != null)
			{
				AudioSource.PlayClipAtPoint(item.equipSFX, base.transform.position);
			}
		}

		// Token: 0x06008402 RID: 33794 RVA: 0x0035D9F0 File Offset: 0x0035BBF0
		private void UnequipCurrent(bool immediateHoldRelease = false)
		{
			if (this._spawnedHandObject != null)
			{
				if (immediateHoldRelease)
				{
					this._spawnedHandObject.SetActive(false);
				}
				Object.Destroy(this._spawnedHandObject);
				this._spawnedHandObject = null;
			}
			if (this._activeItem != null && this.playerAnimator != null)
			{
				if (!string.IsNullOrEmpty(this._activeItem.holdAnimationParameter) && PlayerInventoryManager.HasAnimatorParameter(this.playerAnimator, this._activeItem.holdAnimationParameter))
				{
					this.playerAnimator.SetBool(this._activeItem.holdAnimationParameter, false);
				}
				if (immediateHoldRelease)
				{
					if (this._layerFadeRoutine != null)
					{
						base.StopCoroutine(this._layerFadeRoutine);
						this._layerFadeRoutine = null;
					}
					if (this._holdLayerIndex != -1)
					{
						this.playerAnimator.SetLayerWeight(this._holdLayerIndex, 0f);
					}
				}
				else
				{
					this.StartLayerFade(this._holdLayerIndex, 0f);
				}
				int num = this.ResolveSecondaryHoldLayer(this._activeItem);
				if (num != -1)
				{
					this.playerAnimator.SetLayerWeight(num, 0f);
				}
			}
			this._activeItem = null;
			if (this.coverController != null)
			{
				this.coverController.enabled = true;
			}
		}

		// Token: 0x06008403 RID: 33795 RVA: 0x0035DB22 File Offset: 0x0035BD22
		public void ToggleHandPrefabVisibility(bool visible)
		{
			if (this._spawnedHandObject != null)
			{
				this._spawnedHandObject.SetActive(visible);
			}
		}

		// Token: 0x06008404 RID: 33796 RVA: 0x0035DB3E File Offset: 0x0035BD3E
		public void SuspendActiveItem()
		{
			if (this._activeItem == null)
			{
				return;
			}
			this.UnequipCurrent(false);
		}

		// Token: 0x06008405 RID: 33797 RVA: 0x0035DB56 File Offset: 0x0035BD56
		public void SuspendActiveItemImmediate()
		{
			if (this._activeItem == null)
			{
				return;
			}
			this.UnequipCurrent(true);
		}

		// Token: 0x06008406 RID: 33798 RVA: 0x0035DB70 File Offset: 0x0035BD70
		public void StowHeldItemForInteraction()
		{
			WearableItemSO wearableItemSO = this.GetActiveSlotItem() as WearableItemSO;
			bool flag = wearableItemSO != null && wearableItemSO.stowForInteraction;
			if (this._wearableUseController == null)
			{
				vThirdPersonInput componentInParent = base.GetComponentInParent<vThirdPersonInput>(true);
				this._wearableUseController = ((componentInParent != null) ? componentInParent.transform : base.transform.root).GetComponentInChildren<WearableUseController>(true);
			}
			WearableUseController wearableUseController = this._wearableUseController;
			if (wearableUseController != null)
			{
				wearableUseController.StowEquipmentForInteraction();
			}
			if (this._activeItem == null && this._spawnedHandObject == null && !flag)
			{
				return;
			}
			this.UnequipCurrent(true);
			this.activeSlotIndex = -1;
			Action<int> onActiveSlotChanged = this.OnActiveSlotChanged;
			if (onActiveSlotChanged == null)
			{
				return;
			}
			onActiveSlotChanged(-1);
		}

		// Token: 0x06008407 RID: 33799 RVA: 0x0035DC24 File Offset: 0x0035BE24
		public void ResumeActiveItem()
		{
			if (PoolWaterEntryPreparation.IsSwimming(base.transform))
			{
				return;
			}
			if (ExhibitionEventDirector.Instance != null && ExhibitionEventDirector.Instance.IsEventActive)
			{
				return;
			}
			if (PlayerActivityGate.IsResourceOwned(PlayerContextResource.Rider) && !this.CanUseInventoryOnRoof)
			{
				return;
			}
			if (this.activeSlotIndex < 0 || this.activeSlotIndex >= this.maxSlots)
			{
				return;
			}
			if (this.slots[this.activeSlotIndex] == null)
			{
				return;
			}
			if (this._activeItem != null)
			{
				return;
			}
			this.EquipItem(this.slots[this.activeSlotIndex]);
		}

		// Token: 0x06008408 RID: 33800 RVA: 0x0035DCC4 File Offset: 0x0035BEC4
		private void DisableInteractablesOnObject(GameObject obj)
		{
			IInteractable[] componentsInChildren = obj.GetComponentsInChildren<IInteractable>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				MonoBehaviour monoBehaviour = componentsInChildren[i] as MonoBehaviour;
				if (monoBehaviour != null)
				{
					monoBehaviour.enabled = false;
				}
			}
			InteractablePromptUI[] componentsInChildren2 = obj.GetComponentsInChildren<InteractablePromptUI>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].gameObject.SetActive(false);
			}
			InventoryPickup[] componentsInChildren3 = obj.GetComponentsInChildren<InventoryPickup>(true);
			for (int i = 0; i < componentsInChildren3.Length; i++)
			{
				componentsInChildren3[i].enabled = false;
			}
		}

		// Token: 0x06008409 RID: 33801 RVA: 0x0035DD40 File Offset: 0x0035BF40
		private void EnableAllRenderers(GameObject obj)
		{
			foreach (Renderer renderer in obj.GetComponentsInChildren<Renderer>(true))
			{
				renderer.enabled = true;
				if (renderer.shadowCastingMode == ShadowCastingMode.ShadowsOnly)
				{
					renderer.shadowCastingMode = ShadowCastingMode.On;
				}
			}
		}

		// Token: 0x0600840A RID: 33802 RVA: 0x0035DD7E File Offset: 0x0035BF7E
		private int ResolveSecondaryHoldLayer(InventoryItemSO item)
		{
			if (this.playerAnimator == null || item == null || string.IsNullOrEmpty(item.secondaryHoldLayerName))
			{
				return -1;
			}
			return this.playerAnimator.GetLayerIndex(item.secondaryHoldLayerName);
		}

		// Token: 0x0600840B RID: 33803 RVA: 0x0035DDB7 File Offset: 0x0035BFB7
		private void StartLayerFade(int layerIndex, float targetWeight)
		{
			if (layerIndex == -1)
			{
				return;
			}
			if (this._layerFadeRoutine != null)
			{
				base.StopCoroutine(this._layerFadeRoutine);
			}
			this._layerFadeRoutine = base.StartCoroutine(this.LayerFadeRoutine(layerIndex, targetWeight));
		}

		// Token: 0x0600840C RID: 33804 RVA: 0x0035DDE6 File Offset: 0x0035BFE6
		private IEnumerator LayerFadeRoutine(int layerIndex, float targetWeight)
		{
			if (this.playerAnimator == null)
			{
				yield break;
			}
			float currentWeight = this.playerAnimator.GetLayerWeight(layerIndex);
			float speed = this.layerTransitionSpeed;
			while (!Mathf.Approximately(currentWeight, targetWeight))
			{
				currentWeight = Mathf.MoveTowards(currentWeight, targetWeight, Time.deltaTime * speed);
				this.playerAnimator.SetLayerWeight(layerIndex, currentWeight);
				yield return null;
			}
			this.playerAnimator.SetLayerWeight(layerIndex, targetWeight);
			yield break;
		}

		// Token: 0x0600840D RID: 33805 RVA: 0x0035DE04 File Offset: 0x0035C004
		public void DropItem(int index)
		{
			if (index < 0 || index >= this.maxSlots || this.slots[index] == null)
			{
				return;
			}
			int itemStackCount = this.GetItemStackCount(index);
			if (!this.slots[index].isStackable || itemStackCount <= 1)
			{
				InventoryItemSO inventoryItemSO = this.slots[index];
				Action<int, InventoryItemSO> onItemRemoving = this.OnItemRemoving;
				if (onItemRemoving != null)
				{
					onItemRemoving(index, inventoryItemSO);
				}
				bool flag = this.activeSlotIndex == index;
				if (flag)
				{
					this.UnequipCurrent(false);
				}
				this.slots[index] = null;
				this.stackCounts[index] = 0;
				this.itemRarities[index] = ItemRarity.Common;
				this.itemVariants[index] = -1;
				Action<int> onItemRemoved = this.OnItemRemoved;
				if (onItemRemoved != null)
				{
					onItemRemoved(index);
				}
				if (flag)
				{
					this.activeSlotIndex = -1;
					Action<int> onActiveSlotChanged = this.OnActiveSlotChanged;
					if (onActiveSlotChanged == null)
					{
						return;
					}
					onActiveSlotChanged(-1);
				}
				return;
			}
			this.stackCounts[index] = itemStackCount - 1;
			Action<int, int, int> onItemStackChanged = this.OnItemStackChanged;
			if (onItemStackChanged == null)
			{
				return;
			}
			onItemStackChanged(index, itemStackCount, this.stackCounts[index]);
		}

		// Token: 0x0600840E RID: 33806 RVA: 0x0035DF13 File Offset: 0x0035C113
		public void DropEntireItemStack(int index)
		{
			if (index < 0 || index >= this.maxSlots || this.slots[index] == null)
			{
				return;
			}
			this.stackCounts[index] = 1;
			this.DropItem(index);
		}

		// Token: 0x0600840F RID: 33807 RVA: 0x0035DF4C File Offset: 0x0035C14C
		public bool DropEntireActiveItemStack()
		{
			if (this.activeSlotIndex < 0)
			{
				return false;
			}
			int num = this.activeSlotIndex;
			this.DropEntireItemStack(num);
			return this.GetItemStackCount(num) < 1;
		}

		// Token: 0x06008410 RID: 33808 RVA: 0x0035DF7C File Offset: 0x0035C17C
		public void ClearRunInventory()
		{
			WearableUseController wearableUseController = CollarRunHandoff.ResolveWearController(this);
			for (int i = 0; i < this.MaxClothSlots; i++)
			{
				WearableItemSO wearableItemSO = this.GetClothAt(i) as WearableItemSO;
				if (wearableItemSO != null && wearableUseController != null)
				{
					wearableUseController.Unwear(wearableItemSO);
				}
				this.RemoveCloth(i);
			}
			for (int j = 0; j < this.MaxItemSlots; j++)
			{
				WearableItemSO wearableItemSO2 = this.GetItemAt(j) as WearableItemSO;
				if (wearableItemSO2 != null && wearableUseController != null)
				{
					wearableUseController.Unwear(wearableItemSO2);
				}
				this.DropEntireItemStack(j);
			}
		}

		// Token: 0x06008411 RID: 33809 RVA: 0x0035E004 File Offset: 0x0035C204
		public bool ConsumeActiveItem()
		{
			if (this.activeSlotIndex == -1)
			{
				return false;
			}
			int num = this.activeSlotIndex;
			this.DropItem(this.activeSlotIndex);
			return this.GetItemStackCount(num) < 1 || this.activeSlotIndex == num;
		}

		// Token: 0x06008412 RID: 33810 RVA: 0x0035E044 File Offset: 0x0035C244
		public InventoryItemSO GetActiveItem()
		{
			return this._activeItem;
		}

		// Token: 0x06008413 RID: 33811 RVA: 0x0035E04C File Offset: 0x0035C24C
		public bool RespawnActiveHandObject()
		{
			if (PoolWaterEntryPreparation.IsSwimming(base.transform))
			{
				return false;
			}
			if (PlayerActivityGate.IsResourceOwned(PlayerContextResource.Rider) && !this.CanUseInventoryOnRoof)
			{
				return false;
			}
			if (this.activeSlotIndex < 0 || this.activeSlotIndex >= this.slots.Count)
			{
				return false;
			}
			InventoryItemSO inventoryItemSO = this.slots[this.activeSlotIndex];
			if (inventoryItemSO == null)
			{
				return false;
			}
			this.EquipItem(inventoryItemSO);
			Action<int> onActiveSlotChanged = this.OnActiveSlotChanged;
			if (onActiveSlotChanged != null)
			{
				onActiveSlotChanged(this.activeSlotIndex);
			}
			Action<int, InventoryItemSO> onItemVisualChanged = this.OnItemVisualChanged;
			if (onItemVisualChanged != null)
			{
				onItemVisualChanged(this.activeSlotIndex, inventoryItemSO);
			}
			return this._spawnedHandObject != null;
		}

		// Token: 0x06008414 RID: 33812 RVA: 0x0035E0F8 File Offset: 0x0035C2F8
		public void NotifyActiveItemVisualChanged()
		{
			if (this.activeSlotIndex < 0 || this.activeSlotIndex >= this.slots.Count || this.slots[this.activeSlotIndex] == null)
			{
				return;
			}
			Action<int, InventoryItemSO> onItemVisualChanged = this.OnItemVisualChanged;
			if (onItemVisualChanged == null)
			{
				return;
			}
			onItemVisualChanged(this.activeSlotIndex, this.slots[this.activeSlotIndex]);
		}

		// Token: 0x06008415 RID: 33813 RVA: 0x0035E164 File Offset: 0x0035C364
		public void MoveItem(int from, int to)
		{
			if (from == to)
			{
				return;
			}
			if (from < 0 || from >= this.maxSlots || to < 0 || to >= this.maxSlots)
			{
				return;
			}
			if (this.slots[from] == null && this.slots[to] == null)
			{
				return;
			}
			List<InventoryItemSO> list = this.slots;
			List<InventoryItemSO> list2 = this.slots;
			InventoryItemSO inventoryItemSO = this.slots[to];
			InventoryItemSO inventoryItemSO2 = this.slots[from];
			list[from] = inventoryItemSO;
			list2[to] = inventoryItemSO2;
			List<float> list3 = this.durability;
			List<float> list4 = this.durability;
			float num = this.durability[to];
			float num2 = this.durability[from];
			list3[from] = num;
			list4[to] = num2;
			List<int> list5 = this.stackCounts;
			List<int> list6 = this.stackCounts;
			int num3 = this.stackCounts[to];
			int num4 = this.stackCounts[from];
			list5[from] = num3;
			list6[to] = num4;
			List<ItemRarity> list7 = this.itemRarities;
			List<ItemRarity> list8 = this.itemRarities;
			ItemRarity itemRarity = this.itemRarities[to];
			ItemRarity itemRarity2 = this.itemRarities[from];
			list7[from] = itemRarity;
			list8[to] = itemRarity2;
			List<int> list9 = this.itemVariants;
			list6 = this.itemVariants;
			num4 = this.itemVariants[to];
			num3 = this.itemVariants[from];
			list9[from] = num4;
			list6[to] = num3;
			if (this.activeSlotIndex == from)
			{
				this.activeSlotIndex = to;
			}
			else if (this.activeSlotIndex == to)
			{
				this.activeSlotIndex = from;
			}
			Action<bool, int, int> onSlotsMoved = this.OnSlotsMoved;
			if (onSlotsMoved != null)
			{
				onSlotsMoved(false, from, to);
			}
			Action<int, InventoryItemSO> onItemAdded = this.OnItemAdded;
			if (onItemAdded != null)
			{
				onItemAdded(from, this.slots[from]);
			}
			Action<int, InventoryItemSO> onItemAdded2 = this.OnItemAdded;
			if (onItemAdded2 != null)
			{
				onItemAdded2(to, this.slots[to]);
			}
			Action<int> onActiveSlotChanged = this.OnActiveSlotChanged;
			if (onActiveSlotChanged == null)
			{
				return;
			}
			onActiveSlotChanged(this.activeSlotIndex);
		}

		// Token: 0x06008416 RID: 33814 RVA: 0x0035E39C File Offset: 0x0035C59C
		public bool AddClothing(InventoryItemSO item)
		{
			return this.AddClothingReturnIndex(item, false) >= 0;
		}

		// Token: 0x06008417 RID: 33815 RVA: 0x0035E3AC File Offset: 0x0035C5AC
		public int AddClothingReturnIndex(InventoryItemSO item, bool allowDuplicate = false)
		{
			return this.AddClothingReturnIndexCore(item, allowDuplicate, (item != null) ? item.rarity : ItemRarity.Common, -1, 1f, false);
		}

		// Token: 0x06008418 RID: 33816 RVA: 0x0035E3CF File Offset: 0x0035C5CF
		public int AddMirroredClothingReturnIndex(InventoryItemSO item, int variantIndex)
		{
			return this.AddClothingReturnIndexCore(item, false, (item != null) ? item.rarity : ItemRarity.Common, variantIndex, 1f, false);
		}

		// Token: 0x06008419 RID: 33817 RVA: 0x0035E3F2 File Offset: 0x0035C5F2
		public int AddClothingReturnIndex(InventoryItemSO item, bool allowDuplicate, ItemRarity instanceRarity, int variantIndex, float durability01 = 1f, bool registerFind = true)
		{
			return this.AddClothingReturnIndexCore(item, allowDuplicate, instanceRarity, variantIndex, durability01, registerFind);
		}

		// Token: 0x0600841A RID: 33818 RVA: 0x0035E404 File Offset: 0x0035C604
		private int AddClothingReturnIndexCore(InventoryItemSO item, bool allowDuplicate, ItemRarity instanceRarity, int variantIndex, float durability01, bool registerFind)
		{
			if (item == null || item.isVaulted || !CollarAvailabilityPolicy.IsAvailable(item))
			{
				return -1;
			}
			if (!LongWayHomeLootPolicy.CanAddClothing(item, registerFind))
			{
				return -1;
			}
			if (!allowDuplicate)
			{
				for (int i = 0; i < this.clothSlots.Count; i++)
				{
					if (this.clothSlots[i] == item)
					{
						return i;
					}
				}
			}
			int num = -1;
			for (int j = 0; j < this.clothSlots.Count; j++)
			{
				if (this.clothSlots[j] == null)
				{
					num = j;
					break;
				}
			}
			if (num == -1)
			{
				ToastService.Show("No free clothing slots!", "Wardrobe", null, -1f, "inventory_feedback");
				return -1;
			}
			this.clothSlots[num] = item;
			this.clothRarities[num] = instanceRarity;
			this.clothVariants[num] = variantIndex;
			this.clothDurability[num] = this.GetClothDurabilityBase(num) * Mathf.Clamp01(durability01);
			if (registerFind)
			{
				CollectionUnlockRegistry.RegisterGameplayItemFound(item, instanceRarity, variantIndex, this);
			}
			Action<int, InventoryItemSO> onClothAdded = this.OnClothAdded;
			if (onClothAdded != null)
			{
				onClothAdded(num, item);
			}
			if (this.autoFocusFirstClothOnAdd && this.activeClothIndex == -1 && this._activeItem == null)
			{
				this.SelectCloth(num);
			}
			return num;
		}

		// Token: 0x0600841B RID: 33819 RVA: 0x0035E544 File Offset: 0x0035C744
		public void SelectCloth(int index)
		{
			if (ExhibitionEventDirector.Instance != null && ExhibitionEventDirector.Instance.IsEventActive)
			{
				return;
			}
			if (this.IsSlotInputLocked)
			{
				WearableItemSO wearableItemSO = this.GetActiveSlotItem() as WearableItemSO;
				if (wearableItemSO != null && wearableItemSO.wearOnlyWhileSelected)
				{
					return;
				}
			}
			if (index < 0 || index >= this.clothSlots.Count)
			{
				return;
			}
			if (this.clothSlots[index] == null)
			{
				this.ClearActiveCloth();
				return;
			}
			if (this.activeClothIndex == index)
			{
				this.ClearActiveCloth();
				return;
			}
			if (this.GetItemAt(this.activeSlotIndex) != null)
			{
				this._lastFocusedItemIndex = this.activeSlotIndex;
			}
			if (this.activeSlotIndex >= 0)
			{
				this.UnequipCurrent(false);
				this.activeSlotIndex = -1;
				Action<int> onActiveSlotChanged = this.OnActiveSlotChanged;
				if (onActiveSlotChanged != null)
				{
					onActiveSlotChanged(-1);
				}
			}
			this.activeClothIndex = index;
			this._activeCloth = this.clothSlots[index];
			this._lastFocusedClothIndex = index;
			Action<int> onActiveClothChanged = this.OnActiveClothChanged;
			if (onActiveClothChanged != null)
			{
				onActiveClothChanged(index);
			}
			this.PlayClothSwitchSfx();
		}

		// Token: 0x0600841C RID: 33820 RVA: 0x0035E64C File Offset: 0x0035C84C
		private void PlayClothSwitchSfx()
		{
			if (this.clothSwitchSFX == null || this.clothSwitchVolume <= 0f)
			{
				return;
			}
			if (this.clothAudioSource != null)
			{
				this.clothAudioSource.PlayOneShot(this.clothSwitchSFX, this.clothSwitchVolume);
				return;
			}
			AudioSource.PlayClipAtPoint(this.clothSwitchSFX, base.transform.position, this.clothSwitchVolume);
		}

		// Token: 0x0600841D RID: 33821 RVA: 0x0035E6B8 File Offset: 0x0035C8B8
		public void CycleCloth(int direction)
		{
			if (ExhibitionEventDirector.Instance != null && ExhibitionEventDirector.Instance.IsEventActive)
			{
				return;
			}
			if (direction == 0)
			{
				direction = 1;
			}
			int count = this.clothSlots.Count;
			if (count == 0)
			{
				return;
			}
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				if (this.clothSlots[i] != null)
				{
					num++;
				}
			}
			if (num == 0)
			{
				this.ClearActiveCloth();
				return;
			}
			int num2 = ((this.activeClothIndex >= 0) ? this.activeClothIndex : ((direction > 0) ? (-1) : count));
			int num3 = ((direction > 0) ? 1 : (-1));
			int j = 0;
			while (j < count)
			{
				num2 = ((num2 + num3) % count + count) % count;
				if (this.clothSlots[num2] != null)
				{
					if (num2 == this.activeClothIndex)
					{
						return;
					}
					this.SelectCloth(num2);
					return;
				}
				else
				{
					j++;
				}
			}
		}

		// Token: 0x0600841E RID: 33822 RVA: 0x0035E790 File Offset: 0x0035C990
		public void RemoveCloth(int index)
		{
			if (index < 0 || index >= this.clothSlots.Count || this.clothSlots[index] == null)
			{
				return;
			}
			InventoryItemSO inventoryItemSO = this.clothSlots[index];
			Action<int, InventoryItemSO> onClothRemoving = this.OnClothRemoving;
			if (onClothRemoving != null)
			{
				onClothRemoving(index, inventoryItemSO);
			}
			bool flag = this.activeClothIndex == index;
			this.clothSlots[index] = null;
			this.clothDurability[index] = 0f;
			this.clothRarities[index] = ItemRarity.Common;
			this.clothVariants[index] = -1;
			Action<int> onClothRemoved = this.OnClothRemoved;
			if (onClothRemoved != null)
			{
				onClothRemoved(index);
			}
			if (flag)
			{
				this.activeClothIndex = -1;
				this._activeCloth = null;
				Action<int> onActiveClothChanged = this.OnActiveClothChanged;
				if (onActiveClothChanged == null)
				{
					return;
				}
				onActiveClothChanged(-1);
			}
		}

		// Token: 0x0600841F RID: 33823 RVA: 0x0035E858 File Offset: 0x0035CA58
		public void DropCloth(int index)
		{
			this.RemoveCloth(index);
		}

		// Token: 0x06008420 RID: 33824 RVA: 0x0035E864 File Offset: 0x0035CA64
		public void MoveCloth(int from, int to)
		{
			if (from == to)
			{
				return;
			}
			if (from < 0 || from >= this.clothSlots.Count || to < 0 || to >= this.clothSlots.Count)
			{
				return;
			}
			if (this.clothSlots[from] == null && this.clothSlots[to] == null)
			{
				return;
			}
			List<InventoryItemSO> list = this.clothSlots;
			List<InventoryItemSO> list2 = this.clothSlots;
			InventoryItemSO inventoryItemSO = this.clothSlots[to];
			InventoryItemSO inventoryItemSO2 = this.clothSlots[from];
			list[from] = inventoryItemSO;
			list2[to] = inventoryItemSO2;
			List<float> list3 = this.clothDurability;
			List<float> list4 = this.clothDurability;
			float num = this.clothDurability[to];
			float num2 = this.clothDurability[from];
			list3[from] = num;
			list4[to] = num2;
			List<ItemRarity> list5 = this.clothRarities;
			List<ItemRarity> list6 = this.clothRarities;
			ItemRarity itemRarity = this.clothRarities[to];
			ItemRarity itemRarity2 = this.clothRarities[from];
			list5[from] = itemRarity;
			list6[to] = itemRarity2;
			List<int> list7 = this.clothVariants;
			List<int> list8 = this.clothVariants;
			int num3 = this.clothVariants[to];
			int num4 = this.clothVariants[from];
			list7[from] = num3;
			list8[to] = num4;
			if (this.activeClothIndex == from)
			{
				this.activeClothIndex = to;
			}
			else if (this.activeClothIndex == to)
			{
				this.activeClothIndex = from;
			}
			Action<bool, int, int> onSlotsMoved = this.OnSlotsMoved;
			if (onSlotsMoved != null)
			{
				onSlotsMoved(true, from, to);
			}
			Action<int, InventoryItemSO> onClothAdded = this.OnClothAdded;
			if (onClothAdded != null)
			{
				onClothAdded(from, this.clothSlots[from]);
			}
			Action<int, InventoryItemSO> onClothAdded2 = this.OnClothAdded;
			if (onClothAdded2 != null)
			{
				onClothAdded2(to, this.clothSlots[to]);
			}
			Action<int> onActiveClothChanged = this.OnActiveClothChanged;
			if (onActiveClothChanged == null)
			{
				return;
			}
			onActiveClothChanged(this.activeClothIndex);
		}

		// Token: 0x06008421 RID: 33825 RVA: 0x0035EA60 File Offset: 0x0035CC60
		private void ClearActiveCloth()
		{
			if (this.activeClothIndex == -1 && this._activeCloth == null)
			{
				return;
			}
			this.activeClothIndex = -1;
			this._activeCloth = null;
			Action<int> onActiveClothChanged = this.OnActiveClothChanged;
			if (onActiveClothChanged != null)
			{
				onActiveClothChanged(-1);
			}
			this.PlayClothSwitchSfx();
		}

		// Token: 0x06008422 RID: 33826 RVA: 0x0035EAA0 File Offset: 0x0035CCA0
		public void SetClothDurability(int index, float absoluteValue)
		{
			if (index < 0 || index >= this.clothDurability.Count)
			{
				return;
			}
			this.clothDurability[index] = absoluteValue;
			Action<int, float> onClothDurabilityChanged = this.OnClothDurabilityChanged;
			if (onClothDurabilityChanged == null)
			{
				return;
			}
			onClothDurabilityChanged(index, Mathf.Clamp01(absoluteValue / this.GetClothDurabilityBase(index)));
		}

		// Token: 0x06008423 RID: 33827 RVA: 0x0035EAEC File Offset: 0x0035CCEC
		public float GetClothDurability(int index)
		{
			if (index < 0 || index >= this.clothDurability.Count)
			{
				return 0f;
			}
			return this.clothDurability[index];
		}

		// Token: 0x06008424 RID: 33828 RVA: 0x0035EB14 File Offset: 0x0035CD14
		public bool HasClothDurabilityAt(int index, InventoryItemSO expectedItem, float amount = 1f)
		{
			return expectedItem != null && amount > 0f && index >= 0 && index < this.clothSlots.Count && index < this.clothDurability.Count && this.clothSlots[index] == expectedItem && expectedItem.hasDurability && expectedItem.maxDurability > 0f && this.clothDurability[index] + 0.0001f >= amount;
		}

		// Token: 0x06008425 RID: 33829 RVA: 0x0035EB90 File Offset: 0x0035CD90
		public bool TryConsumeClothDurabilityAt(int index, InventoryItemSO expectedItem, float amount, out float remaining)
		{
			remaining = 0f;
			if (!this.HasClothDurabilityAt(index, expectedItem, amount))
			{
				return false;
			}
			this.clothDurability[index] = Mathf.Max(0f, this.clothDurability[index] - amount);
			remaining = this.clothDurability[index];
			Action<int, float> onClothDurabilityChanged = this.OnClothDurabilityChanged;
			if (onClothDurabilityChanged != null)
			{
				onClothDurabilityChanged(index, Mathf.Clamp01(this.clothDurability[index] / this.GetClothDurabilityBase(index)));
			}
			if (this.clothDurability[index] <= 0f)
			{
				this.RemoveCloth(index);
			}
			return true;
		}

		// Token: 0x06008426 RID: 33830 RVA: 0x0035EC2C File Offset: 0x0035CE2C
		public ItemRarity GetItemRarityAt(int index)
		{
			if (index >= 0 && index < this.itemRarities.Count && index < this.slots.Count && this.slots[index] != null)
			{
				return this.itemRarities[index];
			}
			InventoryItemSO itemAt = this.GetItemAt(index);
			if (!(itemAt != null))
			{
				return ItemRarity.Common;
			}
			return itemAt.rarity;
		}

		// Token: 0x06008427 RID: 33831 RVA: 0x0035EC93 File Offset: 0x0035CE93
		public int GetItemVariantAt(int index)
		{
			if (index < 0 || index >= this.itemVariants.Count)
			{
				return -1;
			}
			return this.itemVariants[index];
		}

		// Token: 0x06008428 RID: 33832 RVA: 0x0035ECB8 File Offset: 0x0035CEB8
		public int GetItemStackCount(int index)
		{
			if (index < 0 || index >= this.slots.Count || this.slots[index] == null)
			{
				return 0;
			}
			if (index >= this.stackCounts.Count)
			{
				return 1;
			}
			return Mathf.Max(1, this.stackCounts[index]);
		}

		// Token: 0x06008429 RID: 33833 RVA: 0x0035ED10 File Offset: 0x0035CF10
		public ItemRarity GetClothRarityAt(int index)
		{
			if (index >= 0 && index < this.clothRarities.Count && index < this.clothSlots.Count && this.clothSlots[index] != null)
			{
				return this.clothRarities[index];
			}
			InventoryItemSO clothAt = this.GetClothAt(index);
			if (!(clothAt != null))
			{
				return ItemRarity.Common;
			}
			return clothAt.rarity;
		}

		// Token: 0x0600842A RID: 33834 RVA: 0x0035ED77 File Offset: 0x0035CF77
		public int GetClothVariantAt(int index)
		{
			if (index < 0 || index >= this.clothVariants.Count)
			{
				return -1;
			}
			return this.clothVariants[index];
		}

		// Token: 0x0600842B RID: 33835 RVA: 0x0035ED9C File Offset: 0x0035CF9C
		public bool SetClothInstanceLook(int index, ItemRarity instanceRarity, int variantIndex)
		{
			if (index < 0 || index >= this.clothSlots.Count || this.clothSlots[index] == null || index >= this.clothRarities.Count || index >= this.clothVariants.Count || index >= this.clothDurability.Count)
			{
				return false;
			}
			float num = Mathf.Max(0.0001f, this.GetClothDurabilityBase(index));
			float num2 = Mathf.Clamp01(this.clothDurability[index] / num);
			this.clothRarities[index] = instanceRarity;
			this.clothVariants[index] = variantIndex;
			this.clothDurability[index] = this.GetClothDurabilityBase(index) * num2;
			Action<int, InventoryItemSO> onClothAdded = this.OnClothAdded;
			if (onClothAdded != null)
			{
				onClothAdded(index, this.clothSlots[index]);
			}
			Action<int, float> onClothDurabilityChanged = this.OnClothDurabilityChanged;
			if (onClothDurabilityChanged != null)
			{
				onClothDurabilityChanged(index, num2);
			}
			return true;
		}

		// Token: 0x0600842C RID: 33836 RVA: 0x0035EE84 File Offset: 0x0035D084
		public float GetClothWearDurationScaled(int index)
		{
			WearableItemSO wearableItemSO = this.GetClothAt(index) as WearableItemSO;
			if (wearableItemSO == null || !wearableItemSO.hasWearDuration || wearableItemSO.wearDuration <= 0f)
			{
				return 0f;
			}
			RaritySettingsSO instance = RaritySettingsSO.Instance;
			float num = ((instance != null) ? instance.GetDurationMultiplier(wearableItemSO.rarity, this.GetClothRarityAt(index)) : 1f);
			return wearableItemSO.wearDuration * num;
		}

		// Token: 0x0600842D RID: 33837 RVA: 0x0035EEF4 File Offset: 0x0035D0F4
		public float GetClothDurabilityBase(int index)
		{
			InventoryItemSO inventoryItemSO = ((index >= 0 && index < this.clothSlots.Count) ? this.clothSlots[index] : null);
			WearableItemSO wearableItemSO = inventoryItemSO as WearableItemSO;
			if (wearableItemSO != null && wearableItemSO.hasWearDuration && wearableItemSO.wearDuration > 0f)
			{
				return this.GetClothWearDurationScaled(index);
			}
			if (!(inventoryItemSO != null) || inventoryItemSO.maxDurability <= 0f)
			{
				return 1f;
			}
			return inventoryItemSO.maxDurability;
		}

		// Token: 0x0600842E RID: 33838 RVA: 0x0035EF6C File Offset: 0x0035D16C
		public void PlayPickupAnimation(InventoryItemSO item, Transform targetItem, Action onComplete = null)
		{
			this.TryPickupWithAnimation(item, targetItem, delegate
			{
				Action onComplete2 = onComplete;
				if (onComplete2 != null)
				{
					onComplete2();
				}
				return true;
			});
		}

		// Token: 0x0600842F RID: 33839 RVA: 0x0035EF9C File Offset: 0x0035D19C
		public void TryPickupWithAnimation(InventoryItemSO item, Transform targetItem, Func<bool> tryPickup)
		{
			if (tryPickup == null)
			{
				throw new ArgumentNullException("tryPickup");
			}
			PlayerActionEntry playerActionEntry = ((item != null && item.pickupAnimationOverride != null) ? item.pickupAnimationOverride : this.defaultPickupAction);
			if (this.actionAnimator != null && playerActionEntry != null && targetItem != null)
			{
				Vector3 position = targetItem.position;
				Vector3 localScale = targetItem.localScale;
				Behaviour behaviour = targetItem.GetComponent<InventoryPickup>();
				if (behaviour == null)
				{
					behaviour = targetItem.GetComponent<WearablePickup>();
				}
				bool flag = behaviour != null && behaviour.enabled;
				if (behaviour != null)
				{
					behaviour.enabled = false;
				}
				InteractablePromptUI componentInChildren = targetItem.GetComponentInChildren<InteractablePromptUI>(true);
				if (componentInChildren != null)
				{
					componentInChildren.Hide();
				}
				float num = this.actionAnimator.TriggerAction(playerActionEntry);
				vGenericAction genericAction = this.actionAnimator.GenericAction;
				vTriggerGenericAction vTriggerGenericAction = ((genericAction != null && genericAction.doingAction) ? genericAction.triggerAction : null);
				Coroutine coroutine = base.StartCoroutine(this.PickupMicroInteraction(targetItem, 0.1f));
				base.StartCoroutine(this.WaitAndTryPickup(Mathf.Min(num * 0.3f, 0.15f), tryPickup, targetItem, position, localScale, behaviour, flag, coroutine));
				base.StartCoroutine(this.WaitAndFinishPickupAction(num, vTriggerGenericAction));
				return;
			}
			tryPickup();
		}

		// Token: 0x06008430 RID: 33840 RVA: 0x0035F0EC File Offset: 0x0035D2EC
		private IEnumerator PickupMicroInteraction(Transform item, float duration)
		{
			if (item == null)
			{
				yield break;
			}
			Vector3 startScale = item.localScale;
			Vector3 startPos = item.position;
			float elapsed = 0f;
			while (elapsed < duration && item != null)
			{
				elapsed += Time.deltaTime;
				float num = elapsed / duration;
				item.localScale = Vector3.Lerp(startScale, Vector3.zero, num);
				Vector3 vector = base.transform.position + Vector3.up * 1.2f;
				item.position = Vector3.Lerp(startPos, vector, num);
				yield return null;
			}
			yield break;
		}

		// Token: 0x06008431 RID: 33841 RVA: 0x0035F10C File Offset: 0x0035D30C
		private IEnumerator WaitAndTryPickup(float delay, Func<bool> tryPickup, Transform targetItem, Vector3 originalPosition, Vector3 originalScale, Behaviour pickup, bool wasEnabled, Coroutine microInteraction)
		{
			yield return new WaitForSeconds(delay);
			if (tryPickup())
			{
				yield break;
			}
			if (microInteraction != null)
			{
				base.StopCoroutine(microInteraction);
			}
			if (targetItem != null)
			{
				targetItem.position = originalPosition;
				targetItem.localScale = originalScale;
			}
			if (pickup != null)
			{
				pickup.enabled = wasEnabled;
			}
			yield break;
		}

		// Token: 0x06008432 RID: 33842 RVA: 0x0035F163 File Offset: 0x0035D363
		private IEnumerator WaitAndFinishPickupAction(float duration, vTriggerGenericAction pickupTrigger)
		{
			yield return new WaitForSeconds(duration);
			if (this.actionAnimator == null)
			{
				yield break;
			}
			vGenericAction ga = this.actionAnimator.GenericAction;
			if (ga == null || ((pickupTrigger != null) ? (ga.doingAction && ga.triggerAction == pickupTrigger) : (!ga.doingAction)))
			{
				this.actionAnimator.FinishCurrentAction();
			}
			else if (pickupTrigger != null && ga != null && ga.doingAction)
			{
				float grace = 1.5f;
				float t = 0f;
				bool sawAnimation = false;
				while (t < grace && ga != null && ga.doingAction)
				{
					if (ga.playingAnimation)
					{
						sawAnimation = true;
						break;
					}
					t += Time.deltaTime;
					yield return null;
				}
				if (!sawAnimation && ga != null && ga.doingAction)
				{
					Debug.LogWarning("[PlayerInventoryManager] Azione fantasma post-pickup (doingAction senza animazione, trigger rieletto) - FinishAction di recupero.");
					this.actionAnimator.FinishCurrentAction();
				}
			}
			this.actionAnimator.RestoreOverrideLayer();
			yield break;
		}

		// Token: 0x06008433 RID: 33843 RVA: 0x0035F180 File Offset: 0x0035D380
		private static bool HasAnimatorParameter(Animator animator, string paramName)
		{
			if (animator == null || string.IsNullOrEmpty(paramName))
			{
				return false;
			}
			AnimatorControllerParameter[] parameters = animator.parameters;
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].name == paramName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040086BE RID: 34494
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x040086BF RID: 34495
		[Header("Settings")]
		[SerializeField]
		private int maxSlots = 3;

		// Token: 0x040086C0 RID: 34496
		[Tooltip("Number of clothing slots. Expandable for future modes.")]
		[SerializeField]
		private int maxClothSlots = 6;

		// Token: 0x040086C1 RID: 34497
		[SerializeField]
		private Animator playerAnimator;

		// Token: 0x040086C2 RID: 34498
		[SerializeField]
		private PlayerActionAnimator actionAnimator;

		// Token: 0x040086C3 RID: 34499
		[SerializeField]
		private CoverPoseController coverController;

		// Token: 0x040086C4 RID: 34500
		[Header("Input Configuration")]
		[Tooltip("Input for picking up items (default: F)")]
		public GenericInput pickupInput = new GenericInput("F", "X", "X");

		// Token: 0x040086C5 RID: 34501
		[SerializeField]
		private GenericInput slot1Input = new GenericInput("Alpha1", "", "");

		// Token: 0x040086C6 RID: 34502
		[SerializeField]
		private GenericInput slot2Input = new GenericInput("Alpha2", "", "");

		// Token: 0x040086C7 RID: 34503
		[SerializeField]
		private GenericInput slot3Input = new GenericInput("Alpha3", "", "");

		// Token: 0x040086C8 RID: 34504
		[Tooltip("Activate/deactivate the interactable cloth slot (like 1/2/3 for items). If no cloth is focused, focuses the first occupied one; pressing again clears focus.")]
		[SerializeField]
		private GenericInput clothSelectInput = new GenericInput("Alpha4", "", "");

		// Token: 0x040086C9 RID: 34505
		[Tooltip("Cycle the interactable cloth slot forward, skipping empty slots.")]
		[SerializeField]
		private GenericInput clothCycleInput = new GenericInput("Space", "", "");

		// Token: 0x040086CA RID: 34506
		[Header("Animations")]
		[SerializeField]
		private PlayerActionEntry defaultPickupAction;

		// Token: 0x040086CB RID: 34507
		[SerializeField]
		private string holdLayerName = "HoldingItem";

		// Token: 0x040086CC RID: 34508
		[SerializeField]
		private string armsLayerName = "Arms";

		// Token: 0x040086CD RID: 34509
		[SerializeField]
		private float layerTransitionSpeed = 10f;

		// Token: 0x040086CE RID: 34510
		[Header("Data - Items (hotbar 1/2/3)")]
		[SerializeField]
		private List<InventoryItemSO> slots = new List<InventoryItemSO>();

		// Token: 0x040086CF RID: 34511
		[SerializeField]
		private List<float> durability = new List<float>();

		// Token: 0x040086D0 RID: 34512
		[SerializeField]
		private List<int> stackCounts = new List<int>();

		// Token: 0x040086D1 RID: 34513
		[SerializeField]
		private List<ItemRarity> itemRarities = new List<ItemRarity>();

		// Token: 0x040086D2 RID: 34514
		[SerializeField]
		private List<int> itemVariants = new List<int>();

		// Token: 0x040086D3 RID: 34515
		[SerializeField]
		private int activeSlotIndex = -1;

		// Token: 0x040086D4 RID: 34516
		private int _lastFocusedItemIndex = -1;

		// Token: 0x040086D5 RID: 34517
		[Header("Data - Clothing (cloth slots)")]
		[SerializeField]
		private List<InventoryItemSO> clothSlots = new List<InventoryItemSO>();

		// Token: 0x040086D6 RID: 34518
		[SerializeField]
		private List<float> clothDurability = new List<float>();

		// Token: 0x040086D7 RID: 34519
		[SerializeField]
		private List<ItemRarity> clothRarities = new List<ItemRarity>();

		// Token: 0x040086D8 RID: 34520
		[SerializeField]
		private List<int> clothVariants = new List<int>();

		// Token: 0x040086D9 RID: 34521
		[SerializeField]
		private int activeClothIndex = -1;

		// Token: 0x040086DA RID: 34522
		[Tooltip("If true, adding a cloth while nothing is focused auto-focuses it (turns the cloth interactable slot ON). Default OFF so the cloth interactable starts disabled at match start - the player presses 4 to engage it.")]
		[SerializeField]
		private bool autoFocusFirstClothOnAdd;

		// Token: 0x040086DB RID: 34523
		[Header("Cloth Focus Audio")]
		[Tooltip("AudioSource used for cloth-focus switch SFX. Falls back to a temporary point-play if null.")]
		[SerializeField]
		private AudioSource clothAudioSource;

		// Token: 0x040086DC RID: 34524
		[Tooltip("Played when the focused (interactable) cloth slot changes via 4 / Space / cycling.")]
		[SerializeField]
		private AudioClip clothSwitchSFX;

		// Token: 0x040086DD RID: 34525
		[Range(0f, 1f)]
		[SerializeField]
		private float clothSwitchVolume = 1f;

		// Token: 0x040086EB RID: 34539
		private GameObject _spawnedHandObject;

		// Token: 0x040086EC RID: 34540
		private InventoryItemSO _activeItem;

		// Token: 0x040086ED RID: 34541
		private HideFlashController _timedCoverOwner;

		// Token: 0x040086EE RID: 34542
		private WearableUseController _wearableUseController;

		// Token: 0x040086EF RID: 34543
		private int _holdLayerIndex = -1;

		// Token: 0x040086F0 RID: 34544
		private int _armsLayerIndex = -1;

		// Token: 0x040086F1 RID: 34545
		private Coroutine _layerFadeRoutine;

		// Token: 0x040086F2 RID: 34546
		private InventoryItemSO _activeCloth;

		// Token: 0x040086F3 RID: 34547
		private int _lastFocusedClothIndex = -1;

		// Token: 0x040086F4 RID: 34548
		private bool _legacySlotInputLocked;

		// Token: 0x040086F5 RID: 34549
		private readonly HashSet<int> _slotInputLockOwners = new HashSet<int>();

		// Token: 0x040086F6 RID: 34550
		private static readonly GenericInput ControllerCancelSelectionInput = new GenericInput("None", "B", "");

		// Token: 0x040086F7 RID: 34551
		private static readonly GenericInput ControllerPreviousSlotInput = new GenericInput("None", "DPadLeft", "");

		// Token: 0x040086F8 RID: 34552
		private static readonly GenericInput ControllerNextSlotInput = new GenericInput("None", "DPadRight", "");
	}
}
