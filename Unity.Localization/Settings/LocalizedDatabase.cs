using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Localization.Operations;
using UnityEngine.Localization.Tables;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization.Settings
{
	// Token: 0x0200009B RID: 155
	[Serializable]
	public abstract class LocalizedDatabase<TTable, TEntry> : IPreloadRequired, IReset, IDisposable where TTable : DetailedLocalizationTable<TEntry> where TEntry : TableEntry
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x0000E834 File Offset: 0x0000CA34
		public AsyncOperationHandle PreloadOperation
		{
			get
			{
				if (!this.m_PreloadOperationHandle.IsValid())
				{
					PreloadDatabaseOperation<TTable, TEntry> preloadDatabaseOperation = PreloadDatabaseOperation<TTable, TEntry>.Pool.Get();
					preloadDatabaseOperation.Init(this);
					this.m_PreloadOperationHandle = AddressablesInterface.ResourceManager.StartOperation<LocalizedDatabase<TTable, TEntry>>(preloadDatabaseOperation, default(AsyncOperationHandle));
				}
				return this.m_PreloadOperationHandle;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x0000E885 File Offset: 0x0000CA85
		internal Action<AsyncOperationHandle> ReleaseNextFrame
		{
			get
			{
				return this.m_ReleaseNextFrame;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x0000E88D File Offset: 0x0000CA8D
		[TupleElementNames(new string[] { "localeIdentifier", "tableNameOrGuid" })]
		internal Dictionary<ValueTuple<LocaleIdentifier, string>, AsyncOperationHandle<TTable>> TableOperations
		{
			[return: TupleElementNames(new string[] { "localeIdentifier", "tableNameOrGuid" })]
			get;
		} = new Dictionary<ValueTuple<LocaleIdentifier, string>, AsyncOperationHandle<TTable>>();

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x0000E895 File Offset: 0x0000CA95
		internal Dictionary<Guid, AsyncOperationHandle<SharedTableData>> SharedTableDataOperations { get; } = new Dictionary<Guid, AsyncOperationHandle<SharedTableData>>();

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x0000E89D File Offset: 0x0000CA9D
		// (set) Token: 0x06000410 RID: 1040 RVA: 0x0000E8A5 File Offset: 0x0000CAA5
		public virtual TableReference DefaultTable
		{
			get
			{
				return this.m_DefaultTableReference;
			}
			set
			{
				this.m_DefaultTableReference = value;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x0000E8AE File Offset: 0x0000CAAE
		// (set) Token: 0x06000412 RID: 1042 RVA: 0x0000E8B6 File Offset: 0x0000CAB6
		public ITableProvider TableProvider
		{
			get
			{
				return this.m_CustomTableProvider;
			}
			set
			{
				this.m_CustomTableProvider = value;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x0000E8BF File Offset: 0x0000CABF
		// (set) Token: 0x06000414 RID: 1044 RVA: 0x0000E8C7 File Offset: 0x0000CAC7
		public ITablePostprocessor TablePostprocessor
		{
			get
			{
				return this.m_CustomTablePostprocessor;
			}
			set
			{
				this.m_CustomTablePostprocessor = value;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x0000E8D0 File Offset: 0x0000CAD0
		// (set) Token: 0x06000416 RID: 1046 RVA: 0x0000E8D8 File Offset: 0x0000CAD8
		public bool UseFallback
		{
			get
			{
				return this.m_UseFallback;
			}
			set
			{
				this.m_UseFallback = value;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x0000E8E1 File Offset: 0x0000CAE1
		// (set) Token: 0x06000418 RID: 1048 RVA: 0x0000E8E9 File Offset: 0x0000CAE9
		public AsynchronousBehaviour AsynchronousBehaviour
		{
			get
			{
				return this.m_AsynchronousBehaviour;
			}
			set
			{
				this.m_AsynchronousBehaviour = value;
			}
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x0000E8F4 File Offset: 0x0000CAF4
		public LocalizedDatabase()
		{
			this.m_PatchTableContentsAction = new Action<AsyncOperationHandle<TTable>>(this.PatchTableContents);
			this.m_RegisterSharedTableAndGuidOperationAction = new Action<AsyncOperationHandle<TTable>>(this.RegisterSharedTableAndGuidOperation);
			this.m_RegisterCompletedTableOperationAction = new Action<AsyncOperationHandle<TTable>>(this.RegisterCompletedTableOperation);
			this.m_ReleaseNextFrame = new Action<AsyncOperationHandle>(LocalizationBehaviour.ReleaseNextFrame);
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0000E965 File Offset: 0x0000CB65
		internal TableReference GetDefaultTable()
		{
			if (this.m_DefaultTableReference.ReferenceType == TableReference.Type.Empty)
			{
				throw new Exception("Trying to get the DefaultTable however the " + base.GetType().Name + " DefaultTable value has not been set. This can be configured in the Localization Settings.");
			}
			return this.m_DefaultTableReference;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0000E99C File Offset: 0x0000CB9C
		internal void RegisterCompletedTableOperation(AsyncOperationHandle<TTable> tableOperation)
		{
			if (!tableOperation.IsDone)
			{
				tableOperation.Completed += this.m_RegisterCompletedTableOperationAction;
				return;
			}
			TTable result = tableOperation.Result;
			if (result == null)
			{
				return;
			}
			this.RegisterTableNameOperation(tableOperation, result.LocaleIdentifier, result.TableCollectionName);
			if (tableOperation.IsValid())
			{
				this.RegisterSharedTableAndGuidOperation(tableOperation);
			}
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0000EA04 File Offset: 0x0000CC04
		private void RegisterTableNameOperation(AsyncOperationHandle<TTable> tableOperation, LocaleIdentifier localeIdentifier, string tableName)
		{
			ValueTuple<LocaleIdentifier, string> valueTuple = new ValueTuple<LocaleIdentifier, string>(localeIdentifier, tableName);
			if (this.TableOperations.ContainsKey(valueTuple))
			{
				return;
			}
			this.TableOperations[valueTuple] = tableOperation;
			if (this.TablePostprocessor != null)
			{
				if (tableOperation.IsDone)
				{
					this.PatchTableContents(tableOperation);
					return;
				}
				tableOperation.Completed += this.m_PatchTableContentsAction;
			}
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x0000EA5C File Offset: 0x0000CC5C
		private void RegisterSharedTableAndGuidOperation(AsyncOperationHandle<TTable> tableOperation)
		{
			if (!tableOperation.IsDone)
			{
				tableOperation.Completed += this.m_RegisterSharedTableAndGuidOperationAction;
				return;
			}
			TTable result = tableOperation.Result;
			if (result == null)
			{
				return;
			}
			Guid tableCollectionNameGuid = result.SharedData.TableCollectionNameGuid;
			if (!this.SharedTableDataOperations.ContainsKey(tableCollectionNameGuid))
			{
				this.SharedTableDataOperations[tableCollectionNameGuid] = AddressablesInterface.ResourceManager.CreateCompletedOperation<SharedTableData>(result.SharedData, null);
			}
			ValueTuple<LocaleIdentifier, string> valueTuple = new ValueTuple<LocaleIdentifier, string>(result.LocaleIdentifier, TableReference.StringFromGuid(tableCollectionNameGuid));
			if (!this.TableOperations.ContainsKey(valueTuple))
			{
				AddressablesInterface.Acquire(tableOperation);
				this.TableOperations[valueTuple] = tableOperation;
			}
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0000EB16 File Offset: 0x0000CD16
		public AsyncOperationHandle<TTable> GetDefaultTableAsync()
		{
			return this.GetTableAsync(this.GetDefaultTable(), null);
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0000EB28 File Offset: 0x0000CD28
		public virtual AsyncOperationHandle<TTable> GetTableAsync(TableReference tableReference, Locale locale = null)
		{
			bool flag = locale != null || LocalizationSettings.SelectedLocaleAsync.IsDone;
			bool flag2 = true;
			if (flag)
			{
				if (locale == null)
				{
					if (LocalizationSettings.SelectedLocaleAsync.Result == null)
					{
						return AddressablesInterface.ResourceManager.CreateCompletedOperation<TTable>(default(TTable), "SelectedLocale is null. Database could not get table.");
					}
					locale = LocalizationSettings.SelectedLocaleAsync.Result;
				}
				flag2 = false;
			}
			tableReference.Validate();
			string text = ((tableReference.ReferenceType == TableReference.Type.Guid) ? TableReference.StringFromGuid(tableReference.TableCollectionNameGuid) : tableReference.TableCollectionName);
			LocaleIdentifier localeIdentifier = (flag2 ? LocalizedDatabase<TTable, TEntry>.k_SelectedLocaleId : locale.Identifier);
			AsyncOperationHandle<TTable> asyncOperationHandle;
			if (this.TableOperations.TryGetValue(new ValueTuple<LocaleIdentifier, string>(localeIdentifier, text), out asyncOperationHandle))
			{
				if (asyncOperationHandle.IsValid())
				{
					return asyncOperationHandle;
				}
				this.TableOperations.Remove(new ValueTuple<LocaleIdentifier, string>(localeIdentifier, text));
			}
			LoadTableOperation<TTable, TEntry> loadTableOperation = this.CreateLoadTableOperation();
			loadTableOperation.Init(this, tableReference, locale);
			loadTableOperation.Dependency = LocalizationSettings.InitializationOperation;
			AsyncOperationHandle<TTable> asyncOperationHandle2 = AddressablesInterface.ResourceManager.StartOperation<TTable>(loadTableOperation, LocalizationSettings.InitializationOperation);
			if (flag2 || tableReference.ReferenceType == TableReference.Type.Guid)
			{
				if (!flag2)
				{
					AddressablesInterface.Acquire(asyncOperationHandle2);
				}
				this.TableOperations[new ValueTuple<LocaleIdentifier, string>(localeIdentifier, text)] = asyncOperationHandle2;
			}
			else
			{
				this.RegisterTableNameOperation(asyncOperationHandle2, localeIdentifier, text);
			}
			this.RegisterCompletedTableOperation(asyncOperationHandle2);
			return asyncOperationHandle2;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x0000EC8D File Offset: 0x0000CE8D
		public virtual TTable GetTable(TableReference tableReference, Locale locale = null)
		{
			return AsyncOperationUtility.SynchronousLoad<TTable>(this.GetTableAsync(tableReference, locale));
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x0000EC9C File Offset: 0x0000CE9C
		public AsyncOperationHandle PreloadTables(TableReference tableReference, Locale locale = null)
		{
			PreloadTablesOperation<TTable, TEntry> preloadTablesOperation = this.CreatePreloadTablesOperation();
			preloadTablesOperation.Init(this, new TableReference[] { tableReference }, locale);
			preloadTablesOperation.Dependency = LocalizationSettings.InitializationOperation;
			AsyncOperationHandle<LocalizedDatabase<TTable, TEntry>> asyncOperationHandle = AddressablesInterface.ResourceManager.StartOperation<LocalizedDatabase<TTable, TEntry>>(preloadTablesOperation, LocalizationSettings.InitializationOperation);
			if (PlaymodeState.IsPlaying)
			{
				asyncOperationHandle.CompletedTypeless += this.ReleaseNextFrame;
			}
			return asyncOperationHandle;
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0000ED08 File Offset: 0x0000CF08
		public AsyncOperationHandle PreloadTables(IList<TableReference> tableReferences, Locale locale = null)
		{
			PreloadTablesOperation<TTable, TEntry> preloadTablesOperation = this.CreatePreloadTablesOperation();
			preloadTablesOperation.Init(this, tableReferences, locale);
			preloadTablesOperation.Dependency = LocalizationSettings.InitializationOperation;
			AsyncOperationHandle<LocalizedDatabase<TTable, TEntry>> asyncOperationHandle = AddressablesInterface.ResourceManager.StartOperation<LocalizedDatabase<TTable, TEntry>>(preloadTablesOperation, LocalizationSettings.InitializationOperation);
			if (PlaymodeState.IsPlaying)
			{
				asyncOperationHandle.CompletedTypeless += this.ReleaseNextFrame;
			}
			return asyncOperationHandle;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0000ED68 File Offset: 0x0000CF68
		public void ReleaseAllTables(Locale locale = null)
		{
			HashSet<TTable> hashSet;
			using (CollectionPool<HashSet<TTable>, TTable>.Get(out hashSet))
			{
				foreach (AsyncOperationHandle<TTable> asyncOperationHandle in this.TableOperations.Values)
				{
					if (asyncOperationHandle.IsValid() && (!(locale != null) || !(asyncOperationHandle.Result.LocaleIdentifier != locale.Identifier)))
					{
						if (asyncOperationHandle.Result != null && !hashSet.Contains(asyncOperationHandle.Result))
						{
							this.ReleaseTableContents(asyncOperationHandle.Result);
							hashSet.Add(asyncOperationHandle.Result);
						}
						AddressablesInterface.Release(asyncOperationHandle);
					}
				}
			}
			foreach (KeyValuePair<Guid, AsyncOperationHandle<SharedTableData>> keyValuePair in this.SharedTableDataOperations)
			{
				AddressablesInterface.SafeRelease(keyValuePair.Value);
			}
			this.SharedTableDataOperations.Clear();
			if (this.m_PreloadOperationHandle.IsValid())
			{
				if (this.m_PreloadOperationHandle.IsDone)
				{
					AddressablesInterface.Release(this.m_PreloadOperationHandle);
				}
				this.m_PreloadOperationHandle = default(AsyncOperationHandle);
			}
			this.TableOperations.Clear();
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000EEF0 File Offset: 0x0000D0F0
		public void ReleaseTable(TableReference tableReference, Locale locale = null)
		{
			tableReference.Validate();
			bool flag = locale == LocalizationSettings.SelectedLocaleAsync.Result;
			if (locale == null)
			{
				locale = LocalizationSettings.SelectedLocaleAsync.Result;
				flag = true;
				if (locale == null)
				{
					return;
				}
			}
			SharedTableData sharedTableData;
			if (tableReference.ReferenceType == TableReference.Type.Guid)
			{
				AsyncOperationHandle<SharedTableData> asyncOperationHandle;
				if (!this.SharedTableDataOperations.TryGetValue(tableReference.TableCollectionNameGuid, out asyncOperationHandle) || asyncOperationHandle.Result == null)
				{
					return;
				}
				sharedTableData = asyncOperationHandle.Result;
			}
			else
			{
				ValueTuple<LocaleIdentifier, string> valueTuple = new ValueTuple<LocaleIdentifier, string>(locale.Identifier, tableReference.TableCollectionName);
				AsyncOperationHandle<TTable> asyncOperationHandle2;
				if (!this.TableOperations.TryGetValue(valueTuple, out asyncOperationHandle2) || asyncOperationHandle2.Result == null)
				{
					return;
				}
				sharedTableData = asyncOperationHandle2.Result.SharedData;
			}
			if (sharedTableData == null)
			{
				return;
			}
			int num = 0;
			bool flag2 = false;
			List<ValueTuple<LocaleIdentifier, string>> list;
			using (CollectionPool<List<ValueTuple<LocaleIdentifier, string>>, ValueTuple<LocaleIdentifier, string>>.Get(out list))
			{
				foreach (KeyValuePair<ValueTuple<LocaleIdentifier, string>, AsyncOperationHandle<TTable>> keyValuePair in this.TableOperations)
				{
					if (keyValuePair.Value.IsValid() && !(keyValuePair.Value.Result == null) && !(keyValuePair.Value.Result.SharedData != sharedTableData))
					{
						if (keyValuePair.Key.Item1 == locale.Identifier || (flag && keyValuePair.Key.Item1 == LocalizedDatabase<TTable, TEntry>.k_SelectedLocaleId))
						{
							if (!flag2)
							{
								this.ReleaseTableContents(keyValuePair.Value.Result);
								flag2 = true;
							}
							AddressablesInterface.SafeRelease(keyValuePair.Value);
							list.Add(keyValuePair.Key);
						}
						else
						{
							num++;
						}
					}
				}
				foreach (ValueTuple<LocaleIdentifier, string> valueTuple2 in list)
				{
					this.TableOperations.Remove(valueTuple2);
				}
				AsyncOperationHandle<SharedTableData> asyncOperationHandle3;
				if (num == 0 && this.SharedTableDataOperations.TryGetValue(sharedTableData.TableCollectionNameGuid, out asyncOperationHandle3))
				{
					AddressablesInterface.SafeRelease(asyncOperationHandle3);
					this.SharedTableDataOperations.Remove(sharedTableData.TableCollectionNameGuid);
				}
			}
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0000F1AC File Offset: 0x0000D3AC
		public virtual AsyncOperationHandle<IList<TTable>> GetAllTables(Locale locale = null)
		{
			LoadAllTablesOperation<TTable, TEntry> loadAllTablesOperation = LoadAllTablesOperation<TTable, TEntry>.Pool.Get();
			loadAllTablesOperation.Init(this, locale);
			loadAllTablesOperation.Dependency = LocalizationSettings.InitializationOperation;
			AsyncOperationHandle<IList<TTable>> asyncOperationHandle = AddressablesInterface.ResourceManager.StartOperation<IList<TTable>>(loadAllTablesOperation, LocalizationSettings.InitializationOperation);
			if (PlaymodeState.IsPlaying)
			{
				asyncOperationHandle.CompletedTypeless += this.ReleaseNextFrame;
			}
			return asyncOperationHandle;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0000F208 File Offset: 0x0000D408
		public virtual bool IsTableLoaded(TableReference tableReference, Locale locale = null)
		{
			string text = ((tableReference.ReferenceType == TableReference.Type.Guid) ? TableReference.StringFromGuid(tableReference.TableCollectionNameGuid) : tableReference.TableCollectionName);
			ValueTuple<LocaleIdentifier, string> valueTuple = ((locale != null) ? new ValueTuple<LocaleIdentifier, string>(locale.Identifier, text) : new ValueTuple<LocaleIdentifier, string>(LocalizationSettings.SelectedLocaleAsync.Result.Identifier, text));
			AsyncOperationHandle<TTable> asyncOperationHandle;
			return this.TableOperations.TryGetValue(valueTuple, out asyncOperationHandle) && asyncOperationHandle.Status == AsyncOperationStatus.Succeeded;
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000F281 File Offset: 0x0000D481
		internal virtual LoadTableOperation<TTable, TEntry> CreateLoadTableOperation()
		{
			return LoadTableOperation<TTable, TEntry>.Pool.Get();
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0000F28D File Offset: 0x0000D48D
		internal virtual PreloadTablesOperation<TTable, TEntry> CreatePreloadTablesOperation()
		{
			return PreloadTablesOperation<TTable, TEntry>.Pool.Get();
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0000F29C File Offset: 0x0000D49C
		public virtual AsyncOperationHandle<LocalizedDatabase<TTable, TEntry>.TableEntryResult> GetTableEntryAsync(TableReference tableReference, TableEntryReference tableEntryReference, Locale locale = null, FallbackBehavior fallbackBehavior = FallbackBehavior.UseProjectSettings)
		{
			AsyncOperationHandle<TTable> tableAsync = this.GetTableAsync(tableReference, locale);
			GetTableEntryOperation<TTable, TEntry> getTableEntryOperation = GetTableEntryOperation<TTable, TEntry>.Pool.Get();
			bool flag = ((fallbackBehavior != FallbackBehavior.UseProjectSettings) ? (fallbackBehavior == FallbackBehavior.UseFallback) : this.UseFallback);
			getTableEntryOperation.Init(this, tableAsync, tableReference, tableEntryReference, locale, flag, true);
			getTableEntryOperation.Dependency = tableAsync;
			return AddressablesInterface.ResourceManager.StartOperation<LocalizedDatabase<TTable, TEntry>.TableEntryResult>(getTableEntryOperation, tableAsync);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000F2F9 File Offset: 0x0000D4F9
		public virtual LocalizedDatabase<TTable, TEntry>.TableEntryResult GetTableEntry(TableReference tableReference, TableEntryReference tableEntryReference, Locale locale = null, FallbackBehavior fallbackBehavior = FallbackBehavior.UseProjectSettings)
		{
			return AsyncOperationUtility.SynchronousLoad<LocalizedDatabase<TTable, TEntry>.TableEntryResult>(this.GetTableEntryAsync(tableReference, tableEntryReference, locale, fallbackBehavior));
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000F30C File Offset: 0x0000D50C
		internal AsyncOperationHandle<SharedTableData> GetSharedTableData(Guid tableNameGuid)
		{
			AsyncOperationHandle<SharedTableData> asyncOperationHandle;
			if (this.SharedTableDataOperations.TryGetValue(tableNameGuid, out asyncOperationHandle))
			{
				return asyncOperationHandle;
			}
			asyncOperationHandle = AddressablesInterface.LoadAssetFromGUID<SharedTableData>(TableReference.StringFromGuid(tableNameGuid));
			this.SharedTableDataOperations[tableNameGuid] = asyncOperationHandle;
			return asyncOperationHandle;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0000F345 File Offset: 0x0000D545
		internal virtual void ReleaseTableContents(TTable table)
		{
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x0000F347 File Offset: 0x0000D547
		public virtual void OnLocaleChanged(Locale locale)
		{
			this.ReleaseAllTables(null);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000F350 File Offset: 0x0000D550
		private void PatchTableContents(AsyncOperationHandle<TTable> tableOperation)
		{
			if (this.TablePostprocessor != null && tableOperation.Result != null)
			{
				this.TablePostprocessor.PostprocessTable(tableOperation.Result);
			}
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0000F385 File Offset: 0x0000D585
		public void ResetState()
		{
			this.ReleaseAllTables(null);
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000F38E File Offset: 0x0000D58E
		void IDisposable.Dispose()
		{
			this.ReleaseAllTables(null);
		}

		// Token: 0x0400012A RID: 298
		[SerializeField]
		private TableReference m_DefaultTableReference;

		// Token: 0x0400012B RID: 299
		[SerializeReference]
		private ITableProvider m_CustomTableProvider;

		// Token: 0x0400012C RID: 300
		[SerializeReference]
		private ITablePostprocessor m_CustomTablePostprocessor;

		// Token: 0x0400012D RID: 301
		[SerializeField]
		private AsynchronousBehaviour m_AsynchronousBehaviour;

		// Token: 0x0400012E RID: 302
		[SerializeField]
		private bool m_UseFallback;

		// Token: 0x0400012F RID: 303
		internal AsyncOperationHandle m_PreloadOperationHandle;

		// Token: 0x04000130 RID: 304
		private Action<AsyncOperationHandle> m_ReleaseNextFrame;

		// Token: 0x04000131 RID: 305
		private readonly Action<AsyncOperationHandle<TTable>> m_PatchTableContentsAction;

		// Token: 0x04000132 RID: 306
		private readonly Action<AsyncOperationHandle<TTable>> m_RegisterSharedTableAndGuidOperationAction;

		// Token: 0x04000133 RID: 307
		private readonly Action<AsyncOperationHandle<TTable>> m_RegisterCompletedTableOperationAction;

		// Token: 0x04000134 RID: 308
		internal static readonly LocaleIdentifier k_SelectedLocaleId = new LocaleIdentifier("selected locale placeholder");

		// Token: 0x02000147 RID: 327
		public struct TableEntryResult
		{
			// Token: 0x1700018D RID: 397
			// (get) Token: 0x060007CD RID: 1997 RVA: 0x00019323 File Offset: 0x00017523
			public readonly TEntry Entry { get; }

			// Token: 0x1700018E RID: 398
			// (get) Token: 0x060007CE RID: 1998 RVA: 0x0001932B File Offset: 0x0001752B
			public readonly TTable Table { get; }

			// Token: 0x060007CF RID: 1999 RVA: 0x00019333 File Offset: 0x00017533
			internal TableEntryResult(TEntry entry, TTable table)
			{
				this.Entry = entry;
				this.Table = table;
			}
		}
	}
}
