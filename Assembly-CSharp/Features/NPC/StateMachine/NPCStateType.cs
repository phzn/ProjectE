using System;

namespace Features.NPC.StateMachine
{
	// Token: 0x02000F0E RID: 3854
	public enum NPCStateType
	{
		// Token: 0x040076E8 RID: 30440
		None,
		// Token: 0x040076E9 RID: 30441
		Idle = 10,
		// Token: 0x040076EA RID: 30442
		Wandering,
		// Token: 0x040076EB RID: 30443
		Performing,
		// Token: 0x040076EC RID: 30444
		LookingAt = 20,
		// Token: 0x040076ED RID: 30445
		StopAndStare,
		// Token: 0x040076EE RID: 30446
		Searching,
		// Token: 0x040076EF RID: 30447
		Confusion,
		// Token: 0x040076F0 RID: 30448
		Stumbling = 30,
		// Token: 0x040076F1 RID: 30449
		Tasered = 35,
		// Token: 0x040076F2 RID: 30450
		Detected = 40,
		// Token: 0x040076F3 RID: 30451
		Frozen = 50
	}
}
