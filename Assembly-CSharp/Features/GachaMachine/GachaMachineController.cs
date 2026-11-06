using System;
using System.Collections;
using System.Collections.Generic;
using Features.DebugTools;
using Features.Detection;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.FirstPerson;
using ProjectExhibitionist.Interactions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Features.GachaMachine
{
	// Token: 0x020011E5 RID: 4581
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(-40)]
	public sealed class GachaMachineController : MonoBehaviour, IInteractable, IInteractionCandidate
	{
		// Token: 0x06008D3C RID: 36156 RVA: 0x0039A964 File Offset: 0x00398B64
		private bool SetFeatureCamera(bool enabled)
		{
			if (!enabled)
			{
				this.SuspendInsideFirstPersonView();
				this.DisableAllFeatureCameras();
				this.ResetFeatureCameraNavigation();
				if (this._disabledMainCamera != null)
				{
					this._disabledMainCamera.enabled = this._disabledMainCameraWasEnabled;
				}
				this._disabledMainCamera = null;
				return true;
			}
			if (this._activeFeatureCamera != null)
			{
				return true;
			}
			Camera camera = Camera.main;
			if (camera == null && this._thirdPersonInput != null)
			{
				camera = this._thirdPersonInput.cameraMain;
			}
			bool flag = camera != null && camera.enabled;
			this.ResetFeatureCameraNavigation();
			this._activeCameraIndex = 0;
			if (!this.ActivateFeatureCamera(this._activeCameraIndex))
			{
				Debug.LogWarning("[GachaMachine] No valid feature-camera preset; keeping the normal player camera enabled.", this);
				return false;
			}
			this._disabledMainCamera = camera;
			if (this._disabledMainCamera == this._activeFeatureCamera)
			{
				this._disabledMainCamera = null;
			}
			if (this._disabledMainCamera != null)
			{
				this._disabledMainCameraWasEnabled = flag;
				this._disabledMainCamera.enabled = false;
			}
			return true;
		}

		// Token: 0x06008D3D RID: 36157 RVA: 0x0039AA68 File Offset: 0x00398C68
		private bool ActivateFeatureCamera(int requestedIndex)
		{
			if (this.featureCameras == null || this.featureCameras.Length == 0)
			{
				return false;
			}
			if (this._firstPersonViewBound && this._firstPersonController != null && this._firstPersonController.IsFirstPerson)
			{
				this._firstPersonController.ExitFirstPerson();
			}
			this.DisableAllFeatureCameras();
			for (int i = 0; i < this.featureCameras.Length; i++)
			{
				int num = (requestedIndex + i + this.featureCameras.Length) % this.featureCameras.Length;
				Camera camera = this.featureCameras[num];
				if (!(camera == null))
				{
					this._activeCameraIndex = num;
					this._activeFeatureCamera = camera;
					camera.gameObject.SetActive(true);
					camera.enabled = true;
					if (this._disabledMainCamera != null)
					{
						this._disabledMainCamera.enabled = false;
					}
					this.ApplyFeatureCameraNavigationImmediately(num);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008D3E RID: 36158 RVA: 0x0039AB3C File Offset: 0x00398D3C
		private void CycleFeatureCamera()
		{
			if (this.featureCameras == null || this.featureCameras.Length == 0)
			{
				return;
			}
			this.ActivateFeatureCamera((this._activeCameraIndex + 1) % this.featureCameras.Length);
		}

		// Token: 0x06008D3F RID: 36159 RVA: 0x0039AB68 File Offset: 0x00398D68
		private void DisableAllFeatureCameras()
		{
			if (this.featureCameras != null)
			{
				foreach (Camera camera in this.featureCameras)
				{
					if (!(camera == null))
					{
						camera.enabled = false;
						camera.gameObject.SetActive(false);
					}
				}
			}
			this._activeFeatureCamera = null;
		}

		// Token: 0x06008D40 RID: 36160 RVA: 0x0039ABBC File Offset: 0x00398DBC
		private void CaptureFeatureCameraNavigationBaselines()
		{
			int num = ((this.featureCameras != null) ? this.featureCameras.Length : 0);
			this._featureCameraNavigation = new GachaMachineController.FeatureCameraNavigationState[num];
			for (int i = 0; i < num; i++)
			{
				Camera camera = this.featureCameras[i];
				if (!(camera == null))
				{
					this._featureCameraNavigation[i] = new GachaMachineController.FeatureCameraNavigationState
					{
						camera = camera,
						authoredLocalPosition = camera.transform.localPosition,
						authoredLocalRotation = camera.transform.localRotation,
						authoredFieldOfView = camera.fieldOfView,
						targetFieldOfView = camera.fieldOfView
					};
				}
			}
		}

		// Token: 0x06008D41 RID: 36161 RVA: 0x0039AC58 File Offset: 0x00398E58
		private void EnsureFeatureCameraNavigationBaselines()
		{
			int num = ((this.featureCameras != null) ? this.featureCameras.Length : 0);
			if (this._featureCameraNavigation == null || this._featureCameraNavigation.Length != num)
			{
				this.CaptureFeatureCameraNavigationBaselines();
				return;
			}
			for (int i = 0; i < num; i++)
			{
				GachaMachineController.FeatureCameraNavigationState featureCameraNavigationState = this._featureCameraNavigation[i];
				if (((featureCameraNavigationState != null) ? featureCameraNavigationState.camera : null) != this.featureCameras[i])
				{
					this.CaptureFeatureCameraNavigationBaselines();
					return;
				}
			}
		}

		// Token: 0x06008D42 RID: 36162 RVA: 0x0039ACCC File Offset: 0x00398ECC
		private void ResetFeatureCameraNavigation()
		{
			this.EnsureFeatureCameraNavigationBaselines();
			if (this._featureCameraNavigation == null)
			{
				return;
			}
			foreach (GachaMachineController.FeatureCameraNavigationState featureCameraNavigationState in this._featureCameraNavigation)
			{
				if (featureCameraNavigationState != null && !(featureCameraNavigationState.camera == null))
				{
					featureCameraNavigationState.targetFieldOfView = featureCameraNavigationState.authoredFieldOfView;
					featureCameraNavigationState.camera.fieldOfView = featureCameraNavigationState.authoredFieldOfView;
					featureCameraNavigationState.camera.transform.localPosition = featureCameraNavigationState.authoredLocalPosition;
					featureCameraNavigationState.camera.transform.localRotation = featureCameraNavigationState.authoredLocalRotation;
				}
			}
		}

		// Token: 0x06008D43 RID: 36163 RVA: 0x0039AD5C File Offset: 0x00398F5C
		private void UpdateFeatureCameraNavigationInput()
		{
			if (ControllerInput.IsActive && ControllerWorldPointer.BlocksContextInput)
			{
				return;
			}
			if (!this.enableCameraZoom || this._activeFeatureCamera == null)
			{
				return;
			}
			GachaMachineController.FeatureCameraNavigationState featureCameraNavigationState = this.ActiveFeatureCameraNavigationState();
			if (featureCameraNavigationState == null)
			{
				return;
			}
			float num = Mathf.Min(this.minimumCameraFieldOfView, featureCameraNavigationState.authoredFieldOfView);
			float num2 = (ControllerInput.IsActive ? (((ControllerWorldPointer.Held(ControllerButton.RightShoulder) ? 1f : 0f) - (ControllerWorldPointer.Held(ControllerButton.LeftShoulder) ? 1f : 0f)) * Time.unscaledDeltaTime * 3f) : GameplayInput.mouseScrollDelta.y);
			if (Mathf.Abs(num2) > 0.001f)
			{
				featureCameraNavigationState.targetFieldOfView = Mathf.Clamp(featureCameraNavigationState.targetFieldOfView - num2 * this.cameraZoomDegreesPerWheelStep, num, featureCameraNavigationState.authoredFieldOfView);
			}
		}

		// Token: 0x06008D44 RID: 36164 RVA: 0x0039AE24 File Offset: 0x00399024
		private void ApplyActiveFeatureCameraNavigation()
		{
			if (!this.enableCameraZoom || this._activeFeatureCamera == null)
			{
				return;
			}
			GachaMachineController.FeatureCameraNavigationState featureCameraNavigationState = this.ActiveFeatureCameraNavigationState();
			if (featureCameraNavigationState == null || featureCameraNavigationState.camera == null)
			{
				return;
			}
			float unscaledDeltaTime = Time.unscaledDeltaTime;
			float num = 1f - Mathf.Exp(-this.cameraZoomSmoothingResponse * unscaledDeltaTime);
			featureCameraNavigationState.camera.fieldOfView = Mathf.Lerp(featureCameraNavigationState.camera.fieldOfView, featureCameraNavigationState.targetFieldOfView, num);
			featureCameraNavigationState.camera.transform.localPosition = featureCameraNavigationState.authoredLocalPosition;
			featureCameraNavigationState.camera.transform.localRotation = featureCameraNavigationState.authoredLocalRotation;
		}

		// Token: 0x06008D45 RID: 36165 RVA: 0x0039AECC File Offset: 0x003990CC
		private void ApplyFeatureCameraNavigationImmediately(int index)
		{
			this.EnsureFeatureCameraNavigationBaselines();
			if (this._featureCameraNavigation == null || index < 0 || index >= this._featureCameraNavigation.Length)
			{
				return;
			}
			GachaMachineController.FeatureCameraNavigationState featureCameraNavigationState = this._featureCameraNavigation[index];
			if (featureCameraNavigationState == null || featureCameraNavigationState.camera == null)
			{
				return;
			}
			featureCameraNavigationState.camera.transform.localPosition = featureCameraNavigationState.authoredLocalPosition;
			featureCameraNavigationState.camera.transform.localRotation = featureCameraNavigationState.authoredLocalRotation;
		}

		// Token: 0x06008D46 RID: 36166 RVA: 0x0039AF40 File Offset: 0x00399140
		private GachaMachineController.FeatureCameraNavigationState ActiveFeatureCameraNavigationState()
		{
			this.EnsureFeatureCameraNavigationBaselines();
			if (this._featureCameraNavigation == null || this._activeCameraIndex < 0 || this._activeCameraIndex >= this._featureCameraNavigation.Length)
			{
				return null;
			}
			GachaMachineController.FeatureCameraNavigationState featureCameraNavigationState = this._featureCameraNavigation[this._activeCameraIndex];
			if (featureCameraNavigationState == null || !(featureCameraNavigationState.camera == this._activeFeatureCamera))
			{
				return null;
			}
			return featureCameraNavigationState;
		}

		// Token: 0x06008D47 RID: 36167 RVA: 0x0039AFA0 File Offset: 0x003991A0
		private void BeginInsideFirstPersonView()
		{
			if (this._firstPersonController == null || this._disabledMainCamera == null)
			{
				return;
			}
			if (!this._firstPersonViewBound)
			{
				if (this._firstPersonController.OnEnterFirstPerson == null)
				{
					this._firstPersonController.OnEnterFirstPerson = new UnityEvent();
				}
				if (this._firstPersonController.OnExitFirstPerson == null)
				{
					this._firstPersonController.OnExitFirstPerson = new UnityEvent();
				}
				this._firstPersonController.OnEnterFirstPerson.AddListener(new UnityAction(this.ShowInsideFirstPersonCamera));
				this._firstPersonController.OnExitFirstPerson.AddListener(new UnityAction(this.RestoreInsidePresetCamera));
				this._firstPersonViewBound = true;
			}
			this._firstPersonController.ResumeAfterInteraction(this);
		}

		// Token: 0x06008D48 RID: 36168 RVA: 0x0039B057 File Offset: 0x00399257
		private void ShowInsideFirstPersonCamera()
		{
			if (!this._firstPersonViewBound || this._state != GachaMachineController.SessionState.Inside)
			{
				return;
			}
			if (this._disabledMainCamera == null)
			{
				this._firstPersonController.ExitFirstPerson();
				return;
			}
			this.DisableAllFeatureCameras();
			this._disabledMainCamera.enabled = true;
		}

		// Token: 0x06008D49 RID: 36169 RVA: 0x0039B097 File Offset: 0x00399297
		private void RestoreInsidePresetCamera()
		{
			if (this._firstPersonViewBound && this._state == GachaMachineController.SessionState.Inside)
			{
				this.ActivateFeatureCamera(this._activeCameraIndex);
			}
		}

		// Token: 0x06008D4A RID: 36170 RVA: 0x0039B0B8 File Offset: 0x003992B8
		private void SuspendInsideFirstPersonView()
		{
			if (!this._firstPersonViewBound)
			{
				return;
			}
			this._firstPersonViewBound = false;
			if (this._firstPersonController == null)
			{
				return;
			}
			UnityEvent onEnterFirstPerson = this._firstPersonController.OnEnterFirstPerson;
			if (onEnterFirstPerson != null)
			{
				onEnterFirstPerson.RemoveListener(new UnityAction(this.ShowInsideFirstPersonCamera));
			}
			UnityEvent onExitFirstPerson = this._firstPersonController.OnExitFirstPerson;
			if (onExitFirstPerson != null)
			{
				onExitFirstPerson.RemoveListener(new UnityAction(this.RestoreInsidePresetCamera));
			}
			this._firstPersonController.ExitFirstPerson();
			this._firstPersonController.EndInteractionView(this);
			this._firstPersonController.SuspendForInteraction(this);
			if (this._state == GachaMachineController.SessionState.Inside)
			{
				this.ActivateFeatureCamera(this._activeCameraIndex);
			}
		}

		// Token: 0x17001696 RID: 5782
		// (get) Token: 0x06008D4B RID: 36171 RVA: 0x0039B161 File Offset: 0x00399361
		public InteractionConfigSO Config
		{
			get
			{
				return this.enterConfig;
			}
		}

		// Token: 0x17001697 RID: 5783
		// (get) Token: 0x06008D4C RID: 36172 RVA: 0x0039B169 File Offset: 0x00399369
		public bool IsBeingUsed
		{
			get
			{
				return this._state > GachaMachineController.SessionState.Outside;
			}
		}

		// Token: 0x17001698 RID: 5784
		// (get) Token: 0x06008D4D RID: 36173 RVA: 0x0039B174 File Offset: 0x00399374
		public GachaMachineController.SessionState State
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x17001699 RID: 5785
		// (get) Token: 0x06008D4E RID: 36174 RVA: 0x0039B17C File Offset: 0x0039937C
		public GachaMachineSide ActiveSide
		{
			get
			{
				if (this.ActiveEntry == null)
				{
					return GachaMachineSide.Left;
				}
				return this.ActiveEntry.side;
			}
		}

		// Token: 0x1700169A RID: 5786
		// (get) Token: 0x06008D4F RID: 36175 RVA: 0x0039B193 File Offset: 0x00399393
		public Transform RuntimePlayer
		{
			get
			{
				return this._player;
			}
		}

		// Token: 0x1700169B RID: 5787
		// (get) Token: 0x06008D50 RID: 36176 RVA: 0x0039B19B File Offset: 0x0039939B
		public Animator RuntimePlayerAnimator
		{
			get
			{
				return this._playerAnimator;
			}
		}

		// Token: 0x1700169C RID: 5788
		// (get) Token: 0x06008D51 RID: 36177 RVA: 0x0039B1A3 File Offset: 0x003993A3
		public GachaClawRig ActiveClaw
		{
			get
			{
				return this._activeClaw;
			}
		}

		// Token: 0x1700169D RID: 5789
		// (get) Token: 0x06008D52 RID: 36178 RVA: 0x0039B1AB File Offset: 0x003993AB
		public Transform ActiveInsidePoseTransform
		{
			get
			{
				if (this.ActiveEntry == null)
				{
					return null;
				}
				return this.ActiveEntry.insidePose;
			}
		}

		// Token: 0x1700169E RID: 5790
		// (get) Token: 0x06008D53 RID: 36179 RVA: 0x0039B1C2 File Offset: 0x003993C2
		public IReadOnlyList<Camera> FeatureCameras
		{
			get
			{
				return this.featureCameras;
			}
		}

		// Token: 0x1700169F RID: 5791
		// (get) Token: 0x06008D54 RID: 36180 RVA: 0x0039B1CA File Offset: 0x003993CA
		public float CandidateSqrDistance
		{
			get
			{
				return this._candidateSqrDistance;
			}
		}

		// Token: 0x170016A0 RID: 5792
		// (get) Token: 0x06008D55 RID: 36181 RVA: 0x0039B1D2 File Offset: 0x003993D2
		public bool IsCandidateEligible
		{
			get
			{
				return this._state == GachaMachineController.SessionState.Outside && this._player != null && this._candidateEntryIndex >= 0 && !this.IsFadeRunning;
			}
		}

		// Token: 0x170016A1 RID: 5793
		// (get) Token: 0x06008D56 RID: 36182 RVA: 0x000FCE88 File Offset: 0x000FB088
		public int CandidateInstanceId
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		// Token: 0x170016A2 RID: 5794
		// (get) Token: 0x06008D57 RID: 36183 RVA: 0x0039B1FE File Offset: 0x003993FE
		private GachaMachineController.EntryPoint ActiveEntry
		{
			get
			{
				if (this.entries == null || this._activeEntryIndex < 0 || this._activeEntryIndex >= this.entries.Length)
				{
					return null;
				}
				return this.entries[this._activeEntryIndex];
			}
		}

		// Token: 0x170016A3 RID: 5795
		// (get) Token: 0x06008D58 RID: 36184 RVA: 0x0039B230 File Offset: 0x00399430
		private bool IsFadeRunning
		{
			get
			{
				return ScreenFadeController.Instance != null && ScreenFadeController.Instance.IsFading;
			}
		}

		// Token: 0x170016A4 RID: 5796
		// (get) Token: 0x06008D59 RID: 36185 RVA: 0x0039B24B File Offset: 0x0039944B
		private float FadeWatchdogDuration
		{
			get
			{
				return Mathf.Max(1.5f, this.fadeOutDuration + this.fadeHoldDuration + this.fadeInDuration + 1.5f);
			}
		}

		// Token: 0x06008D5A RID: 36186 RVA: 0x0039B274 File Offset: 0x00399474
		private void Awake()
		{
			ControllerContextBindings.SetLabel(this.insideHudConfig, "WASD", "LS", false);
			ControllerContextBindings.SetLabel(this.insideHudConfig, "Space / Shift", "LT / RT", false);
			ControllerContextBindings.SetLabel(this.insideHudConfig, "F", "A", false);
			ControllerContextBindings.SetLabel(this.insideHudConfig, "1 / 2 / 3 / C", "D-Pad / Y", false);
			ControllerContextBindings.SetLabel(this.insideHudConfig, "WHEEL", "LB / RB", false);
			ControllerContextBindings.SetLabel(this.insideHudConfig, "V", "R3", false);
			ControllerContextBindings.SetLabel(this.insideHudConfig, "E", "B", false);
			ControllerContextBindings.Set(this.exitInput, "B");
			ControllerContextBindings.Set(this.cameraOneInput, "DPadLeft");
			ControllerContextBindings.Set(this.cameraTwoInput, "DPadUp");
			ControllerContextBindings.Set(this.cameraThreeInput, "DPadRight");
			ControllerContextBindings.Set(this.cycleCameraInput, "Y");
			if (this.promptUI == null)
			{
				this.promptUI = base.GetComponentInChildren<InteractablePromptUI>(true);
			}
			if (this.featureCameras == null || this.featureCameras.Length == 0)
			{
				List<Camera> list = new List<Camera>();
				foreach (Camera camera in base.GetComponentsInChildren<Camera>(true))
				{
					if (camera != null && camera.name.StartsWith("GachaCamera", StringComparison.Ordinal))
					{
						list.Add(camera);
					}
				}
				list.Sort((Camera a, Camera b) => string.CompareOrdinal(a.name, b.name));
				this.featureCameras = list.ToArray();
			}
			if (this.clawRigs == null || this.clawRigs.Length == 0)
			{
				this.clawRigs = base.GetComponentsInChildren<GachaClawRig>(true);
			}
			if (this.plushField == null)
			{
				this.plushField = base.GetComponentInChildren<GachaPlushField>(true);
			}
			this.CaptureFeatureCameraNavigationBaselines();
			this.ResetFeatureCameraNavigation();
			this.DisableAllFeatureCameras();
			if (this.promptUI != null)
			{
				this.promptUI.Hide();
			}
		}

		// Token: 0x06008D5B RID: 36187 RVA: 0x0039B470 File Offset: 0x00399670
		private void OnEnable()
		{
			this._state = GachaMachineController.SessionState.Outside;
			this._candidateSqrDistance = float.MaxValue;
			this._candidateEntryIndex = -1;
			this._nextPlayerResolveTime = 0f;
			this._clawGrabOffsetWorld = Vector3.zero;
			this._pinExitFacing = false;
			InteractionCandidateRegistry.Register(this);
			this.ResolvePlayerReferences();
		}

		// Token: 0x06008D5C RID: 36188 RVA: 0x0039B4C0 File Offset: 0x003996C0
		private void OnDisable()
		{
			InteractionCandidateRegistry.Unregister(this);
			this.HidePrompt();
			if (this._state != GachaMachineController.SessionState.Outside || this._playerStateCaptured || this._activityLease != null)
			{
				this.ForceAbort();
				return;
			}
			this.HideInsideHud();
			this.DisableAllClawControl();
			if (this.plushField != null)
			{
				this.plushField.EndSession();
			}
			this.SetFeatureCamera(false);
		}

		// Token: 0x06008D5D RID: 36189 RVA: 0x0039B528 File Offset: 0x00399728
		private void Update()
		{
			if (this._state == GachaMachineController.SessionState.Inside)
			{
				this.UpdateInsideInput();
				return;
			}
			if (this._state != GachaMachineController.SessionState.Outside)
			{
				this.HidePrompt();
				return;
			}
			if (!this.EnsurePlayerTransform())
			{
				this._candidateEntryIndex = -1;
				this._candidateSqrDistance = float.MaxValue;
				this.HidePrompt();
				return;
			}
			this.RefreshClosestValidEntry();
			if (!this.IsCandidateEligible || !InteractionCandidateRegistry.IsWinner(this))
			{
				this.HidePrompt();
				return;
			}
			this.ShowPrompt();
			if (this.interactInput != null && this.interactInput.GetButtonDown())
			{
				this.TryBeginEntry(this._candidateEntryIndex);
			}
		}

		// Token: 0x06008D5E RID: 36190 RVA: 0x0039B5C0 File Offset: 0x003997C0
		private void LateUpdate()
		{
			if (this._layersForced && this._playerAnimator != null)
			{
				this.ApplyForcedLayerWeights();
			}
			if (this._transferredInside)
			{
				this.PinPlayerToInsidePose();
			}
			if (this._state == GachaMachineController.SessionState.Exiting && this._pinExitFacing)
			{
				this.PinPlayerExitFacing();
			}
			if (this._state == GachaMachineController.SessionState.Inside)
			{
				this.ApplyActiveFeatureCameraNavigation();
			}
		}

		// Token: 0x06008D5F RID: 36191 RVA: 0x0039B61D File Offset: 0x0039981D
		public bool CanInteract(GameObject player)
		{
			if (player != null && this._player == null)
			{
				this._player = player.transform;
				this.ResolvePlayerReferences();
			}
			this.RefreshClosestValidEntry();
			return this.IsCandidateEligible;
		}

		// Token: 0x06008D60 RID: 36192 RVA: 0x0039B654 File Offset: 0x00399854
		public void OnInteract(GameObject player)
		{
			if (player != null && this._player == null)
			{
				this._player = player.transform;
				this.ResolvePlayerReferences();
			}
			this.RefreshClosestValidEntry();
			if (this._candidateEntryIndex >= 0)
			{
				this.TryBeginEntry(this._candidateEntryIndex);
			}
		}

		// Token: 0x06008D61 RID: 36193 RVA: 0x0039B6A8 File Offset: 0x003998A8
		public void OnActionExecuted(string actionName, GameObject player)
		{
			if (this._state == GachaMachineController.SessionState.Inside && !string.IsNullOrEmpty(actionName) && (actionName.IndexOf("esci", StringComparison.OrdinalIgnoreCase) >= 0 || actionName.IndexOf("exit", StringComparison.OrdinalIgnoreCase) >= 0 || actionName.IndexOf("get out", StringComparison.OrdinalIgnoreCase) >= 0))
			{
				this.BeginExit();
				return;
			}
			this.OnInteract(player);
		}

		// Token: 0x06008D62 RID: 36194 RVA: 0x0039B704 File Offset: 0x00399904
		private void RefreshClosestValidEntry()
		{
			this._candidateEntryIndex = -1;
			this._candidateSqrDistance = float.MaxValue;
			if (this._player == null || this.entries == null)
			{
				return;
			}
			Vector3 vector = Vector3.ProjectOnPlane(this._player.forward, Vector3.up);
			if (vector.sqrMagnitude > 0.0001f)
			{
				vector.Normalize();
			}
			float num = Mathf.Cos(this.maximumFacingAngle * 0.017453292f);
			float num2 = this.interactionRadius * this.interactionRadius;
			for (int i = 0; i < this.entries.Length; i++)
			{
				GachaMachineController.EntryPoint entryPoint = this.entries[i];
				if (entryPoint != null && !(entryPoint.enterStart == null) && !(entryPoint.insidePose == null))
				{
					Vector3 vector2 = this._player.position - entryPoint.enterStart.position;
					if (Mathf.Abs(vector2.y) <= this.maximumVerticalDifference)
					{
						vector2.y = 0f;
						float sqrMagnitude = vector2.sqrMagnitude;
						if (sqrMagnitude <= num2)
						{
							Vector3 vector3 = Vector3.ProjectOnPlane(entryPoint.enterStart.forward, Vector3.up);
							if (this.requirePlayerFacingHatch && vector3.sqrMagnitude > 0.0001f)
							{
								vector3.Normalize();
								if (Vector3.Dot(vector, vector3) < num)
								{
									goto IL_0155;
								}
							}
							if (sqrMagnitude < this._candidateSqrDistance)
							{
								this._candidateSqrDistance = sqrMagnitude;
								this._candidateEntryIndex = i;
							}
						}
					}
				}
				IL_0155:;
			}
			if (this._candidateEntryIndex >= 0 && this.entries[this._candidateEntryIndex].side != GachaMachineSide.Right)
			{
				this._candidateEntryIndex = -1;
				this._candidateSqrDistance = float.MaxValue;
			}
		}

		// Token: 0x06008D63 RID: 36195 RVA: 0x0039B8A8 File Offset: 0x00399AA8
		private bool TryBeginEntry(int entryIndex)
		{
			if (this._state != GachaMachineController.SessionState.Outside || this.entries == null || entryIndex < 0 || entryIndex >= this.entries.Length || this.IsFadeRunning)
			{
				return false;
			}
			GachaMachineController.EntryPoint entryPoint = this.entries[entryIndex];
			if (entryPoint == null || entryPoint.side != GachaMachineSide.Right || entryPoint.enterStart == null || entryPoint.insidePose == null)
			{
				return false;
			}
			if (!this.EnsurePlayerReferences() || !this.HasRequiredAnimatorStates())
			{
				return false;
			}
			if (!FullBodyHideActivityContext.TryAcquire("GachaMachine#" + base.GetInstanceID().ToString(), this, ref this._activityLease, PlayerActivity.Emote | PlayerActivity.Cover | PlayerActivity.Flash | PlayerActivity.RapidHUD | PlayerActivity.Interact | PlayerActivity.StanceSwitch | PlayerActivity.Undress | PlayerActivity.LingerieOpen | PlayerActivity.Inventory | PlayerActivity.Phone | PlayerActivity.Crouch | PlayerActivity.ArmsPose, true, false))
			{
				return false;
			}
			this._lifecycleGeneration++;
			this._activeEntryIndex = entryIndex;
			this._state = GachaMachineController.SessionState.Entering;
			this._clawGrabOffsetWorld = Vector3.zero;
			this._pinExitFacing = false;
			this.HidePrompt();
			this.HideInsideHud();
			this.CaptureAndLockPlayerState();
			if (!base.isActiveAndEnabled || this._state != GachaMachineController.SessionState.Entering)
			{
				return false;
			}
			this.SnapPlayer(entryPoint.enterStart.position, GachaMachineController.UprightRotation(entryPoint.enterStart));
			this._playerAnimator.CrossFadeInFixedTime("Ciel_Gacha_Enter", this.animationCrossFade, 0, 0f);
			if (this._sequence != null)
			{
				base.StopCoroutine(this._sequence);
			}
			this._sequence = base.StartCoroutine(this.EnterRoutine(this._lifecycleGeneration));
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[GachaMachine] Entering the {0} hatch.", entryPoint.side), this);
			}
			return true;
		}

		// Token: 0x06008D64 RID: 36196 RVA: 0x0039BA2D File Offset: 0x00399C2D
		private IEnumerator EnterRoutine(int generation)
		{
			yield return this.WaitUntilAnimatorState("Ciel_Gacha_Enter", 2f, generation);
			if (!this.IsCurrentAnimatorState("Ciel_Gacha_Enter"))
			{
				this.AbortFromSequence("Animator did not enter 'Ciel_Gacha_Enter'.");
				yield break;
			}
			float timeout = Mathf.Max(this.enterTransferTimeSeconds + 2f, 4f);
			float startedAt = Time.time;
			while (generation == this._lifecycleGeneration && this._state == GachaMachineController.SessionState.Entering && Time.time - startedAt < timeout)
			{
				AnimatorStateInfo currentAnimatorStateInfo = this._playerAnimator.GetCurrentAnimatorStateInfo(0);
				if (!currentAnimatorStateInfo.IsName("Ciel_Gacha_Enter"))
				{
					break;
				}
				float num = ((currentAnimatorStateInfo.length > 0.001f) ? currentAnimatorStateInfo.length : ((this.enterClip != null) ? this.enterClip.length : 8f));
				float num2 = Mathf.Clamp01(currentAnimatorStateInfo.normalizedTime) * num;
				float num3 = Mathf.Max(0f, this.enterTransferTimeSeconds - this.fadeOutDuration);
				if (num2 >= num3)
				{
					break;
				}
				yield return null;
			}
			if (generation != this._lifecycleGeneration || this._state != GachaMachineController.SessionState.Entering)
			{
				yield break;
			}
			bool transferComplete = false;
			bool transferSucceeded = false;
			this.FadeTransition(delegate
			{
				transferSucceeded = this.TransferPlayerInside(generation);
			}, delegate
			{
				transferComplete = true;
			}, generation);
			float fadeDeadline = Time.unscaledTime + this.FadeWatchdogDuration;
			while (!transferComplete && generation == this._lifecycleGeneration && Time.unscaledTime < fadeDeadline)
			{
				yield return null;
			}
			if (generation != this._lifecycleGeneration || this._state != GachaMachineController.SessionState.Entering)
			{
				yield break;
			}
			if (!transferComplete || !transferSucceeded)
			{
				this.AbortFromSequence((!transferComplete) ? "Enter fade was interrupted before completion." : "The black-frame transfer inside failed.");
				yield break;
			}
			this._state = GachaMachineController.SessionState.Inside;
			this.BeginInsideFirstPersonView();
			this.EnableClawForSide(this.ActiveSide);
			this.ShowInsideHud();
			this._sequence = null;
			if (this.debugLogs)
			{
				Debug.Log(string.Format("[GachaMachine] Ciel is hidden inside the {0} bay.", this.ActiveSide), this);
			}
			yield break;
		}

		// Token: 0x06008D65 RID: 36197 RVA: 0x0039BA44 File Offset: 0x00399C44
		private bool TransferPlayerInside(int generation)
		{
			if (generation != this._lifecycleGeneration || this._state != GachaMachineController.SessionState.Entering)
			{
				return false;
			}
			GachaMachineController.EntryPoint activeEntry = this.ActiveEntry;
			if (activeEntry == null || activeEntry.insidePose == null || this._player == null || this._playerAnimator == null)
			{
				return false;
			}
			this.SnapPlayer(activeEntry.insidePose.position, activeEntry.insidePose.rotation);
			this.AttachPlayerToMachine(true);
			this._transferredInside = true;
			string text = ((activeEntry.side == GachaMachineSide.Left) ? "Ciel_Gacha_Bored_L" : "Ciel_Gacha_Bored_R");
			this._playerAnimator.Play(text, 0, 0f);
			this.SetFeatureCamera(true);
			PlayerStealthBridge.SetHiddenSource(this, true);
			if (this.plushField != null && !this.plushField.BeginSession(activeEntry.side, this._player, this._playerAnimator))
			{
				Debug.LogWarning("[GachaMachine] Ciel entered, but the plush drop could not be started.", this);
			}
			return true;
		}

		// Token: 0x06008D66 RID: 36198 RVA: 0x0039BB34 File Offset: 0x00399D34
		private void UpdateInsideInput()
		{
			if (PlayerActivityGate.IsResourceOwned(PlayerContextResource.ModalUI))
			{
				return;
			}
			if (FlyCamera.IsActive)
			{
				return;
			}
			if (this.exitInput != null && this.exitInput.GetButtonDown())
			{
				this.BeginExit();
				return;
			}
			if (this.cameraOneInput != null && this.cameraOneInput.GetButtonDown())
			{
				this.ActivateFeatureCamera(0);
			}
			else if (this.cameraTwoInput != null && this.cameraTwoInput.GetButtonDown())
			{
				this.ActivateFeatureCamera(1);
			}
			else if (this.cameraThreeInput != null && this.cameraThreeInput.GetButtonDown())
			{
				this.ActivateFeatureCamera(2);
			}
			else if (this.cycleCameraInput != null && this.cycleCameraInput.GetButtonDown())
			{
				this.CycleFeatureCamera();
			}
			if (ControllerInput.IsActive && ControllerWorldPointer.Down(ControllerButton.RightStick))
			{
				FirstPersonController firstPersonController = this._firstPersonController;
				if (firstPersonController != null)
				{
					firstPersonController.ToggleFirstPerson();
				}
			}
			this.UpdateFeatureCameraNavigationInput();
		}

		// Token: 0x06008D67 RID: 36199 RVA: 0x0039BC10 File Offset: 0x00399E10
		private void BeginExit()
		{
			if (this._state != GachaMachineController.SessionState.Inside || this.IsFadeRunning)
			{
				return;
			}
			this.SuspendInsideFirstPersonView();
			this._state = GachaMachineController.SessionState.Exiting;
			this.ClearClawGrabOffset();
			this._pinExitFacing = false;
			this.HideInsideHud();
			this.DisableAllClawControl();
			if (this._sequence != null)
			{
				base.StopCoroutine(this._sequence);
			}
			this._sequence = base.StartCoroutine(this.ExitRoutine(this._lifecycleGeneration));
		}

		// Token: 0x06008D68 RID: 36200 RVA: 0x0039BC81 File Offset: 0x00399E81
		private IEnumerator ExitRoutine(int generation)
		{
			bool transferComplete = false;
			bool transferSucceeded = false;
			this.FadeTransition(delegate
			{
				transferSucceeded = this.TransferPlayerOutsideForExit(generation);
			}, delegate
			{
				transferComplete = true;
			}, generation);
			float fadeDeadline = Time.unscaledTime + this.FadeWatchdogDuration;
			while (!transferComplete && generation == this._lifecycleGeneration && Time.unscaledTime < fadeDeadline)
			{
				yield return null;
			}
			if (generation != this._lifecycleGeneration || this._state != GachaMachineController.SessionState.Exiting)
			{
				yield break;
			}
			if (!transferComplete || !transferSucceeded)
			{
				this.AbortFromSequence((!transferComplete) ? "Exit fade was interrupted before completion." : "The black-frame transfer outside failed.");
				yield break;
			}
			yield return this.WaitUntilAnimatorState("Ciel_Gacha_Exit", 2f, generation);
			if (!this.IsCurrentAnimatorState("Ciel_Gacha_Exit"))
			{
				this.AbortFromSequence("Animator did not enter 'Ciel_Gacha_Exit'.");
				yield break;
			}
			float timeout = Mathf.Max((this.exitClip != null) ? (this.exitClip.length + 2f) : 6f, 3f);
			float startedAt = Time.time;
			while (generation == this._lifecycleGeneration && this._state == GachaMachineController.SessionState.Exiting && Time.time - startedAt < timeout)
			{
				AnimatorStateInfo currentAnimatorStateInfo = this._playerAnimator.GetCurrentAnimatorStateInfo(0);
				if (!currentAnimatorStateInfo.IsName("Ciel_Gacha_Exit") || currentAnimatorStateInfo.normalizedTime >= 0.98f)
				{
					break;
				}
				yield return null;
			}
			if (generation != this._lifecycleGeneration || this._state != GachaMachineController.SessionState.Exiting)
			{
				yield break;
			}
			this.FinishExit();
			this._sequence = null;
			yield break;
		}

		// Token: 0x06008D69 RID: 36201 RVA: 0x0039BC98 File Offset: 0x00399E98
		private bool TransferPlayerOutsideForExit(int generation)
		{
			if (generation != this._lifecycleGeneration || this._state != GachaMachineController.SessionState.Exiting)
			{
				return false;
			}
			GachaMachineController.EntryPoint activeEntry = this.ActiveEntry;
			if (activeEntry == null || this._player == null || this._playerAnimator == null)
			{
				return false;
			}
			if (this.plushField != null)
			{
				this.plushField.ReleasePlayerCollisionProxy();
			}
			this.SetFeatureCamera(false);
			this.AttachPlayerToMachine(false);
			this._transferredInside = false;
			this._clawGrabOffsetWorld = Vector3.zero;
			if (activeEntry.enterStart == null)
			{
				return false;
			}
			Vector3 position = activeEntry.enterStart.position;
			this._exitFacingRotation = GachaMachineController.OppositeUprightRotation(activeEntry.enterStart);
			this._playerAnimator.Play("Ciel_Gacha_Exit", 0, this.exitStartNormalizedTime);
			this._playerAnimator.Update(0f);
			this.SnapPlayer(position, this._exitFacingRotation);
			this._pinExitFacing = true;
			PlayerStealthBridge.SetHiddenSource(this, false);
			return true;
		}

		// Token: 0x06008D6A RID: 36202 RVA: 0x0039BD8C File Offset: 0x00399F8C
		private void FinishExit()
		{
			PlayerStealthBridge.SetHiddenSource(this, false);
			this.DisableAllClawControl();
			if (this.plushField != null)
			{
				this.plushField.EndSession();
			}
			this.SetFeatureCamera(false);
			this.AttachPlayerToMachine(false);
			this._transferredInside = false;
			this._pinExitFacing = false;
			this.TransitionToLocomotion(false);
			this.RestorePlayerState();
			FullBodyHideActivityContext.Release(ref this._activityLease);
			this._activeEntryIndex = -1;
			this._state = GachaMachineController.SessionState.Outside;
			this._candidateEntryIndex = -1;
			this._candidateSqrDistance = float.MaxValue;
			if (this.debugLogs)
			{
				Debug.Log("[GachaMachine] Exit complete.", this);
			}
		}

		// Token: 0x06008D6B RID: 36203 RVA: 0x0039BE28 File Offset: 0x0039A028
		private IEnumerator WaitUntilAnimatorState(string stateName, float timeout, int generation)
		{
			float startedAt = Time.unscaledTime;
			while (generation == this._lifecycleGeneration && Time.unscaledTime - startedAt < timeout && this._playerAnimator != null && !this._playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06008D6C RID: 36204 RVA: 0x0039BE4C File Offset: 0x0039A04C
		private bool IsCurrentAnimatorState(string stateName)
		{
			return this._playerAnimator != null && this._playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
		}

		// Token: 0x06008D6D RID: 36205 RVA: 0x0039BE80 File Offset: 0x0039A080
		private void FadeTransition(Action middle, Action complete, int generation)
		{
			Action action = delegate
			{
				if (generation != this._lifecycleGeneration || !this.isActiveAndEnabled)
				{
					return;
				}
				try
				{
					Action middle2 = middle;
					if (middle2 != null)
					{
						middle2();
					}
				}
				catch (Exception ex)
				{
					Debug.LogException(ex, this);
				}
			};
			Action action2 = delegate
			{
				if (generation == this._lifecycleGeneration && this.isActiveAndEnabled)
				{
					this._sessionFade = null;
					Action complete2 = complete;
					if (complete2 == null)
					{
						return;
					}
					complete2();
				}
			};
			if (ScreenFadeController.Instance != null)
			{
				this._sessionFade = ScreenFadeController.Instance;
				this._sessionFade.FadeOutIn(action, this.fadeOutDuration, this.fadeInDuration, this.fadeHoldDuration, action2);
				return;
			}
			action();
			action2();
		}

		// Token: 0x06008D6E RID: 36206 RVA: 0x0039BF0C File Offset: 0x0039A10C
		private void EnableClawForSide(GachaMachineSide side)
		{
			this.DisableAllClawControl();
			if (this.clawRigs == null)
			{
				return;
			}
			GachaClawRig[] array = this.clawRigs;
			int i = 0;
			while (i < array.Length)
			{
				GachaClawRig gachaClawRig = array[i];
				if (!(gachaClawRig == null) && gachaClawRig.Side == side)
				{
					gachaClawRig.ConfigureGrabSession(this, this._player, this._playerAnimator);
					gachaClawRig.SetControlEnabled(true);
					if (gachaClawRig.ControlEnabled)
					{
						this._activeClaw = gachaClawRig;
						return;
					}
					gachaClawRig.ClearGrabSession();
					break;
				}
				else
				{
					i++;
				}
			}
			Debug.LogWarning(string.Format("[GachaMachine] No configured claw rig for side {0}.", side), this);
		}

		// Token: 0x06008D6F RID: 36207 RVA: 0x0039BF9C File Offset: 0x0039A19C
		private void DisableAllClawControl()
		{
			if (this.clawRigs != null)
			{
				foreach (GachaClawRig gachaClawRig in this.clawRigs)
				{
					if (gachaClawRig != null)
					{
						gachaClawRig.SetControlEnabled(false);
						gachaClawRig.ClearGrabSession();
					}
				}
			}
			this._activeClaw = null;
		}

		// Token: 0x06008D70 RID: 36208 RVA: 0x0039BFE8 File Offset: 0x0039A1E8
		private bool EnsurePlayerReferences()
		{
			if (this._player != null && this._playerAnimator != null && this._controller != null)
			{
				return true;
			}
			if (Time.unscaledTime < this._nextPlayerResolveTime)
			{
				return false;
			}
			this._nextPlayerResolveTime = Time.unscaledTime + 1f;
			this.ResolvePlayerReferences();
			return this._player != null && this._playerAnimator != null && this._controller != null;
		}

		// Token: 0x06008D71 RID: 36209 RVA: 0x0039C074 File Offset: 0x0039A274
		private bool EnsurePlayerTransform()
		{
			if (this._player != null)
			{
				return true;
			}
			if (Time.unscaledTime < this._nextPlayerResolveTime)
			{
				return false;
			}
			this._nextPlayerResolveTime = Time.unscaledTime + 1f;
			this.ResolvePlayerReferences();
			return this._player != null;
		}

		// Token: 0x06008D72 RID: 36210 RVA: 0x0039C0C4 File Offset: 0x0039A2C4
		private void ResolvePlayerReferences()
		{
			if (PlayerRegistry.Player != null)
			{
				this._player = PlayerRegistry.Player;
			}
			if (this._player == null)
			{
				vThirdPersonController vThirdPersonController = Object.FindObjectOfType<vThirdPersonController>();
				if (vThirdPersonController != null)
				{
					this._player = vThirdPersonController.transform;
				}
			}
			if (this._player == null)
			{
				return;
			}
			this._controller = this._player.GetComponentInChildren<vThirdPersonController>();
			this._thirdPersonInput = this._player.GetComponentInChildren<vThirdPersonInput>();
			this._actionAnimator = this._player.GetComponentInChildren<PlayerActionAnimator>();
			this._playerAnimator = ((this._controller != null && this._controller.animator != null) ? this._controller.animator : this._player.GetComponentInChildren<Animator>());
			this._firstPersonController = this._player.GetComponentInChildren<FirstPersonController>();
			this._playerBody = ((this._controller != null && this._controller._rigidbody != null) ? this._controller._rigidbody : this._player.GetComponent<Rigidbody>());
			this._playerCapsule = ((this._controller != null && this._controller._capsuleCollider != null) ? this._controller._capsuleCollider : this._player.GetComponent<CapsuleCollider>());
		}

		// Token: 0x06008D73 RID: 36211 RVA: 0x0039C224 File Offset: 0x0039A424
		private bool HasRequiredAnimatorStates()
		{
			return !(this._playerAnimator == null) && (this._playerAnimator.HasState(0, Animator.StringToHash("Ciel_Gacha_Enter")) && this._playerAnimator.HasState(0, Animator.StringToHash("Ciel_Gacha_Bored_L")) && this._playerAnimator.HasState(0, Animator.StringToHash("Ciel_Gacha_Bored_R"))) && this._playerAnimator.HasState(0, Animator.StringToHash("Ciel_Gacha_Exit"));
		}

		// Token: 0x06008D74 RID: 36212 RVA: 0x0039C2A4 File Offset: 0x0039A4A4
		private void ShowPrompt()
		{
			if (this._promptVisible && this.promptUI != null && this.promptUI.gameObject.activeInHierarchy)
			{
				return;
			}
			this._promptVisible = true;
			if (this.promptUI != null && this.enterConfig != null)
			{
				if (!this.promptUI.gameObject.activeSelf)
				{
					this.promptUI.gameObject.SetActive(true);
				}
				this.promptUI.Show(this.enterConfig);
			}
		}

		// Token: 0x06008D75 RID: 36213 RVA: 0x0039C334 File Offset: 0x0039A534
		private void HidePrompt()
		{
			if (!this._promptVisible && (this.promptUI == null || !this.promptUI.gameObject.activeSelf))
			{
				return;
			}
			this._promptVisible = false;
			if (this.promptUI != null)
			{
				this.promptUI.Hide();
			}
		}

		// Token: 0x06008D76 RID: 36214 RVA: 0x0039C38A File Offset: 0x0039A58A
		private void ShowInsideHud()
		{
			if (this._insideHudVisible)
			{
				return;
			}
			this._insideHudVisible = true;
			if (this.insideHudConfig != null && InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.Show(this.insideHudConfig);
			}
		}

		// Token: 0x06008D77 RID: 36215 RVA: 0x0039C3C7 File Offset: 0x0039A5C7
		private void HideInsideHud()
		{
			if (!this._insideHudVisible)
			{
				return;
			}
			this._insideHudVisible = false;
			if (InteractionHUDController.Instance != null)
			{
				InteractionHUDController.Instance.HideIfCurrent(this.insideHudConfig);
			}
		}

		// Token: 0x06008D78 RID: 36216 RVA: 0x0039C3F7 File Offset: 0x0039A5F7
		public bool InterruptForEncounter()
		{
			if (this._state == GachaMachineController.SessionState.Outside && !this._playerStateCaptured && this._activityLease == null)
			{
				return false;
			}
			this.ForceAbort();
			return true;
		}

		// Token: 0x06008D79 RID: 36217 RVA: 0x0039C41C File Offset: 0x0039A61C
		private void ForceAbort()
		{
			this._lifecycleGeneration++;
			FlyCamera.StopForActivityLease(this._activityLease);
			if (this._sequence != null)
			{
				base.StopCoroutine(this._sequence);
				this._sequence = null;
			}
			if (this._sessionFade != null)
			{
				this._sessionFade.SetClear();
			}
			this._sessionFade = null;
			PlayerStealthBridge.SetHiddenSource(this, false);
			this.HidePrompt();
			this.HideInsideHud();
			this.DisableAllClawControl();
			if (this.plushField != null)
			{
				this.plushField.EndSession();
			}
			this.SetFeatureCamera(false);
			this.AttachPlayerToMachine(false);
			this._transferredInside = false;
			this._clawGrabOffsetWorld = Vector3.zero;
			this._pinExitFacing = false;
			if (this._playerStateCaptured && this._player != null)
			{
				this.SnapPlayer(this._preSessionPosition, this._preSessionRotation);
			}
			this.TransitionToLocomotion(true);
			this.RestorePlayerState();
			FullBodyHideActivityContext.Release(ref this._activityLease);
			this._activeEntryIndex = -1;
			this._candidateEntryIndex = -1;
			this._candidateSqrDistance = float.MaxValue;
			this._state = GachaMachineController.SessionState.Outside;
		}

		// Token: 0x06008D7A RID: 36218 RVA: 0x0039C538 File Offset: 0x0039A738
		private void AbortFromSequence(string reason)
		{
			Debug.LogError("[GachaMachine] " + reason + " Restoring the player and aborting the session.", this);
			this._sequence = null;
			this.ForceAbort();
		}

		// Token: 0x06008D7B RID: 36219 RVA: 0x0039C560 File Offset: 0x0039A760
		private void OnDrawGizmosSelected()
		{
			if (this.entries == null)
			{
				return;
			}
			foreach (GachaMachineController.EntryPoint entryPoint in this.entries)
			{
				if (entryPoint != null && !(entryPoint.enterStart == null))
				{
					Gizmos.color = ((entryPoint.side == GachaMachineSide.Left) ? new Color(0.2f, 0.75f, 1f, 0.85f) : new Color(1f, 0.35f, 0.75f, 0.85f));
					Gizmos.DrawWireSphere(entryPoint.enterStart.position, this.interactionRadius);
					Gizmos.DrawRay(entryPoint.enterStart.position + Vector3.up * 0.05f, entryPoint.enterStart.forward * 0.55f);
					if (entryPoint.insidePose != null)
					{
						Gizmos.DrawLine(entryPoint.enterStart.position, entryPoint.insidePose.position);
					}
				}
			}
		}

		// Token: 0x06008D7C RID: 36220 RVA: 0x0039C668 File Offset: 0x0039A868
		private void CaptureAndLockPlayerState()
		{
			if (this._player == null || this._playerStateCaptured)
			{
				return;
			}
			this._preSessionPosition = this._player.position;
			this._preSessionRotation = this._player.rotation;
			this._preSessionParent = this._player.parent;
			this._preSessionScene = this._player.gameObject.scene;
			if (this._controller != null)
			{
				this._savedLockMovement = this._controller.lockMovement;
				this._savedLockRotation = this._controller.lockRotation;
			}
			if (this._thirdPersonInput != null)
			{
				this._savedLockCameraInput = this._thirdPersonInput.lockCameraInput;
				this._savedSuppressCameraZoom = this._thirdPersonInput.suppressCameraZoom;
			}
			if (this._playerBody != null)
			{
				this._savedBodyUseGravity = this._playerBody.useGravity;
				this._savedBodyKinematic = this._playerBody.isKinematic;
				this._savedBodyDetectCollisions = this._playerBody.detectCollisions;
			}
			if (this._playerCapsule != null)
			{
				this._savedCapsuleEnabled = this._playerCapsule.enabled;
				this._savedCapsuleTrigger = this._playerCapsule.isTrigger;
			}
			this._restoreFirstPerson = this._firstPersonController != null && this._firstPersonController.IsFirstPerson;
			this._playerStateCaptured = true;
			if (this._controller != null)
			{
				this._controller.lockMovement = true;
				this._controller.lockRotation = true;
			}
			if (this._thirdPersonInput != null)
			{
				this._thirdPersonInput.lockCameraInput = true;
				this._thirdPersonInput.suppressCameraZoom = true;
			}
			if (this._playerBody != null)
			{
				if (!this._playerBody.isKinematic)
				{
					this._playerBody.velocity = Vector3.zero;
					this._playerBody.angularVelocity = Vector3.zero;
				}
				this._playerBody.useGravity = false;
				this._playerBody.isKinematic = true;
			}
			if (this._playerCapsule != null)
			{
				this._playerCapsule.isTrigger = true;
			}
			if (this._actionAnimator != null)
			{
				this._actionAnimator.DisableEmotionLayers();
				this._actionAnimator.EnableRootMotionForAction();
				this._rootMotionPushed = true;
			}
			this.BeginForcedLayerWeights();
			if (this._restoreFirstPerson)
			{
				this._firstPersonController.ExitFirstPerson();
			}
			if (!this._playerStateCaptured)
			{
				return;
			}
			FirstPersonController firstPersonController = this._firstPersonController;
			if (firstPersonController != null)
			{
				firstPersonController.BeginInteractionView(this, true, 80f, false, true, null, null, null);
			}
			FirstPersonController firstPersonController2 = this._firstPersonController;
			if (firstPersonController2 == null)
			{
				return;
			}
			firstPersonController2.SuspendForInteraction(this);
		}

		// Token: 0x06008D7D RID: 36221 RVA: 0x0039C914 File Offset: 0x0039AB14
		private void RestorePlayerState()
		{
			if (!this._playerStateCaptured)
			{
				return;
			}
			this.EndForcedLayerWeights();
			if (this._rootMotionPushed && this._actionAnimator != null)
			{
				this._actionAnimator.RestoreRootMotionForAction();
				this._actionAnimator.RestoreOverrideLayer();
			}
			this._rootMotionPushed = false;
			if (this._playerCapsule != null)
			{
				this._playerCapsule.isTrigger = this._savedCapsuleTrigger;
				this._playerCapsule.enabled = this._savedCapsuleEnabled;
			}
			if (this._playerBody != null)
			{
				this._playerBody.isKinematic = this._savedBodyKinematic;
				this._playerBody.useGravity = this._savedBodyUseGravity;
				this._playerBody.detectCollisions = this._savedBodyDetectCollisions;
				if (!this._playerBody.isKinematic)
				{
					this._playerBody.velocity = Vector3.zero;
					this._playerBody.angularVelocity = Vector3.zero;
				}
			}
			if (this._controller != null)
			{
				this._controller.lockMovement = this._savedLockMovement;
				this._controller.lockRotation = this._savedLockRotation;
			}
			if (this._thirdPersonInput != null)
			{
				this._thirdPersonInput.lockCameraInput = this._savedLockCameraInput;
				this._thirdPersonInput.suppressCameraZoom = this._savedSuppressCameraZoom;
			}
			FirstPersonController firstPersonController = this._firstPersonController;
			if (firstPersonController != null)
			{
				firstPersonController.EndInteractionView(this);
			}
			FirstPersonController firstPersonController2 = this._firstPersonController;
			if (firstPersonController2 != null)
			{
				firstPersonController2.ResumeAfterInteraction(this);
			}
			if (this._restoreFirstPerson && this._firstPersonController != null)
			{
				this._firstPersonController.EnterFirstPerson();
			}
			this._restoreFirstPerson = false;
			this._playerStateCaptured = false;
		}

		// Token: 0x06008D7E RID: 36222 RVA: 0x0039CAB4 File Offset: 0x0039ACB4
		private void BeginForcedLayerWeights()
		{
			if (this._playerAnimator == null || this._layersForced)
			{
				return;
			}
			this._savedLayerWeights = new float[this._playerAnimator.layerCount];
			for (int i = 1; i < this._playerAnimator.layerCount; i++)
			{
				this._savedLayerWeights[i] = this._playerAnimator.GetLayerWeight(i);
			}
			this._layersForced = true;
			this.ApplyForcedLayerWeights();
		}

		// Token: 0x06008D7F RID: 36223 RVA: 0x0039CB28 File Offset: 0x0039AD28
		private void ApplyForcedLayerWeights()
		{
			if (this._playerAnimator == null)
			{
				return;
			}
			for (int i = 1; i < this._playerAnimator.layerCount; i++)
			{
				if (!(this._playerAnimator.GetLayerName(i) == "Face"))
				{
					this._playerAnimator.SetLayerWeight(i, 0f);
				}
			}
		}

		// Token: 0x06008D80 RID: 36224 RVA: 0x0039CB84 File Offset: 0x0039AD84
		private void EndForcedLayerWeights()
		{
			if (!this._layersForced)
			{
				return;
			}
			this._layersForced = false;
			if (this._playerAnimator != null && this._savedLayerWeights != null)
			{
				int num = Mathf.Min(this._playerAnimator.layerCount, this._savedLayerWeights.Length);
				for (int i = 1; i < num; i++)
				{
					if (!(this._playerAnimator.GetLayerName(i) == "Face"))
					{
						this._playerAnimator.SetLayerWeight(i, this._savedLayerWeights[i]);
					}
				}
			}
			this._savedLayerWeights = null;
		}

		// Token: 0x06008D81 RID: 36225 RVA: 0x0039CC10 File Offset: 0x0039AE10
		private void AttachPlayerToMachine(bool attach)
		{
			if (this._player == null || this._playerParented == attach)
			{
				return;
			}
			if (attach)
			{
				this._player.SetParent(base.transform, true);
			}
			else
			{
				this._player.SetParent((this._preSessionParent != null) ? this._preSessionParent : null, true);
				if (this._preSessionScene.IsValid() && this._preSessionScene.isLoaded && this._player.gameObject.scene != this._preSessionScene)
				{
					this._player.SetParent(null, true);
					SceneManager.MoveGameObjectToScene(this._player.gameObject, this._preSessionScene);
				}
			}
			this._playerParented = attach;
		}

		// Token: 0x06008D82 RID: 36226 RVA: 0x0039CCD0 File Offset: 0x0039AED0
		private void SnapPlayer(Vector3 position, Quaternion rotation)
		{
			if (this._player == null)
			{
				return;
			}
			this._player.SetPositionAndRotation(position, rotation);
			if (this._playerBody != null)
			{
				this._playerBody.position = position;
				this._playerBody.rotation = rotation;
				if (!this._playerBody.isKinematic)
				{
					this._playerBody.velocity = Vector3.zero;
					this._playerBody.angularVelocity = Vector3.zero;
				}
			}
			Physics.SyncTransforms();
		}

		// Token: 0x06008D83 RID: 36227 RVA: 0x0039CD54 File Offset: 0x0039AF54
		private void PinPlayerToInsidePose()
		{
			GachaMachineController.EntryPoint activeEntry = this.ActiveEntry;
			if (this._player == null || activeEntry == null || activeEntry.insidePose == null)
			{
				return;
			}
			Vector3 vector = activeEntry.insidePose.position + this._clawGrabOffsetWorld;
			this._player.SetPositionAndRotation(vector, activeEntry.insidePose.rotation);
			if (this._playerBody != null)
			{
				this._playerBody.position = vector;
				this._playerBody.rotation = activeEntry.insidePose.rotation;
			}
		}

		// Token: 0x06008D84 RID: 36228 RVA: 0x0039CDE8 File Offset: 0x0039AFE8
		public void SetClawGrabOffset(Vector3 requestedWorldOffset)
		{
			if (this._state != GachaMachineController.SessionState.Inside || !this._transferredInside)
			{
				this._clawGrabOffsetWorld = Vector3.zero;
				return;
			}
			Vector3 vector = base.transform.InverseTransformVector(requestedWorldOffset);
			vector.x = Mathf.Clamp(vector.x, -this.maximumClawGrabLateralOffset, this.maximumClawGrabLateralOffset);
			vector.y = Mathf.Clamp(vector.y, 0f, this.maximumClawGrabLift);
			vector.z = Mathf.Clamp(vector.z, -this.maximumClawGrabDepthOffset, this.maximumClawGrabDepthOffset);
			this._clawGrabOffsetWorld = base.transform.TransformVector(vector);
		}

		// Token: 0x06008D85 RID: 36229 RVA: 0x0039CE8C File Offset: 0x0039B08C
		public void ClearClawGrabOffset()
		{
			this._clawGrabOffsetWorld = Vector3.zero;
		}

		// Token: 0x06008D86 RID: 36230 RVA: 0x0039CE9C File Offset: 0x0039B09C
		private void PinPlayerExitFacing()
		{
			if (this._player == null)
			{
				return;
			}
			this._player.rotation = this._exitFacingRotation;
			if (this._playerBody != null)
			{
				this._playerBody.rotation = this._exitFacingRotation;
			}
		}

		// Token: 0x06008D87 RID: 36231 RVA: 0x0039CEE8 File Offset: 0x0039B0E8
		private static Quaternion UprightRotation(Transform anchor)
		{
			Vector3 vector = Vector3.ProjectOnPlane(anchor.forward, Vector3.up);
			if (vector.sqrMagnitude <= 0.0001f)
			{
				return Quaternion.Euler(0f, anchor.eulerAngles.y, 0f);
			}
			return Quaternion.LookRotation(vector.normalized, Vector3.up);
		}

		// Token: 0x06008D88 RID: 36232 RVA: 0x0039CF40 File Offset: 0x0039B140
		private static Quaternion OppositeUprightRotation(Transform anchor)
		{
			Vector3 vector = -Vector3.ProjectOnPlane(anchor.forward, Vector3.up);
			if (vector.sqrMagnitude <= 0.0001f)
			{
				return Quaternion.Euler(0f, anchor.eulerAngles.y + 180f, 0f);
			}
			return Quaternion.LookRotation(vector.normalized, Vector3.up);
		}

		// Token: 0x06008D89 RID: 36233 RVA: 0x0039CFA4 File Offset: 0x0039B1A4
		private bool TransitionToLocomotion(bool immediate)
		{
			if (this._playerAnimator == null)
			{
				return false;
			}
			string[] array = new string[] { "Base Layer.Locomotion.Free Locomotion.Free Movement", "Free Movement", "Locomotion", "Grounded", "Idle" };
			for (int i = 0; i < array.Length; i++)
			{
				int num = Animator.StringToHash(array[i]);
				if (this._playerAnimator.HasState(0, num))
				{
					if (immediate)
					{
						this._playerAnimator.Play(num, 0, 0f);
					}
					else
					{
						this._playerAnimator.CrossFadeInFixedTime(num, this.animationCrossFade, 0);
					}
					return true;
				}
			}
			Debug.LogError("[GachaMachine] Could not resolve Ciel's Free Movement state; CustomAction cleanup could not be guaranteed.", this);
			return false;
		}

		// Token: 0x0400903D RID: 36925
		[Header("Feature Cameras")]
		[Tooltip("Three authored camera presets created below the machine prefab.")]
		[SerializeField]
		private Camera[] featureCameras = Array.Empty<Camera>();

		// Token: 0x0400903E RID: 36926
		[SerializeField]
		private GenericInput cameraOneInput = new GenericInput("Alpha1", "DPadLeft", "DPadLeft");

		// Token: 0x0400903F RID: 36927
		[SerializeField]
		private GenericInput cameraTwoInput = new GenericInput("Alpha2", "DPadUp", "DPadUp");

		// Token: 0x04009040 RID: 36928
		[SerializeField]
		private GenericInput cameraThreeInput = new GenericInput("Alpha3", "DPadRight", "DPadRight");

		// Token: 0x04009041 RID: 36929
		[SerializeField]
		private GenericInput cycleCameraInput = new GenericInput("C", "Y", "Y");

		// Token: 0x04009042 RID: 36930
		[Header("Camera Zoom")]
		[FormerlySerializedAs("enableCameraZoomAndPan")]
		[SerializeField]
		private bool enableCameraZoom = true;

		// Token: 0x04009043 RID: 36931
		[SerializeField]
		[Range(5f, 60f)]
		private float minimumCameraFieldOfView = 20f;

		// Token: 0x04009044 RID: 36932
		[SerializeField]
		[Min(0.1f)]
		private float cameraZoomDegreesPerWheelStep = 7f;

		// Token: 0x04009045 RID: 36933
		[SerializeField]
		[Min(0.1f)]
		private float cameraZoomSmoothingResponse = 12f;

		// Token: 0x04009046 RID: 36934
		[SerializeField]
		[HideInInspector]
		private int approvedCameraPresetVersion;

		// Token: 0x04009047 RID: 36935
		private Camera _disabledMainCamera;

		// Token: 0x04009048 RID: 36936
		private bool _disabledMainCameraWasEnabled;

		// Token: 0x04009049 RID: 36937
		private Camera _activeFeatureCamera;

		// Token: 0x0400904A RID: 36938
		private int _activeCameraIndex;

		// Token: 0x0400904B RID: 36939
		private GachaMachineController.FeatureCameraNavigationState[] _featureCameraNavigation;

		// Token: 0x0400904C RID: 36940
		private bool _firstPersonViewBound;

		// Token: 0x0400904D RID: 36941
		public const string EnterStateName = "Ciel_Gacha_Enter";

		// Token: 0x0400904E RID: 36942
		public const string IdleLeftStateName = "Ciel_Gacha_Bored_L";

		// Token: 0x0400904F RID: 36943
		public const string IdleRightStateName = "Ciel_Gacha_Bored_R";

		// Token: 0x04009050 RID: 36944
		public const string ExitStateName = "Ciel_Gacha_Exit";

		// Token: 0x04009051 RID: 36945
		[Header("Entrances")]
		[SerializeField]
		private GachaMachineController.EntryPoint[] entries = Array.Empty<GachaMachineController.EntryPoint>();

		// Token: 0x04009052 RID: 36946
		[SerializeField]
		[Min(0.2f)]
		private float interactionRadius = 1.2f;

		// Token: 0x04009053 RID: 36947
		[SerializeField]
		[Min(0f)]
		private float maximumVerticalDifference = 0.8f;

		// Token: 0x04009054 RID: 36948
		[SerializeField]
		[Range(0f, 180f)]
		private float maximumFacingAngle = 62f;

		// Token: 0x04009055 RID: 36949
		[Tooltip("Require Ciel to face the hatch before offering the interaction.")]
		[SerializeField]
		private bool requirePlayerFacingHatch;

		// Token: 0x04009056 RID: 36950
		[Header("Interaction UI")]
		[SerializeField]
		private InteractionConfigSO enterConfig;

		// Token: 0x04009057 RID: 36951
		[SerializeField]
		private InteractionConfigSO insideHudConfig;

		// Token: 0x04009058 RID: 36952
		[SerializeField]
		private InteractablePromptUI promptUI;

		// Token: 0x04009059 RID: 36953
		[SerializeField]
		private GenericInput interactInput = new GenericInput("E", "A", "A");

		// Token: 0x0400905A RID: 36954
		[SerializeField]
		private GenericInput exitInput = new GenericInput("E", "B", "B");

		// Token: 0x0400905B RID: 36955
		[Header("Animation")]
		[SerializeField]
		private AnimationClip enterClip;

		// Token: 0x0400905C RID: 36956
		[SerializeField]
		private AnimationClip idleClip;

		// Token: 0x0400905D RID: 36957
		[SerializeField]
		private AnimationClip exitClip;

		// Token: 0x0400905E RID: 36958
		[Tooltip("EnterFade is 8 seconds long, but its useful partial action ends at exactly 2 seconds. Fade-out begins early enough for the black-frame transfer to land on this time.")]
		[SerializeField]
		[Min(0f)]
		private float enterTransferTimeSeconds = 2f;

		// Token: 0x0400905F RID: 36959
		[Tooltip("Exit matches Enter at normalized 0.60: start there while black, then reveal the stand-up portion.")]
		[SerializeField]
		[Range(0f, 1f)]
		private float exitStartNormalizedTime = 0.6f;

		// Token: 0x04009060 RID: 36960
		[SerializeField]
		[Range(0f, 0.5f)]
		private float animationCrossFade = 0.12f;

		// Token: 0x04009061 RID: 36961
		[Header("Fade")]
		[SerializeField]
		[Min(0.01f)]
		private float fadeOutDuration = 0.25f;

		// Token: 0x04009062 RID: 36962
		[SerializeField]
		[Min(0.01f)]
		private float fadeInDuration = 0.28f;

		// Token: 0x04009063 RID: 36963
		[SerializeField]
		[Min(0f)]
		private float fadeHoldDuration = 0.12f;

		// Token: 0x04009064 RID: 36964
		[Header("Claws")]
		[SerializeField]
		private GachaClawRig[] clawRigs = Array.Empty<GachaClawRig>();

		// Token: 0x04009065 RID: 36965
		[Header("Claw Grab Response")]
		[Tooltip("Maximum sideways displacement of Ciel while a closed claw pulls a body zone.")]
		[SerializeField]
		[Min(0f)]
		private float maximumClawGrabLateralOffset = 0.08f;

		// Token: 0x04009066 RID: 36966
		[Tooltip("Maximum upward displacement of Ciel while being lifted by the claw.")]
		[SerializeField]
		[Min(0f)]
		private float maximumClawGrabLift = 0.14f;

		// Token: 0x04009067 RID: 36967
		[Tooltip("Maximum front/back displacement of Ciel while held by the claw.")]
		[SerializeField]
		[Min(0f)]
		private float maximumClawGrabDepthOffset = 0.06f;

		// Token: 0x04009068 RID: 36968
		[Header("Plush Field")]
		[Tooltip("Persistent physical plush pile. Only Ciel's active bay is reshuffled and dropped on entry.")]
		[SerializeField]
		private GachaPlushField plushField;

		// Token: 0x04009069 RID: 36969
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x0400906A RID: 36970
		private Transform _player;

		// Token: 0x0400906B RID: 36971
		private Animator _playerAnimator;

		// Token: 0x0400906C RID: 36972
		private PlayerActionAnimator _actionAnimator;

		// Token: 0x0400906D RID: 36973
		private vThirdPersonController _controller;

		// Token: 0x0400906E RID: 36974
		private vThirdPersonInput _thirdPersonInput;

		// Token: 0x0400906F RID: 36975
		private FirstPersonController _firstPersonController;

		// Token: 0x04009070 RID: 36976
		private Rigidbody _playerBody;

		// Token: 0x04009071 RID: 36977
		private CapsuleCollider _playerCapsule;

		// Token: 0x04009072 RID: 36978
		private GachaMachineController.SessionState _state;

		// Token: 0x04009073 RID: 36979
		private int _candidateEntryIndex = -1;

		// Token: 0x04009074 RID: 36980
		private int _activeEntryIndex = -1;

		// Token: 0x04009075 RID: 36981
		private float _candidateSqrDistance = float.MaxValue;

		// Token: 0x04009076 RID: 36982
		private float _nextPlayerResolveTime;

		// Token: 0x04009077 RID: 36983
		private bool _promptVisible;

		// Token: 0x04009078 RID: 36984
		private bool _insideHudVisible;

		// Token: 0x04009079 RID: 36985
		private Coroutine _sequence;

		// Token: 0x0400907A RID: 36986
		private ScreenFadeController _sessionFade;

		// Token: 0x0400907B RID: 36987
		private int _lifecycleGeneration;

		// Token: 0x0400907C RID: 36988
		private PlayerActivityLease _activityLease;

		// Token: 0x0400907D RID: 36989
		private GachaClawRig _activeClaw;

		// Token: 0x0400907E RID: 36990
		private bool _transferredInside;

		// Token: 0x0400907F RID: 36991
		private Vector3 _clawGrabOffsetWorld;

		// Token: 0x04009080 RID: 36992
		private bool _pinExitFacing;

		// Token: 0x04009081 RID: 36993
		private Quaternion _exitFacingRotation;

		// Token: 0x04009082 RID: 36994
		private bool _playerStateCaptured;

		// Token: 0x04009083 RID: 36995
		private Vector3 _preSessionPosition;

		// Token: 0x04009084 RID: 36996
		private Quaternion _preSessionRotation;

		// Token: 0x04009085 RID: 36997
		private Transform _preSessionParent;

		// Token: 0x04009086 RID: 36998
		private Scene _preSessionScene;

		// Token: 0x04009087 RID: 36999
		private bool _playerParented;

		// Token: 0x04009088 RID: 37000
		private bool _savedLockMovement;

		// Token: 0x04009089 RID: 37001
		private bool _savedLockRotation;

		// Token: 0x0400908A RID: 37002
		private bool _savedLockCameraInput;

		// Token: 0x0400908B RID: 37003
		private bool _savedSuppressCameraZoom;

		// Token: 0x0400908C RID: 37004
		private bool _savedBodyUseGravity;

		// Token: 0x0400908D RID: 37005
		private bool _savedBodyKinematic;

		// Token: 0x0400908E RID: 37006
		private bool _savedBodyDetectCollisions;

		// Token: 0x0400908F RID: 37007
		private bool _savedCapsuleEnabled;

		// Token: 0x04009090 RID: 37008
		private bool _savedCapsuleTrigger;

		// Token: 0x04009091 RID: 37009
		private bool _restoreFirstPerson;

		// Token: 0x04009092 RID: 37010
		private bool _rootMotionPushed;

		// Token: 0x04009093 RID: 37011
		private float[] _savedLayerWeights;

		// Token: 0x04009094 RID: 37012
		private bool _layersForced;

		// Token: 0x020011E6 RID: 4582
		private sealed class FeatureCameraNavigationState
		{
			// Token: 0x04009095 RID: 37013
			public Camera camera;

			// Token: 0x04009096 RID: 37014
			public Vector3 authoredLocalPosition;

			// Token: 0x04009097 RID: 37015
			public Quaternion authoredLocalRotation;

			// Token: 0x04009098 RID: 37016
			public float authoredFieldOfView;

			// Token: 0x04009099 RID: 37017
			public float targetFieldOfView;
		}

		// Token: 0x020011E7 RID: 4583
		public enum SessionState
		{
			// Token: 0x0400909B RID: 37019
			Outside,
			// Token: 0x0400909C RID: 37020
			Entering,
			// Token: 0x0400909D RID: 37021
			Inside,
			// Token: 0x0400909E RID: 37022
			Exiting
		}

		// Token: 0x020011E8 RID: 4584
		[Serializable]
		public sealed class EntryPoint
		{
			// Token: 0x0400909F RID: 37023
			public GachaMachineSide side;

			// Token: 0x040090A0 RID: 37024
			[Tooltip("Standing start of GMachine01_EnterFade, immediately in front of the black hatch.")]
			public Transform enterStart;

			// Token: 0x040090A1 RID: 37025
			[Tooltip("Start of GMachine01_Exit, revealed while the screen is black.")]
			public Transform exitStart;

			// Token: 0x040090A2 RID: 37026
			[Tooltip("Final stationary root for GMachine04_Bored among the plush toys.")]
			public Transform insidePose;
		}
	}
}
