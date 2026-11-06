using System;
using Invector.vEventSystems;
using UnityEngine;

namespace Invector.vCharacterController
{
	// Token: 0x02000696 RID: 1686
	public class vThirdPersonAnimator : vThirdPersonMotor
	{
		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x060027BB RID: 10171 RVA: 0x000E2CB4 File Offset: 0x000E0EB4
		// (set) Token: 0x060027BC RID: 10172 RVA: 0x000E2CBC File Offset: 0x000E0EBC
		public Vector3 lastCharacterPosition { get; protected set; }

		// Token: 0x060027BD RID: 10173 RVA: 0x000E2CC5 File Offset: 0x000E0EC5
		protected override void Start()
		{
			base.Start();
			this.RegisterAnimatorStateInfos();
		}

		// Token: 0x060027BE RID: 10174 RVA: 0x000E2CD3 File Offset: 0x000E0ED3
		protected virtual void RegisterAnimatorStateInfos()
		{
			base.animatorStateInfos = new vAnimatorStateInfos(base.GetComponent<Animator>());
			base.animatorStateInfos.RegisterListener();
		}

		// Token: 0x060027BF RID: 10175 RVA: 0x000E2CF1 File Offset: 0x000E0EF1
		protected override void OnEnable()
		{
			base.OnEnable();
			if (base.animatorStateInfos.animator != null)
			{
				base.animatorStateInfos.RegisterListener();
			}
		}

		// Token: 0x060027C0 RID: 10176 RVA: 0x000E2D17 File Offset: 0x000E0F17
		protected override void OnDisable()
		{
			base.OnDisable();
			base.animatorStateInfos.RemoveListener();
		}

		// Token: 0x060027C1 RID: 10177 RVA: 0x000E2D2A File Offset: 0x000E0F2A
		public virtual void UpdateAnimator()
		{
			if (base.animator == null || !base.animator.enabled)
			{
				return;
			}
			this.AnimatorLayerControl();
			this.ActionsControl();
			this.TriggerRandomIdle();
			this.UpdateAnimatorParameters();
			this.DeadAnimation();
		}

		// Token: 0x060027C2 RID: 10178 RVA: 0x000E2D68 File Offset: 0x000E0F68
		public virtual void AnimatorLayerControl()
		{
			this.baseLayerInfo = base.animator.GetCurrentAnimatorStateInfo(this.baseLayer);
			this.underBodyInfo = base.animator.GetCurrentAnimatorStateInfo(this.underBodyLayer);
			this.rightArmInfo = base.animator.GetCurrentAnimatorStateInfo(this.rightArmLayer);
			this.leftArmInfo = base.animator.GetCurrentAnimatorStateInfo(this.leftArmLayer);
			this.upperBodyInfo = base.animator.GetCurrentAnimatorStateInfo(this.upperBodyLayer);
			this.fullBodyInfo = base.animator.GetCurrentAnimatorStateInfo(this.fullbodyLayer);
		}

		// Token: 0x060027C3 RID: 10179 RVA: 0x000E2E00 File Offset: 0x000E1000
		public virtual void ActionsControl()
		{
			this.isRolling = this.IsAnimatorTag("IsRolling");
			this.isTurningOnSpot = this.IsAnimatorTag("TurnOnSpot");
			this.lockAnimMovement = this.IsAnimatorTag("LockMovement");
			this.lockAnimRotation = this.IsAnimatorTag("LockRotation");
			this.customAction = this.IsAnimatorTag("CustomAction");
			this.isInAirborne = this.IsAnimatorTag("Airborne");
		}

