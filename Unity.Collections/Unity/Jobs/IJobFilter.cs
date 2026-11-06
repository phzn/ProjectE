using System;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Jobs
{
	// Token: 0x0200000A RID: 10
	[JobProducerType(typeof(IJobFilterExtensions.JobFilterProducer<>))]
	public interface IJobFilter
	{
		// Token: 0x06000011 RID: 17
		bool Execute(int index);
	}
}
