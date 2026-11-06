using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;

namespace UnityEngine.UIElements.UIR
{
	// Token: 0x02000452 RID: 1106
	internal class OpacityIdAccelerator : IDisposable
	{
		// Token: 0x0600229B RID: 8859 RVA: 0x00085560 File Offset: 0x00083760
		public void CreateJob(NativeSlice<Vertex> oldVerts, NativeSlice<Vertex> newVerts, Color32 opacityData, int vertexCount)
		{
			JobHandle jobHandle = new OpacityIdAccelerator.OpacityIdUpdateJob
			{
				oldVerts = oldVerts,
				newVerts = newVerts,
				opacityData = opacityData
			}.Schedule(vertexCount, 128, default(JobHandle));
			bool flag = this.m_NextJobIndex == this.m_Jobs.Length;
			if (flag)
			{
				this.m_Jobs[0] = JobHandle.CombineDependencies(this.m_Jobs);
				this.m_NextJobIndex = 1;
				JobHandle.ScheduleBatchedJobs();
			}
			int nextJobIndex = this.m_NextJobIndex;
			this.m_NextJobIndex = nextJobIndex + 1;
			this.m_Jobs[nextJobIndex] = jobHandle;
		}

		// Token: 0x0600229C RID: 8860 RVA: 0x00085604 File Offset: 0x00083804
		public void CompleteJobs()
		{
			bool flag = this.m_NextJobIndex > 0;
			if (flag)
			{
				bool flag2 = this.m_NextJobIndex > 1;
				if (flag2)
				{
					JobHandle.CombineDependencies(this.m_Jobs.Slice<JobHandle>(0, this.m_NextJobIndex)).Complete();
				}
				else
				{
					this.m_Jobs[0].Complete();
				}
			}
			this.m_NextJobIndex = 0;
		}

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x0600229D RID: 8861 RVA: 0x0008566C File Offset: 0x0008386C
		// (set) Token: 0x0600229E RID: 8862 RVA: 0x00085674 File Offset: 0x00083874
		private protected bool disposed { protected get; private set; }

		// Token: 0x0600229F RID: 8863 RVA: 0x0008567D File Offset: 0x0008387D
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060022A0 RID: 8864 RVA: 0x00085690 File Offset: 0x00083890
		protected virtual void Dispose(bool disposing)
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

		// Token: 0x04000F7D RID: 3965
		private const int k_VerticesPerBatch = 128;

		// Token: 0x04000F7E RID: 3966
		private const int k_JobLimit = 256;

		// Token: 0x04000F7F RID: 3967
		private NativeArray<JobHandle> m_Jobs = new NativeArray<JobHandle>(256, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);

		// Token: 0x04000F80 RID: 3968
		private int m_NextJobIndex;

		// Token: 0x02000453 RID: 1107
		private struct OpacityIdUpdateJob : IJobParallelFor
		{
			// Token: 0x060022A2 RID: 8866 RVA: 0x000856E0 File Offset: 0x000838E0
			public void Execute(int i)
			{
				Vertex vertex = this.oldVerts[i];
				vertex.opacityColorPages.r = this.opacityData.r;
				vertex.opacityColorPages.g = this.opacityData.g;
				vertex.ids.b = this.opacityData.b;
				this.newVerts[i] = vertex;
			}

			// Token: 0x04000F82 RID: 3970
			[NativeDisableContainerSafetyRestriction]
			public NativeSlice<Vertex> oldVerts;

			// Token: 0x04000F83 RID: 3971
			[NativeDisableContainerSafetyRestriction]
			public NativeSlice<Vertex> newVerts;

			// Token: 0x04000F84 RID: 3972
			public Color32 opacityData;
		}
	}
}
