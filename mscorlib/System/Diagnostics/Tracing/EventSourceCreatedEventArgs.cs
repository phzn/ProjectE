using System;

namespace System.Diagnostics.Tracing
{
	/// <summary>Provides data for the <see cref="E:System.Diagnostics.Tracing.EventListener.EventSourceCreated" /> event.</summary>
	// Token: 0x020009FA RID: 2554
	public class EventSourceCreatedEventArgs : EventArgs
	{
		/// <summary>Get the event source that is attaching to the listener.</summary>
		/// <returns>The event source that is attaching to the listener.</returns>
		// Token: 0x17000F99 RID: 3993
		// (get) Token: 0x06005B1C RID: 23324 RVA: 0x00134821 File Offset: 0x00132A21
		// (set) Token: 0x06005B1D RID: 23325 RVA: 0x00134829 File Offset: 0x00132A29
		public EventSource EventSource { get; internal set; }
	}
}
