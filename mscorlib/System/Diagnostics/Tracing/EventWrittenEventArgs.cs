using System;
using System.Collections.ObjectModel;
using Unity;

namespace System.Diagnostics.Tracing
{
	/// <summary>Provides data for the <see cref="M:System.Diagnostics.Tracing.EventListener.OnEventWritten(System.Diagnostics.Tracing.EventWrittenEventArgs)" /> callback.</summary>
	// Token: 0x020009FC RID: 2556
	public class EventWrittenEventArgs : EventArgs
	{
		// Token: 0x06005B1F RID: 23327 RVA: 0x00134832 File Offset: 0x00132A32
		internal EventWrittenEventArgs(EventSource eventSource)
		{
			this.EventSource = eventSource;
		}

		/// <summary>Gets the activity ID on the thread that the event was written to.</summary>
		/// <returns>The activity ID on the thread that the event was written to.</returns>
		// Token: 0x17000F9A RID: 3994
		// (get) Token: 0x06005B20 RID: 23328 RVA: 0x00134841 File Offset: 0x00132A41
		public Guid ActivityId
		{
			get
			{
				return EventSource.CurrentThreadActivityId;
			}
		}

		/// <summary>Gets the channel for the event.</summary>
		/// <returns>The channel for the event.</returns>
		// Token: 0x17000F9B RID: 3995
		// (get) Token: 0x06005B21 RID: 23329 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public EventChannel Channel
		{
			get
			{
				return EventChannel.None;
			}
		}

		/// <summary>Gets the event identifier.</summary>
		/// <returns>The event identifier.</returns>
		// Token: 0x17000F9C RID: 3996
		// (get) Token: 0x06005B22 RID: 23330 RVA: 0x00134848 File Offset: 0x00132A48
		// (set) Token: 0x06005B23 RID: 23331 RVA: 0x00134850 File Offset: 0x00132A50
		public int EventId { get; internal set; }

		// Token: 0x17000F9D RID: 3997
		// (get) Token: 0x06005B24 RID: 23332 RVA: 0x00134859 File Offset: 0x00132A59
		// (set) Token: 0x06005B25 RID: 23333 RVA: 0x00134861 File Offset: 0x00132A61
		public long OSThreadId { get; internal set; }

		// Token: 0x17000F9E RID: 3998
		// (get) Token: 0x06005B26 RID: 23334 RVA: 0x0013486A File Offset: 0x00132A6A
		// (set) Token: 0x06005B27 RID: 23335 RVA: 0x00134872 File Offset: 0x00132A72
		public DateTime TimeStamp { get; internal set; }

		/// <summary>Gets the name of the event.</summary>
		/// <returns>The name of the event.</returns>
		// Token: 0x17000F9F RID: 3999
		// (get) Token: 0x06005B28 RID: 23336 RVA: 0x0013487B File Offset: 0x00132A7B
		// (set) Token: 0x06005B29 RID: 23337 RVA: 0x00134883 File Offset: 0x00132A83
		public string EventName { get; internal set; }

		/// <summary>Gets the event source object.</summary>
		/// <returns>The event source object.</returns>
		// Token: 0x17000FA0 RID: 4000
		// (get) Token: 0x06005B2A RID: 23338 RVA: 0x0013488C File Offset: 0x00132A8C
		// (set) Token: 0x06005B2B RID: 23339 RVA: 0x00134894 File Offset: 0x00132A94
		public EventSource EventSource { get; private set; }

		/// <summary>Gets the keywords for the event.</summary>
		/// <returns>The keywords for the event.</returns>
		// Token: 0x17000FA1 RID: 4001
		// (get) Token: 0x06005B2C RID: 23340 RVA: 0x0005CD52 File Offset: 0x0005AF52
		public EventKeywords Keywords
		{
			get
			{
				return EventKeywords.None;
			}
		}

