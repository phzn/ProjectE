using System;
using UnityEngine.UI;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x0200012D RID: 301
	public class DebugUIHandlerMessageBox : DebugUIHandlerWidget
	{
		// Token: 0x060008FD RID: 2301 RVA: 0x00029DC0 File Offset: 0x00027FC0
		internal override void SetWidget(DebugUI.Widget widget)
		{
			base.SetWidget(widget);
			this.m_Field = base.CastWidget<DebugUI.MessageBox>();
			this.nameLabel.text = this.m_Field.displayName;
			Image component = base.GetComponent<Image>();
			DebugUI.MessageBox.Style style = this.m_Field.style;
			if (style == DebugUI.MessageBox.Style.Warning)
			{
				component.color = DebugUIHandlerMessageBox.k_WarningBackgroundColor;
				return;
			}
			if (style != DebugUI.MessageBox.Style.Error)
			{
				return;
			}
			component.color = DebugUIHandlerMessageBox.k_ErrorBackgroundColor;
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x00029E33 File Offset: 0x00028033
		public override bool OnSelection(bool fromNext, DebugUIHandlerWidget previous)
		{
			return false;
		}

		// Token: 0x04000540 RID: 1344
		public Text nameLabel;

		// Token: 0x04000541 RID: 1345
		private DebugUI.MessageBox m_Field;

		// Token: 0x04000542 RID: 1346
		private static Color32 k_WarningBackgroundColor = new Color32(231, 180, 3, 30);

		// Token: 0x04000543 RID: 1347
		private static Color32 k_WarningTextColor = new Color32(231, 180, 3, byte.MaxValue);

		// Token: 0x04000544 RID: 1348
		private static Color32 k_ErrorBackgroundColor = new Color32(231, 75, 3, 30);

		// Token: 0x04000545 RID: 1349
		private static Color32 k_ErrorTextColor = new Color32(231, 75, 3, byte.MaxValue);
	}
}
