using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
	// Token: 0x020006CB RID: 1739
	internal abstract class WebReadStream : Stream
	{
		// Token: 0x17000BB7 RID: 2999
		// (get) Token: 0x060037F4 RID: 14324 RVA: 0x000C4FBA File Offset: 0x000C31BA
		public WebOperation Operation { get; }

		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x060037F5 RID: 14325 RVA: 0x000C4FC2 File Offset: 0x000C31C2
		protected Stream InnerStream { get; }

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x060037F6 RID: 14326 RVA: 0x00002F6A File Offset: 0x0000116A
		internal string ME
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060037F7 RID: 14327 RVA: 0x000C4FCA File Offset: 0x000C31CA
		public WebReadStream(WebOperation operation, Stream innerStream)
		{
			this.Operation = operation;
			this.InnerStream = innerStream;
		}

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x060037F8 RID: 14328 RVA: 0x000044FA File Offset: 0x000026FA
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x060037F9 RID: 14329 RVA: 0x000044FA File Offset: 0x000026FA
		// (set) Token: 0x060037FA RID: 14330 RVA: 0x000044FA File Offset: 0x000026FA
		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x060037FB RID: 14331 RVA: 0x00003062 File Offset: 0x00001262
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x060037FC RID: 14332 RVA: 0x0000390E File Offset: 0x00001B0E
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x060037FD RID: 14333 RVA: 0x00003062 File Offset: 0x00001262
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060037FE RID: 14334 RVA: 0x000044FA File Offset: 0x000026FA
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060037FF RID: 14335 RVA: 0x000044FA File Offset: 0x000026FA
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003800 RID: 14336 RVA: 0x000044FA File Offset: 0x000026FA
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003801 RID: 14337 RVA: 0x000044FA File Offset: 0x000026FA
		public override void Flush()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003802 RID: 14338 RVA: 0x000C4FE0 File Offset: 0x000C31E0
		protected Exception GetException(Exception e)
		{
			e = HttpWebRequest.FlattenException(e);
			if (e is WebException)
			{
				return e;
			}
			if (this.Operation.Aborted || e is OperationCanceledException || e is ObjectDisposedException)
			{
				return HttpWebRequest.CreateRequestAbortedException();
			}
			return e;
		}

		// Token: 0x06003803 RID: 14339 RVA: 0x000C5018 File Offset: 0x000C3218
		public override int Read(byte[] buffer, int offset, int size)
		{
			if (!this.CanRead)
			{
				throw new NotSupportedException("The stream does not support reading.");
			}
			this.Operation.ThrowIfClosedOrDisposed();
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			int num = buffer.Length;
			if (offset < 0 || num < offset)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || num - offset < size)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			int result;
			try
			{
				result = this.ReadAsync(buffer, offset, size, CancellationToken.None).Result;
			}
			catch (Exception ex)
			{
				throw this.GetException(ex);
			}
			return result;
		}

		// Token: 0x06003804 RID: 14340 RVA: 0x000C50B0 File Offset: 0x000C32B0
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback cb, object state)
		{
			if (!this.CanRead)
			{
				throw new NotSupportedException("The stream does not support reading.");
			}
			this.Operation.ThrowIfClosedOrDisposed();
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			int num = buffer.Length;
			if (offset < 0 || num < offset)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || num - offset < size)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			return TaskToApm.Begin(this.ReadAsync(buffer, offset, size, CancellationToken.None), cb, state);
		}

		// Token: 0x06003805 RID: 14341 RVA: 0x000C512C File Offset: 0x000C332C
		public override int EndRead(IAsyncResult r)
		{
			if (r == null)
			{
				throw new ArgumentNullException("r");
			}
			int num;
			try
			{
				num = TaskToApm.End<int>(r);
			}
			catch (Exception ex)
			{
				throw this.GetException(ex);
			}
			return num;
		}

		// Token: 0x06003806 RID: 14342 RVA: 0x000C516C File Offset: 0x000C336C
		public sealed override async Task<int> ReadAsync(byte[] buffer, int offset, int size, CancellationToken cancellationToken)
		{
			this.Operation.ThrowIfDisposed(cancellationToken);
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			int num = buffer.Length;
			if (offset < 0 || num < offset)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || num - offset < size)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			int num3;
			try
			{
				int num2 = await this.ProcessReadAsync(buffer, offset, size, cancellationToken).ConfigureAwait(false);
				if (num2 != 0)
				{
					num3 = num2;
				}
				else
				{
					await this.FinishReading(cancellationToken).ConfigureAwait(false);
					num3 = 0;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
			}
			return num3;
		}

		// Token: 0x06003807 RID: 14343
		protected abstract Task<int> ProcessReadAsync(byte[] buffer, int offset, int size, CancellationToken cancellationToken);

		// Token: 0x06003808 RID: 14344 RVA: 0x000C51D0 File Offset: 0x000C33D0
		internal virtual Task FinishReading(CancellationToken cancellationToken)
		{
			this.Operation.ThrowIfDisposed(cancellationToken);
			WebReadStream webReadStream = this.InnerStream as WebReadStream;
			if (webReadStream != null)
			{
				return webReadStream.FinishReading(cancellationToken);
			}
			return Task.CompletedTask;
		}

		// Token: 0x06003809 RID: 14345 RVA: 0x000C5205 File Offset: 0x000C3405
		protected override void Dispose(bool disposing)
		{
			if (disposing && !this.disposed)
			{
				this.disposed = true;
				if (this.InnerStream != null)
				{
					this.InnerStream.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x040020C4 RID: 8388
		private bool disposed;
	}
}
