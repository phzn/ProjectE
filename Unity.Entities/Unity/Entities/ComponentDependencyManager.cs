using System;
using System.Diagnostics;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Profiling;

namespace Unity.Entities
{
	// Token: 0x02000035 RID: 53
	internal struct ComponentDependencyManager
	{
		// Token: 0x0600013F RID: 319 RVA: 0x00006EEC File Offset: 0x000050EC
		private unsafe ushort GetTypeArrayIndex(TypeIndex typeIndex)
		{
			ushort num = this.m_TypeArrayIndices[typeIndex.Index];
			if (num != 65535)
			{
				return num;
			}
			ushort dependencyHandlesCount = this.m_DependencyHandlesCount;
			this.m_DependencyHandlesCount = dependencyHandlesCount + 1;
			num = dependencyHandlesCount;
			this.m_TypeArrayIndices[typeIndex.Index] = num;
			this.m_DependencyHandles[num].TypeIndex = typeIndex;
			this.m_DependencyHandles[num].NumReadFences = 0;
			this.m_DependencyHandles[num].WriteFence = default(JobHandle);
			return num;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00006F84 File Offset: 0x00005184
		private unsafe void ClearDependencies()
		{
			for (int i = 0; i < (int)this.m_DependencyHandlesCount; i++)
			{
				this.m_TypeArrayIndices[this.m_DependencyHandles[i].TypeIndex.Index] = ushort.MaxValue;
			}
			this.m_DependencyHandlesCount = 0;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00006FD4 File Offset: 0x000051D4
		public unsafe void OnCreate(WorldUnmanaged world)
		{
			this.m_World = world;
			this.m_TypeArrayIndices = (ushort*)Memory.Unmanaged.Allocate(16384L, 16, Allocator.Persistent);
			UnsafeUtility.MemSet((void*)this.m_TypeArrayIndices, byte.MaxValue, 16384L);
			this.m_ReadJobFences = (JobHandle*)Memory.Unmanaged.Allocate((long)(sizeof(JobHandle) * 17 * 8192), 16, Allocator.Persistent);
			UnsafeUtility.MemClear((void*)this.m_ReadJobFences, (long)(sizeof(JobHandle) * 17 * 8192));
			this.EntityTypeIndex = TypeManager.GetTypeIndex<Entity>();
			this.m_DependencyHandles = (ComponentDependencyManager.DependencyHandle*)Memory.Unmanaged.Allocate((long)(sizeof(ComponentDependencyManager.DependencyHandle) * 8192), 16, Allocator.Persistent);
			UnsafeUtility.MemClear((void*)this.m_DependencyHandles, (long)(sizeof(ComponentDependencyManager.DependencyHandle) * 8192));
			this.m_DependencyHandlesCount = 0;
			this._IsInTransaction = 0;
			this.m_ExclusiveTransactionDependency = default(JobHandle);
			this.m_Marker = new ProfilerMarker("CompleteAllJobs");
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000070C2 File Offset: 0x000052C2
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private static void AssertCompleteSyncPoint()
		{
			if (JobsUtility.IsExecutingJob)
			{
				throw new InvalidOperationException("Jobs accessing the entity manager must issue a complete sync point");
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x000070D8 File Offset: 0x000052D8
		public unsafe void CompleteAllJobs()
		{
			SystemHandle executingSystem = this.m_World.ExecutingSystem;
			if (executingSystem != default(SystemHandle))
			{
				SystemState* ptr = this.m_World.ResolveSystemState(executingSystem);
				if (ptr != null)
				{
					ptr->m_JobHandle.Complete();
				}
			}
			if (this.m_DependencyHandlesCount != 0)
			{
				this.GetCombinedDependencyForAllTypes().Complete();
				this.ClearDependencies();
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000713B File Offset: 0x0000533B
		public void CompleteAllJobsAndInvalidateArrays()
		{
			this.CompleteAllJobs();
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000713B File Offset: 0x0000533B
		public void CompleteAllJobsAndCheckDeallocateAndThrow()
		{
			this.CompleteAllJobs();
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00007144 File Offset: 0x00005344
		public void Dispose()
		{
			this.GetCombinedDependencyForAllTypes().Complete();
			Memory.Unmanaged.Free<ushort>(this.m_TypeArrayIndices, Allocator.Persistent);
			Memory.Unmanaged.Free<ComponentDependencyManager.DependencyHandle>(this.m_DependencyHandles, Allocator.Persistent);
			this.m_DependencyHandles = null;
			Memory.Unmanaged.Free<JobHandle>(this.m_ReadJobFences, Allocator.Persistent);
			this.m_ReadJobFences = null;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x000071A4 File Offset: 0x000053A4
		public void PreDisposeCheck()
		{
			this.GetCombinedDependencyForAllTypes().Complete();
		}

		// Token: 0x06000148 RID: 328 RVA: 0x000071C0 File Offset: 0x000053C0
		public unsafe void CompleteDependenciesNoChecks(TypeIndex* readerTypes, int readerTypesCount, TypeIndex* writerTypes, int writerTypesCount)
		{
			this.GetDependency(readerTypes, readerTypesCount, writerTypes, writerTypesCount, true).Complete();
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000071E4 File Offset: 0x000053E4
		public unsafe bool HasReaderOrWriterDependency(TypeIndex typeIndex, JobHandle dependency)
		{
			ushort num = this.m_TypeArrayIndices[typeIndex.Index];
			if (num == 65535)
			{
				return false;
			}
			JobHandle writeFence = this.m_DependencyHandles[num].WriteFence;
			if (JobHandle.CheckFenceIsDependencyOrDidSyncFence(dependency, writeFence))
			{
				return true;
			}
			int numReadFences = this.m_DependencyHandles[num].NumReadFences;
			for (int i = 0; i < numReadFences; i++)
			{
				JobHandle jobHandle = this.m_ReadJobFences[(int)(num * 17) + i];
				if (JobHandle.CheckFenceIsDependencyOrDidSyncFence(dependency, jobHandle))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000727C File Offset: 0x0000547C
		public unsafe JobHandle GetDependency(TypeIndex* readerTypes, int readerTypesCount, TypeIndex* writerTypes, int writerTypesCount, bool clearReadFencesAfterCombining)
		{
			JobHandle* ptr;
			int num;
			checked
			{
				ptr = stackalloc JobHandle[unchecked((UIntPtr)(readerTypesCount + writerTypesCount * 18)) * (UIntPtr)sizeof(JobHandle)];
				num = 0;
			}
			for (int num2 = 0; num2 != readerTypesCount; num2++)
			{
				ushort num3 = this.m_TypeArrayIndices[readerTypes[num2].Index];
				if (num3 != 65535)
				{
					ptr[(IntPtr)(num++) * (IntPtr)sizeof(JobHandle)] = this.m_DependencyHandles[num3].WriteFence;
				}
			}
			for (int num4 = 0; num4 != writerTypesCount; num4++)
			{
				ushort num5 = this.m_TypeArrayIndices[writerTypes[num4].Index];
				if (num5 != 65535)
				{
					ptr[(IntPtr)(num++) * (IntPtr)sizeof(JobHandle)] = this.m_DependencyHandles[num5].WriteFence;
					int numReadFences = this.m_DependencyHandles[num5].NumReadFences;
					JobHandle* ptr2 = this.m_ReadJobFences + num5 * 17;
					for (int num6 = 0; num6 != numReadFences; num6++)
					{
						ptr[(IntPtr)(num++) * (IntPtr)sizeof(JobHandle)] = ptr2[num6];
					}
					if (clearReadFencesAfterCombining)
					{
						this.m_DependencyHandles[num5].NumReadFences = 0;
					}
				}
			}
			if (Hint.Unlikely(num == 0))
			{
				return default(JobHandle);
			}
			if (num == 1)
			{
				return *ptr;
			}
			return JobHandleUnsafeUtility.CombineDependencies(ptr, num);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000740C File Offset: 0x0000560C
		public unsafe JobHandle AddDependency(TypeIndex* readerTypes, int readerTypesCount, TypeIndex* writerTypes, int writerTypesCount, JobHandle dependency)
		{
			if (readerTypesCount == 0 && writerTypesCount == 0)
			{
				ushort typeArrayIndex = this.GetTypeArrayIndex(this.EntityTypeIndex);
				this.m_ReadJobFences[(int)(typeArrayIndex * 17) + this.m_DependencyHandles[typeArrayIndex].NumReadFences] = dependency;
				this.m_DependencyHandles[typeArrayIndex].NumReadFences++;
				if (this.m_DependencyHandles[typeArrayIndex].NumReadFences == 17)
				{
					this.CombineReadDependencies(typeArrayIndex);
				}
				return dependency;
			}
			for (int num = 0; num != writerTypesCount; num++)
			{
				this.m_DependencyHandles[this.GetTypeArrayIndex(writerTypes[num])].WriteFence = dependency;
			}
			for (int num2 = 0; num2 != readerTypesCount; num2++)
			{
				ushort typeArrayIndex2 = this.GetTypeArrayIndex(readerTypes[num2]);
				this.m_ReadJobFences[(int)(typeArrayIndex2 * 17) + this.m_DependencyHandles[typeArrayIndex2].NumReadFences] = dependency;
				this.m_DependencyHandles[typeArrayIndex2].NumReadFences++;
				if (this.m_DependencyHandles[typeArrayIndex2].NumReadFences == 17)
				{
					this.CombineReadDependencies(typeArrayIndex2);
				}
			}
			return dependency;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000756C File Offset: 0x0000576C
		internal unsafe void CompleteWriteDependencyNoChecks(TypeIndex typeIndex)
		{
			ushort num = this.m_TypeArrayIndices[typeIndex.Index];
			if (num != 65535)
			{
				this.m_DependencyHandles[num].WriteFence.Complete();
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x000075B0 File Offset: 0x000057B0
		internal unsafe void CompleteReadAndWriteDependencyNoChecks(TypeIndex typeIndex)
		{
			ushort num = this.m_TypeArrayIndices[typeIndex.Index];
			if (num != 65535)
			{
				int numReadFences = this.m_DependencyHandles[num].NumReadFences;
				if (numReadFences == 0)
				{
					this.m_DependencyHandles[num].WriteFence.Complete();
					return;
				}
				int num2 = numReadFences + 1;
				JobHandle* ptr;
				checked
				{
					ptr = stackalloc JobHandle[unchecked((UIntPtr)num2) * (UIntPtr)sizeof(JobHandle)];
				}
				JobHandle* ptr2 = this.m_ReadJobFences + num * 17;
				for (int i = 0; i < numReadFences; i++)
				{
					ptr[i] = ptr2[i];
				}
				ptr[numReadFences] = this.m_DependencyHandles[num].WriteFence;
				JobHandleUnsafeUtility.CombineDependencies(ptr, num2).Complete();
				this.m_DependencyHandles[num].NumReadFences = 0;
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x000076B2 File Offset: 0x000058B2
		public void CompleteWriteDependency(TypeIndex type)
		{
			this.CompleteWriteDependencyNoChecks(type);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x000076BB File Offset: 0x000058BB
		public void CompleteReadAndWriteDependency(TypeIndex type)
		{
			this.CompleteReadAndWriteDependencyNoChecks(type);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000076C4 File Offset: 0x000058C4
		private unsafe JobHandle CombineReadDependencies(ushort typeArrayIndex)
		{
			JobHandle jobHandle = JobHandleUnsafeUtility.CombineDependencies(this.m_ReadJobFences + typeArrayIndex * 17, this.m_DependencyHandles[typeArrayIndex].NumReadFences);
			this.m_ReadJobFences[typeArrayIndex * 17] = jobHandle;
			this.m_DependencyHandles[typeArrayIndex].NumReadFences = 1;
			return jobHandle;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00007730 File Offset: 0x00005930
		private unsafe JobHandle GetCombinedDependencyForAllTypes()
		{
			JobHandle* ptr;
			int num;
			checked
			{
				ptr = stackalloc JobHandle[unchecked((UIntPtr)(this.m_DependencyHandlesCount * 18)) * (UIntPtr)sizeof(JobHandle)];
				num = 0;
			}
			for (int num2 = 0; num2 != (int)this.m_DependencyHandlesCount; num2++)
			{
				ptr[(IntPtr)(num++) * (IntPtr)sizeof(JobHandle)] = this.m_DependencyHandles[num2].WriteFence;
				int numReadFences = this.m_DependencyHandles[num2].NumReadFences;
				JobHandle* ptr2 = this.m_ReadJobFences + num2 * 17;
				for (int i = 0; i < numReadFences; i++)
				{
					ptr[(IntPtr)(num++) * (IntPtr)sizeof(JobHandle)] = ptr2[i];
				}
			}
			if (Hint.Unlikely(num == 0))
			{
				return default(JobHandle);
			}
			if (num == 1)
			{
				return *ptr;
			}
			return JobHandleUnsafeUtility.CombineDependencies(ptr, num);
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000152 RID: 338 RVA: 0x0000781C File Offset: 0x00005A1C
		// (set) Token: 0x06000153 RID: 339 RVA: 0x00007827 File Offset: 0x00005A27
		internal bool IsInTransaction
		{
			get
			{
				return this._IsInTransaction > 0;
			}
			set
			{
				this._IsInTransaction = (value ? 1 : 0);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00007837 File Offset: 0x00005A37
		// (set) Token: 0x06000155 RID: 341 RVA: 0x0000783F File Offset: 0x00005A3F
		public JobHandle ExclusiveTransactionDependency
		{
			get
			{
				return this.m_ExclusiveTransactionDependency;
			}
			set
			{
				this.m_ExclusiveTransactionDependency = value;
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00007848 File Offset: 0x00005A48
		public void PreEndExclusiveTransaction()
		{
			if (this._IsInTransaction == 1)
			{
				this.m_ExclusiveTransactionDependency.Complete();
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000785E File Offset: 0x00005A5E
		public void EndExclusiveTransaction()
		{
			if (this._IsInTransaction == 0)
			{
				return;
			}
			this._IsInTransaction = 0;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00007870 File Offset: 0x00005A70
		public void BeginExclusiveTransaction()
		{
			if (this._IsInTransaction == 1)
			{
				return;
			}
			this._IsInTransaction = 1;
			this.m_ExclusiveTransactionDependency = this.GetCombinedDependencyForAllTypes();
			this.ClearDependencies();
		}

		// Token: 0x04000054 RID: 84
		private const int kMaxWriteJobHandles = 1;

		// Token: 0x04000055 RID: 85
		private const int kMaxReadJobHandles = 17;

		// Token: 0x04000056 RID: 86
		private const int kMaxTypes = 8192;

		// Token: 0x04000057 RID: 87
		private unsafe ushort* m_TypeArrayIndices;

		// Token: 0x04000058 RID: 88
		private unsafe ComponentDependencyManager.DependencyHandle* m_DependencyHandles;

		// Token: 0x04000059 RID: 89
		private ushort m_DependencyHandlesCount;

		// Token: 0x0400005A RID: 90
		private unsafe JobHandle* m_ReadJobFences;

		// Token: 0x0400005B RID: 91
		private TypeIndex EntityTypeIndex;

		// Token: 0x0400005C RID: 92
		private const ushort NullTypeIndex = 65535;

		// Token: 0x0400005D RID: 93
		private JobHandle m_ExclusiveTransactionDependency;

		// Token: 0x0400005E RID: 94
		private byte _IsInTransaction;

		// Token: 0x0400005F RID: 95
		private ProfilerMarker m_Marker;

		// Token: 0x04000060 RID: 96
		private WorldUnmanaged m_World;

		// Token: 0x02000036 RID: 54
		private struct DependencyHandle
		{
			// Token: 0x04000061 RID: 97
			public JobHandle WriteFence;

			// Token: 0x04000062 RID: 98
			public int NumReadFences;

			// Token: 0x04000063 RID: 99
			public TypeIndex TypeIndex;
		}
	}
}
