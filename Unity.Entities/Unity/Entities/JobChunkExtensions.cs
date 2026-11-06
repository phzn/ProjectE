using System;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Entities
{
	// Token: 0x020001AB RID: 427
	public static class JobChunkExtensions
	{
		// Token: 0x06000A1A RID: 2586 RVA: 0x000321FC File Offset: 0x000303FC
		public static void EarlyJobInit<T>() where T : struct, IJobChunk
		{
			JobChunkExtensions.JobChunkProducer<T>.Initialize();
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x00032204 File Offset: 0x00030404
		public static JobHandle Schedule<T>(this T jobData, EntityQuery query, JobHandle dependsOn) where T : struct, IJobChunk
		{
			return JobChunkExtensions.ScheduleInternal<T>(ref jobData, query, dependsOn, ScheduleMode.Single, default(NativeArray<int>));
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x00032224 File Offset: 0x00030424
		public static JobHandle ScheduleByRef<T>(this T jobData, EntityQuery query, JobHandle dependsOn) where T : struct, IJobChunk
		{
			return JobChunkExtensions.ScheduleInternal<T>(ref jobData, query, dependsOn, ScheduleMode.Single, default(NativeArray<int>));
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x00032244 File Offset: 0x00030444
		public static JobHandle ScheduleParallel<T>(this T jobData, EntityQuery query, JobHandle dependsOn) where T : struct, IJobChunk
		{
			return JobChunkExtensions.ScheduleInternal<T>(ref jobData, query, dependsOn, ScheduleMode.Batched, default(NativeArray<int>));
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x00032264 File Offset: 0x00030464
		public static JobHandle ScheduleParallelByRef<T>(this T jobData, EntityQuery query, JobHandle dependsOn) where T : struct, IJobChunk
		{
			return JobChunkExtensions.ScheduleInternal<T>(ref jobData, query, dependsOn, ScheduleMode.Batched, default(NativeArray<int>));
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x00032284 File Offset: 0x00030484
		public static void Run<T>(this T jobData, EntityQuery query) where T : struct, IJobChunk
		{
			JobChunkExtensions.ScheduleInternal<T>(ref jobData, query, default(JobHandle), ScheduleMode.Run, default(NativeArray<int>));
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x000322B0 File Offset: 0x000304B0
		public static void RunByRef<T>(this T jobData, EntityQuery query) where T : struct, IJobChunk
		{
			JobChunkExtensions.ScheduleInternal<T>(ref jobData, query, default(JobHandle), ScheduleMode.Run, default(NativeArray<int>));
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x000322D8 File Offset: 0x000304D8
		internal unsafe static void RunByRefWithoutJobs<T>(this T jobData, EntityQuery query) where T : struct, IJobChunk
		{
			EntityQueryImpl* ptr = query._GetImpl();
			if (ptr->_Access->IsInExclusiveTransaction)
			{
				throw new InvalidOperationException("You can't schedule an IJobChunk while an exclusive transaction is active");
			}
			if (query.HasFilter() || ptr->_QueryData->HasEnableableComponents != 0)
			{
				ptr->SyncFilterTypes();
				UnsafeChunkCacheIterator unsafeChunkCacheIterator = new UnsafeChunkCacheIterator(in ptr->_Filter, ptr->_QueryData->HasEnableableComponents > 0, ptr->GetMatchingChunkCache(), ptr->_QueryData->MatchingArchetypes.Ptr);
				int num = -1;
				v128 v = default(v128);
				ArchetypeChunk archetypeChunk;
				int num2;
				byte b;
				while (unsafeChunkCacheIterator.MoveNextChunk(ref num, out archetypeChunk, out num2, out b, ref v))
				{
					jobData.Execute(in archetypeChunk, num, b > 0, in v);
				}
			}
			else
			{
				UnsafeCachedChunkList matchingChunkCache = ptr->GetMatchingChunkCache();
				ChunkIndex* chunkIndices = matchingChunkCache.ChunkIndices;
				int length = matchingChunkCache.Length;
				ArchetypeChunk archetypeChunk2 = new ArchetypeChunk(ChunkIndex.Null, matchingChunkCache.EntityComponentStore);
				v128 v2 = default(v128);
				for (int i = 0; i < length; i++)
				{
					archetypeChunk2.m_Chunk = chunkIndices[i];
					jobData.Execute(in archetypeChunk2, i, false, in v2);
				}
			}
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x000323FC File Offset: 0x000305FC
		internal unsafe static JobHandle ScheduleInternal<T>(ref T jobData, EntityQuery query, JobHandle dependsOn, ScheduleMode mode, NativeArray<int> chunkBaseEntityIndices) where T : struct, IJobChunk
		{
			EntityQueryImpl* ptr = query._GetImpl();
			if (ptr->_Access->IsInExclusiveTransaction)
			{
				throw new InvalidOperationException("You can't schedule an IJobChunk while an exclusive transaction is active");
			}
			EntityQueryData* queryData = ptr->_QueryData;
			UnsafeCachedChunkList matchingChunkCache = ptr->GetMatchingChunkCache();
			int length = matchingChunkCache.Length;
			bool flag = mode == ScheduleMode.Batched;
			JobChunkExtensions.JobChunkWrapper<T> jobChunkWrapper = new JobChunkExtensions.JobChunkWrapper<T>
			{
				MatchingArchetypes = queryData->MatchingArchetypes,
				CachedChunks = matchingChunkCache,
				Filter = ptr->_Filter,
				JobData = jobData,
				IsParallel = (flag ? 1 : 0),
				QueryHasEnableableComponents = ((queryData->HasEnableableComponents != 0) ? 1 : 0)
			};
			JobChunkExtensions.JobChunkProducer<T>.Initialize();
			IntPtr intPtr = *JobChunkExtensions.JobChunkProducer<T>.reflectionData.Data;
			JobsUtility.JobScheduleParameters jobScheduleParameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf<JobChunkExtensions.JobChunkWrapper<T>>(ref jobChunkWrapper), intPtr, dependsOn, mode);
			JobHandle jobHandle = default(JobHandle);
			if (!flag)
			{
				jobHandle = JobsUtility.Schedule(ref jobScheduleParameters);
			}
			else
			{
				jobHandle = JobsUtility.ScheduleParallelFor(ref jobScheduleParameters, length, 1);
			}
			return jobHandle;
		}

		// Token: 0x020001AC RID: 428
		internal struct JobChunkWrapper<T> where T : struct
		{
			// Token: 0x040004C6 RID: 1222
			public T JobData;

			// Token: 0x040004C7 RID: 1223
			public UnsafeMatchingArchetypePtrList MatchingArchetypes;

			// Token: 0x040004C8 RID: 1224
			public UnsafeCachedChunkList CachedChunks;

			// Token: 0x040004C9 RID: 1225
			public EntityQueryFilter Filter;

			// Token: 0x040004CA RID: 1226
			public int IsParallel;

			// Token: 0x040004CB RID: 1227
			public int QueryHasEnableableComponents;
		}

		// Token: 0x020001AD RID: 429
		internal struct JobChunkProducer<T> where T : struct, IJobChunk
		{
			// Token: 0x06000A23 RID: 2595 RVA: 0x000324EC File Offset: 0x000306EC
			[BurstDiscard]
			internal unsafe static void Initialize()
			{
				if (*JobChunkExtensions.JobChunkProducer<T>.reflectionData.Data == IntPtr.Zero)
				{
					*JobChunkExtensions.JobChunkProducer<T>.reflectionData.Data = JobsUtility.CreateJobReflectionData(typeof(JobChunkExtensions.JobChunkWrapper<T>), typeof(T), new JobChunkExtensions.JobChunkProducer<T>.ExecuteJobFunction(JobChunkExtensions.JobChunkProducer<T>.Execute));
				}
			}

			// Token: 0x06000A24 RID: 2596 RVA: 0x00032540 File Offset: 0x00030740
			public static void Execute(ref JobChunkExtensions.JobChunkWrapper<T> jobWrapper, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex)
			{
				JobChunkExtensions.JobChunkProducer<T>.ExecuteInternal(ref jobWrapper, bufferRangePatchData, ref ranges, jobIndex);
			}

			// Token: 0x06000A25 RID: 2597 RVA: 0x0003254C File Offset: 0x0003074C
			internal unsafe static void ExecuteInternal(ref JobChunkExtensions.JobChunkWrapper<T> jobWrapper, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex)
			{
				UnsafeCachedChunkList cachedChunks = jobWrapper.CachedChunks;
				UnsafeChunkCacheIterator unsafeChunkCacheIterator = new UnsafeChunkCacheIterator(in jobWrapper.Filter, jobWrapper.QueryHasEnableableComponents != 0, jobWrapper.CachedChunks, jobWrapper.MatchingArchetypes.Ptr);
				bool flag = jobWrapper.IsParallel == 1;
				bool requiresMatchesFilter = jobWrapper.Filter.RequiresMatchesFilter;
				do
				{
					int num = 0;
					int length = cachedChunks.Length;
					if (flag && !JobsUtility.GetWorkStealingRange(ref ranges, jobIndex, out num, out length))
					{
						break;
					}
					if (jobWrapper.QueryHasEnableableComponents == 0 && !requiresMatchesFilter)
					{
						ChunkIndex* chunkIndices = cachedChunks.ChunkIndices;
						ArchetypeChunk archetypeChunk = new ArchetypeChunk(ChunkIndex.Null, cachedChunks.EntityComponentStore);
						v128 v = default(v128);
						for (int i = num; i < length; i++)
						{
							archetypeChunk.m_Chunk = chunkIndices[i];
							jobWrapper.JobData.Execute(in archetypeChunk, i, false, in v);
						}
					}
					else
					{
						unsafeChunkCacheIterator.Length = length;
						int num2 = num - 1;
						v128 v2 = default(v128);
						ArchetypeChunk archetypeChunk2;
						int num3;
						byte b;
						while (unsafeChunkCacheIterator.MoveNextChunk(ref num2, out archetypeChunk2, out num3, out b, ref v2))
						{
							jobWrapper.JobData.Execute(in archetypeChunk2, num2, b > 0, in v2);
						}
					}
				}
				while (flag);
			}

			// Token: 0x040004CC RID: 1228
			internal static readonly SharedStatic<IntPtr> reflectionData = SharedStatic<IntPtr>.GetOrCreate<JobChunkExtensions.JobChunkProducer<T>>(0U);

			// Token: 0x020001AE RID: 430
			// (Invoke) Token: 0x06000A28 RID: 2600
			public delegate void ExecuteJobFunction(ref JobChunkExtensions.JobChunkWrapper<T> jobWrapper, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);
		}
	}
}
