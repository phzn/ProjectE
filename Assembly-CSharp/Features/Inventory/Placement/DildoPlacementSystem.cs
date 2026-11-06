using System;
using System.Collections;
using System.Collections.Generic;
using Features.Detection;
using Features.Inventory.Actions;
using Features.PhoneCompanion;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.FirstPerson;
using ProjectExhibitionist.Interactions;
using ProjectExhibitionist.PhotoBooth;
using ProjectExhibitionist.UI;
using UnityEngine;

namespace Features.Inventory.Placement
{
	// Token: 0x0200113B RID: 4411
	[DefaultExecutionOrder(-250)]
	public class DildoPlacementSystem : MonoBehaviour
	{
		// Token: 0x17001530 RID: 5424
		// (get) Token: 0x060085C1 RID: 34241 RVA: 0x003690BD File Offset: 0x003672BD
		private bool UsesTripodCamera
		{
			get
			{
				return this._placementLease != null && this._placementLease.Resources == (PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem);
			}
		}

		// Token: 0x17001531 RID: 5425
		// (get) Token: 0x060085C2 RID: 34242 RVA: 0x003690D8 File Offset: 0x003672D8
		public bool IsPlacing
		{
			get
			{
				return this._isPlacing;
			}
		}

		// Token: 0x17001532 RID: 5426
		// (get) Token: 0x060085C3 RID: 34243 RVA: 0x003690E0 File Offset: 0x003672E0
		// (set) Token: 0x060085C4 RID: 34244 RVA: 0x003690E7 File Offset: 0x003672E7
		public static bool AnyPlacing { get; private set; }

		// Token: 0x17001533 RID: 5427
		// (get) Token: 0x060085C5 RID: 34245 RVA: 0x003690F0 File Offset: 0x003672F0
		public static bool OwnsPlacementInput
		{
			get
			{
				return DildoPlacementSystem.AnyPlacing || (InventoryControllerInput.PlacementHeld && PlayerInventoryManager.Instance != null && PlayerInventoryManager.Instance.GetActiveItem() != null && PlayerInventoryManager.Instance.GetActiveItem().isPlaceable);
			}
		}

		// Token: 0x060085C6 RID: 34246 RVA: 0x00369140 File Offset: 0x00367340
		private void Start()
		{
			if (this.playerInput == null)
			{
				this.playerInput = base.GetComponentInParent<vThirdPersonInput>();
			}
			if (this.playerRoot == null && this.playerInput != null)
			{
				this.playerRoot = this.playerInput.transform;
			}
			this.ResolveFirstPersonController();
		}

		// Token: 0x060085C7 RID: 34247 RVA: 0x0036919A File Offset: 0x0036739A
		private void OnDisable()
		{
			this.InterruptForEncounter();
		}

		// Token: 0x060085C8 RID: 34248 RVA: 0x003691A2 File Offset: 0x003673A2
		public void InterruptForEncounter()
		{
			if (this._isPlacing)
			{
				this.ExitPlacement();
			}
		}

		// Token: 0x060085C9 RID: 34249 RVA: 0x003691B4 File Offset: 0x003673B4
		private void Update()
		{
			if (GraphicsSettingsRuntimeUI.AnyOpen || GameplayInputGate.BlocksGameplayInput)
			{
				return;
			}
			if (this._isPlacing)
			{
				if (this._placementLease == null || !this._placementLease.IsActive || this.inventoryManager == null || this.inventoryManager.GetActiveItem() != this._activeItem)
				{
					this.ExitPlacement();
					return;
				}
				if (TextInputFocus.BlocksGameplayInput || Time.timeScale <= 0f)
				{
					return;
				}
				if (!this.CanContinuePlacement() || InventoryControllerInput.TryCancelPlacement(this) || !InventoryControllerInput.PlacementHeld)
				{
					this.ExitPlacement();
					return;
				}
				this.UpdatePlacement();
				if (InventoryControllerInput.TryConfirmPlacement())
				{
					if (this._lastResult.IsValid)
					{
						this.ConfirmPlacement();
						return;
					}
					this.PlayFailSFX();
					this.ShowPhotoPlacementFeedback(this._activeItem, ControllerInput.IsActive ? "Aim at a flat wall or floor nearby, then press RB while holding LB." : "Aim at a flat wall or floor nearby, then click LMB while holding RMB.");
				}
				return;
			}
			else
			{
				if (this.inventoryManager == null || TextInputFocus.BlocksGameplayInput || Time.timeScale <= 0f)
				{
					return;
				}
				InventoryItemSO activeItem = this.inventoryManager.GetActiveItem();
				if (activeItem != null && activeItem.isPlaceable && InventoryControllerInput.PlacementDown)
				{
					this.EnterPlacement(activeItem);
				}
				return;
			}
		}

