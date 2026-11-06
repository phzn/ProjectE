using System;
using System.Collections.Generic;
using Unity.Jobs;

namespace Pathfinding.Jobs
{
	// Token: 0x02000189 RID: 393
	public struct JobHandleWithMainThreadWork<T> where T : struct
	{
		// Token: 0x06000BDD RID: 3037 RVA: 0x00041A50 File Offset: 0x0003FC50
		public JobHandleWithMainThreadWork(IEnumerator<ValueTuple<JobHandle, T>> handles, JobDependencyTracker tracker)
		{
			this.coroutine = handles;
			this.tracker = tracker;
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x00041A60 File Offset: 0x0003FC60
		public void Complete()
		{
			this.tracker.timeSlice = TimeSlice.Infinite;
			while (this.coroutine.MoveNext())
			{
				ValueTuple<JobHandle, T> valueTuple = this.coroutine.Current;
				valueTuple.Item1.Complete();
			}
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00041AA4 File Offset: 0x0003FCA4
		public IEnumerable<T?> CompleteTimeSliced(float maxMillisPerStep)
		{
			this.tracker.timeSlice = TimeSlice.MillisFromNow(maxMillisPerStep);
			while (this.coroutine.MoveNext())
			{
				ValueTuple<JobHandle, T> valueTuple;
				if (maxMillisPerStep < float.PositiveInfinity)
				{
					for (;;)
					{
						valueTuple = this.coroutine.Current;
						if (valueTuple.Item1.IsCompleted)
						{
							break;
						}
						T? t = null;
						this.tracker.timeSlice = TimeSlice.MillisFromNow(maxMillisPerStep);
					}
				}
				valueTuple = this.coroutine.Current;
				valueTuple.Item1.Complete();
				yield return new T?(this.coroutine.Current.Item2);
				this.tracker.timeSlice = TimeSlice.MillisFromNow(maxMillisPerStep);
			}
			yield break;
			yield break;
		}

		// Token: 0x040007C5 RID: 1989
		private JobDependencyTracker tracker;

		// Token: 0x040007C6 RID: 1990
		private IEnumerator<ValueTuple<JobHandle, T>> coroutine;
	}
}
