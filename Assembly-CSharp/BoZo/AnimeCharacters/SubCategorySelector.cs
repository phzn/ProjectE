using System;
using System.Collections;
using System.Collections.Generic;
using Features.Localization;
using ProjectExhibitionist.Core;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Bozo.AnimeCharacters
{
	// Token: 0x020008B3 RID: 2227
	public class SubCategorySelector : MonoBehaviour, IScrollHandler, IEventSystemHandler, IBeginDragHandler, IEndDragHandler, IControllerScrollSync
	{
		// Token: 0x06003BBB RID: 15291 RVA: 0x00163168 File Offset: 0x00161368
		public void Init(List<CharacterSubCategorySO> subCategories, Action<CharacterSubCategorySO> callback)
		{
			ControllerShortcutHint.Bind(base.transform, "LT + (LB / RB)", false);
			this.subCategories = subCategories ?? new List<CharacterSubCategorySO>();
			this.onSubCategorySelected = callback;
			this.EnsureHorizontalScroll();
			this.smoothingScroll = false;
			this.draggingScroll = false;
			if (this.horizontalScroll != null)
			{
				this.horizontalScroll.StopMovement();
			}
			foreach (Button button in this.buttons)
			{
				if (button != null)
				{
					button.gameObject.SetActive(false);
					Object.Destroy(button.gameObject);
				}
			}
			this.buttons.Clear();
			this.selectionBackgrounds.Clear();
			this.selectionColors.Clear();
			this.buttonData.Clear();
			this.persistentContents.Clear();
			foreach (CharacterSubCategorySO characterSubCategorySO in this.subCategories)
			{
				this.CreateButton(characterSubCategorySO);
			}
			this.ConfigureContentSizing();
			RectTransform rectTransform = this.buttonContainer as RectTransform;
			if (rectTransform != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
			}
			if (this.horizontalScroll != null)
			{
				this.horizontalScroll.horizontalNormalizedPosition = 0f;
				this.targetNormalizedPosition = 0f;
			}
			if (this.resetScrollRoutine != null)
			{
				base.StopCoroutine(this.resetScrollRoutine);
			}
			this.resetScrollRoutine = base.StartCoroutine(this.ResetScrollAfterLayout());
			if (this.subCategories.Count > 0 && this.buttons.Count > 0)
			{
				this.SelectSubCategory(this.subCategories[0]);
			}
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x0016333C File Offset: 0x0016153C
		private void CreateButton(CharacterSubCategorySO subCategory)
		{
			if (this.buttonPrefab == null || this.buttonContainer == null)
			{
				return;
			}
			GameObject gameObject = Object.Instantiate<GameObject>(this.buttonPrefab, this.buttonContainer);
			gameObject.SetActive(true);
			LayoutElement layoutElement = gameObject.GetComponent<LayoutElement>();
			if (layoutElement == null)
			{
				layoutElement = gameObject.AddComponent<LayoutElement>();
			}
			layoutElement.minWidth = this.fixedButtonWidth;
			layoutElement.preferredWidth = this.fixedButtonWidth;
			layoutElement.flexibleWidth = 0f;
			Button button = gameObject.GetComponent<Button>();
			if (button != null)
			{
				Mask component = gameObject.GetComponent<Mask>();
				if (component != null)
				{
					component.enabled = false;
				}
				RectMask2D component2 = gameObject.GetComponent<RectMask2D>();
				if (component2 != null)
				{
					component2.enabled = false;
				}
				Image image = gameObject.GetComponent<Image>();
				if (image == null)
				{
					image = gameObject.AddComponent<Image>();
				}
				image.color = new Color(1f, 1f, 1f, 0.001f);
				image.raycastTarget = true;
				button.targetGraphic = image;
				this.ConfigureButtonState(gameObject, "Normal", subCategory);
				this.ConfigureButtonState(gameObject, "Selected", subCategory);
				this.CreatePersistentSelectionBackground(gameObject, button);
				this.CreatePersistentContent(gameObject, button, subCategory);
				button.onClick.AddListener(delegate
				{
					this.HighlightButton(button);
					Action<CharacterSubCategorySO> action = this.onSubCategorySelected;
					if (action == null)
					{
						return;
					}
					action(subCategory);
				});
				this.buttons.Add(button);
				this.buttonData[button] = subCategory;
			}
		}

		// Token: 0x06003BBD RID: 15293 RVA: 0x001634F4 File Offset: 0x001616F4
		private void CreatePersistentContent(GameObject buttonObj, Button button, CharacterSubCategorySO subCategory)
		{
			Transform transform = buttonObj.transform.Find("PersistentContent");
			GameObject gameObject = ((transform != null) ? transform.gameObject : new GameObject("PersistentContent", new Type[] { typeof(RectTransform) }));
			if (transform == null)
			{
				gameObject.transform.SetParent(buttonObj.transform, false);
			}
			gameObject.layer = buttonObj.layer;
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.one;
			component.offsetMin = Vector2.zero;
			component.offsetMax = Vector2.zero;
			component.localScale = Vector3.one;
			Transform transform2 = gameObject.transform.Find("Icon");
			GameObject gameObject2 = ((transform2 != null) ? transform2.gameObject : new GameObject("Icon", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image)
			}));
			if (transform2 == null)
			{
				gameObject2.transform.SetParent(gameObject.transform, false);
			}
			gameObject2.layer = buttonObj.layer;
			RectTransform component2 = gameObject2.GetComponent<RectTransform>();
			component2.anchorMin = new Vector2(0.5f, 1f);
			component2.anchorMax = new Vector2(0.5f, 1f);
			component2.pivot = new Vector2(0.5f, 1f);
			component2.anchoredPosition = new Vector2(0f, -14f);
			component2.sizeDelta = new Vector2(34f, 34f);
			Image component3 = gameObject2.GetComponent<Image>();
			component3.sprite = ((subCategory != null) ? subCategory.icon : null);
			component3.color = Color.white;
			component3.preserveAspect = true;
			component3.raycastTarget = false;
			component3.enabled = component3.sprite != null;
			Transform transform3 = gameObject.transform.Find("Label");
			GameObject gameObject3 = ((transform3 != null) ? transform3.gameObject : new GameObject("Label", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Text)
			}));
			if (transform3 == null)
			{
				gameObject3.transform.SetParent(gameObject.transform, false);
			}
			gameObject3.layer = buttonObj.layer;
			RectTransform component4 = gameObject3.GetComponent<RectTransform>();
			component4.anchorMin = new Vector2(0f, 0f);
			component4.anchorMax = new Vector2(1f, 0f);
			component4.pivot = new Vector2(0.5f, 0f);
			component4.anchoredPosition = new Vector2(0f, 13f);
			component4.sizeDelta = new Vector2(-8f, 29f);
			Text component5 = gameObject3.GetComponent<Text>();
			component5.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			component5.fontSize = 18;
			component5.resizeTextForBestFit = true;
			component5.resizeTextMinSize = 11;
			component5.resizeTextMaxSize = 18;
			component5.alignment = TextAnchor.MiddleCenter;
			component5.color = Color.white;
			component5.raycastTarget = false;
			LocalizedLegacyGameText localizedLegacyGameText = LocalizedLegacyGameText.BindSource(component5, "cielos_apps", (subCategory != null) ? subCategory.subCategoryName : string.Empty);
			if (localizedLegacyGameText != null)
			{
				localizedLegacyGameText.FitTranslationToRectangle();
			}
			gameObject.SetActive(true);
			gameObject.transform.SetAsLastSibling();
			this.persistentContents[button] = gameObject;
			Transform transform4 = buttonObj.transform.Find("Normal");
			Transform transform5 = buttonObj.transform.Find("Selected");
			if (transform4 != null)
			{
				transform4.gameObject.SetActive(false);
			}
			if (transform5 != null)
			{
				transform5.gameObject.SetActive(false);
			}
		}

		// Token: 0x06003BBE RID: 15294 RVA: 0x001638C0 File Offset: 0x00161AC0
		private void CreatePersistentSelectionBackground(GameObject buttonObj, Button button)
		{
			Color color = new Color32(232, 88, 121, byte.MaxValue);
			Transform transform = buttonObj.transform.Find("Selected");
			if (transform != null)
			{
				Image image = transform.GetComponent<Image>();
				if (image == null)
				{
					image = transform.GetComponentInChildren<Image>(true);
				}
				if (image != null && image.color.a > 0.01f)
				{
					color = image.color;
				}
			}
			GameObject gameObject = new GameObject("PersistentSelectionBackground", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image)
			});
			gameObject.transform.SetParent(buttonObj.transform, false);
			gameObject.transform.SetAsFirstSibling();
			gameObject.layer = buttonObj.layer;
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = Vector2.zero;
			component.anchorMax = Vector2.one;
			component.offsetMin = Vector2.zero;
			component.offsetMax = Vector2.zero;
			Image component2 = gameObject.GetComponent<Image>();
			component2.color = new Color(color.r, color.g, color.b, 0f);
			component2.raycastTarget = false;
			this.selectionBackgrounds[button] = component2;
			this.selectionColors[button] = color;
			Transform transform2 = buttonObj.transform.Find("Normal");
			if (transform2 != null)
			{
				transform2.gameObject.SetActive(false);
			}
			if (transform != null)
			{
				transform.gameObject.SetActive(false);
			}
		}

		// Token: 0x06003BBF RID: 15295 RVA: 0x00163A58 File Offset: 0x00161C58
		private void ConfigureButtonState(GameObject buttonObj, string stateName, CharacterSubCategorySO subCategory)
		{
			Transform transform = buttonObj.transform.Find(stateName);
			if (transform == null)
			{
				return;
			}
			Text[] componentsInChildren = transform.GetComponentsInChildren<Text>(true);
			int i;
			for (i = 0; i < componentsInChildren.Length; i++)
			{
				LocalizedLegacyGameText localizedLegacyGameText = LocalizedLegacyGameText.BindSource(componentsInChildren[i], "cielos_apps", subCategory.subCategoryName);
				if (localizedLegacyGameText != null)
				{
					localizedLegacyGameText.FitTranslationToRectangle();
				}
			}
			Image[] componentsInChildren2 = transform.GetComponentsInChildren<Image>(true);
			i = 0;
			while (i < componentsInChildren2.Length)
			{
				Image image = componentsInChildren2[i];
				if (!(image.transform == transform))
				{
					if (subCategory.icon != null)
					{
						image.sprite = subCategory.icon;
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06003BC0 RID: 15296 RVA: 0x00163AF8 File Offset: 0x00161CF8
		public void SelectSubCategory(CharacterSubCategorySO subCategory)
		{
			int num = this.subCategories.IndexOf(subCategory);
			if (num >= 0 && num < this.buttons.Count)
			{
				Button button = this.buttons[num];
				this.HighlightButton(button);
				Action<CharacterSubCategorySO> action = this.onSubCategorySelected;
				if (action == null)
				{
					return;
				}
				action(subCategory);
			}
		}

		// Token: 0x06003BC1 RID: 15297 RVA: 0x00163B4C File Offset: 0x00161D4C
		private void HighlightButton(Button selectedButton)
		{
			this.controllerSelectedButton = selectedButton;
			foreach (Button button in this.buttons)
			{
				if (button != null)
				{
					this.SetButtonState(button, false);
				}
			}
			if (selectedButton != null)
			{
				this.SetButtonState(selectedButton, true);
			}
		}

		// Token: 0x06003BC2 RID: 15298 RVA: 0x00163BC4 File Offset: 0x00161DC4
		public Button ControllerStep(int direction)
		{
			if (direction == 0 || this.buttons.Count == 0)
			{
				return null;
			}
			int num = this.buttons.IndexOf(this.controllerSelectedButton);
			for (int i = 1; i <= this.buttons.Count; i++)
			{
				int num2 = (Mathf.Max(0, num) + ((direction > 0) ? i : (-i)) + this.buttons.Count) % this.buttons.Count;
				Button button = this.buttons[num2];
				if (!(button == null) && button.isActiveAndEnabled && button.IsInteractable())
				{
					button.onClick.Invoke();
					ControllerUINavigation.Reveal(button.gameObject);
					return button;
				}
			}
			return null;
		}

		// Token: 0x06003BC3 RID: 15299 RVA: 0x00163C74 File Offset: 0x00161E74
		private void SetButtonState(Button button, bool isSelected)
		{
			Transform transform = button.transform.Find("Normal");
			Transform transform2 = button.transform.Find("Selected");
			if (transform != null)
			{
				transform.gameObject.SetActive(false);
			}
			if (transform2 != null)
			{
				transform2.gameObject.SetActive(false);
			}
			Image image;
			Color color;
			if (this.selectionBackgrounds.TryGetValue(button, out image) && image != null && this.selectionColors.TryGetValue(button, out color))
			{
				image.color = (isSelected ? color : new Color(color.r, color.g, color.b, 0f));
			}
			button.gameObject.SetActive(true);
			CharacterSubCategorySO characterSubCategorySO;
			if (this.buttonData.TryGetValue(button, out characterSubCategorySO))
			{
				this.CreatePersistentContent(button.gameObject, button, characterSubCategorySO);
			}
		}

		// Token: 0x06003BC4 RID: 15300 RVA: 0x00163D4C File Offset: 0x00161F4C
		private void EnsureHorizontalScroll()
		{
			RectTransform rectTransform = base.transform as RectTransform;
			if (rectTransform != null)
			{
				RectTransform rectTransform2 = this.buttonContainer as RectTransform;
				if (rectTransform2 != null)
				{
					HorizontalLayoutGroup component = base.GetComponent<HorizontalLayoutGroup>();
					if (component != null && this.buttonContainer != base.transform)
					{
						component.enabled = false;
					}
					HorizontalLayoutGroup horizontalLayoutGroup = rectTransform2.GetComponent<HorizontalLayoutGroup>();
					if (horizontalLayoutGroup == null)
					{
						horizontalLayoutGroup = rectTransform2.gameObject.AddComponent<HorizontalLayoutGroup>();
					}
					horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
					horizontalLayoutGroup.childControlWidth = true;
					horizontalLayoutGroup.childControlHeight = true;
					horizontalLayoutGroup.childForceExpandWidth = false;
					horizontalLayoutGroup.childForceExpandHeight = true;
					horizontalLayoutGroup.spacing = 4f;
					int num = Mathf.CeilToInt(this.arrowWidth + 6f);
					horizontalLayoutGroup.padding = new RectOffset(num, num, 0, 0);
					ContentSizeFitter contentSizeFitter = rectTransform2.GetComponent<ContentSizeFitter>();
					if (contentSizeFitter == null)
					{
						contentSizeFitter = rectTransform2.gameObject.AddComponent<ContentSizeFitter>();
					}
					contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
					contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
					rectTransform2.anchorMin = new Vector2(0f, 0f);
					rectTransform2.anchorMax = new Vector2(0f, 1f);
					rectTransform2.pivot = new Vector2(0f, 0.5f);
					rectTransform2.anchoredPosition = Vector2.zero;
					rectTransform2.sizeDelta = new Vector2(rectTransform2.sizeDelta.x, 0f);
					this.horizontalScroll = base.GetComponent<ScrollRect>();
					if (this.horizontalScroll == null)
					{
						this.horizontalScroll = base.gameObject.AddComponent<ScrollRect>();
					}
					this.horizontalScroll.viewport = rectTransform;
					this.horizontalScroll.content = rectTransform2;
					this.horizontalScroll.horizontal = true;
					this.horizontalScroll.vertical = false;
					this.horizontalScroll.movementType = ScrollRect.MovementType.Elastic;
					this.horizontalScroll.elasticity = 0.08f;
					this.horizontalScroll.inertia = true;
					this.horizontalScroll.decelerationRate = 0.12f;
					this.horizontalScroll.scrollSensitivity = 0f;
					this.EnsureNavigationArrows(rectTransform);
					return;
				}
			}
		}

		// Token: 0x06003BC5 RID: 15301 RVA: 0x00163F50 File Offset: 0x00162150
		private void ConfigureContentSizing()
		{
			RectTransform rectTransform = base.transform as RectTransform;
			if (rectTransform != null)
			{
				RectTransform rectTransform2 = this.buttonContainer as RectTransform;
				if (rectTransform2 != null)
				{
					HorizontalLayoutGroup component = rectTransform2.GetComponent<HorizontalLayoutGroup>();
					ContentSizeFitter component2 = rectTransform2.GetComponent<ContentSizeFitter>();
					if (component == null)
					{
						return;
					}
					float width = rectTransform.rect.width;
					float num = (float)this.buttons.Count * this.fixedButtonWidth + (float)Mathf.Max(0, this.buttons.Count - 1) * component.spacing;
					this.tabsFitViewport = this.buttons.Count > 0 && width > 1f && num <= width;
					int num2 = (this.tabsFitViewport ? 0 : Mathf.CeilToInt(this.arrowWidth + 6f));
					component.padding = new RectOffset(num2, num2, 0, 0);
					for (int i = 0; i < this.buttons.Count; i++)
					{
						if (!(this.buttons[i] == null))
						{
							LayoutElement component3 = this.buttons[i].GetComponent<LayoutElement>();
							if (!(component3 == null))
							{
								component3.minWidth = (this.tabsFitViewport ? 0f : this.fixedButtonWidth);
								component3.preferredWidth = (this.tabsFitViewport ? 0f : this.fixedButtonWidth);
								component3.flexibleWidth = (this.tabsFitViewport ? 1f : 0f);
							}
						}
					}
					if (component2 != null)
					{
						component2.enabled = !this.tabsFitViewport;
					}
					if (this.tabsFitViewport)
					{
						rectTransform2.anchorMin = Vector2.zero;
						rectTransform2.anchorMax = Vector2.one;
						rectTransform2.pivot = new Vector2(0.5f, 0.5f);
						rectTransform2.offsetMin = Vector2.zero;
						rectTransform2.offsetMax = Vector2.zero;
						if (this.horizontalScroll != null)
						{
							this.horizontalScroll.horizontal = false;
							return;
						}
					}
					else
					{
						rectTransform2.anchorMin = new Vector2(0f, 0f);
						rectTransform2.anchorMax = new Vector2(0f, 1f);
						rectTransform2.pivot = new Vector2(0f, 0.5f);
						rectTransform2.anchoredPosition = Vector2.zero;
						rectTransform2.sizeDelta = new Vector2(rectTransform2.sizeDelta.x, 0f);
						if (this.horizontalScroll != null)
						{
							this.horizontalScroll.horizontal = true;
						}
					}
					return;
				}
			}
		}

		// Token: 0x06003BC6 RID: 15302 RVA: 0x001641D4 File Offset: 0x001623D4
		public void OnScroll(PointerEventData eventData)
		{
			if (this.draggingScroll || this.horizontalScroll == null || this.horizontalScroll.content == null)
			{
				return;
			}
			if (this.horizontalScroll.content.rect.width - ((RectTransform)this.horizontalScroll.transform).rect.width <= 0.01f)
			{
				return;
			}
			float num = Mathf.Sign(eventData.scrollDelta.y);
			if (Mathf.Abs(eventData.scrollDelta.y) > 0.01f)
			{
				this.ScrollByButtons(-num * this.wheelStepInButtons);
			}
			eventData.Use();
		}

		// Token: 0x06003BC7 RID: 15303 RVA: 0x00164286 File Offset: 0x00162486
		public void OnBeginDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.draggingScroll = true;
			this.CancelScrollSmoothing();
		}

		// Token: 0x06003BC8 RID: 15304 RVA: 0x0016429E File Offset: 0x0016249E
		public void OnEndDrag(PointerEventData eventData)
		{
			if (eventData.button != PointerEventData.InputButton.Left)
			{
				return;
			}
			this.draggingScroll = false;
			this.CancelScrollSmoothing();
		}

		// Token: 0x06003BC9 RID: 15305 RVA: 0x001642B6 File Offset: 0x001624B6
		private void OnDisable()
		{
			this.draggingScroll = false;
			this.CancelScrollSmoothing();
		}

		// Token: 0x06003BCA RID: 15306 RVA: 0x001642C5 File Offset: 0x001624C5
		private void CancelScrollSmoothing()
		{
			this.smoothingScroll = false;
			if (this.horizontalScroll != null)
			{
				this.targetNormalizedPosition = this.horizontalScroll.horizontalNormalizedPosition;
			}
		}

		// Token: 0x06003BCB RID: 15307 RVA: 0x001642ED File Offset: 0x001624ED
		public void SyncControllerScroll()
		{
			this.CancelScrollSmoothing();
		}

		// Token: 0x06003BCC RID: 15308 RVA: 0x001642F8 File Offset: 0x001624F8
		private void UpdateScrollPosition(float deltaTime)
		{
			if (this.horizontalScroll == null || this.horizontalScroll.content == null)
			{
				return;
			}
			float horizontalNormalizedPosition = this.horizontalScroll.horizontalNormalizedPosition;
			if (this.draggingScroll || !this.smoothingScroll)
			{
				this.targetNormalizedPosition = horizontalNormalizedPosition;
				return;
			}
			float num = 1f - Mathf.Exp(-this.scrollSmoothing * deltaTime);
			float num2 = Mathf.Lerp(horizontalNormalizedPosition, this.targetNormalizedPosition, num);
			if (Mathf.Abs(num2 - this.targetNormalizedPosition) <= 0.0001f)
			{
				num2 = this.targetNormalizedPosition;
				this.smoothingScroll = false;
			}
			this.horizontalScroll.horizontalNormalizedPosition = num2;
		}

		// Token: 0x06003BCD RID: 15309 RVA: 0x0016439C File Offset: 0x0016259C
		private void LateUpdate()
		{
			if (this.horizontalScroll != null)
			{
				this.UpdateScrollPosition(Time.unscaledDeltaTime);
				this.UpdateArrowState();
			}
			for (int i = 0; i < this.buttons.Count; i++)
			{
				Button button = this.buttons[i];
				CharacterSubCategorySO characterSubCategorySO;
				if (!(button == null) && this.buttonData.TryGetValue(button, out characterSubCategorySO))
				{
					button.gameObject.SetActive(true);
					button.transform.localScale = Vector3.one;
					Transform transform = button.transform.Find("Normal");
					Transform transform2 = button.transform.Find("Selected");
					if (transform != null && transform.gameObject.activeSelf)
					{
						transform.gameObject.SetActive(false);
					}
					if (transform2 != null && transform2.gameObject.activeSelf)
					{
						transform2.gameObject.SetActive(false);
					}
					GameObject gameObject;
					if (!this.persistentContents.TryGetValue(button, out gameObject) || gameObject == null)
					{
						this.CreatePersistentContent(button.gameObject, button, characterSubCategorySO);
					}
					else
					{
						gameObject.SetActive(true);
						gameObject.transform.localScale = Vector3.one;
					}
				}
			}
		}

		// Token: 0x06003BCE RID: 15310 RVA: 0x001644D9 File Offset: 0x001626D9
		private IEnumerator ResetScrollAfterLayout()
		{
			yield return null;
			RectTransform rectTransform = this.buttonContainer as RectTransform;
			if (rectTransform != null)
			{
				this.ConfigureContentSizing();
				LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
				Vector2 anchoredPosition = rectTransform.anchoredPosition;
				anchoredPosition.x = 0f;
				rectTransform.anchoredPosition = anchoredPosition;
			}
			this.targetNormalizedPosition = 0f;
			if (this.horizontalScroll != null)
			{
				this.horizontalScroll.horizontalNormalizedPosition = 0f;
				this.horizontalScroll.velocity = Vector2.zero;
			}
			this.UpdateArrowState();
			this.resetScrollRoutine = null;
			yield break;
		}

		// Token: 0x06003BCF RID: 15311 RVA: 0x001644E8 File Offset: 0x001626E8
		private void EnsureNavigationArrows(RectTransform viewport)
		{
			this.previousButton = this.EnsureArrowButton("PreviousSubcategory", "‹", viewport, TextAnchor.MiddleLeft, -1);
			this.nextButton = this.EnsureArrowButton("NextSubcategory", "›", viewport, TextAnchor.MiddleRight, 1);
		}

		// Token: 0x06003BD0 RID: 15312 RVA: 0x0016451C File Offset: 0x0016271C
		private Button EnsureArrowButton(string objectName, string glyph, RectTransform viewport, TextAnchor alignment, int direction)
		{
			Transform transform = base.transform.Find(objectName);
			GameObject gameObject = ((transform != null) ? transform.gameObject : new GameObject(objectName, new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image),
				typeof(Button)
			}));
			if (transform == null)
			{
				gameObject.transform.SetParent(base.transform, false);
			}
			RectTransform component = gameObject.GetComponent<RectTransform>();
			bool flag = direction < 0;
			component.anchorMin = new Vector2(flag ? 0f : 1f, 0f);
			component.anchorMax = new Vector2(flag ? 0f : 1f, 1f);
			component.pivot = new Vector2(flag ? 0f : 1f, 0.5f);
			component.anchoredPosition = Vector2.zero;
			component.sizeDelta = new Vector2(this.arrowWidth, 0f);
			Image component2 = gameObject.GetComponent<Image>();
			component2.color = new Color(0.105f, 0.055f, 0.16f, 0.92f);
			component2.raycastTarget = true;
			Button component3 = gameObject.GetComponent<Button>();
			component3.targetGraphic = component2;
			component3.onClick.RemoveAllListeners();
			component3.onClick.AddListener(delegate
			{
				this.ScrollByButtons((float)direction * 1.35f);
			});
			Transform transform2 = gameObject.transform.Find("Glyph");
			GameObject gameObject2 = ((transform2 != null) ? transform2.gameObject : new GameObject("Glyph", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Text)
			}));
			if (transform2 == null)
			{
				gameObject2.transform.SetParent(gameObject.transform, false);
			}
			RectTransform component4 = gameObject2.GetComponent<RectTransform>();
			component4.anchorMin = Vector2.zero;
			component4.anchorMax = Vector2.one;
			component4.offsetMin = Vector2.zero;
			component4.offsetMax = Vector2.zero;
			Text component5 = gameObject2.GetComponent<Text>();
			component5.text = glyph;
			component5.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
			component5.fontSize = 34;
			component5.fontStyle = FontStyle.Bold;
			component5.alignment = TextAnchor.MiddleCenter;
			component5.color = Color.white;
			component5.raycastTarget = false;
			gameObject.transform.SetAsLastSibling();
			return component3;
		}

		// Token: 0x06003BD1 RID: 15313 RVA: 0x001647AC File Offset: 0x001629AC
		private void ScrollByButtons(float buttonCount)
		{
			if (this.draggingScroll)
			{
				return;
			}
			float hiddenWidth = this.GetHiddenWidth();
			if (hiddenWidth <= 0.01f)
			{
				return;
			}
			if (!this.smoothingScroll)
			{
				this.targetNormalizedPosition = this.horizontalScroll.horizontalNormalizedPosition;
			}
			this.horizontalScroll.StopMovement();
			this.targetNormalizedPosition = Mathf.Clamp01(this.targetNormalizedPosition + buttonCount * this.fixedButtonWidth / hiddenWidth);
			this.smoothingScroll = true;
		}

		// Token: 0x06003BD2 RID: 15314 RVA: 0x0016481C File Offset: 0x00162A1C
		private float GetHiddenWidth()
		{
			if (this.horizontalScroll == null || this.horizontalScroll.content == null)
			{
				return 0f;
			}
			return this.horizontalScroll.content.rect.width - ((RectTransform)this.horizontalScroll.transform).rect.width;
		}

		// Token: 0x06003BD3 RID: 15315 RVA: 0x00164888 File Offset: 0x00162A88
		private void UpdateArrowState()
		{
			float hiddenWidth = this.GetHiddenWidth();
			bool flag = !this.tabsFitViewport && hiddenWidth > 0.01f;
			if (this.previousButton != null)
			{
				this.previousButton.gameObject.SetActive(flag);
				this.previousButton.interactable = flag && this.targetNormalizedPosition > 0.001f;
			}
			if (this.nextButton != null)
			{
				this.nextButton.gameObject.SetActive(flag);
				this.nextButton.interactable = flag && this.targetNormalizedPosition < 0.999f;
			}
		}

		// Token: 0x04003BF6 RID: 15350
		[SerializeField]
		private GameObject buttonPrefab;

		// Token: 0x04003BF7 RID: 15351
		[SerializeField]
		private Transform buttonContainer;

		// Token: 0x04003BF8 RID: 15352
		[SerializeField]
		[Min(72f)]
		private float fixedButtonWidth = 104f;

		// Token: 0x04003BF9 RID: 15353
		[SerializeField]
		[Min(0.05f)]
		private float wheelStepInButtons = 0.38f;

		// Token: 0x04003BFA RID: 15354
		[SerializeField]
		[Min(1f)]
		private float scrollSmoothing = 9f;

		// Token: 0x04003BFB RID: 15355
		[SerializeField]
		[Min(24f)]
		private float arrowWidth = 38f;

		// Token: 0x04003BFC RID: 15356
		private List<CharacterSubCategorySO> subCategories = new List<CharacterSubCategorySO>();

		// Token: 0x04003BFD RID: 15357
		private Action<CharacterSubCategorySO> onSubCategorySelected;

		// Token: 0x04003BFE RID: 15358
		private List<Button> buttons = new List<Button>();

		// Token: 0x04003BFF RID: 15359
		private Button controllerSelectedButton;

		// Token: 0x04003C00 RID: 15360
		private readonly Dictionary<Button, Image> selectionBackgrounds = new Dictionary<Button, Image>();

		// Token: 0x04003C01 RID: 15361
		private readonly Dictionary<Button, Color> selectionColors = new Dictionary<Button, Color>();

		// Token: 0x04003C02 RID: 15362
		private readonly Dictionary<Button, CharacterSubCategorySO> buttonData = new Dictionary<Button, CharacterSubCategorySO>();

		// Token: 0x04003C03 RID: 15363
		private readonly Dictionary<Button, GameObject> persistentContents = new Dictionary<Button, GameObject>();

		// Token: 0x04003C04 RID: 15364
		private ScrollRect horizontalScroll;

		// Token: 0x04003C05 RID: 15365
		private Button previousButton;

		// Token: 0x04003C06 RID: 15366
		private Button nextButton;

		// Token: 0x04003C07 RID: 15367
		private float targetNormalizedPosition;

		// Token: 0x04003C08 RID: 15368
		private bool smoothingScroll;

		// Token: 0x04003C09 RID: 15369
		private bool draggingScroll;

		// Token: 0x04003C0A RID: 15370
		private Coroutine resetScrollRoutine;

		// Token: 0x04003C0B RID: 15371
		private bool tabsFitViewport;
	}
}
