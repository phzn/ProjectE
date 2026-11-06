using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace UnityEngine.Localization.Operations
{
	// Token: 0x020000C4 RID: 196
	internal class PreloadLocaleOperation<TTable, TEntry> : WaitForCurrentOperationAsyncOperationBase<LocalizedDatabase<TTable, TEntry>> where TTable : DetailedLocalizationTable<TEntry> where TEntry : TableEntry
	{
		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x00013EE7 File Offset: 0x000120E7
		protected override float Progress
		{
			get
			{
				return this.m_Progress;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x00013EEF File Offset: 0x000120EF
		protected override string DebugName
		{
			get
			{
				return string.Format("Preload ({0}) {1}", this.m_Locale, this.m_Database.GetType());
			}
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00013F0C File Offset: 0x0001210C
		public PreloadLocaleOperation()
		{
			this.m_LoadTablesAction = new Action<AsyncOperationHandle<IList<IResourceLocation>>>(this.LoadTables);
			this.m_LoadTableContentsAction = new Action<AsyncOperationHandle<TTable>>(this.LoadTableContents);
			this.m_FinishPreloadingAction = new Action<AsyncOperationHandle>(this.FinishPreloading);
			this.m_PreloadTablesCompletedAction = new Action<AsyncOperationHandle<IList<AsyncOperationHandle>>>(this.PreloadTablesCompleted);
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x00013F88 File Offset: 0x00012188
		public void Init(LocalizedDatabase<TTable, TEntry> database, Locale locale)
		{
			this.m_Database = database;
			this.m_Locale = locale;
			this.m_LoadTablesOperations.Clear();
			this.m_PreloadTableContentsOperations.Clear();
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x00013FAE File Offset: 0x000121AE
		protected override void Execute()
		{
			this.BeginPreloading();
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x00013FB8 File Offset: 0x000121B8
		private void BeginPreloading()
		{
			this.m_Progress = 0f;
			string text = AddressHelper.FormatAssetLabel(this.m_Locale.Identifier);
			this.m_ResourceLabels.Clear();
			this.m_ResourceLabels.Add(text);
			this.m_ResourceLabels.Add("Preload");
			this.m_LoadResourcesOperation = AddressablesInterface.LoadResourceLocationsWithLabelsAsync(this.m_ResourceLabels, Addressables.MergeMode.Intersection, typeof(TTable));
			if (!this.m_LoadResourcesOperation.IsValid())
			{
				this.CompleteAndRelease(true, null);
				return;
			}
			if (this.m_LoadResourcesOperation.IsDone)
			{
				this.LoadTables(this.m_LoadResourcesOperation);
				return;
			}
			base.CurrentOperation = this.m_LoadResourcesOperation;
			this.m_LoadResourcesOperation.Completed += this.m_LoadTablesAction;
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x00014078 File Offset: 0x00012278
		private void LoadTables(AsyncOperationHandle<IList<IResourceLocation>> loadResourcesOperation)
		{
			if (loadResourcesOperation.Status != AsyncOperationStatus.Succeeded)
			{
				bool flag = false;
				string text = "Failed to locate preload tables for ";
				Locale locale = this.m_Locale;
				this.CompleteAndRelease(flag, text + ((locale != null) ? locale.ToString() : null));
				return;
			}
			if (loadResourcesOperation.Result.Count == 0)
			{
				this.m_Progress = 1f;
				this.CompleteAndRelease(true, null);
				return;
			}
			foreach (IResourceLocation resourceLocation in loadResourcesOperation.Result)
			{
				AsyncOperationHandle<TTable> asyncOperationHandle = AddressablesInterface.LoadTableFromLocation<TTable>(resourceLocation);
				this.m_LoadTablesOperations.Add(asyncOperationHandle);
				if (asyncOperationHandle.IsDone)
				{
					this.LoadTableContents(asyncOperationHandle);
				}
				else
				{
					asyncOperationHandle.Completed += this.m_LoadTableContentsAction;
				}
			}
			this.m_LoadTablesGroupOperation = AddressablesInterface.CreateGroupOperation(this.m_LoadTablesOperations);
			if (this.m_LoadTablesGroupOperation.IsDone)
			{
				this.PreloadTablesCompleted(this.m_LoadTablesGroupOperation);
				return;
			}
			base.CurrentOperation = this.m_LoadTablesGroupOperation;
			this.m_LoadTablesGroupOperation.Completed += this.m_PreloadTablesCompletedAction;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00014194 File Offset: 0x00012394
		private void LoadTableContents(AsyncOperationHandle<TTable> operation)
		{
			this.m_Progress += 1f / (float)this.m_LoadTablesOperations.Count;
			if (operation.Result == null)
			{
				return;
			}
			TTable result = operation.Result;
			string tableCollectionName = result.TableCollectionName;
			AsyncOperationHandle<TTable> asyncOperationHandle;
			if (this.m_Database.TableOperations.TryGetValue(new ValueTuple<LocaleIdentifier, string>(result.LocaleIdentifier, tableCollectionName), out asyncOperationHandle))
			{
				LocalizationBehaviour.ReleaseNextFrame(operation);
				if (asyncOperationHandle.IsDone && asyncOperationHandle.Result != result)
				{
					Debug.LogError(string.Format("A table with the same key `{0}` already exists. Something went wrong during preloading of {1}. Table {2} does not match {3}.", new object[] { tableCollectionName, this.m_Locale, result, asyncOperationHandle.Result }));
					return;
				}
			}
			else
			{
				this.m_Database.RegisterCompletedTableOperation(operation);
			}
			IPreloadRequired preloadRequired = result as IPreloadRequired;
			if (preloadRequired != null)
			{
				AsyncOperationHandle preloadOperation = preloadRequired.PreloadOperation;
				if (!preloadOperation.IsDone)
				{
					this.m_PreloadTableContentsOperations.Add(preloadOperation);
				}
			}
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x000142AC File Offset: 0x000124AC
		private void PreloadTablesCompleted(AsyncOperationHandle<IList<AsyncOperationHandle>> obj)
		{
			if (this.m_PreloadTableContentsOperations.Count == 0)
			{
				this.CompleteAndRelease(true, null);
				return;
			}
			this.m_LoadTableContentsOperation = AddressablesInterface.CreateGroupOperation(this.m_PreloadTableContentsOperations);
			if (this.m_LoadTableContentsOperation.IsDone)
			{
				this.FinishPreloading(this.m_LoadTableContentsOperation);
				return;
			}
			base.CurrentOperation = this.m_LoadTableContentsOperation;
			this.m_LoadTableContentsOperation.CompletedTypeless += this.m_FinishPreloadingAction;
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x00014321 File Offset: 0x00012521
		private void FinishPreloading(AsyncOperationHandle op)
		{
			this.m_Progress = 1f;
			this.CompleteAndRelease(op.Status == AsyncOperationStatus.Succeeded, null);
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0001433F File Offset: 0x0001253F
		private void CompleteAndRelease(bool success, string errorMsg)
		{
			AddressablesInterface.ReleaseAndReset<IList<IResourceLocation>>(ref this.m_LoadResourcesOperation);
			AddressablesInterface.ReleaseAndReset<IList<AsyncOperationHandle>>(ref this.m_LoadTablesGroupOperation);
			AddressablesInterface.ReleaseAndReset<IList<AsyncOperationHandle>>(ref this.m_LoadTableContentsOperation);
			base.Complete(this.m_Database, success, errorMsg);
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00014370 File Offset: 0x00012570
		protected override void Destroy()
		{
			base.Destroy();
			PreloadLocaleOperation<TTable, TEntry>.Pool.Release(this);
		}

		// Token: 0x040001D5 RID: 469
		private readonly Action<AsyncOperationHandle<IList<IResourceLocation>>> m_LoadTablesAction;

		// Token: 0x040001D6 RID: 470
		private readonly Action<AsyncOperationHandle<TTable>> m_LoadTableContentsAction;

		// Token: 0x040001D7 RID: 471
		private readonly Action<AsyncOperationHandle> m_FinishPreloadingAction;

		// Token: 0x040001D8 RID: 472
		private readonly Action<AsyncOperationHandle<IList<AsyncOperationHandle>>> m_PreloadTablesCompletedAction;

		// Token: 0x040001D9 RID: 473
		private LocalizedDatabase<TTable, TEntry> m_Database;

		// Token: 0x040001DA RID: 474
		private Locale m_Locale;

		// Token: 0x040001DB RID: 475
		private AsyncOperationHandle<IList<IResourceLocation>> m_LoadResourcesOperation;

		// Token: 0x040001DC RID: 476
		private AsyncOperationHandle<IList<AsyncOperationHandle>> m_LoadTablesGroupOperation;

		// Token: 0x040001DD RID: 477
		private AsyncOperationHandle<IList<AsyncOperationHandle>> m_LoadTableContentsOperation;

		// Token: 0x040001DE RID: 478
		private readonly List<AsyncOperationHandle> m_LoadTablesOperations = new List<AsyncOperationHandle>();

		// Token: 0x040001DF RID: 479
		private readonly List<AsyncOperationHandle> m_PreloadTableContentsOperations = new List<AsyncOperationHandle>();

		// Token: 0x040001E0 RID: 480
		private readonly List<string> m_ResourceLabels = new List<string>();

		// Token: 0x040001E1 RID: 481
		private float m_Progress;

		// Token: 0x040001E2 RID: 482
		public static readonly ObjectPool<PreloadLocaleOperation<TTable, TEntry>> Pool = new ObjectPool<PreloadLocaleOperation<TTable, TEntry>>(() => new PreloadLocaleOperation<TTable, TEntry>(), null, null, null, false, 10, 10000);
	}
}
