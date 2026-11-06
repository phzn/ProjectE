using System;
using UnityEngine;
using UnityEngine.AI;

namespace Invector.vCharacterController.AI
{
	// Token: 0x020006CA RID: 1738
	public class vSimpleMeleeAI_Animator : vSimpleMeleeAI_Motor
	{
		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x06002BD2 RID: 11218 RVA: 0x000E5E8E File Offset: 0x000E408E
		private int baseLayer
		{
			get
			{
				return base.animator.GetLayerIndex("Base Layer");
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06002BD3 RID: 11219 RVA: 0x000E5EA0 File Offset: 0x000E40A0
		private int underBodyLayer
		{
			get
			{
				return base.animator.GetLayerIndex("UnderBody");
			}
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06002BD4 RID: 11220 RVA: 0x000E5EB2 File Offset: 0x000E40B2
		private int rightArmLayer
		{
			get
			{
				return base.animator.GetLayerIndex("RightArm");
			}
		}

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06002BD5 RID: 11221 RVA: 0x000E5EC4 File Offset: 0x000E40C4
		private int leftArmLayer
		{
			get
			{
				return base.animator.GetLayerIndex("LeftArm");
			}
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x06002BD6 RID: 11222 RVA: 0x000E5ED6 File Offset: 0x000E40D6
		private int upperBodyLayer
		{
			get
			{
				return base.animator.GetLayerIndex("UpperBody");
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06002BD7 RID: 11223 RVA: 0x000E5EE8 File Offset: 0x000E40E8
		private int fullbodyLayer
		{
			get
			{
				return base.animator.GetLayerIndex("FullBody");
			}
		}

		// Token: 0x06002BD8 RID: 11224 RVA: 0x000F3EC0 File Offset: 0x000F20C0
		public void UpdateAnimator(float _speed, float _direction)
		{
			if (base.animator == null || !base.animator.enabled)
			{
				return;
			}
			this.LayerControl();
			this.LocomotionAnimation(_speed, _direction);
			this.RollAnimation();
			this.CrouchAnimation();
			this.ResetAndLockAgent();
			this.MoveSetIDControl();
			this.MeleeATK_Animation();
			this.DEF_Animation();
			this.DeadAnimation();
		}

		// Token: 0x06002BD9 RID: 11225 RVA: 0x000F3F24 File Offset: 0x000F2124
		private void LayerControl()
		{
			this.baseLayerInfo = base.animator.GetCurrentAnimatorStateInfo(this.baseLayer);
			this.underBodyInfo = base.animator.GetCurrentAnimatorStateInfo(this.underBodyLayer);
			this.rightArmInfo = base.animator.GetCurrentAnimatorStateInfo(this.rightArmLayer);
			this.leftArmInfo = base.animator.GetCurrentAnimatorStateInfo(this.leftArmLayer);
			this.upperBodyInfo = base.animator.GetCurrentAnimatorStateInfo(this.upperBodyLayer);
			this.fullBodyInfo = base.animator.GetCurrentAnimatorStateInfo(this.fullbodyLayer);
		}

		// Token: 0x06002BDA RID: 11226 RVA: 0x000F3FBC File Offset: 0x000F21BC
		private void OnAnimatorMove()
		{
			this.actions = this.baseLayerInfo.IsTag("CustomAction") || this.lockMovement;
			if (Time.timeScale == 0f || this.agent == null)
			{
				return;
			}
			if (this.agent.enabled && !this.agent.isOnOffMeshLink && this.agent.updatePosition)
			{
				Vector3 vector = base.animator.deltaPosition / Time.deltaTime;
				if (!vector.IsVectorNaN())
				{
					this.agent.velocity = vector;
				}
				else
				{
					this.agent.velocity = Vector3.zero;
				}
			}
			if (!this._rigidbody.useGravity && !this.actions && !this.agent.isOnOffMeshLink)
			{
				this._rigidbody.velocity = base.animator.deltaPosition;
			}
			if (!this.agent.updatePosition && !this.actions)
			{
				Vector3 vector2 = (this.agent.enabled ? this.agent.nextPosition : this.destination);
				if (Vector3.Distance(base.transform.position, vector2) > 0.5f)
				{
					this.desiredRotation = Quaternion.LookRotation(vector2 - base.transform.position);
					Quaternion quaternion = Quaternion.Euler(base.transform.eulerAngles.x, this.desiredRotation.eulerAngles.y, base.transform.eulerAngles.z);
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, quaternion, this.agent.angularSpeed * Time.deltaTime);
				}
				base.transform.position = base.animator.rootPosition;
				return;
			}
			if (base.OnStrafeArea && !this.actions && this.currentTarget.transform != null && this.canSeeTarget && this.currentHealth > 0f)
			{
				Vector3 vector3 = this.currentTarget.transform.position - base.transform.position;
				float num = ((this.meleeManager != null && base.isAttacking) ? (this.attackRotationSpeed * Time.deltaTime) : (this.strafeRotationSpeed * Time.deltaTime));
				Quaternion quaternion2 = Quaternion.LookRotation(Vector3.RotateTowards(base.transform.forward, vector3, num, 0f));
				base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, quaternion2.eulerAngles.y, base.transform.eulerAngles.z);
				return;
			}
			if (this.agent.isOnOffMeshLink && !this.actions)
			{
				Vector3 endPos = this.agent.nextOffMeshLinkData.endPos;
				this.targetPos = endPos;
				OffMeshLinkData currentOffMeshLinkData = this.agent.currentOffMeshLinkData;
				this.desiredRotation = Quaternion.LookRotation(new Vector3(currentOffMeshLinkData.endPos.x, base.transform.position.y, currentOffMeshLinkData.endPos.z) - base.transform.position);
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, this.desiredRotation, this.agent.angularSpeed * 2f * Time.deltaTime);
				return;
			}
			if (this.agent.desiredVelocity.magnitude <= 0.1f || this.actions || !this.agent.enabled || this.currentHealth <= 0f)
			{
				if (this.actions || this.currentHealth <= 0f || base.isAttacking)
				{
					if (this.isRolling)
					{
						this.desiredRotation = Quaternion.LookRotation(this.rollDirection, Vector3.up);
						base.transform.rotation = this.desiredRotation;
					}
					else
					{
						base.transform.rotation = base.animator.rootRotation;
					}
					if (!this.agent.enabled)
					{
						this.destination = base.transform.position;
						base.transform.position = base.animator.rootPosition;
					}
				}
				return;
			}
			if (this.meleeManager != null && base.isAttacking)
			{
				this.desiredRotation = Quaternion.LookRotation(this.agent.desiredVelocity);
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, this.desiredRotation, this.agent.angularSpeed * this.attackRotationSpeed * Time.deltaTime);
				return;
			}
			this.desiredRotation = Quaternion.LookRotation(this.agent.desiredVelocity);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, this.desiredRotation, this.agent.angularSpeed * Time.deltaTime);
		}

		// Token: 0x06002BDB RID: 11227 RVA: 0x000F44D8 File Offset: 0x000F26D8
		private void LocomotionAnimation(float _speed, float _direction)
		{
			this.isGrounded = (this.agent.enabled ? this.agent.isOnNavMesh : (this.isRolling || this.groundDistance <= this.groundCheckDistance));
			base.animator.SetBool("IsGrounded", this.isGrounded);
			_speed = Mathf.Clamp(_speed, -this.maxSpeed, this.maxSpeed);
			if (base.OnStrafeArea)
			{
				_direction = Mathf.Clamp(_direction, -this.strafeSpeed, this.strafeSpeed);
			}
			float num = ((Mathf.Abs(_speed) > 0.1f) ? _speed : 0f);
			float num2 = ((Mathf.Abs(_direction) > 0.1f) ? _direction : 0f);
			Vector2 vector = new Vector2(num, num2);
			this.strafeInput = Mathf.Clamp(vector.magnitude, 0f, 1.5f);
			base.animator.SetFloat("InputMagnitude", this.strafeInput, 0.2f, Time.deltaTime);
			base.animator.SetFloat("InputVertical", this.actions ? 0f : ((_speed != 0f) ? _speed : 0f), 0.2f, Time.fixedDeltaTime);
			base.animator.SetFloat("InputHorizontal", _direction, 0.2f, Time.fixedDeltaTime);
			base.animator.SetBool("IsStrafing", base.OnStrafeArea);
			base.animator.SetBool("isDead", this.isDead);
		}

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06002BDC RID: 11228 RVA: 0x000F465A File Offset: 0x000F285A
		protected virtual float maxSpeed
		{
			get
			{
				if (!base.OnStrafeArea)
				{
					return this.chaseSpeed;
				}
				return this.strafeSpeed;
			}
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x000F4674 File Offset: 0x000F2874
		private void DeadAnimation()
		{
			if (!this.isDead)
			{
				return;
			}
			if (!this.triggerDieBehaviour)
			{
				this.triggerDieBehaviour = true;
				this.DeathBehaviour();
			}
			if (this.deathBy == vCharacter.DeathBy.Animation)
			{
				if (this.fullBodyInfo.IsName("Dead") && this.fullBodyInfo.normalizedTime >= 0.99f && this.groundDistance <= 0.15f)
				{
					base.RemoveComponents();
					return;
				}
			}
			else if (this.deathBy == vCharacter.DeathBy.AnimationWithRagdoll)
			{
				if (this.fullBodyInfo.IsName("Dead") && this.fullBodyInfo.normalizedTime >= 0.8f)
				{
					base.onActiveRagdoll.Invoke(null);
					base.RemoveComponents();
					return;
				}
			}
			else if (this.deathBy == vCharacter.DeathBy.Ragdoll)
			{
				base.onActiveRagdoll.Invoke(null);
				base.RemoveComponents();
			}
		}

		// Token: 0x06002BDE RID: 11230 RVA: 0x000F473D File Offset: 0x000F293D
		private void DeathBehaviour()
		{
			base.animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
			if (this.deathBy == vCharacter.DeathBy.Animation || this.deathBy == vCharacter.DeathBy.AnimationWithRagdoll)
			{
				base.animator.SetBool("isDead", this.isDead);
			}
		}

		// Token: 0x06002BDF RID: 11231 RVA: 0x000F4772 File Offset: 0x000F2972
		private void CrouchAnimation()
		{
			base.animator.SetBool("IsCrouching", this.isCrouched);
			if (base.animator != null && base.animator.enabled)
			{
				base.CheckAutoCrouch();
			}
		}

		// Token: 0x06002BE0 RID: 11232 RVA: 0x000F47AC File Offset: 0x000F29AC
		protected void RollAnimation()
		{
			if (base.animator == null || !base.animator.enabled)
			{
				return;
			}
			this.isRolling = this.baseLayerInfo.IsName("Roll");
			if (this.isRolling)
			{
				this._rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
				this._rigidbody.useGravity = true;
				this.agent.enabled = false;
				this.agent.updatePosition = false;
			}
		}

		// Token: 0x06002BE1 RID: 11233 RVA: 0x000F4824 File Offset: 0x000F2A24
		private void ResetAIRotation()
		{
			base.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, 0f);
		}

		// Token: 0x06002BE2 RID: 11234 RVA: 0x000F4850 File Offset: 0x000F2A50
		private void MoveSetIDControl()
		{
			if (this.meleeManager == null)
			{
				return;
			}
			base.animator.SetFloat("MoveSet_ID", (float)this.meleeManager.GetMoveSetID());
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x000F487D File Offset: 0x000F2A7D
		private void MeleeATK_Animation()
		{
			if (this.meleeManager == null)
			{
				return;
			}
			if (this.actions)
			{
				this.attackCount = 0;
			}
			base.animator.SetInteger("AttackID", this.meleeManager.GetAttackID());
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x000F48B8 File Offset: 0x000F2AB8
		private void DEF_Animation()
		{
			if (this.meleeManager == null)
			{
				return;
			}
			if (this.isBlocking)
			{
				base.animator.SetInteger("DefenseID", this.meleeManager.GetDefenseID());
			}
			base.animator.SetBool("IsBlocking", this.isBlocking);
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x000F490D File Offset: 0x000F2B0D
		public void MeleeAttack()
		{
			if (base.animator != null && base.animator.enabled && !this.actions)
			{
				base.animator.SetTrigger("WeakAttack");
			}
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x000F4944 File Offset: 0x000F2B44
		private void ResetAndLockAgent()
		{
			this.lockMovement = this.fullBodyInfo.IsTag("LockMovement") || this.upperBodyInfo.IsTag("ResetState");
			if (this.lockMovement)
			{
				if (this.attackCount > 0)
				{
					this.canAttack = false;
					this.attackCount = 0;
				}
				if (this.baseLayerInfo.normalizedTime > 0.1f)
				{
					base.animator.ResetTrigger("ResetState");
					this._rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
					this._rigidbody.useGravity = true;
					this.agent.enabled = false;
					this.agent.updatePosition = false;
				}
			}
		}

		// Token: 0x06002BE7 RID: 11239 RVA: 0x000F49F0 File Offset: 0x000F2BF0
		public void TriggerRecoil(int recoil_id)
		{
			if (base.animator != null && base.animator.enabled && !this.isRolling)
			{
				base.animator.SetInteger("RecoilID", recoil_id);
				base.animator.SetTrigger("TriggerRecoil");
				base.animator.SetTrigger("ResetState");
			}
		}

		// Token: 0x04002B18 RID: 11032
		private bool triggerDieBehaviour;

		// Token: 0x04002B19 RID: 11033
		private bool resetState;

		// Token: 0x04002B1A RID: 11034
		private float strafeInput;

		// Token: 0x04002B1B RID: 11035
		public AnimatorStateInfo baseLayerInfo;

		// Token: 0x04002B1C RID: 11036
		public AnimatorStateInfo rightArmInfo;

		// Token: 0x04002B1D RID: 11037
		public AnimatorStateInfo leftArmInfo;

		// Token: 0x04002B1E RID: 11038
		public AnimatorStateInfo fullBodyInfo;

		// Token: 0x04002B1F RID: 11039
		public AnimatorStateInfo upperBodyInfo;

		// Token: 0x04002B20 RID: 11040
		public AnimatorStateInfo underBodyInfo;
	}
}
