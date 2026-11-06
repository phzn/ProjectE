using System;
using UnityEngine.Localization.Metadata;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization.Tables
{
	// Token: 0x02000027 RID: 39
	public class AssetTableEntry : TableEntry
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00004BC3 File Offset: 0x00002DC3
		// (set) Token: 0x06000115 RID: 277 RVA: 0x00004BCB File Offset: 0x00002DCB
		internal AsyncOperationHandle<Object[]> PreloadAsyncOperation { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00004BD4 File Offset: 0x00002DD4
		// (set) Token: 0x06000117 RID: 279 RVA: 0x00004BDC File Offset: 0x00002DDC
		internal AsyncOperationHandle AsyncOperation { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00004BE5 File Offset: 0x00002DE5
		// (set) Token: 0x06000119 RID: 281 RVA: 0x00004BF2 File Offset: 0x00002DF2
		public string Address
		{
			get
			{
				return base.Data.Localized;
			}
			set
			{
				base.Data.Localized = value;
				this.m_GuidCache = null;
				this.m_SubAssetNameCache = null;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00004C0E File Offset: 0x00002E0E
		// (set) Token: 0x0600011B RID: 283 RVA: 0x00004C2F File Offset: 0x00002E2F
		public string Guid
		{
			get
			{
				if (this.m_GuidCache == null)
				{
					this.m_GuidCache = AssetAddress.GetGuid(this.Address);
				}
				return this.m_GuidCache;
			}
			set
			{
				this.Address = value;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00004C38 File Offset: 0x00002E38
		public string SubAssetName
		{
			get
			{
				if (this.m_SubAssetNameCache == null)
				{
					this.m_SubAssetNameCache = AssetAddress.GetSubAssetName(this.Address);
				}
				return this.m_SubAssetNameCache;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00004C59 File Offset: 0x00002E59
		public bool IsEmpty
		{
			get
			{
				return string.IsNullOrEmpty(this.Address);
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00004C66 File Offset: 0x00002E66
		public bool IsSubAsset
		{
			get
			{
				return AssetAddress.IsSubAsset(this.Address);
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00004C73 File Offset: 0x00002E73
		internal AssetTableEntry()
		{
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00004C7C File Offset: 0x00002E7C
		public void RemoveFromTable()
		{
			AssetTable assetTable = base.Table as AssetTable;
			if (assetTable == null)
			{
				Debug.LogWarning(string.Format("Failed to remove {0} with id {1} and address `{2}` as it does not belong to a table.", "AssetTableEntry", base.KeyId, this.Address));
				return;
			}
			assetTable.Remove(base.KeyId);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00004CD4 File Offset: 0x00002ED4
		internal Type GetExpectedType()
		{
			foreach (IMetadata metadata in base.Table.SharedData.Metadata.MetadataEntries)
			{
				AssetTypeMetadata assetTypeMetadata = metadata as AssetTypeMetadata;
				if (assetTypeMetadata != null && assetTypeMetadata.Contains(base.KeyId))
				{
					return assetTypeMetadata.Type;
				}
			}
			return typeof(Object);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00004D54 File Offset: 0x00002F54
		public void SetAssetOverride<T>(T asset) where T : Object
		{
			AddressablesInterface.SafeRelease(this.AsyncOperation);
			this.AsyncOperation = AddressablesInterface.ResourceManager.CreateCompletedOperation<T>(asset, null);
		}

		// Token: 0x04000044 RID: 68
		private string m_GuidCache;

		// Token: 0x04000045 RID: 69
		private string m_SubAssetNameCache;
	}
}
