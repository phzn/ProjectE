using System;
using System.Collections;
using System.Collections.Generic;
using Features.BodyPaintStudio;
using Features.DebugTools;
using Features.Detection;
using Features.Flash;
using Features.Inventory;
using Features.NeckSigns;
using Features.PlayerEmotes;
using Features.PlayerEmotions;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.FirstPerson;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace Features.HideFlash
{
	// Token: 0x020011A5 RID: 4517
	public class HideFlashController : MonoBehaviour, IFlashGate
	{
		// Token: 0x1700160D RID: 5645
		// (get) Token: 0x06008A46 RID: 35398 RVA: 0x0038832E File Offset: 0x0038652E
		public static bool HasHeldCoverItem
		{
			get
			{
				return PlayerActivityGate.HasLease("HideFlash.HeldCover");
			}
		}

		// Token: 0x06008A47 RID: 35399 RVA: 0x0038833C File Offset: 0x0038653C
		private void Start()
		{
			if (this.inventoryManager == null)
			{
				this.inventoryManager = base.GetComponentInParent<PlayerInventoryManager>();
			}
			if (this.playerAnimator == null)
			{
				this.playerAnimator = base.GetComponentInParent<Animator>();
			}
			if (this.audioSource == null)
			{
				this.audioSource = base.GetComponentInParent<AudioSource>();
			}
			if (this.playerActionAnimator == null)
			{
				this.playerActionAnimator = base.GetComponentInParent<PlayerActionAnimator>();
			}
			if (this.emotionSource == null)
			{
				this.emotionSource = DetectionEmotionSource.Instance;
			}
			if (this.playerAnimator != null)
			{
				this._hideAndFlashLayerIndex = this.playerAnimator.GetLayerIndex(this.hideAndFlashLayerName);
				this._holdingItemLayerIndex = this.playerAnimator.GetLayerIndex(this.holdingItemLayerName);
				if (this._hideAndFlashLayerIndex == -1)
				{
					Debug.LogWarning("[HideFlashController] Layer '" + this.hideAndFlashLayerName + "' not found in animator.", this);
				}
				if (this._holdingItemLayerIndex == -1)
				{
					Debug.LogWarning("[HideFlashController] Layer '" + this.holdingItemLayerName + "' not found in animator.", this);
				}
			}
			if (this.inventoryManager != null)
			{
				this.inventoryManager.OnActiveSlotChanged += this.OnActiveSlotChanged;
				return;
			}
			Debug.LogWarning("[HideFlashController] No PlayerInventoryManager found. Assign it in the inspector.", this);
		}

		// Token: 0x06008A48 RID: 35400 RVA: 0x00388480 File Offset: 0x00386680
		private void Update()
		{
			if (!this._isEquipped || this._activeItem == null)
			{
				return;
			}
			if (FlyCamera.IsActive)
			{
				this._photoModeSuspended = true;
				return;
			}
			if (this._photoModeSuspended)
			{
				this._photoModeSuspended = false;
				if (this._isFlashing && this._activeItem.flashIsLoop && !this._activeItem.flashInput.GetButton())
				{
					this.CancelFlash();
				}
			}
			this.RefreshFlashHUD();
			if (this._paintSurface != null && InteractionHUDController.Instance != null && InteractionHUDController.Instance.IsActionVisible(this._flashHudConfig, "H") && !ControllerInput.UiInputConsumedThisFrame && (ControllerInput.IsActive ? (ControllerGameplayBridge.AllowsAxes && ControllerInput.GetKeyDown(KeyCode.H)) : GameplayInput.GetKeyDown(KeyCode.H)) && this.TryPaintHeldItem())
			{
				return;
			}
			if (PlayerActivityGate.IsBlocked(PlayerActivity.Flash) && !this._isFlashing)
			{
				return;
			}
			bool buttonDown = this._activeItem.flashInput.GetButtonDown();
			bool buttonUp = this._activeItem.flashInput.GetButtonUp();
			if (this.debugLogs && (buttonDown || buttonUp))
			{
				Debug.Log(string.Format("[HideFlashController] Input: Down={0}, Up={1}, IsFlashing={2}, IsLoop={3}", new object[]
				{
					buttonDown,
					buttonUp,
					this._isFlashing,
					this._activeItem.flashIsLoop
				}), this);
			}
			if (!this._isFlashing && buttonDown)
			{
				this.TryStartFlash();
			}
			if (this._isFlashing && this._activeItem.flashIsLoop && buttonUp)
			{
				this.CancelFlash();
			}
			this.SyncBoxAnimators();
		}

		// Token: 0x06008A49 RID: 35401 RVA: 0x0038861C File Offset: 0x0038681C
		private void SyncBoxAnimators()
		{
			if (this._spawnedPropAnimators.Count == 0 || this.playerAnimator == null)
			{
				return;
			}
			float @float = this.playerAnimator.GetFloat(HideFlashController._boxMagnitudeHash);
			bool @bool = this.playerAnimator.GetBool(HideFlashController._boxCrouchHash);
			foreach (Animator animator in this._spawnedPropAnimators)
			{
				if (!(animator == null))
				{
					animator.SetFloat(HideFlashController._boxMagnitudeHash, @float);
					animator.SetBool(HideFlashController._boxCrouchHash, @bool);
					animator.SetBool(HideFlashController._boxFlashActiveHash, this._isFlashing);
				}
			}
		}

		// Token: 0x06008A4A RID: 35402 RVA: 0x003886D8 File Offset: 0x003868D8
		private void OnDestroy()
		{
			if (this.inventoryManager != null)
			{
				this.inventoryManager.OnActiveSlotChanged -= this.OnActiveSlotChanged;
			}
			this.ForceUnequip();
			if (this._flashHudConfig != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this._flashHudConfig);
					return;
				}
				Object.DestroyImmediate(this._flashHudConfig);
			}
		}

		// Token: 0x06008A4B RID: 35403 RVA: 0x0038873C File Offset: 0x0038693C
		private void OnDisable()
		{
			this.ForceUnequip();
		}

		// Token: 0x06008A4C RID: 35404 RVA: 0x00388744 File Offset: 0x00386944
		private void OnActiveSlotChanged(int slotIndex)
		{
			HideFlashItemSO hideFlashItemSO = ((slotIndex >= 0) ? this.inventoryManager.GetActiveItem() : null) as HideFlashItemSO;
			if (hideFlashItemSO != null)
			{
				this.Equip(hideFlashItemSO, false);
				return;
			}
			if (this._isEquipped)
			{
				if (this.debugLogs)
				{
					Debug.Log("[HideFlashController] Slot changed to non-HideFlash item, unequipping...", this);
				}
				this.Unequip();
			}
		}

		// Token: 0x06008A4D RID: 35405 RVA: 0x0038879C File Offset: 0x0038699C
		private void Equip(HideFlashItemSO item, bool isFromPickup = false)
		{
			if (item == null)
			{
				return;
			}
			if (this._isEquipped)
			{
				this.Unequip();
			}
			if (!this.TryAcquireHeldCoverContext())
			{
				return;
			}
			FlashGateRegistry.InterruptGarmentsForBodyHandoff((this.playerAnimator != null) ? this.playerAnimator.transform : base.transform.root);
			GameplayEmoteController componentInParent = base.GetComponentInParent<GameplayEmoteController>();
			if (componentInParent != null)
			{
				componentInParent.ForceEndEmoteImmediate();
			}
			CoverPoseController instance = CoverPoseController.Instance;
			if (instance != null && instance.IsCovering)
			{
				instance.DeactivateCoverPose();
			}
			vThirdPersonController componentInParent2 = base.GetComponentInParent<vThirdPersonController>();
			if (componentInParent2 != null)
			{
				componentInParent2.isCrouching = false;
			}
			this._activeItem = item;
			this._isEquipped = true;
			this._isEquipping = true;
			this._equipRoutine = base.StartCoroutine(this.EquipRoutine(item, isFromPickup));
			if (!this._isEquipping)
			{
				this._equipRoutine = null;
			}
		}

		// Token: 0x06008A4E RID: 35406 RVA: 0x00388872 File Offset: 0x00386A72
		public void EquipFromPickup(HideFlashItemSO item)
		{
			this.Equip(item, true);
		}

		// Token: 0x06008A4F RID: 35407 RVA: 0x0038887C File Offset: 0x00386A7C
		private IEnumerator EquipRoutine(HideFlashItemSO item, bool isFromPickup)
		{
			if (isFromPickup && item.pickupActionEntry != null && this.playerActionAnimator != null)
			{
				this.FadeHideAndFlashLayer(0f, 0f);
				float num = this.playerActionAnimator.TriggerAction(item.pickupActionEntry);
				if (item.pickupActionEntry.TriggerPrefab != null)
				{
					this._emotionSuppressionDebt++;
				}
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[HideFlashController] Pickup emote started, waiting {0}s...", num), this);
				}
				if (num > 0f)
				{
					yield return new WaitForSeconds(num);
				}
				this.RepayEmotionSuppressionDebt();
			}
			else if (this.equipDelay > 0f)
			{
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[HideFlashController] Waiting {0}s for equip...", this.equipDelay), this);
				}
				yield return new WaitForSeconds(this.equipDelay);
			}
			this.ApplyLocomotionOverride(item);
			this.SpawnProps(item);
			this.SetAnimatorBool(HideFlashController._holdingHash, this.holdingParamName, true);
			this.FadeHideAndFlashLayer(1f, 0f);
			this.FadeHoldingItemLayer(0f);
			this._equipRoutine = null;
			this._isEquipping = false;
			this.SetConcealment(true);
			this.ForceEmotionLayerRecheck();
			DetectionEmotionSource detectionEmotionSource = this.emotionSource;
			if (detectionEmotionSource != null)
			{
				detectionEmotionSource.OnFlashCoverEquipped();
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[HideFlashController] Equipped: {0} (from pickup: {1})", item.itemName, isFromPickup), this);
			}
			yield break;
		}

		// Token: 0x06008A50 RID: 35408 RVA: 0x00388899 File Offset: 0x00386A99
		private void RepayEmotionSuppressionDebt()
		{
			while (this._emotionSuppressionDebt > 0)
			{
				this._emotionSuppressionDebt--;
				PlayerActionAnimator playerActionAnimator = this.playerActionAnimator;
				if (playerActionAnimator != null)
				{
					playerActionAnimator.RestoreOverrideLayer();
				}
			}
		}

		// Token: 0x06008A51 RID: 35409 RVA: 0x003888C8 File Offset: 0x00386AC8
		private void Unequip()
		{
			if (!this._isEquipped)
			{
				return;
			}
			this.SetConcealment(false);
			bool flag = this._isFlashing || this._isEquipping || this._isEndingFlash;
			if (this._flashRoutine != null)
			{
				base.StopCoroutine(this._flashRoutine);
				this._flashRoutine = null;
			}
			if (this._equipRoutine != null)
			{
				base.StopCoroutine(this._equipRoutine);
				this._equipRoutine = null;
			}
			this._isEquipping = false;
			this.StopFlashEndWatch();
			this.RepayEmotionSuppressionDebt();
			if (this._isFlashing)
			{
				this.FireWornDetailTrigger(false);
			}
			this._isFlashing = false;
			this.SetAnimatorBool(HideFlashController._boxFlashActiveHash, "HideFlashActive", false);
			if (flag)
			{
				PlayerActionAnimator playerActionAnimator = this.playerActionAnimator;
				if (playerActionAnimator != null)
				{
					playerActionAnimator.CancelCurrentAction();
				}
			}
			this.SetAnimatorBool(HideFlashController._holdingHash, this.holdingParamName, false);
			this.FadeHideAndFlashLayer(0f, 0f);
			this.RestoreLocomotionOverride();
			this.DestroyProps();
			this._activeItem = null;
			this._isEquipped = false;
			this._photoModeSuspended = false;
			this.ReleaseHeldCoverContext();
			if (this.debugLogs)
			{
				Debug.Log("[HideFlashController] Unequipped.", this);
			}
			this.ForceEmotionLayerRecheck();
			DetectionEmotionSource detectionEmotionSource = this.emotionSource;
			if (detectionEmotionSource != null)
			{
				detectionEmotionSource.OnFlashCoverUnequipped();
			}
			HideFlashController.HideFlashEvent onUnequipped = this.OnUnequipped;
			if (onUnequipped == null)
			{
				return;
			}
			onUnequipped.Invoke();
		}

		// Token: 0x06008A52 RID: 35410 RVA: 0x00388A04 File Offset: 0x00386C04
		private void ForceUnequip()
		{
			this.SetConcealment(false);
			if (this._isEquipped)
			{
				this.Unequip();
			}
			this.ReleaseHeldCoverContext();
		}

		// Token: 0x06008A53 RID: 35411 RVA: 0x00388A21 File Offset: 0x00386C21
		private bool TryAcquireHeldCoverContext()
		{
			return (this._heldCoverLease != null && this._heldCoverLease.IsActive) || PlayerActivityGate.TryAcquire("HideFlash.HeldCover", this, PlayerActivity.Inventory, PlayerContextResource.HeldItem, PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.RapidHUD | PlayerActivity.Interact | PlayerActivity.StanceSwitch | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Phone | PlayerActivity.Crouch | PlayerActivity.ArmsPose, out this._heldCoverLease, true, null);
		}

		// Token: 0x06008A54 RID: 35412 RVA: 0x00388A59 File Offset: 0x00386C59
		private void ReleaseHeldCoverContext()
		{
			PlayerActivityLease heldCoverLease = this._heldCoverLease;
			if (heldCoverLease != null)
			{
				heldCoverLease.Dispose();
			}
			this._heldCoverLease = null;
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.HideIfCurrent(this._flashHudConfig);
		}

		// Token: 0x06008A55 RID: 35413 RVA: 0x00388A8C File Offset: 0x00386C8C
		private void RefreshFlashHUD()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance == null)
			{
				return;
			}
			if (this._heldCoverLease == null || !this._heldCoverLease.IsActive || this._isEquipping || !PlayerActivityGate.Allows(PlayerActivity.Flash) || this._activeItem == null || this._activeItem.flashInput == null || !this._activeItem.flashInput.useInput || this._activeItem.flashActionEntry == null)
			{
				instance.HideIfCurrent(this._flashHudConfig);
				return;
			}
			if (this._flashHudConfig == null)
			{
				this._flashHudConfig = ScriptableObject.CreateInstance<InteractionConfigSO>();
				this._flashHudConfig.name = "Held cover item controls";
				this._flashHudConfig.hideFlags = HideFlags.DontSave;
				this._flashHudConfig.usableActions.Add(new UsableAction
				{
					actionName = "Flash"
				});
				ControllerContextBindings.SetLabel(this._flashHudConfig, "H", ControllerBindings.Label(KeyCode.H), false);
			}
			string text = this._activeItem.flashInput.buttonName;
			if (text == "Mouse0")
			{
				text = "Left Mouse";
			}
			else if (text == "Mouse1")
			{
				text = "Right Mouse";
			}
			this._flashHudConfig.usableActions[0].keyDisplayLabel = text;
			bool flag = this.OwnsPaintSurface(this._paintSurface) && this.ResolvePaintMode() != null && this._paintMode.CanEnterHeldItemMode();
			if (flag && this._flashHudConfig.usableActions.Count == 1)
			{
				this._flashHudConfig.usableActions.Add(new UsableAction
				{
					keyDisplayLabel = "H",
					actionName = "Draw on chalkboard"
				});
			}
			else if (!flag && this._flashHudConfig.usableActions.Count > 1)
			{
				this._flashHudConfig.usableActions.RemoveAt(1);
			}
			instance.Show(this._flashHudConfig);
		}

		// Token: 0x06008A56 RID: 35414 RVA: 0x00388C75 File Offset: 0x00386E75
		private BodyPaintModeController ResolvePaintMode()
		{
			if (this._paintMode == null && this.playerAnimator != null)
			{
				this._paintMode = this.playerAnimator.GetComponentInChildren<BodyPaintModeController>(true);
			}
			return this._paintMode;
		}

		// Token: 0x06008A57 RID: 35415 RVA: 0x00388CAC File Offset: 0x00386EAC
		public bool OwnsPaintSurface(NeckSignSurface surface)
		{
			if (!this._isEquipped || this._isEquipping || this._isFlashing || this._isEndingFlash || this._activeItem == null || this._activeItem.itemId != "hideflash_chalkboard" || this._heldCoverLease == null || !this._heldCoverLease.IsActive || surface == null || !surface.isActiveAndEnabled || surface.PartId != "Item.Chalkboard" || surface.TargetRenderer == null || !surface.TargetRenderer.enabled)
			{
				return false;
			}
			foreach (GameObject gameObject in this._spawnedProps)
			{
				if (gameObject != null && surface.transform.IsChildOf(gameObject.transform))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008A58 RID: 35416 RVA: 0x00388DC0 File Offset: 0x00386FC0
		public bool TryPaintHeldItem()
		{
			if (!this.OwnsPaintSurface(this._paintSurface) || this.ResolvePaintMode() == null || !this._paintMode.TryEnterHeldItemMode(this._paintSurface))
			{
				return false;
			}
			if (ControllerInput.IsActive)
			{
				ControllerInput.ConsumeUIInput();
			}
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance != null)
			{
				instance.HideIfCurrent(this._flashHudConfig);
			}
			return true;
		}

		// Token: 0x06008A59 RID: 35417 RVA: 0x00388E22 File Offset: 0x00387022
		private void ForceEmotionLayerRecheck()
		{
			if (PlayerEmotionController.Instance != null)
			{
				PlayerEmotionController.Instance.ForceLayerWeightUpdate();
				if (this.debugLogs)
				{
					Debug.Log("[HideFlashController] Forced emotion layer recheck.", this);
				}
			}
		}

		// Token: 0x06008A5A RID: 35418 RVA: 0x00388E50 File Offset: 0x00387050
		private void ApplyLocomotionOverride(HideFlashItemSO item)
		{
			if (this.playerAnimator == null)
			{
				return;
			}
			this.playerAnimator.SetInteger(HideFlashController._itemTypeHash, item.itemTypeId);
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[HideFlashController] Set item type: {0} for '{1}'", item.itemTypeId, item.itemName), this);
			}
		}

		// Token: 0x06008A5B RID: 35419 RVA: 0x00388EAB File Offset: 0x003870AB
		private void RestoreLocomotionOverride()
		{
			if (this.playerAnimator == null)
			{
				return;
			}
			this.playerAnimator.SetInteger(HideFlashController._itemTypeHash, 0);
			if (this.debugLogs)
			{
				Debug.Log("[HideFlashController] Reset item type to 0.", this);
			}
		}

		// Token: 0x06008A5C RID: 35420 RVA: 0x00388EE0 File Offset: 0x003870E0
		private void SpawnProps(HideFlashItemSO item)
		{
			if (item.usesWornDetailAnimator)
			{
				this.ResolveWornDetailAnimator(item);
				return;
			}
			if (item.propPrefab == null || this.playerAnimator == null)
			{
				return;
			}
			if (item.isSkinnedProp)
			{
				this.SpawnSkinnedProp(item);
				return;
			}
			this.SpawnClassicProp(item);
		}

		// Token: 0x06008A5D RID: 35421 RVA: 0x00388F34 File Offset: 0x00387134
		private void SpawnSkinnedProp(HideFlashItemSO item)
		{
			Transform transform = this.ResolveAttachBone(item, 0);
			if (transform == null)
			{
				Debug.LogWarning("[HideFlashController] No target bone for '" + item.itemName + "'.", this);
				return;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(item.propPrefab, transform);
			FirstPersonVisibleProp.Mark(gameObject);
			gameObject.name = item.propPrefab.name;
			gameObject.transform.localPosition = item.GetPositionOffset(0);
			gameObject.transform.localRotation = Quaternion.Euler(item.GetRotationOffset(0));
			gameObject.transform.localScale = item.propPrefab.transform.localScale;
			gameObject.SetActive(true);
			HideFlashController.DisableInteractionComponents(gameObject);
			this._spawnedProps.Add(gameObject);
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[HideFlashController] Prop '{0}' spawned under '{1}' (scale preserved: {2}).", gameObject.name, transform.name, gameObject.transform.localScale), this);
			}
		}

		// Token: 0x06008A5E RID: 35422 RVA: 0x00389024 File Offset: 0x00387224
		private void SpawnClassicProp(HideFlashItemSO item)
		{
			int num = ((item.attachBones != null && item.attachBones.Length != 0) ? item.attachBones.Length : 1);
			for (int i = 0; i < num; i++)
			{
				GameObject propPrefabForBone = item.GetPropPrefabForBone(i);
				if (!(propPrefabForBone == null))
				{
					Transform transform = this.ResolveAttachBone(item, i);
					if (transform == null)
					{
						Debug.LogWarning(string.Format("[HideFlashController] No target bone (index {0}) for '{1}'.", i, item.itemName), this);
					}
					else
					{
						GameObject gameObject = Object.Instantiate<GameObject>(propPrefabForBone, transform);
						FirstPersonVisibleProp.Mark(gameObject);
						gameObject.name = propPrefabForBone.name;
						gameObject.transform.localPosition = item.GetPositionOffset(i);
						gameObject.transform.localRotation = Quaternion.Euler(item.GetRotationOffset(i));
						gameObject.transform.localScale = propPrefabForBone.transform.localScale;
						gameObject.SetActive(true);
						HideFlashController.DisableInteractionComponents(gameObject);
						this._spawnedProps.Add(gameObject);
						if (item.itemId == "hideflash_chalkboard")
						{
							this._paintSurface = gameObject.GetComponentInChildren<NeckSignSurface>();
						}
						if (this.debugLogs)
						{
							Debug.Log(string.Format("[HideFlashController] Classic prop '{0}' spawned under '{1}' (scale preserved: {2}).", gameObject.name, transform.name, gameObject.transform.localScale), this);
						}
					}
				}
			}
		}

		// Token: 0x06008A5F RID: 35423 RVA: 0x00389178 File Offset: 0x00387378
		private Transform ResolveAttachBone(HideFlashItemSO item, int index)
		{
			if (item.attachBones != null && item.attachBones.Length > index)
			{
				Transform boneTransform = this.playerAnimator.GetBoneTransform(item.attachBones[index]);
				if (boneTransform != null)
				{
					return boneTransform;
				}
			}
			return this.playerAnimator.GetBoneTransform(HumanBodyBones.Chest);
		}

		// Token: 0x06008A60 RID: 35424 RVA: 0x003891C4 File Offset: 0x003873C4
		private static Transform FindDeep(Transform parent, string name)
		{
			foreach (object obj in parent)
			{
				Transform transform = (Transform)obj;
				if (transform.name == name)
				{
					return transform;
				}
				Transform transform2 = HideFlashController.FindDeep(transform, name);
				if (transform2 != null)
				{
					return transform2;
				}
			}
			return null;
		}

		// Token: 0x06008A61 RID: 35425 RVA: 0x00389240 File Offset: 0x00387440
		private void DestroyProps()
		{
			foreach (GameObject gameObject in this._spawnedProps)
			{
				if (gameObject != null)
				{
					Object.Destroy(gameObject);
				}
			}
			this._spawnedProps.Clear();
			this._spawnedPropAnimators.Clear();
			this._wornDetailAnimator = null;
			this._paintSurface = null;
		}

		// Token: 0x06008A62 RID: 35426 RVA: 0x003892C0 File Offset: 0x003874C0
		private void ResolveWornDetailAnimator(HideFlashItemSO item)
		{
			if (this.playerAnimator == null)
			{
				return;
			}
			if (this._wornDetailAnimator != null && this._wornDetailAnimator.runtimeAnimatorController != null)
			{
				return;
			}
			if (this._wornDetailAnimator == null)
			{
				foreach (Animator animator in this.playerAnimator.GetComponentsInChildren<Animator>(true))
				{
					if (!(animator == this.playerAnimator) && HideFlashController.NameMatchesWorn(item, animator.gameObject.name) && !(animator.GetComponent<SkinnedMeshRenderer>() != null))
					{
						this._wornDetailAnimator = animator;
						break;
					}
				}
			}
			if (this._wornDetailAnimator == null)
			{
				Transform transform = this.FindWornDetailContainer(item);
				if (transform != null)
				{
					this._wornDetailAnimator = transform.GetComponent<Animator>();
					if (this._wornDetailAnimator == null)
					{
						this._wornDetailAnimator = transform.gameObject.AddComponent<Animator>();
					}
				}
			}
			if (this._wornDetailAnimator == null)
			{
				Debug.LogWarning(string.Concat(new string[] { "[HideFlashController] No worn garment '", item.wornObjectName, "' found under the player. '", item.itemName, "' will not deform during the flash." }), this);
				return;
			}
			if (this._wornDetailAnimator.runtimeAnimatorController == null)
			{
				if (item.detailOnlyController == null)
				{
					Debug.LogWarning("[HideFlashController] Worn detail Animator on '" + this._wornDetailAnimator.gameObject.name + "' has no controller and item.detailOnlyController is unassigned.", this);
					return;
				}
				this._wornDetailAnimator.runtimeAnimatorController = item.detailOnlyController;
			}
			this._wornDetailAnimator.applyRootMotion = false;
			this._wornDetailAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
		}

		// Token: 0x06008A63 RID: 35427 RVA: 0x00389468 File Offset: 0x00387668
		private Transform FindWornDetailContainer(HideFlashItemSO item)
		{
			if (this.playerAnimator == null)
			{
				return null;
			}
			foreach (Transform transform in this.playerAnimator.GetComponentsInChildren<Transform>(true))
			{
				if (HideFlashController.NameMatchesWorn(item, transform.name) && !(transform.GetComponent<SkinnedMeshRenderer>() != null) && !(transform.GetComponentInChildren<SkinnedMeshRenderer>(true) == null))
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x06008A64 RID: 35428 RVA: 0x003894D2 File Offset: 0x003876D2
		private static bool NameMatchesWorn(HideFlashItemSO item, string n)
		{
			return !string.IsNullOrEmpty(n) && !(item == null) && !string.IsNullOrEmpty(item.wornObjectName) && (n == item.wornObjectName || n.StartsWith(item.wornObjectName));
		}

		// Token: 0x06008A65 RID: 35429 RVA: 0x00389510 File Offset: 0x00387710
		private void FireWornDetailTrigger(bool open)
		{
			if (this._activeItem == null || !this._activeItem.usesWornDetailAnimator)
			{
				return;
			}
			if (this._wornDetailAnimator == null || this._wornDetailAnimator.runtimeAnimatorController == null)
			{
				this.ResolveWornDetailAnimator(this._activeItem);
			}
			if (this._wornDetailAnimator == null || !this._wornDetailAnimator.isActiveAndEnabled)
			{
				return;
			}
			int num = Animator.StringToHash(this._activeItem.detailStartTrigger);
			int num2 = Animator.StringToHash(this._activeItem.detailEndTrigger);
			this._wornDetailAnimator.ResetTrigger(num);
			this._wornDetailAnimator.ResetTrigger(num2);
			this._wornDetailAnimator.SetTrigger(open ? num : num2);
		}

		// Token: 0x06008A66 RID: 35430 RVA: 0x003895D0 File Offset: 0x003877D0
		private static void DisableInteractionComponents(GameObject go)
		{
			IInteractable[] componentsInChildren = go.GetComponentsInChildren<IInteractable>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				MonoBehaviour monoBehaviour = componentsInChildren[i] as MonoBehaviour;
				if (monoBehaviour != null)
				{
					monoBehaviour.enabled = false;
				}
			}
			InteractablePromptUI[] componentsInChildren2 = go.GetComponentsInChildren<InteractablePromptUI>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].gameObject.SetActive(false);
			}
		}

		// Token: 0x06008A67 RID: 35431 RVA: 0x0038962C File Offset: 0x0038782C
		private void TryStartFlash()
		{
			if (!this._isEquipped || this._isEquipping || !PlayerActivityGate.Allows(PlayerActivity.Flash) || TextInputFocus.BlocksGameplayInput || FlyCamera.IsActive)
			{
				return;
			}
			if (this._isFlashing)
			{
				if (this.debugLogs)
				{
					Debug.Log("[HideFlashController] TryStartFlash: already flashing, skipped.", this);
				}
				return;
			}
			if (this._activeItem == null || this._activeItem.flashActionEntry == null)
			{
				if (this.debugLogs)
				{
					Debug.Log("[HideFlashController] TryStartFlash: no item or action entry.", this);
				}
				return;
			}
			if (this.playerActionAnimator == null)
			{
				Debug.LogWarning("[HideFlashController] PlayerActionAnimator not assigned — cannot trigger flash.", this);
				return;
			}
			this._isFlashing = true;
			this.SetConcealment(false);
			this.StopFlashEndWatch();
			this.RepayEmotionSuppressionDebt();
			this.SetAnimatorBool(HideFlashController._boxFlashActiveHash, "HideFlashActive", true);
			this.FireWornDetailTrigger(true);
			this.FadeHideAndFlashLayer(0f, this.flashStartLayerFadeDelay);
			DetectionEmotionSource detectionEmotionSource = this.emotionSource;
			if (detectionEmotionSource != null)
			{
				detectionEmotionSource.OnFlashExecuted(0f);
			}
			this.PlayFlashSFX(this._activeItem.flashStartSFX);
			float num = this.playerActionAnimator.TriggerAction(this._activeItem.flashActionEntry);
			if (this._activeItem.flashActionEntry.TriggerPrefab != null)
			{
				this._emotionSuppressionDebt++;
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[HideFlashController] Flash: START (HideFlashActive=true, HideAndFlashLayer=0, Duration={0}, IsLoop={1})", num, this._activeItem.flashIsLoop), this);
			}
			if (!this._activeItem.flashIsLoop)
			{
				this._flashRoutine = base.StartCoroutine(this.WaitFlashComplete(num));
			}
		}

		// Token: 0x06008A68 RID: 35432 RVA: 0x003897BA File Offset: 0x003879BA
		private IEnumerator WaitFlashComplete(float duration)
		{
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[HideFlashController] WaitFlashComplete: starting wait for {0}s", duration), this);
			}
			if (duration > 0f)
			{
				yield return new WaitForSeconds(duration);
			}
			this._isFlashing = false;
			this.SetAnimatorBool(HideFlashController._boxFlashActiveHash, "HideFlashActive", false);
			this.FireWornDetailTrigger(false);
			this.PlayFlashSFX((this._activeItem != null) ? this._activeItem.flashEndSFX : null);
			this.BeginFlashEndRestore();
			this._flashRoutine = null;
			if (this.debugLogs)
			{
				Debug.Log("[HideFlashController] WaitFlashComplete: finished, Flash: COMPLETE (HideFlashActive=false, layer restore pending FlashEnd)", this);
			}
			yield break;
		}

		// Token: 0x06008A69 RID: 35433 RVA: 0x003897D0 File Offset: 0x003879D0
		private void BeginFlashEndRestore()
		{
			this.StopFlashEndWatch();
			this._isEndingFlash = true;
			this._flashEndWatchRoutine = base.StartCoroutine(this.RestoreLayerAfterFlashEnd());
			if (!this._isEndingFlash)
			{
				this._flashEndWatchRoutine = null;
			}
			PlayerActionAnimator playerActionAnimator = this.playerActionAnimator;
			if (playerActionAnimator == null)
			{
				return;
			}
			vGenericAction genericAction = playerActionAnimator.GenericAction;
			if (genericAction == null)
			{
				return;
			}
			genericAction.FinishAction();
		}

		// Token: 0x06008A6A RID: 35434 RVA: 0x00389825 File Offset: 0x00387A25
		private void StopFlashEndWatch()
		{
			this._isEndingFlash = false;
			if (this._flashEndWatchRoutine != null)
			{
				base.StopCoroutine(this._flashEndWatchRoutine);
				this._flashEndWatchRoutine = null;
			}
		}

		// Token: 0x06008A6B RID: 35435 RVA: 0x00389849 File Offset: 0x00387A49
		private IEnumerator RestoreLayerAfterFlashEnd()
		{
			float t = 0f;
			bool sawEnd = false;
			while (t < this.flashEndWatchTimeout && !(this.playerAnimator == null) && !HideFlashController.MoveInputPressed())
			{
				if (HideFlashController.IsFlashEndState(this.playerAnimator.GetCurrentAnimatorStateInfo(0)))
				{
					sawEnd = true;
					break;
				}
				t += Time.deltaTime;
				yield return null;
			}
			if (sawEnd)
			{
				while (this.playerAnimator != null)
				{
					AnimatorStateInfo currentAnimatorStateInfo = this.playerAnimator.GetCurrentAnimatorStateInfo(0);
					if (!HideFlashController.IsFlashEndState(currentAnimatorStateInfo) || HideFlashController.MoveInputPressed())
					{
						break;
					}
					float num = 1f / Mathf.Max(this.layerTransitionSpeed, 0.01f) / Mathf.Max(currentAnimatorStateInfo.length, 0.01f);
					float num2 = Mathf.Max(0f, Mathf.Min(this.flashEndLayerRestoreTime, 0.78f - num));
					if (currentAnimatorStateInfo.normalizedTime >= num2)
					{
						break;
					}
					yield return null;
				}
			}
			if (this._isEquipped)
			{
				this.FadeHideAndFlashLayer(1f, 0f);
			}
			float wt = 0f;
			while (this.playerAnimator != null && this._hideAndFlashLayerIndex != -1 && wt < 1f && this.playerAnimator.GetLayerWeight(this._hideAndFlashLayerIndex) < 0.99f)
			{
				wt += Time.deltaTime;
				yield return null;
			}
			if (!this._isFlashing && this._isEquipped)
			{
				PlayerActionAnimator playerActionAnimator = this.playerActionAnimator;
				if (playerActionAnimator != null)
				{
					playerActionAnimator.FinishCurrentAction();
				}
			}
			this.RepayEmotionSuppressionDebt();
			this._flashEndWatchRoutine = null;
			this._isEndingFlash = false;
			if (this._isEquipped && !this._isFlashing)
			{
				this.SetConcealment(true);
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[HideFlashController] FlashEnd watch done (sawEnd={0}), layer restored and action lock released.", sawEnd), this);
			}
			yield break;
		}

		// Token: 0x06008A6C RID: 35436 RVA: 0x00389858 File Offset: 0x00387A58
		private static bool MoveInputPressed()
		{
			return Mathf.Abs(GameplayInput.GetAxisRaw("Horizontal")) > 0.1f || Mathf.Abs(GameplayInput.GetAxisRaw("Vertical")) > 0.1f;
		}

		// Token: 0x06008A6D RID: 35437 RVA: 0x00389888 File Offset: 0x00387A88
		private static bool IsFlashEndState(AnimatorStateInfo info)
		{
			return info.IsName("HideFlash_FlashEnd") || info.IsName("HideFlash_FlashEnd 1");
		}

		// Token: 0x06008A6E RID: 35438 RVA: 0x003898A6 File Offset: 0x00387AA6
		private void PlayFlashSFX(AudioClip clip)
		{
			if (clip == null || this.audioSource == null)
			{
				return;
			}
			this.audioSource.PlayOneShot(clip);
		}

		// Token: 0x06008A6F RID: 35439 RVA: 0x003898CC File Offset: 0x00387ACC
		private void SetAnimatorBool(int hash, string paramName, bool value)
		{
			if (this.playerAnimator == null)
			{
				return;
			}
			AnimatorControllerParameter[] parameters = this.playerAnimator.parameters;
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].nameHash == hash)
				{
					this.playerAnimator.SetBool(hash, value);
					return;
				}
			}
		}

		// Token: 0x06008A70 RID: 35440 RVA: 0x0038991C File Offset: 0x00387B1C
		private void FadeHideAndFlashLayer(float targetWeight, float delay = 0f)
		{
			if (this._hideAndFlashLayerIndex == -1)
			{
				return;
			}
			if (this._hideAndFlashFadeRoutine != null)
			{
				base.StopCoroutine(this._hideAndFlashFadeRoutine);
			}
			if (!base.isActiveAndEnabled)
			{
				if (this.playerAnimator != null)
				{
					this.playerAnimator.SetLayerWeight(this._hideAndFlashLayerIndex, targetWeight);
				}
				this._hideAndFlashFadeRoutine = null;
				return;
			}
			this._hideAndFlashFadeRoutine = base.StartCoroutine(this.LayerFadeRoutine(this._hideAndFlashLayerIndex, targetWeight, delay));
		}

		// Token: 0x06008A71 RID: 35441 RVA: 0x00389994 File Offset: 0x00387B94
		private void FadeHoldingItemLayer(float targetWeight)
		{
			if (this._holdingItemLayerIndex == -1)
			{
				return;
			}
			if (this._holdingItemFadeRoutine != null)
			{
				base.StopCoroutine(this._holdingItemFadeRoutine);
			}
			if (!base.isActiveAndEnabled)
			{
				if (this.playerAnimator != null)
				{
					this.playerAnimator.SetLayerWeight(this._holdingItemLayerIndex, targetWeight);
				}
				this._holdingItemFadeRoutine = null;
				return;
			}
			this._holdingItemFadeRoutine = base.StartCoroutine(this.LayerFadeRoutine(this._holdingItemLayerIndex, targetWeight, 0f));
		}

		// Token: 0x06008A72 RID: 35442 RVA: 0x00389A0D File Offset: 0x00387C0D
		private IEnumerator LayerFadeRoutine(int layerIndex, float targetWeight, float delay = 0f)
		{
			if (this.playerAnimator == null)
			{
				yield break;
			}
			if (delay > 0f)
			{
				yield return new WaitForSeconds(delay);
			}
			float current = this.playerAnimator.GetLayerWeight(layerIndex);
			while (!Mathf.Approximately(current, targetWeight))
			{
				current = Mathf.MoveTowards(current, targetWeight, Time.deltaTime * this.layerTransitionSpeed);
				this.playerAnimator.SetLayerWeight(layerIndex, current);
				yield return null;
			}
			this.playerAnimator.SetLayerWeight(layerIndex, targetWeight);
			yield break;
		}

		// Token: 0x1700160E RID: 5646
		// (get) Token: 0x06008A73 RID: 35443 RVA: 0x00389A31 File Offset: 0x00387C31
		public bool IsEquipped
		{
			get
			{
				return this._isEquipped;
			}
		}

		// Token: 0x1700160F RID: 5647
		// (get) Token: 0x06008A74 RID: 35444 RVA: 0x00389A39 File Offset: 0x00387C39
		public bool IsFlashing
		{
			get
			{
				return this._isFlashing;
			}
		}

		// Token: 0x17001610 RID: 5648
		// (get) Token: 0x06008A75 RID: 35445 RVA: 0x00389A41 File Offset: 0x00387C41
		public bool IsExposureActive
		{
			get
			{
				return this._isFlashing || this._isEndingFlash;
			}
		}

		// Token: 0x17001611 RID: 5649
		// (get) Token: 0x06008A76 RID: 35446 RVA: 0x00389A53 File Offset: 0x00387C53
		public HideFlashItemSO ActiveItem
		{
			get
			{
				return this._activeItem;
			}
		}

		// Token: 0x06008A77 RID: 35447 RVA: 0x00389A5B File Offset: 0x00387C5B
		private void SetConcealment(bool covered)
		{
			PlayerStealthBridge.SetConcealmentSource(this, (covered && this._activeItem != null) ? this._activeItem.coverage : 0f);
		}

		// Token: 0x06008A78 RID: 35448 RVA: 0x00389A86 File Offset: 0x00387C86
		public bool InterruptForDetection()
		{
			return this.InterruptForWorldInteraction();
		}

		// Token: 0x06008A79 RID: 35449 RVA: 0x00389A8E File Offset: 0x00387C8E
		public bool InterruptForWorldInteraction()
		{
			return this.InterruptCover(true);
		}

		// Token: 0x06008A7A RID: 35450 RVA: 0x00389A98 File Offset: 0x00387C98
		public bool CanStowForInventorySwap(PlayerInventoryManager owner, int slotIndex, InventoryItemSO item)
		{
			return owner != null && this.inventoryManager == owner && this._isEquipped && this._activeItem != null && this._activeItem == item && this._heldCoverLease != null && this._heldCoverLease.IsActive && slotIndex == owner.ActiveSlotIndex && owner.GetItemAt(slotIndex) == item && owner.GetActiveItem() == item;
		}

		// Token: 0x06008A7B RID: 35451 RVA: 0x00389B1B File Offset: 0x00387D1B
		public bool TryStowForInventorySwap(PlayerInventoryManager owner, int slotIndex, InventoryItemSO item)
		{
			return this.CanStowForInventorySwap(owner, slotIndex, item) && this.InterruptCover(false);
		}

		// Token: 0x06008A7C RID: 35452 RVA: 0x00389B34 File Offset: 0x00387D34
		private bool InterruptCover(bool clearSelection)
		{
			if (!this._isEquipped || this._activeItem == null)
			{
				return false;
			}
			HideFlashItemSO activeItem = this._activeItem;
			Animator wornDetailAnimator = this._wornDetailAnimator;
			bool flag = activeItem.usesWornDetailAnimator && this.IsExposureActive;
			foreach (GameObject gameObject in this._spawnedProps)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(false);
				}
			}
			if (this.inventoryManager != null && this.inventoryManager.GetActiveSlotItem() == activeItem)
			{
				if (clearSelection)
				{
					this.inventoryManager.SelectSlot(this.inventoryManager.ActiveSlotIndex);
				}
				else
				{
					this.inventoryManager.SuspendActiveItemImmediate();
				}
			}
			this.ForceUnequip();
			if (this._hideAndFlashFadeRoutine != null)
			{
				base.StopCoroutine(this._hideAndFlashFadeRoutine);
				this._hideAndFlashFadeRoutine = null;
			}
			if (this._holdingItemFadeRoutine != null)
			{
				base.StopCoroutine(this._holdingItemFadeRoutine);
				this._holdingItemFadeRoutine = null;
			}
			if (this.playerAnimator != null && this._hideAndFlashLayerIndex != -1)
			{
				this.playerAnimator.SetLayerWeight(this._hideAndFlashLayerIndex, 0f);
			}
			if (flag && wornDetailAnimator != null && wornDetailAnimator.runtimeAnimatorController != null)
			{
				wornDetailAnimator.ResetTrigger(Animator.StringToHash(activeItem.detailStartTrigger));
				wornDetailAnimator.ResetTrigger(Animator.StringToHash(activeItem.detailEndTrigger));
				FlashDetailPose.Complete(wornDetailAnimator, "Flash_End", 0);
			}
			return true;
		}

		// Token: 0x17001612 RID: 5650
		// (get) Token: 0x06008A7D RID: 35453 RVA: 0x00389A39 File Offset: 0x00387C39
		public bool IsEngaged
		{
			get
			{
				return this._isFlashing;
			}
		}

		// Token: 0x06008A7E RID: 35454 RVA: 0x00389CC4 File Offset: 0x00387EC4
		void IFlashGate.SetRemoved()
		{
			this.CancelFlash();
		}

		// Token: 0x06008A7F RID: 35455 RVA: 0x00004F31 File Offset: 0x00003131
		void IFlashGate.SetRestored()
		{
		}

		// Token: 0x06008A80 RID: 35456 RVA: 0x00389CCC File Offset: 0x00387ECC
		public void CancelFlash()
		{
			if (!this._isFlashing)
			{
				if (this.debugLogs)
				{
					Debug.Log("[HideFlashController] CancelFlash: not flashing, skipped.", this);
				}
				return;
			}
			if (this._flashRoutine != null)
			{
				base.StopCoroutine(this._flashRoutine);
				this._flashRoutine = null;
			}
			this._isFlashing = false;
			this.SetAnimatorBool(HideFlashController._boxFlashActiveHash, "HideFlashActive", false);
			this.FireWornDetailTrigger(false);
			this.PlayFlashSFX((this._activeItem != null) ? this._activeItem.flashEndSFX : null);
			this.BeginFlashEndRestore();
			if (this.debugLogs)
			{
				Debug.Log("[HideFlashController] CancelFlash: executed (HideFlashActive=false, layer restore pending FlashEnd).", this);
			}
		}

		// Token: 0x04008D15 RID: 36117
		public HideFlashController.HideFlashEvent OnUnequipped = new HideFlashController.HideFlashEvent();

		// Token: 0x04008D16 RID: 36118
		[Header("References — assign in Inspector")]
		[SerializeField]
		private Animator playerAnimator;

		// Token: 0x04008D17 RID: 36119
		[SerializeField]
		private PlayerInventoryManager inventoryManager;

		// Token: 0x04008D18 RID: 36120
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x04008D19 RID: 36121
		[Tooltip("Same PlayerActionAnimator used by DildoActionExecutor and GameplayEmoteController.")]
		[SerializeField]
		private PlayerActionAnimator playerActionAnimator;

		// Token: 0x04008D1A RID: 36122
		[Tooltip("Optional container for spawned prop visuals (e.g. a child GO of the player). If null, props spawn under the Animator root.")]
		[SerializeField]
		private Transform propContainer;

		// Token: 0x04008D1B RID: 36123
		[Tooltip("Emotion source for Flash/Hide-Flash scoring. Auto-found if not assigned.")]
		[SerializeField]
		private DetectionEmotionSource emotionSource;

		// Token: 0x04008D1C RID: 36124
		[Header("Animator Parameter Names")]
		[Tooltip("Bool set to true while a HideFlash item is equipped.")]
		[SerializeField]
		private string holdingParamName = "HideFlashHolding";

		// Token: 0x04008D1D RID: 36125
		[Header("Animator Layers")]
		[Tooltip("Name of the HideAndFlash layer in the AnimatorController (managed by this controller).")]
		[SerializeField]
		private string hideAndFlashLayerName = "HideAndFlash";

		// Token: 0x04008D1E RID: 36126
		[Tooltip("Name of the HoldingItem layer — forced to 0 while a HideFlash item is active.")]
		[SerializeField]
		private string holdingItemLayerName = "HoldingItem";

		// Token: 0x04008D1F RID: 36127
		[Tooltip("Speed of layer weight transitions (higher = faster). Lower values = smoother/dolce transitions.")]
		[SerializeField]
		private float layerTransitionSpeed = 3f;

		// Token: 0x04008D20 RID: 36128
		[Header("Equip Timing")]
		[Tooltip("Delay in seconds before spawning the prop and activating layers after equip starts. Use 0 for immediate, or match to your pickup animation duration.")]
		[SerializeField]
		private float equipDelay = 0.5f;

		// Token: 0x04008D21 RID: 36129
		[Header("Flash End Smoothing")]
		[Tooltip("Delay before the HideAndFlash layer fades OUT when a flash starts. Should match the trigger's crossFadeTransition (0.25): the full-weight hold pose hides the locomotion->FlashStart crossfade underneath, so the hand (and the held prop) never dips through the locomotion pose.")]
		[SerializeField]
		private float flashStartLayerFadeDelay = 0.25f;

		// Token: 0x04008D22 RID: 36130
		[Tooltip("Latest normalized time of the FlashEnd state at which the HideAndFlash layer starts fading back in. The actual start is moved earlier if needed so the fade COMPLETES before the FlashEnd->Exit transition begins (no locomotion contamination on the hands).")]
		[Range(0f, 1f)]
		[SerializeField]
		private float flashEndLayerRestoreTime = 0.7f;

		// Token: 0x04008D23 RID: 36131
		[Tooltip("Safety timeout: if the Base Layer never reaches a FlashEnd state (exotic action chains), the layer is restored anyway after this many seconds.")]
		[SerializeField]
		private float flashEndWatchTimeout = 3f;

		// Token: 0x04008D24 RID: 36132
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04008D25 RID: 36133
		private HideFlashItemSO _activeItem;

		// Token: 0x04008D26 RID: 36134
		private readonly List<GameObject> _spawnedProps = new List<GameObject>();

		// Token: 0x04008D27 RID: 36135
		private readonly List<Animator> _spawnedPropAnimators = new List<Animator>();

		// Token: 0x04008D28 RID: 36136
		private Animator _wornDetailAnimator;

		// Token: 0x04008D29 RID: 36137
		private Coroutine _flashRoutine;

		// Token: 0x04008D2A RID: 36138
		private Coroutine _hideAndFlashFadeRoutine;

		// Token: 0x04008D2B RID: 36139
		private Coroutine _holdingItemFadeRoutine;

		// Token: 0x04008D2C RID: 36140
		private Coroutine _equipRoutine;

		// Token: 0x04008D2D RID: 36141
		private Coroutine _flashEndWatchRoutine;

		// Token: 0x04008D2E RID: 36142
		private bool _isFlashing;

		// Token: 0x04008D2F RID: 36143
		private bool _isEquipped;

		// Token: 0x04008D30 RID: 36144
		private bool _isEquipping;

		// Token: 0x04008D31 RID: 36145
		private bool _isEndingFlash;

		// Token: 0x04008D32 RID: 36146
		private bool _photoModeSuspended;

		// Token: 0x04008D33 RID: 36147
		private PlayerActivityLease _heldCoverLease;

		// Token: 0x04008D34 RID: 36148
		private InteractionConfigSO _flashHudConfig;

		// Token: 0x04008D35 RID: 36149
		private BodyPaintModeController _paintMode;

		// Token: 0x04008D36 RID: 36150
		private NeckSignSurface _paintSurface;

		// Token: 0x04008D37 RID: 36151
		private const string HeldCoverGateReason = "HideFlash.HeldCover";

		// Token: 0x04008D38 RID: 36152
		private const PlayerActivity HeldCoverBlockedActivities = PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.RapidHUD | PlayerActivity.Interact | PlayerActivity.StanceSwitch | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Phone | PlayerActivity.Crouch | PlayerActivity.ArmsPose;

		// Token: 0x04008D39 RID: 36153
		private int _hideAndFlashLayerIndex = -1;

		// Token: 0x04008D3A RID: 36154
		private int _holdingItemLayerIndex = -1;

		// Token: 0x04008D3B RID: 36155
		private static readonly int _holdingHash = Animator.StringToHash("HideFlashHolding");

		// Token: 0x04008D3C RID: 36156
		private static readonly int _boxMagnitudeHash = Animator.StringToHash("InputMagnitude");

		// Token: 0x04008D3D RID: 36157
		private static readonly int _boxCrouchHash = Animator.StringToHash("IsCrouching");

		// Token: 0x04008D3E RID: 36158
		private static readonly int _boxFlashActiveHash = Animator.StringToHash("HideFlashActive");

		// Token: 0x04008D3F RID: 36159
		private static readonly int _itemTypeHash = Animator.StringToHash("HideFlashItemType");

		// Token: 0x04008D40 RID: 36160
		private int _emotionSuppressionDebt;

		// Token: 0x04008D41 RID: 36161
		private const float FlashEndExitNormalizedTime = 0.78f;

		// Token: 0x020011A6 RID: 4518
		[Serializable]
		public class HideFlashEvent : UnityEvent
		{
		}
	}
}
