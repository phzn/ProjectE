using System;
using Features.CensorBars;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BoZo.CharacterCreator.MC.StyleSystem.UI
{
	// Token: 0x02000811 RID: 2065
	public class MCStyleCardUI : MonoBehaviour
	{
		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x0600370A RID: 14090 RVA: 0x0013DA88 File Offset: 0x0013BC88
		public int PresetIndex
		{
			get
			{
				return this.presetIndex;
			}
		}

		// Token: 0x0600370B RID: 14091 RVA: 0x0013DA90 File Offset: 0x0013BC90
		private void Awake()
		{
			if (this.icon != null)
			{
				RectTransform rectTransform = this.icon.rectTransform;
				this._authoredIconAnchorMin = rectTransform.anchorMin;
				this._authoredIconAnchorMax = rectTransform.anchorMax;
				this._authoredIconSizeDelta = rectTransform.sizeDelta;
				this._authoredIconAnchoredPosition = rectTransform.anchoredPosition;
			}
			this._button = base.GetComponent<Button>();
			if (this._button != null)
			{
				this._button.onClick.AddListener(new UnityAction(this.HandleClick));
			}
		}

		// Token: 0x0600370C RID: 14092 RVA: 0x0013DB1D File Offset: 0x0013BD1D
		private void OnDestroy()
		{
			if (this._button != null)
			{
				this._button.onClick.RemoveListener(new UnityAction(this.HandleClick));
			}
		}

		// Token: 0x0600370D RID: 14093 RVA: 0x0013DB4C File Offset: 0x0013BD4C
		public void Configure(int index, string displayName, Sprite previewIcon, bool sensitiveVisual = false)
		{
			this.presetIndex = index;
			if (this.label != null)
			{
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this.label, "cielos_apps", displayName, null);
				if (localizedGameText != null)
				{
					localizedGameText.FitTranslationToRectangle(false);
				}
			}
			if (this.icon != null)
			{
				StreamerCensorImage.Bind(this.icon, sensitiveVisual);
				this.icon.preserveAspect = true;
				this.ConfigureIconBounds(displayName);
				if (previewIcon != null)
				{
					this.icon.sprite = previewIcon;
					this.icon.enabled = true;
				}
				else
				{
					this.icon.enabled = false;
				}
			}
			this.SetSelected(false);
		}

		// Token: 0x0600370E RID: 14094 RVA: 0x0013DBF0 File Offset: 0x0013BDF0
		private void ConfigureIconBounds(string displayName)
		{
			RectTransform rectTransform = this.icon.rectTransform;
			if (!string.IsNullOrWhiteSpace(displayName) && displayName.IndexOf("Eyebrow", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
				rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
				rectTransform.pivot = new Vector2(0.5f, 0.5f);
				rectTransform.anchoredPosition = new Vector2(0f, 12f);
				rectTransform.sizeDelta = new Vector2(92f, 48f);
				return;
			}
			rectTransform.anchorMin = this._authoredIconAnchorMin;
			rectTransform.anchorMax = this._authoredIconAnchorMax;
			rectTransform.anchoredPosition = this._authoredIconAnchoredPosition;
			rectTransform.sizeDelta = this._authoredIconSizeDelta;
		}

		// Token: 0x0600370F RID: 14095 RVA: 0x0013DCC2 File Offset: 0x0013BEC2
		public void SetSelected(bool selected)
		{
			if (this.selectedBorder != null)
			{
				this.selectedBorder.SetActive(selected);
			}
		}

		// Token: 0x06003710 RID: 14096 RVA: 0x0013DCDE File Offset: 0x0013BEDE
		private void HandleClick()
		{
			UnityEvent<int> onCardClicked = this.OnCardClicked;
			if (onCardClicked == null)
			{
				return;
			}
			onCardClicked.Invoke(this.presetIndex);
		}

		// Token: 0x04003731 RID: 14129
		[Header("Visuals")]
		[SerializeField]
		private Image icon;

		// Token: 0x04003732 RID: 14130
		[SerializeField]
		private TMP_Text label;

		// Token: 0x04003733 RID: 14131
		[SerializeField]
		private GameObject selectedBorder;

		// Token: 0x04003734 RID: 14132
		[Header("State")]
		[SerializeField]
		private int presetIndex = -1;

		// Token: 0x04003735 RID: 14133
		public UnityEvent<int> OnCardClicked = new UnityEvent<int>();

		// Token: 0x04003736 RID: 14134
		private Button _button;

		// Token: 0x04003737 RID: 14135
		private Vector2 _authoredIconAnchorMin;

		// Token: 0x04003738 RID: 14136
		private Vector2 _authoredIconAnchorMax;

		// Token: 0x04003739 RID: 14137
		private Vector2 _authoredIconSizeDelta;

		// Token: 0x0400373A RID: 14138
		private Vector2 _authoredIconAnchoredPosition;
	}
}
