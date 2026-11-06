using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using BoZo.CharacterCreator;
using BoZo.CharacterCreator.MC;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Rank;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core.Backends
{
	// Token: 0x02000B20 RID: 2848
	public class CielMainCharacterCustomizationBackend : CharacterCustomizationBackendBase, IColorCustomizationBackend, IClothingCustomizationBackend, ISkinDecalCustomizationBackend
	{
		// Token: 0x060050AF RID: 20655 RVA: 0x001F3584 File Offset: 0x001F1784
		public List<SkinDecal> GetDecalCatalog()
		{
			return this.decalCatalog;
		}

		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x060050B0 RID: 20656 RVA: 0x001F358C File Offset: 0x001F178C
		public override string BackendId
		{
			get
			{
				return "ciel-main-character";
			}
		}

		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x060050B1 RID: 20657 RVA: 0x001F3593 File Offset: 0x001F1793
		public override CustomizationFeature SupportedFeatures
		{
			get
			{
				return CustomizationFeature.BlendShapes | CustomizationFeature.Colors | CustomizationFeature.ClothingToggles | CustomizationFeature.SkinDecals | CustomizationFeature.Presets;
			}
		}

		// Token: 0x060050B2 RID: 20658 RVA: 0x001F3597 File Offset: 0x001F1797
		public override bool CanInitialize(CustomizationSessionContext context)
		{
			if (context == null)
			{
				return false;
			}
			if (context.targetType != CustomizationTargetType.MainCharacter)
			{
				return false;
			}
			if (this.blendShapeController == null)
			{
				Debug.LogWarning("[" + this.BackendId + "] CanInitialize failed: blendShapeController is not assigned. Please assign the CharacterBlendShapeController reference on the CielCustomization GameObject in the inspector.");
				return false;
			}
			return true;
		}

		// Token: 0x060050B3 RID: 20659 RVA: 0x001F35D4 File Offset: 0x001F17D4
		private BlendShapeMapSO.BlendShapeEntry FindBlendShapeEntry(BlendShapeValue blendShape)
		{
			if (this.blendShapeController == null || this.blendShapeController.BlendShapeMap == null || blendShape == null)
			{
				return null;
			}
			List<BlendShapeMapSO.BlendShapeEntry> blendShapeMappings = this.blendShapeController.BlendShapeMap.blendShapeMappings;
			if (blendShapeMappings == null)
			{
				return null;
			}
			for (int i = 0; i < blendShapeMappings.Count; i++)
			{
				BlendShapeMapSO.BlendShapeEntry blendShapeEntry = blendShapeMappings[i];
				if (blendShapeEntry != null && string.Equals(blendShapeEntry.displayName, blendShape.blendShapeName, StringComparison.OrdinalIgnoreCase) && (string.IsNullOrWhiteSpace(blendShape.category) || string.Equals(blendShapeEntry.category, blendShape.category, StringComparison.OrdinalIgnoreCase)) && (string.IsNullOrWhiteSpace(blendShape.subcategory) || string.Equals(blendShapeEntry.subcategory, blendShape.subcategory, StringComparison.OrdinalIgnoreCase)))
				{
					return blendShapeEntry;
				}
			}
			return null;
		}

		// Token: 0x060050B4 RID: 20660 RVA: 0x001F3698 File Offset: 0x001F1898
		public override bool Initialize(CustomizationSessionContext context)
		{
			if (!base.Initialize(context))
			{
				return false;
			}
			this._runtimeBlendShapes.Clear();
			if (this.styleCustomizationService != null)
			{
				this.styleCustomizationService.SetCustomizationPreview(true);
			}
			return true;
		}

		// Token: 0x060050B5 RID: 20661 RVA: 0x001F36CB File Offset: 0x001F18CB
		public override void Shutdown()
		{
			if (this.styleCustomizationService != null)
			{
				this.styleCustomizationService.SetCustomizationPreview(false);
			}
			this._runtimeBlendShapes.Clear();
			base.Shutdown();
		}

		// Token: 0x060050B6 RID: 20662 RVA: 0x001F36F8 File Offset: 0x001F18F8
		public override bool TrySetBlendShape(string blendShapeName, float value)
		{
			if (!base.IsInitialized || this.blendShapeController == null || string.IsNullOrWhiteSpace(blendShapeName))
			{
				return false;
			}
			this.blendShapeController.SetBlendShapeValue(blendShapeName, value);
			this._runtimeBlendShapes[blendShapeName] = value;
			return true;
		}

		// Token: 0x060050B7 RID: 20663 RVA: 0x001F3738 File Offset: 0x001F1938
		public override bool TryGetBlendShape(string blendShapeName, out float value)
		{
			value = 0f;
			if (!base.IsInitialized || this.blendShapeController == null || string.IsNullOrWhiteSpace(blendShapeName))
			{
				return false;
			}
			if (this._runtimeBlendShapes.TryGetValue(blendShapeName, out value))
			{
				return true;
			}
			value = this.blendShapeController.GetBlendShapeValue(blendShapeName);
			this._runtimeBlendShapes[blendShapeName] = value;
			return true;
		}

		// Token: 0x060050B8 RID: 20664 RVA: 0x001F379A File Offset: 0x001F199A
		public bool TrySetClothingGroupState(string groupId, bool enabled)
		{
			return this.TrySetClothingGroupState(groupId, enabled, true);
		}

		// Token: 0x060050B9 RID: 20665 RVA: 0x001F37A8 File Offset: 0x001F19A8
		private bool TrySetClothingGroupState(string groupId, bool enabled, bool showRestriction)
		{
			if (!base.IsInitialized || this.clothingToggleController == null || string.IsNullOrWhiteSpace(groupId))
			{
				return false;
			}
			if (!enabled && this.styleCustomizationService != null && this.styleCustomizationService.EnforcesRankWardrobe)
			{
				foreach (MCStylePartDefinitionSO mcstylePartDefinitionSO in this.styleCustomizationService.PartDefinitions)
				{
					if (mcstylePartDefinitionSO != null && RankWardrobePolicy.IsWardrobePart(mcstylePartDefinitionSO.partId) && (string.Equals(mcstylePartDefinitionSO.clothingGroupName, groupId, StringComparison.OrdinalIgnoreCase) || string.Equals(mcstylePartDefinitionSO.partId, groupId, StringComparison.OrdinalIgnoreCase)))
					{
						if (showRestriction)
						{
							this.styleCustomizationService.ShowFirstRunOutfitRestriction();
						}
						return false;
					}
				}
			}
			this.clothingToggleController.SetGroupState(groupId, enabled);
			return true;
		}

		// Token: 0x060050BA RID: 20666 RVA: 0x001F3890 File Offset: 0x001F1A90
		public bool TrySetColor(string category, string propertyName, Color color)
		{
			MCColorPart mccolorPart;
			return base.IsInitialized && !(this.colorCustomizationService == null) && CielMainCharacterCustomizationBackend.TryParseColorPart(category, out mccolorPart) && this.colorCustomizationService.TrySetCustomColor(mccolorPart, color);
		}

		// Token: 0x060050BB RID: 20667 RVA: 0x001F38D0 File Offset: 0x001F1AD0
		public bool TryGetColor(string category, string propertyName, out Color color)
		{
			color = Color.white;
			MCColorPart mccolorPart;
			return base.IsInitialized && !(this.colorCustomizationService == null) && CielMainCharacterCustomizationBackend.TryParseColorPart(category, out mccolorPart) && this.colorCustomizationService.TryGetCurrentColor(mccolorPart, out color);
		}

		// Token: 0x060050BC RID: 20668 RVA: 0x001F3919 File Offset: 0x001F1B19
		public bool TryGetClothingGroupState(string groupId, out bool enabled)
		{
			enabled = false;
			if (!base.IsInitialized || this.clothingToggleController == null || string.IsNullOrWhiteSpace(groupId))
			{
				return false;
			}
			enabled = this.clothingToggleController.IsGroupActive(groupId);
			return true;
		}

		// Token: 0x060050BD RID: 20669 RVA: 0x001F3950 File Offset: 0x001F1B50
		public bool TrySetDecalState(string decalId, bool enabled, float opacity, Color tint)
		{
			if (!base.IsInitialized || this.skinDecalController == null || string.IsNullOrWhiteSpace(decalId))
			{
				return false;
			}
			SkinDecal skinDecal = this.FindDecalById(decalId);
			if (skinDecal == null || skinDecal.gameplayOnly)
			{
				return false;
			}
			if (!this.skinDecalController.HasDecal(skinDecal))
			{
				this.skinDecalController.AddDecal(skinDecal, new Color?(tint), new float?(Mathf.Clamp01(opacity)));
			}
			this.skinDecalController.SetDecalTint(skinDecal, tint);
			this.skinDecalController.SetDecalOpacity(skinDecal, Mathf.Clamp01(opacity));
			this.skinDecalController.SetDecalActive(skinDecal, enabled);
			return true;
		}

		// Token: 0x060050BE RID: 20670 RVA: 0x001F39F4 File Offset: 0x001F1BF4
		public override CharacterStateData CaptureState()
		{
			CharacterStateData characterStateData = new CharacterStateData();
			if (this.blendShapeController != null && this.blendShapeController.BlendShapeMap != null)
			{
				List<BlendShapeMapSO.BlendShapeEntry> blendShapeMappings = this.blendShapeController.BlendShapeMap.blendShapeMappings;
				if (blendShapeMappings != null)
				{
					for (int i = 0; i < blendShapeMappings.Count; i++)
					{
						BlendShapeMapSO.BlendShapeEntry blendShapeEntry = blendShapeMappings[i];
						if (blendShapeEntry != null && !string.IsNullOrWhiteSpace(blendShapeEntry.displayName))
						{
							float entryAuthoredUiValue = this.blendShapeController.GetEntryAuthoredUiValue(blendShapeEntry);
							characterStateData.blendShapes.Add(new BlendShapeValue(blendShapeEntry.category, blendShapeEntry.subcategory, blendShapeEntry.displayName, entryAuthoredUiValue));
						}
					}
				}
			}
			else
			{
				foreach (KeyValuePair<string, float> keyValuePair in this._runtimeBlendShapes)
				{
					characterStateData.blendShapes.Add(new BlendShapeValue("MainCharacter", "Runtime", keyValuePair.Key, keyValuePair.Value));
				}
			}
			this.CaptureClothingState(characterStateData);
			this.CaptureSkinDecalState(characterStateData);
			this.CaptureColorState(characterStateData);
			return characterStateData;
		}

		// Token: 0x060050BF RID: 20671 RVA: 0x001F3B20 File Offset: 0x001F1D20
		public override bool ApplyState(CharacterStateData stateData)
		{
			if (!base.IsInitialized || stateData == null)
			{
				return false;
			}
			for (int i = 0; i < stateData.blendShapes.Count; i++)
			{
				BlendShapeValue blendShapeValue = stateData.blendShapes[i];
				if (blendShapeValue != null && !string.IsNullOrWhiteSpace(blendShapeValue.blendShapeName))
				{
					BlendShapeMapSO.BlendShapeEntry blendShapeEntry = this.FindBlendShapeEntry(blendShapeValue);
					if (blendShapeEntry != null)
					{
						this.blendShapeController.ApplyEntryValue(blendShapeEntry, blendShapeValue.value);
					}
					else
					{
						this.TrySetBlendShape(blendShapeValue.blendShapeName, blendShapeValue.value);
					}
				}
			}
			this.ApplyClothingState(stateData);
			this.ApplySkinDecalState(stateData);
			this.ApplyColorState(stateData);
			if (this.styleCustomizationService != null)
			{
				this.styleCustomizationService.EnforceCustomizationWardrobe();
			}
			return true;
		}

		// Token: 0x060050C0 RID: 20672 RVA: 0x001F3BCE File Offset: 0x001F1DCE
		private void CaptureColorState(CharacterStateData stateData)
		{
			if (stateData == null)
			{
				return;
			}
			if (this.colorCustomizationService != null)
			{
				this.colorCustomizationService.CaptureState(stateData);
			}
			if (this.styleCustomizationService != null)
			{
				this.styleCustomizationService.CaptureState(stateData);
			}
		}

		// Token: 0x060050C1 RID: 20673 RVA: 0x001F3C08 File Offset: 0x001F1E08
		private void ApplyColorState(CharacterStateData stateData)
		{
			if (stateData == null)
			{
				return;
			}
			if (this.colorCustomizationService != null)
			{
				this.colorCustomizationService.ApplyState(stateData);
			}
			if (this.styleCustomizationService != null)
			{
				this.styleCustomizationService.ApplyState(stateData, true);
			}
		}

		// Token: 0x060050C2 RID: 20674 RVA: 0x001F3C43 File Offset: 0x001F1E43
		private static bool TryParseColorPart(string category, out MCColorPart part)
		{
			if (string.IsNullOrWhiteSpace(category))
			{
				part = MCColorPart.Skin;
				return false;
			}
			return Enum.TryParse<MCColorPart>(category, true, out part);
		}

		// Token: 0x060050C3 RID: 20675 RVA: 0x001F3C5C File Offset: 0x001F1E5C
		private void CaptureClothingState(CharacterStateData stateData)
		{
			if (stateData == null || this.clothingToggleController == null || this.clothingToggleController.clothingGroups == null)
			{
				return;
			}
			for (int i = 0; i < this.clothingToggleController.clothingGroups.Count; i++)
			{
				ClothingGroup clothingGroup = this.clothingToggleController.clothingGroups[i];
				if (clothingGroup != null && !string.IsNullOrWhiteSpace(clothingGroup.groupName))
				{
					stateData.clothingToggles.Add(new ClothingToggleValue(clothingGroup.groupName, clothingGroup.isActive));
				}
			}
		}

		// Token: 0x060050C4 RID: 20676 RVA: 0x001F3CE4 File Offset: 0x001F1EE4
		private void ApplyClothingState(CharacterStateData stateData)
		{
			if (stateData == null || this.clothingToggleController == null)
			{
				return;
			}
			for (int i = 0; i < stateData.clothingToggles.Count; i++)
			{
				ClothingToggleValue clothingToggleValue = stateData.clothingToggles[i];
				if (clothingToggleValue != null && !string.IsNullOrWhiteSpace(clothingToggleValue.groupId))
				{
					this.TrySetClothingGroupState(clothingToggleValue.groupId, clothingToggleValue.isEnabled, false);
				}
			}
		}

		// Token: 0x060050C5 RID: 20677 RVA: 0x001F3D4C File Offset: 0x001F1F4C
		private void CaptureSkinDecalState(CharacterStateData stateData)
		{
			if (stateData == null || this.skinDecalController == null)
			{
				return;
			}
			SkinDecalSaveData saveData = this.skinDecalController.GetSaveData(true);
			if (saveData == null || saveData.entries == null)
			{
				return;
			}
			for (int i = 0; i < saveData.entries.Count; i++)
			{
				SkinDecalSaveData.Entry entry = saveData.entries[i];
				if (entry != null && !string.IsNullOrWhiteSpace(entry.decalName) && entry.isActive)
				{
					stateData.skinDecals.Add(new SkinDecalStateValue(entry.decalName, true, entry.opacity, entry.tint));
				}
			}
		}

		// Token: 0x060050C6 RID: 20678 RVA: 0x001F3DE4 File Offset: 0x001F1FE4
		private void ApplySkinDecalState(CharacterStateData stateData)
		{
			if (stateData == null || this.skinDecalController == null)
			{
				return;
			}
			this.skinDecalController.DeactivateAllDecals();
			if (stateData.skinDecals == null)
			{
				return;
			}
			for (int i = 0; i < stateData.skinDecals.Count; i++)
			{
				SkinDecalStateValue skinDecalStateValue = stateData.skinDecals[i];
				if (skinDecalStateValue != null && !string.IsNullOrWhiteSpace(skinDecalStateValue.decalId))
				{
					this.TrySetDecalState(skinDecalStateValue.decalId, skinDecalStateValue.isEnabled, skinDecalStateValue.opacity, skinDecalStateValue.tint);
				}
			}
		}

		// Token: 0x060050C7 RID: 20679 RVA: 0x001F3E6C File Offset: 0x001F206C
		private SkinDecal FindDecalById(string decalId)
		{
			for (int i = 0; i < this.decalCatalog.Count; i++)
			{
				SkinDecal skinDecal = this.decalCatalog[i];
				if (skinDecal != null && skinDecal.name == decalId)
				{
					return skinDecal;
				}
			}
			return null;
		}

		// Token: 0x04005146 RID: 20806
		[Header("References")]
		[SerializeField]
		private CharacterBlendShapeController blendShapeController;

		// Token: 0x04005147 RID: 20807
		[SerializeField]
		private MCColorCustomizationService colorCustomizationService;

		// Token: 0x04005148 RID: 20808
		[SerializeField]
		private MCStyleCustomizationService styleCustomizationService;

		// Token: 0x04005149 RID: 20809
		[SerializeField]
		private ClothingToggleController clothingToggleController;

		// Token: 0x0400514A RID: 20810
		[SerializeField]
		private SkinDecalController skinDecalController;

		// Token: 0x0400514B RID: 20811
		[SerializeField]
		private List<SkinDecal> decalCatalog = new List<SkinDecal>();

		// Token: 0x0400514C RID: 20812
		private readonly Dictionary<string, float> _runtimeBlendShapes = new Dictionary<string, float>();
	}
}
