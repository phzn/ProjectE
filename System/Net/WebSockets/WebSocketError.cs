using System;

namespace System.Net.WebSockets
{
	/// <summary>Contains the list of possible WebSocket errors.</summary>
	// Token: 0x0200083B RID: 2107
	public enum WebSocketError
	{
		/// <summary>Indicates that there was no native error information for the exception.</summary>
		// Token: 0x040028AA RID: 10410
		Success,
		/// <summary>Indicates that a WebSocket frame with an unknown opcode was received.</summary>
		// Token: 0x040028AB RID: 10411
		InvalidMessageType,
		/// <summary>Indicates a general error.</summary>
		// Token: 0x040028AC RID: 10412
		Faulted,
		/// <summary>Indicates that an unknown native error occurred.</summary>
		// Token: 0x040028AD RID: 10413
		NativeError,
		/// <summary>Indicates that the incoming request was not a valid websocket request.</summary>
		// Token: 0x040028AE RID: 10414
		NotAWebSocket,
		/// <summary>Indicates that the client requested an unsupported version of the WebSocket protocol.</summary>
		// Token: 0x040028AF RID: 10415
		UnsupportedVersion,
		/// <summary>Indicates that the client requested an unsupported WebSocket subprotocol.</summary>
		// Token: 0x040028B0 RID: 10416
		UnsupportedProtocol,
		/// <summary>Indicates an error occurred when parsing the HTTP headers during the opening handshake.</summary>
		// Token: 0x040028B1 RID: 10417
		HeaderError,
		/// <summary>Indicates that the connection was terminated unexpectedly.</summary>
		// Token: 0x040028B2 RID: 10418
		ConnectionClosedPrematurely,
		/// <summary>Indicates the WebSocket is an invalid state for the given operation (such as being closed or aborted).</summary>
		// Token: 0x040028B3 RID: 10419
		InvalidState
	}
}
