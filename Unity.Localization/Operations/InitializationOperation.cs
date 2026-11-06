using System;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace UnityEngine.Localization.Operations
{
	// Token: 0x020000BD RID: 189
	internal class InitializationOperation : WaitForCurrentOperationAsyncOperationBase<LocalizationSettings>
	{
		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000550 RID: 1360 RVA: 0x00012CB0 File Offset: 0x00010EB0
		protected override float Progress
		{
			get
			{
				if (base.CurrentOperation.IsValid())
				{
					return ((float)(3 - this.m_RemainingSteps) + base.CurrentOperation.PercentComplete) / 4f;
				}
				return base.Progress;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000551 RID: 1361 RVA: 0x00012CF2 File Offset: 0x00010EF2
		protected override string DebugName
		{
			get
			{
				return "Localization Settings Initialization";
			}
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00012CFC File Offset: 0x00010EFC
		public InitializationOperation()
		{
			this.m_LoadLocalesCompletedAction = new Action<AsyncOperationHandle<Locale>>(this.LoadLocalesCompleted);
			this.m_FinishPreloadingTablesAction = delegate(AsyncOperationHandle _)
			{
				this.PreloadTablesCompleted();
			};
			this.m_LoadLocales = delegate(AsyncOperationHandle _)
			{
				this.LoadLocales();
			};
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00012D50 File Offset: 0x00010F50
		public void Init(LocalizationSettings settings)
		{
			this.m_Settings = settings;
			this.m_LoadDatabasesOperations.Clear();
			this.m_RemainingSteps = 3;
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00012D6C File Offset: 0x00010F6C
		protected override void Execute()
		{
			InitializationOperation.UnloadBundlesOperation unloadBundlesOperation = GenericPool<InitializationOperation.UnloadBundlesOperation>.Get();
			this.m_UnloadBundlesOperationHandle = AddressablesInterface.ResourceManager.StartOperation<object>(unloadBundlesOperation, default(AsyncOperationHandle));
			if (!this.m_UnloadBundlesOperationHandle.IsDone)
			{
				base.CurrentOperation = this.m_UnloadBundlesOperationHandle;
				this.m_UnloadBundlesOperationHandle.Completed += this.m_LoadLocales;
				return;
			}
			this.LoadLocales();
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00012DD0 File Offset: 0x00010FD0
		private void LoadLocales()
		{
			AddressablesInterface.SafeRelease(this.m_UnloadBundlesOperationHandle);
			AsyncOperationHandle<Locale> selectedLocaleAsync = this.m_Settings.GetSelectedLocaleAsync();
			if (!selectedLocaleAsync.IsDone)
			{
				base.CurrentOperation = selectedLocaleAsync;
				selectedLocaleAsync.Completed += this.m_LoadLocalesCompletedAction;
				return;
			}
			this.LoadLocalesCompleted(selectedLocaleAsync);
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00012E1E File Offset: 0x0001101E
		private bool CheckOperationSucceeded(AsyncOperationHandle handle, string errorMessage)
		{
			if (handle.Status != AsyncOperationStatus.Succeeded)
			{
				bool flag = false;
				Exception operationException = handle.OperationException;
				this.FinishInitializing(flag, string.Format(errorMessage, (operationException != null) ? operationException.Message : null));
				return false;
			}
			return true;
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00012E4D File Offset: 0x0001104D
		private void LoadLocalesCompleted(AsyncOperationHandle<Locale> operationHandle)
		{
			if (this.CheckOperationSucceeded(operationHandle, "Failed to initialize localization, could not load the selected locale.\n{0}"))
			{
				this.PreloadTables();
			}
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00012E68 File Offset: 0x00011068
		private void PreloadTables()
		{
			this.m_RemainingSteps--;
			IPreloadRequired assetDatabase = this.m_Settings.GetAssetDatabase();
			if (assetDatabase != null && !assetDatabase.PreloadOperation.IsDone)
			{
				this.m_LoadDatabasesOperations.Add(assetDatabase.PreloadOperation);
			}
			else
			{
				this.m_RemainingSteps--;
			}
			IPreloadRequired stringDatabase = this.m_Settings.GetStringDatabase();
			if (stringDatabase != null && !stringDatabase.PreloadOperation.IsDone)
			{
				this.m_LoadDatabasesOperations.Add(stringDatabase.PreloadOperation);
			}
			else
			{
				this.m_RemainingSteps--;
			}
			if (this.m_LoadDatabasesOperations.Count > 0)
			{
				this.m_PreloadDatabasesOperation = AddressablesInterface.CreateGroupOperation(this.m_LoadDatabasesOperations);
				base.CurrentOperation = this.m_PreloadDatabasesOperation;
				this.m_PreloadDatabasesOperation.CompletedTypeless += this.m_FinishPreloadingTablesAction;
				return;
			}
			this.PreloadTablesCompleted();
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00012F4C File Offset: 0x0001114C
		private void PreloadTablesCompleted()
		{
			IPreloadRequired assetDatabase = this.m_Settings.GetAssetDatabase();
			if (assetDatabase != null && !this.CheckOperationSucceeded(assetDatabase.PreloadOperation, "Failed to initialize localization, could not preload asset tables.\n{0}"))
			{
				return;
			}
			IPreloadRequired stringDatabase = this.m_Settings.GetStringDatabase();
			if (stringDatabase != null && !this.CheckOperationSucceeded(stringDatabase.PreloadOperation, "Failed to initialize localization, could not preload string tables.\n{0}"))
			{
				return;
			}
			this.FinishInitializing(true, null);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00012FA8 File Offset: 0x000111A8
		private void PostInitializeExtensions()
		{
			foreach (IStartupLocaleSelector startupLocaleSelector in this.m_Settings.GetStartupLocaleSelectors())
			{
				IInitialize initialize = startupLocaleSelector as IInitialize;
				if (initialize != null)
				{
					initialize.PostInitialization(this.m_Settings);
				}
			}
			IInitialize initialize2 = this.m_Settings.GetAvailableLocales() as IInitialize;
			if (initialize2 != null)
			{
				initialize2.PostInitialization(this.m_Settings);
			}
			IInitialize initialize3 = this.m_Settings.GetAssetDatabase() as IInitialize;
			if (initialize3 != null)
			{
				initialize3.PostInitialization(this.m_Settings);
			}
			IInitialize initialize4 = this.m_Settings.GetStringDatabase() as IInitialize;
			if (initialize4 == null)
			{
				return;
			}
			initialize4.PostInitialization(this.m_Settings);
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x00013070 File Offset: 0x00011270
		private void FinishInitializing(AsyncOperationHandle op)
		{
			bool flag = op.Status == AsyncOperationStatus.Succeeded;
			Exception operationException = op.OperationException;
			this.FinishInitializing(flag, (operationException != null) ? operationException.Message : null);
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00013095 File Offset: 0x00011295
		private void FinishInitializing(bool success, string error)
		{
			AddressablesInterface.ReleaseAndReset<IList<AsyncOperationHandle>>(ref this.m_PreloadDatabasesOperation);
			this.PostInitializeExtensions();
			base.Complete(this.m_Settings, success, error);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x000130B6 File Offset: 0x000112B6
		protected override void Destroy()
		{
			base.Destroy();
			InitializationOperation.Pool.Release(this);
		}

		// Token: 0x040001A7 RID: 423
		private AsyncOperationHandle m_UnloadBundlesOperationHandle;

		// Token: 0x040001A8 RID: 424
		private readonly Action<AsyncOperationHandle> m_LoadLocales;

		// Token: 0x040001A9 RID: 425
		internal const string k_LocaleError = "Failed to initialize localization, could not load the selected locale.\n{0}";

		// Token: 0x040001AA RID: 426
		internal const string k_PreloadAssetTablesError = "Failed to initialize localization, could not preload asset tables.\n{0}";

		// Token: 0x040001AB RID: 427
		internal const string k_PreloadStringTablesError = "Failed to initialize localization, could not preload string tables.\n{0}";

		// Token: 0x040001AC RID: 428
		private readonly Action<AsyncOperationHandle<Locale>> m_LoadLocalesCompletedAction;

		// Token: 0x040001AD RID: 429
		private readonly Action<AsyncOperationHandle> m_FinishPreloadingTablesAction;

		// Token: 0x040001AE RID: 430
		private LocalizationSettings m_Settings;

		// Token: 0x040001AF RID: 431
		private readonly List<AsyncOperationHandle> m_LoadDatabasesOperations = new List<AsyncOperationHandle>();

		// Token: 0x040001B0 RID: 432
		private AsyncOperationHandle<IList<AsyncOperationHandle>> m_PreloadDatabasesOperation;

		// Token: 0x040001B1 RID: 433
		private int m_RemainingSteps;

		// Token: 0x040001B2 RID: 434
		private const int k_PreloadSteps = 3;

		// Token: 0x040001B3 RID: 435
		public static readonly ObjectPool<InitializationOperation> Pool = new ObjectPool<InitializationOperation>(() => new InitializationOperation(), null, null, null, false, 10, 10000);

		// Token: 0x02000154 RID: 340
		private class UnloadBundlesOperation : AsyncOperationBase<object>
		{
			// Token: 0x060007F2 RID: 2034 RVA: 0x000194FC File Offset: 0x000176FC
			public UnloadBundlesOperation()
			{
				this.m_OperationCompleted = new Action<AsyncOperation>(this.OnOperationCompleted);
			}

			// Token: 0x060007F3 RID: 2035 RVA: 0x00019524 File Offset: 0x00017724
			protected override void Execute()
			{
				if (AssetBundleProvider.UnloadingAssetBundleCount == 0)
				{
					base.Complete(null, true, null);
					return;
				}
				this.m_UnloadBundleOperations.Clear();
				foreach (AssetBundleUnloadOperation assetBundleUnloadOperation in AssetBundleProvider.UnloadingBundles.Values)
				{
					if (!assetBundleUnloadOperation.isDone)
					{
						this.m_UnloadBundleOperations.Add(assetBundleUnloadOperation);
						assetBundleUnloadOperation.completed += this.m_OperationCompleted;
					}
				}
			}

			// Token: 0x060007F4 RID: 2036 RVA: 0x000195B0 File Offset: 0x000177B0
			private void OnOperationCompleted(AsyncOperation obj)
			{
				this.m_UnloadBundleOperations.Remove(obj);
				if (this.m_UnloadBundleOperations.Count == 0)
				{
					base.Complete(null, true, null);
				}
			}

			// Token: 0x060007F5 RID: 2037 RVA: 0x000195D5 File Offset: 0x000177D5
			protected override bool InvokeWaitForCompletion()
			{
				AssetBundleProvider.WaitForAllUnloadingBundlesToComplete();
				return true;
			}

			// Token: 0x060007F6 RID: 2038 RVA: 0x000195DD File Offset: 0x000177DD
			protected override void Destroy()
			{
				GenericPool<InitializationOperation.UnloadBundlesOperation>.Release(this);
			}

			// Token: 0x04000307 RID: 775
			private readonly Action<AsyncOperation> m_OperationCompleted;

			// Token: 0x04000308 RID: 776
			private readonly List<AsyncOperation> m_UnloadBundleOperations = new List<AsyncOperation>();
		}
	}
}
