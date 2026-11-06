using System;
using Unity.Collections;
using Unity.Jobs;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000449 RID: 1097
	internal class JobMerger : IDisposable
	{
		// Token: 0x06002274 RID: 8820 RVA: 0x00084BAB File Offset: 0x00082DAB
		public JobMerger(int capacity)
		{
			Debug.Assert(capacity > 1);
			this.m_Jobs = new NativeArray<JobHandle>(capacity, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x00084BD0 File Offset: 0x00082DD0
		public void Add(JobHandle job)
		{
			bool flag = this.m_JobCount < this.m_Jobs.Length;
			if (flag)
			{
				int jobCount = this.m_JobCount;
				this.m_JobCount = jobCount + 1;
				this.m_Jobs[jobCount] = job;
			}
			else
			{
				this.m_Jobs[0] = JobHandle.CombineDependencies(this.m_Jobs);
				this.m_Jobs[1] = job;
				this.m_JobCount = 2;
			}
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x00084C44 File Offset: 0x00082E44
		public JobHandle MergeAndReset()
		{
			JobHandle jobHandle = default(JobHandle);
			bool flag = this.m_JobCount > 1;
			if (flag)
			{
				jobHandle = JobHandle.CombineDependencies(this.m_Jobs.Slice<JobHandle>(0, this.m_JobCount));
			}
			else
			{
				bool flag2 = this.m_JobCount == 1;
				if (flag2)
				{
					jobHandle = this.m_Jobs[0];
				}
			}
			this.m_JobCount = 0;
			return jobHandle;
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06002277 RID: 8823 RVA: 0x00084CA7 File Offset: 0x00082EA7
		// (set) Token: 0x06002278 RID: 8824 RVA: 0x00084CAF File Offset: 0x00082EAF
		private protected bool disposed { protected get; private set; }

		// Token: 0x06002279 RID: 8825 RVA: 0x00084CB8 File Offset: 0x00082EB8
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x00084CCC File Offset: 0x00082ECC
		protected void Dispose(bool disposing)
		{
			bool disposed = this.disposed;
			if (!disposed)
			{
				if (disposing)
				{
					this.m_Jobs.Dispose();
				}
				this.disposed = true;
			}
		}

		// Token: 0x04000F61 RID: 3937
		private NativeArray<JobHandle> m_Jobs;

		// Token: 0x04000F62 RID: 3938
		private int m_JobCount;
	}
}
