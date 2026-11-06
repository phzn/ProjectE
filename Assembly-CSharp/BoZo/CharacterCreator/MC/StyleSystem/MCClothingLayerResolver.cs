using System;
using System.Collections.Generic;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using UnityEngine;

namespace BoZo.CharacterCreator.MC.StyleSystem
{
	// Token: 0x020007D6 RID: 2006
	public class MCClothingLayerResolver : MonoBehaviour
	{
		// Token: 0x06003538 RID: 13624 RVA: 0x0012F116 File Offset: 0x0012D316
		private void OnEnable()
		{
			this.ResolveRefs();
			this.Subscribe();
			this.MarkDirty();
		}

		// Token: 0x06003539 RID: 13625 RVA: 0x0012F12A File Offset: 0x0012D32A
		private void OnDisable()
		{
			this.Unsubscribe();
		}

		// Token: 0x0600353A RID: 13626 RVA: 0x0012F134 File Offset: 0x0012D334
		private void ResolveRefs()
		{
			if (this.styleService == null)
			{
				this.styleService = base.GetComponentInParent<MCStyleCustomizationService>();
				if (this.styleService == null)
				{
					this.styleService = Object.FindObjectOfType<MCStyleCustomizationService>(true);
				}
			}
			if (this.appearanceApplier == null && this.styleService != null)
			{
				this.appearanceApplier = this.styleService.GetComponentInParent<MainCharacterAppearanceApplier>();
			}
			this.BuildBakedMap();
		}

		// Token: 0x0600353B RID: 13627 RVA: 0x0012F1A8 File Offset: 0x0012D3A8
		private void BuildBakedMap()
		{
			this._bakedByPartId.Clear();
			if (this.appearanceApplier == null || this.appearanceApplier.DefaultLookParts == null)
			{
				return;
			}
			foreach (MainCharacterAppearanceApplier.DefaultLookPart defaultLookPart in this.appearanceApplier.DefaultLookParts)
			{
				if (defaultLookPart != null && !string.IsNullOrWhiteSpace(defaultLookPart.partId) && defaultLookPart.sceneDefaultObject != null)
				{
					this._bakedByPartId[defaultLookPart.partId] = defaultLookPart.sceneDefaultObject;
				}
			}
		}

		// Token: 0x0600353C RID: 13628 RVA: 0x0012F250 File Offset: 0x0012D450
		private void Subscribe()
		{
			if (this.styleService != null && !this._subscribed)
			{
				this.styleService.OnActiveStateChanged += this.MarkDirty;
				this.styleService.OnSlotVisibilityChanged += this.MarkDirty;
				this._subscribed = true;
			}
		}

		// Token: 0x0600353D RID: 13629 RVA: 0x0012F2A8 File Offset: 0x0012D4A8
		private void Unsubscribe()
		{
			if (this.styleService != null && this._subscribed)
			{
				this.styleService.OnActiveStateChanged -= this.MarkDirty;
				this.styleService.OnSlotVisibilityChanged -= this.MarkDirty;
				this._subscribed = false;
			}
		}

		// Token: 0x0600353E RID: 13630 RVA: 0x0012F300 File Offset: 0x0012D500
		private void MarkDirty()
		{
			this._dirty = true;
		}

		// Token: 0x0600353F RID: 13631 RVA: 0x0012F309 File Offset: 0x0012D509
		private void LateUpdate()
		{
			if (!this._dirty)
			{
				return;
			}
			this._dirty = false;
			this.Resolve();
		}

