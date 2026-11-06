using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001C1 RID: 449
	internal class EventDebuggerPathTrace : EventDebuggerTrace
	{
		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x00035DB6 File Offset: 0x00033FB6
		public PropagationPaths paths { get; }

		// Token: 0x06000DD1 RID: 3537 RVA: 0x00035DBE File Offset: 0x00033FBE
		public EventDebuggerPathTrace(IPanel panel, EventBase evt, PropagationPaths paths)
			: base(panel, evt, -1L, null)
		{
			this.paths = paths;
		}
	}
}
