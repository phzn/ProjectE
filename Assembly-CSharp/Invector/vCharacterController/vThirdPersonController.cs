using System;
using System.Collections;
using UnityEngine;

namespace Invector.vCharacterController
{
	// Token: 0x02000698 RID: 1688
	[vClassHeader("THIRD PERSON CONTROLLER", true, "icon_v2", false, "", iconName = "controllerIcon")]
	public class vThirdPersonController : vThirdPersonAnimator
	{
		// Token: 0x060027CF RID: 10191 RVA: 0x000E37CB File Offset: 0x000E19CB
		protected override void OnDisable()
		{
			base.OnDisable();
		}

		// Token: 0x060027D0 RID: 10192 RVA: 0x000E37D3 File Offset: 0x000E19D3
		public virtual void MoveToPosition(Transform targetPosition)
		{
			this.MoveToPosition(targetPosition.position);
		}

		// Token: 0x060027D1 RID: 10193 RVA: 0x000E37E4 File Offset: 0x000E19E4
		public virtual void MoveToPosition(Vector3 targetPosition)
		{
			Vector3 vector = targetPosition - base.transform.position;
			vector.y = 0f;
			if (vector.magnitude < 0.1f)
			{
				this.input = Vector3.zero;
				this.moveDirection = Vector3.zero;
				return;
			}
			this.input = base.transform.InverseTransformDirection(vector.normalized);
			this.moveDirection = vector.normalized;
		}

		// Token: 0x060027D2 RID: 10194 RVA: 0x000E385C File Offset: 0x000E1A5C
		public virtual void ControlAnimatorRootMotion()
		{
			if (!base.enabled)
			{
				return;
			}
			if (this.isRolling)
			{
				this.RollBehavior();
				return;
			}
			if (this.customAction || this.lockAnimMovement)
			{
				this.StopCharacterWithLerp();
				base.transform.position = base.animator.rootPosition;
				base.transform.rotation = base.animator.rootRotation;
			}
			if (this.useRootMotion)
			{
				this.MoveCharacter(this.moveDirection);
			}
		}

		// Token: 0x060027D3 RID: 10195 RVA: 0x000E38D8 File Offset: 0x000E1AD8
		public virtual void ControlLocomotionType()
		{
			if (this.lockAnimMovement || this.lockMovement || this.customAction)
			{
				return;
			}
			if (!this.lockSetMoveSpeed)
			{
				if ((this.locomotionType.Equals(vThirdPersonMotor.LocomotionType.FreeWithStrafe) && !this.isStrafing) || this.locomotionType.Equals(vThirdPersonMotor.LocomotionType.OnlyFree))
				{
					this.SetControllerMoveSpeed(this.freeSpeed);
					this.SetAnimatorMoveSpeed(this.freeSpeed);
				}
				else if (this.locomotionType.Equals(vThirdPersonMotor.LocomotionType.OnlyStrafe) || (this.locomotionType.Equals(vThirdPersonMotor.LocomotionType.FreeWithStrafe) && this.isStrafing))
				{
					this.isStrafing = true;
					this.SetControllerMoveSpeed(this.strafeSpeed);
					this.SetAnimatorMoveSpeed(this.strafeSpeed);
				}
			}
			if (!this.useRootMotion)
			{
				this.MoveCharacter(this.moveDirection);
			}
		}

		// Token: 0x060027D4 RID: 10196 RVA: 0x000E39D8 File Offset: 0x000E1BD8
		public virtual void ControlRotationType()
		{
			if (this.lockAnimRotation || this.lockRotation || this.customAction || this.isRolling)
			{
				return;
			}
			if (this.input != Vector3.zero || (this.isStrafing ? this.strafeSpeed.rotateWithCamera : this.freeSpeed.rotateWithCamera))
			{
				if (this.lockAnimMovement)
				{
					this.inputSmooth = Vector3.Lerp(this.inputSmooth, this.input, (this.isStrafing ? this.strafeSpeed.movementSmooth : this.freeSpeed.movementSmooth) * Time.deltaTime);
				}
				Vector3 vector = ((((this.isStrafing && base.isGrounded && (!this.isSprinting || !this.sprintOnlyFree)) || (this.freeSpeed.rotateWithCamera && this.input == Vector3.zero)) && this.rotateTarget) ? this.rotateTarget.forward : this.moveDirection);
				this.RotateToDirection(vector);
			}
		}

