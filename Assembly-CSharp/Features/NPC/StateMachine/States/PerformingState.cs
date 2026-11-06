using System;

namespace Features.NPC.StateMachine.States
{
	// Token: 0x02000F15 RID: 3861
	public class PerformingState : NPCStateBase
	{
		// Token: 0x170012FD RID: 4861
		// (get) Token: 0x0600742E RID: 29742 RVA: 0x002F354F File Offset: 0x002F174F
		public override NPCStateType StateType
		{
			get
			{
				return NPCStateType.Performing;
			}
		}

		// Token: 0x170012FE RID: 4862
		// (get) Token: 0x0600742F RID: 29743 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public override bool LockProps
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170012FF RID: 4863
		// (get) Token: 0x06007430 RID: 29744 RVA: 0x0002AE8C File Offset: 0x0002908C
		public override bool LockNavigation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007431 RID: 29745 RVA: 0x002F3554 File Offset: 0x002F1754
		public override NPCStateType? OnUpdate(NPCFSMContext context)
		{
			return null;
		}

		// Token: 0x06007432 RID: 29746 RVA: 0x0002AE8C File Offset: 0x0002908C
		public override bool CanTransitionTo(NPCStateType targetState)
		{
			return true;
		}
	}
}
