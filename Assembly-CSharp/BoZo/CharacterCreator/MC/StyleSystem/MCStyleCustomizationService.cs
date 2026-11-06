using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using Features.Inventory.Wearable;
using Features.NeckSigns;
using Features.NipplePiercings;
using Features.Rank;
using MagicaCloth2;
using ProjectExhibitionist;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using ProjectExhibitionist.UI;
using RayznGames.DressRigger;
using UnityEngine;

namespace BoZo.CharacterCreator.MC.StyleSystem
{
	// Token: 0x020007E4 RID: 2020
	public class MCStyleCustomizationService : MonoBehaviour
	{
		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x06003581 RID: 13697 RVA: 0x00130ED4 File Offset: 0x0012F0D4
		public bool IsPreviewClothingHidden
		{
			get
			{
				return this._previewClothingHidden;
			}
		}

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x06003582 RID: 13698 RVA: 0x00130EDC File Offset: 0x0012F0DC
		public bool EnforcesRankWardrobe
		{
			get
			{
				return this._enforceCustomizationWardrobe && RankWardrobePolicy.IsRestricted;
			}
		}

		// Token: 0x06003583 RID: 13699 RVA: 0x00130EED File Offset: 0x0012F0ED
		public void SetCustomizationPreview(bool enabled)
		{
			if (enabled && base.GetComponent<MCPubicHairPreviewMask>() == null)
			{
				base.gameObject.AddComponent<MCPubicHairPreviewMask>();
			}
			if (!enabled)
			{
				this.SetPreviewClothingHidden(false);
			}
			this._enforceCustomizationWardrobe = enabled;
			if (enabled)
			{
				this.EnforceCustomizationWardrobe();
			}
			this.RefreshShrinkForVisibleParts();
		}

		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x06003584 RID: 13700 RVA: 0x00130F2C File Offset: 0x0012F12C
		public MCStylePartDefinitionSO ActiveDefinition
		{
			get
			{
				if (this._activeState == null)
				{
					return null;
				}
				return this._activeState.definition;
			}
		}

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x06003585 RID: 13701 RVA: 0x00130F43 File Offset: 0x0012F143
		public IReadOnlyList<MCStylePartDefinitionSO> PartDefinitions
		{
			get
			{
				return this.partDefinitions;
			}
		}

		// Token: 0x14000059 RID: 89
		// (add) Token: 0x06003586 RID: 13702 RVA: 0x00130F4C File Offset: 0x0012F14C
		// (remove) Token: 0x06003587 RID: 13703 RVA: 0x00130F84 File Offset: 0x0012F184
		public event Action OnActiveStateChanged;

		// Token: 0x1400005A RID: 90
		// (add) Token: 0x06003588 RID: 13704 RVA: 0x00130FBC File Offset: 0x0012F1BC
		// (remove) Token: 0x06003589 RID: 13705 RVA: 0x00130FF4 File Offset: 0x0012F1F4
		public event Action<string> OnNeckSignDesignsChanged;

		// Token: 0x1400005B RID: 91
		// (add) Token: 0x0600358A RID: 13706 RVA: 0x0013102C File Offset: 0x0012F22C
		// (remove) Token: 0x0600358B RID: 13707 RVA: 0x00131064 File Offset: 0x0012F264
		public event Action OnSlotVisibilityChanged;

		// Token: 0x1400005C RID: 92
		// (add) Token: 0x0600358C RID: 13708 RVA: 0x0013109C File Offset: 0x0012F29C
		// (remove) Token: 0x0600358D RID: 13709 RVA: 0x001310D4 File Offset: 0x0012F2D4
		public event Action OnPreviewClothingVisibilityChanged;

		// Token: 0x1400005D RID: 93
		// (add) Token: 0x0600358E RID: 13710 RVA: 0x0013110C File Offset: 0x0012F30C
		// (remove) Token: 0x0600358F RID: 13711 RVA: 0x00131144 File Offset: 0x0012F344
		public event Action<MCStylePartDefinitionSO, GameObject> OnPhysicalSwapCompleted;

		// Token: 0x1400005E RID: 94
		// (add) Token: 0x06003590 RID: 13712 RVA: 0x0013117C File Offset: 0x0012F37C
		// (remove) Token: 0x06003591 RID: 13713 RVA: 0x001311B4 File Offset: 0x0012F3B4
		public event Action<MCStylePartDefinitionSO, GameObject> OnPhysicalRemoved;

		// Token: 0x06003592 RID: 13714 RVA: 0x001311EC File Offset: 0x0012F3EC
		private void OnEnable()
		{
			if (Application.isPlaying && base.GetComponent<NeckSignActionsUI>() == null)
			{
				base.gameObject.AddComponent<NeckSignActionsUI>().Configure(this);
			}
			if (base.GetComponentInParent<CustomizationPreviewPhysicsGuard>(true) != null && base.GetComponent<MCPubicHairPreviewMask>() == null)
			{
				base.gameObject.AddComponent<MCPubicHairPreviewMask>();
			}
			RankProgressionService.Changed += this.RemoveUnavailableAccessories;
			this.RemoveUnavailableAccessories();
			this.RefreshHairMorphs();
		}

		// Token: 0x06003593 RID: 13715 RVA: 0x00131265 File Offset: 0x0012F465
		private static bool IsAccessoryPresetAvailable(MCStylePresetSO preset)
		{
			return CollarAvailabilityPolicy.IsPresetAvailable(preset) && NipplePiercingRankPolicy.IsPresetAvailable(preset) && LeadChokerAvailabilityPolicy.IsPresetAvailable(preset) && HalloweenWardrobePolicy.IsPresetAvailable(preset);
		}

