using System;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Jobs
{
	// Token: 0x0200000F RID: 15
	[JobProducerType(typeof(IJobParallelForBatchExtensions.JobParallelForBatchProducer<>))]
	public interface IJobParallelForBatch
	{
		// Token: 0x06000025 RID: 37
		void Execute(int startIndex, int count);
	}
}
