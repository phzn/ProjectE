using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Principal;
using Unity;

namespace System.Net.WebSockets
{
	/// <summary>Provides access to information received by the <see cref="T:System.Net.HttpListener" /> class when accepting WebSocket connections.</summary>
	// Token: 0x02000828 RID: 2088
	public class HttpListenerWebSocketContext : WebSocketContext
	{
		// Token: 0x060042A6 RID: 17062 RVA: 0x000E7F94 File Offset: 0x000E6194
		internal HttpListenerWebSocketContext(Uri requestUri, NameValueCollection headers, CookieCollection cookieCollection, IPrincipal user, bool isAuthenticated, bool isLocal, bool isSecureConnection, string origin, IEnumerable<string> secWebSocketProtocols, string secWebSocketVersion, string secWebSocketKey, WebSocket webSocket)
		{
			this._cookieCollection = new CookieCollection();
			this._cookieCollection.Add(cookieCollection);
			this._headers = new NameValueCollection(headers);
			this._user = HttpListenerWebSocketContext.CopyPrincipal(user);
			this._requestUri = requestUri;
			this._isAuthenticated = isAuthenticated;
			this._isLocal = isLocal;
			this._isSecureConnection = isSecureConnection;
			this._origin = origin;
			this._secWebSocketProtocols = secWebSocketProtocols;
			this._secWebSocketVersion = secWebSocketVersion;
			this._secWebSocketKey = secWebSocketKey;
			this._webSocket = webSocket;
		}

		/// <summary>Gets the URI requested by the WebSocket client.</summary>
		/// <returns>The URI requested by the WebSocket client.</returns>
		// Token: 0x17000EE6 RID: 3814
		// (get) Token: 0x060042A7 RID: 17063 RVA: 0x000E801E File Offset: 0x000E621E
		public override Uri RequestUri
		{
			get
			{
				return this._requestUri;
			}
		}

		/// <summary>Gets the HTTP headers received by the <see cref="T:System.Net.HttpListener" /> object in the WebSocket opening handshake.</summary>
		/// <returns>The HTTP headers received by the <see cref="T:System.Net.HttpListener" /> object.</returns>
		// Token: 0x17000EE7 RID: 3815
		// (get) Token: 0x060042A8 RID: 17064 RVA: 0x000E8026 File Offset: 0x000E6226
		public override NameValueCollection Headers
		{
			get
			{
				return this._headers;
			}
		}

		/// <summary>Gets the value of the Origin HTTP header included in the WebSocket opening handshake.</summary>
		/// <returns>The value of the Origin HTTP header.</returns>
		// Token: 0x17000EE8 RID: 3816
		// (get) Token: 0x060042A9 RID: 17065 RVA: 0x000E802E File Offset: 0x000E622E
		public override string Origin
		{
			get
			{
				return this._origin;
			}
		}

		/// <summary>Gets the list of the Secure WebSocket protocols included in the WebSocket opening handshake.</summary>
		/// <returns>The list of the Secure WebSocket protocols.</returns>
		// Token: 0x17000EE9 RID: 3817
		// (get) Token: 0x060042AA RID: 17066 RVA: 0x000E8036 File Offset: 0x000E6236
		public override IEnumerable<string> SecWebSocketProtocols
		{
			get
			{
				return this._secWebSocketProtocols;
			}
		}

		/// <summary>Gets the list of sub-protocols requested by the WebSocket client.</summary>
		/// <returns>The list of sub-protocols requested by the WebSocket client.</returns>
		// Token: 0x17000EEA RID: 3818
		// (get) Token: 0x060042AB RID: 17067 RVA: 0x000E803E File Offset: 0x000E623E
		public override string SecWebSocketVersion
		{
			get
			{
				return this._secWebSocketVersion;
			}
		}

		/// <summary>Gets the value of the SecWebSocketKey HTTP header included in the WebSocket opening handshake.</summary>
		/// <returns>The value of the SecWebSocketKey HTTP header.</returns>
		// Token: 0x17000EEB RID: 3819
		// (get) Token: 0x060042AC RID: 17068 RVA: 0x000E8046 File Offset: 0x000E6246
		public override string SecWebSocketKey
		{
			get
			{
				return this._secWebSocketKey;
			}
		}

		/// <summary>Gets the cookies received by the <see cref="T:System.Net.HttpListener" /> object in the WebSocket opening handshake.</summary>
		/// <returns>The cookies received by the <see cref="T:System.Net.HttpListener" /> object.</returns>
		// Token: 0x17000EEC RID: 3820
		// (get) Token: 0x060042AD RID: 17069 RVA: 0x000E804E File Offset: 0x000E624E
		public override CookieCollection CookieCollection
		{
			get
			{
				return this._cookieCollection;
			}
		}

