using System;
using DA_Assets.CR;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.MainMenu
{
	// Token: 0x020010AA RID: 4266
	[DefaultExecutionOrder(-600)]
	public sealed class MenuDialogShell : MonoBehaviour, ICancelHandler, IEventSystemHandler
	{
		// Token: 0x1700148E RID: 5262
		// (get) Token: 0x06008131 RID: 33073 RVA: 0x0034C5B4 File Offset: 0x0034A7B4
		// (set) Token: 0x06008132 RID: 33074 RVA: 0x0034C5BC File Offset: 0x0034A7BC
		public RectTransform Card { get; private set; }

		// Token: 0x1700148F RID: 5263
		// (get) Token: 0x06008133 RID: 33075 RVA: 0x0034C5C5 File Offset: 0x0034A7C5
		// (set) Token: 0x06008134 RID: 33076 RVA: 0x0034C5CD File Offset: 0x0034A7CD
		public TMP_FontAsset Font { get; private set; }

		// Token: 0x17001490 RID: 5264
		// (get) Token: 0x06008135 RID: 33077 RVA: 0x0034C5D6 File Offset: 0x0034A7D6
		public bool IsOpen
		{
			get
			{
				return base.gameObject.activeSelf;
			}
		}

		// Token: 0x06008136 RID: 33078 RVA: 0x0034C5E4 File Offset: 0x0034A7E4
		public static MenuDialogShell Create(Transform canvas, string name, TMP_FontAsset font, Vector2 cardSize)
		{
			RectTransform rectTransform = MenuDialogShell.Rect(name, canvas, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
			rectTransform.gameObject.SetActive(false);
			MenuDialogShell menuDialogShell = rectTransform.gameObject.AddComponent<MenuDialogShell>();
			menuDialogShell._root = rectTransform;
			menuDialogShell.Font = font;
			menuDialogShell._group = rectTransform.gameObject.AddComponent<CanvasGroup>();
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = new Color(0.012f, 0.006f, 0.016f, 0.88f);
			image.raycastTarget = true;
			Canvas canvas2 = rectTransform.gameObject.AddComponent<Canvas>();
			canvas2.overrideSorting = true;
			Canvas componentInParent = canvas.GetComponentInParent<Canvas>();
			canvas2.sortingOrder = ((componentInParent != null) ? (componentInParent.sortingOrder + 200) : 200);
			if (componentInParent != null)
			{
				canvas2.sortingLayerID = componentInParent.sortingLayerID;
			}
			rectTransform.gameObject.AddComponent<GraphicRaycaster>();
			GameplayHudVisibility.RegisterCanvas(canvas2);
			menuDialogShell.Card = MenuDialogShell.Rect("Dialog Card", rectTransform, Vector2.one * 0.5f, Vector2.one * 0.5f, -cardSize * 0.5f, cardSize * 0.5f);
			MenuDialogShell.Surface(menuDialogShell.Card, MenuDialogShell.Window, 24f);
			Shadow shadow = menuDialogShell.Card.gameObject.AddComponent<Shadow>();
			shadow.effectColor = new Color(0f, 0f, 0f, 0.34f);
			shadow.effectDistance = new Vector2(0f, -12f);
			menuDialogShell.SetCardSize(cardSize);
			return menuDialogShell;
		}

		// Token: 0x06008137 RID: 33079 RVA: 0x0034C780 File Offset: 0x0034A980
		public void SetCardSize(Vector2 size)
		{
			this._cardSize = size;
			this.Card.sizeDelta = size;
			this.FitCard(1f);
		}

		// Token: 0x06008138 RID: 33080 RVA: 0x0034C7A0 File Offset: 0x0034A9A0
		public void Show(Button initialFocus, GameObject returnFocus, Action cancel)
		{
			this._initialFocus = initialFocus;
			this._returnFocus = returnFocus;
			this._cancel = cancel;
			this._elapsed = 0f;
			this._group.alpha = (Application.isPlaying ? 0f : 1f);
			this._group.interactable = true;
			this._group.blocksRaycasts = true;
			base.gameObject.SetActive(true);
			base.transform.SetAsLastSibling();
			this.FitCard(Application.isPlaying ? 0.97f : 1f);
			this.SelectInitial();
		}

		// Token: 0x06008139 RID: 33081 RVA: 0x0034C83C File Offset: 0x0034AA3C
		public void Close(bool restoreFocus = true)
		{
			GameObject returnFocus = this._returnFocus;
			this._cancel = null;
			this._returnFocus = null;
			base.gameObject.SetActive(false);
			if (restoreFocus && returnFocus != null && returnFocus.activeInHierarchy && EventSystem.current != null)
			{
				EventSystem.current.SetSelectedGameObject(returnFocus);
			}
		}

		// Token: 0x0600813A RID: 33082 RVA: 0x0034C898 File Offset: 0x0034AA98
		public void OnCancel(BaseEventData eventData)
		{
			Action cancel = this._cancel;
			if (cancel != null)
			{
				cancel();
			}
			else
			{
				this.Close(true);
			}
			if (eventData != null)
			{
				eventData.Use();
			}
		}

		// Token: 0x0600813B RID: 33083 RVA: 0x0034C8C8 File Offset: 0x0034AAC8
		private void Update()
		{
			this._elapsed += Time.unscaledDeltaTime;
			float num = Mathf.Clamp01(this._elapsed / 0.18f);
			float num2 = 1f - Mathf.Pow(1f - num, 3f);
			this._group.alpha = num2;
			this.FitCard(Mathf.Lerp(0.97f, 1f, num2));
			if (ControllerTextKeyboard.IsOpen)
			{
				return;
			}
			if (!EscapeInputCoordinator.TryConsumeForSettings(KeyCode.Escape, this))
			{
				if (EventSystem.current != null)
				{
					GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
					Selectable selectable = ((currentSelectedGameObject != null) ? currentSelectedGameObject.GetComponent<Selectable>() : null);
					if (currentSelectedGameObject == null || !currentSelectedGameObject.transform.IsChildOf(base.transform) || (selectable != null && (!selectable.isActiveAndEnabled || !selectable.IsInteractable())))
					{
						this.SelectInitial();
					}
				}
				return;
			}
			Action cancel = this._cancel;
			if (cancel != null)
			{
				cancel();
				return;
			}
			this.Close(true);
		}

		// Token: 0x0600813C RID: 33084 RVA: 0x0034C9C8 File Offset: 0x0034ABC8
		private void SelectInitial()
		{
			if (EventSystem.current == null)
			{
				return;
			}
			Button button = this._initialFocus;
			if (button == null || !button.isActiveAndEnabled || !button.IsInteractable())
			{
				button = null;
				foreach (Button button2 in this.Card.GetComponentsInChildren<Button>())
				{
					if (button2.IsInteractable())
					{
						button = button2;
						break;
					}
				}
			}
			EventSystem.current.SetSelectedGameObject((button != null) ? button.gameObject : base.gameObject);
		}

		// Token: 0x0600813D RID: 33085 RVA: 0x0034CA50 File Offset: 0x0034AC50
		private void FitCard(float animationScale)
		{
			if (this._root == null || this.Card == null)
			{
				return;
			}
			Vector2 size = this._root.rect.size;
			float num = ((size.x > 0f && size.y > 0f) ? Mathf.Min(new float[]
			{
				1f,
				(size.x - 40f) / this._cardSize.x,
				(size.y - 40f) / this._cardSize.y
			}) : 1f);
			this.Card.localScale = Vector3.one * (Mathf.Max(0.1f, num) * animationScale);
		}

		// Token: 0x0600813E RID: 33086 RVA: 0x0034CB1C File Offset: 0x0034AD1C
		public static void SetNavigation(params Button[] buttons)
		{
			for (int i = 0; i < buttons.Length; i++)
			{
				Navigation navigation = new Navigation
				{
					mode = Navigation.Mode.Explicit
				};
				navigation.selectOnLeft = (navigation.selectOnUp = buttons[(i + buttons.Length - 1) % buttons.Length]);
				navigation.selectOnRight = (navigation.selectOnDown = buttons[(i + 1) % buttons.Length]);
				buttons[i].navigation = navigation;
			}
		}

		// Token: 0x0600813F RID: 33087 RVA: 0x0034CB8A File Offset: 0x0034AD8A
		public static RectTransform Box(string name, Transform parent, float x, float y, float width, float height)
		{
			return MenuDialogShell.Rect(name, parent, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(x, -y - height), new Vector2(x + width, -y));
		}

		// Token: 0x06008140 RID: 33088 RVA: 0x0034CBC8 File Offset: 0x0034ADC8
		public static RectTransform Rect(string name, Transform parent, Vector2 min, Vector2 max, Vector2 offsetMin, Vector2 offsetMax)
		{
			RectTransform component = new GameObject(name, new Type[] { typeof(RectTransform) }).GetComponent<RectTransform>();
			component.SetParent(parent, false);
			component.anchorMin = min;
			component.anchorMax = max;
			component.offsetMin = offsetMin;
			component.offsetMax = offsetMax;
			return component;
		}

		// Token: 0x06008141 RID: 33089 RVA: 0x0034CC19 File Offset: 0x0034AE19
		public static Image Surface(RectTransform rect, Color color, float radius = 14f)
		{
			Image image = rect.gameObject.AddComponent<Image>();
			image.color = color;
			rect.gameObject.AddComponent<CornerRounder>().SetRadii(Vector4.one * radius);
			return image;
		}

		// Token: 0x06008142 RID: 33090 RVA: 0x0034CC48 File Offset: 0x0034AE48
		public static TMP_Text Label(RectTransform rect, string value, float size, Color color, TMP_FontAsset font, FontStyles style = FontStyles.Normal, TextAlignmentOptions alignment = TextAlignmentOptions.Left)
		{
			TextMeshProUGUI textMeshProUGUI = rect.gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.text = value;
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.fontStyle = style;
			textMeshProUGUI.alignment = alignment;
			textMeshProUGUI.color = color;
			textMeshProUGUI.raycastTarget = false;
			textMeshProUGUI.richText = false;
			textMeshProUGUI.enableWordWrapping = true;
			textMeshProUGUI.overflowMode = TextOverflowModes.Ellipsis;
			if (font != null)
			{
				textMeshProUGUI.font = font;
			}
			return textMeshProUGUI;
		}

		// Token: 0x06008143 RID: 33091 RVA: 0x0034CCB8 File Offset: 0x0034AEB8
		public static Button ActionButton(RectTransform rect, string title, TMP_FontAsset font, bool primary = false)
		{
			Image image = MenuDialogShell.Surface(rect, Color.white, 14f);
			Button button = rect.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			ColorBlock colors = button.colors;
			colors.normalColor = (primary ? MenuDialogShell.Pink : MenuDialogShell.Row);
			colors.highlightedColor = (primary ? new Color(1f, 0.49f, 0.72f) : new Color(0.27f, 0.18f, 0.31f));
			colors.selectedColor = colors.highlightedColor;
			colors.pressedColor = (primary ? new Color(0.86f, 0.2f, 0.48f) : new Color(0.2f, 0.12f, 0.25f));
			colors.disabledColor = new Color(0.19f, 0.14f, 0.2f, 0.7f);
			colors.fadeDuration = 0.08f;
			button.colors = colors;
			MenuDialogShell.Label(MenuDialogShell.Rect("Label", rect, Vector2.zero, Vector2.one, new Vector2(14f, 0f), new Vector2(-14f, 0f)), title, 18f, MenuDialogShell.White, font, FontStyles.Bold, TextAlignmentOptions.Center);
			MainMenuButtonFx mainMenuButtonFx = rect.gameObject.AddComponent<MainMenuButtonFx>();
			mainMenuButtonFx.SetTintChildGraphics(false);
			mainMenuButtonFx.SetScaleEffectsEnabled(false);
			return button;
		}

		// Token: 0x04008481 RID: 33921
		public static readonly Color Window = new Color(0.075f, 0.035f, 0.075f, 1f);

		// Token: 0x04008482 RID: 33922
		public static readonly Color Row = new Color(0.165f, 0.118f, 0.2f, 1f);

		// Token: 0x04008483 RID: 33923
		public static readonly Color Pink = new Color(1f, 0.31f, 0.61f, 1f);

		// Token: 0x04008484 RID: 33924
		public static readonly Color White = new Color(1f, 0.97f, 1f, 1f);

		// Token: 0x04008485 RID: 33925
		public static readonly Color Muted = new Color(0.78f, 0.67f, 0.76f, 1f);

		// Token: 0x04008488 RID: 33928
		private CanvasGroup _group;

		// Token: 0x04008489 RID: 33929
		private RectTransform _root;

		// Token: 0x0400848A RID: 33930
		private Vector2 _cardSize;

		// Token: 0x0400848B RID: 33931
		private Button _initialFocus;

		// Token: 0x0400848C RID: 33932
		private GameObject _returnFocus;

		// Token: 0x0400848D RID: 33933
		private Action _cancel;

		// Token: 0x0400848E RID: 33934
		private float _elapsed;
	}
}
