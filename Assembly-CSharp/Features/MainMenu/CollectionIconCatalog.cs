using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.MainMenu
{
	// Token: 0x0200102C RID: 4140
	public sealed class CollectionIconCatalog : ScriptableObject
	{
		// Token: 0x06007D7A RID: 32122 RVA: 0x003368C8 File Offset: 0x00334AC8
		public Sprite ResolveIcon(string assetGuid, int variantIndex)
		{
			if (string.IsNullOrEmpty(assetGuid))
			{
				return null;
			}
			if (this._byGuid == null)
			{
				this._byGuid = new Dictionary<string, CollectionIconCatalog.Entry>(StringComparer.Ordinal);
				foreach (CollectionIconCatalog.Entry entry in this.entries)
				{
					if (entry != null && !string.IsNullOrEmpty(entry.assetGuid))
					{
						this._byGuid[entry.assetGuid] = entry;
					}
				}
			}
			CollectionIconCatalog.Entry entry2;
			if (!this._byGuid.TryGetValue(assetGuid, out entry2))
			{
				return null;
			}
			if (variantIndex < 0 || entry2.surfaceIcons == null || variantIndex >= entry2.surfaceIcons.Length || !(entry2.surfaceIcons[variantIndex] != null))
			{
				return entry2.icon;
			}
			return entry2.surfaceIcons[variantIndex];
		}

		// Token: 0x04008157 RID: 33111
		public const string ResourcePath = "CollectionIconCatalog";

		// Token: 0x04008158 RID: 33112
		[SerializeField]
		private CollectionIconCatalog.Entry[] entries = Array.Empty<CollectionIconCatalog.Entry>();

		// Token: 0x04008159 RID: 33113
		private Dictionary<string, CollectionIconCatalog.Entry> _byGuid;

		// Token: 0x0200102D RID: 4141
		[Serializable]
		public sealed class Entry
		{
			// Token: 0x0400815A RID: 33114
			public string assetGuid;

			// Token: 0x0400815B RID: 33115
			public Sprite icon;

			// Token: 0x0400815C RID: 33116
			public Sprite[] surfaceIcons = Array.Empty<Sprite>();
		}
	}
}
