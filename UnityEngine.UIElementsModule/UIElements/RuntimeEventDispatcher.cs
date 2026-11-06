using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000252 RID: 594
	internal static class RuntimeEventDispatcher
	{
		// Token: 0x06001116 RID: 4374 RVA: 0x0003DD48 File Offset: 0x0003BF48
		public static EventDispatcher Create()
		{
			return EventDispatcher.CreateForRuntime(new List<IEventDispatchingStrategy>
			{
				new PointerCaptureDispatchingStrategy(),
				new MouseCaptureDispatchingStrategy(),
				new KeyboardEventDispatchingStrategy(),
				new PointerEventDispatchingStrategy(),
				new MouseEventDispatchingStrategy(),
				new NavigationEventDispatchingStrategy(),
				new DefaultDispatchingStrategy()
			});
		}
	}
}
