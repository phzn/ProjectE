using System;
using System.Collections.Generic;
using BoZo.CharacterCreator.MC.StyleSystem;
using UnityEngine;

namespace Features.Inventory.Wearable
{
	// Token: 0x0200111F RID: 4383
	public sealed class WardrobeMetadataIndex : ScriptableObject
	{
		// Token: 0x1700150D RID: 5389
		// (get) Token: 0x060084DF RID: 34015 RVA: 0x003633B7 File Offset: 0x003615B7
		public IReadOnlyList<WardrobeMetadataIndex.Entry> Entries
		{
			get
			{
				return this.entries ?? Array.Empty<WardrobeMetadataIndex.Entry>();
			}
		}

		// Token: 0x1700150E RID: 5390
		// (get) Token: 0x060084E0 RID: 34016 RVA: 0x003633C8 File Offset: 0x003615C8
		public int SchemaVersion
		{
			get
			{
				return this.schemaVersion;
			}
		}

		// Token: 0x060084E1 RID: 34017 RVA: 0x003633D0 File Offset: 0x003615D0
		public static WardrobeMetadataIndex Load()
		{
			if (WardrobeMetadataIndex._loadAttempted)
			{
				return WardrobeMetadataIndex._cached;
			}
			WardrobeMetadataIndex._loadAttempted = true;
			WardrobeMetadataIndex._cached = Resources.Load<WardrobeMetadataIndex>("WardrobeMetadataIndex");
			if (WardrobeMetadataIndex._cached == null || WardrobeMetadataIndex._cached.schemaVersion != 1)
			{
				WardrobeMetadataIndex._cached = null;
				Debug.LogError("[WardrobeMetadataIndex] Missing or incompatible generated index. Rebuild the wardrobe metadata before Play/build.");
			}
			return WardrobeMetadataIndex._cached;
		}

		// Token: 0x060084E2 RID: 34018 RVA: 0x0036342E File Offset: 0x0036162E
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		public static void ResetRuntimeCache()
		{
			WardrobeMetadataIndex._cached = null;
			WardrobeMetadataIndex._loadAttempted = false;
		}

		// Token: 0x060084E3 RID: 34019 RVA: 0x0036343C File Offset: 0x0036163C
		public IReadOnlyList<WardrobeMetadataIndex.Entry> GetPresetCandidates(MCStylePresetSO preset)
		{
			if (preset == null)
			{
				return Array.Empty<WardrobeMetadataIndex.Entry>();
			}
			if (this._entriesByPresetId == null)
			{
				this._entriesByPresetId = new Dictionary<string, List<WardrobeMetadataIndex.Entry>>(StringComparer.Ordinal);
				foreach (WardrobeMetadataIndex.Entry entry in this.Entries)
				{
					if (entry != null && entry.hasPreset)
					{
						string text = entry.presetId ?? string.Empty;
						List<WardrobeMetadataIndex.Entry> list;
						if (!this._entriesByPresetId.TryGetValue(text, out list))
						{
							this._entriesByPresetId.Add(text, list = new List<WardrobeMetadataIndex.Entry>());
						}
						list.Add(entry);
					}
				}
			}
			List<WardrobeMetadataIndex.Entry> list2;
			if (!this._entriesByPresetId.TryGetValue(preset.presetId ?? string.Empty, out list2))
			{
				return Array.Empty<WardrobeMetadataIndex.Entry>();
			}
			return list2;
		}

		// Token: 0x060084E4 RID: 34020 RVA: 0x00363520 File Offset: 0x00361720
		public bool TryLoadMappedWearables(MCStylePresetSO preset, out List<WearableItemSO> result, Func<WardrobeMetadataIndex.Entry, WearableItemSO> loadItem = null)
		{
			result = new List<WearableItemSO>();
			bool flag = true;
			foreach (WardrobeMetadataIndex.Entry entry in this.GetPresetCandidates(preset))
			{
				WearableItemSO wearableItemSO = ((loadItem != null) ? loadItem(entry) : entry.LoadItem());
				if (wearableItemSO == null)
				{
					flag = false;
				}
				else if (wearableItemSO.mcStylePreset == preset)
				{
					result.Add(wearableItemSO);
				}
			}
			return flag;
		}

