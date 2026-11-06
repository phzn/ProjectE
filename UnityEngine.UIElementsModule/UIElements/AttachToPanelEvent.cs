using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200020B RID: 523
	public class AttachToPanelEvent : PanelChangedEventBase<AttachToPanelEvent>
	{
		// Token: 0x06000F4B RID: 3915 RVA: 0x0003911C File Offset: 0x0003731C
		static AttachToPanelEvent()
		{
			EventBase<AttachToPanelEvent>.SetCreateFunction(() => new AttachToPanelEvent());
		}
	}
}
