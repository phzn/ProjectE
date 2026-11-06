using System;

namespace System.Net.WebSockets
{
	/// <summary>Indicates the message type.</summary>
	// Token: 0x0200083D RID: 2109
	public enum WebSocketMessageType
	{
		/// <summary>The message is clear text.</summary>
		// Token: 0x040028B6 RID: 10422
		Text,
		/// <summary>The message is in binary format.</summary>
		// Token: 0x040028B7 RID: 10423
		Binary,
		/// <summary>A receive has completed because a close message was received.</summary>
		// Token: 0x040028B8 RID: 10424
		Close
	}
}
