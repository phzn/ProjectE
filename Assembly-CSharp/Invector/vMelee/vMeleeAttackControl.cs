using System;
using System.Collections.Generic;
using Invector.vEventSystems;
using UnityEngine;

namespace Invector.vMelee
{
	// Token: 0x0200050D RID: 1293
	public class vMeleeAttackControl : StateMachineBehaviour
	{
		// Token: 0x06001F63 RID: 8035 RVA: 0x000BAE08 File Offset: 0x000B9008
		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			this.mFighter = animator.GetComponent<vIAttackListener>();
			this.isAttacking = true;
			if (this.mFighter != null)
			{
				this.mFighter.OnEnableAttack();
			}
			if (this.debug)
			{
				Debug.Log("Enter " + this.damageType);
			}
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x000BAE58 File Offset: 0x000B9058
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (stateInfo.normalizedTime % 1f >= this.startDamage && stateInfo.normalizedTime % 1f <= this.endDamage && !this.isActive)
			{
				if (this.debug)
				{
					Debug.Log(string.Concat(new string[]
					{
						animator.name,
						" attack ",
						this.damageType,
						" enable damage in ",
						Math.Round((double)(stateInfo.normalizedTime % 1f), 2).ToString()
					}));
				}
				this.isActive = true;
				this.ActiveDamage(animator, true);
			}
			else if (stateInfo.normalizedTime % 1f > this.endDamage && this.isActive)
			{
				if (this.debug)
				{
					Debug.Log(string.Concat(new string[]
					{
						animator.name,
						" attack ",
						this.damageType,
						" disable damage in ",
						Math.Round((double)(stateInfo.normalizedTime % 1f), 2).ToString()
					}));
				}
				this.isActive = false;
				this.ActiveDamage(animator, false);
			}
			if (stateInfo.normalizedTime % 1f > this.endDamage && this.isAttacking)
			{
				this.isAttacking = false;
				if (this.mFighter != null)
				{
					this.mFighter.OnDisableAttack();
				}
			}
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x000BAFC4 File Offset: 0x000B91C4
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (this.debug)
			{
				Debug.Log("Exit " + this.damageType);
			}
			if (this.isActive)
			{
				this.isActive = false;
				this.ActiveDamage(animator, false);
			}
			if (this.isAttacking)
			{
				this.isAttacking = false;
				if (this.mFighter != null)
				{
					this.mFighter.OnDisableAttack();
				}
			}
			if (this.mFighter != null && this.resetAttackTrigger)
			{
				this.mFighter.ResetAttackTriggers();
			}
			if (this.debug)
			{
				Debug.Log(animator.name + " attack " + this.damageType + " stateExit");
			}
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x000BB06C File Offset: 0x000B926C
		private void ActiveDamage(Animator animator, bool value)
		{
			vMeleeManager component = animator.GetComponent<vMeleeManager>();
			if (component)
			{
				component.SetActiveAttack(this.bodyParts, this.meleeAttackType, value, this.damageMultiplier, this.recoilID, this.reactionID, this.ignoreDefense, this.activeRagdoll, this.senselessTime, this.damageType);
			}
		}

		// Token: 0x04002007 RID: 8199
		[Tooltip("normalizedTime of Active Damage")]
		public float startDamage = 0.05f;

		// Token: 0x04002008 RID: 8200
		[Tooltip("normalizedTime of Disable Damage")]
		public float endDamage = 0.9f;

		// Token: 0x04002009 RID: 8201
		public int damageMultiplier;

		// Token: 0x0400200A RID: 8202
		public int recoilID;

		// Token: 0x0400200B RID: 8203
		public int reactionID;

		// Token: 0x0400200C RID: 8204
		public vAttackType meleeAttackType;

		// Token: 0x0400200D RID: 8205
		[Tooltip("You can use a name as reference to trigger a custom HitDamageParticle")]
		public string damageType;

		// Token: 0x0400200E RID: 8206
		[HideInInspector]
		[Header("This work with vMeleeManager to active vMeleeAttackObject from bodyPart and id")]
		public List<string> bodyParts = new List<string> { "RightLowerArm" };

		// Token: 0x0400200F RID: 8207
		public bool ignoreDefense;

		// Token: 0x04002010 RID: 8208
		public bool activeRagdoll;

		// Token: 0x04002011 RID: 8209
		[vHideInInspector("activeRagdoll", false)]
		[Tooltip("Time to keep Ragdoll active")]
		public float senselessTime;

		// Token: 0x04002012 RID: 8210
		[Tooltip("Check true in the last attack of your combo to reset the triggers")]
		public bool resetAttackTrigger;

		// Token: 0x04002013 RID: 8211
		private bool isActive;

		// Token: 0x04002014 RID: 8212
		public bool debug;

		// Token: 0x04002015 RID: 8213
		private vIAttackListener mFighter;

		// Token: 0x04002016 RID: 8214
		private bool isAttacking;
	}
}
