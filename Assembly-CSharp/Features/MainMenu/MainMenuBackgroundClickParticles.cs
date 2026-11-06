using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.MainMenu
{
	// Token: 0x02001041 RID: 4161
	public sealed class MainMenuBackgroundClickParticles : MonoBehaviour
	{
		// Token: 0x06007E65 RID: 32357 RVA: 0x0033B080 File Offset: 0x00339280
		private void Awake()
		{
			GameObject gameObject = GameObject.Find("MainMenu/FigmaCanvas");
			this._canvas = ((gameObject != null) ? gameObject.GetComponent<Canvas>() : null);
			if (this._canvas == null)
			{
				base.enabled = false;
				return;
			}
			Transform transform = this._canvas.transform.Find("Main Menu - v4");
			Transform transform2 = ((transform != null) ? transform : this._canvas.transform);
			GameObject gameObject2 = new GameObject("Background Click Particles", new Type[] { typeof(RectTransform) });
			this._particleLayer = gameObject2.GetComponent<RectTransform>();
			this._particleLayer.SetParent(transform2, false);
			this._particleLayer.anchorMin = Vector2.zero;
			this._particleLayer.anchorMax = Vector2.one;
			this._particleLayer.offsetMin = Vector2.zero;
			this._particleLayer.offsetMax = Vector2.zero;
			this._particleLayer.SetAsFirstSibling();
			this._softCircle = MainMenuBackgroundClickParticles.BuildSoftCircleSprite();
			this._hub = Object.FindObjectOfType<MainMenuHubPanels>();
		}

		// Token: 0x06007E66 RID: 32358 RVA: 0x0033B18C File Offset: 0x0033938C
		private void Update()
		{
			if (this._particleLayer == null || !Input.GetMouseButtonDown(0))
			{
				return;
			}
			if (this._hub != null && this._hub.IsPanelOpen)
			{
				return;
			}
			Vector2 vector = Input.mousePosition;
			if (this.IsOverInteractiveUi(vector))
			{
				return;
			}
			Camera camera = ((this._canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : this._canvas.worldCamera);
			Vector2 vector2;
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this._particleLayer, vector, camera, out vector2))
			{
				base.StartCoroutine(this.AnimateBurst(vector2));
			}
		}

		// Token: 0x06007E67 RID: 32359 RVA: 0x0033B21C File Offset: 0x0033941C
		public void SpawnBurst(Vector2 localPosition)
		{
			if (this._particleLayer != null)
			{
				base.StartCoroutine(this.AnimateBurst(localPosition));
			}
		}

		// Token: 0x06007E68 RID: 32360 RVA: 0x0033B23C File Offset: 0x0033943C
		private bool IsOverInteractiveUi(Vector2 screenPosition)
		{
			if (EventSystem.current == null)
			{
				return false;
			}
			this._raycastResults.Clear();
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
			{
				position = screenPosition
			};
			EventSystem.current.RaycastAll(pointerEventData, this._raycastResults);
			for (int i = 0; i < this._raycastResults.Count; i++)
			{
				GameObject gameObject = this._raycastResults[i].gameObject;
				if (gameObject.GetComponentInParent<Selectable>() != null || gameObject.GetComponentInParent<MainMenuButtonFx>() != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007E69 RID: 32361 RVA: 0x0033B2D0 File Offset: 0x003394D0
		private IEnumerator AnimateBurst(Vector2 origin)
		{
			GameObject rootObject = new GameObject("Click Burst", new Type[] { typeof(RectTransform) });
			RectTransform component = rootObject.GetComponent<RectTransform>();
			component.SetParent(this._particleLayer, false);
			RectTransform rectTransform = component;
			RectTransform rectTransform2 = component;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform2.anchorMax = vector;
			rectTransform.anchorMin = vector;
			component.anchoredPosition = origin;
			component.sizeDelta = Vector2.zero;
			Image glow = this.CreateParticleImage("Center Glow", component, 74f);
			glow.color = new Color(1f, 0.62f, 0.8f, 0.55f);
			Image ring = this.CreateParticleImage("Soft Ring", component, 34f);
			ring.color = new Color(1f, 0.53f, 0.73f, 0.42f);
			List<MainMenuBackgroundClickParticles.Dot> dots = new List<MainMenuBackgroundClickParticles.Dot>(12);
			int num = Random.Range(9, 13);
			float num2 = Random.Range(0f, 6.2831855f);
			for (int i = 0; i < num; i++)
			{
				float num3 = num2 + 6.2831855f * (float)i / (float)num + Random.Range(-0.16f, 0.16f);
				Vector2 vector2 = new Vector2(Mathf.Cos(num3), Mathf.Sin(num3));
				float num4 = Random.Range(5.5f, 11.5f);
				int num5 = i % 3;
				Color color;
				if (num5 != 0)
				{
					if (num5 != 1)
					{
						color = new Color(1f, 0.72f, 0.86f, 0.92f);
					}
					else
					{
						color = new Color(1f, 0.38f, 0.65f, 0.92f);
					}
				}
				else
				{
					color = new Color(1f, 0.98f, 0.94f, 0.95f);
				}
				Color color2 = color;
				Image image = this.CreateParticleImage("Spark Halo", component, num4 * 2.7f);
				image.color = new Color(color2.r, color2.g, color2.b, 0.34f);
				Image image2 = this.CreateParticleImage("Creamy Spark", component, num4);
				image2.color = color2;
				dots.Add(new MainMenuBackgroundClickParticles.Dot
				{
					rect = image2.rectTransform,
					image = image2,
					halo = image,
					velocity = vector2 * Random.Range(74f, 150f),
					spin = Random.Range(-170f, 170f),
					startSize = num4,
					startAlpha = color2.a,
					haloStartAlpha = 0.34f
				});
			}
			float elapsed = 0f;
			while (elapsed < 0.55f)
			{
				float unscaledDeltaTime = Time.unscaledDeltaTime;
				elapsed += unscaledDeltaTime;
				float num6 = Mathf.Clamp01(elapsed / 0.55f);
				float num7 = 1f - Mathf.Pow(1f - num6, 3f);
				float num8 = Mathf.Pow(1f - num6, 1.5f);
				glow.rectTransform.localScale = Vector3.one * Mathf.Lerp(0.4f, 1.9f, num7);
				Color color3 = glow.color;
				color3.a = 0.55f * Mathf.Pow(1f - num6, 2.2f);
				glow.color = color3;
				ring.rectTransform.localScale = Vector3.one * Mathf.Lerp(0.38f, 2.1f, num7);
				Color color4 = ring.color;
				color4.a = 0.42f * Mathf.Pow(1f - num6, 1.6f);
				ring.color = color4;
				for (int j = 0; j < dots.Count; j++)
				{
					MainMenuBackgroundClickParticles.Dot dot = dots[j];
					dot.rect.anchoredPosition += dot.velocity * unscaledDeltaTime;
					dot.velocity = Vector2.Lerp(dot.velocity, new Vector2(0f, -26f), unscaledDeltaTime * 4.2f);
					dot.rect.Rotate(0f, 0f, dot.spin * unscaledDeltaTime);
					float num9 = 1f + Mathf.Sin((num6 * 3.1f + (float)j * 0.7f) * 3.1415927f * 2f) * 0.18f;
					float num10 = Mathf.Lerp(0.7f, 1.2f, Mathf.Sin(num6 * 3.1415927f)) * num9;
					dot.rect.sizeDelta = Vector2.one * dot.startSize * num10;
					dot.halo.rectTransform.anchoredPosition = dot.rect.anchoredPosition;
					dot.halo.rectTransform.sizeDelta = dot.rect.sizeDelta * 2.7f;
					Color color5 = dot.image.color;
					color5.a = dot.startAlpha * num8;
					dot.image.color = color5;
					Color color6 = dot.halo.color;
					color6.a = dot.haloStartAlpha * num8;
					dot.halo.color = color6;
				}
				yield return null;
			}
			Object.Destroy(rootObject);
			yield break;
		}

		// Token: 0x06007E6A RID: 32362 RVA: 0x0033B2E8 File Offset: 0x003394E8
		private Image CreateParticleImage(string objectName, Transform parent, float size)
		{
			GameObject gameObject = new GameObject(objectName, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image)
			});
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.SetParent(parent, false);
			Vector2 vector = new Vector2(0.5f, 0.5f);
			component.anchorMax = vector;
			component.anchorMin = vector;
			component.sizeDelta = Vector2.one * size;
			Image component2 = gameObject.GetComponent<Image>();
			component2.sprite = this._softCircle;
			component2.raycastTarget = false;
			return component2;
		}

		// Token: 0x06007E6B RID: 32363 RVA: 0x0033B380 File Offset: 0x00339580
		private static Sprite BuildSoftCircleSprite()
		{
			Texture2D texture2D = new Texture2D(32, 32, TextureFormat.RGBA32, false)
			{
				name = "Main Menu Click Particle",
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
					float num3 = Mathf.Pow(1f - Mathf.Clamp01(num2), 0.72f);
					texture2D.SetPixel(j, i, new Color(1f, 1f, 1f, num3));
				}
			}
			texture2D.Apply(false, true);
			return Sprite.Create(texture2D, new Rect(0f, 0f, 32f, 32f), new Vector2(0.5f, 0.5f), 100f);
		}

		// Token: 0x040081F4 RID: 33268
		private const string CanvasPath = "MainMenu/FigmaCanvas";

		// Token: 0x040081F5 RID: 33269
		private const string MenuGroupName = "Main Menu - v4";

		// Token: 0x040081F6 RID: 33270
		private readonly List<RaycastResult> _raycastResults = new List<RaycastResult>(12);

		// Token: 0x040081F7 RID: 33271
		private RectTransform _particleLayer;

		// Token: 0x040081F8 RID: 33272
		private Canvas _canvas;

		// Token: 0x040081F9 RID: 33273
		private Sprite _softCircle;

		// Token: 0x040081FA RID: 33274
		private MainMenuHubPanels _hub;

		// Token: 0x02001042 RID: 4162
		private sealed class Dot
		{
			// Token: 0x040081FB RID: 33275
			public RectTransform rect;

			// Token: 0x040081FC RID: 33276
			public Image image;

			// Token: 0x040081FD RID: 33277
			public Image halo;

			// Token: 0x040081FE RID: 33278
			public Vector2 velocity;

			// Token: 0x040081FF RID: 33279
			public float spin;

			// Token: 0x04008200 RID: 33280
			public float startSize;

			// Token: 0x04008201 RID: 33281
			public float startAlpha;

			// Token: 0x04008202 RID: 33282
			public float haloStartAlpha;
		}
	}
}
