using System;
using System.Collections;
using Invector.vCamera;
using UnityEngine;
using UnityEngine.Events;

namespace Invector.vCharacterController
{
	// Token: 0x020006C3 RID: 1731
	[vClassHeader("Input Manager", true, "icon_v2", false, "", iconName = "inputIcon")]
	public class vThirdPersonInput_Sandbox : vMonoBehaviour, vIAnimatorMoveReceiver
	{
		// Token: 0x14000049 RID: 73
		// (add) Token: 0x06002B77 RID: 11127 RVA: 0x000F2DF0 File Offset: 0x000F0FF0
		// (remove) Token: 0x06002B78 RID: 11128 RVA: 0x000F2E28 File Offset: 0x000F1028
		public event vThirdPersonInput_Sandbox.OnUpdateEvent onUpdate;

		// Token: 0x1400004A RID: 74
		// (add) Token: 0x06002B79 RID: 11129 RVA: 0x000F2E60 File Offset: 0x000F1060
		// (remove) Token: 0x06002B7A RID: 11130 RVA: 0x000F2E98 File Offset: 0x000F1098
		public event vThirdPersonInput_Sandbox.OnUpdateEvent onLateUpdate;

		// Token: 0x1400004B RID: 75
		// (add) Token: 0x06002B7B RID: 11131 RVA: 0x000F2ED0 File Offset: 0x000F10D0
		// (remove) Token: 0x06002B7C RID: 11132 RVA: 0x000F2F08 File Offset: 0x000F1108
		public event vThirdPersonInput_Sandbox.OnUpdateEvent onFixedUpdate;