		// Token: 0x060085CA RID: 34250 RVA: 0x003692E4 File Offset: 0x003674E4
		private void EnterPlacement(InventoryItemSO item)
		{
			if (this._isPlacing || item == null || !item.isPlaceable || DildoPlacementSystem.AnyPlacing || TextInputFocus.BlocksGameplayInput || Time.timeScale <= 0f)
			{
				return;
			}
			if (!this.TryAcquirePlacementContext())
			{
				this.ShowPhotoPlacementFeedback(item, "Finish the current action before placing a photo.");
				return;
			}
			this._isPlacing = true;
			DildoPlacementSystem.AnyPlacing = true;
			this._activeItem = item;
			this._lastResult = PlacementResult.Invalid;
			this._lastSurfaceType = PlacementSurfaceType.Invalid;
			this.ResolveFirstPersonController();
			this._wasFirstPerson = this.fpController != null && this.fpController.IsFirstPerson;
			if (!this.UsesTripodCamera && this.fpController != null)
			{
				this.fpController.EnterFirstPerson();
			}
			if (this.playerInput != null)
			{
				this.playerInput.SetLockAllInput(false);
				vMeleeCombatInput vMeleeCombatInput = this.playerInput as vMeleeCombatInput;
				if (vMeleeCombatInput != null)
				{
					vMeleeCombatInput.SetLockMeleeInput(true);
				}
			}
			base.StartCoroutine(this.EnsureInputUnlocked());
			this._placementCover = ((this.playerRoot != null) ? this.playerRoot.GetComponent<CoverPoseController>() : null);
			if (this._placementCover != null)
			{
				this._savedCoverEnabled = this._placementCover.enabled;
				this._placementCover.enabled = false;
			}
			this.CacheAnchors();
			this.SpawnGhostForSurface(PlacementSurfaceType.Floor);
			this.DisableRadialMenuInput();
			if (this.inventoryManager != null)
			{
				this.inventoryManager.ToggleHandPrefabVisibility(false);
			}
			this.SuppressPlayerVisualsIfRequested();
			this.UpdatePhotoPlacementHud();
		}

		// Token: 0x060085CB RID: 34251 RVA: 0x00369474 File Offset: 0x00367674
		private void ExitPlacement()
		{
			if (!this._isPlacing)
			{
				return;
			}
			this._isPlacing = false;
			DildoPlacementSystem.AnyPlacing = false;
			base.StopAllCoroutines();
			if (!this.UsesTripodCamera && !this._wasFirstPerson && this.fpController != null)
			{
				this.fpController.ExitFirstPerson();
			}
			if (this._placementCover != null && this._savedCoverEnabled)
			{
				this._placementCover.enabled = true;
			}
			this._placementCover = null;
			this._savedCoverEnabled = false;
			if (this.ghost != null)
			{
				this.ghost.Destroy();
			}
			this.RestoreRadialMenuInput();
			this.RestorePlayerVisuals();
			if (this.inventoryManager != null)
			{
				this.inventoryManager.ToggleHandPrefabVisibility(true);
			}
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance != null)
			{
				instance.HideIfCurrent(this._photoPlacementConfig);
			}
			this._activeItem = null;
			this.ReleasePlacementContext();
		}

		// Token: 0x060085CC RID: 34252 RVA: 0x0036955C File Offset: 0x0036775C
		private bool TryAcquirePlacementContext()
		{
			if (this._placementLease != null && this._placementLease.IsActive)
			{
				return false;
			}
			PlayerContextResource playerContextResource = (SmartphoneController.IsStableTripodCameraOwner ? (PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem) : (PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem));
			if (!PlayerActivityGate.TryAcquire("Inventory.Placement", this, PlayerActivity.Interact, playerContextResource, PlayerActivity.AllPlayerActions, out this._placementLease, false, null))
			{
				return false;
			}
			if (this.playerInput != null)
			{
				this._savedInputLock = this.playerInput.lockInput;
				this._savedCameraInputLock = this.playerInput.lockCameraInput;
				this._savedMoveInputLock = this.playerInput.lockMoveInput;
				vMeleeCombatInput vMeleeCombatInput = this.playerInput as vMeleeCombatInput;
				if (vMeleeCombatInput != null)
				{
					this._savedMeleeInputLock = vMeleeCombatInput.lockMeleeInput;
				}
				this._placementController = this.playerInput.cc;
				if (this._placementController != null)
				{
					this._savedMovementLock = this._placementController.lockMovement;
					this._savedRotationLock = this._placementController.lockRotation;
				}
			}
			return true;
		}

		// Token: 0x060085CD RID: 34253 RVA: 0x00369650 File Offset: 0x00367850
		private bool CanContinuePlacement()
		{
			if (this._placementLease != null && this._placementLease.IsActive)
			{
				PlayerActivity playerActivity = PlayerActivity.Interact;
				PlayerContextResource resources = this._placementLease.Resources;
				Predicate<Object> predicate;
				if ((predicate = this._placementOwnerPredicate) == null)
				{
					predicate = (this._placementOwnerPredicate = new Predicate<Object>(this.IsPlacementOwner));
				}
				return PlayerActivityGate.CanAcquireAfterReleasing(playerActivity, resources, predicate);
			}
			return false;
		}

