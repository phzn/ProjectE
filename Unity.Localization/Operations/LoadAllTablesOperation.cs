using System;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization.Operations
{
	// Token: 0x020000BE RID: 190
	internal class LoadAllTablesOperation<TTable, TEntry> : WaitForCurrentOperationAsyncOperationBase<IList<TTable>> where TTable : DetailedLocalizationTable<TEntry> where TEntry : TableEntry
	{
		// Token: 0x06000561 RID: 1377 RVA: 0x00013100 File Offset: 0x00011300
		public LoadAllTablesOperation()
		{
			this.m_LoadingCompletedAction = new Action<AsyncOperationHandle<IList<TTable>>>(this.LoadingCompleted);
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0001311A File Offset: 0x0001131A
		public void Init(LocalizedDatabase<TTable, TEntry> database, Locale locale)
		{
			this.m_Database = database;
			this.m_SelectedLocale = locale;
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0001312C File Offset: 0x0001132C
		protected override void Execute()
		{
			if (this.m_SelectedLocale == null)
			{
				this.m_SelectedLocale = LocalizationSettings.SelectedLocale;
				if (this.m_SelectedLocale == null)
				{
					base.Complete(null, false, "SelectedLocale is null. Could not load table.");
					return;
				}
			}
			string text = ((this.m_SelectedLocale != null) ? AddressHelper.FormatAssetLabel(this.m_SelectedLocale.Identifier) : AddressHelper.FormatAssetLabel(LocalizationSettings.SelectedLocaleAsync.Result.Identifier));
			this.m_AllTablesOperation = AddressablesInterface.LoadAssetsWithLabel<TTable>(text, null);
			if (this.m_AllTablesOperation.IsDone)
			{
				this.LoadingCompleted(this.m_AllTablesOperation);
				return;
			}
			this.m_AllTablesOperation.Completed += this.m_LoadingCompletedAction;
			base.CurrentOperation = this.m_AllTablesOperation;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x000131F0 File Offset: 0x000113F0
		private void LoadingCompleted(AsyncOperationHandle<IList<TTable>> obj)
		{
			if (obj.Result != null)
			{
				foreach (TTable ttable in obj.Result)
				{
					if (!(ttable == null))
					{
						this.m_Database.GetTableAsync(ttable.TableCollectionName, this.m_SelectedLocale);
					}
				}
			}
			IList<TTable> result = obj.Result;
			bool flag = obj.Status == AsyncOperationStatus.Succeeded;
			Exception operationException = obj.OperationException;
			base.Complete(result, flag, (operationException != null) ? operationException.Message : null);
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0001329C File Offset: 0x0001149C
		protected override void Destroy()
		{
			base.Destroy();
			LoadAllTablesOperation<TTable, TEntry>.Pool.Release(this);
			AddressablesInterface.ReleaseAndReset<IList<TTable>>(ref this.m_AllTablesOperation);
		}

		// Token: 0x040001B4 RID: 436
		private readonly Action<AsyncOperationHandle<IList<TTable>>> m_LoadingCompletedAction;

		// Token: 0x040001B5 RID: 437
		private AsyncOperationHandle<IList<TTable>> m_AllTablesOperation;

		// Token: 0x040001B6 RID: 438
		private LocalizedDatabase<TTable, TEntry> m_Database;

		// Token: 0x040001B7 RID: 439
		private Locale m_SelectedLocale;

		// Token: 0x040001B8 RID: 440
		public static readonly ObjectPool<LoadAllTablesOperation<TTable, TEntry>> Pool = new ObjectPool<LoadAllTablesOperation<TTable, TEntry>>(() => new LoadAllTablesOperation<TTable, TEntry>(), null, null, null, false, 10, 10000);
	}
}
