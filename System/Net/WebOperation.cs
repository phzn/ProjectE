using System;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
	// Token: 0x020006C8 RID: 1736
	internal class WebOperation
	{
		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x060037CE RID: 14286 RVA: 0x000C4696 File Offset: 0x000C2896
		public HttpWebRequest Request { get; }

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x060037CF RID: 14287 RVA: 0x000C469E File Offset: 0x000C289E
		// (set) Token: 0x060037D0 RID: 14288 RVA: 0x000C46A6 File Offset: 0x000C28A6
		public WebConnection Connection { get; private set; }

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x060037D1 RID: 14289 RVA: 0x000C46AF File Offset: 0x000C28AF
		// (set) Token: 0x060037D2 RID: 14290 RVA: 0x000C46B7 File Offset: 0x000C28B7
		public ServicePoint ServicePoint { get; private set; }

		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x060037D3 RID: 14291 RVA: 0x000C46C0 File Offset: 0x000C28C0
		public BufferOffsetSize WriteBuffer { get; }

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x060037D4 RID: 14292 RVA: 0x000C46C8 File Offset: 0x000C28C8
		public bool IsNtlmChallenge { get; }

		// Token: 0x17000BB2 RID: 2994
		// (get) Token: 0x060037D5 RID: 14293 RVA: 0x00002F6A File Offset: 0x0000116A
		internal string ME
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060037D6 RID: 14294 RVA: 0x000C46D0 File Offset: 0x000C28D0
		public WebOperation(HttpWebRequest request, BufferOffsetSize writeBuffer, bool isNtlmChallenge, CancellationToken cancellationToken)
		{
			this.Request = request;
			this.WriteBuffer = writeBuffer;
			this.IsNtlmChallenge = isNtlmChallenge;
			this.cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
			this.requestTask = new WebCompletionSource<WebRequestStream>(true);
			this.requestWrittenTask = new WebCompletionSource<WebRequestStream>(true);
			this.responseTask = new WebCompletionSource<WebResponseStream>(true);
			this.finishedTask = new WebCompletionSource<ValueTuple<bool, WebOperation>>(true);
		}

		// Token: 0x17000BB3 RID: 2995
		// (get) Token: 0x060037D7 RID: 14295 RVA: 0x000C4735 File Offset: 0x000C2935
		public bool Aborted
		{
			get
			{
				return this.disposedInfo != null || this.Request.Aborted || (this.cts != null && this.cts.IsCancellationRequested);
			}
		}

		// Token: 0x17000BB4 RID: 2996
		// (get) Token: 0x060037D8 RID: 14296 RVA: 0x000C4766 File Offset: 0x000C2966
		public bool Closed
		{
			get
			{
				return this.Aborted || this.closedInfo != null;
			}
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x000C477B File Offset: 0x000C297B
		public void Abort()
		{
			if (!this.SetDisposed(ref this.disposedInfo).Item2)
			{
				return;
			}
			CancellationTokenSource cancellationTokenSource = this.cts;
			if (cancellationTokenSource != null)
			{
				cancellationTokenSource.Cancel();
			}
			this.SetCanceled();
			this.Close();
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x000C47B0 File Offset: 0x000C29B0
		public void Close()
		{
			if (!this.SetDisposed(ref this.closedInfo).Item2)
			{
				return;
			}
			WebRequestStream webRequestStream = Interlocked.Exchange<WebRequestStream>(ref this.writeStream, null);
			if (webRequestStream != null)
			{
				try
				{
					webRequestStream.Close();
				}
				catch
				{
				}
			}
		}

		// Token: 0x060037DB RID: 14299 RVA: 0x000C47FC File Offset: 0x000C29FC
		private void SetCanceled()
		{
			OperationCanceledException ex = new OperationCanceledException();
			this.requestTask.TrySetCanceled(ex);
			this.requestWrittenTask.TrySetCanceled(ex);
			this.responseTask.TrySetCanceled(ex);
			this.Finish(false, ex);
		}

		// Token: 0x060037DC RID: 14300 RVA: 0x000C483E File Offset: 0x000C2A3E
		private void SetError(Exception error)
		{
			this.requestTask.TrySetException(error);
			this.requestWrittenTask.TrySetException(error);
			this.responseTask.TrySetException(error);
			this.Finish(false, error);
		}

		// Token: 0x060037DD RID: 14301 RVA: 0x000C4870 File Offset: 0x000C2A70
		private ValueTuple<ExceptionDispatchInfo, bool> SetDisposed(ref ExceptionDispatchInfo field)
		{
			ExceptionDispatchInfo exceptionDispatchInfo = ExceptionDispatchInfo.Capture(new WebException(SR.GetString("The request was canceled"), WebExceptionStatus.RequestCanceled));
			ExceptionDispatchInfo exceptionDispatchInfo2 = Interlocked.CompareExchange<ExceptionDispatchInfo>(ref field, exceptionDispatchInfo, null);
			return new ValueTuple<ExceptionDispatchInfo, bool>(exceptionDispatchInfo2 ?? exceptionDispatchInfo, exceptionDispatchInfo2 == null);
		}

		// Token: 0x060037DE RID: 14302 RVA: 0x000C48AB File Offset: 0x000C2AAB
		internal ExceptionDispatchInfo CheckDisposed(CancellationToken cancellationToken)
		{
			if (this.Aborted || cancellationToken.IsCancellationRequested)
			{
				return this.CheckThrowDisposed(false, ref this.disposedInfo);
			}
			return null;
		}

		// Token: 0x060037DF RID: 14303 RVA: 0x000C48CD File Offset: 0x000C2ACD
		internal void ThrowIfDisposed()
		{
			this.ThrowIfDisposed(CancellationToken.None);
		}

		// Token: 0x060037E0 RID: 14304 RVA: 0x000C48DA File Offset: 0x000C2ADA
		internal void ThrowIfDisposed(CancellationToken cancellationToken)
		{
			if (this.Aborted || cancellationToken.IsCancellationRequested)
			{
				this.CheckThrowDisposed(true, ref this.disposedInfo);
			}
		}

		// Token: 0x060037E1 RID: 14305 RVA: 0x000C48FB File Offset: 0x000C2AFB
		internal void ThrowIfClosedOrDisposed()
		{
			this.ThrowIfClosedOrDisposed(CancellationToken.None);
		}

		// Token: 0x060037E2 RID: 14306 RVA: 0x000C4908 File Offset: 0x000C2B08
		internal void ThrowIfClosedOrDisposed(CancellationToken cancellationToken)
		{
			if (this.Closed || cancellationToken.IsCancellationRequested)
			{
				this.CheckThrowDisposed(true, ref this.closedInfo);
			}
		}

		// Token: 0x060037E3 RID: 14307 RVA: 0x000C492C File Offset: 0x000C2B2C
		private ExceptionDispatchInfo CheckThrowDisposed(bool throwIt, ref ExceptionDispatchInfo field)
		{
			ValueTuple<ExceptionDispatchInfo, bool> valueTuple = this.SetDisposed(ref field);
			ExceptionDispatchInfo item = valueTuple.Item1;
			if (valueTuple.Item2)
			{
				CancellationTokenSource cancellationTokenSource = this.cts;
				if (cancellationTokenSource != null)
				{
					cancellationTokenSource.Cancel();
				}
			}
			if (throwIt)
			{
				item.Throw();
			}
			return item;
		}

		// Token: 0x060037E4 RID: 14308 RVA: 0x000C496C File Offset: 0x000C2B6C
		internal void RegisterRequest(ServicePoint servicePoint, WebConnection connection)
		{
			if (servicePoint == null)
			{
				throw new ArgumentNullException("servicePoint");
			}
			if (connection == null)
			{
				throw new ArgumentNullException("connection");
			}
			lock (this)
			{
				if (Interlocked.CompareExchange(ref this.requestSent, 1, 0) != 0)
				{
					throw new InvalidOperationException("Invalid nested call.");
				}
				this.ServicePoint = servicePoint;
				this.Connection = connection;
			}
			this.cts.Token.Register(delegate
			{
				this.Request.FinishedReading = true;
				this.SetDisposed(ref this.disposedInfo);
			});
		}

		// Token: 0x060037E5 RID: 14309 RVA: 0x000C4A08 File Offset: 0x000C2C08
		public void SetPriorityRequest(WebOperation operation)
		{
			lock (this)
			{
				if (this.requestSent != 1 || this.ServicePoint == null || this.finished != 0)
				{
					throw new InvalidOperationException("Should never happen.");
				}
				if (Interlocked.CompareExchange<WebOperation>(ref this.priorityRequest, operation, null) != null)
				{
					throw new InvalidOperationException("Invalid nested request.");
				}
			}
		}

		// Token: 0x060037E6 RID: 14310 RVA: 0x000C4A7C File Offset: 0x000C2C7C
		public async Task<Stream> GetRequestStream()
		{
			return await this.requestTask.WaitForCompletion().ConfigureAwait(false);
		}

		// Token: 0x060037E7 RID: 14311 RVA: 0x000C4ABF File Offset: 0x000C2CBF
		internal Task<WebRequestStream> GetRequestStreamInternal()
		{
			return this.requestTask.WaitForCompletion();
		}

		// Token: 0x060037E8 RID: 14312 RVA: 0x000C4ACC File Offset: 0x000C2CCC
		public Task WaitUntilRequestWritten()
		{
			return this.requestWrittenTask.WaitForCompletion();
		}

		// Token: 0x17000BB5 RID: 2997
		// (get) Token: 0x060037E9 RID: 14313 RVA: 0x000C4AD9 File Offset: 0x000C2CD9
		public WebRequestStream WriteStream
		{
			get
			{
				this.ThrowIfDisposed();
				return this.writeStream;
			}
		}

		// Token: 0x060037EA RID: 14314 RVA: 0x000C4AE7 File Offset: 0x000C2CE7
		public Task<WebResponseStream> GetResponseStream()
		{
			return this.responseTask.WaitForCompletion();
		}

		// Token: 0x17000BB6 RID: 2998
		// (get) Token: 0x060037EB RID: 14315 RVA: 0x000C4AF4 File Offset: 0x000C2CF4
		internal WebCompletionSource<ValueTuple<bool, WebOperation>> Finished
		{
			get
			{
				return this.finishedTask;
			}
		}

		// Token: 0x060037EC RID: 14316 RVA: 0x000C4AFC File Offset: 0x000C2CFC
		internal async void Run()
		{
			try
			{
				this.ThrowIfClosedOrDisposed();
				WebRequestStream webRequestStream = await this.Connection.InitConnection(this, this.cts.Token).ConfigureAwait(false);
				WebRequestStream requestStream = webRequestStream;
				this.ThrowIfClosedOrDisposed();
				this.writeStream = requestStream;
				await requestStream.Initialize(this.cts.Token).ConfigureAwait(false);
				this.ThrowIfClosedOrDisposed();
				this.requestTask.TrySetCompleted(requestStream);
				WebResponseStream stream = new WebResponseStream(requestStream);
				this.responseStream = stream;
				await stream.InitReadAsync(this.cts.Token).ConfigureAwait(false);
				this.responseTask.TrySetCompleted(stream);
				requestStream = null;
				stream = null;
			}
			catch (OperationCanceledException)
			{
				this.SetCanceled();
			}
			catch (Exception ex)
			{
				this.SetError(ex);
			}
		}

		// Token: 0x060037ED RID: 14317 RVA: 0x000C4B33 File Offset: 0x000C2D33
		internal void CompleteRequestWritten(WebRequestStream stream, Exception error = null)
		{
			if (error != null)
			{
				this.SetError(error);
				return;
			}
			this.requestWrittenTask.TrySetCompleted(stream);
		}

		// Token: 0x060037EE RID: 14318 RVA: 0x000C4B50 File Offset: 0x000C2D50
		internal void Finish(bool ok, Exception error = null)
		{
			if (Interlocked.CompareExchange(ref this.finished, 1, 0) != 0)
			{
				return;
			}
			WebResponseStream webResponseStream;
			WebOperation webOperation;
			lock (this)
			{
				webResponseStream = Interlocked.Exchange<WebResponseStream>(ref this.responseStream, null);
				webOperation = Interlocked.Exchange<WebOperation>(ref this.priorityRequest, null);
				this.Request.FinishedReading = true;
			}
			if (error != null)
			{
				if (webOperation != null)
				{
					webOperation.SetError(error);
				}
				this.finishedTask.TrySetException(error);
				return;
			}
			bool flag2 = !this.Aborted && ok && webResponseStream != null && webResponseStream.KeepAlive;
			if (webOperation != null && webOperation.Aborted)
			{
				webOperation = null;
				flag2 = false;
			}
			this.finishedTask.TrySetCompleted(new ValueTuple<bool, WebOperation>(flag2, webOperation));
		}

		// Token: 0x040020AA RID: 8362
		internal readonly int ID;

		// Token: 0x040020AB RID: 8363
		private CancellationTokenSource cts;

		// Token: 0x040020AC RID: 8364
		private WebCompletionSource<WebRequestStream> requestTask;

		// Token: 0x040020AD RID: 8365
		private WebCompletionSource<WebRequestStream> requestWrittenTask;

		// Token: 0x040020AE RID: 8366
		private WebCompletionSource<WebResponseStream> responseTask;

		// Token: 0x040020AF RID: 8367
		private WebCompletionSource<ValueTuple<bool, WebOperation>> finishedTask;

		// Token: 0x040020B0 RID: 8368
		private WebRequestStream writeStream;

		// Token: 0x040020B1 RID: 8369
		private WebResponseStream responseStream;

		// Token: 0x040020B2 RID: 8370
		private ExceptionDispatchInfo disposedInfo;

		// Token: 0x040020B3 RID: 8371
		private ExceptionDispatchInfo closedInfo;

		// Token: 0x040020B4 RID: 8372
		private WebOperation priorityRequest;

		// Token: 0x040020B5 RID: 8373
		private int requestSent;

		// Token: 0x040020B6 RID: 8374
		private int finished;
	}
}
