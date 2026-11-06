using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Pathfinding.Pooling;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

namespace Pathfinding.Jobs
{
	// Token: 0x0200018C RID: 396
	public class JobDependencyTracker : IAstarPooledObject
	{
		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x00041C77 File Offset: 0x0003FE77
		public bool forceLinearDependencies
		{
			get
			{
				if (this.linearDependencies == LinearDependencies.Check)
				{
					this.SetLinearDependencies(false);
				}
				return this.linearDependencies == LinearDependencies.Enabled;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000BE9 RID: 3049 RVA: 0x00041C94 File Offset: 0x0003FE94
		public JobHandle AllWritesDependency
		{
			get
			{
				NativeArray<JobHandle> nativeArray = new NativeArray<JobHandle>(this.slots.Count, Allocator.Temp, NativeArrayOptions.ClearMemory);
				for (int i = 0; i < this.slots.Count; i++)
				{
					nativeArray[i] = this.slots[i].lastWrite.handle;
				}
				JobHandle jobHandle = JobHandle.CombineDependencies(nativeArray);
				nativeArray.Dispose();
				return jobHandle;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x00041CF6 File Offset: 0x0003FEF6
		private bool supportsMultithreading
		{
			get
			{
				return JobsUtility.JobWorkerCount > 0;
			}
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x00041D00 File Offset: 0x0003FF00
		public void SetLinearDependencies(bool linearDependencies)
		{
			if (!this.supportsMultithreading)
			{
				linearDependencies = true;
			}
			if (linearDependencies)
			{
				this.AllWritesDependency.Complete();
			}
			this.linearDependencies = (linearDependencies ? LinearDependencies.Enabled : LinearDependencies.Disabled);
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x00041D38 File Offset: 0x0003FF38
		public NativeArray<T> NewNativeArray<[IsUnmanaged] T>(int length, Allocator allocator, NativeArrayOptions options = NativeArrayOptions.ClearMemory) where T : struct, ValueType
		{
			NativeArray<T> nativeArray = new NativeArray<T>(length, allocator, options);
			this.Track<T>(nativeArray, options == NativeArrayOptions.ClearMemory);
			return nativeArray;
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x00041D5C File Offset: 0x0003FF5C
		public void Track<[IsUnmanaged] T>(NativeArray<T> array, bool initialized = true) where T : struct, ValueType
		{
			this.slots.Add(new JobDependencyTracker.NativeArraySlot
			{
				hash = NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<T>(array),
				lastWrite = default(JobDependencyTracker.JobInstance),
				lastReads = ListPool<JobDependencyTracker.JobInstance>.Claim(),
				initialized = initialized
			});
			if (this.arena == null)
			{
				this.arena = new DisposeArena();
			}
			this.arena.Add<T>(array);
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x00041DCB File Offset: 0x0003FFCB
		public void Persist<[IsUnmanaged] T>(NativeArray<T> array) where T : struct, ValueType
		{
			if (this.arena == null)
			{
				return;
			}
			this.arena.Remove<T>(array);
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x00041DE4 File Offset: 0x0003FFE4
		public JobHandle ScheduleBatch(NativeArray<RaycastCommand> commands, NativeArray<RaycastHit> results, int minCommandsPerJob)
		{
			if (this.forceLinearDependencies)
			{
				RaycastCommand.ScheduleBatch(commands, results, minCommandsPerJob, default(JobHandle)).Complete();
				return default(JobHandle);
			}
			JobDependencyTracker.JobRaycastCommandDummy jobRaycastCommandDummy = new JobDependencyTracker.JobRaycastCommandDummy
			{
				commands = commands,
				results = results
			};
			JobHandle dependencies = JobDependencyAnalyzer<JobDependencyTracker.JobRaycastCommandDummy>.GetDependencies(ref jobRaycastCommandDummy, this);
			JobHandle jobHandle = RaycastCommand.ScheduleBatch(commands, results, minCommandsPerJob, dependencies);
			JobDependencyAnalyzer<JobDependencyTracker.JobRaycastCommandDummy>.Scheduled(ref jobRaycastCommandDummy, this, jobHandle);
			return jobHandle;
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x00041E54 File Offset: 0x00040054
		public JobHandle ScheduleBatch(NativeArray<SpherecastCommand> commands, NativeArray<RaycastHit> results, int minCommandsPerJob)
		{
			if (this.forceLinearDependencies)
			{
				SpherecastCommand.ScheduleBatch(commands, results, minCommandsPerJob, default(JobHandle)).Complete();
				return default(JobHandle);
			}
			JobDependencyTracker.JobSpherecastCommandDummy jobSpherecastCommandDummy = new JobDependencyTracker.JobSpherecastCommandDummy
			{
				commands = commands,
				results = results
			};
			JobHandle dependencies = JobDependencyAnalyzer<JobDependencyTracker.JobSpherecastCommandDummy>.GetDependencies(ref jobSpherecastCommandDummy, this);
			JobHandle jobHandle = SpherecastCommand.ScheduleBatch(commands, results, minCommandsPerJob, dependencies);
			JobDependencyAnalyzer<JobDependencyTracker.JobSpherecastCommandDummy>.Scheduled(ref jobSpherecastCommandDummy, this, jobHandle);
			return jobHandle;
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00041EC4 File Offset: 0x000400C4
		public JobHandle ScheduleBatch(NativeArray<OverlapCapsuleCommand> commands, NativeArray<ColliderHit> results, int minCommandsPerJob)
		{
			if (this.forceLinearDependencies)
			{
				OverlapCapsuleCommand.ScheduleBatch(commands, results, minCommandsPerJob, 1, default(JobHandle)).Complete();
				return default(JobHandle);
			}
			JobDependencyTracker.JobOverlapCapsuleCommandDummy jobOverlapCapsuleCommandDummy = new JobDependencyTracker.JobOverlapCapsuleCommandDummy
			{
				commands = commands,
				results = results
			};
			JobHandle dependencies = JobDependencyAnalyzer<JobDependencyTracker.JobOverlapCapsuleCommandDummy>.GetDependencies(ref jobOverlapCapsuleCommandDummy, this);
			JobHandle jobHandle = OverlapCapsuleCommand.ScheduleBatch(commands, results, minCommandsPerJob, 1, dependencies);
			JobDependencyAnalyzer<JobDependencyTracker.JobOverlapCapsuleCommandDummy>.Scheduled(ref jobOverlapCapsuleCommandDummy, this, jobHandle);
			return jobHandle;
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00041F38 File Offset: 0x00040138
		public JobHandle ScheduleBatch(NativeArray<OverlapSphereCommand> commands, NativeArray<ColliderHit> results, int minCommandsPerJob)
		{
			if (this.forceLinearDependencies)
			{
				OverlapSphereCommand.ScheduleBatch(commands, results, minCommandsPerJob, 1, default(JobHandle)).Complete();
				return default(JobHandle);
			}
			JobDependencyTracker.JobOverlapSphereCommandDummy jobOverlapSphereCommandDummy = new JobDependencyTracker.JobOverlapSphereCommandDummy
			{
				commands = commands,
				results = results
			};
			JobHandle dependencies = JobDependencyAnalyzer<JobDependencyTracker.JobOverlapSphereCommandDummy>.GetDependencies(ref jobOverlapSphereCommandDummy, this);
			JobHandle jobHandle = OverlapSphereCommand.ScheduleBatch(commands, results, minCommandsPerJob, 1, dependencies);
			JobDependencyAnalyzer<JobDependencyTracker.JobOverlapSphereCommandDummy>.Scheduled(ref jobOverlapSphereCommandDummy, this, jobHandle);
			return jobHandle;
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x00041FAA File Offset: 0x000401AA
		public void DeferFree(GCHandle handle, JobHandle dependsOn)
		{
			if (this.arena == null)
			{
				this.arena = new DisposeArena();
			}
			this.arena.Add(handle);
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x00041FCC File Offset: 0x000401CC
		internal void JobReadsFrom(JobHandle job, long nativeArrayHash, int jobHash)
		{
			for (int i = 0; i < this.slots.Count; i++)
			{
				JobDependencyTracker.NativeArraySlot nativeArraySlot = this.slots[i];
				if (nativeArraySlot.hash == nativeArrayHash)
				{
					nativeArraySlot.lastReads.Add(new JobDependencyTracker.JobInstance
					{
						handle = job,
						hash = jobHash
					});
					return;
				}
			}
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x0004202C File Offset: 0x0004022C
		internal void JobWritesTo(JobHandle job, long nativeArrayHash, int jobHash)
		{
			for (int i = 0; i < this.slots.Count; i++)
			{
				JobDependencyTracker.NativeArraySlot nativeArraySlot = this.slots[i];
				if (nativeArraySlot.hash == nativeArrayHash)
				{
					nativeArraySlot.lastWrite = new JobDependencyTracker.JobInstance
					{
						handle = job,
						hash = jobHash
					};
					nativeArraySlot.lastReads.Clear();
					nativeArraySlot.initialized = true;
					nativeArraySlot.hasWrite = true;
					this.slots[i] = nativeArraySlot;
					return;
				}
			}
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x000420B0 File Offset: 0x000402B0
		private void Dispose()
		{
			for (int i = 0; i < this.slots.Count; i++)
			{
				ListPool<JobDependencyTracker.JobInstance>.Release(this.slots[i].lastReads);
			}
			this.slots.Clear();
			if (this.arena != null)
			{
				this.arena.DisposeAll();
			}
			this.linearDependencies = LinearDependencies.Check;
			if (this.dependenciesScratchBuffer.IsCreated)
			{
				this.dependenciesScratchBuffer.Dispose();
			}
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x00042128 File Offset: 0x00040328
		public void ClearMemory()
		{
			this.AllWritesDependency.Complete();
			this.Dispose();
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x00042149 File Offset: 0x00040349
		void IAstarPooledObject.OnEnterPool()
		{
			this.Dispose();
		}

		// Token: 0x040007D2 RID: 2002
		internal List<JobDependencyTracker.NativeArraySlot> slots = ListPool<JobDependencyTracker.NativeArraySlot>.Claim();

		// Token: 0x040007D3 RID: 2003
		private DisposeArena arena;

		// Token: 0x040007D4 RID: 2004
		internal NativeArray<JobHandle> dependenciesScratchBuffer;

		// Token: 0x040007D5 RID: 2005
		private LinearDependencies linearDependencies;

		// Token: 0x040007D6 RID: 2006
		internal TimeSlice timeSlice = TimeSlice.Infinite;

		// Token: 0x0200018D RID: 397
		internal struct JobInstance
		{
			// Token: 0x040007D7 RID: 2007
			public JobHandle handle;

			// Token: 0x040007D8 RID: 2008
			public int hash;
		}

		// Token: 0x0200018E RID: 398
		internal struct NativeArraySlot
		{
			// Token: 0x040007D9 RID: 2009
			public long hash;

			// Token: 0x040007DA RID: 2010
			public JobDependencyTracker.JobInstance lastWrite;

			// Token: 0x040007DB RID: 2011
			public List<JobDependencyTracker.JobInstance> lastReads;

			// Token: 0x040007DC RID: 2012
			public bool initialized;

			// Token: 0x040007DD RID: 2013
			public bool hasWrite;
		}

		// Token: 0x0200018F RID: 399
		private struct JobRaycastCommandDummy : IJob
		{
			// Token: 0x06000BFA RID: 3066 RVA: 0x00003788 File Offset: 0x00001988
			public void Execute()
			{
			}

			// Token: 0x040007DE RID: 2014
			[ReadOnly]
			public NativeArray<RaycastCommand> commands;

			// Token: 0x040007DF RID: 2015
			[WriteOnly]
			public NativeArray<RaycastHit> results;
		}

		// Token: 0x02000190 RID: 400
		private struct JobSpherecastCommandDummy : IJob
		{
			// Token: 0x06000BFB RID: 3067 RVA: 0x00003788 File Offset: 0x00001988
			public void Execute()
			{
			}

			// Token: 0x040007E0 RID: 2016
			[ReadOnly]
			public NativeArray<SpherecastCommand> commands;

			// Token: 0x040007E1 RID: 2017
			[WriteOnly]
			public NativeArray<RaycastHit> results;
		}

		// Token: 0x02000191 RID: 401
		private struct JobOverlapCapsuleCommandDummy : IJob
		{
			// Token: 0x06000BFC RID: 3068 RVA: 0x00003788 File Offset: 0x00001988
			public void Execute()
			{
			}

			// Token: 0x040007E2 RID: 2018
			[ReadOnly]
			public NativeArray<OverlapCapsuleCommand> commands;

			// Token: 0x040007E3 RID: 2019
			[WriteOnly]
			public NativeArray<ColliderHit> results;
		}

		// Token: 0x02000192 RID: 402
		private struct JobOverlapSphereCommandDummy : IJob
		{
			// Token: 0x06000BFD RID: 3069 RVA: 0x00003788 File Offset: 0x00001988
			public void Execute()
			{
			}

			// Token: 0x040007E4 RID: 2020
			[ReadOnly]
			public NativeArray<OverlapSphereCommand> commands;

			// Token: 0x040007E5 RID: 2021
			[WriteOnly]
			public NativeArray<ColliderHit> results;
		}
	}
}
