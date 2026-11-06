using System;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Core.Formatting;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Localization.Tables;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization.Operations
{
	// Token: 0x020000BB RID: 187
	internal class GetLocalizedStringOperation : WaitForCurrentOperationAsyncOperationBase<string>
	{
		// Token: 0x0600053D RID: 1341 RVA: 0x000124B0 File Offset: 0x000106B0
		public void Init(AsyncOperationHandle<LocalizedDatabase<StringTable, StringTableEntry>.TableEntryResult> tableEntryOperation, Locale locale, LocalizedStringDatabase database, TableReference tableReference, TableEntryReference tableEntryReference, IList<object> arguments, IVariableGroup localVariables, bool autoRelease)
		{
			this.m_TableEntryOperation = tableEntryOperation;
			this.m_SelectedLocale = locale;
			AddressablesInterface.Acquire(this.m_TableEntryOperation);
			this.m_Database = database;
			this.m_TableReference = tableReference;
			this.m_TableEntryReference = tableEntryReference;
			this.m_Arguments = arguments;
			this.m_LocalVariables = localVariables;
			this.m_AutoRelease = autoRelease;
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0001250C File Offset: 0x0001070C
		protected override void Execute()
		{
			if (this.m_SelectedLocale == null)
			{
				this.m_SelectedLocale = LocalizationSettings.SelectedLocaleAsync.Result;
				if (this.m_SelectedLocale == null)
				{
					this.CompleteAndRelease(null, false, "SelectedLocale is null. Could not get localized string.");
					return;
				}
			}
			if (this.m_TableEntryOperation.Status != AsyncOperationStatus.Succeeded)
			{
				this.CompleteAndRelease(null, false, "Load Table Operation Failed");
				return;
			}
			try
			{
				StringTableEntry entry = this.m_TableEntryOperation.Result.Entry;
				FormatCache formatCache = ((entry != null) ? entry.GetOrCreateFormatCache() : null);
				if (formatCache != null)
				{
					formatCache.LocalVariables = this.m_LocalVariables;
				}
				string text = this.m_Database.GenerateLocalizedString(this.m_TableEntryOperation.Result.Table, entry, this.m_TableReference, this.m_TableEntryReference, this.m_SelectedLocale, this.m_Arguments);
				if (formatCache != null)
				{
					formatCache.LocalVariables = null;
				}
				this.CompleteAndRelease(text, true, null);
			}
			catch (Exception ex)
			{
				this.CompleteAndRelease(null, false, ex.Message);
			}
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x00012618 File Offset: 0x00010818
		public void CompleteAndRelease(string result, bool success, string errorMsg)
		{
			base.Complete(result, success, errorMsg);
			AddressablesInterface.SafeRelease(this.m_TableEntryOperation);
			if (this.m_AutoRelease && PlaymodeState.IsPlaying)
			{
				LocalizationBehaviour.ReleaseNextFrame(base.Handle);
			}
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x00012652 File Offset: 0x00010852
		protected override void Destroy()
		{
			base.Destroy();
			GetLocalizedStringOperation.Pool.Release(this);
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x00012668 File Offset: 0x00010868
		public override string ToString()
		{
			return string.Format("{0}, Locale: {1}, Table: {2}, Entry: {3}", new object[]
			{
				base.GetType().Name,
				this.m_SelectedLocale,
				this.m_TableReference,
				this.m_TableEntryReference
			});
		}

		// Token: 0x04000192 RID: 402
		private LocalizedStringDatabase m_Database;

		// Token: 0x04000193 RID: 403
		private AsyncOperationHandle<LocalizedDatabase<StringTable, StringTableEntry>.TableEntryResult> m_TableEntryOperation;

		// Token: 0x04000194 RID: 404
		private TableReference m_TableReference;

		// Token: 0x04000195 RID: 405
		private TableEntryReference m_TableEntryReference;

		// Token: 0x04000196 RID: 406
		private Locale m_SelectedLocale;

		// Token: 0x04000197 RID: 407
		private IList<object> m_Arguments;

		// Token: 0x04000198 RID: 408
		private IVariableGroup m_LocalVariables;

		// Token: 0x04000199 RID: 409
		private bool m_AutoRelease;

		// Token: 0x0400019A RID: 410
		public static readonly ObjectPool<GetLocalizedStringOperation> Pool = new ObjectPool<GetLocalizedStringOperation>(() => new GetLocalizedStringOperation(), null, null, null, false, 10, 10000);
	}
}
