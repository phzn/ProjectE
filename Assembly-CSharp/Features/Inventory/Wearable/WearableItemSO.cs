using System;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using Invector.vCharacterController;
using ProjectExhibitionist;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.Inventory.Wearable
{
	// Token: 0x02001126 RID: 4390
	[CreateAssetMenu(fileName = "WearableItem_New", menuName = "Features/Inventory/Wearable Item")]
	public class WearableItemSO : InventoryItemSO
	{
		// Token: 0x17001518 RID: 5400
		// (get) Token: 0x060084FA RID: 34042 RVA: 0x00363884 File Offset: 0x00361A84
		public bool UsesMCStyle
		{
			get
			{
				return !string.IsNullOrWhiteSpace(this.mcStylePartId) && this.mcStylePreset != null;
			}
		}

		// Token: 0x060084FB RID: 34043 RVA: 0x003638A4 File Offset: 0x00361AA4
		public bool BlocksMouthWhileWorn(Component wearer)
		{
			if (!this.physicallyBlocksMouth)
			{
				return false;
			}
			if (this.itemId != "cloth_mask" || wearer == null)
			{
				return true;
			}
			vThirdPersonInput componentInParent = wearer.GetComponentInParent<vThirdPersonInput>(true);
			MaskToggleController componentInChildren = ((componentInParent != null) ? componentInParent.transform : wearer.transform.root).GetComponentInChildren<MaskToggleController>(true);
			return componentInChildren == null || !componentInChildren.IsMaskAvailable || !componentInChildren.IsLowered;
		}

		// Token: 0x060084FC RID: 34044 RVA: 0x00363920 File Offset: 0x00361B20
		public float ResolveDetectionCoverage(MCSurfaceVariantSO surface)
		{
			WearableSurfaceGameplayOverride wearableSurfaceGameplayOverride = this.FindSurfaceGameplayOverride(surface);
			if (wearableSurfaceGameplayOverride == null || !wearableSurfaceGameplayOverride.overrideDetectionCoverage)
			{
				return this.detectionCoverage;
			}
			return wearableSurfaceGameplayOverride.detectionCoverage;
		}

		// Token: 0x060084FD RID: 34045 RVA: 0x00363950 File Offset: 0x00361B50
		public bool ResolveShowInGameplayInventory(MCSurfaceVariantSO surface)
		{
			if (this.isVaulted)
			{
				return false;
			}
			WearableSurfaceGameplayOverride wearableSurfaceGameplayOverride = this.FindSurfaceGameplayOverride(surface);
			if (wearableSurfaceGameplayOverride == null || !wearableSurfaceGameplayOverride.overrideShowInGameplayInventory)
			{
				return this.showInGameplayInventory;
			}
			return wearableSurfaceGameplayOverride.showInGameplayInventory;
		}

		// Token: 0x17001519 RID: 5401
		// (get) Token: 0x060084FE RID: 34046 RVA: 0x00363988 File Offset: 0x00361B88
		public bool HasGameplaySurfaceOverride
		{
			get
			{
				if (this.isVaulted || this.surfaceGameplayOverrides == null)
				{
					return false;
				}
				for (int i = 0; i < this.surfaceGameplayOverrides.Count; i++)
				{
					WearableSurfaceGameplayOverride wearableSurfaceGameplayOverride = this.surfaceGameplayOverrides[i];
					if (wearableSurfaceGameplayOverride != null && wearableSurfaceGameplayOverride.overrideShowInGameplayInventory && wearableSurfaceGameplayOverride.showInGameplayInventory)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060084FF RID: 34047 RVA: 0x003639E0 File Offset: 0x00361BE0
		private WearableSurfaceGameplayOverride FindSurfaceGameplayOverride(MCSurfaceVariantSO surface)
		{
			if (surface == null || string.IsNullOrWhiteSpace(surface.variantId) || this.surfaceGameplayOverrides == null)
			{
				return null;
			}
			for (int i = 0; i < this.surfaceGameplayOverrides.Count; i++)
			{
				WearableSurfaceGameplayOverride wearableSurfaceGameplayOverride = this.surfaceGameplayOverrides[i];
				if (wearableSurfaceGameplayOverride != null && string.Equals(wearableSurfaceGameplayOverride.surfaceVariantId, surface.variantId, StringComparison.OrdinalIgnoreCase))
				{
					return wearableSurfaceGameplayOverride;
				}
			}
			return null;
		}

		// Token: 0x040087D2 RID: 34770
		[Header("MCStyle Wear (canonical)")]
		[Tooltip("partId of the MCStyle slot this wearable maps to (e.g. 'Accessory.HeadBag', 'Outfit.Top'). When set together with mcStylePreset, wearing routes through the player's MCStyleCustomizationService (AddRuntimePreset + SelectStyleForSlot) - the SAME canonical path the customization uses. Leave empty to fall back to the legacy clothingGroupName path.")]
		public string mcStylePartId;

		// Token: 0x040087D3 RID: 34771
		[Tooltip("MCStyle preset (physical prefab) spawned/attached on the rig when this item is worn. Added to the slot's wardrobe at runtime if not already present. Required for the MCStyle path.")]
		public MCStylePresetSO mcStylePreset;

		// Token: 0x040087D4 RID: 34772
		[Tooltip("If true (default) Unwear only HIDES the spawned outfit (keeps it in the wardrobe, instant re-wear). If false Unwear fully unequips the slot. Only used on the MCStyle path.")]
		public bool mcStyleHideOnUnwear = true;

		// Token: 0x040087D5 RID: 34773
		[Header("Equipment selection")]
		[Tooltip("Selecting this hotbar item wears it immediately. It never creates a hand prop, including delayed resume calls.")]
		public bool wearOnSelection;

		// Token: 0x040087D6 RID: 34774
		[Tooltip("Selection owns the worn state: selecting another slot takes this equipment off. No separate wear/unwear action.")]
		public bool wearOnlyWhileSelected;

		// Token: 0x040087D7 RID: 34775
		[Tooltip("Put this equipment away when encounters, game over or water entry stow inventory items. Keeps the inventory copy.")]
		public bool stowForInteraction;

		// Token: 0x040087D8 RID: 34776
		[Header("Gameplay role")]
		[Tooltip("If true this garment has gameplay utility (cover / exhibition value) and appears in the in-game cloth inventory, where the player can take it off / put it back on. If false it's AESTHETIC ONLY - chosen/customized during character customization but NOT mirrored into the gameplay inventory (e.g. glasses, headpin). Default true so existing garments keep showing up.")]
		public bool showInGameplayInventory = true;

		// Token: 0x040087D9 RID: 34777
		[Header("Garment shape")]
		[Tooltip("This garment includes a skirt, either on its own or as part of a full outfit. Wind grates accept every visible worn skirt using this property, regardless of its item id, inventory instance, or color variant. Its skirt cloth must react to wind.")]
		public bool containsSkirt;

		// Token: 0x040087DA RID: 34778
		[Header("Stamina benefit")]
		[Tooltip("Stamina consumption multiplier while visibly worn. 0.8 saves 20%. Only the strongest worn benefit applies, so full outfits and separate shoes cannot stack.")]
		[Range(0.1f, 1f)]
		public float staminaCostMultiplier = 1f;

		// Token: 0x040087DB RID: 34779
		[Header("Modesty (clothed/naked coverage)")]
		[Tooltip("True if this garment DECENTLY covers the upper body (e.g. Top1/Top2). Obscene tops (bikini/lingerie bras, micro top) leave this false: wearing them never counts toward the 'clothed' state.")]
		public bool coversTopDecently;

		// Token: 0x040087DC RID: 34780
		[Tooltip("True if this garment DECENTLY covers the lower body (e.g. leggings). Panties / micro bottoms leave this false. A decent one-piece can set both covers* flags.")]
		public bool coversBottomDecently;

		// Token: 0x040087DD RID: 34781
		[Tooltip("If true, wearing this item forces the NAKED state no matter what else is worn (e.g. gag ball, rope).")]
		public bool forcesNaked;

		// Token: 0x040087DE RID: 34782
		[Header("Copertura FISICA (3.3-B) — NON confondere con la modesty sopra")]
		[Tooltip("True se questo capo copre FISICAMENTE la parte SOPRA (seno), a prescindere dalla decenza: es. ExposeBra/BikiniBra sono 'fisicamente sopra' (coprono i boobs) anche se NON decenti. Usata dal gating cover pose (compenetrazione). I flag modesty restano per detection/D11.")]
		public bool physicallyCoversTop;

		// Token: 0x040087DF RID: 34783
		[Tooltip("True se questo capo copre FISICAMENTE la parte SOTTO (inguine), a prescindere dalla decenza.")]
		public bool physicallyCoversBottom;

		// Token: 0x040087E0 RID: 34784
		[Tooltip("Physically obstructs the mouth while worn and visible (for example a gag or face covering).")]
		public bool physicallyBlocksMouth;

		// Token: 0x040087E1 RID: 34785
		[Header("Action restrictions while worn")]
		[Tooltip("Player activity categories disabled while this garment is worn and visible. Use ArmsPose to disable Stimulation, Hold Boobs, Face Cover, etc. without disabling Wall Cover.")]
		public PlayerActivity blockedActivitiesWhileWorn;

		// Token: 0x040087E2 RID: 34786
		[Header("Detection coverage")]
		[Tooltip("Contribution to the player's detection coverage. Values are additive across all worn, visible garments: 10 fully suppresses detection, 0 is neutral, negative values attract attention. Surface-specific exceptions (for example sheer fabric or sunglasses) live below.")]
		public float detectionCoverage;

		// Token: 0x040087E3 RID: 34787
		[Tooltip("Optional per-surface overrides. Match by the stable MCSurfaceVariantSO.variantId, not by list index, so reordering color variants does not change gameplay.")]
		public List<WearableSurfaceGameplayOverride> surfaceGameplayOverrides = new List<WearableSurfaceGameplayOverride>();

		// Token: 0x040087E4 RID: 34788
		[Header("Wear Duration (consumes only while worn)")]
		[Tooltip("If true, this clothing wears out over time WHILE WORN. The durability bar drains only while the item is on the body; taking it off pauses the timer. When it reaches 0 the item breaks: it is auto-unworn and removed from the wardrobe (no toast, just breakSFX).")]
		public bool hasWearDuration;

		// Token: 0x040087E5 RID: 34789
		[Tooltip("Seconds of wear before the item breaks. Drains only while worn.")]
		[Min(0.1f)]
		public float wearDuration = 60f;

		// Token: 0x040087E6 RID: 34790
		[Tooltip("Sound played when the item wears out and breaks. Optional.")]
		public AudioClip breakSFX;

		// Token: 0x040087E7 RID: 34791
		[Header("Clothing Group Mapping (legacy fallback)")]
		[Tooltip("Legacy: name of the ClothingGroup on the player's ClothingToggleController this item maps to. Used only when the MCStyle fields above are not set.")]
		public string clothingGroupName;

		// Token: 0x040087E8 RID: 34792
		[Header("Wear Actions")]
		[Tooltip("Actions available while the item is held in hand. Use Visibility on each action to gate it by current wear state (Always / WhenWorn / WhenUnworn).")]
		public List<WearableActionDefinition> wearActions = new List<WearableActionDefinition>();

		// Token: 0x040087E9 RID: 34793
		[Header("Body Blendshapes (driven via BlendShapeManager)")]
		[Tooltip("Blendshapes lerped from unwornValue to wornValue while wearing. Resolved by name across all renderers known to BlendShapeManager.")]
		public List<WearableBlendshape> blendshapes = new List<WearableBlendshape>();

		// Token: 0x040087EA RID: 34794
		[Header("Material Opacity Fade (LilToon Transparent / One Pass)")]
		[Tooltip("If true, fades the alpha of the main color on every renderer inside the matching clothing group's outfitObjects.")]
		public bool useOpacityFade;

		// Token: 0x040087EB RID: 34795
		[Tooltip("Shader color property carrying the main color (LilToon: '_Color').")]
		public string mainColorProperty = "_Color";

		// Token: 0x040087EC RID: 34796
		[Range(0f, 1f)]
		public float wornAlpha = 1f;

		// Token: 0x040087ED RID: 34797
		[Range(0f, 1f)]
		public float unwornAlpha;

		// Token: 0x040087EE RID: 34798
		[Header("URP Vignette")]
		[Tooltip("If true, fades the URP Vignette intensity (on the Volume assigned to WearableUseController) from 0 to vignetteIntensity while wearing, and back to 0 while unwearing.")]
		public bool useVignette;

		// Token: 0x040087EF RID: 34799
		[Tooltip("Target Vignette intensity when fully worn (0..1).")]
		[Range(0f, 1f)]
		public float vignetteIntensity = 0.7f;

		// Token: 0x040087F0 RID: 34800
		[Tooltip("Duration (seconds) of the vignette fade-in/out. Set to 0 for instant.")]
		[Range(0f, 5f)]
		public float vignetteTransitionDuration = 0.4f;

		// Token: 0x040087F1 RID: 34801
		[Tooltip("Easing curve applied to the vignette fade (input/output 0->1).")]
		public AnimationCurve vignetteCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		// Token: 0x040087F2 RID: 34802
		[Header("Secondary Effects (toggled by SecondaryEffectToggle actions while worn)")]
		[Tooltip("Sub-effects toggled while the item is worn. Opacity targets the same outfit materials as the main effect; blendshapes go through BlendShapeManager.")]
		public List<WearableSecondaryEffect> secondaryEffects = new List<WearableSecondaryEffect>();
	}
}
