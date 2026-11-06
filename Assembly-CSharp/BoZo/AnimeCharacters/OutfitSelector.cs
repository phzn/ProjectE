using System;
using DA_Assets.CR;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008C0 RID: 2240
	public class OutfitSelector : MonoBehaviour
	{
		// Token: 0x06003C2B RID: 15403 RVA: 0x00165ED0 File Offset: 0x001640D0
		private void Awake()
		{
			if (this.button == null)
			{
				this.button = base.GetComponentInChildren<Button>();
			}
			if (this.button != null)
			{
				this.button.onClick.RemoveListener(new UnityAction(this.OnSelect));
				this.button.onClick.AddListener(new UnityAction(this.OnSelect));
			}
			if (this.itemNameText == null)
			{
				this.itemNameText = base.GetComponentInChildren<TextMeshProUGUI>(true);
			}
			if (this.icon == null)
			{
				this.icon = base.GetComponentInChildren<Image>(true);
			}
		}

		// Token: 0x06003C2C RID: 15404 RVA: 0x00165F73 File Offset: 0x00164173
		public void Init(StyleItemSO styleItem, StyleManager styleManager)
		{
			this.Init(styleItem, styleManager, false);
		}

		// Token: 0x06003C2D RID: 15405 RVA: 0x00165F80 File Offset: 0x00164180
		public void Init(StyleItemSO styleItem, StyleManager styleManager, bool allowToggleOff)
		{
			if (this.button == null)
			{
				this.button = base.GetComponentInChildren<Button>();
			}
			if (this.itemNameText == null)
			{
				this.itemNameText = base.GetComponentInChildren<TextMeshProUGUI>(true);
			}
			if (this.icon == null)
			{
				this.icon = base.GetComponentInChildren<Image>(true);
			}
			if (this.button != null)
			{
				this.button.onClick.RemoveListener(new UnityAction(this.OnSelect));
				this.button.onClick.AddListener(new UnityAction(this.OnSelect));
			}
			this.styleItem = styleItem;
			this.styleManager = styleManager;
			this.allowToggleOff = allowToggleOff;
			if (styleManager != null)
			{
				this.PolishLegacyCard();
			}
			if (styleItem != null)
			{
				if (this.icon != null && styleItem.icon != null)
				{
					this.icon.sprite = styleItem.icon;
					this.icon.gameObject.SetActive(true);
				}
				else if (this.icon != null)
				{
					this.icon.gameObject.SetActive(false);
				}
				if (this.itemNameText != null)
				{
					LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this.itemNameText, "cielos_apps", OutfitSelector.FormatDisplayName(styleItem.itemName), null);
					if (localizedGameText != null)
					{
						localizedGameText.FitTranslationToRectangle(false);
					}
					this.itemNameText.gameObject.SetActive(!string.IsNullOrEmpty(styleItem.itemName));
				}
			}
		}

		// Token: 0x06003C2E RID: 15406 RVA: 0x00166108 File Offset: 0x00164308
		public void Init(Outfit outfit, CharacterCreator characterCreator)
		{
			StyleItemSO styleItemSO = ScriptableObject.CreateInstance<StyleItemSO>();
			styleItemSO.itemName = outfit.OutfitName;
			styleItemSO.icon = outfit.OutfitIcon;
			styleItemSO.itemType = outfit.Type;
			styleItemSO.prefab = outfit.gameObject;
			this.styleItem = styleItemSO;
			if (outfit.OutfitIcon != null)
			{
				this.icon.sprite = outfit.OutfitIcon;
				this.icon.gameObject.SetActive(true);
			}
			else
			{
				this.icon.gameObject.SetActive(false);
			}
			if (this.itemNameText != null)
			{
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this.itemNameText, "cielos_apps", OutfitSelector.FormatDisplayName(outfit.OutfitName), null);
				if (localizedGameText != null)
				{
					localizedGameText.FitTranslationToRectangle(false);
				}
				this.itemNameText.gameObject.SetActive(!string.IsNullOrEmpty(outfit.OutfitName));
			}
			if (this.button == null)
			{
				this.button = base.GetComponentInChildren<Button>();
			}
			this.button.onClick.AddListener(delegate
			{
				if (characterCreator != null)
				{
					characterCreator.SetOutfit(outfit);
				}
				this.OnOutfitSelected.Invoke(outfit);
			});
		}

		// Token: 0x06003C2F RID: 15407 RVA: 0x00166264 File Offset: 0x00164464
		private void OnSelect()
		{
			if (this.styleItem == null)
			{
				return;
			}
			if (this.styleManager != null)
			{
				if (this.allowToggleOff && this.styleManager.IsStyleEquipped(this.styleItem))
				{
					this.styleManager.RemoveStyle(this.styleItem.itemType);
				}
				else
				{
					this.styleManager.ApplyStyle(this.styleItem);
				}
			}
			this.OnStyleItemSelected.Invoke(this.styleItem);
		}

		// Token: 0x06003C30 RID: 15408 RVA: 0x001662E4 File Offset: 0x001644E4
		public void SetVisible(OutfitType type)
		{
			bool flag = false;
			if (this.styleItem != null && this.styleItem.itemType == type)
			{
				flag = true;
			}
			base.gameObject.SetActive(flag);
		}

		// Token: 0x06003C31 RID: 15409 RVA: 0x00166322 File Offset: 0x00164522
		public void SetVisable(OutfitType type)
		{
			this.SetVisible(type);
		}

		// Token: 0x06003C32 RID: 15410 RVA: 0x0016632B File Offset: 0x0016452B
		public OutfitType GetItemType()
		{
			StyleItemSO styleItemSO = this.styleItem;
			if (styleItemSO == null)
			{
				return null;
			}
			return styleItemSO.itemType;
		}

		// Token: 0x06003C33 RID: 15411 RVA: 0x0016633E File Offset: 0x0016453E
		public StyleItemSO GetStyleItem()
		{
			return this.styleItem;
		}

		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x06003C34 RID: 15412 RVA: 0x000AD00A File Offset: 0x000AB20A
		public Transform ContextualControlsRoot
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x06003C35 RID: 15413 RVA: 0x00166346 File Offset: 0x00164546
		public void SetNameRaised(bool raised)
		{
			if (this.itemNameText == null)
			{
				return;
			}
			this.itemNameText.rectTransform.anchoredPosition = new Vector2(0f, raised ? 25f : 10f);
		}

		// Token: 0x06003C36 RID: 15414 RVA: 0x00166380 File Offset: 0x00164580
		public void SetActiveVisual(bool active)
		{
			if (this.selectedVisual != null)
			{
				this.selectedVisual.SetActive(false);
			}
			if (this.cardOutline != null)
			{
				this.cardOutline.effectColor = (active ? new Color32(230, 84, 123, byte.MaxValue) : new Color32(156, 86, 189, byte.MaxValue));
				this.cardOutline.effectDistance = (active ? new Vector2(4f, -4f) : new Vector2(3f, -3f));
			}
			if (!active)
			{
				for (int i = base.transform.childCount - 1; i >= 0; i--)
				{
					Transform child = base.transform.GetChild(i);
					if (!(child.name != "Context Colors") || !(child.name != "Context Palettes") || !(child.name != "Context Variants"))
					{
						Transform transform = child;
						transform.name += " (stale)";
						child.gameObject.SetActive(false);
						Object.Destroy(child.gameObject);
					}
				}
				this.SetNameRaised(false);
			}
		}

		// Token: 0x06003C37 RID: 15415 RVA: 0x001664B8 File Offset: 0x001646B8
		private void PolishLegacyCard()
		{
			RectTransform rectTransform = base.transform as RectTransform;
			if (rectTransform != null)
			{
				rectTransform.sizeDelta = new Vector2(180f, 180f);
			}
			LayoutElement layoutElement = base.GetComponent<LayoutElement>();
			if (layoutElement == null)
			{
				layoutElement = base.gameObject.AddComponent<LayoutElement>();
			}
			layoutElement.preferredWidth = 180f;
			layoutElement.preferredHeight = 180f;
			Image image = this.FindImage("Backing");
			Image image2 = this.FindImage("Boarder");
			if (image2 == null)
			{
				image2 = this.FindImage("Border");
			}
			if (image != null)
			{
				image.gameObject.SetActive(false);
			}
			if (image2 != null)
			{
				image2.gameObject.SetActive(false);
			}
			if (this.selectedVisual != null)
			{
				this.selectedVisual.SetActive(false);
			}
			Mask component = base.GetComponent<Mask>();
			if (component != null)
			{
				component.enabled = false;
			}
			RectMask2D component2 = base.GetComponent<RectMask2D>();
			if (component2 != null)
			{
				component2.enabled = false;
			}
			Image image3 = base.GetComponent<Image>();
			if (image3 == null)
			{
				image3 = base.gameObject.AddComponent<Image>();
			}
			image3.color = new Color32(54, 34, 68, byte.MaxValue);
			image3.raycastTarget = true;
			if (this.button != null)
			{
				this.button.targetGraphic = image3;
			}
			CornerRounder cornerRounder = base.GetComponent<CornerRounder>();
			if (cornerRounder == null)
			{
				cornerRounder = base.gameObject.AddComponent<CornerRounder>();
			}
			cornerRounder.SetRadii(new Vector4(14f, 14f, 14f, 14f));
			this.cardOutline = base.GetComponent<Outline>();
			if (this.cardOutline == null)
			{
				this.cardOutline = base.gameObject.AddComponent<Outline>();
			}
			this.cardOutline.effectColor = new Color32(156, 86, 189, byte.MaxValue);
			this.cardOutline.effectDistance = new Vector2(3f, -3f);
			if (this.icon != null)
			{
				this.icon.preserveAspect = true;
				this.icon.raycastTarget = false;
				RectTransform rectTransform2 = this.icon.rectTransform;
				rectTransform2.anchorMin = Vector2.zero;
				rectTransform2.anchorMax = Vector2.one;
				rectTransform2.offsetMin = new Vector2(16f, 62f);
				rectTransform2.offsetMax = new Vector2(-16f, -16f);
			}
			if (this.itemNameText == null)
			{
				GameObject gameObject = new GameObject("Item Name", new Type[]
				{
					typeof(RectTransform),
					typeof(CanvasRenderer),
					typeof(TextMeshProUGUI)
				});
				gameObject.layer = base.gameObject.layer;
				gameObject.transform.SetParent(base.transform, false);
				this.itemNameText = gameObject.GetComponent<TextMeshProUGUI>();
			}
			RectTransform rectTransform3 = this.itemNameText.rectTransform;
			rectTransform3.anchorMin = new Vector2(0f, 0f);
			rectTransform3.anchorMax = new Vector2(1f, 0f);
			rectTransform3.pivot = new Vector2(0.5f, 0f);
			rectTransform3.anchoredPosition = new Vector2(0f, 10f);
			rectTransform3.sizeDelta = new Vector2(-18f, 26f);
			this.itemNameText.alignment = TextAlignmentOptions.Center;
			this.itemNameText.fontSize = 14f;
			this.itemNameText.fontStyle = FontStyles.Bold | FontStyles.SmallCaps;
			this.itemNameText.enableAutoSizing = true;
			this.itemNameText.fontSizeMin = 10f;
			this.itemNameText.fontSizeMax = 14f;
			this.itemNameText.color = Color.white;
			this.itemNameText.raycastTarget = false;
			if (this.button != null)
			{
				ColorBlock colors = this.button.colors;
				colors.normalColor = Color.white;
				colors.highlightedColor = new Color(1f, 1f, 1f, 0.88f);
				colors.pressedColor = new Color(0.82f, 0.82f, 0.82f, 1f);
				colors.selectedColor = Color.white;
				colors.fadeDuration = 0.08f;
				this.button.colors = colors;
			}
		}

		// Token: 0x06003C38 RID: 15416 RVA: 0x0016692C File Offset: 0x00164B2C
		private Image FindImage(string childName)
		{
			Transform transform = base.transform.Find(childName);
			if (!(transform != null))
			{
				return null;
			}
			return transform.GetComponent<Image>();
		}

		// Token: 0x06003C39 RID: 15417 RVA: 0x00166958 File Offset: 0x00164B58
		private static string FormatDisplayName(string rawName)
		{
			if (string.IsNullOrWhiteSpace(rawName))
			{
				return string.Empty;
			}
			string text = rawName.Replace('_', ' ').Trim();
			string[] array = new string[] { "Hair Front ", "HairFront ", "Hair Back ", "HairBack " };
			for (int i = 0; i < array.Length; i++)
			{
				if (text.StartsWith(array[i], StringComparison.OrdinalIgnoreCase))
				{
					text = text.Substring(array[i].Length).Trim();
					break;
				}
			}
			return text;
		}

		// Token: 0x04003C3E RID: 15422
		[SerializeField]
		private Image icon;

		// Token: 0x04003C3F RID: 15423
		[SerializeField]
		private TextMeshProUGUI itemNameText;

		// Token: 0x04003C40 RID: 15424
		[SerializeField]
		private GameObject selectedVisual;

		// Token: 0x04003C41 RID: 15425
		private StyleItemSO styleItem;

		// Token: 0x04003C42 RID: 15426
		private StyleManager styleManager;

		// Token: 0x04003C43 RID: 15427
		private Button button;

		// Token: 0x04003C44 RID: 15428
		private bool allowToggleOff;

		// Token: 0x04003C45 RID: 15429
		private Outline cardOutline;

		// Token: 0x04003C46 RID: 15430
		public UnityEvent<StyleItemSO> OnStyleItemSelected = new UnityEvent<StyleItemSO>();

		// Token: 0x04003C47 RID: 15431
		public UnityEvent<Outfit> OnOutfitSelected = new UnityEvent<Outfit>();
	}
}
