using System;
using System.Collections.Generic;
using UnityEngine.Localization.Metadata;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization.Operations
{
	// Token: 0x020000BC RID: 188
	internal class GetTableEntryOperation<TTable, TEntry> : WaitForCurrentOperationAsyncOperationBase<LocalizedDatabase<TTable, TEntry>.TableEntryResult> where TTable : DetailedLocalizationTable<TEntry> where TEntry : TableEntry
	{
		// Token: 0x06000544 RID: 1348 RVA: 0x000126E7 File Offset: 0x000108E7
		public GetTableEntryOperation()
		{
			this.m_ExtractEntryFromTableAction = new Action<AsyncOperationHandle<TTable>>(this.ExtractEntryFromTable);
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00012704 File Offset: 0x00010904
		public void Init(LocalizedDatabase<TTable, TEntry> database, AsyncOperationHandle<TTable> loadTableOperation, TableReference tableReference, TableEntryReference tableEntryReference, Locale selectedLoale, bool UseFallBack, bool autoRelease)
		{
			this.m_Database = database;
			this.m_LoadTableOperation = loadTableOperation;
			AddressablesInterface.Acquire(this.m_LoadTableOperation);
			this.m_TableReference = tableReference;
			this.m_TableEntryReference = tableEntryReference;
			this.m_SelectedLocale = selectedLoale;
			this.m_UseFallback = UseFallBack;
			this.m_AutoRelease = autoRelease;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x00012758 File Offset: 0x00010958
		protected override void Execute()
		{
			AsyncOperationHandle<TTable> loadTableOperation = this.m_LoadTableOperation;
			this.m_LoadTableOperation = default(AsyncOperationHandle<TTable>);
			if (this.m_SelectedLocale == null)
			{
				this.m_SelectedLocale = LocalizationSettings.SelectedLocaleAsync.Result;
				if (this.m_SelectedLocale == null)
				{
					this.CompleteAndRelease(default(LocalizedDatabase<TTable, TEntry>.TableEntryResult), false, "SelectedLocale is null. Could not get table entry.");
					AddressablesInterface.SafeRelease(loadTableOperation);
					return;
				}
			}
			this.m_CurrentLocale = this.m_SelectedLocale;
			this.ExtractEntryFromTable(loadTableOperation);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x000127DC File Offset: 0x000109DC
		private void ExtractEntryFromTable(AsyncOperationHandle<TTable> asyncOperation)
		{
			TTable ttable = asyncOperation.Result;
			TEntry tentry = ((ttable != null) ? ttable.GetEntryFromReference(this.m_TableEntryReference) : default(TEntry));
			if (this.HandleEntryOverride(asyncOperation, tentry) || this.HandleFallback(asyncOperation, tentry))
			{
				return;
			}
			this.m_LoadTableOperation = asyncOperation;
			this.CompleteAndRelease(new LocalizedDatabase<TTable, TEntry>.TableEntryResult(tentry, asyncOperation.Result), true, null);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00012844 File Offset: 0x00010A44
		private bool HandleEntryOverride(AsyncOperationHandle<TTable> asyncOperation, TEntry entry)
		{
			if (entry != null)
			{
				for (int i = 0; i < entry.MetadataEntries.Count; i++)
				{
					IEntryOverride entryOverride = entry.MetadataEntries[i] as IEntryOverride;
					if (entryOverride != null && this.ApplyEntryOverride(entryOverride, asyncOperation, entry))
					{
						return true;
					}
				}
			}
			TEntry tentry = entry;
			SharedTableData.SharedTableEntry sharedTableEntry;
			if ((sharedTableEntry = ((tentry != null) ? tentry.SharedEntry : null)) == null)
			{
				TTable ttable = asyncOperation.Result;
				sharedTableEntry = ((ttable != null) ? ttable.SharedData.GetEntryFromReference(this.m_TableEntryReference) : null);
			}
			SharedTableData.SharedTableEntry sharedTableEntry2 = sharedTableEntry;
			if (sharedTableEntry2 != null)
			{
				for (int j = 0; j < sharedTableEntry2.Metadata.MetadataEntries.Count; j++)
				{
					IEntryOverride entryOverride2 = sharedTableEntry2.Metadata.MetadataEntries[j] as IEntryOverride;
					if (entryOverride2 != null && this.ApplyEntryOverride(entryOverride2, asyncOperation, entry))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00012920 File Offset: 0x00010B20
		private bool ApplyEntryOverride(IEntryOverride entryOverride, AsyncOperationHandle<TTable> asyncOperation, TEntry entry)
		{
			if (entryOverride == null)
			{
				return false;
			}
			TableReference tableReference;
			TableEntryReference tableEntryReference;
			EntryOverrideType @override = entryOverride.GetOverride(out tableReference, out tableEntryReference);
			if (@override == EntryOverrideType.None)
			{
				return false;
			}
			if (@override == EntryOverrideType.Entry)
			{
				this.m_TableEntryReference = tableEntryReference;
				this.ExtractEntryFromTable(asyncOperation);
				return true;
			}
			if (@override == EntryOverrideType.Table)
			{
				TEntry tentry = entry;
				SharedTableData.SharedTableEntry sharedTableEntry;
				if ((sharedTableEntry = ((tentry != null) ? tentry.SharedEntry : null)) == null)
				{
					TTable ttable = asyncOperation.Result;
					sharedTableEntry = ((ttable != null) ? ttable.SharedData.GetEntryFromReference(this.m_TableEntryReference) : null);
				}
				SharedTableData.SharedTableEntry sharedTableEntry2 = sharedTableEntry;
				this.m_TableEntryReference = sharedTableEntry2.Key;
			}
			else if (@override == EntryOverrideType.TableAndEntry)
			{
				this.m_TableEntryReference = tableEntryReference;
			}
			AddressablesInterface.Release(asyncOperation);
			asyncOperation = this.m_Database.GetTableAsync(tableReference, this.m_CurrentLocale);
			AddressablesInterface.Acquire(asyncOperation);
			if (asyncOperation.IsDone)
			{
				this.ExtractEntryFromTable(asyncOperation);
			}
			else
			{
				base.CurrentOperation = asyncOperation;
				asyncOperation.Completed += this.m_ExtractEntryFromTableAction;
			}
			return true;
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00012A08 File Offset: 0x00010C08
		private Locale GetNextFallback(Locale currentLocale)
		{
			if (this.m_FallbackQueue == null)
			{
				this.m_FallbackQueue = CollectionPool<List<Locale>, Locale>.Get();
				this.m_HandledFallbacks = CollectionPool<HashSet<Locale>, Locale>.Get();
			}
			if (!this.m_HandledFallbacks.Contains(currentLocale))
			{
				this.m_HandledFallbacks.Add(currentLocale);
			}
			IEnumerable<Locale> fallbacks = currentLocale.GetFallbacks();
			if (fallbacks != null)
			{
				foreach (Locale locale in fallbacks)
				{
					if (!this.m_HandledFallbacks.Contains(locale))
					{
						this.m_HandledFallbacks.Add(locale);
						this.m_FallbackQueue.Add(locale);
					}
				}
			}
			if (this.m_FallbackQueue.Count == 0)
			{
				return null;
			}
			Locale locale2 = this.m_FallbackQueue[0];
			this.m_FallbackQueue.RemoveAt(0);
			return locale2;
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00012ADC File Offset: 0x00010CDC
		private bool HandleFallback(AsyncOperationHandle<TTable> asyncOperation, TEntry entry)
		{
			if ((entry == null || string.IsNullOrEmpty(entry.Data.Localized)) && this.m_UseFallback)
			{
				Locale nextFallback = this.GetNextFallback(this.m_CurrentLocale);
				if (nextFallback != null)
				{
					this.m_CurrentLocale = nextFallback;
					AddressablesInterface.Release(asyncOperation);
					asyncOperation = this.m_Database.GetTableAsync(this.m_TableReference, this.m_CurrentLocale);
					AddressablesInterface.Acquire(asyncOperation);
					if (asyncOperation.IsDone)
					{
						this.ExtractEntryFromTable(asyncOperation);
					}
					else
					{
						base.CurrentOperation = asyncOperation;
						asyncOperation.Completed += this.m_ExtractEntryFromTableAction;
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00012B8D File Offset: 0x00010D8D
		private void CompleteAndRelease(LocalizedDatabase<TTable, TEntry>.TableEntryResult result, bool success, string errorMsg)
		{
			base.Complete(result, success, errorMsg);
			if (this.m_AutoRelease && PlaymodeState.IsPlaying)
			{
				LocalizationBehaviour.ReleaseNextFrame(base.Handle);
			}
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00012BB8 File Offset: 0x00010DB8
		protected override void Destroy()
		{
			AddressablesInterface.SafeRelease(this.m_LoadTableOperation);
			this.m_LoadTableOperation = default(AsyncOperationHandle<TTable>);
			base.Destroy();
			GetTableEntryOperation<TTable, TEntry>.Pool.Release(this);
			if (this.m_FallbackQueue != null)
			{
				CollectionPool<List<Locale>, Locale>.Release(this.m_FallbackQueue);
				CollectionPool<HashSet<Locale>, Locale>.Release(this.m_HandledFallbacks);
				this.m_FallbackQueue = null;
				this.m_HandledFallbacks = null;
			}
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00012C20 File Offset: 0x00010E20
		public override string ToString()
		{
			return string.Format("{0}, Current Locale: {1}, Selected Locale: {2}, Table: {3}, Entry: {4}, Fallback: {5}", new object[]
			{
				base.GetType().Name,
				this.m_CurrentLocale,
				this.m_SelectedLocale,
				this.m_TableReference,
				this.m_TableEntryReference,
				this.m_UseFallback
			});
		}

		// Token: 0x0400019B RID: 411
		private readonly Action<AsyncOperationHandle<TTable>> m_ExtractEntryFromTableAction;

		// Token: 0x0400019C RID: 412
		private AsyncOperationHandle<TTable> m_LoadTableOperation;

		// Token: 0x0400019D RID: 413
		private TableReference m_TableReference;

		// Token: 0x0400019E RID: 414
		private TableEntryReference m_TableEntryReference;

		// Token: 0x0400019F RID: 415
		private LocalizedDatabase<TTable, TEntry> m_Database;

		// Token: 0x040001A0 RID: 416
		private Locale m_SelectedLocale;

		// Token: 0x040001A1 RID: 417
		private Locale m_CurrentLocale;

		// Token: 0x040001A2 RID: 418
		private HashSet<Locale> m_HandledFallbacks;

		// Token: 0x040001A3 RID: 419
		private List<Locale> m_FallbackQueue;

		// Token: 0x040001A4 RID: 420
		private bool m_UseFallback;

		// Token: 0x040001A5 RID: 421
		private bool m_AutoRelease;

		// Token: 0x040001A6 RID: 422
		public static readonly ObjectPool<GetTableEntryOperation<TTable, TEntry>> Pool = new ObjectPool<GetTableEntryOperation<TTable, TEntry>>(() => new GetTableEntryOperation<TTable, TEntry>(), null, null, null, false, 10, 10000);
	}
}
