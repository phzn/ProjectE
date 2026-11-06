using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000253 RID: 595
	internal interface IRuntimePanel
	{
		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06001117 RID: 4375
		PanelSettings panelSettings { get; }

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06001118 RID: 4376
		// (set) Token: 0x06001119 RID: 4377
		GameObject selectableGameObject { get; set; }
	}
}