		// Token: 0x060085CE RID: 34254 RVA: 0x003696A5 File Offset: 0x003678A5
		private bool IsPlacementOwner(Object owner)
		{
			return owner == this;
		}

		// Token: 0x060085CF RID: 34255 RVA: 0x003696B0 File Offset: 0x003678B0
		private void ReleasePlacementContext()
		{
			if (this._placementLease == null)
			{
				return;
			}
			if (this.playerInput != null)
			{
				this.playerInput.SetLockAllInput(this._savedInputLock);
				if (!this.UsesTripodCamera)
				{
					this.playerInput.lockCameraInput = this._savedCameraInputLock;
				}
				this.playerInput.lockMoveInput = this._savedMoveInputLock;
				vMeleeCombatInput vMeleeCombatInput = this.playerInput as vMeleeCombatInput;
				if (vMeleeCombatInput != null)
				{
					vMeleeCombatInput.SetLockMeleeInput(this._savedMeleeInputLock);
				}
			}
			if (this._placementController != null)
			{
				this._placementController.lockMovement = this._savedMovementLock;
				this._placementController.lockRotation = this._savedRotationLock;
				this._placementController = null;
			}
			this._placementLease.Dispose();
			this._placementLease = null;
		}

		// Token: 0x060085D0 RID: 34256 RVA: 0x00369774 File Offset: 0x00367974
		private void SuppressPlayerVisualsIfRequested()
		{
			this.RestorePlayerVisuals();
			if (!this.UsesTripodCamera)
			{
				IInventoryPlacementPlayerVisibilityProvider inventoryPlacementPlayerVisibilityProvider = this._activeItem as IInventoryPlacementPlayerVisibilityProvider;
				if (inventoryPlacementPlayerVisibilityProvider != null && inventoryPlacementPlayerVisibilityProvider.HidePlayerVisualsDuringPlacement && !(this.playerRoot == null))
				{
					foreach (Renderer renderer in this.playerRoot.GetComponentsInChildren<Renderer>(true))
					{
						if (!(renderer == null))
						{
							this._suppressedPlayerRenderers.Add(renderer);
							this._savedPlayerForceRenderingOff.Add(renderer.forceRenderingOff);
							renderer.forceRenderingOff = true;
						}
					}
					return;
				}
			}
		}

		// Token: 0x060085D1 RID: 34257 RVA: 0x00369804 File Offset: 0x00367A04
		private void RestorePlayerVisuals()
		{
			int num = Mathf.Min(this._suppressedPlayerRenderers.Count, this._savedPlayerForceRenderingOff.Count);
			for (int i = 0; i < num; i++)
			{
				Renderer renderer = this._suppressedPlayerRenderers[i];
				if (renderer != null)
				{
					renderer.forceRenderingOff = this._savedPlayerForceRenderingOff[i];
				}
			}
			this._suppressedPlayerRenderers.Clear();
			this._savedPlayerForceRenderingOff.Clear();
		}

		// Token: 0x060085D2 RID: 34258 RVA: 0x00369878 File Offset: 0x00367A78
		private void DisableRadialMenuInput()
		{
			this._suppressedRadialInputs.Clear();
			foreach (UltimateRadialMenuInputManager ultimateRadialMenuInputManager in Object.FindObjectsOfType<UltimateRadialMenuInputManager>(true))
			{
				if (!(ultimateRadialMenuInputManager == null) && ultimateRadialMenuInputManager.enabled)
				{
					ultimateRadialMenuInputManager.enabled = false;
					this._suppressedRadialInputs.Add(ultimateRadialMenuInputManager);
				}
			}
		}

		// Token: 0x060085D3 RID: 34259 RVA: 0x003698D0 File Offset: 0x00367AD0
		private void RestoreRadialMenuInput()
		{
			foreach (UltimateRadialMenuInputManager ultimateRadialMenuInputManager in this._suppressedRadialInputs)
			{
				if (ultimateRadialMenuInputManager != null)
				{
					ultimateRadialMenuInputManager.enabled = true;
				}
			}
			this._suppressedRadialInputs.Clear();
		}

