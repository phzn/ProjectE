using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001FF RID: 511
	public interface INavigationEvent
	{
		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000F13 RID: 3859
		EventModifiers modifiers { get; }

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000F14 RID: 3860
		NavigationDeviceType deviceType { get; }

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000F15 RID: 3861
		bool shiftKey { get; }

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000F16 RID: 3862
		bool ctrlKey { get; }

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000F17 RID: 3863
		bool commandKey { get; }

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000F18 RID: 3864
		bool altKey { get; }

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000F19 RID: 3865
		bool actionKey { get; }
	}
}
