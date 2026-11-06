using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001F8 RID: 504
	[EventCategory(EventCategory.EnterLeave)]
	public class MouseOutEvent : MouseEventBase<MouseOutEvent>
	{
		// Token: 0x06000EFA RID: 3834 RVA: 0x000383FE File Offset: 0x000365FE
		static MouseOutEvent()
		{
			EventBase<MouseOutEvent>.SetCreateFunction(() => new MouseOutEvent());
		}
	}
}
