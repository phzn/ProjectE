using System;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization.Operations
{
	// Token: 0x020000BF RID: 191
	internal class LoadAssetOperation<TObject> : WaitForCurrentOperationAsyncOperationBase<TObject> where TObject : Object
	{
		// Token: 0x06000567 RID: 1383 RVA: 0x000132E1 File Offset: 0x000114E1
		public LoadAssetOperation()
		{
			this.m_AssetLoadedAction = new Action<AsyncOperationHandle<TObject>>(this.AssetLoaded);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x000132FB File Offset: 0x000114FB
		public void Init(AsyncOperationHandle<LocalizedDatabase<AssetTable, AssetTableEntry>.TableEntryResult> loadTableEntryOperation, bool autoRelease)
		{
			this.m_TableEntryOperation = loadTableEntryOperation;
			AddressablesInterface.Acquire(this.m_TableEntryOperation);
			this.m_AutoRelease = autoRelease;
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x0001331C File Offset: 0x0001151C
		protected override void Execute()
		{
			if (this.m_TableEntryOperation.Status != AsyncOperationStatus.Succeeded)
			{
				base.Complete(default(TObject), false, "Load Table Entry Operation Failed");
				AddressablesInterface.Release(this.m_TableEntryOperation);
				return;
			}
			if (this.m_TableEntryOperation.Result.Table == null || this.m_TableEntryOperation.Result.Entry == null)
			{
				this.CompleteAndRelease(default(TObject), true, null);
				return;
			}
			this.m_LoadAssetOperation = this.m_TableEntryOperation.Result.Table.GetAssetAsync<TObject>(this.m_TableEntryOperation.Result.Entry);
			AddressablesInterface.Acquire(this.m_LoadAssetOperation);
			if (this.m_LoadAssetOperation.IsDone)
			{
				this.AssetLoaded(this.m_LoadAssetOperation);
				return;
			}
			base.CurrentOperation = this.m_LoadAssetOperation;
			this.m_LoadAssetOperation.Completed += this.m_AssetLoadedAction;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001341C File Offset: 0x0001161C
		private void AssetLoaded(AsyncOperationHandle<TObject> handle)
		{
			if (handle.Status != AsyncOperationStatus.Succeeded)
			{
				this.CompleteAndRelease(default(TObject), false, "GetAssetAsync failed to load the asset.");
				return;
			}
			this.CompleteAndRelease(handle.Result, true, null);
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00013458 File Offset: 0x00011658
		public void CompleteAndRelease(TObject result, bool success, string errorMsg)
		{
			base.Complete(result, success, errorMsg);
			AddressablesInterface.Release(this.m_TableEntryOperation);
			if (this.m_AutoRelease && PlaymodeState.IsPlaying)
			{
				LocalizationBehaviour.ReleaseNextFrame(base.Handle);
			}
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00013492 File Offset: 0x00011692
		protected override void Destroy()
		{
			AddressablesInterface.ReleaseAndReset<TObject>(ref this.m_LoadAssetOperation);
			base.Destroy();
			LoadAssetOperation<TObject>.Pool.Release(this);
		}

		// Token: 0x040001B9 RID: 441
		private readonly Action<AsyncOperationHandle<TObject>> m_AssetLoadedAction;

		// Token: 0x040001BA RID: 442
		private AsyncOperationHandle<LocalizedDatabase<AssetTable, AssetTableEntry>.TableEntryResult> m_TableEntryOperation;

		// Token: 0x040001BB RID: 443
		private AsyncOperationHandle<TObject> m_LoadAssetOperation;

		// Token: 0x040001BC RID: 444
		private bool m_AutoRelease;

		// Token: 0x040001BD RID: 445
		public static readonly ObjectPool<LoadAssetOperation<TObject>> Pool = new ObjectPool<LoadAssetOperation<TObject>>(() => new LoadAssetOperation<TObject>(), null, null, null, false, 10, 10000);
	}
}
