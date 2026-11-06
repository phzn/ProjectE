using System;
using System.Collections;
using System.Collections.Generic;
using Bozo.AnimeCharacters;
using BoZo.CharacterCreator;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.Home;
using Features.Inventory;
using Features.PubicHairMask;
using Features.Rank;
using Features.RunModes;
using Invector.vEventSystems;
using ProjectExhibitionist.CharacterCustomization.Core.Presets;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.Futanari;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000B07 RID: 2823
	[DisallowMultipleComponent]
	public class MainCharacterAppearanceApplier : MonoBehaviour
	{
		// Token: 0x17000D50 RID: 3408
		// (get) Token: 0x06004FDD RID: 20445 RVA: 0x001EEAEA File Offset: 0x001ECCEA
		public IReadOnlyList<MainCharacterAppearanceApplier.DefaultLookPart> DefaultLookParts
		{
			get
			{
				return this.defaultLookParts;
			}
		}

		// Token: 0x17000D51 RID: 3409
		// (get) Token: 0x06004FDE RID: 20446 RVA: 0x001EEAF2 File Offset: 0x001ECCF2
		public bool UsesSavedMainCharacterPreset
		{
			get
			{
				return this.applySavedPresetOnStart;
			}
		}

		// Token: 0x17000D52 RID: 3410
		// (get) Token: 0x06004FDF RID: 20447 RVA: 0x001EEAFA File Offset: 0x001ECCFA
		// (set) Token: 0x06004FE0 RID: 20448 RVA: 0x001EEB02 File Offset: 0x001ECD02
		public CustomizationPresetData LastAppliedPreset { get; private set; }

		// Token: 0x14000089 RID: 137
		// (add) Token: 0x06004FE1 RID: 20449 RVA: 0x001EEB0C File Offset: 0x001ECD0C
		// (remove) Token: 0x06004FE2 RID: 20450 RVA: 0x001EEB44 File Offset: 0x001ECD44
		public event Action<MainCharacterAppearanceApplier> AppearanceApplied;

		// Token: 0x06004FE3 RID: 20451 RVA: 0x001EEB7C File Offset: 0x001ECD7C
		private void Awake()
		{
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in base.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				Mesh sharedMesh = skinnedMeshRenderer.sharedMesh;
				if (!(sharedMesh == null) && sharedMesh.blendShapeCount != 0)
				{
					float[] array = new float[sharedMesh.blendShapeCount];
					for (int j = 0; j < array.Length; j++)
					{
						array[j] = skinnedMeshRenderer.GetBlendShapeWeight(j);
					}
					this._homeMorphBaseline.Add(new MainCharacterAppearanceApplier.MorphBaseline
					{
						renderer = skinnedMeshRenderer,
						mesh = sharedMesh,
						weights = array
					});
				}
			}
		}

		// Token: 0x06004FE4 RID: 20452 RVA: 0x001EEC10 File Offset: 0x001ECE10
		public void RestoreAuthoredMorphsForHome()
		{
			BlendShapeManager[] componentsInChildren = base.GetComponentsInChildren<BlendShapeManager>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].ClearOverridesForHomeReturn();
			}
			foreach (MainCharacterAppearanceApplier.MorphBaseline morphBaseline in this._homeMorphBaseline)
			{
				if (!(morphBaseline.renderer == null) && !(morphBaseline.renderer.sharedMesh != morphBaseline.mesh))
				{
					for (int j = 0; j < morphBaseline.weights.Length; j++)
					{
						morphBaseline.renderer.SetBlendShapeWeight(j, morphBaseline.weights[j]);
					}
				}
			}
		}

		// Token: 0x06004FE5 RID: 20453 RVA: 0x001EECD0 File Offset: 0x001ECED0
		private void OnActiveSceneChanged(Scene previous, Scene current)
		{
			if (!this.applySavedPresetOnStart || current.name != "CielHome" || previous == current)
			{
				return;
			}
			this._homeRestorePending = true;
			if (this._homeRestoreRoutine != null)
			{
				base.StopCoroutine(this._homeRestoreRoutine);
			}
			this._homeRestoreRoutine = base.StartCoroutine(this.RestoreHomeAfterStandaloneLoad());
		}

		// Token: 0x06004FE6 RID: 20454 RVA: 0x001EED2F File Offset: 0x001ECF2F
		private IEnumerator RestoreHomeAfterStandaloneLoad()
		{
			yield return null;
			while (this._homeRestorePending && SceneTransitionManager.Instance != null && SceneTransitionManager.Instance.IsTransitioning)
			{
				yield return null;
			}
			this._homeRestoreRoutine = null;
			this.RestoreHomeStateIfPending();
			yield break;
		}

		// Token: 0x06004FE7 RID: 20455 RVA: 0x001EED3E File Offset: 0x001ECF3E
		public void RestoreHomeStateIfPending()
		{
			if (this._homeRestorePending)
			{
				this.RestoreHomeState();
			}
		}

		// Token: 0x06004FE8 RID: 20456 RVA: 0x001EED50 File Offset: 0x001ECF50
		public void RestoreHomeState()
		{
			if (!this.applySavedPresetOnStart || SceneManager.GetActiveScene().name != "CielHome")
			{
				return;
			}
			this._homeRestorePending = false;
			RunModeService.ClearPreparedRun();
			CustomizationPresetData customizationPresetData;
			MainCharacterAppearanceService.TryGetInUseMainCharacterPreset(out customizationPresetData);
			HomePlayerStateRestoration.Restore(base.transform, customizationPresetData);
		}

		// Token: 0x06004FE9 RID: 20457 RVA: 0x001EEDA0 File Offset: 0x001ECFA0
		private void OnEnable()
		{
			SceneManager.activeSceneChanged += this.OnActiveSceneChanged;
			if (this.styleService == null)
			{
				this.styleService = base.GetComponentInChildren<MCStyleCustomizationService>(true);
			}
			if (this.styleService != null && !this._subscribedToStyleEvents)
			{
				this.styleService.OnActiveStateChanged += this.SyncDefaultLookVisibility;
				this.styleService.OnPreviewClothingVisibilityChanged += this.SyncDefaultLookVisibility;
				this._subscribedToStyleEvents = true;
			}
			if (this._previewVisibilityApplied)
			{
				this.SyncDefaultLookVisibility();
			}
			if (this.applySavedPresetOnStart && !this._subscribedToInUseEvent)
			{
				MainCharacterAppearanceEvents.InUsePresetChanged += this.ReloadAppearance;
				this._subscribedToInUseEvent = true;
			}
		}

		// Token: 0x06004FEA RID: 20458 RVA: 0x001EEE5C File Offset: 0x001ED05C
		private void OnDisable()
		{
			SceneManager.activeSceneChanged -= this.OnActiveSceneChanged;
			if (this._homeRestoreRoutine != null)
			{
				base.StopCoroutine(this._homeRestoreRoutine);
			}
			this._homeRestoreRoutine = null;
			if (this.styleService != null && this._subscribedToStyleEvents)
			{
				this.styleService.OnActiveStateChanged -= this.SyncDefaultLookVisibility;
				this.styleService.OnPreviewClothingVisibilityChanged -= this.SyncDefaultLookVisibility;
				this._subscribedToStyleEvents = false;
			}
			if (this._subscribedToInUseEvent)
			{
				MainCharacterAppearanceEvents.InUsePresetChanged -= this.ReloadAppearance;
				this._subscribedToInUseEvent = false;
			}
		}

		// Token: 0x06004FEB RID: 20459 RVA: 0x001EEF00 File Offset: 0x001ED100
		private IEnumerator Start()
		{
			if (!this.applySavedPresetOnStart)
			{
				yield break;
			}
			yield return null;
			this.ReloadAppearance();
			yield break;
		}

		// Token: 0x06004FEC RID: 20460 RVA: 0x001EEF0F File Offset: 0x001ED10F
		public void SyncDefaultLookVisibility()
		{
			if (this.styleService == null)
			{
				this.styleService = base.GetComponentInChildren<MCStyleCustomizationService>(true);
			}
			if (this.styleService == null)
			{
				return;
			}
			this.ApplyDefaultLookVisibility();
		}

		// Token: 0x06004FED RID: 20461 RVA: 0x001EEF44 File Offset: 0x001ED144
		public void ReloadAppearance()
		{
			CustomizationPresetData customizationPresetData;
			if (!MainCharacterAppearanceService.TryGetInUseMainCharacterPreset(out customizationPresetData))
			{
				customizationPresetData = new CustomizationPresetData
				{
					isSystemPreset = true,
					stateData = new CharacterStateData()
				};
			}
			this.ApplyPreset(customizationPresetData);
		}

		// Token: 0x06004FEE RID: 20462 RVA: 0x001EEF7C File Offset: 0x001ED17C
		public void ApplyPreset(CustomizationPresetData preset)
		{
			if (((preset != null) ? preset.stateData : null) == null)
			{
				return;
			}
			using (PubicHairStencilMask.BeginAppearanceUpdate(base.transform))
			{
				this.ApplyPresetWithMaterialOwnership(preset);
			}
			this.LastAppliedPreset = preset;
			Action<MainCharacterAppearanceApplier> appearanceApplied = this.AppearanceApplied;
			if (appearanceApplied == null)
			{
				return;
			}
			appearanceApplied(this);
		}

		// Token: 0x06004FEF RID: 20463 RVA: 0x001EEFE0 File Offset: 0x001ED1E0
		private void ApplyPresetWithMaterialOwnership(CustomizationPresetData preset)
		{
			this.ResolveComponents();
			bool flag = this.applySavedPresetOnStart && RankWardrobePolicy.IsRestricted && !WardrobeGameplayContext.IsPresentationOnly(this);
			MCStyleCustomizationService.WardrobeSnapshot wardrobeSnapshot = ((flag && this.styleService != null) ? this.styleService.CaptureWardrobeState(this.clothingToggleController) : null);
			this.ResetToDefaultsBeforeApply(preset.isSystemPreset);
			this.ApplyStateData(preset.stateData);
			if (flag)
			{
				RankRuntimeController rankRuntimeController = RankRuntimeController.Current;
				if (rankRuntimeController != null)
				{
					rankRuntimeController.RefreshOutfitAfterAppearance(wardrobeSnapshot);
				}
			}
			if (this.enableDebugLogs)
			{
				Debug.Log("[MCAppearanceApplier] Preset in uso del main character applicato al player.", this);
			}
		}

		// Token: 0x06004FF0 RID: 20464 RVA: 0x001EF074 File Offset: 0x001ED274
		private void ResetToDefaultsBeforeApply(bool isSystemPreset)
		{
			ClothingToggleController clothingToggleController = this.clothingToggleController;
			if (clothingToggleController != null)
			{
				clothingToggleController.ResetVisibilityForPreset();
			}
			MCStyleCustomizationService mcstyleCustomizationService = this.styleService;
			if (mcstyleCustomizationService != null)
			{
				mcstyleCustomizationService.SetNaked(false);
			}
			if (this.styleService != null)
			{
				if (isSystemPreset)
				{
					this.styleService.ResetForNewPresetDraft();
				}
				else
				{
					this.styleService.ResetAllPartsToDefaults();
				}
			}
			CharacterBlendShapeController characterBlendShapeController = this.blendShapeController;
			if (characterBlendShapeController != null)
			{
				characterBlendShapeController.ResetMappedValuesToDefaults();
			}
			if (this.skinDecalController != null)
			{
				this.skinDecalController.ResetToStartupDefaults();
				if (!isSystemPreset)
				{
					this.skinDecalController.DeactivateAllDecals();
				}
			}
		}

		// Token: 0x06004FF1 RID: 20465 RVA: 0x001EF108 File Offset: 0x001ED308
		private void ResolveComponents()
		{
			if (this.styleService == null)
			{
				this.styleService = base.GetComponentInChildren<MCStyleCustomizationService>(true);
			}
			if (this.blendShapeController == null)
			{
				this.blendShapeController = base.GetComponentInChildren<CharacterBlendShapeController>(true);
			}
			if (this.clothingToggleController == null)
			{
				this.clothingToggleController = base.GetComponentInChildren<ClothingToggleController>(true);
			}
			if (this.skinDecalController == null)
			{
				this.skinDecalController = base.GetComponentInChildren<SkinDecalController>(true);
			}
		}

		// Token: 0x06004FF2 RID: 20466 RVA: 0x001EF184 File Offset: 0x001ED384
		private void ApplyStateData(CharacterStateData stateData)
		{
			this.ResetFutanariBaseline();
			this.ApplyBlendShapes(stateData.blendShapes);
			this.ApplySkinDecals(stateData.skinDecals);
			if (this.styleService != null)
			{
				this.styleService.ApplyState(stateData);
				this.ApplyDefaultLookVisibility();
			}
			else
			{
				this.WarnMissing("MCStyleCustomizationService", "stili/outfit/colori");
			}
			this.ApplyClothingToggles(stateData.clothingToggles);
			if (RunModeService.ShouldStartClean && this.styleService != null)
			{
				this.styleService.SetFunctionalWardrobeHidden(true);
			}
			this.SyncBoobsSizeState(stateData.blendShapes);
			this.ReRegisterInvectorAnimatorListeners();
		}

		// Token: 0x06004FF3 RID: 20467 RVA: 0x001EF220 File Offset: 0x001ED420
		private void ReRegisterInvectorAnimatorListeners()
		{
			vIAnimatorStateInfoController componentInChildren = base.GetComponentInChildren<vIAnimatorStateInfoController>(true);
			if (componentInChildren != null)
			{
				componentInChildren.Register();
			}
		}

		// Token: 0x06004FF4 RID: 20468 RVA: 0x001EF240 File Offset: 0x001ED440
		private void ApplyDefaultLookVisibility()
		{
			this._previewVisibilityApplied = this.styleService.IsPreviewClothingHidden;
			for (int i = 0; i < this.defaultLookParts.Count; i++)
			{
				MainCharacterAppearanceApplier.DefaultLookPart defaultLookPart = this.defaultLookParts[i];
				bool flag;
				if (defaultLookPart != null && !(defaultLookPart.sceneDefaultObject == null) && !string.IsNullOrWhiteSpace(defaultLookPart.partId) && this.styleService.TryGetPartUsesSceneDefault(defaultLookPart.partId, out flag))
				{
					defaultLookPart.sceneDefaultObject.SetActive(flag && !this.styleService.IsPartHiddenForPreview(defaultLookPart.partId));
					if (this.enableDebugLogs && !flag)
					{
						Debug.Log(string.Concat(new string[]
						{
							"[MCAppearanceApplier] Look di default '",
							defaultLookPart.sceneDefaultObject.name,
							"' nascosto (parte '",
							defaultLookPart.partId,
							"' stilizzata dal preset)."
						}), this);
					}
				}
			}
		}

		// Token: 0x06004FF5 RID: 20469 RVA: 0x001EF334 File Offset: 0x001ED534
		private void ResetFutanariBaseline()
		{
			FutanariModeController componentInChildren = base.GetComponentInChildren<FutanariModeController>(true);
			if (componentInChildren != null)
			{
				componentInChildren.SetFutanariMode(false);
			}
		}

		// Token: 0x06004FF6 RID: 20470 RVA: 0x001EF35C File Offset: 0x001ED55C
		private void ApplyBlendShapes(List<BlendShapeValue> blendShapes)
		{
			if (blendShapes == null || blendShapes.Count == 0)
			{
				return;
			}
			if (this.blendShapeController == null || this.blendShapeController.BlendShapeMap == null)
			{
				this.WarnMissing("CharacterBlendShapeController", "blendshape corpo/viso");
				return;
			}
			List<BlendShapeMapSO.BlendShapeEntry> blendShapeMappings = this.blendShapeController.BlendShapeMap.blendShapeMappings;
			if (blendShapeMappings == null)
			{
				return;
			}
			for (int i = 0; i < blendShapes.Count; i++)
			{
				BlendShapeValue value = blendShapes[i];
				if (value != null && !string.IsNullOrWhiteSpace(value.blendShapeName))
				{
					BlendShapeMapSO.BlendShapeEntry blendShapeEntry = blendShapeMappings.Find((BlendShapeMapSO.BlendShapeEntry e) => e != null && e.displayName == value.blendShapeName);
					if (blendShapeEntry != null)
					{
						this.blendShapeController.ApplyEntryValue(blendShapeEntry, value.value);
					}
				}
			}
		}

		// Token: 0x06004FF7 RID: 20471 RVA: 0x001EF428 File Offset: 0x001ED628
		private void SyncBoobsSizeState(List<BlendShapeValue> blendShapes)
		{
			BoobsController componentInChildren = base.GetComponentInChildren<BoobsController>(true);
			if (componentInChildren == null)
			{
				this.WarnMissing("BoobsController", "taglia seno (BoobsType)");
				return;
			}
			float num = 3f;
			if (blendShapes != null)
			{
				BlendShapeValue blendShapeValue = blendShapes.Find((BlendShapeValue b) => b != null && b.blendShapeName == this.breastSizeEntryName);
				if (blendShapeValue != null)
				{
					num = blendShapeValue.value;
				}
			}
			componentInChildren.SetStateFromBoobsTypeValue(Mathf.RoundToInt(num));
		}

		// Token: 0x06004FF8 RID: 20472 RVA: 0x001EF48C File Offset: 0x001ED68C
		private void ApplySkinDecals(List<SkinDecalStateValue> skinDecals)
		{
			if (skinDecals == null || skinDecals.Count == 0)
			{
				return;
			}
			if (this.skinDecalController == null)
			{
				this.WarnMissing("SkinDecalController", "decal pelle");
				return;
			}
			for (int i = 0; i < skinDecals.Count; i++)
			{
				SkinDecalStateValue skinDecalStateValue = skinDecals[i];
				if (skinDecalStateValue != null && !string.IsNullOrWhiteSpace(skinDecalStateValue.decalId))
				{
					this.skinDecalController.TrySetDecalStateByName(skinDecalStateValue.decalId, skinDecalStateValue.isEnabled, skinDecalStateValue.opacity, skinDecalStateValue.tint);
				}
			}
		}

		// Token: 0x06004FF9 RID: 20473 RVA: 0x001EF514 File Offset: 0x001ED714
		private void ApplyClothingToggles(List<ClothingToggleValue> clothingToggles)
		{
			if (clothingToggles == null || clothingToggles.Count == 0)
			{
				return;
			}
			if (this.clothingToggleController == null)
			{
				this.WarnMissing("ClothingToggleController", "gruppi vestiario");
				return;
			}
			for (int i = 0; i < clothingToggles.Count; i++)
			{
				ClothingToggleValue clothingToggleValue = clothingToggles[i];
				if (clothingToggleValue != null && !string.IsNullOrWhiteSpace(clothingToggleValue.groupId))
				{
					this.clothingToggleController.SetGroupState(clothingToggleValue.groupId, clothingToggleValue.isEnabled);
				}
			}
		}

		// Token: 0x06004FFA RID: 20474 RVA: 0x001EF58C File Offset: 0x001ED78C
		private void WarnMissing(string componentName, string feature)
		{
			Debug.LogWarning(string.Concat(new string[] { "[MCAppearanceApplier] ", componentName, " non trovato sul player: ", feature, " non applicati." }), this);
		}

		// Token: 0x040050CA RID: 20682
		[Header("Riferimenti rig (se nulli: GetComponentInChildren)")]
		[SerializeField]
		private MCStyleCustomizationService styleService;

		// Token: 0x040050CB RID: 20683
		[SerializeField]
		private CharacterBlendShapeController blendShapeController;

		// Token: 0x040050CC RID: 20684
		[SerializeField]
		private ClothingToggleController clothingToggleController;

		// Token: 0x040050CD RID: 20685
		[SerializeField]
		private SkinDecalController skinDecalController;

		// Token: 0x040050CE RID: 20686
		[Header("Look di default baked nel prefab")]
		[Tooltip("Oggetti del look di default (es. capelli baked) da nascondere quando il preset applica uno stile per la parte corrispondente: il sistema stili spawna il pezzo nuovo e quello baked resterebbe visibile sovrapposto.")]
		[SerializeField]
		private List<MainCharacterAppearanceApplier.DefaultLookPart> defaultLookParts = new List<MainCharacterAppearanceApplier.DefaultLookPart>();

		// Token: 0x040050CF RID: 20687
		[Header("Taglia seno (BoobsType)")]
		[Tooltip("displayName dell'entry Select del BlendShapeMap che codifica la taglia del seno nel preset. Il value dell'entry (0-5) coincide con BoobsConfigSO.animatorBoobsTypeValue.")]
		[SerializeField]
		private string breastSizeEntryName = "Breast Size";

		// Token: 0x040050D0 RID: 20688
		[Header("Modalità")]
		[Tooltip("Se true (default), allo Start applica il preset in uso letto da disco. Sul PREVIEW di customizzazione (CielPreview) va messo a FALSE: lo stato lo guida la sessione (CustomizationHubPresetListController), non il preset su disco. La mappatura defaultLookParts resta usata via SyncDefaultLookVisibility().")]
		[SerializeField]
		private bool applySavedPresetOnStart = true;

		// Token: 0x040050D1 RID: 20689
		[SerializeField]
		private bool enableDebugLogs;

		// Token: 0x040050D4 RID: 20692
		private bool _subscribedToStyleEvents;

		// Token: 0x040050D5 RID: 20693
		private bool _subscribedToInUseEvent;

		// Token: 0x040050D6 RID: 20694
		private bool _previewVisibilityApplied;

		// Token: 0x040050D7 RID: 20695
		private bool _homeRestorePending;

		// Token: 0x040050D8 RID: 20696
		private Coroutine _homeRestoreRoutine;

		// Token: 0x040050D9 RID: 20697
		private readonly List<MainCharacterAppearanceApplier.MorphBaseline> _homeMorphBaseline = new List<MainCharacterAppearanceApplier.MorphBaseline>();

		// Token: 0x02000B08 RID: 2824
		[Serializable]
		public class DefaultLookPart
		{
			// Token: 0x040050DA RID: 20698
			[Tooltip("partId della part definition (es. 'Head.Hair').")]
			public string partId;

			// Token: 0x040050DB RID: 20699
			[Tooltip("GameObject del look di default baked nel prefab (es. i capelli di default). Viene nascosto quando il preset applica uno stile per questa parte, ri-mostrato se il preset usa lo scene default.")]
			public GameObject sceneDefaultObject;
		}

		// Token: 0x02000B09 RID: 2825
		private sealed class MorphBaseline
		{
			// Token: 0x040050DC RID: 20700
			public SkinnedMeshRenderer renderer;

			// Token: 0x040050DD RID: 20701
			public Mesh mesh;

			// Token: 0x040050DE RID: 20702
			public float[] weights;
		}
	}
}
