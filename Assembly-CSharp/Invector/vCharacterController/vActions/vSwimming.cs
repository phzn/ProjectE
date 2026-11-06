using System;
using System.Collections.Generic;
using Features.PoolDive;
using PoolWaterTools;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x0200070C RID: 1804
	[vClassHeader("Swimming Action", true, "icon_v2", false, "")]
	public class vSwimming : vActionListener
	{
		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06002E91 RID: 11921 RVA: 0x00106A6E File Offset: 0x00104C6E
		// (set) Token: 0x06002E92 RID: 11922 RVA: 0x00106A76 File Offset: 0x00104C76
		public virtual bool isSwimming
		{
			get
			{
				return this._isSwimming;
			}
			set
			{
				this._isSwimming = value;
			}
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06002E93 RID: 11923 RVA: 0x00106A80 File Offset: 0x00104C80
		public bool IsClimbingOutOfWater
		{
			get
			{
				if (this._genericAction == null)
				{
					this._genericAction = base.GetComponentInParent<vGenericAction>(true);
				}
				return this._genericAction != null && this._genericAction.doingAction && this._genericAction.triggerAction != null && this._genericAction.triggerAction.playAnimation == "ClimbUp";
			}
		}

		// Token: 0x06002E94 RID: 11924 RVA: 0x00106AF4 File Offset: 0x00104CF4
		protected override void Start()
		{
			ControllerContextBindings.Set(this.swimUpInput, "A");
			ControllerContextBindings.Set(this.swimDownInput, "RT");
			base.Start();
			this.tpInput = base.GetComponentInParent<vThirdPersonInput>();
			if (this.tpInput)
			{
				this.tpInput.onUpdate -= this.UpdateSwimmingBehavior;
				this.tpInput.onUpdate += this.UpdateSwimmingBehavior;
			}
		}

		// Token: 0x06002E95 RID: 11925 RVA: 0x00106B70 File Offset: 0x00104D70
		protected override void SetUpListener()
		{
			base.actionEnter = true;
			base.actionExit = true;
			base.actionStay = true;
		}

		// Token: 0x06002E96 RID: 11926 RVA: 0x00106B88 File Offset: 0x00104D88
		protected virtual void OnDrawGizmosSelected()
		{
			if (!this.water)
			{
				return;
			}
			Matrix4x4 matrix = Gizmos.matrix;
			Vector3 vector = new Vector3(base.transform.position.x, this.waterHeightLevel, base.transform.position.z);
			Gizmos.color = Color.blue * 0.8f;
			Gizmos.matrix = Matrix4x4.TRS(vector, Quaternion.identity, new Vector3(1f, 0.001f, 1f));
			Gizmos.DrawWireSphere(Vector3.zero, 0.5f);
			Gizmos.matrix = matrix;
			Gizmos.color = Color.green * 0.8f;
			Gizmos.DrawLine(vector, this.characterCenter);
			Gizmos.matrix = Matrix4x4.TRS(this.characterCenter, Quaternion.identity, new Vector3(1f, 0.001f, 1f));
			Gizmos.DrawWireSphere(Vector3.zero, 0.25f);
		}

		// Token: 0x06002E97 RID: 11927 RVA: 0x00106C78 File Offset: 0x00104E78
		protected virtual void UpdateSwimmingBehavior()
		{
			if (this.IsClimbingOutOfWater)
			{
				this._climbSuspendedSwimming = true;
				return;
			}
			if (this._climbSuspendedSwimming)
			{
				this._climbSuspendedSwimming = false;
				this.ExitSwimState();
			}
			if (!this.inTheWater)
			{
				return;
			}
			if (!this.HasActiveWaterVolume())
			{
				if (this.tpInput != null)
				{
					PoolDiveController componentInChildren = this.tpInput.GetComponentInChildren<PoolDiveController>(true);
					if (componentInChildren != null)
					{
						componentInChildren.InterruptForEncounter();
					}
				}
				this.ForceExitSwimming();
				if (this.tpInput != null)
				{
					PoolEntryStateGuard componentInChildren2 = this.tpInput.GetComponentInChildren<PoolEntryStateGuard>(true);
					if (componentInChildren2 == null)
					{
						return;
					}
					componentInChildren2.RestoreForEncounter();
				}
				return;
			}
			this.UnderWaterBehaviour();
			this.SwimmingBehaviour();
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x00106D18 File Offset: 0x00104F18
		private bool HasActiveWaterVolume()
		{
			if (this.water == null || !this.water.activeInHierarchy)
			{
				return false;
			}
			this.water.GetComponents<Collider>(this._waterColliders);
			for (int i = 0; i < this._waterColliders.Count; i++)
			{
				if (this._waterColliders[i] != null && this._waterColliders[i].enabled)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x00106D94 File Offset: 0x00104F94
		protected virtual void SwimmingBehaviour()
		{
			if (this.water)
			{
				this.waterHeightLevel = this.water.transform.position.y;
				this.currentCharacterDepth = -(this.characterCenter.y - this.waterHeightLevel);
				this.isUnderWater = this.currentCharacterDepth > 0.5f;
				this.isSwimming = (this.isSwimming ? (this.currentCharacterDepth >= -0.2f) : (this.currentCharacterDepth >= 0f));
			}
			if (this.isSwimming)
			{
				if (this.tpInput.cc.currentHealth > 0f)
				{
					if (!this.triggerSwimState)
					{
						this.EnterSwimState();
					}
					this.SwimUpOrDownInput();
					this.tpInput.SetStrafeLocomotion(false);
					this.tpInput.MoveInput();
					this.tpInput.cc.SetAnimatorMoveSpeed(this.tpInput.cc.freeSpeed);
					return;
				}
			}
			else
			{
				this.ExitSwimState();
			}
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x00106E98 File Offset: 0x00105098
		protected virtual void UnderWaterBehaviour()
		{
			if (this.isUnderWater)
			{
				this.StaminaConsumption();
				if (!this.triggerUnderWater)
				{
					this.tpInput.cc.colliderRadius = this.colliderRadius;
					this.tpInput.cc.colliderHeight = this.colliderHeight;
					this.triggerUnderWater = true;
					this.triggerAboveWater = false;
					this.OnUnderWater.Invoke();
					return;
				}
			}
			else
			{
				this.WaterRingEffect();
				if (!this.triggerAboveWater && this.triggerSwimState)
				{
					this.tpInput.cc.ResetCapsule();
					this.triggerUnderWater = false;
					this.triggerAboveWater = true;
					this.OnAboveWater.Invoke();
				}
			}
		}

		// Token: 0x06002E9B RID: 11931 RVA: 0x00106F44 File Offset: 0x00105144
		protected virtual void StaminaConsumption()
		{
			if (this.tpInput.cc.currentStamina <= 0f)
			{
				this.tpInput.cc.ChangeHealth(-this.healthConsumption);
				return;
			}
			this.tpInput.cc.ReduceStamina(this.stamina, true);
			this.tpInput.cc.currentStaminaRecoveryDelay = 0.25f;
		}

		// Token: 0x06002E9C RID: 11932 RVA: 0x00106FAC File Offset: 0x001051AC
		public override void OnActionEnter(Collider other)
		{
			if (other == null || !other.gameObject.CompareTag(this.waterTag) || this.tpInput == null || this.tpInput.cc == null)
			{
				return;
			}
			if (this.IsClimbingOutOfWater)
			{
				return;
			}
			if (this.inTheWater && this.water == other.gameObject)
			{
				return;
			}
			PoolDiveController componentInChildren = this.tpInput.GetComponentInChildren<PoolDiveController>(true);
			bool flag = componentInChildren != null && componentInChildren.IsWaterHandoffActive;
			if (flag && this.tpInput.cc.customAction)
			{
				return;
			}
			if (this.tpInput.cc.customAction && this.characterCenter.y > other.transform.position.y)
			{
				return;
			}
			if (!flag)
			{
				PoolWaterEntryPreparation.Prepare(this.tpInput.transform);
			}
			if (!this.tpInput.cc.customAction)
			{
				if (this.debugMode)
				{
					Debug.Log("Player enter the Water");
				}
				PoolEntryStateGuard componentInChildren2 = this.tpInput.GetComponentInChildren<PoolEntryStateGuard>(true);
				if (componentInChildren2 != null)
				{
					componentInChildren2.CaptureSnapshot("water trigger entry");
				}
				this.inTheWater = true;
				this.water = other.gameObject;
				this.waterHeightLevel = other.transform.position.y;
				this.originalMoveSpeed = this.tpInput.cc.moveSpeed;
				this.originalRotationSpeed = this.tpInput.cc.freeSpeed.rotationSpeed;
				this.originalAnimationSmooth = this.tpInput.cc.freeSpeed.animationSmooth;
				this.originalMovementSmooth = this.tpInput.cc.freeSpeed.movementSmooth;
				if (this.tpInput.cc.verticalVelocity <= this.velocityToImpact && this.impactEffect)
				{
					Vector3 vector = new Vector3(base.transform.position.x, other.transform.position.y, base.transform.position.z);
					Object.Instantiate<GameObject>(this.impactEffect, vector, this.tpInput.transform.rotation).transform.SetParent(vObjectContainer.root, true);
				}
			}
		}

		// Token: 0x06002E9D RID: 11933 RVA: 0x001071F0 File Offset: 0x001053F0
		public override void OnActionStay(Collider other)
		{
			if (!this.inTheWater)
			{
				this.OnActionEnter(other);
			}
		}

		// Token: 0x06002E9E RID: 11934 RVA: 0x00107204 File Offset: 0x00105404
		public override void OnActionExit(Collider other)
		{
			if (other.gameObject.CompareTag(this.waterTag))
			{
				if (this.debugMode)
				{
					Debug.Log("Player left the Water");
				}
				if (other.gameObject == this.water)
				{
					this.water = null;
					this.inTheWater = false;
					this.isSwimming = false;
					this.ExitSwimState();
					if (this.waterDrops)
					{
						Vector3 vector = new Vector3(base.transform.position.x, base.transform.position.y + this.waterDropsYOffset, base.transform.position.z);
						Object.Instantiate<GameObject>(this.waterDrops, vector, this.tpInput.transform.rotation).transform.parent = base.transform;
					}
				}
			}
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x001072E4 File Offset: 0x001054E4
		protected virtual void EnterSwimState()
		{
			if (this.IsClimbingOutOfWater)
			{
				return;
			}
			PoolDiveController componentInChildren = this.tpInput.GetComponentInChildren<PoolDiveController>(true);
			if (componentInChildren == null || !componentInChildren.IsWaterHandoffActive)
			{
				PoolWaterEntryPreparation.Prepare(this.tpInput.transform);
			}
			if (this.debugMode)
			{
				Debug.Log("Player is Swimming");
			}
			this.triggerUnderWater = false;
			this.triggerAboveWater = false;
			this.triggerSwimState = true;
			this.AcquireSwimmingActivity();
			this.OnEnterWater.Invoke();
			this.tpInput.SetLockAllInput(true);
			this.tpInput.cc.disableCheckGround = true;
			this.tpInput.cc.disableSnapToGround = true;
			this.tpInput.cc.lockSetMoveSpeed = true;
			this.tpInput.cc.moveSpeed = this.swimForwardSpeed;
			this.tpInput.cc.freeSpeed.rotationSpeed = this.swimRotationSpeed;
			this.tpInput.cc.freeSpeed.animationSmooth = this.swimAnimationSmooth;
			this.tpInput.cc.freeSpeed.movementSmooth = this.swimMovementSmooth;
			this.ResetPlayerValues();
			this.tpInput.cc.animator.CrossFadeInFixedTime("Swimming", 0.25f);
			this.tpInput.cc._rigidbody.useGravity = false;
			this.tpInput.cc._rigidbody.drag = 10f;
			this.tpInput.cc._capsuleCollider.isTrigger = false;
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x00107474 File Offset: 0x00105674
		protected virtual void ExitSwimState()
		{
			this.ReleaseSwimmingActivity();
			if (!this.triggerSwimState)
			{
				return;
			}
			if (this.debugMode)
			{
				Debug.Log("Player Stop Swimming");
			}
			this.isUnderWater = false;
			this.triggerSwimState = false;
			this.OnExitWater.Invoke();
			this.tpInput.SetLockAllInput(false);
			this.tpInput.cc.disableCheckGround = false;
			this.tpInput.cc.lockSetMoveSpeed = false;
			this.tpInput.cc.disableSnapToGround = false;
			this.tpInput.cc.moveSpeed = this.originalMoveSpeed;
			this.tpInput.cc.freeSpeed.rotationSpeed = this.originalRotationSpeed;
			this.tpInput.cc.freeSpeed.animationSmooth = this.originalAnimationSmooth;
			this.tpInput.cc.freeSpeed.movementSmooth = this.originalMovementSmooth;
			this.tpInput.cc.animator.SetInteger(vAnimatorParameters.ActionState, 0);
			if (!this.IsClimbingOutOfWater)
			{
				this.tpInput.cc.ResetCapsule();
				this.tpInput.cc._rigidbody.useGravity = true;
			}
			this.tpInput.cc._rigidbody.drag = 0f;
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x001075C4 File Offset: 0x001057C4
		public virtual void ForceExitSwimming()
		{
			bool flag = !this.triggerSwimState && (this.inTheWater || this.isUnderWater || this.triggerUnderWater);
			this.water = null;
			this.inTheWater = false;
			this.isSwimming = false;
			this.ExitSwimState();
			this.isUnderWater = false;
			this.triggerUnderWater = false;
			this.triggerAboveWater = false;
			this.currentCharacterDepth = 0f;
			this.swimUpInterpolate = (this.swimDownInterpolate = 0f);
			this._climbSuspendedSwimming = false;
			if (flag)
			{
				UnityEvent onExitWater = this.OnExitWater;
				if (onExitWater == null)
				{
					return;
				}
				onExitWater.Invoke();
			}
		}

		// Token: 0x06002EA2 RID: 11938 RVA: 0x0010765D File Offset: 0x0010585D
		private void AcquireSwimmingActivity()
		{
			if (this._swimActivity != null && this._swimActivity.IsActive)
			{
				return;
			}
			PlayerActivityGate.TryAcquire("Swimming", this, PlayerActivity.None, PlayerContextResource.WaterMovement, PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.RapidHUD | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Inventory | PlayerActivity.Phone | PlayerActivity.Crouch | PlayerActivity.ArmsPose, out this._swimActivity, false, null);
		}

		// Token: 0x06002EA3 RID: 11939 RVA: 0x00107694 File Offset: 0x00105894
		private void ReleaseSwimmingActivity()
		{
			PlayerActivityLease swimActivity = this._swimActivity;
			if (swimActivity != null)
			{
				swimActivity.Dispose();
			}
			this._swimActivity = null;
		}

		// Token: 0x06002EA4 RID: 11940 RVA: 0x001076AE File Offset: 0x001058AE
		protected virtual void OnDisable()
		{
			this.ReleaseSwimmingActivity();
		}

		// Token: 0x06002EA5 RID: 11941 RVA: 0x001076B8 File Offset: 0x001058B8
		protected virtual void SwimUpOrDownInput()
		{
			if (this.tpInput.cc.customAction || this.IsClimbingOutOfWater || this.tpInput.cc._rigidbody.isKinematic)
			{
				return;
			}
			bool flag = this.currentCharacterDepth > 0.2f;
			Vector3 input = this.tpInput.cc.input;
			if ((this.swimUpInput.GetButton() || !this.swimUpAndDown) && flag)
			{
				if (this.debugMode)
				{
					Debug.Log("Player Swimming UP");
				}
				this.swimDownInterpolate = 0f;
				this.swimUpInterpolate += Time.deltaTime * this.swimUpDownSmooth;
				this.swimUpInterpolate = Mathf.Clamp(this.swimUpInterpolate, 0f, 1f);
				Vector3 velocity = this.tpInput.cc._rigidbody.velocity;
				velocity.y = Mathf.Lerp(velocity.y, this.swimUpSpeed, this.updownSmoothCurve.Evaluate(this.swimUpInterpolate));
				this.tpInput.cc._rigidbody.velocity = velocity;
				input.y = 1f;
				this.tpInput.cc.animator.SetInteger(vAnimatorParameters.ActionState, 4);
			}
			else if (this.swimDownInput.GetButton() && this.swimUpAndDown)
			{
				if (this.debugMode)
				{
					Debug.Log("Player Swimming Down");
				}
				this.swimUpInterpolate = 0f;
				this.swimDownInterpolate += Time.deltaTime * this.swimUpDownSmooth;
				this.swimDownInterpolate = Mathf.Clamp(this.swimDownInterpolate, 0f, 1f);
				Vector3 velocity2 = this.tpInput.cc._rigidbody.velocity;
				velocity2.y = Mathf.Lerp(velocity2.y, this.swimDownSpeed, this.updownSmoothCurve.Evaluate(this.swimDownInterpolate));
				this.tpInput.cc._rigidbody.velocity = velocity2;
				input.y = -1f;
				this.tpInput.cc.animator.SetInteger(vAnimatorParameters.ActionState, 3);
			}
			else
			{
				input.y = 0f;
				this.swimDownInterpolate = 0f;
				this.swimUpInterpolate = 0f;
				Vector3 velocity3 = this.tpInput.cc._rigidbody.velocity;
				velocity3.y = Mathf.Lerp(velocity3.y, 0f, this.swimUpDownSmooth * Time.deltaTime);
				this.tpInput.cc._rigidbody.velocity = velocity3;
				if (this.isUnderWater)
				{
					this.tpInput.cc.animator.SetInteger(vAnimatorParameters.ActionState, 2);
				}
				else
				{
					Vector3 position = base.transform.position;
					position.y = this.waterHeightLevel - (this.tpInput.cc.colliderHeightDefault * 0.5f + this.heightOffset);
					base.transform.position = Vector3.Lerp(base.transform.position, position, 0.5f * Time.deltaTime);
					this.tpInput.cc.animator.SetInteger(vAnimatorParameters.ActionState, 1);
				}
			}
			this.tpInput.cc.input = input;
		}

		// Token: 0x06002EA6 RID: 11942 RVA: 0x00107A1C File Offset: 0x00105C1C
		protected virtual void WaterRingEffect()
		{
			if (!this.waterRingEffect)
			{
				return;
			}
			if (this.tpInput.cc.input != Vector3.zero)
			{
				this.waterRingSpawnFrequency = this.waterRingFrequencySwim;
			}
			else
			{
				this.waterRingSpawnFrequency = this.waterRingFrequencyIdle;
			}
			this.timer += Time.deltaTime;
			if (this.timer >= this.waterRingSpawnFrequency)
			{
				Vector3 vector = new Vector3(base.transform.position.x, this.waterHeightLevel, base.transform.position.z);
				Object.Instantiate<GameObject>(this.waterRingEffect, vector, this.tpInput.transform.rotation).transform.SetParent(vObjectContainer.root, true);
				this.timer = 0f;
			}
		}

		// Token: 0x06002EA7 RID: 11943 RVA: 0x00107AF4 File Offset: 0x00105CF4
		protected virtual void ResetPlayerValues()
		{
			this.tpInput.cc.isJumping = false;
			this.tpInput.cc.isSprinting = false;
			this.tpInput.cc.isCrouching = false;
			this.tpInput.cc.animator.SetFloat(vAnimatorParameters.InputHorizontal, 0f);
			this.tpInput.cc.animator.SetFloat(vAnimatorParameters.InputVertical, 0f);
			this.tpInput.cc.animator.SetInteger(vAnimatorParameters.ActionState, 1);
			this.tpInput.cc.isGrounded = true;
			this.tpInput.cc.animator.SetBool(vAnimatorParameters.IsGrounded, true);
			this.tpInput.cc.verticalVelocity = 0f;
		}

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06002EA8 RID: 11944 RVA: 0x00107BCE File Offset: 0x00105DCE
		public virtual Vector3 characterCenter
		{
			get
			{
				return base.transform.position + Vector3.up * (this.tpInput.cc.colliderHeightDefault * 0.5f + this.heightOffset);
			}
		}

		// Token: 0x04002EE9 RID: 12009
		[vEditorToolbar("Settings", false, "", false, false)]
		[Tooltip("Name of the tag assign into the Water object")]
		public string waterTag = "Water";

		// Token: 0x04002EEA RID: 12010
		[Header("Speed & Extra Options")]
		[Tooltip("Uncheck if you don't want to go under water")]
		public bool swimUpAndDown = true;

		// Token: 0x04002EEB RID: 12011
		[Tooltip("Speed to swim forward")]
		public float swimForwardSpeed = 4f;

		// Token: 0x04002EEC RID: 12012
		[Tooltip("Speed to rotate the character")]
		public float swimRotationSpeed = 4f;

		// Token: 0x04002EED RID: 12013
		[Tooltip("Smooth value for the character movement")]
		public float swimMovementSmooth = 2f;

		// Token: 0x04002EEE RID: 12014
		[Tooltip("Smooth value for the character animation transition")]
		public float swimAnimationSmooth = 1f;

		// Token: 0x04002EEF RID: 12015
		[Tooltip("Smooth value for the character movement up and down")]
		public float swimUpDownSmooth = 2f;

		// Token: 0x04002EF0 RID: 12016
		[vHelpBox("! Assign a curve here, otherwise the character won't move up or down !", vHelpBoxAttribute.MessageType.None)]
		public AnimationCurve updownSmoothCurve;

		// Token: 0x04002EF1 RID: 12017
		[Tooltip("Speed to swim up")]
		public float swimUpSpeed = 3f;

		// Token: 0x04002EF2 RID: 12018
		[Tooltip("Speed to swim down")]
		public float swimDownSpeed = -3f;

		// Token: 0x04002EF3 RID: 12019
		[Tooltip("Increase the radius of the capsule collider to avoid enter walls")]
		public float colliderRadius = 0.5f;

		// Token: 0x04002EF4 RID: 12020
		[Tooltip("Increase the radius of the capsule collider to avoid enter walls")]
		public float colliderHeight = 0.5f;

		// Token: 0x04002EF5 RID: 12021
		[Tooltip("Height offset to match the character Y position")]
		public float heightOffset = 0.3f;

		// Token: 0x04002EF6 RID: 12022
		[Header("Health/Stamina Consumption")]
		[Tooltip("Leave with 0 if you don't want to use stamina consumption")]
		public float stamina = 15f;

		// Token: 0x04002EF7 RID: 12023
		[Tooltip("How much health will drain after all the stamina were consumed")]
		public int healthConsumption = 1;

		// Token: 0x04002EF8 RID: 12024
		[Header("Particle Effects")]
		public GameObject impactEffect;

		// Token: 0x04002EF9 RID: 12025
		[Tooltip("Check the Rigibody.Y of the character to trigger the ImpactEffect Particle")]
		public float velocityToImpact = -4f;

		// Token: 0x04002EFA RID: 12026
		public GameObject waterRingEffect;

		// Token: 0x04002EFB RID: 12027
		[Tooltip("Frequency to instantiate the WaterRing effect while standing still")]
		public float waterRingFrequencyIdle = 0.8f;

		// Token: 0x04002EFC RID: 12028
		[Tooltip("Frequency to instantiate the WaterRing effect while swimming")]
		public float waterRingFrequencySwim = 0.15f;

		// Token: 0x04002EFD RID: 12029
		[Tooltip("Instantiate a prefab when exit the water")]
		public GameObject waterDrops;

		// Token: 0x04002EFE RID: 12030
		[Tooltip("Y Offset based at the capsule collider")]
		public float waterDropsYOffset = 1.6f;

		// Token: 0x04002EFF RID: 12031
		[Header("Inputs")]
		[Tooltip("Input to make the character go up")]
		public GenericInput swimUpInput = new GenericInput("Space", "X", "X");

		// Token: 0x04002F00 RID: 12032
		[Tooltip("Input to make the character go down")]
		public GenericInput swimDownInput = new GenericInput("LeftShift", "Y", "Y");

		// Token: 0x04002F01 RID: 12033
		[vEditorToolbar("Events", false, "", false, false)]
		public UnityEvent OnEnterWater;

		// Token: 0x04002F02 RID: 12034
		public UnityEvent OnExitWater;

		// Token: 0x04002F03 RID: 12035
		public UnityEvent OnAboveWater;

		// Token: 0x04002F04 RID: 12036
		public UnityEvent OnUnderWater;

		// Token: 0x04002F05 RID: 12037
		[vEditorToolbar("Debug", false, "", false, false)]
		[Tooltip("Debug Mode will show the current behavior at the console window")]
		public bool debugMode;

		// Token: 0x04002F06 RID: 12038
		[vReadOnly(false)]
		public float currentCharacterDepth;

		// Token: 0x04002F07 RID: 12039
		[vReadOnly(false)]
		public GameObject water;

		// Token: 0x04002F08 RID: 12040
		[vReadOnly(false)]
		[SerializeField]
		protected bool _isSwimming;

		// Token: 0x04002F09 RID: 12041
		[vReadOnly(false)]
		public bool inTheWater;

		// Token: 0x04002F0A RID: 12042
		[vReadOnly(false)]
		[SerializeField]
		public bool isUnderWater;

		// Token: 0x04002F0B RID: 12043
		protected float swimUpInterpolate;

		// Token: 0x04002F0C RID: 12044
		protected float swimDownInterpolate;

		// Token: 0x04002F0D RID: 12045
		protected float waterHeightLevel;

		// Token: 0x04002F0E RID: 12046
		protected vThirdPersonInput tpInput;

		// Token: 0x04002F0F RID: 12047
		protected float timer;

		// Token: 0x04002F10 RID: 12048
		protected float originalMoveSpeed;

		// Token: 0x04002F11 RID: 12049
		protected float originalRotationSpeed;

		// Token: 0x04002F12 RID: 12050
		protected float originalMovementSmooth;

		// Token: 0x04002F13 RID: 12051
		protected float originalAnimationSmooth;

		// Token: 0x04002F14 RID: 12052
		protected float waterRingSpawnFrequency;

		// Token: 0x04002F15 RID: 12053
		protected bool triggerSwimState;

		// Token: 0x04002F16 RID: 12054
		protected bool triggerUnderWater;

		// Token: 0x04002F17 RID: 12055
		protected bool triggerAboveWater;

		// Token: 0x04002F18 RID: 12056
		private PlayerActivityLease _swimActivity;

		// Token: 0x04002F19 RID: 12057
		private vGenericAction _genericAction;

		// Token: 0x04002F1A RID: 12058
		private bool _climbSuspendedSwimming;

		// Token: 0x04002F1B RID: 12059
		private readonly List<Collider> _waterColliders = new List<Collider>(2);
	}
}
