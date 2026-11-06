using System;

namespace Features.NPC.SceneSystem
{
	// Token: 0x02000F3A RID: 3898
	public enum NPCBehaviorPattern
	{
		// Token: 0x040077C9 RID: 30665
		Idle,
		// Token: 0x040077CA RID: 30666
		SitIdle,
		// Token: 0x040077CB RID: 30667
		SitAction,
		// Token: 0x040077CC RID: 30668
		StandAction,
		// Token: 0x040077CD RID: 30669
		Patrol,
		// Token: 0x040077CE RID: 30670
		LookAtPlayer,
		// Token: 0x040077CF RID: 30671
		LookAtTarget,
		// Token: 0x040077D0 RID: 30672
		WalkToAndSit,
		// Token: 0x040077D1 RID: 30673
		Custom
	}
}
