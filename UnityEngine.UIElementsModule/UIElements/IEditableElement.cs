using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020000CC RID: 204
	internal interface IEditableElement
	{
		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060006CB RID: 1739
		// (set) Token: 0x060006CC RID: 1740
		Action editingStarted { get; set; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060006CD RID: 1741
		// (set) Token: 0x060006CE RID: 1742
		Action editingEnded { get; set; }
	}
}
