using System;
using System.Collections;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.Rank;
using Invector.vCharacterController;
using ProjectExhibitionist;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using UnityEngine;

namespace Features.Prison
{
	// Token: 0x02000DCA RID: 3530
	public class PrisonNakedOverride : MonoBehaviour
	{
		// Token: 0x17001126 RID: 4390
		// (get) Token: 0x0600684B RID: 26699 RVA: 0x0029B7BE File Offset: 0x002999BE
		// (set) Token: 0x0600684C RID: 26700 RVA: 0x0029B7C6 File Offset: 0x002999C6
		public bool IsReady { get; private set; }

		// Token: 0x0600684D RID: 26701 RVA: 0x0029B7CF File Offset: 0x002999CF
		private void OnEnable()
		{
			this.IsReady = false;
			if (Application.isPlaying)
			{
				this._initializationRoutine = base.StartCoroutine(this.InitializeWhenReady());
			}
		}

		// Token: 0x0600684E RID: 26702 RVA: 0x0029B7F1 File Offset: 0x002999F1
		private IEnumerator InitializeWhenReady()
		{
			yield return null;
			yield return null;
			MainCharacterAppearanceApplier componentInChildren = base.GetComponentInChildren<MainCharacterAppearanceApplier>(true);
			if (componentInChildren != null)
			{
				componentInChildren.ReloadAppearance();
			}
			else
			{
				Debug.LogWarning("[PrisonNakedOverride] MainCharacterAppearanceApplier non trovato: preset non ri-applicato.", this);
			}
			this.ResolveReferences();
			this.DisableClothingInputs();
			this.StripFunctionalWornSlots();
			this.ClearWardrobeInventory();
			this.IsReady = true;
			this._initializationRoutine = null;
			yield break;
		}

		// Token: 0x0600684F RID: 26703 RVA: 0x0029B800 File Offset: 0x00299A00
		private void LateUpdate()
		{
			this.StripFunctionalWornSlots();
			this.ClearWardrobeInventory();
			this.DisableClothingInputs();
		}

		// Token: 0x06006850 RID: 26704 RVA: 0x0029B814 File Offset: 0x00299A14
		private void ResolveReferences()
		{
			if (this.clothingToggleController == null)
			{
				this.clothingToggleController = base.GetComponentInChildren<ClothingToggleController>(true);
			}
			if (this._playerInput == null)
			{
				this._playerInput = base.GetComponentInChildren<vThirdPersonInput>(true);
			}
			if (this._style == null)
			{
				this._style = base.GetComponentInChildren<MCStyleCustomizationService>(true);
			}
			if (this._inventory == null)
			{
				this._inventory = base.GetComponentInChildren<PlayerInventoryManager>(true);
			}
		}

		// Token: 0x06006851 RID: 26705 RVA: 0x0029B890 File Offset: 0x00299A90
		private void ClearWardrobeInventory()
		{
			if (this._inventory == null)
			{
				return;
			}
			for (int i = 0; i < this._inventory.MaxClothSlots; i++)
			{
				InventoryItemSO clothAt = this._inventory.GetClothAt(i);
				if (!(clothAt == null))
				{
					WearableItemSO wearableItemSO = clothAt as WearableItemSO;
					if (wearableItemSO == null || PrisonNakedOverride.IsFunctionalOutfitSlot(wearableItemSO.mcStylePartId))
					{
						this._inventory.RemoveCloth(i);
					}
				}
			}
			for (int j = 0; j < this._inventory.MaxItemSlots; j++)
			{
				WearableItemSO wearableItemSO2 = this._inventory.GetItemAt(j) as WearableItemSO;
				if (wearableItemSO2 != null && PrisonNakedOverride.IsFunctionalOutfitSlot(wearableItemSO2.mcStylePartId))
				{
					this._inventory.DropEntireItemStack(j);
				}
			}
		}

