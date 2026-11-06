using System;
using Features.Detection;
using UnityEngine;

namespace Features.NPC.StateMachine.States
{
	// Token: 0x02000F16 RID: 3862
	public class SearchingState : NPCStateBase
	{
		// Token: 0x17001300 RID: 4864
		// (get) Token: 0x06007434 RID: 29748 RVA: 0x002F356A File Offset: 0x002F176A
		public override NPCStateType StateType
		{
			get
			{
				return NPCStateType.Searching;
			}
		}

		// Token: 0x17001301 RID: 4865
		// (get) Token: 0x06007435 RID: 29749 RVA: 0x0002AE8C File Offset: 0x0002908C
		public override bool LockProps
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001302 RID: 4866
		// (get) Token: 0x06007436 RID: 29750 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public override bool LockNavigation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06007437 RID: 29751 RVA: 0x002F3570 File Offset: 0x002F1770
		public override void OnEnter(NPCFSMContext context, INPCState previousState)
		{
			base.OnEnter(context, previousState);
			this._isActivelyTracking = true;
			this._lostSightElapsed = -1f;
			this._currentAnimation = null;
			this._passiveReactionNavLocked = false;
			this._passiveReactionAnimation = null;
			context.ZeroLocomotion();
			context.ResetToIdleAnimation();
			context.EnableAlertLayer();
			if (context.PlayerPosition != null)
			{
				context.LastSeenPlayerPosition = context.PlayerPosition.Value;
			}
			context.SetDestination(context.LastSeenPlayerPosition);
			context.StopLooking();
			context.SetAlertExpression();
			this._currentAnimation = context.GetRandomSearchingAnimation();
			context.SetTrigger("SearchingTrigger");
			if (this._currentAnimation != null)
			{
				float num = (float)this._currentAnimation.animationIndex / 4f;
				context.SetFloat("SearchingType", num);
				context.ActivateAlertProps(this._currentAnimation);
				base.Log(context, string.Format("Started searching: {0} (index={1}, normalized={2:F2})", this._currentAnimation.displayName, this._currentAnimation.animationIndex, num));
				return;
			}
			context.SetFloat("SearchingType", Random.Range(0f, 1f));
			base.Log(context, string.Format("Started searching/chasing toward {0} (fallback, no profile)", context.LastSeenPlayerPosition));
		}

		// Token: 0x06007438 RID: 29752 RVA: 0x002F36B0 File Offset: 0x002F18B0
		public override NPCStateType? OnUpdate(NPCFSMContext context)
		{
			DetectionSource detectionSource = context.DetectionSource;
			bool flag = detectionSource == DetectionSource.Visual;
			bool flag2 = detectionSource > DetectionSource.None;
			if (context.Senses != null && context.Senses.IsPassiveReacting)
			{
				if (!this._passiveReactionNavLocked)
				{
					context.LockNavigation();
					context.ZeroLocomotion();
					context.StartLookingAtPlayer();
					this._passiveReactionNavLocked = true;
					if (this._currentAnimation != null)
					{
						context.DeactivateAlertProps(this._currentAnimation);
					}
					if (context.Senses.PassiveReactionEntry != null)
					{
						context.ActivateAlertProps(context.Senses.PassiveReactionEntry);
						this._passiveReactionAnimation = context.Senses.PassiveReactionEntry;
					}
				}
				if (flag && context.PlayerPosition != null)
				{
					context.LastSeenPlayerPosition = context.PlayerPosition.Value;
					context.RotateToward(context.PlayerPosition.Value, 5f);
				}
				if (!flag2)
				{
					if (this._lostSightElapsed < 0f)
					{
						this._lostSightElapsed = 0f;
					}
					this._lostSightElapsed += NPCSimulationPause.SimulationDeltaTime;
					this._isActivelyTracking = false;
					if (this._lostSightElapsed >= 2f)
					{
						base.Log(context, string.Format("Passive reaction: lost player for {0}s, going to Confusion", 2f));
						return new NPCStateType?(NPCStateType.Confusion);
					}
				}
				else
				{
					this._lostSightElapsed = -1f;
					this._isActivelyTracking = true;
				}
				return null;
			}
			if (this._passiveReactionNavLocked)
			{
				context.UnlockNavigation();
				context.StopLooking();
				if (this._passiveReactionAnimation != null)
				{
					context.DeactivateAlertProps(this._passiveReactionAnimation);
					this._passiveReactionAnimation = null;
				}
				this._passiveReactionNavLocked = false;
			}
			if (flag2)
			{
				this._lostSightElapsed = -1f;
				this._isActivelyTracking = true;
				if (flag && context.PlayerPosition != null)
				{
					context.LastSeenPlayerPosition = context.PlayerPosition.Value;
					context.SetDestination(context.PlayerPosition.Value);
				}
				else
				{
					context.SetDestination(context.LastSeenPlayerPosition);
				}
			}
			else
			{
				if (this._lostSightElapsed < 0f)
				{
					this._lostSightElapsed = 0f;
				}
				this._lostSightElapsed += NPCSimulationPause.SimulationDeltaTime;
				this._isActivelyTracking = false;
				context.SetDestination(context.LastSeenPlayerPosition);
			}
			if (!this._isActivelyTracking && this._lostSightElapsed >= 0f && this._lostSightElapsed >= 2f)
			{
				base.Log(context, string.Format("Lost player for {0:F1}s, going to Confusion", this._lostSightElapsed));
				return new NPCStateType?(NPCStateType.Confusion);
			}
			if (context.Attention >= 0.99f)
			{
				base.Log(context, "Attention maxed out, escalating to Detected");
				return new NPCStateType?(NPCStateType.Detected);
			}
			return null;
		}

		// Token: 0x06007439 RID: 29753 RVA: 0x002F3954 File Offset: 0x002F1B54
		public override void OnExit(NPCFSMContext context, INPCState nextState)
		{
			base.OnExit(context, nextState);
			context.StopLooking();
			if (this._passiveReactionNavLocked)
			{
				context.UnlockNavigation();
				this._passiveReactionNavLocked = false;
			}
			if (context.Senses != null && !context.Senses.IsPassiveReacting)
			{
				context.SetInt("ReactionType", 0);
			}
			if (this._passiveReactionAnimation != null)
			{
				context.DeactivateAlertProps(this._passiveReactionAnimation);
				this._passiveReactionAnimation = null;
			}
			if (this._currentAnimation != null)
			{
				context.DeactivateAlertProps(this._currentAnimation);
				this._currentAnimation = null;
			}
			if (nextState != null && !nextState.StateType.IsAlertState() && nextState.StateType != NPCStateType.Detected)
			{
				context.DisableAlertLayer();
			}
		}

		// Token: 0x0600743A RID: 29754 RVA: 0x002F3A01 File Offset: 0x002F1C01
		public override bool CanTransitionTo(NPCStateType targetState)
		{
			if (targetState <= NPCStateType.Stumbling)
			{
				if (targetState != NPCStateType.Confusion && targetState != NPCStateType.Stumbling)
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

		// Token: 0x040076FD RID: 30461
		private bool _isActivelyTracking;

		// Token: 0x040076FE RID: 30462
		private float _lostSightElapsed = -1f;

		// Token: 0x040076FF RID: 30463
		private AlertAnimationEntry _currentAnimation;

		// Token: 0x04007700 RID: 30464
		private const float LostSightTimeout = 2f;

		// Token: 0x04007701 RID: 30465
		private bool _passiveReactionNavLocked;

		// Token: 0x04007702 RID: 30466
		private NPCReactionEntry _passiveReactionAnimation;
	}
}
