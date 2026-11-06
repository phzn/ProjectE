using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000205 RID: 517
	public class NavigationCancelEvent : NavigationEventBase<NavigationCancelEvent>
	{
		// Token: 0x06000F39 RID: 3897 RVA: 0x00039027 File Offset: 0x00037227
		static NavigationCancelEvent()
		{
			EventBase<NavigationCancelEvent>.SetCreateFunction(() => new NavigationCancelEvent());
		}
	}
}
