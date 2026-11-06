using System;

namespace MoreMountains.Tools
{
	// Token: 0x020000DE RID: 222
	public interface MMEventListener<T> : MMEventListenerBase
	{
		// Token: 0x060004C4 RID: 1220
		void OnMMEvent(T eventType);
	}
}
