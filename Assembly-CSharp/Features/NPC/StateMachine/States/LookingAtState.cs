using System;
using Features.Detection;

namespace Features.NPC.StateMachine.States
{
	// Token: 0x02000F14 RID: 3860
	public class LookingAtState : NPCStateBase
	{
		// Token: 0x170012FA RID: 4858
		// (get) Token: 0x06007426 RID: 29734 RVA: 0x002F346E File Offset: 0x002F166E
		public override NPCStateType StateType
		{
			get
			{
				return NPCStateType.LookingAt;
			}
		}

		// Token: 0x170012FB RID: 4859
		// (get) Token: 0x06007427 RID: 29735 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public override bool LockProps
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170012FC RID: 4860
		// (get) Token: 0x06007428 RID: 29736 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public override bool LockNavigation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007429 RID: 29737 RVA: 0x002F3474 File Offset: 0x002F1674
		public override void OnEnter(NPCFSMContext context, INPCState previousState)
		{
			base.OnEnter(context, previousState);
			context.StartLookingAtPlayer();
			context.SetAlertExpression();
			if (context.PlayerPosition != null)
			{
				context.LastSeenPlayerPosition = context.PlayerPosition.Value;
			}
		}

		// Token: 0x0600742A RID: 29738 RVA: 0x002F34BC File Offset: 0x002F16BC
		public override NPCStateType? OnUpdate(NPCFSMContext context)
		{
			float attention = context.Attention;
			DetectionSource detectionSource = context.DetectionSource;
			if (detectionSource == DetectionSource.Visual && context.PlayerPosition != null)
			{
				context.LastSeenPlayerPosition = context.PlayerPosition.Value;
			}
			if (attention >= 0.4f)
			{
				context.StopLooking();
				return new NPCStateType?((detectionSource == DetectionSource.Visual) ? NPCStateType.StopAndStare : NPCStateType.Searching);
			}
			float num = 0.10000001f;
			if (attention < num)
			{
				context.StopLooking();
				return new NPCStateType?(NPCStateType.Wandering);
			}
			return null;
		}

		// Token: 0x0600742B RID: 29739 RVA: 0x002F353F File Offset: 0x002F173F
		public override void OnExit(NPCFSMContext context, INPCState nextState)
		{
			base.OnExit(context, nextState);
			context.StopLooking();
		}

		// Token: 0x0600742C RID: 29740 RVA: 0x0002AE8C File Offset: 0x0002908C
		public override bool CanTransitionTo(NPCStateType targetState)
		{
			return true;
		}

		// Token: 0x040076FA RID: 30458
		private const float LookAtThreshold = 0.15f;

		// Token: 0x040076FB RID: 30459
		private const float AlertThreshold = 0.4f;

		// Token: 0x040076FC RID: 30460
		private const float Hysteresis = 0.05f;
	}
}
