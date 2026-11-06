using System;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Minimap;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExhibitionistDemo.Movement
{
	// Token: 0x02000798 RID: 1944
	[DefaultExecutionOrder(110)]
	[DisallowMultipleComponent]
	public sealed class AutoLocomotionHUD : MonoBehaviour
	{
		// Token: 0x0600334D RID: 13133 RVA: 0x001232C4 File Offset: 0x001214C4
		public static bool TryGetVisibleIndicatorBottom(out float bottom)
		{
			bottom = 0f;
			AutoLocomotionHUD autoLocomotionHUD = AutoLocomotionHUD.activeInstance;
			if (autoLocomotionHUD == null)
			{
				return false;
			}
			bool flag = false;
			AutoLocomotionHUD.IncludeVisibleBottom(autoLocomotionHUD.badgeRect, ref flag, ref bottom);
			AutoLocomotionHUD.IncludeVisibleBottom(autoLocomotionHUD.speedRect, ref flag, ref bottom);
			return flag;
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x00123308 File Offset: 0x00121508
		public void Bind(AutoLocomotionController owner)
		{
			this.controller = owner;
		}

		// Token: 0x0600334F RID: 13135 RVA: 0x00123311 File Offset: 0x00121511
		private void Awake()
		{
			AutoLocomotionHUD.activeInstance = this;
			this.BuildUi();
		}

		// Token: 0x06003350 RID: 13136 RVA: 0x0012331F File Offset: 0x0012151F
		private void OnEnable()
		{
			if (this.canvasObject != null)
			{
				this.canvasObject.SetActive(true);
			}
		}

		// Token: 0x06003351 RID: 13137 RVA: 0x0012333B File Offset: 0x0012153B
		private void OnDisable()
		{
			if (this.canvasObject != null)
			{
				this.canvasObject.SetActive(false);
			}
		}

		// Token: 0x06003352 RID: 13138 RVA: 0x00123358 File Offset: 0x00121558
		private void OnDestroy()
		{
			if (AutoLocomotionHUD.activeInstance == this)
			{
				AutoLocomotionHUD.activeInstance = null;
			}
			if (this.canvasObject != null)
			{
				Object.Destroy(this.canvasObject);
			}
			if (this.walkSprite != null)
			{
				Object.Destroy(this.walkSprite);
			}
			if (this.runSprite != null)
			{
				Object.Destroy(this.runSprite);
			}
			if (this.walkTexture != null)
			{
				Object.Destroy(this.walkTexture);
			}
			if (this.runTexture != null)
			{
				Object.Destroy(this.runTexture);
			}
		}

		// Token: 0x06003353 RID: 13139 RVA: 0x001233F8 File Offset: 0x001215F8
		private static void IncludeVisibleBottom(RectTransform rect, ref bool found, ref float bottom)
		{
			if (rect == null || !rect.gameObject.activeInHierarchy)
			{
				return;
			}
			float num = rect.rect.height * Mathf.Abs(rect.localScale.y);
			float num2 = rect.anchoredPosition.y - num;
			if (!found || num2 < bottom)
			{
				bottom = num2;
			}
			found = true;
		}

		// Token: 0x06003354 RID: 13140 RVA: 0x0012345C File Offset: 0x0012165C
		private void Update()
		{
			if (this.controller == null)
			{
				this.controller = base.GetComponent<AutoLocomotionController>();
			}
			bool isMapExpanded = ProceduralMiniMapController.IsMapExpanded;
			this.UpdateBadge(isMapExpanded);
			this.UpdateSpeedFeedback(isMapExpanded);
		}

		// Token: 0x06003355 RID: 13141 RVA: 0x00123498 File Offset: 0x00121698
		public void ShowSpeedFeedback(AutoLocomotionController.AutoLocomotionMode mode, int tier, float multiplier)
		{
			if (this.speedRect == null || ProceduralMiniMapController.IsMapExpanded)
			{
				return;
			}
			this.speedText.text = "x" + multiplier.ToString("0.0");
			this.speedVisibleUntil = Time.unscaledTime + 1.05f;
			this.speedTransitionStarted = Time.unscaledTime;
			this.speedVisible = true;
			this.speedRect.gameObject.SetActive(true);
		}

		// Token: 0x06003356 RID: 13142 RVA: 0x00123510 File Offset: 0x00121710
		private void UpdateBadge(bool mapExpanded)
		{
			if (this.badgeRect == null)
			{
				return;
			}
			bool flag = this.controller != null && this.controller.isActiveAndEnabled && this.controller.ActiveMode != AutoLocomotionController.AutoLocomotionMode.Off && !mapExpanded;
			if (this.badgeRect.gameObject.activeSelf != flag)
			{
				this.badgeRect.gameObject.SetActive(flag);
			}
			if (!flag)
			{
				return;
			}
			bool flag2 = this.controller.ActiveMode == AutoLocomotionController.AutoLocomotionMode.Run;
			Sprite sprite = (flag2 ? this.runSprite : this.walkSprite);
			if (this.badgeIcon.sprite != sprite)
			{
				this.badgeIcon.sprite = sprite;
			}
			float unscaledTime = Time.unscaledTime;
			bool flag3 = flag2 && this.controller.IsAutoRunWaitingForFullStamina;
			float num = Mathf.Sin(unscaledTime * (flag3 ? 3.2f : 5.2f)) * 0.5f + 0.5f;
			this.badgeGroup.alpha = (flag3 ? Mathf.Lerp(0.48f, 0.82f, num) : Mathf.Lerp(0.82f, 1f, num));
			this.badgeRect.localScale = Vector3.one * Mathf.Lerp(1f, 1.035f, num);
			this.badgeRect.anchoredPosition = AutoLocomotionHUD.BadgePosition + Vector2.up * Mathf.Lerp(0f, 0.8f, num);
		}

		// Token: 0x06003357 RID: 13143 RVA: 0x00123688 File Offset: 0x00121888
		private void UpdateSpeedFeedback(bool mapExpanded)
		{
			if (this.speedRect == null)
			{
				return;
			}
			if (mapExpanded)
			{
				this.HideSpeedImmediate();
				return;
			}
			if (!this.speedVisible && !this.speedRect.gameObject.activeSelf)
			{
				return;
			}
			bool flag = this.speedVisible && Time.unscaledTime <= this.speedVisibleUntil;
			float num = (flag ? 1f : 0f);
			this.speedGroup.alpha = Mathf.MoveTowards(this.speedGroup.alpha, num, Time.unscaledDeltaTime * (flag ? 12f : 8f));
			float num2 = Mathf.Max(0f, Time.unscaledTime - this.speedTransitionStarted);
			float num3 = ((flag && num2 < 0.18f) ? (Mathf.Sin(num2 / 0.18f * 3.1415927f) * 0.08f) : 0f);
			this.speedRect.localScale = Vector3.one * (1f + num3);
			Vector2 speedPosition = AutoLocomotionHUD.SpeedPosition;
			speedPosition.x += Mathf.Lerp(4f, 0f, Mathf.Clamp01(num2 / 0.14f));
			this.speedRect.anchoredPosition = speedPosition;
			if (!flag && this.speedGroup.alpha <= 0.001f)
			{
				this.HideSpeedImmediate();
			}
		}

		// Token: 0x06003358 RID: 13144 RVA: 0x001237D9 File Offset: 0x001219D9
		private void HideSpeedImmediate()
		{
			this.speedVisible = false;
			this.speedGroup.alpha = 0f;
			this.speedRect.localScale = Vector3.one;
			this.speedRect.gameObject.SetActive(false);
		}

		// Token: 0x06003359 RID: 13145 RVA: 0x00123814 File Offset: 0x00121A14
		private void BuildUi()
		{
			this.walkSprite = AutoLocomotionHUD.CreateLocomotionSprite(false, out this.walkTexture);
			this.runSprite = AutoLocomotionHUD.CreateLocomotionSprite(true, out this.runTexture);
			this.canvasObject = new GameObject("[Auto Locomotion HUD]", new Type[]
			{
				typeof(RectTransform),
				typeof(Canvas),
				typeof(CanvasScaler)
			});
			this.canvasObject.transform.SetParent(null, false);
			Canvas component = this.canvasObject.GetComponent<Canvas>();
			component.renderMode = RenderMode.ScreenSpaceOverlay;
			component.sortingOrder = 265;
			GameplayHudVisibility.RegisterCanvas(component);
			CanvasScaler component2 = this.canvasObject.GetComponent<CanvasScaler>();
			component2.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			component2.referenceResolution = new Vector2(1920f, 1080f);
			component2.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
			component2.matchWidthOrHeight = 0.5f;
			RectTransform component3 = this.canvasObject.GetComponent<RectTransform>();
			this.badgeRect = AutoLocomotionHUD.CreateTopLeftRect("Auto Locomotion Badge", component3, AutoLocomotionHUD.BadgePosition, new Vector2(28f, 28f));
			this.badgeGroup = this.badgeRect.gameObject.AddComponent<CanvasGroup>();
			this.badgeGroup.blocksRaycasts = false;
			this.badgeGroup.interactable = false;
			this.badgeIcon = this.badgeRect.gameObject.AddComponent<Image>();
			this.badgeIcon.sprite = this.walkSprite;
			this.badgeIcon.color = Color.white;
			this.badgeIcon.preserveAspect = true;
			this.badgeIcon.raycastTarget = false;
			this.badgeRect.gameObject.SetActive(false);
			this.speedRect = AutoLocomotionHUD.CreateTopLeftRect("Auto Locomotion Speed", component3, AutoLocomotionHUD.SpeedPosition, new Vector2(54f, 25f));
			this.speedGroup = this.speedRect.gameObject.AddComponent<CanvasGroup>();
			this.speedGroup.alpha = 0f;
			this.speedGroup.blocksRaycasts = false;
			this.speedGroup.interactable = false;
			this.speedText = this.speedRect.gameObject.AddComponent<TextMeshProUGUI>();
			if (TMP_Settings.defaultFontAsset != null)
			{
				this.speedText.font = TMP_Settings.defaultFontAsset;
			}
			this.speedText.text = "x1.0";
			this.speedText.fontSize = 15f;
			this.speedText.fontStyle = FontStyles.Bold;
			this.speedText.alignment = TextAlignmentOptions.Center;
			this.speedText.color = Color.white;
			this.speedText.enableWordWrapping = false;
			this.speedText.raycastTarget = false;
			this.speedText.outlineWidth = 0.12f;
			this.speedText.outlineColor = new Color32(0, 0, 0, 190);
			this.speedRect.gameObject.SetActive(false);
		}

		// Token: 0x0600335A RID: 13146 RVA: 0x00123AE0 File Offset: 0x00121CE0
		private static RectTransform CreateTopLeftRect(string name, Transform parent, Vector2 position, Vector2 size)
		{
			GameObject gameObject = new GameObject(name, new Type[] { typeof(RectTransform) });
			gameObject.transform.SetParent(parent, false);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			Vector2 vector = new Vector2(0f, 1f);
			component.anchorMax = vector;
			component.anchorMin = vector;
			component.pivot = new Vector2(0f, 1f);
			component.anchoredPosition = position;
			component.sizeDelta = size;
			return component;
		}

		// Token: 0x0600335B RID: 13147 RVA: 0x00123B5C File Offset: 0x00121D5C
		private static Sprite CreateLocomotionSprite(bool running, out Texture2D texture)
		{
			texture = new Texture2D(64, 64, TextureFormat.RGBA32, false)
			{
				name = (running ? "AutoRunIcon" : "AutoWalkIcon"),
				filterMode = FilterMode.Bilinear,
				wrapMode = TextureWrapMode.Clamp
			};
			Color32[] array = new Color32[4096];
			Vector2 vector = (running ? new Vector2(36f, 51f) : new Vector2(33f, 51f));
			Vector2 vector2 = (running ? new Vector2(33f, 43f) : new Vector2(32f, 43f));
			Vector2 vector3 = (running ? new Vector2(28f, 29f) : new Vector2(30f, 29f));
			for (int i = 0; i < 64; i++)
			{
				for (int j = 0; j < 64; j++)
				{
					Vector2 vector4 = new Vector2((float)j + 0.5f, (float)i + 0.5f);
					float num = AutoLocomotionHUD.CircleAlpha(vector4, vector, 5.4f);
					num = Mathf.Max(num, AutoLocomotionHUD.SegmentAlpha(vector4, vector2, vector3, 3.5f));
					if (running)
					{
						num = Mathf.Max(num, AutoLocomotionHUD.SegmentAlpha(vector4, new Vector2(32f, 39f), new Vector2(19f, 43f), 2.4f));
						num = Mathf.Max(num, AutoLocomotionHUD.SegmentAlpha(vector4, new Vector2(32f, 39f), new Vector2(45f, 34f), 2.4f));
						num = Mathf.Max(num, AutoLocomotionHUD.SegmentAlpha(vector4, vector3, new Vector2(13f, 21f), 3f));
						num = Mathf.Max(num, AutoLocomotionHUD.SegmentAlpha(vector4, vector3, new Vector2(39f, 12f), 3f));
						num = Mathf.Max(num, AutoLocomotionHUD.SegmentAlpha(vector4, new Vector2(4f, 37f), new Vector2(17f, 37f), 1.7f));
						num = Mathf.Max(num, AutoLocomotionHUD.SegmentAlpha(vector4, new Vector2(7f, 29f), new Vector2(17f, 29f), 1.4f));
					}
					else
					{
						num = Mathf.Max(num, AutoLocomotionHUD.SegmentAlpha(vector4, new Vector2(31f, 39f), new Vector2(21f, 32f), 2.4f));
						num = Mathf.Max(num, AutoLocomotionHUD.SegmentAlpha(vector4, new Vector2(31f, 39f), new Vector2(42f, 36f), 2.4f));
						num = Mathf.Max(num, AutoLocomotionHUD.SegmentAlpha(vector4, vector3, new Vector2(21f, 14f), 3f));
						num = Mathf.Max(num, AutoLocomotionHUD.SegmentAlpha(vector4, vector3, new Vector2(41f, 19f), 3f));
					}
					byte b = (byte)Mathf.RoundToInt(Mathf.Clamp01(num) * 255f);
					array[i * 64 + j] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, b);
				}
			}
			texture.SetPixels32(array);
			texture.Apply(false, true);
			Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, 64f, 64f), new Vector2(0.5f, 0.5f), 100f);
			sprite.name = texture.name;
			return sprite;
		}

		// Token: 0x0600335C RID: 13148 RVA: 0x00123ED9 File Offset: 0x001220D9
		private static float CircleAlpha(Vector2 point, Vector2 center, float radius)
		{
			return Mathf.Clamp01(radius + 0.75f - Vector2.Distance(point, center));
		}

		// Token: 0x0600335D RID: 13149 RVA: 0x00123EF0 File Offset: 0x001220F0
		private static float SegmentAlpha(Vector2 point, Vector2 start, Vector2 end, float radius)
		{
			Vector2 vector = end - start;
			float sqrMagnitude = vector.sqrMagnitude;
			float num = ((sqrMagnitude > 0f) ? Mathf.Clamp01(Vector2.Dot(point - start, vector) / sqrMagnitude) : 0f);
			float num2 = Vector2.Distance(point, start + vector * num);
			return Mathf.Clamp01(radius + 0.75f - num2);
		}

		// Token: 0x040033F1 RID: 13297
		private static readonly Vector2 BadgePosition = new Vector2(210f, -22f);

		// Token: 0x040033F2 RID: 13298
		private static readonly Vector2 SpeedPosition = new Vector2(243f, -23.5f);

		// Token: 0x040033F3 RID: 13299
		private static AutoLocomotionHUD activeInstance;

		// Token: 0x040033F4 RID: 13300
		private AutoLocomotionController controller;

		// Token: 0x040033F5 RID: 13301
		private GameObject canvasObject;

		// Token: 0x040033F6 RID: 13302
		private RectTransform badgeRect;

		// Token: 0x040033F7 RID: 13303
		private CanvasGroup badgeGroup;

		// Token: 0x040033F8 RID: 13304
		private Image badgeIcon;

		// Token: 0x040033F9 RID: 13305
		private RectTransform speedRect;

		// Token: 0x040033FA RID: 13306
		private CanvasGroup speedGroup;

		// Token: 0x040033FB RID: 13307
		private TextMeshProUGUI speedText;

		// Token: 0x040033FC RID: 13308
		private Sprite walkSprite;

		// Token: 0x040033FD RID: 13309
		private Sprite runSprite;

		// Token: 0x040033FE RID: 13310
		private Texture2D walkTexture;

		// Token: 0x040033FF RID: 13311
		private Texture2D runTexture;

		// Token: 0x04003400 RID: 13312
		private float speedVisibleUntil = -1f;

		// Token: 0x04003401 RID: 13313
		private float speedTransitionStarted = -1f;

		// Token: 0x04003402 RID: 13314
		private bool speedVisible;
	}
}
