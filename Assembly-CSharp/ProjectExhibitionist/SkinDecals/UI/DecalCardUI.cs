using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ProjectExhibitionist.SkinDecals.UI
{
	// Token: 0x02000941 RID: 2369
	public class DecalCardUI : MonoBehaviour
	{
		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x0600407E RID: 16510 RVA: 0x0017F129 File Offset: 0x0017D329
		public SkinDecal Decal
		{
			get
			{
				return this._decal;
			}
		}

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x0600407F RID: 16511 RVA: 0x0017F131 File Offset: 0x0017D331
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
		}

		// Token: 0x06004080 RID: 16512 RVA: 0x0017F13C File Offset: 0x0017D33C
		public void Setup(SkinDecal decal, bool isActive, Action<SkinDecal, bool> onToggled)
		{
			this._decal = decal;
			this._onToggled = onToggled;
			if (this.nameLabel != null)
			{
				this.nameLabel.text = decal.displayName;
			}
			if (this.previewImage != null)
			{
				this.previewImage.sprite = decal.previewSprite;
				if (this.previewImage.sprite == null)
				{
					Texture2D texture2D = ((decal.bodyTexture != null) ? decal.bodyTexture : decal.headTexture);
					if (texture2D != null)
					{
						this.previewImage.sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
					}
				}
				this.previewImage.preserveAspect = true;
			}
			if (this.toggleButton != null)
			{
				this.toggleButton.onClick.RemoveAllListeners();
				this.toggleButton.onClick.AddListener(new UnityAction(this.OnToggleClicked));
			}
			this.SetActiveState(isActive);
		}

		// Token: 0x06004081 RID: 16513 RVA: 0x0017F25C File Offset: 0x0017D45C
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
		}

		// Token: 0x06004082 RID: 16514 RVA: 0x0017F2D4 File Offset: 0x0017D4D4
		private void OnToggleClicked()
		{
			bool flag = !this._isActive;
			this.SetActiveState(flag);
			Action<SkinDecal, bool> onToggled = this._onToggled;
			if (onToggled == null)
			{
				return;
			}
			onToggled(this._decal, flag);
		}

		// Token: 0x04004050 RID: 16464
		[Header("UI References")]
		[SerializeField]
		private Image previewImage;

		// Token: 0x04004051 RID: 16465
		[SerializeField]
		private TextMeshProUGUI nameLabel;

		// Token: 0x04004052 RID: 16466
		[SerializeField]
		private GameObject activeIndicator;

		// Token: 0x04004053 RID: 16467
		[SerializeField]
		private GameObject inactiveIndicator;

		// Token: 0x04004054 RID: 16468
		[SerializeField]
		private Button toggleButton;

		// Token: 0x04004055 RID: 16469
		[SerializeField]
		private Image cardBackground;

		// Token: 0x04004056 RID: 16470
		[Header("Style")]
		[SerializeField]
		private Color activeBackgroundColor = new Color(0.91f, 0.36f, 0.49f, 0.35f);

		// Token: 0x04004057 RID: 16471
		[SerializeField]
		private Color inactiveBackgroundColor = new Color(0.17f, 0.1f, 0.25f, 0.6f);

		// Token: 0x04004058 RID: 16472
		private SkinDecal _decal;

		// Token: 0x04004059 RID: 16473
		private bool _isActive;

		// Token: 0x0400405A RID: 16474
		private Action<SkinDecal, bool> _onToggled;
	}
}