		// Token: 0x060085D4 RID: 34260 RVA: 0x00369938 File Offset: 0x00367B38
		private void UpdatePlacement()
		{
			Camera main = Camera.main;
			if (main == null || this.surfaceDetector == null || this.playerRoot == null)
			{
				return;
			}
			this._lastResult = this.DetectPlacementSurface(main);
			if (this._lastResult.SurfaceType != this._lastSurfaceType)
			{
				this._lastSurfaceType = this._lastResult.SurfaceType;
				this.SpawnGhostForSurface(this._lastResult.SurfaceType);
			}
			if (this._lastResult.IsValid && !this.HasPrefabForSurface(this._lastResult.SurfaceType))
			{
				this._lastResult.IsValid = false;
			}
			if (this._lastResult.IsValid && !MovingPlacementSupport.Allows(this._activeItem, this._lastResult.SurfaceTransform))
			{
				this._lastResult.IsValid = false;
			}
			this.ApplyAnchorToResult(ref this._lastResult);
			this.ApplyItemPlacementPose(ref this._lastResult);
			if (this._lastResult.IsValid)
			{
				IInventoryPlacementAreaProvider inventoryPlacementAreaProvider = this._activeItem as IInventoryPlacementAreaProvider;
				if (inventoryPlacementAreaProvider != null && !inventoryPlacementAreaProvider.IsPlacementAreaValid(this._lastResult.HitPosition, this._lastResult.HitNormal, this._lastResult.SurfaceType == PlacementSurfaceType.Wall, this.playerRoot))
				{
					this._lastResult.IsValid = false;
				}
			}
			this.ghost.UpdateTarget(this._lastResult.Position, this._lastResult.Rotation, this._lastResult.IsValid, this._lastResult.SurfaceTransform);
			this.ghost.SetVisible(this._lastResult.IsValid);
			this.UpdatePhotoPlacementHud();
		}

		// Token: 0x060085D5 RID: 34261 RVA: 0x00369AD8 File Offset: 0x00367CD8
		private PlacementResult DetectPlacementSurface(Camera cam)
		{
			Vector3? vector2;
			if (!this.UsesTripodCamera)
			{
				PlacementSurfaceDetector placementSurfaceDetector = this.surfaceDetector;
				LayerMask layerMask = this.placementLayerMask;
				float num = this.maxPlacementDistance;
				Transform transform = this.playerRoot;
				Vector3? vector = new Vector3?(cam.transform.position);
				vector2 = null;
				return placementSurfaceDetector.DetectSurface(cam, layerMask, num, transform, vector, vector2);
			}
			Vector3 vector3 = Vector3.ProjectOnPlane(this.playerRoot.forward, Vector3.up).normalized;
			if (vector3.sqrMagnitude < 0.001f)
			{
				vector3 = Vector3.forward;
			}
			PlacementSurfaceDetector placementSurfaceDetector2 = this.surfaceDetector;
			LayerMask layerMask2 = this.placementLayerMask;
			float num2 = this.maxPlacementDistance;
			Transform transform2 = this.playerRoot;
			vector2 = new Vector3?(vector3 - Vector3.up * 0.55f);
			return placementSurfaceDetector2.DetectSurface(cam, layerMask2, num2, transform2, null, vector2);
		}

