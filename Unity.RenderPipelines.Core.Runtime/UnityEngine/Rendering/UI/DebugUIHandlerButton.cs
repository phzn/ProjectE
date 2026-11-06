using System;
using UnityEngine.UI;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x0200011E RID: 286
	public class DebugUIHandlerButton : DebugUIHandlerWidget
	{
		// Token: 0x0600088B RID: 2187 RVA: 0x000281A0 File Offset: 0x000263A0
		internal override void SetWidget(DebugUI.Widget widget)
		{
			base.SetWidget(widget);
			this.m_Field = base.CastWidget<DebugUI.Button>();
			this.nameLabel.text = this.m_Field.displayName;
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x000281CB File Offset: 0x000263CB
		public override bool OnSelection(bool fromNext, DebugUIHandlerWidget previous)
		{
			this.nameLabel.color = this.colorSelected;
			return true;
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x000281DF File Offset: 0x000263DF
		public override void OnDeselection()
		{
			this.nameLabel.color = this.colorDefault;
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x000281F2 File Offset: 0x000263F2
		public override void OnAction()
		{
			if (this.m_Field.action != null)
			{
				this.m_Field.action();
			}
		}

		// Token: 0x04000505 RID: 1285
		public Text nameLabel;

		// Token: 0x04000506 RID: 1286
		private DebugUI.Button m_Field;
	}
}
