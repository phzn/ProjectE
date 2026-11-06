using System;

namespace System.Net.WebSockets
{
	/// <summary>Represents well known WebSocket close codes as defined in section 11.7 of the WebSocket protocol spec.</summary>
	// Token: 0x02000839 RID: 2105
	public enum WebSocketCloseStatus
	{
		/// <summary>(1000) The connection has closed after the request was fulfilled.</summary>
		// Token: 0x0400289F RID: 10399
		NormalClosure = 1000,
		/// <summary>(1001) Indicates an endpoint is being removed. Either the server or client will become unavailable.</summary>
		// Token: 0x040028A0 RID: 10400
		EndpointUnavailable,
		/// <summary>(1002) The client or server is terminating the connection because of a protocol error.</summary>
		// Token: 0x040028A1 RID: 10401
		ProtocolError,
		/// <summary>(1003) The client or server is terminating the connection because it cannot accept the data type it received.</summary>
		// Token: 0x040028A2 RID: 10402
		InvalidMessageType,
		/// <summary>No error specified.</summary>
		// Token: 0x040028A3 RID: 10403
		Empty = 1005,
		/// <summary>(1007) The client or server is terminating the connection because it has received data inconsistent with the message type.</summary>
		// Token: 0x040028A4 RID: 10404
		InvalidPayloadData = 1007,
		/// <summary>(1008) The connection will be closed because an endpoint has received a message that violates its policy.</summary>
		// Token: 0x040028A5 RID: 10405
		PolicyViolation,
		/// <summary>(1004) Reserved for future use.</summary>
		// Token: 0x040028A6 RID: 10406
		MessageTooBig,
		/// <summary>(1010) The client is terminating the connection because it expected the server to negotiate an extension.</summary>
		// Token: 0x040028A7 RID: 10407
		MandatoryExtension,
		/// <summary>The connection will be closed by the server because of an error on the server.</summary>
		// Token: 0x040028A8 RID: 10408
		InternalServerError
	}
}
