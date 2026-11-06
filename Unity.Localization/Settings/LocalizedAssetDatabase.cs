using System;
using UnityEngine.Localization.Operations;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization.Settings
{
	// Token: 0x02000095 RID: 149
	[Serializable]
	public class LocalizedAssetDatabase : LocalizedDatabase<AssetTable, AssetTableEntry>
	{
		// Token: 0x06000402 RID: 1026 RVA: 0x0000E793 File Offset: 0x0000C993
		public AsyncOperationHandle<TObject> GetLocalizedAssetAsync<TObject>(TableEntryReference tableEntryReference, Locale locale = null, FallbackBehavior fallbackBehavior = FallbackBehavior.UseProjectSettings) where TObject : Object
		{
			return this.GetLocalizedAssetAsync<TObject>(base.GetDefaultTable(), tableEntryReference, locale, fallbackBehavior);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0000E7A4 File Offset: 0x0000C9A4
		public TObject GetLocalizedAsset<TObject>(TableEntryReference tableEntryReference, Locale locale = null) where TObject : Object
		{
			return AsyncOperationUtility.SynchronousLoad<TObject>(this.GetLocalizedAssetAsync<TObject>(base.GetDefaultTable(), tableEntryReference, locale, FallbackBehavior.UseProjectSettings));
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0000E7BA File Offset: 0x0000C9BA
		public virtual AsyncOperationHandle<TObject> GetLocalizedAssetAsync<TObject>(TableReference tableReference, TableEntryReference tableEntryReference, Locale locale = null, FallbackBehavior fallbackBehavior = FallbackBehavior.UseProjectSettings) where TObject : Object
		{
			return this.GetLocalizedAssetAsyncInternal<TObject>(tableReference, tableEntryReference, locale, fallbackBehavior);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0000E7C7 File Offset: 0x0000C9C7
		public virtual TObject GetLocalizedAsset<TObject>(TableReference tableReference, TableEntryReference tableEntryReference, Locale locale = null) where TObject : Object
		{
			return AsyncOperationUtility.SynchronousLoad<TObject>(this.GetLocalizedAssetAsyncInternal<TObject>(tableReference, tableEntryReference, locale, FallbackBehavior.UseProjectSettings));
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x0000E7D8 File Offset: 0x0000C9D8
		protected virtual AsyncOperationHandle<TObject> GetLocalizedAssetAsyncInternal<TObject>(TableReference tableReference, TableEntryReference tableEntryReference, Locale locale, FallbackBehavior fallbackBehavior = FallbackBehavior.UseProjectSettings) where TObject : Object
		{
			AsyncOperationHandle<LocalizedDatabase<AssetTable, AssetTableEntry>.TableEntryResult> tableEntryAsync = this.GetTableEntryAsync(tableReference, tableEntryReference, locale, fallbackBehavior);
			LoadAssetOperation<TObject> loadAssetOperation = LoadAssetOperation<TObject>.Pool.Get();
			loadAssetOperation.Init(tableEntryAsync, true);
			loadAssetOperation.Dependency = tableEntryAsync;
			return AddressablesInterface.ResourceManager.StartOperation<TObject>(loadAssetOperation, tableEntryAsync);
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x0000E821 File Offset: 0x0000CA21
		internal override void ReleaseTableContents(AssetTable table)
		{
			table.ReleaseAssets();
		}
	}
}
