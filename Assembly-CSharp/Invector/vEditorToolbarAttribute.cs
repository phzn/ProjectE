using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x02000468 RID: 1128
	public class vEditorToolbarAttribute : PropertyAttribute
	{
		// Token: 0x06001BEF RID: 7151 RVA: 0x000AF76A File Offset: 0x000AD96A
		public vEditorToolbarAttribute(string title, bool useIcon = false, string iconName = "", bool overrideIcon = false, bool overrideChildOrder = false)
		{
			this.title = title;
			this.icon = iconName;
			this.useIcon = useIcon;
			this.overrideChildOrder = overrideChildOrder;
			this.overrideIcon = overrideIcon;
		}

		// Token: 0x04001D4F RID: 7503
		public readonly string title;

		// Token: 0x04001D50 RID: 7504
		public readonly string icon;

		// Token: 0x04001D51 RID: 7505
		public readonly bool useIcon;

		// Token: 0x04001D52 RID: 7506
		public readonly bool overrideChildOrder;

		// Token: 0x04001D53 RID: 7507
		public readonly bool overrideIcon;
	}
}
