using System;

namespace Features.NPC.StateMachine.States
{
	// Token: 0x02000F1A RID: 3866
	public class WanderingState : NPCStateBase
	{
		// Token: 0x1700130C RID: 4876
		// (get) Token: 0x0600745A RID: 29786 RVA: 0x002F42BD File Offset: 0x002F24BD
		public override NPCStateType StateType
		{
			get
			{
				return NPCStateType.Wandering;
			}
		}

		// Token: 0x1700130D RID: 4877
		// (get) Token: 0x0600745B RID: 29787 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public override bool LockProps
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700130E RID: 4878
		// (get) Token: 0x0600745C RID: 29788 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public override bool LockNavigation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600745D RID: 29789 RVA: 0x002F3436 File Offset: 0x002F1636
		public override void OnEnter(NPCFSMContext context, INPCState previousState)
		{
			base.OnEnter(context, previousState);
			if (previousState == null || previousState.StateType != NPCStateType.Detected)
			{
				context.ResetFacialExpression();
			}
		}

		// Token: 0x0600745E RID: 29790 RVA: 0x002F42C4 File Offset: 0x002F24C4
		public override NPCStateType? OnUpdate(NPCFSMContext context)
		{
			return null;
		}

		// Token: 0x0600745F RID: 29791 RVA: 0x0002AE8C File Offset: 0x0002908C
		public override bool CanTransitionTo(NPCStateType targetState)
		{
			return true;
		}
	}
}
