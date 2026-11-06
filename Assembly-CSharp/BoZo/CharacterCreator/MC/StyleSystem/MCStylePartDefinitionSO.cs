using System;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace BoZo.CharacterCreator.MC.StyleSystem
{
	// Token: 0x020007F4 RID: 2036
	[CreateAssetMenu(fileName = "MCStylePartDefinition", menuName = "Character Creator/MC/Style Part Definition")]
	public class MCStylePartDefinitionSO : ScriptableObject
	{
		// Token: 0x170009C2 RID: 2498
		// (get) Token: 0x0600364B RID: 13899 RVA: 0x00138ABF File Offset: 0x00136CBF
		public bool IsBoneAttachedAccessory
		{
			get
			{
				return !string.IsNullOrWhiteSpace(this.attachBoneName);
			}
		}

		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x0600364C RID: 13900 RVA: 0x00138ACF File Offset: 0x00136CCF
		public int StyleCount
		{
			get
			{
				if (this.stylePresets == null)
				{
					return 0;
				}
				return this.stylePresets.Count;
			}
		}

		// Token: 0x0600364D RID: 13901 RVA: 0x00138AE6 File Offset: 0x00136CE6
		public bool TryGetStylePreset(int index, out MCStylePresetSO preset)
		{
			preset = null;
			if (this.stylePresets == null || index < 0 || index >= this.stylePresets.Count)
			{
				return false;
			}
			preset = this.stylePresets[index];
			return preset != null;
		}

		// Token: 0x0600364E RID: 13902 RVA: 0x00138B1D File Offset: 0x00136D1D
		public int ResolveDefaultStyleIndex()
		{
			if (this.stylePresets == null || this.stylePresets.Count == 0)
			{
				return -1;
			}
			return Mathf.Clamp(this.defaultStyleIndex, 0, this.stylePresets.Count - 1);
		}

		// Token: 0x0400363E RID: 13886
		[Header("Identity")]
		public string partId = "Head.Hair";

		// Token: 0x0400363F RID: 13887
		public string displayName = "Hair";

		// Token: 0x04003640 RID: 13888
		public MCColorPart colorPart = MCColorPart.Hair;

		// Token: 0x04003641 RID: 13889
		public MCStylePartKind partKind;

		// Token: 0x04003642 RID: 13890
		[Header("Equipment / Categoria")]
		[Tooltip("Se true questo slot è una PARTE DEL CORPO/ASPETTO (capelli, peli, pelle, occhi...) e resta visibile nello stato 'nudo'. Se false è un INDUMENTO/ACCESSORIO e viene nascosto da Naked. Default false.")]
		public bool isBodyAppearance;

		// Token: 0x04003643 RID: 13891
		[Tooltip("Categoria UI che raggruppa più slot sotto un bottone (es. 'Outfit', 'Lingerie', 'Miscellanea'). Vuoto = nessuna categoria esplicita.")]
		public string categoryKey;

		// Token: 0x04003644 RID: 13892
		[Tooltip("Ordine dello slot dentro la categoria UI (crescente).")]
		public int slotSortOrder;

		// Token: 0x04003645 RID: 13893
		[Tooltip("Ordine di layer per la risoluzione visibilità (più alto = più esterno). Indicativo per il resolver.")]
		public int layerOrder;

		// Token: 0x04003646 RID: 13894
		[Tooltip("Se true, questo slot resta visibile anche in stato 'naked' (es. lingerie).")]
		public bool keepVisibleWhenNaked;

		// Token: 0x04003647 RID: 13895
		[Header("Layering / Esclusione (livello categoria/slot)")]
		[Tooltip("partId di slot che vengono RIMOSSI quando un item di questo slot viene equipaggiato (esclusione hard). Override per-item su MCStylePresetSO.")]
		public List<string> incompatibleSlots = new List<string>();

		// Token: 0x04003648 RID: 13896
		[Tooltip("Regole di visibilità condizionale valide per tutti gli item di questo slot. Override per-item su MCStylePresetSO.itemLayerRuleOverrides.")]
		public List<MCLayerRuleSO> categoryLayerRules = new List<MCLayerRuleSO>();

		// Token: 0x04003649 RID: 13897
		[Header("Default Handling")]
		public MCStyleDefaultBehavior defaultBehavior;

		// Token: 0x0400364A RID: 13898
		[Min(0f)]
		public int defaultStyleIndex;

		// Token: 0x0400364B RID: 13899
		[Header("Optional None Selection")]
		[Tooltip("If enabled, the style grid shows a first card that removes this part entirely (independent from Scene Default).")]
		public bool allowNoSelection;

		// Token: 0x0400364C RID: 13900
		[Tooltip("Label shown for the optional no-selection card.")]
		public string noSelectionDisplayName = "None";

		// Token: 0x0400364D RID: 13901
		[Tooltip("Optional icon for the no-selection card.")]
		public Sprite noSelectionIcon;

		// Token: 0x0400364E RID: 13902
		[Tooltip("If true and allowNoSelection is enabled, this part starts with no item selected.")]
		public bool defaultToNoSelection;

		// Token: 0x0400364F RID: 13903
		[Header("Accessorio mono-bone (rigido)")]
		[Tooltip("Se valorizzato (es. 'head.x'), questo slot è un ACCESSORIO RIGIDO (occhiali, forcella, gioiello): il prefab viene parentato DIRETTAMENTE a questo bone preservando il suo local TRS, SENZA il rebind/skinning di PlayerDresser. Vuoto = outfit skinnato normale.")]
		public string attachBoneName;

		// Token: 0x04003650 RID: 13904
		[Header("OutfitSystem Integration (Physical parts only)")]
		[Tooltip("OutfitType used by OutfitSystem for this part slot (e.g. Hair, Top, Bottom). Leave null for Virtual parts.")]
		public OutfitType outfitType;

		// Token: 0x04003651 RID: 13905
		[Tooltip("ClothingToggleController group name to update after a physical swap (e.g. 'Hair', 'Top'). Leave empty if not applicable.")]
		public string clothingGroupName;

		// Token: 0x04003652 RID: 13906
		[Tooltip("Extra physics bone patterns for PlayerDresser when attaching this part (e.g. 'hair', 'ribbon'). Merged with PlayerDresser defaults.")]
		public string[] extraPhysicsBonePatterns;

		// Token: 0x04003653 RID: 13907
		[Header("Presets")]
		public List<MCStylePresetSO> stylePresets = new List<MCStylePresetSO>();
	}
}
