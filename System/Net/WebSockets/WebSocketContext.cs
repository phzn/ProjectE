using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Principal;

namespace System.Net.WebSockets
{
	/// <summary>Used for accessing the information in the WebSocket handshake.</summary>
	// Token: 0x0200083A RID: 2106
	public abstract class WebSocketContext
	{
		/// <summary>The URI requested by the WebSocket client.</summary>
		/// <returns>Returns <see cref="T:System.Uri" />.</returns>
		// Token: 0x17000F11 RID: 3857
		// (get) Token: 0x06004331 RID: 17201
		public abstract Uri RequestUri { get; }

		/// <summary>The HTTP headers that were sent to the server during the opening handshake.</summary>
		/// <returns>Returns <see cref="T:System.Collections.Specialized.NameValueCollection" />.</returns>
		// Token: 0x17000F12 RID: 3858
		// (get) Token: 0x06004332 RID: 17202
		public abstract NameValueCollection Headers { get; }

		/// <summary>The value of the Origin HTTP header included in the opening handshake.</summary>
		/// <returns>Returns <see cref="T:System.String" />.</returns>
		// Token: 0x17000F13 RID: 3859
		// (get) Token: 0x06004333 RID: 17203
		public abstract string Origin { get; }

		/// <summary>The value of the SecWebSocketKey HTTP header included in the opening handshake.</summary>
		/// <returns>Returns <see cref="T:System.Collections.Generic.IEnumerable`1" />.</returns>
		// Token: 0x17000F14 RID: 3860
		// (get) Token: 0x06004334 RID: 17204
		public abstract IEnumerable<string> SecWebSocketProtocols { get; }

		/// <summary>The list of subprotocols requested by the WebSocket client.</summary>
		/// <returns>Returns <see cref="T:System.String" />.</returns>
		// Token: 0x17000F15 RID: 3861
		// (get) Token: 0x06004335 RID: 17205
		public abstract string SecWebSocketVersion { get; }

		/// <summary>The value of the SecWebSocketKey HTTP header included in the opening handshake.</summary>
		/// <returns>Returns <see cref="T:System.String" />.</returns>
		// Token: 0x17000F16 RID: 3862
		// (get) Token: 0x06004336 RID: 17206
		public abstract string SecWebSocketKey { get; }

		/// <summary>The cookies that were passed to the server during the opening handshake.</summary>
		/// <returns>Returns <see cref="T:System.Net.CookieCollection" />.</returns>
		// Token: 0x17000F17 RID: 3863
		// (get) Token: 0x06004337 RID: 17207
		public abstract CookieCollection CookieCollection { get; }

		/// <summary>An object used to obtain identity, authentication information, and security roles for the WebSocket client.</summary>
		/// <returns>Returns <see cref="T:System.Security.Principal.IPrincipal" />.</returns>
		// Token: 0x17000F18 RID: 3864
		// (get) Token: 0x06004338 RID: 17208
		public abstract IPrincipal User { get; }

		/// <summary>Whether the WebSocket client is authenticated.</summary>
		/// <returns>Returns <see cref="T:System.Boolean" />.</returns>
		// Token: 0x17000F19 RID: 3865
		// (get) Token: 0x06004339 RID: 17209
		public abstract bool IsAuthenticated { get; }

		/// <summary>Whether the WebSocket client connected from the local machine.</summary>
		/// <returns>Returns <see cref="T:System.Boolean" />.</returns>
		// Token: 0x17000F1A RID: 3866
		// (get) Token: 0x0600433A RID: 17210
		public abstract bool IsLocal { get; }

		/// <summary>Whether the WebSocket connection is secured using Secure Sockets Layer (SSL).</summary>
		/// <returns>Returns <see cref="T:System.Boolean" />.</returns>
		// Token: 0x17000F1B RID: 3867
		// (get) Token: 0x0600433B RID: 17211
		public abstract bool IsSecureConnection { get; }

		/// <summary>The WebSocket instance used to interact (send/receive/close/etc) with the WebSocket connection.</summary>
		/// <returns>Returns <see cref="T:System.Net.WebSockets.WebSocket" />.</returns>
		// Token: 0x17000F1C RID: 3868
		// (get) Token: 0x0600433C RID: 17212
		public abstract WebSocket WebSocket { get; }
	}
}
