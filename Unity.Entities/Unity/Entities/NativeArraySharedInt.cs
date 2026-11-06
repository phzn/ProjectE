using System;
using System.Diagnostics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x02000292 RID: 658
	public struct NativeArraySharedInt : IDisposable
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06001162 RID: 4450 RVA: 0x00049769 File Offset: 0x00047969
		public NativeArray<int> SourceBuffer
		{
			get
			{
				return this.m_SourceBuffer;
			}
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x00049774 File Offset: 0x00047974
		public NativeArraySharedInt(NativeArray<int> sourceBuffer, AllocatorManager.AllocatorHandle allocator)
		{
			this.m_SourceBuffer = sourceBuffer;
			this.m_SourceIndexBySortedSourceIndex = CollectionHelper.CreateNativeArray<int>(sourceBuffer.Length, allocator, NativeArrayOptions.ClearMemory);
			this.m_SortedSourceIndexBySharedIndex = new NativeList<int>(allocator);
			this.m_SharedIndexCountsBySharedIndex = new NativeList<int>(allocator);
			this.m_SharedIndicesBySourceIndex = CollectionHelper.CreateNativeArray<int>(sourceBuffer.Length, allocator, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x000497C8 File Offset: 0x000479C8
		public void Dispose()
		{
			this.m_SourceIndexBySortedSourceIndex.Dispose();
			this.m_SortedSourceIndexBySharedIndex.Dispose();
			this.m_SharedIndexCountsBySharedIndex.Dispose();
			this.m_SharedIndicesBySourceIndex.Dispose();
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x000497F8 File Offset: 0x000479F8
		public JobHandle Schedule(JobHandle inputDeps)
		{
			int length = this.m_SourceBuffer.Length;
			if (length == 0)
			{
				return inputDeps;
			}
			int num = (length + 1023) / 1024;
			NativeArray<IndexedInt> nativeArray = new NativeArray<IndexedInt>(length, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			JobHandle jobHandle = new CopyIndexedInt
			{
				Src = this.m_SourceBuffer,
				Dst = nativeArray
			}.Schedule(length, 1024, inputDeps);
			int threadIndexCount = JobsUtility.ThreadIndexCount;
			int num2 = num / threadIndexCount;
			JobHandle jobHandle2 = new SegmentSortInt
			{
				Data = nativeArray,
				SegmentWidth = 1024
			}.Schedule(num, num2, jobHandle);
			JobHandle jobHandle3 = new SegmentSortMergeInt
			{
				IndexedSourceBuffer = nativeArray,
				SourceIndexBySortedSourceIndex = this.m_SourceIndexBySortedSourceIndex,
				SortedSourceIndexBySharedIndex = this.m_SortedSourceIndexBySharedIndex,
				SharedIndexCountsBySharedIndex = this.m_SharedIndexCountsBySharedIndex,
				SharedIndicesBySourceIndex = this.m_SharedIndicesBySourceIndex,
				SegmentWidth = 1024
			}.Schedule(jobHandle2);
			return nativeArray.Dispose(jobHandle3);
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x000498F8 File Offset: 0x00047AF8
		public NativeArray<int> GetSortedIndices()
		{
			return this.m_SourceIndexBySortedSourceIndex;
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06001167 RID: 4455 RVA: 0x00049900 File Offset: 0x00047B00
		public int SharedValueCount
		{
			get
			{
				return this.m_SortedSourceIndexBySharedIndex.Length;
			}
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x0004990D File Offset: 0x00047B0D
		public int GetSharedIndexBySourceIndex(int indexIntoSourceBuffer)
		{
			return this.m_SharedIndicesBySourceIndex[indexIntoSourceBuffer];
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x0004991B File Offset: 0x00047B1B
		public NativeArray<int> GetSharedIndexArray()
		{
			return this.m_SharedIndicesBySourceIndex;
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x00049924 File Offset: 0x00047B24
		public NativeArray<int> GetSharedValueIndicesBySourceIndex(int indexIntoSourceBuffer)
		{
			int num = this.m_SharedIndicesBySourceIndex[indexIntoSourceBuffer];
			return this.GetSharedValueIndicesBySharedIndex(num);
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x00049948 File Offset: 0x00047B48
		public int GetSharedValueIndexCountBySourceIndex(int indexIntoSourceBuffer)
		{
			int num = this.m_SharedIndicesBySourceIndex[indexIntoSourceBuffer];
			return this.m_SharedIndexCountsBySharedIndex[num];
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x0004996E File Offset: 0x00047B6E
		public NativeArray<int> GetSharedValueIndexCountArray()
		{
			return this.m_SharedIndexCountsBySharedIndex.AsArray();
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x0004997C File Offset: 0x00047B7C
		public unsafe NativeArray<int> GetSharedValueIndicesBySharedIndex(int sharedValueIndex)
		{
			int num = this.m_SortedSourceIndexBySharedIndex[sharedValueIndex];
			int num2 = this.m_SharedIndexCountsBySharedIndex[sharedValueIndex];
			return NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<int>((void*)((byte*)NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<int>(this.m_SourceIndexBySortedSourceIndex) + (IntPtr)num * 4), num2, Allocator.None);
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x00002287 File Offset: 0x00000487
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		private void SharedValueIndicesSetSafetyHandle(ref NativeArray<int> arr)
		{
		}

		// Token: 0x040006AD RID: 1709
		[ReadOnly]
		private readonly NativeArray<int> m_SourceBuffer;

		// Token: 0x040006AE RID: 1710
		private NativeArray<int> m_SourceIndexBySortedSourceIndex;

		// Token: 0x040006AF RID: 1711
		private NativeList<int> m_SortedSourceIndexBySharedIndex;

		// Token: 0x040006B0 RID: 1712
		private NativeList<int> m_SharedIndexCountsBySharedIndex;

		// Token: 0x040006B1 RID: 1713
		private NativeArray<int> m_SharedIndicesBySourceIndex;
	}
}
