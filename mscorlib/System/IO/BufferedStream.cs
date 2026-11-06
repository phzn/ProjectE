using System;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
	/// <summary>Adds a buffering layer to read and write operations on another stream. This class cannot be inherited.</summary>
	// Token: 0x02000B41 RID: 2881
	public sealed class BufferedStream : Stream
	{
		// Token: 0x0600680A RID: 26634 RVA: 0x001629B3 File Offset: 0x00160BB3
		internal SemaphoreSlim LazyEnsureAsyncActiveSemaphoreInitialized()
		{
			return LazyInitializer.EnsureInitialized<SemaphoreSlim>(ref this._asyncActiveSemaphore, () => new SemaphoreSlim(1, 1));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.BufferedStream" /> class with a default buffer size of 4096 bytes.</summary>
		/// <param name="stream">The current stream.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stream" /> is <see langword="null" />.</exception>
		// Token: 0x0600680B RID: 26635 RVA: 0x001629DF File Offset: 0x00160BDF
		public BufferedStream(Stream stream)
			: this(stream, 4096)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.BufferedStream" /> class with the specified buffer size.</summary>
		/// <param name="stream">The current stream.</param>
		/// <param name="bufferSize">The buffer size in bytes.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="stream" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="bufferSize" /> is negative.</exception>
		// Token: 0x0600680C RID: 26636 RVA: 0x001629F0 File Offset: 0x00160BF0
		public BufferedStream(Stream stream, int bufferSize)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (bufferSize <= 0)
			{
				throw new ArgumentOutOfRangeException("bufferSize", SR.Format("'{0}' must be greater than zero.", "bufferSize"));
			}
			this._stream = stream;
			this._bufferSize = bufferSize;
			if (!this._stream.CanRead && !this._stream.CanWrite)
			{
				throw new ObjectDisposedException(null, "Cannot access a closed Stream.");
			}
		}

		// Token: 0x0600680D RID: 26637 RVA: 0x00162A63 File Offset: 0x00160C63
		private void EnsureNotClosed()
		{
			if (this._stream == null)
			{
				throw new ObjectDisposedException(null, "Cannot access a closed Stream.");
			}
		}

		// Token: 0x0600680E RID: 26638 RVA: 0x00162A79 File Offset: 0x00160C79
		private void EnsureCanSeek()
		{
			if (!this._stream.CanSeek)
			{
				throw new NotSupportedException("Stream does not support seeking.");
			}
		}

		// Token: 0x0600680F RID: 26639 RVA: 0x00162A93 File Offset: 0x00160C93
		private void EnsureCanRead()
		{
			if (!this._stream.CanRead)
			{
				throw new NotSupportedException("Stream does not support reading.");
			}
		}

		// Token: 0x06006810 RID: 26640 RVA: 0x00162AAD File Offset: 0x00160CAD
		private void EnsureCanWrite()
		{
			if (!this._stream.CanWrite)
			{
				throw new NotSupportedException("Stream does not support writing.");
			}
		}

		// Token: 0x06006811 RID: 26641 RVA: 0x00162AC8 File Offset: 0x00160CC8
		private void EnsureShadowBufferAllocated()
		{
			if (this._buffer.Length != this._bufferSize || this._bufferSize >= 81920)
			{
				return;
			}
			byte[] array = new byte[Math.Min(this._bufferSize + this._bufferSize, 81920)];
			Buffer.BlockCopy(this._buffer, 0, array, 0, this._writePos);
			this._buffer = array;
		}

		// Token: 0x06006812 RID: 26642 RVA: 0x00162B2B File Offset: 0x00160D2B
		private void EnsureBufferAllocated()
		{
			if (this._buffer == null)
			{
				this._buffer = new byte[this._bufferSize];
			}
		}

		// Token: 0x17001204 RID: 4612
		// (get) Token: 0x06006813 RID: 26643 RVA: 0x00162B46 File Offset: 0x00160D46
		public Stream UnderlyingStream
		{
			get
			{
				return this._stream;
			}
		}

		// Token: 0x17001205 RID: 4613
		// (get) Token: 0x06006814 RID: 26644 RVA: 0x00162B4E File Offset: 0x00160D4E
		public int BufferSize
		{
			get
			{
				return this._bufferSize;
			}
		}

		/// <summary>Gets a value indicating whether the current stream supports reading.</summary>
		/// <returns>
		///   <see langword="true" /> if the stream supports reading; <see langword="false" /> if the stream is closed or was opened with write-only access.</returns>
		// Token: 0x17001206 RID: 4614
		// (get) Token: 0x06006815 RID: 26645 RVA: 0x00162B56 File Offset: 0x00160D56
		public override bool CanRead
		{
			get
			{
				return this._stream != null && this._stream.CanRead;
			}
		}

		/// <summary>Gets a value indicating whether the current stream supports writing.</summary>
		/// <returns>
		///   <see langword="true" /> if the stream supports writing; <see langword="false" /> if the stream is closed or was opened with read-only access.</returns>
		// Token: 0x17001207 RID: 4615
		// (get) Token: 0x06006816 RID: 26646 RVA: 0x00162B6D File Offset: 0x00160D6D
		public override bool CanWrite
		{
			get
			{
				return this._stream != null && this._stream.CanWrite;
			}
		}

		/// <summary>Gets a value indicating whether the current stream supports seeking.</summary>
		/// <returns>
		///   <see langword="true" /> if the stream supports seeking; <see langword="false" /> if the stream is closed or if the stream was constructed from an operating system handle such as a pipe or output to the console.</returns>
		// Token: 0x17001208 RID: 4616
		// (get) Token: 0x06006817 RID: 26647 RVA: 0x00162B84 File Offset: 0x00160D84
		public override bool CanSeek
		{
			get
			{
				return this._stream != null && this._stream.CanSeek;
			}
		}

		/// <summary>Gets the stream length in bytes.</summary>
		/// <returns>The stream length in bytes.</returns>
		/// <exception cref="T:System.IO.IOException">The underlying stream is <see langword="null" /> or closed.</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support seeking.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
		// Token: 0x17001209 RID: 4617
		// (get) Token: 0x06006818 RID: 26648 RVA: 0x00162B9B File Offset: 0x00160D9B
		public override long Length
		{
			get
			{
				this.EnsureNotClosed();
				if (this._writePos > 0)
				{
					this.FlushWrite();
				}
				return this._stream.Length;
			}
		}

		/// <summary>Gets the position within the current stream.</summary>
		/// <returns>The position within the current stream.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The value passed to <see cref="M:System.IO.BufferedStream.Seek(System.Int64,System.IO.SeekOrigin)" /> is negative.</exception>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs, such as the stream being closed.</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support seeking.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
		// Token: 0x1700120A RID: 4618
		// (get) Token: 0x06006819 RID: 26649 RVA: 0x00162BBD File Offset: 0x00160DBD
		// (set) Token: 0x0600681A RID: 26650 RVA: 0x00162BEC File Offset: 0x00160DEC
		public override long Position
		{
			get
			{
				this.EnsureNotClosed();
				this.EnsureCanSeek();
				return this._stream.Position + (long)(this._readPos - this._readLen + this._writePos);
			}
			set
			{
				if (value < 0L)
				{
					throw new ArgumentOutOfRangeException("value", "Non-negative number required.");
				}
				this.EnsureNotClosed();
				this.EnsureCanSeek();
				if (this._writePos > 0)
				{
					this.FlushWrite();
				}
				this._readPos = 0;
				this._readLen = 0;
				this._stream.Seek(value, SeekOrigin.Begin);
			}
		}

		// Token: 0x0600681B RID: 26651 RVA: 0x00162C48 File Offset: 0x00160E48
		public override async ValueTask DisposeAsync()
		{
			try
			{
				if (this._stream != null)
				{
					object obj = null;
					try
					{
						await base.FlushAsync().ConfigureAwait(false);
					}
					catch (object obj)
					{
					}
					await this._stream.DisposeAsync().ConfigureAwait(false);
					object obj2 = obj;
					if (obj2 != null)
					{
						Exception ex = obj2 as Exception;
						if (ex == null)
						{
							throw obj2;
						}
						ExceptionDispatchInfo.Capture(ex).Throw();
					}
					obj = null;
				}
			}
			finally
			{
				this._stream = null;
				this._buffer = null;
			}
		}

		// Token: 0x0600681C RID: 26652 RVA: 0x00162C8C File Offset: 0x00160E8C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this._stream != null)
				{
					try
					{
						this.Flush();
					}
					finally
					{
						this._stream.Dispose();
					}
				}
			}
			finally
			{
				this._stream = null;
				this._buffer = null;
				base.Dispose(disposing);
			}
		}

		/// <summary>Clears all buffers for this stream and causes any buffered data to be written to the underlying device.</summary>
		/// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
		/// <exception cref="T:System.IO.IOException">The data source or repository is not open.</exception>
		// Token: 0x0600681D RID: 26653 RVA: 0x00162CEC File Offset: 0x00160EEC
		public override void Flush()
		{
			this.EnsureNotClosed();
			if (this._writePos > 0)
			{
				this.FlushWrite();
				return;
			}
			if (this._readPos < this._readLen)
			{
				if (this._stream.CanSeek)
				{
					this.FlushRead();
				}
				if (this._stream.CanWrite)
				{
					this._stream.Flush();
				}
				return;
			}
			if (this._stream.CanWrite)
			{
				this._stream.Flush();
			}
			this._writePos = (this._readPos = (this._readLen = 0));
		}

		/// <summary>Asynchronously clears all buffers for this stream, causes any buffered data to be written to the underlying device, and monitors cancellation requests.</summary>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>A task that represents the asynchronous flush operation.</returns>
		/// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
		// Token: 0x0600681E RID: 26654 RVA: 0x00162D7A File Offset: 0x00160F7A
		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<int>(cancellationToken);
			}
			this.EnsureNotClosed();
			return this.FlushAsyncInternal(cancellationToken);
		}

		// Token: 0x0600681F RID: 26655 RVA: 0x00162D9C File Offset: 0x00160F9C
		private async Task FlushAsyncInternal(CancellationToken cancellationToken)
		{
			SemaphoreSlim sem = this.LazyEnsureAsyncActiveSemaphoreInitialized();
			await sem.WaitAsync().ConfigureAwait(false);
			try
			{
				if (this._writePos > 0)
				{
					await this.FlushWriteAsync(cancellationToken).ConfigureAwait(false);
				}
				else if (this._readPos < this._readLen)
				{
					if (this._stream.CanSeek)
					{
						this.FlushRead();
					}
					if (this._stream.CanWrite)
					{
						await this._stream.FlushAsync(cancellationToken).ConfigureAwait(false);
					}
				}
				else if (this._stream.CanWrite)
				{
					await this._stream.FlushAsync(cancellationToken).ConfigureAwait(false);
				}
			}
			finally
			{
				sem.Release();
			}
		}

		// Token: 0x06006820 RID: 26656 RVA: 0x00162DE7 File Offset: 0x00160FE7
		private void FlushRead()
		{
			if (this._readPos - this._readLen != 0)
			{
				this._stream.Seek((long)(this._readPos - this._readLen), SeekOrigin.Current);
			}
			this._readPos = 0;
			this._readLen = 0;
		}

		// Token: 0x06006821 RID: 26657 RVA: 0x00162E24 File Offset: 0x00161024
		private void ClearReadBufferBeforeWrite()
		{
			if (this._readPos == this._readLen)
			{
				this._readPos = (this._readLen = 0);
				return;
			}
			if (!this._stream.CanSeek)
			{
				throw new NotSupportedException("Cannot write to a BufferedStream while the read buffer is not empty if the underlying stream is not seekable. Ensure that the stream underlying this BufferedStream can seek or avoid interleaving read and write operations on this BufferedStream.");
			}
			this.FlushRead();
		}

		// Token: 0x06006822 RID: 26658 RVA: 0x00162E6E File Offset: 0x0016106E
		private void FlushWrite()
		{
			this._stream.Write(this._buffer, 0, this._writePos);
			this._writePos = 0;
			this._stream.Flush();
		}

		// Token: 0x06006823 RID: 26659 RVA: 0x00162E9C File Offset: 0x0016109C
		private async Task FlushWriteAsync(CancellationToken cancellationToken)
		{
			await this._stream.WriteAsync(new ReadOnlyMemory<byte>(this._buffer, 0, this._writePos), cancellationToken).ConfigureAwait(false);
			this._writePos = 0;
			await this._stream.FlushAsync(cancellationToken).ConfigureAwait(false);
		}

		// Token: 0x06006824 RID: 26660 RVA: 0x00162EE8 File Offset: 0x001610E8
		private int ReadFromBuffer(byte[] array, int offset, int count)
		{
			int num = this._readLen - this._readPos;
			if (num == 0)
			{
				return 0;
			}
			if (num > count)
			{
				num = count;
			}
			Buffer.BlockCopy(this._buffer, this._readPos, array, offset, num);
			this._readPos += num;
			return num;
		}

		// Token: 0x06006825 RID: 26661 RVA: 0x00162F34 File Offset: 0x00161134
		private int ReadFromBuffer(Span<byte> destination)
		{
			int num = Math.Min(this._readLen - this._readPos, destination.Length);
			if (num > 0)
			{
				new ReadOnlySpan<byte>(this._buffer, this._readPos, num).CopyTo(destination);
				this._readPos += num;
			}
			return num;
		}

		// Token: 0x06006826 RID: 26662 RVA: 0x00162F8C File Offset: 0x0016118C
		private int ReadFromBuffer(byte[] array, int offset, int count, out Exception error)
		{
			int num;
			try
			{
				error = null;
				num = this.ReadFromBuffer(array, offset, count);
			}
			catch (Exception ex)
			{
				error = ex;
				num = 0;
			}
			return num;
		}

		/// <summary>Copies bytes from the current buffered stream to an array.</summary>
		/// <param name="array">The buffer to which bytes are to be copied.</param>
		/// <param name="offset">The byte offset in the buffer at which to begin reading bytes.</param>
		/// <param name="count">The number of bytes to be read.</param>
		/// <returns>The total number of bytes read into <paramref name="array" />. This can be less than the number of bytes requested if that many bytes are not currently available, or 0 if the end of the stream has been reached before any data can be read.</returns>
		/// <exception cref="T:System.ArgumentException">Length of <paramref name="array" /> minus <paramref name="offset" /> is less than <paramref name="count" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.IO.IOException">The stream is not open or is <see langword="null" />.</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
		// Token: 0x06006827 RID: 26663 RVA: 0x00162FC4 File Offset: 0x001611C4
		public override int Read(byte[] array, int offset, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", "Buffer cannot be null.");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (array.Length - offset < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			this.EnsureNotClosed();
			this.EnsureCanRead();
			int num = this.ReadFromBuffer(array, offset, count);
			if (num == count)
			{
				return num;
			}
			int num2 = num;
			if (num > 0)
			{
				count -= num;
				offset += num;
			}
			this._readPos = (this._readLen = 0);
			if (this._writePos > 0)
			{
				this.FlushWrite();
			}
			if (count >= this._bufferSize)
			{
				return this._stream.Read(array, offset, count) + num2;
			}
			this.EnsureBufferAllocated();
			this._readLen = this._stream.Read(this._buffer, 0, this._bufferSize);
			num = this.ReadFromBuffer(array, offset, count);
			return num + num2;
		}

		// Token: 0x06006828 RID: 26664 RVA: 0x001630B8 File Offset: 0x001612B8
		public override int Read(Span<byte> destination)
		{
			this.EnsureNotClosed();
			this.EnsureCanRead();
			int num = this.ReadFromBuffer(destination);
			if (num == destination.Length)
			{
				return num;
			}
			if (num > 0)
			{
				destination = destination.Slice(num);
			}
			this._readPos = (this._readLen = 0);
			if (this._writePos > 0)
			{
				this.FlushWrite();
			}
			if (destination.Length >= this._bufferSize)
			{
				return this._stream.Read(destination) + num;
			}
			this.EnsureBufferAllocated();
			this._readLen = this._stream.Read(this._buffer, 0, this._bufferSize);
			return this.ReadFromBuffer(destination) + num;
		}

		// Token: 0x06006829 RID: 26665 RVA: 0x00163160 File Offset: 0x00161360
		private Task<int> LastSyncCompletedReadTask(int val)
		{
			Task<int> task = this._lastSyncCompletedReadTask;
			if (task != null && task.Result == val)
			{
				return task;
			}
			task = Task.FromResult<int>(val);
			this._lastSyncCompletedReadTask = task;
			return task;
		}

		/// <summary>Asynchronously reads a sequence of bytes from the current stream, advances the position within the stream by the number of bytes read, and monitors cancellation requests.</summary>
		/// <param name="buffer">The buffer to write the data into.</param>
		/// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin writing data from the stream.</param>
		/// <param name="count">The maximum number of bytes to read.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>A task that represents the asynchronous read operation. The value of the <paramref name="TResult" /> parameter contains the total number of bytes read into the buffer. The result value can be less than the number of bytes requested if the number of bytes currently available is less than the requested number, or it can be 0 (zero) if the end of the stream has been reached.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset" /> and <paramref name="count" /> is larger than the buffer length.</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The stream is currently in use by a previous read operation.</exception>
		// Token: 0x0600682A RID: 26666 RVA: 0x00163194 File Offset: 0x00161394
		public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", "Buffer cannot be null.");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<int>(cancellationToken);
			}
			this.EnsureNotClosed();
			this.EnsureCanRead();
			int num = 0;
			SemaphoreSlim semaphoreSlim = this.LazyEnsureAsyncActiveSemaphoreInitialized();
			Task task = semaphoreSlim.WaitAsync();
			if (task.IsCompletedSuccessfully)
			{
				bool flag = true;
				try
				{
					Exception ex;
					num = this.ReadFromBuffer(buffer, offset, count, out ex);
					flag = num == count || ex != null;
					if (flag)
					{
						return (ex == null) ? this.LastSyncCompletedReadTask(num) : Task.FromException<int>(ex);
					}
				}
				finally
				{
					if (flag)
					{
						semaphoreSlim.Release();
					}
				}
			}
			return this.ReadFromUnderlyingStreamAsync(new Memory<byte>(buffer, offset + num, count - num), cancellationToken, num, task).AsTask();
		}

		// Token: 0x0600682B RID: 26667 RVA: 0x0016329C File Offset: 0x0016149C
		public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return new ValueTask<int>(Task.FromCanceled<int>(cancellationToken));
			}
			this.EnsureNotClosed();
			this.EnsureCanRead();
			int num = 0;
			SemaphoreSlim semaphoreSlim = this.LazyEnsureAsyncActiveSemaphoreInitialized();
			Task task = semaphoreSlim.WaitAsync();
			if (task.IsCompletedSuccessfully)
			{
				bool flag = true;
				try
				{
					num = this.ReadFromBuffer(buffer.Span);
					flag = num == buffer.Length;
					if (flag)
					{
						return new ValueTask<int>(num);
					}
				}
				finally
				{
					if (flag)
					{
						semaphoreSlim.Release();
					}
				}
			}
			return this.ReadFromUnderlyingStreamAsync(buffer.Slice(num), cancellationToken, num, task);
		}

		// Token: 0x0600682C RID: 26668 RVA: 0x0016333C File Offset: 0x0016153C
		private async ValueTask<int> ReadFromUnderlyingStreamAsync(Memory<byte> buffer, CancellationToken cancellationToken, int bytesAlreadySatisfied, Task semaphoreLockTask)
		{
			await semaphoreLockTask.ConfigureAwait(false);
			int num2;
			try
			{
				int num = this.ReadFromBuffer(buffer.Span);
				if (num == buffer.Length)
				{
					num2 = bytesAlreadySatisfied + num;
				}
				else
				{
					if (num > 0)
					{
						buffer = buffer.Slice(num);
						bytesAlreadySatisfied += num;
					}
					int num3 = 0;
					this._readLen = num3;
					this._readPos = num3;
					if (this._writePos > 0)
					{
						await this.FlushWriteAsync(cancellationToken).ConfigureAwait(false);
					}
					if (buffer.Length >= this._bufferSize)
					{
						int num4 = bytesAlreadySatisfied;
						num2 = num4 + await this._stream.ReadAsync(buffer, cancellationToken).ConfigureAwait(false);
					}
					else
					{
						this.EnsureBufferAllocated();
						this._readLen = await this._stream.ReadAsync(new Memory<byte>(this._buffer, 0, this._bufferSize), cancellationToken).ConfigureAwait(false);
						num2 = bytesAlreadySatisfied + this.ReadFromBuffer(buffer.Span);
					}
				}
			}
			finally
			{
				this.LazyEnsureAsyncActiveSemaphoreInitialized().Release();
			}
			return num2;
		}

		/// <summary>Begins an asynchronous read operation. (Consider using <see cref="M:System.IO.BufferedStream.ReadAsync(System.Byte[],System.Int32,System.Int32,System.Threading.CancellationToken)" /> instead.)</summary>
		/// <param name="buffer">The buffer to read the data into.</param>
		/// <param name="offset">The byte offset in <paramref name="buffer" /> at which to begin writing data read from the stream.</param>
		/// <param name="count">The maximum number of bytes to read.</param>
		/// <param name="callback">An optional asynchronous callback, to be called when the read is complete.</param>
		/// <param name="state">A user-provided object that distinguishes this particular asynchronous read request from other requests.</param>
		/// <returns>An object that represents the asynchronous read, which could still be pending.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.IO.IOException">Attempted an asynchronous read past the end of the stream.</exception>
		/// <exception cref="T:System.ArgumentException">The buffer length minus <paramref name="offset" /> is less than <paramref name="count" />.</exception>
		/// <exception cref="T:System.NotSupportedException">The current stream does not support the read operation.</exception>
		// Token: 0x0600682D RID: 26669 RVA: 0x000A438E File Offset: 0x000A258E
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return TaskToApm.Begin(this.ReadAsync(buffer, offset, count, CancellationToken.None), callback, state);
		}

		/// <summary>Waits for the pending asynchronous read operation to complete. (Consider using <see cref="M:System.IO.BufferedStream.ReadAsync(System.Byte[],System.Int32,System.Int32,System.Threading.CancellationToken)" /> instead.)</summary>
		/// <param name="asyncResult">The reference to the pending asynchronous request to wait for.</param>
		/// <returns>The number of bytes read from the stream, between 0 (zero) and the number of bytes you requested. Streams only return 0 only at the end of the stream, otherwise, they should block until at least 1 byte is available.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">This <see cref="T:System.IAsyncResult" /> object was not created by calling <see cref="M:System.IO.BufferedStream.BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /> on this class.</exception>
		// Token: 0x0600682E RID: 26670 RVA: 0x000A43A7 File Offset: 0x000A25A7
		public override int EndRead(IAsyncResult asyncResult)
		{
			return TaskToApm.End<int>(asyncResult);
		}

		/// <summary>Reads a byte from the underlying stream and returns the byte cast to an <see langword="int" />, or returns -1 if reading from the end of the stream.</summary>
		/// <returns>The byte cast to an <see langword="int" />, or -1 if reading from the end of the stream.</returns>
		/// <exception cref="T:System.IO.IOException">An I/O error occurs, such as the stream being closed.</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
		// Token: 0x0600682F RID: 26671 RVA: 0x001633A0 File Offset: 0x001615A0
		public override int ReadByte()
		{
			if (this._readPos == this._readLen)
			{
				return this.ReadByteSlow();
			}
			byte[] buffer = this._buffer;
			int readPos = this._readPos;
			this._readPos = readPos + 1;
			return buffer[readPos];
		}

		// Token: 0x06006830 RID: 26672 RVA: 0x001633DC File Offset: 0x001615DC
		private int ReadByteSlow()
		{
			this.EnsureNotClosed();
			this.EnsureCanRead();
			if (this._writePos > 0)
			{
				this.FlushWrite();
			}
			this.EnsureBufferAllocated();
			this._readLen = this._stream.Read(this._buffer, 0, this._bufferSize);
			this._readPos = 0;
			if (this._readLen == 0)
			{
				return -1;
			}
			byte[] buffer = this._buffer;
			int readPos = this._readPos;
			this._readPos = readPos + 1;
			return buffer[readPos];
		}

		// Token: 0x06006831 RID: 26673 RVA: 0x00163454 File Offset: 0x00161654
		private void WriteToBuffer(byte[] array, ref int offset, ref int count)
		{
			int num = Math.Min(this._bufferSize - this._writePos, count);
			if (num <= 0)
			{
				return;
			}
			this.EnsureBufferAllocated();
			Buffer.BlockCopy(array, offset, this._buffer, this._writePos, num);
			this._writePos += num;
			count -= num;
			offset += num;
		}

		// Token: 0x06006832 RID: 26674 RVA: 0x001634B0 File Offset: 0x001616B0
		private int WriteToBuffer(ReadOnlySpan<byte> buffer)
		{
			int num = Math.Min(this._bufferSize - this._writePos, buffer.Length);
			if (num > 0)
			{
				this.EnsureBufferAllocated();
				buffer.Slice(0, num).CopyTo(new Span<byte>(this._buffer, this._writePos, num));
				this._writePos += num;
			}
			return num;
		}

		// Token: 0x06006833 RID: 26675 RVA: 0x00163514 File Offset: 0x00161714
		private void WriteToBuffer(byte[] array, ref int offset, ref int count, out Exception error)
		{
			try
			{
				error = null;
				this.WriteToBuffer(array, ref offset, ref count);
			}
			catch (Exception ex)
			{
				error = ex;
			}
		}

		/// <summary>Copies bytes to the buffered stream and advances the current position within the buffered stream by the number of bytes written.</summary>
		/// <param name="array">The byte array from which to copy <paramref name="count" /> bytes to the current buffered stream.</param>
		/// <param name="offset">The offset in the buffer at which to begin copying bytes to the current buffered stream.</param>
		/// <param name="count">The number of bytes to be written to the current buffered stream.</param>
		/// <exception cref="T:System.ArgumentException">Length of <paramref name="array" /> minus <paramref name="offset" /> is less than <paramref name="count" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="array" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.IO.IOException">The stream is closed or <see langword="null" />.</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
		// Token: 0x06006834 RID: 26676 RVA: 0x00163548 File Offset: 0x00161748
		public override void Write(byte[] array, int offset, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array", "Buffer cannot be null.");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (array.Length - offset < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			this.EnsureNotClosed();
			this.EnsureCanWrite();
			if (this._writePos == 0)
			{
				this.ClearReadBufferBeforeWrite();
			}
			int num = checked(this._writePos + count);
			if (checked(num + count >= this._bufferSize + this._bufferSize))
			{
				if (this._writePos > 0)
				{
					if (num <= this._bufferSize + this._bufferSize && num <= 81920)
					{
						this.EnsureShadowBufferAllocated();
						Buffer.BlockCopy(array, offset, this._buffer, this._writePos, count);
						this._stream.Write(this._buffer, 0, num);
						this._writePos = 0;
						return;
					}
					this._stream.Write(this._buffer, 0, this._writePos);
					this._writePos = 0;
				}
				this._stream.Write(array, offset, count);
				return;
			}
			this.WriteToBuffer(array, ref offset, ref count);
			if (this._writePos < this._bufferSize)
			{
				return;
			}
			this._stream.Write(this._buffer, 0, this._writePos);
			this._writePos = 0;
			this.WriteToBuffer(array, ref offset, ref count);
		}

		// Token: 0x06006835 RID: 26677 RVA: 0x001636A4 File Offset: 0x001618A4
		public override void Write(ReadOnlySpan<byte> buffer)
		{
			this.EnsureNotClosed();
			this.EnsureCanWrite();
			if (this._writePos == 0)
			{
				this.ClearReadBufferBeforeWrite();
			}
			int num = checked(this._writePos + buffer.Length);
			if (checked(num + buffer.Length >= this._bufferSize + this._bufferSize))
			{
				if (this._writePos > 0)
				{
					if (num <= this._bufferSize + this._bufferSize && num <= 81920)
					{
						this.EnsureShadowBufferAllocated();
						buffer.CopyTo(new Span<byte>(this._buffer, this._writePos, buffer.Length));
						this._stream.Write(this._buffer, 0, num);
						this._writePos = 0;
						return;
					}
					this._stream.Write(this._buffer, 0, this._writePos);
					this._writePos = 0;
				}
				this._stream.Write(buffer);
				return;
			}
			int num2 = this.WriteToBuffer(buffer);
			if (this._writePos < this._bufferSize)
			{
				return;
			}
			buffer = buffer.Slice(num2);
			this._stream.Write(this._buffer, 0, this._writePos);
			this._writePos = 0;
			num2 = this.WriteToBuffer(buffer);
		}

		/// <summary>Asynchronously writes a sequence of bytes to the current stream, advances the current position within this stream by the number of bytes written, and monitors cancellation requests.</summary>
		/// <param name="buffer">The buffer to write data from.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> from which to begin copying bytes to the stream.</param>
		/// <param name="count">The maximum number of bytes to write.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>A task that represents the asynchronous write operation.</returns>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset" /> and <paramref name="count" /> is larger than the buffer length.</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The stream has been disposed.</exception>
		/// <exception cref="T:System.InvalidOperationException">The stream is currently in use by a previous write operation.</exception>
		// Token: 0x06006836 RID: 26678 RVA: 0x001637CC File Offset: 0x001619CC
		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", "Buffer cannot be null.");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", "Non-negative number required.");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Non-negative number required.");
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException("Offset and length were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.");
			}
			return this.WriteAsync(new ReadOnlyMemory<byte>(buffer, offset, count), cancellationToken).AsTask();
		}

		// Token: 0x06006837 RID: 26679 RVA: 0x00163840 File Offset: 0x00161A40
		public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return new ValueTask(Task.FromCanceled<int>(cancellationToken));
			}
			this.EnsureNotClosed();
			this.EnsureCanWrite();
			SemaphoreSlim semaphoreSlim = this.LazyEnsureAsyncActiveSemaphoreInitialized();
			Task task = semaphoreSlim.WaitAsync();
			if (task.IsCompletedSuccessfully)
			{
				bool flag = true;
				try
				{
					if (this._writePos == 0)
					{
						this.ClearReadBufferBeforeWrite();
					}
					flag = buffer.Length < this._bufferSize - this._writePos;
					if (flag)
					{
						this.WriteToBuffer(buffer.Span);
						return default(ValueTask);
					}
				}
				finally
				{
					if (flag)
					{
						semaphoreSlim.Release();
					}
				}
			}
			return new ValueTask(this.WriteToUnderlyingStreamAsync(buffer, cancellationToken, task));
		}

		// Token: 0x06006838 RID: 26680 RVA: 0x001638F8 File Offset: 0x00161AF8
		private async Task WriteToUnderlyingStreamAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken, Task semaphoreLockTask)
		{
			await semaphoreLockTask.ConfigureAwait(false);
			try
			{
				if (this._writePos == 0)
				{
					this.ClearReadBufferBeforeWrite();
				}
				int num = checked(this._writePos + buffer.Length);
				if (checked(num + buffer.Length < this._bufferSize + this._bufferSize))
				{
					buffer = buffer.Slice(this.WriteToBuffer(buffer.Span));
					if (this._writePos >= this._bufferSize)
					{
						await this._stream.WriteAsync(new ReadOnlyMemory<byte>(this._buffer, 0, this._writePos), cancellationToken).ConfigureAwait(false);
						this._writePos = 0;
						this.WriteToBuffer(buffer.Span);
					}
				}
				else
				{
					if (this._writePos > 0)
					{
						if (num <= this._bufferSize + this._bufferSize && num <= 81920)
						{
							this.EnsureShadowBufferAllocated();
							buffer.Span.CopyTo(new Span<byte>(this._buffer, this._writePos, buffer.Length));
							await this._stream.WriteAsync(new ReadOnlyMemory<byte>(this._buffer, 0, num), cancellationToken).ConfigureAwait(false);
							this._writePos = 0;
							return;
						}
						await this._stream.WriteAsync(new ReadOnlyMemory<byte>(this._buffer, 0, this._writePos), cancellationToken).ConfigureAwait(false);
						this._writePos = 0;
					}
					await this._stream.WriteAsync(buffer, cancellationToken).ConfigureAwait(false);
				}
			}
			finally
			{
				this.LazyEnsureAsyncActiveSemaphoreInitialized().Release();
			}
		}

		/// <summary>Begins an asynchronous write operation. (Consider using <see cref="M:System.IO.BufferedStream.WriteAsync(System.Byte[],System.Int32,System.Int32,System.Threading.CancellationToken)" /> instead.)</summary>
		/// <param name="buffer">The buffer containing data to write to the current stream.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
		/// <param name="count">The maximum number of bytes to write.</param>
		/// <param name="callback">The method to be called when the asynchronous write operation is completed.</param>
		/// <param name="state">A user-provided object that distinguishes this particular asynchronous write request from other requests.</param>
		/// <returns>An object that references the asynchronous write which could still be pending.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="buffer" /> length minus <paramref name="offset" /> is less than <paramref name="count" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="buffer" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="offset" /> or <paramref name="count" /> is negative.</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
		// Token: 0x06006839 RID: 26681 RVA: 0x000A45BB File Offset: 0x000A27BB
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			return TaskToApm.Begin(this.WriteAsync(buffer, offset, count, CancellationToken.None), callback, state);
		}

		/// <summary>Ends an asynchronous write operation and blocks until the I/O operation is complete. (Consider using <see cref="M:System.IO.BufferedStream.WriteAsync(System.Byte[],System.Int32,System.Int32,System.Threading.CancellationToken)" /> instead.)</summary>
		/// <param name="asyncResult">The pending asynchronous request.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="asyncResult" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">This <see cref="T:System.IAsyncResult" /> object was not created by calling <see cref="M:System.IO.BufferedStream.BeginWrite(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)" /> on this class.</exception>
		// Token: 0x0600683A RID: 26682 RVA: 0x000A45D4 File Offset: 0x000A27D4
		public override void EndWrite(IAsyncResult asyncResult)
		{
			TaskToApm.End(asyncResult);
		}

		/// <summary>Writes a byte to the current position in the buffered stream.</summary>
		/// <param name="value">A byte to write to the stream.</param>
		/// <exception cref="T:System.NotSupportedException">The stream does not support writing.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="value" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
		// Token: 0x0600683B RID: 26683 RVA: 0x00163954 File Offset: 0x00161B54
		public override void WriteByte(byte value)
		{
			this.EnsureNotClosed();
			if (this._writePos == 0)
			{
				this.EnsureCanWrite();
				this.ClearReadBufferBeforeWrite();
				this.EnsureBufferAllocated();
			}
			if (this._writePos >= this._bufferSize - 1)
			{
				this.FlushWrite();
			}
			byte[] buffer = this._buffer;
			int writePos = this._writePos;
			this._writePos = writePos + 1;
			buffer[writePos] = value;
		}

		/// <summary>Sets the position within the current buffered stream.</summary>
		/// <param name="offset">A byte offset relative to <paramref name="origin" />.</param>
		/// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point from which to obtain the new position.</param>
		/// <returns>The new position within the current buffered stream.</returns>
		/// <exception cref="T:System.IO.IOException">The stream is not open or is <see langword="null" />.</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support seeking.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
		// Token: 0x0600683C RID: 26684 RVA: 0x001639B0 File Offset: 0x00161BB0
		public override long Seek(long offset, SeekOrigin origin)
		{
			this.EnsureNotClosed();
			this.EnsureCanSeek();
			if (this._writePos > 0)
			{
				this.FlushWrite();
				return this._stream.Seek(offset, origin);
			}
			if (this._readLen - this._readPos > 0 && origin == SeekOrigin.Current)
			{
				offset -= (long)(this._readLen - this._readPos);
			}
			long position = this.Position;
			long num = this._stream.Seek(offset, origin);
			this._readPos = (int)(num - (position - (long)this._readPos));
			if (0 <= this._readPos && this._readPos < this._readLen)
			{
				this._stream.Seek((long)(this._readLen - this._readPos), SeekOrigin.Current);
			}
			else
			{
				this._readPos = (this._readLen = 0);
			}
			return num;
		}

		/// <summary>Sets the length of the buffered stream.</summary>
		/// <param name="value">An integer indicating the desired length of the current buffered stream in bytes.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="value" /> is negative.</exception>
		/// <exception cref="T:System.IO.IOException">The stream is not open or is <see langword="null" />.</exception>
		/// <exception cref="T:System.NotSupportedException">The stream does not support both writing and seeking.</exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
		// Token: 0x0600683D RID: 26685 RVA: 0x00163A78 File Offset: 0x00161C78
		public override void SetLength(long value)
		{
			if (value < 0L)
			{
				throw new ArgumentOutOfRangeException("value", "Non-negative number required.");
			}
			this.EnsureNotClosed();
			this.EnsureCanSeek();
			this.EnsureCanWrite();
			this.Flush();
			this._stream.SetLength(value);
		}

		// Token: 0x0600683E RID: 26686 RVA: 0x00163AB4 File Offset: 0x00161CB4
		public override void CopyTo(Stream destination, int bufferSize)
		{
			StreamHelpers.ValidateCopyToArgs(this, destination, bufferSize);
			int num = this._readLen - this._readPos;
			if (num > 0)
			{
				destination.Write(this._buffer, this._readPos, num);
				this._readPos = (this._readLen = 0);
			}
			else if (this._writePos > 0)
			{
				this.FlushWrite();
			}
			this._stream.CopyTo(destination, bufferSize);
		}

		// Token: 0x0600683F RID: 26687 RVA: 0x00163B1C File Offset: 0x00161D1C
		public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			StreamHelpers.ValidateCopyToArgs(this, destination, bufferSize);
			if (!cancellationToken.IsCancellationRequested)
			{
				return this.CopyToAsyncCore(destination, bufferSize, cancellationToken);
			}
			return Task.FromCanceled<int>(cancellationToken);
		}

		// Token: 0x06006840 RID: 26688 RVA: 0x00163B40 File Offset: 0x00161D40
		private async Task CopyToAsyncCore(Stream destination, int bufferSize, CancellationToken cancellationToken)
		{
			await this.LazyEnsureAsyncActiveSemaphoreInitialized().WaitAsync().ConfigureAwait(false);
			try
			{
				int num = this._readLen - this._readPos;
				if (num > 0)
				{
					await destination.WriteAsync(new ReadOnlyMemory<byte>(this._buffer, this._readPos, num), cancellationToken).ConfigureAwait(false);
					int num2 = 0;
					this._readLen = num2;
					this._readPos = num2;
				}
				else if (this._writePos > 0)
				{
					await this.FlushWriteAsync(cancellationToken).ConfigureAwait(false);
				}
				await this._stream.CopyToAsync(destination, bufferSize, cancellationToken).ConfigureAwait(false);
			}
			finally
			{
				this._asyncActiveSemaphore.Release();
			}
		}

		// Token: 0x04003C87 RID: 15495
		private const int MaxShadowBufferSize = 81920;

		// Token: 0x04003C88 RID: 15496
		private const int DefaultBufferSize = 4096;

		// Token: 0x04003C89 RID: 15497
		private Stream _stream;

		// Token: 0x04003C8A RID: 15498
		private byte[] _buffer;

		// Token: 0x04003C8B RID: 15499
		private readonly int _bufferSize;

		// Token: 0x04003C8C RID: 15500
		private int _readPos;

		// Token: 0x04003C8D RID: 15501
		private int _readLen;

		// Token: 0x04003C8E RID: 15502
		private int _writePos;

		// Token: 0x04003C8F RID: 15503
		private Task<int> _lastSyncCompletedReadTask;

		// Token: 0x04003C90 RID: 15504
		private SemaphoreSlim _asyncActiveSemaphore;
	}
}
