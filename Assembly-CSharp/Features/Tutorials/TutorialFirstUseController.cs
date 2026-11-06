using System;
using System.Collections.Generic;
using Features.Home;
using Features.Loading;
using Invector.vCharacterController;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Core.Rendering;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.Tutorials
{
	// Token: 0x02000BD3 RID: 3027
	[DefaultExecutionOrder(-20000)]
	[DisallowMultipleComponent]
	public sealed class TutorialFirstUseController : MonoBehaviour
	{
		// Token: 0x17000E85 RID: 3717
		// (get) Token: 0x06005719 RID: 22297 RVA: 0x0021E69B File Offset: 0x0021C89B
		// (set) Token: 0x0600571A RID: 22298 RVA: 0x0021E6A2 File Offset: 0x0021C8A2
		public static TutorialFirstUseController Instance { get; private set; }

		// Token: 0x17000E86 RID: 3718
		// (get) Token: 0x0600571B RID: 22299 RVA: 0x0021E6AA File Offset: 0x0021C8AA
		// (set) Token: 0x0600571C RID: 22300 RVA: 0x0021E6B2 File Offset: 0x0021C8B2
		public string ActiveTopicId { get; private set; }

		// Token: 0x0600571D RID: 22301 RVA: 0x0021E6BB File Offset: 0x0021C8BB
		public bool IsPresenting(string topicId)
		{
			return this._ownsActivityLock && string.Equals(this.ActiveTopicId, topicId, StringComparison.Ordinal);
		}

		// Token: 0x0600571E RID: 22302 RVA: 0x0021E6D4 File Offset: 0x0021C8D4
		public void Initialize(GraphicsSettingsRuntimeUI pauseMenu)
		{
			this._pauseMenu = pauseMenu;
			TutorialFirstUseController.Instance = this;
			this.ObserveCurrentContext();
		}

		// Token: 0x0600571F RID: 22303 RVA: 0x0021E6E9 File Offset: 0x0021C8E9
		private void OnEnable()
		{
			if (TutorialFirstUseController.Instance == null)
			{
				TutorialFirstUseController.Instance = this;
			}
		}

		// Token: 0x06005720 RID: 22304 RVA: 0x0021E6FE File Offset: 0x0021C8FE
		private void OnDisable()
		{
			this.ReleaseActivityLock();
			if (TutorialFirstUseController.Instance == this)
			{
				TutorialFirstUseController.Instance = null;
			}
		}

		// Token: 0x06005721 RID: 22305 RVA: 0x0021E719 File Offset: 0x0021C919
		private void OnDestroy()
		{
			this.OnDisable();
		}

		// Token: 0x06005722 RID: 22306 RVA: 0x0021E724 File Offset: 0x0021C924
		private void Update()
		{
			if (this._observedContext != SceneContext.Current)
			{
				this.ObserveCurrentContext();
			}
			if (this._sceneHintAt > 0f && Time.unscaledTime >= this._sceneHintAt)
			{
				this._sceneHintAt = (this.TryShowSceneIntroduction() ? (-1f) : (Time.unscaledTime + 0.25f));
			}
			if (!this.CanObserveInput())
			{
				return;
			}
			if (GameplayInput.GetKeyDown(KeyCode.P) && PlayerActivityGate.Allows(PlayerActivity.Phone))
			{
				TutorialService.TryShowFirstUse("smartphone");
				return;
			}
			if (GameplayInput.GetKeyDown(KeyCode.K) && PlayerActivityGate.Allows(PlayerActivity.DebugFly))
			{
				TutorialService.TryShowFirstUse("photo_mode");
			}
		}

		// Token: 0x06005723 RID: 22307 RVA: 0x0021E7D0 File Offset: 0x0021C9D0
		public bool TryPresent(string topicId)
		{
			if (this.IsPresenting(topicId))
			{
				return true;
			}
			TutorialTopic tutorialTopic;
			if (this._pauseMenu == null || TutorialProgress.HasSeen(topicId) || !TutorialCatalog.TryGetTopic(topicId, out tutorialTopic))
			{
				return false;
			}
			if (this._ownsActivityLock && this._pauseMenu.IsOpen)
			{
				if (!this._sceneTopics.Contains(topicId))
				{
					this._sceneTopics.Enqueue(topicId);
				}
				return true;
			}
			if (this._pauseMenu.IsOpen || !this.IsPresentationEnvironmentReady())
			{
				return false;
			}
			this.EnsureSceneIntroductionsQueued();
			this.AcquireActivityLock(topicId);
			if (!this._pauseMenu.ShowTutorial(topicId, true))
			{
				this.ReleaseActivityLock();
				return false;
			}
			TutorialProgress.MarkSeen(topicId);
			this._inputEnabledAt = Time.unscaledTime + 0.65f;
			return true;
		}

		// Token: 0x06005724 RID: 22308 RVA: 0x0021E890 File Offset: 0x0021CA90
		public bool TryContinuePresentation()
		{
			if (!this._ownsActivityLock || this._pauseMenu == null || !this._pauseMenu.IsOpen)
			{
				return false;
			}
			while (this._sceneTopics.Count > 0)
			{
				string text = this._sceneTopics.Peek();
				TutorialTopic tutorialTopic;
				if (TutorialProgress.HasSeen(text) || !TutorialCatalog.TryGetTopic(text, out tutorialTopic))
				{
					this._sceneTopics.Dequeue();
				}
				else
				{
					if (!this._pauseMenu.ShowNextTutorial(text))
					{
						return false;
					}
					this._sceneTopics.Dequeue();
					this.ActiveTopicId = text;
					TutorialProgress.MarkSeen(text);
					return true;
				}
			}
			this._sceneHintAt = -1f;
			return false;
		}

		// Token: 0x06005725 RID: 22309 RVA: 0x0021E933 File Offset: 0x0021CB33
		public void NotifyDismissed()
		{
			if (!this._ownsActivityLock)
			{
				return;
			}
			this.ReleaseActivityLock();
			this._inputEnabledAt = Time.unscaledTime + 0.65f;
		}

		// Token: 0x06005726 RID: 22310 RVA: 0x0021E958 File Offset: 0x0021CB58
		private bool CanObserveInput()
		{
			return !TextInputFocus.BlocksGameplayInput && this._pauseMenu != null && !this._pauseMenu.IsOpen && this._observedContext != null && this._observedContext.Profile != null && this.IsPresentationEnvironmentReady() && Time.timeScale > 0.0001f && Time.unscaledTime >= this._inputEnabledAt;
		}

		// Token: 0x06005727 RID: 22311 RVA: 0x0021E9D0 File Offset: 0x0021CBD0
		private void ObserveCurrentContext()
		{
			this._observedContext = SceneContext.Current;
			this._homeIntro = Object.FindObjectOfType<CielHomeIntroSequence>(true);
			this._sceneTopics.Clear();
			this._sceneTopicsQueued = false;
			this._sceneHintAt = ((this._observedContext != null) ? (Time.unscaledTime + 1.1f) : (-1f));
			this._inputEnabledAt = Time.unscaledTime + 1.25f;
		}

		// Token: 0x06005728 RID: 22312 RVA: 0x0021EA40 File Offset: 0x0021CC40
		private bool TryShowSceneIntroduction()
		{
			if (this._observedContext == null || this._observedContext.Profile == null || this._pauseMenu == null || this._pauseMenu.IsOpen || !this.IsPresentationEnvironmentReady() || Time.unscaledTime < this._inputEnabledAt)
			{
				return false;
			}
			this.EnsureSceneIntroductionsQueued();
			while (this._sceneTopics.Count > 0)
			{
				string text = this._sceneTopics.Peek();
				TutorialTopic tutorialTopic;
				if (TutorialProgress.HasSeen(text) || !TutorialCatalog.TryGetTopic(text, out tutorialTopic))
				{
					this._sceneTopics.Dequeue();
				}
				else
				{
					if (!this.TryPresent(text))
					{
						return false;
					}
					this._sceneTopics.Dequeue();
					return this._sceneTopics.Count == 0;
				}
			}
			return true;
		}

		// Token: 0x06005729 RID: 22313 RVA: 0x0021EB0C File Offset: 0x0021CD0C
		private void EnsureSceneIntroductionsQueued()
		{
			if (!this._sceneTopicsQueued && this._observedContext != null && this._observedContext.Profile != null)
			{
				this.QueueSceneIntroductions(this._observedContext.gameObject.scene.name, this._observedContext.Profile.participatesInRun, this._observedContext.Profile.detection);
			}
		}

		// Token: 0x0600572A RID: 22314 RVA: 0x0021EB80 File Offset: 0x0021CD80
		private void QueueSceneIntroductions(string sceneName, bool participatesInRun, bool detectionEnabled)
		{
			this._sceneTopics.Clear();
			if (participatesInRun && !string.Equals(sceneName, "CielHome", StringComparison.OrdinalIgnoreCase))
			{
				for (int i = 0; i < TutorialFirstUseController.RunIntroductionTopics.Length; i++)
				{
					this._sceneTopics.Enqueue(TutorialFirstUseController.RunIntroductionTopics[i]);
				}
			}
			string text = TutorialFirstUseController.ResolveAutomaticSceneTopic(sceneName, participatesInRun, detectionEnabled);
			if (!string.IsNullOrEmpty(text))
			{
				this._sceneTopics.Enqueue(text);
			}
			this._sceneTopicsQueued = true;
		}

		// Token: 0x0600572B RID: 22315 RVA: 0x0021EBF1 File Offset: 0x0021CDF1
		public static string ResolveAutomaticSceneTopic(string sceneName, bool participatesInRun, bool detectionEnabled)
		{
			if (string.Equals(sceneName, "CielHome", StringComparison.OrdinalIgnoreCase))
			{
				return "introduction";
			}
			if (!participatesInRun || !detectionEnabled)
			{
				return null;
			}
			return "detection";
		}

		// Token: 0x0600572C RID: 22316 RVA: 0x0021EC14 File Offset: 0x0021CE14
		private bool IsPresentationEnvironmentReady()
		{
			if (this._observedContext == null || this._observedContext.Profile == null || Time.timeScale <= 0.0001f)
			{
				return false;
			}
			if (this._homeIntro != null && this._homeIntro.isActiveAndEnabled && !this._homeIntro.HasCompleted)
			{
				return false;
			}
			if (!this.CanPresentOverCurrentActivity())
			{
				return false;
			}
			Scene activeScene = SceneManager.GetActiveScene();
			if (!activeScene.IsValid() || activeScene.handle != this._observedContext.gameObject.scene.handle)
			{
				return false;
			}
			SceneTransitionManager instance = SceneTransitionManager.Instance;
			if (instance != null && instance.IsTransitioning)
			{
				return false;
			}
			LoadingScreenController existingInstance = LoadingScreenController.ExistingInstance;
			return (!(existingInstance != null) || !existingInstance.IsLoadingActive) && SceneBootLoader.CoreReady && GameSystemsBootstrapper.GameplayReady;
		}

		// Token: 0x0600572D RID: 22317 RVA: 0x0021ECF8 File Offset: 0x0021CEF8
		private bool CanPresentOverCurrentActivity()
		{
			Transform player = PlayerRegistry.Player;
			vThirdPersonController vThirdPersonController = ((player != null) ? player.GetComponent<vThirdPersonController>() : null);
			vThirdPersonInput vThirdPersonInput = ((player != null) ? player.GetComponent<vThirdPersonInput>() : null);
			return (!(vThirdPersonController != null) || (!vThirdPersonController.customAction && !vThirdPersonController.isRolling && !vThirdPersonController.ragdolled && !vThirdPersonController.isJumping && vThirdPersonController.isGrounded)) && (!(vThirdPersonInput != null) || !vThirdPersonInput.lockInput) && (this._pauseMenu != null && this._pauseMenu.CanOpen && PlayerActivityGate.Allows(PlayerActivity.Interact)) && !PlayerActivityGate.IsResourceOwned(PlayerContextResource.WorldInteraction | PlayerContextResource.PlayerControl | PlayerContextResource.Camera | PlayerContextResource.FullBodyAnimation | PlayerContextResource.ModalUI | PlayerContextResource.Rider | PlayerContextResource.HeldItem);
		}

		// Token: 0x0600572E RID: 22318 RVA: 0x0021EDA5 File Offset: 0x0021CFA5
		private void AcquireActivityLock(string topicId)
		{
			this.ReleaseActivityLock();
			this.ActiveTopicId = topicId;
			PlayerActivityGate.Push("Tutorial.FirstUse", PlayerActivity.AllPlayerActions);
			this._ownsActivityLock = true;
		}

		// Token: 0x0600572F RID: 22319 RVA: 0x0021EDCA File Offset: 0x0021CFCA
		private void ReleaseActivityLock()
		{
			if (!this._ownsActivityLock)
			{
				return;
			}
			PlayerActivityGate.Pop("Tutorial.FirstUse");
			this._ownsActivityLock = false;
			this.ActiveTopicId = null;
		}

		// Token: 0x06005730 RID: 22320 RVA: 0x0021EDED File Offset: 0x0021CFED
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetRuntimeState()
		{
			TutorialFirstUseController.Instance = null;
		}

		// Token: 0x0400579C RID: 22428
		private const string ActivityLockReason = "Tutorial.FirstUse";

		// Token: 0x0400579D RID: 22429
		private static readonly string[] RunIntroductionTopics = new string[] { "movement", "perspective", "crouch_actions", "inventory", "minimap", "rapid_actions", "world_cover" };

		// Token: 0x0400579E RID: 22430
		private GraphicsSettingsRuntimeUI _pauseMenu;

		// Token: 0x0400579F RID: 22431
		private SceneContext _observedContext;

		// Token: 0x040057A0 RID: 22432
		private CielHomeIntroSequence _homeIntro;

		// Token: 0x040057A1 RID: 22433
		private float _sceneHintAt = -1f;

		// Token: 0x040057A2 RID: 22434
		private float _inputEnabledAt;

		// Token: 0x040057A3 RID: 22435
		private bool _ownsActivityLock;

		// Token: 0x040057A4 RID: 22436
		private readonly Queue<string> _sceneTopics = new Queue<string>();

		// Token: 0x040057A5 RID: 22437
		private bool _sceneTopicsQueued;
	}
}
