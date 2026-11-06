using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200018D RID: 397
	public class PointerCaptureEvent : PointerCaptureEventBase<PointerCaptureEvent>
	{
		// Token: 0x06000C6F RID: 3183 RVA: 0x00031D7E File Offset: 0x0002FF7E
		static PointerCaptureEvent()
		{
			EventBase<PointerCaptureEvent>.SetCreateFunction(() => new PointerCaptureEvent());
		}
	}
}
