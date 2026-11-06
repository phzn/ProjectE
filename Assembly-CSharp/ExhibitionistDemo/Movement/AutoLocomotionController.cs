using System;
using ExhibitionistDemo.UI;
using Invector.vCharacterController;
using ProjectExhibitionist.Minimap;
using UnityEngine;
using UnityEngine.Serialization;

namespace ExhibitionistDemo.Movement
{
	// Token: 0x02000796 RID: 1942
	[DefaultExecutionOrder(100)]
	[RequireComponent(typeof(vThirdPersonController))]
	[RequireComponent(typeof(vThirdPersonInput))]
	public class AutoLocomotionController : MonoBehaviour
	{
		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x06003333 RID: 13107 RVA: 0x0012299E File Offset: 0x00120B9E
		public AutoLocomotionController.AutoLocomotionMode ActiveMode
		{
			get
			{
				return this.activeMode;
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x06003334 RID: 13108 RVA: 0x001229A6 File Offset: 0x00120BA6
		public AutoLocomotionController.AutoLocomotionMode EffectiveMode
		{
			get
			{
				if (this.activeMode != AutoLocomotionController.AutoLocomotionMode.Run || !this.autoRunWaitingForFullStamina)
				{
					return this.activeMode;
				}
				return AutoLocomotionController.AutoLocomotionMode.Walk;
			}
		}

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x06003335 RID: 13109 RVA: 0x001229C1 File Offset: 0x00120BC1
		public bool IsAutoRunWaitingForFullStamina
		{
			get
			{
				return this.autoRunWaitingForFullStamina;
			}
		}

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x06003336 RID: 13110 RVA: 0x001229C9 File Offset: 0x00120BC9
		public int WalkSpeedTier
		{
			get
			{
				return this.walkSpeedTier;
			}
		}

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x06003337 RID: 13111 RVA: 0x001229D1 File Offset: 0x00120BD1
		public int RunSpeedTier
		{
			get
			{
				return this.runSpeedTier;
			}
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x06003338 RID: 13112 RVA: 0x001229D9 File Offset: 0x00120BD9
		public float StaminaNormalized
		{
			get
			{
				if (!(this.staminaController != null))
				{
					return 1f;
				}
				return this.staminaController.StaminaNormalized;
			}
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x001229FC File Offset: 0x00120BFC
		private void Awake()
		{
			this.controller = base.GetComponent<vThirdPersonController>();
			this.inputController = base.GetComponent<vThirdPersonInput>();
			this.staminaController = base.GetComponent<CustomStaminaController>();
			this.locomotionHud = base.GetComponent<AutoLocomotionHUD>();
			if (this.locomotionHud == null)
			{
				this.locomotionHud = base.gameObject.AddComponent<AutoLocomotionHUD>();
			}
			this.locomotionHud.Bind(this);
		}

		// Token: 0x0600333A RID: 13114 RVA: 0x00122A64 File Offset: 0x00120C64
		private void OnDisable()
		{
			this.CancelAutoLocomotion();
			this.ReleaseCameraZoom();
			this.modifierPressActive = false;
		}

		// Token: 0x0600333B RID: 13115 RVA: 0x00122A7C File Offset: 0x00120C7C
		private void Update()
		{
			if (this.controller == null || this.inputController == null)
			{
				return;
			}
			if (Time.timeScale <= 0f)
			{
				this.modifierPressActive = false;
				this.ReleaseCameraZoom();
				return;
			}
			if (ProceduralMiniMapController.IsMapExpanded)
			{
				this.AbortModifierChord();
			}
			else
			{
				this.HandleAutoMoveChord();
			}
			if (this.activeMode == AutoLocomotionController.AutoLocomotionMode.Off)
			{
				this.manualCancelArmed = true;
				this.controller.automaticMovementSpeedMultiplier = 1f;
				return;
			}
			if (this.ShouldCancelForControllerState())
			{
				this.CancelAutoLocomotion();
				return;
			}
			if (this.HasManualMovementInput())
			{
				this.CancelAutoLocomotion();
				return;
			}
			Vector3 input = this.controller.input;
			input.x = 0f;
			input.z = Mathf.Max(1f, input.z);
			this.controller.input = input;
			this.controller.moveDirection = this.automaticMoveDirection;
			this.controller.isSprinting = false;
			if (this.activeMode == AutoLocomotionController.AutoLocomotionMode.Run)
			{
				bool flag = this.staminaController == null || this.staminaController.CanRun;
				if (!flag)
				{
					this.autoRunWaitingForFullStamina = true;
				}
				if (this.autoRunWaitingForFullStamina && (this.staminaController == null || this.staminaController.IsStaminaFull))
				{
					this.autoRunWaitingForFullStamina = false;
					flag = this.staminaController == null || this.staminaController.CanRun;
				}
				this.controller.isRunning = flag && !this.autoRunWaitingForFullStamina;
			}
			else
			{
				this.autoRunWaitingForFullStamina = false;
				this.controller.isRunning = false;
			}
			this.controller.automaticMovementSpeedMultiplier = this.GetActiveSpeedMultiplier();
		}

		// Token: 0x0600333C RID: 13116 RVA: 0x00122C28 File Offset: 0x00120E28
		private void HandleAutoMoveChord()
		{
			if (this.autoMoveInput == null || !this.autoMoveInput.useInput)
			{
				this.ReleaseCameraZoom();
				return;
			}
			if (this.autoMoveInput.GetButtonDown())
			{
				this.modifierPressActive = true;
				this.modifierUsedForSpeed = false;
				this.runModifierOnPress = this.controller.isRunning || (this.inputController.runInput != null && this.inputController.runInput.useInput && this.inputController.runInput.GetButton());
				this.activationEligibleOnPress = this.IsMovingManuallyForward();
				this.SuppressCameraZoom();
			}
			if (this.modifierPressActive && this.autoMoveInput.GetButton())
			{
				float num = ((this.speedAdjustmentInput != null && this.speedAdjustmentInput.useInput) ? this.speedAdjustmentInput.GetAxisRaw() : 0f);
				if (Mathf.Abs(num) > 0.001f)
				{
					this.modifierUsedForSpeed = true;
					this.AdjustSpeedTier((num > 0f) ? 1 : (-1));
				}
			}
			if (this.modifierPressActive && this.autoMoveInput.GetButtonUp())
			{
				if (!this.modifierUsedForSpeed)
				{
					AutoLocomotionController.AutoLocomotionMode autoLocomotionMode = (this.runModifierOnPress ? AutoLocomotionController.AutoLocomotionMode.Run : AutoLocomotionController.AutoLocomotionMode.Walk);
					if (this.activeMode == autoLocomotionMode || this.activationEligibleOnPress)
					{
						this.ToggleMode(autoLocomotionMode);
					}
				}
				this.modifierPressActive = false;
				this.activationEligibleOnPress = false;
				this.ReleaseCameraZoom();
			}
		}

		// Token: 0x0600333D RID: 13117 RVA: 0x00122D7F File Offset: 0x00120F7F
		private void ToggleMode(AutoLocomotionController.AutoLocomotionMode requestedMode)
		{
			if (this.activeMode == requestedMode)
			{
				this.CancelAutoLocomotion();
				return;
			}
			this.activeMode = requestedMode;
			this.autoRunWaitingForFullStamina = false;
			this.CaptureAutomaticMoveDirection();
			this.AcquireMoveDirectionLock();
			this.manualCancelArmed = false;
		}

		// Token: 0x0600333E RID: 13118 RVA: 0x00122DB4 File Offset: 0x00120FB4
		private void AdjustSpeedTier(int direction)
		{
			AutoLocomotionController.AutoLocomotionMode autoLocomotionMode = ((this.activeMode != AutoLocomotionController.AutoLocomotionMode.Off) ? this.EffectiveMode : (this.runModifierOnPress ? AutoLocomotionController.AutoLocomotionMode.Run : AutoLocomotionController.AutoLocomotionMode.Walk));
			if (autoLocomotionMode == AutoLocomotionController.AutoLocomotionMode.Run)
			{
				this.runSpeedTier = Mathf.Clamp(this.runSpeedTier + direction, 0, 2);
				AutoLocomotionHUD autoLocomotionHUD = this.locomotionHud;
				if (autoLocomotionHUD == null)
				{
					return;
				}
				autoLocomotionHUD.ShowSpeedFeedback(autoLocomotionMode, this.runSpeedTier, this.GetSpeedMultiplier(this.runSpeedTier));
				return;
			}
			else
			{
				this.walkSpeedTier = Mathf.Clamp(this.walkSpeedTier + direction, 0, 2);
				AutoLocomotionHUD autoLocomotionHUD2 = this.locomotionHud;
				if (autoLocomotionHUD2 == null)
				{
					return;
				}
				autoLocomotionHUD2.ShowSpeedFeedback(autoLocomotionMode, this.walkSpeedTier, this.GetSpeedMultiplier(this.walkSpeedTier));
				return;
			}
		}

		// Token: 0x0600333F RID: 13119 RVA: 0x00122E54 File Offset: 0x00121054
		private float GetActiveSpeedMultiplier()
		{
			int num = ((this.EffectiveMode == AutoLocomotionController.AutoLocomotionMode.Run) ? this.runSpeedTier : this.walkSpeedTier);
			return this.GetSpeedMultiplier(num);
		}

		// Token: 0x06003340 RID: 13120 RVA: 0x00122E80 File Offset: 0x00121080
		private float GetSpeedMultiplier(int tier)
		{
			if (tier == 0)
			{
				return Mathf.Max(0.1f, this.lowerSpeedMultiplier);
			}
			if (tier != 2)
			{
				return Mathf.Max(0.1f, this.normalSpeedMultiplier);
			}
			return Mathf.Max(0.1f, this.fasterSpeedMultiplier);
		}

		// Token: 0x06003341 RID: 13121 RVA: 0x00122EC0 File Offset: 0x001210C0
		private bool ShouldCancelForControllerState()
		{
			return !this.inputController.enabled || !this.controller.enabled || this.inputController.lockInput || this.inputController.lockMoveInput || this.controller.ragdolled || this.controller.isDead || this.controller.customAction || this.controller.isCrouching;
		}

		// Token: 0x06003342 RID: 13122 RVA: 0x00122F38 File Offset: 0x00121138
		private bool HasManualMovementInput()
		{
			float num;
			float num2;
			this.ReadManualMovementAxes(out num, out num2);
			bool flag = Mathf.Abs(num) >= this.manualMovementCancelThreshold || Mathf.Abs(num2) >= this.manualMovementCancelThreshold;
			if (!this.manualCancelArmed)
			{
				if (!flag)
				{
					this.manualCancelArmed = true;
				}
				return false;
			}
			return flag;
		}

		// Token: 0x06003343 RID: 13123 RVA: 0x00122F88 File Offset: 0x00121188
		private bool IsMovingManuallyForward()
		{
			float num;
			float num2;
			this.ReadManualMovementAxes(out num, out num2);
			bool flag = num2 >= this.manualMovementCancelThreshold;
			bool flag2 = new Vector2(num, num2).sqrMagnitude >= this.manualMovementCancelThreshold * this.manualMovementCancelThreshold;
			return this.activeMode == AutoLocomotionController.AutoLocomotionMode.Off && flag && flag2 && this.controller.isGrounded && !this.ShouldCancelForControllerState();
		}

		// Token: 0x06003344 RID: 13124 RVA: 0x00122FF8 File Offset: 0x001211F8
		private void ReadManualMovementAxes(out float horizontal, out float vertical)
		{
			horizontal = ((this.inputController.horizontalInput != null && this.inputController.horizontalInput.useInput) ? this.inputController.horizontalInput.GetAxisRaw() : 0f);
			vertical = ((this.inputController.verticalInput != null && this.inputController.verticalInput.useInput) ? this.inputController.verticalInput.GetAxisRaw() : 0f);
		}

		// Token: 0x06003345 RID: 13125 RVA: 0x00123078 File Offset: 0x00121278
		public void CancelAutoLocomotion()
		{
			this.AbortModifierChord();
			this.activeMode = AutoLocomotionController.AutoLocomotionMode.Off;
			this.autoRunWaitingForFullStamina = false;
			this.manualCancelArmed = true;
			this.ReleaseMoveDirectionLock();
			if (this.controller != null)
			{
				this.controller.automaticMovementSpeedMultiplier = 1f;
				this.controller.isRunning = false;
				this.controller.isSprinting = false;
			}
		}

		// Token: 0x06003346 RID: 13126 RVA: 0x001230DC File Offset: 0x001212DC
		private void AbortModifierChord()
		{
			this.modifierPressActive = false;
			this.modifierUsedForSpeed = false;
			this.activationEligibleOnPress = false;
			this.ReleaseCameraZoom();
		}

		// Token: 0x06003347 RID: 13127 RVA: 0x001230F9 File Offset: 0x001212F9
		private void SuppressCameraZoom()
		{
			if (this.ownsCameraZoomSuppression)
			{
				return;
			}
			this.previousCameraZoomSuppression = this.inputController.suppressCameraZoom;
			this.inputController.suppressCameraZoom = true;
			this.ownsCameraZoomSuppression = true;
		}

		// Token: 0x06003348 RID: 13128 RVA: 0x00123128 File Offset: 0x00121328
		private void ReleaseCameraZoom()
		{
			if (!this.ownsCameraZoomSuppression || this.inputController == null)
			{
				return;
			}
			this.inputController.suppressCameraZoom = this.previousCameraZoomSuppression;
			this.ownsCameraZoomSuppression = false;
		}

		// Token: 0x06003349 RID: 13129 RVA: 0x0012315C File Offset: 0x0012135C
		private void CaptureAutomaticMoveDirection()
		{
			this.automaticMoveDirection = this.controller.moveDirection;
			this.automaticMoveDirection.y = 0f;
			if (this.automaticMoveDirection.sqrMagnitude < 0.0001f)
			{
				this.automaticMoveDirection = base.transform.forward;
				this.automaticMoveDirection.y = 0f;
			}
			this.automaticMoveDirection.Normalize();
		}

		// Token: 0x0600334A RID: 13130 RVA: 0x001231C8 File Offset: 0x001213C8
		private void AcquireMoveDirectionLock()
		{
			if (this.ownsMoveDirectionLock || this.inputController == null)
			{
				return;
			}
			this.previousMoveDirectionLock = this.inputController.lockUpdateMoveDirection;
			this.inputController.lockUpdateMoveDirection = true;
			this.ownsMoveDirectionLock = true;
		}

		// Token: 0x0600334B RID: 13131 RVA: 0x00123205 File Offset: 0x00121405
		private void ReleaseMoveDirectionLock()
		{
			if (!this.ownsMoveDirectionLock || this.inputController == null)
			{
				return;
			}
			this.inputController.lockUpdateMoveDirection = this.previousMoveDirectionLock;
			this.ownsMoveDirectionLock = false;
		}

		// Token: 0x040033D5 RID: 13269
		[Header("Inputs")]
		public GenericInput autoMoveInput = new GenericInput("CapsLock", "", "");

		// Token: 0x040033D6 RID: 13270
		public GenericInput speedAdjustmentInput = new GenericInput("Mouse ScrollWheel", true, "", false, "", false);

		// Token: 0x040033D7 RID: 13271
		[Header("Speed Tiers")]
		[Tooltip("Velocity multiplier for the lower speed tier")]
		[Min(0.1f)]
		public float lowerSpeedMultiplier = 0.8f;

		// Token: 0x040033D8 RID: 13272
		[Tooltip("Velocity multiplier for the normal speed tier")]
		[Min(0.1f)]
		public float normalSpeedMultiplier = 1f;

		// Token: 0x040033D9 RID: 13273
		[Tooltip("Velocity multiplier for the faster speed tier")]
		[Min(0.1f)]
		public float fasterSpeedMultiplier = 1.2f;

		// Token: 0x040033DA RID: 13274
		[Header("Conflict Handling")]
		[Tooltip("Any manual movement-axis input cancels automatic locomotion")]
		[FormerlySerializedAs("backwardCancelThreshold")]
		[Range(0.01f, 1f)]
		public float manualMovementCancelThreshold = 0.1f;

		// Token: 0x040033DB RID: 13275
		private vThirdPersonController controller;

		// Token: 0x040033DC RID: 13276
		private vThirdPersonInput inputController;

		// Token: 0x040033DD RID: 13277
		private CustomStaminaController staminaController;

		// Token: 0x040033DE RID: 13278
		private AutoLocomotionHUD locomotionHud;

		// Token: 0x040033DF RID: 13279
		private AutoLocomotionController.AutoLocomotionMode activeMode;

		// Token: 0x040033E0 RID: 13280
		private int walkSpeedTier = 1;

		// Token: 0x040033E1 RID: 13281
		private int runSpeedTier = 1;

		// Token: 0x040033E2 RID: 13282
		private bool modifierPressActive;

		// Token: 0x040033E3 RID: 13283
		private bool modifierUsedForSpeed;

		// Token: 0x040033E4 RID: 13284
		private bool runModifierOnPress;

		// Token: 0x040033E5 RID: 13285
		private bool activationEligibleOnPress;

		// Token: 0x040033E6 RID: 13286
		private bool manualCancelArmed = true;

		// Token: 0x040033E7 RID: 13287
		private bool ownsCameraZoomSuppression;

		// Token: 0x040033E8 RID: 13288
		private bool previousCameraZoomSuppression;

		// Token: 0x040033E9 RID: 13289
		private bool ownsMoveDirectionLock;

		// Token: 0x040033EA RID: 13290
		private bool previousMoveDirectionLock;

		// Token: 0x040033EB RID: 13291
		private Vector3 automaticMoveDirection;

		// Token: 0x040033EC RID: 13292
		private bool autoRunWaitingForFullStamina;

		// Token: 0x02000797 RID: 1943
		public enum AutoLocomotionMode
		{
			// Token: 0x040033EE RID: 13294
			Off,
			// Token: 0x040033EF RID: 13295
			Walk,
			// Token: 0x040033F0 RID: 13296
			Run
		}
	}
}
