using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace ProjectExhibitionist.Core.Rendering
{
	// Token: 0x02000ABF RID: 2751
	public static class ScenePostFxQualityService
	{
		// Token: 0x06004E1E RID: 19998 RVA: 0x001E44A4 File Offset: 0x001E26A4
		internal static bool AuthoredActive(VolumeComponent component)
		{
			bool flag;
			if (!ScenePostFxQualityService.AuthoredActiveState.TryGetValue(component, out flag))
			{
				return component.active;
			}
			return flag;
		}

		// Token: 0x06004E1F RID: 19999 RVA: 0x001E44C8 File Offset: 0x001E26C8
		internal static bool IsAccessibilityVolume(Volume volume)
		{
			return volume == ScenePostFxQualityService._accessibilityVolume;
		}

		// Token: 0x06004E20 RID: 20000 RVA: 0x001E44D8 File Offset: 0x001E26D8
		public static void Apply(GraphicsSettingsModel model, string sceneName = null)
		{
			if (model == null)
			{
				return;
			}
			bool flag = (string.IsNullOrEmpty(sceneName) ? SceneManager.GetActiveScene().name : sceneName) == "MainMenu";
			HashSet<VolumeComponent> hashSet = new HashSet<VolumeComponent>(GameplayVignetteMixer.ComponentReferenceComparer.Instance);
			foreach (Volume volume in Object.FindObjectsOfType<Volume>(true))
			{
				if (!(volume == null))
				{
					if (volume.HasInstantiatedProfile())
					{
						foreach (VolumeComponent volumeComponent in volume.profile.components)
						{
							if (volumeComponent != null)
							{
								hashSet.Add(volumeComponent);
							}
						}
					}
					if (!(volume == ScenePostFxQualityService._accessibilityVolume) && (!(volume.sharedProfile == null) || volume.HasInstantiatedProfile()))
					{
						VolumeProfile profile = volume.profile;
						for (int j = 0; j < profile.components.Count; j++)
						{
							VolumeComponent volumeComponent2 = profile.components[j];
							if (!(volumeComponent2 == null))
							{
								hashSet.Add(volumeComponent2);
								bool flag2;
								if (!ScenePostFxQualityService.AuthoredActiveState.TryGetValue(volumeComponent2, out flag2))
								{
									Vignette vignette = volumeComponent2 as Vignette;
									flag2 = ((vignette != null) ? GameplayVignetteMixer.BaselineActive(vignette) : volumeComponent2.active);
									ScenePostFxQualityService.AuthoredActiveState.Add(volumeComponent2, flag2);
								}
								volumeComponent2.active = model.postProcessing && flag2;
							}
						}
						ScenePostFxQualityService.SetActive<Bloom>(profile, model.postProcessing && model.bloom);
						ScenePostFxQualityService.SetActive<MotionBlur>(profile, model.postProcessing && model.motionBlur && !flag);
						ScenePostFxQualityService.SetActive<DepthOfField>(profile, model.postProcessing && model.depthOfField);
						ScenePostFxQualityService.SetActive<FilmGrain>(profile, model.postProcessing && model.filmGrain);
						ScenePostFxQualityService.SetActive<ChromaticAberration>(profile, model.postProcessing && model.chromaticAberration);
					}
				}
			}
			List<VolumeComponent> list = new List<VolumeComponent>();
			foreach (VolumeComponent volumeComponent3 in ScenePostFxQualityService.AuthoredActiveState.Keys)
			{
				if (!hashSet.Contains(volumeComponent3))
				{
					list.Add(volumeComponent3);
				}
			}
			foreach (VolumeComponent volumeComponent4 in list)
			{
				ScenePostFxQualityService.AuthoredActiveState.Remove(volumeComponent4);
			}
			GameplayVignetteMixer.SetPostProcessingEnabled(model.postProcessing);
			ScenePostFxQualityService.EnsureAccessibilityVolume();
			ScenePostFxQualityService.ApplyColorVision(model.colorVision, model.colorVisionStrength);
			ScenePostFxQualityService.ApplyBrightness(model.brightness);
		}

		// Token: 0x06004E21 RID: 20001 RVA: 0x001E47B8 File Offset: 0x001E29B8
		private static void SetActive<T>(VolumeProfile profile, bool active) where T : VolumeComponent
		{
			T t;
			if (profile != null && profile.TryGet<T>(out t))
			{
				t.active = active;
			}
		}

		// Token: 0x06004E22 RID: 20002 RVA: 0x001E47E4 File Offset: 0x001E29E4
		private static void EnsureAccessibilityVolume()
		{
			if (ScenePostFxQualityService._accessibilityVolume != null && ScenePostFxQualityService._channelMixer != null && ScenePostFxQualityService._brightness != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("Graphics Accessibility Filter");
			Object.DontDestroyOnLoad(gameObject);
			ScenePostFxQualityService._accessibilityVolume = gameObject.AddComponent<Volume>();
			ScenePostFxQualityService._accessibilityVolume.isGlobal = true;
			ScenePostFxQualityService._accessibilityVolume.priority = 10000f;
			ScenePostFxQualityService._accessibilityVolume.weight = 1f;
			VolumeProfile volumeProfile = ScriptableObject.CreateInstance<VolumeProfile>();
			volumeProfile.name = "Graphics Accessibility Filter (Runtime)";
			ScenePostFxQualityService._accessibilityVolume.sharedProfile = volumeProfile;
			ScenePostFxQualityService._accessibilityVolume.profile = volumeProfile;
			ScenePostFxQualityService._channelMixer = volumeProfile.Add<ChannelMixer>(true);
			ScenePostFxQualityService._brightness = volumeProfile.Add<LiftGammaGain>(false);
		}

		// Token: 0x06004E23 RID: 20003 RVA: 0x001E489C File Offset: 0x001E2A9C
		private static void ApplyBrightness(float value)
		{
			value = GraphicsSettingsModel.SanitizeBrightness(value);
			ScenePostFxQualityService._brightness.active = !Mathf.Approximately(value, 1f);
			ScenePostFxQualityService._brightness.gamma.Override(new Vector4(1f, 1f, 1f, value - 1f));
		}

		// Token: 0x06004E24 RID: 20004 RVA: 0x001E48F4 File Offset: 0x001E2AF4
		private static void ApplyColorVision(GraphicsSettingsModel.ColorVisionMode mode, float strength)
		{
			if (ScenePostFxQualityService._channelMixer == null)
			{
				return;
			}
			strength = Mathf.Clamp01(strength);
			ScenePostFxQualityService._channelMixer.active = mode != GraphicsSettingsModel.ColorVisionMode.Normal && strength > 0.001f;
			float[] array = ScenePostFxQualityService.MatrixFor(mode);
			float[] array2 = new float[] { 1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f };
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Mathf.Lerp(array2[i], array[i], strength) * 100f;
			}
			ScenePostFxQualityService._channelMixer.redOutRedIn.Override(array[0]);
			ScenePostFxQualityService._channelMixer.redOutGreenIn.Override(array[1]);
			ScenePostFxQualityService._channelMixer.redOutBlueIn.Override(array[2]);
			ScenePostFxQualityService._channelMixer.greenOutRedIn.Override(array[3]);
			ScenePostFxQualityService._channelMixer.greenOutGreenIn.Override(array[4]);
			ScenePostFxQualityService._channelMixer.greenOutBlueIn.Override(array[5]);
			ScenePostFxQualityService._channelMixer.blueOutRedIn.Override(array[6]);
			ScenePostFxQualityService._channelMixer.blueOutGreenIn.Override(array[7]);
			ScenePostFxQualityService._channelMixer.blueOutBlueIn.Override(array[8]);
		}

		// Token: 0x06004E25 RID: 20005 RVA: 0x001E4A10 File Offset: 0x001E2C10
		private static float[] MatrixFor(GraphicsSettingsModel.ColorVisionMode mode)
		{
			switch (mode)
			{
			case GraphicsSettingsModel.ColorVisionMode.Protanopia:
				return new float[] { 0.817f, 0.183f, 0f, 0.333f, 0.667f, 0f, 0f, 0.125f, 0.875f };
			case GraphicsSettingsModel.ColorVisionMode.Deuteranopia:
				return new float[] { 0.8f, 0.2f, 0f, 0.258f, 0.742f, 0f, 0f, 0.142f, 0.858f };
			case GraphicsSettingsModel.ColorVisionMode.Tritanopia:
				return new float[] { 0.967f, 0.033f, 0f, 0f, 0.733f, 0.267f, 0f, 0.183f, 0.817f };
			default:
				return new float[] { 1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f };
			}
		}

		// Token: 0x04004F70 RID: 20336
		private static Volume _accessibilityVolume;

		// Token: 0x04004F71 RID: 20337
		private static ChannelMixer _channelMixer;

		// Token: 0x04004F72 RID: 20338
		private static LiftGammaGain _brightness;

		// Token: 0x04004F73 RID: 20339
		private static readonly Dictionary<VolumeComponent, bool> AuthoredActiveState = new Dictionary<VolumeComponent, bool>(GameplayVignetteMixer.ComponentReferenceComparer.Instance);
	}
}
