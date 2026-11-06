using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000207 RID: 519
	public class NavigationSubmitEvent : NavigationEventBase<NavigationSubmitEvent>
	{
		// Token: 0x06000F3E RID: 3902 RVA: 0x0003905C File Offset: 0x0003725C
		static NavigationSubmitEvent()
		{
			EventBase<NavigationSubmitEvent>.SetCreateFunction(() => new NavigationSubmitEvent());
		}
	}
}
