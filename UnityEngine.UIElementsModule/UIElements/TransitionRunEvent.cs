using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000239 RID: 569
	public sealed class TransitionRunEvent : TransitionEventBase<TransitionRunEvent>
	{
		// Token: 0x06001052 RID: 4178 RVA: 0x0003B894 File Offset: 0x00039A94
		static TransitionRunEvent()
		{
			EventBase<TransitionRunEvent>.SetCreateFunction(() => new TransitionRunEvent());
		}
	}
}
