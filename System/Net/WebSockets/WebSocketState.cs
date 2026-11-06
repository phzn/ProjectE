using System;

namespace System.Net.WebSockets
{
	/// <summary>Defines the different states a WebSockets instance can be in.</summary>
	// Token: 0x0200083F RID: 2111
	public enum WebSocketState
	{
		/// <summary>Reserved for future use.</summary>
		// Token: 0x040028BF RID: 10431
		None,
		/// <summary>The connection is negotiating the handshake with the remote endpoint.</summary>
		// Token: 0x040028C0 RID: 10432
		Connecting,
		/// <summary>The initial state after the HTTP handshake has been completed.</summary>
		// Token: 0x040028C1 RID: 10433
		Open,
		/// <summary>A close message was sent to the remote endpoint.</summary>
		// Token: 0x040028C2 RID: 10434
		CloseSent,
		/// <summary>A close message was received from the remote endpoint.</summary>
		// Token: 0x040028C3 RID: 10435
		CloseReceived,
		/// <summary>Indicates the WebSocket close handshake completed gracefully.</summary>
		// Token: 0x040028C4 RID: 10436
		Closed,
		/// <summary>Reserved for future use.</summary>
		// Token: 0x040028C5 RID: 10437
		Aborted
	}
}
