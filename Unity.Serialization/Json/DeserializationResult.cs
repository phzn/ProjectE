using System;
using System.Collections.Generic;
using System.Linq;

namespace Unity.Serialization.Json
{
	// Token: 0x0200002B RID: 43
	public readonly struct DeserializationResult
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600009F RID: 159 RVA: 0x0000346C File Offset: 0x0000166C
		public IEnumerable<DeserializationEvent> Events
		{
			get
			{
				IEnumerable<DeserializationEvent> events = this.m_Events;
				return events ?? Enumerable.Empty<DeserializationEvent>();
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x0000348A File Offset: 0x0000168A
		public IEnumerable<DeserializationEvent> Logs
		{
			get
			{
				return this.Events.Where<DeserializationEvent>((DeserializationEvent e) => e.Type == EventType.Log);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x000034B6 File Offset: 0x000016B6
		public IEnumerable<DeserializationEvent> Errors
		{
			get
			{
				return this.Events.Where<DeserializationEvent>((DeserializationEvent e) => e.Type == EventType.Error);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x000034E2 File Offset: 0x000016E2
		public IEnumerable<DeserializationEvent> Warnings
		{
			get
			{
				return this.Events.Where<DeserializationEvent>((DeserializationEvent e) => e.Type == EventType.Warning);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x0000350E File Offset: 0x0000170E
		public IEnumerable<DeserializationEvent> Exceptions
		{
			get
			{
				return this.Events.Where<DeserializationEvent>((DeserializationEvent e) => e.Type == EventType.Exception);
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000353A File Offset: 0x0000173A
		internal DeserializationResult(List<DeserializationEvent> events)
		{
			this.m_Events = events;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003544 File Offset: 0x00001744
		public void Throw()
		{
			IEnumerable<Exception> enumerable = from e in this.Events
				where e.Payload is Exception
				select (Exception)e.Payload;
			if (enumerable.Count<Exception>() == 1)
			{
				throw enumerable.First<Exception>();
			}
			throw new AggregateException(enumerable);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000035B6 File Offset: 0x000017B6
		public bool DidSucceed()
		{
			return !this.Events.Any<DeserializationEvent>((DeserializationEvent e) => e.Type == EventType.Exception || e.Type == EventType.Error);
		}

		// Token: 0x0400003E RID: 62
		private readonly List<DeserializationEvent> m_Events;
	}
}