		// Token: 0x06006852 RID: 26706 RVA: 0x0029B944 File Offset: 0x00299B44
		private void StripFunctionalWornSlots()
		{
			this.ResolveReferences();
			if (this._style != null)
			{
				foreach (MCStylePartDefinitionSO mcstylePartDefinitionSO in this._style.PartDefinitions)
				{
					if (!(mcstylePartDefinitionSO == null) && !mcstylePartDefinitionSO.isBodyAppearance && RankWardrobePolicy.IsWardrobePart(mcstylePartDefinitionSO.partId))
					{
						string partId = mcstylePartDefinitionSO.partId;
						GameObject spawnedRoot = this._style.GetSpawnedRoot(partId);
						if (this.clothingToggleController != null && this.clothingToggleController.ManagesSlot(partId))
						{
							if (this.clothingToggleController.IsSlotVisible(partId) || (spawnedRoot != null && spawnedRoot.activeSelf))
							{
								this.clothingToggleController.SetSlotVisible(partId, false);
							}
						}
						else if (spawnedRoot != null && spawnedRoot.activeSelf)
						{
							this._style.SetSlotPhysicallyHidden(partId, true);
						}
					}
				}
			}
			if (this.clothingToggleController == null)
			{
				Debug.LogWarning("[PrisonNakedOverride] ClothingToggleController non trovato: player non spogliato.", this);
				return;
			}
			foreach (string text in new List<string>(this.clothingToggleController.ManagedSlots))
			{
				if (PrisonNakedOverride.IsFunctionalOutfitSlot(text) && this.clothingToggleController.IsSlotVisible(text))
				{
					this.clothingToggleController.SetSlotVisible(text, false);
					if (this.enableDebugLogs)
					{
						Debug.Log("[PrisonNakedOverride] Nascosto outfit '" + text + "'.", this);
					}
				}
			}
		}

		// Token: 0x06006853 RID: 26707 RVA: 0x0029BAF4 File Offset: 0x00299CF4
		private static bool IsFunctionalOutfitSlot(string partId)
		{
			return RankWardrobePolicy.IsWardrobePart(partId);
		}

		// Token: 0x06006854 RID: 26708 RVA: 0x0029BAFC File Offset: 0x00299CFC
		private void DisableClothingInputs()
		{
			if (this._playerInput == null || this._inputCaptured)
			{
				return;
			}
			this._inputCaptured = true;
			if (this._playerInput.toggleAllClothingInput != null)
			{
				this._previousToggleAllInputEnabled = this._playerInput.toggleAllClothingInput.useInput;
				this._playerInput.toggleAllClothingInput.useInput = false;
			}
			GenericInput[] toggleGroupInputs = this._playerInput.toggleGroupInputs;
			if (toggleGroupInputs == null)
			{
				return;
			}
			this._previousGroupInputEnabled = new bool[toggleGroupInputs.Length];
			for (int i = 0; i < toggleGroupInputs.Length; i++)
			{
				if (toggleGroupInputs[i] != null)
				{
					this._previousGroupInputEnabled[i] = toggleGroupInputs[i].useInput;
					toggleGroupInputs[i].useInput = false;
				}
			}
		}

		// Token: 0x06006855 RID: 26709 RVA: 0x0029BBA8 File Offset: 0x00299DA8
		private void OnDisable()
		{
			if (this._initializationRoutine != null)
			{
				base.StopCoroutine(this._initializationRoutine);
			}
			this._initializationRoutine = null;
			this.IsReady = false;
			if (!this._inputCaptured || this._playerInput == null)
			{
				return;
			}
			if (this._playerInput.toggleAllClothingInput != null)
			{
				this._playerInput.toggleAllClothingInput.useInput = this._previousToggleAllInputEnabled;
			}
			GenericInput[] toggleGroupInputs = this._playerInput.toggleGroupInputs;
			if (toggleGroupInputs != null && this._previousGroupInputEnabled != null)
			{
				int num = Mathf.Min(toggleGroupInputs.Length, this._previousGroupInputEnabled.Length);
				for (int i = 0; i < num; i++)
				{
					if (toggleGroupInputs[i] != null)
					{
						toggleGroupInputs[i].useInput = this._previousGroupInputEnabled[i];
					}
				}
			}
			this._inputCaptured = false;
		}

		// Token: 0x04006AC5 RID: 27333
		[Tooltip("Se nullo: risolto con GetComponentInChildren sul player.")]
		[SerializeField]
		private ClothingToggleController clothingToggleController;

		// Token: 0x04006AC6 RID: 27334
		[SerializeField]
		private bool enableDebugLogs;

		// Token: 0x04006AC7 RID: 27335
		private vThirdPersonInput _playerInput;

		// Token: 0x04006AC8 RID: 27336
		private bool _previousToggleAllInputEnabled;

		// Token: 0x04006AC9 RID: 27337
		private bool[] _previousGroupInputEnabled;

		// Token: 0x04006ACA RID: 27338
		private bool _inputCaptured;

		// Token: 0x04006ACB RID: 27339
		private MCStyleCustomizationService _style;

		// Token: 0x04006ACC RID: 27340
		private PlayerInventoryManager _inventory;

		// Token: 0x04006ACD RID: 27341
		private Coroutine _initializationRoutine;
	}
}
