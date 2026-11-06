using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200018B RID: 395
	public class PointerCaptureOutEvent : PointerCaptureEventBase<PointerCaptureOutEvent>
	{
		// Token: 0x06000C6A RID: 3178 RVA: 0x00031D49 File Offset: 0x0002FF49
		static PointerCaptureOutEvent()
		{
			EventBase<PointerCaptureOutEvent>.SetCreateFunction(() => new PointerCaptureOutEvent());
		}
	}
}
