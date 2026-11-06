using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200022A RID: 554
	[EventCategory(EventCategory.EnterLeave)]
	public sealed class PointerOverEvent : PointerEventBase<PointerOverEvent>
	{
		// Token: 0x06001018 RID: 4120 RVA: 0x0003B2E9 File Offset: 0x000394E9
		static PointerOverEvent()
		{
			EventBase<PointerOverEvent>.SetCreateFunction(() => new PointerOverEvent());
		}
	}
}
