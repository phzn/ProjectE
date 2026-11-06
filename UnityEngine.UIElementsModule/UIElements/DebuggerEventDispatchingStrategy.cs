using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200019F RID: 415
	internal class DebuggerEventDispatchingStrategy : IEventDispatchingStrategy
	{
		// Token: 0x06000CA3 RID: 3235 RVA: 0x00032188 File Offset: 0x00030388
		public bool CanDispatchEvent(EventBase evt)
		{
			return false;
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public void DispatchEvent(EventBase evt, IPanel panel)
		{
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public void PostDispatch(EventBase evt, IPanel panel)
		{
		}
	}
}
