using System;
using System.Collections.Generic;
using BoZo.CharacterCreator;
using Features.CielOS;
using Features.Detection;
using Features.Inventory;
using Features.MainMenu;
using Features.PlayerEmotes;
using Features.RapidActions;
using Invector.vCharacterController;
using PoolWaterTools;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using UnityEngine;

namespace Features.InsertableToys
{
	// Token: 0x02001173 RID: 4467
	[DefaultExecutionOrder(1000)]
	[DisallowMultipleComponent]
	public sealed class InsertableToyController : MonoBehaviour
	{
		// Token: 0x1700159F RID: 5535
		// (get) Token: 0x0600883B RID: 34875 RVA: 0x0037BF4A File Offset: 0x0037A14A
		public bool IsInserted
		{
			get
			{
				return this._insertedItem != null;
			}
		}

		// Token: 0x170015A0 RID: 5536
		// (get) Token: 0x0600883C RID: 34876 RVA: 0x0037BF58 File Offset: 0x0037A158
		public InsertableToyItemSO InsertedItem
		{
			get
			{
				return this._insertedItem;
			}
		}

		// Token: 0x170015A1 RID: 5537
		// (get) Token: 0x0600883D RID: 34877 RVA: 0x0037BF60 File Offset: 0x0037A160
		public InsertionSite ActiveInsertionSite
		{
			get
			{
				return this._insertedSite;
			}
		}

		// Token: 0x170015A2 RID: 5538
		// (get) Token: 0x0600883E RID: 34878 RVA: 0x0037BF68 File Offset: 0x0037A168
		private bool WaterOwnsMovement
		{
			get
			{
				return PlayerActivityGate.IsResourceOwned(PlayerContextResource.WaterMovement) || PoolWaterEntryPreparation.IsSwimming(base.transform);
			}
		}

		// Token: 0x170015A3 RID: 5539
		// (get) Token: 0x0600883F RID: 34879 RVA: 0x0037BF84 File Offset: 0x0037A184
		public bool ReservesControllerCoverInput
		{
			get
			{
				ControllerButton controllerButton;
				bool flag;
				return base.isActiveAndEnabled && this.IsInserted && !this.WaterOwnsMovement && InsertableToyController.MayPresentActions() && ControllerBindings.TryButton(this._insertedItem.RemoveInputKey, out controllerButton, out flag) && controllerButton == ControllerButton.LeftShoulder && !flag;
			}
		}

		// Token: 0x06008840 RID: 34880 RVA: 0x0037BFD0 File Offset: 0x0037A1D0
		private void Awake()
		{
			this._blendShapeSourceId = string.Format("InsertableToy.{0}", base.GetInstanceID());
			this._actionAudioRandomState ^= (uint)base.GetInstanceID();
			this.ResolveDependencies();
		}

		// Token: 0x06008841 RID: 34881 RVA: 0x0037C006 File Offset: 0x0037A206
		private void OnEnable()
		{
			this.ResolveDependencies();
			this.HookInventoryEvents();
		}

		// Token: 0x06008842 RID: 34882 RVA: 0x0037C014 File Offset: 0x0037A214
		private void Update()
		{
			this.TryCompletePendingRunningSpeedRestore();
			this.ResolveDependencies();
			this.HookInventoryEvents();
			ItemUsageSignal.Set(this, this.IsInserted);
			if (this._inventory == null)
			{
				if (this.IsInserted)
				{
					this.CleanupInsertion(false);
				}
				this.ReleaseActivity();
				this.MaintainHud(null, false);
				return;
			}
			if (this.IsInserted)
			{
				if (!this.TryAcquireInsertedActivity(false))
				{
					this.CleanupInsertion(false);
					this.MaintainHud(this.GetHeldToy(), false);
					return;
				}
				if (!this.InsertedSlotStillOwnsItem())
				{
					this.CleanupInsertion(false);
					this.MaintainHud(this.GetHeldToy(), false);
					return;
				}
				this.MaintainInsertedRunningSpeed();
				this.SetInsertedAnimatorState(true);
				this.MaintainHud(this._insertedItem, true);
				if ((!(this._rapidActions != null) || !this._rapidActions.IsRebindOpen) && this.CanReadOurHudInput() && this._removeInput != null && this._removeInput.GetButtonDown())
				{
					if (ControllerInput.IsActive)
					{
						ControllerInput.ConsumeUIInput();
					}
					this.RemoveInsertedToy();
				}
				return;
			}
			else
			{
				InsertableToyItemSO heldToy = this.GetHeldToy();
				this.ConfigureHeldVisual(heldToy);
				this.MaintainHud(heldToy, false);
				if (heldToy == null || !this.CanReadOurHudInput())
				{
					this.ReleaseActivity();
					return;
				}
				if (!this.TryAcquireHeldActionActivity())
				{
					return;
				}
				if (!base.isActiveAndEnabled || !this.CanReadOurHudInput())
				{
					this.ReleaseActivity();
					return;
				}
				if (InsertableToyController.IsUsable(heldToy.AnalPlacement) && this._analInput != null && this._analInput.GetButtonDown())
				{
					this.TryInsert(InsertionSite.Anal);
					return;
				}
				if (InsertableToyController.IsUsable(heldToy.VaginalPlacement) && this._vaginalInput != null && this._vaginalInput.GetButtonDown())
				{
					this.TryInsert(InsertionSite.Vaginal);
				}
				return;
			}
		}

		// Token: 0x06008843 RID: 34883 RVA: 0x0037C1C0 File Offset: 0x0037A3C0
		private void LateUpdate()
		{
			if (this.IsInserted)
			{
				this.MaintainFaceOverride();
				string text = ((this._insertedPlacement != null) ? this._insertedPlacement.BlendShapeName : null);
				if (this._blendShapeManager != null && (string.IsNullOrWhiteSpace(text) || !this._blendShapeManager.HasBlendShape(text)))
				{
					this._blendShapeManager.RemoveAllOverridesFromSource(this._blendShapeSourceId);
					if (this._registeredBlendShapeManager == this._blendShapeManager)
					{
						this._registeredBlendShapeManager = null;
					}
					this._blendShapeManager = null;
				}
				if (this._blendShapeManager == null)
				{
					BlendShapeManager blendShapeManager = BlendShapeManager.ResolveForRig((this._animator != null) ? this._animator : this);
					if (blendShapeManager != null && !string.IsNullOrWhiteSpace(text) && blendShapeManager.HasBlendShape(text))
					{
						this._blendShapeManager = blendShapeManager;
						this.ApplyBlendShapeOverride();
					}
					else
					{
						if (!this.HasUsableFallbackBinding() && Time.unscaledTime >= this._nextBlendShapeRebindTime)
						{
							this._nextBlendShapeRebindTime = Time.unscaledTime + 1f;
							this.PrepareBlendShapeOverride(false);
						}
						if (this._blendShapeManager != null)
						{
							this.ApplyBlendShapeOverride();
						}
						else
						{
							this.ApplyFallbackBlendShape();
						}
					}
				}
				this.SetInsertedAnimatorState(true);
				this.MaintainHud(this._insertedItem, true);
				return;
			}
			this.MaintainHud(this.GetHeldToy(), false);
		}

