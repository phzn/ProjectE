using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001CF RID: 463
	internal interface IEventDispatchingStrategy
	{
		// Token: 0x06000E13 RID: 3603
		bool CanDispatchEvent(EventBase evt);

		// Token: 0x06000E14 RID: 3604
		void DispatchEvent(EventBase evt, IPanel panel);
	}
}
