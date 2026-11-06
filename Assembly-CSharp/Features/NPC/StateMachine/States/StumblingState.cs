using System;
using UnityEngine;

namespace Features.NPC.StateMachine.States
{
	// Token: 0x02000F18 RID: 3864
	public class StumblingState : NPCStateBase
	{
		// Token: 0x17001306 RID: 4870
		// (get) Token: 0x06007449 RID: 29769 RVA: 0x002F3EB2 File Offset: 0x002F20B2
		public override NPCStateType StateType
		{
			get
			{
				return NPCStateType.Stumbling;
			}
		}

		// Token: 0x17001307 RID: 4871
		// (get) Token: 0x0600744A RID: 29770 RVA: 0x0002AE8C File Offset: 0x0002908C
		public override bool LockProps
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001308 RID: 4872
		// (get) Token: 0x0600744B RID: 29771 RVA: 0x0002AE8C File Offset: 0x0002908C
		public override bool LockNavigation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600744C RID: 29772 RVA: 0x002F3EB8 File Offset: 0x002F20B8
		public override void OnEnter(NPCFSMContext context, INPCState previousState)
		{
			context.PrepareToStop(0.2f);
			context.ZeroLocomotion();
			context.ResetToIdleAnimation();
			base.OnEnter(context, previousState);
			context.EnableReactionsLayer();
			context.SetAlertExpression();
			context.StopLooking();
			if (context.PlayerPosition != null)
			{
				Vector3 vector = context.PlayerPosition.Value - context.Transform.position;
				float num = Vector3.SignedAngle(context.Transform.forward, vector, Vector3.up);
				if (Mathf.Abs(num) < 45f)
				{
					this._stumbleDirection = 0;
				}
				else if (Mathf.Abs(num) > 135f)
				{
					this._stumbleDirection = 1;
				}
				else if (num < 0f)
				{
					this._stumbleDirection = 2;
				}
				else
				{
					this._stumbleDirection = 3;
				}
			}
			else
			{
				this._stumbleDirection = 0;
			}
			context.SetTrigger("StumbleTrigger");
			context.SetFloat("StumbleDirection", (float)this._stumbleDirection);
			base.Log(context, string.Format("Stumbling, direction: {0}", this._stumbleDirection));
		}

		// Token: 0x0600744D RID: 29773 RVA: 0x002F3FC4 File Offset: 0x002F21C4
		public override NPCStateType? OnUpdate(NPCFSMContext context)
		{
			if (context.TimeInState >= 1.5f)
			{
				if (context.Senses != null && context.Senses.CurrentAttention < 0.5f)
				{
					context.Senses.SetAttentionState(0.5f);
				}
				if (context.PlayerPosition != null)
				{
					context.LastSeenPlayerPosition = context.PlayerPosition.Value;
				}
				base.Log(context, "Stumble complete, transitioning to Searching");
				return new NPCStateType?(NPCStateType.Searching);
			}
			return null;
		}

		// Token: 0x0600744E RID: 29774 RVA: 0x002F404F File Offset: 0x002F224F
		public override void OnExit(NPCFSMContext context, INPCState nextState)
		{
			base.OnExit(context, nextState);
			context.DisableReactionsLayer();
		}

		// Token: 0x0600744F RID: 29775 RVA: 0x002F405F File Offset: 0x002F225F
		public override bool CanTransitionTo(NPCStateType targetState)
		{
			return targetState == NPCStateType.Searching || targetState.GetPriority() > this.StateType.GetPriority();
		}

		// Token: 0x0400770F RID: 30479
		private const float StumbleDuration = 1.5f;

		// Token: 0x04007710 RID: 30480
		private int _stumbleDirection;
	}
}
