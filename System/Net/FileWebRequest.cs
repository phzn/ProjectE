using System;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System.Net
{
	/// <summary>Provides a file system implementation of the <see cref="T:System.Net.WebRequest" /> class.</summary>
	// Token: 0x02000657 RID: 1623
	[Serializable]
	public class FileWebRequest : WebRequest, ISerializable
	{
		// Token: 0x0600331E RID: 13086 RVA: 0x000B2550 File Offset: 0x000B0750
		internal FileWebRequest(Uri uri)
		{
			if (uri.Scheme != Uri.UriSchemeFile)
			{
				throw new ArgumentOutOfRangeException("uri");
			}
			this.m_uri = uri;
			this.m_fileAccess = FileAccess.Read;
			this.m_headers = new WebHeaderCollection(WebHeaderCollectionType.FileWebRequest);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.FileWebRequest" /> class from the specified instances of the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> classes.</summary>
		/// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that contains the information that is required to serialize the new <see cref="T:System.Net.FileWebRequest" /> object.</param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> object that contains the source of the serialized stream that is associated with the new <see cref="T:System.Net.FileWebRequest" /> object.</param>
		// Token: 0x0600331F RID: 13087 RVA: 0x000B25AC File Offset: 0x000B07AC
		[Obsolete("Serialization is obsoleted for this type. http://go.microsoft.com/fwlink/?linkid=14202")]
		protected FileWebRequest(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
			this.m_headers = (WebHeaderCollection)serializationInfo.GetValue("headers", typeof(WebHeaderCollection));
			this.m_proxy = (IWebProxy)serializationInfo.GetValue("proxy", typeof(IWebProxy));
			this.m_uri = (Uri)serializationInfo.GetValue("uri", typeof(Uri));
			this.m_connectionGroupName = serializationInfo.GetString("connectionGroupName");
			this.m_method = serializationInfo.GetString("method");
			this.m_contentLength = serializationInfo.GetInt64("contentLength");
			this.m_timeout = serializationInfo.GetInt32("timeout");
			this.m_fileAccess = (FileAccess)serializationInfo.GetInt32("fileAccess");
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the required data to serialize the <see cref="T:System.Net.FileWebRequest" />.</summary>
		/// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized data for the <see cref="T:System.Net.FileWebRequest" />.</param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains the destination of the serialized stream that is associated with the new <see cref="T:System.Net.FileWebRequest" />.</param>
		// Token: 0x06003320 RID: 13088 RVA: 0x000AB65E File Offset: 0x000A985E
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data needed to serialize the target object.</summary>
		/// <param name="serializationInfo">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that specifies the destination for this serialization.</param>
		// Token: 0x06003321 RID: 13089 RVA: 0x000B268C File Offset: 0x000B088C
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		protected override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			serializationInfo.AddValue("headers", this.m_headers, typeof(WebHeaderCollection));
			serializationInfo.AddValue("proxy", this.m_proxy, typeof(IWebProxy));
			serializationInfo.AddValue("uri", this.m_uri, typeof(Uri));
			serializationInfo.AddValue("connectionGroupName", this.m_connectionGroupName);
			serializationInfo.AddValue("method", this.m_method);
			serializationInfo.AddValue("contentLength", this.m_contentLength);
			serializationInfo.AddValue("timeout", this.m_timeout);
			serializationInfo.AddValue("fileAccess", this.m_fileAccess);
			serializationInfo.AddValue("preauthenticate", false);
			base.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x06003322 RID: 13090 RVA: 0x000B2758 File Offset: 0x000B0958
		internal bool Aborted
		{
			get
			{
				return this.m_Aborted != 0;
			}
		}

		/// <summary>Gets or sets the name of the connection group for the request. This property is reserved for future use.</summary>
		/// <returns>The name of the connection group for the request.</returns>
		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x06003323 RID: 13091 RVA: 0x000B2763 File Offset: 0x000B0963
		// (set) Token: 0x06003324 RID: 13092 RVA: 0x000B276B File Offset: 0x000B096B
		public override string ConnectionGroupName
		{
			get
			{
				return this.m_connectionGroupName;
			}
			set
			{
				this.m_connectionGroupName = value;
			}
		}

		/// <summary>Gets or sets the content length of the data being sent.</summary>
		/// <returns>The number of bytes of request data being sent.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <see cref="P:System.Net.FileWebRequest.ContentLength" /> is less than 0.</exception>
		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x06003325 RID: 13093 RVA: 0x000B2774 File Offset: 0x000B0974
		// (set) Token: 0x06003326 RID: 13094 RVA: 0x000B277C File Offset: 0x000B097C
		public override long ContentLength
		{
			get
			{
				return this.m_contentLength;
			}
			set
			{
				if (value < 0L)
				{
					throw new ArgumentException(SR.GetString("The Content-Length value must be greater than or equal to zero."), "value");
				}
				this.m_contentLength = value;
			}
		}

		/// <summary>Gets or sets the content type of the data being sent. This property is reserved for future use.</summary>
		/// <returns>The content type of the data being sent.</returns>
		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x06003327 RID: 13095 RVA: 0x000B279F File Offset: 0x000B099F
		// (set) Token: 0x06003328 RID: 13096 RVA: 0x000B27B1 File Offset: 0x000B09B1
		public override string ContentType
		{
			get
			{
				return this.m_headers["Content-Type"];
			}
			set
			{
				this.m_headers["Content-Type"] = value;
			}
		}

		/// <summary>Gets or sets the credentials that are associated with this request. This property is reserved for future use.</summary>
		/// <returns>An <see cref="T:System.Net.ICredentials" /> that contains the authentication credentials that are associated with this request. The default is <see langword="null" />.</returns>
		// Token: 0x17000A55 RID: 2645
		// (get) Token: 0x06003329 RID: 13097 RVA: 0x000B27C4 File Offset: 0x000B09C4
		// (set) Token: 0x0600332A RID: 13098 RVA: 0x000B27CC File Offset: 0x000B09CC
		public override ICredentials Credentials
		{
			get
			{
				return this.m_credentials;
			}
			set
			{
				this.m_credentials = value;
			}
		}

		/// <summary>Gets a collection of the name/value pairs that are associated with the request. This property is reserved for future use.</summary>
		/// <returns>A <see cref="T:System.Net.WebHeaderCollection" /> that contains header name/value pairs associated with this request.</returns>
		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x0600332B RID: 13099 RVA: 0x000B27D5 File Offset: 0x000B09D5
		public override WebHeaderCollection Headers
		{
			get
			{
				return this.m_headers;
			}
		}

		/// <summary>Gets or sets the protocol method used for the request. This property is reserved for future use.</summary>
		/// <returns>The protocol method to use in this request.</returns>
		/// <exception cref="T:System.ArgumentException">The method is invalid.  
		/// -or-
		///  The method is not supported.  
		/// -or-
		///  Multiple methods were specified.</exception>
		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x0600332C RID: 13100 RVA: 0x000B27DD File Offset: 0x000B09DD
		// (set) Token: 0x0600332D RID: 13101 RVA: 0x000B27E5 File Offset: 0x000B09E5
		public override string Method
		{
			get
			{
				return this.m_method;
			}
			set
			{
				if (ValidationHelper.IsBlankString(value))
				{
					throw new ArgumentException(SR.GetString("Cannot set null or blank methods on request."), "value");
				}
				this.m_method = value;
			}
		}

		/// <summary>Gets or sets a value that indicates whether to preauthenticate a request. This property is reserved for future use.</summary>
		/// <returns>
		///   <see langword="true" /> to preauthenticate; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x0600332E RID: 13102 RVA: 0x000B280B File Offset: 0x000B0A0B
		// (set) Token: 0x0600332F RID: 13103 RVA: 0x000B2813 File Offset: 0x000B0A13
		public override bool PreAuthenticate
		{
			get
			{
				return this.m_preauthenticate;
			}
			set
			{
				this.m_preauthenticate = true;
			}
		}

		/// <summary>Gets or sets the network proxy to use for this request. This property is reserved for future use.</summary>
		/// <returns>An <see cref="T:System.Net.IWebProxy" /> that indicates the network proxy to use for this request.</returns>
		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x06003330 RID: 13104 RVA: 0x000B281C File Offset: 0x000B0A1C
		// (set) Token: 0x06003331 RID: 13105 RVA: 0x000B2824 File Offset: 0x000B0A24
		public override IWebProxy Proxy
		{
			get
			{
				return this.m_proxy;
			}
			set
			{
				this.m_proxy = value;
			}
		}

		/// <summary>Gets or sets the length of time until the request times out.</summary>
		/// <returns>The time, in milliseconds, until the request times out, or the value <see cref="F:System.Threading.Timeout.Infinite" /> to indicate that the request does not time out.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value specified is less than or equal to zero and is not <see cref="F:System.Threading.Timeout.Infinite" />.</exception>
		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x06003332 RID: 13106 RVA: 0x000B282D File Offset: 0x000B0A2D
		// (set) Token: 0x06003333 RID: 13107 RVA: 0x000B2835 File Offset: 0x000B0A35
		public override int Timeout
		{
			get
			{
				return this.m_timeout;
			}
			set
			{
				if (value < 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException("value", SR.GetString("Timeout can be only be set to 'System.Threading.Timeout.Infinite' or a value >= 0."));
				}
				this.m_timeout = value;
			}
		}

		/// <summary>Gets the Uniform Resource Identifier (URI) of the request.</summary>
		/// <returns>A <see cref="T:System.Uri" /> that contains the URI of the request.</returns>
		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x06003334 RID: 13108 RVA: 0x000B285B File Offset: 0x000B0A5B
		public override Uri RequestUri
		{
			get
			{
				return this.m_uri;
			}
		}

		/// <summary>Begins an asynchronous request for a <see cref="T:System.IO.Stream" /> object to use to write data.</summary>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate.</param>
		/// <param name="state">An object that contains state information for this request.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous request.</returns>
		/// <exception cref="T:System.Net.ProtocolViolationException">The <see cref="P:System.Net.FileWebRequest.Method" /> property is <c>GET</c> and the application writes to the stream.</exception>
		/// <exception cref="T:System.InvalidOperationException">The stream is being used by a previous call to <see cref="M:System.Net.FileWebRequest.BeginGetRequestStream(System.AsyncCallback,System.Object)" />.</exception>
		/// <exception cref="T:System.ApplicationException">No write stream is available.</exception>
		/// <exception cref="T:System.Net.WebException">The <see cref="T:System.Net.FileWebRequest" /> was aborted.</exception>
		// Token: 0x06003335 RID: 13109 RVA: 0x000B2864 File Offset: 0x000B0A64
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
		{
			try
			{
				if (this.Aborted)
				{
					throw ExceptionHelper.RequestAbortedException;
				}
				if (!this.CanGetRequestStream())
				{
					throw new ProtocolViolationException(SR.GetString("Cannot send a content-body with this verb-type."));
				}
				if (this.m_response != null)
				{
					throw new InvalidOperationException(SR.GetString("This operation cannot be performed after the request has been submitted."));
				}
				lock (this)
				{
					if (this.m_writePending)
					{
						throw new InvalidOperationException(SR.GetString("Cannot re-call BeginGetRequestStream/BeginGetResponse while a previous call is still in progress."));
					}
					this.m_writePending = true;
				}
				this.m_ReadAResult = new LazyAsyncResult(this, state, callback);
				ThreadPool.QueueUserWorkItem(FileWebRequest.s_GetRequestStreamCallback, this.m_ReadAResult);
			}
			catch (Exception)
			{
				bool on = Logging.On;
				throw;
			}
			finally
			{
			}
			return this.m_ReadAResult;
		}

		/// <summary>Begins an asynchronous request for a file system resource.</summary>
		/// <param name="callback">The <see cref="T:System.AsyncCallback" /> delegate.</param>
		/// <param name="state">An object that contains state information for this request.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that references the asynchronous request.</returns>
		/// <exception cref="T:System.InvalidOperationException">The stream is already in use by a previous call to <see cref="M:System.Net.FileWebRequest.BeginGetResponse(System.AsyncCallback,System.Object)" />.</exception>
		/// <exception cref="T:System.Net.WebException">The <see cref="T:System.Net.FileWebRequest" /> was aborted.</exception>
		// Token: 0x06003336 RID: 13110 RVA: 0x000B2940 File Offset: 0x000B0B40
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			try
			{
				if (this.Aborted)
				{
					throw ExceptionHelper.RequestAbortedException;
				}
				lock (this)
				{
					if (this.m_readPending)
					{
						throw new InvalidOperationException(SR.GetString("Cannot re-call BeginGetRequestStream/BeginGetResponse while a previous call is still in progress."));
					}
					this.m_readPending = true;
				}
				this.m_WriteAResult = new LazyAsyncResult(this, state, callback);
				ThreadPool.QueueUserWorkItem(FileWebRequest.s_GetResponseCallback, this.m_WriteAResult);
			}
			catch (Exception)
			{
				bool on = Logging.On;
				throw;
			}
			finally
			{
			}
			return this.m_WriteAResult;
		}

		// Token: 0x06003337 RID: 13111 RVA: 0x000B29EC File Offset: 0x000B0BEC
		private bool CanGetRequestStream()
		{
			return !KnownHttpVerb.Parse(this.m_method).ContentBodyNotAllowed;
		}

		/// <summary>Ends an asynchronous request for a <see cref="T:System.IO.Stream" /> instance that the application uses to write data.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that references the pending request for a stream.</param>
		/// <returns>A <see cref="T:System.IO.Stream" /> object that the application uses to write data.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is <see langword="null" />.</exception>
		// Token: 0x06003338 RID: 13112 RVA: 0x000B2A04 File Offset: 0x000B0C04
		public override Stream EndGetRequestStream(IAsyncResult asyncResult)
		{
			Stream stream;
			try
			{
				LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
				if (asyncResult == null || lazyAsyncResult == null)
				{
					throw (asyncResult == null) ? new ArgumentNullException("asyncResult") : new ArgumentException(SR.GetString("The AsyncResult is not valid."), "asyncResult");
				}
				object obj = lazyAsyncResult.InternalWaitForCompletion();
				if (obj is Exception)
				{
					throw (Exception)obj;
				}
				stream = (Stream)obj;
				this.m_writePending = false;
			}
			catch (Exception)
			{
				bool on = Logging.On;
				throw;
			}
			finally
			{
			}
			return stream;
		}

		/// <summary>Ends an asynchronous request for a file system resource.</summary>
		/// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> that references the pending request for a response.</param>
		/// <returns>A <see cref="T:System.Net.WebResponse" /> that contains the response from the file system resource.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is <see langword="null" />.</exception>
		// Token: 0x06003339 RID: 13113 RVA: 0x000B2A90 File Offset: 0x000B0C90
		public override WebResponse EndGetResponse(IAsyncResult asyncResult)
		{
			WebResponse webResponse;
			try
			{
				LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
				if (asyncResult == null || lazyAsyncResult == null)
				{
					throw (asyncResult == null) ? new ArgumentNullException("asyncResult") : new ArgumentException(SR.GetString("The AsyncResult is not valid."), "asyncResult");
				}
				object obj = lazyAsyncResult.InternalWaitForCompletion();
				if (obj is Exception)
				{
					throw (Exception)obj;
				}
				webResponse = (WebResponse)obj;
				this.m_readPending = false;
			}
			catch (Exception)
			{
				bool on = Logging.On;
				throw;
			}
			finally
			{
			}
			return webResponse;
		}

		/// <summary>Returns a <see cref="T:System.IO.Stream" /> object for writing data to the file system resource.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> for writing data to the file system resource.</returns>
		/// <exception cref="T:System.Net.WebException">The request times out.</exception>
		// Token: 0x0600333A RID: 13114 RVA: 0x000B2B1C File Offset: 0x000B0D1C
		public override Stream GetRequestStream()
		{
			IAsyncResult asyncResult;
			try
			{
				asyncResult = this.BeginGetRequestStream(null, null);
				if (this.Timeout != -1 && !asyncResult.IsCompleted && (!asyncResult.AsyncWaitHandle.WaitOne(this.Timeout, false) || !asyncResult.IsCompleted))
				{
					if (this.m_stream != null)
					{
						this.m_stream.Close();
					}
					throw new WebException(NetRes.GetWebStatusString(WebExceptionStatus.Timeout), WebExceptionStatus.Timeout);
				}
			}
			catch (Exception)
			{
				bool on = Logging.On;
				throw;
			}
			finally
			{
			}
			return this.EndGetRequestStream(asyncResult);
		}

		/// <summary>Returns a response to a file system request.</summary>
		/// <returns>A <see cref="T:System.Net.WebResponse" /> that contains the response from the file system resource.</returns>
		/// <exception cref="T:System.Net.WebException">The request timed out.</exception>
		// Token: 0x0600333B RID: 13115 RVA: 0x000B2BB0 File Offset: 0x000B0DB0
		public override WebResponse GetResponse()
		{
			this.m_syncHint = true;
			IAsyncResult asyncResult;
			try
			{
				asyncResult = this.BeginGetResponse(null, null);
				if (this.Timeout != -1 && !asyncResult.IsCompleted && (!asyncResult.AsyncWaitHandle.WaitOne(this.Timeout, false) || !asyncResult.IsCompleted))
				{
					if (this.m_response != null)
					{
						this.m_response.Close();
					}
					throw new WebException(NetRes.GetWebStatusString(WebExceptionStatus.Timeout), WebExceptionStatus.Timeout);
				}
			}
			catch (Exception)
			{
				bool on = Logging.On;
				throw;
			}
			finally
			{
			}
			return this.EndGetResponse(asyncResult);
		}

		// Token: 0x0600333C RID: 13116 RVA: 0x000B2C4C File Offset: 0x000B0E4C
		private static void GetRequestStreamCallback(object state)
		{
			LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)state;
			FileWebRequest fileWebRequest = (FileWebRequest)lazyAsyncResult.AsyncObject;
			try
			{
				if (fileWebRequest.m_stream == null)
				{
					fileWebRequest.m_stream = new FileWebStream(fileWebRequest, fileWebRequest.m_uri.LocalPath, FileMode.Create, FileAccess.Write, FileShare.Read);
					fileWebRequest.m_fileAccess = FileAccess.Write;
					fileWebRequest.m_writing = true;
				}
			}
			catch (Exception ex)
			{
				Exception ex2 = new WebException(ex.Message, ex);
				lazyAsyncResult.InvokeCallback(ex2);
				return;
			}
			lazyAsyncResult.InvokeCallback(fileWebRequest.m_stream);
		}

		// Token: 0x0600333D RID: 13117 RVA: 0x000B2CD4 File Offset: 0x000B0ED4
		private static void GetResponseCallback(object state)
		{
			LazyAsyncResult lazyAsyncResult = (LazyAsyncResult)state;
			FileWebRequest fileWebRequest = (FileWebRequest)lazyAsyncResult.AsyncObject;
			if (fileWebRequest.m_writePending || fileWebRequest.m_writing)
			{
				FileWebRequest fileWebRequest2 = fileWebRequest;
				lock (fileWebRequest2)
				{
					if (fileWebRequest.m_writePending || fileWebRequest.m_writing)
					{
						fileWebRequest.m_readerEvent = new ManualResetEvent(false);
					}
				}
			}
			if (fileWebRequest.m_readerEvent != null)
			{
				fileWebRequest.m_readerEvent.WaitOne();
			}
			try
			{
				if (fileWebRequest.m_response == null)
				{
					fileWebRequest.m_response = new FileWebResponse(fileWebRequest, fileWebRequest.m_uri, fileWebRequest.m_fileAccess, !fileWebRequest.m_syncHint);
				}
			}
			catch (Exception ex)
			{
				Exception ex2 = new WebException(ex.Message, ex);
				lazyAsyncResult.InvokeCallback(ex2);
				return;
			}
			lazyAsyncResult.InvokeCallback(fileWebRequest.m_response);
		}

		// Token: 0x0600333E RID: 13118 RVA: 0x000B2DBC File Offset: 0x000B0FBC
		internal void UnblockReader()
		{
			lock (this)
			{
				if (this.m_readerEvent != null)
				{
					this.m_readerEvent.Set();
				}
			}
			this.m_writing = false;
		}

		/// <summary>Always throws a <see cref="T:System.NotSupportedException" />.</summary>
		/// <returns>Always throws a <see cref="T:System.NotSupportedException" />.</returns>
		/// <exception cref="T:System.NotSupportedException">Default credentials are not supported for file Uniform Resource Identifiers (URIs).</exception>
		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x0600333F RID: 13119 RVA: 0x000A0568 File Offset: 0x0009E768
		// (set) Token: 0x06003340 RID: 13120 RVA: 0x000A0568 File Offset: 0x0009E768
		public override bool UseDefaultCredentials
		{
			get
			{
				throw ExceptionHelper.PropertyNotSupportedException;
			}
			set
			{
				throw ExceptionHelper.PropertyNotSupportedException;
			}
		}

		/// <summary>Cancels a request to an Internet resource.</summary>
		// Token: 0x06003341 RID: 13121 RVA: 0x000B2E0C File Offset: 0x000B100C
		public override void Abort()
		{
			bool on = Logging.On;
			try
			{
				if (Interlocked.Increment(ref this.m_Aborted) == 1)
				{
					LazyAsyncResult readAResult = this.m_ReadAResult;
					LazyAsyncResult writeAResult = this.m_WriteAResult;
					WebException ex = new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
					Stream stream = this.m_stream;
					if (readAResult != null && !readAResult.IsCompleted)
					{
						readAResult.InvokeCallback(ex);
					}
					if (writeAResult != null && !writeAResult.IsCompleted)
					{
						writeAResult.InvokeCallback(ex);
					}
					if (stream != null)
					{
						if (stream is ICloseEx)
						{
							((ICloseEx)stream).CloseEx(CloseExState.Abort);
						}
						else
						{
							stream.Close();
						}
					}
					if (this.m_response != null)
					{
						((ICloseEx)this.m_response).CloseEx(CloseExState.Abort);
					}
				}
			}
			catch (Exception)
			{
				bool on2 = Logging.On;
				throw;
			}
			finally
			{
			}
		}

		// Token: 0x04001DFB RID: 7675
		private static WaitCallback s_GetRequestStreamCallback = new WaitCallback(FileWebRequest.GetRequestStreamCallback);

		// Token: 0x04001DFC RID: 7676
		private static WaitCallback s_GetResponseCallback = new WaitCallback(FileWebRequest.GetResponseCallback);

		// Token: 0x04001DFD RID: 7677
		private string m_connectionGroupName;

		// Token: 0x04001DFE RID: 7678
		private long m_contentLength;

		// Token: 0x04001DFF RID: 7679
		private ICredentials m_credentials;

		// Token: 0x04001E00 RID: 7680
		private FileAccess m_fileAccess;

		// Token: 0x04001E01 RID: 7681
		private WebHeaderCollection m_headers;

		// Token: 0x04001E02 RID: 7682
		private string m_method = "GET";

		// Token: 0x04001E03 RID: 7683
		private bool m_preauthenticate;

		// Token: 0x04001E04 RID: 7684
		private IWebProxy m_proxy;

		// Token: 0x04001E05 RID: 7685
		private ManualResetEvent m_readerEvent;

		// Token: 0x04001E06 RID: 7686
		private bool m_readPending;

		// Token: 0x04001E07 RID: 7687
		private WebResponse m_response;

		// Token: 0x04001E08 RID: 7688
		private Stream m_stream;

		// Token: 0x04001E09 RID: 7689
		private bool m_syncHint;

		// Token: 0x04001E0A RID: 7690
		private int m_timeout = 100000;

		// Token: 0x04001E0B RID: 7691
		private Uri m_uri;

		// Token: 0x04001E0C RID: 7692
		private bool m_writePending;

		// Token: 0x04001E0D RID: 7693
		private bool m_writing;

		// Token: 0x04001E0E RID: 7694
		private LazyAsyncResult m_WriteAResult;

		// Token: 0x04001E0F RID: 7695
		private LazyAsyncResult m_ReadAResult;

		// Token: 0x04001E10 RID: 7696
		private int m_Aborted;
	}
}
