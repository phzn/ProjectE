using System;

namespace Features.NPC.StateMachine.States
{
	// Token: 0x02000F13 RID: 3859
	public class IdleState : NPCStateBase
	{
		// Token: 0x170012F7 RID: 4855
		// (get) Token: 0x0600741F RID: 29727 RVA: 0x002F3432 File Offset: 0x002F1632
		public override NPCStateType StateType
		{
			get
			{
				return NPCStateType.Idle;
			}
		}

		// Token: 0x170012F8 RID: 4856
		// (get) Token: 0x06007420 RID: 29728 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public override bool LockProps
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170012F9 RID: 4857
		// (get) Token: 0x06007421 RID: 29729 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public override bool LockNavigation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007422 RID: 29730 RVA: 0x002F3436 File Offset: 0x002F1636
		public override void OnEnter(NPCFSMContext context, INPCState previousState)
		{
			base.OnEnter(context, previousState);
			if (previousState == null || previousState.StateType != NPCStateType.Detected)
			{
				context.ResetFacialExpression();
			}
		}

		// Token: 0x06007423 RID: 29731 RVA: 0x002F3458 File Offset: 0x002F1658
		public override NPCStateType? OnUpdate(NPCFSMContext context)
		{
			return null;
		}

		// Token: 0x06007424 RID: 29732 RVA: 0x0002AE8C File Offset: 0x0002908C
		public override bool CanTransitionTo(NPCStateType targetState)
		{
			return true;
		}
	}
}