		// Token: 0x06008844 RID: 34884 RVA: 0x0037C310 File Offset: 0x0037A510
		public bool TryInsert(InsertionSite site)
		{
			if (this.IsInserted || this._inventory == null || this.WaterOwnsMovement || !InsertableToyController.MayPresentActions())
			{
				return false;
			}
			InsertableToyItemSO heldToy = this.GetHeldToy();
			if (heldToy == null)
			{
				return false;
			}
			InsertableToyPlacement placement = heldToy.GetPlacement(site);
			if (!InsertableToyController.IsUsable(placement))
			{
				return false;
			}
			if (!UncoveredBottomRequirement.TryAllow())
			{
				return false;
			}
			this.ResolveDependencies();
			Transform transform = this.ResolveSocket(placement);
			if (transform == null)
			{
				Debug.LogWarning("[InsertableToyController] Socket '" + placement.SocketTransformName + "' / " + string.Format("bone '{0}' not found for '{1}'. Insertion cancelled.", placement.SocketBone, heldToy.itemName), this);
				return false;
			}
			GameObject insertedPrefab = heldToy.InsertedPrefab;
			if (insertedPrefab == null)
			{
				Debug.LogWarning("[InsertableToyController] '" + heldToy.itemName + "' has no inserted or hand prefab. Insertion cancelled.", this);
				return false;
			}
			int activeSlotIndex = this._inventory.ActiveSlotIndex;
			if (activeSlotIndex < 0 || this._inventory.GetItemAt(activeSlotIndex) != heldToy)
			{
				return false;
			}
			if (!this.TryAcquireInsertedActivity(true))
			{
				return false;
			}
			if (!base.isActiveAndEnabled)
			{
				this.ReleaseActivity();
				return false;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(insertedPrefab, transform, false);
			gameObject.name = string.Format("{0}_Inserted_{1}", insertedPrefab.name, site);
			gameObject.transform.localPosition = placement.LocalPosition;
			gameObject.transform.localRotation = Quaternion.Euler(placement.LocalEulerAngles);
			gameObject.transform.localScale = Vector3.Scale(insertedPrefab.transform.localScale, placement.LocalScale);
			InsertableToyController.ConfigureAsNonPhysicalProp(gameObject);
			this._insertedItem = heldToy;
			this._insertedPlacement = placement;
			this._insertedSite = site;
			this._insertedSlot = activeSlotIndex;
			this._insertedVisual = gameObject;
			ItemUsageSignal.Set(this, true);
			this.ApplyInsertedRunningSpeed(heldToy);
			this._slotLockOwned = this._inventory.AcquireSlotInputLock(this);
			this._inventory.SuspendActiveItemImmediate();
			this._configuredHandVisual = null;
			this.PrepareBlendShapeOverride(true);
			this.ApplyBlendShapeOverride();
			this.AcquireEmotionOverrides();
			this.MaintainFaceOverride();
			this.SetInsertedAnimatorState(true);
			this.InvalidateHud();
			this.MaintainHud(this._insertedItem, true);
			this.PlayActionSfx(heldToy.InsertionSfx, heldToy.InsertionSfxVolume, heldToy.InsertionSfxPitchMin, heldToy.InsertionSfxPitchMax);
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[InsertableToyController] Inserted '{0}' ({1}).", heldToy.itemName, site), this);
			}
			return true;
		}

		// Token: 0x06008845 RID: 34885 RVA: 0x0037C578 File Offset: 0x0037A778
		public bool RemoveInsertedToy()
		{
			if (!this.IsInserted || this.WaterOwnsMovement)
			{
				return false;
			}
			if (PlayerActivityGate.IsResourceOwned(PlayerContextResource.FullBodyAnimation))
			{
				return false;
			}
			InsertableToyItemSO insertedItem = this._insertedItem;
			if (!this.CleanupInsertion(true))
			{
				return false;
			}
			this.MaintainHud(this.GetHeldToy(), false);
			this.PlayActionSfx(insertedItem.RemovalSfx, insertedItem.RemovalSfxVolume, insertedItem.RemovalSfxPitchMin, insertedItem.RemovalSfxPitchMax);
			return true;
		}

		// Token: 0x06008846 RID: 34886 RVA: 0x0037C5E0 File Offset: 0x0037A7E0
		public void PrepareForWaterEntry()
		{
			if (this.IsInserted)
			{
				this.TryAcquireInsertedActivity(false);
				PlayerInventoryManager inventory = this._inventory;
				if (inventory != null)
				{
					inventory.SuspendActiveItemImmediate();
				}
				this._configuredHandVisual = null;
				this.SetInsertedAnimatorState(false);
			}
			else
			{
				this.ReleaseActivity();
			}
			if (this._runtimeHudConfig != null)
			{
				InteractionHUDController instance = InteractionHUDController.Instance;
				if (instance != null)
				{
					instance.HideIfCurrent(this._runtimeHudConfig);
				}
			}
			this._hudShownFrame = -1;
			this.InvalidateHud();
		}

		// Token: 0x06008847 RID: 34887 RVA: 0x0037C658 File Offset: 0x0037A858
		private void ResolveDependencies()
		{
			if (this._inventory == null)
			{
				this._inventory = base.GetComponent<PlayerInventoryManager>();
				if (this._inventory == null)
				{
					this._inventory = PlayerInventoryManager.Instance;
				}
			}
			if (this._rapidActions == null && (this.IsInserted || this.GetHeldToy() != null))
			{
				if (this._inventory != null)
				{
					this._rapidActions = this._inventory.GetComponentInChildren<RapidActionsHUDController>(true);
				}
				if (this._rapidActions == null)
				{
					this._rapidActions = Object.FindObjectOfType<RapidActionsHUDController>(true);
				}
			}
			Animator animator = ((this._inventory != null) ? this._inventory.PlayerAnimator : null);
			if (animator == null && this._inventory != null)
			{
				animator = this._inventory.GetComponentInChildren<Animator>(true);
			}
			if (this._animator != animator)
			{
				Animator animator2 = this._animator;
				if (this.IsInserted)
				{
					InsertableToyController.SetAnimatorBoolIfPresent(animator2, this.insertedAnimatorParameter, false);
					this.ReleaseBlendShapeOverride();
					this.ReleaseEmotionOverrides();
				}
				this._animator = animator;
				this._resolvedAnimatorController = null;
				this._hasInsertedAnimatorParameter = false;
				if (this.IsInserted)
				{
					this.RebindInsertionToCurrentAnimator();
				}
			}
		}

		// Token: 0x06008848 RID: 34888 RVA: 0x0037C790 File Offset: 0x0037A990
		private void RebindInsertionToCurrentAnimator()
		{
			if (!this.IsInserted)
			{
				return;
			}
			Transform transform = this.ResolveSocket(this._insertedPlacement);
			GameObject gameObject = ((this._insertedItem != null) ? this._insertedItem.InsertedPrefab : null);
			if (transform == null || gameObject == null)
			{
				this.CleanupInsertion(true);
				return;
			}
			if (this._insertedVisual == null)
			{
				this._insertedVisual = Object.Instantiate<GameObject>(gameObject, transform, false);
				this._insertedVisual.name = string.Format("{0}_Inserted_{1}", gameObject.name, this._insertedSite);
				InsertableToyController.ConfigureAsNonPhysicalProp(this._insertedVisual);
			}
			else
			{
				this._insertedVisual.transform.SetParent(transform, false);
			}
			this._insertedVisual.transform.localPosition = this._insertedPlacement.LocalPosition;
			this._insertedVisual.transform.localRotation = Quaternion.Euler(this._insertedPlacement.LocalEulerAngles);
			this._insertedVisual.transform.localScale = Vector3.Scale(gameObject.transform.localScale, this._insertedPlacement.LocalScale);
			this.PrepareBlendShapeOverride(true);
			this.ApplyBlendShapeOverride();
			this.AcquireEmotionOverrides();
			this.MaintainFaceOverride();
			this.SetInsertedAnimatorState(true);
		}

