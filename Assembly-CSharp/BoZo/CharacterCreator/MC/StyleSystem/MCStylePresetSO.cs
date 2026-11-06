using System;
using System.Collections.Generic;
using UnityEngine;

namespace BoZo.CharacterCreator.MC.StyleSystem
{
	// Token: 0x020007F6 RID: 2038
	[CreateAssetMenu(fileName = "MCStylePreset", menuName = "Character Creator/MC/Style Preset")]
	public class MCStylePresetSO : ScriptableObject
	{
		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x06003650 RID: 13904 RVA: 0x00138BAC File Offset: 0x00136DAC
		public int SurfaceCount
		{
			get
			{
				if (this.surfaceVariants == null)
				{
					return 0;
				}
				return this.surfaceVariants.Count;
			}
		}

		// Token: 0x06003651 RID: 13905 RVA: 0x00138BC3 File Offset: 0x00136DC3
		public bool TryGetSurfaceVariant(int index, out MCSurfaceVariantSO variant)
		{
			variant = null;
			if (this.surfaceVariants == null || index < 0 || index >= this.surfaceVariants.Count)
			{
				return false;
			}
			variant = this.surfaceVariants[index];
			return variant != null;
		}

		// Token: 0x06003652 RID: 13906 RVA: 0x00138BFA File Offset: 0x00136DFA
		public int ResolveDefaultSurfaceIndex()
		{
			if (this.surfaceVariants == null || this.surfaceVariants.Count == 0)
			{
				return -1;
			}
			return Mathf.Clamp(this.defaultSurfaceIndex, 0, this.surfaceVariants.Count - 1);
		}

		// Token: 0x04003658 RID: 13912
		[Header("Hair shape customization")]
		[Tooltip("Opt-in artistic controls. Exact renderer/shape bindings; garment fitting shapes are reserved.")]
		public List<MCStylePresetSO.HairMorphControl> hairMorphControls = new List<MCStylePresetSO.HairMorphControl>();

		// Token: 0x04003659 RID: 13913
		[Header("Identity")]
		public string presetId = "style";

		// Token: 0x0400365A RID: 13914
		public string displayName = "Style";

		// Token: 0x0400365B RID: 13915
		[Tooltip("Temporarily unavailable in customization and saved appearance application, including admin rank.")]
		public bool isVaulted;

		// Token: 0x0400365C RID: 13916
		[Tooltip("Obtainable during a run only; excluded from customization and saved appearance presets.")]
		public bool gameplayOnly;

		// Token: 0x0400365D RID: 13917
		[Tooltip("Show this garment only in the Special Events clothing category, keeping its existing slot and unlock rules.")]
		public bool isSpecialEventClothing;

		// Token: 0x0400365E RID: 13918
		public Sprite previewIcon;

		// Token: 0x0400365F RID: 13919
		[Header("Accessory card variants (optional)")]
		[Tooltip("Presets with the same non-empty group share one accessory card and its variant arrows. Saved preset identities remain separate.")]
		public string accessoryVariantGroupId;

		// Token: 0x04003660 RID: 13920
		public string accessoryVariantGroupName;

		// Token: 0x04003661 RID: 13921
		public string accessoryVariantLabel;

		// Token: 0x04003662 RID: 13922
		public int accessoryVariantOrder;

		// Token: 0x04003663 RID: 13923
		[Header("Clothing shrink (blend shape imposti al CORPO quando equipaggiato)")]
		[Tooltip("Blend shape di shrink applicati a corpo/viso mentre questo outfit è indossato, per evitare compenetrazioni. Rimossi automaticamente allo unequip. Se più outfit chiedono lo stesso blend shape, vince il valore più alto.")]
		public List<MCStylePresetSO.ClothingShrinkEntry> clothingShrinkBlendShapes = new List<MCStylePresetSO.ClothingShrinkEntry>();

		// Token: 0x04003664 RID: 13924
		[Tooltip("Fitting aggiuntivo solo nell'anteprima di customizzazione. Utile quando durante il gioco un controller del capo guida lo stesso morph attraverso le sue animazioni. Si combina per valore massimo con il fitting normale e viene rilasciato uscendo dall'anteprima.")]
		public List<MCStylePresetSO.ClothingShrinkEntry> previewOnlyClothingShrinkBlendShapes = new List<MCStylePresetSO.ClothingShrinkEntry>();

		// Token: 0x04003665 RID: 13925
		[Header("Pose-driven shrink (cover, emote e azioni)")]
		[Tooltip("Margine dinamico per zona del corpo. Non identifica una singola animazione: reagisce alla posa reale campionata dopo l'Animator e quindi copre anche blend, transizioni e nuove emote. Il margine viene rimosso in modo interpolato a fine azione.")]
		public List<MCStylePresetSO.ActionShrinkEntry> actionShrinkBlendShapes = new List<MCStylePresetSO.ActionShrinkEntry>();

		// Token: 0x04003666 RID: 13926
		[Header("Genital coverage")]
		[Tooltip("Il capo copre visivamente i genitali: mentre e' realmente visibile forza ai valori di default soltanto i morph genitali esposti nella character customization. Non tocca Kupa_V/Kupa_A e quindi non interferisce con interazioni o insertable toys.")]
		public bool resetCustomGenitalBlendShapesWhenWorn;

		// Token: 0x04003667 RID: 13927
		[Tooltip("Compatibilita' con la Futanari Mode. Automatic usa la regola dello slot; Hide nasconde penis/testicoli e sospende la loro fisica; KeepVisibleAndCollide li mantiene visibili e li aggiunge ai collider MagicaCloth del capo.")]
		public MCFutanariClothingMode futanariClothingMode;

