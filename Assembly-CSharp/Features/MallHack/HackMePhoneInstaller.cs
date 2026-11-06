using System;
using Features.Localization;
using Features.OxGram;
using Features.PhoneCompanion;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.MallHack
{
	// Token: 0x0200101A RID: 4122
	public static class HackMePhoneInstaller
	{
		// Token: 0x06007D07 RID: 32007 RVA: 0x00332DD8 File Offset: 0x00330FD8
		public static void Ensure(GameObject phoneInstance, SmartphoneController owner)
		{
			if (phoneInstance == null)
			{
				return;
			}
			if (phoneInstance.GetComponentInChildren<HackMePhoneApp>(true) != null)
			{
				return;
			}
			Transform transform = HackMePhoneInstaller.FindDescendant(phoneInstance.transform, "Home");
			Transform transform2 = ((transform != null) ? transform.parent : null);
			if (transform == null || transform2 == null)
			{
				Debug.LogWarning("[HackMe] Classic phone Home/Panel hierarchy was not found.", phoneInstance);
				return;
			}
			TMP_FontAsset tmp_FontAsset = HackMePhoneInstaller.FindPhoneFont(transform);
			HackMePhoneApp hackMePhoneApp = HackMePhoneInstaller.CreateAppPanel(transform2, owner, tmp_FontAsset);
			if (hackMePhoneApp == null)
			{
				return;
			}
			if (!HackMePhoneInstaller.CreateHomeIcon(transform, hackMePhoneApp, tmp_FontAsset))
			{
				Object.Destroy(hackMePhoneApp.gameObject);
				Debug.LogWarning("[HackMe] Could not create the HackMe Home icon.", phoneInstance);
			}
		}

		// Token: 0x06007D08 RID: 32008 RVA: 0x00332E7C File Offset: 0x0033107C
		private static HackMePhoneApp CreateAppPanel(Transform panel, SmartphoneController owner, TMP_FontAsset font)
		{
			GameObject gameObject = new GameObject("HackMePanel", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image),
				typeof(Mask),
				typeof(Animator)
			});
			gameObject.layer = panel.gameObject.layer;
			gameObject.SetActive(false);
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.SetParent(panel, false);
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.sizeDelta = new Vector2(2.0443f, 4.3207f);
			rectTransform.anchoredPosition3D = Vector3.zero;
			rectTransform.localScale = new Vector3(0.20591347f, 0.20591347f, 0.20591347f);
			Image component = gameObject.GetComponent<Image>();
			component.color = new Color(0.018f, 0.027f, 0.024f, 1f);
			component.raycastTarget = true;
			gameObject.GetComponent<Mask>().showMaskGraphic = true;
			Animator component2 = gameObject.GetComponent<Animator>();
			Animator animator = HackMePhoneInstaller.FindNativeAppAnimator(panel);
			if (animator != null)
			{
				component2.runtimeAnimatorController = animator.runtimeAnimatorController;
			}
			component2.keepAnimatorStateOnDisable = false;
			HackMePhoneApp hackMePhoneApp = gameObject.AddComponent<HackMePhoneApp>();
			hackMePhoneApp.Initialize(owner, font);
			rectTransform.SetAsLastSibling();
			return hackMePhoneApp;
		}

		// Token: 0x06007D09 RID: 32009 RVA: 0x00332FE0 File Offset: 0x003311E0
		private static bool CreateHomeIcon(Transform home, HackMePhoneApp app, TMP_FontAsset font)
		{
			Transform transform = HackMePhoneInstaller.FindDescendant(home, "CielGram");
			if (transform == null)
			{
				Button componentInChildren = home.GetComponentInChildren<Button>(true);
				transform = ((componentInChildren != null) ? componentInChildren.transform : null);
			}
			if (transform == null || transform.parent == null)
			{
				return false;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(transform.gameObject, transform.parent, false);
			gameObject.name = "HackMe";
			gameObject.SetActive(true);
			OxGramIconLauncher component = gameObject.GetComponent<OxGramIconLauncher>();
			if (component != null)
			{
				component.enabled = false;
				Object.Destroy(component);
			}
			Button button = gameObject.GetComponent<Button>();
			if (button == null)
			{
				button = gameObject.AddComponent<Button>();
			}
			button.onClick = new Button.ButtonClickedEvent();
			button.onClick.AddListener(new UnityAction(app.OpenApp));
			Image component2 = gameObject.GetComponent<Image>();
			if (component2 != null)
			{
				component2.type = Image.Type.Simple;
				component2.preserveAspect = false;
				component2.color = Color.black;
				button.targetGraphic = component2;
			}
			Transform transform2 = HackMePhoneInstaller.FindDescendant(gameObject.transform, "MessageRedDot");
			if (transform2 != null)
			{
				transform2.gameObject.SetActive(false);
			}
			TMP_Text tmp_Text = null;
			TMP_Text[] componentsInChildren = gameObject.GetComponentsInChildren<TMP_Text>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].name.StartsWith("IconName"))
				{
					tmp_Text = componentsInChildren[i];
					break;
				}
			}
			if (tmp_Text == null && componentsInChildren.Length != 0)
			{
				tmp_Text = componentsInChildren[0];
			}
			if (tmp_Text != null)
			{
				tmp_Text.text = "HackMe";
				tmp_Text.color = Color.white;
				LocalizedGameText.BindFontOnly(tmp_Text);
				HackMePhoneInstaller.AddTerminalGlyph(gameObject.transform, tmp_Text, font);
			}
			PhoneHomeGridReorder componentInParent = gameObject.GetComponentInParent<PhoneHomeGridReorder>();
			PhoneHomeIconDrag phoneHomeIconDrag = gameObject.GetComponent<PhoneHomeIconDrag>();
			if (componentInParent != null)
			{
				RectTransform rectTransform = gameObject.transform as RectTransform;
				if (rectTransform != null)
				{
					rectTransform.localScale = Vector3.one;
					rectTransform.sizeDelta = componentInParent.Grid.cellSize;
				}
				if (phoneHomeIconDrag == null)
				{
					phoneHomeIconDrag = gameObject.AddComponent<PhoneHomeIconDrag>();
				}
				phoneHomeIconDrag.Init(componentInParent);
				componentInParent.ApplySavedOrder();
				RectTransform rectTransform2 = componentInParent.transform as RectTransform;
				if (rectTransform2 != null)
				{
					LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform2);
				}
			}
			return true;
		}

		// Token: 0x06007D0A RID: 32010 RVA: 0x00333230 File Offset: 0x00331430
		private static void AddTerminalGlyph(Transform icon, TMP_Text labelTemplate, TMP_FontAsset font)
		{
			TMP_Text tmp_Text = Object.Instantiate<TMP_Text>(labelTemplate, icon, false);
			tmp_Text.name = "HackMeGlyph";
			tmp_Text.text = ">_";
			if (font != null)
			{
				tmp_Text.font = font;
			}
			tmp_Text.fontSize = 46f;
			tmp_Text.fontStyle = FontStyles.Bold;
			tmp_Text.color = new Color(0.24f, 1f, 0.53f, 1f);
			tmp_Text.alignment = TextAlignmentOptions.Center;
			tmp_Text.raycastTarget = false;
			RectTransform rectTransform = tmp_Text.rectTransform;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			rectTransform.anchorMax = vector;
			rectTransform.anchorMin = vector;
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.anchoredPosition = Vector2.zero;
			rectTransform.sizeDelta = new Vector2(72f, 68f);
			tmp_Text.transform.SetSiblingIndex(0);
			LocalizedGameText localizedGameText = LocalizedGameText.BindFontOnly(tmp_Text);
			if (font != null)
			{
				localizedGameText.SetSourceFont(font);
			}
		}

		// Token: 0x06007D0B RID: 32011 RVA: 0x0033332C File Offset: 0x0033152C
		private static Animator FindNativeAppAnimator(Transform panel)
		{
			Transform transform = panel.Find("OxGramPanel");
			Animator animator = ((transform != null) ? transform.GetComponent<Animator>() : null);
			if (animator != null && animator.runtimeAnimatorController != null)
			{
				return animator;
			}
			foreach (object obj in panel)
			{
				animator = ((Transform)obj).GetComponent<Animator>();
				if (animator != null && animator.runtimeAnimatorController != null)
				{
					return animator;
				}
			}
			return null;
		}

		// Token: 0x06007D0C RID: 32012 RVA: 0x003333D8 File Offset: 0x003315D8
		private static TMP_FontAsset FindPhoneFont(Transform home)
		{
			TMP_Text componentInChildren = home.GetComponentInChildren<TMP_Text>(true);
			if (!(componentInChildren != null))
			{
				return TMP_Settings.defaultFontAsset;
			}
			return LocalizedGameText.SourceFont(componentInChildren);
		}

		// Token: 0x06007D0D RID: 32013 RVA: 0x00333404 File Offset: 0x00331604
		private static Transform FindDescendant(Transform root, string objectName)
		{
			if (root == null)
			{
				return null;
			}
			Transform[] componentsInChildren = root.GetComponentsInChildren<Transform>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].name == objectName)
				{
					return componentsInChildren[i];
				}
			}
			return null;
		}
	}
}
