using System;
using Features.CielOS.UI;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CielOS.Panel
{
	// Token: 0x020013D7 RID: 5079
	[RequireComponent(typeof(Button))]
	public class CielOSAppMenuEntry : MonoBehaviour
	{
		// Token: 0x170018D7 RID: 6359
		// (get) Token: 0x06009CDE RID: 40158 RVA: 0x00406144 File Offset: 0x00404344
		public string AppId
		{
			get
			{
				return this._appId;
			}
		}

		// Token: 0x06009CDF RID: 40159 RVA: 0x0040614C File Offset: 0x0040434C
		private void Awake()
		{
			if (this.button == null)
			{
				this.button = base.GetComponent<Button>();
			}
		}

		// Token: 0x06009CE0 RID: 40160 RVA: 0x00406168 File Offset: 0x00404368
		public void Bind(string appId, string displayName, Sprite icon, Action<string> onClick, bool locked = false)
		{
			this._appId = appId;
			this._onClick = onClick;
			if (this.label != null)
			{
				LocalizedGameText.BindSource(this.label, "cielos_shell", displayName, null).FitTranslationToRectangle(false);
			}
			if (this.iconImage != null)
			{
				this.iconImage.enabled = true;
				if (icon != null)
				{
					this.iconImage.sprite = icon;
					this.iconImage.color = Color.white;
				}
				else
				{
					Color color = ((CielOSTheme.Active != null) ? CielOSTheme.Active.accent : new Color(0.66f, 0.38f, 0.79f, 1f));
					CielOSIcon.Apply(this.iconImage, CielOSAppIcons.ForApp(appId), color, 64);
				}
			}
			CielOSAppAccess.ApplyLock(this.iconImage, locked);
			if (this.button != null)
			{
				this.button.onClick.RemoveAllListeners();
				this.button.onClick.AddListener(delegate
				{
					Action<string> onClick2 = this._onClick;
					if (onClick2 == null)
					{
						return;
					}
					onClick2(this._appId);
				});
				CielOSSfx.Attach(this.button);
			}
		}

		// Token: 0x0400A16A RID: 41322
		[SerializeField]
		private Image iconImage;

		// Token: 0x0400A16B RID: 41323
		[SerializeField]
		private TMP_Text label;

		// Token: 0x0400A16C RID: 41324
		[SerializeField]
		private Button button;

		// Token: 0x0400A16D RID: 41325
		private string _appId;

		// Token: 0x0400A16E RID: 41326
		private Action<string> _onClick;
	}
}
