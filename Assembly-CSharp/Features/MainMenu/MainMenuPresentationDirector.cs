using System;
using System.Collections;
using System.Collections.Generic;
using BoZo.CharacterCreator;
using BoZo.CharacterCreator.MC.StyleSystem;
using DA_Assets.DAB;
using Features.CielVoice;
using Features.Inventory;
using Features.Inventory.Wearable;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Features.MainMenu
{
	// Token: 0x0200106D RID: 4205
	[DefaultExecutionOrder(10000)]
	public sealed class MainMenuPresentationDirector : MonoBehaviour
	{
		// Token: 0x1700147A RID: 5242
		// (get) Token: 0x06007FA9 RID: 32681 RVA: 0x0034205C File Offset: 0x0034025C
		// (set) Token: 0x06007FAA RID: 32682 RVA: 0x00342064 File Offset: 0x00340264
		public bool IsPresentationReady { get; private set; }

		// Token: 0x06007FAB RID: 32683 RVA: 0x0034206D File Offset: 0x0034026D
		private void Awake()
		{
			this.PrepareMenuCharacter();
			this.SetupAudio();
			this.SetupButtons();
			MainMenuPresentationDirector.SetupLogo();
			this.SetupEntranceAnimation();
			this.SetupStageLights();
			MainMenuPresentationDirector.SetupFireflies();
			this.SetupCursorDrivenPresentation();
			base.StartCoroutine(this.EnsureCommonOutfit());
		}

		// Token: 0x06007FAC RID: 32684 RVA: 0x003420AC File Offset: 0x003402AC
		private void Update()
		{
			float num = (this._musicHeldForIntro ? 0.35f : 1f);
			this._musicGain = Mathf.MoveTowards(this._musicGain, num, Time.unscaledDeltaTime / 1.2f);
			CrossfadeLoopPlayer musicLoop = this._musicLoop;
			if (musicLoop != null)
			{
				musicLoop.Tick(this.musicVolume * Mathf.SmoothStep(0f, 1f, this._musicGain));
			}
			float unscaledTime = Time.unscaledTime;
			for (int i = 0; i < this._stageLights.Count; i++)
			{
				MainMenuPresentationDirector.StageLight stageLight = this._stageLights[i];
				if (!(stageLight.light == null))
				{
					stageLight.light.intensity = stageLight.baseIntensity * (1f + Mathf.Sin(unscaledTime * 0.32f + stageLight.phase) * stageLight.amplitude);
				}
			}
		}

		// Token: 0x06007FAD RID: 32685 RVA: 0x00342184 File Offset: 0x00340384
		private void LateUpdate()
		{
			for (int i = 0; i < this._lockedMaskRenderers.Count; i++)
			{
				Renderer renderer = this._lockedMaskRenderers[i];
				if (!(renderer == null))
				{
					renderer.enabled = false;
					renderer.forceRenderingOff = true;
				}
			}
			for (int j = 0; j < this._lockedEyeHeartShapes.Count; j++)
			{
				MainMenuPresentationDirector.LockedBlendShape lockedBlendShape = this._lockedEyeHeartShapes[j];
				if (lockedBlendShape.renderer != null)
				{
					lockedBlendShape.renderer.SetBlendShapeWeight(lockedBlendShape.index, 0f);
				}
			}
		}

		// Token: 0x06007FAE RID: 32686 RVA: 0x00342214 File Offset: 0x00340414
		private void SetupAudio()
		{
			this._musicHeldForIntro = IntroMenuMusic.TryClaim(base.transform, this.mainMenuMusic, out this._musicLoop);
			this._musicGain = (this._musicHeldForIntro ? 0.35f : 0f);
			if (!this._musicHeldForIntro)
			{
				this._musicLoop = new CrossfadeLoopPlayer(this.CreateMusicSource("MainMenuMusic A"), this.CreateMusicSource("MainMenuMusic B"), this.mainMenuMusic, this.loopCrossfadeSeconds, this.loopStartSeconds, this.loopEndSeconds);
			}
			this._hoverSourceA = this.CreateUiSource("Creamy UI Hover A");
			this._hoverSourceB = this.CreateUiSource("Creamy UI Hover B");
			this._uiSource = this.CreateUiSource("Creamy UI Click");
			this._hoverClip = MainMenuPresentationDirector.BuildUiTone("Creamy Hover", 0.105f, false);
			this._clickClip = MainMenuPresentationDirector.BuildUiTone("Creamy Click", 0.17f, true);
		}

		// Token: 0x06007FAF RID: 32687 RVA: 0x003422F8 File Offset: 0x003404F8
		public void ReleaseIntroMusic()
		{
			this._musicHeldForIntro = false;
		}

		// Token: 0x06007FB0 RID: 32688 RVA: 0x00342301 File Offset: 0x00340501
		private void OnEnable()
		{
			if (this._musicLoop != null)
			{
				this._startMusicRoutine = base.StartCoroutine(this.StartMusicWhenReady());
			}
		}

		// Token: 0x06007FB1 RID: 32689 RVA: 0x0034231D File Offset: 0x0034051D
		private void OnDisable()
		{
			if (this._startMusicRoutine != null)
			{
				base.StopCoroutine(this._startMusicRoutine);
				this._startMusicRoutine = null;
			}
			CrossfadeLoopPlayer musicLoop = this._musicLoop;
			if (musicLoop == null)
			{
				return;
			}
			musicLoop.Stop();
		}

		// Token: 0x06007FB2 RID: 32690 RVA: 0x0034234A File Offset: 0x0034054A
		private IEnumerator StartMusicWhenReady()
		{
			if (this.mainMenuMusic == null)
			{
				Debug.LogWarning("[MainMenuAudio] MainMenuMusic is not assigned.", this);
				yield break;
			}
			if (this.mainMenuMusic.loadState == AudioDataLoadState.Unloaded)
			{
				this.mainMenuMusic.LoadAudioData();
			}
			while (base.isActiveAndEnabled && this.mainMenuMusic.loadState == AudioDataLoadState.Loading)
			{
				yield return null;
			}
			if (base.isActiveAndEnabled && this.mainMenuMusic.loadState == AudioDataLoadState.Loaded)
			{
				this._musicLoop.Play();
			}
			else if (this.mainMenuMusic.loadState == AudioDataLoadState.Failed)
			{
				Debug.LogWarning("[MainMenuAudio] MainMenuMusic failed to load.", this);
			}
			this._startMusicRoutine = null;
			yield break;
		}

		// Token: 0x06007FB3 RID: 32691 RVA: 0x0034235C File Offset: 0x0034055C
		private AudioSource CreateMusicSource(string sourceName)
		{
			GameObject gameObject = new GameObject(sourceName);
			gameObject.transform.SetParent(base.transform, false);
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			audioSource.loop = false;
			audioSource.spatialBlend = 0f;
			audioSource.dopplerLevel = 0f;
			audioSource.volume = 0f;
			GameAudioRouter.Route(audioSource, GameAudioRouter.Channel.Music);
			return audioSource;
		}

		// Token: 0x06007FB4 RID: 32692 RVA: 0x003423BC File Offset: 0x003405BC
		private AudioSource CreateUiSource(string sourceName)
		{
			AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
			audioSource.name = sourceName;
			audioSource.playOnAwake = false;
			audioSource.spatialBlend = 0f;
			audioSource.loop = false;
			return audioSource;
		}

		// Token: 0x06007FB5 RID: 32693 RVA: 0x003423EC File Offset: 0x003405EC
		private void SetupButtons()
		{
			Transform transform = MainMenuPresentationDirector.FindTransform("MainMenu/FigmaCanvas/Main Menu - v4/MENU");
			Transform transform2 = MainMenuPresentationDirector.FindTransform("MainMenu/FigmaCanvas/Main Menu - v4/SUPPORT/SOCIALS");
			this.AddFxToChildren(transform);
			this.AddFxToChildren(transform2);
		}

		// Token: 0x06007FB6 RID: 32694 RVA: 0x00342420 File Offset: 0x00340620
		private void AddFxToChildren(Transform root)
		{
			if (root == null)
			{
				return;
			}
			for (int i = 0; i < root.childCount; i++)
			{
				GameObject gameObject = root.GetChild(i).gameObject;
				if (gameObject.activeInHierarchy && gameObject.name.StartsWith("btn -"))
				{
					Button component = gameObject.GetComponent<Button>();
					if (!(component != null) || component.interactable)
					{
						DAButton component2 = gameObject.GetComponent<DAButton>();
						if (!(component2 != null) || component2.Interactable)
						{
							MainMenuButtonFx mainMenuButtonFx = gameObject.GetComponent<MainMenuButtonFx>();
							if (mainMenuButtonFx == null)
							{
								mainMenuButtonFx = gameObject.AddComponent<MainMenuButtonFx>();
							}
							mainMenuButtonFx.Initialize(this);
						}
					}
				}
			}
		}

		// Token: 0x06007FB7 RID: 32695 RVA: 0x003424C4 File Offset: 0x003406C4
		internal void PlayButtonHover()
		{
			if (Time.unscaledTime - this._lastHoverTime < 0.045f || this._hoverClip == null)
			{
				return;
			}
			this._lastHoverTime = Time.unscaledTime;
			AudioSource audioSource = (this._nextHoverUsesA ? this._hoverSourceA : this._hoverSourceB);
			AudioSource audioSource2 = (this._nextHoverUsesA ? this._hoverSourceB : this._hoverSourceA);
			this._nextHoverUsesA = !this._nextHoverUsesA;
			audioSource.Stop();
			audioSource.clip = this._hoverClip;
			audioSource.pitch = Random.Range(0.992f, 1.008f);
			audioSource.volume = 0f;
			audioSource.Play();
			if (this._hoverMixRoutine != null)
			{
				base.StopCoroutine(this._hoverMixRoutine);
			}
			this._hoverMixRoutine = base.StartCoroutine(this.CrossfadeHover(audioSource2, audioSource));
		}

		// Token: 0x06007FB8 RID: 32696 RVA: 0x0034259C File Offset: 0x0034079C
		internal void PlayButtonClick()
		{
			if (this._uiSource == null || this._clickClip == null)
			{
				return;
			}
			this._uiSource.pitch = Random.Range(0.995f, 1.006f);
			this._uiSource.PlayOneShot(this._clickClip, 0.39f * GameSettingsService.SfxVolume);
		}

		// Token: 0x06007FB9 RID: 32697 RVA: 0x003425FC File Offset: 0x003407FC
		private IEnumerator CrossfadeHover(AudioSource outgoing, AudioSource incoming)
		{
			float outgoingStart = ((outgoing != null) ? outgoing.volume : 0f);
			float elapsed = 0f;
			while (elapsed < 0.038f)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / 0.038f));
				if (outgoing != null)
				{
					outgoing.volume = Mathf.Lerp(outgoingStart, 0f, num);
				}
				if (incoming != null)
				{
					incoming.volume = Mathf.Lerp(0f, 0.28f * GameSettingsService.SfxVolume, num);
				}
				yield return null;
			}
			if (outgoing != null)
			{
				outgoing.Stop();
				outgoing.volume = 0f;
			}
			if (incoming != null)
			{
				incoming.volume = 0.28f * GameSettingsService.SfxVolume;
			}
			this._hoverMixRoutine = null;
			yield break;
		}

		// Token: 0x06007FBA RID: 32698 RVA: 0x00342619 File Offset: 0x00340819
		private void SetupCursorDrivenPresentation()
		{
			if (base.GetComponent<MainMenuBackgroundClickParticles>() == null)
			{
				base.gameObject.AddComponent<MainMenuBackgroundClickParticles>();
			}
		}

		// Token: 0x06007FBB RID: 32699 RVA: 0x00342638 File Offset: 0x00340838
		private static void SetupLogo()
		{
			Transform transform = MainMenuPresentationDirector.FindTransform("MainMenu/FigmaCanvas/Main Menu - v4/Logo/Image");
			if (transform != null && transform.GetComponent<MainMenuLogoShimmer>() == null)
			{
				transform.gameObject.AddComponent<MainMenuLogoShimmer>();
			}
		}

		// Token: 0x06007FBC RID: 32700 RVA: 0x00342673 File Offset: 0x00340873
		private void SetupEntranceAnimation()
		{
			if (base.GetComponent<MainMenuEntranceAnimator>() == null)
			{
				base.gameObject.AddComponent<MainMenuEntranceAnimator>();
			}
		}

		// Token: 0x06007FBD RID: 32701 RVA: 0x00342690 File Offset: 0x00340890
		private void SetupStageLights()
		{
			Light light = MainMenuPresentationDirector.ConfigureSpot("CharacterSetup/Spot Light", new Vector3(-1.35f, 3.25f, 2.35f), new Color(1f, 0.64f, 0.48f), 7.2f, 49f, 8.5f, 0.075f, 0f);
			Light light2 = MainMenuPresentationDirector.ConfigureSpot("Enviroment---------------------------------------------------/SpotLightSX", new Vector3(1.95f, 2.8f, 2.1f), new Color(0.94f, 0.38f, 0.67f), 4.2f, 56f, 8f, 0.09f, 2.1f);
			Light light3 = MainMenuPresentationDirector.ConfigureSpot("Enviroment---------------------------------------------------/SpotLightDX", new Vector3(0.65f, 3.45f, -2.15f), new Color(0.48f, 0.57f, 1f), 5.4f, 43f, 9f, 0.11f, 4.2f);
			this.RegisterStageLight(light, 7.2f, 0f, 0.075f);
			this.RegisterStageLight(light2, 4.2f, 2.1f, 0.09f);
			this.RegisterStageLight(light3, 5.4f, 4.2f, 0.11f);
		}

		// Token: 0x06007FBE RID: 32702 RVA: 0x003427C4 File Offset: 0x003409C4
		private static Light ConfigureSpot(string path, Vector3 position, Color color, float intensity, float spotAngle, float range, float pulse, float phase)
		{
			Transform transform = MainMenuPresentationDirector.FindTransform(path);
			if (transform == null)
			{
				return null;
			}
			transform.gameObject.SetActive(true);
			transform.position = position;
			transform.LookAt(new Vector3(0f, 1.25f, 0f));
			Light light = transform.GetComponent<Light>();
			if (light == null)
			{
				light = transform.gameObject.AddComponent<Light>();
			}
			light.type = LightType.Spot;
			light.color = color;
			light.intensity = intensity;
			light.range = range;
			light.spotAngle = spotAngle;
			light.innerSpotAngle = spotAngle * 0.58f;
			light.shadows = LightShadows.Soft;
			light.shadowStrength = 0.48f;
			light.renderMode = LightRenderMode.ForcePixel;
			return light;
		}

		// Token: 0x06007FBF RID: 32703 RVA: 0x00342879 File Offset: 0x00340A79
		private void RegisterStageLight(Light light, float intensity, float phase, float amplitude)
		{
			if (light == null)
			{
				return;
			}
			this._stageLights.Add(new MainMenuPresentationDirector.StageLight
			{
				light = light,
				baseIntensity = intensity,
				phase = phase,
				amplitude = amplitude
			});
		}

		// Token: 0x06007FC0 RID: 32704 RVA: 0x003428B4 File Offset: 0x00340AB4
		private static void SetupFireflies()
		{
			if (GameObject.Find("Menu Fireflies") != null)
			{
				return;
			}
			ParticleSystem particleSystem = new GameObject("Menu Fireflies")
			{
				transform = 
				{
					position = new Vector3(0f, 1.35f, 0f)
				}
			}.AddComponent<ParticleSystem>();
			particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
			ParticleSystem.MainModule main = particleSystem.main;
			main.loop = true;
			main.duration = 8f;
			main.simulationSpace = ParticleSystemSimulationSpace.World;
			main.startLifetime = new ParticleSystem.MinMaxCurve(4.2f, 7.2f);
			main.startSpeed = new ParticleSystem.MinMaxCurve(0.025f, 0.09f);
			main.startSize = new ParticleSystem.MinMaxCurve(0.018f, 0.045f);
			main.startColor = new ParticleSystem.MinMaxGradient(new Color(1f, 0.88f, 0.58f, 0.92f), new Color(1f, 0.72f, 0.86f, 0.84f));
			main.maxParticles = 42;
			particleSystem.emission.rateOverTime = 3.6f;
			ParticleSystem.ShapeModule shape = particleSystem.shape;
			shape.shapeType = ParticleSystemShapeType.Box;
			shape.scale = new Vector3(2.15f, 2.45f, 1.15f);
			ParticleSystem.NoiseModule noise = particleSystem.noise;
			noise.enabled = true;
			noise.quality = ParticleSystemNoiseQuality.Medium;
			noise.strength = new ParticleSystem.MinMaxCurve(0.055f, 0.12f);
			noise.frequency = 0.32f;
			noise.scrollSpeed = 0.06f;
			noise.damping = true;
			ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particleSystem.colorOverLifetime;
			colorOverLifetime.enabled = true;
			Gradient gradient = new Gradient();
			gradient.SetKeys(new GradientColorKey[]
			{
				new GradientColorKey(new Color(1f, 0.9f, 0.62f), 0f),
				new GradientColorKey(new Color(1f, 0.72f, 0.88f), 0.55f),
				new GradientColorKey(new Color(0.82f, 0.82f, 1f), 1f)
			}, new GradientAlphaKey[]
			{
				new GradientAlphaKey(0f, 0f),
				new GradientAlphaKey(0.82f, 0.18f),
				new GradientAlphaKey(0.68f, 0.72f),
				new GradientAlphaKey(0f, 1f)
			});
			colorOverLifetime.color = gradient;
			ParticleSystemRenderer component = particleSystem.GetComponent<ParticleSystemRenderer>();
			component.renderMode = ParticleSystemRenderMode.Billboard;
			component.sortingFudge = 1.5f;
			Shader shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");
			if (shader != null)
			{
				Material material = new Material(shader)
				{
					name = "Menu Firefly Material"
				};
				Texture2D texture2D = MainMenuPresentationDirector.BuildFireflyTexture();
				material.mainTexture = texture2D;
				if (material.HasProperty("_BaseMap"))
				{
					material.SetTexture("_BaseMap", texture2D);
				}
				if (material.HasProperty("_BaseColor"))
				{
					material.SetColor("_BaseColor", Color.white);
				}
				material.SetFloat("_Surface", 1f);
				material.SetFloat("_Blend", 2f);
				material.SetFloat("_SrcBlend", 5f);
				material.SetFloat("_DstBlend", 1f);
				material.SetFloat("_SrcBlendAlpha", 1f);
				material.SetFloat("_DstBlendAlpha", 10f);
				material.SetFloat("_ZWrite", 0f);
				material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
				material.SetOverrideTag("RenderType", "Transparent");
				material.renderQueue = 3000;
				component.material = material;
			}
			particleSystem.Play();
		}

		// Token: 0x06007FC1 RID: 32705 RVA: 0x00342C8E File Offset: 0x00340E8E
		private IEnumerator EnsureCommonOutfit()
		{
			this.IsPresentationReady = false;
			yield return null;
			yield return new WaitForEndOfFrame();
			GameObject player = ((this._menuPlayer != null) ? this._menuPlayer : MainMenuPresentationDirector.FindMenuPlayerRoot());
			if (!base.isActiveAndEnabled || player == null)
			{
				yield break;
			}
			this.LockEyeHeartBlendShapes(player);
			WearableUseController componentInChildren = player.GetComponentInChildren<WearableUseController>(true);
			if (componentInChildren != null)
			{
				componentInChildren.enabled = false;
			}
			MCStyleCustomizationService styleService = player.GetComponentInChildren<MCStyleCustomizationService>(true);
			MainMenuPresentationDirector.RemoveMenuMask(styleService);
			WardrobeMetadataIndex wardrobe = WardrobeMetadataIndex.Load();
			WardrobeMetadataIndex.Entry entry = MainMenuPresentationDirector.SelectCommonOutfit((wardrobe != null) ? wardrobe.Entries : null, "Outfit.Top", true, false);
			ResourceRequest topRequest = ((entry != null) ? entry.LoadItemAsync() : null);
			if (topRequest != null)
			{
				yield return topRequest;
			}
			if (!base.isActiveAndEnabled || player == null)
			{
				yield break;
			}
			bool topEquipped = MainMenuPresentationDirector.EquipCommon(styleService, (topRequest != null) ? (topRequest.asset as WearableItemSO) : null, "Outfit.Top", true, false);
			WardrobeMetadataIndex.Entry entry2 = MainMenuPresentationDirector.SelectCommonOutfit((wardrobe != null) ? wardrobe.Entries : null, "Outfit.Bottom", false, true);
			ResourceRequest bottomRequest = ((entry2 != null) ? entry2.LoadItemAsync() : null);
			if (bottomRequest != null)
			{
				yield return bottomRequest;
			}
			if (!base.isActiveAndEnabled || player == null)
			{
				yield break;
			}
			bool flag = MainMenuPresentationDirector.EquipCommon(styleService, (bottomRequest != null) ? (bottomRequest.asset as WearableItemSO) : null, "Outfit.Bottom", false, true);
			if (!topEquipped)
			{
				MainMenuPresentationDirector.EnableNamedRenderer(player.transform, "Top1");
			}
			if (!flag)
			{
				MainMenuPresentationDirector.EnableNamedRenderer(player.transform, "Leggings");
			}
			MainMenuPresentationDirector.ApplySystemDefaultBody(player);
			MainMenuPresentationDirector.HideMenuMaskObjects(player);
			this.LockMenuMaskRenderers(player);
			base.StartCoroutine(MainMenuPresentationDirector.KeepSystemDefaultBoobs(player));
			base.StartCoroutine(this.KeepMenuMaskOff(styleService, player));
			int num;
			for (int frame = 0; frame < 3; frame = num + 1)
			{
				yield return new WaitForEndOfFrame();
				if (!base.isActiveAndEnabled || player == null)
				{
					yield break;
				}
				MainMenuPresentationDirector.ApplySystemDefaultBody(player);
				MainMenuPresentationDirector.HideMenuMaskObjects(player);
				this.LockMenuMaskRenderers(player);
				num = frame;
			}
			this.IsPresentationReady = true;
			yield break;
		}

		// Token: 0x06007FC2 RID: 32706 RVA: 0x00342CA0 File Offset: 0x00340EA0
		private void PrepareMenuCharacter()
		{
			this._menuPlayer = MainMenuPresentationDirector.FindMenuPlayerRoot();
			if (this._menuPlayer == null)
			{
				return;
			}
			MainCharacterAppearanceApplier componentInChildren = this._menuPlayer.GetComponentInChildren<MainCharacterAppearanceApplier>(true);
			if (componentInChildren != null)
			{
				componentInChildren.enabled = false;
			}
			WearableUseController componentInChildren2 = this._menuPlayer.GetComponentInChildren<WearableUseController>(true);
			if (componentInChildren2 != null)
			{
				componentInChildren2.enabled = false;
			}
			this.LockMenuMaskRenderers(this._menuPlayer);
		}

		// Token: 0x06007FC3 RID: 32707 RVA: 0x00342D10 File Offset: 0x00340F10
		private static GameObject FindMenuPlayerRoot()
		{
			foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
			{
				if (!(gameObject == null))
				{
					MainCharacterAppearanceApplier mainCharacterAppearanceApplier = gameObject.GetComponent<MainCharacterAppearanceApplier>();
					if (mainCharacterAppearanceApplier == null)
					{
						mainCharacterAppearanceApplier = gameObject.GetComponentInParent<MainCharacterAppearanceApplier>(true);
					}
					if (mainCharacterAppearanceApplier != null)
					{
						return mainCharacterAppearanceApplier.gameObject;
					}
				}
			}
			return null;
		}

		// Token: 0x06007FC4 RID: 32708 RVA: 0x00342D6C File Offset: 0x00340F6C
		private static void ApplySystemDefaultBody(GameObject player)
		{
			CharacterBlendShapeController componentInChildren = player.GetComponentInChildren<CharacterBlendShapeController>(true);
			if (componentInChildren != null && componentInChildren.BlendShapeMap != null)
			{
				componentInChildren.RebindRenderersAndRecache();
				List<BlendShapeMapSO.BlendShapeEntry> blendShapeMappings = componentInChildren.BlendShapeMap.blendShapeMappings;
				if (blendShapeMappings != null)
				{
					for (int i = 0; i < blendShapeMappings.Count; i++)
					{
						BlendShapeMapSO.BlendShapeEntry blendShapeEntry = blendShapeMappings[i];
						if (blendShapeEntry != null)
						{
							componentInChildren.ApplyEntryValue(blendShapeEntry, blendShapeEntry.GetUiDefaultValue());
						}
					}
				}
			}
			BoobsController componentInChildren2 = player.GetComponentInChildren<BoobsController>(true);
			if (componentInChildren2 != null)
			{
				componentInChildren2.SetStateFromBoobsTypeValue(3);
			}
		}

		// Token: 0x06007FC5 RID: 32709 RVA: 0x00342DF4 File Offset: 0x00340FF4
		private void LockMenuMaskRenderers(GameObject player)
		{
			this._lockedMaskRenderers.Clear();
			if (player == null)
			{
				return;
			}
			foreach (Renderer renderer in player.GetComponentsInChildren<Renderer>(true))
			{
				if (!(renderer == null))
				{
					Transform transform = renderer.transform;
					bool flag = MainMenuPresentationDirector.IsMenuMaskName(transform.name);
					if (!flag && transform.parent != null)
					{
						flag = MainMenuPresentationDirector.IsMenuMaskName(transform.parent.name);
					}
					if (flag)
					{
						renderer.enabled = false;
						renderer.forceRenderingOff = true;
						this._lockedMaskRenderers.Add(renderer);
					}
				}
			}
		}

		// Token: 0x06007FC6 RID: 32710 RVA: 0x00342E8F File Offset: 0x0034108F
		private static void RemoveMenuMask(MCStyleCustomizationService styleService)
		{
			if (styleService != null)
			{
				styleService.SetSlotEquipped("Outfit.Mask", false);
			}
		}

		// Token: 0x06007FC7 RID: 32711 RVA: 0x00342EA7 File Offset: 0x003410A7
		private IEnumerator KeepMenuMaskOff(MCStyleCustomizationService styleService, GameObject player)
		{
			int num;
			for (int attempt = 0; attempt < 8; attempt = num + 1)
			{
				MainMenuPresentationDirector.RemoveMenuMask(styleService);
				MainMenuPresentationDirector.HideMenuMaskObjects(player);
				this.LockMenuMaskRenderers(player);
				yield return new WaitForSecondsRealtime((attempt < 3) ? 0.1f : 0.35f);
				num = attempt;
			}
			yield break;
		}

		// Token: 0x06007FC8 RID: 32712 RVA: 0x00342EC4 File Offset: 0x003410C4
		private static IEnumerator KeepSystemDefaultBoobs(GameObject player)
		{
			BoobsController boobsController = player.GetComponentInChildren<BoobsController>(true);
			if (boobsController == null)
			{
				yield break;
			}
			int num;
			for (int attempt = 0; attempt < 6; attempt = num + 1)
			{
				boobsController.SetStateFromBoobsTypeValue(3);
				yield return new WaitForSecondsRealtime((attempt < 3) ? 0.1f : 0.35f);
				num = attempt;
			}
			yield break;
		}

		// Token: 0x06007FC9 RID: 32713 RVA: 0x00342ED4 File Offset: 0x003410D4
		private static void HideMenuMaskObjects(GameObject player)
		{
			if (player == null)
			{
				return;
			}
			foreach (Transform transform in player.GetComponentsInChildren<Transform>(true))
			{
				if (transform != null && MainMenuPresentationDirector.IsMenuMaskName(transform.name))
				{
					transform.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x06007FCA RID: 32714 RVA: 0x00342F27 File Offset: 0x00341127
		private static bool IsMenuMaskName(string objectName)
		{
			return !string.IsNullOrEmpty(objectName) && objectName.StartsWith("Mask", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06007FCB RID: 32715 RVA: 0x00342F40 File Offset: 0x00341140
		private void LockEyeHeartBlendShapes(GameObject player)
		{
			this._lockedEyeHeartShapes.Clear();
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in player.GetComponentsInChildren<SkinnedMeshRenderer>(true))
			{
				Mesh sharedMesh = skinnedMeshRenderer.sharedMesh;
				if (!(sharedMesh == null))
				{
					for (int j = 0; j < sharedMesh.blendShapeCount; j++)
					{
						if (sharedMesh.GetBlendShapeName(j).IndexOf("pupil_heart", StringComparison.OrdinalIgnoreCase) >= 0)
						{
							skinnedMeshRenderer.SetBlendShapeWeight(j, 0f);
							this._lockedEyeHeartShapes.Add(new MainMenuPresentationDirector.LockedBlendShape
							{
								renderer = skinnedMeshRenderer,
								index = j
							});
						}
					}
				}
			}
		}

		// Token: 0x06007FCC RID: 32716 RVA: 0x00342FDC File Offset: 0x003411DC
		internal static WardrobeMetadataIndex.Entry SelectCommonOutfit(IReadOnlyList<WardrobeMetadataIndex.Entry> wardrobe, string partId, bool requireTopCoverage, bool requireBottomCoverage)
		{
			if (wardrobe == null)
			{
				return null;
			}
			List<WardrobeMetadataIndex.Entry> list = new List<WardrobeMetadataIndex.Entry>();
			for (int i = 0; i < wardrobe.Count; i++)
			{
				WardrobeMetadataIndex.Entry entry = wardrobe[i];
				if (entry != null && entry.category == InventoryCategory.Clothing && entry.rarity == ItemRarity.Common && entry.usesMCStyle && string.Equals(entry.mcStylePartId, partId, StringComparison.OrdinalIgnoreCase) && (!requireTopCoverage || entry.coversTopDecently) && (!requireBottomCoverage || entry.coversBottomDecently))
				{
					list.Add(entry);
				}
			}
			if (list.Count != 0)
			{
				return list[Random.Range(0, list.Count)];
			}
			return null;
		}

		// Token: 0x06007FCD RID: 32717 RVA: 0x00343074 File Offset: 0x00341274
		private static bool EquipCommon(MCStyleCustomizationService styleService, WearableItemSO selected, string partId, bool requireTopCoverage, bool requireBottomCoverage)
		{
			if (styleService == null || selected == null || selected.category != InventoryCategory.Clothing || selected.rarity != ItemRarity.Common || !selected.UsesMCStyle || !string.Equals(selected.mcStylePartId, partId, StringComparison.OrdinalIgnoreCase) || (requireTopCoverage && !selected.coversTopDecently) || (requireBottomCoverage && !selected.coversBottomDecently))
			{
				return false;
			}
			if (!styleService.EquipRuntimePreset(partId, selected.mcStylePreset))
			{
				return false;
			}
			int num = selected.PickVariantForTier(ItemRarity.Common);
			if (num >= 0)
			{
				styleService.SelectSurfaceForSlot(partId, num);
			}
			return true;
		}

		// Token: 0x06007FCE RID: 32718 RVA: 0x003430FC File Offset: 0x003412FC
		private static void EnableNamedRenderer(Transform root, string objectName)
		{
			foreach (Transform transform in root.GetComponentsInChildren<Transform>(true))
			{
				if (!(transform == null) && !(transform.name != objectName))
				{
					transform.gameObject.SetActive(true);
					Renderer component = transform.GetComponent<Renderer>();
					if (component != null)
					{
						component.enabled = true;
					}
					return;
				}
			}
		}

		// Token: 0x06007FCF RID: 32719 RVA: 0x00343160 File Offset: 0x00341360
		private static Transform FindTransform(string path)
		{
			int num = path.IndexOf('/');
			GameObject gameObject = GameObject.Find((num >= 0) ? path.Substring(0, num) : path);
			if (gameObject == null)
			{
				return null;
			}
			if (num < 0)
			{
				return gameObject.transform;
			}
			return gameObject.transform.Find(path.Substring(num + 1));
		}

		// Token: 0x06007FD0 RID: 32720 RVA: 0x003431B8 File Offset: 0x003413B8
		private static Texture2D BuildFireflyTexture()
		{
			Texture2D texture2D = new Texture2D(32, 32, TextureFormat.RGBA32, false)
			{
				name = "Menu Firefly Soft Disc",
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear
			};
			Vector2 vector = new Vector2(15.5f, 15.5f);
			float num = 16f;
			for (int i = 0; i < 32; i++)
			{
				for (int j = 0; j < 32; j++)
				{
					float num2 = Vector2.Distance(new Vector2((float)j, (float)i), vector) / num;
					float num3 = Mathf.Pow(1f - Mathf.Clamp01(num2), 1.65f);
					texture2D.SetPixel(j, i, new Color(1f, 1f, 1f, num3));
				}
			}
			texture2D.Apply(false, true);
			return texture2D;
		}

		// Token: 0x06007FD1 RID: 32721 RVA: 0x00343274 File Offset: 0x00341474
		internal static AudioClip BuildUiTone(string clipName, float duration, bool click)
		{
			int num = Mathf.CeilToInt(duration * 22050f);
			float[] array = new float[num];
			uint num2 = 2463534242U;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			for (int i = 0; i < num; i++)
			{
				float num6 = (float)i / 22050f;
				float num7 = num6 / duration;
				float num8 = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(num6 / 0.008f));
				float num9 = Mathf.Pow(1f - Mathf.Clamp01(num7), click ? 2.7f : 2.35f);
				float num10 = num8 * num9;
				float num16;
				if (click)
				{
					float num11 = Mathf.Lerp(142f, 92f, Mathf.Pow(num7, 0.72f));
					float num12 = Mathf.Lerp(246f, 188f, num7);
					num3 += 6.2831855f * num11 / 22050f;
					num4 += 6.2831855f * num12 / 22050f;
					float num13 = Mathf.Sin(num3) * 0.42f;
					float num14 = Mathf.Sin(num4) * Mathf.Exp(-num6 * 11f) * 0.15f;
					num2 = num2 * 1664525U + 1013904223U;
					float num15 = ((num2 >> 9) & 8388607U) / 4194304f - 1f;
					num5 = Mathf.Lerp(num5, num15, 0.035f);
					num16 = (num13 + num14 + num5 * 0.018f) * num10;
				}
				else
				{
					float num17 = Mathf.Lerp(286f, 218f, Mathf.SmoothStep(0f, 1f, num7));
					num3 += 6.2831855f * num17 / 22050f;
					num4 += 6.2831855f * num17 * 0.5f / 22050f;
					num16 = (Mathf.Sin(num3) * 0.25f + Mathf.Sin(num4) * 0.16f) * num10;
				}
				array[i] = num16;
			}
			AudioClip audioClip = AudioClip.Create(clipName, num, 1, 22050, false);
			audioClip.SetData(array, 0);
			return audioClip;
		}

		// Token: 0x04008311 RID: 33553
		private const string MenuRootPath = "MainMenu/FigmaCanvas/Main Menu - v4/MENU";

		// Token: 0x04008312 RID: 33554
		private const string SocialRootPath = "MainMenu/FigmaCanvas/Main Menu - v4/SUPPORT/SOCIALS";

		// Token: 0x04008313 RID: 33555
		private const string LogoImagePath = "MainMenu/FigmaCanvas/Main Menu - v4/Logo/Image";

		// Token: 0x04008314 RID: 33556
		[Header("Menu music")]
		[SerializeField]
		private AudioClip mainMenuMusic;

		// Token: 0x04008315 RID: 33557
		[SerializeField]
		[Range(0f, 1f)]
		private float musicVolume = 0.2f;

		// Token: 0x04008316 RID: 33558
		[Tooltip("Skip the quiet lead-in when starting and repeating the song.")]
		[SerializeField]
		[Min(0f)]
		private float loopStartSeconds = 0.04f;

		// Token: 0x04008317 RID: 33559
		[Tooltip("Repeat before the outro fades to silence; 0 uses the full clip.")]
		[SerializeField]
		[Min(0f)]
		private float loopEndSeconds = 169.85f;

		// Token: 0x04008318 RID: 33560
		[SerializeField]
		[Min(0.02f)]
		private float loopCrossfadeSeconds = 0.4f;

		// Token: 0x04008319 RID: 33561
		private readonly List<MainMenuPresentationDirector.StageLight> _stageLights = new List<MainMenuPresentationDirector.StageLight>();

		// Token: 0x0400831A RID: 33562
		private readonly List<MainMenuPresentationDirector.LockedBlendShape> _lockedEyeHeartShapes = new List<MainMenuPresentationDirector.LockedBlendShape>();

		// Token: 0x0400831B RID: 33563
		private readonly List<Renderer> _lockedMaskRenderers = new List<Renderer>();

		// Token: 0x0400831C RID: 33564
		private GameObject _menuPlayer;

		// Token: 0x0400831D RID: 33565
		private CrossfadeLoopPlayer _musicLoop;

		// Token: 0x0400831E RID: 33566
		private Coroutine _startMusicRoutine;

		// Token: 0x0400831F RID: 33567
		private bool _musicHeldForIntro;

		// Token: 0x04008320 RID: 33568
		private float _musicGain;

		// Token: 0x04008321 RID: 33569
		private AudioSource _hoverSourceA;

		// Token: 0x04008322 RID: 33570
		private AudioSource _hoverSourceB;

		// Token: 0x04008323 RID: 33571
		private AudioSource _uiSource;

		// Token: 0x04008324 RID: 33572
		private AudioClip _hoverClip;

		// Token: 0x04008325 RID: 33573
		private AudioClip _clickClip;

		// Token: 0x04008326 RID: 33574
		private Coroutine _hoverMixRoutine;

		// Token: 0x04008327 RID: 33575
		private bool _nextHoverUsesA;

		// Token: 0x04008328 RID: 33576
		private float _lastHoverTime = -10f;

		// Token: 0x0200106E RID: 4206
		private sealed class StageLight
		{
			// Token: 0x0400832A RID: 33578
			public Light light;

			// Token: 0x0400832B RID: 33579
			public float baseIntensity;

			// Token: 0x0400832C RID: 33580
			public float phase;

			// Token: 0x0400832D RID: 33581
			public float amplitude;
		}

		// Token: 0x0200106F RID: 4207
		private sealed class LockedBlendShape
		{
			// Token: 0x0400832E RID: 33582
			public SkinnedMeshRenderer renderer;

			// Token: 0x0400832F RID: 33583
			public int index;
		}
	}
}