		// Token: 0x1400004C RID: 76
		// (add) Token: 0x06002B7D RID: 11133 RVA: 0x000F2F40 File Offset: 0x000F1140
		// (remove) Token: 0x06002B7E RID: 11134 RVA: 0x000F2F78 File Offset: 0x000F1178
		public event vThirdPersonInput_Sandbox.OnUpdateEvent onAnimatorMove;

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x06002B7F RID: 11135 RVA: 0x000F2FAD File Offset: 0x000F11AD
		// (set) Token: 0x06002B80 RID: 11136 RVA: 0x000F2FB5 File Offset: 0x000F11B5
		[HideInInspector]
		public virtual bool lockInput
		{
			get
			{
				return this._lockInput;
			}
			set
			{
				this._lockInput = value;
			}
		}

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06002B81 RID: 11137 RVA: 0x000E4315 File Offset: 0x000E2515
		protected InputDevice inputDevice
		{
			get
			{
				return vInput.instance.inputDevice;
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06002B82 RID: 11138 RVA: 0x000F2FBE File Offset: 0x000F11BE
		// (set) Token: 0x06002B83 RID: 11139 RVA: 0x000F2FC6 File Offset: 0x000F11C6
		public virtual bool lockUpdateMoveDirection { get; set; }

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06002B84 RID: 11140 RVA: 0x000F2FD0 File Offset: 0x000F11D0
		// (set) Token: 0x06002B85 RID: 11141 RVA: 0x000F3038 File Offset: 0x000F1238
		public virtual Camera cameraMain
		{
			get
			{
				if (!this._cameraMain && !this.withoutMainCamera)
				{
					if (!Camera.main)
					{
						Debug.Log("Missing a Camera with the tag MainCamera, please add one.");
						this.withoutMainCamera = true;
					}
					else
					{
						this._cameraMain = Camera.main;
						this.cc.rotateTarget = this._cameraMain.transform;
					}
				}
				return this._cameraMain;
			}
			set
			{
				this._cameraMain = value;
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06002B86 RID: 11142 RVA: 0x000F3044 File Offset: 0x000F1244
		public Animator animator
		{
			get
			{
				if (this.cc == null)
				{
					this.cc = base.GetComponent<vThirdPersonController>();
				}
				if (this.cc.animator == null)
				{
					return base.GetComponent<Animator>();
				}
				return this.cc.animator;
			}
		}

		// Token: 0x06002B87 RID: 11143 RVA: 0x000F3090 File Offset: 0x000F1290
		protected virtual void Start()
		{
			this.cc = base.GetComponent<vThirdPersonController>();
			if (this.cc != null)
			{
				this.cc.Init();
			}
			this.cc.onDead.AddListener(delegate(GameObject _gameObject)
			{
				this.cc.ResetInputAnimatorParameters();
				this.SetLockAllInput(true);
				this.cc.StopCharacter();
			});
			base.StartCoroutine(this.CharacterInit());
			this.ShowCursor(this.showCursorOnStart);
			this.LockCursor(this.unlockCursorOnStart);
			this.EnableOnAnimatorMove();
		}

		// Token: 0x06002B88 RID: 11144 RVA: 0x000F3109 File Offset: 0x000F1309
		protected virtual IEnumerator CharacterInit()
		{
			this.FindCamera();
			yield return new WaitForEndOfFrame();
			this.FindHUD();
			yield break;
		}

		// Token: 0x06002B89 RID: 11145 RVA: 0x000F3118 File Offset: 0x000F1318
		public virtual void FindHUD()
		{
			if (this.hud == null && vHUDController.instance != null)
			{
				this.hud = vHUDController.instance;
				this.hud.Init(this.cc);
			}
		}

		// Token: 0x06002B8A RID: 11146 RVA: 0x000F3154 File Offset: 0x000F1354
		public virtual void FindCamera()
		{
			vThirdPersonCamera[] array = Object.FindObjectsOfType<vThirdPersonCamera>();
			if (array.Length > 1)
			{
				this.tpCamera = Array.Find<vThirdPersonCamera>(array, (vThirdPersonCamera tp) => !tp.isInit);
				if (this.tpCamera == null)
				{
					this.tpCamera = array[0];
				}
				if (this.tpCamera != null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (this.tpCamera != array[i])
						{
							Object.Destroy(array[i].gameObject);
						}
					}
				}
			}
			else if (array.Length == 1)
			{
				this.tpCamera = array[0];
			}
			if (this.tpCamera && this.tpCamera.mainTarget != base.transform)
			{
				this.tpCamera.SetMainTarget(base.transform);
			}
		}

		// Token: 0x06002B8B RID: 11147 RVA: 0x000F322F File Offset: 0x000F142F
		protected virtual void LateUpdate()
		{
			if (this.cc == null)
			{
				return;
			}
			if (!this.updateIK)
			{
				return;
			}
			if (this.onLateUpdate != null)
			{
				this.onLateUpdate();
			}
			this.CameraInput();
			this.UpdateCameraStates();
			this.updateIK = false;
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x000F3270 File Offset: 0x000F1470
		protected virtual void FixedUpdate()
		{
			if (this.onFixedUpdate != null)
			{
				this.onFixedUpdate();
			}
			this.cc.UpdateMotor();
			this.cc.ControlLocomotionType();
			this.ControlRotation();
			this.cc.UpdateAnimator();
			this.updateIK = true;
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06002B8D RID: 11149 RVA: 0x000F32C0 File Offset: 0x000F14C0
		public virtual bool rotateToLockTargetConditions
		{
			get
			{
				return this.tpCamera && this.tpCamera.lockTarget && this.cc.isStrafing && !this.cc.isRolling && !this.cc.isJumping && !this.cc.customAction;
			}
		}

		// Token: 0x06002B8E RID: 11150 RVA: 0x000F3324 File Offset: 0x000F1524
		public virtual void ControlRotation()
		{
			if (this.cameraMain && !this.lockUpdateMoveDirection && !this.cc.keepDirection)
			{
				this.cc.UpdateMoveDirection(this.cameraMain.transform);
			}
			if (this.rotateToLockTargetConditions)
			{
				this.cc.RotateToPosition(this.tpCamera.lockTarget.position);
				return;
			}
			this.cc.ControlRotationType();
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x000F3398 File Offset: 0x000F1598
		protected virtual void Update()
		{
			if (this.cc == null || Time.timeScale == 0f)
			{
				return;
			}
			if (this.onUpdate != null)
			{
				this.onUpdate();
			}
			this.InputHandle();
			this.UpdateHUD();
		}

		// Token: 0x06002B90 RID: 11152 RVA: 0x000F33D4 File Offset: 0x000F15D4
		public virtual void OnAnimatorMoveEvent()
		{
			if (this.cc == null)
			{
				return;
			}
			this.cc.ControlAnimatorRootMotion();
			if (this.onAnimatorMove != null)
			{
				this.onAnimatorMove();
			}
		}

		// Token: 0x06002B91 RID: 11153 RVA: 0x000F3403 File Offset: 0x000F1603
		public virtual void SetLockBasicInput(bool value)
		{
			this.lockInput = value;
		}

		// Token: 0x06002B92 RID: 11154 RVA: 0x000F340C File Offset: 0x000F160C
		public virtual void SetLockAllInput(bool value)
		{
			this.SetLockBasicInput(value);
		}

		// Token: 0x06002B93 RID: 11155 RVA: 0x000ADA34 File Offset: 0x000ABC34
		public virtual void ShowCursor(bool value)
		{
			Cursor.visible = value;
		}

		// Token: 0x06002B94 RID: 11156 RVA: 0x000E4844 File Offset: 0x000E2A44
		public virtual void LockCursor(bool value)
		{
			if (!value)
			{
				Cursor.lockState = CursorLockMode.Locked;
				return;
			}
			Cursor.lockState = CursorLockMode.None;
		}

		// Token: 0x06002B95 RID: 11157 RVA: 0x000F3415 File Offset: 0x000F1615
		public virtual void SetLockCameraInput(bool value)
		{
			this.lockCameraInput = value;
			if (this.lockCameraInput)
			{
				this.OnLockCamera.Invoke();
				return;
			}
			this.OnUnlockCamera.Invoke();
		}

		// Token: 0x06002B96 RID: 11158 RVA: 0x000F343D File Offset: 0x000F163D
		public virtual void SetLockUpdateMoveDirection(bool value)
		{
			this.lockUpdateMoveDirection = value;
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x000F3446 File Offset: 0x000F1646
		public virtual void SetWalkByDefault(bool value)
		{
			this.cc.freeSpeed.walkByDefault = value;
			this.cc.strafeSpeed.walkByDefault = value;
		}

		// Token: 0x06002B98 RID: 11160 RVA: 0x000F346C File Offset: 0x000F166C
		public virtual void ResetWalkByDefault()
		{
			this.cc.freeSpeed.walkByDefault = this.cc.freeSpeed.defaultWalkByDefault;
			this.cc.strafeSpeed.walkByDefault = this.cc.strafeSpeed.defaultWalkByDefault;
		}

		// Token: 0x06002B99 RID: 11161 RVA: 0x000F34B9 File Offset: 0x000F16B9
		public virtual void SetStrafeLocomotion(bool value)
		{
			this.cc.lockInStrafe = value;
			this.cc.isStrafing = value;
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x06002B9A RID: 11162 RVA: 0x000F34D3 File Offset: 0x000F16D3
		// (set) Token: 0x06002B9B RID: 11163 RVA: 0x000F34DB File Offset: 0x000F16DB
		public virtual vAnimatorMoveSender animatorMoveSender { get; set; }

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06002B9C RID: 11164 RVA: 0x000F34E4 File Offset: 0x000F16E4
		// (set) Token: 0x06002B9D RID: 11165 RVA: 0x000F34EC File Offset: 0x000F16EC
		protected bool _useAnimatorMove { get; set; }

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06002B9E RID: 11166 RVA: 0x000F34F5 File Offset: 0x000F16F5
		// (set) Token: 0x06002B9F RID: 11167 RVA: 0x000F3500 File Offset: 0x000F1700
		public virtual bool UseAnimatorMove
		{
			get
			{
				return this._useAnimatorMove;
			}
			set
			{
				if (this._useAnimatorMove != value)
				{
					if (value)
					{
						this.animatorMoveSender = base.gameObject.AddComponent<vAnimatorMoveSender>();
						UnityEvent unityEvent = this.onEnableAnimatorMove;
						if (unityEvent != null)
						{
							unityEvent.Invoke();
						}
					}
					else
					{
						if (this.animatorMoveSender)
						{
							Object.Destroy(this.animatorMoveSender);
						}
						UnityEvent unityEvent2 = this.onEnableAnimatorMove;
						if (unityEvent2 != null)
						{
							unityEvent2.Invoke();
						}
					}
				}
				this._useAnimatorMove = value;
			}
		}

		// Token: 0x06002BA0 RID: 11168 RVA: 0x000F356D File Offset: 0x000F176D
		public virtual void EnableOnAnimatorMove()
		{
			this.UseAnimatorMove = true;
		}

		// Token: 0x06002BA1 RID: 11169 RVA: 0x000F3576 File Offset: 0x000F1776
		public virtual void DisableOnAnimatorMove()
		{
			this.UseAnimatorMove = false;
		}

		// Token: 0x06002BA2 RID: 11170 RVA: 0x000F3580 File Offset: 0x000F1780
		public virtual void InputHandle()
		{
			if (this.lockInput || this.cc.ragdolled)
			{
				return;
			}
			this.MoveInput();
			this.RunInput();
			this.SprintInput();
			this.CrouchInput();
			this.StrafeInput();
			this.JumpInput();
			this.RollInput();
		}

		// Token: 0x06002BA3 RID: 11171 RVA: 0x000F35D0 File Offset: 0x000F17D0
		public virtual void MoveInput()
		{
			if (!this.lockMoveInput)
			{
				Vector3 input = this.cc.input;
				input.x = this.horizontalInput.GetAxisRaw();
				input.z = this.verticalInput.GetAxisRaw();
				this.cc.input = input;
			}
			if (Input.GetKeyDown(this.toggleWalk))
			{
				this.cc.alwaysWalkByDefault = !this.cc.alwaysWalkByDefault;
			}
			this.cc.ControlKeepDirection();
		}

		// Token: 0x06002BA4 RID: 11172 RVA: 0x000F3652 File Offset: 0x000F1852
		public virtual void RunInput()
		{
			if (this.runInput.useInput)
			{
				this.cc.Run(this.cc.useContinuousRun ? this.runInput.GetButtonDown() : this.runInput.GetButton());
			}
		}

		// Token: 0x06002BA5 RID: 11173 RVA: 0x000F3691 File Offset: 0x000F1891
		public virtual void SprintInput()
		{
			if (this.sprintInput.useInput)
			{
				this.cc.Sprint(this.cc.useContinuousSprint ? this.sprintInput.GetButtonDown() : this.sprintInput.GetButton());
			}
		}

		// Token: 0x06002BA6 RID: 11174 RVA: 0x000F36D0 File Offset: 0x000F18D0
		public virtual void StrafeInput()
		{
			if (this.strafeInput.GetButtonDown())
			{
				this.cc.Strafe();
			}
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x000F36EA File Offset: 0x000F18EA
		public virtual void CrouchInput()
		{
			this.cc.AutoCrouch();
			if (this.crouchInput.useInput && this.crouchInput.GetButtonDown())
			{
				this.cc.Crouch();
			}
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x000F371C File Offset: 0x000F191C
		public virtual bool JumpConditions()
		{
			return !this.cc.inJumpStarted && !this.cc.customAction && !this.cc.isCrouching && this.cc.isGrounded && (float)((int)this.cc.GroundAngle()) <= this.cc.slopeLimit && this.cc.currentStamina >= this.cc.jumpStamina && !this.cc.isJumping && !this.cc.isRolling;
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x000F37AC File Offset: 0x000F19AC
		public virtual void JumpInput()
		{
			if (this.jumpInput.GetButtonDown() && this.JumpConditions())
			{
				this.cc.Jump(true);
			}
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x000F37D0 File Offset: 0x000F19D0
		public virtual bool RollConditions()
		{
			return (!this.cc.isRolling || this.cc.canRollAgain) && this.cc.isGrounded && this.cc.input != Vector3.zero && !this.cc.customAction && this.cc.currentStamina > this.cc.rollStamina && !this.cc.isJumping && !this.cc.isSliding;
		}

		// Token: 0x06002BAB RID: 11179 RVA: 0x000F385D File Offset: 0x000F1A5D
		public virtual void RollInput()
		{
			if (this.rollInput.GetButtonDown() && this.RollConditions())
			{
				this.cc.Roll();
			}
		}

		// Token: 0x06002BAC RID: 11180 RVA: 0x000F3880 File Offset: 0x000F1A80
		public virtual void CameraInput()
		{
			if (!this.cameraMain)
			{
				return;
			}
			if (this.tpCamera == null)
			{
				return;
			}
			float num = (this.lockCameraInput ? 0f : this.rotateCameraYInput.GetAxis());
			float num2 = (this.lockCameraInput ? 0f : this.rotateCameraXInput.GetAxis());
			if (this.invertCameraInputHorizontal)
			{
				num2 *= -1f;
			}
			if (this.invertCameraInputVertical)
			{
				num *= -1f;
			}
			float axis = this.cameraZoomInput.GetAxis();
			this.tpCamera.RotateCamera(num2, num);
			if (!this.lockCameraInput)
			{
				this.tpCamera.Zoom(axis);
			}
		}

		// Token: 0x06002BAD RID: 11181 RVA: 0x000F3930 File Offset: 0x000F1B30
		public virtual void UpdateCameraStates()
		{
			if (this.ignoreTpCamera)
			{
				return;
			}
			if (this.tpCamera == null && !this._tpCameraSearched)
			{
				this._tpCameraSearched = true;
				this.tpCamera = Object.FindObjectOfType<vThirdPersonCamera>();
				if (this.tpCamera == null)
				{
					return;
				}
				if (this.tpCamera)
				{
					this.tpCamera.SetMainTarget(base.transform);
					this.tpCamera.Init();
				}
			}
			if (this.tpCamera == null)
			{
				return;
			}
			if (this.changeCameraState)
			{
				this.tpCamera.ChangeState(this.customCameraState, this.customlookAtPoint, this.smoothCameraState);
				return;
			}
			if (this.cc.isCrouching)
			{
				this.tpCamera.ChangeState("Crouch", true);
				return;
			}
			if (this.cc.isStrafing)
			{
				this.tpCamera.ChangeState("Strafing", true);
				return;
			}
			this.tpCamera.ChangeState("Default", true);
		}

		// Token: 0x06002BAE RID: 11182 RVA: 0x000F3A2A File Offset: 0x000F1C2A
		public virtual void ChangeCameraState(string cameraState, bool useLerp = true)
		{
			if (useLerp)
			{
				this.ChangeCameraStateWithLerp(cameraState);
				return;
			}
			this.ChangeCameraStateNoLerp(cameraState);
		}

		// Token: 0x06002BAF RID: 11183 RVA: 0x000F3A3E File Offset: 0x000F1C3E
		public virtual void ResetCameraAngleSmooth()
		{
			if (this.tpCamera)
			{
				this.tpCamera.ResetAngle();
			}
		}

		// Token: 0x06002BB0 RID: 11184 RVA: 0x000F3A58 File Offset: 0x000F1C58
		public virtual void ResetCameraAngleWithoutSmooth()
		{
			if (this.tpCamera)
			{
				this.tpCamera.ResetAngleWithoutSmooth();
			}
		}

		// Token: 0x06002BB1 RID: 11185 RVA: 0x000F3A72 File Offset: 0x000F1C72
		public virtual void ChangeCameraStateWithLerp(string cameraState)
		{
			this.changeCameraState = true;
			this.customCameraState = cameraState;
			this.smoothCameraState = true;
		}

		// Token: 0x06002BB2 RID: 11186 RVA: 0x000F3A89 File Offset: 0x000F1C89
		public virtual void ChangeCameraStateNoLerp(string cameraState)
		{
			this.changeCameraState = true;
			this.customCameraState = cameraState;
			this.smoothCameraState = false;
		}

		// Token: 0x06002BB3 RID: 11187 RVA: 0x000F3AA0 File Offset: 0x000F1CA0
		public virtual void ResetCameraState()
		{
			this.changeCameraState = false;
			this.customCameraState = string.Empty;
		}

		// Token: 0x06002BB4 RID: 11188 RVA: 0x000F3AB4 File Offset: 0x000F1CB4
		public virtual void UpdateHUD()
		{
			if (this.hud == null)
			{
				if (!this._hudSearched)
				{
					this._hudSearched = true;
					if (vHUDController.instance != null)
					{
						this.hud = vHUDController.instance;
						this.hud.Init(this.cc);
					}
				}
				if (this.hud == null)
				{
					return;
				}
			}
			this.hud.UpdateHUD(this.cc);
		}

		// Token: 0x06002BB6 RID: 11190 RVA: 0x000C43E2 File Offset: 0x000C25E2
		bool vIAnimatorMoveReceiver.get_enabled()
		{
			return base.enabled;
		}

		// Token: 0x06002BB7 RID: 11191 RVA: 0x000C43EA File Offset: 0x000C25EA
		void vIAnimatorMoveReceiver.set_enabled(bool value)
		{
			base.enabled = value;
		}

		// Token: 0x04002AE3 RID: 10979
		[vEditorToolbar("Inputs", false, "", false, false)]
		[vHelpBox("Check these options if you need to use the mouse cursor, ex: <b>2.5D, Topdown or Mobile</b>", vHelpBoxAttribute.MessageType.Info)]
		public bool unlockCursorOnStart;

		// Token: 0x04002AE4 RID: 10980
		public bool showCursorOnStart;

		// Token: 0x04002AE5 RID: 10981
		[vHelpBox("PC only - use it to toggle between run/walk", vHelpBoxAttribute.MessageType.Info)]
		public KeyCode toggleWalk = KeyCode.CapsLock;

		// Token: 0x04002AE6 RID: 10982
		[Header("Movement Input")]
		public GenericInput horizontalInput = new GenericInput("Horizontal", "LeftAnalogHorizontal", "Horizontal");

		// Token: 0x04002AE7 RID: 10983
		public GenericInput verticalInput = new GenericInput("Vertical", "LeftAnalogVertical", "Vertical");

		// Token: 0x04002AE8 RID: 10984
		public GenericInput runInput = new GenericInput("LeftShift", "LeftStickClick", "LeftStickClick");

		// Token: 0x04002AE9 RID: 10985
		public GenericInput sprintInput = new GenericInput("LeftControl", "LeftStickClick", "LeftStickClick");

		// Token: 0x04002AEA RID: 10986
		public GenericInput crouchInput = new GenericInput("C", "Y", "Y");

		// Token: 0x04002AEB RID: 10987
		public GenericInput strafeInput = new GenericInput("Tab", "RightStickClick", "RightStickClick");

		// Token: 0x04002AEC RID: 10988
		public GenericInput jumpInput = new GenericInput("Space", "X", "X");

		// Token: 0x04002AED RID: 10989
		public GenericInput rollInput = new GenericInput("Q", "B", "B");

		// Token: 0x04002AEE RID: 10990
		protected bool _lockInput;

		// Token: 0x04002AEF RID: 10991
		[vEditorToolbar("Camera Settings", false, "", false, false)]
		public bool lockCameraInput;

		// Token: 0x04002AF0 RID: 10992
		public bool invertCameraInputVertical;

		// Token: 0x04002AF1 RID: 10993
		public bool invertCameraInputHorizontal;

		// Token: 0x04002AF2 RID: 10994
		[vEditorToolbar("Inputs", false, "", false, false)]
		[Header("Camera Input")]
		public GenericInput rotateCameraXInput = new GenericInput("Mouse X", "RightAnalogHorizontal", "Mouse X");

		// Token: 0x04002AF3 RID: 10995
		public GenericInput rotateCameraYInput = new GenericInput("Mouse Y", "RightAnalogVertical", "Mouse Y");

		// Token: 0x04002AF4 RID: 10996
		public GenericInput cameraZoomInput = new GenericInput("Mouse ScrollWheel", "", "");

		// Token: 0x04002AF5 RID: 10997
		[vEditorToolbar("Events", false, "", false, false)]
		public UnityEvent OnLockCamera;

		// Token: 0x04002AF6 RID: 10998
		public UnityEvent OnUnlockCamera;

		// Token: 0x04002AF7 RID: 10999
		public UnityEvent onEnableAnimatorMove = new UnityEvent();

		// Token: 0x04002AF8 RID: 11000
		public UnityEvent onDisableDisableAnimatorMove = new UnityEvent();

		// Token: 0x04002AF9 RID: 11001
		[HideInInspector]
		public vThirdPersonCamera tpCamera;

		// Token: 0x04002AFA RID: 11002
		[HideInInspector]
		public bool ignoreTpCamera;

		// Token: 0x04002AFB RID: 11003
		[HideInInspector]
		public string customCameraState;

		// Token: 0x04002AFC RID: 11004
		[HideInInspector]
		public string customlookAtPoint;

		// Token: 0x04002AFD RID: 11005
		[HideInInspector]
		public bool changeCameraState;

		// Token: 0x04002AFE RID: 11006
		[HideInInspector]
		public bool smoothCameraState;

		// Token: 0x04002AFF RID: 11007
		[HideInInspector]
		public vThirdPersonController cc;

		// Token: 0x04002B00 RID: 11008
		[HideInInspector]
		public vHUDController hud;

		// Token: 0x04002B01 RID: 11009
		protected bool updateIK;

		// Token: 0x04002B02 RID: 11010
		protected bool isInit;

		// Token: 0x04002B03 RID: 11011
		[HideInInspector]
		public bool lockMoveInput;

		// Token: 0x04002B04 RID: 11012
		protected Camera _cameraMain;

		// Token: 0x04002B05 RID: 11013
		protected bool withoutMainCamera;

		// Token: 0x04002B09 RID: 11017
		private bool _tpCameraSearched;

		// Token: 0x04002B0A RID: 11018
		private bool _hudSearched;

		// Token: 0x020006C4 RID: 1732
		// (Invoke) Token: 0x06002BBA RID: 11194
		public delegate void OnUpdateEvent();
	}
}
