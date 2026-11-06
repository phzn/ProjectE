using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net.Cache;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mono.Net.Security;
using Mono.Security.Interface;
using Unity;

namespace System.Net
{
	/// <summary>Provides an HTTP-specific implementation of the <see cref="T:System.Net.WebRequest" /> class.</summary>
	// Token: 0x02000694 RID: 1684
	[Serializable]
	public class HttpWebRequest : WebRequest, ISerializable
	{
		// Token: 0x06003570 RID: 13680 RVA: 0x000BAD64 File Offset: 0x000B8F64
		static HttpWebRequest()
		{
			NetConfig netConfig = ConfigurationSettings.GetConfig("system.net/settings") as NetConfig;
			if (netConfig != null)
			{
				HttpWebRequest.defaultMaxResponseHeadersLength = netConfig.MaxResponseHeadersLength;
			}
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x000BADA8 File Offset: 0x000B8FA8
		internal HttpWebRequest(Uri uri)
		{
			this.allowAutoRedirect = true;
			this.allowBuffering = true;
			this.contentLength = -1L;
			this.keepAlive = true;
			this.maxAutoRedirect = 50;
			this.mediaType = string.Empty;
			this.method = "GET";
			this.initialMethod = "GET";
			this.pipelined = true;
			this.version = HttpVersion.Version11;
			this.timeout = 100000;
			this.continueTimeout = 350;
			this.locker = new object();
			this.readWriteTimeout = 300000;
			base..ctor();
			this.requestUri = uri;
			this.actualUri = uri;
			this.proxy = WebRequest.InternalDefaultWebProxy;
			this.webHeaders = new WebHeaderCollection(WebHeaderCollectionType.HttpWebRequest);
			this.ThrowOnError = true;
			this.ResetAuthorization();
		}

		// Token: 0x06003572 RID: 13682 RVA: 0x000BAE71 File Offset: 0x000B9071
		internal HttpWebRequest(Uri uri, MobileTlsProvider tlsProvider, MonoTlsSettings settings = null)
			: this(uri)
		{
			this.tlsProvider = tlsProvider;
			this.tlsSettings = settings;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.HttpWebRequest" /> class from the specified instances of the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> classes. This constructor is obsolete.</summary>
		/// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains the information required to serialize the new <see cref="T:System.Net.HttpWebRequest" /> object.</param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object that contains the source and destination of the serialized stream associated with the new <see cref="T:System.Net.HttpWebRequest" /> object.</param>
		// Token: 0x06003573 RID: 13683 RVA: 0x000BAE88 File Offset: 0x000B9088
		[Obsolete("Serialization is obsoleted for this type.  http://go.microsoft.com/fwlink/?linkid=14202")]
		protected HttpWebRequest(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.allowAutoRedirect = true;
			this.allowBuffering = true;
			this.contentLength = -1L;
			this.keepAlive = true;
			this.maxAutoRedirect = 50;
			this.mediaType = string.Empty;
			this.method = "GET";
			this.initialMethod = "GET";
			this.pipelined = true;
			this.version = HttpVersion.Version11;
			this.timeout = 100000;
			this.continueTimeout = 350;
			this.locker = new object();
			this.readWriteTimeout = 300000;
			base..ctor();
			throw new SerializationException();
		}

		// Token: 0x06003574 RID: 13684 RVA: 0x000BAF24 File Offset: 0x000B9124
		private void ResetAuthorization()
		{
			this.auth_state = new HttpWebRequest.AuthorizationState(this, false);
			this.proxy_auth_state = new HttpWebRequest.AuthorizationState(this, true);
		}

		// Token: 0x06003575 RID: 13685 RVA: 0x000BAF40 File Offset: 0x000B9140
		private void SetSpecialHeaders(string HeaderName, string value)
		{
			value = WebHeaderCollection.CheckBadChars(value, true);
			this.webHeaders.RemoveInternal(HeaderName);
			if (value.Length != 0)
			{
				this.webHeaders.AddInternal(HeaderName, value);
			}
		}

		/// <summary>Gets or sets the value of the <see langword="Accept" /> HTTP header.</summary>
		/// <returns>The value of the <see langword="Accept" /> HTTP header. The default value is <see langword="null" />.</returns>
		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x06003576 RID: 13686 RVA: 0x000BAF6C File Offset: 0x000B916C
		// (set) Token: 0x06003577 RID: 13687 RVA: 0x000BAF7E File Offset: 0x000B917E
		public string Accept
		{
			get
			{
				return this.webHeaders["Accept"];
			}
			set
			{
				this.CheckRequestStarted();
				this.SetSpecialHeaders("Accept", value);
			}
		}

		/// <summary>Gets the Uniform Resource Identifier (URI) of the Internet resource that actually responds to the request.</summary>
		/// <returns>A <see cref="T:System.Uri" /> that identifies the Internet resource that actually responds to the request. The default is the URI used by the <see cref="M:System.Net.WebRequest.Create(System.String)" /> method to initialize the request.</returns>
		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x06003578 RID: 13688 RVA: 0x000BAF92 File Offset: 0x000B9192
		// (set) Token: 0x06003579 RID: 13689 RVA: 0x000BAF9A File Offset: 0x000B919A
		public Uri Address
		{
			get
			{
				return this.actualUri;
			}
			internal set
			{
				this.actualUri = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the request should follow redirection responses.</summary>
		/// <returns>
		///   <see langword="true" /> if the request should automatically follow redirection responses from the Internet resource; otherwise, <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x0600357A RID: 13690 RVA: 0x000BAFA3 File Offset: 0x000B91A3
		// (set) Token: 0x0600357B RID: 13691 RVA: 0x000BAFAB File Offset: 0x000B91AB
		public virtual bool AllowAutoRedirect
		{
			get
			{
				return this.allowAutoRedirect;
			}
			set
			{
				this.allowAutoRedirect = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether to buffer the data sent to the Internet resource.</summary>
		/// <returns>
		///   <see langword="true" /> to enable buffering of the data sent to the Internet resource; <see langword="false" /> to disable buffering. The default is <see langword="true" />.</returns>
		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x0600357C RID: 13692 RVA: 0x000BAFB4 File Offset: 0x000B91B4
		// (set) Token: 0x0600357D RID: 13693 RVA: 0x000BAFBC File Offset: 0x000B91BC
		public virtual bool AllowWriteStreamBuffering
		{
			get
			{
				return this.allowBuffering;
			}
			set
			{
				this.allowBuffering = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether to buffer the received from the Internet resource.</summary>
		/// <returns>
		///   <see langword="true" /> to enable buffering of the data received from the Internet resource; <see langword="false" /> to disable buffering. The default is <see langword="false" />.</returns>
		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x0600357E RID: 13694 RVA: 0x000BAFC5 File Offset: 0x000B91C5
		// (set) Token: 0x0600357F RID: 13695 RVA: 0x000BAFCD File Offset: 0x000B91CD
		public virtual bool AllowReadStreamBuffering
		{
			get
			{
				return this.allowReadStreamBuffering;
			}
			set
			{
				this.allowReadStreamBuffering = value;
			}
		}

		// Token: 0x06003580 RID: 13696 RVA: 0x0001FD2F File Offset: 0x0001DF2F
		private static Exception GetMustImplement()
		{
			return new NotImplementedException();
		}

		/// <summary>Gets or sets the type of decompression that is used.</summary>
		/// <returns>A <see cref="T:System.Net.DecompressionMethods" /> object that indicates the type of decompression that is used.</returns>
		/// <exception cref="T:System.InvalidOperationException">The object's current state does not allow this property to be set.</exception>
		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06003581 RID: 13697 RVA: 0x000BAFD6 File Offset: 0x000B91D6
		// (set) Token: 0x06003582 RID: 13698 RVA: 0x000BAFDE File Offset: 0x000B91DE
		public DecompressionMethods AutomaticDecompression
		{
			get
			{
				return this.auto_decomp;
			}
			set
			{
				this.CheckRequestStarted();
				this.auto_decomp = value;
			}
		}

		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x06003583 RID: 13699 RVA: 0x000BAFED File Offset: 0x000B91ED
		internal bool InternalAllowBuffering
		{
			get
			{
				return this.allowBuffering && this.MethodWithBuffer;
			}
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x06003584 RID: 13700 RVA: 0x000BB000 File Offset: 0x000B9200
		private bool MethodWithBuffer
		{
			get
			{
				return this.method != "HEAD" && this.method != "GET" && this.method != "MKCOL" && this.method != "CONNECT" && this.method != "TRACE";
			}
		}

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06003585 RID: 13701 RVA: 0x000BB067 File Offset: 0x000B9267
		internal MobileTlsProvider TlsProvider
		{
			get
			{
				return this.tlsProvider;
			}
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06003586 RID: 13702 RVA: 0x000BB06F File Offset: 0x000B926F
		internal MonoTlsSettings TlsSettings
		{
			get
			{
				return this.tlsSettings;
			}
		}

		/// <summary>Gets or sets the collection of security certificates that are associated with this request.</summary>
		/// <returns>The <see cref="T:System.Security.Cryptography.X509Certificates.X509CertificateCollection" /> that contains the security certificates associated with this request.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value specified for a set operation is <see langword="null" />.</exception>
		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06003587 RID: 13703 RVA: 0x000BB077 File Offset: 0x000B9277
		// (set) Token: 0x06003588 RID: 13704 RVA: 0x000BB092 File Offset: 0x000B9292
		public X509CertificateCollection ClientCertificates
		{
			get
			{
				if (this.certificates == null)
				{
					this.certificates = new X509CertificateCollection();
				}
				return this.certificates;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.certificates = value;
			}
		}

		/// <summary>Gets or sets the value of the <see langword="Connection" /> HTTP header.</summary>
		/// <returns>The value of the <see langword="Connection" /> HTTP header. The default value is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentException">The value of <see cref="P:System.Net.HttpWebRequest.Connection" /> is set to Keep-alive or Close.</exception>
		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06003589 RID: 13705 RVA: 0x000BB0A9 File Offset: 0x000B92A9
		// (set) Token: 0x0600358A RID: 13706 RVA: 0x000BB0BC File Offset: 0x000B92BC
		public string Connection
		{
			get
			{
				return this.webHeaders["Connection"];
			}
			set
			{
				this.CheckRequestStarted();
				if (string.IsNullOrWhiteSpace(value))
				{
					this.webHeaders.RemoveInternal("Connection");
					return;
				}
				string text = value.ToLowerInvariant();
				if (text.Contains("keep-alive") || text.Contains("close"))
				{
					throw new ArgumentException("Keep-Alive and Close may not be set using this property.", "value");
				}
				string text2 = HttpValidationHelpers.CheckBadHeaderValueChars(value);
				this.webHeaders.CheckUpdate("Connection", text2);
			}
		}

		/// <summary>Gets or sets the name of the connection group for the request.</summary>
		/// <returns>The name of the connection group for this request. The default value is <see langword="null" />.</returns>
		// Token: 0x17000AF6 RID: 2806
		// (get) Token: 0x0600358B RID: 13707 RVA: 0x000BB131 File Offset: 0x000B9331
		// (set) Token: 0x0600358C RID: 13708 RVA: 0x000BB139 File Offset: 0x000B9339
		public override string ConnectionGroupName
		{
			get
			{
				return this.connectionGroup;
			}
			set
			{
				this.connectionGroup = value;
			}
		}

		/// <summary>Gets or sets the <see langword="Content-length" /> HTTP header.</summary>
		/// <returns>The number of bytes of data to send to the Internet resource. The default is -1, which indicates the property has not been set and that there is no request data to send.</returns>
		/// <exception cref="T:System.InvalidOperationException">The request has been started by calling the <see cref="M:System.Net.HttpWebRequest.GetRequestStream" />, <see cref="M:System.Net.HttpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />, <see cref="M:System.Net.HttpWebRequest.GetResponse" />, or <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" /> method.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The new <see cref="P:System.Net.HttpWebRequest.ContentLength" /> value is less than 0.</exception>
		// Token: 0x17000AF7 RID: 2807
		// (get) Token: 0x0600358D RID: 13709 RVA: 0x000BB142 File Offset: 0x000B9342
		// (set) Token: 0x0600358E RID: 13710 RVA: 0x000BB14A File Offset: 0x000B934A
		public override long ContentLength
		{
			get
			{
				return this.contentLength;
			}
			set
			{
				this.CheckRequestStarted();
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value", "Content-Length must be >= 0");
				}
				this.contentLength = value;
				this.haveContentLength = true;
			}
		}

		// Token: 0x17000AF8 RID: 2808
		// (set) Token: 0x0600358F RID: 13711 RVA: 0x000BB175 File Offset: 0x000B9375
		internal long InternalContentLength
		{
			set
			{
				this.contentLength = value;
			}
		}

		// Token: 0x17000AF9 RID: 2809
		// (get) Token: 0x06003590 RID: 13712 RVA: 0x000BB17E File Offset: 0x000B937E
		// (set) Token: 0x06003591 RID: 13713 RVA: 0x000BB186 File Offset: 0x000B9386
		internal bool ThrowOnError { get; set; }

		/// <summary>Gets or sets the value of the <see langword="Content-type" /> HTTP header.</summary>
		/// <returns>The value of the <see langword="Content-type" /> HTTP header. The default value is <see langword="null" />.</returns>
		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x06003592 RID: 13714 RVA: 0x000BB18F File Offset: 0x000B938F
		// (set) Token: 0x06003593 RID: 13715 RVA: 0x000BB1A1 File Offset: 0x000B93A1
		public override string ContentType
		{
			get
			{
				return this.webHeaders["Content-Type"];
			}
			set
			{
				this.SetSpecialHeaders("Content-Type", value);
			}
		}

		/// <summary>Gets or sets the delegate method called when an HTTP 100-continue response is received from the Internet resource.</summary>
		/// <returns>A delegate that implements the callback method that executes when an HTTP Continue response is returned from the Internet resource. The default value is <see langword="null" />.</returns>
		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x06003594 RID: 13716 RVA: 0x000BB1AF File Offset: 0x000B93AF
		// (set) Token: 0x06003595 RID: 13717 RVA: 0x000BB1B7 File Offset: 0x000B93B7
		public HttpContinueDelegate ContinueDelegate
		{
			get
			{
				return this.continueDelegate;
			}
			set
			{
				this.continueDelegate = value;
			}
		}

		/// <summary>Gets or sets the cookies associated with the request.</summary>
		/// <returns>A <see cref="T:System.Net.CookieContainer" /> that contains the cookies associated with this request.</returns>
		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x06003596 RID: 13718 RVA: 0x000BB1C0 File Offset: 0x000B93C0
		// (set) Token: 0x06003597 RID: 13719 RVA: 0x000BB1C8 File Offset: 0x000B93C8
		public virtual CookieContainer CookieContainer
		{
			get
			{
				return this.cookieContainer;
			}
			set
			{
				this.cookieContainer = value;
			}
		}

		/// <summary>Gets or sets authentication information for the request.</summary>
		/// <returns>An <see cref="T:System.Net.ICredentials" /> that contains the authentication credentials associated with the request. The default is <see langword="null" />.</returns>
		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x06003598 RID: 13720 RVA: 0x000BB1D1 File Offset: 0x000B93D1
		// (set) Token: 0x06003599 RID: 13721 RVA: 0x000BB1D9 File Offset: 0x000B93D9
		public override ICredentials Credentials
		{
			get
			{
				return this.credentials;
			}
			set
			{
				this.credentials = value;
			}
		}

		/// <summary>Gets or sets the <see langword="Date" /> HTTP header value to use in an HTTP request.</summary>
		/// <returns>The Date header value in the HTTP request.</returns>
		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x0600359A RID: 13722 RVA: 0x000BB1E4 File Offset: 0x000B93E4
		// (set) Token: 0x0600359B RID: 13723 RVA: 0x000BB223 File Offset: 0x000B9423
		public DateTime Date
		{
			get
			{
				string text = this.webHeaders["Date"];
				if (text == null)
				{
					return DateTime.MinValue;
				}
				return DateTime.ParseExact(text, "r", CultureInfo.InvariantCulture).ToLocalTime();
			}
			set
			{
				this.SetDateHeaderHelper("Date", value);
			}
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x000BB231 File Offset: 0x000B9431
		private void SetDateHeaderHelper(string headerName, DateTime dateTime)
		{
			if (dateTime == DateTime.MinValue)
			{
				this.SetSpecialHeaders(headerName, null);
				return;
			}
			this.SetSpecialHeaders(headerName, HttpProtocolUtils.date2string(dateTime));
		}

		/// <summary>Gets or sets the default cache policy for this request.</summary>
		/// <returns>A <see cref="T:System.Net.Cache.HttpRequestCachePolicy" /> that specifies the cache policy in effect for this request when no other policy is applicable.</returns>
		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x0600359D RID: 13725 RVA: 0x000BB256 File Offset: 0x000B9456
		// (set) Token: 0x0600359E RID: 13726 RVA: 0x000BB25D File Offset: 0x000B945D
		[MonoTODO]
		public new static RequestCachePolicy DefaultCachePolicy
		{
			get
			{
				return HttpWebRequest.defaultCachePolicy;
			}
			set
			{
				HttpWebRequest.defaultCachePolicy = value;
			}
		}

		/// <summary>Gets or sets the default maximum length of an HTTP error response.</summary>
		/// <returns>The default maximum length of an HTTP error response.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than 0 and is not equal to -1.</exception>
		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x0600359F RID: 13727 RVA: 0x000BB265 File Offset: 0x000B9465
		// (set) Token: 0x060035A0 RID: 13728 RVA: 0x000BB26C File Offset: 0x000B946C
		[MonoTODO]
		public static int DefaultMaximumErrorResponseLength
		{
			get
			{
				return HttpWebRequest.defaultMaximumErrorResponseLength;
			}
			set
			{
				HttpWebRequest.defaultMaximumErrorResponseLength = value;
			}
		}

		/// <summary>Gets or sets the value of the <see langword="Expect" /> HTTP header.</summary>
		/// <returns>The contents of the <see langword="Expect" /> HTTP header. The default value is <see langword="null" />.  
		///
		///  The value for this property is stored in <see cref="T:System.Net.WebHeaderCollection" />. If WebHeaderCollection is set, the property value is lost.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <see langword="Expect" /> is set to a string that contains "100-continue" as a substring.</exception>
		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x060035A1 RID: 13729 RVA: 0x000BB274 File Offset: 0x000B9474
		// (set) Token: 0x060035A2 RID: 13730 RVA: 0x000BB288 File Offset: 0x000B9488
		public string Expect
		{
			get
			{
				return this.webHeaders["Expect"];
			}
			set
			{
				this.CheckRequestStarted();
				string text = value;
				if (text != null)
				{
					text = text.Trim().ToLower();
				}
				if (text == null || text.Length == 0)
				{
					this.webHeaders.RemoveInternal("Expect");
					return;
				}
				if (text == "100-continue")
				{
					throw new ArgumentException("100-Continue cannot be set with this property.", "value");
				}
				this.webHeaders.CheckUpdate("Expect", value);
			}
		}

		/// <summary>Gets a value that indicates whether a response has been received from an Internet resource.</summary>
		/// <returns>
		///   <see langword="true" /> if a response has been received; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x060035A3 RID: 13731 RVA: 0x000BB2F6 File Offset: 0x000B94F6
		public virtual bool HaveResponse
		{
			get
			{
				return this.haveResponse;
			}
		}

		/// <summary>Specifies a collection of the name/value pairs that make up the HTTP headers.</summary>
		/// <returns>A <see cref="T:System.Net.WebHeaderCollection" /> that contains the name/value pairs that make up the headers for the HTTP request.</returns>
		/// <exception cref="T:System.InvalidOperationException">The request has been started by calling the <see cref="M:System.Net.HttpWebRequest.GetRequestStream" />, <see cref="M:System.Net.HttpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />, <see cref="M:System.Net.HttpWebRequest.GetResponse" />, or <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" /> method.</exception>
		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x060035A4 RID: 13732 RVA: 0x000BB2FE File Offset: 0x000B94FE
		// (set) Token: 0x060035A5 RID: 13733 RVA: 0x000BB308 File Offset: 0x000B9508
		public override WebHeaderCollection Headers
		{
			get
			{
				return this.webHeaders;
			}
			set
			{
				this.CheckRequestStarted();
				WebHeaderCollection webHeaderCollection = new WebHeaderCollection(WebHeaderCollectionType.HttpWebRequest);
				foreach (string text in value.AllKeys)
				{
					webHeaderCollection.Add(text, value[text]);
				}
				this.webHeaders = webHeaderCollection;
			}
		}

		/// <summary>Gets or sets the Host header value to use in an HTTP request independent from the request URI.</summary>
		/// <returns>The Host header value in the HTTP request.</returns>
		/// <exception cref="T:System.ArgumentNullException">The Host header cannot be set to <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The Host header cannot be set to an invalid value.</exception>
		/// <exception cref="T:System.InvalidOperationException">The Host header cannot be set after the <see cref="T:System.Net.HttpWebRequest" /> has already started to be sent.</exception>
		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x060035A6 RID: 13734 RVA: 0x000BB358 File Offset: 0x000B9558
		// (set) Token: 0x060035A7 RID: 13735 RVA: 0x000BB3C0 File Offset: 0x000B95C0
		public string Host
		{
			get
			{
				Uri uri = this.hostUri ?? this.Address;
				if ((!(this.hostUri == null) && this.hostHasPort) || !this.Address.IsDefaultPort)
				{
					return uri.Host + ":" + uri.Port.ToString();
				}
				return uri.Host;
			}
			set
			{
				this.CheckRequestStarted();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				Uri uri;
				if (value.IndexOf('/') != -1 || !this.TryGetHostUri(value, out uri))
				{
					throw new ArgumentException("The specified value is not a valid Host header string.", "value");
				}
				this.hostUri = uri;
				if (!this.hostUri.IsDefaultPort)
				{
					this.hostHasPort = true;
					return;
				}
				if (value.IndexOf(':') == -1)
				{
					this.hostHasPort = false;
					return;
				}
				int num = value.IndexOf(']');
				this.hostHasPort = num == -1 || value.LastIndexOf(':') > num;
			}
		}

		// Token: 0x060035A8 RID: 13736 RVA: 0x000BB457 File Offset: 0x000B9657
		private bool TryGetHostUri(string hostName, out Uri hostUri)
		{
			return Uri.TryCreate(this.Address.Scheme + "://" + hostName + this.Address.PathAndQuery, UriKind.Absolute, out hostUri);
		}

		/// <summary>Gets or sets the value of the <see langword="If-Modified-Since" /> HTTP header.</summary>
		/// <returns>A <see cref="T:System.DateTime" /> that contains the contents of the <see langword="If-Modified-Since" /> HTTP header. The default value is the current date and time.</returns>
		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x060035A9 RID: 13737 RVA: 0x000BB484 File Offset: 0x000B9684
		// (set) Token: 0x060035AA RID: 13738 RVA: 0x000BB4D0 File Offset: 0x000B96D0
		public DateTime IfModifiedSince
		{
			get
			{
				string text = this.webHeaders["If-Modified-Since"];
				if (text == null)
				{
					return DateTime.Now;
				}
				DateTime dateTime;
				try
				{
					dateTime = MonoHttpDate.Parse(text);
				}
				catch (Exception)
				{
					dateTime = DateTime.Now;
				}
				return dateTime;
			}
			set
			{
				this.CheckRequestStarted();
				this.webHeaders.SetInternal("If-Modified-Since", value.ToUniversalTime().ToString("r", null));
			}
		}

		/// <summary>Gets or sets a value that indicates whether to make a persistent connection to the Internet resource.</summary>
		/// <returns>
		///   <see langword="true" /> if the request to the Internet resource should contain a <see langword="Connection" /> HTTP header with the value Keep-alive; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x060035AB RID: 13739 RVA: 0x000BB508 File Offset: 0x000B9708
		// (set) Token: 0x060035AC RID: 13740 RVA: 0x000BB510 File Offset: 0x000B9710
		public bool KeepAlive
		{
			get
			{
				return this.keepAlive;
			}
			set
			{
				this.keepAlive = value;
			}
		}

		/// <summary>Gets or sets the maximum number of redirects that the request follows.</summary>
		/// <returns>The maximum number of redirection responses that the request follows. The default value is 50.</returns>
		/// <exception cref="T:System.ArgumentException">The value is set to 0 or less.</exception>
		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x060035AD RID: 13741 RVA: 0x000BB519 File Offset: 0x000B9719
		// (set) Token: 0x060035AE RID: 13742 RVA: 0x000BB521 File Offset: 0x000B9721
		public int MaximumAutomaticRedirections
		{
			get
			{
				return this.maxAutoRedirect;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentException("Must be > 0", "value");
				}
				this.maxAutoRedirect = value;
			}
		}

		/// <summary>Gets or sets the maximum allowed length of the response headers.</summary>
		/// <returns>The length, in kilobytes (1024 bytes), of the response headers.</returns>
		/// <exception cref="T:System.InvalidOperationException">The property is set after the request has already been submitted.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value is less than 0 and is not equal to -1.</exception>
		// Token: 0x17000B08 RID: 2824
		// (get) Token: 0x060035AF RID: 13743 RVA: 0x000BB53E File Offset: 0x000B973E
		// (set) Token: 0x060035B0 RID: 13744 RVA: 0x000BB546 File Offset: 0x000B9746
		[MonoTODO("Use this")]
		public int MaximumResponseHeadersLength
		{
			get
			{
				return this.maxResponseHeadersLength;
			}
			set
			{
				this.CheckRequestStarted();
				if (value < 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException("value", "The specified value must be greater than 0.");
				}
				this.maxResponseHeadersLength = value;
			}
		}

		/// <summary>Gets or sets the default for the <see cref="P:System.Net.HttpWebRequest.MaximumResponseHeadersLength" /> property.</summary>
		/// <returns>The length, in kilobytes (1024 bytes), of the default maximum for response headers received. The default configuration file sets this value to 64 kilobytes.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value is not equal to -1 and is less than zero.</exception>
		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x060035B1 RID: 13745 RVA: 0x000BB56D File Offset: 0x000B976D
		// (set) Token: 0x060035B2 RID: 13746 RVA: 0x000BB574 File Offset: 0x000B9774
		[MonoTODO("Use this")]
		public static int DefaultMaximumResponseHeadersLength
		{
			get
			{
				return HttpWebRequest.defaultMaxResponseHeadersLength;
			}
			set
			{
				HttpWebRequest.defaultMaxResponseHeadersLength = value;
			}
		}

		/// <summary>Gets or sets a time-out in milliseconds when writing to or reading from a stream.</summary>
		/// <returns>The number of milliseconds before the writing or reading times out. The default value is 300,000 milliseconds (5 minutes).</returns>
		/// <exception cref="T:System.InvalidOperationException">The request has already been sent.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified for a set operation is less than or equal to zero and is not equal to <see cref="F:System.Threading.Timeout.Infinite" /></exception>
		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x060035B3 RID: 13747 RVA: 0x000BB57C File Offset: 0x000B977C
		// (set) Token: 0x060035B4 RID: 13748 RVA: 0x000BB584 File Offset: 0x000B9784
		public int ReadWriteTimeout
		{
			get
			{
				return this.readWriteTimeout;
			}
			set
			{
				this.CheckRequestStarted();
				if (value <= 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException("value", "Timeout can be only be set to 'System.Threading.Timeout.Infinite' or a value > 0.");
				}
				this.readWriteTimeout = value;
			}
		}

		/// <summary>Gets or sets a timeout, in milliseconds, to wait until the 100-Continue is received from the server.</summary>
		/// <returns>The timeout, in milliseconds, to wait until the 100-Continue is received.</returns>
		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x060035B5 RID: 13749 RVA: 0x000BB5AB File Offset: 0x000B97AB
		// (set) Token: 0x060035B6 RID: 13750 RVA: 0x000BB5B3 File Offset: 0x000B97B3
		[MonoTODO]
		public int ContinueTimeout
		{
			get
			{
				return this.continueTimeout;
			}
			set
			{
				this.CheckRequestStarted();
				if (value < 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException("value", "Timeout can be only be set to 'System.Threading.Timeout.Infinite' or a value >= 0.");
				}
				this.continueTimeout = value;
			}
		}

		/// <summary>Gets or sets the media type of the request.</summary>
		/// <returns>The media type of the request. The default value is <see langword="null" />.</returns>
		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x060035B7 RID: 13751 RVA: 0x000BB5DA File Offset: 0x000B97DA
		// (set) Token: 0x060035B8 RID: 13752 RVA: 0x000BB5E2 File Offset: 0x000B97E2
		public string MediaType
		{
			get
			{
				return this.mediaType;
			}
			set
			{
				this.mediaType = value;
			}
		}

		/// <summary>Gets or sets the method for the request.</summary>
		/// <returns>The request method to use to contact the Internet resource. The default value is GET.</returns>
		/// <exception cref="T:System.ArgumentException">No method is supplied.  
		///  -or-  
		///  The method string contains invalid characters.</exception>
		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x060035B9 RID: 13753 RVA: 0x000BB5EB File Offset: 0x000B97EB
		// (set) Token: 0x060035BA RID: 13754 RVA: 0x000BB5F4 File Offset: 0x000B97F4
		public override string Method
		{
			get
			{
				return this.method;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentException("Cannot set null or blank methods on request.", "value");
				}
				if (HttpValidationHelpers.IsInvalidMethodOrHeaderString(value))
				{
					throw new ArgumentException("Cannot set null or blank methods on request.", "value");
				}
				this.method = value.ToUpperInvariant();
				if (this.method != "HEAD" && this.method != "GET" && this.method != "POST" && this.method != "PUT" && this.method != "DELETE" && this.method != "CONNECT" && this.method != "TRACE" && this.method != "MKCOL")
				{
					this.method = value;
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether to pipeline the request to the Internet resource.</summary>
		/// <returns>
		///   <see langword="true" /> if the request should be pipelined; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x060035BB RID: 13755 RVA: 0x000BB6D7 File Offset: 0x000B98D7
		// (set) Token: 0x060035BC RID: 13756 RVA: 0x000BB6DF File Offset: 0x000B98DF
		public bool Pipelined
		{
			get
			{
				return this.pipelined;
			}
			set
			{
				this.pipelined = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether to send an Authorization header with the request.</summary>
		/// <returns>
		///   <see langword="true" /> to send an  HTTP Authorization header with requests after authentication has taken place; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x060035BD RID: 13757 RVA: 0x000BB6E8 File Offset: 0x000B98E8
		// (set) Token: 0x060035BE RID: 13758 RVA: 0x000BB6F0 File Offset: 0x000B98F0
		public override bool PreAuthenticate
		{
			get
			{
				return this.preAuthenticate;
			}
			set
			{
				this.preAuthenticate = value;
			}
		}

		/// <summary>Gets or sets the version of HTTP to use for the request.</summary>
		/// <returns>The HTTP version to use for the request. The default is <see cref="F:System.Net.HttpVersion.Version11" />.</returns>
		/// <exception cref="T:System.ArgumentException">The HTTP version is set to a value other than 1.0 or 1.1.</exception>
		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x060035BF RID: 13759 RVA: 0x000BB6F9 File Offset: 0x000B98F9
		// (set) Token: 0x060035C0 RID: 13760 RVA: 0x000BB701 File Offset: 0x000B9901
		public Version ProtocolVersion
		{
			get
			{
				return this.version;
			}
			set
			{
				if (value != HttpVersion.Version10 && value != HttpVersion.Version11)
				{
					throw new ArgumentException("Only HTTP/1.0 and HTTP/1.1 version requests are currently supported.", "value");
				}
				this.force_version = true;
				this.version = value;
			}
		}

		/// <summary>Gets or sets proxy information for the request.</summary>
		/// <returns>The <see cref="T:System.Net.IWebProxy" /> object to use to proxy the request. The default value is set by calling the <see cref="P:System.Net.GlobalProxySelection.Select" /> property.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <see cref="P:System.Net.HttpWebRequest.Proxy" /> is set to <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The request has been started by calling <see cref="M:System.Net.HttpWebRequest.GetRequestStream" />, <see cref="M:System.Net.HttpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />, <see cref="M:System.Net.HttpWebRequest.GetResponse" />, or <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" />.</exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have permission for the requested operation.</exception>
		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x060035C1 RID: 13761 RVA: 0x000BB73B File Offset: 0x000B993B
		// (set) Token: 0x060035C2 RID: 13762 RVA: 0x000BB743 File Offset: 0x000B9943
		public override IWebProxy Proxy
		{
			get
			{
				return this.proxy;
			}
			set
			{
				this.CheckRequestStarted();
				this.proxy = value;
				this.servicePoint = null;
				this.GetServicePoint();
			}
		}

		/// <summary>Gets or sets the value of the <see langword="Referer" /> HTTP header.</summary>
		/// <returns>The value of the <see langword="Referer" /> HTTP header. The default value is <see langword="null" />.</returns>
		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x060035C3 RID: 13763 RVA: 0x000BB760 File Offset: 0x000B9960
		// (set) Token: 0x060035C4 RID: 13764 RVA: 0x000BB772 File Offset: 0x000B9972
		public string Referer
		{
			get
			{
				return this.webHeaders["Referer"];
			}
			set
			{
				this.CheckRequestStarted();
				if (value == null || value.Trim().Length == 0)
				{
					this.webHeaders.RemoveInternal("Referer");
					return;
				}
				this.webHeaders.SetInternal("Referer", value);
			}
		}

		/// <summary>Gets the original Uniform Resource Identifier (URI) of the request.</summary>
		/// <returns>A <see cref="T:System.Uri" /> that contains the URI of the Internet resource passed to the <see cref="M:System.Net.WebRequest.Create(System.String)" /> method.</returns>
		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x060035C5 RID: 13765 RVA: 0x000BB7AC File Offset: 0x000B99AC
		public override Uri RequestUri
		{
			get
			{
				return this.requestUri;
			}
		}

		/// <summary>Gets or sets a value that indicates whether to send data in segments to the Internet resource.</summary>
		/// <returns>
		///   <see langword="true" /> to send data to the Internet resource in segments; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The request has been started by calling the <see cref="M:System.Net.HttpWebRequest.GetRequestStream" />, <see cref="M:System.Net.HttpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />, <see cref="M:System.Net.HttpWebRequest.GetResponse" />, or <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" /> method.</exception>
		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x060035C6 RID: 13766 RVA: 0x000BB7B4 File Offset: 0x000B99B4
		// (set) Token: 0x060035C7 RID: 13767 RVA: 0x000BB7BC File Offset: 0x000B99BC
		public bool SendChunked
		{
			get
			{
				return this.sendChunked;
			}
			set
			{
				this.CheckRequestStarted();
				this.sendChunked = value;
			}
		}

		/// <summary>Gets the service point to use for the request.</summary>
		/// <returns>A <see cref="T:System.Net.ServicePoint" /> that represents the network connection to the Internet resource.</returns>
		// Token: 0x17000B15 RID: 2837
		// (get) Token: 0x060035C8 RID: 13768 RVA: 0x000BB7CB File Offset: 0x000B99CB
		public ServicePoint ServicePoint
		{
			get
			{
				return this.GetServicePoint();
			}
		}

		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x060035C9 RID: 13769 RVA: 0x000BB7D3 File Offset: 0x000B99D3
		internal ServicePoint ServicePointNoLock
		{
			get
			{
				return this.servicePoint;
			}
		}

		/// <summary>Gets a value that indicates whether the request provides support for a <see cref="T:System.Net.CookieContainer" />.</summary>
		/// <returns>
		///   <see langword="true" /> if the request provides support for a <see cref="T:System.Net.CookieContainer" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000B17 RID: 2839
		// (get) Token: 0x060035CA RID: 13770 RVA: 0x0000390E File Offset: 0x00001B0E
		public virtual bool SupportsCookieContainer
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets or sets the time-out value in milliseconds for the <see cref="M:System.Net.HttpWebRequest.GetResponse" /> and <see cref="M:System.Net.HttpWebRequest.GetRequestStream" /> methods.</summary>
		/// <returns>The number of milliseconds to wait before the request times out. The default value is 100,000 milliseconds (100 seconds).</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified is less than zero and is not <see cref="F:System.Threading.Timeout.Infinite" />.</exception>
		// Token: 0x17000B18 RID: 2840
		// (get) Token: 0x060035CB RID: 13771 RVA: 0x000BB7DB File Offset: 0x000B99DB
		// (set) Token: 0x060035CC RID: 13772 RVA: 0x000BB7E3 File Offset: 0x000B99E3
		public override int Timeout
		{
			get
			{
				return this.timeout;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.timeout = value;
			}
		}

		/// <summary>Gets or sets the value of the <see langword="Transfer-encoding" /> HTTP header.</summary>
		/// <returns>The value of the <see langword="Transfer-encoding" /> HTTP header. The default value is <see langword="null" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="P:System.Net.HttpWebRequest.TransferEncoding" /> is set when <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is <see langword="false" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <see cref="P:System.Net.HttpWebRequest.TransferEncoding" /> is set to the value "Chunked".</exception>
		// Token: 0x17000B19 RID: 2841
		// (get) Token: 0x060035CD RID: 13773 RVA: 0x000BB7FB File Offset: 0x000B99FB
		// (set) Token: 0x060035CE RID: 13774 RVA: 0x000BB810 File Offset: 0x000B9A10
		public string TransferEncoding
		{
			get
			{
				return this.webHeaders["Transfer-Encoding"];
			}
			set
			{
				this.CheckRequestStarted();
				if (string.IsNullOrWhiteSpace(value))
				{
					this.webHeaders.RemoveInternal("Transfer-Encoding");
					return;
				}
				if (value.ToLower().Contains("chunked"))
				{
					throw new ArgumentException("Chunked encoding must be set via the SendChunked property.", "value");
				}
				if (!this.SendChunked)
				{
					throw new InvalidOperationException("TransferEncoding requires the SendChunked property to be set to true.");
				}
				string text = HttpValidationHelpers.CheckBadHeaderValueChars(value);
				this.webHeaders.CheckUpdate("Transfer-Encoding", text);
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that controls whether default credentials are sent with requests.</summary>
		/// <returns>
		///   <see langword="true" /> if the default credentials are used; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">You attempted to set this property after the request was sent.</exception>
		// Token: 0x17000B1A RID: 2842
		// (get) Token: 0x060035CF RID: 13775 RVA: 0x000BB889 File Offset: 0x000B9A89
		// (set) Token: 0x060035D0 RID: 13776 RVA: 0x000BB898 File Offset: 0x000B9A98
		public override bool UseDefaultCredentials
		{
			get
			{
				return CredentialCache.DefaultCredentials == this.Credentials;
			}
			set
			{
				this.Credentials = (value ? CredentialCache.DefaultCredentials : null);
			}
		}

		/// <summary>Gets or sets the value of the <see langword="User-agent" /> HTTP header.</summary>
		/// <returns>The value of the <see langword="User-agent" /> HTTP header. The default value is <see langword="null" />.  
		///
		///  The value for this property is stored in <see cref="T:System.Net.WebHeaderCollection" />. If WebHeaderCollection is set, the property value is lost.</returns>
		// Token: 0x17000B1B RID: 2843
		// (get) Token: 0x060035D1 RID: 13777 RVA: 0x000BB8AB File Offset: 0x000B9AAB
		// (set) Token: 0x060035D2 RID: 13778 RVA: 0x000BB8BD File Offset: 0x000B9ABD
		public string UserAgent
		{
			get
			{
				return this.webHeaders["User-Agent"];
			}
			set
			{
				this.webHeaders.SetInternal("User-Agent", value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether to allow high-speed NTLM-authenticated connection sharing.</summary>
		/// <returns>
		///   <see langword="true" /> to keep the authenticated connection open; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000B1C RID: 2844
		// (get) Token: 0x060035D3 RID: 13779 RVA: 0x000BB8D0 File Offset: 0x000B9AD0
		// (set) Token: 0x060035D4 RID: 13780 RVA: 0x000BB8D8 File Offset: 0x000B9AD8
		public bool UnsafeAuthenticatedConnectionSharing
		{
			get
			{
				return this.unsafe_auth_blah;
			}
			set
			{
				this.unsafe_auth_blah = value;
			}
		}

		// Token: 0x17000B1D RID: 2845
		// (get) Token: 0x060035D5 RID: 13781 RVA: 0x000BB8E1 File Offset: 0x000B9AE1
		internal bool GotRequestStream
		{
			get
			{
				return this.gotRequestStream;
			}
		}

		// Token: 0x17000B1E RID: 2846
		// (get) Token: 0x060035D6 RID: 13782 RVA: 0x000BB8E9 File Offset: 0x000B9AE9
		// (set) Token: 0x060035D7 RID: 13783 RVA: 0x000BB8F1 File Offset: 0x000B9AF1
		internal bool ExpectContinue
		{
			get
			{
				return this.expectContinue;
			}
			set
			{
				this.expectContinue = value;
			}
		}

		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x060035D8 RID: 13784 RVA: 0x000BAF92 File Offset: 0x000B9192
		internal Uri AuthUri
		{
			get
			{
				return this.actualUri;
			}
		}

		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x060035D9 RID: 13785 RVA: 0x000BB8FA File Offset: 0x000B9AFA
		internal bool ProxyQuery
		{
			get
			{
				return this.servicePoint.UsesProxy && !this.servicePoint.UseConnect;
			}
		}

		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x060035DA RID: 13786 RVA: 0x000BB919 File Offset: 0x000B9B19
		internal ServerCertValidationCallback ServerCertValidationCallback
		{
			get
			{
				return this.certValidationCallback;
			}
		}

		/// <summary>Gets or sets a callback function to validate the server certificate.</summary>
		/// <returns>A callback function to validate the server certificate.</returns>
		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x060035DB RID: 13787 RVA: 0x000BB921 File Offset: 0x000B9B21
		// (set) Token: 0x060035DC RID: 13788 RVA: 0x000BB938 File Offset: 0x000B9B38
		public RemoteCertificateValidationCallback ServerCertificateValidationCallback
		{
			get
			{
				if (this.certValidationCallback == null)
				{
					return null;
				}
				return this.certValidationCallback.ValidationCallback;
			}
			set
			{
				if (value == null)
				{
					this.certValidationCallback = null;
					return;
				}
				this.certValidationCallback = new ServerCertValidationCallback(value);
			}
		}

		// Token: 0x060035DD RID: 13789 RVA: 0x000BB954 File Offset: 0x000B9B54
		internal ServicePoint GetServicePoint()
		{
			object obj = this.locker;
			lock (obj)
			{
				if (this.hostChanged || this.servicePoint == null)
				{
					this.servicePoint = ServicePointManager.FindServicePoint(this.actualUri, this.proxy);
					this.hostChanged = false;
				}
			}
			return this.servicePoint;
		}

		/// <summary>Adds a byte range header to a request for a specific range from the beginning or end of the requested data.</summary>
		/// <param name="range">The starting or ending point of the range.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="rangeSpecifier" /> is invalid.</exception>
		/// <exception cref="T:System.InvalidOperationException">The range header could not be added.</exception>
		// Token: 0x060035DE RID: 13790 RVA: 0x000BB9C4 File Offset: 0x000B9BC4
		public void AddRange(int range)
		{
			this.AddRange("bytes", (long)range);
		}

		/// <summary>Adds a byte range header to the request for a specified range.</summary>
		/// <param name="from">The position at which to start sending data.</param>
		/// <param name="to">The position at which to stop sending data.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="rangeSpecifier" /> is invalid.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="from" /> is greater than <paramref name="to" />  
		/// -or-  
		/// <paramref name="from" /> or <paramref name="to" /> is less than 0.</exception>
		/// <exception cref="T:System.InvalidOperationException">The range header could not be added.</exception>
		// Token: 0x060035DF RID: 13791 RVA: 0x000BB9D3 File Offset: 0x000B9BD3
		public void AddRange(int from, int to)
		{
			this.AddRange("bytes", (long)from, (long)to);
		}

		/// <summary>Adds a Range header to a request for a specific range from the beginning or end of the requested data.</summary>
		/// <param name="rangeSpecifier">The description of the range.</param>
		/// <param name="range">The starting or ending point of the range.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="rangeSpecifier" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="rangeSpecifier" /> is invalid.</exception>
		/// <exception cref="T:System.InvalidOperationException">The range header could not be added.</exception>
		// Token: 0x060035E0 RID: 13792 RVA: 0x000BB9E4 File Offset: 0x000B9BE4
		public void AddRange(string rangeSpecifier, int range)
		{
			this.AddRange(rangeSpecifier, (long)range);
		}

		/// <summary>Adds a range header to a request for a specified range.</summary>
		/// <param name="rangeSpecifier">The description of the range.</param>
		/// <param name="from">The position at which to start sending data.</param>
		/// <param name="to">The position at which to stop sending data.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="rangeSpecifier" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="from" /> is greater than <paramref name="to" />  
		/// -or-  
		/// <paramref name="from" /> or <paramref name="to" /> is less than 0.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="rangeSpecifier" /> is invalid.</exception>
		/// <exception cref="T:System.InvalidOperationException">The range header could not be added.</exception>
		// Token: 0x060035E1 RID: 13793 RVA: 0x000BB9EF File Offset: 0x000B9BEF
		public void AddRange(string rangeSpecifier, int from, int to)
		{
			this.AddRange(rangeSpecifier, (long)from, (long)to);
		}

		/// <summary>Adds a byte range header to a request for a specific range from the beginning or end of the requested data.</summary>
		/// <param name="range">The starting or ending point of the range.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="rangeSpecifier" /> is invalid.</exception>
		/// <exception cref="T:System.InvalidOperationException">The range header could not be added.</exception>
		// Token: 0x060035E2 RID: 13794 RVA: 0x000BB9FC File Offset: 0x000B9BFC
		public void AddRange(long range)
		{
			this.AddRange("bytes", range);
		}

		/// <summary>Adds a byte range header to the request for a specified range.</summary>
		/// <param name="from">The position at which to start sending data.</param>
		/// <param name="to">The position at which to stop sending data.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="rangeSpecifier" /> is invalid.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="from" /> is greater than <paramref name="to" />  
		/// -or-  
		/// <paramref name="from" /> or <paramref name="to" /> is less than 0.</exception>
		/// <exception cref="T:System.InvalidOperationException">The range header could not be added.</exception>
		// Token: 0x060035E3 RID: 13795 RVA: 0x000BBA0A File Offset: 0x000B9C0A
		public void AddRange(long from, long to)
		{
			this.AddRange("bytes", from, to);
		}

		/// <summary>Adds a Range header to a request for a specific range from the beginning or end of the requested data.</summary>
		/// <param name="rangeSpecifier">The description of the range.</param>
		/// <param name="range">The starting or ending point of the range.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="rangeSpecifier" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="rangeSpecifier" /> is invalid.</exception>
		/// <exception cref="T:System.InvalidOperationException">The range header could not be added.</exception>
		// Token: 0x060035E4 RID: 13796 RVA: 0x000BBA1C File Offset: 0x000B9C1C
		public void AddRange(string rangeSpecifier, long range)
		{
			if (rangeSpecifier == null)
			{
				throw new ArgumentNullException("rangeSpecifier");
			}
			if (!WebHeaderCollection.IsValidToken(rangeSpecifier))
			{
				throw new ArgumentException("Invalid range specifier", "rangeSpecifier");
			}
			string text = this.webHeaders["Range"];
			if (text == null)
			{
				text = rangeSpecifier + "=";
			}
			else
			{
				if (string.Compare(text.Substring(0, text.IndexOf('=')), rangeSpecifier, StringComparison.OrdinalIgnoreCase) != 0)
				{
					throw new InvalidOperationException("A different range specifier is already in use");
				}
				text += ",";
			}
			string text2 = range.ToString(CultureInfo.InvariantCulture);
			if (range < 0L)
			{
				text = text + "0" + text2;
			}
			else
			{
				text = text + text2 + "-";
			}
			this.webHeaders.ChangeInternal("Range", text);
		}

		/// <summary>Adds a range header to a request for a specified range.</summary>
		/// <param name="rangeSpecifier">The description of the range.</param>
		/// <param name="from">The position at which to start sending data.</param>
		/// <param name="to">The position at which to stop sending data.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="rangeSpecifier" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="from" /> is greater than <paramref name="to" />  
		/// -or-  
		/// <paramref name="from" /> or <paramref name="to" /> is less than 0.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="rangeSpecifier" /> is invalid.</exception>
		/// <exception cref="T:System.InvalidOperationException">The range header could not be added.</exception>
		// Token: 0x060035E5 RID: 13797 RVA: 0x000BBAE0 File Offset: 0x000B9CE0
		public void AddRange(string rangeSpecifier, long from, long to)
		{
			if (rangeSpecifier == null)
			{
				throw new ArgumentNullException("rangeSpecifier");
			}
			if (!WebHeaderCollection.IsValidToken(rangeSpecifier))
			{
				throw new ArgumentException("Invalid range specifier", "rangeSpecifier");
			}
			if (from > to || from < 0L)
			{
				throw new ArgumentOutOfRangeException("from");
			}
			if (to < 0L)
			{
				throw new ArgumentOutOfRangeException("to");
			}
			string text = this.webHeaders["Range"];
			if (text == null)
			{
				text = rangeSpecifier + "=";
			}
			else
			{
				text += ",";
			}
			text = string.Format("{0}{1}-{2}", text, from, to);
			this.webHeaders.ChangeInternal("Range", text);
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x000BBB90 File Offset: 0x000B9D90
		private WebOperation SendRequest(bool redirecting, BufferOffsetSize writeBuffer, CancellationToken cancellationToken)
		{
			object obj = this.locker;
			WebOperation webOperation2;
			lock (obj)
			{
				if (!redirecting && this.requestSent)
				{
					WebOperation webOperation = this.currentOperation;
					if (webOperation == null)
					{
						throw new InvalidOperationException("Should never happen!");
					}
					webOperation2 = webOperation;
				}
				else
				{
					WebOperation webOperation = new WebOperation(this, writeBuffer, false, cancellationToken);
					if (Interlocked.CompareExchange<WebOperation>(ref this.currentOperation, webOperation, null) != null)
					{
						throw new InvalidOperationException("Invalid nested call.");
					}
					this.requestSent = true;
					if (!redirecting)
					{
						this.redirects = 0;
					}
					this.servicePoint = this.GetServicePoint();
					this.servicePoint.SendRequest(webOperation, this.connectionGroup);
					webOperation2 = webOperation;
				}
			}
			return webOperation2;
		}

		// Token: 0x060035E7 RID: 13799 RVA: 0x000BBC44 File Offset: 0x000B9E44
		private Task<Stream> MyGetRequestStreamAsync(CancellationToken cancellationToken)
		{
			if (this.Aborted)
			{
				throw HttpWebRequest.CreateRequestAbortedException();
			}
			bool flag = !(this.method == "GET") && !(this.method == "CONNECT") && !(this.method == "HEAD") && !(this.method == "TRACE");
			if (this.method == null || !flag)
			{
				throw new ProtocolViolationException("Cannot send a content-body with this verb-type.");
			}
			if (this.contentLength == -1L && !this.sendChunked && !this.allowBuffering && this.KeepAlive)
			{
				throw new ProtocolViolationException("Content-Length not set");
			}
			string transferEncoding = this.TransferEncoding;
			if (!this.sendChunked && transferEncoding != null && transferEncoding.Trim() != "")
			{
				throw new InvalidOperationException("TransferEncoding requires the SendChunked property to be set to true.");
			}
			object obj = this.locker;
			WebOperation webOperation;
			lock (obj)
			{
				if (this.getResponseCalled)
				{
					throw new InvalidOperationException("This operation cannot be performed after the request has been submitted.");
				}
				webOperation = this.currentOperation;
				if (webOperation == null)
				{
					this.initialMethod = this.method;
					this.gotRequestStream = true;
					webOperation = this.SendRequest(false, null, cancellationToken);
				}
			}
			return webOperation.GetRequestStream();
		}

		/// <summary>Begins an asynchronous request for a <see cref="T:System.IO.Stream" /> object to use to write data.</summary>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate.</param>
		/// <param name="state">The state object for this request.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous request.</returns>
		/// <exception cref="T:System.Net.ProtocolViolationException">The <see cref="P:System.Net.HttpWebRequest.Method" /> property is GET or HEAD.  
		///  -or-  
		///  <see cref="P:System.Net.HttpWebRequest.KeepAlive" /> is <see langword="true" />, <see cref="P:System.Net.HttpWebRequest.AllowWriteStreamBuffering" /> is <see langword="false" />, <see cref="P:System.Net.HttpWebRequest.ContentLength" /> is -1, <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is <see langword="false" />, and <see cref="P:System.Net.HttpWebRequest.Method" /> is POST or PUT.</exception>
		/// <exception cref="T:System.InvalidOperationException">The stream is being used by a previous call to <see cref="M:System.Net.HttpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />  
		///  -or-  
		///  <see cref="P:System.Net.HttpWebRequest.TransferEncoding" /> is set to a value and <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is <see langword="false" />.  
		///  -or-  
		///  The thread pool is running out of threads.</exception>
		/// <exception cref="T:System.NotSupportedException">The request cache validator indicated that the response for this request can be served from the cache; however, requests that write data must not use the cache. This exception can occur if you are using a custom cache validator that is incorrectly implemented.</exception>
		/// <exception cref="T:System.Net.WebException">
		///   <see cref="M:System.Net.HttpWebRequest.Abort" /> was previously called.</exception>
		/// <exception cref="T:System.ObjectDisposedException">In a .NET Compact Framework application, a request stream with zero content length was not obtained and closed correctly. For more information about handling zero content length requests, see Network Programming in the .NET Compact Framework.</exception>
		// Token: 0x060035E8 RID: 13800 RVA: 0x000BBD90 File Offset: 0x000B9F90
		public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
		{
			return TaskToApm.Begin(this.RunWithTimeout<Stream>(new Func<CancellationToken, Task<Stream>>(this.MyGetRequestStreamAsync)), callback, state);
		}

		/// <summary>Ends an asynchronous request for a <see cref="T:System.IO.Stream" /> object to use to write data.</summary>
		/// <param name="asyncResult">The pending request for a stream.</param>
		/// <returns>A <see cref="T:System.IO.Stream" /> to use to write request data.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.IO.IOException">The request did not complete, and no stream is available.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by the current instance from a call to <see cref="M:System.Net.HttpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">This method was called previously using <paramref name="asyncResult" />.</exception>
		/// <exception cref="T:System.Net.WebException">
		///   <see cref="M:System.Net.HttpWebRequest.Abort" /> was previously called.  
		/// -or-  
		/// An error occurred while processing the request.</exception>
		// Token: 0x060035E9 RID: 13801 RVA: 0x000BBDAC File Offset: 0x000B9FAC
		public override Stream EndGetRequestStream(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			Stream stream;
			try
			{
				stream = TaskToApm.End<Stream>(asyncResult);
			}
			catch (Exception ex)
			{
				throw this.GetWebException(ex);
			}
			return stream;
		}

		/// <summary>Gets a <see cref="T:System.IO.Stream" /> object to use to write request data.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> to use to write request data.</returns>
		/// <exception cref="T:System.Net.ProtocolViolationException">The <see cref="P:System.Net.HttpWebRequest.Method" /> property is GET or HEAD.  
		///  -or-  
		///  <see cref="P:System.Net.HttpWebRequest.KeepAlive" /> is <see langword="true" />, <see cref="P:System.Net.HttpWebRequest.AllowWriteStreamBuffering" /> is <see langword="false" />, <see cref="P:System.Net.HttpWebRequest.ContentLength" /> is -1, <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is <see langword="false" />, and <see cref="P:System.Net.HttpWebRequest.Method" /> is POST or PUT.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Net.HttpWebRequest.GetRequestStream" /> method is called more than once.  
		///  -or-  
		///  <see cref="P:System.Net.HttpWebRequest.TransferEncoding" /> is set to a value and <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is <see langword="false" />.</exception>
		/// <exception cref="T:System.NotSupportedException">The request cache validator indicated that the response for this request can be served from the cache; however, requests that write data must not use the cache. This exception can occur if you are using a custom cache validator that is incorrectly implemented.</exception>
		/// <exception cref="T:System.Net.WebException">
		///   <see cref="M:System.Net.HttpWebRequest.Abort" /> was previously called.  
		/// -or-  
		/// The time-out period for the request expired.  
		/// -or-  
		/// An error occurred while processing the request.</exception>
		/// <exception cref="T:System.ObjectDisposedException">In a .NET Compact Framework application, a request stream with zero content length was not obtained and closed correctly. For more information about handling zero content length requests, see Network Programming in the .NET Compact Framework.</exception>
		// Token: 0x060035EA RID: 13802 RVA: 0x000BBDEC File Offset: 0x000B9FEC
		public override Stream GetRequestStream()
		{
			Stream result;
			try
			{
				result = this.GetRequestStreamAsync().Result;
			}
			catch (Exception ex)
			{
				throw this.GetWebException(ex);
			}
			return result;
		}

		/// <summary>Gets a <see cref="T:System.IO.Stream" /> object to use to write request data and outputs the <see cref="T:System.Net.TransportContext" /> associated with the stream.</summary>
		/// <param name="context">The <see cref="T:System.Net.TransportContext" /> for the <see cref="T:System.IO.Stream" />.</param>
		/// <returns>A <see cref="T:System.IO.Stream" /> to use to write request data.</returns>
		/// <exception cref="T:System.Exception">The <see cref="M:System.Net.HttpWebRequest.GetRequestStream" /> method was unable to obtain the <see cref="T:System.IO.Stream" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Net.HttpWebRequest.GetRequestStream" /> method is called more than once.  
		///  -or-  
		///  <see cref="P:System.Net.HttpWebRequest.TransferEncoding" /> is set to a value and <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is <see langword="false" />.</exception>
		/// <exception cref="T:System.NotSupportedException">The request cache validator indicated that the response for this request can be served from the cache; however, requests that write data must not use the cache. This exception can occur if you are using a custom cache validator that is incorrectly implemented.</exception>
		/// <exception cref="T:System.Net.ProtocolViolationException">The <see cref="P:System.Net.HttpWebRequest.Method" /> property is GET or HEAD.  
		///  -or-  
		///  <see cref="P:System.Net.HttpWebRequest.KeepAlive" /> is <see langword="true" />, <see cref="P:System.Net.HttpWebRequest.AllowWriteStreamBuffering" /> is <see langword="false" />, <see cref="P:System.Net.HttpWebRequest.ContentLength" /> is -1, <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is <see langword="false" />, and <see cref="P:System.Net.HttpWebRequest.Method" /> is POST or PUT.</exception>
		/// <exception cref="T:System.Net.WebException">
		///   <see cref="M:System.Net.HttpWebRequest.Abort" /> was previously called.  
		/// -or-  
		/// The time-out period for the request expired.  
		/// -or-  
		/// An error occurred while processing the request.</exception>
		// Token: 0x060035EB RID: 13803 RVA: 0x0000829A File Offset: 0x0000649A
		[MonoTODO]
		public Stream GetRequestStream(out TransportContext context)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x000BBE24 File Offset: 0x000BA024
		public override Task<Stream> GetRequestStreamAsync()
		{
			return this.RunWithTimeout<Stream>(new Func<CancellationToken, Task<Stream>>(this.MyGetRequestStreamAsync));
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x000BBE38 File Offset: 0x000BA038
		internal static Task<T> RunWithTimeout<T>(Func<CancellationToken, Task<T>> func, int timeout, Action abort, Func<bool> aborted, CancellationToken cancellationToken)
		{
			CancellationTokenSource cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
			return HttpWebRequest.RunWithTimeoutWorker<T>(func(cancellationTokenSource.Token), timeout, abort, aborted, cancellationTokenSource);
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x000BBE64 File Offset: 0x000BA064
		private static async Task<T> RunWithTimeoutWorker<T>(Task<T> workerTask, int timeout, Action abort, Func<bool> aborted, CancellationTokenSource cts)
		{
			T result;
			try
			{
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = ServicePointScheduler.WaitAsync(workerTask, timeout).ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
				}
				if (!configuredTaskAwaiter.GetResult())
				{
					try
					{
						cts.Cancel();
						abort();
					}
					catch
					{
					}
					workerTask.ContinueWith<int?>(delegate(Task<T> t)
					{
						AggregateException exception = t.Exception;
						if (exception == null)
						{
							return null;
						}
						return new int?(exception.GetHashCode());
					}, TaskContinuationOptions.OnlyOnFaulted);
					throw new WebException("The operation has timed out.", WebExceptionStatus.Timeout);
				}
				result = workerTask.Result;
			}
			catch (Exception ex)
			{
				throw HttpWebRequest.GetWebException(ex, aborted());
			}
			finally
			{
				cts.Dispose();
			}
			return result;
		}

		// Token: 0x060035EF RID: 13807 RVA: 0x000BBEC8 File Offset: 0x000BA0C8
		private Task<T> RunWithTimeout<T>(Func<CancellationToken, Task<T>> func)
		{
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
			return HttpWebRequest.RunWithTimeoutWorker<T>(func(cancellationTokenSource.Token), this.timeout, new Action(this.Abort), () => this.Aborted, cancellationTokenSource);
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x000BBF0C File Offset: 0x000BA10C
		private async Task<HttpWebResponse> MyGetResponseAsync(CancellationToken cancellationToken)
		{
			if (this.Aborted)
			{
				throw HttpWebRequest.CreateRequestAbortedException();
			}
			WebCompletionSource completion = new WebCompletionSource();
			object obj = this.locker;
			WebOperation operation;
			lock (obj)
			{
				this.getResponseCalled = true;
				WebCompletionSource webCompletionSource = Interlocked.CompareExchange<WebCompletionSource>(ref this.responseTask, completion, null);
				if (webCompletionSource != null)
				{
					webCompletionSource.ThrowOnError();
					if (this.haveResponse && webCompletionSource.Task.IsCompleted)
					{
						return this.webResponse;
					}
					throw new InvalidOperationException("Cannot re-call start of asynchronous method while a previous call is still in progress.");
				}
				else
				{
					operation = this.currentOperation;
					if (this.currentOperation != null)
					{
						this.writeStream = this.currentOperation.WriteStream;
					}
					this.initialMethod = this.method;
					operation = this.SendRequest(false, null, cancellationToken);
				}
			}
			HttpWebResponse httpWebResponse;
			for (;;)
			{
				WebException throwMe = null;
				HttpWebResponse response = null;
				WebResponseStream stream = null;
				bool redirect = false;
				bool mustReadAll = false;
				WebOperation ntlm = null;
				BufferOffsetSize writeBuffer = null;
				try
				{
					cancellationToken.ThrowIfCancellationRequested();
					WebRequestStream webRequestStream = await operation.GetRequestStreamInternal().ConfigureAwait(false);
					this.writeStream = webRequestStream;
					await this.writeStream.WriteRequestAsync(cancellationToken).ConfigureAwait(false);
					stream = await operation.GetResponseStream();
					ValueTuple<HttpWebResponse, bool, bool, BufferOffsetSize, WebOperation> valueTuple = await this.GetResponseFromData(stream, cancellationToken).ConfigureAwait(false);
					response = valueTuple.Item1;
					redirect = valueTuple.Item2;
					mustReadAll = valueTuple.Item3;
					writeBuffer = valueTuple.Item4;
					ntlm = valueTuple.Item5;
				}
				catch (Exception ex)
				{
					throwMe = this.GetWebException(ex);
				}
				obj = this.locker;
				lock (obj)
				{
					if (throwMe != null)
					{
						this.haveResponse = true;
						completion.TrySetException(throwMe);
						throw throwMe;
					}
					if (!redirect)
					{
						this.haveResponse = true;
						this.webResponse = response;
						completion.TrySetCompleted();
						httpWebResponse = response;
						break;
					}
					this.finished_reading = false;
					this.haveResponse = false;
					this.webResponse = null;
					this.currentOperation = ntlm;
				}
				try
				{
					if (mustReadAll)
					{
						await stream.ReadAllAsync(redirect || ntlm != null, cancellationToken).ConfigureAwait(false);
					}
					operation.Finish(true, null);
					response.Close();
				}
				catch (Exception ex2)
				{
					throwMe = this.GetWebException(ex2);
				}
				obj = this.locker;
				lock (obj)
				{
					if (throwMe != null)
					{
						this.haveResponse = true;
						WebResponseStream webResponseStream = stream;
						if (webResponseStream != null)
						{
							webResponseStream.Close();
						}
						completion.TrySetException(throwMe);
						throw throwMe;
					}
					if (ntlm == null)
					{
						operation = this.SendRequest(true, writeBuffer, cancellationToken);
					}
					else
					{
						operation = ntlm;
					}
				}
				throwMe = null;
				response = null;
				stream = null;
				ntlm = null;
				writeBuffer = null;
			}
			return httpWebResponse;
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x000BBF58 File Offset: 0x000BA158
		[return: TupleElementNames(new string[] { "response", "redirect", "mustReadAll", "writeBuffer", "ntlm" })]
		private async Task<ValueTuple<HttpWebResponse, bool, bool, BufferOffsetSize, WebOperation>> GetResponseFromData(WebResponseStream stream, CancellationToken cancellationToken)
		{
			HttpWebResponse response = new HttpWebResponse(this.actualUri, this.method, stream, this.cookieContainer);
			WebException throwMe = null;
			bool redirect = false;
			bool mustReadAll = false;
			WebOperation webOperation = null;
			Task<BufferOffsetSize> task = null;
			BufferOffsetSize bufferOffsetSize = null;
			object obj = this.locker;
			lock (obj)
			{
				ValueTuple<bool, bool, Task<BufferOffsetSize>, WebException> valueTuple = this.CheckFinalStatus(response);
				redirect = valueTuple.Item1;
				mustReadAll = valueTuple.Item2;
				task = valueTuple.Item3;
				throwMe = valueTuple.Item4;
			}
			if (throwMe != null)
			{
				if (mustReadAll)
				{
					await stream.ReadAllAsync(false, cancellationToken).ConfigureAwait(false);
				}
				throw throwMe;
			}
			if (task != null)
			{
				bufferOffsetSize = await task.ConfigureAwait(false);
			}
			obj = this.locker;
			lock (obj)
			{
				bool flag2 = this.ProxyQuery && this.proxy != null && !this.proxy.IsBypassed(this.actualUri);
				if (!redirect)
				{
					if ((flag2 ? this.proxy_auth_state : this.auth_state).IsNtlmAuthenticated && response.StatusCode < HttpStatusCode.BadRequest)
					{
						stream.Connection.NtlmAuthenticated = true;
					}
					if (this.writeStream != null)
					{
						this.writeStream.KillBuffer();
					}
					return new ValueTuple<HttpWebResponse, bool, bool, BufferOffsetSize, WebOperation>(response, false, false, bufferOffsetSize, null);
				}
				if (this.sendChunked)
				{
					this.sendChunked = false;
					this.webHeaders.RemoveInternal("Transfer-Encoding");
				}
				webOperation = this.HandleNtlmAuth(stream, response, bufferOffsetSize, cancellationToken).Item1;
			}
			return new ValueTuple<HttpWebResponse, bool, bool, BufferOffsetSize, WebOperation>(response, true, mustReadAll, bufferOffsetSize, webOperation);
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x000BBFAC File Offset: 0x000BA1AC
		internal static Exception FlattenException(Exception e)
		{
			AggregateException ex = e as AggregateException;
			if (ex != null)
			{
				ex = ex.Flatten();
				if (ex.InnerExceptions.Count == 1)
				{
					return ex.InnerException;
				}
			}
			return e;
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x000BBFE0 File Offset: 0x000BA1E0
		private WebException GetWebException(Exception e)
		{
			return HttpWebRequest.GetWebException(e, this.Aborted);
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x000BBFF0 File Offset: 0x000BA1F0
		private static WebException GetWebException(Exception e, bool aborted)
		{
			e = HttpWebRequest.FlattenException(e);
			WebException ex = e as WebException;
			if (ex != null && (!aborted || ex.Status == WebExceptionStatus.RequestCanceled || ex.Status == WebExceptionStatus.Timeout))
			{
				return ex;
			}
			if (aborted || e is OperationCanceledException || e is ObjectDisposedException)
			{
				return HttpWebRequest.CreateRequestAbortedException();
			}
			return new WebException(e.Message, e, WebExceptionStatus.UnknownError, null);
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x000BC04F File Offset: 0x000BA24F
		internal static WebException CreateRequestAbortedException()
		{
			return new WebException(SR.Format("The request was aborted: The request was canceled.", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
		}

		/// <summary>Begins an asynchronous request to an Internet resource.</summary>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate</param>
		/// <param name="state">The state object for this request.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous request for a response.</returns>
		/// <exception cref="T:System.InvalidOperationException">The stream is already in use by a previous call to <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" />  
		///  -or-  
		///  <see cref="P:System.Net.HttpWebRequest.TransferEncoding" /> is set to a value and <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is <see langword="false" />.  
		///  -or-  
		///  The thread pool is running out of threads.</exception>
		/// <exception cref="T:System.Net.ProtocolViolationException">
		///   <see cref="P:System.Net.HttpWebRequest.Method" /> is GET or HEAD, and either <see cref="P:System.Net.HttpWebRequest.ContentLength" /> is greater than zero or <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is <see langword="true" />.  
		/// -or-  
		/// <see cref="P:System.Net.HttpWebRequest.KeepAlive" /> is <see langword="true" />, <see cref="P:System.Net.HttpWebRequest.AllowWriteStreamBuffering" /> is <see langword="false" />, and either <see cref="P:System.Net.HttpWebRequest.ContentLength" /> is -1, <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is <see langword="false" /> and <see cref="P:System.Net.HttpWebRequest.Method" /> is POST or PUT.  
		/// -or-  
		/// The <see cref="T:System.Net.HttpWebRequest" /> has an entity body but the <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" /> method is called without calling the <see cref="M:System.Net.HttpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" /> method.  
		/// -or-  
		/// The <see cref="P:System.Net.HttpWebRequest.ContentLength" /> is greater than zero, but the application does not write all of the promised data.</exception>
		/// <exception cref="T:System.Net.WebException">
		///   <see cref="M:System.Net.HttpWebRequest.Abort" /> was previously called.</exception>
		// Token: 0x060035F6 RID: 13814 RVA: 0x000BC068 File Offset: 0x000BA268
		public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			if (this.Aborted)
			{
				throw HttpWebRequest.CreateRequestAbortedException();
			}
			string transferEncoding = this.TransferEncoding;
			if (!this.sendChunked && transferEncoding != null && transferEncoding.Trim() != "")
			{
				throw new InvalidOperationException("TransferEncoding requires the SendChunked property to be set to true.");
			}
			return TaskToApm.Begin(this.RunWithTimeout<HttpWebResponse>(new Func<CancellationToken, Task<HttpWebResponse>>(this.MyGetResponseAsync)), callback, state);
		}

		/// <summary>Ends an asynchronous request to an Internet resource.</summary>
		/// <param name="asyncResult">The pending request for a response.</param>
		/// <returns>A <see cref="T:System.Net.WebResponse" /> that contains the response from the Internet resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">This method was called previously using <paramref name="asyncResult." />  
		///  -or-  
		///  The <see cref="P:System.Net.HttpWebRequest.ContentLength" /> property is greater than 0 but the data has not been written to the request stream.</exception>
		/// <exception cref="T:System.Net.WebException">
		///   <see cref="M:System.Net.HttpWebRequest.Abort" /> was previously called.  
		/// -or-  
		/// An error occurred while processing the request.</exception>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by the current instance from a call to <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" />.</exception>
		// Token: 0x060035F7 RID: 13815 RVA: 0x000BC0CC File Offset: 0x000BA2CC
		public override WebResponse EndGetResponse(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			WebResponse webResponse;
			try
			{
				webResponse = TaskToApm.End<HttpWebResponse>(asyncResult);
			}
			catch (Exception ex)
			{
				throw this.GetWebException(ex);
			}
			return webResponse;
		}

		/// <summary>Ends an asynchronous request for a <see cref="T:System.IO.Stream" /> object to use to write data and outputs the <see cref="T:System.Net.TransportContext" /> associated with the stream.</summary>
		/// <param name="asyncResult">The pending request for a stream.</param>
		/// <param name="context">The <see cref="T:System.Net.TransportContext" /> for the <see cref="T:System.IO.Stream" />.</param>
		/// <returns>A <see cref="T:System.IO.Stream" /> to use to write request data.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> was not returned by the current instance from a call to <see cref="M:System.Net.HttpWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">This method was called previously using <paramref name="asyncResult" />.</exception>
		/// <exception cref="T:System.IO.IOException">The request did not complete, and no stream is available.</exception>
		/// <exception cref="T:System.Net.WebException">
		///   <see cref="M:System.Net.HttpWebRequest.Abort" /> was previously called.  
		/// -or-  
		/// An error occurred while processing the request.</exception>
		// Token: 0x060035F8 RID: 13816 RVA: 0x000BC10C File Offset: 0x000BA30C
		public Stream EndGetRequestStream(IAsyncResult asyncResult, out TransportContext context)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			context = null;
			return this.EndGetRequestStream(asyncResult);
		}

		/// <summary>Returns a response from an Internet resource.</summary>
		/// <returns>A <see cref="T:System.Net.WebResponse" /> that contains the response from the Internet resource.</returns>
		/// <exception cref="T:System.InvalidOperationException">The stream is already in use by a previous call to <see cref="M:System.Net.HttpWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" />.  
		///  -or-  
		///  <see cref="P:System.Net.HttpWebRequest.TransferEncoding" /> is set to a value and <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is <see langword="false" />.</exception>
		/// <exception cref="T:System.Net.ProtocolViolationException">
		///   <see cref="P:System.Net.HttpWebRequest.Method" /> is GET or HEAD, and either <see cref="P:System.Net.HttpWebRequest.ContentLength" /> is greater or equal to zero or <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is <see langword="true" />.  
		/// -or-  
		/// <see cref="P:System.Net.HttpWebRequest.KeepAlive" /> is <see langword="true" />, <see cref="P:System.Net.HttpWebRequest.AllowWriteStreamBuffering" /> is <see langword="false" />, <see cref="P:System.Net.HttpWebRequest.ContentLength" /> is -1, <see cref="P:System.Net.HttpWebRequest.SendChunked" /> is <see langword="false" />, and <see cref="P:System.Net.HttpWebRequest.Method" /> is POST or PUT.  
		/// -or-  
		/// The <see cref="T:System.Net.HttpWebRequest" /> has an entity body but the <see cref="M:System.Net.HttpWebRequest.GetResponse" /> method is called without calling the <see cref="M:System.Net.HttpWebRequest.GetRequestStream" /> method.  
		/// -or-  
		/// The <see cref="P:System.Net.HttpWebRequest.ContentLength" /> is greater than zero, but the application does not write all of the promised data.</exception>
		/// <exception cref="T:System.NotSupportedException">The request cache validator indicated that the response for this request can be served from the cache; however, this request includes data to be sent to the server. Requests that send data must not use the cache. This exception can occur if you are using a custom cache validator that is incorrectly implemented.</exception>
		/// <exception cref="T:System.Net.WebException">
		///   <see cref="M:System.Net.HttpWebRequest.Abort" /> was previously called.  
		/// -or-  
		/// The time-out period for the request expired.  
		/// -or-  
		/// An error occurred while processing the request.</exception>
		// Token: 0x060035F9 RID: 13817 RVA: 0x000BC128 File Offset: 0x000BA328
		public override WebResponse GetResponse()
		{
			WebResponse result;
			try
			{
				result = this.GetResponseAsync().Result;
			}
			catch (Exception ex)
			{
				throw this.GetWebException(ex);
			}
			return result;
		}

		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x060035FA RID: 13818 RVA: 0x000BC160 File Offset: 0x000BA360
		// (set) Token: 0x060035FB RID: 13819 RVA: 0x000BC168 File Offset: 0x000BA368
		internal bool FinishedReading
		{
			get
			{
				return this.finished_reading;
			}
			set
			{
				this.finished_reading = value;
			}
		}

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x060035FC RID: 13820 RVA: 0x000BC171 File Offset: 0x000BA371
		internal bool Aborted
		{
			get
			{
				return Interlocked.CompareExchange(ref this.aborted, 0, 0) == 1;
			}
		}

		/// <summary>Cancels a request to an Internet resource.</summary>
		// Token: 0x060035FD RID: 13821 RVA: 0x000BC184 File Offset: 0x000BA384
		public override void Abort()
		{
			if (Interlocked.CompareExchange(ref this.aborted, 1, 0) == 1)
			{
				return;
			}
			this.haveResponse = true;
			WebOperation webOperation = this.currentOperation;
			if (webOperation != null)
			{
				webOperation.Abort();
			}
			WebCompletionSource webCompletionSource = this.responseTask;
			if (webCompletionSource != null)
			{
				webCompletionSource.TrySetCanceled();
			}
			if (this.webResponse != null)
			{
				try
				{
					this.webResponse.Close();
					this.webResponse = null;
				}
				catch
				{
				}
			}
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the target object.</summary>
		/// <param name="serializationInfo">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that specifies the destination for this serialization.</param>
		// Token: 0x060035FE RID: 13822 RVA: 0x000BC1FC File Offset: 0x000BA3FC
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			throw new SerializationException();
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data required to serialize the target object.</summary>
		/// <param name="serializationInfo">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that specifies the destination for this serialization.</param>
		// Token: 0x060035FF RID: 13823 RVA: 0x000BC1FC File Offset: 0x000BA3FC
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		protected override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			throw new SerializationException();
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x000BC203 File Offset: 0x000BA403
		private void CheckRequestStarted()
		{
			if (this.requestSent)
			{
				throw new InvalidOperationException("request started");
			}
		}

		// Token: 0x06003601 RID: 13825 RVA: 0x000BC218 File Offset: 0x000BA418
		internal void DoContinueDelegate(int statusCode, WebHeaderCollection headers)
		{
			if (this.continueDelegate != null)
			{
				this.continueDelegate(statusCode, headers);
			}
		}

		// Token: 0x06003602 RID: 13826 RVA: 0x000BC22F File Offset: 0x000BA42F
		private void RewriteRedirectToGet()
		{
			this.method = "GET";
			this.webHeaders.RemoveInternal("Transfer-Encoding");
			this.sendChunked = false;
		}

		// Token: 0x06003603 RID: 13827 RVA: 0x000BC254 File Offset: 0x000BA454
		private bool Redirect(HttpStatusCode code, WebResponse response)
		{
			this.redirects++;
			Exception ex = null;
			string text = null;
			switch (code)
			{
			case HttpStatusCode.MultipleChoices:
				ex = new WebException("Ambiguous redirect.");
				goto IL_0097;
			case HttpStatusCode.MovedPermanently:
			case HttpStatusCode.Found:
				if (this.method == "POST")
				{
					this.RewriteRedirectToGet();
					goto IL_0097;
				}
				goto IL_0097;
			case HttpStatusCode.SeeOther:
				this.RewriteRedirectToGet();
				goto IL_0097;
			case HttpStatusCode.NotModified:
				return false;
			case HttpStatusCode.UseProxy:
				ex = new NotImplementedException("Proxy support not available.");
				goto IL_0097;
			case HttpStatusCode.TemporaryRedirect:
				goto IL_0097;
			}
			string text2 = "Invalid status code: ";
			int num = (int)code;
			ex = new ProtocolViolationException(text2 + num.ToString());
			IL_0097:
			if (this.method != "GET" && !this.InternalAllowBuffering && this.ResendContentFactory == null && (this.writeStream.WriteBufferLength > 0 || this.contentLength > 0L))
			{
				ex = new WebException("The request requires buffering data to succeed.", null, WebExceptionStatus.ProtocolError, response);
			}
			if (ex != null)
			{
				throw ex;
			}
			if (this.AllowWriteStreamBuffering || this.method == "GET")
			{
				this.contentLength = -1L;
			}
			text = response.Headers["Location"];
			if (text == null)
			{
				throw new WebException(string.Format("No Location header found for {0}", (int)code), null, WebExceptionStatus.ProtocolError, response);
			}
			Uri uri = this.actualUri;
			try
			{
				this.actualUri = new Uri(this.actualUri, text);
			}
			catch (Exception)
			{
				throw new WebException(string.Format("Invalid URL ({0}) for {1}", text, (int)code), null, WebExceptionStatus.ProtocolError, response);
			}
			this.hostChanged = this.actualUri.Scheme != uri.Scheme || this.Host != uri.Authority;
			return true;
		}

		// Token: 0x06003604 RID: 13828 RVA: 0x000BC410 File Offset: 0x000BA610
		private string GetHeaders()
		{
			bool flag = false;
			if (this.sendChunked)
			{
				flag = true;
				this.webHeaders.ChangeInternal("Transfer-Encoding", "chunked");
				this.webHeaders.RemoveInternal("Content-Length");
			}
			else if (this.contentLength != -1L)
			{
				if (this.auth_state.NtlmAuthState == HttpWebRequest.NtlmAuthState.Challenge || this.proxy_auth_state.NtlmAuthState == HttpWebRequest.NtlmAuthState.Challenge)
				{
					if (this.haveContentLength || this.gotRequestStream || this.contentLength > 0L)
					{
						this.webHeaders.SetInternal("Content-Length", "0");
					}
					else
					{
						this.webHeaders.RemoveInternal("Content-Length");
					}
				}
				else
				{
					if (this.contentLength > 0L)
					{
						flag = true;
					}
					if (this.haveContentLength || this.gotRequestStream || this.contentLength > 0L)
					{
						this.webHeaders.SetInternal("Content-Length", this.contentLength.ToString());
					}
				}
				this.webHeaders.RemoveInternal("Transfer-Encoding");
			}
			else
			{
				this.webHeaders.RemoveInternal("Content-Length");
			}
			if (this.actualVersion == HttpVersion.Version11 && flag && this.servicePoint.SendContinue)
			{
				this.webHeaders.ChangeInternal("Expect", "100-continue");
				this.expectContinue = true;
			}
			else
			{
				this.webHeaders.RemoveInternal("Expect");
				this.expectContinue = false;
			}
			bool proxyQuery = this.ProxyQuery;
			string text = (proxyQuery ? "Proxy-Connection" : "Connection");
			this.webHeaders.RemoveInternal((!proxyQuery) ? "Proxy-Connection" : "Connection");
			Version protocolVersion = this.servicePoint.ProtocolVersion;
			bool flag2 = protocolVersion == null || protocolVersion == HttpVersion.Version10;
			if (this.keepAlive && (this.version == HttpVersion.Version10 || flag2))
			{
				if (this.webHeaders[text] == null || this.webHeaders[text].IndexOf("keep-alive", StringComparison.OrdinalIgnoreCase) == -1)
				{
					this.webHeaders.ChangeInternal(text, "keep-alive");
				}
			}
			else if (!this.keepAlive && this.version == HttpVersion.Version11)
			{
				this.webHeaders.ChangeInternal(text, "close");
			}
			string text2;
			if (this.hostUri != null)
			{
				if (this.hostHasPort)
				{
					text2 = this.hostUri.GetComponents(UriComponents.HostAndPort, UriFormat.Unescaped);
				}
				else
				{
					text2 = this.hostUri.GetComponents(UriComponents.Host, UriFormat.Unescaped);
				}
			}
			else if (this.Address.IsDefaultPort)
			{
				text2 = this.Address.GetComponents(UriComponents.Host, UriFormat.Unescaped);
			}
			else
			{
				text2 = this.Address.GetComponents(UriComponents.HostAndPort, UriFormat.Unescaped);
			}
			this.webHeaders.SetInternal("Host", text2);
			if (this.cookieContainer != null)
			{
				string cookieHeader = this.cookieContainer.GetCookieHeader(this.actualUri);
				if (cookieHeader != "")
				{
					this.webHeaders.ChangeInternal("Cookie", cookieHeader);
				}
				else
				{
					this.webHeaders.RemoveInternal("Cookie");
				}
			}
			string text3 = null;
			if ((this.auto_decomp & DecompressionMethods.GZip) != DecompressionMethods.None)
			{
				text3 = "gzip";
			}
			if ((this.auto_decomp & DecompressionMethods.Deflate) != DecompressionMethods.None)
			{
				text3 = ((text3 != null) ? "gzip, deflate" : "deflate");
			}
			if (text3 != null)
			{
				this.webHeaders.ChangeInternal("Accept-Encoding", text3);
			}
			if (!this.usedPreAuth && this.preAuthenticate)
			{
				this.DoPreAuthenticate();
			}
			return this.webHeaders.ToString();
		}

		// Token: 0x06003605 RID: 13829 RVA: 0x000BC784 File Offset: 0x000BA984
		private void DoPreAuthenticate()
		{
			bool flag = this.proxy != null && !this.proxy.IsBypassed(this.actualUri);
			ICredentials credentials = ((!flag || this.credentials != null) ? this.credentials : this.proxy.Credentials);
			Authorization authorization = AuthenticationManager.PreAuthenticate(this, credentials);
			if (authorization == null)
			{
				return;
			}
			this.webHeaders.RemoveInternal("Proxy-Authorization");
			this.webHeaders.RemoveInternal("Authorization");
			string text = ((flag && this.credentials == null) ? "Proxy-Authorization" : "Authorization");
			this.webHeaders[text] = authorization.Message;
			this.usedPreAuth = true;
		}

		// Token: 0x06003606 RID: 13830 RVA: 0x000BC830 File Offset: 0x000BAA30
		internal byte[] GetRequestHeaders()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text;
			if (!this.ProxyQuery)
			{
				text = this.actualUri.PathAndQuery;
			}
			else
			{
				text = string.Format("{0}://{1}{2}", this.actualUri.Scheme, this.Host, this.actualUri.PathAndQuery);
			}
			if (!this.force_version && this.servicePoint.ProtocolVersion != null && this.servicePoint.ProtocolVersion < this.version)
			{
				this.actualVersion = this.servicePoint.ProtocolVersion;
			}
			else
			{
				this.actualVersion = this.version;
			}
			stringBuilder.AppendFormat("{0} {1} HTTP/{2}.{3}\r\n", new object[]
			{
				this.method,
				text,
				this.actualVersion.Major,
				this.actualVersion.Minor
			});
			stringBuilder.Append(this.GetHeaders());
			string text2 = stringBuilder.ToString();
			return Encoding.UTF8.GetBytes(text2);
		}

		// Token: 0x06003607 RID: 13831 RVA: 0x000BC938 File Offset: 0x000BAB38
		private ValueTuple<WebOperation, bool> HandleNtlmAuth(WebResponseStream stream, HttpWebResponse response, BufferOffsetSize writeBuffer, CancellationToken cancellationToken)
		{
			bool flag = response.StatusCode == HttpStatusCode.ProxyAuthenticationRequired;
			if ((flag ? this.proxy_auth_state : this.auth_state).NtlmAuthState == HttpWebRequest.NtlmAuthState.None)
			{
				return new ValueTuple<WebOperation, bool>(null, false);
			}
			bool flag2 = this.auth_state.NtlmAuthState == HttpWebRequest.NtlmAuthState.Challenge || this.proxy_auth_state.NtlmAuthState == HttpWebRequest.NtlmAuthState.Challenge;
			WebOperation webOperation = new WebOperation(this, writeBuffer, flag2, cancellationToken);
			stream.Operation.SetPriorityRequest(webOperation);
			ICredentials credentials = ((!flag || this.proxy == null) ? this.credentials : this.proxy.Credentials);
			if (credentials != null)
			{
				stream.Connection.NtlmCredential = credentials.GetCredential(this.requestUri, "NTLM");
				stream.Connection.UnsafeAuthenticatedConnectionSharing = this.unsafe_auth_blah;
			}
			return new ValueTuple<WebOperation, bool>(webOperation, flag2);
		}

		// Token: 0x06003608 RID: 13832 RVA: 0x000BCA04 File Offset: 0x000BAC04
		private bool CheckAuthorization(WebResponse response, HttpStatusCode code)
		{
			if (code != HttpStatusCode.ProxyAuthenticationRequired)
			{
				return this.auth_state.CheckAuthorization(response, code);
			}
			return this.proxy_auth_state.CheckAuthorization(response, code);
		}

		// Token: 0x06003609 RID: 13833 RVA: 0x000BCA2C File Offset: 0x000BAC2C
		[return: TupleElementNames(new string[] { "task", "throwMe" })]
		private ValueTuple<Task<BufferOffsetSize>, WebException> GetRewriteHandler(HttpWebResponse response, bool redirect)
		{
			if (redirect)
			{
				if (!this.MethodWithBuffer)
				{
					return new ValueTuple<Task<BufferOffsetSize>, WebException>(null, null);
				}
				if (this.writeStream.WriteBufferLength == 0 || this.contentLength == 0L)
				{
					return new ValueTuple<Task<BufferOffsetSize>, WebException>(null, null);
				}
			}
			if (this.AllowWriteStreamBuffering)
			{
				return new ValueTuple<Task<BufferOffsetSize>, WebException>(Task.FromResult<BufferOffsetSize>(this.writeStream.GetWriteBuffer()), null);
			}
			if (this.ResendContentFactory == null)
			{
				return new ValueTuple<Task<BufferOffsetSize>, WebException>(null, new WebException("The request requires buffering data to succeed.", null, WebExceptionStatus.ProtocolError, response));
			}
			return new ValueTuple<Task<BufferOffsetSize>, WebException>(async delegate
			{
				BufferOffsetSize bufferOffsetSize;
				using (MemoryStream ms = new MemoryStream())
				{
					await this.ResendContentFactory(ms).ConfigureAwait(false);
					byte[] array = ms.ToArray();
					bufferOffsetSize = new BufferOffsetSize(array, 0, array.Length, false);
				}
				return bufferOffsetSize;
			}(), null);
		}

		// Token: 0x0600360A RID: 13834 RVA: 0x000BCABC File Offset: 0x000BACBC
		[return: TupleElementNames(new string[] { "redirect", "mustReadAll", "writeBuffer", "throwMe" })]
		private ValueTuple<bool, bool, Task<BufferOffsetSize>, WebException> CheckFinalStatus(HttpWebResponse response)
		{
			WebException ex = null;
			bool flag = false;
			Task<BufferOffsetSize> task = null;
			HttpStatusCode statusCode = response.StatusCode;
			if (((!this.auth_state.IsCompleted && statusCode == HttpStatusCode.Unauthorized && this.credentials != null) || (this.ProxyQuery && !this.proxy_auth_state.IsCompleted && statusCode == HttpStatusCode.ProxyAuthenticationRequired)) && !this.usedPreAuth && this.CheckAuthorization(response, statusCode))
			{
				flag = true;
				if (!this.MethodWithBuffer)
				{
					return new ValueTuple<bool, bool, Task<BufferOffsetSize>, WebException>(true, flag, null, null);
				}
				ValueTuple<Task<BufferOffsetSize>, WebException> rewriteHandler = this.GetRewriteHandler(response, false);
				task = rewriteHandler.Item1;
				ex = rewriteHandler.Item2;
				if (ex == null)
				{
					return new ValueTuple<bool, bool, Task<BufferOffsetSize>, WebException>(true, flag, task, null);
				}
				if (!this.ThrowOnError)
				{
					return new ValueTuple<bool, bool, Task<BufferOffsetSize>, WebException>(false, flag, null, null);
				}
				this.writeStream.InternalClose();
				this.writeStream = null;
				response.Close();
				return new ValueTuple<bool, bool, Task<BufferOffsetSize>, WebException>(false, flag, null, ex);
			}
			else
			{
				if (statusCode >= HttpStatusCode.BadRequest)
				{
					ex = new WebException(string.Format("The remote server returned an error: ({0}) {1}.", (int)statusCode, response.StatusDescription), null, WebExceptionStatus.ProtocolError, response);
					flag = true;
				}
				else if (statusCode == HttpStatusCode.NotModified && this.allowAutoRedirect)
				{
					ex = new WebException(string.Format("The remote server returned an error: ({0}) {1}.", (int)statusCode, response.StatusDescription), null, WebExceptionStatus.ProtocolError, response);
				}
				else if (statusCode >= HttpStatusCode.MultipleChoices && this.allowAutoRedirect && this.redirects >= this.maxAutoRedirect)
				{
					ex = new WebException("Max. redirections exceeded.", null, WebExceptionStatus.ProtocolError, response);
					flag = true;
				}
				if (ex == null)
				{
					int num = (int)statusCode;
					bool flag2 = false;
					if (this.allowAutoRedirect && num >= 300)
					{
						flag2 = this.Redirect(statusCode, response);
						ValueTuple<Task<BufferOffsetSize>, WebException> rewriteHandler2 = this.GetRewriteHandler(response, true);
						task = rewriteHandler2.Item1;
						ex = rewriteHandler2.Item2;
						if (flag2 && !this.unsafe_auth_blah)
						{
							this.auth_state.Reset();
							this.proxy_auth_state.Reset();
						}
					}
					if (num >= 300 && num != 304)
					{
						flag = true;
					}
					if (ex == null)
					{
						return new ValueTuple<bool, bool, Task<BufferOffsetSize>, WebException>(flag2, flag, task, null);
					}
				}
				if (!this.ThrowOnError)
				{
					return new ValueTuple<bool, bool, Task<BufferOffsetSize>, WebException>(false, flag, null, null);
				}
				if (this.writeStream != null)
				{
					this.writeStream.InternalClose();
					this.writeStream = null;
				}
				return new ValueTuple<bool, bool, Task<BufferOffsetSize>, WebException>(false, flag, null, ex);
			}
		}

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x0600360B RID: 13835 RVA: 0x000BCCD0 File Offset: 0x000BAED0
		// (set) Token: 0x0600360C RID: 13836 RVA: 0x000BCCD8 File Offset: 0x000BAED8
		internal bool ReuseConnection { get; set; }

		// Token: 0x0600360D RID: 13837 RVA: 0x000BCCE4 File Offset: 0x000BAEE4
		internal static StringBuilder GenerateConnectionGroup(string connectionGroupName, bool unsafeConnectionGroup, bool isInternalGroup)
		{
			StringBuilder stringBuilder = new StringBuilder(connectionGroupName);
			stringBuilder.Append(unsafeConnectionGroup ? "U>" : "S>");
			if (isInternalGroup)
			{
				stringBuilder.Append("I>");
			}
			return stringBuilder;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.HttpWebRequest" /> class. This constructor is obsolete.</summary>
		// Token: 0x06003610 RID: 13840 RVA: 0x00013BCA File Offset: 0x00011DCA
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
		public HttpWebRequest()
		{
			global::Unity.ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04001F1B RID: 7963
		private Uri requestUri;

		// Token: 0x04001F1C RID: 7964
		private Uri actualUri;

		// Token: 0x04001F1D RID: 7965
		private bool hostChanged;

		// Token: 0x04001F1E RID: 7966
		private bool allowAutoRedirect;

		// Token: 0x04001F1F RID: 7967
		private bool allowBuffering;

		// Token: 0x04001F20 RID: 7968
		private bool allowReadStreamBuffering;

		// Token: 0x04001F21 RID: 7969
		private X509CertificateCollection certificates;

		// Token: 0x04001F22 RID: 7970
		private string connectionGroup;

		// Token: 0x04001F23 RID: 7971
		private bool haveContentLength;

		// Token: 0x04001F24 RID: 7972
		private long contentLength;

		// Token: 0x04001F25 RID: 7973
		private HttpContinueDelegate continueDelegate;

		// Token: 0x04001F26 RID: 7974
		private CookieContainer cookieContainer;

		// Token: 0x04001F27 RID: 7975
		private ICredentials credentials;

		// Token: 0x04001F28 RID: 7976
		private bool haveResponse;

		// Token: 0x04001F29 RID: 7977
		private bool requestSent;

		// Token: 0x04001F2A RID: 7978
		private WebHeaderCollection webHeaders;

		// Token: 0x04001F2B RID: 7979
		private bool keepAlive;

		// Token: 0x04001F2C RID: 7980
		private int maxAutoRedirect;

		// Token: 0x04001F2D RID: 7981
		private string mediaType;

		// Token: 0x04001F2E RID: 7982
		private string method;

		// Token: 0x04001F2F RID: 7983
		private string initialMethod;

		// Token: 0x04001F30 RID: 7984
		private bool pipelined;

		// Token: 0x04001F31 RID: 7985
		private bool preAuthenticate;

		// Token: 0x04001F32 RID: 7986
		private bool usedPreAuth;

		// Token: 0x04001F33 RID: 7987
		private Version version;

		// Token: 0x04001F34 RID: 7988
		private bool force_version;

		// Token: 0x04001F35 RID: 7989
		private Version actualVersion;

		// Token: 0x04001F36 RID: 7990
		private IWebProxy proxy;

		// Token: 0x04001F37 RID: 7991
		private bool sendChunked;

		// Token: 0x04001F38 RID: 7992
		private ServicePoint servicePoint;

		// Token: 0x04001F39 RID: 7993
		private int timeout;

		// Token: 0x04001F3A RID: 7994
		private int continueTimeout;

		// Token: 0x04001F3B RID: 7995
		private WebRequestStream writeStream;

		// Token: 0x04001F3C RID: 7996
		private HttpWebResponse webResponse;

		// Token: 0x04001F3D RID: 7997
		private WebCompletionSource responseTask;

		// Token: 0x04001F3E RID: 7998
		private WebOperation currentOperation;

		// Token: 0x04001F3F RID: 7999
		private int aborted;

		// Token: 0x04001F40 RID: 8000
		private bool gotRequestStream;

		// Token: 0x04001F41 RID: 8001
		private int redirects;

		// Token: 0x04001F42 RID: 8002
		private bool expectContinue;

		// Token: 0x04001F43 RID: 8003
		private bool getResponseCalled;

		// Token: 0x04001F44 RID: 8004
		private object locker;

		// Token: 0x04001F45 RID: 8005
		private bool finished_reading;

		// Token: 0x04001F46 RID: 8006
		private DecompressionMethods auto_decomp;

		// Token: 0x04001F47 RID: 8007
		private int maxResponseHeadersLength;

		// Token: 0x04001F48 RID: 8008
		private static int defaultMaxResponseHeadersLength = 64;

		// Token: 0x04001F49 RID: 8009
		private static int defaultMaximumErrorResponseLength = 64;

		// Token: 0x04001F4A RID: 8010
		private static RequestCachePolicy defaultCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);

		// Token: 0x04001F4B RID: 8011
		private int readWriteTimeout;

		// Token: 0x04001F4C RID: 8012
		private MobileTlsProvider tlsProvider;

		// Token: 0x04001F4D RID: 8013
		private MonoTlsSettings tlsSettings;

		// Token: 0x04001F4E RID: 8014
		private ServerCertValidationCallback certValidationCallback;

		// Token: 0x04001F4F RID: 8015
		private bool hostHasPort;

		// Token: 0x04001F50 RID: 8016
		private Uri hostUri;

		// Token: 0x04001F51 RID: 8017
		private HttpWebRequest.AuthorizationState auth_state;

		// Token: 0x04001F52 RID: 8018
		private HttpWebRequest.AuthorizationState proxy_auth_state;

		// Token: 0x04001F53 RID: 8019
		[NonSerialized]
		internal Func<Stream, Task> ResendContentFactory;

		// Token: 0x04001F54 RID: 8020
		internal readonly int ID;

		// Token: 0x04001F56 RID: 8022
		private bool unsafe_auth_blah;

		// Token: 0x02000695 RID: 1685
		private enum NtlmAuthState
		{
			// Token: 0x04001F59 RID: 8025
			None,
			// Token: 0x04001F5A RID: 8026
			Challenge,
			// Token: 0x04001F5B RID: 8027
			Response
		}

		// Token: 0x02000696 RID: 1686
		private struct AuthorizationState
		{
			// Token: 0x17000B26 RID: 2854
			// (get) Token: 0x06003611 RID: 13841 RVA: 0x000BCD6B File Offset: 0x000BAF6B
			public bool IsCompleted
			{
				get
				{
					return this.isCompleted;
				}
			}

			// Token: 0x17000B27 RID: 2855
			// (get) Token: 0x06003612 RID: 13842 RVA: 0x000BCD73 File Offset: 0x000BAF73
			public HttpWebRequest.NtlmAuthState NtlmAuthState
			{
				get
				{
					return this.ntlm_auth_state;
				}
			}

			// Token: 0x17000B28 RID: 2856
			// (get) Token: 0x06003613 RID: 13843 RVA: 0x000BCD7B File Offset: 0x000BAF7B
			public bool IsNtlmAuthenticated
			{
				get
				{
					return this.isCompleted && this.ntlm_auth_state > HttpWebRequest.NtlmAuthState.None;
				}
			}

			// Token: 0x06003614 RID: 13844 RVA: 0x000BCD90 File Offset: 0x000BAF90
			public AuthorizationState(HttpWebRequest request, bool isProxy)
			{
				this.request = request;
				this.isProxy = isProxy;
				this.isCompleted = false;
				this.ntlm_auth_state = HttpWebRequest.NtlmAuthState.None;
			}

			// Token: 0x06003615 RID: 13845 RVA: 0x000BCDB0 File Offset: 0x000BAFB0
			public bool CheckAuthorization(WebResponse response, HttpStatusCode code)
			{
				this.isCompleted = false;
				if (code == HttpStatusCode.Unauthorized && this.request.credentials == null)
				{
					return false;
				}
				if (this.isProxy != (code == HttpStatusCode.ProxyAuthenticationRequired))
				{
					return false;
				}
				if (this.isProxy && (this.request.proxy == null || this.request.proxy.Credentials == null))
				{
					return false;
				}
				string[] values = response.Headers.GetValues(this.isProxy ? "Proxy-Authenticate" : "WWW-Authenticate");
				if (values == null || values.Length == 0)
				{
					return false;
				}
				ICredentials credentials = ((!this.isProxy) ? this.request.credentials : this.request.proxy.Credentials);
				Authorization authorization = null;
				string[] array = values;
				for (int i = 0; i < array.Length; i++)
				{
					authorization = AuthenticationManager.Authenticate(array[i], this.request, credentials);
					if (authorization != null)
					{
						break;
					}
				}
				if (authorization == null)
				{
					return false;
				}
				this.request.webHeaders[this.isProxy ? "Proxy-Authorization" : "Authorization"] = authorization.Message;
				this.isCompleted = authorization.Complete;
				if (authorization.ModuleAuthenticationType == "NTLM")
				{
					this.ntlm_auth_state++;
				}
				return true;
			}

			// Token: 0x06003616 RID: 13846 RVA: 0x000BCEEB File Offset: 0x000BB0EB
			public void Reset()
			{
				this.isCompleted = false;
				this.ntlm_auth_state = HttpWebRequest.NtlmAuthState.None;
				this.request.webHeaders.RemoveInternal(this.isProxy ? "Proxy-Authorization" : "Authorization");
			}

			// Token: 0x06003617 RID: 13847 RVA: 0x000BCF1F File Offset: 0x000BB11F
			public override string ToString()
			{
				return string.Format("{0}AuthState [{1}:{2}]", this.isProxy ? "Proxy" : "", this.isCompleted, this.ntlm_auth_state);
			}

			// Token: 0x04001F5C RID: 8028
			private readonly HttpWebRequest request;

			// Token: 0x04001F5D RID: 8029
			private readonly bool isProxy;

			// Token: 0x04001F5E RID: 8030
			private bool isCompleted;

			// Token: 0x04001F5F RID: 8031
			private HttpWebRequest.NtlmAuthState ntlm_auth_state;
		}
	}
}
