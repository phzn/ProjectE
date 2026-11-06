using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200022C RID: 556
	[EventCategory(EventCategory.EnterLeave)]
	public sealed class PointerOutEvent : PointerEventBase<PointerOutEvent>
	{
		// Token: 0x0600101D RID: 4125 RVA: 0x0003B31E File Offset: 0x0003951E
		static PointerOutEvent()
		{
			EventBase<PointerOutEvent>.SetCreateFunction(() => new PointerOutEvent());
		}
	}
}
