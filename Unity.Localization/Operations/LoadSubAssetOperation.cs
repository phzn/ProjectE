using System;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization.Operations
{
	// Token: 0x020000C0 RID: 192
	internal class LoadSubAssetOperation<TObject> : WaitForCurrentOperationAsyncOperationBase<TObject> where TObject : Object
	{
		// Token: 0x0600056E RID: 1390 RVA: 0x000134D7 File Offset: 0x000116D7
		public LoadSubAssetOperation()
		{
			this.m_AssetLoadedAction = new Action<AsyncOperationHandle<TObject>>(this.AssetLoaded);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x000134F4 File Offset: 0x000116F4
		public void Init(AsyncOperationHandle<Object[]> preloadOperations, string address, bool isSubAsset, string subAssetName)
		{
			base.Dependency = preloadOperations;
			this.m_PreloadOperations = preloadOperations;
			if (this.m_PreloadOperations.IsValid())
			{
				AddressablesInterface.Acquire(this.m_PreloadOperations);
			}
			this.m_Address = address;
			this.m_IsSubAsset = isSubAsset;
			this.m_SubAssetName = subAssetName;
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00013548 File Offset: 0x00011748
		protected override void Execute()
		{
			if (this.m_PreloadOperations.IsValid())
			{
				if (this.m_PreloadOperations.Status != AsyncOperationStatus.Succeeded)
				{
					base.Complete(default(TObject), false, this.m_PreloadOperations.OperationException.Message);
					return;
				}
				foreach (Object @object in this.m_PreloadOperations.Result)
				{
					TObject tobject = @object as TObject;
					if (tobject != null && (!this.m_IsSubAsset || !(this.m_SubAssetName != @object.name)))
					{
						base.Complete(tobject, true, null);
						return;
					}
				}
			}
			this.m_AssetOperation = AddressablesInterface.LoadAssetFromGUID<TObject>(this.m_Address);
			if (this.m_AssetOperation.IsDone)
			{
				this.AssetLoaded(this.m_AssetOperation);
				return;
			}
			base.CurrentOperation = this.m_AssetOperation;
			this.m_AssetOperation.Completed += this.m_AssetLoadedAction;
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0001363C File Offset: 0x0001183C
		private void AssetLoaded(AsyncOperationHandle<TObject> handle)
		{
			if (handle.Status != AsyncOperationStatus.Succeeded)
			{
				base.Complete(default(TObject), false, string.Format("Failed to load sub-asset {0} from the address {1}.", this.m_SubAssetName, this.m_Address));
				return;
			}
			base.Complete(handle.Result, true, null);
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00013689 File Offset: 0x00011889
		protected override void Destroy()
		{
			AddressablesInterface.ReleaseAndReset<Object[]>(ref this.m_PreloadOperations);
			AddressablesInterface.ReleaseAndReset<TObject>(ref this.m_AssetOperation);
			base.Destroy();
			LoadSubAssetOperation<TObject>.Pool.Release(this);
		}

		// Token: 0x040001BE RID: 446
		private readonly Action<AsyncOperationHandle<TObject>> m_AssetLoadedAction;

		// Token: 0x040001BF RID: 447
		private AsyncOperationHandle<TObject> m_AssetOperation;

		// Token: 0x040001C0 RID: 448
		private AsyncOperationHandle<Object[]> m_PreloadOperations;

		// Token: 0x040001C1 RID: 449
		private string m_Address;

		// Token: 0x040001C2 RID: 450
		private bool m_IsSubAsset;

		// Token: 0x040001C3 RID: 451
		private string m_SubAssetName;

		// Token: 0x040001C4 RID: 452
		public static readonly ObjectPool<LoadSubAssetOperation<TObject>> Pool = new ObjectPool<LoadSubAssetOperation<TObject>>(() => new LoadSubAssetOperation<TObject>(), null, null, null, false, 10, 10000);
	}
}
