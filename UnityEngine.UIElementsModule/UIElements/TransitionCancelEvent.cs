using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200023F RID: 575
	public sealed class TransitionCancelEvent : TransitionEventBase<TransitionCancelEvent>
	{
		// Token: 0x06001061 RID: 4193 RVA: 0x0003B933 File Offset: 0x00039B33
		static TransitionCancelEvent()
		{
			EventBase<TransitionCancelEvent>.SetCreateFunction(() => new TransitionCancelEvent());
		}
	}
}