		// Token: 0x06008849 RID: 34889 RVA: 0x0037C8D4 File Offset: 0x0037AAD4
		private void HookInventoryEvents()
		{
			if (this._inventoryEventsHooked || this._inventory == null)
			{
				return;
			}
			this._inventory.OnItemRemoved += this.HandleItemRemoved;
			this._inventory.OnActiveSlotChanged += this.HandleActiveSlotChanged;
			this._inventoryEventsHooked = true;
		}

		// Token: 0x0600884A RID: 34890 RVA: 0x0037C930 File Offset: 0x0037AB30
		private void UnhookInventoryEvents()
		{
			if (!this._inventoryEventsHooked || this._inventory == null)
			{
				return;
			}
			this._inventory.OnItemRemoved -= this.HandleItemRemoved;
			this._inventory.OnActiveSlotChanged -= this.HandleActiveSlotChanged;
			this._inventoryEventsHooked = false;
		}

		// Token: 0x0600884B RID: 34891 RVA: 0x0037C989 File Offset: 0x0037AB89
		private void HandleItemRemoved(int slot)
		{
			if (this.IsInserted && slot == this._insertedSlot)
			{
				this.CleanupInsertion(false);
				return;
			}
			this.InvalidateHud();
		}

		// Token: 0x0600884C RID: 34892 RVA: 0x0037C9AC File Offset: 0x0037ABAC
		private void HandleActiveSlotChanged(int slot)
		{
			if (this.IsInserted)
			{
				if (this._inventory != null && slot >= 0 && this._inventory.GetItemAt(slot) == this._insertedItem)
				{
					this._insertedSlot = slot;
					this._inventory.SuspendActiveItemImmediate();
				}
				else
				{
					this.CleanupInsertion(false);
				}
			}
			this.InvalidateHud();
		}

		// Token: 0x0600884D RID: 34893 RVA: 0x0037CA11 File Offset: 0x0037AC11
		private InsertableToyItemSO GetHeldToy()
		{
			if (!(this._inventory != null))
			{
				return null;
			}
			return this._inventory.GetActiveItem() as InsertableToyItemSO;
		}

		// Token: 0x0600884E RID: 34894 RVA: 0x0037CA34 File Offset: 0x0037AC34
		private bool InsertedSlotStillOwnsItem()
		{
			return this._inventory != null && this._insertedSlot >= 0 && this._inventory.ActiveSlotIndex == this._insertedSlot && this._inventory.GetItemAt(this._insertedSlot) == this._insertedItem;
		}

		// Token: 0x0600884F RID: 34895 RVA: 0x0037CA89 File Offset: 0x0037AC89
		private static bool IsUsable(InsertableToyPlacement placement)
		{
			return placement != null && placement.Enabled && placement.InputKey > KeyCode.None;
		}

		// Token: 0x06008850 RID: 34896 RVA: 0x0037CAA4 File Offset: 0x0037ACA4
		private bool TryAcquireInsertedActivity(bool protectCommitFrame)
		{
			PlayerContextResource playerContextResource = (protectCommitFrame ? (PlayerContextResource.WorldInteraction | PlayerContextResource.BodyAttachment) : PlayerContextResource.BodyAttachment);
			PlayerActivity playerActivity = (protectCommitFrame ? PlayerActivity.Interact : PlayerActivity.None);
			if (this._activityLease != null && this._activityLease.IsActive)
			{
				return PlayerActivityGate.TryUpdate(this._activityLease, playerActivity, playerContextResource, PlayerActivity.None);
			}
			this._activityLease = null;
			return PlayerActivityGate.TryAcquire(this._blendShapeSourceId, this, playerActivity, playerContextResource, PlayerActivity.None, out this._activityLease, false, null);
		}

		// Token: 0x06008851 RID: 34897 RVA: 0x0037CB0C File Offset: 0x0037AD0C
		private void ApplyInsertedRunningSpeed(InsertableToyItemSO item)
		{
			if (this.WaterOwnsMovement)
			{
				return;
			}
			this.TryCompletePendingRunningSpeedRestore();
			if (this._runningSpeedRestorePending)
			{
				return;
			}
			float num = ((item != null) ? item.InsertedRunningSpeed : 0f);
			vThirdPersonController vThirdPersonController = this.ResolveLocomotionController();
			if (this._runningSpeedOwner != vThirdPersonController)
			{
				this.RestoreInsertedRunningSpeed();
				if (this._runningSpeedRestorePending)
				{
					return;
				}
			}
			if (vThirdPersonController == null || vThirdPersonController.freeSpeed == null || num <= 0f)
			{
				return;
			}
			if (!this._runningSpeedOverrideOwned)
			{
				this._runningSpeedOwner = vThirdPersonController;
				this._savedRunningSpeed = vThirdPersonController.freeSpeed.runningSpeed;
				this._runningSpeedOverrideOwned = true;
			}
			this._appliedRunningSpeed = num;
			vThirdPersonController.freeSpeed.runningSpeed = num;
			if (vThirdPersonController.moveSpeed > num)
			{
				vThirdPersonController.moveSpeed = num;
			}
		}

		// Token: 0x06008852 RID: 34898 RVA: 0x0037CBD0 File Offset: 0x0037ADD0
		private void MaintainInsertedRunningSpeed()
		{
			if (!this.IsInserted || this.WaterOwnsMovement)
			{
				return;
			}
			vThirdPersonController vThirdPersonController = this.ResolveLocomotionController();
			if (!this._runningSpeedOverrideOwned || vThirdPersonController != this._runningSpeedOwner)
			{
				this.ApplyInsertedRunningSpeed(this._insertedItem);
				return;
			}
			if (vThirdPersonController != null && vThirdPersonController.freeSpeed != null && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.FullBodyAnimation))
			{
				if (!Mathf.Approximately(vThirdPersonController.freeSpeed.runningSpeed, this._appliedRunningSpeed))
				{
					vThirdPersonController.freeSpeed.runningSpeed = this._appliedRunningSpeed;
				}
				if (vThirdPersonController.moveSpeed > this._appliedRunningSpeed)
				{
					vThirdPersonController.moveSpeed = this._appliedRunningSpeed;
				}
			}
		}

		// Token: 0x06008853 RID: 34899 RVA: 0x0037CC74 File Offset: 0x0037AE74
		private vThirdPersonController ResolveLocomotionController()
		{
			Transform transform = ((this._inventory != null) ? this._inventory.transform : base.transform);
			vThirdPersonController vThirdPersonController = transform.GetComponent<vThirdPersonController>();
			if (vThirdPersonController == null)
			{
				vThirdPersonController = transform.GetComponentInParent<vThirdPersonController>(true);
			}
			if (vThirdPersonController == null && this._animator != null)
			{
				vThirdPersonController = this._animator.GetComponentInParent<vThirdPersonController>(true);
			}
			if (vThirdPersonController == null)
			{
				vThirdPersonController = transform.GetComponentInChildren<vThirdPersonController>(true);
			}
			return vThirdPersonController;
		}

