using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200023D RID: 573
	public sealed class TransitionEndEvent : TransitionEventBase<TransitionEndEvent>
	{
		// Token: 0x0600105C RID: 4188 RVA: 0x0003B8FE File Offset: 0x00039AFE
		static TransitionEndEvent()
		{
			EventBase<TransitionEndEvent>.SetCreateFunction(() => new TransitionEndEvent());
		}
	}
}