		// Token: 0x060027C4 RID: 10180 RVA: 0x000E2E74 File Offset: 0x000E1074
		public virtual void UpdateAnimatorParameters()
		{
			if (this.disableAnimations)
			{
				return;
			}
			base.animator.SetBool(vAnimatorParameters.IsStrafing, this.isStrafing);
			base.animator.SetBool(vAnimatorParameters.IsRunning, this.isRunning);
			base.animator.SetBool(vAnimatorParameters.IsSprinting, this.isSprinting);
			base.animator.SetBool(vAnimatorParameters.IsSliding, base.isSliding && !this.isRolling);
			base.animator.SetBool(vAnimatorParameters.IsCrouching, this.isCrouching);
			base.animator.SetBool(vAnimatorParameters.IsGrounded, base.isGrounded);
			base.animator.SetBool(vAnimatorParameters.IsDead, this.isDead);
			base.animator.SetFloat(vAnimatorParameters.GroundDistance, this.groundDistance);
			base.animator.SetFloat(vAnimatorParameters.GroundAngle, this.GroundAngleFromDirection());
			if (!base.isGrounded)
			{
				base.animator.SetFloat(vAnimatorParameters.VerticalVelocity, this.verticalVelocity);
			}
			if (this.isStrafing)
			{
				base.animator.SetFloat(vAnimatorParameters.InputHorizontal, this.horizontalSpeed, this.strafeSpeed.animationSmooth, Time.fixedDeltaTime);
				base.animator.SetFloat(vAnimatorParameters.InputVertical, this.verticalSpeed, this.strafeSpeed.animationSmooth, Time.fixedDeltaTime);
			}
			else
			{
				base.animator.SetFloat(vAnimatorParameters.InputVertical, this.verticalSpeed, this.freeSpeed.animationSmooth, Time.fixedDeltaTime);
				base.animator.SetFloat(vAnimatorParameters.InputHorizontal, 0f, this.freeSpeed.animationSmooth, Time.fixedDeltaTime);
			}
			base.animator.SetFloat(vAnimatorParameters.InputMagnitude, Mathf.LerpUnclamped(this.inputMagnitude, 0f, this.stopMoveWeight), this.isStrafing ? this.strafeSpeed.animationSmooth : this.freeSpeed.animationSmooth, Time.fixedDeltaTime);
			if (this.useLeanMovementAnim && this.inputMagnitude >= 0.1f)
			{
				base.animator.SetFloat(vAnimatorParameters.RotationMagnitude, this.rotationMagnitude, this.leanSmooth, Time.fixedDeltaTime);
				return;
			}
			if (this.useTurnOnSpotAnim && this.inputMagnitude < 0.1f)
			{
				base.animator.SetFloat(vAnimatorParameters.RotationMagnitude, (float)Math.Round((double)this.rotationMagnitude, 2), this.turnOnSpotSmooth, Time.fixedDeltaTime);
			}
		}

		// Token: 0x060027C5 RID: 10181 RVA: 0x000E30E0 File Offset: 0x000E12E0
		public virtual void SetAnimatorMoveSpeed(vThirdPersonMotor.vMovementSpeed speed)
		{
			Vector3 vector = base.transform.InverseTransformDirection(this.moveDirection);
			this.verticalSpeed = vector.z;
			this.horizontalSpeed = vector.x;
			Vector2 vector2 = new Vector2(this.verticalSpeed, this.horizontalSpeed);
			if (speed.walkByDefault || this.alwaysWalkByDefault)
			{
				this.inputMagnitude = Mathf.Clamp(vector2.magnitude, 0f, (this.isRunning || this.isSprinting) ? 1f : 0.5f);
				return;
			}
			float magnitude = vector2.magnitude;
			this.sprintWeight = Mathf.Lerp(this.sprintWeight, this.isSprinting ? 1f : 0f, (this.isStrafing ? this.strafeSpeed.movementSmooth : this.freeSpeed.movementSmooth) * Time.fixedDeltaTime);
			this.inputMagnitude = Mathf.Clamp(Mathf.Lerp(magnitude, magnitude + ((vector2.magnitude > 0.1f) ? 0.5f : 0f), this.sprintWeight), 0f, this.isSprinting ? 1.5f : 1f);
		}

		// Token: 0x060027C6 RID: 10182 RVA: 0x000E3210 File Offset: 0x000E1410
		public virtual void SetInputDirection(Vector3 direction)
		{
			float num = (((double)direction.magnitude >= 0.1) ? base.transform.forward.AngleFormOtherDirection(direction).y : 0f);
			if (vAnimatorParameters.InputDirection != -1)
			{
				base.animator.SetFloat(vAnimatorParameters.InputDirection, num);
			}
		}

		// Token: 0x060027C7 RID: 10183 RVA: 0x000E3268 File Offset: 0x000E1468
		public virtual void ResetInputAnimatorParameters()
		{
			base.animator.SetBool(vAnimatorParameters.IsSprinting, false);
			base.animator.SetBool(vAnimatorParameters.IsSliding, false);
			base.animator.SetBool(vAnimatorParameters.IsCrouching, false);
			base.animator.SetBool(vAnimatorParameters.IsGrounded, true);
			base.animator.SetFloat(vAnimatorParameters.GroundDistance, 0f);
			base.animator.SetFloat("InputHorizontal", 0f);
			base.animator.SetFloat("InputVertical", 0f);
			base.animator.SetFloat("InputMagnitude", 0f);
		}