		// Token: 0x04008789 RID: 34697
		public const string ResourcePath = "WardrobeMetadataIndex";

		// Token: 0x0400878A RID: 34698
		public const int CurrentSchemaVersion = 1;

		// Token: 0x0400878B RID: 34699
		[SerializeField]
		private int schemaVersion = 1;

		// Token: 0x0400878C RID: 34700
		[SerializeField]
		private WardrobeMetadataIndex.Entry[] entries = Array.Empty<WardrobeMetadataIndex.Entry>();

		// Token: 0x0400878D RID: 34701
		[NonSerialized]
		private Dictionary<string, List<WardrobeMetadataIndex.Entry>> _entriesByPresetId;

		// Token: 0x0400878E RID: 34702
		private static WardrobeMetadataIndex _cached;

		// Token: 0x0400878F RID: 34703
		private static bool _loadAttempted;

		// Token: 0x02001120 RID: 4384
		[Serializable]
		public sealed class SurfaceEntry
		{
			// Token: 0x04008790 RID: 34704
			public int index;

			// Token: 0x04008791 RID: 34705
			public bool exists;

			// Token: 0x04008792 RID: 34706
			public string variantId;

			// Token: 0x04008793 RID: 34707
			public string assetName;

			// Token: 0x04008794 RID: 34708
			public string displayName;

			// Token: 0x04008795 RID: 34709
			public bool showInGameplayInventory;

			// Token: 0x04008796 RID: 34710
			public float detectionCoverage;
		}

		// Token: 0x02001121 RID: 4385
		[Serializable]
		public sealed class RarityEntry
		{
			// Token: 0x04008797 RID: 34711
			public bool exists;

			// Token: 0x04008798 RID: 34712
			public ItemRarity tier;

			// Token: 0x04008799 RID: 34713
			public int surfaceIndex;

			// Token: 0x0400879A RID: 34714
			public string surfaceName;
		}

		// Token: 0x02001122 RID: 4386
		[Serializable]
		public sealed class Entry
		{
			// Token: 0x060084E8 RID: 34024 RVA: 0x003635C4 File Offset: 0x003617C4
			public WearableItemSO LoadItem()
			{
				if (!this.isWearable || string.IsNullOrEmpty(this.resourcePath))
				{
					return null;
				}
				WearableItemSO wearableItemSO = Resources.Load<WearableItemSO>(this.resourcePath);
				if (!(wearableItemSO != null) || !string.Equals(wearableItemSO.itemId, this.itemId, StringComparison.Ordinal))
				{
					return null;
				}
				return wearableItemSO;
			}

			// Token: 0x060084E9 RID: 34025 RVA: 0x00363614 File Offset: 0x00361814
			public ResourceRequest LoadItemAsync()
			{
				if (!this.isWearable || string.IsNullOrEmpty(this.resourcePath))
				{
					return null;
				}
				return Resources.LoadAsync<WearableItemSO>(this.resourcePath);
			}

			// Token: 0x060084EA RID: 34026 RVA: 0x00363638 File Offset: 0x00361838
			public bool TryGetSurface(int index, out WardrobeMetadataIndex.SurfaceEntry surface)
			{
				surface = ((this.surfaces != null && index >= 0 && index < this.surfaces.Length) ? this.surfaces[index] : null);
				return surface != null && surface.exists;
			}

			// Token: 0x060084EB RID: 34027 RVA: 0x0036366C File Offset: 0x0036186C
			public bool ResolveShowInGameplayInventory(int surfaceIndex)
			{
				if (this.isVaulted)
				{
					return false;
				}
				WardrobeMetadataIndex.SurfaceEntry surfaceEntry;
				if (!this.TryGetSurface(surfaceIndex, out surfaceEntry))
				{
					return this.showInGameplayInventory;
				}
				return surfaceEntry.showInGameplayInventory;
			}

			// Token: 0x060084EC RID: 34028 RVA: 0x0036369C File Offset: 0x0036189C
			public float ResolveDetectionCoverage(int surfaceIndex)
			{
				WardrobeMetadataIndex.SurfaceEntry surfaceEntry;
				if (!this.TryGetSurface(surfaceIndex, out surfaceEntry))
				{
					return this.detectionCoverage;
				}
				return surfaceEntry.detectionCoverage;
			}

