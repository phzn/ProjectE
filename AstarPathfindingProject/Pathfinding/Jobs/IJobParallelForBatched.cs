using System;
using Unity.Jobs.LowLevel.Unsafe;

namespace Pathfinding.Jobs
{
	// Token: 0x02000182 RID: 386
	[JobProducerType(typeof(JobParallelForBatchedExtensions.ParallelForBatchJobStruct<>))]
	public interface IJobParallelForBatched
	{
		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000BC7 RID: 3015
		bool allowBoundsChecks { get; }

		// Token: 0x06000BC8 RID: 3016
		void Execute(int startIndex, int count);
	}
}
