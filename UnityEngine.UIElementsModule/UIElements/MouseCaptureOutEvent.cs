using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000191 RID: 401
	public class MouseCaptureOutEvent : MouseCaptureEventBase<MouseCaptureOutEvent>
	{
		// Token: 0x06000C78 RID: 3192 RVA: 0x00031DEB File Offset: 0x0002FFEB
		static MouseCaptureOutEvent()
		{
			EventBase<MouseCaptureOutEvent>.SetCreateFunction(() => new MouseCaptureOutEvent());
		}
	}
}
