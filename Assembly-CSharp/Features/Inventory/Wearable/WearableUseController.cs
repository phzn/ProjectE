using System;
using System.Collections;
using System.Collections.Generic;
using BoZo.CharacterCreator;
using BoZo.CharacterCreator.MC;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.CielOS;
using Features.DebugTools;
using Features.Localization;
using Features.Missions;
using Features.Rank;
using Features.RunModes;
using Features.TimeFreeze;
using Features.Vehicles.Traffic;
using ProjectExhibitionist;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.CoverWall;
using ProjectExhibitionist.Interactions;
using ProjectExhibitionist.UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Features.Inventory.Wearable
{
	// Token: 0x0200112C RID: 4396
	public class WearableUseController : MonoBehaviour
	{
		// Token: 0x140000F6 RID: 246
		// (add) Token: 0x0600851E RID: 34078 RVA: 0x00364500 File Offset: 0x00362700
		// (remove) Token: 0x0600851F RID: 34079 RVA: 0x00364538 File Offset: 0x00362738
		public event Action<WearableItemSO> OnItemWorn;

		// Token: 0x140000F7 RID: 247
		// (add) Token: 0x06008520 RID: 34080 RVA: 0x00364570 File Offset: 0x00362770
		// (remove) Token: 0x06008521 RID: 34081 RVA: 0x003645A8 File Offset: 0x003627A8
		public event Action<WearableItemSO> OnItemUnworn;

		// Token: 0x17001522 RID: 5410
		// (get) Token: 0x06008522 RID: 34082 RVA: 0x003645E0 File Offset: 0x003627E0
		private string ArmActionsLockReason
		{
			get
			{
				string text;
				if ((text = this._armActionsLockReason) == null)
				{
					text = (this._armActionsLockReason = string.Format("WearableArmActions:{0}", base.GetInstanceID()));
				}
				return text;
			}
		}

		// Token: 0x17001523 RID: 5411
		// (get) Token: 0x06008523 RID: 34083 RVA: 0x00364618 File Offset: 0x00362818
		private BlendShapeManager RigBlendShapeManager
		{
			get
			{
				if (!(this._rigBlendShapeManager != null))
				{
					return this._rigBlendShapeManager = BlendShapeManager.ResolveForRig(this);
				}
				return this._rigBlendShapeManager;
			}
		}

		// Token: 0x06008524 RID: 34084 RVA: 0x0036464C File Offset: 0x0036284C
		private void Awake()
		{
			if (this.inventory == null)
			{
				this.inventory = base.GetComponent<PlayerInventoryManager>();
			}
			if (this.clothingController == null)
			{
				this.clothingController = base.GetComponent<ClothingToggleController>();
			}
			if (this.styleService == null)
			{
				this.styleService = base.GetComponentInChildren<MCStyleCustomizationService>(true);
			}
			if (this.styleService == null)
			{
				this.styleService = Object.FindObjectOfType<MCStyleCustomizationService>(true);
			}
		}

		// Token: 0x06008525 RID: 34085 RVA: 0x003646C4 File Offset: 0x003628C4
		private bool EnsureVignetteResolved()
		{
			if (this._vignette != null)
			{
				return true;
			}
			if (this.postProcessVolume != null && this.postProcessVolume.profile != null)
			{
				this.postProcessVolume.profile.TryGet<Vignette>(out this._vignette);
				if (this._vignette != null)
				{
					return true;
				}
			}
			foreach (Volume volume in Object.FindObjectsOfType<Volume>(true))
			{
				Vignette vignette;
				if (!(volume == null) && volume.isGlobal && volume.isActiveAndEnabled && !(volume.profile == null) && volume.profile.TryGet<Vignette>(out vignette) && vignette != null)
				{
					this.postProcessVolume = volume;
					this._vignette = vignette;
					return true;
				}
			}
			if (this.debugLogs)
			{
				Debug.LogWarning("[WearableUseController] No loaded Volume with a Vignette override found. Per-item vignette will have no effect.");
			}
			return false;
		}

		// Token: 0x06008526 RID: 34086 RVA: 0x003647A4 File Offset: 0x003629A4
		private void OnEnable()
		{
			if (this.inventory != null)
			{
				this.inventory.OnActiveSlotChanged += this.HandleActiveSlotChanged;
				this.inventory.OnItemRemoving += this.HandleItemRemoving;
				this.inventory.OnItemRemoved += this.HandleItemRemoved;
				this.inventory.OnDurabilityChanged += this.HandleItemDurabilityChanged;
				this.inventory.OnActiveClothChanged += this.HandleActiveClothChanged;
				this.inventory.OnClothRemoving += this.HandleClothRemoving;
				this.inventory.OnClothRemoved += this.HandleClothRemoved;
				this.inventory.OnClothDurabilityChanged += this.HandleClothDurabilityChanged;
				this.inventory.OnSlotsMoved += this.HandleSlotsMoved;
			}
			if (this.clothingController != null)
			{
				this.clothingController.OnSlotVisibilityChanged += this.HandleSlotVisibilityChangedExternally;
			}
			if (this.styleService != null)
			{
				this.styleService.OnActiveStateChanged += this.HandleGameplaySurfaceChanged;
			}
			this.HandleGameplaySurfaceChanged();
			this.RefreshWornActivityGate();
			PlayerInventoryManager playerInventoryManager = this.inventory;
			if (playerInventoryManager != null)
			{
				playerInventoryManager.ReleaseSlotInputLock(this);
			}
			foreach (WearableUseController.WornEntry wornEntry in this._worn.Values)
			{
				this.CacheOutfitMaterials(wornEntry);
				this.SettleSecondaryEffects(wornEntry);
			}
			this.RecomputeVignette();
		}

		// Token: 0x06008527 RID: 34087 RVA: 0x00364954 File Offset: 0x00362B54
		private void OnDisable()
		{
			foreach (WearableUseController.WornEntry wornEntry in this._worn.Values)
			{
				this.SettleSecondaryEffects(wornEntry);
				this.ReleaseAnimatedMainOverrides(wornEntry.item);
			}
			PlayerInventoryManager playerInventoryManager = this.inventory;
			if (playerInventoryManager != null)
			{
				playerInventoryManager.ReleaseSlotInputLock(this);
			}
			this.ReleaseVignette();
			this.HideActionsHud();
			if (this.inventory != null)
			{
				this.inventory.OnActiveSlotChanged -= this.HandleActiveSlotChanged;
				this.inventory.OnItemRemoving -= this.HandleItemRemoving;
				this.inventory.OnItemRemoved -= this.HandleItemRemoved;
				this.inventory.OnDurabilityChanged -= this.HandleItemDurabilityChanged;
				this.inventory.OnActiveClothChanged -= this.HandleActiveClothChanged;
				this.inventory.OnClothRemoving -= this.HandleClothRemoving;
				this.inventory.OnClothRemoved -= this.HandleClothRemoved;
				this.inventory.OnClothDurabilityChanged -= this.HandleClothDurabilityChanged;
				this.inventory.OnSlotsMoved -= this.HandleSlotsMoved;
			}
			if (this.clothingController != null)
			{
				this.clothingController.OnSlotVisibilityChanged -= this.HandleSlotVisibilityChangedExternally;
			}
			if (this.styleService != null)
			{
				this.styleService.OnActiveStateChanged -= this.HandleGameplaySurfaceChanged;
			}
			PlayerActivityGate.Pop(this.ArmActionsLockReason);
			this._appliedWornBlockedActivities = PlayerActivity.None;
		}

		// Token: 0x06008528 RID: 34088 RVA: 0x00364B14 File Offset: 0x00362D14
		private void ReleaseVignette()
		{
			if (this._vignetteRoutine != null)
			{
				base.StopCoroutine(this._vignetteRoutine);
			}
			this._vignetteRoutine = null;
			this._wearableVignetteIntensity = 0f;
			GameplayVignetteMixer.Release(this._vignette, this);
		}

		// Token: 0x06008529 RID: 34089 RVA: 0x00364B48 File Offset: 0x00362D48
		private void HandleGameplaySurfaceChanged()
		{
			if (this._applyingWearVisual || this.styleService == null)
			{
				return;
			}
			List<WearableItemSO> list = null;
			foreach (KeyValuePair<WearableItemSO, WearableUseController.WornEntry> keyValuePair in this._worn)
			{
				WearableUseController.WornEntry value = keyValuePair.Value;
				if (keyValuePair.Key != null && ((WearableUseController.UsesExclusiveCosmeticSlot(keyValuePair.Key) && !this.ActivePresetMatches(keyValuePair.Key)) || (keyValuePair.Key.HasGameplaySurfaceOverride && this.ActivePresetMatches(keyValuePair.Key) && !this.ActiveGameplaySurfaceMatches(keyValuePair.Key, value.source, value.slotIndex))))
				{
					List<WearableItemSO> list2;
					if ((list2 = list) == null)
					{
						list2 = (list = new List<WearableItemSO>());
					}
					list2.Add(keyValuePair.Key);
				}
			}
			if (list != null)
			{
				foreach (WearableItemSO wearableItemSO in list)
				{
					this.Unwear(wearableItemSO);
				}
			}
			list = null;
			foreach (KeyValuePair<WearableItemSO, WearableUseController.WornEntry> keyValuePair2 in this._externallyHiddenWorn)
			{
				WearableUseController.WornEntry value2 = keyValuePair2.Value;
				if (keyValuePair2.Key != null && ((WearableUseController.UsesExclusiveCosmeticSlot(keyValuePair2.Key) && !this.ActivePresetMatches(keyValuePair2.Key)) || (keyValuePair2.Key.HasGameplaySurfaceOverride && this.ActivePresetMatches(keyValuePair2.Key) && !this.ActiveGameplaySurfaceMatches(keyValuePair2.Key, value2.source, value2.slotIndex))))
				{
					List<WearableItemSO> list3;
					if ((list3 = list) == null)
					{
						list3 = (list = new List<WearableItemSO>());
					}
					list3.Add(keyValuePair2.Key);
				}
			}
			if (list != null)
			{
				foreach (WearableItemSO wearableItemSO2 in list)
				{
					this._externallyHiddenWorn.Remove(wearableItemSO2);
				}
			}
		}

		// Token: 0x0600852A RID: 34090 RVA: 0x00364D90 File Offset: 0x00362F90
		private static bool UsesExclusiveCosmeticSlot(WearableItemSO item)
		{
			return item != null && (CollarAvailabilityPolicy.IsCollar(item) || string.Equals(item.mcStylePartId, "Accessory.Eyewear", StringComparison.OrdinalIgnoreCase));
		}

		// Token: 0x0600852B RID: 34091 RVA: 0x00364DB8 File Offset: 0x00362FB8
		private void HandleSlotVisibilityChangedExternally(string partId, bool visible)
		{
			if (this._applyingWearVisual)
			{
				return;
			}
			if (string.IsNullOrEmpty(partId))
			{
				return;
			}
			if (!visible)
			{
				KeyValuePair<WearableItemSO, WearableUseController.WornEntry> keyValuePair = this.FindWornEntryByPartId(partId);
				if (keyValuePair.Key == null)
				{
					return;
				}
				this.ReleaseMainOverrides(keyValuePair.Key);
				this.ReleaseAllSecondaryOverrides(keyValuePair.Key, keyValuePair.Value);
				this._worn.Remove(keyValuePair.Key);
				this._externallyHiddenWorn[keyValuePair.Key] = keyValuePair.Value;
				this.RefreshWornActivityGate();
				if (this.debugLogs)
				{
					Debug.Log(string.Concat(new string[]
					{
						"[WearableUse] External HIDE slot '",
						partId,
						"' → unwear '",
						keyValuePair.Key.name,
						"' (emit OnItemUnworn)"
					}), this);
				}
				Action<WearableItemSO> onItemUnworn = this.OnItemUnworn;
				if (onItemUnworn == null)
				{
					return;
				}
				onItemUnworn(keyValuePair.Key);
				return;
			}
			else
			{
				KeyValuePair<WearableItemSO, WearableUseController.WornEntry> keyValuePair2 = this.FindHiddenEntryByPartId(partId);
				if (keyValuePair2.Key == null)
				{
					return;
				}
				this._externallyHiddenWorn.Remove(keyValuePair2.Key);
				if (!this.IsWorn(keyValuePair2.Key))
				{
					this._worn[keyValuePair2.Key] = keyValuePair2.Value;
					this.RefreshBlendShapeTargetsIfNeeded(keyValuePair2.Key);
					this.RefreshWornActivityGate();
					if (this.debugLogs)
					{
						Debug.Log(string.Concat(new string[]
						{
							"[WearableUse] External SHOW slot '",
							partId,
							"' → re-wear '",
							keyValuePair2.Key.name,
							"' (emit OnItemWorn)"
						}), this);
					}
					Action<WearableItemSO> onItemWorn = this.OnItemWorn;
					if (onItemWorn == null)
					{
						return;
					}
					onItemWorn(keyValuePair2.Key);
				}
				return;
			}
		}

		// Token: 0x0600852C RID: 34092 RVA: 0x00364F68 File Offset: 0x00363168
		private KeyValuePair<WearableItemSO, WearableUseController.WornEntry> FindWornEntryByPartId(string partId)
		{
			foreach (KeyValuePair<WearableItemSO, WearableUseController.WornEntry> keyValuePair in this._worn)
			{
				if (keyValuePair.Key != null && keyValuePair.Key.mcStylePartId == partId)
				{
					return keyValuePair;
				}
			}
			return default(KeyValuePair<WearableItemSO, WearableUseController.WornEntry>);
		}

		// Token: 0x0600852D RID: 34093 RVA: 0x00364FE8 File Offset: 0x003631E8
		private KeyValuePair<WearableItemSO, WearableUseController.WornEntry> FindHiddenEntryByPartId(string partId)
		{
			foreach (KeyValuePair<WearableItemSO, WearableUseController.WornEntry> keyValuePair in this._externallyHiddenWorn)
			{
				if (keyValuePair.Key != null && keyValuePair.Key.mcStylePartId == partId)
				{
					return keyValuePair;
				}
			}
			return default(KeyValuePair<WearableItemSO, WearableUseController.WornEntry>);
		}

		// Token: 0x0600852E RID: 34094 RVA: 0x00365068 File Offset: 0x00363268
		private void OnDestroy()
		{
			PlayerInventoryManager playerInventoryManager = this.inventory;
			if (playerInventoryManager != null)
			{
				playerInventoryManager.ReleaseSlotInputLock(this);
			}
			this.ReleaseVignette();
			PlayerActivityGate.Pop(this.ArmActionsLockReason);
			this._appliedWornBlockedActivities = PlayerActivity.None;
			if (this._runtimeHudConfig == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(this._runtimeHudConfig);
				return;
			}
			Object.DestroyImmediate(this._runtimeHudConfig);
		}

		// Token: 0x0600852F RID: 34095 RVA: 0x003650CC File Offset: 0x003632CC
		private void Update()
		{
			if (FlyCamera.IsActive)
			{
				if (this._activeWearable != null && this._timeFreezeRuntime != null && this._timeFreezeRuntime.IsActiveFor(this._activeWearable))
				{
					for (int i = 0; i < this._activeWearable.wearActions.Count; i++)
					{
						WearableActionDefinition wearableActionDefinition = this._activeWearable.wearActions[i];
						if (!(wearableActionDefinition == null) && wearableActionDefinition.ActionType == WearableActionType.TimeFreeze && wearableActionDefinition.Input.GetButtonDown())
						{
							this.ExecuteAction(wearableActionDefinition);
						}
					}
				}
				return;
			}
			this.TickWearTimers();
			if (this._activeWearable == null || this._activeWearable.wearOnlyWhileSelected)
			{
				return;
			}
			bool flag = this.IsFocusedInstanceWorn();
			for (int j = 0; j < this._activeWearable.wearActions.Count; j++)
			{
				WearableActionDefinition wearableActionDefinition2 = this._activeWearable.wearActions[j];
				if (!(wearableActionDefinition2 == null) && this.IsActionVisible(wearableActionDefinition2, flag) && wearableActionDefinition2.Input.GetButtonDown())
				{
					this.ExecuteAction(wearableActionDefinition2);
				}
			}
		}

		// Token: 0x06008530 RID: 34096 RVA: 0x003651E8 File Offset: 0x003633E8
		private void LateUpdate()
		{
			this.MaintainFocusedActionsHud();
			this.RefreshWornActivityGate();
			if (this.RigBlendShapeManager == null)
			{
				return;
			}
			foreach (KeyValuePair<WearableItemSO, WearableUseController.WornEntry> keyValuePair in this._worn)
			{
				WearableUseController.WornEntry value = keyValuePair.Value;
				this.ApplyMainBlendshapesToManager(value.item);
				foreach (KeyValuePair<string, WearableUseController.SecondaryEffectRuntime> keyValuePair2 in value.secondaryRuntimes)
				{
					WearableUseController.SecondaryEffectRuntime value2 = keyValuePair2.Value;
					if (value2.progress > 0f || value2.isActive)
					{
						this.ApplySecondaryBlendshapesToManager(value.item, value2);
					}
				}
			}
		}

		// Token: 0x06008531 RID: 34097 RVA: 0x003652D0 File Offset: 0x003634D0
		private void HandleActiveSlotChanged(int slotIndex)
		{
			WearableItemSO wearableItemSO = ((this.inventory != null) ? this.inventory.GetActiveSlotItem() : null) as WearableItemSO;
			if (this._activeWearable != null && this._activeWearable.wearOnlyWhileSelected && (this._activeWearable != wearableItemSO || this._activeSlotIndex != slotIndex))
			{
				this.Unwear(this._activeWearable);
			}
			if (wearableItemSO == null && this._activeSource == WearableUseController.WearableSource.Cloth)
			{
				this.ClearFocus();
				return;
			}
			this._activeSlotIndex = slotIndex;
			this._activeWearable = wearableItemSO;
			this._activeSource = ((wearableItemSO != null) ? WearableUseController.WearableSource.Item : WearableUseController.WearableSource.None);
			if (wearableItemSO != null)
			{
				this.ReconcileVisibleWearable(wearableItemSO, WearableUseController.WearableSource.Item, slotIndex);
				if (wearableItemSO.wearOnSelection && !this.IsWorn(wearableItemSO) && wearableItemSO.wearActions != null)
				{
					foreach (WearableActionDefinition wearableActionDefinition in wearableItemSO.wearActions)
					{
						if (wearableActionDefinition != null && (wearableActionDefinition.ActionType == WearableActionType.Wear || wearableActionDefinition.ActionType == WearableActionType.ToggleWear) && this.CanUseFocusedAction(wearableActionDefinition))
						{
							this.Wear(wearableItemSO);
							break;
						}
					}
				}
				this.RefreshHud();
				if (this.IsInstanceWorn(wearableItemSO, WearableUseController.WearableSource.Item, slotIndex))
				{
					this.inventory.SuspendActiveItem();
					return;
				}
			}
			else
			{
				this.HideActionsHud();
			}
		}

		// Token: 0x06008532 RID: 34098 RVA: 0x00365434 File Offset: 0x00363634
		private void HandleActiveClothChanged(int clothIndex)
		{
			WearableItemSO wearableItemSO = ((this.inventory != null) ? (this.inventory.GetActiveCloth() as WearableItemSO) : null);
			if (wearableItemSO == null)
			{
				if (this._activeSource == WearableUseController.WearableSource.Cloth)
				{
					this.ClearFocus();
				}
				return;
			}
			this._activeSlotIndex = clothIndex;
			this._activeWearable = wearableItemSO;
			this._activeSource = WearableUseController.WearableSource.Cloth;
			this.ReconcileVisibleWearable(wearableItemSO, WearableUseController.WearableSource.Cloth, clothIndex);
			this.RefreshHud();
		}

		// Token: 0x06008533 RID: 34099 RVA: 0x003654A0 File Offset: 0x003636A0
		private void ClearFocus()
		{
			this._activeWearable = null;
			this._activeSlotIndex = -1;
			this._activeSource = WearableUseController.WearableSource.None;
			this.HideActionsHud();
		}

		// Token: 0x06008534 RID: 34100 RVA: 0x003654BD File Offset: 0x003636BD
		private void HandleItemRemoving(int slotIndex, InventoryItemSO removedItem)
		{
			this.EnsureRemovedWearableIsUnworn(removedItem as WearableItemSO, WearableUseController.WearableSource.Item, slotIndex);
		}

		// Token: 0x06008535 RID: 34101 RVA: 0x003654CD File Offset: 0x003636CD
		private void HandleItemRemoved(int slotIndex)
		{
			this.RemoveWornBySlot(WearableUseController.WearableSource.Item, slotIndex);
			if (this._activeSource == WearableUseController.WearableSource.Item && slotIndex == this._activeSlotIndex)
			{
				this.ClearFocus();
			}
		}

		// Token: 0x06008536 RID: 34102 RVA: 0x003654EF File Offset: 0x003636EF
		private void HandleClothRemoving(int slotIndex, InventoryItemSO removedItem)
		{
			this.EnsureRemovedWearableIsUnworn(removedItem as WearableItemSO, WearableUseController.WearableSource.Cloth, slotIndex);
		}

		// Token: 0x06008537 RID: 34103 RVA: 0x003654FF File Offset: 0x003636FF
		private void HandleClothRemoved(int slotIndex)
		{
			this.RemoveWornBySlot(WearableUseController.WearableSource.Cloth, slotIndex);
			if (this._activeSource == WearableUseController.WearableSource.Cloth && slotIndex == this._activeSlotIndex)
			{
				this.ClearFocus();
			}
		}

		// Token: 0x06008538 RID: 34104 RVA: 0x00365521 File Offset: 0x00363721
		public bool RemoveMirroredCloth(WearableItemSO item, int slotIndex)
		{
			return this.RemoveMirroredInventoryEntry(item, WearableUseController.WearableSource.Cloth, slotIndex);
		}

		// Token: 0x06008539 RID: 34105 RVA: 0x0036552C File Offset: 0x0036372C
		public bool RemoveMirroredItem(WearableItemSO item, int slotIndex)
		{
			return this.RemoveMirroredInventoryEntry(item, WearableUseController.WearableSource.Item, slotIndex);
		}

		// Token: 0x0600853A RID: 34106 RVA: 0x00365538 File Offset: 0x00363738
		private bool RemoveMirroredInventoryEntry(WearableItemSO item, WearableUseController.WearableSource source, int slotIndex)
		{
			if (this.inventory == null || item == null)
			{
				return false;
			}
			if (((source == WearableUseController.WearableSource.Item) ? this.inventory.GetItemAt(slotIndex) : this.inventory.GetClothAt(slotIndex)) != item)
			{
				return false;
			}
			WearableUseController.MirrorRemoval mirrorRemoval = this._mirrorRemoval;
			this._mirrorRemoval = new WearableUseController.MirrorRemoval
			{
				item = item,
				source = source,
				slotIndex = slotIndex
			};
			bool flag;
			try
			{
				if (source == WearableUseController.WearableSource.Item)
				{
					this.inventory.DropItem(slotIndex);
				}
				else
				{
					this.inventory.RemoveCloth(slotIndex);
				}
				flag = true;
			}
			finally
			{
				this._mirrorRemoval = mirrorRemoval;
			}
			return flag;
		}

		// Token: 0x0600853B RID: 34107 RVA: 0x003655E8 File Offset: 0x003637E8
		private bool IsRemovingMirror(WearableItemSO item, WearableUseController.WearableSource source, int slotIndex)
		{
			return this._mirrorRemoval != null && this._mirrorRemoval.item == item && this._mirrorRemoval.source == source && this._mirrorRemoval.slotIndex == slotIndex;
		}

		// Token: 0x0600853C RID: 34108 RVA: 0x00365624 File Offset: 0x00363824
		private void RemoveWornBySlot(WearableUseController.WearableSource source, int slotIndex)
		{
			WearableItemSO wearableItemSO = null;
			foreach (KeyValuePair<WearableItemSO, WearableUseController.WornEntry> keyValuePair in this._worn)
			{
				if (keyValuePair.Value.source == source && keyValuePair.Value.slotIndex == slotIndex)
				{
					wearableItemSO = keyValuePair.Key;
					break;
				}
			}
			if (wearableItemSO != null)
			{
				this.DoUnwear(wearableItemSO);
			}
			WearableItemSO wearableItemSO2 = null;
			foreach (KeyValuePair<WearableItemSO, WearableUseController.WornEntry> keyValuePair2 in this._externallyHiddenWorn)
			{
				if (keyValuePair2.Value.source == source && keyValuePair2.Value.slotIndex == slotIndex)
				{
					wearableItemSO2 = keyValuePair2.Key;
					break;
				}
			}
			if (wearableItemSO2 != null)
			{
				this._externallyHiddenWorn.Remove(wearableItemSO2);
			}
		}

		// Token: 0x0600853D RID: 34109 RVA: 0x00365728 File Offset: 0x00363928
		private void EnsureRemovedWearableIsUnworn(WearableItemSO item, WearableUseController.WearableSource source, int slotIndex)
		{
			if (item == null)
			{
				return;
			}
			WearableUseController.WornEntry wornEntry;
			if (this._worn.TryGetValue(item, out wornEntry))
			{
				if (wornEntry.source == source && wornEntry.slotIndex == slotIndex)
				{
					this.DoUnwear(item);
				}
				return;
			}
			if (this.IsRemovingMirror(item, source, slotIndex))
			{
				WearableUseController.WornEntry wornEntry2;
				if (this._externallyHiddenWorn.TryGetValue(item, out wornEntry2) && wornEntry2.source == source && wornEntry2.slotIndex == slotIndex)
				{
					this.ResetAllSecondaryEffectsImmediate(wornEntry2);
					this.ReleaseMainOverrides(item);
					this.ReleaseAllSecondaryOverrides(item, wornEntry2);
					this._externallyHiddenWorn.Remove(item);
				}
				return;
			}
			this._externallyHiddenWorn.Remove(item);
			if (!this.IsWearableVisualWorn(item))
			{
				return;
			}
			if (!this.ActiveGameplaySurfaceMatches(item, source, slotIndex))
			{
				return;
			}
			this.ApplyWearVisual(item, false);
			this.ReleaseMainOverrides(item);
			this.RefreshWornActivityGate();
			this.RecomputeVignette();
			if (!this.AnyWornItem())
			{
				PlayerInventoryManager playerInventoryManager = this.inventory;
				if (playerInventoryManager != null)
				{
					playerInventoryManager.ReleaseSlotInputLock(this);
				}
			}
			if (this.debugLogs)
			{
				Debug.LogWarning("[WearableUse] Recovered untracked worn visual '" + item.itemName + "' " + string.Format("while removing {0} slot {1}.", source, slotIndex), this);
			}
			Action<WearableItemSO> onItemUnworn = this.OnItemUnworn;
			if (onItemUnworn == null)
			{
				return;
			}
			onItemUnworn(item);
		}

		// Token: 0x0600853E RID: 34110 RVA: 0x0036585C File Offset: 0x00363A5C
		public bool IsWorn(WearableItemSO item)
		{
			return item != null && this._worn.ContainsKey(item);
		}

		// Token: 0x0600853F RID: 34111 RVA: 0x00365878 File Offset: 0x00363A78
		public void CopyWornItemsTo(List<WearableItemSO> destination)
		{
			if (destination == null)
			{
				throw new ArgumentNullException("destination");
			}
			destination.Clear();
			foreach (KeyValuePair<WearableItemSO, WearableUseController.WornEntry> keyValuePair in this._worn)
			{
				if (keyValuePair.Key != null)
				{
					destination.Add(keyValuePair.Key);
				}
			}
		}

		// Token: 0x17001524 RID: 5412
		// (get) Token: 0x06008540 RID: 34112 RVA: 0x003658F4 File Offset: 0x00363AF4
		public bool IsMouthPhysicallyBlocked
		{
			get
			{
				foreach (KeyValuePair<WearableItemSO, WearableUseController.WornEntry> keyValuePair in this._worn)
				{
					if (keyValuePair.Key != null && keyValuePair.Key.BlocksMouthWhileWorn(this))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06008541 RID: 34113 RVA: 0x00365968 File Offset: 0x00363B68
		public bool IsItemInstanceWorn(WearableItemSO item, int slotIndex)
		{
			return this.IsInstanceWorn(item, WearableUseController.WearableSource.Item, slotIndex);
		}

		// Token: 0x06008542 RID: 34114 RVA: 0x00365974 File Offset: 0x00363B74
		public bool HasVisibleSkirt()
		{
			foreach (WearableItemSO wearableItemSO in this._worn.Keys)
			{
				if (wearableItemSO != null && wearableItemSO.containsSkirt && this.IsWearableRootVisible(wearableItemSO) && (!wearableItemSO.UsesMCStyle || this.styleService == null || this.ActivePresetMatches(wearableItemSO)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17001525 RID: 5413
		// (get) Token: 0x06008543 RID: 34115 RVA: 0x00365A08 File Offset: 0x00363C08
		public float WornStaminaCostMultiplier
		{
			get
			{
				float num = 1f;
				foreach (WearableItemSO wearableItemSO in this._worn.Keys)
				{
					if (wearableItemSO != null && wearableItemSO.staminaCostMultiplier > 0f && wearableItemSO.staminaCostMultiplier < num && this.IsWearableRootVisible(wearableItemSO) && (!wearableItemSO.UsesMCStyle || this.styleService == null || this.ActivePresetMatches(wearableItemSO)))
					{
						num = Mathf.Clamp(wearableItemSO.staminaCostMultiplier, 0.1f, 1f);
					}
				}
				return num;
			}
		}

		// Token: 0x06008544 RID: 34116 RVA: 0x00365AC0 File Offset: 0x00363CC0
		public bool HasWornInventoryDurability(WearableItemSO item, float amount = 1f)
		{
			WearableUseController.WornEntry wornEntry;
			if (item == null || !this._worn.TryGetValue(item, out wornEntry))
			{
				return false;
			}
			PlayerInventoryManager playerInventoryManager = ((this.inventory != null) ? this.inventory : PlayerInventoryManager.Instance);
			if (playerInventoryManager == null)
			{
				return false;
			}
			if (wornEntry.source != WearableUseController.WearableSource.Item)
			{
				return wornEntry.source == WearableUseController.WearableSource.Cloth && playerInventoryManager.HasClothDurabilityAt(wornEntry.slotIndex, item, amount);
			}
			return playerInventoryManager.HasItemDurabilityAt(wornEntry.slotIndex, item, amount);
		}

		// Token: 0x06008545 RID: 34117 RVA: 0x00365B40 File Offset: 0x00363D40
		public bool TryConsumeWornInventoryDurability(WearableItemSO item, float amount, out float remaining)
		{
			remaining = 0f;
			WearableUseController.WornEntry wornEntry;
			if (item == null || !this._worn.TryGetValue(item, out wornEntry))
			{
				return false;
			}
			PlayerInventoryManager playerInventoryManager = ((this.inventory != null) ? this.inventory : PlayerInventoryManager.Instance);
			if (playerInventoryManager == null)
			{
				return false;
			}
			if (wornEntry.source == WearableUseController.WearableSource.Item)
			{
				return playerInventoryManager.TryConsumeItemDurabilityAt(wornEntry.slotIndex, item, amount, out remaining);
			}
			return wornEntry.source == WearableUseController.WearableSource.Cloth && playerInventoryManager.TryConsumeClothDurabilityAt(wornEntry.slotIndex, item, amount, out remaining);
		}

		// Token: 0x06008546 RID: 34118 RVA: 0x00365BCC File Offset: 0x00363DCC
		public bool IsWorn(string itemId, bool requireVisible = true)
		{
			if (string.IsNullOrEmpty(itemId))
			{
				return false;
			}
			foreach (KeyValuePair<WearableItemSO, WearableUseController.WornEntry> keyValuePair in this._worn)
			{
				WearableItemSO key = keyValuePair.Key;
				if (!(key == null) && string.Equals(key.itemId, itemId, StringComparison.OrdinalIgnoreCase) && (!requireVisible || this.IsWearableRootVisible(key)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008547 RID: 34119 RVA: 0x00365C58 File Offset: 0x00363E58
		public WearableItemSO GetWorn(string itemId, bool requireVisible = true)
		{
			if (string.IsNullOrEmpty(itemId))
			{
				return null;
			}
			foreach (KeyValuePair<WearableItemSO, WearableUseController.WornEntry> keyValuePair in this._worn)
			{
				WearableItemSO key = keyValuePair.Key;
				if (!(key == null) && string.Equals(key.itemId, itemId, StringComparison.OrdinalIgnoreCase) && (!requireVisible || this.IsWearableRootVisible(key)))
				{
					return key;
				}
			}
			return null;
		}

		// Token: 0x06008548 RID: 34120 RVA: 0x00365CE4 File Offset: 0x00363EE4
		private bool IsWearableRootVisible(WearableItemSO item)
		{
			if (item == null)
			{
				return false;
			}
			if (!item.UsesMCStyle || this.styleService == null)
			{
				return true;
			}
			GameObject spawnedRoot = this.styleService.GetSpawnedRoot(item.mcStylePartId);
			return spawnedRoot != null && spawnedRoot.activeSelf;
		}

		// Token: 0x06008549 RID: 34121 RVA: 0x00365D38 File Offset: 0x00363F38
		private bool IsWearableVisualWorn(WearableItemSO item)
		{
			if (item == null)
			{
				return false;
			}
			if (!item.UsesMCStyle)
			{
				return this.clothingController != null && !string.IsNullOrEmpty(item.clothingGroupName) && this.clothingController.IsGroupActive(item.clothingGroupName);
			}
			if (this.styleService == null || !this.ActivePresetMatches(item) || !this.ActiveGameplaySurfaceMatches(item, WearableUseController.WearableSource.None, -1))
			{
				return false;
			}
			GameObject spawnedRoot = this.styleService.GetSpawnedRoot(item.mcStylePartId);
			return spawnedRoot != null && spawnedRoot.activeSelf;
		}

		// Token: 0x0600854A RID: 34122 RVA: 0x00365DD0 File Offset: 0x00363FD0
		private bool ActivePresetMatches(WearableItemSO item)
		{
			if (item == null || this.styleService == null || item.mcStylePreset == null)
			{
				return false;
			}
			MCStylePresetSO activePreset = this.styleService.GetActivePreset(item.mcStylePartId);
			return activePreset == item.mcStylePreset || (activePreset != null && !string.IsNullOrEmpty(activePreset.presetId) && string.Equals(activePreset.presetId, item.mcStylePreset.presetId, StringComparison.Ordinal));
		}

		// Token: 0x0600854B RID: 34123 RVA: 0x00365E58 File Offset: 0x00364058
		private bool ActiveGameplaySurfaceMatches(WearableItemSO item, WearableUseController.WearableSource source = WearableUseController.WearableSource.None, int slotIndex = -1)
		{
			if (item == null)
			{
				return false;
			}
			if (!item.HasGameplaySurfaceOverride || !item.UsesMCStyle)
			{
				return true;
			}
			if (this.styleService == null)
			{
				return false;
			}
			MCStylePresetSO activePreset = this.styleService.GetActivePreset(item.mcStylePartId);
			int slotSurfaceIndex = this.styleService.GetSlotSurfaceIndex(item.mcStylePartId);
			MCSurfaceVariantSO mcsurfaceVariantSO;
			if (activePreset == null || !activePreset.TryGetSurfaceVariant(slotSurfaceIndex, out mcsurfaceVariantSO) || !item.ResolveShowInGameplayInventory(mcsurfaceVariantSO))
			{
				return false;
			}
			int num = ((source == WearableUseController.WearableSource.Cloth && this.inventory != null) ? this.inventory.GetClothVariantAt(slotIndex) : (-1));
			return num < 0 || num == slotSurfaceIndex;
		}

		// Token: 0x0600854C RID: 34124 RVA: 0x00365F04 File Offset: 0x00364104
		private void ReconcileVisibleWearable(WearableItemSO item, WearableUseController.WearableSource source, int slotIndex)
		{
			if (item == null || this.IsWorn(item) || !this.IsWearableVisualWorn(item))
			{
				return;
			}
			if (!this.ActiveGameplaySurfaceMatches(item, source, slotIndex))
			{
				return;
			}
			if (this.debugLogs)
			{
				Debug.LogWarning("[WearableUse] Recovering visible but untracked wearable " + string.Format("'{0}' from {1} slot {2}.", item.itemName, source, slotIndex), this);
			}
			if (source == WearableUseController.WearableSource.Item)
			{
				this.MarkAlreadyWornItem(item, slotIndex);
				return;
			}
			if (source == WearableUseController.WearableSource.Cloth)
			{
				this.MarkAlreadyWorn(item, slotIndex);
			}
		}

		// Token: 0x0600854D RID: 34125 RVA: 0x00365F86 File Offset: 0x00364186
		private bool IsFocusedInstanceWorn()
		{
			return this.IsInstanceWorn(this._activeWearable, this._activeSource, this._activeSlotIndex);
		}

		// Token: 0x0600854E RID: 34126 RVA: 0x00365FA0 File Offset: 0x003641A0
		private bool IsInstanceWorn(WearableItemSO item, WearableUseController.WearableSource source, int slotIndex)
		{
			WearableUseController.WornEntry wornEntry;
			return !(item == null) && this._worn.TryGetValue(item, out wornEntry) && wornEntry.source == source && wornEntry.slotIndex == slotIndex;
		}

		// Token: 0x0600854F RID: 34127 RVA: 0x00365FDC File Offset: 0x003641DC
		private static string LocalizedItemName(string itemName, string fallback)
		{
			if (!string.IsNullOrEmpty(itemName))
			{
				return GameLocalization.GetSource("items", itemName);
			}
			return GameLocalization.GetSource("inventory_feedback", fallback);
		}

		// Token: 0x06008550 RID: 34128 RVA: 0x00365FFD File Offset: 0x003641FD
		private static void ShowAlreadyWornFeedback(WearableItemSO item)
		{
			string wornName = ((item != null) ? item.itemName : null);
			ToastService.ShowComputed(() => GameLocalization.FormatSource("inventory_feedback", "You are already wearing a {0}!", new object[] { WearableUseController.LocalizedItemName(wornName, "this") }), "Wardrobe", -1f, "inventory_feedback", null);
		}

		// Token: 0x06008551 RID: 34129 RVA: 0x0036603C File Offset: 0x0036423C
		private bool TryValidateWear(WearableItemSO item)
		{
			if (item == null || item.isVaulted || !CollarAvailabilityPolicy.CanWear(item))
			{
				return false;
			}
			string reason;
			WearableItemSO wearableItemSO;
			if (!WardrobeLayeringRules.TryFindConflict(item, this.EnumerateEquippedWearables(), out wearableItemSO, out reason))
			{
				return true;
			}
			string candidateName = ((item != null) ? item.itemName : null);
			string conflictName = ((wearableItemSO != null) ? wearableItemSO.itemName : null);
			ToastService.ShowComputed(() => GameLocalization.FormatSource("inventory_feedback", "Remove {0} first. {1}", new object[]
			{
				WearableUseController.LocalizedItemName(conflictName, "the conflicting garment"),
				GameLocalization.GetSource("inventory_feedback", reason)
			}), "Can't wear {0}", -1f, "inventory_feedback", () => GameLocalization.FormatSource("inventory_feedback", "Can't wear {0}", new object[] { WearableUseController.LocalizedItemName(candidateName, "that garment") }));
			return false;
		}

		// Token: 0x06008552 RID: 34130 RVA: 0x003660D3 File Offset: 0x003642D3
		private IEnumerable<WearableItemSO> EnumerateEquippedWearables()
		{
			foreach (WearableItemSO wearableItemSO in this._worn.Keys)
			{
				yield return wearableItemSO;
			}
			Dictionary<WearableItemSO, WearableUseController.WornEntry>.KeyCollection.Enumerator enumerator = default(Dictionary<WearableItemSO, WearableUseController.WornEntry>.KeyCollection.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06008553 RID: 34131 RVA: 0x003660E4 File Offset: 0x003642E4
		private void RetireConflictingHiddenEntries(WearableItemSO candidate)
		{
			if (candidate == null || this._externallyHiddenWorn.Count == 0)
			{
				return;
			}
			List<WearableItemSO> list = null;
			foreach (WearableItemSO wearableItemSO in this._externallyHiddenWorn.Keys)
			{
				string text;
				if (!(wearableItemSO == null) && (wearableItemSO == candidate || WardrobeLayeringRules.Conflicts(candidate, wearableItemSO, out text)))
				{
					List<WearableItemSO> list2;
					if ((list2 = list) == null)
					{
						list2 = (list = new List<WearableItemSO>());
					}
					list2.Add(wearableItemSO);
				}
			}
			if (list == null)
			{
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				WearableItemSO wearableItemSO2 = list[i];
				this._externallyHiddenWorn.Remove(wearableItemSO2);
				if (wearableItemSO2 != candidate && wearableItemSO2.UsesMCStyle && this.styleService != null)
				{
					this.styleService.SetSlotEquipped(wearableItemSO2.mcStylePartId, false);
				}
			}
		}

		// Token: 0x17001526 RID: 5414
		// (get) Token: 0x06008554 RID: 34132 RVA: 0x003661DC File Offset: 0x003643DC
		public PlayerInventoryManager Inventory
		{
			get
			{
				return this.inventory;
			}
		}

		// Token: 0x17001527 RID: 5415
		// (get) Token: 0x06008555 RID: 34133 RVA: 0x003661E4 File Offset: 0x003643E4
		public MCStyleCustomizationService StyleService
		{
			get
			{
				return this.styleService;
			}
		}

		// Token: 0x06008556 RID: 34134 RVA: 0x003661EC File Offset: 0x003643EC
		private bool IsActionVisible(WearableActionDefinition action, bool worn)
		{
			if (!this.CanUseFocusedAction(action))
			{
				return false;
			}
			WearableActionVisibility visibility = action.Visibility;
			if (visibility != WearableActionVisibility.WhenWorn)
			{
				return visibility != WearableActionVisibility.WhenUnworn || !worn;
			}
			return worn;
		}

		// Token: 0x06008557 RID: 34135 RVA: 0x00366220 File Offset: 0x00364420
		private static bool IsTakeOffBlocked(WearableItemSO item)
		{
			if (item != null && item.category == InventoryCategory.Item && !item.coversTopDecently && !item.coversBottomDecently && !item.physicallyCoversTop && !item.physicallyCoversBottom)
			{
				return false;
			}
			if (PlayerActivityGate.IsBlocked(PlayerActivity.Undress))
			{
				return true;
			}
			RankRuntimeController rankRuntimeController = RankRuntimeController.Current;
			return rankRuntimeController != null && !rankRuntimeController.CanUnwearItem(item);
		}

		// Token: 0x06008558 RID: 34136 RVA: 0x00366288 File Offset: 0x00364488
		private void ExecuteAction(WearableActionDefinition action)
		{
			if (this._activeWearable == null || !this.CanUseFocusedAction(action))
			{
				return;
			}
			switch (action.ActionType)
			{
			case WearableActionType.ToggleWear:
				if (!this.IsFocusedInstanceWorn())
				{
					this.TryWearFocused();
					return;
				}
				if (WearableUseController.IsTakeOffBlocked(this._activeWearable))
				{
					BlockedActionFeedback.Show(null);
					return;
				}
				this.Unwear(this._activeWearable);
				return;
			case WearableActionType.Wear:
				if (!this.IsFocusedInstanceWorn())
				{
					this.TryWearFocused();
					return;
				}
				break;
			case WearableActionType.Unwear:
				if (this.IsFocusedInstanceWorn())
				{
					if (WearableUseController.IsTakeOffBlocked(this._activeWearable))
					{
						BlockedActionFeedback.Show(null);
						return;
					}
					this.Unwear(this._activeWearable);
					return;
				}
				break;
			case WearableActionType.SecondaryEffectToggle:
			{
				if (!this.IsFocusedInstanceWorn())
				{
					return;
				}
				WearableSecondaryEffect wearableSecondaryEffect = this._activeWearable.secondaryEffects.Find((WearableSecondaryEffect e) => e != null && e.effectId == action.SecondaryEffectId);
				if (wearableSecondaryEffect != null)
				{
					bool flag = this.ToggleSecondaryEffect(this._worn[this._activeWearable], wearableSecondaryEffect);
					this.RefreshHud();
					if (flag)
					{
						MissionGameplaySignals.NotifyWearableAction(this._activeWearable, action.ActionId, action.DisplayName);
						return;
					}
				}
				else if (this.debugLogs)
				{
					Debug.LogWarning(string.Concat(new string[]
					{
						"[WearableUseController] No secondary effect with id '",
						action.SecondaryEffectId,
						"' on '",
						this._activeWearable.name,
						"'"
					}));
					return;
				}
				break;
			}
			case WearableActionType.TimeFreeze:
			{
				if (!this.IsFocusedInstanceWorn())
				{
					return;
				}
				TimeFreezeController timeFreezeController = this.EnsureTimeFreezeRuntime(this._activeWearable);
				bool flag2 = timeFreezeController != null && timeFreezeController.IsActive;
				if (timeFreezeController != null && timeFreezeController.TryToggle(this._activeWearable) && !flag2)
				{
					MissionGameplaySignals.NotifyWearableAction(this._activeWearable, action.ActionId, action.DisplayName);
				}
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x06008559 RID: 34137 RVA: 0x00366478 File Offset: 0x00364678
		private TimeFreezeController EnsureTimeFreezeRuntime(WearableItemSO item)
		{
			bool flag = false;
			if (item != null && item.wearActions != null)
			{
				for (int i = 0; i < item.wearActions.Count; i++)
				{
					WearableActionDefinition wearableActionDefinition = item.wearActions[i];
					if (wearableActionDefinition != null && wearableActionDefinition.ActionType == WearableActionType.TimeFreeze)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return null;
			}
			if (this._timeFreezeRuntime == null)
			{
				this._timeFreezeRuntime = base.GetComponent<TimeFreezeController>();
			}
			if (this._timeFreezeRuntime == null)
			{
				this._timeFreezeRuntime = base.gameObject.AddComponent<TimeFreezeController>();
			}
			return this._timeFreezeRuntime;
		}

		// Token: 0x0600855A RID: 34138 RVA: 0x00366515 File Offset: 0x00364715
		private void TryWearFocused()
		{
			if (this._activeWearable == null)
			{
				return;
			}
			if (this.IsWorn(this._activeWearable))
			{
				WearableUseController.ShowAlreadyWornFeedback(this._activeWearable);
				return;
			}
			this.Wear(this._activeWearable);
		}

		// Token: 0x0600855B RID: 34139 RVA: 0x0036654C File Offset: 0x0036474C
		public void ToggleFromInventory(WearableItemSO item, int index, bool clothing)
		{
			if (item == null || this.inventory == null || item.wearActions == null)
			{
				return;
			}
			if (!clothing && item.wearOnlyWhileSelected)
			{
				if (this.inventory.GetItemAt(index) == item)
				{
					this.inventory.SelectSlot(index);
				}
				return;
			}
			bool flag = this.IsWorn(item);
			if (clothing)
			{
				if (this.inventory.GetClothAt(index) != item)
				{
					return;
				}
				if (this.inventory.ActiveClothIndex != index)
				{
					this.inventory.SelectCloth(index);
				}
				if (this.inventory.ActiveClothIndex != index)
				{
					return;
				}
				this.HandleActiveClothChanged(index);
			}
			else
			{
				if (this.inventory.GetItemAt(index) != item)
				{
					return;
				}
				if (this.inventory.ActiveSlotIndex != index || this.inventory.ActiveClothIndex >= 0)
				{
					this.inventory.SelectSlot(index);
				}
				if (this.inventory.ActiveSlotIndex != index || this.inventory.ActiveClothIndex >= 0)
				{
					return;
				}
				this.HandleActiveSlotChanged(index);
			}
			if (!clothing && item.wearOnSelection && !flag)
			{
				return;
			}
			bool flag2 = (clothing ? this.IsClothSlotWorn(index) : this.IsItemInstanceWorn(item, index));
			foreach (WearableActionDefinition wearableActionDefinition in item.wearActions)
			{
				if (!(wearableActionDefinition == null) && (wearableActionDefinition.ActionType == WearableActionType.ToggleWear || wearableActionDefinition.ActionType == (flag2 ? WearableActionType.Unwear : WearableActionType.Wear)))
				{
					this.ExecuteAction(wearableActionDefinition);
					break;
				}
			}
		}

		// Token: 0x0600855C RID: 34140 RVA: 0x003666E4 File Offset: 0x003648E4
		public void Wear(WearableItemSO item)
		{
			if (WardrobeGameplayContext.IsPresentationOnly(this))
			{
				return;
			}
			if (item == null)
			{
				return;
			}
			if (this.IsWorn(item))
			{
				return;
			}
			if (!this.TryValidateWear(item))
			{
				return;
			}
			this.RetireConflictingHiddenEntries(item);
			WearableUseController.WornEntry wornEntry = new WearableUseController.WornEntry
			{
				item = item,
				slotIndex = this._activeSlotIndex,
				source = this._activeSource,
				displacedCosmetic = this.CaptureDisplacedCosmetic(item)
			};
			this._worn[item] = wornEntry;
			this.RefreshWornActivityGate();
			this.ApplyWearVisual(item, true);
			this.RefreshBlendShapeTargetsIfNeeded(item);
			this.ApplyInstanceSurfaceVariant(wornEntry);
			if (wornEntry.source == WearableUseController.WearableSource.Item)
			{
				PlayerInventoryManager playerInventoryManager = this.inventory;
				if (playerInventoryManager != null)
				{
					playerInventoryManager.SuspendActiveItem();
				}
			}
			this.CacheOutfitMaterials(wornEntry);
			this.InitSecondaryEffectRuntimes(wornEntry);
			if (item.useOpacityFade)
			{
				this.ApplyAlphaToOutfitMaterials(wornEntry.outfitMaterials, item.mainColorProperty, item.wornAlpha);
			}
			this.StartWearTimer(wornEntry);
			this.RecomputeVignette();
			this.RefreshHud();
			this.EnsureTimeFreezeRuntime(item);
			Action<WearableItemSO> onItemWorn = this.OnItemWorn;
			if (onItemWorn == null)
			{
				return;
			}
			onItemWorn(item);
		}

		// Token: 0x0600855D RID: 34141 RVA: 0x003667F0 File Offset: 0x003649F0
		public void WearClothFromSlot(WearableItemSO item, int clothSlotIndex)
		{
			if (WardrobeGameplayContext.IsPresentationOnly(this))
			{
				return;
			}
			if (item == null || this.IsWorn(item))
			{
				return;
			}
			PlayerInventoryManager playerInventoryManager = this.inventory;
			if (playerInventoryManager != null)
			{
				playerInventoryManager.SelectCloth(clothSlotIndex);
			}
			if (this._activeSource != WearableUseController.WearableSource.Cloth || this._activeSlotIndex != clothSlotIndex)
			{
				this._activeWearable = item;
				this._activeSlotIndex = clothSlotIndex;
				this._activeSource = WearableUseController.WearableSource.Cloth;
			}
			this.Wear(item);
		}

		// Token: 0x0600855E RID: 34142 RVA: 0x0036685C File Offset: 0x00364A5C
		private MCStyleCustomizationService.SlotAppearanceSnapshot CaptureDisplacedCosmetic(WearableItemSO item)
		{
			if (this.styleService == null || item == null || !item.UsesMCStyle || !string.Equals(item.mcStylePartId, "Accessory.Eyewear", StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			MCStylePresetSO activePreset = this.styleService.GetActivePreset(item.mcStylePartId);
			if (activePreset == null)
			{
				return null;
			}
			WearableItemSO wearableItemSO = CustomizationWardrobeDiscoveryPolicy.GetMappedWearable(activePreset);
			if (wearableItemSO == null && activePreset == item.mcStylePreset)
			{
				wearableItemSO = item;
			}
			MCSurfaceVariantSO mcsurfaceVariantSO;
			activePreset.TryGetSurfaceVariant(this.styleService.GetSlotSurfaceIndex(item.mcStylePartId), out mcsurfaceVariantSO);
			if (wearableItemSO != null && wearableItemSO.ResolveShowInGameplayInventory(mcsurfaceVariantSO))
			{
				return null;
			}
			return this.styleService.CaptureSlotAppearance(item.mcStylePartId, this.clothingController);
		}

		// Token: 0x0600855F RID: 34143 RVA: 0x00366920 File Offset: 0x00364B20
		public void WearItemFromSlot(WearableItemSO item, int itemSlotIndex)
		{
			if (WardrobeGameplayContext.IsPresentationOnly(this))
			{
				return;
			}
			if (item == null || item.category != InventoryCategory.Item || this.IsWorn(item))
			{
				return;
			}
			if (this.inventory == null || this.inventory.GetItemAt(itemSlotIndex) != item)
			{
				return;
			}
			if (this.inventory.ActiveSlotIndex != itemSlotIndex)
			{
				this.inventory.SelectSlot(itemSlotIndex);
			}
			if (this.inventory.ActiveSlotIndex != itemSlotIndex)
			{
				return;
			}
			if (this._activeSource != WearableUseController.WearableSource.Item || this._activeSlotIndex != itemSlotIndex)
			{
				this._activeWearable = item;
				this._activeSlotIndex = itemSlotIndex;
				this._activeSource = WearableUseController.WearableSource.Item;
			}
			this.Wear(item);
		}

		// Token: 0x06008560 RID: 34144 RVA: 0x003669CC File Offset: 0x00364BCC
		public void MarkAlreadyWorn(WearableItemSO item, int clothSlotIndex)
		{
			if (WardrobeGameplayContext.IsPresentationOnly(this))
			{
				return;
			}
			if (item == null || item.isVaulted || !CollarAvailabilityPolicy.CanWear(item) || this.IsWorn(item))
			{
				return;
			}
			WearableUseController.WornEntry wornEntry = new WearableUseController.WornEntry
			{
				item = item,
				slotIndex = clothSlotIndex,
				source = WearableUseController.WearableSource.Cloth
			};
			this._worn[item] = wornEntry;
			this.RefreshWornActivityGate();
			this.RefreshBlendShapeTargetsIfNeeded(item);
			this.ApplyInstanceSurfaceVariant(wornEntry);
			this.CacheOutfitMaterials(wornEntry);
			this.InitSecondaryEffectRuntimes(wornEntry);
			if (item.useOpacityFade)
			{
				this.ApplyAlphaToOutfitMaterials(wornEntry.outfitMaterials, item.mainColorProperty, item.wornAlpha);
			}
			this.StartWearTimer(wornEntry);
			this.RecomputeVignette();
			this.EnsureTimeFreezeRuntime(item);
			Action<WearableItemSO> onItemWorn = this.OnItemWorn;
			if (onItemWorn == null)
			{
				return;
			}
			onItemWorn(item);
		}

		// Token: 0x06008561 RID: 34145 RVA: 0x00366A98 File Offset: 0x00364C98
		public void MarkAlreadyWornItem(WearableItemSO item, int itemSlotIndex)
		{
			if (WardrobeGameplayContext.IsPresentationOnly(this))
			{
				return;
			}
			if (item == null || item.isVaulted || !CollarAvailabilityPolicy.CanWear(item) || this.IsWorn(item))
			{
				return;
			}
			WearableUseController.WornEntry wornEntry = new WearableUseController.WornEntry
			{
				item = item,
				slotIndex = itemSlotIndex,
				source = WearableUseController.WearableSource.Item
			};
			this._worn[item] = wornEntry;
			this.RefreshWornActivityGate();
			this.RefreshBlendShapeTargetsIfNeeded(item);
			if (this.inventory != null && this.inventory.ActiveSlotIndex != itemSlotIndex)
			{
				this.inventory.SelectSlot(itemSlotIndex);
			}
			if (this._activeSource != WearableUseController.WearableSource.Item || this._activeSlotIndex != itemSlotIndex)
			{
				this._activeWearable = item;
				this._activeSlotIndex = itemSlotIndex;
				this._activeSource = WearableUseController.WearableSource.Item;
			}
			this.ApplyInstanceSurfaceVariant(wornEntry);
			this.CacheOutfitMaterials(wornEntry);
			this.InitSecondaryEffectRuntimes(wornEntry);
			if (item.useOpacityFade)
			{
				this.ApplyAlphaToOutfitMaterials(wornEntry.outfitMaterials, item.mainColorProperty, item.wornAlpha);
			}
			this.StartWearTimer(wornEntry);
			this.RecomputeVignette();
			PlayerInventoryManager playerInventoryManager = this.inventory;
			if (playerInventoryManager != null)
			{
				playerInventoryManager.SuspendActiveItemImmediate();
			}
			this.RefreshHud();
			this.EnsureTimeFreezeRuntime(item);
			Action<WearableItemSO> onItemWorn = this.OnItemWorn;
			if (onItemWorn == null)
			{
				return;
			}
			onItemWorn(item);
		}

		// Token: 0x06008562 RID: 34146 RVA: 0x00366BC8 File Offset: 0x00364DC8
		public void StowEquipmentForInteraction()
		{
			List<WearableItemSO> list = new List<WearableItemSO>();
			foreach (WearableItemSO wearableItemSO in this._worn.Keys)
			{
				if (wearableItemSO != null && wearableItemSO.stowForInteraction)
				{
					list.Add(wearableItemSO);
				}
			}
			foreach (WearableItemSO wearableItemSO2 in list)
			{
				this.DoUnwear(wearableItemSO2);
			}
			list.Clear();
			foreach (WearableItemSO wearableItemSO3 in this._externallyHiddenWorn.Keys)
			{
				if (wearableItemSO3 != null && wearableItemSO3.stowForInteraction)
				{
					list.Add(wearableItemSO3);
				}
			}
			foreach (WearableItemSO wearableItemSO4 in list)
			{
				this._externallyHiddenWorn.Remove(wearableItemSO4);
				this.ApplyWearVisual(wearableItemSO4, false);
			}
			this.RefreshHud();
		}

		// Token: 0x06008563 RID: 34147 RVA: 0x00366D2C File Offset: 0x00364F2C
		public void Unwear(WearableItemSO item)
		{
			if (!this.IsWorn(item))
			{
				return;
			}
			WearableUseController.WornEntry wornEntry = this._worn[item];
			WearableUseController.WearableSource source = wornEntry.source;
			int slotIndex = wornEntry.slotIndex;
			this.DoUnwear(item);
			if (source == WearableUseController.WearableSource.Item && this._activeSource == WearableUseController.WearableSource.Item && this._activeWearable == item && this._activeSlotIndex == slotIndex && this.inventory != null && this.inventory.ActiveSlotIndex == slotIndex)
			{
				this.inventory.ResumeActiveItem();
			}
			this.RefreshHud();
		}

		// Token: 0x06008564 RID: 34148 RVA: 0x00366DB4 File Offset: 0x00364FB4
		private void DoUnwear(WearableItemSO item)
		{
			WearableUseController.WornEntry wornEntry;
			if (!this._worn.TryGetValue(item, out wornEntry))
			{
				return;
			}
			this.ResetAllSecondaryEffectsImmediate(wornEntry);
			if (!this.IsRemovingMirror(item, wornEntry.source, wornEntry.slotIndex))
			{
				if (item.useOpacityFade)
				{
					this.ApplyAlphaToOutfitMaterials(wornEntry.outfitMaterials, item.mainColorProperty, item.unwornAlpha);
				}
				this.ApplyWearVisual(item, false);
			}
			this.ReleaseMainOverrides(item);
			this.ReleaseAllSecondaryOverrides(item, wornEntry);
			this._worn.Remove(item);
			this.RefreshWornActivityGate();
			this.RecomputeVignette();
			if (!this.AnyWornItem())
			{
				PlayerInventoryManager playerInventoryManager = this.inventory;
				if (playerInventoryManager != null)
				{
					playerInventoryManager.ReleaseSlotInputLock(this);
				}
			}
			Action<WearableItemSO> onItemUnworn = this.OnItemUnworn;
			if (onItemUnworn == null)
			{
				return;
			}
			onItemUnworn(item);
		}

		// Token: 0x06008565 RID: 34149 RVA: 0x00366E6C File Offset: 0x0036506C
		private void RefreshWornActivityGate()
		{
			PlayerActivity playerActivity = PlayerActivity.None;
			foreach (KeyValuePair<WearableItemSO, WearableUseController.WornEntry> keyValuePair in this._worn)
			{
				WearableUseController.WornEntry value = keyValuePair.Value;
				WearableItemSO wearableItemSO = ((value != null) ? value.item : null);
				if (wearableItemSO != null)
				{
					playerActivity |= wearableItemSO.blockedActivitiesWhileWorn;
				}
			}
			bool flag = PlayerActivityGate.HasLock(this.ArmActionsLockReason);
			if (playerActivity == PlayerActivity.None)
			{
				if (flag)
				{
					PlayerActivityGate.Pop(this.ArmActionsLockReason);
				}
				this._appliedWornBlockedActivities = PlayerActivity.None;
				return;
			}
			if (!flag || playerActivity != this._appliedWornBlockedActivities)
			{
				PlayerActivityGate.Push(this.ArmActionsLockReason, playerActivity);
			}
			this._appliedWornBlockedActivities = playerActivity;
		}

		// Token: 0x06008566 RID: 34150 RVA: 0x00366F28 File Offset: 0x00365128
		private bool AnyWornItem()
		{
			foreach (KeyValuePair<WearableItemSO, WearableUseController.WornEntry> keyValuePair in this._worn)
			{
				if (keyValuePair.Value.source == WearableUseController.WearableSource.Item)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008567 RID: 34151 RVA: 0x00366F8C File Offset: 0x0036518C
		private void HandleSlotsMoved(bool clothing, int from, int to)
		{
			WearableUseController.WearableSource wearableSource = (clothing ? WearableUseController.WearableSource.Cloth : WearableUseController.WearableSource.Item);
			foreach (WearableUseController.WornEntry wornEntry in this._worn.Values)
			{
				WearableUseController.FollowSlotMove(wornEntry, wearableSource, from, to);
			}
			foreach (WearableUseController.WornEntry wornEntry2 in this._externallyHiddenWorn.Values)
			{
				WearableUseController.FollowSlotMove(wornEntry2, wearableSource, from, to);
			}
			if (this._activeSource == wearableSource)
			{
				if (this._activeSlotIndex == from)
				{
					this._activeSlotIndex = to;
					return;
				}
				if (this._activeSlotIndex == to)
				{
					this._activeSlotIndex = from;
				}
			}
		}

		// Token: 0x06008568 RID: 34152 RVA: 0x00367060 File Offset: 0x00365260
		private static void FollowSlotMove(WearableUseController.WornEntry entry, WearableUseController.WearableSource source, int from, int to)
		{
			if (entry.source != source)
			{
				return;
			}
			if (entry.slotIndex == from)
			{
				entry.slotIndex = to;
				return;
			}
			if (entry.slotIndex == to)
			{
				entry.slotIndex = from;
			}
		}

		// Token: 0x06008569 RID: 34153 RVA: 0x00367090 File Offset: 0x00365290
		public bool IsClothSlotWorn(int clothSlotIndex)
		{
			foreach (KeyValuePair<WearableItemSO, WearableUseController.WornEntry> keyValuePair in this._worn)
			{
				if (keyValuePair.Value.source == WearableUseController.WearableSource.Cloth && keyValuePair.Value.slotIndex == clothSlotIndex)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600856A RID: 34154 RVA: 0x00367104 File Offset: 0x00365304
		private void ApplyWearVisual(WearableItemSO item, bool worn)
		{
			if (item == null)
			{
				return;
			}
			bool applyingWearVisual = this._applyingWearVisual;
			this._applyingWearVisual = true;
			try
			{
				if (item.UsesMCStyle)
				{
					if (this.styleService == null)
					{
						if (this.debugLogs)
						{
							Debug.LogWarning("[WearableUseController] '" + item.itemName + "' uses MCStyle wear but no MCStyleCustomizationService was resolved on the rig. Wear has no visual effect.");
						}
					}
					else
					{
						string mcStylePartId = item.mcStylePartId;
						bool flag = this.clothingController != null && this.clothingController.ManagesSlot(mcStylePartId);
						if (worn)
						{
							bool flag2 = this.ActivePresetMatches(item);
							if (flag && flag2)
							{
								this.clothingController.SetSlotVisible(mcStylePartId, true);
							}
							else if (this.styleService.EquipRuntimePreset(mcStylePartId, item.mcStylePreset))
							{
								if (this.clothingController != null && this.clothingController.ManagesSlot(mcStylePartId))
								{
									this.clothingController.SetSlotVisible(mcStylePartId, true);
								}
								else
								{
									this.styleService.SetSlotPhysicallyHidden(mcStylePartId, false);
								}
							}
							else if (this.debugLogs)
							{
								Debug.LogWarning(string.Concat(new string[] { "[WearableUseController] MCStyle wear failed for '", item.itemName, "' (partId '", mcStylePartId, "'). Check the slot exists on the service." }));
							}
							this.ResetGarmentDetailAnimators(mcStylePartId, item.mcStylePreset);
						}
						else if (this.ActivePresetMatches(item))
						{
							WearableUseController.WornEntry wornEntry;
							if (this.ActiveGameplaySurfaceMatches(item, this._worn.TryGetValue(item, out wornEntry) ? wornEntry.source : WearableUseController.WearableSource.None, (wornEntry != null) ? wornEntry.slotIndex : (-1)))
							{
								if (((wornEntry != null) ? wornEntry.displacedCosmetic : null) == null || !this.styleService.RestoreSlotAppearance(wornEntry.displacedCosmetic))
								{
									if (flag)
									{
										this.clothingController.SetSlotVisible(mcStylePartId, false);
									}
									else
									{
										this.styleService.SetSlotPhysicallyHidden(mcStylePartId, true);
									}
								}
							}
						}
					}
				}
				else if (this.clothingController != null && !string.IsNullOrEmpty(item.clothingGroupName))
				{
					this.clothingController.SetGroupState(item.clothingGroupName, worn);
				}
			}
			finally
			{
				this._applyingWearVisual = applyingWearVisual;
			}
		}

		// Token: 0x0600856B RID: 34155 RVA: 0x00367330 File Offset: 0x00365530
		private void ResetGarmentDetailAnimators(string partId, MCStylePresetSO preset)
		{
			if (this.styleService == null || string.IsNullOrEmpty(partId))
			{
				return;
			}
			base.StartCoroutine(this.ResetGarmentDetailAnimatorsNextFrame(partId, preset));
		}

		// Token: 0x0600856C RID: 34156 RVA: 0x00367358 File Offset: 0x00365558
		private IEnumerator ResetGarmentDetailAnimatorsNextFrame(string partId, MCStylePresetSO preset)
		{
			yield return null;
			GameObject spawnedRoot = this.styleService.GetSpawnedRoot(partId);
			if (spawnedRoot == null)
			{
				yield break;
			}
			foreach (Animator animator in spawnedRoot.GetComponentsInChildren<Animator>(true))
			{
				if (!(animator == null) && animator.runtimeAnimatorController != null && animator.HasState(0, WearableUseController.EmptyStateHash))
				{
					animator.enabled = false;
				}
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in spawnedRoot.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				Mesh sharedMesh = skinnedMeshRenderer.sharedMesh;
				if (!(sharedMesh == null))
				{
					for (int j = 0; j < sharedMesh.blendShapeCount; j++)
					{
						skinnedMeshRenderer.SetBlendShapeWeight(j, 0f);
					}
				}
			}
			WearableUseController.RestoreAuthoredBlendShapeDefaults(spawnedRoot, (preset != null) ? preset.physicalPrefab : null);
			if (this._blendShapeSync == null && this.styleService != null)
			{
				this._blendShapeSync = this.styleService.GetComponentInParent<ClothingBlendShapeSyncController>();
			}
			if (this._blendShapeSync != null)
			{
				this._blendShapeSync.RequestResync();
			}
			yield break;
		}

		// Token: 0x0600856D RID: 34157 RVA: 0x00367378 File Offset: 0x00365578
		private static void RestoreAuthoredBlendShapeDefaults(GameObject runtimeRoot, GameObject authoredPrefab)
		{
			if (runtimeRoot == null || authoredPrefab == null)
			{
				return;
			}
			SkinnedMeshRenderer[] componentsInChildren = runtimeRoot.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in authoredPrefab.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				if (!(skinnedMeshRenderer == null) && !(skinnedMeshRenderer.sharedMesh == null))
				{
					SkinnedMeshRenderer skinnedMeshRenderer2 = null;
					foreach (SkinnedMeshRenderer skinnedMeshRenderer3 in componentsInChildren)
					{
						if (!(skinnedMeshRenderer3 == null) && !(skinnedMeshRenderer3.sharedMesh == null) && !(skinnedMeshRenderer3.name != skinnedMeshRenderer.name) && (skinnedMeshRenderer3.sharedMesh == skinnedMeshRenderer.sharedMesh || skinnedMeshRenderer3.sharedMesh.name == skinnedMeshRenderer.sharedMesh.name))
						{
							skinnedMeshRenderer2 = skinnedMeshRenderer3;
							break;
						}
					}
					if (!(skinnedMeshRenderer2 == null))
					{
						for (int k = 0; k < skinnedMeshRenderer.sharedMesh.blendShapeCount; k++)
						{
							float blendShapeWeight = skinnedMeshRenderer.GetBlendShapeWeight(k);
							if (!Mathf.Approximately(blendShapeWeight, 0f))
							{
								string blendShapeName = skinnedMeshRenderer.sharedMesh.GetBlendShapeName(k);
								int blendShapeIndex = skinnedMeshRenderer2.sharedMesh.GetBlendShapeIndex(blendShapeName);
								if (blendShapeIndex >= 0)
								{
									skinnedMeshRenderer2.SetBlendShapeWeight(blendShapeIndex, blendShapeWeight);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600856E RID: 34158 RVA: 0x003674D0 File Offset: 0x003656D0
		private void StartWearTimer(WearableUseController.WornEntry entry)
		{
			WearableItemSO item = entry.item;
			if (entry.source == WearableUseController.WearableSource.Cloth && item != null && item.hasWearDuration && item.wearDuration > 0f)
			{
				float num = ((this.inventory != null) ? this.inventory.GetClothWearDurationScaled(entry.slotIndex) : 0f);
				if (num <= 0f)
				{
					num = item.wearDuration;
				}
				float num2 = ((this.inventory != null) ? this.inventory.GetClothDurability(entry.slotIndex) : num);
				entry.wearTimeRemaining = ((num2 > 0f) ? Mathf.Min(num2, num) : num);
				return;
			}
			entry.wearTimeRemaining = -1f;
		}

		// Token: 0x0600856F RID: 34159 RVA: 0x00367590 File Offset: 0x00365790
		private void ApplyInstanceSurfaceVariant(WearableUseController.WornEntry entry)
		{
			if (entry == null || entry.item == null || !entry.item.UsesMCStyle)
			{
				return;
			}
			if (entry.source != WearableUseController.WearableSource.Cloth || this.styleService == null || this.inventory == null)
			{
				return;
			}
			int clothVariantAt = this.inventory.GetClothVariantAt(entry.slotIndex);
			if (clothVariantAt < 0)
			{
				return;
			}
			string mcStylePartId = entry.item.mcStylePartId;
			int slotSurfaceCount = this.styleService.GetSlotSurfaceCount(mcStylePartId);
			if (clothVariantAt >= slotSurfaceCount)
			{
				if (this.debugLogs)
				{
					Debug.LogWarning(string.Format("[WearableUseController] Instance variant {0} out of range ", clothVariantAt) + string.Format("({0} surfaces) for '{1}' - keeping current look.", slotSurfaceCount, entry.item.itemName), this);
				}
				return;
			}
			this.styleService.SelectSurfaceForSlot(mcStylePartId, clothVariantAt);
		}

		// Token: 0x06008570 RID: 34160 RVA: 0x00367664 File Offset: 0x00365864
		private void TickWearTimers()
		{
			if (WardrobeGameplayContext.IsPresentationOnly(this))
			{
				return;
			}
			if (PlayerInventoryManager.InfiniteClothingDurabilityInActiveScene)
			{
				return;
			}
			if (GameplayDurationPause.IsPaused)
			{
				return;
			}
			if (this._worn.Count == 0)
			{
				return;
			}
			List<WearableItemSO> list = null;
			foreach (KeyValuePair<WearableItemSO, WearableUseController.WornEntry> keyValuePair in this._worn)
			{
				WearableUseController.WornEntry value = keyValuePair.Value;
				if (value.wearTimeRemaining >= 0f)
				{
					float num = Mathf.Max(0.25f, RunModeService.ClothingDurationMultiplier);
					value.wearTimeRemaining -= Time.deltaTime / num;
					if (value.wearTimeRemaining <= 0f)
					{
						List<WearableItemSO> list2;
						if ((list2 = list) == null)
						{
							list2 = (list = new List<WearableItemSO>());
						}
						list2.Add(value.item);
					}
					else
					{
						PlayerInventoryManager playerInventoryManager = this.inventory;
						if (playerInventoryManager != null)
						{
							playerInventoryManager.SetClothDurability(value.slotIndex, value.wearTimeRemaining);
						}
					}
				}
			}
			if (list != null)
			{
				foreach (WearableItemSO wearableItemSO in list)
				{
					this.BreakWornCloth(wearableItemSO);
				}
			}
		}

		// Token: 0x06008571 RID: 34161 RVA: 0x003677A0 File Offset: 0x003659A0
		private void BreakWornCloth(WearableItemSO item)
		{
			if (WardrobeGameplayContext.IsPresentationOnly(this))
			{
				return;
			}
			if (PlayerInventoryManager.InfiniteClothingDurabilityInActiveScene)
			{
				return;
			}
			WearableUseController.WornEntry wornEntry;
			if (!this._worn.TryGetValue(item, out wornEntry))
			{
				return;
			}
			int slotIndex = wornEntry.slotIndex;
			if (item.breakSFX != null)
			{
				AudioSource.PlayClipAtPoint(item.breakSFX, base.transform.position);
			}
			MissionGameplaySignals.NotifyClothingBroken(item);
			this.DoUnwear(item);
			PlayerInventoryManager playerInventoryManager = this.inventory;
			if (playerInventoryManager != null)
			{
				playerInventoryManager.RemoveCloth(slotIndex);
			}
			if (this._activeWearable == item)
			{
				this.ClearFocus();
			}
			this.RefreshHud();
		}

		// Token: 0x06008572 RID: 34162 RVA: 0x00367834 File Offset: 0x00365A34
		private void RecomputeVignette()
		{
			float num = 0f;
			float num2 = 0.4f;
			AnimationCurve animationCurve = null;
			bool flag = false;
			foreach (KeyValuePair<WearableItemSO, WearableUseController.WornEntry> keyValuePair in this._worn)
			{
				WearableItemSO item = keyValuePair.Value.item;
				if (!(item == null) && item.useVignette)
				{
					flag = true;
					if (item.vignetteIntensity > num)
					{
						num = item.vignetteIntensity;
						num2 = item.vignetteTransitionDuration;
						animationCurve = item.vignetteCurve;
					}
				}
			}
			this.FadeVignetteTo(num, flag ? num2 : 0.4f, animationCurve);
		}

		// Token: 0x06008573 RID: 34163 RVA: 0x003678EC File Offset: 0x00365AEC
		private void FadeVignetteTo(float target, float duration, AnimationCurve curve)
		{
			if (this._vignetteRoutine != null)
			{
				base.StopCoroutine(this._vignetteRoutine);
			}
			this._vignetteRoutine = null;
			if (target <= 0f && this._wearableVignetteIntensity <= 0f)
			{
				GameplayVignetteMixer.Release(this._vignette, this);
				return;
			}
			if (!this.EnsureVignetteResolved())
			{
				return;
			}
			if (duration <= 0f)
			{
				this.ApplyVignetteIntensity(target);
				this._vignetteRoutine = null;
				return;
			}
			if (!base.isActiveAndEnabled || !base.gameObject.activeInHierarchy)
			{
				this.ApplyVignetteIntensity(target);
				this._vignetteRoutine = null;
				return;
			}
			this._vignetteRoutine = base.StartCoroutine(this.VignetteFadeRoutine(target, duration, curve));
		}

		// Token: 0x06008574 RID: 34164 RVA: 0x0036798F File Offset: 0x00365B8F
		private IEnumerator VignetteFadeRoutine(float target, float duration, AnimationCurve curve)
		{
			float from = this._wearableVignetteIntensity;
			float elapsed = 0f;
			AnimationCurve c = ((curve != null && curve.length > 0) ? curve : AnimationCurve.Linear(0f, 0f, 1f, 1f));
			while (elapsed < duration)
			{
				elapsed += Time.deltaTime;
				float num = Mathf.Clamp01(elapsed / duration);
				this.ApplyVignetteIntensity(Mathf.Lerp(from, target, c.Evaluate(num)));
				yield return null;
			}
			this.ApplyVignetteIntensity(target);
			this._vignetteRoutine = null;
			yield break;
		}

		// Token: 0x06008575 RID: 34165 RVA: 0x003679B4 File Offset: 0x00365BB4
		private void ApplyVignetteIntensity(float intensity)
		{
			this._wearableVignetteIntensity = Mathf.Clamp01(intensity);
			if (this._wearableVignetteIntensity <= 0f)
			{
				GameplayVignetteMixer.Release(this._vignette, this);
				return;
			}
			GameplayVignetteMixer.Set(this._vignette, this, 30, this._wearableVignetteIntensity, null, null);
		}

		// Token: 0x06008576 RID: 34166 RVA: 0x00367A10 File Offset: 0x00365C10
		private void ApplyAlphaToOutfitMaterials(List<Material> materials, string property, float alpha)
		{
			string text = (string.IsNullOrEmpty(property) ? "_Color" : property);
			for (int i = 0; i < materials.Count; i++)
			{
				Material material = materials[i];
				if (!(material == null) && material.HasProperty(text))
				{
					Color color = material.GetColor(text);
					color.a = alpha;
					material.SetColor(text, color);
				}
			}
		}

		// Token: 0x06008577 RID: 34167 RVA: 0x00367A74 File Offset: 0x00365C74
		private void ApplyMainBlendshapesToManager(WearableItemSO item)
		{
			BlendShapeManager rigBlendShapeManager = this.RigBlendShapeManager;
			if (rigBlendShapeManager == null || item == null || item.blendshapes == null)
			{
				return;
			}
			string mainSourceId = this.GetMainSourceId(item);
			for (int i = 0; i < item.blendshapes.Count; i++)
			{
				WearableBlendshape wearableBlendshape = item.blendshapes[i];
				if (!string.IsNullOrEmpty(wearableBlendshape.blendshapeName))
				{
					rigBlendShapeManager.SetOverride(wearableBlendshape.blendshapeName, wearableBlendshape.wornValue, mainSourceId, this.blendShapePriority, false);
				}
			}
		}

		// Token: 0x06008578 RID: 34168 RVA: 0x00367AF8 File Offset: 0x00365CF8
		private void ReleaseMainOverrides(WearableItemSO item)
		{
			BlendShapeManager rigBlendShapeManager = this.RigBlendShapeManager;
			if (rigBlendShapeManager == null)
			{
				return;
			}
			this.ReleaseAnimatedMainOverrides(item);
			rigBlendShapeManager.RemoveAllOverridesFromSource(this.GetMainSourceId(item));
		}

		// Token: 0x06008579 RID: 34169 RVA: 0x00367B2C File Offset: 0x00365D2C
		private void ReleaseAnimatedMainOverrides(WearableItemSO item)
		{
			BlendShapeManager rigBlendShapeManager = this.RigBlendShapeManager;
			if (rigBlendShapeManager == null || item == null || item.blendshapes == null)
			{
				return;
			}
			string mainSourceId = this.GetMainSourceId(item);
			foreach (WearableBlendshape wearableBlendshape in item.blendshapes)
			{
				if (wearableBlendshape != null && wearableBlendshape.releaseToAnimationOnUnwear)
				{
					rigBlendShapeManager.RemoveOverride(wearableBlendshape.blendshapeName, mainSourceId, true, true, 0f);
				}
			}
		}

		// Token: 0x0600857A RID: 34170 RVA: 0x00367BC4 File Offset: 0x00365DC4
		private void RefreshBlendShapeTargetsIfNeeded(WearableItemSO item)
		{
			if (item == null || item.blendshapes == null || item.blendshapes.Count == 0)
			{
				return;
			}
			BlendShapeManager rigBlendShapeManager = this.RigBlendShapeManager;
			if (rigBlendShapeManager == null)
			{
				return;
			}
			rigBlendShapeManager.Rebuild();
		}

		// Token: 0x0600857B RID: 34171 RVA: 0x00367BF8 File Offset: 0x00365DF8
		private void CacheOutfitMaterials(WearableUseController.WornEntry entry)
		{
			WearableItemSO item = entry.item;
			entry.outfitMaterials.Clear();
			if (item.UsesMCStyle)
			{
				if (this.styleService == null)
				{
					return;
				}
				GameObject spawnedRoot = this.styleService.GetSpawnedRoot(item.mcStylePartId);
				if (spawnedRoot == null)
				{
					return;
				}
				this.CollectMaterialsFrom(spawnedRoot, entry.outfitMaterials);
				return;
			}
			else
			{
				if (this.clothingController == null || string.IsNullOrEmpty(item.clothingGroupName))
				{
					return;
				}
				ClothingGroup clothingGroup = this.clothingController.clothingGroups.Find((ClothingGroup g) => g.groupName == item.clothingGroupName);
				if (clothingGroup == null)
				{
					return;
				}
				for (int i = 0; i < clothingGroup.outfitObjects.Count; i++)
				{
					GameObject gameObject = clothingGroup.outfitObjects[i];
					if (!(gameObject == null))
					{
						this.CollectMaterialsFrom(gameObject, entry.outfitMaterials);
					}
				}
				return;
			}
		}

		// Token: 0x0600857C RID: 34172 RVA: 0x00367CEC File Offset: 0x00365EEC
		private void CollectMaterialsFrom(GameObject root, List<Material> into)
		{
			Renderer[] componentsInChildren = root.GetComponentsInChildren<Renderer>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				foreach (Material material in componentsInChildren[i].materials)
				{
					if (material != null)
					{
						into.Add(material);
					}
				}
			}
		}

		// Token: 0x0600857D RID: 34173 RVA: 0x00367D40 File Offset: 0x00365F40
		private void InitSecondaryEffectRuntimes(WearableUseController.WornEntry entry)
		{
			entry.secondaryRuntimes.Clear();
			WearableItemSO item = entry.item;
			for (int i = 0; i < item.secondaryEffects.Count; i++)
			{
				WearableSecondaryEffect wearableSecondaryEffect = item.secondaryEffects[i];
				if (wearableSecondaryEffect != null && !string.IsNullOrEmpty(wearableSecondaryEffect.effectId))
				{
					entry.secondaryRuntimes[wearableSecondaryEffect.effectId] = new WearableUseController.SecondaryEffectRuntime
					{
						data = wearableSecondaryEffect,
						progress = 0f,
						isActive = false
					};
				}
			}
		}

		// Token: 0x0600857E RID: 34174 RVA: 0x00367DC4 File Offset: 0x00365FC4
		private bool ToggleSecondaryEffect(WearableUseController.WornEntry entry, WearableSecondaryEffect effect)
		{
			WearableUseController.SecondaryEffectRuntime secondaryEffectRuntime;
			if (!entry.secondaryRuntimes.TryGetValue(effect.effectId, out secondaryEffectRuntime))
			{
				return false;
			}
			secondaryEffectRuntime.isActive = !secondaryEffectRuntime.isActive;
			this.StartSecondaryTransition(entry, secondaryEffectRuntime, secondaryEffectRuntime.isActive ? 1f : 0f);
			return secondaryEffectRuntime.isActive;
		}

		// Token: 0x0600857F RID: 34175 RVA: 0x00367E1C File Offset: 0x0036601C
		private void StartSecondaryTransition(WearableUseController.WornEntry entry, WearableUseController.SecondaryEffectRuntime rt, float toProgress)
		{
			if (rt.routine != null)
			{
				base.StopCoroutine(rt.routine);
			}
			rt.routine = null;
			if (!base.isActiveAndEnabled || !base.gameObject.activeInHierarchy)
			{
				rt.progress = toProgress;
				this.ApplySecondaryOpacity(entry, rt);
				return;
			}
			rt.routine = base.StartCoroutine(this.SecondaryTransitionRoutine(entry, rt, toProgress));
		}

		// Token: 0x06008580 RID: 34176 RVA: 0x00367E80 File Offset: 0x00366080
		private void SettleSecondaryEffects(WearableUseController.WornEntry entry)
		{
			foreach (WearableUseController.SecondaryEffectRuntime secondaryEffectRuntime in entry.secondaryRuntimes.Values)
			{
				if (secondaryEffectRuntime.routine != null)
				{
					base.StopCoroutine(secondaryEffectRuntime.routine);
				}
				secondaryEffectRuntime.routine = null;
				secondaryEffectRuntime.progress = (secondaryEffectRuntime.isActive ? 1f : 0f);
				this.ApplySecondaryOpacity(entry, secondaryEffectRuntime);
			}
		}

		// Token: 0x06008581 RID: 34177 RVA: 0x00367F10 File Offset: 0x00366110
		private IEnumerator SecondaryTransitionRoutine(WearableUseController.WornEntry entry, WearableUseController.SecondaryEffectRuntime rt, float toProgress)
		{
			WearableSecondaryEffect data = rt.data;
			float fromProgress = rt.progress;
			float duration = Mathf.Max(0.0001f, data.transitionDuration);
			AnimationCurve curve = ((data.transitionCurve != null && data.transitionCurve.length > 0) ? data.transitionCurve : AnimationCurve.Linear(0f, 0f, 1f, 1f));
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.deltaTime;
				float num = Mathf.Clamp01(elapsed / duration);
				float num2 = curve.Evaluate(num);
				rt.progress = Mathf.Lerp(fromProgress, toProgress, num2);
				this.ApplySecondaryOpacity(entry, rt);
				yield return null;
			}
			rt.progress = toProgress;
			this.ApplySecondaryOpacity(entry, rt);
			rt.routine = null;
			yield break;
		}

		// Token: 0x06008582 RID: 34178 RVA: 0x00367F34 File Offset: 0x00366134
		private void ApplySecondaryOpacity(WearableUseController.WornEntry entry, WearableUseController.SecondaryEffectRuntime rt)
		{
			WearableSecondaryEffect data = rt.data;
			if (!data.useOpacityFade)
			{
				return;
			}
			float num = Mathf.Lerp(data.inactiveAlpha, data.activeAlpha, rt.progress);
			this.ApplyAlphaToOutfitMaterials(entry.outfitMaterials, data.mainColorProperty, num);
		}

		// Token: 0x06008583 RID: 34179 RVA: 0x00367F7C File Offset: 0x0036617C
		private void ApplySecondaryBlendshapesToManager(WearableItemSO item, WearableUseController.SecondaryEffectRuntime rt)
		{
			BlendShapeManager rigBlendShapeManager = this.RigBlendShapeManager;
			if (rigBlendShapeManager == null || item == null)
			{
				return;
			}
			string secondarySourceId = this.GetSecondarySourceId(item, rt.data.effectId);
			for (int i = 0; i < rt.data.blendshapes.Count; i++)
			{
				WearableBlendshape wearableBlendshape = rt.data.blendshapes[i];
				if (!string.IsNullOrEmpty(wearableBlendshape.blendshapeName))
				{
					float num = Mathf.Lerp(wearableBlendshape.unwornValue, wearableBlendshape.wornValue, rt.progress);
					rigBlendShapeManager.SetOverride(wearableBlendshape.blendshapeName, num, secondarySourceId, this.blendShapePriority, false);
				}
			}
		}

		// Token: 0x06008584 RID: 34180 RVA: 0x00368020 File Offset: 0x00366220
		private void ResetAllSecondaryEffectsImmediate(WearableUseController.WornEntry entry)
		{
			foreach (KeyValuePair<string, WearableUseController.SecondaryEffectRuntime> keyValuePair in entry.secondaryRuntimes)
			{
				WearableUseController.SecondaryEffectRuntime value = keyValuePair.Value;
				if (value.routine != null)
				{
					base.StopCoroutine(value.routine);
					value.routine = null;
				}
				value.progress = 0f;
				value.isActive = false;
				if (!this.IsRemovingMirror(entry.item, entry.source, entry.slotIndex))
				{
					this.ApplySecondaryOpacity(entry, value);
				}
			}
		}

		// Token: 0x06008585 RID: 34181 RVA: 0x003680C4 File Offset: 0x003662C4
		private void ReleaseAllSecondaryOverrides(WearableItemSO item, WearableUseController.WornEntry entry)
		{
			BlendShapeManager rigBlendShapeManager = this.RigBlendShapeManager;
			if (rigBlendShapeManager == null || item == null)
			{
				return;
			}
			foreach (KeyValuePair<string, WearableUseController.SecondaryEffectRuntime> keyValuePair in entry.secondaryRuntimes)
			{
				rigBlendShapeManager.RemoveAllOverridesFromSource(this.GetSecondarySourceId(item, keyValuePair.Value.data.effectId));
			}
		}

		// Token: 0x06008586 RID: 34182 RVA: 0x00368148 File Offset: 0x00366348
		private string GetMainSourceId(WearableItemSO item)
		{
			string text = ((!string.IsNullOrEmpty(item.itemId)) ? item.itemId : item.name);
			return "Wearable." + text;
		}

		// Token: 0x06008587 RID: 34183 RVA: 0x0036817C File Offset: 0x0036637C
		private string GetSecondarySourceId(WearableItemSO item, string effectId)
		{
			string text = ((!string.IsNullOrEmpty(item.itemId)) ? item.itemId : item.name);
			return "WearableEffect." + text + "." + effectId;
		}

		// Token: 0x06008588 RID: 34184 RVA: 0x003681B8 File Offset: 0x003663B8
		private bool CanUseFocusedActionInCover(WearableActionDefinition action)
		{
			return WallCoverActivityLock.ActiveController == null || (WallCoverActivityLock.IsPlayerInCover && this._activeSource == WearableUseController.WearableSource.Cloth && PlayerActivityGate.Allows(PlayerActivity.Inventory) && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI) && (action.ActionType == WearableActionType.Wear || action.ActionType == WearableActionType.Unwear || action.ActionType == WearableActionType.ToggleWear));
		}

		// Token: 0x06008589 RID: 34185 RVA: 0x00368218 File Offset: 0x00366418
		private bool CanUseFocusedAction(WearableActionDefinition action)
		{
			return !(action == null) && !TextInputFocus.BlocksGameplayInput && !CielOSController.AnyOpen && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI) && ((action.ActionType == WearableActionType.TimeFreeze && this._timeFreezeRuntime != null && this._timeFreezeRuntime.IsActiveFor(this._activeWearable)) || (!FlyCamera.IsActive && Time.timeScale > 0f && PlayerActivityGate.Allows(PlayerActivity.Inventory) && this.CanUseFocusedActionInCover(action) && (WallCoverActivityLock.IsPlayerInCover || (!PlayerActivityGate.IsResourceOwned(PlayerContextResource.FullBodyAnimation) && (!PlayerActivityGate.IsResourceOwned(PlayerContextResource.Rider) || CarRoofCarrier.IsCarSurfing(base.transform))))));
		}

		// Token: 0x0600858A RID: 34186 RVA: 0x003682C8 File Offset: 0x003664C8
		private void RefreshHud()
		{
			if (this._activeWearable == null || this._activeWearable.wearOnlyWhileSelected)
			{
				this.HideActionsHud();
				return;
			}
			if (WallCoverActivityLock.ActiveController != null)
			{
				InteractionHUDController instance = InteractionHUDController.Instance;
				if (instance == null)
				{
					return;
				}
				instance.HideIfCurrent(this._runtimeHudConfig);
				InteractionConfigSO interactionConfigSO = this.BuildHudConfig(this._activeWearable, this.IsFocusedInstanceWorn());
				if (interactionConfigSO.usableActions.Count > 0)
				{
					instance.ShowSupplemental(this, interactionConfigSO);
					this._showingCoverActions = true;
					return;
				}
				this.HideActionsHud();
				return;
			}
			else
			{
				if (this._showingCoverActions)
				{
					InteractionHUDController instance2 = InteractionHUDController.Instance;
					if (instance2 != null)
					{
						instance2.HideSupplemental(this);
					}
					this._showingCoverActions = false;
				}
				if (PlayerActivityGate.IsResourceOwned(PlayerContextResource.Rider) && !CarRoofCarrier.IsCarSurfing(base.transform))
				{
					this.HideActionsHud();
					return;
				}
				InteractionHUDController instance3 = InteractionHUDController.Instance;
				if (instance3 != null && instance3.CurrentConfig != null && instance3.CurrentConfig != this._runtimeHudConfig)
				{
					return;
				}
				InteractionConfigSO interactionConfigSO2 = this.BuildHudConfig(this._activeWearable, this.IsFocusedInstanceWorn());
				if (interactionConfigSO2 != null)
				{
					InteractionHUDController instance4 = InteractionHUDController.Instance;
					if (instance4 == null)
					{
						return;
					}
					instance4.Show(interactionConfigSO2, true);
				}
				return;
			}
		}

		// Token: 0x0600858B RID: 34187 RVA: 0x003683F4 File Offset: 0x003665F4
		private void MaintainFocusedActionsHud()
		{
			if (this._activeWearable != null && this._activeWearable.wearOnlyWhileSelected)
			{
				this.HideActionsHud();
				return;
			}
			if (this._activeWearable != null && !this._activeWearable.wearActions.Exists(new Predicate<WearableActionDefinition>(this.CanUseFocusedAction)))
			{
				this.HideActionsHud();
				return;
			}
			if (WallCoverActivityLock.ActiveController != null)
			{
				if (FlyCamera.IsActive)
				{
					this.HideActionsHud();
					return;
				}
				this.RefreshHud();
				return;
			}
			else
			{
				if (this._showingCoverActions)
				{
					this.HideActionsHud();
				}
				if (this._activeWearable == null || FlyCamera.IsActive)
				{
					return;
				}
				InteractionHUDController instance = InteractionHUDController.Instance;
				if (instance == null)
				{
					return;
				}
				if (instance.CurrentConfig != null)
				{
					return;
				}
				if (TimeFreezeController.IsAnyActive || PlayerActivityGate.Allows(PlayerActivity.Interact))
				{
					this.RefreshHud();
				}
				return;
			}
		}

		// Token: 0x0600858C RID: 34188 RVA: 0x003684D4 File Offset: 0x003666D4
		private void HideActionsHud()
		{
			if (this._showingCoverActions)
			{
				InteractionHUDController instance = InteractionHUDController.Instance;
				if (instance != null)
				{
					instance.HideSupplemental(this);
				}
				this._showingCoverActions = false;
			}
			if (this._runtimeHudConfig != null)
			{
				InteractionHUDController instance2 = InteractionHUDController.Instance;
				if (instance2 == null)
				{
					return;
				}
				instance2.HideIfCurrent(this._runtimeHudConfig);
			}
		}

		// Token: 0x0600858D RID: 34189 RVA: 0x00368525 File Offset: 0x00366725
		private void HandleItemDurabilityChanged(int slotIndex, float durability01)
		{
			this.RefreshChargeHint(WearableUseController.WearableSource.Item, slotIndex);
		}

		// Token: 0x0600858E RID: 34190 RVA: 0x0036852F File Offset: 0x0036672F
		private void HandleClothDurabilityChanged(int slotIndex, float durability01)
		{
			this.RefreshChargeHint(WearableUseController.WearableSource.Cloth, slotIndex);
		}

		// Token: 0x0600858F RID: 34191 RVA: 0x0036853C File Offset: 0x0036673C
		private void RefreshChargeHint(WearableUseController.WearableSource source, int slotIndex)
		{
			if (this._activeWearable == null || this._activeSource != source || this._activeSlotIndex != slotIndex)
			{
				return;
			}
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance == null || this._runtimeHudConfig == null || instance.CurrentConfig != this._runtimeHudConfig)
			{
				return;
			}
			foreach (WearableActionDefinition wearableActionDefinition in this._activeWearable.wearActions)
			{
				if (!(wearableActionDefinition == null) && wearableActionDefinition.ActionType == WearableActionType.TimeFreeze)
				{
					this.RefreshHud();
					break;
				}
			}
		}

		// Token: 0x06008590 RID: 34192 RVA: 0x003685FC File Offset: 0x003667FC
		private InteractionConfigSO BuildHudConfig(WearableItemSO wearable, bool worn)
		{
			if (this._runtimeHudConfig == null)
			{
				this._runtimeHudConfig = ScriptableObject.CreateInstance<InteractionConfigSO>();
			}
			this._runtimeHudConfig.objectName = ((!string.IsNullOrEmpty(wearable.itemName)) ? wearable.itemName : "Wearable");
			this._runtimeHudConfig.mainActionName = string.Empty;
			this._runtimeHudConfig.mainKeyDisplayLabel = string.Empty;
			this._runtimeHudConfig.usableActions.Clear();
			for (int i = 0; i < wearable.wearActions.Count; i++)
			{
				WearableActionDefinition wearableActionDefinition = wearable.wearActions[i];
				if (!(wearableActionDefinition == null) && this.IsActionVisible(wearableActionDefinition, worn))
				{
					string text = this.ResolveActionLabel(wearable, wearableActionDefinition);
					this._runtimeHudConfig.usableActions.Add(new UsableAction
					{
						actionName = text,
						keyDisplayLabel = wearableActionDefinition.KeyDisplayLabel,
						description = wearableActionDefinition.Description
					});
				}
			}
			return this._runtimeHudConfig;
		}

		// Token: 0x06008591 RID: 34193 RVA: 0x003686F0 File Offset: 0x003668F0
		private string ResolveActionLabel(WearableItemSO wearable, WearableActionDefinition action)
		{
			if (action.ActionType == WearableActionType.TimeFreeze)
			{
				if (this._timeFreezeRuntime != null && this._timeFreezeRuntime.IsActiveFor(wearable) && !string.IsNullOrEmpty(action.DisplayNameWhenActive))
				{
					return action.DisplayNameWhenActive;
				}
				return string.Format("{0} ({1})", action.DisplayName, this.GetWornInventoryUsesRemaining(wearable));
			}
			else
			{
				if (action.ActionType != WearableActionType.SecondaryEffectToggle)
				{
					return action.DisplayName;
				}
				if (string.IsNullOrEmpty(action.DisplayNameWhenActive))
				{
					return action.DisplayName;
				}
				WearableUseController.WornEntry wornEntry;
				if (!this._worn.TryGetValue(wearable, out wornEntry))
				{
					return action.DisplayName;
				}
				WearableUseController.SecondaryEffectRuntime secondaryEffectRuntime;
				if (!wornEntry.secondaryRuntimes.TryGetValue(action.SecondaryEffectId, out secondaryEffectRuntime))
				{
					return action.DisplayName;
				}
				if (!secondaryEffectRuntime.isActive)
				{
					return action.DisplayName;
				}
				return action.DisplayNameWhenActive;
			}
		}

		// Token: 0x06008592 RID: 34194 RVA: 0x003687C4 File Offset: 0x003669C4
		private int GetWornInventoryUsesRemaining(WearableItemSO wearable)
		{
			WearableUseController.WornEntry wornEntry;
			if (wearable == null || !this._worn.TryGetValue(wearable, out wornEntry))
			{
				return 0;
			}
			PlayerInventoryManager playerInventoryManager = ((this.inventory != null) ? this.inventory : PlayerInventoryManager.Instance);
			if (playerInventoryManager == null || !wearable.hasDurability || wearable.maxDurability <= 0f)
			{
				return 0;
			}
			float num;
			if (wornEntry.source == WearableUseController.WearableSource.Item && playerInventoryManager.GetItemAt(wornEntry.slotIndex) == wearable)
			{
				num = playerInventoryManager.GetItemDurability01(wornEntry.slotIndex) * wearable.maxDurability;
			}
			else
			{
				if (wornEntry.source != WearableUseController.WearableSource.Cloth || !(playerInventoryManager.GetClothAt(wornEntry.slotIndex) == wearable))
				{
					return 0;
				}
				num = playerInventoryManager.GetClothDurability(wornEntry.slotIndex);
			}
			return Mathf.Max(0, Mathf.FloorToInt(num + 0.0001f));
		}

		// Token: 0x06008593 RID: 34195 RVA: 0x0036889B File Offset: 0x00366A9B
		internal void RefreshFocusedActionsHud()
		{
			if (this._activeWearable != null)
			{
				this.RefreshHud();
			}
		}

		// Token: 0x06008594 RID: 34196 RVA: 0x003688B4 File Offset: 0x00366AB4
		public void AppendVisibleWearActions(WearableItemSO item, List<UsableAction> into)
		{
			if (item == null || item.wearActions == null || into == null)
			{
				return;
			}
			bool flag = ((item == this._activeWearable) ? this.IsFocusedInstanceWorn() : this.IsWorn(item));
			for (int i = 0; i < item.wearActions.Count; i++)
			{
				WearableActionDefinition wearableActionDefinition = item.wearActions[i];
				if (!(wearableActionDefinition == null) && this.IsActionVisible(wearableActionDefinition, flag))
				{
					into.Add(new UsableAction
					{
						actionName = this.ResolveActionLabel(item, wearableActionDefinition),
						keyDisplayLabel = wearableActionDefinition.KeyDisplayLabel,
						description = wearableActionDefinition.Description
					});
				}
			}
		}

		// Token: 0x06008595 RID: 34197 RVA: 0x0036895C File Offset: 0x00366B5C
		public void AppendVisibleWearActions(WearableItemSO item, InteractionActionBuffer into)
		{
			if (item == null || item.wearActions == null || into == null)
			{
				return;
			}
			bool flag = ((item == this._activeWearable) ? this.IsFocusedInstanceWorn() : this.IsWorn(item));
			for (int i = 0; i < item.wearActions.Count; i++)
			{
				WearableActionDefinition wearableActionDefinition = item.wearActions[i];
				if (!(wearableActionDefinition == null) && this.IsActionVisible(wearableActionDefinition, flag))
				{
					into.Add(wearableActionDefinition.KeyDisplayLabel, this.ResolveActionLabel(item, wearableActionDefinition), wearableActionDefinition.Description);
				}
			}
		}

		// Token: 0x0400881A RID: 34842
		[Header("References")]
		[SerializeField]
		private PlayerInventoryManager inventory;

		// Token: 0x0400881B RID: 34843
		[SerializeField]
		private ClothingToggleController clothingController;

		// Token: 0x0400881C RID: 34844
		[Tooltip("Player's MCStyle service - canonical wear path. Auto-resolved from the rig if left empty.")]
		[SerializeField]
		private MCStyleCustomizationService styleService;

		// Token: 0x0400881D RID: 34845
		[Tooltip("URP post-process Volume hosting the Vignette override driven by wearables.")]
		[SerializeField]
		private Volume postProcessVolume;

		// Token: 0x0400881E RID: 34846
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x0400881F RID: 34847
		[HideInInspector]
		[SerializeField]
		private BlendShapePriority blendShapePriority = BlendShapePriority.Vehicle;

		// Token: 0x04008820 RID: 34848
		private WearableUseController.MirrorRemoval _mirrorRemoval;

		// Token: 0x04008821 RID: 34849
		private readonly Dictionary<WearableItemSO, WearableUseController.WornEntry> _worn = new Dictionary<WearableItemSO, WearableUseController.WornEntry>();

		// Token: 0x04008824 RID: 34852
		private WearableItemSO _activeWearable;

		// Token: 0x04008825 RID: 34853
		private int _activeSlotIndex = -1;

		// Token: 0x04008826 RID: 34854
		private WearableUseController.WearableSource _activeSource;

		// Token: 0x04008827 RID: 34855
		private InteractionConfigSO _runtimeHudConfig;

		// Token: 0x04008828 RID: 34856
		private TimeFreezeController _timeFreezeRuntime;

		// Token: 0x04008829 RID: 34857
		private Vignette _vignette;

		// Token: 0x0400882A RID: 34858
		private Coroutine _vignetteRoutine;

		// Token: 0x0400882B RID: 34859
		private float _wearableVignetteIntensity;

		// Token: 0x0400882C RID: 34860
		private BlendShapeManager _rigBlendShapeManager;

		// Token: 0x0400882D RID: 34861
		private string _armActionsLockReason;

		// Token: 0x0400882E RID: 34862
		private PlayerActivity _appliedWornBlockedActivities;

		// Token: 0x0400882F RID: 34863
		private bool _showingCoverActions;

		// Token: 0x04008830 RID: 34864
		private bool _applyingWearVisual;

		// Token: 0x04008831 RID: 34865
		private readonly Dictionary<WearableItemSO, WearableUseController.WornEntry> _externallyHiddenWorn = new Dictionary<WearableItemSO, WearableUseController.WornEntry>();

		// Token: 0x04008832 RID: 34866
		private static readonly int EmptyStateHash = Animator.StringToHash("Empty");

		// Token: 0x04008833 RID: 34867
		private ClothingBlendShapeSyncController _blendShapeSync;

		// Token: 0x0200112D RID: 4397
		private enum WearableSource
		{
			// Token: 0x04008835 RID: 34869
			None,
			// Token: 0x04008836 RID: 34870
			Item,
			// Token: 0x04008837 RID: 34871
			Cloth
		}

		// Token: 0x0200112E RID: 4398
		private sealed class MirrorRemoval
		{
			// Token: 0x04008838 RID: 34872
			public WearableItemSO item;

			// Token: 0x04008839 RID: 34873
			public WearableUseController.WearableSource source;

			// Token: 0x0400883A RID: 34874
			public int slotIndex;
		}

		// Token: 0x0200112F RID: 4399
		private class SecondaryEffectRuntime
		{
			// Token: 0x0400883B RID: 34875
			public WearableSecondaryEffect data;

			// Token: 0x0400883C RID: 34876
			public bool isActive;

			// Token: 0x0400883D RID: 34877
			public float progress;

			// Token: 0x0400883E RID: 34878
			public Coroutine routine;
		}

		// Token: 0x02001130 RID: 4400
		private class WornEntry
		{
			// Token: 0x0400883F RID: 34879
			public WearableItemSO item;

			// Token: 0x04008840 RID: 34880
			public int slotIndex;

			// Token: 0x04008841 RID: 34881
			public WearableUseController.WearableSource source;

			// Token: 0x04008842 RID: 34882
			public MCStyleCustomizationService.SlotAppearanceSnapshot displacedCosmetic;

			// Token: 0x04008843 RID: 34883
			public float wearTimeRemaining = -1f;

			// Token: 0x04008844 RID: 34884
			public readonly List<Material> outfitMaterials = new List<Material>();

			// Token: 0x04008845 RID: 34885
			public readonly Dictionary<string, WearableUseController.SecondaryEffectRuntime> secondaryRuntimes = new Dictionary<string, WearableUseController.SecondaryEffectRuntime>();
		}
	}
}
