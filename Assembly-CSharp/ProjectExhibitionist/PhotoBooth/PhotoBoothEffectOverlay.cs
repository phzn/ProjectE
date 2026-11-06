using System;
using System.Collections.Generic;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExhibitionist.PhotoBooth
{
	// Token: 0x02000979 RID: 2425
	[DisallowMultipleComponent]
	public sealed class PhotoBoothEffectOverlay : MonoBehaviour
	{
		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x060042AA RID: 17066 RVA: 0x0018E216 File Offset: 0x0018C416
		public int EffectCount
		{
			get
			{
				return PhotoBoothEffectOverlay.EffectNames.Length;
			}
		}

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x060042AB RID: 17067 RVA: 0x0018E21F File Offset: 0x0018C41F
		public string CurrentEffectName
		{
			get
			{
				return PhotoBoothEffectOverlay.EffectNames[Mathf.Clamp(this._effectIndex, 0, PhotoBoothEffectOverlay.EffectNames.Length - 1)];
			}
		}

		// Token: 0x060042AC RID: 17068 RVA: 0x0018E23C File Offset: 0x0018C43C
		public void Configure(Camera camera)
		{
			this.targetCamera = camera;
		}

		// Token: 0x060042AD RID: 17069 RVA: 0x0018E245 File Offset: 0x0018C445
		private void Awake()
		{
			if (this.targetCamera == null)
			{
				this.targetCamera = base.GetComponentInParent<Camera>();
			}
			this.EnsureCanvas();
			this.SetEffect(0);
		}

		// Token: 0x060042AE RID: 17070 RVA: 0x0018E26E File Offset: 0x0018C46E
		public void SetEffect(int index)
		{
			this._effectIndex = (index % PhotoBoothEffectOverlay.EffectNames.Length + PhotoBoothEffectOverlay.EffectNames.Length) % PhotoBoothEffectOverlay.EffectNames.Length;
			this.EnsureCanvas();
			this.RebuildItems();
		}

		// Token: 0x060042AF RID: 17071 RVA: 0x0018E29C File Offset: 0x0018C49C
		private void EnsureCanvas()
		{
			if (this._canvas != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("Photo Effects Canvas", new Type[]
			{
				typeof(RectTransform),
				typeof(Canvas),
				typeof(CanvasScaler)
			});
			gameObject.transform.SetParent(base.transform, false);
			this._canvas = gameObject.GetComponent<Canvas>();
			this._canvas.renderMode = RenderMode.ScreenSpaceCamera;
			this._canvas.worldCamera = this.targetCamera;
			this._canvas.planeDistance = 0.35f;
			this._canvas.sortingOrder = 40;
			GameplayHudVisibility.RegisterCanvas(this._canvas);
			CanvasScaler component = gameObject.GetComponent<CanvasScaler>();
			component.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			component.referenceResolution = new Vector2(1920f, 1080f);
			component.matchWidthOrHeight = 0.5f;
			this._root = (RectTransform)gameObject.transform;
			this._root.anchorMin = Vector2.zero;
			this._root.anchorMax = Vector2.one;
			this._root.offsetMin = Vector2.zero;
			this._root.offsetMax = Vector2.zero;
		}

		// Token: 0x060042B0 RID: 17072 RVA: 0x0018E3D0 File Offset: 0x0018C5D0
		private void RebuildItems()
		{
			foreach (PhotoBoothEffectOverlay.EffectItem effectItem in this._items)
			{
				if (effectItem.rect != null)
				{
					Object.Destroy(effectItem.rect.gameObject);
				}
			}
			this._items.Clear();
			if (this._effectIndex == 0 || this._root == null)
			{
				if (this._canvas != null)
				{
					this._canvas.enabled = false;
				}
				return;
			}
			this._canvas.enabled = true;
			Random random = new Random(8123 + this._effectIndex * 127);
			int num = ((this._effectIndex == 1) ? 12 : ((this._effectIndex == 2) ? 14 : 18));
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = new GameObject("Effect_" + i.ToString(), new Type[] { typeof(RectTransform) });
				gameObject.transform.SetParent(this._root, false);
				RectTransform rectTransform = (RectTransform)gameObject.transform;
				Vector2 vector = PhotoBoothEffectOverlay.EdgeAnchor(i, random);
				float num2 = ((this._effectIndex == 3) ? Mathf.Lerp(48f, 138f, (float)random.NextDouble()) : Mathf.Lerp(42f, 82f, (float)random.NextDouble()));
				float num3 = ((this._effectIndex == 1) ? Mathf.Lerp(-24f, 24f, (float)random.NextDouble()) : Mathf.Lerp(-9f, 9f, (float)random.NextDouble()));
				rectTransform.anchorMin = vector;
				rectTransform.anchorMax = vector;
				rectTransform.pivot = new Vector2(0.5f, 0.5f);
				rectTransform.anchoredPosition = Vector2.zero;
				rectTransform.sizeDelta = Vector2.one * num2;
				rectTransform.localRotation = Quaternion.Euler(0f, 0f, num3);
				if (this._effectIndex == 3)
				{
					this.BuildBokeh(gameObject, i, random);
				}
				else
				{
					PhotoBoothEffectOverlay.BuildSymbol(gameObject, this._effectIndex, i, num2, random);
				}
				this._items.Add(new PhotoBoothEffectOverlay.EffectItem
				{
					rect = rectTransform,
					basePosition = Vector2.zero,
					speed = Mathf.Lerp(0.35f, 0.8f, (float)random.NextDouble()),
					phase = Mathf.Lerp(0f, 6.2831855f, (float)random.NextDouble()),
					baseRotation = num3
				});
			}
		}

		// Token: 0x060042B1 RID: 17073 RVA: 0x0018E67C File Offset: 0x0018C87C
		private static Vector2 EdgeAnchor(int index, Random random)
		{
			float num = (float)random.NextDouble();
			float num2 = (float)random.NextDouble();
			switch (index % 4)
			{
			case 0:
				return new Vector2(Mathf.Lerp(0.035f, 0.19f, num), Mathf.Lerp(0.1f, 0.9f, num2));
			case 1:
				return new Vector2(Mathf.Lerp(0.81f, 0.965f, num), Mathf.Lerp(0.1f, 0.9f, num2));
			case 2:
				return new Vector2(Mathf.Lerp(0.2f, 0.8f, num), Mathf.Lerp(0.86f, 0.96f, num2));
			default:
				return new Vector2(Mathf.Lerp(0.2f, 0.8f, num), Mathf.Lerp(0.04f, 0.15f, num2));
			}
		}

		// Token: 0x060042B2 RID: 17074 RVA: 0x0018E748 File Offset: 0x0018C948
		private static void BuildSymbol(GameObject target, int shape, int index, float size, Random random)
		{
			Text text = target.AddComponent<Text>();
			text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			text.text = ((shape == 1) ? "✦" : "♥");
			text.fontSize = Mathf.RoundToInt(size * ((shape == 1) ? 0.92f : 0.82f));
			text.fontStyle = FontStyle.Bold;
			text.alignment = TextAnchor.MiddleCenter;
			text.horizontalOverflow = HorizontalWrapMode.Overflow;
			text.verticalOverflow = VerticalWrapMode.Overflow;
			text.raycastTarget = false;
			float num = (float)random.NextDouble();
			text.color = ((shape == 1) ? Color.Lerp(new Color(1f, 0.68f, 0.1f, 0.94f), new Color(1f, 0.96f, 0.62f, 0.98f), num) : Color.Lerp(new Color(1f, 0.12f, 0.38f, 0.9f), new Color(1f, 0.58f, 0.78f, 0.96f), num));
			Outline outline = target.AddComponent<Outline>();
			outline.effectColor = new Color(0.18f, 0.02f, 0.1f, 0.35f);
			outline.effectDistance = new Vector2(2f, -2f);
		}

		// Token: 0x060042B3 RID: 17075 RVA: 0x0018E880 File Offset: 0x0018CA80
		private void BuildBokeh(GameObject target, int index, Random random)
		{
			Image image = target.AddComponent<Image>();
			image.sprite = this.GetBokehSprite();
			image.preserveAspect = true;
			image.raycastTarget = false;
			Color[] array = new Color[]
			{
				new Color(0.42f, 0.8f, 1f, 0.25f),
				new Color(1f, 0.4f, 0.72f, 0.23f),
				new Color(1f, 0.76f, 0.3f, 0.22f)
			};
			Color color = array[index % array.Length];
			color.a *= Mathf.Lerp(0.72f, 1f, (float)random.NextDouble());
			image.color = color;
		}

		// Token: 0x060042B4 RID: 17076 RVA: 0x0018E94C File Offset: 0x0018CB4C
		private Sprite GetBokehSprite()
		{
			if (this._bokehSprite != null)
			{
				return this._bokehSprite;
			}
			this._bokehTexture = new Texture2D(128, 128, TextureFormat.RGBA32, false)
			{
				name = "PhotoBooth Soft Bokeh",
				hideFlags = HideFlags.DontSave,
				filterMode = FilterMode.Bilinear,
				wrapMode = TextureWrapMode.Clamp
			};
			Color32[] array = new Color32[16384];
			for (int i = 0; i < 128; i++)
			{
				for (int j = 0; j < 128; j++)
				{
					float num = ((float)j + 0.5f) / 128f * 2f - 1f;
					float num2 = ((float)i + 0.5f) / 128f * 2f - 1f;
					float num3 = Mathf.Sqrt(num * num + num2 * num2);
					float num4 = 1f - Mathf.SmoothStep(0.22f, 1f, num3);
					array[i * 128 + j] = new Color(1f, 1f, 1f, num4);
				}
			}
			this._bokehTexture.SetPixels32(array);
			this._bokehTexture.Apply(false, true);
			this._bokehSprite = Sprite.Create(this._bokehTexture, new Rect(0f, 0f, 128f, 128f), new Vector2(0.5f, 0.5f), 128f, 0U, SpriteMeshType.FullRect);
			this._bokehSprite.name = this._bokehTexture.name;
			this._bokehSprite.hideFlags = HideFlags.DontSave;
			return this._bokehSprite;
		}

		// Token: 0x060042B5 RID: 17077 RVA: 0x0018EAE4 File Offset: 0x0018CCE4
		private void Update()
		{
			if (this._effectIndex == 0)
			{
				return;
			}
			float unscaledTime = Time.unscaledTime;
			for (int i = 0; i < this._items.Count; i++)
			{
				PhotoBoothEffectOverlay.EffectItem effectItem = this._items[i];
				if (!(effectItem.rect == null))
				{
					float num = Mathf.Sin(unscaledTime * effectItem.speed + effectItem.phase);
					effectItem.rect.anchoredPosition = effectItem.basePosition + new Vector2(num * 5f, num * 8f);
					effectItem.rect.localScale = Vector3.one * (1f + num * 0.055f);
					effectItem.rect.localRotation = Quaternion.Euler(0f, 0f, effectItem.baseRotation + num * 4f);
				}
			}
		}

		// Token: 0x060042B6 RID: 17078 RVA: 0x0018EBC2 File Offset: 0x0018CDC2
		private void OnDestroy()
		{
			if (this._bokehSprite != null)
			{
				Object.Destroy(this._bokehSprite);
			}
			if (this._bokehTexture != null)
			{
				Object.Destroy(this._bokehTexture);
			}
		}

		// Token: 0x040042BE RID: 17086
		[SerializeField]
		private Camera targetCamera;

		// Token: 0x040042BF RID: 17087
		private Canvas _canvas;

		// Token: 0x040042C0 RID: 17088
		private RectTransform _root;

		// Token: 0x040042C1 RID: 17089
		private readonly List<PhotoBoothEffectOverlay.EffectItem> _items = new List<PhotoBoothEffectOverlay.EffectItem>(20);

		// Token: 0x040042C2 RID: 17090
		private Sprite _bokehSprite;

		// Token: 0x040042C3 RID: 17091
		private Texture2D _bokehTexture;

		// Token: 0x040042C4 RID: 17092
		private int _effectIndex;

		// Token: 0x040042C5 RID: 17093
		public static readonly string[] EffectNames = new string[] { "NONE", "STARS", "HEARTS", "BOKEH" };

		// Token: 0x0200097A RID: 2426
		private sealed class EffectItem
		{
			// Token: 0x040042C6 RID: 17094
			public RectTransform rect;

			// Token: 0x040042C7 RID: 17095
			public float speed;

			// Token: 0x040042C8 RID: 17096
			public float phase;

			// Token: 0x040042C9 RID: 17097
			public Vector2 basePosition;

			// Token: 0x040042CA RID: 17098
			public float baseRotation;
		}
	}
}
