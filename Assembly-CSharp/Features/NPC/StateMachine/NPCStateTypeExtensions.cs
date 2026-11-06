using System;

namespace Features.NPC.StateMachine
{
	// Token: 0x02000F0F RID: 3855
	public static class NPCStateTypeExtensions
	{
		// Token: 0x06007404 RID: 29700 RVA: 0x002F3147 File Offset: 0x002F1347
		public static bool IsNormalState(this NPCStateType state)
		{
			return state == NPCStateType.Idle || state == NPCStateType.Wandering || state == NPCStateType.Performing;
		}

		// Token: 0x06007405 RID: 29701 RVA: 0x002F315A File Offset: 0x002F135A
		public static bool IsAlertState(this NPCStateType state)
		{
			return state == NPCStateType.LookingAt || state == NPCStateType.StopAndStare || state == NPCStateType.Searching || state == NPCStateType.Confusion;
		}

		// Token: 0x06007406 RID: 29702 RVA: 0x002F3172 File Offset: 0x002F1372
		public static bool IsCriticalState(this NPCStateType state)
		{
			return state == NPCStateType.Stumbling || state == NPCStateType.Detected || state == NPCStateType.Frozen;
		}

		// Token: 0x06007407 RID: 29703 RVA: 0x000F5D4D File Offset: 0x000F3F4D
		public static int GetPriority(this NPCStateType state)
		{
			return (int)state;
		}
	}
}
