using System;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization.Operations
{
	// Token: 0x020000C3 RID: 195
	internal class PreloadDatabaseOperation<TTable, TEntry> : WaitForCurrentOperationAsyncOperationBase<LocalizedDatabase<TTable, TEntry>> where TTable : DetailedLocalizationTable<TEntry> where TEntry : TableEntry
	{
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x00013BAC File Offset: 0x00011DAC
		protected override float Progress
		{
			get
			{
				if (!base.CurrentOperation.IsValid())
				{
					return base.Progress;
				}
				return base.CurrentOperation.PercentComplete;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x00013BDE File Offset: 0x00011DDE
		protected override string DebugName
		{
			get
			{
				return string.Format("Preload {0}", this.m_Database.GetType());
			}
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00013BF5 File Offset: 0x00011DF5
		public PreloadDatabaseOperation()
		{
			this.m_CompleteOperation = new Action<AsyncOperationHandle>(this.CompleteOperation);
			this.m_CompleteGenericGroup = new Action<AsyncOperationHandle<IList<AsyncOperationHandle>>>(this.CompleteGenericGroup);
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x00013C21 File Offset: 0x00011E21
		public void Init(LocalizedDatabase<TTable, TEntry> database)
		{
			this.m_Database = database;
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00013C2C File Offset: 0x00011E2C
		protected override void Execute()
		{
			AsyncOperationHandle<Locale> selectedLocaleAsync = LocalizationSettings.SelectedLocaleAsync;
			if (selectedLocaleAsync.Result == null)
			{
				base.Complete(this.m_Database, true, null);
				return;
			}
			switch (LocalizationSettings.PreloadBehavior)
			{
			case PreloadBehavior.NoPreloading:
				base.Complete(this.m_Database, true, null);
				return;
			case PreloadBehavior.PreloadSelectedLocale:
			{
				AsyncOperationHandle asyncOperationHandle = this.PreloadLocale(selectedLocaleAsync.Result);
				if (asyncOperationHandle.IsDone)
				{
					this.m_CompleteOperation(asyncOperationHandle);
					return;
				}
				asyncOperationHandle.Completed += this.m_CompleteOperation;
				base.CurrentOperation = asyncOperationHandle;
				return;
			}
			case PreloadBehavior.PreloadSelectedLocaleAndFallbacks:
			{
				HashSet<Locale> hashSet;
				using (CollectionPool<HashSet<Locale>, Locale>.Get(out hashSet))
				{
					hashSet.Add(selectedLocaleAsync.Result);
					this.GetAllFallbackLocales(selectedLocaleAsync.Result, hashSet);
					this.PreloadLocales(hashSet);
					return;
				}
				break;
			}
			case PreloadBehavior.PreloadAllLocales:
				break;
			default:
				return;
			}
			this.PreloadLocales(LocalizationSettings.AvailableLocales.Locales);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00013D20 File Offset: 0x00011F20
		private void GetAllFallbackLocales(Locale current, HashSet<Locale> locales)
		{
			foreach (Locale locale in current.GetFallbacks())
			{
				if (!locales.Contains(locale))
				{
					locales.Add(locale);
					this.GetAllFallbackLocales(locale, locales);
				}
			}
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00013D80 File Offset: 0x00011F80
		private AsyncOperationHandle PreloadLocale(Locale locale)
		{
			PreloadLocaleOperation<TTable, TEntry> preloadLocaleOperation = PreloadLocaleOperation<TTable, TEntry>.Pool.Get();
			preloadLocaleOperation.Init(this.m_Database, locale);
			return AddressablesInterface.ResourceManager.StartOperation<LocalizedDatabase<TTable, TEntry>>(preloadLocaleOperation, default(AsyncOperationHandle));
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00013DC0 File Offset: 0x00011FC0
		private void PreloadLocales(ICollection<Locale> locales)
		{
			List<AsyncOperationHandle> list;
			using (CollectionPool<List<AsyncOperationHandle>, AsyncOperationHandle>.Get(out list))
			{
				foreach (Locale locale in locales)
				{
					AsyncOperationHandle asyncOperationHandle = this.PreloadLocale(locale);
					if (!asyncOperationHandle.IsDone)
					{
						list.Add(asyncOperationHandle);
					}
				}
				if (list.Count > 0)
				{
					AsyncOperationHandle<IList<AsyncOperationHandle>> asyncOperationHandle2 = AddressablesInterface.CreateGroupOperation(list);
					asyncOperationHandle2.Completed += this.m_CompleteGenericGroup;
					base.CurrentOperation = asyncOperationHandle2;
				}
				else
				{
					base.Complete(this.m_Database, true, null);
				}
			}
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x00013E7C File Offset: 0x0001207C
		private void CompleteOperation(AsyncOperationHandle operationHandle)
		{
			AddressablesInterface.Release(operationHandle);
			base.Complete(this.m_Database, true, null);
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00013E92 File Offset: 0x00012092
		private void CompleteGenericGroup(AsyncOperationHandle<IList<AsyncOperationHandle>> operationHandle)
		{
			AddressablesInterface.Release(operationHandle);
			base.Complete(this.m_Database, true, null);
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00013EAD File Offset: 0x000120AD
		protected override void Destroy()
		{
			base.Destroy();
			PreloadDatabaseOperation<TTable, TEntry>.Pool.Release(this);
		}

		// Token: 0x040001D1 RID: 465
		private readonly Action<AsyncOperationHandle> m_CompleteOperation;

		// Token: 0x040001D2 RID: 466
		private readonly Action<AsyncOperationHandle<IList<AsyncOperationHandle>>> m_CompleteGenericGroup;

		// Token: 0x040001D3 RID: 467
		private LocalizedDatabase<TTable, TEntry> m_Database;

		// Token: 0x040001D4 RID: 468
		public static readonly ObjectPool<PreloadDatabaseOperation<TTable, TEntry>> Pool = new ObjectPool<PreloadDatabaseOperation<TTable, TEntry>>(() => new PreloadDatabaseOperation<TTable, TEntry>(), null, null, null, false, 10, 10000);
	}
}