		// Token: 0x060027D5 RID: 10197 RVA: 0x000E3AEC File Offset: 0x000E1CEC
		public virtual void ControlKeepDirection()
		{
			if (!this.keepDirection)
			{
				this.oldInput = this.input;
				return;
			}
			if ((this.input.magnitude < 0.01f || Vector3.Distance(this.oldInput, this.input) > 0.9f) && this.keepDirection)
			{
				this.keepDirection = false;
			}
		}

		// Token: 0x060027D6 RID: 10198 RVA: 0x000E3B4C File Offset: 0x000E1D4C
		public virtual void UpdateMoveDirection(Transform referenceTransform = null)
		{
			if (this.isRolling && !this.rollControl)
			{
				this.moveDirection = Vector3.Lerp(this.moveDirection, Vector3.zero, (this.isStrafing ? this.strafeSpeed.movementSmooth : this.freeSpeed.movementSmooth) * Time.deltaTime);
				return;
			}
			if (referenceTransform && !this.rotateByWorld)
			{
				Vector3 right = referenceTransform.right;
				right.y = 0f;
				Vector3 vector = Quaternion.AngleAxis(-90f, Vector3.up) * right;
				this.moveDirection = this.inputSmooth.x * right + this.inputSmooth.z * vector;
				Vector3 vector2 = this.input.x * right + this.input.z * vector;
				this.SetInputDirection(vector2);
				return;
			}
			this.moveDirection = new Vector3(this.inputSmooth.x, 0f, this.inputSmooth.z);
			Vector3 vector3 = new Vector3(this.input.x, 0f, this.input.z);
			this.SetInputDirection(vector3);
		}

		// Token: 0x060027D7 RID: 10199 RVA: 0x000E3C94 File Offset: 0x000E1E94
		public virtual void Run(bool value)
		{
			if (value && this.isCrouching && this.hasMovementInput && this.CanExitCrouch())
			{
				this.isCrouching = false;
			}
			if (value && !this.isCrouching && this.hasMovementInput)
			{
				if (base.isGrounded && this.useContinuousRun)
				{
					this.isRunning = !this.isRunning;
					return;
				}
				if (!this.useContinuousRun && !this.isRunning)
				{
					this.isRunning = true;
					return;
				}
			}
			else if (!value && !this.useContinuousRun && this.isRunning)
			{
				this.isRunning = false;
			}
		}

		// Token: 0x060027D8 RID: 10200 RVA: 0x000E3D28 File Offset: 0x000E1F28
		public virtual void Sprint(bool value)
		{
			bool flag = (!this.isCrouching || (!base.inCrouchArea && this.CanExitCrouch())) && (this.currentStamina > 0f && this.hasMovementInput) && (!this.isStrafing || ((double)this.horizontalSpeed < 0.5 && (double)this.horizontalSpeed > -0.5 && this.verticalSpeed > 0.1f) || this.sprintOnlyFree);
			if (value && flag)
			{
				if (this.currentStamina > (this.finishStaminaOnSprint ? this.sprintStamina : 0f) && this.hasMovementInput)
				{
					this.finishStaminaOnSprint = false;
					if (base.isGrounded && this.useContinuousSprint)
					{
						this.isCrouching = false;
						this.isSprinting = !this.isSprinting;
						if (this.isSprinting)
						{
							this.OnStartSprinting.Invoke();
							this.alwaysWalkByDefault = false;
							return;
						}
						this.OnFinishSprinting.Invoke();
						return;
					}
					else if (!this.isSprinting)
					{
						this.OnStartSprinting.Invoke();
						this.alwaysWalkByDefault = false;
						this.isSprinting = true;
						return;
					}
				}
				else if (!this.useContinuousSprint && this.isSprinting)
				{
					if (this.currentStamina <= 0f)
					{
						this.finishStaminaOnSprint = true;
						this.OnFinishSprintingByStamina.Invoke();
					}
					this.isSprinting = false;
					this.OnFinishSprinting.Invoke();
					return;
				}
			}
			else if (this.isSprinting && (!this.useContinuousSprint || !flag))
			{
				if (this.currentStamina <= 0f)
				{
					this.finishStaminaOnSprint = true;
					this.OnFinishSprintingByStamina.Invoke();
				}
				this.isSprinting = false;
				this.OnFinishSprinting.Invoke();
			}
		}

