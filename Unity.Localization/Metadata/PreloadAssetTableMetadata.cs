using System;

namespace UnityEngine.Localization.Metadata
{
	// Token: 0x020000E4 RID: 228
	[Metadata(AllowedTypes = MetadataType.SharedTableData | MetadataType.AssetTable, MenuItem = "Preload Assets")]
	[Serializable]
	public class PreloadAssetTableMetadata : IMetadata
	{
		// Token: 0x1700015A RID: 346
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x00015195 File Offset: 0x00013395
		// (set) Token: 0x0600061C RID: 1564 RVA: 0x0001519D File Offset: 0x0001339D
		public PreloadAssetTableMetadata.PreloadBehaviour Behaviour
		{
			get
			{
				return this.m_PreloadBehaviour;
			}
			set
			{
				this.m_PreloadBehaviour = value;
			}
		}

		// Token: 0x0400021A RID: 538
		[SerializeField]
		private PreloadAssetTableMetadata.PreloadBehaviour m_PreloadBehaviour;

		// Token: 0x0200015F RID: 351
		public enum PreloadBehaviour
		{
			// Token: 0x04000317 RID: 791
			NoPreload,
			// Token: 0x04000318 RID: 792
			PreloadAll
		}
	}
}
