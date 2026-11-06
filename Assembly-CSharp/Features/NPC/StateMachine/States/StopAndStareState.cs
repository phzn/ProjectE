using System;
using Features.Detection;
using UnityEngine;

namespace Features.NPC.StateMachine.States
{
	// Token: 0x02000F17 RID: 3863
	public class StopAndStareState : NPCStateBase
	{
		// Token: 0x17001303 RID: 4867
		// (get) Token: 0x0600743C RID: 29756 RVA: 0x002F3A34 File Offset: 0x002F1C34
		public override NPCStateType StateType
		{
			get
			{
				return NPCStateType.StopAndStare;
			}
		}

		// Token: 0x17001304 RID: 4868
		// (get) Token: 0x0600743D RID: 29757 RVA: 0x0002AE8C File Offset: 0x0002908C
		public override bool LockProps
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001305 RID: 4869
		// (get) Token: 0x0600743E RID: 29758 RVA: 0x0000D91D File Offset: 0x0000BB1D
		public override bool LockNavigation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600743F RID: 29759 RVA: 0x002F3A38 File Offset: 0x002F1C38
		public override void OnEnter(NPCFSMContext context, INPCState previousState)
		{
			context.PrepareToStop(0.3f);
			context.ZeroLocomotion();
			context.ResetToIdleAnimation();
			base.OnEnter(context, previousState);
			this._lostSightElapsed = -1f;
			this._hasTriggeredAnimation = false;
			this._currentAnimation = null;
			this._passiveReactionActive = false;
			this._passiveReactionAnimation = null;
			this._duration = Random.Range(1.5f, 3f);
			this._hasLastVisualPosition = context.DetectionSource == DetectionSource.Visual && context.PlayerPosition != null;
			if (this._hasLastVisualPosition)
			{
				context.LastSeenPlayerPosition = context.PlayerPosition.Value;
			}
			context.StartLookingAtPlayer();
			base.Log(context, "Preparing to stop for StopAndStare");
		}

		// Token: 0x06007440 RID: 29760 RVA: 0x002F3AF0 File Offset: 0x002F1CF0
		private void TriggerSurprisedAnimation(NPCFSMContext context)
		{
			if (this._hasTriggeredAnimation)
			{
				return;
			}
			this._hasTriggeredAnimation = true;
			context.LockNavigation();
			context.EnableReactionsLayer();
			context.SetAlertExpression();
			this._currentAnimation = context.GetRandomStopAndStareAnimation();
			context.SetTrigger("SurprisedTrigger");
			if (this._currentAnimation != null)
			{
				context.SetFloat("SurprisedIndex", (float)this._currentAnimation.animationIndex);
				context.ActivateAlertProps(this._currentAnimation);
				base.Log(context, string.Format("Surprised animation triggered: {0} (index={1})", this._currentAnimation.displayName, this._currentAnimation.animationIndex));
				return;
			}
			context.SetFloat("SurprisedIndex", (float)Random.Range(0, 3));
			base.Log(context, "Surprised animation triggered (fallback, no profile)");
		}

		// Token: 0x06007441 RID: 29761 RVA: 0x002F3BAD File Offset: 0x002F1DAD
		public override NPCStateType? OnUpdate(NPCFSMContext context)
		{
			return this.Tick(context, NPCSimulationPause.SimulationDeltaTime);
		}

		// Token: 0x06007442 RID: 29762 RVA: 0x002F3BBC File Offset: 0x002F1DBC
		internal NPCStateType? Tick(NPCFSMContext context, float deltaTime)
		{
			this.RefreshPassiveReaction(context);
			this.UpdateVisualTracking(context, deltaTime);
			if (this._lostSightElapsed >= 2f)
			{
				base.Log(context, "Lost visual contact for too long, going to Confusion");
				return new NPCStateType?(NPCStateType.Confusion);
			}
			if (!this._hasTriggeredAnimation)
			{
				if (context.HasStoppedMoving(0.1f) || context.TimeInState >= 1f)
				{
					this.TriggerSurprisedAnimation(context);
				}
				return null;
			}
			float attention = context.Attention;
			if (attention >= 0.99f)
			{
				base.Log(context, "Attention maxed out, escalating to Detected");
				return new NPCStateType?(NPCStateType.Detected);
			}
			if (attention < 0.2f)
			{
				base.Log(context, string.Format("Attention dropped to {0:F2}, going to Confusion", attention));
				return new NPCStateType?(NPCStateType.Confusion);
			}
			return null;
		}

