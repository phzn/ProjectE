using System;
using System.Collections;
using System.Globalization;
using System.IO;
using Features.DebugTools;
using Features.Inventory;
using Features.Loading;
using Features.MainMenu;
using Features.RenderingPrecision;
using Features.Vehicles.Traffic;
using Invector.vCamera;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.FirstPerson
{
	// Token: 0x020009EA RID: 2538
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(1100)]
	public sealed class FirstPersonCameraCalibration : MonoBehaviour
	{
		// Token: 0x17000C2B RID: 3115
		// (get) Token: 0x060047CC RID: 18380 RVA: 0x001B4674 File Offset: 0x001B2874
		// (set) Token: 0x060047CD RID: 18381 RVA: 0x001B467C File Offset: 0x001B287C
		public bool IsOpen { get; private set; }

		// Token: 0x17000C2C RID: 3116
		// (get) Token: 0x060047CE RID: 18382 RVA: 0x001B4685 File Offset: 0x001B2885
		// (set) Token: 0x060047CF RID: 18383 RVA: 0x001B468D File Offset: 0x001B288D
		public bool HasPreview { get; private set; }

		// Token: 0x17000C2D RID: 3117
		// (get) Token: 0x060047D0 RID: 18384 RVA: 0x001B4696 File Offset: 0x001B2896
		// (set) Token: 0x060047D1 RID: 18385 RVA: 0x001B469E File Offset: 0x001B289E
		public string LastSavedPath { get; private set; }

		// Token: 0x17000C2E RID: 3118
		// (get) Token: 0x060047D2 RID: 18386 RVA: 0x001B46A7 File Offset: 0x001B28A7
		public static bool IsSceneLoading
		{
			get
			{
				return (LoadingScreenController.ExistingInstance != null && LoadingScreenController.ExistingInstance.IsLoadingActive) || (SceneTransitionManager.Instance != null && SceneTransitionManager.Instance.IsTransitioning);
			}
		}

		// Token: 0x060047D3 RID: 18387 RVA: 0x001B46DD File Offset: 0x001B28DD
		private void Awake()
		{
			this.controller = base.GetComponent<FirstPersonController>();
			this.playerInput = base.GetComponent<vThirdPersonInput>();
		}

		// Token: 0x060047D4 RID: 18388 RVA: 0x001B46F7 File Offset: 0x001B28F7
		private void OnEnable()
		{
			WorldOrigin.Shifted += this.ApplyWorldOriginShift;
		}

		// Token: 0x060047D5 RID: 18389 RVA: 0x001B470A File Offset: 0x001B290A
		private void ApplyWorldOriginShift(Vector3 delta)
		{
			if (!this.hasBasePose)
			{
				return;
			}
			this.basePosition += delta;
			this.appliedPosition += delta;
		}

		// Token: 0x060047D6 RID: 18390 RVA: 0x001B4739 File Offset: 0x001B2939
		private void Update()
		{
			if (GameplayInputGate.BlocksGameplayInput)
			{
				return;
			}
			if (GameplayInput.GetKeyDown(KeyCode.F4) | (Keyboard.current != null && Keyboard.current.f4Key.wasPressedThisFrame))
			{
				this.toggleRequested = true;
			}
		}

		// Token: 0x060047D7 RID: 18391 RVA: 0x001B4770 File Offset: 0x001B2970
		private void LateUpdate()
		{
			if (this.controller == null || !this.controller.isActiveAndEnabled || !this.controller.IsFirstPerson || FirstPersonCameraCalibration.IsSceneLoading)
			{
				this.EndSession();
				this.toggleRequested = false;
				return;
			}
			if (this.HasPreview && (this.sessionParent != base.transform.parent || this.sessionAnchor != this.controller.CameraAnchorOverride || this.sessionHidden != (this.controller.YawClampOverrideCenter != null) || this.sessionMovementLocked != this.controller.IsRidingVehicle || this.sessionRoofRide != CarRoofCarrier.IsPlayerMounted || this.sessionCarriedItem != this.controller.CarriedCameraItem))
			{
				this.EndSession();
			}
			if (this.toggleRequested)
			{
				this.toggleRequested = false;
				if (this.IsOpen)
				{
					this.ResumePreview();
				}
				else
				{
					this.OpenPanel();
				}
			}
			if (!this.HasPreview || this.view == null || this.rig == null || this.controller.CameraPoseFrame != Time.frameCount)
			{
				return;
			}
			this.ApplyPreview();
			if (this.IsOpen)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
			if (this.saveRequested)
			{
				this.saveRequested = false;
				this.SaveCapture();
			}
		}

		// Token: 0x060047D8 RID: 18392 RVA: 0x001B48DC File Offset: 0x001B2ADC
		public bool OpenPanel()
		{
			if (this.IsOpen)
			{
				return true;
			}
			if (this.controller == null || !this.controller.IsFirstPerson || this.playerInput == null || this.playerInput.tpCamera == null || Camera.main == null)
			{
				return false;
			}
			if (Time.timeScale <= 0f || FlyCamera.IsActive || FirstPersonCameraCalibration.IsSceneLoading)
			{
				return false;
			}
			FirstPersonCalibrationContext firstPersonCalibrationContext = FirstPersonCalibrationContext.CaptureContext(this.controller);
			if (!PlayerActivityGate.TryAcquire("First person camera calibration", this, PlayerActivity.None, PlayerContextResource.ModalUI | PlayerContextResource.WorldSimulation, PlayerActivity.AllPlayerActions, out this.lease, false, null))
			{
				return false;
			}
			this.rig = this.playerInput.tpCamera;
			this.view = Camera.main;
			if (!this.HasPreview)
			{
				this.originalFov = this.view.fieldOfView;
				this.originalNear = this.view.nearClipPlane;
				this.adjustment = new FirstPersonCalibrationValues
				{
					fieldOfView = this.originalFov,
					nearClipPlane = this.originalNear
				};
				this.sessionParent = base.transform.parent;
				this.sessionAnchor = this.controller.CameraAnchorOverride;
				this.sessionHidden = this.controller.YawClampOverrideCenter != null;
				this.sessionMovementLocked = this.controller.IsRidingVehicle;
				this.sessionRoofRide = CarRoofCarrier.IsPlayerMounted;
				this.sessionCarriedItem = this.controller.CarriedCameraItem;
				this.HasPreview = true;
			}
			this.openedContext = firstPersonCalibrationContext;
			this.previousTimeScale = Time.timeScale;
			this.previousAudioPause = AudioListener.pause;
			this.previousInputLock = this.playerInput.lockInput;
			this.previousExternalLock = this.controller.ExternalLock;
			this.previousCursorLock = Cursor.lockState;
			this.previousCursorVisible = Cursor.visible;
			this.playerInput.lockInput = true;
			this.controller.ExternalLock = true;
			Time.timeScale = 0f;
			AudioListener.pause = true;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			this.IsOpen = true;
			this.status = "Posa fermata. Regola la visuale e salva il campione.";
			return true;
		}

		// Token: 0x060047D9 RID: 18393 RVA: 0x001B4B00 File Offset: 0x001B2D00
		private void ApplyPreview()
		{
			if (this.adjustment == null)
			{
				this.adjustment = new FirstPersonCalibrationValues();
			}
			this.adjustment.Sanitize();
			this.basePosition = this.rig.transform.position;
			this.baseRotation = this.rig.transform.rotation;
			this.hasBasePose = true;
			this.rig.transform.position += base.transform.rotation * this.adjustment.positionOffset;
			this.rig.transform.rotation *= Quaternion.Euler(this.adjustment.rotationOffset);
			this.appliedPosition = this.rig.transform.position;
			this.appliedRotation = this.rig.transform.rotation;
			this.view.fieldOfView = this.adjustment.fieldOfView;
			this.view.nearClipPlane = this.adjustment.nearClipPlane;
		}

		// Token: 0x060047DA RID: 18394 RVA: 0x001B4C18 File Offset: 0x001B2E18
		public void ResumePreview()
		{
			if (!this.IsOpen)
			{
				return;
			}
			this.IsOpen = false;
			this.saveRequested = false;
			if (this.playerInput != null)
			{
				this.playerInput.lockInput = this.previousInputLock;
			}
			if (this.controller != null)
			{
				this.controller.ExternalLock = this.previousExternalLock;
			}
			if (Time.timeScale == 0f)
			{
				Time.timeScale = this.previousTimeScale;
			}
			AudioListener.pause = this.previousAudioPause;
			Cursor.lockState = this.previousCursorLock;
			Cursor.visible = this.previousCursorVisible;
			PlayerActivityLease playerActivityLease = this.lease;
			if (playerActivityLease != null)
			{
				playerActivityLease.Dispose();
			}
			this.lease = null;
		}

		// Token: 0x060047DB RID: 18395 RVA: 0x001B4CCC File Offset: 0x001B2ECC
		public void EndSession()
		{
			this.ResumePreview();
			if (this.HasPreview && this.view != null)
			{
				this.view.fieldOfView = this.originalFov;
				if (this.controller != null && this.controller.IsFirstPerson)
				{
					this.view.nearClipPlane = this.originalNear;
				}
			}
			this.HasPreview = false;
			this.saveRequested = false;
			if (this.hasBasePose && this.rig != null && (this.rig.transform.position - this.appliedPosition).sqrMagnitude < 1E-07f && Quaternion.Angle(this.rig.transform.rotation, this.appliedRotation) < 0.05f)
			{
				this.rig.transform.SetPositionAndRotation(this.basePosition, this.baseRotation);
			}
			this.hasBasePose = false;
		}

		// Token: 0x060047DC RID: 18396 RVA: 0x001B4DC5 File Offset: 0x001B2FC5
		private void OnDisable()
		{
			WorldOrigin.Shifted -= this.ApplyWorldOriginShift;
			this.EndSession();
		}

		// Token: 0x060047DD RID: 18397 RVA: 0x001B4DDE File Offset: 0x001B2FDE
		private void OnDestroy()
		{
			this.EndSession();
		}

		// Token: 0x060047DE RID: 18398 RVA: 0x001B4DE8 File Offset: 0x001B2FE8
		private void SaveCapture()
		{
			try
			{
				FirstPersonCalibrationCapture firstPersonCalibrationCapture = new FirstPersonCalibrationCapture
				{
					capturedUtc = DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture),
					scene = SceneManager.GetActiveScene().path,
					situation = "Situazione da descrivere in chat",
					screenshotFile = "AUTO",
					adjustment = this.adjustment.Clone(),
					context = this.openedContext,
					controllerSettingsJson = JsonUtility.ToJson(new FirstPersonCameraCalibration.ControllerSettings(this.controller)),
					playerPosition = base.transform.position,
					playerEuler = base.transform.eulerAngles,
					cameraPosition = this.view.transform.position,
					cameraEuler = this.view.transform.eulerAngles,
					cameraInPlayerSpace = base.transform.InverseTransformPoint(this.view.transform.position),
					previousTimeScale = this.previousTimeScale
				};
				this.LastSavedPath = FirstPersonCalibrationStore.Save(firstPersonCalibrationCapture, null);
				this.status = "SALVATO: " + Path.GetFileName(this.LastSavedPath) + "\nOra scrivimi in chat in quale situazione hai fatto la taratura.";
				Debug.Log("[FP Calibration] Campione salvato: " + this.LastSavedPath, this);
				if (!this.capturingScreenshot)
				{
					base.StartCoroutine(this.CaptureScreenshot(Path.ChangeExtension(this.LastSavedPath, ".png")));
				}
			}
			catch (Exception ex)
			{
				this.status = "Salvataggio non riuscito: " + ex.Message;
				Debug.LogWarning("[FP Calibration] " + this.status, this);
			}
		}

		// Token: 0x060047DF RID: 18399 RVA: 0x001B4FA4 File Offset: 0x001B31A4
		public void RequestSave()
		{
			if (this.IsOpen && !this.capturingScreenshot)
			{
				this.saveRequested = true;
			}
		}

		// Token: 0x060047E0 RID: 18400 RVA: 0x001B4FC0 File Offset: 0x001B31C0
		private void OnGUI()
		{
			if (!this.HasPreview || this.capturingScreenshot)
			{
				return;
			}
			if (!GameSettingsService.ShowGameplayUI && SceneManager.GetActiveScene().name != "MainMenu")
			{
				return;
			}
			if (!this.IsOpen)
			{
				GUI.Box(new Rect(16f, 12f, 330f, 25f), "Camera in prova - F4 per regolare / salvare");
				return;
			}
			if (this.wrappedLabel == null)
			{
				this.wrappedLabel = new GUIStyle(GUI.skin.label)
				{
					wordWrap = true
				};
				this.titleLabel = new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold
				};
			}
			this.window.width = Mathf.Min(370f, (float)Screen.width - 24f);
			this.window.height = Mathf.Min(680f, (float)Screen.height - 54f);
			this.window.x = Mathf.Clamp(this.window.x, 0f, Mathf.Max(0f, (float)Screen.width - this.window.width));
			this.window.y = Mathf.Clamp(this.window.y, 0f, Mathf.Max(0f, (float)Screen.height - this.window.height));
			this.window = GUI.Window(base.GetInstanceID(), this.window, new GUI.WindowFunction(this.DrawWindow), "TARATURA PRIMA PERSONA - F4");
		}

		// Token: 0x060047E1 RID: 18401 RVA: 0x001B514D File Offset: 0x001B334D
		private IEnumerator CaptureScreenshot(string path)
		{
			this.capturingScreenshot = true;
			yield return new WaitForEndOfFrame();
			Texture2D texture2D = null;
			try
			{
				if (!this.IsOpen || this.view == null)
				{
					yield break;
				}
				texture2D = ScreenCapture.CaptureScreenshotAsTexture();
				if (texture2D == null)
				{
					throw new InvalidOperationException("Immagine non disponibile.");
				}
				File.WriteAllBytes(path, texture2D.EncodeToPNG());
				yield break;
			}
			catch (Exception ex)
			{
				this.status = this.status + "\nValori salvati, immagine non disponibile: " + ex.Message;
				Debug.LogWarning("[FP Calibration] Screenshot: " + ex.Message, this);
				yield break;
			}
			finally
			{
				if (texture2D != null)
				{
					Object.Destroy(texture2D);
				}
				this.capturingScreenshot = false;
			}
			yield break;
		}

		// Token: 0x060047E2 RID: 18402 RVA: 0x001B5164 File Offset: 0x001B3364
		private void DrawWindow(int id)
		{
			this.scroll = GUILayout.BeginScrollView(this.scroll, Array.Empty<GUILayoutOption>());
			GUILayout.Label("Posa ferma - anteprima temporanea", this.titleLabel, Array.Empty<GUILayoutOption>());
			if (this.openedContext != null)
			{
				GUILayout.Label(this.openedContext.carriedItemCamera ? ("Oggetto impugnato - preset unico\n" + this.openedContext.carriedItemName) : string.Concat(new string[]
				{
					this.openedContext.vehicleType,
					"  |  ",
					this.openedContext.stance,
					"\n",
					this.openedContext.poseState
				}), this.wrappedLabel, Array.Empty<GUILayoutOption>());
			}
			GUILayout.Label("Posizione rispetto al personaggio (metri)", this.titleLabel, Array.Empty<GUILayoutOption>());
			Vector3 positionOffset = this.adjustment.positionOffset;
			positionOffset.x = FirstPersonCameraCalibration.Slider("Destra / sinistra", positionOffset.x, -1f, 1f, 0.005f);
			positionOffset.y = FirstPersonCameraCalibration.Slider("Alto / basso", positionOffset.y, -1f, 1f, 0.005f);
			positionOffset.z = FirstPersonCameraCalibration.Slider("Avanti / indietro", positionOffset.z, -1f, 1f, 0.005f);
			this.adjustment.positionOffset = positionOffset;
			GUILayout.Label("Rotazione aggiuntiva (gradi)", this.titleLabel, Array.Empty<GUILayoutOption>());
			Vector3 rotationOffset = this.adjustment.rotationOffset;
			rotationOffset.x = FirstPersonCameraCalibration.Slider("Inclinazione", rotationOffset.x, -90f, 90f, 0.5f);
			rotationOffset.y = FirstPersonCameraCalibration.Slider("Direzione", rotationOffset.y, -180f, 180f, 0.5f);
			rotationOffset.z = FirstPersonCameraCalibration.Slider("Rollio", rotationOffset.z, -45f, 45f, 0.5f);
			this.adjustment.rotationOffset = rotationOffset;
			this.adjustment.fieldOfView = FirstPersonCameraCalibration.Slider("Campo visivo (FOV)", this.adjustment.fieldOfView, 20f, 120f, 1f);
			this.adjustment.nearClipPlane = FirstPersonCameraCalibration.Slider("Taglio vicino", this.adjustment.nearClipPlane, 0.001f, 0.3f, 0.001f);
			GUILayout.Space(6f);
			if (GUILayout.Button("SALVA CAMPIONE", new GUILayoutOption[] { GUILayout.Height(32f) }))
			{
				this.RequestSave();
			}
			if (GUILayout.Button("Prova in movimento / chiudi (F4)", new GUILayoutOption[] { GUILayout.Height(28f) }))
			{
				this.ResumePreview();
			}
			if (GUILayout.Button("Azzera le regolazioni", Array.Empty<GUILayoutOption>()))
			{
				this.adjustment = new FirstPersonCalibrationValues
				{
					fieldOfView = this.originalFov,
					nearClipPlane = this.originalNear
				};
			}
			if (GUILayout.Button("Termina prova e ripristina camera", Array.Empty<GUILayoutOption>()))
			{
				this.EndSession();
			}
			GUILayout.Label(this.status, this.wrappedLabel, Array.Empty<GUILayoutOption>());
			GUILayout.Label("I campioni restano salvati dopo lo Stop. Scrivimi l'azione in chat: li usero' per impostare i valori definitivi.", this.wrappedLabel, Array.Empty<GUILayoutOption>());
			GUILayout.EndScrollView();
			GUI.DragWindow(new Rect(0f, 0f, this.window.width, 21f));
		}

		// Token: 0x060047E3 RID: 18403 RVA: 0x001B54AC File Offset: 0x001B36AC
		private static float Slider(string label, float value, float min, float max, float step)
		{
			GUILayout.Label(label + ": " + value.ToString("0.000", CultureInfo.InvariantCulture), Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("-", new GUILayoutOption[] { GUILayout.Width(26f) }))
			{
				value -= step;
			}
			value = GUILayout.HorizontalSlider(value, min, max, Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("+", new GUILayoutOption[] { GUILayout.Width(26f) }))
			{
				value += step;
			}
			GUILayout.EndHorizontal();
			return Mathf.Clamp(value, min, max);
		}

		// Token: 0x0400479F RID: 18335
		public FirstPersonCalibrationValues adjustment = new FirstPersonCalibrationValues();

		// Token: 0x040047A3 RID: 18339
		private FirstPersonController controller;

		// Token: 0x040047A4 RID: 18340
		private vThirdPersonInput playerInput;

		// Token: 0x040047A5 RID: 18341
		private vThirdPersonCamera rig;

		// Token: 0x040047A6 RID: 18342
		private Camera view;

		// Token: 0x040047A7 RID: 18343
		private float originalFov;

		// Token: 0x040047A8 RID: 18344
		private float originalNear;

		// Token: 0x040047A9 RID: 18345
		private float previousTimeScale;

		// Token: 0x040047AA RID: 18346
		private bool previousAudioPause;

		// Token: 0x040047AB RID: 18347
		private bool previousInputLock;

		// Token: 0x040047AC RID: 18348
		private bool previousExternalLock;

		// Token: 0x040047AD RID: 18349
		private CursorLockMode previousCursorLock;

		// Token: 0x040047AE RID: 18350
		private bool previousCursorVisible;

		// Token: 0x040047AF RID: 18351
		private PlayerActivityLease lease;

		// Token: 0x040047B0 RID: 18352
		private bool toggleRequested;

		// Token: 0x040047B1 RID: 18353
		private bool saveRequested;

		// Token: 0x040047B2 RID: 18354
		private string status = "";

		// Token: 0x040047B3 RID: 18355
		private FirstPersonCalibrationContext openedContext;

		// Token: 0x040047B4 RID: 18356
		private Transform sessionParent;

		// Token: 0x040047B5 RID: 18357
		private Transform sessionAnchor;

		// Token: 0x040047B6 RID: 18358
		private bool sessionHidden;

		// Token: 0x040047B7 RID: 18359
		private bool sessionMovementLocked;

		// Token: 0x040047B8 RID: 18360
		private bool sessionRoofRide;

		// Token: 0x040047B9 RID: 18361
		private InventoryItemSO sessionCarriedItem;

		// Token: 0x040047BA RID: 18362
		private Vector2 scroll;

		// Token: 0x040047BB RID: 18363
		private Rect window = new Rect(16f, 42f, 370f, 620f);

		// Token: 0x040047BC RID: 18364
		private GUIStyle wrappedLabel;

		// Token: 0x040047BD RID: 18365
		private GUIStyle titleLabel;

		// Token: 0x040047BE RID: 18366
		private bool capturingScreenshot;

		// Token: 0x040047BF RID: 18367
		private Vector3 basePosition;

		// Token: 0x040047C0 RID: 18368
		private Quaternion baseRotation;

		// Token: 0x040047C1 RID: 18369
		private bool hasBasePose;

		// Token: 0x040047C2 RID: 18370
		private Vector3 appliedPosition;

		// Token: 0x040047C3 RID: 18371
		private Quaternion appliedRotation;

		// Token: 0x020009EB RID: 2539
		[Serializable]
		private sealed class ControllerSettings
		{
			// Token: 0x060047E5 RID: 18405 RVA: 0x001B558C File Offset: 0x001B378C
			public ControllerSettings(FirstPersonController fp)
			{
				this.confidentPreset = fp.confidentPreset;
				this.embarrassedPreset = fp.embarrassedPreset;
				this.carriedItemPreset = fp.carriedItemPreset;
				this.coverEyeHeightOffset = fp.coverEyeHeightOffset;
				this.coverEyeForwardOffset = fp.coverEyeForwardOffset;
				this.verticalDamping = fp.verticalDamping;
				this.presetBlendSpeed = fp.presetBlendSpeed;
				this.fpNearClipPlane = fp.fpNearClipPlane;
				this.vehicleYawLimit = fp.vehicleYawLimit;
				this.vehiclePitchMin = fp.vehiclePitchMin;
				this.vehiclePitchMax = fp.vehiclePitchMax;
				this.yMinLimit = fp.yMinLimit;
				this.yMaxLimit = fp.yMaxLimit;
				this.hideSpotForwardOffset = fp.hideSpotForwardOffset;
				this.hideSpotVerticalOffset = fp.hideSpotVerticalOffset;
				this.hideSpotPitchMin = fp.hideSpotPitchMin;
				this.hideSpotPitchMax = fp.hideSpotPitchMax;
				this.hideSpotYawHalfRange = fp.hideSpotYawHalfRange;
				this.hideSpotEntryPitch = fp.hideSpotEntryPitch;
				this.isHideSpot = fp.YawClampOverrideCenter != null;
				this.hasExternalAnchor = fp.CameraAnchorOverride != null;
				this.externalAnchorName = (this.hasExternalAnchor ? fp.CameraAnchorOverride.name : "");
			}

			// Token: 0x040047C4 RID: 18372
			public FPCameraPreset confidentPreset;

			// Token: 0x040047C5 RID: 18373
			public FPCameraPreset embarrassedPreset;

			// Token: 0x040047C6 RID: 18374
			public FPCameraPreset carriedItemPreset;

			// Token: 0x040047C7 RID: 18375
			public float coverEyeHeightOffset;

			// Token: 0x040047C8 RID: 18376
			public float coverEyeForwardOffset;

			// Token: 0x040047C9 RID: 18377
			public float verticalDamping;

			// Token: 0x040047CA RID: 18378
			public float presetBlendSpeed;

			// Token: 0x040047CB RID: 18379
			public float fpNearClipPlane;

			// Token: 0x040047CC RID: 18380
			public float vehicleYawLimit;

			// Token: 0x040047CD RID: 18381
			public float vehiclePitchMin;

			// Token: 0x040047CE RID: 18382
			public float vehiclePitchMax;

			// Token: 0x040047CF RID: 18383
			public float yMinLimit;

			// Token: 0x040047D0 RID: 18384
			public float yMaxLimit;

			// Token: 0x040047D1 RID: 18385
			public float hideSpotForwardOffset;

			// Token: 0x040047D2 RID: 18386
			public float hideSpotVerticalOffset;

			// Token: 0x040047D3 RID: 18387
			public float hideSpotPitchMin;

			// Token: 0x040047D4 RID: 18388
			public float hideSpotPitchMax;

			// Token: 0x040047D5 RID: 18389
			public float hideSpotYawHalfRange;

			// Token: 0x040047D6 RID: 18390
			public float hideSpotEntryPitch;

			// Token: 0x040047D7 RID: 18391
			public bool isHideSpot;

			// Token: 0x040047D8 RID: 18392
			public bool hasExternalAnchor;

			// Token: 0x040047D9 RID: 18393
			public string externalAnchorName;
		}
	}
}
