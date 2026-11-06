using System;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization
{
	// Token: 0x0200001A RID: 26
	[Serializable]
	public abstract class LocalizedTable<TTable, TEntry> where TTable : DetailedLocalizationTable<TEntry> where TEntry : TableEntry
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000D8 RID: 216
		protected abstract LocalizedDatabase<TTable, TEntry> Database { get; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x0000408B File Offset: 0x0000228B
		// (set) Token: 0x060000DA RID: 218 RVA: 0x00004093 File Offset: 0x00002293
		public AsyncOperationHandle<TTable> CurrentLoadingOperationHandle { get; internal set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000DB RID: 219 RVA: 0x0000409C File Offset: 0x0000229C
		// (set) Token: 0x060000DC RID: 220 RVA: 0x000040A4 File Offset: 0x000022A4
		public TableReference TableReference
		{
			get
			{
				return this.m_TableReference;
			}
			set
			{
				if (value.Equals(this.m_TableReference))
				{
					return;
				}
				this.m_TableReference = value;
				this.ForceUpdate();
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000DD RID: 221 RVA: 0x000040C4 File Offset: 0x000022C4
		public bool IsEmpty
		{
			get
			{
				return this.TableReference.ReferenceType == TableReference.Type.Empty;
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060000DE RID: 222 RVA: 0x000040E4 File Offset: 0x000022E4
		// (remove) Token: 0x060000DF RID: 223 RVA: 0x00004165 File Offset: 0x00002365
		public event LocalizedTable<TTable, TEntry>.ChangeHandler TableChanged
		{
			add
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				this.m_ChangeHandler.Add(value, 5);
				if (this.m_ChangeHandler.Length == 1)
				{
					LocalizationSettings.ValidateSettingsExist("");
					LocalizationSettings.SelectedLocaleChanged += this.m_SelectedLocaleChanged;
					this.ForceUpdate();
					return;
				}
				if (this.CurrentLoadingOperationHandle.IsValid() && this.CurrentLoadingOperationHandle.IsDone)
				{
					value(this.CurrentLoadingOperationHandle.Result);
				}
			}
			remove
			{
				this.m_ChangeHandler.RemoveByMovingTail(value);
				if (this.m_ChangeHandler.Length == 0)
				{
					LocalizationSettings.SelectedLocaleChanged -= this.m_SelectedLocaleChanged;
					this.ClearLoadingOperation();
				}
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004191 File Offset: 0x00002391
		public LocalizedTable()
		{
			this.m_SelectedLocaleChanged = new Action<Locale>(this.HandleLocaleChange);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000041AB File Offset: 0x000023AB
		public AsyncOperationHandle<TTable> GetTableAsync()
		{
			return this.Database.GetTableAsync(this.TableReference, null);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000041BF File Offset: 0x000023BF
		public TTable GetTable()
		{
			return AsyncOperationUtility.SynchronousLoad<TTable>(this.GetTableAsync());
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000041CC File Offset: 0x000023CC
		protected void ForceUpdate()
		{
			if (this.m_ChangeHandler.Length != 0)
			{
				this.HandleLocaleChange(null);
			}
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000041E4 File Offset: 0x000023E4
		private void InvokeChangeHandler(TTable value)
		{
			try
			{
				this.m_ChangeHandler.LockForChanges();
				int length = this.m_ChangeHandler.Length;
				if (length == 1)
				{
					this.m_ChangeHandler.SingleDelegate(value);
				}
				else if (length > 1)
				{
					LocalizedTable<TTable, TEntry>.ChangeHandler[] multiDelegates = this.m_ChangeHandler.MultiDelegates;
					for (int i = 0; i < length; i++)
					{
						multiDelegates[i](value);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			this.m_ChangeHandler.UnlockForChanges();
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00004268 File Offset: 0x00002468
		private void HandleLocaleChange(Locale _)
		{
			this.ClearLoadingOperation();
			if (this.IsEmpty)
			{
				return;
			}
			this.CurrentLoadingOperationHandle = this.GetTableAsync();
			if (this.CurrentLoadingOperationHandle.IsDone)
			{
				this.AutomaticLoadingCompleted(this.CurrentLoadingOperationHandle);
				return;
			}
			this.CurrentLoadingOperationHandle.Completed += this.AutomaticLoadingCompleted;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x000042C8 File Offset: 0x000024C8
		private void AutomaticLoadingCompleted(AsyncOperationHandle<TTable> loadOperation)
		{
			if (loadOperation.Status != AsyncOperationStatus.Succeeded)
			{
				this.CurrentLoadingOperationHandle = default(AsyncOperationHandle<TTable>);
				return;
			}
			this.InvokeChangeHandler(loadOperation.Result);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x000042FC File Offset: 0x000024FC
		private void ClearLoadingOperation()
		{
			if (this.CurrentLoadingOperationHandle.IsValid())
			{
				if (!this.CurrentLoadingOperationHandle.IsDone)
				{
					this.CurrentLoadingOperationHandle.Completed -= this.AutomaticLoadingCompleted;
				}
				this.CurrentLoadingOperationHandle = default(AsyncOperationHandle<TTable>);
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004352 File Offset: 0x00002552
		public override string ToString()
		{
			return this.TableReference;
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00004360 File Offset: 0x00002560
		[Obsolete("CurrentLoadingOperation is deprecated, use CurrentLoadingOperationHandle instead.")]
		public AsyncOperationHandle<TTable>? CurrentLoadingOperation
		{
			get
			{
				return new AsyncOperationHandle<TTable>?(this.CurrentLoadingOperationHandle.IsValid() ? this.CurrentLoadingOperationHandle : default(AsyncOperationHandle<TTable>));
			}
		}

		// Token: 0x0400002D RID: 45
		[SerializeField]
		private TableReference m_TableReference;

		// Token: 0x0400002E RID: 46
		private CallbackArray<LocalizedTable<TTable, TEntry>.ChangeHandler> m_ChangeHandler;

		// Token: 0x0400002F RID: 47
		private Action<Locale> m_SelectedLocaleChanged;

		// Token: 0x02000119 RID: 281
		// (Invoke) Token: 0x06000705 RID: 1797
		public delegate void ChangeHandler(TTable value);
	}
}
