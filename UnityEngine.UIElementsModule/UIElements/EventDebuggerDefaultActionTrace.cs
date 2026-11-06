using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001C0 RID: 448
	internal class EventDebuggerDefaultActionTrace : EventDebuggerTrace
	{
		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000DCD RID: 3533 RVA: 0x00035D6E File Offset: 0x00033F6E
		public PropagationPhase phase { get; }

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000DCE RID: 3534 RVA: 0x00035D78 File Offset: 0x00033F78
		public string targetName
		{
			get
			{
				return base.eventBase.target.GetType().FullName;
			}
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x00035D9F File Offset: 0x00033F9F
		public EventDebuggerDefaultActionTrace(IPanel panel, EventBase evt, PropagationPhase phase, long duration, IEventHandler mouseCapture)
			: base(panel, evt, duration, mouseCapture)
		{
			this.phase = phase;
		}
	}
}
