using System;
using Features.Detection;
using Features.NPC.StateMachine;
using Features.Police;
using Pathfinding;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000ECF RID: 3791
	[DisallowMultipleComponent]
	public sealed class NPCAmbientSuspension : MonoBehaviour
	{
		// Token: 0x17001284 RID: 4740
		// (get) Token: 0x06007187 RID: 29063 RVA: 0x002E1EC9 File Offset: 0x002E00C9
		// (set) Token: 0x06007188 RID: 29064 RVA: 0x002E1ED1 File Offset: 0x002E00D1
		public bool IsSuspended { get; private set; }

		// Token: 0x17001285 RID: 4741
		// (get) Token: 0x06007189 RID: 29065 RVA: 0x002E1EDA File Offset: 0x002E00DA
		public bool IsInitialized
		{
			get
			{
				return this._reaction != null;
			}
		}

		// Token: 0x17001286 RID: 4742
		// (get) Token: 0x0600718A RID: 29066 RVA: 0x002E1EE8 File Offset: 0x002E00E8
		public float AwarenessRadius
		{
			get
			{
				return Mathf.Max((this._senses != null) ? NpcVisionDifficultyPolicy.ApplyRadius(this._senses.EffectiveViewRadius) : 0f, Mathf.Max((this._reaction != null) ? this._reaction.EffectiveTriggerRange : 0f, (this._fieldLook != null) ? this._fieldLook.EffectiveExitRange : 0f));
			}
		}

		// Token: 0x0600718B RID: 29067 RVA: 0x002E1F64 File Offset: 0x002E0164
		public void Initialize(SeatedRandomReaction reaction)
		{
			if (this._reaction != null || reaction == null)
			{
				return;
			}
			this._reaction = reaction;
			this._senses = base.GetComponent<NPCSenses>();
			this._fieldLook = base.GetComponent<FieldPlayerLookAtCiel>();
			this._look = base.GetComponent<NPCSimpleLookAt>();
			this._attention = base.GetComponent<AttentionUIController>();
			this._controller = base.GetComponent<AStarNPCController>();
			this._follower = base.GetComponent<FollowerEntity>();
			this._stateMachine = base.GetComponent<NPCStateMachine>();
			this._recovery = base.GetComponent<NPCRagdollRecovery>();
			this._stumble = base.GetComponent<NPCStumbleHandler>();
			this._animators = base.GetComponentsInChildren<Animator>(true);
			this._renderers = base.GetComponentsInChildren<Renderer>(true);
			this._animatorEnabled = new bool[this._animators.Length];
			this._keepAnimatorState = new bool[this._animators.Length];
			this._renderingOff = new bool[this._renderers.Length];
			this._reaction.ProximitySuspension = this;
			if (this._fieldLook != null)
			{
				this._fieldLook.ProximitySuspension = this;
			}
			if (this._look != null)
			{
				this._look.ProximitySuspension = this;
			}
			if (this._attention != null)
			{
				this._attention.ProximitySuspension = this;
			}
		}

		// Token: 0x17001287 RID: 4743
		// (get) Token: 0x0600718C RID: 29068 RVA: 0x002E20AC File Offset: 0x002E02AC
		public bool HasGameplayOwnership
		{
			get
			{
				if (this._reaction == null || this._reaction.IsReacting)
				{
					return true;
				}
				if (this._recovery == null)
				{
					base.TryGetComponent<NPCRagdollRecovery>(out this._recovery);
				}
				return (this._controller != null && (this._controller.enabled || this._controller.IsManualMode)) || (this._follower != null && this._follower.enabled) || (this._stateMachine != null && this._stateMachine.IsInitialized && !this._stateMachine.IsInNormalState) || (this._recovery != null && this._recovery.IsRecovering) || (this._stumble != null && this._stumble.IsStumbling) || (this._senses != null && (NPCTaserApplier.HasFallbackStun(this._senses) || this._senses.IsAlert || this._senses.IsDetected || this._senses.IsSearching || this._senses.IsPassiveReacting || this._senses.IsGhostFleeing || this._senses.IsInCooldown || this._senses.HasLineOfSight || this._senses.HasAudioContact || this._senses.CurrentAttention > 0f));
			}
		}

		// Token: 0x0600718D RID: 29069 RVA: 0x002E2234 File Offset: 0x002E0434
		public void SetSuspended(bool suspended)
		{
			if (suspended == this.IsSuspended || !this.IsInitialized)
			{
				return;
			}
			if (suspended && (!base.gameObject.activeInHierarchy || this.HasGameplayOwnership))
			{
				return;
			}
			this.IsSuspended = suspended;
			if (suspended)
			{
				this._sensesEnabled = this._senses != null && this._senses.enabled;
				if (this._sensesEnabled)
				{
					this._senses.enabled = false;
				}
				for (int i = 0; i < this._animators.Length; i++)
				{
					Animator animator = this._animators[i];
					if (!(animator == null))
					{
						this._animatorEnabled[i] = animator.enabled;
						this._keepAnimatorState[i] = animator.keepAnimatorStateOnDisable;
						animator.keepAnimatorStateOnDisable = true;
						animator.enabled = false;
					}
				}
				for (int j = 0; j < this._renderers.Length; j++)
				{
					if (!(this._renderers[j] == null))
					{
						this._renderingOff[j] = this._renderers[j].forceRenderingOff;
						this._renderers[j].forceRenderingOff = true;
					}
				}
				return;
			}
			for (int k = 0; k < this._animators.Length; k++)
			{
				Animator animator2 = this._animators[k];
				if (!(animator2 == null))
				{
					animator2.enabled = this._animatorEnabled[k];
					animator2.keepAnimatorStateOnDisable = this._keepAnimatorState[k];
				}
			}
			for (int l = 0; l < this._renderers.Length; l++)
			{
				if (this._renderers[l] != null)
				{
					this._renderers[l].forceRenderingOff = this._renderingOff[l];
				}
			}
			if (this._sensesEnabled && this._senses != null)
			{
				this._senses.enabled = true;
			}
		}

		// Token: 0x0600718E RID: 29070 RVA: 0x002E23EB File Offset: 0x002E05EB
		private void OnDisable()
		{
			this.SetSuspended(false);
		}

		// Token: 0x0600718F RID: 29071 RVA: 0x002E23F4 File Offset: 0x002E05F4
		private void OnDestroy()
		{
			this.SetSuspended(false);
			if (this._reaction != null && this._reaction.ProximitySuspension == this)
			{
				this._reaction.ProximitySuspension = null;
			}
			if (this._fieldLook != null && this._fieldLook.ProximitySuspension == this)
			{
				this._fieldLook.ProximitySuspension = null;
			}
			if (this._look != null && this._look.ProximitySuspension == this)
			{
				this._look.ProximitySuspension = null;
			}
			if (this._attention != null && this._attention.ProximitySuspension == this)
			{
				this._attention.ProximitySuspension = null;
			}
		}

		// Token: 0x0400747E RID: 29822
		private SeatedRandomReaction _reaction;

		// Token: 0x0400747F RID: 29823
		private NPCSenses _senses;

		// Token: 0x04007480 RID: 29824
		private FieldPlayerLookAtCiel _fieldLook;

		// Token: 0x04007481 RID: 29825
		private NPCSimpleLookAt _look;

		// Token: 0x04007482 RID: 29826
		private AttentionUIController _attention;

		// Token: 0x04007483 RID: 29827
		private AStarNPCController _controller;

		// Token: 0x04007484 RID: 29828
		private FollowerEntity _follower;

		// Token: 0x04007485 RID: 29829
		private NPCStateMachine _stateMachine;

		// Token: 0x04007486 RID: 29830
		private NPCRagdollRecovery _recovery;

		// Token: 0x04007487 RID: 29831
		private NPCStumbleHandler _stumble;

		// Token: 0x04007488 RID: 29832
		private Animator[] _animators;

		// Token: 0x04007489 RID: 29833
		private Renderer[] _renderers;

		// Token: 0x0400748A RID: 29834
		private bool[] _animatorEnabled;

		// Token: 0x0400748B RID: 29835
		private bool[] _keepAnimatorState;

		// Token: 0x0400748C RID: 29836
		private bool[] _renderingOff;

		// Token: 0x0400748D RID: 29837
		private bool _sensesEnabled;
	}
}
