using System;

namespace Features.NPC.StateMachine.States
{
	// Token: 0x02000F11 RID: 3857
	public class DetectedState : NPCStateBase
	{
		// Token: 0x170012F1 RID: 4849
		// (get) Token: 0x06007411 RID: 29713 RVA: 0x002F3379 File Offset: 0x002F1579
		public override NPCStateType StateType
		{
			get
			{
				return NPCStateType.Detected;
			}
		}

		// Token: 0x170012F2 RID: 4850
		// (get) Token: 0x06007412 RID: 29714 RVA: 0x0002AE8C File Offset: 0x0002908C
		public override bool LockProps
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170012F3 RID: 4851
		// (get) Token: 0x06007413 RID: 29715 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public override bool LockNavigation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007414 RID: 29716 RVA: 0x002F3380 File Offset: 0x002F1580
		public override void OnEnter(NPCFSMContext context, INPCState previousState)
		{
			base.OnEnter(context, previousState);
			context.StopLooking();
			if (context.PlayerPosition != null)
			{
				context.LastSeenPlayerPosition = context.PlayerPosition.Value;
			}
			base.Log(context, "DETECTED! Starting chase/flee behavior");
		}

		// Token: 0x06007415 RID: 29717 RVA: 0x002F33CC File Offset: 0x002F15CC
		public override NPCStateType? OnUpdate(NPCFSMContext context)
		{
			return null;
		}

		// Token: 0x06007416 RID: 29718 RVA: 0x002F33E2 File Offset: 0x002F15E2
		public override bool CanTransitionTo(NPCStateType targetState)
		{
			return targetState == NPCStateType.Frozen;
		}
	}
}
