using System;
using System.Collections;
using Cinemachine;
using Features.Detection;
using ProjectExhibitionist.Core.SceneManagement;
using ProjectExhibitionist.FirstPerson;
using UnityEngine;

namespace Features.Stadium
{
	// Token: 0x02000C4C RID: 3148
	public class StadiumScreenController : MonoBehaviour
	{
		// Token: 0x14000098 RID: 152
		// (add) Token: 0x06005AD8 RID: 23256 RVA: 0x00239924 File Offset: 0x00237B24
		// (remove) Token: 0x06005AD9 RID: 23257 RVA: 0x0023995C File Offset: 0x00237B5C
		public event Action CollectionSwapped;

		// Token: 0x17000EEE RID: 3822
		// (get) Token: 0x06005ADA RID: 23258 RVA: 0x00239991 File Offset: 0x00237B91
		// (set) Token: 0x06005ADB RID: 23259 RVA: 0x00239999 File Offset: 0x00237B99
		public bool IsPlayerFocusActive { get; private set; }

		// Token: 0x17000EEF RID: 3823
		// (get) Token: 0x06005ADC RID: 23260 RVA: 0x002399A2 File Offset: 0x00237BA2
		public DetectionCameraController CycleController
		{
			get
			{
				return this.cycleController;
			}
		}

		// Token: 0x06005ADD RID: 23261 RVA: 0x002399AA File Offset: 0x00237BAA
		private void OnEnable()
		{
			this.RefreshHeadVisibleCamera(PlayerRegistry.Player);
		}

		// Token: 0x06005ADE RID: 23262 RVA: 0x002399B7 File Offset: 0x00237BB7
		private void Start()
		{
			this.SwitchToAmbient();
		}

		// Token: 0x06005ADF RID: 23263 RVA: 0x002399BF File Offset: 0x00237BBF
		private void OnDisable()
		{
			this.StopAmbientLoop();
			this.ReleaseHeadVisibleCamera();
		}

		// Token: 0x06005AE0 RID: 23264 RVA: 0x002399CD File Offset: 0x00237BCD
		private void LateUpdate()
		{
			this.RefreshHeadVisibleCamera((PlayerRegistry.Player != null) ? PlayerRegistry.Player : this._screenPlayer);
		}

		// Token: 0x06005AE1 RID: 23265 RVA: 0x002399F0 File Offset: 0x00237BF0
		private void RefreshHeadVisibleCamera(Transform player)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			Camera camera = ((this.cycleController != null) ? this.cycleController.OutputCameraOverride : null);
			if (this._screenPlayer == player && this._headVisibleCamera == camera)
			{
				return;
			}
			this.ReleaseHeadVisibleCamera();
			this._screenPlayer = player;
			this._headVisibleCamera = camera;
			if (player == null || camera == null)
			{
				return;
			}
			this._headVisibilityController = player.GetComponentInChildren<FirstPersonController>(true);
			if (this._headVisibilityController != null)
			{
				this._headVisibilityController.RegisterHeadVisibleCamera(camera);
			}
		}

		// Token: 0x06005AE2 RID: 23266 RVA: 0x00239A8E File Offset: 0x00237C8E
		private void ReleaseHeadVisibleCamera()
		{
			if (this._headVisibilityController != null)
			{
				this._headVisibilityController.UnregisterHeadVisibleCamera(this._headVisibleCamera);
			}
			this._headVisibilityController = null;
			this._headVisibleCamera = null;
			this._screenPlayer = null;
		}

		// Token: 0x06005AE3 RID: 23267 RVA: 0x00239AC4 File Offset: 0x00237CC4
		public void SwitchToAmbient()
		{
			if (this.cycleController != null)
			{
				this.cycleController.Deactivate();
			}
			this.IsPlayerFocusActive = false;
			this.ActivateAmbientVCam(0);
			this.StartAmbientLoop();
			Action collectionSwapped = this.CollectionSwapped;
			if (collectionSwapped != null)
			{
				collectionSwapped();
			}
			if (this.debugLogs)
			{
				Debug.Log("[StadiumScreenController] Switched to Ambient (pre-placed VCams)");
			}
		}

		// Token: 0x06005AE4 RID: 23268 RVA: 0x00239B24 File Offset: 0x00237D24
		public void SwitchToPlayerFocus(Transform player)
		{
			this.RefreshHeadVisibleCamera(player);
			if (this.cycleController == null || this.playerFocusPresets == null)
			{
				return;
			}
			this.StopAmbientLoop();
			this.ParkAllAmbientVCams();
			this.cycleController.Deactivate();
			this.cycleController.ActivateForTVPhase(this.playerFocusPresets, player, null);
			this.IsPlayerFocusActive = true;
			Action collectionSwapped = this.CollectionSwapped;
			if (collectionSwapped != null)
			{
				collectionSwapped();
			}
			if (this.debugLogs)
			{
				Debug.Log("[StadiumScreenController] Switched to PlayerFocus presets", player);
			}
		}

