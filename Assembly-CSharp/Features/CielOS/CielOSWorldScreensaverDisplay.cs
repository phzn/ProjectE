using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.CielOS
{
	// Token: 0x02001397 RID: 5015
	[DefaultExecutionOrder(500)]
	public sealed class CielOSWorldScreensaverDisplay : MonoBehaviour
	{
		// Token: 0x06009B4B RID: 39755 RVA: 0x003F7E2E File Offset: 0x003F602E
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void RegisterInstaller()
		{
			SceneManager.sceneLoaded -= CielOSWorldScreensaverDisplay.OnSceneLoaded;
			SceneManager.sceneLoaded += CielOSWorldScreensaverDisplay.OnSceneLoaded;
		}

		// Token: 0x06009B4C RID: 39756 RVA: 0x003F7E54 File Offset: 0x003F6054
		private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (!string.Equals(scene.name, "CielHome", StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			GameObject[] rootGameObjects = scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				if (rootGameObjects[i].GetComponentInChildren<CielOSWorldScreensaverDisplay>(true) != null)
				{
					return;
				}
			}
			GameObject gameObject = new GameObject("CielOS World Screensavers");
			SceneManager.MoveGameObjectToScene(gameObject, scene);
			gameObject.AddComponent<CielOSWorldScreensaverDisplay>();
		}

		// Token: 0x06009B4D RID: 39757 RVA: 0x003F7EB8 File Offset: 0x003F60B8
		private void OnEnable()
		{
			this.BindDisplays();
			CielOSWallpaperService.ScreensaverChanged += this.Refresh;
			this.Refresh();
			if (Application.isPlaying && CielOSWallpaperService.Instance == null)
			{
				this._waitForService = base.StartCoroutine(this.WaitForService());
			}
		}

		// Token: 0x06009B4E RID: 39758 RVA: 0x003F7F08 File Offset: 0x003F6108
		private IEnumerator WaitForService()
		{
			while (CielOSWallpaperService.Instance == null)
			{
				yield return null;
			}
			this._waitForService = null;
			this.Refresh();
			yield break;
		}

		// Token: 0x06009B4F RID: 39759 RVA: 0x003F7F17 File Offset: 0x003F6117
		private void OnDisable()
		{
			this.ReleaseBindings();
		}

		// Token: 0x06009B50 RID: 39760 RVA: 0x003F7F17 File Offset: 0x003F6117
		private void OnDestroy()
		{
			this.ReleaseBindings();
		}

		// Token: 0x06009B51 RID: 39761 RVA: 0x003F7F20 File Offset: 0x003F6120
		private void BindDisplays()
		{
			if (this._renderers.Count != 0)
			{
				return;
			}
			HashSet<Renderer> hashSet = new HashSet<Renderer>();
			GameObject[] rootGameObjects = base.gameObject.scene.GetRootGameObjects();
			for (int i = 0; i < rootGameObjects.Length; i++)
			{
				foreach (CielOSMonitorSurface cielOSMonitorSurface in rootGameObjects[i].GetComponentsInChildren<CielOSMonitorSurface>(true))
				{
					Renderer targetRenderer = cielOSMonitorSurface.TargetRenderer;
					if (!(targetRenderer == null) && !(targetRenderer.gameObject.scene != base.gameObject.scene) && hashSet.Add(targetRenderer))
					{
						Material[] sharedMaterials = targetRenderer.sharedMaterials;
						Material[] array = (Material[])sharedMaterials.Clone();
						List<CielOSWorldScreensaverDisplay.ScreenSlot> list = new List<CielOSWorldScreensaverDisplay.ScreenSlot>();
						for (int k = 0; k < sharedMaterials.Length; k++)
						{
							int displayIndex = cielOSMonitorSurface.GetDisplayIndex(k);
							Material material = sharedMaterials[k];
							if (displayIndex >= 0 && !(material == null) && !(material.shader == null) && !(material.shader.name != "Ciel/Environment/Monitor Display"))
							{
								Material material2 = new Material(material)
								{
									name = string.Format("CielOS Display {0} (Runtime)", displayIndex + 1)
								};
								material2.SetTexture(CielOSWorldScreensaverDisplay.ScreenTexture, Texture2D.blackTexture);
								array[k] = material2;
								list.Add(new CielOSWorldScreensaverDisplay.ScreenSlot(k, displayIndex, material2, targetRenderer));
							}
						}
						if (list.Count != 0)
						{
							targetRenderer.sharedMaterials = array;
							this._renderers.Add(new CielOSWorldScreensaverDisplay.RendererBinding(targetRenderer, sharedMaterials, list));
						}
					}
				}
			}
		}

		// Token: 0x06009B52 RID: 39762 RVA: 0x003F80D0 File Offset: 0x003F62D0
		private void Refresh()
		{
			CielOSWallpaperService instance = CielOSWallpaperService.Instance;
			for (int i = 0; i < this._images.Length; i++)
			{
				Sprite sprite = ((instance != null) ? instance.LoadScreensaverSprite(i) : null);
				Sprite sprite2 = this._images[i];
				this._images[i] = sprite;
				foreach (CielOSWorldScreensaverDisplay.RendererBinding rendererBinding in this._renderers)
				{
					foreach (CielOSWorldScreensaverDisplay.ScreenSlot screenSlot in rendererBinding.slots)
					{
						if (screenSlot.displayIndex == i)
						{
							screenSlot.material.SetTexture(CielOSWorldScreensaverDisplay.ScreenTexture, (sprite != null) ? sprite.texture : Texture2D.blackTexture);
						}
					}
				}
				CielOSWorldScreensaverDisplay.ReleaseImage(sprite2);
			}
		}

		// Token: 0x06009B53 RID: 39763 RVA: 0x003F81D4 File Offset: 0x003F63D4
		private void ReleaseBindings()
		{
			CielOSWallpaperService.ScreensaverChanged -= this.Refresh;
			if (this._waitForService != null)
			{
				base.StopCoroutine(this._waitForService);
				this._waitForService = null;
			}
			foreach (CielOSWorldScreensaverDisplay.RendererBinding rendererBinding in this._renderers)
			{
				if (rendererBinding.renderer != null)
				{
					Material[] sharedMaterials = rendererBinding.renderer.sharedMaterials;
					foreach (CielOSWorldScreensaverDisplay.ScreenSlot screenSlot in rendererBinding.slots)
					{
						if (screenSlot.materialIndex < sharedMaterials.Length && sharedMaterials[screenSlot.materialIndex] == screenSlot.material)
						{
							sharedMaterials[screenSlot.materialIndex] = rendererBinding.originals[screenSlot.materialIndex];
						}
					}
					rendererBinding.renderer.sharedMaterials = sharedMaterials;
				}
				foreach (CielOSWorldScreensaverDisplay.ScreenSlot screenSlot2 in rendererBinding.slots)
				{
					CielOSWorldScreensaverDisplay.ReleaseObject(screenSlot2.material);
				}
			}
			this._renderers.Clear();
			for (int i = 0; i < this._images.Length; i++)
			{
				CielOSWorldScreensaverDisplay.ReleaseImage(this._images[i]);
				this._images[i] = null;
			}
		}

		// Token: 0x06009B54 RID: 39764 RVA: 0x003F8370 File Offset: 0x003F6570
		private static void ReleaseImage(Sprite sprite)
		{
			if (sprite == null)
			{
				return;
			}
			Object texture = sprite.texture;
			CielOSWorldScreensaverDisplay.ReleaseObject(sprite);
			CielOSWorldScreensaverDisplay.ReleaseObject(texture);
		}

		// Token: 0x06009B55 RID: 39765 RVA: 0x000A4FF8 File Offset: 0x000A31F8
		private static void ReleaseObject(Object value)
		{
			if (value == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(value);
				return;
			}
			Object.DestroyImmediate(value);
		}

		// Token: 0x0400A035 RID: 41013
		private const string DisplayShaderName = "Ciel/Environment/Monitor Display";

		// Token: 0x0400A036 RID: 41014
		private static readonly int ScreenTexture = Shader.PropertyToID("_ScreenTex");

		// Token: 0x0400A037 RID: 41015
		private readonly List<CielOSWorldScreensaverDisplay.RendererBinding> _renderers = new List<CielOSWorldScreensaverDisplay.RendererBinding>();

		// Token: 0x0400A038 RID: 41016
		private readonly Sprite[] _images = new Sprite[3];

		// Token: 0x0400A039 RID: 41017
		private Coroutine _waitForService;

		// Token: 0x02001398 RID: 5016
		private sealed class ScreenSlot
		{
			// Token: 0x06009B58 RID: 39768 RVA: 0x003F83BD File Offset: 0x003F65BD
			public ScreenSlot(int materialIndex, int displayIndex, Material material, Renderer renderer)
			{
				this.materialIndex = materialIndex;
				this.displayIndex = displayIndex;
				this.material = material;
			}

			// Token: 0x0400A03A RID: 41018
			public readonly int materialIndex;

			// Token: 0x0400A03B RID: 41019
			public readonly int displayIndex;

			// Token: 0x0400A03C RID: 41020
			public readonly Material material;
		}

		// Token: 0x02001399 RID: 5017
		private sealed class RendererBinding
		{
			// Token: 0x06009B59 RID: 39769 RVA: 0x003F83DA File Offset: 0x003F65DA
			public RendererBinding(Renderer renderer, Material[] originals, List<CielOSWorldScreensaverDisplay.ScreenSlot> slots)
			{
				this.renderer = renderer;
				this.originals = originals;
				this.slots = slots;
			}

			// Token: 0x0400A03D RID: 41021
			public readonly Renderer renderer;

			// Token: 0x0400A03E RID: 41022
			public readonly Material[] originals;

			// Token: 0x0400A03F RID: 41023
			public readonly List<CielOSWorldScreensaverDisplay.ScreenSlot> slots;
		}
	}
}