		// Token: 0x060027D9 RID: 10201 RVA: 0x000E3EDA File Offset: 0x000E20DA
		public virtual void Crouch()
		{
			if (base.isGrounded && !this.customAction)
			{
				this.AutoCrouch();
				if (this.isCrouching && this.CanExitCrouch())
				{
					this.isCrouching = false;
					return;
				}
				this.isCrouching = true;
				this.isSprinting = false;
			}
		}

		// Token: 0x060027DA RID: 10202 RVA: 0x000E3F18 File Offset: 0x000E2118
		public virtual void Strafe()
		{
			this.isStrafing = !this.isStrafing;
		}

		// Token: 0x060027DB RID: 10203 RVA: 0x000E3F2C File Offset: 0x000E212C
		public virtual void Jump(bool consumeStamina = false)
		{
			this.jumpCounter = this.jumpTimer;
			this.OnJump.Invoke();
			if (this.input.sqrMagnitude < 0.1f)
			{
				base.StartCoroutine(this.DelayToJump());
				base.animator.CrossFadeInFixedTime("Jump", 0.1f);
			}
			else
			{
				this.isJumping = true;
				base.animator.CrossFadeInFixedTime("JumpMove", 0.2f);
			}
			if (consumeStamina && this.useBuiltInStamina)
			{
				this.ReduceStamina(this.jumpStamina, false);
				this.currentStaminaRecoveryDelay = 1f;
			}
		}

		// Token: 0x060027DC RID: 10204 RVA: 0x000E3FC8 File Offset: 0x000E21C8
		protected IEnumerator DelayToJump()
		{
			this.inJumpStarted = true;
			yield return new WaitForSeconds(this.jumpStandingDelay);
			this.isJumping = true;
			this.inJumpStarted = false;
			yield break;
		}

		// Token: 0x060027DD RID: 10205 RVA: 0x000E3FD8 File Offset: 0x000E21D8
		public virtual void Roll()
		{
			this.OnRoll.Invoke();
			this.isRolling = true;
			base.animator.CrossFadeInFixedTime("Roll", this.rollTransition, this.baseLayer);
			if (this.useBuiltInStamina)
			{
				this.ReduceStamina(this.rollStamina, false);
				this.currentStaminaRecoveryDelay = 2f;
			}
		}

		// Token: 0x060027DE RID: 10206 RVA: 0x000E4034 File Offset: 0x000E2234
		protected override void OnTriggerStay(Collider other)
		{
			try
			{
				this.CheckForAutoCrouch(other);
			}
			catch (UnityException ex)
			{
				Debug.LogWarning(ex.Message);
			}
			base.OnTriggerStay(other);
		}

		// Token: 0x060027DF RID: 10207 RVA: 0x000E4070 File Offset: 0x000E2270
		protected override void OnTriggerExit(Collider other)
		{
			this.AutoCrouchExit(other);
			base.OnTriggerExit(other);
		}
	}
}
