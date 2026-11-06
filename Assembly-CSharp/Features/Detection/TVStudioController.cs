using System;
using System.Collections;
using Cinemachine;
using Features.DebugTools;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x02001302 RID: 4866
	public class TVStudioController : MonoBehaviour, IEncounterPreReactionPhase, IEncounterPreReactionCancellation, IEncounterMode
	{
		// Token: 0x1700181C RID: 6172
		// (get) Token: 0x060096FB RID: 38651 RVA: 0x003DF17A File Offset: 0x003DD37A
		public GenericInput EnterInput
		{
			get
			{
				return this.enterInput;
			}
		}

		// Token: 0x1700181D RID: 6173
		// (get) Token: 0x060096FC RID: 38652 RVA: 0x003DF182 File Offset: 0x003DD382
		public string UIHintText
		{
			get
			{
				return this.uiHintText;
			}
		}

		// Token: 0x1700181E RID: 6174
		// (get) Token: 0x060096FD RID: 38653 RVA: 0x003DF18A File Offset: 0x003DD38A
		public GameObject HintUI
		{
			get
			{
				return this.mainFlowHintUI;
			}
		}

		// Token: 0x1700181F RID: 6175
		// (get) Token: 0x060096FE RID: 38654 RVA: 0x003DF192 File Offset: 0x003DD392
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
		}

		// Token: 0x060096FF RID: 38655 RVA: 0x003DF19C File Offset: 0x003DD39C
		private void Awake()
		{
			ControllerContextBindings.Set(this.enterInput, "X");
			ControllerContextBindings.Set(this.tvCamInput, "RB");
			ControllerKeyHint.BindMatching(this.mainFlowHintUI, this.enterInput);
			ControllerKeyHint.BindMatching(this.studioHintUI, this.tvCamInput);
			GenericInput genericInput = new GenericInput(this.enterInput.keyboard, "B", this.enterInput.mobile)
			{
				preserveJoystickBinding = true
			};
			ControllerKeyHint.BindMatching(this.studioHintUI, genericInput);
			if (this.studioViewVCam != null)
			{
				this.studioViewVCam.Priority = this.studioVCamInactivePriority;
			}
			if (this.studioWideVCam != null)
			{
				this.studioWideVCam.Priority = this.studioVCamInactivePriority;
			}
			if (this.tvMonitorVCam != null)
			{
				this.tvMonitorVCam.Priority = this.studioVCamInactivePriority;
			}
			if (this.studioHintUI != null)
			{
				this.studioHintUI.SetActive(false);
			}
			if (this.forcePlayerScreenSkyboxClear && this.playerScreenCamera != null)
			{
				this.playerScreenCamera.clearFlags = CameraClearFlags.Skybox;
			}
			if (this.playerScreenCamera != null)
			{
				this.playerScreenCamera.enabled = false;
			}
			if (this.anchorTransform != null)
			{
				this._anchorInitialRotation = this.anchorTransform.rotation;
				this._hasAnchorInitialRotation = true;
			}
			this.CaptureAnchorInitialState();
		}

		// Token: 0x06009700 RID: 38656 RVA: 0x003DF2FE File Offset: 0x003DD4FE
		private void Start()
		{
			this.CaptureAnchorInitialState();
		}

		// Token: 0x06009701 RID: 38657 RVA: 0x003DF306 File Offset: 0x003DD506
		private void OnDisable()
		{
			this.CancelPreReaction();
		}

		// Token: 0x06009702 RID: 38658 RVA: 0x003DF310 File Offset: 0x003DD510
		private void CapturePresentationState()
		{
			this._presentationStateCaptured = true;
			this._studioRootWasActive = this.studioRoot != null && this.studioRoot.activeSelf;
			this._presentationBrain = ((Camera.main != null) ? Camera.main.GetComponent<CinemachineBrain>() : null);
			if (this._presentationBrain != null)
			{
				this._presentationBlend = this._presentationBrain.m_DefaultBlend;
			}
			this._presentationTvBrain = ((this.playerScreenCamera != null) ? this.playerScreenCamera.GetComponent<CinemachineBrain>() : null);
			if (this._presentationTvBrain != null)
			{
				this._presentationTvBlend = this._presentationTvBrain.m_DefaultBlend;
			}
		}

		// Token: 0x06009703 RID: 38659 RVA: 0x003DF3C6 File Offset: 0x003DD5C6
		private void ReleasePresentationState()
		{
			this._presentationStateCaptured = false;
			this._presentationBrain = null;
			this._presentationTvBrain = null;
			this._presenterDialogueManager = null;
			this._presenterDialogueToken = null;
		}

		// Token: 0x06009704 RID: 38660 RVA: 0x003DF3EC File Offset: 0x003DD5EC
		public void CancelPreReaction()
		{
			if (!this._presentationStateCaptured)
			{
				return;
			}
			base.StopAllCoroutines();
			this._bgAudioCoroutine = null;
			if (this._presenterDialogueManager != null)
			{
				this._presenterDialogueManager.TryEndDialogue(this._presenterDialogueToken, false);
			}
			if (this.newsIntro != null)
			{
				this.newsIntro.CancelPlayback();
				this.newsIntro.gameObject.SetActive(false);
			}
			this.ResetStudioState();
			if (this.playerScreenCamera != null)
			{
				this.playerScreenCamera.enabled = false;
			}
			if (this.studioHintUI != null)
			{
				this.studioHintUI.SetActive(false);
			}
			if (this.mainFlowHintUI != null)
			{
				this.mainFlowHintUI.SetActive(false);
			}
			if (this._presentationBrain != null)
			{
				this._presentationBrain.m_DefaultBlend = this._presentationBlend;
			}
			if (this._presentationTvBrain != null)
			{
				this._presentationTvBrain.m_DefaultBlend = this._presentationTvBlend;
			}
			DetectionAudioManager instance = DetectionAudioManager.Instance;
			if (instance != null)
			{
				instance.RestoreAmbientVolume(0f);
			}
			DetectionAudioManager instance2 = DetectionAudioManager.Instance;
			if (instance2 != null)
			{
				instance2.RestoreHeartbeatVolume(0f);
			}
			this._isActive = false;
			bool studioRootWasActive = this._studioRootWasActive;
			this.ReleasePresentationState();
			if (this.studioRoot != null)
			{
				this.studioRoot.SetActive(studioRootWasActive);
			}
		}

		// Token: 0x06009705 RID: 38661 RVA: 0x003DF544 File Offset: 0x003DD744
		private void CaptureAnchorInitialState()
		{
			if (this._hasAnchorInitialState)
			{
				return;
			}
			if (this.anchorAnimator == null || !this.anchorAnimator.isInitialized)
			{
				return;
			}
			AnimatorStateInfo currentAnimatorStateInfo = this.anchorAnimator.GetCurrentAnimatorStateInfo(0);
			this._anchorInitialStateHash = ((currentAnimatorStateInfo.shortNameHash != 0) ? currentAnimatorStateInfo.shortNameHash : currentAnimatorStateInfo.fullPathHash);
			this._hasAnchorInitialState = this._anchorInitialStateHash != 0;
		}

		// Token: 0x06009706 RID: 38662 RVA: 0x003DF5B4 File Offset: 0x003DD7B4
		private void ResetStudioState()
		{
			if (this.anchorTransform != null && this._hasAnchorInitialRotation)
			{
				this.anchorTransform.rotation = this._anchorInitialRotation;
			}
			if (this.anchorAnimator != null && this._hasAnchorInitialState)
			{
				this.anchorAnimator.Play(this._anchorInitialStateHash, 0, 0f);
			}
			if (this.anchorMicProp != null)
			{
				this.anchorMicProp.SetActive(false);
			}
			if (this._bgAudioCoroutine != null)
			{
				base.StopCoroutine(this._bgAudioCoroutine);
				this._bgAudioCoroutine = null;
			}
			if (this.backgroundAudioSource != null && this.backgroundAudioSource.isPlaying)
			{
				this.backgroundAudioSource.Stop();
			}
			if (this.studioWideVCam != null)
			{
				this.studioWideVCam.Priority = this.studioVCamInactivePriority;
				this.studioWideVCam.gameObject.SetActive(false);
			}
			if (this.studioViewVCam != null)
			{
				this.studioViewVCam.Priority = this.studioVCamInactivePriority;
				this.studioViewVCam.gameObject.SetActive(false);
			}
			if (this.tvMonitorVCam != null)
			{
				this.tvMonitorVCam.Priority = this.studioVCamInactivePriority;
				this.tvMonitorVCam.gameObject.SetActive(false);
			}
			if (this.tvCameraCycler != null && this.tvCameraCycler.IsActive)
			{
				this.tvCameraCycler.Deactivate();
			}
		}

		// Token: 0x06009707 RID: 38663 RVA: 0x003DF728 File Offset: 0x003DD928
		private void ActivateTvCameraCycler(Transform npcTransform = null)
		{
			if (this.tvCameraCycler == null || this.tvCameraPresets == null || this.tvCameraPresets.Count == 0)
			{
				return;
			}
			if (this.tvCameraCycler.IsActive)
			{
				return;
			}
			vThirdPersonInput vThirdPersonInput = Object.FindObjectOfType<vThirdPersonInput>();
			Transform transform = ((vThirdPersonInput != null) ? vThirdPersonInput.transform : null);
			this.tvCameraCycler.ActivateForTVPhase(this.tvCameraPresets, transform, npcTransform);
		}

		// Token: 0x06009708 RID: 38664 RVA: 0x003DF799 File Offset: 0x003DD999
		private IEnumerator ActivateTvCameraCyclerWithCut(Transform npcTransform = null)
		{
			CinemachineBrain tvBrain = ((this.playerScreenCamera != null) ? this.playerScreenCamera.GetComponent<CinemachineBrain>() : null);
			CinemachineBlendDefinition savedTvBlend = default(CinemachineBlendDefinition);
			bool hasSaved = false;
			if (tvBrain != null)
			{
				savedTvBlend = tvBrain.m_DefaultBlend;
				hasSaved = true;
				tvBrain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
			}
			this.ActivateTvCameraCycler(npcTransform);
			yield return null;
			if (hasSaved)
			{
				tvBrain.m_DefaultBlend = savedTvBlend;
			}
			yield break;
		}

		// Token: 0x06009709 RID: 38665 RVA: 0x003DF7AF File Offset: 0x003DD9AF
		private void DeactivateTvCameraCycler()
		{
			if (this.tvCameraCycler != null && this.tvCameraCycler.IsActive)
			{
				this.tvCameraCycler.Deactivate();
			}
		}

		// Token: 0x0600970A RID: 38666 RVA: 0x003DF7D7 File Offset: 0x003DD9D7
		IEnumerator IEncounterPreReactionPhase.Execute(ExhibitionEventDirector director)
		{
			this.ResetStudioState();
			this.CapturePresentationState();
			CinemachineBrain brain = ((Camera.main != null) ? Camera.main.GetComponent<CinemachineBrain>() : null);
			CinemachineBlendDefinition savedBlend = default(CinemachineBlendDefinition);
			bool hasSavedBlend = false;
			if (brain != null)
			{
				savedBlend = brain.m_DefaultBlend;
				hasSavedBlend = true;
				brain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
			}
			DetectionAudioManager instance = DetectionAudioManager.Instance;
			if (instance != null)
			{
				instance.FadeAmbientTo(0f, this.musicMuteDuration);
			}
			DetectionAudioManager instance2 = DetectionAudioManager.Instance;
			if (instance2 != null)
			{
				instance2.FadeHeartbeatTo(0f, this.musicMuteDuration);
			}
			Coroutine introPrepareTask = null;
			if (this.newsIntro != null)
			{
				this.newsIntro.gameObject.SetActive(true);
				introPrepareTask = base.StartCoroutine(this.newsIntro.PrepareVideoAsync());
			}
			yield return base.StartCoroutine(this.DoFade(false, this.fadeDuration));
			if (introPrepareTask != null)
			{
				yield return introPrepareTask;
			}
			if (this.newsIntro != null)
			{
				yield return base.StartCoroutine(this.newsIntro.PlayPrepared(false));
			}
			if (this.studioRoot != null)
			{
				this.studioRoot.SetActive(true);
			}
			if (this.playerScreenCamera != null)
			{
				this.playerScreenCamera.enabled = true;
			}
			yield return base.StartCoroutine(this.ActivateStudioCamerasWithCut());
			yield return base.StartCoroutine(this.ActivateTvCameraCyclerWithCut(director.ActiveInitiatorTransform));
			if (this.backgroundAudioSource != null && this.backgroundAudioClip != null)
			{
				this.backgroundAudioSource.clip = this.backgroundAudioClip;
				this.backgroundAudioSource.loop = true;
				this.backgroundAudioSource.spatialBlend = 0f;
				this.backgroundAudioSource.volume = 0f;
				this.backgroundAudioSource.Play();
				if (this._bgAudioCoroutine != null)
				{
					base.StopCoroutine(this._bgAudioCoroutine);
				}
				this._bgAudioCoroutine = base.StartCoroutine(this.FadeAudio(this.backgroundAudioSource, this.backgroundVolume, this.fadeDuration));
			}
			if (this.newsIntro != null)
			{
				base.StartCoroutine(this.newsIntro.FadeOutOverlay());
			}
			yield return base.StartCoroutine(this.DoFade(true, this.fadeDuration));
			if (this.newsIntro != null)
			{
				this.newsIntro.gameObject.SetActive(false);
			}
			if (this.studioWideVCam != null)
			{
				yield return base.StartCoroutine(this.ZoomInToAnchor());
			}
			if (this.anchorMicProp != null)
			{
				this.anchorMicProp.SetActive(true);
			}
			if (this.anchorAnimator != null && !string.IsNullOrEmpty(this.talkingStateName))
			{
				this.anchorAnimator.Play(this.talkingStateName);
			}
			if (this.presenterLines != null && this.presenterLines.Length != 0 && SimpleDialogueManager.Instance != null)
			{
				TVStudioController.<>c__DisplayClass69_0 CS$<>8__locals1 = new TVStudioController.<>c__DisplayClass69_0();
				CS$<>8__locals1.dialogueDone = false;
				this._presenterDialogueManager = SimpleDialogueManager.Instance;
				this._presenterDialogueToken = new object();
				this._presenterDialogueManager.StartOwnedDialogue(this._presenterDialogueToken, DialogueLine.CloneForPresentation(this.presenterLines, "dialogue", null), delegate
				{
					CS$<>8__locals1.dialogueDone = true;
				});
				while (!CS$<>8__locals1.dialogueDone)
				{
					yield return null;
				}
				CS$<>8__locals1 = null;
			}
			if (this.postDialogueDelay > 0f)
			{
				yield return new WaitForSeconds(this.postDialogueDelay);
			}
			yield return base.StartCoroutine(this.AnchorPostDialogue());
			if (this.backgroundAudioSource != null && this.backgroundAudioSource.isPlaying)
			{
				if (this._bgAudioCoroutine != null)
				{
					base.StopCoroutine(this._bgAudioCoroutine);
				}
				this._bgAudioCoroutine = base.StartCoroutine(this.FadeAudio(this.backgroundAudioSource, 0f, this.fadeDuration));
			}
			yield return base.StartCoroutine(this.DoFade(false, this.fadeDuration));
			yield return null;
			this.DeactivateTvCameraCycler();
			if (this.playerScreenCamera != null)
			{
				this.playerScreenCamera.enabled = false;
			}
			yield return base.StartCoroutine(this.CutToVCam(this.studioVCamInactivePriority));
			if (this.studioWideVCam != null)
			{
				this.studioWideVCam.Priority = this.studioVCamInactivePriority;
			}
			DetectionAudioManager instance3 = DetectionAudioManager.Instance;
			if (instance3 != null)
			{
				instance3.RestoreAmbientVolume(this.musicRestoreDuration);
			}
			DetectionAudioManager instance4 = DetectionAudioManager.Instance;
			if (instance4 != null)
			{
				instance4.RestoreHeartbeatVolume(this.musicRestoreDuration);
			}
			if (hasSavedBlend)
			{
				brain.m_DefaultBlend = savedBlend;
			}
			this.ReleasePresentationState();
			yield break;
		}

		// Token: 0x0600970B RID: 38667 RVA: 0x003DF7ED File Offset: 0x003DD9ED
		IEnumerator IEncounterMode.Execute(ExhibitionEventDirector director)
		{
			this.CapturePresentationState();
			this._isActive = true;
			CinemachineBrain brain = ((Camera.main != null) ? Camera.main.GetComponent<CinemachineBrain>() : null);
			CinemachineBlendDefinition savedEnterBlend = default(CinemachineBlendDefinition);
			if (brain != null)
			{
				savedEnterBlend = brain.m_DefaultBlend;
				brain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
			}
			DetectionAudioManager instance = DetectionAudioManager.Instance;
			if (instance != null)
			{
				instance.FadeAmbientTo(0f, this.postEmoteFadeDuration);
			}
			DetectionAudioManager instance2 = DetectionAudioManager.Instance;
			if (instance2 != null)
			{
				instance2.FadeHeartbeatTo(0f, this.postEmoteFadeDuration);
			}
			yield return base.StartCoroutine(this.DoFade(false, this.postEmoteFadeDuration));
			yield return null;
			if (this.studioWideVCam != null)
			{
				this.studioWideVCam.Priority = this.studioVCamInactivePriority;
			}
			if (this.tvMonitorVCam != null)
			{
				this.tvMonitorVCam.Priority = this.studioVCamInactivePriority;
			}
			director.RestoreEncounterNPCLayers();
			if (this.studioRoot != null && !this.studioRoot.activeSelf)
			{
				this.studioRoot.SetActive(true);
			}
			if (this.studioViewVCam != null)
			{
				this.studioViewVCam.gameObject.SetActive(true);
				this.studioViewVCam.Priority = this.studioVCamActivePriority + 5;
			}
			if (this.tvMonitorVCam != null && !this.tvMonitorVCam.gameObject.activeSelf)
			{
				this.tvMonitorVCam.gameObject.SetActive(true);
			}
			if (this.playerScreenCamera != null)
			{
				this.playerScreenCamera.enabled = true;
			}
			yield return base.StartCoroutine(this.ActivateTvCameraCyclerWithCut(director.ActiveInitiatorTransform));
			if (this.backgroundAudioSource != null && this.backgroundAudioClip != null)
			{
				this.backgroundAudioSource.clip = this.backgroundAudioClip;
				this.backgroundAudioSource.loop = true;
				this.backgroundAudioSource.spatialBlend = 0f;
				this.backgroundAudioSource.volume = 0f;
				this.backgroundAudioSource.Play();
				if (this._bgAudioCoroutine != null)
				{
					base.StopCoroutine(this._bgAudioCoroutine);
				}
				this._bgAudioCoroutine = base.StartCoroutine(this.FadeAudio(this.backgroundAudioSource, this.backgroundVolume, this.postEmoteFadeDuration));
			}
			yield return base.StartCoroutine(this.DoFade(true, this.postEmoteFadeDuration));
			if (brain != null)
			{
				brain.m_DefaultBlend = savedEnterBlend;
			}
			if (this.studioHintUI != null)
			{
				this.studioHintUI.SetActive(true);
			}
			while (this.enterInput.GetButton())
			{
				yield return null;
			}
			float earliestExit = Time.unscaledTime + 0.1f;
			bool tvCamActive = false;
			for (;;)
			{
				if (FlyCameraPhotoMode.AnyOpen)
				{
					yield return null;
				}
				else
				{
					if (Time.unscaledTime >= earliestExit && ControllerContextBindings.ExitPressed(this.enterInput))
					{
						break;
					}
					if (this.tvMonitorVCam != null && this.tvCamInput.GetButtonDown())
					{
						tvCamActive = !tvCamActive;
						this.tvMonitorVCam.Priority = (tvCamActive ? (this.studioVCamActivePriority + 10) : this.studioVCamInactivePriority);
					}
					yield return null;
				}
			}
			if (this.studioHintUI != null)
			{
				this.studioHintUI.SetActive(false);
			}
			CinemachineBlendDefinition savedExitBlend = default(CinemachineBlendDefinition);
			if (brain != null)
			{
				savedExitBlend = brain.m_DefaultBlend;
				brain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
			}
			if (this.backgroundAudioSource != null && this.backgroundAudioSource.isPlaying)
			{
				if (this._bgAudioCoroutine != null)
				{
					base.StopCoroutine(this._bgAudioCoroutine);
				}
				this._bgAudioCoroutine = base.StartCoroutine(this.FadeAudio(this.backgroundAudioSource, 0f, this.postEmoteFadeDuration));
			}
			yield return base.StartCoroutine(this.DoFade(false, this.postEmoteFadeDuration));
			yield return null;
			this.DeactivateTvCameraCycler();
			if (this.playerScreenCamera != null)
			{
				this.playerScreenCamera.enabled = false;
			}
			if (this.tvMonitorVCam != null)
			{
				this.tvMonitorVCam.Priority = this.studioVCamInactivePriority;
			}
			if (this.studioViewVCam != null)
			{
				this.studioViewVCam.Priority = this.studioVCamInactivePriority;
			}
			director.HideEncounterNPCsOnLayer();
			yield return null;
			yield return base.StartCoroutine(this.DoFade(true, this.postEmoteFadeDuration));
			if (brain != null)
			{
				brain.m_DefaultBlend = savedExitBlend;
			}
			DetectionAudioManager instance3 = DetectionAudioManager.Instance;
			if (instance3 != null)
			{
				instance3.RestoreAmbientVolume(this.musicRestoreDuration);
			}
			DetectionAudioManager instance4 = DetectionAudioManager.Instance;
			if (instance4 != null)
			{
				instance4.RestoreHeartbeatVolume(this.musicRestoreDuration);
			}
			this._isActive = false;
			this.ReleasePresentationState();
			yield break;
		}

		// Token: 0x0600970C RID: 38668 RVA: 0x003DF803 File Offset: 0x003DDA03
		private IEnumerator ActivateStudioCamerasWithCut()
		{
			CinemachineBrain brain = ((Camera.main != null) ? Camera.main.GetComponent<CinemachineBrain>() : null);
			if (brain != null)
			{
				CinemachineBlendDefinition saved = brain.m_DefaultBlend;
				brain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
				this.ConfigureStartCamLive();
				yield return null;
				brain.m_DefaultBlend = saved;
				saved = default(CinemachineBlendDefinition);
			}
			else
			{
				this.ConfigureStartCamLive();
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600970D RID: 38669 RVA: 0x003DF814 File Offset: 0x003DDA14
		private void ConfigureStartCamLive()
		{
			if (this.studioWideVCam != null)
			{
				this.studioWideVCam.gameObject.SetActive(true);
				this.studioWideVCam.Priority = this.studioVCamActivePriority + 5;
				if (this.studioViewVCam != null)
				{
					this.studioViewVCam.gameObject.SetActive(false);
					this.studioViewVCam.Priority = this.studioVCamActivePriority;
					return;
				}
			}
			else if (this.studioViewVCam != null)
			{
				this.studioViewVCam.gameObject.SetActive(true);
				this.studioViewVCam.Priority = this.studioVCamActivePriority + 5;
			}
		}

		// Token: 0x0600970E RID: 38670 RVA: 0x003DF8B6 File Offset: 0x003DDAB6
		private IEnumerator CutToVCam(int priority)
		{
			if (this.studioViewVCam == null)
			{
				yield break;
			}
			CinemachineBrain brain = ((Camera.main != null) ? Camera.main.GetComponent<CinemachineBrain>() : null);
			if (brain != null)
			{
				CinemachineBlendDefinition saved = brain.m_DefaultBlend;
				brain.m_DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
				bool isEnter = priority > this.studioVCamInactivePriority;
				this.studioViewVCam.Priority = (isEnter ? 9999 : priority);
				yield return null;
				if (isEnter)
				{
					this.studioViewVCam.Priority = priority + 5;
				}
				brain.m_DefaultBlend = saved;
				saved = default(CinemachineBlendDefinition);
			}
			else
			{
				this.studioViewVCam.Priority = priority;
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600970F RID: 38671 RVA: 0x003DF8CC File Offset: 0x003DDACC
		private IEnumerator ZoomInToAnchor()
		{
			if (this.studioWideVCam == null || this.studioViewVCam == null)
			{
				yield break;
			}
			CinemachineBrain brain = ((Camera.main != null) ? Camera.main.GetComponent<CinemachineBrain>() : null);
			CinemachineBlendDefinition saved = default(CinemachineBlendDefinition);
			bool hasSavedBlend = false;
			if (brain != null)
			{
				saved = brain.m_DefaultBlend;
				hasSavedBlend = true;
				brain.m_DefaultBlend = new CinemachineBlendDefinition(this.zoomInBlendStyle, this.zoomInDuration);
			}
			this.studioViewVCam.gameObject.SetActive(true);
			this.studioViewVCam.Priority = this.studioVCamActivePriority + 10;
			yield return new WaitForSeconds(this.zoomInDuration);
			this.studioWideVCam.Priority = this.studioVCamInactivePriority;
			this.studioWideVCam.gameObject.SetActive(false);
			if (hasSavedBlend)
			{
				brain.m_DefaultBlend = saved;
			}
			yield break;
		}

		// Token: 0x06009710 RID: 38672 RVA: 0x003DF8DB File Offset: 0x003DDADB
		private IEnumerator AnchorPostDialogue()
		{
			if (this.anchorMicProp != null)
			{
				if (this.micDeactivateDelay > 0f)
				{
					yield return new WaitForSeconds(this.micDeactivateDelay);
				}
				this.anchorMicProp.SetActive(false);
			}
			if (this.anchorTransform != null && this.tvLookTarget != null)
			{
				Vector3 vector = this.tvLookTarget.position - this.anchorTransform.position;
				vector.y = 0f;
				if (vector != Vector3.zero)
				{
					Quaternion targetRot = Quaternion.LookRotation(vector.normalized);
					if (!string.IsNullOrEmpty(this.turnToTVStateName) && this.anchorAnimator != null)
					{
						this.anchorAnimator.Play(this.turnToTVStateName);
					}
					if (this.turnToTVDuration > 0f)
					{
						Quaternion startRot = this.anchorTransform.rotation;
						float elapsed = 0f;
						while (elapsed < this.turnToTVDuration)
						{
							elapsed += Time.deltaTime;
							this.anchorTransform.rotation = Quaternion.Slerp(startRot, targetRot, elapsed / this.turnToTVDuration);
							yield return null;
						}
						this.anchorTransform.rotation = targetRot;
						startRot = default(Quaternion);
					}
					else
					{
						this.anchorTransform.rotation = targetRot;
					}
					targetRot = default(Quaternion);
				}
			}
			if (this.anchorAnimator != null && !string.IsNullOrEmpty(this.idleStateName))
			{
				this.anchorAnimator.Play(this.idleStateName);
			}
			yield break;
		}

		// Token: 0x06009711 RID: 38673 RVA: 0x003DF8EA File Offset: 0x003DDAEA
		private IEnumerator DoFade(bool fadeIn, float duration)
		{
			if (ScreenFadeController.Instance == null)
			{
				yield break;
			}
			bool done = false;
			if (fadeIn)
			{
				ScreenFadeController.Instance.FadeIn(duration, delegate
				{
					done = true;
				});
			}
			else
			{
				ScreenFadeController.Instance.FadeOut(duration, delegate
				{
					done = true;
				});
			}
			while (!done)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06009712 RID: 38674 RVA: 0x003DF900 File Offset: 0x003DDB00
		private IEnumerator FadeAudio(AudioSource source, float targetVolume, float duration)
		{
			if (source == null)
			{
				yield break;
			}
			float startVolume = source.volume;
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.unscaledDeltaTime;
				source.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
				yield return null;
			}
			source.volume = targetVolume;
			if (targetVolume <= 0f)
			{
				source.Stop();
			}
			yield break;
		}

		// Token: 0x04009C11 RID: 39953
		[Header("Studio Scene")]
		public GameObject studioRoot;

		// Token: 0x04009C12 RID: 39954
		public CinemachineVirtualCamera studioViewVCam;

		// Token: 0x04009C13 RID: 39955
		[Tooltip("Priority della studioViewVCam durante il suo utilizzo normale (dopo il cut iniziale).")]
		public int studioVCamActivePriority = 30;

		// Token: 0x04009C14 RID: 39956
		[Tooltip("Priority quando lo studio è inattivo (0 = sempre sotto tutto).")]
		public int studioVCamInactivePriority;

		// Token: 0x04009C15 RID: 39957
		[Header("Player Screen Camera")]
		[Tooltip("Camera dedicata (NON MainCamera) — posizionala come GO fisso nella scena senza follow script per evitare oscillazioni. Assegna RenderTexture come targetTexture.")]
		public Camera playerScreenCamera;

		// Token: 0x04009C16 RID: 39958
		[Tooltip("Se true, all'attivazione settiamo playerScreenCamera.clearFlags = Skybox così la TV renderizza il cielo (es. Azure[Sky]) invece di vederlo nero. Necessario perché la cam, quando configurata come ClearFlags = Nothing/SolidColor=black, non disegna lo skybox.")]
		public bool forcePlayerScreenSkyboxClear = true;

		// Token: 0x04009C17 RID: 39959
		[Header("Anchor NPC")]
		public Animator anchorAnimator;

		// Token: 0x04009C18 RID: 39960
		[Tooltip("Stato Talking nell'animator (imposta Loop Time = true sulla clip).")]
		public string talkingStateName = "Talking";

		// Token: 0x04009C19 RID: 39961
		[Tooltip("Stato Idle a cui tornare dopo il dialogo.")]
		public string idleStateName = "Idle";

		// Token: 0x04009C1A RID: 39962
		[Header("Anchor Dialogue")]
		public DialogueLine[] presenterLines;

		// Token: 0x04009C1B RID: 39963
		[Header("Anchor Post-Dialogue")]
		[Tooltip("Microfono del presenter. Attivato quando inizia il Talking, disattivato a fine dialogo.")]
		public GameObject anchorMicProp;

		// Token: 0x04009C1C RID: 39964
		[Tooltip("Secondi tra fine dialogo e spegnimento del microfono (0 = immediato).")]
		public float micDeactivateDelay = 0.2f;

		// Token: 0x04009C1D RID: 39965
		[Tooltip("Transform del presenter usato per la rotazione verso la TV. Se null la rotazione è saltata.")]
		public Transform anchorTransform;

		// Token: 0x04009C1E RID: 39966
		[Tooltip("Punto target verso cui girare dopo il dialogo (es. centro schermo TV). Se null la rotazione è saltata.")]
		public Transform tvLookTarget;

		// Token: 0x04009C1F RID: 39967
		[Tooltip("Durata del blend di rotazione verso la TV in secondi. 0 = istantaneo.")]
		public float turnToTVDuration = 0.6f;

		// Token: 0x04009C20 RID: 39968
		[Tooltip("Stato Animator opzionale triggerato durante la rotazione (es. 'TurnRight'). Lasciare vuoto per nessuno.")]
		public string turnToTVStateName = "";

		// Token: 0x04009C21 RID: 39969
		[Header("TV News Intro (opzionale)")]
		[Tooltip("Il GO può essere inattivo in scena — viene attivato a runtime.")]
		public TVNewsIntroController newsIntro;

		// Token: 0x04009C22 RID: 39970
		[Header("Zoom-In Intro (opzionale)")]
		[Tooltip("VCam laterale/wide di partenza. Se assegnata la scena parte da qui e zooma verso studioViewVCam dopo il crossfade dell'intro. Lasciare vuota per saltare lo zoom.")]
		public CinemachineVirtualCamera studioWideVCam;

		// Token: 0x04009C23 RID: 39971
		[Tooltip("Durata del blend da studioWideVCam a studioViewVCam (secondi).")]
		public float zoomInDuration = 1.5f;

		// Token: 0x04009C24 RID: 39972
		[Tooltip("Stile del blend Cinemachine per lo zoom-in.")]
		public CinemachineBlendDefinition.Style zoomInBlendStyle = CinemachineBlendDefinition.Style.EaseInOut;

		// Token: 0x04009C25 RID: 39973
		[Header("Background Audio (durante dialogo anchor)")]
		[Tooltip("Audio in loop per tutta la durata della scenetta nello studio.")]
		public AudioClip backgroundAudioClip;

		// Token: 0x04009C26 RID: 39974
		[Range(0f, 1f)]
		public float backgroundVolume = 0.3f;

		// Token: 0x04009C27 RID: 39975
		public AudioSource backgroundAudioSource;

		// Token: 0x04009C28 RID: 39976
		[Header("Detection Music")]
		[Tooltip("Durata fade-out musica di detection all'ingresso nello studio.")]
		public float musicMuteDuration = 0.5f;

		// Token: 0x04009C29 RID: 39977
		[Tooltip("Durata fade-in di ripristino musica di detection all'uscita.")]
		public float musicRestoreDuration = 1f;

		// Token: 0x04009C2A RID: 39978
		[Header("Fade & Timing")]
		[Tooltip("Durata fade per la sequenza pre-reaction.")]
		public float fadeDuration = 0.5f;

		// Token: 0x04009C2B RID: 39979
		[Tooltip("Durata fade rapido nel post-emote mode (cambio camera nascosto dal nero).")]
		public float postEmoteFadeDuration = 0.25f;

		// Token: 0x04009C2C RID: 39980
		[Tooltip("Attesa dopo la fine del dialogo del presentatore prima del FadeOut finale.")]
		public float postDialogueDelay = 1f;

		// Token: 0x04009C2D RID: 39981
		[Header("Post-Emote Mode")]
		public GenericInput enterInput = new GenericInput("T", "B", "B");

		// Token: 0x04009C2E RID: 39982
		public string uiHintText = "Studio TV [T]";

		// Token: 0x04009C2F RID: 39983
		[Tooltip("GO hint nel flusso principale (mostrato da WaitForEmoteAndSkip — es. 'Studio TV [T]').")]
		public GameObject mainFlowHintUI;

		// Token: 0x04009C30 RID: 39984
		[Tooltip("GO hint interno allo studio (es. 'Esci dallo Studio [T]'). Attivato/disattivato da Execute.")]
		public GameObject studioHintUI;

		// Token: 0x04009C31 RID: 39985
		[Header("TV Monitor Camera (Post-Emote Mode)")]
		[Tooltip("VCam puntata verso la TV dello studio. Se assegnata è accessibile con tvCamInput durante il post-emote.")]
		public CinemachineVirtualCamera tvMonitorVCam;

		// Token: 0x04009C32 RID: 39986
		[Tooltip("Input per attivare/disattivare la camera puntata alla TV.")]
		public GenericInput tvCamInput = new GenericInput("V", "RightBumper", "RightBumper");

		// Token: 0x04009C33 RID: 39987
		[Header("TV Cycle Camera (presets sulla TV monitor)")]
		[Tooltip("Istanza separata di DetectionCameraController dedicata alla TV. Setup richiesto:\n 1) Crea un layer dedicato (es. 'TVCameraOnly').\n 2) playerScreenCamera (TV camera): aggiungi un CinemachineBrain, e nel suo cullingMask\n    INCLUDI quel layer. Il cullingMask di Camera.main lo deve ESCLUDERE.\n 3) Crea un GO con DetectionCameraController + un CinemachineVirtualCamera figlio.\n    Metti il CM child sul layer 'TVCameraOnly'. Assegnalo a ManagedVCam.\n 4) Sul controller: OutputCameraOverride = playerScreenCamera, CycleCameraInput = C.\nCinemachine 2.10 filtra i VCam tramite cullingMask della Camera output del brain,\ncosì la main brain ignora i VCam della TV e i due flussi restano isolati.")]
		public DetectionCameraController tvCameraCycler;

		// Token: 0x04009C34 RID: 39988
		[Tooltip("Collezione di preset DetectionCameraPresetCollectionSO da usare per la TV cycle camera. Stesso SO type usato per le emote/NPC camera presets nel main detection flow.")]
		public DetectionCameraPresetCollectionSO tvCameraPresets;

		// Token: 0x04009C35 RID: 39989
		private bool _isActive;

		// Token: 0x04009C36 RID: 39990
		private Coroutine _bgAudioCoroutine;

		// Token: 0x04009C37 RID: 39991
		private bool _presentationStateCaptured;

		// Token: 0x04009C38 RID: 39992
		private CinemachineBrain _presentationBrain;

		// Token: 0x04009C39 RID: 39993
		private CinemachineBlendDefinition _presentationBlend;

		// Token: 0x04009C3A RID: 39994
		private CinemachineBrain _presentationTvBrain;

		// Token: 0x04009C3B RID: 39995
		private CinemachineBlendDefinition _presentationTvBlend;

		// Token: 0x04009C3C RID: 39996
		private bool _studioRootWasActive;

		// Token: 0x04009C3D RID: 39997
		private SimpleDialogueManager _presenterDialogueManager;

		// Token: 0x04009C3E RID: 39998
		private object _presenterDialogueToken;

		// Token: 0x04009C3F RID: 39999
		private Quaternion _anchorInitialRotation;

		// Token: 0x04009C40 RID: 40000
		private bool _hasAnchorInitialRotation;

		// Token: 0x04009C41 RID: 40001
		private int _anchorInitialStateHash;

		// Token: 0x04009C42 RID: 40002
		private bool _hasAnchorInitialState;
	}
}