		// Token: 0x06005AE5 RID: 23269 RVA: 0x00239BAA File Offset: 0x00237DAA
		private void StartAmbientLoop()
		{
			this.StopAmbientLoop();
			if (this.ambientVCams == null || this.ambientVCams.Length <= 1)
			{
				return;
			}
			this._ambientLoop = base.StartCoroutine(this.AmbientRoutine());
		}

		// Token: 0x06005AE6 RID: 23270 RVA: 0x00239BD8 File Offset: 0x00237DD8
		private void StopAmbientLoop()
		{
			if (this._ambientLoop != null)
			{
				base.StopCoroutine(this._ambientLoop);
				this._ambientLoop = null;
			}
		}

		// Token: 0x06005AE7 RID: 23271 RVA: 0x00239BF5 File Offset: 0x00237DF5
		private IEnumerator AmbientRoutine()
		{
			for (;;)
			{
				float num = Random.Range(this.ambientIntervalMin, this.ambientIntervalMax);
				yield return new WaitForSeconds(num);
				if (this.IsPlayerFocusActive)
				{
					break;
				}
				if (this.ambientVCams == null || this.ambientVCams.Length <= 1)
				{
					goto IL_0069;
				}
				int num2 = (this._ambientIndex + 1) % this.ambientVCams.Length;
				this.ActivateAmbientVCam(num2);
				if (this.debugLogs)
				{
					Debug.Log(string.Format("[StadiumScreenController] Ambient cut → [{0}] '{1}'", num2, this.ambientVCams[num2].name));
				}
			}
			yield break;
			IL_0069:
			yield break;
			yield break;
		}

		// Token: 0x06005AE8 RID: 23272 RVA: 0x00239C04 File Offset: 0x00237E04
		private void ActivateAmbientVCam(int index)
		{
			if (this.ambientVCams == null || this.ambientVCams.Length == 0)
			{
				return;
			}
			if (index < 0 || index >= this.ambientVCams.Length)
			{
				return;
			}
			this._ambientIndex = index;
			for (int i = 0; i < this.ambientVCams.Length; i++)
			{
				CinemachineVirtualCameraBase cinemachineVirtualCameraBase = this.ambientVCams[i];
				if (!(cinemachineVirtualCameraBase == null))
				{
					cinemachineVirtualCameraBase.Priority = ((i == index) ? this.ambientActivePriority : this.ambientInactivePriority);
				}
			}
		}

		// Token: 0x06005AE9 RID: 23273 RVA: 0x00239C78 File Offset: 0x00237E78
		private void ParkAllAmbientVCams()
		{
			if (this.ambientVCams == null)
			{
				return;
			}
			for (int i = 0; i < this.ambientVCams.Length; i++)
			{
				if (this.ambientVCams[i] != null)
				{
					this.ambientVCams[i].Priority = this.ambientInactivePriority;
				}
			}
			this._ambientIndex = -1;
		}

		// Token: 0x04005C06 RID: 23558
		[Header("Player Focus (DetectionCameraController-driven)")]
		[Tooltip("Dedicated DetectionCameraController whose ManagedVCam renders into the stadium screen's RenderTexture. Used only for the Player Focus phase.")]
		[SerializeField]
		private DetectionCameraController cycleController;

		// Token: 0x04005C07 RID: 23559
		[SerializeField]
		private DetectionCameraPresetCollectionSO playerFocusPresets;

		// Token: 0x04005C08 RID: 23560
		[Header("Ambient (pre-placed VCams)")]
		[Tooltip("List of pre-placed VCams cycled while the player is outside the field. Place them in the scene framed exactly as you want, on the same layer as the cycle controller's VCam (StadiumScreenOnly). The list is rotated on a timer and the active one gets a priority boost (instant cut, no blend).")]
		[SerializeField]
		private CinemachineVirtualCameraBase[] ambientVCams;

		// Token: 0x04005C09 RID: 23561
		[Tooltip("Priority assigned to the ambient VCam that's currently 'live'.")]
		[SerializeField]
		private int ambientActivePriority = 50;

		// Token: 0x04005C0A RID: 23562
		[Tooltip("Priority assigned to ambient VCams that are not currently live.")]
		[SerializeField]
		private int ambientInactivePriority;

		// Token: 0x04005C0B RID: 23563
		[Tooltip("Min seconds between ambient cuts (randomized between min and max).")]
		[Min(0.5f)]
		[SerializeField]
		private float ambientIntervalMin = 5f;

		// Token: 0x04005C0C RID: 23564
		[Min(0.5f)]
		[SerializeField]
		private float ambientIntervalMax = 9f;

		// Token: 0x04005C0D RID: 23565
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04005C10 RID: 23568
		private Coroutine _ambientLoop;

		// Token: 0x04005C11 RID: 23569
		private int _ambientIndex = -1;

		// Token: 0x04005C12 RID: 23570
		private Transform _screenPlayer;

		// Token: 0x04005C13 RID: 23571
		private FirstPersonController _headVisibilityController;

		// Token: 0x04005C14 RID: 23572
		private Camera _headVisibleCamera;
	}
}
