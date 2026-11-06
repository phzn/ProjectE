using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Jobs;

namespace Pathfinding.Jobs
{
	// Token: 0x02000195 RID: 405
	public static class IJobExtensions
	{
		// Token: 0x06000C03 RID: 3075 RVA: 0x000421EC File Offset: 0x000403EC
		public static JobHandle Schedule<T>(this T data, JobDependencyTracker tracker) where T : struct, IJob
		{
			if (tracker.forceLinearDependencies)
			{
				data.Run<T>();
				return default(JobHandle);
			}
			JobHandle jobHandle = data.Schedule(JobDependencyAnalyzer<T>.GetDependencies(ref data, tracker));
			JobDependencyAnalyzer<T>.Scheduled(ref data, tracker, jobHandle);
			return jobHandle;
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x0004222C File Offset: 0x0004042C
		public static JobHandle ScheduleBatch<T>(this T data, int arrayLength, int minIndicesPerJobCount, JobDependencyTracker tracker, JobHandle additionalDependency = default(JobHandle)) where T : struct, IJobParallelForBatched
		{
			if (tracker.forceLinearDependencies)
			{
				additionalDependency.Complete();
				data.RunBatch(arrayLength);
				return default(JobHandle);
			}
			JobHandle jobHandle = data.ScheduleBatch(arrayLength, minIndicesPerJobCount, JobDependencyAnalyzer<T>.GetDependencies(ref data, tracker, additionalDependency));
			JobDependencyAnalyzer<T>.Scheduled(ref data, tracker, jobHandle);
			return jobHandle;
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x00042278 File Offset: 0x00040478
		public static JobHandle ScheduleManaged<T>(this T data, JobHandle dependsOn) where T : struct, IJob
		{
			return new IJobExtensions.ManagedJob
			{
				handle = GCHandle.Alloc(data)
			}.Schedule(dependsOn);
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x000422A8 File Offset: 0x000404A8
		public static JobHandle ScheduleManaged(this Action data, JobHandle dependsOn)
		{
			return new IJobExtensions.ManagedActionJob
			{
				handle = GCHandle.Alloc(data)
			}.Schedule(dependsOn);
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x000422D4 File Offset: 0x000404D4
		public static JobHandle GetDependencies<T>(this T data, JobDependencyTracker tracker) where T : struct, IJob
		{
			if (tracker.forceLinearDependencies)
			{
				return default(JobHandle);
			}
			return JobDependencyAnalyzer<T>.GetDependencies(ref data, tracker);
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x000422FB File Offset: 0x000404FB
		public static IEnumerator<JobHandle> ExecuteMainThreadJob<T>(this T data, JobDependencyTracker tracker) where T : struct, IJobTimeSliced
		{
			if (tracker.forceLinearDependencies)
			{
				data.Execute();
				yield break;
			}
			JobHandle dependencies = JobDependencyAnalyzer<T>.GetDependencies(ref data, tracker);
			yield return dependencies;
			while (!data.Execute(tracker.timeSlice))
			{
				JobHandle jobHandle = default(JobHandle);
			}
			yield break;
			yield break;
		}

		// Token: 0x02000196 RID: 406
		private struct ManagedJob : IJob
		{
			// Token: 0x06000C09 RID: 3081 RVA: 0x00042311 File Offset: 0x00040511
			public void Execute()
			{
				((IJob)this.handle.Target).Execute();
				this.handle.Free();
			}

			// Token: 0x040007E8 RID: 2024
			public GCHandle handle;
		}

		// Token: 0x02000197 RID: 407
		private struct ManagedActionJob : IJob
		{
			// Token: 0x06000C0A RID: 3082 RVA: 0x00042333 File Offset: 0x00040533
			public void Execute()
			{
				((Action)this.handle.Target)();
				this.handle.Free();
			}

			// Token: 0x040007E9 RID: 2025
			public GCHandle handle;
		}
	}
}
