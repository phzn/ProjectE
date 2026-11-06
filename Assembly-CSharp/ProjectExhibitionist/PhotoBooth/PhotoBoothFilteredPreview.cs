using System;
using Features.CensorBars;
using Features.Localization;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExhibitionist.PhotoBooth
{
	// Token: 0x0200097B RID: 2427
	[DisallowMultipleComponent]
	public sealed class PhotoBoothFilteredPreview : MonoBehaviour
	{
		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x060042BA RID: 17082 RVA: 0x0018EC38 File Offset: 0x0018CE38
		public bool IsAvailable
		{
			get
			{
				return this._material != null;
			}
		}

		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x060042BB RID: 17083 RVA: 0x0018EC46 File Offset: 0x0018CE46
		public bool IsPresenting
		{
			get
			{
				return this._canvas != null && this._canvas.enabled && this._presentationCamera != null && this._presentationCamera.enabled;
			}
		}

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x060042BC RID: 17084 RVA: 0x0018EC7E File Offset: 0x0018CE7E
		public RenderTexture PreviewTexture
		{
			get
			{
				return this._previewTexture;
			}
		}

		// Token: 0x060042BD RID: 17085 RVA: 0x0018EC86 File Offset: 0x0018CE86
		public void Configure(Camera sourceCamera, float captureAspect)
		{
			this._sourceCamera = sourceCamera;
			this._captureAspect = Mathf.Clamp(captureAspect, 0.4f, 1f);
			this.EnsureRuntimeObjects();
			this.UpdatePortraitPreviewUv();
		}

		// Token: 0x060042BE RID: 17086 RVA: 0x0018ECB4 File Offset: 0x0018CEB4
		public void Begin()
		{
			if (this._sourceCamera == null)
			{
				return;
			}
			this.EnsureRuntimeObjects();
			this.EnsureTexture();
			if (this._material == null || this._previewTexture == null)
			{
				return;
			}
			this._previousTarget = this._sourceCamera.targetTexture;
			this._sourceCamera.targetTexture = this._previewTexture;
			this._image.texture = this._previewTexture;
			if (this._portraitPreviewImage != null)
			{
				this._portraitPreviewImage.texture = this._previewTexture;
				this.UpdatePortraitPreviewUv();
			}
			if (this._presentationCamera != null)
			{
				this._presentationCamera.enabled = true;
			}
			this._canvas.enabled = true;
		}

		// Token: 0x060042BF RID: 17087 RVA: 0x0018ED78 File Offset: 0x0018CF78
		public void End()
		{
			if (this._sourceCamera != null && this._sourceCamera.targetTexture == this._previewTexture)
			{
				this._sourceCamera.targetTexture = this._previousTarget;
			}
			this._previousTarget = null;
			if (this._canvas != null)
			{
				this._canvas.enabled = false;
			}
			if (this._presentationCamera != null)
			{
				this._presentationCamera.enabled = false;
			}
		}

		// Token: 0x060042C0 RID: 17088 RVA: 0x0018EDF8 File Offset: 0x0018CFF8
		public void Apply(float exposure, float contrast, float hue, float saturation, float temperature, float tint, Color colorFilter)
		{
			this.EnsureRuntimeObjects();
			if (this._material == null)
			{
				return;
			}
			this._material.SetFloat("_Exposure", exposure);
			this._material.SetFloat("_Contrast", contrast);
			this._material.SetFloat("_Hue", hue);
			this._material.SetFloat("_Saturation", saturation);
			this._material.SetFloat("_Temperature", temperature);
			this._material.SetFloat("_Tint", tint);
			this._material.SetColor("_ColorFilter", colorFilter);
		}

		// Token: 0x060042C1 RID: 17089 RVA: 0x0018EE95 File Offset: 0x0018D095
		public void ApplyLighting(Color lightColor, float normalizedStrength)
		{
			this.EnsureRuntimeObjects();
			if (this._material == null)
			{
				return;
			}
			this._material.SetColor("_LightColor", lightColor);
			this._material.SetFloat("_LightStrength", Mathf.Clamp01(normalizedStrength));
		}

		// Token: 0x060042C2 RID: 17090 RVA: 0x0018EED4 File Offset: 0x0018D0D4
		public void ApplyLayout(int layoutIndex)
		{
			this.EnsureRuntimeObjects();
			layoutIndex = (layoutIndex % PhotoBoothPhotoLayout.Count + PhotoBoothPhotoLayout.Count) % PhotoBoothPhotoLayout.Count;
			if (this._layoutIndex == layoutIndex && this._layoutOverlayTexture != null)
			{
				return;
			}
			this._layoutIndex = layoutIndex;
			if (this._layoutOverlayTexture != null)
			{
				Object.Destroy(this._layoutOverlayTexture);
			}
			int num = 256;
			int num2 = Mathf.Max(256, Mathf.RoundToInt((float)num / this._captureAspect));
			this._layoutOverlayTexture = PhotoBoothPhotoLayout.CreateOverlayTexture(num, num2, layoutIndex);
			if (this._portraitLayoutOverlay != null)
			{
				this._portraitLayoutOverlay.texture = this._layoutOverlayTexture;
				this._portraitLayoutOverlay.enabled = layoutIndex != 0;
			}
		}

		// Token: 0x060042C3 RID: 17091 RVA: 0x0018EF8F File Offset: 0x0018D18F
		public void Process(RenderTexture source, RenderTexture destination)
		{
			this.EnsureRuntimeObjects();
			if (this._material != null)
			{
				Graphics.Blit(source, destination, this._material);
				return;
			}
			Graphics.Blit(source, destination);
		}

		// Token: 0x060042C4 RID: 17092 RVA: 0x0018EFBC File Offset: 0x0018D1BC
		private void EnsureRuntimeObjects()
		{
			if (this._material == null)
			{
				Shader shader = ((this.filterShader != null) ? this.filterShader : Shader.Find("Hidden/ProjectExhibitionist/PhotoBoothImageFilter"));
				if (shader != null)
				{
					this._material = new Material(shader)
					{
						name = "PhotoBooth Image Filter (Runtime)",
						hideFlags = HideFlags.DontSave
					};
				}
				else
				{
					Debug.LogError("[PhotoBooth] Missing shader Hidden/ProjectExhibitionist/PhotoBoothImageFilter.", this);
				}
			}
			if (this._canvas != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("PhotoBooth_FilteredPreview", new Type[] { typeof(RectTransform) });
			gameObject.transform.SetParent(base.transform, false);
			this._canvas = gameObject.AddComponent<Canvas>();
			this._canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			this._canvas.sortingOrder = 850;
			GameplayHudVisibility.RegisterCanvas(this._canvas);
			this._canvas.enabled = false;
			CanvasScaler canvasScaler = gameObject.AddComponent<CanvasScaler>();
			canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
			canvasScaler.matchWidthOrHeight = 0.5f;
			GameObject gameObject2 = new GameObject("Filtered Camera Image", new Type[] { typeof(RectTransform) });
			gameObject2.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform = (RectTransform)gameObject2.transform;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			this._image = gameObject2.AddComponent<RawImage>();
			StreamerCensorImage.Bind(this._image, false);
			this._image.raycastTarget = false;
			this._image.color = Color.white;
			this._image.material = this._material;
			this.BuildPortraitPreview(gameObject.transform);
			GameObject gameObject3 = new GameObject("PhotoBooth_PresentationCamera");
			gameObject3.transform.SetParent(base.transform, false);
			this._presentationCamera = gameObject3.AddComponent<Camera>();
			this._presentationCamera.enabled = false;
			this._presentationCamera.cullingMask = 0;
			this._presentationCamera.clearFlags = CameraClearFlags.Color;
			this._presentationCamera.backgroundColor = Color.black;
			this._presentationCamera.depth = -100f;
			this._presentationCamera.allowHDR = false;
			this._presentationCamera.allowMSAA = false;
		}

		// Token: 0x060042C5 RID: 17093 RVA: 0x0018F214 File Offset: 0x0018D414
		private void EnsureTexture()
		{
			int num = Mathf.Max(640, Screen.width);
			int num2 = Mathf.Max(360, Screen.height);
			if (this._previewTexture != null && num == this._width && num2 == this._height)
			{
				return;
			}
			this.ReleaseTexture();
			this._width = num;
			this._height = num2;
			this._previewTexture = new RenderTexture(num, num2, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB)
			{
				name = "PhotoBooth Preview",
				antiAliasing = 1,
				filterMode = FilterMode.Bilinear,
				wrapMode = TextureWrapMode.Clamp
			};
			this._previewTexture.Create();
			this.UpdatePortraitPreviewUv();
		}

		// Token: 0x060042C6 RID: 17094 RVA: 0x0018F2BC File Offset: 0x0018D4BC
		private void BuildPortraitPreview(Transform canvasRoot)
		{
			Image image = PhotoBoothFilteredPreview.NewImage(canvasRoot, "Photo Preview Frame", new Color(1f, 0.34f, 0.64f, 0.98f));
			RectTransform rectTransform = image.rectTransform;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.zero;
			rectTransform.pivot = Vector2.zero;
			rectTransform.sizeDelta = new Vector2(234f, 344f);
			rectTransform.anchoredPosition = new Vector2(34f, 34f);
			image.raycastTarget = false;
			Shadow shadow = image.gameObject.AddComponent<Shadow>();
			shadow.effectColor = new Color(0f, 0f, 0f, 0.48f);
			shadow.effectDistance = new Vector2(8f, -8f);
			Image image2 = PhotoBoothFilteredPreview.NewImage(rectTransform, "Preview Inner", new Color(0.018f, 0.024f, 0.04f, 1f));
			PhotoBoothFilteredPreview.Stretch(image2.rectTransform, Vector2.zero, Vector2.one, new Vector2(4f, 4f), new Vector2(-4f, -4f));
			image2.raycastTarget = false;
			GameObject gameObject = new GameObject("Portrait Preview", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(RawImage)
			});
			gameObject.transform.SetParent(image2.transform, false);
			RectTransform rectTransform2 = (RectTransform)gameObject.transform;
			PhotoBoothFilteredPreview.Stretch(rectTransform2, Vector2.zero, Vector2.one, new Vector2(3f, 3f), new Vector2(-3f, -3f));
			this._portraitPreviewImage = gameObject.GetComponent<RawImage>();
			StreamerCensorImage.Bind(this._portraitPreviewImage, false);
			this._portraitPreviewImage.raycastTarget = false;
			this._portraitPreviewImage.color = Color.white;
			this._portraitPreviewImage.material = this._material;
			GameObject gameObject2 = new GameObject("Photo Layout Preview", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(RawImage)
			});
			gameObject2.transform.SetParent(rectTransform2, false);
			PhotoBoothFilteredPreview.Stretch((RectTransform)gameObject2.transform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			this._portraitLayoutOverlay = gameObject2.GetComponent<RawImage>();
			this._portraitLayoutOverlay.raycastTarget = false;
			this._portraitLayoutOverlay.color = Color.white;
			if (this._layoutIndex >= 0)
			{
				this.ApplyLayout(this._layoutIndex);
			}
			Image image3 = PhotoBoothFilteredPreview.NewImage(rectTransform, "Preview Label Background", new Color(0.08f, 0.025f, 0.065f, 0.94f));
			RectTransform rectTransform3 = image3.rectTransform;
			rectTransform3.anchorMin = new Vector2(0f, 1f);
			rectTransform3.anchorMax = new Vector2(0f, 1f);
			rectTransform3.pivot = new Vector2(0f, 1f);
			rectTransform3.sizeDelta = new Vector2(112f, 36f);
			rectTransform3.anchoredPosition = new Vector2(10f, -10f);
			image3.raycastTarget = false;
			Text text = PhotoBoothFilteredPreview.NewText(rectTransform3, "Preview Label", "PREVIEW", 18, FontStyle.Bold, TextAnchor.MiddleCenter);
			PhotoBoothFilteredPreview.Stretch(text.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			text.color = new Color(1f, 0.68f, 0.84f, 1f);
			LocalizedLegacyGameText.BindSource(text, "photobooth_ui", "PREVIEW").FitTranslationToRectangle();
			Outline outline = text.gameObject.AddComponent<Outline>();
			outline.effectColor = new Color(0f, 0f, 0f, 0.55f);
			outline.effectDistance = new Vector2(1f, -1f);
		}

		// Token: 0x060042C7 RID: 17095 RVA: 0x0018F698 File Offset: 0x0018D898
		private void UpdatePortraitPreviewUv()
		{
			if (this._portraitPreviewImage == null || this._previewTexture == null || this._previewTexture.height <= 0)
			{
				return;
			}
			float num = (float)this._previewTexture.width / (float)this._previewTexture.height;
			Rect rect = new Rect(0f, 0f, 1f, 1f);
			if (num > this._captureAspect)
			{
				rect.width = this._captureAspect / num;
				rect.x = (1f - rect.width) * 0.5f;
			}
			else if (num < this._captureAspect)
			{
				rect.height = num / this._captureAspect;
				rect.y = (1f - rect.height) * 0.5f;
			}
			this._portraitPreviewImage.uvRect = rect;
		}

		// Token: 0x060042C8 RID: 17096 RVA: 0x0018F778 File Offset: 0x0018D978
		private static Image NewImage(Transform parent, string name, Color color)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image)
			});
			gameObject.transform.SetParent(parent, false);
			Image component = gameObject.GetComponent<Image>();
			component.color = color;
			return component;
		}

		// Token: 0x060042C9 RID: 17097 RVA: 0x0018F7D4 File Offset: 0x0018D9D4
		private static Text NewText(Transform parent, string name, string content, int size, FontStyle style, TextAnchor alignment)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Text)
			});
			gameObject.transform.SetParent(parent, false);
			Text component = gameObject.GetComponent<Text>();
			component.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			component.text = content;
			component.fontSize = size;
			component.fontStyle = style;
			component.alignment = alignment;
			component.raycastTarget = false;
			return component;
		}

		// Token: 0x060042CA RID: 17098 RVA: 0x0018F85B File Offset: 0x0018DA5B
		private static void Stretch(RectTransform rect, Vector2 min, Vector2 max, Vector2 offsetMin, Vector2 offsetMax)
		{
			rect.anchorMin = min;
			rect.anchorMax = max;
			rect.offsetMin = offsetMin;
			rect.offsetMax = offsetMax;
		}

		// Token: 0x060042CB RID: 17099 RVA: 0x0018F87A File Offset: 0x0018DA7A
		private void ReleaseTexture()
		{
			if (this._previewTexture == null)
			{
				return;
			}
			this._previewTexture.Release();
			Object.Destroy(this._previewTexture);
			this._previewTexture = null;
		}

		// Token: 0x060042CC RID: 17100 RVA: 0x0018F8A8 File Offset: 0x0018DAA8
		private void OnDestroy()
		{
			this.End();
			this.ReleaseTexture();
			if (this._material != null)
			{
				Object.Destroy(this._material);
			}
			if (this._layoutOverlayTexture != null)
			{
				Object.Destroy(this._layoutOverlayTexture);
			}
		}

		// Token: 0x040042CB RID: 17099
		private const string ShaderName = "Hidden/ProjectExhibitionist/PhotoBoothImageFilter";

		// Token: 0x040042CC RID: 17100
		[SerializeField]
		private Shader filterShader;

		// Token: 0x040042CD RID: 17101
		private Camera _sourceCamera;

		// Token: 0x040042CE RID: 17102
		private Camera _presentationCamera;

		// Token: 0x040042CF RID: 17103
		private Canvas _canvas;

		// Token: 0x040042D0 RID: 17104
		private RawImage _image;

		// Token: 0x040042D1 RID: 17105
		private RawImage _portraitPreviewImage;

		// Token: 0x040042D2 RID: 17106
		private RawImage _portraitLayoutOverlay;

		// Token: 0x040042D3 RID: 17107
		private Material _material;

		// Token: 0x040042D4 RID: 17108
		private RenderTexture _previewTexture;

		// Token: 0x040042D5 RID: 17109
		private Texture2D _layoutOverlayTexture;

		// Token: 0x040042D6 RID: 17110
		private RenderTexture _previousTarget;

		// Token: 0x040042D7 RID: 17111
		private int _width;

		// Token: 0x040042D8 RID: 17112
		private int _height;

		// Token: 0x040042D9 RID: 17113
		private float _captureAspect = 0.6666667f;

		// Token: 0x040042DA RID: 17114
		private int _layoutIndex = -1;
	}
}
