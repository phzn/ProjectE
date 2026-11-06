using System;

namespace Unity.Serialization.Json
{
	// Token: 0x0200002A RID: 42
	public readonly struct DeserializationEvent
	{
		// Token: 0x0600009D RID: 157 RVA: 0x0000344F File Offset: 0x0000164F
		internal DeserializationEvent(EventType type, object payload)
		{
			this.Type = type;
			this.Payload = payload;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0000345F File Offset: 0x0000165F
		public override string ToString()
		{
			return this.Payload.ToString();
		}

		// Token: 0x0400003C RID: 60
		public readonly EventType Type;

		// Token: 0x0400003D RID: 61
		public readonly object Payload;
	}
}