		/// <summary>Gets an object used to obtain identity, authentication information, and security roles for the WebSocket client.</summary>
		/// <returns>The identity, authentication information, and security roles for the WebSocket client.</returns>
		// Token: 0x17000EED RID: 3821
		// (get) Token: 0x060042AE RID: 17070 RVA: 0x000E8056 File Offset: 0x000E6256
		public override IPrincipal User
		{
			get
			{
				return this._user;
			}
		}

		/// <summary>Gets a value that indicates if the WebSocket client is authenticated.</summary>
		/// <returns>
		///   <see langword="true" /> if the WebSocket client is authenticated; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000EEE RID: 3822
		// (get) Token: 0x060042AF RID: 17071 RVA: 0x000E805E File Offset: 0x000E625E
		public override bool IsAuthenticated
		{
			get
			{
				return this._isAuthenticated;
			}
		}

		/// <summary>Gets a value that indicates if the WebSocket client connected from the local machine.</summary>
		/// <returns>
		///   <see langword="true" /> if the WebSocket client connected from the local machine; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000EEF RID: 3823
		// (get) Token: 0x060042B0 RID: 17072 RVA: 0x000E8066 File Offset: 0x000E6266
		public override bool IsLocal
		{
			get
			{
				return this._isLocal;
			}
		}

		/// <summary>Gets a value that indicates if the WebSocket connection is secured using Secure Sockets Layer (SSL).</summary>
		/// <returns>
		///   <see langword="true" /> if the WebSocket connection is secured using SSL; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000EF0 RID: 3824
		// (get) Token: 0x060042B1 RID: 17073 RVA: 0x000E806E File Offset: 0x000E626E
		public override bool IsSecureConnection
		{
			get
			{
				return this._isSecureConnection;
			}
		}

		/// <summary>Gets the <see cref="T:System.Net.WebSockets.WebSocket" /> instance used to send and receive data over the <see cref="T:System.Net.WebSockets.WebSocket" /> connection.</summary>
		/// <returns>The <see cref="T:System.Net.WebSockets.WebSocket" /> instance used to send and receive data over the <see cref="T:System.Net.WebSockets.WebSocket" /> connection.</returns>
		// Token: 0x17000EF1 RID: 3825
		// (get) Token: 0x060042B2 RID: 17074 RVA: 0x000E8076 File Offset: 0x000E6276
		public override WebSocket WebSocket
		{
			get
			{
				return this._webSocket;
			}
		}

		// Token: 0x060042B3 RID: 17075 RVA: 0x000E8080 File Offset: 0x000E6280
		private static IPrincipal CopyPrincipal(IPrincipal user)
		{
			if (user != null)
			{
				if (user is WindowsPrincipal)
				{
					throw new PlatformNotSupportedException();
				}
				HttpListenerBasicIdentity httpListenerBasicIdentity = user.Identity as HttpListenerBasicIdentity;
				if (httpListenerBasicIdentity != null)
				{
					return new GenericPrincipal(new HttpListenerBasicIdentity(httpListenerBasicIdentity.Name, httpListenerBasicIdentity.Password), null);
				}
			}
			return null;
		}

		// Token: 0x060042B4 RID: 17076 RVA: 0x00013BCA File Offset: 0x00011DCA
		internal HttpListenerWebSocketContext()
		{
			global::Unity.ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x0400282B RID: 10283
		private readonly Uri _requestUri;

		// Token: 0x0400282C RID: 10284
		private readonly NameValueCollection _headers;

		// Token: 0x0400282D RID: 10285
		private readonly CookieCollection _cookieCollection;

		// Token: 0x0400282E RID: 10286
		private readonly IPrincipal _user;

		// Token: 0x0400282F RID: 10287
		private readonly bool _isAuthenticated;

		// Token: 0x04002830 RID: 10288
		private readonly bool _isLocal;

		// Token: 0x04002831 RID: 10289
		private readonly bool _isSecureConnection;

		// Token: 0x04002832 RID: 10290
		private readonly string _origin;

		// Token: 0x04002833 RID: 10291
		private readonly IEnumerable<string> _secWebSocketProtocols;

		// Token: 0x04002834 RID: 10292
		private readonly string _secWebSocketVersion;

		// Token: 0x04002835 RID: 10293
		private readonly string _secWebSocketKey;

		// Token: 0x04002836 RID: 10294
		private readonly WebSocket _webSocket;
	}
}