		// Token: 0x06007443 RID: 29763 RVA: 0x002F3C80 File Offset: 0x002F1E80
		private void UpdateVisualTracking(NPCFSMContext context, float deltaTime)
		{
			if (context.DetectionSource == DetectionSource.Visual && context.PlayerPosition != null)
			{
				this._hasLastVisualPosition = true;
				context.LastSeenPlayerPosition = context.PlayerPosition.Value;
				this._lostSightElapsed = -1f;
				context.RotateTowardWithDelta(context.LastSeenPlayerPosition, 5f, deltaTime);
				context.StartLookingAtPlayer();
				return;
			}
			if (this._lostSightElapsed < 0f)
			{
				this._lostSightElapsed = 0f;
				context.StopLooking();
			}
			if (this._hasLastVisualPosition && this._lostSightElapsed < 2f)
			{
				context.RotateTowardWithDelta(context.LastSeenPlayerPosition, 5f, deltaTime);
			}
			this._lostSightElapsed += deltaTime;
		}

		// Token: 0x06007444 RID: 29764 RVA: 0x002F3D38 File Offset: 0x002F1F38
		private void RefreshPassiveReaction(NPCFSMContext context)
		{
			if (!(context.Senses != null) || !context.Senses.IsPassiveReacting)
			{
				if (this._passiveReactionActive)
				{
					this.ReleasePassiveReaction(context);
					context.SetInt("ReactionType", 0);
				}
				return;
			}
			if (this._passiveReactionActive)
			{
				return;
			}
			this._passiveReactionAnimation = context.Senses.PassiveReactionEntry;
			if (this._passiveReactionAnimation != null && this._currentAnimation != null)
			{
				context.DeactivateAlertProps(this._currentAnimation);
				this._currentAnimation = null;
			}
			context.LockNavigation();
			context.ZeroLocomotion();
			context.EnableReactionsLayer();
			context.StartLookingAtPlayer();
			context.ActivateAlertProps(this._passiveReactionAnimation);
			this._passiveReactionActive = true;
			this._hasTriggeredAnimation = true;
		}

		// Token: 0x06007445 RID: 29765 RVA: 0x002F3DEB File Offset: 0x002F1FEB
		private void ReleasePassiveReaction(NPCFSMContext context)
		{
			if (this._passiveReactionAnimation != null)
			{
				context.DeactivateAlertProps(this._passiveReactionAnimation);
			}
			this._passiveReactionAnimation = null;
			this._passiveReactionActive = false;
		}

		// Token: 0x06007446 RID: 29766 RVA: 0x002F3E10 File Offset: 0x002F2010
		public override void OnExit(NPCFSMContext context, INPCState nextState)
		{
			base.OnExit(context, nextState);
			context.StopLooking();
			this.ReleasePassiveReaction(context);
			if (context.Senses != null && !context.Senses.IsPassiveReacting)
			{
				context.SetInt("ReactionType", 0);
			}
			if (this._currentAnimation != null)
			{
				context.DeactivateAlertProps(this._currentAnimation);
				this._currentAnimation = null;
			}
			if (nextState != null && nextState.StateType != NPCStateType.Confusion && nextState.StateType != NPCStateType.Detected)
			{
				context.DisableReactionsLayer();
			}
			if (this._hasTriggeredAnimation)
			{
				context.UnlockNavigation();
			}
		}

		// Token: 0x06007447 RID: 29767 RVA: 0x002F3A01 File Offset: 0x002F1C01
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

		// Token: 0x04007703 RID: 30467
		private const float MinDuration = 1.5f;

		// Token: 0x04007704 RID: 30468
		private const float MaxDuration = 3f;

		// Token: 0x04007705 RID: 30469
		private float _duration;

		// Token: 0x04007706 RID: 30470
		private const float ConfusionThreshold = 0.2f;

		// Token: 0x04007707 RID: 30471
		private float _lostSightElapsed = -1f;

		// Token: 0x04007708 RID: 30472
		private const float LostSightGracePeriod = 2f;

		// Token: 0x04007709 RID: 30473
		private bool _hasLastVisualPosition;

		// Token: 0x0400770A RID: 30474
		private bool _hasTriggeredAnimation;

		// Token: 0x0400770B RID: 30475
		private const float MaxWaitForStop = 1f;

		// Token: 0x0400770C RID: 30476
		private AlertAnimationEntry _currentAnimation;

		// Token: 0x0400770D RID: 30477
		private bool _passiveReactionActive;

		// Token: 0x0400770E RID: 30478
		private NPCReactionEntry _passiveReactionAnimation;
	}
}