		// Token: 0x06003540 RID: 13632 RVA: 0x0012F324 File Offset: 0x0012D524
		private void Resolve()
		{
			if (this.styleService == null)
			{
				return;
			}
			Dictionary<string, MCLayerRuleSO> dictionary = new Dictionary<string, MCLayerRuleSO>();
			foreach (MCStylePartDefinitionSO mcstylePartDefinitionSO in this.styleService.PartDefinitions)
			{
				if (!(mcstylePartDefinitionSO == null))
				{
					if (mcstylePartDefinitionSO.categoryLayerRules != null)
					{
						foreach (MCLayerRuleSO mclayerRuleSO in mcstylePartDefinitionSO.categoryLayerRules)
						{
							if (mclayerRuleSO != null)
							{
								dictionary[mclayerRuleSO.PairKey] = mclayerRuleSO;
							}
						}
					}
					MCStylePresetSO activePreset = this.styleService.GetActivePreset(mcstylePartDefinitionSO.partId);
					if (activePreset != null && activePreset.itemLayerRuleOverrides != null)
					{
						foreach (MCLayerRuleSO mclayerRuleSO2 in activePreset.itemLayerRuleOverrides)
						{
							if (mclayerRuleSO2 != null)
							{
								dictionary[mclayerRuleSO2.PairKey] = mclayerRuleSO2;
							}
						}
					}
				}
			}
			foreach (MCLayerRuleSO mclayerRuleSO3 in dictionary.Values)
			{
				string text;
				bool flag = this.styleService.TryGetEquippedItem(mclayerRuleSO3.occluderSlotId, out text);
				if (flag)
				{
					GameObject spawnedRoot = this.styleService.GetSpawnedRoot(mclayerRuleSO3.occluderSlotId);
					if (spawnedRoot != null && !spawnedRoot.activeSelf)
					{
						flag = false;
					}
				}
				if (flag && !string.IsNullOrWhiteSpace(mclayerRuleSO3.occluderPresetIdFilter) && !string.Equals(text, mclayerRuleSO3.occluderPresetIdFilter, StringComparison.OrdinalIgnoreCase))
				{
					flag = false;
				}
				bool flag2 = flag == mclayerRuleSO3.whenOccluderEquipped;
				bool flag3 = mclayerRuleSO3.effect == MCLayerRuleSO.LayerEffect.HideOccluded;
				bool flag4 = (flag2 ? (!flag3) : flag3);
				this.ApplyOccludedVisibility(mclayerRuleSO3.occludedSlotId, flag4);
				if (this.verboseLogs)
				{
					Debug.Log(string.Format("[MCLayerResolver] {0}: occluderEquipped={1} conditionMet={2} → occluded '{3}' visible={4}", new object[] { mclayerRuleSO3.PairKey, flag, flag2, mclayerRuleSO3.occludedSlotId, flag4 }), this);
				}
			}
		}

		// Token: 0x06003541 RID: 13633 RVA: 0x0012F5D4 File Offset: 0x0012D7D4
		private void ApplyOccludedVisibility(string occludedSlotId, bool visible)
		{
			if (string.IsNullOrWhiteSpace(occludedSlotId))
			{
				return;
			}
			visible = visible && !this.styleService.IsPartHiddenForPreview(occludedSlotId);
			GameObject spawnedRoot = this.styleService.GetSpawnedRoot(occludedSlotId);
			if (spawnedRoot != null && spawnedRoot.activeSelf != visible)
			{
				this.styleService.SetSlotPhysicallyHidden(occludedSlotId, !visible);
			}
			GameObject gameObject;
			if (this._bakedByPartId.TryGetValue(occludedSlotId, out gameObject) && gameObject != null)
			{
				bool flag2;
				bool flag = this.styleService.TryGetPartUsesSceneDefault(occludedSlotId, out flag2) && flag2;
				bool flag3 = visible && flag;
				if (gameObject.activeSelf != flag3)
				{
					gameObject.SetActive(flag3);
				}
			}
		}

		// Token: 0x06003542 RID: 13634 RVA: 0x0012F672 File Offset: 0x0012D872
		public void ForceResolve()
		{
			this.ResolveRefs();
			this.Resolve();
		}

		// Token: 0x04003578 RID: 13688
		[Tooltip("Service degli stili MC. Se nullo viene risolto nei genitori/scena.")]
		[SerializeField]
		private MCStyleCustomizationService styleService;

		// Token: 0x04003579 RID: 13689
		[Tooltip("Applier (modalità preview) per accedere ai GO baked di default da nascondere/mostrare.")]
		[SerializeField]
		private MainCharacterAppearanceApplier appearanceApplier;

		// Token: 0x0400357A RID: 13690
		[SerializeField]
		private bool verboseLogs;

		// Token: 0x0400357B RID: 13691
		private bool _dirty;

		// Token: 0x0400357C RID: 13692
		private bool _subscribed;

		// Token: 0x0400357D RID: 13693
		private readonly Dictionary<string, GameObject> _bakedByPartId = new Dictionary<string, GameObject>();
	}
}
