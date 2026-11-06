using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001DA RID: 474
	public class KeyUpEvent : KeyboardEventBase<KeyUpEvent>
	{
		// Token: 0x06000E4E RID: 3662 RVA: 0x000370D8 File Offset: 0x000352D8
		static KeyUpEvent()
		{
			EventBase<KeyUpEvent>.SetCreateFunction(() => new KeyUpEvent());
		}
	}
}
