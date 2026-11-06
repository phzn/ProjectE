using System;

namespace Features.NPC.StateMachine
{
	// Token: 0x02000F09 RID: 3849
	public interface INPCState
	{
		// Token: 0x170012C9 RID: 4809
		// (get) Token: 0x06007383 RID: 29571
		NPCStateType StateType { get; }

		// Token: 0x170012CA RID: 4810
		// (get) Token: 0x06007384 RID: 29572
		int Priority { get; }

		// Token: 0x170012CB RID: 4811
		// (get) Token: 0x06007385 RID: 29573
		bool LockProps { get; }

		// Token: 0x170012CC RID: 4812
		// (get) Token: 0x06007386 RID: 29574
		bool LockNavigation { get; }

		// Token: 0x06007387 RID: 29575
		void OnEnter(NPCFSMContext context, INPCState previousState);

		// Token: 0x06007388 RID: 29576
		NPCStateType? OnUpdate(NPCFSMContext context);

		// Token: 0x06007389 RID: 29577
		void OnExit(NPCFSMContext context, INPCState nextState);

		// Token: 0x0600738A RID: 29578
		bool CanTransitionTo(NPCStateType targetState);
	}
}
