using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001BF RID: 447
	internal class EventDebuggerCallTrace : EventDebuggerTrace
	{
		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000DC7 RID: 3527 RVA: 0x00035D0F File Offset: 0x00033F0F
		public int callbackHashCode { get; }

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000DC8 RID: 3528 RVA: 0x00035D17 File Offset: 0x00033F17
		public string callbackName { get; }

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x00035D1F File Offset: 0x00033F1F
		public bool propagationHasStopped { get; }

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000DCA RID: 3530 RVA: 0x00035D27 File Offset: 0x00033F27
		public bool immediatePropagationHasStopped { get; }

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000DCB RID: 3531 RVA: 0x00035D2F File Offset: 0x00033F2F
		public bool defaultHasBeenPrevented { get; }

		// Token: 0x06000DCC RID: 3532 RVA: 0x00035D37 File Offset: 0x00033F37
		public EventDebuggerCallTrace(IPanel panel, EventBase evt, int cbHashCode, string cbName, bool propagationHasStopped, bool immediatePropagationHasStopped, bool defaultHasBeenPrevented, long duration, IEventHandler mouseCapture)
			: base(panel, evt, duration, mouseCapture)
		{
			this.callbackHashCode = cbHashCode;
			this.callbackName = cbName;
			this.propagationHasStopped = propagationHasStopped;
			this.immediatePropagationHasStopped = immediatePropagationHasStopped;
			this.defaultHasBeenPrevented = defaultHasBeenPrevented;
		}
	}
}
