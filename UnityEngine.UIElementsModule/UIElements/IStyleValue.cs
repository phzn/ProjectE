using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200030D RID: 781
	internal interface IStyleValue<T>
	{
		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x06001B0B RID: 6923
		// (set) Token: 0x06001B0C RID: 6924
		T value { get; set; }

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06001B0D RID: 6925
		// (set) Token: 0x06001B0E RID: 6926
		StyleKeyword keyword { get; set; }
	}
}
