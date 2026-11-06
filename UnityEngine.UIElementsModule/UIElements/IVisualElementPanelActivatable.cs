using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200040A RID: 1034
	internal interface IVisualElementPanelActivatable
	{
		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x0600210D RID: 8461
		VisualElement element { get; }

		// Token: 0x0600210E RID: 8462
		bool CanBeActivated();

		// Token: 0x0600210F RID: 8463
		void OnPanelActivate();

		// Token: 0x06002110 RID: 8464
		void OnPanelDeactivate();
	}
}
