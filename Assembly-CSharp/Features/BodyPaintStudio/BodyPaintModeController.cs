using System;
using System.Collections;
using System.Collections.Generic;
using Features.BodyPaintStudio.UI;
using Features.DebugTools;
using Features.Detection;
using Features.HideFlash;
using Features.NeckSigns;
using Features.RunModes;
using Invector.vCamera;
using Invector.vCharacterController;
using ProjectExhibitionist;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.BodyPaintStudio
{
	// Token: 0x0200150E RID: 5390
	[RequireComponent(typeof(BodyPaintCanvasService))]
	[RequireComponent(typeof(BodyPaintOrbitCamera))]
	public class BodyPaintModeController : MonoBehaviour
	{
		// Token: 0x170019AC RID: 6572
		// (get) Token: 0x0600A66D RID: 42605 RVA: 0x00459106 File Offset: 0x00457306
		public bool IsActive
		{
			get
			{
				return this._active;
			}
		}

		// Token: 0x170019AD RID: 6573
		// (get) Token: 0x0600A66E RID: 42606 RVA: 0x0045910E File Offset: 0x0045730E
		public bool IsItemPainting
		{
			get
			{
				return this._active && this._paintingItem;
			}
		}

		// Token: 0x170019AE RID: 6574
		// (get) Token: 0x0600A66F RID: 42607 RVA: 0x00459120 File Offset: 0x00457320
		public bool IsHeldItemPainting
		{
			get
			{
				return this._active && this._paintingHeldItem;
			}
		}

		// Token: 0x170019AF RID: 6575
		// (get) Token: 0x0600A670 RID: 42608 RVA: 0x00459132 File Offset: 0x00457332
		public NeckSignSurface ItemSurface
		{
			get
			{
				return this._itemSurface;
			}
		}

		// Token: 0x0600A671 RID: 42609 RVA: 0x0045913C File Offset: 0x0045733C
		private void Awake()
		{
			this._canvasService = base.GetComponent<BodyPaintCanvasService>();
			this._orbitCamera = base.GetComponent<BodyPaintOrbitCamera>();
			this._playerInput = base.GetComponentInParent<vThirdPersonInput>();
			this._animator = base.GetComponentInParent<Animator>();
			this._playerRoot = ((this._playerInput != null) ? this._playerInput.transform : ((this._animator != null) ? this._animator.transform : base.transform.root));
			this._actionAnimator = this._playerRoot.GetComponentInChildren<PlayerActionAnimator>(true);
			if (this._actionAnimator == null)
			{
				this._actionAnimator = Object.FindObjectOfType<PlayerActionAnimator>();
			}
			if (this._actionAnimator == null)
			{
				Debug.LogWarning("[BodyPaintModeController] PlayerActionAnimator non trovato: i layer emotivi resteranno attivi sopra la posa.", this);
			}
		}

		// Token: 0x0600A672 RID: 42610 RVA: 0x00459204 File Offset: 0x00457404
		private void Update()
		{
			bool flag = this._active && this._ui != null && this._ui.IsPresetDialogOpen;
			if (flag)
			{
				this._waitForPaintRelease = true;
				if (!ControllerTextKeyboard.IsOpen && !ControllerInput.UiInputConsumedThisFrame && !GraphicsSettingsRuntimeUI.AnyOpen && !EscapeInputCoordinator.ConsumedThisFrame && (Input.GetKeyDown(KeyCode.Escape) || EscapeInputCoordinator.TryConsume(this)))
				{
					EscapeInputCoordinator.Consume(this);
					this._ui.TryClosePresetDialog();
				}
			}
			if (this._active && this._ui != null && this._ui.BrushCursor != null)
			{
				this._ui.BrushCursor.SetActive(!this._ui.IsPresetDialogOpen);
			}
			if (this._active && this._paintingItem && (this._itemSurface == null || !this._itemSurface.isActiveAndEnabled || (this._paintingHeldItem && !BodyPaintModeController.HeldOwnerOwnsSurface(this._itemSurface))))
			{
				this.ExitMode();
				return;
			}
			if (!flag && this.enableDebugToggle && GameplayInput.GetKeyDown(this.toggleKey) && this.CanToggle())
			{
				if (this._active)
				{
					this.ExitMode();
				}
				else
				{
					this.TryEnterModeInternal(true);
				}
			}
			if (this._active)
			{
				this.FreezeWorld();
			}
			if (this._active && this._paintReady)
			{
				this.TickColliderRebake(ControllerInput.IsActive ? ControllerWorldPointer.Down(ControllerButton.RightTrigger) : GameplayInput.GetMouseButtonDown(0));
				this.TickPainting();
				if (this._paintingItem)
				{
					this._itemSurface.FlushPaint();
				}
				if (!this._paintingItem)
				{
					this._canvasService.FlushDilation();
					return;
				}
				if (ControllerInput.IsActive ? (!ControllerWorldPointer.Held(ControllerButton.RightTrigger)) : (!GameplayInput.GetMouseButton(0)))
				{
					this._itemSurface.CommitDesign();
				}
			}
		}

		// Token: 0x0600A673 RID: 42611 RVA: 0x004593D3 File Offset: 0x004575D3
		public bool CanEnterMode()
		{
			return this.CanEnterMode(false);
		}

		// Token: 0x0600A674 RID: 42612 RVA: 0x004593DC File Offset: 0x004575DC
		public bool CanEnterHeldItemMode()
		{
			return this.CanEnterMode(true);
		}

		// Token: 0x0600A675 RID: 42613 RVA: 0x004593E8 File Offset: 0x004575E8
		private bool CanEnterMode(bool heldItem)
		{
			if (this._active)
			{
				return false;
			}
			if (heldItem)
			{
				PlayerContextResource playerContextResource;
				if (PlayerActivityGate.GetResourceOwner<HideFlashController>(PlayerContextResource.HeldItem) == null || PlayerActivityGate.IsResourceOwned(PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.ModalUI) || !PlayerActivityGate.CanAcquireUtilityModal(PlayerActivity.Inventory, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.ModalUI | PlayerContextResource.HeldItem, out playerContextResource, null))
				{
					return false;
				}
			}
			else if (!PlayerActivityGate.Allows(PlayerActivity.Inventory) || PlayerActivityGate.IsResourceOwned(PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.ModalUI | PlayerContextResource.HeldItem))
			{
				return false;
			}
			if (FlyCamera.IsActive)
			{
				return false;
			}
			if (Time.timeScale <= 0f)
			{
				return false;
			}
			if (this._playerInput == null || this._playerInput.cc == null)
			{
				return false;
			}
			vThirdPersonController cc = this._playerInput.cc;
			return cc.isGrounded && !cc.customAction && !cc.ragdolled;
		}

		// Token: 0x0600A676 RID: 42614 RVA: 0x004594A5 File Offset: 0x004576A5
		private bool CanToggle()
		{
			return this._active || this.CanEnterMode();
		}

		// Token: 0x0600A677 RID: 42615 RVA: 0x004594B7 File Offset: 0x004576B7
		public bool TryEnterMode()
		{
			return this.TryEnterModeInternal(false);
		}

		// Token: 0x0600A678 RID: 42616 RVA: 0x004594C0 File Offset: 0x004576C0
		public bool TryEnterItemMode(NeckSignSurface surface)
		{
			if (this._active || surface == null || !surface.isActiveAndEnabled || this._playerRoot == null || !surface.transform.IsChildOf(this._playerRoot))
			{
				return false;
			}
			this._itemSurface = surface;
			this._paintingItem = true;
			if (this.TryEnterModeInternal(false))
			{
				return true;
			}
			this._itemSurface = null;
			this._paintingItem = false;
			return false;
		}

		// Token: 0x0600A679 RID: 42617 RVA: 0x00459534 File Offset: 0x00457734
		public bool TryEnterHeldItemMode(NeckSignSurface surface)
		{
			if (this._active || surface == null || !surface.isActiveAndEnabled || this._playerRoot == null || !surface.transform.IsChildOf(this._playerRoot) || !BodyPaintModeController.HeldOwnerOwnsSurface(surface))
			{
				return false;
			}
			this._itemSurface = surface;
			this._paintingItem = true;
			this._paintingHeldItem = true;
			if (this.TryEnterModeInternal(false))
			{
				return true;
			}
			this._itemSurface = null;
			this._paintingItem = false;
			this._paintingHeldItem = false;
			return false;
		}

		// Token: 0x0600A67A RID: 42618 RVA: 0x004595BC File Offset: 0x004577BC
		private static bool HeldOwnerOwnsSurface(NeckSignSurface surface)
		{
			HideFlashController resourceOwner = PlayerActivityGate.GetResourceOwner<HideFlashController>(PlayerContextResource.HeldItem);
			return resourceOwner != null && resourceOwner.OwnsPaintSurface(surface);
		}

		// Token: 0x0600A67B RID: 42619 RVA: 0x004595E4 File Offset: 0x004577E4
		private bool TryEnterModeInternal(bool debugEntry)
		{
			if (!this.CanEnterMode(this._paintingHeldItem) || !this.TryAcquireActivityContext())
			{
				return false;
			}
			if (!base.isActiveAndEnabled || this._activityLease == null || !this._activityLease.IsActive)
			{
				this.ReleaseActivityContext();
				return false;
			}
			bool active;
			try
			{
				if (debugEntry)
				{
					RunModeService.DisqualifyLeaderboardRun("Body painting was opened with a debug shortcut.");
				}
				this.BeginMode();
				active = this._active;
			}
			catch
			{
				this.ExitMode();
				throw;
			}
			return active;
		}

		// Token: 0x0600A67C RID: 42620 RVA: 0x00459668 File Offset: 0x00457868
		public void EnterMode()
		{
			this.TryEnterMode();
		}

		// Token: 0x0600A67D RID: 42621 RVA: 0x00459674 File Offset: 0x00457874
		private bool TryAcquireActivityContext()
		{
			if (this._activityLease != null)
			{
				if (this._activityLease.IsActive)
				{
					return true;
				}
				this._activityLease = null;
			}
			if (this._paintingHeldItem)
			{
				return PlayerActivityGate.TryAcquireUtilityModal("BodyPaint#" + base.GetInstanceID().ToString(), this, PlayerActivity.Inventory, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.ModalUI | PlayerContextResource.HeldItem, PlayerActivity.AllPlayerActions, out this._activityLease, null);
			}
			return PlayerActivityGate.TryAcquire("BodyPaint#" + base.GetInstanceID().ToString(), this, PlayerActivity.Inventory, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.ModalUI | PlayerContextResource.HeldItem, PlayerActivity.AllPlayerActions, out this._activityLease, false, null);
		}

		// Token: 0x0600A67E RID: 42622 RVA: 0x0045970B File Offset: 0x0045790B
		private void ReleaseActivityContext()
		{
			if (this._activityLease == null)
			{
				return;
			}
			this._activityLease.Dispose();
			this._activityLease = null;
		}

		// Token: 0x0600A67F RID: 42623 RVA: 0x00459728 File Offset: 0x00457928
		private void BeginMode()
		{
			if (this._active)
			{
				return;
			}
			this._active = true;
			this._paintReady = false;
			this.FreezeWorld();
			if (!this._paintingHeldItem)
			{
				this.UseUnscaledPlayerAnimator();
			}
			if (this._playerInput != null)
			{
				this._prevLockInput = this._playerInput.lockInput;
				this._prevLockCamera = this._playerInput.lockCameraInput;
				this._playerInput.SetLockAllInput(true);
				this._playerInput.SetLockCameraInput(true);
				if (this._playerInput.cc != null)
				{
					this._playerInput.cc.input = Vector2.zero;
					this._playerInput.cc.isSprinting = false;
				}
			}
			this._prevCursorLock = Cursor.lockState;
			this._prevCursorVisible = Cursor.visible;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			if (!this._paintingItem)
			{
				this.HideFaceAccessories();
			}
			else if (!this._paintingHeldItem)
			{
				this.HideOtherNeckSignRenderers();
			}
			if (!this._paintingItem)
			{
				this._canvasService.EnsureOverlays();
			}
			if (!this._paintingItem)
			{
				this.HideBlushMaterials();
			}
			if (!this._paintingHeldItem)
			{
				if (this._actionAnimator != null)
				{
					this._actionAnimator.DisableEmotionLayers();
				}
				this._currentPoseIndex = Mathf.Max(1, this.poseIndex);
				this.SetPose(this._currentPoseIndex);
			}
			this._mainCamera = Camera.main;
			vThirdPersonCamera vThirdPersonCamera = ((this._playerInput != null) ? this._playerInput.tpCamera : vThirdPersonCamera.instance);
			this._orbitCamera.Activate(vThirdPersonCamera, this._playerRoot);
			this.EnsureUI();
			if (this._ui != null)
			{
				this._ui.gameObject.SetActive(true);
				this._ui.ConfigureSurface(this._paintingHeldItem ? "DRAW ON CHALKBOARD" : (this._paintingItem ? ((this._itemSurface.PartId == "Accessory.NeckTag") ? "PAINT NECK TAG" : "PAINT HANGING SIGN") : "BODY PAINT"), this._paintingItem && !this._paintingHeldItem, !this._paintingHeldItem);
				this._ui.ConfigurePresets(this._paintingItem ? null : this._canvasService, this._mainCamera);
				if (this._paintingHeldItem)
				{
					this._ui.SetColor(Color.white);
				}
				if (this._ui.BrushCursor != null)
				{
					this._ui.BrushCursor.SetActive(true);
				}
			}
			this._orbitCamera.InputBlocker = () => this._ui != null && (this._ui.IsPresetDialogOpen || this.IsPointerOverPaintUI(ControllerWorldPointer.Position(this)));
			this.HideGameHUD();
			if (this._paintingHeldItem)
			{
				this._itemSurface.BuildPaintCollider();
				this._orbitCamera.FramePaintSurface(this._itemSurface);
				this._paintReady = true;
			}
			else
			{
				this._enterRoutine = base.StartCoroutine(this.PreparePaintSurfaceRoutine());
			}
			if (this.debugLogs)
			{
				Debug.Log("[BodyPaintModeController] Enter body paint mode");
			}
		}

		// Token: 0x0600A680 RID: 42624 RVA: 0x00459A21 File Offset: 0x00457C21
		private IEnumerator PreparePaintSurfaceRoutine()
		{
			yield return new WaitForSecondsRealtime(this.poseSettleSeconds);
			if (this._playerRoot != null)
			{
				foreach (NeckSignSpring neckSignSpring in this._playerRoot.GetComponentsInChildren<NeckSignSpring>())
				{
					if (neckSignSpring.isActiveAndEnabled)
					{
						neckSignSpring.PrepareForPainting();
					}
				}
			}
			if (this._paintingItem)
			{
				if (this._itemSurface == null || !this._itemSurface.isActiveAndEnabled)
				{
					this.ExitMode();
					yield break;
				}
				this._itemSurface.BuildPaintCollider();
				Bounds paintBounds = this._itemSurface.PaintBounds;
				this._orbitCamera.FollowAnimatedPivotFull(this._itemSurface.transform, paintBounds.center - this._itemSurface.transform.position);
				this._orbitCamera.EnableZoom(Mathf.Clamp(paintBounds.size.magnitude * 2.4f, 0.35f, 1.2f), 0.12f, 2f);
			}
			else
			{
				this._canvasService.BuildPaintColliders(null);
			}
			Physics.SyncTransforms();
			this._lastBakeTime = Time.unscaledTime;
			this._paintReady = true;
			this._enterRoutine = null;
			if (this.debugLogs)
			{
				Debug.Log("[BodyPaintModeController] Paint surface pronta (tempo freezato)");
			}
			yield break;
		}

		// Token: 0x0600A681 RID: 42625 RVA: 0x00459A30 File Offset: 0x00457C30
		public void SelectPose(int index)
		{
			if (!this._active || this._paintingHeldItem || index < 1)
			{
				return;
			}
			if (index == this._currentPoseIndex && this._paintReady)
			{
				return;
			}
			this._currentPoseIndex = index;
			this._paintReady = false;
			this._lastPaintScreenPos = null;
			if (this._enterRoutine != null)
			{
				base.StopCoroutine(this._enterRoutine);
			}
			this.SetPose(index);
			this._enterRoutine = base.StartCoroutine(this.PreparePaintSurfaceRoutine());
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[BodyPaintModeController] Cambio posa -> {0}", index));
			}
		}

		// Token: 0x0600A682 RID: 42626 RVA: 0x00459AC8 File Offset: 0x00457CC8
		private void HideGameHUD()
		{
			this._disabledCanvases.Clear();
			Canvas canvas = ((this._ui != null) ? this._ui.GetComponentInParent<Canvas>() : null);
			foreach (Canvas canvas2 in Object.FindObjectsOfType<Canvas>())
			{
				if (canvas2.isRootCanvas && canvas2.enabled && canvas2.renderMode != RenderMode.WorldSpace && (!(canvas != null) || !(canvas2 == canvas)))
				{
					canvas2.enabled = false;
					this._disabledCanvases.Add(canvas2);
				}
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[BodyPaintModeController] HUD nascosto: {0} canvas", this._disabledCanvases.Count));
			}
		}

		// Token: 0x0600A683 RID: 42627 RVA: 0x00459B7C File Offset: 0x00457D7C
		private void RestoreGameHUD()
		{
			foreach (Canvas canvas in this._disabledCanvases)
			{
				if (canvas != null)
				{
					canvas.enabled = true;
				}
			}
			this._disabledCanvases.Clear();
		}

		// Token: 0x0600A684 RID: 42628 RVA: 0x00459BE4 File Offset: 0x00457DE4
		private void HideOtherNeckSignRenderers()
		{
			this._hiddenNeckRenderers.Clear();
			if (this._playerRoot == null)
			{
				return;
			}
			foreach (NeckSignSurface neckSignSurface in this._playerRoot.GetComponentsInChildren<NeckSignSurface>())
			{
				if (!(neckSignSurface == this._itemSurface))
				{
					foreach (Renderer renderer in neckSignSurface.GetComponentsInChildren<Renderer>())
					{
						if (renderer.enabled)
						{
							this._hiddenNeckRenderers.Add(renderer);
							renderer.enabled = false;
						}
					}
				}
			}
		}

		// Token: 0x0600A685 RID: 42629 RVA: 0x00459C74 File Offset: 0x00457E74
		private void RestoreOtherNeckSignRenderers()
		{
			foreach (Renderer renderer in this._hiddenNeckRenderers)
			{
				if (renderer != null)
				{
					renderer.enabled = true;
				}
			}
			this._hiddenNeckRenderers.Clear();
		}

		// Token: 0x0600A686 RID: 42630 RVA: 0x00459CDC File Offset: 0x00457EDC
		private void HideFaceAccessories()
		{
			if (this._clothingToggle == null)
			{
				this._clothingToggle = this._playerRoot.GetComponentInChildren<ClothingToggleController>(true);
			}
			this._hiddenSlots.Clear();
			if (this._clothingToggle == null)
			{
				return;
			}
			foreach (string text in this.hiddenSlotsDuringPaint)
			{
				if (this._clothingToggle.ManagesSlot(text) && this._clothingToggle.IsSlotVisible(text))
				{
					this._clothingToggle.SetSlotVisible(text, false);
					this._hiddenSlots.Add(text);
				}
			}
			if (this.debugLogs && this._hiddenSlots.Count > 0)
			{
				Debug.Log("[BodyPaintModeController] Accessori nascosti: " + string.Join(", ", this._hiddenSlots));
			}
		}

		// Token: 0x0600A687 RID: 42631 RVA: 0x00459DA8 File Offset: 0x00457FA8
		private void HideBlushMaterials()
		{
			this._blushSwappedRenderers.Clear();
			if (this.invisibleMaterial == null)
			{
				Debug.LogWarning("[BodyPaintModeController] invisibleMaterial non assegnato: esegui Tools > Body Paint > Setup Player. Blush NON nascosto.", this);
				return;
			}
			foreach (Renderer renderer in this._playerRoot.GetComponentsInChildren<Renderer>(true))
			{
				Material[] sharedMaterials = renderer.sharedMaterials;
				bool flag = false;
				for (int j = 0; j < sharedMaterials.Length; j++)
				{
					if (!(sharedMaterials[j] == null) && !(sharedMaterials[j] == this.invisibleMaterial) && sharedMaterials[j].name.IndexOf("blush", StringComparison.OrdinalIgnoreCase) >= 0)
					{
						if (!flag)
						{
							this._blushSwappedRenderers.Add(new KeyValuePair<Renderer, Material[]>(renderer, renderer.sharedMaterials));
							flag = true;
						}
						sharedMaterials[j] = this.invisibleMaterial;
					}
				}
				if (flag)
				{
					renderer.sharedMaterials = sharedMaterials;
				}
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[BodyPaintModeController] Blush nascosto su {0} renderer", this._blushSwappedRenderers.Count));
			}
		}

		// Token: 0x0600A688 RID: 42632 RVA: 0x00459EAC File Offset: 0x004580AC
		private void RestoreBlushMaterials()
		{
			foreach (KeyValuePair<Renderer, Material[]> keyValuePair in this._blushSwappedRenderers)
			{
				if (keyValuePair.Key != null)
				{
					keyValuePair.Key.sharedMaterials = keyValuePair.Value;
				}
			}
			this._blushSwappedRenderers.Clear();
		}

		// Token: 0x0600A689 RID: 42633 RVA: 0x00459F28 File Offset: 0x00458128
		private void RestoreFaceAccessories()
		{
			if (this._clothingToggle == null)
			{
				return;
			}
			foreach (string text in this._hiddenSlots)
			{
				this._clothingToggle.SetSlotVisible(text, true);
			}
			this._hiddenSlots.Clear();
		}

		// Token: 0x0600A68A RID: 42634 RVA: 0x00459F9C File Offset: 0x0045819C
		private void FreezeWorld()
		{
			if (!this._timeFrozen)
			{
				this._prevTimeScale = Time.timeScale;
				Time.timeScale = 0f;
				this._timeFrozen = true;
			}
			else if (Time.timeScale != 0f)
			{
				Time.timeScale = 0f;
			}
			SensoryManager instance = SensoryManager.Instance;
			if (!this._detectionPauseApplied && instance != null)
			{
				this._previousDetectionPaused = instance.IsPaused;
				instance.SetPaused(true);
				this._detectionPauseApplied = true;
			}
		}

		// Token: 0x0600A68B RID: 42635 RVA: 0x0045A018 File Offset: 0x00458218
		private void RestoreWorld()
		{
			if (this._detectionPauseApplied)
			{
				SensoryManager instance = SensoryManager.Instance;
				if (instance != null)
				{
					instance.SetPaused(this._previousDetectionPaused);
				}
				this._detectionPauseApplied = false;
			}
			if (this._timeFrozen)
			{
				Time.timeScale = this._prevTimeScale;
				this._timeFrozen = false;
			}
		}

		// Token: 0x0600A68C RID: 42636 RVA: 0x0045A069 File Offset: 0x00458269
		private void UseUnscaledPlayerAnimator()
		{
			if (this._animator == null || this._animatorUsesUnscaledTime)
			{
				return;
			}
			this._previousAnimatorUpdateMode = this._animator.updateMode;
			this._animator.updateMode = AnimatorUpdateMode.UnscaledTime;
			this._animatorUsesUnscaledTime = true;
		}

		// Token: 0x0600A68D RID: 42637 RVA: 0x0045A0A6 File Offset: 0x004582A6
		private void RestorePlayerAnimatorUpdateMode()
		{
			if (this._animator == null || !this._animatorUsesUnscaledTime)
			{
				return;
			}
			this._animator.updateMode = this._previousAnimatorUpdateMode;
			this._animatorUsesUnscaledTime = false;
		}

		// Token: 0x0600A68E RID: 42638 RVA: 0x0045A0D8 File Offset: 0x004582D8
		private void TickColliderRebake(bool strokeStarting)
		{
			if (this._timeFrozen)
			{
				return;
			}
			float num = (strokeStarting ? 0.15f : Mathf.Max(0.2f, this.rebakeIntervalSeconds));
			if (Time.unscaledTime - this._lastBakeTime < num)
			{
				return;
			}
			if (this._paintingItem)
			{
				this._itemSurface.BuildPaintCollider();
			}
			else
			{
				this._canvasService.BuildPaintColliders(new BodyPaintCanvasService.PaintChannel?(BodyPaintCanvasService.PaintChannel.Body));
			}
			Physics.SyncTransforms();
			this._lastBakeTime = Time.unscaledTime;
		}

		// Token: 0x0600A68F RID: 42639 RVA: 0x0045A150 File Offset: 0x00458350
		public void ExitMode()
		{
			if (!this._active)
			{
				return;
			}
			bool paintingHeldItem = this._paintingHeldItem;
			this._active = false;
			this._paintReady = false;
			this._lastPaintScreenPos = null;
			if (this._enterRoutine != null)
			{
				base.StopCoroutine(this._enterRoutine);
				this._enterRoutine = null;
			}
			if (this._paintingItem)
			{
				if (this._itemSurface != null)
				{
					this._itemSurface.CommitDesign();
					this._itemSurface.DestroyPaintCollider();
				}
			}
			else
			{
				this._canvasService.RefreshCoverage();
				this._canvasService.DestroyPaintColliders();
			}
			this._itemSurface = null;
			this._paintingItem = false;
			this._paintingHeldItem = false;
			this.RestoreOtherNeckSignRenderers();
			this.RestoreFaceAccessories();
			this.RestoreBlushMaterials();
			this.RestoreGameHUD();
			if (this._pickRoutine != null)
			{
				base.StopCoroutine(this._pickRoutine);
				this._pickRoutine = null;
			}
			if (!paintingHeldItem)
			{
				this.SetPose(0);
			}
			if (!paintingHeldItem && this._actionAnimator != null)
			{
				this._actionAnimator.RestoreOverrideLayer();
			}
			this._orbitCamera.Deactivate();
			if (this._ui != null)
			{
				this._ui.ConfigurePresets(null, null);
				if (this._ui.BrushCursor != null)
				{
					this._ui.BrushCursor.SetActive(false);
				}
				this._ui.gameObject.SetActive(false);
			}
			Cursor.lockState = this._prevCursorLock;
			Cursor.visible = this._prevCursorVisible;
			if (this._playerInput != null)
			{
				this._playerInput.SetLockAllInput(this._prevLockInput);
				this._playerInput.SetLockCameraInput(this._prevLockCamera);
			}
			this.RestorePlayerAnimatorUpdateMode();
			this.RestoreWorld();
			this.ReleaseActivityContext();
			if (this.debugLogs)
			{
				Debug.Log("[BodyPaintModeController] Exit body paint mode");
			}
		}

		// Token: 0x0600A690 RID: 42640 RVA: 0x0045A317 File Offset: 0x00458517
		private void OnDisable()
		{
			if (this._active)
			{
				this.ExitMode();
				return;
			}
			this.ReleaseActivityContext();
		}

		// Token: 0x0600A691 RID: 42641 RVA: 0x0045A317 File Offset: 0x00458517
		private void OnDestroy()
		{
			if (this._active)
			{
				this.ExitMode();
				return;
			}
			this.ReleaseActivityContext();
		}

		// Token: 0x0600A692 RID: 42642 RVA: 0x0045A330 File Offset: 0x00458530
		private void SetPose(int index)
		{
			if (this._animator == null)
			{
				return;
			}
			if (index <= 0)
			{
				this.CrossFadeToLocomotion();
				return;
			}
			int num = Animator.StringToHash("BodyPaintPose_" + index.ToString());
			if (!this._animator.HasState(0, num))
			{
				Debug.LogWarning(string.Format("[BodyPaintModeController] Stato '{0}{1}' non trovato ", "BodyPaintPose_", index) + "sul Base Layer: esegui Tools > Body Paint > Build Pose States.", this);
				return;
			}
			this._animator.CrossFadeInFixedTime(num, this.poseBlendSeconds, 0);
		}

		// Token: 0x0600A693 RID: 42643 RVA: 0x0045A3B8 File Offset: 0x004585B8
		private void CrossFadeToLocomotion()
		{
			string[] array = new string[] { "Free Movement", "Locomotion", "Idle", "Grounded" };
			for (int i = 0; i < array.Length; i++)
			{
				int num = Animator.StringToHash(array[i]);
				if (this._animator.HasState(0, num))
				{
					this._animator.CrossFadeInFixedTime(num, this.poseBlendSeconds, 0);
					return;
				}
			}
			Debug.LogWarning("[BodyPaintModeController] Nessuno stato locomotion trovato sul Base Layer.", this);
		}

		// Token: 0x0600A694 RID: 42644 RVA: 0x0045A434 File Offset: 0x00458634
		private void EnsureUI()
		{
			if (this._ui != null)
			{
				return;
			}
			if (this.uiPrefab == null)
			{
				Debug.LogError("[BodyPaintModeController] uiPrefab non assegnato: esegui Tools > Body Paint > Build UI Prefab e assegnalo.", this);
				return;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(this.uiPrefab);
			gameObject.name = "BodyPaintUI";
			this._ui = gameObject.GetComponentInChildren<BodyPaintUIController>(true);
			if (this._ui == null)
			{
				Debug.LogError("[BodyPaintModeController] BodyPaintUIController mancante nel prefab UI!", this);
				return;
			}
			this._ui.OnExitRequested += this.ExitMode;
			this._ui.OnClearRequested += this.ClearCurrentPaint;
			this._ui.OnPoseSelected += this.SelectPose;
		}

		// Token: 0x0600A695 RID: 42645 RVA: 0x0045A4ED File Offset: 0x004586ED
		public void ClearCurrentPaint()
		{
			if (!this._active)
			{
				return;
			}
			if (this._paintingItem)
			{
				if (this._itemSurface != null)
				{
					this._itemSurface.ClearPaint();
					return;
				}
			}
			else
			{
				this._canvasService.ClearAllPaint();
			}
		}

		// Token: 0x0600A696 RID: 42646 RVA: 0x0045A528 File Offset: 0x00458728
		private bool IsPointerOverPaintUI(Vector2 screenPos)
		{
			if (EventSystem.current == null || this._ui == null)
			{
				return false;
			}
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
			{
				position = screenPos
			};
			this._uiRaycastResults.Clear();
			EventSystem.current.RaycastAll(pointerEventData, this._uiRaycastResults);
			Transform transform = this._ui.transform;
			for (int i = 0; i < this._uiRaycastResults.Count; i++)
			{
				GameObject gameObject = this._uiRaycastResults[i].gameObject;
				if (gameObject != null && gameObject.transform.IsChildOf(transform))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A697 RID: 42647 RVA: 0x0045A5D4 File Offset: 0x004587D4
		private void TickPainting()
		{
			if (this._ui == null || this._mainCamera == null)
			{
				return;
			}
			if (this._ui.IsPresetDialogOpen)
			{
				this._lastPaintScreenPos = null;
				this._waitForPaintRelease = true;
				return;
			}
			if (this._waitForPaintRelease)
			{
				this._lastPaintScreenPos = null;
				if (ControllerInput.IsActive ? ControllerWorldPointer.Held(ControllerButton.RightTrigger) : GameplayInput.GetMouseButton(0))
				{
					return;
				}
				this._waitForPaintRelease = false;
			}
			bool flag = this.IsPointerOverPaintUI(ControllerWorldPointer.Position(this));
			if (this.debugLogs && (ControllerInput.IsActive ? ControllerWorldPointer.Down(ControllerButton.RightTrigger) : GameplayInput.GetMouseButtonDown(0)))
			{
				Debug.Log(string.Format("[BodyPaintModeController] click: paintReady={0} overUI={1}", this._paintReady, flag));
			}
			if (this._ui.IsEyedropper)
			{
				if ((ControllerInput.IsActive ? ControllerWorldPointer.Down(ControllerButton.RightTrigger) : GameplayInput.GetMouseButtonDown(0)) && !flag && this._pickRoutine == null)
				{
					this._pickRoutine = base.StartCoroutine(this.PickColorRoutine(ControllerWorldPointer.Position(this)));
				}
				this._lastPaintScreenPos = null;
				return;
			}
			if (!(ControllerInput.IsActive ? ControllerWorldPointer.Held(ControllerButton.RightTrigger) : GameplayInput.GetMouseButton(0)) || flag)
			{
				this._lastPaintScreenPos = null;
				return;
			}
			Vector2 vector = ControllerWorldPointer.Position(this);
			if (this._lastPaintScreenPos == null)
			{
				this.PaintAtScreenPos(vector);
				this._lastPaintScreenPos = new Vector2?(vector);
				return;
			}
			Vector2 value = this._lastPaintScreenPos.Value;
			Vector2 vector2 = vector - value;
			float magnitude = vector2.magnitude;
			if (magnitude <= Mathf.Max(0f, this.stabilizerRadiusPx))
			{
				return;
			}
			Vector2 vector3 = value + vector2 * ((magnitude - this.stabilizerRadiusPx) / magnitude);
			float num = Mathf.Max(2f, this.BrushScreenRadiusPx() * this.stampSpacingFraction);
			int num2 = Mathf.Clamp(Mathf.CeilToInt(Vector2.Distance(value, vector3) / num), 1, 128);
			for (int i = 1; i <= num2; i++)
			{
				this.PaintAtScreenPos(Vector2.Lerp(value, vector3, (float)i / (float)num2));
			}
			this._lastPaintScreenPos = new Vector2?(vector3);
		}

		// Token: 0x0600A698 RID: 42648 RVA: 0x0045A7F8 File Offset: 0x004589F8
		private IEnumerator PickColorRoutine(Vector2 screenPos)
		{
			yield return new WaitForEndOfFrame();
			Texture2D texture2D = new Texture2D(1, 1, TextureFormat.RGB24, false);
			texture2D.ReadPixels(new Rect(screenPos.x, screenPos.y, 1f, 1f), 0, 0);
			texture2D.Apply();
			Color pixel = texture2D.GetPixel(0, 0);
			Object.Destroy(texture2D);
			if (this._ui != null)
			{
				this._ui.SetColor(pixel);
				this._ui.ClearEyedropper();
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[BodyPaintModeController] Contagocce: {0}", pixel));
			}
			this._pickRoutine = null;
			yield break;
		}

		// Token: 0x0600A699 RID: 42649 RVA: 0x0045A810 File Offset: 0x00458A10
		private float BrushScreenRadiusPx()
		{
			if (this._ui == null || this._mainCamera == null)
			{
				return 8f;
			}
			float num = Mathf.Tan(this._mainCamera.fieldOfView * 0.5f * 0.017453292f);
			float num2 = Mathf.Max(0.05f, this._lastHitDistance);
			return this._ui.BrushRadiusMeters * ((float)Screen.height * 0.5f) / (num * num2);
		}

		// Token: 0x0600A69A RID: 42650 RVA: 0x0045A88C File Offset: 0x00458A8C
		private void PaintAtScreenPos(Vector2 screenPos)
		{
			Ray ray = this._mainCamera.ScreenPointToRay(screenPos);
			if (this._paintingItem)
			{
				Vector3 vector;
				if (this._itemSurface != null && this._itemSurface.RaycastPaintSurface(ray, out vector))
				{
					this._lastHitDistance = Vector3.Distance(this._mainCamera.transform.position, vector);
					this._itemSurface.StampBrushWorld(vector, ray.direction, this._ui.CurrentColor, this._ui.BrushRadiusMeters, 0.6f, this._ui.BrushOpacity, this._ui.IsEraser);
				}
				return;
			}
			BodyPaintCanvasService.PaintChannel paintChannel;
			Vector2 vector2;
			Vector3 vector3;
			if (!this._canvasService.RaycastPaintSurface(ray, out paintChannel, out vector2, out vector3))
			{
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[BodyPaintModeController] raycast MISS a screenPos={0}", screenPos));
				}
				return;
			}
			this._lastHitDistance = Vector3.Distance(this._mainCamera.transform.position, vector3);
			this._canvasService.StampBrushWorld(vector3, ray.direction, this._ui.CurrentColor, this._ui.BrushRadiusMeters, 0.6f, this._ui.BrushOpacity, this._ui.IsEraser);
		}

		// Token: 0x0400AB3D RID: 43837
		private const string GateReason = "BodyPaint";

		// Token: 0x0400AB3E RID: 43838
		private const PlayerActivity RequiredActivity = PlayerActivity.Inventory;

		// Token: 0x0400AB3F RID: 43839
		private const PlayerContextResource ModeResources = PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.ModalUI | PlayerContextResource.HeldItem;

		// Token: 0x0400AB40 RID: 43840
		private const string PoseStatePrefix = "BodyPaintPose_";

		// Token: 0x0400AB41 RID: 43841
		[Header("Input")]
		[Tooltip("Scorciatoia di sviluppo opzionale. Il percorso ufficiale usa la Custom Body Paint Pen.")]
		[SerializeField]
		private KeyCode toggleKey = KeyCode.U;

		// Token: 0x0400AB42 RID: 43842
		[SerializeField]
		private bool enableDebugToggle;

		// Token: 0x0400AB43 RID: 43843
		[Header("Pose")]
		[Tooltip("Posa iniziale del layer BodyPaintPose (1 = Statue1, 2 = Statue2)")]
		[SerializeField]
		private int poseIndex = 1;

		// Token: 0x0400AB44 RID: 43844
		[Tooltip("Durata del crossfade dentro/fuori gli stati posa (Base Layer)")]
		[SerializeField]
		private float poseBlendSeconds = 0.25f;

		// Token: 0x0400AB45 RID: 43845
		[Tooltip("Attesa prima del bake dei collider di painting (la posa deve assestarsi)")]
		[SerializeField]
		private float poseSettleSeconds = 0.9f;

		// Token: 0x0400AB46 RID: 43846
		[Tooltip("Ogni quanto ri-bakare il collider del corpo: le MagicaCloth (tette/sedere) continuano a muoversi, il collider deve inseguire la superficie visibile")]
		[SerializeField]
		private float rebakeIntervalSeconds = 0.7f;

		// Token: 0x0400AB47 RID: 43847
		[Header("UI")]
		[Tooltip("Prefab con BodyPaintUIController (creato da Tools/Body Paint/Build UI)")]
		[SerializeField]
		private GameObject uiPrefab;

		// Token: 0x0400AB48 RID: 43848
		[Header("Stabilizzatore pennello (stile Krita)")]
		[Tooltip("Guinzaglio in pixel: il pennello segue il mouse solo oltre questo raggio. Il tremolio della mano dentro il raggio viene assorbito → linee lisce. 0 = disattivo.")]
		[SerializeField]
		private float stabilizerRadiusPx = 12f;

		// Token: 0x0400AB49 RID: 43849
		[Tooltip("Distanza tra gli stamp come frazione del raggio del pennello a schermo (piu' basso = tratto piu' denso e uniforme)")]
		[SerializeField]
		[Range(0.1f, 1f)]
		private float stampSpacingFraction = 0.35f;

		// Token: 0x0400AB4A RID: 43850
		[Header("Accessori viso")]
		[Tooltip("Slot nascosti durante il painting: gli occhiali/mascherina coprono la faccia e il paint finirebbe NASCOSTO dietro di loro (il pennello colpisce la pelle sotto). Vengono ripristinati all'uscita.")]
		[SerializeField]
		private string[] hiddenSlotsDuringPaint = new string[] { "Accessory.Eyewear", "Outfit.Mask" };

		// Token: 0x0400AB4B RID: 43851
		[Tooltip("Materiale che non renderizza nulla (shader BodyPaint/Invisible): durante il painting sostituisce i materiali 'blush' cosi' il player dipinge la PELLE, non il rossore sopra. Ripristinati all'uscita. Assegnato da Setup Player.")]
		[SerializeField]
		private Material invisibleMaterial;

		// Token: 0x0400AB4C RID: 43852
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x0400AB4D RID: 43853
		private BodyPaintCanvasService _canvasService;

		// Token: 0x0400AB4E RID: 43854
		private BodyPaintOrbitCamera _orbitCamera;

		// Token: 0x0400AB4F RID: 43855
		private vThirdPersonInput _playerInput;

		// Token: 0x0400AB50 RID: 43856
		private Animator _animator;

		// Token: 0x0400AB51 RID: 43857
		private Transform _playerRoot;

		// Token: 0x0400AB52 RID: 43858
		private Camera _mainCamera;

		// Token: 0x0400AB53 RID: 43859
		private BodyPaintUIController _ui;

		// Token: 0x0400AB54 RID: 43860
		private PlayerActionAnimator _actionAnimator;

		// Token: 0x0400AB55 RID: 43861
		private bool _active;

		// Token: 0x0400AB56 RID: 43862
		private bool _paintReady;

		// Token: 0x0400AB57 RID: 43863
		private Coroutine _enterRoutine;

		// Token: 0x0400AB58 RID: 43864
		private PlayerActivityLease _activityLease;

		// Token: 0x0400AB59 RID: 43865
		private bool _prevLockInput;

		// Token: 0x0400AB5A RID: 43866
		private bool _prevLockCamera;

		// Token: 0x0400AB5B RID: 43867
		private CursorLockMode _prevCursorLock;

		// Token: 0x0400AB5C RID: 43868
		private bool _prevCursorVisible;

		// Token: 0x0400AB5D RID: 43869
		private Vector2? _lastPaintScreenPos;

		// Token: 0x0400AB5E RID: 43870
		private float _lastHitDistance = 2f;

		// Token: 0x0400AB5F RID: 43871
		private readonly List<RaycastResult> _uiRaycastResults = new List<RaycastResult>();

		// Token: 0x0400AB60 RID: 43872
		private float _lastBakeTime = float.NegativeInfinity;

		// Token: 0x0400AB61 RID: 43873
		private float _prevTimeScale = 1f;

		// Token: 0x0400AB62 RID: 43874
		private bool _timeFrozen;

		// Token: 0x0400AB63 RID: 43875
		private bool _detectionPauseApplied;

		// Token: 0x0400AB64 RID: 43876
		private bool _previousDetectionPaused;

		// Token: 0x0400AB65 RID: 43877
		private AnimatorUpdateMode _previousAnimatorUpdateMode;

		// Token: 0x0400AB66 RID: 43878
		private bool _animatorUsesUnscaledTime;

		// Token: 0x0400AB67 RID: 43879
		private int _currentPoseIndex;

		// Token: 0x0400AB68 RID: 43880
		private ClothingToggleController _clothingToggle;

		// Token: 0x0400AB69 RID: 43881
		private readonly List<string> _hiddenSlots = new List<string>();

		// Token: 0x0400AB6A RID: 43882
		private readonly List<Renderer> _hiddenNeckRenderers = new List<Renderer>();

		// Token: 0x0400AB6B RID: 43883
		private readonly List<KeyValuePair<Renderer, Material[]>> _blushSwappedRenderers = new List<KeyValuePair<Renderer, Material[]>>();

		// Token: 0x0400AB6C RID: 43884
		private readonly List<Canvas> _disabledCanvases = new List<Canvas>();

		// Token: 0x0400AB6D RID: 43885
		private Coroutine _pickRoutine;

		// Token: 0x0400AB6E RID: 43886
		private NeckSignSurface _itemSurface;

		// Token: 0x0400AB6F RID: 43887
		private bool _paintingItem;

		// Token: 0x0400AB70 RID: 43888
		private bool _paintingHeldItem;

		// Token: 0x0400AB71 RID: 43889
		private bool _waitForPaintRelease;
	}
}
