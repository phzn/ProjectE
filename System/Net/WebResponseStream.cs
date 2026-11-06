using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
	// Token: 0x020006D6 RID: 1750
	internal class WebResponseStream : WebConnectionStream
	{
		// Token: 0x17000BC6 RID: 3014
		// (get) Token: 0x06003835 RID: 14389 RVA: 0x000C6B66 File Offset: 0x000C4D66
		public WebRequestStream RequestStream { get; }

		// Token: 0x17000BC7 RID: 3015
		// (get) Token: 0x06003836 RID: 14390 RVA: 0x000C6B6E File Offset: 0x000C4D6E
		// (set) Token: 0x06003837 RID: 14391 RVA: 0x000C6B76 File Offset: 0x000C4D76
		public WebHeaderCollection Headers { get; private set; }

		// Token: 0x17000BC8 RID: 3016
		// (get) Token: 0x06003838 RID: 14392 RVA: 0x000C6B7F File Offset: 0x000C4D7F
		// (set) Token: 0x06003839 RID: 14393 RVA: 0x000C6B87 File Offset: 0x000C4D87
		public HttpStatusCode StatusCode { get; private set; }

		// Token: 0x17000BC9 RID: 3017
		// (get) Token: 0x0600383A RID: 14394 RVA: 0x000C6B90 File Offset: 0x000C4D90
		// (set) Token: 0x0600383B RID: 14395 RVA: 0x000C6B98 File Offset: 0x000C4D98
		public string StatusDescription { get; private set; }

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x0600383C RID: 14396 RVA: 0x000C6BA1 File Offset: 0x000C4DA1
		// (set) Token: 0x0600383D RID: 14397 RVA: 0x000C6BA9 File Offset: 0x000C4DA9
		public Version Version { get; private set; }

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x0600383E RID: 14398 RVA: 0x000C6BB2 File Offset: 0x000C4DB2
		// (set) Token: 0x0600383F RID: 14399 RVA: 0x000C6BBA File Offset: 0x000C4DBA
		public bool KeepAlive { get; private set; }

		// Token: 0x06003840 RID: 14400 RVA: 0x000C6BC3 File Offset: 0x000C4DC3
		public WebResponseStream(WebRequestStream request)
			: base(request.Connection, request.Operation)
		{
			this.RequestStream = request;
		}

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x06003841 RID: 14401 RVA: 0x0000390E File Offset: 0x00001B0E
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x06003842 RID: 14402 RVA: 0x00003062 File Offset: 0x00001262
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x06003843 RID: 14403 RVA: 0x000C6BE9 File Offset: 0x000C4DE9
		// (set) Token: 0x06003844 RID: 14404 RVA: 0x000C6BF1 File Offset: 0x000C4DF1
		private bool ChunkedRead { get; set; }

		// Token: 0x06003845 RID: 14405 RVA: 0x000C6BFC File Offset: 0x000C4DFC
		public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			int num = buffer.Length;
			if (offset < 0 || num < offset)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || num - offset < count)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (Interlocked.CompareExchange(ref this.nestedRead, 1, 0) != 0)
			{
				throw new InvalidOperationException("Invalid nested call.");
			}
			WebCompletionSource completion = new WebCompletionSource();
			while (!cancellationToken.IsCancellationRequested)
			{
				WebCompletionSource webCompletionSource = Interlocked.CompareExchange<WebCompletionSource>(ref this.pendingRead, completion, null);
				if (webCompletionSource == null)
				{
					break;
				}
				await webCompletionSource.WaitForCompletion().ConfigureAwait(false);
			}
			int nbytes = 0;
			Exception throwMe = null;
			try
			{
				nbytes = await this.ProcessRead(buffer, offset, count, cancellationToken).ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				throwMe = this.GetReadException(WebExceptionStatus.ReceiveFailure, ex, "ReadAsync");
			}
			object obj;
			if (throwMe != null)
			{
				obj = this.locker;
				lock (obj)
				{
					completion.TrySetException(throwMe);
					this.pendingRead = null;
					this.nestedRead = 0;
				}
				this.closed = true;
				base.Operation.Finish(false, throwMe);
				throw throwMe;
			}
			obj = this.locker;
			lock (obj)
			{
				completion.TrySetCompleted();
				this.pendingRead = null;
				this.nestedRead = 0;
			}
			if (nbytes <= 0 && !this.read_eof)
			{
				this.read_eof = true;
				if (!this.nextReadCalled && !this.nextReadCalled)
				{
					this.nextReadCalled = true;
					base.Operation.Finish(true, null);
				}
			}
			return nbytes;
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x000C6C60 File Offset: 0x000C4E60
		private Task<int> ProcessRead(byte[] buffer, int offset, int size, CancellationToken cancellationToken)
		{
			if (this.read_eof)
			{
				return Task.FromResult<int>(0);
			}
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<int>(cancellationToken);
			}
			return HttpWebRequest.RunWithTimeout<int>((CancellationToken ct) => this.innerStream.ReadAsync(buffer, offset, size, ct), this.ReadTimeout, delegate
			{
				this.Operation.Abort();
				this.innerStream.Dispose();
			}, () => this.Operation.Aborted, cancellationToken);
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x000C6CE0 File Offset: 0x000C4EE0
		protected override bool TryReadFromBufferedContent(byte[] buffer, int offset, int count, out int result)
		{
			if (this.bufferedEntireContent)
			{
				BufferedReadStream bufferedReadStream = this.innerStream as BufferedReadStream;
				if (bufferedReadStream != null)
				{
					return bufferedReadStream.TryReadFromBuffer(buffer, offset, count, out result);
				}
			}
			result = 0;
			return false;
		}

		// Token: 0x06003848 RID: 14408 RVA: 0x000C6D18 File Offset: 0x000C4F18
		private bool CheckAuthHeader(string headerName)
		{
			string text = this.Headers[headerName];
			return text != null && text.IndexOf("NTLM", StringComparison.Ordinal) != -1;
		}

		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x06003849 RID: 14409 RVA: 0x000C6D4C File Offset: 0x000C4F4C
		private bool ExpectContent
		{
			get
			{
				return !(base.Request.Method == "HEAD") && (this.StatusCode >= HttpStatusCode.OK && this.StatusCode != HttpStatusCode.NoContent) && this.StatusCode != HttpStatusCode.NotModified;
			}
		}

		// Token: 0x0600384A RID: 14410 RVA: 0x000C6DA0 File Offset: 0x000C4FA0
		private void Initialize(BufferOffsetSize buffer)
		{
			string text = this.Headers["Transfer-Encoding"];
			bool flag = text != null && text.IndexOf("chunked", StringComparison.OrdinalIgnoreCase) != -1;
			string text2 = this.Headers["Content-Length"];
			long num;
			if (!flag && !string.IsNullOrEmpty(text2))
			{
				if (!long.TryParse(text2, out num))
				{
					num = long.MaxValue;
				}
			}
			else
			{
				num = long.MaxValue;
			}
			string text3 = null;
			if (this.ExpectContent)
			{
				text3 = this.Headers["Transfer-Encoding"];
			}
			this.ChunkedRead = text3 != null && text3.IndexOf("chunked", StringComparison.OrdinalIgnoreCase) != -1;
			if (this.Version == HttpVersion.Version11 && this.RequestStream.KeepAlive)
			{
				this.KeepAlive = true;
				string text4 = this.Headers[base.ServicePoint.UsesProxy ? "Proxy-Connection" : "Connection"];
				if (text4 != null)
				{
					text4 = text4.ToLower();
					this.KeepAlive = text4.IndexOf("keep-alive", StringComparison.Ordinal) != -1;
					if (text4.IndexOf("close", StringComparison.Ordinal) != -1)
					{
						this.KeepAlive = false;
					}
				}
				if (!this.ChunkedRead && num == 9223372036854775807L)
				{
					this.KeepAlive = false;
				}
			}
			Stream stream;
			if (!this.ExpectContent || (!this.ChunkedRead && (long)buffer.Size >= num))
			{
				this.bufferedEntireContent = true;
				this.innerStream = new BufferedReadStream(base.Operation, null, buffer);
				stream = this.innerStream;
			}
			else if (buffer.Size > 0)
			{
				stream = new BufferedReadStream(base.Operation, this.RequestStream.InnerStream, buffer);
			}
			else
			{
				stream = this.RequestStream.InnerStream;
			}
			if (this.ChunkedRead)
			{
				this.innerStream = new MonoChunkStream(base.Operation, stream, this.Headers);
			}
			else if (!this.bufferedEntireContent)
			{
				if (num != 9223372036854775807L)
				{
					this.innerStream = new FixedSizeReadStream(base.Operation, stream, num);
				}
				else
				{
					this.innerStream = new BufferedReadStream(base.Operation, stream, null);
				}
			}
			string text5 = this.Headers["Content-Encoding"];
			if (text5 == "gzip" && (base.Request.AutomaticDecompression & DecompressionMethods.GZip) != DecompressionMethods.None)
			{
				this.innerStream = ContentDecodeStream.Create(base.Operation, this.innerStream, ContentDecodeStream.Mode.GZip);
				this.Headers.Remove(HttpRequestHeader.ContentEncoding);
			}
			else if (text5 == "deflate" && (base.Request.AutomaticDecompression & DecompressionMethods.Deflate) != DecompressionMethods.None)
			{
				this.innerStream = ContentDecodeStream.Create(base.Operation, this.innerStream, ContentDecodeStream.Mode.Deflate);
				this.Headers.Remove(HttpRequestHeader.ContentEncoding);
			}
			if (!this.ExpectContent)
			{
				this.nextReadCalled = true;
				base.Operation.Finish(true, null);
			}
		}

		// Token: 0x0600384B RID: 14411 RVA: 0x000C7078 File Offset: 0x000C5278
		private async Task<byte[]> ReadAllAsyncInner(CancellationToken cancellationToken)
		{
			long maximumSize = (long)HttpWebRequest.DefaultMaximumErrorResponseLength << 16;
			byte[] array;
			using (MemoryStream ms = new MemoryStream())
			{
				while (ms.Position < maximumSize)
				{
					cancellationToken.ThrowIfCancellationRequested();
					byte[] buffer = new byte[16384];
					int num = await this.ProcessRead(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);
					if (num < 0)
					{
						throw new IOException();
					}
					if (num == 0)
					{
						break;
					}
					ms.Write(buffer, 0, num);
					buffer = null;
				}
				array = ms.ToArray();
			}
			return array;
		}

		// Token: 0x0600384C RID: 14412 RVA: 0x000C70C4 File Offset: 0x000C52C4
		internal async Task ReadAllAsync(bool resending, CancellationToken cancellationToken)
		{
			if (this.read_eof || this.bufferedEntireContent || this.nextReadCalled)
			{
				if (!this.nextReadCalled)
				{
					this.nextReadCalled = true;
					base.Operation.Finish(true, null);
				}
			}
			else
			{
				WebCompletionSource completion = new WebCompletionSource();
				CancellationTokenSource timeoutCts = new CancellationTokenSource();
				try
				{
					Task timeoutTask = Task.Delay(this.ReadTimeout, timeoutCts.Token);
					ConfiguredTaskAwaitable<Task>.ConfiguredTaskAwaiter configuredTaskAwaiter;
					do
					{
						cancellationToken.ThrowIfCancellationRequested();
						WebCompletionSource webCompletionSource = Interlocked.CompareExchange<WebCompletionSource>(ref this.pendingRead, completion, null);
						if (webCompletionSource == null)
						{
							goto IL_0147;
						}
						Task<object> task = webCompletionSource.WaitForCompletion();
						configuredTaskAwaiter = Task.WhenAny(new Task[] { task, timeoutTask }).ConfigureAwait(false).GetAwaiter();
						if (!configuredTaskAwaiter.IsCompleted)
						{
							await configuredTaskAwaiter;
							ConfiguredTaskAwaitable<Task>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
							configuredTaskAwaiter = configuredTaskAwaiter2;
							configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<Task>.ConfiguredTaskAwaiter);
						}
					}
					while (configuredTaskAwaiter.GetResult() != timeoutTask);
					throw new WebException("The operation has timed out.", WebExceptionStatus.Timeout);
					IL_0147:
					timeoutTask = null;
				}
				finally
				{
					timeoutCts.Cancel();
					timeoutCts.Dispose();
				}
				try
				{
					cancellationToken.ThrowIfCancellationRequested();
					if (this.read_eof || this.bufferedEntireContent)
					{
						return;
					}
					if (resending && !this.KeepAlive)
					{
						this.Close();
						return;
					}
					byte[] array = await this.ReadAllAsyncInner(cancellationToken).ConfigureAwait(false);
					BufferOffsetSize bufferOffsetSize = new BufferOffsetSize(array, 0, array.Length, false);
					this.innerStream = new BufferedReadStream(base.Operation, null, bufferOffsetSize);
					this.bufferedEntireContent = true;
					this.nextReadCalled = true;
					completion.TrySetCompleted();
				}
				catch (Exception ex)
				{
					completion.TrySetException(ex);
					throw;
				}
				finally
				{
					this.pendingRead = null;
				}
				base.Operation.Finish(true, null);
			}
		}

		// Token: 0x0600384D RID: 14413 RVA: 0x000C7117 File Offset: 0x000C5317
		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			return Task.FromException(new NotSupportedException("The stream does not support writing."));
		}

		// Token: 0x0600384E RID: 14414 RVA: 0x000C7128 File Offset: 0x000C5328
		protected override void Close_internal(ref bool disposed)
		{
			if (!this.closed && !this.nextReadCalled)
			{
				this.nextReadCalled = true;
				if (this.read_eof || this.bufferedEntireContent)
				{
					disposed = true;
					WebReadStream webReadStream = this.innerStream;
					if (webReadStream != null)
					{
						webReadStream.Dispose();
					}
					this.innerStream = null;
					base.Operation.Finish(true, null);
					return;
				}
				this.closed = true;
				disposed = true;
				base.Operation.Finish(false, null);
			}
		}

		// Token: 0x0600384F RID: 14415 RVA: 0x000C719C File Offset: 0x000C539C
		private WebException GetReadException(WebExceptionStatus status, Exception error, string where)
		{
			error = base.GetException(error);
			string.Format("Error getting response stream ({0}): {1}", where, status);
			if (error == null)
			{
				return new WebException(string.Format("Error getting response stream ({0}): {1}", where, status), status);
			}
			WebException ex = error as WebException;
			if (ex != null)
			{
				return ex;
			}
			if (base.Operation.Aborted || error is OperationCanceledException || error is ObjectDisposedException)
			{
				return HttpWebRequest.CreateRequestAbortedException();
			}
			return new WebException(string.Format("Error getting response stream ({0}): {1} {2}", where, status, error.Message), status, WebExceptionInternalStatus.RequestFatal, error);
		}

		// Token: 0x06003850 RID: 14416 RVA: 0x000C7230 File Offset: 0x000C5430
		internal async Task InitReadAsync(CancellationToken cancellationToken)
		{
			BufferOffsetSize buffer = new BufferOffsetSize(new byte[4096], false);
			ReadState state = ReadState.None;
			int position = 0;
			for (;;)
			{
				base.Operation.ThrowIfClosedOrDisposed(cancellationToken);
				int num = await this.RequestStream.InnerStream.ReadAsync(buffer.Buffer, buffer.Offset, buffer.Size, cancellationToken).ConfigureAwait(false);
				if (num == 0)
				{
					break;
				}
				if (num < 0)
				{
					goto Block_2;
				}
				buffer.Offset += num;
				buffer.Size -= num;
				if (state == ReadState.None)
				{
					try
					{
						int num2 = position;
						if (!this.GetResponse(buffer, ref position, ref state))
						{
							position = num2;
						}
					}
					catch (Exception ex)
					{
						throw this.GetReadException(WebExceptionStatus.ServerProtocolViolation, ex, "ReadDoneAsync4");
					}
				}
				if (state == ReadState.Aborted)
				{
					goto Block_4;
				}
				if (state == ReadState.Content)
				{
					goto Block_5;
				}
				int num3 = num * 2;
				if (num3 > buffer.Size)
				{
					byte[] array = new byte[buffer.Buffer.Length + num3];
					Buffer.BlockCopy(buffer.Buffer, 0, array, 0, buffer.Offset);
					buffer = new BufferOffsetSize(array, buffer.Offset, array.Length - buffer.Offset, false);
				}
				state = ReadState.None;
				position = 0;
			}
			throw this.GetReadException(WebExceptionStatus.ReceiveFailure, null, "ReadDoneAsync2");
			Block_2:
			throw this.GetReadException(WebExceptionStatus.ServerProtocolViolation, null, "ReadDoneAsync3");
			Block_4:
			throw this.GetReadException(WebExceptionStatus.RequestCanceled, null, "ReadDoneAsync5");
			Block_5:
			buffer.Size = buffer.Offset - position;
			buffer.Offset = position;
			try
			{
				this.Initialize(buffer);
			}
			catch (Exception ex2)
			{
				throw this.GetReadException(WebExceptionStatus.ReceiveFailure, ex2, "ReadDoneAsync6");
			}
		}

		// Token: 0x06003851 RID: 14417 RVA: 0x000C727C File Offset: 0x000C547C
		private bool GetResponse(BufferOffsetSize buffer, ref int pos, ref ReadState state)
		{
			string text = null;
			bool flag = false;
			bool flag2 = false;
			while (state != ReadState.Aborted)
			{
				if (state != ReadState.None)
				{
					goto IL_00F2;
				}
				if (!WebConnection.ReadLine(buffer.Buffer, ref pos, buffer.Offset, ref text))
				{
					return false;
				}
				if (text == null)
				{
					flag2 = true;
				}
				else
				{
					flag2 = false;
					state = ReadState.Status;
					string[] array = text.Split(' ', StringSplitOptions.None);
					if (array.Length < 2)
					{
						throw this.GetReadException(WebExceptionStatus.ServerProtocolViolation, null, "GetResponse");
					}
					if (string.Compare(array[0], "HTTP/1.1", true) == 0)
					{
						this.Version = HttpVersion.Version11;
						base.ServicePoint.SetVersion(HttpVersion.Version11);
					}
					else
					{
						this.Version = HttpVersion.Version10;
						base.ServicePoint.SetVersion(HttpVersion.Version10);
					}
					this.StatusCode = (HttpStatusCode)uint.Parse(array[1]);
					if (array.Length >= 3)
					{
						this.StatusDescription = string.Join(" ", array, 2, array.Length - 2);
					}
					else
					{
						this.StatusDescription = string.Empty;
					}
					if (pos >= buffer.Offset)
					{
						return true;
					}
					goto IL_00F2;
				}
				IL_027F:
				if (!flag2 && !flag)
				{
					throw this.GetReadException(WebExceptionStatus.ServerProtocolViolation, null, "GetResponse");
				}
				continue;
				IL_00F2:
				flag2 = false;
				if (state != ReadState.Status)
				{
					goto IL_027F;
				}
				state = ReadState.Headers;
				this.Headers = new WebHeaderCollection();
				List<string> list = new List<string>();
				bool flag3 = false;
				while (!flag3 && WebConnection.ReadLine(buffer.Buffer, ref pos, buffer.Offset, ref text))
				{
					if (text == null)
					{
						flag3 = true;
					}
					else if (text.Length > 0 && (text[0] == ' ' || text[0] == '\t'))
					{
						int num = list.Count - 1;
						if (num < 0)
						{
							break;
						}
						string text2 = list[num] + text;
						list[num] = text2;
					}
					else
					{
						list.Add(text);
					}
				}
				if (!flag3)
				{
					return false;
				}
				foreach (string text3 in list)
				{
					int num2 = text3.IndexOf(':');
					if (num2 == -1)
					{
						throw new ArgumentException("no colon found", "header");
					}
					string text4 = text3.Substring(0, num2);
					string text5 = text3.Substring(num2 + 1).Trim();
					if (WebHeaderCollection.AllowMultiValues(text4))
					{
						this.Headers.AddInternal(text4, text5);
					}
					else
					{
						this.Headers.SetInternal(text4, text5);
					}
				}
				if (this.StatusCode != HttpStatusCode.Continue)
				{
					state = ReadState.Content;
					return true;
				}
				base.ServicePoint.SendContinue = true;
				if (pos >= buffer.Offset)
				{
					return true;
				}
				if (base.Request.ExpectContinue)
				{
					base.Request.DoContinueDelegate((int)this.StatusCode, this.Headers);
					base.Request.ExpectContinue = false;
				}
				state = ReadState.None;
				flag = true;
				goto IL_027F;
			}
			throw this.GetReadException(WebExceptionStatus.RequestCanceled, null, "GetResponse");
		}

		// Token: 0x04002112 RID: 8466
		private WebReadStream innerStream;

		// Token: 0x04002113 RID: 8467
		private bool nextReadCalled;

		// Token: 0x04002114 RID: 8468
		private bool bufferedEntireContent;

		// Token: 0x04002115 RID: 8469
		private WebCompletionSource pendingRead;

		// Token: 0x04002116 RID: 8470
		private object locker = new object();

		// Token: 0x04002117 RID: 8471
		private int nestedRead;

		// Token: 0x04002118 RID: 8472
		private bool read_eof;

		// Token: 0x0400211F RID: 8479
		internal readonly string ME;
	}
}
