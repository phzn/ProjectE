using System;
using System.IO;
using Features.CielOS.UI;
using Features.CielOS.VFS;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.CielOS.Apps
{
	// Token: 0x020013F7 RID: 5111
	public class CielOSPaintApp : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler, IPointerUpHandler
	{
		// Token: 0x170018DD RID: 6365
		// (get) Token: 0x06009DCA RID: 40394 RVA: 0x0040E1A2 File Offset: 0x0040C3A2
		private Color BrushColor
		{
			get
			{
				return this._palette[Mathf.Clamp(this._activeSlot, 0, this._palette.Length - 1)];
			}
		}

		// Token: 0x06009DCB RID: 40395 RVA: 0x0040E1C8 File Offset: 0x0040C3C8
		private void Awake()
		{
			this._texW = Mathf.Max(160, this.resolution);
			this._texH = Mathf.RoundToInt((float)this._texW * 9f / 16f);
			this.BuildUI();
			this.BuildTexture();
		}

		// Token: 0x06009DCC RID: 40396 RVA: 0x0040E218 File Offset: 0x0040C418
		private void BuildUI()
		{
			CielOSTheme active = CielOSTheme.Active;
			RectTransform rectTransform = (RectTransform)base.transform;
			foreach (object obj in ((base.transform.parent != null) ? base.transform.parent : base.transform))
			{
				Transform transform = (Transform)obj;
				if (transform != base.transform)
				{
					transform.gameObject.SetActive(false);
				}
			}
			foreach (object obj2 in base.transform)
			{
				((Transform)obj2).gameObject.SetActive(false);
			}
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			RawImage component = base.GetComponent<RawImage>();
			if (component != null)
			{
				component.enabled = false;
			}
			VerticalLayoutGroup verticalLayoutGroup = rectTransform.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.padding = new RectOffset(8, 8, 8, 6);
			verticalLayoutGroup.spacing = 6f;
			verticalLayoutGroup.childControlWidth = true;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			RectTransform rectTransform2 = this.NewChild("Toolbar", rectTransform);
			Image image = rectTransform2.gameObject.AddComponent<Image>();
			image.color = ((active != null) ? active.controlBackground : new Color(0.18f, 0.14f, 0.23f, 1f));
			CielOSTheme.Round(image);
			GridLayoutGroup gridLayoutGroup = rectTransform2.gameObject.AddComponent<GridLayoutGroup>();
			gridLayoutGroup.padding = new RectOffset(8, 8, 6, 6);
			gridLayoutGroup.cellSize = new Vector2(30f, 30f);
			gridLayoutGroup.spacing = new Vector2(6f, 6f);
			gridLayoutGroup.startCorner = GridLayoutGroup.Corner.UpperLeft;
			gridLayoutGroup.startAxis = GridLayoutGroup.Axis.Horizontal;
			gridLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
			gridLayoutGroup.constraint = GridLayoutGroup.Constraint.Flexible;
			rectTransform2.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			for (int i = 0; i < this._palette.Length; i++)
			{
				int slot = i;
				RectTransform rectTransform3 = this.NewChild("Swatch" + i.ToString(), rectTransform2);
				Image image2 = rectTransform3.gameObject.AddComponent<Image>();
				image2.color = this._palette[i];
				CielOSTheme.Round(image2);
				rectTransform3.gameObject.AddComponent<Button>().onClick.AddListener(delegate
				{
					this.SelectSlot(slot);
				});
				rectTransform3.gameObject.AddComponent<CielOSRightClickable>().OnRightClick = delegate(Vector2 _)
				{
					this.EditSlot(slot);
				};
				RectTransform rectTransform4 = this.NewChild("Sel", rectTransform3);
				rectTransform4.anchorMin = Vector2.zero;
				rectTransform4.anchorMax = Vector2.one;
				rectTransform4.offsetMin = new Vector2(-2f, -2f);
				rectTransform4.offsetMax = new Vector2(2f, 2f);
				rectTransform4.SetAsFirstSibling();
				Image image3 = rectTransform4.gameObject.AddComponent<Image>();
				image3.color = ((active != null) ? active.logoAccent : Color.white);
				image3.raycastTarget = false;
				CielOSTheme.Round(image3);
				rectTransform4.gameObject.SetActive(i == this._activeSlot);
				this._swatch[i] = image2;
				this._swatchSel[i] = image3;
			}
			this._brushBtns = new Image[3];
			this._brushBtns[0] = this.AddToolButton(rectTransform2, CielOSIconId.Grid, delegate
			{
				this.SetBrush(2);
			}, "Small brush");
			this._brushBtns[1] = this.AddToolButton(rectTransform2, CielOSIconId.Grid, delegate
			{
				this.SetBrush(5);
			}, "Medium brush");
			this._brushBtns[2] = this.AddToolButton(rectTransform2, CielOSIconId.Grid, delegate
			{
				this.SetBrush(10);
			}, "Large brush");
			for (int j = 0; j < 3; j++)
			{
				this.MakeDot(this._brushBtns[j], (new float[] { 6f, 10f, 14f })[j]);
			}
			this.HighlightBrush(1);
			this.AddToolButton(rectTransform2, CielOSIconId.Paint, new Action(this.EditActiveColor), "Edit colour");
			this.AddToolButton(rectTransform2, CielOSIconId.Close, new Action(this.ClearCanvas), "Clear");
			this.AddToolButton(rectTransform2, CielOSIconId.Upload, new Action(this.ImportImage), "Load image from PC");
			this.AddToolButton(rectTransform2, CielOSIconId.Files, new Action(this.Save), "Save to /Drawings");
			RectTransform rectTransform5 = this.NewChild("CanvasArea", rectTransform);
			LayoutElement layoutElement = rectTransform5.gameObject.AddComponent<LayoutElement>();
			layoutElement.flexibleHeight = 1f;
			layoutElement.minHeight = 120f;
			Image image4 = rectTransform5.gameObject.AddComponent<Image>();
			image4.color = CielOSPaintApp.CanvasColor;
			image4.raycastTarget = false;
			RectTransform rectTransform6 = this.NewChild("Surface", rectTransform5);
			rectTransform6.anchorMin = Vector2.zero;
			rectTransform6.anchorMax = Vector2.one;
			rectTransform6.offsetMin = Vector2.zero;
			rectTransform6.offsetMax = Vector2.zero;
			this._surface = rectTransform6.gameObject.AddComponent<RawImage>();
			this._surfaceRect = rectTransform6;
			CielOSTheme.Outline(this._surface, new Color?((active != null) ? active.outline : new Color(0.78f, 0.62f, 0.9f, 0.35f)), 1f);
			rectTransform6.gameObject.AddComponent<CielOSPaintSurfaceProxy>().app = this;
			this.RefreshSurfaceViewport(false);
			RectTransform rectTransform7 = this.NewChild("Status", rectTransform);
			LayoutElement layoutElement2 = rectTransform7.gameObject.AddComponent<LayoutElement>();
			layoutElement2.minHeight = 18f;
			layoutElement2.preferredHeight = 18f;
			layoutElement2.flexibleHeight = 0f;
			this._status = rectTransform7.gameObject.AddComponent<TextMeshProUGUI>();
			this._status.text = "Left-click a swatch to pick · right-click to edit · Save → /Drawings";
			this._status.fontSize = 11f;
			this._status.enableAutoSizing = true;
			this._status.fontSizeMin = 9f;
			this._status.fontSizeMax = 11f;
			this._status.enableWordWrapping = false;
			this._status.overflowMode = TextOverflowModes.Ellipsis;
			this._status.alignment = TextAlignmentOptions.Center;
			this._status.color = ((active != null) ? new Color(active.controlText.r, active.controlText.g, active.controlText.b, 0.7f) : Color.gray);
			if (active != null && active.uiFont != null)
			{
				this._status.font = active.uiFont;
			}
			CielOSContentText.Bind(this._status, this._status.text);
		}

		// Token: 0x06009DCD RID: 40397 RVA: 0x003FDF19 File Offset: 0x003FC119
		private RectTransform NewChild(string name, Transform parent)
		{
			GameObject gameObject = new GameObject(name, new Type[] { typeof(RectTransform) });
			gameObject.transform.SetParent(parent, false);
			return (RectTransform)gameObject.transform;
		}

		// Token: 0x06009DCE RID: 40398 RVA: 0x0040E918 File Offset: 0x0040CB18
		private Image AddToolButton(Transform parent, CielOSIconId icon, Action onClick, string tip)
		{
			CielOSTheme active = CielOSTheme.Active;
			RectTransform rectTransform = this.NewChild(tip, parent);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = ((active != null) ? new Color(active.controlBackground.r * 0.7f, active.controlBackground.g * 0.7f, active.controlBackground.b * 0.7f, 1f) : new Color(0.13f, 0.1f, 0.17f, 1f));
			CielOSTheme.Round(image);
			rectTransform.gameObject.AddComponent<Button>().onClick.AddListener(delegate
			{
				onClick();
			});
			RectTransform rectTransform2 = this.NewChild("Icon", rectTransform);
			rectTransform2.anchorMin = Vector2.zero;
			rectTransform2.anchorMax = Vector2.one;
			rectTransform2.offsetMin = new Vector2(6f, 6f);
			rectTransform2.offsetMax = new Vector2(-6f, -6f);
			Image image2 = rectTransform2.gameObject.AddComponent<Image>();
			CielOSIcon.Apply(image2, icon, (active != null) ? active.logoAccent : Color.white, 64);
			return image2;
		}

		// Token: 0x06009DCF RID: 40399 RVA: 0x0040EA50 File Offset: 0x0040CC50
		private void MakeDot(Image iconImg, float size)
		{
			if (iconImg == null)
			{
				return;
			}
			CielOSIcon.Apply(iconImg, CielOSIconId.Grid, Color.clear, 8);
			GameObject gameObject = new GameObject("Dot", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject.transform.SetParent(iconImg.transform.parent, false);
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform.pivot = vector;
			rectTransform.anchorMin = (rectTransform.anchorMax = vector);
			rectTransform.sizeDelta = new Vector2(size, size);
			Image component = gameObject.GetComponent<Image>();
			component.color = ((CielOSTheme.Active != null) ? CielOSTheme.Active.logoAccent : Color.white);
			component.raycastTarget = false;
			CielOSTheme.Circle(component);
		}

		// Token: 0x06009DD0 RID: 40400 RVA: 0x0040EB2C File Offset: 0x0040CD2C
		private void HighlightBrush(int idx)
		{
			CielOSTheme active = CielOSTheme.Active;
			int num = 0;
			for (;;)
			{
				int num2 = num;
				Image[] brushBtns = this._brushBtns;
				if (num2 >= ((brushBtns != null) ? brushBtns.Length : 0))
				{
					break;
				}
				Image image = ((this._brushBtns[num] != null) ? this._brushBtns[num].transform.parent.GetComponent<Image>() : null);
				if (!(image == null))
				{
					bool flag = num == idx;
					image.color = (flag ? ((active != null) ? active.accent : new Color(0.66f, 0.38f, 0.79f, 1f)) : ((active != null) ? new Color(active.controlBackground.r * 0.7f, active.controlBackground.g * 0.7f, active.controlBackground.b * 0.7f, 1f) : new Color(0.13f, 0.1f, 0.17f, 1f)));
				}
				num++;
			}
		}

		// Token: 0x06009DD1 RID: 40401 RVA: 0x0040EC34 File Offset: 0x0040CE34
		private void SelectSlot(int slot)
		{
			this._activeSlot = slot;
			for (int i = 0; i < this._swatchSel.Length; i++)
			{
				if (this._swatchSel[i] != null)
				{
					this._swatchSel[i].gameObject.SetActive(i == slot);
				}
			}
		}

		// Token: 0x06009DD2 RID: 40402 RVA: 0x0040EC84 File Offset: 0x0040CE84
		private void EditSlot(int slot)
		{
			this.SelectSlot(slot);
			if (this.colorPicker == null)
			{
				this.SetStatus("No colour picker assigned.", Array.Empty<object>());
				return;
			}
			this.colorPicker.Open(this._palette[slot], delegate(Color c)
			{
				this._palette[slot] = c;
				if (this._swatch[slot] != null)
				{
					this._swatch[slot].color = c;
				}
			});
		}

		// Token: 0x06009DD3 RID: 40403 RVA: 0x0040ECF8 File Offset: 0x0040CEF8
		private void SetBrush(int r)
		{
			this._brushRadius = r;
			this.HighlightBrush((r <= 2) ? 0 : ((r <= 5) ? 1 : 2));
		}

		// Token: 0x06009DD4 RID: 40404 RVA: 0x0040ED16 File Offset: 0x0040CF16
		public void EditActiveColor()
		{
			this.EditSlot(this._activeSlot);
		}

		// Token: 0x06009DD5 RID: 40405 RVA: 0x0040ED24 File Offset: 0x0040CF24
		public void SetBrushSmall()
		{
			this.SetBrush(2);
		}

		// Token: 0x06009DD6 RID: 40406 RVA: 0x0040ED2D File Offset: 0x0040CF2D
		public void SetBrushMedium()
		{
			this.SetBrush(5);
		}

		// Token: 0x06009DD7 RID: 40407 RVA: 0x0040ED36 File Offset: 0x0040CF36
		public void SetBrushLarge()
		{
			this.SetBrush(10);
		}

		// Token: 0x06009DD8 RID: 40408 RVA: 0x0040ED40 File Offset: 0x0040CF40
		private void BuildTexture()
		{
			this._tex = new Texture2D(this._texW, this._texH, TextureFormat.RGBA32, false)
			{
				filterMode = FilterMode.Bilinear
			};
			this.Clear(CielOSPaintApp.CanvasColor);
			if (this._surface != null)
			{
				this._surface.texture = this._tex;
			}
			this.RefreshSurfaceViewport(true);
		}

		// Token: 0x06009DD9 RID: 40409 RVA: 0x0040EDA0 File Offset: 0x0040CFA0
		private void OnRectTransformDimensionsChange()
		{
			if (base.isActiveAndEnabled)
			{
				RectTransform rectTransform = base.transform as RectTransform;
				if (rectTransform != null)
				{
					LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
				}
			}
			this._lastPixel = null;
		}

		// Token: 0x06009DDA RID: 40410 RVA: 0x0040EDD6 File Offset: 0x0040CFD6
		private void LateUpdate()
		{
			this.RefreshSurfaceViewport(false);
		}

		// Token: 0x06009DDB RID: 40411 RVA: 0x0040EDE0 File Offset: 0x0040CFE0
		public static Rect CalculateCoverUvRect(Vector2 surfaceSize, Vector2 textureSize)
		{
			if (surfaceSize.x <= 0f || surfaceSize.y <= 0f || textureSize.x <= 0f || textureSize.y <= 0f)
			{
				return new Rect(0f, 0f, 1f, 1f);
			}
			float num = surfaceSize.x / surfaceSize.y;
			float num2 = textureSize.x / textureSize.y;
			if (num > num2)
			{
				float num3 = num2 / num;
				return new Rect(0f, (1f - num3) * 0.5f, 1f, num3);
			}
			float num4 = num / num2;
			return new Rect((1f - num4) * 0.5f, 0f, num4, 1f);
		}

		// Token: 0x06009DDC RID: 40412 RVA: 0x0040EE9E File Offset: 0x0040D09E
		public static Vector2 MapSurfaceUvToDocumentUv(Rect viewport, Vector2 surfaceUv)
		{
			return new Vector2(viewport.x + surfaceUv.x * viewport.width, viewport.y + surfaceUv.y * viewport.height);
		}

		// Token: 0x06009DDD RID: 40413 RVA: 0x0040EED4 File Offset: 0x0040D0D4
		private void RefreshSurfaceViewport(bool force = false)
		{
			if (this._surface == null || this._surfaceRect == null)
			{
				return;
			}
			Vector2 size = this._surfaceRect.rect.size;
			if (!force && (size - this._lastSurfaceSize).sqrMagnitude < 0.01f)
			{
				return;
			}
			this._lastSurfaceSize = size;
			this._surface.uvRect = CielOSPaintApp.CalculateCoverUvRect(size, new Vector2((float)this._texW, (float)this._texH));
			this._lastPixel = null;
		}

		// Token: 0x06009DDE RID: 40414 RVA: 0x0040EF68 File Offset: 0x0040D168
		private void OnDestroy()
		{
			if (this._tex != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this._tex);
				}
				else
				{
					Object.DestroyImmediate(this._tex);
				}
			}
			this._tex = null;
		}

		// Token: 0x06009DDF RID: 40415 RVA: 0x0040EF9E File Offset: 0x0040D19E
		public void ClearCanvas()
		{
			this.Clear(CielOSPaintApp.CanvasColor);
			this.SetStatus("Cleared.", Array.Empty<object>());
		}

		// Token: 0x06009DE0 RID: 40416 RVA: 0x0040EFBC File Offset: 0x0040D1BC
		public void Save()
		{
			try
			{
				byte[] array = this._tex.EncodeToPNG();
				string text = string.Format("drawing_{0}.png", (int)(Time.realtimeSinceStartup * 1000f));
				string text2 = "/Drawings/" + text;
				if (CielOSFileSystem.Instance.WriteBytes(text2, array))
				{
					this.SetStatus("Saved to {0}", new object[] { text2 });
				}
				else
				{
					this.SetStatus("Save failed.", Array.Empty<object>());
				}
			}
			catch (Exception ex)
			{
				this.SetStatus("Save error: {0}", new object[] { ex.Message });
			}
		}

		// Token: 0x06009DE1 RID: 40417 RVA: 0x0040F064 File Offset: 0x0040D264
		public void ImportImage()
		{
			CielOSFileBrowser browser = CielOSFileBrowser.Ensure(base.GetComponentInParent<Canvas>().transform);
			browser.Open("Load image", new string[] { ".png", ".jpg", ".jpeg" }, delegate(string real)
			{
				if (string.IsNullOrEmpty(real))
				{
					return;
				}
				Texture2D texture2D = null;
				try
				{
					byte[] array = File.ReadAllBytes(real);
					texture2D = new Texture2D(2, 2);
					if (!texture2D.LoadImage(array))
					{
						this.SetStatus("Could not load image.", Array.Empty<object>());
					}
					else
					{
						this.BlitToCanvas(texture2D);
						browser.RememberSuccessfulDirectory(real);
						this.SetStatus("Loaded {0}", new object[] { Path.GetFileName(real) });
					}
				}
				catch (Exception ex)
				{
					this.SetStatus("Load error: {0}", new object[] { ex.Message });
				}
				finally
				{
					if (texture2D != null)
					{
						if (Application.isPlaying)
						{
							Object.Destroy(texture2D);
						}
						else
						{
							Object.DestroyImmediate(texture2D);
						}
					}
				}
			}, null, "cielos_shell", false);
		}

		// Token: 0x06009DE2 RID: 40418 RVA: 0x0040F0D8 File Offset: 0x0040D2D8
		private void BlitToCanvas(Texture2D src)
		{
			this.Clear(CielOSPaintApp.CanvasColor);
			float num = (float)this._texW / (float)this._texH;
			float num2 = (float)src.width / (float)src.height;
			int num3;
			int num4;
			if (num2 > num)
			{
				num3 = this._texW;
				num4 = Mathf.RoundToInt((float)this._texW / num2);
			}
			else
			{
				num4 = this._texH;
				num3 = Mathf.RoundToInt((float)this._texH * num2);
			}
			int num5 = (this._texW - num3) / 2;
			int num6 = (this._texH - num4) / 2;
			for (int i = 0; i < num4; i++)
			{
				for (int j = 0; j < num3; j++)
				{
					float num7 = (float)j / (float)num3;
					float num8 = (float)i / (float)num4;
					Color pixelBilinear = src.GetPixelBilinear(num7, num8);
					this._tex.SetPixel(num5 + j, num6 + i, pixelBilinear);
				}
			}
			this._tex.Apply();
		}

		// Token: 0x06009DE3 RID: 40419 RVA: 0x0040F1B8 File Offset: 0x0040D3B8
		public void OnPointerDown(PointerEventData e)
		{
			this._lastPixel = null;
			this.PaintAt(e);
		}

		// Token: 0x06009DE4 RID: 40420 RVA: 0x0040F1CD File Offset: 0x0040D3CD
		public void OnDrag(PointerEventData e)
		{
			this.PaintAt(e);
		}

		// Token: 0x06009DE5 RID: 40421 RVA: 0x0040F1D6 File Offset: 0x0040D3D6
		public void OnPointerUp(PointerEventData e)
		{
			this._lastPixel = null;
		}

		// Token: 0x06009DE6 RID: 40422 RVA: 0x0040F1E4 File Offset: 0x0040D3E4
		private void PaintAt(PointerEventData eventData)
		{
			if (this._tex == null || this._surfaceRect == null)
			{
				return;
			}
			Vector2 vector;
			if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(this._surfaceRect, eventData.position, eventData.pressEventCamera, out vector))
			{
				return;
			}
			Rect rect = this._surfaceRect.rect;
			if (rect.width <= 0f || rect.height <= 0f)
			{
				return;
			}
			float num = (vector.x - rect.x) / rect.width;
			float num2 = (vector.y - rect.y) / rect.height;
			if (num < 0f || num > 1f || num2 < 0f || num2 > 1f)
			{
				return;
			}
			Vector2 vector2 = CielOSPaintApp.MapSurfaceUvToDocumentUv((this._surface != null) ? this._surface.uvRect : new Rect(0f, 0f, 1f, 1f), new Vector2(num, num2));
			float x = vector2.x;
			float y = vector2.y;
			Vector2Int vector2Int = new Vector2Int(Mathf.Clamp((int)(x * (float)this._texW), 0, this._texW - 1), Mathf.Clamp((int)(y * (float)this._texH), 0, this._texH - 1));
			if (this._lastPixel != null)
			{
				this.DrawLine(this._lastPixel.Value, vector2Int);
			}
			else
			{
				this.DrawDot(vector2Int);
			}
			this._lastPixel = new Vector2Int?(vector2Int);
			this._tex.Apply();
		}

		// Token: 0x06009DE7 RID: 40423 RVA: 0x0040F36C File Offset: 0x0040D56C
		private void DrawDot(Vector2Int c)
		{
			int num = this._brushRadius * this._brushRadius;
			Color brushColor = this.BrushColor;
			for (int i = -this._brushRadius; i <= this._brushRadius; i++)
			{
				for (int j = -this._brushRadius; j <= this._brushRadius; j++)
				{
					if (j * j + i * i <= num)
					{
						int num2 = c.x + j;
						int num3 = c.y + i;
						if (num2 >= 0 && num2 < this._texW && num3 >= 0 && num3 < this._texH)
						{
							this._tex.SetPixel(num2, num3, brushColor);
						}
					}
				}
			}
		}

		// Token: 0x06009DE8 RID: 40424 RVA: 0x0040F40C File Offset: 0x0040D60C
		private void DrawLine(Vector2Int a, Vector2Int b)
		{
			int num = Mathf.Max(1, Mathf.CeilToInt(Vector2Int.Distance(a, b)));
			for (int i = 0; i <= num; i++)
			{
				float num2 = (float)i / (float)num;
				this.DrawDot(new Vector2Int(Mathf.RoundToInt(Mathf.Lerp((float)a.x, (float)b.x, num2)), Mathf.RoundToInt(Mathf.Lerp((float)a.y, (float)b.y, num2))));
			}
		}

		// Token: 0x06009DE9 RID: 40425 RVA: 0x0040F480 File Offset: 0x0040D680
		private void Clear(Color c)
		{
			Color32[] array = new Color32[this._texW * this._texH];
			Color32 color = c;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = color;
			}
			this._tex.SetPixels32(array);
			this._tex.Apply();
		}

		// Token: 0x06009DEA RID: 40426 RVA: 0x0040F4D4 File Offset: 0x0040D6D4
		private void SetStatus(string msg, params object[] arguments)
		{
			if (arguments.Length == 0)
			{
				CielOSContentText.Bind(this._status, msg);
				return;
			}
			CielOSContentText.BindFormat(this._status, msg, arguments);
		}

		// Token: 0x0400A22A RID: 41514
		[SerializeField]
		private int resolution = 768;

		// Token: 0x0400A22B RID: 41515
		[SerializeField]
		private CielOSColorPicker colorPicker;

		// Token: 0x0400A22C RID: 41516
		private static readonly Color[] DefaultPalette = new Color[]
		{
			new Color(0.66f, 0.38f, 0.79f, 1f),
			Color.black,
			Color.white,
			new Color(0.86f, 0.3f, 0.3f, 1f),
			new Color(0.3f, 0.75f, 0.85f, 1f),
			new Color(0.95f, 0.8f, 0.3f, 1f),
			new Color(0.4f, 0.8f, 0.45f, 1f),
			new Color(0.95f, 0.55f, 0.75f, 1f)
		};

		// Token: 0x0400A22D RID: 41517
		private readonly Color[] _palette = (Color[])CielOSPaintApp.DefaultPalette.Clone();

		// Token: 0x0400A22E RID: 41518
		private readonly Image[] _swatch = new Image[8];

		// Token: 0x0400A22F RID: 41519
		private readonly Image[] _swatchSel = new Image[8];

		// Token: 0x0400A230 RID: 41520
		private RawImage _surface;

		// Token: 0x0400A231 RID: 41521
		private RectTransform _surfaceRect;

		// Token: 0x0400A232 RID: 41522
		private Texture2D _tex;

		// Token: 0x0400A233 RID: 41523
		private int _texW = 768;

		// Token: 0x0400A234 RID: 41524
		private int _texH = 432;

		// Token: 0x0400A235 RID: 41525
		private Vector2Int? _lastPixel;

		// Token: 0x0400A236 RID: 41526
		private int _activeSlot;

		// Token: 0x0400A237 RID: 41527
		private int _brushRadius = 5;

		// Token: 0x0400A238 RID: 41528
		private TMP_Text _status;

		// Token: 0x0400A239 RID: 41529
		private Image[] _brushBtns;

		// Token: 0x0400A23A RID: 41530
		private Vector2 _lastSurfaceSize = new Vector2(-1f, -1f);

		// Token: 0x0400A23B RID: 41531
		private static readonly Color CanvasColor = new Color(0.97f, 0.97f, 0.98f, 1f);
	}
}
