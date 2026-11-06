using System;
using Pathfinding.Jobs;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Pathfinding.Graphs.Grid.Jobs
{
	// Token: 0x02000227 RID: 551
	internal struct JobCheckCollisions : IJobTimeSliced, IJob
	{
		// Token: 0x06000E32 RID: 3634 RVA: 0x000573C9 File Offset: 0x000555C9
		public void Execute()
		{
			this.Execute(TimeSlice.Infinite);
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x000573D8 File Offset: 0x000555D8
		public bool Execute(TimeSlice timeSlice)
		{
			for (int i = this.startIndex; i < this.nodePositions.Length; i++)
			{
				this.collisionResult[i] = this.collisionResult[i] && this.collision.Check(this.nodePositions[i]);
				if ((i & 127) == 0 && timeSlice.expired)
				{
					this.startIndex = i + 1;
					return false;
				}
			}
			return true;
		}

		// Token: 0x04000A5B RID: 2651
		[ReadOnly]
		public NativeArray<Vector3> nodePositions;

		// Token: 0x04000A5C RID: 2652
		public NativeArray<bool> collisionResult;

		// Token: 0x04000A5D RID: 2653
		public GraphCollision collision;

		// Token: 0x04000A5E RID: 2654
		private int startIndex;
	}
}
