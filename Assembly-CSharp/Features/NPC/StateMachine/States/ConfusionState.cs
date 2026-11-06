using System;
using Features.Detection;
using UnityEngine;

namespace Features.NPC.StateMachine.States
{
	// Token: 0x02000F10 RID: 3856
	public class ConfusionState : NPCStateBase
	{
		// Token: 0x170012EE RID: 4846
		// (get) Token: 0x06007408 RID: 29704 RVA: 0x002F3185 File Offset: 0x002F1385
		public override NPCStateType StateType
		{
			get
			{
				return NPCStateType.Confusion;
			}
		}

		// Token: 0x170012EF RID: 4847
		// (get) Token: 0x06007409 RID: 29705 RVA: 0x0002AE8C File Offset: 0x0002908C
		public override bool LockProps
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170012F0 RID: 4848
		// (get) Token: 0x0600740A RID: 29706 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public override bool LockNavigation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600740B RID: 29707 RVA: 0x002F318C File Offset: 0x002F138C
		public override void OnEnter(NPCFSMContext context, INPCState previousState)
		{
			context.PrepareToStop(0.3f);
			context.ZeroLocomotion();
			context.ResetToIdleAnimation();
			base.OnEnter(context, previousState);
			this._duration = Random.Range(2f, 4f);
			this._hasTriggeredAnimation = false;
			this._animationStartTime = 0f;
			context.DisableAlertLayer();
			context.StopLooking();
			base.Log(context, string.Format("Preparing to stop for confusion, duration: {0:F1}s", this._duration));
		}

		// Token: 0x0600740C RID: 29708 RVA: 0x002F3208 File Offset: 0x002F1408
		private void TriggerConfusionAnimation(NPCFSMContext context)
		{
			if (this._hasTriggeredAnimation)
			{
				return;
			}
			this._hasTriggeredAnimation = true;
			this._animationStartTime = context.TimeInState;
			context.LockNavigation();
			context.EnableReactionsLayer();
			context.SetAlertExpression();
			context.SetTrigger("ConfusionTrigger");
			base.Log(context, "Confusion animation triggered");
		}

		// Token: 0x0600740D RID: 29709 RVA: 0x002F325C File Offset: 0x002F145C
		public override NPCStateType? OnUpdate(NPCFSMContext context)
		{
			if (!this._hasTriggeredAnimation)
			{
				if (context.HasStoppedMoving(0.1f) || context.TimeInState >= 1f)
				{
					this.TriggerConfusionAnimation(context);
				}
				return null;
			}
			if (context.Senses != null)
			{
				float num = 0.5f;
				float currentAttention = context.Senses.CurrentAttention;
				float num2 = Mathf.Max(0f, currentAttention - num * NPCSimulationPause.SimulationDeltaTime);
				context.Senses.SetAttentionState(num2);
			}
			if (context.TimeInState - this._animationStartTime >= this._duration)
			{
				NPCSenses senses = context.Senses;
				if (senses != null)
				{
					senses.SetAttentionState(0f);
				}
				base.Log(context, "Confusion complete, returning to Wandering");
				return new NPCStateType?(NPCStateType.Wandering);
			}
			return null;
		}

		// Token: 0x0600740E RID: 29710 RVA: 0x002F3324 File Offset: 0x002F1524
		public override void OnExit(NPCFSMContext context, INPCState nextState)
		{
			base.OnExit(context, nextState);
			if (nextState == null || nextState.StateType != NPCStateType.Detected)
			{
				context.DisableReactionsLayer();
			}
			if (this._hasTriggeredAnimation)
			{
				context.UnlockNavigation();
			}
		}

		// Token: 0x0600740F RID: 29711 RVA: 0x002F334F File Offset: 0x002F154F
		public override bool CanTransitionTo(NPCStateType targetState)
		{
			if (targetState <= NPCStateType.Stumbling)
			{
				if (targetState - NPCStateType.Idle > 2 && targetState != NPCStateType.Stumbling)
				{
					return false;
				}
			}
			else if (targetState != NPCStateType.Detected && targetState != NPCStateType.Frozen)
			{
				return false;
			}
			return true;
		}

		// Token: 0x040076F4 RID: 30452
		private const float MinDuration = 2f;

		// Token: 0x040076F5 RID: 30453
		private const float MaxDuration = 4f;

		// Token: 0x040076F6 RID: 30454
		private float _duration;

		// Token: 0x040076F7 RID: 30455
		private bool _hasTriggeredAnimation;

		// Token: 0x040076F8 RID: 30456
		private float _animationStartTime;

		// Token: 0x040076F9 RID: 30457
		private const float MaxWaitForStop = 1f;
	}
}
