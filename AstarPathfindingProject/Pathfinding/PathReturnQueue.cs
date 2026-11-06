using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x020000C4 RID: 196
	internal class PathReturnQueue
	{
		// Token: 0x06000688 RID: 1672 RVA: 0x0001EC5E File Offset: 0x0001CE5E
		public PathReturnQueue(object pathsClaimedSilentlyBy, Action OnReturnedPaths)
		{
			this.pathsClaimedSilentlyBy = pathsClaimedSilentlyBy;
			this.OnReturnedPaths = OnReturnedPaths;
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0001EC8C File Offset: 0x0001CE8C
		public void Enqueue(Path path)
		{
			lock (this)
			{
				this.pathReturnQueueWriting.Enqueue(path);
			}
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0001ECD0 File Offset: 0x0001CED0
		public void ReturnPaths(bool timeSlice)
		{
			long num = (timeSlice ? (DateTime.UtcNow.Ticks + 10000L) : 0L);
			int num2 = 0;
			int num3 = 0;
			for (;;)
			{
				if (this.pathReturnQueueReading.Count == 0)
				{
					lock (this)
					{
						Memory.Swap<Queue<Path>>(ref this.pathReturnQueueReading, ref this.pathReturnQueueWriting);
					}
					if (this.pathReturnQueueReading.Count == 0)
					{
						break;
					}
				}
				Path path = this.pathReturnQueueReading.Dequeue();
				((IPathInternals)path).AdvanceState(PathState.Returning);
				try
				{
					((IPathInternals)path).ReturnPath();
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
				}
				((IPathInternals)path).AdvanceState(PathState.Returned);
				path.Release(this.pathsClaimedSilentlyBy, true);
				num2++;
				num3++;
				if (num2 > 5 && timeSlice)
				{
					num2 = 0;
					if (DateTime.UtcNow.Ticks >= num)
					{
						break;
					}
				}
			}
			if (num3 > 0)
			{
				this.OnReturnedPaths();
			}
		}

		// Token: 0x04000435 RID: 1077
		private Queue<Path> pathReturnQueueWriting = new Queue<Path>();

		// Token: 0x04000436 RID: 1078
		private Queue<Path> pathReturnQueueReading = new Queue<Path>();

		// Token: 0x04000437 RID: 1079
		private readonly object pathsClaimedSilentlyBy;

		// Token: 0x04000438 RID: 1080
		private readonly Action OnReturnedPaths;
	}
}
