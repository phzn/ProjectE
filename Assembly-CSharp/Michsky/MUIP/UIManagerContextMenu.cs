using System;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000254 RID: 596
	[ExecuteInEditMode]
	public class UIManagerContextMenu : MonoBehaviour
	{
		// Token: 0x06000FEB RID: 4075 RVA: 0x00069E34 File Offset: 0x00068034
		private void Awake()
		{
			if (this.UIManagerAsset == null)
			{
				this.UIManagerAsset = Resources.Load<UIManager>("MUIP Manager");
			}
			base.enabled = true;
			if (!this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateContextMenu();
				base.enabled = false;
			}
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x00069E80 File Offset: 0x00068080
		private void Update()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateContextMenu();
			}
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x00069EA4 File Offset: 0x000680A4
		private void UpdateContextMenu()
		{
			if (this.backgroundImage != null)
			{
				this.backgroundImage.color = this.UIManagerAsset.contextBackgroundColor;
			}
		}

		// Token: 0x04001155 RID: 4437
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x04001156 RID: 4438
		[Header("Resources")]
		[SerializeField]
		private Image backgroundImage;
	}
}
