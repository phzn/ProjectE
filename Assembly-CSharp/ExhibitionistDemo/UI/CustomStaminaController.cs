using System;
using Features.Inventory.Wearable;
using Features.RunModes;
using Features.WardrobeCombos;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using Invector.vCover;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Minimap;
using UnityEngine;
using UnityEngine.Events;

namespace ExhibitionistDemo.UI
{
	// Token: 0x02000790 RID: 1936
	[DefaultExecutionOrder(-100)]
	[RequireComponent(typeof(vThirdPersonController))]
	public class CustomStaminaController : MonoBehaviour
	{
		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x060032D0 RID: 13008 RVA: 0x0012057E File Offset: 0x0011E77E
		private static bool UnlimitedStamina
		{
			get
			{
				return RunModeService.UnlimitedStamina;
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x060032D1 RID: 13009 RVA: 0x00120588 File Offset: 0x0011E788
		public bool CanRun
		{
			get
			{
				return CustomStaminaController.UnlimitedStamina || !this.useCustomStaminaConsumption || !this.forceWalkOnNoStamina || (!this.runBlocked && this.controller != null && this.controller.currentStamina > 0f);
			}
		}

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x060032D2 RID: 13010 RVA: 0x001205D8 File Offset: 0x0011E7D8
		public bool CanSprint
		{
			get
			{
				return CustomStaminaController.UnlimitedStamina || !this.useCustomStaminaConsumption || !this.forceWalkOnNoStamina || (!this.sprintBlocked && this.controller != null && this.controller.currentStamina > 0f);
			}
		}

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x060032D3 RID: 13011 RVA: 0x00120628 File Offset: 0x0011E828
		public float StaminaNormalized
		{
			get
			{
				if (!CustomStaminaController.UnlimitedStamina && !(this.controller == null) && this.controller.maxStamina > 0f)
				{
					return Mathf.Clamp01(this.controller.currentStamina / this.controller.maxStamina);
				}
				return 1f;
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x060032D4 RID: 13012 RVA: 0x00120680 File Offset: 0x0011E880
		public bool IsStaminaFull
		{
			get
			{
				return CustomStaminaController.UnlimitedStamina || this.controller == null || this.controller.maxStamina <= 0f || this.controller.currentStamina >= this.controller.maxStamina - 0.01f;
			}
		}

		// Token: 0x060032D5 RID: 13013 RVA: 0x001206D7 File Offset: 0x0011E8D7
		private void Awake()
		{
			this.CacheController();
		}

		// Token: 0x060032D6 RID: 13014 RVA: 0x001206DF File Offset: 0x0011E8DF
		private void OnEnable()
		{
			this.CacheController();
			this.RegisterActionEvents();
			this.ApplyStaminaAuthority();
		}

		// Token: 0x060032D7 RID: 13015 RVA: 0x001206F3 File Offset: 0x0011E8F3
		private void OnDisable()
		{
			this.UnregisterActionEvents();
			this.RestoreBuiltInStamina();
		}

		// Token: 0x060032D8 RID: 13016 RVA: 0x00120704 File Offset: 0x0011E904
		private void CacheController()
		{
			if (this.controller == null)
			{
				this.controller = base.GetComponent<vThirdPersonController>();
			}
			if (this.controller == null)
			{
				Debug.LogError("CustomStaminaController: vThirdPersonController not found!", this);
				base.enabled = false;
				return;
			}
			this.inputController = base.GetComponent<vThirdPersonInput>();
			this.genericAction = base.GetComponent<vGenericAction>();
			this.coverController = base.GetComponent<vBasicCoverController>();
			this.wearables = base.GetComponent<WearableUseController>();
			if (!this.cachedOriginalCosts)
			{
				this.originalJumpStamina = this.controller.jumpStamina;
				this.originalRollStamina = this.controller.rollStamina;
				this.cachedOriginalCosts = true;
			}
		}

		// Token: 0x060032D9 RID: 13017 RVA: 0x001207B0 File Offset: 0x0011E9B0
		private void ApplyStaminaAuthority()
		{
			if (this.controller == null)
			{
				return;
			}
			this.unlimitedStaminaWasActive = CustomStaminaController.UnlimitedStamina;
			this.controller.ignoreStaminaConsumption = this.unlimitedStaminaWasActive;
			this.controller.useBuiltInStamina = !this.useCustomStaminaConsumption && !this.unlimitedStaminaWasActive;
			if (this.unlimitedStaminaWasActive)
			{
				this.RefreshActionCosts();
				this.RefillUnlimitedStamina();
				return;
			}
			if (this.useCustomStaminaConsumption)
			{
				this.RefreshActionCosts();
				this.recoveryDelayRemaining = Mathf.Max(this.recoveryDelayRemaining, this.controller.currentStaminaRecoveryDelay);
				return;
			}
			this.RestoreOriginalActionCosts();
		}

		// Token: 0x060032DA RID: 13018 RVA: 0x0012084D File Offset: 0x0011EA4D
		private void RestoreBuiltInStamina()
		{
			if (this.controller == null)
			{
				return;
			}
			this.controller.useBuiltInStamina = true;
			this.controller.ignoreStaminaConsumption = false;
			this.unlimitedStaminaWasActive = false;
			this.RestoreOriginalActionCosts();
		}

		// Token: 0x060032DB RID: 13019 RVA: 0x00120883 File Offset: 0x0011EA83
		private void RestoreOriginalActionCosts()
		{
			if (!this.cachedOriginalCosts)
			{
				return;
			}
			this.controller.jumpStamina = this.originalJumpStamina;
			this.controller.rollStamina = this.originalRollStamina;
		}

		// Token: 0x060032DC RID: 13020 RVA: 0x001208B0 File Offset: 0x0011EAB0
		private void FixedUpdate()
		{
			if (this.controller == null)
			{
				return;
			}
			if (this.unlimitedStaminaWasActive != CustomStaminaController.UnlimitedStamina)
			{
				this.ApplyStaminaAuthority();
			}
			if (CustomStaminaController.UnlimitedStamina)
			{
				this.RefreshActionCosts();
				this.RefillUnlimitedStamina();
				return;
			}
			if (!this.useCustomStaminaConsumption)
			{
				return;
			}
			this.RefreshActionCosts();
			if (WardrobeComboRuntime.HasAthletic)
			{
				this.controller.currentStamina = this.controller.maxStamina;
				this.controller.currentStaminaRecoveryDelay = 0f;
				this.recoveryDelayRemaining = 0f;
				this.ClearExhaustion();
				return;
			}
			float fixedDeltaTime = Time.fixedDeltaTime;
			this.UpdateCooldownTimers(fixedDeltaTime);
			this.UpdateBlockedStates();
			bool flag = this.HandleMovementStamina(fixedDeltaTime);
			if (this.controller.currentStamina <= 0f && this.forceWalkOnNoStamina)
			{
				this.BeginExhaustion();
			}
			this.UpdateBlockedStates();
			this.HandleStaminaRecovery(flag, fixedDeltaTime);
			this.UpdateBlockedStates();
		}

		// Token: 0x060032DD RID: 13021 RVA: 0x00120990 File Offset: 0x0011EB90
		private void UpdateCooldownTimers(float deltaTime)
		{
			this.runCooldownTimer = Mathf.Max(0f, this.runCooldownTimer - deltaTime);
			this.sprintCooldownTimer = Mathf.Max(0f, this.sprintCooldownTimer - deltaTime);
		}

		// Token: 0x060032DE RID: 13022 RVA: 0x001209C4 File Offset: 0x0011EBC4
		private void UpdateBlockedStates()
		{
			if (this.runBlocked && this.runCooldownTimer <= 0f && this.controller.currentStamina >= Mathf.Min(this.staminaReactivationThreshold, this.controller.maxStamina))
			{
				this.runBlocked = false;
			}
			if (this.sprintBlocked && this.sprintCooldownTimer <= 0f && this.controller.currentStamina >= Mathf.Min(this.sprintReactivationThreshold, this.controller.maxStamina))
			{
				this.sprintBlocked = false;
			}
			if (this.runBlocked)
			{
				this.controller.isRunning = false;
			}
			if (this.sprintBlocked)
			{
				this.controller.isSprinting = false;
			}
		}

		// Token: 0x060032DF RID: 13023 RVA: 0x00120A78 File Offset: 0x0011EC78
		private bool HandleMovementStamina(float deltaTime)
		{
			if (!this.CanConsumeLocomotionStamina())
			{
				return false;
			}
			if (!this.controller.isGrounded || this.controller.input.sqrMagnitude <= 0.01f)
			{
				return false;
			}
			if (this.consumeOnSprint && this.controller.isSprinting && !this.sprintBlocked)
			{
				return this.ConsumeStamina(this.sprintStaminaCost * deltaTime);
			}
			if (this.consumeOnRun && this.controller.isRunning && !this.controller.isSprinting && !this.runBlocked)
			{
				return this.ConsumeStamina(this.runStaminaCost * deltaTime);
			}
			return this.consumeOnWalk && !this.controller.isRunning && !this.controller.isSprinting && this.ConsumeStamina(this.walkStaminaCost * deltaTime);
		}

		// Token: 0x060032E0 RID: 13024 RVA: 0x00120B54 File Offset: 0x0011ED54
		private bool CanConsumeLocomotionStamina()
		{
			if (!this.controller.enabled || this.controller.isDead || this.controller.ragdolled || this.controller.isCrouching || this.controller.customAction || this.controller.isRolling || this.controller.isJumping || this.controller.inJumpStarted || this.controller.isSliding || this.controller.lockMovement || this.controller.lockAnimMovement)
			{
				return false;
			}
			if (this.inputController != null && (!this.inputController.enabled || this.inputController.lockInput || this.inputController.lockMoveInput))
			{
				return false;
			}
			if (this.genericAction != null && this.genericAction.doingAction)
			{
				return false;
			}
			if (this.coverController != null && (this.coverController.inCover || this.coverController.goingToCoverPoint))
			{
				return false;
			}
			if (GameplayInputGate.BlocksGameplayInput || PlayerActivityGate.IsResourceOwned(PlayerContextResource.PlayerControl | PlayerContextResource.Rider))
			{
				return false;
			}
			if (!PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI))
			{
				return true;
			}
			ProceduralMiniMapController resourceOwner = PlayerActivityGate.GetResourceOwner<ProceduralMiniMapController>(PlayerContextResource.ModalUI);
			return resourceOwner != null && resourceOwner.IsExpanded;
		}

		// Token: 0x060032E1 RID: 13025 RVA: 0x00120CAC File Offset: 0x0011EEAC
		private void RegisterActionEvents()
		{
			if (this.controller == null)
			{
				return;
			}
			this.controller.OnJump.RemoveListener(new UnityAction(this.HandleJumpAction));
			this.controller.OnRoll.RemoveListener(new UnityAction(this.HandleRollAction));
			this.controller.OnJump.AddListener(new UnityAction(this.HandleJumpAction));
			this.controller.OnRoll.AddListener(new UnityAction(this.HandleRollAction));
			if (this.wearables != null)
			{
				this.wearables.OnItemWorn -= this.HandleEquipmentChanged;
				this.wearables.OnItemUnworn -= this.HandleEquipmentChanged;
				this.wearables.OnItemWorn += this.HandleEquipmentChanged;
				this.wearables.OnItemUnworn += this.HandleEquipmentChanged;
			}
		}

		// Token: 0x060032E2 RID: 13026 RVA: 0x00120DA4 File Offset: 0x0011EFA4
		private void UnregisterActionEvents()
		{
			if (this.controller == null)
			{
				return;
			}
			this.controller.OnJump.RemoveListener(new UnityAction(this.HandleJumpAction));
			this.controller.OnRoll.RemoveListener(new UnityAction(this.HandleRollAction));
			if (this.wearables != null)
			{
				this.wearables.OnItemWorn -= this.HandleEquipmentChanged;
				this.wearables.OnItemUnworn -= this.HandleEquipmentChanged;
			}
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x060032E3 RID: 13027 RVA: 0x00120E34 File Offset: 0x0011F034
		private float EquippedStaminaCostMultiplier
		{
			get
			{
				if (!(this.wearables != null) || !this.wearables.isActiveAndEnabled)
				{
					return 1f;
				}
				return this.wearables.WornStaminaCostMultiplier;
			}
		}

		// Token: 0x060032E4 RID: 13028 RVA: 0x00120E62 File Offset: 0x0011F062
		private void HandleEquipmentChanged(WearableItemSO item)
		{
			this.RefreshActionCosts();
		}

		// Token: 0x060032E5 RID: 13029 RVA: 0x00120E6C File Offset: 0x0011F06C
		private void RefreshActionCosts()
		{
			if (this.controller == null)
			{
				return;
			}
			if (CustomStaminaController.UnlimitedStamina)
			{
				this.controller.jumpStamina = 0f;
				this.controller.rollStamina = 0f;
				return;
			}
			if (!this.useCustomStaminaConsumption)
			{
				return;
			}
			float equippedStaminaCostMultiplier = this.EquippedStaminaCostMultiplier;
			this.controller.jumpStamina = (this.consumeOnJump ? (Mathf.Max(0f, this.jumpStaminaCost) * equippedStaminaCostMultiplier) : 0f);
			this.controller.rollStamina = (this.consumeOnRoll ? (Mathf.Max(0f, this.rollStaminaCost) * equippedStaminaCostMultiplier) : 0f);
		}

		// Token: 0x060032E6 RID: 13030 RVA: 0x00120F18 File Offset: 0x0011F118
		private void HandleJumpAction()
		{
			if (this.useCustomStaminaConsumption && this.consumeOnJump && this.ConsumeStamina(this.jumpStaminaCost))
			{
				this.OnActionStaminaConsumed();
			}
		}

		// Token: 0x060032E7 RID: 13031 RVA: 0x00120F3E File Offset: 0x0011F13E
		private void HandleRollAction()
		{
			if (this.useCustomStaminaConsumption && this.consumeOnRoll && this.ConsumeStamina(this.rollStaminaCost))
			{
				this.OnActionStaminaConsumed();
			}
		}

		// Token: 0x060032E8 RID: 13032 RVA: 0x00120F64 File Offset: 0x0011F164
		private void OnActionStaminaConsumed()
		{
			this.recoveryDelayRemaining = Mathf.Max(0f, this.staminaRecoveryDelay);
			this.controller.currentStaminaRecoveryDelay = this.recoveryDelayRemaining;
			if (this.controller.currentStamina <= 0f && this.forceWalkOnNoStamina)
			{
				this.BeginExhaustion();
			}
		}

		// Token: 0x060032E9 RID: 13033 RVA: 0x00120FB8 File Offset: 0x0011F1B8
		private void HandleStaminaRecovery(bool consumedThisStep, float deltaTime)
		{
			if (consumedThisStep)
			{
				this.recoveryDelayRemaining = Mathf.Max(0f, this.staminaRecoveryDelay);
			}
			else if (this.recoveryDelayRemaining > 0f)
			{
				this.recoveryDelayRemaining = Mathf.Max(0f, this.recoveryDelayRemaining - deltaTime);
			}
			else if (this.controller.currentStamina < this.controller.maxStamina)
			{
				float num = (this.recoveringFromExhaustion ? Mathf.Max(1f, this.exhaustedStaminaRecoveryMultiplier) : 1f);
				this.controller.currentStamina = Mathf.Min(this.controller.maxStamina, this.controller.currentStamina + Mathf.Max(0f, this.staminaRecoveryRate) * num * deltaTime);
				if (this.controller.currentStamina >= this.controller.maxStamina - 0.01f)
				{
					this.recoveringFromExhaustion = false;
				}
			}
			this.controller.currentStaminaRecoveryDelay = this.recoveryDelayRemaining;
		}

		// Token: 0x060032EA RID: 13034 RVA: 0x001210BC File Offset: 0x0011F2BC
		private bool ConsumeStamina(float amount)
		{
			if (CustomStaminaController.UnlimitedStamina)
			{
				this.RefillUnlimitedStamina();
				return false;
			}
			amount = Mathf.Max(0f, amount);
			amount *= this.EquippedStaminaCostMultiplier;
			if (amount <= 0f)
			{
				return false;
			}
			float currentStamina = this.controller.currentStamina;
			this.controller.currentStamina = Mathf.Clamp(currentStamina - amount, 0f, this.controller.maxStamina);
			if (currentStamina > 0f && this.controller.currentStamina <= 0f)
			{
				this.controller.OnStaminaEnd.Invoke();
			}
			return true;
		}

		// Token: 0x060032EB RID: 13035 RVA: 0x00121154 File Offset: 0x0011F354
		private void BeginExhaustion()
		{
			bool flag = !this.sprintBlocked && this.controller.isSprinting;
			this.recoveringFromExhaustion = true;
			if (!this.runBlocked)
			{
				this.runBlocked = true;
				this.runCooldownTimer = Mathf.Max(0f, this.runCooldownTime);
			}
			if (!this.sprintBlocked)
			{
				this.sprintBlocked = true;
				this.sprintCooldownTimer = Mathf.Max(0f, this.sprintCooldownTime);
			}
			this.controller.isRunning = false;
			this.controller.isSprinting = false;
			if (flag)
			{
				this.controller.OnFinishSprintingByStamina.Invoke();
				this.controller.OnFinishSprinting.Invoke();
			}
		}

		// Token: 0x060032EC RID: 13036 RVA: 0x00121202 File Offset: 0x0011F402
		private void ClearExhaustion()
		{
			this.runBlocked = false;
			this.sprintBlocked = false;
			this.runCooldownTimer = 0f;
			this.sprintCooldownTimer = 0f;
			this.recoveringFromExhaustion = false;
		}

		// Token: 0x060032ED RID: 13037 RVA: 0x00121230 File Offset: 0x0011F430
		private void RefillUnlimitedStamina()
		{
			this.controller.currentStamina = this.controller.maxStamina;
			this.controller.currentStaminaRecoveryDelay = 0f;
			this.controller.finishStaminaOnSprint = false;
			this.recoveryDelayRemaining = 0f;
			this.ClearExhaustion();
		}

		// Token: 0x060032EE RID: 13038 RVA: 0x00121280 File Offset: 0x0011F480
		public void SetCustomStaminaEnabled(bool value)
		{
			this.useCustomStaminaConsumption = value;
			this.ClearExhaustion();
			this.ApplyStaminaAuthority();
		}

		// Token: 0x060032EF RID: 13039 RVA: 0x00121295 File Offset: 0x0011F495
		public void SetRunStaminaCost(float cost)
		{
			this.runStaminaCost = Mathf.Max(0f, cost);
		}

		// Token: 0x060032F0 RID: 13040 RVA: 0x001212A8 File Offset: 0x0011F4A8
		public void SetSprintStaminaCost(float cost)
		{
			this.sprintStaminaCost = Mathf.Max(0f, cost);
		}

		// Token: 0x04003373 RID: 13171
		[Header("Stamina Consumption Settings")]
		[Tooltip("Enable custom stamina consumption (overrides Invector's default sprint-only behavior)")]
		public bool useCustomStaminaConsumption = true;

		// Token: 0x04003374 RID: 13172
		[Header("Movement Stamina Consumption")]
		[Tooltip("Consume stamina while walking")]
		public bool consumeOnWalk;

		// Token: 0x04003375 RID: 13173
		[Tooltip("Stamina consumed per second while walking")]
		public float walkStaminaCost = 5f;

		// Token: 0x04003376 RID: 13174
		[Tooltip("Consume stamina while running")]
		public bool consumeOnRun = true;

		// Token: 0x04003377 RID: 13175
		[Tooltip("Stamina consumed per second while running")]
		public float runStaminaCost = 15f;

		// Token: 0x04003378 RID: 13176
		[Tooltip("Consume stamina while sprinting")]
		public bool consumeOnSprint = true;

		// Token: 0x04003379 RID: 13177
		[Tooltip("Stamina consumed per second while sprinting")]
		public float sprintStaminaCost = 30f;

		// Token: 0x0400337A RID: 13178
		[Header("Action Stamina Consumption")]
		[Tooltip("Consume stamina on jump")]
		public bool consumeOnJump = true;

		// Token: 0x0400337B RID: 13179
		[Tooltip("Stamina consumed per jump")]
		public float jumpStaminaCost = 30f;

		// Token: 0x0400337C RID: 13180
		[Tooltip("Consume stamina on roll")]
		public bool consumeOnRoll = true;

		// Token: 0x0400337D RID: 13181
		[Tooltip("Stamina consumed per roll")]
		public float rollStaminaCost = 25f;

		// Token: 0x0400337E RID: 13182
		[Header("Recovery Settings")]
		[Tooltip("Delay before stamina starts recovering after consumption (seconds)")]
		public float staminaRecoveryDelay = 1f;

		// Token: 0x0400337F RID: 13183
		[Tooltip("Stamina recovery rate per second")]
		public float staminaRecoveryRate = 20f;

		// Token: 0x04003380 RID: 13184
		[Min(1f)]
		public float exhaustedStaminaRecoveryMultiplier = 2f;

		// Token: 0x04003381 RID: 13185
		[Header("Movement Control")]
		[Tooltip("Force walk when stamina runs out during run")]
		public bool forceWalkOnNoStamina = true;

		// Token: 0x04003382 RID: 13186
		[Tooltip("Stamina must recharge to this amount before allowing run again (prevents loop)")]
		public float staminaReactivationThreshold = 30f;

		// Token: 0x04003383 RID: 13187
		[Tooltip("Stamina must recharge to this amount before allowing sprint again")]
		public float sprintReactivationThreshold = 50f;

		// Token: 0x04003384 RID: 13188
		[Tooltip("Cooldown time after stamina depletion before can run again (seconds)")]
		public float runCooldownTime = 1f;

		// Token: 0x04003385 RID: 13189
		[Tooltip("Cooldown time after stamina depletion before can sprint again (seconds)")]
		public float sprintCooldownTime = 1.5f;

		// Token: 0x04003386 RID: 13190
		private vThirdPersonController controller;

		// Token: 0x04003387 RID: 13191
		private vThirdPersonInput inputController;

		// Token: 0x04003388 RID: 13192
		private vGenericAction genericAction;

		// Token: 0x04003389 RID: 13193
		private vBasicCoverController coverController;

		// Token: 0x0400338A RID: 13194
		private WearableUseController wearables;

		// Token: 0x0400338B RID: 13195
		private bool runBlocked;

		// Token: 0x0400338C RID: 13196
		private bool sprintBlocked;

		// Token: 0x0400338D RID: 13197
		private float runCooldownTimer;

		// Token: 0x0400338E RID: 13198
		private float sprintCooldownTimer;

		// Token: 0x0400338F RID: 13199
		private float recoveryDelayRemaining;

		// Token: 0x04003390 RID: 13200
		private float originalJumpStamina;

		// Token: 0x04003391 RID: 13201
		private float originalRollStamina;

		// Token: 0x04003392 RID: 13202
		private bool cachedOriginalCosts;

		// Token: 0x04003393 RID: 13203
		private bool recoveringFromExhaustion;

		// Token: 0x04003394 RID: 13204
		private bool unlimitedStaminaWasActive;
	}
}
