using System;
using DA_Assets.CR;
using DA_Assets.DAB;
using Features.Localization;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.MainMenu
{
	// Token: 0x02001062 RID: 4194
	[DisallowMultipleComponent]
	public sealed class MainMenuExitMenu : MonoBehaviour
	{
		// Token: 0x06007F47 RID: 32583 RVA: 0x00340420 File Offset: 0x0033E620
		public void BuildIfNeeded()
		{
			if (this._built)
			{
				return;
			}
			GameObject gameObject = GameObject.Find("btn - Credits");
			if (gameObject == null)
			{
				Debug.LogWarning("[MainMenu] EXIT GAME not created: 'btn - Credits' template missing.");
				return;
			}
			Canvas componentInParent = gameObject.GetComponentInParent<Canvas>();
			if (componentInParent == null)
			{
				Debug.LogWarning("[MainMenu] EXIT GAME not created: Main Menu Canvas missing.");
				return;
			}
			this.BuildExitEntry(gameObject);
			this.BuildConfirmation(componentInParent.transform, gameObject.GetComponentInChildren<TMP_Text>(true));
			this._built = true;
		}

		// Token: 0x06007F48 RID: 32584 RVA: 0x00340491 File Offset: 0x0033E691
		private void Update()
		{
			if (this._prompt == null || !this._prompt.activeSelf)
			{
				return;
			}
			if (EscapeInputCoordinator.TryConsume(this))
			{
				this.CloseConfirmation();
			}
		}

		// Token: 0x06007F49 RID: 32585 RVA: 0x003404C0 File Offset: 0x0033E6C0
		private void BuildExitEntry(GameObject template)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(template, template.transform.parent);
			gameObject.name = "btn - Exit Game";
			gameObject.transform.SetAsLastSibling();
			TMP_Text componentInChildren = gameObject.GetComponentInChildren<TMP_Text>(true);
			if (componentInChildren != null)
			{
				LocalizedGameText.BindSource(componentInChildren, "settings", "Exit Game", null);
			}
			CanvasGroup component = gameObject.GetComponent<CanvasGroup>();
			if (component != null)
			{
				component.alpha = 1f;
				component.interactable = true;
				component.blocksRaycasts = true;
			}
			DAButton component2 = gameObject.GetComponent<DAButton>();
			if (component2 != null)
			{
				component2.enabled = false;
			}
			Button button = gameObject.GetComponent<Button>();
			if (button == null)
			{
				button = gameObject.AddComponent<Button>();
			}
			button.transition = Selectable.Transition.None;
			button.targetGraphic = gameObject.GetComponentInChildren<Image>(true);
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(new UnityAction(this.OpenConfirmation));
			if (gameObject.GetComponent<MainMenuButtonFx>() == null)
			{
				gameObject.AddComponent<MainMenuButtonFx>();
			}
			RectTransform rectTransform = gameObject.transform.parent as RectTransform;
			if (rectTransform != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
			}
			MainMenuEntranceAnimator component3 = base.GetComponent<MainMenuEntranceAnimator>();
			if (component3 != null)
			{
				component3.RegisterButton(gameObject.transform as RectTransform);
			}
		}

		// Token: 0x06007F4A RID: 32586 RVA: 0x00340604 File Offset: 0x0033E804
		private void BuildConfirmation(Transform canvas, TMP_Text styleSource)
		{
			this._prompt = MainMenuExitMenu.CreateRect("ExitConfirmation", canvas, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero).gameObject;
			this._prompt.transform.SetAsLastSibling();
			Image image = this._prompt.AddComponent<Image>();
			image.color = MainMenuExitMenu.Overlay;
			image.raycastTarget = true;
			RectTransform rectTransform = MainMenuExitMenu.CreateRect("Card", this._prompt.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-360f, -190f), new Vector2(360f, 190f));
			rectTransform.gameObject.AddComponent<Image>().color = MainMenuExitMenu.Window;
			MainMenuExitMenu.Round(rectTransform.gameObject, 22f);
			TMP_FontAsset tmp_FontAsset = LocalizedGameText.SourceFont(styleSource);
			MainMenuExitMenu.CreateText("Title", rectTransform, "EXIT GAME?", 48f, FontStyles.Bold, TextAlignmentOptions.Center, MainMenuExitMenu.White, new Vector2(0f, 0.59f), new Vector2(1f, 0.84f), new Vector2(42f, 0f), new Vector2(-42f, 0f), tmp_FontAsset);
			MainMenuExitMenu.CreateText("Description", rectTransform, "Are you sure you want to close the game?", 21f, FontStyles.Normal, TextAlignmentOptions.Center, MainMenuExitMenu.Muted, new Vector2(0f, 0.36f), new Vector2(1f, 0.54f), new Vector2(42f, 0f), new Vector2(-42f, 0f), tmp_FontAsset);
			this._cancelButton = MainMenuExitMenu.CreateActionButton(rectTransform, "Cancel", "CANCEL", new Vector2(0.08f, 0.1f), new Vector2(0.48f, 0.3f), tmp_FontAsset, MainMenuExitMenu.White);
			this._cancelButton.onClick.AddListener(new UnityAction(this.CloseConfirmation));
			ControllerNavigationScope.Configure(this._prompt, new Action(this.CloseConfirmation), this._cancelButton.gameObject, 100, false);
			MainMenuExitMenu.CreateActionButton(rectTransform, "Exit", "EXIT GAME", new Vector2(0.52f, 0.1f), new Vector2(0.92f, 0.3f), tmp_FontAsset, MainMenuExitMenu.Pink).onClick.AddListener(new UnityAction(MainMenuExitMenu.QuitGame));
			this._prompt.SetActive(false);
		}

		// Token: 0x06007F4B RID: 32587 RVA: 0x00340874 File Offset: 0x0033EA74
		private void OpenConfirmation()
		{
			if (this._prompt == null)
			{
				return;
			}
			this._prompt.SetActive(true);
			this._prompt.transform.SetAsLastSibling();
			if (EventSystem.current != null && this._cancelButton != null)
			{
				EventSystem.current.SetSelectedGameObject(this._cancelButton.gameObject);
			}
		}

		// Token: 0x06007F4C RID: 32588 RVA: 0x003408DC File Offset: 0x0033EADC
		private void CloseConfirmation()
		{
			if (this._prompt != null)
			{
				this._prompt.SetActive(false);
			}
		}

		// Token: 0x06007F4D RID: 32589 RVA: 0x003408F8 File Offset: 0x0033EAF8
		private static void QuitGame()
		{
			Application.Quit();
		}

		// Token: 0x06007F4E RID: 32590 RVA: 0x00340900 File Offset: 0x0033EB00
		private static Button CreateActionButton(Transform parent, string name, string label, Vector2 anchorMin, Vector2 anchorMax, TMP_FontAsset font, Color textColor)
		{
			RectTransform rectTransform = MainMenuExitMenu.CreateRect(name, parent, anchorMin, anchorMax, Vector2.zero, Vector2.zero);
			Image image = rectTransform.gameObject.AddComponent<Image>();
			image.color = MainMenuExitMenu.Row;
			MainMenuExitMenu.Round(rectTransform.gameObject, 14f);
			Button button = rectTransform.gameObject.AddComponent<Button>();
			button.targetGraphic = image;
			button.transition = Selectable.Transition.ColorTint;
			ColorBlock colors = button.colors;
			colors.normalColor = MainMenuExitMenu.Row;
			colors.highlightedColor = new Color(0.243f, 0.165f, 0.298f, 1f);
			colors.pressedColor = new Color(0.41f, 0.35f, 0.45f, 1f);
			colors.selectedColor = colors.highlightedColor;
			colors.fadeDuration = 0.08f;
			button.colors = colors;
			MainMenuExitMenu.CreateText("Label", rectTransform, label, 21f, FontStyles.Bold, TextAlignmentOptions.Center, textColor, Vector2.zero, Vector2.one, new Vector2(12f, 0f), new Vector2(-12f, 0f), font);
			rectTransform.gameObject.AddComponent<MainMenuButtonFx>();
			return button;
		}

		// Token: 0x06007F4F RID: 32591 RVA: 0x00340A28 File Offset: 0x0033EC28
		private static TMP_Text CreateText(string name, Transform parent, string value, float size, FontStyles style, TextAlignmentOptions alignment, Color color, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax, TMP_FontAsset font)
		{
			TextMeshProUGUI textMeshProUGUI = MainMenuExitMenu.CreateRect(name, parent, anchorMin, anchorMax, offsetMin, offsetMax).gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.fontSize = size;
			textMeshProUGUI.fontStyle = style;
			textMeshProUGUI.alignment = alignment;
			textMeshProUGUI.color = color;
			textMeshProUGUI.raycastTarget = false;
			if (font != null)
			{
				textMeshProUGUI.font = font;
			}
			LocalizedGameText.BindSource(textMeshProUGUI, "settings", value, null);
			return textMeshProUGUI;
		}

		// Token: 0x06007F50 RID: 32592 RVA: 0x00340A98 File Offset: 0x0033EC98
		private static RectTransform CreateRect(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
		{
			RectTransform component = new GameObject(name, new Type[] { typeof(RectTransform) }).GetComponent<RectTransform>();
			component.SetParent(parent, false);
			component.anchorMin = anchorMin;
			component.anchorMax = anchorMax;
			component.offsetMin = offsetMin;
			component.offsetMax = offsetMax;
			component.localScale = Vector3.one;
			return component;
		}

		// Token: 0x06007F51 RID: 32593 RVA: 0x001A3764 File Offset: 0x001A1964
		private static void Round(GameObject target, float radius)
		{
			target.AddComponent<CornerRounder>().SetRadii(new Vector4(radius, radius, radius, radius));
		}

		// Token: 0x040082CA RID: 33482
		private static readonly Color Overlay = new Color(0.012f, 0.006f, 0.016f, 0.88f);

		// Token: 0x040082CB RID: 33483
		private static readonly Color Window = new Color(0.075f, 0.035f, 0.075f, 0.99f);

		// Token: 0x040082CC RID: 33484
		private static readonly Color Row = new Color(0.165f, 0.118f, 0.2f, 0.98f);

		// Token: 0x040082CD RID: 33485
		private static readonly Color Pink = new Color(1f, 0.31f, 0.61f, 1f);

		// Token: 0x040082CE RID: 33486
		private static readonly Color White = new Color(1f, 0.97f, 1f, 1f);

		// Token: 0x040082CF RID: 33487
		private static readonly Color Muted = new Color(0.78f, 0.67f, 0.76f, 1f);

		// Token: 0x040082D0 RID: 33488
		private GameObject _prompt;

		// Token: 0x040082D1 RID: 33489
		private Button _cancelButton;

		// Token: 0x040082D2 RID: 33490
		private bool _built;
	}
}
