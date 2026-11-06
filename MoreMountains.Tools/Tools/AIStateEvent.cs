using System;

namespace MoreMountains.Tools
{
	// Token: 0x0200017B RID: 379
	public struct AIStateEvent
	{
		// Token: 0x060008E5 RID: 2277 RVA: 0x000296B6 File Offset: 0x000278B6
		public AIStateEvent(AIBrain brain, AIState exitState, AIState enterState)
		{
			this.Brain = brain;
			this.ExitState = exitState;
			this.EnterState = enterState;
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x000296CD File Offset: 0x000278CD
		public static void Trigger(AIBrain brain, AIState exitState, AIState enterState)
		{
			AIStateEvent.e.Brain = brain;
			AIStateEvent.e.ExitState = exitState;
			AIStateEvent.e.EnterState = enterState;
			MMEventManager.TriggerEvent<AIStateEvent>(AIStateEvent.e);
		}

		// Token: 0x04000A7F RID: 2687
		public AIBrain Brain;

		// Token: 0x04000A80 RID: 2688
		public AIState ExitState;

		// Token: 0x04000A81 RID: 2689
		public AIState EnterState;

		// Token: 0x04000A82 RID: 2690
		private static AIStateEvent e;
	}
}