		// Token: 0x06008854 RID: 34900 RVA: 0x0037CCF4 File Offset: 0x0037AEF4
		private void RestoreInsertedRunningSpeed()
		{
			if (!this._runningSpeedOverrideOwned)
			{
				return;
			}
			if (this._runningSpeedOwner == null || this._runningSpeedOwner.freeSpeed == null)
			{
				this.ClearRunningSpeedOverrideState();
				return;
			}
			float runningSpeed = this._runningSpeedOwner.freeSpeed.runningSpeed;
			if (PlayerActivityGate.IsResourceOwned(PlayerContextResource.FullBodyAnimation))
			{
				this._runningSpeedRestorePending = true;
				return;
			}
			if (Mathf.Approximately(runningSpeed, this._appliedRunningSpeed))
			{
				this._runningSpeedOwner.freeSpeed.runningSpeed = this._savedRunningSpeed;
				this.ClearRunningSpeedOverrideState();
				return;
			}
			Mathf.Approximately(runningSpeed, this._savedRunningSpeed);
			this.ClearRunningSpeedOverrideState();
		}

		// Token: 0x06008855 RID: 34901 RVA: 0x0037CD8C File Offset: 0x0037AF8C
		private void TryCompletePendingRunningSpeedRestore()
		{
			if (!this._runningSpeedRestorePending)
			{
				return;
			}
			if (this._runningSpeedOwner == null || this._runningSpeedOwner.freeSpeed == null)
			{
				this.ClearRunningSpeedOverrideState();
				return;
			}
			if (PlayerActivityGate.IsResourceOwned(PlayerContextResource.FullBodyAnimation))
			{
				return;
			}
			float runningSpeed = this._runningSpeedOwner.freeSpeed.runningSpeed;
			if (Mathf.Approximately(runningSpeed, this._appliedRunningSpeed))
			{
				this._runningSpeedOwner.freeSpeed.runningSpeed = this._savedRunningSpeed;
				this.ClearRunningSpeedOverrideState();
				return;
			}
			if (Mathf.Approximately(runningSpeed, this._savedRunningSpeed) || !PlayerActivityGate.IsResourceOwned(PlayerContextResource.FullBodyAnimation))
			{
				this.ClearRunningSpeedOverrideState();
			}
		}

		// Token: 0x06008856 RID: 34902 RVA: 0x0037CE25 File Offset: 0x0037B025
		private void ClearRunningSpeedOverrideState()
		{
			this._runningSpeedOwner = null;
			this._savedRunningSpeed = 0f;
			this._appliedRunningSpeed = 0f;
			this._runningSpeedOverrideOwned = false;
			this._runningSpeedRestorePending = false;
		}

		// Token: 0x06008857 RID: 34903 RVA: 0x0037CE54 File Offset: 0x0037B054
		private bool TryAcquireHeldActionActivity()
		{
			if (this._activityLease != null && this._activityLease.IsActive)
			{
				return PlayerActivityGate.TryUpdate(this._activityLease, PlayerActivity.Interact, PlayerContextResource.WorldInteraction, PlayerActivity.None);
			}
			this._activityLease = null;
			return PlayerActivityGate.TryAcquire(this._blendShapeSourceId + ".HeldActions", this, PlayerActivity.Interact, PlayerContextResource.WorldInteraction, PlayerActivity.None, out this._activityLease, false, null);
		}

		// Token: 0x06008858 RID: 34904 RVA: 0x0037CEAF File Offset: 0x0037B0AF
		private void ReleaseActivity()
		{
			PlayerActivityLease activityLease = this._activityLease;
			if (activityLease != null)
			{
				activityLease.Dispose();
			}
			this._activityLease = null;
		}

		// Token: 0x06008859 RID: 34905 RVA: 0x0037CECC File Offset: 0x0037B0CC
		private Transform ResolveSocket(InsertableToyPlacement placement)
		{
			if (this._animator == null || placement == null)
			{
				return null;
			}
			if (!string.IsNullOrWhiteSpace(placement.SocketTransformName))
			{
				foreach (Transform transform in this._animator.GetComponentsInChildren<Transform>(true))
				{
					if (transform != null && string.Equals(transform.name, placement.SocketTransformName, StringComparison.Ordinal))
					{
						return transform;
					}
				}
			}
			if (!this._animator.isHuman || placement.SocketBone == HumanBodyBones.LastBone)
			{
				return null;
			}
			return this._animator.GetBoneTransform(placement.SocketBone);
		}

		// Token: 0x0600885A RID: 34906 RVA: 0x0037CF64 File Offset: 0x0037B164
		private void ConfigureHeldVisual(InsertableToyItemSO heldToy)
		{
			GameObject gameObject = ((heldToy != null && this._inventory != null) ? this._inventory.GetSpawnedHandObject() : null);
			if (gameObject == this._configuredHandVisual)
			{
				return;
			}
			this._configuredHandVisual = gameObject;
			if (this._configuredHandVisual != null)
			{
				InsertableToyController.ConfigureAsNonPhysicalProp(this._configuredHandVisual);
			}
		}

