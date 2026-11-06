using System;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization.Settings
{
	// Token: 0x02000099 RID: 153
	public interface ITableProvider
	{
		// Token: 0x06000409 RID: 1033
		AsyncOperationHandle<TTable> ProvideTableAsync<TTable>(string tableCollectionName, Locale locale) where TTable : LocalizationTable;
	}
}
