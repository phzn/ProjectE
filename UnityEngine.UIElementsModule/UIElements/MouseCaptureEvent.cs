using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000193 RID: 403
	public class MouseCaptureEvent : MouseCaptureEventBase<MouseCaptureEvent>
	{
		// Token: 0x06000C7D RID: 3197 RVA: 0x00031E20 File Offset: 0x00030020
		static MouseCaptureEvent()
		{
			EventBase<MouseCaptureEvent>.SetCreateFunction(() => new MouseCaptureEvent());
		}
	}
}
