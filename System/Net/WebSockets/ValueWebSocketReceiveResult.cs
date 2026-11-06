using System;

namespace System.Net.WebSockets
{
	// Token: 0x02000835 RID: 2101
	public readonly struct ValueWebSocketReceiveResult
	{
		// Token: 0x06004310 RID: 17168 RVA: 0x000E9B8E File Offset: 0x000E7D8E
		public ValueWebSocketReceiveResult(int count, WebSocketMessageType messageType, bool endOfMessage)
		{
			if (count < 0)
			{
				ValueWebSocketReceiveResult.ThrowCountOutOfRange();
			}
			if (messageType > WebSocketMessageType.Close)
			{
				ValueWebSocketReceiveResult.ThrowMessageTypeOutOfRange();
			}
			this._countAndEndOfMessage = (uint)(count | (endOfMessage ? int.MinValue : 0));
			this._messageType = messageType;
		}

		// Token: 0x17000F09 RID: 3849
		// (get) Token: 0x06004311 RID: 17169 RVA: 0x000E9BBC File Offset: 0x000E7DBC
		public int Count
		{
			get
			{
				return (int)(this._countAndEndOfMessage & 2147483647U);
			}
		}

		// Token: 0x17000F0A RID: 3850
		// (get) Token: 0x06004312 RID: 17170 RVA: 0x000E9BCA File Offset: 0x000E7DCA
		public bool EndOfMessage
		{
			get
			{
				return (this._countAndEndOfMessage & 2147483648U) == 2147483648U;
			}
		}

		// Token: 0x17000F0B RID: 3851
		// (get) Token: 0x06004313 RID: 17171 RVA: 0x000E9BDF File Offset: 0x000E7DDF
		public WebSocketMessageType MessageType
		{
			get
			{
				return this._messageType;
			}
		}

		// Token: 0x06004314 RID: 17172 RVA: 0x000E9BE7 File Offset: 0x000E7DE7
		private static void ThrowCountOutOfRange()
		{
			throw new ArgumentOutOfRangeException("count");
		}

		// Token: 0x06004315 RID: 17173 RVA: 0x000E9BF3 File Offset: 0x000E7DF3
		private static void ThrowMessageTypeOutOfRange()
		{
			throw new ArgumentOutOfRangeException("messageType");
		}

		// Token: 0x0400288C RID: 10380
		private readonly uint _countAndEndOfMessage;

		// Token: 0x0400288D RID: 10381
		private readonly WebSocketMessageType _messageType;
	}
}
