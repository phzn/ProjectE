using System;
using System.Collections;
using System.Collections.Generic;
using Features.Inventory;
using Features.Inventory.Placement;
using Features.WardrobeFeedback;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.FirstPerson;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace ProjectExhibitionist.PhotoBooth
{
	// Token: 0x02000991 RID: 2449
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-50)]
	public sealed class PhotoBoothPrintHolder : MonoBehaviour
	{
		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x06004407 RID: 17415 RVA: 0x0019773C File Offset: 0x0019593C
		public static bool ControllerOwnsPhotoPan
		{
			get
			{
				return PhotoBoothPrintHolder._activePhotoViewer != null && PhotoBoothPrintHolder._activePhotoViewer.IsHoldingPrint && PhotoBoothPrintHolder._activePhotoViewer._heldInventoryItem != null && PhotoBoothPrintHolder._activePhotoViewer._photoViewZoomActive && PhotoBoothPrintHolder._activePhotoViewer._firstPersonController != null && PhotoBoothPrintHolder._activePhotoViewer._firstPersonController.IsFirstPerson && !DildoPlacementSystem.AnyPlacing && ControllerInput.IsActive && !ControllerInput.Modifier && ControllerInput.GetButton(ControllerButton.RightTrigger) && ControllerGameplayBridge.AllowsAxes && !ControllerWorldPointer.BlocksContextInput;
			}
		}

		// Token: 0x06004408 RID: 17416 RVA: 0x001977D1 File Offset: 0x001959D1
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetPhotoViewer()
		{
			PhotoBoothPrintHolder._activePhotoViewer = null;
		}

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x06004409 RID: 17417 RVA: 0x001977D9 File Offset: 0x001959D9
		public bool IsHoldingPrint
		{
			get
			{
				return this._heldPrint != null;
			}
		}

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x0600440A RID: 17418 RVA: 0x001977E7 File Offset: 0x001959E7
		public PhotoBoothPrint HeldPrint
		{
			get
			{
				return this._heldPrint;
			}
		}

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x0600440B RID: 17419 RVA: 0x001977EF File Offset: 0x001959EF
		public float PoseWeight
		{
			get
			{
				return this._poseWeight;
			}
		}

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x0600440C RID: 17420 RVA: 0x001977F7 File Offset: 0x001959F7
		public int AnimationLayerIndex
		{
			get
			{
				return this._layerIndex;
			}
		}

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x0600440D RID: 17421 RVA: 0x001977FF File Offset: 0x001959FF
		public Vector3 LeftGripTarget
		{
			get
			{
				return this._leftGripTarget;
			}
		}

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x0600440E RID: 17422 RVA: 0x00197807 File Offset: 0x00195A07
		public Vector3 RightGripTarget
		{
			get
			{
				return this._rightGripTarget;
			}
		}

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x0600440F RID: 17423 RVA: 0x0019780F File Offset: 0x00195A0F
		public int IkCallbackCount
		{
			get
			{
				return this._ikCallbackCount;
			}
		}

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x06004410 RID: 17424 RVA: 0x00197817 File Offset: 0x00195A17
		public int LastIkLayer
		{
			get
			{
				return this._lastIkLayer;
			}
		}

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x06004411 RID: 17425 RVA: 0x0019781F File Offset: 0x00195A1F
		public bool AnimationRigReady
		{
			get
			{
				return this._rigReady;
			}
		}

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x06004412 RID: 17426 RVA: 0x00197827 File Offset: 0x00195A27
		public float AnimationRigWeight
		{
			get
			{
				if (!(this._printRig != null))
				{
					return -1f;
				}
				return this._printRig.weight;
			}
		}

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x06004413 RID: 17427 RVA: 0x00197848 File Offset: 0x00195A48
		public bool IsParentedToRightHand
		{
			get
			{
				return this._heldPrint != null && this._rightHand != null && this._heldPrint.PivotTransform.parent == this._rightHand;
			}
		}

		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x06004414 RID: 17428 RVA: 0x00197884 File Offset: 0x00195A84
		public string HeldPrintParentName
		{
			get
			{
				if (!(this._heldPrint != null) || !(this._heldPrint.PivotTransform.parent != null))
				{
					return string.Empty;
				}
				return this._heldPrint.PivotTransform.parent.name;
			}
		}

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x06004415 RID: 17429 RVA: 0x001978D2 File Offset: 0x00195AD2
		public Vector3 HeldPrintLocalPosition
		{
			get
			{
				if (!(this._heldPrint != null))
				{
					return Vector3.zero;
				}
				return this._heldPrint.PivotTransform.localPosition;
			}
		}

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x06004416 RID: 17430 RVA: 0x001978F8 File Offset: 0x00195AF8
		public Vector3 HeldPrintLocalEulerAngles
		{
			get
			{
				if (!(this._heldPrint != null))
				{
					return Vector3.zero;
				}
				return this._heldPrint.PivotTransform.localEulerAngles;
			}
		}

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x06004417 RID: 17431 RVA: 0x0019791E File Offset: 0x00195B1E
		public Vector3 HeldPrintLocalScale
		{
			get
			{
				if (!(this._heldPrint != null))
				{
					return Vector3.one;
				}
				return this._heldPrint.PivotTransform.localScale;
			}
		}

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x06004418 RID: 17432 RVA: 0x00197944 File Offset: 0x00195B44
		public int HeldPhotoCount
		{
			get
			{
				if (!(this._heldInventoryItem != null))
				{
					return 0;
				}
				return this._heldInventoryItem.PhotoCount;
			}
		}

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x06004419 RID: 17433 RVA: 0x00197961 File Offset: 0x00195B61
		public int SelectedPhotoNumber
		{
			get
			{
				if (!(this._heldInventoryItem != null))
				{
					return 0;
				}
				return this._heldInventoryItem.SelectedDisplayNumber;
			}
		}

		// Token: 0x0600441A RID: 17434 RVA: 0x00197980 File Offset: 0x00195B80
		public static PhotoBoothPrintHolder GetOrCreate(vThirdPersonController player)
		{
			if (player == null)
			{
				return null;
			}
			Animator animator = player.animator;
			if (animator == null)
			{
				animator = player.GetComponent<Animator>();
			}
			if (animator == null || !animator.isHuman)
			{
				foreach (Animator animator2 in player.GetComponentsInChildren<Animator>(true))
				{
					if (!(animator2 == null) && animator2.isHuman)
					{
						animator = animator2;
						break;
					}
				}
			}
			if (animator == null || !animator.isHuman)
			{
				return null;
			}
			PhotoBoothPrintHolder photoBoothPrintHolder = animator.GetComponent<PhotoBoothPrintHolder>();
			if (photoBoothPrintHolder == null)
			{
				photoBoothPrintHolder = animator.gameObject.AddComponent<PhotoBoothPrintHolder>();
			}
			photoBoothPrintHolder.Initialize(animator, player.transform);
			return photoBoothPrintHolder;
		}

		// Token: 0x0600441B RID: 17435 RVA: 0x00197A30 File Offset: 0x00195C30
		public void Initialize(Animator animator, Transform playerRoot)
		{
			this._animator = animator;
			this._playerRoot = playerRoot;
			this._chest = animator.GetBoneTransform(HumanBodyBones.UpperChest);
			if (this._chest == null)
			{
				this._chest = animator.GetBoneTransform(HumanBodyBones.Chest);
			}
			this._rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
			this._layerIndex = animator.GetLayerIndex("PosterHold");
			this._firstPersonController = ((playerRoot != null) ? playerRoot.GetComponent<FirstPersonController>() : null);
			this.BindInventory();
			this.EnsureAnimationRig();
			this.EnsureHudConfigs();
		}

		// Token: 0x0600441C RID: 17436 RVA: 0x00197AC0 File Offset: 0x00195CC0
		public bool StoreInInventory(PhotoBoothPrint print)
		{
			if (print == null)
			{
				return false;
			}
			this.BindInventory();
			if (this._inventory == null)
			{
				return false;
			}
			PhotoBoothPhotoItemSO photoBoothPhotoItemSO = print.CreateInventoryItem();
			if (photoBoothPhotoItemSO == null)
			{
				return false;
			}
			if (!this._inventory.AddItem(photoBoothPhotoItemSO))
			{
				photoBoothPhotoItemSO.AbandonWithoutDestroyingTextures();
				return false;
			}
			print.CommitInventoryCollection();
			return true;
		}

		// Token: 0x0600441D RID: 17437 RVA: 0x00197B20 File Offset: 0x00195D20
		public void Hold(PhotoBoothPrint print)
		{
			if (print == null || this._animator == null)
			{
				return;
			}
			if (this._heldPrint != null && this._heldPrint != print)
			{
				this.ClearHeldState(0.08f);
			}
			this._heldPrint = print;
			this._heldPrint.SetHeld(true);
			this.ApplyHeldPose();
			this.AttachPrintToTunedHandPose();
			if (this._restoreAnimatorModeRoutine != null)
			{
				base.StopCoroutine(this._restoreAnimatorModeRoutine);
				this._restoreAnimatorModeRoutine = null;
			}
			if (!this._animatorModeOverridden)
			{
				this._savedAnimatorUpdateMode = this._animator.updateMode;
				this._animator.updateMode = AnimatorUpdateMode.Normal;
				this._animatorModeOverridden = true;
			}
			this._poseWeight = 1f;
		}

		// Token: 0x0600441E RID: 17438 RVA: 0x00197BE0 File Offset: 0x00195DE0
		public void DropCurrent()
		{
			if (this._heldPrint == null)
			{
				return;
			}
			if (this._heldInventoryItem != null && this._inventory != null && this._inventory.ActiveSlotIndex >= 0)
			{
				this._inventory.SelectSlot(this._inventory.ActiveSlotIndex);
				return;
			}
			PhotoBoothPrint heldPrint = this._heldPrint;
			this._heldPrint = null;
			this.StopPhotoViewZoom();
			this._poseWeight = 0f;
			this.ScheduleAnimatorModeRestore(0.22f);
			Vector3 vector = ((this._playerRoot != null) ? this._playerRoot.forward : base.transform.forward);
			Vector3 vector2 = ((this._playerRoot != null) ? this._playerRoot.position : base.transform.position) + vector * 0.52f + Vector3.up * 0.28f;
			Quaternion quaternion = Quaternion.LookRotation(Vector3.up, -vector) * Quaternion.AngleAxis(90f, Vector3.up);
			heldPrint.Drop(vector2, quaternion);
		}

		// Token: 0x0600441F RID: 17439 RVA: 0x00197D04 File Offset: 0x00195F04
		public void NotifyPrintDestroyed(PhotoBoothPrint print)
		{
			if (this._heldPrint != print)
			{
				return;
			}
			this.StopPhotoViewZoom();
			this._heldPrint = null;
			this._heldInventoryItem = null;
			this._poseWeight = 0f;
			this.ScheduleAnimatorModeRestore(0.14f);
		}

		// Token: 0x06004420 RID: 17440 RVA: 0x00197D40 File Offset: 0x00195F40
		private void Update()
		{
			if (this._heldPrint == null || this._heldInventoryItem == null)
			{
				this.StopPhotoViewZoom();
				return;
			}
			if (PhotoBoothPrintHolder.TryConsumeControllerDelete(this._firstPersonController != null && this._firstPersonController.IsFirstPerson && this._pageTurnRoutine == null && !DildoPlacementSystem.AnyPlacing))
			{
				this.DestroySelectedPhoto();
				return;
			}
			if (!ControllerInput.IsActive && GameplayInput.GetKeyDown(KeyCode.X))
			{
				this.DropCurrent();
				return;
			}
			if (GameplayInput.GetKeyDown(KeyCode.R) && this._heldInventoryItem.PhotoCount > 1 && this._pageTurnRoutine == null && !DildoPlacementSystem.AnyPlacing)
			{
				this._pageTurnRoutine = base.StartCoroutine(this.TurnToNextPhoto());
			}
			if (GameplayInput.GetKeyDown(KeyCode.Delete) && this._pageTurnRoutine == null && !DildoPlacementSystem.AnyPlacing)
			{
				this.DestroySelectedPhoto();
			}
			this.UpdatePhotoViewZoomInput();
		}

		// Token: 0x06004421 RID: 17441 RVA: 0x00197E21 File Offset: 0x00196021
		internal static bool TryConsumeControllerDelete(bool ownsViewer)
		{
			if (!ownsViewer || !ControllerInput.IsActive || ControllerWorldPointer.BlocksContextInput || !ControllerGameplayBridge.AllowsAxes || !ControllerInput.Modifier || !ControllerInput.GetButtonDown(ControllerButton.North))
			{
				return false;
			}
			ControllerInput.ConsumeUIInput();
			return true;
		}

		// Token: 0x06004422 RID: 17442 RVA: 0x00197E54 File Offset: 0x00196054
		private bool DestroySelectedPhoto()
		{
			if (this._heldPrint == null || this._heldInventoryItem == null || this._inventory == null || this._inventory.ActiveSlotIndex < 0)
			{
				return false;
			}
			PhotoBoothPhotoItemSO heldInventoryItem = this._heldInventoryItem;
			Vector3 visualCenter = this._heldPrint.VisualCenter;
			if (!heldInventoryItem.DestroySelectedPhoto())
			{
				return false;
			}
			WardrobeFeedback.PlayItemDiscardAt(visualCenter, new Color(1f, 0.16f, 0.42f, 1f));
			this._inventory.ConsumeActiveItem();
			if (heldInventoryItem.PhotoCount > 0)
			{
				if (this._heldPrint != null)
				{
					this._heldPrint.RefreshInventoryVisual(heldInventoryItem);
				}
				this._inventory.NotifyActiveItemVisualChanged();
			}
			else
			{
				heldInventoryItem.DisposeRuntimeAssets();
			}
			return true;
		}

		// Token: 0x06004423 RID: 17443 RVA: 0x00197F1B File Offset: 0x0019611B
		private void LateUpdate()
		{
			this.UpdatePhotoViewZoom();
			this.UpdateHeldActionsHud();
		}

		// Token: 0x06004424 RID: 17444 RVA: 0x00197F29 File Offset: 0x00196129
		private IEnumerator TurnToNextPhoto()
		{
			if (this._heldPrint == null || this._heldInventoryItem == null)
			{
				yield break;
			}
			Transform pivot = this._heldPrint.PivotTransform;
			this._pageTurnBaseScale = pivot.localScale;
			this._pageTurnBaseRotation = pivot.localRotation;
			float elapsed = 0f;
			while (elapsed < 0.14f && pivot != null)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / 0.14f));
				pivot.localScale = new Vector3(this._pageTurnBaseScale.x * Mathf.Lerp(1f, 0.08f, num), this._pageTurnBaseScale.y, this._pageTurnBaseScale.z);
				pivot.localRotation = this._pageTurnBaseRotation * Quaternion.Euler(0f, 76f * num, 0f);
				yield return null;
			}
			if (this._heldPrint != null && this._heldInventoryItem != null && this._heldInventoryItem.SelectRelative(1))
			{
				this._heldPrint.RefreshInventoryVisual(this._heldInventoryItem);
				this.BeginAlbumPreload();
			}
			elapsed = 0f;
			while (elapsed < 0.14f && pivot != null)
			{
				elapsed += Time.unscaledDeltaTime;
				float num2 = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / 0.14f));
				pivot.localScale = new Vector3(this._pageTurnBaseScale.x * Mathf.Lerp(0.08f, 1f, num2), this._pageTurnBaseScale.y, this._pageTurnBaseScale.z);
				pivot.localRotation = this._pageTurnBaseRotation * Quaternion.Euler(0f, 76f * (1f - num2), 0f);
				yield return null;
			}
			if (pivot != null)
			{
				pivot.localScale = this._pageTurnBaseScale;
				pivot.localRotation = this._pageTurnBaseRotation;
			}
			this._pageTurnRoutine = null;
			yield break;
		}

		// Token: 0x06004425 RID: 17445 RVA: 0x00197F38 File Offset: 0x00196138
		private void UpdatePhotoViewZoomInput()
		{
			if (!(this._firstPersonController != null) || !this._firstPersonController.IsFirstPerson || DildoPlacementSystem.AnyPlacing)
			{
				this.StopPhotoViewZoom();
				return;
			}
			this.BeginPhotoViewMovement();
			if (!this._photoViewZoomActive)
			{
				this._photoViewCamera = Camera.main;
				if (this._photoViewCamera == null)
				{
					return;
				}
				this._photoViewDefaultFov = this._photoViewCamera.fieldOfView;
				this._photoViewTargetFov = this._photoViewDefaultFov;
				this._photoViewDefaultCameraLocalPosition = this._photoViewCamera.transform.localPosition;
				this._photoViewCurrentPan = Vector2.zero;
				this._photoViewTargetPan = Vector2.zero;
				this._photoViewPanVelocity = Vector2.zero;
				this._photoViewZoomActive = true;
			}
			float y = GameplayInput.mouseScrollDelta.y;
			if (Mathf.Abs(y) > 0.001f)
			{
				this._photoViewTargetFov = Mathf.Clamp(this._photoViewTargetFov - y * 7f, 20f, this._photoViewDefaultFov);
			}
			PhotoBoothPrintHolder._activePhotoViewer = this;
			bool controllerOwnsPhotoPan = PhotoBoothPrintHolder.ControllerOwnsPhotoPan;
			bool flag = (ControllerInput.IsActive ? controllerOwnsPhotoPan : GameplayInput.GetMouseButton(2));
			if (this._firstPersonController != null)
			{
				this._firstPersonController.SuppressMouseLookForPhotoView = flag;
			}
			if (flag)
			{
				Vector2 vector = (controllerOwnsPhotoPan ? (ControllerInput.Modifier ? Vector2.zero : (ControllerInput.Look * (60f * Time.unscaledDeltaTime))) : new Vector2(GameplayInput.GetAxisRaw("Mouse X"), GameplayInput.GetAxisRaw("Mouse Y")));
				this._photoViewTargetPan.x = Mathf.Clamp(this._photoViewTargetPan.x + vector.x * 0.008f, -0.18f, 0.18f);
				this._photoViewTargetPan.y = Mathf.Clamp(this._photoViewTargetPan.y + vector.y * 0.008f, -0.14f, 0.14f);
			}
		}

		// Token: 0x06004426 RID: 17446 RVA: 0x00198118 File Offset: 0x00196318
		private void UpdatePhotoViewZoom()
		{
			if (!this._photoViewZoomActive || this._photoViewCamera == null)
			{
				return;
			}
			this._photoViewCamera.fieldOfView = Mathf.Lerp(this._photoViewCamera.fieldOfView, this._photoViewTargetFov, 1f - Mathf.Exp(-12f * Time.unscaledDeltaTime));
			this._photoViewCurrentPan = Vector2.SmoothDamp(this._photoViewCurrentPan, this._photoViewTargetPan, ref this._photoViewPanVelocity, 0.075f, float.PositiveInfinity, Time.unscaledDeltaTime);
			this._photoViewCamera.transform.localPosition = this._photoViewDefaultCameraLocalPosition + new Vector3(this._photoViewCurrentPan.x, this._photoViewCurrentPan.y, 0f);
		}

		// Token: 0x06004427 RID: 17447 RVA: 0x001981DC File Offset: 0x001963DC
		private void StopPhotoViewZoom()
		{
			if (PhotoBoothPrintHolder._activePhotoViewer == this)
			{
				PhotoBoothPrintHolder._activePhotoViewer = null;
			}
			if (this._photoViewMovementController != null)
			{
				this._photoViewMovementController.moveToDirectionInFree = this._savedMoveToDirectionInFree;
				this._photoViewMovementController = null;
			}
			if (this._firstPersonController != null)
			{
				this._firstPersonController.SuppressMouseLookForPhotoView = false;
			}
			if (!this._photoViewZoomActive)
			{
				return;
			}
			if (this._photoViewCamera != null)
			{
				this._photoViewCamera.fieldOfView = this._photoViewDefaultFov;
				this._photoViewCamera.transform.localPosition = this._photoViewDefaultCameraLocalPosition;
			}
			this._photoViewCamera = null;
			this._photoViewZoomActive = false;
			this._photoViewCurrentPan = Vector2.zero;
			this._photoViewTargetPan = Vector2.zero;
			this._photoViewPanVelocity = Vector2.zero;
		}

		// Token: 0x06004428 RID: 17448 RVA: 0x001982AC File Offset: 0x001964AC
		private void BeginPhotoViewMovement()
		{
			if (this._photoViewMovementController != null)
			{
				return;
			}
			vThirdPersonController vThirdPersonController = ((this._playerRoot != null) ? this._playerRoot.GetComponent<vThirdPersonController>() : this._firstPersonController.GetComponent<vThirdPersonController>());
			if (vThirdPersonController == null)
			{
				return;
			}
			this._savedMoveToDirectionInFree = vThirdPersonController.moveToDirectionInFree;
			this._photoViewMovementController = vThirdPersonController;
			vThirdPersonController.moveToDirectionInFree = true;
		}

		// Token: 0x06004429 RID: 17449 RVA: 0x00198314 File Offset: 0x00196514
		private void EnsureHudConfigs()
		{
			if (this._thirdPersonSingleConfig != null)
			{
				return;
			}
			this._thirdPersonSingleConfig = PhotoBoothPrintHolder.CreateHeldConfig(false, false);
			this._thirdPersonAlbumConfig = PhotoBoothPrintHolder.CreateHeldConfig(false, true);
			this._firstPersonSingleConfig = PhotoBoothPrintHolder.CreateHeldConfig(true, false);
			this._firstPersonAlbumConfig = PhotoBoothPrintHolder.CreateHeldConfig(true, true);
		}

		// Token: 0x0600442A RID: 17450 RVA: 0x00198364 File Offset: 0x00196564
		private static InteractionConfigSO CreateHeldConfig(bool firstPerson, bool album)
		{
			InteractionConfigSO interactionConfigSO = ScriptableObject.CreateInstance<InteractionConfigSO>();
			interactionConfigSO.hideFlags = HideFlags.DontSave;
			interactionConfigSO.objectName = "Photo Booth Photos";
			interactionConfigSO.usableActions.Clear();
			if (album)
			{
				interactionConfigSO.usableActions.Add(new UsableAction
				{
					actionName = "Next Photo",
					keyDisplayLabel = "R"
				});
			}
			List<UsableAction> usableActions = interactionConfigSO.usableActions;
			UsableAction usableAction2;
			if (!firstPerson)
			{
				UsableAction usableAction = new UsableAction();
				usableAction.actionName = "Enter First Person to View";
				usableAction2 = usableAction;
				usableAction.keyDisplayLabel = "V";
			}
			else
			{
				UsableAction usableAction3 = new UsableAction();
				usableAction3.actionName = "Zoom Photo";
				usableAction2 = usableAction3;
				usableAction3.keyDisplayLabel = "WHEEL";
			}
			usableActions.Add(usableAction2);
			if (firstPerson)
			{
				interactionConfigSO.usableActions.Add(new UsableAction
				{
					actionName = "Pan Photo",
					keyDisplayLabel = "MMB + DRAG"
				});
			}
			interactionConfigSO.usableActions.Add(new UsableAction
			{
				actionName = "Aim Photo at Wall / Floor",
				keyDisplayLabel = "HOLD RMB"
			});
			interactionConfigSO.usableActions.Add(new UsableAction
			{
				actionName = "DESTROY PHOTO",
				keyDisplayLabel = "DEL"
			});
			ControllerContextBindings.SetLabel(interactionConfigSO, "R", "LT + X", false);
			ControllerContextBindings.SetLabel(interactionConfigSO, "V", "R3", false);
			ControllerContextBindings.SetLabel(interactionConfigSO, "WHEEL", "LT + RS", false);
			ControllerContextBindings.SetLabel(interactionConfigSO, "MMB + DRAG", "Hold RT + RS", false);
			ControllerContextBindings.SetLabel(interactionConfigSO, "HOLD RMB", "Hold LB", false);
			ControllerContextBindings.SetLabel(interactionConfigSO, "DEL", firstPerson ? "LT + Y" : "R3 then LT + Y", false);
			return interactionConfigSO;
		}

		// Token: 0x0600442B RID: 17451 RVA: 0x001984EC File Offset: 0x001966EC
		private void UpdateHeldActionsHud()
		{
			InteractionHUDController instance = InteractionHUDController.Instance;
			if (instance == null)
			{
				return;
			}
			this.EnsureHudConfigs();
			InteractionConfigSO interactionConfigSO = null;
			if (this._heldPrint != null && this._heldInventoryItem != null && !DildoPlacementSystem.AnyPlacing)
			{
				bool flag = this._firstPersonController != null && this._firstPersonController.IsFirstPerson;
				bool flag2 = this._heldInventoryItem.PhotoCount > 1;
				interactionConfigSO = (flag ? (flag2 ? this._firstPersonAlbumConfig : this._firstPersonSingleConfig) : (flag2 ? this._thirdPersonAlbumConfig : this._thirdPersonSingleConfig));
			}
			if (interactionConfigSO == this._shownConfig)
			{
				if (interactionConfigSO != null && (!instance.IsShowing || instance.CurrentConfig != interactionConfigSO))
				{
					instance.Show(interactionConfigSO);
				}
				return;
			}
			if (interactionConfigSO != null)
			{
				instance.Show(interactionConfigSO);
			}
			else if (this._shownConfig != null && instance.CurrentConfig == this._shownConfig)
			{
				instance.Hide();
			}
			this._shownConfig = interactionConfigSO;
		}

		// Token: 0x0600442C RID: 17452 RVA: 0x001985FB File Offset: 0x001967FB
		private void OnAnimatorIK(int layerIndex)
		{
			this._ikCallbackCount++;
			this._lastIkLayer = layerIndex;
		}

		// Token: 0x0600442D RID: 17453 RVA: 0x00198614 File Offset: 0x00196814
		private void ApplyHeldPose()
		{
			if (this._heldPrint == null || this._playerRoot == null)
			{
				return;
			}
			Vector3 vector;
			Quaternion quaternion;
			this.GetDesiredPaperPose(out vector, out quaternion);
			this._heldPrint.SetVisualWorldPose(vector, quaternion);
		}

		// Token: 0x0600442E RID: 17454 RVA: 0x00198658 File Offset: 0x00196858
		private void GetDesiredPaperPose(out Vector3 position, out Quaternion rotation)
		{
			Vector3 vector = ((this._chest != null) ? this._chest.position : (this._playerRoot.position + this._playerRoot.up * 1.25f));
			position = vector + this._playerRoot.forward * 0.37f + this._playerRoot.up * 0.055f;
			rotation = Quaternion.LookRotation(this._playerRoot.up, -this._playerRoot.forward) * Quaternion.AngleAxis(90f, Vector3.up);
		}

		// Token: 0x0600442F RID: 17455 RVA: 0x0019871C File Offset: 0x0019691C
		private void EnsureAnimationRig()
		{
			if (this._rigReady || this._animator == null)
			{
				return;
			}
			this._rigBuilder = this._animator.GetComponent<RigBuilder>();
			if (this._rigBuilder == null)
			{
				return;
			}
			Transform transform = PhotoBoothPrintHolder.FindRigChild(this._animator.transform, "PhotoPrintRig");
			if (transform == null)
			{
				return;
			}
			this._printRig = transform.GetComponent<Rig>();
			this._leftRigTarget = PhotoBoothPrintHolder.FindRigChild(transform, "LeftHand_Print_Target");
			this._rightRigTarget = PhotoBoothPrintHolder.FindRigChild(transform, "RightHand_Print_Target");
			this._leftRigHint = PhotoBoothPrintHolder.FindRigChild(transform, "LeftElbow_Print_Hint");
			this._rightRigHint = PhotoBoothPrintHolder.FindRigChild(transform, "RightElbow_Print_Hint");
			foreach (TwoBoneIKConstraint twoBoneIKConstraint in transform.GetComponentsInChildren<TwoBoneIKConstraint>(true))
			{
				if (twoBoneIKConstraint.name == "LeftArm_Print_IK")
				{
					this._leftArmRig = twoBoneIKConstraint;
				}
				else if (twoBoneIKConstraint.name == "RightArm_Print_IK")
				{
					this._rightArmRig = twoBoneIKConstraint;
				}
			}
			this._rigReady = this._printRig != null && this._leftArmRig != null && this._rightArmRig != null && this._leftRigTarget != null && this._rightRigTarget != null && this._leftRigHint != null && this._rightRigHint != null;
			if (this._leftArmRig != null)
			{
				this._leftArmRig.weight = 1f;
			}
			if (this._rightArmRig != null)
			{
				this._rightArmRig.weight = 1f;
			}
			if (this._printRig != null)
			{
				this._printRig.weight = 0f;
			}
		}

		// Token: 0x06004430 RID: 17456 RVA: 0x001988E4 File Offset: 0x00196AE4
		private static Transform FindRigChild(Transform root, string name)
		{
			foreach (Transform transform in root.GetComponentsInChildren<Transform>(true))
			{
				if (transform.name == name)
				{
					return transform;
				}
			}
			return null;
		}

		// Token: 0x06004431 RID: 17457 RVA: 0x0019891C File Offset: 0x00196B1C
		private void UpdateRigTargets()
		{
			if (!this._rigReady || this._heldPrint == null || this._playerRoot == null)
			{
				return;
			}
			Vector3 vector;
			Vector3 vector2;
			this.CalculateGripPose(out this._leftGripTarget, out this._rightGripTarget, out vector, out vector2);
			Vector3 vector3;
			Quaternion quaternion;
			this.GetDesiredPaperPose(out vector3, out quaternion);
			this._leftRigTarget.SetPositionAndRotation(this._leftGripTarget, this._heldPrint.transform.rotation);
			this._rightRigTarget.SetPositionAndRotation(this._rightGripTarget, quaternion);
			this._leftRigHint.position = vector;
			this._rightRigHint.position = vector2;
		}

		// Token: 0x06004432 RID: 17458 RVA: 0x001989BC File Offset: 0x00196BBC
		private void CalculateGripPose(out Vector3 leftGrip, out Vector3 rightGrip, out Vector3 leftElbowHint, out Vector3 rightElbowHint)
		{
			Vector3 visualCenter = this._heldPrint.VisualCenter;
			Vector3 vector;
			Quaternion quaternion;
			this.GetDesiredPaperPose(out vector, out quaternion);
			float num = this._heldPrint.GetWorldSizeAlong(this._playerRoot.right) * 0.5f;
			float num2 = this._heldPrint.GetWorldSizeAlong(this._playerRoot.up) * 0.5f;
			float num3 = Mathf.Clamp(num + 0.045f, 0.105f, 0.145f);
			float num4 = Mathf.Clamp(num2 * 0.72f, 0.09f, 0.125f);
			Vector3 vector2 = -this._playerRoot.forward * 0.018f;
			Vector3 vector3 = visualCenter - this._playerRoot.up * num4 + vector2;
			Vector3 vector4 = vector - this._playerRoot.up * num4 + vector2;
			leftGrip = vector3 - this._playerRoot.right * num3;
			rightGrip = vector4 + this._playerRoot.right * num3;
			this._leftGripTarget = leftGrip;
			this._rightGripTarget = rightGrip;
			Vector3 vector5 = ((this._chest != null) ? this._chest.position : (vector - this._playerRoot.forward * 0.37f - this._playerRoot.up * 0.055f)) - this._playerRoot.up * 0.24f + this._playerRoot.forward * 0.025f;
			leftElbowHint = vector5 - this._playerRoot.right * 0.31f;
			rightElbowHint = vector5 + this._playerRoot.right * 0.31f;
		}

		// Token: 0x06004433 RID: 17459 RVA: 0x00198BBE File Offset: 0x00196DBE
		private void AttachPrintToTunedHandPose()
		{
			if (this._heldPrint == null || this._rightHand == null)
			{
				return;
			}
			this._heldPrint.AttachToHand(this._rightHand, PhotoBoothPrintHolder.TunedHeldPrintLocalPosition, PhotoBoothPrintHolder.TunedHeldPrintLocalEuler, PhotoBoothPrintHolder.TunedHeldPrintLocalScale);
		}

		// Token: 0x06004434 RID: 17460 RVA: 0x00198BFD File Offset: 0x00196DFD
		private void ScheduleAnimatorModeRestore(float delay)
		{
			if (!this._animatorModeOverridden)
			{
				return;
			}
			if (this._restoreAnimatorModeRoutine != null)
			{
				base.StopCoroutine(this._restoreAnimatorModeRoutine);
			}
			this._restoreAnimatorModeRoutine = base.StartCoroutine(this.RestoreAnimatorModeAfter(delay));
		}

		// Token: 0x06004435 RID: 17461 RVA: 0x00198C2F File Offset: 0x00196E2F
		private IEnumerator RestoreAnimatorModeAfter(float delay)
		{
			yield return new WaitForSeconds(delay);
			if (this._animator != null && this._heldPrint == null)
			{
				this._animator.updateMode = this._savedAnimatorUpdateMode;
			}
			this._animatorModeOverridden = false;
			this._restoreAnimatorModeRoutine = null;
			yield break;
		}

		// Token: 0x06004436 RID: 17462 RVA: 0x00198C48 File Offset: 0x00196E48
		private void BindInventory()
		{
			PlayerInventoryManager playerInventoryManager = PlayerInventoryManager.Instance;
			if (playerInventoryManager == null && this._playerRoot != null)
			{
				playerInventoryManager = this._playerRoot.GetComponentInChildren<PlayerInventoryManager>(true);
			}
			if (playerInventoryManager == this._inventory)
			{
				return;
			}
			if (this._inventory != null)
			{
				this._inventory.OnActiveSlotChanged -= this.HandleActiveSlotChanged;
				this._inventory.OnItemStackChanged -= this.HandleItemStackChanged;
			}
			this._inventory = playerInventoryManager;
			if (this._inventory != null)
			{
				this._inventory.OnActiveSlotChanged += this.HandleActiveSlotChanged;
				this._inventory.OnItemStackChanged += this.HandleItemStackChanged;
				this.HandleActiveSlotChanged(this._inventory.ActiveSlotIndex);
			}
		}

		// Token: 0x06004437 RID: 17463 RVA: 0x00198D20 File Offset: 0x00196F20
		private void HandleItemStackChanged(int slotIndex, int previous, int current)
		{
			if (this._inventory == null || slotIndex != this._inventory.ActiveSlotIndex || this._heldPrint == null || this._heldInventoryItem == null)
			{
				return;
			}
			this._heldPrint.RefreshInventoryVisual(this._heldInventoryItem);
			this.BeginAlbumPreload();
		}

		// Token: 0x06004438 RID: 17464 RVA: 0x00198D80 File Offset: 0x00196F80
		private void HandleActiveSlotChanged(int slotIndex)
		{
			if (this._inventory == null)
			{
				return;
			}
			PhotoBoothPhotoItemSO photoBoothPhotoItemSO = this._inventory.GetActiveItem() as PhotoBoothPhotoItemSO;
			if (photoBoothPhotoItemSO == null)
			{
				if (this._heldInventoryItem != null)
				{
					this.ClearHeldState(0.2f);
				}
				return;
			}
			GameObject spawnedHandObject = this._inventory.GetSpawnedHandObject();
			PhotoBoothPrint photoBoothPrint = ((spawnedHandObject != null) ? spawnedHandObject.GetComponent<PhotoBoothPrint>() : null);
			if (photoBoothPrint == null && spawnedHandObject != null)
			{
				photoBoothPhotoItemSO.ConfigureHandInstance(spawnedHandObject);
				photoBoothPrint = spawnedHandObject.GetComponent<PhotoBoothPrint>();
			}
			if (photoBoothPrint == null)
			{
				return;
			}
			this.Hold(photoBoothPrint);
			this._heldInventoryItem = photoBoothPhotoItemSO;
			this.BeginAlbumPreload();
		}

		// Token: 0x06004439 RID: 17465 RVA: 0x00198E2D File Offset: 0x0019702D
		private void ClearHeldState(float blendDuration)
		{
			this.CancelPageTurn();
			this.StopAlbumPreload();
			this.StopPhotoViewZoom();
			this._heldPrint = null;
			this._heldInventoryItem = null;
			this._poseWeight = 0f;
			this.ScheduleAnimatorModeRestore(blendDuration + 0.02f);
		}

		// Token: 0x0600443A RID: 17466 RVA: 0x00198E67 File Offset: 0x00197067
		private void OnDisable()
		{
			this.StopPhotoViewZoom();
		}

		// Token: 0x0600443B RID: 17467 RVA: 0x00198E70 File Offset: 0x00197070
		private void OnDestroy()
		{
			this.CancelPageTurn();
			this.StopAlbumPreload();
			this.StopPhotoViewZoom();
			if (this._shownConfig != null && InteractionHUDController.Instance != null && InteractionHUDController.Instance.CurrentConfig == this._shownConfig)
			{
				InteractionHUDController.Instance.Hide();
			}
			if (this._inventory != null)
			{
				this._inventory.OnActiveSlotChanged -= this.HandleActiveSlotChanged;
				this._inventory.OnItemStackChanged -= this.HandleItemStackChanged;
			}
			PhotoBoothPrintHolder.DestroyHudConfig(this._thirdPersonSingleConfig);
			PhotoBoothPrintHolder.DestroyHudConfig(this._thirdPersonAlbumConfig);
			PhotoBoothPrintHolder.DestroyHudConfig(this._firstPersonSingleConfig);
			PhotoBoothPrintHolder.DestroyHudConfig(this._firstPersonAlbumConfig);
		}

		// Token: 0x0600443C RID: 17468 RVA: 0x00198F34 File Offset: 0x00197134
		private void CancelPageTurn()
		{
			if (this._pageTurnRoutine == null)
			{
				return;
			}
			base.StopCoroutine(this._pageTurnRoutine);
			this._pageTurnRoutine = null;
			if (this._heldPrint == null)
			{
				return;
			}
			this._heldPrint.PivotTransform.localScale = this._pageTurnBaseScale;
			this._heldPrint.PivotTransform.localRotation = this._pageTurnBaseRotation;
		}

		// Token: 0x0600443D RID: 17469 RVA: 0x00198F98 File Offset: 0x00197198
		private void BeginAlbumPreload()
		{
			this.StopAlbumPreload();
			if (this._heldInventoryItem == null || this._heldInventoryItem.PhotoCount < 2)
			{
				return;
			}
			this._albumPreloadRoutine = base.StartCoroutine(this.PreloadAlbumWindow(this._heldInventoryItem));
		}

		// Token: 0x0600443E RID: 17470 RVA: 0x00198FD5 File Offset: 0x001971D5
		private IEnumerator PreloadAlbumWindow(PhotoBoothPhotoItemSO album)
		{
			album.TrimArchiveTextureCache(2, 8);
			int num;
			for (int delta = 1; delta <= 8; delta = num + 1)
			{
				yield return null;
				if (album == null || album != this._heldInventoryItem)
				{
					break;
				}
				album.PreloadRelative(delta);
				num = delta;
			}
			if (album != null && album == this._heldInventoryItem)
			{
				album.TrimArchiveTextureCache(2, 8);
			}
			this._albumPreloadRoutine = null;
			yield break;
		}

		// Token: 0x0600443F RID: 17471 RVA: 0x00198FEB File Offset: 0x001971EB
		private void StopAlbumPreload()
		{
			if (this._albumPreloadRoutine == null)
			{
				return;
			}
			base.StopCoroutine(this._albumPreloadRoutine);
			this._albumPreloadRoutine = null;
		}

		// Token: 0x06004440 RID: 17472 RVA: 0x00199009 File Offset: 0x00197209
		private static void DestroyHudConfig(InteractionConfigSO config)
		{
			if (config != null)
			{
				Object.Destroy(config);
			}
		}

		// Token: 0x040043AB RID: 17323
		private const string LayerName = "PosterHold";

		// Token: 0x040043AC RID: 17324
		private const float HeldForwardOffset = 0.37f;

		// Token: 0x040043AD RID: 17325
		private const float HeldUpOffset = 0.055f;

		// Token: 0x040043AE RID: 17326
		private const float GripSideOutset = 0.045f;

		// Token: 0x040043AF RID: 17327
		private const float PageTurnHalfDuration = 0.14f;

		// Token: 0x040043B0 RID: 17328
		private const float PhotoViewMinimumFov = 20f;

		// Token: 0x040043B1 RID: 17329
		private const float PhotoViewZoomPerStep = 7f;

		// Token: 0x040043B2 RID: 17330
		private const float PhotoViewPanSensitivity = 0.008f;

		// Token: 0x040043B3 RID: 17331
		private const float PhotoViewPanSmoothTime = 0.075f;

		// Token: 0x040043B4 RID: 17332
		private const float PhotoViewMaxPanX = 0.18f;

		// Token: 0x040043B5 RID: 17333
		private const float PhotoViewMaxPanY = 0.14f;

		// Token: 0x040043B6 RID: 17334
		private static PhotoBoothPrintHolder _activePhotoViewer;

		// Token: 0x040043B7 RID: 17335
		private const int AlbumCacheBehind = 2;

		// Token: 0x040043B8 RID: 17336
		private const int AlbumPreloadAhead = 8;

		// Token: 0x040043B9 RID: 17337
		private static readonly Vector3 TunedHeldPrintLocalPosition = new Vector3(0.0808f, 0.1058f, -0.0086f);

		// Token: 0x040043BA RID: 17338
		private static readonly Vector3 TunedHeldPrintLocalEuler = new Vector3(94.44598f, -26.33099f, 144.113f);

		// Token: 0x040043BB RID: 17339
		private static readonly Vector3 TunedHeldPrintLocalScale = new Vector3(0.7142859f, 0.7142863f, 0.714286f);

		// Token: 0x040043BC RID: 17340
		private Animator _animator;

		// Token: 0x040043BD RID: 17341
		private Transform _playerRoot;

		// Token: 0x040043BE RID: 17342
		private Transform _chest;

		// Token: 0x040043BF RID: 17343
		private Transform _rightHand;

		// Token: 0x040043C0 RID: 17344
		private PhotoBoothPrint _heldPrint;

		// Token: 0x040043C1 RID: 17345
		private int _layerIndex = -1;

		// Token: 0x040043C2 RID: 17346
		private float _poseWeight;

		// Token: 0x040043C3 RID: 17347
		private Vector3 _leftGripTarget;

		// Token: 0x040043C4 RID: 17348
		private Vector3 _rightGripTarget;

		// Token: 0x040043C5 RID: 17349
		private int _ikCallbackCount;

		// Token: 0x040043C6 RID: 17350
		private int _lastIkLayer = -1;

		// Token: 0x040043C7 RID: 17351
		private RigBuilder _rigBuilder;

		// Token: 0x040043C8 RID: 17352
		private Rig _printRig;

		// Token: 0x040043C9 RID: 17353
		private TwoBoneIKConstraint _leftArmRig;

		// Token: 0x040043CA RID: 17354
		private TwoBoneIKConstraint _rightArmRig;

		// Token: 0x040043CB RID: 17355
		private Transform _leftRigTarget;

		// Token: 0x040043CC RID: 17356
		private Transform _rightRigTarget;

		// Token: 0x040043CD RID: 17357
		private Transform _leftRigHint;

		// Token: 0x040043CE RID: 17358
		private Transform _rightRigHint;

		// Token: 0x040043CF RID: 17359
		private bool _rigReady;

		// Token: 0x040043D0 RID: 17360
		private PlayerInventoryManager _inventory;

		// Token: 0x040043D1 RID: 17361
		private PhotoBoothPhotoItemSO _heldInventoryItem;

		// Token: 0x040043D2 RID: 17362
		private AnimatorUpdateMode _savedAnimatorUpdateMode;

		// Token: 0x040043D3 RID: 17363
		private bool _animatorModeOverridden;

		// Token: 0x040043D4 RID: 17364
		private Coroutine _restoreAnimatorModeRoutine;

		// Token: 0x040043D5 RID: 17365
		private Coroutine _pageTurnRoutine;

		// Token: 0x040043D6 RID: 17366
		private Coroutine _albumPreloadRoutine;

		// Token: 0x040043D7 RID: 17367
		private Vector3 _pageTurnBaseScale;

		// Token: 0x040043D8 RID: 17368
		private Quaternion _pageTurnBaseRotation;

		// Token: 0x040043D9 RID: 17369
		private FirstPersonController _firstPersonController;

		// Token: 0x040043DA RID: 17370
		private vThirdPersonController _photoViewMovementController;

		// Token: 0x040043DB RID: 17371
		private bool _savedMoveToDirectionInFree;

		// Token: 0x040043DC RID: 17372
		private InteractionConfigSO _thirdPersonSingleConfig;

		// Token: 0x040043DD RID: 17373
		private InteractionConfigSO _thirdPersonAlbumConfig;

		// Token: 0x040043DE RID: 17374
		private InteractionConfigSO _firstPersonSingleConfig;

		// Token: 0x040043DF RID: 17375
		private InteractionConfigSO _firstPersonAlbumConfig;

		// Token: 0x040043E0 RID: 17376
		private InteractionConfigSO _shownConfig;

		// Token: 0x040043E1 RID: 17377
		private Camera _photoViewCamera;

		// Token: 0x040043E2 RID: 17378
		private bool _photoViewZoomActive;

		// Token: 0x040043E3 RID: 17379
		private float _photoViewDefaultFov;

		// Token: 0x040043E4 RID: 17380
		private float _photoViewTargetFov;

		// Token: 0x040043E5 RID: 17381
		private Vector3 _photoViewDefaultCameraLocalPosition;

		// Token: 0x040043E6 RID: 17382
		private Vector2 _photoViewCurrentPan;

		// Token: 0x040043E7 RID: 17383
		private Vector2 _photoViewTargetPan;

		// Token: 0x040043E8 RID: 17384
		private Vector2 _photoViewPanVelocity;
	}
}
