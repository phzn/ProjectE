using System;
using System.IO;
using Features.CensorBars;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.CielOS.Apps
{
	// Token: 0x020013EF RID: 5103
	public class CielOSImageViewerApp : MonoBehaviour, IScrollHandler, IEventSystemHandler, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerClickHandler
	{
		// Token: 0x06009D7E RID: 40318 RVA: 0x0040B8A8 File Offset: 0x00409AA8
		public void AdjustControllerView(Vector2 direction, bool zoom)
		{
			if (zoom)
			{
				this._zoomTarget = Mathf.Clamp(this._zoomTarget + direction.y * Time.unscaledDeltaTime * 2f, this.minZoom, this.maxZoom);
				return;
			}
			this._panTarget += direction * (420f * Time.unscaledDeltaTime);
		}

		// Token: 0x06009D7F RID: 40319 RVA: 0x0040B90B File Offset: 0x00409B0B
		public static CielOSImageViewerApp Mount(RectTransform content, string virtualPath)
		{
			CielOSImageViewerApp cielOSImageViewerApp = CielOSImageViewerApp.MountEmpty(content);
			cielOSImageViewerApp.Load(virtualPath);
			return cielOSImageViewerApp;
		}

		// Token: 0x06009D80 RID: 40320 RVA: 0x0040B91A File Offset: 0x00409B1A
		public static CielOSImageViewerApp Mount(RectTransform content, Sprite sprite, string displayName = "image")
		{
			CielOSImageViewerApp cielOSImageViewerApp = CielOSImageViewerApp.MountEmpty(content);
			cielOSImageViewerApp.LoadSprite(sprite, displayName);
			return cielOSImageViewerApp;
		}

		// Token: 0x06009D81 RID: 40321 RVA: 0x0040B92C File Offset: 0x00409B2C
		private static CielOSImageViewerApp MountEmpty(RectTransform content)
		{
			GameObject gameObject = new GameObject("ImageViewer", new Type[]
			{
				typeof(RectTransform),
				typeof(CielOSImageViewerApp)
			});
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.SetParent(content, false);
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			CielOSImageViewerApp component = gameObject.GetComponent<CielOSImageViewerApp>();
			component.Build();
			return component;
		}

		// Token: 0x06009D82 RID: 40322 RVA: 0x0040B9AC File Offset: 0x00409BAC
		private void Build()
		{
			GameObject gameObject = new GameObject("Viewport", new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(RectMask2D)
			});
			this._viewport = (RectTransform)gameObject.transform;
			this._viewport.SetParent(base.transform, false);
			this._viewport.anchorMin = Vector2.zero;
			this._viewport.anchorMax = Vector2.one;
			this._viewport.offsetMin = Vector2.zero;
			this._viewport.offsetMax = Vector2.zero;
			Graphic component = gameObject.GetComponent<Image>();
			CielOSTheme active = CielOSTheme.Active;
			component.color = ((active != null) ? new Color(active.windowBackground.r, active.windowBackground.g, active.windowBackground.b, 1f) : new Color(0.08f, 0.06f, 0.1f, 1f));
			gameObject.AddComponent<CielOSImageViewerApp.PointerRelay>().Init(this);
			GameObject gameObject2 = new GameObject("Backdrop", new Type[]
			{
				typeof(RectTransform),
				typeof(RawImage)
			});
			this._backdropRt = (RectTransform)gameObject2.transform;
			this._backdropRt.SetParent(this._viewport, false);
			RectTransform backdropRt = this._backdropRt;
			RectTransform backdropRt2 = this._backdropRt;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			backdropRt2.anchorMax = vector;
			backdropRt.anchorMin = vector;
			this._backdropRt.pivot = new Vector2(0.5f, 0.5f);
			this._backdrop = gameObject2.GetComponent<RawImage>();
			StreamerCensorImage.Bind(this._backdrop, false);
			this._backdrop.color = new Color(0.42f, 0.34f, 0.46f, 0.52f);
			this._backdrop.raycastTarget = false;
			GameObject gameObject3 = new GameObject("Picture", new Type[]
			{
				typeof(RectTransform),
				typeof(RawImage)
			});
			this._imageRt = (RectTransform)gameObject3.transform;
			this._imageRt.SetParent(this._viewport, false);
			RectTransform imageRt = this._imageRt;
			RectTransform imageRt2 = this._imageRt;
			vector = new Vector2(0.5f, 0.5f);
			imageRt2.anchorMax = vector;
			imageRt.anchorMin = vector;
			this._imageRt.pivot = new Vector2(0.5f, 0.5f);
			this._imageRt.anchoredPosition = Vector2.zero;
			this._raw = gameObject3.GetComponent<RawImage>();
			StreamerCensorImage.Bind(this._raw, false);
			this._raw.raycastTarget = false;
		}

		// Token: 0x06009D83 RID: 40323 RVA: 0x0040BC64 File Offset: 0x00409E64
		private void Load(string virtualPath)
		{
			Sprite sprite = CielOSWallpaperService.LoadSprite(virtualPath);
			if (sprite == null || sprite.texture == null)
			{
				this.ShowError(Path.GetFileName(virtualPath));
				return;
			}
			this._raw.texture = sprite.texture;
			this._backdrop.texture = sprite.texture;
			this._ownedSprite = sprite;
			this._imgW = sprite.texture.width;
			this._imgH = sprite.texture.height;
			this.FitToViewport(this._imgW, this._imgH);
			this.ResetView();
		}

		// Token: 0x06009D84 RID: 40324 RVA: 0x0040BD00 File Offset: 0x00409F00
		private void LoadSprite(Sprite sp, string displayName)
		{
			if (sp == null || sp.texture == null)
			{
				this.ShowError(displayName);
				return;
			}
			this._raw.texture = sp.texture;
			this._backdrop.texture = sp.texture;
			Rect textureRect = sp.textureRect;
			Rect rect = new Rect(textureRect.x / (float)sp.texture.width, textureRect.y / (float)sp.texture.height, textureRect.width / (float)sp.texture.width, textureRect.height / (float)sp.texture.height);
			this._raw.uvRect = rect;
			this._backdrop.uvRect = rect;
			this._imgW = (int)textureRect.width;
			this._imgH = (int)textureRect.height;
			this.FitToViewport(this._imgW, this._imgH);
			this.ResetView();
		}

		// Token: 0x06009D85 RID: 40325 RVA: 0x0040BDF8 File Offset: 0x00409FF8
		private void FitToViewport(int texW, int texH)
		{
			float num = ((texH > 0) ? ((float)texW / (float)texH) : 1f);
			Rect rect = this._viewport.rect;
			float num2 = Mathf.Max(16f, rect.width);
			float num3 = Mathf.Max(16f, rect.height);
			Vector2 vector = CielOSImageViewerApp.CalculateAspectSize(num2, num3, num, false);
			this._baseW = vector.x;
			this._baseH = vector.y;
			this._imageRt.sizeDelta = vector;
			if (this._backdropRt != null)
			{
				this._backdropRt.sizeDelta = CielOSImageViewerApp.CalculateAspectSize(num2, num3, num, true);
				this._backdropRt.anchoredPosition = Vector2.zero;
			}
		}

		// Token: 0x06009D86 RID: 40326 RVA: 0x0040BEAC File Offset: 0x0040A0AC
		public static Vector2 CalculateAspectSize(float areaWidth, float areaHeight, float mediaAspect, bool cover)
		{
			areaWidth = Mathf.Max(1f, areaWidth);
			areaHeight = Mathf.Max(1f, areaHeight);
			mediaAspect = Mathf.Max(0.01f, mediaAspect);
			float num = areaWidth / areaHeight;
			if (!(cover ? (mediaAspect < num) : (mediaAspect > num)))
			{
				return new Vector2(areaHeight * mediaAspect, areaHeight);
			}
			return new Vector2(areaWidth, areaWidth / mediaAspect);
		}

		// Token: 0x06009D87 RID: 40327 RVA: 0x0040BF08 File Offset: 0x0040A108
		private void OnDestroy()
		{
			if (this._ownedSprite == null)
			{
				return;
			}
			Texture2D texture = this._ownedSprite.texture;
			Object.Destroy(this._ownedSprite);
			if (texture != null)
			{
				Object.Destroy(texture);
			}
			this._ownedSprite = null;
		}

		// Token: 0x06009D88 RID: 40328 RVA: 0x0040BF54 File Offset: 0x0040A154
		private void ResetView()
		{
			this._zoom = (this._zoomTarget = 1f);
			this._pan = (this._panTarget = Vector2.zero);
			if (this._imageRt != null)
			{
				this._imageRt.localScale = Vector3.one;
				this._imageRt.anchoredPosition = Vector2.zero;
			}
		}

		// Token: 0x06009D89 RID: 40329 RVA: 0x0040BFB8 File Offset: 0x0040A1B8
		private void LateUpdate()
		{
			if (this._viewport == null || this._raw == null || this._raw.texture == null)
			{
				return;
			}
			Rect rect = this._viewport.rect;
			if (Mathf.Abs(rect.width - this._lastVpW) > 0.5f || Mathf.Abs(rect.height - this._lastVpH) > 0.5f)
			{
				this._lastVpW = rect.width;
				this._lastVpH = rect.height;
				this.FitToViewport((this._imgW > 0) ? this._imgW : this._raw.texture.width, (this._imgH > 0) ? this._imgH : this._raw.texture.height);
				if (Mathf.Approximately(this._zoomTarget, 1f))
				{
					this._pan = (this._panTarget = Vector2.zero);
				}
			}
		}

		// Token: 0x06009D8A RID: 40330 RVA: 0x0040C0C0 File Offset: 0x0040A2C0
		private void Update()
		{
			if (this._imageRt == null)
			{
				return;
			}
			this._zoom = Mathf.Lerp(this._zoom, this._zoomTarget, 1f - Mathf.Exp(-this.zoomSmooth * Time.unscaledDeltaTime));
			this._imageRt.localScale = Vector3.one * this._zoom;
			this._panTarget = this.ClampPan(this._panTarget, this._zoomTarget);
			this._pan = Vector2.Lerp(this._pan, this._panTarget, 1f - Mathf.Exp(-this.panSmooth * Time.unscaledDeltaTime));
			this._pan = this.ClampPan(this._pan, this._zoom);
			this._imageRt.anchoredPosition = this._pan;
		}

		// Token: 0x06009D8B RID: 40331 RVA: 0x0040C198 File Offset: 0x0040A398
		public void OnScroll(PointerEventData e)
		{
			float y = e.scrollDelta.y;
			if (Mathf.Abs(y) < 0.0001f)
			{
				return;
			}
			float zoomTarget = this._zoomTarget;
			float num = 1f + Mathf.Sign(y) * this.zoomStep;
			float num2 = Mathf.Clamp(zoomTarget * num, this.minZoom, this.maxZoom);
			if (Mathf.Approximately(num2, zoomTarget))
			{
				return;
			}
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this._viewport, e.position, e.pressEventCamera, out vector);
			float num3 = num2 / zoomTarget;
			this._panTarget = (this._panTarget - vector) * num3 + vector;
			this._zoomTarget = num2;
		}

		// Token: 0x06009D8C RID: 40332 RVA: 0x0040C240 File Offset: 0x0040A440
		public void OnPointerDown(PointerEventData e)
		{
			if (e.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this._dragging = true;
			this._moved = 0f;
		}

		// Token: 0x06009D8D RID: 40333 RVA: 0x0040C260 File Offset: 0x0040A460
		public void OnDrag(PointerEventData e)
		{
			if (!this._dragging || e.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this._moved += e.delta.magnitude;
			float num = this.CanvasScale();
			this._panTarget = this.ClampPan(this._panTarget + e.delta / num, this._zoomTarget);
		}

		// Token: 0x06009D8E RID: 40334 RVA: 0x0040C2C9 File Offset: 0x0040A4C9
		public void OnPointerUp(PointerEventData e)
		{
			this._dragging = false;
		}

		// Token: 0x06009D8F RID: 40335 RVA: 0x0040C2D2 File Offset: 0x0040A4D2
		public void OnPointerClick(PointerEventData e)
		{
			if (e.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			if (e.clickCount >= 2)
			{
				if (this._zoomTarget > 1.01f)
				{
					this.ResetTargets();
					return;
				}
				this.ZoomAt(2f, e);
			}
		}

		// Token: 0x06009D90 RID: 40336 RVA: 0x0040C306 File Offset: 0x0040A506
		private void ResetTargets()
		{
			this._zoomTarget = 1f;
			this._panTarget = Vector2.zero;
		}

		// Token: 0x06009D91 RID: 40337 RVA: 0x0040C320 File Offset: 0x0040A520
		private void ZoomAt(float zoom, PointerEventData e)
		{
			float zoomTarget = this._zoomTarget;
			float num = Mathf.Clamp(zoom, this.minZoom, this.maxZoom);
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this._viewport, e.position, e.pressEventCamera, out vector);
			float num2 = num / zoomTarget;
			this._panTarget = (this._panTarget - vector) * num2 + vector;
			this._zoomTarget = num;
		}

		// Token: 0x06009D92 RID: 40338 RVA: 0x0040C38C File Offset: 0x0040A58C
		private Vector2 ClampPan(Vector2 p, float zoom)
		{
			if (this._viewport == null)
			{
				return Vector2.zero;
			}
			Rect rect = this._viewport.rect;
			float num = this._baseW * zoom;
			float num2 = this._baseH * zoom;
			float num3 = Mathf.Max(0f, (num - rect.width) * 0.5f);
			float num4 = Mathf.Max(0f, (num2 - rect.height) * 0.5f);
			return new Vector2(Mathf.Clamp(p.x, -num3, num3), Mathf.Clamp(p.y, -num4, num4));
		}

		// Token: 0x06009D93 RID: 40339 RVA: 0x0040C424 File Offset: 0x0040A624
		private float CanvasScale()
		{
			Canvas componentInParent = base.GetComponentInParent<Canvas>();
			float num = ((componentInParent != null) ? componentInParent.scaleFactor : 1f);
			if (num > 0.0001f)
			{
				return num;
			}
			return 1f;
		}

		// Token: 0x06009D94 RID: 40340 RVA: 0x0040C460 File Offset: 0x0040A660
		private void ShowError(string name)
		{
			GameObject gameObject = new GameObject("Error", new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.SetParent((this._viewport != null) ? this._viewport : ((RectTransform)base.transform), false);
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			TextMeshProUGUI component = gameObject.GetComponent<TextMeshProUGUI>();
			component.text = "Can't open\n" + name;
			component.alignment = TextAlignmentOptions.Center;
			component.color = new Color(1f, 1f, 1f, 0.6f);
			component.fontSize = 16f;
			component.raycastTarget = false;
			CielOSTheme active = CielOSTheme.Active;
			if (active != null && active.uiFont != null)
			{
				component.font = active.uiFont;
			}
			CielOSContentText.BindFormat(component, "Can't open\n{0}", new object[] { name });
		}

		// Token: 0x0400A1F0 RID: 41456
		[Header("Zoom")]
		public float minZoom = 1f;

		// Token: 0x0400A1F1 RID: 41457
		public float maxZoom = 6f;

		// Token: 0x0400A1F2 RID: 41458
		public float zoomStep = 0.25f;

		// Token: 0x0400A1F3 RID: 41459
		public float zoomSmooth = 16f;

		// Token: 0x0400A1F4 RID: 41460
		[Header("Pan")]
		public float panSmooth = 18f;

		// Token: 0x0400A1F5 RID: 41461
		private RectTransform _viewport;

		// Token: 0x0400A1F6 RID: 41462
		private RectTransform _backdropRt;

		// Token: 0x0400A1F7 RID: 41463
		private RectTransform _imageRt;

		// Token: 0x0400A1F8 RID: 41464
		private RawImage _backdrop;

		// Token: 0x0400A1F9 RID: 41465
		private RawImage _raw;

		// Token: 0x0400A1FA RID: 41466
		private Sprite _ownedSprite;

		// Token: 0x0400A1FB RID: 41467
		private float _baseW;

		// Token: 0x0400A1FC RID: 41468
		private float _baseH;

		// Token: 0x0400A1FD RID: 41469
		private float _zoom = 1f;

		// Token: 0x0400A1FE RID: 41470
		private float _zoomTarget = 1f;

		// Token: 0x0400A1FF RID: 41471
		private Vector2 _pan;

		// Token: 0x0400A200 RID: 41472
		private Vector2 _panTarget;

		// Token: 0x0400A201 RID: 41473
		private int _imgW;

		// Token: 0x0400A202 RID: 41474
		private int _imgH;

		// Token: 0x0400A203 RID: 41475
		private float _lastVpW;

		// Token: 0x0400A204 RID: 41476
		private float _lastVpH;

		// Token: 0x0400A205 RID: 41477
		private bool _dragging;

		// Token: 0x0400A206 RID: 41478
		private float _moved;

		// Token: 0x0400A207 RID: 41479
		private const float DragThreshold = 6f;

		// Token: 0x020013F0 RID: 5104
		private class PointerRelay : MonoBehaviour, IScrollHandler, IEventSystemHandler, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerClickHandler
		{
			// Token: 0x06009D96 RID: 40342 RVA: 0x0040C5E8 File Offset: 0x0040A7E8
			public void Init(CielOSImageViewerApp app)
			{
				this._app = app;
			}

			// Token: 0x06009D97 RID: 40343 RVA: 0x0040C5F1 File Offset: 0x0040A7F1
			public void OnScroll(PointerEventData e)
			{
				CielOSImageViewerApp app = this._app;
				if (app == null)
				{
					return;
				}
				app.OnScroll(e);
			}

			// Token: 0x06009D98 RID: 40344 RVA: 0x0040C604 File Offset: 0x0040A804
			public void OnPointerDown(PointerEventData e)
			{
				CielOSImageViewerApp app = this._app;
				if (app == null)
				{
					return;
				}
				app.OnPointerDown(e);
			}

			// Token: 0x06009D99 RID: 40345 RVA: 0x0040C617 File Offset: 0x0040A817
			public void OnDrag(PointerEventData e)
			{
				CielOSImageViewerApp app = this._app;
				if (app == null)
				{
					return;
				}
				app.OnDrag(e);
			}

			// Token: 0x06009D9A RID: 40346 RVA: 0x0040C62A File Offset: 0x0040A82A
			public void OnPointerUp(PointerEventData e)
			{
				CielOSImageViewerApp app = this._app;
				if (app == null)
				{
					return;
				}
				app.OnPointerUp(e);
			}

			// Token: 0x06009D9B RID: 40347 RVA: 0x0040C63D File Offset: 0x0040A83D
			public void OnPointerClick(PointerEventData e)
			{
				CielOSImageViewerApp app = this._app;
				if (app == null)
				{
					return;
				}
				app.OnPointerClick(e);
			}

			// Token: 0x0400A208 RID: 41480
			private CielOSImageViewerApp _app;
		}
	}
}
