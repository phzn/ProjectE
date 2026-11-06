using System;
using System.Collections;
using Cinemachine;
using Features.Detection;
using Features.Police;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;

namespace Features.Stadium
{
	// Token: 0x02000C4E RID: 3150
	[DefaultExecutionOrder(10000)]
	public class StadiumScreenZoomController : MonoBehaviour
	{
		// Token: 0x14000099 RID: 153
		// (add) Token: 0x06005AF1 RID: 23281 RVA: 0x00239DD0 File Offset: 0x00237FD0
		// (remove) Token: 0x06005AF2 RID: 23282 RVA: 0x00239E08 File Offset: 0x00238008
		public event Action ZoomEntered;

		// Token: 0x1400009A RID: 154
		// (add) Token: 0x06005AF3 RID: 23283 RVA: 0x00239E40 File Offset: 0x00238040
		// (remove) Token: 0x06005AF4 RID: 23284 RVA: 0x00239E78 File Offset: 0x00238078
		public event Action ZoomExited;

		// Token: 0x17000EF2 RID: 3826
		// (get) Token: 0x06005AF5 RID: 23285 RVA: 0x00239EAD File Offset: 0x002380AD
		public bool IsZoomActive
		{
			get
			{
				return this._zoomActive;
			}
		}

		// Token: 0x06005AF6 RID: 23286 RVA: 0x00239EB5 File Offset: 0x002380B5
		private void Awake()
		{
			if (this.mainCameraBrain == null && Camera.main != null)
			{
				this.mainCameraBrain = Camera.main.GetComponent<CinemachineBrain>();
			}
			this.ResolvePlayerInput();
			this.ForceNeutralState();
		}

