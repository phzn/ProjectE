using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001A8 RID: 424
	// (Invoke) Token: 0x06000D11 RID: 3345
	public delegate void EventCallback<in TEventType, in TCallbackArgs>(TEventType evt, TCallbackArgs userArgs);
}
