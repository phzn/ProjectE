using System;
using System.Collections;
using Features.CielOS.Apps.Story;
using Features.CielOS.VFS;
using Features.CielOS.WindowManager;
using Features.PhoneCompanion;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.CielOS
{
	// Token: 0x02001382 RID: 4994
	[DefaultExecutionOrder(-100)]
	public class CielOSController : MonoBehaviour
	{
		// Token: 0x170018A8 RID: 6312
		// (get) Token: 0x06009AA3 RID: 39587 RVA: 0x003F4145 File Offset: 0x003F2345
		// (set) Token: 0x06009AA4 RID: 39588 RVA: 0x003F414C File Offset: 0x003F234C
		public static CielOSController Instance { get; private set; }

		// Token: 0x170018A9 RID: 6313
		// (get) Token: 0x06009AA5 RID: 39589 RVA: 0x003F4154 File Offset: 0x003F2354
		// (set) Token: 0x06009AA6 RID: 39590 RVA: 0x003F415C File Offset: 0x003F235C
		public bool IsOpen { get; private set; }

		// Token: 0x170018AA RID: 6314
		// (get) Token: 0x06009AA7 RID: 39591 RVA: 0x003F4165 File Offset: 0x003F2365
		public static bool AnyOpen
		{
			get
			{
				return CielOSController.Instance != null && CielOSController.Instance.IsOpen;
			}
		}

		// Token: 0x170018AB RID: 6315
		// (get) Token: 0x06009AA8 RID: 39592 RVA: 0x003F4180 File Offset: 0x003F2380
		public bool CanOpen
		{
			get
			{
				return !this._closing && (this.IsOpen || (PlayerActivityGate.Allows(PlayerActivity.Interact) && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.ModalUI | PlayerContextResource.HeldItem)));
			}
		}

		// Token: 0x06009AA9 RID: 39593 RVA: 0x003F41AC File Offset: 0x003F23AC
		private void Awake()
		{
			if (CielOSController.Instance != null && CielOSController.Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			CielOSController.Instance = this;
			if (this.theme != null)
			{
				CielOSTheme.Active = this.theme;
			}
			CielOSSfx.Configure(this.uiClickClip, this.uiClickVolume);
			if (this.canvasGroup == null)
			{
				this.canvasGroup = base.GetComponent<CanvasGroup>();
			}
			CielOSPhotosSync.Ensure();
			this.ApplyVisibility(false, true);
			if (Application.isPlaying && base.GetComponent<CielOSStoryFlow>() == null)
			{
				base.gameObject.AddComponent<CielOSStoryFlow>();
			}
			if (Application.isPlaying && base.GetComponent<CielOSAppUnlockNotice>() == null)
			{
				base.gameObject.AddComponent<CielOSAppUnlockNotice>();
			}
		}

		// Token: 0x06009AAA RID: 39594 RVA: 0x003F4279 File Offset: 0x003F2479
		private void OnDestroy()
		{
			this.CloseInternal(true);
			if (CielOSController.Instance == this)
			{
				CielOSController.Instance = null;
			}
		}

		// Token: 0x06009AAB RID: 39595 RVA: 0x003F4295 File Offset: 0x003F2495
		private void OnDisable()
		{
			this.CloseInternal(true);
		}

		// Token: 0x06009AAC RID: 39596 RVA: 0x003F429E File Offset: 0x003F249E
		private void Update()
		{
			if (!this.IsOpen && this._fadeCo == null)
			{
				return;
			}
			if (PhotoPolaroidViewer.IsOpen)
			{
				return;
			}
			if (EscapeInputCoordinator.TryConsume(this.closeKey, this))
			{
				this.Close();
			}
		}

		// Token: 0x06009AAD RID: 39597 RVA: 0x003F42CD File Offset: 0x003F24CD
		public void Open()
		{
			this.TryOpen();
		}

		// Token: 0x06009AAE RID: 39598 RVA: 0x003F42D8 File Offset: 0x003F24D8
		public bool TryOpen()
		{
			if (this._closing)
			{
				return false;
			}
			if (this.IsOpen)
			{
				return true;
			}
			if (!this.TryAcquireDesktopContext())
			{
				return false;
			}
			if (!base.isActiveAndEnabled || this._desktopLease == null || !this._desktopLease.IsActive)
			{
				this.ReleaseDesktopContext();
				return false;
			}
			bool flag;
			try
			{
				this.IsOpen = true;
				if (this.desktopRoot != null)
				{
					this.desktopRoot.SetActive(true);
				}
				CielOSWallpaperService instance = CielOSWallpaperService.Instance;
				if (instance != null)
				{
					instance.RestoreWallpaper();
				}
				this.SetGameplayLocked(true);
				this.ApplyVisibility(true, false);
				CielOSAppUnlockNotice component = base.GetComponent<CielOSAppUnlockNotice>();
				if (component != null)
				{
					component.OnPcOpened();
				}
				CielOSStoryFlow component2 = base.GetComponent<CielOSStoryFlow>();
				if (component2 != null)
				{
					component2.RefreshGuidance();
				}
				flag = true;
			}
			catch
			{
				this.CloseInternal(true);
				throw;
			}
			return flag;
		}

		// Token: 0x06009AAF RID: 39599 RVA: 0x003F43AC File Offset: 0x003F25AC
		public void Close()
		{
			this.CloseInternal(false);
		}

		// Token: 0x06009AB0 RID: 39600 RVA: 0x003F43B8 File Offset: 0x003F25B8
		private void CloseInternal(bool instant)
		{
			if (this._closing)
			{
				if (instant)
				{
					this.ApplyVisibility(false, true);
				}
				return;
			}
			bool isOpen = this.IsOpen;
			if (isOpen && !instant)
			{
				if (this.windowManager == null)
				{
					this.windowManager = Object.FindObjectOfType<CielOSWindowManager>(true);
				}
				if (this.windowManager != null && !this.windowManager.CanCloseAll())
				{
					return;
				}
			}
			if (!isOpen && !this._gameplayStateCaptured && this._desktopLease == null)
			{
				if (instant)
				{
					this.ApplyVisibility(false, true);
				}
				return;
			}
			this._closing = true;
			this.IsOpen = false;
			try
			{
				CielOSStoryFlow component = base.GetComponent<CielOSStoryFlow>();
				if (component != null)
				{
					component.HideGuidance();
				}
				CielOSAppUnlockNotice component2 = base.GetComponent<CielOSAppUnlockNotice>();
				if (component2 != null)
				{
					component2.Hide();
				}
				if (isOpen && this.windowManager == null)
				{
					this.windowManager = Object.FindObjectOfType<CielOSWindowManager>(true);
				}
				if (isOpen && this.windowManager != null)
				{
					this.windowManager.CloseAll();
				}
			}
			finally
			{
				try
				{
					CielOSSession.Clear();
					this.SetGameplayLocked(false);
					this.ApplyVisibility(false, instant);
				}
				finally
				{
					try
					{
						this.ReleaseDesktopContext();
					}
					finally
					{
						this._closing = false;
					}
				}
			}
		}

		// Token: 0x06009AB1 RID: 39601 RVA: 0x003F44F8 File Offset: 0x003F26F8
		public void Toggle()
		{
			if (this.IsOpen)
			{
				this.Close();
				return;
			}
			this.Open();
		}

		// Token: 0x06009AB2 RID: 39602 RVA: 0x003F4510 File Offset: 0x003F2710
		private void ApplyVisibility(bool visible, bool instant)
		{
			if (this._fadeCo != null)
			{
				base.StopCoroutine(this._fadeCo);
				this._fadeCo = null;
			}
			if (this.canvasGroup == null)
			{
				return;
			}
			this.canvasGroup.interactable = visible;
			this.canvasGroup.blocksRaycasts = visible;
			if (instant || this.fadeDuration <= 0f || !base.isActiveAndEnabled)
			{
				this.canvasGroup.alpha = (visible ? 1f : 0f);
				return;
			}
			this._fadeCo = base.StartCoroutine(this.FadeRoutine(visible ? 1f : 0f));
		}

		// Token: 0x06009AB3 RID: 39603 RVA: 0x003F45B3 File Offset: 0x003F27B3
		private IEnumerator FadeRoutine(float target)
		{
			float start = this.canvasGroup.alpha;
			float t = 0f;
			while (this.canvasGroup != null && t < this.fadeDuration)
			{
				t += Time.unscaledDeltaTime;
				this.canvasGroup.alpha = Mathf.Lerp(start, target, t / this.fadeDuration);
				yield return null;
			}
			if (this.canvasGroup != null)
			{
				this.canvasGroup.alpha = target;
			}
			this._fadeCo = null;
			yield break;
		}

		// Token: 0x06009AB4 RID: 39604 RVA: 0x003F45CC File Offset: 0x003F27CC
		private void SetGameplayLocked(bool locked)
		{
			if (!this.lockPlayerInput)
			{
				return;
			}
			if (this._playerInput == null)
			{
				this._playerInput = ((this.player != null) ? this.player.GetComponentInChildren<vThirdPersonInput>(true) : Object.FindObjectOfType<vThirdPersonInput>());
			}
			if (locked)
			{
				if (this._gameplayStateCaptured)
				{
					return;
				}
				if (this._playerInput != null)
				{
					this._prevLockBasic = this._playerInput.lockInput;
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
				this._gameplayStateCaptured = true;
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
				return;
			}
			else
			{
				if (!this._gameplayStateCaptured)
				{
					return;
				}
				if (this._playerInput != null)
				{
					this._playerInput.SetLockAllInput(this._prevLockBasic);
					this._playerInput.SetLockCameraInput(this._prevLockCamera);
				}
				Cursor.lockState = this._prevCursorLock;
				Cursor.visible = this._prevCursorVisible;
				this._gameplayStateCaptured = false;
				return;
			}
		}

		// Token: 0x06009AB5 RID: 39605 RVA: 0x003F472C File Offset: 0x003F292C
		private bool TryAcquireDesktopContext()
		{
			if (this._desktopLease != null)
			{
				if (this._desktopLease.IsActive)
				{
					return true;
				}
				this._desktopLease = null;
			}
			return PlayerActivityGate.TryAcquire("CielOS#" + base.GetInstanceID().ToString(), this, PlayerActivity.Interact, PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.ModalUI | PlayerContextResource.HeldItem, PlayerActivity.AllPlayerActions, out this._desktopLease, false, null);
		}

		// Token: 0x06009AB6 RID: 39606 RVA: 0x003F4786 File Offset: 0x003F2986
		private void ReleaseDesktopContext()
		{
			PlayerActivityLease desktopLease = this._desktopLease;
			this._desktopLease = null;
			if (desktopLease == null)
			{
				return;
			}
			desktopLease.Dispose();
		}

		// Token: 0x04009F77 RID: 40823
		[Header("Visuals")]
		[Tooltip("CanvasGroup faded in/out for the whole desktop. If null, found on this object.")]
		[SerializeField]
		private CanvasGroup canvasGroup;

		// Token: 0x04009F78 RID: 40824
		[Tooltip("Seconds for the open/close fade.")]
		[SerializeField]
		private float fadeDuration = 0.25f;

		// Token: 0x04009F79 RID: 40825
		[Tooltip("Theme: published to CielOSTheme.Active so runtime-built UI picks up rounded sprites + palette.")]
		[SerializeField]
		private CielOSTheme theme;

		// Token: 0x04009F7A RID: 40826
		[Header("Audio")]
		[Tooltip("Clip riprodotta al click sui pulsanti dell'OS (dock, menu app, taskbar, finestre, power).")]
		[SerializeField]
		private AudioClip uiClickClip;

		// Token: 0x04009F7B RID: 40827
		[Range(0f, 1f)]
		[SerializeField]
		private float uiClickVolume = 1f;

		// Token: 0x04009F7C RID: 40828
		[Header("Desktop")]
		[Tooltip("Root of the desktop (wallpaper + panel + window layer). Enabled while the OS is open. Optional.")]
		[SerializeField]
		private GameObject desktopRoot;

		// Token: 0x04009F7D RID: 40829
		[Tooltip("Window manager: alla chiusura dell'OS (anche via ESC) tutte le finestre aperte vengono chiuse, così le app (es. Customization) fanno il loro teardown. Se null, cercato in scena.")]
		[SerializeField]
		private CielOSWindowManager windowManager;

		// Token: 0x04009F7E RID: 40830
		[Header("Input Gating")]
		[Tooltip("Player root used to find the Invector input component. If null, searched in the scene.")]
		[SerializeField]
		private GameObject player;

		// Token: 0x04009F7F RID: 40831
		[Tooltip("Lock Invector movement + camera while the OS is open.")]
		[SerializeField]
		private bool lockPlayerInput = true;

		// Token: 0x04009F80 RID: 40832
		[Tooltip("Key that closes the OS / backs out of an app.")]
		[SerializeField]
		private KeyCode closeKey = KeyCode.Escape;

		// Token: 0x04009F82 RID: 40834
		public const string GateReason = "CielOS";

		// Token: 0x04009F83 RID: 40835
		private const PlayerActivity RequiredActivity = PlayerActivity.Interact;

		// Token: 0x04009F84 RID: 40836
		private const PlayerContextResource DesktopResources = PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.ModalUI | PlayerContextResource.HeldItem;

		// Token: 0x04009F85 RID: 40837
		private vThirdPersonInput _playerInput;

		// Token: 0x04009F86 RID: 40838
		private bool _prevLockBasic;

		// Token: 0x04009F87 RID: 40839
		private bool _prevLockCamera;

		// Token: 0x04009F88 RID: 40840
		private CursorLockMode _prevCursorLock;

		// Token: 0x04009F89 RID: 40841
		private bool _prevCursorVisible;

		// Token: 0x04009F8A RID: 40842
		private bool _gameplayStateCaptured;

		// Token: 0x04009F8B RID: 40843
		private bool _closing;

		// Token: 0x04009F8C RID: 40844
		private Coroutine _fadeCo;

		// Token: 0x04009F8D RID: 40845
		private PlayerActivityLease _desktopLease;
	}
}
