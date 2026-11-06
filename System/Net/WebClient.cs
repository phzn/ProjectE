using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net.Cache;
using System.Net.Http;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
	/// <summary>Provides common methods for sending data to and receiving data from a resource identified by a URI.</summary>
	// Token: 0x02000597 RID: 1431
	public class WebClient : Component
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Net.WebClient" /> class.</summary>
		// Token: 0x06002E8E RID: 11918 RVA: 0x000A0D10 File Offset: 0x0009EF10
		public WebClient()
		{
			if (base.GetType() == typeof(WebClient))
			{
				GC.SuppressFinalize(this);
			}
		}

		/// <summary>Occurs when an asynchronous resource-download operation completes.</summary>
		// Token: 0x1400005F RID: 95
		// (add) Token: 0x06002E8F RID: 11919 RVA: 0x000A0D48 File Offset: 0x0009EF48
		// (remove) Token: 0x06002E90 RID: 11920 RVA: 0x000A0D80 File Offset: 0x0009EF80
		public event DownloadStringCompletedEventHandler DownloadStringCompleted;

		/// <summary>Occurs when an asynchronous data download operation completes.</summary>
		// Token: 0x14000060 RID: 96
		// (add) Token: 0x06002E91 RID: 11921 RVA: 0x000A0DB8 File Offset: 0x0009EFB8
		// (remove) Token: 0x06002E92 RID: 11922 RVA: 0x000A0DF0 File Offset: 0x0009EFF0
		public event DownloadDataCompletedEventHandler DownloadDataCompleted;

		/// <summary>Occurs when an asynchronous file download operation completes.</summary>
		// Token: 0x14000061 RID: 97
		// (add) Token: 0x06002E93 RID: 11923 RVA: 0x000A0E28 File Offset: 0x0009F028
		// (remove) Token: 0x06002E94 RID: 11924 RVA: 0x000A0E60 File Offset: 0x0009F060
		public event AsyncCompletedEventHandler DownloadFileCompleted;

		/// <summary>Occurs when an asynchronous string-upload operation completes.</summary>
		// Token: 0x14000062 RID: 98
		// (add) Token: 0x06002E95 RID: 11925 RVA: 0x000A0E98 File Offset: 0x0009F098
		// (remove) Token: 0x06002E96 RID: 11926 RVA: 0x000A0ED0 File Offset: 0x0009F0D0
		public event UploadStringCompletedEventHandler UploadStringCompleted;

		/// <summary>Occurs when an asynchronous data-upload operation completes.</summary>
		// Token: 0x14000063 RID: 99
		// (add) Token: 0x06002E97 RID: 11927 RVA: 0x000A0F08 File Offset: 0x0009F108
		// (remove) Token: 0x06002E98 RID: 11928 RVA: 0x000A0F40 File Offset: 0x0009F140
		public event UploadDataCompletedEventHandler UploadDataCompleted;

		/// <summary>Occurs when an asynchronous file-upload operation completes.</summary>
		// Token: 0x14000064 RID: 100
		// (add) Token: 0x06002E99 RID: 11929 RVA: 0x000A0F78 File Offset: 0x0009F178
		// (remove) Token: 0x06002E9A RID: 11930 RVA: 0x000A0FB0 File Offset: 0x0009F1B0
		public event UploadFileCompletedEventHandler UploadFileCompleted;

		/// <summary>Occurs when an asynchronous upload of a name/value collection completes.</summary>
		// Token: 0x14000065 RID: 101
		// (add) Token: 0x06002E9B RID: 11931 RVA: 0x000A0FE8 File Offset: 0x0009F1E8
		// (remove) Token: 0x06002E9C RID: 11932 RVA: 0x000A1020 File Offset: 0x0009F220
		public event UploadValuesCompletedEventHandler UploadValuesCompleted;

		/// <summary>Occurs when an asynchronous operation to open a stream containing a resource completes.</summary>
		// Token: 0x14000066 RID: 102
		// (add) Token: 0x06002E9D RID: 11933 RVA: 0x000A1058 File Offset: 0x0009F258
		// (remove) Token: 0x06002E9E RID: 11934 RVA: 0x000A1090 File Offset: 0x0009F290
		public event OpenReadCompletedEventHandler OpenReadCompleted;

		/// <summary>Occurs when an asynchronous operation to open a stream to write data to a resource completes.</summary>
		// Token: 0x14000067 RID: 103
		// (add) Token: 0x06002E9F RID: 11935 RVA: 0x000A10C8 File Offset: 0x0009F2C8
		// (remove) Token: 0x06002EA0 RID: 11936 RVA: 0x000A1100 File Offset: 0x0009F300
		public event OpenWriteCompletedEventHandler OpenWriteCompleted;

		/// <summary>Occurs when an asynchronous download operation successfully transfers some or all of the data.</summary>
		// Token: 0x14000068 RID: 104
		// (add) Token: 0x06002EA1 RID: 11937 RVA: 0x000A1138 File Offset: 0x0009F338
		// (remove) Token: 0x06002EA2 RID: 11938 RVA: 0x000A1170 File Offset: 0x0009F370
		public event DownloadProgressChangedEventHandler DownloadProgressChanged;

		/// <summary>Occurs when an asynchronous upload operation successfully transfers some or all of the data.</summary>
		// Token: 0x14000069 RID: 105
		// (add) Token: 0x06002EA3 RID: 11939 RVA: 0x000A11A8 File Offset: 0x0009F3A8
		// (remove) Token: 0x06002EA4 RID: 11940 RVA: 0x000A11E0 File Offset: 0x0009F3E0
		public event UploadProgressChangedEventHandler UploadProgressChanged;

		/// <summary>Raises the <see cref="E:System.Net.WebClient.DownloadStringCompleted" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Net.DownloadStringCompletedEventArgs" /> object containing event data.</param>
		// Token: 0x06002EA5 RID: 11941 RVA: 0x000A1215 File Offset: 0x0009F415
		protected virtual void OnDownloadStringCompleted(DownloadStringCompletedEventArgs e)
		{
			DownloadStringCompletedEventHandler downloadStringCompleted = this.DownloadStringCompleted;
			if (downloadStringCompleted == null)
			{
				return;
			}
			downloadStringCompleted(this, e);
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.DownloadDataCompleted" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Net.DownloadDataCompletedEventArgs" /> object that contains event data.</param>
		// Token: 0x06002EA6 RID: 11942 RVA: 0x000A1229 File Offset: 0x0009F429
		protected virtual void OnDownloadDataCompleted(DownloadDataCompletedEventArgs e)
		{
			DownloadDataCompletedEventHandler downloadDataCompleted = this.DownloadDataCompleted;
			if (downloadDataCompleted == null)
			{
				return;
			}
			downloadDataCompleted(this, e);
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.DownloadFileCompleted" /> event.</summary>
		/// <param name="e">An <see cref="T:System.ComponentModel.AsyncCompletedEventArgs" /> object containing event data.</param>
		// Token: 0x06002EA7 RID: 11943 RVA: 0x000A123D File Offset: 0x0009F43D
		protected virtual void OnDownloadFileCompleted(AsyncCompletedEventArgs e)
		{
			AsyncCompletedEventHandler downloadFileCompleted = this.DownloadFileCompleted;
			if (downloadFileCompleted == null)
			{
				return;
			}
			downloadFileCompleted(this, e);
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.DownloadProgressChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Net.DownloadProgressChangedEventArgs" /> object containing event data.</param>
		// Token: 0x06002EA8 RID: 11944 RVA: 0x000A1251 File Offset: 0x0009F451
		protected virtual void OnDownloadProgressChanged(DownloadProgressChangedEventArgs e)
		{
			DownloadProgressChangedEventHandler downloadProgressChanged = this.DownloadProgressChanged;
			if (downloadProgressChanged == null)
			{
				return;
			}
			downloadProgressChanged(this, e);
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.UploadStringCompleted" /> event.</summary>
		/// <param name="e">An <see cref="T:System.Net.UploadStringCompletedEventArgs" /> object containing event data.</param>
		// Token: 0x06002EA9 RID: 11945 RVA: 0x000A1265 File Offset: 0x0009F465
		protected virtual void OnUploadStringCompleted(UploadStringCompletedEventArgs e)
		{
			UploadStringCompletedEventHandler uploadStringCompleted = this.UploadStringCompleted;
			if (uploadStringCompleted == null)
			{
				return;
			}
			uploadStringCompleted(this, e);
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.UploadDataCompleted" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Net.UploadDataCompletedEventArgs" /> object containing event data.</param>
		// Token: 0x06002EAA RID: 11946 RVA: 0x000A1279 File Offset: 0x0009F479
		protected virtual void OnUploadDataCompleted(UploadDataCompletedEventArgs e)
		{
			UploadDataCompletedEventHandler uploadDataCompleted = this.UploadDataCompleted;
			if (uploadDataCompleted == null)
			{
				return;
			}
			uploadDataCompleted(this, e);
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.UploadFileCompleted" /> event.</summary>
		/// <param name="e">An <see cref="T:System.Net.UploadFileCompletedEventArgs" /> object containing event data.</param>
		// Token: 0x06002EAB RID: 11947 RVA: 0x000A128D File Offset: 0x0009F48D
		protected virtual void OnUploadFileCompleted(UploadFileCompletedEventArgs e)
		{
			UploadFileCompletedEventHandler uploadFileCompleted = this.UploadFileCompleted;
			if (uploadFileCompleted == null)
			{
				return;
			}
			uploadFileCompleted(this, e);
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.UploadValuesCompleted" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Net.UploadValuesCompletedEventArgs" /> object containing event data.</param>
		// Token: 0x06002EAC RID: 11948 RVA: 0x000A12A1 File Offset: 0x0009F4A1
		protected virtual void OnUploadValuesCompleted(UploadValuesCompletedEventArgs e)
		{
			UploadValuesCompletedEventHandler uploadValuesCompleted = this.UploadValuesCompleted;
			if (uploadValuesCompleted == null)
			{
				return;
			}
			uploadValuesCompleted(this, e);
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.UploadProgressChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.Net.UploadProgressChangedEventArgs" /> object containing event data.</param>
		// Token: 0x06002EAD RID: 11949 RVA: 0x000A12B5 File Offset: 0x0009F4B5
		protected virtual void OnUploadProgressChanged(UploadProgressChangedEventArgs e)
		{
			UploadProgressChangedEventHandler uploadProgressChanged = this.UploadProgressChanged;
			if (uploadProgressChanged == null)
			{
				return;
			}
			uploadProgressChanged(this, e);
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.OpenReadCompleted" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Net.OpenReadCompletedEventArgs" /> object containing event data.</param>
		// Token: 0x06002EAE RID: 11950 RVA: 0x000A12C9 File Offset: 0x0009F4C9
		protected virtual void OnOpenReadCompleted(OpenReadCompletedEventArgs e)
		{
			OpenReadCompletedEventHandler openReadCompleted = this.OpenReadCompleted;
			if (openReadCompleted == null)
			{
				return;
			}
			openReadCompleted(this, e);
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.OpenWriteCompleted" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Net.OpenWriteCompletedEventArgs" /> object containing event data.</param>
		// Token: 0x06002EAF RID: 11951 RVA: 0x000A12DD File Offset: 0x0009F4DD
		protected virtual void OnOpenWriteCompleted(OpenWriteCompletedEventArgs e)
		{
			OpenWriteCompletedEventHandler openWriteCompleted = this.OpenWriteCompleted;
			if (openWriteCompleted == null)
			{
				return;
			}
			openWriteCompleted(this, e);
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x000A12F4 File Offset: 0x0009F4F4
		private void StartOperation()
		{
			if (Interlocked.Increment(ref this._callNesting) > 1)
			{
				this.EndOperation();
				throw new NotSupportedException("WebClient does not support concurrent I/O operations.");
			}
			this._contentLength = -1L;
			this._webResponse = null;
			this._webRequest = null;
			this._method = null;
			this._canceled = false;
			WebClient.ProgressData progress = this._progress;
			if (progress == null)
			{
				return;
			}
			progress.Reset();
		}

		// Token: 0x06002EB1 RID: 11953 RVA: 0x000A1354 File Offset: 0x0009F554
		private AsyncOperation StartAsyncOperation(object userToken)
		{
			if (!this._initWebClientAsync)
			{
				this._openReadOperationCompleted = delegate(object arg)
				{
					this.OnOpenReadCompleted((OpenReadCompletedEventArgs)arg);
				};
				this._openWriteOperationCompleted = delegate(object arg)
				{
					this.OnOpenWriteCompleted((OpenWriteCompletedEventArgs)arg);
				};
				this._downloadStringOperationCompleted = delegate(object arg)
				{
					this.OnDownloadStringCompleted((DownloadStringCompletedEventArgs)arg);
				};
				this._downloadDataOperationCompleted = delegate(object arg)
				{
					this.OnDownloadDataCompleted((DownloadDataCompletedEventArgs)arg);
				};
				this._downloadFileOperationCompleted = delegate(object arg)
				{
					this.OnDownloadFileCompleted((AsyncCompletedEventArgs)arg);
				};
				this._uploadStringOperationCompleted = delegate(object arg)
				{
					this.OnUploadStringCompleted((UploadStringCompletedEventArgs)arg);
				};
				this._uploadDataOperationCompleted = delegate(object arg)
				{
					this.OnUploadDataCompleted((UploadDataCompletedEventArgs)arg);
				};
				this._uploadFileOperationCompleted = delegate(object arg)
				{
					this.OnUploadFileCompleted((UploadFileCompletedEventArgs)arg);
				};
				this._uploadValuesOperationCompleted = delegate(object arg)
				{
					this.OnUploadValuesCompleted((UploadValuesCompletedEventArgs)arg);
				};
				this._reportDownloadProgressChanged = delegate(object arg)
				{
					this.OnDownloadProgressChanged((DownloadProgressChangedEventArgs)arg);
				};
				this._reportUploadProgressChanged = delegate(object arg)
				{
					this.OnUploadProgressChanged((UploadProgressChangedEventArgs)arg);
				};
				this._progress = new WebClient.ProgressData();
				this._initWebClientAsync = true;
			}
			AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(userToken);
			this.StartOperation();
			this._asyncOp = asyncOperation;
			return asyncOperation;
		}

		// Token: 0x06002EB2 RID: 11954 RVA: 0x000A1459 File Offset: 0x0009F659
		private void EndOperation()
		{
			Interlocked.Decrement(ref this._callNesting);
		}

		/// <summary>Gets or sets the <see cref="T:System.Text.Encoding" /> used to upload and download strings.</summary>
		/// <returns>A <see cref="T:System.Text.Encoding" /> that is used to encode strings. The default value of this property is the encoding returned by <see cref="P:System.Text.Encoding.Default" />.</returns>
		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x06002EB3 RID: 11955 RVA: 0x000A1467 File Offset: 0x0009F667
		// (set) Token: 0x06002EB4 RID: 11956 RVA: 0x000A146F File Offset: 0x0009F66F
		public Encoding Encoding
		{
			get
			{
				return this._encoding;
			}
			set
			{
				WebClient.ThrowIfNull(value, "Encoding");
				this._encoding = value;
			}
		}

		/// <summary>Gets or sets the base URI for requests made by a <see cref="T:System.Net.WebClient" />.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the base URI for requests made by a <see cref="T:System.Net.WebClient" /> or <see cref="F:System.String.Empty" /> if no base address has been specified.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <see cref="P:System.Net.WebClient.BaseAddress" /> is set to an invalid URI. The inner exception may contain information that will help you locate the error.</exception>
		// Token: 0x17000975 RID: 2421
		// (get) Token: 0x06002EB5 RID: 11957 RVA: 0x000A1483 File Offset: 0x0009F683
		// (set) Token: 0x06002EB6 RID: 11958 RVA: 0x000A14A4 File Offset: 0x0009F6A4
		public string BaseAddress
		{
			get
			{
				if (!(this._baseAddress != null))
				{
					return string.Empty;
				}
				return this._baseAddress.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this._baseAddress = null;
					return;
				}
				try
				{
					this._baseAddress = new Uri(value);
				}
				catch (UriFormatException ex)
				{
					throw new ArgumentException("The specified value is not a valid base address.", "value", ex);
				}
			}
		}

		/// <summary>Gets or sets the network credentials that are sent to the host and used to authenticate the request.</summary>
		/// <returns>An <see cref="T:System.Net.ICredentials" /> containing the authentication credentials for the request. The default is <see langword="null" />.</returns>
		// Token: 0x17000976 RID: 2422
		// (get) Token: 0x06002EB7 RID: 11959 RVA: 0x000A14F4 File Offset: 0x0009F6F4
		// (set) Token: 0x06002EB8 RID: 11960 RVA: 0x000A14FC File Offset: 0x0009F6FC
		public ICredentials Credentials
		{
			get
			{
				return this._credentials;
			}
			set
			{
				this._credentials = value;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that controls whether the <see cref="P:System.Net.CredentialCache.DefaultCredentials" /> are sent with requests.</summary>
		/// <returns>
		///   <see langword="true" /> if the default credentials are used; otherwise <see langword="false" />. The default value is <see langword="false" />.</returns>
		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x06002EB9 RID: 11961 RVA: 0x000A1505 File Offset: 0x0009F705
		// (set) Token: 0x06002EBA RID: 11962 RVA: 0x000A1514 File Offset: 0x0009F714
		public bool UseDefaultCredentials
		{
			get
			{
				return this._credentials == CredentialCache.DefaultCredentials;
			}
			set
			{
				this._credentials = (value ? CredentialCache.DefaultCredentials : null);
			}
		}

		/// <summary>Gets or sets a collection of header name/value pairs associated with the request.</summary>
		/// <returns>A <see cref="T:System.Net.WebHeaderCollection" /> containing header name/value pairs associated with this request.</returns>
		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x06002EBB RID: 11963 RVA: 0x000A1528 File Offset: 0x0009F728
		// (set) Token: 0x06002EBC RID: 11964 RVA: 0x000A154D File Offset: 0x0009F74D
		public WebHeaderCollection Headers
		{
			get
			{
				WebHeaderCollection webHeaderCollection;
				if ((webHeaderCollection = this._headers) == null)
				{
					webHeaderCollection = (this._headers = new WebHeaderCollection());
				}
				return webHeaderCollection;
			}
			set
			{
				this._headers = value;
			}
		}

		/// <summary>Gets or sets a collection of query name/value pairs associated with the request.</summary>
		/// <returns>A <see cref="T:System.Collections.Specialized.NameValueCollection" /> that contains query name/value pairs associated with the request. If no pairs are associated with the request, the value is an empty <see cref="T:System.Collections.Specialized.NameValueCollection" />.</returns>
		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x06002EBD RID: 11965 RVA: 0x000A1558 File Offset: 0x0009F758
		// (set) Token: 0x06002EBE RID: 11966 RVA: 0x000A157D File Offset: 0x0009F77D
		public NameValueCollection QueryString
		{
			get
			{
				NameValueCollection nameValueCollection;
				if ((nameValueCollection = this._requestParameters) == null)
				{
					nameValueCollection = (this._requestParameters = new NameValueCollection());
				}
				return nameValueCollection;
			}
			set
			{
				this._requestParameters = value;
			}
		}

		/// <summary>Gets a collection of header name/value pairs associated with the response.</summary>
		/// <returns>A <see cref="T:System.Net.WebHeaderCollection" /> containing header name/value pairs associated with the response, or <see langword="null" /> if no response has been received.</returns>
		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x06002EBF RID: 11967 RVA: 0x000A1586 File Offset: 0x0009F786
		public WebHeaderCollection ResponseHeaders
		{
			get
			{
				WebResponse webResponse = this._webResponse;
				if (webResponse == null)
				{
					return null;
				}
				return webResponse.Headers;
			}
		}

		/// <summary>Gets or sets the proxy used by this <see cref="T:System.Net.WebClient" /> object.</summary>
		/// <returns>An <see cref="T:System.Net.IWebProxy" /> instance used to send requests.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <see cref="P:System.Net.WebClient.Proxy" /> is set to <see langword="null" />.</exception>
		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x06002EC0 RID: 11968 RVA: 0x000A1599 File Offset: 0x0009F799
		// (set) Token: 0x06002EC1 RID: 11969 RVA: 0x000A15AF File Offset: 0x0009F7AF
		public IWebProxy Proxy
		{
			get
			{
				if (!this._proxySet)
				{
					return WebRequest.DefaultWebProxy;
				}
				return this._proxy;
			}
			set
			{
				this._proxy = value;
				this._proxySet = true;
			}
		}

		/// <summary>Gets or sets the application's cache policy for any resources obtained by this WebClient instance using <see cref="T:System.Net.WebRequest" /> objects.</summary>
		/// <returns>A <see cref="T:System.Net.Cache.RequestCachePolicy" /> object that represents the application's caching requirements.</returns>
		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x06002EC2 RID: 11970 RVA: 0x000A15BF File Offset: 0x0009F7BF
		// (set) Token: 0x06002EC3 RID: 11971 RVA: 0x000A15C7 File Offset: 0x0009F7C7
		public RequestCachePolicy CachePolicy { get; set; }

		/// <summary>Gets whether a Web request is in progress.</summary>
		/// <returns>
		///   <see langword="true" /> if the Web request is still in progress; otherwise <see langword="false" />.</returns>
		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x06002EC4 RID: 11972 RVA: 0x000A15D0 File Offset: 0x0009F7D0
		public bool IsBusy
		{
			get
			{
				return this._asyncOp != null;
			}
		}

		/// <summary>Returns a <see cref="T:System.Net.WebRequest" /> object for the specified resource.</summary>
		/// <param name="address">A <see cref="T:System.Uri" /> that identifies the resource to request.</param>
		/// <returns>A new <see cref="T:System.Net.WebRequest" /> object for the specified resource.</returns>
		// Token: 0x06002EC5 RID: 11973 RVA: 0x000A15DC File Offset: 0x0009F7DC
		protected virtual WebRequest GetWebRequest(Uri address)
		{
			WebRequest webRequest = WebRequest.Create(address);
			this.CopyHeadersTo(webRequest);
			if (this.Credentials != null)
			{
				webRequest.Credentials = this.Credentials;
			}
			if (this._method != null)
			{
				webRequest.Method = this._method;
			}
			if (this._contentLength != -1L)
			{
				webRequest.ContentLength = this._contentLength;
			}
			if (this._proxySet)
			{
				webRequest.Proxy = this._proxy;
			}
			if (this.CachePolicy != null)
			{
				webRequest.CachePolicy = this.CachePolicy;
			}
			return webRequest;
		}

		/// <summary>Returns the <see cref="T:System.Net.WebResponse" /> for the specified <see cref="T:System.Net.WebRequest" />.</summary>
		/// <param name="request">A <see cref="T:System.Net.WebRequest" /> that is used to obtain the response.</param>
		/// <returns>A <see cref="T:System.Net.WebResponse" /> containing the response for the specified <see cref="T:System.Net.WebRequest" />.</returns>
		// Token: 0x06002EC6 RID: 11974 RVA: 0x000A1660 File Offset: 0x0009F860
		protected virtual WebResponse GetWebResponse(WebRequest request)
		{
			WebResponse response = request.GetResponse();
			this._webResponse = response;
			return response;
		}

		/// <summary>Returns the <see cref="T:System.Net.WebResponse" /> for the specified <see cref="T:System.Net.WebRequest" /> using the specified <see cref="T:System.IAsyncResult" />.</summary>
		/// <param name="request">A <see cref="T:System.Net.WebRequest" /> that is used to obtain the response.</param>
		/// <param name="result">An <see cref="T:System.IAsyncResult" /> object obtained from a previous call to <see cref="M:System.Net.WebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" /> .</param>
		/// <returns>A <see cref="T:System.Net.WebResponse" /> containing the response for the specified <see cref="T:System.Net.WebRequest" />.</returns>
		// Token: 0x06002EC7 RID: 11975 RVA: 0x000A167C File Offset: 0x0009F87C
		protected virtual WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
		{
			WebResponse webResponse = request.EndGetResponse(result);
			this._webResponse = webResponse;
			return webResponse;
		}

		// Token: 0x06002EC8 RID: 11976 RVA: 0x000A169C File Offset: 0x0009F89C
		private async Task<WebResponse> GetWebResponseTaskAsync(WebRequest request)
		{
			BeginEndAwaitableAdapter beginEndAwaitableAdapter = new BeginEndAwaitableAdapter();
			request.BeginGetResponse(BeginEndAwaitableAdapter.Callback, beginEndAwaitableAdapter);
			IAsyncResult asyncResult = await beginEndAwaitableAdapter;
			return this.GetWebResponse(request, asyncResult);
		}

		/// <summary>Downloads the resource as a <see cref="T:System.Byte" /> array from the URI specified.</summary>
		/// <param name="address">The URI from which to download data.</param>
		/// <returns>A <see cref="T:System.Byte" /> array containing the downloaded resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while downloading data.</exception>
		/// <exception cref="T:System.NotSupportedException">The method has been called simultaneously on multiple threads.</exception>
		// Token: 0x06002EC9 RID: 11977 RVA: 0x000A16E7 File Offset: 0x0009F8E7
		public byte[] DownloadData(string address)
		{
			return this.DownloadData(this.GetUri(address));
		}

		/// <summary>Downloads the resource as a <see cref="T:System.Byte" /> array from the URI specified.</summary>
		/// <param name="address">The URI represented by the <see cref="T:System.Uri" /> object, from which to download data.</param>
		/// <returns>A <see cref="T:System.Byte" /> array containing the downloaded resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06002ECA RID: 11978 RVA: 0x000A16F8 File Offset: 0x0009F8F8
		public byte[] DownloadData(Uri address)
		{
			WebClient.ThrowIfNull(address, "address");
			this.StartOperation();
			byte[] array;
			try
			{
				WebRequest webRequest;
				array = this.DownloadDataInternal(address, out webRequest);
			}
			finally
			{
				this.EndOperation();
			}
			return array;
		}

		// Token: 0x06002ECB RID: 11979 RVA: 0x000A173C File Offset: 0x0009F93C
		private byte[] DownloadDataInternal(Uri address, out WebRequest request)
		{
			request = null;
			byte[] array;
			try
			{
				request = (this._webRequest = this.GetWebRequest(this.GetUri(address)));
				array = this.DownloadBits(request, new ChunkedMemoryStream());
			}
			catch (Exception ex) when (!(ex is OutOfMemoryException))
			{
				WebClient.AbortRequest(request);
				if (ex is WebException || ex is SecurityException)
				{
					throw;
				}
				throw new WebException("An exception occurred during a WebClient request.", ex);
			}
			return array;
		}

		/// <summary>Downloads the resource with the specified URI to a local file.</summary>
		/// <param name="address">The URI from which to download data.</param>
		/// <param name="fileName">The name of the local file that is to receive the data.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="filename" /> is <see langword="null" /> or <see cref="F:System.String.Empty" />.  
		///  -or-  
		///  The file does not exist.  
		///  -or- An error occurred while downloading data.</exception>
		/// <exception cref="T:System.NotSupportedException">The method has been called simultaneously on multiple threads.</exception>
		// Token: 0x06002ECC RID: 11980 RVA: 0x000A17CC File Offset: 0x0009F9CC
		public void DownloadFile(string address, string fileName)
		{
			this.DownloadFile(this.GetUri(address), fileName);
		}

		/// <summary>Downloads the resource with the specified URI to a local file.</summary>
		/// <param name="address">The URI specified as a <see cref="T:System.String" />, from which to download data.</param>
		/// <param name="fileName">The name of the local file that is to receive the data.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="filename" /> is <see langword="null" /> or <see cref="F:System.String.Empty" />.  
		///  -or-  
		///  The file does not exist.  
		///  -or-  
		///  An error occurred while downloading data.</exception>
		/// <exception cref="T:System.NotSupportedException">The method has been called simultaneously on multiple threads.</exception>
		// Token: 0x06002ECD RID: 11981 RVA: 0x000A17DC File Offset: 0x0009F9DC
		public void DownloadFile(Uri address, string fileName)
		{
			WebClient.ThrowIfNull(address, "address");
			WebClient.ThrowIfNull(fileName, "fileName");
			WebRequest webRequest = null;
			FileStream fileStream = null;
			bool flag = false;
			this.StartOperation();
			try
			{
				fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
				webRequest = (this._webRequest = this.GetWebRequest(this.GetUri(address)));
				this.DownloadBits(webRequest, fileStream);
				flag = true;
			}
			catch (Exception ex) when (!(ex is OutOfMemoryException))
			{
				WebClient.AbortRequest(webRequest);
				if (ex is WebException || ex is SecurityException)
				{
					throw;
				}
				throw new WebException("An exception occurred during a WebClient request.", ex);
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
					if (!flag)
					{
						File.Delete(fileName);
					}
				}
				this.EndOperation();
			}
		}

		/// <summary>Opens a readable stream for the data downloaded from a resource with the URI specified as a <see cref="T:System.String" />.</summary>
		/// <param name="address">The URI specified as a <see cref="T:System.String" /> from which to download data.</param>
		/// <returns>A <see cref="T:System.IO.Stream" /> used to read data from a resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while downloading data.</exception>
		// Token: 0x06002ECE RID: 11982 RVA: 0x000A18B8 File Offset: 0x0009FAB8
		public Stream OpenRead(string address)
		{
			return this.OpenRead(this.GetUri(address));
		}

		/// <summary>Opens a readable stream for the data downloaded from a resource with the URI specified as a <see cref="T:System.Uri" /></summary>
		/// <param name="address">The URI specified as a <see cref="T:System.Uri" /> from which to download data.</param>
		/// <returns>A <see cref="T:System.IO.Stream" /> used to read data from a resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while downloading data.</exception>
		// Token: 0x06002ECF RID: 11983 RVA: 0x000A18C8 File Offset: 0x0009FAC8
		public Stream OpenRead(Uri address)
		{
			WebClient.ThrowIfNull(address, "address");
			WebRequest webRequest = null;
			this.StartOperation();
			Stream responseStream;
			try
			{
				webRequest = (this._webRequest = this.GetWebRequest(this.GetUri(address)));
				responseStream = (this._webResponse = this.GetWebResponse(webRequest)).GetResponseStream();
			}
			catch (Exception ex) when (!(ex is OutOfMemoryException))
			{
				WebClient.AbortRequest(webRequest);
				if (ex is WebException || ex is SecurityException)
				{
					throw;
				}
				throw new WebException("An exception occurred during a WebClient request.", ex);
			}
			finally
			{
				this.EndOperation();
			}
			return responseStream;
		}

		/// <summary>Opens a stream for writing data to the specified resource.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <returns>A <see cref="T:System.IO.Stream" /> used to write data to the resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while opening the stream.</exception>
		// Token: 0x06002ED0 RID: 11984 RVA: 0x000A1984 File Offset: 0x0009FB84
		public Stream OpenWrite(string address)
		{
			return this.OpenWrite(this.GetUri(address), null);
		}

		/// <summary>Opens a stream for writing data to the specified resource.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <returns>A <see cref="T:System.IO.Stream" /> used to write data to the resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while opening the stream.</exception>
		// Token: 0x06002ED1 RID: 11985 RVA: 0x000A1994 File Offset: 0x0009FB94
		public Stream OpenWrite(Uri address)
		{
			return this.OpenWrite(address, null);
		}

		/// <summary>Opens a stream for writing data to the specified resource, using the specified method.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="method">The method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <returns>A <see cref="T:System.IO.Stream" /> used to write data to the resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while opening the stream.</exception>
		// Token: 0x06002ED2 RID: 11986 RVA: 0x000A199E File Offset: 0x0009FB9E
		public Stream OpenWrite(string address, string method)
		{
			return this.OpenWrite(this.GetUri(address), method);
		}

		/// <summary>Opens a stream for writing data to the specified resource, by using the specified method.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="method">The method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <returns>A <see cref="T:System.IO.Stream" /> used to write data to the resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while opening the stream.</exception>
		// Token: 0x06002ED3 RID: 11987 RVA: 0x000A19B0 File Offset: 0x0009FBB0
		public Stream OpenWrite(Uri address, string method)
		{
			WebClient.ThrowIfNull(address, "address");
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			WebRequest webRequest = null;
			this.StartOperation();
			Stream stream;
			try
			{
				this._method = method;
				webRequest = (this._webRequest = this.GetWebRequest(this.GetUri(address)));
				stream = new WebClient.WebClientWriteStream(webRequest.GetRequestStream(), webRequest, this);
			}
			catch (Exception ex) when (!(ex is OutOfMemoryException))
			{
				WebClient.AbortRequest(webRequest);
				if (ex is WebException || ex is SecurityException)
				{
					throw;
				}
				throw new WebException("An exception occurred during a WebClient request.", ex);
			}
			finally
			{
				this.EndOperation();
			}
			return stream;
		}

		/// <summary>Uploads a data buffer to a resource identified by a URI.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="data">The data buffer to send to the resource.</param>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="data" /> is <see langword="null" />.  
		///  -or-  
		///  An error occurred while sending the data.  
		///  -or-  
		///  There was no response from the server hosting the resource.</exception>
		// Token: 0x06002ED4 RID: 11988 RVA: 0x000A1A74 File Offset: 0x0009FC74
		public byte[] UploadData(string address, byte[] data)
		{
			return this.UploadData(this.GetUri(address), null, data);
		}

		/// <summary>Uploads a data buffer to a resource identified by a URI.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="data">The data buffer to send to the resource.</param>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="data" /> is <see langword="null" />.  
		///  -or-  
		///  An error occurred while sending the data.  
		///  -or-  
		///  There was no response from the server hosting the resource.</exception>
		// Token: 0x06002ED5 RID: 11989 RVA: 0x000A1A85 File Offset: 0x0009FC85
		public byte[] UploadData(Uri address, byte[] data)
		{
			return this.UploadData(address, null, data);
		}

		/// <summary>Uploads a data buffer to the specified resource, using the specified method.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="method">The HTTP method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The data buffer to send to the resource.</param>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="data" /> is <see langword="null" />.  
		///  -or-  
		///  An error occurred while uploading the data.  
		///  -or-  
		///  There was no response from the server hosting the resource.</exception>
		// Token: 0x06002ED6 RID: 11990 RVA: 0x000A1A90 File Offset: 0x0009FC90
		public byte[] UploadData(string address, string method, byte[] data)
		{
			return this.UploadData(this.GetUri(address), method, data);
		}

		/// <summary>Uploads a data buffer to the specified resource, using the specified method.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="method">The HTTP method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The data buffer to send to the resource.</param>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="data" /> is <see langword="null" />.  
		///  -or-  
		///  An error occurred while uploading the data.  
		///  -or-  
		///  There was no response from the server hosting the resource.</exception>
		// Token: 0x06002ED7 RID: 11991 RVA: 0x000A1AA4 File Offset: 0x0009FCA4
		public byte[] UploadData(Uri address, string method, byte[] data)
		{
			WebClient.ThrowIfNull(address, "address");
			WebClient.ThrowIfNull(data, "data");
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			this.StartOperation();
			byte[] array;
			try
			{
				WebRequest webRequest;
				array = this.UploadDataInternal(address, method, data, out webRequest);
			}
			finally
			{
				this.EndOperation();
			}
			return array;
		}

		// Token: 0x06002ED8 RID: 11992 RVA: 0x000A1B00 File Offset: 0x0009FD00
		private byte[] UploadDataInternal(Uri address, string method, byte[] data, out WebRequest request)
		{
			request = null;
			byte[] array;
			try
			{
				this._method = method;
				this._contentLength = (long)data.Length;
				request = (this._webRequest = this.GetWebRequest(this.GetUri(address)));
				array = this.UploadBits(request, null, data, 0, null, null);
			}
			catch (Exception ex) when (!(ex is OutOfMemoryException))
			{
				WebClient.AbortRequest(request);
				if (ex is WebException || ex is SecurityException)
				{
					throw;
				}
				throw new WebException("An exception occurred during a WebClient request.", ex);
			}
			return array;
		}

		// Token: 0x06002ED9 RID: 11993 RVA: 0x000A1BA4 File Offset: 0x0009FDA4
		private void OpenFileInternal(bool needsHeaderAndBoundary, string fileName, ref FileStream fs, ref byte[] buffer, ref byte[] formHeaderBytes, ref byte[] boundaryBytes)
		{
			fileName = Path.GetFullPath(fileName);
			WebHeaderCollection headers = this.Headers;
			string text = headers["Content-Type"];
			if (text == null)
			{
				text = "application/octet-stream";
			}
			else if (text.StartsWith("multipart/", StringComparison.OrdinalIgnoreCase))
			{
				throw new WebException("The Content-Type header cannot be set to a multipart type for this request.");
			}
			fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
			int num = 8192;
			this._contentLength = -1L;
			if (string.Equals(this._method, "POST", StringComparison.Ordinal))
			{
				if (needsHeaderAndBoundary)
				{
					string text2 = "---------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
					headers["Content-Type"] = "multipart/form-data; boundary=" + text2;
					string text3 = string.Concat(new string[]
					{
						"--",
						text2,
						"\r\nContent-Disposition: form-data; name=\"file\"; filename=\"",
						Path.GetFileName(fileName),
						"\"\r\nContent-Type: ",
						text,
						"\r\n\r\n"
					});
					formHeaderBytes = Encoding.UTF8.GetBytes(text3);
					boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + text2 + "--\r\n");
				}
				else
				{
					formHeaderBytes = Array.Empty<byte>();
					boundaryBytes = Array.Empty<byte>();
				}
				if (fs.CanSeek)
				{
					this._contentLength = fs.Length + (long)formHeaderBytes.Length + (long)boundaryBytes.Length;
					num = (int)Math.Min(8192L, fs.Length);
				}
			}
			else
			{
				headers["Content-Type"] = text;
				formHeaderBytes = null;
				boundaryBytes = null;
				if (fs.CanSeek)
				{
					this._contentLength = fs.Length;
					num = (int)Math.Min(8192L, fs.Length);
				}
			}
			buffer = new byte[num];
		}

		/// <summary>Uploads the specified local file to a resource with the specified URI.</summary>
		/// <param name="address">The URI of the resource to receive the file. For example, ftp://localhost/samplefile.txt.</param>
		/// <param name="fileName">The file to send to the resource. For example, "samplefile.txt".</param>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="fileName" /> is <see langword="null" />, is <see cref="F:System.String.Empty" />, contains invalid characters, or does not exist.  
		///  -or-  
		///  An error occurred while uploading the file.  
		///  -or-  
		///  There was no response from the server hosting the resource.  
		///  -or-  
		///  The <see langword="Content-type" /> header begins with <see langword="multipart" />.</exception>
		// Token: 0x06002EDA RID: 11994 RVA: 0x000A1D63 File Offset: 0x0009FF63
		public byte[] UploadFile(string address, string fileName)
		{
			return this.UploadFile(this.GetUri(address), fileName);
		}

		/// <summary>Uploads the specified local file to a resource with the specified URI.</summary>
		/// <param name="address">The URI of the resource to receive the file. For example, ftp://localhost/samplefile.txt.</param>
		/// <param name="fileName">The file to send to the resource. For example, "samplefile.txt".</param>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="fileName" /> is <see langword="null" />, is <see cref="F:System.String.Empty" />, contains invalid characters, or does not exist.  
		///  -or-  
		///  An error occurred while uploading the file.  
		///  -or-  
		///  There was no response from the server hosting the resource.  
		///  -or-  
		///  The <see langword="Content-type" /> header begins with <see langword="multipart" />.</exception>
		// Token: 0x06002EDB RID: 11995 RVA: 0x000A1D73 File Offset: 0x0009FF73
		public byte[] UploadFile(Uri address, string fileName)
		{
			return this.UploadFile(address, null, fileName);
		}

		/// <summary>Uploads the specified local file to the specified resource, using the specified method.</summary>
		/// <param name="address">The URI of the resource to receive the file.</param>
		/// <param name="method">The method used to send the file to the resource. If <see langword="null" />, the default is POST for http and STOR for ftp.</param>
		/// <param name="fileName">The file to send to the resource.</param>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="fileName" /> is <see langword="null" />, is <see cref="F:System.String.Empty" />, contains invalid characters, or does not exist.  
		///  -or-  
		///  An error occurred while uploading the file.  
		///  -or-  
		///  There was no response from the server hosting the resource.  
		///  -or-  
		///  The <see langword="Content-type" /> header begins with <see langword="multipart" />.</exception>
		// Token: 0x06002EDC RID: 11996 RVA: 0x000A1D7E File Offset: 0x0009FF7E
		public byte[] UploadFile(string address, string method, string fileName)
		{
			return this.UploadFile(this.GetUri(address), method, fileName);
		}

		/// <summary>Uploads the specified local file to the specified resource, using the specified method.</summary>
		/// <param name="address">The URI of the resource to receive the file.</param>
		/// <param name="method">The method used to send the file to the resource. If <see langword="null" />, the default is POST for http and STOR for ftp.</param>
		/// <param name="fileName">The file to send to the resource.</param>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="fileName" /> is <see langword="null" />, is <see cref="F:System.String.Empty" />, contains invalid characters, or does not exist.  
		///  -or-  
		///  An error occurred while uploading the file.  
		///  -or-  
		///  There was no response from the server hosting the resource.  
		///  -or-  
		///  The <see langword="Content-type" /> header begins with <see langword="multipart" />.</exception>
		// Token: 0x06002EDD RID: 11997 RVA: 0x000A1D90 File Offset: 0x0009FF90
		public byte[] UploadFile(Uri address, string method, string fileName)
		{
			WebClient.ThrowIfNull(address, "address");
			WebClient.ThrowIfNull(fileName, "fileName");
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			FileStream fileStream = null;
			WebRequest webRequest = null;
			this.StartOperation();
			byte[] array4;
			try
			{
				this._method = method;
				byte[] array = null;
				byte[] array2 = null;
				byte[] array3 = null;
				Uri uri = this.GetUri(address);
				bool flag = uri.Scheme != Uri.UriSchemeFile;
				this.OpenFileInternal(flag, fileName, ref fileStream, ref array3, ref array, ref array2);
				webRequest = (this._webRequest = this.GetWebRequest(uri));
				array4 = this.UploadBits(webRequest, fileStream, array3, 0, array, array2);
			}
			catch (Exception ex)
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
				if (ex is OutOfMemoryException)
				{
					throw;
				}
				WebClient.AbortRequest(webRequest);
				if (ex is WebException || ex is SecurityException)
				{
					throw;
				}
				throw new WebException("An exception occurred during a WebClient request.", ex);
			}
			finally
			{
				this.EndOperation();
			}
			return array4;
		}

		// Token: 0x06002EDE RID: 11998 RVA: 0x000A1E8C File Offset: 0x000A008C
		private byte[] GetValuesToUpload(NameValueCollection data)
		{
			WebHeaderCollection headers = this.Headers;
			string text = headers["Content-Type"];
			if (text != null && !string.Equals(text, "application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase))
			{
				throw new WebException("The Content-Type header cannot be changed from its default value for this request.");
			}
			headers["Content-Type"] = "application/x-www-form-urlencoded";
			string text2 = string.Empty;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string text3 in data.AllKeys)
			{
				stringBuilder.Append(text2);
				stringBuilder.Append(WebClient.UrlEncode(text3));
				stringBuilder.Append('=');
				stringBuilder.Append(WebClient.UrlEncode(data[text3]));
				text2 = "&";
			}
			byte[] bytes = Encoding.ASCII.GetBytes(stringBuilder.ToString());
			this._contentLength = (long)bytes.Length;
			return bytes;
		}

		/// <summary>Uploads the specified name/value collection to the resource identified by the specified URI.</summary>
		/// <param name="address">The URI of the resource to receive the collection.</param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource.</param>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="data" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="data" /> is <see langword="null" />.  
		///  -or-  
		///  There was no response from the server hosting the resource.  
		///  -or-  
		///  An error occurred while opening the stream.  
		///  -or-  
		///  The <see langword="Content-type" /> header is not <see langword="null" /> or "application/x-www-form-urlencoded".</exception>
		// Token: 0x06002EDF RID: 11999 RVA: 0x000A1F59 File Offset: 0x000A0159
		public byte[] UploadValues(string address, NameValueCollection data)
		{
			return this.UploadValues(this.GetUri(address), null, data);
		}

		/// <summary>Uploads the specified name/value collection to the resource identified by the specified URI.</summary>
		/// <param name="address">The URI of the resource to receive the collection.</param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource.</param>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="data" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="data" /> is <see langword="null" />.  
		///  -or-  
		///  There was no response from the server hosting the resource.  
		///  -or-  
		///  An error occurred while opening the stream.  
		///  -or-  
		///  The <see langword="Content-type" /> header is not <see langword="null" /> or "application/x-www-form-urlencoded".</exception>
		// Token: 0x06002EE0 RID: 12000 RVA: 0x000A1F6A File Offset: 0x000A016A
		public byte[] UploadValues(Uri address, NameValueCollection data)
		{
			return this.UploadValues(address, null, data);
		}

		/// <summary>Uploads the specified name/value collection to the resource identified by the specified URI, using the specified method.</summary>
		/// <param name="address">The URI of the resource to receive the collection.</param>
		/// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource.</param>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="data" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="data" /> is <see langword="null" />.  
		///  -or-  
		///  An error occurred while opening the stream.  
		///  -or-  
		///  There was no response from the server hosting the resource.  
		///  -or-  
		///  The <see langword="Content-type" /> header value is not <see langword="null" /> and is not <see langword="application/x-www-form-urlencoded" />.</exception>
		// Token: 0x06002EE1 RID: 12001 RVA: 0x000A1F75 File Offset: 0x000A0175
		public byte[] UploadValues(string address, string method, NameValueCollection data)
		{
			return this.UploadValues(this.GetUri(address), method, data);
		}

		/// <summary>Uploads the specified name/value collection to the resource identified by the specified URI, using the specified method.</summary>
		/// <param name="address">The URI of the resource to receive the collection.</param>
		/// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource.</param>
		/// <returns>A <see cref="T:System.Byte" /> array containing the body of the response from the resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="data" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="data" /> is <see langword="null" />.  
		///  -or-  
		///  An error occurred while opening the stream.  
		///  -or-  
		///  There was no response from the server hosting the resource.  
		///  -or-  
		///  The <see langword="Content-type" /> header value is not <see langword="null" /> and is not <see langword="application/x-www-form-urlencoded" />.</exception>
		// Token: 0x06002EE2 RID: 12002 RVA: 0x000A1F88 File Offset: 0x000A0188
		public byte[] UploadValues(Uri address, string method, NameValueCollection data)
		{
			WebClient.ThrowIfNull(address, "address");
			WebClient.ThrowIfNull(data, "data");
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			WebRequest webRequest = null;
			this.StartOperation();
			byte[] array;
			try
			{
				byte[] valuesToUpload = this.GetValuesToUpload(data);
				this._method = method;
				webRequest = (this._webRequest = this.GetWebRequest(this.GetUri(address)));
				array = this.UploadBits(webRequest, null, valuesToUpload, 0, null, null);
			}
			catch (Exception ex) when (!(ex is OutOfMemoryException))
			{
				WebClient.AbortRequest(webRequest);
				if (ex is WebException || ex is SecurityException)
				{
					throw;
				}
				throw new WebException("An exception occurred during a WebClient request.", ex);
			}
			finally
			{
				this.EndOperation();
			}
			return array;
		}

		/// <summary>Uploads the specified string to the specified resource, using the POST method.</summary>
		/// <param name="address">The URI of the resource to receive the string. For Http resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
		/// <param name="data">The string to be uploaded.</param>
		/// <returns>A <see cref="T:System.String" /> containing the response sent by the server.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="data" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  There was no response from the server hosting the resource.</exception>
		// Token: 0x06002EE3 RID: 12003 RVA: 0x000A2060 File Offset: 0x000A0260
		public string UploadString(string address, string data)
		{
			return this.UploadString(this.GetUri(address), null, data);
		}

		/// <summary>Uploads the specified string to the specified resource, using the POST method.</summary>
		/// <param name="address">The URI of the resource to receive the string. For Http resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
		/// <param name="data">The string to be uploaded.</param>
		/// <returns>A <see cref="T:System.String" /> containing the response sent by the server.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="data" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  There was no response from the server hosting the resource.</exception>
		// Token: 0x06002EE4 RID: 12004 RVA: 0x000A2071 File Offset: 0x000A0271
		public string UploadString(Uri address, string data)
		{
			return this.UploadString(address, null, data);
		}

		/// <summary>Uploads the specified string to the specified resource, using the specified method.</summary>
		/// <param name="address">The URI of the resource to receive the string. This URI must identify a resource that can accept a request sent with the <paramref name="method" /> method.</param>
		/// <param name="method">The HTTP method used to send the string to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The string to be uploaded.</param>
		/// <returns>A <see cref="T:System.String" /> containing the response sent by the server.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="data" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  There was no response from the server hosting the resource.  
		///  -or-  
		///  <paramref name="method" /> cannot be used to send content.</exception>
		// Token: 0x06002EE5 RID: 12005 RVA: 0x000A207C File Offset: 0x000A027C
		public string UploadString(string address, string method, string data)
		{
			return this.UploadString(this.GetUri(address), method, data);
		}

		/// <summary>Uploads the specified string to the specified resource, using the specified method.</summary>
		/// <param name="address">The URI of the resource to receive the string. This URI must identify a resource that can accept a request sent with the <paramref name="method" /> method.</param>
		/// <param name="method">The HTTP method used to send the string to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The string to be uploaded.</param>
		/// <returns>A <see cref="T:System.String" /> containing the response sent by the server.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="data" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  There was no response from the server hosting the resource.  
		///  -or-  
		///  <paramref name="method" /> cannot be used to send content.</exception>
		// Token: 0x06002EE6 RID: 12006 RVA: 0x000A2090 File Offset: 0x000A0290
		public string UploadString(Uri address, string method, string data)
		{
			WebClient.ThrowIfNull(address, "address");
			WebClient.ThrowIfNull(data, "data");
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			this.StartOperation();
			string stringUsingEncoding;
			try
			{
				byte[] bytes = this.Encoding.GetBytes(data);
				WebRequest webRequest;
				byte[] array = this.UploadDataInternal(address, method, bytes, out webRequest);
				stringUsingEncoding = this.GetStringUsingEncoding(webRequest, array);
			}
			finally
			{
				this.EndOperation();
			}
			return stringUsingEncoding;
		}

		/// <summary>Downloads the requested resource as a <see cref="T:System.String" />. The resource to download is specified as a <see cref="T:System.String" /> containing the URI.</summary>
		/// <param name="address">A <see cref="T:System.String" /> containing the URI to download.</param>
		/// <returns>A <see cref="T:System.String" /> containing the requested resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while downloading the resource.</exception>
		/// <exception cref="T:System.NotSupportedException">The method has been called simultaneously on multiple threads.</exception>
		// Token: 0x06002EE7 RID: 12007 RVA: 0x000A2104 File Offset: 0x000A0304
		public string DownloadString(string address)
		{
			return this.DownloadString(this.GetUri(address));
		}

		/// <summary>Downloads the requested resource as a <see cref="T:System.String" />. The resource to download is specified as a <see cref="T:System.Uri" />.</summary>
		/// <param name="address">A <see cref="T:System.Uri" /> object containing the URI to download.</param>
		/// <returns>A <see cref="T:System.String" /> containing the requested resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while downloading the resource.</exception>
		/// <exception cref="T:System.NotSupportedException">The method has been called simultaneously on multiple threads.</exception>
		// Token: 0x06002EE8 RID: 12008 RVA: 0x000A2114 File Offset: 0x000A0314
		public string DownloadString(Uri address)
		{
			WebClient.ThrowIfNull(address, "address");
			this.StartOperation();
			string stringUsingEncoding;
			try
			{
				WebRequest webRequest;
				byte[] array = this.DownloadDataInternal(address, out webRequest);
				stringUsingEncoding = this.GetStringUsingEncoding(webRequest, array);
			}
			finally
			{
				this.EndOperation();
			}
			return stringUsingEncoding;
		}

		// Token: 0x06002EE9 RID: 12009 RVA: 0x000A2160 File Offset: 0x000A0360
		private static void AbortRequest(WebRequest request)
		{
			try
			{
				if (request != null)
				{
					request.Abort();
				}
			}
			catch (Exception ex) when (!(ex is OutOfMemoryException))
			{
			}
		}

		// Token: 0x06002EEA RID: 12010 RVA: 0x000A21A8 File Offset: 0x000A03A8
		private void CopyHeadersTo(WebRequest request)
		{
			if (this._headers == null)
			{
				return;
			}
			HttpWebRequest httpWebRequest = request as HttpWebRequest;
			if (httpWebRequest == null)
			{
				return;
			}
			string text = this._headers["Accept"];
			string text2 = this._headers["Connection"];
			string text3 = this._headers["Content-Type"];
			string text4 = this._headers["Expect"];
			string text5 = this._headers["Referer"];
			string text6 = this._headers["User-Agent"];
			string text7 = this._headers["Host"];
			this._headers.Remove("Accept");
			this._headers.Remove("Connection");
			this._headers.Remove("Content-Type");
			this._headers.Remove("Expect");
			this._headers.Remove("Referer");
			this._headers.Remove("User-Agent");
			this._headers.Remove("Host");
			request.Headers = this._headers;
			if (!string.IsNullOrEmpty(text))
			{
				httpWebRequest.Accept = text;
			}
			if (!string.IsNullOrEmpty(text2))
			{
				httpWebRequest.Connection = text2;
			}
			if (!string.IsNullOrEmpty(text3))
			{
				httpWebRequest.ContentType = text3;
			}
			if (!string.IsNullOrEmpty(text4))
			{
				httpWebRequest.Expect = text4;
			}
			if (!string.IsNullOrEmpty(text5))
			{
				httpWebRequest.Referer = text5;
			}
			if (!string.IsNullOrEmpty(text6))
			{
				httpWebRequest.UserAgent = text6;
			}
			if (!string.IsNullOrEmpty(text7))
			{
				httpWebRequest.Host = text7;
			}
		}

		// Token: 0x06002EEB RID: 12011 RVA: 0x000A2334 File Offset: 0x000A0534
		private Uri GetUri(string address)
		{
			WebClient.ThrowIfNull(address, "address");
			Uri uri;
			if (this._baseAddress != null)
			{
				if (!Uri.TryCreate(this._baseAddress, address, out uri))
				{
					return new Uri(Path.GetFullPath(address));
				}
			}
			else if (!Uri.TryCreate(address, UriKind.Absolute, out uri))
			{
				return new Uri(Path.GetFullPath(address));
			}
			return this.GetUri(uri);
		}

		// Token: 0x06002EEC RID: 12012 RVA: 0x000A2394 File Offset: 0x000A0594
		private Uri GetUri(Uri address)
		{
			WebClient.ThrowIfNull(address, "address");
			Uri uri = address;
			if (!address.IsAbsoluteUri && this._baseAddress != null && !Uri.TryCreate(this._baseAddress, address, out uri))
			{
				return address;
			}
			if (string.IsNullOrEmpty(uri.Query) && this._requestParameters != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				string text = string.Empty;
				for (int i = 0; i < this._requestParameters.Count; i++)
				{
					stringBuilder.Append(text).Append(this._requestParameters.AllKeys[i]).Append('=')
						.Append(this._requestParameters[i]);
					text = "&";
				}
				uri = new UriBuilder(uri)
				{
					Query = stringBuilder.ToString()
				}.Uri;
			}
			return uri;
		}

		// Token: 0x06002EED RID: 12013 RVA: 0x000A2460 File Offset: 0x000A0660
		private byte[] DownloadBits(WebRequest request, Stream writeStream)
		{
			byte[] array2;
			try
			{
				WebResponse webResponse = (this._webResponse = this.GetWebResponse(request));
				long contentLength = webResponse.ContentLength;
				byte[] array = new byte[(contentLength == -1L || contentLength > 65536L) ? 65536L : contentLength];
				if (writeStream is ChunkedMemoryStream)
				{
					if (contentLength > 2147483647L)
					{
						throw new WebException("The message length limit was exceeded", WebExceptionStatus.MessageLengthLimitExceeded);
					}
					writeStream.SetLength((long)array.Length);
				}
				using (Stream responseStream = webResponse.GetResponseStream())
				{
					if (responseStream != null)
					{
						int num;
						while ((num = responseStream.Read(array, 0, array.Length)) != 0)
						{
							writeStream.Write(array, 0, num);
						}
					}
				}
				ChunkedMemoryStream chunkedMemoryStream = writeStream as ChunkedMemoryStream;
				array2 = ((chunkedMemoryStream != null) ? chunkedMemoryStream.ToArray() : null);
			}
			catch (Exception ex) when (!(ex is OutOfMemoryException))
			{
				if (writeStream != null)
				{
					writeStream.Close();
				}
				WebClient.AbortRequest(request);
				if (ex is WebException || ex is SecurityException)
				{
					throw;
				}
				throw new WebException("An exception occurred during a WebClient request.", ex);
			}
			return array2;
		}

		// Token: 0x06002EEE RID: 12014 RVA: 0x000A2580 File Offset: 0x000A0780
		private async void DownloadBitsAsync(WebRequest request, Stream writeStream, AsyncOperation asyncOp, Action<byte[], Exception, AsyncOperation> completionDelegate)
		{
			Exception exception = null;
			try
			{
				WebResponse webResponse = await this.GetWebResponseTaskAsync(request).ConfigureAwait(false);
				WebResponse webResponse2 = webResponse;
				this._webResponse = webResponse2;
				WebResponse webResponse3 = webResponse2;
				long contentLength = webResponse3.ContentLength;
				byte[] copyBuffer = new byte[(contentLength == -1L || contentLength > 65536L) ? 65536L : contentLength];
				if (writeStream is ChunkedMemoryStream)
				{
					if (contentLength > 2147483647L)
					{
						throw new WebException("The message length limit was exceeded", WebExceptionStatus.MessageLengthLimitExceeded);
					}
					writeStream.SetLength((long)copyBuffer.Length);
				}
				if (contentLength >= 0L)
				{
					this._progress.TotalBytesToReceive = contentLength;
				}
				using (writeStream)
				{
					using (Stream readStream = webResponse3.GetResponseStream())
					{
						if (readStream != null)
						{
							for (;;)
							{
								int num = await readStream.ReadAsync(new Memory<byte>(copyBuffer), default(CancellationToken)).ConfigureAwait(false);
								if (num == 0)
								{
									break;
								}
								this._progress.BytesReceived += (long)num;
								if (this._progress.BytesReceived != this._progress.TotalBytesToReceive)
								{
									this.PostProgressChanged(asyncOp, this._progress);
								}
								await writeStream.WriteAsync(new ReadOnlyMemory<byte>(copyBuffer, 0, num), default(CancellationToken)).ConfigureAwait(false);
							}
						}
						if (this._progress.TotalBytesToReceive < 0L)
						{
							this._progress.TotalBytesToReceive = this._progress.BytesReceived;
						}
						this.PostProgressChanged(asyncOp, this._progress);
					}
					Stream readStream = null;
				}
				Stream stream = null;
				ChunkedMemoryStream chunkedMemoryStream = writeStream as ChunkedMemoryStream;
				completionDelegate((chunkedMemoryStream != null) ? chunkedMemoryStream.ToArray() : null, null, asyncOp);
				copyBuffer = null;
			}
			catch (Exception ex) when (!(ex is OutOfMemoryException))
			{
				exception = WebClient.GetExceptionToPropagate(ex);
				WebClient.AbortRequest(request);
				if (writeStream != null)
				{
					writeStream.Close();
				}
			}
			finally
			{
				if (exception != null)
				{
					completionDelegate(null, exception, asyncOp);
				}
			}
		}

		// Token: 0x06002EEF RID: 12015 RVA: 0x000A25D8 File Offset: 0x000A07D8
		private byte[] UploadBits(WebRequest request, Stream readStream, byte[] buffer, int chunkSize, byte[] header, byte[] footer)
		{
			byte[] array;
			try
			{
				if (request.RequestUri.Scheme == Uri.UriSchemeFile)
				{
					footer = (header = null);
				}
				using (Stream requestStream = request.GetRequestStream())
				{
					if (header != null)
					{
						requestStream.Write(header, 0, header.Length);
					}
					if (readStream != null)
					{
						try
						{
							for (;;)
							{
								int num = readStream.Read(buffer, 0, buffer.Length);
								if (num <= 0)
								{
									break;
								}
								requestStream.Write(buffer, 0, num);
							}
							goto IL_008F;
						}
						finally
						{
							if (readStream != null)
							{
								((IDisposable)readStream).Dispose();
							}
						}
					}
					int num2;
					for (int i = 0; i < buffer.Length; i += num2)
					{
						num2 = buffer.Length - i;
						if (chunkSize != 0 && num2 > chunkSize)
						{
							num2 = chunkSize;
						}
						requestStream.Write(buffer, i, num2);
					}
					IL_008F:
					if (footer != null)
					{
						requestStream.Write(footer, 0, footer.Length);
					}
				}
				array = this.DownloadBits(request, new ChunkedMemoryStream());
			}
			catch (Exception ex) when (!(ex is OutOfMemoryException))
			{
				WebClient.AbortRequest(request);
				if (ex is WebException || ex is SecurityException)
				{
					throw;
				}
				throw new WebException("An exception occurred during a WebClient request.", ex);
			}
			return array;
		}

		// Token: 0x06002EF0 RID: 12016 RVA: 0x000A2714 File Offset: 0x000A0914
		private async void UploadBitsAsync(WebRequest request, Stream readStream, byte[] buffer, int chunkSize, byte[] header, byte[] footer, AsyncOperation asyncOp, Action<byte[], Exception, AsyncOperation> completionDelegate)
		{
			this._progress.HasUploadPhase = true;
			Exception exception = null;
			try
			{
				if (request.RequestUri.Scheme == Uri.UriSchemeFile)
				{
					header = (footer = null);
				}
				Stream stream = await request.GetRequestStreamAsync().ConfigureAwait(false);
				using (Stream writeStream = stream)
				{
					if (header != null)
					{
						await writeStream.WriteAsync(new ReadOnlyMemory<byte>(header), default(CancellationToken)).ConfigureAwait(false);
						this._progress.BytesSent += (long)header.Length;
						this.PostProgressChanged(asyncOp, this._progress);
					}
					if (readStream != null)
					{
						using (readStream)
						{
							for (;;)
							{
								int bytesRead = await readStream.ReadAsync(new Memory<byte>(buffer), default(CancellationToken)).ConfigureAwait(false);
								if (bytesRead <= 0)
								{
									break;
								}
								await writeStream.WriteAsync(new ReadOnlyMemory<byte>(buffer, 0, bytesRead), default(CancellationToken)).ConfigureAwait(false);
								this._progress.BytesSent += (long)bytesRead;
								this.PostProgressChanged(asyncOp, this._progress);
							}
						}
						Stream stream2 = null;
					}
					else
					{
						int bytesRead = 0;
						while (bytesRead < buffer.Length)
						{
							int toWrite = buffer.Length - bytesRead;
							if (chunkSize != 0 && toWrite > chunkSize)
							{
								toWrite = chunkSize;
							}
							await writeStream.WriteAsync(new ReadOnlyMemory<byte>(buffer, bytesRead, toWrite), default(CancellationToken)).ConfigureAwait(false);
							bytesRead += toWrite;
							this._progress.BytesSent += (long)toWrite;
							this.PostProgressChanged(asyncOp, this._progress);
						}
					}
					if (footer != null)
					{
						await writeStream.WriteAsync(new ReadOnlyMemory<byte>(footer), default(CancellationToken)).ConfigureAwait(false);
						this._progress.BytesSent += (long)footer.Length;
						this.PostProgressChanged(asyncOp, this._progress);
					}
				}
				Stream writeStream = null;
				this.DownloadBitsAsync(request, new ChunkedMemoryStream(), asyncOp, completionDelegate);
			}
			catch (Exception ex) when (!(ex is OutOfMemoryException))
			{
				exception = WebClient.GetExceptionToPropagate(ex);
				WebClient.AbortRequest(request);
			}
			finally
			{
				if (exception != null)
				{
					completionDelegate(null, exception, asyncOp);
				}
			}
		}

		// Token: 0x06002EF1 RID: 12017 RVA: 0x000A2790 File Offset: 0x000A0990
		private static bool ByteArrayHasPrefix(byte[] prefix, byte[] byteArray)
		{
			if (prefix == null || byteArray == null || prefix.Length > byteArray.Length)
			{
				return false;
			}
			for (int i = 0; i < prefix.Length; i++)
			{
				if (prefix[i] != byteArray[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002EF2 RID: 12018 RVA: 0x000A27C8 File Offset: 0x000A09C8
		private string GetStringUsingEncoding(WebRequest request, byte[] data)
		{
			Encoding encoding = null;
			int num = -1;
			string text;
			try
			{
				text = request.ContentType;
			}
			catch (Exception ex) when (ex is NotImplementedException || ex is NotSupportedException)
			{
				text = null;
			}
			if (text != null)
			{
				text = text.ToLower(CultureInfo.InvariantCulture);
				string[] array = text.Split(WebClient.s_parseContentTypeSeparators);
				bool flag = false;
				foreach (string text2 in array)
				{
					if (text2 == "charset")
					{
						flag = true;
					}
					else if (flag)
					{
						try
						{
							encoding = Encoding.GetEncoding(text2);
						}
						catch (ArgumentException)
						{
							break;
						}
					}
				}
			}
			if (encoding == null)
			{
				Encoding[] array3 = WebClient.s_knownEncodings;
				for (int j = 0; j < array3.Length; j++)
				{
					byte[] preamble = array3[j].GetPreamble();
					if (WebClient.ByteArrayHasPrefix(preamble, data))
					{
						encoding = array3[j];
						num = preamble.Length;
						break;
					}
				}
			}
			if (encoding == null)
			{
				encoding = this.Encoding;
			}
			if (num == -1)
			{
				byte[] preamble2 = encoding.GetPreamble();
				num = (WebClient.ByteArrayHasPrefix(preamble2, data) ? preamble2.Length : 0);
			}
			return encoding.GetString(data, num, data.Length - num);
		}

		// Token: 0x06002EF3 RID: 12019 RVA: 0x000A28FC File Offset: 0x000A0AFC
		private string MapToDefaultMethod(Uri address)
		{
			if (!string.Equals(((!address.IsAbsoluteUri && this._baseAddress != null) ? new Uri(this._baseAddress, address) : address).Scheme, Uri.UriSchemeFtp, StringComparison.Ordinal))
			{
				return "POST";
			}
			return "STOR";
		}

		// Token: 0x06002EF4 RID: 12020 RVA: 0x000A294C File Offset: 0x000A0B4C
		private static string UrlEncode(string str)
		{
			if (str == null)
			{
				return null;
			}
			byte[] bytes = Encoding.UTF8.GetBytes(str);
			return Encoding.ASCII.GetString(WebClient.UrlEncodeBytesToBytesInternal(bytes, 0, bytes.Length, false));
		}

		// Token: 0x06002EF5 RID: 12021 RVA: 0x000A2980 File Offset: 0x000A0B80
		private static byte[] UrlEncodeBytesToBytesInternal(byte[] bytes, int offset, int count, bool alwaysCreateReturnValue)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < count; i++)
			{
				char c = (char)bytes[offset + i];
				if (c == ' ')
				{
					num++;
				}
				else if (!WebClient.IsSafe(c))
				{
					num2++;
				}
			}
			if (!alwaysCreateReturnValue && num == 0 && num2 == 0)
			{
				return bytes;
			}
			byte[] array = new byte[count + num2 * 2];
			int num3 = 0;
			for (int j = 0; j < count; j++)
			{
				byte b = bytes[offset + j];
				char c2 = (char)b;
				if (WebClient.IsSafe(c2))
				{
					array[num3++] = b;
				}
				else if (c2 == ' ')
				{
					array[num3++] = 43;
				}
				else
				{
					array[num3++] = 37;
					array[num3++] = (byte)WebClient.IntToHex((b >> 4) & 15);
					array[num3++] = (byte)WebClient.IntToHex((int)(b & 15));
				}
			}
			return array;
		}

		// Token: 0x06002EF6 RID: 12022 RVA: 0x000A2A4B File Offset: 0x000A0C4B
		private static char IntToHex(int n)
		{
			if (n <= 9)
			{
				return (char)(n + 48);
			}
			return (char)(n - 10 + 97);
		}

		// Token: 0x06002EF7 RID: 12023 RVA: 0x000A2A60 File Offset: 0x000A0C60
		private static bool IsSafe(char ch)
		{
			if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9'))
			{
				return true;
			}
			if (ch != '!')
			{
				switch (ch)
				{
				case '\'':
				case '(':
				case ')':
				case '*':
				case '-':
				case '.':
					return true;
				case '+':
				case ',':
					break;
				default:
					if (ch == '_')
					{
						return true;
					}
					break;
				}
				return false;
			}
			return true;
		}

		// Token: 0x06002EF8 RID: 12024 RVA: 0x000A2AC3 File Offset: 0x000A0CC3
		private void InvokeOperationCompleted(AsyncOperation asyncOp, SendOrPostCallback callback, AsyncCompletedEventArgs eventArgs)
		{
			if (Interlocked.CompareExchange<AsyncOperation>(ref this._asyncOp, null, asyncOp) == asyncOp)
			{
				this.EndOperation();
				asyncOp.PostOperationCompleted(callback, eventArgs);
			}
		}

		/// <summary>Opens a readable stream containing the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to retrieve.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and address is invalid.  
		///  -or-  
		///  An error occurred while downloading the resource.  
		///  -or-  
		///  An error occurred while opening the stream.</exception>
		// Token: 0x06002EF9 RID: 12025 RVA: 0x000A2AE3 File Offset: 0x000A0CE3
		public void OpenReadAsync(Uri address)
		{
			this.OpenReadAsync(address, null);
		}

		/// <summary>Opens a readable stream containing the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to retrieve.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and address is invalid.  
		///  -or-  
		///  An error occurred while downloading the resource.  
		///  -or-  
		///  An error occurred while opening the stream.</exception>
		// Token: 0x06002EFA RID: 12026 RVA: 0x000A2AF0 File Offset: 0x000A0CF0
		public void OpenReadAsync(Uri address, object userToken)
		{
			WebClient.ThrowIfNull(address, "address");
			AsyncOperation asyncOp = this.StartAsyncOperation(userToken);
			try
			{
				WebRequest request = (this._webRequest = this.GetWebRequest(this.GetUri(address)));
				request.BeginGetResponse(delegate(IAsyncResult iar)
				{
					Stream stream = null;
					Exception ex2 = null;
					try
					{
						stream = (this._webResponse = this.GetWebResponse(request, iar)).GetResponseStream();
					}
					catch (Exception ex3) when (!(ex3 is OutOfMemoryException))
					{
						ex2 = WebClient.GetExceptionToPropagate(ex3);
					}
					this.InvokeOperationCompleted(asyncOp, this._openReadOperationCompleted, new OpenReadCompletedEventArgs(stream, ex2, this._canceled, asyncOp.UserSuppliedState));
				}, null);
			}
			catch (Exception ex) when (!(ex is OutOfMemoryException))
			{
				this.InvokeOperationCompleted(asyncOp, this._openReadOperationCompleted, new OpenReadCompletedEventArgs(null, WebClient.GetExceptionToPropagate(ex), this._canceled, asyncOp.UserSuppliedState));
			}
		}

		/// <summary>Opens a stream for writing data to the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06002EFB RID: 12027 RVA: 0x000A2BBC File Offset: 0x000A0DBC
		public void OpenWriteAsync(Uri address)
		{
			this.OpenWriteAsync(address, null, null);
		}

		/// <summary>Opens a stream for writing data to the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="method">The method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		// Token: 0x06002EFC RID: 12028 RVA: 0x000A2BC7 File Offset: 0x000A0DC7
		public void OpenWriteAsync(Uri address, string method)
		{
			this.OpenWriteAsync(address, method, null);
		}

		/// <summary>Opens a stream for writing data to the specified resource, using the specified method. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="method">The method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while opening the stream.</exception>
		// Token: 0x06002EFD RID: 12029 RVA: 0x000A2BD4 File Offset: 0x000A0DD4
		public void OpenWriteAsync(Uri address, string method, object userToken)
		{
			WebClient.ThrowIfNull(address, "address");
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			AsyncOperation asyncOp = this.StartAsyncOperation(userToken);
			try
			{
				this._method = method;
				WebRequest request = (this._webRequest = this.GetWebRequest(this.GetUri(address)));
				request.BeginGetRequestStream(delegate(IAsyncResult iar)
				{
					WebClient.WebClientWriteStream webClientWriteStream = null;
					Exception ex2 = null;
					try
					{
						webClientWriteStream = new WebClient.WebClientWriteStream(request.EndGetRequestStream(iar), request, this);
					}
					catch (Exception ex3) when (!(ex3 is OutOfMemoryException))
					{
						ex2 = WebClient.GetExceptionToPropagate(ex3);
					}
					this.InvokeOperationCompleted(asyncOp, this._openWriteOperationCompleted, new OpenWriteCompletedEventArgs(webClientWriteStream, ex2, this._canceled, asyncOp.UserSuppliedState));
				}, null);
			}
			catch (Exception ex) when (!(ex is OutOfMemoryException))
			{
				OpenWriteCompletedEventArgs e = new OpenWriteCompletedEventArgs(null, WebClient.GetExceptionToPropagate(ex), this._canceled, asyncOp.UserSuppliedState);
				this.InvokeOperationCompleted(asyncOp, this._openWriteOperationCompleted, e);
			}
		}

		// Token: 0x06002EFE RID: 12030 RVA: 0x000A2CB4 File Offset: 0x000A0EB4
		private void DownloadStringAsyncCallback(byte[] returnBytes, Exception exception, object state)
		{
			AsyncOperation asyncOperation = (AsyncOperation)state;
			string text = null;
			try
			{
				if (returnBytes != null)
				{
					text = this.GetStringUsingEncoding(this._webRequest, returnBytes);
				}
			}
			catch (Exception ex) when (!(ex is OutOfMemoryException))
			{
				exception = WebClient.GetExceptionToPropagate(ex);
			}
			DownloadStringCompletedEventArgs e = new DownloadStringCompletedEventArgs(text, exception, this._canceled, asyncOperation.UserSuppliedState);
			this.InvokeOperationCompleted(asyncOperation, this._downloadStringOperationCompleted, e);
		}

		/// <summary>Downloads the resource specified as a <see cref="T:System.Uri" />. This method does not block the calling thread.</summary>
		/// <param name="address">A <see cref="T:System.Uri" /> containing the URI to download.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while downloading the resource.</exception>
		// Token: 0x06002EFF RID: 12031 RVA: 0x000A2D38 File Offset: 0x000A0F38
		public void DownloadStringAsync(Uri address)
		{
			this.DownloadStringAsync(address, null);
		}

		/// <summary>Downloads the specified string to the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">A <see cref="T:System.Uri" /> containing the URI to download.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while downloading the resource.</exception>
		// Token: 0x06002F00 RID: 12032 RVA: 0x000A2D44 File Offset: 0x000A0F44
		public void DownloadStringAsync(Uri address, object userToken)
		{
			WebClient.ThrowIfNull(address, "address");
			AsyncOperation asyncOperation = this.StartAsyncOperation(userToken);
			try
			{
				WebRequest webRequest = (this._webRequest = this.GetWebRequest(this.GetUri(address)));
				this.DownloadBitsAsync(webRequest, new ChunkedMemoryStream(), asyncOperation, new Action<byte[], Exception, AsyncOperation>(this.DownloadStringAsyncCallback));
			}
			catch (Exception ex) when (!(ex is OutOfMemoryException))
			{
				this.DownloadStringAsyncCallback(null, WebClient.GetExceptionToPropagate(ex), asyncOperation);
			}
		}

		// Token: 0x06002F01 RID: 12033 RVA: 0x000A2DD8 File Offset: 0x000A0FD8
		private void DownloadDataAsyncCallback(byte[] returnBytes, Exception exception, object state)
		{
			AsyncOperation asyncOperation = (AsyncOperation)state;
			DownloadDataCompletedEventArgs e = new DownloadDataCompletedEventArgs(returnBytes, exception, this._canceled, asyncOperation.UserSuppliedState);
			this.InvokeOperationCompleted(asyncOperation, this._downloadDataOperationCompleted, e);
		}

		/// <summary>Downloads the resource as a <see cref="T:System.Byte" /> array from the URI specified as an asynchronous operation.</summary>
		/// <param name="address">A <see cref="T:System.Uri" /> containing the URI to download.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while downloading the resource.</exception>
		// Token: 0x06002F02 RID: 12034 RVA: 0x000A2E0E File Offset: 0x000A100E
		public void DownloadDataAsync(Uri address)
		{
			this.DownloadDataAsync(address, null);
		}

		/// <summary>Downloads the resource as a <see cref="T:System.Byte" /> array from the URI specified as an asynchronous operation.</summary>
		/// <param name="address">A <see cref="T:System.Uri" /> containing the URI to download.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while downloading the resource.</exception>
		// Token: 0x06002F03 RID: 12035 RVA: 0x000A2E18 File Offset: 0x000A1018
		public void DownloadDataAsync(Uri address, object userToken)
		{
			WebClient.ThrowIfNull(address, "address");
			AsyncOperation asyncOperation = this.StartAsyncOperation(userToken);
			try
			{
				WebRequest webRequest = (this._webRequest = this.GetWebRequest(this.GetUri(address)));
				this.DownloadBitsAsync(webRequest, new ChunkedMemoryStream(), asyncOperation, new Action<byte[], Exception, AsyncOperation>(this.DownloadDataAsyncCallback));
			}
			catch (Exception ex) when (!(ex is OutOfMemoryException))
			{
				this.DownloadDataAsyncCallback(null, WebClient.GetExceptionToPropagate(ex), asyncOperation);
			}
		}

		// Token: 0x06002F04 RID: 12036 RVA: 0x000A2EAC File Offset: 0x000A10AC
		private void DownloadFileAsyncCallback(byte[] returnBytes, Exception exception, object state)
		{
			AsyncOperation asyncOperation = (AsyncOperation)state;
			AsyncCompletedEventArgs e = new AsyncCompletedEventArgs(exception, this._canceled, asyncOperation.UserSuppliedState);
			this.InvokeOperationCompleted(asyncOperation, this._downloadFileOperationCompleted, e);
		}

		/// <summary>Downloads, to a local file, the resource with the specified URI. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to download.</param>
		/// <param name="fileName">The name of the file to be placed on the local computer.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while downloading the resource.</exception>
		/// <exception cref="T:System.InvalidOperationException">The local file specified by <paramref name="fileName" /> is in use by another thread.</exception>
		// Token: 0x06002F05 RID: 12037 RVA: 0x000A2EE1 File Offset: 0x000A10E1
		public void DownloadFileAsync(Uri address, string fileName)
		{
			this.DownloadFileAsync(address, fileName, null);
		}

		/// <summary>Downloads, to a local file, the resource with the specified URI. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to download.</param>
		/// <param name="fileName">The name of the file to be placed on the local computer.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while downloading the resource.</exception>
		/// <exception cref="T:System.InvalidOperationException">The local file specified by <paramref name="fileName" /> is in use by another thread.</exception>
		// Token: 0x06002F06 RID: 12038 RVA: 0x000A2EEC File Offset: 0x000A10EC
		public void DownloadFileAsync(Uri address, string fileName, object userToken)
		{
			WebClient.ThrowIfNull(address, "address");
			WebClient.ThrowIfNull(fileName, "fileName");
			FileStream fileStream = null;
			AsyncOperation asyncOperation = this.StartAsyncOperation(userToken);
			try
			{
				fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
				WebRequest webRequest = (this._webRequest = this.GetWebRequest(this.GetUri(address)));
				this.DownloadBitsAsync(webRequest, fileStream, asyncOperation, new Action<byte[], Exception, AsyncOperation>(this.DownloadFileAsyncCallback));
			}
			catch (Exception ex) when (!(ex is OutOfMemoryException))
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
				this.DownloadFileAsyncCallback(null, WebClient.GetExceptionToPropagate(ex), asyncOperation);
			}
		}

		/// <summary>Uploads the specified string to the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the string. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
		/// <param name="data">The string to be uploaded.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="data" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  There was no response from the server hosting the resource.</exception>
		// Token: 0x06002F07 RID: 12039 RVA: 0x000A2F9C File Offset: 0x000A119C
		public void UploadStringAsync(Uri address, string data)
		{
			this.UploadStringAsync(address, null, data, null);
		}

		/// <summary>Uploads the specified string to the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the string. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
		/// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The string to be uploaded.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="data" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="method" /> cannot be used to send content.  
		///  -or-  
		///  There was no response from the server hosting the resource.</exception>
		// Token: 0x06002F08 RID: 12040 RVA: 0x000A2FA8 File Offset: 0x000A11A8
		public void UploadStringAsync(Uri address, string method, string data)
		{
			this.UploadStringAsync(address, method, data, null);
		}

		/// <summary>Uploads the specified string to the specified resource. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the string. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
		/// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The string to be uploaded.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="data" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="method" /> cannot be used to send content.  
		///  -or-  
		///  There was no response from the server hosting the resource.</exception>
		// Token: 0x06002F09 RID: 12041 RVA: 0x000A2FB4 File Offset: 0x000A11B4
		public void UploadStringAsync(Uri address, string method, string data, object userToken)
		{
			WebClient.ThrowIfNull(address, "address");
			WebClient.ThrowIfNull(data, "data");
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			AsyncOperation asyncOperation = this.StartAsyncOperation(userToken);
			try
			{
				byte[] bytes = this.Encoding.GetBytes(data);
				this._method = method;
				this._contentLength = (long)bytes.Length;
				WebRequest webRequest = (this._webRequest = this.GetWebRequest(this.GetUri(address)));
				this.UploadBitsAsync(webRequest, null, bytes, 0, null, null, asyncOperation, delegate(byte[] bytesResult, Exception error, AsyncOperation uploadAsyncOp)
				{
					string text = null;
					if (error == null && bytesResult != null)
					{
						try
						{
							text = this.GetStringUsingEncoding(this._webRequest, bytesResult);
						}
						catch (Exception ex2) when (!(ex2 is OutOfMemoryException))
						{
							error = WebClient.GetExceptionToPropagate(ex2);
						}
					}
					this.InvokeOperationCompleted(uploadAsyncOp, this._uploadStringOperationCompleted, new UploadStringCompletedEventArgs(text, error, this._canceled, uploadAsyncOp.UserSuppliedState));
				});
			}
			catch (Exception ex) when (!(ex is OutOfMemoryException))
			{
				UploadStringCompletedEventArgs e = new UploadStringCompletedEventArgs(null, WebClient.GetExceptionToPropagate(ex), this._canceled, asyncOperation.UserSuppliedState);
				this.InvokeOperationCompleted(asyncOperation, this._uploadStringOperationCompleted, e);
			}
		}

		/// <summary>Uploads a data buffer to a resource identified by a URI, using the POST method. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="data">The data buffer to send to the resource.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while opening the stream.  
		///  -or-  
		///  There was no response from the server hosting the resource.</exception>
		// Token: 0x06002F0A RID: 12042 RVA: 0x000A309C File Offset: 0x000A129C
		public void UploadDataAsync(Uri address, byte[] data)
		{
			this.UploadDataAsync(address, null, data, null);
		}

		/// <summary>Uploads a data buffer to a resource identified by a URI, using the specified method. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="method">The method used to send the data to the resource. If <see langword="null" />, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The data buffer to send to the resource.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while opening the stream.  
		///  -or-  
		///  There was no response from the server hosting the resource.</exception>
		// Token: 0x06002F0B RID: 12043 RVA: 0x000A30A8 File Offset: 0x000A12A8
		public void UploadDataAsync(Uri address, string method, byte[] data)
		{
			this.UploadDataAsync(address, method, data, null);
		}

		/// <summary>Uploads a data buffer to a resource identified by a URI, using the specified method and identifying token.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="method">The method used to send the data to the resource. If <see langword="null" />, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The data buffer to send to the resource.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while opening the stream.  
		///  -or-  
		///  There was no response from the server hosting the resource.</exception>
		// Token: 0x06002F0C RID: 12044 RVA: 0x000A30B4 File Offset: 0x000A12B4
		public void UploadDataAsync(Uri address, string method, byte[] data, object userToken)
		{
			WebClient.ThrowIfNull(address, "address");
			WebClient.ThrowIfNull(data, "data");
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			AsyncOperation asyncOp = this.StartAsyncOperation(userToken);
			try
			{
				this._method = method;
				this._contentLength = (long)data.Length;
				WebRequest webRequest = (this._webRequest = this.GetWebRequest(this.GetUri(address)));
				int num = 0;
				if (this.UploadProgressChanged != null)
				{
					num = (int)Math.Min(8192L, (long)data.Length);
				}
				this.UploadBitsAsync(webRequest, null, data, num, null, null, asyncOp, delegate(byte[] result, Exception error, AsyncOperation uploadAsyncOp)
				{
					this.InvokeOperationCompleted(asyncOp, this._uploadDataOperationCompleted, new UploadDataCompletedEventArgs(result, error, this._canceled, uploadAsyncOp.UserSuppliedState));
				});
			}
			catch (Exception ex) when (!(ex is OutOfMemoryException))
			{
				UploadDataCompletedEventArgs e = new UploadDataCompletedEventArgs(null, WebClient.GetExceptionToPropagate(ex), this._canceled, asyncOp.UserSuppliedState);
				this.InvokeOperationCompleted(asyncOp, this._uploadDataOperationCompleted, e);
			}
		}

		/// <summary>Uploads the specified local file to the specified resource, using the POST method. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the file. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
		/// <param name="fileName">The file to send to the resource.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="fileName" /> is <see langword="null" />, is <see cref="F:System.String.Empty" />, contains invalid character, or the specified path to the file does not exist.  
		///  -or-  
		///  An error occurred while opening the stream.  
		///  -or-  
		///  There was no response from the server hosting the resource.  
		///  -or-  
		///  The <see langword="Content-type" /> header begins with <see langword="multipart" />.</exception>
		// Token: 0x06002F0D RID: 12045 RVA: 0x000A31C8 File Offset: 0x000A13C8
		public void UploadFileAsync(Uri address, string fileName)
		{
			this.UploadFileAsync(address, null, fileName, null);
		}

		/// <summary>Uploads the specified local file to the specified resource, using the POST method. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the file. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
		/// <param name="method">The method used to send the data to the resource. If <see langword="null" />, the default is POST for http and STOR for ftp.</param>
		/// <param name="fileName">The file to send to the resource.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="fileName" /> is <see langword="null" />, is <see cref="F:System.String.Empty" />, contains invalid character, or the specified path to the file does not exist.  
		///  -or-  
		///  An error occurred while opening the stream.  
		///  -or-  
		///  There was no response from the server hosting the resource.  
		///  -or-  
		///  The <see langword="Content-type" /> header begins with <see langword="multipart" />.</exception>
		// Token: 0x06002F0E RID: 12046 RVA: 0x000A31D4 File Offset: 0x000A13D4
		public void UploadFileAsync(Uri address, string method, string fileName)
		{
			this.UploadFileAsync(address, method, fileName, null);
		}

		/// <summary>Uploads the specified local file to the specified resource, using the POST method. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the file. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
		/// <param name="method">The method used to send the data to the resource. If <see langword="null" />, the default is POST for http and STOR for ftp.</param>
		/// <param name="fileName">The file to send to the resource.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="fileName" /> is <see langword="null" />, is <see cref="F:System.String.Empty" />, contains invalid character, or the specified path to the file does not exist.  
		///  -or-  
		///  An error occurred while opening the stream.  
		///  -or-  
		///  There was no response from the server hosting the resource.  
		///  -or-  
		///  The <see langword="Content-type" /> header begins with <see langword="multipart" />.</exception>
		// Token: 0x06002F0F RID: 12047 RVA: 0x000A31E0 File Offset: 0x000A13E0
		public void UploadFileAsync(Uri address, string method, string fileName, object userToken)
		{
			WebClient.ThrowIfNull(address, "address");
			WebClient.ThrowIfNull(fileName, "fileName");
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			FileStream fileStream = null;
			AsyncOperation asyncOp = this.StartAsyncOperation(userToken);
			try
			{
				this._method = method;
				byte[] array = null;
				byte[] array2 = null;
				byte[] array3 = null;
				Uri uri = this.GetUri(address);
				bool flag = uri.Scheme != Uri.UriSchemeFile;
				this.OpenFileInternal(flag, fileName, ref fileStream, ref array3, ref array, ref array2);
				WebRequest webRequest = (this._webRequest = this.GetWebRequest(uri));
				this.UploadBitsAsync(webRequest, fileStream, array3, 0, array, array2, asyncOp, delegate(byte[] result, Exception error, AsyncOperation uploadAsyncOp)
				{
					this.InvokeOperationCompleted(asyncOp, this._uploadFileOperationCompleted, new UploadFileCompletedEventArgs(result, error, this._canceled, uploadAsyncOp.UserSuppliedState));
				});
			}
			catch (Exception ex) when (!(ex is OutOfMemoryException))
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
				UploadFileCompletedEventArgs e = new UploadFileCompletedEventArgs(null, WebClient.GetExceptionToPropagate(ex), this._canceled, asyncOp.UserSuppliedState);
				this.InvokeOperationCompleted(asyncOp, this._uploadFileOperationCompleted, e);
			}
		}

		/// <summary>Uploads the data in the specified name/value collection to the resource identified by the specified URI. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the collection. This URI must identify a resource that can accept a request sent with the default method.</param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="data" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  There was no response from the server hosting the resource.</exception>
		// Token: 0x06002F10 RID: 12048 RVA: 0x000A3310 File Offset: 0x000A1510
		public void UploadValuesAsync(Uri address, NameValueCollection data)
		{
			this.UploadValuesAsync(address, null, data, null);
		}

		/// <summary>Uploads the data in the specified name/value collection to the resource identified by the specified URI, using the specified method. This method does not block the calling thread.</summary>
		/// <param name="address">The URI of the resource to receive the collection. This URI must identify a resource that can accept a request sent with the <paramref name="method" /> method.</param>
		/// <param name="method">The method used to send the string to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="data" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  There was no response from the server hosting the resource.  
		///  -or-  
		///  <paramref name="method" /> cannot be used to send content.</exception>
		// Token: 0x06002F11 RID: 12049 RVA: 0x000A331C File Offset: 0x000A151C
		public void UploadValuesAsync(Uri address, string method, NameValueCollection data)
		{
			this.UploadValuesAsync(address, method, data, null);
		}

		/// <summary>Uploads the data in the specified name/value collection to the resource identified by the specified URI, using the specified method. This method does not block the calling thread, and allows the caller to pass an object to the method that is invoked when the operation completes.</summary>
		/// <param name="address">The URI of the resource to receive the collection. This URI must identify a resource that can accept a request sent with the <paramref name="method" /> method.</param>
		/// <param name="method">The HTTP method used to send the string to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource.</param>
		/// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="data" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  There was no response from the server hosting the resource.  
		///  -or-  
		///  <paramref name="method" /> cannot be used to send content.</exception>
		// Token: 0x06002F12 RID: 12050 RVA: 0x000A3328 File Offset: 0x000A1528
		public void UploadValuesAsync(Uri address, string method, NameValueCollection data, object userToken)
		{
			WebClient.ThrowIfNull(address, "address");
			WebClient.ThrowIfNull(data, "data");
			if (method == null)
			{
				method = this.MapToDefaultMethod(address);
			}
			AsyncOperation asyncOp = this.StartAsyncOperation(userToken);
			try
			{
				byte[] valuesToUpload = this.GetValuesToUpload(data);
				this._method = method;
				WebRequest webRequest = (this._webRequest = this.GetWebRequest(this.GetUri(address)));
				int num = 0;
				if (this.UploadProgressChanged != null)
				{
					num = (int)Math.Min(8192L, (long)valuesToUpload.Length);
				}
				this.UploadBitsAsync(webRequest, null, valuesToUpload, num, null, null, asyncOp, delegate(byte[] result, Exception error, AsyncOperation uploadAsyncOp)
				{
					this.InvokeOperationCompleted(asyncOp, this._uploadValuesOperationCompleted, new UploadValuesCompletedEventArgs(result, error, this._canceled, uploadAsyncOp.UserSuppliedState));
				});
			}
			catch (Exception ex) when (!(ex is OutOfMemoryException))
			{
				UploadValuesCompletedEventArgs e = new UploadValuesCompletedEventArgs(null, WebClient.GetExceptionToPropagate(ex), this._canceled, asyncOp.UserSuppliedState);
				this.InvokeOperationCompleted(asyncOp, this._uploadValuesOperationCompleted, e);
			}
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x000A343C File Offset: 0x000A163C
		private static Exception GetExceptionToPropagate(Exception e)
		{
			if (!(e is WebException) && !(e is SecurityException))
			{
				return new WebException("An exception occurred during a WebClient request.", e);
			}
			return e;
		}

		/// <summary>Cancels a pending asynchronous operation.</summary>
		// Token: 0x06002F14 RID: 12052 RVA: 0x000A345B File Offset: 0x000A165B
		public void CancelAsync()
		{
			WebRequest webRequest = this._webRequest;
			this._canceled = true;
			WebClient.AbortRequest(webRequest);
		}

		/// <summary>Downloads the resource as a <see cref="T:System.String" /> from the URI specified as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to download.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.Byte" /> array containing the downloaded resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while downloading the resource.</exception>
		// Token: 0x06002F15 RID: 12053 RVA: 0x000A346F File Offset: 0x000A166F
		public Task<string> DownloadStringTaskAsync(string address)
		{
			return this.DownloadStringTaskAsync(this.GetUri(address));
		}

		/// <summary>Downloads the resource as a <see cref="T:System.String" /> from the URI specified as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to download.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.Byte" /> array containing the downloaded resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while downloading the resource.</exception>
		// Token: 0x06002F16 RID: 12054 RVA: 0x000A3480 File Offset: 0x000A1680
		public Task<string> DownloadStringTaskAsync(Uri address)
		{
			TaskCompletionSource<string> tcs = new TaskCompletionSource<string>(address);
			DownloadStringCompletedEventHandler handler = null;
			handler = delegate(object sender, DownloadStringCompletedEventArgs e)
			{
				this.HandleCompletion<DownloadStringCompletedEventArgs, DownloadStringCompletedEventHandler, string>(tcs, e, (DownloadStringCompletedEventArgs args) => args.Result, handler, delegate(WebClient webClient, DownloadStringCompletedEventHandler completion)
				{
					webClient.DownloadStringCompleted -= completion;
				});
			};
			this.DownloadStringCompleted += handler;
			try
			{
				this.DownloadStringAsync(address, tcs);
			}
			catch
			{
				this.DownloadStringCompleted -= handler;
				throw;
			}
			return tcs.Task;
		}

		/// <summary>Opens a readable stream containing the specified resource as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to retrieve.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.IO.Stream" /> used to read data from a resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and address is invalid.  
		///  -or-  
		///  An error occurred while downloading the resource.  
		///  -or-  
		///  An error occurred while opening the stream.</exception>
		// Token: 0x06002F17 RID: 12055 RVA: 0x000A3504 File Offset: 0x000A1704
		public Task<Stream> OpenReadTaskAsync(string address)
		{
			return this.OpenReadTaskAsync(this.GetUri(address));
		}

		/// <summary>Opens a readable stream containing the specified resource as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to retrieve.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.IO.Stream" /> used to read data from a resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and address is invalid.  
		///  -or-  
		///  An error occurred while downloading the resource.  
		///  -or-  
		///  An error occurred while opening the stream.</exception>
		// Token: 0x06002F18 RID: 12056 RVA: 0x000A3514 File Offset: 0x000A1714
		public Task<Stream> OpenReadTaskAsync(Uri address)
		{
			TaskCompletionSource<Stream> tcs = new TaskCompletionSource<Stream>(address);
			OpenReadCompletedEventHandler handler = null;
			handler = delegate(object sender, OpenReadCompletedEventArgs e)
			{
				this.HandleCompletion<OpenReadCompletedEventArgs, OpenReadCompletedEventHandler, Stream>(tcs, e, (OpenReadCompletedEventArgs args) => args.Result, handler, delegate(WebClient webClient, OpenReadCompletedEventHandler completion)
				{
					webClient.OpenReadCompleted -= completion;
				});
			};
			this.OpenReadCompleted += handler;
			try
			{
				this.OpenReadAsync(address, tcs);
			}
			catch
			{
				this.OpenReadCompleted -= handler;
				throw;
			}
			return tcs.Task;
		}

		/// <summary>Opens a stream for writing data to the specified resource as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.IO.Stream" /> used to write data to the resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while opening the stream.</exception>
		// Token: 0x06002F19 RID: 12057 RVA: 0x000A3598 File Offset: 0x000A1798
		public Task<Stream> OpenWriteTaskAsync(string address)
		{
			return this.OpenWriteTaskAsync(this.GetUri(address), null);
		}

		/// <summary>Opens a stream for writing data to the specified resource as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.IO.Stream" /> used to write data to the resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while opening the stream.</exception>
		// Token: 0x06002F1A RID: 12058 RVA: 0x000A35A8 File Offset: 0x000A17A8
		public Task<Stream> OpenWriteTaskAsync(Uri address)
		{
			return this.OpenWriteTaskAsync(address, null);
		}

		/// <summary>Opens a stream for writing data to the specified resource as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="method">The method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.IO.Stream" /> used to write data to the resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while opening the stream.</exception>
		// Token: 0x06002F1B RID: 12059 RVA: 0x000A35B2 File Offset: 0x000A17B2
		public Task<Stream> OpenWriteTaskAsync(string address, string method)
		{
			return this.OpenWriteTaskAsync(this.GetUri(address), method);
		}

		/// <summary>Opens a stream for writing data to the specified resource as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="method">The method used to send the data to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.IO.Stream" /> used to write data to the resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while opening the stream.</exception>
		// Token: 0x06002F1C RID: 12060 RVA: 0x000A35C4 File Offset: 0x000A17C4
		public Task<Stream> OpenWriteTaskAsync(Uri address, string method)
		{
			TaskCompletionSource<Stream> tcs = new TaskCompletionSource<Stream>(address);
			OpenWriteCompletedEventHandler handler = null;
			handler = delegate(object sender, OpenWriteCompletedEventArgs e)
			{
				this.HandleCompletion<OpenWriteCompletedEventArgs, OpenWriteCompletedEventHandler, Stream>(tcs, e, (OpenWriteCompletedEventArgs args) => args.Result, handler, delegate(WebClient webClient, OpenWriteCompletedEventHandler completion)
				{
					webClient.OpenWriteCompleted -= completion;
				});
			};
			this.OpenWriteCompleted += handler;
			try
			{
				this.OpenWriteAsync(address, method, tcs);
			}
			catch
			{
				this.OpenWriteCompleted -= handler;
				throw;
			}
			return tcs.Task;
		}

		/// <summary>Uploads the specified string to the specified resource as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to receive the string. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
		/// <param name="data">The string to be uploaded.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.String" /> containing the response sent by the server.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="data" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  There was no response from the server hosting the resource.</exception>
		// Token: 0x06002F1D RID: 12061 RVA: 0x000A364C File Offset: 0x000A184C
		public Task<string> UploadStringTaskAsync(string address, string data)
		{
			return this.UploadStringTaskAsync(address, null, data);
		}

		/// <summary>Uploads the specified string to the specified resource as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to receive the string. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
		/// <param name="data">The string to be uploaded.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.String" /> containing the response sent by the server.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="data" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  There was no response from the server hosting the resource.</exception>
		// Token: 0x06002F1E RID: 12062 RVA: 0x000A3657 File Offset: 0x000A1857
		public Task<string> UploadStringTaskAsync(Uri address, string data)
		{
			return this.UploadStringTaskAsync(address, null, data);
		}

		/// <summary>Uploads the specified string to the specified resource as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to receive the string. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
		/// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The string to be uploaded.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.String" /> containing the response sent by the server.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="data" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="method" /> cannot be used to send content.  
		///  -or-  
		///  There was no response from the server hosting the resource.</exception>
		// Token: 0x06002F1F RID: 12063 RVA: 0x000A3662 File Offset: 0x000A1862
		public Task<string> UploadStringTaskAsync(string address, string method, string data)
		{
			return this.UploadStringTaskAsync(this.GetUri(address), method, data);
		}

		/// <summary>Uploads the specified string to the specified resource as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to receive the string. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
		/// <param name="method">The HTTP method used to send the file to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The string to be uploaded.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.String" /> containing the response sent by the server.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="data" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="method" /> cannot be used to send content.  
		///  -or-  
		///  There was no response from the server hosting the resource.</exception>
		// Token: 0x06002F20 RID: 12064 RVA: 0x000A3674 File Offset: 0x000A1874
		public Task<string> UploadStringTaskAsync(Uri address, string method, string data)
		{
			TaskCompletionSource<string> tcs = new TaskCompletionSource<string>(address);
			UploadStringCompletedEventHandler handler = null;
			handler = delegate(object sender, UploadStringCompletedEventArgs e)
			{
				this.HandleCompletion<UploadStringCompletedEventArgs, UploadStringCompletedEventHandler, string>(tcs, e, (UploadStringCompletedEventArgs args) => args.Result, handler, delegate(WebClient webClient, UploadStringCompletedEventHandler completion)
				{
					webClient.UploadStringCompleted -= completion;
				});
			};
			this.UploadStringCompleted += handler;
			try
			{
				this.UploadStringAsync(address, method, data, tcs);
			}
			catch
			{
				this.UploadStringCompleted -= handler;
				throw;
			}
			return tcs.Task;
		}

		/// <summary>Downloads the resource as a <see cref="T:System.Byte" /> array from the URI specified as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to download.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.Byte" /> array containing the downloaded resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while downloading the resource.</exception>
		// Token: 0x06002F21 RID: 12065 RVA: 0x000A36FC File Offset: 0x000A18FC
		public Task<byte[]> DownloadDataTaskAsync(string address)
		{
			return this.DownloadDataTaskAsync(this.GetUri(address));
		}

		/// <summary>Downloads the resource as a <see cref="T:System.Byte" /> array from the URI specified as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to download.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.Byte" /> array containing the downloaded resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while downloading the resource.</exception>
		// Token: 0x06002F22 RID: 12066 RVA: 0x000A370C File Offset: 0x000A190C
		public Task<byte[]> DownloadDataTaskAsync(Uri address)
		{
			TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>(address);
			DownloadDataCompletedEventHandler handler = null;
			handler = delegate(object sender, DownloadDataCompletedEventArgs e)
			{
				this.HandleCompletion<DownloadDataCompletedEventArgs, DownloadDataCompletedEventHandler, byte[]>(tcs, e, (DownloadDataCompletedEventArgs args) => args.Result, handler, delegate(WebClient webClient, DownloadDataCompletedEventHandler completion)
				{
					webClient.DownloadDataCompleted -= completion;
				});
			};
			this.DownloadDataCompleted += handler;
			try
			{
				this.DownloadDataAsync(address, tcs);
			}
			catch
			{
				this.DownloadDataCompleted -= handler;
				throw;
			}
			return tcs.Task;
		}

		/// <summary>Downloads the specified resource to a local file as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to download.</param>
		/// <param name="fileName">The name of the file to be placed on the local computer.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while downloading the resource.</exception>
		/// <exception cref="T:System.InvalidOperationException">The local file specified by <paramref name="fileName" /> is in use by another thread.</exception>
		// Token: 0x06002F23 RID: 12067 RVA: 0x000A3790 File Offset: 0x000A1990
		public Task DownloadFileTaskAsync(string address, string fileName)
		{
			return this.DownloadFileTaskAsync(this.GetUri(address), fileName);
		}

		/// <summary>Downloads the specified resource to a local file as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to download.</param>
		/// <param name="fileName">The name of the file to be placed on the local computer.</param>
		/// <returns>The task object representing the asynchronous operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while downloading the resource.</exception>
		/// <exception cref="T:System.InvalidOperationException">The local file specified by <paramref name="fileName" /> is in use by another thread.</exception>
		// Token: 0x06002F24 RID: 12068 RVA: 0x000A37A0 File Offset: 0x000A19A0
		public Task DownloadFileTaskAsync(Uri address, string fileName)
		{
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>(address);
			AsyncCompletedEventHandler handler = null;
			handler = delegate(object sender, AsyncCompletedEventArgs e)
			{
				this.HandleCompletion<AsyncCompletedEventArgs, AsyncCompletedEventHandler, object>(tcs, e, (AsyncCompletedEventArgs args) => null, handler, delegate(WebClient webClient, AsyncCompletedEventHandler completion)
				{
					webClient.DownloadFileCompleted -= completion;
				});
			};
			this.DownloadFileCompleted += handler;
			try
			{
				this.DownloadFileAsync(address, fileName, tcs);
			}
			catch
			{
				this.DownloadFileCompleted -= handler;
				throw;
			}
			return tcs.Task;
		}

		/// <summary>Uploads a data buffer that contains a <see cref="T:System.Byte" /> array to the URI specified as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="data">The data buffer to send to the resource.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.Byte" /> array containing the body of the response received from the resource when the data buffer was uploaded.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while opening the stream.  
		///  -or-  
		///  There was no response from the server hosting the resource.</exception>
		// Token: 0x06002F25 RID: 12069 RVA: 0x000A3828 File Offset: 0x000A1A28
		public Task<byte[]> UploadDataTaskAsync(string address, byte[] data)
		{
			return this.UploadDataTaskAsync(this.GetUri(address), null, data);
		}

		/// <summary>Uploads a data buffer that contains a <see cref="T:System.Byte" /> array to the URI specified as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="data">The data buffer to send to the resource.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.Byte" /> array containing the body of the response received from the resource when the data buffer was uploaded.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while opening the stream.  
		///  -or-  
		///  There was no response from the server hosting the resource.</exception>
		// Token: 0x06002F26 RID: 12070 RVA: 0x000A3839 File Offset: 0x000A1A39
		public Task<byte[]> UploadDataTaskAsync(Uri address, byte[] data)
		{
			return this.UploadDataTaskAsync(address, null, data);
		}

		/// <summary>Uploads a data buffer that contains a <see cref="T:System.Byte" /> array to the URI specified as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="method">The method used to send the data to the resource. If <see langword="null" />, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The data buffer to send to the resource.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.Byte" /> array containing the body of the response received from the resource when the data buffer was uploaded.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while opening the stream.  
		///  -or-  
		///  There was no response from the server hosting the resource.</exception>
		// Token: 0x06002F27 RID: 12071 RVA: 0x000A3844 File Offset: 0x000A1A44
		public Task<byte[]> UploadDataTaskAsync(string address, string method, byte[] data)
		{
			return this.UploadDataTaskAsync(this.GetUri(address), method, data);
		}

		/// <summary>Uploads a data buffer that contains a <see cref="T:System.Byte" /> array to the URI specified as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to receive the data.</param>
		/// <param name="method">The method used to send the data to the resource. If <see langword="null" />, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The data buffer to send to the resource.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.Byte" /> array containing the body of the response received from the resource when the data buffer was uploaded.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while opening the stream.  
		///  -or-  
		///  There was no response from the server hosting the resource.</exception>
		// Token: 0x06002F28 RID: 12072 RVA: 0x000A3858 File Offset: 0x000A1A58
		public Task<byte[]> UploadDataTaskAsync(Uri address, string method, byte[] data)
		{
			TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>(address);
			UploadDataCompletedEventHandler handler = null;
			handler = delegate(object sender, UploadDataCompletedEventArgs e)
			{
				this.HandleCompletion<UploadDataCompletedEventArgs, UploadDataCompletedEventHandler, byte[]>(tcs, e, (UploadDataCompletedEventArgs args) => args.Result, handler, delegate(WebClient webClient, UploadDataCompletedEventHandler completion)
				{
					webClient.UploadDataCompleted -= completion;
				});
			};
			this.UploadDataCompleted += handler;
			try
			{
				this.UploadDataAsync(address, method, data, tcs);
			}
			catch
			{
				this.UploadDataCompleted -= handler;
				throw;
			}
			return tcs.Task;
		}

		/// <summary>Uploads the specified local file to a resource as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to receive the file. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
		/// <param name="fileName">The local file to send to the resource.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.Byte" /> array containing the body of the response received from the resource when the file was uploaded.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="fileName" /> is <see langword="null" />, is <see cref="F:System.String.Empty" />, contains invalid character, or the specified path to the file does not exist.  
		///  -or-  
		///  An error occurred while opening the stream.  
		///  -or-  
		///  There was no response from the server hosting the resource.  
		///  -or-  
		///  The <see langword="Content-type" /> header begins with <see langword="multipart" />.</exception>
		// Token: 0x06002F29 RID: 12073 RVA: 0x000A38E0 File Offset: 0x000A1AE0
		public Task<byte[]> UploadFileTaskAsync(string address, string fileName)
		{
			return this.UploadFileTaskAsync(this.GetUri(address), null, fileName);
		}

		/// <summary>Uploads the specified local file to a resource as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to receive the file. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
		/// <param name="fileName">The local file to send to the resource.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.Byte" /> array containing the body of the response received from the resource when the file was uploaded.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="fileName" /> is <see langword="null" />, is <see cref="F:System.String.Empty" />, contains invalid character, or the specified path to the file does not exist.  
		///  -or-  
		///  An error occurred while opening the stream.  
		///  -or-  
		///  There was no response from the server hosting the resource.  
		///  -or-  
		///  The <see langword="Content-type" /> header begins with <see langword="multipart" />.</exception>
		// Token: 0x06002F2A RID: 12074 RVA: 0x000A38F1 File Offset: 0x000A1AF1
		public Task<byte[]> UploadFileTaskAsync(Uri address, string fileName)
		{
			return this.UploadFileTaskAsync(address, null, fileName);
		}

		/// <summary>Uploads the specified local file to a resource as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to receive the file. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
		/// <param name="method">The method used to send the data to the resource. If <see langword="null" />, the default is POST for http and STOR for ftp.</param>
		/// <param name="fileName">The local file to send to the resource.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.Byte" /> array containing the body of the response received from the resource when the file was uploaded.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="fileName" /> is <see langword="null" />, is <see cref="F:System.String.Empty" />, contains invalid character, or the specified path to the file does not exist.  
		///  -or-  
		///  An error occurred while opening the stream.  
		///  -or-  
		///  There was no response from the server hosting the resource.  
		///  -or-  
		///  The <see langword="Content-type" /> header begins with <see langword="multipart" />.</exception>
		// Token: 0x06002F2B RID: 12075 RVA: 0x000A38FC File Offset: 0x000A1AFC
		public Task<byte[]> UploadFileTaskAsync(string address, string method, string fileName)
		{
			return this.UploadFileTaskAsync(this.GetUri(address), method, fileName);
		}

		/// <summary>Uploads the specified local file to a resource as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to receive the file. For HTTP resources, this URI must identify a resource that can accept a request sent with the POST method, such as a script or ASP page.</param>
		/// <param name="method">The method used to send the data to the resource. If <see langword="null" />, the default is POST for http and STOR for ftp.</param>
		/// <param name="fileName">The local file to send to the resource.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.Byte" /> array containing the body of the response received from the resource when the file was uploaded.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="fileName" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" /> and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="fileName" /> is <see langword="null" />, is <see cref="F:System.String.Empty" />, contains invalid character, or the specified path to the file does not exist.  
		///  -or-  
		///  An error occurred while opening the stream.  
		///  -or-  
		///  There was no response from the server hosting the resource.  
		///  -or-  
		///  The <see langword="Content-type" /> header begins with <see langword="multipart" />.</exception>
		// Token: 0x06002F2C RID: 12076 RVA: 0x000A3910 File Offset: 0x000A1B10
		public Task<byte[]> UploadFileTaskAsync(Uri address, string method, string fileName)
		{
			TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>(address);
			UploadFileCompletedEventHandler handler = null;
			handler = delegate(object sender, UploadFileCompletedEventArgs e)
			{
				this.HandleCompletion<UploadFileCompletedEventArgs, UploadFileCompletedEventHandler, byte[]>(tcs, e, (UploadFileCompletedEventArgs args) => args.Result, handler, delegate(WebClient webClient, UploadFileCompletedEventHandler completion)
				{
					webClient.UploadFileCompleted -= completion;
				});
			};
			this.UploadFileCompleted += handler;
			try
			{
				this.UploadFileAsync(address, method, fileName, tcs);
			}
			catch
			{
				this.UploadFileCompleted -= handler;
				throw;
			}
			return tcs.Task;
		}

		/// <summary>Uploads the specified name/value collection to the resource identified by the specified URI as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to receive the collection.</param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.Byte" /> array containing the response sent by the server.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="data" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.  
		///  -or-  
		///  There was no response from the server hosting the resource.  
		///  -or-  
		///  An error occurred while opening the stream.  
		///  -or-  
		///  The <see langword="Content-type" /> header is not <see langword="null" /> or "application/x-www-form-urlencoded".</exception>
		// Token: 0x06002F2D RID: 12077 RVA: 0x000A3998 File Offset: 0x000A1B98
		public Task<byte[]> UploadValuesTaskAsync(string address, NameValueCollection data)
		{
			return this.UploadValuesTaskAsync(this.GetUri(address), null, data);
		}

		/// <summary>Uploads the specified name/value collection to the resource identified by the specified URI as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to receive the collection.</param>
		/// <param name="method">The HTTP method used to send the collection to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.Byte" /> array containing the response sent by the server.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="data" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="method" /> cannot be used to send content.  
		///  -or-  
		///  There was no response from the server hosting the resource.  
		///  -or-  
		///  An error occurred while opening the stream.  
		///  -or-  
		///  The <see langword="Content-type" /> header is not <see langword="null" /> or "application/x-www-form-urlencoded".</exception>
		// Token: 0x06002F2E RID: 12078 RVA: 0x000A39A9 File Offset: 0x000A1BA9
		public Task<byte[]> UploadValuesTaskAsync(string address, string method, NameValueCollection data)
		{
			return this.UploadValuesTaskAsync(this.GetUri(address), method, data);
		}

		/// <summary>Uploads the specified name/value collection to the resource identified by the specified URI as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to receive the collection.</param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.Byte" /> array containing the response sent by the server.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="data" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.  
		///  -or-  
		///  An error occurred while opening the stream.  
		///  -or-  
		///  There was no response from the server hosting the resource.  
		///  -or-  
		///  The <see langword="Content-type" /> header value is not <see langword="null" /> and is not <see langword="application/x-www-form-urlencoded" />.</exception>
		// Token: 0x06002F2F RID: 12079 RVA: 0x000A39BA File Offset: 0x000A1BBA
		public Task<byte[]> UploadValuesTaskAsync(Uri address, NameValueCollection data)
		{
			return this.UploadValuesTaskAsync(address, null, data);
		}

		/// <summary>Uploads the specified name/value collection to the resource identified by the specified URI as an asynchronous operation using a task object.</summary>
		/// <param name="address">The URI of the resource to receive the collection.</param>
		/// <param name="method">The HTTP method used to send the collection to the resource. If null, the default is POST for http and STOR for ftp.</param>
		/// <param name="data">The <see cref="T:System.Collections.Specialized.NameValueCollection" /> to send to the resource.</param>
		/// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="T:System.Byte" /> array containing the response sent by the server.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="address" /> parameter is <see langword="null" />.  
		///  -or-  
		///  The <paramref name="data" /> parameter is <see langword="null" />.</exception>
		/// <exception cref="T:System.Net.WebException">The URI formed by combining <see cref="P:System.Net.WebClient.BaseAddress" />, and <paramref name="address" /> is invalid.  
		///  -or-  
		///  <paramref name="method" /> cannot be used to send content.  
		///  -or-  
		///  There was no response from the server hosting the resource.  
		///  -or-  
		///  An error occurred while opening the stream.  
		///  -or-  
		///  The <see langword="Content-type" /> header is not <see langword="null" /> or "application/x-www-form-urlencoded".</exception>
		// Token: 0x06002F30 RID: 12080 RVA: 0x000A39C8 File Offset: 0x000A1BC8
		public Task<byte[]> UploadValuesTaskAsync(Uri address, string method, NameValueCollection data)
		{
			TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>(address);
			UploadValuesCompletedEventHandler handler = null;
			handler = delegate(object sender, UploadValuesCompletedEventArgs e)
			{
				this.HandleCompletion<UploadValuesCompletedEventArgs, UploadValuesCompletedEventHandler, byte[]>(tcs, e, (UploadValuesCompletedEventArgs args) => args.Result, handler, delegate(WebClient webClient, UploadValuesCompletedEventHandler completion)
				{
					webClient.UploadValuesCompleted -= completion;
				});
			};
			this.UploadValuesCompleted += handler;
			try
			{
				this.UploadValuesAsync(address, method, data, tcs);
			}
			catch
			{
				this.UploadValuesCompleted -= handler;
				throw;
			}
			return tcs.Task;
		}

		// Token: 0x06002F31 RID: 12081 RVA: 0x000A3A50 File Offset: 0x000A1C50
		private void HandleCompletion<TAsyncCompletedEventArgs, TCompletionDelegate, T>(TaskCompletionSource<T> tcs, TAsyncCompletedEventArgs e, Func<TAsyncCompletedEventArgs, T> getResult, TCompletionDelegate handler, Action<WebClient, TCompletionDelegate> unregisterHandler) where TAsyncCompletedEventArgs : AsyncCompletedEventArgs
		{
			if (e.UserState == tcs)
			{
				try
				{
					unregisterHandler(this, handler);
				}
				finally
				{
					if (e.Error != null)
					{
						tcs.TrySetException(e.Error);
					}
					else if (e.Cancelled)
					{
						tcs.TrySetCanceled();
					}
					else
					{
						tcs.TrySetResult(getResult(e));
					}
				}
			}
		}

		// Token: 0x06002F32 RID: 12082 RVA: 0x000A3AD0 File Offset: 0x000A1CD0
		private void PostProgressChanged(AsyncOperation asyncOp, WebClient.ProgressData progress)
		{
			if (asyncOp != null && (progress.BytesSent > 0L || progress.BytesReceived > 0L))
			{
				if (progress.HasUploadPhase)
				{
					if (this.UploadProgressChanged != null)
					{
						int num = ((progress.TotalBytesToReceive < 0L && progress.BytesReceived == 0L) ? ((progress.TotalBytesToSend < 0L) ? 0 : ((progress.TotalBytesToSend == 0L) ? 50 : ((int)(50L * progress.BytesSent / progress.TotalBytesToSend)))) : ((progress.TotalBytesToSend < 0L) ? 50 : ((progress.TotalBytesToReceive == 0L) ? 100 : ((int)(50L * progress.BytesReceived / progress.TotalBytesToReceive + 50L)))));
						asyncOp.Post(this._reportUploadProgressChanged, new UploadProgressChangedEventArgs(num, asyncOp.UserSuppliedState, progress.BytesSent, progress.TotalBytesToSend, progress.BytesReceived, progress.TotalBytesToReceive));
						return;
					}
				}
				else if (this.DownloadProgressChanged != null)
				{
					int num = ((progress.TotalBytesToReceive < 0L) ? 0 : ((progress.TotalBytesToReceive == 0L) ? 100 : ((int)(100L * progress.BytesReceived / progress.TotalBytesToReceive))));
					asyncOp.Post(this._reportDownloadProgressChanged, new DownloadProgressChangedEventArgs(num, asyncOp.UserSuppliedState, progress.BytesReceived, progress.TotalBytesToReceive));
				}
			}
		}

		// Token: 0x06002F33 RID: 12083 RVA: 0x000517FF File Offset: 0x0004F9FF
		private static void ThrowIfNull(object argument, string parameterName)
		{
			if (argument == null)
			{
				throw new ArgumentNullException(parameterName);
			}
		}

		/// <summary>Gets or sets a value that indicates whether to buffer the data read from the Internet resource for a <see cref="T:System.Net.WebClient" /> instance.</summary>
		/// <returns>
		///   <see langword="true" /> to enable buffering of the data received from the Internet resource; <see langword="false" /> to disable buffering. The default is <see langword="true" />.</returns>
		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x06002F34 RID: 12084 RVA: 0x000A3C09 File Offset: 0x000A1E09
		// (set) Token: 0x06002F35 RID: 12085 RVA: 0x000A3C11 File Offset: 0x000A1E11
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
		public bool AllowReadStreamBuffering { get; set; }

		/// <summary>Gets or sets a value that indicates whether to buffer the data written to the Internet resource for a <see cref="T:System.Net.WebClient" /> instance.</summary>
		/// <returns>
		///   <see langword="true" /> to enable buffering of the data written to the Internet resource; <see langword="false" /> to disable buffering. The default is <see langword="true" />.</returns>
		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x06002F36 RID: 12086 RVA: 0x000A3C1A File Offset: 0x000A1E1A
		// (set) Token: 0x06002F37 RID: 12087 RVA: 0x000A3C22 File Offset: 0x000A1E22
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
		public bool AllowWriteStreamBuffering { get; set; }

		/// <summary>Occurs when an asynchronous operation to write data to a resource using a write stream is closed.</summary>
		// Token: 0x1400006A RID: 106
		// (add) Token: 0x06002F38 RID: 12088 RVA: 0x00003917 File Offset: 0x00001B17
		// (remove) Token: 0x06002F39 RID: 12089 RVA: 0x00003917 File Offset: 0x00001B17
		[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public event WriteStreamClosedEventHandler WriteStreamClosed
		{
			add
			{
			}
			remove
			{
			}
		}

		/// <summary>Raises the <see cref="E:System.Net.WebClient.WriteStreamClosed" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Net.WriteStreamClosedEventArgs" /> object containing event data.</param>
		// Token: 0x06002F3A RID: 12090 RVA: 0x00003917 File Offset: 0x00001B17
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
		protected virtual void OnWriteStreamClosed(WriteStreamClosedEventArgs e)
		{
		}

		// Token: 0x040019AD RID: 6573
		private const int DefaultCopyBufferLength = 8192;

		// Token: 0x040019AE RID: 6574
		private const int DefaultDownloadBufferLength = 65536;

		// Token: 0x040019AF RID: 6575
		private const string DefaultUploadFileContentType = "application/octet-stream";

		// Token: 0x040019B0 RID: 6576
		private const string UploadFileContentType = "multipart/form-data";

		// Token: 0x040019B1 RID: 6577
		private const string UploadValuesContentType = "application/x-www-form-urlencoded";

		// Token: 0x040019B2 RID: 6578
		private Uri _baseAddress;

		// Token: 0x040019B3 RID: 6579
		private ICredentials _credentials;

		// Token: 0x040019B4 RID: 6580
		private WebHeaderCollection _headers;

		// Token: 0x040019B5 RID: 6581
		private NameValueCollection _requestParameters;

		// Token: 0x040019B6 RID: 6582
		private WebResponse _webResponse;

		// Token: 0x040019B7 RID: 6583
		private WebRequest _webRequest;

		// Token: 0x040019B8 RID: 6584
		private Encoding _encoding = Encoding.Default;

		// Token: 0x040019B9 RID: 6585
		private string _method;

		// Token: 0x040019BA RID: 6586
		private long _contentLength = -1L;

		// Token: 0x040019BB RID: 6587
		private bool _initWebClientAsync;

		// Token: 0x040019BC RID: 6588
		private bool _canceled;

		// Token: 0x040019BD RID: 6589
		private WebClient.ProgressData _progress;

		// Token: 0x040019BE RID: 6590
		private IWebProxy _proxy;

		// Token: 0x040019BF RID: 6591
		private bool _proxySet;

		// Token: 0x040019C0 RID: 6592
		private int _callNesting;

		// Token: 0x040019C1 RID: 6593
		private AsyncOperation _asyncOp;

		// Token: 0x040019C2 RID: 6594
		private SendOrPostCallback _downloadDataOperationCompleted;

		// Token: 0x040019C3 RID: 6595
		private SendOrPostCallback _openReadOperationCompleted;

		// Token: 0x040019C4 RID: 6596
		private SendOrPostCallback _openWriteOperationCompleted;

		// Token: 0x040019C5 RID: 6597
		private SendOrPostCallback _downloadStringOperationCompleted;

		// Token: 0x040019C6 RID: 6598
		private SendOrPostCallback _downloadFileOperationCompleted;

		// Token: 0x040019C7 RID: 6599
		private SendOrPostCallback _uploadStringOperationCompleted;

		// Token: 0x040019C8 RID: 6600
		private SendOrPostCallback _uploadDataOperationCompleted;

		// Token: 0x040019C9 RID: 6601
		private SendOrPostCallback _uploadFileOperationCompleted;

		// Token: 0x040019CA RID: 6602
		private SendOrPostCallback _uploadValuesOperationCompleted;

		// Token: 0x040019CB RID: 6603
		private SendOrPostCallback _reportDownloadProgressChanged;

		// Token: 0x040019CC RID: 6604
		private SendOrPostCallback _reportUploadProgressChanged;

		// Token: 0x040019D9 RID: 6617
		private static readonly char[] s_parseContentTypeSeparators = new char[] { ';', '=', ' ' };

		// Token: 0x040019DA RID: 6618
		private static readonly Encoding[] s_knownEncodings = new Encoding[]
		{
			Encoding.UTF8,
			Encoding.UTF32,
			Encoding.Unicode,
			Encoding.BigEndianUnicode
		};

		// Token: 0x02000598 RID: 1432
		private sealed class ProgressData
		{
			// Token: 0x06002F48 RID: 12104 RVA: 0x000A3D94 File Offset: 0x000A1F94
			internal void Reset()
			{
				this.BytesSent = 0L;
				this.TotalBytesToSend = -1L;
				this.BytesReceived = 0L;
				this.TotalBytesToReceive = -1L;
				this.HasUploadPhase = false;
			}

			// Token: 0x040019DD RID: 6621
			internal long BytesSent;

			// Token: 0x040019DE RID: 6622
			internal long TotalBytesToSend = -1L;

			// Token: 0x040019DF RID: 6623
			internal long BytesReceived;

			// Token: 0x040019E0 RID: 6624
			internal long TotalBytesToReceive = -1L;

			// Token: 0x040019E1 RID: 6625
			internal bool HasUploadPhase;
		}

		// Token: 0x02000599 RID: 1433
		private sealed class WebClientWriteStream : DelegatingStream
		{
			// Token: 0x06002F4A RID: 12106 RVA: 0x000A3DD5 File Offset: 0x000A1FD5
			public WebClientWriteStream(Stream stream, WebRequest request, WebClient webClient)
				: base(stream)
			{
				this._request = request;
				this._webClient = webClient;
			}

			// Token: 0x06002F4B RID: 12107 RVA: 0x000A3DEC File Offset: 0x000A1FEC
			protected override void Dispose(bool disposing)
			{
				try
				{
					if (disposing)
					{
						this._webClient.GetWebResponse(this._request).Dispose();
					}
				}
				finally
				{
					base.Dispose(disposing);
				}
			}

			// Token: 0x040019E2 RID: 6626
			private readonly WebRequest _request;

			// Token: 0x040019E3 RID: 6627
			private readonly WebClient _webClient;
		}
	}
}
