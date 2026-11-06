using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001EA RID: 490
	public class ContextClickEvent : MouseEventBase<ContextClickEvent>
	{
		// Token: 0x06000EC5 RID: 3781 RVA: 0x00038073 File Offset: 0x00036273
		static ContextClickEvent()
		{
			EventBase<ContextClickEvent>.SetCreateFunction(() => new ContextClickEvent());
		}
	}
}