		// Token: 0x04003668 RID: 13928
		[Tooltip("Blend shape di apertura che rilasciano la copertura di questo capo durante il flash. Letti soltanto: mesh, scala e posa del modello restano quelle originali.")]
		public string[] futanariOpeningShapes = Array.Empty<string>();

		// Token: 0x04003669 RID: 13929
		[Tooltip("Per capi animati senza MagicaCloth: la superficie limita la posa del rig Futanari, mantenendo dimensioni, mesh e animazioni originali del capo.")]
		public bool bendFutanariOnContact;

		// Token: 0x0400366A RID: 13930
		[Tooltip("Correzione aggiuntiva di Shrink_Hip applicata soltanto quando entrambe le anche sono fortemente flesse (supina, gambe raccolte, squat profondo).")]
		[Range(0f, 10f)]
		public float deepPelvisShrinkAdditional;

		// Token: 0x0400366B RID: 13931
		[Header("Body physics while worn (MagicaCloth)")]
		[Tooltip("Riduce in modo reversibile il contributo della simulazione MagicaCloth mentre questo capo e' realmente visibile sul corpo. I valori vengono interpolati allo equip/unequip; 1 mantiene la fisica naked, 0 usa soltanto la posa animata.")]
		public bool overrideBodyPhysics;

		// Token: 0x0400366C RID: 13932
		[Range(0f, 1f)]
		public float breastPhysicsWeight = 1f;

		// Token: 0x0400366D RID: 13933
		[Range(0f, 1f)]
		public float buttPhysicsWeight = 1f;

		// Token: 0x0400366E RID: 13934
		[Header("Physical (optional)")]
		public GameObject physicalPrefab;

		// Token: 0x0400366F RID: 13935
		public bool replaceExistingPhysical = true;

		// Token: 0x04003670 RID: 13936
		[Header("Surface")]
		public List<MCSurfaceVariantSO> surfaceVariants = new List<MCSurfaceVariantSO>();

		// Token: 0x04003671 RID: 13937
		[Min(0f)]
		public int defaultSurfaceIndex;

		// Token: 0x04003672 RID: 13938
		[Header("Layering override per-item")]
		[Tooltip("Accessory that can coexist with garments in every OTHER wardrobe slot. Prevents their slot exclusions from removing this preset and vice versa. The one-preset-per-slot limit still applies; physical coverage is configured on the wearable item.")]
		public bool ignoreOtherSlotConflicts;

		// Token: 0x04003673 RID: 13939
		[Tooltip("Se true, incompatibleSlotsOverride SOSTITUISCE la lista di esclusione dello slot per questo item specifico.")]
		public bool overrideIncompatibleSlots;

		// Token: 0x04003674 RID: 13940
		[Tooltip("Esclusione hard specifica di questo item (usata solo se overrideIncompatibleSlots=true).")]
		public List<string> incompatibleSlotsOverride = new List<string>();

		// Token: 0x04003675 RID: 13941
		[Tooltip("Regole di visibilità specifiche di questo item: per la stessa coppia occluder→occluded VINCONO sulle regole di categoria dello slot.")]
		public List<MCLayerRuleSO> itemLayerRuleOverrides = new List<MCLayerRuleSO>();

		// Token: 0x020007F7 RID: 2039
		[Serializable]
		public class HairMorphBinding
		{
			// Token: 0x04003676 RID: 13942
			public string rendererName;

			// Token: 0x04003677 RID: 13943
			public string blendShapeName;

			// Token: 0x04003678 RID: 13944
			public float minValue;

			// Token: 0x04003679 RID: 13945
			public float defaultValue;

			// Token: 0x0400367A RID: 13946
			public float maxValue = 100f;
		}

		// Token: 0x020007F8 RID: 2040
		[Serializable]
		public class HairMorphControl
		{
			// Token: 0x0400367B RID: 13947
			public string id;

			// Token: 0x0400367C RID: 13948
			public string displayName;

			// Token: 0x0400367D RID: 13949
			public float min;

			// Token: 0x0400367E RID: 13950
			public float max = 100f;

			// Token: 0x0400367F RID: 13951
			public float step = 1f;

			// Token: 0x04003680 RID: 13952
			public float defaultValue;

			// Token: 0x04003681 RID: 13953
			public List<MCStylePresetSO.HairMorphBinding> bindings = new List<MCStylePresetSO.HairMorphBinding>();
		}

		// Token: 0x020007F9 RID: 2041
		[Serializable]
		public class ClothingShrinkEntry
		{
			// Token: 0x04003682 RID: 13954
			[Tooltip("Nome ESATTO del blend shape sul corpo/viso (es. 'Body.WaistShrink', 'Breast.Shrink').")]
			public string blendShapeName;

			// Token: 0x04003683 RID: 13955
			[Tooltip("Valore di shrink da imporre quando questo outfit è equipaggiato (0-100).")]
			[Range(0f, 100f)]
			public float value = 100f;
		}

		// Token: 0x020007FA RID: 2042
		[Serializable]
		public class ActionShrinkEntry
		{
			// Token: 0x04003684 RID: 13956
			[Tooltip("Blend shape regionale da rinforzare soltanto mentre la posa comprime quella zona.")]
			public string blendShapeName;

			// Token: 0x04003685 RID: 13957
			[Tooltip("Incremento massimo sopra il fitting statico del capo. Il valore effettivo viene calcolato continuamente dagli angoli delle ossa, quindi torna a zero in posa neutra.")]
			[Range(0f, 25f)]
			public float maximumAdditionalValue = 1f;
		}
	}
}
