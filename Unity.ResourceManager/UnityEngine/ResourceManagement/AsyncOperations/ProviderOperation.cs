using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.Util;
using UnityEngine.Scripting;

namespace UnityEngine.ResourceManagement.AsyncOperations
{
	// Token: 0x0200005F RID: 95
	[Preserve]
	internal class ProviderOperation<TObject> : AsyncOperationBase<TObject>, IGenericProviderOperation, ICachable
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002DD RID: 733 RVA: 0x000097FC File Offset: 0x000079FC
		// (set) Token: 0x060002DE RID: 734 RVA: 0x00009804 File Offset: 0x00007A04
		IOperationCacheKey ICachable.Key { get; set; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002DF RID: 735 RVA: 0x0000980D File Offset: 0x00007A0D
		public int ProvideHandleVersion
		{
			get
			{
				return this.m_ProvideHandleVersion;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002E0 RID: 736 RVA: 0x00009815 File Offset: 0x00007A15
		public IResourceLocation Location
		{
			get
			{
				return this.m_Location;
			}
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000981D File Offset: 0x00007A1D
		public void SetDownloadProgressCallback(Func<DownloadStatus> callback)
		{
			this.m_GetDownloadProgressCallback = callback;
			if (this.m_GetDownloadProgressCallback != null)
			{
				this.m_DownloadStatus = this.m_GetDownloadProgressCallback();
			}
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000983F File Offset: 0x00007A3F
		public void SetWaitForCompletionCallback(Func<bool> callback)
		{
			this.m_WaitForCompletionCallback = callback;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00009848 File Offset: 0x00007A48
		protected override bool InvokeWaitForCompletion()
		{
			if (base.IsDone || this.m_ProviderCompletedCalled)
			{
				return true;
			}
			if (this.m_DepOp.IsValid() && !this.m_DepOp.IsDone)
			{
				this.m_DepOp.WaitForCompletion();
			}
			ResourceManager rm = this.m_RM;
			if (rm != null)
			{
				rm.Update(Time.unscaledDeltaTime);
			}
			if (!this.HasExecuted)
			{
				base.InvokeExecute();
			}
			return this.m_WaitForCompletionCallback != null && this.m_WaitForCompletionCallback();
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x000098C8 File Offset: 0x00007AC8
		internal override DownloadStatus GetDownloadStatus(HashSet<object> visited)
		{
			DownloadStatus downloadStatus = (this.m_DepOp.IsValid() ? this.m_DepOp.InternalGetDownloadStatus(visited) : default(DownloadStatus));
			if (this.m_GetDownloadProgressCallback != null)
			{
				this.m_DownloadStatus = this.m_GetDownloadProgressCallback();
			}
			if (base.Status == AsyncOperationStatus.Succeeded)
			{
				this.m_DownloadStatus.DownloadedBytes = this.m_DownloadStatus.TotalBytes;
			}
			return new DownloadStatus
			{
				DownloadedBytes = this.m_DownloadStatus.DownloadedBytes + downloadStatus.DownloadedBytes,
				TotalBytes = this.m_DownloadStatus.TotalBytes + downloadStatus.TotalBytes,
				IsDone = base.IsDone
			};
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00009989 File Offset: 0x00007B89
		public override void GetDependencies(List<AsyncOperationHandle> deps)
		{
			if (this.m_DepOp.IsValid())
			{
				deps.Add(this.m_DepOp);
			}
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x000099A9 File Offset: 0x00007BA9
		internal override void ReleaseDependencies()
		{
			if (this.m_DepOp.IsValid())
			{
				this.m_DepOp.Release();
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x000099C3 File Offset: 0x00007BC3
		protected override string DebugName
		{
			get
			{
				return string.Format("Resource<{0}>({1})", typeof(TObject).Name, (this.m_Location == null) ? "Invalid" : AsyncOperationBase<TObject>.ShortenPath(this.m_Location.InternalId, true));
			}
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x00009A00 File Offset: 0x00007C00
		public void GetDependencies(IList<object> dstList)
		{
			dstList.Clear();
			if (!this.m_DepOp.IsValid())
			{
				return;
			}
			if (this.m_DepOp.Result == null)
			{
				return;
			}
			for (int i = 0; i < this.m_DepOp.Result.Count; i++)
			{
				dstList.Add(this.m_DepOp.Result[i].Result);
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060002EA RID: 746 RVA: 0x00009A69 File Offset: 0x00007C69
		public Type RequestedType
		{
			get
			{
				return typeof(TObject);
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060002EB RID: 747 RVA: 0x00009A75 File Offset: 0x00007C75
		public int DependencyCount
		{
			get
			{
				if (this.m_DepOp.IsValid() && this.m_DepOp.Result != null)
				{
					return this.m_DepOp.Result.Count;
				}
				return 0;
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00009AA4 File Offset: 0x00007CA4
		public TDepObject GetDependency<TDepObject>(int index)
		{
			if (!this.m_DepOp.IsValid() || this.m_DepOp.Result == null)
			{
				throw new Exception("Cannot get dependency because no dependencies were available");
			}
			return (TDepObject)((object)this.m_DepOp.Result[index].Result);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00009AF4 File Offset: 0x00007CF4
		public void SetProgressCallback(Func<float> callback)
		{
			this.m_GetProgressCallback = callback;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00009B00 File Offset: 0x00007D00
		public void ProviderCompleted<T>(T result, bool status, Exception e)
		{
			this.m_ProvideHandleVersion++;
			this.m_GetProgressCallback = null;
			this.m_GetDownloadProgressCallback = null;
			this.m_WaitForCompletionCallback = null;
			this.m_NeedsRelease = status;
			this.m_ProviderCompletedCalled = true;
			ProviderOperation<T> providerOperation = this as ProviderOperation<T>;
			if (providerOperation != null)
			{
				providerOperation.Result = result;
			}
			else if (result == null && !typeof(TObject).IsValueType)
			{
				base.Result = (TObject)((object)null);
			}
			else
			{
				if (result == null || !typeof(TObject).IsAssignableFrom(result.GetType()))
				{
					string text = string.Format("Provider of type {0} with id {1} has provided a result of type {2} which cannot be converted to requested type {3}. The operation will be marked as failed.", new object[]
					{
						this.m_Provider.GetType().ToString(),
						this.m_Provider.ProviderId,
						typeof(T),
						typeof(TObject)
					});
					base.Complete(base.Result, false, text);
					throw new Exception(text);
				}
				base.Result = (TObject)((object)result);
			}
			base.Complete(base.Result, status, e, this.m_ReleaseDependenciesOnFailure);
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060002EF RID: 751 RVA: 0x00009C2C File Offset: 0x00007E2C
		protected override float Progress
		{
			get
			{
				float num3;
				try
				{
					float num = 1f;
					float num2 = 0f;
					if (this.m_GetProgressCallback != null)
					{
						num2 += this.m_GetProgressCallback();
					}
					if (!this.m_DepOp.IsValid() || this.m_DepOp.Result == null || this.m_DepOp.Result.Count == 0)
					{
						num2 += 1f;
						num += 1f;
					}
					else
					{
						foreach (AsyncOperationHandle asyncOperationHandle in this.m_DepOp.Result)
						{
							num2 += asyncOperationHandle.PercentComplete;
							num += 1f;
						}
					}
					num3 = Mathf.Min(num2 / num, 0.99f);
				}
				catch
				{
					num3 = 0f;
				}
				return num3;
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00009D14 File Offset: 0x00007F14
		protected override void Execute()
		{
			if (this.m_DepOp.IsValid() && this.m_DepOp.Status == AsyncOperationStatus.Failed && (this.m_Provider.BehaviourFlags & ProviderBehaviourFlags.CanProvideWithFailedDependencies) == ProviderBehaviourFlags.None)
			{
				this.ProviderCompleted<TObject>(default(TObject), false, new Exception("Dependency Exception", this.m_DepOp.OperationException));
				return;
			}
			try
			{
				this.m_Provider.Provide(new ProvideHandle(this.m_ResourceManager, this));
			}
			catch (Exception ex)
			{
				this.ProviderCompleted<TObject>(default(TObject), false, ex);
			}
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00009DB0 File Offset: 0x00007FB0
		public void Init(ResourceManager rm, IResourceProvider provider, IResourceLocation location, AsyncOperationHandle<IList<AsyncOperationHandle>> depOp)
		{
			this.m_DownloadStatus = default(DownloadStatus);
			this.m_ResourceManager = rm;
			this.m_DepOp = depOp;
			if (this.m_DepOp.IsValid())
			{
				this.m_DepOp.Acquire();
			}
			this.m_Provider = provider;
			this.m_Location = location;
			this.m_ReleaseDependenciesOnFailure = true;
			this.m_ProviderCompletedCalled = false;
			this.SetWaitForCompletionCallback(new Func<bool>(this.WaitForCompletionHandler));
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00009E20 File Offset: 0x00008020
		public void Init(ResourceManager rm, IResourceProvider provider, IResourceLocation location, AsyncOperationHandle<IList<AsyncOperationHandle>> depOp, bool releaseDependenciesOnFailure)
		{
			this.m_DownloadStatus = default(DownloadStatus);
			this.m_ResourceManager = rm;
			this.m_DepOp = depOp;
			if (this.m_DepOp.IsValid())
			{
				this.m_DepOp.Acquire();
			}
			this.m_Provider = provider;
			this.m_Location = location;
			this.m_ReleaseDependenciesOnFailure = releaseDependenciesOnFailure;
			this.m_ProviderCompletedCalled = false;
			this.SetWaitForCompletionCallback(new Func<bool>(this.WaitForCompletionHandler));
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00009E90 File Offset: 0x00008090
		private bool WaitForCompletionHandler()
		{
			if (base.IsDone)
			{
				return true;
			}
			if (!this.m_DepOp.IsDone)
			{
				this.m_DepOp.WaitForCompletion();
			}
			if (!this.HasExecuted)
			{
				base.InvokeExecute();
			}
			return base.IsDone;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00009ECC File Offset: 0x000080CC
		protected override void Destroy()
		{
			if (this.m_NeedsRelease)
			{
				this.m_Provider.Release(this.m_Location, base.Result);
			}
			if (this.m_DepOp.IsValid())
			{
				this.m_DepOp.Release();
			}
			base.Result = default(TObject);
			this.m_Location = null;
		}

		// Token: 0x0400010F RID: 271
		private bool m_ReleaseDependenciesOnFailure = true;

		// Token: 0x04000110 RID: 272
		private Func<float> m_GetProgressCallback;

		// Token: 0x04000111 RID: 273
		private Func<DownloadStatus> m_GetDownloadProgressCallback;

		// Token: 0x04000112 RID: 274
		private Func<bool> m_WaitForCompletionCallback;

		// Token: 0x04000113 RID: 275
		private bool m_ProviderCompletedCalled;

		// Token: 0x04000114 RID: 276
		private DownloadStatus m_DownloadStatus;

		// Token: 0x04000115 RID: 277
		private IResourceProvider m_Provider;

		// Token: 0x04000116 RID: 278
		internal AsyncOperationHandle<IList<AsyncOperationHandle>> m_DepOp;

		// Token: 0x04000117 RID: 279
		private IResourceLocation m_Location;

		// Token: 0x04000118 RID: 280
		private int m_ProvideHandleVersion;

		// Token: 0x04000119 RID: 281
		private bool m_NeedsRelease;

		// Token: 0x0400011B RID: 283
		private ResourceManager m_ResourceManager;

		// Token: 0x0400011C RID: 284
		private const float k_OperationWaitingToCompletePercentComplete = 0.99f;

		// Token: 0x0400011D RID: 285
		internal const string kInvalidHandleMsg = "The ProvideHandle is invalid. After the handle has been completed, it can no longer be used";
	}
}
