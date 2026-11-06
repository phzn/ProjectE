using System;
using System.Collections;
using DA_Assets.CR;
using Features.MainMenu;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Features.Localization
{
	// Token: 0x020010BC RID: 4284
	public sealed class FirstLaunchLanguageScreen : MonoBehaviour
	{
		// Token: 0x060081F6 RID: 33270 RVA: 0x00351748 File Offset: 0x0034F948
		public static IEnumerator ShowIfNeeded(Scene owner, TMP_FontAsset font)
		{
			if (!GameLocalization.NeedsLanguageSelection)
			{
				yield break;
			}
			GameObject root = new GameObject("Choose Your Language", new Type[] { typeof(RectTransform) });
			SceneManager.MoveGameObjectToScene(root, owner);
			FirstLaunchLanguageScreen screen = root.AddComponent<FirstLaunchLanguageScreen>();
			screen.Build(font);
			while (!screen._confirmed)
			{
				yield return null;
			}
			root.SetActive(false);
			Object.Destroy(root);
			yield break;
		}

		// Token: 0x060081F7 RID: 33271 RVA: 0x00351760 File Offset: 0x0034F960
		private void Build(TMP_FontAsset font)
		{
			Canvas canvas = base.gameObject.AddComponent<Canvas>();
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			canvas.sortingOrder = 32760;
			CanvasScaler canvasScaler = base.gameObject.AddComponent<CanvasScaler>();
			canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
			canvasScaler.matchWidthOrHeight = 0.5f;
			base.gameObject.AddComponent<GraphicRaycaster>();
			base.gameObject.AddComponent<Image>().color = MenuDialogShell.Window;
			this._background = LanguageDropdown.Rect("Project Exhibitionist Pattern", base.transform, Vector2.one * 0.5f, Vector2.one * 0.5f);
			RawImage rawImage = this._background.gameObject.AddComponent<RawImage>();
			this._pattern = Resources.Load<Texture2D>("Tutorials/Art/project_exhibitionist_background");
			rawImage.texture = this._pattern;
			rawImage.color = new Color(0.8f, 0.8f, 0.8f, 1f);
			rawImage.raycastTarget = false;
			this._card = LanguageDropdown.Rect("Language Card", base.transform, Vector2.one * 0.5f, Vector2.one * 0.5f);
			this._card.sizeDelta = FirstLaunchLanguageScreen.CardSize;
			MenuDialogShell.Surface(this._card, new Color(0.075f, 0.035f, 0.075f, 0.96f), 24f);
			Shadow shadow = this._card.gameObject.AddComponent<Shadow>();
			shadow.effectColor = new Color(0f, 0f, 0f, 0.3f);
			shadow.effectDistance = new Vector2(0f, -12f);
			MenuDialogShell.Label(MenuDialogShell.Box("Game Name", this._card, 64f, 48f, 912f, 30f), "PROJECT EXHIBITIONIST", 20f, MenuDialogShell.Pink, font, FontStyles.Bold, TextAlignmentOptions.Left).characterSpacing = 3f;
			this.Text("Title", "CHOOSE YOUR LANGUAGE", font, 60f, 96f, 138f, true);
			this.Text("Hint", "You can change this later in Settings.", font, 25f, 252f, 64f, false).color = MenuDialogShell.Muted;
			this._dropdown = LanguageDropdown.Create(this._card, font);
			RectTransform rectTransform = (RectTransform)this._dropdown.transform;
			Vector2 vector = new Vector2(0f, 1f);
			rectTransform.anchorMax = vector;
			rectTransform.anchorMin = vector;
			rectTransform.pivot = new Vector2(0f, 1f);
			rectTransform.sizeDelta = new Vector2(912f, 76f);
			rectTransform.anchoredPosition = new Vector2(64f, -350f);
			FirstLaunchLanguageScreen.StyleDropdown(this._dropdown);
			GameLocalizationConfig config = GameLocalization.Config;
			this._dropdown.SetLanguages((config != null) ? config.languages : null, font);
			this._requestedCode = GameLocalization.RequestedCode;
			this._dropdown.onValueChanged.AddListener(new UnityAction<int>(this.SelectLanguage));
			this._status = this.Text("Status", "", font, 22f, 444f, 64f, false);
			this._status.color = MenuDialogShell.Muted;
			RectTransform rectTransform2 = MenuDialogShell.Box("Continue", this._card, 64f, 544f, 912f, 72f);
			this._continue = MenuDialogShell.ActionButton(rectTransform2, "CONTINUE", font, true);
			TMP_Text componentInChildren = this._continue.GetComponentInChildren<TMP_Text>();
			componentInChildren.fontSize = 26f;
			LocalizedGameText.BindSource(componentInChildren, "startup_ui", "CONTINUE", null).FitTranslationToRectangle(true);
			this._continue.onClick.AddListener(new UnityAction(this.Confirm));
			this._dropdown.navigation = new Navigation
			{
				mode = Navigation.Mode.Explicit,
				selectOnDown = this._continue
			};
			this._continue.navigation = new Navigation
			{
				mode = Navigation.Mode.Explicit,
				selectOnUp = this._dropdown
			};
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			this.Refresh();
			Canvas.ForceUpdateCanvases();
			this.FitComposition();
			if (EventSystem.current != null)
			{
				EventSystem.current.SetSelectedGameObject(this._dropdown.gameObject);
			}
		}

		// Token: 0x060081F8 RID: 33272 RVA: 0x00351BC4 File Offset: 0x0034FDC4
		private TMP_Text Text(string name, string english, TMP_FontAsset font, float size, float top, float height, bool bold = false)
		{
			TMP_Text tmp_Text = MenuDialogShell.Label(MenuDialogShell.Box(name, this._card, 64f, top, 912f, height), english, size, MenuDialogShell.White, font, bold ? FontStyles.Bold : FontStyles.Normal, TextAlignmentOptions.Left);
			tmp_Text.enableAutoSizing = true;
			tmp_Text.fontSizeMin = size * 0.68f;
			tmp_Text.fontSizeMax = size;
			tmp_Text.overflowMode = TextOverflowModes.Overflow;
			LocalizedGameText.BindSource(tmp_Text, "startup_ui", english, null);
			return tmp_Text;
		}

		// Token: 0x060081F9 RID: 33273 RVA: 0x00351C38 File Offset: 0x0034FE38
		private static void StyleDropdown(LanguageDropdown dropdown)
		{
			dropdown.GetComponent<Image>().color = Color.white;
			dropdown.gameObject.AddComponent<CornerRounder>().SetRadii(Vector4.one * 14f);
			ColorBlock colors = dropdown.colors;
			colors.normalColor = MenuDialogShell.Row;
			colors.highlightedColor = new Color(0.27f, 0.18f, 0.31f);
			colors.selectedColor = colors.highlightedColor;
			colors.pressedColor = new Color(0.2f, 0.12f, 0.25f);
			colors.disabledColor = new Color(0.19f, 0.14f, 0.2f, 0.7f);
			dropdown.colors = colors;
			dropdown.captionText.fontSize = (dropdown.captionText.fontSizeMax = 28f);
			dropdown.captionText.fontSizeMin = 20f;
			dropdown.captionText.rectTransform.offsetMin = new Vector2(26f, 4f);
			dropdown.captionText.rectTransform.offsetMax = new Vector2(-70f, -4f);
			dropdown.template.gameObject.AddComponent<CornerRounder>().SetRadii(Vector4.one * 14f);
			dropdown.template.GetComponent<Image>().color = MenuDialogShell.Window;
			dropdown.transform.Find("Open List").gameObject.SetActive(false);
			RectTransform rectTransform = LanguageDropdown.Rect("Chevron", dropdown.transform, new Vector2(1f, 0.5f), new Vector2(1f, 0.5f));
			rectTransform.sizeDelta = new Vector2(24f, 14f);
			rectTransform.anchoredPosition = new Vector2(-36f, 0f);
			for (int i = 0; i < 2; i++)
			{
				RectTransform rectTransform2 = LanguageDropdown.Rect("Stroke", rectTransform, Vector2.one * 0.5f, Vector2.one * 0.5f);
				rectTransform2.sizeDelta = new Vector2(14f, 3f);
				rectTransform2.anchoredPosition = new Vector2((i == 0) ? (-4.5f) : 4.5f, 0f);
				rectTransform2.localRotation = Quaternion.Euler(0f, 0f, (i == 0) ? (-45f) : 45f);
				Image image = rectTransform2.gameObject.AddComponent<Image>();
				image.color = MenuDialogShell.Pink;
				image.raycastTarget = false;
			}
		}

		// Token: 0x060081FA RID: 33274 RVA: 0x00351EB9 File Offset: 0x003500B9
		private void LateUpdate()
		{
			this.FitComposition();
		}

		// Token: 0x060081FB RID: 33275 RVA: 0x00351EC4 File Offset: 0x003500C4
		private void FitComposition()
		{
			if (this._card == null)
			{
				return;
			}
			Vector2 size = ((RectTransform)base.transform).rect.size;
			if (size.x <= 0f || size.y <= 0f)
			{
				return;
			}
			float num = Mathf.Min(new float[]
			{
				1f,
				(size.x - 80f) / FirstLaunchLanguageScreen.CardSize.x,
				(size.y - 80f) / FirstLaunchLanguageScreen.CardSize.y
			});
			this._card.localScale = Vector3.one * Mathf.Max(0.1f, num);
			if (this._pattern != null)
			{
				float num2 = Mathf.Max(size.x / (float)this._pattern.width, size.y / (float)this._pattern.height);
				this._background.sizeDelta = new Vector2((float)this._pattern.width, (float)this._pattern.height) * num2;
			}
		}

		// Token: 0x060081FC RID: 33276 RVA: 0x00351FE4 File Offset: 0x003501E4
		private void SelectLanguage(int index)
		{
			GameLocalizationConfig.Language[] array = ((GameLocalization.Config != null) ? GameLocalization.Config.languages : null);
			if (array == null || index < 0 || index >= array.Length)
			{
				return;
			}
			this._requestedCode = array[index].code;
			GameLocalization.SetLocale(this._requestedCode, false);
			this.Refresh();
		}

		// Token: 0x060081FD RID: 33277 RVA: 0x0035203C File Offset: 0x0035023C
		private void Refresh()
		{
			if (this._dropdown == null || this._continue == null)
			{
				return;
			}
			bool flag = !GameLocalization.IsLoading && GameLocalization.HasLoadedTable;
			this._dropdown.interactable = this._dropdown.options.Count > 0;
			if (GameLocalization.IsLoading)
			{
				this._requestedCode = GameLocalization.RequestedCode;
			}
			this._continue.interactable = flag && GameLocalization.CurrentCode == this._requestedCode;
			if (GameLocalization.Config != null)
			{
				GameLocalizationConfig.Language[] languages = GameLocalization.Config.languages;
				for (int i = 0; i < languages.Length; i++)
				{
					if (languages[i].code == this._requestedCode)
					{
						this._dropdown.SetIndexWithoutNotify(i);
					}
				}
			}
			string text = (GameLocalization.IsLoading ? "Loading language..." : ((!this._continue.interactable) ? "Could not load this language. Please select it again." : ""));
			LocalizedGameText.BindSource(this._status, "startup_ui", text, null);
		}

		// Token: 0x060081FE RID: 33278 RVA: 0x00352149 File Offset: 0x00350349
		private void Confirm()
		{
			if (this._continue.interactable && GameLocalization.ConfirmCurrentLanguage())
			{
				this._confirmed = true;
			}
		}

		// Token: 0x060081FF RID: 33279 RVA: 0x00352166 File Offset: 0x00350366
		private void OnEnable()
		{
			GameLocalization.LocaleChanged += this.Refresh;
			GameLocalization.LoadingChanged += this.Refresh;
			this.Refresh();
		}

		// Token: 0x06008200 RID: 33280 RVA: 0x00352190 File Offset: 0x00350390
		private void OnDisable()
		{
			GameLocalization.LocaleChanged -= this.Refresh;
			GameLocalization.LoadingChanged -= this.Refresh;
		}

		// Token: 0x04008536 RID: 34102
		private static readonly Vector2 CardSize = new Vector2(1040f, 680f);

		// Token: 0x04008537 RID: 34103
		private RectTransform _card;

		// Token: 0x04008538 RID: 34104
		private RectTransform _background;

		// Token: 0x04008539 RID: 34105
		private Texture2D _pattern;

		// Token: 0x0400853A RID: 34106
		private LanguageDropdown _dropdown;

		// Token: 0x0400853B RID: 34107
		private Button _continue;

		// Token: 0x0400853C RID: 34108
		private TMP_Text _status;

		// Token: 0x0400853D RID: 34109
		private string _requestedCode;

		// Token: 0x0400853E RID: 34110
		private bool _confirmed;
	}
}
