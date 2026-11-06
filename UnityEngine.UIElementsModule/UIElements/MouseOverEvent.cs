using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001F6 RID: 502
	[EventCategory(EventCategory.EnterLeave)]
	public class MouseOverEvent : MouseEventBase<MouseOverEvent>
	{
		// Token: 0x06000EF5 RID: 3829 RVA: 0x000383C9 File Offset: 0x000365C9
		static MouseOverEvent()
		{
			EventBase<MouseOverEvent>.SetCreateFunction(() => new MouseOverEvent());
		}
	}
}
