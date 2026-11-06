using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Mono.Net.Security;
using Mono.Security.Interface;

namespace System.Net.Http
{
	// Token: 0x0200000E RID: 14
	internal class MonoWebRequestHandler : IMonoHttpClientHandler, IDisposable
	{
		// Token: 0x0600007C RID: 124 RVA: 0x00002C70 File Offset: 0x00000E70
		public MonoWebRequestHandler()
		{
			this.allowAutoRedirect = true;
			this.maxAutomaticRedirections = 50;
			this.maxRequestContentBufferSize = 2147483647L;
			this.useCookies = true;
			this.useProxy = true;
			this.allowPipelining = true;
			this.authenticationLevel = AuthenticationLevel.MutualAuthRequested;
			this.cachePolicy = WebRequest.DefaultCachePolicy;
			this.continueTimeout = TimeSpan.FromMilliseconds(350.0);
			this.impersonationLevel = TokenImpersonationLevel.Delegation;
			this.maxResponseHeadersLength = HttpWebRequest.DefaultMaximumResponseHeadersLength;
			this.readWriteTimeout = 300000;
			this.serverCertificateValidationCallback = null;
			this.unsafeAuthenticatedConnectionSharing = false;
			this.connectionGroupName = "HttpClientHandler" + Interlocked.Increment(ref MonoWebRequestHandler.groupCounter).ToString();
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00002D26 File Offset: 0x00000F26
		internal void EnsureModifiability()
		{
			if (this.sentRequest)
			{
				throw new InvalidOperationException("This instance has already started one or more requests. Properties can only be modified before sending the first request.");
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00002D3B File Offset: 0x00000F3B
		// (set) Token: 0x0600007F RID: 127 RVA: 0x00002D43 File Offset: 0x00000F43
		public bool AllowAutoRedirect
		{
			get
			{
				return this.allowAutoRedirect;
			}
			set
			{
				this.EnsureModifiability();
				this.allowAutoRedirect = value;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00002D52 File Offset: 0x00000F52
		// (set) Token: 0x06000081 RID: 129 RVA: 0x00002D5A File Offset: 0x00000F5A
		public DecompressionMethods AutomaticDecompression
		{
			get
			{
				return this.automaticDecompression;
			}
			set
			{
				this.EnsureModifiability();
				this.automaticDecompression = value;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00002D6C File Offset: 0x00000F6C
		// (set) Token: 0x06000083 RID: 131 RVA: 0x00002D91 File Offset: 0x00000F91
		public CookieContainer CookieContainer
		{
			get
			{
				CookieContainer cookieContainer;
				if ((cookieContainer = this.cookieContainer) == null)
				{
					cookieContainer = (this.cookieContainer = new CookieContainer());
				}
				return cookieContainer;
			}
			set
			{
				this.EnsureModifiability();
				this.cookieContainer = value;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00002DA0 File Offset: 0x00000FA0
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00002DA8 File Offset: 0x00000FA8
		public ICredentials Credentials
		{
			get
			{
				return this.credentials;
			}
			set
			{
				this.EnsureModifiability();
				this.credentials = value;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00002DB7 File Offset: 0x00000FB7
		// (set) Token: 0x06000087 RID: 135 RVA: 0x00002DBF File Offset: 0x00000FBF
		public int MaxAutomaticRedirections
		{
			get
			{
				return this.maxAutomaticRedirections;
			}
			set
			{
				this.EnsureModifiability();
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException();
				}
				this.maxAutomaticRedirections = value;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00002DD8 File Offset: 0x00000FD8
		// (set) Token: 0x06000089 RID: 137 RVA: 0x00002DE0 File Offset: 0x00000FE0
		public long MaxRequestContentBufferSize
		{
			get
			{
				return this.maxRequestContentBufferSize;
			}
			set
			{
				this.EnsureModifiability();
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException();
				}
				this.maxRequestContentBufferSize = value;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00002DFA File Offset: 0x00000FFA
		// (set) Token: 0x0600008B RID: 139 RVA: 0x00002E02 File Offset: 0x00001002
		public bool PreAuthenticate
		{
			get
			{
				return this.preAuthenticate;
			}
			set
			{
				this.EnsureModifiability();
				this.preAuthenticate = value;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00002E11 File Offset: 0x00001011
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00002E19 File Offset: 0x00001019
		public IWebProxy Proxy
		{
			get
			{
				return this.proxy;
			}
			set
			{
				this.EnsureModifiability();
				if (!this.UseProxy)
				{
					throw new InvalidOperationException();
				}
				this.proxy = value;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600008E RID: 142 RVA: 0x000028C2 File Offset: 0x00000AC2
		public virtual bool SupportsAutomaticDecompression
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600008F RID: 143 RVA: 0x000028C2 File Offset: 0x00000AC2
		public virtual bool SupportsProxy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000090 RID: 144 RVA: 0x000028C2 File Offset: 0x00000AC2
		public virtual bool SupportsRedirectConfiguration
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00002E36 File Offset: 0x00001036
		// (set) Token: 0x06000092 RID: 146 RVA: 0x00002E3E File Offset: 0x0000103E
		public bool UseCookies
		{
			get
			{
				return this.useCookies;
			}
			set
			{
				this.EnsureModifiability();
				this.useCookies = value;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00002E4D File Offset: 0x0000104D
		// (set) Token: 0x06000094 RID: 148 RVA: 0x00002E55 File Offset: 0x00001055
		public bool UseProxy
		{
			get
			{
				return this.useProxy;
			}
			set
			{
				this.EnsureModifiability();
				this.useProxy = value;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00002E64 File Offset: 0x00001064
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00002E6C File Offset: 0x0000106C
		public bool AllowPipelining
		{
			get
			{
				return this.allowPipelining;
			}
			set
			{
				this.EnsureModifiability();
				this.allowPipelining = value;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00002E7B File Offset: 0x0000107B
		// (set) Token: 0x06000098 RID: 152 RVA: 0x00002E83 File Offset: 0x00001083
		public RequestCachePolicy CachePolicy
		{
			get
			{
				return this.cachePolicy;
			}
			set
			{
				this.EnsureModifiability();
				this.cachePolicy = value;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00002E92 File Offset: 0x00001092
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00002E9A File Offset: 0x0000109A
		public AuthenticationLevel AuthenticationLevel
		{
			get
			{
				return this.authenticationLevel;
			}
			set
			{
				this.EnsureModifiability();
				this.authenticationLevel = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00002EA9 File Offset: 0x000010A9
		// (set) Token: 0x0600009C RID: 156 RVA: 0x00002EB1 File Offset: 0x000010B1
		[MonoTODO]
		public TimeSpan ContinueTimeout
		{
			get
			{
				return this.continueTimeout;
			}
			set
			{
				this.EnsureModifiability();
				this.continueTimeout = value;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00002EC0 File Offset: 0x000010C0
		// (set) Token: 0x0600009E RID: 158 RVA: 0x00002EC8 File Offset: 0x000010C8
		public TokenImpersonationLevel ImpersonationLevel
		{
			get
			{
				return this.impersonationLevel;
			}
			set
			{
				this.EnsureModifiability();
				this.impersonationLevel = value;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600009F RID: 159 RVA: 0x00002ED7 File Offset: 0x000010D7
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x00002EDF File Offset: 0x000010DF
		public int MaxResponseHeadersLength
		{
			get
			{
				return this.maxResponseHeadersLength;
			}
			set
			{
				this.EnsureModifiability();
				this.maxResponseHeadersLength = value;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00002EEE File Offset: 0x000010EE
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x00002EF6 File Offset: 0x000010F6
		public int ReadWriteTimeout
		{
			get
			{
				return this.readWriteTimeout;
			}
			set
			{
				this.EnsureModifiability();
				this.readWriteTimeout = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00002F05 File Offset: 0x00001105
		// (set) Token: 0x060000A4 RID: 164 RVA: 0x00002F0D File Offset: 0x0000110D
		public RemoteCertificateValidationCallback ServerCertificateValidationCallback
		{
			get
			{
				return this.serverCertificateValidationCallback;
			}
			set
			{
				this.EnsureModifiability();
				this.serverCertificateValidationCallback = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00002F1C File Offset: 0x0000111C
		// (set) Token: 0x060000A6 RID: 166 RVA: 0x00002F24 File Offset: 0x00001124
		public bool UnsafeAuthenticatedConnectionSharing
		{
			get
			{
				return this.unsafeAuthenticatedConnectionSharing;
			}
			set
			{
				this.EnsureModifiability();
				this.unsafeAuthenticatedConnectionSharing = value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00002F34 File Offset: 0x00001134
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x00002F59 File Offset: 0x00001159
		public SslClientAuthenticationOptions SslOptions
		{
			get
			{
				SslClientAuthenticationOptions sslClientAuthenticationOptions;
				if ((sslClientAuthenticationOptions = this.sslOptions) == null)
				{
					sslClientAuthenticationOptions = (this.sslOptions = new SslClientAuthenticationOptions());
				}
				return sslClientAuthenticationOptions;
			}
			set
			{
				this.EnsureModifiability();
				this.sslOptions = value;
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00002F68 File Offset: 0x00001168
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00002F71 File Offset: 0x00001171
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && !this.disposed)
			{
				Volatile.Write(ref this.disposed, true);
				ServicePointManager.CloseConnectionGroup(this.connectionGroupName);
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00002F95 File Offset: 0x00001195
		private bool GetConnectionKeepAlive(HttpRequestHeaders headers)
		{
			return headers.Connection.Any<string>((string l) => string.Equals(l, "Keep-Alive", StringComparison.OrdinalIgnoreCase));
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00002FC4 File Offset: 0x000011C4
		internal virtual HttpWebRequest CreateWebRequest(HttpRequestMessage request)
		{
			HttpWebRequest httpWebRequest;
			if (HttpUtilities.IsSupportedSecureScheme(request.RequestUri.Scheme))
			{
				httpWebRequest = new HttpWebRequest(request.RequestUri, Mono.Net.Security.MonoTlsProviderFactory.GetProviderInternal(), MonoTlsSettings.CopyDefaultSettings());
				httpWebRequest.TlsSettings.ClientCertificateSelectionCallback = (string t, X509CertificateCollection lc, X509Certificate rc, string[] ai) => this.SslOptions.LocalCertificateSelectionCallback(this, t, lc, rc, ai);
			}
			else
			{
				httpWebRequest = new HttpWebRequest(request.RequestUri);
			}
			httpWebRequest.ThrowOnError = false;
			httpWebRequest.AllowWriteStreamBuffering = false;
			if (request.Version == HttpVersion.Version20)
			{
				httpWebRequest.ProtocolVersion = HttpVersion.Version11;
			}
			else
			{
				httpWebRequest.ProtocolVersion = request.Version;
			}
			httpWebRequest.ConnectionGroupName = this.connectionGroupName;
			httpWebRequest.Method = request.Method.Method;
			bool? flag;
			bool flag2;
			if (httpWebRequest.ProtocolVersion == HttpVersion.Version10)
			{
				httpWebRequest.KeepAlive = this.GetConnectionKeepAlive(request.Headers);
			}
			else
			{
				HttpWebRequest httpWebRequest2 = httpWebRequest;
				flag = request.Headers.ConnectionClose;
				flag2 = true;
				httpWebRequest2.KeepAlive = !((flag.GetValueOrDefault() == flag2) & (flag != null));
			}
			if (this.allowAutoRedirect)
			{
				httpWebRequest.AllowAutoRedirect = true;
				httpWebRequest.MaximumAutomaticRedirections = this.maxAutomaticRedirections;
			}
			else
			{
				httpWebRequest.AllowAutoRedirect = false;
			}
			httpWebRequest.AutomaticDecompression = this.automaticDecompression;
			httpWebRequest.PreAuthenticate = this.preAuthenticate;
			if (this.useCookies)
			{
				httpWebRequest.CookieContainer = this.CookieContainer;
			}
			httpWebRequest.Credentials = this.credentials;
			if (this.useProxy)
			{
				httpWebRequest.Proxy = this.proxy;
			}
			else
			{
				httpWebRequest.Proxy = null;
			}
			ServicePoint servicePoint = httpWebRequest.ServicePoint;
			flag = request.Headers.ExpectContinue;
			flag2 = true;
			servicePoint.Expect100Continue = (flag.GetValueOrDefault() == flag2) & (flag != null);
			if (this.timeout != null)
			{
				httpWebRequest.Timeout = (int)this.timeout.Value.TotalMilliseconds;
			}
			httpWebRequest.ServerCertificateValidationCallback = this.SslOptions.RemoteCertificateValidationCallback;
			WebHeaderCollection headers = httpWebRequest.Headers;
			foreach (KeyValuePair<string, IEnumerable<string>> keyValuePair in request.Headers)
			{
				IEnumerable<string> enumerable = keyValuePair.Value;
				if (keyValuePair.Key == "Host")
				{
					httpWebRequest.Host = request.Headers.Host;
				}
				else
				{
					if (keyValuePair.Key == "Transfer-Encoding")
					{
						enumerable = enumerable.Where<string>((string l) => l != "chunked");
					}
					string singleHeaderString = PlatformHelper.GetSingleHeaderString(keyValuePair.Key, enumerable);
					if (singleHeaderString != null)
					{
						headers.AddInternal(keyValuePair.Key, singleHeaderString);
					}
				}
			}
			return httpWebRequest;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000327C File Offset: 0x0000147C
		private HttpResponseMessage CreateResponseMessage(HttpWebResponse wr, HttpRequestMessage requestMessage, CancellationToken cancellationToken)
		{
			HttpResponseMessage httpResponseMessage = new HttpResponseMessage(wr.StatusCode);
			httpResponseMessage.RequestMessage = requestMessage;
			httpResponseMessage.ReasonPhrase = wr.StatusDescription;
			httpResponseMessage.Content = PlatformHelper.CreateStreamContent(wr.GetResponseStream(), cancellationToken);
			WebHeaderCollection headers = wr.Headers;
			for (int i = 0; i < headers.Count; i++)
			{
				string key = headers.GetKey(i);
				string[] values = headers.GetValues(i);
				HttpHeaders httpHeaders;
				if (PlatformHelper.IsContentHeader(key))
				{
					httpHeaders = httpResponseMessage.Content.Headers;
				}
				else
				{
					httpHeaders = httpResponseMessage.Headers;
				}
				httpHeaders.TryAddWithoutValidation(key, values);
			}
			requestMessage.RequestUri = wr.ResponseUri;
			return httpResponseMessage;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000331C File Offset: 0x0000151C
		private static bool MethodHasBody(HttpMethod method)
		{
			string method2 = method.Method;
			return !(method2 == "HEAD") && !(method2 == "GET") && !(method2 == "MKCOL") && !(method2 == "CONNECT") && !(method2 == "TRACE");
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003374 File Offset: 0x00001574
		public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().ToString());
			}
			FieldInfo fieldInfo = typeof(CancellationToken).GetField("_source", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
			CancellationTokenSource cancellationTokenSource = (CancellationTokenSource)fieldInfo.GetValue(cancellationToken);
			fieldInfo = typeof(CancellationTokenSource).GetField("_timer", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
			Timer timer = (Timer)fieldInfo.GetValue(cancellationTokenSource);
			if (timer != null)
			{
				fieldInfo = typeof(Timer).GetField("due_time_ms", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic);
				this.timeout = new TimeSpan?(TimeSpan.FromMilliseconds((double)((long)fieldInfo.GetValue(timer))));
			}
			Volatile.Write(ref this.sentRequest, true);
			HttpWebRequest wrequest = this.CreateWebRequest(request);
			HttpWebResponse wresponse = null;
			try
			{
				using (cancellationToken.Register(delegate(object l)
				{
					((HttpWebRequest)l).Abort();
				}, wrequest))
				{
					HttpContent content = request.Content;
					if (content != null)
					{
						WebHeaderCollection headers = wrequest.Headers;
						foreach (KeyValuePair<string, IEnumerable<string>> keyValuePair in content.Headers)
						{
							foreach (string text in keyValuePair.Value)
							{
								headers.AddInternal(keyValuePair.Key, text);
							}
						}
						bool? transferEncodingChunked = request.Headers.TransferEncodingChunked;
						bool flag = true;
						if ((transferEncodingChunked.GetValueOrDefault() == flag) & (transferEncodingChunked != null))
						{
							wrequest.SendChunked = true;
						}
						else
						{
							long? contentLength = content.Headers.ContentLength;
							if (contentLength != null)
							{
								wrequest.ContentLength = contentLength.Value;
							}
							else
							{
								if (this.MaxRequestContentBufferSize == 0L)
								{
									throw new InvalidOperationException("The content length of the request content can't be determined. Either set TransferEncodingChunked to true, load content into buffer, or set MaxRequestContentBufferSize.");
								}
								await content.LoadIntoBufferAsync(this.MaxRequestContentBufferSize).ConfigureAwait(false);
								wrequest.ContentLength = content.Headers.ContentLength.Value;
							}
						}
						wrequest.ResendContentFactory = new Func<Stream, Task>(content.CopyToAsync);
						using (Stream stream = await wrequest.GetRequestStreamAsync().ConfigureAwait(false))
						{
							await request.Content.CopyToAsync(stream).ConfigureAwait(false);
						}
						Stream stream = null;
					}
					else if (MonoWebRequestHandler.MethodHasBody(request.Method))
					{
						wrequest.ContentLength = 0L;
					}
					wresponse = (HttpWebResponse)(await wrequest.GetResponseAsync().ConfigureAwait(false));
					content = null;
				}
				CancellationTokenRegistration cancellationTokenRegistration = default(CancellationTokenRegistration);
			}
			catch (WebException ex)
			{
				if (ex.Status != WebExceptionStatus.RequestCanceled)
				{
					throw new HttpRequestException("An error occurred while sending the request", ex);
				}
			}
			catch (IOException ex2)
			{
				throw new HttpRequestException("An error occurred while sending the request", ex2);
			}
			HttpResponseMessage httpResponseMessage;
			if (cancellationToken.IsCancellationRequested)
			{
				TaskCompletionSource<HttpResponseMessage> taskCompletionSource = new TaskCompletionSource<HttpResponseMessage>();
				taskCompletionSource.SetCanceled();
				httpResponseMessage = await taskCompletionSource.Task;
			}
			else
			{
				httpResponseMessage = this.CreateResponseMessage(wresponse, request, cancellationToken);
			}
			return httpResponseMessage;
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x000033C7 File Offset: 0x000015C7
		// (set) Token: 0x060000B1 RID: 177 RVA: 0x000033C7 File Offset: 0x000015C7
		public ICredentials DefaultProxyCredentials
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x000033C7 File Offset: 0x000015C7
		// (set) Token: 0x060000B3 RID: 179 RVA: 0x000033C7 File Offset: 0x000015C7
		public int MaxConnectionsPerServer
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x000033C7 File Offset: 0x000015C7
		public IDictionary<string, object> Properties
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000033CE File Offset: 0x000015CE
		void IMonoHttpClientHandler.SetWebRequestTimeout(TimeSpan timeout)
		{
			this.timeout = new TimeSpan?(timeout);
		}

		// Token: 0x04000020 RID: 32
		private static long groupCounter;

		// Token: 0x04000021 RID: 33
		private bool allowAutoRedirect;

		// Token: 0x04000022 RID: 34
		private DecompressionMethods automaticDecompression;

		// Token: 0x04000023 RID: 35
		private CookieContainer cookieContainer;

		// Token: 0x04000024 RID: 36
		private ICredentials credentials;

		// Token: 0x04000025 RID: 37
		private int maxAutomaticRedirections;

		// Token: 0x04000026 RID: 38
		private long maxRequestContentBufferSize;

		// Token: 0x04000027 RID: 39
		private bool preAuthenticate;

		// Token: 0x04000028 RID: 40
		private IWebProxy proxy;

		// Token: 0x04000029 RID: 41
		private bool useCookies;

		// Token: 0x0400002A RID: 42
		private bool useProxy;

		// Token: 0x0400002B RID: 43
		private SslClientAuthenticationOptions sslOptions;

		// Token: 0x0400002C RID: 44
		private bool allowPipelining;

		// Token: 0x0400002D RID: 45
		private RequestCachePolicy cachePolicy;

		// Token: 0x0400002E RID: 46
		private AuthenticationLevel authenticationLevel;

		// Token: 0x0400002F RID: 47
		private TimeSpan continueTimeout;

		// Token: 0x04000030 RID: 48
		private TokenImpersonationLevel impersonationLevel;

		// Token: 0x04000031 RID: 49
		private int maxResponseHeadersLength;

		// Token: 0x04000032 RID: 50
		private int readWriteTimeout;

		// Token: 0x04000033 RID: 51
		private RemoteCertificateValidationCallback serverCertificateValidationCallback;

		// Token: 0x04000034 RID: 52
		private bool unsafeAuthenticatedConnectionSharing;

		// Token: 0x04000035 RID: 53
		private bool sentRequest;

		// Token: 0x04000036 RID: 54
		private string connectionGroupName;

		// Token: 0x04000037 RID: 55
		private TimeSpan? timeout;

		// Token: 0x04000038 RID: 56
		private bool disposed;
	}
}
