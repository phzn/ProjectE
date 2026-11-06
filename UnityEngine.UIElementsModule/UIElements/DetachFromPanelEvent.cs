using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200020D RID: 525
	public class DetachFromPanelEvent : PanelChangedEventBase<DetachFromPanelEvent>
	{
		// Token: 0x06000F50 RID: 3920 RVA: 0x00039151 File Offset: 0x00037351
		static DetachFromPanelEvent()
		{
			EventBase<DetachFromPanelEvent>.SetCreateFunction(() => new DetachFromPanelEvent());
		}
	}
}
