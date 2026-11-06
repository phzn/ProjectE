using System;
using DA_Assets.DAB;
using Features.Localization;
using Features.RunModes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.MainMenu
{
	// Token: 0x020010A3 RID: 4259
	[DisallowMultipleComponent]
	public sealed class MainMenuWorkshopMenu : MonoBehaviour
	{
		// Token: 0x060080F6 RID: 33014 RVA: 0x0034963C File Offset: 0x0034783C
		public void BuildIfNeeded()
		{
			if (this._built)
			{
				return;
			}
			MainMenuHubPanels component = base.GetComponent<MainMenuHubPanels>();
			GameObject gameObject = GameObject.Find("btn - Credits");
			Canvas canvas = ((gameObject != null) ? gameObject.GetComponentInParent<Canvas>() : null);
			if (component == null || canvas == null)
			{
				return;
			}
			bool canInitializeSteam = RankedPlatformPolicy.CanInitializeSteam;
			if (canInitializeSteam)
			{
				TMP_Text componentInChildren = gameObject.GetComponentInChildren<TMP_Text>(true);
				MainMenuWorkshopPanel mainMenuWorkshopPanel = MainMenuWorkshopPanel.Build(canvas.transform, LocalizedGameText.SourceFont(componentInChildren), new Action(component.ClosePanels));
				component.RegisterWorkshop(mainMenuWorkshopPanel);
			}
			GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, gameObject.transform.parent);
			gameObject2.name = "btn - Workshop";
			Transform transform = gameObject.transform.parent.Find("btn - Settings");
			gameObject2.transform.SetSiblingIndex((transform != null) ? transform.GetSiblingIndex() : gameObject.transform.GetSiblingIndex());
			TMP_Text componentInChildren2 = gameObject2.GetComponentInChildren<TMP_Text>(true);
			if (componentInChildren2 != null)
			{
				LocalizedGameText.Bind(componentInChildren2, "menu.workshop", "Workshop", null).FitTranslationToRectangle(true);
			}
			CanvasGroup component2 = gameObject2.GetComponent<CanvasGroup>();
			if (component2 != null)
			{
				component2.alpha = 1f;
				component2.interactable = (component2.blocksRaycasts = true);
			}
			DAButton component3 = gameObject2.GetComponent<DAButton>();
			if (component3 != null)
			{
				component3.enabled = false;
			}
			Button button = gameObject2.GetComponent<Button>() ?? gameObject2.AddComponent<Button>();
			button.interactable = canInitializeSteam;
			button.transition = Selectable.Transition.None;
			button.targetGraphic = gameObject2.GetComponentInChildren<Image>(true);
			button.onClick = new Button.ButtonClickedEvent();
			if (canInitializeSteam)
			{
				button.onClick.AddListener(new UnityAction(component.OpenWorkshop));
				if (gameObject2.GetComponent<MainMenuButtonFx>() == null)
				{
					gameObject2.AddComponent<MainMenuButtonFx>();
				}
			}
			else
			{
				button.navigation = new Navigation
				{
					mode = Navigation.Mode.None
				};
				if (component3 != null)
				{
					component3.Interactable = false;
				}
				MainMenuButtonFx component4 = gameObject2.GetComponent<MainMenuButtonFx>();
				if (component4 != null)
				{
					component4.enabled = false;
				}
				BasePropertyAnimatorBase[] componentsInChildren = gameObject2.GetComponentsInChildren<BasePropertyAnimatorBase>(true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = false;
				}
				foreach (Graphic graphic in gameObject2.GetComponentsInChildren<Graphic>(true))
				{
					Color color = graphic.color;
					float num = color.grayscale * 0.65f;
					graphic.color = new Color(num, num, num, color.a);
					graphic.raycastTarget = false;
				}
			}
			RectTransform rectTransform = gameObject2.transform.parent as RectTransform;
			if (rectTransform != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
			}
			MainMenuEntranceAnimator component5 = base.GetComponent<MainMenuEntranceAnimator>();
			if (component5 != null)
			{
				component5.RegisterButton(gameObject2.transform as RectTransform);
			}
			this._built = true;
		}

		// Token: 0x04008438 RID: 33848
		private bool _built;
	}
}
