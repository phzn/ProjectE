using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace System.Net.WebSockets
{
	/// <summary>Options to use with a  <see cref="T:System.Net.WebSockets.ClientWebSocket" /> object.</summary>
	// Token: 0x0200082D RID: 2093
	public sealed class ClientWebSocketOptions
	{
		// Token: 0x060042CF RID: 17103 RVA: 0x000E852E File Offset: 0x000E672E
		internal ClientWebSocketOptions()
		{
			this._requestedSubProtocols = new List<string>();
			this._requestHeaders = new WebHeaderCollection();
		}

		/// <summary>Creates a HTTP request header and its value.</summary>
		/// <param name="headerName">The name of the HTTP header.</param>
		/// <param name="headerValue">The value of the HTTP header.</param>
		// Token: 0x060042D0 RID: 17104 RVA: 0x000E856D File Offset: 0x000E676D
		public void SetRequestHeader(string headerName, string headerValue)
		{
			this.ThrowIfReadOnly();
			this._requestHeaders.Set(headerName, headerValue);
		}

		// Token: 0x17000EF9 RID: 3833
		// (get) Token: 0x060042D1 RID: 17105 RVA: 0x000E8582 File Offset: 0x000E6782
		internal WebHeaderCollection RequestHeaders
		{
			get
			{
				return this._requestHeaders;
			}
		}

		// Token: 0x17000EFA RID: 3834
		// (get) Token: 0x060042D2 RID: 17106 RVA: 0x000E858A File Offset: 0x000E678A
		internal List<string> RequestedSubProtocols
		{
			get
			{
				return this._requestedSubProtocols;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that indicates if default credentials should be used during WebSocket handshake.</summary>
		/// <returns>
		///   <see langword="true" /> if default credentials should be used during WebSocket handshake; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000EFB RID: 3835
		// (get) Token: 0x060042D3 RID: 17107 RVA: 0x000E8592 File Offset: 0x000E6792
		// (set) Token: 0x060042D4 RID: 17108 RVA: 0x000E859A File Offset: 0x000E679A
		public bool UseDefaultCredentials
		{
			get
			{
				return this._useDefaultCredentials;
			}
			set
			{
				this.ThrowIfReadOnly();
				this._useDefaultCredentials = value;
			}
		}

		/// <summary>Gets or sets the credential information for the client.</summary>
		/// <returns>The credential information for the client.</returns>
		// Token: 0x17000EFC RID: 3836
		// (get) Token: 0x060042D5 RID: 17109 RVA: 0x000E85A9 File Offset: 0x000E67A9
		// (set) Token: 0x060042D6 RID: 17110 RVA: 0x000E85B1 File Offset: 0x000E67B1
		public ICredentials Credentials
		{
			get
			{
				return this._credentials;
			}
			set
			{
				this.ThrowIfReadOnly();
				this._credentials = value;
			}
		}

		/// <summary>Gets or sets the proxy for WebSocket requests.</summary>
		/// <returns>The proxy for WebSocket requests.</returns>
		// Token: 0x17000EFD RID: 3837
		// (get) Token: 0x060042D7 RID: 17111 RVA: 0x000E85C0 File Offset: 0x000E67C0
		// (set) Token: 0x060042D8 RID: 17112 RVA: 0x000E85C8 File Offset: 0x000E67C8
		public IWebProxy Proxy
		{
			get
			{
				return this._proxy;
			}
			set
			{
				this.ThrowIfReadOnly();
				this._proxy = value;
			}
		}

		/// <summary>Gets or sets a collection of client side certificates.</summary>
		/// <returns>A collection of client side certificates.</returns>
		// Token: 0x17000EFE RID: 3838
		// (get) Token: 0x060042D9 RID: 17113 RVA: 0x000E85D7 File Offset: 0x000E67D7
		// (set) Token: 0x060042DA RID: 17114 RVA: 0x000E85F2 File Offset: 0x000E67F2
		public X509CertificateCollection ClientCertificates
		{
			get
			{
				if (this._clientCertificates == null)
				{
					this._clientCertificates = new X509CertificateCollection();
				}
				return this._clientCertificates;
			}
			set
			{
				this.ThrowIfReadOnly();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._clientCertificates = value;
			}
		}

		// Token: 0x17000EFF RID: 3839
		// (get) Token: 0x060042DB RID: 17115 RVA: 0x000E860F File Offset: 0x000E680F
		// (set) Token: 0x060042DC RID: 17116 RVA: 0x000E8617 File Offset: 0x000E6817
		public RemoteCertificateValidationCallback RemoteCertificateValidationCallback
		{
			get
			{
				return this._remoteCertificateValidationCallback;
			}
			set
			{
				this.ThrowIfReadOnly();
				this._remoteCertificateValidationCallback = value;
			}
		}

		/// <summary>Gets or sets the cookies associated with the request.</summary>
		/// <returns>The cookies associated with the request.</returns>
		// Token: 0x17000F00 RID: 3840
		// (get) Token: 0x060042DD RID: 17117 RVA: 0x000E8626 File Offset: 0x000E6826
		// (set) Token: 0x060042DE RID: 17118 RVA: 0x000E862E File Offset: 0x000E682E
		public CookieContainer Cookies
		{
			get
			{
				return this._cookies;
			}
			set
			{
				this.ThrowIfReadOnly();
				this._cookies = value;
			}
		}

		/// <summary>Adds a sub-protocol to be negotiated during the WebSocket connection handshake.</summary>
		/// <param name="subProtocol">The WebSocket sub-protocol to add.</param>
		// Token: 0x060042DF RID: 17119 RVA: 0x000E8640 File Offset: 0x000E6840
		public void AddSubProtocol(string subProtocol)
		{
			this.ThrowIfReadOnly();
			WebSocketValidate.ValidateSubprotocol(subProtocol);
			using (List<string>.Enumerator enumerator = this._requestedSubProtocols.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (string.Equals(enumerator.Current, subProtocol, StringComparison.OrdinalIgnoreCase))
					{
						throw new ArgumentException(SR.Format("Duplicate protocols are not allowed: '{0}'.", subProtocol), "subProtocol");
					}
				}
			}
			this._requestedSubProtocols.Add(subProtocol);
		}

		/// <summary>Gets or sets the WebSocket protocol keep-alive interval.</summary>
		/// <returns>The WebSocket protocol keep-alive interval.</returns>
		// Token: 0x17000F01 RID: 3841
		// (get) Token: 0x060042E0 RID: 17120 RVA: 0x000E86C4 File Offset: 0x000E68C4
		// (set) Token: 0x060042E1 RID: 17121 RVA: 0x000E86CC File Offset: 0x000E68CC
		public TimeSpan KeepAliveInterval
		{
			get
			{
				return this._keepAliveInterval;
			}
			set
			{
				this.ThrowIfReadOnly();
				if (value != Timeout.InfiniteTimeSpan && value < TimeSpan.Zero)
				{
					throw new ArgumentOutOfRangeException("value", value, SR.Format("The argument must be a value greater than {0}.", Timeout.InfiniteTimeSpan.ToString()));
				}
				this._keepAliveInterval = value;
			}
		}

		// Token: 0x17000F02 RID: 3842
		// (get) Token: 0x060042E2 RID: 17122 RVA: 0x000E872B File Offset: 0x000E692B
		internal int ReceiveBufferSize
		{
			get
			{
				return this._receiveBufferSize;
			}
		}

		// Token: 0x17000F03 RID: 3843
		// (get) Token: 0x060042E3 RID: 17123 RVA: 0x000E8733 File Offset: 0x000E6933
		internal int SendBufferSize
		{
			get
			{
				return this._sendBufferSize;
			}
		}

		// Token: 0x17000F04 RID: 3844
		// (get) Token: 0x060042E4 RID: 17124 RVA: 0x000E873B File Offset: 0x000E693B
		internal ArraySegment<byte>? Buffer
		{
			get
			{
				return this._buffer;
			}
		}

		/// <summary>Sets the client buffer parameters.</summary>
		/// <param name="receiveBufferSize">The size, in bytes, of the client receive buffer.</param>
		/// <param name="sendBufferSize">The size, in bytes, of the client send buffer.</param>
		// Token: 0x060042E5 RID: 17125 RVA: 0x000E8744 File Offset: 0x000E6944
		public void SetBuffer(int receiveBufferSize, int sendBufferSize)
		{
			this.ThrowIfReadOnly();
			if (receiveBufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("receiveBufferSize", receiveBufferSize, SR.Format("The argument must be a value greater than {0}.", 1));
			}
			if (sendBufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("sendBufferSize", sendBufferSize, SR.Format("The argument must be a value greater than {0}.", 1));
			}
			this._receiveBufferSize = receiveBufferSize;
			this._sendBufferSize = sendBufferSize;
			this._buffer = null;
		}

		/// <summary>Sets client buffer parameters.</summary>
		/// <param name="receiveBufferSize">The size, in bytes, of the client receive buffer.</param>
		/// <param name="sendBufferSize">The size, in bytes, of the client send buffer.</param>
		/// <param name="buffer">The receive buffer to use.</param>
		// Token: 0x060042E6 RID: 17126 RVA: 0x000E87BC File Offset: 0x000E69BC
		public void SetBuffer(int receiveBufferSize, int sendBufferSize, ArraySegment<byte> buffer)
		{
			this.ThrowIfReadOnly();
			if (receiveBufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("receiveBufferSize", receiveBufferSize, SR.Format("The argument must be a value greater than {0}.", 1));
			}
			if (sendBufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("sendBufferSize", sendBufferSize, SR.Format("The argument must be a value greater than {0}.", 1));
			}
			WebSocketValidate.ValidateArraySegment(buffer, "buffer");
			if (buffer.Count == 0)
			{
				throw new ArgumentOutOfRangeException("buffer");
			}
			this._receiveBufferSize = receiveBufferSize;
			this._sendBufferSize = sendBufferSize;
			this._buffer = new ArraySegment<byte>?(buffer);
		}

		// Token: 0x060042E7 RID: 17127 RVA: 0x000E8852 File Offset: 0x000E6A52
		internal void SetToReadOnly()
		{
			this._isReadOnly = true;
		}

		// Token: 0x060042E8 RID: 17128 RVA: 0x000E885B File Offset: 0x000E6A5B
		private void ThrowIfReadOnly()
		{
			if (this._isReadOnly)
			{
				throw new InvalidOperationException("The WebSocket has already been started.");
			}
		}

		// Token: 0x04002846 RID: 10310
		private bool _isReadOnly;

		// Token: 0x04002847 RID: 10311
		private readonly List<string> _requestedSubProtocols;

		// Token: 0x04002848 RID: 10312
		private readonly WebHeaderCollection _requestHeaders;

		// Token: 0x04002849 RID: 10313
		private TimeSpan _keepAliveInterval = WebSocket.DefaultKeepAliveInterval;

		// Token: 0x0400284A RID: 10314
		private bool _useDefaultCredentials;

		// Token: 0x0400284B RID: 10315
		private ICredentials _credentials;

		// Token: 0x0400284C RID: 10316
		private IWebProxy _proxy;

		// Token: 0x0400284D RID: 10317
		private X509CertificateCollection _clientCertificates;

		// Token: 0x0400284E RID: 10318
		private CookieContainer _cookies;

		// Token: 0x0400284F RID: 10319
		private int _receiveBufferSize = 4096;

		// Token: 0x04002850 RID: 10320
		private int _sendBufferSize = 4096;

		// Token: 0x04002851 RID: 10321
		private ArraySegment<byte>? _buffer;

		// Token: 0x04002852 RID: 10322
		private RemoteCertificateValidationCallback _remoteCertificateValidationCallback;
	}
}
