using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001BE RID: 446
	internal class EventDebuggerTrace
	{
		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000DC1 RID: 3521 RVA: 0x00035C96 File Offset: 0x00033E96
		public EventDebuggerEventRecord eventBase { get; }

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000DC2 RID: 3522 RVA: 0x00035C9E File Offset: 0x00033E9E
		public IEventHandler focusedElement { get; }

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000DC3 RID: 3523 RVA: 0x00035CA6 File Offset: 0x00033EA6
		public IEventHandler mouseCapture { get; }

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000DC4 RID: 3524 RVA: 0x00035CAE File Offset: 0x00033EAE
		// (set) Token: 0x06000DC5 RID: 3525 RVA: 0x00035CB6 File Offset: 0x00033EB6
		public long duration { get; set; }

		// Token: 0x06000DC6 RID: 3526 RVA: 0x00035CC0 File Offset: 0x00033EC0
		public EventDebuggerTrace(IPanel panel, EventBase evt, long duration, IEventHandler mouseCapture)
		{
			this.eventBase = new EventDebuggerEventRecord(evt);
			IEventHandler eventHandler;
			if (panel == null)
			{
				eventHandler = null;
			}
			else
			{
				FocusController focusController = panel.focusController;
				eventHandler = ((focusController != null) ? focusController.focusedElement : null);
			}
			this.focusedElement = eventHandler;
			this.mouseCapture = mouseCapture;
			this.duration = duration;
		}
	}
}
