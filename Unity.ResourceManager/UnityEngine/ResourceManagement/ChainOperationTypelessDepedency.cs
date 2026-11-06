using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.Exceptions;

namespace UnityEngine.ResourceManagement
{
	// Token: 0x02000009 RID: 9
	internal class ChainOperationTypelessDepedency<TObject> : AsyncOperationBase<TObject>
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002979 File Offset: 0x00000B79
		internal AsyncOperationHandle<TObject> WrappedOp
		{
			get
			{
				return this.m_WrappedOp;
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002981 File Offset: 0x00000B81
		public ChainOperationTypelessDepedency()
		{
			this.m_CachedOnWrappedCompleted = new Action<AsyncOperationHandle<TObject>>(this.OnWrappedCompleted);
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000037 RID: 55 RVA: 0x000029A2 File Offset: 0x00000BA2
		protected override string DebugName
		{
			get
			{
				return "ChainOperation<" + typeof(TObject).Name + "> - " + this.m_DepOp.DebugName;
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000029CD File Offset: 0x00000BCD
		public override void GetDependencies(List<AsyncOperationHandle> deps)
		{
			if (this.m_DepOp.IsValid())
			{
				deps.Add(this.m_DepOp);
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000029E8 File Offset: 0x00000BE8
		public void Init(AsyncOperationHandle dependentOp, Func<AsyncOperationHandle, AsyncOperationHandle<TObject>> callback, bool releaseDependenciesOnFailure)
		{
			this.m_DepOp = dependentOp;
			this.m_DepOp.Acquire();
			this.m_Callback = callback;
			this.m_ReleaseDependenciesOnFailure = releaseDependenciesOnFailure;
			this.RefreshDownloadStatus(null);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002A14 File Offset: 0x00000C14
		protected override bool InvokeWaitForCompletion()
		{
			if (base.IsDone)
			{
				return true;
			}
			if (!this.m_DepOp.IsDone)
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
			if (!this.m_WrappedOp.IsValid())
			{
				return this.m_WrappedOp.IsDone;
			}
			base.Result = this.m_WrappedOp.WaitForCompletion();
			return true;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002A93 File Offset: 0x00000C93
		protected override void Execute()
		{
			this.m_WrappedOp = this.m_Callback(this.m_DepOp);
			this.m_WrappedOp.Completed += this.m_CachedOnWrappedCompleted;
			this.m_Callback = null;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002AC4 File Offset: 0x00000CC4
		private void OnWrappedCompleted(AsyncOperationHandle<TObject> x)
		{
			OperationException ex = null;
			if (x.Status == AsyncOperationStatus.Failed)
			{
				ex = new OperationException("ChainOperation failed because dependent operation failed", x.OperationException);
			}
			base.Complete(this.m_WrappedOp.Result, x.Status == AsyncOperationStatus.Succeeded, ex, this.m_ReleaseDependenciesOnFailure);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002B11 File Offset: 0x00000D11
		protected override void Destroy()
		{
			if (this.m_WrappedOp.IsValid())
			{
				this.m_WrappedOp.Release();
			}
			if (this.m_DepOp.IsValid())
			{
				this.m_DepOp.Release();
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002B43 File Offset: 0x00000D43
		internal override void ReleaseDependencies()
		{
			if (this.m_DepOp.IsValid())
			{
				this.m_DepOp.Release();
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002B60 File Offset: 0x00000D60
		internal override DownloadStatus GetDownloadStatus(HashSet<object> visited)
		{
			this.RefreshDownloadStatus(visited);
			return new DownloadStatus
			{
				DownloadedBytes = this.m_depStatus.DownloadedBytes + this.m_wrapStatus.DownloadedBytes,
				TotalBytes = this.m_depStatus.TotalBytes + this.m_wrapStatus.TotalBytes,
				IsDone = base.IsDone
			};
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002BC8 File Offset: 0x00000DC8
		private void RefreshDownloadStatus(HashSet<object> visited = null)
		{
			this.m_depStatus = (this.m_DepOp.IsValid() ? this.m_DepOp.InternalGetDownloadStatus(visited) : this.m_depStatus);
			this.m_wrapStatus = (this.m_WrappedOp.IsValid() ? this.m_WrappedOp.InternalGetDownloadStatus(visited) : this.m_wrapStatus);
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002C24 File Offset: 0x00000E24
		protected override float Progress
		{
			get
			{
				DownloadStatus downloadStatus = this.GetDownloadStatus(new HashSet<object>());
				if (!downloadStatus.IsDone && downloadStatus.DownloadedBytes == 0L)
				{
					return 0f;
				}
				float num = 0f;
				int num2 = 2;
				if (this.m_DepOp.IsValid())
				{
					num += this.m_DepOp.PercentComplete;
				}
				else
				{
					num += 1f;
				}
				if (this.m_WrappedOp.IsValid())
				{
					num += this.m_WrappedOp.PercentComplete;
				}
				else
				{
					num += 1f;
				}
				return num / (float)num2;
			}
		}

		// Token: 0x04000011 RID: 17
		private AsyncOperationHandle m_DepOp;

		// Token: 0x04000012 RID: 18
		private AsyncOperationHandle<TObject> m_WrappedOp;

		// Token: 0x04000013 RID: 19
		private DownloadStatus m_depStatus;

		// Token: 0x04000014 RID: 20
		private DownloadStatus m_wrapStatus;

		// Token: 0x04000015 RID: 21
		private Func<AsyncOperationHandle, AsyncOperationHandle<TObject>> m_Callback;

		// Token: 0x04000016 RID: 22
		private Action<AsyncOperationHandle<TObject>> m_CachedOnWrappedCompleted;

		// Token: 0x04000017 RID: 23
		private bool m_ReleaseDependenciesOnFailure = true;
	}
}
