using System;

namespace Pathfinding.RVO
{
	// Token: 0x020002BF RID: 703
	[Flags]
	public enum AgentDebugFlags : byte
	{
		// Token: 0x04000C98 RID: 3224
		Nothing = 0,
		// Token: 0x04000C99 RID: 3225
		ObstacleVelocityObstacles = 1,
		// Token: 0x04000C9A RID: 3226
		AgentVelocityObstacles = 2,
		// Token: 0x04000C9B RID: 3227
		ReachedState = 4,
		// Token: 0x04000C9C RID: 3228
		DesiredVelocity = 8,
		// Token: 0x04000C9D RID: 3229
		ChosenVelocity = 16,
		// Token: 0x04000C9E RID: 3230
		Obstacles = 32,
		// Token: 0x04000C9F RID: 3231
		ForwardClearance = 64,
		// Token: 0x04000CA0 RID: 3232
		[Obsolete("Renamed to ObstacleVelocityObstacles")]
		ObstacleVOs = 1,
		// Token: 0x04000CA1 RID: 3233
		[Obsolete("Renamed to AgentVelocityObstacles")]
		AgentVOs = 2
	}
}
