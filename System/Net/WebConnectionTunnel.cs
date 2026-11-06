using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
	// Token: 0x020006C4 RID: 1732
	internal class WebConnectionTunnel
	{
		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x060037B4 RID: 14260 RVA: 0x000C3CE6 File Offset: 0x000C1EE6
		public HttpWebRequest Request { get; }

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x060037B5 RID: 14261 RVA: 0x000C3CEE File Offset: 0x000C1EEE
		public Uri ConnectUri { get; }

		// Token: 0x060037B6 RID: 14262 RVA: 0x000C3CF6 File Offset: 0x000C1EF6
		public WebConnectionTunnel(HttpWebRequest request, Uri connectUri)
		{
			this.Request = request;
			this.ConnectUri = connectUri;
		}

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x060037B7 RID: 14263 RVA: 0x000C3D0C File Offset: 0x000C1F0C
		// (set) Token: 0x060037B8 RID: 14264 RVA: 0x000C3D14 File Offset: 0x000C1F14
		public bool Success { get; private set; }

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x060037B9 RID: 14265 RVA: 0x000C3D1D File Offset: 0x000C1F1D
		// (set) Token: 0x060037BA RID: 14266 RVA: 0x000C3D25 File Offset: 0x000C1F25
		public bool CloseConnection { get; private set; }

		// Token: 0x17000BA7 RID: 2983
		// (get) Token: 0x060037BB RID: 14267 RVA: 0x000C3D2E File Offset: 0x000C1F2E
		// (set) Token: 0x060037BC RID: 14268 RVA: 0x000C3D36 File Offset: 0x000C1F36
		public int StatusCode { get; private set; }

		// Token: 0x17000BA8 RID: 2984
		// (get) Token: 0x060037BD RID: 14269 RVA: 0x000C3D3F File Offset: 0x000C1F3F
		// (set) Token: 0x060037BE RID: 14270 RVA: 0x000C3D47 File Offset: 0x000C1F47
		public string StatusDescription { get; private set; }

		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x060037BF RID: 14271 RVA: 0x000C3D50 File Offset: 0x000C1F50
		// (set) Token: 0x060037C0 RID: 14272 RVA: 0x000C3D58 File Offset: 0x000C1F58
		public string[] Challenge { get; private set; }

		// Token: 0x17000BAA RID: 2986
		// (get) Token: 0x060037C1 RID: 14273 RVA: 0x000C3D61 File Offset: 0x000C1F61
		// (set) Token: 0x060037C2 RID: 14274 RVA: 0x000C3D69 File Offset: 0x000C1F69
		public WebHeaderCollection Headers { get; private set; }

		// Token: 0x17000BAB RID: 2987
		// (get) Token: 0x060037C3 RID: 14275 RVA: 0x000C3D72 File Offset: 0x000C1F72
		// (set) Token: 0x060037C4 RID: 14276 RVA: 0x000C3D7A File Offset: 0x000C1F7A
		public Version ProxyVersion { get; private set; }

		// Token: 0x17000BAC RID: 2988
		// (get) Token: 0x060037C5 RID: 14277 RVA: 0x000C3D83 File Offset: 0x000C1F83
		// (set) Token: 0x060037C6 RID: 14278 RVA: 0x000C3D8B File Offset: 0x000C1F8B
		public byte[] Data { get; private set; }

		// Token: 0x060037C7 RID: 14279 RVA: 0x000C3D94 File Offset: 0x000C1F94
		internal async Task Initialize(Stream stream, CancellationToken cancellationToken)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("CONNECT ");
			stringBuilder.Append(this.Request.Address.Host);
			stringBuilder.Append(':');
			stringBuilder.Append(this.Request.Address.Port);
			stringBuilder.Append(" HTTP/");
			if (this.Request.ProtocolVersion == HttpVersion.Version11)
			{
				stringBuilder.Append("1.1");
			}
			else
			{
				stringBuilder.Append("1.0");
			}
			stringBuilder.Append("\r\nHost: ");
			stringBuilder.Append(this.Request.Address.Authority);
			bool flag = false;
			string[] challenge = this.Challenge;
			this.Challenge = null;
			string text = this.Request.Headers["Proxy-Authorization"];
			bool have_auth = text != null;
			if (have_auth)
			{
				stringBuilder.Append("\r\nProxy-Authorization: ");
				stringBuilder.Append(text);
				flag = text.ToUpper().Contains("NTLM");
			}
			else if (challenge != null && this.StatusCode == 407)
			{
				ICredentials credentials = this.Request.Proxy.Credentials;
				have_auth = true;
				if (this.connectRequest == null)
				{
					this.connectRequest = (HttpWebRequest)WebRequest.Create(string.Concat(new string[]
					{
						this.ConnectUri.Scheme,
						"://",
						this.ConnectUri.Host,
						":",
						this.ConnectUri.Port.ToString(),
						"/"
					}));
					this.connectRequest.Method = "CONNECT";
					this.connectRequest.Credentials = credentials;
				}
				if (credentials != null)
				{
					for (int i = 0; i < challenge.Length; i++)
					{
						Authorization authorization = AuthenticationManager.Authenticate(challenge[i], this.connectRequest, credentials);
						if (authorization != null)
						{
							flag = authorization.ModuleAuthenticationType == "NTLM";
							stringBuilder.Append("\r\nProxy-Authorization: ");
							stringBuilder.Append(authorization.Message);
							break;
						}
					}
				}
			}
			if (flag)
			{
				stringBuilder.Append("\r\nProxy-Connection: keep-alive");
				this.ntlmAuthState++;
			}
			stringBuilder.Append("\r\n\r\n");
			this.StatusCode = 0;
			byte[] bytes = Encoding.Default.GetBytes(stringBuilder.ToString());
			await stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken).ConfigureAwait(false);
			ValueTuple<WebHeaderCollection, byte[], int> valueTuple = await this.ReadHeaders(stream, cancellationToken).ConfigureAwait(false);
			this.Headers = valueTuple.Item1;
			this.Data = valueTuple.Item2;
			this.StatusCode = valueTuple.Item3;
			if ((!have_auth || this.ntlmAuthState == WebConnectionTunnel.NtlmAuthState.Challenge) && this.Headers != null && this.StatusCode == 407)
			{
				string text2 = this.Headers["Connection"];
				if (!string.IsNullOrEmpty(text2) && text2.ToLower() == "close")
				{
					this.CloseConnection = true;
				}
				this.Challenge = this.Headers.GetValues("Proxy-Authenticate");
				this.Success = false;
			}
			else
			{
				this.Success = this.StatusCode == 200 && this.Headers != null;
			}
			if (this.Challenge == null && (this.StatusCode == 401 || this.StatusCode == 407))
			{
				HttpWebResponse httpWebResponse = new HttpWebResponse(this.ConnectUri, "CONNECT", (HttpStatusCode)this.StatusCode, this.Headers);
				throw new WebException((this.StatusCode == 407) ? "(407) Proxy Authentication Required" : "(401) Unauthorized", null, WebExceptionStatus.ProtocolError, httpWebResponse);
			}
		}

		// Token: 0x060037C8 RID: 14280 RVA: 0x000C3DE8 File Offset: 0x000C1FE8
		private async Task<ValueTuple<WebHeaderCollection, byte[], int>> ReadHeaders(Stream stream, CancellationToken cancellationToken)
		{
			byte[] retBuffer = null;
			int status = 200;
			byte[] buffer = new byte[1024];
			MemoryStream ms = new MemoryStream();
			int num2;
			WebHeaderCollection webHeaderCollection;
			for (;;)
			{
				cancellationToken.ThrowIfCancellationRequested();
				int num = await stream.ReadAsync(buffer, 0, 1024, cancellationToken).ConfigureAwait(false);
				if (num == 0)
				{
					break;
				}
				ms.Write(buffer, 0, num);
				num2 = 0;
				string text = null;
				bool flag = false;
				webHeaderCollection = new WebHeaderCollection();
				while (WebConnection.ReadLine(ms.GetBuffer(), ref num2, (int)ms.Length, ref text))
				{
					if (text == null)
					{
						goto Block_2;
					}
					if (flag)
					{
						webHeaderCollection.Add(text);
					}
					else
					{
						string[] array = text.Split(' ', StringSplitOptions.None);
						if (array.Length < 2)
						{
							goto Block_6;
						}
						if (string.Compare(array[0], "HTTP/1.1", true) == 0)
						{
							this.ProxyVersion = HttpVersion.Version11;
						}
						else
						{
							if (string.Compare(array[0], "HTTP/1.0", true) != 0)
							{
								goto IL_022A;
							}
							this.ProxyVersion = HttpVersion.Version10;
						}
						status = (int)uint.Parse(array[1]);
						if (array.Length >= 3)
						{
							this.StatusDescription = string.Join(" ", array, 2, array.Length - 2);
						}
						flag = true;
					}
				}
			}
			throw WebConnection.GetException(WebExceptionStatus.ServerProtocolViolation, null);
			Block_2:
			string text2 = webHeaderCollection["Content-Length"];
			int num3;
			if (string.IsNullOrEmpty(text2) || !int.TryParse(text2, out num3))
			{
				num3 = 0;
			}
			if (ms.Length - (long)num2 - (long)num3 > 0L)
			{
				retBuffer = new byte[ms.Length - (long)num2 - (long)num3];
				Buffer.BlockCopy(ms.GetBuffer(), num2 + num3, retBuffer, 0, retBuffer.Length);
			}
			else
			{
				this.FlushContents(stream, num3 - (int)(ms.Length - (long)num2));
			}
			return new ValueTuple<WebHeaderCollection, byte[], int>(webHeaderCollection, retBuffer, status);
			Block_6:
			throw WebConnection.GetException(WebExceptionStatus.ServerProtocolViolation, null);
			IL_022A:
			throw WebConnection.GetException(WebExceptionStatus.ServerProtocolViolation, null);
		}

		// Token: 0x060037C9 RID: 14281 RVA: 0x000C3E3C File Offset: 0x000C203C
		private void FlushContents(Stream stream, int contentLength)
		{
			while (contentLength > 0)
			{
				byte[] array = new byte[contentLength];
				int num = stream.Read(array, 0, contentLength);
				if (num <= 0)
				{
					break;
				}
				contentLength -= num;
			}
		}

		// Token: 0x04002085 RID: 8325
		private HttpWebRequest connectRequest;

		// Token: 0x04002086 RID: 8326
		private WebConnectionTunnel.NtlmAuthState ntlmAuthState;

		// Token: 0x020006C5 RID: 1733
		private enum NtlmAuthState
		{
			// Token: 0x04002090 RID: 8336
			None,
			// Token: 0x04002091 RID: 8337
			Challenge,
			// Token: 0x04002092 RID: 8338
			Response
		}
	}
}
