using System;
using Unity.Jobs;

namespace Pathfinding.Jobs
{
	// Token: 0x02000194 RID: 404
	public interface IJobTimeSliced : IJob
	{
		// Token: 0x06000C02 RID: 3074
		bool Execute(TimeSlice timeSlice);
	}
}
