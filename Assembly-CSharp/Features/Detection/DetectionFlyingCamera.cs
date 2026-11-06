using System;
using System.Collections.Generic;
using Cinemachine;
using Features.DebugTools;
using Invector;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Features.Detection
{
	// Token: 0x0200126F RID: 4719
	public class DetectionFlyingCamera : MonoBehaviour
	{
		// Token: 0x060092BB RID: 37563 RVA: 0x003C3113 File Offset: 0x003C1313
		internal static void NormalizePhotoInput(GenericInput input)
		{
			if (input == null)
			{
				return;
			}
			input.keyboard = "K";
			input.keyboardAxis = false;
			input.preserveJoystickBinding = true;
		}

		// Token: 0x17001760 RID: 5984
		// (get) Token: 0x060092BC RID: 37564 RVA: 0x003C3132 File Offset: 0x003C1332
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
		}

		// Token: 0x17001761 RID: 5985
		// (get) Token: 0x060092BD RID: 37565 RVA: 0x003C313A File Offset: 0x003C133A
		public float CurrentMoveSpeed
		{
			get
			{
				return this._currentMoveSpeed;
			}
		}

		// Token: 0x060092BE RID: 37566 RVA: 0x003C3144 File Offset: 0x003C1344
		private void Awake()
		{
			DetectionFlyingCamera.NormalizePhotoInput(this.ExitInput);
			ControllerContextBindings.Set(this.ExitInput, "B");
			if (this.VirtualCamera != null)
			{
				this._savedPriority = this.VirtualCamera.Priority;
				if (!this.VirtualCamera.gameObject.activeSelf)
				{
					this.VirtualCamera.gameObject.SetActive(true);
				}
			}
			if (this.FlyModeUI != null)
			{
				this.FlyModeUI.SetActive(false);
			}
			this._cachedBrain = Object.FindObjectOfType<CinemachineBrain>();
		}

		// Token: 0x060092BF RID: 37567 RVA: 0x003C31D4 File Offset: 0x003C13D4
		public void Activate(Vector3 origin, Camera outputCamera = null, float? initialMoveSpeed = null)
		{
			FlyCamera.ConsumeToggleForCurrentFrame();
			DetectionFlyingCamera.NormalizePhotoInput(this.ExitInput);
			if (this._isActive)
			{
				Debug.LogWarning("[DetectionFlyingCamera] Activate called while already active");
				return;
			}
			if (this.VirtualCamera == null)
			{
				Debug.LogError("[DetectionFlyingCamera] No VirtualCamera assigned!");
				return;
			}
			Debug.Log(string.Format("[DetectionFlyingCamera] Activate requested | VCam='{0}' activeSelf={1} activeInHierarchy={2} priority={3}", new object[]
			{
				this.VirtualCamera.name,
				this.VirtualCamera.gameObject.activeSelf,
				this.VirtualCamera.gameObject.activeInHierarchy,
				this.VirtualCamera.Priority
			}));
			this._vcamTransform = this.VirtualCamera.transform;
			this._origin = origin;
			this._currentMoveSpeed = Mathf.Clamp(initialMoveSpeed ?? this.MoveSpeed, this.MinSpeed, this.MaxSpeed);
			this._currentVelocity = (this._velocityRef = Vector3.zero);
			if (outputCamera == null)
			{
				outputCamera = Camera.main;
			}
			this._cachedBrain = ((outputCamera != null) ? outputCamera.GetComponent<CinemachineBrain>() : null);
			if (outputCamera == null)
			{
				this._cachedBrain = CinemachineCore.Instance.FindPotentialTargetBrain(this.VirtualCamera);
				outputCamera = ((this._cachedBrain != null) ? this._cachedBrain.OutputCamera : null);
			}
			if (!this.VirtualCamera.gameObject.activeSelf)
			{
				this.VirtualCamera.gameObject.SetActive(true);
				Debug.Log("[DetectionFlyingCamera] VirtualCamera GO was inactive, forced active for FlyMode");
			}
			this._savedFollow = this.VirtualCamera.Follow;
			this._savedLookAt = this.VirtualCamera.LookAt;
			if (this._savedFollow != null || this._savedLookAt != null)
			{
				Debug.LogWarning("[DetectionFlyingCamera] Clearing Follow/LookAt for manual fly control | Follow=" + ((this._savedFollow != null) ? this._savedFollow.name : "null") + ", LookAt=" + ((this._savedLookAt != null) ? this._savedLookAt.name : "null"));
			}
			this.VirtualCamera.Follow = null;
			this.VirtualCamera.LookAt = null;
			this._savedVirtualNearClip = this.VirtualCamera.m_Lens.NearClipPlane;
			this.VirtualCamera.m_Lens.NearClipPlane = 0.001f;
			this._photoOutput = outputCamera;
			if (this._photoOutput != null)
			{
				this._savedOutputNearClip = this._photoOutput.nearClipPlane;
				this._savedOcclusionCulling = this._photoOutput.useOcclusionCulling;
				this.ApplyPhotoClipping();
				FlyCamera.DisableCullingFade(null, this._disabledCullingFades);
				CinemachineCore.CameraUpdatedEvent.AddListener(new UnityAction<CinemachineBrain>(this.OnCameraUpdated));
			}
			if (outputCamera != null)
			{
				this._vcamTransform.position = outputCamera.transform.position;
				this._vcamTransform.rotation = outputCamera.transform.rotation;
			}
			else
			{
				this._vcamTransform.position = origin + Vector3.up * 1.6f + Vector3.back * 2f;
				this._vcamTransform.rotation = Quaternion.identity;
			}
			Vector3 eulerAngles = this._vcamTransform.eulerAngles;
			this._yaw = eulerAngles.y;
			this._pitch = eulerAngles.x;
			if (this._pitch > 180f)
			{
				this._pitch -= 360f;
			}
			this._targetYaw = this._yaw;
			this._targetPitch = this._pitch;
			this._savedPriority = this.VirtualCamera.Priority;
			int num = int.MinValue;
			foreach (CinemachineVirtualCameraBase cinemachineVirtualCameraBase in Object.FindObjectsOfType<CinemachineVirtualCameraBase>(true))
			{
				if (!(cinemachineVirtualCameraBase == null) && !(cinemachineVirtualCameraBase == this.VirtualCamera) && cinemachineVirtualCameraBase.Priority > num)
				{
					num = cinemachineVirtualCameraBase.Priority;
				}
			}
			int num2 = this._savedPriority + this.ActivePriorityBoost;
			int num3 = ((num == int.MinValue) ? num2 : (num + 10));
			int num4 = Mathf.Max(num2, num3);
			this.VirtualCamera.Priority = num4;
			Debug.Log(string.Format("[DetectionFlyingCamera] Priority boosted | old={0} new={1} maxOther={2}", this._savedPriority, this.VirtualCamera.Priority, num));
			if (this.FlyModeUI != null)
			{
				this.FlyModeUI.SetActive(false);
			}
			this.HideSceneUI();
			if (outputCamera != null)
			{
				if (this._photoMode == null)
				{
					this._photoMode = base.gameObject.AddComponent<FlyCameraPhotoMode>();
				}
				string text = ((this.ExitInput != null && !string.IsNullOrEmpty(this.ExitInput.keyboard)) ? this.ExitInput.keyboard : "K");
				this._photoMode.Enter(outputCamera, outputCamera, text, true);
			}
			this._isActive = true;
			this._nextControlLogTime = Time.unscaledTime;
			this._loggedLiveCameraStatus = false;
			Debug.Log(string.Format("[DetectionFlyingCamera] Activated at origin {0}, radius {1}", origin, this.MaxRadius));
		}

		// Token: 0x060092C0 RID: 37568 RVA: 0x003C3708 File Offset: 0x003C1908
		public void Deactivate()
		{
			FlyCamera.ConsumeToggleForCurrentFrame();
			if (!this._isActive)
			{
				Debug.LogWarning("[DetectionFlyingCamera] Deactivate called while not active");
				return;
			}
			this._isActive = false;
			CinemachineCore.CameraUpdatedEvent.RemoveListener(new UnityAction<CinemachineBrain>(this.OnCameraUpdated));
			if (this._photoMode != null)
			{
				this._photoMode.Exit();
			}
			if (this._photoOutput != null)
			{
				this._photoOutput.nearClipPlane = this._savedOutputNearClip;
				this._photoOutput.useOcclusionCulling = this._savedOcclusionCulling;
				this._photoOutput = null;
			}
			FlyCamera.RestoreCullingFade(this._disabledCullingFades);
			this.RestoreSceneUI();
			if (this.FlyModeUI != null)
			{
				this.FlyModeUI.SetActive(false);
				Debug.Log("[DetectionFlyingCamera] FlyModeUI hidden on deactivate");
			}
			if (this.VirtualCamera != null)
			{
				this.VirtualCamera.Priority = this._savedPriority;
				this.VirtualCamera.Follow = this._savedFollow;
				this.VirtualCamera.LookAt = this._savedLookAt;
				this.VirtualCamera.m_Lens.NearClipPlane = this._savedVirtualNearClip;
				this.VirtualCamera.gameObject.SetActive(true);
				Debug.Log(string.Format("[DetectionFlyingCamera] VCam restored | priority={0} activeSelf={1} follow={2} lookAt={3}", new object[]
				{
					this.VirtualCamera.Priority,
					this.VirtualCamera.gameObject.activeSelf,
					(this._savedFollow != null) ? this._savedFollow.name : "null",
					(this._savedLookAt != null) ? this._savedLookAt.name : "null"
				}));
			}
			Debug.Log("[DetectionFlyingCamera] Deactivated, VCam priority restored");
		}

		// Token: 0x060092C1 RID: 37569 RVA: 0x003C38CA File Offset: 0x003C1ACA
		public bool WasExitPressed()
		{
			if (GraphicsSettingsRuntimeUI.AnyOpen)
			{
				return false;
			}
			bool flag = this.WasEscapePressed() || (this._isActive && this.ExitInput.GetButtonDown());
			if (flag)
			{
				Debug.Log("[DetectionFlyingCamera] Exit input detected");
			}
			return flag;
		}

		// Token: 0x060092C2 RID: 37570 RVA: 0x003C3903 File Offset: 0x003C1B03
		public bool WasEscapePressed()
		{
			return this._isActive && EscapeInputCoordinator.TryConsumeForPhotoMode(this);
		}

		// Token: 0x060092C3 RID: 37571 RVA: 0x003C3915 File Offset: 0x003C1B15
		public bool IsExitInputPressed()
		{
			return this._isActive && this.ExitInput.GetButton();
		}

		// Token: 0x060092C4 RID: 37572 RVA: 0x003C392C File Offset: 0x003C1B2C
		public bool HandleOwnerInput(GenericInput enterInput, ref bool ownsCamera, DetectionCameraController presetCamera, Vector3 origin, Camera outputCamera)
		{
			if (GraphicsSettingsRuntimeUI.AnyOpen)
			{
				return false;
			}
			if (this._isActive)
			{
				if (!ownsCamera || (!this.WasExitPressed() && (enterInput == null || !enterInput.GetButtonDown())))
				{
					return false;
				}
				this.Deactivate();
				ownsCamera = false;
				if (presetCamera != null)
				{
					presetCamera.OnFlyModeExit();
				}
				return true;
			}
			else
			{
				if (ownsCamera)
				{
					ownsCamera = false;
					if (presetCamera != null)
					{
						presetCamera.OnFlyModeExit();
					}
				}
				if (EscapeInputCoordinator.ConsumedThisFrame)
				{
					return false;
				}
				if (enterInput == null || !enterInput.GetButtonDown())
				{
					return false;
				}
				if (presetCamera != null)
				{
					presetCamera.OnFlyModeEnter();
				}
				this.Activate(origin, outputCamera, new float?(1.5f));
				ownsCamera = this.IsActive;
				if (!ownsCamera && presetCamera != null)
				{
					presetCamera.OnFlyModeExit();
				}
				return true;
			}
		}

		// Token: 0x060092C5 RID: 37573 RVA: 0x003C39E8 File Offset: 0x003C1BE8
		private void Update()
		{
			if (!this._isActive || this._vcamTransform == null)
			{
				return;
			}
			if (GraphicsSettingsRuntimeUI.AnyOpen)
			{
				return;
			}
			if (TextInputFocus.BlocksGameplayInput)
			{
				return;
			}
			if (!this._loggedLiveCameraStatus)
			{
				if (this._cachedBrain == null)
				{
					this._cachedBrain = Object.FindObjectOfType<CinemachineBrain>();
				}
				if (this._cachedBrain == null)
				{
					Debug.LogError("[DetectionFlyingCamera] No CinemachineBrain found in scene. FlyCam VCam cannot become live");
				}
				else if (this._cachedBrain.ActiveVirtualCamera == null)
				{
					Debug.LogWarning("[DetectionFlyingCamera] CinemachineBrain has no ActiveVirtualCamera yet");
				}
				else
				{
					string text = ((this._cachedBrain.ActiveVirtualCamera.VirtualCameraGameObject != null) ? this._cachedBrain.ActiveVirtualCamera.VirtualCameraGameObject.name : this._cachedBrain.ActiveVirtualCamera.Name);
					bool flag = this._cachedBrain.ActiveVirtualCamera.VirtualCameraGameObject == this.VirtualCamera.gameObject;
					Debug.Log(string.Format("[DetectionFlyingCamera] Brain live camera='{0}' flyCamLive={1}", text, flag));
					if (!flag)
					{
						Debug.LogWarning("[DetectionFlyingCamera] FlyCam is active but NOT live on brain. Check competing VCam priorities/blends.");
					}
				}
				this._loggedLiveCameraStatus = true;
			}
			Vector2 mouseDelta = FlyCamera.GetMouseDelta();
			float x = mouseDelta.x;
			float y = mouseDelta.y;
			bool flag2 = Mathf.Abs(x) > 0.0001f || Mathf.Abs(y) > 0.0001f;
			bool flag3 = FlyCamera.GetMoveInput().sqrMagnitude > 0.0001f;
			if ((flag2 || flag3) && Time.unscaledTime >= this._nextControlLogTime)
			{
				this._nextControlLogTime = Time.unscaledTime + 0.5f;
				Debug.Log(string.Format("[DetectionFlyingCamera] Control input detected | move={0} look={1} mouse=({2:0.000},{3:0.000}) pos={4} rot={5}", new object[]
				{
					flag3,
					flag2,
					x,
					y,
					this._vcamTransform.position,
					this._vcamTransform.eulerAngles
				}));
			}
			this.HandleLook();
			this.HandleMovement();
			this.EnforceBounds();
			if (FlyCamera.GetShutterKeyDown() && this._photoMode != null)
			{
				this._photoMode.CapturePhoto();
			}
		}

		// Token: 0x060092C6 RID: 37574 RVA: 0x003C3C04 File Offset: 0x003C1E04
		private void HandleLook()
		{
			Vector2 vector = FlyCamera.GetMouseDelta() * this.LookSensitivity;
			this._targetYaw += vector.x;
			this._targetPitch = Mathf.Clamp(this._targetPitch - vector.y, -89f, 89f);
			float num = 1f - Mathf.Exp(-this.LookSmoothing * Time.unscaledDeltaTime);
			this._yaw = Mathf.LerpAngle(this._yaw, this._targetYaw, num);
			this._pitch = Mathf.Lerp(this._pitch, this._targetPitch, num);
			this._vcamTransform.rotation = Quaternion.Euler(this._pitch, this._yaw, 0f);
		}

		// Token: 0x060092C7 RID: 37575 RVA: 0x003C3CC1 File Offset: 0x003C1EC1
		private void OnCameraUpdated(CinemachineBrain brain)
		{
			if (!this._isActive || brain != this._cachedBrain)
			{
				return;
			}
			this.ApplyPhotoClipping();
		}

		// Token: 0x060092C8 RID: 37576 RVA: 0x003C3CE0 File Offset: 0x003C1EE0
		private void ApplyPhotoClipping()
		{
			if (this._photoOutput == null)
			{
				return;
			}
			this._photoOutput.nearClipPlane = 0.001f;
			this._photoOutput.useOcclusionCulling = false;
		}

		// Token: 0x060092C9 RID: 37577 RVA: 0x003C3D10 File Offset: 0x003C1F10
		private void HandleMovement()
		{
			this.AdjustMovementSpeed(FlyCamera.GetScroll(), FlyCamera.GetActionSpeedModifier());
			float num = this._currentMoveSpeed * (FlyCamera.GetBoost() ? this.FastMultiplier : 1f);
			Vector3 vector = this._vcamTransform.TransformDirection(FlyCamera.GetMoveInput()) * num;
			this._currentVelocity = Vector3.SmoothDamp(this._currentVelocity, vector, ref this._velocityRef, this.MoveSmoothTime, float.PositiveInfinity, Time.unscaledDeltaTime);
			this._vcamTransform.position += this._currentVelocity * Time.unscaledDeltaTime;
		}

		// Token: 0x060092CA RID: 37578 RVA: 0x003C3DAE File Offset: 0x003C1FAE
		private void AdjustMovementSpeed(float scroll, bool actionSpeedModifier)
		{
			if (actionSpeedModifier || Mathf.Abs(scroll) <= 0.01f)
			{
				return;
			}
			this._currentMoveSpeed = FlyCamera.AdjustMoveSpeed(this._currentMoveSpeed, scroll, this.MinSpeed, this.MaxSpeed);
		}

		// Token: 0x060092CB RID: 37579 RVA: 0x003C3DE0 File Offset: 0x003C1FE0
		private void EnforceBounds()
		{
			Vector3 vector = this._vcamTransform.position - this._origin;
			float magnitude = vector.magnitude;
			if (magnitude <= 0.001f)
			{
				return;
			}
			if (this.UseSoftBounds)
			{
				if (magnitude > this.MaxRadius * 0.8f)
				{
					float num = Mathf.InverseLerp(this.MaxRadius * 0.8f, this.MaxRadius, magnitude);
					Vector3 vector2 = -vector.normalized * num * this.SoftBoundStrength * Time.unscaledDeltaTime;
					this._vcamTransform.position += vector2;
				}
				if (magnitude > this.MaxRadius * 1.1f)
				{
					this._vcamTransform.position = this._origin + vector.normalized * this.MaxRadius;
					return;
				}
			}
			else if (magnitude > this.MaxRadius)
			{
				this._vcamTransform.position = this._origin + vector.normalized * this.MaxRadius;
			}
		}

		// Token: 0x060092CC RID: 37580 RVA: 0x003C3EF0 File Offset: 0x003C20F0
		private void OnDisable()
		{
			if (this._isActive)
			{
				this.Deactivate();
			}
		}

		// Token: 0x060092CD RID: 37581 RVA: 0x003C3F00 File Offset: 0x003C2100
		private void HideSceneUI()
		{
			this._hiddenCanvases.Clear();
			foreach (Canvas canvas in Object.FindObjectsOfType<Canvas>(false))
			{
				if (!ActionUiSuppressionScope.IsProtectedCanvas(canvas) && canvas.enabled && (!(this._photoMode != null) || !(canvas.GetComponentInParent<FlyCameraPhotoViewfinder>() != null)))
				{
					canvas.enabled = false;
					this._hiddenCanvases.Add(canvas);
				}
			}
		}

		// Token: 0x060092CE RID: 37582 RVA: 0x003C3F70 File Offset: 0x003C2170
		private void RestoreSceneUI()
		{
			foreach (Canvas canvas in this._hiddenCanvases)
			{
				if (canvas != null)
				{
					canvas.enabled = true;
				}
			}
			this._hiddenCanvases.Clear();
		}

		// Token: 0x0400974B RID: 38731
		[Header("Cinemachine")]
		[Tooltip("Dedicated virtual camera used for flying mode. Must have Body/Aim set to 'Do Nothing'.")]
		public CinemachineVirtualCamera VirtualCamera;

		// Token: 0x0400974C RID: 38732
		[Tooltip("Priority boost applied when flying mode is active")]
		public int ActivePriorityBoost = 100;

		// Token: 0x0400974D RID: 38733
		[Header("Movement")]
		[Tooltip("Base fly speed (units/sec)")]
		public float MoveSpeed = 5f;

		// Token: 0x0400974E RID: 38734
		[Tooltip("Speed multiplier when holding Shift")]
		public float FastMultiplier = 2.5f;

		// Token: 0x0400974F RID: 38735
		[Tooltip("Mouse look sensitivity")]
		public float LookSensitivity = 2f;

		// Token: 0x04009750 RID: 38736
		[Tooltip("Scroll-wheel movement speed range, matching the regular photo fly camera controls.")]
		public float MinSpeed = 0.25f;

		// Token: 0x04009751 RID: 38737
		public float MaxSpeed = 80f;

		// Token: 0x04009752 RID: 38738
		public float MoveSmoothTime = 0.12f;

		// Token: 0x04009753 RID: 38739
		public float LookSmoothing = 12f;

		// Token: 0x04009754 RID: 38740
		[Header("Bounds")]
		[Tooltip("Maximum distance from the origin the camera can travel")]
		public float MaxRadius = 15f;

		// Token: 0x04009755 RID: 38741
		[Tooltip("If true, softly pushes camera back when near the boundary instead of hard clamping")]
		public bool UseSoftBounds = true;

		// Token: 0x04009756 RID: 38742
		[Tooltip("How strongly the camera is pushed back near the boundary (higher = harder wall)")]
		public float SoftBoundStrength = 5f;

		// Token: 0x04009757 RID: 38743
		[Header("Camera UI Overlay")]
		[Tooltip("Legacy overlay root, kept hidden while the shared photo viewfinder is used.")]
		[FormerlySerializedAs("CameraOverlayUI")]
		public GameObject FlyModeUI;

		// Token: 0x04009758 RID: 38744
		[Header("Input")]
		[Tooltip("Key to exit flying camera mode")]
		public GenericInput ExitInput = new GenericInput("K", "Y", "Y");

		// Token: 0x04009759 RID: 38745
		private bool _isActive;

		// Token: 0x0400975A RID: 38746
		private Vector3 _origin;

		// Token: 0x0400975B RID: 38747
		private float _yaw;

		// Token: 0x0400975C RID: 38748
		private float _pitch;

		// Token: 0x0400975D RID: 38749
		private float _targetYaw;

		// Token: 0x0400975E RID: 38750
		private float _targetPitch;

		// Token: 0x0400975F RID: 38751
		private float _currentMoveSpeed;

		// Token: 0x04009760 RID: 38752
		private Vector3 _currentVelocity;

		// Token: 0x04009761 RID: 38753
		private Vector3 _velocityRef;

		// Token: 0x04009762 RID: 38754
		private Transform _vcamTransform;

		// Token: 0x04009763 RID: 38755
		private int _savedPriority;

		// Token: 0x04009764 RID: 38756
		private Transform _savedFollow;

		// Token: 0x04009765 RID: 38757
		private Transform _savedLookAt;

		// Token: 0x04009766 RID: 38758
		private float _nextControlLogTime;

		// Token: 0x04009767 RID: 38759
		private CinemachineBrain _cachedBrain;

		// Token: 0x04009768 RID: 38760
		private bool _loggedLiveCameraStatus;

		// Token: 0x04009769 RID: 38761
		private FlyCameraPhotoMode _photoMode;

		// Token: 0x0400976A RID: 38762
		private readonly List<Canvas> _hiddenCanvases = new List<Canvas>();

		// Token: 0x0400976B RID: 38763
		private Camera _photoOutput;

		// Token: 0x0400976C RID: 38764
		private float _savedOutputNearClip;

		// Token: 0x0400976D RID: 38765
		private float _savedVirtualNearClip;

		// Token: 0x0400976E RID: 38766
		private bool _savedOcclusionCulling;

		// Token: 0x0400976F RID: 38767
		private readonly List<vCullingFadeControl> _disabledCullingFades = new List<vCullingFadeControl>();
	}
}
