using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001C4 RID: 452
	public interface IFocusEvent
	{
		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000DEB RID: 3563
		Focusable relatedTarget { get; }

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000DEC RID: 3564
		FocusChangeDirection direction { get; }
	}
}
