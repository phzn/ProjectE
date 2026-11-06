using System;
using Features.NPC;
using Features.RunModes;
using Features.TimeFreeze;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Features.Detection
{
	// Token: 0x0200123D RID: 4669
	public class AttentionUIController : MonoBehaviour
	{
		// Token: 0x17001734 RID: 5940
		// (get) Token: 0x06009130 RID: 37168 RVA: 0x003B710A File Offset: 0x003B530A
		// (set) Token: 0x06009131 RID: 37169 RVA: 0x003B7112 File Offset: 0x003B5312
		public IDetectionUISource UISource { get; private set; }

		// Token: 0x06009132 RID: 37170 RVA: 0x003B711B File Offset: 0x003B531B
		public void SetUISource(IDetectionUISource source)
		{
			this.UISource = source;
		}

		// Token: 0x06009133 RID: 37171 RVA: 0x003B711B File Offset: 0x003B531B
		public void SetTroupe(ReporterTroupe troupe)
		{
			this.UISource = troupe;
		}

		// Token: 0x17001735 RID: 5941
		// (get) Token: 0x06009134 RID: 37172 RVA: 0x003B7124 File Offset: 0x003B5324
		// (set) Token: 0x06009135 RID: 37173 RVA: 0x003B712C File Offset: 0x003B532C
		internal NPCAmbientSuspension ProximitySuspension { get; set; }

		// Token: 0x17001736 RID: 5942
		// (get) Token: 0x06009136 RID: 37174 RVA: 0x003B7135 File Offset: 0x003B5335
		// (set) Token: 0x06009137 RID: 37175 RVA: 0x003B713C File Offset: 0x003B533C
		public static bool GlobalForceHideAll { get; set; }

		// Token: 0x06009138 RID: 37176 RVA: 0x003B7144 File Offset: 0x003B5344
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void RegisterSceneResetHook()
		{
			AttentionUIController.GlobalForceHideAll = false;
			SceneManager.sceneLoaded -= AttentionUIController.OnSceneLoadedResetStatics;
			SceneManager.sceneLoaded += AttentionUIController.OnSceneLoadedResetStatics;
		}

		// Token: 0x06009139 RID: 37177 RVA: 0x003B716E File Offset: 0x003B536E
		private static void OnSceneLoadedResetStatics(Scene scene, LoadSceneMode mode)
		{
			AttentionUIController.GlobalForceHideAll = false;
		}

		// Token: 0x0600913A RID: 37178 RVA: 0x003B7178 File Offset: 0x003B5378
		private void OnEnable()
		{
			this.ConfigureRendering();
			NPCSimulationPause.PauseChanged -= this.OnSimulationPauseChanged;
			NPCSimulationPause.PauseChanged += this.OnSimulationPauseChanged;
			this._simulationPaused = NPCSimulationPause.IsPaused;
			if (this._simulationPaused)
			{
				this.ResetPresentationState();
				this.HideCanvasImmediate();
			}
		}

		// Token: 0x0600913B RID: 37179 RVA: 0x003B71CC File Offset: 0x003B53CC
		private void OnDisable()
		{
			NPCSimulationPause.PauseChanged -= this.OnSimulationPauseChanged;
		}

		// Token: 0x0600913C RID: 37180 RVA: 0x003B71E0 File Offset: 0x003B53E0
		private void Start()
		{
			this.ConfigureRendering();
			this._mainCamera = Camera.main;
			if (this.CanvasRoot)
			{
				this.CanvasRoot.gameObject.SetActive(false);
			}
			if (SensoryManager.Instance)
			{
				this._config = SensoryManager.Instance.GlobalConfig;
			}
			if (ExhibitionEventDirector.Instance != null)
			{
				ExhibitionEventDirector.Instance.OnDetectionStarted.AddListener(new UnityAction<NPCSenses>(this.OnDetectionTriggered));
				ExhibitionEventDirector.Instance.OnCooldownEnded.AddListener(new UnityAction<NPCSenses>(this.OnCooldownEnded));
			}
		}

		// Token: 0x0600913D RID: 37181 RVA: 0x003B727C File Offset: 0x003B547C
		private void OnDestroy()
		{
			NPCSimulationPause.PauseChanged -= this.OnSimulationPauseChanged;
			if (ExhibitionEventDirector.Instance != null)
			{
				ExhibitionEventDirector.Instance.OnDetectionStarted.RemoveListener(new UnityAction<NPCSenses>(this.OnDetectionTriggered));
				ExhibitionEventDirector.Instance.OnCooldownEnded.RemoveListener(new UnityAction<NPCSenses>(this.OnCooldownEnded));
			}
		}

		// Token: 0x0600913E RID: 37182 RVA: 0x003B72DD File Offset: 0x003B54DD
		private void OnDetectionTriggered(NPCSenses npc)
		{
			if (npc == this.TargetNPC)
			{
				this._forcedHidden = true;
				if (this.CanvasRoot)
				{
					this.CanvasRoot.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x0600913F RID: 37183 RVA: 0x003B7312 File Offset: 0x003B5512
		private void OnCooldownEnded(NPCSenses npc)
		{
			if (npc == this.TargetNPC)
			{
				this._forcedHidden = false;
			}
		}

		// Token: 0x06009140 RID: 37184 RVA: 0x003B732C File Offset: 0x003B552C
		private void LateUpdate()
		{
			if (this.ProximitySuspension != null && this.ProximitySuspension.IsSuspended)
			{
				this.HideCanvasImmediate();
				return;
			}
			this.ConfigureRendering();
			if (AttentionUIController.GlobalForceHideAll || TimeFreezeController.IsAnyActive || this._simulationPaused || NPCSimulationPause.IsPaused || Time.frameCount <= this._resumePresentationAfterFrame)
			{
				this.HideCanvasImmediate();
				return;
			}
			if (this.TargetNPC == null)
			{
				return;
			}
			if (this._forcedHidden)
			{
				if (this.CanvasRoot && this.CanvasRoot.gameObject.activeSelf)
				{
					this.CanvasRoot.gameObject.SetActive(false);
				}
				return;
			}
			if (this.CanvasRoot == null)
			{
				return;
			}
			if (this.UISource != null)
			{
				this.UpdateSourceMode();
			}
			else
			{
				this.UpdateStandardMode();
			}
			if (this.Billboard && this.CanvasRoot.gameObject.activeInHierarchy)
			{
				if (this._mainCamera == null)
				{
					this._mainCamera = Camera.main;
				}
				if (this._mainCamera != null)
				{
					this.CanvasRoot.transform.rotation = Quaternion.LookRotation(this.CanvasRoot.transform.position - this._mainCamera.transform.position);
				}
			}
		}

		// Token: 0x06009141 RID: 37185 RVA: 0x003B747C File Offset: 0x003B567C
		private void ConfigureRendering()
		{
			if (this.CanvasRoot == null || this.CanvasRoot == this._renderingCanvas)
			{
				return;
			}
			GameplayHudVisibility.RegisterCanvas(this.CanvasRoot);
			Material material = Resources.Load<Material>("NPCDetectionUI");
			if (material == null)
			{
				return;
			}
			foreach (Graphic graphic in this.CanvasRoot.GetComponentsInChildren<Graphic>(true))
			{
				if (graphic.material == graphic.defaultMaterial)
				{
					graphic.material = material;
				}
			}
			this._renderingCanvas = this.CanvasRoot;
		}

		// Token: 0x06009142 RID: 37186 RVA: 0x003B7510 File Offset: 0x003B5710
		private void OnSimulationPauseChanged(bool paused)
		{
			this._simulationPaused = paused;
			this.ResetPresentationState();
			this.HideCanvasImmediate();
			if (paused)
			{
				this._resumePresentationAfterFrame = -1;
				return;
			}
			this._resumePresentationAfterFrame = Time.frameCount + 1;
		}

		// Token: 0x06009143 RID: 37187 RVA: 0x003B753D File Offset: 0x003B573D
		private void HideCanvasImmediate()
		{
			if (this.CanvasRoot != null && this.CanvasRoot.gameObject.activeSelf)
			{
				this.CanvasRoot.gameObject.SetActive(false);
			}
		}

		// Token: 0x06009144 RID: 37188 RVA: 0x003B7570 File Offset: 0x003B5770
		private void ResetPresentationState()
		{
			this._displayedSource = DetectionSource.None;
			this._sourceChangeTime = Time.time;
			if (this.FillImage != null)
			{
				this.FillImage.fillAmount = 0f;
			}
			if (this.VisualDetectionIcon != null)
			{
				this.VisualDetectionIcon.gameObject.SetActive(false);
			}
			if (this.AudioDetectionIcon != null)
			{
				this.AudioDetectionIcon.gameObject.SetActive(false);
			}
		}

		// Token: 0x06009145 RID: 37189 RVA: 0x003B75EC File Offset: 0x003B57EC
		private void UpdateSourceMode()
		{
			bool visualActive = this.UISource.VisualActive;
			bool audioActive = this.UISource.AudioActive;
			float num = ((RunModeService.IsLongWayHome && this.TargetNPC != null) ? this.TargetNPC.CurrentAttention : this.UISource.DetectionLevel);
			bool flag = visualActive || audioActive || num > 0.01f;
			if (this.CanvasRoot.gameObject.activeSelf != flag)
			{
				this.CanvasRoot.gameObject.SetActive(flag);
			}
			if (!flag)
			{
				return;
			}
			if (this.FillImage)
			{
				bool flag2 = num > 0.01f;
				if (this.FillImage.enabled != flag2)
				{
					this.FillImage.enabled = flag2;
				}
				if (flag2)
				{
					this.FillImage.fillAmount = num;
					if (this.ColorGradient != null)
					{
						this.FillImage.color = this.ColorGradient.Evaluate(num);
					}
				}
			}
			if (this.VisualDetectionIcon)
			{
				this.VisualDetectionIcon.gameObject.SetActive(visualActive);
			}
			if (this.AudioDetectionIcon)
			{
				this.AudioDetectionIcon.gameObject.SetActive(audioActive);
			}
			if (visualActive || audioActive)
			{
				int num2 = Time.frameCount % 60;
			}
		}

		// Token: 0x06009146 RID: 37190 RVA: 0x003B7724 File Offset: 0x003B5924
		private void UpdateStandardMode()
		{
			float currentAttention = this.TargetNPC.CurrentAttention;
			DetectionSource currentDetectionSource = this.TargetNPC.CurrentDetectionSource;
			bool flag = currentAttention > 0.01f || currentDetectionSource > DetectionSource.None;
			if (this.CanvasRoot.gameObject.activeSelf != flag)
			{
				this.CanvasRoot.gameObject.SetActive(flag);
			}
			if (!flag)
			{
				return;
			}
			if (this.FillImage)
			{
				this.FillImage.fillAmount = currentAttention;
				if (this.ColorGradient != null)
				{
					this.FillImage.color = this.ColorGradient.Evaluate(currentAttention);
				}
			}
			if (currentDetectionSource != this._displayedSource && (currentDetectionSource == DetectionSource.Visual || Time.time - this._sourceChangeTime >= this.IconSwitchDelay))
			{
				this._displayedSource = currentDetectionSource;
				this._sourceChangeTime = Time.time;
			}
			bool flag2 = this._displayedSource == DetectionSource.Visual;
			bool flag3 = this._displayedSource == DetectionSource.Audio;
			if (this.VisualDetectionIcon)
			{
				this.VisualDetectionIcon.gameObject.SetActive(flag2);
			}
			if (this.AudioDetectionIcon)
			{
				this.AudioDetectionIcon.gameObject.SetActive(flag3);
			}
			if (this.TargetNPC.IsDetected || currentAttention > 0.5f)
			{
				int num = Time.frameCount % 60;
			}
		}

		// Token: 0x04009555 RID: 38229
		[Header("References")]
		public NPCSenses TargetNPC;

		// Token: 0x04009556 RID: 38230
		public Canvas CanvasRoot;

		// Token: 0x04009558 RID: 38232
		[Header("Progress Bar")]
		[Tooltip("Image with Fill method set (Radial or Horizontal)")]
		public Image FillImage;

		// Token: 0x04009559 RID: 38233
		public Gradient ColorGradient;

		// Token: 0x0400955A RID: 38234
		[Header("Detection Source Icons")]
		[Tooltip("Icon shown when NPC detects player visually (eye icon)")]
		public Image VisualDetectionIcon;

		// Token: 0x0400955B RID: 38235
		[Tooltip("Icon shown when NPC detects player by sound (ear icon)")]
		public Image AudioDetectionIcon;

		// Token: 0x0400955C RID: 38236
		[Header("Settings")]
		public bool Billboard = true;

		// Token: 0x0400955D RID: 38237
		[Tooltip("Minimum time an icon must be active before it can switch (prevents flickering)")]
		public float IconSwitchDelay = 0.5f;

		// Token: 0x0400955E RID: 38238
		private Camera _mainCamera;

		// Token: 0x04009560 RID: 38240
		private Canvas _renderingCanvas;

		// Token: 0x04009561 RID: 38241
		private DetectionConfigSO _config;

		// Token: 0x04009562 RID: 38242
		private bool _forcedHidden;

		// Token: 0x04009563 RID: 38243
		private bool _simulationPaused;

		// Token: 0x04009564 RID: 38244
		private int _resumePresentationAfterFrame = -1;

		// Token: 0x04009565 RID: 38245
		private DetectionSource _displayedSource;

		// Token: 0x04009566 RID: 38246
		private float _sourceChangeTime;
	}
}