			// Token: 0x060084ED RID: 34029 RVA: 0x003636C4 File Offset: 0x003618C4
			public int PickVariantForTier(ItemRarity tier)
			{
				if (this.rarityVariants == null || this.rarityVariants.Length == 0)
				{
					return -1;
				}
				int num = 0;
				foreach (WardrobeMetadataIndex.RarityEntry rarityEntry in this.rarityVariants)
				{
					if (rarityEntry != null && rarityEntry.exists && rarityEntry.tier == tier)
					{
						num++;
					}
				}
				if (num == 0)
				{
					return 0;
				}
				int num2 = Random.Range(0, num);
				foreach (WardrobeMetadataIndex.RarityEntry rarityEntry2 in this.rarityVariants)
				{
					if (rarityEntry2 != null && rarityEntry2.exists && rarityEntry2.tier == tier && num2-- == 0)
					{
						return rarityEntry2.surfaceIndex;
					}
				}
				return 0;
			}

			// Token: 0x0400879B RID: 34715
			public string resourcePath;

			// Token: 0x0400879C RID: 34716
			public string assetGuid;

			// Token: 0x0400879D RID: 34717
			public string assetName;

			// Token: 0x0400879E RID: 34718
			public string itemId;

			// Token: 0x0400879F RID: 34719
			public string itemName;

			// Token: 0x040087A0 RID: 34720
			public InventoryCategory category;

			// Token: 0x040087A1 RID: 34721
			public ItemRarity rarity;

			// Token: 0x040087A2 RID: 34722
			public ItemRarity maxRollableRarity;

			// Token: 0x040087A3 RID: 34723
			public bool isVaulted;

			// Token: 0x040087A4 RID: 34724
			public bool sensitiveVisual;

			// Token: 0x040087A5 RID: 34725
			public bool isSensitiveVisual;

			// Token: 0x040087A6 RID: 34726
			public bool isWearable;

			// Token: 0x040087A7 RID: 34727
			public bool hasPreset;

			// Token: 0x040087A8 RID: 34728
			public bool usesMCStyle;

			// Token: 0x040087A9 RID: 34729
			public string mcStylePartId;

			// Token: 0x040087AA RID: 34730
			public string presetId;

			// Token: 0x040087AB RID: 34731
			public string presetName;

			// Token: 0x040087AC RID: 34732
			public string presetAssetGuid;

			// Token: 0x040087AD RID: 34733
			public bool presetIsVaulted;

			// Token: 0x040087AE RID: 34734
			public bool presetGameplayOnly;

			// Token: 0x040087AF RID: 34735
			public bool ignoreOtherSlotConflicts;

			// Token: 0x040087B0 RID: 34736
			public int defaultSurfaceIndex = -1;

			// Token: 0x040087B1 RID: 34737
			public int surfaceCount;

			// Token: 0x040087B2 RID: 34738
			public bool showInGameplayInventory;

			// Token: 0x040087B3 RID: 34739
			public bool hasGameplaySurfaceOverride;

			// Token: 0x040087B4 RID: 34740
			public bool coversTopDecently;

			// Token: 0x040087B5 RID: 34741
			public bool coversBottomDecently;

			// Token: 0x040087B6 RID: 34742
			public bool forcesNaked;

			// Token: 0x040087B7 RID: 34743
			public bool physicallyCoversTop;

			// Token: 0x040087B8 RID: 34744
			public bool physicallyCoversBottom;

			// Token: 0x040087B9 RID: 34745
			public bool physicallyBlocksMouth;

			// Token: 0x040087BA RID: 34746
			public bool containsSkirt;

			// Token: 0x040087BB RID: 34747
			public float detectionCoverage;

			// Token: 0x040087BC RID: 34748
			public WardrobeMetadataIndex.SurfaceEntry[] surfaces = Array.Empty<WardrobeMetadataIndex.SurfaceEntry>();

			// Token: 0x040087BD RID: 34749
			public WardrobeMetadataIndex.RarityEntry[] rarityVariants = Array.Empty<WardrobeMetadataIndex.RarityEntry>();

			// Token: 0x040087BE RID: 34750
			public string[] rankIds = Array.Empty<string>();
		}
	}
}
