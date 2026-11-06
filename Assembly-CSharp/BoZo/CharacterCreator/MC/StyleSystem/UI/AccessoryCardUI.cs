using System;
using Features.CensorBars;
using Features.Localization;
using ProjectExhibitionist.CharacterCustomization.Core.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BoZo.CharacterCreator.MC.StyleSystem.UI
{
	// Token: 0x0200080A RID: 2058
	public class AccessoryCardUI : MonoBehaviour
	{
		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x060036D4 RID: 14036 RVA: 0x0013C224 File Offset: 0x0013A424
		public string PartId
		{
			get
			{
				return this._partId;
			}
		}

		// Token: 0x060036D5 RID: 14037 RVA: 0x0013C22C File Offset: 0x0013A42C
		public void Setup(string partId, string displayName, bool isActive, int styleCount, Action<string, bool> onToggled, Action<string, int> onStyleCycled)
		{
			this._partId = partId;
			this._onToggled = onToggled;
			this._onStyleCycled = onStyleCycled;
			if (this.nameLabel != null)
			{
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this.nameLabel, "cielos_apps", displayName, null);
				if (localizedGameText != null)
				{
					localizedGameText.FitTranslationToRectangle(false);
				}
			}
			if (this.toggleButton != null)
			{
				this.toggleButton.onClick.RemoveAllListeners();
				this.toggleButton.onClick.AddListener(new UnityAction(this.OnToggleClicked));
			}
			bool flag = styleCount > 1;
			if (this.styleSelectorRoot != null)
			{
				this.styleSelectorRoot.SetActive(flag);
			}
			if (this.nextStyleButton != null)
			{
				this.nextStyleButton.onClick.RemoveAllListeners();
				this.nextStyleButton.onClick.AddListener(delegate
				{
					Action<string, int> onStyleCycled2 = this._onStyleCycled;
					if (onStyleCycled2 == null)
					{
						return;
					}
					onStyleCycled2(this._partId, 1);
				});
				this.nextStyleButton.gameObject.SetActive(flag);
			}
			if (this.prevStyleButton != null)
			{
				this.prevStyleButton.onClick.RemoveAllListeners();
				this.prevStyleButton.onClick.AddListener(delegate
				{
					Action<string, int> onStyleCycled3 = this._onStyleCycled;
					if (onStyleCycled3 == null)
					{
						return;
					}
					onStyleCycled3(this._partId, -1);
				});
				this.prevStyleButton.gameObject.SetActive(flag);
			}
			this.SetActiveState(isActive);
		}

		// Token: 0x060036D6 RID: 14038 RVA: 0x0013C373 File Offset: 0x0013A573
		public void SetStyleLabel(string text)
		{
			if (this.styleLabel != null)
			{
				CustomizationPresentation.BindColorLabel(this.styleLabel, text, false);
			}
		}

		// Token: 0x060036D7 RID: 14039 RVA: 0x0013C390 File Offset: 0x0013A590
		public void SetPreview(Sprite sprite, bool sensitiveVisual = false)
		{
			if (this.previewImage == null)
			{
				return;
			}
			this.previewImage.sprite = sprite;
			StreamerCensorImage.Bind(this.previewImage, sensitiveVisual);
			this.previewImage.preserveAspect = true;
			this.previewImage.enabled = sprite != null;
		}

		// Token: 0x060036D8 RID: 14040 RVA: 0x0013C3E4 File Offset: 0x0013A5E4
		public void SetActiveState(bool isActive)
		{
			this._isActive = isActive;
			if (this.activeIndicator != null)
			{
				this.activeIndicator.SetActive(isActive);
			}
			if (this.inactiveIndicator != null)
			{
				this.inactiveIndicator.SetActive(!isActive);
			}
			if (this.cardBackground != null)
			{
				this.cardBackground.color = (isActive ? this.activeBackgroundColor : this.inactiveBackgroundColor);
			}
			if (this.styleSelectorRoot != null && this.styleSelectorRoot.activeSelf)
			{
				if (this.nextStyleButton != null)
				{
					this.nextStyleButton.interactable = isActive;
				}
				if (this.prevStyleButton != null)
				{
					this.prevStyleButton.interactable = isActive;
				}
			}
		}

		// Token: 0x060036D9 RID: 14041 RVA: 0x0013C4A8 File Offset: 0x0013A6A8
		private void OnToggleClicked()
		{
			bool flag = !this._isActive;
			this.SetActiveState(flag);
			Action<string, bool> onToggled = this._onToggled;
			if (onToggled == null)
			{
				return;
			}
			onToggled(this._partId, flag);
		}

		// Token: 0x04003704 RID: 14084
		[Header("UI References")]
		[SerializeField]
		private TextMeshProUGUI nameLabel;

		// Token: 0x04003705 RID: 14085
		[SerializeField]
		private Image previewImage;

		// Token: 0x04003706 RID: 14086
		[SerializeField]
		private GameObject activeIndicator;

		// Token: 0x04003707 RID: 14087
		[SerializeField]
		private GameObject inactiveIndicator;

		// Token: 0x04003708 RID: 14088
		[SerializeField]
		private Button toggleButton;

		// Token: 0x04003709 RID: 14089
		[SerializeField]
		private Image cardBackground;

		// Token: 0x0400370A RID: 14090
		[Header("Selezione stile (opzionale)")]
		[Tooltip("Bottoni per ciclare lo stile dell'accessorio (mostrati solo se ha >1 stile).")]
		[SerializeField]
		private Button nextStyleButton;

		// Token: 0x0400370B RID: 14091
		[SerializeField]
		private Button prevStyleButton;

		// Token: 0x0400370C RID: 14092
		[SerializeField]
		private TextMeshProUGUI styleLabel;

		// Token: 0x0400370D RID: 14093
		[SerializeField]
		private GameObject styleSelectorRoot;

		// Token: 0x0400370E RID: 14094
		[Header("Style")]
		[SerializeField]
		private Color activeBackgroundColor = new Color(0.91f, 0.36f, 0.49f, 0.35f);

		// Token: 0x0400370F RID: 14095
		[SerializeField]
		private Color inactiveBackgroundColor = new Color(0.17f, 0.1f, 0.25f, 0.6f);

		// Token: 0x04003710 RID: 14096
		private string _partId;

		// Token: 0x04003711 RID: 14097
		private bool _isActive;

		// Token: 0x04003712 RID: 14098
		private Action<string, bool> _onToggled;

		// Token: 0x04003713 RID: 14099
		private Action<string, int> _onStyleCycled;
	}
}
