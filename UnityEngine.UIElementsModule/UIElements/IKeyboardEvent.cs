using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001D5 RID: 469
	public interface IKeyboardEvent
	{
		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000E2B RID: 3627
		EventModifiers modifiers { get; }

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000E2C RID: 3628
		char character { get; }

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000E2D RID: 3629
		KeyCode keyCode { get; }

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000E2E RID: 3630
		bool shiftKey { get; }

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000E2F RID: 3631
		bool ctrlKey { get; }

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000E30 RID: 3632
		bool commandKey { get; }

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000E31 RID: 3633
		bool altKey { get; }

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000E32 RID: 3634
		bool actionKey { get; }
	}
}
