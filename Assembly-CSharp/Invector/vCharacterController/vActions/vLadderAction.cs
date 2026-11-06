using System;
using System.Collections;
using System.Collections.Generic;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x0200071A RID: 1818
	[vClassHeader("Ladder Action", true, "icon_v2", false, "", iconName = "ladderIcon")]
	public class vLadderAction : vActionListener
	{
		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06002F22 RID: 12066 RVA: 0x00109E4B File Offset: 0x0010804B
		// (set) Token: 0x06002F23 RID: 12067 RVA: 0x00109E53 File Offset: 0x00108053
		protected virtual float speed
		{
			get
			{
				return this._speed;
			}
			set
			{
				this._speed = value;
			}
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06002F24 RID: 12068 RVA: 0x00109E5C File Offset: 0x0010805C
		// (set) Token: 0x06002F25 RID: 12069 RVA: 0x00109E64 File Offset: 0x00108064
		public virtual bool isUsingLadder
		{
			get
			{
				return this._isUsingLadder;
			}
			set
			{
				this._isUsingLadder = value;
			}
		}

		// Token: 0x06002F26 RID: 12070 RVA: 0x00109E6D File Offset: 0x0010806D
		protected override void SetUpListener()
		{
			base.actionEnter = false;
			base.actionStay = true;
			base.actionExit = true;
		}

		// Token: 0x06002F27 RID: 12071 RVA: 0x00109E84 File Offset: 0x00108084
		protected override void Start()
		{
			ControllerContextBindings.Set(this.exitInput, "B");
			ControllerContextBindings.Set(this.slideDownInput, "X");
			base.Start();
			this.tpInput = base.GetComponent<vThirdPersonInput>();
			if (this.tpInput)
			{
				this.tpInput.onUpdate -= this.UpdateLadderBehavior;
				this.tpInput.onUpdate += this.UpdateLadderBehavior;
				this.tpInput.onAnimatorMove -= this.UsingLadder;
				this.tpInput.onAnimatorMove += this.UsingLadder;
			}
		}

		// Token: 0x06002F28 RID: 12072 RVA: 0x00109F30 File Offset: 0x00108130
		protected virtual void UpdateLadderBehavior()
		{
			this.AutoEnterLadder();
			this.EnterLadderInput();
			this.ExitLadderInput();
		}

		// Token: 0x06002F29 RID: 12073 RVA: 0x00109F44 File Offset: 0x00108144
		protected virtual void EnterLadderInput()
		{
			if (this.targetLadderAction == null || this.tpInput.cc.customAction || this.tpInput.cc.isJumping || !this.tpInput.cc.isGrounded || this.tpInput.cc.isRolling)
			{
				return;
			}
			if (this.enterInput.GetButtonDown() && !this.enterLadderStarted && !this.isUsingLadder && !this.targetLadderAction.autoAction)
			{
				this.TriggerEnterLadder();
			}
		}

		// Token: 0x06002F2A RID: 12074 RVA: 0x00109FD8 File Offset: 0x001081D8
		protected virtual void ExitLadderInput()
		{
			if (!this.isUsingLadder)
			{
				return;
			}
			if (this.tpInput.cc.baseLayerInfo.IsName("EnterLadderTop") || this.tpInput.cc.baseLayerInfo.IsName("EnterLadderBottom"))
			{
				return;
			}
			if (this.targetLadderAction == null)
			{
				if (this.tpInput.cc.IsAnimatorTag("ClimbLadder"))
				{
					if (this.slideDownInput.GetButtonDown() && !this.inExitingLadderAnimation)
					{
						this.tpInput.cc.animator.CrossFadeInFixedTime("Ladder_SlideDown", 0.2f);
					}
					if (this.exitInput.GetButtonDown())
					{
						if (this.debugMode)
						{
							Debug.Log("Quick Exit..." + this.currentLadderAction.name + "_" + this.currentLadderAction.transform.parent.gameObject.name);
						}
						this.tpInput.cc.animator.speed = 1f;
						this.tpInput.cc.animator.CrossFadeInFixedTime("QuickExitLadder", 0.1f);
						base.Invoke("ResetPlayerSettings", 0.5f);
						return;
					}
				}
			}
			else
			{
				this.currentLadderAction = this.targetLadderAction;
				string exitAnimation = this.targetLadderAction.exitAnimation;
				if (exitAnimation == "ExitLadderBottom")
				{
					if ((this.exitInput.GetButtonDown() && !this.triggerExitOnce) || (this.speed <= -0.05f && !this.triggerExitOnce) || (this.tpInput.cc.IsAnimatorTag("LadderSlideDown") && this.targetLadderAction != null && !this.triggerExitOnce))
					{
						if (this.debugMode)
						{
							Debug.Log("Exit Bottom..." + this.currentLadderAction.name + "_" + this.currentLadderAction.transform.parent.gameObject.name);
						}
						this.triggerExitOnce = true;
						this.tpInput.cc.animator.CrossFadeInFixedTime(this.targetLadderAction.exitAnimation, 0.1f);
						return;
					}
				}
				else if (exitAnimation == "ExitLadderTop" && this.tpInput.cc.IsAnimatorTag("ClimbLadder") && this.speed >= 0.05f && !this.triggerExitOnce && !this.tpInput.cc.animator.IsInTransition(0))
				{
					if (this.debugMode)
					{
						Debug.Log("Exit Top..." + this.currentLadderAction.name + "_" + this.currentLadderAction.transform.parent.gameObject.name);
					}
					this.triggerExitOnce = true;
					this.tpInput.cc.animator.CrossFadeInFixedTime(this.targetLadderAction.exitAnimation, 0.1f);
				}
			}
		}

		// Token: 0x06002F2B RID: 12075 RVA: 0x0010A2E4 File Offset: 0x001084E4
		protected virtual void AutoEnterLadder()
		{
			if (this.targetLadderAction == null || !this.targetLadderAction.autoAction)
			{
				return;
			}
			if (this.tpInput.cc.customAction || this.isUsingLadder || this.tpInput.cc.animator.IsInTransition(0) || this.tpInput.cc.isRolling)
			{
				return;
			}
			if (this.targetLadderAction.autoAction && this.tpInput.cc.input != Vector3.zero && !this.tpInput.cc.customAction)
			{
				Vector3 vector = Camera.main.transform.TransformDirection(new Vector3(this.tpInput.cc.input.x, 0f, this.tpInput.cc.input.z));
				vector.y = 0f;
				if (Vector3.Distance(vector.normalized, this.targetLadderAction.transform.forward) < 0.8f)
				{
					this.TriggerEnterLadder();
				}
			}
		}

		// Token: 0x06002F2C RID: 12076 RVA: 0x0010A40C File Offset: 0x0010860C
		public virtual void TriggerMidAirEnterLadder(vTriggerLadderMiddle ladderMiddle)
		{
			if (this.tpInput.cc.isGrounded || this.isAligningMidAir)
			{
				return;
			}
			this.isAligningMidAir = true;
			Vector3 vector = ladderMiddle.refTarget.position;
			bool flag = false;
			float num = Vector3.Distance(vector, base.transform.position);
			int num2 = (int)(ladderMiddle._collider.size.y / ladderMiddle.stepHeight);
			for (int i = 0; i < num2; i++)
			{
				vector = ladderMiddle.refTarget.position + ladderMiddle.transform.up * ladderMiddle.stepHeight * (float)i;
				float num3 = Vector3.Distance(vector, base.transform.position);
				if (num3 <= num)
				{
					num = num3;
					flag = true;
				}
				else if (flag)
				{
					break;
				}
			}
			this.tpInput.cc.DisableGravityAndCollision();
			this.tpInput.cc.isCrouching = false;
			this.tpInput.cc.ControlCapsuleHeight();
			this.tpInput.UpdateCameraStates();
			this.tpInput.cc.UpdateAnimator();
			this.triggerEnterOnce = true;
			this.enterLadderStarted = true;
			this.tpInput.cc.animator.SetInteger(vAnimatorParameters.ActionState, 1);
			this.tpInput.cc.StopCharacter();
			this.tpInput.SetLockAllInput(true);
			this.tpInput.cc.ResetInputAnimatorParameters();
			if (ladderMiddle.transform.InverseTransformPoint(base.transform.position).x >= 0f)
			{
				this.tpInput.cc.animator.CrossFadeInFixedTime("IdleLadder_Left", 0.25f);
			}
			else
			{
				this.tpInput.cc.animator.CrossFadeInFixedTime("IdleLadder_Right", 0.25f);
			}
			this.tpInput.cc.disableAnimations = true;
			base.StartCoroutine(this.AlignToMiddleLadder(vector, ladderMiddle.refTarget.rotation));
		}

		// Token: 0x06002F2D RID: 12077 RVA: 0x0010A602 File Offset: 0x00108802
		private IEnumerator AlignToMiddleLadder(Vector3 pos, Quaternion rot)
		{
			float time = 0f;
			Vector3 position = base.transform.position;
			Quaternion rotation = base.transform.rotation;
			while (time <= 1f)
			{
				base.transform.position = Vector3.Lerp(position, pos, time);
				base.transform.rotation = Quaternion.Lerp(rotation, rot, time);
				time += Time.fixedDeltaTime * 8f;
				yield return null;
			}
			do
			{
				base.transform.position = pos;
				base.transform.rotation = rot;
				yield return null;
			}
			while (this.tpInput.cc.animator.IsInTransition(0));
			yield return new WaitForEndOfFrame();
			this.isUsingLadder = true;
			this.isAligningMidAir = false;
			yield break;
		}

		// Token: 0x06002F2E RID: 12078 RVA: 0x0010A620 File Offset: 0x00108820
		protected virtual void TriggerEnterLadder()
		{
			if (this.debugMode)
			{
				Debug.Log("Enter Ladder");
			}
			this.OnExitTriggerLadder.Invoke();
			if (this.targetLadderAction.targetCharacterParent)
			{
				base.transform.parent = this.targetLadderAction.targetCharacterParent;
			}
			this.tpInput.cc.isCrouching = false;
			this.tpInput.cc.ControlCapsuleHeight();
			this.tpInput.UpdateCameraStates();
			this.tpInput.cc.UpdateAnimator();
			this.OnEnterLadder.Invoke();
			this.triggerEnterOnce = true;
			this.enterLadderStarted = true;
			this.tpInput.cc.animator.SetInteger(vAnimatorParameters.ActionState, 1);
			this.tpInput.SetLockAllInput(true);
			this.tpInput.cc.ResetInputAnimatorParameters();
			this.targetLadderAction.OnDoAction.Invoke();
			this.currentLadderAction = this.targetLadderAction;
			if (!string.IsNullOrEmpty(this.currentLadderAction.playAnimation))
			{
				if (this.debugMode)
				{
					Debug.Log("TriggerAnimation " + this.currentLadderAction.name + "_" + this.currentLadderAction.transform.parent.gameObject.name);
				}
				this.tpInput.cc.animator.CrossFadeInFixedTime(this.currentLadderAction.playAnimation, 0.25f);
				this.isUsingLadder = true;
				this.tpInput.cc.disableAnimations = true;
				this.tpInput.cc.StopCharacter();
			}
		}

		// Token: 0x06002F2F RID: 12079 RVA: 0x0010A7C0 File Offset: 0x001089C0
		protected virtual void UsingLadder()
		{
			if (!this.isUsingLadder)
			{
				return;
			}
			this.tpInput.cc.AnimatorLayerControl();
			this.tpInput.cc.ActionsControl();
			this.tpInput.CameraInput();
			this.speed = this.verticalInput.GetAxis();
			this.tpInput.cc.animator.SetFloat(vAnimatorParameters.InputVertical, this.speed, 0.1f, Time.deltaTime);
			if (this.speed >= 0.05f || this.speed <= -0.05f)
			{
				this.tpInput.cc.animator.speed = Mathf.Lerp(this.tpInput.cc.animator.speed, this.currentClimbSpeed, 2f * Time.deltaTime);
			}
			else
			{
				this.tpInput.cc.animator.speed = Mathf.Lerp(this.tpInput.cc.animator.speed, 1f, 2f * Time.deltaTime);
			}
			if (this.fastClimbInput.GetButton() && this.tpInput.cc.currentStamina > 0f)
			{
				this.currentClimbSpeed = this.fastClimbSpeed;
				this.StaminaConsumption();
			}
			else
			{
				this.currentClimbSpeed = this.climbSpeed;
			}
			bool flag = this.tpInput.cc.baseLayerInfo.IsName("EnterLadderTop") || (this.tpInput.cc.baseLayerInfo.IsName("EnterLadderBottom") && !this.tpInput.cc.animator.IsInTransition(0));
			if (flag)
			{
				this.inEnterLadderAnimation = true;
				this.tpInput.cc.DisableGravityAndCollision();
				if (this.currentLadderAction != null)
				{
					this.currentLadderAction.OnPlayerExit.Invoke();
				}
				if (this.currentLadderAction.useTriggerRotation)
				{
					if (this.debugMode)
					{
						Debug.Log("Rotating to target..." + this.currentLadderAction.name + "_" + this.currentLadderAction.transform.parent.gameObject.name);
					}
					this.EvaluateToRotation(this.currentLadderAction.enterRotationCurve, this.currentLadderAction.matchTarget.transform.rotation, this.tpInput.cc.baseLayerInfo.normalizedTime);
				}
				if (this.currentLadderAction.matchTarget != null)
				{
					if (base.transform.parent != this.currentLadderAction.targetCharacterParent)
					{
						base.transform.parent = this.currentLadderAction.targetCharacterParent;
					}
					if (this.debugMode)
					{
						Debug.Log("Match Target to Enter..." + this.currentLadderAction.name + "_" + this.currentLadderAction.transform.parent.gameObject.name);
					}
					this.EvaluateToPosition(this.currentLadderAction.enterPositionXZCurve, this.currentLadderAction.enterPositionYCurve, this.currentLadderAction.matchTarget.position, this.tpInput.cc.baseLayerInfo.normalizedTime);
				}
			}
			if (!flag && this.inEnterLadderAnimation)
			{
				this.enterLadderStarted = false;
				this.inEnterLadderAnimation = false;
			}
			this.TriggerExitLadder();
		}

		// Token: 0x06002F30 RID: 12080 RVA: 0x0010AB20 File Offset: 0x00108D20
		protected virtual void TriggerExitLadder()
		{
			this.inExitingLadderAnimation = this.tpInput.cc.baseLayerInfo.IsName("ExitLadderTop") || this.tpInput.cc.baseLayerInfo.IsName("ExitLadderBottom") || this.tpInput.cc.baseLayerInfo.IsName("QuickExitLadder");
			if (this.inExitingLadderAnimation)
			{
				this.tpInput.cc.animator.speed = 1f;
				if (this.currentLadderAction.exitMatchTarget != null && !this.tpInput.cc.baseLayerInfo.IsName("QuickExitLadder"))
				{
					if (this.debugMode)
					{
						Debug.Log("Match Target to exit..." + this.currentLadderAction.name + "_" + this.currentLadderAction.transform.parent.gameObject.name);
					}
					this.EvaluateToPosition(this.currentLadderAction.exitPositionXZCurve, this.currentLadderAction.exitPositionYCurve, this.currentLadderAction.exitMatchTarget.position, this.tpInput.cc.baseLayerInfo.normalizedTime);
				}
				Vector3 vector = new Vector3(0f, this.tpInput.animator.rootRotation.eulerAngles.y, 0f);
				this.EvaluateToRotation(this.currentLadderAction.exitRotationCurve, Quaternion.Euler(vector), this.tpInput.cc.baseLayerInfo.normalizedTime);
				if (this.tpInput.cc.baseLayerInfo.normalizedTime >= 0.8f)
				{
					this.ResetPlayerSettings();
				}
			}
		}

		// Token: 0x06002F31 RID: 12081 RVA: 0x0010ACE0 File Offset: 0x00108EE0
		protected virtual void EvaluateToPosition(AnimationCurve XZ, AnimationCurve Y, Vector3 targetPosition, float normalizedTime)
		{
			Vector3 rootPosition = this.tpInput.cc.animator.rootPosition;
			float num = XZ.Evaluate(normalizedTime);
			float num2 = Y.Evaluate(normalizedTime);
			if (num < 1f)
			{
				rootPosition.x = Mathf.Lerp(rootPosition.x, targetPosition.x, num);
				rootPosition.z = Mathf.Lerp(rootPosition.z, targetPosition.z, num);
			}
			if (num2 < 1f)
			{
				rootPosition.y = Mathf.Lerp(rootPosition.y, targetPosition.y, num2);
			}
			base.transform.position = rootPosition;
		}

		// Token: 0x06002F32 RID: 12082 RVA: 0x0010AD7C File Offset: 0x00108F7C
		protected virtual void EvaluateToRotation(AnimationCurve curve, Quaternion targetRotation, float normalizedTime)
		{
			Quaternion quaternion = this.tpInput.cc.animator.rootRotation;
			float num = curve.Evaluate(normalizedTime);
			if (num < 1f)
			{
				quaternion = Quaternion.Lerp(quaternion, targetRotation, num);
			}
			base.transform.rotation = quaternion;
		}

		// Token: 0x06002F33 RID: 12083 RVA: 0x0010ADC4 File Offset: 0x00108FC4
		protected virtual void StaminaConsumption()
		{
			if (this.tpInput.cc.currentStamina <= 0f)
			{
				return;
			}
			this.tpInput.cc.ReduceStamina(this.fastClimbStamina, true);
			this.tpInput.cc.currentStaminaRecoveryDelay = 0.25f;
		}

		// Token: 0x06002F34 RID: 12084 RVA: 0x0010AE18 File Offset: 0x00109018
		protected virtual void AddLadderTrigger(vTriggerLadderAction _ladderAction)
		{
			if (this.targetLadderAction != _ladderAction)
			{
				this.targetLadderAction = _ladderAction;
				if (this.debugMode)
				{
					Debug.Log("TriggerStay " + this.targetLadderAction.name + "_" + this.targetLadderAction.transform.parent.gameObject.name);
				}
			}
			if (!this.actionTriggers.Contains(this.targetLadderAction))
			{
				this.actionTriggers.Add(this.targetLadderAction);
				this.targetLadderAction.OnPlayerEnter.Invoke();
			}
		}

		// Token: 0x06002F35 RID: 12085 RVA: 0x0010AEAF File Offset: 0x001090AF
		protected virtual void RemoveLadderTrigger(vTriggerLadderAction _ladderAction)
		{
			if (_ladderAction == this.targetLadderAction)
			{
				this.targetLadderAction = null;
			}
			if (this.actionTriggers.Contains(_ladderAction))
			{
				this.actionTriggers.Remove(_ladderAction);
				_ladderAction.OnPlayerExit.Invoke();
			}
		}

		// Token: 0x06002F36 RID: 12086 RVA: 0x0010AEEC File Offset: 0x001090EC
		protected virtual void CheckForTriggerAction(Collider other)
		{
			vTriggerLadderAction component = other.GetComponent<vTriggerLadderAction>();
			if (!component)
			{
				this.AddLadderTrigger(component);
				return;
			}
			float num = Vector3.Distance(base.transform.forward, component.transform.forward);
			if (this.isUsingLadder && component != null)
			{
				if (this.targetLadderAction != component)
				{
					this.targetLadderAction = component;
					if (!this.actionTriggers.Contains(this.targetLadderAction))
					{
						this.actionTriggers.Add(this.targetLadderAction);
						return;
					}
				}
			}
			else
			{
				if ((!component.activeFromForward || num <= 0.8f) && !this.isUsingLadder)
				{
					this.AddLadderTrigger(component);
					this.OnEnterTriggerLadder.Invoke();
					return;
				}
				this.RemoveLadderTrigger(component);
			}
		}

		// Token: 0x06002F37 RID: 12087 RVA: 0x0010AFAC File Offset: 0x001091AC
		public virtual void ResetPlayerSettings()
		{
			if (this.debugMode)
			{
				Debug.Log("Reset Player Settings");
			}
			this.speed = 0f;
			this.targetLadderAction = null;
			this.isUsingLadder = false;
			this.OnExitLadder.Invoke();
			this.triggerExitOnce = false;
			this.triggerEnterOnce = false;
			this.inEnterLadderAnimation = false;
			this.enterLadderStarted = false;
			this.tpInput.cc.animator.SetInteger(vAnimatorParameters.ActionState, 0);
			this.tpInput.cc.EnableGravityAndCollision();
			this.tpInput.SetLockAllInput(false);
			this.tpInput.cc.StopCharacter();
			this.tpInput.cc.disableAnimations = false;
			if (base.transform.parent != null)
			{
				base.transform.parent = null;
			}
		}

		// Token: 0x06002F38 RID: 12088 RVA: 0x0010B082 File Offset: 0x00109282
		public override void OnActionStay(Collider other)
		{
			if (other.gameObject.CompareTag(this.actionTag))
			{
				this.CheckForTriggerAction(other);
			}
		}

		// Token: 0x06002F39 RID: 12089 RVA: 0x0010B0A0 File Offset: 0x001092A0
		public override void OnActionExit(Collider other)
		{
			if (other.gameObject.CompareTag(this.actionTag))
			{
				vTriggerLadderAction component = other.GetComponent<vTriggerLadderAction>();
				if (!component)
				{
					return;
				}
				this.RemoveLadderTrigger(component);
				if (this.debugMode)
				{
					Debug.Log("TriggerExit " + other.name + "_" + other.transform.parent.gameObject.name);
				}
				this.OnExitTriggerLadder.Invoke();
			}
		}

		// Token: 0x04002F51 RID: 12113
		[vEditorToolbar("Settings", false, "", false, true, order = 0)]
		[Tooltip("Tag of the object you want to access")]
		public string actionTag = "LadderTrigger";

		// Token: 0x04002F52 RID: 12114
		[Tooltip("Speed multiplier for the climb ladder animations")]
		public float climbSpeed = 1.5f;

		// Token: 0x04002F53 RID: 12115
		[Tooltip("Speed multiplier for the climb ladder animations when the fastClimbInput is pressed")]
		public float fastClimbSpeed = 3f;

		// Token: 0x04002F54 RID: 12116
		[Tooltip("How much Stamina will be consumed when climbing faster")]
		public float fastClimbStamina = 30f;

		// Token: 0x04002F55 RID: 12117
		[Tooltip("Input to use the ladder going up or down")]
		public GenericInput verticalInput = new GenericInput("Vertical", "LeftAnalogVertical", "Vertical");

		// Token: 0x04002F56 RID: 12118
		[Tooltip("Input to enter the ladder")]
		public GenericInput enterInput = new GenericInput("E", "A", "A");

		// Token: 0x04002F57 RID: 12119
		[Tooltip("Input to exit the ladder")]
		public GenericInput exitInput = new GenericInput("Space", "B", "B");

		// Token: 0x04002F58 RID: 12120
		[Tooltip("Input to climb faster")]
		public GenericInput fastClimbInput = new GenericInput("LeftShift", "LeftStickClick", "LeftStickClick");

		// Token: 0x04002F59 RID: 12121
		[Tooltip("Input to climb faster")]
		public GenericInput slideDownInput = new GenericInput("Q", "X", "X");

		// Token: 0x04002F5A RID: 12122
		[vEditorToolbar("Events", false, "", false, false)]
		public UnityEvent OnEnterLadder;

		// Token: 0x04002F5B RID: 12123
		public UnityEvent OnExitLadder;

		// Token: 0x04002F5C RID: 12124
		public UnityEvent OnEnterTriggerLadder;

		// Token: 0x04002F5D RID: 12125
		public UnityEvent OnExitTriggerLadder;

		// Token: 0x04002F5E RID: 12126
		[vEditorToolbar("Debug", false, "", false, false)]
		public bool debugMode;

		// Token: 0x04002F5F RID: 12127
		[vReadOnly(false)]
		[SerializeField]
		protected vTriggerLadderAction targetLadderAction;

		// Token: 0x04002F60 RID: 12128
		[vReadOnly(false)]
		[SerializeField]
		protected vTriggerLadderAction currentLadderAction;

		// Token: 0x04002F61 RID: 12129
		protected List<vTriggerLadderAction> actionTriggers = new List<vTriggerLadderAction>();

		// Token: 0x04002F62 RID: 12130
		[vReadOnly(false)]
		[SerializeField]
		protected float _speed;

		// Token: 0x04002F63 RID: 12131
		[vReadOnly(false)]
		[SerializeField]
		protected float currentClimbSpeed;

		// Token: 0x04002F64 RID: 12132
		[vReadOnly(false)]
		[SerializeField]
		protected bool _isUsingLadder;

		// Token: 0x04002F65 RID: 12133
		[vReadOnly(false)]
		[SerializeField]
		protected bool enterLadderStarted;

		// Token: 0x04002F66 RID: 12134
		[vReadOnly(false)]
		[SerializeField]
		protected bool inEnterLadderAnimation;

		// Token: 0x04002F67 RID: 12135
		[vReadOnly(false)]
		[SerializeField]
		protected bool inExitingLadderAnimation;

		// Token: 0x04002F68 RID: 12136
		[vReadOnly(false)]
		[SerializeField]
		protected bool triggerEnterOnce;

		// Token: 0x04002F69 RID: 12137
		[vReadOnly(false)]
		[SerializeField]
		protected bool triggerExitOnce;

		// Token: 0x04002F6A RID: 12138
		protected vThirdPersonInput tpInput;

		// Token: 0x04002F6B RID: 12139
		protected bool isAligningMidAir;
	}
}