		// Token: 0x06005AF7 RID: 23287 RVA: 0x00239EF0 File Offset: 0x002380F0
		private void ResolvePlayerInput()
		{
			if (PlayerRegistry.Player == null)
			{
				return;
			}
			if (this.playerInput == null)
			{
				this.playerInput = PlayerRegistry.Player.GetComponentInChildren<vThirdPersonInput>(true);
			}
			if (this._playerController == null)
			{
				this._playerController = PlayerRegistry.Player.GetComponentInChildren<vThirdPersonController>(true);
			}
			if (this.debugLogs)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[StadiumScreenZoom] Resolve → input=",
					(this.playerInput != null) ? this.playerInput.name : "NULL",
					", controller=",
					(this._playerController != null) ? this._playerController.name : "NULL",
					" (registry root: ",
					PlayerRegistry.Player.name,
					")"
				}), this);
			}
		}

		// Token: 0x06005AF8 RID: 23288 RVA: 0x00239FD8 File Offset: 0x002381D8
		private void OnEnable()
		{
			this.ResolvePlayerInput();
		}

		// Token: 0x06005AF9 RID: 23289 RVA: 0x00239FE0 File Offset: 0x002381E0
		private void OnDisable()
		{
			this.ForceNeutralState();
		}

		// Token: 0x06005AFA RID: 23290 RVA: 0x00239FE8 File Offset: 0x002381E8
		private void Update()
		{
			if (StadiumScreenZoomController.IsEncounterViewBlocked)
			{
				return;
			}
			if (this._zoomActive)
			{
				this.ReserveCrouchInput(true);
			}
			if (this._exiting)
			{
				return;
			}
			if (this.toggleInput != null && this.toggleInput.GetButtonDown())
			{
				this.ToggleZoom();
				return;
			}
			if (this._zoomActive && this.cycleInput != null && this.cycleInput.GetButtonDown() && this.screenCycleController != null)
			{
				this.screenCycleController.CycleToNextPreset();
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[StadiumScreenZoom] Cycle → preset {0}/{1}", this.screenCycleController.CurrentPresetIndex, this.screenCycleController.ActivePresetCount), this);
				}
			}
		}

		// Token: 0x06005AFB RID: 23291 RVA: 0x0023A0A0 File Offset: 0x002382A0
		private void LateUpdate()
		{
			if (!this._zoomActive || this._playerController == null)
			{
				return;
			}
			if (this._playerController.isCrouching)
			{
				this._playerController.isCrouching = false;
				if (this.debugLogs)
				{
					Debug.Log("[StadiumScreenZoom] LateUpdate cleared isCrouching", this);
				}
			}
			if (this._playerAnimator == null && this._playerController != null)
			{
				this._playerAnimator = this._playerController.GetComponentInChildren<Animator>(true);
			}
			if (this._playerAnimator != null && this._playerAnimator.isActiveAndEnabled)
			{
				this._playerAnimator.SetBool(StadiumScreenZoomController.IsCrouchingHash, false);
			}
		}

		// Token: 0x06005AFC RID: 23292 RVA: 0x0023A14A File Offset: 0x0023834A
		public void ToggleZoom()
		{
			if (this._exiting || StadiumScreenZoomController.IsEncounterViewBlocked)
			{
				return;
			}
			if (this._zoomActive)
			{
				this.BeginExit();
				return;
			}
			this.EnterZoom();
		}

		// Token: 0x17000EF3 RID: 3827
		// (get) Token: 0x06005AFD RID: 23293 RVA: 0x0023A174 File Offset: 0x00238374
		private static bool IsEncounterViewBlocked
		{
			get
			{
				return (ExhibitionEventDirector.Instance != null && ExhibitionEventDirector.Instance.IsEventActive) || (PlayerStunExecutor.Instance != null && (PlayerStunExecutor.Instance.IsStunned || PlayerStunExecutor.Instance.ExternalLockHold)) || (PerpWalkController.Instance != null && PerpWalkController.Instance.IsActive) || DetectionInteractionGate.IsGameOver || DetectionTransitionGate.IsBlocked || GameplayInputGate.BlocksGameplayInput || TextInputFocus.BlocksGameplayInput || PlayerActivityGate.IsResourceOwned(PlayerContextResource.Camera | PlayerContextResource.ModalUI);
			}
		}

		// Token: 0x06005AFE RID: 23294 RVA: 0x0023A200 File Offset: 0x00238400
		private void EnterZoom()
		{
			if (this._zoomActive || this._exiting || StadiumScreenZoomController.IsEncounterViewBlocked)
			{
				return;
			}
			this.ResolvePlayerInput();
			if (this.mainCameraBrain == null && Camera.main != null)
			{
				this.mainCameraBrain = Camera.main.GetComponent<CinemachineBrain>();
			}
			if (this.monitorVCam == null || this.mainCameraBrain == null || Camera.main == null)
			{
				Debug.LogWarning("[StadiumScreenZoom] EnterZoom aborted: required ref is null", this);
				return;
			}
			this._zoomActive = true;
			this._zoomGeneration++;
			Transform transform = Camera.main.transform;
			this.CaptureCameraState(Camera.main);
			this.EnsureAnchor();
			Transform parent = transform.parent;
			if (parent != null && this._anchorVCam.transform.parent != parent)
			{
				this._anchorVCam.transform.SetParent(parent, false);
			}
			this._anchorVCam.transform.localPosition = this._savedCameraLocalPos;
			this._anchorVCam.transform.localRotation = this._savedCameraLocalRot;
			this._anchorVCam.m_Lens.FieldOfView = Camera.main.fieldOfView;
			this._anchorVCam.Priority = this.anchorPriority;
			this._anchorVCam.gameObject.SetActive(true);
			this.mainCameraBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, this.blendDuration);
			this.mainCameraBrain.enabled = true;
			this.monitorVCam.Priority = this.inactivePriority;
			this.ReserveCrouchInput(true);
			if (this._enterRoutine != null)
			{
				base.StopCoroutine(this._enterRoutine);
			}
			this._enterRoutine = base.StartCoroutine(this.RaiseMonitorAfterFrame(this._zoomGeneration));
			Action zoomEntered = this.ZoomEntered;
			if (zoomEntered != null)
			{
				zoomEntered();
			}
			if (this.debugLogs)
			{
				Debug.Log("[StadiumScreenZoom] EnterZoom", this);
			}
		}

		// Token: 0x06005AFF RID: 23295 RVA: 0x0023A3E7 File Offset: 0x002385E7
		private IEnumerator RaiseMonitorAfterFrame(int generation)
		{
			yield return null;
			yield return new WaitForEndOfFrame();
			if (generation != this._zoomGeneration)
			{
				yield break;
			}
			if (this._zoomActive && this.monitorVCam != null)
			{
				this.monitorVCam.Priority = this.activePriority;
			}
			this._enterRoutine = null;
			yield break;
		}

		// Token: 0x06005B00 RID: 23296 RVA: 0x0023A400 File Offset: 0x00238600
		private void BeginExit()
		{
			if (this._exiting)
			{
				return;
			}
			this._exiting = true;
			this._zoomActive = false;
			Action zoomExited = this.ZoomExited;
			if (zoomExited != null)
			{
				zoomExited();
			}
			if (this.monitorVCam != null)
			{
				this.monitorVCam.Priority = this.inactivePriority;
			}
			this.ReserveCrouchInput(false);
			if (this._exitRoutine != null)
			{
				base.StopCoroutine(this._exitRoutine);
			}
			this._exitRoutine = base.StartCoroutine(this.ExitAfterBlend(this._zoomGeneration));
		}

		// Token: 0x06005B01 RID: 23297 RVA: 0x0023A487 File Offset: 0x00238687
		private IEnumerator ExitAfterBlend(int generation)
		{
			float num = Mathf.Max(0f, this.blendDuration);
			if (num > 0f)
			{
				yield return new WaitForSecondsRealtime(num);
			}
			if (generation != this._zoomGeneration)
			{
				yield break;
			}
			this.RestoreCameraState();
			if (this._anchorVCam != null)
			{
				this._anchorVCam.Priority = 0;
				this._anchorVCam.gameObject.SetActive(false);
			}
			this._exiting = false;
			this._exitRoutine = null;
			if (this.debugLogs)
			{
				Debug.Log("[StadiumScreenZoom] ExitZoom finalized", this);
			}
			yield break;
		}

		// Token: 0x06005B02 RID: 23298 RVA: 0x0023A4A0 File Offset: 0x002386A0
		private void EnsureAnchor()
		{
			if (this._anchorVCam != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("StadiumZoom_Anchor");
			if (this.monitorVCam != null)
			{
				gameObject.layer = this.monitorVCam.gameObject.layer;
			}
			this._anchorVCam = gameObject.AddComponent<CinemachineVirtualCamera>();
			this._anchorVCam.Priority = 0;
			gameObject.SetActive(false);
		}

		// Token: 0x06005B03 RID: 23299 RVA: 0x0023A50C File Offset: 0x0023870C
		private void ReserveCrouchInput(bool reserved)
		{
			if (reserved && (this.playerInput == null || this._playerController == null))
			{
				this.ResolvePlayerInput();
			}
			if (this.playerInput == null)
			{
				return;
			}
			if (reserved)
			{
				if (!this._crouchSaved && this.playerInput.crouchInput != null)
				{
					this._savedCrouchUseInput = this.playerInput.crouchInput.useInput;
					this._crouchSaved = true;
				}
				if (this.playerInput.crouchInput != null)
				{
					this.playerInput.crouchInput.useInput = false;
					return;
				}
			}
			else if (this._crouchSaved && this.playerInput.crouchInput != null)
			{
				this.playerInput.crouchInput.useInput = this._savedCrouchUseInput;
				this._crouchSaved = false;
			}
		}

		// Token: 0x06005B04 RID: 23300 RVA: 0x0023A5D4 File Offset: 0x002387D4
		private void ForceNeutralState()
		{
			bool zoomActive = this._zoomActive;
			this._zoomGeneration++;
			if (this._exitRoutine != null)
			{
				base.StopCoroutine(this._exitRoutine);
				this._exitRoutine = null;
			}
			if (this._enterRoutine != null)
			{
				base.StopCoroutine(this._enterRoutine);
				this._enterRoutine = null;
			}
			this._exiting = false;
			this._zoomActive = false;
			if (this.monitorVCam != null)
			{
				this.monitorVCam.Priority = this.inactivePriority;
			}
			if (this._anchorVCam != null)
			{
				this._anchorVCam.Priority = 0;
				this._anchorVCam.gameObject.SetActive(false);
			}
			this.RestoreCameraState();
			this.ReserveCrouchInput(false);
			if (zoomActive)
			{
				Action zoomExited = this.ZoomExited;
				if (zoomExited == null)
				{
					return;
				}
				zoomExited();
			}
		}

		// Token: 0x06005B05 RID: 23301 RVA: 0x00239FE0 File Offset: 0x002381E0
		public void InterruptForEncounter()
		{
			this.ForceNeutralState();
		}

		// Token: 0x06005B06 RID: 23302 RVA: 0x0023A6A4 File Offset: 0x002388A4
		private void CaptureCameraState(Camera camera)
		{
			this._savedCamera = camera;
			this._savedCameraLocalPos = camera.transform.localPosition;
			this._savedCameraLocalRot = camera.transform.localRotation;
			this._savedCameraFieldOfView = camera.fieldOfView;
			this._savedCameraNearClip = camera.nearClipPlane;
			this._savedCameraFarClip = camera.farClipPlane;
			this._savedBrainEnabled = this.mainCameraBrain.enabled;
			this._savedBrainBlend = this.mainCameraBrain.m_DefaultBlend;
			this._hasSavedTransform = true;
		}

		// Token: 0x06005B07 RID: 23303 RVA: 0x0023A728 File Offset: 0x00238928
		private void RestoreCameraState()
		{
			if (!this._hasSavedTransform)
			{
				return;
			}
			this._hasSavedTransform = false;
			if (this.mainCameraBrain != null)
			{
				this.mainCameraBrain.enabled = this._savedBrainEnabled;
				this.mainCameraBrain.m_DefaultBlend = this._savedBrainBlend;
			}
			if (this._savedCamera != null)
			{
				this._savedCamera.transform.localPosition = this._savedCameraLocalPos;
				this._savedCamera.transform.localRotation = this._savedCameraLocalRot;
				this._savedCamera.fieldOfView = this._savedCameraFieldOfView;
				this._savedCamera.nearClipPlane = this._savedCameraNearClip;
				this._savedCamera.farClipPlane = this._savedCameraFarClip;
				this._savedCamera = null;
			}
		}

		// Token: 0x04005C18 RID: 23576
		[Header("References")]
		[SerializeField]
		private CinemachineVirtualCameraBase monitorVCam;

		// Token: 0x04005C19 RID: 23577
		[Tooltip("Brain on the player's MainCamera. Toggled enabled while zoom is active.")]
		[SerializeField]
		private CinemachineBrain mainCameraBrain;

		// Token: 0x04005C1A RID: 23578
		[Tooltip("Cycle controller of the maxi screen — its CycleToNextPreset() is invoked when the player presses the cycle input while zoom is active.")]
		[SerializeField]
		private DetectionCameraController screenCycleController;

		// Token: 0x04005C1B RID: 23579
		[Tooltip("Player input — only crouch is reserved for the monitor's camera-cycle key.")]
		[SerializeField]
		private vThirdPersonInput playerInput;

		// Token: 0x04005C1C RID: 23580
		[Header("Input")]
		[SerializeField]
		private GenericInput toggleInput = new GenericInput("T", "RightBumper", "RightBumper");

		// Token: 0x04005C1D RID: 23581
		[SerializeField]
		private GenericInput cycleInput = new GenericInput("C", "DPadUp", "DPadUp");

		// Token: 0x04005C1E RID: 23582
		[Header("Priority")]
		[SerializeField]
		private int anchorPriority = 10;

		// Token: 0x04005C1F RID: 23583
		[SerializeField]
		private int activePriority = 30;

		// Token: 0x04005C20 RID: 23584
		[SerializeField]
		private int inactivePriority;

		// Token: 0x04005C21 RID: 23585
		[Header("Blend")]
		[Min(0f)]
		[SerializeField]
		private float blendDuration = 1f;

		// Token: 0x04005C22 RID: 23586
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04005C25 RID: 23589
		private bool _zoomActive;

		// Token: 0x04005C26 RID: 23590
		private bool _exiting;

		// Token: 0x04005C27 RID: 23591
		private CinemachineVirtualCamera _anchorVCam;

		// Token: 0x04005C28 RID: 23592
		private Vector3 _savedCameraLocalPos;

		// Token: 0x04005C29 RID: 23593
		private Quaternion _savedCameraLocalRot;

		// Token: 0x04005C2A RID: 23594
		private bool _hasSavedTransform;

		// Token: 0x04005C2B RID: 23595
		private Camera _savedCamera;

		// Token: 0x04005C2C RID: 23596
		private float _savedCameraFieldOfView;

		// Token: 0x04005C2D RID: 23597
		private float _savedCameraNearClip;

		// Token: 0x04005C2E RID: 23598
		private float _savedCameraFarClip;

		// Token: 0x04005C2F RID: 23599
		private bool _savedBrainEnabled;

		// Token: 0x04005C30 RID: 23600
		private CinemachineBlendDefinition _savedBrainBlend;

		// Token: 0x04005C31 RID: 23601
		private bool _crouchSaved;

		// Token: 0x04005C32 RID: 23602
		private bool _savedCrouchUseInput;

		// Token: 0x04005C33 RID: 23603
		private Coroutine _exitRoutine;

		// Token: 0x04005C34 RID: 23604
		private Coroutine _enterRoutine;

		// Token: 0x04005C35 RID: 23605
		private int _zoomGeneration;

		// Token: 0x04005C36 RID: 23606
		private vThirdPersonController _playerController;

		// Token: 0x04005C37 RID: 23607
		private Animator _playerAnimator;

		// Token: 0x04005C38 RID: 23608
		private static readonly int IsCrouchingHash = Animator.StringToHash("IsCrouching");
	}
}
