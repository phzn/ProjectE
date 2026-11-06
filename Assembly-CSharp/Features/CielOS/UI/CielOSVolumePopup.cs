using System;
using Features.MainMenu;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CielOS.UI
{
	// Token: 0x020013D2 RID: 5074
	public class CielOSVolumePopup : MonoBehaviour
	{
		// Token: 0x170018D5 RID: 6357
		// (get) Token: 0x06009CAC RID: 40108 RVA: 0x00403F74 File Offset: 0x00402174
		// (set) Token: 0x06009CAD RID: 40109 RVA: 0x00403F7B File Offset: 0x0040217B
		public static CielOSVolumePopup Instance { get; private set; }

		// Token: 0x06009CAE RID: 40110 RVA: 0x00403F84 File Offset: 0x00402184
		public static CielOSVolumePopup Ensure(Transform canvas)
		{
			if (CielOSVolumePopup.Instance != null)
			{
				return CielOSVolumePopup.Instance;
			}
			GameObject gameObject = new GameObject("CielOSVolumePopup", new Type[] { typeof(RectTransform) });
			gameObject.transform.SetParent(canvas, false);
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			CielOSVolumePopup.Instance = gameObject.AddComponent<CielOSVolumePopup>();
			CielOSVolumePopup.Instance.Build();
			return CielOSVolumePopup.Instance;
		}

		// Token: 0x06009CAF RID: 40111 RVA: 0x00404020 File Offset: 0x00402220
		private void Build()
		{
			CielOSTheme active = CielOSTheme.Active;
			GameSettingsService.ApplyAudio();
			GameObject gameObject = new GameObject("Blocker", new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(Button)
			});
			gameObject.transform.SetParent(base.transform, false);
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
			gameObject.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
			gameObject.GetComponent<Button>().onClick.AddListener(new UnityAction(this.Close));
			GameObject gameObject2 = new GameObject("Card", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject2.transform.SetParent(base.transform, false);
			this._panel = (RectTransform)gameObject2.transform;
			this._panel.sizeDelta = new Vector2(220f, 56f);
			Image component = gameObject2.GetComponent<Image>();
			component.color = ((active != null) ? active.panelBackground : new Color(0.09f, 0.06f, 0.12f, 1f));
			CielOSTheme.Round(component);
			CielOSTheme.AddBorder(this._panel, null, 1f);
			GameObject gameObject3 = new GameObject("Glyph", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject3.transform.SetParent(this._panel, false);
			RectTransform rectTransform2 = (RectTransform)gameObject3.transform;
			rectTransform2.anchorMin = new Vector2(0f, 0.5f);
			rectTransform2.anchorMax = new Vector2(0f, 0.5f);
			rectTransform2.pivot = new Vector2(0f, 0.5f);
			rectTransform2.sizeDelta = new Vector2(22f, 22f);
			rectTransform2.anchoredPosition = new Vector2(14f, 0f);
			this._glyph = gameObject3.GetComponent<Image>();
			CielOSIcon.Apply(this._glyph, CielOSIconId.Volume, (active != null) ? active.trayIconTint : Color.white, 64);
			this._slider = this.BuildSlider(this._panel);
			RectTransform rectTransform3 = (RectTransform)this._slider.transform;
			rectTransform3.anchorMin = new Vector2(0f, 0.5f);
			rectTransform3.anchorMax = new Vector2(1f, 0.5f);
			rectTransform3.pivot = new Vector2(0f, 0.5f);
			rectTransform3.offsetMin = new Vector2(46f, -9f);
			rectTransform3.offsetMax = new Vector2(-16f, 9f);
			this._slider.value = GameSettingsService.MasterVolume;
			this._slider.onValueChanged.AddListener(delegate(float v)
			{
				GameSettingsService.SetMasterVolume(v);
			});
			base.gameObject.SetActive(false);
		}

		// Token: 0x06009CB0 RID: 40112 RVA: 0x00404368 File Offset: 0x00402568
		public void ToggleAt(RectTransform anchor)
		{
			if (base.gameObject.activeSelf)
			{
				this.Close();
				return;
			}
			base.gameObject.SetActive(true);
			base.transform.SetAsLastSibling();
			this._slider.SetValueWithoutNotify(GameSettingsService.MasterVolume);
			RectTransform rectTransform = (RectTransform)base.transform;
			Vector2 vector = RectTransformUtility.WorldToScreenPoint(null, anchor.position);
			Vector2 vector2;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, vector, null, out vector2);
			RectTransform panel = this._panel;
			RectTransform panel2 = this._panel;
			Vector2 vector3 = new Vector2(0.5f, 0.5f);
			panel2.anchorMax = vector3;
			panel.anchorMin = vector3;
			this._panel.pivot = new Vector2(0.5f, 1f);
			this._panel.anchoredPosition = new Vector2(Mathf.Clamp(vector2.x, -rectTransform.rect.width / 2f + 120f, rectTransform.rect.width / 2f - 120f), vector2.y - 22f);
		}

		// Token: 0x06009CB1 RID: 40113 RVA: 0x00112329 File Offset: 0x00110529
		public void Close()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06009CB2 RID: 40114 RVA: 0x00404478 File Offset: 0x00402678
		private Slider BuildSlider(Transform parent)
		{
			CielOSTheme active = CielOSTheme.Active;
			GameObject gameObject = new GameObject("Slider", new Type[]
			{
				typeof(RectTransform),
				typeof(Slider)
			});
			gameObject.transform.SetParent(parent, false);
			Slider component = gameObject.GetComponent<Slider>();
			GameObject gameObject2 = new GameObject("BG", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			CielOSVolumePopup.Stretch((RectTransform)gameObject2.transform);
			Image component2 = gameObject2.GetComponent<Image>();
			component2.color = ((active != null) ? active.barTrack : new Color(0.2f, 0.15f, 0.26f, 1f));
			CielOSTheme.Round(component2);
			GameObject gameObject3 = new GameObject("Fill Area", new Type[] { typeof(RectTransform) });
			gameObject3.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform = (RectTransform)gameObject3.transform;
			rectTransform.anchorMin = new Vector2(0f, 0.25f);
			rectTransform.anchorMax = new Vector2(1f, 0.75f);
			rectTransform.offsetMin = new Vector2(2f, 0f);
			rectTransform.offsetMax = new Vector2(-2f, 0f);
			GameObject gameObject4 = new GameObject("Fill", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject4.transform.SetParent(gameObject3.transform, false);
			RectTransform rectTransform2 = (RectTransform)gameObject4.transform;
			rectTransform2.anchorMin = Vector2.zero;
			rectTransform2.anchorMax = Vector2.one;
			rectTransform2.sizeDelta = Vector2.zero;
			Image component3 = gameObject4.GetComponent<Image>();
			component3.color = ((active != null) ? active.barFill : new Color(0.66f, 0.38f, 0.79f, 1f));
			CielOSTheme.Round(component3);
			GameObject gameObject5 = new GameObject("Handle Slide Area", new Type[] { typeof(RectTransform) });
			gameObject5.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform3 = (RectTransform)gameObject5.transform;
			CielOSVolumePopup.Stretch(rectTransform3);
			rectTransform3.offsetMin = new Vector2(8f, 0f);
			rectTransform3.offsetMax = new Vector2(-8f, 0f);
			GameObject gameObject6 = new GameObject("Handle", new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject6.transform.SetParent(gameObject5.transform, false);
			RectTransform rectTransform4 = (RectTransform)gameObject6.transform;
			rectTransform4.sizeDelta = new Vector2(14f, 14f);
			Image component4 = gameObject6.GetComponent<Image>();
			component4.color = ((active != null) ? active.logoAccent : Color.white);
			CielOSTheme.Circle(component4);
			component.fillRect = rectTransform2;
			component.handleRect = rectTransform4;
			component.targetGraphic = component4;
			component.direction = Slider.Direction.LeftToRight;
			component.minValue = 0f;
			component.maxValue = 1f;
			return component;
		}

		// Token: 0x06009CB3 RID: 40115 RVA: 0x001A38AD File Offset: 0x001A1AAD
		private static void Stretch(RectTransform rt)
		{
			rt.anchorMin = Vector2.zero;
			rt.anchorMax = Vector2.one;
			rt.offsetMin = Vector2.zero;
			rt.offsetMax = Vector2.zero;
		}

		// Token: 0x0400A142 RID: 41282
		private RectTransform _panel;

		// Token: 0x0400A143 RID: 41283
		private Slider _slider;

		// Token: 0x0400A144 RID: 41284
		private Image _glyph;
	}
}