		// Token: 0x0600885B RID: 34907 RVA: 0x0037CFC8 File Offset: 0x0037B1C8
		private static void ConfigureAsNonPhysicalProp(GameObject prop)
		{
			if (prop == null)
			{
				return;
			}
			Collider[] componentsInChildren = prop.GetComponentsInChildren<Collider>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != null)
				{
					componentsInChildren[i].enabled = false;
				}
			}
			foreach (Rigidbody rigidbody in prop.GetComponentsInChildren<Rigidbody>(true))
			{
				if (!(rigidbody == null))
				{
					rigidbody.velocity = Vector3.zero;
					rigidbody.angularVelocity = Vector3.zero;
					rigidbody.useGravity = false;
					rigidbody.isKinematic = true;
					rigidbody.detectCollisions = false;
				}
			}
			foreach (MonoBehaviour monoBehaviour in prop.GetComponentsInChildren<MonoBehaviour>(true))
			{
				if (monoBehaviour is IInteractable || monoBehaviour is IInteractionCandidate)
				{
					monoBehaviour.enabled = false;
				}
			}
			InteractablePromptUI[] componentsInChildren4 = prop.GetComponentsInChildren<InteractablePromptUI>(true);
			for (int l = 0; l < componentsInChildren4.Length; l++)
			{
				if (componentsInChildren4[l] != null)
				{
					componentsInChildren4[l].gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x0600885C RID: 34908 RVA: 0x0037D0D8 File Offset: 0x0037B2D8
		private void PrepareBlendShapeOverride(bool logIfMissing)
		{
			this._blendShapeBindings.Clear();
			string text = ((this._insertedPlacement != null) ? this._insertedPlacement.BlendShapeName : null);
			BlendShapeManager blendShapeManager = BlendShapeManager.ResolveForRig((this._animator != null) ? this._animator : this);
			this._blendShapeManager = ((blendShapeManager != null && !string.IsNullOrWhiteSpace(text) && blendShapeManager.HasBlendShape(text)) ? blendShapeManager : null);
			this._nextBlendShapeRebindTime = Time.unscaledTime + 1f;
			if (this._animator == null || string.IsNullOrWhiteSpace(text))
			{
				return;
			}
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in this._animator.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				Mesh mesh = ((skinnedMeshRenderer != null) ? skinnedMeshRenderer.sharedMesh : null);
				if (!(mesh == null))
				{
					for (int j = 0; j < mesh.blendShapeCount; j++)
					{
						string text2 = mesh.GetBlendShapeName(j);
						int num = text2.LastIndexOf(':');
						if (num >= 0)
						{
							text2 = text2.Substring(num + 1);
						}
						if (string.Equals(text2, text, StringComparison.Ordinal))
						{
							this._blendShapeBindings.Add(new InsertableToyController.BlendShapeBinding
							{
								Renderer = skinnedMeshRenderer,
								Index = j,
								Baseline = skinnedMeshRenderer.GetBlendShapeWeight(j)
							});
						}
					}
				}
			}
			if (logIfMissing && this._blendShapeBindings.Count == 0 && this._blendShapeManager == null)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"[InsertableToyController] Blend shape '",
					text,
					"' not found under '",
					this._animator.name,
					"'."
				}), this);
			}
		}

		// Token: 0x0600885D RID: 34909 RVA: 0x0037D288 File Offset: 0x0037B488
		private bool HasUsableFallbackBinding()
		{
			for (int i = 0; i < this._blendShapeBindings.Count; i++)
			{
				InsertableToyController.BlendShapeBinding blendShapeBinding = this._blendShapeBindings[i];
				if (blendShapeBinding.Renderer != null && blendShapeBinding.Renderer.sharedMesh != null && blendShapeBinding.Index >= 0 && blendShapeBinding.Index < blendShapeBinding.Renderer.sharedMesh.blendShapeCount)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600885E RID: 34910 RVA: 0x0037D300 File Offset: 0x0037B500
		private void ApplyBlendShapeOverride()
		{
			if (this._insertedPlacement == null || string.IsNullOrWhiteSpace(this._insertedPlacement.BlendShapeName))
			{
				return;
			}
			if (this._blendShapeManager != null)
			{
				if (this._registeredBlendShapeManager != null && this._registeredBlendShapeManager != this._blendShapeManager)
				{
					this._registeredBlendShapeManager.RemoveAllOverridesFromSource(this._blendShapeSourceId);
				}
				this._registeredBlendShapeManager = this._blendShapeManager;
				this._blendShapeManager.SetOverride(this._insertedPlacement.BlendShapeName, this._insertedPlacement.BlendShapeWeight, this._blendShapeSourceId, BlendShapePriority.Animation, false);
				return;
			}
			this.ApplyFallbackBlendShape();
		}

		// Token: 0x0600885F RID: 34911 RVA: 0x0037D3A8 File Offset: 0x0037B5A8
		private void ApplyFallbackBlendShape()
		{
			if (this._insertedPlacement == null)
			{
				return;
			}
			float blendShapeWeight = this._insertedPlacement.BlendShapeWeight;
			for (int i = 0; i < this._blendShapeBindings.Count; i++)
			{
				InsertableToyController.BlendShapeBinding blendShapeBinding = this._blendShapeBindings[i];
				if (blendShapeBinding.Renderer != null && blendShapeBinding.Renderer.sharedMesh != null && blendShapeBinding.Index >= 0 && blendShapeBinding.Index < blendShapeBinding.Renderer.sharedMesh.blendShapeCount)
				{
					blendShapeBinding.Renderer.SetBlendShapeWeight(blendShapeBinding.Index, blendShapeWeight);
				}
			}
		}

		// Token: 0x06008860 RID: 34912 RVA: 0x0037D444 File Offset: 0x0037B644
		private void ReleaseBlendShapeOverride()
		{
			if (this._registeredBlendShapeManager != null)
			{
				this._registeredBlendShapeManager.RemoveAllOverridesFromSource(this._blendShapeSourceId);
			}
			if (this._blendShapeManager == null)
			{
				for (int i = 0; i < this._blendShapeBindings.Count; i++)
				{
					InsertableToyController.BlendShapeBinding blendShapeBinding = this._blendShapeBindings[i];
					if (blendShapeBinding.Renderer != null && blendShapeBinding.Renderer.sharedMesh != null && blendShapeBinding.Index >= 0 && blendShapeBinding.Index < blendShapeBinding.Renderer.sharedMesh.blendShapeCount)
					{
						blendShapeBinding.Renderer.SetBlendShapeWeight(blendShapeBinding.Index, blendShapeBinding.Baseline);
					}
				}
			}
			this._blendShapeBindings.Clear();
			this._blendShapeManager = null;
			this._registeredBlendShapeManager = null;
			this._nextBlendShapeRebindTime = 0f;
		}

		// Token: 0x06008861 RID: 34913 RVA: 0x0037D520 File Offset: 0x0037B720
		private PlayerEmotionController ResolveEmotionController()
		{
			PlayerEmotionController playerEmotionController = null;
			if (this._animator != null)
			{
				playerEmotionController = this._animator.GetComponentInParent<PlayerEmotionController>(true);
			}
			if (playerEmotionController == null && this._inventory != null)
			{
				playerEmotionController = this._inventory.GetComponentInChildren<PlayerEmotionController>(true);
			}
			if (playerEmotionController == null && PlayerEmotionController.Instance != null && (this._animator == null || PlayerEmotionController.Instance.PlayerAnimator == this._animator))
			{
				playerEmotionController = PlayerEmotionController.Instance;
			}
			return playerEmotionController;
		}

		// Token: 0x06008862 RID: 34914 RVA: 0x0037D5B0 File Offset: 0x0037B7B0
		private void AcquireEmotionOverrides()
		{
			PlayerEmotionController playerEmotionController = this.ResolveEmotionController();
			if (this._emotionOwner != playerEmotionController)
			{
				this._emotionOwner = playerEmotionController;
				this._emotionLayersHeld = false;
				this._faceLockOwned = false;
				this._faceWasAlreadyOverridden = false;
				this._faceOwnershipInitialized = false;
			}
			if (!this._faceBaselineCaptured)
			{
				this._savedFaceValid = this.TryReadFaceExpression(this._emotionOwner, out this._savedFaceExpression);
				if (!this._savedFaceValid && this._animator != null)
				{
					this._savedFaceValid = this.TryGetAnimatorIntParameter(this._animator, "FaceExpressionIndex", out this._savedFaceExpression);
				}
				this._faceBaselineCaptured = this._savedFaceValid;
			}
			if (this._emotionOwner == null)
			{
				return;
			}
			if (!this._faceOwnershipInitialized)
			{
				this._faceWasAlreadyOverridden = this._emotionOwner.IsFacialExpressionOverriddenForAction;
				this._faceOwnershipInitialized = true;
			}
			if (!this._emotionLayersHeld && this._emotionOwner.PlayerAnimator != null)
			{
				this._emotionOwner.DisableEmotionLayersForAction();
				this._emotionLayersHeld = true;
			}
			if (!this._faceWasAlreadyOverridden && !this._faceLockOwned)
			{
				this._emotionOwner.DisableFacialExpressionForAction();
				this._faceLockOwned = true;
			}
		}

		// Token: 0x06008863 RID: 34915 RVA: 0x0037D6D4 File Offset: 0x0037B8D4
		private void MaintainFaceOverride()
		{
			if (!this.IsInserted)
			{
				return;
			}
			if (this._emotionOwner == null || !this._emotionLayersHeld || !this._faceBaselineCaptured)
			{
				this.AcquireEmotionOverrides();
			}
			int num2;
			if (this._emotionOwner != null)
			{
				if (!this._emotionOwner.IsFacialExpressionOverriddenForAction)
				{
					this._emotionOwner.DisableFacialExpressionForAction();
					this._faceLockOwned = true;
				}
				int num;
				if (!this.TryReadFaceExpression(this._emotionOwner, out num) || num != this.orgasmExpressionIndex)
				{
					this._emotionOwner.SetExpression(this.orgasmExpressionIndex);
					return;
				}
			}
			else if (this._animator != null && this.TryGetAnimatorIntParameter(this._animator, "FaceExpressionIndex", out num2) && num2 != this.orgasmExpressionIndex)
			{
				this._animator.SetInteger("FaceExpressionIndex", this.orgasmExpressionIndex);
			}
		}

		// Token: 0x06008864 RID: 34916 RVA: 0x0037D7A8 File Offset: 0x0037B9A8
		private void ReleaseEmotionOverrides()
		{
			bool flag = this.IsCurrentFace(this.orgasmExpressionIndex);
			if (this._emotionOwner != null)
			{
				if (this._faceLockOwned)
				{
					if (this._emotionOwner.IsFacialExpressionOverriddenForAction && flag)
					{
						this._emotionOwner.RestoreFacialExpression();
					}
				}
				else if (this._faceWasAlreadyOverridden && this._savedFaceValid && flag)
				{
					this._emotionOwner.SetExpression(this._savedFaceExpression);
				}
				if (this._emotionLayersHeld)
				{
					this._emotionOwner.RestoreEmotionLayers();
				}
			}
			else if (this._savedFaceValid && flag && this._animator != null)
			{
				this._animator.SetInteger("FaceExpressionIndex", this._savedFaceExpression);
			}
			this._emotionOwner = null;
			this._emotionLayersHeld = false;
			this._faceLockOwned = false;
			this._faceWasAlreadyOverridden = false;
			this._faceOwnershipInitialized = false;
			this._faceBaselineCaptured = false;
			this._savedFaceValid = false;
			this._savedFaceExpression = 0;
		}

		// Token: 0x06008865 RID: 34917 RVA: 0x0037D898 File Offset: 0x0037BA98
		private bool TryReadFaceExpression(PlayerEmotionController emotion, out int value)
		{
			value = 0;
			if (emotion == null || emotion.PlayerAnimator == null)
			{
				return false;
			}
			string text = (string.IsNullOrEmpty(emotion.ExpressionParamName) ? "FaceExpressionIndex" : emotion.ExpressionParamName);
			return this.TryGetAnimatorIntParameter(emotion.PlayerAnimator, text, out value);
		}

		// Token: 0x06008866 RID: 34918 RVA: 0x0037D8EC File Offset: 0x0037BAEC
		private bool IsCurrentFace(int expected)
		{
			int num;
			if (this._emotionOwner != null && this.TryReadFaceExpression(this._emotionOwner, out num))
			{
				return num == expected;
			}
			return this._animator != null && this.TryGetAnimatorIntParameter(this._animator, "FaceExpressionIndex", out num) && num == expected;
		}

		// Token: 0x06008867 RID: 34919 RVA: 0x0037D948 File Offset: 0x0037BB48
		private bool TryGetAnimatorIntParameter(Animator animator, string name, out int value)
		{
			value = 0;
			if (animator == null || string.IsNullOrEmpty(name))
			{
				return false;
			}
			RuntimeAnimatorController runtimeAnimatorController = animator.runtimeAnimatorController;
			if (this._cachedFaceParameterAnimator != animator || this._cachedFaceParameterController != runtimeAnimatorController || this._cachedFaceParameterName != name)
			{
				this._cachedFaceParameterAnimator = animator;
				this._cachedFaceParameterController = runtimeAnimatorController;
				this._cachedFaceParameterName = name;
				this._cachedFaceParameterHash = Animator.StringToHash(name);
				this._cachedFaceParameterExists = false;
				foreach (AnimatorControllerParameter animatorControllerParameter in animator.parameters)
				{
					if (animatorControllerParameter.type == AnimatorControllerParameterType.Int && !(animatorControllerParameter.name != name))
					{
						this._cachedFaceParameterHash = animatorControllerParameter.nameHash;
						this._cachedFaceParameterExists = true;
						break;
					}
				}
			}
			if (!this._cachedFaceParameterExists)
			{
				return false;
			}
			value = animator.GetInteger(this._cachedFaceParameterHash);
			return true;
		}

		// Token: 0x06008868 RID: 34920 RVA: 0x0037DA24 File Offset: 0x0037BC24
		private void SetInsertedAnimatorState(bool value)
		{
			value &= !this.WaterOwnsMovement;
			if (this._animator == null || string.IsNullOrEmpty(this.insertedAnimatorParameter))
			{
				return;
			}
			this.ResolveInsertedAnimatorParameter();
			if (this._hasInsertedAnimatorParameter && this._animator.GetBool(this.insertedAnimatorParameter) != value)
			{
				this._animator.SetBool(this.insertedAnimatorParameter, value);
			}
		}

		// Token: 0x06008869 RID: 34921 RVA: 0x0037DA90 File Offset: 0x0037BC90
		private static void SetAnimatorBoolIfPresent(Animator animator, string parameterName, bool value)
		{
			if (animator == null || string.IsNullOrEmpty(parameterName))
			{
				return;
			}
			foreach (AnimatorControllerParameter animatorControllerParameter in animator.parameters)
			{
				if (animatorControllerParameter.type == AnimatorControllerParameterType.Bool && !(animatorControllerParameter.name != parameterName))
				{
					animator.SetBool(animatorControllerParameter.nameHash, value);
					return;
				}
			}
		}

		// Token: 0x0600886A RID: 34922 RVA: 0x0037DAF0 File Offset: 0x0037BCF0
		private void ResolveInsertedAnimatorParameter()
		{
			RuntimeAnimatorController runtimeAnimatorController = this._animator.runtimeAnimatorController;
			if (this._resolvedAnimatorController == runtimeAnimatorController)
			{
				return;
			}
			this._resolvedAnimatorController = runtimeAnimatorController;
			this._hasInsertedAnimatorParameter = false;
			AnimatorControllerParameter[] parameters = this._animator.parameters;
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].type == AnimatorControllerParameterType.Bool && parameters[i].name == this.insertedAnimatorParameter)
				{
					this._hasInsertedAnimatorParameter = true;
					return;
				}
			}
		}

		// Token: 0x0600886B RID: 34923 RVA: 0x0037DB68 File Offset: 0x0037BD68
		private void InvalidateHud()
		{
			this._hudItem = null;
			this._hudNeedsRefresh = true;
		}

		// Token: 0x0600886C RID: 34924 RVA: 0x0037DB78 File Offset: 0x0037BD78
		private void MaintainHud(InsertableToyItemSO item, bool inserted)
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (item == null)
			{
				if (this._runtimeHudConfig != null && instance != null)
				{
					instance.HideIfCurrent(this._runtimeHudConfig);
				}
				this._hudItem = null;
				this._hudNeedsRefresh = false;
				this._hudShownFrame = -1;
				return;
			}
			if (this._runtimeHudConfig == null)
			{
				this._runtimeHudConfig = ScriptableObject.CreateInstance<InteractionConfigSO>();
				this._runtimeHudConfig.name = "InsertableToyActions_Runtime";
				this._runtimeHudConfig.hideFlags = HideFlags.HideAndDontSave;
				this._hudNeedsRefresh = true;
			}
			if (this._hudItem != item || this._hudInsertedMode != inserted)
			{
				this.BuildHudConfig(item, inserted);
				this._hudItem = item;
				this._hudInsertedMode = inserted;
				this._hudNeedsRefresh = true;
			}
			if (instance == null)
			{
				return;
			}
			if (this.WaterOwnsMovement || !InsertableToyController.MayPresentActions())
			{
				instance.HideIfCurrent(this._runtimeHudConfig);
				return;
			}
			if (instance.CurrentConfig == this._runtimeHudConfig)
			{
				if (this._hudNeedsRefresh)
				{
					instance.Show(this._runtimeHudConfig, true);
					this._hudShownFrame = Time.frameCount;
					this._hudNeedsRefresh = false;
				}
				return;
			}
			if (instance.CurrentConfig == null)
			{
				instance.Show(this._runtimeHudConfig, true);
				this._hudShownFrame = Time.frameCount;
				this._hudNeedsRefresh = false;
			}
		}

		// Token: 0x0600886D RID: 34925 RVA: 0x0037DCD0 File Offset: 0x0037BED0
		private void BuildHudConfig(InsertableToyItemSO item, bool inserted)
		{
			this._runtimeHudConfig.objectName = (inserted ? item.InsertedHudName : item.itemName);
			this._runtimeHudConfig.mainActionName = string.Empty;
			this._runtimeHudConfig.mainKeyDisplayLabel = string.Empty;
			this._runtimeHudConfig.secondaryActionName = string.Empty;
			this._runtimeHudConfig.secondaryKeyDisplayLabel = string.Empty;
			this._runtimeHudConfig.usableActions.Clear();
			this._analInput = null;
			this._vaginalInput = null;
			this._removeInput = null;
			if (inserted)
			{
				if (item.RemoveInputKey != KeyCode.None)
				{
					this._removeInput = InsertableToyController.CreateInput(item.RemoveInputKey);
					this._runtimeHudConfig.usableActions.Add(new UsableAction
					{
						actionName = item.RemoveActionName,
						keyDisplayLabel = item.RemoveKeyDisplayLabel,
						description = "Return the item to Ciel's hand"
					});
				}
				return;
			}
			this.AddPlacementAction(item.AnalPlacement, ref this._analInput);
			this.AddPlacementAction(item.VaginalPlacement, ref this._vaginalInput);
		}

		// Token: 0x0600886E RID: 34926 RVA: 0x0037DDD8 File Offset: 0x0037BFD8
		private void AddPlacementAction(InsertableToyPlacement placement, ref GenericInput input)
		{
			if (!InsertableToyController.IsUsable(placement))
			{
				return;
			}
			input = InsertableToyController.CreateInput(placement.InputKey);
			this._runtimeHudConfig.usableActions.Add(new UsableAction
			{
				actionName = placement.ActionName,
				keyDisplayLabel = placement.KeyDisplayLabel,
				description = ((placement.Site == InsertionSite.Anal) ? "Insert the held item into Ciel's butt" : "Insert the held item into Ciel's vagina")
			});
		}

		// Token: 0x0600886F RID: 34927 RVA: 0x0037DE42 File Offset: 0x0037C042
		private static GenericInput CreateInput(KeyCode key)
		{
			return new GenericInput(key.ToString(), string.Empty, string.Empty);
		}

		// Token: 0x06008870 RID: 34928 RVA: 0x0037DE60 File Offset: 0x0037C060
		private bool CanReadOurHudInput()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			return !this.WaterOwnsMovement && InsertableToyController.MayPresentActions() && instance != null && instance.IsShowing && instance.CurrentConfig == this._runtimeHudConfig && !this._hudNeedsRefresh && this._hudShownFrame != Time.frameCount;
		}

		// Token: 0x06008871 RID: 34929 RVA: 0x0037DEC0 File Offset: 0x0037C0C0
		private static bool MayPresentActions()
		{
			return PlayerActivityGate.Allows(PlayerActivity.Interact) && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.FullBodyAnimation) && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.WaterMovement) && !CielOSController.AnyOpen;
		}

		// Token: 0x06008872 RID: 34930 RVA: 0x0037DEEC File Offset: 0x0037C0EC
		private void PlayActionSfx(AudioClip clip, float volume, float pitchMin, float pitchMax)
		{
			volume = Mathf.Clamp01(volume);
			if (clip == null || volume <= 0f)
			{
				return;
			}
			AudioSource audioSource = this.EnsureActionAudioSource();
			if (audioSource == null)
			{
				return;
			}
			float num = Mathf.Clamp(Mathf.Min(pitchMin, pitchMax), 0.1f, 3f);
			float num2 = Mathf.Clamp(Mathf.Max(pitchMin, pitchMax), 0.1f, 3f);
			audioSource.pitch = (Mathf.Approximately(num, num2) ? num : Mathf.Lerp(num, num2, this.NextActionAudioRandom01()));
			audioSource.PlayOneShot(clip, volume);
		}

		// Token: 0x06008873 RID: 34931 RVA: 0x0037DF7C File Offset: 0x0037C17C
		private float NextActionAudioRandom01()
		{
			uint num = this._actionAudioRandomState;
			if (num == 0U)
			{
				num = 2738958700U;
			}
			num ^= num << 13;
			num ^= num >> 17;
			num ^= num << 5;
			this._actionAudioRandomState = num;
			return (num & 16777215U) / 16777215f;
		}

		// Token: 0x06008874 RID: 34932 RVA: 0x0037DFC4 File Offset: 0x0037C1C4
		private AudioSource EnsureActionAudioSource()
		{
			if (this._actionAudioSource != null)
			{
				return this._actionAudioSource;
			}
			Transform transform = base.transform.Find("InsertableToyActionAudio");
			if (transform != null)
			{
				this._actionAudioSource = transform.GetComponent<AudioSource>();
				if (this._actionAudioSource == null)
				{
					this._actionAudioSource = transform.gameObject.AddComponent<AudioSource>();
				}
			}
			else
			{
				GameObject gameObject = new GameObject("InsertableToyActionAudio");
				gameObject.transform.SetParent(base.transform, false);
				this._actionAudioSource = gameObject.AddComponent<AudioSource>();
			}
			this._actionAudioSource.playOnAwake = false;
			this._actionAudioSource.loop = false;
			this._actionAudioSource.spatialBlend = 0f;
			this._actionAudioSource.dopplerLevel = 0f;
			this._actionAudioSource.volume = 1f;
			GameAudioRouter.Route(this._actionAudioSource, GameAudioRouter.Channel.Sfx);
			return this._actionAudioSource;
		}

		// Token: 0x06008875 RID: 34933 RVA: 0x0037E0B0 File Offset: 0x0037C2B0
		private bool CleanupInsertion(bool resumeHand)
		{
			if (!this.IsInserted || this._cleaningUp)
			{
				return false;
			}
			this._cleaningUp = true;
			ItemUsageSignal.Set(this, false);
			InsertableToyItemSO insertedItem = this._insertedItem;
			int insertedSlot = this._insertedSlot;
			this.ReleaseBlendShapeOverride();
			this.ReleaseEmotionOverrides();
			this.SetInsertedAnimatorState(false);
			this.RestoreInsertedRunningSpeed();
			if (this._insertedVisual != null)
			{
				this._insertedVisual.SetActive(false);
				Object.Destroy(this._insertedVisual);
			}
			this._insertedVisual = null;
			this._insertedPlacement = null;
			this._insertedItem = null;
			this._insertedSlot = -1;
			if (this._slotLockOwned && this._inventory != null)
			{
				this._inventory.ReleaseSlotInputLock(this);
			}
			this._slotLockOwned = false;
			this.ReleaseActivity();
			if (resumeHand && this._inventory != null && this._inventory.ActiveSlotIndex == insertedSlot && this._inventory.GetItemAt(insertedSlot) == insertedItem && base.gameObject.scene.IsValid() && base.gameObject.scene.isLoaded)
			{
				this._inventory.ResumeActiveItem();
				this.ConfigureHeldVisual(insertedItem);
			}
			this._cleaningUp = false;
			this.InvalidateHud();
			if (this.debugLogs)
			{
				Debug.Log("[InsertableToyController] Removed '" + insertedItem.itemName + "' from body.", this);
			}
			return true;
		}

		// Token: 0x06008876 RID: 34934 RVA: 0x0037E218 File Offset: 0x0037C418
		private void OnDisable()
		{
			ItemUsageSignal.Set(this, false);
			this.UnhookInventoryEvents();
			if (this.IsInserted)
			{
				this.CleanupInsertion(true);
			}
			this.RestoreInsertedRunningSpeed();
			this.ReleaseActivity();
			if (this._runtimeHudConfig != null)
			{
				InteractionHUDController instance = InteractionHUDController.Instance;
				if (instance == null)
				{
					return;
				}
				instance.HideIfCurrent(this._runtimeHudConfig);
			}
		}

		// Token: 0x06008877 RID: 34935 RVA: 0x0037E274 File Offset: 0x0037C474
		private void OnDestroy()
		{
			ItemUsageSignal.Set(this, false);
			this.UnhookInventoryEvents();
			if (this.IsInserted)
			{
				this.CleanupInsertion(false);
			}
			this.RestoreInsertedRunningSpeed();
			this.ReleaseActivity();
			if (this._runtimeHudConfig == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(this._runtimeHudConfig);
			}
			else
			{
				Object.DestroyImmediate(this._runtimeHudConfig);
			}
			this._runtimeHudConfig = null;
		}

		// Token: 0x04008AF0 RID: 35568
		private const int DefaultOrgasmExpressionIndex = 5;

		// Token: 0x04008AF1 RID: 35569
		private const string DefaultInsertedAnimatorParameter = "InsertedToyActive";

		// Token: 0x04008AF2 RID: 35570
		private const string ActionAudioObjectName = "InsertableToyActionAudio";

		// Token: 0x04008AF3 RID: 35571
		private const PlayerActivity InsertedBlockedActivities = PlayerActivity.None;

		// Token: 0x04008AF4 RID: 35572
		private const PlayerContextResource InsertedResources = PlayerContextResource.BodyAttachment;

		// Token: 0x04008AF5 RID: 35573
		private const PlayerContextResource InsertedCommitResources = PlayerContextResource.WorldInteraction | PlayerContextResource.BodyAttachment;

		// Token: 0x04008AF6 RID: 35574
		private const PlayerContextResource HeldActionResources = PlayerContextResource.WorldInteraction;

		// Token: 0x04008AF7 RID: 35575
		[Header("Animator")]
		[SerializeField]
		private string insertedAnimatorParameter = "InsertedToyActive";

		// Token: 0x04008AF8 RID: 35576
		[SerializeField]
		private int orgasmExpressionIndex = 5;

		// Token: 0x04008AF9 RID: 35577
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04008AFA RID: 35578
		private PlayerInventoryManager _inventory;

		// Token: 0x04008AFB RID: 35579
		private Animator _animator;

		// Token: 0x04008AFC RID: 35580
		private bool _inventoryEventsHooked;

		// Token: 0x04008AFD RID: 35581
		private InsertableToyItemSO _insertedItem;

		// Token: 0x04008AFE RID: 35582
		private InsertableToyPlacement _insertedPlacement;

		// Token: 0x04008AFF RID: 35583
		private InsertionSite _insertedSite;

		// Token: 0x04008B00 RID: 35584
		private int _insertedSlot = -1;

		// Token: 0x04008B01 RID: 35585
		private GameObject _insertedVisual;

		// Token: 0x04008B02 RID: 35586
		private GameObject _configuredHandVisual;

		// Token: 0x04008B03 RID: 35587
		private bool _slotLockOwned;

		// Token: 0x04008B04 RID: 35588
		private bool _cleaningUp;

		// Token: 0x04008B05 RID: 35589
		private PlayerActivityLease _activityLease;

		// Token: 0x04008B06 RID: 35590
		private vThirdPersonController _runningSpeedOwner;

		// Token: 0x04008B07 RID: 35591
		private float _savedRunningSpeed;

		// Token: 0x04008B08 RID: 35592
		private float _appliedRunningSpeed;

		// Token: 0x04008B09 RID: 35593
		private bool _runningSpeedOverrideOwned;

		// Token: 0x04008B0A RID: 35594
		private bool _runningSpeedRestorePending;

		// Token: 0x04008B0B RID: 35595
		private BlendShapeManager _blendShapeManager;

		// Token: 0x04008B0C RID: 35596
		private BlendShapeManager _registeredBlendShapeManager;

		// Token: 0x04008B0D RID: 35597
		private readonly List<InsertableToyController.BlendShapeBinding> _blendShapeBindings = new List<InsertableToyController.BlendShapeBinding>(4);

		// Token: 0x04008B0E RID: 35598
		private string _blendShapeSourceId;

		// Token: 0x04008B0F RID: 35599
		private float _nextBlendShapeRebindTime;

		// Token: 0x04008B10 RID: 35600
		private PlayerEmotionController _emotionOwner;

		// Token: 0x04008B11 RID: 35601
		private bool _emotionLayersHeld;

		// Token: 0x04008B12 RID: 35602
		private bool _faceLockOwned;

		// Token: 0x04008B13 RID: 35603
		private bool _faceWasAlreadyOverridden;

		// Token: 0x04008B14 RID: 35604
		private bool _faceOwnershipInitialized;

		// Token: 0x04008B15 RID: 35605
		private bool _faceBaselineCaptured;

		// Token: 0x04008B16 RID: 35606
		private bool _savedFaceValid;

		// Token: 0x04008B17 RID: 35607
		private int _savedFaceExpression;

		// Token: 0x04008B18 RID: 35608
		private Animator _cachedFaceParameterAnimator;

		// Token: 0x04008B19 RID: 35609
		private RuntimeAnimatorController _cachedFaceParameterController;

		// Token: 0x04008B1A RID: 35610
		private string _cachedFaceParameterName;

		// Token: 0x04008B1B RID: 35611
		private int _cachedFaceParameterHash;

		// Token: 0x04008B1C RID: 35612
		private bool _cachedFaceParameterExists;

		// Token: 0x04008B1D RID: 35613
		private RuntimeAnimatorController _resolvedAnimatorController;

		// Token: 0x04008B1E RID: 35614
		private bool _hasInsertedAnimatorParameter;

		// Token: 0x04008B1F RID: 35615
		private AudioSource _actionAudioSource;

		// Token: 0x04008B20 RID: 35616
		private uint _actionAudioRandomState = 2738958700U;

		// Token: 0x04008B21 RID: 35617
		private InteractionConfigSO _runtimeHudConfig;

		// Token: 0x04008B22 RID: 35618
		private InsertableToyItemSO _hudItem;

		// Token: 0x04008B23 RID: 35619
		private bool _hudInsertedMode;

		// Token: 0x04008B24 RID: 35620
		private bool _hudNeedsRefresh;

		// Token: 0x04008B25 RID: 35621
		private GenericInput _analInput;

		// Token: 0x04008B26 RID: 35622
		private GenericInput _vaginalInput;

		// Token: 0x04008B27 RID: 35623
		private GenericInput _removeInput;

		// Token: 0x04008B28 RID: 35624
		private RapidActionsHUDController _rapidActions;

		// Token: 0x04008B29 RID: 35625
		private int _hudShownFrame = -1;

		// Token: 0x02001174 RID: 4468
		private sealed class BlendShapeBinding
		{
			// Token: 0x04008B2A RID: 35626
			public SkinnedMeshRenderer Renderer;

			// Token: 0x04008B2B RID: 35627
			public int Index;

			// Token: 0x04008B2C RID: 35628
			public float Baseline;
		}
	}
}
