using System;
using Features.Localization;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Bozo.AnimeCharacters
{
	// Token: 0x02000836 RID: 2102
	public class BodyShapeSliders : MonoBehaviour
	{
		// Token: 0x060037AB RID: 14251 RVA: 0x001455B5 File Offset: 0x001437B5
		private void Start()
		{
			if (this.system != null && this.bodyShape != null)
			{
				this.Init(this.system, this.bodyShape);
			}
		}

		// Token: 0x060037AC RID: 14252 RVA: 0x001455E8 File Offset: 0x001437E8
		public void Init(OutfitSystem system, BodyShapeModifier mod)
		{
			this.system = system;
			this.bodyShape = mod;
			LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this.title, "cielos_apps", mod.shapeName, null);
			if (localizedGameText != null)
			{
				localizedGameText.FitTranslationToRectangle(false);
			}
			this.EnsureResetButton();
			Slider[] componentsInChildren = base.GetComponentsInChildren<Slider>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				ControllerSliderHint.BindReset(componentsInChildren[i], this.resetButton);
			}
			foreach (TMP_Text tmp_Text in base.GetComponentsInChildren<TMP_Text>(true))
			{
				if (!(tmp_Text == this.title) && (tmp_Text.text == "Angle" || tmp_Text.text == "Size" || tmp_Text.text == "Position"))
				{
					LocalizedGameText localizedGameText2 = LocalizedGameText.BindSource(tmp_Text, "cielos_apps", tmp_Text.text, null);
					if (localizedGameText2 != null)
					{
						localizedGameText2.FitTranslationToRectangle(false);
					}
				}
			}
			if (this.bodyShape.useScale)
			{
				if (this.bodyShape.linkScaleAxis)
				{
					this.xScaleSlider.transform.parent.gameObject.SetActive(false);
					this.yScaleSlider.transform.parent.gameObject.SetActive(false);
					this.zScaleSlider.transform.parent.gameObject.SetActive(false);
				}
				else
				{
					if (!this.bodyShape.useXScale)
					{
						this.xScaleSlider.transform.parent.gameObject.SetActive(false);
					}
					if (!this.bodyShape.useYScale)
					{
						this.yScaleSlider.transform.parent.gameObject.SetActive(false);
					}
					if (!this.bodyShape.useZScale)
					{
						this.zScaleSlider.transform.parent.gameObject.SetActive(false);
					}
					this.scaleSlider.transform.parent.gameObject.SetActive(false);
					this.scaleSlider.onValueChanged.AddListener(new UnityAction<float>(this.SetScale));
				}
				this.scaleSlider.onValueChanged.AddListener(new UnityAction<float>(this.SetScale));
				this.xScaleSlider.onValueChanged.AddListener(new UnityAction<float>(this.SetScale));
				this.yScaleSlider.onValueChanged.AddListener(new UnityAction<float>(this.SetScale));
				this.zScaleSlider.onValueChanged.AddListener(new UnityAction<float>(this.SetScale));
				this.scaleSlider.minValue = this.bodyShape.scaleRange.x;
				this.scaleSlider.maxValue = this.bodyShape.scaleRange.y;
				this.xScaleSlider.minValue = this.bodyShape.scaleRange.x;
				this.xScaleSlider.maxValue = this.bodyShape.scaleRange.y;
				this.yScaleSlider.minValue = this.bodyShape.scaleRange.x;
				this.yScaleSlider.maxValue = this.bodyShape.scaleRange.y;
				this.zScaleSlider.minValue = this.bodyShape.scaleRange.x;
				this.zScaleSlider.maxValue = this.bodyShape.scaleRange.y;
				this.scaleSlider.SetValueWithoutNotify(this.bodyShape.scaleValue);
				this.xScaleSlider.SetValueWithoutNotify(this.bodyShape.xScaleValue);
				this.yScaleSlider.SetValueWithoutNotify(this.bodyShape.yScaleValue);
				this.zScaleSlider.SetValueWithoutNotify(this.bodyShape.zScaleValue);
			}
			else
			{
				this.scaleContainer.gameObject.SetActive(false);
			}
			if (this.bodyShape.usePosition)
			{
				if (!this.bodyShape.useXPos)
				{
					this.xPositionSlider.transform.parent.gameObject.SetActive(false);
				}
				if (!this.bodyShape.useYPos)
				{
					this.yPositionSlider.transform.parent.gameObject.SetActive(false);
				}
				if (!this.bodyShape.useZPos)
				{
					this.zPositionSlider.transform.parent.gameObject.SetActive(false);
				}
				this.xPositionSlider.minValue = this.bodyShape.posRange.x;
				this.xPositionSlider.maxValue = this.bodyShape.posRange.y;
				this.yPositionSlider.minValue = this.bodyShape.posRange.x;
				this.yPositionSlider.maxValue = this.bodyShape.posRange.y;
				this.zPositionSlider.minValue = this.bodyShape.posRange.x;
				this.zPositionSlider.maxValue = this.bodyShape.posRange.y;
				this.xPositionSlider.onValueChanged.AddListener(new UnityAction<float>(this.SetPosition));
				this.yPositionSlider.onValueChanged.AddListener(new UnityAction<float>(this.SetPosition));
				this.zPositionSlider.onValueChanged.AddListener(new UnityAction<float>(this.SetPosition));
				this.xPositionSlider.SetValueWithoutNotify(this.bodyShape.xPosValue);
				this.yPositionSlider.SetValueWithoutNotify(this.bodyShape.yPosValue);
				this.zPositionSlider.SetValueWithoutNotify(this.bodyShape.zPosValue);
			}
			else
			{
				this.positionContainer.gameObject.SetActive(false);
			}
			if (this.bodyShape.useRotation)
			{
				this.rotationSlider.onValueChanged.AddListener(new UnityAction<float>(this.SetRotation));
				this.rotationSlider.minValue = this.bodyShape.rotRange.x;
				this.rotationSlider.maxValue = this.bodyShape.rotRange.y;
				this.rotationSlider.SetValueWithoutNotify(this.bodyShape.rotation);
			}
			else
			{
				this.rotationContainer.gameObject.SetActive(false);
			}
			this.RefreshResetButton();
		}

		// Token: 0x060037AD RID: 14253 RVA: 0x00145C02 File Offset: 0x00143E02
		public void SetScale(float v)
		{
			this.bodyShape.SetScale(this.xScaleSlider.value, this.yScaleSlider.value, this.zScaleSlider.value, this.scaleSlider.value);
			this.RefreshResetButton();
		}

		// Token: 0x060037AE RID: 14254 RVA: 0x00145C41 File Offset: 0x00143E41
		public void SetPosition(float v)
		{
			this.bodyShape.SetPosition(this.xPositionSlider.value, this.yPositionSlider.value, this.zPositionSlider.value);
			this.RefreshResetButton();
		}

		// Token: 0x060037AF RID: 14255 RVA: 0x00145C75 File Offset: 0x00143E75
		public void SetRotation(float v)
		{
			this.bodyShape.SetRotation(this.rotationSlider.value);
			this.RefreshResetButton();
		}

		// Token: 0x060037B0 RID: 14256 RVA: 0x00145C94 File Offset: 0x00143E94
		private void EnsureResetButton()
		{
			if (this.resetButton != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("Reset Modifier", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image),
				typeof(Button),
				typeof(LayoutElement)
			});
			gameObject.layer = base.gameObject.layer;
			Transform transform = ((this.title != null) ? this.title.transform : base.transform);
			gameObject.transform.SetParent(transform, false);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = new Vector2(1f, 0.5f);
			component.anchorMax = new Vector2(1f, 0.5f);
			component.pivot = new Vector2(1f, 0.5f);
			component.anchoredPosition = new Vector2(-12f, 0f);
			component.sizeDelta = new Vector2(38f, 38f);
			gameObject.GetComponent<LayoutElement>().ignoreLayout = true;
			Image component2 = gameObject.GetComponent<Image>();
			component2.sprite = this.resetIcon;
			component2.color = new Color32(237, 91, 126, byte.MaxValue);
			component2.preserveAspect = true;
			this.resetButton = gameObject.GetComponent<Button>();
			ColorBlock colors = this.resetButton.colors;
			colors.normalColor = Color.white;
			colors.highlightedColor = new Color(1f, 1f, 1f, 0.86f);
			colors.pressedColor = new Color(0.82f, 0.82f, 0.82f, 1f);
			colors.selectedColor = Color.white;
			colors.fadeDuration = 0.08f;
			this.resetButton.colors = colors;
			this.resetButton.onClick.AddListener(new UnityAction(this.ResetModifier));
		}

		// Token: 0x060037B1 RID: 14257 RVA: 0x00145E98 File Offset: 0x00144098
		private void ResetModifier()
		{
			if (this.bodyShape == null)
			{
				return;
			}
			this.bodyShape.ResetToDefault();
			this.scaleSlider.SetValueWithoutNotify(this.bodyShape.scaleValue);
			this.xScaleSlider.SetValueWithoutNotify(this.bodyShape.xScaleValue);
			this.yScaleSlider.SetValueWithoutNotify(this.bodyShape.yScaleValue);
			this.zScaleSlider.SetValueWithoutNotify(this.bodyShape.zScaleValue);
			this.xPositionSlider.SetValueWithoutNotify(this.bodyShape.xPosValue);
			this.yPositionSlider.SetValueWithoutNotify(this.bodyShape.yPosValue);
			this.zPositionSlider.SetValueWithoutNotify(this.bodyShape.zPosValue);
			this.rotationSlider.SetValueWithoutNotify(this.bodyShape.rotation);
			this.RefreshResetButton();
		}

		// Token: 0x060037B2 RID: 14258 RVA: 0x00145F75 File Offset: 0x00144175
		private void RefreshResetButton()
		{
			if (this.resetButton != null && this.bodyShape != null)
			{
				this.resetButton.gameObject.SetActive(!this.bodyShape.IsAtDefault);
			}
		}

		// Token: 0x0400385F RID: 14431
		private OutfitSystem system;

		// Token: 0x04003860 RID: 14432
		private BodyShapeModifier bodyShape;

		// Token: 0x04003861 RID: 14433
		[SerializeField]
		private TMP_Text title;

		// Token: 0x04003862 RID: 14434
		[SerializeField]
		private Transform scaleContainer;

		// Token: 0x04003863 RID: 14435
		[SerializeField]
		private Slider scaleSlider;

		// Token: 0x04003864 RID: 14436
		[SerializeField]
		private Slider xScaleSlider;

		// Token: 0x04003865 RID: 14437
		[SerializeField]
		private Slider yScaleSlider;

		// Token: 0x04003866 RID: 14438
		[SerializeField]
		private Slider zScaleSlider;

		// Token: 0x04003867 RID: 14439
		[SerializeField]
		private Transform positionContainer;

		// Token: 0x04003868 RID: 14440
		[SerializeField]
		private Slider xPositionSlider;

		// Token: 0x04003869 RID: 14441
		[SerializeField]
		private Slider yPositionSlider;

		// Token: 0x0400386A RID: 14442
		[SerializeField]
		private Slider zPositionSlider;

		// Token: 0x0400386B RID: 14443
		[SerializeField]
		private Transform rotationContainer;

		// Token: 0x0400386C RID: 14444
		[SerializeField]
		private Slider rotationSlider;

		// Token: 0x0400386D RID: 14445
		[SerializeField]
		private Sprite resetIcon;

		// Token: 0x0400386E RID: 14446
		private Button resetButton;
	}
}
