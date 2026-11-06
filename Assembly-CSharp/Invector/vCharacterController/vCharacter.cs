using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vCharacterController
{
	// Token: 0x0200068B RID: 1675
	[vClassHeader("vCharacter", true, "icon_v2", false, "")]
	[Serializable]
	public class vCharacter : vHealthController, vICharacter, vIHealthController, vIDamageReceiver
	{
		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x0600275C RID: 10076 RVA: 0x000E0FBB File Offset: 0x000DF1BB
		// (set) Token: 0x0600275D RID: 10077 RVA: 0x000E0FC3 File Offset: 0x000DF1C3
		public Animator animator { get; protected set; }

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x0600275E RID: 10078 RVA: 0x000E0FCC File Offset: 0x000DF1CC
		// (set) Token: 0x0600275F RID: 10079 RVA: 0x000E0FD4 File Offset: 0x000DF1D4
		public virtual bool ragdolled
		{
			get
			{
				return this._ragdolled;
			}
			set
			{
				this._ragdolled = value;
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06002760 RID: 10080 RVA: 0x000E0FDD File Offset: 0x000DF1DD
		// (set) Token: 0x06002761 RID: 10081 RVA: 0x000E0FE5 File Offset: 0x000DF1E5
		public OnActiveRagdoll onActiveRagdoll
		{
			get
			{
				return this._onActiveRagdoll;
			}
			protected set
			{
				this._onActiveRagdoll = value;
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06002762 RID: 10082 RVA: 0x000E0FEE File Offset: 0x000DF1EE
		// (set) Token: 0x06002763 RID: 10083 RVA: 0x000E0FF6 File Offset: 0x000DF1F6
		public virtual bool isCrouching
		{
			get
			{
				return this._isCrouching;
			}
			set
			{
				if (value != this._isCrouching)
				{
					if (value)
					{
						this.OnCrouch.Invoke();
					}
					else
					{
						this.OnStandUp.Invoke();
					}
				}
				this._isCrouching = value;
			}
		}

		// Token: 0x06002764 RID: 10084 RVA: 0x000E1024 File Offset: 0x000DF224
		public virtual void Init()
		{
			this.animator = base.GetComponent<Animator>();
			if (this.animator)
			{
				this.hitDirectionHash = new vAnimatorParameter(this.animator, "HitDirection");
				this.reactionIDHash = new vAnimatorParameter(this.animator, "ReactionID");
				this.triggerReactionHash = new vAnimatorParameter(this.animator, "TriggerReaction");
				this.triggerResetStateHash = new vAnimatorParameter(this.animator, "ResetState");
				this.recoilIDHash = new vAnimatorParameter(this.animator, "RecoilID");
				this.triggerRecoilHash = new vAnimatorParameter(this.animator, "TriggerRecoil");
			}
			this.LoadActionControllers(this.debugActionListener);
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x00004F31 File Offset: 0x00003131
		public virtual void ResetRagdoll()
		{
		}

		// Token: 0x06002766 RID: 10086 RVA: 0x00004F31 File Offset: 0x00003131
		public virtual void EnableRagdoll()
		{
		}

		// Token: 0x06002767 RID: 10087 RVA: 0x000E10DD File Offset: 0x000DF2DD
		protected virtual void OnTriggerEnter(Collider other)
		{
			this.onActionEnter.Invoke(other);
		}

		// Token: 0x06002768 RID: 10088 RVA: 0x000E10EB File Offset: 0x000DF2EB
		protected virtual void OnTriggerStay(Collider other)
		{
			this.onActionStay.Invoke(other);
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x000E10F9 File Offset: 0x000DF2F9
		protected virtual void OnTriggerExit(Collider other)
		{
			this.onActionExit.Invoke(other);
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x000E1107 File Offset: 0x000DF307
		public override void TakeDamage(vDamage damage)
		{
			base.TakeDamage(damage);
			this.TriggerDamageReaction(damage);
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x000E1118 File Offset: 0x000DF318
		protected virtual void TriggerDamageReaction(vDamage damage)
		{
			if (this.animator != null && this.animator.enabled && !damage.activeRagdoll && this.currentHealth > 0f)
			{
				if (this.hitDirectionHash.isValid && damage.sender)
				{
					this.animator.SetInteger(this.hitDirectionHash, (int)base.transform.HitAngle(damage.sender.position, true));
				}
				if (damage.hitReaction)
				{
					if (this.reactionIDHash.isValid)
					{
						this.animator.SetInteger(this.reactionIDHash, damage.reaction_id);
					}
					if (this.triggerReactionHash.isValid)
					{
						this.SetAnimatorTrigger(this.triggerReactionHash);
					}
					if (this.triggerResetStateHash.isValid)
					{
						this.SetAnimatorTrigger(this.triggerResetStateHash);
					}
				}
				else
				{
					if (this.recoilIDHash.isValid)
					{
						this.animator.SetInteger(this.recoilIDHash, damage.recoil_id);
					}
					if (this.triggerRecoilHash.isValid)
					{
						this.SetAnimatorTrigger(this.triggerRecoilHash);
					}
					if (this.triggerResetStateHash.isValid)
					{
						this.SetAnimatorTrigger(this.triggerResetStateHash);
					}
				}
			}
			if (damage.activeRagdoll)
			{
				this.onActiveRagdoll.Invoke(damage);
			}
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x000E1291 File Offset: 0x000DF491
		private IEnumerator SetTriggerRoutine(int trigger)
		{
			this.animator.SetTrigger(trigger);
			yield return new WaitForSeconds(0.1f);
			this.animator.ResetTrigger(trigger);
			yield break;
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x000E12A7 File Offset: 0x000DF4A7
		public virtual void SetAnimatorTrigger(int trigger)
		{
			base.StartCoroutine(this.SetTriggerRoutine(trigger));
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x000AD00A File Offset: 0x000AB20A
		Transform vIDamageReceiver.get_transform()
		{
			return base.transform;
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x000B0592 File Offset: 0x000AE792
		GameObject vIDamageReceiver.get_gameObject()
		{
			return base.gameObject;
		}

		// Token: 0x040027F0 RID: 10224
		[vEditorToolbar("Health", false, "", false, false)]
		public vCharacter.DeathBy deathBy;

		// Token: 0x040027F1 RID: 10225
		public bool removeComponentsAfterDie;

		// Token: 0x040027F2 RID: 10226
		[vEditorToolbar("Debug", false, "", false, false, order = 9)]
		[HideInInspector]
		public bool debugActionListener;

		// Token: 0x040027F4 RID: 10228
		public bool _ragdolled;

		// Token: 0x040027F5 RID: 10229
		[vEditorToolbar("Events", false, "", false, false)]
		public UnityEvent OnCrouch;

		// Token: 0x040027F6 RID: 10230
		public UnityEvent OnStandUp;

		// Token: 0x040027F7 RID: 10231
		[SerializeField]
		protected OnActiveRagdoll _onActiveRagdoll = new OnActiveRagdoll();

		// Token: 0x040027F8 RID: 10232
		public UnityEvent onDisableRagdoll;

		// Token: 0x040027F9 RID: 10233
		[Header("Check if Character is in Trigger with tag Action")]
		[HideInInspector]
		public OnActionHandle onActionEnter = new OnActionHandle();

		// Token: 0x040027FA RID: 10234
		[HideInInspector]
		public OnActionHandle onActionStay = new OnActionHandle();

		// Token: 0x040027FB RID: 10235
		[HideInInspector]
		public OnActionHandle onActionExit = new OnActionHandle();

		// Token: 0x040027FC RID: 10236
		protected vAnimatorParameter hitDirectionHash;

		// Token: 0x040027FD RID: 10237
		protected vAnimatorParameter reactionIDHash;

		// Token: 0x040027FE RID: 10238
		protected vAnimatorParameter triggerReactionHash;

		// Token: 0x040027FF RID: 10239
		protected vAnimatorParameter triggerResetStateHash;

		// Token: 0x04002800 RID: 10240
		protected vAnimatorParameter recoilIDHash;

		// Token: 0x04002801 RID: 10241
		protected vAnimatorParameter triggerRecoilHash;

		// Token: 0x04002802 RID: 10242
		protected bool isInit;

		// Token: 0x04002803 RID: 10243
		protected bool _isCrouching;

		// Token: 0x0200068C RID: 1676
		public enum DeathBy
		{
			// Token: 0x04002805 RID: 10245
			Animation,
			// Token: 0x04002806 RID: 10246
			AnimationWithRagdoll,
			// Token: 0x04002807 RID: 10247
			Ragdoll
		}
	}
}
