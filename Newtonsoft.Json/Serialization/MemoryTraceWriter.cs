using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000098 RID: 152
	[NullableContext(1)]
	[Nullable(0)]
	public class MemoryTraceWriter : ITraceWriter
	{
		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000800 RID: 2048 RVA: 0x00022BA0 File Offset: 0x00020DA0
		// (set) Token: 0x06000801 RID: 2049 RVA: 0x00022BA8 File Offset: 0x00020DA8
		public TraceLevel LevelFilter { get; set; }

		// Token: 0x06000802 RID: 2050 RVA: 0x00022BB1 File Offset: 0x00020DB1
		public MemoryTraceWriter()
		{
			this.LevelFilter = TraceLevel.Verbose;
			this._traceMessages = new Queue<string>();
			this._lock = new object();
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00022BD8 File Offset: 0x00020DD8
		public void Trace(TraceLevel level, string message, [Nullable(2)] Exception ex)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff", CultureInfo.InvariantCulture));
			stringBuilder.Append(" ");
			stringBuilder.Append(level.ToString("g"));
			stringBuilder.Append(" ");
			stringBuilder.Append(message);
			string text = stringBuilder.ToString();
			object @lock = this._lock;
			lock (@lock)
			{
				if (this._traceMessages.Count >= 1000)
				{
					this._traceMessages.Dequeue();
				}
				this._traceMessages.Enqueue(text);
			}
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00022C9C File Offset: 0x00020E9C
		public IEnumerable<string> GetTraceMessages()
		{
			return this._traceMessages;
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00022CA4 File Offset: 0x00020EA4
		public override string ToString()
		{
			object @lock = this._lock;
			string text2;
			lock (@lock)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string text in this._traceMessages)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(text);
				}
				text2 = stringBuilder.ToString();
			}
			return text2;
		}

		// Token: 0x040002D6 RID: 726
		private readonly Queue<string> _traceMessages;

		// Token: 0x040002D7 RID: 727
		private readonly object _lock;
	}
}
