using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace UnityEngine.ResourceManagement.ResourceProviders
{
	// Token: 0x0200003A RID: 58
	public struct ProvideHandle
	{
		// Token: 0x0600017C RID: 380 RVA: 0x000069FF File Offset: 0x00004BFF
		internal ProvideHandle(ResourceManager rm, IGenericProviderOperation op)
		{
			this.m_ResourceManager = rm;
			this.m_InternalOp = op;
			this.m_Version = op.ProvideHandleVersion;
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00006A1B File Offset: 0x00004C1B
		internal bool IsValid
		{
			get
			{
				return this.m_InternalOp != null && this.m_InternalOp.ProvideHandleVersion == this.m_Version;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00006A3A File Offset: 0x00004C3A
		internal IGenericProviderOperation InternalOp
		{
			get
			{
				if (this.m_InternalOp.ProvideHandleVersion != this.m_Version)
				{
					throw new Exception("The ProvideHandle is invalid. After the handle has been completed, it can no longer be used");
				}
				return this.m_InternalOp;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600017F RID: 383 RVA: 0x00006A60 File Offset: 0x00004C60
		public ResourceManager ResourceManager
		{
			get
			{
				return this.m_ResourceManager;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00006A68 File Offset: 0x00004C68
		public Type Type
		{
			get
			{
				return this.InternalOp.RequestedType;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00006A75 File Offset: 0x00004C75
		public IResourceLocation Location
		{
			get
			{
				return this.InternalOp.Location;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000182 RID: 386 RVA: 0x00006A82 File Offset: 0x00004C82
		public int DependencyCount
		{
			get
			{
				return this.InternalOp.DependencyCount;
			}
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00006A8F File Offset: 0x00004C8F
		public TDepObject GetDependency<TDepObject>(int index)
		{
			return this.InternalOp.GetDependency<TDepObject>(index);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00006A9D File Offset: 0x00004C9D
		public void GetDependencies(IList<object> list)
		{
			this.InternalOp.GetDependencies(list);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00006AAB File Offset: 0x00004CAB
		public void SetProgressCallback(Func<float> callback)
		{
			this.InternalOp.SetProgressCallback(callback);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00006AB9 File Offset: 0x00004CB9
		public void SetDownloadProgressCallbacks(Func<DownloadStatus> callback)
		{
			this.InternalOp.SetDownloadProgressCallback(callback);
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00006AC7 File Offset: 0x00004CC7
		public void SetWaitForCompletionCallback(Func<bool> callback)
		{
			this.InternalOp.SetWaitForCompletionCallback(callback);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00006AD5 File Offset: 0x00004CD5
		public void Complete<T>(T result, bool status, Exception exception)
		{
			this.InternalOp.ProviderCompleted<T>(result, status, exception);
		}

		// Token: 0x0400009A RID: 154
		private int m_Version;

		// Token: 0x0400009B RID: 155
		private IGenericProviderOperation m_InternalOp;

		// Token: 0x0400009C RID: 156
		private ResourceManager m_ResourceManager;
	}
}
