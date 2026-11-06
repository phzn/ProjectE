using System;
using System.Collections.Generic;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.PlayerEmotes;
using UnityEngine;

namespace Features.Achievements
{
	// Token: 0x02001532 RID: 5426
	public sealed class AchievementCollectionCatalog : ScriptableObject
	{
		// Token: 0x170019DC RID: 6620
		// (get) Token: 0x0600A80C RID: 43020 RVA: 0x00462D71 File Offset: 0x00460F71
		public IReadOnlyList<string> ClothingTargetGuids
		{
			get
			{
				return this.clothingTargetGuids ?? Array.Empty<string>();
			}
		}

		// Token: 0x170019DD RID: 6621
		// (get) Token: 0x0600A80D RID: 43021 RVA: 0x00462D82 File Offset: 0x00460F82
		public IReadOnlyList<WardrobeMetadataIndex.Entry> ClothingMetadata
		{
			get
			{
				return this.clothingMetadata ?? Array.Empty<WardrobeMetadataIndex.Entry>();
			}
		}

		// Token: 0x170019DE RID: 6622
		// (get) Token: 0x0600A80E RID: 43022 RVA: 0x00462D94 File Offset: 0x00460F94
		public bool HasCurrentMetadata
		{
			get
			{
				return this.metadataVersion == 1 && (this.clothes == null || this.clothes.Length == 0) && this.ClothingMetadata.Count == this.ClothingTargetGuids.Count && this.ClothingTargetGuids.Count > 0;
			}
		}

		// Token: 0x0400ACC0 RID: 44224
		public const int CurrentMetadataVersion = 1;

		// Token: 0x0400ACC1 RID: 44225
		[SerializeField]
		[HideInInspector]
		private InventoryItemSO[] clothes;

		// Token: 0x0400ACC2 RID: 44226
		[SerializeField]
		private string[] clothingTargetGuids = Array.Empty<string>();

		// Token: 0x0400ACC3 RID: 44227
		[SerializeField]
		private WardrobeMetadataIndex.Entry[] clothingMetadata = Array.Empty<WardrobeMetadataIndex.Entry>();

		// Token: 0x0400ACC4 RID: 44228
		[SerializeField]
		[HideInInspector]
		private int metadataVersion;

		// Token: 0x0400ACC5 RID: 44229
		public PoiEmoteProfileSO[] pointsOfInterest;
	}
}
