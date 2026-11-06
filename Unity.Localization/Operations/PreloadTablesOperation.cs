using System;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization.Operations
{
	// Token: 0x020000C5 RID: 197
	internal class PreloadTablesOperation<TTable, TEntry> : WaitForCurrentOperationAsyncOperationBase<LocalizedDatabase<TTable, TEntry>> where TTable : DetailedLocalizationTable<TEntry> where TEntry : TableEntry
	{
		// Token: 0x060005A0 RID: 1440 RVA: 0x000143AC File Offset: 0x000125AC
		public PreloadTablesOperation()
		{
			this.m_LoadTableContentsAction = delegate(AsyncOperationHandle<IList<AsyncOperationHandle>> a)
			{
				this.LoadTableContents();
				AddressablesInterface.Release(a);
			};
			this.m_FinishPreloadingAction = new Action<AsyncOperationHandle>(this.FinishPreloading);
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x00014404 File Offset: 0x00012604
		public void Init(LocalizedDatabase<TTable, TEntry> database, IList<TableReference> tableReference, Locale locale = null)
		{
			this.m_Database = database;
			this.m_TableReferences = tableReference;
			this.m_SelectedLocale = locale;
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x0001441B File Offset: 0x0001261B
		protected override void Execute()
		{
			this.BeginPreloadingTables();
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x00014424 File Offset: 0x00012624
		private void BeginPreloadingTables()
		{
			foreach (TableReference tableReference in this.m_TableReferences)
			{
				AsyncOperationHandle<TTable> tableAsync = this.m_Database.GetTableAsync(tableReference, this.m_SelectedLocale);
				this.m_LoadTables.Add(tableAsync);
				if (!tableAsync.IsDone)
				{
					this.m_LoadTablesOperation.Add(tableAsync);
				}
			}
			if (this.m_LoadTablesOperation.Count > 0)
			{
				this.m_LoadTablesOperationHandle = AddressablesInterface.CreateGroupOperation(this.m_LoadTablesOperation);
				if (!this.m_LoadTablesOperationHandle.IsDone)
				{
					base.CurrentOperation = this.m_LoadTablesOperationHandle;
					this.m_LoadTablesOperationHandle.Completed += this.m_LoadTableContentsAction;
					return;
				}
			}
			this.LoadTableContents();
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x000144F8 File Offset: 0x000126F8
		private void LoadTableContents()
		{
			foreach (AsyncOperationHandle<TTable> asyncOperationHandle in this.m_LoadTables)
			{
				if (asyncOperationHandle.Result == null)
				{
					base.Complete(null, false, "Table is null.");
					return;
				}
				IPreloadRequired preloadRequired = asyncOperationHandle.Result as IPreloadRequired;
				if (preloadRequired != null)
				{
					this.m_PreloadTablesOperations.Add(preloadRequired.PreloadOperation);
				}
			}
			if (this.m_PreloadTablesOperations.Count == 0)
			{
				base.Complete(this.m_Database, true, null);
				return;
			}
			this.m_PreloadTablesContentsHandle = AddressablesInterface.CreateGroupOperation(this.m_PreloadTablesOperations);
			if (!this.m_PreloadTablesContentsHandle.IsDone)
			{
				base.CurrentOperation = this.m_PreloadTablesContentsHandle;
				this.m_PreloadTablesContentsHandle.CompletedTypeless += this.m_FinishPreloadingAction;
				return;
			}
			this.FinishPreloading(this.m_PreloadTablesContentsHandle);
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00014600 File Offset: 0x00012800
		private void FinishPreloading(AsyncOperationHandle op)
		{
			base.Complete(this.m_Database, op.Status == AsyncOperationStatus.Succeeded, null);
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001461C File Offset: 0x0001281C
		protected override void Destroy()
		{
			base.Destroy();
			AddressablesInterface.ReleaseAndReset<IList<AsyncOperationHandle>>(ref this.m_LoadTablesOperationHandle);
			AddressablesInterface.ReleaseAndReset<IList<AsyncOperationHandle>>(ref this.m_PreloadTablesContentsHandle);
			this.m_LoadTables.Clear();
			this.m_LoadTablesOperation.Clear();
			this.m_PreloadTablesOperations.Clear();
			this.m_TableReferences = null;
			PreloadTablesOperation<TTable, TEntry>.Pool.Release(this);
		}

		// Token: 0x040001E3 RID: 483
		private LocalizedDatabase<TTable, TEntry> m_Database;

		// Token: 0x040001E4 RID: 484
		private readonly List<AsyncOperationHandle<TTable>> m_LoadTables = new List<AsyncOperationHandle<TTable>>();

		// Token: 0x040001E5 RID: 485
		private readonly List<AsyncOperationHandle> m_LoadTablesOperation = new List<AsyncOperationHandle>();

		// Token: 0x040001E6 RID: 486
		private readonly List<AsyncOperationHandle> m_PreloadTablesOperations = new List<AsyncOperationHandle>();

		// Token: 0x040001E7 RID: 487
		private readonly Action<AsyncOperationHandle<IList<AsyncOperationHandle>>> m_LoadTableContentsAction;

		// Token: 0x040001E8 RID: 488
		private readonly Action<AsyncOperationHandle> m_FinishPreloadingAction;

		// Token: 0x040001E9 RID: 489
		private AsyncOperationHandle<IList<AsyncOperationHandle>> m_LoadTablesOperationHandle;

		// Token: 0x040001EA RID: 490
		private AsyncOperationHandle<IList<AsyncOperationHandle>> m_PreloadTablesContentsHandle;

		// Token: 0x040001EB RID: 491
		private IList<TableReference> m_TableReferences;

		// Token: 0x040001EC RID: 492
		private Locale m_SelectedLocale;

		// Token: 0x040001ED RID: 493
		public static readonly ObjectPool<PreloadTablesOperation<TTable, TEntry>> Pool = new ObjectPool<PreloadTablesOperation<TTable, TEntry>>(() => new PreloadTablesOperation<TTable, TEntry>(), null, null, null, false, 10, 10000);
	}
}
