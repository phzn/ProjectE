using System;
using System.Collections;
using System.Collections.Generic;
using Features.Detection;
using Features.HideFlash;
using Features.MainMenu;
using Invector.vCamera;
using ProjectExhibitionist;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.FirstPerson;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Invector.vCharacterController
{
	// Token: 0x0200069A RID: 1690
	[vClassHeader("Input Manager", true, "icon_v2", false, "", iconName = "inputIcon")]
	public class vThirdPersonInput : vMonoBehaviour, vIAnimatorMoveReceiver
	{
		// Token: 0x14000042 RID: 66
		// (add) Token: 0x060027E7 RID: 10215 RVA: 0x000E4104 File Offset: 0x000E2304
		// (remove) Token: 0x060027E8 RID: 10216 RVA: 0x000E413C File Offset: 0x000E233C
		public event vThirdPersonInput.OnUpdateEvent onUpdate;

		// Token: 0x14000043 RID: 67
		// (add) Token: 0x060027E9 RID: 10217 RVA: 0x000E4174 File Offset: 0x000E2374
		// (remove) Token: 0x060027EA RID: 10218 RVA: 0x000E41AC File Offset: 0x000E23AC
		public event vThirdPersonInput.OnUpdateEvent onLateUpdate;

		// Token: 0x14000044 RID: 68
		// (add) Token: 0x060027EB RID: 10219 RVA: 0x000E41E4 File Offset: 0x000E23E4
		// (remove) Token: 0x060027EC RID: 10220 RVA: 0x000E421C File Offset: 0x000E241C
		public event vThirdPersonInput.OnUpdateEvent onFixedUpdate;

		// Token: 0x14000045 RID: 69
		// (add) Token: 0x060027ED RID: 10221 RVA: 0x000E4254 File Offset: 0x000E2454
		// (remove) Token: 0x060027EE RID: 10222 RVA: 0x000E428C File Offset: 0x000E248C
		public event vThirdPersonInput.OnUpdateEvent onAnimatorMove;

		// Token: 0x060027EF RID: 10223 RVA: 0x000E42C1 File Offset: 0x000E24C1
		public void AcquireClothingToggleInputBlock(Object owner)
		{
			if (owner != null)
			{
				this._clothingToggleInputBlockers.Add(owner.GetInstanceID());
			}
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x000E42DE File Offset: 0x000E24DE
		public void ReleaseClothingToggleInputBlock(Object owner)
		{
			if (owner != null)
			{
				this._clothingToggleInputBlockers.Remove(owner.GetInstanceID());
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x060027F1 RID: 10225 RVA: 0x000E42FB File Offset: 0x000E24FB
		// (set) Token: 0x060027F2 RID: 10226 RVA: 0x000E4303 File Offset: 0x000E2503
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
				if (value)
				{
					this.ResetControllerRun();
				}
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x060027F3 RID: 10227 RVA: 0x000E4315 File Offset: 0x000E2515
		protected InputDevice inputDevice
		{
			get
			{
				return vInput.instance.inputDevice;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x060027F4 RID: 10228 RVA: 0x000E4321 File Offset: 0x000E2521
		// (set) Token: 0x060027F5 RID: 10229 RVA: 0x000E4329 File Offset: 0x000E2529
		public virtual bool lockUpdateMoveDirection { get; set; }

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x060027F6 RID: 10230 RVA: 0x000E4334 File Offset: 0x000E2534
		// (set) Token: 0x060027F7 RID: 10231 RVA: 0x000E439C File Offset: 0x000E259C
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

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x060027F8 RID: 10232 RVA: 0x000E43A8 File Offset: 0x000E25A8
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

		// Token: 0x060027F9 RID: 10233 RVA: 0x000E43F4 File Offset: 0x000E25F4
		protected virtual void Start()
		{
			this.cc = base.GetComponent<vThirdPersonController>();
			if (this.cc != null)
			{
				this.cc.Init();
			}
			if (this.firstPersonController == null)
			{
				this.firstPersonController = base.GetComponent<FirstPersonController>();
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

		// Token: 0x060027FA RID: 10234 RVA: 0x000E4487 File Offset: 0x000E2687
		protected virtual IEnumerator CharacterInit()
		{
			this.FindCamera();
			yield return new WaitForEndOfFrame();
			this.FindHUD();
			this.FindClothingController();
			yield break;
		}

		// Token: 0x060027FB RID: 10235 RVA: 0x000E4496 File Offset: 0x000E2696
		public virtual void FindClothingController()
		{
			if (this.clothingController == null)
			{
				this.clothingController = base.GetComponent<ClothingToggleController>();
				if (this.clothingController == null)
				{
					this.clothingController = base.GetComponentInChildren<ClothingToggleController>();
				}
			}
		}

		// Token: 0x060027FC RID: 10236 RVA: 0x000E44CC File Offset: 0x000E26CC
		public virtual void FindHUD()
		{
			if (this.hud == null && vHUDController.instance != null)
			{
				this.hud = vHUDController.instance;
				this.hud.Init(this.cc);
			}
		}

		// Token: 0x060027FD RID: 10237 RVA: 0x000E4508 File Offset: 0x000E2708
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

		// Token: 0x060027FE RID: 10238 RVA: 0x000E45E4 File Offset: 0x000E27E4
		protected virtual void LateUpdate()
		{
			if (this.cc == null)
			{
				return;
			}
			if (this.updateIK && this.onLateUpdate != null)
			{
				this.onLateUpdate();
			}
			this.CameraInput();
			if (!this.updateIK)
			{
				return;
			}
			this.UpdateCameraStates();
			this.updateIK = false;
		}

		// Token: 0x060027FF RID: 10239 RVA: 0x000E4638 File Offset: 0x000E2838
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

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06002800 RID: 10240 RVA: 0x000E4688 File Offset: 0x000E2888
		public virtual bool rotateToLockTargetConditions
		{
			get
			{
				return this.tpCamera && this.tpCamera.lockTarget && this.cc.isStrafing && !this.cc.isRolling && !this.cc.isJumping && !this.cc.customAction;
			}
		}

		// Token: 0x06002801 RID: 10241 RVA: 0x000E46EC File Offset: 0x000E28EC
		public virtual void ControlRotation()
		{
			if (this.cameraMain && !this.lockUpdateMoveDirection && !this.cc.keepDirection)
			{
				Transform transform = ((this.firstPersonController != null && this.firstPersonController.IsFirstPerson) ? this.cc.transform : this.cameraMain.transform);
				this.cc.UpdateMoveDirection(transform);
			}
			if (this.firstPersonController != null && this.firstPersonController.IsFirstPerson)
			{
				return;
			}
			if (this.rotateToLockTargetConditions)
			{
				this.cc.RotateToPosition(this.tpCamera.lockTarget.position);
				return;
			}
			this.cc.ControlRotationType();
		}

		// Token: 0x06002802 RID: 10242 RVA: 0x000E47AC File Offset: 0x000E29AC
		protected virtual void Update()
		{
			if (this.cc == null || Time.timeScale == 0f)
			{
				this.ResetControllerRun();
				return;
			}
			if (this.onUpdate != null)
			{
				this.onUpdate();
			}
			this.InputHandle();
			this.UpdateHUD();
		}

		// Token: 0x06002803 RID: 10243 RVA: 0x000E47F9 File Offset: 0x000E29F9
		protected virtual void OnDisable()
		{
			this.ResetControllerRun();
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x000E4801 File Offset: 0x000E2A01
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

		// Token: 0x06002805 RID: 10245 RVA: 0x000E4830 File Offset: 0x000E2A30
		public virtual void SetLockBasicInput(bool value)
		{
			this.lockInput = value;
		}

		// Token: 0x06002806 RID: 10246 RVA: 0x000E483B File Offset: 0x000E2A3B
		public virtual void SetLockAllInput(bool value)
		{
			this.SetLockBasicInput(value);
		}

		// Token: 0x06002807 RID: 10247 RVA: 0x000ADA34 File Offset: 0x000ABC34
		public virtual void ShowCursor(bool value)
		{
			Cursor.visible = value;
		}

		// Token: 0x06002808 RID: 10248 RVA: 0x000E4844 File Offset: 0x000E2A44
		public virtual void LockCursor(bool value)
		{
			if (!value)
			{
				Cursor.lockState = CursorLockMode.Locked;
				return;
			}
			Cursor.lockState = CursorLockMode.None;
		}

		// Token: 0x06002809 RID: 10249 RVA: 0x000E4856 File Offset: 0x000E2A56
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

		// Token: 0x0600280A RID: 10250 RVA: 0x000E487E File Offset: 0x000E2A7E
		public virtual void SetLockUpdateMoveDirection(bool value)
		{
			this.lockUpdateMoveDirection = value;
		}

		// Token: 0x0600280B RID: 10251 RVA: 0x000E4887 File Offset: 0x000E2A87
		public virtual void SetWalkByDefault(bool value)
		{
			this.cc.freeSpeed.walkByDefault = value;
			this.cc.strafeSpeed.walkByDefault = value;
		}

		// Token: 0x0600280C RID: 10252 RVA: 0x000E48AC File Offset: 0x000E2AAC
		public virtual void ResetWalkByDefault()
		{
			this.cc.freeSpeed.walkByDefault = this.cc.freeSpeed.defaultWalkByDefault;
			this.cc.strafeSpeed.walkByDefault = this.cc.strafeSpeed.defaultWalkByDefault;
		}

		// Token: 0x0600280D RID: 10253 RVA: 0x000E48F9 File Offset: 0x000E2AF9
		public virtual void SetStrafeLocomotion(bool value)
		{
			this.cc.lockInStrafe = value;
			this.cc.isStrafing = value;
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x0600280E RID: 10254 RVA: 0x000E4913 File Offset: 0x000E2B13
		// (set) Token: 0x0600280F RID: 10255 RVA: 0x000E491B File Offset: 0x000E2B1B
		public virtual vAnimatorMoveSender animatorMoveSender { get; set; }

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06002810 RID: 10256 RVA: 0x000E4924 File Offset: 0x000E2B24
		// (set) Token: 0x06002811 RID: 10257 RVA: 0x000E492C File Offset: 0x000E2B2C
		protected bool _useAnimatorMove { get; set; }

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06002812 RID: 10258 RVA: 0x000E4935 File Offset: 0x000E2B35
		// (set) Token: 0x06002813 RID: 10259 RVA: 0x000E4940 File Offset: 0x000E2B40
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

		// Token: 0x06002814 RID: 10260 RVA: 0x000E49AD File Offset: 0x000E2BAD
		public virtual void EnableOnAnimatorMove()
		{
			this.UseAnimatorMove = true;
		}

		// Token: 0x06002815 RID: 10261 RVA: 0x000E49B6 File Offset: 0x000E2BB6
		public virtual void DisableOnAnimatorMove()
		{
			this.UseAnimatorMove = false;
		}

		// Token: 0x06002816 RID: 10262 RVA: 0x000E49C0 File Offset: 0x000E2BC0
		public virtual void InputHandle()
		{
			if (this.lockInput || this.cc.ragdolled)
			{
				this.ResetControllerRun();
				return;
			}
			this.MoveInput();
			this.RunInput();
			this.SprintInput();
			this.CrouchInput();
			this.StrafeInput();
			this.JumpInput();
			this.RollInput();
			this.ClothingToggleInput();
			this.CoverPoseInput();
			this.FirstPersonToggleInput();
		}

		// Token: 0x06002817 RID: 10263 RVA: 0x000E4A28 File Offset: 0x000E2C28
		public virtual void MoveInput()
		{
			if (!this.lockMoveInput)
			{
				Vector3 input = this.cc.input;
				input.x = this.horizontalInput.GetAxisRaw();
				input.z = this.verticalInput.GetAxisRaw();
				this.cc.input = input;
			}
			if (!TextInputFocus.BlocksGameplayInput && GameplayInput.GetKeyDown(this.toggleWalk))
			{
				this.cc.alwaysWalkByDefault = !this.cc.alwaysWalkByDefault;
			}
			this.cc.ControlKeepDirection();
		}

		// Token: 0x06002818 RID: 10264 RVA: 0x000E4AB4 File Offset: 0x000E2CB4
		public virtual void RunInput()
		{
			if (!ControllerInput.IsActive)
			{
				this.ResetControllerRun();
				if (this.runInput.useInput)
				{
					this.cc.Run(this.cc.useContinuousRun ? vThirdPersonInput.PrimaryDown(this.runInput, KeyCode.LeftShift) : vThirdPersonInput.PrimaryHeld(this.runInput, KeyCode.LeftShift));
				}
				return;
			}
			int deviceId = Gamepad.current.deviceId;
			if (this._controllerRunDeviceId != deviceId)
			{
				this.ResetControllerRun();
				this._controllerRunDeviceId = deviceId;
			}
			if (!this.runInput.useInput || this.lockInput || this.lockMoveInput || this.cc.ragdolled || this.cc.isDead || this.cc.customAction || this.cc.isCrouching || !this.cc.hasMovementInput || !ControllerGameplayBridge.AllowsAxes || ControllerWorldPointer.BlocksContextInput || PlayerActivityGate.IsResourceOwned(PlayerContextResource.PlayerControl | PlayerContextResource.ModalUI))
			{
				this.ResetControllerRun();
				return;
			}
			this._controllerOwnsRun = true;
			if (!ControllerInput.Modifier && ControllerInput.GetButtonDown(ControllerButton.LeftStick))
			{
				this._controllerRunLatched = !this._controllerRunLatched;
			}
			if (this._controllerRunLatched)
			{
				this.cc.Run(!this.cc.useContinuousRun || !this.cc.isRunning);
				return;
			}
			this.cc.isRunning = false;
		}

		// Token: 0x06002819 RID: 10265 RVA: 0x000E4C19 File Offset: 0x000E2E19
		private void ResetControllerRun()
		{
			if (this._controllerOwnsRun && this.cc != null)
			{
				this.cc.isRunning = false;
			}
			this._controllerRunLatched = false;
			this._controllerOwnsRun = false;
			this._controllerRunDeviceId = -1;
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x000E4C54 File Offset: 0x000E2E54
		public virtual void SprintInput()
		{
			if (this.sprintInput.useInput)
			{
				this.cc.Sprint(this.cc.useContinuousSprint ? vThirdPersonInput.PrimaryDown(this.sprintInput, KeyCode.LeftControl) : vThirdPersonInput.PrimaryHeld(this.sprintInput, KeyCode.LeftControl));
			}
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x000E4CA8 File Offset: 0x000E2EA8
		public virtual void StrafeInput()
		{
			if (ControllerInput.IsActive)
			{
				return;
			}
			if (vThirdPersonInput.PrimaryDown(this.strafeInput, KeyCode.Tab))
			{
				this.cc.Strafe();
			}
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x000E4CCC File Offset: 0x000E2ECC
		public virtual void CrouchInput()
		{
			if (PlayerActivityGate.IsBlocked(PlayerActivity.Crouch))
			{
				return;
			}
			this.cc.AutoCrouch();
			if (this.crouchInput.useInput && vThirdPersonInput.PrimaryDown(this.crouchInput, KeyCode.C))
			{
				this.cc.Crouch();
			}
		}

		// Token: 0x0600281D RID: 10269 RVA: 0x000E4D18 File Offset: 0x000E2F18
		public virtual bool JumpConditions()
		{
			return !HideFlashController.HasHeldCoverItem && (!this.cc.inJumpStarted && !this.cc.customAction && !this.cc.isCrouching && this.cc.isGrounded && (float)((int)this.cc.GroundAngle()) <= this.cc.slopeLimit && this.cc.currentStamina >= this.cc.jumpStamina && !this.cc.isJumping) && !this.cc.isRolling;
		}

		// Token: 0x0600281E RID: 10270 RVA: 0x000E4DB1 File Offset: 0x000E2FB1
		public virtual void JumpInput()
		{
			if (!vThirdPersonInput.PrimaryDown(this.jumpInput, KeyCode.Space))
			{
				return;
			}
			if (this.JumpConditions())
			{
				this.cc.Jump(true);
			}
		}

		// Token: 0x0600281F RID: 10271 RVA: 0x000E4DD8 File Offset: 0x000E2FD8
		public virtual bool RollConditions()
		{
			return !HideFlashController.HasHeldCoverItem && (!(this.firstPersonController != null) || !this.firstPersonController.ShouldBlockRoll()) && ((!this.cc.isRolling || this.cc.canRollAgain) && this.cc.isGrounded && this.cc.input != Vector3.zero && !this.cc.customAction && this.cc.currentStamina > this.cc.rollStamina && !this.cc.isJumping) && !this.cc.isSliding;
		}

		// Token: 0x06002820 RID: 10272 RVA: 0x000E4E8B File Offset: 0x000E308B
		public virtual void RollInput()
		{
			if (vThirdPersonInput.PrimaryDown(this.rollInput, KeyCode.Q) && this.RollConditions())
			{
				this.cc.Roll();
			}
		}

		// Token: 0x06002821 RID: 10273 RVA: 0x000E4EB0 File Offset: 0x000E30B0
		public virtual void FirstPersonToggleInput()
		{
			if (ControllerInput.IsActive && PlayerActivityGate.IsResourceOwned(PlayerContextResource.Camera | PlayerContextResource.ModalUI))
			{
				return;
			}
			if (vThirdPersonInput.PrimaryDown(this.firstPersonToggleInput, KeyCode.V))
			{
				if (this.firstPersonController == null)
				{
					this.firstPersonController = base.GetComponent<FirstPersonController>();
				}
				if (this.firstPersonController != null)
				{
					this.firstPersonController.ToggleFirstPerson();
				}
			}
		}

		// Token: 0x06002822 RID: 10274 RVA: 0x000E4F10 File Offset: 0x000E3110
		public virtual void CoverPoseInput()
		{
			if (ControllerInput.IsActive && ControllerContextBindings.CoverPoseInputReserved)
			{
				return;
			}
			if (this.suppressBuiltInCoverPoseInput)
			{
				return;
			}
			if (this.clothingController != null && this.clothingController.IsFullyClothed)
			{
				return;
			}
			if (CoverPoseController.Instance != null && !CoverPoseController.Instance.IsCoverAllowed)
			{
				return;
			}
			if (vThirdPersonInput.PrimaryDown(this.coverPoseInput, KeyCode.X))
			{
				this.ToggleCoverPose();
			}
		}

		// Token: 0x06002823 RID: 10275 RVA: 0x000E4F80 File Offset: 0x000E3180
		public virtual void ToggleCoverPose()
		{
			if (CoverPoseController.Instance != null)
			{
				if (CoverPoseController.Instance.IsCovering)
				{
					CoverPoseController.Instance.DeactivateCoverPose();
					return;
				}
				CoverPoseController.Instance.ActivateCoverPoseFromInput();
			}
		}

		// Token: 0x06002824 RID: 10276 RVA: 0x000E4FB0 File Offset: 0x000E31B0
		public virtual void ClothingToggleInput()
		{
			if (this.clothingController == null)
			{
				return;
			}
			if (PlayerActivityGate.IsBlocked(PlayerActivity.Undress))
			{
				return;
			}
			if (this._clothingToggleInputBlockers.Count > 0)
			{
				return;
			}
			for (int i = 0; i < this.toggleGroupInputs.Length; i++)
			{
				if (this.toggleGroupInputs[i].GetButtonDown())
				{
					this.clothingController.ToggleGroup(i);
				}
			}
		}

		// Token: 0x06002825 RID: 10277 RVA: 0x000E5014 File Offset: 0x000E3214
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
			bool flag = this.firstPersonController != null && this.firstPersonController.IsFirstPerson;
			bool flag2 = this.lockCameraInput && (vThirdPersonInput.ForceCameraLock || !flag);
			float num = (flag2 ? 0f : this.rotateCameraYInput.GetAxis());
			float num2 = (flag2 ? 0f : this.rotateCameraXInput.GetAxis());
			if (this.invertCameraInputHorizontal)
			{
				num2 *= -1f;
			}
			if (this.invertCameraInputVertical)
			{
				num *= -1f;
			}
			if (!ControllerInput.IsActive)
			{
				Vector2 vector = GameSettingsService.ApplyMouseLookInversion(new Vector2(num2, num));
				num2 = vector.x;
				num = vector.y;
			}
			float num3 = (this.suppressCameraZoom ? 0f : (ControllerInput.IsActive ? (ControllerGameplayBridge.AllowsAxes ? ControllerInput.Scroll : 0f) : this.cameraZoomInput.GetAxis()));
			float num4 = (ControllerGameplayBridge.AllowsAxes ? ControllerInput.CameraHeight : 0f);
			bool flag3 = (ControllerInput.IsActive ? (num4 > 0f) : this.cameraHeightUpInput.GetButton());
			bool flag4 = (ControllerInput.IsActive ? (num4 < 0f) : this.cameraHeightDownInput.GetButton());
			if (flag)
			{
				this.firstPersonController.RotateCameraFP(num2, num);
				return;
			}
			this.tpCamera.RotateCamera(num2, num);
			if (!this.lockCameraInput)
			{
				this.tpCamera.Zoom(num3);
				if (flag3)
				{
					this.tpCamera.AdjustHeight(1f);
				}
				if (flag4)
				{
					this.tpCamera.AdjustHeight(-1f);
				}
			}
		}

		// Token: 0x06002826 RID: 10278 RVA: 0x000E51C3 File Offset: 0x000E33C3
		private static bool PrimaryDown(GenericInput input, KeyCode controllerAction)
		{
			if (input == null || !input.useInput)
			{
				return false;
			}
			if (!ControllerInput.IsActive)
			{
				return input.GetButtonDown();
			}
			return GameplayInput.GetKeyDown(controllerAction);
		}

		// Token: 0x06002827 RID: 10279 RVA: 0x000E51E6 File Offset: 0x000E33E6
		private static bool PrimaryHeld(GenericInput input, KeyCode controllerAction)
		{
			if (input == null || !input.useInput)
			{
				return false;
			}
			if (!ControllerInput.IsActive)
			{
				return input.GetButton();
			}
			return GameplayInput.GetKey(controllerAction);
		}

		// Token: 0x06002828 RID: 10280 RVA: 0x000E520C File Offset: 0x000E340C
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
			if (this.firstPersonController != null && this.firstPersonController.IsFirstPerson)
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

		// Token: 0x06002829 RID: 10281 RVA: 0x000E5322 File Offset: 0x000E3522
		public virtual void ChangeCameraState(string cameraState, bool useLerp = true)
		{
			if (useLerp)
			{
				this.ChangeCameraStateWithLerp(cameraState);
				return;
			}
			this.ChangeCameraStateNoLerp(cameraState);
		}

		// Token: 0x0600282A RID: 10282 RVA: 0x000E5336 File Offset: 0x000E3536
		public virtual void ResetCameraAngleSmooth()
		{
			if (this.tpCamera)
			{
				this.tpCamera.ResetAngle();
			}
		}

		// Token: 0x0600282B RID: 10283 RVA: 0x000E5350 File Offset: 0x000E3550
		public virtual void ResetCameraAngleWithoutSmooth()
		{
			if (this.tpCamera)
			{
				this.tpCamera.ResetAngleWithoutSmooth();
			}
		}

		// Token: 0x0600282C RID: 10284 RVA: 0x000E536A File Offset: 0x000E356A
		public virtual void ChangeCameraStateWithLerp(string cameraState)
		{
			this.changeCameraState = true;
			this.customCameraState = cameraState;
			this.smoothCameraState = true;
		}

		// Token: 0x0600282D RID: 10285 RVA: 0x000E5381 File Offset: 0x000E3581
		public virtual void ChangeCameraStateNoLerp(string cameraState)
		{
			this.changeCameraState = true;
			this.customCameraState = cameraState;
			this.smoothCameraState = false;
		}

		// Token: 0x0600282E RID: 10286 RVA: 0x000E5398 File Offset: 0x000E3598
		public virtual void ResetCameraState()
		{
			this.changeCameraState = false;
			this.customCameraState = string.Empty;
		}

		// Token: 0x0600282F RID: 10287 RVA: 0x000E53AC File Offset: 0x000E35AC
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

		// Token: 0x06002831 RID: 10289 RVA: 0x000C43E2 File Offset: 0x000C25E2
		bool vIAnimatorMoveReceiver.get_enabled()
		{
			return base.enabled;
		}

		// Token: 0x06002832 RID: 10290 RVA: 0x000C43EA File Offset: 0x000C25EA
		void vIAnimatorMoveReceiver.set_enabled(bool value)
		{
			base.enabled = value;
		}

		// Token: 0x04002872 RID: 10354
		[vEditorToolbar("Inputs", false, "", false, false)]
		[vHelpBox("Check these options if you need to use the mouse cursor, ex: <b>2.5D, Topdown or Mobile</b>", vHelpBoxAttribute.MessageType.Info)]
		public bool unlockCursorOnStart;

		// Token: 0x04002873 RID: 10355
		public bool showCursorOnStart;

		// Token: 0x04002874 RID: 10356
		[vHelpBox("PC only - use it to toggle between run/walk", vHelpBoxAttribute.MessageType.Info)]
		public KeyCode toggleWalk = KeyCode.CapsLock;

		// Token: 0x04002875 RID: 10357
		[Header("Movement Input")]
		public GenericInput horizontalInput = new GenericInput("Horizontal", "LeftAnalogHorizontal", "Horizontal");

		// Token: 0x04002876 RID: 10358
		public GenericInput verticalInput = new GenericInput("Vertical", "LeftAnalogVertical", "Vertical");

		// Token: 0x04002877 RID: 10359
		public GenericInput runInput = new GenericInput("LeftShift", "LeftStickClick", "LeftStickClick");

		// Token: 0x04002878 RID: 10360
		public GenericInput sprintInput = new GenericInput("LeftControl", "LeftStickClick", "LeftStickClick");

		// Token: 0x04002879 RID: 10361
		public GenericInput crouchInput = new GenericInput("C", "Y", "Y");

		// Token: 0x0400287A RID: 10362
		public GenericInput strafeInput = new GenericInput("Tab", "RightStickClick", "RightStickClick");

		// Token: 0x0400287B RID: 10363
		public GenericInput jumpInput = new GenericInput("Space", "X", "X");

		// Token: 0x0400287C RID: 10364
		public GenericInput rollInput = new GenericInput("Q", "B", "B");

		// Token: 0x0400287D RID: 10365
		[Header("Clothing Toggle")]
		[HideInInspector]
		public GenericInput toggleAllClothingInput = new GenericInput("", "", "")
		{
			useInput = false
		};

		// Token: 0x0400287E RID: 10366
		public GenericInput[] toggleGroupInputs = new GenericInput[]
		{
			new GenericInput("F1", "", ""),
			new GenericInput("F2", "", ""),
			new GenericInput("F3", "", "")
		};

		// Token: 0x0400287F RID: 10367
		[HideInInspector]
		public ClothingToggleController clothingController;

		// Token: 0x04002880 RID: 10368
		private readonly HashSet<int> _clothingToggleInputBlockers = new HashSet<int>();

		// Token: 0x04002881 RID: 10369
		[Header("Cover Pose")]
		[Tooltip("Input to toggle cover pose on/off")]
		public GenericInput coverPoseInput = new GenericInput("X", "A", "A");

		// Token: 0x04002882 RID: 10370
		[HideInInspector]
		public bool suppressBuiltInCoverPoseInput;

		// Token: 0x04002883 RID: 10371
		[HideInInspector]
		public bool suppressCameraZoom;

		// Token: 0x04002884 RID: 10372
		[Header("First Person")]
		[Tooltip("Input to toggle first person camera mode")]
		public GenericInput firstPersonToggleInput = new GenericInput("V", "", "");

		// Token: 0x04002885 RID: 10373
		[HideInInspector]
		public FirstPersonController firstPersonController;

		// Token: 0x04002886 RID: 10374
		protected bool _lockInput;

		// Token: 0x04002887 RID: 10375
		private bool _controllerRunLatched;

		// Token: 0x04002888 RID: 10376
		private int _controllerRunDeviceId = -1;

		// Token: 0x04002889 RID: 10377
		private bool _controllerOwnsRun;

		// Token: 0x0400288A RID: 10378
		[vEditorToolbar("Camera Settings", false, "", false, false)]
		public bool lockCameraInput;

		// Token: 0x0400288B RID: 10379
		public static bool ForceCameraLock;

		// Token: 0x0400288C RID: 10380
		public bool invertCameraInputVertical;

		// Token: 0x0400288D RID: 10381
		public bool invertCameraInputHorizontal;

		// Token: 0x0400288E RID: 10382
		[vEditorToolbar("Inputs", false, "", false, false)]
		[Header("Camera Input")]
		public GenericInput rotateCameraXInput = new GenericInput("Mouse X", "RightAnalogHorizontal", "Mouse X");

		// Token: 0x0400288F RID: 10383
		public GenericInput rotateCameraYInput = new GenericInput("Mouse Y", "RightAnalogVertical", "Mouse Y");

		// Token: 0x04002890 RID: 10384
		public GenericInput cameraZoomInput = new GenericInput("Mouse ScrollWheel", "", "");

		// Token: 0x04002891 RID: 10385
		public GenericInput cameraHeightUpInput = new GenericInput("UpArrow", "DPadUp", "DPadUp");

		// Token: 0x04002892 RID: 10386
		public GenericInput cameraHeightDownInput = new GenericInput("DownArrow", "DPadDown", "DPadDown");

		// Token: 0x04002893 RID: 10387
		[vEditorToolbar("Events", false, "", false, false)]
		public UnityEvent OnLockCamera;

		// Token: 0x04002894 RID: 10388
		public UnityEvent OnUnlockCamera;

		// Token: 0x04002895 RID: 10389
		public UnityEvent onEnableAnimatorMove = new UnityEvent();

		// Token: 0x04002896 RID: 10390
		public UnityEvent onDisableDisableAnimatorMove = new UnityEvent();

		// Token: 0x04002897 RID: 10391
		[HideInInspector]
		public vThirdPersonCamera tpCamera;

		// Token: 0x04002898 RID: 10392
		[HideInInspector]
		public bool ignoreTpCamera;

		// Token: 0x04002899 RID: 10393
		[HideInInspector]
		public string customCameraState;

		// Token: 0x0400289A RID: 10394
		[HideInInspector]
		public string customlookAtPoint;

		// Token: 0x0400289B RID: 10395
		[HideInInspector]
		public bool changeCameraState;

		// Token: 0x0400289C RID: 10396
		[HideInInspector]
		public bool smoothCameraState;

		// Token: 0x0400289D RID: 10397
		[HideInInspector]
		public vThirdPersonController cc;

		// Token: 0x0400289E RID: 10398
		[HideInInspector]
		public vHUDController hud;

		// Token: 0x0400289F RID: 10399
		protected bool updateIK;

		// Token: 0x040028A0 RID: 10400
		protected bool isInit;

		// Token: 0x040028A1 RID: 10401
		[HideInInspector]
		public bool lockMoveInput;

		// Token: 0x040028A2 RID: 10402
		protected Camera _cameraMain;

		// Token: 0x040028A3 RID: 10403
		protected bool withoutMainCamera;

		// Token: 0x040028A7 RID: 10407
		private bool _tpCameraSearched;

		// Token: 0x040028A8 RID: 10408
		private bool _hudSearched;

		// Token: 0x0200069B RID: 1691
		// (Invoke) Token: 0x06002835 RID: 10293
		public delegate void OnUpdateEvent();
	}
}