		// Token: 0x06003594 RID: 13716 RVA: 0x00131288 File Offset: 0x0012F488
		private void RemoveUnavailableAccessories()
		{
			foreach (MCStyleCustomizationService.StyleRuntimeState styleRuntimeState in this._stateByPartId.Values)
			{
				MCStylePresetSO mcstylePresetSO;
				if (styleRuntimeState != null && styleRuntimeState.definition != null && styleRuntimeState.definition.TryGetStylePreset(styleRuntimeState.styleIndex, out mcstylePresetSO) && !MCStyleCustomizationService.IsAccessoryPresetAvailable(mcstylePresetSO))
				{
					this.ApplyState(styleRuntimeState, true);
				}
			}
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x00131310 File Offset: 0x0012F510
		private void Start()
		{
			if (this.initializeOnStart)
			{
				this.EnsureRuntimeStateInitialized();
			}
		}

		// Token: 0x06003596 RID: 13718 RVA: 0x00131320 File Offset: 0x0012F520
		private void EnsureRuntimeStateInitialized()
		{
			if (this._stateByPartId.Count == 0)
			{
				this.InitializeRuntimeState();
			}
		}

		// Token: 0x06003597 RID: 13719 RVA: 0x00131338 File Offset: 0x0012F538
		public void ResetAllPartsToDefaults()
		{
			this.EnsureRuntimeStateInitialized();
			this.ClearHairMorphValues();
			this._neckSignDesigns.Clear();
			Action<string> onNeckSignDesignsChanged = this.OnNeckSignDesignsChanged;
			if (onNeckSignDesignsChanged != null)
			{
				onNeckSignDesignsChanged(null);
			}
			this._shaderSlidersByPartId.Clear();
			foreach (KeyValuePair<string, MCStyleCustomizationService.StyleRuntimeState> keyValuePair in this._stateByPartId)
			{
				MCStyleCustomizationService.StyleRuntimeState value = keyValuePair.Value;
				if (value != null && !(value.definition == null))
				{
					MCStyleCustomizationService.ApplyDefinitionDefaultSelection(value, value.definition);
					value.hasCustomPrimary = false;
					value.hasCustomSecondary = false;
					this.ApplyState(value, false);
				}
			}
			this.ApplyNeckSignDesignsToSpawned();
			if (this.EnforcesRankWardrobe)
			{
				this.EnforceCustomizationWardrobe();
			}
			Action onActiveStateChanged = this.OnActiveStateChanged;
			if (onActiveStateChanged == null)
			{
				return;
			}
			onActiveStateChanged();
		}

		// Token: 0x06003598 RID: 13720 RVA: 0x00131418 File Offset: 0x0012F618
		public void ResetForNewPresetDraft()
		{
			this.EnsureRuntimeStateInitialized();
			this.ClearHairMorphValues();
			this._neckSignDesigns.Clear();
			Action<string> onNeckSignDesignsChanged = this.OnNeckSignDesignsChanged;
			if (onNeckSignDesignsChanged != null)
			{
				onNeckSignDesignsChanged(null);
			}
			this._shaderSlidersByPartId.Clear();
			foreach (KeyValuePair<string, MCStyleCustomizationService.StyleRuntimeState> keyValuePair in this._stateByPartId)
			{
				MCStyleCustomizationService.StyleRuntimeState value = keyValuePair.Value;
				if (value != null && !(value.definition == null))
				{
					if (value.definition.isBodyAppearance || string.Equals(value.definition.partId, "Head.Glasses", StringComparison.OrdinalIgnoreCase) || string.Equals(value.definition.partId, "Head.Headpin", StringComparison.OrdinalIgnoreCase) || string.Equals(value.definition.partId, "Accessory.Eyewear", StringComparison.OrdinalIgnoreCase) || string.Equals(value.definition.partId, "Accessory.Headpin", StringComparison.OrdinalIgnoreCase))
					{
						MCStyleCustomizationService.ApplyDefinitionDefaultSelection(value, value.definition);
					}
					else
					{
						value.isUsingNoSelection = true;
						value.isUsingSceneDefault = false;
						value.styleIndex = -1;
						value.surfaceIndex = -1;
						value.colorPresetIndex = -1;
					}
					value.hasCustomPrimary = false;
					value.hasCustomSecondary = false;
					this.ApplyState(value, false);
				}
			}
			this._isNaked = false;
			if (this.EnforcesRankWardrobe)
			{
				this.EnforceCustomizationWardrobe();
			}
			Action onActiveStateChanged = this.OnActiveStateChanged;
			if (onActiveStateChanged == null)
			{
				return;
			}
			onActiveStateChanged();
		}

		// Token: 0x06003599 RID: 13721 RVA: 0x0013159C File Offset: 0x0012F79C
		public void InitializeRuntimeState()
		{
			this.EnsureOwnedRuntimeDefinitions();
			MCStyleCustomizationService.StyleRuntimeState activeState = this._activeState;
			string text;
			if (activeState == null)
			{
				text = null;
			}
			else
			{
				MCStylePartDefinitionSO definition = activeState.definition;
				text = ((definition != null) ? definition.partId : null);
			}
			string text2 = text;
			this.SetPreviewClothingHidden(false);
			this.CleanupRuntimeSpawns();
			this.BuildVirtualRendererLookup();
			this._activeState = null;
			this._stateByPartId.Clear();
			for (int i = 0; i < this.partDefinitions.Count; i++)
			{
				MCStylePartDefinitionSO mcstylePartDefinitionSO = this.partDefinitions[i];
				if (mcstylePartDefinitionSO == null || string.IsNullOrWhiteSpace(mcstylePartDefinitionSO.partId))
				{
					Debug.LogWarning(string.Format("[MCStyleService] Skipping null/empty definition at index {0}", i));
				}
				else
				{
					MCStyleCustomizationService.StyleRuntimeState styleRuntimeState = new MCStyleCustomizationService.StyleRuntimeState
					{
						definition = mcstylePartDefinitionSO,
						styleIndex = -1,
						surfaceIndex = -1,
						colorPresetIndex = -1
					};
					MCStyleCustomizationService.ApplyDefinitionDefaultSelection(styleRuntimeState, mcstylePartDefinitionSO);
					this._stateByPartId[mcstylePartDefinitionSO.partId] = styleRuntimeState;
					this.ApplyState(styleRuntimeState, false);
				}
			}
			if (!string.IsNullOrEmpty(text2))
			{
				this._stateByPartId.TryGetValue(text2, out this._activeState);
			}
			if (this._activeState == null && this.partDefinitions.Count > 0)
			{
				for (int j = 0; j < this.partDefinitions.Count; j++)
				{
					MCStyleCustomizationService.StyleRuntimeState styleRuntimeState2;
					if (this.partDefinitions[j] != null && this._stateByPartId.TryGetValue(this.partDefinitions[j].partId, out styleRuntimeState2))
					{
						this._activeState = styleRuntimeState2;
						break;
					}
				}
			}
			Action onActiveStateChanged = this.OnActiveStateChanged;
			if (onActiveStateChanged == null)
			{
				return;
			}
			onActiveStateChanged();
		}

		// Token: 0x0600359A RID: 13722 RVA: 0x00131728 File Offset: 0x0012F928
		private MCStyleCustomizationService.StyleRuntimeState ResolveOrCreateState(string partId)
		{
			if (string.IsNullOrWhiteSpace(partId))
			{
				return null;
			}
			MCStyleCustomizationService.StyleRuntimeState styleRuntimeState;
			if (this._stateByPartId.TryGetValue(partId, out styleRuntimeState) && styleRuntimeState != null)
			{
				return styleRuntimeState;
			}
			for (int i = 0; i < this.partDefinitions.Count; i++)
			{
				MCStylePartDefinitionSO mcstylePartDefinitionSO = this.partDefinitions[i];
				if (!(mcstylePartDefinitionSO == null) && !string.IsNullOrWhiteSpace(mcstylePartDefinitionSO.partId) && string.Equals(mcstylePartDefinitionSO.partId, partId, StringComparison.OrdinalIgnoreCase))
				{
					MCStyleCustomizationService.StyleRuntimeState styleRuntimeState2 = new MCStyleCustomizationService.StyleRuntimeState
					{
						definition = mcstylePartDefinitionSO,
						styleIndex = -1,
						surfaceIndex = -1,
						colorPresetIndex = -1
					};
					MCStyleCustomizationService.ApplyDefinitionDefaultSelection(styleRuntimeState2, mcstylePartDefinitionSO);
					this._stateByPartId[mcstylePartDefinitionSO.partId] = styleRuntimeState2;
					return styleRuntimeState2;
				}
			}
			return null;
		}

		// Token: 0x0600359B RID: 13723 RVA: 0x001317E0 File Offset: 0x0012F9E0
		private void OnDestroy()
		{
			this.CleanupRuntimeSpawns();
			Animator componentInParent = base.GetComponentInParent<Animator>();
			this.RestoreOwnedMaterials(((componentInParent != null) ? componentInParent.transform : null) ?? base.transform);
			foreach (Renderer renderer in new List<Renderer>(this._ownedMaterialRenderers))
			{
				if (renderer != null)
				{
					this.RestoreOwnedMaterials(renderer.transform);
				}
			}
			this.ReleaseUnusedOwnedMaterials();
			this._materialInstanceCache.Clear();
			this._ownedMaterialRenderers.Clear();
			this.ReleaseOwnedRuntimeDefinitions();
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x00131890 File Offset: 0x0012FA90
		private void EnsureOwnedRuntimeDefinitions()
		{
			if (!this.useOwnedRuntimeDefinitions || this._runtimeDefinitionsOwned)
			{
				return;
			}
			Dictionary<MCStylePartDefinitionSO, MCStylePartDefinitionSO> dictionary = new Dictionary<MCStylePartDefinitionSO, MCStylePartDefinitionSO>();
			List<MCStylePartDefinitionSO> list = new List<MCStylePartDefinitionSO>(this.partDefinitions.Count);
			foreach (MCStylePartDefinitionSO mcstylePartDefinitionSO in this.partDefinitions)
			{
				if (mcstylePartDefinitionSO == null)
				{
					list.Add(null);
				}
				else
				{
					MCStylePartDefinitionSO mcstylePartDefinitionSO2;
					if (!dictionary.TryGetValue(mcstylePartDefinitionSO, out mcstylePartDefinitionSO2))
					{
						mcstylePartDefinitionSO2 = Object.Instantiate<MCStylePartDefinitionSO>(mcstylePartDefinitionSO);
						mcstylePartDefinitionSO2.name = mcstylePartDefinitionSO.name + " (Runtime)";
						mcstylePartDefinitionSO2.hideFlags = HideFlags.DontSave;
						dictionary.Add(mcstylePartDefinitionSO, mcstylePartDefinitionSO2);
						this._ownedRuntimeDefinitions.Add(mcstylePartDefinitionSO2);
					}
					list.Add(mcstylePartDefinitionSO2);
				}
			}
			List<MCStyleCustomizationService.VirtualPartRendererBinding> list2 = new List<MCStyleCustomizationService.VirtualPartRendererBinding>(this.virtualPartRenderers.Count);
			foreach (MCStyleCustomizationService.VirtualPartRendererBinding virtualPartRendererBinding in this.virtualPartRenderers)
			{
				if (virtualPartRendererBinding == null)
				{
					list2.Add(null);
				}
				else
				{
					MCStylePartDefinitionSO mcstylePartDefinitionSO3 = virtualPartRendererBinding.definition;
					MCStylePartDefinitionSO mcstylePartDefinitionSO4;
					if (mcstylePartDefinitionSO3 != null && dictionary.TryGetValue(mcstylePartDefinitionSO3, out mcstylePartDefinitionSO4))
					{
						mcstylePartDefinitionSO3 = mcstylePartDefinitionSO4;
					}
					list2.Add(new MCStyleCustomizationService.VirtualPartRendererBinding
					{
						definition = mcstylePartDefinitionSO3,
						targetRenderers = ((virtualPartRendererBinding.targetRenderers != null) ? new List<MCStyleCustomizationService.VirtualPartRendererBinding.RendererReference>(virtualPartRendererBinding.targetRenderers) : null)
					});
				}
			}
			this.partDefinitions = list;
			this.virtualPartRenderers = list2;
			this._runtimeDefinitionsOwned = true;
		}

		// Token: 0x0600359D RID: 13725 RVA: 0x00131A34 File Offset: 0x0012FC34
		private void ReleaseOwnedRuntimeDefinitions()
		{
			foreach (MCStylePartDefinitionSO mcstylePartDefinitionSO in this._ownedRuntimeDefinitions)
			{
				if (!(mcstylePartDefinitionSO == null))
				{
					if (Application.isPlaying)
					{
						Object.Destroy(mcstylePartDefinitionSO);
					}
					else
					{
						Object.DestroyImmediate(mcstylePartDefinitionSO);
					}
				}
			}
			this._ownedRuntimeDefinitions.Clear();
		}

		// Token: 0x0600359E RID: 13726 RVA: 0x00131AAC File Offset: 0x0012FCAC
		private void BuildVirtualRendererLookup()
		{
			this._virtualRenderersByPartId.Clear();
			this._virtualRendererTargetsByPartId.Clear();
			for (int i = 0; i < this.virtualPartRenderers.Count; i++)
			{
				MCStyleCustomizationService.VirtualPartRendererBinding virtualPartRendererBinding = this.virtualPartRenderers[i];
				if (virtualPartRendererBinding != null && !(virtualPartRendererBinding.definition == null) && !string.IsNullOrWhiteSpace(virtualPartRendererBinding.definition.partId))
				{
					List<Renderer> list = new List<Renderer>();
					Dictionary<Renderer, MCSurfaceVariantSO.RendererTargetType> dictionary = new Dictionary<Renderer, MCSurfaceVariantSO.RendererTargetType>();
					for (int j = 0; j < virtualPartRendererBinding.targetRenderers.Count; j++)
					{
						MCStyleCustomizationService.VirtualPartRendererBinding.RendererReference rendererReference = virtualPartRendererBinding.targetRenderers[j];
						if (rendererReference != null && rendererReference.renderer != null)
						{
							if (!list.Contains(rendererReference.renderer))
							{
								list.Add(rendererReference.renderer);
							}
							dictionary[rendererReference.renderer] = rendererReference.rendererTarget;
						}
					}
					if (list.Count > 0)
					{
						this._virtualRenderersByPartId[virtualPartRendererBinding.definition.partId] = list;
						this._virtualRendererTargetsByPartId[virtualPartRendererBinding.definition.partId] = dictionary;
					}
				}
			}
		}

		// Token: 0x0600359F RID: 13727 RVA: 0x00131BD4 File Offset: 0x0012FDD4
		public bool TryGetPartUsesSceneDefault(string partId, out bool usesSceneDefault)
		{
			usesSceneDefault = true;
			MCStyleCustomizationService.StyleRuntimeState styleRuntimeState;
			if (string.IsNullOrWhiteSpace(partId) || !this._stateByPartId.TryGetValue(partId, out styleRuntimeState) || styleRuntimeState == null)
			{
				return false;
			}
			usesSceneDefault = styleRuntimeState.isUsingSceneDefault;
			return true;
		}

		// Token: 0x060035A0 RID: 13728 RVA: 0x00131C0C File Offset: 0x0012FE0C
		public bool TryGetEquippedItem(string partId, out string presetId)
		{
			presetId = null;
			MCStyleCustomizationService.StyleRuntimeState styleRuntimeState;
			if (string.IsNullOrWhiteSpace(partId) || !this._stateByPartId.TryGetValue(partId, out styleRuntimeState) || styleRuntimeState == null)
			{
				return false;
			}
			if (styleRuntimeState.isUsingSceneDefault || styleRuntimeState.isUsingNoSelection)
			{
				return false;
			}
			MCStylePresetSO mcstylePresetSO;
			if (styleRuntimeState.appliedPhysicalPreset != null)
			{
				presetId = styleRuntimeState.appliedPhysicalPreset.presetId;
			}
			else if (styleRuntimeState.definition != null && styleRuntimeState.definition.TryGetStylePreset(styleRuntimeState.styleIndex, out mcstylePresetSO) && mcstylePresetSO != null)
			{
				presetId = mcstylePresetSO.presetId;
			}
			return true;
		}

		// Token: 0x060035A1 RID: 13729 RVA: 0x00131CA0 File Offset: 0x0012FEA0
		public GameObject GetSpawnedRoot(string partId)
		{
			MCStyleCustomizationService.StyleRuntimeState styleRuntimeState;
			if (!string.IsNullOrWhiteSpace(partId) && this._stateByPartId.TryGetValue(partId, out styleRuntimeState) && styleRuntimeState != null)
			{
				return styleRuntimeState.spawnedGO;
			}
			return null;
		}

		// Token: 0x060035A2 RID: 13730 RVA: 0x00131CD0 File Offset: 0x0012FED0
		public MCStylePresetSO GetActivePreset(string partId)
		{
			MCStyleCustomizationService.StyleRuntimeState styleRuntimeState;
			if (string.IsNullOrWhiteSpace(partId) || !this._stateByPartId.TryGetValue(partId, out styleRuntimeState) || styleRuntimeState == null || styleRuntimeState.isUsingSceneDefault || styleRuntimeState.isUsingNoSelection)
			{
				return null;
			}
			if (styleRuntimeState.appliedPhysicalPreset != null)
			{
				return styleRuntimeState.appliedPhysicalPreset;
			}
			MCStylePresetSO mcstylePresetSO;
			if (styleRuntimeState.definition != null && styleRuntimeState.definition.TryGetStylePreset(styleRuntimeState.styleIndex, out mcstylePresetSO))
			{
				return mcstylePresetSO;
			}
			return null;
		}

		// Token: 0x060035A3 RID: 13731 RVA: 0x00131D48 File Offset: 0x0012FF48
		public bool IsSlotEquipped(string partId)
		{
			string text;
			return this.TryGetEquippedItem(partId, out text);
		}

		// Token: 0x060035A4 RID: 13732 RVA: 0x00131D60 File Offset: 0x0012FF60
		public bool SetSlotEquipped(string partId, bool equipped)
		{
			this.EnsureRuntimeStateInitialized();
			if (!equipped && !this.CanUnequipCustomizationSlot(partId))
			{
				this.ShowFirstRunOutfitRestriction();
				return false;
			}
			MCStyleCustomizationService.StyleRuntimeState styleRuntimeState = this.ResolveOrCreateState(partId);
			if (styleRuntimeState == null || styleRuntimeState.definition == null)
			{
				return false;
			}
			this._activeState = styleRuntimeState;
			if (equipped)
			{
				int num = styleRuntimeState.definition.ResolveDefaultStyleIndex();
				if (num < 0)
				{
					num = 0;
				}
				MCStylePresetSO mcstylePresetSO;
				if (!styleRuntimeState.definition.TryGetStylePreset(num, out mcstylePresetSO) || mcstylePresetSO == null || mcstylePresetSO.isVaulted)
				{
					return false;
				}
				if (!this.IsCustomizationStyleAllowed(styleRuntimeState.definition, mcstylePresetSO))
				{
					return false;
				}
				styleRuntimeState.isUsingNoSelection = false;
				styleRuntimeState.isUsingSceneDefault = false;
				styleRuntimeState.styleIndex = num;
				styleRuntimeState.surfaceIndex = mcstylePresetSO.ResolveDefaultSurfaceIndex();
				styleRuntimeState.colorPresetIndex = MCStyleCustomizationService.ResolveDefaultColorPresetIndex(mcstylePresetSO, styleRuntimeState.surfaceIndex);
				this.UnequipIncompatibleSlots(styleRuntimeState, mcstylePresetSO);
			}
			else
			{
				if (styleRuntimeState.definition.allowNoSelection)
				{
					styleRuntimeState.isUsingNoSelection = true;
					styleRuntimeState.isUsingSceneDefault = false;
				}
				else
				{
					styleRuntimeState.isUsingNoSelection = false;
					styleRuntimeState.isUsingSceneDefault = true;
				}
				styleRuntimeState.styleIndex = -1;
				styleRuntimeState.surfaceIndex = -1;
				styleRuntimeState.colorPresetIndex = -1;
			}
			this.ApplySelection(styleRuntimeState);
			return true;
		}

		// Token: 0x060035A5 RID: 13733 RVA: 0x00131E7C File Offset: 0x0013007C
		private bool CanUnequipCustomizationSlot(string partId)
		{
			if (!this.EnforcesRankWardrobe || !RankWardrobePolicy.IsWardrobePart(partId))
			{
				return true;
			}
			List<WearableItemSO> list = new List<WearableItemSO>();
			foreach (string text in this._stateByPartId.Keys)
			{
				if (!string.Equals(text, partId, StringComparison.OrdinalIgnoreCase))
				{
					WearableItemSO mappedWearable = CustomizationWardrobeDiscoveryPolicy.GetMappedWearable(this.GetActivePreset(text));
					if (mappedWearable != null && RankWardrobePolicy.IsWardrobePart(text))
					{
						list.Add(mappedWearable);
					}
				}
			}
			using (List<WearableItemSO>.Enumerator enumerator2 = RankWardrobePolicy.ResolveOutfit(list).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (string.Equals(enumerator2.Current.mcStylePartId, partId, StringComparison.OrdinalIgnoreCase))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060035A6 RID: 13734 RVA: 0x00131F68 File Offset: 0x00130168
		public int AddRuntimePreset(string partId, MCStylePresetSO preset)
		{
			this.EnsureRuntimeStateInitialized();
			if (preset == null || preset.isVaulted || !MCStyleCustomizationService.IsAccessoryPresetAvailable(preset))
			{
				return -1;
			}
			MCStylePartDefinitionSO definition = this.GetDefinition(partId);
			if (definition == null)
			{
				Debug.LogWarning("[MCStyleService] AddRuntimePreset: unknown partId '" + partId + "'. Add a MCStylePartDefinitionSO with this partId to the service's partDefinitions.");
				return -1;
			}
			if (definition.stylePresets == null)
			{
				Debug.LogWarning("[MCStyleService] AddRuntimePreset: definition '" + partId + "' has no stylePresets list.");
				return -1;
			}
			int num = definition.stylePresets.IndexOf(preset);
			if (num >= 0)
			{
				return num;
			}
			definition.stylePresets.Add(preset);
			return definition.stylePresets.Count - 1;
		}

		// Token: 0x060035A7 RID: 13735 RVA: 0x0013200C File Offset: 0x0013020C
		public bool EquipRuntimePreset(string partId, MCStylePresetSO preset)
		{
			int num = this.AddRuntimePreset(partId, preset);
			return num >= 0 && this.SelectStyleForSlot(partId, this.ResolveUiIndexForStyleIndex(partId, num));
		}

		// Token: 0x060035A8 RID: 13736 RVA: 0x00132038 File Offset: 0x00130238
		public int ResolveUiIndexForStyleIndex(string partId, int styleIndex)
		{
			MCStylePartDefinitionSO definition = this.GetDefinition(partId);
			if (definition == null || styleIndex < 0)
			{
				return styleIndex;
			}
			int num = (definition.allowNoSelection ? 1 : 0);
			return styleIndex + num;
		}

		// Token: 0x060035A9 RID: 13737 RVA: 0x0013206C File Offset: 0x0013026C
		public bool SelectStyleForSlot(string partId, int uiIndex)
		{
			this.EnsureRuntimeStateInitialized();
			MCStyleCustomizationService.StyleRuntimeState styleRuntimeState = this.ResolveOrCreateState(partId);
			if (styleRuntimeState == null)
			{
				return false;
			}
			this._activeState = styleRuntimeState;
			return this.SelectStyle(uiIndex);
		}

		// Token: 0x060035AA RID: 13738 RVA: 0x0013209C File Offset: 0x0013029C
		private MCStylePresetSO ResolveSlotPreset(MCStyleCustomizationService.StyleRuntimeState state)
		{
			if (state == null || state.definition == null)
			{
				return null;
			}
			int num = state.styleIndex;
			if (num < 0)
			{
				num = state.definition.ResolveDefaultStyleIndex();
			}
			MCStylePresetSO mcstylePresetSO;
			if (!state.definition.TryGetStylePreset(num, out mcstylePresetSO))
			{
				return null;
			}
			return mcstylePresetSO;
		}

		// Token: 0x060035AB RID: 13739 RVA: 0x001320E8 File Offset: 0x001302E8
		public int GetSlotSurfaceCount(string partId)
		{
			this.EnsureRuntimeStateInitialized();
			MCStyleCustomizationService.StyleRuntimeState styleRuntimeState = this.ResolveOrCreateState(partId);
			MCStylePresetSO mcstylePresetSO = this.ResolveSlotPreset(styleRuntimeState);
			if (!(mcstylePresetSO != null))
			{
				return 0;
			}
			return mcstylePresetSO.SurfaceCount;
		}

		// Token: 0x060035AC RID: 13740 RVA: 0x0013211C File Offset: 0x0013031C
		public int GetSlotSurfaceIndex(string partId)
		{
			this.EnsureRuntimeStateInitialized();
			MCStyleCustomizationService.StyleRuntimeState styleRuntimeState = this.ResolveOrCreateState(partId);
			if (styleRuntimeState == null)
			{
				return 0;
			}
			return Mathf.Max(0, styleRuntimeState.surfaceIndex);
		}

		// Token: 0x060035AD RID: 13741 RVA: 0x00132148 File Offset: 0x00130348
		public List<string> GetSlotSurfaceNames(string partId)
		{
			List<string> list = new List<string>();
			this.EnsureRuntimeStateInitialized();
			MCStyleCustomizationService.StyleRuntimeState styleRuntimeState = this.ResolveOrCreateState(partId);
			MCStylePresetSO mcstylePresetSO = this.ResolveSlotPreset(styleRuntimeState);
			if (mcstylePresetSO == null)
			{
				return list;
			}
			for (int i = 0; i < mcstylePresetSO.SurfaceCount; i++)
			{
				MCSurfaceVariantSO mcsurfaceVariantSO;
				if (mcstylePresetSO.TryGetSurfaceVariant(i, out mcsurfaceVariantSO) && mcsurfaceVariantSO != null)
				{
					list.Add(string.IsNullOrWhiteSpace(mcsurfaceVariantSO.displayName) ? mcsurfaceVariantSO.name : mcsurfaceVariantSO.displayName);
				}
				else
				{
					list.Add(string.Format("Style {0}", i + 1));
				}
			}
			return list;
		}

		// Token: 0x060035AE RID: 13742 RVA: 0x001321E4 File Offset: 0x001303E4
		public Sprite GetSlotPreviewIcon(string partId)
		{
			this.EnsureRuntimeStateInitialized();
			MCStyleCustomizationService.StyleRuntimeState styleRuntimeState = this.ResolveOrCreateState(partId);
			MCStylePresetSO mcstylePresetSO = this.ResolveSlotPreset(styleRuntimeState);
			if (mcstylePresetSO == null)
			{
				return null;
			}
			int num = ((styleRuntimeState != null) ? Mathf.Max(0, styleRuntimeState.surfaceIndex) : 0);
			MCSurfaceVariantSO mcsurfaceVariantSO;
			if (mcstylePresetSO.TryGetSurfaceVariant(num, out mcsurfaceVariantSO) && mcsurfaceVariantSO != null && mcsurfaceVariantSO.previewIcon != null)
			{
				return mcsurfaceVariantSO.previewIcon;
			}
			return mcstylePresetSO.previewIcon;
		}

		// Token: 0x060035AF RID: 13743 RVA: 0x00132254 File Offset: 0x00130454
		public bool SelectSurfaceForSlot(string partId, int surfaceIndex)
		{
			this.EnsureRuntimeStateInitialized();
			MCStyleCustomizationService.StyleRuntimeState styleRuntimeState = this.ResolveOrCreateState(partId);
			if (styleRuntimeState == null || styleRuntimeState.definition == null)
			{
				return false;
			}
			if ((styleRuntimeState.isUsingSceneDefault || styleRuntimeState.isUsingNoSelection || styleRuntimeState.styleIndex < 0) && !this.SetSlotEquipped(partId, true))
			{
				return false;
			}
			this._activeState = styleRuntimeState;
			return this.SelectSurface(surfaceIndex);
		}

		// Token: 0x060035B0 RID: 13744 RVA: 0x001322B8 File Offset: 0x001304B8
		public MCStylePartDefinitionSO GetDefinition(string partId)
		{
			if (string.IsNullOrWhiteSpace(partId))
			{
				return null;
			}
			MCStyleCustomizationService.StyleRuntimeState styleRuntimeState;
			if (this._stateByPartId.TryGetValue(partId, out styleRuntimeState) && styleRuntimeState != null)
			{
				return styleRuntimeState.definition;
			}
			for (int i = 0; i < this.partDefinitions.Count; i++)
			{
				if (this.partDefinitions[i] != null && string.Equals(this.partDefinitions[i].partId, partId, StringComparison.OrdinalIgnoreCase))
				{
					return this.partDefinitions[i];
				}
			}
			return null;
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x0013233C File Offset: 0x0013053C
		public bool SetActivePart(MCStylePartDefinitionSO definition)
		{
			this.EnsureRuntimeStateInitialized();
			if (definition == null || string.IsNullOrWhiteSpace(definition.partId))
			{
				return false;
			}
			MCStyleCustomizationService.StyleRuntimeState styleRuntimeState;
			if (!this._stateByPartId.TryGetValue(definition.partId, out styleRuntimeState))
			{
				styleRuntimeState = new MCStyleCustomizationService.StyleRuntimeState
				{
					definition = definition,
					isUsingSceneDefault = (definition.defaultBehavior == MCStyleDefaultBehavior.KeepSceneState),
					styleIndex = ((definition.defaultBehavior == MCStyleDefaultBehavior.ApplyConfiguredDefault) ? definition.ResolveDefaultStyleIndex() : (-1))
				};
				this._stateByPartId[definition.partId] = styleRuntimeState;
				this.ApplyState(styleRuntimeState, false);
			}
			this._activeState = styleRuntimeState;
			Action onActiveStateChanged = this.OnActiveStateChanged;
			if (onActiveStateChanged != null)
			{
				onActiveStateChanged();
			}
			return true;
		}

		// Token: 0x060035B2 RID: 13746 RVA: 0x001323E4 File Offset: 0x001305E4
		public bool SelectStyle(int uiIndex)
		{
			if (this._activeState == null || this._activeState.definition == null)
			{
				return false;
			}
			bool allowNoSelection = this._activeState.definition.allowNoSelection;
			int num = (allowNoSelection ? 1 : 0);
			if (allowNoSelection && uiIndex == 0)
			{
				return this.SetSlotEquipped(this._activeState.definition.partId, false);
			}
			int num2 = uiIndex - num;
			MCStylePresetSO mcstylePresetSO;
			if (!this._activeState.definition.TryGetStylePreset(num2, out mcstylePresetSO) || mcstylePresetSO == null || mcstylePresetSO.isVaulted)
			{
				return false;
			}
			if (!this.IsCustomizationStyleAllowed(this._activeState.definition, mcstylePresetSO))
			{
				return false;
			}
			this._activeState.isUsingNoSelection = false;
			this._activeState.isUsingSceneDefault = false;
			this._activeState.styleIndex = num2;
			this._activeState.surfaceIndex = mcstylePresetSO.ResolveDefaultSurfaceIndex();
			this._activeState.colorPresetIndex = MCStyleCustomizationService.ResolveDefaultColorPresetIndex(mcstylePresetSO, this._activeState.surfaceIndex);
			this._activeState.hasCustomPrimary = false;
			this._activeState.hasCustomSecondary = false;
			this.UnequipIncompatibleSlots(this._activeState, mcstylePresetSO);
			this.SetClothingGroupState(this._activeState, true);
			this.ApplySelection(this._activeState);
			return true;
		}

		// Token: 0x060035B3 RID: 13747 RVA: 0x00132518 File Offset: 0x00130718
		public bool SelectSurface(int index)
		{
			MCStylePresetSO mcstylePresetSO;
			if (!this.TryGetSelectedStyle(out mcstylePresetSO))
			{
				return false;
			}
			MCSurfaceVariantSO mcsurfaceVariantSO;
			if (!mcstylePresetSO.TryGetSurfaceVariant(index, out mcsurfaceVariantSO) || mcsurfaceVariantSO == null)
			{
				return false;
			}
			if (!this.IsCustomizationStyleAllowed(this._activeState.definition, mcstylePresetSO))
			{
				return false;
			}
			if (this._enforceCustomizationWardrobe && !this._activeState.definition.isBodyAppearance && !CustomizationWardrobeDiscoveryPolicy.IsSurfaceDiscovered(mcstylePresetSO, index))
			{
				return false;
			}
			this._activeState.surfaceIndex = index;
			this._activeState.colorPresetIndex = MCStyleCustomizationService.ResolveDefaultColorPresetIndex(mcstylePresetSO, index);
			this._activeState.hasCustomPrimary = false;
			this._activeState.hasCustomSecondary = false;
			this.UnequipIncompatibleSlots(this._activeState, mcstylePresetSO);
			this.ApplySelection(this._activeState);
			return true;
		}

		// Token: 0x060035B4 RID: 13748 RVA: 0x001325D4 File Offset: 0x001307D4
		public bool SelectColorPreset(int index)
		{
			MCSurfaceVariantSO mcsurfaceVariantSO;
			if (!this.TryGetSelectedSurface(out mcsurfaceVariantSO) || mcsurfaceVariantSO.colorProfile == null)
			{
				return false;
			}
			MCColorProfileSO.ColorPresetEntry colorPresetEntry;
			if (!mcsurfaceVariantSO.colorProfile.TryGetPreset(index, out colorPresetEntry) || colorPresetEntry == null)
			{
				return false;
			}
			this._activeState.colorPresetIndex = index;
			this._activeState.hasCustomPrimary = false;
			this._activeState.hasCustomSecondary = false;
			this.ApplyState(this._activeState, true);
			return true;
		}

		// Token: 0x060035B5 RID: 13749 RVA: 0x00132644 File Offset: 0x00130844
		public bool SetCustomColor(MCColorChannelId channelId, Color color)
		{
			MCSurfaceVariantSO mcsurfaceVariantSO;
			if (!this.TryGetSelectedSurface(out mcsurfaceVariantSO) || mcsurfaceVariantSO.colorProfile == null || !mcsurfaceVariantSO.colorProfile.allowCustomPicker)
			{
				return false;
			}
			if (channelId == MCColorChannelId.Primary)
			{
				this._activeState.customPrimary = color;
				this._activeState.hasCustomPrimary = true;
			}
			else
			{
				this._activeState.customSecondary = color;
				this._activeState.hasCustomSecondary = true;
			}
			this.ApplyColorState(this._activeState, mcsurfaceVariantSO);
			Action onActiveStateChanged = this.OnActiveStateChanged;
			if (onActiveStateChanged != null)
			{
				onActiveStateChanged();
			}
			return true;
		}

		// Token: 0x060035B6 RID: 13750 RVA: 0x001326CC File Offset: 0x001308CC
		public List<string> GetStyleNames()
		{
			List<string> list = new List<string>();
			if (this._activeState == null || this._activeState.definition == null)
			{
				return list;
			}
			if (this._activeState.definition.allowNoSelection)
			{
				list.Add(string.IsNullOrWhiteSpace(this._activeState.definition.noSelectionDisplayName) ? "None" : this._activeState.definition.noSelectionDisplayName);
			}
			for (int i = 0; i < this._activeState.definition.stylePresets.Count; i++)
			{
				MCStylePresetSO mcstylePresetSO = this._activeState.definition.stylePresets[i];
				if (!(mcstylePresetSO == null))
				{
					list.Add(string.IsNullOrWhiteSpace(mcstylePresetSO.displayName) ? mcstylePresetSO.name : mcstylePresetSO.displayName);
				}
			}
			return list;
		}

		// Token: 0x060035B7 RID: 13751 RVA: 0x001327A4 File Offset: 0x001309A4
		public int GetSelectedStyleUiIndex()
		{
			if (this._activeState == null || this._activeState.definition == null)
			{
				return 0;
			}
			int num = (this._activeState.definition.allowNoSelection ? 1 : 0);
			if (this._activeState.isUsingNoSelection && num == 1)
			{
				return 0;
			}
			if (this._activeState.isUsingSceneDefault)
			{
				return Mathf.Max(0, this._activeState.definition.ResolveDefaultStyleIndex() + num);
			}
			return Mathf.Max(0, this._activeState.styleIndex + num);
		}

		// Token: 0x060035B8 RID: 13752 RVA: 0x00132834 File Offset: 0x00130A34
		public List<string> GetSurfaceNames()
		{
			List<string> list = new List<string>();
			MCStylePresetSO mcstylePresetSO;
			if (!this.TryGetSelectedStyle(out mcstylePresetSO))
			{
				return list;
			}
			for (int i = 0; i < mcstylePresetSO.surfaceVariants.Count; i++)
			{
				MCSurfaceVariantSO mcsurfaceVariantSO = mcstylePresetSO.surfaceVariants[i];
				if (!(mcsurfaceVariantSO == null))
				{
					list.Add(string.IsNullOrWhiteSpace(mcsurfaceVariantSO.displayName) ? mcsurfaceVariantSO.name : mcsurfaceVariantSO.displayName);
				}
			}
			return list;
		}

		// Token: 0x060035B9 RID: 13753 RVA: 0x001328A1 File Offset: 0x00130AA1
		public int GetSelectedSurfaceIndex()
		{
			if (this._activeState == null)
			{
				return 0;
			}
			return Mathf.Max(0, this._activeState.surfaceIndex);
		}

		// Token: 0x060035BA RID: 13754 RVA: 0x001328C0 File Offset: 0x00130AC0
		public List<string> GetColorPresetNames()
		{
			List<string> list = new List<string>();
			MCSurfaceVariantSO mcsurfaceVariantSO;
			if (!this.TryGetSelectedSurface(out mcsurfaceVariantSO) || mcsurfaceVariantSO.colorProfile == null)
			{
				return list;
			}
			for (int i = 0; i < mcsurfaceVariantSO.colorProfile.presets.Count; i++)
			{
				MCColorProfileSO.ColorPresetEntry colorPresetEntry = mcsurfaceVariantSO.colorProfile.presets[i];
				if (colorPresetEntry != null)
				{
					list.Add(string.IsNullOrWhiteSpace(colorPresetEntry.displayName) ? string.Format("Preset {0}", i + 1) : colorPresetEntry.displayName);
				}
			}
			return list;
		}

		// Token: 0x060035BB RID: 13755 RVA: 0x0013294B File Offset: 0x00130B4B
		public int GetSelectedColorPresetIndex()
		{
			if (this._activeState == null)
			{
				return 0;
			}
			return Mathf.Max(0, this._activeState.colorPresetIndex);
		}

		// Token: 0x060035BC RID: 13756 RVA: 0x00132968 File Offset: 0x00130B68
		public bool IsSurfaceSelectionVisible()
		{
			MCStylePresetSO mcstylePresetSO;
			return this.TryGetSelectedStyle(out mcstylePresetSO) && mcstylePresetSO.SurfaceCount > 1;
		}

		// Token: 0x060035BD RID: 13757 RVA: 0x0013298C File Offset: 0x00130B8C
		public bool IsColorSelectionVisible()
		{
			MCSurfaceVariantSO mcsurfaceVariantSO;
			return this.TryGetSelectedSurface(out mcsurfaceVariantSO) && mcsurfaceVariantSO.surfaceType == MCSurfaceType.Naked && mcsurfaceVariantSO.colorProfile != null;
		}

		// Token: 0x060035BE RID: 13758 RVA: 0x001329BA File Offset: 0x00130BBA
		public bool IsSecondaryChannelVisible()
		{
			return this.IsColorSelectionVisible() && this.GetActiveColorProfile().ClampedChannelCount > 1;
		}

		// Token: 0x060035BF RID: 13759 RVA: 0x001329D4 File Offset: 0x00130BD4
		public bool IsCustomPickerAllowed()
		{
			MCColorProfileSO activeColorProfile = this.GetActiveColorProfile();
			return activeColorProfile != null && activeColorProfile.allowCustomPicker;
		}

		// Token: 0x060035C0 RID: 13760 RVA: 0x001329FC File Offset: 0x00130BFC
		public bool TryGetCurrentChannelColor(MCColorChannelId channelId, out Color color)
		{
			color = Color.white;
			MCSurfaceVariantSO mcsurfaceVariantSO;
			if (!this.TryGetSelectedSurface(out mcsurfaceVariantSO) || mcsurfaceVariantSO.colorProfile == null)
			{
				return false;
			}
			if (channelId == MCColorChannelId.Primary && this._activeState.hasCustomPrimary)
			{
				color = this._activeState.customPrimary;
				return true;
			}
			if (channelId == MCColorChannelId.Secondary && this._activeState.hasCustomSecondary)
			{
				color = this._activeState.customSecondary;
				return true;
			}
			MCColorProfileSO.ColorPresetEntry colorPresetEntry;
			if (mcsurfaceVariantSO.colorProfile.TryGetPreset(this._activeState.colorPresetIndex, out colorPresetEntry) && colorPresetEntry != null)
			{
				color = ((channelId == MCColorChannelId.Primary) ? colorPresetEntry.primaryColor : colorPresetEntry.secondaryColor);
				return true;
			}
			if (mcsurfaceVariantSO.colorProfile.presets.Count > 0 && mcsurfaceVariantSO.colorProfile.presets[0] != null)
			{
				MCColorProfileSO.ColorPresetEntry colorPresetEntry2 = mcsurfaceVariantSO.colorProfile.presets[0];
				color = ((channelId == MCColorChannelId.Primary) ? colorPresetEntry2.primaryColor : colorPresetEntry2.secondaryColor);
				return true;
			}
			return false;
		}

		// Token: 0x060035C1 RID: 13761 RVA: 0x00132B00 File Offset: 0x00130D00
		public void CaptureState(CharacterStateData stateData)
		{
			this.EnsureRuntimeStateInitialized();
			if (stateData == null)
			{
				return;
			}
			this.CaptureHairMorphValues(stateData);
			foreach (MCStyleCustomizationService.StyleRuntimeState styleRuntimeState in this._stateByPartId.Values)
			{
				if (styleRuntimeState.spawnedGO != null)
				{
					foreach (NeckSignSurface neckSignSurface in styleRuntimeState.spawnedGO.GetComponentsInChildren<NeckSignSurface>(true))
					{
						this._neckSignDesigns.Set(neckSignSurface.CaptureDesign());
					}
				}
			}
			stateData.neckSignDesigns = this._neckSignDesigns.Capture();
			if (stateData.colors == null)
			{
				stateData.colors = new List<ColorValue>();
			}
			stateData.colors.RemoveAll((ColorValue v) => v != null && string.Equals(v.category, "MCStyle", StringComparison.OrdinalIgnoreCase));
			foreach (KeyValuePair<string, MCStyleCustomizationService.StyleRuntimeState> keyValuePair in this._stateByPartId)
			{
				MCStyleCustomizationService.StyleRuntimeState value = keyValuePair.Value;
				if (value != null && !(value.definition == null))
				{
					stateData.colors.Add(new ColorValue("MCStyle", value.definition.partId, "StyleIndex", MCStyleCustomizationService.EncodeStateValue(value.styleIndex, value.isUsingSceneDefault)));
					stateData.colors.Add(new ColorValue("MCStyle", value.definition.partId, "SurfaceIndex", MCStyleCustomizationService.EncodeStateValue(value.surfaceIndex, false)));
					stateData.colors.Add(new ColorValue("MCStyle", value.definition.partId, "ColorPresetIndex", MCStyleCustomizationService.EncodeStateValue(value.colorPresetIndex, false)));
					stateData.colors.Add(new ColorValue("MCStyle", value.definition.partId, "NoSelection", MCStyleCustomizationService.EncodeStateFloat(value.isUsingNoSelection ? 1f : 0f)));
					if (value.hasCustomPrimary)
					{
						stateData.colors.Add(new ColorValue("MCStyle", value.definition.partId, "PrimaryCustom", value.customPrimary));
					}
					if (value.hasCustomSecondary)
					{
						stateData.colors.Add(new ColorValue("MCStyle", value.definition.partId, "SecondaryCustom", value.customSecondary));
					}
					Dictionary<string, float> dictionary;
					if (this._shaderSlidersByPartId.TryGetValue(value.definition.partId, out dictionary) && dictionary != null)
					{
						foreach (KeyValuePair<string, float> keyValuePair2 in dictionary)
						{
							stateData.colors.Add(new ColorValue("MCStyle", value.definition.partId, "ShaderSlider:" + keyValuePair2.Key, MCStyleCustomizationService.EncodeStateFloat(keyValuePair2.Value)));
						}
					}
				}
			}
		}

		// Token: 0x060035C2 RID: 13762 RVA: 0x00132E5C File Offset: 0x0013105C
		public MCStyleCustomizationService.SlotAppearanceSnapshot CaptureSlotAppearance(string partId, ClothingToggleController toggles = null)
		{
			this.EnsureRuntimeStateInitialized();
			MCStyleCustomizationService.StyleRuntimeState styleRuntimeState;
			if (string.IsNullOrEmpty(partId) || !this._stateByPartId.TryGetValue(partId, out styleRuntimeState) || styleRuntimeState == null || styleRuntimeState.definition == null || styleRuntimeState.isUsingNoSelection || styleRuntimeState.isUsingSceneDefault || styleRuntimeState.spawnedGO == null || !styleRuntimeState.spawnedGO.activeSelf)
			{
				return null;
			}
			ClothingToggleController clothingToggleController = ((toggles != null) ? toggles : this.clothingToggleController);
			return new MCStyleCustomizationService.SlotAppearanceSnapshot
			{
				owner = this,
				partId = partId,
				toggles = clothingToggleController,
				state = this.CaptureWardrobeSlot(partId, styleRuntimeState, clothingToggleController)
			};
		}

		// Token: 0x060035C3 RID: 13763 RVA: 0x00132F04 File Offset: 0x00131104
		public bool RestoreSlotAppearance(MCStyleCustomizationService.SlotAppearanceSnapshot snapshot)
		{
			MCStyleCustomizationService.StyleRuntimeState styleRuntimeState;
			if (snapshot == null || snapshot.owner != this || !this._stateByPartId.TryGetValue(snapshot.partId, out styleRuntimeState) || styleRuntimeState == null)
			{
				return false;
			}
			MCStyleCustomizationService.WardrobeSlotSnapshot state = snapshot.state;
			styleRuntimeState.styleIndex = state.styleIndex;
			styleRuntimeState.surfaceIndex = state.surfaceIndex;
			styleRuntimeState.colorPresetIndex = state.colorPresetIndex;
			styleRuntimeState.isUsingSceneDefault = state.sceneDefault;
			styleRuntimeState.isUsingNoSelection = state.noSelection;
			styleRuntimeState.hasCustomPrimary = state.hasPrimary;
			styleRuntimeState.hasCustomSecondary = state.hasSecondary;
			styleRuntimeState.customPrimary = state.primary;
			styleRuntimeState.customSecondary = state.secondary;
			if (state.shaderSliders == null)
			{
				this._shaderSlidersByPartId.Remove(snapshot.partId);
			}
			else
			{
				this._shaderSlidersByPartId[snapshot.partId] = new Dictionary<string, float>(state.shaderSliders, StringComparer.Ordinal);
			}
			MCStylePresetSO mcstylePresetSO;
			if (MCStyleCustomizationService.IsSharedNeckSlot(snapshot.partId) && !styleRuntimeState.isUsingNoSelection && !styleRuntimeState.isUsingSceneDefault && styleRuntimeState.definition.TryGetStylePreset(styleRuntimeState.styleIndex, out mcstylePresetSO))
			{
				this.UnequipIncompatibleSlots(styleRuntimeState, mcstylePresetSO);
			}
			this.ApplyState(styleRuntimeState, false);
			Action onActiveStateChanged = this.OnActiveStateChanged;
			if (onActiveStateChanged != null)
			{
				onActiveStateChanged();
			}
			if (snapshot.toggles != null && state.managedToggle)
			{
				snapshot.toggles.SetSlotVisible(snapshot.partId, state.toggleVisible);
			}
			this.SetSlotPhysicallyHidden(snapshot.partId, !state.rootActive);
			return true;
		}

		// Token: 0x060035C4 RID: 13764 RVA: 0x00133084 File Offset: 0x00131284
		private MCStyleCustomizationService.WardrobeSlotSnapshot CaptureWardrobeSlot(string partId, MCStyleCustomizationService.StyleRuntimeState state, ClothingToggleController toggles)
		{
			Dictionary<string, float> dictionary;
			return new MCStyleCustomizationService.WardrobeSlotSnapshot
			{
				styleIndex = state.styleIndex,
				surfaceIndex = state.surfaceIndex,
				colorPresetIndex = state.colorPresetIndex,
				sceneDefault = state.isUsingSceneDefault,
				noSelection = state.isUsingNoSelection,
				hasPrimary = state.hasCustomPrimary,
				hasSecondary = state.hasCustomSecondary,
				primary = state.customPrimary,
				secondary = state.customSecondary,
				shaderSliders = (this._shaderSlidersByPartId.TryGetValue(partId, out dictionary) ? new Dictionary<string, float>(dictionary, StringComparer.Ordinal) : null),
				hadRoot = (state.spawnedGO != null),
				rootActive = (state.spawnedGO != null && state.spawnedGO.activeSelf),
				managedToggle = (toggles != null && toggles.ManagesSlot(partId)),
				toggleVisible = (toggles != null && toggles.IsSlotVisible(partId))
			};
		}

		// Token: 0x060035C5 RID: 13765 RVA: 0x0013318C File Offset: 0x0013138C
		public MCStyleCustomizationService.WardrobeSnapshot CaptureWardrobeState(ClothingToggleController toggles = null)
		{
			this.EnsureRuntimeStateInitialized();
			MCStyleCustomizationService.WardrobeSnapshot wardrobeSnapshot = new MCStyleCustomizationService.WardrobeSnapshot
			{
				owner = this,
				activePartId = ((this.ActiveDefinition != null) ? this.ActiveDefinition.partId : null),
				isNaked = this._isNaked,
				toggles = ((toggles != null) ? toggles : this.clothingToggleController)
			};
			foreach (KeyValuePair<string, MCStyleCustomizationService.StyleRuntimeState> keyValuePair in this._stateByPartId)
			{
				MCStyleCustomizationService.StyleRuntimeState value = keyValuePair.Value;
				if (value != null && !(value.definition == null) && !value.definition.isBodyAppearance)
				{
					wardrobeSnapshot.slots.Add(keyValuePair.Key, this.CaptureWardrobeSlot(keyValuePair.Key, value, wardrobeSnapshot.toggles));
				}
			}
			return wardrobeSnapshot;
		}

		// Token: 0x060035C6 RID: 13766 RVA: 0x00133280 File Offset: 0x00131480
		public bool RestoreWardrobeState(MCStyleCustomizationService.WardrobeSnapshot snapshot)
		{
			if (snapshot == null || snapshot.owner != this)
			{
				return false;
			}
			foreach (KeyValuePair<string, MCStyleCustomizationService.StyleRuntimeState> keyValuePair in this._stateByPartId)
			{
				MCStyleCustomizationService.StyleRuntimeState value = keyValuePair.Value;
				if (value != null && !(value.definition == null) && !value.definition.isBodyAppearance)
				{
					MCStyleCustomizationService.WardrobeSlotSnapshot wardrobeSlotSnapshot;
					if (!snapshot.slots.TryGetValue(keyValuePair.Key, out wardrobeSlotSnapshot))
					{
						value.styleIndex = (value.surfaceIndex = (value.colorPresetIndex = -1));
						value.isUsingNoSelection = value.definition.allowNoSelection;
						value.isUsingSceneDefault = !value.isUsingNoSelection;
						value.hasCustomPrimary = (value.hasCustomSecondary = false);
						this._shaderSlidersByPartId.Remove(keyValuePair.Key);
					}
					else
					{
						value.styleIndex = wardrobeSlotSnapshot.styleIndex;
						value.surfaceIndex = wardrobeSlotSnapshot.surfaceIndex;
						value.colorPresetIndex = wardrobeSlotSnapshot.colorPresetIndex;
						value.isUsingSceneDefault = wardrobeSlotSnapshot.sceneDefault;
						value.isUsingNoSelection = wardrobeSlotSnapshot.noSelection;
						value.hasCustomPrimary = wardrobeSlotSnapshot.hasPrimary;
						value.hasCustomSecondary = wardrobeSlotSnapshot.hasSecondary;
						value.customPrimary = wardrobeSlotSnapshot.primary;
						value.customSecondary = wardrobeSlotSnapshot.secondary;
						if (wardrobeSlotSnapshot.shaderSliders == null)
						{
							this._shaderSlidersByPartId.Remove(keyValuePair.Key);
						}
						else
						{
							this._shaderSlidersByPartId[keyValuePair.Key] = new Dictionary<string, float>(wardrobeSlotSnapshot.shaderSliders, StringComparer.Ordinal);
						}
					}
				}
			}
			foreach (KeyValuePair<string, MCStyleCustomizationService.StyleRuntimeState> keyValuePair2 in this._stateByPartId)
			{
				MCStyleCustomizationService.StyleRuntimeState value2 = keyValuePair2.Value;
				MCStylePresetSO mcstylePresetSO;
				if (value2 != null && MCStyleCustomizationService.IsSharedNeckSlot(keyValuePair2.Key) && !value2.isUsingNoSelection && !value2.isUsingSceneDefault && value2.definition.TryGetStylePreset(value2.styleIndex, out mcstylePresetSO))
				{
					this.UnequipIncompatibleSlots(value2, mcstylePresetSO);
				}
			}
			if (this._isNaked != snapshot.isNaked)
			{
				this._isNaked = snapshot.isNaked;
			}
			foreach (KeyValuePair<string, MCStyleCustomizationService.StyleRuntimeState> keyValuePair3 in this._stateByPartId)
			{
				if (keyValuePair3.Value != null && keyValuePair3.Value.definition != null && !keyValuePair3.Value.definition.isBodyAppearance)
				{
					this.ApplyState(keyValuePair3.Value, false);
				}
			}
			MCStyleCustomizationService.StyleRuntimeState styleRuntimeState;
			this._activeState = ((snapshot.activePartId != null && this._stateByPartId.TryGetValue(snapshot.activePartId, out styleRuntimeState)) ? styleRuntimeState : null);
			Action onActiveStateChanged = this.OnActiveStateChanged;
			if (onActiveStateChanged != null)
			{
				onActiveStateChanged();
			}
			foreach (KeyValuePair<string, MCStyleCustomizationService.WardrobeSlotSnapshot> keyValuePair4 in snapshot.slots)
			{
				if (snapshot.toggles != null && keyValuePair4.Value.managedToggle)
				{
					snapshot.toggles.SetSlotVisible(keyValuePair4.Key, keyValuePair4.Value.toggleVisible);
				}
			}
			foreach (KeyValuePair<string, MCStyleCustomizationService.WardrobeSlotSnapshot> keyValuePair5 in snapshot.slots)
			{
				if (keyValuePair5.Value.hadRoot)
				{
					this.SetSlotPhysicallyHidden(keyValuePair5.Key, !keyValuePair5.Value.rootActive);
				}
			}
			return true;
		}

		// Token: 0x060035C7 RID: 13767 RVA: 0x001336BC File Offset: 0x001318BC
		public void ApplyState(CharacterStateData stateData)
		{
			this.ApplyState(stateData, false);
		}

		// Token: 0x060035C8 RID: 13768 RVA: 0x001336C8 File Offset: 0x001318C8
		public void ApplyState(CharacterStateData stateData, bool enforceWardrobeDiscoveries)
		{
			bool flag = this._enforceCustomizationWardrobe != enforceWardrobeDiscoveries;
			this._enforceCustomizationWardrobe = enforceWardrobeDiscoveries;
			this.EnsureRuntimeStateInitialized();
			if (flag)
			{
				this.RefreshShrinkForVisibleParts();
			}
			this.ApplyHairMorphValues(stateData);
			this._neckSignDesigns.Apply((stateData != null) ? stateData.neckSignDesigns : null);
			Action<string> onNeckSignDesignsChanged = this.OnNeckSignDesignsChanged;
			if (onNeckSignDesignsChanged != null)
			{
				onNeckSignDesignsChanged(null);
			}
			if (stateData == null || stateData.colors == null || stateData.colors.Count == 0)
			{
				this.ApplyNeckSignDesignsToSpawned();
				this.RefreshHairMorphs();
				if (this.EnforcesRankWardrobe)
				{
					this.EnforceCustomizationWardrobe();
				}
				return;
			}
			IList<ColorValue> list = this.MigrateLegacyDrapeColors(stateData.colors);
			for (int i = 0; i < list.Count; i++)
			{
				ColorValue colorValue = list[i];
				if (colorValue != null && string.Equals(colorValue.category, "MCStyle", StringComparison.OrdinalIgnoreCase))
				{
					MCStyleCustomizationService.StyleRuntimeState styleRuntimeState = this.ResolveOrCreateState(colorValue.subcategory);
					if (styleRuntimeState != null)
					{
						if (string.Equals(colorValue.colorProperty, "StyleIndex", StringComparison.OrdinalIgnoreCase))
						{
							int num;
							bool flag2;
							MCStyleCustomizationService.DecodeStateValue(colorValue.color, out num, out flag2);
							MCStylePresetSO mcstylePresetSO;
							MCStylePresetSO mcstylePresetSO2;
							if (!flag2 && num == 1 && styleRuntimeState.definition != null && styleRuntimeState.definition.partId == "Accessory.LeadChoker" && !styleRuntimeState.definition.TryGetStylePreset(num, out mcstylePresetSO) && styleRuntimeState.definition.TryGetStylePreset(0, out mcstylePresetSO2) && mcstylePresetSO2.presetId == "accessory_lead_choker")
							{
								num = 0;
							}
							styleRuntimeState.styleIndex = num;
							styleRuntimeState.isUsingSceneDefault = flag2;
						}
						else if (string.Equals(colorValue.colorProperty, "SurfaceIndex", StringComparison.OrdinalIgnoreCase))
						{
							int num2;
							bool flag3;
							MCStyleCustomizationService.DecodeStateValue(colorValue.color, out num2, out flag3);
							styleRuntimeState.surfaceIndex = num2;
						}
						else if (string.Equals(colorValue.colorProperty, "ColorPresetIndex", StringComparison.OrdinalIgnoreCase))
						{
							bool flag3;
							int num3;
							MCStyleCustomizationService.DecodeStateValue(colorValue.color, out num3, out flag3);
							styleRuntimeState.colorPresetIndex = num3;
						}
						else if (string.Equals(colorValue.colorProperty, "NoSelection", StringComparison.OrdinalIgnoreCase))
						{
							styleRuntimeState.isUsingNoSelection = MCStyleCustomizationService.DecodeStateFloat(colorValue.color) > 0.5f;
						}
						else if (string.Equals(colorValue.colorProperty, "PrimaryCustom", StringComparison.OrdinalIgnoreCase))
						{
							styleRuntimeState.customPrimary = colorValue.color;
							styleRuntimeState.hasCustomPrimary = true;
						}
						else if (string.Equals(colorValue.colorProperty, "SecondaryCustom", StringComparison.OrdinalIgnoreCase))
						{
							styleRuntimeState.customSecondary = colorValue.color;
							styleRuntimeState.hasCustomSecondary = true;
						}
						else if (colorValue.colorProperty != null && colorValue.colorProperty.StartsWith("ShaderSlider:", StringComparison.OrdinalIgnoreCase))
						{
							string text = colorValue.colorProperty.Substring("ShaderSlider:".Length);
							if (!string.IsNullOrEmpty(text))
							{
								string text2 = ((styleRuntimeState.definition != null) ? styleRuntimeState.definition.partId : colorValue.subcategory);
								Dictionary<string, float> dictionary;
								if (!this._shaderSlidersByPartId.TryGetValue(text2, out dictionary))
								{
									dictionary = new Dictionary<string, float>(StringComparer.Ordinal);
									this._shaderSlidersByPartId[text2] = dictionary;
								}
								dictionary[text] = MCStyleCustomizationService.DecodeStateFloat(colorValue.color);
							}
						}
					}
				}
			}
			foreach (MCStyleCustomizationService.StyleRuntimeState styleRuntimeState2 in this._stateByPartId.Values)
			{
				MCStylePresetSO mcstylePresetSO3;
				if (styleRuntimeState2 != null && styleRuntimeState2.definition != null && styleRuntimeState2.definition.TryGetStylePreset(styleRuntimeState2.styleIndex, out mcstylePresetSO3) && mcstylePresetSO3 != null && mcstylePresetSO3.gameplayOnly)
				{
					styleRuntimeState2.isUsingNoSelection = true;
					styleRuntimeState2.isUsingSceneDefault = false;
					styleRuntimeState2.styleIndex = -1;
					styleRuntimeState2.surfaceIndex = -1;
				}
			}
			if (enforceWardrobeDiscoveries)
			{
				this.RemoveUndiscoveredCustomizationSelections();
			}
			foreach (KeyValuePair<string, MCStyleCustomizationService.StyleRuntimeState> keyValuePair in this._stateByPartId)
			{
				MCStyleCustomizationService.StyleRuntimeState value = keyValuePair.Value;
				MCStylePresetSO mcstylePresetSO4;
				if (value != null && !value.isUsingSceneDefault && !value.isUsingNoSelection && value.definition.TryGetStylePreset(value.styleIndex, out mcstylePresetSO4) && mcstylePresetSO4 != null)
				{
					this.UnequipIncompatibleSlots(value, mcstylePresetSO4);
				}
			}
			if (this.EnforcesRankWardrobe)
			{
				this.NormalizeRankWardrobeSelections(null);
			}
			foreach (KeyValuePair<string, MCStyleCustomizationService.StyleRuntimeState> keyValuePair2 in this._stateByPartId)
			{
				this.ApplyState(keyValuePair2.Value, false);
			}
			this.ApplyNeckSignDesignsToSpawned();
			if (this.EnforcesRankWardrobe)
			{
				this.RestoreRankWardrobeVisibility();
			}
			Action onActiveStateChanged = this.OnActiveStateChanged;
			if (onActiveStateChanged == null)
			{
				return;
			}
			onActiveStateChanged();
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x00133B8C File Offset: 0x00131D8C
		private bool IsCustomizationStyleAllowed(MCStylePartDefinitionSO definition, MCStylePresetSO preset)
		{
			if (this._enforceCustomizationWardrobe && preset != null && preset.gameplayOnly)
			{
				return false;
			}
			if (!MCStyleCustomizationService.IsAccessoryPresetAvailable(preset))
			{
				return false;
			}
			if (!this.EnforcesRankWardrobe || !RankWardrobePolicy.IsWardrobePart(definition.partId) || RankWardrobePolicy.IsAllowedPreset(preset))
			{
				return true;
			}
			this.ShowFirstRunOutfitRestriction();
			return false;
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x00133BE4 File Offset: 0x00131DE4
		public void ShowFirstRunOutfitRestriction()
		{
			if (!this.EnforcesRankWardrobe || !RankWardrobePolicy.IsFirstRunOutfitRequired)
			{
				return;
			}
			if (Application.isPlaying)
			{
				ToastService.ShowComputed(RankWardrobePolicy.CreateFirstRunOutfitMessage(), "Ciel", 4f, "rank_ui", null);
			}
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x00133C17 File Offset: 0x00131E17
		private void ApplySelection(MCStyleCustomizationService.StyleRuntimeState state)
		{
			if (this.EnforcesRankWardrobe)
			{
				this.ApplyState(state, false);
				this.EnforceCustomizationWardrobe(state);
				return;
			}
			this.ApplyState(state, true);
		}

		// Token: 0x060035CC RID: 13772 RVA: 0x00133C39 File Offset: 0x00131E39
		public void EnforceCustomizationWardrobe()
		{
			this.EnforceCustomizationWardrobe(null);
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x00133C44 File Offset: 0x00131E44
		private void EnforceCustomizationWardrobe(MCStyleCustomizationService.StyleRuntimeState preferredSelection)
		{
			this._enforceCustomizationWardrobe = true;
			if (!this.EnforcesRankWardrobe)
			{
				return;
			}
			this.EnsureRuntimeStateInitialized();
			foreach (WearableItemSO wearableItemSO in RankWardrobePolicy.GetAvailableItems())
			{
				if (wearableItemSO != null && wearableItemSO.UsesMCStyle && this.GetDefinition(wearableItemSO.mcStylePartId) != null)
				{
					this.AddRuntimePreset(wearableItemSO.mcStylePartId, wearableItemSO.mcStylePreset);
				}
			}
			this.NormalizeRankWardrobeSelections(preferredSelection);
			foreach (KeyValuePair<string, MCStyleCustomizationService.StyleRuntimeState> keyValuePair in this._stateByPartId)
			{
				if (RankWardrobePolicy.IsWardrobePart(keyValuePair.Key))
				{
					this.ApplyState(keyValuePair.Value, false);
				}
			}
			this.RestoreRankWardrobeVisibility();
			Action onActiveStateChanged = this.OnActiveStateChanged;
			if (onActiveStateChanged == null)
			{
				return;
			}
			onActiveStateChanged();
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x00133D54 File Offset: 0x00131F54
		private void NormalizeRankWardrobeSelections(MCStyleCustomizationService.StyleRuntimeState preferredSelection = null)
		{
			List<WearableItemSO> list = new List<WearableItemSO>();
			WearableItemSO wearableItemSO = null;
			foreach (KeyValuePair<string, MCStyleCustomizationService.StyleRuntimeState> keyValuePair in this._stateByPartId)
			{
				MCStyleCustomizationService.StyleRuntimeState value = keyValuePair.Value;
				MCStylePresetSO mcstylePresetSO;
				if (RankWardrobePolicy.IsWardrobePart(keyValuePair.Key) && value != null && !(value.definition == null) && !value.isUsingSceneDefault && !value.isUsingNoSelection && value.definition.TryGetStylePreset(value.styleIndex, out mcstylePresetSO))
				{
					WearableItemSO mappedWearable = CustomizationWardrobeDiscoveryPolicy.GetMappedWearable(mcstylePresetSO);
					if (!(mappedWearable == null))
					{
						if (value == preferredSelection)
						{
							wearableItemSO = mappedWearable;
						}
						else
						{
							list.Add(mappedWearable);
						}
					}
				}
			}
			if (wearableItemSO != null)
			{
				list.Add(wearableItemSO);
			}
			List<WearableItemSO> list2 = RankWardrobePolicy.ResolveOutfit(list);
			using (Dictionary<string, MCStyleCustomizationService.StyleRuntimeState>.Enumerator enumerator = this._stateByPartId.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, MCStyleCustomizationService.StyleRuntimeState> pair = enumerator.Current;
					MCStyleCustomizationService.StyleRuntimeState value2 = pair.Value;
					if (RankWardrobePolicy.IsWardrobePart(pair.Key) && value2 != null && !(value2.definition == null))
					{
						WearableItemSO wearableItemSO2 = list2.Find((WearableItemSO candidate) => candidate != null && string.Equals(candidate.mcStylePartId, pair.Key, StringComparison.OrdinalIgnoreCase));
						MCStylePresetSO mcstylePresetSO2 = ((wearableItemSO2 != null) ? wearableItemSO2.mcStylePreset : null);
						MCStylePresetSO mcstylePresetSO3;
						if (!(mcstylePresetSO2 != null) || value2.isUsingSceneDefault || value2.isUsingNoSelection || !value2.definition.TryGetStylePreset(value2.styleIndex, out mcstylePresetSO3) || !(mcstylePresetSO3 == mcstylePresetSO2))
						{
							value2.isUsingNoSelection = mcstylePresetSO2 == null;
							value2.isUsingSceneDefault = false;
							value2.styleIndex = ((mcstylePresetSO2 != null) ? this.AddRuntimePreset(pair.Key, mcstylePresetSO2) : (-1));
							value2.surfaceIndex = ((mcstylePresetSO2 != null) ? mcstylePresetSO2.ResolveDefaultSurfaceIndex() : (-1));
							value2.colorPresetIndex = MCStyleCustomizationService.ResolveDefaultColorPresetIndex(mcstylePresetSO2, value2.surfaceIndex);
							value2.hasCustomPrimary = false;
							value2.hasCustomSecondary = false;
						}
					}
				}
			}
			this._isNaked = false;
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x00133FD4 File Offset: 0x001321D4
		private void RestoreRankWardrobeVisibility()
		{
			foreach (KeyValuePair<string, MCStyleCustomizationService.StyleRuntimeState> keyValuePair in this._stateByPartId)
			{
				if (RankWardrobePolicy.IsWardrobePart(keyValuePair.Key) && this.IsSlotEquipped(keyValuePair.Key))
				{
					this.SetClothingGroupState(keyValuePair.Value, true);
					if (this.clothingToggleController != null)
					{
						this.clothingToggleController.SetSlotVisible(keyValuePair.Key, true);
					}
					this.SetSlotPhysicallyHidden(keyValuePair.Key, false);
				}
			}
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x0013407C File Offset: 0x0013227C
		private IList<ColorValue> MigrateLegacyDrapeColors(List<ColorValue> colors)
		{
			bool flag = false;
			foreach (ColorValue colorValue in colors)
			{
				if (colorValue != null && string.Equals(colorValue.category, "MCStyle", StringComparison.OrdinalIgnoreCase))
				{
					if (string.Equals(colorValue.subcategory, "Outfit.Drape", StringComparison.OrdinalIgnoreCase))
					{
						return colors;
					}
					if (string.Equals(colorValue.subcategory, "Outfit.Bottom", StringComparison.OrdinalIgnoreCase) && string.Equals(colorValue.colorProperty, "StyleIndex", StringComparison.OrdinalIgnoreCase))
					{
						int num;
						bool flag2;
						MCStyleCustomizationService.DecodeStateValue(colorValue.color, out num, out flag2);
						flag = num == 5 && !flag2;
					}
				}
			}
			if (!flag)
			{
				return colors;
			}
			MCStylePartDefinitionSO definition = this.GetDefinition("Outfit.Bottom");
			MCStylePartDefinitionSO definition2 = this.GetDefinition("Outfit.Drape");
			if (definition2 == null || definition2.stylePresets == null)
			{
				return colors;
			}
			MCStylePresetSO mcstylePresetSO;
			if (definition != null && definition.TryGetStylePreset(5, out mcstylePresetSO) && mcstylePresetSO != null && !string.Equals(mcstylePresetSO.presetId, "full_coverage_outfit_back", StringComparison.OrdinalIgnoreCase))
			{
				return colors;
			}
			int num2 = definition2.stylePresets.FindIndex((MCStylePresetSO preset) => preset != null && string.Equals(preset.presetId, "full_coverage_outfit_back", StringComparison.OrdinalIgnoreCase));
			if (num2 < 0)
			{
				return colors;
			}
			List<ColorValue> list = new List<ColorValue>(colors.Count + 4);
			foreach (ColorValue colorValue2 in colors)
			{
				if (colorValue2 != null && string.Equals(colorValue2.category, "MCStyle", StringComparison.OrdinalIgnoreCase) && string.Equals(colorValue2.subcategory, "Outfit.Bottom", StringComparison.OrdinalIgnoreCase))
				{
					Color color = (string.Equals(colorValue2.colorProperty, "StyleIndex", StringComparison.OrdinalIgnoreCase) ? MCStyleCustomizationService.EncodeStateValue(num2, false) : colorValue2.color);
					list.Add(new ColorValue("MCStyle", "Outfit.Drape", colorValue2.colorProperty, color));
				}
				else
				{
					list.Add(colorValue2);
				}
			}
			list.Add(new ColorValue("MCStyle", "Outfit.Bottom", "StyleIndex", MCStyleCustomizationService.EncodeStateValue(-1, false)));
			list.Add(new ColorValue("MCStyle", "Outfit.Bottom", "SurfaceIndex", MCStyleCustomizationService.EncodeStateValue(-1, false)));
			list.Add(new ColorValue("MCStyle", "Outfit.Bottom", "ColorPresetIndex", MCStyleCustomizationService.EncodeStateValue(-1, false)));
			list.Add(new ColorValue("MCStyle", "Outfit.Bottom", "NoSelection", MCStyleCustomizationService.EncodeStateFloat(1f)));
			return list;
		}

		// Token: 0x060035D1 RID: 13777 RVA: 0x00134338 File Offset: 0x00132538
		private void RemoveUndiscoveredCustomizationSelections()
		{
			foreach (KeyValuePair<string, MCStyleCustomizationService.StyleRuntimeState> keyValuePair in this._stateByPartId)
			{
				MCStyleCustomizationService.StyleRuntimeState value = keyValuePair.Value;
				MCStylePresetSO mcstylePresetSO;
				if (value != null && !(value.definition == null) && !value.definition.isBodyAppearance && !value.isUsingSceneDefault && !value.isUsingNoSelection && value.definition.TryGetStylePreset(value.styleIndex, out mcstylePresetSO) && !(mcstylePresetSO == null))
				{
					if (!CustomizationWardrobeDiscoveryPolicy.Evaluate(mcstylePresetSO).IsDiscovered)
					{
						value.isUsingNoSelection = value.definition.allowNoSelection;
						value.isUsingSceneDefault = !value.definition.allowNoSelection;
						value.styleIndex = -1;
						value.surfaceIndex = -1;
						value.colorPresetIndex = -1;
						value.hasCustomPrimary = false;
						value.hasCustomSecondary = false;
					}
					else
					{
						int num = CustomizationWardrobeDiscoveryPolicy.ResolvePreferredDiscoveredSurfaceIndex(mcstylePresetSO, value.surfaceIndex);
						if (num >= 0 && num != value.surfaceIndex)
						{
							value.surfaceIndex = num;
							value.colorPresetIndex = MCStyleCustomizationService.ResolveDefaultColorPresetIndex(mcstylePresetSO, num);
							value.hasCustomPrimary = false;
							value.hasCustomSecondary = false;
						}
					}
				}
			}
		}

		// Token: 0x060035D2 RID: 13778 RVA: 0x00134498 File Offset: 0x00132698
		private static bool IsSharedNeckSlot(string partId)
		{
			return CollarAvailabilityPolicy.IsCollarPart(partId) || string.Equals(partId, "Accessory.HalloweenCollar", StringComparison.Ordinal) || string.Equals(partId, "Accessory.LilyNecklace", StringComparison.Ordinal);
		}

		// Token: 0x060035D3 RID: 13779 RVA: 0x001344C0 File Offset: 0x001326C0
		private void UnequipIncompatibleSlots(MCStyleCustomizationService.StyleRuntimeState source, MCStylePresetSO preset)
		{
			if (source == null || source.definition == null || preset == null)
			{
				return;
			}
			bool flag = MCStyleCustomizationService.IsSharedNeckSlot(source.definition.partId);
			if (flag && !MCStyleCustomizationService.IsAccessoryPresetAvailable(preset))
			{
				return;
			}
			if (preset.ignoreOtherSlotConflicts && !flag)
			{
				return;
			}
			List<string> list = (preset.overrideIncompatibleSlots ? preset.incompatibleSlotsOverride : source.definition.incompatibleSlots);
			WearableItemSO gameplayWearable = MCStyleCustomizationService.GetGameplayWearable(source, preset);
			foreach (KeyValuePair<string, MCStyleCustomizationService.StyleRuntimeState> keyValuePair in this._stateByPartId)
			{
				string key = keyValuePair.Key;
				MCStyleCustomizationService.StyleRuntimeState value = keyValuePair.Value;
				if (!string.IsNullOrWhiteSpace(key) && !string.Equals(key, source.definition.partId, StringComparison.OrdinalIgnoreCase) && value != null && !(value.definition == null) && !value.isUsingSceneDefault && !value.isUsingNoSelection)
				{
					bool flag2 = flag && MCStyleCustomizationService.IsSharedNeckSlot(key);
					MCStylePresetSO mcstylePresetSO;
					value.definition.TryGetStylePreset(value.styleIndex, out mcstylePresetSO);
					if (flag2 || (!preset.ignoreOtherSlotConflicts && (!(mcstylePresetSO != null) || !mcstylePresetSO.ignoreOtherSlotConflicts)))
					{
						WearableItemSO gameplayWearable2 = MCStyleCustomizationService.GetGameplayWearable(value, mcstylePresetSO);
						bool flag3 = flag2;
						if (!flag3 && gameplayWearable != null && gameplayWearable2 != null)
						{
							string text;
							flag3 = WardrobeLayeringRules.Conflicts(gameplayWearable, gameplayWearable2, out text);
						}
						else if (!flag3 && list != null)
						{
							int num = 0;
							while (num < list.Count && !flag3)
							{
								flag3 = string.Equals(list[num], key, StringComparison.OrdinalIgnoreCase);
								num++;
							}
						}
						if (flag3)
						{
							if (value.definition.allowNoSelection)
							{
								value.isUsingNoSelection = true;
								value.isUsingSceneDefault = false;
							}
							else
							{
								value.isUsingNoSelection = false;
								value.isUsingSceneDefault = true;
							}
							value.styleIndex = -1;
							value.surfaceIndex = -1;
							value.colorPresetIndex = -1;
							value.hasCustomPrimary = false;
							value.hasCustomSecondary = false;
							this.ApplyState(value, false);
						}
					}
				}
			}
		}

		// Token: 0x060035D4 RID: 13780 RVA: 0x00134704 File Offset: 0x00132904
		private static WearableItemSO GetGameplayWearable(MCStyleCustomizationService.StyleRuntimeState state, MCStylePresetSO preset)
		{
			if (state == null || preset == null)
			{
				return null;
			}
			WearableItemSO mappedWearable = CustomizationWardrobeDiscoveryPolicy.GetMappedWearable(preset);
			if (mappedWearable == null)
			{
				return null;
			}
			MCSurfaceVariantSO mcsurfaceVariantSO = null;
			if (preset.SurfaceCount > 0 && !preset.TryGetSurfaceVariant(state.surfaceIndex, out mcsurfaceVariantSO))
			{
				return null;
			}
			if (!mappedWearable.ResolveShowInGameplayInventory(mcsurfaceVariantSO))
			{
				return null;
			}
			return mappedWearable;
		}

		// Token: 0x060035D5 RID: 13781 RVA: 0x0013475C File Offset: 0x0013295C
		private void ApplyState(MCStyleCustomizationService.StyleRuntimeState state, bool broadcast)
		{
			if (state == null || state.definition == null)
			{
				return;
			}
			if (state.isUsingSceneDefault)
			{
				this.UnregisterShrinkForPart(state.definition.partId);
				this.ClearPhysicalForPart(state);
				if (broadcast)
				{
					Action onActiveStateChanged = this.OnActiveStateChanged;
					if (onActiveStateChanged == null)
					{
						return;
					}
					onActiveStateChanged();
				}
				return;
			}
			if (state.isUsingNoSelection)
			{
				this.UnregisterShrinkForPart(state.definition.partId);
				this.ClearPhysicalForPart(state, true);
				if (broadcast)
				{
					Action onActiveStateChanged2 = this.OnActiveStateChanged;
					if (onActiveStateChanged2 == null)
					{
						return;
					}
					onActiveStateChanged2();
				}
				return;
			}
			MCStylePresetSO mcstylePresetSO;
			if (!state.definition.TryGetStylePreset(state.styleIndex, out mcstylePresetSO) || mcstylePresetSO == null)
			{
				if (!state.definition.isBodyAppearance && state.styleIndex >= 0)
				{
					this.UnregisterShrinkForPart(state.definition.partId);
					this.ClearPhysicalForPart(state, true);
				}
				return;
			}
			if (mcstylePresetSO.isVaulted || !MCStyleCustomizationService.IsAccessoryPresetAvailable(mcstylePresetSO))
			{
				state.isUsingNoSelection = true;
				state.isUsingSceneDefault = false;
				state.styleIndex = -1;
				state.surfaceIndex = -1;
				state.colorPresetIndex = -1;
				this.UnregisterShrinkForPart(state.definition.partId);
				this.ClearPhysicalForPart(state, true);
				if (broadcast)
				{
					Action onActiveStateChanged3 = this.OnActiveStateChanged;
					if (onActiveStateChanged3 == null)
					{
						return;
					}
					onActiveStateChanged3();
				}
				return;
			}
			if (MCStyleCustomizationService.ShouldApplyPhysicalStyle(state, mcstylePresetSO))
			{
				this.ApplyPhysicalStyle(state, mcstylePresetSO);
			}
			this.ApplyPreviewClothingVisibility(state);
			this.RegisterShrinkForPart(state.definition.partId, mcstylePresetSO);
			if (string.Equals(state.definition.partId, "Head.Hair", StringComparison.Ordinal))
			{
				this.RefreshHairMorphs();
			}
			MCSurfaceVariantSO mcsurfaceVariantSO;
			if (!mcstylePresetSO.TryGetSurfaceVariant(state.surfaceIndex, out mcsurfaceVariantSO) || mcsurfaceVariantSO == null)
			{
				if (broadcast)
				{
					Action onActiveStateChanged4 = this.OnActiveStateChanged;
					if (onActiveStateChanged4 == null)
					{
						return;
					}
					onActiveStateChanged4();
				}
				return;
			}
			this.ApplySurface(state, mcsurfaceVariantSO);
			if (mcsurfaceVariantSO.surfaceType == MCSurfaceType.Naked)
			{
				this.ApplyColorState(state, mcsurfaceVariantSO);
			}
			else
			{
				this.ResetColorLayers(state, mcsurfaceVariantSO);
			}
			this.ApplyShaderOverrides(state, mcsurfaceVariantSO);
			if (state.spawnedGO != null)
			{
				NeckSignSurface[] componentsInChildren = state.spawnedGO.GetComponentsInChildren<NeckSignSurface>(true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].ApplyTexture();
				}
			}
			if (broadcast)
			{
				Action onActiveStateChanged5 = this.OnActiveStateChanged;
				if (onActiveStateChanged5 == null)
				{
					return;
				}
				onActiveStateChanged5();
			}
		}

		// Token: 0x060035D6 RID: 13782 RVA: 0x00134976 File Offset: 0x00132B76
		private static bool UsesRuntimeAttach(string partId)
		{
			return !string.IsNullOrEmpty(partId) && MCStyleCustomizationService.RuntimeAttachParts.Contains(partId);
		}

		// Token: 0x060035D7 RID: 13783 RVA: 0x00134990 File Offset: 0x00132B90
		private void ApplyPhysicalStyle(MCStyleCustomizationService.StyleRuntimeState state, MCStylePresetSO preset)
		{
			if (state.definition.partKind != MCStylePartKind.Physical)
			{
				return;
			}
			this.RemoveSpawnedOutfit(state);
			if (preset.physicalPrefab == null)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"[MCStyleService] ApplyPhysicalStyle '",
					state.definition.partId,
					"' — physicalPrefab is null on preset '",
					preset.presetId,
					"', skipping."
				}));
				return;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(preset.physicalPrefab);
			gameObject.name = preset.physicalPrefab.name;
			state.spawnedGO = gameObject;
			this.ApplySpawnedPartLayer(gameObject, (state.definition != null) ? state.definition.partId : null);
			if (MCStyleCustomizationService.ContainsAvatarRig(gameObject))
			{
				Debug.LogError(string.Concat(new string[]
				{
					"[MCStyleService] Physical preset '",
					preset.presetId,
					"' for part '",
					state.definition.partId,
					"' appears to contain a full Animator/Avatar rig. Use a clothing-only prefab for MC physical styles. Attach skipped to avoid Avatar/Rebind corruption."
				}));
				Object.Destroy(gameObject);
				state.spawnedGO = null;
				state.appliedPhysicalPreset = null;
				return;
			}
			if (state.definition.IsBoneAttachedAccessory)
			{
				this.AttachBoneAccessory(state, gameObject);
			}
			else if (this.playerDresser != null)
			{
				if (!(MCStyleCustomizationService.UsesRuntimeAttach(state.definition.partId) ? this.playerDresser.AttachClothingRuntime(gameObject) : this.playerDresser.AttachClothing(gameObject)))
				{
					Debug.LogError(string.Concat(new string[]
					{
						"[MCStyleService] PlayerDresser failed to attach '",
						gameObject.name,
						"' for part '",
						state.definition.partId,
						"'. Check prefab SMR/bones and PlayerDresser.characterBody."
					}));
				}
				else if (string.Equals(state.definition.partId, "Head.Hair", StringComparison.Ordinal))
				{
					Animator componentInParent = this.playerDresser.GetComponentInParent<Animator>();
					Transform transform = ((componentInParent != null) ? componentInParent.transform : this.playerDresser.transform);
					List<GameObject> list = new List<GameObject>();
					foreach (MCStyleCustomizationService.StyleRuntimeState styleRuntimeState in this._stateByPartId.Values)
					{
						if (styleRuntimeState.spawnedGO != null)
						{
							list.Add(styleRuntimeState.spawnedGO);
						}
					}
					this._hairPhysicsOwnership.Acquire(state.definition.partId, gameObject, transform, list);
				}
			}
			else
			{
				Debug.LogWarning("[MCStyleService] PlayerDresser not assigned. Prefab '" + gameObject.name + "' parented under service transform without bone rebinding.");
				gameObject.transform.SetParent(base.transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
			}
			this.InjectMagicaColliders(gameObject);
			this.ApplySpawnedPartLayer(gameObject, (state.definition != null) ? state.definition.partId : null);
			this.NotifyClothingToggle(state, gameObject);
			state.appliedPhysicalPreset = preset;
			NeckSignSurface[] componentsInChildren = gameObject.GetComponentsInChildren<NeckSignSurface>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				NeckSignSurface neckSignSurface = componentsInChildren[i];
				neckSignSurface.ApplyDesign(this._neckSignDesigns.Get(neckSignSurface.PartId));
				NeckSignSurface ownedSurface = neckSignSurface;
				neckSignSurface.DesignChanged += delegate
				{
					this.OnNeckSignDesignChanged(ownedSurface);
				};
			}
			Action<MCStylePartDefinitionSO, GameObject> onPhysicalSwapCompleted = this.OnPhysicalSwapCompleted;
			if (onPhysicalSwapCompleted == null)
			{
				return;
			}
			onPhysicalSwapCompleted(state.definition, gameObject);
		}

		// Token: 0x060035D8 RID: 13784 RVA: 0x00134D1C File Offset: 0x00132F1C
		public NeckSignDesignData GetNeckSignDesign(string partId)
		{
			return this._neckSignDesigns.Get(partId);
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x00134D2C File Offset: 0x00132F2C
		public void SetNeckSignDesign(NeckSignDesignData design)
		{
			this._neckSignDesigns.Set(design);
			if (design == null)
			{
				return;
			}
			GameObject spawnedRoot = this.GetSpawnedRoot(design.partId);
			if (spawnedRoot != null)
			{
				foreach (NeckSignSurface neckSignSurface in spawnedRoot.GetComponentsInChildren<NeckSignSurface>(true))
				{
					neckSignSurface.ApplyDesign(this._neckSignDesigns.Get(neckSignSurface.PartId));
				}
			}
			Action<string> onNeckSignDesignsChanged = this.OnNeckSignDesignsChanged;
			if (onNeckSignDesignsChanged == null)
			{
				return;
			}
			onNeckSignDesignsChanged(design.partId);
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x00134DA8 File Offset: 0x00132FA8
		private void ApplyNeckSignDesignsToSpawned()
		{
			foreach (MCStyleCustomizationService.StyleRuntimeState styleRuntimeState in this._stateByPartId.Values)
			{
				if (styleRuntimeState.spawnedGO != null)
				{
					foreach (NeckSignSurface neckSignSurface in styleRuntimeState.spawnedGO.GetComponentsInChildren<NeckSignSurface>(true))
					{
						neckSignSurface.ApplyDesign(this._neckSignDesigns.Get(neckSignSurface.PartId));
					}
				}
			}
		}

		// Token: 0x060035DB RID: 13787 RVA: 0x00134E40 File Offset: 0x00133040
		private void OnNeckSignDesignChanged(NeckSignSurface surface)
		{
			if (surface == null)
			{
				return;
			}
			NeckSignDesignData neckSignDesignData = surface.CaptureDesign();
			this._neckSignDesigns.Set(neckSignDesignData);
			Action<string> onNeckSignDesignsChanged = this.OnNeckSignDesignsChanged;
			if (onNeckSignDesignsChanged != null)
			{
				onNeckSignDesignsChanged(neckSignDesignData.partId);
			}
			MainCharacterAppearanceApplier componentInParent = base.GetComponentInParent<MainCharacterAppearanceApplier>();
			if (Application.isPlaying && !this._enforceCustomizationWardrobe && componentInParent != null && componentInParent.UsesSavedMainCharacterPreset)
			{
				MainCharacterAppearanceService.TrySaveNeckSignDesign(neckSignDesignData);
			}
		}

		// Token: 0x060035DC RID: 13788 RVA: 0x00134EB0 File Offset: 0x001330B0
		private void ApplySpawnedPartLayer(GameObject go, string partId)
		{
			if (go == null)
			{
				return;
			}
			string text = ((!string.IsNullOrEmpty(partId) && partId.IndexOf("hair", StringComparison.OrdinalIgnoreCase) >= 0) ? this.hairLayerName : this.clothingLayerName);
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			int num = LayerMask.NameToLayer(text);
			if (num < 0)
			{
				Debug.LogWarning(string.Concat(new string[] { "[MCStyleService] Layer '", text, "' non esiste; layer non assegnato a '", go.name, "'." }));
				return;
			}
			MCStyleCustomizationService.SetLayerRecursive(go.transform, num);
		}

		// Token: 0x060035DD RID: 13789 RVA: 0x00134F4C File Offset: 0x0013314C
		private static void SetLayerRecursive(Transform root, int layer)
		{
			if (root == null)
			{
				return;
			}
			root.gameObject.layer = layer;
			for (int i = 0; i < root.childCount; i++)
			{
				MCStyleCustomizationService.SetLayerRecursive(root.GetChild(i), layer);
			}
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x00134F90 File Offset: 0x00133190
		private void AttachBoneAccessory(MCStyleCustomizationService.StyleRuntimeState state, GameObject go)
		{
			Transform transform = this.FindAttachBone(state.definition.attachBoneName);
			if (transform == null)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"[MCStyleService] Accessorio '",
					state.definition.partId,
					"': bone '",
					state.definition.attachBoneName,
					"' non trovato nel rig. Prefab non attaccato."
				}), go);
				return;
			}
			Vector3 localPosition = go.transform.localPosition;
			Quaternion localRotation = go.transform.localRotation;
			Vector3 localScale = go.transform.localScale;
			go.transform.SetParent(transform, false);
			go.transform.localPosition = localPosition;
			go.transform.localRotation = localRotation;
			go.transform.localScale = localScale;
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x00135054 File Offset: 0x00133254
		private Transform FindAttachBone(string boneName)
		{
			if (string.IsNullOrWhiteSpace(boneName))
			{
				return null;
			}
			Transform transform = null;
			if (this.playerDresser != null)
			{
				SkinnedMeshRenderer characterBody = this.playerDresser.GetCharacterBody();
				if (characterBody != null && characterBody.rootBone != null)
				{
					transform = characterBody.rootBone;
				}
			}
			Transform transform2 = ((transform != null) ? MCStyleCustomizationService.FindInChildrenByName(transform, boneName) : null);
			if (transform2 != null)
			{
				return transform2;
			}
			return MCStyleCustomizationService.FindInChildrenByName(base.transform.root, boneName);
		}

		// Token: 0x060035E0 RID: 13792 RVA: 0x001350D8 File Offset: 0x001332D8
		private static Transform FindInChildrenByName(Transform root, string name)
		{
			if (root == null)
			{
				return null;
			}
			foreach (Transform transform in root.GetComponentsInChildren<Transform>(true))
			{
				if (string.Equals(transform.name, name, StringComparison.OrdinalIgnoreCase))
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x060035E1 RID: 13793 RVA: 0x0013511C File Offset: 0x0013331C
		private static bool ContainsAvatarRig(GameObject root)
		{
			if (root == null)
			{
				return false;
			}
			foreach (Animator animator in root.GetComponentsInChildren<Animator>(true))
			{
				if (animator != null && animator.avatar != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x00135168 File Offset: 0x00133368
		private void InjectMagicaColliders(GameObject spawnedOutfit)
		{
			if (spawnedOutfit == null)
			{
				return;
			}
			ColliderComponent[] array = this.ResolveAvailableMagicaColliders();
			if (array == null || array.Length == 0)
			{
				Debug.LogWarning("[MCStyleService] No Magica colliders found on MC hierarchy while applying Magica colliders.");
				return;
			}
			MagicaCloth[] componentsInChildren = spawnedOutfit.GetComponentsInChildren<MagicaCloth>(true);
			if (componentsInChildren == null || componentsInChildren.Length == 0)
			{
				return;
			}
			foreach (MagicaCloth magicaCloth in componentsInChildren)
			{
				if (!(magicaCloth == null) && magicaCloth.SerializeData != null && magicaCloth.SerializeData.colliderCollisionConstraint != null && !(magicaCloth.GetComponentInParent<ClothColliderProfile>(true) != null))
				{
					List<ColliderComponent> list = magicaCloth.SerializeData.colliderCollisionConstraint.colliderList;
					if (list == null)
					{
						list = new List<ColliderComponent>();
						magicaCloth.SerializeData.colliderCollisionConstraint.colliderList = list;
					}
					foreach (ColliderComponent colliderComponent in array)
					{
						if (!(colliderComponent == null) && !list.Contains(colliderComponent))
						{
							list.Add(colliderComponent);
						}
					}
				}
			}
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x00135258 File Offset: 0x00133458
		private ColliderComponent[] ResolveAvailableMagicaColliders()
		{
			List<ColliderComponent> list = new List<ColliderComponent>(32);
			HashSet<ColliderComponent> hashSet = new HashSet<ColliderComponent>();
			MCStyleCustomizationService.CollectMagicaColliders(base.transform, list, hashSet);
			if (this.playerDresser != null)
			{
				MCStyleCustomizationService.CollectMagicaColliders(this.playerDresser.transform, list, hashSet);
				SkinnedMeshRenderer characterBody = this.playerDresser.GetCharacterBody();
				if (characterBody != null)
				{
					MCStyleCustomizationService.CollectMagicaColliders(characterBody.transform, list, hashSet);
					if (characterBody.rootBone != null)
					{
						MCStyleCustomizationService.CollectMagicaColliders(characterBody.rootBone, list, hashSet);
						MCStyleCustomizationService.CollectMagicaColliders(characterBody.rootBone.root, list, hashSet);
					}
					else
					{
						MCStyleCustomizationService.CollectMagicaColliders(characterBody.transform.root, list, hashSet);
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x060035E4 RID: 13796 RVA: 0x0013530C File Offset: 0x0013350C
		private static void CollectMagicaColliders(Transform root, List<ColliderComponent> output, HashSet<ColliderComponent> seen)
		{
			if (root == null)
			{
				return;
			}
			foreach (ColliderComponent colliderComponent in root.GetComponentsInChildren<ColliderComponent>(true))
			{
				if (!(colliderComponent == null) && seen.Add(colliderComponent))
				{
					output.Add(colliderComponent);
				}
			}
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x00135358 File Offset: 0x00133558
		private static bool ShouldApplyPhysicalStyle(MCStyleCustomizationService.StyleRuntimeState state, MCStylePresetSO targetPreset)
		{
			return state != null && !(targetPreset == null) && !(state.definition == null) && state.definition.partKind == MCStylePartKind.Physical && (state.spawnedGO == null || state.appliedPhysicalPreset != targetPreset);
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x001353AC File Offset: 0x001335AC
		private BlendShapeManager ResolveBlendShapeManager()
		{
			if (this._blendShapeManager == null)
			{
				this._blendShapeManager = BlendShapeManager.ResolveForRig(this);
			}
			return this._blendShapeManager;
		}

		// Token: 0x060035E7 RID: 13799 RVA: 0x001353CE File Offset: 0x001335CE
		private void RegisterShrinkForPart(string partId, MCStylePresetSO preset)
		{
			this.UpdateShrinkForPart(partId, preset);
			this.RecomputeAndApplyShrink();
		}

		// Token: 0x060035E8 RID: 13800 RVA: 0x001353E0 File Offset: 0x001335E0
		private void RefreshShrinkForVisibleParts()
		{
			this._shrinkByPartId.Clear();
			this._previewShrinkByPartId.Clear();
			this._genitalCoveringPartIds.Clear();
			foreach (KeyValuePair<string, MCStyleCustomizationService.StyleRuntimeState> keyValuePair in this._stateByPartId)
			{
				this.UpdateShrinkForPart(keyValuePair.Key, this.GetActivePreset(keyValuePair.Key));
			}
			this.RecomputeAndApplyShrink();
		}

		// Token: 0x060035E9 RID: 13801 RVA: 0x00135470 File Offset: 0x00133670
		private void UpdateShrinkForPart(string partId, MCStylePresetSO preset)
		{
			if (string.IsNullOrWhiteSpace(partId))
			{
				return;
			}
			MCStyleCustomizationService.StyleRuntimeState styleRuntimeState;
			if (!this._stateByPartId.TryGetValue(partId, out styleRuntimeState) || styleRuntimeState == null || styleRuntimeState.spawnedGO == null || !styleRuntimeState.spawnedGO.activeSelf)
			{
				this._shrinkByPartId.Remove(partId);
				this._previewShrinkByPartId.Remove(partId);
				this._genitalCoveringPartIds.Remove(partId);
				return;
			}
			if (preset != null && preset.resetCustomGenitalBlendShapesWhenWorn)
			{
				this._genitalCoveringPartIds.Add(partId);
			}
			else
			{
				this._genitalCoveringPartIds.Remove(partId);
			}
			if (preset == null || preset.clothingShrinkBlendShapes == null || preset.clothingShrinkBlendShapes.Count == 0)
			{
				this._shrinkByPartId.Remove(partId);
			}
			else
			{
				this._shrinkByPartId[partId] = preset.clothingShrinkBlendShapes;
			}
			if (!this._enforceCustomizationWardrobe || preset == null || preset.previewOnlyClothingShrinkBlendShapes == null || preset.previewOnlyClothingShrinkBlendShapes.Count == 0)
			{
				this._previewShrinkByPartId.Remove(partId);
				return;
			}
			this._previewShrinkByPartId[partId] = preset.previewOnlyClothingShrinkBlendShapes;
		}

		// Token: 0x060035EA RID: 13802 RVA: 0x0013558E File Offset: 0x0013378E
		private void UnregisterShrinkForPart(string partId)
		{
			if (string.IsNullOrWhiteSpace(partId))
			{
				return;
			}
			if (this._shrinkByPartId.Remove(partId) | this._previewShrinkByPartId.Remove(partId) | this._genitalCoveringPartIds.Remove(partId))
			{
				this.RecomputeAndApplyShrink();
			}
		}

		// Token: 0x060035EB RID: 13803 RVA: 0x001355C8 File Offset: 0x001337C8
		private void RecomputeAndApplyShrink()
		{
			BlendShapeManager blendShapeManager = this.ResolveBlendShapeManager();
			if (blendShapeManager == null)
			{
				return;
			}
			Dictionary<string, float> dictionary = new Dictionary<string, float>(StringComparer.Ordinal);
			MCStyleCustomizationService.AggregateShrink(this._shrinkByPartId, dictionary);
			HashSet<string> hashSet = new HashSet<string>(dictionary.Keys, StringComparer.Ordinal);
			if (this._enforceCustomizationWardrobe)
			{
				MCStyleCustomizationService.AggregateShrink(this._previewShrinkByPartId, dictionary);
			}
			foreach (KeyValuePair<string, float> keyValuePair in dictionary)
			{
				blendShapeManager.SetOverride(keyValuePair.Key, keyValuePair.Value, "MCClothingShrink", BlendShapePriority.Clothing, false);
			}
			foreach (string text in this._appliedShrinkBlendShapes)
			{
				if (!dictionary.ContainsKey(text))
				{
					blendShapeManager.RemoveOverride(text, "MCClothingShrink", false, this._appliedPreviewOnlyShrinkBlendShapes.Contains(text), 0f);
				}
			}
			this._appliedShrinkBlendShapes.Clear();
			this._appliedPreviewOnlyShrinkBlendShapes.Clear();
			foreach (string text2 in dictionary.Keys)
			{
				this._appliedShrinkBlendShapes.Add(text2);
				if (!hashSet.Contains(text2))
				{
					this._appliedPreviewOnlyShrinkBlendShapes.Add(text2);
				}
			}
			bool flag = this._genitalCoveringPartIds.Count > 0;
			for (int i = 0; i < MCStyleCustomizationService.CustomGenitalBlendShapes.Length; i++)
			{
				string text3 = MCStyleCustomizationService.CustomGenitalBlendShapes[i];
				if (flag)
				{
					blendShapeManager.SetOverride(text3, 0f, "MCClothingGenitalReset", BlendShapePriority.Clothing, false);
				}
				else
				{
					blendShapeManager.RemoveOverride(text3, "MCClothingGenitalReset", false, false, 0f);
				}
			}
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x001357B8 File Offset: 0x001339B8
		private static void AggregateShrink(Dictionary<string, List<MCStylePresetSO.ClothingShrinkEntry>> byPart, Dictionary<string, float> aggregated)
		{
			foreach (List<MCStylePresetSO.ClothingShrinkEntry> list in byPart.Values)
			{
				if (list != null)
				{
					foreach (MCStylePresetSO.ClothingShrinkEntry clothingShrinkEntry in list)
					{
						float num;
						if (clothingShrinkEntry != null && !string.IsNullOrWhiteSpace(clothingShrinkEntry.blendShapeName) && (!aggregated.TryGetValue(clothingShrinkEntry.blendShapeName, out num) || clothingShrinkEntry.value > num))
						{
							aggregated[clothingShrinkEntry.blendShapeName] = clothingShrinkEntry.value;
						}
					}
				}
			}
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x0013587C File Offset: 0x00133A7C
		private void RemoveSpawnedOutfit(MCStyleCustomizationService.StyleRuntimeState state)
		{
			if (state.definition != null && string.Equals(state.definition.partId, "Head.Hair", StringComparison.Ordinal))
			{
				this.ReleaseHairMorphs();
			}
			if (state.definition != null && string.Equals(state.definition.partId, "Head.Hair", StringComparison.Ordinal))
			{
				this._hairPhysicsOwnership.Release();
			}
			if (state.spawnedGO == null)
			{
				if (state.definition != null)
				{
					this.UnregisterShrinkForPart(state.definition.partId);
				}
				state.appliedPhysicalPreset = null;
				return;
			}
			this._previewClothingVisibility.Remove(state.spawnedGO);
			Action<MCStylePartDefinitionSO, GameObject> onPhysicalRemoved = this.OnPhysicalRemoved;
			if (onPhysicalRemoved != null)
			{
				onPhysicalRemoved(state.definition, state.spawnedGO);
			}
			if (state.definition != null)
			{
				this.UnregisterShrinkForPart(state.definition.partId);
			}
			if (state.spawnedGO != null)
			{
				this.RestoreOwnedMaterials(state.spawnedGO.transform);
			}
			this.ReleaseUnusedOwnedMaterials();
			bool flag = state.definition != null && state.definition.IsBoneAttachedAccessory;
			if (this.playerDresser != null && !flag)
			{
				if (MCStyleCustomizationService.UsesRuntimeAttach((state.definition != null) ? state.definition.partId : null))
				{
					this.playerDresser.DetachClothingRuntime(state.spawnedGO, true);
				}
				else
				{
					this.playerDresser.DetachClothing(state.spawnedGO);
				}
			}
			else if (state.spawnedGO != null)
			{
				Object.Destroy(state.spawnedGO);
			}
			state.spawnedGO = null;
			state.appliedPhysicalPreset = null;
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x00135A30 File Offset: 0x00133C30
		private void CleanupRuntimeSpawns()
		{
			this._hairPhysicsOwnership.Release();
			if (this._stateByPartId == null || this._stateByPartId.Count == 0)
			{
				return;
			}
			foreach (KeyValuePair<string, MCStyleCustomizationService.StyleRuntimeState> keyValuePair in this._stateByPartId)
			{
				if (keyValuePair.Value != null)
				{
					this.RemoveSpawnedOutfit(keyValuePair.Value);
				}
			}
		}

		// Token: 0x060035EF RID: 13807 RVA: 0x00135AB4 File Offset: 0x00133CB4
		private void ClearPhysicalForPart(MCStyleCustomizationService.StyleRuntimeState state)
		{
			this.ClearPhysicalForPart(state, false);
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x00135AC0 File Offset: 0x00133CC0
		private void ClearPhysicalForPart(MCStyleCustomizationService.StyleRuntimeState state, bool forceDisableClothingGroup)
		{
			this.RemoveSpawnedOutfit(state);
			if (!forceDisableClothingGroup || state == null || state.definition == null || this.clothingToggleController == null || string.IsNullOrWhiteSpace(state.definition.clothingGroupName))
			{
				return;
			}
			this.clothingToggleController.SetGroupState(state.definition.clothingGroupName, false);
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x00135B20 File Offset: 0x00133D20
		private void SetClothingGroupState(MCStyleCustomizationService.StyleRuntimeState state, bool active)
		{
			if (state == null || state.definition == null || this.clothingToggleController == null || string.IsNullOrWhiteSpace(state.definition.clothingGroupName))
			{
				return;
			}
			this.clothingToggleController.SetGroupState(state.definition.clothingGroupName, active);
		}

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x060035F2 RID: 13810 RVA: 0x00135B76 File Offset: 0x00133D76
		public bool IsNaked
		{
			get
			{
				return this._isNaked;
			}
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x00135B80 File Offset: 0x00133D80
		public bool IsPartHiddenForPreview(string partId)
		{
			MCStyleCustomizationService.StyleRuntimeState styleRuntimeState;
			return this._previewClothingHidden && !string.IsNullOrWhiteSpace(partId) && this._stateByPartId.TryGetValue(partId, out styleRuntimeState) && this.IsPreviewGarment(styleRuntimeState);
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x00135BB8 File Offset: 0x00133DB8
		private bool IsPreviewGarment(MCStyleCustomizationService.StyleRuntimeState state)
		{
			if (state == null || state.definition == null || state.definition.isBodyAppearance || state.definition.keepVisibleWhenNaked)
			{
				return false;
			}
			MCStylePresetSO activePreset = this.GetActivePreset(state.definition.partId);
			return CustomizationWardrobeDiscoveryPolicy.GetMappedWearable(activePreset) == null || MCStyleCustomizationService.GetGameplayWearable(state, activePreset) != null;
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x00135C24 File Offset: 0x00133E24
		public void SetPreviewClothingHidden(bool hidden)
		{
			if (this._previewClothingHidden == hidden)
			{
				return;
			}
			if (hidden)
			{
				this.EnsureRuntimeStateInitialized();
			}
			this._previewClothingHidden = hidden;
			foreach (MCStyleCustomizationService.StyleRuntimeState styleRuntimeState in this._stateByPartId.Values)
			{
				if (styleRuntimeState != null && !(styleRuntimeState.spawnedGO == null))
				{
					bool flag;
					if (hidden)
					{
						this.ApplyPreviewClothingVisibility(styleRuntimeState);
					}
					else if (this._previewClothingVisibility.TryGetValue(styleRuntimeState.spawnedGO, out flag))
					{
						styleRuntimeState.spawnedGO.SetActive(flag);
						if (flag)
						{
							this.RegisterShrinkForPart(styleRuntimeState.definition.partId, this.GetActivePreset(styleRuntimeState.definition.partId));
						}
						else
						{
							this.UnregisterShrinkForPart(styleRuntimeState.definition.partId);
						}
					}
				}
			}
			if (!hidden)
			{
				this._previewClothingVisibility.Clear();
			}
			Action onPreviewClothingVisibilityChanged = this.OnPreviewClothingVisibilityChanged;
			if (onPreviewClothingVisibilityChanged != null)
			{
				onPreviewClothingVisibilityChanged();
			}
			Action onSlotVisibilityChanged = this.OnSlotVisibilityChanged;
			if (onSlotVisibilityChanged == null)
			{
				return;
			}
			onSlotVisibilityChanged();
		}

		// Token: 0x060035F6 RID: 13814 RVA: 0x00135D38 File Offset: 0x00133F38
		private void ApplyPreviewClothingVisibility(MCStyleCustomizationService.StyleRuntimeState state)
		{
			if (!this._previewClothingHidden || state == null || state.spawnedGO == null)
			{
				return;
			}
			if (!this.IsPreviewGarment(state))
			{
				bool flag;
				if (this._previewClothingVisibility.TryGetValue(state.spawnedGO, out flag))
				{
					state.spawnedGO.SetActive(flag);
					this._previewClothingVisibility.Remove(state.spawnedGO);
				}
				return;
			}
			if (!this._previewClothingVisibility.ContainsKey(state.spawnedGO))
			{
				this._previewClothingVisibility.Add(state.spawnedGO, state.spawnedGO.activeSelf);
			}
			state.spawnedGO.SetActive(false);
			this.UnregisterShrinkForPart(state.definition.partId);
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x00135DE8 File Offset: 0x00133FE8
		private void OnDisable()
		{
			this.ReleaseHairMorphs();
			RankProgressionService.Changed -= this.RemoveUnavailableAccessories;
			this.SetPreviewClothingHidden(false);
		}

		// Token: 0x060035F8 RID: 13816 RVA: 0x00135E08 File Offset: 0x00134008
		private void NotifyClothingToggle(MCStyleCustomizationService.StyleRuntimeState state, GameObject newGO)
		{
			if (newGO == null)
			{
				return;
			}
			bool flag = this._isNaked && state.definition != null && !state.definition.isBodyAppearance;
			newGO.SetActive(!flag);
			this.ApplyPreviewClothingVisibility(state);
		}

		// Token: 0x060035F9 RID: 13817 RVA: 0x00135E58 File Offset: 0x00134058
		public void ToggleNaked()
		{
			this.SetNaked(!this._isNaked);
		}

		// Token: 0x060035FA RID: 13818 RVA: 0x00135E6C File Offset: 0x0013406C
		public void SetNaked(bool naked)
		{
			if (naked && this.EnforcesRankWardrobe)
			{
				this.ShowFirstRunOutfitRestriction();
				return;
			}
			this._isNaked = naked;
			foreach (KeyValuePair<string, MCStyleCustomizationService.StyleRuntimeState> keyValuePair in this._stateByPartId)
			{
				MCStyleCustomizationService.StyleRuntimeState value = keyValuePair.Value;
				if (value != null && !(value.definition == null) && !(value.spawnedGO == null) && !value.definition.isBodyAppearance)
				{
					value.spawnedGO.SetActive(!naked);
					this.ApplyPreviewClothingVisibility(value);
					if (naked)
					{
						this.UnregisterShrinkForPart(value.definition.partId);
					}
					else
					{
						this.RegisterShrinkForPart(value.definition.partId, this.GetActivePreset(value.definition.partId));
					}
				}
			}
			Action onActiveStateChanged = this.OnActiveStateChanged;
			if (onActiveStateChanged == null)
			{
				return;
			}
			onActiveStateChanged();
		}

		// Token: 0x060035FB RID: 13819 RVA: 0x00135F6C File Offset: 0x0013416C
		public void SetFunctionalWardrobeHidden(bool hidden)
		{
			this.EnsureRuntimeStateInitialized();
			foreach (KeyValuePair<string, MCStyleCustomizationService.StyleRuntimeState> keyValuePair in this._stateByPartId)
			{
				MCStyleCustomizationService.StyleRuntimeState value = keyValuePair.Value;
				if (value != null && !(value.definition == null) && !value.definition.isBodyAppearance && !(value.spawnedGO == null))
				{
					MCStylePresetSO activePreset = this.GetActivePreset(value.definition.partId);
					if (!(MCStyleCustomizationService.GetGameplayWearable(value, activePreset) == null))
					{
						if (this.clothingToggleController != null && this.clothingToggleController.UsingService && this.clothingToggleController.ManagesSlot(keyValuePair.Key))
						{
							this.clothingToggleController.SetSlotVisible(keyValuePair.Key, !hidden);
						}
						else
						{
							value.spawnedGO.SetActive(!hidden);
							if (hidden)
							{
								this.UnregisterShrinkForPart(value.definition.partId);
							}
							else
							{
								this.RegisterShrinkForPart(value.definition.partId, activePreset);
							}
						}
					}
				}
			}
			Action onActiveStateChanged = this.OnActiveStateChanged;
			if (onActiveStateChanged == null)
			{
				return;
			}
			onActiveStateChanged();
		}

		// Token: 0x060035FC RID: 13820 RVA: 0x001360B4 File Offset: 0x001342B4
		public bool SetSlotPhysicallyHidden(string partId, bool hidden)
		{
			if (hidden && this.EnforcesRankWardrobe && RankWardrobePolicy.IsWardrobePart(partId))
			{
				return false;
			}
			MCStyleCustomizationService.StyleRuntimeState styleRuntimeState;
			if (string.IsNullOrWhiteSpace(partId) || !this._stateByPartId.TryGetValue(partId, out styleRuntimeState) || styleRuntimeState == null || styleRuntimeState.definition == null || styleRuntimeState.spawnedGO == null)
			{
				return false;
			}
			if (this.IsPartHiddenForPreview(partId))
			{
				this.ApplyPreviewClothingVisibility(styleRuntimeState);
			}
			else
			{
				styleRuntimeState.spawnedGO.SetActive(!hidden);
			}
			if (!styleRuntimeState.spawnedGO.activeSelf)
			{
				this.UnregisterShrinkForPart(partId);
			}
			else
			{
				this.RegisterShrinkForPart(partId, this.GetActivePreset(partId));
			}
			Action onSlotVisibilityChanged = this.OnSlotVisibilityChanged;
			if (onSlotVisibilityChanged != null)
			{
				onSlotVisibilityChanged();
			}
			return true;
		}

		// Token: 0x060035FD RID: 13821 RVA: 0x00136168 File Offset: 0x00134368
		private void ApplySurface(MCStyleCustomizationService.StyleRuntimeState state, MCSurfaceVariantSO surface)
		{
			List<Renderer> activeRenderers = this.GetActiveRenderers(state);
			this.ApplyMaterialBindings(state, surface, activeRenderers);
			if (!(surface != null) || surface.textureBindings == null || surface.textureBindings.Length == 0)
			{
				for (int i = 0; i < activeRenderers.Count; i++)
				{
					Renderer renderer = activeRenderers[i];
					if (!(renderer == null))
					{
						Material[] cachedMaterials = this.GetCachedMaterials(renderer);
						if (cachedMaterials != null)
						{
							foreach (Material material in cachedMaterials)
							{
								if (!(material == null))
								{
									if (surface.baseMap != null)
									{
										MCStyleCustomizationService.ApplyTextureIfPresent(material, surface.baseMapPropertyName, surface.baseMap);
									}
									if (surface.normalMap != null)
									{
										MCStyleCustomizationService.ApplyTextureIfPresent(material, surface.normalMapPropertyName, surface.normalMap);
									}
									if (surface.maskMap != null)
									{
										MCStyleCustomizationService.ApplyTextureIfPresent(material, surface.maskMapPropertyName, surface.maskMap);
									}
								}
							}
						}
					}
				}
				return;
			}
			for (int k = 0; k < activeRenderers.Count; k++)
			{
				Renderer renderer2 = activeRenderers[k];
				if (!(renderer2 == null))
				{
					Material[] cachedMaterials2 = this.GetCachedMaterials(renderer2);
					if (cachedMaterials2 != null)
					{
						for (int l = 0; l < surface.textureBindings.Length; l++)
						{
							MCSurfaceVariantSO.TextureTargetBinding textureTargetBinding = surface.textureBindings[l];
							if (textureTargetBinding != null && textureTargetBinding.HasAnyTexture && this.MatchesTextureBindingTarget(state, renderer2, textureTargetBinding.rendererTarget))
							{
								int materialIndex = textureTargetBinding.materialIndex;
								if (materialIndex >= 0)
								{
									if (materialIndex < cachedMaterials2.Length)
									{
										MCStyleCustomizationService.ApplyTextureOverrideToMaterial(cachedMaterials2[materialIndex], surface, textureTargetBinding);
									}
								}
								else
								{
									for (int m = 0; m < cachedMaterials2.Length; m++)
									{
										MCStyleCustomizationService.ApplyTextureOverrideToMaterial(cachedMaterials2[m], surface, textureTargetBinding);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060035FE RID: 13822 RVA: 0x00136334 File Offset: 0x00134534
		private void ApplyMaterialBindings(MCStyleCustomizationService.StyleRuntimeState state, MCSurfaceVariantSO surface, List<Renderer> renderers)
		{
			if (surface == null || surface.materialBindings == null || surface.materialBindings.Length == 0)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < renderers.Count; i++)
			{
				Renderer renderer = renderers[i];
				if (!(renderer == null))
				{
					Material[] sharedMaterials = renderer.sharedMaterials;
					if (sharedMaterials != null && sharedMaterials.Length != 0)
					{
						bool flag2 = false;
						for (int j = 0; j < surface.materialBindings.Length; j++)
						{
							MCSurfaceVariantSO.MaterialTargetBinding materialTargetBinding = surface.materialBindings[j];
							if (materialTargetBinding != null && !(materialTargetBinding.material == null) && this.MatchesTextureBindingTarget(state, renderer, materialTargetBinding.rendererTarget) && (string.IsNullOrWhiteSpace(materialTargetBinding.rendererName) || string.Equals(renderer.name, materialTargetBinding.rendererName, StringComparison.Ordinal)))
							{
								if (materialTargetBinding.materialIndex >= 0)
								{
									if (materialTargetBinding.materialIndex < sharedMaterials.Length && sharedMaterials[materialTargetBinding.materialIndex] != materialTargetBinding.material)
									{
										sharedMaterials[materialTargetBinding.materialIndex] = materialTargetBinding.material;
										flag2 = true;
									}
								}
								else
								{
									for (int k = 0; k < sharedMaterials.Length; k++)
									{
										if (sharedMaterials[k] != materialTargetBinding.material)
										{
											sharedMaterials[k] = materialTargetBinding.material;
											flag2 = true;
										}
									}
								}
							}
						}
						if (flag2)
						{
							renderer.sharedMaterials = sharedMaterials;
							this._materialInstanceCache.Remove(renderer);
							flag = true;
						}
					}
				}
			}
			if (flag)
			{
				this.ReleaseUnusedOwnedMaterials();
			}
		}

		// Token: 0x060035FF RID: 13823 RVA: 0x001364B0 File Offset: 0x001346B0
		private static void ApplyTextureOverrideToMaterial(Material material, MCSurfaceVariantSO surface, MCSurfaceVariantSO.TextureTargetBinding binding)
		{
			if (material == null || surface == null || binding == null)
			{
				return;
			}
			string text = (string.IsNullOrWhiteSpace(binding.baseMapPropertyName) ? surface.baseMapPropertyName : binding.baseMapPropertyName);
			string text2 = (string.IsNullOrWhiteSpace(binding.normalMapPropertyName) ? surface.normalMapPropertyName : binding.normalMapPropertyName);
			string text3 = (string.IsNullOrWhiteSpace(binding.maskMapPropertyName) ? surface.maskMapPropertyName : binding.maskMapPropertyName);
			MCStyleCustomizationService.ApplyTextureIfPresent(material, text, binding.baseMap);
			MCStyleCustomizationService.ApplyTextureIfPresent(material, text2, binding.normalMap);
			MCStyleCustomizationService.ApplyTextureIfPresent(material, text3, binding.maskMap);
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x00136550 File Offset: 0x00134750
		private bool MatchesTextureBindingTarget(MCStyleCustomizationService.StyleRuntimeState state, Renderer renderer, MCSurfaceVariantSO.RendererTargetType target)
		{
			Dictionary<Renderer, MCSurfaceVariantSO.RendererTargetType> dictionary;
			MCSurfaceVariantSO.RendererTargetType rendererTargetType;
			return !(renderer == null) && (target == MCSurfaceVariantSO.RendererTargetType.Any || (state != null && !(state.definition == null) && state.definition.partKind == MCStylePartKind.Virtual && this._virtualRendererTargetsByPartId.TryGetValue(state.definition.partId, out dictionary) && dictionary != null && dictionary.TryGetValue(renderer, out rendererTargetType) && rendererTargetType == target));
		}

		// Token: 0x06003601 RID: 13825 RVA: 0x001365C0 File Offset: 0x001347C0
		private void ApplyColorState(MCStyleCustomizationService.StyleRuntimeState state, MCSurfaceVariantSO surface)
		{
			MCColorProfileSO colorProfile = surface.colorProfile;
			if (colorProfile == null || surface.surfaceType != MCSurfaceType.Naked)
			{
				return;
			}
			int num = MCStyleCustomizationService.ResolveScopedMaterialIndex(state, surface);
			Color white;
			if (!MCStyleCustomizationService.TryResolvePresetColor(colorProfile, state, MCColorChannelId.Primary, out white))
			{
				white = Color.white;
			}
			this.ApplyChannelColor(state, colorProfile, MCColorChannelId.Primary, white, num);
			if (colorProfile.ClampedChannelCount > 1)
			{
				Color color;
				if (!MCStyleCustomizationService.TryResolvePresetColor(colorProfile, state, MCColorChannelId.Secondary, out color))
				{
					color = white;
				}
				this.ApplyChannelColor(state, colorProfile, MCColorChannelId.Secondary, color, num);
			}
		}

		// Token: 0x06003602 RID: 13826 RVA: 0x00136630 File Offset: 0x00134830
		private static int ResolveScopedMaterialIndex(MCStyleCustomizationService.StyleRuntimeState state, MCSurfaceVariantSO surface)
		{
			if (state == null || surface == null || surface.textureBindings == null || surface.textureBindings.Length == 0)
			{
				return -1;
			}
			for (int i = 0; i < surface.textureBindings.Length; i++)
			{
				MCSurfaceVariantSO.TextureTargetBinding textureTargetBinding = surface.textureBindings[i];
				if (textureTargetBinding != null && textureTargetBinding.materialIndex >= 0)
				{
					return textureTargetBinding.materialIndex;
				}
			}
			return -1;
		}

		// Token: 0x06003603 RID: 13827 RVA: 0x0013668C File Offset: 0x0013488C
		private void ResetColorLayers(MCStyleCustomizationService.StyleRuntimeState state, MCSurfaceVariantSO surface)
		{
			MCSurfaceVariantSO mcsurfaceVariantSO = MCStyleCustomizationService.FindNakedVariant(state);
			int num = ((mcsurfaceVariantSO != null) ? MCStyleCustomizationService.ResolveScopedMaterialIndex(state, mcsurfaceVariantSO) : MCStyleCustomizationService.ResolveScopedMaterialIndex(state, surface));
			HashSet<MCColorCustomizationService.ColorLayer> hashSet = MCStyleCustomizationService.ResolveOwnedColorLayers(mcsurfaceVariantSO);
			if (hashSet.Count == 0)
			{
				this.DisableColorLayer(state, surface, MCColorCustomizationService.ColorLayer.Main2nd, "_Main2ndTex", num);
				this.DisableColorLayer(state, surface, MCColorCustomizationService.ColorLayer.Main3rd, "_Main3rdTex", num);
				return;
			}
			if (hashSet.Contains(MCColorCustomizationService.ColorLayer.Main2nd))
			{
				this.DisableColorLayer(state, surface, MCColorCustomizationService.ColorLayer.Main2nd, "_Main2ndTex", num);
			}
			if (hashSet.Contains(MCColorCustomizationService.ColorLayer.Main3rd))
			{
				this.DisableColorLayer(state, surface, MCColorCustomizationService.ColorLayer.Main3rd, "_Main3rdTex", num);
			}
		}

		// Token: 0x06003604 RID: 13828 RVA: 0x00136718 File Offset: 0x00134918
		private static MCSurfaceVariantSO FindNakedVariant(MCStyleCustomizationService.StyleRuntimeState state)
		{
			if (state == null || state.definition == null)
			{
				return null;
			}
			MCStylePresetSO mcstylePresetSO;
			if (!state.definition.TryGetStylePreset(state.styleIndex, out mcstylePresetSO) || mcstylePresetSO == null)
			{
				return null;
			}
			for (int i = 0; i < mcstylePresetSO.SurfaceCount; i++)
			{
				MCSurfaceVariantSO mcsurfaceVariantSO;
				if (mcstylePresetSO.TryGetSurfaceVariant(i, out mcsurfaceVariantSO) && mcsurfaceVariantSO != null && mcsurfaceVariantSO.colorProfile != null)
				{
					return mcsurfaceVariantSO;
				}
			}
			return null;
		}

		// Token: 0x06003605 RID: 13829 RVA: 0x00136790 File Offset: 0x00134990
		private static HashSet<MCColorCustomizationService.ColorLayer> ResolveOwnedColorLayers(MCSurfaceVariantSO nakedVariant)
		{
			HashSet<MCColorCustomizationService.ColorLayer> hashSet = new HashSet<MCColorCustomizationService.ColorLayer>();
			if (nakedVariant == null || nakedVariant.colorProfile == null || nakedVariant.colorProfile.channels == null)
			{
				return hashSet;
			}
			MCColorProfileSO colorProfile = nakedVariant.colorProfile;
			int num = 0;
			while (num < colorProfile.channels.Count && num < colorProfile.ClampedChannelCount)
			{
				MCColorProfileSO.ColorChannelBinding colorChannelBinding = colorProfile.channels[num];
				if (colorChannelBinding != null)
				{
					hashSet.Add(colorChannelBinding.shaderLayer);
				}
				num++;
			}
			return hashSet;
		}

		// Token: 0x06003606 RID: 13830 RVA: 0x00136810 File Offset: 0x00134A10
		private bool HasAuthoredMaterialBinding(MCStyleCustomizationService.StyleRuntimeState state, MCSurfaceVariantSO surface, Renderer renderer, int materialIndex)
		{
			if (surface == null || surface.materialBindings == null)
			{
				return false;
			}
			foreach (MCSurfaceVariantSO.MaterialTargetBinding materialTargetBinding in surface.materialBindings)
			{
				if (materialTargetBinding != null && !(materialTargetBinding.material == null) && (materialTargetBinding.materialIndex < 0 || materialTargetBinding.materialIndex == materialIndex) && this.MatchesTextureBindingTarget(state, renderer, materialTargetBinding.rendererTarget) && (string.IsNullOrWhiteSpace(materialTargetBinding.rendererName) || string.Equals(renderer.name, materialTargetBinding.rendererName, StringComparison.Ordinal)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003607 RID: 13831 RVA: 0x001368A4 File Offset: 0x00134AA4
		private void DisableColorLayer(MCStyleCustomizationService.StyleRuntimeState state, MCSurfaceVariantSO surface, MCColorCustomizationService.ColorLayer layer, string texturePropertyName, int scopedMaterialIndex)
		{
			string text;
			string text2;
			string text3;
			string text4;
			MCStyleCustomizationService.ResolveLayerDefaults(layer, out text, out text2, out text3, out text4);
			List<Renderer> activeRenderers = this.GetActiveRenderers(state);
			for (int i = 0; i < activeRenderers.Count; i++)
			{
				Renderer renderer = activeRenderers[i];
				if (!(renderer == null))
				{
					Material[] cachedMaterials = this.GetCachedMaterials(renderer);
					if (cachedMaterials != null)
					{
						int num = ((scopedMaterialIndex >= 0) ? scopedMaterialIndex : 0);
						int num2 = ((scopedMaterialIndex >= 0) ? (scopedMaterialIndex + 1) : cachedMaterials.Length);
						if (num < cachedMaterials.Length)
						{
							int num3 = num;
							while (num3 < num2 && num3 < cachedMaterials.Length)
							{
								if (!this.HasAuthoredMaterialBinding(state, surface, renderer, num3))
								{
									Material material = cachedMaterials[num3];
									if (!(material == null))
									{
										if (!string.IsNullOrWhiteSpace(text3) && material.HasProperty(text3))
										{
											material.SetFloat(text3, 0f);
										}
										if (!string.IsNullOrWhiteSpace(text2) && material.HasProperty(text2))
										{
											material.SetFloat(text2, 0f);
										}
										if (!string.IsNullOrWhiteSpace(text) && material.HasProperty(text))
										{
											material.SetColor(text, Color.white);
										}
										if (!string.IsNullOrWhiteSpace(texturePropertyName) && material.HasProperty(texturePropertyName))
										{
											material.SetTexture(texturePropertyName, null);
										}
										if (!string.IsNullOrWhiteSpace(text4) && material.HasProperty(text4))
										{
											material.SetTexture(text4, null);
										}
									}
								}
								num3++;
							}
						}
					}
				}
			}
		}

		// Token: 0x06003608 RID: 13832 RVA: 0x00136A10 File Offset: 0x00134C10
		private void ApplyChannelColor(MCStyleCustomizationService.StyleRuntimeState state, MCColorProfileSO profile, MCColorChannelId channelId, Color color)
		{
			this.ApplyChannelColor(state, profile, channelId, color, -1);
		}

		// Token: 0x06003609 RID: 13833 RVA: 0x00136A20 File Offset: 0x00134C20
		private void ApplyChannelColor(MCStyleCustomizationService.StyleRuntimeState state, MCColorProfileSO profile, MCColorChannelId channelId, Color color, int scopedMaterialIndex)
		{
			MCColorProfileSO.ColorChannelBinding colorChannelBinding = MCStyleCustomizationService.FindChannel(profile, channelId);
			if (colorChannelBinding == null)
			{
				return;
			}
			string text;
			string text2;
			string text3;
			string text4;
			MCStyleCustomizationService.ResolveLayerDefaults(colorChannelBinding.shaderLayer, out text, out text2, out text3, out text4);
			string text5 = (string.IsNullOrWhiteSpace(colorChannelBinding.colorPropertyNameOverride) ? text : colorChannelBinding.colorPropertyNameOverride);
			string text6 = (string.IsNullOrWhiteSpace(colorChannelBinding.blendModePropertyNameOverride) ? text2 : colorChannelBinding.blendModePropertyNameOverride);
			string text7 = (string.IsNullOrWhiteSpace(colorChannelBinding.enablePropertyNameOverride) ? text3 : colorChannelBinding.enablePropertyNameOverride);
			string text8 = (string.IsNullOrWhiteSpace(colorChannelBinding.maskPropertyNameOverride) ? text4 : colorChannelBinding.maskPropertyNameOverride);
			List<Renderer> activeRenderers = this.GetActiveRenderers(state);
			for (int i = 0; i < activeRenderers.Count; i++)
			{
				Renderer renderer = activeRenderers[i];
				if (!(renderer == null))
				{
					Material[] cachedMaterials = this.GetCachedMaterials(renderer);
					if (cachedMaterials != null)
					{
						int num = ((scopedMaterialIndex >= 0) ? scopedMaterialIndex : 0);
						int num2 = ((scopedMaterialIndex >= 0) ? (scopedMaterialIndex + 1) : cachedMaterials.Length);
						if (num < cachedMaterials.Length)
						{
							int num3 = num;
							while (num3 < num2 && num3 < cachedMaterials.Length)
							{
								Material material = cachedMaterials[num3];
								if (!(material == null))
								{
									if (!string.IsNullOrWhiteSpace(text5) && material.HasProperty(text5))
									{
										material.SetColor(text5, color);
									}
									if (!string.IsNullOrWhiteSpace(text6) && material.HasProperty(text6))
									{
										material.SetFloat(text6, 3f);
									}
									if (!string.IsNullOrWhiteSpace(text7) && material.HasProperty(text7))
									{
										material.SetFloat(text7, 1f);
									}
									if (!string.IsNullOrWhiteSpace(text8) && colorChannelBinding.maskTexture != null && material.HasProperty(text8))
									{
										material.SetTexture(text8, colorChannelBinding.maskTexture);
									}
								}
								num3++;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600360A RID: 13834 RVA: 0x00136BEC File Offset: 0x00134DEC
		private static void ResolveLayerDefaults(MCColorCustomizationService.ColorLayer layer, out string colorProperty, out string blendModeProperty, out string enableProperty, out string maskProperty)
		{
			if (layer == MCColorCustomizationService.ColorLayer.Main3rd)
			{
				colorProperty = "_Color3rd";
				blendModeProperty = "_Main3rdTexBlendMode";
				enableProperty = "_UseMain3rdTex";
				maskProperty = "_Main3rdBlendMask";
				return;
			}
			colorProperty = "_Color2nd";
			blendModeProperty = "_Main2ndTexBlendMode";
			enableProperty = "_UseMain2ndTex";
			maskProperty = "_Main2ndBlendMask";
		}

		// Token: 0x0600360B RID: 13835 RVA: 0x00136C38 File Offset: 0x00134E38
		private bool TryGetSelectedStyle(out MCStylePresetSO preset)
		{
			preset = null;
			return this._activeState != null && !(this._activeState.definition == null) && !this._activeState.isUsingSceneDefault && !this._activeState.isUsingNoSelection && this._activeState.definition.TryGetStylePreset(this._activeState.styleIndex, out preset) && preset != null;
		}

		// Token: 0x0600360C RID: 13836 RVA: 0x00136CA8 File Offset: 0x00134EA8
		private static void ApplyDefinitionDefaultSelection(MCStyleCustomizationService.StyleRuntimeState state, MCStylePartDefinitionSO definition)
		{
			if (state == null || definition == null)
			{
				return;
			}
			state.isUsingNoSelection = false;
			state.isUsingSceneDefault = false;
			state.styleIndex = -1;
			state.surfaceIndex = -1;
			state.colorPresetIndex = -1;
			if (definition.allowNoSelection && definition.defaultToNoSelection)
			{
				state.isUsingNoSelection = true;
				return;
			}
			if (definition.defaultBehavior == MCStyleDefaultBehavior.KeepSceneState)
			{
				state.isUsingSceneDefault = true;
				return;
			}
			if (definition.defaultBehavior != MCStyleDefaultBehavior.ApplyConfiguredDefault)
			{
				return;
			}
			state.styleIndex = definition.ResolveDefaultStyleIndex();
			MCStylePresetSO mcstylePresetSO;
			if (definition.TryGetStylePreset(state.styleIndex, out mcstylePresetSO) && mcstylePresetSO != null)
			{
				state.surfaceIndex = mcstylePresetSO.ResolveDefaultSurfaceIndex();
				state.colorPresetIndex = MCStyleCustomizationService.ResolveDefaultColorPresetIndex(mcstylePresetSO, state.surfaceIndex);
			}
		}

		// Token: 0x0600360D RID: 13837 RVA: 0x00136D5C File Offset: 0x00134F5C
		private bool TryGetSelectedSurface(out MCSurfaceVariantSO surface)
		{
			surface = null;
			MCStylePresetSO mcstylePresetSO;
			return this.TryGetSelectedStyle(out mcstylePresetSO) && mcstylePresetSO.TryGetSurfaceVariant(this._activeState.surfaceIndex, out surface) && surface != null;
		}

		// Token: 0x0600360E RID: 13838 RVA: 0x00136D98 File Offset: 0x00134F98
		private MCColorProfileSO GetActiveColorProfile()
		{
			MCSurfaceVariantSO mcsurfaceVariantSO;
			if (this.TryGetSelectedSurface(out mcsurfaceVariantSO) && mcsurfaceVariantSO.colorProfile != null)
			{
				return mcsurfaceVariantSO.colorProfile;
			}
			return null;
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x00136DC8 File Offset: 0x00134FC8
		private static MCColorProfileSO.ColorChannelBinding FindChannel(MCColorProfileSO profile, MCColorChannelId channelId)
		{
			if (profile == null || profile.channels == null)
			{
				return null;
			}
			for (int i = 0; i < profile.channels.Count; i++)
			{
				MCColorProfileSO.ColorChannelBinding colorChannelBinding = profile.channels[i];
				if (colorChannelBinding != null && colorChannelBinding.channelId == channelId)
				{
					return colorChannelBinding;
				}
			}
			return null;
		}

		// Token: 0x06003610 RID: 13840 RVA: 0x00136E1C File Offset: 0x0013501C
		private static int ResolveDefaultColorPresetIndex(MCStylePresetSO preset, int surfaceIndex)
		{
			if (preset == null)
			{
				return -1;
			}
			MCSurfaceVariantSO mcsurfaceVariantSO;
			if (!preset.TryGetSurfaceVariant(surfaceIndex, out mcsurfaceVariantSO) || mcsurfaceVariantSO == null || mcsurfaceVariantSO.colorProfile == null || mcsurfaceVariantSO.colorProfile.presets == null || mcsurfaceVariantSO.colorProfile.presets.Count == 0)
			{
				return -1;
			}
			return Mathf.Clamp(mcsurfaceVariantSO.colorProfile.defaultPresetIndex, 0, mcsurfaceVariantSO.colorProfile.presets.Count - 1);
		}

		// Token: 0x06003611 RID: 13841 RVA: 0x00136E9C File Offset: 0x0013509C
		private static bool TryResolvePresetColor(MCColorProfileSO profile, MCStyleCustomizationService.StyleRuntimeState state, MCColorChannelId channelId, out Color color)
		{
			color = Color.white;
			if (profile == null || state == null)
			{
				return false;
			}
			if (channelId == MCColorChannelId.Primary && state.hasCustomPrimary)
			{
				color = state.customPrimary;
				return true;
			}
			if (channelId == MCColorChannelId.Secondary && state.hasCustomSecondary)
			{
				color = state.customSecondary;
				return true;
			}
			MCColorProfileSO.ColorPresetEntry colorPresetEntry;
			if (profile.TryGetPreset(state.colorPresetIndex, out colorPresetEntry) && colorPresetEntry != null)
			{
				color = ((channelId == MCColorChannelId.Primary) ? colorPresetEntry.primaryColor : colorPresetEntry.secondaryColor);
				return true;
			}
			if (profile.presets != null && profile.presets.Count > 0 && profile.presets[0] != null)
			{
				MCColorProfileSO.ColorPresetEntry colorPresetEntry2 = profile.presets[0];
				color = ((channelId == MCColorChannelId.Primary) ? colorPresetEntry2.primaryColor : colorPresetEntry2.secondaryColor);
				return true;
			}
			return false;
		}

		// Token: 0x06003612 RID: 13842 RVA: 0x00136F6C File Offset: 0x0013516C
		private static void ApplyTextureIfPresent(Material material, string propertyName, Texture texture)
		{
			if (material == null || string.IsNullOrWhiteSpace(propertyName) || !material.HasProperty(propertyName))
			{
				return;
			}
			material.SetTexture(propertyName, texture);
		}

		// Token: 0x06003613 RID: 13843 RVA: 0x00136F91 File Offset: 0x00135191
		private static Color EncodeStateValue(int index, bool isSceneDefault)
		{
			return new Color((float)Mathf.Clamp(index + 1000, 0, 2000) / 2000f, isSceneDefault ? 1f : 0f, 0f, 1f);
		}

		// Token: 0x06003614 RID: 13844 RVA: 0x00136FCA File Offset: 0x001351CA
		private static void DecodeStateValue(Color color, out int index, out bool isSceneDefault)
		{
			index = Mathf.RoundToInt(color.r * 2000f) - 1000;
			isSceneDefault = color.g > 0.5f;
		}

		// Token: 0x06003615 RID: 13845 RVA: 0x00136FF4 File Offset: 0x001351F4
		private static Color EncodeStateFloat(float value)
		{
			return new Color(value, 0f, 0f, 1f);
		}

		// Token: 0x06003616 RID: 13846 RVA: 0x0013700B File Offset: 0x0013520B
		private static float DecodeStateFloat(Color color)
		{
			return color.r;
		}

		// Token: 0x06003617 RID: 13847 RVA: 0x00137014 File Offset: 0x00135214
		private Material[] GetCachedMaterials(Renderer renderer)
		{
			if (renderer == null)
			{
				return null;
			}
			Material[] array;
			bool flag = this._materialInstanceCache.TryGetValue(renderer, out array) && array != null;
			Material[] sharedMaterials = renderer.sharedMaterials;
			if (flag && sharedMaterials.Length == array.Length)
			{
				bool flag2 = true;
				for (int i = 0; i < sharedMaterials.Length; i++)
				{
					if (sharedMaterials[i] != array[i])
					{
						flag2 = false;
						break;
					}
				}
				if (flag2)
				{
					return array;
				}
			}
			array = new Material[sharedMaterials.Length];
			for (int j = 0; j < sharedMaterials.Length; j++)
			{
				Material material = sharedMaterials[j];
				if (material == null || this._ownedMaterialSources.ContainsKey(material))
				{
					array[j] = material;
				}
				else
				{
					Material material2 = new Material(material)
					{
						name = material.name + " (Instance)"
					};
					this._ownedMaterialSources.Add(material2, material);
					array[j] = material2;
				}
			}
			renderer.sharedMaterials = array;
			this._materialInstanceCache[renderer] = array;
			this._ownedMaterialRenderers.Add(renderer);
			if (flag)
			{
				this.ReleaseUnusedOwnedMaterials();
			}
			return array;
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x00137124 File Offset: 0x00135324
		private void RestoreOwnedMaterials(Transform root)
		{
			if (root == null)
			{
				return;
			}
			foreach (Renderer renderer in root.GetComponentsInChildren<Renderer>(true))
			{
				Material[] sharedMaterials = renderer.sharedMaterials;
				bool flag = false;
				for (int j = 0; j < sharedMaterials.Length; j++)
				{
					Material material;
					if (sharedMaterials[j] != null && this._ownedMaterialSources.TryGetValue(sharedMaterials[j], out material))
					{
						sharedMaterials[j] = material;
						flag = true;
					}
				}
				if (flag)
				{
					renderer.sharedMaterials = sharedMaterials;
				}
				this._materialInstanceCache.Remove(renderer);
				this._ownedMaterialRenderers.Remove(renderer);
			}
		}

		// Token: 0x06003619 RID: 13849 RVA: 0x001371C0 File Offset: 0x001353C0
		private void ReleaseUnusedOwnedMaterials()
		{
			if (this._ownedMaterialSources.Count == 0)
			{
				return;
			}
			HashSet<Material> hashSet = new HashSet<Material>();
			Animator componentInParent = base.GetComponentInParent<Animator>();
			Renderer[] componentsInChildren = (((componentInParent != null) ? componentInParent.transform : null) ?? base.transform).GetComponentsInChildren<Renderer>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				foreach (Material material in componentsInChildren[i].sharedMaterials)
				{
					if (material != null)
					{
						hashSet.Add(material);
					}
				}
			}
			List<Renderer> list = new List<Renderer>();
			foreach (Renderer renderer in this._ownedMaterialRenderers)
			{
				if (renderer == null)
				{
					list.Add(renderer);
				}
				else
				{
					foreach (Material material2 in renderer.sharedMaterials)
					{
						if (material2 != null)
						{
							hashSet.Add(material2);
						}
					}
				}
			}
			foreach (Renderer renderer2 in list)
			{
				this._materialInstanceCache.Remove(renderer2);
				this._ownedMaterialRenderers.Remove(renderer2);
			}
			List<Material> list2 = new List<Material>();
			foreach (Material material3 in this._ownedMaterialSources.Keys)
			{
				if (material3 == null || !hashSet.Contains(material3))
				{
					list2.Add(material3);
				}
			}
			foreach (Material material4 in list2)
			{
				this._ownedMaterialSources.Remove(material4);
				if (!(material4 == null))
				{
					if (Application.isPlaying)
					{
						Object.Destroy(material4);
					}
					else
					{
						Object.DestroyImmediate(material4);
					}
				}
			}
		}

		// Token: 0x0600361A RID: 13850 RVA: 0x00137400 File Offset: 0x00135600
		private List<Renderer> GetActiveRenderers(MCStyleCustomizationService.StyleRuntimeState state)
		{
			List<Renderer> list = new List<Renderer>();
			if (state == null || state.definition == null)
			{
				return list;
			}
			if (state.spawnedGO != null)
			{
				foreach (Renderer renderer in state.spawnedGO.GetComponentsInChildren<Renderer>(true))
				{
					if (renderer != null && !list.Contains(renderer))
					{
						list.Add(renderer);
					}
				}
			}
			List<Renderer> list2;
			if (list.Count == 0 && state.definition.partKind == MCStylePartKind.Virtual && this._virtualRenderersByPartId.TryGetValue(state.definition.partId, out list2))
			{
				for (int j = 0; j < list2.Count; j++)
				{
					Renderer renderer2 = list2[j];
					if (renderer2 != null && !list.Contains(renderer2))
					{
						list.Add(renderer2);
					}
				}
			}
			return list;
		}

		// Token: 0x0600361B RID: 13851 RVA: 0x001374DC File Offset: 0x001356DC
		private void ApplyShaderOverrides(MCStyleCustomizationService.StyleRuntimeState state, MCSurfaceVariantSO surface)
		{
			if (state == null || surface == null || surface.shaderOverrides == null)
			{
				return;
			}
			Dictionary<string, float> dictionary = null;
			if (state.definition != null)
			{
				this._shaderSlidersByPartId.TryGetValue(state.definition.partId, out dictionary);
			}
			List<Renderer> activeRenderers = this.GetActiveRenderers(state);
			for (int i = 0; i < activeRenderers.Count; i++)
			{
				Renderer renderer = activeRenderers[i];
				if (!(renderer == null))
				{
					Material[] cachedMaterials = this.GetCachedMaterials(renderer);
					if (cachedMaterials != null)
					{
						for (int j = 0; j < cachedMaterials.Length; j++)
						{
							if (cachedMaterials[j] != null)
							{
								surface.shaderOverrides.ApplyToMaterial(cachedMaterials[j], dictionary);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600361C RID: 13852 RVA: 0x00137598 File Offset: 0x00135798
		public bool SetShaderSliderValue(string propertyName, float value)
		{
			if (this._activeState == null || this._activeState.definition == null || string.IsNullOrEmpty(propertyName))
			{
				return false;
			}
			MCSurfaceVariantSO mcsurfaceVariantSO;
			if (!this.TryGetSelectedSurface(out mcsurfaceVariantSO) || mcsurfaceVariantSO == null || mcsurfaceVariantSO.shaderOverrides == null)
			{
				return false;
			}
			string partId = this._activeState.definition.partId;
			Dictionary<string, float> dictionary;
			if (!this._shaderSlidersByPartId.TryGetValue(partId, out dictionary))
			{
				dictionary = new Dictionary<string, float>(StringComparer.Ordinal);
				this._shaderSlidersByPartId[partId] = dictionary;
			}
			dictionary[propertyName] = value;
			this.ApplyShaderOverrides(this._activeState, mcsurfaceVariantSO);
			return true;
		}

		// Token: 0x0600361D RID: 13853 RVA: 0x0013763C File Offset: 0x0013583C
		public float GetShaderSliderValue(string propertyName)
		{
			if (this._activeState == null || this._activeState.definition == null || string.IsNullOrEmpty(propertyName))
			{
				return 0f;
			}
			string partId = this._activeState.definition.partId;
			Dictionary<string, float> dictionary;
			float num;
			if (this._shaderSlidersByPartId.TryGetValue(partId, out dictionary) && dictionary.TryGetValue(propertyName, out num))
			{
				return num;
			}
			MCSurfaceVariantSO mcsurfaceVariantSO;
			if (!this.TryGetSelectedSurface(out mcsurfaceVariantSO) || mcsurfaceVariantSO == null || mcsurfaceVariantSO.shaderOverrides == null)
			{
				return 0f;
			}
			List<MCShaderParameterOverrideSO.ShaderOverrideEntry> entries = mcsurfaceVariantSO.shaderOverrides.entries;
			if (entries != null)
			{
				for (int i = 0; i < entries.Count; i++)
				{
					MCShaderParameterOverrideSO.ShaderOverrideEntry shaderOverrideEntry = entries[i];
					if (shaderOverrideEntry != null && string.Equals(shaderOverrideEntry.propertyName, propertyName, StringComparison.Ordinal))
					{
						return shaderOverrideEntry.floatValue;
					}
				}
			}
			return 0f;
		}

		// Token: 0x0600361E RID: 13854 RVA: 0x0013771C File Offset: 0x0013591C
		public float GetShaderSliderDefaultValue(string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				return 0f;
			}
			MCSurfaceVariantSO mcsurfaceVariantSO;
			if (!this.TryGetSelectedSurface(out mcsurfaceVariantSO) || mcsurfaceVariantSO == null || mcsurfaceVariantSO.shaderOverrides == null)
			{
				return 0f;
			}
			List<MCShaderParameterOverrideSO.ShaderOverrideEntry> entries = mcsurfaceVariantSO.shaderOverrides.entries;
			if (entries == null)
			{
				return 0f;
			}
			for (int i = 0; i < entries.Count; i++)
			{
				MCShaderParameterOverrideSO.ShaderOverrideEntry shaderOverrideEntry = entries[i];
				if (shaderOverrideEntry != null && shaderOverrideEntry.type == MCShaderParameterOverrideSO.OverrideType.Float && string.Equals(shaderOverrideEntry.propertyName, propertyName, StringComparison.Ordinal))
				{
					return shaderOverrideEntry.floatValue;
				}
			}
			return 0f;
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x001377B4 File Offset: 0x001359B4
		public List<MCShaderParameterOverrideSO.ShaderOverrideEntry> GetActiveExposedSliders()
		{
			MCSurfaceVariantSO mcsurfaceVariantSO;
			if (!this.TryGetSelectedSurface(out mcsurfaceVariantSO) || mcsurfaceVariantSO == null || mcsurfaceVariantSO.shaderOverrides == null)
			{
				return new List<MCShaderParameterOverrideSO.ShaderOverrideEntry>();
			}
			return mcsurfaceVariantSO.shaderOverrides.GetExposedSliders();
		}

		// Token: 0x06003620 RID: 13856 RVA: 0x001377F4 File Offset: 0x001359F4
		public bool HasActiveExposedSliders()
		{
			MCSurfaceVariantSO mcsurfaceVariantSO;
			return this.TryGetSelectedSurface(out mcsurfaceVariantSO) && mcsurfaceVariantSO != null && mcsurfaceVariantSO.shaderOverrides != null && mcsurfaceVariantSO.shaderOverrides.HasExposedSliders;
		}

		// Token: 0x170009C1 RID: 2497
		// (get) Token: 0x06003621 RID: 13857 RVA: 0x00137830 File Offset: 0x00135A30
		private string HairMorphSource
		{
			get
			{
				return "MCStyleHairMorph:" + base.GetInstanceID().ToString();
			}
		}

		// Token: 0x06003622 RID: 13858 RVA: 0x00137858 File Offset: 0x00135A58
		public List<MCStylePresetSO.HairMorphControl> GetActiveHairMorphControls()
		{
			List<MCStylePresetSO.HairMorphControl> list = new List<MCStylePresetSO.HairMorphControl>();
			MCStylePresetSO mcstylePresetSO = this.ActiveHairMorphPreset();
			if (((mcstylePresetSO != null) ? mcstylePresetSO.hairMorphControls : null) == null)
			{
				return list;
			}
			HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
			foreach (MCStylePresetSO.HairMorphControl hairMorphControl in mcstylePresetSO.hairMorphControls)
			{
				if (MCStyleCustomizationService.ValidHairControl(hairMorphControl) && hashSet.Add(hairMorphControl.id))
				{
					list.Add(MCStyleCustomizationService.CloneHairControl(hairMorphControl));
				}
			}
			return list;
		}

		// Token: 0x06003623 RID: 13859 RVA: 0x001378F4 File Offset: 0x00135AF4
		public float GetHairMorphValue(string id)
		{
			MCStylePresetSO mcstylePresetSO = this.ActiveHairMorphPreset();
			MCStylePresetSO.HairMorphControl hairMorphControl = MCStyleCustomizationService.FindHairControl(mcstylePresetSO, id);
			if (hairMorphControl != null)
			{
				return this.StoredHairMorphValue(mcstylePresetSO.presetId, hairMorphControl);
			}
			return 0f;
		}

		// Token: 0x06003624 RID: 13860 RVA: 0x00137928 File Offset: 0x00135B28
		public bool SetHairMorphValue(string id, float value)
		{
			if (!MCStyleCustomizationService.FiniteHairValue(value))
			{
				return false;
			}
			MCStylePresetSO mcstylePresetSO = this.ActiveHairMorphPreset();
			MCStylePresetSO.HairMorphControl hairMorphControl = MCStyleCustomizationService.FindHairControl(mcstylePresetSO, id);
			if (hairMorphControl == null)
			{
				return false;
			}
			value = MCStyleCustomizationService.ClampHairControlValue(hairMorphControl, value);
			Dictionary<string, float> dictionary;
			if (!this._hairMorphValues.TryGetValue(mcstylePresetSO.presetId, out dictionary))
			{
				dictionary = (this._hairMorphValues[mcstylePresetSO.presetId] = new Dictionary<string, float>(StringComparer.Ordinal));
			}
			if (Mathf.Approximately(value, hairMorphControl.defaultValue))
			{
				dictionary.Remove(id);
			}
			else
			{
				dictionary[id] = value;
			}
			if (dictionary.Count == 0)
			{
				this._hairMorphValues.Remove(mcstylePresetSO.presetId);
			}
			this.RefreshHairMorphs();
			return true;
		}

		// Token: 0x06003625 RID: 13861 RVA: 0x001379D0 File Offset: 0x00135BD0
		private MCStylePresetSO ActiveHairMorphPreset()
		{
			MCStyleCustomizationService.StyleRuntimeState activeState = this._activeState;
			if (((activeState != null) ? activeState.definition : null) == null || this._activeState.definition.partId != "Head.Hair" || this._activeState.isUsingNoSelection || this._activeState.isUsingSceneDefault)
			{
				return null;
			}
			MCStylePresetSO mcstylePresetSO;
			if (!this._activeState.definition.TryGetStylePreset(this._activeState.styleIndex, out mcstylePresetSO) || !(mcstylePresetSO != null) || mcstylePresetSO.isVaulted)
			{
				return null;
			}
			return mcstylePresetSO;
		}

		// Token: 0x06003626 RID: 13862 RVA: 0x00137A64 File Offset: 0x00135C64
		private void CaptureHairMorphValues(CharacterStateData state)
		{
			state.styleMorphs = new List<StyleMorphValue>();
			foreach (KeyValuePair<string, Dictionary<string, float>> keyValuePair in this._hairMorphValues)
			{
				foreach (KeyValuePair<string, float> keyValuePair2 in keyValuePair.Value)
				{
					state.styleMorphs.Add(new StyleMorphValue("Head.Hair", keyValuePair.Key, keyValuePair2.Key, keyValuePair2.Value));
				}
			}
			state.styleMorphs.Sort(delegate(StyleMorphValue a, StyleMorphValue b)
			{
				int num = string.CompareOrdinal(a.presetId, b.presetId);
				if (num == 0)
				{
					return string.CompareOrdinal(a.controlId, b.controlId);
				}
				return num;
			});
		}

		// Token: 0x06003627 RID: 13863 RVA: 0x00137B4C File Offset: 0x00135D4C
		private void ApplyHairMorphValues(CharacterStateData state)
		{
			this.ClearHairMorphValues();
			if (((state != null) ? state.styleMorphs : null) == null)
			{
				return;
			}
			MCStylePartDefinitionSO definition = this.GetDefinition("Head.Hair");
			if (((definition != null) ? definition.stylePresets : null) == null)
			{
				return;
			}
			using (List<StyleMorphValue>.Enumerator enumerator = state.styleMorphs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					StyleMorphValue entry = enumerator.Current;
					if (entry != null && !(entry.partId != "Head.Hair") && MCStyleCustomizationService.FiniteHairValue(entry.value))
					{
						MCStylePresetSO mcstylePresetSO = definition.stylePresets.Find((MCStylePresetSO candidate) => candidate != null && !candidate.isVaulted && candidate.presetId == entry.presetId);
						MCStylePresetSO.HairMorphControl hairMorphControl = MCStyleCustomizationService.FindHairControl(mcstylePresetSO, entry.controlId);
						if (hairMorphControl != null)
						{
							float num = MCStyleCustomizationService.ClampHairControlValue(hairMorphControl, entry.value);
							if (!Mathf.Approximately(num, hairMorphControl.defaultValue))
							{
								Dictionary<string, float> dictionary;
								if (!this._hairMorphValues.TryGetValue(mcstylePresetSO.presetId, out dictionary))
								{
									dictionary = (this._hairMorphValues[mcstylePresetSO.presetId] = new Dictionary<string, float>(StringComparer.Ordinal));
								}
								dictionary[hairMorphControl.id] = num;
							}
						}
					}
				}
			}
		}

		// Token: 0x06003628 RID: 13864 RVA: 0x00137CAC File Offset: 0x00135EAC
		private void ClearHairMorphValues()
		{
			this.ReleaseHairMorphs();
			this._hairMorphValues.Clear();
		}

		// Token: 0x06003629 RID: 13865 RVA: 0x00137CBF File Offset: 0x00135EBF
		private void ReleaseHairMorphs()
		{
			if (this._hairMorphManager != null)
			{
				this._hairMorphManager.RemoveRendererOverridesFromSource(this.HairMorphSource);
			}
			this._hairMorphManager = null;
		}

		// Token: 0x0600362A RID: 13866 RVA: 0x00137CE8 File Offset: 0x00135EE8
		private void RefreshHairMorphs()
		{
			this.ReleaseHairMorphs();
			MCStyleCustomizationService.StyleRuntimeState styleRuntimeState;
			MCStylePresetSO mcstylePresetSO;
			Dictionary<string, float> dictionary;
			if (!base.isActiveAndEnabled || !this._stateByPartId.TryGetValue("Head.Hair", out styleRuntimeState) || styleRuntimeState.isUsingNoSelection || styleRuntimeState.isUsingSceneDefault || styleRuntimeState.spawnedGO == null || !styleRuntimeState.definition.TryGetStylePreset(styleRuntimeState.styleIndex, out mcstylePresetSO) || mcstylePresetSO == null || mcstylePresetSO.isVaulted || mcstylePresetSO.physicalPrefab == null || mcstylePresetSO.hairMorphControls == null || !this._hairMorphValues.TryGetValue(mcstylePresetSO.presetId, out dictionary))
			{
				return;
			}
			BlendShapeManager blendShapeManager = this.ResolveBlendShapeManager();
			if (blendShapeManager == null)
			{
				return;
			}
			SkinnedMeshRenderer[] componentsInChildren = styleRuntimeState.spawnedGO.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			SkinnedMeshRenderer[] componentsInChildren2 = mcstylePresetSO.physicalPrefab.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			foreach (MCStylePresetSO.HairMorphControl hairMorphControl in mcstylePresetSO.hairMorphControls)
			{
				float num;
				if (MCStyleCustomizationService.ValidHairControl(hairMorphControl) && dictionary.TryGetValue(hairMorphControl.id, out num))
				{
					foreach (MCStylePresetSO.HairMorphBinding hairMorphBinding in hairMorphControl.bindings)
					{
						SkinnedMeshRenderer skinnedMeshRenderer = MCStyleCustomizationService.UniqueHairRenderer(componentsInChildren, hairMorphBinding.rendererName);
						SkinnedMeshRenderer skinnedMeshRenderer2 = MCStyleCustomizationService.UniqueHairRenderer(componentsInChildren2, hairMorphBinding.rendererName);
						if (!(skinnedMeshRenderer == null) && !(skinnedMeshRenderer2 == null) && !(skinnedMeshRenderer2.sharedMesh == null))
						{
							int blendShapeIndex = skinnedMeshRenderer2.sharedMesh.GetBlendShapeIndex(hairMorphBinding.blendShapeName);
							if (blendShapeIndex >= 0)
							{
								float num2 = MCStyleCustomizationService.HairBindingValue(hairMorphControl, hairMorphBinding, num);
								if (blendShapeManager.SetRendererOverride(skinnedMeshRenderer, hairMorphBinding.blendShapeName, num2, this.HairMorphSource, BlendShapePriority.Customization, new float?(skinnedMeshRenderer2.GetBlendShapeWeight(blendShapeIndex)), MCStyleCustomizationService.HairFitShapes, true))
								{
									this._hairMorphManager = blendShapeManager;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600362B RID: 13867 RVA: 0x00137F1C File Offset: 0x0013611C
		private static SkinnedMeshRenderer UniqueHairRenderer(SkinnedMeshRenderer[] renderers, string name)
		{
			SkinnedMeshRenderer skinnedMeshRenderer = null;
			foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in renderers)
			{
				if (skinnedMeshRenderer2.name == name)
				{
					if (skinnedMeshRenderer != null)
					{
						return null;
					}
					skinnedMeshRenderer = skinnedMeshRenderer2;
				}
			}
			return skinnedMeshRenderer;
		}

		// Token: 0x0600362C RID: 13868 RVA: 0x00137F5C File Offset: 0x0013615C
		private float StoredHairMorphValue(string presetId, MCStylePresetSO.HairMorphControl control)
		{
			Dictionary<string, float> dictionary;
			float num;
			if (!this._hairMorphValues.TryGetValue(presetId, out dictionary) || !dictionary.TryGetValue(control.id, out num))
			{
				return control.defaultValue;
			}
			return MCStyleCustomizationService.ClampHairControlValue(control, num);
		}

		// Token: 0x0600362D RID: 13869 RVA: 0x00137F98 File Offset: 0x00136198
		private static MCStylePresetSO.HairMorphControl FindHairControl(MCStylePresetSO preset, string id)
		{
			if (string.IsNullOrEmpty(id) || ((preset != null) ? preset.hairMorphControls : null) == null)
			{
				return null;
			}
			MCStylePresetSO.HairMorphControl hairMorphControl = null;
			foreach (MCStylePresetSO.HairMorphControl hairMorphControl2 in preset.hairMorphControls)
			{
				if (hairMorphControl2 != null && hairMorphControl2.id == id && MCStyleCustomizationService.ValidHairControl(hairMorphControl2))
				{
					if (hairMorphControl != null)
					{
						return null;
					}
					hairMorphControl = hairMorphControl2;
				}
			}
			return hairMorphControl;
		}

		// Token: 0x0600362E RID: 13870 RVA: 0x00138024 File Offset: 0x00136224
		private static bool ValidHairControl(MCStylePresetSO.HairMorphControl control)
		{
			if (control == null || string.IsNullOrWhiteSpace(control.id) || !MCStyleCustomizationService.FiniteHairValue(control.min) || !MCStyleCustomizationService.FiniteHairValue(control.max) || !MCStyleCustomizationService.FiniteHairValue(control.defaultValue) || !MCStyleCustomizationService.FiniteHairValue(control.step) || control.min >= control.max || control.defaultValue < control.min || control.defaultValue > control.max || control.step < 0f || control.bindings == null || control.bindings.Count == 0)
			{
				return false;
			}
			foreach (MCStylePresetSO.HairMorphBinding hairMorphBinding in control.bindings)
			{
				if (hairMorphBinding == null || string.IsNullOrWhiteSpace(hairMorphBinding.rendererName) || string.IsNullOrWhiteSpace(hairMorphBinding.blendShapeName) || Array.IndexOf<string>(MCStyleCustomizationService.HairFitShapes, hairMorphBinding.blendShapeName) >= 0 || !MCStyleCustomizationService.FiniteHairValue(hairMorphBinding.minValue) || !MCStyleCustomizationService.FiniteHairValue(hairMorphBinding.defaultValue) || !MCStyleCustomizationService.FiniteHairValue(hairMorphBinding.maxValue))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x00138168 File Offset: 0x00136368
		private static float ClampHairControlValue(MCStylePresetSO.HairMorphControl control, float value)
		{
			value = Mathf.Clamp(value, control.min, control.max);
			if (Mathf.Approximately(value, control.defaultValue))
			{
				return control.defaultValue;
			}
			if (control.step > 0f)
			{
				value = control.min + Mathf.Round((value - control.min) / control.step) * control.step;
			}
			return Mathf.Clamp(value, control.min, control.max);
		}

		// Token: 0x06003630 RID: 13872 RVA: 0x001381E4 File Offset: 0x001363E4
		private static float HairBindingValue(MCStylePresetSO.HairMorphControl control, MCStylePresetSO.HairMorphBinding binding, float value)
		{
			if (value < control.defaultValue)
			{
				return Mathf.Lerp(binding.minValue, binding.defaultValue, Mathf.InverseLerp(control.min, control.defaultValue, value));
			}
			if (value > control.defaultValue)
			{
				return Mathf.Lerp(binding.defaultValue, binding.maxValue, Mathf.InverseLerp(control.defaultValue, control.max, value));
			}
			return binding.defaultValue;
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x00138254 File Offset: 0x00136454
		private static MCStylePresetSO.HairMorphControl CloneHairControl(MCStylePresetSO.HairMorphControl source)
		{
			MCStylePresetSO.HairMorphControl hairMorphControl = new MCStylePresetSO.HairMorphControl
			{
				id = source.id,
				displayName = source.displayName,
				min = source.min,
				max = source.max,
				step = source.step,
				defaultValue = source.defaultValue
			};
			foreach (MCStylePresetSO.HairMorphBinding hairMorphBinding in source.bindings)
			{
				hairMorphControl.bindings.Add(new MCStylePresetSO.HairMorphBinding
				{
					rendererName = hairMorphBinding.rendererName,
					blendShapeName = hairMorphBinding.blendShapeName,
					minValue = hairMorphBinding.minValue,
					defaultValue = hairMorphBinding.defaultValue,
					maxValue = hairMorphBinding.maxValue
				});
			}
			return hairMorphControl;
		}

		// Token: 0x06003632 RID: 13874 RVA: 0x0002920B File Offset: 0x0002740B
		private static bool FiniteHairValue(float value)
		{
			return !float.IsNaN(value) && !float.IsInfinity(value);
		}

		// Token: 0x040035D2 RID: 13778
		[Header("Definitions")]
		[SerializeField]
		private List<MCStylePartDefinitionSO> partDefinitions = new List<MCStylePartDefinitionSO>();

		// Token: 0x040035D3 RID: 13779
		[Header("Virtual Part Renderers")]
		[Tooltip("Map virtual parts (Skin, Eyes, Eyebrows) to their scene renderers. Skin needs both Head+Body SMR; Eyes/Eyebrows need Head SMR only.")]
		[SerializeField]
		private List<MCStyleCustomizationService.VirtualPartRendererBinding> virtualPartRenderers = new List<MCStyleCustomizationService.VirtualPartRendererBinding>();

		// Token: 0x040035D4 RID: 13780
		[Header("Runtime Dependencies")]
		[Tooltip("Handles bone rebinding and physics bone reparenting for physical outfit prefabs.")]
		[SerializeField]
		private PlayerDresser playerDresser;

		// Token: 0x040035D5 RID: 13781
		[Tooltip("Optional: update clothing group references after physical swap.")]
		[SerializeField]
		private ClothingToggleController clothingToggleController;

		// Token: 0x040035D6 RID: 13782
		[Tooltip("Optional: update renderer references after physical swap.")]
		[SerializeField]
		private SkinDecalController skinDecalController;

		// Token: 0x040035D7 RID: 13783
		[Header("Layers (spawned parts)")]
		[Tooltip("Layer assegnato ai capelli spawnati dinamicamente (partId che contiene 'Hair').")]
		[SerializeField]
		private string hairLayerName = "Player";

		// Token: 0x040035D8 RID: 13784
		[Tooltip("Layer assegnato ai vestiti spawnati dinamicamente (estetici e funzionali).")]
		[SerializeField]
		private string clothingLayerName = "PlayerClothing";

		// Token: 0x040035D9 RID: 13785
		[Header("Initialization")]
		[SerializeField]
		private bool initializeOnStart = true;

		// Token: 0x040035DA RID: 13786
		[Tooltip("Own runtime copies of these definitions so presentation-only wardrobe additions cannot mutate their assets.")]
		[SerializeField]
		private bool useOwnedRuntimeDefinitions;

		// Token: 0x040035DB RID: 13787
		private bool _runtimeDefinitionsOwned;

		// Token: 0x040035DC RID: 13788
		private readonly List<MCStylePartDefinitionSO> _ownedRuntimeDefinitions = new List<MCStylePartDefinitionSO>();

		// Token: 0x040035DD RID: 13789
		private readonly Dictionary<string, MCStyleCustomizationService.StyleRuntimeState> _stateByPartId = new Dictionary<string, MCStyleCustomizationService.StyleRuntimeState>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040035DE RID: 13790
		private readonly MCStyleHairPhysicsOwnership _hairPhysicsOwnership = new MCStyleHairPhysicsOwnership();

		// Token: 0x040035DF RID: 13791
		private readonly Dictionary<string, List<Renderer>> _virtualRenderersByPartId = new Dictionary<string, List<Renderer>>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040035E0 RID: 13792
		private readonly Dictionary<string, Dictionary<Renderer, MCSurfaceVariantSO.RendererTargetType>> _virtualRendererTargetsByPartId = new Dictionary<string, Dictionary<Renderer, MCSurfaceVariantSO.RendererTargetType>>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040035E1 RID: 13793
		private readonly Dictionary<Renderer, Material[]> _materialInstanceCache = new Dictionary<Renderer, Material[]>();

		// Token: 0x040035E2 RID: 13794
		private readonly Dictionary<Material, Material> _ownedMaterialSources = new Dictionary<Material, Material>();

		// Token: 0x040035E3 RID: 13795
		private readonly HashSet<Renderer> _ownedMaterialRenderers = new HashSet<Renderer>();

		// Token: 0x040035E4 RID: 13796
		private readonly Dictionary<string, Dictionary<string, float>> _shaderSlidersByPartId = new Dictionary<string, Dictionary<string, float>>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040035E5 RID: 13797
		private MCStyleCustomizationService.StyleRuntimeState _activeState;

		// Token: 0x040035E6 RID: 13798
		private readonly NeckSignDesignStore _neckSignDesigns = new NeckSignDesignStore();

		// Token: 0x040035E7 RID: 13799
		private bool _enforceCustomizationWardrobe;

		// Token: 0x040035E8 RID: 13800
		private bool _previewClothingHidden;

		// Token: 0x040035E9 RID: 13801
		private readonly Dictionary<GameObject, bool> _previewClothingVisibility = new Dictionary<GameObject, bool>();

		// Token: 0x040035EA RID: 13802
		private const string ShrinkOverrideSource = "MCClothingShrink";

		// Token: 0x040035EB RID: 13803
		private const string GenitalResetOverrideSource = "MCClothingGenitalReset";

		// Token: 0x040035EC RID: 13804
		private static readonly string[] CustomGenitalBlendShapes = new string[] { "Clit", "Clit_2", "Labia_off", "Labia_small", "Labia_shape_A", "Labia_shape_B" };

		// Token: 0x040035ED RID: 13805
		private readonly Dictionary<string, List<MCStylePresetSO.ClothingShrinkEntry>> _shrinkByPartId = new Dictionary<string, List<MCStylePresetSO.ClothingShrinkEntry>>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040035EE RID: 13806
		private readonly Dictionary<string, List<MCStylePresetSO.ClothingShrinkEntry>> _previewShrinkByPartId = new Dictionary<string, List<MCStylePresetSO.ClothingShrinkEntry>>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040035EF RID: 13807
		private readonly HashSet<string> _appliedShrinkBlendShapes = new HashSet<string>(StringComparer.Ordinal);

		// Token: 0x040035F0 RID: 13808
		private readonly HashSet<string> _appliedPreviewOnlyShrinkBlendShapes = new HashSet<string>(StringComparer.Ordinal);

		// Token: 0x040035F1 RID: 13809
		private readonly HashSet<string> _genitalCoveringPartIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040035F2 RID: 13810
		private BlendShapeManager _blendShapeManager;

		// Token: 0x040035F9 RID: 13817
		private static readonly HashSet<string> RuntimeAttachParts = new HashSet<string>
		{
			"Outfit.Swimsuit", "Outfit.Misc", "Outfit.Drape", "Outfit.Top", "Outfit.Underwear", "Outfit.Socks", "Outfit.Bottom", "Outfit.Shoes", "Outfit.Hoodie", "Outfit.Mask",
			"Outfit.Blindfold", "Outfit.PubicHair", "Outfit.HalloweenCorset", "Accessory.HalloweenCollar", "Outfit.HalloweenAssPasties", "Accessory.CowHeadband", "Accessory.CowTail", "Accessory.StadiumParachute"
		};

		// Token: 0x040035FA RID: 13818
		private bool _isNaked;

		// Token: 0x040035FB RID: 13819
		private const string HairMorphPartId = "Head.Hair";

		// Token: 0x040035FC RID: 13820
		private static readonly string[] HairFitShapes = new string[] { "RainCoat_HideHair", "RainCoat_HoodDown_HairFit", "Ghost_HideHair", "BAG_ON", "PantiesHead_HairFit", "PantiesHeadFace_HairFit" };

		// Token: 0x040035FD RID: 13821
		private readonly Dictionary<string, Dictionary<string, float>> _hairMorphValues = new Dictionary<string, Dictionary<string, float>>(StringComparer.Ordinal);

		// Token: 0x040035FE RID: 13822
		private BlendShapeManager _hairMorphManager;

		// Token: 0x020007E5 RID: 2021
		[Serializable]
		private class StyleRuntimeState
		{
			// Token: 0x040035FF RID: 13823
			public MCStylePartDefinitionSO definition;

			// Token: 0x04003600 RID: 13824
			public int styleIndex = -1;

			// Token: 0x04003601 RID: 13825
			public int surfaceIndex = -1;

			// Token: 0x04003602 RID: 13826
			public int colorPresetIndex = -1;

			// Token: 0x04003603 RID: 13827
			public bool isUsingSceneDefault = true;

			// Token: 0x04003604 RID: 13828
			public bool isUsingNoSelection;

			// Token: 0x04003605 RID: 13829
			public bool hasCustomPrimary;

			// Token: 0x04003606 RID: 13830
			public bool hasCustomSecondary;

			// Token: 0x04003607 RID: 13831
			public Color customPrimary = Color.white;

			// Token: 0x04003608 RID: 13832
			public Color customSecondary = Color.white;

			// Token: 0x04003609 RID: 13833
			public GameObject spawnedGO;

			// Token: 0x0400360A RID: 13834
			public MCStylePresetSO appliedPhysicalPreset;
		}

		// Token: 0x020007E6 RID: 2022
		[Serializable]
		public class VirtualPartRendererBinding
		{
			// Token: 0x0400360B RID: 13835
			[Tooltip("The PartDefinition this binding targets.")]
			public MCStylePartDefinitionSO definition;

			// Token: 0x0400360C RID: 13836
			[Tooltip("Scene renderers for this part. Tag each reference as Head or Body for type-safe matching.")]
			public List<MCStyleCustomizationService.VirtualPartRendererBinding.RendererReference> targetRenderers = new List<MCStyleCustomizationService.VirtualPartRendererBinding.RendererReference>();

			// Token: 0x020007E7 RID: 2023
			[Serializable]
			public class RendererReference
			{
				// Token: 0x0400360D RID: 13837
				[Tooltip("Type-safe renderer target for this reference.")]
				public MCSurfaceVariantSO.RendererTargetType rendererTarget;

				// Token: 0x0400360E RID: 13838
				public Renderer renderer;
			}
		}

		// Token: 0x020007E8 RID: 2024
		public sealed class WardrobeSnapshot
		{
			// Token: 0x0400360F RID: 13839
			internal MCStyleCustomizationService owner;

			// Token: 0x04003610 RID: 13840
			internal string activePartId;

			// Token: 0x04003611 RID: 13841
			internal bool isNaked;

			// Token: 0x04003612 RID: 13842
			internal ClothingToggleController toggles;

			// Token: 0x04003613 RID: 13843
			internal readonly Dictionary<string, MCStyleCustomizationService.WardrobeSlotSnapshot> slots = new Dictionary<string, MCStyleCustomizationService.WardrobeSlotSnapshot>(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x020007E9 RID: 2025
		internal sealed class WardrobeSlotSnapshot
		{
			// Token: 0x04003614 RID: 13844
			internal int styleIndex;

			// Token: 0x04003615 RID: 13845
			internal int surfaceIndex;

			// Token: 0x04003616 RID: 13846
			internal int colorPresetIndex;

			// Token: 0x04003617 RID: 13847
			internal bool sceneDefault;

			// Token: 0x04003618 RID: 13848
			internal bool noSelection;

			// Token: 0x04003619 RID: 13849
			internal bool hasPrimary;

			// Token: 0x0400361A RID: 13850
			internal bool hasSecondary;

			// Token: 0x0400361B RID: 13851
			internal Color primary;

			// Token: 0x0400361C RID: 13852
			internal Color secondary;

			// Token: 0x0400361D RID: 13853
			internal Dictionary<string, float> shaderSliders;

			// Token: 0x0400361E RID: 13854
			internal bool hadRoot;

			// Token: 0x0400361F RID: 13855
			internal bool rootActive;

			// Token: 0x04003620 RID: 13856
			internal bool managedToggle;

			// Token: 0x04003621 RID: 13857
			internal bool toggleVisible;
		}

		// Token: 0x020007EA RID: 2026
		public sealed class SlotAppearanceSnapshot
		{
			// Token: 0x04003622 RID: 13858
			internal MCStyleCustomizationService owner;

			// Token: 0x04003623 RID: 13859
			internal string partId;

			// Token: 0x04003624 RID: 13860
			internal MCStyleCustomizationService.WardrobeSlotSnapshot state;

			// Token: 0x04003625 RID: 13861
			internal ClothingToggleController toggles;
		}
	}
}
