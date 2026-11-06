using System;
using Features.Police;
using UnityEngine;

namespace Features.NPC.StateMachine.States
{
	// Token: 0x02000F19 RID: 3865
	public class TaseredState : NPCStateBase
	{
		// Token: 0x17001309 RID: 4873
		// (get) Token: 0x06007451 RID: 29777 RVA: 0x002F407B File Offset: 0x002F227B
		public override NPCStateType StateType
		{
			get
			{
				return NPCStateType.Tasered;
			}
		}

		// Token: 0x1700130A RID: 4874
		// (get) Token: 0x06007452 RID: 29778 RVA: 0x0002AE8C File Offset: 0x0002908C
		public override bool LockProps
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700130B RID: 4875
		// (get) Token: 0x06007453 RID: 29779 RVA: 0x0002AE8C File Offset: 0x0002908C
		public override bool LockNavigation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007454 RID: 29780 RVA: 0x002F4080 File Offset: 0x002F2280
		public override void OnEnter(NPCFSMContext context, INPCState previousState)
		{
			context.PrepareToStop(0f);
			context.ZeroLocomotion();
			context.ResetToIdleAnimation();
			base.OnEnter(context, previousState);
			this._sensesWasEnabled = context.Senses != null && context.Senses.enabled;
			if (context.Senses != null)
			{
				context.Senses.enabled = false;
			}
			context.StopLooking();
			context.EnableReactionsLayer();
			int num = Random.Range(0, 2);
			context.SetFloat("TaseredVariant", (float)num);
			context.SetTrigger("TaseredTrigger");
			this._reactionsLayerIndex = ((context.Animator != null) ? context.Animator.GetLayerIndex("Reactions") : (-1));
			this._enteredTaseredOnce = false;
			TaserSfx.PlayShockOn(context.GameObject);
			base.Log(context, string.Format("NPC tasered, variant={0}", num));
		}

		// Token: 0x06007455 RID: 29781 RVA: 0x002F4164 File Offset: 0x002F2364
		public override NPCStateType? OnUpdate(NPCFSMContext context)
		{
			if (context.TimeInState >= 10f)
			{
				base.Log(context, "Tasered safety timeout, returning to Idle");
				return new NPCStateType?(NPCStateType.Idle);
			}
			if (context.Animator == null || this._reactionsLayerIndex < 0)
			{
				if (context.TimeInState >= 0.3f)
				{
					return new NPCStateType?(NPCStateType.Idle);
				}
				return null;
			}
			else
			{
				if (context.TimeInState < 0.3f)
				{
					return null;
				}
				AnimatorStateInfo currentAnimatorStateInfo = context.Animator.GetCurrentAnimatorStateInfo(this._reactionsLayerIndex);
				if (currentAnimatorStateInfo.tagHash == TaseredState.TaseredTagHash)
				{
					this._enteredTaseredOnce = true;
				}
				if (this._enteredTaseredOnce && currentAnimatorStateInfo.tagHash != TaseredState.TaseredTagHash && !context.Animator.IsInTransition(this._reactionsLayerIndex))
				{
					base.Log(context, "Tasered animation chain finished, returning to Idle");
					return new NPCStateType?(NPCStateType.Idle);
				}
				return null;
			}
		}

		// Token: 0x06007456 RID: 29782 RVA: 0x002F424C File Offset: 0x002F244C
		public override void OnExit(NPCFSMContext context, INPCState nextState)
		{
			base.OnExit(context, nextState);
			if (context.Senses != null)
			{
				context.Senses.enabled = this._sensesWasEnabled;
			}
			context.ResetFacialExpression();
			context.DisableReactionsLayer();
		}

		// Token: 0x06007457 RID: 29783 RVA: 0x002F4281 File Offset: 0x002F2481
		public override bool CanTransitionTo(NPCStateType targetState)
		{
			return targetState == NPCStateType.Idle || targetState.GetPriority() > this.StateType.GetPriority();
		}

		// Token: 0x04007711 RID: 30481
		private const float SafetyTimeout = 10f;

		// Token: 0x04007712 RID: 30482
		private const float MinTimeBeforeExitCheck = 0.3f;

		// Token: 0x04007713 RID: 30483
		private const int VariantCount = 2;

		// Token: 0x04007714 RID: 30484
		private static readonly int TaseredTagHash = Animator.StringToHash("Tasered");

		// Token: 0x04007715 RID: 30485
		private bool _sensesWasEnabled;

		// Token: 0x04007716 RID: 30486
		private int _reactionsLayerIndex = -1;

		// Token: 0x04007717 RID: 30487
		private bool _enteredTaseredOnce;
	}
}
