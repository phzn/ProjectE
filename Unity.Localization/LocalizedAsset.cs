using System;
using UnityEngine.Localization.Operations;
using UnityEngine.Localization.Settings;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization
{
	// Token: 0x02000013 RID: 19
	[Serializable]
	public class LocalizedAsset<TObject> : LocalizedAssetBase, IDisposable where TObject : Object
	{
		// Token: 0x1700000E RID: 14
		// (set) Token: 0x06000061 RID: 97 RVA: 0x00002808 File Offset: 0x00000A08
		public override bool WaitForCompletion
		{
			set
			{
				if (value == this.WaitForCompletion)
				{
					return;
				}
				base.WaitForCompletion = value;
				if (value && this.CurrentLoadingOperationHandle.IsValid() && !this.CurrentLoadingOperationHandle.IsDone)
				{
					this.CurrentLoadingOperationHandle.WaitForCompletion();
				}
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00002858 File Offset: 0x00000A58
		internal override bool ForceSynchronous
		{
			get
			{
				return this.WaitForCompletion || LocalizationSettings.AssetDatabase.AsynchronousBehaviour == AsynchronousBehaviour.ForceSynchronous;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00002871 File Offset: 0x00000A71
		// (set) Token: 0x06000064 RID: 100 RVA: 0x00002879 File Offset: 0x00000A79
		public AsyncOperationHandle<TObject> CurrentLoadingOperationHandle { get; internal set; }

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000065 RID: 101 RVA: 0x00002884 File Offset: 0x00000A84
		// (remove) Token: 0x06000066 RID: 102 RVA: 0x00002905 File Offset: 0x00000B05
		public event LocalizedAsset<TObject>.ChangeHandler AssetChanged
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
					this.ForceUpdate();
					LocalizationSettings.SelectedLocaleChanged += this.m_SelectedLocaleChanged;
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

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00002931 File Offset: 0x00000B31
		public bool HasChangeHandler
		{
			get
			{
				return this.m_ChangeHandler.Length != 0;
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002941 File Offset: 0x00000B41
		public LocalizedAsset()
		{
			this.m_SelectedLocaleChanged = new Action<Locale>(this.HandleLocaleChange);
			this.m_AutomaticLoadingCompleted = new Action<AsyncOperationHandle<TObject>>(this.AutomaticLoadingCompleted);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000296D File Offset: 0x00000B6D
		public AsyncOperationHandle<TObject> LoadAssetAsync()
		{
			return this.LoadAssetAsync<TObject>();
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002975 File Offset: 0x00000B75
		public override AsyncOperationHandle<T> LoadAssetAsync<T>()
		{
			LocalizationSettings.ValidateSettingsExist("Can not Load Asset.");
			return LocalizationSettings.AssetDatabase.GetLocalizedAssetAsync<T>(base.TableReference, base.TableEntryReference, base.LocaleOverride, FallbackBehavior.UseProjectSettings);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000029A0 File Offset: 0x00000BA0
		public override AsyncOperationHandle<Object> LoadAssetAsObjectAsync()
		{
			AsyncOperationHandle<TObject> asyncOperationHandle = this.LoadAssetAsync();
			LocalizedAsset<TObject>.ConvertToObjectOperation convertToObjectOperation = LocalizedAsset<TObject>.ConvertToObjectOperation.Pool.Get();
			convertToObjectOperation.Init(asyncOperationHandle);
			return AddressablesInterface.ResourceManager.StartOperation<Object>(convertToObjectOperation, default(AsyncOperationHandle));
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000029DA File Offset: 0x00000BDA
		public TObject LoadAsset()
		{
			return AsyncOperationUtility.SynchronousLoad<TObject>(this.LoadAssetAsync());
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000029E7 File Offset: 0x00000BE7
		protected internal override void ForceUpdate()
		{
			if (this.m_ChangeHandler.Length != 0)
			{
				this.HandleLocaleChange(null);
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00002A00 File Offset: 0x00000C00
		private void HandleLocaleChange(Locale locale)
		{
			if (base.IsEmpty)
			{
				this.ClearLoadingOperation();
				return;
			}
			this.m_PreviousLoadingOperation = this.CurrentLoadingOperationHandle;
			this.CurrentLoadingOperationHandle = this.LoadAssetAsync();
			AddressablesInterface.Acquire(this.CurrentLoadingOperationHandle);
			if (!this.CurrentLoadingOperationHandle.IsDone)
			{
				if (!this.WaitForCompletion && LocalizationSettings.AssetDatabase.AsynchronousBehaviour != AsynchronousBehaviour.ForceSynchronous)
				{
					this.CurrentLoadingOperationHandle.Completed += this.m_AutomaticLoadingCompleted;
					return;
				}
				this.CurrentLoadingOperationHandle.WaitForCompletion();
			}
			this.AutomaticLoadingCompleted(this.CurrentLoadingOperationHandle);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002A9C File Offset: 0x00000C9C
		private void AutomaticLoadingCompleted(AsyncOperationHandle<TObject> loadOperation)
		{
			if (loadOperation.Status != AsyncOperationStatus.Succeeded)
			{
				this.CurrentLoadingOperationHandle = default(AsyncOperationHandle<TObject>);
				return;
			}
			this.InvokeChangeHandler(loadOperation.Result);
			this.ClearPreviousLoadingOperation();
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002AD8 File Offset: 0x00000CD8
		private void InvokeChangeHandler(TObject value)
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
					LocalizedAsset<TObject>.ChangeHandler[] multiDelegates = this.m_ChangeHandler.MultiDelegates;
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

		// Token: 0x06000071 RID: 113 RVA: 0x00002B5C File Offset: 0x00000D5C
		internal void ClearLoadingOperation()
		{
			this.ClearLoadingOperation(this.CurrentLoadingOperationHandle);
			this.CurrentLoadingOperationHandle = default(AsyncOperationHandle<TObject>);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00002B84 File Offset: 0x00000D84
		private void ClearPreviousLoadingOperation()
		{
			this.ClearLoadingOperation(this.m_PreviousLoadingOperation);
			this.m_PreviousLoadingOperation = default(AsyncOperationHandle<TObject>);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00002B9E File Offset: 0x00000D9E
		private void ClearLoadingOperation(AsyncOperationHandle<TObject> operationHandle)
		{
			if (operationHandle.IsValid())
			{
				if (!operationHandle.IsDone)
				{
					operationHandle.Completed -= this.m_AutomaticLoadingCompleted;
				}
				AddressablesInterface.Release(operationHandle);
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00002BCA File Offset: 0x00000DCA
		protected override void Reset()
		{
			this.ClearLoadingOperation();
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00002BD4 File Offset: 0x00000DD4
		~LocalizedAsset()
		{
			this.ClearLoadingOperation();
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00002C00 File Offset: 0x00000E00
		void IDisposable.Dispose()
		{
			this.m_ChangeHandler.Clear();
			LocalizationSettings.SelectedLocaleChanged -= this.m_SelectedLocaleChanged;
			this.ClearLoadingOperation();
			GC.SuppressFinalize(this);
		}

		// Token: 0x0400000D RID: 13
		private CallbackArray<LocalizedAsset<TObject>.ChangeHandler> m_ChangeHandler;

		// Token: 0x0400000E RID: 14
		private Action<Locale> m_SelectedLocaleChanged;

		// Token: 0x0400000F RID: 15
		private Action<AsyncOperationHandle<TObject>> m_AutomaticLoadingCompleted;

		// Token: 0x04000010 RID: 16
		private AsyncOperationHandle<TObject> m_PreviousLoadingOperation;

		// Token: 0x0200010E RID: 270
		// (Invoke) Token: 0x060006D7 RID: 1751
		public delegate void ChangeHandler(TObject value);

		// Token: 0x0200010F RID: 271
		private class ConvertToObjectOperation : WaitForCurrentOperationAsyncOperationBase<Object>
		{
			// Token: 0x060006DA RID: 1754 RVA: 0x0001758B File Offset: 0x0001578B
			public void Init(AsyncOperationHandle<TObject> operation)
			{
				AddressablesInterface.ResourceManager.Acquire<TObject>(operation);
				this.m_Operation = operation;
				base.CurrentOperation = operation;
			}

			// Token: 0x060006DB RID: 1755 RVA: 0x000175AC File Offset: 0x000157AC
			protected override void Execute()
			{
				if (this.m_Operation.IsDone)
				{
					this.OnCompleted(this.m_Operation);
					return;
				}
				this.m_Operation.Completed += this.OnCompleted;
			}

			// Token: 0x060006DC RID: 1756 RVA: 0x000175DF File Offset: 0x000157DF
			private void OnCompleted(AsyncOperationHandle<TObject> op)
			{
				base.Complete(op.Result, op.Status == AsyncOperationStatus.Succeeded, null);
			}

			// Token: 0x060006DD RID: 1757 RVA: 0x000175FE File Offset: 0x000157FE
			protected override void Destroy()
			{
				AddressablesInterface.Release(this.m_Operation);
				LocalizedAsset<TObject>.ConvertToObjectOperation.Pool.Release(this);
			}

			// Token: 0x04000253 RID: 595
			public static readonly ObjectPool<LocalizedAsset<TObject>.ConvertToObjectOperation> Pool = new ObjectPool<LocalizedAsset<TObject>.ConvertToObjectOperation>(() => new LocalizedAsset<TObject>.ConvertToObjectOperation(), null, null, null, false, 10, 10000);

			// Token: 0x04000254 RID: 596
			private AsyncOperationHandle<TObject> m_Operation;
		}
	}
}
