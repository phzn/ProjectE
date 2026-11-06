using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Features.MainMenu;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.Core.Rendering
{
	// Token: 0x02000A9A RID: 2714
	public class GraphicsSettingsController : MonoBehaviour
	{
		// Token: 0x17000D06 RID: 3334
		// (get) Token: 0x06004D3B RID: 19771 RVA: 0x001DFBE4 File Offset: 0x001DDDE4
		// (set) Token: 0x06004D3C RID: 19772 RVA: 0x001DFBEB File Offset: 0x001DDDEB
		public static GraphicsSettingsController Instance { get; private set; }

		// Token: 0x1400007E RID: 126
		// (add) Token: 0x06004D3D RID: 19773 RVA: 0x001DFBF4 File Offset: 0x001DDDF4
		// (remove) Token: 0x06004D3E RID: 19774 RVA: 0x001DFC2C File Offset: 0x001DDE2C
		public event Action<GraphicsSettingsSO> OnQualityChanged;

		// Token: 0x1400007F RID: 127
		// (add) Token: 0x06004D3F RID: 19775 RVA: 0x001DFC64 File Offset: 0x001DDE64
		// (remove) Token: 0x06004D40 RID: 19776 RVA: 0x001DFC9C File Offset: 0x001DDE9C
		public event Action<float> OnRenderScaleChanged;

		// Token: 0x14000080 RID: 128
		// (add) Token: 0x06004D41 RID: 19777 RVA: 0x001DFCD4 File Offset: 0x001DDED4
		// (remove) Token: 0x06004D42 RID: 19778 RVA: 0x001DFD0C File Offset: 0x001DDF0C
		public event Action<GraphicsSettingsModel> OnModelApplied;

		// Token: 0x17000D07 RID: 3335
		// (get) Token: 0x06004D43 RID: 19779 RVA: 0x001DFD44 File Offset: 0x001DDF44
		public static bool IsStartupPresentationActive
		{
			get
			{
				return SceneManager.GetSceneByName("GameWarmUp").isLoaded;
			}
		}

		// Token: 0x06004D44 RID: 19780 RVA: 0x001DFD63 File Offset: 0x001DDF63
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void ApplyStartupFramePacing()
		{
			GraphicsSettingsApplier.ApplyFramePacing(GraphicsSettingsModel.FromJson(PlayerPrefs.GetString("Graphics_ModelJson_v2", null)) ?? new GraphicsSettingsModel(), true);
		}

		// Token: 0x17000D08 RID: 3336
		// (get) Token: 0x06004D45 RID: 19781 RVA: 0x001DFD84 File Offset: 0x001DDF84
		public GraphicsSettingsModel Model
		{
			get
			{
				return this._model.Clone();
			}
		}

		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x06004D46 RID: 19782 RVA: 0x001DFD94 File Offset: 0x001DDF94
		public GraphicsSettingsSO CurrentProfile
		{
			get
			{
				foreach (GraphicsSettingsSO graphicsSettingsSO in this.AllProfilesTyped())
				{
					if (graphicsSettingsSO != null && this._model.QualityEquals(graphicsSettingsSO.model))
					{
						return graphicsSettingsSO;
					}
				}
				return null;
			}
		}

		// Token: 0x17000D0A RID: 3338
		// (get) Token: 0x06004D47 RID: 19783 RVA: 0x001DFE00 File Offset: 0x001DE000
		public GraphicsSettingsSO.QualityLevel CurrentQualityLevel
		{
			get
			{
				GraphicsSettingsSO currentProfile = this.CurrentProfile;
				if (currentProfile == null)
				{
					return GraphicsSettingsSO.QualityLevel.Medium;
				}
				return currentProfile.qualityLevel;
			}
		}

		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x06004D48 RID: 19784 RVA: 0x001DFE13 File Offset: 0x001DE013
		public int CurrentQualityIndex
		{
			get
			{
				if (!(this.CurrentProfile != null))
				{
					return 4;
				}
				return (int)this.CurrentProfile.qualityLevel;
			}
		}

		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x06004D49 RID: 19785 RVA: 0x001DFE30 File Offset: 0x001DE030
		public string CurrentPresetName
		{
			get
			{
				if (!(this.CurrentProfile != null))
				{
					return "Custom";
				}
				return this.CurrentProfile.displayName;
			}
		}

		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x06004D4A RID: 19786 RVA: 0x001DFE51 File Offset: 0x001DE051
		public UniversalRenderPipelineAsset RuntimePipeline
		{
			get
			{
				return this._runtimeAsset;
			}
		}

		// Token: 0x06004D4B RID: 19787 RVA: 0x001DFE5C File Offset: 0x001DE05C
		private void Awake()
		{
			if (GraphicsSettingsController.Instance != null && GraphicsSettingsController.Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			GraphicsSettingsController.Instance = this;
			if (base.transform.parent != null)
			{
				base.transform.SetParent(null, true);
			}
			Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x06004D4C RID: 19788 RVA: 0x001DFEC0 File Offset: 0x001DE0C0
		private void Start()
		{
			this.Initialize();
		}

		// Token: 0x06004D4D RID: 19789 RVA: 0x001DFEC8 File Offset: 0x001DE0C8
		private void OnEnable()
		{
			SceneManager.sceneLoaded += this.HandleSceneLoaded;
			SceneManager.sceneUnloaded += this.HandleSceneUnloaded;
		}

		// Token: 0x06004D4E RID: 19790 RVA: 0x001DFEEC File Offset: 0x001DE0EC
		private void OnDisable()
		{
			SceneManager.sceneLoaded -= this.HandleSceneLoaded;
			SceneManager.sceneUnloaded -= this.HandleSceneUnloaded;
			this.UnhookChunkDecalUpdates();
		}

		// Token: 0x06004D4F RID: 19791 RVA: 0x001DFF18 File Offset: 0x001DE118
		public void Initialize()
		{
			if (this._initialized)
			{
				return;
			}
			this._initialized = true;
			this.SetupRuntimePipeline();
			this._model = this.LoadSavedModel() ?? this.BuildInitialModel();
			this.ApplyAll(true);
			GameSettingsService.ApplyLowGraphicsHudDefault(this.CurrentProfile != null && this.CurrentProfile.qualityLevel == GraphicsSettingsSO.QualityLevel.Low, false);
		}

		// Token: 0x06004D50 RID: 19792 RVA: 0x001DFF7D File Offset: 0x001DE17D
		public void UpdateModel(Action<GraphicsSettingsModel> mutate, bool applyDisplay = false)
		{
			if (mutate == null)
			{
				return;
			}
			mutate(this._model);
			this.ApplyAll(applyDisplay);
			this.Persist();
		}

		// Token: 0x06004D51 RID: 19793 RVA: 0x001DFF9C File Offset: 0x001DE19C
		public void SetQualityLevel(GraphicsSettingsSO.QualityLevel level)
		{
			GraphicsSettingsSO profileForLevel = this.GetProfileForLevel(level);
			if (profileForLevel == null)
			{
				Debug.LogWarning(string.Format("[GraphicsSettings] Profile not found for level: {0}", level));
				return;
			}
			this.ApplyProfile(profileForLevel);
		}

		// Token: 0x06004D52 RID: 19794 RVA: 0x001DFFD7 File Offset: 0x001DE1D7
		public void ApplyProfile(GraphicsSettingsSO profile)
		{
			if (profile == null)
			{
				return;
			}
			this._model.CopyQualityFrom(profile.model);
			this.ApplyAll(false);
			this.Persist();
			GameSettingsService.ApplyLowGraphicsHudDefault(profile.qualityLevel == GraphicsSettingsSO.QualityLevel.Low, true);
		}

		// Token: 0x06004D53 RID: 19795 RVA: 0x001E0010 File Offset: 0x001DE210
		public void SetRenderScale(float scale)
		{
			this.UpdateModel(delegate(GraphicsSettingsModel m)
			{
				m.renderScale = Mathf.Clamp(scale, 0.5f, 2f);
			}, false);
		}

		// Token: 0x06004D54 RID: 19796 RVA: 0x001E0040 File Offset: 0x001DE240
		public void SetVSync(bool enabled)
		{
			this.UpdateModel(delegate(GraphicsSettingsModel m)
			{
				m.SetVerticalSync(enabled);
			}, false);
		}

		// Token: 0x06004D55 RID: 19797 RVA: 0x001E0070 File Offset: 0x001DE270
		public void SetTargetFrameRate(int fps)
		{
			this.UpdateModel(delegate(GraphicsSettingsModel m)
			{
				m.SetFrameRateLimit(fps);
			}, false);
		}

		// Token: 0x06004D56 RID: 19798 RVA: 0x001E009D File Offset: 0x001DE29D
		public void RefreshCurrentProfile()
		{
			this.ApplyAll(false);
		}

		// Token: 0x06004D57 RID: 19799 RVA: 0x001E00A8 File Offset: 0x001DE2A8
		public string GetCurrentProfileDescription()
		{
			GraphicsSettingsSO currentProfile = this.CurrentProfile;
			if (!(currentProfile != null))
			{
				return "Custom";
			}
			return currentProfile.displayName + ": " + currentProfile.description;
		}

		// Token: 0x06004D58 RID: 19800 RVA: 0x001E00E1 File Offset: 0x001DE2E1
		public void ResetToDefaults()
		{
			PlayerPrefs.DeleteKey("Graphics_ModelJson_v2");
			PlayerPrefs.DeleteKey("Graphics_QualityLevel");
			PlayerPrefs.Save();
			this._model = this.BuildInitialModel();
			this.ApplyAll(true);
		}

		// Token: 0x06004D59 RID: 19801 RVA: 0x001E0110 File Offset: 0x001DE310
		public void ApplyDisplayWithBackup(int width, int height, FullScreenMode mode)
		{
			this._displayBackup = this._model.Clone();
			this._model.resolutionWidth = width;
			this._model.resolutionHeight = height;
			this._model.fullScreenMode = (int)mode;
			GraphicsSettingsApplier.ApplyDisplay(this._model);
		}

		// Token: 0x06004D5A RID: 19802 RVA: 0x001E015D File Offset: 0x001DE35D
		public void CommitDisplay()
		{
			this._displayBackup = null;
			this.Persist();
		}

		// Token: 0x06004D5B RID: 19803 RVA: 0x001E016C File Offset: 0x001DE36C
		public void RevertDisplay()
		{
			if (this._displayBackup == null)
			{
				return;
			}
			this._model.resolutionWidth = this._displayBackup.resolutionWidth;
			this._model.resolutionHeight = this._displayBackup.resolutionHeight;
			this._model.fullScreenMode = this._displayBackup.fullScreenMode;
			this._displayBackup = null;
			GraphicsSettingsApplier.ApplyDisplay(this._model);
			this.Persist();
		}

		// Token: 0x06004D5C RID: 19804 RVA: 0x001E01DC File Offset: 0x001DE3DC
		private void ApplyAll(bool applyDisplay)
		{
			this._model.brightness = GraphicsSettingsModel.SanitizeBrightness(this._model.brightness);
			if (this._runtimeAsset != null && QualitySettings.renderPipeline != this._runtimeAsset)
			{
				QualitySettings.renderPipeline = this._runtimeAsset;
			}
			string presentationAwareSceneName = GraphicsSettingsController.GetPresentationAwareSceneName();
			this.ApplyPipelineForScene(presentationAwareSceneName);
			GraphicsSettingsApplier.ApplyEngineQuality(this._model, GraphicsSettingsController.IsStartupPresentationActive);
			GraphicsSettingsApplier.ApplyRendererFeatures(this._model, this._runtimeRendererFeatures);
			GraphicsSettingsApplier.ApplyDecalProjectors(this._model, null);
			GraphicsSettingsApplier.ApplyCamera(this._model, Camera.main);
			ScenePostFxQualityService.Apply(this._model, presentationAwareSceneName);
			ClothQualityService.Apply(this._model.magicaClothEnabled);
			if (applyDisplay)
			{
				GraphicsSettingsApplier.ApplyDisplay(this._model);
			}
			base.StopCoroutine("ApplyWorldWhenReady");
			base.StartCoroutine("ApplyWorldWhenReady");
			Action<GraphicsSettingsModel> onModelApplied = this.OnModelApplied;
			if (onModelApplied != null)
			{
				onModelApplied(this._model);
			}
			Action<GraphicsSettingsSO> onQualityChanged = this.OnQualityChanged;
			if (onQualityChanged != null)
			{
				onQualityChanged(this.CurrentProfile);
			}
			Action<float> onRenderScaleChanged = this.OnRenderScaleChanged;
			if (onRenderScaleChanged == null)
			{
				return;
			}
			onRenderScaleChanged(this._model.renderScale);
		}

		// Token: 0x06004D5D RID: 19805 RVA: 0x001E0303 File Offset: 0x001DE503
		private IEnumerator ApplyWorldWhenReady()
		{
			this.PruneDestroyedChunkManager();
			int num;
			for (int i = 0; i < 300; i = num + 1)
			{
				if (ChunkManager.Instance != null)
				{
					this.HookChunkDecalUpdates(ChunkManager.Instance);
					GraphicsSettingsApplier.ApplyDecalProjectors(this._model, null);
					this.HookWorldApply();
					yield break;
				}
				yield return null;
				this.PruneDestroyedChunkManager();
				num = i;
			}
			yield break;
		}

		// Token: 0x06004D5E RID: 19806 RVA: 0x001E0312 File Offset: 0x001DE512
		private void HookWorldApply()
		{
			GraphicsSettingsApplier.ApplyWorld(this._model);
		}

		// Token: 0x06004D5F RID: 19807 RVA: 0x001E031F File Offset: 0x001DE51F
		public void ReapplySceneRendering()
		{
			GraphicsSettingsApplier.ApplyCamera(this._model, Camera.main);
			ScenePostFxQualityService.Apply(this._model, GraphicsSettingsController.GetPresentationAwareSceneName());
			GraphicsSettingsApplier.ApplyWorld(this._model);
		}

		// Token: 0x06004D60 RID: 19808 RVA: 0x001E034C File Offset: 0x001DE54C
		public void ReapplyWorld()
		{
			this.ReapplySceneRendering();
		}

		// Token: 0x06004D61 RID: 19809 RVA: 0x001E0354 File Offset: 0x001DE554
		private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (!this._initialized)
			{
				return;
			}
			GraphicsSettingsApplier.ApplyFramePacing(this._model, GraphicsSettingsController.IsStartupPresentationActive);
			if (this._runtimeAsset != null && QualitySettings.renderPipeline != this._runtimeAsset)
			{
				QualitySettings.renderPipeline = this._runtimeAsset;
			}
			string presentationAwareSceneName = GraphicsSettingsController.GetPresentationAwareSceneName();
			this.ApplyPipelineForScene(presentationAwareSceneName);
			GraphicsSettingsApplier.ApplyRendererFeatures(this._model, this._runtimeRendererFeatures);
			GraphicsSettingsApplier.ApplyDecalProjectors(this._model, null);
			GraphicsSettingsApplier.ApplyCamera(this._model, Camera.main);
			ScenePostFxQualityService.Apply(this._model, presentationAwareSceneName);
			base.StopCoroutine("ApplyWorldWhenReady");
			base.StartCoroutine("ApplyWorldWhenReady");
		}

		// Token: 0x06004D62 RID: 19810 RVA: 0x001E0404 File Offset: 0x001DE604
		private void HandleSceneUnloaded(Scene scene)
		{
			this.PruneDestroyedChunkManager();
			if (this._initialized && string.Equals(scene.name, "GameWarmUp", StringComparison.Ordinal))
			{
				GraphicsSettingsApplier.ApplyFramePacing(this._model, false);
			}
			if (!this._initialized || !string.Equals(scene.name, "MainMenu", StringComparison.Ordinal))
			{
				return;
			}
			string presentationAwareSceneName = GraphicsSettingsController.GetPresentationAwareSceneName();
			this.ApplyPipelineForScene(presentationAwareSceneName);
			GraphicsSettingsApplier.ApplyCamera(this._model, Camera.main);
			ScenePostFxQualityService.Apply(this._model, presentationAwareSceneName);
		}

		// Token: 0x06004D63 RID: 19811 RVA: 0x001E0488 File Offset: 0x001DE688
		private void ApplyPipelineForScene(string sceneName)
		{
			bool flag = string.Equals(sceneName, "MainMenu", StringComparison.Ordinal);
			GraphicsSettingsApplier.ApplyPipeline(this._model, this._runtimeAsset, flag);
		}

		// Token: 0x06004D64 RID: 19812 RVA: 0x001E04B4 File Offset: 0x001DE6B4
		private static string GetPresentationAwareSceneName()
		{
			Scene sceneByName = SceneManager.GetSceneByName("MainMenu");
			if (sceneByName.IsValid() && sceneByName.isLoaded)
			{
				return "MainMenu";
			}
			return SceneManager.GetActiveScene().name;
		}

		// Token: 0x06004D65 RID: 19813 RVA: 0x001E04F1 File Offset: 0x001DE6F1
		private void PruneDestroyedChunkManager()
		{
			if (this._decalChunkManager != null && this._decalChunkManager == null)
			{
				this.UnhookChunkDecalUpdates();
			}
		}

		// Token: 0x06004D66 RID: 19814 RVA: 0x001E0510 File Offset: 0x001DE710
		private void HookChunkDecalUpdates(ChunkManager manager)
		{
			if (this._decalChunkManager == manager)
			{
				return;
			}
			this.UnhookChunkDecalUpdates();
			this._decalChunkManager = manager;
			if (this._decalChunkManager != null)
			{
				this._decalChunkManager.OnChunkZonesReady += this.HandleChunkZonesReady;
			}
		}

		// Token: 0x06004D67 RID: 19815 RVA: 0x001E055E File Offset: 0x001DE75E
		private void UnhookChunkDecalUpdates()
		{
			if (this._decalChunkManager != null)
			{
				this._decalChunkManager.OnChunkZonesReady -= this.HandleChunkZonesReady;
			}
			this._decalChunkManager = null;
		}

		// Token: 0x06004D68 RID: 19816 RVA: 0x001E058C File Offset: 0x001DE78C
		private void HandleChunkZonesReady(ChunkInstance chunk)
		{
			if (chunk == null)
			{
				return;
			}
			ChunkContentRootTraversal.Collect(chunk, true, this._decalChunkRootScratch);
			for (int i = 0; i < this._decalChunkRootScratch.Count; i++)
			{
				GraphicsSettingsApplier.ApplyDecalProjectors(this._model, this._decalChunkRootScratch[i]);
				GraphicsSettingsApplier.ApplyTerrainQuality(this._model, Camera.main, this._decalChunkRootScratch[i]);
			}
			this._decalChunkRootScratch.Clear();
		}

		// Token: 0x06004D69 RID: 19817 RVA: 0x001E0600 File Offset: 0x001DE800
		private void SetupRuntimePipeline()
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = ((this.basePipelineAsset != null) ? this.basePipelineAsset : (GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset));
			if (universalRenderPipelineAsset == null)
			{
				Debug.LogError("[GraphicsSettings] Nessun UniversalRenderPipelineAsset disponibile.");
				return;
			}
			this._originalQualityPipeline = QualitySettings.renderPipeline;
			this._runtimeAsset = Object.Instantiate<UniversalRenderPipelineAsset>(universalRenderPipelineAsset);
			this._runtimeAsset.name = universalRenderPipelineAsset.name + " (Runtime)";
			if (!(GraphicsSettingsController.RendererDataListField == null))
			{
				ScriptableRendererData[] array = GraphicsSettingsController.RendererDataListField.GetValue(universalRenderPipelineAsset) as ScriptableRendererData[];
				if (array != null)
				{
					ScriptableRendererData[] array2 = new ScriptableRendererData[array.Length];
					List<ScriptableRendererFeature> list = new List<ScriptableRendererFeature>();
					int worldRendererIndex = GraphicsSettingsController.GetWorldRendererIndex(universalRenderPipelineAsset);
					for (int i = 0; i < array.Length; i++)
					{
						ScriptableRendererData scriptableRendererData = array[i];
						if (!(scriptableRendererData == null))
						{
							ScriptableRendererData scriptableRendererData2 = Object.Instantiate<ScriptableRendererData>(scriptableRendererData);
							scriptableRendererData2.name = scriptableRendererData.name + " (Runtime)";
							array2[i] = scriptableRendererData2;
							UniversalRendererData universalRendererData = scriptableRendererData2 as UniversalRendererData;
							if (universalRendererData != null)
							{
								if (i == worldRendererIndex)
								{
									universalRendererData.renderingMode = RenderingMode.ForwardPlus;
								}
								universalRendererData.intermediateTextureMode = IntermediateTextureMode.Auto;
							}
							List<ScriptableRendererFeature> rendererFeatures = scriptableRendererData2.rendererFeatures;
							for (int j = 0; j < rendererFeatures.Count; j++)
							{
								ScriptableRendererFeature scriptableRendererFeature = rendererFeatures[j];
								if (!(scriptableRendererFeature == null))
								{
									ScriptableRendererFeature scriptableRendererFeature2 = Object.Instantiate<ScriptableRendererFeature>(scriptableRendererFeature);
									scriptableRendererFeature2.name = scriptableRendererFeature.name + " (Runtime)";
									rendererFeatures[j] = scriptableRendererFeature2;
									list.Add(scriptableRendererFeature2);
								}
							}
							scriptableRendererData2.SetDirty();
						}
					}
					GraphicsSettingsController.RendererDataListField.SetValue(this._runtimeAsset, array2);
					this._runtimeRendererData = array2;
					this._runtimeRendererFeatures = list.ToArray();
					QualitySettings.renderPipeline = this._runtimeAsset;
					return;
				}
			}
			Debug.LogError("[GraphicsSettings] Impossibile isolare i Renderer Data URP runtime.");
			QualitySettings.renderPipeline = this._runtimeAsset;
		}

		// Token: 0x06004D6A RID: 19818 RVA: 0x001E07D5 File Offset: 0x001DE9D5
		private static int GetWorldRendererIndex(UniversalRenderPipelineAsset pipeline)
		{
			if (!(GraphicsSettingsController.DefaultRendererIndexField != null))
			{
				return 0;
			}
			return (int)GraphicsSettingsController.DefaultRendererIndexField.GetValue(pipeline);
		}

		// Token: 0x06004D6B RID: 19819 RVA: 0x001E07F8 File Offset: 0x001DE9F8
		[Conditional("UNITY_EDITOR")]
		[Conditional("DEVELOPMENT_BUILD")]
		private void ValidateAppliedRuntimeState()
		{
			if (this._runtimeAsset == null)
			{
				return;
			}
			List<string> runtimeValidationFailures = this.GetRuntimeValidationFailures();
			if (runtimeValidationFailures.Count > 0)
			{
				Debug.LogError("[GraphicsSettings] Runtime validation FAILED: " + string.Join(", ", runtimeValidationFailures));
				return;
			}
			string text = string.Format("{0}|{1:F2}|", this.CurrentPresetName, this._runtimeAsset.renderScale) + string.Format("{0}|{1}|{2}", this._runtimeAsset.maxAdditionalLightsCount, this._model.ssao, this._model.decals);
			if (this._lastRuntimeValidationSignature != text)
			{
				this._lastRuntimeValidationSignature = text;
				Debug.Log(string.Concat(new string[]
				{
					"[GraphicsSettings] Runtime validation OK: ",
					this.CurrentPresetName,
					", ",
					string.Format("scale={0:F2}, world lights=Forward+, ", this._runtimeAsset.renderScale),
					string.Format("opaque={0}, SSAO={1}, decals={2}.", this._runtimeAsset.supportsCameraOpaqueTexture, this._model.ssao, this._model.decals)
				}));
			}
		}

		// Token: 0x06004D6C RID: 19820 RVA: 0x001E093C File Offset: 0x001DEB3C
		public List<string> GetRuntimeValidationFailures()
		{
			List<string> list = new List<string>();
			if (this._runtimeAsset == null)
			{
				list.Add("Runtime pipeline missing");
				return list;
			}
			if (QualitySettings.renderPipeline != this._runtimeAsset)
			{
				list.Add("Active quality pipeline");
			}
			if (!Mathf.Approximately(this._runtimeAsset.renderScale, Mathf.Clamp(this._model.renderScale, 0.5f, 2f)))
			{
				list.Add("renderScale");
			}
			UpscalingFilterSelection upscalingFilterSelection = ((!(GraphicsSettingsController.GetPresentationAwareSceneName() == "MainMenu") && this._model.upscaling == GraphicsSettingsModel.UpscalingMode.FSR) ? UpscalingFilterSelection.FSR : UpscalingFilterSelection.Linear);
			if (this._runtimeAsset.upscalingFilter != upscalingFilterSelection)
			{
				list.Add("Upscaling");
			}
			if (!this._runtimeAsset.fsrOverrideSharpness || !Mathf.Approximately(this._runtimeAsset.fsrSharpness, Mathf.Clamp01(this._model.fsrSharpness)))
			{
				list.Add("FSR sharpness");
			}
			if (this._runtimeAsset.supportsHDR != this._model.hdr)
			{
				list.Add("HDR");
			}
			if (this._runtimeAsset.supportsCameraOpaqueTexture != this._model.opaqueTexture)
			{
				list.Add("OpaqueTexture");
			}
			if (this._runtimeAsset.msaaSampleCount != GraphicsSettingsApplier.SanitizeMsaa(this._model.msaa))
			{
				list.Add("MSAA");
			}
			if (this._runtimeAsset.maxAdditionalLightsCount != Mathf.Clamp(this._model.additionalLightsPerObject, 1, 8))
			{
				list.Add("AdditionalLights");
			}
			if (this._runtimeAsset.additionalLightsRenderingMode != LightRenderingMode.PerPixel)
			{
				list.Add("Fixture lighting");
			}
			if (this._runtimeAsset.supportsAdditionalLightShadows)
			{
				list.Add("Additional-light shadows");
			}
			if (!Mathf.Approximately(this._runtimeAsset.shadowDistance, this._model.shadowsEnabled ? Mathf.Clamp(this._model.shadowDistance, 0f, 500f) : 0f))
			{
				list.Add("Shadow distance");
			}
			if (this._runtimeAsset.shadowCascadeCount != Mathf.Clamp(this._model.shadowCascades, 1, 4))
			{
				list.Add("Shadow cascades");
			}
			if (this._runtimeAsset.mainLightShadowmapResolution != Mathf.ClosestPowerOfTwo(Mathf.Clamp(this._model.mainShadowResolution, 512, 4096)))
			{
				list.Add("Shadow resolution");
			}
			if (this._runtimeAsset.supportsSoftShadows != this._model.softShadows)
			{
				list.Add("Soft shadows");
			}
			if (QualitySettings.globalTextureMipmapLimit != 3 - Mathf.Clamp(this._model.textureQuality, 0, 3))
			{
				list.Add("Texture quality");
			}
			if (!Mathf.Approximately(QualitySettings.lodBias, Mathf.Clamp(this._model.lodBias, 0.25f, 4f)))
			{
				list.Add("LOD bias");
			}
			if (QualitySettings.realtimeReflectionProbes != this._model.realtimeReflectionProbes)
			{
				list.Add("Reflection probes");
			}
			AnisotropicFiltering anisotropicFiltering = ((this._model.anisotropic == GraphicsSettingsModel.AnisoMode.Disabled) ? AnisotropicFiltering.Disable : ((this._model.anisotropic == GraphicsSettingsModel.AnisoMode.PerTexture) ? AnisotropicFiltering.Enable : AnisotropicFiltering.ForceEnable));
			if (QualitySettings.anisotropicFiltering != anisotropicFiltering)
			{
				list.Add("Anisotropic filtering");
			}
			if (QualitySettings.vSyncCount != GraphicsSettingsApplier.GetVSyncCount(this._model, GraphicsSettingsController.IsStartupPresentationActive))
			{
				list.Add("VSync");
			}
			if (Application.targetFrameRate != GraphicsSettingsApplier.GetTargetFrameRate(this._model, GraphicsSettingsController.IsStartupPresentationActive))
			{
				list.Add("FPS cap");
			}
			if (ClothQualityService.Enabled != this._model.magicaClothEnabled)
			{
				list.Add("Cloth switch");
			}
			int worldRendererIndex = GraphicsSettingsController.GetWorldRendererIndex(this._runtimeAsset);
			if (worldRendererIndex >= 0 && worldRendererIndex < this._runtimeRendererData.Length)
			{
				UniversalRendererData universalRendererData = this._runtimeRendererData[worldRendererIndex] as UniversalRendererData;
				if (universalRendererData != null && universalRendererData.renderingMode == RenderingMode.ForwardPlus)
				{
					goto IL_03D2;
				}
			}
			list.Add("World renderer is not Forward+");
			IL_03D2:
			for (int i = 0; i < this._runtimeRendererFeatures.Length; i++)
			{
				ScriptableRendererFeature scriptableRendererFeature = this._runtimeRendererFeatures[i];
				if (!(scriptableRendererFeature == null))
				{
					string name = scriptableRendererFeature.GetType().Name;
					if (name.IndexOf("ScreenSpaceAmbientOcclusion", StringComparison.OrdinalIgnoreCase) >= 0 && scriptableRendererFeature.isActive != this._model.ssao)
					{
						list.Add("SSAO");
					}
					if (name.IndexOf("DecalRendererFeature", StringComparison.OrdinalIgnoreCase) >= 0 && !scriptableRendererFeature.isActive)
					{
						list.Add("DecalFeature");
					}
				}
			}
			Camera main = Camera.main;
			UniversalAdditionalCameraData universalAdditionalCameraData;
			if (main != null && main.TryGetComponent<UniversalAdditionalCameraData>(out universalAdditionalCameraData))
			{
				bool flag = !CharacterPreviewRendering.IsTransparentPreview(main) && GraphicsSettingsApplier.RequiresPostProcessingPasses(this._model, main.gameObject.scene.name == "MainMenu");
				if (universalAdditionalCameraData.renderPostProcessing != flag)
				{
					list.Add("Camera post/AA/upscale passes");
				}
			}
			return list;
		}

		// Token: 0x06004D6D RID: 19821 RVA: 0x00004F31 File Offset: 0x00003131
		private void RestoreEditorState()
		{
		}

		// Token: 0x06004D6E RID: 19822 RVA: 0x001E0E11 File Offset: 0x001DF011
		private void Persist()
		{
			PlayerPrefs.SetString("Graphics_ModelJson_v2", this._model.ToJson());
			PlayerPrefs.Save();
		}

		// Token: 0x06004D6F RID: 19823 RVA: 0x001E0E30 File Offset: 0x001DF030
		private GraphicsSettingsModel LoadSavedModel()
		{
			GraphicsSettingsModel graphicsSettingsModel = GraphicsSettingsModel.FromJson(PlayerPrefs.GetString("Graphics_ModelJson_v2", null));
			if (graphicsSettingsModel != null)
			{
				return graphicsSettingsModel;
			}
			int @int = PlayerPrefs.GetInt("Graphics_QualityLevel", -1);
			if (@int >= 0)
			{
				GraphicsSettingsSO profileForLevel = this.GetProfileForLevel((GraphicsSettingsSO.QualityLevel)@int);
				if (profileForLevel != null)
				{
					GraphicsSettingsModel graphicsSettingsModel2 = new GraphicsSettingsModel();
					graphicsSettingsModel2.CopyQualityFrom(profileForLevel.model);
					return graphicsSettingsModel2;
				}
			}
			return null;
		}

		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x06004D70 RID: 19824 RVA: 0x001E0E87 File Offset: 0x001DF087
		private static int SystemMemoryMB
		{
			get
			{
				if (GraphicsSettingsController._systemMemoryOverrideMB <= 0)
				{
					return EngineMemorySettings.SystemMemoryMB;
				}
				return GraphicsSettingsController._systemMemoryOverrideMB;
			}
		}

		// Token: 0x06004D71 RID: 19825 RVA: 0x001E0E9C File Offset: 0x001DF09C
		private GraphicsSettingsModel BuildInitialModel()
		{
			GraphicsSettingsSO graphicsSettingsSO = (this.autoDetectOnStart ? this.AutoDetectQualityLevel() : (this.defaultProfile ?? this.mediumProfile));
			int systemMemoryMB = GraphicsSettingsController.SystemMemoryMB;
			bool flag = this.autoDetectOnStart && systemMemoryMB < 24576;
			if (flag && systemMemoryMB < 12288 && this.lowProfile != null)
			{
				graphicsSettingsSO = this.lowProfile;
			}
			GraphicsSettingsModel graphicsSettingsModel = new GraphicsSettingsModel();
			if (graphicsSettingsSO != null)
			{
				graphicsSettingsModel.CopyQualityFrom(graphicsSettingsSO.model);
			}
			if (flag)
			{
				graphicsSettingsModel.textureQuality = Mathf.Min(graphicsSettingsModel.textureQuality, 1);
				Debug.Log(string.Format("[GraphicsSettings] First launch with {0} MB RAM: ", systemMemoryMB) + string.Format("{0} preset, texture quality {1}.", (graphicsSettingsSO != null) ? graphicsSettingsSO.displayName : "default", graphicsSettingsModel.textureQuality));
			}
			return graphicsSettingsModel;
		}

		// Token: 0x06004D72 RID: 19826 RVA: 0x001E0F78 File Offset: 0x001DF178
		public GraphicsSettingsSO AutoDetectQualityLevel()
		{
			int graphicsMemorySize = SystemInfo.graphicsMemorySize;
			int systemMemoryMB = GraphicsSettingsController.SystemMemoryMB;
			int graphicsShaderLevel = SystemInfo.graphicsShaderLevel;
			bool isMobilePlatform = Application.isMobilePlatform;
			bool flag = graphicsMemorySize < 4096 || systemMemoryMB < 8192 || graphicsShaderLevel < 45;
			bool flag2 = graphicsMemorySize >= 8192 && systemMemoryMB >= 16384 && graphicsShaderLevel >= 50;
			if (flag || isMobilePlatform)
			{
				GraphicsSettingsSO graphicsSettingsSO;
				if ((graphicsSettingsSO = this.lowProfile) == null)
				{
					graphicsSettingsSO = this.defaultProfile ?? this.mediumProfile;
				}
				return graphicsSettingsSO;
			}
			if (flag2)
			{
				GraphicsSettingsSO graphicsSettingsSO2;
				if ((graphicsSettingsSO2 = this.highProfile) == null)
				{
					graphicsSettingsSO2 = this.defaultProfile ?? this.mediumProfile;
				}
				return graphicsSettingsSO2;
			}
			return this.mediumProfile ?? this.defaultProfile;
		}

		// Token: 0x06004D73 RID: 19827 RVA: 0x001E1020 File Offset: 0x001DF220
		public GraphicsSettingsSO GetProfileForLevel(GraphicsSettingsSO.QualityLevel level)
		{
			GraphicsSettingsSO graphicsSettingsSO;
			switch (level)
			{
			case GraphicsSettingsSO.QualityLevel.Low:
				graphicsSettingsSO = this.lowProfile;
				break;
			case GraphicsSettingsSO.QualityLevel.Medium:
				graphicsSettingsSO = this.mediumProfile;
				break;
			case GraphicsSettingsSO.QualityLevel.High:
				graphicsSettingsSO = this.highProfile;
				break;
			case GraphicsSettingsSO.QualityLevel.Ultra:
				graphicsSettingsSO = this.ultraProfile;
				break;
			default:
				graphicsSettingsSO = this.mediumProfile;
				break;
			}
			return graphicsSettingsSO;
		}

		// Token: 0x06004D74 RID: 19828 RVA: 0x001E1071 File Offset: 0x001DF271
		private IEnumerable<GraphicsSettingsSO> AllProfilesTyped()
		{
			yield return this.lowProfile;
			yield return this.mediumProfile;
			yield return this.highProfile;
			yield return this.ultraProfile;
			yield break;
		}

		// Token: 0x06004D75 RID: 19829 RVA: 0x001E1081 File Offset: 0x001DF281
		private void OnApplicationQuit()
		{
			this.RestoreEditorState();
		}

		// Token: 0x06004D76 RID: 19830 RVA: 0x001E1089 File Offset: 0x001DF289
		private void OnDestroy()
		{
			if (GraphicsSettingsController.Instance == this)
			{
				this.RestoreEditorState();
				GraphicsSettingsController.Instance = null;
			}
		}

		// Token: 0x04004EAD RID: 20141
		[Header("Profiles")]
		[SerializeField]
		private GraphicsSettingsSO lowProfile;

		// Token: 0x04004EAE RID: 20142
		[SerializeField]
		private GraphicsSettingsSO mediumProfile;

		// Token: 0x04004EAF RID: 20143
		[SerializeField]
		private GraphicsSettingsSO highProfile;

		// Token: 0x04004EB0 RID: 20144
		[SerializeField]
		private GraphicsSettingsSO ultraProfile;

		// Token: 0x04004EB1 RID: 20145
		[Header("Default & Auto")]
		[Tooltip("Profilo da applicare all'avvio se non c'è preferenza salvata")]
		[SerializeField]
		private GraphicsSettingsSO defaultProfile;

		// Token: 0x04004EB2 RID: 20146
		[Tooltip("Rileva automaticamente il profilo migliore basato sull'hardware")]
		[SerializeField]
		private bool autoDetectOnStart = true;

		// Token: 0x04004EB3 RID: 20147
		[Header("Pipeline")]
		[Tooltip("Asset URP base da clonare al boot. Se null usa la pipeline corrente.")]
		[SerializeField]
		private UniversalRenderPipelineAsset basePipelineAsset;

		// Token: 0x04004EB4 RID: 20148
		[Tooltip("Riferimenti legacy. Le feature operative vengono clonate insieme al renderer.")]
		[SerializeField]
		private ScriptableRendererFeature[] ssaoFeatures;

		// Token: 0x04004EB8 RID: 20152
		private const string PREFS_MODEL_JSON = "Graphics_ModelJson_v2";

		// Token: 0x04004EB9 RID: 20153
		private const string PREFS_QUALITY_LEVEL = "Graphics_QualityLevel";

		// Token: 0x04004EBA RID: 20154
		private GraphicsSettingsModel _model = new GraphicsSettingsModel();

		// Token: 0x04004EBB RID: 20155
		private UniversalRenderPipelineAsset _runtimeAsset;

		// Token: 0x04004EBC RID: 20156
		private ScriptableRendererData[] _runtimeRendererData = Array.Empty<ScriptableRendererData>();

		// Token: 0x04004EBD RID: 20157
		private ScriptableRendererFeature[] _runtimeRendererFeatures = Array.Empty<ScriptableRendererFeature>();

		// Token: 0x04004EBE RID: 20158
		private RenderPipelineAsset _originalQualityPipeline;

		// Token: 0x04004EBF RID: 20159
		private GraphicsSettingsModel _displayBackup;

		// Token: 0x04004EC0 RID: 20160
		private bool _initialized;

		// Token: 0x04004EC1 RID: 20161
		private string _lastRuntimeValidationSignature;

		// Token: 0x04004EC2 RID: 20162
		private ChunkManager _decalChunkManager;

		// Token: 0x04004EC3 RID: 20163
		private readonly List<GameObject> _decalChunkRootScratch = new List<GameObject>(8);

		// Token: 0x04004EC4 RID: 20164
		private const string MainMenuSceneName = "MainMenu";

		// Token: 0x04004EC5 RID: 20165
		private const string StartupSceneName = "GameWarmUp";

		// Token: 0x04004EC6 RID: 20166
		private static readonly FieldInfo RendererDataListField = typeof(UniversalRenderPipelineAsset).GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04004EC7 RID: 20167
		private static readonly FieldInfo DefaultRendererIndexField = typeof(UniversalRenderPipelineAsset).GetField("m_DefaultRendererIndex", BindingFlags.Instance | BindingFlags.NonPublic);

		// Token: 0x04004EC8 RID: 20168
		private const int ReducedTextureMemoryMB = 24576;

		// Token: 0x04004EC9 RID: 20169
		private const int LowPresetMemoryMB = 12288;

		// Token: 0x04004ECA RID: 20170
		private static int _systemMemoryOverrideMB;
	}
}
