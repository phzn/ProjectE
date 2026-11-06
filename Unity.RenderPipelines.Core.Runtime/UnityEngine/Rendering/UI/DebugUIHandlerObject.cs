using System;
using UnityEngine.UI;

namespace UnityEngine.Rendering.UI
{
	// Token: 0x0200012E RID: 302
	public class DebugUIHandlerObject : DebugUIHandlerWidget
	{
		// Token: 0x06000901 RID: 2305 RVA: 0x00029EAC File Offset: 0x000280AC
		internal override void SetWidget(DebugUI.Widget widget)
		{
			base.SetWidget(widget);
			DebugUI.ObjectField objectField = base.CastWidget<DebugUI.ObjectField>();
			this.nameLabel.text = objectField.displayName;
			this.valueLabel.text = objectField.GetValue().name;
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x00029EEE File Offset: 0x000280EE
		public override bool OnSelection(bool fromNext, DebugUIHandlerWidget previous)
		{
			this.nameLabel.color = this.colorSelected;
			this.valueLabel.color = this.colorSelected;
			return true;
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x00029F13 File Offset: 0x00028113
		public override void OnDeselection()
		{
			this.nameLabel.color = this.colorDefault;
			this.valueLabel.color = this.colorDefault;
		}

		// Token: 0x04000546 RID: 1350
		public Text nameLabel;

		// Token: 0x04000547 RID: 1351
		public Text valueLabel;
	}
}
