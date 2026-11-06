using System;
using Features.CielOS.Desktop;
using Features.CielOS.UI;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.CielOS.Apps.Story
{
	// Token: 0x02001432 RID: 5170
	public sealed class CielOSStorySpotlight : MonoBehaviour
	{
		// Token: 0x170018EF RID: 6383
		// (get) Token: 0x06009FAE RID: 40878 RVA: 0x0041DD15 File Offset: 0x0041BF15
		public bool IsVisible
		{
			get
			{
				return this._overlay != null && this._overlay.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x06009FAF RID: 40879 RVA: 0x0041DD38 File Offset: 0x0041BF38
		public void Show(CielOSDesktopIcon icon, UnityAction onClick)
		{
			if (!base.isActiveAndEnabled || icon == null || !icon.gameObject.activeInHierarchy)
			{
				this.Hide();
				return;
			}
			Canvas canvas = base.GetComponentInParent<Canvas>();
			if (canvas == null)
			{
				canvas = icon.GetComponentInParent<Canvas>();
			}
			if (canvas == null || !canvas.isActiveAndEnabled)
			{
				this.Hide();
				return;
			}
			this.EnsureOverlay(canvas.transform);
			this._icon = icon;
			this._onClick = onClick;
			this._overlay.gameObject.SetActive(true);
			this._overlay.SetAsLastSibling();
			if (EventSystem.current != null)
			{
				EventSystem.current.SetSelectedGameObject(null);
			}
			this.UpdateLayout();
		}

		// Token: 0x06009FB0 RID: 40880 RVA: 0x0041DDED File Offset: 0x0041BFED
		public void Hide()
		{
			this._onClick = null;
			this._icon = null;
			if (this._overlay != null)
			{
				this._overlay.gameObject.SetActive(false);
			}
		}

		// Token: 0x06009FB1 RID: 40881 RVA: 0x0041DE1C File Offset: 0x0041C01C
		private void OnDisable()
		{
			this.Hide();
		}

		// Token: 0x06009FB2 RID: 40882 RVA: 0x0041DE24 File Offset: 0x0041C024
		private void OnDestroy()
		{
			if (this._overlay == null)
			{
				return;
			}
			if (Application.isPlaying)
			{
				Object.Destroy(this._overlay.gameObject);
				return;
			}
			Object.DestroyImmediate(this._overlay.gameObject);
		}

		// Token: 0x06009FB3 RID: 40883 RVA: 0x0041DE60 File Offset: 0x0041C060
		private void LateUpdate()
		{
			if (!this.IsVisible)
			{
				return;
			}
			if (this._icon == null || !this._icon.gameObject.activeInHierarchy)
			{
				this.Hide();
				return;
			}
			this._overlay.SetAsLastSibling();
			this.UpdateLayout();
		}

		// Token: 0x06009FB4 RID: 40884 RVA: 0x0041DEAE File Offset: 0x0041C0AE
		private void ActivateStory()
		{
			if (!this.IsVisible || this._onClick == null)
			{
				return;
			}
			UnityAction onClick = this._onClick;
			this.Hide();
			onClick();
		}

		// Token: 0x06009FB5 RID: 40885 RVA: 0x0041DED4 File Offset: 0x0041C0D4
		private void EnsureOverlay(Transform parent)
		{
			if (this._overlay != null)
			{
				if (this._overlay.parent != parent)
				{
					this._overlay.SetParent(parent, false);
				}
				return;
			}
			this._overlay = CielOSStorySpotlight.CreateRect("StorySpotlight", parent);
			this._overlay.anchorMin = Vector2.zero;
			this._overlay.anchorMax = Vector2.one;
			this._overlay.offsetMin = (this._overlay.offsetMax = Vector2.zero);
			for (int i = 0; i < this._scrims.Length; i++)
			{
				this._scrims[i] = CielOSStorySpotlight.CreateRect("Scrim" + i.ToString(), this._overlay).gameObject.AddComponent<Image>();
				this._scrims[i].color = new Color(0f, 0f, 0f, 0.7f);
				this._scrims[i].raycastTarget = true;
			}
			this._hotspotRect = CielOSStorySpotlight.CreateRect("OpenStory", this._overlay);
			Image image = this._hotspotRect.gameObject.AddComponent<Image>();
			image.color = Color.clear;
			this._hotspot = this._hotspotRect.gameObject.AddComponent<Button>();
			this._hotspot.targetGraphic = image;
			this._hotspot.transition = Selectable.Transition.None;
			this._hotspot.navigation = new Navigation
			{
				mode = Navigation.Mode.None
			};
			this._hotspot.onClick.AddListener(new UnityAction(this.ActivateStory));
			RectTransform rectTransform = CielOSStorySpotlight.CreateRect("OffscreenStoryShortcut", this._hotspotRect);
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			RectTransform rectTransform2 = rectTransform;
			Vector2 vector = (rectTransform.offsetMax = Vector2.zero);
			rectTransform2.offsetMin = vector;
			this._fallbackShortcut = rectTransform.gameObject;
			RectTransform rectTransform3 = CielOSStorySpotlight.CreateRect("StoryIcon", rectTransform);
			vector = new Vector2(0.5f, 1f);
			rectTransform3.anchorMax = vector;
			rectTransform3.anchorMin = vector;
			rectTransform3.pivot = new Vector2(0.5f, 1f);
			rectTransform3.anchoredPosition = new Vector2(0f, -14f);
			rectTransform3.sizeDelta = new Vector2(64f, 64f);
			Image image2 = rectTransform3.gameObject.AddComponent<Image>();
			CielOSIcon.ApplyBadge(image2, CielOSAppIcons.ForApp("story"), 64);
			image2.raycastTarget = false;
			TMP_Text tmp_Text = CielOSStorySpotlight.CreateText("StoryLabel", rectTransform, "Story", 18f);
			tmp_Text.alignment = TextAlignmentOptions.Center;
			RectTransform rectTransform4 = (RectTransform)tmp_Text.transform;
			rectTransform4.anchorMin = new Vector2(0f, 0f);
			rectTransform4.anchorMax = new Vector2(1f, 0f);
			rectTransform4.pivot = new Vector2(0.5f, 0f);
			rectTransform4.anchoredPosition = new Vector2(0f, 10f);
			rectTransform4.sizeDelta = new Vector2(0f, 28f);
			for (int j = 0; j < this._outline.Length; j++)
			{
				this._outline[j] = CielOSStorySpotlight.CreateRect("Glow" + j.ToString(), this._overlay).gameObject.AddComponent<Image>();
				this._outline[j].raycastTarget = false;
			}
			TMP_Text tmp_Text2 = CielOSStorySpotlight.CreateText("ChapterPrompt", this._overlay, "<b>New Story chapter</b>\n<size=20>Open Story to continue</size>", 26f);
			this._captionRect = (RectTransform)tmp_Text2.transform;
		}

		// Token: 0x06009FB6 RID: 40886 RVA: 0x0041E25C File Offset: 0x0041C45C
		private void UpdateLayout()
		{
			if (this._overlay == null || this._icon == null)
			{
				return;
			}
			Rect rect = this._overlay.rect;
			if (rect.width <= 1f || rect.height <= 1f)
			{
				return;
			}
			this._icon.Rect.GetWorldCorners(this._corners);
			Vector2 vector = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
			Vector2 vector2 = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
			foreach (Vector3 vector3 in this._corners)
			{
				Vector2 vector4 = this._overlay.InverseTransformPoint(vector3);
				vector = Vector2.Min(vector, vector4);
				vector2 = Vector2.Max(vector2, vector4);
			}
			Rect rect2 = Rect.MinMaxRect(vector.x, vector.y, vector2.x, vector2.y);
			bool flag;
			Rect rect3 = CielOSStorySpotlight.FitHighlight(rect, rect2, 12f, out flag);
			this._fallbackShortcut.SetActive(flag);
			CielOSStorySpotlight.ApplyRect(this._hotspotRect, rect3, rect);
			CielOSStorySpotlight.ApplyRect((RectTransform)this._scrims[0].transform, Rect.MinMaxRect(rect.xMin, rect.yMin, rect3.xMin, rect.yMax), rect);
			CielOSStorySpotlight.ApplyRect((RectTransform)this._scrims[1].transform, Rect.MinMaxRect(rect3.xMax, rect.yMin, rect.xMax, rect.yMax), rect);
			CielOSStorySpotlight.ApplyRect((RectTransform)this._scrims[2].transform, Rect.MinMaxRect(rect3.xMin, rect3.yMax, rect3.xMax, rect.yMax), rect);
			CielOSStorySpotlight.ApplyRect((RectTransform)this._scrims[3].transform, Rect.MinMaxRect(rect3.xMin, rect.yMin, rect3.xMax, rect3.yMin), rect);
			float num = 0.5f + 0.5f * Mathf.Sin(Time.unscaledTime * 3.5f);
			for (int j = 0; j < 2; j++)
			{
				float num2 = ((j == 0) ? 7f : 2f);
				Color accent = CielOSStorySpotlight.Accent;
				accent.a = ((j == 0) ? Mathf.Lerp(0.14f, 0.28f, num) : Mathf.Lerp(0.72f, 1f, num));
				for (int k = 0; k < 4; k++)
				{
					Image image = this._outline[j * 4 + k];
					image.color = accent;
					Rect rect4 = ((k == 0) ? new Rect(rect3.xMin, rect3.yMin, num2, rect3.height) : ((k == 1) ? new Rect(rect3.xMax - num2, rect3.yMin, num2, rect3.height) : ((k == 2) ? new Rect(rect3.xMin, rect3.yMax - num2, rect3.width, num2) : new Rect(rect3.xMin, rect3.yMin, rect3.width, num2))));
					CielOSStorySpotlight.ApplyRect((RectTransform)image.transform, rect4, rect);
				}
			}
			Rect rect5 = CielOSStorySpotlight.FitCaption(rect, rect3);
			this._captionRect.gameObject.SetActive(!rect5.Overlaps(rect3));
			CielOSStorySpotlight.ApplyRect(this._captionRect, rect5, rect);
		}

		// Token: 0x06009FB7 RID: 40887 RVA: 0x0041E5DC File Offset: 0x0041C7DC
		public static Rect FitHighlight(Rect viewport, Rect target, float padding, out bool fallback)
		{
			Rect rect = Rect.MinMaxRect(Mathf.Max(viewport.xMin, target.xMin), Mathf.Max(viewport.yMin, target.yMin), Mathf.Min(viewport.xMax, target.xMax), Mathf.Min(viewport.yMax, target.yMax));
			fallback = rect.width < 32f || rect.height < 32f;
			if (fallback)
			{
				float num = Mathf.Min(140f, viewport.width);
				float num2 = Mathf.Min(126f, viewport.height);
				return new Rect(viewport.center.x - num * 0.5f, viewport.center.y - num2 * 0.5f, num, num2);
			}
			return Rect.MinMaxRect(Mathf.Max(viewport.xMin, target.xMin - padding), Mathf.Max(viewport.yMin, target.yMin - padding), Mathf.Min(viewport.xMax, target.xMax + padding), Mathf.Min(viewport.yMax, target.yMax + padding));
		}

		// Token: 0x06009FB8 RID: 40888 RVA: 0x0041E710 File Offset: 0x0041C910
		private static Rect FitCaption(Rect viewport, Rect hole)
		{
			float num = Mathf.Min(18f, Mathf.Min(viewport.width, viewport.height) * 0.1f);
			float num2 = Mathf.Min(350f, Mathf.Max(1f, viewport.width - num * 2f));
			float num3 = Mathf.Min(86f, Mathf.Max(1f, viewport.height - num * 2f));
			float num4 = hole.xMax + 24f;
			float num5 = hole.center.y - num3 * 0.5f;
			if (num4 + num2 > viewport.xMax - num)
			{
				num4 = hole.xMin - 24f - num2;
				if (num4 < viewport.xMin + num)
				{
					num4 = hole.center.x - num2 * 0.5f;
					num5 = hole.yMin - 24f - num3;
					if (num5 < viewport.yMin + num)
					{
						num5 = hole.yMax + 24f;
					}
				}
			}
			return new Rect(Mathf.Clamp(num4, viewport.xMin + num, viewport.xMax - num - num2), Mathf.Clamp(num5, viewport.yMin + num, viewport.yMax - num - num3), num2, num3);
		}

		// Token: 0x06009FB9 RID: 40889 RVA: 0x003E80BB File Offset: 0x003E62BB
		private static RectTransform CreateRect(string name, Transform parent)
		{
			GameObject gameObject = new GameObject(name, new Type[] { typeof(RectTransform) });
			gameObject.transform.SetParent(parent, false);
			return (RectTransform)gameObject.transform;
		}

		// Token: 0x06009FBA RID: 40890 RVA: 0x0041E858 File Offset: 0x0041CA58
		private static TMP_Text CreateText(string name, Transform parent, string text, float size)
		{
			TextMeshProUGUI textMeshProUGUI = CielOSStorySpotlight.CreateRect(name, parent).gameObject.AddComponent<TextMeshProUGUI>();
			CielOSTheme active = CielOSTheme.Active;
			if (active != null && active.uiFont != null)
			{
				textMeshProUGUI.font = active.uiFont;
			}
			textMeshProUGUI.text = text;
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.color = new Color(0.94f, 0.88f, 1f, 1f);
			textMeshProUGUI.alignment = TextAlignmentOptions.MidlineLeft;
			textMeshProUGUI.enableWordWrapping = true;
			textMeshProUGUI.raycastTarget = false;
			LocalizedGameText.BindSource(textMeshProUGUI, "phone_story", text, null).FitTranslationToRectangle(false);
			return textMeshProUGUI;
		}

		// Token: 0x06009FBB RID: 40891 RVA: 0x0041E8FC File Offset: 0x0041CAFC
		private static void ApplyRect(RectTransform transform, Rect bounds, Rect parentBounds)
		{
			transform.anchorMin = (transform.anchorMax = Vector2.zero);
			transform.pivot = Vector2.zero;
			transform.anchoredPosition = bounds.min - parentBounds.min;
			transform.sizeDelta = new Vector2(Mathf.Max(0f, bounds.width), Mathf.Max(0f, bounds.height));
		}

		// Token: 0x0400A392 RID: 41874
		private static readonly Color Accent = new Color(0.77f, 0.46f, 1f, 1f);

		// Token: 0x0400A393 RID: 41875
		private const float HolePadding = 12f;

		// Token: 0x0400A394 RID: 41876
		private readonly Vector3[] _corners = new Vector3[4];

		// Token: 0x0400A395 RID: 41877
		private readonly Image[] _scrims = new Image[4];

		// Token: 0x0400A396 RID: 41878
		private readonly Image[] _outline = new Image[8];

		// Token: 0x0400A397 RID: 41879
		private RectTransform _overlay;

		// Token: 0x0400A398 RID: 41880
		private RectTransform _hotspotRect;

		// Token: 0x0400A399 RID: 41881
		private Button _hotspot;

		// Token: 0x0400A39A RID: 41882
		private RectTransform _captionRect;

		// Token: 0x0400A39B RID: 41883
		private GameObject _fallbackShortcut;

		// Token: 0x0400A39C RID: 41884
		private CielOSDesktopIcon _icon;

		// Token: 0x0400A39D RID: 41885
		private UnityAction _onClick;
	}
}
