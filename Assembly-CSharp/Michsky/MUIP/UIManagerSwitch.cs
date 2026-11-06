using System;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
	// Token: 0x02000264 RID: 612
	[ExecuteInEditMode]
	public class UIManagerSwitch : MonoBehaviour
	{
		// Token: 0x0600101C RID: 4124 RVA: 0x0006B168 File Offset: 0x00069368
		private void Awake()
		{
			if (this.UIManagerAsset == null)
			{
				this.UIManagerAsset = Resources.Load<UIManager>("MUIP Manager");
			}
			base.enabled = true;
			if (!this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateSwitch();
				base.enabled = false;
			}
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x0006B1B4 File Offset: 0x000693B4
		private void Update()
		{
			if (this.UIManagerAsset == null)
			{
				return;
			}
			if (this.UIManagerAsset.enableDynamicUpdate)
			{
				this.UpdateSwitch();
			}
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x0006B1D8 File Offset: 0x000693D8
		private void UpdateSwitch()
		{
			if (!this.overrideColors)
			{
				this.border.color = new Color(this.UIManagerAsset.switchBorderColor.r, this.UIManagerAsset.switchBorderColor.g, this.UIManagerAsset.switchBorderColor.b, this.border.color.a);
				this.background.color = new Color(this.UIManagerAsset.switchBackgroundColor.r, this.UIManagerAsset.switchBackgroundColor.g, this.UIManagerAsset.switchBackgroundColor.b, this.background.color.a);
				this.handleOn.color = new Color(this.UIManagerAsset.switchHandleOnColor.r, this.UIManagerAsset.switchHandleOnColor.g, this.UIManagerAsset.switchHandleOnColor.b, this.handleOn.color.a);
				this.handleOff.color = new Color(this.UIManagerAsset.switchHandleOffColor.r, this.UIManagerAsset.switchHandleOffColor.g, this.UIManagerAsset.switchHandleOffColor.b, this.handleOff.color.a);
			}
		}

		// Token: 0x040011AC RID: 4524
		[Header("Settings")]
		[SerializeField]
		private UIManager UIManagerAsset;

		// Token: 0x040011AD RID: 4525
		[HideInInspector]
		public bool overrideColors;

		// Token: 0x040011AE RID: 4526
		[Header("Resources")]
		[SerializeField]
		private Image border;

		// Token: 0x040011AF RID: 4527
		[SerializeField]
		private Image background;

		// Token: 0x040011B0 RID: 4528
		[SerializeField]
		private Image handleOn;

		// Token: 0x040011B1 RID: 4529
		[SerializeField]
		private Image handleOff;
	}
}