		// Token: 0x060027C8 RID: 10184 RVA: 0x000E3310 File Offset: 0x000E1510
		protected virtual void TriggerRandomIdle()
		{
			if (this.input != Vector3.zero || this.customAction)
			{
				return;
			}
			if (this.randomIdleTime > 0f)
			{
				if (this.input.sqrMagnitude == 0f && !this.isCrouching && this._capsuleCollider.enabled && base.isGrounded)
				{
					this.randomIdleCount += Time.fixedDeltaTime;
					if (this.randomIdleCount > 6f)
					{
						this.randomIdleCount = 0f;
						base.animator.SetTrigger(vAnimatorParameters.IdleRandomTrigger);
						base.animator.SetInteger(vAnimatorParameters.IdleRandom, Random.Range(1, 4));
						return;
					}
				}
				else
				{
					this.randomIdleCount = 0f;
					base.animator.SetInteger(vAnimatorParameters.IdleRandom, 0);
				}
			}
		}

		// Token: 0x060027C9 RID: 10185 RVA: 0x000E33E8 File Offset: 0x000E15E8
		protected virtual void DeadAnimation()
		{
			if (!this.isDead)
			{
				return;
			}
			if (this.deathBy == vCharacter.DeathBy.Animation)
			{
				int num = 0;
				vAnimatorStateInfos.vStateInfo stateInfoUsingTag = base.animatorStateInfos.GetStateInfoUsingTag("Dead");
				if (stateInfoUsingTag != null && !base.animator.IsInTransition(num) && stateInfoUsingTag.normalizedTime >= 0.99f && this.groundDistance <= 0.15f)
				{
					base.RemoveComponents();
					return;
				}
			}
			else if (this.deathBy == vCharacter.DeathBy.AnimationWithRagdoll)
			{
				int num2 = 0;
				vAnimatorStateInfos.vStateInfo stateInfoUsingTag2 = base.animatorStateInfos.GetStateInfoUsingTag("Dead");
				if (stateInfoUsingTag2 != null && !base.animator.IsInTransition(num2) && stateInfoUsingTag2.normalizedTime >= 0.8f)
				{
					base.onActiveRagdoll.Invoke(null);
					return;
				}
			}
			else if (this.deathBy == vCharacter.DeathBy.Ragdoll)
			{
				base.onActiveRagdoll.Invoke(null);
			}
		}

		// Token: 0x060027CA RID: 10186 RVA: 0x000E34AA File Offset: 0x000E16AA
		public virtual void SetActionState(int value)
		{
			base.animator.SetInteger(vAnimatorParameters.ActionState, value);
		}

		// Token: 0x060027CB RID: 10187 RVA: 0x000E34C0 File Offset: 0x000E16C0
		public virtual bool IsAnimatorTag(string tag)
		{
			return !(base.animator == null) && ((base.animatorStateInfos != null && base.animatorStateInfos.HasTag(tag)) || this.baseLayerInfo.IsTag(tag) || this.underBodyInfo.IsTag(tag) || this.rightArmInfo.IsTag(tag) || this.leftArmInfo.IsTag(tag) || this.upperBodyInfo.IsTag(tag) || this.fullBodyInfo.IsTag(tag));
		}

		// Token: 0x060027CC RID: 10188 RVA: 0x000E3558 File Offset: 0x000E1758
		public virtual void MatchTarget(Vector3 matchPosition, Quaternion matchRotation, AvatarTarget target, MatchTargetWeightMask weightMask, float normalisedStartTime, float normalisedEndTime)
		{
			if (base.animator.isMatchingTarget || base.animator.IsInTransition(0))
			{
				return;
			}
			if (Mathf.Repeat(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1f) > normalisedEndTime)
			{
				return;
			}
			base.animator.MatchTarget(matchPosition, matchRotation, target, weightMask, normalisedStartTime, normalisedEndTime);
		}

		// Token: 0x04002844 RID: 10308
		public float randomIdleCount;

		// Token: 0x04002845 RID: 10309
		public const float walkSpeed = 0.5f;

		// Token: 0x04002846 RID: 10310
		public const float runningSpeed = 1f;

		// Token: 0x04002847 RID: 10311
		public const float sprintSpeed = 1.5f;
	}
}
