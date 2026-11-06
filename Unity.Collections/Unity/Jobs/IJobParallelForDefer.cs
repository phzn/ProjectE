using System;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Jobs
{
	// Token: 0x02000013 RID: 19
	[JobProducerType(typeof(IJobParallelForDeferExtensions.JobParallelForDeferProducer<>))]
	public interface IJobParallelForDefer
	{
		// Token: 0x06000039 RID: 57
		void Execute(int index);
	}
}