		/// <summary>Gets the level of the event.</summary>
		/// <returns>The level of the event.</returns>
		// Token: 0x17000FA2 RID: 4002
		// (get) Token: 0x06005B2D RID: 23341 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public EventLevel Level
		{
			get
			{
				return EventLevel.LogAlways;
			}
		}

		/// <summary>Gets the message for the event.</summary>
		/// <returns>The message for the event.</returns>
		// Token: 0x17000FA3 RID: 4003
		// (get) Token: 0x06005B2E RID: 23342 RVA: 0x0013489D File Offset: 0x00132A9D
		// (set) Token: 0x06005B2F RID: 23343 RVA: 0x001348A5 File Offset: 0x00132AA5
		public string Message { get; internal set; }

		/// <summary>Gets the operation code for the event.</summary>
		/// <returns>The operation code for the event.</returns>
		// Token: 0x17000FA4 RID: 4004
		// (get) Token: 0x06005B30 RID: 23344 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public EventOpcode Opcode
		{
			get
			{
				return EventOpcode.Info;
			}
		}

		/// <summary>Gets the payload for the event.</summary>
		/// <returns>The payload for the event.</returns>
		// Token: 0x17000FA5 RID: 4005
		// (get) Token: 0x06005B31 RID: 23345 RVA: 0x001348AE File Offset: 0x00132AAE
		// (set) Token: 0x06005B32 RID: 23346 RVA: 0x001348B6 File Offset: 0x00132AB6
		public ReadOnlyCollection<object> Payload { get; internal set; }

		/// <summary>Returns a list of strings that represent the property names of the event.</summary>
		/// <returns>Returns <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" />.</returns>
		// Token: 0x17000FA6 RID: 4006
		// (get) Token: 0x06005B33 RID: 23347 RVA: 0x001348BF File Offset: 0x00132ABF
		// (set) Token: 0x06005B34 RID: 23348 RVA: 0x001348C7 File Offset: 0x00132AC7
		public ReadOnlyCollection<string> PayloadNames { get; internal set; }

		/// <summary>Gets the identifier of an activity that is related to the activity represented by the current instance.</summary>
		/// <returns>The identifier of the related activity, or <see cref="F:System.Guid.Empty" /> if there is no related activity.</returns>
		// Token: 0x17000FA7 RID: 4007
		// (get) Token: 0x06005B35 RID: 23349 RVA: 0x001348D0 File Offset: 0x00132AD0
		// (set) Token: 0x06005B36 RID: 23350 RVA: 0x001348D8 File Offset: 0x00132AD8
		public Guid RelatedActivityId { get; internal set; }

		/// <summary>Returns the tags specified in the call to the <see cref="M:System.Diagnostics.Tracing.EventSource.Write(System.String,System.Diagnostics.Tracing.EventSourceOptions)" /> method.</summary>
		/// <returns>Returns <see cref="T:System.Diagnostics.Tracing.EventTags" />.</returns>
		// Token: 0x17000FA8 RID: 4008
		// (get) Token: 0x06005B37 RID: 23351 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public EventTags Tags
		{
			get
			{
				return EventTags.None;
			}
		}

		/// <summary>Gets the task for the event.</summary>
		/// <returns>The task for the event.</returns>
		// Token: 0x17000FA9 RID: 4009
		// (get) Token: 0x06005B38 RID: 23352 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public EventTask Task
		{
			get
			{
				return EventTask.None;
			}
		}

		/// <summary>Gets the version of the event.</summary>
		/// <returns>The version of the event.</returns>
		// Token: 0x17000FAA RID: 4010
		// (get) Token: 0x06005B39 RID: 23353 RVA: 0x0000DDA9 File Offset: 0x0000BFA9
		public byte Version
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06005B3A RID: 23354 RVA: 0x000173AD File Offset: 0x000155AD
		internal EventWrittenEventArgs()
		{
			ThrowStub.ThrowNotSupportedException();
		}
	}
}
