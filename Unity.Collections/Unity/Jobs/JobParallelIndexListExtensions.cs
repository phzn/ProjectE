using System;
using Unity.Collections;

namespace Unity.Jobs
{
	// Token: 0x02000008 RID: 8
	[Obsolete("'JobParallelIndexListExtensions' has been deprecated; Use 'IJobFilterExtensions' instead.", false)]
	public static class JobParallelIndexListExtensions
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002160 File Offset: 0x00000360
		[Obsolete("The signature for 'ScheduleAppend' has changed. 'innerloopBatchCount' is no longer part of this API.", false)]
		public static JobHandle ScheduleAppend<T>(this T jobData, NativeList<int> indices, int arrayLength, int innerloopBatchCount, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobFilter
		{
			return jobData.ScheduleAppend(indices, arrayLength, dependsOn);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000216C File Offset: 0x0000036C
		[Obsolete("The signature for 'ScheduleFilter' has changed. 'innerloopBatchCount' is no longer part of this API.")]
		public static JobHandle ScheduleFilter<T>(this T jobData, NativeList<int> indices, int innerloopBatchCount, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobFilter
		{
			return jobData.ScheduleFilter(indices, dependsOn);
		}
	}
}
