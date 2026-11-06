using System;
using System.Collections.Generic;
using BoZo.CharacterCreator;
using BoZo.CharacterCreator.MC.StyleSystem;
using Features.CensorBars;
using Features.ExposeLingerie;
using Features.Flash;
using Features.Inventory;
using Features.Inventory.Wearable;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectExhibitionist
{
	// Token: 0x020008D0 RID: 2256
	public class ClothingToggleController : MonoBehaviour
	{
		// Token: 0x14000068 RID: 104
		// (add) Token: 0x06003CB0 RID: 15536 RVA: 0x0016919C File Offset: 0x0016739C
		// (remove) Token: 0x06003CB1 RID: 15537 RVA: 0x001691D4 File Offset: 0x001673D4
		[Tooltip("Strongly-typed coverage event (same firing as OnClothingStateChanged).")]
		public event Action<bool> OnCoverageChanged;

		// Token: 0x14000069 RID: 105
		// (add) Token: 0x06003CB2 RID: 15538 RVA: 0x0016920C File Offset: 0x0016740C
		// (remove) Token: 0x06003CB3 RID: 15539 RVA: 0x00169244 File Offset: 0x00167444
		[Tooltip("Fired whenever a single managed slot's visibility changes (partId, visible). Lets systems that track worn-state per item (WearableUseController) stay in sync when the global naked↔clothed Tab toggle hides/shows slots without going through them.")]
		public event Action<string, bool> OnSlotVisibilityChanged;

		// Token: 0x1400006A RID: 106
		// (add) Token: 0x06003CB4 RID: 15540 RVA: 0x0016927C File Offset: 0x0016747C
		// (remove) Token: 0x06003CB5 RID: 15541 RVA: 0x001692B4 File Offset: 0x001674B4
		public event Action OnWardrobeHierarchyChanged;

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x06003CB6 RID: 15542 RVA: 0x001692E9 File Offset: 0x001674E9
		public bool IsCovered
		{
			get
			{
				return this.isFullyClothed;
			}
		}

		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x06003CB7 RID: 15543 RVA: 0x001692E9 File Offset: 0x001674E9
		public bool IsFullyClothed
		{
			get
			{
				return this.isFullyClothed;
			}
		}

		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x06003CB8 RID: 15544 RVA: 0x001692F1 File Offset: 0x001674F1
		public bool UsingService
		{
			get
			{
				return this._serviceBound;
			}
		}

		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x06003CB9 RID: 15545 RVA: 0x001692F9 File Offset: 0x001674F9
		public bool IsTopCovered
		{
			get
			{
				return this._topCovered;
			}
		}

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x06003CBA RID: 15546 RVA: 0x00169301 File Offset: 0x00167501
		public bool IsBottomCovered
		{
			get
			{
				return this._bottomCovered;
			}
		}

		// Token: 0x1400006B RID: 107
		// (add) Token: 0x06003CBB RID: 15547 RVA: 0x0016930C File Offset: 0x0016750C
		// (remove) Token: 0x06003CBC RID: 15548 RVA: 0x00169344 File Offset: 0x00167544
		public event Action OnGranularCoverageChanged;

		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x06003CBD RID: 15549 RVA: 0x00169379 File Offset: 0x00167579
		public float RawDetectionCoverage
		{
			get
			{
				return this._rawDetectionCoverage;
			}
		}

		// Token: 0x17000A41 RID: 2625
		// (get) Token: 0x06003CBE RID: 15550 RVA: 0x00169381 File Offset: 0x00167581
		public float DetectionCoverage
		{
			get
			{
				return this._detectionCoverage;
			}
		}

		// Token: 0x17000A42 RID: 2626
		// (get) Token: 0x06003CBF RID: 15551 RVA: 0x00169389 File Offset: 0x00167589
		public float DetectionAttentionMultiplier
		{
			get
			{
				return ClothingToggleController.DetectionCoverageToAttentionMultiplier(this._detectionCoverage);
			}
		}

		// Token: 0x17000A43 RID: 2627
		// (get) Token: 0x06003CC0 RID: 15552 RVA: 0x00169396 File Offset: 0x00167596
		public bool IsDetectionFullyCovered
		{
			get
			{
				return this._detectionCoverage >= 9.9999f;
			}
		}

		// Token: 0x17000A44 RID: 2628
		// (get) Token: 0x06003CC1 RID: 15553 RVA: 0x001693A8 File Offset: 0x001675A8
		public bool HasVisibleFullCoverageOutfit
		{
			get
			{
				foreach (WearableItemSO wearableItemSO in this.DetectionItems)
				{
					if (wearableItemSO.physicallyCoversTop && wearableItemSO.physicallyCoversBottom && this.IsWearableWornAndVisible(wearableItemSO) && this.styleService.GetSpawnedRoot(wearableItemSO.mcStylePartId).activeInHierarchy && wearableItemSO.ResolveDetectionCoverage(this.ResolveActiveSurface(wearableItemSO)) >= 9.9999f)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1400006C RID: 108
		// (add) Token: 0x06003CC2 RID: 15554 RVA: 0x00169440 File Offset: 0x00167640
		// (remove) Token: 0x06003CC3 RID: 15555 RVA: 0x00169478 File Offset: 0x00167678
		public event Action<float> OnDetectionCoverageChanged;

		// Token: 0x06003CC4 RID: 15556 RVA: 0x001694AD File Offset: 0x001676AD
		public static float DetectionCoverageToAttentionMultiplier(float coverage)
		{
			return Mathf.Max(0f, 1f - coverage / 10f);
		}

		// Token: 0x17000A45 RID: 2629
		// (get) Token: 0x06003CC5 RID: 15557 RVA: 0x001694C6 File Offset: 0x001676C6
		public bool IsTopPhysicallyCovered
		{
			get
			{
				return this._topPhysicallyCovered;
			}
		}

		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x06003CC6 RID: 15558 RVA: 0x001694CE File Offset: 0x001676CE
		public bool IsBottomPhysicallyCovered
		{
			get
			{
				return this._bottomPhysicallyCovered;
			}
		}

		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x06003CC7 RID: 15559 RVA: 0x001694D8 File Offset: 0x001676D8
		public float NormalizedBodyCoverage
		{
			get
			{
				if (this.isFullyClothed)
				{
					return 1f;
				}
				if (!this._serviceBound)
				{
					return 0f;
				}
				float num = (this._topPhysicallyCovered ? 0.5f : 0f) + (this._bottomPhysicallyCovered ? 0.5f : 0f);
				float num2 = (this._topCovered ? 0.5f : 0f) + (this._bottomCovered ? 0.5f : 0f);
				return Mathf.Clamp01(num * 0.7f + num2 * 0.3f);
			}
		}

		// Token: 0x06003CC8 RID: 15560 RVA: 0x00169567 File Offset: 0x00167767
		private void Start()
		{
			this.BindService();
			if (this._serviceBound)
			{
				this.RecomputeCoverage(false);
				return;
			}
			this.InitializeBlendShapeCache();
		}

		// Token: 0x06003CC9 RID: 15561 RVA: 0x00169585 File Offset: 0x00167785
		private void OnEnable()
		{
			if (Application.isPlaying)
			{
				StreamerCensorCharacter.Ensure(this);
			}
			if (!this._serviceBound)
			{
				this.BindService();
			}
		}

		// Token: 0x06003CCA RID: 15562 RVA: 0x001695A2 File Offset: 0x001677A2
		private void OnDisable()
		{
			this.UnsubscribeService();
		}

		// Token: 0x06003CCB RID: 15563 RVA: 0x001695A2 File Offset: 0x001677A2
		private void OnDestroy()
		{
			this.UnsubscribeService();
		}

		// Token: 0x06003CCC RID: 15564 RVA: 0x001695AC File Offset: 0x001677AC
		private void BindService()
		{
			if (this._serviceBound)
			{
				return;
			}
			this._presentationItemsDirty = true;
			if (this.styleService == null)
			{
				this.styleService = base.GetComponentInChildren<MCStyleCustomizationService>(true) ?? Object.FindObjectOfType<MCStyleCustomizationService>(true);
			}
			if (this.styleService == null)
			{
				return;
			}
			this.styleService.OnActiveStateChanged += this.HandleServiceStateChanged;
			this.styleService.OnPhysicalSwapCompleted += this.HandlePhysicalChanged;
			this.styleService.OnPhysicalRemoved += this.HandlePhysicalChanged;
			this.styleService.OnSlotVisibilityChanged += this.HandleServiceSlotVisibilityChanged;
			this._serviceBound = true;
			this.RebuildManagedSlots();
		}

		// Token: 0x06003CCD RID: 15565 RVA: 0x00169668 File Offset: 0x00167868
		private void UnsubscribeService()
		{
			if (!this._serviceBound || this.styleService == null)
			{
				return;
			}
			this.styleService.OnActiveStateChanged -= this.HandleServiceStateChanged;
			this.styleService.OnPhysicalSwapCompleted -= this.HandlePhysicalChanged;
			this.styleService.OnPhysicalRemoved -= this.HandlePhysicalChanged;
			this.styleService.OnSlotVisibilityChanged -= this.HandleServiceSlotVisibilityChanged;
			this._serviceBound = false;
		}

		// Token: 0x06003CCE RID: 15566 RVA: 0x001696EF File Offset: 0x001678EF
		private void HandleServiceStateChanged()
		{
			this._presentationItemsDirty = true;
			this.RebuildManagedSlots();
			Action onWardrobeHierarchyChanged = this.OnWardrobeHierarchyChanged;
			if (onWardrobeHierarchyChanged == null)
			{
				return;
			}
			onWardrobeHierarchyChanged();
		}

		// Token: 0x06003CCF RID: 15567 RVA: 0x001696EF File Offset: 0x001678EF
		private void HandlePhysicalChanged(MCStylePartDefinitionSO def, GameObject go)
		{
			this._presentationItemsDirty = true;
			this.RebuildManagedSlots();
			Action onWardrobeHierarchyChanged = this.OnWardrobeHierarchyChanged;
			if (onWardrobeHierarchyChanged == null)
			{
				return;
			}
			onWardrobeHierarchyChanged();
		}

		// Token: 0x06003CD0 RID: 15568 RVA: 0x0016970E File Offset: 0x0016790E
		private void HandleServiceSlotVisibilityChanged()
		{
			this.RecomputeCoverage(true);
		}

		// Token: 0x06003CD1 RID: 15569 RVA: 0x00169718 File Offset: 0x00167918
		private void RebuildManagedSlots()
		{
			if (!this._serviceBound || this.styleService == null)
			{
				return;
			}
			this._managedOrder.Clear();
			this.AppendWornSlots(this.coveragePartIds);
			this.AppendWornSlots(this.aestheticPartIds);
			List<string> list = new List<string>();
			foreach (string text in this._slotVisible.Keys)
			{
				if (!this._managedOrder.Contains(text))
				{
					list.Add(text);
				}
			}
			foreach (string text2 in list)
			{
				this._slotVisible.Remove(text2);
			}
			foreach (string text3 in this._managedOrder)
			{
				bool flag;
				if (!this._slotVisible.TryGetValue(text3, out flag))
				{
					flag = true;
					this._slotVisible[text3] = true;
				}
				this.ApplySlotVisibility(text3, flag);
			}
			this.RecomputeCoverage(true);
		}

		// Token: 0x06003CD2 RID: 15570 RVA: 0x00169874 File Offset: 0x00167A74
		private void AppendWornSlots(List<string> partIds)
		{
			if (partIds == null)
			{
				return;
			}
			foreach (string text in partIds)
			{
				if (!string.IsNullOrWhiteSpace(text) && !this._managedOrder.Contains(text) && this.styleService.IsSlotEquipped(text))
				{
					this._managedOrder.Add(text);
				}
			}
		}

		// Token: 0x06003CD3 RID: 15571 RVA: 0x001698F0 File Offset: 0x00167AF0
		private void ApplySlotVisibility(string partId, bool visible)
		{
			bool flag = this.styleService != null && this.styleService.SetSlotPhysicallyHidden(partId, !visible);
			if (this.enableDebugLogs)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[ClothingToggle] ",
					partId,
					" -> ",
					visible ? "ON" : "OFF",
					" ",
					string.Format("(handled={0})", flag)
				}), this);
			}
			Action<string, bool> onSlotVisibilityChanged = this.OnSlotVisibilityChanged;
			if (onSlotVisibilityChanged == null)
			{
				return;
			}
			onSlotVisibilityChanged(partId, visible);
		}

		// Token: 0x06003CD4 RID: 15572 RVA: 0x0016998B File Offset: 0x00167B8B
		private bool IsCoveragePart(string partId)
		{
			return this.coveragePartIds != null && this.coveragePartIds.Contains(partId);
		}

		// Token: 0x06003CD5 RID: 15573 RVA: 0x001699A4 File Offset: 0x00167BA4
		private void RecomputeCoverage(bool fireEvents)
		{
			bool flag4;
			bool flag5;
			bool flag6;
			if (this._serviceBound)
			{
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				foreach (WearableItemSO wearableItemSO in this.ModestyItems)
				{
					if (this.IsWearableWornAndVisible(wearableItemSO))
					{
						if (wearableItemSO.forcesNaked)
						{
							flag3 = true;
							break;
						}
						flag |= wearableItemSO.coversTopDecently;
						flag2 |= wearableItemSO.coversBottomDecently;
					}
				}
				flag4 = !flag3 && flag;
				flag5 = !flag3 && flag2;
				flag6 = flag4 && flag5;
			}
			else
			{
				flag6 = this.clothingGroups.TrueForAll((ClothingGroup g) => g.isActive);
				flag4 = flag6;
				flag5 = flag6;
			}
			bool flag7;
			bool flag8;
			this.ComputePhysicalCoverage(out flag7, out flag8);
			float num = (this._serviceBound ? this.ComputeDetectionCoverage() : (flag6 ? 10f : 0f));
			this._rawDetectionCoverage = num;
			num = ClothingToggleController.ApplyPhysicalCoverageCap(Mathf.Min(10f, num), flag7, flag8);
			this.SetCoverageState(flag6, flag4, flag5, flag7, flag8, num, fireEvents);
		}

		// Token: 0x06003CD6 RID: 15574 RVA: 0x00169AD8 File Offset: 0x00167CD8
		private float ComputeDetectionCoverage()
		{
			float num = 0f;
			foreach (WearableItemSO wearableItemSO in this.DetectionItems)
			{
				if (this.IsWearableWornAndVisible(wearableItemSO))
				{
					num += wearableItemSO.ResolveDetectionCoverage(this.ResolveActiveSurface(wearableItemSO));
				}
			}
			return num;
		}

		// Token: 0x06003CD7 RID: 15575 RVA: 0x00169B44 File Offset: 0x00167D44
		public static float ApplyPhysicalCoverageCap(float coverage, bool topPhysicallyCovered, bool bottomPhysicallyCovered)
		{
			if (topPhysicallyCovered && bottomPhysicallyCovered)
			{
				return coverage;
			}
			return Mathf.Min(9f, coverage);
		}

		// Token: 0x06003CD8 RID: 15576 RVA: 0x00169B5C File Offset: 0x00167D5C
		private void ComputePhysicalCoverage(out bool topPhysical, out bool bottomPhysical)
		{
			topPhysical = false;
			bottomPhysical = false;
			if (this.styleService == null)
			{
				return;
			}
			this.ResolveOpenControllers();
			bool flag = this._fullFlash != null && this._fullFlash.IsOpen;
			bool isWornBraOpen = this.IsWornBraOpen;
			bool flag2 = this._lingerie != null && this._lingerie.IsPantiesOpen && this.IsWearingVisiblePreset("Outfit.Underwear", "expose_panties");
			foreach (WearableItemSO wearableItemSO in this.PhysicalItems)
			{
				if (this.IsWearableWornAndVisible(wearableItemSO))
				{
					string presetId = wearableItemSO.mcStylePreset.presetId;
					if ((!isWornBraOpen || !(presetId == "expose_bra")) && (!flag2 || !(presetId == "expose_panties")) && (!flag || !(presetId == "towel_bath")))
					{
						if (wearableItemSO.physicallyCoversTop)
						{
							topPhysical = true;
						}
						if (wearableItemSO.physicallyCoversBottom)
						{
							bottomPhysical = true;
						}
					}
				}
			}
		}

		// Token: 0x06003CD9 RID: 15577 RVA: 0x00169C7C File Offset: 0x00167E7C
		public void GetCurrentPhysicalCoverage(out bool topPhysical, out bool bottomPhysical)
		{
			this.ComputePhysicalCoverage(out topPhysical, out bottomPhysical);
		}

		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x06003CDA RID: 15578 RVA: 0x00169C88 File Offset: 0x00167E88
		public bool IsMouthPhysicallyBlocked
		{
			get
			{
				foreach (WearableItemSO wearableItemSO in this.DetectionItems)
				{
					if (wearableItemSO.BlocksMouthWhileWorn(this) && this.IsWearableWornAndVisible(wearableItemSO))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x06003CDB RID: 15579 RVA: 0x00169CF0 File Offset: 0x00167EF0
		public bool IsBottomPhysicallyCoveredNow
		{
			get
			{
				if (this.styleService == null)
				{
					return this._bottomPhysicallyCovered;
				}
				bool flag;
				bool flag2;
				this.ComputePhysicalCoverage(out flag, out flag2);
				return flag2;
			}
		}

		// Token: 0x17000A4A RID: 2634
		// (get) Token: 0x06003CDC RID: 15580 RVA: 0x00169D1D File Offset: 0x00167F1D
		public bool IsWornBraOpen
		{
			get
			{
				this.ResolveOpenControllers();
				return this._lingerie != null && this._lingerie.IsBraOpen && this.IsWearingVisiblePreset("Outfit.Top", "expose_bra");
			}
		}

		// Token: 0x17000A4B RID: 2635
		// (get) Token: 0x06003CDD RID: 15581 RVA: 0x00169D54 File Offset: 0x00167F54
		public bool HasVisibleUpperGarment
		{
			get
			{
				if (this.styleService == null)
				{
					return true;
				}
				foreach (WearableItemSO wearableItemSO in this.DetectionItems)
				{
					if ((wearableItemSO.physicallyCoversTop || wearableItemSO.mcStylePartId == "Outfit.Top") && this.IsWearableWornAndVisible(wearableItemSO))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x06003CDE RID: 15582 RVA: 0x00169DDC File Offset: 0x00167FDC
		private List<WearableItemSO> ModestyItems
		{
			get
			{
				if (this._modestyItems == null && this.CanUsePresentationIndex)
				{
					return this.GetPresentationItems();
				}
				if (this._modestyItems == null)
				{
					this._modestyItems = new List<WearableItemSO>();
					foreach (WearableItemSO wearableItemSO in Resources.LoadAll<WearableItemSO>(string.IsNullOrWhiteSpace(this.wardrobeResourcesPath) ? "Wardrobe" : this.wardrobeResourcesPath))
					{
						if (!(wearableItemSO == null) && wearableItemSO.UsesMCStyle && (wearableItemSO.coversTopDecently || wearableItemSO.coversBottomDecently || wearableItemSO.forcesNaked))
						{
							this._modestyItems.Add(wearableItemSO);
						}
					}
				}
				return this._modestyItems;
			}
		}

		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x06003CDF RID: 15583 RVA: 0x00169E80 File Offset: 0x00168080
		private List<WearableItemSO> DetectionItems
		{
			get
			{
				if (this._detectionItems == null && this.CanUsePresentationIndex)
				{
					return this.GetPresentationItems();
				}
				if (this._detectionItems == null)
				{
					this._detectionItems = new List<WearableItemSO>();
					foreach (WearableItemSO wearableItemSO in Resources.LoadAll<WearableItemSO>(string.IsNullOrWhiteSpace(this.wardrobeResourcesPath) ? "Wardrobe" : this.wardrobeResourcesPath))
					{
						if (wearableItemSO != null && wearableItemSO.UsesMCStyle)
						{
							this._detectionItems.Add(wearableItemSO);
						}
					}
				}
				return this._detectionItems;
			}
		}

		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x06003CE0 RID: 15584 RVA: 0x00169F0C File Offset: 0x0016810C
		private List<WearableItemSO> PhysicalItems
		{
			get
			{
				if (this._physicalItems == null && this.CanUsePresentationIndex)
				{
					return this.GetPresentationItems();
				}
				if (this._physicalItems == null)
				{
					this._physicalItems = new List<WearableItemSO>();
					foreach (WearableItemSO wearableItemSO in Resources.LoadAll<WearableItemSO>(string.IsNullOrWhiteSpace(this.wardrobeResourcesPath) ? "Wardrobe" : this.wardrobeResourcesPath))
					{
						if (!(wearableItemSO == null) && wearableItemSO.UsesMCStyle && (wearableItemSO.physicallyCoversTop || wearableItemSO.physicallyCoversBottom))
						{
							this._physicalItems.Add(wearableItemSO);
						}
					}
				}
				return this._physicalItems;
			}
		}

		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x06003CE1 RID: 15585 RVA: 0x00169FA8 File Offset: 0x001681A8
		private bool CanUsePresentationIndex
		{
			get
			{
				return (string.IsNullOrWhiteSpace(this.wardrobeResourcesPath) || string.Equals(this.wardrobeResourcesPath.Trim('/'), "Wardrobe", StringComparison.Ordinal)) && WardrobeGameplayContext.IsPresentationOnly(this);
			}
		}

		// Token: 0x06003CE2 RID: 15586 RVA: 0x00169FDC File Offset: 0x001681DC
		private List<WearableItemSO> GetPresentationItems()
		{
			if (!this._presentationItemsDirty && this._presentationItemsService == this.styleService)
			{
				return this._presentationItems;
			}
			this._presentationItems.Clear();
			this._presentationItemsService = this.styleService;
			this._presentationItemsDirty = false;
			if (this.styleService == null)
			{
				return this._presentationItems;
			}
			WardrobeMetadataIndex wardrobeMetadataIndex = WardrobeMetadataIndex.Load();
			if (wardrobeMetadataIndex == null)
			{
				return this._presentationItems;
			}
			foreach (WardrobeMetadataIndex.Entry entry in wardrobeMetadataIndex.Entries)
			{
				if (entry != null && entry.usesMCStyle)
				{
					MCStylePresetSO activePreset = this.styleService.GetActivePreset(entry.mcStylePartId);
					if (!(activePreset == null) && string.Equals(activePreset.presetId, entry.presetId, StringComparison.Ordinal))
					{
						WearableItemSO wearableItemSO = entry.LoadItem();
						if (wearableItemSO != null && wearableItemSO.UsesMCStyle)
						{
							this._presentationItems.Add(wearableItemSO);
						}
					}
				}
			}
			return this._presentationItems;
		}

		// Token: 0x06003CE3 RID: 15587 RVA: 0x0016A0F8 File Offset: 0x001682F8
		private void ResolveOpenControllers()
		{
			if (this._fullFlash != null && this._lingerie != null && Time.frameCount < this._nextOpenResolveFrame)
			{
				return;
			}
			this._nextOpenResolveFrame = Time.frameCount + 60;
			Transform transform = ((this.styleService != null) ? this.styleService.transform : base.transform);
			vThirdPersonInput componentInParent = transform.GetComponentInParent<vThirdPersonInput>(true);
			if (componentInParent != null)
			{
				transform = componentInParent.transform;
			}
			if (this._fullFlash == null)
			{
				this._fullFlash = transform.GetComponentInParent<ClothFullFlashController>() ?? transform.GetComponentInChildren<ClothFullFlashController>(true);
			}
			if (this._lingerie == null)
			{
				this._lingerie = transform.GetComponentInParent<ExposeLingerieController>() ?? transform.GetComponentInChildren<ExposeLingerieController>(true);
			}
		}

		// Token: 0x06003CE4 RID: 15588 RVA: 0x0016A1C4 File Offset: 0x001683C4
		private bool IsWearableWornAndVisible(WearableItemSO item)
		{
			if (item == null || this.styleService == null)
			{
				return false;
			}
			MCStylePresetSO activePreset = this.styleService.GetActivePreset(item.mcStylePartId);
			if (activePreset == null)
			{
				return false;
			}
			if (activePreset != item.mcStylePreset && !string.Equals(activePreset.presetId, item.mcStylePreset.presetId, StringComparison.Ordinal))
			{
				return false;
			}
			GameObject spawnedRoot = this.styleService.GetSpawnedRoot(item.mcStylePartId);
			return spawnedRoot != null && spawnedRoot.activeSelf;
		}

		// Token: 0x06003CE5 RID: 15589 RVA: 0x0016A254 File Offset: 0x00168454
		private MCSurfaceVariantSO ResolveActiveSurface(WearableItemSO item)
		{
			if (item == null || this.styleService == null)
			{
				return null;
			}
			MCStylePresetSO activePreset = this.styleService.GetActivePreset(item.mcStylePartId);
			if (activePreset == null)
			{
				return null;
			}
			int slotSurfaceIndex = this.styleService.GetSlotSurfaceIndex(item.mcStylePartId);
			MCSurfaceVariantSO mcsurfaceVariantSO;
			if (!activePreset.TryGetSurfaceVariant(slotSurfaceIndex, out mcsurfaceVariantSO))
			{
				return null;
			}
			return mcsurfaceVariantSO;
		}

		// Token: 0x06003CE6 RID: 15590 RVA: 0x0016A2B8 File Offset: 0x001684B8
		private void SetCoverageState(bool covered, bool topCovered, bool bottomCovered, bool topPhysical, bool bottomPhysical, float detectionCoverage, bool fireEvents)
		{
			bool flag = this.isFullyClothed != covered;
			bool flag2 = this._topCovered != topCovered || this._bottomCovered != bottomCovered || this._topPhysicallyCovered != topPhysical || this._bottomPhysicallyCovered != bottomPhysical;
			bool flag3 = !Mathf.Approximately(this._detectionCoverage, detectionCoverage);
			this.isFullyClothed = covered;
			this._topCovered = topCovered;
			this._bottomCovered = bottomCovered;
			this._topPhysicallyCovered = topPhysical;
			this._bottomPhysicallyCovered = bottomPhysical;
			this._detectionCoverage = detectionCoverage;
			if (fireEvents && flag)
			{
				UnityEvent<bool> onClothingStateChanged = this.OnClothingStateChanged;
				if (onClothingStateChanged != null)
				{
					onClothingStateChanged.Invoke(this.isFullyClothed);
				}
				Action<bool> onCoverageChanged = this.OnCoverageChanged;
				if (onCoverageChanged != null)
				{
					onCoverageChanged(this.isFullyClothed);
				}
			}
			if (fireEvents && flag2)
			{
				Action onGranularCoverageChanged = this.OnGranularCoverageChanged;
				if (onGranularCoverageChanged != null)
				{
					onGranularCoverageChanged();
				}
			}
			if (fireEvents && flag3)
			{
				Action<float> onDetectionCoverageChanged = this.OnDetectionCoverageChanged;
				if (onDetectionCoverageChanged == null)
				{
					return;
				}
				onDetectionCoverageChanged(this._detectionCoverage);
			}
		}

		// Token: 0x06003CE7 RID: 15591 RVA: 0x0016A3A4 File Offset: 0x001685A4
		public bool IsWearingAnything(bool ignoreAccessorySlots = false)
		{
			if (!this._serviceBound)
			{
				return this.isFullyClothed;
			}
			foreach (WearableItemSO wearableItemSO in this.DetectionItems)
			{
				if ((!ignoreAccessorySlots || wearableItemSO.mcStylePartId == null || !wearableItemSO.mcStylePartId.StartsWith("Accessory.", StringComparison.Ordinal)) && this.IsWearableWornAndVisible(wearableItemSO))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003CE8 RID: 15592 RVA: 0x0016A430 File Offset: 0x00168630
		public bool IsWearingVisiblePreset(string partId, string presetId)
		{
			string text;
			if (this.styleService == null || !this.styleService.TryGetEquippedItem(partId, out text) || !string.Equals(text, presetId, StringComparison.Ordinal))
			{
				return false;
			}
			GameObject spawnedRoot = this.styleService.GetSpawnedRoot(partId);
			return spawnedRoot != null && spawnedRoot.activeInHierarchy;
		}

		// Token: 0x06003CE9 RID: 15593 RVA: 0x0016A488 File Offset: 0x00168688
		public bool IsSlotVisible(string partId)
		{
			bool flag;
			return this._slotVisible.TryGetValue(partId, out flag) && flag;
		}

		// Token: 0x06003CEA RID: 15594 RVA: 0x0016A4A5 File Offset: 0x001686A5
		public bool ManagesSlot(string partId)
		{
			return this._managedOrder.Contains(partId);
		}

		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x06003CEB RID: 15595 RVA: 0x0016A4B3 File Offset: 0x001686B3
		public IReadOnlyList<string> ManagedSlots
		{
			get
			{
				return this._managedOrder;
			}
		}

		// Token: 0x06003CEC RID: 15596 RVA: 0x0016A4BB File Offset: 0x001686BB
		public void ResetVisibilityForPreset()
		{
			this._slotVisible.Clear();
			if (this._serviceBound)
			{
				this.RebuildManagedSlots();
				return;
			}
			if (this.styleService == null)
			{
				this.SetAllClothing(true);
			}
		}

		// Token: 0x06003CED RID: 15597 RVA: 0x0016A4EC File Offset: 0x001686EC
		public void SetSlotVisible(string partId, bool visible)
		{
			if (!this._serviceBound || !this._managedOrder.Contains(partId))
			{
				return;
			}
			this._slotVisible[partId] = visible;
			this.ApplySlotVisibility(partId, visible);
			this.RecomputeCoverage(true);
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x0016A521 File Offset: 0x00168721
		public void ToggleSlot(string partId)
		{
			if (!this._serviceBound || !this._managedOrder.Contains(partId))
			{
				return;
			}
			this.SetSlotVisible(partId, !this.IsSlotVisible(partId));
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x0016A54C File Offset: 0x0016874C
		public void ToggleAllClothing()
		{
			if (this._serviceBound)
			{
				bool flag = false;
				foreach (string text in this._managedOrder)
				{
					if (this.IsCoveragePart(text) && this.IsSlotVisible(text))
					{
						flag = true;
						break;
					}
				}
				this.SetAllClothing(!flag);
				return;
			}
			this.isFullyClothed = !this.isFullyClothed;
			foreach (ClothingGroup clothingGroup in this.clothingGroups)
			{
				this.SetGroupState(clothingGroup, this.isFullyClothed);
			}
			this.RecomputeCoverage(true);
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x0016A624 File Offset: 0x00168824
		public void SetAllClothing(bool clothed)
		{
			if (this._serviceBound)
			{
				foreach (string text in this._managedOrder)
				{
					if (this.IsCoveragePart(text))
					{
						this._slotVisible[text] = clothed;
						this.ApplySlotVisibility(text, clothed);
					}
				}
				this.RecomputeCoverage(true);
				return;
			}
			foreach (ClothingGroup clothingGroup in this.clothingGroups)
			{
				this.SetGroupState(clothingGroup, clothed);
			}
			this.RecomputeCoverage(true);
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x0016A6E8 File Offset: 0x001688E8
		public void ToggleEverythingIncludingAesthetics()
		{
			if (!this._serviceBound)
			{
				this.ToggleAllClothing();
				return;
			}
			bool flag = false;
			foreach (string text in this._managedOrder)
			{
				if (this.IsSlotVisible(text))
				{
					flag = true;
					break;
				}
			}
			bool flag2 = !flag;
			foreach (string text2 in this._managedOrder)
			{
				this._slotVisible[text2] = flag2;
				this.ApplySlotVisibility(text2, flag2);
			}
			this.RecomputeCoverage(true);
		}

		// Token: 0x06003CF2 RID: 15602 RVA: 0x0016A7B4 File Offset: 0x001689B4
		public void ToggleGroup(int index)
		{
			if (!this._serviceBound)
			{
				if (index >= 0 && index < this.clothingGroups.Count)
				{
					ClothingGroup clothingGroup = this.clothingGroups[index];
					clothingGroup.isActive = !clothingGroup.isActive;
					this.SetGroupState(clothingGroup, clothingGroup.isActive);
					this.RecomputeCoverage(true);
				}
				return;
			}
			if (index < 0 || index >= this._managedOrder.Count)
			{
				return;
			}
			this.ToggleSlot(this._managedOrder[index]);
		}

		// Token: 0x06003CF3 RID: 15603 RVA: 0x0016A834 File Offset: 0x00168A34
		public void ToggleGroup(string groupName)
		{
			if (this._serviceBound)
			{
				string text = this.ResolvePartId(groupName);
				if (text != null)
				{
					this.ToggleSlot(text);
				}
				return;
			}
			ClothingGroup clothingGroup = this.clothingGroups.Find((ClothingGroup g) => g.groupName == groupName);
			if (clothingGroup != null)
			{
				clothingGroup.isActive = !clothingGroup.isActive;
				this.SetGroupState(clothingGroup, clothingGroup.isActive);
				this.RecomputeCoverage(true);
			}
		}

		// Token: 0x06003CF4 RID: 15604 RVA: 0x0016A8AC File Offset: 0x00168AAC
		public void SetGroupState(string groupName, bool active)
		{
			if (this._serviceBound)
			{
				string text = this.ResolvePartId(groupName);
				if (text != null)
				{
					this.SetSlotVisible(text, active);
				}
				return;
			}
			ClothingGroup clothingGroup = this.clothingGroups.Find((ClothingGroup g) => g.groupName == groupName);
			if (clothingGroup != null)
			{
				this.SetGroupState(clothingGroup, active);
				this.RecomputeCoverage(true);
			}
		}

		// Token: 0x06003CF5 RID: 15605 RVA: 0x0016A914 File Offset: 0x00168B14
		public bool IsGroupActive(string groupName)
		{
			if (this._serviceBound)
			{
				string text = this.ResolvePartId(groupName);
				return text != null && this.IsSlotVisible(text);
			}
			ClothingGroup clothingGroup = this.clothingGroups.Find((ClothingGroup g) => g.groupName == groupName);
			return clothingGroup != null && clothingGroup.isActive;
		}

		// Token: 0x06003CF6 RID: 15606 RVA: 0x0016A974 File Offset: 0x00168B74
		private string ResolvePartId(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return null;
			}
			if (this._managedOrder.Contains(name))
			{
				return name;
			}
			foreach (string text in this._managedOrder)
			{
				MCStylePartDefinitionSO mcstylePartDefinitionSO = ((this.styleService != null) ? this.styleService.GetDefinition(text) : null);
				if (mcstylePartDefinitionSO != null && string.Equals(mcstylePartDefinitionSO.displayName, name, StringComparison.OrdinalIgnoreCase))
				{
					return text;
				}
			}
			return null;
		}

		// Token: 0x06003CF7 RID: 15607 RVA: 0x0016AA18 File Offset: 0x00168C18
		private void Update()
		{
			if (!this._serviceBound)
			{
				return;
			}
			this.ResolveOpenControllers();
			bool flag = this._fullFlash != null && this._fullFlash.IsOpen;
			bool flag2 = this._lingerie != null && this._lingerie.IsBraOpen;
			bool flag3 = this._lingerie != null && this._lingerie.IsPantiesOpen;
			if (flag != this._lastFullFlashOpen || flag2 != this._lastBraOpen || flag3 != this._lastPantiesOpen)
			{
				this._lastFullFlashOpen = flag;
				this._lastBraOpen = flag2;
				this._lastPantiesOpen = flag3;
				this.RecomputeCoverage(true);
			}
		}

		// Token: 0x06003CF8 RID: 15608 RVA: 0x0016AAC2 File Offset: 0x00168CC2
		private void LateUpdate()
		{
			if (this._serviceBound || !this.overrideAnimatorBlendShapes)
			{
				return;
			}
			this.ApplyAllBlendShapes();
		}

		// Token: 0x06003CF9 RID: 15609 RVA: 0x0016AADC File Offset: 0x00168CDC
		private void InitializeBlendShapeCache()
		{
			this._blendShapeIndexCache.Clear();
			foreach (ClothingGroup clothingGroup in this.clothingGroups)
			{
				foreach (BlendshapeSetting blendshapeSetting in clothingGroup.blendshapeSettings)
				{
					if (!(blendshapeSetting.targetRenderer == null) && !(blendshapeSetting.targetRenderer.sharedMesh == null))
					{
						int blendShapeIndex = blendshapeSetting.targetRenderer.sharedMesh.GetBlendShapeIndex(blendshapeSetting.blendshapeName);
						if (blendShapeIndex >= 0)
						{
							this._blendShapeIndexCache[blendshapeSetting] = blendShapeIndex;
						}
					}
				}
			}
			this._cacheInitialized = true;
		}

		// Token: 0x06003CFA RID: 15610 RVA: 0x0016ABC0 File Offset: 0x00168DC0
		private void ApplyAllBlendShapes()
		{
			if (!this._cacheInitialized)
			{
				this.InitializeBlendShapeCache();
			}
			foreach (ClothingGroup clothingGroup in this.clothingGroups)
			{
				foreach (BlendshapeSetting blendshapeSetting in clothingGroup.blendshapeSettings)
				{
					int num;
					if (this._blendShapeIndexCache.TryGetValue(blendshapeSetting, out num) && !(blendshapeSetting.targetRenderer == null))
					{
						float num2 = (this.ResolveEffectiveGroupActive(clothingGroup) ? blendshapeSetting.clothedValue : blendshapeSetting.nakedValue);
						if (BlendShapeManager.Instance != null)
						{
							BlendShapeManager.Instance.SetOverride(blendshapeSetting.blendshapeName, num2, "ClothingToggle", BlendShapePriority.Clothing, false);
						}
						else
						{
							blendshapeSetting.targetRenderer.SetBlendShapeWeight(num, num2);
						}
					}
				}
			}
		}

		// Token: 0x06003CFB RID: 15611 RVA: 0x0016ACCC File Offset: 0x00168ECC
		private bool ResolveEffectiveGroupActive(ClothingGroup group)
		{
			return group != null && (group.isActive || (!this.isFullyClothed && group.keepActiveWhenNaked));
		}

		// Token: 0x06003CFC RID: 15612 RVA: 0x0016ACF0 File Offset: 0x00168EF0
		private void SetGroupState(ClothingGroup group, bool active)
		{
			group.isActive = active;
			bool flag = this.ResolveEffectiveGroupActive(group);
			foreach (GameObject gameObject in group.outfitObjects)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(flag);
				}
			}
			foreach (BlendshapeSetting blendshapeSetting in group.blendshapeSettings)
			{
				if (!(blendshapeSetting.targetRenderer == null))
				{
					Mesh sharedMesh = blendshapeSetting.targetRenderer.sharedMesh;
					if (!(sharedMesh == null))
					{
						float num = (flag ? blendshapeSetting.clothedValue : blendshapeSetting.nakedValue);
						int blendShapeIndex = sharedMesh.GetBlendShapeIndex(blendshapeSetting.blendshapeName);
						if (blendShapeIndex >= 0)
						{
							if (BlendShapeManager.Instance != null)
							{
								BlendShapeManager.Instance.SetOverride(blendshapeSetting.blendshapeName, num, "ClothingToggle", BlendShapePriority.Clothing, false);
							}
							else
							{
								blendshapeSetting.targetRenderer.SetBlendShapeWeight(blendShapeIndex, num);
							}
						}
					}
				}
			}
		}

		// Token: 0x04003CB8 RID: 15544
		[Header("Auto-bind (MCStyle)")]
		[Tooltip("Source of the worn items. If null, found on the player (children) then in scene. When present, this drives the toggle; the legacy clothingGroups list is ignored.")]
		[SerializeField]
		private MCStyleCustomizationService styleService;

		// Token: 0x04003CB9 RID: 15545
		[Tooltip("partIds of the slots that COVER the body. Toggling any of these on/off changes the coverage state (IsCovered / IsFullyClothed). Defaults: Top, Bottom, Mask.")]
		[SerializeField]
		private List<string> coveragePartIds = new List<string> { "Outfit.Top", "Outfit.Bottom", "Outfit.Mask" };

		// Token: 0x04003CBA RID: 15546
		[Tooltip("partIds of the purely AESTHETIC slots. Toggleable, but never affect coverage. Defaults: Shoes, Eyewear (occhiali), Headpin and Cow accessories.")]
		[SerializeField]
		private List<string> aestheticPartIds = new List<string> { "Outfit.Drape", "Outfit.Shoes", "Accessory.Eyewear", "Accessory.Headpin", "Accessory.CowHeadband", "Accessory.CowTail" };

		// Token: 0x04003CBB RID: 15547
		[Tooltip("Resources folder the wardrobe WearableItemSO assets are loaded from (empty = all Resources). Their modesty flags (coversTopDecently/coversBottomDecently/forcesNaked) drive the clothed/naked state.")]
		[SerializeField]
		private string wardrobeResourcesPath = "";

		// Token: 0x04003CBC RID: 15548
		[Header("Legacy Clothing Groups (fallback only)")]
		[Tooltip("Used ONLY when no MCStyleCustomizationService is found. Hand-wired outfit GameObjects + blendshapes, as in the original controller.")]
		public List<ClothingGroup> clothingGroups = new List<ClothingGroup>();

		// Token: 0x04003CBD RID: 15549
		[Header("Current State")]
		[SerializeField]
		private bool isFullyClothed = true;

		// Token: 0x04003CBE RID: 15550
		[Header("Events")]
		[Tooltip("Fired when the COVERAGE state changes. Parameter: isCovered. Kept under the original name for back-compat (consumers treat it as 'is the player covered').")]
		public UnityEvent<bool> OnClothingStateChanged;

		// Token: 0x04003CC2 RID: 15554
		[Header("Animator Override (legacy path)")]
		[Tooltip("Re-apply legacy blend shapes in LateUpdate to override Animator-controlled values. Only used on the legacy (no-service) path.")]
		[SerializeField]
		private bool overrideAnimatorBlendShapes = true;

		// Token: 0x04003CC3 RID: 15555
		[Header("Debug")]
		[SerializeField]
		private bool enableDebugLogs;

		// Token: 0x04003CC4 RID: 15556
		private readonly Dictionary<string, bool> _slotVisible = new Dictionary<string, bool>();

		// Token: 0x04003CC5 RID: 15557
		private readonly List<string> _managedOrder = new List<string>();

		// Token: 0x04003CC6 RID: 15558
		private bool _serviceBound;

		// Token: 0x04003CC7 RID: 15559
		private List<WearableItemSO> _modestyItems;

		// Token: 0x04003CC8 RID: 15560
		private readonly List<WearableItemSO> _presentationItems = new List<WearableItemSO>(8);

		// Token: 0x04003CC9 RID: 15561
		private bool _presentationItemsDirty = true;

		// Token: 0x04003CCA RID: 15562
		private MCStyleCustomizationService _presentationItemsService;

		// Token: 0x04003CCB RID: 15563
		private Dictionary<BlendshapeSetting, int> _blendShapeIndexCache = new Dictionary<BlendshapeSetting, int>();

		// Token: 0x04003CCC RID: 15564
		private bool _cacheInitialized;

		// Token: 0x04003CCD RID: 15565
		private bool _topCovered;

		// Token: 0x04003CCE RID: 15566
		private bool _bottomCovered;

		// Token: 0x04003CD0 RID: 15568
		private float _detectionCoverage;

		// Token: 0x04003CD1 RID: 15569
		private float _rawDetectionCoverage;

		// Token: 0x04003CD3 RID: 15571
		private bool _topPhysicallyCovered;

		// Token: 0x04003CD4 RID: 15572
		private bool _bottomPhysicallyCovered;

		// Token: 0x04003CD5 RID: 15573
		private List<WearableItemSO> _physicalItems;

		// Token: 0x04003CD6 RID: 15574
		private List<WearableItemSO> _detectionItems;

		// Token: 0x04003CD7 RID: 15575
		private ClothFullFlashController _fullFlash;

		// Token: 0x04003CD8 RID: 15576
		private ExposeLingerieController _lingerie;

		// Token: 0x04003CD9 RID: 15577
		private int _nextOpenResolveFrame;

		// Token: 0x04003CDA RID: 15578
		private bool _lastFullFlashOpen;

		// Token: 0x04003CDB RID: 15579
		private bool _lastBraOpen;

		// Token: 0x04003CDC RID: 15580
		private bool _lastPantiesOpen;
	}
}
