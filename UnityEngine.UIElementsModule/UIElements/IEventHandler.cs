using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001C2 RID: 450
	public interface IEventHandler
	{
		// Token: 0x06000DD2 RID: 3538
		void SendEvent(EventBase e);

		// Token: 0x06000DD3 RID: 3539
		void HandleEvent(EventBase evt);

		// Token: 0x06000DD4 RID: 3540
		bool HasTrickleDownHandlers();

		// Token: 0x06000DD5 RID: 3541
		bool HasBubbleUpHandlers();
	}
}
