using System;

namespace Features.NPC.StateMachine.States
{
	// Token: 0x02000F12 RID: 3858
	public class FrozenState : NPCStateBase
	{
		// Token: 0x170012F4 RID: 4852
		// (get) Token: 0x06007418 RID: 29720 RVA: 0x002F33E9 File Offset: 0x002F15E9
		public override NPCStateType StateType
		{
			get
			{
				return NPCStateType.Frozen;
			}
		}

		// Token: 0x170012F5 RID: 4853
		// (get) Token: 0x06007419 RID: 29721 RVA: 0x0002AE8C File Offset: 0x0002908C
		public override bool LockProps
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170012F6 RID: 4854
		// (get) Token: 0x0600741A RID: 29722 RVA: 0x0002AE8C File Offset: 0x0002908C
		public override bool LockNavigation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600741B RID: 29723 RVA: 0x002F33ED File Offset: 0x002F15ED
		public override void OnEnter(NPCFSMContext context, INPCState previousState)
		{
			base.OnEnter(context, previousState);
			context.StopLooking();
			context.SetFloat("VerticalSpeed", 0f);
			base.Log(context, "FROZEN by event");
		}

		// Token: 0x0600741C RID: 29724 RVA: 0x002F341C File Offset: 0x002F161C
		public override NPCStateType? OnUpdate(NPCFSMContext context)
		{
			return null;
		}

		// Token: 0x0600741D RID: 29725 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public override bool CanTransitionTo(NPCStateType targetState)
		{
			return false;
		}
	}
}
