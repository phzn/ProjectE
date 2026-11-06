using System;

namespace System.Net.WebSockets
{
	/// <summary>An instance of this class represents the result of performing a single ReceiveAsync operation on a WebSocket.</summary>
	// Token: 0x0200083E RID: 2110
	public class WebSocketReceiveResult
	{
		/// <summary>Creates an instance of the <see cref="T:System.Net.WebSockets.WebSocketReceiveResult" /> class.</summary>
		/// <param name="count">The number of bytes received.</param>
		/// <param name="messageType">The type of message that was received.</param>
		/// <param name="endOfMessage">Indicates whether this is the final message.</param>
		// Token: 0x06004353 RID: 17235 RVA: 0x000EA4F8 File Offset: 0x000E86F8
		public WebSocketReceiveResult(int count, WebSocketMessageType messageType, bool endOfMessage)
			: this(count, messageType, endOfMessage, null, null)
		{
		}

		/// <summary>Creates an instance of the <see cref="T:System.Net.WebSockets.WebSocketReceiveResult" /> class.</summary>
		/// <param name="count">The number of bytes received.</param>
		/// <param name="messageType">The type of message that was received.</param>
		/// <param name="endOfMessage">Indicates whether this is the final message.</param>
		/// <param name="closeStatus">Indicates the <see cref="T:System.Net.WebSockets.WebSocketCloseStatus" /> of the connection.</param>
		/// <param name="closeStatusDescription">The description of <paramref name="closeStatus" />.</param>
		// Token: 0x06004354 RID: 17236 RVA: 0x000EA518 File Offset: 0x000E8718
		public WebSocketReceiveResult(int count, WebSocketMessageType messageType, bool endOfMessage, WebSocketCloseStatus? closeStatus, string closeStatusDescription)
		{
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this.Count = count;
			this.EndOfMessage = endOfMessage;
			this.MessageType = messageType;
			this.CloseStatus = closeStatus;
			this.CloseStatusDescription = closeStatusDescription;
		}

		/// <summary>Indicates the number of bytes that the WebSocket received.</summary>
		/// <returns>Returns <see cref="T:System.Int32" />.</returns>
		// Token: 0x17000F1F RID: 3871
		// (get) Token: 0x06004355 RID: 17237 RVA: 0x000EA554 File Offset: 0x000E8754
		public int Count { get; }

		/// <summary>Indicates whether the message has been received completely.</summary>
		/// <returns>Returns <see cref="T:System.Boolean" />.</returns>
		// Token: 0x17000F20 RID: 3872
		// (get) Token: 0x06004356 RID: 17238 RVA: 0x000EA55C File Offset: 0x000E875C
		public bool EndOfMessage { get; }

		/// <summary>Indicates whether the current message is a UTF-8 message or a binary message.</summary>
		/// <returns>Returns <see cref="T:System.Net.WebSockets.WebSocketMessageType" />.</returns>
		// Token: 0x17000F21 RID: 3873
		// (get) Token: 0x06004357 RID: 17239 RVA: 0x000EA564 File Offset: 0x000E8764
		public WebSocketMessageType MessageType { get; }

		/// <summary>Indicates the reason why the remote endpoint initiated the close handshake.</summary>
		/// <returns>Returns <see cref="T:System.Net.WebSockets.WebSocketCloseStatus" />.</returns>
		// Token: 0x17000F22 RID: 3874
		// (get) Token: 0x06004358 RID: 17240 RVA: 0x000EA56C File Offset: 0x000E876C
		public WebSocketCloseStatus? CloseStatus { get; }

		/// <summary>Returns the optional description that describes why the close handshake has been initiated by the remote endpoint.</summary>
		/// <returns>Returns <see cref="T:System.String" />.</returns>
		// Token: 0x17000F23 RID: 3875
		// (get) Token: 0x06004359 RID: 17241 RVA: 0x000EA574 File Offset: 0x000E8774
		public string CloseStatusDescription { get; }
	}
}