		// Token: 0x060085D6 RID: 34262 RVA: 0x00369B9C File Offset: 0x00367D9C
		private void UpdatePhotoPlacementHud()
		{
			if (!(this._activeItem is PhotoBoothPhotoItemSO))
			{
				return;
			}
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance == null)
			{
				return;
			}
			if (this._photoPlacementConfig == null)
			{
				this._photoPlacementConfig = ScriptableObject.CreateInstance<InteractionConfigSO>();
				this._photoPlacementConfig.hideFlags = HideFlags.HideAndDontSave;
				this._photoPlacementConfig.objectName = "Photo Booth Photos";
				this._photoPlacementConfig.usableActions.Add(new UsableAction
				{
					actionName = "Aim at Wall / Floor",
					keyDisplayLabel = "HOLD RMB"
				});
				this._photoPlacementConfig.usableActions.Add(new UsableAction
				{
					keyDisplayLabel = "LMB"
				});
				this._photoPlacementConfig.usableActions.Add(new UsableAction
				{
					actionName = "Cancel Placement",
					keyDisplayLabel = "RELEASE RMB"
				});
			}
			this._photoPlacementConfig.usableActions[1].actionName = ((!this._lastResult.IsValid) ? "Find a Flat Surface Nearby" : ((this._lastResult.SurfaceType == PlacementSurfaceType.Wall) ? "Hang Selected Photo" : "Place Photo Stack"));
			this._photoPlacementConfig.usableActions[0].keyDisplayLabel = (ControllerInput.IsActive ? "HOLD LB" : "HOLD RMB");
			this._photoPlacementConfig.usableActions[1].keyDisplayLabel = (ControllerInput.IsActive ? "RB" : "LMB");
			this._photoPlacementConfig.usableActions[2].keyDisplayLabel = (ControllerInput.IsActive ? "B / RELEASE LB" : "RELEASE RMB");
			ControllerContextBindings.MarkControllerLabels(this._photoPlacementConfig, ControllerInput.IsActive);
			instance.Show(this._photoPlacementConfig);
		}

		// Token: 0x060085D7 RID: 34263 RVA: 0x00369D51 File Offset: 0x00367F51
		private void ShowPhotoPlacementFeedback(InventoryItemSO item, string message)
		{
			if (!(item is PhotoBoothPhotoItemSO) || Time.unscaledTime - this._photoFeedbackLastTime < 1f)
			{
				return;
			}
			this._photoFeedbackLastTime = Time.unscaledTime;
			ToastService.Show(message, "Photo Booth", null, -1f, "photobooth_ui");
		}

		// Token: 0x060085D8 RID: 34264 RVA: 0x00369D90 File Offset: 0x00367F90
		private void OnDestroy()
		{
			if (this._photoPlacementConfig == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(this._photoPlacementConfig);
				return;
			}
			Object.DestroyImmediate(this._photoPlacementConfig);
		}

		// Token: 0x060085D9 RID: 34265 RVA: 0x00369DBF File Offset: 0x00367FBF
		private bool HasPrefabForSurface(PlacementSurfaceType surface)
		{
			if (this._activeItem == null)
			{
				return false;
			}
			if (surface != PlacementSurfaceType.Wall)
			{
				return this._activeItem.floorPrefab != null;
			}
			return this._activeItem.wallPrefab != null;
		}

		// Token: 0x060085DA RID: 34266 RVA: 0x00369DF8 File Offset: 0x00367FF8
		private void ConfirmPlacement()
		{
			if (this._activeItem == null || !this._lastResult.IsValid || !MovingPlacementSupport.Allows(this._activeItem, this._lastResult.SurfaceTransform))
			{
				return;
			}
			GameObject gameObject = ((this._lastResult.SurfaceType == PlacementSurfaceType.Wall) ? this._activeItem.wallPrefab : this._activeItem.floorPrefab);
			if (gameObject == null)
			{
				return;
			}
			GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, this._lastResult.Position, this._lastResult.Rotation, (this._lastResult.SurfaceTransform != null) ? null : this.placedObjectsContainer);
			if (this._lastResult.SurfaceTransform != null)
			{
				gameObject2.transform.SetParent(this._lastResult.SurfaceTransform, true);
			}
			gameObject2.name = string.Format("Placed_{0}_{1}", this._activeItem.itemName, this._lastResult.SurfaceType);
			InventoryItemSO inventoryItemSO = this._activeItem;
			bool flag = this._lastResult.SurfaceType == PlacementSurfaceType.Floor && this._activeItem is IInventoryWholeStackWorldProvider;
			IInventoryWholeStackWorldProvider inventoryWholeStackWorldProvider = (flag ? (this._activeItem as IInventoryWholeStackWorldProvider) : null);
			IInventoryWorldInstanceProvider inventoryWorldInstanceProvider = ((inventoryWholeStackWorldProvider == null) ? (this._activeItem as IInventoryWorldInstanceProvider) : null);
			if (inventoryWholeStackWorldProvider != null)
			{
				inventoryItemSO = inventoryWholeStackWorldProvider.PrepareWholeStackWorldInstance(gameObject2);
				if (inventoryItemSO == null)
				{
					Object.Destroy(gameObject2);
					return;
				}
			}
			else if (inventoryWorldInstanceProvider != null)
			{
				inventoryItemSO = inventoryWorldInstanceProvider.PrepareWorldInstance(gameObject2);
				if (inventoryItemSO == null)
				{
					Object.Destroy(gameObject2);
					return;
				}
			}
			else
			{
				IInventoryInstanceVisualProvider inventoryInstanceVisualProvider = this._activeItem as IInventoryInstanceVisualProvider;
				if (inventoryInstanceVisualProvider != null)
				{
					inventoryInstanceVisualProvider.ConfigureWorldInstance(gameObject2);
				}
			}
			IInventorySurfaceConformProvider inventorySurfaceConformProvider = inventoryItemSO as IInventorySurfaceConformProvider;
			if (inventorySurfaceConformProvider != null)
			{
				inventorySurfaceConformProvider.ConformWorldInstanceToSurface(gameObject2, this._lastResult.HitPosition, this._lastResult.HitNormal, this._lastResult.SurfaceType == PlacementSurfaceType.Wall);
			}
			PlacedDildoInteractable placedDildoInteractable = gameObject2.GetComponent<PlacedDildoInteractable>();
			if (placedDildoInteractable == null)
			{
				placedDildoInteractable = gameObject2.AddComponent<PlacedDildoInteractable>();
			}
			InteractablePromptUI componentInChildren = gameObject2.GetComponentInChildren<InteractablePromptUI>(true);
			Transform transform = ((this.playerInput != null) ? this.playerInput.transform : this.playerRoot);
			List<DildoActionDefinition> list = (inventoryItemSO.supportsPlacedActions ? this.allDildoActions : null);
			placedDildoInteractable.Initialize(list, this._lastResult.SurfaceType, null, this.actionExecutor, inventoryItemSO, componentInChildren, transform);
			if (this._lastResult.SurfaceTransform != null)
			{
				gameObject2.AddComponent<MovingPlacedItem>().Initialize(this._lastResult.SurfaceTransform);
			}
			if (this._lastResult.SurfaceType == PlacementSurfaceType.Wall)
			{
				PhotoBoothPhotoItemSO photoBoothPhotoItemSO = inventoryItemSO as PhotoBoothPhotoItemSO;
				if (photoBoothPhotoItemSO != null)
				{
					PhotoBoothHomePlacedPhotoPersistence.RegisterNew(gameObject2, photoBoothPhotoItemSO);
				}
			}
			if (inventoryWorldInstanceProvider != null)
			{
				inventoryWorldInstanceProvider.CommitWorldInstance();
			}
			if (this.placementSuccessSFX != null)
			{
				this.audioSource.PlayOneShot(this.placementSuccessSFX);
			}
			if (flag)
			{
				this.inventoryManager.DropEntireActiveItemStack();
			}
			else
			{
				this.inventoryManager.ConsumeActiveItem();
			}
			bool flag2 = inventoryWorldInstanceProvider != null && this.inventoryManager.GetActiveItem() != null;
			this.ExitPlacement();
			if (flag2)
			{
				this.inventoryManager.RespawnActiveHandObject();
			}
		}

		// Token: 0x060085DB RID: 34267 RVA: 0x0036A108 File Offset: 0x00368308
		private IEnumerator EnsureInputUnlocked()
		{
			int i = 0;
			while (i < 5 && this._isPlacing && !(this.playerInput == null) && this.CanContinuePlacement())
			{
				if (Time.timeScale <= 0f || TextInputFocus.BlocksGameplayInput || GraphicsSettingsRuntimeUI.AnyOpen)
				{
					yield return null;
				}
				else
				{
					if (!this.UsesTripodCamera)
					{
						this.ResolveFirstPersonController();
						if (this.fpController != null && !this.fpController.IsFirstPerson)
						{
							this.fpController.EnterFirstPerson();
						}
					}
					this.playerInput.lockInput = false;
					if (!this.UsesTripodCamera)
					{
						this.playerInput.lockCameraInput = false;
					}
					this.playerInput.lockMoveInput = false;
					if (this.playerInput.cc != null)
					{
						this.playerInput.cc.lockMovement = false;
						this.playerInput.cc.lockRotation = false;
					}
					yield return null;
				}
				int num = i;
				i = num + 1;
			}
			yield break;
		}

		// Token: 0x060085DC RID: 34268 RVA: 0x0036A118 File Offset: 0x00368318
		private void ResolveFirstPersonController()
		{
			if (this.fpController != null)
			{
				return;
			}
			if (this.playerInput != null)
			{
				this.fpController = this.playerInput.GetComponent<FirstPersonController>();
				if (this.fpController == null)
				{
					this.fpController = this.playerInput.GetComponentInParent<FirstPersonController>();
				}
				if (this.fpController == null)
				{
					this.fpController = this.playerInput.GetComponentInChildren<FirstPersonController>(true);
				}
			}
			if (this.fpController == null && this.playerRoot != null)
			{
				this.fpController = this.playerRoot.GetComponent<FirstPersonController>();
				if (this.fpController == null)
				{
					this.fpController = this.playerRoot.GetComponentInChildren<FirstPersonController>(true);
				}
			}
		}

		// Token: 0x060085DD RID: 34269 RVA: 0x0036A1E0 File Offset: 0x003683E0
		private void ApplyItemPlacementPose(ref PlacementResult result)
		{
			if (!result.IsValid || this._activeItem == null)
			{
				return;
			}
			bool flag = result.SurfaceType == PlacementSurfaceType.Wall;
			if (flag && result.SurfaceTransform == null)
			{
				IInventoryPlacementHeightProvider inventoryPlacementHeightProvider = this._activeItem as IInventoryPlacementHeightProvider;
				if (inventoryPlacementHeightProvider != null && inventoryPlacementHeightProvider.UseSurfaceHitHeight)
				{
					float num = result.FloorPosition.y + Mathf.Max(0f, inventoryPlacementHeightProvider.MinimumHeightAboveFloor);
					float num2 = result.FloorPosition.y + Mathf.Max(inventoryPlacementHeightProvider.MinimumHeightAboveFloor, inventoryPlacementHeightProvider.MaximumHeightAboveFloor);
					result.Position = new Vector3(result.HitPosition.x, Mathf.Clamp(result.HitPosition.y, num, num2), result.HitPosition.z);
				}
			}
			IInventoryPlacementPoseProvider inventoryPlacementPoseProvider = this._activeItem as IInventoryPlacementPoseProvider;
			if (inventoryPlacementPoseProvider != null)
			{
				result.Position = inventoryPlacementPoseProvider.AdjustPlacementPosition(result.Position, result.HitNormal, flag);
				result.Rotation = inventoryPlacementPoseProvider.AdjustPlacementRotation(result.Rotation, flag);
			}
		}

		// Token: 0x060085DE RID: 34270 RVA: 0x0036A2E8 File Offset: 0x003684E8
		private void PlayFailSFX()
		{
			if (Time.time - this._failSFXLastPlayedTime < this.failSFXCooldown)
			{
				return;
			}
			if (this.placementFailSFX != null)
			{
				this.audioSource.PlayOneShot(this.placementFailSFX);
			}
			this._failSFXLastPlayedTime = Time.time;
		}

		// Token: 0x060085DF RID: 34271 RVA: 0x0036A334 File Offset: 0x00368534
		private void SpawnGhostForSurface(PlacementSurfaceType surface)
		{
			if (this._activeItem == null)
			{
				return;
			}
			GameObject gameObject = ((surface == PlacementSurfaceType.Wall) ? this._activeItem.wallPrefab : this._activeItem.floorPrefab);
			if (gameObject == null)
			{
				return;
			}
			IInventoryPlacementSilhouetteProvider inventoryPlacementSilhouetteProvider = this._activeItem as IInventoryPlacementSilhouetteProvider;
			if (inventoryPlacementSilhouetteProvider != null)
			{
				this.ghost.SpawnSilhouette(inventoryPlacementSilhouetteProvider.PlacementSilhouetteWorldSize);
				return;
			}
			Vector3? vector = null;
			IInventoryPlacementScaleProvider inventoryPlacementScaleProvider = this._activeItem as IInventoryPlacementScaleProvider;
			if (inventoryPlacementScaleProvider != null)
			{
				vector = new Vector3?(inventoryPlacementScaleProvider.PlacementPreviewWorldScale);
			}
			IInventoryCenteredPlacementPreviewProvider inventoryCenteredPlacementPreviewProvider = this._activeItem as IInventoryCenteredPlacementPreviewProvider;
			bool flag = inventoryCenteredPlacementPreviewProvider != null && inventoryCenteredPlacementPreviewProvider.CenterPlacementPreviewOnVisual;
			this.ghost.Spawn(gameObject, this.ghostMaterial, vector, flag);
		}

		// Token: 0x060085E0 RID: 34272 RVA: 0x0036A3F0 File Offset: 0x003685F0
		private void CacheAnchors()
		{
			if (this._activeItem == null)
			{
				return;
			}
			this._hasFloorAnchor = this.CacheAnchor(this._activeItem.floorPrefab, out this._floorAnchorLocalPos, out this._floorAnchorLocalRot);
			this._hasWallAnchor = this.CacheAnchor(this._activeItem.wallPrefab, out this._wallAnchorLocalPos, out this._wallAnchorLocalRot);
		}

		// Token: 0x060085E1 RID: 34273 RVA: 0x0036A454 File Offset: 0x00368654
		private bool CacheAnchor(GameObject prefab, out Vector3 pos, out Quaternion rot)
		{
			pos = Vector3.zero;
			rot = Quaternion.identity;
			if (prefab == null)
			{
				return false;
			}
			PlacementAnchor componentInChildren = prefab.GetComponentInChildren<PlacementAnchor>(true);
			if (componentInChildren == null)
			{
				return false;
			}
			pos = componentInChildren.LocalPosition;
			rot = componentInChildren.LocalRotation;
			return true;
		}

		// Token: 0x060085E2 RID: 34274 RVA: 0x0036A4B0 File Offset: 0x003686B0
		public void EnterPlacementMode()
		{
			if (this._isPlacing || this.inventoryManager == null)
			{
				return;
			}
			InventoryItemSO activeItem = this.inventoryManager.GetActiveItem();
			if (activeItem != null && activeItem.isPlaceable)
			{
				this.EnterPlacement(activeItem);
			}
		}

		// Token: 0x060085E3 RID: 34275 RVA: 0x0036A4F8 File Offset: 0x003686F8
		private void ApplyAnchorToResult(ref PlacementResult result)
		{
			if (!((result.SurfaceType == PlacementSurfaceType.Wall) ? this._hasWallAnchor : this._hasFloorAnchor))
			{
				return;
			}
			Vector3 vector = ((result.SurfaceType == PlacementSurfaceType.Wall) ? this._wallAnchorLocalPos : this._floorAnchorLocalPos);
			Quaternion quaternion = ((result.SurfaceType == PlacementSurfaceType.Wall) ? this._wallAnchorLocalRot : this._floorAnchorLocalRot);
			Quaternion quaternion2 = result.Rotation * Quaternion.Inverse(quaternion);
			result.Position -= quaternion2 * vector;
			result.Rotation = quaternion2;
		}

		// Token: 0x04008869 RID: 34921
		[Header("Internal References")]
		[SerializeField]
		private PlacementSurfaceDetector surfaceDetector;

		// Token: 0x0400886A RID: 34922
		[SerializeField]
		private PlacementGhost ghost;

		// Token: 0x0400886B RID: 34923
		[SerializeField]
		private AudioSource audioSource;

		// Token: 0x0400886C RID: 34924
		[Header("External References (Explicit)")]
		[SerializeField]
		private Transform playerRoot;

		// Token: 0x0400886D RID: 34925
		[SerializeField]
		private vThirdPersonInput playerInput;

		// Token: 0x0400886E RID: 34926
		[SerializeField]
		private FirstPersonController fpController;

		// Token: 0x0400886F RID: 34927
		[SerializeField]
		private PlayerInventoryManager inventoryManager;

		// Token: 0x04008870 RID: 34928
		[SerializeField]
		private DildoActionExecutor actionExecutor;

		// Token: 0x04008871 RID: 34929
		[Header("Data")]
		[SerializeField]
		private List<DildoActionDefinition> allDildoActions = new List<DildoActionDefinition>();

		// Token: 0x04008872 RID: 34930
		[Header("Settings")]
		[SerializeField]
		private LayerMask placementLayerMask;

		// Token: 0x04008873 RID: 34931
		[SerializeField]
		private float maxPlacementDistance = 5f;

		// Token: 0x04008874 RID: 34932
		[SerializeField]
		private Material ghostMaterial;

		// Token: 0x04008875 RID: 34933
		[SerializeField]
		private Transform placedObjectsContainer;

		// Token: 0x04008876 RID: 34934
		[Header("Audio")]
		[SerializeField]
		private AudioClip placementSuccessSFX;

		// Token: 0x04008877 RID: 34935
		[SerializeField]
		private AudioClip placementFailSFX;

		// Token: 0x04008878 RID: 34936
		[SerializeField]
		private float failSFXCooldown = 0.5f;

		// Token: 0x04008879 RID: 34937
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x0400887A RID: 34938
		private bool _isPlacing;

		// Token: 0x0400887B RID: 34939
		private InventoryItemSO _activeItem;

		// Token: 0x0400887C RID: 34940
		private PlacementResult _lastResult;

		// Token: 0x0400887D RID: 34941
		private float _failSFXLastPlayedTime = -999f;

		// Token: 0x0400887E RID: 34942
		private PlacementSurfaceType _lastSurfaceType;

		// Token: 0x0400887F RID: 34943
		private PlayerActivityLease _placementLease;

		// Token: 0x04008880 RID: 34944
		private Predicate<Object> _placementOwnerPredicate;

		// Token: 0x04008881 RID: 34945
		private const PlayerContextResource PlacementResources = PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem;

		// Token: 0x04008882 RID: 34946
		private const PlayerContextResource TripodPlacementResources = PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.FullBodyAnimation | PlayerContextResource.HeldItem;

		// Token: 0x04008883 RID: 34947
		private bool _savedInputLock;

		// Token: 0x04008884 RID: 34948
		private bool _savedCameraInputLock;

		// Token: 0x04008885 RID: 34949
		private bool _savedMoveInputLock;

		// Token: 0x04008886 RID: 34950
		private bool _savedMeleeInputLock;

		// Token: 0x04008887 RID: 34951
		private bool _savedMovementLock;

		// Token: 0x04008888 RID: 34952
		private bool _savedRotationLock;

		// Token: 0x04008889 RID: 34953
		private bool _wasFirstPerson;

		// Token: 0x0400888A RID: 34954
		private vThirdPersonController _placementController;

		// Token: 0x0400888B RID: 34955
		private CoverPoseController _placementCover;

		// Token: 0x0400888C RID: 34956
		private bool _savedCoverEnabled;

		// Token: 0x0400888D RID: 34957
		private InteractionConfigSO _photoPlacementConfig;

		// Token: 0x0400888E RID: 34958
		private float _photoFeedbackLastTime = -999f;

		// Token: 0x0400888F RID: 34959
		private readonly List<UltimateRadialMenuInputManager> _suppressedRadialInputs = new List<UltimateRadialMenuInputManager>();

		// Token: 0x04008890 RID: 34960
		private readonly List<Renderer> _suppressedPlayerRenderers = new List<Renderer>();

		// Token: 0x04008891 RID: 34961
		private readonly List<bool> _savedPlayerForceRenderingOff = new List<bool>();

		// Token: 0x04008892 RID: 34962
		private bool _hasFloorAnchor;

		// Token: 0x04008893 RID: 34963
		private Vector3 _floorAnchorLocalPos;

		// Token: 0x04008894 RID: 34964
		private Quaternion _floorAnchorLocalRot;

		// Token: 0x04008895 RID: 34965
		private bool _hasWallAnchor;

		// Token: 0x04008896 RID: 34966
		private Vector3 _wallAnchorLocalPos;

		// Token: 0x04008897 RID: 34967
		private Quaternion _wallAnchorLocalRot;
	}
}
