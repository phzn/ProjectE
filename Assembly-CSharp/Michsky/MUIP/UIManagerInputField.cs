using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x0200025C RID: 604
	[ExecuteInEditMode]
	public class UIManagerInputField : MonoBehaviour
	{
		// Token: 0x06000FFF RID: 4095 RVA: 0x0006A70C File Offset: 0x0006890C
		private void Awake()
		{
			if (this.UIManagerAsset == null)
			{
				this.UIManagerAsset = Resources.Load<UIManager>("MUIP Manager");
			}
			base.enabled = true;
			if (!this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateInputField();
				base.enabled = false;
			}
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x0006A758 File Offset: 0x00068958
		private void Update()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateInputField();
			}
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x0006A77C File Offset: 0x0006897C
		private void UpdateInputField()
		{
			if (!this.overrideColors)
			{
				this.mainText.color = new Color(this.UIManagerAsset.inputFieldColor.r, this.UIManagerAsset.inputFieldColor.g, this.UIManagerAsset.inputFieldColor.b, this.mainText.color.a);
				this.placeholderText.color = new Color(this.UIManagerAsset.inputFieldColor.r, this.UIManagerAsset.inputFieldColor.g, this.UIManagerAsset.inputFieldColor.b, this.placeholderText.color.a);
				this.filledImage.color = new Color(this.UIManagerAsset.inputFieldColor.r, this.UIManagerAsset.inputFieldColor.g, this.UIManagerAsset.inputFieldColor.b, this.filledImage.color.a);
				this.backgroundImage.color = new Color(this.UIManagerAsset.inputFieldColor.r, this.UIManagerAsset.inputFieldColor.g, this.UIManagerAsset.inputFieldColor.b, this.backgroundImage.color.a);
			}
			if (!this.overrideFonts)
			{
				this.mainText.font = this.UIManagerAsset.inputFieldFont;
				this.placeholderText.font = this.UIManagerAsset.inputFieldFont;
			}
		}

		// Token: 0x0400117E RID: 4478
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x0400117F RID: 4479
		public bool overrideColors;

		// Token: 0x04001180 RID: 4480
		public bool overrideFonts;

		// Token: 0x04001181 RID: 4481
		[Header("Resources")]
		[SerializeField]
		private TextMeshProUGUI mainText;

		// Token: 0x04001182 RID: 4482
		[SerializeField]
		private TextMeshProUGUI placeholderText;

		// Token: 0x04001183 RID: 4483
		[SerializeField]
		private Image filledImage;

		// Token: 0x04001184 RID: 4484
		[SerializeField]
		private Image backgroundImage;
	}
}
