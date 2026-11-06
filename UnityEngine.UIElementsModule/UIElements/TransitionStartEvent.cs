using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200023B RID: 571
	public sealed class TransitionStartEvent : TransitionEventBase<TransitionStartEvent>
	{
		// Token: 0x06001057 RID: 4183 RVA: 0x0003B8C9 File Offset: 0x00039AC9
		static TransitionStartEvent()
		{
			EventBase<TransitionStartEvent>.SetCreateFunction(() => new TransitionStartEvent());
		}
	}
}
